using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.Data;


namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A simple edit form which updates multi database connection settings.
    /// </summary>
    internal class frmConnections : System.Windows.Forms.Form
	{
		#region Fields

		private System.Windows.Forms.Button cmdUpdate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.Label lblDefPassword;
		private System.Windows.Forms.Label lblDefUser;
		private System.Windows.Forms.Label lblAuthentication;
		private System.Windows.Forms.TextBox txtDefPassword;
		private System.Windows.Forms.TextBox txtDefUser;
		private System.Windows.Forms.ComboBox cboAuthentication;
		private System.Windows.Forms.Label lblDatabase;
		private System.Windows.Forms.Label lblServer;
		private System.Windows.Forms.TextBox txtDatabase;
		private System.Windows.Forms.TextBox txtServer;

		/// <summary>
		/// Currently worked on multidb object.
		/// </summary>
		private DatabaseConnections _cnn;
		private DatabaseSettings _db;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnOpen;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Button btnSaveAs;
		private System.Windows.Forms.ComboBox cboMultiDB;
		private System.Windows.Forms.Label lblSetting;
		private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkUseAppRoles;
        private System.Windows.Forms.TextBox txtAppRoleName;
        private System.Windows.Forms.TextBox txtAppRolePassword;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblAppRoleName;
        private System.Windows.Forms.Label lblAppRolePassword;

		#endregion
        private ResourceLookup resourceLookup1;
        private Panel panel1;
        private CheckBox chkKeepOpen;
        private TextBox txtServiceLocation;
        private Label label2;
        private GroupBox groupBox3;
        private CheckBox chkUseNoLock;
        private IContainer components;

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor of the form.
		/// </summary>
		private frmConnections()
		{
            this.Font = new Font("Calibri", 9);
            this.BackColor = Color.White;

            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
			Global.RightToLeftFormConverter(this);
		}

		/// <summary>
		/// Creates an instance of this form and sets all the default multidb settings to the
		/// relevant editable boxes.
		/// </summary>
		/// <param name="db">Multi databse object to retrieve information.</param>
		public frmConnections(DatabaseConnections cnn, DatabaseSettings db) : this()
		{
			_cnn = cnn;
			_db = db;
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
            this.cmdUpdate = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDefPassword = new System.Windows.Forms.Label();
            this.lblDefUser = new System.Windows.Forms.Label();
            this.lblAuthentication = new System.Windows.Forms.Label();
            this.txtDefPassword = new System.Windows.Forms.TextBox();
            this.txtDefUser = new System.Windows.Forms.TextBox();
            this.cboAuthentication = new System.Windows.Forms.ComboBox();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.lblServer = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.txtServer = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.cboMultiDB = new System.Windows.Forms.ComboBox();
            this.lblSetting = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkUseAppRoles = new System.Windows.Forms.CheckBox();
            this.txtAppRoleName = new System.Windows.Forms.TextBox();
            this.txtAppRolePassword = new System.Windows.Forms.TextBox();
            this.lblAppRoleName = new System.Windows.Forms.Label();
            this.lblAppRolePassword = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkKeepOpen = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkUseNoLock = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtServiceLocation = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdUpdate
            // 
            this.cmdUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdUpdate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmdUpdate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cmdUpdate.Location = new System.Drawing.Point(7, 7);
            this.resourceLookup1.SetLookup(this.cmdUpdate, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmdUpdate", "&Save", ""));
            this.cmdUpdate.Name = "cmdUpdate";
            this.cmdUpdate.Size = new System.Drawing.Size(75, 23);
            this.cmdUpdate.TabIndex = 8;
            this.cmdUpdate.Text = "&Save";
            this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(8, 43);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblDesc", "Description", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 20);
            this.label1.TabIndex = 52;
            this.label1.Text = "Description";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(114, 42);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(208, 20);
            this.txtDescription.TabIndex = 0;
            this.txtDescription.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // lblDefPassword
            // 
            this.lblDefPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblDefPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDefPassword.Location = new System.Drawing.Point(8, 158);
            this.resourceLookup1.SetLookup(this.lblDefPassword, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblDefPassword", "Password", ""));
            this.lblDefPassword.Name = "lblDefPassword";
            this.lblDefPassword.Size = new System.Drawing.Size(98, 20);
            this.lblDefPassword.TabIndex = 51;
            this.lblDefPassword.Text = "Password";
            this.lblDefPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDefUser
            // 
            this.lblDefUser.BackColor = System.Drawing.Color.Transparent;
            this.lblDefUser.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDefUser.Location = new System.Drawing.Point(8, 135);
            this.resourceLookup1.SetLookup(this.lblDefUser, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblDefUser", "Login", ""));
            this.lblDefUser.Name = "lblDefUser";
            this.lblDefUser.Size = new System.Drawing.Size(98, 20);
            this.lblDefUser.TabIndex = 50;
            this.lblDefUser.Text = "Login";
            this.lblDefUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAuthentication
            // 
            this.lblAuthentication.BackColor = System.Drawing.Color.Transparent;
            this.lblAuthentication.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblAuthentication.Location = new System.Drawing.Point(8, 66);
            this.resourceLookup1.SetLookup(this.lblAuthentication, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblAuth", "Authentication", ""));
            this.lblAuthentication.Name = "lblAuthentication";
            this.lblAuthentication.Size = new System.Drawing.Size(98, 20);
            this.lblAuthentication.TabIndex = 49;
            this.lblAuthentication.Text = "Authentication";
            this.lblAuthentication.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDefPassword
            // 
            this.txtDefPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtDefPassword.Location = new System.Drawing.Point(114, 158);
            this.txtDefPassword.MaxLength = 30;
            this.txtDefPassword.Name = "txtDefPassword";
            this.txtDefPassword.PasswordChar = '*';
            this.txtDefPassword.Size = new System.Drawing.Size(270, 20);
            this.txtDefPassword.TabIndex = 5;
            this.txtDefPassword.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // txtDefUser
            // 
            this.txtDefUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDefUser.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtDefUser.Location = new System.Drawing.Point(114, 135);
            this.txtDefUser.MaxLength = 30;
            this.txtDefUser.Name = "txtDefUser";
            this.txtDefUser.PasswordChar = '*';
            this.txtDefUser.Size = new System.Drawing.Size(270, 20);
            this.txtDefUser.TabIndex = 4;
            this.txtDefUser.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // cboAuthentication
            // 
            this.cboAuthentication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboAuthentication.Items.AddRange(new object[] {
            "OMS",
            "SQL",
            "NT",
            "AAD"});
            this.cboAuthentication.Location = new System.Drawing.Point(114, 65);
            this.cboAuthentication.Name = "cboAuthentication";
            this.cboAuthentication.Size = new System.Drawing.Size(152, 21);
            this.cboAuthentication.TabIndex = 1;
            this.cboAuthentication.SelectedIndexChanged += new System.EventHandler(this.cboAuthentication_SelectedIndexChanged);
            this.cboAuthentication.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // lblDatabase
            // 
            this.lblDatabase.BackColor = System.Drawing.Color.Transparent;
            this.lblDatabase.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDatabase.Location = new System.Drawing.Point(8, 112);
            this.resourceLookup1.SetLookup(this.lblDatabase, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblDatabase", "Database", ""));
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(98, 20);
            this.lblDatabase.TabIndex = 47;
            this.lblDatabase.Text = "Database";
            this.lblDatabase.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblServer
            // 
            this.lblServer.BackColor = System.Drawing.Color.Transparent;
            this.lblServer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblServer.Location = new System.Drawing.Point(8, 89);
            this.resourceLookup1.SetLookup(this.lblServer, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblServer", "Server", ""));
            this.lblServer.Name = "lblServer";
            this.lblServer.Size = new System.Drawing.Size(98, 20);
            this.lblServer.TabIndex = 46;
            this.lblServer.Text = "Server";
            this.lblServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtDatabase
            // 
            this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabase.Location = new System.Drawing.Point(114, 112);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(270, 20);
            this.txtDatabase.TabIndex = 3;
            this.txtDatabase.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // txtServer
            // 
            this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServer.Location = new System.Drawing.Point(114, 89);
            this.txtServer.Name = "txtServer";
            this.txtServer.Size = new System.Drawing.Size(270, 20);
            this.txtServer.TabIndex = 2;
            this.txtServer.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClose.Location = new System.Drawing.Point(7, 85);
            this.resourceLookup1.SetLookup(this.btnClose, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnClose", "&Close", ""));
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "&Close";
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdd.Location = new System.Drawing.Point(330, 42);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(24, 20);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "+";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRemove.Location = new System.Drawing.Point(354, 42);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(24, 20);
            this.btnRemove.TabIndex = 7;
            this.btnRemove.Text = "-";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOpen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOpen.Location = new System.Drawing.Point(7, 33);
            this.resourceLookup1.SetLookup(this.btnOpen, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnOpen", "&Open ...", ""));
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 9;
            this.btnOpen.Text = "&Open ...";
            this.btnOpen.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "xml";
            this.openFileDialog1.Filter = "Multi Database Files|*.xml";
            this.openFileDialog1.Title = "Open Multi Database Settings";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xml";
            this.saveFileDialog1.Filter = "Multi Database Files|*.xml";
            this.saveFileDialog1.Title = "Save Multi Database Settings";
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnSaveAs.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSaveAs.Location = new System.Drawing.Point(7, 59);
            this.resourceLookup1.SetLookup(this.btnSaveAs, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnSaveAs", "Save &As ...", ""));
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(75, 23);
            this.btnSaveAs.TabIndex = 10;
            this.btnSaveAs.Text = "Save &As ...";
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // cboMultiDB
            // 
            this.cboMultiDB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboMultiDB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMultiDB.Location = new System.Drawing.Point(114, 9);
            this.cboMultiDB.Name = "cboMultiDB";
            this.cboMultiDB.Size = new System.Drawing.Size(270, 21);
            this.cboMultiDB.TabIndex = 53;
            this.cboMultiDB.SelectedIndexChanged += new System.EventHandler(this.cboMultiDB_SelectedIndexChanged);
            // 
            // lblSetting
            // 
            this.lblSetting.BackColor = System.Drawing.Color.Transparent;
            this.lblSetting.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSetting.Location = new System.Drawing.Point(8, 10);
            this.resourceLookup1.SetLookup(this.lblSetting, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblSetting", "Setting", ""));
            this.lblSetting.Name = "lblSetting";
            this.lblSetting.Size = new System.Drawing.Size(98, 20);
            this.lblSetting.TabIndex = 55;
            this.lblSetting.Text = "Setting";
            this.lblSetting.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Location = new System.Drawing.Point(8, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(375, 3);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            // 
            // chkUseAppRoles
            // 
            this.chkUseAppRoles.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkUseAppRoles.Location = new System.Drawing.Point(8, 222);
            this.resourceLookup1.SetLookup(this.chkUseAppRoles, new FWBS.OMS.UI.Windows.ResourceLookupItem("chkUseAppRoles", "Use Application Role", ""));
            this.chkUseAppRoles.Name = "chkUseAppRoles";
            this.chkUseAppRoles.Size = new System.Drawing.Size(156, 24);
            this.chkUseAppRoles.TabIndex = 9;
            this.chkUseAppRoles.Text = "Use Application Role";
            this.chkUseAppRoles.CheckedChanged += new System.EventHandler(this.chkUseAppRoles_CheckedChanged);
            this.chkUseAppRoles.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // txtAppRoleName
            // 
            this.txtAppRoleName.Enabled = false;
            this.txtAppRoleName.Location = new System.Drawing.Point(114, 249);
            this.txtAppRoleName.Name = "txtAppRoleName";
            this.txtAppRoleName.PasswordChar = '*';
            this.txtAppRoleName.Size = new System.Drawing.Size(269, 20);
            this.txtAppRoleName.TabIndex = 10;
            this.txtAppRoleName.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // txtAppRolePassword
            // 
            this.txtAppRolePassword.Enabled = false;
            this.txtAppRolePassword.Location = new System.Drawing.Point(114, 275);
            this.txtAppRolePassword.Name = "txtAppRolePassword";
            this.txtAppRolePassword.PasswordChar = '*';
            this.txtAppRolePassword.Size = new System.Drawing.Size(269, 20);
            this.txtAppRolePassword.TabIndex = 11;
            this.txtAppRolePassword.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // lblAppRoleName
            // 
            this.lblAppRoleName.BackColor = System.Drawing.Color.Transparent;
            this.lblAppRoleName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblAppRoleName.Location = new System.Drawing.Point(8, 249);
            this.resourceLookup1.SetLookup(this.lblAppRoleName, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblAppRoleName", "Role Name", ""));
            this.lblAppRoleName.Name = "lblAppRoleName";
            this.lblAppRoleName.Size = new System.Drawing.Size(98, 20);
            this.lblAppRoleName.TabIndex = 60;
            this.lblAppRoleName.Text = "Role Name";
            this.lblAppRoleName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblAppRolePassword
            // 
            this.lblAppRolePassword.BackColor = System.Drawing.Color.Transparent;
            this.lblAppRolePassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblAppRolePassword.Location = new System.Drawing.Point(8, 275);
            this.resourceLookup1.SetLookup(this.lblAppRolePassword, new FWBS.OMS.UI.Windows.ResourceLookupItem("lblAppRolePass", "Password", ""));
            this.lblAppRolePassword.Name = "lblAppRolePassword";
            this.lblAppRolePassword.Size = new System.Drawing.Size(98, 20);
            this.lblAppRolePassword.TabIndex = 61;
            this.lblAppRolePassword.Text = "Password";
            this.lblAppRolePassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Location = new System.Drawing.Point(8, 213);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(375, 3);
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            // 
            // chkKeepOpen
            // 
            this.chkKeepOpen.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkKeepOpen.Location = new System.Drawing.Point(8, 184);
            this.resourceLookup1.SetLookup(this.chkKeepOpen, new FWBS.OMS.UI.Windows.ResourceLookupItem("KEEPCNNOPEN", "Keep Connection Open", ""));
            this.chkKeepOpen.Name = "chkKeepOpen";
            this.chkKeepOpen.Size = new System.Drawing.Size(156, 24);
            this.chkKeepOpen.TabIndex = 6;
            this.chkKeepOpen.Text = "Keep Connection Open";
            this.chkKeepOpen.Leave += new System.EventHandler(this.txtDescription_Leave);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(7, 323);
            this.resourceLookup1.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("SERVICELOCATION", "Service Location", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 20);
            this.label2.TabIndex = 64;
            this.label2.Text = "Service Location";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkUseNoLock
            // 
            this.chkUseNoLock.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkUseNoLock.Location = new System.Drawing.Point(183, 184);
            this.resourceLookup1.SetLookup(this.chkUseNoLock, new FWBS.OMS.UI.Windows.ResourceLookupItem("USENOLOCK", "Use No Lock", ""));
            this.chkUseNoLock.Name = "chkUseNoLock";
            this.chkUseNoLock.Size = new System.Drawing.Size(156, 24);
            this.chkUseNoLock.TabIndex = 8;
            this.chkUseNoLock.Text = "Use No Lock";
            this.chkUseNoLock.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.cmdUpdate);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Controls.Add(this.btnSaveAs);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(390, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(88, 353);
            this.panel1.TabIndex = 62;
            // 
            // txtServiceLocation
            // 
            this.txtServiceLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtServiceLocation.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtServiceLocation.Location = new System.Drawing.Point(113, 323);
            this.txtServiceLocation.MaxLength = 30;
            this.txtServiceLocation.Name = "txtServiceLocation";
            this.txtServiceLocation.PasswordChar = '*';
            this.txtServiceLocation.Size = new System.Drawing.Size(270, 20);
            this.txtServiceLocation.TabIndex = 12;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Location = new System.Drawing.Point(11, 309);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(375, 3);
            this.groupBox3.TabIndex = 65;
            this.groupBox3.TabStop = false;
            // 
            // frmConnections
            // 
            this.AcceptButton = this.cmdUpdate;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(478, 353);
            this.ControlBox = false;
            this.Controls.Add(this.chkUseNoLock);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.txtServiceLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkKeepOpen);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblAppRolePassword);
            this.Controls.Add(this.lblAppRoleName);
            this.Controls.Add(this.txtAppRolePassword);
            this.Controls.Add(this.txtAppRoleName);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.txtDefPassword);
            this.Controls.Add(this.txtDefUser);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.txtServer);
            this.Controls.Add(this.chkUseAppRoles);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblSetting);
            this.Controls.Add(this.cboMultiDB);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblDefPassword);
            this.Controls.Add(this.lblDefUser);
            this.Controls.Add(this.lblAuthentication);
            this.Controls.Add(this.cboAuthentication);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.lblServer);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmConnections", "Connection Properties", ""));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmConnections";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Connection Properties";
            this.Load += new System.EventHandler(this.frmConnections_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Methods

		private void SetDefaults()
		{
			txtDescription.Text = _db.Description;
			cboAuthentication.Text = _db.LoginType;
			txtServer.Text = _db.Server;
			txtDatabase.Text = _db.DatabaseName;
            chkKeepOpen.Checked = _db.ConnectionAlwaysOpen;
            txtDefUser.Text = FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(_db.UserName);
            txtDefPassword.Text = FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(_db.Password);

			Text = String.Format("Database Settings - {0}", _cnn.CurrentLocation);

            //New populate application role controls
            if (FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(_db.ApplicationRoleName) != "")
            {
                //Check the box
                chkUseAppRoles.Checked = true;
                //now set the application role username and password 
                txtAppRoleName.Text = FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(_db.ApplicationRoleName);
                txtAppRolePassword.Text = FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(_db.ApplicationRolePassword);
            }
            else
            {
                chkUseAppRoles.Checked = false;
                txtAppRoleName.Text = String.Empty;
                txtAppRolePassword.Text = String.Empty;
            }

            chkUseNoLock.Checked = _db.UseNoLock;
            txtServiceLocation.Text = _db.ServiceLocation;
		}

		private void ApplyPermissions()
		{
			bool perm = true;

			string file = _cnn.CurrentLocation;
			if (System.IO.File.Exists(file) && System.IO.Path.IsPathRooted(file))
			{
				try
				{

					//By default deny access to the C Drive.....
					System.Security.Permissions.FileIOPermission permission = new System.Security.Permissions.FileIOPermission(System.Security.Permissions.FileIOPermissionAccess.Write, file);
					permission.Demand();

				}
				catch (Security.SecurityException)
				{
					perm = false;
				}
				catch(Exception ex)
				{
					perm = false;
					ErrorBox.Show(this, ex);
				}
				

				if (perm)
				{
					System.IO.FileInfo f = new System.IO.FileInfo(_cnn.CurrentLocation);
			
					if ((f.Attributes | System.IO.FileAttributes.ReadOnly) == f.Attributes)
						perm = false;
				}

				cmdUpdate.Enabled = perm;
				btnSaveAs.Enabled = perm;
			}
		}

		private void BuildList()
		{
			try
			{
				cboMultiDB.SelectedIndexChanged -= new EventHandler(this.cboMultiDB_SelectedIndexChanged);

				//Clear existing items from the combo.
				cboMultiDB.Items.Clear();

				//Loop round each DatabaseSettings object in the mult databases collection.
				for (int ctr = 0; ctr < _cnn.Count; ctr++)
				{
					cboMultiDB.Items.Add(_cnn[ctr]);
				}

				cboMultiDB.SelectedIndexChanged += new EventHandler(this.cboMultiDB_SelectedIndexChanged);

				//Set the combo to the current databse default.
				cboMultiDB.SelectedIndex   = _db.Number;
		
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
			finally
			{
			}
		}


		private void cboAuthentication_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				bool vis = true;
				switch(cboAuthentication.SelectedItem.ToString())
				{
					case "OMS":
						vis = true;
						break;
					case "SQL":
						vis = false;
						break;
					case "AAD":
					case "NT":
						vis = false;
						break;
				}

				txtDefUser.Enabled = vis;
				txtDefPassword.Enabled = vis;
			}
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void cmdUpdate_Click(object sender, System.EventArgs e)
		{
			try
			{
                cmdUpdate.Focus();

				_cnn.Save();
				this.DialogResult = DialogResult.OK;
			}
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void btnAdd_Click(object sender, System.EventArgs e)
		{

			try
			{
				_db = _cnn.CreateDatabaseSettings();
				_db.LoginType = "OMS";
				cboMultiDB.Items.Add(_db);
				cboMultiDB.SelectedItem = _db;
				txtDescription.Focus();
				txtDescription.SelectAll();
			}
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void btnBrowse_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					_cnn.Open(openFileDialog1.FileName);
					_db = _cnn.Default;
					BuildList();
					ApplyPermissions();
				}
			}
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			try
			{
				_cnn.Remove(_db);
				_db = _cnn.Default;
				BuildList();
			}
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void btnSaveAs_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
				{
					_cnn.SaveAs(saveFileDialog1.FileName);
					BuildList();
				}
			}
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void frmConnections_Load(object sender, System.EventArgs e)
		{
			try
			{
				BuildList();
				ApplyPermissions();
			}
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void cboMultiDB_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				_db = (DatabaseSettings)cboMultiDB.SelectedItem;
				SetDefaults();
			}
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void txtDescription_Leave(object sender, System.EventArgs e)
		{
			if (sender == txtDescription) _db.Description = txtDescription.Text;
			if (sender == cboAuthentication) _db.LoginType = cboAuthentication.Text;
			if (sender == txtServer) _db.Server = txtServer.Text;
			if (sender == txtDatabase) _db.DatabaseName = txtDatabase.Text;
			if (sender == txtDefUser) _db.ChangeUser(txtDefUser.Text);
			if (sender == txtDefPassword) _db.ChangePassword(txtDefPassword.Text);
            if (sender == chkUseAppRoles)
            {
                _db.ChangeAppRoleName(txtAppRoleName.Text);
                _db.ChangeAppRolePassword(txtAppRolePassword.Text);
            }
            if (sender == txtAppRoleName) _db.ChangeAppRoleName(txtAppRoleName.Text);
            if (sender == txtAppRolePassword) _db.ChangeAppRolePassword(txtAppRolePassword.Text);
            if (sender == chkKeepOpen) _db.ConnectionAlwaysOpen = chkKeepOpen.Checked;
            if (sender == chkUseNoLock) _db.UseNoLock = chkUseNoLock.Checked;
            if (sender == txtServiceLocation) _db.ServiceLocation = txtServiceLocation.Text;
		}
        //Set the application role controls based on the result of the checkbox
        private void chkUseAppRoles_CheckedChanged(object sender, System.EventArgs e)
        {
            //Empty the username and password
            txtAppRoleName.Text = "";
            txtAppRolePassword.Text = "";

            //Set enabled
            txtAppRoleName.Enabled = chkUseAppRoles.Checked;
            txtAppRolePassword.Enabled = chkUseAppRoles.Checked;

        }



		#endregion

        #region Properties

        public DatabaseSettings SelectedSettings
        {
            get
            {
                return (DatabaseSettings)cboMultiDB.SelectedItem;
            }
        }

        #endregion

    }
}
