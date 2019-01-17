using System;
using System.Drawing;
using System.Windows.Forms;

namespace SvgFileTypePlugin
{
    public partial class UiDialog : Form
    {
        public UiDialog()
        {
            InitializeComponent();
            this.warningBox.Image = SystemIcons.Warning.ToBitmap();
        }

        public int Dpi => (int) nudDpi.Value;
        public int CanvasW => (int) canvasw.Value;
        public int CanvasH => (int) canvash.Value;
        public bool KeepAspectRatio => cbKeepAR.Checked;
        private Size _sizeHint;
        public bool ImportOpacity => this.cbOpacity.Checked;
        public bool ImportHiddenLayers => this.cbLayers.Checked;
        public bool ImportLayers => cbPSDSupport.Checked;

        private static int bigImageSize = 1280;
        public LayersMode LayerMode
        {
            get
            {
                if(rbAll.Checked)
                {
                    return LayersMode.All;
                }
                else if(rbFlat.Checked)
                {
                    return LayersMode.Flat;
                }
                else
                {
                    return LayersMode.Groups;
                }
            }
        }

        int originalPDI = 96;
        public void SetSvgInfo(
            int viewportw,
            int viewporth,
            int viewboxx = 0,
            int viewboxy = 0,
            int viewboxw = 0,
            int viewboxh = 0,
            int dpi = 96)
        {
            this.originalPDI = dpi;
            if (viewportw > 0)
                vpw.Text = viewportw.ToString();
            if (viewporth > 0)
                vph.Text = viewporth.ToString();
            if (viewboxx > 0)
                vbx.Text = viewboxx.ToString();
            if (viewboxy > 0)
                vby.Text = viewboxy.ToString();
            if (viewboxw > 0)
                vbw.Text = viewboxw.ToString();
            if (viewboxh > 0)
                vbh.Text = viewboxh.ToString();

            if (viewportw > 0 && viewporth > 0)
                _sizeHint = new Size(viewportw, viewporth);
            else if (viewboxx > 0 && viewboxy > 0)
                _sizeHint = new Size(viewboxx, viewboxy);
            else if (viewboxw > 0 && viewboxh > 0)
                _sizeHint = new Size(viewboxw, viewboxh);
            else
                _sizeHint = new Size(500, 500);

            this.nudDpi.Value = dpi;
            changedProgramatically = true;

            if (_sizeHint.Width > bigImageSize || _sizeHint.Height > bigImageSize)
            {
                warningBox.Visible = true;
                // Set default size from numberic default input and keep aspect ratio.
                // Default is 500
                canvash.Value = canvasw.Value * _sizeHint.Height / _sizeHint.Width;
            }
            else
            {
                warningBox.Visible = false;
                // Keep original image size and show warning
                canvasw.Value = _sizeHint.Width;
                canvash.Value = _sizeHint.Height;
            }

            changedProgramatically = false;

            ResolveControlsVisibility();
        }

        bool changedProgramatically = false;
        private void canvasw_ValueChanged(object sender, EventArgs e)
        {
            if (changedProgramatically)
                return;

            warningBox.Visible = false;

            if (!KeepAspectRatio)
                return;
           
            canvash.Value = canvasw.Value * _sizeHint.Height / _sizeHint.Width;
        }

        private void canvash_ValueChanged(object sender, EventArgs e)
        {
            if (changedProgramatically)
                return;

            warningBox.Visible = false;

            if (!KeepAspectRatio)
                return;

            canvasw.Value = canvash.Value * _sizeHint.Width / _sizeHint.Height;
        }

        private void cbKeepAR_CheckedChanged(object sender, EventArgs e)
        {
            canvasw_ValueChanged(sender, e);
        }

        private void btnUseOriginal_Click(object sender, EventArgs e)
        {
            changedProgramatically = true;
            warningBox.Visible = false;
            // Keep original image size and show warning
            canvasw.Value = _sizeHint.Width;
            canvash.Value = _sizeHint.Height;
            this.nudDpi.Value = originalPDI;
            changedProgramatically = false;
        }

        private void ResolvePropertiesVisibility(object sender, EventArgs e)
        {
            ResolveControlsVisibility();
        }

        private void ResolveControlsVisibility()
        {
            this.cbOpacity.Enabled = this.cbLayers.Enabled = this.cbPSDSupport.Enabled = !rbFlat.Checked;
        }
    }
}
