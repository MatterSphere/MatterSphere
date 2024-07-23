using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FWBS.OMS.UI.DocumentManagement.Addins;

namespace FWBS.OMS.UI.Windows.Admin
{
	/// <summary>
	/// AnOMS type editing admin kit control.
	/// </summary>
	public class ucEntitiesV2 : FWBS.OMS.UI.Windows.Admin.ucEditBase2
	{
		#region Fields

		private DataTable dtTabCaptions = CodeLookup.GetLookups("DLGTABCAPTION");
		private DataTable dtPnlCaptions = CodeLookup.GetLookups("DLGPNLCAPTION");
		private FWBS.OMS.OMSType _current = null;
		private Type _omstype = typeof(FWBS.OMS.ClientType);

		#endregion

		#region Controls

		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel pnlProperties;
		private FWBS.Common.UI.Windows.eXPPanel pnlActions;
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.ToolBarButton tbNewTab;
		private System.Windows.Forms.ToolBarButton tbRemoveTab;
		private FWBS.OMS.UI.TabControl tbTabEditor;
		private System.Windows.Forms.Panel pnlInfoBack;
        private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.DataGridTableStyle lstEntities;
		private System.Windows.Forms.DataGridTextBoxColumn ecNone;
		private FWBS.OMS.UI.Windows.DataGridImageColumn ecICode;
		private System.Windows.Forms.Timer timRefresh;
        private System.Windows.Forms.LinkLabel lnkAddExtendedData;
        private Panel pnlTabContainer;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme1;
        private Panel pnlTreeView;
        private Telerik.WinControls.UI.RadTreeView radTreeView1;
		private System.Windows.Forms.DataGridTextBoxColumn ecDesc;
		
		#endregion

		#region Contructors

		public ucEntitiesV2()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
            SetLookups();
            SetupEvents();
		}


		public ucEntitiesV2(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : base(mainparent,editparent, Params)
		{
            _omstype = Session.CurrentSession.TypeManager.Load(Convert.ToString(Params["Type"].Value));
			
			InitializeComponent();
            SetLookups();
            SetupEvents();
			
			propertyGrid1.HelpVisible=true;
			this.tbReturn.Text = "Return";

			ecICode.ImageList = FWBS.OMS.UI.Windows.Images.Entities();
			lstList.Text = "Refresh";
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

        private void SetLookups()
        {
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            this.BresourceLookup1.SetLookup(this.lnkAddExtendedData, new FWBS.OMS.UI.Windows.ResourceLookupItem("AddExtData", "Add Extended Data from this Type to Selected Types", ""));
            this.BresourceLookup1.SetLookup(this.tbNewTab, new FWBS.OMS.UI.Windows.ResourceLookupItem("New", "New", ""));
            this.BresourceLookup1.SetLookup(this.tbRemoveTab, new FWBS.OMS.UI.Windows.ResourceLookupItem("Remove", "Remove", ""));
            this.BresourceLookup1.SetLookup(this.ecICode, new FWBS.OMS.UI.Windows.ResourceLookupItem("Code", "Code", ""));
            this.BresourceLookup1.SetLookup(this.ecDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("Description", "Description", ""));
        }

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlProperties = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlActions = new FWBS.Common.UI.Windows.eXPPanel();
            this.lnkAddExtendedData = new System.Windows.Forms.LinkLabel();
            this.tbNewTab = new System.Windows.Forms.ToolBarButton();
            this.tbRemoveTab = new System.Windows.Forms.ToolBarButton();
            this.tbTabEditor = new FWBS.OMS.UI.TabControl();
            this.pnlInfoBack = new System.Windows.Forms.Panel();
            this.lstEntities = new System.Windows.Forms.DataGridTableStyle();
            this.ecNone = new System.Windows.Forms.DataGridTextBoxColumn();
            this.ecICode = new FWBS.OMS.UI.Windows.DataGridImageColumn();
            this.ecDesc = new System.Windows.Forms.DataGridTextBoxColumn();
            this.timRefresh = new System.Windows.Forms.Timer(this.components);
            this.pnlTabContainer = new System.Windows.Forms.Panel();
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme();
            this.pnlTreeView = new System.Windows.Forms.Panel();
            this.radTreeView1 = new Telerik.WinControls.UI.RadTreeView();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.pnlProperties.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.pnlTabContainer.SuspendLayout();
            this.pnlTreeView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView1)).BeginInit();
            this.SuspendLayout();
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.pnlTabContainer);
            this.tpEdit.Controls.Add(this.pnlTreeView);
            this.tpEdit.Controls.Add(this.pnlInfoBack);
            this.tpEdit.Controls.Add(this.splitter1);
            this.tpEdit.Controls.Add(this.pnlProperties);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Size = new System.Drawing.Size(807, 383);
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlProperties, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlInfoBack, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlTreeView, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlTabContainer, 0);
            // 
            // pnlEdit
            // 
            this.pnlEdit.BackColor = System.Drawing.Color.White;
            this.pnlEdit.Size = new System.Drawing.Size(807, 50);
            // 
            // labSelectedObject
            // 
            this.labSelectedObject.Size = new System.Drawing.Size(807, 22);
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            // 
            // tbClose
            // 
            this.BresourceLookup1.SetLookup(this.tbClose, new FWBS.OMS.UI.Windows.ResourceLookupItem("Close", "Close", ""));
            // 
            // tbReturn
            // 
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            // 
            // pnlToolbarContainer
            // 
            this.pnlToolbarContainer.Size = new System.Drawing.Size(807, 26);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(573, 50);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(4, 333);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // pnlProperties
            // 
            this.pnlProperties.Controls.Add(this.propertyGrid1);
            this.pnlProperties.Controls.Add(this.splitter2);
            this.pnlProperties.Controls.Add(this.pnlActions);
            this.pnlProperties.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlProperties.Location = new System.Drawing.Point(577, 50);
            this.pnlProperties.Name = "pnlProperties";
            this.pnlProperties.Size = new System.Drawing.Size(230, 333);
            this.pnlProperties.TabIndex = 3;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(230, 295);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 295);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(230, 3);
            this.splitter2.TabIndex = 205;
            this.splitter2.TabStop = false;
            // 
            // pnlActions
            // 
            this.pnlActions.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.Color.White);
            this.pnlActions.BorderLine = true;
            this.pnlActions.Controls.Add(this.lnkAddExtendedData);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlActions.Location = new System.Drawing.Point(0, 298);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(3);
            this.pnlActions.Size = new System.Drawing.Size(230, 35);
            this.pnlActions.TabIndex = 204;
            // 
            // lnkAddExtendedData
            // 
            this.lnkAddExtendedData.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkAddExtendedData.Location = new System.Drawing.Point(3, 3);
            this.lnkAddExtendedData.Name = "lnkAddExtendedData";
            this.lnkAddExtendedData.Size = new System.Drawing.Size(224, 27);
            this.lnkAddExtendedData.TabIndex = 0;
            this.lnkAddExtendedData.TabStop = true;
            this.lnkAddExtendedData.Text = "Add Extended Data from this Type to Selected Types";
            this.lnkAddExtendedData.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkAddExtendedData_LinkClicked);
            // 
            // tbNewTab
            // 
            this.tbNewTab.ImageIndex = 2;
            this.tbNewTab.Name = "tbNewTab";
            this.tbNewTab.Text = "New";
            // 
            // tbRemoveTab
            // 
            this.tbRemoveTab.ImageIndex = 4;
            this.tbRemoveTab.Name = "tbRemoveTab";
            this.tbRemoveTab.Text = "Remove";
            // 
            // tbTabEditor
            // 
            this.tbTabEditor.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tbTabEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTabEditor.ItemSize = new System.Drawing.Size(0, 1);
            this.tbTabEditor.Location = new System.Drawing.Point(1, 1);
            this.tbTabEditor.Margin = new System.Windows.Forms.Padding(0);
            this.tbTabEditor.Name = "tbTabEditor";
            this.tbTabEditor.Padding = new System.Drawing.Point(0, 0);
            this.tbTabEditor.SelectedIndex = 0;
            this.tbTabEditor.Size = new System.Drawing.Size(182, 331);
            this.tbTabEditor.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tbTabEditor.TabIndex = 201;
            // 
            // pnlInfoBack
            // 
            this.pnlInfoBack.AutoScroll = true;
            this.pnlInfoBack.BackColor = System.Drawing.Color.White;
            this.pnlInfoBack.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlInfoBack.Location = new System.Drawing.Point(415, 50);
            this.pnlInfoBack.Name = "pnlInfoBack";
            this.pnlInfoBack.Padding = new System.Windows.Forms.Padding(10);
            this.pnlInfoBack.Size = new System.Drawing.Size(158, 333);
            this.pnlInfoBack.TabIndex = 202;
            // 
            // lstEntities
            // 
            this.lstEntities.DataGrid = null;
            this.lstEntities.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            // 
            // ecNone
            // 
            this.ecNone.Format = "";
            this.ecNone.FormatInfo = null;
            this.ecNone.ReadOnly = true;
            this.ecNone.Width = 0;
            // 
            // ecICode
            // 
            this.ecICode.Format = "";
            this.ecICode.FormatInfo = null;
            this.ecICode.ImageColumn = "typeGlyph";
            this.ecICode.ImageIndex = -1;
            this.ecICode.ImageList = null;
            this.ecICode.MappingName = "typeCode";
            this.ecICode.ReadOnly = true;
            this.ecICode.Width = 125;
            // 
            // ecDesc
            // 
            this.ecDesc.Format = "";
            this.ecDesc.FormatInfo = null;
            this.ecDesc.MappingName = "typeDesc";
            this.ecDesc.ReadOnly = true;
            this.ecDesc.Width = 400;
            // 
            // timRefresh
            // 
            this.timRefresh.Interval = 500;
            this.timRefresh.Tick += new System.EventHandler(this.timRefresh_Tick);
            // 
            // pnlTabContainer
            // 
            this.pnlTabContainer.AutoScroll = true;
            this.pnlTabContainer.BackColor = System.Drawing.Color.White;
            this.pnlTabContainer.Controls.Add(this.tbTabEditor);
            this.pnlTabContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTabContainer.Location = new System.Drawing.Point(231, 50);
            this.pnlTabContainer.Name = "pnlTabContainer";
            this.pnlTabContainer.Padding = new System.Windows.Forms.Padding(1);
            this.pnlTabContainer.Size = new System.Drawing.Size(184, 333);
            this.pnlTabContainer.TabIndex = 205;
            // 
            // pnlTreeView
            // 
            this.pnlTreeView.AutoScroll = true;
            this.pnlTreeView.BackColor = System.Drawing.Color.White;
            this.pnlTreeView.Controls.Add(this.radTreeView1);
            this.pnlTreeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTreeView.Location = new System.Drawing.Point(0, 50);
            this.pnlTreeView.Name = "pnlTreeView";
            this.pnlTreeView.Size = new System.Drawing.Size(231, 333);
            this.pnlTreeView.TabIndex = 205;
            // 
            // radTreeView1
            // 
            this.radTreeView1.BackColor = System.Drawing.Color.White;
            this.radTreeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radTreeView1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.radTreeView1.Location = new System.Drawing.Point(0, 0);
            this.radTreeView1.Margin = new System.Windows.Forms.Padding(0);
            this.radTreeView1.Name = "radTreeView1";
            this.radTreeView1.Size = new System.Drawing.Size(231, 333);
            this.radTreeView1.TabIndex = 0;
            this.radTreeView1.ThemeName = "Windows8";
            // 
            // ucEntitiesV2
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Name = "ucEntitiesV2";
            this.Size = new System.Drawing.Size(1106, 425);
            this.ParentChanged += new System.EventHandler(this.ucOMSType_ParentChanged);
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.pnlProperties.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlTabContainer.ResumeLayout(false);
            this.pnlTreeView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radTreeView1)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#endregion

		#region Destructors

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

		#endregion

        private void Unbind()
        {
            if (_current != null)
            {
                _current.Dirty -= new EventHandler(CallDirty);
                _current.Tabs.Cleared -= new Crownwood.Magic.Collections.CollectionClear(ClearTab);
                _current.Panels.Cleared -= new Crownwood.Magic.Collections.CollectionClear(ClearPanel);
                _current.Tabs.Inserted -= new Crownwood.Magic.Collections.CollectionChange(AddTab);
                _current.Panels.Inserted -= new Crownwood.Magic.Collections.CollectionChange(AddPanel);
            }
        }

        private void Bind(OMSType type)
        {
            if (type == _current)
                return;

            _current = type;

            _current.Tabs.Cleared += new Crownwood.Magic.Collections.CollectionClear(ClearTab);
            _current.Panels.Cleared += new Crownwood.Magic.Collections.CollectionClear(ClearPanel);
            _current.Tabs.Inserted += new Crownwood.Magic.Collections.CollectionChange(AddTab);
            _current.Panels.Inserted += new Crownwood.Magic.Collections.CollectionChange(AddPanel);

        }

		protected override void LoadSingleItem(string type)
		{
            Session.CurrentSession.ClearCache();
			OMSType newitem = OMSType.GetOMSType(_omstype, type);
            LoadSingleItem(newitem);
            BuildTreeView();
		}

		protected void LoadSingleItem(OMSType type)
		{
            if (type == _current)
                return;

            ShowEditor(false);
			this.EditParent.SuspendLayout();
			labSelectedObject.Text = type.Code + " - " + _omstype.Name;

			try
			{
				dtTabCaptions = CodeLookup.GetLookups("DLGTABCAPTION");
				dtPnlCaptions = CodeLookup.GetLookups("DLGPNLCAPTION");

                Unbind();

                ClearTab();
				ClearPanel();

                Bind(type);

				propertyGrid1.SelectedObject = _current;
				_current.Dirty += new EventHandler(CallDirty);

				int ctr = 0;
				foreach (FWBS.OMS.OMSType.Panel pnl in _current.Panels)
				{
					AddPanel(ctr, pnl);
					ctr++;
				}
				ctr = 0;
				foreach (FWBS.OMS.OMSType.Tab tab in _current.Tabs)
				{
					AddTab(ctr, tab);
					ctr++;
				}
                BuildTreeView();
				Refresh();

			}
			finally
			{
				this.EditParent.ResumeLayout();
			}		
		}


		private void CallDirty(object sender,EventArgs e)
		{
			base.IsDirty = true;
			timRefresh.Enabled = false;
			timRefresh.Enabled = true;
		}

		new private void Refresh()
		{
            if (_current != null)
            {
                pnlInfoBack.Width = LogicalToDeviceUnits(_current.PanelWidth);
                pnlInfoBack.BackColor = _current.PanelBackColour;
                foreach (Control ctrl in pnlInfoBack.Controls)
                {
                    if (ctrl is ucPanelNav)
                    {
                        ucPanelNav pnl = (ucPanelNav)ctrl;
                        pnl.Brightness = _current.PanelBrightness;
                    }
                }
            }
		}

		protected override bool UpdateData()
		{
			if (_current.IsDirty)
			{
			    List<string> errorMessages;
                if (EntityValidation(out errorMessages))
			    {
                    _current.Update();
                    if (_current is FileType)
                    {
                        DMTreeViewXMLManager.RunMigration(ParentForm, (FileType)_current);
                    }
                    this.IsDirty = false;
                    Session.CurrentSession.ClearCache();
                    OMSType newitem = OMSType.GetOMSType(_omstype, _current.Code);
                    LoadSingleItem(newitem);
                }
                else
                {
                    StringBuilder message = new StringBuilder();
                    foreach (var errorMessage in errorMessages)
                    {
                        message.Append(errorMessage);
                        message.Append(Environment.NewLine);
                    }
                    
                    System.Windows.Forms.MessageBox.Show(message.ToString(),
                        Session.CurrentSession.Resources.GetResource("VALIDWARN", "Validation warning", "").Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    
                    return false;
                }
            }
			return true;
		}

	    private bool EntityValidation(out List<string> errorMessages)
	    {
            errorMessages = new List<string>();
	        var nullValueMessageTemplate = "Property \"{0}\" should not be null";
            
            if (string.IsNullOrWhiteSpace(_current.Code))
	        {
                errorMessages.Add(string.Format(nullValueMessageTemplate, "Code"));
            }

            if (string.IsNullOrWhiteSpace(_current.Description))
            {
                errorMessages.Add(string.Format(nullValueMessageTemplate, "Description"));
            }

            if (_current.GetType() == typeof(DocType))
	        {
                var docType = _current as DocType;
                if (docType != null)
                {
                    if (string.IsNullOrWhiteSpace(docType.DefaultDocExtension))
                    {
                        errorMessages.Add(string.Format(nullValueMessageTemplate, "DefaultDocExtension"));
                    }
                    
                    if (string.IsNullOrWhiteSpace(docType.DefaultPrecExtension))
                    {
                        errorMessages.Add(string.Format(nullValueMessageTemplate, "DefaultPrecExtension"));
                    }
                }
            }

	        return !errorMessages.Any();
	    }

        protected override bool CancelData()
		{
			_current.Cancel();
			return true;
		}

		protected override void NewData()
		{
            try
            {
                OMSType newitem = null;
                if (_omstype == typeof(AssociateType))
                {
                    DataTable retval = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("SCRASSTYPENEW", null, EnquiryEngine.EnquiryMode.Add, new Common.KeyValueCollection()) as DataTable;
                    if (retval == null) return;
                    DataRow _row = retval.Rows[0];
                    string Code = Convert.ToString(_row["cmbTemplateType"]);
                    newitem = OMSType.GetOMSType(_omstype, Code);
                    newitem = newitem.Clone();
                    FWBS.OMS.AssociateType _type = newitem as FWBS.OMS.AssociateType;
                    _type.Code = Convert.ToString(_row["txtCode"]);
                    _type.Description = Convert.ToString(_row["txtDescription"]);
                    _type.SetExtraInfo("typeActive", true);
                }
                else if (_omstype == typeof(FileType))
                {
                    DataTable retval = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("SCRFILTYPENEW", null, EnquiryEngine.EnquiryMode.Add, new Common.KeyValueCollection()) as DataTable;
                    if (retval == null) return;
                    DataRow _row = retval.Rows[0];
                    string Code = Convert.ToString(_row["cmbTemplateType"]);
                    newitem = OMSType.GetOMSType(_omstype, Code);
                    newitem = newitem.Clone();
                    FWBS.OMS.FileType _type = newitem as FWBS.OMS.FileType;
                    _type.Code = Convert.ToString(_row["txtCode"]);
                    _type.Description = Convert.ToString(_row["txtDescription"]);
                    _type.DefaultDepartment = Convert.ToString(_row["cboDepartment"]);
                    _type.DefaultFundingCode = Convert.ToString(_row["cboFundType"]);
                    _type.FormTitle.Code = Convert.ToString(_row["cdelkupFormTitle"]);
                    _type.PrecedentLibrary = Convert.ToString(_row["_preclib"]);
                    _type.PrecedentCategory = Convert.ToString(_row["_preccat"]);
                    _type.PrecedentSubCategory = Convert.ToString(_row["_precsubcat"]);
                    _type.PrecedentMinorCategory = Convert.ToString(_row["_precminorcat"]);
                    _type.MilestoneActive = Convert.ToBoolean(_row["chkActive"]);
                    _type.MilestonePlan = Convert.ToString(_row["cboMilestonePlan"]);
                    _type.SetExtraInfo("typeActive", true);
                }
                else if (_omstype == typeof(ClientType))
                {
                    DataTable retval = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("SCRCLITYPENEW", null, EnquiryEngine.EnquiryMode.Add, new Common.KeyValueCollection()) as DataTable;
                    if (retval == null) return;
                    DataRow _row = retval.Rows[0];
                    string Code = Convert.ToString(_row["cboClTempType"]);
                    newitem = OMSType.GetOMSType(_omstype, Code);
                    newitem = newitem.Clone();
                    FWBS.OMS.ClientType _type = newitem as FWBS.OMS.ClientType;
                    _type.Code = Convert.ToString(_row["txtCode"]);
                    _type.Description = Convert.ToString(_row["txtDescription"]);
                    _type.FormTitle.Code = Convert.ToString(_row["cdelkupFormTitle"]);
                    _type.SetExtraInfo("typeActive", true);
                }
                else if (_omstype == typeof(ContactType))
                {
                    DataTable retval = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("SCRCONTYPENEW", null, EnquiryEngine.EnquiryMode.Add, new Common.KeyValueCollection()) as DataTable;
                    if (retval == null) return;
                    DataRow _row = retval.Rows[0];
                    string Code = Convert.ToString(_row["cboConTempType"]);
                    newitem = OMSType.GetOMSType(_omstype, Code);
                    newitem = newitem.Clone();
                    FWBS.OMS.ContactType _type = newitem as FWBS.OMS.ContactType;
                    _type.Code = Convert.ToString(_row["txtCode"]);
                    _type.Description = Convert.ToString(_row["txtDescription"]);
                    _type.FormTitle.Code = Convert.ToString(_row["cdelkupFormTitle"]);
                    _type.SetExtraInfo("typeActive", true);
                }
                else
                {
                    newitem = OMSType.GetOMSType(_omstype, "");
                }

                LoadSingleItem(newitem);
                _current.Code = _current.Code;
                this.IsDirty = true;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

		protected override void DeleteData(string Code)
		{
			OMSType newitem = OMSType.GetOMSType(_omstype, Code);
            Bind(newitem);
            _current.Delete();
		}
		

		protected override string SearchListName
		{
			get
			{
				return OMSType.GetTypesSearchList(_omstype);
			}
		}


		protected override bool Restore(string Code)
		{
			OMSType newitem = OMSType.GetOMSType(_omstype, Code);
            Bind(newitem);
            bool ret = _current.Restore();
            LoadSingleItem(_current);
            return ret;
		}

		protected override void Clone(string Code)
		{
			OMSType newitem = OMSType.GetOMSType(_omstype, Code);
            Unbind();
            newitem = newitem.Clone();
            LoadSingleItem(newitem);
		}


        protected override void ShowList()
        {
            Session.CurrentSession.CurrentFileTypes.Clear();
            Session.CurrentSession.CurrentClientTypes.Clear();
            Session.CurrentSession.CurrentContactTypes.Clear();
            Session.CurrentSession.CurrentAssociateTypes.Clear();
            Session.CurrentSession.CurrentUserTypes.Clear();
            Session.CurrentSession.CurrentFeeEarnerTypes.Clear();
            Session.CurrentSession.CurrentCommandCentreTypes.Clear();
            Session.CurrentSession.CurrentDocumentTypes.Clear();
            Session.CurrentSession.CurrentPrecedentTypes.Clear();
            base.ShowList();
        }
	

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			if (e.ChangedItem.Label == "Code")
			{
				labSelectedObject.Text = _current.Code + " - " + _omstype.Name;
			}
		}


		#region Rendering
		
		protected void ClearPanel()
		{
            Global.RemoveAndDisposeControls(pnlInfoBack);
		}
			
		protected void ClearTab()
		{
			tbTabEditor.TabPages.Clear();
		}

		protected void AddTab(int index, object value)
		{
			FWBS.OMS.OMSType.Tab tab = (FWBS.OMS.OMSType.Tab)value;
			TabPage tp = new TabPage(tab.Description);
			tp.ImageIndex = tab.Glyph;
			ucEntityInformation info = new ucEntityInformation(tab.Description);
			info.Tag = value;
			info.OMSObjectCode.Text = tab.OMSObjectCode;
			info.ObjectType.Text = tab.SourceType.ToString();
			info.Code.Text = tab.Source;
			info.Dock = DockStyle.Fill;
			info.lnkCopy.LinkClicked +=new LinkLabelLinkClickedEventHandler(lnkCopy_LinkClicked);
			tp.Controls.Add(info);
			tbTabEditor.AddTabPage(tp);
		}

		protected void AddPanel(int index, object value)
		{
			FWBS.OMS.OMSType.Panel pnl = (FWBS.OMS.OMSType.Panel)value;
			FWBS.OMS.UI.Windows.ucPanelNav n = new FWBS.OMS.UI.Windows.ucPanelNav(pnl.Description, pnl.Parameter,pnl.Height,pnl.Expanded);
			n.ButtonStyle = (NavButtonStyle)Common.ConvertDef.ToEnum(pnl.Glyph, NavButtonStyle.Grey);
			n.LockOpenClose=true;
			n.Dock = DockStyle.Top;
			pnlInfoBack.Controls.Add(n);
			n.Brightness = _current.PanelBrightness;
			n.BringToFront();
		}

		#endregion

		private void ucOMSType_ParentChanged(object sender, System.EventArgs e)
		{
		}

		private void timRefresh_Tick(object sender, System.EventArgs e)
		{
			timRefresh.Enabled = false;
			this.Refresh();
		}

		private void lnkCopy_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (IsObjectDirty())
			{
				frmCopyTabToo _copy = new frmCopyTabToo(_current.SearchListName, _current.Code);
				if (_copy.ShowDialog() == DialogResult.OK)
				{
					FWBS.OMS.OMSType.Tab selectedtab = null;
					if (sender is Control)
					{
						Control ctrl = sender as Control;
						if (ctrl.Parent != null && ctrl.Parent.Parent is ucEntityInformation)
						{
							ucEntityInformation selected = ctrl.Parent.Parent as ucEntityInformation;
							if (selected.Tag is FWBS.OMS.OMSType.Tab)
							{
								selectedtab = selected.Tag as FWBS.OMS.OMSType.Tab;
							}
						}
					}
					foreach (string Code in Convert.ToString(_copy.eCLCollectionSelector1.Value).Split(','))
					{
						OMSType _new = OMSType.GetOMSType(_omstype, Code);
						bool found = false;
						foreach (FWBS.OMS.OMSType.Tab _tab in _new.Tabs)
						{
							if (selectedtab.Code == _tab.Code)
							{
								found = true;
								if (MessageBox.ShowYesNoQuestion("TABALREADY","The Tab already exists on Type '[%1%] %2%'. Do you wish to overwrite?",true,_new.Code,_new.Description) == DialogResult.Yes)
									_tab.Assign(selectedtab);
							}
						}
						if (found == false)
						{
							FWBS.OMS.OMSType.Tab newtab	= new FWBS.OMS.OMSType.Tab(_new);
							newtab.Assign(selectedtab);
							_new.Tabs.Add(newtab);
						}
						_new.Update();
					}
					MessageBox.ShowInformation("TABCOPYSUC","Tab Copied successfully...");
					Session.CurrentSession.ClearCache(true);
				}
			}
		}

		private void lnkAddExtendedData_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if (IsObjectDirty())
			{
				frmCopyTabToo _copy = new frmCopyTabToo(_current.SearchListName, _current.Code);
				if (_copy.ShowDialog() == DialogResult.OK)
				{
					foreach (string Code in Convert.ToString(_copy.eCLCollectionSelector1.Value).Split(','))
					{
						OMSType _new = OMSType.GetOMSType(_omstype, Code);
						foreach (FWBS.OMS.OMSType.ExtendedData fromext in _current.ExtData)
						{
							FWBS.OMS.OMSType.ExtendedData ext = new FWBS.OMS.OMSType.ExtendedData(_new);
							ext.Code = fromext.Code;
							_new.ExtData.Add(ext);
						}
						_new.Update();
					}
					MessageBox.ShowInformation("EXTDATAADD","Extended Data Added successfully...");
					Session.CurrentSession.ClearCache(true);
				}
			}
		}


        #region Code Added October 2015 for MSv7


        private void SetupEvents()
        {
            this.radTreeView1.MouseUp -= new MouseEventHandler(treeView_MouseUp);
            this.radTreeView1.MouseUp += new MouseEventHandler(treeView_MouseUp);
        }


        private void BuildTreeView()
        {
            if(this.radTreeView1 != null)
                this.radTreeView1.Nodes.Clear();

            Dictionary<TabPage, OMSType.Tab> tabs = new Dictionary<TabPage, OMSType.Tab>();

            foreach (TabPage tp in tbTabEditor.TabPages)
            {
                foreach (Control c in tp.Controls)
                {
                    if (c.GetType() == typeof(ucEntityInformation))
                    {
                        ucEntityInformation ei = (ucEntityInformation)c;
                        FWBS.OMS.OMSType.Tab tab = (FWBS.OMS.OMSType.Tab)ei.Tag;
                        tp.Name = tab.Source;
                        tabs.Add(tp, tab);
                    }
                }
            }

            var tvb = new FWBS.OMS.UI.Windows.TreeViewNavigation.TreeViewBuilder(this.radTreeView1,tabs,this.tbTabEditor);

            if (tvb.Build())
            {
                SubscribeToTreeNavigationEvents();
            }
        }


        private void SubscribeToTreeNavigationEvents()
        {
            radTreeView1.NodeFormatting -= TreeViewNavigation.TreeViewFormatter.NodeFormatting;
            radTreeView1.NodeFormatting += TreeViewNavigation.TreeViewFormatter.NodeFormatting;
        }

        private void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.radTreeView1.Nodes.Count > 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    Telerik.WinControls.UI.RadTreeNode node = radTreeView1.SelectedNode;
                    ShowEntityInformation(Convert.ToString(node.Tag));
                }
            }
        }


        private void ShowEntityInformation(string code)
        {
            bool found = false;
            foreach (TabPage p in tbTabEditor.TabPages)
            {
                foreach (Control c in p.Controls)
                {
                    if (c.GetType() == typeof(ucEntityInformation))
                    {
                        ucEntityInformation ei = (ucEntityInformation)c;
                        FWBS.OMS.OMSType.Tab tab = (FWBS.OMS.OMSType.Tab)ei.Tag;
                        if (tab.Source == code)
                        {
                            tbTabEditor.SelectedTab = p;
                            found = true;
                        }
                    }
                }
                if (found)
                    break;
            }
        }

        #endregion

        protected override void CloseAndReturnToList()
        {
            if (base.IsDirty)
            {
                bool validationFailed;
                DialogResult? dr = this.IsObjectDirtyDialogResult2(out validationFailed);
                if (validationFailed && dr == null)
                {
                    return;
                }
                else
                {
                    if (dr != DialogResult.Cancel)
                    {
                        _current = null;
                        base.ShowList();
                    }
                }
            }
            else
            {
                ShowList();
            }
        }


        public DialogResult? IsObjectDirtyDialogResult2(out bool validationFailed)
        {
            validationFailed = false;

            if (IsDirty)
            {
                base.btnBlue.Focus();
                DialogResult dr = System.Windows.Forms.MessageBox.Show(this.EditParent, Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?", "").Text, "OMS Admin", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    List<string> errorMessages;
                    if (EntityValidation(out errorMessages))
                    {
                        base.SaveChanges();
                        if (IsDirty) return DialogResult.Yes;
                    }
                    else
                    {
                        StringBuilder message = new StringBuilder();
                        foreach (var errorMessage in errorMessages)
                        {
                            message.Append(errorMessage);
                            message.Append(Environment.NewLine);
                        }
                        
                        System.Windows.Forms.MessageBox.Show(message.ToString(),
                            Session.CurrentSession.Resources.GetResource("VALIDWARN", "Validation warning", "").Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        validationFailed = true;
                    }

                }
                if (dr == DialogResult.No)
                {
                    IsDirty = false;
                    CancelData();
                    return DialogResult.No;
                }
                if (dr == DialogResult.Cancel) return DialogResult.Cancel;
            }
            return null;
        }
	}
}

