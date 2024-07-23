using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common.UI.Windows;
using FWBS.OMS.Data;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// The form that allows you to pick document variable fields etc.. that are used
    /// by the field parser.
    /// </summary>
    internal class frmFields : frmNewBrandIdent
	{
		#region Controls

		private System.Windows.Forms.Panel pnlButtons;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnCreate;
		private FWBS.OMS.UI.Windows.ResourceLookup _res;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Label labSp;
		private FWBS.OMS.UI.TabControl tabFields;
		private System.Windows.Forms.TabPage pgeCommon;
		private System.Windows.Forms.TabPage pgeAssociate;
		private System.Windows.Forms.TabPage pgeAppointment;
		private System.Windows.Forms.TabPage pgeOther;
		private System.Windows.Forms.TabPage pgePrompt;
		private System.Windows.Forms.TabPage pgeLookup;
		private FWBS.OMS.UI.Windows.ucNavCommands fieldsAction;
		private FWBS.OMS.UI.Windows.ucSearchControl _fields;
		private System.Windows.Forms.Label lblCommonDesc;
		private System.Windows.Forms.Label lblPromptDesc;
		private System.Windows.Forms.Label lblLookupDesc;
		private System.Windows.Forms.Label lblAssocDesc;
		private System.Windows.Forms.Label lblApntDesc;
		private System.Windows.Forms.Label lblOtherDesc;
		private System.Windows.Forms.Panel pnlHelp;
		private FWBS.Common.UI.Windows.eInformation lblHelp;
		private FWBS.Common.UI.Windows.eTextBox2 txtPromptMessage;
		private System.Windows.Forms.CheckBox chkPromptRequired;
		private FWBS.Common.UI.Windows.eTextBox2 txtLookupField;
		private FWBS.Common.UI.Windows.eXPComboBox cboLookupType;
		private System.Windows.Forms.Label lblLookupTest;
		private FWBS.OMS.UI.Windows.DataGridViewEx dgCodeLookupView;
		private System.Windows.Forms.DataGridViewTextBoxColumn cdCodeColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn cdDescColumn;
		private System.Windows.Forms.Panel pnlAssocLeft;
		private FWBS.Common.UI.Windows.eXPComboBox cboAssocKey;
		private FWBS.Common.UI.Windows.eXPComboBox cboAssocType;
		private FWBS.Common.UI.Windows.eMultiTextBox2 txtAssocMessage;
		private FWBS.OMS.UI.Windows.ucSearchControl schAssocFields;
		private FWBS.OMS.UI.Windows.ucSearchControl schAppFields;
		private System.Windows.Forms.Panel panel1;
		private FWBS.Common.UI.Windows.eMultiTextBox2 txtAppMessage;
		private FWBS.Common.UI.Windows.eXPComboBox cboAppKey;
		private System.Windows.Forms.Panel panel2;
		private FWBS.Common.UI.Windows.eMultiTextBox2 txtOthMessage;
		private FWBS.Common.UI.Windows.eTextBox2 cboOthFilter;
		private FWBS.Common.UI.Windows.eXPComboBox cboOthKey;
		private FWBS.Common.UI.Windows.eXPComboBox cboOthList;
		private FWBS.OMS.UI.Windows.ucSearchControl schOthFields;
		private System.Windows.Forms.TabPage pgeMisc;
		private System.Windows.Forms.Label lblMiscDesc;
		private FWBS.Common.UI.Windows.eTextBox2 txtCode;
		private System.Windows.Forms.CheckBox chkSSM;
		private FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup cboContType;
		private System.Windows.Forms.Button btnExpand;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblFieldDesc;
		private System.Windows.Forms.Panel pnlFields;
        private System.Windows.Forms.Panel pnlFreeSpace2;
        private System.Windows.Forms.Panel pnlFreeSpace1;
        private System.Windows.Forms.Panel pnlFreeSpace;
        private System.Windows.Forms.Panel pnlFreeSpace3;
        private System.Windows.Forms.Panel pnlFreeSpace6;
        private System.Windows.Forms.Panel pnlFreeSpace5;
        private System.Windows.Forms.Panel pnlFreeSpace4;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlContainer;

        #endregion

        #region Fields

        private FWBS.OMS.Interfaces.IOMSApp _controlApp = null;
        private Favourites _fav = new Favourites("FIELDPICKER", "STYLE");

        //key sections for dbPrecedentKeys
        private enum KeySection : byte
        {
            Associate = 1,
            Appointment = 2,
            OtherList = 3
        }

        #endregion

        #region Constructors 

        private frmFields()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
            SetIcon(Images.DialogIcons.EliteApp);
            dgCodeLookupView.AutoGenerateColumns = false;
            cdCodeColumn.HeaderText = Session.CurrentSession.Resources.GetResource("CODE", "Code", null).Text;
            cdDescColumn.HeaderText = Session.CurrentSession.Resources.GetResource("DESCRIPTION", "Description", null).Text;
        }

		internal frmFields(FWBS.OMS.Interfaces.IOMSApp controlApp) : this()
		{
			_controlApp = controlApp;
			if (_controlApp is FWBS.OMS.Interfaces.ISecondStageMergeOMSApp)
				chkSSM.Visible = true;
			else
				chkSSM.Visible = false;

			//INFO: DM - 05/12/03 - GM asked me to do this, take the second stage merge off completely.
			chkSSM.Visible = false;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (fieldsAction != null)
				{
					fieldsAction.Dispose();
				}
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnExpand = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.labSp = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this._res = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pgeCommon = new System.Windows.Forms.TabPage();
            this._fields = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.fieldsAction = new FWBS.OMS.UI.Windows.ucNavCommands();
            this.lblCommonDesc = new System.Windows.Forms.Label();
            this.pgeAssociate = new System.Windows.Forms.TabPage();
            this.schAssocFields = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.pnlAssocLeft = new System.Windows.Forms.Panel();
            this.txtAssocMessage = new FWBS.Common.UI.Windows.eMultiTextBox2();
            this.pnlFreeSpace2 = new System.Windows.Forms.Panel();
            this.cboAssocType = new FWBS.Common.UI.Windows.eXPComboBox();
            this.pnlFreeSpace1 = new System.Windows.Forms.Panel();
            this.cboContType = new FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup();
            this.pnlFreeSpace = new System.Windows.Forms.Panel();
            this.cboAssocKey = new FWBS.Common.UI.Windows.eXPComboBox();
            this.lblAssocDesc = new System.Windows.Forms.Label();
            this.pgeAppointment = new System.Windows.Forms.TabPage();
            this.schAppFields = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtAppMessage = new FWBS.Common.UI.Windows.eMultiTextBox2();
            this.pnlFreeSpace3 = new System.Windows.Forms.Panel();
            this.cboAppKey = new FWBS.Common.UI.Windows.eXPComboBox();
            this.lblApntDesc = new System.Windows.Forms.Label();
            this.pgeOther = new System.Windows.Forms.TabPage();
            this.schOthFields = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtOthMessage = new FWBS.Common.UI.Windows.eMultiTextBox2();
            this.pnlFreeSpace6 = new System.Windows.Forms.Panel();
            this.cboOthFilter = new FWBS.Common.UI.Windows.eTextBox2();
            this.pnlFreeSpace5 = new System.Windows.Forms.Panel();
            this.cboOthKey = new FWBS.Common.UI.Windows.eXPComboBox();
            this.pnlFreeSpace4 = new System.Windows.Forms.Panel();
            this.cboOthList = new FWBS.Common.UI.Windows.eXPComboBox();
            this.lblOtherDesc = new System.Windows.Forms.Label();
            this.pgePrompt = new System.Windows.Forms.TabPage();
            this.lblPromptDesc = new System.Windows.Forms.Label();
            this.txtPromptMessage = new FWBS.Common.UI.Windows.eTextBox2();
            this.chkPromptRequired = new System.Windows.Forms.CheckBox();
            this.txtLookupField = new FWBS.Common.UI.Windows.eTextBox2();
            this.lblLookupDesc = new System.Windows.Forms.Label();
            this.pgeMisc = new System.Windows.Forms.TabPage();
            this.pnlFields = new System.Windows.Forms.Panel();
            this.chkSSM = new System.Windows.Forms.CheckBox();
            this.txtCode = new FWBS.Common.UI.Windows.eTextBox2();
            this.lblFieldDesc = new System.Windows.Forms.Label();
            this.lblMiscDesc = new System.Windows.Forms.Label();
            this.cboLookupType = new FWBS.Common.UI.Windows.eXPComboBox();
            this.pgeLookup = new System.Windows.Forms.TabPage();
            this.dgCodeLookupView = new FWBS.OMS.UI.Windows.DataGridViewEx();
            this.lblLookupTest = new System.Windows.Forms.Label();
            this.cdCodeColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cdDescColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabFields = new FWBS.OMS.UI.TabControl();
            this.pnlHelp = new System.Windows.Forms.Panel();
            this.lblHelp = new FWBS.Common.UI.Windows.eInformation();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlContainer = new System.Windows.Forms.Panel();
            this.pnlButtons.SuspendLayout();
            this.pgeCommon.SuspendLayout();
            this.pgeAssociate.SuspendLayout();
            this.pnlAssocLeft.SuspendLayout();
            this.pgeAppointment.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pgeOther.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pgePrompt.SuspendLayout();
            this.pgeMisc.SuspendLayout();
            this.pnlFields.SuspendLayout();
            this.pgeLookup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgCodeLookupView)).BeginInit();
            this.tabFields.SuspendLayout();
            this.pnlHelp.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnExpand);
            this.pnlButtons.Controls.Add(this.label1);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.labSp);
            this.pnlButtons.Controls.Add(this.btnCreate);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Location = new System.Drawing.Point(685, 5);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Padding = new System.Windows.Forms.Padding(7);
            this.pnlButtons.Size = new System.Drawing.Size(94, 551);
            this.pnlButtons.TabIndex = 3;
            // 
            // btnExpand
            // 
            this.btnExpand.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExpand.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnExpand.Location = new System.Drawing.Point(7, 63);
            this._res.SetLookup(this.btnExpand, new FWBS.OMS.UI.Windows.ResourceLookupItem("SHRINK", "&Shrink <<", ""));
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(80, 23);
            this.btnExpand.TabIndex = 6;
            this.btnExpand.Tag = "EXPAND";
            this.btnExpand.Text = "&Shrink <<";
            this.btnExpand.Click += new System.EventHandler(this.btnExpand_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(7, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 5);
            this.label1.TabIndex = 7;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(7, 35);
            this._res.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCLOSE", "&Close", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Close";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // labSp
            // 
            this.labSp.Dock = System.Windows.Forms.DockStyle.Top;
            this.labSp.Location = new System.Drawing.Point(7, 30);
            this.labSp.Name = "labSp";
            this.labSp.Size = new System.Drawing.Size(80, 5);
            this.labSp.TabIndex = 5;
            // 
            // btnCreate
            // 
            this.btnCreate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCreate.Location = new System.Drawing.Point(7, 7);
            this._res.SetLookup(this.btnCreate, new FWBS.OMS.UI.Windows.ResourceLookupItem("ADD", "&Add", ""));
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(80, 23);
            this.btnCreate.TabIndex = 1;
            this.btnCreate.Text = "&Add";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // pgeCommon
            // 
            this.pgeCommon.BackColor = System.Drawing.SystemColors.Control;
            this.pgeCommon.Controls.Add(this._fields);
            this.pgeCommon.Controls.Add(this.lblCommonDesc);
            this.pgeCommon.Location = new System.Drawing.Point(4, 24);
            this._res.SetLookup(this.pgeCommon, new FWBS.OMS.UI.Windows.ResourceLookupItem("COMMON", "Common", ""));
            this.pgeCommon.Name = "pgeCommon";
            this.pgeCommon.Size = new System.Drawing.Size(672, 427);
            this.pgeCommon.TabIndex = 0;
            this.pgeCommon.Text = "Common";
            // 
            // _fields
            // 
            this._fields.BackColor = System.Drawing.Color.White;
            this._fields.BackGroundColor = System.Drawing.SystemColors.AppWorkspace;
            this._fields.DisplayResultsCaption = false;
            this._fields.Dock = System.Windows.Forms.DockStyle.Fill;
            this._fields.DoubleClickAction = "None";
            this._fields.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._fields.GraphicalPanelVisible = true;
            this._fields.Location = new System.Drawing.Point(0, 32);
            this._fields.Margin = new System.Windows.Forms.Padding(0);
            this._fields.Name = "_fields";
            this._fields.NavCommandPanel = this.fieldsAction;
            this._fields.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this._fields.SearchListCode = "";
            this._fields.SearchListType = "";
            this._fields.Size = new System.Drawing.Size(672, 395);
            this._fields.TabIndex = 1;
            this._fields.ToBeRefreshed = false;
            this._fields.ItemHover += new FWBS.OMS.UI.Windows.SearchItemHoverEventHandler(this._fields_ItemHover);
            this._fields.SearchCompleted += new FWBS.OMS.UI.Windows.SearchCompletedEventHandler(this._fields_SearchCompleted);
            // 
            // fieldsAction
            // 
            this.fieldsAction.Location = new System.Drawing.Point(0, 0);
            this.fieldsAction.Name = "fieldsAction";
            this.fieldsAction.Padding = new System.Windows.Forms.Padding(5);
            this.fieldsAction.PanelBackColor = System.Drawing.Color.Empty;
            this.fieldsAction.Size = new System.Drawing.Size(0, 0);
            this.fieldsAction.TabIndex = 0;
            this.fieldsAction.TabStop = false;
            this.fieldsAction.Visible = false;
            // 
            // lblCommonDesc
            // 
            this.lblCommonDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCommonDesc.Location = new System.Drawing.Point(0, 0);
            this._res.SetLookup(this.lblCommonDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("COMMONFIELDS", "The common fields can be selected and used to merge information from the system i" +
            "nto your documents.  These fields are taken from different object from within th" +
            "e sytem like %CLIENT%, %FILE% etc...", ""));
            this.lblCommonDesc.Name = "lblCommonDesc";
            this.lblCommonDesc.Size = new System.Drawing.Size(672, 32);
            this.lblCommonDesc.TabIndex = 0;
            this.lblCommonDesc.Text = "The common fields can be selected and used to merge information from the system i" +
    "nto your documents.  These fields are taken from different object from within th" +
    "e sytem like %CLIENT%, %FILE% etc...";
            // 
            // pgeAssociate
            // 
            this.pgeAssociate.BackColor = System.Drawing.SystemColors.Control;
            this.pgeAssociate.Controls.Add(this.schAssocFields);
            this.pgeAssociate.Controls.Add(this.pnlAssocLeft);
            this.pgeAssociate.Controls.Add(this.lblAssocDesc);
            this.pgeAssociate.Location = new System.Drawing.Point(4, 24);
            this._res.SetLookup(this.pgeAssociate, new FWBS.OMS.UI.Windows.ResourceLookupItem("ASSOCIATE", "%ASSOCIATE%", ""));
            this.pgeAssociate.Name = "pgeAssociate";
            this.pgeAssociate.Size = new System.Drawing.Size(672, 427);
            this.pgeAssociate.TabIndex = 1;
            this.pgeAssociate.Text = "%ASSOCIATE%";
            // 
            // schAssocFields
            // 
            this.schAssocFields.BackColor = System.Drawing.Color.White;
            this.schAssocFields.BackGroundColor = System.Drawing.SystemColors.AppWorkspace;
            this.schAssocFields.DisplayResultsCaption = false;
            this.schAssocFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schAssocFields.DoubleClickAction = "None";
            this.schAssocFields.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.schAssocFields.GraphicalPanelVisible = true;
            this.schAssocFields.Location = new System.Drawing.Point(284, 32);
            this.schAssocFields.Margin = new System.Windows.Forms.Padding(0);
            this.schAssocFields.Name = "schAssocFields";
            this.schAssocFields.NavCommandPanel = this.fieldsAction;
            this.schAssocFields.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.schAssocFields.SearchListCode = "";
            this.schAssocFields.SearchListType = "";
            this.schAssocFields.Size = new System.Drawing.Size(388, 395);
            this.schAssocFields.TabIndex = 9;
            this.schAssocFields.ToBeRefreshed = false;
            this.schAssocFields.TypeSelectorVisible = false;
            this.schAssocFields.ItemHover += new FWBS.OMS.UI.Windows.SearchItemHoverEventHandler(this._fields_ItemHover);
            // 
            // pnlAssocLeft
            // 
            this.pnlAssocLeft.Controls.Add(this.txtAssocMessage);
            this.pnlAssocLeft.Controls.Add(this.pnlFreeSpace2);
            this.pnlAssocLeft.Controls.Add(this.cboAssocType);
            this.pnlAssocLeft.Controls.Add(this.pnlFreeSpace1);
            this.pnlAssocLeft.Controls.Add(this.cboContType);
            this.pnlAssocLeft.Controls.Add(this.pnlFreeSpace);
            this.pnlAssocLeft.Controls.Add(this.cboAssocKey);
            this.pnlAssocLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlAssocLeft.Location = new System.Drawing.Point(0, 32);
            this.pnlAssocLeft.Name = "pnlAssocLeft";
            this.pnlAssocLeft.Padding = new System.Windows.Forms.Padding(10);
            this.pnlAssocLeft.Size = new System.Drawing.Size(284, 395);
            this.pnlAssocLeft.TabIndex = 1;
            // 
            // txtAssocMessage
            // 
            this.txtAssocMessage.CaptionWidth = 100;
            this.txtAssocMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAssocMessage.IsDirty = false;
            this.txtAssocMessage.Location = new System.Drawing.Point(10, 124);
            this._res.SetLookup(this.txtAssocMessage, new FWBS.OMS.UI.Windows.ResourceLookupItem("MESSAGE", "Message", ""));
            this.txtAssocMessage.MaxLength = 32767;
            this.txtAssocMessage.Name = "txtAssocMessage";
            this.txtAssocMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAssocMessage.Size = new System.Drawing.Size(264, 261);
            this.txtAssocMessage.TabIndex = 8;
            this.txtAssocMessage.TabStop = true;
            this.txtAssocMessage.Text = "Message";
            // 
            // pnlFreeSpace2
            // 
            this.pnlFreeSpace2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFreeSpace2.Location = new System.Drawing.Point(10, 109);
            this.pnlFreeSpace2.Name = "pnlFreeSpace2";
            this.pnlFreeSpace2.Size = new System.Drawing.Size(264, 15);
            this.pnlFreeSpace2.TabIndex = 7;
            // 
            // cboAssocType
            // 
            this.cboAssocType.ActiveSearchEnabled = true;
            this.cboAssocType.CaptionWidth = 100;
            this.cboAssocType.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboAssocType.IsDirty = false;
            this.cboAssocType.Location = new System.Drawing.Point(10, 86);
            this._res.SetLookup(this.cboAssocType, new FWBS.OMS.UI.Windows.ResourceLookupItem("ASSOCAS", "Associated As", ""));
            this.cboAssocType.MaxLength = 0;
            this.cboAssocType.Name = "cboAssocType";
            this.cboAssocType.Size = new System.Drawing.Size(264, 23);
            this.cboAssocType.TabIndex = 6;
            this.cboAssocType.Text = "Associated As";
            // 
            // pnlFreeSpace1
            // 
            this.pnlFreeSpace1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFreeSpace1.Location = new System.Drawing.Point(10, 71);
            this.pnlFreeSpace1.Name = "pnlFreeSpace1";
            this.pnlFreeSpace1.Size = new System.Drawing.Size(264, 15);
            this.pnlFreeSpace1.TabIndex = 5;
            // 
            // cboContType
            // 
            this.cboContType.ActiveSearchEnabled = true;
            this.cboContType.AddNotSet = false;
            this.cboContType.CaptionWidth = 100;
            this.cboContType.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboContType.IsDirty = false;
            this.cboContType.Location = new System.Drawing.Point(10, 48);
            this._res.SetLookup(this.cboContType, new FWBS.OMS.UI.Windows.ResourceLookupItem("CONTTYPE", "Contact Type", ""));
            this.cboContType.MaxLength = 0;
            this.cboContType.Name = "cboContType";
            this.cboContType.NotSetCode = "NOTSET";
            this.cboContType.NotSetType = "RESOURCE";
            this.cboContType.Size = new System.Drawing.Size(264, 23);
            this.cboContType.TabIndex = 4;
            this.cboContType.TerminologyParse = false;
            this.cboContType.Text = "Contact Type";
            this.cboContType.Type = "";
            this.cboContType.ActiveChanged += new System.EventHandler(this.cboContType_ActiveChanged);
            // 
            // pnlFreeSpace
            // 
            this.pnlFreeSpace.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFreeSpace.Location = new System.Drawing.Point(10, 33);
            this.pnlFreeSpace.Name = "pnlFreeSpace";
            this.pnlFreeSpace.Size = new System.Drawing.Size(264, 15);
            this.pnlFreeSpace.TabIndex = 3;
            // 
            // cboAssocKey
            // 
            this.cboAssocKey.ActiveSearchEnabled = true;
            this.cboAssocKey.CaptionWidth = 100;
            this.cboAssocKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboAssocKey.IsDirty = false;
            this.cboAssocKey.LimitToList = false;
            this.cboAssocKey.Location = new System.Drawing.Point(10, 10);
            this._res.SetLookup(this.cboAssocKey, new FWBS.OMS.UI.Windows.ResourceLookupItem("KEY", "Key", ""));
            this.cboAssocKey.MaxLength = 0;
            this.cboAssocKey.Name = "cboAssocKey";
            this.cboAssocKey.Size = new System.Drawing.Size(264, 23);
            this.cboAssocKey.TabIndex = 2;
            this.cboAssocKey.Text = "Key";
            this.cboAssocKey.Value = "";
            this.cboAssocKey.ActiveChanged += new System.EventHandler(this.cboAssocKey_ActiveChanged);
            // 
            // lblAssocDesc
            // 
            this.lblAssocDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblAssocDesc.Location = new System.Drawing.Point(0, 0);
            this._res.SetLookup(this.lblAssocDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("ASSOCFIELDS", "Associate fields allows the associate list of the current %FILE% to be prompted a" +
            "nd a common field of the Associate to be inserted.  You may have more than one A" +
            "ssociate on the document.", ""));
            this.lblAssocDesc.Name = "lblAssocDesc";
            this.lblAssocDesc.Size = new System.Drawing.Size(672, 32);
            this.lblAssocDesc.TabIndex = 0;
            this.lblAssocDesc.Text = "Associate fields allows the associate list of the current %FILE% to be prompted a" +
    "nd a common field of the Associate to be inserted.  You may have more than one A" +
    "ssociate on the document.";
            // 
            // pgeAppointment
            // 
            this.pgeAppointment.BackColor = System.Drawing.SystemColors.Control;
            this.pgeAppointment.Controls.Add(this.schAppFields);
            this.pgeAppointment.Controls.Add(this.panel1);
            this.pgeAppointment.Controls.Add(this.lblApntDesc);
            this.pgeAppointment.Location = new System.Drawing.Point(4, 24);
            this._res.SetLookup(this.pgeAppointment, new FWBS.OMS.UI.Windows.ResourceLookupItem("APPOINTMENT", "Appointment", ""));
            this.pgeAppointment.Name = "pgeAppointment";
            this.pgeAppointment.Size = new System.Drawing.Size(672, 427);
            this.pgeAppointment.TabIndex = 2;
            this.pgeAppointment.Text = "Appointment";
            // 
            // schAppFields
            // 
            this.schAppFields.BackColor = System.Drawing.Color.White;
            this.schAppFields.BackGroundColor = System.Drawing.SystemColors.AppWorkspace;
            this.schAppFields.DisplayResultsCaption = false;
            this.schAppFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schAppFields.DoubleClickAction = "None";
            this.schAppFields.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.schAppFields.GraphicalPanelVisible = true;
            this.schAppFields.Location = new System.Drawing.Point(284, 32);
            this.schAppFields.Margin = new System.Windows.Forms.Padding(0);
            this.schAppFields.Name = "schAppFields";
            this.schAppFields.NavCommandPanel = this.fieldsAction;
            this.schAppFields.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.schAppFields.SearchListCode = "";
            this.schAppFields.SearchListType = "";
            this.schAppFields.Size = new System.Drawing.Size(388, 395);
            this.schAppFields.TabIndex = 5;
            this.schAppFields.ToBeRefreshed = false;
            this.schAppFields.TypeSelectorVisible = false;
            this.schAppFields.ItemHover += new FWBS.OMS.UI.Windows.SearchItemHoverEventHandler(this._fields_ItemHover);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtAppMessage);
            this.panel1.Controls.Add(this.pnlFreeSpace3);
            this.panel1.Controls.Add(this.cboAppKey);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 32);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(284, 395);
            this.panel1.TabIndex = 1;
            // 
            // txtAppMessage
            // 
            this.txtAppMessage.CaptionWidth = 100;
            this.txtAppMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtAppMessage.IsDirty = false;
            this.txtAppMessage.Location = new System.Drawing.Point(10, 48);
            this._res.SetLookup(this.txtAppMessage, new FWBS.OMS.UI.Windows.ResourceLookupItem("MESSAGE", "Message", ""));
            this.txtAppMessage.MaxLength = 32767;
            this.txtAppMessage.Name = "txtAppMessage";
            this.txtAppMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtAppMessage.Size = new System.Drawing.Size(264, 337);
            this.txtAppMessage.TabIndex = 4;
            this.txtAppMessage.TabStop = true;
            this.txtAppMessage.Text = "Message";
            // 
            // pnlFreeSpace3
            // 
            this.pnlFreeSpace3.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFreeSpace3.Location = new System.Drawing.Point(10, 33);
            this.pnlFreeSpace3.Name = "pnlFreeSpace3";
            this.pnlFreeSpace3.Size = new System.Drawing.Size(264, 15);
            this.pnlFreeSpace3.TabIndex = 3;
            // 
            // cboAppKey
            // 
            this.cboAppKey.ActiveSearchEnabled = true;
            this.cboAppKey.CaptionWidth = 100;
            this.cboAppKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboAppKey.IsDirty = false;
            this.cboAppKey.LimitToList = false;
            this.cboAppKey.Location = new System.Drawing.Point(10, 10);
            this._res.SetLookup(this.cboAppKey, new FWBS.OMS.UI.Windows.ResourceLookupItem("KEY", "Key", ""));
            this.cboAppKey.MaxLength = 0;
            this.cboAppKey.Name = "cboAppKey";
            this.cboAppKey.Size = new System.Drawing.Size(264, 23);
            this.cboAppKey.TabIndex = 2;
            this.cboAppKey.Text = "Key";
            this.cboAppKey.Value = "";
            this.cboAppKey.ActiveChanged += new System.EventHandler(this.cboAppKey_ActiveChanged);
            // 
            // lblApntDesc
            // 
            this.lblApntDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblApntDesc.Location = new System.Drawing.Point(0, 0);
            this._res.SetLookup(this.lblApntDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("APNTFIELDS", "Appointment fields allows the current %FILES% Appointment list to be prompted and" +
            " a common field of the Appointment to be inserted.  You may have more than one A" +
            "ppointment on the document.", ""));
            this.lblApntDesc.Name = "lblApntDesc";
            this.lblApntDesc.Size = new System.Drawing.Size(672, 32);
            this.lblApntDesc.TabIndex = 0;
            this.lblApntDesc.Text = "Appointment fields allows the current %FILES% Appointment list to be prompted and" +
    " a common field of the Appointment to be inserted.  You may have more than one A" +
    "ppointment on the document.";
            // 
            // pgeOther
            // 
            this.pgeOther.BackColor = System.Drawing.SystemColors.Control;
            this.pgeOther.Controls.Add(this.schOthFields);
            this.pgeOther.Controls.Add(this.panel2);
            this.pgeOther.Controls.Add(this.lblOtherDesc);
            this.pgeOther.Location = new System.Drawing.Point(4, 24);
            this._res.SetLookup(this.pgeOther, new FWBS.OMS.UI.Windows.ResourceLookupItem("OTHERLIST", "Other List", ""));
            this.pgeOther.Name = "pgeOther";
            this.pgeOther.Size = new System.Drawing.Size(672, 427);
            this.pgeOther.TabIndex = 3;
            this.pgeOther.Text = "Other List";
            // 
            // schOthFields
            // 
            this.schOthFields.BackColor = System.Drawing.Color.White;
            this.schOthFields.BackGroundColor = System.Drawing.SystemColors.AppWorkspace;
            this.schOthFields.DisplayResultsCaption = false;
            this.schOthFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schOthFields.DoubleClickAction = "None";
            this.schOthFields.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.schOthFields.GraphicalPanelVisible = true;
            this.schOthFields.Location = new System.Drawing.Point(284, 32);
            this.schOthFields.Margin = new System.Windows.Forms.Padding(0);
            this.schOthFields.Name = "schOthFields";
            this.schOthFields.NavCommandPanel = this.fieldsAction;
            this.schOthFields.Padding = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.schOthFields.SearchListCode = "";
            this.schOthFields.SearchListType = "";
            this.schOthFields.Size = new System.Drawing.Size(388, 395);
            this.schOthFields.TabIndex = 9;
            this.schOthFields.ToBeRefreshed = false;
            this.schOthFields.TypeSelectorVisible = false;
            this.schOthFields.ItemHover += new FWBS.OMS.UI.Windows.SearchItemHoverEventHandler(this._fields_ItemHover);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtOthMessage);
            this.panel2.Controls.Add(this.pnlFreeSpace6);
            this.panel2.Controls.Add(this.cboOthFilter);
            this.panel2.Controls.Add(this.pnlFreeSpace5);
            this.panel2.Controls.Add(this.cboOthKey);
            this.panel2.Controls.Add(this.pnlFreeSpace4);
            this.panel2.Controls.Add(this.cboOthList);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 32);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Size = new System.Drawing.Size(284, 395);
            this.panel2.TabIndex = 1;
            // 
            // txtOthMessage
            // 
            this.txtOthMessage.CaptionWidth = 100;
            this.txtOthMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOthMessage.IsDirty = false;
            this.txtOthMessage.Location = new System.Drawing.Point(10, 124);
            this._res.SetLookup(this.txtOthMessage, new FWBS.OMS.UI.Windows.ResourceLookupItem("MESSAGE", "Message", ""));
            this.txtOthMessage.MaxLength = 32767;
            this.txtOthMessage.Name = "txtOthMessage";
            this.txtOthMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOthMessage.Size = new System.Drawing.Size(264, 261);
            this.txtOthMessage.TabIndex = 8;
            this.txtOthMessage.TabStop = true;
            this.txtOthMessage.Text = "Message";
            // 
            // pnlFreeSpace6
            // 
            this.pnlFreeSpace6.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFreeSpace6.Location = new System.Drawing.Point(10, 109);
            this.pnlFreeSpace6.Name = "pnlFreeSpace6";
            this.pnlFreeSpace6.Size = new System.Drawing.Size(264, 15);
            this.pnlFreeSpace6.TabIndex = 7;
            // 
            // cboOthFilter
            // 
            this.cboOthFilter.CaptionWidth = 100;
            this.cboOthFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboOthFilter.IsDirty = false;
            this.cboOthFilter.Location = new System.Drawing.Point(10, 86);
            this._res.SetLookup(this.cboOthFilter, new FWBS.OMS.UI.Windows.ResourceLookupItem("FILTER", "Filter", ""));
            this.cboOthFilter.MaxLength = 32767;
            this.cboOthFilter.Name = "cboOthFilter";
            this.cboOthFilter.Size = new System.Drawing.Size(264, 23);
            this.cboOthFilter.TabIndex = 6;
            this.cboOthFilter.TabStop = true;
            this.cboOthFilter.Text = "Filter";
            // 
            // pnlFreeSpace5
            // 
            this.pnlFreeSpace5.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFreeSpace5.Location = new System.Drawing.Point(10, 71);
            this.pnlFreeSpace5.Name = "pnlFreeSpace5";
            this.pnlFreeSpace5.Size = new System.Drawing.Size(264, 15);
            this.pnlFreeSpace5.TabIndex = 5;
            // 
            // cboOthKey
            // 
            this.cboOthKey.ActiveSearchEnabled = true;
            this.cboOthKey.CaptionWidth = 100;
            this.cboOthKey.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboOthKey.IsDirty = false;
            this.cboOthKey.LimitToList = false;
            this.cboOthKey.Location = new System.Drawing.Point(10, 48);
            this._res.SetLookup(this.cboOthKey, new FWBS.OMS.UI.Windows.ResourceLookupItem("KEY", "Key", ""));
            this.cboOthKey.MaxLength = 0;
            this.cboOthKey.Name = "cboOthKey";
            this.cboOthKey.Size = new System.Drawing.Size(264, 23);
            this.cboOthKey.TabIndex = 4;
            this.cboOthKey.Text = "Key";
            this.cboOthKey.Value = "";
            this.cboOthKey.ActiveChanged += new System.EventHandler(this.cboOthKey_ActiveChanged);
            // 
            // pnlFreeSpace4
            // 
            this.pnlFreeSpace4.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFreeSpace4.Location = new System.Drawing.Point(10, 33);
            this.pnlFreeSpace4.Name = "pnlFreeSpace4";
            this.pnlFreeSpace4.Size = new System.Drawing.Size(264, 15);
            this.pnlFreeSpace4.TabIndex = 3;
            // 
            // cboOthList
            // 
            this.cboOthList.ActiveSearchEnabled = true;
            this.cboOthList.CaptionWidth = 100;
            this.cboOthList.Dock = System.Windows.Forms.DockStyle.Top;
            this.cboOthList.IsDirty = false;
            this.cboOthList.Location = new System.Drawing.Point(10, 10);
            this._res.SetLookup(this.cboOthList, new FWBS.OMS.UI.Windows.ResourceLookupItem("LIST", "List", ""));
            this.cboOthList.MaxLength = 0;
            this.cboOthList.Name = "cboOthList";
            this.cboOthList.Size = new System.Drawing.Size(264, 23);
            this.cboOthList.TabIndex = 2;
            this.cboOthList.Text = "List";
            this.cboOthList.ActiveChanged += new System.EventHandler(this.cboOthList_ActiveChanged);
            // 
            // lblOtherDesc
            // 
            this.lblOtherDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblOtherDesc.Location = new System.Drawing.Point(0, 0);
            this._res.SetLookup(this.lblOtherDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("OTHLISTFIELDS", "Prompts the user with any other list of extended information. You may have more t" +
            "han one item on the document.", ""));
            this.lblOtherDesc.Name = "lblOtherDesc";
            this.lblOtherDesc.Size = new System.Drawing.Size(672, 32);
            this.lblOtherDesc.TabIndex = 0;
            this.lblOtherDesc.Text = "Prompts the user with any other list of extended information. You may have more t" +
    "han one item on the document.";
            // 
            // pgePrompt
            // 
            this.pgePrompt.BackColor = System.Drawing.SystemColors.Control;
            this.pgePrompt.Controls.Add(this.lblPromptDesc);
            this.pgePrompt.Controls.Add(this.txtPromptMessage);
            this.pgePrompt.Controls.Add(this.chkPromptRequired);
            this.pgePrompt.Location = new System.Drawing.Point(4, 24);
            this._res.SetLookup(this.pgePrompt, new FWBS.OMS.UI.Windows.ResourceLookupItem("PROMPT", "Prompt", ""));
            this.pgePrompt.Name = "pgePrompt";
            this.pgePrompt.Size = new System.Drawing.Size(672, 427);
            this.pgePrompt.TabIndex = 4;
            this.pgePrompt.Text = "Prompt";
            // 
            // lblPromptDesc
            // 
            this.lblPromptDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPromptDesc.Location = new System.Drawing.Point(0, 0);
            this._res.SetLookup(this.lblPromptDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("PROMPTFIELDS", "Use this type of field to prompt the user for information.  An input box will app" +
            "ear when the %PRECEDENT% merges into a new document with the specified message.", ""));
            this.lblPromptDesc.Name = "lblPromptDesc";
            this.lblPromptDesc.Size = new System.Drawing.Size(672, 32);
            this.lblPromptDesc.TabIndex = 0;
            this.lblPromptDesc.Text = "Use this type of field to prompt the user for information.  An input box will app" +
    "ear when the %PRECEDENT% merges into a new document with the specified message.";
            // 
            // txtPromptMessage
            // 
            this.txtPromptMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPromptMessage.CaptionWidth = 150;
            this.txtPromptMessage.IsDirty = false;
            this.txtPromptMessage.Location = new System.Drawing.Point(8, 56);
            this._res.SetLookup(this.txtPromptMessage, new FWBS.OMS.UI.Windows.ResourceLookupItem("MESSAGE", "Message", ""));
            this.txtPromptMessage.MaxLength = 32767;
            this.txtPromptMessage.Name = "txtPromptMessage";
            this.txtPromptMessage.Size = new System.Drawing.Size(654, 23);
            this.txtPromptMessage.TabIndex = 1;
            this.txtPromptMessage.TabStop = true;
            this.txtPromptMessage.Text = "Message";
            // 
            // chkPromptRequired
            // 
            this.chkPromptRequired.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkPromptRequired.Location = new System.Drawing.Point(8, 88);
            this._res.SetLookup(this.chkPromptRequired, new FWBS.OMS.UI.Windows.ResourceLookupItem("REQUIRED", "Required", ""));
            this.chkPromptRequired.Name = "chkPromptRequired";
            this.chkPromptRequired.Size = new System.Drawing.Size(164, 24);
            this.chkPromptRequired.TabIndex = 2;
            this.chkPromptRequired.Text = "Required";
            // 
            // txtLookupField
            // 
            this.txtLookupField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLookupField.CaptionWidth = 150;
            this.txtLookupField.Casing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtLookupField.IsDirty = false;
            this.txtLookupField.Location = new System.Drawing.Point(8, 43);
            this._res.SetLookup(this.txtLookupField, new FWBS.OMS.UI.Windows.ResourceLookupItem("FIELD", "Field", ""));
            this.txtLookupField.MaxLength = 32767;
            this.txtLookupField.Name = "txtLookupField";
            this.txtLookupField.Size = new System.Drawing.Size(654, 23);
            this.txtLookupField.TabIndex = 1;
            this.txtLookupField.TabStop = true;
            this.txtLookupField.Text = "Field";
            // 
            // lblLookupDesc
            // 
            this.lblLookupDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLookupDesc.Location = new System.Drawing.Point(0, 0);
            this._res.SetLookup(this.lblLookupDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("LOOKUPFIELDS", "A lookup field will interpret a given common field into the code lookup descripti" +
            "on of the current cultures language.  For example, FILESTATUS may return back \'L" +
            "ive\', \'Dead\' etc..", ""));
            this.lblLookupDesc.Name = "lblLookupDesc";
            this.lblLookupDesc.Size = new System.Drawing.Size(672, 32);
            this.lblLookupDesc.TabIndex = 0;
            this.lblLookupDesc.Text = "A lookup field will interpret a given common field into the code lookup descripti" +
    "on of the current cultures language.  For example, FILESTATUS may return back \'L" +
    "ive\', \'Dead\' etc..";
            // 
            // pgeMisc
            // 
            this.pgeMisc.BackColor = System.Drawing.SystemColors.Control;
            this.pgeMisc.Controls.Add(this.pnlFields);
            this.pgeMisc.Controls.Add(this.lblMiscDesc);
            this.pgeMisc.Location = new System.Drawing.Point(4, 24);
            this._res.SetLookup(this.pgeMisc, new FWBS.OMS.UI.Windows.ResourceLookupItem("MISC", "Misc.", ""));
            this.pgeMisc.Name = "pgeMisc";
            this.pgeMisc.Size = new System.Drawing.Size(672, 427);
            this.pgeMisc.TabIndex = 6;
            this.pgeMisc.Text = "Misc.";
            // 
            // pnlFields
            // 
            this.pnlFields.Controls.Add(this.chkSSM);
            this.pnlFields.Controls.Add(this.txtCode);
            this.pnlFields.Controls.Add(this.lblFieldDesc);
            this.pnlFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFields.Location = new System.Drawing.Point(0, 32);
            this.pnlFields.Name = "pnlFields";
            this.pnlFields.Padding = new System.Windows.Forms.Padding(10);
            this.pnlFields.Size = new System.Drawing.Size(672, 395);
            this.pnlFields.TabIndex = 1;
            // 
            // chkSSM
            // 
            this.chkSSM.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkSSM.Location = new System.Drawing.Point(10, 58);
            this._res.SetLookup(this.chkSSM, new FWBS.OMS.UI.Windows.ResourceLookupItem("SSM", "Second Stage Merge", ""));
            this.chkSSM.Name = "chkSSM";
            this.chkSSM.Size = new System.Drawing.Size(164, 24);
            this.chkSSM.TabIndex = 4;
            this.chkSSM.Text = "Second Stage Merge";
            this.chkSSM.Visible = false;
            // 
            // txtCode
            // 
            this.txtCode.CaptionWidth = 150;
            this.txtCode.Casing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCode.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtCode.IsDirty = false;
            this.txtCode.Location = new System.Drawing.Point(10, 34);
            this._res.SetLookup(this.txtCode, new FWBS.OMS.UI.Windows.ResourceLookupItem("FIELDCODE", "Field Code", ""));
            this.txtCode.MaxLength = 32767;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(652, 23);
            this.txtCode.TabIndex = 3;
            this.txtCode.TabStop = true;
            this.txtCode.Text = "Field Code";
            // 
            // lblFieldDesc
            // 
            this.lblFieldDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblFieldDesc.Location = new System.Drawing.Point(10, 10);
            this._res.SetLookup(this.lblFieldDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("FIELDINSERTDESC", "Please specify a field code to insert into the document.", ""));
            this.lblFieldDesc.Name = "lblFieldDesc";
            this.lblFieldDesc.Size = new System.Drawing.Size(652, 24);
            this.lblFieldDesc.TabIndex = 2;
            this.lblFieldDesc.Text = "Please specify a field code to insert into the document.";
            this.lblFieldDesc.Visible = false;
            // 
            // lblMiscDesc
            // 
            this.lblMiscDesc.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMiscDesc.Location = new System.Drawing.Point(0, 0);
            this._res.SetLookup(this.lblMiscDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("MISCFIELDS", "Use the text box below to add a miscellaneous field name.  This could be one that" +
            " does not appear in any of the lists shown by the form.", ""));
            this.lblMiscDesc.Name = "lblMiscDesc";
            this.lblMiscDesc.Size = new System.Drawing.Size(672, 32);
            this.lblMiscDesc.TabIndex = 0;
            this.lblMiscDesc.Text = "Use the text box below to add a miscellaneous field name.  This could be one that" +
    " does not appear in any of the lists shown by the form.";
            // 
            // cboLookupType
            // 
            this.cboLookupType.ActiveSearchEnabled = true;
            this.cboLookupType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLookupType.IsDirty = false;
            this.cboLookupType.Location = new System.Drawing.Point(8, 72);
            this._res.SetLookup(this.cboLookupType, new FWBS.OMS.UI.Windows.ResourceLookupItem("LOOKUPTYPE", "Lookup Type", ""));
            this.cboLookupType.MaxLength = 0;
            this.cboLookupType.Name = "cboLookupType";
            this.cboLookupType.Size = new System.Drawing.Size(654, 23);
            this.cboLookupType.TabIndex = 2;
            this.cboLookupType.Text = "Lookup Type";
            this.cboLookupType.DoesNotExist += new System.ComponentModel.CancelEventHandler(this.cboLookupType_DoesNotExist);
            this.cboLookupType.ActiveChanged += new System.EventHandler(this.cboLookupType_ActiveChanged);
            // 
            // pgeLookup
            // 
            this.pgeLookup.BackColor = System.Drawing.SystemColors.Control;
            this.pgeLookup.Controls.Add(this.dgCodeLookupView);
            this.pgeLookup.Controls.Add(this.lblLookupTest);
            this.pgeLookup.Controls.Add(this.cboLookupType);
            this.pgeLookup.Controls.Add(this.txtLookupField);
            this.pgeLookup.Controls.Add(this.lblLookupDesc);
            this.pgeLookup.Location = new System.Drawing.Point(4, 24);
            this._res.SetLookup(this.pgeLookup, new FWBS.OMS.UI.Windows.ResourceLookupItem("LOOKUP", "Lookup", ""));
            this.pgeLookup.Name = "pgeLookup";
            this.pgeLookup.Size = new System.Drawing.Size(672, 427);
            this.pgeLookup.TabIndex = 5;
            this.pgeLookup.Text = "Lookup";
            // 
            // dgCodeLookupView
            // 
            this.dgCodeLookupView.AllowUserToAddRows = false;
            this.dgCodeLookupView.AllowUserToDeleteRows = false;
            this.dgCodeLookupView.AllowUserToResizeRows = false;
            this.dgCodeLookupView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgCodeLookupView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgCodeLookupView.CaptionLabel = this.lblLookupTest;
            this.dgCodeLookupView.CaptionText = "Test Return Value";
            this.dgCodeLookupView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgCodeLookupView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgCodeLookupView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgCodeLookupView.ColumnHeadersHeight = 36;
            this.dgCodeLookupView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgCodeLookupView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cdCodeColumn,
            this.cdDescColumn});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(3, 5, 5, 3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgCodeLookupView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgCodeLookupView.EnableHeadersVisualStyles = false;
            this.dgCodeLookupView.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(234)))));
            this.dgCodeLookupView.Location = new System.Drawing.Point(8, 135);
            this.dgCodeLookupView.MultiSelect = false;
            this.dgCodeLookupView.Name = "dgCodeLookupView";
            this.dgCodeLookupView.ReadOnly = true;
            this.dgCodeLookupView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgCodeLookupView.RowHeadersVisible = false;
            this.dgCodeLookupView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgCodeLookupView.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgCodeLookupView.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(224)))), ((int)(((byte)(242)))));
            this.dgCodeLookupView.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.dgCodeLookupView.RowTemplate.Height = 32;
            this.dgCodeLookupView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgCodeLookupView.Size = new System.Drawing.Size(654, 280);
            this.dgCodeLookupView.TabIndex = 4;
            // 
            // lblLookupTest
            // 
            this.lblLookupTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLookupTest.AutoEllipsis = true;
            this.lblLookupTest.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblLookupTest.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblLookupTest.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblLookupTest.Location = new System.Drawing.Point(8, 112);
            this._res.SetLookup(this.lblLookupTest, new FWBS.OMS.UI.Windows.ResourceLookupItem("TESTRETURNVALUE", "Test Return Value", ""));
            this.lblLookupTest.Name = "lblLookupTest";
            this.lblLookupTest.Padding = new System.Windows.Forms.Padding(5, 0, 0, 2);
            this.lblLookupTest.Size = new System.Drawing.Size(654, 23);
            this.lblLookupTest.TabIndex = 3;
            this.lblLookupTest.Text = "Test Return Value";
            this.lblLookupTest.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblLookupTest.UseMnemonic = false;
            // 
            // cdCodeColumn
            // 
            this.cdCodeColumn.DataPropertyName = "cdCode";
            this.cdCodeColumn.HeaderText = "Code";
            this.cdCodeColumn.Name = "cdCodeColumn";
            this.cdCodeColumn.ReadOnly = true;
            this.cdCodeColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cdCodeColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.cdCodeColumn.Width = 150;
            // 
            // cdDescColumn
            // 
            this.cdDescColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cdDescColumn.DataPropertyName = "cdDesc";
            this.cdDescColumn.HeaderText = "Description";
            this.cdDescColumn.Name = "cdDescColumn";
            this.cdDescColumn.ReadOnly = true;
            this.cdDescColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tabFields
            // 
            this.tabFields.Controls.Add(this.pgeCommon);
            this.tabFields.Controls.Add(this.pgePrompt);
            this.tabFields.Controls.Add(this.pgeLookup);
            this.tabFields.Controls.Add(this.pgeAssociate);
            this.tabFields.Controls.Add(this.pgeAppointment);
            this.tabFields.Controls.Add(this.pgeOther);
            this.tabFields.Controls.Add(this.pgeMisc);
            this.tabFields.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFields.Location = new System.Drawing.Point(0, 0);
            this.tabFields.Name = "tabFields";
            this.tabFields.SelectedIndex = 0;
            this.tabFields.Size = new System.Drawing.Size(680, 455);
            this.tabFields.TabIndex = 0;
            this.tabFields.SelectedIndexChanged += new System.EventHandler(this.tabFields_SelectedIndexChanged);
            // 
            // pnlHelp
            // 
            this.pnlHelp.Controls.Add(this.lblHelp);
            this.pnlHelp.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlHelp.Location = new System.Drawing.Point(5, 460);
            this.pnlHelp.Name = "pnlHelp";
            this.pnlHelp.Size = new System.Drawing.Size(680, 96);
            this.pnlHelp.TabIndex = 10;
            // 
            // lblHelp
            // 
            this.lblHelp.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblHelp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHelp.Location = new System.Drawing.Point(0, 0);
            this.lblHelp.Name = "lblHelp";
            this.lblHelp.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.lblHelp.Size = new System.Drawing.Size(680, 96);
            this.lblHelp.TabIndex = 0;
            this.lblHelp.Text = "One Line Information Line";
            this.lblHelp.Title = "Title Bar";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.tabFields);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(5, 5);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(680, 455);
            this.pnlMain.TabIndex = 12;
            // 
            // pnlContainer
            // 
            this.pnlContainer.Controls.Add(this.pnlMain);
            this.pnlContainer.Controls.Add(this.pnlHelp);
            this.pnlContainer.Controls.Add(this.pnlButtons);
            this.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContainer.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(5);
            this.pnlContainer.Size = new System.Drawing.Size(784, 561);
            this.pnlContainer.TabIndex = 0;
            // 
            // frmFields
            // 
            this.AcceptButton = this.btnCreate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.pnlContainer);
            this._res.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("frmFields", "Add Field To Document", ""));
            this.MinimizeBox = false;
            this.Name = "frmFields";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Field To Document";
            this.Activated += new System.EventHandler(this.frmFields_Activated);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmFields_Closing);
            this.Load += new System.EventHandler(this.frmFields_Load);
            this.Controls.SetChildIndex(this.pnlContainer, 0);
            this.pnlButtons.ResumeLayout(false);
            this.pgeCommon.ResumeLayout(false);
            this.pgeAssociate.ResumeLayout(false);
            this.pnlAssocLeft.ResumeLayout(false);
            this.pgeAppointment.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pgeOther.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pgePrompt.ResumeLayout(false);
            this.pgeMisc.ResumeLayout(false);
            this.pnlFields.ResumeLayout(false);
            this.pgeLookup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgCodeLookupView)).EndInit();
            this.tabFields.ResumeLayout(false);
            this.pnlHelp.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.pnlContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion


        #endregion

        #region Event Methods

        private void btnExpand_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (Convert.ToString(btnExpand.Tag) == "EXPAND")
                {
                    _expandSize = (this.WindowState == FormWindowState.Normal) ? this.Size : RestoreBounds.Size;
                    btnExpand.Tag = "SHRINK";
                    _res.SetLookup(btnExpand, new FWBS.OMS.UI.Windows.ResourceLookupItem("EXPAND", "E&xpand >>", ""));

                    label1.Visible = false;
                    btnCancel.Visible = false;

                    txtCode.CaptionWidth = 100;
                    lblFieldDesc.Visible = true;
                    pnlFields.Parent = pnlMain;

                    pnlHelp.Visible = false;
                    tabFields.Visible = false;

                    this.FormBorderStyle = FormBorderStyle.FixedDialog;
                    this.MaximizeBox = false;
                    txtCode.Focus();
                }
                else
                {
                    btnExpand.Tag = "EXPAND";
                    _res.SetLookup(btnExpand, new FWBS.OMS.UI.Windows.ResourceLookupItem("SHRINK", "&Shrink <<", ""));

                    label1.Visible = true;
                    btnCancel.Visible = true;

                    txtCode.CaptionWidth = 150;
                    lblFieldDesc.Visible = false;
                    pnlFields.Parent = pgeMisc;
                    pnlFields.BringToFront();

                    pnlHelp.Visible = Common.ConvertDef.ToBoolean(pnlHelp.Tag, false);
                    tabFields.Visible = true;

                    this.FormBorderStyle = FormBorderStyle.Sizable;
                    this.MaximizeBox = true;
                }

                if (DeviceDpi != 96)
                    eBase2.SetRTL(txtCode, (Control)txtCode.Control, LogicalToDeviceUnits(txtCode.CaptionWidth), null);

                ApplySize();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private Size _expandSize = Size.Empty;

        private Size _shrinkSize => LogicalToDeviceUnits(new Size(448, 120));

        private FormWindowState _expandWindowState = FormWindowState.Normal;

        private void ApplySize()
        {
            if (Convert.ToString(btnExpand.Tag) == "SHRINK")
            {
                _expandWindowState = this.WindowState;
                if (_expandWindowState != FormWindowState.Normal)
                    this.WindowState = FormWindowState.Normal;
            }
            else
            {
                if (this.WindowState != _expandWindowState)
                    this.WindowState = _expandWindowState;
            }
            this.Size = FormSize;
        }

        private Size FormSize
        {
            get
            {
                return Convert.ToString(btnExpand.Tag) == "SHRINK" 
                    ? _shrinkSize 
                    : _expandSize;
            }
        }

        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            base.OnDpiChanged(e);

            if (_expandSize != Size.Empty)
            {
                float scaleFactor = (float)e.DeviceDpiNew / e.DeviceDpiOld;
                _expandSize = new Size(Convert.ToInt32(_expandSize.Width * scaleFactor),
                    Convert.ToInt32(_expandSize.Height * scaleFactor)); 
            }
        }

        private void frmFields_Load(object sender, System.EventArgs e)
		{
			try
			{
				_fields.ExternalFilter = "fldhide = false";
				_fields.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.FieldCommon), null, null);
				_fields.SelectItem();
				if (_controlApp != null) _controlApp.RunCommand(_controlApp, "SYSTEM;SHOWFIELDCODES;TRUE");

				if (_fav.Count > 0)
				{
					if (_fav.Param1(0) == "SHRINK")
					{
						btnExpand.Tag = "EXPAND";
						btnExpand_Click(btnExpand, EventArgs.Empty);
					}
				}

                FWBS.OMS.EnquiryEngine.DataLists dataList = new FWBS.OMS.EnquiryEngine.DataLists("DSCLKPTYPESALL");
                cboLookupType.AddItem(dataList.Run() as DataTable);
            }
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void btnCreate_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (_controlApp == null)
				{
					DialogResult = DialogResult.OK;
				}
				else
                {
                    if (!this.UpdateKeys())
                    {
                        return;
                    }

                    string fld = SelectedField;
					
					txtCode.Value = "";
					if (tabFields.Visible == false)
						txtCode.Focus();
					
					if (fld != String.Empty && _controlApp is FWBS.OMS.Interfaces.ISecondStageMergeOMSApp)
					{
						if (AsSecondStageMergeField)
							((FWBS.OMS.Interfaces.ISecondStageMergeOMSApp)_controlApp).AddSecondStageMergeField(_controlApp, fld);
						else
							_controlApp.AddField(_controlApp, fld);
					}
					else if (fld != String.Empty  && _controlApp != null)
						_controlApp.AddField(_controlApp, fld);

					chkSSM.Checked = false;
                    PopulateMultiAssociates();
					PopulateMultiAppointments();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, ex);
			}

		}

        private void _fields_ItemHover(object sender, FWBS.OMS.UI.Windows.SearchItemHoverEventArgs e)
		{
			try
			{
				lblHelp.Text = Convert.ToString(e.ItemList["fldhelp"].Value);
				lblHelp.Title = Convert.ToString(e.ItemList["flddesc"].Value);
			}
			catch{}
			if (lblHelp.Text == String.Empty)
			{
				pnlHelp.Visible = false;
			}
		
			if (_fields.Visible)
				pnlHelp.Visible = (lblHelp.Text != String.Empty);
			else
				pnlHelp.Visible = false;
			pnlHelp.Tag = pnlHelp.Visible;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			Close();
		}

		private void frmFields_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				if (_controlApp != null) _controlApp.RunCommand(_controlApp, "SYSTEM;SHOWFIELDCODES;FALSE");
			
				if (_fav.Count > 0)
				{
					if (_fav.Param1(0) != Convert.ToString(btnExpand.Tag))
					{
						_fav.Param1(0, Convert.ToString(btnExpand.Tag));
						_fav.Update();
					}
				}
				else if (Convert.ToString(btnExpand.Tag) == "SHRINK")
				{
					_fav.AddFavourite("STYLE", "", Convert.ToString(btnExpand.Tag));
					_fav.Update();
				}
			}
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}


		private void frmFields_Activated(object sender, System.EventArgs e)
		{
            try
            {
                //DM - Captures the exception that may occur when a OMS App document no longer exists.
                PopulateMultiAssociates();
                PopulateMultiAppointments();
            }
            catch
            {
            }
		}

        private bool ValidateLookupFieldCode(string field)
        {
            if (!string.IsNullOrEmpty(field))
            {
                using (DataTable allFields = FieldParser.GetFields("all"))
                {
                    if (allFields.Select($"fldName = '{Common.Data.SQLRoutines.ValidateSQL(field)}'").Length != 0)
                        return true;
                }
            }

            var message = Session.CurrentSession.Resources.GetMessage("INVALIDFIELD", "The field code is missing or invalid.", "").Text;
            MessageBox.Show(message, OMS.Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            return false;
        }

        private bool ValidateLookupFieldType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                var message = Session.CurrentSession.Resources.GetMessage("INVALIDLKPTYPE", "The lookup type is not specified.", "").Text;
                MessageBox.Show(message, OMS.Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }

        private void cboLookupType_ActiveChanged(object sender, EventArgs e)
        {
            cdCodeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            cdDescColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            dgCodeLookupView.SortedColumn = null;

            string type = cboLookupType.SelectedValue as string;
            if (!string.IsNullOrEmpty(type))
            {
                dgCodeLookupView.DataSource = CodeLookup.GetLookups(type);
                cdCodeColumn.SortMode = DataGridViewColumnSortMode.Automatic;
                cdDescColumn.SortMode = DataGridViewColumnSortMode.Automatic;
            }
            else
            {
                dgCodeLookupView.DataSource = null;
            }
        }

        private void cboLookupType_DoesNotExist(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cboLookupType.SelectedIndex = 0;
            e.Cancel = true;
        }

        private void tabFields_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			try
			{
				pnlHelp.Tag = pnlHelp.Visible = false;
				switch(tabFields.SelectedIndex)
				{
					case 0:		//COMMON
						break;
					case 1:		//PROMPT
						break;
					case 2:		//LOOKUPS
						break;
					case 3:		//ASSOCIATE
						if (schAssocFields.SearchList == null) 
						{
							schAssocFields.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.FieldAssociates), null, null);
							schAssocFields.TypeSelectorVisible = false;

                            FWBS.OMS.EnquiryEngine.DataLists dl1 = new FWBS.OMS.EnquiryEngine.DataLists("DSCONTTYPEALL");
                            cboContType.ReadOnly = false;
                            cboContType.AddItem(dl1.Run() as System.Data.DataTable);
                            FWBS.OMS.EnquiryEngine.DataLists dl2 = new FWBS.OMS.EnquiryEngine.DataLists("DSASSOCTYPE_ALL");
							cboAssocType.AddItem(dl2.Run() as System.Data.DataTable);
							cboContType_ActiveChanged(null, EventArgs.Empty);
							schAssocFields.SelectItem();
						}

						break;
					case 4:		//APPOINTMENT
						if (schAppFields.SearchList == null) 
						{
							schAppFields.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.FieldAppointments), null, null);
							schAppFields.TypeSelectorVisible = false;
							schAppFields.SelectItem();
						}
						break;
					case 5:		//OTHER
						cboOthList.AddItem(FWBS.OMS.SearchEngine.SearchList.GetSearchLists(), "schcode", "schdesc");
						cboOthList_ActiveChanged(cboOthList, EventArgs.Empty);
						break;
				}

				tabFields.SelectedTab.SelectNextControl(tabFields.SelectedTab.Controls[0],true, true, true, true);
			}
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void cboOthList_ActiveChanged(object sender, System.EventArgs e)
		{
			try
			{
				string code = Convert.ToString(cboOthList.Value);
				PopulateMultiOther();
				if (code != "" || schOthFields.SearchList == null || schOthFields.SearchList.Code != code)
				{
					Common.KeyValueCollection pars = new Common.KeyValueCollection();
					pars.Add("CODE", code);
					schOthFields.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.FieldExtendedData), null, pars);
					schOthFields.Search();
				}
				
			}
			catch(Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void _fields_SearchCompleted(object sender, SearchCompletedEventArgs e)
		{
			_fields.ExternalFilter = "fldhide = false";
		}

		private void cboContType_ActiveChanged(object sender, System.EventArgs e)
		{
			string filter = "conttype is null";
			if (Convert.ToString(cboContType.Value) != String.Empty)
				filter = "conttype = '" + Common.SQLRoutines.RemoveRubbish(Convert.ToString(cboContType.Value)) + "'";

			cboAssocType.Filter(filter);
			cboAssocType.Value = DBNull.Value;
		}

        private void cboAssocKey_ActiveChanged(object sender, EventArgs e)
        {  
            try
            {
                if (Session.CurrentSession.IsConnected)
                {
                    var connection = Session.CurrentSession.CurrentConnection;
                    DataTableExecuteParameters pars = new DataTableExecuteParameters
                    {
                        CommandType = CommandType.Text,
                        Sql = "SELECT cltypeCode, assocType, precKeyMessage FROM dbPrecedentKeys WHERE precKey = @precKey AND keySection = @keySection"
                    };
                    pars.Parameters.Add(connection.CreateParameter("@precKey", this.cboAssocKey.Value));
                    pars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.Associate));

                    DataTable dtbl = connection.Execute(pars);
                    if (dtbl.Rows.Count > 0)
                    {
                        this.cboContType.Value = dtbl.Rows[0]["cltypeCode"].ToString();
                        this.cboAssocType.Value = dtbl.Rows[0]["assocType"].ToString();
                        this.txtAssocMessage.Value = dtbl.Rows[0]["precKeyMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboAppKey_ActiveChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session.CurrentSession.IsConnected)
                {
                    var connection = Session.CurrentSession.CurrentConnection;
                    DataTableExecuteParameters pars = new DataTableExecuteParameters
                    {
                        CommandType = CommandType.Text,
                        Sql = "SELECT precKeyMessage FROM dbPrecedentKeys WHERE precKey = @precKey AND keySection = @keySection"
                    };
                    pars.Parameters.Add(connection.CreateParameter("@precKey", this.cboAppKey.Value));
                    pars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.Appointment));

                    DataTable dtbl = connection.Execute(pars);
                    if (dtbl.Rows.Count > 0)
                    {
                        this.txtAppMessage.Value = dtbl.Rows[0]["precKeyMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cboOthKey_ActiveChanged(object sender, EventArgs e)
        {
            try
            {
                if (Session.CurrentSession.IsConnected)
                {
                    var connection = Session.CurrentSession.CurrentConnection;
                    DataTableExecuteParameters pars = new DataTableExecuteParameters
                    {
                        CommandType = CommandType.Text,
                        Sql = "SELECT precKeyMessage FROM dbPrecedentKeys WHERE precKey = @precKey AND keySection = @keySection"
                    };
                    pars.Parameters.Add(connection.CreateParameter("@precKey", this.cboOthKey.Value));
                    pars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.OtherList));

                    DataTable dtbl = connection.Execute(pars);
                    if (dtbl.Rows.Count > 0)
                    {
                        this.txtOthMessage.Value = dtbl.Rows[0]["precKeyMessage"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Methods

        private bool IsKeyValid(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show(Session.CurrentSession.Resources.GetResource("KEYBLANK", "The key cannot be blank.", "").Text);
                return false;
            }

            if (key.Length > 30)
            {
                MessageBox.Show(Session.CurrentSession.Resources.GetResource("KEY30", "The key cannot be larger than 30 characters.", "").Text);
                return false;
            }

            return true;
        }

        private bool UpdateKeys()
        {
            bool updateCheck = true;

            switch (this.tabFields.SelectedIndex)
            {
                case 0:     //COMMON
                case 1:     //PROMPT
                case 2:     //LOOKUPS
                    break;
                case 3:     //ASSOCIATE
                    updateCheck = this.UpdateAssociateKeys();
                    break;
                case 4:     //APPOINTMENT
                    updateCheck = this.UpdateAppointmentKeys();
                    break;
                case 5:     //OTHER
                    updateCheck = this.UpdateOtherListKeys();
                    break;
            }

            return updateCheck;
        }

        private bool IsAssocKeyChanged(DataRow keyRow)
        {
            return this.cboAssocType.Value.ToString() != keyRow["assocType"].ToString()
                   || this.cboContType.Value.ToString() != keyRow["cltypeCode"].ToString()
                   || this.txtAssocMessage.Value.ToString() != keyRow["precKeyMessage"].ToString();
        }

        private bool UpdateAssociateKeys()
        {
            string selectedValue = this.cboAssocKey.Value.ToString();
            string cboAssocKeyValue = CurrentKeyExists(this.cboAssocKey, selectedValue) ? selectedValue : selectedValue.Trim();

            if (cboAssocKeyValue != selectedValue)
                this.cboAssocKey.DisplayValue = cboAssocKeyValue;
            if (!IsKeyValid(cboAssocKeyValue))
                return false;

            var connection = Session.CurrentSession.CurrentConnection;
            ExecuteParameters pars = new ExecuteParameters { CommandType = CommandType.Text };
            pars.Parameters.Add(connection.CreateParameter("@precKey", cboAssocKeyValue));
            pars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.Associate));
            pars.Parameters.Add(connection.CreateParameter("@cltypeCode", this.cboContType.Value));
            pars.Parameters.Add(connection.CreateParameter("@assocType", this.cboAssocType.Value));
            pars.Parameters.Add(connection.CreateParameter("@precKeyMessage", this.txtAssocMessage.Value));

            if (!CurrentKeyExists(this.cboAssocKey, cboAssocKeyValue))
            {
                pars.Sql = "IF NOT EXISTS ( SELECT @precKey, @keySection INTERSECT SELECT precKey, KeySection FROM dbo.dbPrecedentKeys ) BEGIN INSERT dbo.dbPrecedentKeys(precKey, KeySection, cltypeCode, assocType, precKeyMessage) VALUES(@precKey, @keySection, @cltypeCode, @assocType, @precKeyMessage) END";
                connection.Execute(pars);
            }
            else
            {
                DataTableExecuteParameters dtpars = new DataTableExecuteParameters
                {
                    CommandType = CommandType.Text,
                    Sql = "SELECT cltypeCode, assocType, precKeyMessage FROM dbPrecedentKeys WHERE precKey = @precKey AND keySection = @keySection"
                };
                dtpars.Parameters.Add(connection.CreateParameter("@precKey", cboAssocKeyValue));
                dtpars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.Associate));
                DataTable dtbl = connection.Execute(dtpars);

                if (dtbl.Rows.Count > 0 && IsAssocKeyChanged(dtbl.Rows[0]))
                {
                    if (MessageBox.Show(
                            Session.CurrentSession.Resources.GetResource(
                                "KEYEXISTS",
                                "The key %1% already exists. Would you like to update it?",
                                "",
                                cboAssocKeyValue).Text,
                            Session.CurrentSession.Resources.GetResource("OMS", "OMS", "").Text,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        pars.Sql = "UPDATE dbo.dbPrecedentKeys SET cltypeCode = @cltypeCode, assocType = @assocType, precKeyMessage = @precKeyMessage WHERE precKey = @precKey AND keySection = @keySection";
                        connection.Execute(pars);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private bool UpdateAppointmentKeys()
        {
            string selectedValue = this.cboAppKey.Value.ToString();
            string cboAppKeyValue = CurrentKeyExists(this.cboAppKey, selectedValue) ? selectedValue : selectedValue.Trim();

            if (cboAppKeyValue != selectedValue)
                this.cboAppKey.DisplayValue = cboAppKeyValue;
            if (!IsKeyValid(cboAppKeyValue))
                return false;

            var connection = Session.CurrentSession.CurrentConnection;
            ExecuteParameters pars = new ExecuteParameters { CommandType = CommandType.Text };
            pars.Parameters.Add(connection.CreateParameter("@precKey", cboAppKeyValue));
            pars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.Appointment));
            pars.Parameters.Add(connection.CreateParameter("@precKeyMessage", this.txtAppMessage.Value));

            if (!CurrentKeyExists(this.cboAppKey, cboAppKeyValue))
            {
                pars.Sql = "IF NOT EXISTS ( SELECT @precKey, @keySection INTERSECT SELECT precKey, KeySection FROM dbo.dbPrecedentKeys ) BEGIN INSERT dbo.dbPrecedentKeys(precKey, KeySection, precKeyMessage) VALUES(@precKey, @keySection, @precKeyMessage) END";
                connection.Execute(pars);
            }
            else
            {
                DataTableExecuteParameters dtpars = new DataTableExecuteParameters
                {
                    CommandType = CommandType.Text,
                    Sql = "SELECT precKeyMessage FROM dbPrecedentKeys WHERE precKey = @precKey AND keySection = @keySection"
                };
                dtpars.Parameters.Add(connection.CreateParameter("@precKey", cboAppKeyValue));
                dtpars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.Appointment));
                DataTable dtbl = connection.Execute(dtpars);

                if (dtbl.Rows.Count > 0 && this.txtAppMessage.Value.ToString() != dtbl.Rows[0]["precKeyMessage"].ToString())
                {
                    if (MessageBox.Show(
                            Session.CurrentSession.Resources.GetResource(
                                "KEYEXISTS",
                                "The key %1% already exists. Would you like to update it?",
                                "",
                                cboAppKeyValue).Text,
                            Session.CurrentSession.Resources.GetResource("OMS", "OMS", "").Text,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        pars.Sql = "UPDATE dbo.dbPrecedentKeys SET precKeyMessage = @precKeyMessage WHERE precKey = @precKey AND keySection = @keySection";
                        connection.Execute(pars);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool UpdateOtherListKeys()
        {
            string selectedValue = this.cboOthKey.Value.ToString();
            string cboOthKeyValue = CurrentKeyExists(this.cboOthKey, selectedValue) ? selectedValue : selectedValue.Trim();

            if (cboOthKeyValue != selectedValue)
                this.cboOthKey.DisplayValue = cboOthKeyValue;
            if (!IsKeyValid(cboOthKeyValue))
                return false;

            var connection = Session.CurrentSession.CurrentConnection;
            ExecuteParameters pars = new ExecuteParameters { CommandType = CommandType.Text };
            pars.Parameters.Add(connection.CreateParameter("@precKey", cboOthKeyValue));
            pars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.OtherList));
            pars.Parameters.Add(connection.CreateParameter("@listType", this.cboOthList.Value));
            pars.Parameters.Add(connection.CreateParameter("@precKeyMessage", this.txtOthMessage.Value));

            if (!CurrentKeyExists(this.cboOthKey, cboOthKeyValue) && !IsKeyExistInDataBase(cboOthKeyValue, KeySection.OtherList))
            {
                pars.Sql = "IF NOT EXISTS ( SELECT @precKey, @keySection INTERSECT SELECT precKey , KeySection FROM dbo.dbPrecedentKeys ) BEGIN INSERT dbo.dbPrecedentKeys(precKey, KeySection, listType, precKeyMessage) VALUES(@precKey, @keySection, @listType, @precKeyMessage) END";
                connection.Execute(pars);
            }
            else
            {
                DataTableExecuteParameters dtpars = new DataTableExecuteParameters
                {
                    CommandType = CommandType.Text,
                    Sql = "SELECT listType, precKeyMessage FROM dbPrecedentKeys WHERE precKey = @precKey AND keySection = @keySection"
                };
                dtpars.Parameters.Add(connection.CreateParameter("@precKey", cboOthKeyValue));
                dtpars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.OtherList));
                DataTable dtbl = connection.Execute(dtpars);

                if (dtbl.Rows.Count > 0 && 
                    (this.txtOthMessage.Value.ToString() != dtbl.Rows[0]["precKeyMessage"].ToString() 
                     || this.cboOthList.Value.ToString() != dtbl.Rows[0]["listType"].ToString()))
                {
                    if (MessageBox.Show(
                            Session.CurrentSession.Resources.GetResource(
                                "KEYEXISTS",
                                "The key %1% already exists. Would you like to update it?",
                                "",
                                cboOthKeyValue).Text,
                            Session.CurrentSession.Resources.GetResource("OMS", "OMS", "").Text,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        pars.Sql = "UPDATE dbo.dbPrecedentKeys SET listType = @listType, precKeyMessage = @precKeyMessage WHERE precKey = @precKey AND keySection = @keySection";
                        connection.Execute(pars);
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void PopulateMultiAssociates()
		{
			if (_controlApp != null)
			{
				cboAssocKey.Clear();

                var connection = Session.CurrentSession.CurrentConnection;
                DataTableExecuteParameters pars = new DataTableExecuteParameters
                {
                    CommandType = CommandType.Text,
                    Sql = "SELECT DISTINCT precKey FROM dbPrecedentKeys WHERE keySection = @keySection ORDER BY precKey"
                };
                pars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.Associate));

                DataTable dtbl = connection.Execute(pars);
                foreach (DataRow control in dtbl.Rows)
                {
                    cboAssocKey.AddItem(control["precKey"], control["precKey"].ToString());
                }
            }
		}

		private void PopulateMultiAppointments()
		{
			if (_controlApp != null)
			{
				cboAppKey.Clear();

                var connection = Session.CurrentSession.CurrentConnection;
                DataTableExecuteParameters pars = new DataTableExecuteParameters
                {
                    CommandType = CommandType.Text,
                    Sql = "SELECT DISTINCT precKey FROM dbPrecedentKeys WHERE keySection = @keySection ORDER BY precKey"
                };
                pars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.Appointment));

                DataTable dtbl = connection.Execute(pars);
                foreach (DataRow control in dtbl.Rows)
                {
                    cboAppKey.AddItem(control["precKey"], control["precKey"].ToString());
                }
            }
		}

		private void PopulateMultiOther()
		{
			if (_controlApp != null)
			{
				cboOthKey.Clear();

                var connection = Session.CurrentSession.CurrentConnection;
                DataTableExecuteParameters pars = new DataTableExecuteParameters
                {
                    CommandType = CommandType.Text,
                    Sql = "SELECT DISTINCT precKey FROM dbPrecedentKeys WHERE keySection = @keySection AND listType = @listType ORDER BY precKey"
                };
                pars.Parameters.Add(connection.CreateParameter("@keySection", (byte)KeySection.OtherList));
                pars.Parameters.Add(connection.CreateParameter("@listType", cboOthList.Value));

                DataTable dtbl = connection.Execute(pars);
                foreach (DataRow control in dtbl.Rows)
                {
                    cboOthKey.AddItem(control["precKey"], control["precKey"].ToString());
                }
			}
		}

        private bool CurrentKeyExists(eXPComboBox keyComboBox, string currentKey)
        {
            foreach (var item in keyComboBox.Items)
            {
                if (item.ToString() == currentKey)
                    return true;
            }

            return false;
        }

        private bool IsKeyExistInDataBase(string precKey, KeySection keySection)
        {
            var connection = Session.CurrentSession.CurrentConnection;
            ExecuteParameters pars = new ExecuteParameters
            {
                CommandType = CommandType.Text,
                Sql = "SELECT TOP 1 1 FROM dbPrecedentKeys WHERE precKey = @precKey AND keySection = @keySection"
            };
            pars.Parameters.Add(connection.CreateParameter("@precKey", precKey));
            pars.Parameters.Add(connection.CreateParameter("@keySection", (byte)keySection));

            object result = connection.ExecuteScalar(pars);
            return (result != null);
        }

        #endregion

        #region Properties

		/// <summary>
		/// Gets the currently selected field.
		/// </summary>
		public string SelectedField
		{
			get
			{
				if (Convert.ToString(btnExpand.Tag) == "EXPAND")
				{
					switch(tabFields.SelectedIndex)
					{
						case 0:		//COMMON
							if (_fields.SelectRowItem())
								return FieldParser.CreateField(Convert.ToString(_fields.CurrentItem()[0].Value));
							break;
						case 1:		//PROMPT
							return FieldParser.CreatePromptField(Convert.ToString(txtPromptMessage.Value), chkPromptRequired.Checked);
						case 2:     //LOOKUPS
							string field = Convert.ToString(txtLookupField.Value).Trim();
							string type = cboLookupType.Value as string;
							if (ValidateLookupFieldCode(field) && ValidateLookupFieldType(type))
								return FieldParser.CreateLookupField(field, type);
							break;
						case 3:		//ASSOCIATE
							if (schAssocFields.SelectRowItem())
								return FieldParser.CreateAssociateField(Convert.ToString(cboContType.Value), Convert.ToString(cboAssocType.Value), Convert.ToString(cboAssocKey.Value), Convert.ToString(txtAssocMessage.Value), Convert.ToString(schAssocFields.CurrentItem()[0].Value));
							break;
						case 4:		//APPOINTMENT
							if (schAppFields.SelectRowItem())
								return FieldParser.CreateAppointmentField(Convert.ToString(cboAppKey.Value), Convert.ToString(txtAppMessage.Value), Convert.ToString(schAppFields.CurrentItem()[0].Value));
							break;
						case 5:		//OTHER
							if (schOthFields.SelectRowItem())
								return FieldParser.CreateExtendedListField(Convert.ToString(cboOthList.Value), Convert.ToString(cboOthFilter.Value), Convert.ToString(cboOthKey.Value), Convert.ToString(txtOthMessage.Value), Convert.ToString(schOthFields.CurrentItem()[0].Value));
							break;
						case 6:		//MISC
							return FieldParser.CreateField(Convert.ToString(txtCode.Value));
						default:
							return "";
					}
					return "";
				}
				else
					return FieldParser.CreateField(Convert.ToString(txtCode.Value));

			}
		}

		/// <summary>
		/// A flag that indicates that the chosen field is to be added as a second stage merge field.
		/// </summary>
		public bool AsSecondStageMergeField
		{
			get
			{
				return chkSSM.Checked;
			}
		}

        #endregion

    }
}
