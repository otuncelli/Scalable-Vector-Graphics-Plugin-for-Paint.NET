// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

#define USE_RESVGFORLAYERED

#if RESVG

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using PaintDotNet;
using PaintDotNet.Rendering;
using resvg.net;
using Svg;
using SvgFileTypePlugin.Extensions;

namespace SvgFileTypePlugin.Import;

internal sealed class ResvgConverter : DefaultSvgConverter
{
    static ResvgConverter()
    {
        Resvg.InitLog();
    }

    public static new ResvgConverter Instance { get; } = new ResvgConverter();

    public override string Name => "resvg";

    private ResvgConverter()
    {
    }

    public override Document GetFlatDocument(SvgDocument svg, SvgImportConfig config, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(svg);
        ArgumentNullException.ThrowIfNull(config);

        using Stream stream = svg.AsStream();
        return GetFlatDocument(stream, config, cancellationToken);
    }

    public override Document GetFlatDocument(Stream stream, SvgImportConfig config, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(stream);
        ArgumentNullException.ThrowIfNull(config);

        Logger.WriteLine($"Using {Name}...");
        int width = config.Width;
        int height = config.Height;
        int dpi = config.Ppi;
        Surface surface = new Surface(width, height, SurfaceCreationFlags.DoNotZeroFillHint);
        try
        {
            using (Resvg resvg = Resvg.FromStream(stream, loadSystemFonts: true))
            {
                resvg.Render(surface.Scan0.Pointer, width, height);
            }
            surface.ConvertFromRgba();
            surface.ConvertFromPremultipliedAlpha();
            //surface.ConvertFromPrgba();
        }
        catch (Exception)
        {
            surface.Dispose();
            throw;
        }
        Document document = surface.CreateDocument(takeOwnership: true);
        document.SetDpi(dpi);
        return document;
    }

#if USE_RESVGFORLAYERED
    public override Document GetLayeredDocument(IReadOnlyCollection<SvgVisualElement> elements, SvgImportConfig config, Action<int>? progress = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(elements);
        ArgumentNullException.ThrowIfNull(config);

        Logger.WriteLine($"Using {Name}...");
        cancellationToken.ThrowIfCancellationRequested();
        IEnumerable<BitmapLayer> GetLayers()
        {
            int layersProcessed = 0;
            using ResvgOptions options = new ResvgOptions();
            using Surface surface = new Surface(config.Width, config.Height);
            options.LoadSystemFonts();

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

                surface.Clear();
                RenderSvgDocument(element, (options, surface));
                if (surface.IsEmpty())
                {
                    IncrementProgress(progress, ref layersProcessed);
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
        ArgumentNullException.ThrowIfNull(element);
        ArgumentNullException.ThrowIfNull(context);

        (ResvgOptions options, Surface surface) = (ValueTuple<ResvgOptions, Surface>)context;
        if (element is SvgUse use)
        {
            RenderSvgUseElement(use, e => RenderSvgDocument(e, context));
        }
        else
        {
            SvgDocument clone = element.OwnerDocument.Cleanup();
            using (Resvg resvg = Resvg.FromData(clone.GetXML2(), options))
            {
                resvg.Render(surface.Scan0.Pointer, surface.Width, surface.Height);
            }
            surface.ConvertFromRgba();
            surface.ConvertFromPremultipliedAlpha();
        }
    }
#endif
}
#endif
