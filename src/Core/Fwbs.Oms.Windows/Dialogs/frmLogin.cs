using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using FWBS.OMS.Data;
using FWBS.OMS.Security;




namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// The main OMS.NET login form for the windows UI.
    /// </summary>
    internal class frmLogin : System.Windows.Forms.Form
	{
		#region Fields
		private int _orgheight = 0;
		private DatabaseConnections _connections = new DatabaseConnections(FWBS.OMS.Global.ApplicationName, FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey);


		private System.Windows.Forms.ErrorProvider err;
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.Panel pnlInfo;
		private System.Windows.Forms.Panel pnlMain;
		private System.Windows.Forms.Button cmdCancel;
		private FWBS.Common.UI.Windows.eXPFrame gbLogin;
		private System.Windows.Forms.Button cmdLogon;
        private System.Windows.Forms.Button cmdOptions;
		private System.Windows.Forms.Label lblCopyright;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label lblVersion;
		private System.Windows.Forms.CheckBox chkClearCache;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label lblMultiDB;
		private System.Windows.Forms.ComboBox cboMultiDB;
		private System.Windows.Forms.Label lblPassword;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;

		#endregion
        private PictureBox picLogo2;
        private IContainer components;

		#region Constructors & Destructors

		/// <summary>
		/// Initializes the default constructor of the login form.
		/// </summary>
		internal frmLogin()
		{
            this.BackColor = Color.White;
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
			Global.RightToLeftFormConverter(this);
            this.RightToLeftLayout = (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes);

            _orgheight = this.Height;
            this.Height -= pnlInfo.Height;
            pnlInfo.Visible = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogin));
            this.err = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblInfo = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.cboMultiDB = new System.Windows.Forms.ComboBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.gbLogin = new FWBS.Common.UI.Windows.eXPFrame();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblMultiDB = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdLogon = new System.Windows.Forms.Button();
            this.cmdOptions = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblVersion = new System.Windows.Forms.Label();
            this.chkClearCache = new System.Windows.Forms.CheckBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.picLogo2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            this.pnlInfo.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.gbLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo2)).BeginInit();
            this.SuspendLayout();
            // 
            // err
            // 
            this.err.ContainerControl = this;
            resources.ApplyResources(this.err, "err");
            // 
            // lblInfo
            // 
            resources.ApplyResources(this.lblInfo, "lblInfo");
            this.lblInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblInfo.ForeColor = System.Drawing.Color.Red;
            this.lblInfo.Name = "lblInfo";
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblInfo);
            resources.ApplyResources(this.pnlInfo, "pnlInfo");
            this.pnlInfo.Name = "pnlInfo";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.txtPassword);
            this.pnlMain.Controls.Add(this.cboMultiDB);
            this.pnlMain.Controls.Add(this.txtUser);
            this.pnlMain.Controls.Add(this.lblCopyright);
            this.pnlMain.Controls.Add(this.cmdCancel);
            this.pnlMain.Controls.Add(this.gbLogin);
            this.pnlMain.Controls.Add(this.cmdLogon);
            this.pnlMain.Controls.Add(this.cmdOptions);
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Name = "pnlMain";
            // 
            // txtPassword
            // 
            resources.ApplyResources(this.txtPassword, "txtPassword");
            this.txtPassword.Name = "txtPassword";
            // 
            // cboMultiDB
            // 
            resources.ApplyResources(this.cboMultiDB, "cboMultiDB");
            this.cboMultiDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMultiDB.Name = "cboMultiDB";
            this.cboMultiDB.SelectedIndexChanged += new System.EventHandler(this.cboMultiDB_SelectedIndexChanged);
            // 
            // txtUser
            // 
            resources.ApplyResources(this.txtUser, "txtUser");
            this.txtUser.Name = "txtUser";
            // 
            // lblCopyright
            // 
            resources.ApplyResources(this.lblCopyright, "lblCopyright");
            this.lblCopyright.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.UseMnemonic = false;
            // 
            // cmdCancel
            // 
            resources.ApplyResources(this.cmdCancel, "cmdCancel");
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.No;
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // gbLogin
            // 
            this.gbLogin.Controls.Add(this.lblPassword);
            this.gbLogin.Controls.Add(this.lblMultiDB);
            this.gbLogin.Controls.Add(this.label1);
            this.gbLogin.FontColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            this.gbLogin.FrameBackColor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.gbLogin.FrameForeColor = new FWBS.Common.UI.Windows.ExtColor(FWBS.Common.UI.Windows.ExtColorPresets.FrameLineForeColor, FWBS.Common.UI.Windows.ExtColorTheme.Auto);
            resources.ApplyResources(this.gbLogin, "gbLogin");
            this.gbLogin.Name = "gbLogin";
            this.gbLogin.Enter += new System.EventHandler(this.gbLogin_Enter);
            // 
            // lblPassword
            // 
            resources.ApplyResources(this.lblPassword, "lblPassword");
            this.lblPassword.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Click += new System.EventHandler(this.lblPassword_Click);
            // 
            // lblMultiDB
            // 
            resources.ApplyResources(this.lblMultiDB, "lblMultiDB");
            this.lblMultiDB.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblMultiDB.Name = "lblMultiDB";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Name = "label1";
            // 
            // cmdLogon
            // 
            resources.ApplyResources(this.cmdLogon, "cmdLogon");
            this.cmdLogon.Name = "cmdLogon";
            this.cmdLogon.Click += new System.EventHandler(this.cmdLogon_Click);
            // 
            // cmdOptions
            // 
            resources.ApplyResources(this.cmdOptions, "cmdOptions");
            this.cmdOptions.Name = "cmdOptions";
            this.cmdOptions.Click += new System.EventHandler(this.cmdOptions_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // lblVersion
            // 
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.Name = "lblVersion";
            // 
            // chkClearCache
            // 
            resources.ApplyResources(this.chkClearCache, "chkClearCache");
            this.chkClearCache.Name = "chkClearCache";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xml";
            resources.ApplyResources(this.openFileDialog1, "openFileDialog1");
            // 
            // picLogo2
            // 
            this.picLogo2.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.picLogo2, "picLogo2");
            this.picLogo2.Name = "picLogo2";
            this.picLogo2.TabStop = false;
            // 
            // frmLogin
            // 
            this.AcceptButton = this.cmdLogon;
            resources.ApplyResources(this, "$this");
            this.AutoScaleDimensions = new System.Drawing.SizeF(96.0F, 96.0F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cmdCancel;
            this.ControlBox = false;
            this.Controls.Add(this.chkClearCache);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.picLogo2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmLogin";
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            this.pnlInfo.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.gbLogin.ResumeLayout(false);
            this.gbLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo2)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region Methods

		/// <summary>
		/// Initiates the login procedure of the system.
		/// </summary>
		/// <param name="sender">Logon button object.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdLogon_Click(object sender, System.EventArgs e)
		{
			//Get a reference to the selected multi database settings.
			DatabaseSettings  db = (DatabaseSettings)cboMultiDB.SelectedItem;

			logon_retry:
			try
			{ 
				//Blank all error messages.
				ShowException(null);
				Cursor = Cursors.WaitCursor;
                DisableControls();
				err.SetError (txtUser, "");
				err.SetError (txtPassword, "");
				err.SetError (cboMultiDB, "");
				//Call the session logon method.  If the login is a success then the code
				//will continue its execution, otherwise one of a numerous exceptions will
				//be raised and captured below.
				Session.Login(db.Number, txtUser.Text, txtPassword.Text, chkClearCache.Checked);
				
				//Tell the user that the database version is different to the engine version.
				Version engver = Session.CurrentSession.EngineVersion;
				Version dbver = Session.CurrentSession.DatabaseVersion;
				Version mindbver = Session.CurrentSession.MinimumDatabaseVersion;
				if (mindbver > dbver )
				{
                    MessageBox.ShowInformation("VERSIONDIFFERS", "The database version '%2%' is less than the minimum database version '%1%'.  You may experience some problems.", mindbver.ToString(), dbver.ToString());
				}

				IWin32Window owner = Owner ?? Common.Functions.GetOwnerWindow(this) ?? Services.MainWindow;
				Services.ShowUserAutoWindows(owner);

				//Close the form on success and set the default database connection for next time.
				this.DialogResult = DialogResult.OK;
				_connections.Default = db;
				this.Close();

			}
			catch (InvalidOMSPasswordException ex)
			{
				//Display a message if the password was entered incorrectly.
				err.SetError(txtPassword, ex.Message);
				txtPassword.Focus();
				ShowException(ex);
				ErrorBox.Show(this, ex);
                Cursor = Cursors.Default;
            }
			catch (InvalidOMSUserException  ex)
			{
				//Display a message if the user does not exist within the system.
				err.SetError(txtUser, ex.Message);
				txtUser.Focus();
				ShowException(ex);
				ErrorBox.Show(this, ex);
                Cursor = Cursors.Default;
            }
			catch (InactiveOMSUserException  ex)
			{
				//Display a mnessage if the user specified is no longer active to use the system.
				err.SetError(txtUser, ex.Message);
				txtUser.Focus();
                ShowException(ex);
				ErrorBox.Show(this, ex);
                Cursor = Cursors.Default;
            }
			catch (Data.Exceptions.InvalidLoginException ex)
			{
				//Display a message if the SQL server login could not log into the server.
				if (db.LoginType == "SQL")
				{
					err.SetError(txtUser, ex.Message);
					txtUser.Focus();
					err.SetError(txtPassword, ex.Message);
					txtUser.Focus();
				}
				ShowException(ex);
				ErrorBox.Show(this, ex);
                Cursor = Cursors.Default;
            }
			catch (Data.Exceptions.DataException ex)
			{
				//Display a message for any other type of data connectivity error.
				err.SetError(cboMultiDB, ex.Message);
				cboMultiDB.Focus();
				ShowException(ex);
				ErrorBox.Show(this, ex);
                Cursor = Cursors.Default;
            }
			catch (Exception ex)
			{
				//Display a catch all exception.
                ShowException(ex);
				ErrorBox.Show(this, ex);
                Cursor = Cursors.Default;
            }

			finally
			{
                EnableControls();
			}

		}

        private void EnableControls()
        {
            this.gbLogin.Enabled = true;
            this.cboMultiDB.Enabled = true;
            this.cmdCancel.Enabled = true;
            this.cmdLogon.Enabled = true;
            this.cmdOptions.Enabled = true;
            this.chkClearCache.Enabled = true;
            Application.DoEvents();
        }

        private void DisableControls()
        {
            this.gbLogin.Enabled = false;
            this.cboMultiDB.Enabled = false;
            this.cmdCancel.Enabled = false;
            this.cmdLogon.Enabled = false;
            this.cmdOptions.Enabled = false;
            this.chkClearCache.Enabled = false;
            Application.DoEvents();
           
        }


        public void ShowException(Exception ex)
        {
            if (ex != null)
            {
                lblInfo.Text = ex.Message;
                lblInfo.Visible = true;
                pnlInfo.Visible = true;
                this.Height = _orgheight;
            }
            else
            {
                lblInfo.Text = "";
                pnlInfo.Visible = false;
            }
        }

		/// <summary>
		/// Called on the initial load of the login form.  This method loads the splash screen
		/// within its own thread and then sets all the copyright information and the default user
		/// within the relevant controls on the form.
		/// </summary>
		/// <param name="sender">Login form.</param>
		/// <param name="e">Empty event arguments.</param>
		private void Login_Load(object sender, System.EventArgs e)
		{
			//Set form display information.
			this.Text = FWBS.OMS.Global.ApplicationName;
			AssemblyCopyrightAttribute cpy = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute));

            try
            {
                //Load the branding logo
                Image loginlogo = Branding.GetLoginLogo();
                if (loginlogo != null)
                    picLogo2.Image = loginlogo;
            }
            catch { }
            Version version = Session.CurrentSession.EngineVersion;
            lblVersion.Text = string.Format("v{0} (Build {1})", version.ToString(3), version.Revision);

			lblCopyright.Text = cpy.Copyright;

			txtUser.Text = Environment.UserName;

            cmdLogon.Text = Session.CurrentSession.RegistryRes("Login", OMS.Global.GetResString("CmdLogon", false));
            cmdCancel.Text = Session.CurrentSession.RegistryRes("Cancel", OMS.Global.GetResString("CmdCancel", false));

            label1.Text = Session.CurrentSession.RegistryRes("Username", OMS.Global.GetResString("UserName", false));
            lblMultiDB.Text = Session.CurrentSession.RegistryRes("Database", OMS.Global.GetResString("Database", false));
            lblPassword.Text = Session.CurrentSession.RegistryRes("Password", OMS.Global.GetResString("Password", false));
            chkClearCache.Text = Session.CurrentSession.RegistryRes("ClearCache", OMS.Global.GetResString("ClearCache", false));
            cmdOptions.Text = Session.CurrentSession.RegistryRes("Options", OMS.Global.GetResString("Options", false));
            lblCopyright.Text = Session.CurrentSession.RegistryRes("Copyright", lblCopyright.Text);

            //Build the multi databse combo box and set its default.
            GetMultiDB(null);

            Application.DoEvents();

			try
			{
				//Check whether the user is part of the administrator group
				AppDomain.CurrentDomain.SetPrincipalPolicy(System.Security.Principal.PrincipalPolicy.WindowsPrincipal);
				System.Security.Principal.WindowsPrincipal principal = (System.Security.Principal.WindowsPrincipal)Thread.CurrentPrincipal;
				System.Security.Principal.WindowsIdentity identity = (System.Security.Principal.WindowsIdentity)principal.Identity;

				cmdOptions.Visible = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);
			}
			catch
			{
			}
		
		}


		/// <summary>
		/// Build the multi database combo box.
		/// </summary>
		private void GetMultiDB(DatabaseSettings settings)
		{
			try
			{
				//Clear existing items from the combo.
				cboMultiDB.Items.Clear();


				if (_connections.Count == 0)
				{
                    var messageBoxText = Session.CurrentSession.RegistryRes("DbConnectionNotConfigured", OMS.Global.GetResString("DbConnectionNotConfigured", false));
                    DialogResult res = MessageBox.Show(this, messageBoxText, "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

				retry:
					try
					{
						switch (res)
						{
							case DialogResult.Yes:
							{
								if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
									_connections.Open(openFileDialog1.FileName);
								else
								{
									this.Close();
									return;
								}
							}
								break;
							case DialogResult.No:
							{
								_connections.Open("");
								DatabaseSettings db = _connections.CreateDatabaseSettings();
								db.LoginType = "OMS";
                                using (frmConnections frm = new frmConnections(_connections, db))
                                {
                                    if (frm.ShowDialog(this) == DialogResult.Cancel)
                                        return;
                                    else
                                        settings = frm.SelectedSettings;
                                }
							}
								break;
							default:
								this.Close();
								return;
						}

					}
					catch (Security.SecurityException sex)
					{
						ErrorBox.Show(this, sex);
						goto retry;
					}
					catch (Exception)
					{
                        var text = Session.CurrentSession.RegistryRes("InvalidMultiDbConnConfigFile", OMS.Global.GetResString("InvalidMultiDbConnConfigFile", false));
                        MessageBox.Show(this, text, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
						goto retry;
					}
				}
				

				//Loop round each DatabaseSettings object in the mult databases collection.
				for (int ctr = 0; ctr < _connections.Count; ctr++)
				{
					cboMultiDB.Items.Add(_connections[ctr]);
				}



				//Set the combo to the current databse default.
                if (settings == null)
				    cboMultiDB.SelectedIndex = _connections.Default.Number;
		        else
                    cboMultiDB.SelectedIndex = settings.Number;

				//Make the combo and label visible depending on whether the multi database
				//option is set.
				
				if (cboMultiDB.Items.Count > 1)
				{
					cboMultiDB.Visible = true;			
					lblMultiDB.Visible = true;
				}
				else
				{
					cboMultiDB.Visible = false;			
					lblMultiDB.Visible = false;
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}



		/// <summary>
		/// Exit the application on cancel. 
		/// </summary>
		/// <param name="sender">Cancel button control.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		
		}


		/// <summary>
		/// Display the connection settings form when clicked.
		/// </summary>
		/// <param name="sender">Option button control.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdOptions_Click(object sender, System.EventArgs e)
		{
			if (cboMultiDB.SelectedItem != null)
			{
				DatabaseSettings db = (DatabaseSettings)cboMultiDB.SelectedItem;
                using (frmConnections frm = new frmConnections(_connections, db))
                {
                    DatabaseSettings settings = null;
                    if (frm.ShowDialog(this) == DialogResult.Cancel)
                    {
                        settings = _connections.Default;
                        return;
                    }
                    else
                        settings = frm.SelectedSettings;
                    
                    GetMultiDB(settings);
                }
			}
		
		}

	

		/// <summary>
		/// Global delegate used to select all in a text box when the focus is got.
		/// </summary>
		/// <param name="sender">One of many textboxes on the form.</param>
		/// <param name="e">Empty event arguments.</param>
		private void SelectAllOnFocus(object sender, System.EventArgs e)
		{
			if (sender is TextBox)
			{
				TextBox txt = (TextBox)sender;
				txt.SelectAll();
			}
		}

		/// <summary>
		/// Adjusts the look of the login form and the availablity of certain edit boxes
		/// depending on the type of login to the database server.  For instance, NT authentication
		/// does not allow the user to enter in a user name as this would be taken from the
		/// windows environment.
		/// </summary>
		/// <param name="sender">Multidb combo box control.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cboMultiDB_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			DatabaseSettings db = (DatabaseSettings)cboMultiDB.SelectedItem;

			//Force the loggedin name when connection string is set to NT authentication.
			if (db.LoginType == "NT" || db.LoginType == "AAD")
			{
				txtUser.Text = Environment.UserName;
				txtUser.Enabled = false;
				txtPassword.Enabled = false;
			}
			else
			{
				txtUser.Enabled = true;
				txtPassword.Enabled = true;
			}
		}

		private void txtUser_VisibleChanged(object sender, System.EventArgs e)
		{
			if (this.Visible)
			{
				txtPassword.Focus();
			}
		}

		private void gbLogin_Enter(object sender, System.EventArgs e)
		{
			txtPassword.Focus();
		}

		#endregion

        private void lblPassword_Click(object sender, EventArgs e)
        {

        }



	}


}
