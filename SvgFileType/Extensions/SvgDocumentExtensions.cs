// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Drawing;
using System.IO;
using System.Text;
using Svg;
using SvgFileTypePlugin.Import;

namespace SvgFileTypePlugin.Extensions;

internal static class SvgDocumentExtensions
{
    public static Stream AsStream(this SvgDocument svg)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(svg.GetXML2());
        return new MemoryStream(bytes);
    }

    public static string GetXML2(this SvgDocument svg)
    {
        // A workaround until this gets resolved
        // https://github.com/RazrFalcon/resvg/issues/235
        return svg.GetXML().Replace("&quot;", String.Empty);
    }

    public static SvgDocument Cleanup(this SvgDocument svg)
    {
        SvgFragment frag = (SvgFragment)svg.Clone();
        frag.Cleanup();
        SvgDocument clone = new SvgDocument();
        clone.Children.Add(frag);
        return clone;
    }

    public static IDisposable UseSetRasterDimensions(this SvgDocument svg, SvgImportConfig config)
    {
        int width = config.Width;
        int height = config.Height;
        bool preserveAR = config.PreserveAspectRatio;
        SvgAspectRatio origAR = svg.AspectRatio;
        SizeF origSize = svg.GetDimensions();
        SizeF tmp = origSize;
        svg.RasterizeDimensions(ref tmp, width, height);
        svg.Width = tmp.Width;
        svg.Height = tmp.Height;
        SvgPreserveAspectRatio ar = preserveAR ? SvgPreserveAspectRatio.xMinYMin : SvgPreserveAspectRatio.none;
        svg.AspectRatio = new SvgAspectRatio(ar);
        return Disposable.FromAction(() =>
        {
            SizeF tmp = origSize;
            svg.AspectRatio = origAR;
            svg.RasterizeDimensions(ref tmp, 0, 0);
            svg.Width = tmp.Width;
            svg.Height = tmp.Height;
        });
    }
}
