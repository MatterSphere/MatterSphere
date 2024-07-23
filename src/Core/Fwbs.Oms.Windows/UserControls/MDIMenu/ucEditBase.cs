using System;
using System.Data;
using System.Windows.Forms;
using Infragistics.Win.UltraWinTabControl;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucEditBase.
    /// </summary>
    public class ucEditBase : System.Windows.Forms.UserControl, IOBjectDirty, IObjectUpdate
	{
		public System.Windows.Forms.Panel tpList;
		public System.Windows.Forms.Panel tpEdit;
		protected System.Windows.Forms.ToolBarButton tbNew;
		protected FWBS.Common.UI.Windows.ToolBar tbcLists;
		protected System.Windows.Forms.ToolBarButton tbEdit;
		protected System.Windows.Forms.ToolBarButton tbDelete;
		protected System.Windows.Forms.ToolBarButton tbSp1;
		protected System.Windows.Forms.ToolBarButton tbShowTrash;
		protected System.Windows.Forms.ToolBarButton tbShowActive;
		protected System.Windows.Forms.ToolBarButton tbSp2;
		protected System.Windows.Forms.ToolBarButton tbRestore;
		public FWBS.OMS.UI.Windows.DataGridEx lstList;
		private System.ComponentModel.IContainer components;

		private DataGrid.HitTestInfo _currentItem = null;
		private bool _isdirty = false;
		protected System.Windows.Forms.Panel pnlEdit;
		protected System.Windows.Forms.Label labSelectedObject;
		protected FWBS.Common.UI.Windows.ToolBar tbcEdit;
		protected System.Windows.Forms.ToolBarButton tbSave;
        protected System.Windows.Forms.ToolBarButton tbClose;
		protected System.Windows.Forms.ToolBarButton tbReturn;
		protected System.Windows.Forms.DataGridTableStyle lstStyle;
		protected FWBS.OMS.UI.Windows.DataGridImageColumn dgcCode;
		protected System.Windows.Forms.DataGridTextBoxColumn dgcDesc;
		protected internal System.Windows.Forms.DataGridTextBoxColumn dgcNone;
		protected DataTable dtCT = null;
		public System.Windows.Forms.TextBox txtSearch;
		private System.Windows.Forms.Label label1;
		protected FWBS.OMS.UI.Windows.ResourceLookup BresourceLookup1;
		protected System.Windows.Forms.ToolBarButton tbClone;
		protected System.Windows.Forms.Panel pnlQuickSearch;
		protected string SelectCode = "";
		private IMainParent _mainparent = null;
		protected System.Windows.Forms.FolderBrowserDialog ExportDlg;
		private System.Windows.Forms.Button btnBlue;
        protected Panel pnlListbarContainer;
        protected Panel pnlToolbarContainer;
		private Control _editparent = null;
        private readonly string _modifiedLookup = "[Modified]";
        public event EventHandler Dirty;

		// Invoke the Dirty event; when changed
		protected virtual void OnDirty(EventArgs e) 
		{
			if (Dirty != null)
				Dirty(this, e);
		}
		
		public ucEditBase()
		{
			InitializeComponent();
            this.tbcEdit.ImageList = FWBS.OMS.UI.Windows.Images.Windows8();
            this.tbcLists.ImageList = FWBS.OMS.UI.Windows.Images.Windows8();
        }

        public ucEditBase(IMainParent mainparent, Control editparent) : this()
		{
			_mainparent = mainparent;
			_editparent = editparent;

			this.tpList.Dock = DockStyle.Fill;
			this.tpEdit.Dock = DockStyle.Fill;
            this.Dirty -= new System.EventHandler(this.ucEditBack1_Dirty);
            this.Dirty += new System.EventHandler(this.ucEditBack1_Dirty);

			if (Session.CurrentSession.IsLoggedIn)
			{
                this._currenttype = "OMS";
				Text = Session.CurrentSession.Resources.GetResource("ADMIN","OMS Administration","").Text;
				dtCT = GetData();
				lstList.DataSource = dtCT;
				_modifiedLookup = $"[{Session.CurrentSession.Resources.GetResource("MODIFIED", "Modified", "").Text}]";
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

		public bool IsDirty
		{
			get
			{
				return _isdirty;
			}
			set
			{
				_isdirty = value;
				if (value) 
				{
					OnDirty(EventArgs.Empty);
					labSelectedObject.Text = labSelectedObject.Text.Replace($"{_modifiedLookup} - ", "");
					labSelectedObject.Text = $"{_modifiedLookup} - {labSelectedObject.Text}";
				}
				else
				{
					labSelectedObject.Text = labSelectedObject.Text.Replace($"{_modifiedLookup} - ", "");
				}
			}

		}

		public IMainParent MainParent
		{
			get
			{
				return _mainparent;
			}
		}

		public Control EditParent
		{
			get
			{
				return _editparent;
			}
		}

        public string OriginalTabText { get; set; }

        public UltraTab HostingTab { get; set; }

        private string _currenttype;
        protected string currenttype
        {
            get
            {
                return _currenttype;
            }
            set
            {
                _currenttype = value;
                SetHostingTabText();
            }
        }

        private string _lasttype;
        protected string lasttype
        {
            get
            {
                return _lasttype;
            }
            set
            {
                _lasttype = value;
            }
        }

        public bool directcodelookupaccess;
        public bool DirectCodeLookupAccess
        {
            get
            {
                return directcodelookupaccess;
            }
            set
            {
                directcodelookupaccess = value;
            }
        }

        private void SetHostingTabText()
        {
            if(!string.IsNullOrWhiteSpace(_currenttype) && HostingTab != null)
            {
                if (_currenttype == "OMS")
                    HostingTab.Text = "Code Lookups";
                else
                {
                    if (directcodelookupaccess)
                        HostingTab.Text = OriginalTabText;
                    else
                        HostingTab.Text = _currenttype;
                }
            }
        }

		private void ucEditBack1_Dirty(object sender, System.EventArgs e)
		{
			if (_isdirty == false)
			{
				labSelectedObject.Text = labSelectedObject.Text.Replace($"{_modifiedLookup} - ", "");
				labSelectedObject.Text = $"{_modifiedLookup} - {labSelectedObject.Text}";
			}
			_isdirty=true;
		}

		private void lstList_DoubleClick(object sender, System.EventArgs e)
		{
			if (_currentItem != null && _currentItem.Row > -1 )
				tbcLists_ButtonClick(sender,new ToolBarButtonClickEventArgs(tbEdit));
		}

		private void lstList_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			_currentItem = lstList.HitTest(e.X, e.Y);
		}

		private void lstList_CurrentCellChanged(object sender, System.EventArgs e)
		{
			lstList.CurrentCell = new DataGridCell(lstList.CurrentRowIndex,0);
			lstList.Select(lstList.CurrentRowIndex);
		    CurrentCellChanged();
		}

		protected virtual void LoadSingleItem(string Code)
		{
			if (!DesignMode)
				FWBS.OMS.UI.Windows.MessageBox.Show("You must implement a Override for the LoadSingleItem Method","ucEditBase",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
		}

        public void UpdateObjectData()
        {
            UpdateData();
        }

		protected virtual bool UpdateData()
		{
			if (!DesignMode)
				FWBS.OMS.UI.Windows.MessageBox.Show("You must implement a Override for the UpdateData Method","ucEditBase",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			return false;
		}

		protected virtual void NewData()
		{
			if (!DesignMode)
				FWBS.OMS.UI.Windows.MessageBox.Show("You must implement a Override for the NewData Method","ucEditBase",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
		}

		protected virtual void DeleteData(string Code)
		{
			if (!DesignMode)
				FWBS.OMS.UI.Windows.MessageBox.Show("You must implement a Override for the DeleteData Method","ucEditBase",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
		}
		
		
		protected virtual bool CancelData()
		{
			return true;
		}

		protected virtual DataTable GetData()
		{
			if (!DesignMode)
				FWBS.OMS.UI.Windows.MessageBox.Show("You must implement a Override for the GetData Method","ucEditBase",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			return null;
		}

		protected virtual bool Restore(string Code)
		{
			if (!DesignMode)
				FWBS.OMS.UI.Windows.MessageBox.Show("You must implement a Override for the Restore Method","ucEditBase",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			return false;
		}
			
		protected virtual void Clone(string Code)
		{
			if (!DesignMode)
				FWBS.OMS.UI.Windows.MessageBox.Show("You must implement a Override for the Clone Method","ucEditBase",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
			return;
		}

		private void SaveChanges()
		{
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (UpdateData())
                {
                    IsDirty = false;
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
                return;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
		}

		public bool IsObjectDirty()
		{
			if (IsDirty)
			{
				btnBlue.Focus();
				DialogResult dr = System.Windows.Forms.MessageBox.Show(this.EditParent,Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?","").Text,"OMS Admin",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
				if (dr == DialogResult.Yes) 
				{
					SaveChanges();
					if (IsDirty) return false;
				}
				if (dr == DialogResult.No)
				{
					IsDirty=false;
					CancelData();
				}
				if (dr == DialogResult.Cancel) return false;
			}		
			return true;
		}

        public DialogResult? IsObjectDirtyDialogResult()
        {
            if (IsDirty)
            {
                btnBlue.Focus();
                DialogResult dr = System.Windows.Forms.MessageBox.Show(this.EditParent, Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?", "").Text, "OMS Admin", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    SaveChanges();
                    if (IsDirty) return DialogResult.Yes;
                }
                if (dr == DialogResult.No)
                {
                    IsDirty = false;
                    return DialogResult.No;
                }
                if (dr == DialogResult.Cancel) return DialogResult.Cancel;
            }
            return null;
        }

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.tpList = new System.Windows.Forms.Panel();
            this.lstList = new FWBS.OMS.UI.Windows.DataGridEx();
            this.lstStyle = new System.Windows.Forms.DataGridTableStyle();
            this.dgcNone = new System.Windows.Forms.DataGridTextBoxColumn();
            this.dgcCode = new FWBS.OMS.UI.Windows.DataGridImageColumn();
            this.dgcDesc = new System.Windows.Forms.DataGridTextBoxColumn();
            this.pnlQuickSearch = new System.Windows.Forms.Panel();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlListbarContainer = new System.Windows.Forms.Panel();
            this.tbcLists = new FWBS.Common.UI.Windows.ToolBar();
            this.tbNew = new System.Windows.Forms.ToolBarButton();
            this.tbEdit = new System.Windows.Forms.ToolBarButton();
            this.tbDelete = new System.Windows.Forms.ToolBarButton();
            this.tbClone = new System.Windows.Forms.ToolBarButton();
            this.tbSp1 = new System.Windows.Forms.ToolBarButton();
            this.tbShowActive = new System.Windows.Forms.ToolBarButton();
            this.tbShowTrash = new System.Windows.Forms.ToolBarButton();
            this.tbSp2 = new System.Windows.Forms.ToolBarButton();
            this.tbRestore = new System.Windows.Forms.ToolBarButton();
            this.tpEdit = new System.Windows.Forms.Panel();
            this.pnlEdit = new System.Windows.Forms.Panel();
            this.labSelectedObject = new System.Windows.Forms.Label();
            this.pnlToolbarContainer = new System.Windows.Forms.Panel();
            this.tbcEdit = new FWBS.Common.UI.Windows.ToolBar();
            this.tbSave = new System.Windows.Forms.ToolBarButton();
            this.tbClose = new System.Windows.Forms.ToolBarButton();
            this.tbReturn = new System.Windows.Forms.ToolBarButton();
            this.btnBlue = new System.Windows.Forms.Button();
            this.BresourceLookup1 = new FWBS.OMS.UI.Windows.ResourceLookup(this.components);
            this.ExportDlg = new System.Windows.Forms.FolderBrowserDialog();
            this.tpList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lstList)).BeginInit();
            this.pnlQuickSearch.SuspendLayout();
            this.pnlListbarContainer.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpList
            // 
            this.tpList.Controls.Add(this.lstList);
            this.tpList.Controls.Add(this.pnlQuickSearch);
            this.tpList.Controls.Add(this.pnlListbarContainer);
            this.tpList.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tpList.Location = new System.Drawing.Point(56, 31);
            this.tpList.Name = "tpList";
            this.tpList.Size = new System.Drawing.Size(549, 383);
            this.tpList.TabIndex = 0;
            this.tpList.Text = "List";
            this.tpList.VisibleChanged += new System.EventHandler(this.tpList_VisibleChanged);
            // 
            // lstList
            // 
            this.lstList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.lstList.DataMember = "";
            this.lstList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstList.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.lstList.Location = new System.Drawing.Point(0, 52);
            this.lstList.Name = "lstList";
            this.lstList.PreferredRowHeight = 20;
            this.lstList.ReadOnly = true;
            this.lstList.RowHeaderWidth = 16;
            this.lstList.Size = new System.Drawing.Size(549, 331);
            this.lstList.TabIndex = 196;
            this.lstList.TableStyles.AddRange(new System.Windows.Forms.DataGridTableStyle[] {
            this.lstStyle});
            this.lstList.CurrentCellChanged += new System.EventHandler(this.lstList_CurrentCellChanged);
            this.lstList.TextChanged += new System.EventHandler(this.lstList_TextChanged);
            this.lstList.DoubleClick += new System.EventHandler(this.lstList_DoubleClick);
            this.lstList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lstList_KeyPress);
            this.lstList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lstList_MouseDown);
            // 
            // lstStyle
            // 
            this.lstStyle.DataGrid = this.lstList;
            this.lstStyle.GridColumnStyles.AddRange(new System.Windows.Forms.DataGridColumnStyle[] {
            this.dgcNone,
            this.dgcCode,
            this.dgcDesc});
            this.lstStyle.GridLineColor = System.Drawing.SystemColors.Window;
            this.lstStyle.GridLineStyle = System.Windows.Forms.DataGridLineStyle.None;
            this.lstStyle.HeaderForeColor = System.Drawing.SystemColors.ControlText;
            this.lstStyle.PreferredRowHeight = 20;
            this.lstStyle.ReadOnly = true;
            this.lstStyle.RowHeadersVisible = false;
            // 
            // dgcNone
            // 
            this.dgcNone.Format = "";
            this.dgcNone.FormatInfo = null;
            this.dgcNone.ReadOnly = true;
            this.dgcNone.Width = 0;
            // 
            // dgcCode
            // 
            this.dgcCode.Format = "";
            this.dgcCode.FormatInfo = null;
            this.dgcCode.HeaderText = "Code";
            this.dgcCode.ImageColumn = "";
            this.dgcCode.ImageIndex = -1;
            this.dgcCode.ImageList = null;
            this.BresourceLookup1.SetLookup(this.dgcCode, new FWBS.OMS.UI.Windows.ResourceLookupItem("Code", "Code", ""));
            this.dgcCode.ReadOnly = true;
            this.dgcCode.Width = 88;
            // 
            // dgcDesc
            // 
            this.dgcDesc.Format = "";
            this.dgcDesc.FormatInfo = null;
            this.dgcDesc.HeaderText = "Description";
            this.BresourceLookup1.SetLookup(this.dgcDesc, new FWBS.OMS.UI.Windows.ResourceLookupItem("Description", "Description", ""));
            this.dgcDesc.ReadOnly = true;
            this.dgcDesc.Width = 300;
            // 
            // pnlQuickSearch
            // 
            this.pnlQuickSearch.Controls.Add(this.txtSearch);
            this.pnlQuickSearch.Controls.Add(this.label1);
            this.pnlQuickSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlQuickSearch.Location = new System.Drawing.Point(0, 26);
            this.pnlQuickSearch.Name = "pnlQuickSearch";
            this.pnlQuickSearch.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.pnlQuickSearch.Size = new System.Drawing.Size(549, 26);
            this.pnlQuickSearch.TabIndex = 197;
            // 
            // txtSearch
            // 
            this.txtSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearch.Location = new System.Drawing.Point(48, 0);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(501, 23);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search : ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlListbarContainer
            // 
            this.pnlListbarContainer.Controls.Add(this.tbcLists);
            this.pnlListbarContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlListbarContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlListbarContainer.Name = "pnlListbarContainer";
            this.pnlListbarContainer.Size = new System.Drawing.Size(549, 26);
            this.pnlListbarContainer.TabIndex = 198;
            // 
            // tbcLists
            // 
            this.tbcLists.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbcLists.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbNew,
            this.tbEdit,
            this.tbDelete,
            this.tbClone,
            this.tbSp1,
            this.tbShowActive,
            this.tbShowTrash,
            this.tbSp2,
            this.tbRestore});
            this.tbcLists.Divider = false;
            this.tbcLists.DropDownArrows = true;
            this.tbcLists.Location = new System.Drawing.Point(0, 0);
            this.tbcLists.Name = "tbcLists";
            this.tbcLists.ShowToolTips = true;
            this.tbcLists.Size = new System.Drawing.Size(549, 26);
            this.tbcLists.TabIndex = 195;
            this.tbcLists.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbcLists_ButtonClick);
            // 
            // tbNew
            // 
            this.tbNew.ImageIndex = 0;
            this.BresourceLookup1.SetLookup(this.tbNew, new FWBS.OMS.UI.Windows.ResourceLookupItem("New", "New", ""));
            this.tbNew.Name = "tbNew";
            // 
            // tbEdit
            // 
            this.tbEdit.ImageIndex = 24;
            this.BresourceLookup1.SetLookup(this.tbEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tbEdit.Name = "tbEdit";
            // 
            // tbDelete
            // 
            this.tbDelete.ImageIndex = 6;
            this.BresourceLookup1.SetLookup(this.tbDelete, new FWBS.OMS.UI.Windows.ResourceLookupItem("Delete", "Delete", ""));
            this.tbDelete.Name = "tbDelete";
            // 
            // tbClone
            // 
            this.tbClone.ImageIndex = 4;
            this.BresourceLookup1.SetLookup(this.tbClone, new FWBS.OMS.UI.Windows.ResourceLookupItem("Clone", "Clone", ""));
            this.tbClone.Name = "tbClone";
            // 
            // tbSp1
            // 
            this.tbSp1.Name = "tbSp1";
            this.tbSp1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            this.tbSp1.Visible = false;
            // 
            // tbShowActive
            // 
            this.tbShowActive.ImageIndex = 19;
            this.BresourceLookup1.SetLookup(this.tbShowActive, new FWBS.OMS.UI.Windows.ResourceLookupItem("ShowActive", "Show Active", ""));
            this.tbShowActive.Name = "tbShowActive";
            this.tbShowActive.Pushed = true;
            this.tbShowActive.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbShowActive.Visible = false;
            // 
            // tbShowTrash
            // 
            this.tbShowTrash.ImageIndex = 22;
            this.BresourceLookup1.SetLookup(this.tbShowTrash, new FWBS.OMS.UI.Windows.ResourceLookupItem("ShowTrash", "Show Trash", ""));
            this.tbShowTrash.Name = "tbShowTrash";
            this.tbShowTrash.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            this.tbShowTrash.Visible = false;
            // 
            // tbSp2
            // 
            this.tbSp2.Enabled = false;
            this.tbSp2.Name = "tbSp2";
            this.tbSp2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            this.tbSp2.Visible = false;
            // 
            // tbRestore
            // 
            this.tbRestore.Enabled = false;
            this.tbRestore.ImageIndex = 8;
            this.BresourceLookup1.SetLookup(this.tbRestore, new FWBS.OMS.UI.Windows.ResourceLookupItem("Restore", "Restore", ""));
            this.tbRestore.Name = "tbRestore";
            this.tbRestore.Visible = false;
            // 
            // tpEdit
            // 
            this.tpEdit.BackColor = System.Drawing.Color.White;
            this.tpEdit.Controls.Add(this.pnlEdit);
            this.tpEdit.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tpEdit.Location = new System.Drawing.Point(4, 5);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Name = "tpEdit";
            this.tpEdit.Size = new System.Drawing.Size(549, 383);
            this.tpEdit.TabIndex = 1;
            this.tpEdit.Text = "Edit";
            this.tpEdit.VisibleChanged += new System.EventHandler(this.tpEdit_VisibleChanged);
            // 
            // pnlEdit
            // 
            this.pnlEdit.BackColor = System.Drawing.Color.White;
            this.pnlEdit.Controls.Add(this.labSelectedObject);
            this.pnlEdit.Controls.Add(this.pnlToolbarContainer);
            this.pnlEdit.Controls.Add(this.btnBlue);
            this.pnlEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEdit.Location = new System.Drawing.Point(0, 0);
            this.pnlEdit.Name = "pnlEdit";
            this.pnlEdit.Size = new System.Drawing.Size(549, 50);
            this.pnlEdit.TabIndex = 0;
            // 
            // labSelectedObject
            // 
            this.labSelectedObject.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.labSelectedObject.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labSelectedObject.Dock = System.Windows.Forms.DockStyle.Top;
            this.labSelectedObject.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labSelectedObject.ForeColor = System.Drawing.SystemColors.Window;
            this.labSelectedObject.Location = new System.Drawing.Point(0, 26);
            this.labSelectedObject.Name = "labSelectedObject";
            this.labSelectedObject.Size = new System.Drawing.Size(549, 22);
            this.labSelectedObject.TabIndex = 200;
            this.labSelectedObject.Text = "%1% - Select Object";
            this.labSelectedObject.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlToolbarContainer
            // 
            this.pnlToolbarContainer.Controls.Add(this.tbcEdit);
            this.pnlToolbarContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlToolbarContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlToolbarContainer.Name = "pnlToolbarContainer";
            this.pnlToolbarContainer.Size = new System.Drawing.Size(549, 26);
            this.pnlToolbarContainer.TabIndex = 203;
            // 
            // tbcEdit
            // 
            this.tbcEdit.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbcEdit.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbSave,
            this.tbClose,
            this.tbReturn});
            this.tbcEdit.Divider = false;
            this.tbcEdit.DropDownArrows = true;
            this.tbcEdit.Location = new System.Drawing.Point(0, 0);
            this.tbcEdit.Name = "tbcEdit";
            this.tbcEdit.ShowToolTips = true;
            this.tbcEdit.Size = new System.Drawing.Size(549, 26);
            this.tbcEdit.TabIndex = 199;
            this.tbcEdit.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.tbcEdit.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbcEdit_ButtonClick);
            // 
            // tbSave
            // 
            this.tbSave.ImageIndex = 2;
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            this.tbSave.Name = "tbSave";
            // 
            // tbClose
            // 
            this.tbClose.ImageIndex = 35;
            this.BresourceLookup1.SetLookup(this.tbClose, new FWBS.OMS.UI.Windows.ResourceLookupItem("Close", "Close", ""));
            this.tbClose.Name = "tbClose";
            // 
            // tbReturn
            // 
            this.tbReturn.ImageIndex = 35;
            this.BresourceLookup1.SetLookup(this.tbReturn, new FWBS.OMS.UI.Windows.ResourceLookupItem("Return", "Return", ""));
            this.tbReturn.Name = "tbReturn";
            this.tbReturn.Visible = false;
            // 
            // btnBlue
            // 
            this.btnBlue.Location = new System.Drawing.Point(543, 30);
            this.btnBlue.Name = "btnBlue";
            this.btnBlue.Size = new System.Drawing.Size(1, 1);
            this.btnBlue.TabIndex = 202;
            // 
            // ExportDlg
            // 
            this.ExportDlg.Description = "Export Path to store the OMS XML Object";
            // 
            // ucEditBase
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tpEdit);
            this.Controls.Add(this.tpList);
            this.Name = "ucEditBase";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Size = new System.Drawing.Size(618, 425);
            this.tpList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lstList)).EndInit();
            this.pnlQuickSearch.ResumeLayout(false);
            this.pnlQuickSearch.PerformLayout();
            this.pnlListbarContainer.ResumeLayout(false);
            this.pnlListbarContainer.PerformLayout();
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		protected virtual void tbcLists_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == tbNew)
			{
				NewData();
				//DMB 19/2/2004 added as an attempt to display newly added data to the list
				dtCT = GetData();
				lstList.DataSource = dtCT;
				lstList.Refresh();
				txtSearch_TextChanged(sender,new EventArgs());
			}
			else if (e.Button == tbEdit && tbEdit.Visible && lstList.CurrentRowIndex > -1)
			{
				int y = lstList.CurrentRowIndex;
				if (y != -1)
					LoadSingleItem(Convert.ToString(dtCT.DefaultView[y][SelectCode]));
			}
			else if (e.Button == tbDelete && lstList.CurrentRowIndex > -1)
			{
				int y = lstList.CurrentRowIndex;
				if (y != -1)
				{
					string code = Convert.ToString(dtCT.DefaultView[y][SelectCode]);
					if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("AreYouSure","Are you sure you wish to Delete [%1%]?",code) == DialogResult.Yes)
					{
						DeleteData(code);
					}
					dtCT = GetData();
					lstList.DataSource = dtCT;
					lstList.Refresh();
					txtSearch_TextChanged(sender,new EventArgs());
				}
			}
			else if (e.Button == tbShowActive)
			{
				tbShowActive.Pushed=true;
				tbShowTrash.Pushed=false;
				tbRestore.Enabled=false;
				tbDelete.Enabled=true;
				lstList.Text = "Refresh";
			}
			else if (e.Button == tbShowTrash)
			{
				tbShowActive.Pushed=false;
				tbShowTrash.Pushed=true;
				tbRestore.Enabled=true;
				tbDelete.Enabled=false;
				lstList.Text = "Refresh";
			}
			else if (e.Button == tbRestore)
			{
				int y = lstList.CurrentRowIndex;
				if (y ==-1) return;
				string Code = Convert.ToString(dtCT.DefaultView[y][SelectCode]);
				Restore(Code);
				lstList.Text = "Refresh";
			}
			else if (e.Button == tbClone)
			{
				int y = lstList.CurrentRowIndex;
				if (y ==-1) return;
				string Code = Convert.ToString(dtCT.DefaultView[y][SelectCode]);
				Clone(Code);
				lstList.Text = "Refresh";
			}
		}

		public new void Refresh()
		{
			lstList.Text = "Refresh";
		}

		protected virtual void ShowEditor()
		{
            tpEdit.Visible = true;
            tpList.Visible = false;
            HostingTab.Text = _currenttype;
		}

        protected virtual void ShowList()
		{
            tpList.Visible = true;
            tpEdit.Visible = false;
            this.Refresh();
            lasttype = null;
		}

		protected virtual void tbcEdit_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			btnBlue.Focus();
			while(!btnBlue.Focused)
			{
				Application.DoEvents();
				btnBlue.Focus();
				Application.DoEvents();
			}
			if (e.Button == tbSave)
			{
				SaveChanges();
			}
			if (e.Button == tbReturn)
			{
				SaveAndClose();
			}
            if (e.Button == tbClose)
            {
                CloseAndReturnToList();
                //ShowList();
            }
		}

		public bool ListMode
		{
			get
			{
                return this.tpList.Visible;
			}
		}
		
		public void SaveAndClose()
		{
			if (IsObjectDirty())
			{
				dtCT = GetData();
				lstList.DataSource = dtCT;
				lstList.Refresh();
				txtSearch_TextChanged(this,new EventArgs());
				ShowList();
			}
		}

		private void tpEdit_VisibleChanged(object sender, System.EventArgs e)
		{
			pnlEdit.SendToBack();
		}

		private void tpList_VisibleChanged(object sender, System.EventArgs e)
		{
			if (!DesignMode)
			{
				if (lstList.DataSource != null && dtCT != null && dtCT.Rows.Count > 0) 
				{
					try
					{
                        if (lstList.VisibleRowCount > 0)
                        {
                            lstList.Select(0);
                            lstList.CurrentRowIndex = 0;
                        }
					}
					catch{}
				}
				if (txtSearch.Visible) txtSearch.Focus();
			}
		}

		protected void txtSearch_TextChanged(object sender, System.EventArgs e)
		{
			if (!DesignMode)
			{
				if (lstList.DataSource is System.Data.DataTable)
				{
                    if (lstList.CurrentRowIndex > -1)
                    {
                        try { lstList.UnSelect(lstList.CurrentRowIndex); }
                        catch { }
                    }
					try
					{
						DataTable n = (DataTable)lstList.DataSource;
						string _where = "";
						foreach (System.Windows.Forms.DataGridColumnStyle dgcs in CurrentTableStyle.GridColumnStyles)
						{				
							try
							{
								if (dgcs.MappingName != "" && dtCT.Columns.Contains(dgcs.MappingName) && dtCT.Columns[dgcs.MappingName].DataType == typeof(System.String))
									_where = _where + dgcs.MappingName + " Like '" + txtSearch.Text.Replace("'","''").Replace("[","").Replace("]","") + "%' or ";
							}
							catch
							{}
						}
						_where = _where.Substring(0,_where.Length-4);
						n.DefaultView.RowFilter = _where;
						lstList.CurrentRowIndex =0;
                        if (lstList.CurrentRowIndex > -1 && lstList.CurrentRowIndex < lstList.VisibleRowCount)
						    lstList.Select(0);
					}
					catch
					{}
				}
			}
		}

		private void txtSearch_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Up)
			{
				if (lstList.CurrentRowIndex > 0) 
				{
					lstList.UnSelect(lstList.CurrentRowIndex);
					lstList.CurrentRowIndex--;
				}
				e.Handled=true;
			}
			if (e.KeyCode == Keys.Down)
			{
				if (lstList.CurrentRowIndex < ((DataTable)lstList.DataSource).DefaultView.Count-1) 
				{
					lstList.UnSelect(lstList.CurrentRowIndex);
					lstList.CurrentRowIndex++;
				}
				e.Handled=true;
			}
			else if (e.KeyCode == Keys.Return && tbEdit.Enabled)
				tbcLists_ButtonClick(sender,new ToolBarButtonClickEventArgs(tbEdit));
		}

		private void lstList_TextChanged(object sender, System.EventArgs e)
		{
			if (lstList.Text != null)
			{
				dtCT = GetData();
				lstList.DataSource = dtCT;
				lstList.Refresh();
				lstList.Text = null;
				txtSearch_TextChanged(sender,new EventArgs());
			}
		}

		private void lstList_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar >= 21)
			{
				if (txtSearch.Visible) txtSearch.Focus();
				txtSearch.Text = e.KeyChar.ToString();
				txtSearch.SelectionStart = 1;
			}
			else if (e.KeyChar == 13)
			{
				tbcLists_ButtonClick(sender,new ToolBarButtonClickEventArgs(tbEdit));
			}
		}

		public DataGridTableStyle CurrentTableStyle
		{
			get
			{
				DataGridTableStyle retsty = new DataGridTableStyle();
				if (lstList.DataSource is DataTable)
				{
					foreach (DataGridTableStyle sty in lstList.TableStyles)
						if (sty.MappingName == ((DataTable)lstList.DataSource).TableName)
						{
							retsty = sty;
							break;
						}
				}
				return retsty;
			}
		}

        protected virtual void CloseAndReturnToList()
        {

        }

        protected virtual void CurrentCellChanged()
        {

        }
    }
}
