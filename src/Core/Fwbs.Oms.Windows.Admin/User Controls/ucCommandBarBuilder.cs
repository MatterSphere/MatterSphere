using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucCommandBarBuilder.
    /// </summary>
    public class ucCommandBarBuilder : FWBS.OMS.UI.Windows.Admin.ucEditBase2
	{
		#region Fields
		private System.Windows.Forms.Panel pnlMainBack;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private FWBS.OMS.CommandBarBuilder _currentobj = null;
		private System.Windows.Forms.ToolBarButton tbSp3;
		private System.Windows.Forms.ToolBarButton tbAddMenuItem;
		private System.Windows.Forms.ToolBarButton tbAddMainMenu;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.ToolStrip toolBar1;
		private System.Windows.Forms.Panel panel1;
		private ucMenuContainer SelectedMenuContainer = null;
		private ucMenuItem SelectedMenuItem = null;
		private System.Windows.Forms.ToolBarButton tbAddSubMenu;
		private System.Windows.Forms.Panel pnlApp;
		private FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup cmbApplication;

		private ArrayList MainMenus = new ArrayList();
		private ArrayList MenuCommands = new ArrayList();
		private new ArrayList Select = new ArrayList();
		
        private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ToolBarButton tbDelete;
		private System.Windows.Forms.ToolBarButton tbSelect;
		private System.Windows.Forms.Button btnChange;
		private System.Windows.Forms.ToolBarButton tbMove;
		private System.Windows.Forms.ContextMenu cntxMoveTo;
		private System.Windows.Forms.MenuItem mnuMoveAbove;
		private System.Windows.Forms.MenuItem mnuMoveBelow;
		private System.Windows.Forms.ToolBarButton tbSp4;
		private System.Windows.Forms.ToolBarButton tbSp5;
		private System.Windows.Forms.ToolBarButton tbMoveLeft;
		private System.Windows.Forms.ToolBarButton tbMoveRight;
		private string _code = "";

		#endregion

		#region Constructors
		public ucCommandBarBuilder()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public ucCommandBarBuilder(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : base(mainparent,editparent, Params)
		{
			if (frmMain.PartnerAccess == false)
				Session.CurrentSession.ValidateLicensedFor("SDKALL");
			// This call is required by the Windows Form Designer.
            FWBS.OMS.Session.CurrentSession.Resources.GetResource("CMDSHOWALL", "(No Filter)", "");
            InitializeComponent();
			tpEdit.Text = FWBS.OMS.Session.CurrentSession.Resources.GetResource("CmdBarBuilder","Menu Builder","").Text;
			tpList.Text = tpEdit.Text;
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

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.pnlMainBack = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolBar1 = new System.Windows.Forms.ToolStrip();
            this.panel2 = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tbSp3 = new System.Windows.Forms.ToolBarButton();
            this.tbAddMenuItem = new System.Windows.Forms.ToolBarButton();
            this.tbAddMainMenu = new System.Windows.Forms.ToolBarButton();
            this.tbAddSubMenu = new System.Windows.Forms.ToolBarButton();
            this.pnlApp = new System.Windows.Forms.Panel();
            this.btnChange = new System.Windows.Forms.Button();
            this.cmbApplication = new FWBS.OMS.UI.Windows.eXPComboBoxCodeLookup();
            this.tbDelete = new System.Windows.Forms.ToolBarButton();
            this.tbSp4 = new System.Windows.Forms.ToolBarButton();
            this.tbMove = new System.Windows.Forms.ToolBarButton();
            this.cntxMoveTo = new System.Windows.Forms.ContextMenu();
            this.mnuMoveAbove = new System.Windows.Forms.MenuItem();
            this.mnuMoveBelow = new System.Windows.Forms.MenuItem();
            this.tbSelect = new System.Windows.Forms.ToolBarButton();
            this.tbSp5 = new System.Windows.Forms.ToolBarButton();
            this.tbMoveLeft = new System.Windows.Forms.ToolBarButton();
            this.tbMoveRight = new System.Windows.Forms.ToolBarButton();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.pnlMainBack.SuspendLayout();
            this.panel1.SuspendLayout();
            this.pnlApp.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.pnlMainBack);
            this.tpEdit.Controls.Add(this.pnlApp);
            this.tpEdit.Controls.Add(this.splitter1);
            this.tpEdit.Controls.Add(this.propertyGrid1);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Size = new System.Drawing.Size(764, 383);
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.propertyGrid1, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlApp, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlMainBack, 0);
            // 
            // pnlEdit
            // 
            this.pnlEdit.Size = new System.Drawing.Size(764, 58);
            // 
            // labSelectedObject
            // 
            this.labSelectedObject.Location = new System.Drawing.Point(0, 35);
            this.labSelectedObject.Size = new System.Drawing.Size(764, 22);
            // 
            // tbcEdit
            // 
            this.tbcEdit.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbSp3,
            this.tbAddMainMenu,
            this.tbAddMenuItem,
            this.tbAddSubMenu,
            this.tbDelete,
            this.tbSp4,
            this.tbSelect,
            this.tbMove,
            this.tbSp5,
            this.tbMoveLeft,
            this.tbMoveRight});
            this.tbcEdit.Size = new System.Drawing.Size(764, 35);
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
            // pnlToolbarContainer
            // 
            this.pnlToolbarContainer.Size = new System.Drawing.Size(764, 35);
            // 
            // pnlMainBack
            // 
            this.pnlMainBack.AutoScroll = true;
            this.pnlMainBack.BackColor = System.Drawing.Color.White;
            this.pnlMainBack.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlMainBack.Controls.Add(this.panel1);
            this.pnlMainBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainBack.Location = new System.Drawing.Point(0, 89);
            this.pnlMainBack.Name = "pnlMainBack";
            this.pnlMainBack.Padding = new System.Windows.Forms.Padding(8);
            this.pnlMainBack.Size = new System.Drawing.Size(545, 294);
            this.pnlMainBack.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.toolBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(8, 8);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 1, 0, 1);
            this.panel1.Size = new System.Drawing.Size(525, 32);
            this.panel1.TabIndex = 1;
            // 
            // toolBar1
            // 
            this.toolBar1.AutoSize = false;
            this.toolBar1.BackColor = System.Drawing.SystemColors.Window;
            this.toolBar1.CanOverflow = false;
            this.toolBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolBar1.Location = new System.Drawing.Point(0, 1);
            this.toolBar1.Name = "toolBar1";
            this.toolBar1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolBar1.Size = new System.Drawing.Size(525, 30);
            this.toolBar1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(25, 25);
            this.panel2.TabIndex = 2;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Right;
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.White;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(548, 58);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(216, 325);
            this.propertyGrid1.TabIndex = 3;
            this.propertyGrid1.ToolbarVisible = false;
            this.propertyGrid1.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid1_PropertyValueChanged);
            this.propertyGrid1.SelectedObjectsChanged += new System.EventHandler(this.propertyGrid1_SelectedObjectsChanged);
            // 
            // tbSp3
            // 
            this.tbSp3.Name = "tbSp3";
            this.tbSp3.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbAddMenuItem
            // 
            this.tbAddMenuItem.ImageIndex = 20;
            this.BresourceLookup1.SetLookup(this.tbAddMenuItem, new FWBS.OMS.UI.Windows.ResourceLookupItem("AddMenuItem", "Add Menu Item", ""));
            this.tbAddMenuItem.Name = "tbAddMenuItem";
            this.tbAddMenuItem.Text = "Add Menu Item";
            // 
            // tbAddMainMenu
            // 
            this.tbAddMainMenu.ImageIndex = 19;
            this.BresourceLookup1.SetLookup(this.tbAddMainMenu, new FWBS.OMS.UI.Windows.ResourceLookupItem("AddMainMenu", "Add Main Menu", ""));
            this.tbAddMainMenu.Name = "tbAddMainMenu";
            this.tbAddMainMenu.Text = "Add Main Menu";
            // 
            // tbAddSubMenu
            // 
            this.tbAddSubMenu.ImageIndex = 46;
            this.BresourceLookup1.SetLookup(this.tbAddSubMenu, new FWBS.OMS.UI.Windows.ResourceLookupItem("AddSubMenu", "Add Sub Menu", ""));
            this.tbAddSubMenu.Name = "tbAddSubMenu";
            this.tbAddSubMenu.Text = "Add Sub Menu";
            // 
            // pnlApp
            // 
            this.pnlApp.Controls.Add(this.btnChange);
            this.pnlApp.Controls.Add(this.cmbApplication);
            this.pnlApp.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlApp.Location = new System.Drawing.Point(0, 58);
            this.pnlApp.Name = "pnlApp";
            this.pnlApp.Padding = new System.Windows.Forms.Padding(0, 4, 0, 4);
            this.pnlApp.Size = new System.Drawing.Size(545, 31);
            this.pnlApp.TabIndex = 4;
            // 
            // btnChange
            // 
            this.btnChange.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnChange.Location = new System.Drawing.Point(297, 5);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(23, 21);
            this.btnChange.TabIndex = 2;
            this.btnChange.Text = "...";
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // cmbApplication
            // 
            this.cmbApplication.ActiveSearchEnabled = true;
            this.cmbApplication.AddNotSet = true;
            this.cmbApplication.CaptionWidth = 120;
            this.cmbApplication.Dock = System.Windows.Forms.DockStyle.Left;
            this.cmbApplication.IsDirty = false;
            this.cmbApplication.Location = new System.Drawing.Point(0, 4);
            this.BresourceLookup1.SetLookup(this.cmbApplication, new FWBS.OMS.UI.Windows.ResourceLookupItem("cmbAppFilter", "Application Filter : ", ""));
            this.cmbApplication.MaxLength = 0;
            this.cmbApplication.Name = "cmbApplication";
            this.cmbApplication.NotSetCode = "CMDSHOWALL";
            this.cmbApplication.NotSetType = "RESOURCE";
            this.cmbApplication.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.cmbApplication.Size = new System.Drawing.Size(296, 23);
            this.cmbApplication.TabIndex = 1;
            this.cmbApplication.TerminologyParse = false;
            this.cmbApplication.Text = "Application Filter : ";
            this.cmbApplication.Type = "CMBFILTERS";
            // 
            // tbDelete
            // 
            this.tbDelete.ImageIndex = 6;
            this.BresourceLookup1.SetLookup(this.tbDelete, new FWBS.OMS.UI.Windows.ResourceLookupItem("Delete", "Delete", ""));
            this.tbDelete.Name = "tbDelete";
            this.tbDelete.Text = "Delete";
            // 
            // tbSp4
            // 
            this.tbSp4.Name = "tbSp4";
            this.tbSp4.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbMove
            // 
            this.tbMove.DropDownMenu = this.cntxMoveTo;
            this.tbMove.Enabled = false;
            this.tbMove.ImageIndex = 39;
            this.BresourceLookup1.SetLookup(this.tbMove, new FWBS.OMS.UI.Windows.ResourceLookupItem("MoveTo", "Move To.", ""));
            this.tbMove.Name = "tbMove";
            this.tbMove.Style = System.Windows.Forms.ToolBarButtonStyle.DropDownButton;
            this.tbMove.Text = "Move To.";
            // 
            // cntxMoveTo
            // 
            this.cntxMoveTo.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuMoveAbove,
            this.mnuMoveBelow});
            // 
            // mnuMoveAbove
            // 
            this.mnuMoveAbove.Index = 0;
            this.mnuMoveAbove.Text = "Move To Above";
            this.mnuMoveAbove.Click += new System.EventHandler(this.mnuMoveAbove_Click);
            // 
            // mnuMoveBelow
            // 
            this.mnuMoveBelow.Index = 1;
            this.mnuMoveBelow.Text = "Move To Below";
            this.mnuMoveBelow.Click += new System.EventHandler(this.mnuMoveBelow_Click);
            // 
            // tbSelect
            // 
            this.tbSelect.ImageIndex = 7;
            this.BresourceLookup1.SetLookup(this.tbSelect, new FWBS.OMS.UI.Windows.ResourceLookupItem("Select", "Select", ""));
            this.tbSelect.Name = "tbSelect";
            this.tbSelect.Text = "Select";
            // 
            // tbSp5
            // 
            this.tbSp5.Name = "tbSp5";
            this.tbSp5.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // tbMoveLeft
            // 
            this.tbMoveLeft.Enabled = false;
            this.tbMoveLeft.ImageIndex = 17;
            this.BresourceLookup1.SetLookup(this.tbMoveLeft, new FWBS.OMS.UI.Windows.ResourceLookupItem("tbMoveLeft", "", "Move Left"));
            this.tbMoveLeft.Name = "tbMoveLeft";
            // 
            // tbMoveRight
            // 
            this.tbMoveRight.Enabled = false;
            this.tbMoveRight.ImageIndex = 18;
            this.BresourceLookup1.SetLookup(this.tbMoveRight, new FWBS.OMS.UI.Windows.ResourceLookupItem("tbMoveRight", "", "Move Right"));
            this.tbMoveRight.Name = "tbMoveRight";
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(545, 58);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 325);
            this.splitter1.TabIndex = 5;
            this.splitter1.TabStop = false;
            // 
            // ucCommandBarBuilder
            // 
            this.Name = "ucCommandBarBuilder";
            this.Size = new System.Drawing.Size(824, 425);
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.pnlMainBack.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.pnlApp.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		#region Overrides
		protected override string SearchListName
		{
			get
			{
				return "ADMTOOLBARS";
			}
		}

		protected override void ShowList()
		{
			_code = "";
			CloseAllMenus();
			base.ShowList();
		}

		protected override bool UpdateData()
		{
			foreach (FWBS.OMS.CommandBarItem uctrl in MenuCommands)
			{
				if (uctrl != null)
				{
					if (uctrl.IsDirty)
					{
						uctrl.Update();
					}
				}
			}
			_currentobj.Update();
			return true;
		}



		protected override void LoadSingleItem(string Code)
		{
            SelectedMenuItem = null;

 		    if (_code != Code)
				cmbApplication.SelectedValue = DBNull.Value;
			labSelectedObject.Text = OMS.Session.CurrentSession.Resources.GetResource("EDTCMDBAR","%1% - Command Bar","",Code).Text;
			_code = Code;
			_currentobj = FWBS.OMS.CommandBarBuilder.GetCommandBarBuilder(Code);
			foreach (Control e in MainMenus)
			{
				pnlMainBack.Controls.Remove(e);
                e.Dispose();
			}
			MainMenus.Clear();
			SortedList Geneology = new SortedList();
			MenuCommands.Clear();
			DataView menus = new DataView(_currentobj.CommandBarControls);
            string f = "";
            if (cmbApplication.Value == DBNull.Value)
                f = "Isnull(ctrlParent,'Null Column') = 'Null Column'";
            else
                f = "Isnull(ctrlParent,'Null Column') = 'Null Column' AND ((ctrlfilter = '{0}') OR ((ctrlfilter like '%[*]%' or ctrlfilter like '%{0}%') and ((ctrlfilter like '%[*]%') and ctrlfilter not like '%!{0}%')))";
			f = String.Format(f, Convert.ToString(cmbApplication.Value));
			menus.RowFilter = f;
			menus.Sort = "ctrlLevel, ctrlOrder";
			foreach (DataRowView rw in menus)
			{
				ucMenuContainer item = null;
				if (Convert.ToBoolean(rw["ctrlBeginGroup"]))
					item = AddMainMenu(true);
				else
					item = AddMainMenu();
				item.Tag = FWBS.OMS.CommandBarItem.GetCommandBarItem(Convert.ToInt32(rw["ctrlID"]), new EventHandler(CommandButtonChanged));
				MenuCommands.Add((FWBS.OMS.CommandBarItem)item.Tag);
				item.Text = Session.CurrentSession.Terminology.Parse(Convert.ToString(rw["ctrlDesc"]),true);
				if (Geneology[Convert.ToString(rw["ctrlCode"])] == null)
					Geneology.Add(Convert.ToString(rw["ctrlCode"]),item);
			}

            if (cmbApplication.Value == DBNull.Value)
                f = "Isnull(ctrlParent,'Null Column') <> 'Null Column'";
            else
                f = "Isnull(ctrlParent,'Null Column') <> 'Null Column' AND ((ctrlfilter = '{0}') OR ((ctrlfilter like '%[*]%' or ctrlfilter like '%{0}%') and ((ctrlfilter like '%[*]%') and ctrlfilter not like '%!{0}%')))";
            f = String.Format(f, Convert.ToString(cmbApplication.Value));
			menus.RowFilter = f;
			menus.Sort = "ctrlLevel, ctrlOrder";
			foreach (DataRowView rw in menus)
			{
				ucMenuContainer item =  Geneology[Convert.ToString(rw["ctrlParent"])] as ucMenuContainer;
				if (item != null)
				{
					ucMenuItem mitem = null;
					ucMenuSplit split = new ucMenuSplit();
					split.LineAlignment = MenuSplitAlignment.Horizontal;
					split.Dock = DockStyle.Top;
					item.Controls.Add(split);
					split.BringToFront();
					split.Visible = Convert.ToBoolean(rw["ctrlBeginGroup"]);

					mitem = item.AddMenuItem();
					mitem.Split = split;
					mitem.IconVisible = (rw["ctrlIcon"] != DBNull.Value);
					mitem.Text = Session.CurrentSession.Terminology.Parse(Convert.ToString(rw["ctrlDesc"]),true);
					mitem.Tag = FWBS.OMS.CommandBarItem.GetCommandBarItem(Convert.ToInt32(rw["ctrlID"]), new EventHandler(CommandButtonChanged));
					MenuCommands.Add(mitem.Tag);
					mitem.MouseDown +=new MouseEventHandler(item_MouseDown);
					if (item.Width < mitem.MenuTextWidth)
						item.Width = mitem.MenuTextWidth;
					if (Geneology[Convert.ToString(rw["ctrlCode"])] == null)
						Geneology.Add(Convert.ToString(rw["ctrlCode"]),mitem);
					mitem.BringToFront();
				}
				else
				{
					ucMenuItem pitem =  Geneology[Convert.ToString(rw["ctrlParent"])] as ucMenuItem;
					if (pitem != null)
					{
						pitem.PopoutVisibe = true;
						item = pitem.ChildMenu;
						ucMenuItem mitem = null;

						ucMenuSplit split = new ucMenuSplit();
						split.LineAlignment = MenuSplitAlignment.Horizontal;
						split.Dock = DockStyle.Top;
						item.Controls.Add(split);
						split.BringToFront();
						split.Visible = Convert.ToBoolean(rw["ctrlBeginGroup"]);

						mitem = item.AddMenuItem();
						mitem.IconVisible = (rw["ctrlIcon"] != DBNull.Value);
						mitem.Split = split;
						mitem.Text = Session.CurrentSession.Terminology.Parse(Convert.ToString(rw["ctrlDesc"]),true);
						mitem.Tag = FWBS.OMS.CommandBarItem.GetCommandBarItem(Convert.ToInt32(rw["ctrlID"]), new EventHandler(CommandButtonChanged));
						MenuCommands.Add(mitem.Tag);
						mitem.MouseDown +=new MouseEventHandler(item_MouseDown);
						if (item.Width < mitem.MenuTextWidth)
							item.Width = mitem.MenuTextWidth;
						if (Geneology[Convert.ToString(rw["ctrlCode"])] == null)
							Geneology.Add(Convert.ToString(rw["ctrlCode"]),mitem);
						mitem.BringToFront();
					}
				}
			}
			ShowEditor();
			Application.DoEvents();

		}
		#endregion

		#region Editor Buttons
		private new void tbcEdit_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == tbAddMenuItem) // ADD MENU ITEM
			{
				if (SelectedMenuItem != null && SelectedMenuItem.Tag is FWBS.OMS.CommandBarItem)
				{
					FWBS.OMS.CommandBarItem cmbitem = SelectedMenuItem.Tag as FWBS.OMS.CommandBarItem;
					if (cmbitem.Code == "")
					{
						MessageBox.ShowInformation("ERRNOTBCODE","You must enter a Code ...");
						return;
					}
					else if (cmbitem.MenuCaption == "")
					{
						MessageBox.ShowInformation("ERRNOTBCAP","You must enter a Menu Caption ...");
						return;
					}
				}
				else if (SelectedMenuContainer != null && SelectedMenuContainer.Tag is FWBS.OMS.CommandBarItem)
				{
					FWBS.OMS.CommandBarItem cmbitem = SelectedMenuContainer.Tag as FWBS.OMS.CommandBarItem;
					if (cmbitem.Code == "")
					{
						MessageBox.ShowInformation("ERRNOTBCODE","You must enter a Code ...");
						return;
					}
					else if (cmbitem.MenuCaption == "")
					{
						MessageBox.ShowInformation("ERRNOTBCAP","You must enter a Menu Caption ...");
						return;
					}
				}
				if (SelectedMenuContainer != null)
				{
					FWBS.OMS.CommandBarItem parent = null;
					if (SelectedMenuItem != null)
						parent = SelectedMenuItem.Tag as FWBS.OMS.CommandBarItem;
					else if (SelectedMenuContainer != null)
						parent = SelectedMenuContainer.Tag as FWBS.OMS.CommandBarItem;
				
					string parentmenu = parent.ParentItem;
					if (parentmenu == "") 
					{
						parentmenu = parent.Code;
						if (parent.Type == "msoControlButton")
							parent.Type = "msoControlPopup";
					}

					ucMenuItem _item = AddMenuItem(SelectedMenuContainer);
					int pl = parent.Level;
					if (pl == 0) pl++;
					_item.Tag = FWBS.OMS.CommandBarItem.CreateCommandBarItem(_currentobj.Code, parentmenu,(SelectedMenuContainer.Controls.Count/2) ,pl,"msoControlButton", new EventHandler(CommandButtonChanged));
					_item.IconVisible = false;
					item_MouseDown(_item,new MouseEventArgs(MouseButtons.Left,0,0,0,0));
					MenuCommands.Add(_item.Tag);
					this.IsDirty=true;
				}
			}
			else if (e.Button == tbAddMainMenu) // ADD MAIN MENU
			{
				if (SelectedMenuItem != null && SelectedMenuItem.Tag is FWBS.OMS.CommandBarItem)
				{
					FWBS.OMS.CommandBarItem cmbitem = SelectedMenuItem.Tag as FWBS.OMS.CommandBarItem;
					if (cmbitem.Code == "")
					{
						MessageBox.ShowInformation("ERRNOTBCODE","You must enter a Code ...");
						return;
					}
					else if (cmbitem.MenuCaption == "")
					{
						MessageBox.ShowInformation("ERRNOTBCAP","You must enter a Menu Caption ...");
						return;
					}
				}
				else if (SelectedMenuContainer != null && SelectedMenuContainer.Tag is FWBS.OMS.CommandBarItem)
				{
					FWBS.OMS.CommandBarItem cmbitem = SelectedMenuContainer.Tag as FWBS.OMS.CommandBarItem;
					if (cmbitem.Code == "")
					{
						MessageBox.ShowInformation("ERRNOTBCODE","You must enter a Code ...");
						return;
					}
					else if (cmbitem.MenuCaption == "")
					{
						MessageBox.ShowInformation("ERRNOTBCAP","You must enter a Menu Caption ...");
						return;
					}
				}
				
				ucMenuContainer _item = AddMainMenu();
				_item.Tag = FWBS.OMS.CommandBarItem.CreateCommandBarItem(_currentobj.Code,"",MainMenus.Count + 1,0, "msoControlButton", new EventHandler(CommandButtonChanged));
				MenuCommands.Add(_item.Tag);
				propertyGrid1.SelectedObject = _item.Tag;
				this.IsDirty=true;
			}
			else if (e.Button == tbAddSubMenu && SelectedMenuItem != null) // ADD SUB MENU
			{
				if (SelectedMenuItem.Tag is FWBS.OMS.CommandBarItem)
				{
					FWBS.OMS.CommandBarItem cmbitem = SelectedMenuItem.Tag as FWBS.OMS.CommandBarItem;
					if (cmbitem.Code == "")
					{
						MessageBox.ShowInformation("ERRNOTBCODE","You must enter a Code ...");
						return;
					}
					else if (cmbitem.MenuCaption == "")
					{
						MessageBox.ShowInformation("ERRNOTBCAP","You must enter a Menu Caption ...");
						return;
					}
				}
				
				FWBS.OMS.CommandBarItem parent = SelectedMenuItem.Tag as FWBS.OMS.CommandBarItem;
				parent.Type = "msoControlPopup";
				ucMenuItem last = SelectedMenuItem;
				SelectedMenuItem.PopoutVisibe = true;
				SelectedMenuItem.ChildMenu.Tag = FWBS.OMS.CommandBarItem.CreateCommandBarItem(_currentobj.Code,parent.Code,MainMenus.Count + 1,0,"msoControlButton", new EventHandler(CommandButtonChanged));
				SelectedMenuItem = null;
				item_MouseDown(last,new MouseEventArgs(MouseButtons.Left,0,0,0,0));
				this.IsDirty=true;
			}
			else if (e.Button == tbDelete && SelectedMenuItem != null) // DELETE MENU ITEM
			{
				if (SelectedMenuItem.ChildMenu != null && SelectedMenuItem.ChildMenu.Controls.Count > 0)
				{
					MessageBox.ShowInformation("PLZKILLKIDS","Please Remove the Child Menus from this Menu before attempting to deleting the Main Menu ... ");
					return;
				}
				SelectedMenuContainer.Controls.Remove(SelectedMenuItem);
				SelectedMenuContainer.Controls.Remove(SelectedMenuItem.Split);
				FWBS.OMS.CommandBarItem parent = SelectedMenuItem.Tag as FWBS.OMS.CommandBarItem;
				parent.Delete();
				SelectedMenuContainer.Expand();
				SelectedMenuItem = null;
				propertyGrid1.SelectedObject = null;
				if (SelectedMenuContainer.Controls.Count == 0 && SelectedMenuContainer.ParentPopoutMenu != null)
				{
					SelectedMenuContainer.Parent.Controls.Remove(SelectedMenuContainer);
					parent = SelectedMenuContainer.ParentPopoutMenu.Tag as FWBS.OMS.CommandBarItem;
					parent.Type = "msoControlButton";
					SelectedMenuContainer.ParentPopoutMenu.PopoutVisibe = false;
				}
				else if (SelectedMenuContainer.Controls.Count == 0)
				{
					parent = SelectedMenuContainer.Tag as FWBS.OMS.CommandBarItem;
					parent.Type = "msoControlButton";
					ucMenuContainer1_Click(SelectedMenuContainer,EventArgs.Empty);
				}
				SelectedMenuContainer = null;
				this.IsDirty=true;
			}
			else if (e.Button == tbDelete && SelectedMenuContainer != null) // DELETE MAIN MENU
			{
				if (SelectedMenuContainer.Controls.Count == 0)
				{	
					SelectedMenuContainer.Parent.Controls.Remove(SelectedMenuContainer.Split);
					SelectedMenuContainer.Parent.Controls.Remove(SelectedMenuContainer);
					FWBS.OMS.CommandBarItem parent = SelectedMenuContainer.Tag as FWBS.OMS.CommandBarItem;
					parent.Delete();
					MainMenus.Remove(SelectedMenuContainer);
					SelectedMenuContainer = null;
					propertyGrid1.SelectedObject = null;
					RepositionTopMenus();
					this.IsDirty=true;
				}
				else
				{
					MessageBox.ShowInformation("PLZKILLKIDS","Please Remove the Child Menus from this Menu before attempting to deleting the Main Menu ... ");
				}
			}
				// SELECT
			else if (e.Button == tbSelect && SelectedMenuItem != null && SelectedMenuItem.Tag is CommandBarItem && ((CommandBarItem)SelectedMenuItem.Tag).Code != "")
			{
				SelectedMenuItem.Marked = !SelectedMenuItem.Marked;
				if (SelectedMenuItem.Marked)
					Select.Add(SelectedMenuItem);
				else
					Select.Remove(SelectedMenuItem);
			}
			else if (e.Button == tbMove && SelectedMenuItem != null) // MOVE MENU ITEMS
			{
				if (tbMove.ImageIndex == 39)
				{
					MoveMenuItems(SelectedMenuItem,false);
				}
				else
				{
					MoveMenuItems(SelectedMenuItem,true);
				}

				for (int i = Select.Count-1 ; i > -1; i--)
				{
					ucMenuItem c = Select[i] as ucMenuItem;
					if (c.Marked == false) Select.Remove(c);
				}
				SelectedMenuContainer.Expand();
				this.IsDirty=true;
			}
			else if (e.Button == tbMoveLeft)
			{
				try
				{
					int n = MainMenus.IndexOf(SelectedMenuContainer);
					ucMenuContainer item1 = MainMenus[n] as ucMenuContainer;
					ucMenuSplit split1 = MainMenus[n-1] as ucMenuSplit;
					ucMenuContainer item2 = MainMenus[n-2] as ucMenuContainer;
					ucMenuSplit split2 = MainMenus[n-3] as ucMenuSplit;
				
					MainMenus[n] = item2;
					MainMenus[n-2] = item1;

					MainMenus[n-1] = split2;
					MainMenus[n-3] = split1;

					RepositionTopMenus();
					this.IsDirty=true;
				}
				catch
				{}
			}
			else if (e.Button == tbMoveRight)
			{
				try
				{
					int n = MainMenus.IndexOf(SelectedMenuContainer);

					ucMenuSplit split1 = MainMenus[n-1] as ucMenuSplit;
					ucMenuContainer item1 = MainMenus[n] as ucMenuContainer;

					ucMenuSplit split2 = MainMenus[n+1] as ucMenuSplit;
					ucMenuContainer item2 = MainMenus[n+2] as ucMenuContainer;
				
					MainMenus[n] = item2;
					MainMenus[n-1] = split2;

					MainMenus[n+1] = split1;
					MainMenus[n+2] = item1;

					RepositionTopMenus();
					this.IsDirty=true;
				}
				catch
				{}
			}
		}
		#endregion

		#region Private Menu Events
		private void CommandButtonChanged(object sender, EventArgs e)
		{
			CommandBarItem cmditem = sender as CommandBarItem;
			
			if (SelectedMenuItem != null)
			{
				if (Convert.ToString(cmditem.MenuCaption) != "" && cmditem.Code !="") SelectedMenuItem.Text = Session.CurrentSession.Terminology.Parse(cmditem.MenuCaption,true);
				SelectedMenuItem.Split.Visible = cmditem.BeginGroup; 
				SelectedMenuItem.ParentMenu.Expand();
				SelectedMenuItem.IconVisible = (cmditem.Icon != -1);
				if (cmditem.Level > 1)
					SelectedMenuItem.ParentMenu.Width = 0;
				else
					SelectedMenuItem.ParentMenu.Width = SelectedMenuItem.ParentMenu.HeaderTextWidth + 2;
				foreach (Control n in SelectedMenuItem.ParentMenu.Controls)
				{	
					if (n is ucMenuItem)
					{
						ucMenuItem mi = n as ucMenuItem;
						if (mi.MenuTextWidth > SelectedMenuItem.ParentMenu.Width)
							SelectedMenuItem.ParentMenu.Width = mi.MenuTextWidth;
					}
				}
			}
			else if (SelectedMenuContainer != null)
			{
				if (cmditem.MenuCaption != "") SelectedMenuContainer.Text  = Session.CurrentSession.Terminology.Parse(cmditem.MenuCaption,true);
				SelectedMenuContainer.Split.Visible = cmditem.BeginGroup; 
				RepositionTopMenus();
			}
		}

		private void ucMenuContainer1_Click(object sender, EventArgs e)
		{
			ucMenuItem previousMenuItem = SelectedMenuItem;
			
			if (previousMenuItem != null && previousMenuItem.Tag is FWBS.OMS.CommandBarItem)
			{
				FWBS.OMS.CommandBarItem cmbitem = previousMenuItem.Tag as FWBS.OMS.CommandBarItem;
				if (cmbitem.Code == "")
				{
					MessageBox.ShowInformation("ERRNOTBCODE","You must enter a Code ...");
					return;
				}
				else if (cmbitem.MenuCaption == "")
				{
					MessageBox.ShowInformation("ERRNOTBCAP","You must enter a Menu Caption ...");
					return;
				}
			}

			foreach (Control n in MainMenus)
			{
				if (n is ucMenuContainer)
				{
					ucMenuContainer c = n as ucMenuContainer;
					ClosePopupMenus(c);
					c.Selected = false;
					c.Shrink();
				}
			}
			SelectedMenuContainer = sender as ucMenuContainer;
			item_MouseDown(null,new MouseEventArgs(MouseButtons.Left,0,0,0,0));
			SelectedMenuItem = null;
			propertyGrid1.SelectedObject = SelectedMenuContainer.Tag;
			SelectedMenuContainer.Selected = true;
			SelectedMenuContainer.Expand();

		}

		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			this.IsDirty=true;
		}

		private void item_MouseDown(object sender, MouseEventArgs e)
		{
			if (SelectedMenuItem == sender) return;
			ucMenuItem previousMenuItem = SelectedMenuItem;
			
			if (previousMenuItem != null && previousMenuItem.Tag is FWBS.OMS.CommandBarItem)
			{
				FWBS.OMS.CommandBarItem cmbitem = previousMenuItem.Tag as FWBS.OMS.CommandBarItem;
				if (cmbitem.Code == "")
				{
					MessageBox.ShowInformation("ERRNOTBCODE","You must enter a Code ...");
					return;
				}
				else if (cmbitem.MenuCaption == "")
				{
					MessageBox.ShowInformation("ERRNOTBCAP","You must enter a Menu Caption ...");
					return;
				}
			}
			
			if (SelectedMenuItem != null) 
			{
				SelectedMenuItem.Selected = false;
			}

			SelectedMenuItem = sender as ucMenuItem;
			if (SelectedMenuItem != null)
			{
				if (SelectedMenuItem.ChildMenu != SelectedMenuItem.ParentMenu)
					ClosePopupMenus(SelectedMenuItem.ParentMenu);
			
				SelectedMenuItem.Selected = true;
				if (SelectedMenuItem.ChildMenu != null && SelectedMenuItem.ParentMenu.PopoutMenu == null)
				{
					ucMenuItem item = null;
					pnlMainBack.Controls.Add(SelectedMenuItem.ChildMenu);
					if (SelectedMenuItem.ChildMenu.Controls.Count == 0)
					{
						ucMenuSplit split = new ucMenuSplit();
						split.LineAlignment = MenuSplitAlignment.Horizontal;
						split.Dock = DockStyle.Top;
						SelectedMenuItem.ChildMenu.Controls.Add(split);
						split.BringToFront();
						split.Visible = false;

						FWBS.OMS.CommandBarItem parent = SelectedMenuItem.Tag as FWBS.OMS.CommandBarItem;
						item = SelectedMenuItem.ChildMenu.AddMenuItem();
						item.Text = "Untitled";
						item.Split = split;
						item.Tag = FWBS.OMS.CommandBarItem.CreateCommandBarItem(_currentobj.Code,parent.Code,(SelectedMenuItem.ChildMenu.Controls.Count / 2), parent.Level + 1,"msoControlButton", new EventHandler(CommandButtonChanged));
						item.MouseDown +=new MouseEventHandler(item_MouseDown);
						MenuCommands.Add(item.Tag);
					}

					SelectedMenuItem.ChildMenu.Expand();
					SelectedMenuItem.ChildMenu.BringToFront();
					SelectedMenuItem.ChildMenu.ParentMenu = SelectedMenuContainer;
					SelectedMenuItem.ParentMenu.PopoutMenu = SelectedMenuItem.ChildMenu;
					SelectedMenuContainer = SelectedMenuItem.ChildMenu;
					if (item != null) item_MouseDown(item,new MouseEventArgs(MouseButtons.Left,0,0,0,0));
				}
				
				SelectedMenuContainer = SelectedMenuItem.ParentMenu;
				propertyGrid1.SelectedObject = SelectedMenuItem.Tag;
			}
			else
			{
				propertyGrid1.SelectedObject = null;
			}
		}

		private void btnChange_Click(object sender, System.EventArgs e)
		{
			if (IsObjectDirty())
			{
				CloseAllMenus();
				LoadSingleItem(_code);
			}
		}

		private void mnuMoveAbove_Click(object sender, System.EventArgs e)
		{
			tbMove.ImageIndex = 38;
			tbcEdit_ButtonClick(sender,new ToolBarButtonClickEventArgs(tbMove));
		}

		private void mnuMoveBelow_Click(object sender, System.EventArgs e)
		{
			tbMove.ImageIndex = 39;
			tbcEdit_ButtonClick(sender,new ToolBarButtonClickEventArgs(tbMove));
		}

		private void propertyGrid1_SelectedObjectsChanged(object sender, System.EventArgs e)
		{
			if (propertyGrid1.SelectedObject == null)
			{
				tbAddMenuItem.Enabled = false;
				tbAddSubMenu.Enabled = false;
				tbDelete.Enabled = false;
				tbSelect.Enabled = false;
				tbMove.Enabled = false;
				tbMoveLeft.Enabled = false;
				tbMoveRight.Enabled = false;
				return;
			}
			CommandBarItem cbi = propertyGrid1.SelectedObject as CommandBarItem;
			if (cbi.Level == 0)
			{
				tbMoveLeft.Enabled = true;
				tbMoveRight.Enabled = true;
				tbAddMenuItem.Enabled = true;
				tbAddSubMenu.Enabled = false;
				tbDelete.Enabled = true;
				tbSelect.Enabled = false;
				tbMove.Enabled = false;
			}
			else if (cbi.Level > 0)
			{
				tbMoveLeft.Enabled = false;
				tbMoveRight.Enabled = false;
				tbAddMenuItem.Enabled = true;
				tbAddSubMenu.Enabled = (cbi.Type != "msoControlPopup");
				tbDelete.Enabled = true;
				tbSelect.Enabled = true;
				tbMove.Enabled = true;
			}	
		}

		#endregion

		#region Private
		private void MoveMenuItems(ucMenuItem mi, bool Above)
		{
			ucMenuContainer oldparent = mi.ParentMenu;
			if (mi.Split != null)
			{
				mi.Split.Parent = SelectedMenuContainer;
				mi.Split.BringToFront();
			}
			mi.Parent = SelectedMenuContainer;
			if (mi.ParentMenu.IsShrunk == false) mi.ParentMenu.Expand();
			bool StartMove = false;
			SortedList SorectedControl = null;

			// Re Arranges the Menu to put the new Menu to the Selected Place
			SorectedControl = new SortedList();
			foreach(Control ctrl in SelectedMenuContainer.Controls)
			{
				if ((ctrl is ucMenuSplit) == false)
					SorectedControl.Add(ctrl.Top + ctrl.Height,ctrl);
			}
			if (Above)
			{
				ucMenuItem prevcontrol = null;
				foreach(Control ctrl in SorectedControl.Values)
				{
					if (ctrl is ucMenuItem && ((ucMenuItem)ctrl).Marked && StartMove)
						break;

					if (StartMove)
					{
						((ucMenuItem)ctrl).Split.BringToFront();
						ctrl.BringToFront();
					}
					if (ctrl == SelectedMenuItem && prevcontrol != null) 
					{
						prevcontrol.BringToFront();
						StartMove = true;
					}
					prevcontrol = ((ucMenuItem)ctrl); 
				}			
			}
			else
			{
				int Stage = 0;
				foreach(Control ctrl in SorectedControl.Values)
				{
					if (ctrl is ucMenuItem && ((ucMenuItem)ctrl).Marked && StartMove)
						break;

					if (Stage == 2)
					{
						((ucMenuItem)ctrl).Split.BringToFront();
						ctrl.BringToFront();
					}
					else if (Stage == 1) 
					{
						Stage++;
					}
					else if (ctrl == SelectedMenuItem) 
					{
						ctrl.BringToFront();
						Stage++;
					}
				}	
			}

			// Sets the Order and Level for the Placed Menu
			SorectedControl.Clear();
			foreach(Control ctrl in SelectedMenuContainer.Controls)
			{
				if ((ctrl is ucMenuSplit) == false)
					SorectedControl.Add(ctrl.Top + ctrl.Height,ctrl);
			}
			int i = 0;
			foreach(ucMenuItem ctrl in SorectedControl.Values)
			{
				if (ctrl.Tag is CommandBarItem)
				{
					CommandBarItem cbaritem = ((CommandBarItem)ctrl.Tag);
					cbaritem.Level =((CommandBarItem)SelectedMenuItem.Tag).Level;
					cbaritem.Order = i;
					cbaritem.ParentItem = ((CommandBarItem)SelectedMenuItem.Tag).ParentItem;
					ctrl.ParentMenu = SelectedMenuContainer;
					i++;
				}
			}

			// Sets the Order of the Menu Container the Item was removed from
			SorectedControl.Clear();
			foreach(Control ctrl in oldparent.Controls)
			{
				if ((ctrl is ucMenuSplit) == false)
					SorectedControl.Add(ctrl.Top + ctrl.Height,ctrl);
			}
			if (SorectedControl.Count > 0)
			{
				i = 0;
				foreach(ucMenuItem ctrl in SorectedControl.Values)
				{
					if (ctrl.Tag is CommandBarItem)
					{
						CommandBarItem cbaritem = ((CommandBarItem)ctrl.Tag);
						cbaritem.Order = i;
						i++;
					}
				}
			}
			else if (oldparent.Tag is CommandBarItem)
				((CommandBarItem)oldparent.Tag).Type = "msoControlButton";

		}

		private void CloseAllMenus()
		{
			foreach (Control n in MainMenus)
			{
				if (n is ucMenuContainer)
				{
					ucMenuContainer c = n as ucMenuContainer;
					ClosePopupMenus(c);
					c.Selected = false;
					c.Shrink();
				}
			}
		}

		private void ClosePopupMenus(ucMenuContainer menu)
		{
			if (menu.PopoutMenu != null)
			{
				if (menu.PopoutMenu.PopoutMenu != null)
					ClosePopupMenus(menu.PopoutMenu);
				pnlMainBack.Controls.Remove(menu.PopoutMenu);
                menu.PopoutMenu = null;
			}
		}

		private int NextMenuPos()
		{
			int m = LogicalToDeviceUnits(16), s = LogicalToDeviceUnits(4);
			foreach (Control c in pnlMainBack.Controls)
			{
				if (c is ucMenuContainer)
				{
					ucMenuContainer n = c as ucMenuContainer;
					m += n.HeaderTextWidth + 1;
				}
				else if (c is ucMenuSplit)
				{
					m += s;
				}
			}
			return m;
		}

		private void RepositionTopMenus()
		{
			int o = 0;
			int m = LogicalToDeviceUnits(16), s = LogicalToDeviceUnits(4);
			foreach (Control n in MainMenus)
			{
				if (n is ucMenuContainer)
				{
					ucMenuContainer c = n as ucMenuContainer;
					c.Left = m;
					c.BringToFront();
					CommandBarItem cmdb = c.Tag as CommandBarItem;
					if (cmdb != null)
					{
						cmdb.Order = o++;
					}
					m += c.HeaderTextWidth;
				}
				else if (n is ucMenuSplit)
				{
					n.Left = m;
					n.BringToFront();
					m += s;
				}
			}
		}

		private ucMenuContainer AddMainMenu()		
		{
			return AddMainMenu(false);
		}

		private ucMenuContainer AddMainMenu(bool BeginGroup)
		{
            int y = LogicalToDeviceUnits(12);
			ucMenuSplit split = null;
			split = new ucMenuSplit();
			MainMenus.Add(split);
			split.Location = new Point(NextMenuPos(), y);
			pnlMainBack.Controls.Add(split);
			split.BringToFront();
			split.Visible = BeginGroup;

			ucMenuContainer item = new ucMenuContainer();
			MainMenus.Add(item);
			ucMenuContainer1_Click(item,EventArgs.Empty);
			item.Split = split;
			item.Click +=new EventHandler(ucMenuContainer1_Click);
			item.Location = new Point(NextMenuPos(), y);
			pnlMainBack.Controls.Add(item);
			item.BringToFront();
			item.Shrink();
			return item;
		}

		private ucMenuItem AddMenuItem(ucMenuContainer menu)
		{
			ucMenuSplit split = new ucMenuSplit();
			split.LineAlignment = MenuSplitAlignment.Horizontal;
			split.Dock = DockStyle.Top;
			menu.Controls.Add(split);
			split.BringToFront();
			split.Visible = false;
			
			ucMenuItem item = menu.AddMenuItem();
			item.Split = split;
			item.Text = "Untitled";
			item.MouseDown +=new MouseEventHandler(item_MouseDown);
			menu.Expand();
			return item;
		}

		private void CheckEnabledStated()
		{
			propertyGrid1_SelectedObjectsChanged(this,EventArgs.Empty);
		}

		#endregion

        #region Protected

        protected override void CloseAndReturnToList()
        {
            if (base.IsDirty)
            {
                DialogResult? dr = base.IsObjectDirtyDialogResult();
                if (dr != DialogResult.Cancel)
                {
                    ShowList();
                }
            }
            else
            {
                ShowList();
            }
        }

        #endregion


    }
}
