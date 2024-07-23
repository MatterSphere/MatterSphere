using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A simple form that is used to hold a select client / file user control.  
    /// This form holds extra buttons so that a client / file can be found or added.
    /// </summary>
    internal class frmSelectClientFile : frmNewBrandIdent, FWBS.OMS.UI.Windows.Interfaces.ISelectClientFileDialog, ISupportRightToLeft
	{
		#region Fields

        protected ucAlert pnlAlert;
        protected System.Windows.Forms.Button cmdViewFile;
        protected System.Windows.Forms.Button cmdFind;
        protected System.Windows.Forms.Button cmdCancel;
        protected System.Windows.Forms.Button cmdViewClient;
        protected System.Windows.Forms.Button cmdOK;
        protected System.Windows.Forms.Panel pnlMain;
        protected FWBS.OMS.UI.Windows.ucSelectClientFile ucSelectClientFileSelector;
        protected FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
        private FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private FWBS.OMS.UI.Windows.Accelerators accelerators1;
        protected System.Windows.Forms.Button btnPrivateEmail;
        private Panel panel4;
        protected Button btnCreateClient;
        protected Panel panel6;
        private Panel pnlMainContainer;


        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Default constructor of the form.
        /// </summary>
        public frmSelectClientFile() : base()
        {
            InitializeComponent();
            SetIcon(Images.DialogIcons.File);
        }


        /// <summary>
        /// Constructor to specifying the file to find immediately.  If null then no file
        /// is taken.
        /// </summary>
        /// <param name="file">File to show in the select form.</param>
        public frmSelectClientFile(OMSFile file)
            : this()
        {
            if (Session.CurrentSession.IsLoggedIn)
                ucSelectClientFileSelector.GetFile(file);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

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
            this.pnlAlert = new FWBS.OMS.UI.Windows.ucAlert();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.btnCreateClient = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnPrivateEmail = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmdViewClient = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.cmdFind = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmdViewFile = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.ucSelectClientFileSelector = new FWBS.OMS.UI.Windows.ucSelectClientFile();
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.accelerators1 = new FWBS.OMS.UI.Windows.Accelerators(this.components);
            this.pnlMainContainer = new System.Windows.Forms.Panel();
            this.pnlMain.SuspendLayout();
            this.pnlMainContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlAlert
            // 
            this.pnlAlert.BackColor = System.Drawing.Color.Transparent;
            this.pnlAlert.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAlert.Location = new System.Drawing.Point(5, 5);
            this.pnlAlert.Name = "pnlAlert";
            this.pnlAlert.Size = new System.Drawing.Size(712, 56);
            this.pnlAlert.TabIndex = 4;
            this.pnlAlert.Visible = false;
            // 
            // pnlMain
            // 
            this.pnlMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMain.Controls.Add(this.btnCreateClient);
            this.pnlMain.Controls.Add(this.panel6);
            this.pnlMain.Controls.Add(this.btnPrivateEmail);
            this.pnlMain.Controls.Add(this.panel4);
            this.pnlMain.Controls.Add(this.cmdCancel);
            this.pnlMain.Controls.Add(this.panel1);
            this.pnlMain.Controls.Add(this.cmdViewClient);
            this.pnlMain.Controls.Add(this.panel3);
            this.pnlMain.Controls.Add(this.cmdFind);
            this.pnlMain.Controls.Add(this.panel2);
            this.pnlMain.Controls.Add(this.cmdViewFile);
            this.pnlMain.Controls.Add(this.cmdOK);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlMain.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMain.Location = new System.Drawing.Point(614, 5);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(6, 5, 5, 5);
            this.pnlMain.Size = new System.Drawing.Size(103, 539);
            this.pnlMain.TabIndex = 5;
            // 
            // btnCreateClient
            // 
            this.btnCreateClient.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnCreateClient.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCreateClient.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCreateClient.Location = new System.Drawing.Point(6, 453);
            this.resourceLookup1.SetLookup(this.btnCreateClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCreateClient", "Create %CLIENT%", ""));
            this.btnCreateClient.Name = "btnCreateClient";
            this.btnCreateClient.Size = new System.Drawing.Size(90, 23);
            this.btnCreateClient.TabIndex = 42;
            this.btnCreateClient.Text = "Create %CLIENT%";
            this.btnCreateClient.Click += new System.EventHandler(this.btnCreateClient_Click);
            // 
            // panel6
            // 
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(6, 476);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(90, 5);
            this.panel6.TabIndex = 43;
            // 
            // btnPrivateEmail
            // 
            this.btnPrivateEmail.DialogResult = System.Windows.Forms.DialogResult.Ignore;
            this.btnPrivateEmail.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnPrivateEmail.Enabled = false;
            this.btnPrivateEmail.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrivateEmail.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrivateEmail.Location = new System.Drawing.Point(6, 89);
            this.resourceLookup1.SetLookup(this.btnPrivateEmail, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnPrivateEmail", "Private &Email", ""));
            this.btnPrivateEmail.Name = "btnPrivateEmail";
            this.btnPrivateEmail.Size = new System.Drawing.Size(90, 23);
            this.btnPrivateEmail.TabIndex = 39;
            this.btnPrivateEmail.Text = "Private &Email";
            this.btnPrivateEmail.Visible = false;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(6, 84);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(90, 5);
            this.panel4.TabIndex = 40;
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdCancel.Location = new System.Drawing.Point(6, 61);
            this.resourceLookup1.SetLookup(this.cmdCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdCancel", "Cancel", ""));
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(90, 23);
            this.cmdCancel.TabIndex = 31;
            this.cmdCancel.Text = "Cancel";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(6, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(90, 5);
            this.panel1.TabIndex = 36;
            // 
            // cmdViewClient
            // 
            this.cmdViewClient.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmdViewClient.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdViewClient.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdViewClient.Location = new System.Drawing.Point(6, 481);
            this.resourceLookup1.SetLookup(this.cmdViewClient, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdViewClient", "View %CLIENT%", ""));
            this.cmdViewClient.Name = "cmdViewClient";
            this.cmdViewClient.Size = new System.Drawing.Size(90, 23);
            this.cmdViewClient.TabIndex = 34;
            this.cmdViewClient.Text = "View %CLIENT%";
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(6, 504);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(90, 5);
            this.panel3.TabIndex = 38;
            // 
            // cmdFind
            // 
            this.cmdFind.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmdFind.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdFind.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdFind.Location = new System.Drawing.Point(6, 33);
            this.resourceLookup1.SetLookup(this.cmdFind, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdFind", "Find...", ""));
            this.cmdFind.Name = "cmdFind";
            this.cmdFind.Size = new System.Drawing.Size(90, 23);
            this.cmdFind.TabIndex = 30;
            this.cmdFind.Text = "Find...";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(6, 28);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(90, 5);
            this.panel2.TabIndex = 37;
            // 
            // cmdViewFile
            // 
            this.cmdViewFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cmdViewFile.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdViewFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdViewFile.Location = new System.Drawing.Point(6, 509);
            this.resourceLookup1.SetLookup(this.cmdViewFile, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdViewFile", "View %FILE%", ""));
            this.cmdViewFile.Name = "cmdViewFile";
            this.cmdViewFile.Size = new System.Drawing.Size(90, 23);
            this.cmdViewFile.TabIndex = 33;
            this.cmdViewFile.Text = "View %FILE%";
            // 
            // cmdOK
            // 
            this.cmdOK.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdOK.Location = new System.Drawing.Point(6, 5);
            this.resourceLookup1.SetLookup(this.cmdOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdOK", "Proceed", ""));
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(90, 23);
            this.cmdOK.TabIndex = 33;
            this.cmdOK.Text = "Proceed";
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // ucSelectClientFileSelector
            // 
            this.ucSelectClientFileSelector.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ucSelectClientFileSelector.CancelButton = this.cmdCancel;
            this.ucSelectClientFileSelector.CheckAllFiles = false;
            this.ucSelectClientFileSelector.ClientFileState = FWBS.OMS.UI.Windows.ClientFileState.None;
            this.ucSelectClientFileSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSelectClientFileSelector.FavouriteHeight = 38;
            this.ucSelectClientFileSelector.FindButton = this.cmdFind;
            this.ucSelectClientFileSelector.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.ucSelectClientFileSelector.Location = new System.Drawing.Point(5, 5);
            this.ucSelectClientFileSelector.Name = "ucSelectClientFileSelector";
            this.ucSelectClientFileSelector.OKButton = this.cmdOK;
            this.ucSelectClientFileSelector.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.ucSelectClientFileSelector.SelectClientFileSearchType = FWBS.OMS.UI.Windows.SelectClientFileSearchType.File;
            this.ucSelectClientFileSelector.SelectFileVisible = true;
            this.ucSelectClientFileSelector.Size = new System.Drawing.Size(609, 539);
            this.ucSelectClientFileSelector.TabIndex = 0;
            this.ucSelectClientFileSelector.ViewClientButton = this.cmdViewClient;
            this.ucSelectClientFileSelector.ViewFileButton = this.cmdViewFile;
            this.ucSelectClientFileSelector.Alert += new FWBS.OMS.AlertEventHandler(this.ucClientSelector_Alert);
            this.ucSelectClientFileSelector.StateChanged += new FWBS.OMS.UI.Windows.ClientFileStateChangedEventHandler(this.ucSelectClientFileSelector_StateChanged);
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.Position = false;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ucFormStorage1.State = false;
            this.ucFormStorage1.UniqueID = "Forms\\SelectClientFile";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // accelerators1
            // 
            this.accelerators1.Form = this;
            // 
            // pnlMainContainer
            // 
            this.pnlMainContainer.Controls.Add(this.ucSelectClientFileSelector);
            this.pnlMainContainer.Controls.Add(this.pnlMain);
            this.pnlMainContainer.Controls.Add(this.pnlAlert);
            this.pnlMainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlMainContainer.Name = "pnlMainContainer";
            this.pnlMainContainer.Padding = new System.Windows.Forms.Padding(5);
            this.pnlMainContainer.Size = new System.Drawing.Size(722, 549);
            this.pnlMainContainer.TabIndex = 9;
            // 
            // frmSelectClientFile
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(722, 549);
            this.Controls.Add(this.pnlMainContainer);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelectClientFile";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Loading ...";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmSelectClientFile_Closing);
            this.Load += new System.EventHandler(this.frmSelectClientFile_Load);
            this.Shown += new System.EventHandler(this.frmSelectClientFile_Shown);
            this.pnlMain.ResumeLayout(false);
            this.pnlMainContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        #endregion

        #region Private Methods

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            SetIcon(Images.DialogIcons.File);
        }

        /// <summary>
        /// If a client has been picked then close the form with a success dialog code.
        /// </summary>
        /// <param name="sender">OK button control.</param>
        /// <param name="e">Empty event arguments.</param>
        protected virtual void cmdOK_Click(object sender, System.EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                 if (ucSelectClientFileSelector.SelectedFile == null && ucSelectClientFileSelector.SelectFileVisible)
                {
                    ucSelectClientFileSelector.ClientFileState = ClientFileState.Confirm;
                }
                else if (ucSelectClientFileSelector.ClientFileState == ClientFileState.None)
                {
                    ucSelectClientFileSelector.ClientFileState = ClientFileState.Proceed;
                }
                else if (ucSelectClientFileSelector.ClientFileState == ClientFileState.Confirm)
                {
                    ucSelectClientFileSelector.ClientFileState = ClientFileState.Proceed;
                }
                else if (ucSelectClientFileSelector.ClientFileState == ClientFileState.Proceed)
                {
                    this.DialogResult = DialogResult.OK;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Capture the alert event of the select client/file user control and display it in a 
        /// big red label at the top of the form.
        /// </summary>
        /// <param name="sender">Client / file selector control.</param>
        /// <param name="e">Empty event arguments.</param>
        private void ucClientSelector_Alert(object sender, AlertEventArgs e)
        {
            pnlAlert.SetAlerts(e.Alerts);

        }

        /// <summary>
        /// Captures the state changed event of the client / file control
        /// Use the passed event argument to determine how the proceed / OK
        /// button executes.
        /// </summary>
        /// <param name="sender">Client / file user control object.</param>
        /// <param name="e">State changed event arguments.</param>
        private void ucSelectClientFileSelector_StateChanged(object sender, FWBS.OMS.UI.Windows.ClientFileStateEventArgs e)
        {
            if (e.State == ClientFileState.None)
            {
                this.resourceLookup1.SetLookup(this.cmdOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("Confirm", "Confirm", ""));
            }
            if (e.State == ClientFileState.Confirm)
            {
                this.resourceLookup1.SetLookup(this.cmdOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("Proceed", "Proceed", ""));
            }
            if (e.State == ClientFileState.Proceed)
            {
                this.resourceLookup1.SetLookup(this.cmdOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("Proceed", "Proceed", ""));
            }
        }

        /// <summary>
        /// The load event accesses the resource strings of the buttons and form caption.
        /// </summary>
        /// <param name="sender">This select client / file form.</param>
        /// <param name="e">Empty event arguments.</param>
        private void frmSelectClientFile_Load(object sender, System.EventArgs e)
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                pnlMain.Visible = false;
                ucSelectClientFileSelector.Visible = false;
                if (!DesignMode)
                {
                    Global.ControlParser(this);
                }
                pnlMain.Visible = true;
                ucSelectClientFileSelector.Visible = true;
                UpdateCaption();
            }

            Common.ApplicationSetting ccl = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\Tweaks", "CreateClientButton", "True");
            btnCreateClient.Visible = ccl.ToBoolean();
            Common.ApplicationSetting vwcl = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, @"UI\Tweaks", "ShowViewClientButton", "True");
            Common.ApplicationSetting vwf = new Common.ApplicationSetting(vwcl, "ShowViewFileButton", "True");
            cmdViewClient.Visible = vwcl.ToBoolean();
            cmdViewFile.Visible = vwf.ToBoolean();

            ucSelectClientFileSelector.Focus();

            //This line of code is added because when the RightToLeft property is changed to 
            //RightToLeft.Yes in Global.RightToLeftFormConverter the DialogResult property 
            //gets set to DialogResult.Cancel for some strange reason and only on this form.
            DialogResult = DialogResult.None;
        }

        protected virtual void UpdateCaption()
        {
            this.Text = Session.CurrentSession.Resources.GetResource("SELECTCLIENTFIL", "Select a %CLIENT% and %FILE%", "").Text;
        }

		private void frmSelectClientFile_Closing(object sender, CancelEventArgs e)
		{
			try
			{
                if (Session.CurrentSession.IsLoggedIn)
                {
                    if (this.DialogResult == DialogResult.OK)
                    {
                        ucSelectClientFileSelector.SetFilePhase();
                        ucSelectClientFileSelector.AddToTop10();
                    }
                }
			}
			catch{}
		}

        #endregion

        #region Properties

        /// <summary>
        /// Gets the client that has been selected from the client / file selection control.
        /// </summary>
        [DefaultValue(null)]
        public Client SelectedClient
        {
            get
            {
                if (ucSelectClientFileSelector != null)
                    return ucSelectClientFileSelector.SelectedClient;
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets the file id that has been selected from the client / file selection control.
        /// </summary>
        [DefaultValue(null)]
        public OMSFile SelectedFile
        {
            get
            {
                if (ucSelectClientFileSelector.SelectedFile != null)
                    return ucSelectClientFileSelector.SelectedFile;
                else
                    return null;
            }
        }

        /// <summary>
        /// Gets or Sets whether the file phases combox shopuld be displayed (if any).
        /// </summary>
        [DefaultValue(true)]
        public bool ShowPhases
        {
            get
            {
                return ucSelectClientFileSelector.ShowPhases;
            }
            set
            {
                ucSelectClientFileSelector.ShowPhases = value;
            }
        }

        public bool AllowPrivateAssociate { get; set; }
        
        #endregion

        private void btnCreateClient_Click(object sender, EventArgs e)
        {
            try
            {
                Client n = FWBS.OMS.UI.Windows.Services.Wizards.CreateClient(true);
                if (n != null)
                {
                    ucSelectClientFileSelector.GetClient(n);
                    ucSelectClientFileSelector.ClientFileState = ClientFileState.Proceed;
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void frmSelectClientFile_Shown(object sender, EventArgs e)
        {
            //Prevents problem with form closing before fully loaded.
            btnPrivateEmail.Visible = AllowPrivateAssociate;
            btnPrivateEmail.Enabled = AllowPrivateAssociate;
        }



        public void SetRTL(Form parentform)
        {
            ucSelectClientFileSelector.Padding = new Padding(5, 0, 0, 0);
            foreach (Control ctrl in ucSelectClientFileSelector.Controls)
            {
                Global.RightToLeftControlConverter(ctrl, parentform);
            }
        }
    }
}
