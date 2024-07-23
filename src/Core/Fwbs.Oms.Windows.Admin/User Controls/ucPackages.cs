using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Design.Export;
using FWBS.OMS.Design.Package;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.Windows.Admin
{
    /// <summary>
    /// Summary description for ucPackages.
    /// </summary>
    public class ucPackages : ucEditBase2
	{
		private System.Windows.Forms.Splitter splitter3;
		private System.Windows.Forms.Splitter splitter1;

		private string _code;
		private FWBS.OMS.UI.TreeViewRad treeView1;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme1;
		private FWBS.Common.UI.Windows.ToolBar tbTreeView;
		private System.Windows.Forms.ToolBarButton treeDelete;
		private System.Windows.Forms.ToolBarButton tbUpdate;
		private System.Windows.Forms.ToolBarButton treeUp;
		private System.Windows.Forms.ToolBarButton treeDown;
		private System.Windows.Forms.Panel pnlTree;
		private System.Windows.Forms.Panel pnlOMS;
		private System.Windows.Forms.OpenFileDialog opdScript;
		private System.Windows.Forms.ToolBarButton tbRepair;
		private FWBS.OMS.Design.Package.Packages _currentobj = null;
		private Splitter splitter4;
		private Panel panel1;
		private Panel pnlTreeSearch;
		private TextBox txtTreeSearch;
		private Button btnRefreshTree;
		private Panel panel2;
		private Common.UI.Windows.eInformation eInformation1;
		private Splitter splitter6;
		private Panel pnlProperties;
		private PropertyGrid propertyGrid1;
		private Panel pnlSearchControl;
		private ucSearchControl ucSearchControl1;
		private Splitter splitter2;

		Point mouseDownPoint;
	
		#region InitializeComponent
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucPackages));
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.pnlTree = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.treeView1 = new FWBS.OMS.UI.TreeViewRad();
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme();
            this.pnlTreeSearch = new System.Windows.Forms.Panel();
            this.txtTreeSearch = new System.Windows.Forms.TextBox();
            this.btnRefreshTree = new System.Windows.Forms.Button();
            this.splitter4 = new System.Windows.Forms.Splitter();
            this.tbTreeView = new FWBS.Common.UI.Windows.ToolBar();
            this.treeDelete = new System.Windows.Forms.ToolBarButton();
            this.treeUp = new System.Windows.Forms.ToolBarButton();
            this.treeDown = new System.Windows.Forms.ToolBarButton();
            this.tbUpdate = new System.Windows.Forms.ToolBarButton();
            this.pnlOMS = new System.Windows.Forms.Panel();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.pnlSearchControl = new System.Windows.Forms.Panel();
            this.ucSearchControl1 = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.pnlProperties = new System.Windows.Forms.Panel();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.panel2 = new System.Windows.Forms.Panel();
            this.eInformation1 = new FWBS.Common.UI.Windows.eInformation();
            this.splitter6 = new System.Windows.Forms.Splitter();
            this.opdScript = new System.Windows.Forms.OpenFileDialog();
            this.tbRepair = new System.Windows.Forms.ToolBarButton();
            this.tpList.SuspendLayout();
            this.tpEdit.SuspendLayout();
            this.pnlEdit.SuspendLayout();
            this.pnlToolbarContainer.SuspendLayout();
            this.pnlTree.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.treeView1)).BeginInit();
            this.pnlTreeSearch.SuspendLayout();
            this.pnlOMS.SuspendLayout();
            this.pnlSearchControl.SuspendLayout();
            this.pnlProperties.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tpEdit
            // 
            this.tpEdit.Controls.Add(this.pnlOMS);
            this.tpEdit.Controls.Add(this.splitter3);
            this.tpEdit.Controls.Add(this.splitter1);
            this.tpEdit.Controls.Add(this.pnlTree);
            this.tpEdit.Location = new System.Drawing.Point(0, 0);
            this.BresourceLookup1.SetLookup(this.tpEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("Edit", "Edit", ""));
            this.tpEdit.Size = new System.Drawing.Size(616, 400);
            this.tpEdit.Controls.SetChildIndex(this.pnlEdit, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlTree, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter1, 0);
            this.tpEdit.Controls.SetChildIndex(this.splitter3, 0);
            this.tpEdit.Controls.SetChildIndex(this.pnlOMS, 0);
            // 
            // pnlEdit
            // 
            this.pnlEdit.Size = new System.Drawing.Size(616, 50);
            // 
            // labSelectedObject
            // 
            this.labSelectedObject.Size = new System.Drawing.Size(616, 22);
            // 
            // tbcEdit
            // 
            this.tbcEdit.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.tbUpdate,
            this.tbRepair});
            this.tbcEdit.Size = new System.Drawing.Size(616, 27);
            this.tbcEdit.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbcEdit_ButtonClick);
            // 
            // tbSave
            // 
            this.BresourceLookup1.SetLookup(this.tbSave, new FWBS.OMS.UI.Windows.ResourceLookupItem("Save", "Save", ""));
            this.tbSave.Text = "Save";
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
            this.pnlToolbarContainer.Size = new System.Drawing.Size(616, 26);
            // 
            // splitter3
            // 
            this.splitter3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter3.Location = new System.Drawing.Point(611, 50);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(5, 350);
            this.splitter3.TabIndex = 209;
            this.splitter3.TabStop = false;
            this.splitter3.Visible = false;
            // 
            // splitter1
            // 
            this.splitter1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter1.Location = new System.Drawing.Point(256, 50);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(5, 350);
            this.splitter1.TabIndex = 206;
            this.splitter1.TabStop = false;
            // 
            // pnlTree
            // 
            this.pnlTree.Controls.Add(this.panel1);
            this.pnlTree.Controls.Add(this.pnlTreeSearch);
            this.pnlTree.Controls.Add(this.splitter4);
            this.pnlTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlTree.Location = new System.Drawing.Point(0, 50);
            this.pnlTree.Name = "pnlTree";
            this.pnlTree.Padding = new System.Windows.Forms.Padding(0, 5, 6, 0);
            this.pnlTree.Size = new System.Drawing.Size(256, 350);
            this.pnlTree.TabIndex = 207;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.treeView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 45);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 297);
            this.panel1.TabIndex = 211;
            // 
            // treeView1
            // 
            this.treeView1.AllowAdd = true;
            this.treeView1.AllowDragDrop = true;
            this.treeView1.AllowDrop = true;
            this.treeView1.AllowPlusMinusAnimation = true;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.HideSelection = false;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowLines = true;
            this.treeView1.Size = new System.Drawing.Size(250, 297);
            this.treeView1.TabIndex = 2;
            this.treeView1.ThemeName = "Windows8";
            this.treeView1.DragEnding += new Telerik.WinControls.UI.RadTreeView.DragEndingHandler(this.treeView1_DragEnding);
            this.treeView1.SelectedNodeChanged += new Telerik.WinControls.UI.RadTreeView.RadTreeViewEventHandler(this.treeView1_SelectedNodeChanged);
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.treeView1_DragDrop);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.treeView1_DragEnter);
            // 
            // pnlTreeSearch
            // 
            this.pnlTreeSearch.BackColor = System.Drawing.Color.White;
            this.pnlTreeSearch.Controls.Add(this.txtTreeSearch);
            this.pnlTreeSearch.Controls.Add(this.btnRefreshTree);
            this.pnlTreeSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTreeSearch.Location = new System.Drawing.Point(0, 5);
            this.pnlTreeSearch.Name = "pnlTreeSearch";
            this.pnlTreeSearch.Padding = new System.Windows.Forms.Padding(8, 8, 2, 8);
            this.pnlTreeSearch.Size = new System.Drawing.Size(250, 40);
            this.pnlTreeSearch.TabIndex = 210;
            // 
            // txtTreeSearch
            // 
            this.txtTreeSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTreeSearch.Location = new System.Drawing.Point(8, 9);
            this.txtTreeSearch.Name = "txtTreeSearch";
            this.txtTreeSearch.Size = new System.Drawing.Size(215, 23);
            this.txtTreeSearch.TabIndex = 13;
            this.txtTreeSearch.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtTreeSearch_KeyPress);
            // 
            // btnRefreshTree
            // 
            this.btnRefreshTree.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRefreshTree.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRefreshTree.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshTree.Image")));
            this.btnRefreshTree.Location = new System.Drawing.Point(224, 8);
            this.btnRefreshTree.Name = "btnRefreshTree";
            this.btnRefreshTree.Size = new System.Drawing.Size(24, 24);
            this.btnRefreshTree.TabIndex = 14;
            this.btnRefreshTree.UseVisualStyleBackColor = true;
            this.btnRefreshTree.Click += new System.EventHandler(this.btnRefreshTree_Click);
            // 
            // splitter4
            // 
            this.splitter4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter4.Location = new System.Drawing.Point(0, 342);
            this.splitter4.Name = "splitter4";
            this.splitter4.Size = new System.Drawing.Size(250, 8);
            this.splitter4.TabIndex = 6;
            this.splitter4.TabStop = false;
            // 
            // tbTreeView
            // 
            this.tbTreeView.Appearance = System.Windows.Forms.ToolBarAppearance.Flat;
            this.tbTreeView.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.treeDelete,
            this.treeUp,
            this.treeDown});
            this.tbTreeView.Dock = System.Windows.Forms.DockStyle.None;
            this.tbTreeView.DropDownArrows = true;
            this.tbTreeView.Location = new System.Drawing.Point(65, 19);
            this.tbTreeView.Name = "tbTreeView";
            this.tbTreeView.ShowToolTips = true;
            this.tbTreeView.Size = new System.Drawing.Size(250, 28);
            this.tbTreeView.TabIndex = 3;
            this.tbTreeView.TextAlign = System.Windows.Forms.ToolBarTextAlign.Right;
            this.tbTreeView.Visible = false;
            this.tbTreeView.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbTreeView_ButtonClick);
            // 
            // treeDelete
            // 
            this.treeDelete.Enabled = false;
            this.treeDelete.ImageIndex = 6;
            this.treeDelete.Name = "treeDelete";
            this.treeDelete.Text = "Delete";
            // 
            // treeUp
            // 
            this.treeUp.ImageIndex = 38;
            this.treeUp.Name = "treeUp";
            this.treeUp.Text = "Up";
            // 
            // treeDown
            // 
            this.treeDown.ImageIndex = 39;
            this.treeDown.Name = "treeDown";
            this.treeDown.Text = "Down";
            // 
            // tbUpdate
            // 
            this.tbUpdate.Name = "tbUpdate";
            this.tbUpdate.Text = "Update";
            // 
            // pnlOMS
            // 
            this.pnlOMS.Controls.Add(this.splitter2);
            this.pnlOMS.Controls.Add(this.pnlSearchControl);
            this.pnlOMS.Controls.Add(this.pnlProperties);
            this.pnlOMS.Controls.Add(this.panel2);
            this.pnlOMS.Controls.Add(this.tbTreeView);
            this.pnlOMS.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlOMS.Location = new System.Drawing.Point(261, 50);
            this.pnlOMS.Name = "pnlOMS";
            this.pnlOMS.Size = new System.Drawing.Size(350, 350);
            this.pnlOMS.TabIndex = 210;
            // 
            // splitter2
            // 
            this.splitter2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter2.Location = new System.Drawing.Point(0, 90);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(350, 5);
            this.splitter2.TabIndex = 224;
            this.splitter2.TabStop = false;
            // 
            // pnlSearchControl
            // 
            this.pnlSearchControl.Controls.Add(this.ucSearchControl1);
            this.pnlSearchControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSearchControl.Location = new System.Drawing.Point(0, 0);
            this.pnlSearchControl.Name = "pnlSearchControl";
            this.pnlSearchControl.Size = new System.Drawing.Size(350, 95);
            this.pnlSearchControl.TabIndex = 223;
            // 
            // ucSearchControl1
            // 
            this.ucSearchControl1.BackColor = System.Drawing.Color.White;
            this.ucSearchControl1.BackGroundColor = System.Drawing.Color.White;
            this.ucSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl1.DoubleClickAction = "None";
            this.ucSearchControl1.Enabled = false;
            this.ucSearchControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucSearchControl1.GraphicalPanelVisible = true;
            this.ucSearchControl1.Location = new System.Drawing.Point(0, 0);
            this.ucSearchControl1.Name = "ucSearchControl1";
            this.ucSearchControl1.NavCommandPanel = null;
            this.ucSearchControl1.Padding = new System.Windows.Forms.Padding(5);
            this.ucSearchControl1.RefreshOnEnquiryFormRefreshEvent = false;
            this.ucSearchControl1.SaveSearch = FWBS.OMS.SearchEngine.SaveSearchType.Never;
            this.ucSearchControl1.SearchListCode = "";
            this.ucSearchControl1.SearchListType = "";
            this.ucSearchControl1.SearchPanelVisible = false;
            this.ucSearchControl1.Size = new System.Drawing.Size(350, 95);
            this.ucSearchControl1.TabIndex = 221;
            this.ucSearchControl1.ToBeRefreshed = false;
            this.ucSearchControl1.TypeSelectorVisible = false;
            // 
            // pnlProperties
            // 
            this.pnlProperties.Controls.Add(this.propertyGrid1);
            this.pnlProperties.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlProperties.Location = new System.Drawing.Point(0, 95);
            this.pnlProperties.Name = "pnlProperties";
            this.pnlProperties.Size = new System.Drawing.Size(350, 255);
            this.pnlProperties.TabIndex = 219;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.HelpBackColor = System.Drawing.Color.White;
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(350, 255);
            this.propertyGrid1.TabIndex = 222;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.eInformation1);
            this.panel2.Controls.Add(this.splitter6);
            this.panel2.Location = new System.Drawing.Point(99, 132);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(187, 94);
            this.panel2.TabIndex = 218;
            // 
            // eInformation1
            // 
            this.eInformation1.BackColor = System.Drawing.Color.White;
            this.eInformation1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.eInformation1.Location = new System.Drawing.Point(0, 0);
            this.eInformation1.Name = "eInformation1";
            this.eInformation1.Padding = new System.Windows.Forms.Padding(0, 0, 3, 3);
            this.eInformation1.Size = new System.Drawing.Size(187, 89);
            this.eInformation1.TabIndex = 213;
            this.eInformation1.Text = "One Line Information Line";
            this.eInformation1.Title = "Help Bar";
            this.eInformation1.Visible = false;
            // 
            // splitter6
            // 
            this.splitter6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter6.Location = new System.Drawing.Point(0, 89);
            this.splitter6.Name = "splitter6";
            this.splitter6.Size = new System.Drawing.Size(187, 5);
            this.splitter6.TabIndex = 203;
            this.splitter6.TabStop = false;
            // 
            // opdScript
            // 
            this.opdScript.DefaultExt = "sql";
            this.opdScript.Filter = "SQL Scripts (*.sql)|*.sql|All Files (*.*)|*.*";
            this.opdScript.Title = "Browse for Script";
            // 
            // tbRepair
            // 
            this.tbRepair.Name = "tbRepair";
            this.tbRepair.Text = "Repair Tree";
            this.tbRepair.Visible = false;
            // 
            // ucPackages
            // 
            this.Name = "ucPackages";
            this.tpList.ResumeLayout(false);
            this.tpEdit.ResumeLayout(false);
            this.pnlEdit.ResumeLayout(false);
            this.pnlToolbarContainer.ResumeLayout(false);
            this.pnlToolbarContainer.PerformLayout();
            this.pnlTree.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.treeView1)).EndInit();
            this.pnlTreeSearch.ResumeLayout(false);
            this.pnlTreeSearch.PerformLayout();
            this.pnlOMS.ResumeLayout(false);
            this.pnlOMS.PerformLayout();
            this.pnlSearchControl.ResumeLayout(false);
            this.pnlProperties.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		#region Constructors
		public ucPackages() : base()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			treeView1.ImageList = FWBS.OMS.UI.Windows.Images.AdminMenu16();
		}

		public ucPackages(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params) : base(mainparent,editparent,Params)
		{
			if (frmMain.PartnerAccess == false)
				Session.CurrentSession.ValidateLicensedFor("SDKALL");
			// This call is required by the Windows Form Designer.
			InitializeComponent();
			CreateNodeContextMenu();
			treeView1.ImageList = FWBS.OMS.UI.Windows.Images.AdminMenu16();
			tbTreeView.ImageList = FWBS.OMS.UI.Windows.Images.CoolButtons16();
            SetButtonImage();

			ucSearchControl1.SetSearchListType(Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.Package),null,new FWBS.Common.KeyValueCollection());
			ucSearchControl1.SearchButtonCommands -= new SearchButtonEventHandler(ucSearchControl1_SearchButtonCommands);
			ucSearchControl1.SearchButtonCommands += new SearchButtonEventHandler(ucSearchControl1_SearchButtonCommands);

			ucSearchControl1.dgSearchResults.AllowDrop = true;
			ucSearchControl1.dgSearchResults.MouseDown -= new MouseEventHandler(dgSearchResults_MouseDown);
			ucSearchControl1.dgSearchResults.MouseDown += new MouseEventHandler(dgSearchResults_MouseDown);
			ucSearchControl1.dgSearchResults.MouseMove -= new MouseEventHandler(dgSearchResults_MouseMove);
			ucSearchControl1.dgSearchResults.MouseMove += new MouseEventHandler(dgSearchResults_MouseMove);
			ucSearchControl1.dgSearchResults.DragOver += new DragEventHandler(dgSearchResults_DragOver);
        }

        private void SetButtonImage()
        {
            Bitmap imgRefresh = FWBS.OMS.UI.Properties.Resources.Refresh;
            if (DeviceDpi != 96)
            {
                ScaleBitmapLogicalToDevice(ref imgRefresh);
            }
            this.btnRefreshTree.Image = imgRefresh;
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            SetButtonImage();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

		#endregion

		#region Overrides
		protected override string SearchListName
		{
			get
			{
				return "ADMPACKAGES";
			}
		}


		protected override void LoadSingleItem(string Code)
		{
			_code = Code;
			try
			{
				_currentobj = FWBS.OMS.Design.Package.Packages.GetPackage(Code);
			}
			catch (Exception ex)
			{
                ErrorBox.Show(ParentForm, new Exception(Session.CurrentSession.Resources.GetMessage("LOADPACKAGE", "Error loading Package click Advanced ...", "").Text,ex));
				return;
			}
			_currentobj.DataChanged +=new EventHandler(_currentobj_DataChanged);
			_currentobj.Progress +=new FWBS.OMS.Design.Import.ProgressEventHandler(_import_Progress);
			propertyGrid1.SelectedObject = _currentobj;
			labSelectedObject.Text = _currentobj.Code + Session.CurrentSession.Resources.GetResource("LABSELECTED", " - Package", "").Text;
			ucSearchControl1.Enabled = true;
			BuildTreeview();
			if (treeView1.Nodes.Count > 0)
				treeView1.SelectedNode = treeView1.Nodes[0];
			ShowEditor(false);
			ucSearchControl1.SetSearchListType(Session.CurrentSession.DefaultSystemSearchListGroups(FWBS.OMS.SystemSearchListGroups.Package),null,new FWBS.Common.KeyValueCollection());
			System.IO.DirectoryInfo exp = new System.IO.DirectoryInfo(_currentobj.ExportPath);
			if (exp.Exists == false)
                ErrorBox.Show(ParentForm, new Exception(Session.CurrentSession.Resources.GetMessage("EXPORTNOTEXIST", "Error the Export Path Does not Exists please correct...", "").Text));
			this.IsDirty=false;
		}

		protected override bool UpdateData()
		{
			try
			{
				if (ucSearchControl1.IsDirty)
					ucSearchControl1.UpdateItem();

				FWBS.OMS.Design.Export.TreeView _newtree = new FWBS.OMS.Design.Export.TreeView();
				BuildTreeViewFromTreeView(_newtree,treeView1.Nodes,-1);

				foreach (Dependent d in _currentobj.TreeView.DependentPackages)
					_newtree.DependentPackages.Add(d);

				_currentobj.TreeView = _newtree;
				_currentobj.External = false;
				_currentobj.Update();

				LoadSingleItem(_currentobj.Code);
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
			_code = InputBox.Show(Session.CurrentSession.Resources.GetMessage("PLZECODE2","Please enter a Code for this Package (15 Characters Max)","").Text);
			if (_code == InputBox.CancelText) return;

			if (string.IsNullOrWhiteSpace(_code))
			{
				MessageBox.Show(Session.CurrentSession.Resources.GetResource("NOPKGCODE", "You must enter a code for the new package (15 Characters Max). The new package will not be created.", "").Text, "Package Code Not Supplied", MessageBoxButtons.OK, MessageBoxIcon.Stop);
				return;
			}

			if(_code.Length > 15)
				_code = _code.Substring(0, 15);

			if(!CheckIfPackageAlreadyExists(_code))
			{
				_currentobj = new FWBS.OMS.Design.Package.Packages(false);
				_currentobj.Code = _code;
				_currentobj.DataChanged += new EventHandler(_currentobj_DataChanged);
				_currentobj.Progress += new FWBS.OMS.Design.Import.ProgressEventHandler(_import_Progress);
				SetupPackageEditing(true);
			}
		}

		protected override void Clone(string Code)
		{
			_code = InputBox.Show(Session.CurrentSession.Resources.GetMessage("PLZECODE2", "Please enter a Code for this Package (15 Characters Max)", "").Text);
			if (_code == InputBox.CancelText) return;

			if (_code.Length > 15)
				_code = _code.Substring(0, 15);

			if(!CheckIfPackageAlreadyExists(_code))
			{
				_currentobj = FWBS.OMS.Design.Package.Packages.Clone(Code, _code);
				_currentobj.DataChanged += new EventHandler(_currentobj_DataChanged);
				_currentobj.Progress += new FWBS.OMS.Design.Import.ProgressEventHandler(_import_Progress);
				ucSearchControl1.Enabled = true; 
				SetupPackageEditing(true); 
			} 
		}

		private bool CheckIfPackageAlreadyExists(string code)
		{
			if (code.Length > 15)
				code = code.Substring(0, 15);

			Packages checkpackage = new Packages(false);
			bool result = checkpackage.Exists(code);
			if (result)
			{
				System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("PKGTHERE", "The code you have entered already exists.\n\nPlease enter a different Code for this Package (15 Characters Max) and try again.", "").Text,
					Session.CurrentSession.Resources.GetMessage("PKGTHERETITLE", "Duplicate Package Code", "").Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				checkpackage.Dispose();
			}
			return result;
		}

		private void SetupPackageEditing(bool newObject)
		{
			ucSearchControl1.Enabled = (_currentobj.Code != "");
			labSelectedObject.Text = _currentobj.Code + " - Package";
			propertyGrid1.SelectedObject = _currentobj;
			BuildTreeview();
			ShowEditor(newObject);
			this.IsDirty = false;
		}

		protected override void DeleteData(string Code)
		{
			FWBS.OMS.Design.Package.Packages.DeletePackage(Code);
		}

		#endregion

		#region Progress
		private frmImportProgress _importprogress = null;
		private bool _importprogresscancel = false;

		/// <summary>
		/// Process the Progress Bar Event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _import_Progress(object sender, FWBS.OMS.Design.Import.ProgressEventArgs e)
		{
			if (e.Close)
			{
				if (_importprogress != null)
				{
					_importprogress.Close();
					_importprogress = null;
				}
				return;
			}
			if (_importprogress == null)
			{
				_importprogress = new frmImportProgress();
				_importprogress.Show();
				_importprogress.Cancelled += new EventHandler(_importprogress_Cancelled);
			}

			_importprogress.ProgressBar.Maximum = ((FWBS.OMS.Design.Package.Packages)sender).PackageCount;
			_importprogress.Label = e.Label;
			_importprogress.ProgressBar.Increment(1);
			e.Cancel = _importprogresscancel; // This will alow the Business Layer to Cancel
			Application.DoEvents();
		}

		/// <summary>
		/// If the Cancel Button is Clicked on the Import Progress Form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _importprogress_Cancelled(object sender, EventArgs e)
		{
			_importprogresscancel = true;
		}
		#endregion

		#region Private
		/// <summary>
		/// Recursive Build of the Business TreeView Object
		/// </summary>
		/// <param name="treeview">The TreeView to Read</param>
		/// <param name="Nodes">The Node</param>
		/// <param name="pid">The Parent ID for the Business TreeView</param>
		private void BuildTreeViewFromTreeView(FWBS.OMS.Design.Export.TreeView treeview, RadTreeNodeCollection Nodes, int pid)
		{
			foreach (RadTreeNode node in Nodes)
			{
				DataRowView cdr = node.Tag as DataRowView;
				FWBS.OMS.Design.Export.PackageTypes _type = (FWBS.OMS.Design.Export.PackageTypes)cdr["Type"];

				int n = (_type == PackageTypes.None)
						? treeview.Add(0, 24, "ROOT", "Root", true, -1, FWBS.OMS.Design.Export.PackageTypes.None, "Root", false, false)
						: treeview.Add(Convert.ToInt32(cdr["ImageIndex"]), Convert.ToString(cdr["Code"]), Convert.ToString(cdr["Name"]), Convert.ToBoolean(cdr["Active"]), pid, _type, Convert.ToString(cdr["Description"]), Convert.ToBoolean(cdr["RootImportable"]), Convert.ToBoolean(cdr["InstallOnce"]));

				if (node.Nodes != null)
					BuildTreeViewFromTreeView(treeview, node.Nodes, n);
			}
		}

		/// <summary>
		/// Builds the Graphic Tree View
		/// </summary>
		private void BuildTreeview()
		{
			SortedList _parents = new SortedList();
			treeView1.BeginUpdate();
			treeView1.Nodes.Clear();
			if (_currentobj.TreeView.Source.Rows.Count == 0)
			{
				_currentobj.TreeView.Add(0, 24, "ROOT", "Root", true, -1, FWBS.OMS.Design.Export.PackageTypes.None, "Root", false, false);
				MessageBox.ShowInformation("IMPORTEDONLY", "This is an Imported Package");
				this.propertyGrid1.Enabled = false;
				this.ucSearchControl1.Enabled = false;
			}
			else
			{
				this.propertyGrid1.Enabled = true;
				this.ucSearchControl1.Enabled = true;
			}

			DataView nodes = new DataView(_currentobj.TreeView.Source);
			nodes.RowStateFilter = DataViewRowState.OriginalRows;
			nodes.RowStateFilter = DataViewRowState.CurrentRows;
			nodes.Sort = "Parent,ID";

			foreach (System.Data.DataRowView dr in nodes)
			{
				_currentobj.TreeView.Goto(dr);
				if (_currentobj.TreeView.ParentID == -1  && _currentobj.TreeView.ID != 0)
					_currentobj.TreeView.ParentID = 0;
				var imageindex = (Convert.ToInt32(dr["ImageIndex"])== -1) ? 24 : Convert.ToInt32(dr["ImageIndex"]);
				RadTreeNode tn = new RadTreeNode(_currentobj.TreeView.Name, treeView1.ImageList.Images[imageindex]);
				
				tn.Tag = _currentobj.TreeView.RowView;
				tn.Checked = true;
				if (_parents[_currentobj.TreeView.ParentID] == null)
					treeView1.Nodes.Add(tn);
				else
				{
					((RadTreeNode)_parents[_currentobj.TreeView.ParentID]).Nodes.Add(tn);
					if (Convert.ToInt32(dr["Parent"]) == 0)
						tn.ContextMenu = NodeMenu;
				}

				if (Convert.ToInt32(dr["Parent"]) == -1 || Convert.ToInt32(dr["Parent"]) == 0 || Convert.ToInt32(dr["Parent"]) > 0)
					tn.AllowDrop = false;

				_parents.Add(_currentobj.TreeView.ID,tn);
			}
			treeView1.RootElement.AllowDrop = false;
			treeView1.Nodes[0].Expand();
			treeView1.Nodes[0].Text = _currentobj.Description;
			treeDelete.Enabled = false;
			treeView1.EndUpdate();

		}

		/// <summary>
		/// Recursive Removal of Empty Parent Nodes
		/// </summary>
		/// <param name="node">The Node to Check</param>
		private void RemoveParentIsRootable(RadTreeNode node)
		{
			if (node != null && node.Nodes.Count == 0 && node.Parent != null && node.Tag is DataRowView && Convert.ToBoolean(((DataRowView)node.Tag)["RootImportable"]) == false)
			{
				RadTreeNode nde = treeView1.SelectedNode.Parent;
				node.Remove();
				RemoveParentIsRootable(nde);
			}
		}

		
		/// <summary>
		/// Check if Node is a Child of another Node
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="node"></param>
		/// <returns></returns>
		private bool IsNodeChildofParent(RadTreeNode parent, RadTreeNode node)
		{
			foreach (RadTreeNode n in node.Nodes)
			{
				if (n == parent) 
					return true;
				if (n.Nodes.Count > 0)
				{
					if (IsNodeChildofParent(parent,n))
						return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Adds Items to the Package
		/// </summary>
		/// <param name="sender">The Searchlist Toolbar</param>
		/// <param name="e">The Button Arg</param>
		private void ucSearchControl1_SearchButtonCommands(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
		{
			if (e.ButtonName == "cmdAdd")
			{
				ProcessPackageItemAddition(this.ucSearchControl1.SearchList.Code, Convert.ToString(ucSearchControl1.CurrentItem()[0].Value));
			}
			ucSearchControl1.QuickFilterContol.Focus();
		}


		public void ProcessPackageItemAddition(string searchListCode, string selectedValue)
		{
			if (searchListCode == "SCHSYSPKGMIS")
				AddPackageItem(PackageTypes.Milestones,selectedValue);
			else if (searchListCode == "SCHSYSPKGFMA")
				AddPackageItem(PackageTypes.FileManagement,selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageEnquiry))
				AddPackageItem(PackageTypes.EnquiryForms,selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageSearchLists))
				AddPackageItem(PackageTypes.SearchLists,selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageReports))
				AddPackageItem(PackageTypes.Reports,selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageCodeLookups))
				AddPackageItem(PackageTypes.CodeLookups,selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackagePrecedents))
				AddPackageItem(PackageTypes.Precedents,selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageScripts))
				AddPackageItem(PackageTypes.Scripts,selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageSqlScripts))
				AddPackageItem(PackageTypes.SQLScripts,selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageExtendedData))
				AddPackageItem(PackageTypes.ExtendedData,selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageObjects))
				AddPackageItem(PackageTypes.OMSObjects,selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageDataLists))
				AddPackageItem(PackageTypes.DataLists,selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageDataPackages))
				AddPackageItem(PackageTypes.DataPackages, selectedValue);
			else if (searchListCode == OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackageWorkflows))
				AddPackageItem(PackageTypes.Workflows, selectedValue);
			else if (searchListCode == "SCHSYSPKGUPD")
				AddPackageItem((PackageTypes)ConvertDef.ToEnum(Convert.ToString(ucSearchControl1.CurrentItem()[1].Value), PackageTypes.None), selectedValue);
		}


		private void AddPackageItem(PackageTypes item, string code)
		{
			System.IO.DirectoryInfo _newpath = null; 
			FWBS.OMS.Design.Export.ExportBase eb = null;

			try
			{
				LockState ls = new LockState();
				if (!ls.CheckObjectLockStateForPackaging(code, GetLockableObjectType(item)))
				{
					_newpath = new System.IO.DirectoryInfo(_currentobj.ExportPath);
					_newpath = _newpath.CreateSubdirectory(_currentobj.Code);
					switch (item)
					{
						case PackageTypes.DataLists:
							eb = new FWBS.OMS.Design.Export.DataLists(code, _currentobj.TreeView);
							break;
						case PackageTypes.Screens:
						case PackageTypes.EnquiryForms:
							eb = new FWBS.OMS.Design.Export.EnquiryForm(code, _currentobj.TreeView);
							break;
						case PackageTypes.Reports:
							eb = new FWBS.OMS.Design.Export.Report(code, _currentobj.TreeView);
							break;
						case PackageTypes.SearchLists:
							eb = new FWBS.OMS.Design.Export.SearchList(code, _currentobj.TreeView);
							break;
						case PackageTypes.Scripts:
							eb = new FWBS.OMS.Design.Export.Scripts(code, _currentobj.TreeView);
							break;
						case PackageTypes.CodeLookups:
							eb = new FWBS.OMS.Design.Export.CodeLookupType(code, _currentobj.TreeView);
							break;
						case PackageTypes.Precedents:
							eb = new FWBS.OMS.Design.Export.Precedent(code, _currentobj.TreeView);
							break;
						case PackageTypes.SQLScripts:
							eb = new FWBS.OMS.Design.Export.SQLScripts(code, _currentobj.TreeView);
							break;
						case PackageTypes.ExtendedData:
							eb = new FWBS.OMS.Design.Export.ExtendedData(code, _currentobj.TreeView);
							break;
						case PackageTypes.OMSObjects:
							eb = new FWBS.OMS.Design.Export.OMSObjects(code, _currentobj.TreeView);
							break;
						case PackageTypes.DataPackages:
							eb = new FWBS.OMS.Design.Export.DataPackage(code, _currentobj.TreeView);
							break;
						case PackageTypes.FileManagement:
							eb = new FWBS.OMS.Design.Export.FileManagement(code, _currentobj.TreeView);
							break;
						case PackageTypes.Milestones:
							eb = new FWBS.OMS.Design.Export.Milestones(code, _currentobj.TreeView);
							break;
						case PackageTypes.Workflows:
							eb = new FWBS.OMS.Design.Export.Workflows(code, _currentobj.TreeView);
							break;
					}
				}

				if (eb != null)
				{
					eb.TreeViewParentID = Convert.ToInt32(((DataRowView)treeView1.Nodes[0].Tag)["ID"]);
					eb.ExportTo(_newpath.FullName);
					BuildTreeview();
				}

				OnDirty(EventArgs.Empty);
				_currentobj.CreateTreeViewXml(_currentobj.Description, _currentobj.Help);
			}
			catch (Exception ex)
			{
                if (ex.Message.StartsWith(Session.CurrentSession.Resources.GetResource("STARTSWITH", "Column 'Code, ImageIndex, Parent, Type' is constrained to be unique", "").Text))
					ErrorBox.Show(ParentForm, new Exception(Session.CurrentSession.Resources.GetMessage("ITEMEXISTS", "The Item already exists in the Package...", "").Text, ex));
				else
					ErrorBox.Show(ParentForm, new Exception(Session.CurrentSession.Resources.GetMessage("ERRADDTOPAC", "Error Adding to Package...", "").Text, ex));
			}
			finally
			{
				if (eb != null && eb is ILinkedObjects)
					AutoProcessAssociatedObjectData(eb as ILinkedObjects, code);
			}
		}

		private LockableObjects GetLockableObjectType(PackageTypes item)
		{
			switch (item)
			{
				case PackageTypes.DataLists:
					return LockableObjects.DataList;
				case PackageTypes.Screens:
				case PackageTypes.EnquiryForms:
					return LockableObjects.EnquiryForm;
				case PackageTypes.SearchLists:
					return LockableObjects.SearchList;
				case PackageTypes.Scripts:
					return LockableObjects.Script;
				case PackageTypes.Precedents:
					return LockableObjects.Precedent;
				case PackageTypes.FileManagement:
					return LockableObjects.FileManagement;
				default:
					return LockableObjects.None;
			}
		}

		private void AutoProcessAssociatedObjectData(ILinkedObjects obj, string code)
		{
			if (obj.LinkedObjects != null && obj.LinkedObjects.Count > 0)
			{
				var linkedObjects = obj is FWBS.OMS.Design.Export.Precedent ? obj.LinkedObjects.Where(mp => mp.ID != code) : obj.LinkedObjects;
				foreach(LinkedObject ao in linkedObjects)
					ProcessPackageItemAddition(ao.SearchListCode, ao.ID);
			}
		}

		/// <summary>
		/// Locks down the Search List if the Code is not set
		/// </summary>
		/// <param name="s"></param>
		/// <param name="e"></param>
		private void propertyGrid1_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			ucSearchControl1.Enabled = (_currentobj.Code != "");
			if (e.ChangedItem.PropertyDescriptor.Name == "Description")
			{
				treeView1.Nodes[0].Text = _currentobj.Description;
				treeView1_SelectedNodeChanged(this, new RadTreeViewEventArgs(treeView1.Nodes[0], RadTreeViewAction.ByMouse));
			}
		}

		/// <summary>
		/// Graphic Tree View Tool Bar
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tbTreeView_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == treeDelete)
			{
				DeleteSelectedTreeNode();
			}
			else if (e.Button == treeUp && treeView1.SelectedNode.PrevNode != null)
			{
				RadTreeNode n = (RadTreeNode)treeView1.SelectedNode.Clone();
				treeView1.SelectedNode.Parent.Nodes.Insert(treeView1.SelectedNode.PrevNode.Index,n);
				treeView1.SelectedNode.Remove();
				treeView1.SelectedNode = n;

				FWBS.OMS.Design.Export.TreeView _newtree = new FWBS.OMS.Design.Export.TreeView();
				BuildTreeViewFromTreeView(_newtree,treeView1.Nodes,-1);
				_currentobj.TreeView = _newtree;
				OnDirty(EventArgs.Empty);

			}
			else if (e.Button == treeDown && treeView1.SelectedNode.NextNode != null)
			{
				RadTreeNode n = (RadTreeNode)treeView1.SelectedNode.Clone();
				treeView1.SelectedNode.Parent.Nodes.Insert(treeView1.SelectedNode.NextNode.Index + 1,n);
				treeView1.SelectedNode.Remove();
				treeView1.SelectedNode = n;

				FWBS.OMS.Design.Export.TreeView _newtree = new FWBS.OMS.Design.Export.TreeView();
				BuildTreeViewFromTreeView(_newtree,treeView1.Nodes,-1);
				_currentobj.TreeView = _newtree;

				OnDirty(EventArgs.Empty);
			}
		}

		/// <summary>
		/// The Top Tool Bar in the Editor
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private new void tbcEdit_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			if (e.Button == tbRepair)
			{
				try
				{
					DataView dv = new DataView(_currentobj.TreeView.Source);
					dv.RowFilter = "CODE = 'ROOT'";
					if (dv.Count == 0)
						_currentobj.TreeView.Add(0, 24, "ROOT", _currentobj.Description, true, 0, FWBS.OMS.Design.Export.PackageTypes.None, _currentobj.Description, false,false);
					else
					{
						dv.Delete(0);
						_currentobj.TreeView.Add(0, 24, "ROOT", _currentobj.Description, true, 0, FWBS.OMS.Design.Export.PackageTypes.None, _currentobj.Description, false,false);
					}
					dv.Table.AcceptChanges();
					dv.RowFilter = "";
					dv.Sort = "ID";
					foreach (System.Data.DataRowView dr in dv)
					{
						if (Convert.ToBoolean(dr["RootImportable"]))
							dr["Parent"] = 0;
					}
					BuildTreeview();
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
			}
			else if (e.Button == tbUpdate)
			{
				if (IsObjectDirty())
				{
					_currentobj.RefreshPackage();

					BuildTreeview();
					if (_importprogress != null)
					{
						_importprogress.Close();
						_importprogress = null;
					}
					UpdateData();
				}
			}
		}

		/// <summary>
		/// Grabs the Quick Search Control from the Enquiry Form if the Precedent List is Used
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ucSearchControl1_SearchListLoad(object sender, System.EventArgs e)
		{
			if (this.ucSearchControl1.SearchList != null && this.ucSearchControl1.SearchList.Code == "SCHSYSPKGPREC")
				this.ucSearchControl1.QuickFilterContol = (Control)ucSearchControl1.EnquiryForm.GetIBasicEnquiryControl2("_quickSearch").Control;
			else
				this.ucSearchControl1.QuickFilterContol = null;
		}

		#endregion

		#region Drag & Drop

		void dgSearchResults_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}

		void dgSearchResults_MouseDown(object sender, MouseEventArgs e)
		{
            DataGridView.HitTestInfo hit = ucSearchControl1.dgSearchResults.HitTest(e.X, e.Y);
			if (hit.Type == DataGridViewHitTestType.ColumnHeader)
				return;

            if (e.Button.Equals(MouseButtons.Left))
				mouseDownPoint = e.Location;
		}

		void dgSearchResults_MouseMove(object sender, MouseEventArgs e)
		{
            DataGridView.HitTestInfo hit = ucSearchControl1.dgSearchResults.HitTest(e.X, e.Y);
            if (hit.Type == DataGridViewHitTestType.ColumnHeader)
                return;
            
			double dragDistance = 0;
			Point currentLocation = e.Location;
			if (e.Button.Equals(MouseButtons.Left))
			{
				dragDistance = System.Math.Abs(System.Math.Sqrt(
					(
						System.Math.Pow((currentLocation.X - mouseDownPoint.X), 2)
					+
						System.Math.Pow((currentLocation.Y - mouseDownPoint.Y), 2)
					)));
			}

			if (dragDistance > 10)
			{
				string selected = "";
				foreach (KeyValueCollection kvc in ucSearchControl1.SelectedItems)
				{
					selected += selected == "" ? Convert.ToString(kvc[0].Value) : "," + Convert.ToString(kvc[0].Value);
				}
				ucSearchControl1.dgSearchResults.DoDragDrop(selected, DragDropEffects.Move);
			}
		}

		private void treeView1_SelectedNodeChanged(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
		{
			try
			{
				System.Data.DataRowView rw = e.Node.Tag as System.Data.DataRowView;
				tbTreeView.Enabled = (e.Node.Parent != null && Convert.ToBoolean(rw["RootImportable"]));
				treeDelete.Enabled = (e.Node.Parent != null && Convert.ToBoolean(rw["RootImportable"]));
				eInformation1.Title = Convert.ToString(rw["Name"]);
				eInformation1.Text = Convert.ToString(rw["Description"]);
				if (Convert.ToString(rw["Code"]) == "ROOT")
					propertyGrid1.SelectedObject = _currentobj;
				else
					propertyGrid1.SelectedObject = rw["TreeViewItem"];

				RadTreeNode selected = treeView1.SelectedNode;
				if(selected != treeView1.Nodes[0])
				{
					foreach (RadTreeNode n in treeView1.Nodes[0].Nodes)
					{
						n.Font = new Font(treeView1.Font, FontStyle.Regular);
						SetChildNodeFontToRegular(n);

					}
					selected.Font = new Font(treeView1.Font, FontStyle.Bold);
				}
			}
			catch
			{
				tbTreeView.Enabled = false;
				treeDelete.Enabled = false;
			}
		}

		private void SetChildNodeFontToRegular(RadTreeNode n)
		{
			foreach (RadTreeNode node in n.Nodes)
			{
				node.Font = new Font(treeView1.Font, FontStyle.Regular);
				SetChildNodeFontToRegular(node);
			}
		}

		private void treeView1_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}

		private void treeView1_DragDrop(object sender, DragEventArgs e)
		{
			string passeddata = e.Data.GetData(DataFormats.Text).ToString();
			string[] selected = passeddata.Split(',');
			if (selected.Length > 0)
			{
				foreach (string item in selected)
				{
					ProcessPackageItemAddition(this.ucSearchControl1.SearchList.Code, item);
				}
			}
		}

		private void treeView1_DragEnding(object sender, RadTreeViewDragCancelEventArgs e)
		{
			if (e.TargetNode.Level != e.Node.Level)
			{
				e.Cancel = true;
			}
		}

		RadContextMenu NodeMenu;

		private void CreateNodeContextMenu()
		{
			NodeMenu = new RadContextMenu();
			RadMenuItem menuitem = new RadMenuItem("Delete");
			menuitem.Click += new EventHandler(menuitem_Click);
			NodeMenu.Items.Add(menuitem);
		}

		void menuitem_Click(object sender, EventArgs e)
		{
			DeleteSelectedTreeNode();
		}

		private void DeleteSelectedTreeNode()
		{
			if (treeView1.SelectedNode != null)
			{
				RadTreeNode nde = treeView1.SelectedNode.Parent;
				treeView1.SelectedNode.Remove();
				RemoveParentIsRootable(nde);

				FWBS.OMS.Design.Export.TreeView _newtree = new FWBS.OMS.Design.Export.TreeView();
				BuildTreeViewFromTreeView(_newtree, treeView1.Nodes, -1);
				_currentobj.TreeView = _newtree;

				_currentobj.IsDirty = true;
				OnDirty(EventArgs.Empty);
			}
		}


		#endregion

		#region Dirty Control & Editor Closure

		private void _currentobj_DataChanged(object sender, EventArgs e)
		{
			OnDirty(e);
		}

		public override bool IsDirty
		{
			get
			{
				return (base.IsDirty || ucSearchControl1.IsDirty);
			}
			set
			{
				base.IsDirty = value;
			}
		}

		protected override void CloseAndReturnToList()
		{
			if (base.IsDirty)
			{
				DialogResult? dr = base.IsObjectDirtyDialogResult();
				if (dr != DialogResult.Cancel)
				{
					if(dr == DialogResult.No)
					{
						txtTreeSearch.Text = null;
						base.ShowList();
					}
				}
			}
			else
			{
				txtTreeSearch.Text = null;
				base.ShowList();
			}
	   }

		private bool CheckForPackageDescription()
		{
			bool result = false;
			if (!string.IsNullOrWhiteSpace(_currentobj.Description))
			{
				result = true;
			}
			return result;
		}

		#endregion

		#region TreeView Search

		private void txtTreeSearch_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Return && !string.IsNullOrWhiteSpace(this.txtTreeSearch.Text))
				ExecuteSearchProcess();
		}

		private void ExecuteSearchProcess()
		{
			if (!string.IsNullOrWhiteSpace(this.txtTreeSearch.Text))
			{
				TreeViewSearch(this.txtTreeSearch.Text.ToUpper());
			}
		}

		private void btnRefreshTree_Click(object sender, EventArgs e)
		{
			RestoreNodesToOriginalFormat();
			this.txtTreeSearch.Text = null;
		}

		private void RestoreNodesToOriginalFormat()
		{
			RadTreeNodeCollection nodes = treeView1.Nodes[0].Nodes;
			foreach (RadTreeNode n in nodes)
			{
				SetNodeVisible(n);
				RestoreChildNodesToOriginalFormat(n);
			}
		}

		private void RestoreChildNodesToOriginalFormat(RadTreeNode n)
		{
			foreach (RadTreeNode node in n.Nodes)
			{
				SetNodeVisible(node);
				RestoreChildNodesToOriginalFormat(node);
			}
		}

		public void TreeViewSearch(string searchstring)
		{
			RadTreeNodeCollection nodes = treeView1.Nodes[0].Nodes;
			foreach (RadTreeNode n in nodes)
			{
				if (PerformNodeChecks(n, searchstring))
					SetMatchingNodeVisible(n);
				else
					n.Visible = false;
				TreeViewSearchChildren(n, searchstring);
			}
		}

		private void TreeViewSearchChildren(RadTreeNode treeNode, string searchstring)
		{
			foreach (RadTreeNode rtn in treeNode.Nodes)
			{
				if (PerformNodeChecks(rtn, searchstring))
				{
					SetMatchingNodeVisible(rtn);
					ExpandParentNodes(rtn);
				}
				else
					rtn.Visible = false;
				TreeViewSearchChildren(rtn, searchstring);
			}
		}

		private bool PerformNodeChecks(RadTreeNode n, string searchstring)
		{
			if (n.Text.ToUpper().Contains(searchstring))
				return true;
			else
				return false;
		}

		private void SetMatchingNodeVisible(RadTreeNode n)
		{
			n.Font = new Font(treeView1.Font, FontStyle.Bold);
			n.ForeColor = System.Drawing.Color.CornflowerBlue;
			n.Visible = true;
		}

		private void SetNodeVisible(RadTreeNode n)
		{
			n.Font = new Font(treeView1.Font, FontStyle.Regular);
			n.ForeColor = System.Drawing.Color.Black;
			n.Visible = true;
		}

		private void ExpandParentNodes(RadTreeNode n)
		{
			while (n != null)
			{
				n.Expand();
				n.Visible = true;
				n = n.Parent;
			}
		}

		#endregion

	}
}