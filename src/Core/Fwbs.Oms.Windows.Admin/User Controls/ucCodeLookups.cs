using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.Data;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucCodeLookups.
    /// </summary>
    public class ucCodeLookups : ucEditBase
    {
        #region Fields
        private DataTable dtCL = null;
        private DataTable dtCC = null;
        private ArrayList CodeTypesHistory = new ArrayList();
        private object _currentaddlink = DBNull.Value;
        private CodeLookup _code;
        private IContainer components;
        private System.Windows.Forms.DataGridTableStyle gdsCodeLookups;

        /// <summary>
        /// Data Styles
        /// </summary>
        private FWBS.OMS.UI.Windows.DataGridImageColumn dgcCode2;

        private System.Windows.Forms.DataGridTextBoxColumn gdcSelect;
        private System.Windows.Forms.DataGridTextBoxColumn dgcDescription;
        private System.Windows.Forms.DataGridTextBoxColumn dgcSelect;
        private System.Windows.Forms.DataGridTextBoxColumn dgcAddLink;

        private System.Windows.Forms.ToolBarButton tbCEReturn;
        private System.Windows.Forms.Panel pnlCultures;
        private System.Windows.Forms.ComboBox cmbCulture;
        private System.Windows.Forms.Label labUpdateCulture;
        private System.Windows.Forms.ComboBox cmbDisplay;
        private System.Windows.Forms.Label labDisplayCulture;
        private System.Windows.Forms.ToolBarButton tb_CL_Open;
        private System.Windows.Forms.ToolBarButton tb_CL_Find;
        private System.Windows.Forms.ToolBarButton tb_CL_EditAll;
        private System.Windows.Forms.ToolBarButton tb_CL_EditOne;
        private System.Windows.Forms.ToolBarButton tb_CL_CDParent;
        private System.Windows.Forms.ToolStripSeparator parentSeparator;
        private System.Windows.Forms.DataGridBoolColumn dgcSystem;
        private FWBS.Common.UI.Windows.DataGridLabelColumn dgcType;
        private System.Windows.Forms.DataGridTextBoxColumn dgcType2;
        private DataGridView lstCompare;    // EditAll view
        private DataGridView lstCodeLookup; // EditOne view
        private DataGridView lstNewCodes;   // New view
        private DataGridViewTextBoxColumn cpCode;
        private DataGridViewTextBoxColumn S_Desc;
        private DataGridViewTextBoxColumn S_Help;
        private DataGridViewTextBoxWithButtonColumn D_Desc;
        private DataGridViewTextBoxWithButtonColumn D_Help;
        private DataGridViewTextBoxColumn AddLink;
        private DataGridViewTextBoxColumn S_Cult;
        private DataGridViewTextBoxColumn D_Cult;
        private DataGridViewTextBoxColumn cdGroupCP;
        private DataGridViewTextBoxColumn cdAddLinkCP;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private DataGridViewTextBoxWithButtonColumn dataGridViewTextBoxColumn3;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxWithButtonColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxWithButtonColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn cdType;
        private DataGridViewTextBoxColumn cdCode;
        private DataGridViewComboBoxColumn cdUICultureInfo;
        private DataGridViewTextBoxWithButtonColumn cdDesc;
        private DataGridViewCheckBoxColumn cdSystem;
        private DataGridViewCheckBoxColumn cdGroup;
        private DataGridViewTextBoxColumn cdAddLink;
        private DataGridViewTextBoxWithButtonColumn cdHelp;
        private DataGridViewTextBoxColumn cdID;
        private DataGridViewTextBoxColumn rowGuid;
        private DataGridViewTextBoxColumn cdDeletable;
        private DataGridViewTextBoxWithButtonColumn cdNotes;
        private System.Windows.Forms.ToolBarButton tb_CL_Delete;
        private CodeLookupEditingState EditingState;
        private string editingcode;
        #endregion

        private enum CodeLookupEditingState { List, New, Edit, Compare }

        #region Constructors
        public ucCodeLookups()
        {
            InitializeComponent();
            SetHeaderText();

            lstCodeLookup.AutoGenerateColumns = false;
            lstCompare.AutoGenerateColumns = false;
        }

        public ucCodeLookups(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params)
            : base(mainparent, editparent)
        {
            InitializeComponent();
            SetHeaderText();

            if (Session.CurrentSession.IsLoggedIn)
            {
                SelectCode = "cdcode";
                cmbCulture.DataSource = CodeLookup.GetCultures(false, true);
                cmbCulture.ValueMember = "langcode";
                cmbCulture.DisplayMember = "langdesc";

                cmbDisplay.DataSource = CodeLookup.GetCultures(false, true);
                cmbDisplay.ValueMember = "langcode";
                cmbDisplay.DisplayMember = "langdesc";
                if (Params.Count > 0 && Convert.ToString(Params[0].Value) != "")
                {
                    CodeTypesHistory.Add(Params[0].Value);
                    currenttype = Convert.ToString(Params[0].Value);
                    RefreshCodeLookupList(currenttype);
                }
                else
                    CodeTypesHistory.Add("OMS");
                this.lstList.CaptionText = ResourceLookup.GetLookupText("CodeLookup", "Code Lookup", "", false) + " / ";
                if (CodeTypesHistory.Count > 0)
                    for (int t = 0; t < CodeTypesHistory.Count; t++)
                        this.lstList.CaptionText = this.lstList.CaptionText + CodeTypesHistory[t].ToString() + " / ";

                cdUICultureInfo.DataSource = CodeLookup.GetCultures(false, true);
                cdUICultureInfo.DisplayMember = "langdesc";
                cdUICultureInfo.ValueMember = "langcode";
                this.tb_CL_CDParent.Enabled = false;
                this.tb_CL_EditAll.Enabled = false;
            }
        }

        private void SetHeaderText()
        {
            this.cdType.HeaderText = this.dataGridViewTextBoxColumn1.HeaderText = ResourceLookup.GetLookupText("Type", "Type", "");
            this.cdCode.HeaderText = this.cpCode.HeaderText = this.dataGridViewTextBoxColumn2.HeaderText = ResourceLookup.GetLookupText("Code", "Code", "");
            this.cdUICultureInfo.HeaderText = this.dataGridViewComboBoxColumn1.HeaderText = ResourceLookup.GetLookupText("Culture", "Culture", "");
            this.cdDesc.HeaderText = this.dataGridViewTextBoxColumn3.HeaderText = ResourceLookup.GetLookupText("Description", "Description", "");
            this.cdSystem.HeaderText = this.dataGridViewCheckBoxColumn1.HeaderText = ResourceLookup.GetLookupText("System", "System", "");
            this.cdGroup.HeaderText = this.dataGridViewCheckBoxColumn2.HeaderText = ResourceLookup.GetLookupText("Group", "Group", "");
            this.cdAddLink.HeaderText = this.dataGridViewTextBoxColumn4.HeaderText = ResourceLookup.GetLookupText("AddLink", "Add Link", "");
            this.cdHelp.HeaderText = this.dataGridViewTextBoxColumn5.HeaderText = ResourceLookup.GetLookupText("Help", "Help", "");
            this.cdNotes.HeaderText = this.dataGridViewTextBoxColumn9.HeaderText = ResourceLookup.GetLookupText("Notes", "Notes", "");
            this.S_Desc.HeaderText = ResourceLookup.GetLookupText("SRCDESCRIPTION", "Source Description", "");
            this.S_Help.HeaderText = ResourceLookup.GetLookupText("SRCHELP", "Source Help", "");
            this.D_Desc.HeaderText = ResourceLookup.GetLookupText("DSTDESCRIPTION", "Destination Description", "");
            this.D_Help.HeaderText = ResourceLookup.GetLookupText("DSTHELP", "Destination Help", "");
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gdsCodeLookups = new System.Windows.Forms.DataGridTableStyle();
            this.dgcSelect = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcType = new FWBS.Common.UI.Windows.DataGridLabelColumn();
            this.dgcCode2 = new FWBS.OMS.UI.Windows.DataGridImageColumn();
            this.dgcDescription = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcAddLink = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcSystem = new System.Windows.Forms.DataGridBoolColumn();
            this.gdcSelect = new System.Windows.Forms.DataGridTextBoxColumn();
            this.tbCEReturn = new System.Windows.Forms.ToolBarButton();
            this.pnlCultures = new System.Windows.Forms.Panel();
            this.cmbCulture = new System.Windows.Forms.ComboBox();
            this.labUpdateCulture = new System.Windows.Forms.Label();
            this.cmbDisplay = new System.Windows.Forms.ComboBox();
            this.labDisplayCulture = new System.Windows.Forms.Label();
            this.tb_CL_EditAll = new System.Windows.Forms.ToolBarButton();
            this.tb_CL_EditOne = new System.Windows.Forms.ToolBarButton();
            this.tb_CL_Open = new System.Windows.Forms.ToolBarButton();
            this.tb_CL_Delete = new System.Windows.Forms.ToolBarButton();
            this.tb_CL_Find = new System.Windows.Forms.ToolBarButton();
            this.tb_CL_CDParent = new System.Windows.Forms.ToolBarButton();
            this.parentSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.dgcType2 = new System.Windows.Forms.DataGridTextBoxColumn();
            this.lstCompare = new System.Windows.Forms.DataGridView();
            this.cpCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_Help = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.D_Desc = new FWBS.OMS.UI.Windows.Admin.DataGridViewTextBoxWithButtonColumn();
            this.D_Help = new FWBS.OMS.UI.Windows.Admin.DataGridViewTextBoxWithButtonColumn();
            this.AddLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.S_Cult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.D_Cult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cdGroupCP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cdAddLinkCP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lstCodeLookup = new System.Windows.Forms.DataGridView();
            this.cdType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cdCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cdUICultureInfo = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.cdDesc = new FWBS.OMS.UI.Windows.Admin.DataGridViewTextBoxWithButtonColumn();
            this.cdSystem = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cdGroup = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cdAddLink = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cdHelp = new FWBS.OMS.UI.Windows.Admin.DataGridViewTextBoxWithButtonColumn();
            this.cdID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rowGuid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cdDeletable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cdNotes = new FWBS.OMS.UI.Windows.Admin.DataGridViewTextBoxWithButtonColumn();
            this.lstNewCodes = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxColumn3 = new FWBS.OMS.UI.Windows.Admin.DataGridViewTextBoxWithButtonColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new FWBS.OMS.UI.Windows.Admin.DataGridViewTextBoxWithButtonColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new FWBS.OMS.UI.Windows.Admin.DataGridViewTextBoxWithButtonColumn();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstList)).BeginInit();
            this.pnlEdit.SuspendLayout();
            this.pnlQuickSearch.SuspendLayout();
            this.pnlCultures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstCompare)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstCodeLookup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstNewCodes)).BeginInit();
            this.SuspendLayout();
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.lstNewCodes);
            this.tpEdit.Controls.Add(this.lstCodeLookup);
            this.tpEdit.Controls.Add(this.lstCompare);
            this.tpEdit.Controls.Add(this.pnlCultures);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlCultures, 0);
            this.tpEdit.Controls.SetChildIndex(this.lstCompare, 0);
            this.tpEdit.Controls.SetChildIndex(this.lstCodeLookup, 0);
            this.tpEdit.Controls.SetChildIndex(this.lstNewCodes, 0);
            // 
            // tbNew
            // 
            this.BresourceLookup1.SetLookup(this.tbNew, new FWBS.OMS.UI.Windows.ResourceLookupItem("New", "New", ""));
            // 
            // tbcLists
            // 
            this.tbcLists.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tb_CL_EditOne,
            this.tb_CL_EditAll,
            this.tb_CL_Open,
            this.tb_CL_Delete,
            this.tb_CL_Find,
            this.tb_CL_CDParent});
            this.tbcLists.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbcLists_ButtonClick);
            // 
            // tbEdit
            // 
            this.BresourceLookup1.SetLookup(this.tbEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tbEdit.Visible = false;
            // 
            // tbDelete
            // 
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
            this.BresourceLookup1.SetLookup(this.tbRestore, new FWBS.OMS.UI.Windows.ResourceLookupItem("Restore", "Restore", ""));
            // 
            // lstList
            // 
            this.lstList.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.gdsCodeLookups});
            this.lstList.DoubleClick += new System.EventHandler(this.lstList_DoubleClick);
            this.lstList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstList_KeyPress);
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
            this.tbReturn.ImageIndex = -1;
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            // 
            // txtSearch
            // 
            this.txtSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstList_KeyPress);
            // 
            // tbClone
            // 
            this.tbClone.Visible = false;
            // 
            // gdsCodeLookups
            // 
            this.gdsCodeLookups.DataGrid = this.lstList;
            this.gdsCodeLookups.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dgcSelect,
            this.dgcType,
            this.dgcCode2,
            this.dgcDescription,
            this.dgcAddLink,
            this.dgcSystem});
            this.gdsCodeLookups.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
            this.gdsCodeLookups.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.gdsCodeLookups.MappingName = "LOOKUPS";
            this.gdsCodeLookups.PreferredRowHeight = 20;
            this.gdsCodeLookups.ReadOnly = true;
            this.gdsCodeLookups.RowHeadersVisible = false;
            // 
            // dgcSelect
            // 
            this.dgcSelect.Format = "";
            this.dgcSelect.FormatInfo = null;
            this.dgcSelect.ReadOnly = true;
            this.dgcSelect.Width = 0;
            // 
            // dgcType
            // 
            this.dgcType.AllowMultiSelect = false;
            this.dgcType.DisplayDateAs = FWBS.OMS.SearchEngine.SearchColumnsDateIs.NotApplicable;
            this.dgcType.Format = "";
            this.dgcType.FormatInfo = null;
            this.dgcType.HeaderText = "Type";
            this.dgcType.ImageColumn = "";
            this.dgcType.ImageIndex = -1;
            this.dgcType.ImageList = null;
            this.BresourceLookup1.SetLookup(this.dgcType, new FWBS.OMS.UI.Windows.ResourceLookupItem("Type", "Type", ""));
            this.dgcType.MappingName = "cdType";
            this.dgcType.ReadOnly = true;
            this.dgcType.SearchList = null;
            this.dgcType.SourceDateIs = FWBS.OMS.SearchEngine.SearchColumnsDateIs.NotApplicable;
            this.dgcType.Width = 75;
            // 
            // dgcCode2
            // 
            this.dgcCode2.Format = "";
            this.dgcCode2.FormatInfo = null;
            this.dgcCode2.HeaderText = "Code";
            this.dgcCode2.ImageColumn = "cdIcon";
            this.dgcCode2.ImageIndex = -1;
            this.BresourceLookup1.SetLookup(this.dgcCode2, new FWBS.OMS.UI.Windows.ResourceLookupItem("Code", "Code", ""));
            this.dgcCode2.MappingName = "cdcode";
            this.dgcCode2.ReadOnly = true;
            this.dgcCode2.Resources = FWBS.OMS.UI.Windows.omsImageLists.CoolButtons16;
            this.dgcCode2.Width = 126;
            // 
            // dgcDescription
            // 
            this.dgcDescription.Format = "";
            this.dgcDescription.FormatInfo = null;
            this.dgcDescription.HeaderText = "Description";
            this.BresourceLookup1.SetLookup(this.dgcDescription, new FWBS.OMS.UI.Windows.ResourceLookupItem("Description", "Description", ""));
            this.dgcDescription.MappingName = "cddesc";
            this.dgcDescription.ReadOnly = true;
            this.dgcDescription.Width = 375;
            // 
            // dgcAddLink
            // 
            this.dgcAddLink.Format = "";
            this.dgcAddLink.FormatInfo = null;
            this.dgcAddLink.HeaderText = "Add Link";
            this.BresourceLookup1.SetLookup(this.dgcAddLink, new FWBS.OMS.UI.Windows.ResourceLookupItem("AddLink", "Add Link", ""));
            this.dgcAddLink.MappingName = "cdAddLink";
            this.dgcAddLink.ReadOnly = true;
            this.dgcAddLink.Width = 75;
            // 
            // dgcSystem
            // 
            this.dgcSystem.AllowNull = false;
            this.dgcSystem.HeaderText = "System";
            this.BresourceLookup1.SetLookup(this.dgcSystem, new FWBS.OMS.UI.Windows.ResourceLookupItem("System", "System", ""));
            this.dgcSystem.MappingName = "cdSystem";
            this.dgcSystem.NullValue = "False";
            this.dgcSystem.ReadOnly = true;
            this.dgcSystem.Width = 75;
            // 
            // gdcSelect
            // 
            this.gdcSelect.Format = "";
            this.gdcSelect.FormatInfo = null;
            this.gdcSelect.Width = 0;
            // 
            // tbCEReturn
            // 
            this.tbCEReturn.ImageIndex = 35;
            this.tbCEReturn.Name = "tbCEReturn";
            // 
            // pnlCultures
            // 
            this.pnlCultures.BackColor = System.Drawing.Color.White;
            this.pnlCultures.Controls.Add(this.cmbCulture);
            this.pnlCultures.Controls.Add(this.labUpdateCulture);
            this.pnlCultures.Controls.Add(this.cmbDisplay);
            this.pnlCultures.Controls.Add(this.labDisplayCulture);
            this.pnlCultures.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCultures.Location = new System.Drawing.Point(0, 50);
            this.pnlCultures.Name = "pnlCultures";
            this.pnlCultures.Size = new System.Drawing.Size(549, 25);
            this.pnlCultures.TabIndex = 206;
            // 
            // cmbCulture
            // 
            this.cmbCulture.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbCulture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCulture.ItemHeight = 15;
            this.cmbCulture.Location = new System.Drawing.Point(354, 0);
            this.cmbCulture.Name = "cmbCulture";
            this.cmbCulture.Size = new System.Drawing.Size(168, 23);
            this.cmbCulture.TabIndex = 205;
            this.cmbCulture.SelectionChangeCommitted += new System.EventHandler(this.cmbCulture_SelectionChangeCommitted);
            // 
            // labUpdateCulture
            // 
            this.labUpdateCulture.BackColor = System.Drawing.Color.White;
            this.labUpdateCulture.Dock = System.Windows.Forms.DockStyle.Left;
            this.labUpdateCulture.Location = new System.Drawing.Point(261, 0);
            this.BresourceLookup1.SetLookup(this.labUpdateCulture, new FWBS.OMS.UI.Windows.ResourceLookupItem("UPDATECULTURE", "Update Culture : ", ""));
            this.labUpdateCulture.Name = "labUpdateCulture";
            this.labUpdateCulture.Size = new System.Drawing.Size(93, 25);
            this.labUpdateCulture.TabIndex = 204;
            this.labUpdateCulture.Text = "Update Culture : ";
            this.labUpdateCulture.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbDisplay
            // 
            this.cmbDisplay.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbDisplay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDisplay.ItemHeight = 15;
            this.cmbDisplay.Location = new System.Drawing.Point(93, 0);
            this.cmbDisplay.Name = "cmbDisplay";
            this.cmbDisplay.Size = new System.Drawing.Size(168, 23);
            this.cmbDisplay.TabIndex = 202;
            this.cmbDisplay.SelectionChangeCommitted += new System.EventHandler(this.cmbCulture_SelectionChangeCommitted);
            // 
            // labDisplayCulture
            // 
            this.labDisplayCulture.BackColor = System.Drawing.Color.White;
            this.labDisplayCulture.Dock = System.Windows.Forms.DockStyle.Left;
            this.labDisplayCulture.Location = new System.Drawing.Point(0, 0);
            this.BresourceLookup1.SetLookup(this.labDisplayCulture, new FWBS.OMS.UI.Windows.ResourceLookupItem("DISPLAYCULTURE", "Display Culture : ", ""));
            this.labDisplayCulture.Name = "labDisplayCulture";
            this.labDisplayCulture.Size = new System.Drawing.Size(93, 25);
            this.labDisplayCulture.TabIndex = 203;
            this.labDisplayCulture.Text = "Display Culture : ";
            this.labDisplayCulture.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tb_CL_EditAll
            // 
            this.tb_CL_EditAll.ImageIndex = 24;
            this.BresourceLookup1.SetLookup(this.tb_CL_EditAll, new FWBS.OMS.UI.Windows.ResourceLookupItem("EditAll", "Edit All", ""));
            this.tb_CL_EditAll.Name = "tb_CL_EditAll";
            // 
            // tb_CL_EditOne
            // 
            this.tb_CL_EditOne.ImageIndex = 24;
            this.BresourceLookup1.SetLookup(this.tb_CL_EditOne, new FWBS.OMS.UI.Windows.ResourceLookupItem("EditOne", "Edit One", ""));
            this.tb_CL_EditOne.Name = "tb_CL_EditOne";
            // 
            // tb_CL_Open
            // 
            this.tb_CL_Open.ImageIndex = 1;
            this.BresourceLookup1.SetLookup(this.tb_CL_Open, new FWBS.OMS.UI.Windows.ResourceLookupItem("Open", "Open", ""));
            this.tb_CL_Open.Name = "tb_CL_Open";
            // 
            // tb_CL_Delete
            // 
            this.tb_CL_Delete.ImageIndex = 6;
            this.BresourceLookup1.SetLookup(this.tb_CL_Delete, new FWBS.OMS.UI.Windows.ResourceLookupItem("Delete", "Delete", ""));
            this.tb_CL_Delete.Name = "tb_CL_Delete";
            // 
            // tb_CL_Find
            // 
            this.tb_CL_Find.ImageIndex = 12;
            this.BresourceLookup1.SetLookup(this.tb_CL_Find, new FWBS.OMS.UI.Windows.ResourceLookupItem("Find", "Find", ""));
            this.tb_CL_Find.Name = "tb_CL_Find";
            // 
            // tb_CL_CDParent
            // 
            this.tb_CL_CDParent.ImageIndex = 12;
            this.BresourceLookup1.SetLookup(this.tb_CL_CDParent, new FWBS.OMS.UI.Windows.ResourceLookupItem("Parent", "Parent", ""));
            this.tb_CL_CDParent.Name = "tb_CL_CDParent";
            // 
            // parentSeparator
            // 
            this.parentSeparator.Name = "parentSeparator";
            this.parentSeparator.Size = new System.Drawing.Size(6, 6);
            // 
            // dgcType2
            // 
            this.dgcType2.Alignment = System.Windows.Forms.HorizontalAlignment.Right;
            this.dgcType2.Format = "";
            this.dgcType2.FormatInfo = null;
            this.dgcType2.HeaderText = "Type";
            this.BresourceLookup1.SetLookup(this.dgcType2, new FWBS.OMS.UI.Windows.ResourceLookupItem("Type", "Type", ""));
            this.dgcType2.MappingName = "cdType";
            this.dgcType2.ReadOnly = true;
            this.dgcType2.Width = 75;
            // 
            // lstCompare
            // 
            this.lstCompare.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.lstCompare.BackgroundColor = System.Drawing.Color.White;
            this.lstCompare.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lstCompare.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cpCode,
            this.S_Desc,
            this.S_Help,
            this.D_Desc,
            this.D_Help,
            this.AddLink,
            this.S_Cult,
            this.D_Cult,
            this.cdGroupCP,
            this.cdAddLinkCP});
            this.lstCompare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCompare.Location = new System.Drawing.Point(0, 75);
            this.lstCompare.Name = "lstCompare";
            this.lstCompare.Size = new System.Drawing.Size(549, 308);
            this.lstCompare.TabIndex = 207;
            // 
            // cpCode
            // 
            this.cpCode.DataPropertyName = "Code";
            this.cpCode.HeaderText = "Code";
            this.cpCode.Name = "cpCode";
            this.cpCode.ReadOnly = true;
            // 
            // S_Desc
            // 
            this.S_Desc.DataPropertyName = "S_Desc";
            this.S_Desc.HeaderText = "Source Description";
            this.S_Desc.Name = "S_Desc";
            this.S_Desc.ReadOnly = true;
            this.S_Desc.Width = 300;
            // 
            // S_Help
            // 
            this.S_Help.DataPropertyName = "S_Help";
            this.S_Help.HeaderText = "Source Help";
            this.S_Help.Name = "S_Help";
            this.S_Help.ReadOnly = true;
            this.S_Help.Width = 300;
            // 
            // D_Desc
            // 
            this.D_Desc.DataPropertyName = "D_Desc";
            this.D_Desc.HeaderText = "Destination Description";
            this.D_Desc.Name = "D_Desc";
            this.D_Desc.Width = 300;
            // 
            // D_Help
            // 
            this.D_Help.DataPropertyName = "D_Help";
            this.D_Help.HeaderText = "Destination Help";
            this.D_Help.Name = "D_Help";
            this.D_Help.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.D_Help.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.D_Help.Width = 300;
            // 
            // AddLink
            // 
            this.AddLink.DataPropertyName = "AddLink";
            this.AddLink.HeaderText = "AddLink";
            this.AddLink.Name = "AddLink";
            this.AddLink.Visible = false;
            // 
            // S_Cult
            // 
            this.S_Cult.DataPropertyName = "S_Cult";
            this.S_Cult.HeaderText = "S_Cult";
            this.S_Cult.Name = "S_Cult";
            this.S_Cult.Visible = false;
            // 
            // D_Cult
            // 
            this.D_Cult.DataPropertyName = "D_Cult";
            this.D_Cult.HeaderText = "D_Cult";
            this.D_Cult.Name = "D_Cult";
            this.D_Cult.Visible = false;
            // 
            // cdGroupCP
            // 
            this.cdGroupCP.DataPropertyName = "cdGroup";
            this.cdGroupCP.HeaderText = "cdGroup";
            this.cdGroupCP.Name = "cdGroupCP";
            this.cdGroupCP.Visible = false;
            // 
            // cdAddLinkCP
            // 
            this.cdAddLinkCP.DataPropertyName = "cdAddLink";
            this.cdAddLinkCP.HeaderText = "cdAddLink";
            this.cdAddLinkCP.Name = "cdAddLinkCP";
            this.cdAddLinkCP.Visible = false;
            // 
            // lstCodeLookup
            // 
            this.lstCodeLookup.AllowUserToResizeRows = false;
            this.lstCodeLookup.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.lstCodeLookup.BackgroundColor = System.Drawing.Color.White;
            this.lstCodeLookup.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lstCodeLookup.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cdType,
            this.cdCode,
            this.cdUICultureInfo,
            this.cdDesc,
            this.cdSystem,
            this.cdGroup,
            this.cdAddLink,
            this.cdHelp,
            this.cdID,
            this.rowGuid,
            this.cdDeletable,
            this.cdNotes});
            this.lstCodeLookup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCodeLookup.Location = new System.Drawing.Point(0, 75);
            this.lstCodeLookup.Name = "lstCodeLookup";
            this.lstCodeLookup.Size = new System.Drawing.Size(549, 308);
            this.lstCodeLookup.TabIndex = 208;
            this.lstCodeLookup.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.lstCodeLookup_DataError);
            // 
            // cdType
            // 
            this.cdType.DataPropertyName = "cdType";
            this.cdType.HeaderText = "Type";
            this.cdType.Name = "cdType";
            this.cdType.ReadOnly = true;
            // 
            // cdCode
            // 
            this.cdCode.DataPropertyName = "cdCode";
            this.cdCode.HeaderText = "Code";
            this.cdCode.Name = "cdCode";
            this.cdCode.ReadOnly = true;
            // 
            // cdUICultureInfo
            // 
            this.cdUICultureInfo.DataPropertyName = "cdUICultureInfo";
            this.cdUICultureInfo.HeaderText = "Culture";
            this.cdUICultureInfo.Name = "cdUICultureInfo";
            this.cdUICultureInfo.Width = 150;
            // 
            // cdDesc
            // 
            this.cdDesc.DataPropertyName = "cdDesc";
            this.cdDesc.HeaderText = "Description";
            this.cdDesc.Name = "cdDesc";
            this.cdDesc.Width = 300;
            // 
            // cdSystem
            // 
            this.cdSystem.DataPropertyName = "cdSystem";
            this.cdSystem.HeaderText = "System";
            this.cdSystem.Name = "cdSystem";
            // 
            // cdGroup
            // 
            this.cdGroup.DataPropertyName = "cdGroup";
            this.cdGroup.HeaderText = "Group";
            this.cdGroup.Name = "cdGroup";
            // 
            // cdAddLink
            // 
            this.cdAddLink.DataPropertyName = "cdAddLink";
            this.cdAddLink.HeaderText = "Add Link";
            this.cdAddLink.Name = "cdAddLink";
            // 
            // cdHelp
            // 
            this.cdHelp.DataPropertyName = "cdHelp";
            this.cdHelp.HeaderText = "Help";
            this.cdHelp.Name = "cdHelp";
            this.cdHelp.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cdHelp.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cdHelp.Width = 300;
            // 
            // cdID
            // 
            this.cdID.DataPropertyName = "cdID";
            this.cdID.HeaderText = "cdID";
            this.cdID.Name = "cdID";
            this.cdID.Visible = false;
            // 
            // rowGuid
            // 
            this.rowGuid.DataPropertyName = "rowGuid";
            this.rowGuid.HeaderText = "rowGuid";
            this.rowGuid.Name = "rowGuid";
            this.rowGuid.Visible = false;
            // 
            // cdDeletable
            // 
            this.cdDeletable.DataPropertyName = "cdDeletable";
            this.cdDeletable.HeaderText = "cdDeletable";
            this.cdDeletable.Name = "cdDeletable";
            this.cdDeletable.Visible = false;
            // 
            // cdNotes
            // 
            this.cdNotes.DataPropertyName = "cdNotes";
            this.cdNotes.HeaderText = "Notes";
            this.cdNotes.Name = "cdNotes";
            this.cdNotes.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.cdNotes.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.cdNotes.Width = 300;
            // 
            // lstNewCodes
            // 
            this.lstNewCodes.AllowUserToResizeRows = false;
            this.lstNewCodes.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.lstNewCodes.BackgroundColor = System.Drawing.Color.White;
            this.lstNewCodes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.lstNewCodes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewComboBoxColumn1,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewCheckBoxColumn2,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9});
            this.lstNewCodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstNewCodes.GridColor = System.Drawing.Color.Silver;
            this.lstNewCodes.Location = new System.Drawing.Point(0, 75);
            this.lstNewCodes.Name = "lstNewCodes";
            this.lstNewCodes.Size = new System.Drawing.Size(549, 308);
            this.lstNewCodes.TabIndex = 209;
            this.lstNewCodes.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.lstCodeLookup_DataError);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "cdType";
            this.dataGridViewTextBoxColumn1.HeaderText = "Type";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "cdCode";
            this.dataGridViewTextBoxColumn2.HeaderText = "Code";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.DataPropertyName = "cdUICultureInfo";
            this.dataGridViewComboBoxColumn1.HeaderText = "Culture";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            this.dataGridViewComboBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "cdDesc";
            this.dataGridViewTextBoxColumn3.HeaderText = "Description";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Width = 300;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "cdSystem";
            this.dataGridViewCheckBoxColumn1.HeaderText = "System";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.DataPropertyName = "cdGroup";
            this.dataGridViewCheckBoxColumn2.HeaderText = "Group";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "cdAddLink";
            this.dataGridViewTextBoxColumn4.HeaderText = "Add Link";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "cdHelp";
            this.dataGridViewTextBoxColumn5.HeaderText = "Help";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn5.Width = 300;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "cdID";
            this.dataGridViewTextBoxColumn6.HeaderText = "cdID";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.Visible = false;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "rowGuid";
            this.dataGridViewTextBoxColumn7.HeaderText = "rowGuid";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "cdDeletable";
            this.dataGridViewTextBoxColumn8.HeaderText = "cdDeletable";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.Visible = false;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "cdNotes";
            this.dataGridViewTextBoxColumn9.HeaderText = "Notes";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewTextBoxColumn9.Width = 300;
            // 
            // ucCodeLookups
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Name = "ucCodeLookups";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstList)).EndInit();
            this.pnlEdit.ResumeLayout(false);
            this.pnlQuickSearch.ResumeLayout(false);
            this.pnlQuickSearch.PerformLayout();
            this.pnlCultures.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstCompare)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstCodeLookup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lstNewCodes)).EndInit();
            this.ResumeLayout(false);

        }


        #endregion

        #region Private Control Method
        private new void tbcLists_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
        {
            if (e.Button == tb_CL_Find)
            {
                FindCodeLookup();
            }
            else if (e.Button == tb_CL_EditAll)
            {
                CodeLookupCompare();
            }
            else if (e.Button == tb_CL_Open && this.ListMode)
            {
                LoadSingleItem("");
            }
            else if (lstList.CurrentRowIndex != -1)
            {
                editingcode = Convert.ToString(dtCL.DefaultView[lstList.CurrentRowIndex]["cdcode"]);

                if (e.Button == tb_CL_EditOne)
                {
                    EditSingleCodeLookupItem();
                }
                else if (e.Button == tb_CL_Delete)
                {
                    DeleteCodeLookup();
                }
                else if (e.Button == tb_CL_CDParent)
                {
                    if (EditingState == CodeLookupEditingState.List)
                        ParentType();
                }
            }
            else if (e.Button == tb_CL_CDParent)
            {
                if (EditingState == CodeLookupEditingState.List)
                    ParentType();
            }
        }

        private void lstList_DoubleClick(object sender, System.EventArgs e)
        {
            int y = lstList.CurrentRowIndex;
            if (y != -1)
                LoadSingleItem("");
        }

        private void cmbCulture_SelectionChangeCommitted(object sender, System.EventArgs e)
        {
            RefreshCodeLookupCompare();
            txtSearch_TextChanged(sender, e);
        }

        private void dtCC_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            cmbDisplay.Enabled = false;
            cmbCulture.Enabled = false;
            this.IsDirty = true;
        }

        private void lstList_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                LoadSingleItem("");
            }
            else if (e.KeyChar == 8 && sender == lstList)
            {
                if (EditingState == CodeLookupEditingState.List)
                    ParentType();
            }
        }

        private void lstEdit_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            this.IsDirty = true;
        }

        private void DefaultView_ListChanged(object sender, ListChangedEventArgs e)
        {
            tb_CL_EditAll.Enabled = dtCL.DefaultView.Count > 0;
            tb_CL_EditOne.Enabled = dtCL.DefaultView.Count > 0;
            tb_CL_Open.Enabled = dtCL.DefaultView.Count > 0;
            tb_CL_Delete.Enabled = dtCL.DefaultView.Count > 0;
        }

        private void lstCodeLookup_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.ShowInformation(e.Exception.Message);
        }

        #endregion

        #region Overrides
        protected override void NewData()
        {
            NewCodeLookup();
        }

        protected override void ShowEditor()
        {
            this.tb_CL_CDParent.Enabled = false;
            base.ShowEditor();
        }

        protected override void ShowList()
        {
            SetParentEnabledState();
            SetEditAllEnabledState();
            cmbCulture.Enabled = true;
            cmbDisplay.Enabled = true;
            EditingState = CodeLookupEditingState.List;

            if (!string.IsNullOrWhiteSpace(lasttype))
            {
                RefreshCodeLookupList(lasttype);
                currenttype = lasttype;
            }
            else
            {
                RefreshCodeLookupList(currenttype);
            }

            base.ShowList();
        }

        protected override bool UpdateData()
        {
            SaveCodeLookupChanges();
            return true;
        }

        protected override DataTable GetData()
        {
            //
            // Code Lookup Pages
            //
            string getdatatype;
            if (!string.IsNullOrWhiteSpace(lasttype))
                getdatatype = lasttype;
            else
                getdatatype = currenttype;
            dtCL = CodeLookup.GetLookupsAndAddLinks(getdatatype);
            dtCL.Columns.Add("cdIcon", typeof(Int32), "IIF(cdGroup,36,12)");
            dtCL.DefaultView.Sort = "[CDCODE]";
            dtCL.DefaultView.ListChanged += new ListChangedEventHandler(DefaultView_ListChanged);
            return dtCL;
        }

        protected override void LoadSingleItem(string Code)
        {
            try
            {
                string code = Convert.ToString(dtCL.DefaultView[lstList.CurrentRowIndex]["cdcode"]);
                bool group = Convert.ToBoolean(dtCL.DefaultView[lstList.CurrentRowIndex]["cdgroup"]);
                if (group)
                {
                    if (tb_CL_Open.Enabled == false)
                    {
                        tb_CL_Open.Enabled = false;
                    }
                    else
                    {
                        if (dtCL != null)
                            dtCL.DefaultView.ListChanged -= new ListChangedEventHandler(DefaultView_ListChanged);
                        dtCL = CodeLookup.GetLookupsAndAddLinks(code);
                        dtCL.DefaultView.ListChanged += new ListChangedEventHandler(DefaultView_ListChanged);
                        if (dtCL.Columns.Contains("rowguid"))
                            dtCL.Columns["rowguid"].AllowDBNull = true;
                        dtCL.Columns.Add("cdIcon", typeof(Int32), "IIF(cdGroup,36,12)");
                        CodeTypesHistory.Add(code);
                        currenttype = code;
                        SetParentEnabledState();
                        lstList.DataSource = dtCL;
                        CheckBtnOpenEnabled();
                        this.lstList.CaptionText = ResourceLookup.GetLookupText("CodeLookup", "Code Lookup", "", false) + " / ";
                        for (int t = 0; t < CodeTypesHistory.Count; t++)
                        {
                            this.lstList.CaptionText = this.lstList.CaptionText + CodeTypesHistory[t].ToString() + " / ";
                        }
                        currenttype = code;
                        try
                        {
                            if (lstList.VisibleRowCount > 0)
                            {
                                lstList.CurrentRowIndex = 0;
                                lstList.Select(0);
                            }
                        }
                        catch
                        { }
                        txtSearch.Text = "";
                    }
                }
            }
            catch { }
        }

        #endregion

        #region Private


        private void NewCodeLookup()
        {
            EditingState = CodeLookupEditingState.New;
            if(currenttype != "OMS")
                lasttype = currenttype;
            ShowEditor();
            tbClose.Visible = true;
            tbSave.Text = ResourceLookup.GetLookupText("SAVE&&CLOSE", "Save && Close", null, null);
            tbClose.Text = ResourceLookup.GetLookupText("CLOSENOSAVE", "Close Without Saving", null, null);
            pnlCultures.Visible = false;
            labSelectedObject.Text = currenttype.ToUpperInvariant() + " - " + Session.CurrentSession.Resources.GetResource("NewCodelookups", "New Codelookups", "").Text;
            _code = new CodeLookup(currenttype);
            if (dtCC != null)
            {
                dtCC.RowDeleted -= new DataRowChangeEventHandler(dtCC_RowChanged);
                dtCC.RowChanged -= new DataRowChangeEventHandler(dtCC_RowChanged);
            }
            dtCC = _code.GetDataTable();
            dtCC.RowDeleted += new DataRowChangeEventHandler(dtCC_RowChanged);
            dtCC.RowChanged += new DataRowChangeEventHandler(dtCC_RowChanged);

            if (dtCC.Columns.Contains("rowguid"))
                dtCC.Columns["rowguid"].AllowDBNull = true;
            dtCC.DefaultView.AllowDelete = true;
            dataGridViewComboBoxColumn1.DataSource = CodeLookup.GetCultures(false, true);
            dataGridViewComboBoxColumn1.DisplayMember = "langdesc";
            dataGridViewComboBoxColumn1.ValueMember = "langcode";

            lstNewCodes.DataSource = dtCC;
            lstCodeLookup.Visible = false;
            lstCompare.Visible = false;
            lstNewCodes.Visible = true;
            lstNewCodes.Focus();
        }

        private void SaveCodeLookupChanges()
        {
            try
            {
                base.tbSave.Enabled = false;
                switch (EditingState)
                {
                    case CodeLookupEditingState.New:
                        {
                            CurrencyManager cm = (CurrencyManager)BindingContext[lstNewCodes.DataSource, lstNewCodes.DataMember];
                            cm.EndCurrentEdit();
                            _code.Update();
                            ShowList();
                            break;
                        }
                    case CodeLookupEditingState.Edit:
                        {
                            CurrencyManager cm = (CurrencyManager)BindingContext[lstCodeLookup.DataSource, lstCodeLookup.DataMember];
                            cm.EndCurrentEdit();
                            _code.Update();
                            EditSingleCodeLookupItem();
                            break;
                        }
                    case CodeLookupEditingState.Compare:
                        {
                            DataTable changes = dtCC.GetChanges();
                            if (changes != null)
                            {
                                DataTable edits = null;
                                edits = CodeLookup.GetLookups(new System.Globalization.CultureInfo(Convert.ToString(cmbCulture.SelectedValue)), currenttype);
                                foreach (DataRow dr in changes.Rows)
                                {
                                    CodeLookup.Create(currenttype, Convert.ToString(dr["Code"]), Convert.ToString(dr["D_Desc"]), Convert.ToString(dr["D_Help"]), Convert.ToString(cmbCulture.SelectedValue), false, Convert.ToBoolean(dr["cdGroup"]), dr["cdAddLink"], true, true);
                                }
                            }
                            cmbDisplay.Enabled = true;
                            cmbCulture.Enabled = true;
                            break;
                        }
                }
            }
            finally
            {
                this.IsDirty = false;
                base.tbSave.Enabled = true;
                base.tbClose.Enabled = true;
            }
        }

        private void SetParentEnabledState()
        {
            if (CodeTypesHistory.Count == 1 || currenttype == "OMS" || currenttype == "Code Lookups")
                this.tb_CL_CDParent.Enabled = false;
            else
                this.tb_CL_CDParent.Enabled = true;
        }

        private void SetEditAllEnabledState()
        {
            if (CodeTypesHistory.Count == 1 || currenttype == "OMS")
                this.tb_CL_EditAll.Enabled = false;
            else
                this.tb_CL_EditAll.Enabled = true;
        }

        private void FindCodeLookup()
        {
            try
            {
                FWBS.Common.KeyValueCollection ret = Services.Searches.ShowSearch(Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.FindCodeLookup), new Size(512, 459), null, new FWBS.Common.KeyValueCollection());
                if (ret != null)
                {
                    currenttype = Convert.ToString(ret["cdtype"].Value);
                    if (dtCL != null)
                        dtCL.DefaultView.ListChanged -= new ListChangedEventHandler(DefaultView_ListChanged);
                    dtCL = CodeLookup.GetLookupsAndAddLinks(currenttype);
                    dtCL.DefaultView.ListChanged += new ListChangedEventHandler(DefaultView_ListChanged);
                    dtCL.Columns.Add("cdIcon", typeof(Int32), "IIF(cdGroup,36,12)");
                    lstList.DataSource = null;
                    txtSearch.Text = string.Empty;
                    lstList.CaptionText = string.Format("{0} / {1}", ResourceLookup.GetLookupText("CodeLookup", "Code Lookup", "", false), BuildCodeTypesHistory(currenttype));
                    lstList.DataSource = dtCL;
                    lstList.Refresh();
                    txtSearch.Text = Convert.ToString(ret["cdcode"].Value);
                    this.tb_CL_CDParent.Enabled = true;
                    CheckBtnOpenEnabled();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private string BuildCodeTypesHistory(string _currenttype)
        {
            string historyPath = "";
            CodeTypesHistory.Clear();
            string type = _currenttype;

            DataTable codelookups = GetCodeLookups();
            historyPath = type;


            var buildHistoryPath = new Func<string, bool>((whereClause) =>
            {
                DataRow[] rows = codelookups.Select(string.Format(whereClause, type));
                if (rows != null && rows.Length > 0)
                {
                    type = Convert.ToString(rows[0]["cdType"]);
                    historyPath = type + " / " + historyPath;
                    return true;
                }
                return false;
            });

            while (type != "OMS")
            {
                CodeTypesHistory.Insert(0, type);

                if (!buildHistoryPath("cdCode = '{0}' and cdGroup = 1"))
                    if (!buildHistoryPath("cdCode = '{0}'"))
                        break;
            }

            CodeTypesHistory.Insert(0, type);
            return historyPath;
        }

        public DataTable GetCodeLookups()
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            string sql = @"select * from dbCodelookup";
            System.Data.DataTable dt = connection.ExecuteSQL(sql, null);
            return dt;
        }


        private void DeleteCodeLookup()
        {
            if (string.IsNullOrEmpty(editingcode))
                return; 
            
            try
            {
                object addlink = dtCL.DefaultView[lstList.CurrentRowIndex]["cdaddlink"];
                if (FWBS.OMS.UI.Windows.MessageBox.Show(FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText("AreYouSure", "Are you sure you wish to Delete %1%", "", editingcode), "OMS Admin", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (CodeLookup.Delete(currenttype, editingcode, addlink) == false)
                    {
                        FWBS.OMS.UI.Windows.MessageBox.Show(this, FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText("ErrorDelete", "Error Deleting Code Lookup called %1%", "", editingcode), FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText("FWBS.OMS.UI.Windows.Admin"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        this.txtSearch.Text = string.Empty;
                        RefreshCodeLookupList(currenttype);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void CodeLookupCompare()
        {
            if (dtCL.DefaultView.Count > 0)
            {
                EditingState = CodeLookupEditingState.Compare;
                lasttype = currenttype;
                tbSave.Text = ResourceLookup.GetLookupText("Save");
                tbClose.Text = ResourceLookup.GetLookupText("Close");
                string item = Convert.ToString(dtCL.DefaultView[lstList.CurrentRowIndex]["cdcode"]);
                labSelectedObject.Text = currenttype.ToUpperInvariant() + " - " + Session.CurrentSession.Resources.GetResource("CultureEditor", "Culture Editor", "").Text;
                ShowEditor();
                RefreshCodeLookupCompare();
                pnlCultures.Visible = true;
                lstCodeLookup.Visible = false;
                lstCompare.Visible = true;
                lstNewCodes.Visible = false;
                cmbDisplay.SelectedValue = "{default}";
                cmbCulture.SelectedValue = FWBS.OMS.Session.CurrentSession.DefaultCulture;
                cmbCulture_SelectionChangeCommitted(this, EventArgs.Empty);
                lstCompare.Focus();
            }
        }

        private void EditSingleCodeLookupItem()
        {
            if (string.IsNullOrEmpty(editingcode))
                return;

            lasttype = currenttype;
            EditingState = CodeLookupEditingState.Edit;
            tbSave.Text = ResourceLookup.GetLookupText("Save");
            tbClose.Text = ResourceLookup.GetLookupText("Close");
            tbClose.Visible = true;
            ShowEditor();
            labSelectedObject.Text = editingcode.ToUpperInvariant() + " - " + Session.CurrentSession.Resources.GetResource("CodelookupType", "Codelookup Type", "").Text;
            pnlCultures.Visible = false;
            _code = new CodeLookup(lasttype, editingcode);

            if (dtCC != null)
            {
                dtCC.RowDeleted -= new DataRowChangeEventHandler(dtCC_RowChanged);
                dtCC.RowChanged -= new DataRowChangeEventHandler(dtCC_RowChanged);
            }
            dtCC = _code.GetDataTable();
            dtCC.RowChanged += new DataRowChangeEventHandler(dtCC_RowChanged);
            dtCC.RowDeleted += new DataRowChangeEventHandler(dtCC_RowChanged);
            dtCC.DefaultView.AllowDelete = true;
            lstCodeLookup.DataSource = dtCC;
            lstCodeLookup.Visible = true;
            lstCompare.Visible = false;
            lstNewCodes.Visible = false;
            lstCodeLookup.Focus();
        }

        private void RefreshCodeLookupCompare()
        {
            if (dtCC != null)
            {
                dtCC.RowChanged -= new DataRowChangeEventHandler(dtCC_RowChanged);
                dtCC.RowDeleted -= new DataRowChangeEventHandler(dtCC_RowChanged);
            }
            dtCC = CodeLookup.GetLookupsCompare(currenttype, cmbDisplay.SelectedValue.ToString(), cmbCulture.SelectedValue.ToString());
            dtCC.RowChanged += new DataRowChangeEventHandler(dtCC_RowChanged);
            dtCC.DefaultView.Sort = "Code";
            dtCC.DefaultView.AllowNew = false;

            lstCompare.DataSource = dtCC;
        }

        private void ParentCodeLookups()
        {
            string type = "";
            if (CodeTypesHistory.Count > 0)
            {
                if (Convert.ToString(CodeTypesHistory[CodeTypesHistory.Count - 1]) == "OMS")
                {
                    currenttype = "OMS";
                    type = Convert.ToString(CodeTypesHistory[CodeTypesHistory.Count - 1]);
                }
                else
                {
                    currenttype = Convert.ToString(CodeTypesHistory[CodeTypesHistory.Count - 1]);
                }
                type = Convert.ToString(CodeTypesHistory[CodeTypesHistory.Count - 1]);
            }
            else
            {
                currenttype = "Code Lookups";
                type = "OMS";
                CodeTypesHistory.Add("OMS");
            }
            RefreshCodeLookupList(type);
        }

        private void RefreshCodeLookupList(string type)
        {
            if (dtCL != null)
                dtCL.DefaultView.ListChanged -= new ListChangedEventHandler(DefaultView_ListChanged);
            dtCL = CodeLookup.GetLookupsAndAddLinks(type);
            dtCL.DefaultView.ListChanged += new ListChangedEventHandler(DefaultView_ListChanged);
            dtCL.Columns.Add("cdIcon", typeof(Int32), "IIF(cdGroup,36,12)");
            lstList.DataSource = dtCL;
            lstList.Refresh();
            this.lstList.CaptionText = ResourceLookup.GetLookupText("CodeLookup", "Code Lookup", "", false) + " / ";
            if (CodeTypesHistory.Count > 0)
            {
                for (int t = 0; t < CodeTypesHistory.Count; t++)
                {
                    this.lstList.CaptionText = this.lstList.CaptionText + CodeTypesHistory[t].ToString() + " / ";
                }
            }
            txtSearch_TextChanged(null, new EventArgs());
        }

        private void ParentType()
        {
            if (CodeTypesHistory.Count > 0)
                CodeTypesHistory.RemoveAt(CodeTypesHistory.Count - 1);
            ParentCodeLookups();
            tb_CL_Open.Enabled = true;
            SetParentEnabledState();
            SetEditAllEnabledState();

            if (lstList.VisibleRowCount > 0)
            {
                try
                {
                    lstList.CurrentRowIndex = 0;
                    lstList.Select(0);
                }
                catch
                { }
            }
            Session.CurrentSession.Resources.Refresh();
            txtSearch.Text = "";
        }

        protected override void CloseAndReturnToList()
        {
            ShowList();
        }

        private void CheckBtnOpenEnabled()
        {
            if (dtCL != null && dtCL.Rows.Count > 0)
            {
                this.tb_CL_Open.Enabled = Convert.ToBoolean(dtCL.DefaultView[lstList.CurrentRowIndex]["cdgroup"]);
            }
            else
            {
                this.tb_CL_Open.Enabled = false;
            }
        }

        protected override void CurrentCellChanged()
        {
            CheckBtnOpenEnabled();
        }

        #endregion


    }
}
