using Svg;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace SvgFileTypePlugin
{
    /// <summary>
    /// Supports opening compressed variant of svg
    /// </summary>
    internal class SvgDocumentOpener : IDisposable
    {
        private static readonly byte[] GZipHeaders = {0x1f, 0x8b, 0x8};

        private Stream InnerStream { get; }

        private SvgDocumentOpener(Stream input)
        {
            if (input.Length < 3)
            {
                throw new InvalidDataException();
            }

            var headerBytes = new byte[3];
            input.Read(headerBytes, 0, headerBytes.Length);
            input.Position = 0;
            
            // seems like input stream managed by paint.net
            // so we leave it open and not dispose it
            InnerStream = headerBytes.SequenceEqual(GZipHeaders)
                ? new GZipStream(input, CompressionMode.Decompress, true) 
                : input;
        }

        public static SvgDocument FromStream(Stream stream)
        {
            using (var wrapper = new SvgDocumentOpener(stream))
            {
                return SvgDocument.Open<SvgDocument>(wrapper.InnerStream);
            }
        }

        #region IDisposable

        private bool disposed;

        public void Dispose()
        {
            if (disposed)
            {
                return;
            }

            // seems like input stream managed by paint.net
            // so we leave it open and not dispose it.
            // we only dispose the GZipStream created by
            // this object.
            if (InnerStream is GZipStream)
            {
                InnerStream.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
