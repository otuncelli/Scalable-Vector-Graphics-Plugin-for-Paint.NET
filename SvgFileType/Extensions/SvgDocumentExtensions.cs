// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Drawing;
using System.Threading;
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

        SvgAspectRatio origAR = svg.AspectRatio;
        SizeF origSize = svg.GetDimensions();
        SizeF rasterSize = origSize;
        SvgViewBox origViewBox = svg.ViewBox;
        if (origViewBox.IsEmpty() && !origSize.IsEmpty)
            svg.ViewBox = new SvgViewBox(0, 0, origSize.Width, origSize.Height);
        svg.RasterizeDimensions(ref rasterSize, config.RasterWidth, config.RasterHeight);
        svg.Width = new SvgUnit(SvgUnitType.User, rasterSize.Width);
        svg.Height = new SvgUnit(SvgUnitType.User, rasterSize.Height);
        svg.AspectRatio = new SvgAspectRatio(config.PreserveAspectRatio
            ? SvgPreserveAspectRatio.xMinYMin
            : SvgPreserveAspectRatio.none);
        return new DisposableAction(() =>
        {
            // Restore the original values back
            svg.AspectRatio = origAR;
            svg.Width = origSize.Width;
            svg.Height = origSize.Height;
            svg.ViewBox = origViewBox;
        });
    }

    private sealed class DisposableAction : IDisposable
    {
        private Action? _dispose;
        private readonly Lock @lock = new();

        public DisposableAction(Action dispose)
        {
            ArgumentNullException.ThrowIfNull(dispose);

            _dispose = dispose;
        }

        public void Dispose()
        {
            if (_dispose is null)
            {
                return;
            }

            @lock.Enter();
            try
            {
                _dispose.Invoke();
            }
            finally
            {
                _dispose = null;
                @lock.Exit();
            }
        }
    }
}
