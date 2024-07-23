using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Reports
{
    /// <summary>
    /// Summary description for frmInputBox.
    /// </summary>
    internal class frmRSSettings : FWBS.OMS.UI.Windows.BaseForm
	{
		private System.Windows.Forms.PictureBox PictureBox;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		public System.Windows.Forms.Label Question;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox txtServer;
		private System.Windows.Forms.TextBox txtWebService;
        private Panel pnlBackground;

		internal frmRSSettings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            string server = FWBS.OMS.ReportingServices.SSRSConnect.Server;
            string webserver = FWBS.OMS.ReportingServices.SSRSConnect.WebServer;

            if (server != "")
				txtServer.Text = server;
			if (webserver != "")
				txtWebService.Text = webserver;
		}


		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRSSettings));
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.Question = new System.Windows.Forms.Label();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.PictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtWebService = new System.Windows.Forms.TextBox();
            this.pnlBackground = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).BeginInit();
            this.pnlBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(305, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 25);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(305, 36);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cance&l";
            // 
            // Question
            // 
            this.Question.Location = new System.Drawing.Point(50, 9);
            this.Question.Name = "Question";
            this.Question.Size = new System.Drawing.Size(240, 31);
            this.Question.TabIndex = 2;
            this.Question.Text = "Reporting Server Location";
            // 
            // txtServer
            // 
            this.txtServer.Location = new System.Drawing.Point(7, 70);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(373, 23);
            this.txtServer.TabIndex = 0;
            this.txtServer.Text = "http://reportserver/reports";
            // 
            // PictureBox
            // 
            this.PictureBox.Image = ((System.Drawing.Image)(resources.GetObject("PictureBox.Image")));
            this.PictureBox.Location = new System.Drawing.Point(10, 8);
            this.PictureBox.Name = "PictureBox";
            this.PictureBox.Size = new System.Drawing.Size(32, 32);
            this.PictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBox.TabIndex = 4;
            this.PictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(294, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Web Server Path (http://reportserver/reports)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(371, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Web Service Path (http://reportserver/reportserver/reportservice.asmx)";
            // 
            // txtWebService
            // 
            this.txtWebService.Location = new System.Drawing.Point(8, 120);
            this.txtWebService.Name = "txtWebService";
            this.txtWebService.Size = new System.Drawing.Size(373, 23);
            this.txtWebService.TabIndex = 6;
            this.txtWebService.Text = "http://reportserver/reportserver/reportservice.asmx";
            // 
            // pnlBackground
            // 
            this.pnlBackground.Controls.Add(this.label2);
            this.pnlBackground.Controls.Add(this.txtWebService);
            this.pnlBackground.Controls.Add(this.label1);
            this.pnlBackground.Controls.Add(this.PictureBox);
            this.pnlBackground.Controls.Add(this.txtServer);
            this.pnlBackground.Controls.Add(this.Question);
            this.pnlBackground.Controls.Add(this.btnCancel);
            this.pnlBackground.Controls.Add(this.btnOK);
            this.pnlBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBackground.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlBackground.Location = new System.Drawing.Point(0, 0);
            this.pnlBackground.Name = "pnlBackground";
            this.pnlBackground.Size = new System.Drawing.Size(388, 151);
            this.pnlBackground.TabIndex = 8;
            // 
            // frmRSSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(388, 151);
            this.Controls.Add(this.pnlBackground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRSSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Input Box";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox)).EndInit();
            this.pnlBackground.ResumeLayout(false);
            this.pnlBackground.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			try
			{
                FWBS.OMS.ReportingServices.SSRSConnect.SetReportingServerRegistryKeys(txtServer.Text, txtWebService.Text);
				DialogResult = DialogResult.OK;
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}
	}
}
