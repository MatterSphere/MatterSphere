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
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ErrorProvider err;
		private System.Windows.Forms.PictureBox picLock;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.Windows.Forms.Timer timSetfocus;
        private Label lblLicenseMissing;
        private Common.UI.Windows.eXPFrame2 PnlFrame;
        private Common.UI.Windows.eInformation2 txtHint;
        internal TextBox txtPassword;
        private Label lblPassword;
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

        public frmPasswordRequest(string licensemissing) : this()
        {
            MissingLicenseMessage(licensemissing);
        }


        private void MissingLicenseMessage(string licensemissing)
        {
            this.lblLicenseMissing.Text = "The " + licensemissing + " license is missing.";
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.picLock = new System.Windows.Forms.PictureBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtHint = new FWBS.Common.UI.Windows.eInformation2();
            this.timSetfocus = new System.Windows.Forms.Timer(this.components);
            this.lblLicenseMissing = new System.Windows.Forms.Label();
            this.PnlFrame = new FWBS.Common.UI.Windows.eXPFrame2();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLock)).BeginInit();
            this.PnlFrame.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(8, 31);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCANCEL", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(8, 0);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 24);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // picLock
            // 
            this.picLock.Location = new System.Drawing.Point(8, 8);
            this.picLock.Name = "picLock";
            this.picLock.Size = new System.Drawing.Size(48, 48);
            this.picLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLock.TabIndex = 44;
            this.picLock.TabStop = false;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(9, 55);
            this.resourceLookup1.SetLookup(this.lblPassword, new FWBS.OMS.UI.Windows.ResourceLookupItem("PASSWORD", "Password", ""));
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(60, 20);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtHint
            // 
            this.txtHint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHint.BackColor = System.Drawing.Color.WhiteSmoke;
            this.txtHint.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(222)))), ((int)(((byte)(214)))));
            this.txtHint.ForeColor = System.Drawing.Color.Black;
            this.txtHint.Location = new System.Drawing.Point(12, 87);
            this.resourceLookup1.SetLookup(this.txtHint, new FWBS.OMS.UI.Windows.ResourceLookupItem("txtHint", "Enter the password for support access to the SDK", ""));
            this.txtHint.Name = "txtHint";
            this.txtHint.Padding = new System.Windows.Forms.Padding(1);
            this.txtHint.Size = new System.Drawing.Size(317, 45);
            this.txtHint.TabIndex = 4;
            this.txtHint.TabStop = false;
            this.txtHint.Text = "Today\'s Password is available from the Partner Support Web Site";
            // 
            // timSetfocus
            // 
            this.timSetfocus.Tick += new System.EventHandler(this.timSetfocus_Tick);
            // 
            // lblLicenseMissing
            // 
            this.lblLicenseMissing.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLicenseMissing.ForeColor = System.Drawing.Color.Red;
            this.lblLicenseMissing.Location = new System.Drawing.Point(12, 30);
            this.lblLicenseMissing.Name = "lblLicenseMissing";
            this.lblLicenseMissing.Size = new System.Drawing.Size(317, 18);
            this.lblLicenseMissing.TabIndex = 47;
            // 
            // PnlFrame
            // 
            this.PnlFrame.Controls.Add(this.lblLicenseMissing);
            this.PnlFrame.Controls.Add(this.txtHint);
            this.PnlFrame.Controls.Add(this.txtPassword);
            this.PnlFrame.Controls.Add(this.lblPassword);
            this.PnlFrame.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlFrame.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.PnlFrame.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.PnlFrame.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(218)))), ((int)(((byte)(222)))), ((int)(((byte)(214)))));
            this.PnlFrame.Location = new System.Drawing.Point(64, 8);
            this.PnlFrame.Name = "PnlFrame";
            this.PnlFrame.Size = new System.Drawing.Size(344, 145);
            this.PnlFrame.TabIndex = 48;
            this.PnlFrame.Text = " Partner Access to the 3E MatterSphere Framework SDK";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(73, 54);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(256, 23);
            this.txtPassword.TabIndex = 3;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlButtons.Location = new System.Drawing.Point(408, 8);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(84, 145);
            this.pnlButtons.TabIndex = 49;
            // 
            // frmPasswordRequest
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(500, 161);
            this.ControlBox = false;
            this.Controls.Add(this.PnlFrame);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.picLock);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("PASSWORDREQ", "Password Requst...", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPasswordRequest";
            this.Padding = new System.Windows.Forms.Padding(64, 8, 8, 8);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Password Request...";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPasswordRequest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLock)).EndInit();
            this.PnlFrame.ResumeLayout(false);
            this.PnlFrame.PerformLayout();
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

		private void frmPasswordRequest_Load(object sender, System.EventArgs e)
		{
			picLock.Image = FWBS.OMS.UI.Windows.Images.Lock;
		}
		#endregion

		private void txtPassword_TextChanged(object sender, System.EventArgs e)
		{
		
		}

		private void timSetfocus_Tick(object sender, System.EventArgs e)
		{
			timSetfocus.Enabled = false;
			txtPassword.Focus();
		}

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

    }
}
