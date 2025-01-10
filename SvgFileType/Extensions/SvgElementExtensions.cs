// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Xml.Linq;
using Svg;

namespace SvgFileTypePlugin.Extensions;

internal static class SvgElementExtensions
{
    private static readonly MethodInfo? ElementNameGetter = GetGetMethod("ElementName");
    private static readonly MethodInfo? AttributesGetter = GetGetMethod("Attributes");

    public static string GetName(this SvgElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.GetType().GetCustomAttribute<SvgElementAttribute>()?.ElementName 
            ?? ElementNameGetter?.Invoke(element, null) as string 
            ?? element.GetType().Name;
    }

    public static SvgAttributeCollection? GetAttributes(this SvgElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return (SvgAttributeCollection?)AttributesGetter?.Invoke(element, null);
    }

    public static void Cleanup(this SvgElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        for (int i = element.Children.Count - 1; i >= 0; i--)
        {
            SvgElement child = element.Children[i];
            if (child.Visibility != "visible" && child is not SvgTextBase)
                element.Children.RemoveAt(i);
            else
                child.Cleanup();
        }
    }

    private static MethodInfo? GetGetMethod(string propertyName)
    {
        return typeof(SvgElement).GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance)?.GetGetMethod(true);
    }
}
