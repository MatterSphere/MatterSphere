using System;
using System.Data;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// AnOMS type editing admin kit control.
    /// </summary>
    public class ucOMSType : FWBS.OMS.UI.Windows.Admin.ucEditBase2
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
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.DataGridTableStyle lstEntities;
		private System.Windows.Forms.DataGridTextBoxColumn ecNone;
		private FWBS.OMS.UI.Windows.DataGridImageColumn ecICode;
		private System.Windows.Forms.Timer timRefresh;
        private System.Windows.Forms.LinkLabel lnkAddExtendedData;
		private System.Windows.Forms.DataGridTextBoxColumn ecDesc;
		
		#endregion

		#region Contructors

		public ucOMSType()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		}


		public ucOMSType(frmMain mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : base(mainparent,editparent, Params)
		{
            _omstype = Session.CurrentSession.TypeManager.Load(Convert.ToString(Params["Type"].Value));
			
			InitializeComponent();
			
			propertyGrid1.HelpVisible=true;
			this.tbReturn.Text = "Return";

			ecICode.ImageList = FWBS.OMS.UI.Windows.Images.Entities();
			lstList.Text = "Refresh";
			this.BresourceLookup1.SetLookup(this.tbSave,new FWBS.OMS.UI.Windows.ResourceLookupItem("Save","Save",""));
		}

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.lstEntities = new System.Windows.Forms.DataGridTableStyle();
            this.ecNone = new System.Windows.Forms.DataGridTextBoxColumn();
            this.ecICode = new FWBS.OMS.UI.Windows.DataGridImageColumn();
            this.ecDesc = new System.Windows.Forms.DataGridTextBoxColumn();
            this.timRefresh = new System.Windows.Forms.Timer(this.components);
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.pnlProperties.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.tbTabEditor);
            this.tpEdit.Controls.Add(this.splitter1);
            this.tpEdit.Controls.Add(this.panel2);
            this.tpEdit.Controls.Add(this.pnlProperties);
            this.tpEdit.Controls.Add(this.pnlInfoBack);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlInfoBack, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlProperties, 0);
            this.tpEdit.Controls.SetChildIndex(this.panel2, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
            this.tpEdit.Controls.SetChildIndex(this.tbTabEditor, 0);
            // 
            // pnlEdit
            // 
            this.pnlEdit.BackColor = System.Drawing.Color.White;
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
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(315, 50);
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
            this.pnlProperties.Location = new System.Drawing.Point(319, 50);
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
            this.BresourceLookup1.SetLookup(this.lnkAddExtendedData, new FWBS.OMS.UI.Windows.ResourceLookupItem("AddExtData", "Add Extended Data from this Type to Selected Types", ""));
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
            this.BresourceLookup1.SetLookup(this.tbNewTab, new FWBS.OMS.UI.Windows.ResourceLookupItem("New", "New", ""));
            this.tbNewTab.Name = "tbNewTab";
            this.tbNewTab.Text = "New";
            // 
            // tbRemoveTab
            // 
            this.tbRemoveTab.ImageIndex = 4;
            this.BresourceLookup1.SetLookup(this.tbRemoveTab, new FWBS.OMS.UI.Windows.ResourceLookupItem("Remove", "Remove", ""));
            this.tbRemoveTab.Name = "tbRemoveTab";
            this.tbRemoveTab.Text = "Remove";
            // 
            // tbTabEditor
            // 
            this.tbTabEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbTabEditor.Location = new System.Drawing.Point(162, 50);
            this.tbTabEditor.Multiline = true;
            this.tbTabEditor.Name = "tbTabEditor";
            this.tbTabEditor.SelectedIndex = 0;
            this.tbTabEditor.Size = new System.Drawing.Size(153, 333);
            this.tbTabEditor.TabIndex = 201;
            // 
            // pnlInfoBack
            // 
            this.pnlInfoBack.AutoScroll = true;
            this.pnlInfoBack.BackColor = System.Drawing.Color.White;
            this.pnlInfoBack.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlInfoBack.Location = new System.Drawing.Point(0, 50);
            this.pnlInfoBack.Name = "pnlInfoBack";
            this.pnlInfoBack.Padding = new System.Windows.Forms.Padding(10);
            this.pnlInfoBack.Size = new System.Drawing.Size(158, 333);
            this.pnlInfoBack.TabIndex = 202;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(158, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(4, 333);
            this.panel2.TabIndex = 203;
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
            this.BresourceLookup1.SetLookup(this.ecICode, new FWBS.OMS.UI.Windows.ResourceLookupItem("Code", "Code", ""));
            this.ecICode.MappingName = "typeCode";
            this.ecICode.ReadOnly = true;
            this.ecICode.Width = 125;
            // 
            // ecDesc
            // 
            this.ecDesc.Format = "";
            this.ecDesc.FormatInfo = null;
            this.BresourceLookup1.SetLookup(this.ecDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("Description", "Description", ""));
            this.ecDesc.MappingName = "typeDesc";
            this.ecDesc.ReadOnly = true;
            this.ecDesc.Width = 400;
            // 
            // timRefresh
            // 
            this.timRefresh.Interval = 500;
            this.timRefresh.Tick += new System.EventHandler(this.timRefresh_Tick);
            // 
            // ucOMSType
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Name = "ucOMSType";
            this.ParentChanged += new System.EventHandler(this.ucOMSType_ParentChanged);
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.pnlProperties.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
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
			OMSType newitem = OMSType.GetOMSType(_omstype, type);
            LoadSingleItem(newitem);
		}

		protected void LoadSingleItem(OMSType type)
		{
            if (type == _current)
                return;

            ShowEditor(false);
			this.EditParent.SuspendLayout();
			labSelectedObject.Text = type.Code + " - " + _omstype.Name;
			tbTabEditor.ImageList = FWBS.OMS.UI.Windows.Images.CoolButtons16();

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

		protected override bool UpdateData()
		{
			if (_current.IsDirty)
			{
				_current.Update();
				this.IsDirty=false;
				Session.CurrentSession.ClearCache();
				OMSType newitem = OMSType.GetOMSType(_omstype, _current.Code);
                LoadSingleItem(newitem);
			}
			return true;
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
						if (ctrl.Parent != null && ctrl.Parent.Parent.Parent.Parent is ucEntityInformation)
						{
							ucEntityInformation selected = ctrl.Parent.Parent.Parent.Parent as ucEntityInformation;
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

	}
}

