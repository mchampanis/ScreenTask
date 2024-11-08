﻿namespace ScreenTask
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.gbOptions = new System.Windows.Forms.GroupBox();
            this.gbLog = new System.Windows.Forms.GroupBox();
            this.gbPreview = new System.Windows.Forms.GroupBox();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.pnlOptions = new System.Windows.Forms.Panel();
            this.comboIPs = new System.Windows.Forms.ComboBox();
            this.cbCaptureMouse = new System.Windows.Forms.CheckBox();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbPrivate = new System.Windows.Forms.CheckBox();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numShotEvery = new System.Windows.Forms.NumericUpDown();
            this.comboScreens = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnLaunch = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cbAutoStart = new System.Windows.Forms.CheckBox();
            this.qualitySlider = new System.Windows.Forms.TrackBar();
            this.lblQuality = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShotEvery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualitySlider)).BeginInit();
            this.SuspendLayout();
            // 
            // gbOptions
            // 
            this.gbOptions.Location = new System.Drawing.Point(964, 17);
            this.gbOptions.Margin = new System.Windows.Forms.Padding(4);
            this.gbOptions.Name = "gbOptions";
            this.gbOptions.Padding = new System.Windows.Forms.Padding(4);
            this.gbOptions.Size = new System.Drawing.Size(152, 138);
            this.gbOptions.TabIndex = 0;
            this.gbOptions.TabStop = false;
            this.gbOptions.Text = "Server Options";
            // 
            // gbLog
            // 
            this.gbLog.Location = new System.Drawing.Point(964, 415);
            this.gbLog.Margin = new System.Windows.Forms.Padding(4);
            this.gbLog.Name = "gbLog";
            this.gbLog.Padding = new System.Windows.Forms.Padding(4);
            this.gbLog.Size = new System.Drawing.Size(545, 148);
            this.gbLog.TabIndex = 1;
            this.gbLog.TabStop = false;
            this.gbLog.Text = "Log";
            // 
            // gbPreview
            // 
            this.gbPreview.Location = new System.Drawing.Point(964, 175);
            this.gbPreview.Margin = new System.Windows.Forms.Padding(4);
            this.gbPreview.Name = "gbPreview";
            this.gbPreview.Padding = new System.Windows.Forms.Padding(4);
            this.gbPreview.Size = new System.Drawing.Size(545, 233);
            this.gbPreview.TabIndex = 2;
            this.gbPreview.TabStop = false;
            this.gbPreview.Text = "Preview";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(7, 412);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(475, 114);
            this.txtLog.TabIndex = 3;
            this.txtLog.TextChanged += new System.EventHandler(this.txtLog_TextChanged);
            // 
            // pnlOptions
            // 
            this.pnlOptions.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlOptions.Location = new System.Drawing.Point(1171, 34);
            this.pnlOptions.Margin = new System.Windows.Forms.Padding(4);
            this.pnlOptions.Name = "pnlOptions";
            this.pnlOptions.Size = new System.Drawing.Size(123, 69);
            this.pnlOptions.TabIndex = 5;
            // 
            // comboIPs
            // 
            this.comboIPs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboIPs.FormattingEnabled = true;
            this.comboIPs.Location = new System.Drawing.Point(36, 105);
            this.comboIPs.Margin = new System.Windows.Forms.Padding(4);
            this.comboIPs.Name = "comboIPs";
            this.comboIPs.Size = new System.Drawing.Size(296, 24);
            this.comboIPs.TabIndex = 27;
            // 
            // cbCaptureMouse
            // 
            this.cbCaptureMouse.AutoSize = true;
            this.cbCaptureMouse.BackColor = System.Drawing.Color.Transparent;
            this.cbCaptureMouse.ForeColor = System.Drawing.Color.White;
            this.cbCaptureMouse.Location = new System.Drawing.Point(16, 239);
            this.cbCaptureMouse.Margin = new System.Windows.Forms.Padding(4);
            this.cbCaptureMouse.Name = "cbCaptureMouse";
            this.cbCaptureMouse.Size = new System.Drawing.Size(151, 20);
            this.cbCaptureMouse.TabIndex = 26;
            this.cbCaptureMouse.Text = "Show Mouse Pointer";
            this.cbCaptureMouse.UseVisualStyleBackColor = false;
            this.cbCaptureMouse.CheckedChanged += new System.EventHandler(this.cbCaptureMouse_CheckedChanged);
            // 
            // btnStartServer
            // 
            this.btnStartServer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStartServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartServer.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.btnStartServer.ForeColor = System.Drawing.Color.White;
            this.btnStartServer.Location = new System.Drawing.Point(13, 335);
            this.btnStartServer.Margin = new System.Windows.Forms.Padding(4);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(469, 37);
            this.btnStartServer.TabIndex = 23;
            this.btnStartServer.Tag = "start";
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.UseVisualStyleBackColor = false;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Enabled = false;
            this.txtPassword.Location = new System.Drawing.Point(395, 194);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(87, 22);
            this.txtPassword.TabIndex = 22;
            this.txtPassword.Text = "task";
            // 
            // txtUser
            // 
            this.txtUser.Enabled = false;
            this.txtUser.Location = new System.Drawing.Point(200, 194);
            this.txtUser.Margin = new System.Windows.Forms.Padding(4);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(87, 22);
            this.txtUser.TabIndex = 21;
            this.txtUser.Text = "screen";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(318, 197);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 16);
            this.label4.TabIndex = 20;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(158, 197);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 16);
            this.label3.TabIndex = 19;
            this.label3.Text = "User";
            // 
            // cbPrivate
            // 
            this.cbPrivate.AutoSize = true;
            this.cbPrivate.BackColor = System.Drawing.Color.Transparent;
            this.cbPrivate.ForeColor = System.Drawing.Color.White;
            this.cbPrivate.Location = new System.Drawing.Point(15, 197);
            this.cbPrivate.Margin = new System.Windows.Forms.Padding(4);
            this.cbPrivate.Name = "cbPrivate";
            this.cbPrivate.Size = new System.Drawing.Size(112, 20);
            this.cbPrivate.TabIndex = 18;
            this.cbPrivate.Text = "Authentication";
            this.cbPrivate.UseVisualStyleBackColor = false;
            this.cbPrivate.CheckedChanged += new System.EventHandler(this.cbPrivate_CheckedChanged);
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(378, 106);
            this.numPort.Margin = new System.Windows.Forms.Padding(4);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(104, 22);
            this.numPort.TabIndex = 1;
            this.numPort.Value = new decimal(new int[] {
            7070,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(341, 108);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "Port";
            // 
            // txtURL
            // 
            this.txtURL.Location = new System.Drawing.Point(53, 287);
            this.txtURL.Margin = new System.Windows.Forms.Padding(4);
            this.txtURL.Name = "txtURL";
            this.txtURL.ReadOnly = true;
            this.txtURL.Size = new System.Drawing.Size(315, 22);
            this.txtURL.TabIndex = 17;
            this.txtURL.Text = "(start server first)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(11, 290);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 16);
            this.label1.TabIndex = 14;
            this.label1.Text = "URL";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(13, 108);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(19, 16);
            this.label5.TabIndex = 28;
            this.label5.Text = "IP";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(388, 240);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 16);
            this.label6.TabIndex = 31;
            this.label6.Text = "milliseconds";
            // 
            // numShotEvery
            // 
            this.numShotEvery.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numShotEvery.Location = new System.Drawing.Point(279, 238);
            this.numShotEvery.Margin = new System.Windows.Forms.Padding(4);
            this.numShotEvery.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.numShotEvery.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numShotEvery.Name = "numShotEvery";
            this.numShotEvery.Size = new System.Drawing.Size(101, 22);
            this.numShotEvery.TabIndex = 30;
            this.numShotEvery.ThousandsSeparator = true;
            this.numShotEvery.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // comboScreens
            // 
            this.comboScreens.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboScreens.FormattingEnabled = true;
            this.comboScreens.Location = new System.Drawing.Point(64, 148);
            this.comboScreens.Margin = new System.Windows.Forms.Padding(4);
            this.comboScreens.Name = "comboScreens";
            this.comboScreens.Size = new System.Drawing.Size(195, 24);
            this.comboScreens.TabIndex = 35;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(11, 151);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(50, 16);
            this.label7.TabIndex = 36;
            this.label7.Text = "Screen";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ScreenTask.Properties.Resources.screentask_logo;
            this.pictureBox1.Location = new System.Drawing.Point(15, 11);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(350, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 37;
            this.pictureBox1.TabStop = false;
            // 
            // btnLaunch
            // 
            this.btnLaunch.Enabled = false;
            this.btnLaunch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLaunch.ForeColor = System.Drawing.Color.White;
            this.btnLaunch.Location = new System.Drawing.Point(395, 283);
            this.btnLaunch.Margin = new System.Windows.Forms.Padding(4);
            this.btnLaunch.Name = "btnLaunch";
            this.btnLaunch.Size = new System.Drawing.Size(87, 31);
            this.btnLaunch.TabIndex = 38;
            this.btnLaunch.Text = "Open";
            this.btnLaunch.UseVisualStyleBackColor = true;
            this.btnLaunch.Click += new System.EventHandler(this.btnLaunch_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(221, 240);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(54, 16);
            this.label8.TabIndex = 39;
            this.label8.Text = "Refresh";
            // 
            // cbAutoStart
            // 
            this.cbAutoStart.AutoSize = true;
            this.cbAutoStart.BackColor = System.Drawing.Color.Transparent;
            this.cbAutoStart.ForeColor = System.Drawing.Color.White;
            this.cbAutoStart.Location = new System.Drawing.Point(16, 380);
            this.cbAutoStart.Margin = new System.Windows.Forms.Padding(4);
            this.cbAutoStart.Name = "cbAutoStart";
            this.cbAutoStart.Size = new System.Drawing.Size(129, 20);
            this.cbAutoStart.TabIndex = 40;
            this.cbAutoStart.Text = "Auto Start Server";
            this.cbAutoStart.UseVisualStyleBackColor = false;
            // 
            // qualitySlider
            // 
            this.qualitySlider.Location = new System.Drawing.Point(343, 137);
            this.qualitySlider.Margin = new System.Windows.Forms.Padding(4);
            this.qualitySlider.Maximum = 100;
            this.qualitySlider.Minimum = 1;
            this.qualitySlider.Name = "qualitySlider";
            this.qualitySlider.Size = new System.Drawing.Size(139, 56);
            this.qualitySlider.SmallChange = 10;
            this.qualitySlider.TabIndex = 42;
            this.qualitySlider.TickFrequency = 25;
            this.qualitySlider.Value = 75;
            this.qualitySlider.Scroll += new System.EventHandler(this.qualitySlider_Scroll);
            // 
            // lblQuality
            // 
            this.lblQuality.AutoSize = true;
            this.lblQuality.BackColor = System.Drawing.Color.Transparent;
            this.lblQuality.ForeColor = System.Drawing.Color.White;
            this.lblQuality.Location = new System.Drawing.Point(293, 151);
            this.lblQuality.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblQuality.Name = "lblQuality";
            this.lblQuality.Size = new System.Drawing.Size(48, 16);
            this.lblQuality.TabIndex = 43;
            this.lblQuality.Text = "Quality";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(493, 533);
            this.Controls.Add(this.lblQuality);
            this.Controls.Add(this.qualitySlider);
            this.Controls.Add(this.cbAutoStart);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnLaunch);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboScreens);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.gbPreview);
            this.Controls.Add(this.gbLog);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.gbOptions);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pnlOptions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPrivate);
            this.Controls.Add(this.txtURL);
            this.Controls.Add(this.numPort);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboIPs);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numShotEvery);
            this.Controls.Add(this.cbCaptureMouse);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScreenTask";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShotEvery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qualitySlider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gbOptions;
        private System.Windows.Forms.GroupBox gbLog;
        private System.Windows.Forms.GroupBox gbPreview;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Panel pnlOptions;
        private System.Windows.Forms.ComboBox comboIPs;
        private System.Windows.Forms.CheckBox cbCaptureMouse;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbPrivate;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numShotEvery;
        private System.Windows.Forms.ComboBox comboScreens;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnLaunch;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox cbAutoStart;
        private System.Windows.Forms.TrackBar qualitySlider;
        private System.Windows.Forms.Label lblQuality;
    }
}

