using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// A simple form that displays a password dialog form that needs a password within
    /// a process to continue.
    /// </summary>
    public class frmPasswordRequest : FWBS.OMS.UI.Windows.BaseForm
	{
		#region Controls
		internal System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ErrorProvider err;
		private System.Windows.Forms.PictureBox picLock;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.Windows.Forms.Label lblObjectInfo;
		private FWBS.Common.UI.Windows.eXPFrame pnlAuth;
		private System.Windows.Forms.TextBox txtHint;
		private System.Windows.Forms.Timer timSetfocus;
        private Panel pnlButtons;
		private System.ComponentModel.IContainer components;
		#endregion

		#region Constructors & Destructors

		public frmPasswordRequest()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			Global.RightToLeftFormConverter(this);
			TopMost = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtHint = new System.Windows.Forms.TextBox();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.picLock = new System.Windows.Forms.PictureBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlAuth = new FWBS.Common.UI.Windows.eXPFrame();
            this.lblObjectInfo = new System.Windows.Forms.Label();
            this.timSetfocus = new System.Windows.Forms.Timer(this.components);
            this.pnlButtons = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLock)).BeginInit();
            this.pnlAuth.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(100, 70);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(159, 23);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(17, 70);
            this.resourceLookup1.SetLookup(this.lblPassword, new FWBS.OMS.UI.Windows.ResourceLookupItem("PASSWORD", "Password", ""));
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(72, 20);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(8, 38);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCANCEL", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(8, 8);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 24);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtHint
            // 
            this.txtHint.BackColor = System.Drawing.Color.White;
            this.txtHint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHint.ForeColor = System.Drawing.Color.Black;
            this.txtHint.Location = new System.Drawing.Point(0, 27);
            this.resourceLookup1.SetLookup(this.txtHint, new FWBS.OMS.UI.Windows.ResourceLookupItem("txtHint", "Enter the password for support access to the SDK", ""));
            this.txtHint.Multiline = true;
            this.txtHint.Name = "txtHint";
            this.txtHint.ReadOnly = true;
            this.txtHint.Size = new System.Drawing.Size(280, 29);
            this.txtHint.TabIndex = 0;
            this.txtHint.TabStop = false;
            this.txtHint.Text = "Enter the password for support access to the SDK";
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // picLock
            // 
            this.picLock.Location = new System.Drawing.Point(6, 8);
            this.picLock.Name = "picLock";
            this.picLock.Size = new System.Drawing.Size(38, 48);
            this.picLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLock.TabIndex = 44;
            this.picLock.TabStop = false;
            // 
            // pnlAuth
            // 
            this.pnlAuth.Controls.Add(this.txtHint);
            this.pnlAuth.Controls.Add(this.txtPassword);
            this.pnlAuth.Controls.Add(this.lblPassword);
            this.pnlAuth.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlAuth.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.pnlAuth.FrameBackColor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.pnlAuth.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.pnlAuth.Location = new System.Drawing.Point(58, 41);
            this.resourceLookup1.SetLookup(this.pnlAuth, new FWBS.OMS.UI.Windows.ResourceLookupItem("Authenticate", "Authenticate", ""));
            this.pnlAuth.Name = "pnlAuth";
            this.pnlAuth.Size = new System.Drawing.Size(280, 102);
            this.pnlAuth.TabIndex = 0;
            this.pnlAuth.Text = "Authenticate";
            // 
            // lblObjectInfo
            // 
            this.lblObjectInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblObjectInfo.Location = new System.Drawing.Point(58, 14);
            this.resourceLookup1.SetLookup(this.lblObjectInfo, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblObjectInfo", "Partner Access to SDK", ""));
            this.lblObjectInfo.Name = "lblObjectInfo";
            this.lblObjectInfo.Size = new System.Drawing.Size(280, 23);
            this.lblObjectInfo.TabIndex = 46;
            this.lblObjectInfo.Text = "Partner Access to SDK";
            // 
            // timSetfocus
            // 
            this.timSetfocus.Tick += new System.EventHandler(this.timSetfocus_Tick);
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(342, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(92, 154);
            this.pnlButtons.TabIndex = 47;
            // 
            // frmPasswordRequest
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(434, 154);
            this.ControlBox = false;
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.lblObjectInfo);
            this.Controls.Add(this.picLock);
            this.Controls.Add(this.pnlAuth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("PASSWORDREQ", "Password Request", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPasswordRequest";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Password Request";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPasswordRequest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLock)).EndInit();
            this.pnlAuth.ResumeLayout(false);
            this.pnlAuth.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

		}	   

	    #endregion


		#endregion

		#region Methods

		/// <summary>
		/// Validate the password on the click event of the OK button..
		/// </summary>
		/// <param name="sender">The OK button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void btnOK_Click(object sender, System.EventArgs e)
		{
            //UTCFIX: DM - 30/11/06 - No effect here unless dialing remotely into a computer a day behind
			string password = DateTime.Today.Year.ToString().Substring(0,2) + DateTime.Today.Day.ToString() + Session.CurrentSession.SerialNumber.ToString() + DateTime.Today.Month.ToString() + DateTime.Today.Year.ToString().Substring(2,2);
            if (txtPassword.Text == password || FWBS.Common.Security.Cryptography.Encryption.Encrypt(password) == txtPassword.Text)	
			{
				DialogResult = DialogResult.OK;
				Close();
			}
			else 
			{
				txtPassword.Clear();
				err.SetError(txtPassword, OMS.Session.CurrentSession.Resources.GetMessage("PASSNOT","The password does not match","").Text);
			}
		}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void frmPasswordRequest_Load(object sender, System.EventArgs e)
		{
            picLock.Image = Images.GetCoolButton(86, (Images.IconSize)LogicalToDeviceUnits(48)).ToBitmap();// FWBS.OMS.UI.Windows.Images.Lock;
		}

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            picLock.Image = Images.GetCoolButton(86, (Images.IconSize)LogicalToDeviceUnits(48)).ToBitmap();// FWBS.OMS.UI.Windows.Images.Lock;
        }

		private void txtPassword_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void timSetfocus_Tick(object sender, System.EventArgs e)
		{
			timSetfocus.Enabled = false;
			txtPassword.Focus();
		}

        #endregion
	}
}
