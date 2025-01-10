// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Generic;
using Svg;
using Svg.Transforms;

namespace SvgFileTypePlugin.Extensions;

internal static class SvgUseExtensions
{
    // Most attributes on use do not override those already on the element
    // referenced by use. (This differs from how CSS style attributes override
    // those set 'earlier' in the cascade). Only the attributes x, y, width,
    // height and href on the use element will override those set on the
    // referenced element.However, any other attributes not set on the referenced
    // element will be applied to the use element.
    private static readonly HashSet<string> UseOverrides = new(StringComparer.OrdinalIgnoreCase) 
    { "x", "y", "width", "height", "href", "xlink:href" };

    public static SvgElement? CopyReferencedRootElement(this SvgUse use)
    {
        ArgumentNullException.ThrowIfNull(use);

        SvgElement? refElem = GetReferencedElement(use);
        if (refElem == null) 
            return null;
        List<SvgTransform> transforms = [];
        while (refElem is SvgUse use2)
        {
            AddTransforms(refElem.Transforms);
            refElem = GetReferencedElement(use2);
        }
        if (refElem == null) 
        { 
            transforms.Clear();
            return null; 
        }
        AddTransforms(use.Transforms);
        SvgElement copy = refElem.DeepCopy();
        copy.Transforms ??= [];
        copy.Transforms.AddRange(transforms);
        CopyAttributes(use, copy);
        return copy;

        void AddTransforms(SvgTransformCollection col)
        {
            if (col != null && col.Count > 0)
                transforms.AddRange(col);
        }

        SvgElement? GetReferencedElement(SvgUse use)
        {
            if (use.OwnerDocument == null) { throw new InvalidOperationException("use element doesn't have an owner document."); }
            if (use == null)
                return null;
            Uri uri = use.ReferencedElement;
            if (uri == null)
                return null;
            string id = uri.ToString().Trim();
            if (id.Length < 2 || id == null) 
                return null;
            id = id[1..];
            return use.OwnerDocument.GetElementById(id);
        }

        void CopyAttributes(SvgUse fromUse, SvgElement toElem)
        {
            SvgAttributeCollection? refAttrs = toElem.GetAttributes();
            SvgAttributeCollection? fromUseAttrs = fromUse.GetAttributes();

            if (fromUseAttrs is not null && refAttrs is not null)
            {
                foreach (KeyValuePair<string, object> useAttr in fromUseAttrs)
                {
                    string key = useAttr.Key;
                    if (UseOverrides.Contains(key))
                    {
                        refAttrs[key] = useAttr.Value;
                    }
                }
            }
        }
    }
}
