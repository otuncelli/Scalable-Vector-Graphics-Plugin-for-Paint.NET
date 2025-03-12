// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime;
using System.Threading;
using PaintDotNet;
using Svg;
using SvgFileTypePlugin.Extensions;

namespace SvgFileTypePlugin.Import;

internal sealed class GdipSvgRenderer() : SvgRenderer2(name: "GDI+")
{
    protected override Document GetFlatDocument(string svgdata, SvgImportConfig config, CancellationToken ctoken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(svgdata);
        ArgumentNullException.ThrowIfNull(config);

        ctoken.ThrowIfCancellationRequested();

        using BenchmarkScope _ = new BenchmarkScope();
        SvgDocument svg = SvgDocument.FromSvg<SvgDocument>(svgdata);
        using IDisposable _1 = svg.UseSetRasterDimensions(config);

        List<SvgVisualElement> elements = GetSvgVisualElements(svg, config, ctoken);
        using MemoryFailPoint _2 = GetMemoryFailPoint(config.RasterWidth, config.RasterHeight, 2);

        ResetProgress(elements.Count);
        using Bitmap bmp = new Bitmap(config.RasterWidth, config.RasterHeight);
        using (Graphics g = Graphics.FromImage(bmp))
        {
            // Render all visual elements that are passed here.
            foreach (SvgVisualElement element in elements)
            {
                ctoken.ThrowIfCancellationRequested();
                if (!element.PreRender(elements, hiddenElements: false, ctoken))
                {
                    IncrementProgress();
                    continue;
                }
                RenderSvgDocument(element, g, config);
                IncrementProgress();
            }
        }
        Document document = Document.FromImage(bmp);
        document.SetDpu(config.Ppi);
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
        using Bitmap bmp = new Bitmap(config.RasterWidth, config.RasterHeight);
        using Graphics g = Graphics.FromImage(bmp);
        // Render all visual elements that are passed here.
        foreach (SvgVisualElement element in elements)
        {
            ctoken.ThrowIfCancellationRequested();

            BitmapLayer? layer;
            if (element is GroupBoundary boundaryNode)
            {
                // Render empty group boundary and continue
                layer = boundaryNode.ToEmptyLayer(config.RasterWidth, config.RasterHeight);
                layers.Add(layer);
                IncrementProgress();
                continue;
            }

            if (!element.PreRender(elements, config.ImportHiddenElements, ctoken))
            {
                IncrementProgress();
                continue;
            }

            g.Clear(default);
            RenderSvgDocument(element, g, config);
            Surface surface = Surface.CopyFromBitmap(bmp, detectDishonestAlpha: false);
            if (surface.IsEmpty())
            {
                surface.Dispose();
                IncrementProgress();
                continue;
            }

            layer = new BitmapLayer(surface, takeOwnership: true);
            try
            {
                layer.Name = GetLayerTitle(element);
                if (config.RespectElementOpacity)
                    layer.Opacity = ToByteOpacity(element.Opacity);

                if (config.ImportHiddenElements && !element.IsOriginallyVisible())
                    layer.Visible = false;
            }
            catch (Exception)
            {
                surface.Dispose();
                layer?.Dispose();
                layers.ForEach(layer => layer.Dispose());
                throw;
            }
            IncrementProgress();
            layers.Add(layer);
        }
        return GetDocument(layers, config);
    }

    private void RenderSvgDocument(SvgElement element, Graphics graphics, SvgImportConfig config)
    {
        if (element is SvgUse use)
        {
            RenderSvgUseElement(use, e => RenderSvgDocument(e, graphics, config));
        }
        else
        {
            SvgDocument clone = element.OwnerDocument.RemoveInvisibleAndNonTextElements();
            clone.Draw(graphics);
        }
    }
}
