using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A simple form that displays a password dialog form that needs a password within
    /// a process to continue.
    /// </summary>
    internal class frmSetPassword : BaseForm
	{
		#region Fields

		/// <summary>
		/// The passworded object.
		/// </summary>
		private PasswordProtectedBase _obj = null;

		#endregion

		#region Controls

		internal System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ErrorProvider err;
		private System.Windows.Forms.PictureBox picLock;
		private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.Windows.Forms.Label lblObjectInfo;
		private System.Windows.Forms.Label lblConfirm;
		private System.Windows.Forms.Label lblHint;
		private System.Windows.Forms.TextBox txtConfirm;
		internal System.Windows.Forms.TextBox txtHint;
		internal System.Windows.Forms.TextBox txtOldPassword;
		private System.Windows.Forms.Label labOldPass;
		private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel panel;
        private System.ComponentModel.IContainer components;

		#endregion

		#region Constructors & Destructors

		internal frmSetPassword() : base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			TopMost = false;
		}

		/// <summary>
		/// Constrcuts the form with the password object to validate against.
		/// </summary>
		/// <param name="obj"></param>
		internal frmSetPassword(PasswordProtectedBase obj) : this()
		{
			_obj = obj;
			labOldPass.Visible=true;
			txtOldPassword.Visible=true;
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
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.picLock = new System.Windows.Forms.PictureBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.lblObjectInfo = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblConfirm = new System.Windows.Forms.Label();
            this.lblHint = new System.Windows.Forms.Label();
            this.labOldPass = new System.Windows.Forms.Label();
            this.txtHint = new System.Windows.Forms.TextBox();
            this.txtConfirm = new System.Windows.Forms.TextBox();
            this.txtOldPassword = new System.Windows.Forms.TextBox();
            this.panel = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLock)).BeginInit();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(183, 78);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(130, 23);
            this.txtPassword.TabIndex = 1;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(64, 78);
            this.resourceLookup1.SetLookup(this.lblPassword, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblPassword", "Password:", ""));
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(115, 23);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(338, 37);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cance&l";
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(338, 8);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
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
            // lblObjectInfo
            // 
            this.lblObjectInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblObjectInfo.Location = new System.Drawing.Point(64, 8);
            this.resourceLookup1.SetLookup(this.lblObjectInfo, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblObjectInfo", "Please fill in the Password Details below", ""));
            this.lblObjectInfo.Name = "lblObjectInfo";
            this.lblObjectInfo.Size = new System.Drawing.Size(249, 23);
            this.lblObjectInfo.TabIndex = 46;
            this.lblObjectInfo.Text = "Please fill in the Password Details below";
            this.lblObjectInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClear
            // 
            this.btnClear.Enabled = false;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClear.Location = new System.Drawing.Point(338, 107);
            this.resourceLookup1.SetLookup(this.btnClear, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnClear", "&Clear", ""));
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "&Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // lblConfirm
            // 
            this.lblConfirm.Location = new System.Drawing.Point(64, 107);
            this.resourceLookup1.SetLookup(this.lblConfirm, new FWBS.OMS.UI.Windows.ResourceLookupItem("confPass", "Confirm Password : ", ""));
            this.lblConfirm.Name = "lblConfirm";
            this.lblConfirm.Size = new System.Drawing.Size(115, 23);
            this.lblConfirm.TabIndex = 48;
            this.lblConfirm.Text = "Confirm Password : ";
            this.lblConfirm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHint
            // 
            this.lblHint.Location = new System.Drawing.Point(11, 138);
            this.resourceLookup1.SetLookup(this.lblHint, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblPassHint", "Password Hint : ", ""));
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(92, 23);
            this.lblHint.TabIndex = 49;
            this.lblHint.Text = "Password Hint : ";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labOldPass
            // 
            this.labOldPass.Location = new System.Drawing.Point(64, 48);
            this.resourceLookup1.SetLookup(this.labOldPass, new FWBS.OMS.UI.Windows.ResourceLookupItem("labOldPass", "Old Password:", ""));
            this.labOldPass.Name = "labOldPass";
            this.labOldPass.Size = new System.Drawing.Size(115, 23);
            this.labOldPass.TabIndex = 52;
            this.labOldPass.Text = "Old Password:";
            this.labOldPass.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labOldPass.Visible = false;
            // 
            // txtHint
            // 
            this.txtHint.Location = new System.Drawing.Point(109, 138);
            this.txtHint.Name = "txtHint";
            this.txtHint.Size = new System.Drawing.Size(303, 23);
            this.txtHint.TabIndex = 3;
            // 
            // txtConfirm
            // 
            this.txtConfirm.Location = new System.Drawing.Point(183, 107);
            this.txtConfirm.Name = "txtConfirm";
            this.txtConfirm.PasswordChar = '*';
            this.txtConfirm.Size = new System.Drawing.Size(130, 23);
            this.txtConfirm.TabIndex = 2;
            // 
            // txtOldPassword
            // 
            this.txtOldPassword.Location = new System.Drawing.Point(183, 48);
            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.PasswordChar = '*';
            this.txtOldPassword.Size = new System.Drawing.Size(130, 23);
            this.txtOldPassword.TabIndex = 0;
            this.txtOldPassword.Visible = false;
            this.txtOldPassword.TextChanged += new System.EventHandler(this.txtOldPassword_TextChanged);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.btnClear);
            this.panel.Controls.Add(this.picLock);
            this.panel.Controls.Add(this.btnCancel);
            this.panel.Controls.Add(this.lblObjectInfo);
            this.panel.Controls.Add(this.btnOK);
            this.panel.Controls.Add(this.labOldPass);
            this.panel.Controls.Add(this.txtOldPassword);
            this.panel.Controls.Add(this.lblPassword);
            this.panel.Controls.Add(this.txtPassword);
            this.panel.Controls.Add(this.lblConfirm);
            this.panel.Controls.Add(this.txtConfirm);
            this.panel.Controls.Add(this.lblHint);
            this.panel.Controls.Add(this.txtHint);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(8);
            this.panel.Size = new System.Drawing.Size(428, 171);
            this.panel.TabIndex = 54;
            // 
            // frmSetPassword
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(428, 171);
            this.ControlBox = false;
            this.Controls.Add(this.panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmSetPassword", "Set Password...", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSetPassword";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set Password...";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmPasswordRequest_Load);
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLock)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
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
			err.SetError(txtPassword, String.Empty);
			err.SetError(txtHint, String.Empty);
			try
			{
				if (_obj == null)
				{
					if (txtPassword.Text == txtConfirm.Text)
						DialogResult = DialogResult.OK;
					else
						err.SetError(txtPassword, Session.CurrentSession.Resources.GetMessage("PASSFAILED","The Confirm Password does not match the Password","").Text);
				}
				else
				{
					_obj.ChangePassword(txtOldPassword.Text,txtPassword.Text,txtConfirm.Text);
					DialogResult = DialogResult.OK;
				}
				if (txtHint.Text == "" && txtPassword.Text != "")
					err.SetError(txtHint, Session.CurrentSession.Resources.GetMessage("NOHINT","You must enter a Hint for the Password","").Text);
				else
					_obj.PasswordHint = txtHint.Text;
			}
			catch (Exception ex)
			{
				err.SetError(txtPassword, ex.Message);
			}
		}

        private void frmPasswordRequest_Load(object sender, System.EventArgs e)
		{
            picLock.Image = FWBS.OMS.UI.Windows.Images.Lock;
            txtHint.Text = _obj != null ? _obj.PasswordHint : string.Empty;
			txtOldPassword.Enabled = _obj != null ? _obj.HasPassword : false;
		}

		private void btnClear_Click(object sender, System.EventArgs e)
		{
			txtPassword.Text = "";
			txtConfirm.Text = "";
			txtHint.Text = "";
			btnOK_Click(sender,e);
		}

		private void txtOldPassword_TextChanged(object sender, System.EventArgs e)
		{
			btnClear.Enabled = (txtOldPassword.Text != "");
		}
		#endregion

	}
}
