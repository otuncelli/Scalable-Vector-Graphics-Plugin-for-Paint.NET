namespace SvgFileTypePlugin
{
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
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnOk = new System.Windows.Forms.Button();
            this.GbInfo = new System.Windows.Forms.GroupBox();
            this.LnkUseSvgSettings = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.TbViewboxH = new System.Windows.Forms.TextBox();
            this.TbViewboxW = new System.Windows.Forms.TextBox();
            this.LblViewboxWH = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.TbViewboxY = new System.Windows.Forms.TextBox();
            this.TbViewboxX = new System.Windows.Forms.TextBox();
            this.LblViewboxXY = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TbViewportH = new System.Windows.Forms.TextBox();
            this.TbViewportW = new System.Windows.Forms.TextBox();
            this.LblViewport = new System.Windows.Forms.Label();
            this.GbSizeSelection = new System.Windows.Forms.GroupBox();
            this.PbWarning = new System.Windows.Forms.PictureBox();
            this.CbKeepAR = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.NudCanvasH = new System.Windows.Forms.NumericUpDown();
            this.NudCanvasW = new System.Windows.Forms.NumericUpDown();
            this.NudDpi = new System.Windows.Forms.NumericUpDown();
            this.LblCanvasWH = new System.Windows.Forms.Label();
            this.LblResolution = new System.Windows.Forms.Label();
            this.GbLayers = new System.Windows.Forms.GroupBox();
            this.CbGroupBoundaries = new System.Windows.Forms.CheckBox();
            this.CbHiddenLayers = new System.Windows.Forms.CheckBox();
            this.CbOpacity = new System.Windows.Forms.CheckBox();
            this.RbAllLayers = new System.Windows.Forms.RadioButton();
            this.RbGroups = new System.Windows.Forms.RadioButton();
            this.RbFlatten = new System.Windows.Forms.RadioButton();
            this.LnkGitHub = new System.Windows.Forms.LinkLabel();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ProgressLabel = new SvgFileTypePlugin.TransparentLabel();
            this.GbInfo.SuspendLayout();
            this.GbSizeSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudCanvasH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudCanvasW)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudDpi)).BeginInit();
            this.GbLayers.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.BackColor = System.Drawing.Color.DarkRed;
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancel.ForeColor = System.Drawing.Color.White;
            this.BtnCancel.Location = new System.Drawing.Point(220, 368);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(75, 23);
            this.BtnCancel.TabIndex = 1;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.UseVisualStyleBackColor = false;
            // 
            // BtnOk
            // 
            this.BtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnOk.BackColor = System.Drawing.Color.RoyalBlue;
            this.BtnOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.BtnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOk.ForeColor = System.Drawing.Color.White;
            this.BtnOk.Location = new System.Drawing.Point(139, 368);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(75, 23);
            this.BtnOk.TabIndex = 0;
            this.BtnOk.Text = "OK";
            this.BtnOk.UseVisualStyleBackColor = false;
            // 
            // GbInfo
            // 
            this.GbInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GbInfo.Controls.Add(this.LnkUseSvgSettings);
            this.GbInfo.Controls.Add(this.label6);
            this.GbInfo.Controls.Add(this.TbViewboxH);
            this.GbInfo.Controls.Add(this.TbViewboxW);
            this.GbInfo.Controls.Add(this.LblViewboxWH);
            this.GbInfo.Controls.Add(this.label4);
            this.GbInfo.Controls.Add(this.TbViewboxY);
            this.GbInfo.Controls.Add(this.TbViewboxX);
            this.GbInfo.Controls.Add(this.LblViewboxXY);
            this.GbInfo.Controls.Add(this.label3);
            this.GbInfo.Controls.Add(this.TbViewportH);
            this.GbInfo.Controls.Add(this.TbViewportW);
            this.GbInfo.Controls.Add(this.LblViewport);
            this.GbInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GbInfo.Location = new System.Drawing.Point(13, 13);
            this.GbInfo.Name = "GbInfo";
            this.GbInfo.Size = new System.Drawing.Size(285, 115);
            this.GbInfo.TabIndex = 4;
            this.GbInfo.TabStop = false;
            this.GbInfo.Text = "Size settings given in SVG file";
            // 
            // LnkUseSvgSettings
            // 
            this.LnkUseSvgSettings.AutoSize = true;
            this.LnkUseSvgSettings.Location = new System.Drawing.Point(86, 96);
            this.LnkUseSvgSettings.Name = "LnkUseSvgSettings";
            this.LnkUseSvgSettings.Size = new System.Drawing.Size(189, 16);
            this.LnkUseSvgSettings.TabIndex = 13;
            this.LnkUseSvgSettings.TabStop = true;
            this.LnkUseSvgSettings.Text = "Use size settings given in SVG";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(188, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 16);
            this.label6.TabIndex = 11;
            this.label6.Text = "x";
            // 
            // TbViewboxH
            // 
            this.TbViewboxH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbViewboxH.Location = new System.Drawing.Point(203, 69);
            this.TbViewboxH.Name = "TbViewboxH";
            this.TbViewboxH.ReadOnly = true;
            this.TbViewboxH.Size = new System.Drawing.Size(72, 22);
            this.TbViewboxH.TabIndex = 10;
            this.TbViewboxH.Text = "n/a";
            this.TbViewboxH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TbViewboxW
            // 
            this.TbViewboxW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbViewboxW.Location = new System.Drawing.Point(114, 69);
            this.TbViewboxW.Name = "TbViewboxW";
            this.TbViewboxW.ReadOnly = true;
            this.TbViewboxW.Size = new System.Drawing.Size(72, 22);
            this.TbViewboxW.TabIndex = 9;
            this.TbViewboxW.Text = "n/a";
            this.TbViewboxW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LblViewboxWH
            // 
            this.LblViewboxWH.AutoSize = true;
            this.LblViewboxWH.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblViewboxWH.Location = new System.Drawing.Point(7, 72);
            this.LblViewboxWH.Name = "LblViewboxWH";
            this.LblViewboxWH.Size = new System.Drawing.Size(102, 16);
            this.LblViewboxWH.TabIndex = 8;
            this.LblViewboxWH.Text = "ViewBox (w x h):";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(188, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 16);
            this.label4.TabIndex = 7;
            this.label4.Text = "/";
            // 
            // TbViewboxY
            // 
            this.TbViewboxY.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbViewboxY.Location = new System.Drawing.Point(203, 43);
            this.TbViewboxY.Name = "TbViewboxY";
            this.TbViewboxY.ReadOnly = true;
            this.TbViewboxY.Size = new System.Drawing.Size(72, 22);
            this.TbViewboxY.TabIndex = 6;
            this.TbViewboxY.Text = "n/a";
            this.TbViewboxY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TbViewboxX
            // 
            this.TbViewboxX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbViewboxX.Location = new System.Drawing.Point(114, 43);
            this.TbViewboxX.Name = "TbViewboxX";
            this.TbViewboxX.ReadOnly = true;
            this.TbViewboxX.Size = new System.Drawing.Size(72, 22);
            this.TbViewboxX.TabIndex = 5;
            this.TbViewboxX.Text = "n/a";
            this.TbViewboxX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LblViewboxXY
            // 
            this.LblViewboxXY.AutoSize = true;
            this.LblViewboxXY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblViewboxXY.Location = new System.Drawing.Point(7, 46);
            this.LblViewboxXY.Name = "LblViewboxXY";
            this.LblViewboxXY.Size = new System.Drawing.Size(97, 16);
            this.LblViewboxXY.TabIndex = 4;
            this.LblViewboxXY.Text = "ViewBox (x / y):";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(188, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(14, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "x";
            // 
            // TbViewportH
            // 
            this.TbViewportH.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbViewportH.Location = new System.Drawing.Point(203, 17);
            this.TbViewportH.Name = "TbViewportH";
            this.TbViewportH.ReadOnly = true;
            this.TbViewportH.Size = new System.Drawing.Size(72, 22);
            this.TbViewportH.TabIndex = 2;
            this.TbViewportH.Text = "n/a";
            this.TbViewportH.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TbViewportW
            // 
            this.TbViewportW.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TbViewportW.Location = new System.Drawing.Point(114, 17);
            this.TbViewportW.Name = "TbViewportW";
            this.TbViewportW.ReadOnly = true;
            this.TbViewportW.Size = new System.Drawing.Size(72, 22);
            this.TbViewportW.TabIndex = 1;
            this.TbViewportW.Text = "n/a";
            this.TbViewportW.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LblViewport
            // 
            this.LblViewport.AutoSize = true;
            this.LblViewport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblViewport.Location = new System.Drawing.Point(7, 20);
            this.LblViewport.Name = "LblViewport";
            this.LblViewport.Size = new System.Drawing.Size(102, 16);
            this.LblViewport.TabIndex = 0;
            this.LblViewport.Text = "Viewport (w x h):";
            // 
            // GbSizeSelection
            // 
            this.GbSizeSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GbSizeSelection.Controls.Add(this.PbWarning);
            this.GbSizeSelection.Controls.Add(this.CbKeepAR);
            this.GbSizeSelection.Controls.Add(this.label9);
            this.GbSizeSelection.Controls.Add(this.NudCanvasH);
            this.GbSizeSelection.Controls.Add(this.NudCanvasW);
            this.GbSizeSelection.Controls.Add(this.NudDpi);
            this.GbSizeSelection.Controls.Add(this.LblCanvasWH);
            this.GbSizeSelection.Controls.Add(this.LblResolution);
            this.GbSizeSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GbSizeSelection.Location = new System.Drawing.Point(13, 134);
            this.GbSizeSelection.Name = "GbSizeSelection";
            this.GbSizeSelection.Size = new System.Drawing.Size(285, 94);
            this.GbSizeSelection.TabIndex = 5;
            this.GbSizeSelection.TabStop = false;
            this.GbSizeSelection.Text = "Size selection by user";
            // 
            // PbWarning
            // 
            this.PbWarning.Location = new System.Drawing.Point(99, 45);
            this.PbWarning.Name = "PbWarning";
            this.PbWarning.Size = new System.Drawing.Size(22, 22);
            this.PbWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PbWarning.TabIndex = 14;
            this.PbWarning.TabStop = false;
            this.ToolTip1.SetToolTip(this.PbWarning, "Please make sure you\'ve enough memory, especially if you\'re importing with layers" +
        ".");
            this.PbWarning.Visible = false;
            // 
            // CbKeepAR
            // 
            this.CbKeepAR.AutoSize = true;
            this.CbKeepAR.Checked = true;
            this.CbKeepAR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbKeepAR.Location = new System.Drawing.Point(125, 71);
            this.CbKeepAR.Name = "CbKeepAR";
            this.CbKeepAR.Size = new System.Drawing.Size(132, 20);
            this.CbKeepAR.TabIndex = 13;
            this.CbKeepAR.Text = "Keep aspect ratio";
            this.CbKeepAR.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(196, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(14, 16);
            this.label9.TabIndex = 12;
            this.label9.Text = "x";
            // 
            // NudCanvasH
            // 
            this.NudCanvasH.Location = new System.Drawing.Point(214, 45);
            this.NudCanvasH.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.NudCanvasH.Name = "NudCanvasH";
            this.NudCanvasH.Size = new System.Drawing.Size(65, 22);
            this.NudCanvasH.TabIndex = 4;
            this.NudCanvasH.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NudCanvasH.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // NudCanvasW
            // 
            this.NudCanvasW.Location = new System.Drawing.Point(125, 45);
            this.NudCanvasW.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.NudCanvasW.Name = "NudCanvasW";
            this.NudCanvasW.Size = new System.Drawing.Size(65, 22);
            this.NudCanvasW.TabIndex = 3;
            this.NudCanvasW.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NudCanvasW.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // NudDpi
            // 
            this.NudDpi.Location = new System.Drawing.Point(125, 18);
            this.NudDpi.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.NudDpi.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NudDpi.Name = "NudDpi";
            this.NudDpi.Size = new System.Drawing.Size(65, 22);
            this.NudDpi.TabIndex = 2;
            this.NudDpi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.NudDpi.Value = new decimal(new int[] {
            96,
            0,
            0,
            0});
            // 
            // LblCanvasWH
            // 
            this.LblCanvasWH.AutoSize = true;
            this.LblCanvasWH.Location = new System.Drawing.Point(7, 47);
            this.LblCanvasWH.Name = "LblCanvasWH";
            this.LblCanvasWH.Size = new System.Drawing.Size(96, 16);
            this.LblCanvasWH.TabIndex = 1;
            this.LblCanvasWH.Text = "Canvas (w x h):";
            // 
            // LblResolution
            // 
            this.LblResolution.AutoSize = true;
            this.LblResolution.Location = new System.Drawing.Point(7, 20);
            this.LblResolution.Name = "LblResolution";
            this.LblResolution.Size = new System.Drawing.Size(108, 16);
            this.LblResolution.TabIndex = 0;
            this.LblResolution.Text = "Resolution (DPI):";
            // 
            // GbLayers
            // 
            this.GbLayers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GbLayers.Controls.Add(this.CbGroupBoundaries);
            this.GbLayers.Controls.Add(this.CbHiddenLayers);
            this.GbLayers.Controls.Add(this.CbOpacity);
            this.GbLayers.Controls.Add(this.RbAllLayers);
            this.GbLayers.Controls.Add(this.RbGroups);
            this.GbLayers.Controls.Add(this.RbFlatten);
            this.GbLayers.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GbLayers.Location = new System.Drawing.Point(13, 234);
            this.GbLayers.Name = "GbLayers";
            this.GbLayers.Size = new System.Drawing.Size(285, 97);
            this.GbLayers.TabIndex = 14;
            this.GbLayers.TabStop = false;
            this.GbLayers.Text = "Layers";
            // 
            // CbGroupBoundaries
            // 
            this.CbGroupBoundaries.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CbGroupBoundaries.AutoSize = true;
            this.CbGroupBoundaries.Location = new System.Drawing.Point(105, 65);
            this.CbGroupBoundaries.Name = "CbGroupBoundaries";
            this.CbGroupBoundaries.Size = new System.Drawing.Size(135, 20);
            this.CbGroupBoundaries.TabIndex = 5;
            this.CbGroupBoundaries.Text = "Group boundaries";
            this.ToolTip1.SetToolTip(this.CbGroupBoundaries, "Imports group boundaries as empty layers.");
            this.CbGroupBoundaries.UseVisualStyleBackColor = true;
            // 
            // CbHiddenLayers
            // 
            this.CbHiddenLayers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CbHiddenLayers.AutoSize = true;
            this.CbHiddenLayers.Checked = true;
            this.CbHiddenLayers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbHiddenLayers.Location = new System.Drawing.Point(105, 42);
            this.CbHiddenLayers.Name = "CbHiddenLayers";
            this.CbHiddenLayers.Size = new System.Drawing.Size(111, 20);
            this.CbHiddenLayers.TabIndex = 4;
            this.CbHiddenLayers.Text = "Hidden layers";
            this.CbHiddenLayers.UseVisualStyleBackColor = true;
            // 
            // CbOpacity
            // 
            this.CbOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CbOpacity.AutoSize = true;
            this.CbOpacity.Checked = true;
            this.CbOpacity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CbOpacity.Location = new System.Drawing.Point(105, 20);
            this.CbOpacity.Name = "CbOpacity";
            this.CbOpacity.Size = new System.Drawing.Size(177, 20);
            this.CbOpacity.TabIndex = 3;
            this.CbOpacity.Text = "Opacity as layer property";
            this.CbOpacity.UseVisualStyleBackColor = true;
            // 
            // RbAllLayers
            // 
            this.RbAllLayers.AutoSize = true;
            this.RbAllLayers.Location = new System.Drawing.Point(10, 65);
            this.RbAllLayers.Name = "RbAllLayers";
            this.RbAllLayers.Size = new System.Drawing.Size(85, 20);
            this.RbAllLayers.TabIndex = 2;
            this.RbAllLayers.Text = "All Layers";
            this.RbAllLayers.UseVisualStyleBackColor = true;
            // 
            // RbGroups
            // 
            this.RbGroups.AutoSize = true;
            this.RbGroups.Location = new System.Drawing.Point(10, 42);
            this.RbGroups.Name = "RbGroups";
            this.RbGroups.Size = new System.Drawing.Size(70, 20);
            this.RbGroups.TabIndex = 1;
            this.RbGroups.Text = "Groups";
            this.RbGroups.UseVisualStyleBackColor = true;
            // 
            // RbFlatten
            // 
            this.RbFlatten.AutoSize = true;
            this.RbFlatten.Checked = true;
            this.RbFlatten.Location = new System.Drawing.Point(10, 19);
            this.RbFlatten.Name = "RbFlatten";
            this.RbFlatten.Size = new System.Drawing.Size(66, 20);
            this.RbFlatten.TabIndex = 0;
            this.RbFlatten.TabStop = true;
            this.RbFlatten.Text = "Flatten";
            this.RbFlatten.UseVisualStyleBackColor = true;
            // 
            // LnkGitHub
            // 
            this.LnkGitHub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.LnkGitHub.AutoSize = true;
            this.LnkGitHub.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LnkGitHub.Location = new System.Drawing.Point(10, 371);
            this.LnkGitHub.Name = "LnkGitHub";
            this.LnkGitHub.Size = new System.Drawing.Size(112, 16);
            this.LnkGitHub.TabIndex = 14;
            this.LnkGitHub.TabStop = true;
            this.LnkGitHub.Text = "Project on GitHub";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.Location = new System.Drawing.Point(13, 337);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(285, 23);
            this.ProgressBar.TabIndex = 15;
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
            // ProgressLabel
            // 
            this.ProgressLabel.AutoSize = true;
            this.ProgressLabel.BackColor = System.Drawing.Color.Transparent;
            this.ProgressLabel.Location = new System.Drawing.Point(122, 342);
            this.ProgressLabel.Name = "ProgressLabel";
            this.ProgressLabel.Size = new System.Drawing.Size(41, 13);
            this.ProgressLabel.TabIndex = 16;
            this.ProgressLabel.Text = "Ready!";
            // 
            // SvgImportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(310, 403);
            this.Controls.Add(this.ProgressLabel);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.LnkGitHub);
            this.Controls.Add(this.GbLayers);
            this.Controls.Add(this.GbSizeSelection);
            this.Controls.Add(this.GbInfo);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.BtnCancel);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SvgImportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.GbInfo.ResumeLayout(false);
            this.GbInfo.PerformLayout();
            this.GbSizeSelection.ResumeLayout(false);
            this.GbSizeSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PbWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudCanvasH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudCanvasW)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NudDpi)).EndInit();
            this.GbLayers.ResumeLayout(false);
            this.GbLayers.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.GroupBox GbInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox TbViewboxH;
        private System.Windows.Forms.TextBox TbViewboxW;
        private System.Windows.Forms.Label LblViewboxWH;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox TbViewboxY;
        private System.Windows.Forms.TextBox TbViewboxX;
        private System.Windows.Forms.Label LblViewboxXY;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TbViewportH;
        private System.Windows.Forms.TextBox TbViewportW;
        private System.Windows.Forms.Label LblViewport;
        private System.Windows.Forms.GroupBox GbSizeSelection;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown NudCanvasH;
        private System.Windows.Forms.NumericUpDown NudCanvasW;
        private System.Windows.Forms.NumericUpDown NudDpi;
        private System.Windows.Forms.Label LblCanvasWH;
        private System.Windows.Forms.Label LblResolution;
        private System.Windows.Forms.CheckBox CbKeepAR;
        private System.Windows.Forms.GroupBox GbLayers;
        private System.Windows.Forms.RadioButton RbAllLayers;
        private System.Windows.Forms.RadioButton RbGroups;
        private System.Windows.Forms.RadioButton RbFlatten;
        private System.Windows.Forms.CheckBox CbHiddenLayers;
        private System.Windows.Forms.CheckBox CbOpacity;
        private System.Windows.Forms.CheckBox CbGroupBoundaries;
        private System.Windows.Forms.PictureBox PbWarning;
        private System.Windows.Forms.LinkLabel LnkUseSvgSettings;
        private System.Windows.Forms.LinkLabel LnkGitHub;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private TransparentLabel ProgressLabel;
        private System.Windows.Forms.ToolTip ToolTip1;
    }
}