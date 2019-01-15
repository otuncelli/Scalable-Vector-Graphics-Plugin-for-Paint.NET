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
        }

        public int Dpi => (int) nudDpi.Value;
        public int CanvasW => (int) canvasw.Value;
        public int CanvasH => (int) canvash.Value;
        public bool KeepAspectRatio => cbKeepAR.Checked;
        private Size _sizeHint;
        public bool ImportOpacity => this.cbOpacity.Checked;
        public bool ImportHiddenLayers => this.cbLayers.Checked;

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

        public void SetSvgInfo(
            int viewportw,
            int viewporth,
            int viewboxx = 0,
            int viewboxy = 0,
            int viewboxw = 0,
            int viewboxh = 0,
            int dpi = 96)
        {
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
            canvasw.Value = _sizeHint.Width;
            canvash.Value = _sizeHint.Height;
            changedProgramatically = false;
        }

        bool changedProgramatically = false;
        private void canvasw_ValueChanged(object sender, EventArgs e)
        {
            if (!KeepAspectRatio || changedProgramatically)
                return;
            
            canvash.Value = canvasw.Value * _sizeHint.Height / _sizeHint.Width;
        }

        private void canvash_ValueChanged(object sender, EventArgs e)
        {
            if (!KeepAspectRatio)
                return;
            canvasw.Value = canvash.Value * _sizeHint.Width / _sizeHint.Height;
        }

        private void cbKeepAR_CheckedChanged(object sender, EventArgs e)
        {
            canvasw_ValueChanged(sender, e);
        }
    }
}
