using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace FWBS.OMS.OMSEXPORT
{
	/// <summary>
	/// The About Box.
	/// </summary>
	internal class frmAbout : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox picLogo2;
		private System.Windows.Forms.Button btnSysInfo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label lblServiceControllerVersion;
        private System.Windows.Forms.Label lblServiceComponentVersion;
		private System.Windows.Forms.Label lblServiceBaseVersion;
        private Label lblCopyright;
        private System.Windows.Forms.Button btnOK;

		public frmAbout()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{

		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            this.picLogo2 = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnSysInfo = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblServiceControllerVersion = new System.Windows.Forms.Label();
            this.lblServiceComponentVersion = new System.Windows.Forms.Label();
            this.lblServiceBaseVersion = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo2)).BeginInit();
            this.SuspendLayout();
            // 
            // picLogo2
            // 
            this.picLogo2.BackColor = System.Drawing.Color.White;
            this.picLogo2.Image = ((System.Drawing.Image)(resources.GetObject("picLogo2.Image")));
            this.picLogo2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picLogo2.Location = new System.Drawing.Point(3, 1);
            this.picLogo2.Name = "picLogo2";
            this.picLogo2.Size = new System.Drawing.Size(389, 89);
            this.picLogo2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo2.TabIndex = 41;
            this.picLogo2.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(407, 6);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(109, 26);
            this.btnOK.TabIndex = 42;
            this.btnOK.Text = "OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnSysInfo
            // 
            this.btnSysInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSysInfo.Location = new System.Drawing.Point(407, 39);
            this.btnSysInfo.Name = "btnSysInfo";
            this.btnSysInfo.Size = new System.Drawing.Size(109, 27);
            this.btnSysInfo.TabIndex = 43;
            this.btnSysInfo.Text = "System Info...";
            this.btnSysInfo.Click += new System.EventHandler(this.btnSysInfo_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 104);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 28);
            this.label1.TabIndex = 44;
            this.label1.Text = "Service Controller Version:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 132);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 28);
            this.label2.TabIndex = 45;
            this.label2.Text = "Service Component Version:";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 28);
            this.label3.TabIndex = 46;
            this.label3.Text = "Service Base Version:";
            // 
            // lblServiceControllerVersion
            // 
            this.lblServiceControllerVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceControllerVersion.Location = new System.Drawing.Point(240, 104);
            this.lblServiceControllerVersion.Name = "lblServiceControllerVersion";
            this.lblServiceControllerVersion.Size = new System.Drawing.Size(192, 28);
            this.lblServiceControllerVersion.TabIndex = 47;
            this.lblServiceControllerVersion.Text = "(none)";
            // 
            // lblServiceComponentVersion
            // 
            this.lblServiceComponentVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceComponentVersion.Location = new System.Drawing.Point(240, 132);
            this.lblServiceComponentVersion.Name = "lblServiceComponentVersion";
            this.lblServiceComponentVersion.Size = new System.Drawing.Size(192, 28);
            this.lblServiceComponentVersion.TabIndex = 48;
            this.lblServiceComponentVersion.Text = "(none)";
            // 
            // lblServiceBaseVersion
            // 
            this.lblServiceBaseVersion.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServiceBaseVersion.Location = new System.Drawing.Point(240, 160);
            this.lblServiceBaseVersion.Name = "lblServiceBaseVersion";
            this.lblServiceBaseVersion.Size = new System.Drawing.Size(192, 28);
            this.lblServiceBaseVersion.TabIndex = 49;
            this.lblServiceBaseVersion.Text = "(none)";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoSize = true;
            this.lblCopyright.Location = new System.Drawing.Point(12, 222);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(60, 15);
            this.lblCopyright.TabIndex = 50;
            this.lblCopyright.Text = "Copyright";
            // 
            // frmAbout
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(528, 246);
            this.Controls.Add(this.lblCopyright);
            this.Controls.Add(this.lblServiceBaseVersion);
            this.Controls.Add(this.lblServiceComponentVersion);
            this.Controls.Add(this.lblServiceControllerVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSysInfo);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.picLogo2);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion



		private void btnSysInfo_Click(object sender, System.EventArgs e)
		{
            Process.Start(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "MSInfo32.exe"));
		}

		

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void frmAbout_Load(object sender, System.EventArgs e)
		{
			lblServiceComponentVersion.Text = GetComponentVersion();
			lblServiceControllerVersion.Text = GetControllerVersion();
			lblServiceBaseVersion.Text = GetBaseVersion();
			lblCopyright.Text = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyCopyrightAttribute>()?.Copyright;
		}
	
		private string GetComponentVersion()
		{
			string sret = "";
			string strPath = Application.StartupPath;
			
			try
			{
				strPath += @"\OMSExportService.exe";
			
				FileVersionInfo info = FileVersionInfo.GetVersionInfo(strPath);
            
				sret = info.FileVersion;
			}
			catch
			{
				sret = "File not found!";
			}
			return sret;
		}

		private string GetControllerVersion()
		{
			string sret = "";
			sret = Application.ProductVersion;
			return sret;
		}

		private string GetBaseVersion()
		{
			string sret = "";
			string strPath = Application.StartupPath;
			
			try
			{
				strPath += @"\OMSExport.dll";
			
				FileVersionInfo info = FileVersionInfo.GetVersionInfo(strPath);
            
				sret = info.FileVersion;
			}
			catch
			{
				sret = "File not found!";
			}
			return sret;
		}
		
		

	}
}
