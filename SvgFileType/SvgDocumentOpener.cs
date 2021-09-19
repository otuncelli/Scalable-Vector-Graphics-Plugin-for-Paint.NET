// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using Svg;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SvgFileTypePlugin
{
    internal static class SvgDocumentOpener
    {
        public static SvgDocument Open(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream), "Input stream can not be null.");
            if (!stream.CanRead) throw new IOException("Input stream is not readable.");

            byte[] first3Bytes = new byte[3];
            byte[] gzipHeaderBytes = { 0x1f, 0x8b, 0x8 };
            int read = stream.Read(first3Bytes, 0, 3);
            if (read < 3) throw new IOException("Input stream does not contain valid svg data.");

            bool svgz = first3Bytes.SequenceEqual(gzipHeaderBytes);
            Stream svgStream;

            if (stream.CanSeek)
            {
                stream.Seek(0, SeekOrigin.Begin);
                svgStream = svgz ? new GZipStream(stream, CompressionMode.Decompress, true) : stream;
                return FromStream(svgStream, svgz);
            }
            else
            {
                int length;
                try
                {
                    length = (int)stream.Length;
                }
                catch (NotSupportedException)
                {
                    length = 3;
                }

                using (var memory = new MemoryStream(length))
                {
                    memory.Write(first3Bytes, 0, 3);
                    stream.CopyTo(memory);
                    memory.Seek(0, SeekOrigin.Begin);
                    svgStream = svgz ? (Stream)new GZipStream(memory, CompressionMode.Decompress, false) : memory;
                    return FromStream(svgStream, svgz);
                }
            }
        }

        private static SvgDocument FromStream(Stream stream, bool svgz)
        {
            try
            {
                return SvgDocument.Open<SvgDocument>(stream);
            }
            finally
            {
                if (svgz)
                {
                    stream.Dispose();
                }
            }
        }
    }
}
