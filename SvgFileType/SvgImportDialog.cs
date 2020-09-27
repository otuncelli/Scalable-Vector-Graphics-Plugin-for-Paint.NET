using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SvgFileTypePlugin
{
    public partial class SvgImportDialog : Form
    {
        #region Properties

        public int TargetDpi
        {
            get => (int) nudDpi.Value;
            set => nudDpi.Value = value;
        }

        public int SourceDpi { get; set; } = 96;

        public int CanvasW
        {
            get => (int) canvasw.Value;
            set
            {
                canvasw.Value = value;
                UpdateCanvasH();
            }
        }

        public int CanvasH
        {
            get => (int) canvash.Value;
            set
            {
                canvash.Value = value;
                UpdateCanvasW();
            }
        }

        public int ViewportW
        {
            get => Int32.TryParse(vpw.Text, out int val) ? val : -1;
            set => vpw.Text = value <= 0 ? NotAvailable : value.ToString();
        }

        public int ViewportH
        {
            get => Int32.TryParse(vph.Text, out int val) ? val : -1;
            set => vph.Text = value <= 0 ? NotAvailable : value.ToString();
        }

        public int ViewBoxW
        {
            get => Int32.TryParse(vbw.Text, out int val) ? val : -1;
            set => vbw.Text = value <= 0 ? NotAvailable : value.ToString();
        }

        public int ViewBoxH
        {
            get => Int32.TryParse(vbh.Text, out int val) ? val : -1;
            set => vbh.Text = value <= 0 ? NotAvailable : value.ToString();
        }

        public int ViewBoxX
        {
            get => Int32.TryParse(vbx.Text, out int val) ? val : -1;
            set => vbx.Text = value < 0 ? NotAvailable : value.ToString();
        }

        public int ViewBoxY
        {
            get => Int32.TryParse(vby.Text, out int val) ? val : -1;
            set => vby.Text = value < 0 ? NotAvailable : value.ToString();
        }

        public bool KeepAspectRatio
        {
            get => cbKeepAR.Checked;
            set
            {
                cbKeepAR.Checked = value;
                if (sizeHint.Width > sizeHint.Height)
                {
                    UpdateCanvasW();
                }
                else
                {
                    UpdateCanvasH();
                }
            }
        }

        public bool ImportOpacity
        {
            get => cbOpacity.Checked;
            set => cbOpacity.Checked = value;
        }

        public bool ImportHiddenLayers
        {
            get => cbLayers.Checked;
            set => cbLayers.Checked = value;
        }

        public bool ImportGroupBoundariesAsLayers
        {
            get => cbPSDSupport.Checked;
            set => cbPSDSupport.Checked = value;
        }

        internal LayersMode LayersMode
        {
            get
            {
                if (rbAll.Checked) return LayersMode.All;
                if (rbFlat.Checked) return LayersMode.Flat;
                return LayersMode.Groups;
            }
            set
            {
                if (value == LayersMode.All) rbAll.Checked = true;
                else if (value == LayersMode.Flat) rbFlat.Checked = true;
                else rbGroups.Checked = true;
                UpdateOptionsAvail();
            }
        }

        public string Title
        {
            get => Text;
            set => Text = value;
        }

        #endregion

        public event EventHandler OkClick;

        #region Fields

        private const string NotAvailable = "n/a";
        private const int CanvasSizeWarningThreshold = 1280;
        private Size sizeHint;

        #endregion

        public SvgImportDialog()
        {
            InitializeComponent();
            rbAll.CheckedChanged += Rb_CheckedChanged;
            rbFlat.CheckedChanged += Rb_CheckedChanged;
            rbGroups.CheckedChanged += Rb_CheckedChanged;
            canvasw.KeyUp += Canvas_KeyUp;
            ;
            canvash.KeyUp += Canvas_KeyUp;
            canvasw.ValueChanged += CanvasW_ValueChanged;
            canvash.ValueChanged += CanvasH_ValueChanged;
            cbKeepAR.CheckedChanged += CanvasW_ValueChanged;
            linkLabel1.Click += BtnUseOriginal_Click;
            linkGitHub.LinkClicked += LinkGitHub_LinkClicked;
            btnOk.Click += BtnOk_Click;
            SetDefaults();
        }

        #region Private

        private void SetDefaults()
        {
            warningBox.Image = SystemIcons.Warning.ToBitmap();
            LayersMode = LayersMode.Flat;
            ImportOpacity = true;
            ImportHiddenLayers = true;
            ImportGroupBoundariesAsLayers = true;
            InitSizeHint();
            canvasw.Minimum = 1;
            canvash.Minimum = 1;
            cbKeepAR.Checked = true;
        }

        public void InitSizeHint()
        {
            int w, h;

            if (ViewportW > 0 && ViewportH > 0)
            {
                w = ViewportW;
                h = ViewportH;
            }
            else if (ViewBoxW > 0 && ViewBoxH > 0)
            {
                w = ViewBoxW;
                h = ViewBoxH;
            }
            else
            {
                w = h = 512;
            }

            double ratio = w / (double) h;
            if (w > CanvasSizeWarningThreshold)
            {
                if (w > h)
                {
                    w = CanvasSizeWarningThreshold;
                    h = (int) Math.Ceiling(CanvasSizeWarningThreshold / ratio);
                }
                else
                {
                    h = CanvasSizeWarningThreshold;
                    w = (int) Math.Ceiling(CanvasSizeWarningThreshold * ratio);
                }
            }
            else if (h > CanvasSizeWarningThreshold)
            {
                h = CanvasSizeWarningThreshold;
                w = (int) Math.Ceiling(CanvasSizeWarningThreshold * ratio);
            }

            sizeHint = new Size(w, h);
            UpdateCanvasW();
            UpdateCanvasH();
        }

        private void UpdateCanvasH()
        {
            if (KeepAspectRatio)
            {
                canvash.Value = canvasw.Value * sizeHint.Height / sizeHint.Width;
            }

            warningBox.Visible = canvash.Value > CanvasSizeWarningThreshold;
        }

        private void UpdateCanvasW()
        {
            if (KeepAspectRatio)
            {
                canvasw.Value = canvash.Value * sizeHint.Width / sizeHint.Height;
            }

            warningBox.Visible = canvasw.Value > CanvasSizeWarningThreshold;
        }

        private void UpdateOptionsAvail()
        {
            cbOpacity.Enabled = cbLayers.Enabled = cbPSDSupport.Enabled = !rbFlat.Checked;
            cbPSDSupport.Enabled = rbAll.Checked;
        }

        public void SetValuesGivenInSource()
        {
            // Keep original image size and show warning
            CanvasW = sizeHint.Width;
            CanvasH = sizeHint.Height;
            TargetDpi = SourceDpi;
            warningBox.Visible = CanvasW > CanvasSizeWarningThreshold ||
                                 CanvasH > CanvasSizeWarningThreshold;
        }

        #endregion

        #region Events

        private void CanvasW_ValueChanged(object sender, EventArgs e)
        {
            UpdateCanvasH();
        }

        private void CanvasH_ValueChanged(object sender, EventArgs e)
        {
            UpdateCanvasW();
        }

        private void Canvas_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue >= '0' || e.KeyValue <= '9' ||
                e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                var input = (NumericUpDown) sender;
                if (input == canvasw)
                {
                    UpdateCanvasH();
                }
                else
                {
                    UpdateCanvasW();
                }
            }
            else
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void Rb_CheckedChanged(object sender, EventArgs e)
        {
            UpdateOptionsAvail();
        }

        private void BtnUseOriginal_Click(object sender, EventArgs e)
        {
            SetValuesGivenInSource();
        }

        private void LinkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            const string url = @"https://github.com/" +
                               "otuncelli/Scalable-Vector-Graphics-Plugin-for-Paint.NET";
            Process.Start(url);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            btnOk.Enabled = gr1.Enabled = gr2.Enabled = gr3.Enabled = false;
            OkClick?.Invoke(this, e);
        }

        #endregion

        #region Progress Reporting

        public void ReportProgress(int value)
        {
            if (progress.InvokeRequired)
            {
                progress.Invoke((Action) (() => ReportProgress(value)));
                return;
            }

            progress.Value = value;
            UpdateProgressLabel();
        }

        public void SetMaxProgress(int max)
        {
            if (progress.InvokeRequired)
            {
                progress.Invoke((Action) (() => SetMaxProgress(max)));
                return;
            }

            progress.Maximum = max;
            UpdateProgressLabel();
        }

        private void UpdateProgressLabel()
        {
            lbProgress.Text = progress.Value + " of " + progress.Maximum;
        }

        #endregion
    }
}