// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using Svg;
using System.Reflection;

namespace SvgFileTypePlugin.Import
{
    internal static class SvgElementExtensions
    {
        private static readonly MethodInfo ElementNameGetter = GetGetMethod("ElementName");
        private static readonly MethodInfo AttributesGetter = GetGetMethod("Attributes");

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

        private static MethodInfo GetGetMethod(string propertyName)
        {
            return typeof(SvgElement).GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance)?.GetGetMethod(true);
        }
    }
}
