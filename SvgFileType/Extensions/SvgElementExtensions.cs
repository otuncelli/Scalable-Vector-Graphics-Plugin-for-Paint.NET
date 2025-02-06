// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.IO;
using System.Reflection;
using System.Text;
using Svg;

namespace SvgFileTypePlugin.Extensions;

internal static class SvgElementExtensions
{
    private delegate SvgAttributeCollection AttributesGetterDelegate(SvgElement element);
    private delegate string? ElementNameGetterDelegate(SvgElement element);

    private static readonly AttributesGetterDelegate GetElementAttributes = CreateGetterDelegate<AttributesGetterDelegate>("Attributes");
    private static readonly ElementNameGetterDelegate GetElementName = CreateGetterDelegate<ElementNameGetterDelegate>("ElementName");

    public static string GetName(this SvgElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return element.GetType().GetCustomAttribute<SvgElementAttribute>()?.ElementName
            ?? GetElementName(element)
            ?? element.GetType().Name;
    }

    public static SvgAttributeCollection GetAttributes(this SvgElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        return GetElementAttributes(element);
    }

    public static void RemoveInvisibleAndNonTextElements(this SvgElement element)
    {
        ArgumentNullException.ThrowIfNull(element);

        for (int i = element.Children.Count - 1; i >= 0; i--)
        {
            SvgElement child = element.Children[i];
            if (child.Visibility != "visible" && child is not SvgTextBase)
                element.Children.RemoveAt(i);
            else
                child.RemoveInvisibleAndNonTextElements();
        }
    }

    public static string GetXML_QuotedFuncIRIHack(this SvgElement svg)
    {
        ArgumentNullException.ThrowIfNull(svg);

        return svg.GetXML().Replace("&quot;", string.Empty);
    }

    public static Stream GetXMLAsStream(this SvgElement svg)
    {
        ArgumentNullException.ThrowIfNull(svg);

        string xml = svg.GetXML_QuotedFuncIRIHack();
        byte[] bytes = Encoding.UTF8.GetBytes(xml);
        return new MemoryStream(bytes);
    }

    private static T CreateGetterDelegate<T>(string propertyName) where T : Delegate
    {
        MethodInfo getter = typeof(SvgElement)
            ?.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetGetMethod(true)
            ?? throw new MissingMemberException(nameof(SvgElement), propertyName);
        return getter.CreateDelegate<T>();
    }
}
