// Copyright 2023 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Threading;
using PaintDotNet;
using Svg;
using SvgFileTypePlugin.Extensions;

namespace SvgFileTypePlugin.Import;

internal static class SvgImport
{
    public static Document Load(Stream stream)
    {
        Ensure.IsNotNull(stream, nameof(stream));
        Ensure.IsTrue(stream.CanRead, () => throw new IOException("input stream is not readable."));
        Ensure.IsTrue(stream.CanSeek, () => throw new IOException("input stream is not seekable."));

        ISvgConverter svg2doc = SvgConverterFactory.Get();

        if (stream.Length <= 0)
        {
            return svg2doc.GetNoPathDocument();
        }

        SvgDocument svg = Open(stream);
        if (UIHelper.IsSaveConfigDialogVisible())
        {
            Logger.WriteLine("Generating preview...");
            SvgImportConfig config = new SvgImportConfig
            {
                LayersMode = LayersMode.Flat,
                Width = svg.Width.ToDeviceValue(svg),
                Height = svg.Height.ToDeviceValue(svg),
                Ppi = 96
            };
            return svg2doc.GetType() == typeof(DefaultSvgConverter)
                ? ((DefaultSvgConverter)svg2doc).GetFlatDocument(svg, stream, config)
                : svg2doc.GetFlatDocument(stream, config, CancellationToken.None);
        }

        return SvgImportDialog.ShowAndGetResult(svg);
    }

    private static SvgDocument Open(Stream stream)
    {
        stream.Position = 0;
        byte[] buf = new byte[3];
        if (stream.Read(buf, 0, 3) < 3)
        {
            throw new IOException("input stream is not a valid SVG.");
        }
        stream.Position = 0;
        GZipStream gzip = null;
        if (buf[0] == 0x1f && buf[1] == 0x8b && buf[2] == 0x8)
        {
            gzip = new GZipStream(stream, CompressionMode.Decompress, leaveOpen: true);
        }

        try
        {
            SvgDocument svg = SvgDocument.Open<SvgDocument>(gzip ?? stream);
            // Empty ViewBox workaround
            SizeF sizef = svg.GetDimensions();
            if (svg.ViewBox.IsEmpty() && !sizef.IsEmpty)
            {
                svg.ViewBox = new SvgViewBox(0f, 0f, sizef.Width, sizef.Height);
            }
            return svg;
        }
        finally
        {
            stream.Position = 0;
            gzip?.Dispose();
        }
    }
}
