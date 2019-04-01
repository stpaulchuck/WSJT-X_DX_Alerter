namespace WSJTX_DX_Alerter
{
    partial class FrmDXAlerterMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDXAlerterMain));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.rbMSK144 = new System.Windows.Forms.RadioButton();
			this.rbJT65 = new System.Windows.Forms.RadioButton();
			this.rbJT9 = new System.Windows.Forms.RadioButton();
			this.rbJT4 = new System.Windows.Forms.RadioButton();
			this.rbFT8 = new System.Windows.Forms.RadioButton();
			this.btnClearList = new System.Windows.Forms.Button();
			this.btnExit = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.lblRunStatus = new System.Windows.Forms.Label();
			this.btnRun = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.label5 = new System.Windows.Forms.Label();
			this.txtThreshold = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.chkQuietMode = new System.Windows.Forms.CheckBox();
			this.chkAutoStart = new System.Windows.Forms.CheckBox();
			this.chkLogHits = new System.Windows.Forms.CheckBox();
			this.chkSendText = new System.Windows.Forms.CheckBox();
			this.chkSendEmail = new System.Windows.Forms.CheckBox();
			this.chkUnconfirmedAlert = new System.Windows.Forms.CheckBox();
			this.btnFindFile = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.txtDirPath = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnFindXMLlog = new System.Windows.Forms.Button();
			this.txtXMLlogLocation = new System.Windows.Forms.TextBox();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.tsStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabControl1.ItemSize = new System.Drawing.Size(200, 21);
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(427, 480);
			this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControl1.TabIndex = 0;
			this.tabControl1.Selected += new System.Windows.Forms.TabControlEventHandler(this.tabControl1_Selected);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.btnClearList);
			this.tabPage1.Controls.Add(this.btnExit);
			this.tabPage1.Controls.Add(this.listBox1);
			this.tabPage1.Controls.Add(this.lblRunStatus);
			this.tabPage1.Controls.Add(this.btnRun);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(419, 451);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Main";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.rbMSK144);
			this.groupBox1.Controls.Add(this.rbJT65);
			this.groupBox1.Controls.Add(this.rbJT9);
			this.groupBox1.Controls.Add(this.rbJT4);
			this.groupBox1.Controls.Add(this.rbFT8);
			this.groupBox1.Location = new System.Drawing.Point(261, 317);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(106, 124);
			this.groupBox1.TabIndex = 45;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Mode";
			// 
			// rbMSK144
			// 
			this.rbMSK144.AutoSize = true;
			this.rbMSK144.Location = new System.Drawing.Point(18, 97);
			this.rbMSK144.Name = "rbMSK144";
			this.rbMSK144.Size = new System.Drawing.Size(79, 21);
			this.rbMSK144.TabIndex = 4;
			this.rbMSK144.Text = "MSK144";
			this.rbMSK144.UseVisualStyleBackColor = true;
			this.rbMSK144.CheckedChanged += new System.EventHandler(this.RbMode_CheckedChanged);
			// 
			// rbJT65
			// 
			this.rbJT65.AutoSize = true;
			this.rbJT65.Location = new System.Drawing.Point(19, 78);
			this.rbJT65.Name = "rbJT65";
			this.rbJT65.Size = new System.Drawing.Size(58, 21);
			this.rbJT65.TabIndex = 3;
			this.rbJT65.Text = "JT65";
			this.rbJT65.UseVisualStyleBackColor = true;
			this.rbJT65.CheckedChanged += new System.EventHandler(this.RbMode_CheckedChanged);
			// 
			// rbJT9
			// 
			this.rbJT9.AutoSize = true;
			this.rbJT9.Location = new System.Drawing.Point(19, 59);
			this.rbJT9.Name = "rbJT9";
			this.rbJT9.Size = new System.Drawing.Size(50, 21);
			this.rbJT9.TabIndex = 2;
			this.rbJT9.Text = "JT9";
			this.rbJT9.UseVisualStyleBackColor = true;
			this.rbJT9.CheckedChanged += new System.EventHandler(this.RbMode_CheckedChanged);
			// 
			// rbJT4
			// 
			this.rbJT4.AutoSize = true;
			this.rbJT4.Location = new System.Drawing.Point(19, 40);
			this.rbJT4.Name = "rbJT4";
			this.rbJT4.Size = new System.Drawing.Size(50, 21);
			this.rbJT4.TabIndex = 1;
			this.rbJT4.Text = "JT4";
			this.rbJT4.UseVisualStyleBackColor = true;
			this.rbJT4.CheckedChanged += new System.EventHandler(this.RbMode_CheckedChanged);
			// 
			// rbFT8
			// 
			this.rbFT8.AutoSize = true;
			this.rbFT8.Checked = true;
			this.rbFT8.Location = new System.Drawing.Point(19, 23);
			this.rbFT8.Name = "rbFT8";
			this.rbFT8.Size = new System.Drawing.Size(51, 21);
			this.rbFT8.TabIndex = 0;
			this.rbFT8.TabStop = true;
			this.rbFT8.Text = "FT8";
			this.rbFT8.UseVisualStyleBackColor = true;
			this.rbFT8.CheckedChanged += new System.EventHandler(this.RbMode_CheckedChanged);
			// 
			// btnClearList
			// 
			this.btnClearList.Location = new System.Drawing.Point(47, 392);
			this.btnClearList.Name = "btnClearList";
			this.btnClearList.Size = new System.Drawing.Size(88, 33);
			this.btnClearList.TabIndex = 43;
			this.btnClearList.Text = "Clear List";
			this.btnClearList.UseVisualStyleBackColor = true;
			this.btnClearList.Click += new System.EventHandler(this.btnClearList_Click);
			// 
			// btnExit
			// 
			this.btnExit.Location = new System.Drawing.Point(155, 392);
			this.btnExit.Name = "btnExit";
			this.btnExit.Size = new System.Drawing.Size(88, 33);
			this.btnExit.TabIndex = 35;
			this.btnExit.Text = "Exit";
			this.btnExit.UseVisualStyleBackColor = true;
			this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.ItemHeight = 16;
			this.listBox1.Location = new System.Drawing.Point(35, 47);
			this.listBox1.Name = "listBox1";
			this.listBox1.ScrollAlwaysVisible = true;
			this.listBox1.Size = new System.Drawing.Size(348, 260);
			this.listBox1.TabIndex = 33;
			// 
			// lblRunStatus
			// 
			this.lblRunStatus.BackColor = System.Drawing.Color.Red;
			this.lblRunStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblRunStatus.Location = new System.Drawing.Point(155, 337);
			this.lblRunStatus.Name = "lblRunStatus";
			this.lblRunStatus.Size = new System.Drawing.Size(88, 33);
			this.lblRunStatus.TabIndex = 32;
			this.lblRunStatus.Text = "Stopped";
			this.lblRunStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// btnRun
			// 
			this.btnRun.Enabled = false;
			this.btnRun.Location = new System.Drawing.Point(47, 337);
			this.btnRun.Name = "btnRun";
			this.btnRun.Size = new System.Drawing.Size(88, 33);
			this.btnRun.TabIndex = 31;
			this.btnRun.Text = "Run";
			this.btnRun.UseVisualStyleBackColor = true;
			this.btnRun.Click += new System.EventHandler(this.BtnRun_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(126, 17);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(131, 17);
			this.label3.TabIndex = 34;
			this.label3.Text = "DX Callsigns Found";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.label5);
			this.tabPage2.Controls.Add(this.txtThreshold);
			this.tabPage2.Controls.Add(this.label4);
			this.tabPage2.Controls.Add(this.chkQuietMode);
			this.tabPage2.Controls.Add(this.chkAutoStart);
			this.tabPage2.Controls.Add(this.chkLogHits);
			this.tabPage2.Controls.Add(this.chkSendText);
			this.tabPage2.Controls.Add(this.chkSendEmail);
			this.tabPage2.Controls.Add(this.chkUnconfirmedAlert);
			this.tabPage2.Controls.Add(this.btnFindFile);
			this.tabPage2.Controls.Add(this.label2);
			this.tabPage2.Controls.Add(this.txtDirPath);
			this.tabPage2.Controls.Add(this.label1);
			this.tabPage2.Controls.Add(this.btnFindXMLlog);
			this.tabPage2.Controls.Add(this.txtXMLlogLocation);
			this.tabPage2.Location = new System.Drawing.Point(4, 25);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(419, 451);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Settings";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(267, 202);
			this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(24, 17);
			this.label5.TabIndex = 52;
			this.label5.Text = "db";
			// 
			// txtThreshold
			// 
			this.txtThreshold.Location = new System.Drawing.Point(198, 199);
			this.txtThreshold.Name = "txtThreshold";
			this.txtThreshold.Size = new System.Drawing.Size(53, 23);
			this.txtThreshold.TabIndex = 51;
			this.txtThreshold.Text = "-17";
			this.txtThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			this.txtThreshold.TextChanged += new System.EventHandler(this.txtThreshold_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(72, 202);
			this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(113, 17);
			this.label4.TabIndex = 50;
			this.label4.Text = "Cutoff Threshold";
			// 
			// chkQuietMode
			// 
			this.chkQuietMode.AutoSize = true;
			this.chkQuietMode.Location = new System.Drawing.Point(213, 267);
			this.chkQuietMode.Name = "chkQuietMode";
			this.chkQuietMode.Size = new System.Drawing.Size(100, 21);
			this.chkQuietMode.TabIndex = 49;
			this.chkQuietMode.Text = "Quiet Mode";
			this.chkQuietMode.UseVisualStyleBackColor = true;
			// 
			// chkAutoStart
			// 
			this.chkAutoStart.AutoSize = true;
			this.chkAutoStart.Location = new System.Drawing.Point(213, 364);
			this.chkAutoStart.Name = "chkAutoStart";
			this.chkAutoStart.Size = new System.Drawing.Size(84, 21);
			this.chkAutoStart.TabIndex = 48;
			this.chkAutoStart.Text = "Autostart";
			this.chkAutoStart.UseVisualStyleBackColor = true;
			// 
			// chkLogHits
			// 
			this.chkLogHits.Checked = true;
			this.chkLogHits.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkLogHits.Location = new System.Drawing.Point(52, 347);
			this.chkLogHits.Name = "chkLogHits";
			this.chkLogHits.Size = new System.Drawing.Size(100, 55);
			this.chkLogHits.TabIndex = 47;
			this.chkLogHits.Text = "Log Found Callsigns";
			this.chkLogHits.UseVisualStyleBackColor = true;
			// 
			// chkSendText
			// 
			this.chkSendText.AutoSize = true;
			this.chkSendText.Enabled = false;
			this.chkSendText.Location = new System.Drawing.Point(52, 315);
			this.chkSendText.Name = "chkSendText";
			this.chkSendText.Size = new System.Drawing.Size(124, 21);
			this.chkSendText.TabIndex = 45;
			this.chkSendText.Text = "Send SMS Text";
			this.chkSendText.UseVisualStyleBackColor = true;
			// 
			// chkSendEmail
			// 
			this.chkSendEmail.AutoSize = true;
			this.chkSendEmail.Location = new System.Drawing.Point(213, 315);
			this.chkSendEmail.Name = "chkSendEmail";
			this.chkSendEmail.Size = new System.Drawing.Size(98, 21);
			this.chkSendEmail.TabIndex = 46;
			this.chkSendEmail.Text = "Send Email";
			this.chkSendEmail.UseVisualStyleBackColor = true;
			// 
			// chkUnconfirmedAlert
			// 
			this.chkUnconfirmedAlert.Checked = true;
			this.chkUnconfirmedAlert.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkUnconfirmedAlert.Location = new System.Drawing.Point(52, 246);
			this.chkUnconfirmedAlert.Name = "chkUnconfirmedAlert";
			this.chkUnconfirmedAlert.Size = new System.Drawing.Size(111, 62);
			this.chkUnconfirmedAlert.TabIndex = 43;
			this.chkUnconfirmedAlert.Text = "Alert On Unconfirmed QSO\'s";
			this.chkUnconfirmedAlert.UseVisualStyleBackColor = true;
			// 
			// btnFindFile
			// 
			this.btnFindFile.Location = new System.Drawing.Point(292, 31);
			this.btnFindFile.Name = "btnFindFile";
			this.btnFindFile.Size = new System.Drawing.Size(75, 23);
			this.btnFindFile.TabIndex = 39;
			this.btnFindFile.Text = "Find";
			this.btnFindFile.UseVisualStyleBackColor = true;
			this.btnFindFile.Click += new System.EventHandler(this.BtnFindFile_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(118, 114);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(122, 17);
			this.label2.TabIndex = 40;
			this.label2.Text = "XML Log Location";
			// 
			// txtDirPath
			// 
			this.txtDirPath.Location = new System.Drawing.Point(19, 62);
			this.txtDirPath.Name = "txtDirPath";
			this.txtDirPath.Size = new System.Drawing.Size(369, 23);
			this.txtDirPath.TabIndex = 38;
			this.txtDirPath.Text = "C:\\Users\\chuck.HOMESITE2\\AppData\\Local\\WSJT-X";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(118, 34);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(122, 17);
			this.label1.TabIndex = 37;
			this.label1.Text = "ALL.TXT Location";
			// 
			// btnFindXMLlog
			// 
			this.btnFindXMLlog.Location = new System.Drawing.Point(292, 111);
			this.btnFindXMLlog.Name = "btnFindXMLlog";
			this.btnFindXMLlog.Size = new System.Drawing.Size(75, 23);
			this.btnFindXMLlog.TabIndex = 42;
			this.btnFindXMLlog.Text = "Find";
			this.btnFindXMLlog.UseVisualStyleBackColor = true;
			this.btnFindXMLlog.Click += new System.EventHandler(this.BtnFindXMLlog_Click);
			// 
			// txtXMLlogLocation
			// 
			this.txtXMLlogLocation.Location = new System.Drawing.Point(19, 142);
			this.txtXMLlogLocation.Name = "txtXMLlogLocation";
			this.txtXMLlogLocation.Size = new System.Drawing.Size(369, 23);
			this.txtXMLlogLocation.TabIndex = 41;
			this.txtXMLlogLocation.Text = "\\\\WIZARD2\\XML log\\ad0qk.log";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsStatusLabel,
            this.toolStripStatusLabel2});
			this.statusStrip1.Location = new System.Drawing.Point(0, 480);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(427, 26);
			this.statusStrip1.SizingGrip = false;
			this.statusStrip1.TabIndex = 38;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// tsStatusLabel
			// 
			this.tsStatusLabel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.tsStatusLabel.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.tsStatusLabel.Name = "tsStatusLabel";
			this.tsStatusLabel.Size = new System.Drawing.Size(326, 21);
			this.tsStatusLabel.Spring = true;
			this.tsStatusLabel.Text = "Fetching Files ...";
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
			this.toolStripStatusLabel2.BorderStyle = System.Windows.Forms.Border3DStyle.SunkenOuter;
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Padding = new System.Windows.Forms.Padding(7, 2, 10, 2);
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(86, 21);
			this.toolStripStatusLabel2.Text = "Alerter V2.0";
			// 
			// FrmDXAlerterMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(427, 506);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.tabControl1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4);
			this.MaximizeBox = false;
			this.Name = "FrmDXAlerterMain";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "WSJT-X DX Alerter";
			this.Load += new System.EventHandler(this.FrmDXAlerterMain_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Button btnClearList;
		private System.Windows.Forms.Button btnExit;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.Label lblRunStatus;
		private System.Windows.Forms.Button btnRun;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.CheckBox chkQuietMode;
		private System.Windows.Forms.CheckBox chkAutoStart;
		private System.Windows.Forms.CheckBox chkLogHits;
		private System.Windows.Forms.CheckBox chkSendText;
		private System.Windows.Forms.CheckBox chkSendEmail;
		private System.Windows.Forms.CheckBox chkUnconfirmedAlert;
		private System.Windows.Forms.Button btnFindFile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtDirPath;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnFindXMLlog;
		private System.Windows.Forms.TextBox txtXMLlogLocation;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel tsStatusLabel;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbMSK144;
		private System.Windows.Forms.RadioButton rbJT65;
		private System.Windows.Forms.RadioButton rbJT9;
		private System.Windows.Forms.RadioButton rbJT4;
		private System.Windows.Forms.RadioButton rbFT8;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtThreshold;
		private System.Windows.Forms.Label label4;

		#endregion

#if DEBUG
#endif
	}
}

