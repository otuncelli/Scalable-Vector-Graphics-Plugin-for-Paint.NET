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
        this.components = new System.ComponentModel.Container();
        this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
        this.PbWarning = new System.Windows.Forms.PictureBox();
        this.CbGroupBoundaries = new System.Windows.Forms.CheckBox();
        this.RootPanel = new System.Windows.Forms.Panel();
        this.StatusStrip = new System.Windows.Forms.StatusStrip();
        this.ProgressLabel = new System.Windows.Forms.ToolStripStatusLabel();
        this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
        this.PlaceHolderLabel = new System.Windows.Forms.ToolStripStatusLabel();
        this.UpdateAvailLabel = new System.Windows.Forms.ToolStripStatusLabel();
        this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
        this.LnkGitHub = new System.Windows.Forms.LinkLabel();
        this.LnkForum = new System.Windows.Forms.LinkLabel();
        this.BtnOk = new System.Windows.Forms.Button();
        this.BtnCancel = new System.Windows.Forms.Button();
        this.GbLayers = new System.Windows.Forms.GroupBox();
        this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
        this.RbFlatten = new System.Windows.Forms.RadioButton();
        this.RbGroups = new System.Windows.Forms.RadioButton();
        this.CbHiddenLayers = new System.Windows.Forms.CheckBox();
        this.RbAllElements = new System.Windows.Forms.RadioButton();
        this.CbOpacity = new System.Windows.Forms.CheckBox();
        this.GbSizeSelection = new System.Windows.Forms.GroupBox();
        this.CbKeepAR = new System.Windows.Forms.CheckBox();
        this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
        this.LblResolution = new System.Windows.Forms.Label();
        this.LblCanvasWH = new System.Windows.Forms.Label();
        this.NudCanvasW = new System.Windows.Forms.NumericUpDown();
        this.NudCanvasH = new System.Windows.Forms.NumericUpDown();
        this.label9 = new System.Windows.Forms.Label();
        this.NudDpi = new System.Windows.Forms.NumericUpDown();
        this.GbInfo = new System.Windows.Forms.GroupBox();
        this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
        this.LnkUseSvgSettings = new System.Windows.Forms.Button();
        this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
        this.LblViewport = new System.Windows.Forms.Label();
        this.TbViewboxH = new System.Windows.Forms.TextBox();
        this.label6 = new System.Windows.Forms.Label();
        this.TbViewportW = new System.Windows.Forms.TextBox();
        this.label3 = new System.Windows.Forms.Label();
        this.TbViewboxW = new System.Windows.Forms.TextBox();
        this.TbViewportH = new System.Windows.Forms.TextBox();
        this.LblViewboxWH = new System.Windows.Forms.Label();
        this.LblViewboxXY = new System.Windows.Forms.Label();
        this.TbViewboxY = new System.Windows.Forms.TextBox();
        this.label4 = new System.Windows.Forms.Label();
        this.TbViewboxX = new System.Windows.Forms.TextBox();
        ((System.ComponentModel.ISupportInitialize)(this.PbWarning)).BeginInit();
        this.RootPanel.SuspendLayout();
        this.StatusStrip.SuspendLayout();
        this.tableLayoutPanel3.SuspendLayout();
        this.GbLayers.SuspendLayout();
        this.tableLayoutPanel1.SuspendLayout();
        this.GbSizeSelection.SuspendLayout();
        this.tableLayoutPanel4.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)(this.NudCanvasW)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.NudCanvasH)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.NudDpi)).BeginInit();
        this.GbInfo.SuspendLayout();
        this.tableLayoutPanel9.SuspendLayout();
        this.tableLayoutPanel2.SuspendLayout();
        this.SuspendLayout();
        // 
        // ToolTip1
        // 
        this.ToolTip1.AutomaticDelay = 0;
        this.ToolTip1.AutoPopDelay = 0;
        this.ToolTip1.InitialDelay = 0;
        this.ToolTip1.ReshowDelay = 0;
        this.ToolTip1.ShowAlways = true;
        this.ToolTip1.UseAnimation = false;
        // 
        // PbWarning
        // 
        this.PbWarning.Dock = System.Windows.Forms.DockStyle.Fill;
        this.PbWarning.Location = new System.Drawing.Point(42, 0);
        this.PbWarning.Margin = new System.Windows.Forms.Padding(0);
        this.PbWarning.MinimumSize = new System.Drawing.Size(23, 23);
        this.PbWarning.Name = "PbWarning";
        this.PbWarning.Size = new System.Drawing.Size(23, 29);
        this.PbWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
        this.PbWarning.TabIndex = 2;
        this.PbWarning.TabStop = false;
        this.ToolTip1.SetToolTip(this.PbWarning, "Please make sure you\'ve enough memory, especially if you\'re importing with layers" +
    ".");
        this.PbWarning.Visible = false;
        // 
        // CbGroupBoundaries
        // 
        this.CbGroupBoundaries.AutoSize = true;
        this.CbGroupBoundaries.Dock = System.Windows.Forms.DockStyle.Fill;
        this.CbGroupBoundaries.Enabled = false;
        this.CbGroupBoundaries.Location = new System.Drawing.Point(99, 53);
        this.CbGroupBoundaries.Name = "CbGroupBoundaries";
        this.CbGroupBoundaries.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        this.CbGroupBoundaries.Size = new System.Drawing.Size(236, 19);
        this.CbGroupBoundaries.TabIndex = 2;
        this.CbGroupBoundaries.Text = "Group boundaries";
        this.ToolTip1.SetToolTip(this.CbGroupBoundaries, "Imports group boundaries as empty layers.");
        this.CbGroupBoundaries.UseVisualStyleBackColor = true;
        // 
        // RootPanel
        // 
        this.RootPanel.AutoSize = true;
        this.RootPanel.Controls.Add(this.StatusStrip);
        this.RootPanel.Controls.Add(this.tableLayoutPanel3);
        this.RootPanel.Controls.Add(this.GbLayers);
        this.RootPanel.Controls.Add(this.GbSizeSelection);
        this.RootPanel.Controls.Add(this.GbInfo);
        this.RootPanel.Dock = System.Windows.Forms.DockStyle.Top;
        this.RootPanel.Location = new System.Drawing.Point(0, 0);
        this.RootPanel.Name = "RootPanel";
        this.RootPanel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
        this.RootPanel.Size = new System.Drawing.Size(364, 421);
        this.RootPanel.TabIndex = 0;
        // 
        // StatusStrip
        // 
        this.StatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
        this.ProgressLabel,
        this.ProgressBar,
        this.PlaceHolderLabel,
        this.UpdateAvailLabel});
        this.StatusStrip.Location = new System.Drawing.Point(10, 399);
        this.StatusStrip.Name = "StatusStrip";
        this.StatusStrip.Size = new System.Drawing.Size(344, 22);
        this.StatusStrip.SizingGrip = false;
        this.StatusStrip.TabIndex = 0;
        this.StatusStrip.Text = "statusStrip1";
        // 
        // ProgressLabel
        // 
        this.ProgressLabel.BackColor = System.Drawing.Color.Transparent;
        this.ProgressLabel.Name = "ProgressLabel";
        this.ProgressLabel.Size = new System.Drawing.Size(42, 17);
        this.ProgressLabel.Text = "Ready!";
        // 
        // ProgressBar
        // 
        this.ProgressBar.Name = "ProgressBar";
        this.ProgressBar.Size = new System.Drawing.Size(100, 16);
        this.ProgressBar.Visible = false;
        // 
        // PlaceHolderLabel
        // 
        this.PlaceHolderLabel.BackColor = System.Drawing.Color.Transparent;
        this.PlaceHolderLabel.Name = "PlaceHolderLabel";
        this.PlaceHolderLabel.Size = new System.Drawing.Size(287, 17);
        this.PlaceHolderLabel.Spring = true;
        // 
        // UpdateAvailLabel
        // 
        this.UpdateAvailLabel.BackColor = System.Drawing.Color.Transparent;
        this.UpdateAvailLabel.ForeColor = System.Drawing.Color.Red;
        this.UpdateAvailLabel.Name = "UpdateAvailLabel";
        this.UpdateAvailLabel.Size = new System.Drawing.Size(0, 17);
        // 
        // tableLayoutPanel3
        // 
        this.tableLayoutPanel3.AutoSize = true;
        this.tableLayoutPanel3.ColumnCount = 5;
        this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
        this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel3.Controls.Add(this.LnkGitHub, 0, 0);
        this.tableLayoutPanel3.Controls.Add(this.LnkForum, 1, 0);
        this.tableLayoutPanel3.Controls.Add(this.BtnOk, 3, 0);
        this.tableLayoutPanel3.Controls.Add(this.BtnCancel, 4, 0);
        this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Top;
        this.tableLayoutPanel3.Location = new System.Drawing.Point(10, 350);
        this.tableLayoutPanel3.Name = "tableLayoutPanel3";
        this.tableLayoutPanel3.Padding = new System.Windows.Forms.Padding(10);
        this.tableLayoutPanel3.RowCount = 1;
        this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel3.Size = new System.Drawing.Size(344, 49);
        this.tableLayoutPanel3.TabIndex = 1;
        // 
        // LnkGitHub
        // 
        this.LnkGitHub.Dock = System.Windows.Forms.DockStyle.Fill;
        this.LnkGitHub.Location = new System.Drawing.Point(13, 10);
        this.LnkGitHub.Name = "LnkGitHub";
        this.LnkGitHub.Size = new System.Drawing.Size(45, 29);
        this.LnkGitHub.TabIndex = 2;
        this.LnkGitHub.TabStop = true;
        this.LnkGitHub.Text = "GitHub";
        this.LnkGitHub.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // LnkForum
        // 
        this.LnkForum.AutoSize = true;
        this.LnkForum.Dock = System.Windows.Forms.DockStyle.Fill;
        this.LnkForum.Location = new System.Drawing.Point(64, 10);
        this.LnkForum.Name = "LnkForum";
        this.LnkForum.Size = new System.Drawing.Size(42, 29);
        this.LnkForum.TabIndex = 3;
        this.LnkForum.TabStop = true;
        this.LnkForum.Text = "Forum";
        this.LnkForum.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // BtnOk
        // 
        this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.BtnOk.Location = new System.Drawing.Point(175, 13);
        this.BtnOk.Name = "BtnOk";
        this.BtnOk.Size = new System.Drawing.Size(75, 23);
        this.BtnOk.TabIndex = 0;
        this.BtnOk.Text = "OK";
        this.BtnOk.UseVisualStyleBackColor = true;
        // 
        // BtnCancel
        // 
        this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        | System.Windows.Forms.AnchorStyles.Right)));
        this.BtnCancel.Location = new System.Drawing.Point(256, 13);
        this.BtnCancel.Name = "BtnCancel";
        this.BtnCancel.Size = new System.Drawing.Size(75, 23);
        this.BtnCancel.TabIndex = 1;
        this.BtnCancel.Text = "Cancel";
        this.BtnCancel.UseVisualStyleBackColor = true;
        // 
        // GbLayers
        // 
        this.GbLayers.AutoSize = true;
        this.GbLayers.Controls.Add(this.tableLayoutPanel1);
        this.GbLayers.Dock = System.Windows.Forms.DockStyle.Top;
        this.GbLayers.Location = new System.Drawing.Point(10, 253);
        this.GbLayers.Name = "GbLayers";
        this.GbLayers.Size = new System.Drawing.Size(344, 97);
        this.GbLayers.TabIndex = 2;
        this.GbLayers.TabStop = false;
        this.GbLayers.Text = "Layers";
        // 
        // tableLayoutPanel1
        // 
        this.tableLayoutPanel1.AutoSize = true;
        this.tableLayoutPanel1.ColumnCount = 2;
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel1.Controls.Add(this.RbFlatten, 0, 0);
        this.tableLayoutPanel1.Controls.Add(this.CbGroupBoundaries, 1, 2);
        this.tableLayoutPanel1.Controls.Add(this.RbGroups, 0, 1);
        this.tableLayoutPanel1.Controls.Add(this.CbHiddenLayers, 1, 1);
        this.tableLayoutPanel1.Controls.Add(this.RbAllElements, 0, 2);
        this.tableLayoutPanel1.Controls.Add(this.CbOpacity, 1, 0);
        this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 19);
        this.tableLayoutPanel1.Name = "tableLayoutPanel1";
        this.tableLayoutPanel1.RowCount = 3;
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel1.Size = new System.Drawing.Size(338, 75);
        this.tableLayoutPanel1.TabIndex = 0;
        // 
        // RbFlatten
        // 
        this.RbFlatten.AutoSize = true;
        this.RbFlatten.Checked = true;
        this.RbFlatten.Dock = System.Windows.Forms.DockStyle.Fill;
        this.RbFlatten.Location = new System.Drawing.Point(3, 3);
        this.RbFlatten.Name = "RbFlatten";
        this.RbFlatten.Size = new System.Drawing.Size(90, 19);
        this.RbFlatten.TabIndex = 3;
        this.RbFlatten.TabStop = true;
        this.RbFlatten.Text = "Flatten";
        this.RbFlatten.UseVisualStyleBackColor = true;
        // 
        // RbGroups
        // 
        this.RbGroups.AutoSize = true;
        this.RbGroups.Dock = System.Windows.Forms.DockStyle.Fill;
        this.RbGroups.Location = new System.Drawing.Point(3, 28);
        this.RbGroups.Name = "RbGroups";
        this.RbGroups.Size = new System.Drawing.Size(90, 19);
        this.RbGroups.TabIndex = 4;
        this.RbGroups.Text = "Groups";
        this.RbGroups.UseVisualStyleBackColor = true;
        // 
        // CbHiddenLayers
        // 
        this.CbHiddenLayers.AutoSize = true;
        this.CbHiddenLayers.Checked = true;
        this.CbHiddenLayers.CheckState = System.Windows.Forms.CheckState.Checked;
        this.CbHiddenLayers.Dock = System.Windows.Forms.DockStyle.Fill;
        this.CbHiddenLayers.Enabled = false;
        this.CbHiddenLayers.Location = new System.Drawing.Point(99, 28);
        this.CbHiddenLayers.Name = "CbHiddenLayers";
        this.CbHiddenLayers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        this.CbHiddenLayers.Size = new System.Drawing.Size(236, 19);
        this.CbHiddenLayers.TabIndex = 1;
        this.CbHiddenLayers.Text = "Hidden layers";
        this.CbHiddenLayers.UseVisualStyleBackColor = true;
        // 
        // RbAllElements
        // 
        this.RbAllElements.AutoSize = true;
        this.RbAllElements.Dock = System.Windows.Forms.DockStyle.Fill;
        this.RbAllElements.Location = new System.Drawing.Point(3, 53);
        this.RbAllElements.Name = "RbAllElements";
        this.RbAllElements.Size = new System.Drawing.Size(90, 19);
        this.RbAllElements.TabIndex = 5;
        this.RbAllElements.Text = "All Elements";
        this.RbAllElements.UseVisualStyleBackColor = true;
        // 
        // CbOpacity
        // 
        this.CbOpacity.AutoSize = true;
        this.CbOpacity.Checked = true;
        this.CbOpacity.CheckState = System.Windows.Forms.CheckState.Checked;
        this.CbOpacity.Dock = System.Windows.Forms.DockStyle.Fill;
        this.CbOpacity.Enabled = false;
        this.CbOpacity.Location = new System.Drawing.Point(99, 3);
        this.CbOpacity.Name = "CbOpacity";
        this.CbOpacity.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        this.CbOpacity.Size = new System.Drawing.Size(236, 19);
        this.CbOpacity.TabIndex = 0;
        this.CbOpacity.Text = "Opacity as layer property";
        this.CbOpacity.UseVisualStyleBackColor = true;
        // 
        // GbSizeSelection
        // 
        this.GbSizeSelection.AutoSize = true;
        this.GbSizeSelection.Controls.Add(this.CbKeepAR);
        this.GbSizeSelection.Controls.Add(this.tableLayoutPanel4);
        this.GbSizeSelection.Dock = System.Windows.Forms.DockStyle.Top;
        this.GbSizeSelection.Location = new System.Drawing.Point(10, 154);
        this.GbSizeSelection.Name = "GbSizeSelection";
        this.GbSizeSelection.Size = new System.Drawing.Size(344, 99);
        this.GbSizeSelection.TabIndex = 3;
        this.GbSizeSelection.TabStop = false;
        this.GbSizeSelection.Text = "Size selection by user";
        // 
        // CbKeepAR
        // 
        this.CbKeepAR.AutoSize = true;
        this.CbKeepAR.Checked = true;
        this.CbKeepAR.CheckState = System.Windows.Forms.CheckState.Checked;
        this.CbKeepAR.Dock = System.Windows.Forms.DockStyle.Top;
        this.CbKeepAR.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
        this.CbKeepAR.Location = new System.Drawing.Point(3, 77);
        this.CbKeepAR.Name = "CbKeepAR";
        this.CbKeepAR.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
        this.CbKeepAR.Size = new System.Drawing.Size(338, 19);
        this.CbKeepAR.TabIndex = 0;
        this.CbKeepAR.Text = "Keep aspect ratio";
        this.CbKeepAR.UseVisualStyleBackColor = true;
        // 
        // tableLayoutPanel4
        // 
        this.tableLayoutPanel4.AutoSize = true;
        this.tableLayoutPanel4.ColumnCount = 7;
        this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 23F));
        this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel4.Controls.Add(this.LblResolution, 2, 1);
        this.tableLayoutPanel4.Controls.Add(this.LblCanvasWH, 2, 0);
        this.tableLayoutPanel4.Controls.Add(this.PbWarning, 1, 0);
        this.tableLayoutPanel4.Controls.Add(this.NudCanvasW, 3, 0);
        this.tableLayoutPanel4.Controls.Add(this.NudCanvasH, 5, 0);
        this.tableLayoutPanel4.Controls.Add(this.label9, 4, 0);
        this.tableLayoutPanel4.Controls.Add(this.NudDpi, 3, 1);
        this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Top;
        this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 19);
        this.tableLayoutPanel4.Name = "tableLayoutPanel4";
        this.tableLayoutPanel4.RowCount = 2;
        this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel4.Size = new System.Drawing.Size(338, 58);
        this.tableLayoutPanel4.TabIndex = 1;
        // 
        // LblResolution
        // 
        this.LblResolution.AutoSize = true;
        this.LblResolution.Dock = System.Windows.Forms.DockStyle.Fill;
        this.LblResolution.Location = new System.Drawing.Point(68, 29);
        this.LblResolution.Name = "LblResolution";
        this.LblResolution.Size = new System.Drawing.Size(92, 29);
        this.LblResolution.TabIndex = 0;
        this.LblResolution.Text = "Resolution (DPI)";
        this.LblResolution.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // LblCanvasWH
        // 
        this.LblCanvasWH.AutoSize = true;
        this.LblCanvasWH.Dock = System.Windows.Forms.DockStyle.Fill;
        this.LblCanvasWH.Location = new System.Drawing.Point(68, 0);
        this.LblCanvasWH.Name = "LblCanvasWH";
        this.LblCanvasWH.Size = new System.Drawing.Size(92, 29);
        this.LblCanvasWH.TabIndex = 1;
        this.LblCanvasWH.Text = "Canvas (W×H)";
        this.LblCanvasWH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // NudCanvasW
        // 
        this.NudCanvasW.Dock = System.Windows.Forms.DockStyle.Fill;
        this.NudCanvasW.Location = new System.Drawing.Point(166, 3);
        this.NudCanvasW.Maximum = new decimal(new int[] {
        100000,
        0,
        0,
        0});
        this.NudCanvasW.Minimum = new decimal(new int[] {
        1,
        0,
        0,
        0});
        this.NudCanvasW.MinimumSize = new System.Drawing.Size(50, 0);
        this.NudCanvasW.Name = "NudCanvasW";
        this.NudCanvasW.Size = new System.Drawing.Size(50, 23);
        this.NudCanvasW.TabIndex = 0;
        this.NudCanvasW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.NudCanvasW.Value = new decimal(new int[] {
        512,
        0,
        0,
        0});
        // 
        // NudCanvasH
        // 
        this.NudCanvasH.Dock = System.Windows.Forms.DockStyle.Fill;
        this.NudCanvasH.Location = new System.Drawing.Point(243, 3);
        this.NudCanvasH.Maximum = new decimal(new int[] {
        100000,
        0,
        0,
        0});
        this.NudCanvasH.Minimum = new decimal(new int[] {
        1,
        0,
        0,
        0});
        this.NudCanvasH.MinimumSize = new System.Drawing.Size(50, 0);
        this.NudCanvasH.Name = "NudCanvasH";
        this.NudCanvasH.Size = new System.Drawing.Size(50, 23);
        this.NudCanvasH.TabIndex = 1;
        this.NudCanvasH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.NudCanvasH.Value = new decimal(new int[] {
        512,
        0,
        0,
        0});
        // 
        // label9
        // 
        this.label9.AutoSize = true;
        this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
        this.label9.Location = new System.Drawing.Point(222, 0);
        this.label9.Name = "label9";
        this.label9.Size = new System.Drawing.Size(15, 29);
        this.label9.TabIndex = 5;
        this.label9.Text = "×";
        this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // NudDpi
        // 
        this.NudDpi.Dock = System.Windows.Forms.DockStyle.Fill;
        this.NudDpi.Location = new System.Drawing.Point(166, 32);
        this.NudDpi.Maximum = new decimal(new int[] {
        32767,
        0,
        0,
        0});
        this.NudDpi.Minimum = new decimal(new int[] {
        1,
        0,
        0,
        0});
        this.NudDpi.MinimumSize = new System.Drawing.Size(50, 0);
        this.NudDpi.Name = "NudDpi";
        this.NudDpi.Size = new System.Drawing.Size(50, 23);
        this.NudDpi.TabIndex = 2;
        this.NudDpi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
        this.NudDpi.Value = new decimal(new int[] {
        96,
        0,
        0,
        0});
        // 
        // GbInfo
        // 
        this.GbInfo.AutoSize = true;
        this.GbInfo.Controls.Add(this.tableLayoutPanel9);
        this.GbInfo.Controls.Add(this.tableLayoutPanel2);
        this.GbInfo.Dock = System.Windows.Forms.DockStyle.Top;
        this.GbInfo.Location = new System.Drawing.Point(10, 10);
        this.GbInfo.Name = "GbInfo";
        this.GbInfo.Size = new System.Drawing.Size(344, 144);
        this.GbInfo.TabIndex = 4;
        this.GbInfo.TabStop = false;
        this.GbInfo.Text = "Size settings given in SVG file";
        // 
        // tableLayoutPanel9
        // 
        this.tableLayoutPanel9.AutoSize = true;
        this.tableLayoutPanel9.ColumnCount = 3;
        this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel9.Controls.Add(this.LnkUseSvgSettings, 1, 0);
        this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Top;
        this.tableLayoutPanel9.Location = new System.Drawing.Point(3, 106);
        this.tableLayoutPanel9.Name = "tableLayoutPanel9";
        this.tableLayoutPanel9.RowCount = 1;
        this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel9.Size = new System.Drawing.Size(338, 35);
        this.tableLayoutPanel9.TabIndex = 0;
        // 
        // LnkUseSvgSettings
        // 
        this.LnkUseSvgSettings.AutoSize = true;
        this.LnkUseSvgSettings.Dock = System.Windows.Forms.DockStyle.Top;
        this.LnkUseSvgSettings.Location = new System.Drawing.Point(70, 5);
        this.LnkUseSvgSettings.Margin = new System.Windows.Forms.Padding(5);
        this.LnkUseSvgSettings.Name = "LnkUseSvgSettings";
        this.LnkUseSvgSettings.Size = new System.Drawing.Size(197, 25);
        this.LnkUseSvgSettings.TabIndex = 0;
        this.LnkUseSvgSettings.Text = "▼ Use size settings given in SVG ▼";
        // 
        // tableLayoutPanel2
        // 
        this.tableLayoutPanel2.AutoSize = true;
        this.tableLayoutPanel2.ColumnCount = 6;
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
        this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
        this.tableLayoutPanel2.Controls.Add(this.LblViewport, 1, 0);
        this.tableLayoutPanel2.Controls.Add(this.TbViewboxH, 4, 2);
        this.tableLayoutPanel2.Controls.Add(this.label6, 3, 2);
        this.tableLayoutPanel2.Controls.Add(this.TbViewportW, 2, 0);
        this.tableLayoutPanel2.Controls.Add(this.label3, 3, 0);
        this.tableLayoutPanel2.Controls.Add(this.TbViewboxW, 2, 2);
        this.tableLayoutPanel2.Controls.Add(this.TbViewportH, 4, 0);
        this.tableLayoutPanel2.Controls.Add(this.LblViewboxWH, 1, 2);
        this.tableLayoutPanel2.Controls.Add(this.LblViewboxXY, 1, 1);
        this.tableLayoutPanel2.Controls.Add(this.TbViewboxY, 4, 1);
        this.tableLayoutPanel2.Controls.Add(this.label4, 3, 1);
        this.tableLayoutPanel2.Controls.Add(this.TbViewboxX, 2, 1);
        this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
        this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 19);
        this.tableLayoutPanel2.Name = "tableLayoutPanel2";
        this.tableLayoutPanel2.RowCount = 3;
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
        this.tableLayoutPanel2.Size = new System.Drawing.Size(338, 87);
        this.tableLayoutPanel2.TabIndex = 1;
        // 
        // LblViewport
        // 
        this.LblViewport.AutoSize = true;
        this.LblViewport.Dock = System.Windows.Forms.DockStyle.Fill;
        this.LblViewport.Location = new System.Drawing.Point(50, 0);
        this.LblViewport.Name = "LblViewport";
        this.LblViewport.Size = new System.Drawing.Size(93, 29);
        this.LblViewport.TabIndex = 0;
        this.LblViewport.Text = "Viewport (W×H)";
        this.LblViewport.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // TbViewboxH
        // 
        this.TbViewboxH.Dock = System.Windows.Forms.DockStyle.Fill;
        this.TbViewboxH.Enabled = false;
        this.TbViewboxH.Location = new System.Drawing.Point(232, 61);
        this.TbViewboxH.MinimumSize = new System.Drawing.Size(50, 4);
        this.TbViewboxH.Name = "TbViewboxH";
        this.TbViewboxH.Size = new System.Drawing.Size(56, 23);
        this.TbViewboxH.TabIndex = 5;
        this.TbViewboxH.TabStop = false;
        this.TbViewboxH.Text = "-";
        this.TbViewboxH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // label6
        // 
        this.label6.AutoSize = true;
        this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
        this.label6.Location = new System.Drawing.Point(211, 58);
        this.label6.Name = "label6";
        this.label6.Size = new System.Drawing.Size(15, 29);
        this.label6.TabIndex = 2;
        this.label6.Text = "×";
        this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // TbViewportW
        // 
        this.TbViewportW.Dock = System.Windows.Forms.DockStyle.Fill;
        this.TbViewportW.Enabled = false;
        this.TbViewportW.Location = new System.Drawing.Point(149, 3);
        this.TbViewportW.MinimumSize = new System.Drawing.Size(50, 4);
        this.TbViewportW.Name = "TbViewportW";
        this.TbViewportW.Size = new System.Drawing.Size(56, 23);
        this.TbViewportW.TabIndex = 0;
        this.TbViewportW.TabStop = false;
        this.TbViewportW.Text = "-";
        this.TbViewportW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
        this.label3.Location = new System.Drawing.Point(211, 0);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(15, 29);
        this.label3.TabIndex = 4;
        this.label3.Text = "×";
        this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // TbViewboxW
        // 
        this.TbViewboxW.Dock = System.Windows.Forms.DockStyle.Fill;
        this.TbViewboxW.Enabled = false;
        this.TbViewboxW.Location = new System.Drawing.Point(149, 61);
        this.TbViewboxW.MinimumSize = new System.Drawing.Size(50, 4);
        this.TbViewboxW.Name = "TbViewboxW";
        this.TbViewboxW.Size = new System.Drawing.Size(56, 23);
        this.TbViewboxW.TabIndex = 4;
        this.TbViewboxW.TabStop = false;
        this.TbViewboxW.Text = "-";
        this.TbViewboxW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // TbViewportH
        // 
        this.TbViewportH.Dock = System.Windows.Forms.DockStyle.Fill;
        this.TbViewportH.Enabled = false;
        this.TbViewportH.Location = new System.Drawing.Point(232, 3);
        this.TbViewportH.MinimumSize = new System.Drawing.Size(50, 4);
        this.TbViewportH.Name = "TbViewportH";
        this.TbViewportH.Size = new System.Drawing.Size(56, 23);
        this.TbViewportH.TabIndex = 1;
        this.TbViewportH.TabStop = false;
        this.TbViewportH.Text = "-";
        this.TbViewportH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // LblViewboxWH
        // 
        this.LblViewboxWH.AutoSize = true;
        this.LblViewboxWH.Dock = System.Windows.Forms.DockStyle.Fill;
        this.LblViewboxWH.Location = new System.Drawing.Point(50, 58);
        this.LblViewboxWH.Name = "LblViewboxWH";
        this.LblViewboxWH.Size = new System.Drawing.Size(93, 29);
        this.LblViewboxWH.TabIndex = 7;
        this.LblViewboxWH.Text = "ViewBox (W×H)";
        this.LblViewboxWH.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // LblViewboxXY
        // 
        this.LblViewboxXY.AutoSize = true;
        this.LblViewboxXY.Dock = System.Windows.Forms.DockStyle.Fill;
        this.LblViewboxXY.Location = new System.Drawing.Point(50, 29);
        this.LblViewboxXY.Name = "LblViewboxXY";
        this.LblViewboxXY.Size = new System.Drawing.Size(93, 29);
        this.LblViewboxXY.TabIndex = 8;
        this.LblViewboxXY.Text = "ViewBox (X,Y)";
        this.LblViewboxXY.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
        // 
        // TbViewboxY
        // 
        this.TbViewboxY.Dock = System.Windows.Forms.DockStyle.Fill;
        this.TbViewboxY.Enabled = false;
        this.TbViewboxY.Location = new System.Drawing.Point(232, 32);
        this.TbViewboxY.MinimumSize = new System.Drawing.Size(50, 4);
        this.TbViewboxY.Name = "TbViewboxY";
        this.TbViewboxY.Size = new System.Drawing.Size(56, 23);
        this.TbViewboxY.TabIndex = 3;
        this.TbViewboxY.TabStop = false;
        this.TbViewboxY.Text = "-";
        this.TbViewboxY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
        this.label4.Location = new System.Drawing.Point(211, 29);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(15, 29);
        this.label4.TabIndex = 10;
        this.label4.Text = ",";
        this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
        // 
        // TbViewboxX
        // 
        this.TbViewboxX.Dock = System.Windows.Forms.DockStyle.Fill;
        this.TbViewboxX.Enabled = false;
        this.TbViewboxX.Location = new System.Drawing.Point(149, 32);
        this.TbViewboxX.MinimumSize = new System.Drawing.Size(50, 4);
        this.TbViewboxX.Name = "TbViewboxX";
        this.TbViewboxX.Size = new System.Drawing.Size(56, 23);
        this.TbViewboxX.TabIndex = 2;
        this.TbViewboxX.TabStop = false;
        this.TbViewboxX.Text = "-";
        this.TbViewboxX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
        // 
        // SvgImportDialog
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
        this.AutoSize = true;
        this.ClientSize = new System.Drawing.Size(364, 341);
        this.Controls.Add(this.RootPanel);
        this.Name = "SvgImportDialog";
        ((System.ComponentModel.ISupportInitialize)(this.PbWarning)).EndInit();
        this.RootPanel.ResumeLayout(false);
        this.RootPanel.PerformLayout();
        this.StatusStrip.ResumeLayout(false);
        this.StatusStrip.PerformLayout();
        this.tableLayoutPanel3.ResumeLayout(false);
        this.tableLayoutPanel3.PerformLayout();
        this.GbLayers.ResumeLayout(false);
        this.GbLayers.PerformLayout();
        this.tableLayoutPanel1.ResumeLayout(false);
        this.tableLayoutPanel1.PerformLayout();
        this.GbSizeSelection.ResumeLayout(false);
        this.GbSizeSelection.PerformLayout();
        this.tableLayoutPanel4.ResumeLayout(false);
        this.tableLayoutPanel4.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)(this.NudCanvasW)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.NudCanvasH)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.NudDpi)).EndInit();
        this.GbInfo.ResumeLayout(false);
        this.GbInfo.PerformLayout();
        this.tableLayoutPanel9.ResumeLayout(false);
        this.tableLayoutPanel9.PerformLayout();
        this.tableLayoutPanel2.ResumeLayout(false);
        this.tableLayoutPanel2.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();

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
