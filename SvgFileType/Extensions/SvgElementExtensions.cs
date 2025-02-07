// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
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

        using InvariantUtf8StringWriter writer = new InvariantUtf8StringWriter();
        XmlWriterSettings xmlWriterSettings = new() { Encoding = Encoding.UTF8 };
        using (XmlWriter xmlWriter = new CustomXmlWriter(XmlWriter.Create(writer, xmlWriterSettings)))
        {
            svg.Write(xmlWriter);
            xmlWriter.Flush();
        }
        writer.Flush();
        return writer.ToString();
    }

    public static void WriteXML_QuotedFuncIRIHack(this SvgElement svg, Stream output)
    {
        ArgumentNullException.ThrowIfNull(svg);

        using InvariantUtf8StreamWriter writer = new InvariantUtf8StreamWriter(output);
        XmlWriterSettings xmlWriterSettings = new() { Encoding = Encoding.UTF8 };
        using (XmlWriter xmlWriter = new CustomXmlWriter(XmlWriter.Create(writer, xmlWriterSettings)))
        {
            svg.Write(xmlWriter);
            xmlWriter.Flush();
        }
        writer.Flush();
    }

    private static T CreateGetterDelegate<T>(string propertyName) where T : Delegate
    {
        MethodInfo getter = typeof(SvgElement)
            ?.GetProperty(propertyName, BindingFlags.NonPublic | BindingFlags.Instance)
            ?.GetGetMethod(true)
            ?? throw new MissingMemberException(nameof(SvgElement), propertyName);
        return getter.CreateDelegate<T>();
    }

    private sealed class InvariantUtf8StreamWriter(Stream stream) : StreamWriter(stream, Encoding.UTF8, leaveOpen: true)
    {
        private readonly Stream stream = stream;

        public override Encoding Encoding => Encoding.UTF8;
        public override IFormatProvider FormatProvider => CultureInfo.InvariantCulture;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
                stream.Position = 0;
        }
    }

    private sealed class InvariantUtf8StringWriter() : StringWriter(CultureInfo.InvariantCulture)
    {
        public override Encoding Encoding => Encoding.UTF8;
    }

    private sealed class CustomXmlWriter(XmlWriter writer) : XmlWriter
    {
        private readonly XmlWriter writer = writer;
        public override WriteState WriteState => writer.WriteState;
        public override void Flush() => writer.Flush();
        public override string? LookupPrefix(string ns) => writer.LookupPrefix(ns);
        public override void WriteBase64(byte[] buffer, int index, int count) => writer.WriteBase64(buffer, index, count);
        public override void WriteCData(string? text) => writer.WriteCData(text);
        public override void WriteCharEntity(char ch) => writer.WriteCharEntity(ch);
        public override void WriteChars(char[] buffer, int index, int count) => writer.WriteChars(buffer, index, count);
        public override void WriteComment(string? text) => writer.WriteComment(text);
        public override void WriteDocType(string name, string? pubid, string? sysid, string? subset) => writer.WriteDocType(name, pubid, sysid, subset);
        public override void WriteEndAttribute() => writer.WriteEndAttribute();
        public override void WriteEndDocument() => writer.WriteEndDocument();
        public override void WriteEndElement() => writer.WriteEndElement();
        public override void WriteEntityRef(string name) => writer.WriteEntityRef(name);
        public override void WriteFullEndElement() => writer.WriteFullEndElement();
        public override void WriteProcessingInstruction(string name, string? text) => writer.WriteProcessingInstruction(name, text);
        public override void WriteRaw(char[] buffer, int index, int count) => writer.WriteRaw(buffer, index, count);
        public override void WriteRaw(string data) => writer.WriteRaw(data);
        public override void WriteStartAttribute(string? prefix, string localName, string? ns) => writer.WriteStartAttribute(prefix, localName, ns);
        public override void WriteStartDocument() => writer.WriteStartDocument();
        public override void WriteStartDocument(bool standalone) => writer.WriteStartDocument(standalone);
        public override void WriteStartElement(string? prefix, string localName, string? ns) => writer.WriteStartElement(prefix, localName, ns);
        public override void WriteString(string? text)
        {
            writer.WriteString(text?.Replace("\"", string.Empty).Replace("\'", string.Empty));
        }
        public override void WriteSurrogateCharEntity(char lowChar, char highChar) => writer.WriteSurrogateCharEntity(lowChar, highChar);
        public override void WriteWhitespace(string? ws) => writer.WriteWhitespace(ws);
    }
}
