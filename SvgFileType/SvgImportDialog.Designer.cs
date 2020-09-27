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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SvgImportDialog));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.gr1 = new System.Windows.Forms.GroupBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label6 = new System.Windows.Forms.Label();
            this.vbh = new System.Windows.Forms.TextBox();
            this.vbw = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.vby = new System.Windows.Forms.TextBox();
            this.vbx = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.vph = new System.Windows.Forms.TextBox();
            this.vpw = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gr2 = new System.Windows.Forms.GroupBox();
            this.warningBox = new System.Windows.Forms.PictureBox();
            this.cbKeepAR = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.canvash = new System.Windows.Forms.NumericUpDown();
            this.canvasw = new System.Windows.Forms.NumericUpDown();
            this.nudDpi = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.gr3 = new System.Windows.Forms.GroupBox();
            this.cbPSDSupport = new System.Windows.Forms.CheckBox();
            this.cbLayers = new System.Windows.Forms.CheckBox();
            this.cbOpacity = new System.Windows.Forms.CheckBox();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbGroups = new System.Windows.Forms.RadioButton();
            this.rbFlat = new System.Windows.Forms.RadioButton();
            this.tooltip = new System.Windows.Forms.ToolTip(this.components);
            this.linkGitHub = new System.Windows.Forms.LinkLabel();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.lbProgress = new System.Windows.Forms.Label();
            this.gr1.SuspendLayout();
            this.gr2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warningBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvasw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDpi)).BeginInit();
            this.gr3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.DarkRed;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Location = new System.Drawing.Point(220, 368);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.Location = new System.Drawing.Point(139, 368);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            // 
            // gr1
            // 
            this.gr1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gr1.Controls.Add(this.linkLabel1);
            this.gr1.Controls.Add(this.label6);
            this.gr1.Controls.Add(this.vbh);
            this.gr1.Controls.Add(this.vbw);
            this.gr1.Controls.Add(this.label7);
            this.gr1.Controls.Add(this.label4);
            this.gr1.Controls.Add(this.vby);
            this.gr1.Controls.Add(this.vbx);
            this.gr1.Controls.Add(this.label5);
            this.gr1.Controls.Add(this.label3);
            this.gr1.Controls.Add(this.vph);
            this.gr1.Controls.Add(this.vpw);
            this.gr1.Controls.Add(this.label2);
            this.gr1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gr1.Location = new System.Drawing.Point(13, 13);
            this.gr1.Name = "gr1";
            this.gr1.Size = new System.Drawing.Size(285, 115);
            this.gr1.TabIndex = 4;
            this.gr1.TabStop = false;
            this.gr1.Text = "Size settings given in SVG file";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(86, 96);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(189, 16);
            this.linkLabel1.TabIndex = 13;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Use size settings given in SVG";
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
            // vbh
            // 
            this.vbh.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vbh.Location = new System.Drawing.Point(203, 69);
            this.vbh.Name = "vbh";
            this.vbh.ReadOnly = true;
            this.vbh.Size = new System.Drawing.Size(72, 22);
            this.vbh.TabIndex = 10;
            this.vbh.Text = "n/a";
            this.vbh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // vbw
            // 
            this.vbw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vbw.Location = new System.Drawing.Point(114, 69);
            this.vbw.Name = "vbw";
            this.vbw.ReadOnly = true;
            this.vbw.Size = new System.Drawing.Size(72, 22);
            this.vbw.TabIndex = 9;
            this.vbw.Text = "n/a";
            this.vbw.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(7, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 16);
            this.label7.TabIndex = 8;
            this.label7.Text = "ViewBox (w x h):";
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
            // vby
            // 
            this.vby.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vby.Location = new System.Drawing.Point(203, 43);
            this.vby.Name = "vby";
            this.vby.ReadOnly = true;
            this.vby.Size = new System.Drawing.Size(72, 22);
            this.vby.TabIndex = 6;
            this.vby.Text = "n/a";
            this.vby.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // vbx
            // 
            this.vbx.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vbx.Location = new System.Drawing.Point(114, 43);
            this.vbx.Name = "vbx";
            this.vbx.ReadOnly = true;
            this.vbx.Size = new System.Drawing.Size(72, 22);
            this.vbx.TabIndex = 5;
            this.vbx.Text = "n/a";
            this.vbx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 16);
            this.label5.TabIndex = 4;
            this.label5.Text = "ViewBox (x / y):";
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
            // vph
            // 
            this.vph.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vph.Location = new System.Drawing.Point(203, 17);
            this.vph.Name = "vph";
            this.vph.ReadOnly = true;
            this.vph.Size = new System.Drawing.Size(72, 22);
            this.vph.TabIndex = 2;
            this.vph.Text = "n/a";
            this.vph.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // vpw
            // 
            this.vpw.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vpw.Location = new System.Drawing.Point(114, 17);
            this.vpw.Name = "vpw";
            this.vpw.ReadOnly = true;
            this.vpw.Size = new System.Drawing.Size(72, 22);
            this.vpw.TabIndex = 1;
            this.vpw.Text = "n/a";
            this.vpw.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Viewport (w x h):";
            // 
            // gr2
            // 
            this.gr2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gr2.Controls.Add(this.warningBox);
            this.gr2.Controls.Add(this.cbKeepAR);
            this.gr2.Controls.Add(this.label9);
            this.gr2.Controls.Add(this.canvash);
            this.gr2.Controls.Add(this.canvasw);
            this.gr2.Controls.Add(this.nudDpi);
            this.gr2.Controls.Add(this.label8);
            this.gr2.Controls.Add(this.label1);
            this.gr2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gr2.Location = new System.Drawing.Point(13, 134);
            this.gr2.Name = "gr2";
            this.gr2.Size = new System.Drawing.Size(285, 94);
            this.gr2.TabIndex = 5;
            this.gr2.TabStop = false;
            this.gr2.Text = "Size selection by user";
            // 
            // warningBox
            // 
            this.warningBox.Image = ((System.Drawing.Image)(resources.GetObject("warningBox.Image")));
            this.warningBox.Location = new System.Drawing.Point(99, 45);
            this.warningBox.Name = "warningBox";
            this.warningBox.Size = new System.Drawing.Size(22, 22);
            this.warningBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.warningBox.TabIndex = 14;
            this.warningBox.TabStop = false;
            this.tooltip.SetToolTip(this.warningBox, "File size is reduced with original aspect ratio. Files that have size more than 1" +
        "280 can cause memory problems.");
            // 
            // cbKeepAR
            // 
            this.cbKeepAR.AutoSize = true;
            this.cbKeepAR.Checked = true;
            this.cbKeepAR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbKeepAR.Location = new System.Drawing.Point(125, 71);
            this.cbKeepAR.Name = "cbKeepAR";
            this.cbKeepAR.Size = new System.Drawing.Size(132, 20);
            this.cbKeepAR.TabIndex = 13;
            this.cbKeepAR.Text = "Keep aspect ratio";
            this.cbKeepAR.UseVisualStyleBackColor = true;
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
            // canvash
            // 
            this.canvash.Location = new System.Drawing.Point(214, 45);
            this.canvash.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.canvash.Name = "canvash";
            this.canvash.Size = new System.Drawing.Size(65, 22);
            this.canvash.TabIndex = 4;
            this.canvash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.canvash.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // canvasw
            // 
            this.canvasw.Location = new System.Drawing.Point(125, 45);
            this.canvasw.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.canvasw.Name = "canvasw";
            this.canvasw.Size = new System.Drawing.Size(65, 22);
            this.canvasw.TabIndex = 3;
            this.canvasw.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.canvasw.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // nudDpi
            // 
            this.nudDpi.Location = new System.Drawing.Point(125, 18);
            this.nudDpi.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudDpi.Name = "nudDpi";
            this.nudDpi.Size = new System.Drawing.Size(65, 22);
            this.nudDpi.TabIndex = 2;
            this.nudDpi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudDpi.Value = new decimal(new int[] {
            96,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 16);
            this.label8.TabIndex = 1;
            this.label8.Text = "Canvas (w x h):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Resolution (DPI):";
            // 
            // gr3
            // 
            this.gr3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gr3.Controls.Add(this.cbPSDSupport);
            this.gr3.Controls.Add(this.cbLayers);
            this.gr3.Controls.Add(this.cbOpacity);
            this.gr3.Controls.Add(this.rbAll);
            this.gr3.Controls.Add(this.rbGroups);
            this.gr3.Controls.Add(this.rbFlat);
            this.gr3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gr3.Location = new System.Drawing.Point(13, 234);
            this.gr3.Name = "gr3";
            this.gr3.Size = new System.Drawing.Size(285, 97);
            this.gr3.TabIndex = 14;
            this.gr3.TabStop = false;
            this.gr3.Text = "Layers";
            // 
            // cbPSDSupport
            // 
            this.cbPSDSupport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbPSDSupport.AutoSize = true;
            this.cbPSDSupport.Location = new System.Drawing.Point(105, 65);
            this.cbPSDSupport.Name = "cbPSDSupport";
            this.cbPSDSupport.Size = new System.Drawing.Size(173, 20);
            this.cbPSDSupport.TabIndex = 5;
            this.cbPSDSupport.Text = "Import group boundaries";
            this.tooltip.SetToolTip(this.cbPSDSupport, "Import groups as empty \"start\" and \"end\" layers to determine boundaries (PSD plug" +
        "in compatability)");
            this.cbPSDSupport.UseVisualStyleBackColor = true;
            // 
            // cbLayers
            // 
            this.cbLayers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLayers.AutoSize = true;
            this.cbLayers.Checked = true;
            this.cbLayers.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLayers.Location = new System.Drawing.Point(105, 42);
            this.cbLayers.Name = "cbLayers";
            this.cbLayers.Size = new System.Drawing.Size(148, 20);
            this.cbLayers.TabIndex = 4;
            this.cbLayers.Text = "Import hidden layers";
            this.cbLayers.UseVisualStyleBackColor = true;
            // 
            // cbOpacity
            // 
            this.cbOpacity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbOpacity.AutoSize = true;
            this.cbOpacity.Checked = true;
            this.cbOpacity.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbOpacity.Location = new System.Drawing.Point(105, 20);
            this.cbOpacity.Name = "cbOpacity";
            this.cbOpacity.Size = new System.Drawing.Size(177, 20);
            this.cbOpacity.TabIndex = 3;
            this.cbOpacity.Text = "Opacity as layer property";
            this.tooltip.SetToolTip(this.cbOpacity, "Store opacity as layer parameter. Note: Group opacity may be lost while Paint.NET" +
        " has only one level of Layers.");
            this.cbOpacity.UseVisualStyleBackColor = true;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(10, 65);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(85, 20);
            this.rbAll.TabIndex = 2;
            this.rbAll.Text = "All Layers";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbGroups
            // 
            this.rbGroups.AutoSize = true;
            this.rbGroups.Location = new System.Drawing.Point(10, 42);
            this.rbGroups.Name = "rbGroups";
            this.rbGroups.Size = new System.Drawing.Size(70, 20);
            this.rbGroups.TabIndex = 1;
            this.rbGroups.Text = "Groups";
            this.tooltip.SetToolTip(this.rbGroups, "Results are unstable due to lack of paint.net features.");
            this.rbGroups.UseVisualStyleBackColor = true;
            // 
            // rbFlat
            // 
            this.rbFlat.AutoSize = true;
            this.rbFlat.Checked = true;
            this.rbFlat.Location = new System.Drawing.Point(10, 19);
            this.rbFlat.Name = "rbFlat";
            this.rbFlat.Size = new System.Drawing.Size(89, 20);
            this.rbFlat.TabIndex = 0;
            this.rbFlat.TabStop = true;
            this.rbFlat.Text = "Flat Image";
            this.rbFlat.UseVisualStyleBackColor = true;
            // 
            // tooltip
            // 
            this.tooltip.AutomaticDelay = 100;
            this.tooltip.AutoPopDelay = 4000;
            this.tooltip.InitialDelay = 100;
            this.tooltip.ReshowDelay = 20;
            this.tooltip.ShowAlways = true;
            this.tooltip.ToolTipTitle = "Please make sure you\'ve enough memory, especially if you\'re importing with layers" +
    ".";
            this.tooltip.UseAnimation = false;
            // 
            // linkGitHub
            // 
            this.linkGitHub.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.linkGitHub.AutoSize = true;
            this.linkGitHub.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkGitHub.Location = new System.Drawing.Point(10, 371);
            this.linkGitHub.Name = "linkGitHub";
            this.linkGitHub.Size = new System.Drawing.Size(112, 16);
            this.linkGitHub.TabIndex = 14;
            this.linkGitHub.TabStop = true;
            this.linkGitHub.Text = "Project on GitHub";
            this.tooltip.SetToolTip(this.linkGitHub, "Releases, sources and plugin information");
            // 
            // progress
            // 
            this.progress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progress.Location = new System.Drawing.Point(13, 337);
            this.progress.Name = "progress";
            this.progress.Size = new System.Drawing.Size(285, 23);
            this.progress.TabIndex = 15;
            // 
            // lbProgress
            // 
            this.lbProgress.AutoSize = true;
            this.lbProgress.BackColor = System.Drawing.Color.Transparent;
            this.lbProgress.Location = new System.Drawing.Point(122, 342);
            this.lbProgress.Name = "lbProgress";
            this.lbProgress.Size = new System.Drawing.Size(0, 13);
            this.lbProgress.TabIndex = 16;
            // 
            // SvgImportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(310, 403);
            this.Controls.Add(this.lbProgress);
            this.Controls.Add(this.progress);
            this.Controls.Add(this.linkGitHub);
            this.Controls.Add(this.gr3);
            this.Controls.Add(this.gr2);
            this.Controls.Add(this.gr1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SvgImportDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.gr1.ResumeLayout(false);
            this.gr1.PerformLayout();
            this.gr2.ResumeLayout(false);
            this.gr2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warningBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvasw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDpi)).EndInit();
            this.gr3.ResumeLayout(false);
            this.gr3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox gr1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox vbh;
        private System.Windows.Forms.TextBox vbw;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox vby;
        private System.Windows.Forms.TextBox vbx;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox vph;
        private System.Windows.Forms.TextBox vpw;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gr2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown canvash;
        private System.Windows.Forms.NumericUpDown canvasw;
        private System.Windows.Forms.NumericUpDown nudDpi;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbKeepAR;
        private System.Windows.Forms.GroupBox gr3;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbGroups;
        private System.Windows.Forms.RadioButton rbFlat;
        private System.Windows.Forms.CheckBox cbLayers;
        private System.Windows.Forms.CheckBox cbOpacity;
        private System.Windows.Forms.ToolTip tooltip;
        private System.Windows.Forms.CheckBox cbPSDSupport;
        private System.Windows.Forms.PictureBox warningBox;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkGitHub;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Label lbProgress;
    }
}