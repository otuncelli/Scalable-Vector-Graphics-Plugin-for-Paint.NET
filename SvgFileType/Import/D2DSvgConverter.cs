// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

#define USE_D2DFORLAYERED

using System.Drawing;
using System.IO;
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
#if USE_D2DFORLAYERED
using System;
using System.Collections.Generic;
#endif

namespace SvgFileTypePlugin.Import;

internal class D2DSvgConverter : DefaultSvgConverter
{
    public static new D2DSvgConverter Instance { get; } = new D2DSvgConverter();

    public override string Name => "Direct2D";

    public override Document GetNoPathDocument()
    {
        string text = SR.NoPath;
        IDirectWriteFactory dw = Services.Get<IDirectWriteFactory>();
        IDirect2DFactory d2d = Services.Get<IDirect2DFactory>();
        using ITextFormat textFormat = dw.CreateTextFormat("Arial", null, FontWeight.Bold, FontStyle.Normal, FontStretch.Normal, UIScaleFactor.ConvertFontPointsToDips(24));
        using ITextLayout textLayout = dw.CreateTextLayout(text, textFormat);
        textLayout.WordWrapping = WordWrapping.NoWrap;
        TextMeasurement tm = textLayout.Measure();
        int width = (int)tm.Width;
        int height = (int)tm.Height;
        // StrokeStyleProperties ssprops = StrokeStyleProperties.Default;
        // ssprops.LineJoin = LineJoin.Bevel;
        // ssprops.MiterLimit = 0;
        Surface surface = new Surface(width, height, SurfaceCreationFlags.DoNotZeroFillHint);
        using (IBitmap<ColorBgra32> shared = surface.CreateSharedBitmap())
        using (IBitmap<ColorPbgra32> premltd = shared.CreatePremultipliedAdapter(PremultipliedAdapterOptions.UnPremultiplyOnDispose))
        using (IDeviceContext dc = d2d.CreateBitmapDeviceContext(premltd))
        using (ISolidColorBrush textBrush = dc.CreateSolidColorBrush(Color.Black))
        //using (ISolidColorBrush strokeBrush = dc.CreateSolidColorBrush(Color.Red))
        //using (IGeometry textGeometry = d2d.CreateGeometryFromTextLayout(layout))
        //using (IStrokeStyle strokeStyle = d2d.CreateStrokeStyle(ssprops))
        //using (IGeometry widenedGeometry = textGeometry.Widen(10, strokeStyle))
        //using (IGeometry outerStrokeGeometry = widenedGeometry.CombineWithGeometry(textGeometry, GeometryCombineMode.Exclude))
        using (dc.UseTextRenderingMode(TextRenderingMode.Default))
        using (dc.UseTextAntialiasMode(TextAntialiasMode.Grayscale))
        using (dc.UseBeginDraw())
        {
            dc.Clear(Color.LightGray);
            dc.DrawTextLayout(0, 0, textLayout, textBrush, DrawTextOptions.EnableColorFont);
            // dc.FillGeometry(textGeometry, textBrush);
            // dc.FillGeometry(outerStrokeGeometry, strokeBrush);
        }
        return surface.CreateDocument(takeOwnership: true);
    }

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
        int dpi = config.Ppi;
        Surface surface = new Surface(width, height, SurfaceCreationFlags.DoNotZeroFillHint);
        try
        {
            SizeFloat viewport = new SizeFloat(width, height);
            IDirect2DFactory d2d = Services.Get<IDirect2DFactory>();
            using IBitmap<ColorBgra32> sbitmap = surface.CreateSharedBitmap();
            using IBitmap<ColorPbgra32> pbitmap = sbitmap.CreatePremultipliedAdapter(PremultipliedAdapterOptions.UnPremultiplyOnDispose);
            using IDeviceContext dc = d2d.CreateBitmapDeviceContext(pbitmap);
            using ISvgDocument svg = dc.CreateSvgDocument(stream, viewport);
            using var _ = dc.UseBeginDraw();
            dc.Clear();
            dc.DrawSvgDocument(svg);
        }
        catch
        {
            surface.Dispose();
            throw;
        }
        Document document = surface.CreateDocument(takeOwnership: true);
        document.SetDpi(dpi);
        return document;
    }

#if USE_D2DFORLAYERED // Unfortunately, D2D doesn't support text elements.
    public override Document GetLayeredDocument(IReadOnlyCollection<SvgVisualElement> elements, SvgImportConfig config, Action<int>? progress = null, CancellationToken cancellationToken = default)
    {
        Logger.WriteLine($"Using {Name}...");
        cancellationToken.ThrowIfCancellationRequested();
        IEnumerable<BitmapLayer> GetLayers()
        {
            int layersProcessed = 0;
            IDirect2DFactory d2d = Services.Get<IDirect2DFactory>();
            using Surface surface = new Surface(config.Width, config.Height);
            using IBitmap<ColorBgra32> sharedBitmap = surface.CreateSharedBitmap();
            using IBitmap<ColorPbgra32> premultiplied = sharedBitmap.CreatePremultipliedAdapter(PremultipliedAdapterOptions.None);
            using IDeviceContext dc = d2d.CreateBitmapDeviceContext(premultiplied);
            // Render all visual elements that are passed here.
            foreach (SvgVisualElement element in elements)
            {
                cancellationToken.ThrowIfCancellationRequested();

                BitmapLayer? layer = null;

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

                RenderSvgDocument(element, (dc, true));

                surface.ConvertFromPremultipliedAlpha();
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
        (IDeviceContext dc, bool clear) = (ValueTuple<IDeviceContext, bool>)context;
        if (element is SvgUse use)
        {
            RenderSvgUseElement(use, e => RenderSvgDocument(e, dc));
        }
        else
        {
            SvgDocument clone = element.OwnerDocument.Cleanup();
            using Stream stream = clone.AsStream();
            using ISvgDocument svg1 = dc.CreateSvgDocument(stream, dc.Size);
            using var _ = dc.UseBeginDraw();
            if (clear)
            {
                dc.Clear();
            }
            dc.DrawSvgDocument(svg1);
        }
    }
#endif
}
