namespace SvgFileTypePlugin.Import;

partial class SvgImportDialog
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        ToolTip1 = new System.Windows.Forms.ToolTip(components);
        PbWarning = new System.Windows.Forms.PictureBox();
        CbGroupBoundaries = new System.Windows.Forms.CheckBox();
        RootPanel = new System.Windows.Forms.Panel();
        StatusStrip = new System.Windows.Forms.StatusStrip();
        ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
        ProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
        PlaceHolderLabel = new System.Windows.Forms.ToolStripStatusLabel();
        UpdateAvailLabel = new System.Windows.Forms.ToolStripStatusLabel();
        tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
        LnkGitHub = new System.Windows.Forms.LinkLabel();
        LnkForum = new System.Windows.Forms.LinkLabel();
        BtnOk = new System.Windows.Forms.Button();
        BtnCancel = new System.Windows.Forms.Button();
        GbLayers = new System.Windows.Forms.GroupBox();
        tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        RbFlatten = new System.Windows.Forms.RadioButton();
        RbGroups = new System.Windows.Forms.RadioButton();
        CbHiddenLayers = new System.Windows.Forms.CheckBox();
        RbAllElements = new System.Windows.Forms.RadioButton();
        CbOpacity = new System.Windows.Forms.CheckBox();
        GbSizeSelection = new System.Windows.Forms.GroupBox();
        CbKeepAR = new System.Windows.Forms.CheckBox();
        tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
        LblResolution = new System.Windows.Forms.Label();
        LblCanvasWH = new System.Windows.Forms.Label();
        NudCanvasW = new System.Windows.Forms.NumericUpDown();
        NudCanvasH = new System.Windows.Forms.NumericUpDown();
        label9 = new System.Windows.Forms.Label();
        NudDpi = new System.Windows.Forms.NumericUpDown();
        GbInfo = new System.Windows.Forms.GroupBox();
        tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
        LnkUseSvgSettings = new System.Windows.Forms.Button();
        tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
        LblViewport = new System.Windows.Forms.Label();
        TbViewboxH = new System.Windows.Forms.TextBox();
        label6 = new System.Windows.Forms.Label();
        TbViewportW = new System.Windows.Forms.TextBox();
        label3 = new System.Windows.Forms.Label();
        TbViewboxW = new System.Windows.Forms.TextBox();
        TbViewportH = new System.Windows.Forms.TextBox();
        LblViewboxWH = new System.Windows.Forms.Label();
        LblViewboxXY = new System.Windows.Forms.Label();
        TbViewboxY = new System.Windows.Forms.TextBox();
        label4 = new System.Windows.Forms.Label();
        TbViewboxX = new System.Windows.Forms.TextBox();
        ((System.ComponentModel.ISupportInitialize)PbWarning).BeginInit();
        RootPanel.SuspendLayout();
        StatusStrip.SuspendLayout();
        tableLayoutPanel3.SuspendLayout();
        GbLayers.SuspendLayout();
        tableLayoutPanel1.SuspendLayout();
        GbSizeSelection.SuspendLayout();
        tableLayoutPanel4.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)NudCanvasW).BeginInit();
        ((System.ComponentModel.ISupportInitialize)NudCanvasH).BeginInit();
        ((System.ComponentModel.ISupportInitialize)NudDpi).BeginInit();
        GbInfo.SuspendLayout();
        tableLayoutPanel9.SuspendLayout();
        tableLayoutPanel2.SuspendLayout();
        SuspendLayout();
        // 
        // ToolTip1
        // 
        ToolTip1.AutomaticDelay = 0;
        ToolTip1.AutoPopDelay = 0;
        ToolTip1.InitialDelay = 0;
        ToolTip1.IsBalloon = true;
        ToolTip1.ReshowDelay = 0;
        ToolTip1.ShowAlways = true;
        ToolTip1.UseAnimation = false;
        // 
        // PbWarning
        // 
        PbWarning.Dock = System.Windows.Forms.DockStyle.Fill;
        PbWarning.Location = new System.Drawing.Point(42, 0);
        PbWarning.Margin = new System.Windows.Forms.Padding(0);
        PbWarning.MinimumSize = new System.Drawing.Size(23, 23);
        PbWarning.Name = "PbWarning";
        PbWarning.Size = new System.Drawing.Size(23, 29);
        PbWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        PbWarning.TabIndex = 2;
        PbWarning.TabStop = false;
        ToolTip1.SetToolTip(PbWarning, "Please make sure you've enough memory, especially if you're importing with layers.");
        PbWarning.Visible = false;
        // 
        // CbGroupBoundaries
        // 
        CbGroupBoundaries.AutoSize = true;
        CbGroupBoundaries.Dock = System.Windows.Forms.DockStyle.Fill;
        CbGroupBoundaries.Enabled = false;
        CbGroupBoundaries.Location = new System.Drawing.Point(99, 53);
        CbGroupBoundaries.Name = "CbGroupBoundaries";
        CbGroupBoundaries.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        CbGroupBoundaries.Size = new System.Drawing.Size(236, 19);
        CbGroupBoundaries.TabIndex = 2;
        CbGroupBoundaries.Text = "Group boundaries";
        ToolTip1.SetToolTip(CbGroupBoundaries, "Imports group boundaries as empty layers.");
        CbGroupBoundaries.UseVisualStyleBackColor = true;
        // 
        // RootPanel
        // 
        RootPanel.AutoSize = true;
        RootPanel.Controls.Add(StatusStrip);
        RootPanel.Controls.Add(tableLayoutPanel3);
        RootPanel.Controls.Add(GbLayers);
        RootPanel.Controls.Add(GbSizeSelection);
        RootPanel.Controls.Add(GbInfo);
        RootPanel.Dock = System.Windows.Forms.DockStyle.Top;
        RootPanel.Location = new System.Drawing.Point(0, 0);
        RootPanel.Name = "RootPanel";
        RootPanel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
        RootPanel.Size = new System.Drawing.Size(364, 421);
        RootPanel.TabIndex = 0;
        // 
        // StatusStrip
        // 
        StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ProgressBar, ProgressLabel, PlaceHolderLabel, UpdateAvailLabel });
        StatusStrip.Location = new System.Drawing.Point(10, 399);
        StatusStrip.Name = "StatusStrip";
        StatusStrip.Size = new System.Drawing.Size(344, 22);
        StatusStrip.SizingGrip = false;
        StatusStrip.TabIndex = 0;
        StatusStrip.Text = "statusStrip1";
        // 
        // ProgressBar
        // 
        ProgressBar.Name = "ProgressBar";
        ProgressBar.Size = new System.Drawing.Size(100, 16);
        ProgressBar.Visible = false;
        // 
        // ProgressLabel
        // 
        ProgressLabel.BackColor = System.Drawing.Color.Transparent;
        ProgressLabel.Name = "ProgressLabel";
        ProgressLabel.Size = new System.Drawing.Size(42, 17);
        ProgressLabel.Text = "Ready!";
        // 
        // PlaceHolderLabel
        // 
        PlaceHolderLabel.BackColor = System.Drawing.Color.Transparent;
        PlaceHolderLabel.Name = "PlaceHolderLabel";
        PlaceHolderLabel.Size = new System.Drawing.Size(287, 17);
        PlaceHolderLabel.Spring = true;
        // 
        // UpdateAvailLabel
        // 
        UpdateAvailLabel.BackColor = System.Drawing.Color.Transparent;
        UpdateAvailLabel.ForeColor = System.Drawing.Color.Red;
        UpdateAvailLabel.Name = "UpdateAvailLabel";
        UpdateAvailLabel.Size = new System.Drawing.Size(0, 17);
        // 
        // tableLayoutPanel3
        // 
        tableLayoutPanel3.AutoSize = true;
        tableLayoutPanel3.ColumnCount = 5;
        tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel3.Controls.Add(LnkGitHub, 0, 0);
        tableLayoutPanel3.Controls.Add(LnkForum, 1, 0);
        tableLayoutPanel3.Controls.Add(BtnOk, 3, 0);
        tableLayoutPanel3.Controls.Add(BtnCancel, 4, 0);
        tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
        tableLayoutPanel3.Location = new System.Drawing.Point(10, 350);
        tableLayoutPanel3.Name = "tableLayoutPanel3";
        tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(10);
        tableLayoutPanel3.RowCount = 1;
        tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel3.Size = new System.Drawing.Size(344, 49);
        tableLayoutPanel3.TabIndex = 1;
        // 
        // LnkGitHub
        // 
        LnkGitHub.Dock = System.Windows.Forms.DockStyle.Fill;
        LnkGitHub.Location = new System.Drawing.Point(13, 10);
        LnkGitHub.Name = "LnkGitHub";
        LnkGitHub.Size = new System.Drawing.Size(45, 29);
        LnkGitHub.TabIndex = 2;
        LnkGitHub.TabStop = true;
        LnkGitHub.Text = "GitHub";
        LnkGitHub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // LnkForum
        // 
        LnkForum.AutoSize = true;
        LnkForum.Dock = System.Windows.Forms.DockStyle.Fill;
        LnkForum.Location = new System.Drawing.Point(64, 10);
        LnkForum.Name = "LnkForum";
        LnkForum.Size = new System.Drawing.Size(42, 29);
        LnkForum.TabIndex = 3;
        LnkForum.TabStop = true;
        LnkForum.Text = "Forum";
        LnkForum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // BtnOk
        // 
        BtnOk.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        BtnOk.Location = new System.Drawing.Point(175, 13);
        BtnOk.Name = "BtnOk";
        BtnOk.Size = new System.Drawing.Size(75, 23);
        BtnOk.TabIndex = 0;
        BtnOk.Text = "OK";
        BtnOk.UseVisualStyleBackColor = true;
        // 
        // BtnCancel
        // 
        BtnCancel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        BtnCancel.Location = new System.Drawing.Point(256, 13);
        BtnCancel.Name = "BtnCancel";
        BtnCancel.Size = new System.Drawing.Size(75, 23);
        BtnCancel.TabIndex = 1;
        BtnCancel.Text = "Cancel";
        BtnCancel.UseVisualStyleBackColor = true;
        // 
        // GbLayers
        // 
        GbLayers.AutoSize = true;
        GbLayers.Controls.Add(tableLayoutPanel1);
        GbLayers.Dock = System.Windows.Forms.DockStyle.Top;
        GbLayers.Location = new System.Drawing.Point(10, 253);
        GbLayers.Name = "GbLayers";
        GbLayers.Size = new System.Drawing.Size(344, 97);
        GbLayers.TabIndex = 2;
        GbLayers.TabStop = false;
        GbLayers.Text = "Layers";
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.AutoSize = true;
        tableLayoutPanel1.ColumnCount = 2;
        tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel1.Controls.Add(RbFlatten, 0, 0);
        tableLayoutPanel1.Controls.Add(CbGroupBoundaries, 1, 2);
        tableLayoutPanel1.Controls.Add(RbGroups, 0, 1);
        tableLayoutPanel1.Controls.Add(CbHiddenLayers, 1, 1);
        tableLayoutPanel1.Controls.Add(RbAllElements, 0, 2);
        tableLayoutPanel1.Controls.Add(CbOpacity, 1, 0);
        tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        tableLayoutPanel1.Location = new System.Drawing.Point(3, 19);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 3;
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel1.Size = new System.Drawing.Size(338, 75);
        tableLayoutPanel1.TabIndex = 0;
        // 
        // RbFlatten
        // 
        RbFlatten.AutoSize = true;
        RbFlatten.Checked = true;
        RbFlatten.Dock = System.Windows.Forms.DockStyle.Fill;
        RbFlatten.Location = new System.Drawing.Point(3, 3);
        RbFlatten.Name = "RbFlatten";
        RbFlatten.Size = new System.Drawing.Size(90, 19);
        RbFlatten.TabIndex = 3;
        RbFlatten.TabStop = true;
        RbFlatten.Text = "Flatten";
        RbFlatten.UseVisualStyleBackColor = true;
        // 
        // RbGroups
        // 
        RbGroups.AutoSize = true;
        RbGroups.Dock = System.Windows.Forms.DockStyle.Fill;
        RbGroups.Location = new System.Drawing.Point(3, 28);
        RbGroups.Name = "RbGroups";
        RbGroups.Size = new System.Drawing.Size(90, 19);
        RbGroups.TabIndex = 4;
        RbGroups.Text = "Groups";
        RbGroups.UseVisualStyleBackColor = true;
        // 
        // CbHiddenLayers
        // 
        CbHiddenLayers.AutoSize = true;
        CbHiddenLayers.Checked = true;
        CbHiddenLayers.CheckState = System.Windows.Forms.CheckState.Checked;
        CbHiddenLayers.Dock = System.Windows.Forms.DockStyle.Fill;
        CbHiddenLayers.Enabled = false;
        CbHiddenLayers.Location = new System.Drawing.Point(99, 28);
        CbHiddenLayers.Name = "CbHiddenLayers";
        CbHiddenLayers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        CbHiddenLayers.Size = new System.Drawing.Size(236, 19);
        CbHiddenLayers.TabIndex = 1;
        CbHiddenLayers.Text = "Hidden layers";
        CbHiddenLayers.UseVisualStyleBackColor = true;
        // 
        // RbAllElements
        // 
        RbAllElements.AutoSize = true;
        RbAllElements.Dock = System.Windows.Forms.DockStyle.Fill;
        RbAllElements.Location = new System.Drawing.Point(3, 53);
        RbAllElements.Name = "RbAllElements";
        RbAllElements.Size = new System.Drawing.Size(90, 19);
        RbAllElements.TabIndex = 5;
        RbAllElements.Text = "All Elements";
        RbAllElements.UseVisualStyleBackColor = true;
        // 
        // CbOpacity
        // 
        CbOpacity.AutoSize = true;
        CbOpacity.Checked = true;
        CbOpacity.CheckState = System.Windows.Forms.CheckState.Checked;
        CbOpacity.Dock = System.Windows.Forms.DockStyle.Fill;
        CbOpacity.Enabled = false;
        CbOpacity.Location = new System.Drawing.Point(99, 3);
        CbOpacity.Name = "CbOpacity";
        CbOpacity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        CbOpacity.Size = new System.Drawing.Size(236, 19);
        CbOpacity.TabIndex = 0;
        CbOpacity.Text = "Opacity as layer property";
        CbOpacity.UseVisualStyleBackColor = true;
        // 
        // GbSizeSelection
        // 
        GbSizeSelection.AutoSize = true;
        GbSizeSelection.Controls.Add(CbKeepAR);
        GbSizeSelection.Controls.Add(tableLayoutPanel4);
        GbSizeSelection.Dock = System.Windows.Forms.DockStyle.Top;
        GbSizeSelection.Location = new System.Drawing.Point(10, 154);
        GbSizeSelection.Name = "GbSizeSelection";
        GbSizeSelection.Size = new System.Drawing.Size(344, 99);
        GbSizeSelection.TabIndex = 3;
        GbSizeSelection.TabStop = false;
        GbSizeSelection.Text = "Size selection by user";
        // 
        // CbKeepAR
        // 
        CbKeepAR.AutoSize = true;
        CbKeepAR.Checked = true;
        CbKeepAR.CheckState = System.Windows.Forms.CheckState.Checked;
        CbKeepAR.Dock = System.Windows.Forms.DockStyle.Top;
        CbKeepAR.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
        CbKeepAR.Location = new System.Drawing.Point(3, 77);
        CbKeepAR.Name = "CbKeepAR";
        CbKeepAR.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        CbKeepAR.Size = new System.Drawing.Size(338, 19);
        CbKeepAR.TabIndex = 0;
        CbKeepAR.Text = "Keep aspect ratio";
        CbKeepAR.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel4
        // 
        tableLayoutPanel4.AutoSize = true;
        tableLayoutPanel4.ColumnCount = 7;
        tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
        tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableLayoutPanel4.Controls.Add(LblResolution, 2, 1);
        tableLayoutPanel4.Controls.Add(LblCanvasWH, 2, 0);
        tableLayoutPanel4.Controls.Add(PbWarning, 1, 0);
        tableLayoutPanel4.Controls.Add(NudCanvasW, 3, 0);
        tableLayoutPanel4.Controls.Add(NudCanvasH, 5, 0);
        tableLayoutPanel4.Controls.Add(label9, 4, 0);
        tableLayoutPanel4.Controls.Add(NudDpi, 3, 1);
        tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
        tableLayoutPanel4.Location = new System.Drawing.Point(3, 19);
        tableLayoutPanel4.Name = "tableLayoutPanel4";
        tableLayoutPanel4.RowCount = 2;
        tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel4.Size = new System.Drawing.Size(338, 58);
        tableLayoutPanel4.TabIndex = 1;
        // 
        // LblResolution
        // 
        LblResolution.AutoSize = true;
        LblResolution.Dock = System.Windows.Forms.DockStyle.Fill;
        LblResolution.Location = new System.Drawing.Point(68, 29);
        LblResolution.Name = "LblResolution";
        LblResolution.Size = new System.Drawing.Size(92, 29);
        LblResolution.TabIndex = 0;
        LblResolution.Text = "Resolution (DPI)";
        LblResolution.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // LblCanvasWH
        // 
        LblCanvasWH.AutoSize = true;
        LblCanvasWH.Dock = System.Windows.Forms.DockStyle.Fill;
        LblCanvasWH.Location = new System.Drawing.Point(68, 0);
        LblCanvasWH.Name = "LblCanvasWH";
        LblCanvasWH.Size = new System.Drawing.Size(92, 29);
        LblCanvasWH.TabIndex = 1;
        LblCanvasWH.Text = "Canvas (W×H)";
        LblCanvasWH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // NudCanvasW
        // 
        NudCanvasW.Dock = System.Windows.Forms.DockStyle.Fill;
        NudCanvasW.Location = new System.Drawing.Point(166, 3);
        NudCanvasW.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        NudCanvasW.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        NudCanvasW.MinimumSize = new System.Drawing.Size(50, 0);
        NudCanvasW.Name = "NudCanvasW";
        NudCanvasW.Size = new System.Drawing.Size(50, 23);
        NudCanvasW.TabIndex = 0;
        NudCanvasW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        NudCanvasW.Value = new decimal(new int[] { 512, 0, 0, 0 });
        // 
        // NudCanvasH
        // 
        NudCanvasH.Dock = System.Windows.Forms.DockStyle.Fill;
        NudCanvasH.Location = new System.Drawing.Point(243, 3);
        NudCanvasH.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        NudCanvasH.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        NudCanvasH.MinimumSize = new System.Drawing.Size(50, 0);
        NudCanvasH.Name = "NudCanvasH";
        NudCanvasH.Size = new System.Drawing.Size(50, 23);
        NudCanvasH.TabIndex = 1;
        NudCanvasH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        NudCanvasH.Value = new decimal(new int[] { 512, 0, 0, 0 });
        // 
        // label9
        // 
        label9.AutoSize = true;
        label9.Dock = System.Windows.Forms.DockStyle.Fill;
        label9.Location = new System.Drawing.Point(222, 0);
        label9.Name = "label9";
        label9.Size = new System.Drawing.Size(15, 29);
        label9.TabIndex = 5;
        label9.Text = "×";
        label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // NudDpi
        // 
        NudDpi.Dock = System.Windows.Forms.DockStyle.Fill;
        NudDpi.Location = new System.Drawing.Point(166, 32);
        NudDpi.Maximum = new decimal(new int[] { 32767, 0, 0, 0 });
        NudDpi.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        NudDpi.MinimumSize = new System.Drawing.Size(50, 0);
        NudDpi.Name = "NudDpi";
        NudDpi.Size = new System.Drawing.Size(50, 23);
        NudDpi.TabIndex = 2;
        NudDpi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        NudDpi.Value = new decimal(new int[] { 96, 0, 0, 0 });
        // 
        // GbInfo
        // 
        GbInfo.AutoSize = true;
        GbInfo.Controls.Add(tableLayoutPanel9);
        GbInfo.Controls.Add(tableLayoutPanel2);
        GbInfo.Dock = System.Windows.Forms.DockStyle.Top;
        GbInfo.Location = new System.Drawing.Point(10, 10);
        GbInfo.Name = "GbInfo";
        GbInfo.Size = new System.Drawing.Size(344, 144);
        GbInfo.TabIndex = 4;
        GbInfo.TabStop = false;
        GbInfo.Text = "Size settings given in SVG file";
        // 
        // tableLayoutPanel9
        // 
        tableLayoutPanel9.AutoSize = true;
        tableLayoutPanel9.ColumnCount = 3;
        tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableLayoutPanel9.Controls.Add(LnkUseSvgSettings, 1, 0);
        tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Top;
        tableLayoutPanel9.Location = new System.Drawing.Point(3, 106);
        tableLayoutPanel9.Name = "tableLayoutPanel9";
        tableLayoutPanel9.RowCount = 1;
        tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel9.Size = new System.Drawing.Size(338, 35);
        tableLayoutPanel9.TabIndex = 0;
        // 
        // LnkUseSvgSettings
        // 
        LnkUseSvgSettings.AutoSize = true;
        LnkUseSvgSettings.Dock = System.Windows.Forms.DockStyle.Top;
        LnkUseSvgSettings.Location = new System.Drawing.Point(70, 5);
        LnkUseSvgSettings.Margin = new System.Windows.Forms.Padding(5);
        LnkUseSvgSettings.Name = "LnkUseSvgSettings";
        LnkUseSvgSettings.Size = new System.Drawing.Size(197, 25);
        LnkUseSvgSettings.TabIndex = 0;
        LnkUseSvgSettings.Text = "▼ Use size settings given in SVG ▼";
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.AutoSize = true;
        tableLayoutPanel2.ColumnCount = 6;
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        tableLayoutPanel2.Controls.Add(LblViewport, 1, 0);
        tableLayoutPanel2.Controls.Add(TbViewboxH, 4, 2);
        tableLayoutPanel2.Controls.Add(label6, 3, 2);
        tableLayoutPanel2.Controls.Add(TbViewportW, 2, 0);
        tableLayoutPanel2.Controls.Add(label3, 3, 0);
        tableLayoutPanel2.Controls.Add(TbViewboxW, 2, 2);
        tableLayoutPanel2.Controls.Add(TbViewportH, 4, 0);
        tableLayoutPanel2.Controls.Add(LblViewboxWH, 1, 2);
        tableLayoutPanel2.Controls.Add(LblViewboxXY, 1, 1);
        tableLayoutPanel2.Controls.Add(TbViewboxY, 4, 1);
        tableLayoutPanel2.Controls.Add(label4, 3, 1);
        tableLayoutPanel2.Controls.Add(TbViewboxX, 2, 1);
        tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
        tableLayoutPanel2.Location = new System.Drawing.Point(3, 19);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 3;
        tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
        tableLayoutPanel2.Size = new System.Drawing.Size(338, 87);
        tableLayoutPanel2.TabIndex = 1;
        // 
        // LblViewport
        // 
        LblViewport.AutoSize = true;
        LblViewport.Dock = System.Windows.Forms.DockStyle.Fill;
        LblViewport.Location = new System.Drawing.Point(50, 0);
        LblViewport.Name = "LblViewport";
        LblViewport.Size = new System.Drawing.Size(93, 29);
        LblViewport.TabIndex = 0;
        LblViewport.Text = "Viewport (W×H)";
        LblViewport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // TbViewboxH
        // 
        TbViewboxH.Dock = System.Windows.Forms.DockStyle.Fill;
        TbViewboxH.Enabled = false;
        TbViewboxH.Location = new System.Drawing.Point(232, 61);
        TbViewboxH.MinimumSize = new System.Drawing.Size(50, 4);
        TbViewboxH.Name = "TbViewboxH";
        TbViewboxH.Size = new System.Drawing.Size(56, 23);
        TbViewboxH.TabIndex = 5;
        TbViewboxH.TabStop = false;
        TbViewboxH.Text = "-";
        TbViewboxH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // label6
        // 
        label6.AutoSize = true;
        label6.Dock = System.Windows.Forms.DockStyle.Fill;
        label6.Location = new System.Drawing.Point(211, 58);
        label6.Name = "label6";
        label6.Size = new System.Drawing.Size(15, 29);
        label6.TabIndex = 2;
        label6.Text = "×";
        label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // TbViewportW
        // 
        TbViewportW.Dock = System.Windows.Forms.DockStyle.Fill;
        TbViewportW.Enabled = false;
        TbViewportW.Location = new System.Drawing.Point(149, 3);
        TbViewportW.MinimumSize = new System.Drawing.Size(50, 4);
        TbViewportW.Name = "TbViewportW";
        TbViewportW.Size = new System.Drawing.Size(56, 23);
        TbViewportW.TabIndex = 0;
        TbViewportW.TabStop = false;
        TbViewportW.Text = "-";
        TbViewportW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // label3
        // 
        label3.AutoSize = true;
        label3.Dock = System.Windows.Forms.DockStyle.Fill;
        label3.Location = new System.Drawing.Point(211, 0);
        label3.Name = "label3";
        label3.Size = new System.Drawing.Size(15, 29);
        label3.TabIndex = 4;
        label3.Text = "×";
        label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // TbViewboxW
        // 
        TbViewboxW.Dock = System.Windows.Forms.DockStyle.Fill;
        TbViewboxW.Enabled = false;
        TbViewboxW.Location = new System.Drawing.Point(149, 61);
        TbViewboxW.MinimumSize = new System.Drawing.Size(50, 4);
        TbViewboxW.Name = "TbViewboxW";
        TbViewboxW.Size = new System.Drawing.Size(56, 23);
        TbViewboxW.TabIndex = 4;
        TbViewboxW.TabStop = false;
        TbViewboxW.Text = "-";
        TbViewboxW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // TbViewportH
        // 
        TbViewportH.Dock = System.Windows.Forms.DockStyle.Fill;
        TbViewportH.Enabled = false;
        TbViewportH.Location = new System.Drawing.Point(232, 3);
        TbViewportH.MinimumSize = new System.Drawing.Size(50, 4);
        TbViewportH.Name = "TbViewportH";
        TbViewportH.Size = new System.Drawing.Size(56, 23);
        TbViewportH.TabIndex = 1;
        TbViewportH.TabStop = false;
        TbViewportH.Text = "-";
        TbViewportH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // LblViewboxWH
        // 
        LblViewboxWH.AutoSize = true;
        LblViewboxWH.Dock = System.Windows.Forms.DockStyle.Fill;
        LblViewboxWH.Location = new System.Drawing.Point(50, 58);
        LblViewboxWH.Name = "LblViewboxWH";
        LblViewboxWH.Size = new System.Drawing.Size(93, 29);
        LblViewboxWH.TabIndex = 7;
        LblViewboxWH.Text = "ViewBox (W×H)";
        LblViewboxWH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // LblViewboxXY
        // 
        LblViewboxXY.AutoSize = true;
        LblViewboxXY.Dock = System.Windows.Forms.DockStyle.Fill;
        LblViewboxXY.Location = new System.Drawing.Point(50, 29);
        LblViewboxXY.Name = "LblViewboxXY";
        LblViewboxXY.Size = new System.Drawing.Size(93, 29);
        LblViewboxXY.TabIndex = 8;
        LblViewboxXY.Text = "ViewBox (X,Y)";
        LblViewboxXY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // TbViewboxY
        // 
        TbViewboxY.Dock = System.Windows.Forms.DockStyle.Fill;
        TbViewboxY.Enabled = false;
        TbViewboxY.Location = new System.Drawing.Point(232, 32);
        TbViewboxY.MinimumSize = new System.Drawing.Size(50, 4);
        TbViewboxY.Name = "TbViewboxY";
        TbViewboxY.Size = new System.Drawing.Size(56, 23);
        TbViewboxY.TabIndex = 3;
        TbViewboxY.TabStop = false;
        TbViewboxY.Text = "-";
        TbViewboxY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // label4
        // 
        label4.AutoSize = true;
        label4.Dock = System.Windows.Forms.DockStyle.Fill;
        label4.Location = new System.Drawing.Point(211, 29);
        label4.Name = "label4";
        label4.Size = new System.Drawing.Size(15, 29);
        label4.TabIndex = 10;
        label4.Text = ",";
        label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // TbViewboxX
        // 
        TbViewboxX.Dock = System.Windows.Forms.DockStyle.Fill;
        TbViewboxX.Enabled = false;
        TbViewboxX.Location = new System.Drawing.Point(149, 32);
        TbViewboxX.MinimumSize = new System.Drawing.Size(50, 4);
        TbViewboxX.Name = "TbViewboxX";
        TbViewboxX.Size = new System.Drawing.Size(56, 23);
        TbViewboxX.TabIndex = 2;
        TbViewboxX.TabStop = false;
        TbViewboxX.Text = "-";
        TbViewboxX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // SvgImportDialog
        // 
        AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        AutoSize = true;
        ClientSize = new System.Drawing.Size(364, 341);
        Controls.Add(RootPanel);
        Name = "SvgImportDialog";
        ((System.ComponentModel.ISupportInitialize)PbWarning).EndInit();
        RootPanel.ResumeLayout(false);
        RootPanel.PerformLayout();
        StatusStrip.ResumeLayout(false);
        StatusStrip.PerformLayout();
        tableLayoutPanel3.ResumeLayout(false);
        tableLayoutPanel3.PerformLayout();
        GbLayers.ResumeLayout(false);
        GbLayers.PerformLayout();
        tableLayoutPanel1.ResumeLayout(false);
        tableLayoutPanel1.PerformLayout();
        GbSizeSelection.ResumeLayout(false);
        GbSizeSelection.PerformLayout();
        tableLayoutPanel4.ResumeLayout(false);
        tableLayoutPanel4.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)NudCanvasW).EndInit();
        ((System.ComponentModel.ISupportInitialize)NudCanvasH).EndInit();
        ((System.ComponentModel.ISupportInitialize)NudDpi).EndInit();
        GbInfo.ResumeLayout(false);
        GbInfo.PerformLayout();
        tableLayoutPanel9.ResumeLayout(false);
        tableLayoutPanel9.PerformLayout();
        tableLayoutPanel2.ResumeLayout(false);
        tableLayoutPanel2.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private System.Windows.Forms.ToolTip ToolTip1;
    private System.Windows.Forms.Panel RootPanel;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
    private System.Windows.Forms.LinkLabel LnkGitHub;
    private System.Windows.Forms.LinkLabel LnkForum;
    private System.Windows.Forms.Button BtnOk;
    private System.Windows.Forms.Button BtnCancel;
    private System.Windows.Forms.GroupBox GbLayers;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.RadioButton RbFlatten;
    private System.Windows.Forms.CheckBox CbGroupBoundaries;
    private System.Windows.Forms.RadioButton RbGroups;
    private System.Windows.Forms.CheckBox CbHiddenLayers;
    private System.Windows.Forms.RadioButton RbAllElements;
    private System.Windows.Forms.CheckBox CbOpacity;
    private System.Windows.Forms.GroupBox GbSizeSelection;
    private System.Windows.Forms.CheckBox CbKeepAR;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
    private System.Windows.Forms.Label LblResolution;
    private System.Windows.Forms.Label LblCanvasWH;
    private System.Windows.Forms.PictureBox PbWarning;
    private System.Windows.Forms.NumericUpDown NudCanvasW;
    private System.Windows.Forms.NumericUpDown NudCanvasH;
    private System.Windows.Forms.Label label9;
    private System.Windows.Forms.NumericUpDown NudDpi;
    private System.Windows.Forms.GroupBox GbInfo;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Label LblViewport;
    private System.Windows.Forms.TextBox TbViewboxH;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox TbViewportW;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox TbViewboxW;
    private System.Windows.Forms.TextBox TbViewportH;
    private System.Windows.Forms.Label LblViewboxWH;
    private System.Windows.Forms.Label LblViewboxXY;
    private System.Windows.Forms.TextBox TbViewboxY;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.TextBox TbViewboxX;
    private System.Windows.Forms.StatusStrip StatusStrip;
    private System.Windows.Forms.ToolStripStatusLabel ProgressLabel;
    private System.Windows.Forms.ToolStripProgressBar ProgressBar;
    private System.Windows.Forms.ToolStripStatusLabel PlaceHolderLabel;
    private System.Windows.Forms.ToolStripStatusLabel UpdateAvailLabel;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
    private System.Windows.Forms.Button LnkUseSvgSettings;
}
