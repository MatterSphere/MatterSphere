using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A simple form that displays a password dialog form that needs a password within
    /// a process to continue.
    /// </summary>
    public class frmPasswordRequest : BaseForm
	{
		#region Fields

		/// <summary>
		/// The passworded object.
		/// </summary>
		private FWBS.Common.IPasswordProtected _obj = null;
		private string _password = "";
		private string _hint = "";
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
		private System.Windows.Forms.Label lblUserName;
		internal System.Windows.Forms.TextBox txtUserName;
		private System.Windows.Forms.RadioButton rdoByPassword;
		private System.Windows.Forms.RadioButton rdoByUser;
		private FWBS.Common.UI.Windows.eXPFrame pnlAuth;
		private System.Windows.Forms.Label lblHint;
		private System.Windows.Forms.TextBox txtHint;
		private System.Windows.Forms.Timer timSetfocus;
        private Panel pnlButtons;
		private System.ComponentModel.IContainer components;

		#endregion

		#region Constructors & Destructors

		private frmPasswordRequest() : base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Constrcuts the form with the password object to validate against.
		/// </summary>
		/// <param name="obj"></param>
		internal frmPasswordRequest(FWBS.Common.IPasswordProtected obj) : this()
		{
			_obj = obj;
		}

		internal frmPasswordRequest(string Password, string Hint) : this()
		{
			if (Password == "")
				throw new OMSException2("ERPASSWORDBLANK", "Error password cannot be blank");
			_password = Password;
			_hint = Hint;

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
            this.lblUserName = new System.Windows.Forms.Label();
            this.rdoByPassword = new System.Windows.Forms.RadioButton();
            this.lblHint = new System.Windows.Forms.Label();
            this.pnlAuth = new FWBS.Common.UI.Windows.eXPFrame();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.rdoByUser = new System.Windows.Forms.RadioButton();
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
            this.txtPassword.Location = new System.Drawing.Point(100, 49);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(152, 23);
            this.txtPassword.TabIndex = 1;
            // 
            // lblPassword
            // 
            this.lblPassword.Location = new System.Drawing.Point(10, 49);
            this.resourceLookup1.SetLookup(this.lblPassword, new FWBS.OMS.UI.Windows.ResourceLookupItem("PASSWORD", "Password", ""));
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(74, 23);
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
            this.btnCancel.Size = new System.Drawing.Size(76, 24);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cance&l";
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(8, 8);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNOK", "&OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(76, 24);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtHint
            // 
            this.txtHint.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtHint.ForeColor = System.Drawing.Color.Red;
            this.txtHint.Location = new System.Drawing.Point(100, 23);
            this.txtHint.Multiline = true;
            this.txtHint.Name = "txtHint";
            this.txtHint.ReadOnly = true;
            this.txtHint.Size = new System.Drawing.Size(152, 21);
            this.txtHint.TabIndex = 0;
            // 
            // err
            // 
            this.err.ContainerControl = this;
            // 
            // picLock
            // 
            this.picLock.Location = new System.Drawing.Point(8, 9);
            this.picLock.Name = "picLock";
            this.picLock.Size = new System.Drawing.Size(39, 51);
            this.picLock.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLock.TabIndex = 44;
            this.picLock.TabStop = false;
            // 
            // lblUserName
            // 
            this.lblUserName.Location = new System.Drawing.Point(10, 22);
            this.resourceLookup1.SetLookup(this.lblUserName, new FWBS.OMS.UI.Windows.ResourceLookupItem("USERNAME", "User Name", ""));
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(74, 23);
            this.lblUserName.TabIndex = 48;
            this.lblUserName.Text = "User Name";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblUserName.Visible = false;
            // 
            // rdoByPassword
            // 
            this.rdoByPassword.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdoByPassword.Checked = true;
            this.rdoByPassword.Location = new System.Drawing.Point(8, 80);
            this.resourceLookup1.SetLookup(this.rdoByPassword, new FWBS.OMS.UI.Windows.ResourceLookupItem("BYPASSWORD", "By Password", ""));
            this.rdoByPassword.Name = "rdoByPassword";
            this.rdoByPassword.Size = new System.Drawing.Size(104, 20);
            this.rdoByPassword.TabIndex = 2;
            this.rdoByPassword.TabStop = true;
            this.rdoByPassword.Text = "By Password";
            this.rdoByPassword.CheckedChanged += new System.EventHandler(this.rdoByPassword_CheckedChanged);
            // 
            // lblHint
            // 
            this.lblHint.Location = new System.Drawing.Point(10, 22);
            this.resourceLookup1.SetLookup(this.lblHint, new FWBS.OMS.UI.Windows.ResourceLookupItem("HINT", "Hint", ""));
            this.lblHint.Name = "lblHint";
            this.lblHint.Size = new System.Drawing.Size(39, 23);
            this.lblHint.TabIndex = 49;
            this.lblHint.Text = "Hint";
            this.lblHint.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlAuth
            // 
            this.pnlAuth.Controls.Add(this.txtHint);
            this.pnlAuth.Controls.Add(this.lblHint);
            this.pnlAuth.Controls.Add(this.txtPassword);
            this.pnlAuth.Controls.Add(this.txtUserName);
            this.pnlAuth.Controls.Add(this.lblPassword);
            this.pnlAuth.Controls.Add(this.lblUserName);
            this.pnlAuth.Controls.Add(this.rdoByPassword);
            this.pnlAuth.Controls.Add(this.rdoByUser);
            this.pnlAuth.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlAuth.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.pnlAuth.FrameBackColor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.pnlAuth.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.pnlAuth.Location = new System.Drawing.Point(51, 43);
            this.resourceLookup1.SetLookup(this.pnlAuth, new FWBS.OMS.UI.Windows.ResourceLookupItem("AUTHENTICATE", "Authenticate", ""));
            this.pnlAuth.Name = "pnlAuth";
            this.pnlAuth.Size = new System.Drawing.Size(273, 109);
            this.pnlAuth.TabIndex = 0;
            this.pnlAuth.Text = "Authenticate";
            // 
            // txtUserName
            // 
            this.txtUserName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtUserName.Location = new System.Drawing.Point(100, 22);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(152, 23);
            this.txtUserName.TabIndex = 0;
            this.txtUserName.Visible = false;
            // 
            // rdoByUser
            // 
            this.rdoByUser.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rdoByUser.Location = new System.Drawing.Point(161, 80);
            this.resourceLookup1.SetLookup(this.rdoByUser, new FWBS.OMS.UI.Windows.ResourceLookupItem("rdoByUser", "By User", ""));
            this.rdoByUser.Name = "rdoByUser";
            this.rdoByUser.Size = new System.Drawing.Size(85, 20);
            this.rdoByUser.TabIndex = 3;
            this.rdoByUser.Text = "By User";
            this.rdoByUser.CheckedChanged += new System.EventHandler(this.rdoByPassword_CheckedChanged);
            // 
            // lblObjectInfo
            // 
            this.lblObjectInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblObjectInfo.Location = new System.Drawing.Point(51, 9);
            this.lblObjectInfo.Name = "lblObjectInfo";
            this.lblObjectInfo.Size = new System.Drawing.Size(273, 31);
            this.lblObjectInfo.TabIndex = 46;
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
            this.pnlButtons.Location = new System.Drawing.Point(322, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(92, 165);
            this.pnlButtons.TabIndex = 47;
            // 
            // frmPasswordRequest
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(414, 165);
            this.ControlBox = false;
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.lblObjectInfo);
            this.Controls.Add(this.picLock);
            this.Controls.Add(this.pnlAuth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("PASSFORM", "Password Request...", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmPasswordRequest";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Password Request...";
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
			err.SetError(txtUserName, String.Empty);
			err.SetError(txtPassword, String.Empty);
			if (_obj != null)
			{
				try
				{
					if (rdoByPassword.Checked)
					{
						_obj.CurrentPassword = txtPassword.Text;
						_obj.ValidatePassword();
					}
					else
					{
						_obj.PasswordAuthenticate(txtUserName.Text, txtPassword.Text);
					}
					Cursor = Cursors.WaitCursor;
					DialogResult = DialogResult.OK;
				}
				catch (Security.ServiceUserException ex)
				{
					txtUserName.Focus();
					txtUserName.SelectAll();
					err.SetError(txtUserName, ex.Message);
					ErrorBox.Show(this, ex);
				}
				catch (Security.InactiveOMSUserException ex)
				{
					txtUserName.Focus();
					txtUserName.SelectAll();
					err.SetError(txtUserName, ex.Message);
					ErrorBox.Show(this, ex);
				}
				catch (Security.InvalidOMSPasswordException ex)
				{
					txtPassword.Focus();
					txtPassword.Clear();
					err.SetError(txtPassword, ex.Message);
					ErrorBox.Show(this, ex);
				}
				catch (Exception ex)
				{
					if (rdoByPassword.Checked)
					{
						txtPassword.Focus();
						txtPassword.Clear();
						err.SetError(txtPassword, ex.Message);
					}
					else
					{
						txtUserName.Focus();
						txtUserName.SelectAll();
						err.SetError(txtUserName, ex.Message);
					}
					ErrorBox.Show(this, ex);
				}
				finally
				{
					
				}
			}
			else if (_password != "")
			{
				try
				{
					if (rdoByPassword.Checked)
					{
						if (txtPassword.Text != _password)
							throw new Security.InvalidOMSPasswordException();
					}
					else
					{
						FWBS.OMS.User _authenticated = FWBS.OMS.User.AuthenticateUser(txtUserName.Text, txtPassword.Text);
					}
					Cursor = Cursors.WaitCursor;
					DialogResult = DialogResult.OK;
				}
				catch (Security.ServiceUserException ex)
				{
					txtUserName.Focus();
					txtUserName.SelectAll();
					err.SetError(txtUserName, ex.Message);
					ErrorBox.Show(this, ex);
				}
				catch (Security.InactiveOMSUserException ex)
				{
					txtUserName.Focus();
					txtUserName.SelectAll();
					err.SetError(txtUserName, ex.Message);
					ErrorBox.Show(this, ex);
				}
				catch (Security.InvalidOMSPasswordException ex)
				{
					txtPassword.Focus();
					txtPassword.Clear();
					err.SetError(txtPassword, ex.Message);
					ErrorBox.Show(this, ex);
				}
				catch (Exception ex)
				{
					if (rdoByPassword.Checked)
					{
						txtPassword.Focus();
						txtPassword.Clear();
						err.SetError(txtPassword, ex.Message);
					}
					else
					{
						txtUserName.Focus();
						txtUserName.SelectAll();
						err.SetError(txtUserName, ex.Message);
					}
					ErrorBox.Show(this, ex);
				}
				finally
				{
					
				}
			}
			else 
			{
				txtPassword.Clear();
				err.SetError(txtPassword, OMS.Session.CurrentSession.Resources.GetMessage("PASSNOT","The password does not match","").Text);
			}
		}

		private void frmPasswordRequest_Load(object sender, System.EventArgs e)
		{
            picLock.Image = Images.GetCoolButton(86, (Images.IconSize)LogicalToDeviceUnits(48)).ToBitmap();
			if (_obj != null)
			{
				string info = _obj.ToPasswordString();
				if (info.IndexOf(Environment.NewLine) > 0)
					info = info.Substring(0, info.IndexOf(Environment.NewLine));
				lblObjectInfo.Text = info;
				txtHint.Text = _obj.PasswordHint;
				if (!_obj.IsInternal)
				{
					rdoByUser.Visible = false;
					rdoByPassword.Visible = false;
				}
				Application.DoEvents();
				txtPassword.Focus();
				timSetfocus.Enabled = true;
			}
			else if (_password != "") 
			{
				txtHint.Text = _hint;
				lblObjectInfo.Text = Session.CurrentSession.Resources.GetResource("PASSUNLOCK","Please enter the password to unlock","").Text;
			}
		}

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            picLock.Image = Images.GetCoolButton(86, (Images.IconSize)LogicalToDeviceUnits(48)).ToBitmap();
        }

		private void rdoByPassword_CheckedChanged(object sender, System.EventArgs e)
		{
			if (rdoByUser.Checked)
			{
				lblUserName.Visible = true;
				txtUserName.Visible = true;
				lblHint.Visible = false;
				txtHint.Visible = false;
				txtUserName.Focus();
				txtUserName.SelectAll();
			}
			else
			{
				lblUserName.Visible = false;
				txtUserName.Visible = false;
				lblHint.Visible = true;
				txtHint.Visible = true;
				txtPassword.Focus();
				txtPassword.SelectAll();
			}
		}

		#endregion

		private void timSetfocus_Tick(object sender, System.EventArgs e)
		{
			timSetfocus.Enabled = false;
			Activate();
			txtPassword.Focus();
		}

	}
}
