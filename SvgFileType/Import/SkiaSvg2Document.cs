// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

#if SKIA

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using PaintDotNet;
using SkiaSharp;
using Svg;
using Svg.Skia;
using SvgFileTypePlugin.Extensions;

namespace SvgFileTypePlugin.Import;

internal class SkiaSvg2Document : DefaultSvgConverter
{
    internal static new readonly SkiaSvg2Document Instance = new();

    public override string Name => "Skia";

    public override Document GetFlatDocument(SvgDocument svg, SvgImportConfig config, CancellationToken cancellationToken = default)
    {
        using Stream stream = svg.AsStream();
        return GetFlatDocument(stream, config, cancellationToken);
    }

    public override Document GetFlatDocument(Stream stream, SvgImportConfig config, CancellationToken cancellationToken = default)
    {
        Logger.WriteLine($"Using {Name}...");
        cancellationToken.ThrowIfCancellationRequested();
        int width = config.Width;
        int height = config.Height;
        int ppi = config.Ppi;
        SKSizeI size = new SKSizeI(width, height);
        Surface surface = new Surface(width, height, SurfaceCreationFlags.DoNotZeroFillHint);
        try
        {
            using SKSvg svg = new SKSvg();
            if (svg.Load(stream) is not SKPicture picture)
            {
                return null;
            }
            float scaleX = width / picture.CullRect.Width;
            float scaleY = height / picture.CullRect.Height;
            SKMatrix scale = SKMatrix.CreateScale(scaleX, scaleY);
            SKImageInfo imginfo = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul);
            using SKImage image = SKImage.FromPicture(picture, size, scale);
            using SKPixmap pixmap = new SKPixmap(imginfo, surface.Scan0.Pointer);
            if (!image.ReadPixels(pixmap, 0, 0))
            {
                return null;
            }
        }
        catch
        {
            surface.Dispose();
            throw;
        }
        Document document = surface.CreateDocument(takeOwnership: true);
        document.SetDpi(ppi);
        return document;
    }

    public override Document GetLayeredDocument(IReadOnlyCollection<SvgVisualElement> elements, SvgImportConfig config, Action<int> progress = null, CancellationToken cancellationToken = default)
    {
        Logger.WriteLine($"Using {Name}...");
        cancellationToken.ThrowIfCancellationRequested();
        IEnumerable<BitmapLayer> GetLayers()
        {
            int layersProcessed = 0;
            SKImageInfo imginfo = new SKImageInfo(config.Width, config.Height);
            using Surface surface = new Surface(config.Width, config.Height, SurfaceCreationFlags.DoNotZeroFillHint);
            // Render all visual elements that are passed here.
            foreach (SvgVisualElement element in elements)
            {
                cancellationToken.ThrowIfCancellationRequested();

                BitmapLayer layer = null;

                if (element is GroupBoundary boundaryNode)
                {
                    // Render empty group boundary and continue
                    layer = new BitmapLayer(config.Width, config.Height);
                    try
                    {
                        layer.Name = boundaryNode.Name;
                        // Store related group opacity and visibility.
                        layer.Opacity = ToByteOpacity(boundaryNode.Opacity);
                        layer.Visible = boundaryNode.Visible;
                    }
                    catch (Exception)
                    {
                        layer.Dispose();
                        throw;
                    }
                    IncrementProgress(progress, ref layersProcessed);
                    yield return layer;
                    continue;
                }

                if (!element.PreRender(elements, config.ImportHiddenElements, cancellationToken))
                {
                    IncrementProgress(progress, ref layersProcessed);
                    continue;
                }

                surface.Clear();
                RenderSvgDocument(element, surface);

                layer = new BitmapLayer(surface, takeOwnership: false);
                try
                {
                    layer.Name = GetLayerTitle(element);
                    if (config.RespectElementOpacity)
                    {
                        layer.Opacity = ToByteOpacity(element.Opacity);
                    }

                    if (config.ImportHiddenElements && !element.IsOriginallyVisible())
                    {
                        layer.Visible = false;
                    }
                }
                catch (Exception)
                {
                    surface.Dispose();
                    layer?.Dispose();
                    throw;
                }

                IncrementProgress(progress, ref layersProcessed);
                yield return layer;
            }
        }
        return GetDocument(GetLayers(), config);
    }

    protected override void RenderSvgDocument(SvgElement element, object context)
    {
        Surface surface = (Surface)context;
        if (element is SvgUse use)
        {
            RenderSvgUseElement(use, e => RenderSvgDocument(e, context));
        }
        else
        {
            SvgDocument clone = element.OwnerDocument.Cleanup();
            using Stream stream = clone.AsStream();
            using SKSvg sksvg = new SKSvg();
            if (sksvg.Load(stream) is not SKPicture picture)
            {
                return;
            }
            int width = surface.Width;
            int height = surface.Height;
            SKSizeI size = new SKSizeI(width, height);
            float scaleX = width / picture.CullRect.Width;
            float scaleY = height / picture.CullRect.Height;
            SKMatrix scale = SKMatrix.CreateScale(scaleX, scaleY);
            SKImageInfo skimageinfo = new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Unpremul);
            using SKImage skimg = SKImage.FromPicture(picture, size, scale);
            using SKPixmap pixmap = new SKPixmap(skimageinfo, surface.Scan0.Pointer);
            if (!skimg.ReadPixels(pixmap, 0, 0, SKImageCachingHint.Disallow))
            {
                return;
            }
        }
    }
}

#endif
