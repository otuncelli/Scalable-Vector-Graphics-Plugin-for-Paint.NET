// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Generic;
using Svg;
using Svg.Transforms;

namespace SvgFileTypePlugin.Extensions;

internal static class SvgUseExtensions
{
    public static SvgElement? GetCopyOfReferencedElement(this SvgUse useElement)
    {
        ArgumentNullException.ThrowIfNull(useElement);
        SvgDocument document = useElement.OwnerDocument 
            ?? throw new ArgumentException("Use element does not have an owner document.", nameof(useElement));

        List<SvgTransform> transforms = [];
        SvgElement? referencedElement = useElement;
        while (referencedElement is SvgUse parentUse and { ReferencedElement: Uri uri })
        {
            string href = uri.ToString().TrimStart('#');
            if (href.Length < 1)
            {
                return null;
            }
            if (parentUse.Transforms is { Count: > 0 })
            {
                transforms.AddRange(parentUse.Transforms);
            }
            referencedElement = document.GetElementById(href);
        }
        if (referencedElement is null)
        {
            return null;
        }
        SvgElement copiedElement = referencedElement.DeepCopy();
        useElement.CopyOverridedAttributes(copiedElement);
        copiedElement.Transforms ??= [];
        copiedElement.Transforms.AddRange(transforms);
        return copiedElement;
    }

    private static void CopyOverridedAttributes(this SvgUse useElement, SvgElement targetElement)
    {
        SvgAttributeCollection targetAttributes = targetElement.GetAttributes();
        SvgAttributeCollection sourceAttributes = useElement.GetAttributes();

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
            {
                targetAttributes[key] = attribute.Value;
            }
        }
    }
}
