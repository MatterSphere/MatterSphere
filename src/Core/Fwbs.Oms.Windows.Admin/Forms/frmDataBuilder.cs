using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.SourceEngine;

namespace FWBS.OMS.UI.Windows.Design
{
    /// <summary>
    /// Summary description for frmDataBuilder.
    /// </summary>
    public class frmDataBuilder : FWBS.OMS.UI.Windows.BaseForm
	{
		#region Fields
        private readonly char[] ClosingBits = new char[1] { ')' };
		public DataBuilder Value;
		private DataBuilderMode _mode;
		private bool _updating = false;
		private ArrayList _items = null;
		private SourceType _exclude;
		private bool _grpparameters = true;
		private bool _yestowarnings = false;
		#endregion

		#region Control Declares
		private System.Windows.Forms.Panel pnlTools;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.GroupBox grpParameters;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.Panel panel1;
		private FWBS.OMS.UI.Windows.DataGridEx dataGrid1;
		private System.Windows.Forms.GroupBox grpSQLCode;
		private System.Windows.Forms.TextBox txtSQL;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.ListBox lstParameters;
		private System.Windows.Forms.PropertyGrid ParameterProperties;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnGet;
		private System.Windows.Forms.TextBox txtSource;
		private System.Windows.Forms.ComboBox cmbSourceType;
		private System.Windows.Forms.Button btnTest;
		private System.Windows.Forms.Panel pnlSqlBrowse;
		private System.Windows.Forms.Button btnChangeData;
		private System.Windows.Forms.Panel pnlHelp;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.TextBox txtInfoObject;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ComboBox cmbMethods;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.TextBox labInfo;
		private System.Windows.Forms.Button btnPopulate;
		private System.Windows.Forms.Panel pnlPopulate;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.MenuItem mnuRemove;
		private System.Windows.Forms.MenuItem mnuRemoveAll;
		private System.Windows.Forms.Panel pnlClassObject;
		private System.Windows.Forms.Panel pnlSQL;
		private System.Windows.Forms.Panel pnlTables;
		private System.Windows.Forms.ComboBox cmbTables;
		private System.Windows.Forms.Panel pnlSQLCode;
		private System.Windows.Forms.Label labWhere;
		private System.Windows.Forms.TextBox txtWhere;
		protected FWBS.OMS.UI.Windows.ResourceLookup resourceLookup1;
		private System.Windows.Forms.Splitter splitter3;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Button btnUnlock;
		private System.ComponentModel.IContainer components;
		#endregion

		#region Constructors
		private frmDataBuilder()
		{
		}
			
		public frmDataBuilder(DataBuilder value) : this(value,DataBuilderMode.ListMode)
		{
		}

		public frmDataBuilder(DataBuilder value, DataBuilderMode mode)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			_mode = mode;
            try
            {
                cmbTables.DataSource = EnquiryEngine.Enquiry.GetTableNames();
                cmbTables.DisplayMember = "tblname";
                cmbTables.ValueMember = "tblname";
                _items = FWBS.Common.EnumListItem.EnumToList(typeof(SourceType));
                cmbSourceType.DataSource = _items;
                if (value == null)
                    Value = new DataBuilder();
                else
                {
                    Value = value;
                    cmbSourceType.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
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
		/// the contents of this method with the code Valueor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataBuilder));
            this.pnlTools = new System.Windows.Forms.Panel();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.grpParameters = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ParameterProperties = new System.Windows.Forms.PropertyGrid();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lstParameters = new System.Windows.Forms.ListBox();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.mnuRemove = new System.Windows.Forms.MenuItem();
            this.mnuRemoveAll = new System.Windows.Forms.MenuItem();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grpSQLCode = new System.Windows.Forms.GroupBox();
            this.pnlSQLCode = new System.Windows.Forms.Panel();
            this.pnlSQL = new System.Windows.Forms.Panel();
            this.txtSQL = new System.Windows.Forms.TextBox();
            this.pnlSqlBrowse = new System.Windows.Forms.Panel();
            this.btnChangeData = new System.Windows.Forms.Button();
            this.pnlTables = new System.Windows.Forms.Panel();
            this.txtWhere = new System.Windows.Forms.TextBox();
            this.labWhere = new System.Windows.Forms.Label();
            this.cmbTables = new System.Windows.Forms.ComboBox();
            this.pnlClassObject = new System.Windows.Forms.Panel();
            this.cmbMethods = new System.Windows.Forms.ComboBox();
            this.pnlPopulate = new System.Windows.Forms.Panel();
            this.btnPopulate = new System.Windows.Forms.Button();
            this.labInfo = new System.Windows.Forms.TextBox();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.btnGet = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.cmbSourceType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGrid1 = new FWBS.OMS.UI.Windows.DataGridEx();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlHelp = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.txtInfoObject = new System.Windows.Forms.TextBox();
            this.resourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.pnlTools.SuspendLayout();
            this.grpParameters.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grpSQLCode.SuspendLayout();
            this.pnlSQLCode.SuspendLayout();
            this.pnlSQL.SuspendLayout();
            this.pnlSqlBrowse.SuspendLayout();
            this.pnlTables.SuspendLayout();
            this.pnlClassObject.SuspendLayout();
            this.pnlPopulate.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).BeginInit();
            this.pnlHelp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTools
            // 
            this.pnlTools.Controls.Add(this.btnTest);
            this.pnlTools.Controls.Add(this.btnCancel);
            this.pnlTools.Controls.Add(this.btnOK);
            this.pnlTools.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlTools.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlTools.Location = new System.Drawing.Point(711, 5);
            this.pnlTools.Name = "pnlTools";
            this.pnlTools.Padding = new System.Windows.Forms.Padding(8, 0, 0, 0);
            this.pnlTools.Size = new System.Drawing.Size(84, 456);
            this.pnlTools.TabIndex = 4;
            // 
            // btnTest
            // 
            this.btnTest.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnTest.Location = new System.Drawing.Point(8, 432);
            this.resourceLookup1.SetLookup(this.btnTest, new FWBS.OMS.UI.Windows.ResourceLookupItem("Test", "Test", ""));
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(76, 24);
            this.btnTest.TabIndex = 6;
            this.btnTest.Text = "Test";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(8, 28);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnCancel", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(76, 24);
            this.btnCancel.TabIndex = 3;
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
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "&OK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // grpParameters
            // 
            this.grpParameters.Controls.Add(this.panel3);
            this.grpParameters.Dock = System.Windows.Forms.DockStyle.Left;
            this.grpParameters.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpParameters.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.grpParameters.Location = new System.Drawing.Point(5, 5);
            this.resourceLookup1.SetLookup(this.grpParameters, new FWBS.OMS.UI.Windows.ResourceLookupItem("Parameters", "Parameters", ""));
            this.grpParameters.Name = "grpParameters";
            this.grpParameters.Size = new System.Drawing.Size(203, 456);
            this.grpParameters.TabIndex = 6;
            this.grpParameters.TabStop = false;
            this.grpParameters.Text = "Parameters";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.ParameterProperties);
            this.panel3.Controls.Add(this.splitter3);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 19);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(5);
            this.panel3.Size = new System.Drawing.Size(197, 434);
            this.panel3.TabIndex = 7;
            // 
            // ParameterProperties
            // 
            this.ParameterProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParameterProperties.HelpBackColor = System.Drawing.Color.White;
            this.ParameterProperties.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.ParameterProperties.Location = new System.Drawing.Point(5, 205);
            this.ParameterProperties.Name = "ParameterProperties";
            this.ParameterProperties.Size = new System.Drawing.Size(187, 224);
            this.ParameterProperties.TabIndex = 4;
            this.ParameterProperties.ToolbarVisible = false;
            this.ParameterProperties.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.ParameterProperties_PropertyValueChanged);
            this.ParameterProperties.SelectedObjectsChanged += new System.EventHandler(this.ParameterProperties_SelectedObjectsChanged);
            // 
            // splitter3
            // 
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter3.Location = new System.Drawing.Point(5, 200);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(187, 5);
            this.splitter3.TabIndex = 15;
            this.splitter3.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lstParameters);
            this.panel5.Controls.Add(this.panel4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(187, 195);
            this.panel5.TabIndex = 15;
            // 
            // lstParameters
            // 
            this.lstParameters.ContextMenu = this.contextMenu1;
            this.lstParameters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstParameters.Location = new System.Drawing.Point(0, 0);
            this.lstParameters.Name = "lstParameters";
            this.lstParameters.Size = new System.Drawing.Size(187, 163);
            this.lstParameters.TabIndex = 2;
            this.lstParameters.SelectedIndexChanged += new System.EventHandler(this.lstParameters_SelectedIndexChanged);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuRemove,
            this.mnuRemoveAll});
            // 
            // mnuRemove
            // 
            this.mnuRemove.Index = 0;
            this.mnuRemove.Text = "Remove";
            this.mnuRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // mnuRemoveAll
            // 
            this.mnuRemoveAll.Index = 1;
            this.mnuRemoveAll.Text = "Remove All";
            this.mnuRemoveAll.Click += new System.EventHandler(this.mnuRemoveAll_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnRemove);
            this.panel4.Controls.Add(this.btnAdd);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 163);
            this.panel4.Name = "panel4";
            this.panel4.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.panel4.Size = new System.Drawing.Size(187, 32);
            this.panel4.TabIndex = 3;
            // 
            // btnRemove
            // 
            this.btnRemove.ContextMenu = this.contextMenu1;
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRemove.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRemove.Location = new System.Drawing.Point(112, 5);
            this.resourceLookup1.SetLookup(this.btnRemove, new FWBS.OMS.UI.Windows.ResourceLookupItem("Remove", "Remove", ""));
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(75, 22);
            this.btnRemove.TabIndex = 1;
            this.btnRemove.Text = "#Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Location = new System.Drawing.Point(0, 5);
            this.resourceLookup1.SetLookup(this.btnAdd, new FWBS.OMS.UI.Windows.ResourceLookupItem("Add", "Add", ""));
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 22);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "#Add";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grpSQLCode);
            this.panel1.Controls.Add(this.splitter2);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.dataGrid1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.panel1.Location = new System.Drawing.Point(214, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(497, 456);
            this.panel1.TabIndex = 7;
            // 
            // grpSQLCode
            // 
            this.grpSQLCode.Controls.Add(this.pnlSQLCode);
            this.grpSQLCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSQLCode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.grpSQLCode.Location = new System.Drawing.Point(0, 30);
            this.resourceLookup1.SetLookup(this.grpSQLCode, new FWBS.OMS.UI.Windows.ResourceLookupItem("SQLCode", "SQL Code", ""));
            this.grpSQLCode.Name = "grpSQLCode";
            this.grpSQLCode.Size = new System.Drawing.Size(497, 161);
            this.grpSQLCode.TabIndex = 9;
            this.grpSQLCode.TabStop = false;
            this.grpSQLCode.Text = "SQL Code";
            // 
            // pnlSQLCode
            // 
            this.pnlSQLCode.Controls.Add(this.pnlSQL);
            this.pnlSQLCode.Controls.Add(this.pnlTables);
            this.pnlSQLCode.Controls.Add(this.pnlClassObject);
            this.pnlSQLCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSQLCode.Location = new System.Drawing.Point(3, 19);
            this.pnlSQLCode.Name = "pnlSQLCode";
            this.pnlSQLCode.Padding = new System.Windows.Forms.Padding(7);
            this.pnlSQLCode.Size = new System.Drawing.Size(491, 139);
            this.pnlSQLCode.TabIndex = 0;
            // 
            // pnlSQL
            // 
            this.pnlSQL.Controls.Add(this.txtSQL);
            this.pnlSQL.Controls.Add(this.pnlSqlBrowse);
            this.pnlSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSQL.Location = new System.Drawing.Point(7, 7);
            this.pnlSQL.Name = "pnlSQL";
            this.pnlSQL.Size = new System.Drawing.Size(477, 125);
            this.pnlSQL.TabIndex = 16;
            this.pnlSQL.Visible = false;
            // 
            // txtSQL
            // 
            this.txtSQL.AcceptsReturn = true;
            this.txtSQL.AcceptsTab = true;
            this.txtSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSQL.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtSQL.Location = new System.Drawing.Point(0, 27);
            this.txtSQL.Multiline = true;
            this.txtSQL.Name = "txtSQL";
            this.txtSQL.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSQL.Size = new System.Drawing.Size(477, 98);
            this.txtSQL.TabIndex = 1;
            this.txtSQL.WordWrap = false;
            this.txtSQL.TextChanged += new System.EventHandler(this.txtSQL_TextChanged);
            // 
            // pnlSqlBrowse
            // 
            this.pnlSqlBrowse.Controls.Add(this.btnChangeData);
            this.pnlSqlBrowse.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSqlBrowse.Location = new System.Drawing.Point(0, 0);
            this.pnlSqlBrowse.Name = "pnlSqlBrowse";
            this.pnlSqlBrowse.Padding = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.pnlSqlBrowse.Size = new System.Drawing.Size(477, 27);
            this.pnlSqlBrowse.TabIndex = 2;
            // 
            // btnChangeData
            // 
            this.btnChangeData.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnChangeData.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnChangeData.Location = new System.Drawing.Point(0, 0);
            this.resourceLookup1.SetLookup(this.btnChangeData, new FWBS.OMS.UI.Windows.ResourceLookupItem("Browse", "Browse", ""));
            this.btnChangeData.Name = "btnChangeData";
            this.btnChangeData.Size = new System.Drawing.Size(78, 23);
            this.btnChangeData.TabIndex = 0;
            this.btnChangeData.Text = "Browse";
            this.btnChangeData.Click += new System.EventHandler(this.btnChangeData_Click);
            // 
            // pnlTables
            // 
            this.pnlTables.Controls.Add(this.txtWhere);
            this.pnlTables.Controls.Add(this.labWhere);
            this.pnlTables.Controls.Add(this.cmbTables);
            this.pnlTables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTables.Location = new System.Drawing.Point(7, 7);
            this.pnlTables.Name = "pnlTables";
            this.pnlTables.Size = new System.Drawing.Size(477, 125);
            this.pnlTables.TabIndex = 17;
            this.pnlTables.Visible = false;
            // 
            // txtWhere
            // 
            this.txtWhere.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtWhere.Font = new System.Drawing.Font("Consolas", 9F);
            this.txtWhere.Location = new System.Drawing.Point(0, 45);
            this.txtWhere.Multiline = true;
            this.txtWhere.Name = "txtWhere";
            this.txtWhere.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtWhere.Size = new System.Drawing.Size(477, 80);
            this.txtWhere.TabIndex = 6;
            this.txtWhere.TextChanged += new System.EventHandler(this.txtWhere_TextChanged);
            // 
            // labWhere
            // 
            this.labWhere.Dock = System.Windows.Forms.DockStyle.Top;
            this.labWhere.Location = new System.Drawing.Point(0, 23);
            this.labWhere.Name = "labWhere";
            this.labWhere.Size = new System.Drawing.Size(477, 22);
            this.labWhere.TabIndex = 5;
            this.labWhere.Text = "WHERE";
            this.labWhere.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbTables
            // 
            this.cmbTables.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbTables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTables.Location = new System.Drawing.Point(0, 0);
            this.cmbTables.Name = "cmbTables";
            this.cmbTables.Size = new System.Drawing.Size(477, 23);
            this.cmbTables.TabIndex = 4;
            this.cmbTables.SelectionChangeCommitted += new System.EventHandler(this.cmbTables_SelectionChangeCommitted);
            // 
            // pnlClassObject
            // 
            this.pnlClassObject.Controls.Add(this.cmbMethods);
            this.pnlClassObject.Controls.Add(this.pnlPopulate);
            this.pnlClassObject.Controls.Add(this.labInfo);
            this.pnlClassObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlClassObject.Location = new System.Drawing.Point(7, 7);
            this.pnlClassObject.Name = "pnlClassObject";
            this.pnlClassObject.Size = new System.Drawing.Size(477, 125);
            this.pnlClassObject.TabIndex = 15;
            this.pnlClassObject.Visible = false;
            // 
            // cmbMethods
            // 
            this.cmbMethods.DisplayMember = "MethodDisplay";
            this.cmbMethods.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethods.Location = new System.Drawing.Point(0, 0);
            this.cmbMethods.Name = "cmbMethods";
            this.cmbMethods.Size = new System.Drawing.Size(477, 23);
            this.cmbMethods.TabIndex = 3;
            this.cmbMethods.ValueMember = "MethodName";
            this.cmbMethods.SelectionChangeCommitted += new System.EventHandler(this.cmbMethods_SelectionChangeCommitted);
            // 
            // pnlPopulate
            // 
            this.pnlPopulate.Controls.Add(this.btnPopulate);
            this.pnlPopulate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlPopulate.Location = new System.Drawing.Point(0, 98);
            this.pnlPopulate.Name = "pnlPopulate";
            this.pnlPopulate.Size = new System.Drawing.Size(477, 27);
            this.pnlPopulate.TabIndex = 5;
            // 
            // btnPopulate
            // 
            this.btnPopulate.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPopulate.Location = new System.Drawing.Point(1, 4);
            this.resourceLookup1.SetLookup(this.btnPopulate, new FWBS.OMS.UI.Windows.ResourceLookupItem("Populate", "<< Populate", ""));
            this.btnPopulate.Name = "btnPopulate";
            this.btnPopulate.Size = new System.Drawing.Size(75, 23);
            this.btnPopulate.TabIndex = 0;
            this.btnPopulate.Text = "<< #Populate";
            this.btnPopulate.Click += new System.EventHandler(this.btnPopulate_Click);
            // 
            // labInfo
            // 
            this.labInfo.BackColor = System.Drawing.SystemColors.Control;
            this.labInfo.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.labInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labInfo.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labInfo.Location = new System.Drawing.Point(0, 0);
            this.labInfo.Multiline = true;
            this.labInfo.Name = "labInfo";
            this.labInfo.ReadOnly = true;
            this.labInfo.Size = new System.Drawing.Size(477, 125);
            this.labInfo.TabIndex = 4;
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 191);
            this.splitter2.MinExtra = 100;
            this.splitter2.MinSize = 100;
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(497, 5);
            this.splitter2.TabIndex = 14;
            this.splitter2.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtSource);
            this.panel6.Controls.Add(this.btnGet);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.btnUnlock);
            this.panel6.Controls.Add(this.cmbSourceType);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.panel6.Size = new System.Drawing.Size(497, 30);
            this.panel6.TabIndex = 12;
            // 
            // txtSource
            // 
            this.txtSource.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSource.Enabled = false;
            this.txtSource.Location = new System.Drawing.Point(251, 4);
            this.txtSource.Name = "txtSource";
            this.txtSource.Size = new System.Drawing.Size(223, 23);
            this.txtSource.TabIndex = 3;
            this.txtSource.MouseLeave += new System.EventHandler(this.txtSource_MouseLeave);
            this.txtSource.MouseHover += new System.EventHandler(this.txtSource_MouseHover);
            this.txtSource.Validated += new System.EventHandler(this.txtSource_Validated);
            // 
            // btnGet
            // 
            this.btnGet.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnGet.Enabled = false;
            this.btnGet.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnGet.Location = new System.Drawing.Point(474, 4);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(23, 22);
            this.btnGet.TabIndex = 5;
            this.btnGet.Text = "...";
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Location = new System.Drawing.Point(196, 4);
            this.resourceLookup1.SetLookup(this.label2, new FWBS.OMS.UI.Windows.ResourceLookupItem("Source", " Source :  ", ""));
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 22);
            this.label2.TabIndex = 2;
            this.label2.Text = " Source :  ";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUnlock
            // 
            this.btnUnlock.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnUnlock.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUnlock.Location = new System.Drawing.Point(173, 4);
            this.resourceLookup1.SetLookup(this.btnUnlock, new FWBS.OMS.UI.Windows.ResourceLookupItem("btnUnlock", "U", "Unlock Source Type"));
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(23, 22);
            this.btnUnlock.TabIndex = 8;
            this.btnUnlock.Text = "U";
            this.btnUnlock.Click += new System.EventHandler(this.bntUnlock_Click);
            // 
            // cmbSourceType
            // 
            this.cmbSourceType.DisplayMember = "Name";
            this.cmbSourceType.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbSourceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceType.Location = new System.Drawing.Point(77, 4);
            this.cmbSourceType.Name = "cmbSourceType";
            this.cmbSourceType.Size = new System.Drawing.Size(96, 23);
            this.cmbSourceType.TabIndex = 4;
            this.cmbSourceType.ValueMember = "Value";
            this.cmbSourceType.SelectionChangeCommitted += new System.EventHandler(this.cmbSourceType_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 4);
            this.resourceLookup1.SetLookup(this.label1, new FWBS.OMS.UI.Windows.ResourceLookupItem("SourceType", "Source Type : ", ""));
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Source Type : ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackgroundColor = System.Drawing.Color.White;
            this.dataGrid1.DataMember = "";
            this.dataGrid1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGrid1.GridLineColor = System.Drawing.Color.Silver;
            this.dataGrid1.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.Location = new System.Drawing.Point(0, 196);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(497, 260);
            this.dataGrid1.ReadOnly = true;
            this.dataGrid1.TabIndex = 10;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(208, 5);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(6, 456);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // pnlHelp
            // 
            this.pnlHelp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlHelp.BackColor = System.Drawing.SystemColors.Info;
            this.pnlHelp.Controls.Add(this.pictureBox1);
            this.pnlHelp.Controls.Add(this.panel7);
            this.pnlHelp.Location = new System.Drawing.Point(440, 30);
            this.pnlHelp.Name = "pnlHelp";
            this.pnlHelp.Padding = new System.Windows.Forms.Padding(34, 5, 5, 5);
            this.pnlHelp.Size = new System.Drawing.Size(271, 80);
            this.pnlHelp.TabIndex = 13;
            this.pnlHelp.Visible = false;
            this.pnlHelp.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlHelp_Paint);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(2, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.txtInfoObject);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(34, 5);
            this.panel7.Name = "panel7";
            this.panel7.Padding = new System.Windows.Forms.Padding(4);
            this.panel7.Size = new System.Drawing.Size(232, 70);
            this.panel7.TabIndex = 1;
            // 
            // txtInfoObject
            // 
            this.txtInfoObject.BackColor = System.Drawing.SystemColors.Info;
            this.txtInfoObject.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInfoObject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtInfoObject.Location = new System.Drawing.Point(4, 4);
            this.txtInfoObject.Multiline = true;
            this.txtInfoObject.Name = "txtInfoObject";
            this.txtInfoObject.ReadOnly = true;
            this.txtInfoObject.Size = new System.Drawing.Size(224, 62);
            this.txtInfoObject.TabIndex = 0;
            this.txtInfoObject.Text = "[Object Name], [Assembly Name]\r\n\r\nexample:\r\n    FWBS.OMS.OmsObject, OMS.Library";
            // 
            // frmDataBuilder
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(800, 466);
            this.Controls.Add(this.pnlHelp);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlTools);
            this.Controls.Add(this.grpParameters);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmDataBuilder";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Data Builder";
            this.Load += new System.EventHandler(this.frmDataBuilder_Load);
            this.pnlTools.ResumeLayout(false);
            this.grpParameters.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.grpSQLCode.ResumeLayout(false);
            this.pnlSQLCode.ResumeLayout(false);
            this.pnlSQL.ResumeLayout(false);
            this.pnlSQL.PerformLayout();
            this.pnlSqlBrowse.ResumeLayout(false);
            this.pnlTables.ResumeLayout(false);
            this.pnlTables.PerformLayout();
            this.pnlClassObject.ResumeLayout(false);
            this.pnlClassObject.PerformLayout();
            this.pnlPopulate.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid1)).EndInit();
            this.pnlHelp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		#region Private Methods
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
			string text;
			if (txtSQL.Visible)
				text = txtSQL.Text;
			else
                text = txtWhere.Text;

            var ps = text.Split(new string[] { "(",",", " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			var pa = new List<string>();
            foreach (string pn in ps)
            {
                if (pn.StartsWith("@"))
                {
                    var param = pn.TrimEnd(ClosingBits);
                    if (!pa.Contains(param))
                        pa.Add(param);
                }
            }

			string sqlparam ="";
			if (Value.ParametersInx.Count < pa.Count)
				sqlparam = Convert.ToString(pa[Value.ParametersInx.Count]);
			else
				sqlparam = "@Parameter1";
			int n=1;
			while (Value.ParametersInx[sqlparam] != null)
			{
				n++;	
				sqlparam = "@Parameter" + n.ToString();
			}
            Parameter p = new Parameter(Value, sqlparam, "NVarChar", "", "", FWBS.OMS.SearchEngine.SearchParameterDateIs.NotApplicable);
			Value.ParametersInx.Add(sqlparam,sqlparam);
			Value.Parameters.Add(p);
			ParameterProperties.SelectedObject = p;
			txtSQL_TextChanged(sender,e);
			RefreshParameters();
		}

		private void lstParameters_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (lstParameters.SelectedItem != null && ParameterProperties.SelectedObject != lstParameters.SelectedItem)
				ParameterProperties.SelectedObject = lstParameters.SelectedItem;
		}

		private void btnRemove_Click(object sender, System.EventArgs e)
		{
			if (lstParameters.SelectedItem != null)
			{
				int n = lstParameters.SelectedIndex;
				Parameter p = (Parameter)lstParameters.SelectedItem;
				lstParameters.SelectedItem = null;
				ParameterProperties.SelectedObject = null;
				Value.ParametersInx.Remove(p.SQLParameter);
				Value.Parameters.Remove(p);
				RefreshParameters();
				if (n >= Value.Parameters.Count)
					lstParameters.SelectedIndex = Value.Parameters.Count-1;
				else
					lstParameters.SelectedIndex = n;

				txtSQL_TextChanged(sender,e);
			}
		}

		private void RefreshParameters()
		{
			if (Value.Parameters.Count != Value.ParametersInx.Count)
				Value.BuildIndex();
			try
			{
				lstParameters.BeginUpdate();
				lstParameters.DataSource = null;
				lstParameters.DataSource = Value.Parameters;
				lstParameters.DisplayMember = "SQLParameter";
				lstParameters.ValueMember = "SQLParameter";
			}
			catch
			{
				lstParameters.DataSource = null;
			}
			finally
			{
				lstParameters.EndUpdate();
			}
			if (cmbTables.Visible) cmbTables.Text = Value.TableName;
		}

		private void txtSQL_TextChanged(object sender, System.EventArgs e)
		{
			string _call = "";
			if (pnlSQL.Visible || sender == pnlSQL)
				_call = txtSQL.Text;
			SetSource((SourceType)FWBS.Common.ConvertDef.ToEnum(cmbSourceType.Text,SourceType.OMS),_call,txtSource.Text);
		}

		private void btnOK_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
		}

		private void btnCancel_Click(object sender, System.EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}

		private void frmDataBuilder_Load(object sender, System.EventArgs e)
		{
            try
            {
                SetForm();
                Text = "Data Builder [" + Session.CurrentSession.Resources.GetResource("TESTEDOK", "Tested Ok", "").Text + "]";
                btnOK.Enabled = true;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

		private void SetForm()
		{
			if (_updating) return;
			try
			{
                grpSQLCode.Text = "SQL Code";
                _updating = true;
                pnlClassObject.Visible = false;
                pnlSQL.Visible = false;
                pnlTables.Visible = false;
                pnlSqlBrowse.Visible = false;

                switch (Value.SourceType)
                {
                    case SourceType.Instance:
                        pnlSQL.Visible = true;
					    txtSource.Text = "";
					    txtSource.Enabled=false;
					    btnGet.Enabled = false;
                        grpSQLCode.Text = "Instance Method";
                    break;
                    case SourceType.Class:
                    case SourceType.Object:
					    txtSource.Enabled=true;
					    pnlClassObject.Visible=true;
					    btnGet.Enabled = true;
                        break;
                    case SourceType.OMS:
                        txtSource.Enabled=false;
					    pnlSqlBrowse.Visible = true;
                        if (_mode == DataBuilderMode.EnquiryMode)
                        {
                            pnlTables.Visible = true;
                            cmbTables.DataSource = EnquiryEngine.Enquiry.GetTableNames();
                            cmbTables.DisplayMember = "tblname";
                            cmbTables.ValueMember = "tblname";
                        }
                        else
                            pnlSQL.Visible = true;
                        break;
                    case SourceType.Linked:
                    case SourceType.Dynamic:
					    pnlSqlBrowse.Visible = true;
                        if (Value.SourceType == SourceType.Dynamic) 
					    {
						    btnGet.Enabled = true;
						    Value.ReBind();
					    }
					    txtSource.Enabled = true;
					    if (_mode == DataBuilderMode.EnquiryMode)
					    {
						    pnlSQL.Visible=false;
						    pnlTables.Visible=true;
                            try
                            {
                                cmbTables.ValueMember = "TABLE_NAME";
                                cmbTables.DisplayMember = "TABLE_NAME";
                                cmbTables.DataSource = Value.GetTables();
                                cmbTables.Text = Value.TableName;
                                cmbTables.DropDownStyle = ComboBoxStyle.DropDownList;
                            }
                            catch
                            {
                                cmbTables.DataSource = null;
                                cmbTables.DropDownStyle = ComboBoxStyle.DropDown;
                                cmbTables.Text = Value.TableName;
                            }
					    }
					    else
					    {
						    pnlSQL.Visible=true;
						    pnlTables.Visible=false;
					    }
                    break;
                    default:
                        break;
                }  

				txtSource.Text = Value.Source;

				if (Value.SourceType == SourceType.Instance && Value.Parent != null) 
				{
					try
					{
						cmbMethods.BeginUpdate();
						cmbMethods.DataSource = Enquiry.GetObjectMethods(Value.Parent.GetType());
						if (cmbMethods.Items.Count > 0)
						{
							cmbMethods.DisplayMember = "MethodDisplay";
							cmbMethods.ValueMember = "MethodName";
						}
						cmbMethods.EndUpdate();
					}
					catch (Exception ex)
					{
						ErrorBox.Show(this,ex);
					}
				}
				else if (Value.SourceType == SourceType.Class && Value.Source != "") 
				{
					try
					{
						cmbMethods.BeginUpdate();
						cmbMethods.DataSource = Enquiry.GetObjectStaticMethods(Value.Source);
						if (cmbMethods.Items.Count > 0)
						{
							cmbMethods.DisplayMember = "MethodDisplay";
							cmbMethods.ValueMember = "MethodName";
						}
						cmbMethods.EndUpdate();
					}
					catch (Exception ex)
					{
						ErrorBox.Show(this,ex);
					}
				}
				else if (Value.SourceType == SourceType.Object && Value.Source != "")
				{
					try
					{
						cmbMethods.BeginUpdate();
						cmbMethods.DataSource = Enquiry.GetObjectConstructors(Value.Source);
						if (cmbMethods.Items.Count > 0)
						{
							cmbMethods.DisplayMember = "MethodDisplay";
							cmbMethods.ValueMember = "MethodName";
						}
						cmbMethods.EndUpdate();
					}
					catch (Exception ex)
					{
						ErrorBox.Show(this,ex);
					}
				}
				else
				{
					cmbMethods.DisplayMember = "";
					cmbMethods.ValueMember = "";
					cmbMethods.DataSource = null;
				}

				if (_mode == DataBuilderMode.EnquiryMode)
				{
					cmbTables.SelectedValue = Value.Call;
					if (txtWhere.Text.ToUpper().StartsWith("WHERE")) txtWhere.Text = txtWhere.Text.Substring(5).Trim();
					txtWhere.Text = Value.Where;
				}
				else
				{
					cmbMethods.SelectedValue = Value.Call;
					txtSQL.Text = Value.Call;
				}

				cmbSourceType.SelectedValue = (System.Int64)Value.SourceType;
			
				RefreshParameters();

				Text = "Data Builder [" + Session.CurrentSession.Resources.GetResource("TESTENDOK","Not Tested","").Text + "]";
				btnOK.Enabled=false;
				grpSQLCode.Text = Value.SourceType.ToString() + " Code";
			}
			finally
			{
				_updating = false;
			}
		}


		private void SetSource(SourceType SourceType ,string Call, string Source)
		{
			if (_updating) return;
			Value.Call = Call;
			Value.SourceType = SourceType;
			Value.Source = Source;

			// Source Type Settings
			SetForm();
		}

		private void cmbSourceType_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (_mode == DataBuilderMode.EnquiryMode && (SourceType)(System.Int64)cmbSourceType.SelectedValue == SourceType.Class)
			{
				ErrorBox.Show(this, new SourceException(HelpIndexes.SourceTypeInvalid,"Class","Enquiry Form"));
				cmbSourceType.SelectedValue = (System.Int64)Value.SourceType;
				return;
			}
			SetSource((SourceType)FWBS.Common.ConvertDef.ToEnum(cmbSourceType.Text,SourceType.OMS),"","");
		}

		private bool ValidateParameters()
		{
			System.Text.StringBuilder boundvalues = new System.Text.StringBuilder();
			foreach(Parameter p in Value.Parameters)
			{
                if (p.FieldType == "")
				{
					MessageBox.Show(Session.CurrentSession.Resources.GetResource("VALIDFLDTYPE", "You must enter a Field Type from the List", "").Text, FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK,MessageBoxIcon.Stop);
					lstParameters.SelectedItem = p;					
					return false;
				}
				if (p.SQLParameter == "")
				{
					MessageBox.Show(Session.CurrentSession.Resources.GetResource("VALIDFLDNAME", "You must enter a SQL Field Name e.g SEARCH1", "").Text, FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK,MessageBoxIcon.Stop);
					lstParameters.SelectedItem = p;					
					return false;
				}
				if (p.BoundValue == "")
				{
					MessageBox.Show(Session.CurrentSession.Resources.GetResource("VALIDBOUND", "You must enter a Internal Value Name To replace e.g %SEARCH1%", "").Text, FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK,MessageBoxIcon.Stop);
					lstParameters.SelectedItem = p;					
					return false;
				}
				if (p.TestValue == "")
				{
					MessageBox.Show(Session.CurrentSession.Resources.GetResource("VALIDTEST", "You must enter a Literal Test Value to Replace the Internal Value with e.g TEST", "").Text, FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.OK,MessageBoxIcon.Stop);
					lstParameters.SelectedItem = p;					
					return false;
				}
				if (boundvalues.ToString().IndexOf(p.BoundValue) > -1)
				{
					if (_yestowarnings == false && MessageBox.Show(Session.CurrentSession.Resources.GetResource("USEDBOUND", "The Bound Value of ''%1%'' has already been used in another Parameter. Are you sure this is correct?", "", p.BoundValue).Text, FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.YesNo,MessageBoxIcon.Warning) == DialogResult.No) 
					{
						lstParameters.SelectedItem = p;					
						return false;
					}
					else
						_yestowarnings = true;
				}
				else
					boundvalues.Append(p.BoundValue + ";");

			}
			return true;
		}

        private void btnTest_Click(object sender, System.EventArgs e)
        {
            if (ValidateParameters())
            {
                dataGrid1.DataSource = null;
                Value.Parameters = Value.Parameters;
                var sourcetype = (SourceType)FWBS.Common.ConvertDef.ToEnum(cmbSourceType.Text, SourceType.OMS);
                switch (sourcetype)
                {
                    case SourceType.Class:
                    case SourceType.Object:
                        SetSource(sourcetype, Convert.ToString(cmbMethods.SelectedValue), txtSource.Text);
                        break;
                    case SourceType.Dynamic:
                    case SourceType.Linked:
                    case SourceType.OMS:
                        {
                            switch (_mode)
                            {
                                case DataBuilderMode.EnquiryMode:
                                    SetSource(sourcetype, cmbTables.Text, txtSource.Text);
                                    break;
                                default:
                                    SetSource(sourcetype, txtSQL.Text, txtSource.Text);
                                    break;
                            }
                        }
                        break;
                    case SourceType.Instance:
                        btnOK.Enabled = true;
                        Text = "Data Builder [" + Session.CurrentSession.Resources.GetResource("TESTEDOK", "Tested Ok", "").Text + "]";
                        return;
                    default:
                        break;
                }

                try
                {
                    Cursor = Cursors.WaitCursor;
                    string _lastcall = Value.Call;
                    if (_mode == DataBuilderMode.EnquiryMode)
                    {
                        if (Value.TableName != "")
                        {
                            Value.Call = "SELECT TOP 50 * FROM " + Value.TableName;
                            if (Value.Where != "")
                                if (Value.Where.ToUpper().StartsWith("WHERE") == false)
                                    Value.Call = Value.Call + " WHERE " + Value.Where;
                                else
                                    Value.Call = Value.Call + " " + Value.Where;
                        }
                    }
                    Value.ResetFields();
                    Value.ReBind();
                    object dso = Value.Run(false, true);
                    if (dso is DataSet)
                    {
                        DataSet ds = dso as DataSet;
                        if (ds.Tables.Count > 1)
                        {
                            for (int i = 0; i < ds.Tables.Count; i++)
                                ds.Tables[i].TableName = "TABLE_" + (i + 1).ToString();

                            foreach (DataColumn dc in ds.Tables[0].Columns)
                                Value.Fields.Add(new ReturnFields(null, dc.ColumnName, null));

                            dataGrid1.DataSource = ds;
                        }
                        else
                            dataGrid1.DataSource = ds.Tables[0];
                    }
                    else if (dso is DataTable)
                    {
                        DataTable dt = dso as DataTable;
                        foreach (DataColumn dc in dt.Columns)
                            Value.Fields.Add(new ReturnFields(null, dc.ColumnName, null));
                        dataGrid1.DataSource = dt;
                    }
                    else if (dso is FWBS.OMS.Interfaces.IExtraInfo)
                    {
                        FWBS.OMS.Interfaces.IExtraInfo iei = dso as FWBS.OMS.Interfaces.IExtraInfo;
                        DataTable dt = iei.GetDataTable();
                        foreach (DataColumn dc in dt.Columns)
                            Value.Fields.Add(new ReturnFields(null, dc.ColumnName, null));
                        dataGrid1.DataSource = dt;

                    }
                    SetForm();
                    Value.Call = _lastcall;
                    btnOK.Enabled = true;
                    Text = "Data Builder [" + Session.CurrentSession.Resources.GetResource("TESTEDOK", "Tested Ok", "").Text + "]";
                }
                catch (Exception ex)
                {
                    if (Value.SourceType == SourceType.Object)
                    {
                        btnOK.Enabled = true;
                        Text = "Data Builder [" + Session.CurrentSession.Resources.GetResource("TESTEDOK", "Tested Ok", "").Text + "]";
                    }
                    FWBS.OMS.UI.Windows.ErrorBox.Show(this, ex);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

		private void btnGet_Click(object sender, System.EventArgs e)
		{
			if (Value.SourceType == SourceType.Class || Value.SourceType == SourceType.Object)
			{
				frmListSelector frmListSelector1 = new frmListSelector();
				frmListSelector1.List.Sorted=true;
				Type[] rtypes;
				if (Value.SourceType == SourceType.Class)
					rtypes = FWBS.OMS.EnquiryEngine.Enquiry.GetObjects(true);
				else
					rtypes = FWBS.OMS.EnquiryEngine.Enquiry.GetObjects();
				string[] types = new string[rtypes.Length];
				for(int n = 0; n<rtypes.Length; n++)
					types[n] = rtypes[n].FullName;
				frmListSelector1.List.Items.AddRange(types);
				if (Convert.ToString(txtSource.Text) != "")
					frmListSelector1.List.SelectedValue= txtSource.Text;
				frmListSelector1.ShowHelp = false;
				frmListSelector1.ShowDialog(this);
				if (frmListSelector1.DialogResult == DialogResult.OK)
				{
					txtSource.Text = frmListSelector1.List.Text;
					txtSource_Validated(sender,e);
					Value.ReBind();
				}
                frmListSelector1.Dispose();
            }
			else
			{
				if (txtSource.Text == "")
					txtSource.Text = ConnectionStringBuilder.Show(this);
				else
					txtSource.Text = ConnectionStringBuilder.Show(txtSource.Text,this);
				txtSource_Validated(sender,e);
				Value.ReBind();
                try
                {
                    cmbTables.DataSource = Value.GetTables();
                    cmbTables.ValueMember = "TABLE_NAME";
                    cmbTables.DisplayMember = "TABLE_NAME";
                    cmbTables_SelectionChangeCommitted(sender, e);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(this, ex);
                }
			}
		}

		private void ParameterProperties_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			RefreshParameters();
			_yestowarnings = false;
		}

		private void btnChangeData_Click(object sender, System.EventArgs e)
		{
			try
			{
				DataSet ds = new DataSet();
				ds.Tables.Add(Value.GetTables());
				ds.Tables.Add(Value.GetViews());
				ds.Tables.Add(Value.GetProcedures());
				frmDataDirectory dd = new frmDataDirectory(ds);
				dd.ShowDialog(this);
				if (dd.DialogResult == DialogResult.OK)
				{
					if (dd.tbcDataType.SelectedTab == dd.tpTables)
					{
						txtSQL.Text = "select * from " + dd.lstTables.Text;
						Value.TableName = "";
					}
					if (dd.tbcDataType.SelectedTab == dd.tpQueries)
					{
						txtSQL.Text = "select * from " + dd.lstQueries.Text;
						Value.TableName = "";
					}
					if (dd.tbcDataType.SelectedTab == dd.tpStoredProcs)
					{
						txtSQL.Text = dd.lstStoredProcs.Text;
						DataTable dtp = Value.GetParameters(dd.lstStoredProcs.Text);
						Value.ParametersInx.Clear();
						Value.Parameters.Clear();
						int t = 1;
						foreach (DataRow rw in dtp.Rows)
						{
							string __value ="";
							string __test ="";
							string __name = Convert.ToString(rw["PARAMETER_NAME"]);
							string __type = Convert.ToString(rw["DATA_TYPE"]);
							if (__name.ToUpper() == "@UI")
							{
								__value = "%#UI%";
								__test = "en-gb";
							}
							else
							{
								__value = "%" + t.ToString() + "%";
								if (__type == "SqlDbType.BigInt" || __type == "SqlDbType.Float" || __type == "SqlDbType.Decimal"  || __type == "SqlDbType.Float" || __type == "SqlDbType.Int" || __type == "SqlDbType.Money" || __type == "SqlDbType.Real" || __type == "SqlDbType.SmallInt" || __type == "SqlDbType.SmallMoney" || __type == "SqlDbType.TinyInt")
									__test = "0";
								else if(__type == "SqlDbType.Bit")
									__test = "False";
								else
									__test = "";
							}
                            Value.Parameters.Add(new Parameter(Value, __name, __type, __value, __test, FWBS.OMS.SearchEngine.SearchParameterDateIs.NotApplicable));
							t++;
						}
						Value.BuildIndex();
						RefreshParameters();
					}	
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this,ex);
			}
		}

		private void ParameterProperties_SelectedObjectsChanged(object sender, System.EventArgs e)
		{
			btnRemove.Enabled=true;

		}

		private void txtSource_MouseHover(object sender, System.EventArgs e)
		{
			if (Value.SourceType == SourceType.Object) pnlHelp.Visible=true;
		}

		private void txtSource_MouseLeave(object sender, System.EventArgs e)
		{
			pnlHelp.Visible=false;
		}

		private void pnlHelp_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
            using (Pen p1 = new Pen(System.Drawing.Color.FromArgb(192, 192, 0), 1))
            {
                e.Graphics.DrawRectangle(p1, 0, 0, pnlHelp.Width - 1, this.pnlHelp.Height - 1);
            }
		}

		private void cmbMethods_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			if (Value.SourceType == SourceType.Class)
			{
				ReflectionMethods n = (ReflectionMethods)cmbMethods.SelectedItem;
				Value.Call = n.MethodName;
				ParameterInfo[] parameter = n.Method.GetParameters();
				string _display = n.Method.Name + "(";
				foreach(ParameterInfo pm in parameter)
					_display = _display + pm.ParameterType + " " + pm.Name + ", ";
				if (_display.EndsWith(", ")) _display = _display.Substring(0,_display.Length-2);
				_display = _display + ")";
				labInfo.Text = _display;
			}
			else if (Value.SourceType == SourceType.Object)
			{
				labInfo.Text = cmbMethods.Text;
			}
			Value.Call =  Convert.ToString(cmbMethods.SelectedValue);
			Value.BuildIndex();
			RefreshParameters();
		}

		private void btnPopulate_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (Value.SourceType == SourceType.Class || Value.SourceType == SourceType.Instance)
				{
					ReflectionMethods n = (ReflectionMethods)cmbMethods.SelectedItem;
					Value.Parameters.Clear();
					Value.ParametersInx.Clear();
					ParameterInfo[] parameter = n.Method.GetParameters();
					foreach(ParameterInfo pm in parameter)
					{
                        Value.Parameters.Add(new Parameter(Value, pm.Name, pm.ParameterType.ToString(), "%" + pm.Name + "%", "", FWBS.OMS.SearchEngine.SearchParameterDateIs.NotApplicable));
					}
					Value.BuildIndex();
					RefreshParameters();
				}
				else if (Value.SourceType == SourceType.Object)
				{
					ReflectionMethods n = (ReflectionMethods)cmbMethods.SelectedItem;
					Value.Parameters.Clear();
					Value.ParametersInx.Clear();
					ParameterInfo[] parameter = n.Constuctor.GetParameters();
					foreach(ParameterInfo pm in parameter)
					{
                        Value.Parameters.Add(new Parameter(Value, pm.Name, pm.ParameterType.ToString(), "%" + pm.Name + "%", "", FWBS.OMS.SearchEngine.SearchParameterDateIs.NotApplicable));
					}
					Value.BuildIndex();
					RefreshParameters();
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
		}

		private void mnuRemoveAll_Click(object sender, System.EventArgs e)
		{
			Value.Parameters.Clear();
			Value.ParametersInx.Clear();
			RefreshParameters();
		}

		private void cmbTables_SelectionChangeCommitted(object sender, System.EventArgs e)
		{
			Value.Call = cmbTables.Text;
			Value.TableName = cmbTables.Text;
			if (txtWhere.Text.ToUpper().StartsWith("WHERE"))
				txtWhere.Text = txtWhere.Text.Substring(5).Trim();
			Value.Where = txtWhere.Text;
			btnOK.Enabled=false;
		}

		private void txtWhere_TextChanged(object sender, System.EventArgs e)
		{
			Value.Call = cmbTables.Text;
			Value.TableName = cmbTables.Text;
			if (txtWhere.Text.ToUpper().StartsWith("WHERE"))
				txtWhere.Text = txtWhere.Text.Substring(5).Trim();
			Value.Where = txtWhere.Text;
			btnOK.Enabled=false;
		}

		#endregion


		private void txtSource_Validated(object sender, System.EventArgs e)
		{
			try
			{
				SetSource((SourceType)FWBS.Common.ConvertDef.ToEnum(cmbSourceType.Text,SourceType.OMS),txtSQL.Text,txtSource.Text);
			}
			catch
			{}
		}

		private void bntUnlock_Click(object sender, System.EventArgs e)
		{
			cmbSourceType.Enabled = true;
		}


		#region Properties
		public bool ParametersPanel
		{
			get
			{
				return _grpparameters;
			}
			set
			{
				if (value != _grpparameters)
				{
					grpParameters.Visible=value;
					_grpparameters = value;
					if (value)
					{
						this.Width += grpParameters.Width;
						this.Left -= grpParameters.Width / 2;
					}
					else
					{
						this.Width -= grpParameters.Width;
						this.Left += grpParameters.Width / 2;
					}
				}
			}
		}

		public DataBuilderMode Mode
		{
			get
			{
				return _mode;
			}
			set
			{
				_mode = value;
			}
		}

		public SourceType ExcludeSourceType
		{
			get
			{
				return _exclude;
			}
			set
			{
				_exclude = value;
				_items.Clear();
				// Display the set of legal enum values
				SourceType[] o = (SourceType[]) Enum.GetValues(typeof(SourceType));
				for (int x = o.Length-1; x > -1; x--) 
				{
					SourceType aax = ((SourceType)(o[x]));
					if ((_exclude | aax) != _exclude) 
					{
						_items.Add(new EnumListItem((Int64)aax,aax.ToString()));
					}
                }
                cmbSourceType.DataSource = null;
                cmbSourceType.DataSource = _items;
                cmbSourceType.DisplayMember = "Name";
                cmbSourceType.ValueMember = "Value";
			}
		}
		#endregion

	}
}
