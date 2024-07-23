using System;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.Design.CodeBuilder;



namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucFundType.
    /// </summary>
    public class ucPrecedent : FWBS.OMS.UI.Windows.Admin.ucEditBase, IObjectUnlock, IOBjectDirty
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ToolBarButton tbScript;
		private System.Windows.Forms.ToolBarButton tbSp99;

        private System.Windows.Forms.ToolBarButton tbSpVersioning;

		private FWBS.OMS.UI.Windows.Design.Precedent _currentobj  = null;
		private System.Windows.Forms.DataGridTextBoxColumn dgcPrecDesc;
		private System.Windows.Forms.DataGridTextBoxColumn gdcPrecType;
		private System.Windows.Forms.DataGridTextBoxColumn dgcPrecCategoryDesc;
		private System.Windows.Forms.DataGridTextBoxColumn dgcPrecScript;
		private FWBS.OMS.UI.TabControl tpgEditors;
		private System.Windows.Forms.TabPage tpProperties;
		private System.Windows.Forms.TabPage tpEvents;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
        private FWBS.OMS.Design.CodeBuilder.DataGridEvents dataGridEvents1;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.Panel panel2;
		private FWBS.OMS.UI.Windows.ucPrecedent ucPrecedent1;
        private FWBS.OMS.UI.Windows.DocumentManagement.Addins.DocumentPreviewAddin documentPreviewer1;
		private System.Windows.Forms.ToolBarButton tbEditPrec;
		private System.Windows.Forms.ToolBarButton tpSp3;
		private System.Windows.Forms.ToolBarButton tbExport;
		private FWBS.Common.UI.Windows.eXPPanel pnlActions;
		private System.Windows.Forms.LinkLabel lnkSetPassword;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.ToolBarButton tbArchivePrec;
        private System.Windows.Forms.ToolBarButton tbRestorePrec;
        private CodeWindow _codewindow;
        private LockState ls = new LockState();
        private string precedentID;
        private IMainParent _mainparent;
        private bool objectlocking;
        private readonly string sPrecedent = "Precedent";


		public ucPrecedent()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public ucPrecedent(IMainParent mainparent, Control editparent) : base(mainparent,editparent)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			ucPrecedent1.ShowLibraryCategory = true;
			sPrecedent = Session.CurrentSession.Resources.GetResource("PRECEDENT", "Precedent", "").Text;
			base.SelectCode = "precid";
            _mainparent = mainparent;
            objectlocking = Session.CurrentSession.ObjectLocking;
            currenttype = CodeLookup.GetLookup("ADMINMENU", "AMUSTDOCADMIN", "Standard Document Admin");
		}

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                ucPrecedent1.SetDefaults(null, "");

            base.OnParentChanged(e);
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
                if (_mainparent != null)
                {
                    CodeWindow.FormClosing -= new FormClosingEventHandler(CodeWindow_FormClosing);
                    CodeWindow.Close();
                    ls.UnlockPrecedentObject(precedentID);
                }
			}
			base.Dispose( disposing );
		}


		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tbScript = new System.Windows.Forms.ToolBarButton();
            this.tbSp99 = new System.Windows.Forms.ToolBarButton();
            this.dgcPrecDesc = new System.Windows.Forms.DataGridTextBoxColumn();
            this.gdcPrecType = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcPrecCategoryDesc = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcPrecScript = new System.Windows.Forms.DataGridTextBoxColumn();
            this.tpgEditors = new FWBS.OMS.UI.TabControl();
            this.tpProperties = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlActions = new FWBS.Common.UI.Windows.eXPPanel();
            this.lnkSetPassword = new System.Windows.Forms.LinkLabel();
            this.tpEvents = new System.Windows.Forms.TabPage();
            this.dataGridEvents1 = new FWBS.OMS.Design.CodeBuilder.DataGridEvents();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.documentPreviewer1 = new FWBS.OMS.UI.Windows.DocumentManagement.Addins.DocumentPreviewAddin();
            this.ucPrecedent1 = new FWBS.OMS.UI.Windows.ucPrecedent();
            this.tbEditPrec = new System.Windows.Forms.ToolBarButton();
            this.tpSp3 = new System.Windows.Forms.ToolBarButton();
            this.tbExport = new System.Windows.Forms.ToolBarButton();
            this.tbArchivePrec = new System.Windows.Forms.ToolBarButton();
            this.tbRestorePrec = new System.Windows.Forms.ToolBarButton();
            this.tbSpVersioning = new System.Windows.Forms.ToolBarButton();

            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstList)).BeginInit();
            this.pnlEdit.SuspendLayout();
            this.pnlQuickSearch.SuspendLayout();
            this.pnlListbarContainer.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.tpgEditors.SuspendLayout();
            this.tpProperties.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.tpEvents.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpList
            // 
            this.tpList.Controls.Add(this.ucPrecedent1);
            this.tpList.Controls.SetChildIndex(this.pnlListbarContainer, 0);
            this.tpList.Controls.SetChildIndex(this.lstList, 0);
            this.tpList.Controls.SetChildIndex(this.pnlQuickSearch, 0);
            this.tpList.Controls.SetChildIndex(this.ucPrecedent1, 0);
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.panel2);
            this.tpEdit.Controls.Add(this.splitter1);
            this.tpEdit.Controls.Add(this.tpgEditors);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.tpgEditors, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
            this.tpEdit.Controls.SetChildIndex(this.panel2, 0);
            // 
            // tbNew
            // 
            this.BresourceLookup1.SetLookup(this.tbNew, new FWBS.OMS.UI.Windows.ResourceLookupItem("New", "New", ""));
            this.tbNew.Visible = false;
            // 
            // tbcLists
            // 
            this.tbcLists.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbArchivePrec,
            this.tbRestorePrec});
            this.tbcLists.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbcLists_ButtonClick);
            this.tbcLists.VisibleChanged += new System.EventHandler(this.tbcLists_VisibleChanged);
            // 
            // tbEdit
            // 
            this.BresourceLookup1.SetLookup(this.tbEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            // 
            // tbDelete
            // 
            this.tbDelete.Enabled = false;
            this.BresourceLookup1.SetLookup(this.tbDelete, new FWBS.OMS.UI.Windows.ResourceLookupItem("Delete", "Delete", ""));
            this.tbDelete.Visible = false;
            // 
            // tbShowTrash
            // 
            this.BresourceLookup1.SetLookup(this.tbShowTrash, new FWBS.OMS.UI.Windows.ResourceLookupItem("ShowTrash", "Show Trash", ""));
            // 
            // tbShowActive
            // 
            this.BresourceLookup1.SetLookup(this.tbShowActive, new FWBS.OMS.UI.Windows.ResourceLookupItem("ShowActive", "Show Active", ""));
            // 
            // tbRestore
            // 
            tbRestorePrec.Enabled = false;
            this.BresourceLookup1.SetLookup(this.tbRestorePrec, new FWBS.OMS.UI.Windows.ResourceLookupItem("Restore", "Restore", ""));
            // 
            // lstStyle
            // 
            this.lstStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
                                                                                                       this.dgcPrecDesc,
                                                                                                       this.gdcPrecType,
                                                                                                       this.dgcPrecCategoryDesc,
                                                                                                       this.dgcPrecScript});
            this.lstStyle.MappingName = "PRECEDENTS";
            // 
            // dgcCode
            // 
            this.dgcCode.HeaderText = "ID";
            this.dgcCode.ImageIndex = 0;
            this.BresourceLookup1.SetLookup(this.dgcCode, new FWBS.OMS.UI.Windows.ResourceLookupItem("PrecID", "ID", ""));
            this.dgcCode.MappingName = "PrecID";
            this.dgcCode.Resources = FWBS.OMS.UI.Windows.omsImageLists.AdminMenu16;
            // 
            // dgcDesc
            // 
            this.dgcDesc.HeaderText = "ID";
            this.BresourceLookup1.SetLookup(this.dgcDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("PrecTitle", "Title", ""));
            this.dgcDesc.MappingName = "PrecTitle";
            this.dgcDesc.Width = 75;
            //
            // lstList
            // 
            this.lstList.Location = new System.Drawing.Point(0, 26);
            this.lstList.Size = new System.Drawing.Size(549, 357);
            this.lstList.Visible = false;
            // 
            // tbcEdit
            // 
            this.tbcEdit.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbSp99,
            this.tbScript,
            this.tbEditPrec,
            this.tbSpVersioning});
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
            // tbClone
            // 
            this.tbClone.Visible = false;
            // 
            // pnlQuickSearch
            // 
            this.pnlQuickSearch.Visible = false;
            // 
            // tbScript
            // 
            this.tbScript.ImageIndex = 25;
            this.BresourceLookup1.SetLookup(this.tbScript, new FWBS.OMS.UI.Windows.ResourceLookupItem("EditScript", "Edit Script", ""));
            this.tbScript.Name = "tbScript";
            // 
            // tbSp99
            // 
            this.tbSp99.Name = "tbSp99";
            this.tbSp99.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // dgcPrecDesc
            // 
            this.dgcPrecDesc.Format = "";
            this.dgcPrecDesc.FormatInfo = null;
            this.dgcPrecDesc.HeaderText = "Description";
            this.BresourceLookup1.SetLookup(this.dgcPrecDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("Description", "Description", ""));
            this.dgcPrecDesc.MappingName = "PRecDesc";
            this.dgcPrecDesc.ReadOnly = true;
            this.dgcPrecDesc.Width = 250;
            // 
            // gdcPrecType
            // 
            this.gdcPrecType.Format = "";
            this.gdcPrecType.FormatInfo = null;
            this.gdcPrecType.HeaderText = "Type";
            this.BresourceLookup1.SetLookup(this.gdcPrecType, new FWBS.OMS.UI.Windows.ResourceLookupItem("Type", "Type", ""));
            this.gdcPrecType.MappingName = "PrecType";
            this.gdcPrecType.ReadOnly = true;
            this.gdcPrecType.Width = 75;
            // 
            // dgcPrecCategoryDesc
            // 
            this.dgcPrecCategoryDesc.Format = "";
            this.dgcPrecCategoryDesc.FormatInfo = null;
            this.dgcPrecCategoryDesc.HeaderText = "Category Description";
            this.BresourceLookup1.SetLookup(this.dgcPrecCategoryDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("CateDesc", "Category Description", ""));
            this.dgcPrecCategoryDesc.MappingName = "PrecCategoryDesc";
            this.dgcPrecCategoryDesc.ReadOnly = true;
            this.dgcPrecCategoryDesc.Width = 150;
            // 
            // dgcPrecScript
            // 
            this.dgcPrecScript.Format = "";
            this.dgcPrecScript.FormatInfo = null;
            this.dgcPrecScript.HeaderText = "Script";
            this.BresourceLookup1.SetLookup(this.dgcPrecScript, new FWBS.OMS.UI.Windows.ResourceLookupItem("Script", "Script", ""));
            this.dgcPrecScript.MappingName = "PrecScript";
            this.dgcPrecScript.ReadOnly = true;
            this.dgcPrecScript.Width = 75;
            // 
            // tpgEditors
            // 
            this.tpgEditors.Controls.Add(this.tpProperties);
            this.tpgEditors.Controls.Add(this.tpEvents);
            this.tpgEditors.Dock = System.Windows.Forms.DockStyle.Right;
            this.tpgEditors.Location = new System.Drawing.Point(277, 50);
            this.tpgEditors.Name = "tpgEditors";
            this.tpgEditors.SelectedIndex = 0;
            this.tpgEditors.Size = new System.Drawing.Size(272, 333);
            this.tpgEditors.TabIndex = 3;
            // 
            // tpProperties
            // 
            this.tpProperties.Controls.Add(this.propertyGrid1);
            this.tpProperties.Controls.Add(this.splitter2);
            this.tpProperties.Controls.Add(this.pnlActions);
            this.tpProperties.Location = new System.Drawing.Point(4, 24);
            this.BresourceLookup1.SetLookup(this.tpProperties, new FWBS.OMS.UI.Windows.ResourceLookupItem("PROPERTIES", "Properties", ""));
            this.tpProperties.Name = "tpProperties";
            this.tpProperties.Size = new System.Drawing.Size(264, 305);
            this.tpProperties.TabIndex = 0;
            this.tpProperties.Text = "Properties";
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(264, 267);
            this.propertyGrid1.TabIndex = 3;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            // 
            // splitter2
            // 
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 267);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(264, 3);
            this.splitter2.TabIndex = 204;
            this.splitter2.TabStop = false;
            // 
            // pnlActions
            // 
            this.pnlActions.Backcolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.Control);
            this.pnlActions.BorderLine = true;
            this.pnlActions.Controls.Add(this.lnkSetPassword);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlActions.Forecolor = new FWBS.Common.UI.Windows.ExtColor(System.Drawing.SystemColors.ControlDark);
            this.pnlActions.Location = new System.Drawing.Point(0, 270);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Padding = new System.Windows.Forms.Padding(3);
            this.pnlActions.Size = new System.Drawing.Size(264, 35);
            this.pnlActions.TabIndex = 203;
            // 
            // lnkSetPassword
            // 
            this.lnkSetPassword.Dock = System.Windows.Forms.DockStyle.Top;
            this.lnkSetPassword.Location = new System.Drawing.Point(3, 3);
            this.BresourceLookup1.SetLookup(this.lnkSetPassword, new FWBS.OMS.UI.Windows.ResourceLookupItem("SetPassword", "Set Password", ""));
            this.lnkSetPassword.Name = "lnkSetPassword";
            this.lnkSetPassword.Size = new System.Drawing.Size(258, 14);
            this.lnkSetPassword.TabIndex = 2;
            this.lnkSetPassword.TabStop = true;
            this.lnkSetPassword.Text = "Set Password";
            this.lnkSetPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSetPassword_LinkClicked);
            // 
            // tpEvents
            // 
            this.tpEvents.Controls.Add(this.dataGridEvents1);
            this.tpEvents.Location = new System.Drawing.Point(4, 24);
            this.BresourceLookup1.SetLookup(this.tpEvents, new FWBS.OMS.UI.Windows.ResourceLookupItem("EVENTS", "Events", ""));
            this.tpEvents.Name = "tpEvents";
            this.tpEvents.Size = new System.Drawing.Size(264, 305);
            this.tpEvents.TabIndex = 1;
            this.tpEvents.Text = "Events";
            // 
            // dataGridEvents1
            // 
            this.dataGridEvents1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.dataGridEvents1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridEvents1.Location = new System.Drawing.Point(0, 0);
            this.dataGridEvents1.Name = "dataGridEvents1";
            this.dataGridEvents1.Padding = new System.Windows.Forms.Padding(1);
            this.dataGridEvents1.Size = new System.Drawing.Size(264, 305);
            this.dataGridEvents1.TabIndex = 0;
            this.dataGridEvents1.CodeButtonClick += new System.EventHandler(this.dataGridEvents1_CodeButtonClick);
            this.dataGridEvents1.NewScript += new System.EventHandler(this.dataGridEvents1_NewScript);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(274, 50);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 333);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.documentPreviewer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 50);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(274, 333);
            this.panel2.TabIndex = 5;
            // 
            // documentPreviewer1
            // 
            this.documentPreviewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentPreviewer1.Location = new System.Drawing.Point(0, 0);
            this.documentPreviewer1.Name = "documentPreviewer1";
            this.documentPreviewer1.Padding = new System.Windows.Forms.Padding(5);
            this.documentPreviewer1.Size = new System.Drawing.Size(274, 333);
            this.documentPreviewer1.TabIndex = 0;
            // 
            // ucPrecedent1
            // 
            this.ucPrecedent1.AdditionalInfoVisible = false;
            this.ucPrecedent1.AddJobVisible = false;
            this.ucPrecedent1.ButtonCancelVisible = false;
            this.ucPrecedent1.ButtonContinueVisible = false;
            this.ucPrecedent1.ButtonEditVisible = false;
            this.ucPrecedent1.ButtonPrintVisible = false;
            this.ucPrecedent1.ButtonViewVisible = false;
            this.ucPrecedent1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPrecedent1.JobListVisible = false;
            this.ucPrecedent1.Location = new System.Drawing.Point(0, 52);
            this.ucPrecedent1.Name = "ucPrecedent1";
            this.ucPrecedent1.Padding = new System.Windows.Forms.Padding(3);
            this.ucPrecedent1.ShowLibraryCategory = false;
            this.ucPrecedent1.Size = new System.Drawing.Size(549, 331);
            this.ucPrecedent1.TabIndex = 1;
            this.ucPrecedent1.SelectedItemDoubleClick += new System.EventHandler(this.ucPrecedent1_SelectedItemDoubleClick);
            this.ucPrecedent1.SearchCompleted += UcPrecedent1_SearchCompleted;
            // 
            // tbEditPrec
            // 
            this.tbEditPrec.ImageIndex = 58;
            this.BresourceLookup1.SetLookup(this.tbEditPrec, new FWBS.OMS.UI.Windows.ResourceLookupItem("EditPrec", "Edit Precedent", ""));
            this.tbEditPrec.Name = "tbEditPrec";
            // 
            // tpSp3
            // 
            this.tpSp3.Name = "tpSp3";
            this.tpSp3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbExport
            // 
            this.tbExport.Enabled = false;
            this.tbExport.ImageIndex = 33;
            this.BresourceLookup1.SetLookup(this.tbExport, new FWBS.OMS.UI.Windows.ResourceLookupItem("ExportGrp", "Export Group", ""));
            this.tbExport.Name = "tbExport";
            // 
            // tbArchive
            // 
            this.tbArchivePrec.ImageIndex = 6;
            this.BresourceLookup1.SetLookup(this.tbArchivePrec, new FWBS.OMS.UI.Windows.ResourceLookupItem("Archive", "Archive", ""));
            this.tbArchivePrec.Name = "tbArchive";
            // 
            // tbSpVersioning
            // 
            this.tbSpVersioning.Name = "tbSpVersioning";
            this.tbSpVersioning.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // ucPrecedent
            // 
            this.BresourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("", "", ""));
            this.Name = "ucPrecedent";
            this.Load += new System.EventHandler(this.ucPrecedent_Load);
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstList)).EndInit();
            this.pnlEdit.ResumeLayout(false);
            this.pnlQuickSearch.ResumeLayout(false);
            this.pnlQuickSearch.PerformLayout();
            this.pnlListbarContainer.ResumeLayout(false);
            this.pnlListbarContainer.PerformLayout();
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.tpgEditors.ResumeLayout(false);
            this.tpProperties.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.tpEvents.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        private void UcPrecedent1_SearchCompleted(object sender, EventArgs e)
        {
            //deal with the selected library
            tbArchivePrec.Enabled = !(ucPrecedent1.SelectedLibrary == "ARCHIVE");
            tbRestorePrec.Enabled = (ucPrecedent1.SelectedLibrary == "ARCHIVE");
            //deal with the number of visible rows in the list
            tbEdit.Enabled = (ucPrecedent1.VisibleRows > 0);
            if(ucPrecedent1.SelectedLibrary == "ARCHIVE")
                tbRestorePrec.Enabled = (ucPrecedent1.VisibleRows > 0);
            if(ucPrecedent1.SelectedLibrary != "ARCHIVE")
                tbArchivePrec.Enabled = (ucPrecedent1.VisibleRows > 0);
        }

        #endregion

        protected override void LoadSingleItem(string Code)
		{
			try
			{
                if (Session.CurrentSession.ObjectLocking)
                {
                    if (!ls.CheckObjectLockState(Convert.ToString(ucPrecedent1.SelectedItem["PrecID"].Value), LockableObjects.Precedent))
                    {
                        SetupPrecedent();
                        ls.LockPrecedentObject(Convert.ToString(ucPrecedent1.SelectedItem["PrecID"].Value));
                        precedentID = Convert.ToString(ucPrecedent1.SelectedItem["PrecID"].Value);
                        ls.MarkObjectAsOpen(precedentID, LockableObjects.Precedent);
                    }
                }
                else
                    SetupPrecedent();
    		}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				IsDirty=false;
			}
		}

        private void SetupPrecedent()
        {
            lasttype = currenttype;
            _currentobj = FWBS.OMS.UI.Windows.Design.Precedent.GetPrecedentInDesignMode(Convert.ToInt64(ucPrecedent1.SelectedItem["PrecID"].Value));
            currenttype = _currentobj.Title + " ( ID: " + _currentobj.ID.ToString() + " )";
            propertyGrid1.SelectedObject = _currentobj;
            labSelectedObject.Text = string.Format("{0} - [{1}] - {2}", sPrecedent, _currentobj.ID, _currentobj.Description);
            _currentobj.ScriptChanged += new EventHandler(_currentobj_ScriptChange);
            _currentobj.Changed += new EventHandler(_currentobj_Change);
            propertyGrid1.HelpVisible = true;
            CodeWindow.Text = Session.CurrentSession.Resources.GetResource("PRECCODEBLD", "Precedent Code Builder", "").Text;
            CodeWindow.Load(_currentobj.Script.ScriptType, _currentobj.Script);
            dataGridEvents1.CurrentCodeSurface = CodeWindow;
            dataGridEvents1.SelectedObject = _currentobj.Script.ScriptType;
            documentPreviewer1.Connect(_currentobj);
            documentPreviewer1.RefreshItem();
            ShowEditor();
        }


		protected override DataTable GetData()
		{
			return null;
		}


		public CodeWindow CodeWindow
		{
			get
			{
				if (_codewindow == null)
				{
				   _codewindow = new CodeWindow();
                   _codewindow.Init(null);
				}
				return _codewindow;
			}
		}

		protected override bool UpdateData()
		{
			try
			{
                tpgEditors.SelectedIndex = 0;
                if (CodeWindow.IsDirty)
                {
                    if (CodeWindow.SaveAndCompile() == false)
                    {
                        CodeWindow.Show();
                        CodeWindow.BringToFront();
                        return false;
                    }
                }
				_currentobj.Update();
				IsDirty=false;

				return true;
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
				return false;
			}
		}

		protected override void NewData()
		{
			try
			{
				_currentobj = new Design.Precedent();
				propertyGrid1.SelectedObject = _currentobj;
				labSelectedObject.Text = string.Format("{0} - [{1}] - {2}", sPrecedent, _currentobj.ID, _currentobj.Description);
				_currentobj.ScriptChanged +=new EventHandler(_currentobj_ScriptChange);
				_currentobj.Changed += new EventHandler(_currentobj_Change);
				propertyGrid1.HelpVisible=true;
				CodeWindow.Text = Session.CurrentSession.Resources.GetResource("PRECCODEBLD", "Precedent Code Builder", "").Text;
				CodeWindow.Load(_currentobj.Script.ScriptType,_currentobj.Script);
                dataGridEvents1.CurrentCodeSurface = CodeWindow;
                dataGridEvents1.SelectedObject = _currentobj.Script.ScriptType;
                documentPreviewer1.Connect(_currentobj);
                documentPreviewer1.RefreshItem();
				ShowEditor();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
			}
		}

		protected override void DeleteData(string Code)
		{
			MessageBox.Show("Not Implemented Yet","OMS Admin",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
		}
		
		private new void tbcEdit_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == tbReturn)
			{
				CodeWindow.Hide();
			}
			else if (e.Button == tbScript)
			{
				CodeWindow.Show();
				CodeWindow.BringToFront();
				if (_currentobj.ScriptName == "")
				{
					_currentobj.ScriptName = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_currentobj.ID.ToString());
				}

				if (CodeWindow.HasMethod(dataGridEvents1.SelectedMethodName))
                {
                    CodeWindow.GotoMethod(dataGridEvents1.SelectedMethodName);
                }
                else
                {
                    CodeWindow.GenerateHandler(dataGridEvents1.SelectedMethodName, new GenerateHandlerInfo() { DelegateType = dataGridEvents1.SelectedMethodData.Delegate });
                }
                dataGridEvents1.CurrentCodeSurface = CodeWindow;
                dataGridEvents1.SelectedObject = _currentobj.Script.ScriptType;
                ls.MarkObjectAsOpen(_currentobj.Script.Code, LockableObjects.Script);

                CodeWindow.FormClosing += new FormClosingEventHandler(CodeWindow_FormClosing);
            }
			else if (e.Button == tbEditPrec)
			{
				try
				{
                    FWBS.OMS.UI.Windows.Services.OpenPrecedent(_currentobj, DocOpenMode.Edit, !(Session.CurrentSession.EnablePrecedentVersioning), true);
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
			}

		}

        private void CodeWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            ls.MarkObjectAsClosed(_currentobj.Script.Code, LockableObjects.Script);
        }

		private void _currentobj_ScriptChange(object sender, EventArgs e)
		{
            if (_currentobj.Script.State != ObjectState.Added)
                _currentobj.NewScript();
            CodeWindow.Load(_currentobj.Script.ScriptType, _currentobj.Script);
            dataGridEvents1.CurrentCodeSurface = CodeWindow;
            dataGridEvents1.SelectedObject = _currentobj.Script.ScriptType;
        }

		private void _currentobj_Change(object sender, EventArgs e)
		{
			IsDirty=true;
		}

        private void dataGridEvents1_NewScript(object sender, EventArgs e)
        {
            _currentobj.ScriptName = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_currentobj.ID.ToString());
        }
        
        private void dataGridEvents1_CodeButtonClick(object sender, System.EventArgs e)
		{
			if (_currentobj.ScriptName == "")
			{
			    _currentobj.ScriptName = FWBS.OMS.Script.ScriptGen.GenerateUniqueName(_currentobj.ID.ToString());
			}
			CodeWindow.Show();
			CodeWindow.BringToFront();
            if (CodeWindow.HasMethod(dataGridEvents1.SelectedMethodName))
                CodeWindow.GotoMethod(dataGridEvents1.SelectedMethodName);
            else
            {
                CodeWindow.GenerateHandler(dataGridEvents1.SelectedMethodName, new GenerateHandlerInfo() { DelegateType = dataGridEvents1.SelectedMethodData.Delegate });
            }
		}


		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			labSelectedObject.Text = "Precedent - [" + _currentobj.ID.ToString() + "] - " + _currentobj.Description;
			IsDirty=true;

		}


		private void ucPrecedent1_SelectedItemDoubleClick(object sender, System.EventArgs e)
		{
			LoadSingleItem("");
		}


        private new void tbcLists_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
            if (e.Button == tbEdit)
            {
                LoadSingleItem("");
            }
            else if (e.Button == tbArchivePrec)
            {
                ArchivePrecedent();
            }
            else if (e.Button == tbRestorePrec)
            {
                RestorePrecedent();
            }
        }


        private void ArchivePrecedent()
        {
            try
            {
                long precedentID = Convert.ToInt64(ucPrecedent1.SelectedItem["PrecID"].Value);
                if (precedentID != 0)
                {
                    if (!CheckLockStateOfObject(Convert.ToString(ucPrecedent1.SelectedItem["PrecID"].Value)))
                    {
                        if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("RUSARCHIVE", "Are you sure you wish to Archive [%1%]?", Convert.ToString(precedentID)) == DialogResult.Yes)
                        {
                            if (FWBS.OMS.Precedent.IsPartOfMultiPrecedent(precedentID))
                            {
                                if (System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource("MULTIPRESERR", "The Precedent you have selected for archiving is part of a multi-Precedent. Archiving it will remove it from multi-Precedent configuration. Do you wish to continue?", "").Text, 
                                        Session.CurrentSession.Resources.GetResource("PRESARCH", "Precedent Archiving", "").Text,
                                        System.Windows.Forms.MessageBoxButtons.YesNo,
                                    System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                                {
                                    return;
                                }
                            }
                            if (!FWBS.OMS.Precedent.ArchivePrecedent(precedentID))
                            {
                                FWBS.OMS.UI.Windows.MessageBox.ShowInformation(this, "PARCHIVEFAILED", "The Precedent you selected was not archived.", null);
                            }
                        }
                        ucPrecedent1.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }


        private void RestorePrecedent()
        {
            long precID = Convert.ToInt64(ucPrecedent1.SelectedItem["PrecID"].Value);
            if (precID != 0)
            {
                if (!CheckLockStateOfObject(Convert.ToString(precID)))
                {
                    if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("RUSRESTORE", "Are you sure you wish to restore [%1%]?", Convert.ToString(precID)) == DialogResult.Yes)
                    {
                        if (!FWBS.OMS.Precedent.RestorePrecedent(precID))
                            FWBS.OMS.UI.Windows.MessageBox.ShowInformation(this, "PRESTOREFAILED", "The Precedent you selected was not restored.", null);
                    }
                }
            }
            ucPrecedent1.Refresh();
        }


        private bool CheckLockStateOfObject(string selectedPrec)
        {
            bool result = false;
            if (FWBS.OMS.Session.CurrentSession.ObjectLocking)
            {
                LockState ls = new LockState();
                if (ls.CheckObjectLockState(selectedPrec, LockableObjects.Precedent))
                    return true;
            }
            return result;
        }


		private void ucPrecedent_Load(object sender, System.EventArgs e)
		{

		}


		private void tbcLists_VisibleChanged(object sender, System.EventArgs e)
		{
			if (tbcLists.Visible)
				ucPrecedent1.Focus();
		}


		private void lnkSetPassword_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			FWBS.OMS.UI.Windows.Services.ChangePassword(_currentobj);
		}

        public void UnlockCurrentObject()
        {
            if (objectlocking && !string.IsNullOrWhiteSpace(precedentID))
            {
                ls.UnlockPrecedentObject(precedentID);
                ls.MarkObjectAsClosed(precedentID, LockableObjects.Precedent);
                //check if there is a script window open for this precedent and close it
                if (CodeWindow != null)
                {
                    ls.MarkObjectAsClosed(_currentobj.Script.Code, LockableObjects.Script);
                    CodeWindow.Hide();
                }
            }
        }

        private bool IsObjectDirty()
        {
            return base.IsDirty;
        }

        protected override void CloseAndReturnToList()
        {
            if (base.IsDirty || (CodeWindow.IsDirty))
            {
                DialogResult? dr = base.IsObjectDirtyDialogResult();
                if (dr != DialogResult.Cancel)
                {
                    UnlockCurrentObject();
                    currenttype = lasttype;
                    base.ShowList();
                }
            }
            else
            {
                UnlockCurrentObject();
                currenttype = lasttype;
                base.ShowList();
            }
        }

    }
}
