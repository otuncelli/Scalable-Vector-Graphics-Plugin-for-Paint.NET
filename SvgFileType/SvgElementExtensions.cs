// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using Svg;
using System.Reflection;

namespace SvgFileTypePlugin
{
    internal static class SvgElementExtensions
    {
        private static readonly MethodInfo ElementNameGetter = typeof(SvgElement).GetProperty("ElementName", BindingFlags.NonPublic | BindingFlags.Instance)?.GetGetMethod(true);
        private static readonly MethodInfo AttributesGetter = typeof(SvgElement).GetProperty("Attributes", BindingFlags.NonPublic | BindingFlags.Instance)?.GetGetMethod(true);

        public static string GetName(this SvgElement element)
        {
            return element.GetType().GetCustomAttribute<SvgElementAttribute>()?.ElementName ??
                ElementNameGetter.Invoke(element, null) as string ??
                element.GetType().Name;
        }

        public static SvgAttributeCollection GetAttributes(this SvgElement element)
        {
            return (SvgAttributeCollection)AttributesGetter.Invoke(element, null);
        }
    }
}
