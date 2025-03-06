// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime;
using System.Threading;
using PaintDotNet;
using resvg.net;
using Svg;
using SvgFileTypePlugin.Extensions;

namespace SvgFileTypePlugin.Import;

internal sealed class ResvgSvgRenderer() : SvgRenderer2("resvg")
{
    protected override Document GetFlatDocument(string svgdata, SvgImportConfig config, CancellationToken ctoken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(svgdata);
        ArgumentNullException.ThrowIfNull(config);

        ctoken.ThrowIfCancellationRequested();
        (int width, int height) = (config.RasterWidth, config.RasterHeight);
        using BenchmarkScope _ = new BenchmarkScope();
        using MemoryFailPoint mfp = GetMemoryFailPoint(width, height, 1);
        ResetProgress(1);
        Surface surface = new Surface(width, height);
        try
        {
            using Resvg resvg = Resvg.FromData(svgdata, loadSystemFonts: true);
            ResvgTransform transform = CalculateTransform(resvg.Size, config);
            resvg.Render(surface.Scan0.Pointer, transform, width, height, PixelOpFlags.RgbaToBgra | PixelOpFlags.UnPremultiplyAlpha);
        }
        catch (Exception)
        {
            surface.Dispose();
            throw;
        }

        Document document = surface.CreateSingleLayerDocument(takeOwnership: true);
        document.SetDpi(config.Ppi);
        IncrementProgress();
        return document;
    }

    protected override Document GetLayeredDocument(string svgdata, SvgImportConfig config, CancellationToken ctoken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(svgdata);
        ArgumentNullException.ThrowIfNull(config);

        using BenchmarkScope _ = new BenchmarkScope();
        SvgDocument svg = SvgDocument.FromSvg<SvgDocument>(svgdata);
        using IDisposable _1 = svg.UseSetRasterDimensions(config);

        List<SvgVisualElement> elements = GetSvgVisualElements(svg, config, ctoken);
        ctoken.ThrowIfCancellationRequested();

        using MemoryFailPoint _2 = GetMemoryFailPoint(config.RasterWidth, config.RasterHeight, elements.Count);

        ResetProgress(elements.Count);
        List<BitmapLayer> layers = [];
        using ResvgOptions options = new ResvgOptions();
        options.LoadSystemFonts();

        using Surface surface = new Surface(config.RasterWidth, config.RasterHeight, SurfaceCreationFlags.DoNotZeroFillHint);
        // Render all visual elements that are passed here.
        foreach (SvgVisualElement element in elements)
        {
            ctoken.ThrowIfCancellationRequested();

            BitmapLayer? layer = null;

            if (element is GroupBoundary boundaryNode)
            {
                // Render empty group boundary and continue
                layer = new BitmapLayer(config.RasterWidth, config.RasterHeight);
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
                    layers.ForEach(layer => layer.Dispose());
                    throw;
                }
                IncrementProgress();
                layers.Add(layer);
                continue;
            }

            if (!element.PreRender(elements, config.ImportHiddenElements, ctoken))
            {
                IncrementProgress();
                continue;
            }

            surface.Clear();
            RenderSvgDocument(element, surface, options, config);
            if (surface.IsEmpty())
            {
                IncrementProgress();
                continue;
            }
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
                layer?.Dispose();
                layers.ForEach(layer => layer.Dispose());
                throw;
            }

            IncrementProgress();
            layers.Add(layer);
        }
        return GetDocument(layers, config);
    }

    private void RenderSvgDocument(SvgElement element, Surface surface, ResvgOptions options, SvgImportConfig config)
    {
        if (element is SvgUse use)
        {
            RenderSvgUseElement(use, e => RenderSvgDocument(e, surface, options, config));
        }
        else
        {
            SvgDocument clone = element.OwnerDocument.RemoveInvisibleAndNonTextElements();
            (int width, int height) = (surface.Width, surface.Height);
            using Resvg resvg = Resvg.FromData(clone.GetXML_QuotedFuncIRIHack(), options);
            resvg.Render(surface.Scan0.Pointer, width, height, PixelOpFlags.RgbaToBgra | PixelOpFlags.UnPremultiplyAlpha);
        }
    }

    private static ResvgTransform CalculateTransform(SizeF svgsize, SvgImportConfig config)
    {
        float ratioX = config.RasterWidth / svgsize.Width;
        float ratioY = config.PreserveAspectRatio ? ratioX : config.RasterHeight / svgsize.Height;
        return new ResvgTransform()
        {
            M11 = ratioX,
            M22 = ratioY
        };
    }
}
