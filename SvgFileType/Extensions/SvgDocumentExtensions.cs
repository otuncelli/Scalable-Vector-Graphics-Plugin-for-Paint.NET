// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Drawing;
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
}
