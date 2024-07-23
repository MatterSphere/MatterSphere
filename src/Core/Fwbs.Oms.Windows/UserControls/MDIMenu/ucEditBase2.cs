using System;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.MDIMenu.V2Host.TreeView_Navigation;
using Infragistics.Win.UltraWinTabControl;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucEditBase.
    /// </summary>
    public class ucEditBase2 : System.Windows.Forms.UserControl, IOBjectDirty, IObjectUpdate, ITabControl
    {
		public System.Windows.Forms.Panel tpList;
		public System.Windows.Forms.Panel tpEdit;
		private System.ComponentModel.IContainer components;

		private bool _isdirty = false;
		protected System.Windows.Forms.Panel pnlEdit;
		protected System.Windows.Forms.Label labSelectedObject;
		protected FWBS.Common.UI.Windows.ToolBar tbcEdit;
		protected System.Windows.Forms.ToolBarButton tbSave;
		protected System.Windows.Forms.ToolBarButton tbClose;
		protected System.Windows.Forms.ToolBarButton tbReturn;
		protected DataTable dtCT = null;
		protected FWBS.OMS.UI.Windows.ResourceLookup BresourceLookup1;
		private IMainParent _mainparent = null;
		protected System.Windows.Forms.FolderBrowserDialog ExportDlg;
		protected System.Windows.Forms.Button btnBlue;
		protected FWBS.OMS.UI.Windows.ucSearchControl lstList;
		private Control _editparent = null;
		private OMSToolBarButton tbClone;
		private OMSToolBarButton tbDelete;
		private OMSToolBarButton tbEdit;
		private OMSToolBarButton tbNew;
		private OMSToolBarButton tbRestore;
		private OMSToolBarButton tbShowActive;
		private OMSToolBarButton tbShowTrash;
        protected Panel pnlToolbarContainer;
		private FWBS.Common.KeyValueCollection _params;
		private int _lastPos = 0;
        private readonly string _modifiedLookup = "[Modified]";

		protected bool cloning;

		public event EventHandler Dirty;

		// Invoke the Dirty event; when changed
		protected virtual void OnDirty(EventArgs e) 
		{
			if (Dirty != null)
				Dirty(this, e);
		}
		
		public ucEditBase2()
		{
			InitializeComponent();
            this.tbcEdit.ImageList = FWBS.OMS.UI.Windows.Images.Windows8();
		}
		
		public ucEditBase2(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : this()
		{
			Initialise(mainparent,editparent, Params);
            _modifiedLookup = $"[{Session.CurrentSession.Resources.GetResource("MODIFIED", "Modified", "").Text}]";
        }

		public void Initialise(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params)
		{
			_mainparent = mainparent;
			_editparent = editparent;
			_params = Params;

			this.tpList.Dock = DockStyle.Fill;
			this.tpEdit.Dock = DockStyle.Fill;
            this.Dirty -= new System.EventHandler(this.ucEditBack1_Dirty);
            this.Dirty += new System.EventHandler(this.ucEditBack1_Dirty);

			if (Session.CurrentSession.IsLoggedIn)
			{
				Text = Session.CurrentSession.Resources.GetResource("ADMIN","OMS Administration","").Text;
				tbClone = lstList.GetOMSToolBarButton("cmdClone");
				tbDelete = lstList.GetOMSToolBarButton("cmdDelete");
				tbEdit = lstList.GetOMSToolBarButton("cmdEdit");
				tbNew = lstList.GetOMSToolBarButton("cmdNew");
				tbRestore = lstList.GetOMSToolBarButton("cmdRestore");
				tbShowActive = lstList.GetOMSToolBarButton("cmdActive");
				if (tbShowActive != null) tbShowActive.Style = ToolBarButtonStyle.ToggleButton;
				tbShowTrash = lstList.GetOMSToolBarButton("cmdTrash");
				if (tbShowTrash != null) tbShowTrash.Style = ToolBarButtonStyle.ToggleButton;
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

		public virtual bool IsDirty
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
					labSelectedObject.Text = labSelectedObject.Text.Replace($"{_modifiedLookup} - ","");
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


		private void ucEditBack1_Dirty(object sender, System.EventArgs e)
		{
			if (_isdirty == false)
			{
				labSelectedObject.Text = labSelectedObject.Text.Replace($"{_modifiedLookup} - ", "");
                labSelectedObject.Text = $"{_modifiedLookup} - {labSelectedObject.Text}";
            }
			_isdirty=true;
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

		protected virtual string SearchListName
		{
			get
			{
				if (!DesignMode)
					FWBS.OMS.UI.Windows.MessageBox.Show("You must implement a Override for the SearchListName Property","ucEditBase",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				return null;
			}
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

		protected void SaveChanges()
		{
			try
			{
				if (UpdateData())
				{
					IsDirty = false;
				}
			}
			catch(Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
				return;
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
					if (IsDirty) 
						return false;
				}
				if (dr == DialogResult.No)
				{
					IsDirty = false;
					CancelData();
					CheckForCloning();
					return false;
				}
				if (dr == DialogResult.Cancel) 
					return false;
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
					cloning = false;
					SaveChanges();
					if (IsDirty)
						return DialogResult.Yes;
				}
				if (dr == DialogResult.No)
				{
					IsDirty = false;
					CancelData();
					CheckForCloning();
					return DialogResult.No;
				}
				if (dr == DialogResult.Cancel) return DialogResult.Cancel;
			}
			return null;
		}


		public virtual void DeleteDescriptionCodeLookup()
		{ }


		private void CheckForCloning()
		{
			if (cloning)
			{
				cloning = false;
				DeleteDescriptionCodeLookup();
			}
		}

		public virtual bool CheckForValidDescription()
		{
			return true;
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
            this.lstList = new FWBS.OMS.UI.Windows.ucSearchControl();
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
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpList
            // 
            this.tpList.Controls.Add(this.lstList);
            this.tpList.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tpList.Location = new System.Drawing.Point(56, 31);
            this.tpList.Name = "tpList";
            this.tpList.Size = new System.Drawing.Size(549, 383);
            this.tpList.TabIndex = 0;
            this.tpList.Text = "List";
            // 
            // lstList
            // 
            this.lstList.BackColor = System.Drawing.Color.White;
            this.lstList.BackGroundColor = System.Drawing.SystemColors.Window;
            this.lstList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstList.DoubleClickAction = "None";
            this.lstList.GraphicalPanelVisible = true;
            this.lstList.Location = new System.Drawing.Point(0, 0);
            this.lstList.Name = "lstList";
            this.lstList.NavCommandPanel = null;
            this.lstList.Padding = new System.Windows.Forms.Padding(5);
            this.lstList.RefreshOnEnquiryFormRefreshEvent = false;
            this.lstList.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.lstList.SearchListCode = "";
            this.lstList.SearchListType = "";
            this.lstList.SearchPanelVisible = false;
            this.lstList.Size = new System.Drawing.Size(549, 383);
            this.lstList.TabIndex = 200;
            this.lstList.ToBeRefreshed = false;
            this.lstList.TypeSelectorVisible = false;
            this.lstList.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.lstList_SearchButtonCommands);
            this.lstList.SearchCompleted += new FWBS.OMS.UI.Windows.SearchCompletedEventHandler(this.lstList_SearchCompleted);
            // 
            // tpEdit
            // 
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
            this.tbcEdit.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbcEdit_ButtonClick);
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            this.tbSave.Name = "tbSave";
            // 
            // tbClose
            // 
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
            // ucEditBase2
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tpEdit);
            this.Controls.Add(this.tpList);
            this.Name = "ucEditBase2";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Size = new System.Drawing.Size(618, 425);
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.ResumeLayout(false);

		}
		#endregion

		public new void Load()
		{
			lstList.SetSearchListType(this.SearchListName,null,_params);
			if (lstList.QuickFilterContol != null && lstList.QuickFilterContol.Visible)
				lstList.QuickFilterContol.Focus();
		}
		
		public new void Refresh()
		{
			lstList.Search();
		}

		protected virtual void ShowEditor(bool newObject = false)
		{
			_lastPos = lstList.dgSearchResults.CurrentRowIndex;
			tpEdit.Visible = true;
			tpList.Visible = false;
			string code = GetCode();
			string username = ReturnUserNameForUserRelatedSearchLists();
			if (!string.IsNullOrWhiteSpace(username))
				code = username;

			if (OriginalTabText == code)
				HostingTab.Text = OriginalTabText;
			else
			{
				if (newObject)
					HostingTab.Text = OriginalTabText;
				else
				{
					if (cloning)
					{
						HostingTab.Text = string.Format("{0} - (cloning) {1}", Session.CurrentSession.Terminology.Parse(OriginalTabText, true), code);
					}
					else
						HostingTab.Text = string.Format("{0} - {1}", Session.CurrentSession.Terminology.Parse(OriginalTabText, true), code);
				}
			}
		}

		protected virtual void ShowList()
		{
			tpList.Visible = true;
			tpEdit.Visible = false;
			HostingTab.Text = Session.CurrentSession.Terminology.Parse(OriginalTabText, true);
			this.Refresh();
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
			}
		}

		protected virtual void CloseAndReturnToList()
		{

		}

		public virtual bool ListMode
		{
			get
			{
				return (this.EditParent != null && this.EditParent.Controls[0] == tpList);
			}
		}
		
		public void SaveAndClose()
		{
			if (IsObjectDirty())
			{
				lstList.Search();
				ShowList();
			}
		}

		private void tpEdit_VisibleChanged(object sender, System.EventArgs e)
		{
			pnlEdit.SendToBack();
		}

		protected void lstList_SearchButtonCommands(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
		{
			if (e.ButtonName == "cmdAdd")
			{
				NewData();
			}
			else if (e.ButtonName == "cmdEdit")
			{
				LoadSingleItem(GetCode());
			}
			else if (e.ButtonName == "cmdDelete")
			{
				string code = GetCode();
				if (!CheckLockStateOfObject(code, sender))
				{
					if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("AUSGENERIC", "Are you sure you wish to Delete ?", code) == DialogResult.Yes)
					{
						DeleteData(code);
						lstList.Search();
					}
				}
			}
			else if (e.ButtonName == "cmdShowActive")
			{
				tbShowActive.Pushed=true;
				tbShowTrash.Pushed=false;
				tbRestore.Enabled=false;
				tbDelete.Enabled=true;
				lstList.Search();
			}
			else if (e.ButtonName == "cmdShowTrash")
			{
				tbShowActive.Pushed=false;
				tbShowTrash.Pushed=true;
				tbRestore.Enabled=true;
				tbDelete.Enabled=false;
				lstList.Search();
			}
			else if (e.ButtonName == "cmdRestore")
			{
				string code = GetCode();
				Restore(code);
				lstList.Search();
			}
			else if (e.ButtonName == "cmdClone")
			{
				string code = GetCode();
				Clone(code);
				lstList.Search();
			}
		}

		private bool CheckLockStateOfObject(string code, object sender)
		{
			bool result = false;
			if (FWBS.OMS.Session.CurrentSession.ObjectLocking)
			{
				LockState ls = new LockState();
				LockableObjects lockabletype = GetCurrentObjectType(sender);
				if (lockabletype != LockableObjects.None)
				{
					if (ls.CheckObjectLockState(code, lockabletype))
						return true;
				}
			}
			return result;
		}

		private LockableObjects GetCurrentObjectType(object sender)
		{
			ucSearchControl sc = sender as ucSearchControl;
			switch (sc.SearchList.Code)
			{
				case "ADMSCRIPTS":
					return LockableObjects.Script;
				case "ADMDATALISTS":
					return LockableObjects.DataList;
				case "ADMSEARCHLISTS":
					return LockableObjects.SearchList;
				default:
					return LockableObjects.None;
			}
		}

		protected string GetCode()
		{
			string result = "";
            var currentItem = lstList.CurrentItem();
            if (currentItem != null)
			{
				for (int i = 0; i < currentItem.Count; i++)
					result += Convert.ToString(currentItem[i].Value) + "|";
			}
			else
				result = OriginalTabText;
			return result.Trim('|');
		}

		private string ReturnUserNameForUserRelatedSearchLists()
		{
			string result = "";
			if (lstList.SearchList != null)
			{
				if (lstList.SearchList.Code == "ADMFEELIST" || lstList.SearchList.Code == "ADMUSERS")
				{
					FWBS.OMS.User u = FWBS.OMS.User.GetUser(Convert.ToInt32(lstList.CurrentItem()[0].Value));
					result += u.FullName;
				}
			}
            return result;
		}

  
		private void lstList_SearchCompleted(object sender, FWBS.OMS.UI.Windows.SearchCompletedEventArgs e)
		{
            if (_lastPos > -1 && _lastPos < e.Count)
            {
                lstList.dgSearchResults.CurrentRowIndex = _lastPos;
            }
        }

	    public DialogResult? CheckDialogResult()
	    {
	        return IsObjectDirtyDialogResult();
	    }
    }
}
