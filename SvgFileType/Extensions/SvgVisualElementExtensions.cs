// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Svg;

namespace SvgFileTypePlugin.Extensions;

internal static class SvgVisualElementExtensions
{
    #region OriginalVisibilityAttribute

    private const string OriginalVisibilityAttribute = "original_visibility";

    public static bool IsOriginallyVisible(this SvgVisualElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.CustomAttributes.TryGetValue(OriginalVisibilityAttribute, out string? arg) 
            && string.Equals(arg?.Trim(), "visible", StringComparison.OrdinalIgnoreCase);
    }

    public static void StoreOriginalVisibility(this SvgVisualElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        SvgCustomAttributeCollection col = element.CustomAttributes;
        if (col.ContainsKey(OriginalVisibilityAttribute))
            col[OriginalVisibilityAttribute] = element.Visibility;
        else
            col.Add(OriginalVisibilityAttribute, element.Visibility);
    }

    #endregion

    #region GroupNameAttribute 

    private const string GroupNameAttribute = "group_name";

    public static string? GetGroupName(this SvgVisualElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.CustomAttributes.TryGetValue(GroupNameAttribute, out string? groupName) ? groupName : null;
    }

    public static void SetGroupName(this SvgVisualElement element, string? groupName)
    {
        ArgumentNullException.ThrowIfNull(element);

        SvgCustomAttributeCollection col = element.CustomAttributes;
        if (col.ContainsKey(GroupNameAttribute))
        {
            if (string.IsNullOrEmpty(groupName))
                col.Remove(GroupNameAttribute);
            else
                col[GroupNameAttribute] = groupName;
        }
        else if (!string.IsNullOrEmpty(groupName))
            col.Add(GroupNameAttribute, groupName);
    }

    #endregion

    public static IEnumerable<SvgVisualElement> NotOfType<TElement>(this IEnumerable<SvgVisualElement> source)
        where TElement : SvgVisualElement
    {
        ArgumentNullException.ThrowIfNull(source);

        return source.Where(element => element is not TElement);
    }

    public static bool IsDisplayable(this SvgVisualElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return !element.Display.Trim().Equals("none", StringComparison.OrdinalIgnoreCase);
    }

    public static bool PreRender(this SvgVisualElement element, IReadOnlyCollection<SvgVisualElement> elements, bool hiddenElements, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(elements);

        // Turn off visibility of all other elements
        foreach (SvgVisualElement sibling in elements.Where(e => e != element))
        {
            cancellationToken.ThrowIfCancellationRequested();
            sibling.Visibility = "hidden";
        }

        element.Visibility = "visible";

        // Turn on visibility from node to parent
        for (SvgElement parent = element.Parent; parent != null; parent = parent.Parent)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (parent is not SvgVisualElement visual)
                continue;

            // Check, maybe parent element was initially hidden
            // Skip hidden layers.
            if (!hiddenElements && !visual.IsOriginallyVisible())
                return false;

            visual.Visibility = "visible";
        }
        return true;
    }
}
