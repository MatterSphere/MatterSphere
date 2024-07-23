using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace FWBS.Scanning
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class frmMain : FWBS.OMS.UI.Windows.BaseForm
    {
		private FWBS.Scanning.ucScanning ucScanning1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.FolderBrowserDialog fldFindMyScan;
		private FWBS.OMS.UI.Windows.ucFormStorage ucFormStorage1;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuConnect;
		private System.Windows.Forms.MenuItem mnuDisconnect;
		private System.Windows.Forms.MenuItem mnuSettings;
		private System.Windows.Forms.MenuItem mnuChange;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuAbout;
		private FWBS.OMS.UI.Windows.ucAlert ucAlert1;
		private System.Windows.Forms.MenuItem mnuOptions;
		private System.Windows.Forms.MenuItem mnuCommands;
		private System.Windows.Forms.MenuItem mnuMoveTo;
        private System.Windows.Forms.MenuItem mnuCopy;
        private System.Windows.Forms.MenuItem mnuCopyText;
        private System.Windows.Forms.MenuItem mnuCopyImage;
        private System.Windows.Forms.MenuItem mnuCopyFile;
        private IContainer components;
        private FWBS.OMS.UI.Windows.ResourceLookup resLookup;

		public frmMain()
		{
			//
			// Required for Windows Form Designer support
			//
			this.SetStyle(ControlStyles.DoubleBuffer | 
				ControlStyles.UserPaint | 
				ControlStyles.AllPaintingInWmPaint,
				true);
			this.UpdateStyles();

            InitializeComponent();
            this.Text = Application.SafeTopLevelCaptionFormat;
            AssignCustomMenuShortcut(mnuCopyImage, (Shortcut)(Keys.Control | Keys.Alt | Keys.C));
        }

        private static void AssignCustomMenuShortcut(MenuItem menuItem, Shortcut shortcut)
        {
            var dataField = typeof(MenuItem).GetField("data", BindingFlags.NonPublic | BindingFlags.Instance);
            var updateMenuItemMethod = typeof(MenuItem).GetMethod("UpdateMenuItem", BindingFlags.NonPublic | BindingFlags.Instance);
            var menuItemDataShortcutField = typeof(MenuItem).GetNestedType("MenuItemData", BindingFlags.NonPublic).GetField("shortcut", BindingFlags.NonPublic | BindingFlags.Instance);

            menuItemDataShortcutField.SetValue(dataField.GetValue(menuItem), shortcut);
            updateMenuItemMethod.Invoke(menuItem, new object[] { true });
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.resLookup = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.mnuFile = new System.Windows.Forms.MenuItem();
            this.mnuConnect = new System.Windows.Forms.MenuItem();
            this.mnuDisconnect = new System.Windows.Forms.MenuItem();
            this.mnuCommands = new System.Windows.Forms.MenuItem();
            this.mnuMoveTo = new System.Windows.Forms.MenuItem();
            this.mnuCopy = new System.Windows.Forms.MenuItem();
            this.mnuCopyText = new System.Windows.Forms.MenuItem();
            this.mnuCopyImage = new System.Windows.Forms.MenuItem();
            this.mnuCopyFile = new System.Windows.Forms.MenuItem();
            this.mnuSettings = new System.Windows.Forms.MenuItem();
            this.mnuChange = new System.Windows.Forms.MenuItem();
            this.mnuOptions = new System.Windows.Forms.MenuItem();
            this.mnuHelp = new System.Windows.Forms.MenuItem();
            this.mnuAbout = new System.Windows.Forms.MenuItem();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.fldFindMyScan = new System.Windows.Forms.FolderBrowserDialog();
            this.ucAlert1 = new FWBS.OMS.UI.Windows.ucAlert();
            this.ucFormStorage1 = new FWBS.OMS.UI.Windows.ucFormStorage(this.components);
            this.ucScanning1 = new FWBS.Scanning.ucScanning();
            this.SuspendLayout();
            // 
            // mnuFile
            // 
            this.mnuFile.Index = 0;
            this.resLookup.SetLookup(this.mnuFile, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuFile", "&File", ""));
            this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuConnect,
            this.mnuDisconnect});
            this.mnuFile.Text = "&File";
            // 
            // mnuConnect
            // 
            this.mnuConnect.Enabled = false;
            this.mnuConnect.Index = 0;
            this.resLookup.SetLookup(this.mnuConnect, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuConnect", "&Connect", ""));
            this.mnuConnect.Text = "&Connect";
            this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
            // 
            // mnuDisconnect
            // 
            this.mnuDisconnect.Index = 1;
            this.resLookup.SetLookup(this.mnuDisconnect, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuDisconnect", "Disconnect", ""));
            this.mnuDisconnect.Text = "Disconnect";
            this.mnuDisconnect.Click += new System.EventHandler(this.mnuDisconnect_Click);
            // 
            // mnuCommands
            // 
            this.mnuCommands.Index = 1;
            this.resLookup.SetLookup(this.mnuCommands, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCommands", "&Commands", ""));
            this.mnuCommands.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuMoveTo,
            this.mnuCopy});
            this.mnuCommands.Text = "&Commands";
            // 
            // mnuMoveTo
            // 
            this.mnuMoveTo.Index = 0;
            this.resLookup.SetLookup(this.mnuMoveTo, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuMoveTo", "&Move Image to... ", ""));
            this.mnuMoveTo.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
            this.mnuMoveTo.Text = "&Move Image to... ";
            this.mnuMoveTo.Click += new System.EventHandler(this.mnuMoveTo_Click);
            // 
            // mnuCopy
            // 
            this.mnuCopy.Index = 1;
            this.resLookup.SetLookup(this.mnuCopy, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuCopyClipbrd", "&Copy To Clipboard", ""));
            this.mnuCopy.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuCopyText,
            this.mnuCopyImage,
            this.mnuCopyFile});
            this.mnuCopy.Text = "&Copy To Clipboard";
            // 
            // mnuCopyText
            // 
            this.mnuCopyText.Index = 0;
            this.resLookup.SetLookup(this.mnuCopyText, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuText", "&Text", ""));
            this.mnuCopyText.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
            this.mnuCopyText.Text = "&Text";
            this.mnuCopyText.Click += new System.EventHandler(this.mnuCopyText_Click);
            // 
            // mnuCopyImage
            // 
            this.mnuCopyImage.Index = 1;
            this.resLookup.SetLookup(this.mnuCopyImage, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuImage", "&Image", ""));
            this.mnuCopyImage.Text = "&Image";
            this.mnuCopyImage.Click += new System.EventHandler(this.mnuCopyImage_Click);
            // 
            // mnuCopyFile
            // 
            this.mnuCopyFile.Index = 2;
            this.resLookup.SetLookup(this.mnuCopyFile, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuFile", "&File", ""));
            this.mnuCopyFile.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftC;
            this.mnuCopyFile.Text = "&File";
            this.mnuCopyFile.Click += new System.EventHandler(this.mnuCopyFile_Click);
            // 
            // mnuSettings
            // 
            this.mnuSettings.Index = 2;
            this.resLookup.SetLookup(this.mnuSettings, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuSettings", "&Settings", ""));
            this.mnuSettings.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuChange,
            this.mnuOptions});
            this.mnuSettings.Text = "&Settings";
            // 
            // mnuChange
            // 
            this.mnuChange.Index = 0;
            this.resLookup.SetLookup(this.mnuChange, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuChangeLocat", "Change My Scanning Location", ""));
            this.mnuChange.Text = "Change My Scanning Location";
            this.mnuChange.Click += new System.EventHandler(this.mnuChange_Click);
            // 
            // mnuOptions
            // 
            this.mnuOptions.Index = 1;
            this.resLookup.SetLookup(this.mnuOptions, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuOptions", "Options", ""));
            this.mnuOptions.Text = "Options";
            this.mnuOptions.Click += new System.EventHandler(this.mnuOptions_Click);
            // 
            // mnuHelp
            // 
            this.mnuHelp.Index = 3;
            this.resLookup.SetLookup(this.mnuHelp, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuHelp", "&Help", ""));
            this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuAbout});
            this.mnuHelp.Text = "&Help";
            // 
            // mnuAbout
            // 
            this.mnuAbout.Index = 0;
            this.resLookup.SetLookup(this.mnuAbout, new FWBS.OMS.UI.Windows.ResourceLookupItem("mnuAbout", "&About", ""));
            this.mnuAbout.Text = "&About";
            this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuFile,
            this.mnuCommands,
            this.mnuSettings,
            this.mnuHelp});
            // 
            // ucAlert1
            // 
            this.ucAlert1.BackColor = System.Drawing.Color.Transparent;
            this.ucAlert1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucAlert1.Location = new System.Drawing.Point(0, 0);
            this.ucAlert1.Name = "ucAlert1";
            this.ucAlert1.Size = new System.Drawing.Size(984, 44);
            this.ucAlert1.TabIndex = 13;
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.FormToStore = this;
            this.ucFormStorage1.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ucFormStorage1.UniqueID = "";
            this.ucFormStorage1.Version = ((long)(0));
            // 
            // ucScanning1
            // 
            this.ucScanning1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucScanning1.Location = new System.Drawing.Point(0, 0);
            this.ucScanning1.Name = "ucScanning1";
            this.ucScanning1.ScanLocation = null;
            this.ucScanning1.Size = new System.Drawing.Size(984, 639);
            this.ucScanning1.TabIndex = 4;
            this.ucScanning1.ToBeRefreshed = false;
            this.ucScanning1.Visible = false;
            this.ucScanning1.ChangeImaged += new System.EventHandler(this.ucScanning1_ChangeImaged);
            this.ucScanning1.Alert += new FWBS.OMS.AlertEventHandler(this.ucScanning1_Alert);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(984, 639);
            this.Controls.Add(this.ucScanning1);
            this.Controls.Add(this.ucAlert1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Menu = this.mainMenu1;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

		}
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
            Application.EnableVisualStyles();
            if (FWBS.OMS.UI.Windows.Services.CheckLogin())
            {
                Application.SafeTopLevelCaptionFormat = FWBS.OMS.Branding.APPLICATION_NAME + " " + OMS.Session.CurrentSession.Resources.GetResource("POSTROOMAPP", "Post Room Application", "").Text;
                Application.Run(new frmMain());
            }
		}

		private void mnuConnect_Click(object sender, System.EventArgs e)
		{
            if (FWBS.OMS.UI.Windows.Services.CheckLogin())
            {
                mnuConnect.Enabled = false;
                mnuDisconnect.Enabled = true;
                frmMain_Load(sender, e);
            }
		}

		private void mnuDisconnect_Click(object sender, System.EventArgs e)
		{
			FWBS.OMS.Session.CurrentSession.Disconnect();
			ucScanning1.Visible = false;
            mnuCommands.Visible = false;
            mnuSettings.Visible = false;
            mnuHelp.Visible = false;
            mnuDisconnect.Enabled = false;
            mnuConnect.Enabled = true;
            this.Text = Application.SafeTopLevelCaptionFormat;
        }

		private void mnuChange_Click(object sender, System.EventArgs e)
		{
			ucScanning1.ChangeFolder();
            this.Text = Application.SafeTopLevelCaptionFormat + " - [" + ucScanning1.CurrentUserFolderName + "] - Database [" + FWBS.OMS.Session.CurrentSession.CurrentDatabase.DatabaseName + "]";
        }

		private void frmMain_Load(object sender, System.EventArgs e)
		{
            try
            {
                ucScanning1.Connect(FWBS.OMS.Session.CurrentSession.CurrentUser);
                ucScanning1.RefreshItem();
                ucScanning1.ConvertSupportedFilesToTif();
                ucScanning1.RefreshScanImages();
                mnuCommands.Visible = true;
                mnuSettings.Visible = true;
                mnuHelp.Visible = true;
                this.Text = Application.SafeTopLevelCaptionFormat + " - [" + ucScanning1.CurrentUserFolderName + "] - Database [" + FWBS.OMS.Session.CurrentSession.CurrentDatabase.DatabaseName + "]";
                
                ucScanning1.Visible = true;
                ucScanning1.LoadImage(0);
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
                mnuDisconnect_Click(sender, e);
                if (sender == this)
                    Close();
            }
		}

        private void ucScanning1_Alert(object sender, FWBS.OMS.AlertEventArgs e)
		{
			ucAlert1.SetAlerts(e.Alerts);
		}

		private void mnuOptions_Click(object sender, System.EventArgs e)
		{
            frmSaveSettings settings = new frmSaveSettings(ucScanning1, ucScanning1.MoveLocation.FullName);
			if (settings.ShowDialog() == DialogResult.OK)
			{
				FWBS.Common.RegistryAccess.SetSetting("",Microsoft.Win32.RegistryHive.CurrentUser,@"\Software\FWBS\OMS\2.0\OMSDocumentImporter","AfterSave",Convert.ToInt32(settings.Result));
				FWBS.Common.RegistryAccess.SetSetting("",Microsoft.Win32.RegistryHive.CurrentUser,@"\Software\FWBS\OMS\2.0\OMSDocumentImporter","MovePath",settings.MovePath);
				ucScanning1.Connect(FWBS.OMS.Session.CurrentSession.CurrentUser);
			}
            settings.Dispose();
		}

		private void mnuMoveTo_Click(object sender, System.EventArgs e)
		{
			ucScanning1.MoveImageTo();
		}

		private void ucScanning1_ChangeImaged(object sender, System.EventArgs e)
		{
			bool enabled = ucScanning1.ImageCount > 0;
			mnuMoveTo.Enabled = enabled;
			mnuCopy.Enabled = enabled;
		}

		private void mnuCopyText_Click(object sender, System.EventArgs e)
		{
			ucScanning1.CopyTextToClipboard();
		}

		private void mnuCopyImage_Click(object sender, System.EventArgs e)
		{
			ucScanning1.CopyImageToClipboard();
		}

		private void mnuCopyFile_Click(object sender, System.EventArgs e)
		{
			ucScanning1.CopyFileToClipboard();
		}

		private void mnuAbout_Click(object sender, System.EventArgs e)
		{
			FWBS.OMS.UI.Windows.Services.ShowAbout();
		}

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Form Closed. Close Application", Application.SafeTopLevelCaptionFormat);
            ucScanning1.CleanUpRoutineOnExitOfScanningApplication();
        }
        
	}
}
