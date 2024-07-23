using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// The About Box.
    /// </summary>
    internal class frmAbout : frmNewBrandIdent
    {
        #region Controls

        private FWBS.OMS.UI.TabControl tabControl;
		private System.Windows.Forms.TabPage tpDiag;
        private FWBS.OMS.UI.TabControl tcDiags;
		private System.Windows.Forms.PictureBox picLogo2;
		private System.Windows.Forms.Panel pnlStatus;
		private System.Windows.Forms.Label labUserCap;
		private System.Windows.Forms.Label LabProvider;
		private System.Windows.Forms.Label labUser;
		private System.Windows.Forms.Label lblCopyright;
		private System.Windows.Forms.TabPage tpGeneral;
		private System.Windows.Forms.Label labServer;
		private System.Windows.Forms.Label labDatabase;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label labcult;
        private System.Windows.Forms.TabPage tpAssemblies;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button btnSysInfo;
		private System.Windows.Forms.TabPage tpPartner;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label lblPartnerInfo;
		private System.Windows.Forms.Label lblSuppInfo;
		private System.Windows.Forms.Label lblPAddress;
		private System.Windows.Forms.Label lblName;
		private System.Windows.Forms.Label lblPartnerAddress;
		private System.Windows.Forms.Label lblPartnerName;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label lblPartnerGeneralTel;
		private System.Windows.Forms.Label lblPartnerSuppTel;
        private System.Windows.Forms.LinkLabel lblPartnerWeb;
		private System.Windows.Forms.ColumnHeader colLicType;
		private System.Windows.Forms.ColumnHeader colLicIs;
		private System.Windows.Forms.ImageList imageList1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label lblDemo;
		private System.Windows.Forms.Label lblInactive;
		private System.Windows.Forms.Label lblUnlimited;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.ColumnHeader colLicTotal;
		private System.Windows.Forms.ColumnHeader colLicAllocated;
		private System.Windows.Forms.TabPage tpPackages;
		private FWBS.OMS.UI.Windows.DataGridEx dgPackages;
		private System.Windows.Forms.DataGridTableStyle dgsPackages;
		private FWBS.OMS.UI.Windows.DataGridImageColumn dcpCode;
		private FWBS.Common.UI.Windows.DataGridLabelColumn dcpVersion;
		private FWBS.Common.UI.Windows.DataGridLabelColumn dcpInstalled;
		private System.Windows.Forms.Label lblDBVersion;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label lblEngineVersion;
        private System.Windows.Forms.Label label11;
        private Panel pnlButtons;
        private System.Windows.Forms.TabPage tpAddins;
        private TabPage tpServices;
        private FWBS.OMS.UI.ListView lvwServices;
        private ColumnHeader colServicesName;
        private FWBS.OMS.UI.ListView lvwAddins;
        private ColumnHeader columnHeader1;
        private TextBox txtAddinDescription;
        private TextBox txtServicesMessage;
        private Panel panel1;
        private Panel panel2;
        private TabPage tpConnectedClients;
        private FWBS.OMS.UI.ListView lvwConnectedClients;
        private ColumnHeader colCCName;
        private ColumnHeader colCCProcId;
        private ColumnHeader colCCPath;

        private Label lblVolume;
        private ListViewEx lstAssemblies;
        private ColumnHeader colAssemblyName;
        private ColumnHeader colAssemblyVersion;
        private ColumnHeader colAssemblyCompany;
        private ColumnHeader colAssemblyPKT;
        private ColumnHeader colAssemblyFileVersion;
        private ResourceLookup resourceLookup1;
        private Panel pnlMain;
        private LinkLabel lblPrivacyPolicy;

        #endregion

        #region Constructors

        public frmAbout() : base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            tabControl.TabPages.Remove(tpPartner);

            lstAssemblies.RightToLeft = this.RightToLeft;
            lstAssemblies.RightToLeftLayout = (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes);
            lvwServices.RightToLeft = this.RightToLeft;
            lvwServices.RightToLeftLayout = (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes);
            lvwConnectedClients.RightToLeft = this.RightToLeft;
            lvwConnectedClients.RightToLeftLayout = (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes);
            lvwAddins.RightToLeft = this.RightToLeft;
            lvwAddins.RightToLeftLayout = (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes);
            txtAddinDescription.BackColor = FWBS.Common.UI.Windows.ExtColor.ConvertToRGB(lvwAddins.BackColor);
            txtServicesMessage.BackColor = FWBS.Common.UI.Windows.ExtColor.ConvertToRGB(lvwAddins.BackColor);

            ApplyImages();

            dgPackages.CaptionText = Session.CurrentSession.Resources.GetResource("dgPackages", "Installed Packages", "").Text;
            SetIcon(Images.DialogIcons.EliteApp);
        }


        /// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            try
            {
                if (disposing)
                {
                    try
                    {
                        Connectivity.ConnectivityManager.CurrentManager.Connected -= new EventHandler(CurrentManager_Connected);
                    }
                    catch { }

                    try
                    {
                        Connectivity.ConnectivityManager.CurrentManager.Disconnected -= new MessageEventHandler(CurrentManager_Disconnected);
                    }
                    catch { }

                }
            }
            finally
            {
                base.Dispose(disposing);
            }
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Framework", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("System", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("3rd Party", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup4 = new System.Windows.Forms.ListViewGroup("Scripts", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup5 = new System.Windows.Forms.ListViewGroup("Distributed", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup6 = new System.Windows.Forms.ListViewGroup("Dynamic", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
            System.Windows.Forms.ListViewGroup listViewGroup7 = new System.Windows.Forms.ListViewGroup("Loaded", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup8 = new System.Windows.Forms.ListViewGroup("Unloaded", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup9 = new System.Windows.Forms.ListViewGroup("Errors", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup10 = new System.Windows.Forms.ListViewGroup("Disabled", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup11 = new System.Windows.Forms.ListViewGroup("Connected", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup12 = new System.Windows.Forms.ListViewGroup("Disconnected", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup13 = new System.Windows.Forms.ListViewGroup("Connected", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup14 = new System.Windows.Forms.ListViewGroup("Disconnected", System.Windows.Forms.HorizontalAlignment.Left);
            this.tabControl = new FWBS.OMS.UI.TabControl();
            this.tpGeneral = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblPrivacyPolicy = new System.Windows.Forms.LinkLabel();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.lblEngineVersion = new System.Windows.Forms.Label();
            this.labUser = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.LabProvider = new System.Windows.Forms.Label();
            this.lblDBVersion = new System.Windows.Forms.Label();
            this.labServer = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.labDatabase = new System.Windows.Forms.Label();
            this.labcult = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tpAssemblies = new System.Windows.Forms.TabPage();
            this.lstAssemblies = new System.Windows.Forms.ListViewEx();
            this.colAssemblyName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssemblyVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssemblyFileVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssemblyCompany = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colAssemblyPKT = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.tpDiag = new System.Windows.Forms.TabPage();
            this.tcDiags = new FWBS.OMS.UI.TabControl();
            this.colLicType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLicAllocated = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLicTotal = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLicIs = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblVolume = new System.Windows.Forms.Label();
            this.lblUnlimited = new System.Windows.Forms.Label();
            this.lblInactive = new System.Windows.Forms.Label();
            this.lblDemo = new System.Windows.Forms.Label();
            this.tpPackages = new System.Windows.Forms.TabPage();
            this.dgPackages = new FWBS.OMS.UI.Windows.DataGridEx();
            this.dgsPackages = new System.Windows.Forms.DataGridTableStyle();
            this.dcpCode = new FWBS.OMS.UI.Windows.DataGridImageColumn();
            this.dcpVersion = new FWBS.Common.UI.Windows.DataGridLabelColumn();
            this.dcpInstalled = new FWBS.Common.UI.Windows.DataGridLabelColumn();
            this.tpAddins = new System.Windows.Forms.TabPage();
            this.lvwAddins = new FWBS.OMS.UI.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtAddinDescription = new System.Windows.Forms.TextBox();
            this.tpPartner = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblPartnerInfo = new System.Windows.Forms.Label();
            this.lblPartnerWeb = new System.Windows.Forms.LinkLabel();
            this.lblPartnerName = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblPartnerAddress = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblPartnerSuppTel = new System.Windows.Forms.Label();
            this.lblSuppInfo = new System.Windows.Forms.Label();
            this.lblPartnerGeneralTel = new System.Windows.Forms.Label();
            this.lblPAddress = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.tpServices = new System.Windows.Forms.TabPage();
            this.lvwServices = new FWBS.OMS.UI.ListView();
            this.colServicesName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtServicesMessage = new System.Windows.Forms.TextBox();
            this.tpConnectedClients = new System.Windows.Forms.TabPage();
            this.lvwConnectedClients = new FWBS.OMS.UI.ListView();
            this.colCCName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCCProcId = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colCCPath = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.picLogo2 = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.labUserCap = new System.Windows.Forms.Label();
            this.btnSysInfo = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlMain = new System.Windows.Forms.Panel();
            this.tabControl.SuspendLayout();
            this.tpGeneral.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tpAssemblies.SuspendLayout();
            this.tpDiag.SuspendLayout();
            this.tpPackages.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgPackages)).BeginInit();
            this.tpAddins.SuspendLayout();
            this.tpPartner.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tpServices.SuspendLayout();
            this.tpConnectedClients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo2)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tpGeneral);
            this.tabControl.Controls.Add(this.tpAssemblies);
            this.tabControl.Controls.Add(this.tpDiag);
            this.tabControl.Controls.Add(this.tpPackages);
            this.tabControl.Controls.Add(this.tpAddins);
            this.tabControl.Controls.Add(this.tpPartner);
            this.tabControl.Controls.Add(this.tpServices);
            this.tabControl.Controls.Add(this.tpConnectedClients);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(564, 362);
            this.tabControl.TabIndex = 20;
            this.tabControl.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl_DragDrop);
            // 
            // tpGeneral
            // 
            this.tpGeneral.AllowDrop = true;
            this.tpGeneral.Controls.Add(this.panel1);
            this.tpGeneral.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpGeneral, new FWBS.OMS.UI.Windows.ResourceLookupItem("tpGeneral", "General", ""));
            this.tpGeneral.Name = "tpGeneral";
            this.tpGeneral.Size = new System.Drawing.Size(556, 334);
            this.tpGeneral.TabIndex = 0;
            this.tpGeneral.Text = "General";
            this.tpGeneral.UseVisualStyleBackColor = true;
            this.tpGeneral.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl_DragDrop);
            this.tpGeneral.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControl_DragDrop);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblPrivacyPolicy);
            this.panel1.Controls.Add(this.lblCopyright);
            this.panel1.Controls.Add(this.lblEngineVersion);
            this.panel1.Controls.Add(this.labUser);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.LabProvider);
            this.panel1.Controls.Add(this.lblDBVersion);
            this.panel1.Controls.Add(this.labServer);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.labDatabase);
            this.panel1.Controls.Add(this.labcult);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(556, 334);
            this.panel1.TabIndex = 69;
            // 
            // lblPrivacyPolicy
            // 
            this.lblPrivacyPolicy.AutoSize = true;
            this.lblPrivacyPolicy.Location = new System.Drawing.Point(7, 268);
            this.lblPrivacyPolicy.Name = "lblPrivacyPolicy";
            this.lblPrivacyPolicy.Size = new System.Drawing.Size(80, 15);
            this.lblPrivacyPolicy.TabIndex = 69;
            this.lblPrivacyPolicy.TabStop = true;
            this.lblPrivacyPolicy.Text = "Elite Privacy Statement";
            this.lblPrivacyPolicy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblPrivacyPolicy_LinkClicked);
            // 
            // lblCopyright
            // 
            this.lblCopyright.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCopyright.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.lblCopyright.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblCopyright.Location = new System.Drawing.Point(7, 11);
            this.lblCopyright.Name = "lblCopyright";
            this.lblCopyright.Size = new System.Drawing.Size(542, 81);
            this.lblCopyright.TabIndex = 45;
            this.lblCopyright.Text = "Copyright";
            this.lblCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblEngineVersion
            // 
            this.lblEngineVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblEngineVersion.Location = new System.Drawing.Point(115, 234);
            this.lblEngineVersion.Name = "lblEngineVersion";
            this.lblEngineVersion.Size = new System.Drawing.Size(259, 16);
            this.lblEngineVersion.TabIndex = 68;
            this.lblEngineVersion.Text = "Engine Version";
            this.lblEngineVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labUser
            // 
            this.labUser.Location = new System.Drawing.Point(115, 102);
            this.labUser.Name = "labUser";
            this.labUser.Size = new System.Drawing.Size(259, 16);
            this.labUser.TabIndex = 54;
            this.labUser.Text = "User";
            // 
            // label11
            // 
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(7, 234);
            this.resourceLookup1.SetLookup(this.label11, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblengver", "Engine Version", ""));
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 16);
            this.label11.TabIndex = 67;
            this.label11.Text = "Engine Version";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // LabProvider
            // 
            this.LabProvider.ForeColor = System.Drawing.SystemColors.ControlText;
            this.LabProvider.Location = new System.Drawing.Point(115, 124);
            this.LabProvider.Name = "LabProvider";
            this.LabProvider.Size = new System.Drawing.Size(259, 16);
            this.LabProvider.TabIndex = 55;
            this.LabProvider.Text = "Provider";
            this.LabProvider.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDBVersion
            // 
            this.lblDBVersion.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblDBVersion.Location = new System.Drawing.Point(115, 212);
            this.lblDBVersion.Name = "lblDBVersion";
            this.lblDBVersion.Size = new System.Drawing.Size(259, 16);
            this.lblDBVersion.TabIndex = 66;
            this.lblDBVersion.Text = "Database Version";
            this.lblDBVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labServer
            // 
            this.labServer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labServer.Location = new System.Drawing.Point(115, 146);
            this.labServer.Name = "labServer";
            this.labServer.Size = new System.Drawing.Size(259, 16);
            this.labServer.TabIndex = 56;
            this.labServer.Text = "Server ";
            this.labServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label9.Location = new System.Drawing.Point(7, 212);
            this.resourceLookup1.SetLookup(this.label9, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblDatabaseV", "Database Version", ""));
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 16);
            this.label9.TabIndex = 65;
            this.label9.Text = "Database Version";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labDatabase
            // 
            this.labDatabase.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labDatabase.Location = new System.Drawing.Point(115, 168);
            this.labDatabase.Name = "labDatabase";
            this.labDatabase.Size = new System.Drawing.Size(259, 16);
            this.labDatabase.TabIndex = 57;
            this.labDatabase.Text = "Database ";
            this.labDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labcult
            // 
            this.labcult.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labcult.Location = new System.Drawing.Point(115, 190);
            this.labcult.Name = "labcult";
            this.labcult.Size = new System.Drawing.Size(259, 16);
            this.labcult.TabIndex = 64;
            this.labcult.Text = "Culture";
            this.labcult.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(7, 102);
            this.resourceLookup1.SetLookup(this.label4, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblUser", "User", ""));
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 16);
            this.label4.TabIndex = 58;
            this.label4.Text = "User";
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(7, 190);
            this.resourceLookup1.SetLookup(this.label5, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblCulture", "Culture", ""));
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(105, 16);
            this.label5.TabIndex = 63;
            this.label5.Text = "Culture ";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(7, 124);
            this.resourceLookup1.SetLookup(this.label3, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblprovider", "Provider", ""));
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 16);
            this.label3.TabIndex = 59;
            this.label3.Text = "Provider";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(7, 168);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblDatabase", "Database", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 16);
            this.label1.TabIndex = 61;
            this.label1.Text = "Database ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(7, 146);
            this.resourceLookup1.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblServer", "Server", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 16);
            this.label2.TabIndex = 60;
            this.label2.Text = "Server ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tpAssemblies
            // 
            this.tpAssemblies.Controls.Add(this.lstAssemblies);
            this.tpAssemblies.Controls.Add(this.button2);
            this.tpAssemblies.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpAssemblies, new FWBS.OMS.UI.Windows.ResourceLookupItem("tpAssemblies", "Assembly Versions", ""));
            this.tpAssemblies.Name = "tpAssemblies";
            this.tpAssemblies.Padding = new System.Windows.Forms.Padding(0, 35, 0, 0);
            this.tpAssemblies.Size = new System.Drawing.Size(556, 334);
            this.tpAssemblies.TabIndex = 2;
            this.tpAssemblies.Text = "Assembly Versions";
            this.tpAssemblies.UseVisualStyleBackColor = true;
            // 
            // lstAssemblies
            // 
            this.lstAssemblies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colAssemblyName,
            this.colAssemblyVersion,
            this.colAssemblyFileVersion,
            this.colAssemblyCompany,
            this.colAssemblyPKT});
            this.lstAssemblies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstAssemblies.FullRowSelect = true;
            listViewGroup1.Header = "Framework";
            listViewGroup1.Name = "grpAssemblyFramework";
            listViewGroup2.Header = "System";
            listViewGroup2.Name = "grpAssemblySystem";
            listViewGroup2.Tag = "System";
            listViewGroup3.Header = "3rd Party";
            listViewGroup3.Name = "grpAssembly3rdParty";
            listViewGroup4.Header = "Scripts";
            listViewGroup4.Name = "grpAssemblyScripts";
            listViewGroup4.Tag = "Scripts";
            listViewGroup5.Header = "Distributed";
            listViewGroup5.Name = "grpAssemblyDistributed";
            listViewGroup5.Tag = "Distributed";
            listViewGroup6.Header = "Dynamic";
            listViewGroup6.Name = "grpAssemblyDynamic";
            listViewGroup6.Tag = "Dynamic";
            this.lstAssemblies.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3,
            listViewGroup4,
            listViewGroup5,
            listViewGroup6});
            this.lstAssemblies.Location = new System.Drawing.Point(0, 35);
            this.lstAssemblies.MultiSelect = false;
            this.lstAssemblies.Name = "lstAssemblies";
            this.lstAssemblies.Size = new System.Drawing.Size(556, 299);
            this.lstAssemblies.SmallImageList = this.imageList1;
            this.lstAssemblies.TabIndex = 19;
            this.lstAssemblies.UseCompatibleStateImageBehavior = false;
            this.lstAssemblies.View = System.Windows.Forms.View.Details;
            // 
            // colAssemblyName
            // 
            this.colAssemblyName.Text = "Name";
            this.colAssemblyName.Width = 150;
            // 
            // colAssemblyVersion
            // 
            this.colAssemblyVersion.Text = "Version";
            this.colAssemblyVersion.Width = 70;
            // 
            // colAssemblyFileVersion
            // 
            this.colAssemblyFileVersion.Text = "File Version";
            this.colAssemblyFileVersion.Width = 72;
            // 
            // colAssemblyCompany
            // 
            this.colAssemblyCompany.Text = "Company";
            this.colAssemblyCompany.Width = 110;
            // 
            // colAssemblyPKT
            // 
            this.colAssemblyPKT.Text = "Public Key Token";
            this.colAssemblyPKT.Width = 120;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button2.Location = new System.Drawing.Point(3, 6);
            this.resourceLookup1.SetLookup(this.button2, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCopy", "Copy", ""));
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(57, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Copy";
            this.button2.Click += new System.EventHandler(this.btncopyInfo_Click);
            // 
            // tpDiag
            // 
            this.tpDiag.Controls.Add(this.tcDiags);
            this.tpDiag.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpDiag, new FWBS.OMS.UI.Windows.ResourceLookupItem("tpSessionCache", "Session Cache", ""));
            this.tpDiag.Name = "tpDiag";
            this.tpDiag.Size = new System.Drawing.Size(556, 334);
            this.tpDiag.TabIndex = 1;
            this.tpDiag.Text = "Session Cache";
            this.tpDiag.UseVisualStyleBackColor = true;
            // 
            // tcDiags
            // 
            this.tcDiags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcDiags.Location = new System.Drawing.Point(0, 0);
            this.tcDiags.Multiline = true;
            this.tcDiags.Name = "tcDiags";
            this.tcDiags.Padding = new System.Drawing.Point(0, 0);
            this.tcDiags.SelectedIndex = 0;
            this.tcDiags.Size = new System.Drawing.Size(556, 334);
            this.tcDiags.TabIndex = 0;
            // 
            // colLicType
            // 
            this.colLicType.Text = "Type";
            this.colLicType.Width = 250;
            // 
            // colLicAllocated
            // 
            this.colLicAllocated.Text = "Allocated";
            this.colLicAllocated.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colLicAllocated.Width = 80;
            // 
            // colLicTotal
            // 
            this.colLicTotal.Text = "Total";
            this.colLicTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.colLicTotal.Width = 80;
            // 
            // colLicIs
            // 
            this.colLicIs.Text = "Licensed";
            this.colLicIs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblVolume
            // 
            this.lblVolume.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblVolume.ForeColor = System.Drawing.Color.Purple;
            this.lblVolume.Location = new System.Drawing.Point(0, 69);
            this.resourceLookup1.SetLookup(this.lblVolume, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblVolume", "VOLUME LICENSE", ""));
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(556, 23);
            this.lblVolume.TabIndex = 22;
            this.lblVolume.Text = "VOLUME LICENSE";
            this.lblVolume.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUnlimited
            // 
            this.lblUnlimited.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblUnlimited.ForeColor = System.Drawing.Color.Blue;
            this.lblUnlimited.Location = new System.Drawing.Point(0, 46);
            this.resourceLookup1.SetLookup(this.lblUnlimited, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblUnlimited", "UNLIMITED TERMINAL LICENSE", ""));
            this.lblUnlimited.Name = "lblUnlimited";
            this.lblUnlimited.Size = new System.Drawing.Size(556, 23);
            this.lblUnlimited.TabIndex = 21;
            this.lblUnlimited.Text = "UNLIMITED TERMINAL LICENSE";
            this.lblUnlimited.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblInactive
            // 
            this.lblInactive.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblInactive.ForeColor = System.Drawing.Color.OliveDrab;
            this.lblInactive.Location = new System.Drawing.Point(0, 23);
            this.resourceLookup1.SetLookup(this.lblInactive, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblInactive", "INACTIVE SOFTWARE", ""));
            this.lblInactive.Name = "lblInactive";
            this.lblInactive.Size = new System.Drawing.Size(556, 23);
            this.lblInactive.TabIndex = 20;
            this.lblInactive.Text = "INACTIVE SOFTWARE";
            this.lblInactive.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDemo
            // 
            this.lblDemo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDemo.ForeColor = System.Drawing.Color.Red;
            this.lblDemo.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.lblDemo, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblDemo", "DEMO VERSION - Expires in %1% Days", ""));
            this.lblDemo.Name = "lblDemo";
            this.lblDemo.Size = new System.Drawing.Size(556, 23);
            this.lblDemo.TabIndex = 19;
            this.lblDemo.Text = "DEMO VERSION - Expires in %1% Days";
            this.lblDemo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tpPackages
            // 
            this.tpPackages.Controls.Add(this.dgPackages);
            this.tpPackages.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpPackages, new FWBS.OMS.UI.Windows.ResourceLookupItem("PACKAGES", "Packages", ""));
            this.tpPackages.Name = "tpPackages";
            this.tpPackages.Size = new System.Drawing.Size(556, 334);
            this.tpPackages.TabIndex = 5;
            this.tpPackages.Text = "Packages";
            this.tpPackages.UseVisualStyleBackColor = true;
            // 
            // dgPackages
            // 
            this.dgPackages.AllowNavigation = false;
            this.dgPackages.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dgPackages.CaptionText = "Installed Packages";
            this.dgPackages.DataMember = "";
            this.dgPackages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgPackages.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgPackages.Location = new System.Drawing.Point(0, 0);
            this.dgPackages.Name = "dgPackages";
            this.dgPackages.PreferredRowHeight = 18;
            this.dgPackages.RowHeadersVisible = false;
            this.dgPackages.Size = new System.Drawing.Size(556, 334);
            this.dgPackages.TabIndex = 0;
            this.dgPackages.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.dgsPackages});
            // 
            // dgsPackages
            // 
            this.dgsPackages.DataGrid = this.dgPackages;
            this.dgsPackages.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dcpCode,
            this.dcpVersion,
            this.dcpInstalled});
            this.dgsPackages.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
            this.dgsPackages.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dgsPackages.MappingName = "INSTALLED";
            this.dgsPackages.PreferredRowHeight = 18;
            this.dgsPackages.ReadOnly = true;
            this.dgsPackages.RowHeadersVisible = false;
            // 
            // dcpCode
            // 
            this.dcpCode.Format = "";
            this.dcpCode.FormatInfo = null;
            this.dcpCode.HeaderText = "Code";
            this.dcpCode.ImageColumn = "";
            this.dcpCode.ImageIndex = 24;
            this.resourceLookup1.SetLookup(this.dcpCode, new FWBS.OMS.UI.Windows.ResourceLookupItem("CODE", "Code", ""));
            this.dcpCode.MappingName = "pkgCode";
            this.dcpCode.Resources = FWBS.OMS.UI.Windows.omsImageLists.AdminMenu16;
            this.dcpCode.Width = 250;
            // 
            // dcpVersion
            // 
            this.dcpVersion.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.dcpVersion.AllowMultiSelect = false;
            this.dcpVersion.DisplayDateAs = FWBS.OMS.SearchEngine.SearchColumnsDateIs.NotApplicable;
            this.dcpVersion.Format = "";
            this.dcpVersion.FormatInfo = null;
            this.dcpVersion.HeaderText = "Version";
            this.dcpVersion.ImageColumn = "";
            this.dcpVersion.ImageIndex = -1;
            this.dcpVersion.ImageList = null;
            this.resourceLookup1.SetLookup(this.dcpVersion, new FWBS.OMS.UI.Windows.ResourceLookupItem("VERSION", "Version", ""));
            this.dcpVersion.MappingName = "pkgVersion";
            this.dcpVersion.ReadOnly = true;
            this.dcpVersion.SearchList = null;
            this.dcpVersion.SourceDateIs = FWBS.OMS.SearchEngine.SearchColumnsDateIs.NotApplicable;
            this.dcpVersion.Width = 101;
            // 
            // dcpInstalled
            // 
            this.dcpInstalled.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.dcpInstalled.AllowMultiSelect = false;
            this.dcpInstalled.DisplayDateAs = FWBS.OMS.SearchEngine.SearchColumnsDateIs.NotApplicable;
            this.dcpInstalled.Format = "d";
            this.dcpInstalled.FormatInfo = null;
            this.dcpInstalled.HeaderText = "Installed";
            this.dcpInstalled.ImageColumn = "";
            this.dcpInstalled.ImageIndex = -1;
            this.dcpInstalled.ImageList = null;
            this.resourceLookup1.SetLookup(this.dcpInstalled, new FWBS.OMS.UI.Windows.ResourceLookupItem("dcpInstalled", "Installed", ""));
            this.dcpInstalled.MappingName = "Created";
            this.dcpInstalled.ReadOnly = true;
            this.dcpInstalled.SearchList = null;
            this.dcpInstalled.SourceDateIs = FWBS.OMS.SearchEngine.SearchColumnsDateIs.NotApplicable;
            this.dcpInstalled.Width = 101;
            // 
            // tpAddins
            // 
            this.tpAddins.Controls.Add(this.lvwAddins);
            this.tpAddins.Controls.Add(this.txtAddinDescription);
            this.tpAddins.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpAddins, new FWBS.OMS.UI.Windows.ResourceLookupItem("tpAddins", "Addins", ""));
            this.tpAddins.Name = "tpAddins";
            this.tpAddins.Size = new System.Drawing.Size(556, 334);
            this.tpAddins.TabIndex = 6;
            this.tpAddins.Text = "Addins";
            this.tpAddins.UseVisualStyleBackColor = true;
            // 
            // lvwAddins
            // 
            this.lvwAddins.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lvwAddins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwAddins.FullRowSelect = true;
            listViewGroup7.Header = "Loaded";
            listViewGroup7.Name = "Loaded";
            listViewGroup8.Header = "Unloaded";
            listViewGroup8.Name = "Unloaded";
            listViewGroup9.Header = "Errors";
            listViewGroup9.Name = "Errors";
            listViewGroup10.Header = "Disabled";
            listViewGroup10.Name = "Disabled";
            this.lvwAddins.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup7,
            listViewGroup8,
            listViewGroup9,
            listViewGroup10});
            this.lvwAddins.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwAddins.Location = new System.Drawing.Point(0, 0);
            this.lvwAddins.MultiSelect = false;
            this.lvwAddins.Name = "lvwAddins";
            this.lvwAddins.Size = new System.Drawing.Size(556, 289);
            this.lvwAddins.SmallImageList = this.imageList1;
            this.lvwAddins.TabIndex = 21;
            this.lvwAddins.UseCompatibleStateImageBehavior = false;
            this.lvwAddins.View = System.Windows.Forms.View.Details;
            this.lvwAddins.SelectedIndexChanged += new System.EventHandler(this.lstAddin_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 250;
            // 
            // txtAddinDescription
            // 
            this.txtAddinDescription.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtAddinDescription.Location = new System.Drawing.Point(0, 289);
            this.txtAddinDescription.Multiline = true;
            this.txtAddinDescription.Name = "txtAddinDescription";
            this.txtAddinDescription.ReadOnly = true;
            this.txtAddinDescription.Size = new System.Drawing.Size(556, 45);
            this.txtAddinDescription.TabIndex = 22;
            // 
            // tpPartner
            // 
            this.tpPartner.Controls.Add(this.panel2);
            this.tpPartner.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpPartner, new FWBS.OMS.UI.Windows.ResourceLookupItem("tpPartner", "Partner Information", ""));
            this.tpPartner.Name = "tpPartner";
            this.tpPartner.Size = new System.Drawing.Size(556, 334);
            this.tpPartner.TabIndex = 3;
            this.tpPartner.Text = "Partner Information";
            this.tpPartner.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblPartnerInfo);
            this.panel2.Controls.Add(this.lblPartnerWeb);
            this.panel2.Controls.Add(this.lblPartnerName);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.lblPartnerAddress);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.lblPartnerSuppTel);
            this.panel2.Controls.Add(this.lblSuppInfo);
            this.panel2.Controls.Add(this.lblPartnerGeneralTel);
            this.panel2.Controls.Add(this.lblPAddress);
            this.panel2.Controls.Add(this.lblName);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(556, 334);
            this.panel2.TabIndex = 78;
            // 
            // lblPartnerInfo
            // 
            this.lblPartnerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPartnerInfo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPartnerInfo.Location = new System.Drawing.Point(4, 9);
            this.resourceLookup1.SetLookup(this.lblPartnerInfo, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblPartnerInfo", "Below is the Contact Details for the Partner/Reseller of your software is license" +
            "d from, this information is correct at time of licensing, full details available" +
            " on our website. https://www.elite.com/products/3e/3e-matter-sphere", ""));
            this.lblPartnerInfo.Name = "lblPartnerInfo";
            this.lblPartnerInfo.Size = new System.Drawing.Size(546, 74);
            this.lblPartnerInfo.TabIndex = 65;
            this.lblPartnerInfo.Text = "Below is the Contact Details for the Partner/Reseller of your software is license" +
            "d from, this information is correct at time of licensing, full details available" +
            " on our website. https://www.elite.com/products/3e/3e-matter-sphere";
            this.lblPartnerInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPartnerWeb
            // 
            this.lblPartnerWeb.Location = new System.Drawing.Point(84, 220);
            this.lblPartnerWeb.Name = "lblPartnerWeb";
            this.lblPartnerWeb.Size = new System.Drawing.Size(205, 16);
            this.lblPartnerWeb.TabIndex = 77;
            this.lblPartnerWeb.TabStop = true;
            this.lblPartnerWeb.Text = "https://www.elite.com/products/3e/3e-matter-sphere";
            this.lblPartnerWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblPartnerWeb_LinkClicked);
            // 
            // lblPartnerName
            // 
            this.lblPartnerName.Location = new System.Drawing.Point(84, 83);
            this.lblPartnerName.Name = "lblPartnerName";
            this.lblPartnerName.Size = new System.Drawing.Size(204, 16);
            this.lblPartnerName.TabIndex = 67;
            this.lblPartnerName.Text = "partnername";
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(4, 220);
            this.resourceLookup1.SetLookup(this.label6, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblWebSite", "WebSite", ""));
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 16);
            this.label6.TabIndex = 76;
            this.label6.Text = "WebSite";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPartnerAddress
            // 
            this.lblPartnerAddress.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPartnerAddress.Location = new System.Drawing.Point(84, 99);
            this.lblPartnerAddress.Name = "lblPartnerAddress";
            this.lblPartnerAddress.Size = new System.Drawing.Size(312, 82);
            this.lblPartnerAddress.TabIndex = 68;
            this.lblPartnerAddress.Text = "partneraddress";
            this.lblPartnerAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(4, 200);
            this.resourceLookup1.SetLookup(this.label8, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblGenTel", "General Tel", ""));
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 16);
            this.label8.TabIndex = 74;
            this.label8.Text = "General Tel";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPartnerSuppTel
            // 
            this.lblPartnerSuppTel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPartnerSuppTel.Location = new System.Drawing.Point(84, 181);
            this.lblPartnerSuppTel.Name = "lblPartnerSuppTel";
            this.lblPartnerSuppTel.Size = new System.Drawing.Size(120, 16);
            this.lblPartnerSuppTel.TabIndex = 69;
            this.lblPartnerSuppTel.Text = "partnertel";
            this.lblPartnerSuppTel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSuppInfo
            // 
            this.lblSuppInfo.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblSuppInfo.Location = new System.Drawing.Point(4, 181);
            this.resourceLookup1.SetLookup(this.lblSuppInfo, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblSuppInfo", "Support Tel", ""));
            this.lblSuppInfo.Name = "lblSuppInfo";
            this.lblSuppInfo.Size = new System.Drawing.Size(80, 16);
            this.lblSuppInfo.TabIndex = 73;
            this.lblSuppInfo.Text = "Support Tel";
            this.lblSuppInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPartnerGeneralTel
            // 
            this.lblPartnerGeneralTel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPartnerGeneralTel.Location = new System.Drawing.Point(84, 200);
            this.lblPartnerGeneralTel.Name = "lblPartnerGeneralTel";
            this.lblPartnerGeneralTel.Size = new System.Drawing.Size(120, 16);
            this.lblPartnerGeneralTel.TabIndex = 70;
            this.lblPartnerGeneralTel.Text = "generaltel";
            this.lblPartnerGeneralTel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPAddress
            // 
            this.lblPAddress.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPAddress.Location = new System.Drawing.Point(4, 100);
            this.resourceLookup1.SetLookup(this.lblPAddress, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblPAddress", "Provider", ""));
            this.lblPAddress.Name = "lblPAddress";
            this.lblPAddress.Size = new System.Drawing.Size(80, 16);
            this.lblPAddress.TabIndex = 72;
            this.lblPAddress.Text = "Provider";
            this.lblPAddress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(4, 83);
            this.resourceLookup1.SetLookup(this.lblName, new FWBS.OMS.UI.Windows.ResourceLookupItem("NAME", "Name", ""));
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(80, 16);
            this.lblName.TabIndex = 71;
            this.lblName.Text = "Name";
            // 
            // tpServices
            // 
            this.tpServices.Controls.Add(this.lvwServices);
            this.tpServices.Controls.Add(this.txtServicesMessage);
            this.tpServices.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpServices, new FWBS.OMS.UI.Windows.ResourceLookupItem("tpServices", "Services", ""));
            this.tpServices.Name = "tpServices";
            this.tpServices.Size = new System.Drawing.Size(556, 334);
            this.tpServices.TabIndex = 7;
            this.tpServices.Text = "Services";
            this.tpServices.UseVisualStyleBackColor = true;
            // 
            // lvwServices
            // 
            this.lvwServices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colServicesName});
            this.lvwServices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwServices.FullRowSelect = true;
            listViewGroup11.Header = "Connected";
            listViewGroup11.Name = "Connected";
            listViewGroup12.Header = "Disconnected";
            listViewGroup12.Name = "Disconnected";
            this.lvwServices.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup11,
            listViewGroup12});
            this.lvwServices.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvwServices.Location = new System.Drawing.Point(0, 0);
            this.lvwServices.MultiSelect = false;
            this.lvwServices.Name = "lvwServices";
            this.lvwServices.Size = new System.Drawing.Size(556, 289);
            this.lvwServices.SmallImageList = this.imageList1;
            this.lvwServices.TabIndex = 19;
            this.lvwServices.UseCompatibleStateImageBehavior = false;
            this.lvwServices.View = System.Windows.Forms.View.Details;
            this.lvwServices.SelectedIndexChanged += new System.EventHandler(this.lvwServices_SelectedIndexChanged);
            // 
            // colServicesName
            // 
            this.colServicesName.Text = "Name";
            this.colServicesName.Width = 250;
            // 
            // txtServicesMessage
            // 
            this.txtServicesMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtServicesMessage.Location = new System.Drawing.Point(0, 289);
            this.txtServicesMessage.Multiline = true;
            this.txtServicesMessage.Name = "txtServicesMessage";
            this.txtServicesMessage.ReadOnly = true;
            this.txtServicesMessage.Size = new System.Drawing.Size(556, 45);
            this.txtServicesMessage.TabIndex = 20;
            // 
            // tpConnectedClients
            // 
            this.tpConnectedClients.Controls.Add(this.lvwConnectedClients);
            this.tpConnectedClients.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpConnectedClients, new FWBS.OMS.UI.Windows.ResourceLookupItem("tpConnClient", "Connected Clients", ""));
            this.tpConnectedClients.Name = "tpConnectedClients";
            this.tpConnectedClients.Padding = new System.Windows.Forms.Padding(3);
            this.tpConnectedClients.Size = new System.Drawing.Size(556, 334);
            this.tpConnectedClients.TabIndex = 8;
            this.tpConnectedClients.Text = "Connected Clients";
            this.tpConnectedClients.UseVisualStyleBackColor = true;
            // 
            // lvwConnectedClients
            // 
            this.lvwConnectedClients.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colCCName,
            this.colCCProcId,
            this.colCCPath});
            this.lvwConnectedClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwConnectedClients.FullRowSelect = true;
            listViewGroup13.Header = "Connected";
            listViewGroup13.Name = "Connected";
            listViewGroup14.Header = "Disconnected";
            listViewGroup14.Name = "Disconnected";
            this.lvwConnectedClients.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup13,
            listViewGroup14});
            this.lvwConnectedClients.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvwConnectedClients.Location = new System.Drawing.Point(3, 3);
            this.lvwConnectedClients.MultiSelect = false;
            this.lvwConnectedClients.Name = "lvwConnectedClients";
            this.lvwConnectedClients.ShowGroups = false;
            this.lvwConnectedClients.Size = new System.Drawing.Size(550, 328);
            this.lvwConnectedClients.SmallImageList = this.imageList1;
            this.lvwConnectedClients.TabIndex = 20;
            this.lvwConnectedClients.UseCompatibleStateImageBehavior = false;
            this.lvwConnectedClients.View = System.Windows.Forms.View.Details;
            // 
            // colCCName
            // 
            this.colCCName.Text = "Name";
            this.colCCName.Width = 150;
            // 
            // colCCProcId
            // 
            this.colCCProcId.Text = "Process Id";
            this.colCCProcId.Width = 70;
            // 
            // colCCPath
            // 
            this.colCCPath.Text = "Location";
            this.colCCPath.Width = 250;
            // 
            // pnlStatus
            // 
            this.pnlStatus.Location = new System.Drawing.Point(0, 0);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Size = new System.Drawing.Size(200, 100);
            this.pnlStatus.TabIndex = 0;
            // 
            // picLogo2
            // 
            this.picLogo2.BackColor = System.Drawing.Color.White;
            this.picLogo2.Dock = System.Windows.Forms.DockStyle.Top;
            this.picLogo2.Image = ((System.Drawing.Image)(resources.GetObject("picLogo2.Image")));
            this.picLogo2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.picLogo2.Location = new System.Drawing.Point(0, 0);
            this.picLogo2.Margin = new System.Windows.Forms.Padding(0);
            this.picLogo2.Name = "picLogo2";
            this.picLogo2.Size = new System.Drawing.Size(620, 105);
            this.picLogo2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogo2.TabIndex = 41;
            this.picLogo2.TabStop = false;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOK.Location = new System.Drawing.Point(498, 9);
            this.resourceLookup1.SetLookup(this.btnOK, new FWBS.OMS.UI.Windows.ResourceLookupItem("OK", "OK", ""));
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(60, 25);
            this.btnOK.TabIndex = 42;
            this.btnOK.Text = "OK";
            // 
            // labUserCap
            // 
            this.labUserCap.Location = new System.Drawing.Point(0, 0);
            this.labUserCap.Name = "labUserCap";
            this.labUserCap.Size = new System.Drawing.Size(100, 23);
            this.labUserCap.TabIndex = 0;
            // 
            // btnSysInfo
            // 
            this.btnSysInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSysInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSysInfo.Location = new System.Drawing.Point(407, 9);
            this.resourceLookup1.SetLookup(this.btnSysInfo, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnSysInfo", "System Info...", ""));
            this.btnSysInfo.Name = "btnSysInfo";
            this.btnSysInfo.Size = new System.Drawing.Size(85, 25);
            this.btnSysInfo.TabIndex = 43;
            this.btnSysInfo.Text = "System Info...";
            this.btnSysInfo.Click += new System.EventHandler(this.btnSysInfo_Click);
            // 
            // pnlButtons
            // 
            this.pnlButtons.BackColor = System.Drawing.Color.White;
            this.pnlButtons.Controls.Add(this.btnSysInfo);
            this.pnlButtons.Controls.Add(this.btnOK);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlButtons.Location = new System.Drawing.Point(0, 467);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(564, 44);
            this.pnlButtons.TabIndex = 79;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tabControl);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 105);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(564, 362);
            this.pnlMain.TabIndex = 70;
            // 
            // frmAbout
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(624, 511);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.picLogo2);
            this.Controls.Add(this.pnlButtons);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(624, 530);
            this.Name = "frmAbout";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About...";
            this.Load += new System.EventHandler(this.frmAbout_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.tabControl_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.tabControl_DragDrop);
            this.tabControl.ResumeLayout(false);
            this.tpGeneral.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tpAssemblies.ResumeLayout(false);
            this.tpDiag.ResumeLayout(false);
            this.tpPackages.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgPackages)).EndInit();
            this.tpAddins.ResumeLayout(false);
            this.tpAddins.PerformLayout();
            this.tpPartner.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tpServices.ResumeLayout(false);
            this.tpServices.PerformLayout();
            this.tpConnectedClients.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLogo2)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        #endregion Constructors

        #region Captured Events

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);
            ApplyImages();
        }

        private void frmAbout_Load(object sender, System.EventArgs e)
		{
			if (!DesignMode)
			{
				Global.ControlParser(this);
			}

            try
            {
                //Load the branding logo
                Image aboutlogo = Branding.GetAboutLogo();
                if (aboutlogo != null)
                    picLogo2.Image = aboutlogo;
                
            }
            catch { }

            PopulateAppStats();
            PopulateMemoryCache();
            PopulateAssemblies();
            PopulatePackages();
			PopulateAddins();
            PopulateServices();
            PopulateConnectedClients();
            
			tabControl.SelectedIndex = 0;
            SetColumnHeaderLookups();
		}

        private void btnSysInfo_Click(object sender, System.EventArgs e)
        {
            System.Diagnostics.Process.Start(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "MSInfo32.exe"));
        }

        #endregion

        #region Application Specifics

        private void PopulateAppStats()
        {
            Session _session = Session.OMS;

            this.Text = FWBS.OMS.Global.ApplicationName;
            AssemblyCopyrightAttribute cpy = (AssemblyCopyrightAttribute)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute));

            lblPartnerName.Text = _session.PartnerCompanyName;
            try
            {
                lblPartnerAddress.Text = _session.PartnerAddress.ToString();
            }
            catch (OMSException)
            {
                lblPartnerAddress.Text = String.Empty;
            }
            lblPartnerWeb.Text = _session.PartnerWebSite;
            lblPartnerSuppTel.Text = _session.PartnerSupportTelephone;
            lblPartnerGeneralTel.Text = _session.PartnerTelephone;

            lblCopyright.Text = cpy.Copyright;
            labUser.Text = _session.CurrentUser.FullName + " (" + _session.CurrentUser.Initials + ")";
            labDatabase.Text = _session.CurrentDatabase.DatabaseName.ToUpper();
            labServer.Text = _session.CurrentDatabase.Server;
            LabProvider.Text = _session.CurrentDatabase.Provider;
            labcult.Text = System.Threading.Thread.CurrentThread.CurrentUICulture.NativeName + " (" + System.Threading.Thread.CurrentThread.CurrentUICulture.Name + ")";
            lblDBVersion.Text = _session.DatabaseVersion.ToString();
            Version version = _session.EngineVersion;
            lblEngineVersion.Text = string.Format("{0} (Build {1})", version.ToString(3), version.Revision);
        }

        #endregion

        #region Packages

        private void PopulatePackages()
        {
            System.Data.DataView dv = new System.Data.DataView(FWBS.OMS.Design.Package.Packages.GetImportedPackageList());
            dv.AllowNew = false;
            dgPackages.DataSource = dv;
        }

        #endregion

        #region Assemblies

        private void PopulateAssemblies()
        {
            lstAssemblies.SetGroupState(ListViewGroupState.Collapsible | ListViewGroupState.Collapsed);

            var assemblies = GetLoadedAssemblies();

            if (!imageList1.Images.ContainsKey("ASSEMBLY"))
            {
                imageList1.Images.Add("ASSEMBLY", FWBS.Common.IconReader.GetFileIcon("test.dll", Common.IconReader.IconSize.Small, false).ToBitmap());
                ApplyImages(lstAssemblies);
            }

            foreach (var a in assemblies)
            {
                ListViewGroup grp = null;

                var an = a.GetName();

                var pkt = GetPublicKeyToken(an);

                if (IsScriptAssembly(a))
                    grp = lstAssemblies.Groups["grpAssemblyScripts"];
                else if (a.IsDynamic)
                    grp = lstAssemblies.Groups["grpAssemblyDynamic"];
                else if (Session.CurrentSession.DistributedAssemblyManager.IsDistributedAssembly(a))
                    grp = lstAssemblies.Groups["grpAssemblyDistributed"];
                else if (Fwbs.Framework.ProductInfo.Tokens.PublicKeyToken.Equals(pkt, StringComparison.OrdinalIgnoreCase))
                    grp = lstAssemblies.Groups["grpAssemblyFramework"];
                else if (Fwbs.Framework.Licensing.API.CompanyInfo.Microsoft1.PublicKeyToken.Equals(pkt, StringComparison.OrdinalIgnoreCase)
                    || Fwbs.Framework.Licensing.API.CompanyInfo.Microsoft2.PublicKeyToken.Equals(pkt, StringComparison.OrdinalIgnoreCase)
                    || Fwbs.Framework.Licensing.API.CompanyInfo.Microsoft3.PublicKeyToken.Equals(pkt, StringComparison.OrdinalIgnoreCase)
                    || Fwbs.Framework.Licensing.API.CompanyInfo.Microsoft4.PublicKeyToken.Equals(pkt, StringComparison.OrdinalIgnoreCase)
                    || Fwbs.Framework.Licensing.API.CompanyInfo.Microsoft5.PublicKeyToken.Equals(pkt, StringComparison.OrdinalIgnoreCase)
                    )
                    grp = lstAssemblies.Groups["grpAssemblySystem"];
                else
                    grp = lstAssemblies.Groups["grpAssembly3rdParty"];

                var item = lstAssemblies.Items.Add(an.Name);
                item.ImageKey = "ASSEMBLY";
                item.SubItems.Add(an.Version.ToString());

                var fv = a.Attribute<AssemblyFileVersionAttribute>();
                item.SubItems.Add(fv == null ? an.Version.ToString() : fv.Version);

                var comp = a.Attribute<AssemblyCompanyAttribute>();
                item.SubItems.Add(comp == null ? "" : comp.Company);

                item.SubItems.Add(pkt);

                grp.Items.Add(item);
           }


            foreach (ListViewGroup item in lstAssemblies.Groups)
            {
                lstAssemblies.SetGroupFooter(item, String.Format("{0} loaded...", item.Items.Count));
            }
        }

        private static bool IsScriptAssembly(Assembly assembly)
        {
            return assembly.Attribute<FWBS.OMS.Script.ScriptGenAssemblyAttribute>() != null;
        }

        private static string GetPublicKeyToken(AssemblyName an)
        {
            
            var pkt = an.GetPublicKeyToken();

            if (pkt == null)
                return null;

            var buff = new System.Text.StringBuilder();
            foreach (byte b in pkt)
            {
                buff.Append(String.Format("{0:X2}", b));
            }

            return buff.ToString();
        }

        private static IEnumerable<Assembly> GetLoadedAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().OrderBy(a => a.FullName);
        }

        private void btncopyInfo_Click(object sender, System.EventArgs e)
        {
            StringBuilder strbuild = new StringBuilder("Assemblies....................................");

            strbuild.Append(Environment.NewLine);

            lstAssemblies.ToCSV(strbuild);

            Clipboard.SetDataObject(strbuild.ToString());

            System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("COPIED2CLP", "Copied to Clipboard!", "").Text);
        }

        

        private void tabControl_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            MessageBox.Show(e.Data.GetData("System.String", true).ToString());
        }


        #endregion

        #region Partner Info

        private void lblPartnerWeb_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(((System.Windows.Forms.LinkLabel)sender).Text);
        }

        #endregion

        #region Memory Cache

        private void PopulateMemoryCache()
        {
            Session _session = Session.CurrentSession;


            foreach (TabPage tp in tcDiags.TabPages)
            {
                Global.RemoveAndDisposeControls(tp);
            }
            tcDiags.TabPages.Clear();
            foreach (string k in _session.MemoryCache.Keys)
            {
                TabPage tp = new TabPage(k);
                ListBox lb = new ListBox();
                lb.DataSource = _session.MemoryCache[k].Values;
                lb.Dock = DockStyle.Fill;
                tp.Controls.Add(lb);
                tcDiags.AddTabPage(tp);
                
            }
        }

        private void btnClearCache_Click(object sender, System.EventArgs e)
        {
           Session.CurrentSession.ClearCache();
           PopulateMemoryCache();

        }

        #endregion
       
        #region Addins

        private void PopulateAddins()
		{
			try
			{
				lvwAddins.Items.Clear();

				if (Session.CurrentSession.EnableAddins == false)
				{
					tabControl.TabPages.Remove(tpAddins);
					return;
				}

				FWBS.OMS.Extensibility.AddinManager manager = Session.CurrentSession.Addins;

				foreach (FWBS.OMS.Extensibility.OMSAddin addin in manager)
				{
                    ListViewItem item = new ListViewItem(addin.ToString());
                    lvwAddins.Items.Add(item);

                    switch (addin.Status)
                    {
                        case FWBS.OMS.Extensibility.AddinStatus.Disabled:
                            lvwAddins.Groups["Disabled"].Items.Add(item);
                            break;
                        case FWBS.OMS.Extensibility.AddinStatus.Errors:
                            lvwAddins.Groups["Errors"].Items.Add(item);
                            break;
                        case FWBS.OMS.Extensibility.AddinStatus.Loaded:
                            lvwAddins.Groups["Loaded"].Items.Add(item);
                            break;
                        case FWBS.OMS.Extensibility.AddinStatus.Unloaded:
                            lvwAddins.Groups["Unloaded"].Items.Add(item);
                            break;

                    }
                    item.Tag = addin;
				}
			}
			catch
			{
				tabControl.TabPages.Remove(tpAddins);
			}

        }

        private void lstAddin_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            try
            {
                txtAddinDescription.Text = "...";
                if (lvwAddins.SelectedItems.Count > 0)
                {
                    FWBS.OMS.Extensibility.OMSAddin addin = (FWBS.OMS.Extensibility.OMSAddin)lvwAddins.SelectedItems[0].Tag;
                    System.Text.StringBuilder sb = new StringBuilder();
                    sb.Append(addin.Description);
                    foreach (Exception ex in addin.Errors)
                    {
                        sb.Append(Environment.NewLine);
                        sb.Append(ex.Message);
                    }
                    txtAddinDescription.Text = sb.ToString();
                }
            }
            catch
            { }

        }

        #endregion

        #region Services

        private void PopulateServices()
        {
            try
            {
                lvwServices.Items.Clear();

                Connectivity.IConnectableService[] services = Connectivity.ConnectivityManager.CurrentManager.GetServices();

                Connectivity.ConnectivityManager.CurrentManager.Connected -= new EventHandler(CurrentManager_Connected);
                Connectivity.ConnectivityManager.CurrentManager.Disconnected -= new MessageEventHandler(CurrentManager_Disconnected);
                Connectivity.ConnectivityManager.CurrentManager.Connected += new EventHandler(CurrentManager_Connected);
                Connectivity.ConnectivityManager.CurrentManager.Disconnected += new MessageEventHandler(CurrentManager_Disconnected);

                foreach (Connectivity.IConnectableService srv in services)
                {
                    if (srv.IsConnected)
                        AddService(srv);
                    else
                        RemoveService(srv);
                }
            }
            catch
            {
                tabControl.TabPages.Remove(tpServices);
            }

        }

        private delegate void ServiceChanged(Connectivity.IConnectableService service);

        private void RemoveService(Connectivity.IConnectableService service)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ServiceChanged(RemoveService), service);
                return;
            }

            string name = service.Id.ToString();
            if (lvwServices.Items.ContainsKey(name))
            {
                ListViewItem item = lvwServices.Groups["Connected"].Items[name];
                item.Group = lvwServices.Groups["Disconnected"];
                item.Tag = service;
            }
            else
            {
                ListViewItem item = new ListViewItem(service.ServiceName);
                item.Name = name;
                lvwServices.Items.Add(item);
                lvwServices.Groups["Disconnected"].Items.Add(item);
                item.Tag = service;
            }

        }

        private void AddService(Connectivity.IConnectableService service)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new ServiceChanged(AddService), service);
                return;
            }

            string name = service.Id.ToString();
            if (lvwServices.Items.ContainsKey(name))
            {
                ListViewItem item = lvwServices.Groups["Disconnected"].Items[name];
                item.Group = lvwServices.Groups["Connected"];
                item.Tag = service;
            }
            else
            {
                ListViewItem item = new ListViewItem(service.ServiceName);
                item.Name = name;
                lvwServices.Items.Add(item);
                lvwServices.Groups["Connected"].Items.Add(item);
                item.Tag = service;
            }

        }

        private void CurrentManager_Disconnected(object sender, MessageEventArgs e)
        {
            RemoveService((Connectivity.IConnectableService)sender);
        }

        private void CurrentManager_Connected(object sender, EventArgs e)
        {
            AddService((Connectivity.IConnectableService)sender);
        }

        private void lvwServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtServicesMessage.Text = "...";
                if (lvwServices.SelectedItems.Count > 0)
                {
                    Connectivity.IConnectableService service = (Connectivity.IConnectableService)lvwServices.SelectedItems[0].Tag;
                    System.Text.StringBuilder sb = new StringBuilder();

                    foreach (string msg in service.Messages)
                    {
                        if (msg == null)
                            continue;

                        sb.Append(msg);
                        sb.Append(Environment.NewLine);
                    }

                    txtServicesMessage.Text = sb.ToString();
                }
            }
            catch
            { }
        }


        #endregion


        #region Services

        private void PopulateConnectedClients()
        {
            try
            {
                lvwConnectedClients.Items.Clear();



                ConnectedClientInfo[] clients = Session.CurrentSession.GetConnectedClients();
                foreach (ConnectedClientInfo info in clients)
                {
                    string key = info.Location.FullName;

                    if (imageList1.Images.ContainsKey(key) == false)
                    {
                        IntPtr handle = IntPtr.Zero;
                        try
                        {
                            handle = info.Process.MainWindowHandle;
                        }
                        catch { }

                        System.Drawing.Icon ico = FWBS.Common.Functions.GetSmallWindowIcon(handle);
                        if (ico == null && info.Location.Exists)
                            ico = Icon.ExtractAssociatedIcon(info.Location.FullName);
                        
                        if (ico != null)
                            imageList1.Images.Add(key, ico.ToBitmap());
                    }
                    ApplyImages(lvwConnectedClients);

                    ListViewItem item = new ListViewItem(new string[] { info.Name, info.ProcessId.ToString(), info.Location.FullName });
                    lvwConnectedClients.Items.Add(item);
                    item.ImageKey = key;
                }
            }
            catch
            {
                tabControl.TabPages.Remove(tpConnectedClients);
            }

        }


        #endregion

        private void SetColumnHeaderLookups()
        {
            Res res = Session.CurrentSession.Resources;
            this.columnHeader1.Text = res.GetResource("NAME", "Name", "").Text;
            this.colAssemblyName.Text = this.colServicesName.Text = this.colCCName.Text = this.columnHeader1.Text;
            this.colAssemblyVersion.Text = res.GetResource("VERSION", "Version", "").Text;
            this.colAssemblyFileVersion.Text = res.GetResource("colAssemblFVsn", "File Version", "").Text;
            this.colAssemblyCompany.Text = res.GetResource("colAssemblyCmpn", "Company", "").Text;
            this.colAssemblyPKT.Text = res.GetResource("colAssemblyPKT", "Public Key Token", "").Text;
            this.colLicType.Text = res.GetResource("colLicType", "Type", "").Text;
            this.colLicAllocated.Text = res.GetResource("colLicAllocated", "Allocated", "").Text;
            this.colLicTotal.Text = res.GetResource("colLicTotal", "Total", "").Text;
            this.colLicIs.Text = res.GetResource("colLicIs", "Licensed", "").Text;
            this.colCCProcId.Text = res.GetResource("colCCProcId", "Process Id", "").Text;
            this.colCCPath.Text = res.GetResource("Location", "Location", "").Text;
        }

        private void lblPrivacyPolicy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lblPrivacyPolicy.LinkVisited = true;
            System.Diagnostics.Process.Start("https://www.elite.com/privacy-statement");
        }

        private void ApplyImages()
        {
            ApplyImages(lstAssemblies);
            ApplyImages(lvwAddins);
            ApplyImages(lvwConnectedClients);
            ApplyImages(lvwServices);
        }

        private void ApplyImages(ListView listView)
        {
            listView.SmallImageList = null;
            listView.SmallImageList = FWBS.OMS.UI.Windows.Images.ScaleList(imageList1, LogicalToDeviceUnits(new Size(16, 16)));
        }
    }
}
