namespace SvgFileTypePlugin
{
    partial class UiDialog
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbKeepAR = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.canvash = new System.Windows.Forms.NumericUpDown();
            this.canvasw = new System.Windows.Forms.NumericUpDown();
            this.nudDpi = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvasw)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDpi)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(197, 217);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(116, 217);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.vbh);
            this.groupBox1.Controls.Add(this.vbw);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.vby);
            this.groupBox1.Controls.Add(this.vbx);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.vph);
            this.groupBox1.Controls.Add(this.vpw);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 99);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Size settings given in SVG file";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(169, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "x";
            // 
            // vbh
            // 
            this.vbh.Location = new System.Drawing.Point(187, 69);
            this.vbh.Name = "vbh";
            this.vbh.ReadOnly = true;
            this.vbh.Size = new System.Drawing.Size(65, 20);
            this.vbh.TabIndex = 10;
            this.vbh.Text = "n/a";
            this.vbh.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // vbw
            // 
            this.vbw.Location = new System.Drawing.Point(98, 69);
            this.vbw.Name = "vbw";
            this.vbw.ReadOnly = true;
            this.vbw.Size = new System.Drawing.Size(65, 20);
            this.vbw.TabIndex = 9;
            this.vbw.Text = "n/a";
            this.vbw.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Viewbox (w x h):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(169, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(12, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "/";
            // 
            // vby
            // 
            this.vby.Location = new System.Drawing.Point(187, 43);
            this.vby.Name = "vby";
            this.vby.ReadOnly = true;
            this.vby.Size = new System.Drawing.Size(65, 20);
            this.vby.TabIndex = 6;
            this.vby.Text = "n/a";
            this.vby.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // vbx
            // 
            this.vbx.Location = new System.Drawing.Point(98, 43);
            this.vbx.Name = "vbx";
            this.vbx.ReadOnly = true;
            this.vbx.Size = new System.Drawing.Size(65, 20);
            this.vbx.TabIndex = 5;
            this.vbx.Text = "n/a";
            this.vbx.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Viewbox (x / y):";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(169, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(12, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "x";
            // 
            // vph
            // 
            this.vph.Location = new System.Drawing.Point(187, 17);
            this.vph.Name = "vph";
            this.vph.ReadOnly = true;
            this.vph.Size = new System.Drawing.Size(65, 20);
            this.vph.TabIndex = 2;
            this.vph.Text = "n/a";
            this.vph.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // vpw
            // 
            this.vpw.Location = new System.Drawing.Point(98, 17);
            this.vpw.Name = "vpw";
            this.vpw.ReadOnly = true;
            this.vpw.Size = new System.Drawing.Size(65, 20);
            this.vpw.TabIndex = 1;
            this.vpw.Text = "n/a";
            this.vpw.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Viewport (w x h):";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbKeepAR);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.canvash);
            this.groupBox2.Controls.Add(this.canvasw);
            this.groupBox2.Controls.Add(this.nudDpi);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(13, 119);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(259, 92);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Size selection by user";
            // 
            // cbKeepAR
            // 
            this.cbKeepAR.AutoSize = true;
            this.cbKeepAR.Checked = true;
            this.cbKeepAR.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbKeepAR.Location = new System.Drawing.Point(98, 69);
            this.cbKeepAR.Name = "cbKeepAR";
            this.cbKeepAR.Size = new System.Drawing.Size(109, 17);
            this.cbKeepAR.TabIndex = 13;
            this.cbKeepAR.Text = "Keep aspect ratio";
            this.cbKeepAR.UseVisualStyleBackColor = true;
            this.cbKeepAR.CheckedChanged += new System.EventHandler(this.cbKeepAR_CheckedChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(169, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(12, 13);
            this.label9.TabIndex = 12;
            this.label9.Text = "x";
            // 
            // canvash
            // 
            this.canvash.Location = new System.Drawing.Point(187, 45);
            this.canvash.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.canvash.Name = "canvash";
            this.canvash.Size = new System.Drawing.Size(65, 20);
            this.canvash.TabIndex = 4;
            this.canvash.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.canvash.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.canvash.ValueChanged += new System.EventHandler(this.canvash_ValueChanged);
            // 
            // canvasw
            // 
            this.canvasw.Location = new System.Drawing.Point(98, 45);
            this.canvasw.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.canvasw.Name = "canvasw";
            this.canvasw.Size = new System.Drawing.Size(65, 20);
            this.canvasw.TabIndex = 3;
            this.canvasw.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.canvasw.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.canvasw.ValueChanged += new System.EventHandler(this.canvasw_ValueChanged);
            // 
            // nudDpi
            // 
            this.nudDpi.Location = new System.Drawing.Point(98, 18);
            this.nudDpi.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudDpi.Name = "nudDpi";
            this.nudDpi.Size = new System.Drawing.Size(65, 20);
            this.nudDpi.TabIndex = 2;
            this.nudDpi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudDpi.Value = new decimal(new int[] {
            90,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Canvas (w x h):";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Resolution (DPI):";
            // 
            // UIDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 252);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UiDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SVG (Scalable Vector Graphics) Import";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.canvash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.canvasw)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDpi)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.GroupBox groupBox1;
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
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown canvash;
        private System.Windows.Forms.NumericUpDown canvasw;
        private System.Windows.Forms.NumericUpDown nudDpi;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbKeepAR;
    }
}