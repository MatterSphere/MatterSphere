using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.OMSEXPORT
{
	/// <summary>
	/// Allows the user to configure the registry values for the service.
	/// These procedues will have to be modified as each export object comes along
	/// GetExportObjectValues()
	/// chkExportObject_CheckedChanged
	/// GetInitialValues()
	/// UpdateValues()
	/// </summary>
	public class frmSettings : System.Windows.Forms.Form
	{
		
		#region Fields
		/// <summary>
		/// Registry constants
		/// </summary>
		private const string DIALOG_CAPTION = "FWBS OMS Export Service";
		/// <summary>
		/// Application name from registry
		/// </summary>
		private string _exportApp;
        /// <summary>
        /// Form controls
        /// </summary>
        private System.Windows.Forms.Button btnUpdate;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.CheckBox chkEmailAdmin;
		private System.Windows.Forms.TextBox txtEmailAddress;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtSmtpServer;
        private System.Windows.Forms.Label lblSmtpEncryption;
        private System.Windows.Forms.ComboBox cmbSmtpEncryption;
        private System.Windows.Forms.CheckBox chkSmtpAuthenticate;
        private System.Windows.Forms.Label lblSmtpLogin;
        private System.Windows.Forms.TextBox txtSmtpLogin;
        private System.Windows.Forms.Label lblSmtpPassword;
        private System.Windows.Forms.TextBox txtSmtpPassword;
        private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnTestSQL;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox txtFilePath;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.Label label1;
		
		private System.Windows.Forms.CheckBox chkFullEventLog;
		private System.Windows.Forms.TextBox txtPauseMins;
		private System.Windows.Forms.TextBox txtServerName;
		private System.Windows.Forms.TextBox txtDatabaseName;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.CheckBox chkIntegrated;
		private System.Windows.Forms.CheckBox chkAzure;
		private System.Windows.Forms.GroupBox grp4;
		private System.Windows.Forms.GroupBox grp3;
		private System.Windows.Forms.GroupBox grp2;
		private System.Windows.Forms.GroupBox grp1;
		private System.Windows.Forms.GroupBox grpCMS;
		private System.Windows.Forms.TextBox txtCMSURL;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.TextBox txtCMSPassword;
		private System.Windows.Forms.TextBox txtCMSUsername;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.CheckBox chkCommon;
		private System.Windows.Forms.CheckBox chkCMS;
		private System.Windows.Forms.CheckBox chkCMSIntegrated;
        private CheckBox chkExports;
        private GroupBox grpExports;
        private CheckBox chkExportMatters;
        private CheckBox chkUpdateClients;
        private CheckBox chkExportClients;
        private CheckBox chkExportUsers;
        private CheckBox chkExportFinancials;
        private CheckBox chkExportTime;
        private CheckBox chkUpdateMatters;
        private CheckBox chkExportContacts;
        private CheckBox chkUpdateContacts;
        private TextBox txtEmailFrom;
        private Label label4;
        private CheckBox chkLogToDatabase;
        private CheckBox chkExportLookups;
        private CheckBox chkIGO;
        private GroupBox grpIGO;
        private Label label12;
        private Label label13;
        private TextBox txtIGOPassword;
        private TextBox txtIGOUsername;
        private CheckBox chkIGOIntegrated;
        private Button btnIGOTest;
        private Label label14;
        private TextBox txtIGOServer;
        private Label label15;
        private TextBox txtIGODatabase;
        private Label label19;
        private Label label18;
        private Label label17;
        private Label label16;
        private TextBox txtIGOBranchNo;
        private TextBox txtIGOCompanyNo;
        private Label label21;
        private Label label20;
        private TextBox txtIGOMessage;
        private TextBox txtIGOVersion;
        private TextBox txtIGOProgramName;
        private TextBox txtIGOUserCode;
        private TextBox txtIGOComputer;
        private Label label22;
        private Panel panel2;
        private Panel panel3;
        private CheckBox chkCMSConvertToLocal;
        private Label lblIntegrationApp;
        private ComboBox cboIntegrationApp;
        private TextBox txtExceptionThreshold;
        private Label lblExceptionThreshold;
        private CheckBox chkCustomUpdateUpMatters;
        private CheckBox chkCustomUpdateExMatters;
        private CheckBox chkCustomUpdateUpClients;
        private CheckBox chkCustomUpdateExClients;
        private CheckBox chkCustomUpdateUpContacts;
        private CheckBox chkCustomUpdateExContacts;
        private CheckBox chkCustomUpdateFinancials;
        private CheckBox chkCustomUpdateTime;
        private TextBox txtUpdateUpMatters;
        private TextBox txtUpdateExMatters;
        private TextBox txtUpdateUpClients;
        private TextBox txtUpdateExClients;
        private TextBox txtUpdateUpContacts;
        private TextBox txtUpdateExContacts;
        private TextBox txtUpdateFinancials;
        private TextBox txtUpdateTime;
        private Label lblLogDays;
        private TextBox txtLogDays;
        private CheckBox chkMIL;
        private GroupBox grpMIL;
        private Label lblMilesPassword;
        private Label lblMilesUserName;
        private Label lblMilesServer;
        private TextBox txtMILPassword;
        private TextBox txtMILUsername;
        private TextBox txtMILServer;
        private Label lblTrackingColumn;
        private ComboBox cboTrackingColumn;
        private Label lblMilesOrcaleConnectionType;
        private ComboBox cboOrcaleConnectionType;
        private Label label26;
        private CheckBox chkE3E;
        private GroupBox grpE3E;
        private TextBox txtE3EURL;
        private Label lblE3EBaseUrl;
        private Label lblE3EFailedLocation;
        private Label lblE3ESuccessLocation;
        private TextBox txtE3EFailedLocation;
        private TextBox txtE3ESuccessLocation;
        private Label lblE3EDebug;
        private CheckBox chkE3EDebug;
        private CheckBox chkENT;
        private GroupBox grpENT;
        private TextBox txtEnterpriseBaseURL;
        private TextBox txtEnterpriseServiceUserID;
        private Label label27;
        private Label lblEnterpriseBaseUrl;
        private TextBox txtEnterpriseDateFormat;
        private Label lblEnterpriseDateFormat;
        private Label lblE3ECancelProcess;
        private CheckBox chkE3ECancelProcess;
        private Label lblE3EMatterConflictCheck;
        private CheckBox chkE3EMatterConflictCheck;
        private Label lblE3EResetFlagInCaseOfFailure;
        private CheckBox chkE3EResetFlagInCaseOfFailure;
        private Label lblE3ENewEndpoint;
        private CheckBox chkE3ENewEndpoint;
        private CheckBox chkC3E;
        private GroupBox grpC3E;
        private CheckBox chkC3EResetFlagInCaseOfFailure;
        private Label lblC3EResetFlagInCaseOfFailure;
        private CheckBox chkC3EMatterConflictCheck;
        private Label lblC3EMatterConflictCheck;
        private TextBox txtC3EFailedLocation;
        private Label lblC3EFailedLocation;
        private TextBox txtC3ESuccessLocation;
        private Label lblC3ESuccessLocation;
        private CheckBox chkC3EDebug;
        private Label lblC3EDebug;
        private TextBox txtC3EURL;
        private Label lblC3EBaseUrl;
        private TextBox txtC3EAzureADClientSecret;
        private Label lblC3EAzureADClientSecret;
        private TextBox txtC3EAzureADClientId;
        private Label lblC3EAzureADClientId;
        private TextBox txtC3EAzureADAudience;
        private Label lblC3EAzureADAudience;
        private CheckBox chkC3EAzureADAuth;
        private Label lblC3EAuthorizationMode;
        private CheckBox chkC3EImportInvoices;
        private Label lblC3EImportInvoices;
        private TextBox txtC3EAzureADTenantId;
        private Label lblC3EAzureADTenantId;
        private TextBox txtC3EAzureADInstanceId;
        private Label lblC3EAzureADInstanceId;
        private System.Windows.Forms.TextBox txtUserName;
		
		#endregion

		#region Constructors and Destructors
		
		/// <summary>
		/// Constructor for the form
		/// </summary>
		public frmSettings()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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

		
		#endregion
		
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.btnUpdate = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.txtPauseMins = new System.Windows.Forms.TextBox();
            this.chkEmailAdmin = new System.Windows.Forms.CheckBox();
            this.txtEmailAddress = new System.Windows.Forms.TextBox();
            this.txtSmtpServer = new System.Windows.Forms.TextBox();
            this.chkFullEventLog = new System.Windows.Forms.CheckBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.txtCMSURL = new System.Windows.Forms.TextBox();
            this.txtEmailFrom = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.chkE3EResetFlagInCaseOfFailure = new System.Windows.Forms.CheckBox();
            this.chkC3EResetFlagInCaseOfFailure = new System.Windows.Forms.CheckBox();
            this.lblSmtpEncryption = new System.Windows.Forms.Label();
            this.cmbSmtpEncryption = new System.Windows.Forms.ComboBox();
            this.chkSmtpAuthenticate = new System.Windows.Forms.CheckBox();
            this.lblSmtpLogin = new System.Windows.Forms.Label();
            this.txtSmtpLogin = new System.Windows.Forms.TextBox();
            this.txtSmtpPassword = new System.Windows.Forms.TextBox();
            this.lblSmtpPassword = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnClose = new System.Windows.Forms.Button();
            this.grp4 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grp3 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.grp2 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.chkIntegrated = new System.Windows.Forms.CheckBox();
            this.chkAzure = new System.Windows.Forms.CheckBox();
            this.btnTestSQL = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtDatabaseName = new System.Windows.Forms.TextBox();
            this.grp1 = new System.Windows.Forms.GroupBox();
            this.chkLogToDatabase = new System.Windows.Forms.CheckBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.grpCMS = new System.Windows.Forms.GroupBox();
            this.chkCMSConvertToLocal = new System.Windows.Forms.CheckBox();
            this.chkCMSIntegrated = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtCMSPassword = new System.Windows.Forms.TextBox();
            this.txtCMSUsername = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.grpMIL = new System.Windows.Forms.GroupBox();
            this.lblMilesPassword = new System.Windows.Forms.Label();
            this.lblMilesUserName = new System.Windows.Forms.Label();
            this.lblMilesServer = new System.Windows.Forms.Label();
            this.txtMILPassword = new System.Windows.Forms.TextBox();
            this.txtMILUsername = new System.Windows.Forms.TextBox();
            this.txtMILServer = new System.Windows.Forms.TextBox();
            this.lblMilesOrcaleConnectionType = new System.Windows.Forms.Label();
            this.cboOrcaleConnectionType = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkCommon = new System.Windows.Forms.CheckBox();
            this.chkExports = new System.Windows.Forms.CheckBox();
            this.chkCMS = new System.Windows.Forms.CheckBox();
            this.chkIGO = new System.Windows.Forms.CheckBox();
            this.chkMIL = new System.Windows.Forms.CheckBox();
            this.chkENT = new System.Windows.Forms.CheckBox();
            this.chkE3E = new System.Windows.Forms.CheckBox();
            this.chkC3E = new System.Windows.Forms.CheckBox();
            this.grpExports = new System.Windows.Forms.GroupBox();
            this.lblTrackingColumn = new System.Windows.Forms.Label();
            this.cboTrackingColumn = new System.Windows.Forms.ComboBox();
            this.lblLogDays = new System.Windows.Forms.Label();
            this.txtLogDays = new System.Windows.Forms.TextBox();
            this.txtUpdateUpMatters = new System.Windows.Forms.TextBox();
            this.txtUpdateExMatters = new System.Windows.Forms.TextBox();
            this.txtUpdateUpClients = new System.Windows.Forms.TextBox();
            this.txtUpdateExClients = new System.Windows.Forms.TextBox();
            this.txtUpdateUpContacts = new System.Windows.Forms.TextBox();
            this.txtUpdateExContacts = new System.Windows.Forms.TextBox();
            this.txtUpdateFinancials = new System.Windows.Forms.TextBox();
            this.txtUpdateTime = new System.Windows.Forms.TextBox();
            this.chkCustomUpdateUpMatters = new System.Windows.Forms.CheckBox();
            this.chkCustomUpdateExMatters = new System.Windows.Forms.CheckBox();
            this.chkCustomUpdateUpClients = new System.Windows.Forms.CheckBox();
            this.chkCustomUpdateExClients = new System.Windows.Forms.CheckBox();
            this.chkCustomUpdateUpContacts = new System.Windows.Forms.CheckBox();
            this.chkCustomUpdateExContacts = new System.Windows.Forms.CheckBox();
            this.chkCustomUpdateFinancials = new System.Windows.Forms.CheckBox();
            this.chkCustomUpdateTime = new System.Windows.Forms.CheckBox();
            this.txtExceptionThreshold = new System.Windows.Forms.TextBox();
            this.lblExceptionThreshold = new System.Windows.Forms.Label();
            this.lblIntegrationApp = new System.Windows.Forms.Label();
            this.cboIntegrationApp = new System.Windows.Forms.ComboBox();
            this.chkExportLookups = new System.Windows.Forms.CheckBox();
            this.chkExportFinancials = new System.Windows.Forms.CheckBox();
            this.chkExportTime = new System.Windows.Forms.CheckBox();
            this.chkUpdateMatters = new System.Windows.Forms.CheckBox();
            this.chkExportMatters = new System.Windows.Forms.CheckBox();
            this.chkUpdateClients = new System.Windows.Forms.CheckBox();
            this.chkExportClients = new System.Windows.Forms.CheckBox();
            this.chkUpdateContacts = new System.Windows.Forms.CheckBox();
            this.chkExportContacts = new System.Windows.Forms.CheckBox();
            this.chkExportUsers = new System.Windows.Forms.CheckBox();
            this.grpIGO = new System.Windows.Forms.GroupBox();
            this.txtIGOMessage = new System.Windows.Forms.TextBox();
            this.txtIGOVersion = new System.Windows.Forms.TextBox();
            this.txtIGOProgramName = new System.Windows.Forms.TextBox();
            this.txtIGOUserCode = new System.Windows.Forms.TextBox();
            this.txtIGOComputer = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.txtIGOBranchNo = new System.Windows.Forms.TextBox();
            this.txtIGOCompanyNo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtIGOPassword = new System.Windows.Forms.TextBox();
            this.txtIGOUsername = new System.Windows.Forms.TextBox();
            this.chkIGOIntegrated = new System.Windows.Forms.CheckBox();
            this.btnIGOTest = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.txtIGOServer = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtIGODatabase = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.grpE3E = new System.Windows.Forms.GroupBox();
            this.lblE3EResetFlagInCaseOfFailure = new System.Windows.Forms.Label();
            this.chkE3EMatterConflictCheck = new System.Windows.Forms.CheckBox();
            this.lblE3EMatterConflictCheck = new System.Windows.Forms.Label();
            this.chkE3ECancelProcess = new System.Windows.Forms.CheckBox();
            this.lblE3ECancelProcess = new System.Windows.Forms.Label();
            this.txtE3EFailedLocation = new System.Windows.Forms.TextBox();
            this.lblE3EFailedLocation = new System.Windows.Forms.Label();
            this.txtE3ESuccessLocation = new System.Windows.Forms.TextBox();
            this.lblE3ESuccessLocation = new System.Windows.Forms.Label();
            this.chkE3EDebug = new System.Windows.Forms.CheckBox();
            this.lblE3EDebug = new System.Windows.Forms.Label();
            this.chkE3ENewEndpoint = new System.Windows.Forms.CheckBox();
            this.lblE3ENewEndpoint = new System.Windows.Forms.Label();
            this.txtE3EURL = new System.Windows.Forms.TextBox();
            this.lblE3EBaseUrl = new System.Windows.Forms.Label();
            this.grpENT = new System.Windows.Forms.GroupBox();
            this.txtEnterpriseDateFormat = new System.Windows.Forms.TextBox();
            this.lblEnterpriseDateFormat = new System.Windows.Forms.Label();
            this.txtEnterpriseServiceUserID = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.lblEnterpriseBaseUrl = new System.Windows.Forms.Label();
            this.txtEnterpriseBaseURL = new System.Windows.Forms.TextBox();
            this.grpC3E = new System.Windows.Forms.GroupBox();
            this.txtC3EAzureADTenantId = new System.Windows.Forms.TextBox();
            this.lblC3EAzureADTenantId = new System.Windows.Forms.Label();
            this.txtC3EAzureADClientSecret = new System.Windows.Forms.TextBox();
            this.lblC3EAzureADClientSecret = new System.Windows.Forms.Label();
            this.txtC3EAzureADClientId = new System.Windows.Forms.TextBox();
            this.lblC3EAzureADClientId = new System.Windows.Forms.Label();
            this.txtC3EAzureADAudience = new System.Windows.Forms.TextBox();
            this.lblC3EAzureADAudience = new System.Windows.Forms.Label();
            this.chkC3EAzureADAuth = new System.Windows.Forms.CheckBox();
            this.lblC3EAuthorizationMode = new System.Windows.Forms.Label();
            this.chkC3EImportInvoices = new System.Windows.Forms.CheckBox();
            this.lblC3EImportInvoices = new System.Windows.Forms.Label();
            this.lblC3EResetFlagInCaseOfFailure = new System.Windows.Forms.Label();
            this.chkC3EMatterConflictCheck = new System.Windows.Forms.CheckBox();
            this.lblC3EMatterConflictCheck = new System.Windows.Forms.Label();
            this.txtC3EFailedLocation = new System.Windows.Forms.TextBox();
            this.lblC3EFailedLocation = new System.Windows.Forms.Label();
            this.txtC3ESuccessLocation = new System.Windows.Forms.TextBox();
            this.lblC3ESuccessLocation = new System.Windows.Forms.Label();
            this.chkC3EDebug = new System.Windows.Forms.CheckBox();
            this.lblC3EDebug = new System.Windows.Forms.Label();
            this.txtC3EURL = new System.Windows.Forms.TextBox();
            this.lblC3EBaseUrl = new System.Windows.Forms.Label();
            this.txtC3EAzureADInstanceId = new System.Windows.Forms.TextBox();
            this.lblC3EAzureADInstanceId = new System.Windows.Forms.Label();
            this.grp4.SuspendLayout();
            this.grp3.SuspendLayout();
            this.grp2.SuspendLayout();
            this.grp1.SuspendLayout();
            this.grpCMS.SuspendLayout();
            this.grpMIL.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpExports.SuspendLayout();
            this.grpIGO.SuspendLayout();
            this.grpE3E.SuspendLayout();
            this.grpENT.SuspendLayout();
            this.grpC3E.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(373, 430);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 26);
            this.btnUpdate.TabIndex = 13;
            this.btnUpdate.Text = "&OK";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtPauseMins
            // 
            this.txtPauseMins.Location = new System.Drawing.Point(411, 15);
            this.txtPauseMins.Name = "txtPauseMins";
            this.txtPauseMins.Size = new System.Drawing.Size(29, 20);
            this.txtPauseMins.TabIndex = 10;
            this.toolTip1.SetToolTip(this.txtPauseMins, "On each full iteration the application will pause for this amount of seconds");
            // 
            // chkEmailAdmin
            // 
            this.chkEmailAdmin.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkEmailAdmin.Location = new System.Drawing.Point(13, 18);
            this.chkEmailAdmin.Name = "chkEmailAdmin";
            this.chkEmailAdmin.Size = new System.Drawing.Size(120, 16);
            this.chkEmailAdmin.TabIndex = 7;
            this.chkEmailAdmin.Text = "Email Admin On Error";
            this.toolTip1.SetToolTip(this.chkEmailAdmin, "Check this to enable more detail in the log window");
            this.chkEmailAdmin.CheckedChanged += new System.EventHandler(this.chkEmailAdmin_CheckedChanged);
            // 
            // txtEmailAddress
            // 
            this.txtEmailAddress.Location = new System.Drawing.Point(150, 16);
            this.txtEmailAddress.Name = "txtEmailAddress";
            this.txtEmailAddress.Size = new System.Drawing.Size(128, 20);
            this.txtEmailAddress.TabIndex = 8;
            this.toolTip1.SetToolTip(this.txtEmailAddress, "When the system detects an error it will pause for this amount of minutes multipl" +
        "ying by the number of consecutive errors.");
            // 
            // txtSmtpServer
            // 
            this.txtSmtpServer.Location = new System.Drawing.Point(150, 40);
            this.txtSmtpServer.Name = "txtSmtpServer";
            this.txtSmtpServer.Size = new System.Drawing.Size(297, 20);
            this.txtSmtpServer.TabIndex = 9;
            this.toolTip1.SetToolTip(this.txtSmtpServer, "When the system detects an error it will pause for this amount of minutes multipl" +
        "ying by the number of consecutive errors.");
            // 
            // chkFullEventLog
            // 
            this.chkFullEventLog.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkFullEventLog.Location = new System.Drawing.Point(10, 17);
            this.chkFullEventLog.Name = "chkFullEventLog";
            this.chkFullEventLog.Size = new System.Drawing.Size(172, 24);
            this.chkFullEventLog.TabIndex = 3;
            this.chkFullEventLog.Text = "Full Event Logging Enabled";
            this.toolTip1.SetToolTip(this.chkFullEventLog, "Check this to enable more detail in the log window");
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(152, 45);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(272, 20);
            this.txtFilePath.TabIndex = 4;
            this.txtFilePath.TabStop = false;
            this.toolTip1.SetToolTip(this.txtFilePath, "File name of the perfomance log.");
            // 
            // txtCMSURL
            // 
            this.txtCMSURL.Location = new System.Drawing.Point(110, 71);
            this.txtCMSURL.Name = "txtCMSURL";
            this.txtCMSURL.Size = new System.Drawing.Size(336, 20);
            this.txtCMSURL.TabIndex = 4;
            this.txtCMSURL.TabStop = false;
            this.txtCMSURL.Text = "http:\\\\";
            this.toolTip1.SetToolTip(this.txtCMSURL, "File name of the perfomance log.");
            // 
            // txtEmailFrom
            // 
            this.txtEmailFrom.Location = new System.Drawing.Point(318, 16);
            this.txtEmailFrom.Name = "txtEmailFrom";
            this.txtEmailFrom.Size = new System.Drawing.Size(128, 20);
            this.txtEmailFrom.TabIndex = 18;
            this.toolTip1.SetToolTip(this.txtEmailFrom, "When the system detects an error it will pause for this amount of minutes multipl" +
        "ying by the number of consecutive errors.");
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.ForeColor = System.Drawing.Color.Red;
            this.label26.Location = new System.Drawing.Point(9, 154);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(155, 13);
            this.label26.TabIndex = 54;
            this.label26.Text = "Indigo V2 Procedures Required";
            this.toolTip1.SetToolTip(this.label26, "This version of OMSExport with Indigo requires the Indigo V2 Procedures and the M" +
        "atter Centre Procedures designed with UserID and FeeEarnerID link");
            // 
            // chkE3EResetFlagInCaseOfFailure
            // 
            this.chkE3EResetFlagInCaseOfFailure.AutoSize = true;
            this.chkE3EResetFlagInCaseOfFailure.Location = new System.Drawing.Point(163, 217);
            this.chkE3EResetFlagInCaseOfFailure.Name = "chkE3EResetFlagInCaseOfFailure";
            this.chkE3EResetFlagInCaseOfFailure.Size = new System.Drawing.Size(15, 14);
            this.chkE3EResetFlagInCaseOfFailure.TabIndex = 46;
            this.toolTip1.SetToolTip(this.chkE3EResetFlagInCaseOfFailure, "Reset need export flag in case of failure if checked.");
            this.chkE3EResetFlagInCaseOfFailure.UseVisualStyleBackColor = true;
            // 
            // chkC3EResetFlagInCaseOfFailure
            // 
            this.chkC3EResetFlagInCaseOfFailure.AutoSize = true;
            this.chkC3EResetFlagInCaseOfFailure.Location = new System.Drawing.Point(163, 156);
            this.chkC3EResetFlagInCaseOfFailure.Name = "chkC3EResetFlagInCaseOfFailure";
            this.chkC3EResetFlagInCaseOfFailure.Size = new System.Drawing.Size(15, 14);
            this.chkC3EResetFlagInCaseOfFailure.TabIndex = 44;
            this.toolTip1.SetToolTip(this.chkC3EResetFlagInCaseOfFailure, "Reset need export flag in case of failure if checked.");
            this.chkC3EResetFlagInCaseOfFailure.UseVisualStyleBackColor = true;
            // 
            // lblSmtpEncryption
            // 
            this.lblSmtpEncryption.AutoSize = true;
            this.lblSmtpEncryption.Location = new System.Drawing.Point(8, 68);
            this.lblSmtpEncryption.Name = "lblSmtpEncryption";
            this.lblSmtpEncryption.Size = new System.Drawing.Size(90, 13);
            this.lblSmtpEncryption.TabIndex = 19;
            this.lblSmtpEncryption.Text = "SMTP Encryption";
            // 
            // cmbSmtpEncryption
            // 
            this.cmbSmtpEncryption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSmtpEncryption.FormattingEnabled = true;
            this.cmbSmtpEncryption.Items.AddRange(new object[] {
            "NONE",
            "SSL",
            "STARTTLS"});
            this.cmbSmtpEncryption.Location = new System.Drawing.Point(150, 64);
            this.cmbSmtpEncryption.Name = "cmbSmtpEncryption";
            this.cmbSmtpEncryption.Size = new System.Drawing.Size(91, 21);
            this.cmbSmtpEncryption.TabIndex = 20;
            // 
            // chkSmtpAuthenticate
            // 
            this.chkSmtpAuthenticate.AutoSize = true;
            this.chkSmtpAuthenticate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.chkSmtpAuthenticate.Location = new System.Drawing.Point(260, 65);
            this.chkSmtpAuthenticate.Name = "chkSmtpAuthenticate";
            this.chkSmtpAuthenticate.Size = new System.Drawing.Size(92, 18);
            this.chkSmtpAuthenticate.TabIndex = 21;
            this.chkSmtpAuthenticate.Text = "Authenticate";
            this.chkSmtpAuthenticate.UseVisualStyleBackColor = true;
            // 
            // lblSmtpLogin
            // 
            this.lblSmtpLogin.AutoSize = true;
            this.lblSmtpLogin.Location = new System.Drawing.Point(8, 93);
            this.lblSmtpLogin.Name = "lblSmtpLogin";
            this.lblSmtpLogin.Size = new System.Drawing.Size(66, 13);
            this.lblSmtpLogin.TabIndex = 22;
            this.lblSmtpLogin.Text = "SMTP Login";
            // 
            // txtSmtpLogin
            // 
            this.txtSmtpLogin.Location = new System.Drawing.Point(150, 90);
            this.txtSmtpLogin.Name = "txtSmtpLogin";
            this.txtSmtpLogin.Size = new System.Drawing.Size(297, 20);
            this.txtSmtpLogin.TabIndex = 23;
            // 
            // txtSmtpPassword
            // 
            this.txtSmtpPassword.Location = new System.Drawing.Point(150, 114);
            this.txtSmtpPassword.Name = "txtSmtpPassword";
            this.txtSmtpPassword.PasswordChar = '*';
            this.txtSmtpPassword.Size = new System.Drawing.Size(297, 20);
            this.txtSmtpPassword.TabIndex = 25;
            // 
            // lblSmtpPassword
            // 
            this.lblSmtpPassword.AutoSize = true;
            this.lblSmtpPassword.Location = new System.Drawing.Point(8, 117);
            this.lblSmtpPassword.Name = "lblSmtpPassword";
            this.lblSmtpPassword.Size = new System.Drawing.Size(86, 13);
            this.lblSmtpPassword.TabIndex = 24;
            this.lblSmtpPassword.Text = "SMTP Password";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(461, 430);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 26);
            this.btnClose.TabIndex = 28;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "&Cancel";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grp4
            // 
            this.grp4.Controls.Add(this.txtPauseMins);
            this.grp4.Controls.Add(this.label2);
            this.grp4.Location = new System.Drawing.Point(1000, 372);
            this.grp4.Name = "grp4";
            this.grp4.Size = new System.Drawing.Size(456, 44);
            this.grp4.TabIndex = 26;
            this.grp4.TabStop = false;
            this.grp4.Text = "Service Settings";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(8, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(232, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Minutes to pause between iterations";
            // 
            // grp3
            // 
            this.grp3.Controls.Add(this.txtEmailFrom);
            this.grp3.Controls.Add(this.label4);
            this.grp3.Controls.Add(this.btnTest);
            this.grp3.Controls.Add(this.chkEmailAdmin);
            this.grp3.Controls.Add(this.txtEmailAddress);
            this.grp3.Controls.Add(this.label5);
            this.grp3.Controls.Add(this.txtSmtpServer);
            this.grp3.Controls.Add(this.txtSmtpPassword);
            this.grp3.Controls.Add(this.lblSmtpPassword);
            this.grp3.Controls.Add(this.txtSmtpLogin);
            this.grp3.Controls.Add(this.lblSmtpLogin);
            this.grp3.Controls.Add(this.chkSmtpAuthenticate);
            this.grp3.Controls.Add(this.cmbSmtpEncryption);
            this.grp3.Controls.Add(this.lblSmtpEncryption);
            this.grp3.Location = new System.Drawing.Point(1000, 227);
            this.grp3.Name = "grp3";
            this.grp3.Size = new System.Drawing.Size(456, 143);
            this.grp3.TabIndex = 25;
            this.grp3.TabStop = false;
            this.grp3.Text = "Admin Settings";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(284, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "From:";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(372, 63);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 16;
            this.btnTest.Text = "Test";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "SMTP Server Name";
            // 
            // grp2
            // 
            this.grp2.Controls.Add(this.label6);
            this.grp2.Controls.Add(this.label3);
            this.grp2.Controls.Add(this.txtPassword);
            this.grp2.Controls.Add(this.txtUserName);
            this.grp2.Controls.Add(this.chkIntegrated);
            this.grp2.Controls.Add(this.chkAzure);
            this.grp2.Controls.Add(this.btnTestSQL);
            this.grp2.Controls.Add(this.label8);
            this.grp2.Controls.Add(this.txtServerName);
            this.grp2.Controls.Add(this.label7);
            this.grp2.Controls.Add(this.txtDatabaseName);
            this.grp2.Location = new System.Drawing.Point(1000, 82);
            this.grp2.Name = "grp2";
            this.grp2.Size = new System.Drawing.Size(456, 142);
            this.grp2.TabIndex = 24;
            this.grp2.TabStop = false;
            this.grp2.Text = "OMS Database Settings";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(7, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 18);
            this.label6.TabIndex = 27;
            this.label6.Text = "Password";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(7, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(135, 18);
            this.label3.TabIndex = 26;
            this.label3.Text = "User Name";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(150, 113);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(218, 20);
            this.txtPassword.TabIndex = 25;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(150, 89);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(218, 20);
            this.txtUserName.TabIndex = 24;
            // 
            // chkIntegrated
            // 
            this.chkIntegrated.Checked = true;
            this.chkIntegrated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIntegrated.Location = new System.Drawing.Point(11, 67);
            this.chkIntegrated.Name = "chkIntegrated";
            this.chkIntegrated.Size = new System.Drawing.Size(128, 18);
            this.chkIntegrated.TabIndex = 23;
            this.chkIntegrated.Text = "Integrated Security";
            this.chkIntegrated.CheckedChanged += new System.EventHandler(this.chkIntegrated_CheckedChanged);
            // 
            // chkAzure
            // 
            this.chkAzure.Location = new System.Drawing.Point(150, 67);
            this.chkAzure.Name = "chkAzure";
            this.chkAzure.Size = new System.Drawing.Size(104, 18);
            this.chkAzure.TabIndex = 28;
            this.chkAzure.Text = "Azure";
            // 
            // btnTestSQL
            // 
            this.btnTestSQL.Location = new System.Drawing.Point(372, 42);
            this.btnTestSQL.Name = "btnTestSQL";
            this.btnTestSQL.Size = new System.Drawing.Size(75, 23);
            this.btnTestSQL.TabIndex = 22;
            this.btnTestSQL.Text = "Test";
            this.btnTestSQL.Click += new System.EventHandler(this.btnTestSQL_Click);
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(136, 18);
            this.label8.TabIndex = 21;
            this.label8.Text = "SQL Server Name";
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(150, 17);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(297, 20);
            this.txtServerName.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(8, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(135, 18);
            this.label7.TabIndex = 20;
            this.label7.Text = "Database Name";
            // 
            // txtDatabaseName
            // 
            this.txtDatabaseName.Location = new System.Drawing.Point(150, 43);
            this.txtDatabaseName.Name = "txtDatabaseName";
            this.txtDatabaseName.Size = new System.Drawing.Size(218, 20);
            this.txtDatabaseName.TabIndex = 6;
            // 
            // grp1
            // 
            this.grp1.Controls.Add(this.chkLogToDatabase);
            this.grp1.Controls.Add(this.chkFullEventLog);
            this.grp1.Controls.Add(this.txtFilePath);
            this.grp1.Controls.Add(this.btnBrowse);
            this.grp1.Controls.Add(this.label1);
            this.grp1.Location = new System.Drawing.Point(1000, 8);
            this.grp1.Name = "grp1";
            this.grp1.Size = new System.Drawing.Size(456, 72);
            this.grp1.TabIndex = 23;
            this.grp1.TabStop = false;
            this.grp1.Text = "Log Settings";
            // 
            // chkLogToDatabase
            // 
            this.chkLogToDatabase.AutoSize = true;
            this.chkLogToDatabase.Location = new System.Drawing.Point(188, 21);
            this.chkLogToDatabase.Name = "chkLogToDatabase";
            this.chkLogToDatabase.Size = new System.Drawing.Size(136, 17);
            this.chkLogToDatabase.TabIndex = 7;
            this.chkLogToDatabase.Text = "Log To OMS Database";
            this.chkLogToDatabase.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(424, 45);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(24, 20);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "...";
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 18);
            this.label1.TabIndex = 6;
            this.label1.Text = "Statistics Log Folder";
            // 
            // grpCMS
            // 
            this.grpCMS.Controls.Add(this.chkCMSConvertToLocal);
            this.grpCMS.Controls.Add(this.chkCMSIntegrated);
            this.grpCMS.Controls.Add(this.label10);
            this.grpCMS.Controls.Add(this.label11);
            this.grpCMS.Controls.Add(this.txtCMSPassword);
            this.grpCMS.Controls.Add(this.txtCMSUsername);
            this.grpCMS.Controls.Add(this.txtCMSURL);
            this.grpCMS.Controls.Add(this.label9);
            this.grpCMS.Location = new System.Drawing.Point(1000, 8);
            this.grpCMS.Name = "grpCMS";
            this.grpCMS.Size = new System.Drawing.Size(456, 130);
            this.grpCMS.TabIndex = 31;
            this.grpCMS.TabStop = false;
            this.grpCMS.Text = "CMS Application Settings";
            // 
            // chkCMSConvertToLocal
            // 
            this.chkCMSConvertToLocal.AutoSize = true;
            this.chkCMSConvertToLocal.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkCMSConvertToLocal.Location = new System.Drawing.Point(6, 101);
            this.chkCMSConvertToLocal.Name = "chkCMSConvertToLocal";
            this.chkCMSConvertToLocal.Size = new System.Drawing.Size(134, 17);
            this.chkCMSConvertToLocal.TabIndex = 33;
            this.chkCMSConvertToLocal.Text = "Convert To Local Time";
            this.chkCMSConvertToLocal.UseVisualStyleBackColor = true;
            // 
            // chkCMSIntegrated
            // 
            this.chkCMSIntegrated.Location = new System.Drawing.Point(300, 20);
            this.chkCMSIntegrated.Name = "chkCMSIntegrated";
            this.chkCMSIntegrated.Size = new System.Drawing.Size(148, 20);
            this.chkCMSIntegrated.TabIndex = 32;
            this.chkCMSIntegrated.Text = "Integrated";
            this.chkCMSIntegrated.CheckedChanged += new System.EventHandler(this.chkCMSIntegrated_CheckedChanged);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(9, 44);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(135, 18);
            this.label10.TabIndex = 31;
            this.label10.Text = "Password";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(9, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(135, 18);
            this.label11.TabIndex = 30;
            this.label11.Text = "User Name";
            // 
            // txtCMSPassword
            // 
            this.txtCMSPassword.Location = new System.Drawing.Point(150, 43);
            this.txtCMSPassword.Name = "txtCMSPassword";
            this.txtCMSPassword.PasswordChar = '*';
            this.txtCMSPassword.Size = new System.Drawing.Size(142, 20);
            this.txtCMSPassword.TabIndex = 29;
            // 
            // txtCMSUsername
            // 
            this.txtCMSUsername.Location = new System.Drawing.Point(150, 19);
            this.txtCMSUsername.Name = "txtCMSUsername";
            this.txtCMSUsername.Size = new System.Drawing.Size(142, 20);
            this.txtCMSUsername.TabIndex = 28;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(9, 72);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(103, 18);
            this.label9.TabIndex = 6;
            this.label9.Text = "Base URL";
            // 
            // grpMIL
            // 
            this.grpMIL.Controls.Add(this.lblMilesPassword);
            this.grpMIL.Controls.Add(this.lblMilesUserName);
            this.grpMIL.Controls.Add(this.lblMilesServer);
            this.grpMIL.Controls.Add(this.txtMILPassword);
            this.grpMIL.Controls.Add(this.txtMILUsername);
            this.grpMIL.Controls.Add(this.txtMILServer);
            this.grpMIL.Controls.Add(this.lblMilesOrcaleConnectionType);
            this.grpMIL.Controls.Add(this.cboOrcaleConnectionType);
            this.grpMIL.Location = new System.Drawing.Point(1000, 8);
            this.grpMIL.Name = "grpMIL";
            this.grpMIL.Size = new System.Drawing.Size(456, 364);
            this.grpMIL.TabIndex = 70;
            this.grpMIL.TabStop = false;
            this.grpMIL.Text = "Miles 33 Settings";
            // 
            // lblMilesPassword
            // 
            this.lblMilesPassword.Location = new System.Drawing.Point(9, 80);
            this.lblMilesPassword.Name = "lblMilesPassword";
            this.lblMilesPassword.Size = new System.Drawing.Size(135, 18);
            this.lblMilesPassword.TabIndex = 37;
            this.lblMilesPassword.Text = "Password:";
            this.lblMilesPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMilesUserName
            // 
            this.lblMilesUserName.Location = new System.Drawing.Point(9, 40);
            this.lblMilesUserName.Name = "lblMilesUserName";
            this.lblMilesUserName.Size = new System.Drawing.Size(135, 18);
            this.lblMilesUserName.TabIndex = 36;
            this.lblMilesUserName.Text = "User Name:";
            this.lblMilesUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMilesServer
            // 
            this.lblMilesServer.Location = new System.Drawing.Point(9, 120);
            this.lblMilesServer.Name = "lblMilesServer";
            this.lblMilesServer.Size = new System.Drawing.Size(135, 18);
            this.lblMilesServer.TabIndex = 31;
            this.lblMilesServer.Text = "Server Name:";
            this.lblMilesServer.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtMILPassword
            // 
            this.txtMILPassword.Location = new System.Drawing.Point(149, 80);
            this.txtMILPassword.Name = "txtMILPassword";
            this.txtMILPassword.PasswordChar = '*';
            this.txtMILPassword.Size = new System.Drawing.Size(218, 20);
            this.txtMILPassword.TabIndex = 35;
            // 
            // txtMILUsername
            // 
            this.txtMILUsername.Location = new System.Drawing.Point(149, 40);
            this.txtMILUsername.Name = "txtMILUsername";
            this.txtMILUsername.Size = new System.Drawing.Size(218, 20);
            this.txtMILUsername.TabIndex = 34;
            // 
            // txtMILServer
            // 
            this.txtMILServer.Location = new System.Drawing.Point(149, 120);
            this.txtMILServer.Name = "txtMILServer";
            this.txtMILServer.Size = new System.Drawing.Size(218, 20);
            this.txtMILServer.TabIndex = 36;
            // 
            // lblMilesOrcaleConnectionType
            // 
            this.lblMilesOrcaleConnectionType.Location = new System.Drawing.Point(9, 160);
            this.lblMilesOrcaleConnectionType.Name = "lblMilesOrcaleConnectionType";
            this.lblMilesOrcaleConnectionType.Size = new System.Drawing.Size(135, 18);
            this.lblMilesOrcaleConnectionType.TabIndex = 32;
            this.lblMilesOrcaleConnectionType.Text = "Orcale Connection Type:";
            this.lblMilesOrcaleConnectionType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboOrcaleConnectionType
            // 
            this.cboOrcaleConnectionType.FormattingEnabled = true;
            this.cboOrcaleConnectionType.Items.AddRange(new object[] {
            "Oracle 7 OLEDB",
            "Oracle 10 OLEDB"});
            this.cboOrcaleConnectionType.Location = new System.Drawing.Point(149, 160);
            this.cboOrcaleConnectionType.Name = "cboOrcaleConnectionType";
            this.cboOrcaleConnectionType.Size = new System.Drawing.Size(180, 21);
            this.cboOrcaleConnectionType.TabIndex = 37;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.chkCommon);
            this.panel1.Controls.Add(this.chkExports);
            this.panel1.Controls.Add(this.chkCMS);
            this.panel1.Controls.Add(this.chkIGO);
            this.panel1.Controls.Add(this.chkMIL);
            this.panel1.Controls.Add(this.chkENT);
            this.panel1.Controls.Add(this.chkE3E);
            this.panel1.Controls.Add(this.chkC3E);
            this.panel1.Location = new System.Drawing.Point(8, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(72, 402);
            this.panel1.TabIndex = 32;
            // 
            // chkCommon
            // 
            this.chkCommon.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkCommon.BackColor = System.Drawing.SystemColors.Control;
            this.chkCommon.Checked = true;
            this.chkCommon.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCommon.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCommon.Image = ((System.Drawing.Image)(resources.GetObject("chkCommon.Image")));
            this.chkCommon.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkCommon.Location = new System.Drawing.Point(0, 1);
            this.chkCommon.Name = "chkCommon";
            this.chkCommon.Size = new System.Drawing.Size(68, 62);
            this.chkCommon.TabIndex = 30;
            this.chkCommon.Text = "Common";
            this.chkCommon.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkCommon.UseVisualStyleBackColor = false;
            this.chkCommon.CheckedChanged += new System.EventHandler(this.chkCommon_CheckedChanged);
            this.chkCommon.Click += new System.EventHandler(this.chk_Click);
            // 
            // chkExports
            // 
            this.chkExports.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkExports.BackColor = System.Drawing.SystemColors.Control;
            this.chkExports.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkExports.Image = ((System.Drawing.Image)(resources.GetObject("chkExports.Image")));
            this.chkExports.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkExports.Location = new System.Drawing.Point(0, 63);
            this.chkExports.Name = "chkExports";
            this.chkExports.Size = new System.Drawing.Size(68, 62);
            this.chkExports.TabIndex = 32;
            this.chkExports.Text = "Exports";
            this.chkExports.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkExports.UseVisualStyleBackColor = false;
            this.chkExports.Click += new System.EventHandler(this.chk_Click);
            // 
            // chkCMS
            // 
            this.chkCMS.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkCMS.BackColor = System.Drawing.SystemColors.Control;
            this.chkCMS.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkCMS.Image = ((System.Drawing.Image)(resources.GetObject("chkCMS.Image")));
            this.chkCMS.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkCMS.Location = new System.Drawing.Point(0, 125);
            this.chkCMS.Name = "chkCMS";
            this.chkCMS.Size = new System.Drawing.Size(68, 62);
            this.chkCMS.TabIndex = 31;
            this.chkCMS.Text = "CMS";
            this.chkCMS.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkCMS.UseVisualStyleBackColor = false;
            this.chkCMS.Click += new System.EventHandler(this.chk_Click);
            // 
            // chkIGO
            // 
            this.chkIGO.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkIGO.BackColor = System.Drawing.SystemColors.Control;
            this.chkIGO.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIGO.Image = ((System.Drawing.Image)(resources.GetObject("chkIGO.Image")));
            this.chkIGO.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkIGO.Location = new System.Drawing.Point(0, 187);
            this.chkIGO.Name = "chkIGO";
            this.chkIGO.Size = new System.Drawing.Size(68, 62);
            this.chkIGO.TabIndex = 33;
            this.chkIGO.Text = "Indigo";
            this.chkIGO.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkIGO.UseVisualStyleBackColor = false;
            this.chkIGO.Click += new System.EventHandler(this.chk_Click);
            // 
            // chkMIL
            // 
            this.chkMIL.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkMIL.BackColor = System.Drawing.SystemColors.Control;
            this.chkMIL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMIL.Image = ((System.Drawing.Image)(resources.GetObject("chkMIL.Image")));
            this.chkMIL.Location = new System.Drawing.Point(0, 249);
            this.chkMIL.Name = "chkMIL";
            this.chkMIL.Size = new System.Drawing.Size(68, 62);
            this.chkMIL.TabIndex = 34;
            this.chkMIL.Text = "Miles 33";
            this.chkMIL.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkMIL.UseVisualStyleBackColor = false;
            this.chkMIL.CheckedChanged += new System.EventHandler(this.chkMIL_CheckedChanged);
            this.chkMIL.Click += new System.EventHandler(this.chk_Click);
            // 
            // chkENT
            // 
            this.chkENT.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkENT.BackColor = System.Drawing.SystemColors.Control;
            this.chkENT.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkENT.Image = ((System.Drawing.Image)(resources.GetObject("chkENT.Image")));
            this.chkENT.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkENT.Location = new System.Drawing.Point(0, 311);
            this.chkENT.Name = "chkENT";
            this.chkENT.Size = new System.Drawing.Size(68, 62);
            this.chkENT.TabIndex = 36;
            this.chkENT.Text = "Enterprise";
            this.chkENT.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkENT.UseVisualStyleBackColor = false;
            this.chkENT.Click += new System.EventHandler(this.chk_Click);
            // 
            // chkE3E
            // 
            this.chkE3E.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkE3E.BackColor = System.Drawing.SystemColors.Control;
            this.chkE3E.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkE3E.Image = ((System.Drawing.Image)(resources.GetObject("chkE3E.Image")));
            this.chkE3E.Location = new System.Drawing.Point(0, 373);
            this.chkE3E.Name = "chkE3E";
            this.chkE3E.Size = new System.Drawing.Size(68, 62);
            this.chkE3E.TabIndex = 35;
            this.chkE3E.Text = "Elite 3E";
            this.chkE3E.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkE3E.UseVisualStyleBackColor = false;
            this.chkE3E.Click += new System.EventHandler(this.chk_Click);
            // 
            // chkC3E
            // 
            this.chkC3E.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkC3E.BackColor = System.Drawing.SystemColors.Control;
            this.chkC3E.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkC3E.Image = ((System.Drawing.Image)(resources.GetObject("chkC3E.Image")));
            this.chkC3E.Location = new System.Drawing.Point(0, 435);
            this.chkC3E.Name = "chkC3E";
            this.chkC3E.Size = new System.Drawing.Size(68, 62);
            this.chkC3E.TabIndex = 35;
            this.chkC3E.Text = "3E Cloud";
            this.chkC3E.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.chkC3E.UseVisualStyleBackColor = false;
            this.chkC3E.Click += new System.EventHandler(this.chk_Click);
            // 
            // grpExports
            // 
            this.grpExports.Controls.Add(this.lblTrackingColumn);
            this.grpExports.Controls.Add(this.cboTrackingColumn);
            this.grpExports.Controls.Add(this.lblLogDays);
            this.grpExports.Controls.Add(this.txtLogDays);
            this.grpExports.Controls.Add(this.txtUpdateUpMatters);
            this.grpExports.Controls.Add(this.txtUpdateExMatters);
            this.grpExports.Controls.Add(this.txtUpdateUpClients);
            this.grpExports.Controls.Add(this.txtUpdateExClients);
            this.grpExports.Controls.Add(this.txtUpdateUpContacts);
            this.grpExports.Controls.Add(this.txtUpdateExContacts);
            this.grpExports.Controls.Add(this.txtUpdateFinancials);
            this.grpExports.Controls.Add(this.txtUpdateTime);
            this.grpExports.Controls.Add(this.chkCustomUpdateUpMatters);
            this.grpExports.Controls.Add(this.chkCustomUpdateExMatters);
            this.grpExports.Controls.Add(this.chkCustomUpdateUpClients);
            this.grpExports.Controls.Add(this.chkCustomUpdateExClients);
            this.grpExports.Controls.Add(this.chkCustomUpdateUpContacts);
            this.grpExports.Controls.Add(this.chkCustomUpdateExContacts);
            this.grpExports.Controls.Add(this.chkCustomUpdateFinancials);
            this.grpExports.Controls.Add(this.chkCustomUpdateTime);
            this.grpExports.Controls.Add(this.txtExceptionThreshold);
            this.grpExports.Controls.Add(this.lblExceptionThreshold);
            this.grpExports.Controls.Add(this.lblIntegrationApp);
            this.grpExports.Controls.Add(this.cboIntegrationApp);
            this.grpExports.Controls.Add(this.chkExportLookups);
            this.grpExports.Controls.Add(this.chkExportFinancials);
            this.grpExports.Controls.Add(this.chkExportTime);
            this.grpExports.Controls.Add(this.chkUpdateMatters);
            this.grpExports.Controls.Add(this.chkExportMatters);
            this.grpExports.Controls.Add(this.chkUpdateClients);
            this.grpExports.Controls.Add(this.chkExportClients);
            this.grpExports.Controls.Add(this.chkUpdateContacts);
            this.grpExports.Controls.Add(this.chkExportContacts);
            this.grpExports.Controls.Add(this.chkExportUsers);
            this.grpExports.Location = new System.Drawing.Point(1000, 8);
            this.grpExports.Name = "grpExports";
            this.grpExports.Size = new System.Drawing.Size(456, 406);
            this.grpExports.TabIndex = 33;
            this.grpExports.TabStop = false;
            this.grpExports.Text = "Export Options";
            // 
            // lblTrackingColumn
            // 
            this.lblTrackingColumn.AutoSize = true;
            this.lblTrackingColumn.Location = new System.Drawing.Point(20, 362);
            this.lblTrackingColumn.Name = "lblTrackingColumn";
            this.lblTrackingColumn.Size = new System.Drawing.Size(114, 13);
            this.lblTrackingColumn.TabIndex = 33;
            this.lblTrackingColumn.Text = "Tracking Column Type";
            // 
            // cboTrackingColumn
            // 
            this.cboTrackingColumn.FormattingEnabled = true;
            this.cboTrackingColumn.Items.AddRange(new object[] {
            "Integer",
            "String"});
            this.cboTrackingColumn.Location = new System.Drawing.Point(140, 359);
            this.cboTrackingColumn.Name = "cboTrackingColumn";
            this.cboTrackingColumn.Size = new System.Drawing.Size(180, 21);
            this.cboTrackingColumn.TabIndex = 34;
            // 
            // lblLogDays
            // 
            this.lblLogDays.AutoSize = true;
            this.lblLogDays.Location = new System.Drawing.Point(22, 315);
            this.lblLogDays.Name = "lblLogDays";
            this.lblLogDays.Size = new System.Drawing.Size(225, 13);
            this.lblLogDays.TabIndex = 31;
            this.lblLogDays.Text = "Delete Logs older than X days (0=don\'t delete)";
            // 
            // txtLogDays
            // 
            this.txtLogDays.Location = new System.Drawing.Point(263, 312);
            this.txtLogDays.Name = "txtLogDays";
            this.txtLogDays.Size = new System.Drawing.Size(57, 20);
            this.txtLogDays.TabIndex = 32;
            this.txtLogDays.Text = "14";
            this.txtLogDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtUpdateUpMatters
            // 
            this.txtUpdateUpMatters.Location = new System.Drawing.Point(284, 204);
            this.txtUpdateUpMatters.Name = "txtUpdateUpMatters";
            this.txtUpdateUpMatters.Size = new System.Drawing.Size(150, 20);
            this.txtUpdateUpMatters.TabIndex = 23;
            this.txtUpdateUpMatters.Visible = false;
            // 
            // txtUpdateExMatters
            // 
            this.txtUpdateExMatters.Location = new System.Drawing.Point(284, 181);
            this.txtUpdateExMatters.Name = "txtUpdateExMatters";
            this.txtUpdateExMatters.Size = new System.Drawing.Size(150, 20);
            this.txtUpdateExMatters.TabIndex = 20;
            this.txtUpdateExMatters.Visible = false;
            // 
            // txtUpdateUpClients
            // 
            this.txtUpdateUpClients.Location = new System.Drawing.Point(284, 158);
            this.txtUpdateUpClients.Name = "txtUpdateUpClients";
            this.txtUpdateUpClients.Size = new System.Drawing.Size(150, 20);
            this.txtUpdateUpClients.TabIndex = 17;
            this.txtUpdateUpClients.Visible = false;
            // 
            // txtUpdateExClients
            // 
            this.txtUpdateExClients.Location = new System.Drawing.Point(284, 135);
            this.txtUpdateExClients.Name = "txtUpdateExClients";
            this.txtUpdateExClients.Size = new System.Drawing.Size(150, 20);
            this.txtUpdateExClients.TabIndex = 14;
            this.txtUpdateExClients.Visible = false;
            // 
            // txtUpdateUpContacts
            // 
            this.txtUpdateUpContacts.Location = new System.Drawing.Point(284, 112);
            this.txtUpdateUpContacts.Name = "txtUpdateUpContacts";
            this.txtUpdateUpContacts.Size = new System.Drawing.Size(150, 20);
            this.txtUpdateUpContacts.TabIndex = 11;
            this.txtUpdateUpContacts.Visible = false;
            // 
            // txtUpdateExContacts
            // 
            this.txtUpdateExContacts.Location = new System.Drawing.Point(284, 89);
            this.txtUpdateExContacts.Name = "txtUpdateExContacts";
            this.txtUpdateExContacts.Size = new System.Drawing.Size(150, 20);
            this.txtUpdateExContacts.TabIndex = 8;
            this.txtUpdateExContacts.Visible = false;
            // 
            // txtUpdateFinancials
            // 
            this.txtUpdateFinancials.Location = new System.Drawing.Point(284, 227);
            this.txtUpdateFinancials.Name = "txtUpdateFinancials";
            this.txtUpdateFinancials.Size = new System.Drawing.Size(150, 20);
            this.txtUpdateFinancials.TabIndex = 26;
            this.txtUpdateFinancials.Visible = false;
            // 
            // txtUpdateTime
            // 
            this.txtUpdateTime.Location = new System.Drawing.Point(284, 66);
            this.txtUpdateTime.Name = "txtUpdateTime";
            this.txtUpdateTime.Size = new System.Drawing.Size(150, 20);
            this.txtUpdateTime.TabIndex = 5;
            this.txtUpdateTime.Visible = false;
            // 
            // chkCustomUpdateUpMatters
            // 
            this.chkCustomUpdateUpMatters.AutoSize = true;
            this.chkCustomUpdateUpMatters.Location = new System.Drawing.Point(140, 206);
            this.chkCustomUpdateUpMatters.Name = "chkCustomUpdateUpMatters";
            this.chkCustomUpdateUpMatters.Size = new System.Drawing.Size(129, 17);
            this.chkCustomUpdateUpMatters.TabIndex = 22;
            this.chkCustomUpdateUpMatters.Text = "Custom Update Script";
            this.chkCustomUpdateUpMatters.UseVisualStyleBackColor = true;
            this.chkCustomUpdateUpMatters.CheckStateChanged += new System.EventHandler(this.chkCustomUpdateUpMatters_CheckStateChanged);
            // 
            // chkCustomUpdateExMatters
            // 
            this.chkCustomUpdateExMatters.AutoSize = true;
            this.chkCustomUpdateExMatters.Location = new System.Drawing.Point(140, 183);
            this.chkCustomUpdateExMatters.Name = "chkCustomUpdateExMatters";
            this.chkCustomUpdateExMatters.Size = new System.Drawing.Size(129, 17);
            this.chkCustomUpdateExMatters.TabIndex = 19;
            this.chkCustomUpdateExMatters.Text = "Custom Update Script";
            this.chkCustomUpdateExMatters.UseVisualStyleBackColor = true;
            this.chkCustomUpdateExMatters.CheckStateChanged += new System.EventHandler(this.chkCustomUpdateExMatters_CheckStateChanged);
            // 
            // chkCustomUpdateUpClients
            // 
            this.chkCustomUpdateUpClients.AutoSize = true;
            this.chkCustomUpdateUpClients.Location = new System.Drawing.Point(140, 160);
            this.chkCustomUpdateUpClients.Name = "chkCustomUpdateUpClients";
            this.chkCustomUpdateUpClients.Size = new System.Drawing.Size(129, 17);
            this.chkCustomUpdateUpClients.TabIndex = 16;
            this.chkCustomUpdateUpClients.Text = "Custom Update Script";
            this.chkCustomUpdateUpClients.UseVisualStyleBackColor = true;
            this.chkCustomUpdateUpClients.CheckStateChanged += new System.EventHandler(this.chkCustomUpdateUpClients_CheckStateChanged);
            // 
            // chkCustomUpdateExClients
            // 
            this.chkCustomUpdateExClients.AutoSize = true;
            this.chkCustomUpdateExClients.Location = new System.Drawing.Point(140, 137);
            this.chkCustomUpdateExClients.Name = "chkCustomUpdateExClients";
            this.chkCustomUpdateExClients.Size = new System.Drawing.Size(129, 17);
            this.chkCustomUpdateExClients.TabIndex = 13;
            this.chkCustomUpdateExClients.Text = "Custom Update Script";
            this.chkCustomUpdateExClients.UseVisualStyleBackColor = true;
            this.chkCustomUpdateExClients.CheckStateChanged += new System.EventHandler(this.chkCustomUpdateExClients_CheckStateChanged);
            // 
            // chkCustomUpdateUpContacts
            // 
            this.chkCustomUpdateUpContacts.AutoSize = true;
            this.chkCustomUpdateUpContacts.Location = new System.Drawing.Point(140, 114);
            this.chkCustomUpdateUpContacts.Name = "chkCustomUpdateUpContacts";
            this.chkCustomUpdateUpContacts.Size = new System.Drawing.Size(129, 17);
            this.chkCustomUpdateUpContacts.TabIndex = 10;
            this.chkCustomUpdateUpContacts.Text = "Custom Update Script";
            this.chkCustomUpdateUpContacts.UseVisualStyleBackColor = true;
            this.chkCustomUpdateUpContacts.CheckStateChanged += new System.EventHandler(this.chkCustomUpdateUpContacts_CheckStateChanged);
            // 
            // chkCustomUpdateExContacts
            // 
            this.chkCustomUpdateExContacts.AutoSize = true;
            this.chkCustomUpdateExContacts.Location = new System.Drawing.Point(140, 91);
            this.chkCustomUpdateExContacts.Name = "chkCustomUpdateExContacts";
            this.chkCustomUpdateExContacts.Size = new System.Drawing.Size(129, 17);
            this.chkCustomUpdateExContacts.TabIndex = 7;
            this.chkCustomUpdateExContacts.Text = "Custom Update Script";
            this.chkCustomUpdateExContacts.UseVisualStyleBackColor = true;
            this.chkCustomUpdateExContacts.CheckStateChanged += new System.EventHandler(this.chkCustomUpdateExContacts_CheckStateChanged);
            // 
            // chkCustomUpdateFinancials
            // 
            this.chkCustomUpdateFinancials.AutoSize = true;
            this.chkCustomUpdateFinancials.Location = new System.Drawing.Point(140, 229);
            this.chkCustomUpdateFinancials.Name = "chkCustomUpdateFinancials";
            this.chkCustomUpdateFinancials.Size = new System.Drawing.Size(129, 17);
            this.chkCustomUpdateFinancials.TabIndex = 25;
            this.chkCustomUpdateFinancials.Text = "Custom Update Script";
            this.chkCustomUpdateFinancials.UseVisualStyleBackColor = true;
            this.chkCustomUpdateFinancials.Visible = false;
            this.chkCustomUpdateFinancials.CheckStateChanged += new System.EventHandler(this.chkCustomUpdateFinancials_CheckStateChanged);
            // 
            // chkCustomUpdateTime
            // 
            this.chkCustomUpdateTime.AutoSize = true;
            this.chkCustomUpdateTime.Location = new System.Drawing.Point(140, 69);
            this.chkCustomUpdateTime.Name = "chkCustomUpdateTime";
            this.chkCustomUpdateTime.Size = new System.Drawing.Size(129, 17);
            this.chkCustomUpdateTime.TabIndex = 4;
            this.chkCustomUpdateTime.Text = "Custom Update Script";
            this.chkCustomUpdateTime.UseVisualStyleBackColor = true;
            this.chkCustomUpdateTime.CheckStateChanged += new System.EventHandler(this.chkCustomUpdateTime_CheckStateChanged);
            // 
            // txtExceptionThreshold
            // 
            this.txtExceptionThreshold.Location = new System.Drawing.Point(263, 286);
            this.txtExceptionThreshold.Name = "txtExceptionThreshold";
            this.txtExceptionThreshold.Size = new System.Drawing.Size(57, 20);
            this.txtExceptionThreshold.TabIndex = 30;
            this.txtExceptionThreshold.Text = "20";
            this.txtExceptionThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblExceptionThreshold
            // 
            this.lblExceptionThreshold.AutoSize = true;
            this.lblExceptionThreshold.Location = new System.Drawing.Point(22, 288);
            this.lblExceptionThreshold.Name = "lblExceptionThreshold";
            this.lblExceptionThreshold.Size = new System.Drawing.Size(144, 13);
            this.lblExceptionThreshold.TabIndex = 29;
            this.lblExceptionThreshold.Text = "Exception Threshold Number";
            // 
            // lblIntegrationApp
            // 
            this.lblIntegrationApp.AutoSize = true;
            this.lblIntegrationApp.Location = new System.Drawing.Point(22, 262);
            this.lblIntegrationApp.Name = "lblIntegrationApp";
            this.lblIntegrationApp.Size = new System.Drawing.Size(112, 13);
            this.lblIntegrationApp.TabIndex = 27;
            this.lblIntegrationApp.Text = "Integration Application";
            // 
            // cboIntegrationApp
            // 
            this.cboIntegrationApp.FormattingEnabled = true;
            this.cboIntegrationApp.Items.AddRange(new object[] {
            "Aderant - CMS",
            "Indigo",
            "Miles 33"});
            this.cboIntegrationApp.Location = new System.Drawing.Point(140, 258);
            this.cboIntegrationApp.Name = "cboIntegrationApp";
            this.cboIntegrationApp.Size = new System.Drawing.Size(180, 21);
            this.cboIntegrationApp.TabIndex = 28;
            this.cboIntegrationApp.SelectedIndexChanged += new System.EventHandler(this.cboIntegrationApp_SelectedIndexChanged);
            this.cboIntegrationApp.SelectedValueChanged += new System.EventHandler(this.cboIntegrationApp_SelectedValueChanged);
            this.cboIntegrationApp.Enter += new System.EventHandler(this.cboIntegrationApp_Enter);
            this.cboIntegrationApp.Leave += new System.EventHandler(this.cboIntegrationApp_Leave);
            // 
            // chkExportLookups
            // 
            this.chkExportLookups.AutoSize = true;
            this.chkExportLookups.Location = new System.Drawing.Point(25, 27);
            this.chkExportLookups.Name = "chkExportLookups";
            this.chkExportLookups.Size = new System.Drawing.Size(100, 17);
            this.chkExportLookups.TabIndex = 1;
            this.chkExportLookups.Text = "Export Lookups";
            this.chkExportLookups.UseVisualStyleBackColor = true;
            // 
            // chkExportFinancials
            // 
            this.chkExportFinancials.AutoSize = true;
            this.chkExportFinancials.Location = new System.Drawing.Point(25, 229);
            this.chkExportFinancials.Name = "chkExportFinancials";
            this.chkExportFinancials.Size = new System.Drawing.Size(106, 17);
            this.chkExportFinancials.TabIndex = 24;
            this.chkExportFinancials.Text = "Export Financials";
            this.chkExportFinancials.UseVisualStyleBackColor = true;
            this.chkExportFinancials.Visible = false;
            // 
            // chkExportTime
            // 
            this.chkExportTime.AutoSize = true;
            this.chkExportTime.Location = new System.Drawing.Point(25, 68);
            this.chkExportTime.Name = "chkExportTime";
            this.chkExportTime.Size = new System.Drawing.Size(82, 17);
            this.chkExportTime.TabIndex = 3;
            this.chkExportTime.Text = "Export Time";
            this.chkExportTime.UseVisualStyleBackColor = true;
            // 
            // chkUpdateMatters
            // 
            this.chkUpdateMatters.AutoSize = true;
            this.chkUpdateMatters.Location = new System.Drawing.Point(25, 206);
            this.chkUpdateMatters.Name = "chkUpdateMatters";
            this.chkUpdateMatters.Size = new System.Drawing.Size(99, 17);
            this.chkUpdateMatters.TabIndex = 21;
            this.chkUpdateMatters.Text = "Update Matters";
            this.chkUpdateMatters.UseVisualStyleBackColor = true;
            // 
            // chkExportMatters
            // 
            this.chkExportMatters.AutoSize = true;
            this.chkExportMatters.Location = new System.Drawing.Point(25, 183);
            this.chkExportMatters.Name = "chkExportMatters";
            this.chkExportMatters.Size = new System.Drawing.Size(94, 17);
            this.chkExportMatters.TabIndex = 18;
            this.chkExportMatters.Text = "Export Matters";
            this.chkExportMatters.UseVisualStyleBackColor = true;
            // 
            // chkUpdateClients
            // 
            this.chkUpdateClients.AutoSize = true;
            this.chkUpdateClients.Location = new System.Drawing.Point(25, 160);
            this.chkUpdateClients.Name = "chkUpdateClients";
            this.chkUpdateClients.Size = new System.Drawing.Size(95, 17);
            this.chkUpdateClients.TabIndex = 15;
            this.chkUpdateClients.Text = "Update Clients";
            this.chkUpdateClients.UseVisualStyleBackColor = true;
            // 
            // chkExportClients
            // 
            this.chkExportClients.AutoSize = true;
            this.chkExportClients.Location = new System.Drawing.Point(25, 137);
            this.chkExportClients.Name = "chkExportClients";
            this.chkExportClients.Size = new System.Drawing.Size(90, 17);
            this.chkExportClients.TabIndex = 12;
            this.chkExportClients.Text = "Export Clients";
            this.chkExportClients.UseVisualStyleBackColor = true;
            // 
            // chkUpdateContacts
            // 
            this.chkUpdateContacts.AutoSize = true;
            this.chkUpdateContacts.Location = new System.Drawing.Point(25, 114);
            this.chkUpdateContacts.Name = "chkUpdateContacts";
            this.chkUpdateContacts.Size = new System.Drawing.Size(106, 17);
            this.chkUpdateContacts.TabIndex = 9;
            this.chkUpdateContacts.Text = "Update Contacts";
            this.chkUpdateContacts.UseVisualStyleBackColor = true;
            // 
            // chkExportContacts
            // 
            this.chkExportContacts.AutoSize = true;
            this.chkExportContacts.Location = new System.Drawing.Point(25, 91);
            this.chkExportContacts.Name = "chkExportContacts";
            this.chkExportContacts.Size = new System.Drawing.Size(101, 17);
            this.chkExportContacts.TabIndex = 6;
            this.chkExportContacts.Text = "Export Contacts";
            this.chkExportContacts.UseVisualStyleBackColor = true;
            // 
            // chkExportUsers
            // 
            this.chkExportUsers.AutoSize = true;
            this.chkExportUsers.Location = new System.Drawing.Point(25, 47);
            this.chkExportUsers.Name = "chkExportUsers";
            this.chkExportUsers.Size = new System.Drawing.Size(86, 17);
            this.chkExportUsers.TabIndex = 2;
            this.chkExportUsers.Text = "Export Users";
            this.chkExportUsers.UseVisualStyleBackColor = true;
            // 
            // grpIGO
            // 
            this.grpIGO.Controls.Add(this.label26);
            this.grpIGO.Controls.Add(this.txtIGOMessage);
            this.grpIGO.Controls.Add(this.txtIGOVersion);
            this.grpIGO.Controls.Add(this.txtIGOProgramName);
            this.grpIGO.Controls.Add(this.txtIGOUserCode);
            this.grpIGO.Controls.Add(this.txtIGOComputer);
            this.grpIGO.Controls.Add(this.label22);
            this.grpIGO.Controls.Add(this.label21);
            this.grpIGO.Controls.Add(this.label20);
            this.grpIGO.Controls.Add(this.label19);
            this.grpIGO.Controls.Add(this.label18);
            this.grpIGO.Controls.Add(this.label17);
            this.grpIGO.Controls.Add(this.label16);
            this.grpIGO.Controls.Add(this.txtIGOBranchNo);
            this.grpIGO.Controls.Add(this.txtIGOCompanyNo);
            this.grpIGO.Controls.Add(this.label12);
            this.grpIGO.Controls.Add(this.label13);
            this.grpIGO.Controls.Add(this.txtIGOPassword);
            this.grpIGO.Controls.Add(this.txtIGOUsername);
            this.grpIGO.Controls.Add(this.chkIGOIntegrated);
            this.grpIGO.Controls.Add(this.btnIGOTest);
            this.grpIGO.Controls.Add(this.label14);
            this.grpIGO.Controls.Add(this.txtIGOServer);
            this.grpIGO.Controls.Add(this.label15);
            this.grpIGO.Controls.Add(this.txtIGODatabase);
            this.grpIGO.Controls.Add(this.panel2);
            this.grpIGO.Controls.Add(this.panel3);
            this.grpIGO.Location = new System.Drawing.Point(1000, 12);
            this.grpIGO.Name = "grpIGO";
            this.grpIGO.Size = new System.Drawing.Size(456, 364);
            this.grpIGO.TabIndex = 34;
            this.grpIGO.TabStop = false;
            this.grpIGO.Text = "Indigo Settings";
            // 
            // txtIGOMessage
            // 
            this.txtIGOMessage.Location = new System.Drawing.Point(91, 278);
            this.txtIGOMessage.MaxLength = 255;
            this.txtIGOMessage.Multiline = true;
            this.txtIGOMessage.Name = "txtIGOMessage";
            this.txtIGOMessage.Size = new System.Drawing.Size(342, 57);
            this.txtIGOMessage.TabIndex = 51;
            // 
            // txtIGOVersion
            // 
            this.txtIGOVersion.Location = new System.Drawing.Point(348, 207);
            this.txtIGOVersion.Name = "txtIGOVersion";
            this.txtIGOVersion.Size = new System.Drawing.Size(85, 20);
            this.txtIGOVersion.TabIndex = 50;
            // 
            // txtIGOProgramName
            // 
            this.txtIGOProgramName.Location = new System.Drawing.Point(91, 207);
            this.txtIGOProgramName.MaxLength = 50;
            this.txtIGOProgramName.Name = "txtIGOProgramName";
            this.txtIGOProgramName.Size = new System.Drawing.Size(182, 20);
            this.txtIGOProgramName.TabIndex = 49;
            // 
            // txtIGOUserCode
            // 
            this.txtIGOUserCode.Location = new System.Drawing.Point(348, 231);
            this.txtIGOUserCode.MaxLength = 16;
            this.txtIGOUserCode.Name = "txtIGOUserCode";
            this.txtIGOUserCode.Size = new System.Drawing.Size(85, 20);
            this.txtIGOUserCode.TabIndex = 48;
            // 
            // txtIGOComputer
            // 
            this.txtIGOComputer.Location = new System.Drawing.Point(91, 231);
            this.txtIGOComputer.MaxLength = 50;
            this.txtIGOComputer.Name = "txtIGOComputer";
            this.txtIGOComputer.Size = new System.Drawing.Size(182, 20);
            this.txtIGOComputer.TabIndex = 47;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(9, 280);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(50, 13);
            this.label22.TabIndex = 46;
            this.label22.Text = "Message";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(285, 214);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(42, 13);
            this.label21.TabIndex = 45;
            this.label21.Text = "Version";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(9, 214);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(77, 13);
            this.label20.TabIndex = 44;
            this.label20.Text = "Program Name";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(285, 238);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(57, 13);
            this.label19.TabIndex = 43;
            this.label19.Text = "User Code";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(9, 238);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(52, 13);
            this.label18.TabIndex = 42;
            this.label18.Text = "Computer";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(285, 190);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(58, 13);
            this.label17.TabIndex = 41;
            this.label17.Text = "Branch No";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(9, 190);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(68, 13);
            this.label16.TabIndex = 40;
            this.label16.Text = "Company No";
            // 
            // txtIGOBranchNo
            // 
            this.txtIGOBranchNo.Location = new System.Drawing.Point(348, 183);
            this.txtIGOBranchNo.MaxLength = 32;
            this.txtIGOBranchNo.Name = "txtIGOBranchNo";
            this.txtIGOBranchNo.Size = new System.Drawing.Size(85, 20);
            this.txtIGOBranchNo.TabIndex = 39;
            // 
            // txtIGOCompanyNo
            // 
            this.txtIGOCompanyNo.Location = new System.Drawing.Point(91, 183);
            this.txtIGOCompanyNo.MaxLength = 32;
            this.txtIGOCompanyNo.Name = "txtIGOCompanyNo";
            this.txtIGOCompanyNo.Size = new System.Drawing.Size(182, 20);
            this.txtIGOCompanyNo.TabIndex = 38;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(9, 123);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(135, 18);
            this.label12.TabIndex = 37;
            this.label12.Text = "Password";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(9, 99);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(135, 18);
            this.label13.TabIndex = 36;
            this.label13.Text = "User Name";
            // 
            // txtIGOPassword
            // 
            this.txtIGOPassword.Location = new System.Drawing.Point(149, 122);
            this.txtIGOPassword.Name = "txtIGOPassword";
            this.txtIGOPassword.PasswordChar = '*';
            this.txtIGOPassword.Size = new System.Drawing.Size(218, 20);
            this.txtIGOPassword.TabIndex = 35;
            // 
            // txtIGOUsername
            // 
            this.txtIGOUsername.Location = new System.Drawing.Point(149, 98);
            this.txtIGOUsername.Name = "txtIGOUsername";
            this.txtIGOUsername.Size = new System.Drawing.Size(218, 20);
            this.txtIGOUsername.TabIndex = 34;
            // 
            // chkIGOIntegrated
            // 
            this.chkIGOIntegrated.Checked = true;
            this.chkIGOIntegrated.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIGOIntegrated.Location = new System.Drawing.Point(9, 75);
            this.chkIGOIntegrated.Name = "chkIGOIntegrated";
            this.chkIGOIntegrated.Size = new System.Drawing.Size(128, 16);
            this.chkIGOIntegrated.TabIndex = 33;
            this.chkIGOIntegrated.Text = "Integrated Security";
            this.chkIGOIntegrated.CheckedChanged += new System.EventHandler(this.chkIGOIntegrated_CheckedChanged);
            // 
            // btnIGOTest
            // 
            this.btnIGOTest.Location = new System.Drawing.Point(375, 47);
            this.btnIGOTest.Name = "btnIGOTest";
            this.btnIGOTest.Size = new System.Drawing.Size(75, 23);
            this.btnIGOTest.TabIndex = 32;
            this.btnIGOTest.Text = "Test";
            this.btnIGOTest.Click += new System.EventHandler(this.btnIGOTest_Click);
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(9, 23);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(136, 18);
            this.label14.TabIndex = 31;
            this.label14.Text = "SQL Server Name";
            // 
            // txtIGOServer
            // 
            this.txtIGOServer.Location = new System.Drawing.Point(149, 21);
            this.txtIGOServer.Name = "txtIGOServer";
            this.txtIGOServer.Size = new System.Drawing.Size(218, 20);
            this.txtIGOServer.TabIndex = 28;
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(9, 50);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(135, 18);
            this.label15.TabIndex = 30;
            this.label15.Text = "Database Name";
            // 
            // txtIGODatabase
            // 
            this.txtIGODatabase.Location = new System.Drawing.Point(149, 47);
            this.txtIGODatabase.Name = "txtIGODatabase";
            this.txtIGODatabase.Size = new System.Drawing.Size(218, 20);
            this.txtIGODatabase.TabIndex = 29;
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(7, 174);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(443, 91);
            this.panel2.TabIndex = 52;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(7, 271);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(443, 81);
            this.panel3.TabIndex = 53;
            // 
            // grpE3E
            // 
            this.grpE3E.Controls.Add(this.chkE3EResetFlagInCaseOfFailure);
            this.grpE3E.Controls.Add(this.lblE3EResetFlagInCaseOfFailure);
            this.grpE3E.Controls.Add(this.chkE3EMatterConflictCheck);
            this.grpE3E.Controls.Add(this.lblE3EMatterConflictCheck);
            this.grpE3E.Controls.Add(this.chkE3ECancelProcess);
            this.grpE3E.Controls.Add(this.lblE3ECancelProcess);
            this.grpE3E.Controls.Add(this.txtE3EFailedLocation);
            this.grpE3E.Controls.Add(this.lblE3EFailedLocation);
            this.grpE3E.Controls.Add(this.txtE3ESuccessLocation);
            this.grpE3E.Controls.Add(this.lblE3ESuccessLocation);
            this.grpE3E.Controls.Add(this.chkE3EDebug);
            this.grpE3E.Controls.Add(this.lblE3EDebug);
            this.grpE3E.Controls.Add(this.chkE3ENewEndpoint);
            this.grpE3E.Controls.Add(this.lblE3ENewEndpoint);
            this.grpE3E.Controls.Add(this.txtE3EURL);
            this.grpE3E.Controls.Add(this.lblE3EBaseUrl);
            this.grpE3E.ForeColor = System.Drawing.Color.Black;
            this.grpE3E.Location = new System.Drawing.Point(91, 15);
            this.grpE3E.Name = "grpE3E";
            this.grpE3E.Size = new System.Drawing.Size(456, 364);
            this.grpE3E.TabIndex = 71;
            this.grpE3E.TabStop = false;
            this.grpE3E.Text = "Elite 3E Settings";
            // 
            // lblE3EResetFlagInCaseOfFailure
            // 
            this.lblE3EResetFlagInCaseOfFailure.AutoSize = true;
            this.lblE3EResetFlagInCaseOfFailure.Location = new System.Drawing.Point(9, 217);
            this.lblE3EResetFlagInCaseOfFailure.Name = "lblE3EResetFlagInCaseOfFailure";
            this.lblE3EResetFlagInCaseOfFailure.Size = new System.Drawing.Size(135, 13);
            this.lblE3EResetFlagInCaseOfFailure.TabIndex = 45;
            this.lblE3EResetFlagInCaseOfFailure.Text = "Reset flag in case of failure";
            // 
            // chkE3EMatterConflictCheck
            // 
            this.chkE3EMatterConflictCheck.AutoSize = true;
            this.chkE3EMatterConflictCheck.Location = new System.Drawing.Point(163, 191);
            this.chkE3EMatterConflictCheck.Name = "chkE3EMatterConflictCheck";
            this.chkE3EMatterConflictCheck.Size = new System.Drawing.Size(15, 14);
            this.chkE3EMatterConflictCheck.TabIndex = 44;
            this.chkE3EMatterConflictCheck.UseVisualStyleBackColor = true;
            // 
            // lblE3EMatterConflictCheck
            // 
            this.lblE3EMatterConflictCheck.AutoSize = true;
            this.lblE3EMatterConflictCheck.Location = new System.Drawing.Point(9, 191);
            this.lblE3EMatterConflictCheck.Name = "lblE3EMatterConflictCheck";
            this.lblE3EMatterConflictCheck.Size = new System.Drawing.Size(142, 13);
            this.lblE3EMatterConflictCheck.TabIndex = 43;
            this.lblE3EMatterConflictCheck.Text = "Matter Export Conflict Check";
            // 
            // chkE3ECancelProcess
            // 
            this.chkE3ECancelProcess.AutoSize = true;
            this.chkE3ECancelProcess.Location = new System.Drawing.Point(163, 165);
            this.chkE3ECancelProcess.Name = "chkE3ECancelProcess";
            this.chkE3ECancelProcess.Size = new System.Drawing.Size(15, 14);
            this.chkE3ECancelProcess.TabIndex = 42;
            this.chkE3ECancelProcess.UseVisualStyleBackColor = true;
            // 
            // lblE3ECancelProcess
            // 
            this.lblE3ECancelProcess.AutoSize = true;
            this.lblE3ECancelProcess.Location = new System.Drawing.Point(9, 165);
            this.lblE3ECancelProcess.Name = "lblE3ECancelProcess";
            this.lblE3ECancelProcess.Size = new System.Drawing.Size(100, 13);
            this.lblE3ECancelProcess.TabIndex = 41;
            this.lblE3ECancelProcess.Text = "MS_CancelProcess";
            // 
            // txtE3EFailedLocation
            // 
            this.txtE3EFailedLocation.Location = new System.Drawing.Point(163, 127);
            this.txtE3EFailedLocation.Name = "txtE3EFailedLocation";
            this.txtE3EFailedLocation.Size = new System.Drawing.Size(273, 20);
            this.txtE3EFailedLocation.TabIndex = 40;
            // 
            // lblE3EFailedLocation
            // 
            this.lblE3EFailedLocation.AutoSize = true;
            this.lblE3EFailedLocation.Location = new System.Drawing.Point(9, 128);
            this.lblE3EFailedLocation.Name = "lblE3EFailedLocation";
            this.lblE3EFailedLocation.Size = new System.Drawing.Size(79, 13);
            this.lblE3EFailedLocation.TabIndex = 39;
            this.lblE3EFailedLocation.Text = "Failed Location";
            // 
            // txtE3ESuccessLocation
            // 
            this.txtE3ESuccessLocation.Location = new System.Drawing.Point(163, 103);
            this.txtE3ESuccessLocation.Name = "txtE3ESuccessLocation";
            this.txtE3ESuccessLocation.Size = new System.Drawing.Size(273, 20);
            this.txtE3ESuccessLocation.TabIndex = 38;
            // 
            // lblE3ESuccessLocation
            // 
            this.lblE3ESuccessLocation.AutoSize = true;
            this.lblE3ESuccessLocation.Location = new System.Drawing.Point(9, 105);
            this.lblE3ESuccessLocation.Name = "lblE3ESuccessLocation";
            this.lblE3ESuccessLocation.Size = new System.Drawing.Size(92, 13);
            this.lblE3ESuccessLocation.TabIndex = 37;
            this.lblE3ESuccessLocation.Text = "Success Location";
            // 
            // chkE3EDebug
            // 
            this.chkE3EDebug.AutoSize = true;
            this.chkE3EDebug.Location = new System.Drawing.Point(163, 82);
            this.chkE3EDebug.Name = "chkE3EDebug";
            this.chkE3EDebug.Size = new System.Drawing.Size(15, 14);
            this.chkE3EDebug.TabIndex = 36;
            this.chkE3EDebug.UseVisualStyleBackColor = true;
            // 
            // lblE3EDebug
            // 
            this.lblE3EDebug.AutoSize = true;
            this.lblE3EDebug.Location = new System.Drawing.Point(9, 82);
            this.lblE3EDebug.Name = "lblE3EDebug";
            this.lblE3EDebug.Size = new System.Drawing.Size(69, 13);
            this.lblE3EDebug.TabIndex = 35;
            this.lblE3EDebug.Text = "Debug Mode";
            // 
            // chkE3ENewEndpoint
            // 
            this.chkE3ENewEndpoint.AutoSize = true;
            this.chkE3ENewEndpoint.Location = new System.Drawing.Point(163, 49);
            this.chkE3ENewEndpoint.Name = "chkE3ENewEndpoint";
            this.chkE3ENewEndpoint.Size = new System.Drawing.Size(15, 14);
            this.chkE3ENewEndpoint.TabIndex = 34;
            this.chkE3ENewEndpoint.UseVisualStyleBackColor = true;
            // 
            // lblE3ENewEndpoint
            // 
            this.lblE3ENewEndpoint.AutoSize = true;
            this.lblE3ENewEndpoint.Location = new System.Drawing.Point(9, 49);
            this.lblE3ENewEndpoint.Name = "lblE3ENewEndpoint";
            this.lblE3ENewEndpoint.Size = new System.Drawing.Size(74, 13);
            this.lblE3ENewEndpoint.TabIndex = 33;
            this.lblE3ENewEndpoint.Text = "New Endpoint";
            // 
            // txtE3EURL
            // 
            this.txtE3EURL.Location = new System.Drawing.Point(163, 20);
            this.txtE3EURL.Name = "txtE3EURL";
            this.txtE3EURL.Size = new System.Drawing.Size(273, 20);
            this.txtE3EURL.TabIndex = 32;
            // 
            // lblE3EBaseUrl
            // 
            this.lblE3EBaseUrl.AutoSize = true;
            this.lblE3EBaseUrl.Location = new System.Drawing.Point(9, 23);
            this.lblE3EBaseUrl.Name = "lblE3EBaseUrl";
            this.lblE3EBaseUrl.Size = new System.Drawing.Size(47, 13);
            this.lblE3EBaseUrl.TabIndex = 31;
            this.lblE3EBaseUrl.Text = "Base Url";
            // 
            // grpENT
            // 
            this.grpENT.Controls.Add(this.txtEnterpriseDateFormat);
            this.grpENT.Controls.Add(this.lblEnterpriseDateFormat);
            this.grpENT.Controls.Add(this.txtEnterpriseServiceUserID);
            this.grpENT.Controls.Add(this.label27);
            this.grpENT.Controls.Add(this.lblEnterpriseBaseUrl);
            this.grpENT.Controls.Add(this.txtEnterpriseBaseURL);
            this.grpENT.ForeColor = System.Drawing.Color.Black;
            this.grpENT.Location = new System.Drawing.Point(91, 15);
            this.grpENT.Name = "grpENT";
            this.grpENT.Size = new System.Drawing.Size(462, 366);
            this.grpENT.TabIndex = 72;
            this.grpENT.TabStop = false;
            this.grpENT.Text = "Enterprise Settings";
            // 
            // txtEnterpriseDateFormat
            // 
            this.txtEnterpriseDateFormat.Location = new System.Drawing.Point(134, 69);
            this.txtEnterpriseDateFormat.Name = "txtEnterpriseDateFormat";
            this.txtEnterpriseDateFormat.Size = new System.Drawing.Size(100, 20);
            this.txtEnterpriseDateFormat.TabIndex = 5;
            // 
            // lblEnterpriseDateFormat
            // 
            this.lblEnterpriseDateFormat.AutoSize = true;
            this.lblEnterpriseDateFormat.Location = new System.Drawing.Point(9, 69);
            this.lblEnterpriseDateFormat.Name = "lblEnterpriseDateFormat";
            this.lblEnterpriseDateFormat.Size = new System.Drawing.Size(65, 13);
            this.lblEnterpriseDateFormat.TabIndex = 4;
            this.lblEnterpriseDateFormat.Text = "Date Format";
            // 
            // txtEnterpriseServiceUserID
            // 
            this.txtEnterpriseServiceUserID.Location = new System.Drawing.Point(134, 42);
            this.txtEnterpriseServiceUserID.Name = "txtEnterpriseServiceUserID";
            this.txtEnterpriseServiceUserID.Size = new System.Drawing.Size(100, 20);
            this.txtEnterpriseServiceUserID.TabIndex = 3;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.ForeColor = System.Drawing.Color.Black;
            this.label27.Location = new System.Drawing.Point(9, 43);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(82, 13);
            this.label27.TabIndex = 2;
            this.label27.Text = "Service User ID";
            // 
            // lblEnterpriseBaseUrl
            // 
            this.lblEnterpriseBaseUrl.AutoSize = true;
            this.lblEnterpriseBaseUrl.ForeColor = System.Drawing.Color.Black;
            this.lblEnterpriseBaseUrl.Location = new System.Drawing.Point(9, 20);
            this.lblEnterpriseBaseUrl.Name = "lblEnterpriseBaseUrl";
            this.lblEnterpriseBaseUrl.Size = new System.Drawing.Size(56, 13);
            this.lblEnterpriseBaseUrl.TabIndex = 1;
            this.lblEnterpriseBaseUrl.Text = "Base URL";
            // 
            // txtEnterpriseBaseURL
            // 
            this.txtEnterpriseBaseURL.Location = new System.Drawing.Point(134, 17);
            this.txtEnterpriseBaseURL.Name = "txtEnterpriseBaseURL";
            this.txtEnterpriseBaseURL.Size = new System.Drawing.Size(290, 20);
            this.txtEnterpriseBaseURL.TabIndex = 0;
            // 
            // grpC3E
            // 
            this.grpC3E.Controls.Add(this.txtC3EAzureADInstanceId);
            this.grpC3E.Controls.Add(this.lblC3EAzureADInstanceId);
            this.grpC3E.Controls.Add(this.txtC3EAzureADTenantId);
            this.grpC3E.Controls.Add(this.lblC3EAzureADTenantId);
            this.grpC3E.Controls.Add(this.txtC3EAzureADClientSecret);
            this.grpC3E.Controls.Add(this.lblC3EAzureADClientSecret);
            this.grpC3E.Controls.Add(this.txtC3EAzureADClientId);
            this.grpC3E.Controls.Add(this.lblC3EAzureADClientId);
            this.grpC3E.Controls.Add(this.txtC3EAzureADAudience);
            this.grpC3E.Controls.Add(this.lblC3EAzureADAudience);
            this.grpC3E.Controls.Add(this.chkC3EAzureADAuth);
            this.grpC3E.Controls.Add(this.lblC3EAuthorizationMode);
            this.grpC3E.Controls.Add(this.chkC3EImportInvoices);
            this.grpC3E.Controls.Add(this.lblC3EImportInvoices);
            this.grpC3E.Controls.Add(this.chkC3EResetFlagInCaseOfFailure);
            this.grpC3E.Controls.Add(this.lblC3EResetFlagInCaseOfFailure);
            this.grpC3E.Controls.Add(this.chkC3EMatterConflictCheck);
            this.grpC3E.Controls.Add(this.lblC3EMatterConflictCheck);
            this.grpC3E.Controls.Add(this.txtC3EFailedLocation);
            this.grpC3E.Controls.Add(this.lblC3EFailedLocation);
            this.grpC3E.Controls.Add(this.txtC3ESuccessLocation);
            this.grpC3E.Controls.Add(this.lblC3ESuccessLocation);
            this.grpC3E.Controls.Add(this.chkC3EDebug);
            this.grpC3E.Controls.Add(this.lblC3EDebug);
            this.grpC3E.Controls.Add(this.txtC3EURL);
            this.grpC3E.Controls.Add(this.lblC3EBaseUrl);
            this.grpC3E.ForeColor = System.Drawing.Color.Black;
            this.grpC3E.Location = new System.Drawing.Point(91, 15);
            this.grpC3E.Name = "grpC3E";
            this.grpC3E.Size = new System.Drawing.Size(456, 364);
            this.grpC3E.TabIndex = 73;
            this.grpC3E.TabStop = false;
            this.grpC3E.Text = "Elite 3E Cloud Settings";
            // 
            // txtC3EAzureADTenantId
            // 
            this.txtC3EAzureADTenantId.Location = new System.Drawing.Point(163, 333);
            this.txtC3EAzureADTenantId.Name = "txtC3EAzureADTenantId";
            this.txtC3EAzureADTenantId.Size = new System.Drawing.Size(273, 20);
            this.txtC3EAzureADTenantId.TabIndex = 56;
            this.txtC3EAzureADTenantId.Visible = false;
            // 
            // lblC3EAzureADTenantId
            // 
            this.lblC3EAzureADTenantId.AutoSize = true;
            this.lblC3EAzureADTenantId.Location = new System.Drawing.Point(9, 333);
            this.lblC3EAzureADTenantId.Name = "lblC3EAzureADTenantId";
            this.lblC3EAzureADTenantId.Size = new System.Drawing.Size(53, 13);
            this.lblC3EAzureADTenantId.TabIndex = 55;
            this.lblC3EAzureADTenantId.Text = "Tenant Id";
            this.lblC3EAzureADTenantId.Visible = false;
            // 
            // txtC3EAzureADClientSecret
            // 
            this.txtC3EAzureADClientSecret.Location = new System.Drawing.Point(163, 309);
            this.txtC3EAzureADClientSecret.Name = "txtC3EAzureADClientSecret";
            this.txtC3EAzureADClientSecret.PasswordChar = '*';
            this.txtC3EAzureADClientSecret.Size = new System.Drawing.Size(273, 20);
            this.txtC3EAzureADClientSecret.TabIndex = 54;
            this.txtC3EAzureADClientSecret.Visible = false;
            // 
            // lblC3EAzureADClientSecret
            // 
            this.lblC3EAzureADClientSecret.AutoSize = true;
            this.lblC3EAzureADClientSecret.Location = new System.Drawing.Point(9, 309);
            this.lblC3EAzureADClientSecret.Name = "lblC3EAzureADClientSecret";
            this.lblC3EAzureADClientSecret.Size = new System.Drawing.Size(67, 13);
            this.lblC3EAzureADClientSecret.TabIndex = 53;
            this.lblC3EAzureADClientSecret.Text = "Client Secret";
            this.lblC3EAzureADClientSecret.Visible = false;
            // 
            // txtC3EAzureADClientId
            // 
            this.txtC3EAzureADClientId.Location = new System.Drawing.Point(163, 285);
            this.txtC3EAzureADClientId.Name = "txtC3EAzureADClientId";
            this.txtC3EAzureADClientId.Size = new System.Drawing.Size(273, 20);
            this.txtC3EAzureADClientId.TabIndex = 52;
            this.txtC3EAzureADClientId.Visible = false;
            // 
            // lblC3EAzureADClientId
            // 
            this.lblC3EAzureADClientId.AutoSize = true;
            this.lblC3EAzureADClientId.Location = new System.Drawing.Point(9, 285);
            this.lblC3EAzureADClientId.Name = "lblC3EAzureADClientId";
            this.lblC3EAzureADClientId.Size = new System.Drawing.Size(45, 13);
            this.lblC3EAzureADClientId.TabIndex = 51;
            this.lblC3EAzureADClientId.Text = "Client Id";
            this.lblC3EAzureADClientId.Visible = false;
            // 
            // txtC3EAzureADAudience
            // 
            this.txtC3EAzureADAudience.Location = new System.Drawing.Point(163, 237);
            this.txtC3EAzureADAudience.Name = "txtC3EAzureADAudience";
            this.txtC3EAzureADAudience.Size = new System.Drawing.Size(273, 20);
            this.txtC3EAzureADAudience.TabIndex = 50;
            this.txtC3EAzureADAudience.Visible = false;
            // 
            // lblC3EAzureADAudience
            // 
            this.lblC3EAzureADAudience.AutoSize = true;
            this.lblC3EAzureADAudience.Location = new System.Drawing.Point(9, 237);
            this.lblC3EAzureADAudience.Name = "lblC3EAzureADAudience";
            this.lblC3EAzureADAudience.Size = new System.Drawing.Size(52, 13);
            this.lblC3EAzureADAudience.TabIndex = 49;
            this.lblC3EAzureADAudience.Text = "Audience";
            this.lblC3EAzureADAudience.Visible = false;
            // 
            // chkC3EAzureADAuth
            // 
            this.chkC3EAzureADAuth.AutoSize = true;
            this.chkC3EAzureADAuth.Location = new System.Drawing.Point(163, 216);
            this.chkC3EAzureADAuth.Name = "chkC3EAzureADAuth";
            this.chkC3EAzureADAuth.Size = new System.Drawing.Size(15, 14);
            this.chkC3EAzureADAuth.TabIndex = 48;
            this.chkC3EAzureADAuth.UseVisualStyleBackColor = true;
            this.chkC3EAzureADAuth.CheckedChanged += new System.EventHandler(this.chkC3EAzureADAuth_CheckedChanged);
            // 
            // lblC3EAuthorizationMode
            // 
            this.lblC3EAuthorizationMode.AutoSize = true;
            this.lblC3EAuthorizationMode.Location = new System.Drawing.Point(9, 216);
            this.lblC3EAuthorizationMode.Name = "lblC3EAuthorizationMode";
            this.lblC3EAuthorizationMode.Size = new System.Drawing.Size(127, 13);
            this.lblC3EAuthorizationMode.TabIndex = 47;
            this.lblC3EAuthorizationMode.Text = "Use Elite ID Authorization";
            // 
            // chkC3EImportInvoices
            // 
            this.chkC3EImportInvoices.AutoSize = true;
            this.chkC3EImportInvoices.Location = new System.Drawing.Point(163, 182);
            this.chkC3EImportInvoices.Name = "chkC3EImportInvoices";
            this.chkC3EImportInvoices.Size = new System.Drawing.Size(15, 14);
            this.chkC3EImportInvoices.TabIndex = 46;
            this.chkC3EImportInvoices.UseVisualStyleBackColor = true;
            // 
            // lblC3EImportInvoices
            // 
            this.lblC3EImportInvoices.AutoSize = true;
            this.lblC3EImportInvoices.Location = new System.Drawing.Point(9, 182);
            this.lblC3EImportInvoices.Name = "lblC3EImportInvoices";
            this.lblC3EImportInvoices.Size = new System.Drawing.Size(152, 13);
            this.lblC3EImportInvoices.TabIndex = 45;
            this.lblC3EImportInvoices.Text = "Import 3E Invoice Attachments";
            // 
            // lblC3EResetFlagInCaseOfFailure
            // 
            this.lblC3EResetFlagInCaseOfFailure.AutoSize = true;
            this.lblC3EResetFlagInCaseOfFailure.Location = new System.Drawing.Point(9, 156);
            this.lblC3EResetFlagInCaseOfFailure.Name = "lblC3EResetFlagInCaseOfFailure";
            this.lblC3EResetFlagInCaseOfFailure.Size = new System.Drawing.Size(135, 13);
            this.lblC3EResetFlagInCaseOfFailure.TabIndex = 43;
            this.lblC3EResetFlagInCaseOfFailure.Text = "Reset flag in case of failure";
            // 
            // chkC3EMatterConflictCheck
            // 
            this.chkC3EMatterConflictCheck.AutoSize = true;
            this.chkC3EMatterConflictCheck.Location = new System.Drawing.Point(163, 130);
            this.chkC3EMatterConflictCheck.Name = "chkC3EMatterConflictCheck";
            this.chkC3EMatterConflictCheck.Size = new System.Drawing.Size(15, 14);
            this.chkC3EMatterConflictCheck.TabIndex = 42;
            this.chkC3EMatterConflictCheck.UseVisualStyleBackColor = true;
            // 
            // lblC3EMatterConflictCheck
            // 
            this.lblC3EMatterConflictCheck.AutoSize = true;
            this.lblC3EMatterConflictCheck.Location = new System.Drawing.Point(9, 130);
            this.lblC3EMatterConflictCheck.Name = "lblC3EMatterConflictCheck";
            this.lblC3EMatterConflictCheck.Size = new System.Drawing.Size(142, 13);
            this.lblC3EMatterConflictCheck.TabIndex = 41;
            this.lblC3EMatterConflictCheck.Text = "Matter Export Conflict Check";
            // 
            // txtC3EFailedLocation
            // 
            this.txtC3EFailedLocation.Location = new System.Drawing.Point(163, 92);
            this.txtC3EFailedLocation.Name = "txtC3EFailedLocation";
            this.txtC3EFailedLocation.Size = new System.Drawing.Size(273, 20);
            this.txtC3EFailedLocation.TabIndex = 40;
            // 
            // lblC3EFailedLocation
            // 
            this.lblC3EFailedLocation.AutoSize = true;
            this.lblC3EFailedLocation.Location = new System.Drawing.Point(9, 93);
            this.lblC3EFailedLocation.Name = "lblC3EFailedLocation";
            this.lblC3EFailedLocation.Size = new System.Drawing.Size(79, 13);
            this.lblC3EFailedLocation.TabIndex = 39;
            this.lblC3EFailedLocation.Text = "Failed Location";
            // 
            // txtC3ESuccessLocation
            // 
            this.txtC3ESuccessLocation.Location = new System.Drawing.Point(163, 68);
            this.txtC3ESuccessLocation.Name = "txtC3ESuccessLocation";
            this.txtC3ESuccessLocation.Size = new System.Drawing.Size(273, 20);
            this.txtC3ESuccessLocation.TabIndex = 38;
            // 
            // lblC3ESuccessLocation
            // 
            this.lblC3ESuccessLocation.AutoSize = true;
            this.lblC3ESuccessLocation.Location = new System.Drawing.Point(9, 70);
            this.lblC3ESuccessLocation.Name = "lblC3ESuccessLocation";
            this.lblC3ESuccessLocation.Size = new System.Drawing.Size(92, 13);
            this.lblC3ESuccessLocation.TabIndex = 37;
            this.lblC3ESuccessLocation.Text = "Success Location";
            // 
            // chkC3EDebug
            // 
            this.chkC3EDebug.AutoSize = true;
            this.chkC3EDebug.Location = new System.Drawing.Point(163, 47);
            this.chkC3EDebug.Name = "chkC3EDebug";
            this.chkC3EDebug.Size = new System.Drawing.Size(15, 14);
            this.chkC3EDebug.TabIndex = 36;
            this.chkC3EDebug.UseVisualStyleBackColor = true;
            // 
            // lblC3EDebug
            // 
            this.lblC3EDebug.AutoSize = true;
            this.lblC3EDebug.Location = new System.Drawing.Point(9, 47);
            this.lblC3EDebug.Name = "lblC3EDebug";
            this.lblC3EDebug.Size = new System.Drawing.Size(69, 13);
            this.lblC3EDebug.TabIndex = 35;
            this.lblC3EDebug.Text = "Debug Mode";
            // 
            // txtC3EURL
            // 
            this.txtC3EURL.Location = new System.Drawing.Point(163, 20);
            this.txtC3EURL.Name = "txtC3EURL";
            this.txtC3EURL.Size = new System.Drawing.Size(273, 20);
            this.txtC3EURL.TabIndex = 32;
            // 
            // lblC3EBaseUrl
            // 
            this.lblC3EBaseUrl.AutoSize = true;
            this.lblC3EBaseUrl.Location = new System.Drawing.Point(9, 23);
            this.lblC3EBaseUrl.Name = "lblC3EBaseUrl";
            this.lblC3EBaseUrl.Size = new System.Drawing.Size(47, 13);
            this.lblC3EBaseUrl.TabIndex = 31;
            this.lblC3EBaseUrl.Text = "Base Url";
            // 
            // txtC3EAzureADInstanceId
            // 
            this.txtC3EAzureADInstanceId.Location = new System.Drawing.Point(163, 261);
            this.txtC3EAzureADInstanceId.Name = "txtC3EAzureADInstanceId";
            this.txtC3EAzureADInstanceId.Size = new System.Drawing.Size(273, 20);
            this.txtC3EAzureADInstanceId.TabIndex = 58;
            this.txtC3EAzureADInstanceId.Visible = false;
            // 
            // lblC3EAzureADInstanceId
            // 
            this.lblC3EAzureADInstanceId.AutoSize = true;
            this.lblC3EAzureADInstanceId.Location = new System.Drawing.Point(9, 261);
            this.lblC3EAzureADInstanceId.Name = "lblC3EAzureADInstanceId";
            this.lblC3EAzureADInstanceId.Size = new System.Drawing.Size(85, 13);
            this.lblC3EAzureADInstanceId.TabIndex = 57;
            this.lblC3EAzureADInstanceId.Text = "AAD Instance Id";
            this.lblC3EAzureADInstanceId.Visible = false;
            // 
            // frmSettings
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(559, 469);
            this.Controls.Add(this.grpC3E);
            this.Controls.Add(this.grpE3E);
            this.Controls.Add(this.grpExports);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.grpCMS);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grp4);
            this.Controls.Add(this.grp3);
            this.Controls.Add(this.grp2);
            this.Controls.Add(this.grp1);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.grpMIL);
            this.Controls.Add(this.grpIGO);
            this.Controls.Add(this.grpENT);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.ShowInTaskbar = false;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            this.grp4.ResumeLayout(false);
            this.grp4.PerformLayout();
            this.grp3.ResumeLayout(false);
            this.grp3.PerformLayout();
            this.grp2.ResumeLayout(false);
            this.grp2.PerformLayout();
            this.grp1.ResumeLayout(false);
            this.grp1.PerformLayout();
            this.grpCMS.ResumeLayout(false);
            this.grpCMS.PerformLayout();
            this.grpMIL.ResumeLayout(false);
            this.grpMIL.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.grpExports.ResumeLayout(false);
            this.grpExports.PerformLayout();
            this.grpIGO.ResumeLayout(false);
            this.grpIGO.PerformLayout();
            this.grpE3E.ResumeLayout(false);
            this.grpE3E.PerformLayout();
            this.grpENT.ResumeLayout(false);
            this.grpENT.PerformLayout();
            this.grpC3E.ResumeLayout(false);
            this.grpC3E.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region Event Procedures
		
		/// <summary>
		/// Test the connection to the specified OMS SQL server
		/// </summary>
		private void btnIGOTest_Click(object sender, System.EventArgs e)
		{
			//build connection string
			System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection();
			
			try
			{
				cnn.ConnectionString = IGOConnectionString;
				
				Cursor.Current = Cursors.WaitCursor;
				
				cnn.Open();
				cnn.Close();
				MessageBox.Show("Connection OK");			
				
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);			
			}
			finally
			{
				Cursor.Current = Cursors.Default;
				cnn.Dispose();
			}
		}

		private void btnTestSQL_Click(object sender, System.EventArgs e)
		{
			//build connection string
			System.Data.SqlClient.SqlConnection cnn = new System.Data.SqlClient.SqlConnection();
			
			try
			{
				cnn.ConnectionString = SQLConnectionString;
				
				Cursor.Current = Cursors.WaitCursor;
				
				cnn.Open();
				cnn.Close();
				MessageBox.Show("Connection OK");			
				
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);			
			}
			finally
			{
				Cursor.Current = Cursors.Default;
				cnn.Dispose();
			}
		}



		/// <summary>
		/// Tests the email using settings in text box.
		/// </summary>
		private void btnTest_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor.Current = Cursors.WaitCursor;
                StaticLibrary.SmtpSettings SMTPServer = new StaticLibrary.SmtpSettings()
                {
                    Address = txtSmtpServer.Text,
                    Encryption = cmbSmtpEncryption.SelectedItem as string,
                    Authenticate = chkSmtpAuthenticate.Checked,
                    Login = txtSmtpLogin.Text,
                    Password = txtSmtpPassword.Text
                };
                StaticLibrary.SendEmail(txtEmailAddress.Text, txtEmailFrom.Text, "FWBS OMS Export Service - Test",
				    "This is a test message send from the OMS Export Service configuration utility.", SMTPServer, true); 
				MessageBox.Show("Message Sent.",DIALOG_CAPTION);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error sending email... " + ex.Message,DIALOG_CAPTION);
			}
			finally
			{
				Cursor.Current = Cursors.Default;
			}
		}
		
		
		/// <summary>
		/// Responds to the close button being clicked
		/// </summary>
		private void btnClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}


        /// <summary>
        /// Used to browse to a folder that will be used for the performance log file 
        /// </summary>
        private void btnBrowse_Click(object sender, System.EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFilePath.Text))
            {
                folderBrowserDialog1.SelectedPath = txtFilePath.Text;
            }

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                if (folderName.Length > 0)
                {
                    txtFilePath.Text = folderName;
                }
            }
        }

		
		/// <summary>
		/// load initial values when form loads
		/// </summary>
		private void frmSettings_Load(object sender, System.EventArgs e)
		{
            try
            {
                //set browse path read only but keep normal backcolour
                Color colour1 = new Color();
                Color colour2 = new Color();
                colour1 = txtFilePath.BackColor;
                colour2 = txtFilePath.ForeColor;
                txtFilePath.ReadOnly = true;
                txtFilePath.BackColor = colour1;
                txtFilePath.ForeColor = colour2;

                SetDefaultScreen();

                GetInitialValues();

                GetExportObjectValues();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "OMS Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
		}

        /// <summary>
        /// Display options for intital load
        /// </summary>
        private void SetDefaultScreen()
        {
            grp1.Left = 85;
            grp2.Left = 85;
            grp3.Left = 85;
            grp4.Left = 85;
            grpCMS.Left = 1000;
            grpIGO.Left = 1000;
            grpMIL.Left = 1000;
            grpE3E.Left = 1000;
            grpC3E.Left = 1000;
            grpENT.Left = 1000;
            grpExports.Left = 1000;
            
            chkExports.Checked = false;
            chkCMS.Checked = false;
            chkMIL.Checked = true;
            chkCommon.Checked = true;
        }
		
		
		/// <summary>
		/// update the registry
		/// </summary>
		private void btnUpdate_Click(object sender, System.EventArgs e)
		{
			//validate the fields
			try
			{
				Int32.Parse(txtPauseMins.Text);
			}
			catch
			{
				MessageBox.Show(this,"Please ensure Pause value is numeric.",DIALOG_CAPTION);
				return;
			}
			
			try
			{
				string dir = txtFilePath.Text;
				System.IO.DirectoryInfo dirinf = new System.IO.DirectoryInfo(dir);
			}
			catch
			{
				MessageBox.Show(this,"Specified folder does not exist please browse to existing folder",DIALOG_CAPTION);
				return;
			}
            try
            {
                Int32.Parse(txtLogDays.Text);
            }
            catch
            {
                MessageBox.Show(this, "Please ensure Delete Log Days value is numeric.", DIALOG_CAPTION);
                return;
            }
	
			//update registry
			if(UpdateValues())
				this.Close();
		}
		
		
		#endregion
		
		#region Methods
		
		/// <summary>
		/// Populates controls with values read in from registry
		/// </summary>
		private void GetInitialValues()
		{
			chkFullEventLog.Checked = StaticLibrary.GetBoolSetting("DetailedLogging","",false);

            chkLogToDatabase.Checked = StaticLibrary.GetBoolSetting("LogToDB", "", false);

            txtFilePath.Text = StaticLibrary.GetSetting("StatLogFolder","","");
            txtServerName.Text = StaticLibrary.GetSetting("OMSSQLServer", "", "");
            txtDatabaseName.Text = StaticLibrary.GetSetting("OMSSQLDatabase", "", "");
            txtUserName.Text = StaticLibrary.GetSetting("OMSSQLUID", "", "");
            txtPassword.Text = StaticLibrary.GetSetting("OMSSQLPWD", "", "");
            txtEmailAddress.Text = StaticLibrary.GetSetting("EmailAddress", "", "");
            txtEmailFrom.Text = StaticLibrary.GetSetting("EmailFrom", "", "");
            StaticLibrary.SmtpSettings smtpSettings = StaticLibrary.GetSmtpSettings();
            txtSmtpServer.Text = smtpSettings.Address;
            cmbSmtpEncryption.SelectedItem = smtpSettings.Encryption;
            chkSmtpAuthenticate.Checked = smtpSettings.Authenticate;
            txtSmtpLogin.Text = smtpSettings.Login;
            txtSmtpPassword.Text = smtpSettings.Password;            
            txtPauseMins.Text = StaticLibrary.GetSetting("PauseInterval", "", "");
            chkEmailAdmin.Checked = StaticLibrary.GetBoolSetting("EmailErrors", "",false);
            txtExceptionThreshold.Text = StaticLibrary.GetSetting("ExceptionThreashold", "", "20");

            txtLogDays.Text = StaticLibrary.GetSetting("LogDays", "", "14");
            
            //export options
            chkExportUsers.Checked = StaticLibrary.GetBoolSetting("ExportUsers", "",false);
            chkExportContacts.Checked = StaticLibrary.GetBoolSetting("ExportContacts", "", true);
            chkUpdateContacts.Checked = StaticLibrary.GetBoolSetting("UpdateContacts", "", true);
            chkExportClients.Checked = StaticLibrary.GetBoolSetting("ExportClients", "",true);
            chkUpdateClients.Checked = StaticLibrary.GetBoolSetting("UpdateClients", "",true);
            chkExportMatters.Checked = StaticLibrary.GetBoolSetting("ExportMatters", "",true);
            chkUpdateMatters.Checked = StaticLibrary.GetBoolSetting("UpdateMatters", "",true);
            chkExportTime.Checked = StaticLibrary.GetBoolSetting("ExportTime", "",true);
            chkExportFinancials.Checked = false;
            chkExportLookups.Checked = StaticLibrary.GetBoolSetting("ExportLookups", "", false);

            //  Tracking Colmun type
            cboTrackingColumn.SelectedItem = StaticLibrary.GetSetting("TrackingColumnType", "", "Integer");

            string loginType = StaticLibrary.GetSetting("OMSLoginType", "", "").ToUpper();
			if(loginType == "SQL")
			{
				chkIntegrated.Checked = false;
				chkAzure.Visible = false;
				txtUserName.Enabled = true;
				txtPassword.Enabled = true;
			}
			else
			{
				chkIntegrated.Checked = true;
				chkAzure.Checked = (loginType == "AAD");
				txtUserName.Enabled = false;
				txtPassword.Enabled = false;
			}
			
			
			if(!chkEmailAdmin.Checked)
			{
				txtEmailAddress.Enabled = false;
                txtEmailFrom.Enabled = false;
                txtSmtpServer.Enabled = false;
                cmbSmtpEncryption.Enabled = false;
                chkSmtpAuthenticate.Enabled = false;
                txtSmtpLogin.Enabled = false;
                txtSmtpPassword.Enabled = false;
			}

            _exportApp = StaticLibrary.GetExportAppName();

            List<KeyValuePair<string, string>> apps = new List<KeyValuePair<string, string>>();

            KeyValuePair<string, string> igo = new KeyValuePair<string, string>("IGO", "Indigo");
            KeyValuePair<string, string> mil = new KeyValuePair<string, string>("MIL", "Miles 33");
            KeyValuePair<string, string> e3e = new KeyValuePair<string, string>("E3E", "Elite 3E");
            KeyValuePair<string, string> c3e = new KeyValuePair<string, string>("C3E", "Elite 3E Cloud");
            KeyValuePair<string, string> ent = new KeyValuePair<string, string>("ENT", "Elite Enterprise");

            apps.Add(igo);
            apps.Add(mil);
            apps.Add(e3e);
            apps.Add(c3e);
            apps.Add(ent);
                        
            cboIntegrationApp.DataSource = apps;
            cboIntegrationApp.DisplayMember = "Value";
            cboIntegrationApp.ValueMember = "Key";
            cboIntegrationApp.SelectedValue = _exportApp;

            string apiRelativeUrlTransService2 = StaticLibrary.GetSetting(OMSExportBase.ApiRelativeUrlTransService2xKey, "", "");
            if(string.IsNullOrEmpty(apiRelativeUrlTransService2))
            {
                UpdateSetting(OMSExportBase.ApiRelativeUrlTransService2xKey, OMSExportBase.ApiRelativeUrlTransService2xDefault);
            }

            string apiRelativeUrlTransService3x = StaticLibrary.GetSetting(OMSExportBase.ApiRelativeUrlTransService3xKey, "", "");
            if (string.IsNullOrEmpty(apiRelativeUrlTransService3x))
            {
                UpdateSetting(OMSExportBase.ApiRelativeUrlTransService3xKey, OMSExportBase.ApiRelativeUrlTransService3xDefault);
            }

            string apiRelativeUrlOnPrem = StaticLibrary.GetSetting(OMSExportBase.ApiRelativeUrlOnPremKey, "", "");
            if (string.IsNullOrEmpty(apiRelativeUrlOnPrem))
            {
                UpdateSetting(OMSExportBase.ApiRelativeUrlOnPremKey, OMSExportBase.ApiRelativeUrlOnPremDefault);
            }

            string apiRelativeUrlCloud = StaticLibrary.GetSetting(OMSExportBase.ApiRelativeUrlCloudKey, "", "");
            if (string.IsNullOrEmpty(apiRelativeUrlCloud))
            {
                UpdateSetting(OMSExportBase.ApiRelativeUrlCloudKey, OMSExportBase.ApiRelativeUrlCloudDefault);
            }
    }	
		
		
		/// <summary>
		/// Populates export object controls with inital values
		/// </summary>
		private void GetExportObjectValues()
		{
			//  Hide all export object buttons first
            chkIGO.Visible = false;
            chkCMS.Visible = false;
            chkMIL.Visible = false;
            chkE3E.Visible = false;
            chkC3E.Visible = false;
            chkENT.Visible = false;
            
            chkCustomUpdateTime.Checked = StaticLibrary.GetBoolSetting("CustomUpdateTime", "", false);
            chkCustomUpdateFinancials.Checked = false;
            chkCustomUpdateExContacts.Checked = StaticLibrary.GetBoolSetting("CustomUpdateExContacts", "", false);
            chkCustomUpdateUpContacts.Checked = StaticLibrary.GetBoolSetting("CustomUpdateUpContacts", "", false);
            chkCustomUpdateExClients.Checked = StaticLibrary.GetBoolSetting("CustomUpdateExClients", "", false);
            chkCustomUpdateUpClients.Checked = StaticLibrary.GetBoolSetting("CustomUpdateUpClients", "", false);
            chkCustomUpdateExMatters.Checked = StaticLibrary.GetBoolSetting("CustomUpdateExMatters", "", false);
            chkCustomUpdateUpMatters.Checked = StaticLibrary.GetBoolSetting("CustomUpdateUpMatters", "", false);
            txtUpdateTime.Text = StaticLibrary.GetSetting("CustomUpdateTimeScript", "", "");
            txtUpdateFinancials.Text = StaticLibrary.GetSetting("CustomUpdateFinancialsScript", "", "");
            txtUpdateExContacts.Text = StaticLibrary.GetSetting("CustomUpdateExContactsScript", "", "");
            txtUpdateUpContacts.Text = StaticLibrary.GetSetting("CustomUpdateUpContactsScript", "", "");
            txtUpdateExClients.Text = StaticLibrary.GetSetting("CustomUpdateExClientsScript", "", "");
            txtUpdateUpClients.Text = StaticLibrary.GetSetting("CustomUpdateUpClientsScript", "", "");
            txtUpdateExMatters.Text = StaticLibrary.GetSetting("CustomUpdateExMattersScript", "", "");
            txtUpdateUpMatters.Text = StaticLibrary.GetSetting("CustomUpdateUpMattersScript", "", "");

            //  ##Others added as needed

            if (_exportApp == "IGO")
            {
                chkIGO.Visible = true;
                chkIGO.Left = 0;
                chkIGO.Top = 125;

                txtIGOServer.Text = StaticLibrary.GetSetting("IGOServer", "IGO", "");
                txtIGODatabase.Text = StaticLibrary.GetSetting("IGODatabase", "IGO","");
                string IGOloginType = StaticLibrary.GetSetting("IGOLoginType","IGO","NT");
                if (IGOloginType == "NT")
                {
                    chkIGOIntegrated.Checked = true;
                    txtIGOUsername.Enabled = false;
                    txtIGOPassword.Enabled = false;
                }
                else
                {
                    chkIGOIntegrated.Checked = false;
                    txtIGOUsername.Enabled = true;
                    txtIGOPassword.Enabled = true;
                    txtIGOUsername.Text = StaticLibrary.GetSetting("IGOSQLUID", "IGO", "");
                    txtIGOPassword.Text = StaticLibrary.GetSetting("IGOSQLPWD", "IGO", "");
                }

                //added for Indigo
                txtIGOCompanyNo.Text = StaticLibrary.GetSetting("CompanyNo", "IGO", "");
                txtIGOBranchNo.Text = StaticLibrary.GetSetting("BranchNo", "IGO", "");
                txtIGOComputer.Text = StaticLibrary.GetSetting("Computer", "IGO", "");
                txtIGOProgramName.Text = StaticLibrary.GetSetting("ProgramName", "IGO", "");
                txtIGOUserCode.Text = StaticLibrary.GetSetting("UserCode", "IGO", "");
                txtIGOMessage.Text = StaticLibrary.GetSetting("Message", "IGO", "");
                txtIGOVersion.Text = StaticLibrary.GetSetting("Version", "IGO", "");
            }

            //  Miles 33
            if (_exportApp == "MIL")
            {
                chkMIL.Visible = true;
                chkMIL.Left = 0;
                chkMIL.Top = 125;

                txtMILUsername.Enabled = true;
                txtMILPassword.Enabled = true;
                txtMILServer.Enabled = true;
                cboOrcaleConnectionType.Enabled = true;

                txtMILPassword.Text = StaticLibrary.GetSetting("MILPassword", "MIL", "");
                txtMILUsername.Text = StaticLibrary.GetSetting("MILLogin", "MIL", "");
                txtMILServer.Text = StaticLibrary.GetSetting("MILServer", "MIL", "");
                cboOrcaleConnectionType.SelectedItem = StaticLibrary.GetSetting("OrcaleConnectionType", "MIL", "Oracle 7 OLEDB");
            }

            //  Elite 3E
            if (_exportApp == "E3E")
            {
                chkE3E.Visible = true;
                chkE3E.Left = 0;
                chkE3E.Top = 125;

                txtE3EURL.Text = StaticLibrary.GetSetting("BaseURL", "E3E", "");
                chkE3ENewEndpoint.Checked = StaticLibrary.GetBoolSetting("NewEndpoint", "E3E", false);
                chkE3EDebug.Checked = StaticLibrary.GetBoolSetting("Debug", "E3E", false);
                txtE3ESuccessLocation.Text = StaticLibrary.GetSetting("SuccessLocation", "E3E", "");
                txtE3EFailedLocation.Text = StaticLibrary.GetSetting("FailedLocation", "E3E", "");
                chkE3ECancelProcess.Checked = StaticLibrary.GetBoolSetting("CancelProcess", "E3E", false);
                chkE3EMatterConflictCheck.Checked = StaticLibrary.GetBoolSetting("MatterConflictCheck", "E3E", true);
                chkE3EResetFlagInCaseOfFailure.Checked = StaticLibrary.GetBoolSetting("ResetFlagInCaseOfFailure", "E3E", false);
            }

            //  Elite 3E Cloud
            if (_exportApp == "C3E")
            {
                chkC3E.Visible = true;
                chkC3E.Left = 0;
                chkC3E.Top = 125;

                txtC3EURL.Text = StaticLibrary.GetSetting("BaseURL", "C3E", "");
                chkC3EDebug.Checked = StaticLibrary.GetBoolSetting("Debug", "C3E", false);
                txtC3ESuccessLocation.Text = StaticLibrary.GetSetting("SuccessLocation", "C3E", "");
                txtC3EFailedLocation.Text = StaticLibrary.GetSetting("FailedLocation", "C3E", "");
                chkC3EMatterConflictCheck.Checked = StaticLibrary.GetBoolSetting("MatterConflictCheck", "C3E", false);
                chkC3EResetFlagInCaseOfFailure.Checked = StaticLibrary.GetBoolSetting("ResetFlagInCaseOfFailure", "C3E", false);
                chkC3EImportInvoices.Checked = StaticLibrary.GetBoolSetting("ImportInvoices", "C3E", false);
                chkC3EAzureADAuth.Checked = false;
                if (!string.IsNullOrWhiteSpace(StaticLibrary.GetSetting("AADEnv", "C3E", "")))
                    chkC3EAzureADAuth.Checked = true;
                try
                {
                    string[] value = System.Text.Encoding.UTF8.GetString(
                        EncryptionV2.Decrypt(
                            Convert.FromBase64String(
                                StaticLibrary.GetSetting("AADConfig", "C3E", "")), string.Concat(Environment.MachineName, ":", "AAD"))
                        ).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    if (value.Length > 0) { txtC3EAzureADAudience.Text = value[0]; }
                    if (value.Length > 1) { txtC3EAzureADClientId.Text = value[1]; }
                    if (value.Length > 2) { txtC3EAzureADClientSecret.Text = value[2]; }
                    if (value.Length > 3) { txtC3EAzureADTenantId.Text = value[3]; }
                    if (value.Length > 4) { txtC3EAzureADInstanceId.Text = value[4]; }
                }
                catch { }
            }

            //  Elite Enterprise
            if (_exportApp == "ENT")
            {
                chkENT.Visible = true;
                chkENT.Left = 0;
                chkENT.Top = 125;

                txtEnterpriseBaseURL.Text = StaticLibrary.GetSetting("BaseURL", _exportApp, "");
                txtEnterpriseServiceUserID.Text = StaticLibrary.GetSetting("ServiceUserID", _exportApp, "");
                txtEnterpriseDateFormat.Text = StaticLibrary.GetSetting("DateFormat", _exportApp, "");
            }
		}
	
		/// <summary>
		/// Update registry setting
		/// </summary>
		/// <param name="setting">Setting name</param>
		/// <param name="newValue">value of setting</param>
		private void UpdateSetting(string setting,object newValue)
		{
			UpdateSetting(setting,"Settings",newValue);
		}
		
		/// <summary>
		/// Updates registry setting
		/// </summary>
		/// <param name="setting">Settings name</param>
		/// <param name="app">application name</param>
		/// <param name="newValue">value of setting</param>
		private void UpdateSetting(string setting,string app,object newValue)
		{
			FWBS.OMS.OMSEXPORT.StaticLibrary.UpdateSetting(setting,app,newValue);
		}
		
		/// <summary>
		/// Updates the registry with the new values
		/// </summary>
		/// <param name="updateAppSettings">Indicates whether to update export application settings</param>
		///<returns>boolean</returns>
		private bool UpdateValues(bool updateAppSettings = true)
		{
			try
			{
				//export application
                _exportApp = Convert.ToString(cboIntegrationApp.SelectedValue);
                StaticLibrary.SetExportAppName(_exportApp);

                int threshold = 20;
                int.TryParse(txtExceptionThreshold.Text, out threshold);
                UpdateSetting("ExceptionThreashold", threshold);
                                
                //capture new values
				UpdateSetting("DetailedLogging",chkFullEventLog.Checked);
                UpdateSetting("LogToDB", chkLogToDatabase.Checked);
				UpdateSetting("StatLogFolder",txtFilePath.Text);
				UpdateSetting("OMSSQLServer",txtServerName.Text);
				UpdateSetting("OMSSQLDatabase",txtDatabaseName.Text);
				UpdateSetting("PauseInterval",txtPauseMins.Text);
				UpdateSetting("EmailErrors",chkEmailAdmin.Checked);

                UpdateSetting("LogDays", txtLogDays.Text);
    

                //export options
                UpdateSetting("ExportUsers",chkExportUsers.Checked);
                UpdateSetting("ExportContacts", chkExportContacts.Checked);
                UpdateSetting("UpdateContacts", chkUpdateContacts.Checked);
                UpdateSetting("ExportClients",chkExportClients.Checked);
                UpdateSetting("UpdateClients",chkUpdateClients.Checked);
                UpdateSetting("ExportMatters",chkExportMatters.Checked);
                UpdateSetting("UpdateMatters",chkUpdateMatters.Checked);
                UpdateSetting("ExportTime",chkExportTime.Checked);
                UpdateSetting("ExportFinancials", chkExportFinancials.Checked);
                UpdateSetting("ExportLookups", chkExportLookups.Checked);

                UpdateSetting("CustomUpdateTime", chkCustomUpdateTime.Checked);
                
                if (chkCustomUpdateTime.Checked)
                {
                    UpdateSetting("CustomUpdateTimeScript", txtUpdateTime.Text);
                }

                UpdateSetting("CustomUpdateFinancials", chkCustomUpdateFinancials.Checked);
                
                if (chkCustomUpdateFinancials.Checked)
                {
                    UpdateSetting("CustomUpdateFinancialsScript", txtUpdateFinancials.Text);
                }

                UpdateSetting("CustomUpdateExContacts", chkCustomUpdateExContacts.Checked);

                if (chkCustomUpdateExContacts.Checked)
                {
                    UpdateSetting("CustomUpdateExContactsScript", txtUpdateExContacts.Text);
                }

                UpdateSetting("CustomUpdateUpContacts", chkCustomUpdateUpContacts.Checked);

                if (chkCustomUpdateUpContacts.Checked)
                {
                    UpdateSetting("CustomUpdateUpContactsScript", txtUpdateUpContacts.Text);
                }

                UpdateSetting("CustomUpdateExClients", chkCustomUpdateExClients.Checked);
                
                if (chkCustomUpdateExClients.Checked)
                {
                    UpdateSetting("CustomUpdateExClientsScript", txtUpdateExClients.Text);
                }

                UpdateSetting("CustomUpdateUpClients", chkCustomUpdateUpClients.Checked);
                
                if (chkCustomUpdateUpClients.Checked)
                {
                    UpdateSetting("CustomUpdateUpClientsScript", txtUpdateUpClients.Text);
                }

                UpdateSetting("CustomUpdateExMatters", chkCustomUpdateExMatters.Checked);
                
                if (chkCustomUpdateExMatters.Checked)
                {
                    UpdateSetting("CustomUpdateExMattersScript", txtUpdateExMatters.Text);
                }

                UpdateSetting("CustomUpdateUpMatters", chkCustomUpdateUpMatters.Checked);

                if (chkCustomUpdateUpMatters.Checked)
                {
                    UpdateSetting("CustomUpdateUpMattersScript", txtUpdateUpMatters.Text);
                }


                if(chkEmailAdmin.Checked)
				{
					UpdateSetting("EmailAddress",txtEmailAddress.Text);
                    UpdateSetting("EmailFrom", txtEmailFrom.Text);
                    UpdateSetting("SmtpServer", txtSmtpServer.Text);
                    UpdateSetting("SmtpEncryption", cmbSmtpEncryption.SelectedItem as string);
                    UpdateSetting("SmtpAuthenticate", chkSmtpAuthenticate.Checked);
                    UpdateSetting("SmtpLogin", txtSmtpLogin.Text);
                    UpdateSetting("SmtpPassword", txtSmtpPassword.Text);
                }
				
				if(chkIntegrated.Checked)
				{
					UpdateSetting("OMSLoginType", chkAzure.Checked ? "AAD" : "NT");
				}
				else
				{
					UpdateSetting("OMSLoginType","SQL");
					UpdateSetting("OMSSQLUID",txtUserName.Text);
					UpdateSetting("OMSSQLPWD",txtPassword.Text);
				}

                //  Update Tracking Column type
                UpdateSetting("TrackingColumnType", cboTrackingColumn.SelectedItem);

                if (!updateAppSettings)
                    return true;

                if (_exportApp == "IGO")
                {
                    UpdateSetting("IGOServer", "IGO", txtIGOServer.Text);
                    UpdateSetting("IGODatabase", "IGO", txtIGODatabase.Text);
                    UpdateSetting("CompanyNo", "IGO", txtIGOCompanyNo.Text);
                    UpdateSetting("BranchNo", "IGO", txtIGOBranchNo.Text);
                    UpdateSetting("Computer", "IGO", txtIGOComputer.Text);
                    UpdateSetting("ProgramName", "IGO",txtIGOProgramName.Text);
                    UpdateSetting("userCode", "IGO", txtIGOUserCode.Text);
                    UpdateSetting("Message", "IGO", txtIGOMessage.Text);
                    UpdateSetting("Version", "IGO", txtIGOVersion.Text);
                    if (chkIGOIntegrated.Checked)
                        UpdateSetting("IGOLoginType","IGO", "NT");
                    else
                    {
                        UpdateSetting("IGOLoginType","IGO", "SQL");
                        UpdateSetting("IGOSQLUID","IGO", txtIGOUsername.Text);
                        UpdateSetting("IGOSQLPWD","IGO", txtIGOPassword.Text);
                    }
                }

                //  Miles 33
                if (_exportApp == "MIL")
                {
                    UpdateSetting("MILPassword", "MIL", txtMILPassword.Text);
                    UpdateSetting("MILLogin", "MIL", txtMILUsername.Text);
                    UpdateSetting("MILServer", "MIL", txtMILServer.Text);

                    if (cboOrcaleConnectionType.SelectedItem == null)
                    {
                        cboOrcaleConnectionType.SelectedItem = StaticLibrary.GetSetting("OrcaleConnectionType", "MIL", "Oracle 7 OLEDB");
                    }
                    UpdateSetting("OrcaleConnectionType", "MIL", cboOrcaleConnectionType.SelectedItem);
                }

                // Elite 3E
                if (_exportApp == "E3E")
                {
                    UpdateSetting("BaseURL", "E3E", txtE3EURL.Text);
                    UpdateSetting("NewEndpoint", "E3E", chkE3ENewEndpoint.Checked);
                    UpdateSetting("Debug", "E3E", chkE3EDebug.Checked);
                    UpdateSetting("SuccessLocation", "E3E", txtE3ESuccessLocation.Text); 
                    UpdateSetting("FailedLocation", "E3E", txtE3EFailedLocation.Text);
                    UpdateSetting("CancelProcess", "E3E", chkE3ECancelProcess.Checked);
                    UpdateSetting("MatterConflictCheck", "E3E", chkE3EMatterConflictCheck.Checked);
                    UpdateSetting("ResetFlagInCaseOfFailure", "E3E", chkE3EResetFlagInCaseOfFailure.Checked);
                }

                // Elite 3E Cloud
                if (_exportApp == "C3E")
                {
                    UpdateSetting("BaseURL", "C3E", txtC3EURL.Text);
                    UpdateSetting("Debug", "C3E", chkC3EDebug.Checked);
                    UpdateSetting("SuccessLocation", "C3E", txtC3ESuccessLocation.Text);
                    UpdateSetting("FailedLocation", "C3E", txtC3EFailedLocation.Text);
                    UpdateSetting("MatterConflictCheck", "C3E", chkC3EMatterConflictCheck.Checked);
                    UpdateSetting("ResetFlagInCaseOfFailure", "C3E", chkC3EResetFlagInCaseOfFailure.Checked);
                    UpdateSetting("ImportInvoices", "C3E", chkC3EImportInvoices.Checked);
                    if (chkC3EAzureADAuth.Checked)
                    {
                        string env = StaticLibrary.GetSetting("AADEnv", "C3E", "Prod");
                        if (string.IsNullOrWhiteSpace(env))
                        {
                            UpdateSetting("AADEnv", "C3E", "Prod");
                        }

                        string conf = string.Concat(
                            txtC3EAzureADAudience.Text.Trim(), " ", 
                            txtC3EAzureADClientId.Text.Trim(), " ", 
                            txtC3EAzureADClientSecret.Text.Trim(), " ",
                            txtC3EAzureADTenantId.Text.Trim(), " ",
                            txtC3EAzureADInstanceId.Text.Trim()).Trim();
                        if (!string.IsNullOrEmpty(conf))
                        {
                            conf = Convert.ToBase64String(EncryptionV2.Encrypt(System.Text.Encoding.UTF8.GetBytes(conf), string.Concat(Environment.MachineName, ":", "AAD")));
                        }
                        UpdateSetting("AADConfig", "C3E", conf);
                    }
                    else
                    {
                        UpdateSetting("AADEnv", "C3E", "");
                    }
                }

                // Elite 3E
                if (_exportApp == "ENT")
                {
                    UpdateSetting("BaseURL", _exportApp, txtEnterpriseBaseURL.Text);
                    UpdateSetting("ServiceUserID", _exportApp, txtEnterpriseServiceUserID.Text);
                    UpdateSetting("DateFormat", _exportApp, txtEnterpriseDateFormat.Text);
                }

				return true;
						
			}
			catch(Exception ex)
			{
				MessageBox.Show(this,"Error saving values: " + ex.Message,DIALOG_CAPTION);
				return false;
			}
		}

		private void chkIntegrated_CheckedChanged(object sender, System.EventArgs e)
		{
			txtUserName.Enabled = !chkIntegrated.Checked;
			txtPassword.Enabled = !chkIntegrated.Checked;
			chkAzure.Visible = chkIntegrated.Checked;
			chkAzure.Checked = false;
		}


        /// <summary>
        /// Global handler for all the selection buttons
        /// </summary>
        /// <param name="sender">the button that was clicked</param>
        /// <param name="e"></param>
        private void chk_Click(object sender, EventArgs e)
        {
            string name = ((CheckBox)sender).Name;

            //set everything to origional position
            chkCommon.Checked = false;
            chkExports.Checked = false;
            chkCMS.Checked = false;
            chkIGO.Checked = false;
            chkMIL.Checked = false;
            chkE3E.Checked = false;
            chkC3E.Checked = false;
            chkENT.Checked = false;
            grp1.Left = 1000;
            grp2.Left = 1000;
            grp3.Left = 1000;
            grp4.Left = 1000;
            grpCMS.Left = 1000;
            grpIGO.Left = 1000;
            grpMIL.Left = 1000;
            grpE3E.Left = 1000;
            grpC3E.Left = 1000;
            grpENT.Left = 1000;

            grpExports.Left = 1000;

            switch (name)
            {
                case "chkCMS":
                    grpCMS.Left = 85;
                    chkCMS.Checked = true;
                    break;
                case "chkIGO":
                    grpIGO.Left = 85;
                    chkIGO.Checked = true;
                    break;
                case "chkMIL":
                    grpMIL.Left = 85;
                    chkMIL.Checked = true;
                    break;
                case "chkCommon":
                    grp1.Left = 85;
                    grp2.Left = 85;
                    grp3.Left = 85;
                    grp4.Left = 85;                    
                    chkCommon.Checked = true;
                    break;
                case "chkE3E":
                    grpE3E.Left = 85;
                    chkE3E.Checked = true;
                    break;
                case "chkC3E":
                    grpC3E.Left = 85;
                    chkC3E.Checked = true;
                    break;
                case "chkENT":
                    grpENT.Left = 85;
                    chkENT.Checked = true;
                    break;
                default: //chkExports
                    grpExports.Left = 85;
                    chkExports.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// Disables text boxes if integrated is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void chkEmailAdmin_CheckedChanged(object sender, System.EventArgs e)
		{
            bool enabled = chkEmailAdmin.Checked;
			txtEmailAddress.Enabled = enabled;
            txtEmailFrom.Enabled = enabled;
			txtSmtpServer.Enabled= enabled;
            cmbSmtpEncryption.Enabled = enabled;
            chkSmtpAuthenticate.Enabled = enabled;
            txtSmtpLogin.Enabled = enabled;
            txtSmtpPassword.Enabled = enabled;
		}

        /// <summary>
        /// Disables text boxes if integrated is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		private void chkCMSIntegrated_CheckedChanged(object sender, System.EventArgs e)
		{
			if(chkCMSIntegrated.Checked)
			{
				txtCMSUsername.Enabled = false;
				txtCMSPassword.Enabled = false;
			}
			else
			{
				txtCMSUsername.Enabled = true;
				txtCMSPassword.Enabled = true;
			}
		}

        /// <summary>
        /// Disables text boxes if integrated is checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkIGOIntegrated_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIGOIntegrated.Checked)
            {
                txtIGOUsername.Enabled = false;
                txtIGOPassword.Enabled = false;
            }
            else
            {
                txtIGOUsername.Enabled = true;
                txtIGOPassword.Enabled = true;
            }
        }

		/// <summary>
		/// Generates SQL string from values within the registry
		/// </summary>
		private string SQLConnectionString
		{
			get
			{
				if (txtDatabaseName.Text == "" || txtServerName.Text == "" || (txtUserName.Text == "" & txtUserName.Enabled))
					throw new Exception("Database setting are not configured");

				string strCnn = "Server=" + txtServerName.Text + ";Database=" + txtDatabaseName.Text;
				if (chkIntegrated.Checked)
				{
					strCnn += chkAzure.Checked ? ";Authentication=Active Directory Integrated" : ";Integrated Security=SSPI";
					strCnn += ";Persist Security Info = False";
				}
				else
				{
					strCnn += ";User Id=" + txtUserName.Text + ";Password=" + txtPassword.Text + ";Trusted_Connection=False";
				}
				return strCnn;
			}
		}

        private string IGOConnectionString
        {
            get
            {
                string strCnn = "";

                if (txtIGODatabase.Text == "" || txtIGOServer.Text == "" || ( txtIGOUsername.Text == "" & txtIGOUsername.Enabled))
                    throw new Exception("Database setting are not configured");

                if (chkIGOIntegrated.Checked)
                    strCnn = "Persist Security Info=False;Integrated Security=SSPI;database=" + txtIGODatabase.Text + ";server=" + txtIGOServer.Text;
                else
                    strCnn = "Server=" + txtIGOServer.Text + ";Database=" + txtIGODatabase.Text + ";User Id=" + txtIGOUsername.Text + ";Password=" + txtIGOPassword.Text + ";Trusted_Connection=False";

                return strCnn;
            }


        }


		#endregion


        private void chkCustomUpdateFinancials_CheckStateChanged(object sender, EventArgs e)
        {
            txtUpdateFinancials.Visible = chkCustomUpdateFinancials.Checked;
        }

        private void chkCustomUpdateTime_CheckStateChanged(object sender, EventArgs e)
        {
            txtUpdateTime.Visible = chkCustomUpdateTime.Checked;
        }

        private void chkCustomUpdateExContacts_CheckStateChanged(object sender, EventArgs e)
        {
            txtUpdateExContacts.Visible = chkCustomUpdateExContacts.Checked;
        }

        private void chkCustomUpdateUpContacts_CheckStateChanged(object sender, EventArgs e)
        {
            txtUpdateUpContacts.Visible = chkCustomUpdateUpContacts.Checked;
        }

        private void chkCustomUpdateExClients_CheckStateChanged(object sender, EventArgs e)
        {
            txtUpdateExClients.Visible = chkCustomUpdateExClients.Checked;
        }

        private void chkCustomUpdateUpClients_CheckStateChanged(object sender, EventArgs e)
        {
            txtUpdateUpClients.Visible = chkCustomUpdateUpClients.Checked;
        }

        private void chkCustomUpdateExMatters_CheckStateChanged(object sender, EventArgs e)
        {
            txtUpdateExMatters.Visible = chkCustomUpdateExMatters.Checked;
        }

        private void chkCustomUpdateUpMatters_CheckStateChanged(object sender, EventArgs e)
        {
            txtUpdateUpMatters.Visible = chkCustomUpdateUpMatters.Checked;
        }

        private void cboIntegrationApp_SelectedValueChanged(object sender, EventArgs e)
        {
        }

        private void cboIntegrationApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (active)
            {
                UpdateValues(false);
                GetExportObjectValues();
            }
        }
        bool active = false;
        private void cboIntegrationApp_Enter(object sender, EventArgs e)
        {
            active = true;
        }

        private void cboIntegrationApp_Leave(object sender, EventArgs e)
        {
            active = false;
        }

        private void chkMIL_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkCommon_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkC3EAzureADAuth_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = ((CheckBox)sender).Checked;
            lblC3EAzureADAudience.Visible = isChecked;
            txtC3EAzureADAudience.Visible = isChecked;
            lblC3EAzureADClientId.Visible = isChecked;
            txtC3EAzureADClientId.Visible = isChecked;
            lblC3EAzureADClientSecret.Visible = isChecked;
            txtC3EAzureADClientSecret.Visible = isChecked;
            lblC3EAzureADTenantId.Visible = isChecked;
            txtC3EAzureADTenantId.Visible = isChecked;
            lblC3EAzureADInstanceId.Visible = isChecked;
            txtC3EAzureADInstanceId.Visible = isChecked;
        }
    }
}
