// Copyright 2025 Osman Tunçelli. All rights reserved.
// Use of this source code is governed by GNU General Public License (GPL-2.0) that can be found in the COPYING file.

using System;
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
    private const string NotAvailable = "-";
    private const int CanvasSizeWarningThreshold = 1280;
    private const int DefaultFallbackSize = 512;
    private object? lastModifiedNud;
    private bool dontUpdate;
    private readonly CancellationTokenSource? cts;
    private readonly Size docSize;
    private readonly int docPpi;
    private readonly string svgdata;
    private Size rasterSize;
    private readonly Rectangle docViewbox;
    private readonly string renderer;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Document? Result { get; private set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Exception? Error { get; private set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public override bool ImmersiveDarkMode => true;

    public SvgImportDialog(string svgdata, string renderer = "resvg", CancellationToken ctoken = default) : base(UIHelper.GetMainForm())
    {
        ArgumentNullException.ThrowIfNull(svgdata);
        ArgumentException.ThrowIfNullOrWhiteSpace(renderer);

        InitializeComponent();
        SetUseAppThemeColors();

        this.renderer = renderer;
        this.svgdata = svgdata;
        cts = CancellationTokenSource.CreateLinkedTokenSource(ctoken);

        SvgDocument svg = SvgDocument.FromSvg<SvgDocument>(svgdata);
        docSize = svg.GetDimensions().ToSize();
        docViewbox = Rectangle.Truncate(svg.ViewBox);
        docPpi = svg.Ppi;

        PopulateContols();
        HookEvents();
    }

    protected override void OnLoad(EventArgs e)
    {
        ClientSize = RootPanel.Size;
        base.OnLoad(e);
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        base.OnClosing(e);
        cts?.Dispose();
    }

    #region Private Methods

    private void PopulateContols()
    {
        Text = $"{SvgFileTypePluginSupportInfo.Instance.DisplayName} v{SvgFileTypePluginSupportInfo.Instance.Version}";
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
        ToolTip1.SetToolTip(PbWarning, SR.MemoryWarningText);

        TbViewportW.Text = docSize.Width > 0 ? docSize.Width.ToString() : NotAvailable;
        TbViewportH.Text = docSize.Height > 0 ? docSize.Height.ToString() : NotAvailable;

        if (!docViewbox.IsEmpty)
        {
            TbViewboxX.Text = docViewbox.X.ToString();
            TbViewboxY.Text = docViewbox.Y.ToString();
            TbViewboxW.Text = docViewbox.Width.ToString();
            TbViewboxH.Text = docViewbox.Height.ToString();
        }
        else
        {
            TbViewboxX.Text = NotAvailable;
            TbViewboxY.Text = NotAvailable;
            TbViewboxW.Text = NotAvailable;
            TbViewboxH.Text = NotAvailable;
        }

        rasterSize = docSize.Width == 0 || docSize.Height == 0
            ? new Size(DefaultFallbackSize, DefaultFallbackSize)
            : docSize;

        if (rasterSize.Width * rasterSize.Height > CanvasSizeWarningThreshold * CanvasSizeWarningThreshold)
        {
            double ratio = rasterSize.Width / (double)rasterSize.Height;
            if (rasterSize.Width > rasterSize.Height)
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
        else
        {
            NudCanvasW.Value = rasterSize.Width;
            NudCanvasH.Value = rasterSize.Height;
        }

        UpdateLayersPanel();
    }

    private void HookEvents()
    {
        RbAllElements.CheckedChanged += Rb_CheckedChanged;
        RbFlatten.CheckedChanged += Rb_CheckedChanged;
        RbGroups.CheckedChanged += Rb_CheckedChanged;
        NudCanvasW.Accelerations.Add(new NumericUpDownAcceleration(3, 10));
        NudCanvasW.KeyUp += NudCanvas_KeyUp;
        NudCanvasH.Accelerations.Add(new NumericUpDownAcceleration(3, 10));
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

    private void UpdateLayersPanel()
    {
        CbOpacity.Enabled = CbHiddenLayers.Enabled = CbGroupBoundaries.Enabled = !RbFlatten.Checked;
        CbGroupBoundaries.Enabled = RbAllElements.Checked;
        ShowCanvasSizeWarningIfNeeded();
    }

    private void ShowCanvasSizeWarningIfNeeded()
    {
        PbWarning.Visible = !RbFlatten.Checked && NudCanvasH.Value * NudCanvasW.Value > CanvasSizeWarningThreshold * CanvasSizeWarningThreshold;
    }

    private void UpdateTheOtherNud()
    {
        lastModifiedNud ??= NudCanvasW.Value > NudCanvasH.Value ? NudCanvasW : NudCanvasH;

        if (ReferenceEquals(lastModifiedNud, NudCanvasW))
        {
            decimal newHeight = CbKeepAR.Checked ? NudCanvasW.Value * rasterSize.Height / rasterSize.Width : NudCanvasH.Value;
            newHeight = Math.Clamp(newHeight, 1, Math.Min(NudCanvasH.Maximum, int.MaxValue / (NudCanvasW.Value * 4)));
            NudCanvasH.Value = newHeight;
        }
        else
        {
            decimal newWidth = CbKeepAR.Checked ? NudCanvasH.Value * rasterSize.Width / rasterSize.Height : NudCanvasW.Value;
            newWidth = Math.Clamp(newWidth, 1, Math.Min(NudCanvasW.Maximum, int.MaxValue / (NudCanvasH.Value * 4)));
            NudCanvasW.Value = newWidth;
        }
        ShowCanvasSizeWarningIfNeeded();
    }

    private SvgImportConfig GetSvgImportConfig()
    {
        SvgImportConfig config = new SvgImportConfig
        {
            RasterWidth = (int)NudCanvasW.Value,
            RasterHeight = (int)NudCanvasH.Value,
            PreserveAspectRatio = CbKeepAR.Checked,
            Ppi = (int)NudDpi.Value,
            GroupBoundariesAsLayers = CbGroupBoundaries.Checked,
            ImportHiddenElements = CbHiddenLayers.Checked,
            RespectElementOpacity = CbOpacity.Checked,
            LayersMode = RbAllElements.Checked
                ? LayersMode.All
                : RbFlatten.Checked
                ? LayersMode.Flat
                : LayersMode.Groups
        };
        return config;
    }

    #endregion

    #region Events

    private void NudCanvas_ValueChanged(object? sender, EventArgs e)
    {
        if (dontUpdate)
            return;
        lastModifiedNud = sender;
        UpdateTheOtherNud();
    }

    private void CbKeepAR_CheckedChanged(object? sender, EventArgs e)
    {
        if (dontUpdate)
            return;
        lastModifiedNud = null;
        UpdateTheOtherNud();
    }

    private void NudCanvas_KeyUp(object? sender, KeyEventArgs e)
    {
        if (dontUpdate)
            return;

        lastModifiedNud = sender;
        if (e.KeyValue >= '0' || e.KeyValue <= '9' || e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
        {
            // Handle key events like digits, delete and backspace
            UpdateTheOtherNud();
        }
        else
        {
            // We ignore any other key event on these controls
            e.SuppressKeyPress = true;
            e.Handled = true;
        }
    }

    private void NudCanvas_LostFocus(object? sender, EventArgs e)
    {
        if (sender is not NumericUpDown nud)
            return;

        TextBox? textbox = nud.Controls.OfType<TextBox>().FirstOrDefault();
        if (textbox is not null)
            textbox.Text = Math.Round(nud.Value, nud.DecimalPlaces).ToString();
    }

    private void Rb_CheckedChanged(object? sender, EventArgs e)
    {
        UpdateLayersPanel();
    }

    private void LnkUseSvgSettings_Click(object? sender, EventArgs e)
    {
        // Restore the original sizes and show the size warning if needed
        int w = rasterSize.Width;
        int h = rasterSize.Height;
        dontUpdate = true;
        NudCanvasW.Value = w;
        NudCanvasH.Value = h;
        dontUpdate = false;
        NudDpi.Value = docPpi;
        ShowCanvasSizeWarningIfNeeded();
    }

    private void LnkGitHub_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
            return;
        LaunchUrl(SvgFileTypePluginSupportInfo.Instance.WebsiteUri);
    }

    private void LnkForum_LinkClicked(object? sender, LinkLabelLinkClickedEventArgs e)
    {
        if (e.Button != MouseButtons.Left)
            return;
        LaunchUrl(SvgFileTypePluginSupportInfo.Instance.ForumUri);
    }

    private async void BtnOk_Click(object? sender, EventArgs e)
    {
        Debug.Assert(cts is not null);

        this.Descendants().OfType<GroupBox>().ToList().ForEach(gb => gb.Enabled = false);
        BtnOk.Enabled = false;
        ProgressBar.Visible = true;
        SvgImportConfig config = GetSvgImportConfig();
        using var _ = cts;
        CancellationToken ctoken = cts.Token;
        SvgRenderer2 svgRenderer = SvgRenderer2.Create(renderer);
        svgRenderer.ProgressChanged += OnProgressChanged;

        try
        {
            Result = await Task.Run<Document?>(() => svgRenderer.Rasterize(svgdata, config, ctoken), ctoken)
                .ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                        return task.Result;
                    Error = task.IsCanceled 
                        ? new WarningException(SR.CanceledUponYourRequest, task.Exception)
                        : task.Exception;
                    return null;
                }, ctoken).ConfigureAwait(false);
            DialogResult = Result is not null
                ? DialogResult.OK
                : DialogResult.Cancel;
        }
        catch (Exception ex) when (ex is OperationCanceledException or TaskCanceledException)
        {
            Error = new WarningException(SR.CanceledUponYourRequest, ex);
            DialogResult = DialogResult.Cancel;
        }
        catch (Exception ex)
        {
            Error = ex;
            DialogResult = DialogResult.Cancel;
        }
    }

    private void OnProgressChanged(object? sender, ProgressChangedEventArgs e)
    {
        if (IsDisposed)
            return;

        try
        {
            if (InvokeRequired)
            {
                Invoke(OnProgressChanged, sender, e);
                return;
            }

            int total = (int)e.UserState!;
            if (e.ProgressPercentage == 0)
            {
                if (total == 1)
                {
                    ProgressBar.Style = ProgressBarStyle.Marquee;
                    ProgressLabel.Text = SR.Working;
                }
                else
                {
                    ProgressBar.Style = ProgressBarStyle.Blocks;
                    ProgressBar.Maximum = total;
                    ProgressLabel.Text = SR.Ready;
                }
            }
            if (total > 1)
            {
                ProgressBar.Value = e.ProgressPercentage;
                float percentage = e.ProgressPercentage / (float)total;
                ProgressLabel.Text = percentage.ToString("P2");
            }
        }
        catch (ObjectDisposedException)
        {
            // ignore
        }
    }

    private void BtnCancel_Click(object? sender, EventArgs e)
    {
        BtnCancel.Enabled = false;
        try
        {
            cts?.Cancel();
        }
        catch (ObjectDisposedException)
        {
            // This should never happen
        }
        finally
        {
            Error = new WarningException(SR.CanceledUponYourRequest);
        }
    }

    #endregion

    #region Static

    private static void LaunchUrl(Uri uri)
    {
        Process.Start("explorer", $@"""{uri}""");
    }

    #endregion
}
