// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Generic;
using Svg;
using Svg.Transforms;

namespace SvgFileTypePlugin.Extensions;

internal static class SvgUseExtensions
{
    public static SvgElement? GetCopyOfReferencedElement(this SvgUse use)
    {
        ArgumentNullException.ThrowIfNull(use);
        if (use.OwnerDocument is null)
            throw new ArgumentException("Use element does not have owner document.", nameof(use));

        List<SvgTransform> transforms = [];
        SvgElement? referenced = use;
        while (referenced is SvgUse parentUse && parentUse?.ReferencedElement is Uri uri)
        {
            string href = uri.ToString().TrimStart('#');
            if (href.Length < 1)
                return null;
            if (parentUse.Transforms is not null && parentUse.Transforms.Count > 0)
                transforms.AddRange(parentUse.Transforms);
            referenced = use.OwnerDocument.GetElementById(href);
        }
        if (referenced is null)
            return null;
        SvgElement copied = referenced.DeepCopy();
        use.CopyOverridedAttributes(copied);
        copied.Transforms ??= [];
        copied.Transforms.AddRange(transforms);
        return copied;
    }

    private static void CopyOverridedAttributes(this SvgUse use, SvgElement target)
    {
        SvgAttributeCollection targetAttributes = target.GetAttributes();
        SvgAttributeCollection sourceAttributes = use.GetAttributes();

        foreach (KeyValuePair<string, object> attribute in sourceAttributes)
        {
            string key = attribute.Key.ToLowerInvariant();

            // Most attributes on use do not override those already on the element
            // referenced by use. (This differs from how CSS style attributes override
            // those set 'earlier' in the cascade). Only the attributes x, y, width,
            // height and href on the use element will override those set on the
            // referenced element.However, any other attributes not set on the referenced
            // element will be applied to the use element.
            if (key is "x" or "y" or "width" or "height" or "href" or "xlink:href")
                targetAttributes[key] = attribute.Value;
        }
    }
}
