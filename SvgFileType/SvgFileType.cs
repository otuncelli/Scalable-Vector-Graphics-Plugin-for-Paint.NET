using PaintDotNet;
using Svg;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Windows.Forms;

namespace SvgFileTypePlugin
{
    public class SvgFileType : FileType
    {
        public SvgFileType()
            : base(
                "Scalable Vector Graphics",
                FileTypeFlags.SupportsLoading,
                new[] {".svg", ".svgz"})
        {
        }

        private static Form GetMainForm()
        {
            try
            {
                var form = Control.FromHandle(Process.GetCurrentProcess().MainWindowHandle) as Form;
                return form ?? Application.OpenForms["MainForm"];
            }
            catch
            {
                return null;
            }
        }

        protected override Document OnLoad(Stream input)
        {
            SvgDocument doc;
            using (var wrapper = new SvgStreamWrapper(input))
                doc = SvgDocument.Open<SvgDocument>(wrapper.SvgStream);

            bool keepAspectRatio;
            int resolution;
            int canvasw;
            int canvash;
            var vpw = 0;
            var vph = 0;
            if (!doc.Width.IsNone && !doc.Width.IsEmpty &&
                doc.Width.Type != SvgUnitType.Percentage)
                vpw = (int)doc.Width.Value;
            if (!doc.Height.IsNone && !doc.Height.IsEmpty &&
                doc.Height.Type != SvgUnitType.Percentage)
                vph = (int)doc.Height.Value;
            var vbx = (int)doc.ViewBox.MinX;
            var vby = (int)doc.ViewBox.MinY;
            var vbw = (int)doc.ViewBox.Width;
            var vbh = (int)doc.ViewBox.Height;
            DialogResult dr = DialogResult.Cancel;
            using (var dialog = new UiDialog())
            {
                Form mainForm = GetMainForm();
                if (mainForm != null)
                {
                    mainForm.Invoke((MethodInvoker)(() =>
                    {
                        dialog.SetSvgInfo(vpw, vph, vbx, vby, vbw, vbh);
                        dr = dialog.ShowDialog(mainForm);
                    }));
                }
                else
                {
                    dialog.SetSvgInfo(vpw, vph, vbx, vby, vbw, vbh);
                    dr = dialog.ShowDialog();
                }
                if (dr != DialogResult.OK)
                    throw new OperationCanceledException("Cancelled by user");
                canvasw = dialog.CanvasW;
                canvash = dialog.CanvasH;
                resolution = dialog.Dpi;
                keepAspectRatio = dialog.KeepAspectRatio;
            }
            var bmp = new Bitmap(canvasw, canvash);
            using (Graphics graph = Graphics.FromImage(bmp))
            {
                doc.Ppi = resolution;
                doc.Width = new SvgUnit(SvgUnitType.Pixel, canvasw);
                doc.Height = new SvgUnit(SvgUnitType.Pixel, canvash);
                doc.AspectRatio = keepAspectRatio
                    ? new SvgAspectRatio(SvgPreserveAspectRatio.xMinYMin)
                    : new SvgAspectRatio(SvgPreserveAspectRatio.none);
                doc.Draw(graph);
            }
            return Document.FromImage(bmp);
        }

        private sealed class SvgStreamWrapper : IDisposable
        {
            public Stream SvgStream { get; }

            public SvgStreamWrapper(Stream input)
            {
                if (input.Length < 3)
                    throw new InvalidDataException();
                var headerBytes = new byte[3];
                input.Read(headerBytes, 0, 3);
                input.Position = 0;
                if (headerBytes[0] == 0x1f && headerBytes[1] == 0x8b && headerBytes[2] == 0x8)
                    SvgStream = new GZipStream(input, CompressionMode.Decompress, true);
                else
                    SvgStream = input;
            }

            #region IDisposable

            private bool _disposed;

            public void Dispose()
            {
                if (_disposed)
                    return;
                if (SvgStream is GZipStream)
                    SvgStream.Dispose();
                _disposed = true;
            }

            #endregion
        }
    }
}