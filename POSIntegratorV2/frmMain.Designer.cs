namespace POSIntegratorV2
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.btnSystemSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.btnQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.TabPage = new System.Windows.Forms.TabControl();
            this.tabDM = new System.Windows.Forms.TabPage();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnManualIntegrate = new System.Windows.Forms.Button();
            this.txtDate = new System.Windows.Forms.DateTimePicker();
            this.txtUseItemPrice = new System.Windows.Forms.CheckBox();
            this.txtUserCode = new System.Windows.Forms.TextBox();
            this.txtBranchCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLocalConn = new System.Windows.Forms.TextBox();
            this.txtDomain = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtActivity = new System.Windows.Forms.TextBox();
            this.tabFM = new System.Windows.Forms.TabPage();
            this.btnStopFM = new System.Windows.Forms.Button();
            this.btnStartFM = new System.Windows.Forms.Button();
            this.txtActivityLogFM = new System.Windows.Forms.TextBox();
            this.menuStrip1.SuspendLayout();
            this.TabPage.SuspendLayout();
            this.tabDM.SuspendLayout();
            this.tabFM.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSystemSettings,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(526, 24);
            this.menuStrip1.TabIndex = 24;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // btnSystemSettings
            // 
            this.btnSystemSettings.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSettings,
            this.btnQuit});
            this.btnSystemSettings.Name = "btnSystemSettings";
            this.btnSystemSettings.Size = new System.Drawing.Size(37, 20);
            this.btnSystemSettings.Text = "&File";
            this.btnSystemSettings.Click += new System.EventHandler(this.btnSystemSettings_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(116, 22);
            this.btnSettings.Text = "&Settings";
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(116, 22);
            this.btnQuit.Text = "&Quit";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            // 
            // TabPage
            // 
            this.TabPage.Controls.Add(this.tabDM);
            this.TabPage.Controls.Add(this.tabFM);
            this.TabPage.Location = new System.Drawing.Point(4, 27);
            this.TabPage.Name = "TabPage";
            this.TabPage.SelectedIndex = 0;
            this.TabPage.Size = new System.Drawing.Size(510, 505);
            this.TabPage.TabIndex = 25;
            // 
            // tabDM
            // 
            this.tabDM.Controls.Add(this.btnStop);
            this.tabDM.Controls.Add(this.btnStart);
            this.tabDM.Controls.Add(this.btnManualIntegrate);
            this.tabDM.Controls.Add(this.txtDate);
            this.tabDM.Controls.Add(this.txtUseItemPrice);
            this.tabDM.Controls.Add(this.txtUserCode);
            this.tabDM.Controls.Add(this.txtBranchCode);
            this.tabDM.Controls.Add(this.label6);
            this.tabDM.Controls.Add(this.label5);
            this.tabDM.Controls.Add(this.label4);
            this.tabDM.Controls.Add(this.txtLocalConn);
            this.tabDM.Controls.Add(this.txtDomain);
            this.tabDM.Controls.Add(this.label3);
            this.tabDM.Controls.Add(this.label2);
            this.tabDM.Controls.Add(this.label1);
            this.tabDM.Controls.Add(this.txtActivity);
            this.tabDM.Location = new System.Drawing.Point(4, 22);
            this.tabDM.Name = "tabDM";
            this.tabDM.Padding = new System.Windows.Forms.Padding(3);
            this.tabDM.Size = new System.Drawing.Size(502, 479);
            this.tabDM.TabIndex = 0;
            this.tabDM.Text = "Database Monitoring";
            this.tabDM.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(421, 452);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 41;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click_1);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(340, 452);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 40;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click_1);
            // 
            // btnManualIntegrate
            // 
            this.btnManualIntegrate.Location = new System.Drawing.Point(236, 5);
            this.btnManualIntegrate.Name = "btnManualIntegrate";
            this.btnManualIntegrate.Size = new System.Drawing.Size(118, 23);
            this.btnManualIntegrate.TabIndex = 39;
            this.btnManualIntegrate.Text = "Manually Integrate";
            this.btnManualIntegrate.UseVisualStyleBackColor = true;
            this.btnManualIntegrate.Click += new System.EventHandler(this.btnManualIntegrate_Click_1);
            // 
            // txtDate
            // 
            this.txtDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.txtDate.Location = new System.Drawing.Point(126, 6);
            this.txtDate.Name = "txtDate";
            this.txtDate.Size = new System.Drawing.Size(104, 22);
            this.txtDate.TabIndex = 38;
            // 
            // txtUseItemPrice
            // 
            this.txtUseItemPrice.AutoSize = true;
            this.txtUseItemPrice.Enabled = false;
            this.txtUseItemPrice.Location = new System.Drawing.Point(126, 144);
            this.txtUseItemPrice.Name = "txtUseItemPrice";
            this.txtUseItemPrice.Size = new System.Drawing.Size(15, 14);
            this.txtUseItemPrice.TabIndex = 37;
            this.txtUseItemPrice.UseVisualStyleBackColor = true;
            // 
            // txtUserCode
            // 
            this.txtUserCode.Enabled = false;
            this.txtUserCode.Location = new System.Drawing.Point(126, 115);
            this.txtUserCode.Name = "txtUserCode";
            this.txtUserCode.Size = new System.Drawing.Size(193, 20);
            this.txtUserCode.TabIndex = 36;
            // 
            // txtBranchCode
            // 
            this.txtBranchCode.Enabled = false;
            this.txtBranchCode.Location = new System.Drawing.Point(126, 89);
            this.txtBranchCode.Name = "txtBranchCode";
            this.txtBranchCode.Size = new System.Drawing.Size(193, 20);
            this.txtBranchCode.TabIndex = 35;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(48, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Use Item Price";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 33;
            this.label5.Text = "User Code";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(48, 92);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 32;
            this.label4.Text = "Branch Code";
            // 
            // txtLocalConn
            // 
            this.txtLocalConn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocalConn.Location = new System.Drawing.Point(126, 60);
            this.txtLocalConn.Name = "txtLocalConn";
            this.txtLocalConn.Size = new System.Drawing.Size(370, 22);
            this.txtLocalConn.TabIndex = 31;
            // 
            // txtDomain
            // 
            this.txtDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDomain.Location = new System.Drawing.Point(126, 33);
            this.txtDomain.Name = "txtDomain";
            this.txtDomain.Size = new System.Drawing.Size(370, 22);
            this.txtDomain.TabIndex = 30;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 16);
            this.label3.TabIndex = 29;
            this.label3.Text = "Local Connection";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 16);
            this.label2.TabIndex = 28;
            this.label2.Text = "Domain";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 16);
            this.label1.TabIndex = 27;
            this.label1.Text = "Date";
            // 
            // txtActivity
            // 
            this.txtActivity.Location = new System.Drawing.Point(6, 164);
            this.txtActivity.Multiline = true;
            this.txtActivity.Name = "txtActivity";
            this.txtActivity.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtActivity.Size = new System.Drawing.Size(490, 282);
            this.txtActivity.TabIndex = 26;
            // 
            // tabFM
            // 
            this.tabFM.Controls.Add(this.btnStopFM);
            this.tabFM.Controls.Add(this.btnStartFM);
            this.tabFM.Controls.Add(this.txtActivityLogFM);
            this.tabFM.Location = new System.Drawing.Point(4, 22);
            this.tabFM.Name = "tabFM";
            this.tabFM.Padding = new System.Windows.Forms.Padding(3);
            this.tabFM.Size = new System.Drawing.Size(502, 479);
            this.tabFM.TabIndex = 1;
            this.tabFM.Text = "Folder Monitoring";
            this.tabFM.UseVisualStyleBackColor = true;
            // 
            // btnStopFM
            // 
            this.btnStopFM.Location = new System.Drawing.Point(421, 452);
            this.btnStopFM.Name = "btnStopFM";
            this.btnStopFM.Size = new System.Drawing.Size(75, 23);
            this.btnStopFM.TabIndex = 44;
            this.btnStopFM.Text = "Stop";
            this.btnStopFM.UseVisualStyleBackColor = true;
            this.btnStopFM.Click += new System.EventHandler(this.btnStopFM_Click);
            // 
            // btnStartFM
            // 
            this.btnStartFM.Location = new System.Drawing.Point(340, 452);
            this.btnStartFM.Name = "btnStartFM";
            this.btnStartFM.Size = new System.Drawing.Size(75, 23);
            this.btnStartFM.TabIndex = 43;
            this.btnStartFM.Text = "Start";
            this.btnStartFM.UseVisualStyleBackColor = true;
            this.btnStartFM.Click += new System.EventHandler(this.btnStartFM_Click);
            // 
            // txtActivityLogFM
            // 
            this.txtActivityLogFM.Location = new System.Drawing.Point(6, 164);
            this.txtActivityLogFM.Multiline = true;
            this.txtActivityLogFM.Name = "txtActivityLogFM";
            this.txtActivityLogFM.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtActivityLogFM.Size = new System.Drawing.Size(490, 282);
            this.txtActivityLogFM.TabIndex = 42;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 538);
            this.Controls.Add(this.TabPage);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Integrator Activity";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.TabPage.ResumeLayout(false);
            this.tabDM.ResumeLayout(false);
            this.tabDM.PerformLayout();
            this.tabFM.ResumeLayout(false);
            this.tabFM.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem btnSystemSettings;
        private System.Windows.Forms.ToolStripMenuItem btnSettings;
        private System.Windows.Forms.ToolStripMenuItem btnQuit;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl TabPage;
        private System.Windows.Forms.TabPage tabDM;
        private System.Windows.Forms.Button btnManualIntegrate;
        private System.Windows.Forms.DateTimePicker txtDate;
        private System.Windows.Forms.CheckBox txtUseItemPrice;
        private System.Windows.Forms.TextBox txtUserCode;
        private System.Windows.Forms.TextBox txtBranchCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLocalConn;
        private System.Windows.Forms.TextBox txtDomain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtActivity;
        private System.Windows.Forms.TabPage tabFM;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStopFM;
        private System.Windows.Forms.Button btnStartFM;
        private System.Windows.Forms.TextBox txtActivityLogFM;
    }
}

