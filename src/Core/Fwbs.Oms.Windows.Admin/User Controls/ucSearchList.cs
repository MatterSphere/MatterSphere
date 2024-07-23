using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Dashboard;
using FWBS.OMS.Data;
using FWBS.OMS.Design.CodeBuilder;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.UI.Windows.Design;


namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// 27000 Search List Editor for the Admin Kit
    /// </summary>
    public class ucSearchList : FWBS.OMS.UI.Windows.Admin.ucEditBase2, IObjectUnlock
	{
		private FWBS.OMS.UI.Windows.ucSearchControl ucSearchControl1;
		private System.Windows.Forms.Splitter splitter1;
		private System.ComponentModel.IContainer components = null;
		private SearchListEditor _currentobj;

        private System.Windows.Forms.ToolBarButton tbScript;		
        private System.Windows.Forms.ToolBarButton tbEditForm;
        private System.Windows.Forms.ToolBarButton tbSpVersioning;
        private System.Windows.Forms.ToolBarButton tbCheckin;
        private System.Windows.Forms.ToolBarButton tbCompare;

        private FWBS.OMS.UI.TabControl tabProperties;
		private System.Windows.Forms.TabPage tabProps;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.Splitter splitter2;
		private FWBS.Common.UI.Windows.eXPPanel pnlActions;
		private System.Windows.Forms.LinkLabel lnkSetColumns;
		private System.Windows.Forms.LinkLabel lnkRegSearchObj;
		private System.Windows.Forms.LinkLabel lnkRegisterGroup;
		private System.Windows.Forms.TabPage tabEvents;
        private FWBS.OMS.Design.CodeBuilder.DataGridEvents dataGridEvents1;
        private FWBS.OMS.Design.CodeBuilder.CodeWindow _codeWindow;
		private long _enqversion = -1;
        private LockState ls = new LockState();
        private IMainParent _mainparent;
        private bool objectlocking;

        SearchListVersionDataArchiver searchlistVersionDataArchiver;

        VersionComparisonSelector vcs;

        public ucSearchList()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

        }

		public ucSearchList(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Group) : base(mainparent,editparent, Group)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

            _mainparent = mainparent;

            objectlocking = Session.CurrentSession.ObjectLocking;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
		{
            if (_currentobj != null && objectlocking)
                ls.UnlockSearchListObject(_currentobj.Code);

            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (_mainparent != null)
                {
                    if (_codeWindow != null)
                    {
                        _codeWindow.Close();
                        _codeWindow.FormClosing -= CodeWindow_FormClosing;
                        _codeWindow.Dispose();
                        _codeWindow = null;
                    }
                }
            }
            base.Dispose(disposing);

		}

		protected override string SearchListName
		{
			get
			{
				return "ADMSEARCHLISTS";
			}
		}

        public override bool IsDirty
        {
            get
            {
                return base.IsDirty || (_codeWindow != null && _codeWindow.IsDirty);
            }
            set
            {
                base.IsDirty = value;
                if (_codeWindow != null && value == false) _codeWindow.IsDirty = value;
            }
        }
		
		protected override bool UpdateData()
		{
			try
			{
                tabProperties.SelectedIndex = 0;
                if (!String.IsNullOrEmpty(_currentobj.Script.Code) && (_codeWindow != null) && (_codeWindow.IsDirty || _codeWindow.ScriptGen.IsDirty))
                {
                    if (CodeWindow.SaveAndCompile() == false)
                    {
                        CodeWindow.Show();
                        return false;
                    }
                }
                _currentobj.Update();
				this.IsDirty=false;
                if (objectlocking)
                    ls.MarkObjectAsClosed(_currentobj.Script.Code, LockableObjects.Script);
                LoadSearchList(_currentobj.Code, true, false);
                if (objectlocking)
                {
                    if (!ls.CheckForOpenObjects(_currentobj.Code, LockableObjects.SearchList))
                    {
                        ls.LockSearchListObject(_currentobj.Code);
                        ls.MarkObjectAsOpen(_currentobj.Code, LockableObjects.SearchList);
                        ManageVersioningButtons(true);
                    }
                }
                else
                {
                    ManageVersioningButtons(true);
                }
                return true;
			}
			catch(Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
				return false;
			}
		}


        [Obsolete("This method is no longer used", false)]
        private void ResizeToolbarButtons()
        {
            this.tbcEdit.Visible = false;
            this.tbcEdit.ButtonSize = new Size(17, 17);
            this.tbcEdit.ButtonSize = new Size(16, 16);
            this.tbcEdit.Visible = true;
        }


		protected override void Clone(string Code)
		{
            try
            {
                LoadSearchList(Code, true, true);
                tbScript.Enabled = true;
                dataGridEvents1.Enabled = true;
            }
            catch (Exception ex)
            {
                if (ex.Message != "Cancel")
                    ErrorBox.Show(ParentForm, ex);
                this.ShowList();

            }
		}
		
		protected override void LoadSingleItem(string Code)
		{
            try
            {
                if (Session.CurrentSession.ObjectLocking)
                {
                    if (!ls.CheckObjectLockState(Code, LockableObjects.SearchList))
                    {
                        if (!ls.CheckIfObjectIsAlreadyOpen(Code, LockableObjects.SearchList))
                        {
                            SetupSearchList(Code);
                            ls.LockSearchListObject(Code);
                            ls.MarkObjectAsOpen(Code, LockableObjects.SearchList);
                            bool moreThanObjectOpen = (ls.CheckForOpenObjects(Code, LockableObjects.SearchList)) ? false : true;
                            ManageVersioningButtons(moreThanObjectOpen);
                        }
                    }
                }
                else
                    SetupSearchList(Code);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

        private void SetupSearchList(string Code)
        {
            LoadSearchList(Code, false, false);
            tbScript.Enabled = true;
            ManageVersioningButtons(true);
            dataGridEvents1.Enabled = true;
        }

		protected override void NewData()
		{
			LoadSearchList("",true,false);
		}

		protected override void DeleteData(string Code)
		{
			if (FWBS.OMS.SearchEngine.SearchList.Delete(Code) == false)
			{
				ErrorBox.Show(ParentForm, new OMSException2("27001","Failed to Delete with code : %1%",new Exception(),true,Code));
			}
		}
		
		private void LoadSearchList(string Code, bool NoErrors, bool Clone, bool EnableButtons = false)
		{
            tbcEdit.Enabled = false;
            ucSearchControl1.Visible = false;
            if (IsObjectDirty())
			{
				CodeWindow.Hide();
                var frmAD = this.EditParent as frmAdminDesktop;
                if (frmAD != null)
                {
                    frmAD.Activated -= new EventHandler(ParentForm_Activated);
                    frmAD.Activated += new EventHandler(ParentForm_Activated);
                }

                if (Code == "" || Clone)
                {
                    labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("Untitled", "Untitled", ""), ResourceLookup.GetLookupText("SCHLISTS", "Search Lists", ""));
                    ShowEditor(true);
                }
                else
                {
                    labSelectedObject.Text = string.Format("{0} - {1}", Code, ResourceLookup.GetLookupText("SCHLISTS", "Search Lists", ""));
                    ShowEditor(false);
                }
				Application.DoEvents();
				Cursor = Cursors.WaitCursor;
                try
                {
                    if (Code == "" && Clone == false)
                        _currentobj = new SearchListEditor();
                    else if (Code != "" && Clone)
                        _currentobj = SearchListEditor.Clone(Code);
                    else
                        _currentobj = new SearchListEditor(Code);
                    _currentobj.ScriptChanged += new EventHandler(_currentobj_ScriptChange);
                    _currentobj.CodeChanged += new EventHandler(_currentobj_CodeChanged);
                    _currentobj.Columns.Cleared += new Crownwood.Magic.Collections.CollectionClear(ClearedColumns);
                    _currentobj.ReturnFields.Cleared += new Crownwood.Magic.Collections.CollectionClear(ClearedColumns);
                    _currentobj.Buttons.Cleared += new Crownwood.Magic.Collections.CollectionClear(ClearedColumns);
                    if (_currentobj.EnquiryForm != "")
                    {
                        tbEditForm.Visible = true;
                    }
                    else
                    {
                        tbEditForm.Visible = false;
                    }

                    try
                    {
                        lnkRegisterGroup.Enabled = !(OmsObject.IsObjectRegistered(_currentobj.SearchListType, OMSObjectTypes.ListGroup));
                        lnkRegSearchObj.Enabled = !(OmsObject.IsObjectRegistered(_currentobj.Code, OMSObjectTypes.List));
                    }
                    catch { }
                    propertyGrid1.SelectedObject = _currentobj;
                    propertyGrid1.HelpVisible = true;
                    CodeWindow.Text = Session.CurrentSession.Resources.GetResource("SLCODEBLD", "Search List Code Builder", "").Text;
                    CodeWindow.Load(_currentobj.Script.ScriptType,_currentobj.Script);
                    dataGridEvents1.CurrentCodeSurface = CodeWindow;
                    dataGridEvents1.SelectedObject = _currentobj.Script.ScriptType;

                    Application.DoEvents();
                    try
                    {
                        ucSearchControl1.Visible = true;
                        FWBS.Common.KeyValueCollection _params = new FWBS.Common.KeyValueCollection();
                        foreach (Parameter p in _currentobj.DataBuilder.Parameters)
                        {
                            _params.Add(p.SQLParameter, p.TestValue);
                        }
                        ucSearchControl1.SetSearchList(_currentobj, false, _params);
                        if (_currentobj.ViewStyle == FWBS.OMS.SearchEngine.SearchListStyle.List || _currentobj.ViewStyle == FWBS.OMS.SearchEngine.SearchListStyle.Filter)
                        {
                            try
                            {
                                ucSearchControl1.Search(false, false);
                            }
                            catch
                            {
                                ucSearchControl1.dgSearchResults.DataSource = null;
                                ucSearchControl1.dgSearchResults.Refresh();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ucSearchControl1.Visible = false;
                        if (NoErrors == false)
                            ErrorBox.Show(ParentForm, ex);
                    }
                }
				finally
				{
					Application.DoEvents();
					Cursor = Cursors.Default;
					tbScript.Enabled = true;
                    tbcEdit.Enabled = true;
                    ucSearchControl1.Visible = true;
                    ManageVersioningButtons(EnableButtons);
				}
			}
		}

        void _currentobj_CodeChanged(object sender, EventArgs e)
        {
            dataGridEvents1.Enabled = true;
            tbScript.Enabled = false;
            ManageVersioningButtons(false);
        }

		private void ClearedColumns()
		{
			IsDirty = true;
		}


        protected override void ShowList()
        {
            if (_codeWindow != null)
            {
                _codeWindow.Close();
                _codeWindow.FormClosing -= CodeWindow_FormClosing;
                _codeWindow.Dispose();
                _codeWindow = null;
            }
            base.ShowList();
            ucSearchControl1.dgSearchResults.DataSource = null;
        }

		public CodeWindow CodeWindow
		{
			get
			{
				if (_codeWindow == null)
				{
					_codeWindow = new CodeWindow();
                    _codeWindow.Init(null);
                    _codeWindow.FormClosing += CodeWindow_FormClosing;
				}
				return _codeWindow;
			}
		}


		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tbScript = new System.Windows.Forms.ToolBarButton();
            this.tbEditForm = new System.Windows.Forms.ToolBarButton();
            this.tbSpVersioning = new System.Windows.Forms.ToolBarButton();
            this.tbCheckin = new System.Windows.Forms.ToolBarButton();
            this.tbCompare = new System.Windows.Forms.ToolBarButton();
            this.ucSearchControl1 = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tabProperties = new FWBS.OMS.UI.TabControl();
            this.tabProps = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlActions = new FWBS.Common.UI.Windows.eXPPanel();
            this.lnkSetColumns = new System.Windows.Forms.LinkLabel();
            this.lnkRegSearchObj = new System.Windows.Forms.LinkLabel();
            this.lnkRegisterGroup = new System.Windows.Forms.LinkLabel();
            this.tabEvents = new System.Windows.Forms.TabPage();
            this.dataGridEvents1 = new FWBS.OMS.Design.CodeBuilder.DataGridEvents();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.tabProperties.SuspendLayout();
            this.tabProps.SuspendLayout();
            this.panel2.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.tabEvents.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.ucSearchControl1);
            this.tpEdit.Controls.Add(this.splitter1);
            this.tpEdit.Controls.Add(this.tabProperties);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.tabProperties, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
            this.tpEdit.Controls.SetChildIndex(this.ucSearchControl1, 0);
            // 
            // labSelectedObject
            // 
            this.labSelectedObject.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // tbcEdit
            // 
            this.tbcEdit.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbScript,
            this.tbEditForm,
            this.tbSpVersioning,
            this.tbCheckin,
            this.tbCompare});
            this.tbcEdit.Enabled = false;
            this.tbcEdit.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbcEdit_ButtonClick);
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
            // lstList
            // 
            this.lstList.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.lstList_SearchButtonCommands2);
            // 
            // tbScript
            // 
            this.tbScript.Enabled = false;
            this.BresourceLookup1.SetLookup(this.tbScript, new FWBS.OMS.UI.Windows.ResourceLookupItem("EditScript", "Edit Script", ""));
            this.tbScript.Name = "tbScript";
            // 
            // tbEditForm
            // 
            this.BresourceLookup1.SetLookup(this.tbEditForm, new FWBS.OMS.UI.Windows.ResourceLookupItem("EditForm", "Edit Form", ""));
            this.tbEditForm.Name = "tbEditForm";
            this.tbEditForm.Text = "Edit Form";
            this.tbEditForm.Visible = false;
            // 
            // tbSpVersioning
            // 
            this.tbSpVersioning.Name = "tbSpVersioning";
            this.tbSpVersioning.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbCheckin
            // 
            this.tbCheckin.Enabled = false;
            this.BresourceLookup1.SetLookup(this.tbCheckin, new FWBS.OMS.UI.Windows.ResourceLookupItem("Checkin", "Check in", ""));
            this.tbCheckin.Name = "tbCheckin";
            // 
            // tbCompare
            // 
            this.tbCompare.Enabled = false;
            this.BresourceLookup1.SetLookup(this.tbCompare, new FWBS.OMS.UI.Windows.ResourceLookupItem("Compare", "Version Administration", ""));
            this.tbCompare.Name = "tbCompare";
            // 
            // ucSearchControl1
            // 
            this.ucSearchControl1.AllowReorderColumns = false;
            this.ucSearchControl1.BackColor = System.Drawing.Color.White;
            this.ucSearchControl1.BackGroundColor = System.Drawing.Color.White;
            this.ucSearchControl1.DisplayResultsCaption = false;
            this.ucSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl1.DoubleClickAction = "None";
            this.ucSearchControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucSearchControl1.GraphicalPanelVisible = true;
            this.ucSearchControl1.Location = new System.Drawing.Point(0, 50);
            this.ucSearchControl1.Name = "ucSearchControl1";
            this.ucSearchControl1.NavCommandPanel = null;
            this.ucSearchControl1.Padding = new System.Windows.Forms.Padding(5);
            this.ucSearchControl1.RefreshOnEnquiryFormRefreshEvent = false;
            this.ucSearchControl1.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.ucSearchControl1.SearchListCode = "";
            this.ucSearchControl1.SearchListType = "";
            this.ucSearchControl1.SearchPanelVisible = false;
            this.ucSearchControl1.Size = new System.Drawing.Size(282, 333);
            this.ucSearchControl1.TabIndex = 199;
            this.ucSearchControl1.ToBeRefreshed = false;
            this.ucSearchControl1.TypeSelectorVisible = false;
            this.ucSearchControl1.CommandExecuting += new FWBS.OMS.UI.Windows.CommandExecutingEventHandler(this.ucSearchControl1_CommandExecuting);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(282, 50);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 333);
            this.splitter1.TabIndex = 200;
            this.splitter1.TabStop = false;
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.tabProps);
            this.tabProperties.Controls.Add(this.tabEvents);
            this.tabProperties.Dock = System.Windows.Forms.DockStyle.Right;
            this.tabProperties.Location = new System.Drawing.Point(287, 50);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.SelectedIndex = 0;
            this.tabProperties.Size = new System.Drawing.Size(262, 333);
            this.tabProperties.TabIndex = 201;
            // 
            // tabProps
            // 
            this.tabProps.BackColor = System.Drawing.Color.White;
            this.tabProps.Controls.Add(this.panel2);
            this.tabProps.Location = new System.Drawing.Point(4, 24);
            this.tabProps.Name = "tabProps";
            this.tabProps.Size = new System.Drawing.Size(254, 305);
            this.tabProps.TabIndex = 0;
            this.tabProps.Text = "Properties";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.propertyGrid1);
            this.panel2.Controls.Add(this.splitter2);
            this.panel2.Controls.Add(this.pnlActions);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(254, 305);
            this.panel2.TabIndex = 203;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.White;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(254, 242);
            this.propertyGrid1.TabIndex = 201;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.ViewBackColor = System.Drawing.Color.White;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 242);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(254, 3);
            this.splitter2.TabIndex = 203;
            this.splitter2.TabStop = false;
            // 
            // pnlActions
            // 
            this.pnlActions.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.Color.White);
            this.pnlActions.BorderLine = true;
            this.pnlActions.Controls.Add(this.lnkSetColumns);
            this.pnlActions.Controls.Add(this.lnkRegSearchObj);
            this.pnlActions.Controls.Add(this.lnkRegisterGroup);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlActions.Location = new System.Drawing.Point(0, 245);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(3);
            this.pnlActions.Size = new System.Drawing.Size(254, 60);
            this.pnlActions.TabIndex = 202;
            // 
            // lnkSetColumns
            // 
            this.lnkSetColumns.BackColor = System.Drawing.Color.White;
            this.lnkSetColumns.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkSetColumns.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lnkSetColumns.Location = new System.Drawing.Point(3, 3);
            this.BresourceLookup1.SetLookup(this.lnkSetColumns, new FWBS.OMS.UI.Windows.ResourceLookupItem("SetColumns", "Set Column Widths", ""));
            this.lnkSetColumns.Name = "lnkSetColumns";
            this.lnkSetColumns.Size = new System.Drawing.Size(248, 16);
            this.lnkSetColumns.TabIndex = 2;
            this.lnkSetColumns.TabStop = true;
            this.lnkSetColumns.Text = "Set Columns Widths";
            this.lnkSetColumns.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkGetColumns_LinkClicked);
            // 
            // lnkRegSearchObj
            // 
            this.lnkRegSearchObj.BackColor = System.Drawing.Color.White;
            this.lnkRegSearchObj.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lnkRegSearchObj.Location = new System.Drawing.Point(3, 25);
            this.BresourceLookup1.SetLookup(this.lnkRegSearchObj, new FWBS.OMS.UI.Windows.ResourceLookupItem("RegisterSO", "Register this Search Object", ""));
            this.lnkRegSearchObj.Name = "lnkRegSearchObj";
            this.lnkRegSearchObj.Size = new System.Drawing.Size(248, 16);
            this.lnkRegSearchObj.TabIndex = 1;
            this.lnkRegSearchObj.TabStop = true;
            this.lnkRegSearchObj.Text = "Register this Search Object";
            this.lnkRegSearchObj.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRegSearchObj_LinkClicked);
            // 
            // lnkRegisterGroup
            // 
            this.lnkRegisterGroup.BackColor = System.Drawing.Color.White;
            this.lnkRegisterGroup.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lnkRegisterGroup.Location = new System.Drawing.Point(3, 41);
            this.BresourceLookup1.SetLookup(this.lnkRegisterGroup, new FWBS.OMS.UI.Windows.ResourceLookupItem("RegisterSG", "Register Search Group", ""));
            this.lnkRegisterGroup.Name = "lnkRegisterGroup";
            this.lnkRegisterGroup.Size = new System.Drawing.Size(248, 16);
            this.lnkRegisterGroup.TabIndex = 0;
            this.lnkRegisterGroup.TabStop = true;
            this.lnkRegisterGroup.Text = "Register Search Group";
            this.lnkRegisterGroup.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkRegisterGroup_LinkClicked);
            // 
            // tabEvents
            // 
            this.tabEvents.Controls.Add(this.dataGridEvents1);
            this.tabEvents.Location = new System.Drawing.Point(4, 24);
            this.tabEvents.Name = "tabEvents";
            this.tabEvents.Size = new System.Drawing.Size(254, 305);
            this.tabEvents.TabIndex = 1;
            this.tabEvents.Text = "Events";
            // 
            // dataGridEvents1
            // 
            this.dataGridEvents1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.dataGridEvents1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridEvents1.Enabled = false;
            this.dataGridEvents1.Location = new System.Drawing.Point(0, 0);
            this.dataGridEvents1.Name = "dataGridEvents1";
            this.dataGridEvents1.Padding = new System.Windows.Forms.Padding(1);
            this.dataGridEvents1.Size = new System.Drawing.Size(254, 305);
            this.dataGridEvents1.TabIndex = 1;
            this.dataGridEvents1.NewScript += new System.EventHandler(this.dataGridEvents1_NewScript);
            this.dataGridEvents1.CodeButtonClick += new System.EventHandler(this.dataGridEvents1_CodeButtonClick);
            // 
            // ucSearchList
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Name = "ucSearchList";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.tabProperties.ResumeLayout(false);
            this.tabProps.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.tabEvents.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

        private void _currentobj_ScriptChange(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentobj.Code))
            {
                if (_currentobj.Script.State != ObjectState.Added)
                    _currentobj.NewScript();
                CodeWindow.Load(_currentobj.Script.ScriptType, _currentobj.Script);
                dataGridEvents1.CurrentCodeSurface = CodeWindow;
                dataGridEvents1.SelectedObject = _currentobj.Script.ScriptType;
            }
        }

		private void dataGridEvents1_CodeButtonClick(object sender, System.EventArgs e)
		{
            tbScript_Click();
            if (CodeWindow.HasMethod(dataGridEvents1.SelectedMethodName))
            {
                CodeWindow.GotoMethod(dataGridEvents1.SelectedMethodName);
            }
            else
            {
                CodeWindow.GenerateHandler(dataGridEvents1.SelectedMethodName, new GenerateHandlerInfo() { DelegateType = dataGridEvents1.SelectedMethodData.Delegate });
            }
		}

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			if (_currentobj.Code == "")
				labSelectedObject.Text = string.Format("{0} - {1}", ResourceLookup.GetLookupText("Untitled", "Untitled", ""), ResourceLookup.GetLookupText("SCHLISTS", "Search Lists", ""));
			else
				labSelectedObject.Text = string.Format("{0} - {1}", _currentobj.Code, ResourceLookup.GetLookupText("SCHLISTS", "Search Lists", ""));
			IsDirty=true;
		}

		private void lnkRegisterGroup_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			DataTable dt = null;
			FWBS.Common.KeyValueCollection _param = new FWBS.Common.KeyValueCollection();
			_param.Add("objCode",_currentobj.SearchType);
			_param.Add("objHelp","[List]");
			dt = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.RegisterOMSObject),null,EnquiryMode.Add,false, _param) as DataTable;
			
			if (dt != null)
			{
				string _code = Convert.ToString(dt.Rows[0]["objCode"]);
				string _description = Convert.ToString(dt.Rows[0]["objdescription"]);
				string _type = Convert.ToString(dt.Rows[0]["objType"]);
				string _web = Convert.ToBoolean(dt.Rows[0]["chkWEB"]) == true ? _currentobj.Code : "";
				string _win = Convert.ToBoolean(dt.Rows[0]["chkWindows"]) == true ? _currentobj.Code : "";
				string _pda = Convert.ToBoolean(dt.Rows[0]["chkPDA"]) == true ? _currentobj.Code : "";
				try
				{
					OmsObject.Register(_code,OMSObjectTypes.ListGroup,_type,_description,Convert.ToString(dt.Rows[0]["objHelp"]),_win,_web,_pda);
					lnkRegisterGroup.Enabled = !(OmsObject.IsObjectRegistered(_currentobj.SearchListType,OMSObjectTypes.ListGroup));
					lnkRegSearchObj.Enabled = !(OmsObject.IsObjectRegistered(_currentobj.Code,OMSObjectTypes.List));
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}			
			}
		}

		private void lnkRegSearchObj_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			DataTable dt = null;
			FWBS.Common.KeyValueCollection _param = new FWBS.Common.KeyValueCollection();
			_param.Add("objCode",_currentobj.Code);
			_param.Add("objHelp","[SearchListType]");
			dt = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.RegisterOMSObject),null,EnquiryMode.Add,false, _param) as DataTable;
			if (dt != null)
			{
				string _code = Convert.ToString(dt.Rows[0]["objCode"]);
				string _description = Convert.ToString(dt.Rows[0]["objdescription"]);
				string _type = Convert.ToString(dt.Rows[0]["objType"]);
				string _web = Convert.ToBoolean(dt.Rows[0]["chkWEB"]) == true ? _currentobj.Code : "";
				string _win = Convert.ToBoolean(dt.Rows[0]["chkWindows"]) == true ? _currentobj.Code : "";
				string _pda = Convert.ToBoolean(dt.Rows[0]["chkPDA"]) == true ? _currentobj.Code : "";

                string xml = null;
                if (Convert.ToBoolean(dt.Rows[0]["chkDashboard"]))
                {
                    var height = Convert.ToInt16(dt.Rows[0]["seHeight"]);
                    var width = Convert.ToInt16(dt.Rows[0]["seWidth"]);
                    var priority = Convert.ToInt16(dt.Rows[0]["sePriority"]);
                    xml = DashboardConfigProvider.CreateXml(_currentobj.Code, new Size(width, height), priority);
                }

                try
				{
					OmsObject.Register(_code,OMSObjectTypes.List,_type,_description,Convert.ToString(dt.Rows[0]["objHelp"]),_win,_web,_pda, xml);
					lnkRegisterGroup.Enabled = !(OmsObject.IsObjectRegistered(_currentobj.SearchListType,OMSObjectTypes.ListGroup));
					lnkRegSearchObj.Enabled = !(OmsObject.IsObjectRegistered(_currentobj.Code,OMSObjectTypes.List));
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}	
			}
		}

		private void pnlActions_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
		
		}

		private void lnkGetColumns_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			if (MessageBox.ShowYesNoQuestion("SETCOLUMNS?","Would you like to store the Column widths as currently displayed?") == DialogResult.Yes)
			{
                foreach (DataGridViewColumn viewColumn in ucSearchControl1.dgSearchResults.Columns)
                {
                    foreach (Columns objColumn in _currentobj.Columns)
                    {
                        if (viewColumn.DataPropertyName == objColumn.MappingName &&
                            viewColumn.HeaderText.StartsWith(Session.CurrentSession.Terminology.Parse(objColumn.Text.Description, true)))
                        {
                            objColumn.Width = viewColumn.Width;
                            break;
                        }
                    }
                }
                IsDirty = true;
			}
		}


		new private void tbcEdit_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == tbEditForm)
			{
                tbEditForm_Click();
			}
			else if (e.Button == tbScript)
			{
                tbScript_Click();
            }
            if (e.Button == tbCheckin)
            {
                if(CheckObjectIsValid())
                    tbCheckin_Click();
            }
            if (e.Button == tbCompare)
            {
                if (CheckObjectIsValid()) 
                    tbCompare_Click();
            }
		}


        private void tbEditForm_Click()
        {
            FWBS.OMS.Design.frmDesigner frmDesigner1;
            frmDesigner1 = _mainparent.OpenWindows["ED"] as FWBS.OMS.Design.frmDesigner;
            if (frmDesigner1 == null)
            {
                FWBS.OMS.Design.frmDesigner frmDesigner = MainParent.Action("ED", "") as FWBS.OMS.Design.frmDesigner;
                frmDesigner.frmDesigner_Initialize(true, _currentobj.EnquiryForm, "", "");
                _enqversion = frmDesigner.EnquiryFormVersion;
                ls.MarkObjectAsOpen(_currentobj.EnquiryForm, LockableObjects.EnquiryForm);
                ManageVersioningButtons(false);
            }
            else
                System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("FRMDESINST","There is already an instance of form designer open.\n\nPlease close it before atempting to open this Search List's filter form.","").Text, "Form Designer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void tbScript_Click()
        {
            if (_currentobj.ScriptName == "")
            {
                _currentobj.ScriptName = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_currentobj.Code);
            }
            CodeWindow.Show();
            CodeWindow.BringToFront();

            dataGridEvents1.CurrentCodeSurface = CodeWindow;
            dataGridEvents1.SelectedObject = _currentobj.Script.ScriptType;

            if (objectlocking)
                ls.MarkObjectAsOpen(_currentobj.Script.Code, LockableObjects.Script);
            ManageVersioningButtons(false);
        }


        // ************************************************************************************************
        //
        // CHECK IN
        //
        // ************************************************************************************************


        private void tbCheckin_Click()
        {
            CreateAndArchiveData(true);
        }

        private void CreateAndArchiveData(bool CheckInOnly)
        {
            // To go wherever the ‘Save and Version’ button/menu item will be located
            var searchlistVersionData = GetSearchListFormVersionDataSet();
            searchlistVersionDataArchiver = new SearchListVersionDataArchiver();
            if(!CheckInOnly)
                searchlistVersionDataArchiver.Saved += new EventHandler(searchlistVersionDataArchiver_Saved);
            searchlistVersionDataArchiver.SaveData(searchlistVersionData, _currentobj.Code, _currentobj.Version, versionID: Guid.NewGuid());
        }

        private void searchlistVersionDataArchiver_Saved(object sender, EventArgs e)
        {
            searchlistVersionDataArchiver.Saved -= new EventHandler(searchlistVersionDataArchiver_Saved);
            OpenComparisonTool();
        }
        
        // Methods to build the DataSet for the Archiver objects to use.
        private DataTable BuildVersionDataTable()
        {
            string sql = @"select * from dbSearchListConfig where schCode = @code";
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("code", _currentobj.Code));
            System.Data.DataTable dt = connection.ExecuteSQL(sql, parList);
            dt.TableName = "dbSearchListConfig";
            return dt;
        }

        private DataSet GetSearchListFormVersionDataSet()
        {
            var versionDataToSave = new DataSet();
            var slconfig = BuildVersionDataTable();
            versionDataToSave.Tables.Add(slconfig);
            return versionDataToSave;
        }


        // ************************************************************************************************
        //
        // COMPARE
        //
        // ************************************************************************************************

        private void tbCompare_Click()
        {
            if (CheckObjectInIfNecessary())
                CreateAndArchiveData(false);
            else
                OpenComparisonTool();
        }

        private bool CheckObjectInIfNecessary()
        {
            string sql = "select * from dbSearchListVersionData where Code = @code and Version = @version";
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("code", _currentobj.Code));
            parList.Add(connection.CreateParameter("version", _currentobj.Version));
            System.Data.DataTable dt = connection.ExecuteSQL(sql, parList);
            if (dt != null && dt.Rows.Count > 0)
                return false;
            else
                return true;
        }

        private void OpenComparisonTool()
        {
            vcs = new VersionComparisonSelector(_currentobj.Code, UI.Windows.LockableObjects.SearchList);
            vcs.RestorationCompleted += new EventHandler<RestorationCompletedEventArgs>(vcs_RestorationCompleted);
            var result = vcs.ShowDialog();
            vcs.StartPosition = FormStartPosition.CenterScreen;
            vcs.FormClosing += new FormClosingEventHandler(vcs_FormClosing);
        }

        private void vcs_RestorationCompleted(object sender, RestorationCompletedEventArgs e)
        {
            LoadSearchList(_currentobj.Code, true, false, true);
        }

        private void vcs_FormClosing(object sender, FormClosingEventArgs e)
        {
            vcs.FormClosing -= new FormClosingEventHandler(vcs_FormClosing);
            vcs.RestorationCompleted -= new EventHandler<RestorationCompletedEventArgs>(vcs_RestorationCompleted);
        }

        private bool CheckObjectIsValid()
        {
            return (!string.IsNullOrWhiteSpace(_currentobj.Code) && ConvertDef.ToInt32(_currentobj.Version, 0) != 0);
        }


        // ************************************************************************************************
        //
        // CLOSE
        //
        // ************************************************************************************************

        protected override void CloseAndReturnToList()
        {
            if (this.IsDirty)
            {
                DialogResult? dr = base.IsObjectDirtyDialogResult();
                if (dr != DialogResult.Cancel)
                {
                    UnlockCurrentObject();
                    base.ShowList();
                }
            }
            else
            {
                UnlockCurrentObject();
                ShowList();
            }
        }

        public void UnlockCurrentObject()
        {
            if (_currentobj != null && !string.IsNullOrWhiteSpace(_currentobj.Code))
            {
                if (objectlocking)
                {
                    ls.MarkObjectAsClosed(_currentobj.Code, LockableObjects.SearchList);
                    ls.UnlockSearchListObject(_currentobj.Code);
                }
            }
        }


        // ************************************************************************************************ 


        private void CodeWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentobj.ScriptName))
            {
                if (objectlocking)
                    ls.MarkObjectAsClosed(_currentobj.Script.Code, LockableObjects.Script);
                if (!IsDirty)
                    ManageVersioningButtons(true);
            }
        }


        private void ManageVersioningButtons(bool state)
        {
            this.tbCheckin.Enabled = state;
            this.tbCompare.Enabled = state;
        }


		private void ParentForm_Activated(object sender, EventArgs e)
		{
			if (_currentobj.EnquiryForm != "")
			{
				long _enqnewversion = Enquiry.GetEnquiryFormVersion(_currentobj.EnquiryForm);
				if (_enqnewversion > _enqversion)
				{
					if (MessageBox.ShowYesNoQuestion("ENQFCHANGED","Changes have been detected for the Enquiry Form. Do you wish to Update?") == DialogResult.Yes)
					{
						LoadSearchList(_currentobj.Code,false,false);
						_enqversion = _enqnewversion;
					}
				}
			}
		}

		private void lstList_SearchButtonCommands2(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
		{
			if (e.ButtonName == "cmdPDelete")
			{
				FWBS.OMS.UI.Windows.SearchButtonEventArgs n = new SearchButtonEventArgs("cmdDelete",e.Action);
				base.lstList_SearchButtonCommands(sender,n);
			}
		}

        private void dataGridEvents1_NewScript(object sender, EventArgs e)
        {
            _currentobj.ScriptName = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_currentobj.Code);
        }

        private void ucSearchControl1_CommandExecuting(object sender, CommandExecutingEventArgs e)
        {
            if (e.Action == SearchEngine.ButtonActions.SearchList)
            {
                e.Cancel = true;
                MessageBox.ShowInformation("SLISTSWITCH",
                    "You cannot switch SearchList whilst editing in the Admin Kit");
            }
        }

	}
}

