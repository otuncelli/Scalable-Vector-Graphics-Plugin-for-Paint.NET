// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PaintDotNet;
using Svg;
using SvgFileTypePlugin.Extensions;
using SR = SvgFileTypePlugin.Localization.StringResources;

namespace SvgFileTypePlugin.Import;

internal partial class SvgImportDialog : MyBaseForm
{
    #region Properties

    private Document? document;
    private Exception? reason;
    private const string NotAvailable = "-";
    private const int CanvasSizeWarningThreshold = 1280;
    private object? lastModifiedNud;
    private readonly Size docSize;
    private bool dontUpdate;
    private readonly SvgDocument svg;
    private int layerCount;
    private CancellationTokenSource? cts;

    #endregion

    #region Constructor

    private SvgImportDialog(SvgDocument svg) : base(UIHelper.GetMainForm())
    {
        ArgumentNullException.ThrowIfNull(svg);

        this.svg = svg;
        InitializeComponent();
        SetUseAppThemeColors();
        PopulateControls();
        InitDocSize(out docSize);
        HookEvents();
    }

    #endregion

    #region Private

    #region Initialize UI

    private void InitDocSize(out Size size)
    {
        Rectangle viewbox = svg.ViewBox.ToRectangle();
        size = Size.Round(svg.GetDimensions());

        if (size.Width > 0 && size.Height > 0)
        {
            TbViewportW.Text = size.Width.ToString();
            TbViewportH.Text = size.Height.ToString();
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

            if (size.Width == 0 || size.Height == 0)
            {
                size = new Size(viewbox.Width, viewbox.Height);
            }
        }
        else
        {
            TbViewboxX.Text = NotAvailable;
            TbViewboxY.Text = NotAvailable;
            TbViewboxW.Text = NotAvailable;
            TbViewboxH.Text = NotAvailable;
        }

        if (size.Width == 0 || size.Height == 0)
        {
            // Can't detect dimensions, fallback
            NudCanvasW.Value = NudCanvasW.Value = 512;
        }
        else
        {
            double ratio = size.Width / (double)size.Height;
            if (size.Width > CanvasSizeWarningThreshold)
            {
                if (size.Width > size.Height)
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
            else if (size.Height > CanvasSizeWarningThreshold)
            {
                NudCanvasH.Value = CanvasSizeWarningThreshold;
                NudCanvasW.Value = (int)Math.Round(CanvasSizeWarningThreshold * ratio);
            }
            else
            {
                NudCanvasW.Value = size.Width;
                NudCanvasH.Value = size.Height;
            }
        }

        UpdateLayerMode();
    }

    private void PopulateControls()
    {
        Text = string.Join(" v", MyPluginSupportInfo.Instance.DisplayName, MyPluginSupportInfo.Instance.Version);
        PbWarning.Image = SystemIcons.Warning.ToBitmap();
        GbInfo.Text = SR.SizeSettingsGivenInSvgFile;
        LnkUseSvgSettings.Text = SR.UseSizeSettingsGivenInSvg;
        GbSizeSelection.Text = SR.SizeSelectionByUser;
        LblResolution.Text = SR.Resolution;
        LblCanvasWH.Text = SR.Canvas;
        LblViewport.Text = SR.Viewport;
        LblViewboxWH.Text = SR.ViewBoxWH;
        LblViewboxXY.Text = SR.ViewBoxXY;
        CbKeepAR.Text = SR.KeepAspectRatio;
        GbLayers.Text = SR.Layers;
        RbFlatten.Text = SR.Flatten;
        RbGroups.Text = SR.Groups;
        RbAllElements.Text = SR.AllElements;
        CbOpacity.Text = SR.OpacityAsLayerProperty;
        CbHiddenLayers.Text = SR.ImportHiddenElements;
        CbGroupBoundaries.Text = SR.GroupBoundaries;
        BtnOk.Text = SR.OK;
        BtnCancel.Text = SR.Cancel;
        ProgressLabel.Text = SR.Ready;
        ToolTip1.SetToolTip(CbGroupBoundaries, SR.GroupBoundariesToolTip);
        ToolTip1.SetToolTip(PbWarning, SR.MemoryWarningText);
    }

    #endregion

    private void UpdateOtherInput()
    {
        lastModifiedNud ??= NudCanvasW.Value > NudCanvasH.Value ? NudCanvasW : NudCanvasH;

        if (ReferenceEquals(lastModifiedNud, NudCanvasW))
        {
            decimal newHeight = CbKeepAR.Checked ? NudCanvasW.Value * docSize.Height / docSize.Width : NudCanvasH.Value;
            newHeight = Math.Clamp(newHeight, 1, Math.Min(NudCanvasH.Maximum, int.MaxValue / (NudCanvasW.Value * 4)));
            NudCanvasH.Value = newHeight;
            PbWarning.Visible = !RbFlatten.Checked && (newHeight > CanvasSizeWarningThreshold || NudCanvasW.Value > CanvasSizeWarningThreshold);
        }
        else
        {
            decimal newWidth = CbKeepAR.Checked ? NudCanvasH.Value * docSize.Width / docSize.Height : NudCanvasW.Value;
            newWidth = Math.Clamp(newWidth, 1, Math.Min(NudCanvasW.Maximum, int.MaxValue / (NudCanvasH.Value * 4)));
            NudCanvasW.Value = newWidth;
            PbWarning.Visible = !RbFlatten.Checked && (newWidth > CanvasSizeWarningThreshold || NudCanvasH.Value > CanvasSizeWarningThreshold);
        }
    }

    private void UpdateLayerMode()
    {
        CbOpacity.Enabled = CbHiddenLayers.Enabled = CbGroupBoundaries.Enabled = !RbFlatten.Checked;
        CbGroupBoundaries.Enabled = RbAllElements.Checked;
        PbWarning.Visible = !RbFlatten.Checked && (NudCanvasH.Value > CanvasSizeWarningThreshold || NudCanvasW.Value > CanvasSizeWarningThreshold);
    }

    private Document DoImport(SvgImportConfig config, CancellationToken cancellationToken = default)
    {
        using IDisposable _ = svg.UseSetRasterDimensions(config);
        ISvgConverter svg2doc = SvgConverterFactory.Get();
        if (svg2doc.GetType() != typeof(DefaultSvgConverter) && config.LayersMode == LayersMode.Flat)
        {
            SetupProgress(1);
            return svg2doc.GetFlatDocument(svg, config, cancellationToken);
        }

        List<SvgVisualElement> prepared = svg2doc.Prepare(svg, config, cancellationToken).ToList();
        layerCount = prepared.Count;

        SetupProgress(layerCount);

        if (config.LayersMode == LayersMode.Flat)
        {
            using IDisposable _1 = Utils.UseMemoryFailPoint(config.Width, config.Height);
            return svg2doc.GetFlatDocument(prepared, config, SetProgress, cancellationToken);
        }

        Utils.EnsureMemoryAvailable(config.Width, config.Height, layerCount);
        return svg2doc.GetLayeredDocument(prepared, config, SetProgress, cancellationToken);
    }

    private Document GetDocument()
    {
        if (document != null) { return document; }
        if (reason != null) { throw reason; }
        throw new OperationCanceledException(SR.CanceledUponYourRequest); // This should never happen
    }

    #region Progress

    private void SetupProgress(int max)
    {
        StatusStrip.RunOnUIThread(() =>
        {
            ProgressBar.Visible = true;

            if (max == 1)
            {
                ProgressBar.Style = ProgressBarStyle.Marquee;
                ProgressLabel.Text = SR.Working;
            }
            else
            {
                ProgressBar.Style = ProgressBarStyle.Blocks;
                ProgressBar.Maximum = max;
            }
        });
    }

    private void SetProgress(int value)
    {
        if (Disposing || IsDisposed) { return; }
        try
        {
            StatusStrip.RunOnUIThread(() =>
            {
                value = Math.Clamp(value, ProgressBar.Minimum, ProgressBar.Maximum);
                if (value == ProgressBar.Maximum)
                {
                    // ProgressBar's animation is slow to catch up
                    // This workaround prevents the animation
                    ProgressBar.Maximum = value + 1;
                    ProgressBar.Value = value + 1;
                    ProgressBar.Maximum = value;
                }
                ProgressBar.Value = value;
                ProgressLabel.Text = (value / (float)layerCount).ToString("P2");  // string.Join(" / ", ProgressBar.Value, ProgressBar.Maximum);
            });
        }
        catch (ObjectDisposedException)
        {
            //
        }
    }

    #endregion

    #endregion

    #region Events

    private void HookEvents()
    {
        RbAllElements.CheckedChanged += Rb_CheckedChanged;
        RbFlatten.CheckedChanged += Rb_CheckedChanged;
        RbGroups.CheckedChanged += Rb_CheckedChanged;
        NudCanvasW.KeyUp += NudCanvas_KeyUp;
        NudCanvasH.KeyUp += NudCanvas_KeyUp;
        NudCanvasW.LostFocus += NudCanvas_LostFocus;
        NudCanvasH.LostFocus += NudCanvas_LostFocus;
        NudCanvasW.ValueChanged += NudCanvas_ValueChanged;
        NudCanvasH.ValueChanged += NudCanvas_ValueChanged;
        CbKeepAR.CheckedChanged += CbKeepAR_CheckedChanged;
        LnkUseSvgSettings.Click += LnkUseSvgSettings_Click;
        LnkGitHub.LinkClicked += LnkGitHub_LinkClicked;
        LnkForum.LinkClicked += LnkForum_LinkClicked;
        BtnOk.Click += BtnOk_Click;
        BtnCancel.Click += BtnCancel_Click;
        CancelButton = BtnCancel;
    }

    private void NudCanvas_ValueChanged(object? sender, EventArgs e)
    {
        if (dontUpdate) { return; }
        lastModifiedNud = sender;
        UpdateOtherInput();
    }

    private void CbKeepAR_CheckedChanged(object? sender, EventArgs e)
    {
        if (dontUpdate) { return; }
        UpdateOtherInput();
    }

    private void NudCanvas_KeyUp(object? sender, KeyEventArgs e)
    {
        if (dontUpdate) { return; }
        lastModifiedNud = sender;
        if (e.KeyValue >= '0' || e.KeyValue <= '9' || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
        {
            // handle digit, delete and backspace
            UpdateOtherInput();
        }
        else
        {
            // ignore any other key
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }

    private void NudCanvas_LostFocus(object? sender, EventArgs e)
    {
        if (sender is not NumericUpDown nud)
        {
            return;
        }

        TextBox? textbox = nud.Controls.OfType<TextBox>().FirstOrDefault();
        if (textbox is not null)
        {
            textbox.Text = Math.Round(nud.Value, nud.DecimalPlaces).ToString();
        }
    }

    private void Rb_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateLayerMode();
    }

    private void LnkUseSvgSettings_Click(object? sender, EventArgs e)
    {
        // Keep original image size and show warning
        dontUpdate = true;
        NudCanvasW.Value = docSize.Width;
        NudCanvasH.Value = docSize.Height;
        dontUpdate = false;
        NudDpi.Value = svg.Ppi;
        PbWarning.Visible = !RbFlatten.Checked && (docSize.Width > CanvasSizeWarningThreshold || docSize.Height > CanvasSizeWarningThreshold);
    }

    private static void LaunchUrl(Uri uri)
    {
        Process.Start("explorer", $@"""{uri}""");
    }

    private void LnkGitHub_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        if (e.Button != MouseButtons.Left) { return; }
        LaunchUrl(MyPluginSupportInfo.Instance.WebsiteUri);
    }

    private void LnkForum_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        if (e.Button != MouseButtons.Left) { return; }
        LaunchUrl(MyPluginSupportInfo.Instance.ForumUri);
    }

    private async void BtnOk_Click(object? sender, EventArgs e)
    {
        this.Descendants().OfType<GroupBox>().ToList().ForEach(gb => gb.Enabled = false);
        BtnOk.Enabled = false;

        SvgImportConfig config = new SvgImportConfig
        {
            Width = (int)NudCanvasW.Value,
            Height = (int)NudCanvasH.Value,
            PreserveAspectRatio = CbKeepAR.Checked,
            Ppi = (int)NudDpi.Value,
            GroupBoundariesAsLayers = CbGroupBoundaries.Checked,
            ImportHiddenElements = CbHiddenLayers.Checked,
            RespectElementOpacity = CbOpacity.Checked,
            LayersMode = RbAllElements.Checked ? LayersMode.All : RbFlatten.Checked ? LayersMode.Flat : LayersMode.Groups
        };

        try
        {
            using (cts = new CancellationTokenSource())
            {
                CancellationToken ctoken = cts.Token;
                await Task.Run(() => document = DoImport(config, ctoken)).ContinueWith(task =>
                {
                    reason = task.Exception;
                    if (reason is OperationCanceledException ex)
                    {
                        reason = new WarningException(null, ex);
                    }
                }, TaskContinuationOptions.OnlyOnFaulted)
                    .ConfigureAwait(false);
            }
        }
        //catch (TaskCanceledException)
        //{
        //}
        catch (Exception ex)
        {
            reason = ex;
        }
        finally
        {
            Invoke(Close);
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        BtnCancel.Enabled = false;
        if (cts != null)
        {
            try
            {
                cts.Cancel();
            }
            catch (ObjectDisposedException)
            {
                // this should never happen
            }
        }
    }

    #endregion

    protected override void OnLoad(EventArgs e)
    {
        ClientSize = RootPanel.Size;
        base.OnLoad(e);
    }

    #region Public Static

    public static Document ShowAndGetResult(SvgDocument svg)
    {
        return UIHelper.RunOnUIThread(() =>
        {
            using SvgImportDialog dialog = new SvgImportDialog(svg);
            _ = dialog.ShowDialog();
            return dialog.GetDocument();
        });
    }

    #endregion
}
