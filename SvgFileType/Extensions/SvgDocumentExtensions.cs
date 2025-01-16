// Copyright 2025 Osman Tunçelli. All rights reserved.
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
    public static SvgDocument RemoveInvisibleAndNonTextElements(this SvgDocument svg)
    {
        ArgumentNullException.ThrowIfNull(svg);

        SvgFragment clonedFragment = (SvgFragment)svg.Clone();
        clonedFragment.RemoveInvisibleAndNonTextElements();
        SvgDocument clonedDocument = new SvgDocument();
        clonedDocument.Children.Add(clonedFragment);
        return clonedDocument;
    }

    public static IDisposable UseSetRasterDimensions(this SvgDocument svg, SvgImportConfig config)
    {
        ArgumentNullException.ThrowIfNull(svg);
        ArgumentNullException.ThrowIfNull(config);

        int width = config.RasterWidth;
        int height = config.RasterHeight;
        SvgAspectRatio originalAspectRatio = svg.AspectRatio;
        SizeF originalSize = svg.GetDimensions();
        SvgViewBox originalViewbox = svg.ViewBox;
        SizeF rasterSize = originalSize;
        svg.RasterizeDimensions(ref rasterSize, width, height);
        svg.Width = rasterSize.Width;
        svg.Height = rasterSize.Height;
        svg.ViewBox = new SvgViewBox(0, 0, originalSize.Width, originalSize.Height);
        SvgPreserveAspectRatio aspectRatio = config.PreserveAspectRatio 
            ? SvgPreserveAspectRatio.xMinYMin
            : SvgPreserveAspectRatio.none;
        svg.AspectRatio = new SvgAspectRatio(aspectRatio);
        return Utils.DisposableFromAction(() =>
        {
            // Restore the original values back
            svg.AspectRatio = originalAspectRatio;
            svg.Width = originalSize.Width;
            svg.Height = originalSize.Height;
            svg.ViewBox = originalViewbox;
        });
    }

    public static string GetXML2(this SvgDocument svg)
    {
        ArgumentNullException.ThrowIfNull(svg);

        // This issue has been resolved for resvg but,
        // apparently, Direct2D renderer is also affected.
        // https://github.com/RazrFalcon/resvg/issues/235
        return svg.GetXML().Replace("&quot;", string.Empty);
    }

    public static Stream AsStream(this SvgDocument svg, bool removeQuotes = false)
    {
        ArgumentNullException.ThrowIfNull(svg);

        string xml = removeQuotes ? svg.GetXML2() : svg.GetXML();
        byte[] bytes = Encoding.UTF8.GetBytes(xml);
        return new MemoryStream(bytes);
    }
}
