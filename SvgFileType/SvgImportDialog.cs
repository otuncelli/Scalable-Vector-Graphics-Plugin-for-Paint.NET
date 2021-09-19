// Copyright 2021 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by a LGPL license that can be
// found in the COPYING file.

using Svg;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SvgFileTypePlugin
{
    internal partial class SvgImportDialog : Form
    {
        #region Properties

        public SvgImportConfig GetConfig()
        {
            return new SvgImportConfig
            {
                Width = (int)NudCanvasW.Value,
                Height = (int)NudCanvasH.Value,
                KeepAspectRatio = CbKeepAR.Checked,
                Ppi = (int)NudDpi.Value,
                GroupBoundariesAsLayers = CbGroupBoundaries.Checked,
                HiddenElements = CbHiddenLayers.Checked,
                RespectElementOpacity = CbOpacity.Checked,
                LayersMode = LayersMode
            };
        }

        internal LayersMode LayersMode
        {
            get
            {
                if (RbAllLayers.Checked) return LayersMode.All;
                if (RbFlatten.Checked) return LayersMode.Flat;
                return LayersMode.Groups;
            }
            set
            {
                if (value == LayersMode.All) RbAllLayers.Checked = true;
                else if (value == LayersMode.Flat) RbFlatten.Checked = true;
                else RbGroups.Checked = true;
                UpdateOptionsAvail();
            }
        }

        #endregion

        #region Fields

        public event EventHandler OkClick;
        private const string NotAvailable = "n/a";
        private const int CanvasSizeWarningThreshold = 1280;
        private object lastChangedCtrl;
        private readonly Rectangle viewbox;
        private readonly int sourceWidth, sourceHeight;
        private readonly int sourcePpi;

        #endregion

        #region Constructor

        public SvgImportDialog(SvgDocument svg)
        {
            InitializeComponent();
            FixProgressLabel();

            Text = "SVG Import Plugin v" + MyPluginSupportInfo.VersionString;
            PbWarning.Image = SystemIcons.Warning.ToBitmap();
            ToolTip1.SetToolTip(PbWarning, "Please make sure you've enough memory.");
            LayersMode = LayersMode.Flat;
            CbOpacity.Checked = true;
            CbHiddenLayers.Checked = true;
            CbGroupBoundaries.Checked = false;
            NudCanvasW.Minimum = 1;
            NudCanvasH.Minimum = 1;
            CbKeepAR.Checked = true;
            sourceWidth = svg.Width.ToPixels(svg);
            sourceHeight = svg.Height.ToPixels(svg);
            sourcePpi = svg.Ppi;
            viewbox = svg.ViewBox.ToRectangle();

            if (sourceWidth > 0 && sourceHeight > 0)
            {
                TbViewportW.Text = sourceWidth.ToString();
                TbViewportH.Text = sourceHeight.ToString();
            }
            else
            {
                TbViewportW.Text = NotAvailable;
                TbViewportH.Text = NotAvailable;
            }

            if (!viewbox.IsEmpty)
            {
                TbViewboxX.Text = viewbox.X.ToString();
                TbViewboxY.Text = viewbox.Y.ToString();
                TbViewboxW.Text = viewbox.Width.ToString();
                TbViewboxH.Text = viewbox.Height.ToString();
            }
            else
            {
                TbViewboxX.Text = NotAvailable;
                TbViewboxY.Text = NotAvailable;
                TbViewboxW.Text = NotAvailable;
                TbViewboxH.Text = NotAvailable;
            }

            if (!viewbox.IsEmpty) // /*(sourceWidth == 0 || sourceHeight == 0) && */
            {
                sourceWidth = viewbox.Width;
                sourceHeight = viewbox.Height;
            }

            if (sourceWidth == 0 || sourceHeight == 0)
            {
                NudCanvasW.Value = NudCanvasW.Value = 512;
            }
            else
            {
                double ratio = sourceWidth / (double)sourceHeight;
                if (sourceWidth > CanvasSizeWarningThreshold)
                {
                    if (sourceWidth > sourceHeight)
                    {
                        NudCanvasW.Value = CanvasSizeWarningThreshold;
                        NudCanvasH.Value = (int)Math.Round(CanvasSizeWarningThreshold / ratio);
                    }
                    else
                    {
                        NudCanvasH.Value = CanvasSizeWarningThreshold;
                        NudCanvasW.Value = (int)Math.Round(CanvasSizeWarningThreshold * ratio);
                    }
                }
                else if (sourceHeight > CanvasSizeWarningThreshold)
                {
                    NudCanvasH.Value = CanvasSizeWarningThreshold;
                    NudCanvasW.Value = (int)Math.Round(CanvasSizeWarningThreshold * ratio);
                }
                else
                {
                    NudCanvasW.Value = sourceWidth;
                    NudCanvasH.Value = sourceHeight;
                }
            }

            BindEvents();
        }

        #endregion

        #region Private

        private void FixProgressLabel()
        {
            ProgressLabel.AutoSize = false;
            ProgressLabel.TextAlign = ContentAlignment.MiddleCenter;
            ProgressLabel.Location = ProgressBar.Location;
            ProgressLabel.Width = ProgressBar.Width;
            ProgressLabel.Height = ProgressBar.Height;
        }

        private void UpdateCanvasH()
        {
            decimal newHeight = CbKeepAR.Checked ? NudCanvasW.Value * sourceHeight / sourceWidth : NudCanvasH.Value;
            newHeight = Math.Max(newHeight, 1);
            NudCanvasH.Value = newHeight;
            PbWarning.Visible = !RbFlatten.Checked && (newHeight > CanvasSizeWarningThreshold || NudCanvasW.Value > CanvasSizeWarningThreshold);
        }

        private void UpdateCanvasW()
        {
            decimal newWidth = CbKeepAR.Checked ? NudCanvasH.Value * sourceWidth / sourceHeight : NudCanvasW.Value;
            newWidth = Math.Max(newWidth, 1);
            NudCanvasW.Value = newWidth;
            PbWarning.Visible = !RbFlatten.Checked && (newWidth > CanvasSizeWarningThreshold || NudCanvasH.Value > CanvasSizeWarningThreshold);
        }

        private void UpdateOptionsAvail()
        {
            CbOpacity.Enabled = CbHiddenLayers.Enabled = CbGroupBoundaries.Enabled = !RbFlatten.Checked;
            CbGroupBoundaries.Enabled = RbAllLayers.Checked;
            PbWarning.Visible = !RbFlatten.Checked && (NudCanvasH.Value > CanvasSizeWarningThreshold || NudCanvasW.Value > CanvasSizeWarningThreshold);
        }

        #endregion

        #region Events

        private void BindEvents()
        {
            RbAllLayers.CheckedChanged += Rb_CheckedChanged;
            RbFlatten.CheckedChanged += Rb_CheckedChanged;
            RbGroups.CheckedChanged += Rb_CheckedChanged;
            NudCanvasW.KeyUp += NudCanvas_KeyUp;
            NudCanvasH.KeyUp += NudCanvas_KeyUp;
            NudCanvasW.ValueChanged += NudCanvasW_ValueChanged;
            NudCanvasH.ValueChanged += NudCanvasH_ValueChanged;
            CbKeepAR.CheckedChanged += CbKeepAR_CheckedChanged;
            LnkUseSvgSettings.Click += LnkUseSvgSettings_Click;
            LnkGitHub.LinkClicked += LnkGitHub_LinkClicked;
            BtnOk.Click += BtnOk_Click;
        }

        private void NudCanvasW_ValueChanged(object sender, EventArgs e)
        {
            lastChangedCtrl = sender;
            UpdateCanvasH();
        }

        private void NudCanvasH_ValueChanged(object sender, EventArgs e)
        {
            lastChangedCtrl = sender;
            UpdateCanvasW();
        }

        private void CbKeepAR_CheckedChanged(object sender, EventArgs e)
        {
            if (ReferenceEquals(lastChangedCtrl, NudCanvasW))
            {
                UpdateCanvasH();
            }
            else
            {
                UpdateCanvasW();
            }
        }

        private void NudCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            lastChangedCtrl = sender;

            if (e.KeyValue >= '0' || e.KeyValue <= '9' ||
                e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                var input = (NumericUpDown)sender;
                if (input == NudCanvasW)
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

        private void LnkUseSvgSettings_Click(object sender, EventArgs e)
        {
            // Keep original image size and show warning
            NudCanvasW.ValueChanged -= NudCanvasW_ValueChanged;
            NudCanvasH.ValueChanged -= NudCanvasH_ValueChanged;
            NudCanvasW.Value = sourceWidth;
            NudCanvasH.Value = sourceHeight;
            NudCanvasW.ValueChanged += NudCanvasW_ValueChanged;
            NudCanvasH.ValueChanged += NudCanvasH_ValueChanged;
            NudDpi.Value = sourcePpi;
            PbWarning.Visible = sourceWidth > CanvasSizeWarningThreshold ||
                                 sourceHeight > CanvasSizeWarningThreshold;
        }

        private void LnkGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("explorer", MyPluginSupportInfo.Url);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            BtnOk.Enabled = GbInfo.Enabled = GbSizeSelection.Enabled = GbLayers.Enabled = false;
            OkClick?.Invoke(this, e);
        }

        #endregion

        #region Progress Reporting

        public void ReportProgress(int value)
        {
            if (value < ProgressBar.Minimum || value > ProgressBar.Maximum)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            if (Disposing) { return; }
            if (ProgressBar.InvokeRequired)
            {
                ProgressBar.Invoke((MethodInvoker)(() => ReportProgress(value)));
                return;
            }

            ProgressBar.Value = value;
            UpdateProgressLabel();
        }

        public void SetMaxProgress(int max)
        {
            if (max <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(max));
            }
            if (Disposing) { return; }
            if (ProgressBar.InvokeRequired)
            {
                ProgressBar.Invoke((MethodInvoker)(() => SetMaxProgress(max)));
                return;
            }

            ProgressBar.Maximum = max;
            UpdateProgressLabel();
        }

        private void UpdateProgressLabel()
        {
            ProgressLabel.Text = ProgressBar.Value + " of " + ProgressBar.Maximum;
        }

        #endregion
    }
}