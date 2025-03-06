// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime;
using System.Threading;
using PaintDotNet;
using PaintDotNet.Direct2D1;
using PaintDotNet.DirectWrite;
using PaintDotNet.Imaging;
using PaintDotNet.Rendering;
using Svg;
using SvgFileTypePlugin.Extensions;
using FontStyle = PaintDotNet.DirectWrite.FontStyle;
using IDeviceContext = PaintDotNet.Direct2D1.IDeviceContext;
using SR = SvgFileTypePlugin.Localization.StringResources;

namespace SvgFileTypePlugin.Import;

internal sealed class Direct2DSvgRenderer() : SvgRenderer2(name: "Direct2D")
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
        Surface surface = new Surface(width, height, SurfaceCreationFlags.DoNotZeroFillHint);
        try
        {
            SvgDocument svgdoc = SvgDocument.FromSvg<SvgDocument>(svgdata);
            IDirect2DFactory d2d = Services.Get<IDirect2DFactory>(); // Don't dispose this! It's singleton.
            using IBitmap<ColorBgra32> sbitmap = surface.CreateSharedBitmap();
            using IBitmap<ColorPbgra32> pbitmap = sbitmap.CreatePremultipliedAdapter(PremultipliedAdapterOptions.UnPremultiplyOnDispose);
            using IDeviceContext dc = d2d.CreateBitmapDeviceContext(pbitmap);
            using MemoryStream stream = new MemoryStream();
            svgdoc.WriteXML_QuotedFuncIRIHack(stream);
            using ISvgDocument svg = dc.CreateSvgDocument(stream, viewportSize: new SizeFloat(width, height));
            using DrawingScope _1 = dc.UseBeginDraw();
            dc.Transform = CalculateTransform(svgsize: new SizeF(svgdoc.Width, svgdoc.Height), config);
            dc.Clear();
            dc.DrawSvgDocument(svg);
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

        IDirect2DFactory d2d = Services.Get<IDirect2DFactory>(); // Don't dispose this! It's singleton.
        using Surface surface = new Surface(config.RasterWidth, config.RasterHeight);
        using IBitmap<ColorBgra32> sharedBitmap = surface.CreateSharedBitmap();
        using IBitmap<ColorPbgra32> premultiplied = sharedBitmap.CreatePremultipliedAdapter(PremultipliedAdapterOptions.None);
        using IDeviceContext dc = d2d.CreateBitmapDeviceContext(premultiplied);
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

            RenderSvgDocument(element, dc);
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

    public override Document GetNoPathDocument()
    {
        IDirectWriteFactory dw = Services.Get<IDirectWriteFactory>();
        IDirect2DFactory d2d = Services.Get<IDirect2DFactory>();
        using ITextFormat textFormat = dw.CreateTextFormat("Arial", null, FontWeight.Bold, FontStyle.Normal, FontStretch.Normal, UIScaleFactor.ConvertFontPointsToDips(24));
        using ITextLayout textLayout = dw.CreateTextLayout(SR.NoPath, textFormat);
        textLayout.WordWrapping = WordWrapping.NoWrap;
        TextMeasurement textMeasurement = textLayout.Measure();
        int width = (int)textMeasurement.Width;
        int height = (int)textMeasurement.Height;
        //StrokeStyleProperties strokeStyleProperties = StrokeStyleProperties.Default;
        //strokeStyleProperties.LineJoin = LineJoin.Bevel;
        //strokeStyleProperties.MiterLimit = 0;
        Surface surface = new Surface(width, height, SurfaceCreationFlags.DoNotZeroFillHint);
        using (IBitmap<ColorBgra32> shared = surface.CreateSharedBitmap())
        using (IBitmap<ColorPbgra32> premltd = shared.CreatePremultipliedAdapter(PremultipliedAdapterOptions.UnPremultiplyOnDispose))
        using (IDeviceContext dc = d2d.CreateBitmapDeviceContext(premltd))
        using (ISolidColorBrush textBrush = dc.CreateSolidColorBrush(Color.Black))
        //using (ISolidColorBrush strokeBrush = dc.CreateSolidColorBrush(Color.Red))
        //using (IGeometry textGeometry = d2d.CreateGeometryFromTextLayout(textLayout))
        //using (IStrokeStyle strokeStyle = d2d.CreateStrokeStyle(strokeStyleProperties))
        //using (IGeometry widenedGeometry = textGeometry.Widen(1, strokeStyle))
        //using (IGeometry outerStrokeGeometry = widenedGeometry.CombineWithGeometry(textGeometry, GeometryCombineMode.Exclude))
        using (dc.UseTextRenderingMode(TextRenderingMode.Default))
        using (dc.UseTextAntialiasMode(TextAntialiasMode.Grayscale))
        using (dc.UseBeginDraw())
        {
            dc.Clear(Color.LightGray);
            dc.DrawTextLayout(0, 0, textLayout, textBrush, DrawTextOptions.EnableColorFont);
            //dc.FillGeometry(textGeometry, textBrush);
            //dc.FillGeometry(outerStrokeGeometry, strokeBrush);
        }
        return surface.CreateSingleLayerDocument(takeOwnership: true);
    }

    private void RenderSvgDocument(SvgElement element, IDeviceContext dc)
    {
        if (element is SvgUse use)
        {
            RenderSvgUseElement(use, e => RenderSvgDocument(e, dc));
        }
        else
        {
            SvgDocument clone = element.OwnerDocument.RemoveInvisibleAndNonTextElements();
            using MemoryStream stream = new MemoryStream();
            clone.WriteXML_QuotedFuncIRIHack(stream);
            using ISvgDocument partial = dc.CreateSvgDocument(stream, dc.Size);
            using DrawingScope _ = dc.UseBeginDraw();
            dc.Clear();
            dc.DrawSvgDocument(partial);
        }
    }

    private static Matrix3x2Float CalculateTransform(SizeF svgsize, SvgImportConfig config)
    {
        float ratioX = config.RasterWidth / svgsize.Width;
        float ratioY = config.PreserveAspectRatio ? ratioX : config.RasterHeight / svgsize.Height;
        return new Matrix3x2Float()
        {
            M11 = ratioX,
            M22 = ratioY
        };
    }
}
