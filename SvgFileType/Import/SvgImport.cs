// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Windows.Forms;
using PaintDotNet;
using Svg;
using SvgFileTypePlugin.Extensions;

namespace SvgFileTypePlugin.Import;

internal static class SvgImport
{
    public static Document Load(Stream stream)
    {
        ArgumentNullException.ThrowIfNull(stream);
        if (!stream.CanRead)
            throw new IOException("Input stream is not readable.");
        if (stream.Length <= 0)
            return new Direct2DSvgRenderer().GetNoPathDocument();

        // We are using resvg which gives the best result in my tests.
        // For testing purposes, I also have implemented GDI+ and Direct2D based SVG renderers as well.
        const string rendererName = "resvg";

        string svgdata = Open(stream);
        using StreamTrackerCancellationTokenSource cts = new(stream);
        CancellationToken ctoken = cts.Token;
        if (UIHelper.IsSaveConfigDialogVisible())
        {
            // This is workaround for Saving functionality.
            // We don't want the dialog appear if Paint.NET is generating previews.
            SvgDocument svg = SvgDocument.FromSvg<SvgDocument>(svgdata);
            SvgRenderer2 svgRenderer = SvgRenderer2.Create(rendererName);
            SvgImportConfig config = new SvgImportConfig
            {
                LayersMode = LayersMode.Flat,
                RasterWidth = svg.Width.ToDeviceValue(svg),
                RasterHeight = svg.Height.ToDeviceValue(svg),
                Ppi = 96
            };
            return svgRenderer.Rasterize(svgdata, config, ctoken);
        }

        Document? document = UIHelper.RunOnUIThread(() =>
        {
            using SvgImportDialog dialog = new SvgImportDialog(svgdata, rendererName, ctoken);
            DialogResult dialogResult = dialog.ShowDialog();
            return dialogResult == DialogResult.OK ? dialog.Result : throw dialog.Error ?? new InvalidOperationException();
        });
        Debug.Assert(document is not null);
        return document;
    }

    private static string Open(Stream input)
    {
        using MemoryStream ms = new();
        Span<byte> buf = new byte[3];
        if (input.Read(buf) < 3)
            throw new IOException("Input stream is not a valid SVG.");
        ms.Write(buf);
        input.CopyTo(ms);

        // Do not close the source stream.
        // It also can be used to track cancellation.

        ms.Position = 0;

        ReadOnlySpan<byte> gzipMagicBytes = [0x1F, 0x8B, 0x8];
        input = buf.SequenceEqual(gzipMagicBytes)
            ? new GZipStream(ms, CompressionMode.Decompress, leaveOpen: false)
            : ms;
        using StreamReader reader = new StreamReader(input, leaveOpen: false);
        return reader.ReadToEnd();
    }
}
