using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using FWBS.Common;
using FWBS.OMS.Data;
using FWBS.OMS.DocumentManagement.Commands;
using FWBS.OMS.DocumentManagement.Storage;
using FWBS.OMS.HighQ;
using FWBS.OMS.Interfaces;
using FWBS.OMS.Security.Permissions;
using FWBS.OMS.StatusManagement;
using FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement;
using Infragistics.Win.UltraWinDock;

namespace FWBS.OMS.UI.Windows.DocumentManagement.Addins
{

    /// <summary>
    /// Displays a list of documents associated to a specific client / file.  This is used as
    /// a IConfigurableTypeAddin, to be used in OMSDialogs.
    /// </summary>
    public class ucBuiltInDocuments : ucBaseAddin, Interfaces.IOMSTypeAddin, IDocumentsAddin, IAdvancedPreviewHandler
	{
		#region Fields

		private IOMSType _type = null;
		private Client _client = null;
		private OMSFile _file =null;
		private Contact _contact = null;

		private string _searchlistcode = "";
		private Common.KeyValueCollection parameters;
		private string _docref = String.Empty;
		private string _ourref = String.Empty;
		private string _doctype = String.Empty;
		private string _created = String.Empty;
		private string _createdby = String.Empty;

		private bool _firsttime = true;

		private DocumentHighlightOptions _highlightOptions = new DocumentHighlightOptions();

   
		#endregion

		#region Search List Buttons

		private OMSToolBarButton _inoutdoc = null;
		private OMSToolBarButton _outdoc = null;
		private OMSToolBarButton _indoc = null;

		private OMSToolBarButton _localall = null;
		private OMSToolBarButton _localunchanged = null;
		private OMSToolBarButton _localchanged = null;     

		#endregion

		#region Controls

		private IContainer components;
		private FWBS.OMS.UI.Windows.ucSearchControl ucSearchControl1;
		private ContextMenu mnuOpenDoc;
		private IconMenuItem mnuOpen;
		private IconMenuItem mnuOpenLatest;
		private IconMenuItem mnuOpenVersion;
		private IconMenuItem mnuOpenAndClose;
		private IconMenuItem mnuOpenLatestAndClose;
		private IconMenuItem mnuOpenVersionAndClose;
		private ImageList icons;
		private ImageList imageList1;
		private ucPanelNav pnDetails;
		private ucPanelNav pnlGrouping;
		private ucDocumentGrouping DocumentGrouping;
		private ucNavPanel ucNavPanel2;
		private DocumentPreviewAddin documentPreviewer1;
		private Timer timer1;
		private Timer timerExecCommand;
		private ucNavRichText ucNavRichText1;
		private bool closingForm = false;


		private omsDockManager omsDockManager1;
        private Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1;
        private Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane2;
        private Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1;
		private Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane2;
		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
		private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea2;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
		private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow2;

		private Infragistics.Win.UltraWinDock.AutoHideControl _ucBaseAddinAutoHideControl;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucBaseAddinUnpinnedTabAreaTop;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucBaseAddinUnpinnedTabAreaBottom;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucBaseAddinUnpinnedTabAreaLeft;
		private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucBaseAddinUnpinnedTabAreaRight;
		
		//treeView-related objects
        private ucMatterFoldersTree _matterFoldersTree;

        Point mouseDownPoint;

		#endregion

		#region Events
		
		public event EventHandler DocumentSelecting;
		public event EventHandler DocumentSelected;
		public event EventHandler DocumentsRefreshed;

		private void OnDocumentSelecting()
		{
			EventHandler ev = DocumentSelecting;
			if (ev != null)
			{
				ev(this, EventArgs.Empty);
			}
		}

		private void OnDocumentSelected()
		{
			EventHandler ev = DocumentSelected;
			if (ev != null)
			{
				ev(this, EventArgs.Empty);
			}
		}

		private void OnDocumentsRefreshed()
		{
			EventHandler ev = DocumentsRefreshed;
			if (ev != null)
			{
				ev(this, EventArgs.Empty);
			}
		}

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default constructor of the user control.
		/// </summary>
		public ucBuiltInDocuments()
		{
			Debug.WriteLine("ucBuiltInDocuments Constructor...");
			// This call is required by the Windows.Forms Form Designer.

			//NOTE: To design this form uncomment this but remember to comment before check in.
			//InitializeComponent();

			Debug.WriteLine("ucBuiltInDocuments Constructor Complete...");
		}

        private void FinishFormConstruction()
		{
			if (_firsttime)
			{

				Debug.WriteLine("FinishFormConstruction Start InitializeComponent...");
                InitializeComponent();
                this.AutoScaleMode = AutoScaleMode.Inherit;

                SetDockingFormat();
				Debug.WriteLine("FinishFormConstruction End InitializeComponent...");

				pnlGrouping.Size = new Size(152, 200);
				pnlGrouping.ExpandedHeight = 200;

				Res resources = Session.CurrentSession.Resources;
				if (resources != null)
				{
					Debug.WriteLine("FinishFormConstruction Get Resources...");
					_docref = resources.GetResource("DOCREF", "Document Ref : ", "").Text;
					_ourref = resources.GetResource("OURREF", "Our Ref : ", "").Text;
					_doctype = resources.GetResource("DOCTYPE", "Document Type : ", "").Text;
					_created = resources.GetResource("CREATED", "Created : ", "").Text;
					_createdby = resources.GetResource("CREATEDBY", "Created By : ", "").Text;

					this.mnuOpen.Text = resources.GetResource("OPEN", "Open", "").Text;
					this.mnuOpenAndClose.Text = resources.GetResource("OPEN&&CLOSE", "Open && Close", "").Text;
					this.mnuOpenLatest.Text = this.mnuOpenLatestAndClose.Text = resources.GetResource("LATEST", "Latest", "").Text;
					this.mnuOpenVersion.Text = this.mnuOpenVersionAndClose.Text = resources.GetResource("SPECVERSIONDLG", "Specific Version ...", "").Text;

					omsDockManager1.PaneFromControl(_matterFoldersTree).Text = resources.GetResource("DOCFOLDERS", "Document Folders", "").Text;
					omsDockManager1.PaneFromControl(documentPreviewer1).Text = resources.GetResource("DOCPREVIEW", "Document Preview", "").Text;
					Debug.WriteLine("FinishFormConstruction Get Resources Complete...");
				}
				_firsttime = false;
			}
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (pnDetails != null)
				{
					pnDetails.Controls.Remove(this.ucNavRichText1);
				}

				if (ucNavRichText1 != null)
				{
					ucNavRichText1.Dispose();
				}

				if (ucSearchControl1 != null)
				{
					ucSearchControl1.Dispose();
				}
				if (components != null)
				{
					components.Dispose();
				}

				// TK - get rid of the preview resources
				if (this.documentPreviewer1 != null)
				{
					this.documentPreviewer1.Dispose();
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
            this.components = new System.ComponentModel.Container();
            this.dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("41805f65-7924-466f-b595-233b7a5cdaa6"));
            this.dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("07f2190b-e98d-4e08-ba64-6c9380cf7427"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("41805f65-7924-466f-b595-233b7a5cdaa6"), -1);
            this.dockAreaPane2 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("41805f65-7924-466f-b595-233b7a5cdaa7"));
            this.dockableControlPane2 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("07f2190b-e98d-4e08-ba64-6c9380cf7428"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("41805f65-7924-466f-b595-233b7a5cdaa7"), -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucBuiltInDocuments));
            this._matterFoldersTree = new FWBS.OMS.UI.Windows.ucMatterFoldersTree();
            this.documentPreviewer1 = new FWBS.OMS.UI.Windows.DocumentManagement.Addins.DocumentPreviewAddin();
            this.omsDockManager1 = new omsDockManager(this.components, new omsDockManagerInitializeSettings { InitializePinned = true, InitializeDockingLocation = true, InitializePaneSize = true });
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.dockableWindow2 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this.windowDockingArea2 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this._ucBaseAddinUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucBaseAddinUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucBaseAddinUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucBaseAddinUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucBaseAddinAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.ucSearchControl1 = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.mnuOpenDoc = new System.Windows.Forms.ContextMenu();
            this.mnuOpen = new FWBS.OMS.UI.Windows.IconMenuItem();
            this.mnuOpenLatest = new FWBS.OMS.UI.Windows.IconMenuItem();
            this.mnuOpenVersion = new FWBS.OMS.UI.Windows.IconMenuItem();
            this.mnuOpenAndClose = new FWBS.OMS.UI.Windows.IconMenuItem();
            this.mnuOpenLatestAndClose = new FWBS.OMS.UI.Windows.IconMenuItem();
            this.mnuOpenVersionAndClose = new FWBS.OMS.UI.Windows.IconMenuItem();
            this.icons = new System.Windows.Forms.ImageList(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnDetails = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavRichText1 = new FWBS.OMS.UI.Windows.ucNavRichText();
            this.DocumentGrouping = new FWBS.OMS.UI.Windows.DocumentManagement.ucDocumentGrouping();
            this.pnlGrouping = new FWBS.OMS.UI.Windows.ucPanelNav();
            this.ucNavPanel2 = new FWBS.OMS.UI.Windows.ucNavPanel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timerExecCommand = new System.Windows.Forms.Timer(this.components);
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.omsDockManager1)).BeginInit();
            this.dockableWindow1.SuspendLayout();
            this.dockableWindow2.SuspendLayout();
            this.windowDockingArea2.SuspendLayout();
            this._ucBaseAddinAutoHideControl.SuspendLayout();
            this.pnDetails.SuspendLayout();
            this.pnlGrouping.SuspendLayout();
            this.ucNavPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(182)))), ((int)(((byte)(201)))));
            this.pnlDesign.Controls.Add(this.pnDetails);
            this.pnlDesign.Controls.Add(this.pnlGrouping);
            this.pnlDesign.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlDesign.Size = new System.Drawing.Size(168, 488);
            this.pnlDesign.Controls.SetChildIndex(this.pnlActions, 0);
            this.pnlDesign.Controls.SetChildIndex(this.pnlGrouping, 0);
            this.pnlDesign.Controls.SetChildIndex(this.pnDetails, 0);
            // 
            // pnlActions
            // 
            this.pnlActions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Visible = true;
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // _matterFoldersTree
            // 
            this._matterFoldersTree.AllowDrop = true;
            this._matterFoldersTree.CheckBoxes = true;
            this._matterFoldersTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this._matterFoldersTree.Location = new System.Drawing.Point(0, 0);
            this._matterFoldersTree.Name = "_matterFoldersTree";
            this._matterFoldersTree.Size = new System.Drawing.Size(200, 280);
            this._matterFoldersTree.TabIndex = 0;
            this._matterFoldersTree.NodeCheckedChanged += new FWBS.OMS.UI.Windows.ucMatterFoldersTree.NodeCheckedChangedEventHandler(this._matterFoldersTree_NodeCheckedChanged);
            this._matterFoldersTree.FoldersDrop += new FWBS.OMS.UI.Windows.ucMatterFoldersTree.FoldersDropEventHandler(this.MatterFoldersTreeFoldersDrop);
            // 
            // documentPreviewer1
            // 
            this.documentPreviewer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.documentPreviewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentPreviewer1.Location = new System.Drawing.Point(0, 18);
            this.documentPreviewer1.Name = "documentPreviewer1";
            this.documentPreviewer1.Padding = new System.Windows.Forms.Padding(5);
            this.documentPreviewer1.Size = new System.Drawing.Size(328, 470);
            this.documentPreviewer1.TabIndex = 0;
            this.documentPreviewer1.ToBeRefreshed = false;
            // 
            // omsDockManager1
            // 
            this.omsDockManager1.AnimationEnabled = false;
            this.omsDockManager1.AnimationSpeed = Infragistics.Win.UltraWinDock.AnimationSpeed.StandardSpeedPlus3;
            this.omsDockManager1.CompressUnpinnedTabs = false;
            this.omsDockManager1.DefaultPaneSettings.AllowClose = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.DefaultPaneSettings.AllowDockAsTab = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.DefaultPaneSettings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.DefaultPaneSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.DefaultPaneSettings.AllowMaximize = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.DefaultPaneSettings.AllowMinimize = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.DefaultPaneSettings.DoubleClickAction = Infragistics.Win.UltraWinDock.PaneDoubleClickAction.None;
            dockAreaPane1.DockedBefore = new System.Guid("41805f65-7924-466f-b595-233b7a5cdaa7");
            dockableControlPane1.Control = this._matterFoldersTree;
            dockableControlPane1.FlyoutSize = new System.Drawing.Size(328, -1);
            dockableControlPane1.Key = "DOCTREEVIEW";
            dockableControlPane1.OriginalControlBounds = new System.Drawing.Rectangle(384, -27, 336, 488);
            dockableControlPane1.Pinned = false;
            dockableControlPane1.Settings.AllowClose = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane1.Settings.AllowDockAsTab = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane1.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane1.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane1.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.True;
            dockableControlPane1.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane1.Settings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane1.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane1.Settings.AllowMaximize = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane1.Settings.AllowMinimize = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane1.Settings.AllowPin = Infragistics.Win.DefaultableBoolean.True;
            dockableControlPane1.Settings.AllowResize = Infragistics.Win.DefaultableBoolean.True;
            dockableControlPane1.Size = new System.Drawing.Size(100, 100);
            dockableControlPane1.Text = "Document Folders";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1});
            dockAreaPane1.Size = new System.Drawing.Size(328, 488);
            dockableControlPane2.Control = this.documentPreviewer1;
            dockableControlPane2.FlyoutSize = new System.Drawing.Size(328, -1);
            dockableControlPane2.Key = "DOCPREVIEW";
            dockableControlPane2.OriginalControlBounds = new System.Drawing.Rectangle(384, -27, 336, 488);
            dockableControlPane2.Pinned = false;
            dockableControlPane2.Settings.AllowClose = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane2.Settings.AllowDockAsTab = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane2.Settings.AllowDockBottom = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane2.Settings.AllowDockLeft = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane2.Settings.AllowDockRight = Infragistics.Win.DefaultableBoolean.True;
            dockableControlPane2.Settings.AllowDockTop = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane2.Settings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane2.Settings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane2.Settings.AllowMaximize = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane2.Settings.AllowMinimize = Infragistics.Win.DefaultableBoolean.False;
            dockableControlPane2.Settings.AllowPin = Infragistics.Win.DefaultableBoolean.True;
            dockableControlPane2.Settings.AllowResize = Infragistics.Win.DefaultableBoolean.True;
            dockableControlPane2.Size = new System.Drawing.Size(100, 100);
            dockableControlPane2.Text = "Document Preview";
            dockAreaPane2.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane2});
            dockAreaPane2.Size = new System.Drawing.Size(328, 488);
            this.omsDockManager1.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1,
            dockAreaPane2});
            this.omsDockManager1.HostControl = this;
            this.omsDockManager1.SettingsKey = "";
            this.omsDockManager1.ShowCloseButton = false;
            this.omsDockManager1.ShowDisabledButtons = false;
            this.omsDockManager1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.PaneDisplayed += new Infragistics.Win.UltraWinDock.PaneDisplayedEventHandler(this.omsDockManager1_PaneDisplayed);
            this.omsDockManager1.PaneHidden += new Infragistics.Win.UltraWinDock.PaneHiddenEventHandler(this.omsDockManager1_PaneHidden);
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this._matterFoldersTree);
            this.dockableWindow1.Location = new System.Drawing.Point(5, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.omsDockManager1;
            this.dockableWindow1.Size = new System.Drawing.Size(328, 488);
            this.dockableWindow1.TabIndex = 16;
            // 
            // dockableWindow2
            // 
            this.dockableWindow2.Controls.Add(this.documentPreviewer1);
            this.dockableWindow2.Location = new System.Drawing.Point(5, 0);
            this.dockableWindow2.Name = "dockableWindow2";
            this.dockableWindow2.Owner = this.omsDockManager1;
            this.dockableWindow2.Size = new System.Drawing.Size(328, 488);
            this.dockableWindow2.TabIndex = 17;
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Location = new System.Drawing.Point(542, 0);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.omsDockManager1;
            this.windowDockingArea1.Size = new System.Drawing.Size(333, 488);
            this.windowDockingArea1.TabIndex = 15;
            this.windowDockingArea1.Visible = false;
            // 
            // windowDockingArea2
            // 
            this.windowDockingArea2.Controls.Add(this.dockableWindow2);
            this.windowDockingArea2.Location = new System.Drawing.Point(542, 0);
            this.windowDockingArea2.Name = "windowDockingArea2";
            this.windowDockingArea2.Owner = this.omsDockManager1;
            this.windowDockingArea2.Size = new System.Drawing.Size(333, 488);
            this.windowDockingArea2.TabIndex = 18;
            this.windowDockingArea2.Visible = false;
            // 
            // _ucBaseAddinUnpinnedTabAreaLeft
            // 
            this._ucBaseAddinUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._ucBaseAddinUnpinnedTabAreaLeft.Location = new System.Drawing.Point(0, 0);
            this._ucBaseAddinUnpinnedTabAreaLeft.Name = "_ucBaseAddinUnpinnedTabAreaLeft";
            this._ucBaseAddinUnpinnedTabAreaLeft.Owner = this.omsDockManager1;
            this._ucBaseAddinUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 488);
            this._ucBaseAddinUnpinnedTabAreaLeft.TabIndex = 10;
            // 
            // _ucBaseAddinUnpinnedTabAreaRight
            // 
            this._ucBaseAddinUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._ucBaseAddinUnpinnedTabAreaRight.Location = new System.Drawing.Point(875, 0);
            this._ucBaseAddinUnpinnedTabAreaRight.Name = "_ucBaseAddinUnpinnedTabAreaRight";
            this._ucBaseAddinUnpinnedTabAreaRight.Owner = this.omsDockManager1;
            this._ucBaseAddinUnpinnedTabAreaRight.Size = new System.Drawing.Size(21, 488);
            this._ucBaseAddinUnpinnedTabAreaRight.TabIndex = 11;
            // 
            // _ucBaseAddinUnpinnedTabAreaTop
            // 
            this._ucBaseAddinUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._ucBaseAddinUnpinnedTabAreaTop.Location = new System.Drawing.Point(0, 0);
            this._ucBaseAddinUnpinnedTabAreaTop.Name = "_ucBaseAddinUnpinnedTabAreaTop";
            this._ucBaseAddinUnpinnedTabAreaTop.Owner = this.omsDockManager1;
            this._ucBaseAddinUnpinnedTabAreaTop.Size = new System.Drawing.Size(875, 0);
            this._ucBaseAddinUnpinnedTabAreaTop.TabIndex = 12;
            // 
            // _ucBaseAddinUnpinnedTabAreaBottom
            // 
            this._ucBaseAddinUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ucBaseAddinUnpinnedTabAreaBottom.Location = new System.Drawing.Point(0, 488);
            this._ucBaseAddinUnpinnedTabAreaBottom.Name = "_ucBaseAddinUnpinnedTabAreaBottom";
            this._ucBaseAddinUnpinnedTabAreaBottom.Owner = this.omsDockManager1;
            this._ucBaseAddinUnpinnedTabAreaBottom.Size = new System.Drawing.Size(875, 0);
            this._ucBaseAddinUnpinnedTabAreaBottom.TabIndex = 13;
            // 
            // _ucBaseAddinAutoHideControl
            // 
            this._ucBaseAddinAutoHideControl.Controls.Add(this.dockableWindow1);
            this._ucBaseAddinAutoHideControl.Location = new System.Drawing.Point(842, 0);
            this._ucBaseAddinAutoHideControl.Name = "_ucBaseAddinAutoHideControl";
            this._ucBaseAddinAutoHideControl.Owner = this.omsDockManager1;
            this._ucBaseAddinAutoHideControl.Size = new System.Drawing.Size(33, 488);
            this._ucBaseAddinAutoHideControl.TabIndex = 14;
            // 
            // ucSearchControl1
            // 
            this.ucSearchControl1.BackColor = System.Drawing.Color.White;
            this.ucSearchControl1.BackGroundColor = System.Drawing.Color.White;
            this.ucSearchControl1.ButtonPanelVisible = false;
            this.ucSearchControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucSearchControl1.DoubleClickAction = "None";
            this.ucSearchControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucSearchControl1.Location = new System.Drawing.Point(0, 0);
            this.ucSearchControl1.Margin = new System.Windows.Forms.Padding(0);
            this.ucSearchControl1.Name = "ucSearchControl1";
            this.ucSearchControl1.NavCommandPanel = this.navCommands;
            this.ucSearchControl1.Padding = new System.Windows.Forms.Padding(5);
            this.ucSearchControl1.SearchListCode = "";
            this.ucSearchControl1.SearchListType = "";
            this.ucSearchControl1.Size = new System.Drawing.Size(875, 488);
            this.ucSearchControl1.TabIndex = 8;
            this.ucSearchControl1.ToBeRefreshed = false;
            this.ucSearchControl1.SearchButtonCommands += new FWBS.OMS.UI.Windows.SearchButtonEventHandler(this.ucSearchControl1_SearchButtonCommands);
            this.ucSearchControl1.SearchTypeChanged += new System.EventHandler(this.ucSearchControl1_SearchTypeChanged);
            this.ucSearchControl1.ItemHover += new FWBS.OMS.UI.Windows.SearchItemHoverEventHandler(this.ucSearchControl1_ItemHover);
            this.ucSearchControl1.ItemHovered += new System.EventHandler(this.ucSearchControl1_ItemHovered);
            this.ucSearchControl1.SearchCompleted += new FWBS.OMS.UI.Windows.SearchCompletedEventHandler(this.ucSearchControl1_SearchCompleted_1);
            this.ucSearchControl1.SearchListLoad += new System.EventHandler(this.ucSearchControl1_SearchListLoad);
            this.ucSearchControl1.FilterChanged += new System.EventHandler(this.ucSearchControl1_FilterChanged);

            this.ucSearchControl1.dgSearchResults.AllowDrop = true;
            this.ucSearchControl1.dgSearchResults.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgSearchResults_MouseDown);
            this.ucSearchControl1.dgSearchResults.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgSearchResults_MouseMove);
            this.ucSearchControl1.dgSearchResults.DragOver += new System.Windows.Forms.DragEventHandler(this.dgSearchResults_DragOver);
            // 
            // mnuOpenDoc
            // 
            this.mnuOpenDoc.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOpen,
            this.mnuOpenAndClose});
            // 
            // mnuOpen
            // 
            this.mnuOpen.Index = 0;
            this.mnuOpen.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOpenLatest,
            this.mnuOpenVersion});
            this.mnuOpen.Text = "Open";
            // 
            // mnuOpenLatest
            // 
            this.mnuOpenLatest.Index = 0;
            this.mnuOpenLatest.Tag = "cmdOpen";
            this.mnuOpenLatest.Text = "Latest";
            this.mnuOpenLatest.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuOpenVersion
            // 
            this.mnuOpenVersion.Index = 1;
            this.mnuOpenVersion.Tag = "cmdOpenVersion";
            this.mnuOpenVersion.Text = "Specific Version ...";
            this.mnuOpenVersion.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuOpenAndClose
            // 
            this.mnuOpenAndClose.Index = 1;
            this.mnuOpenAndClose.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.mnuOpenLatestAndClose,
            this.mnuOpenVersionAndClose});
            this.mnuOpenAndClose.Text = "Open && Close";
            // 
            // mnuOpenLatestAndClose
            // 
            this.mnuOpenLatestAndClose.Index = 0;
            this.mnuOpenLatestAndClose.Tag = "cmdOpen&Close";
            this.mnuOpenLatestAndClose.Text = "Latest";
            this.mnuOpenLatestAndClose.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // mnuOpenVersionAndClose
            // 
            this.mnuOpenVersionAndClose.Index = 1;
            this.mnuOpenVersionAndClose.Tag = "cmdOpenVersion&Close";
            this.mnuOpenVersionAndClose.Text = "Specific Version ...";
            this.mnuOpenVersionAndClose.Click += new System.EventHandler(this.MenuItem_Click);
            // 
            // icons
            // 
            this.icons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.icons.ImageSize = new System.Drawing.Size(16, 16);
            this.icons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Tick");
            this.imageList1.Images.SetKeyName(1, "Person");
            this.imageList1.Images.SetKeyName(2, "HighQ");
            // 
            // pnDetails
            // 
            this.pnDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnDetails.Controls.Add(this.ucNavRichText1);
            this.pnDetails.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnDetails.ExpandedHeight = 58;
            this.pnDetails.HeaderColor = System.Drawing.Color.Empty;
            this.pnDetails.Location = new System.Drawing.Point(8, 211);
            this.resourceLookup1.SetLookup(this.pnDetails, new FWBS.OMS.UI.Windows.ResourceLookupItem("DOCDETAILS", "Document Details", ""));
            this.pnDetails.Name = "pnDetails";
            this.pnDetails.Size = new System.Drawing.Size(152, 58);
            this.pnDetails.TabIndex = 3;
            this.pnDetails.TabStop = false;
            this.pnDetails.Text = "Document Details";
            // 
            // ucNavRichText1
            // 
            this.ucNavRichText1.ModernStyle = true;
            this.ucNavRichText1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavRichText1.Location = new System.Drawing.Point(0, 24);
            this.ucNavRichText1.Name = "ucNavRichText1";
            this.ucNavRichText1.Padding = new System.Windows.Forms.Padding(3);
            this.ucNavRichText1.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavRichText1.Rtf = "{\\rtf1\\ansi\\ansicpg1252\\deff0\\deflang2057{\\fonttbl{\\f0\\fnil\\fcharset0 Segoe UI;" +
    "}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs18\\par\r\n}\r\n";
            this.ucNavRichText1.Size = new System.Drawing.Size(152, 27);
            this.ucNavRichText1.TabIndex = 15;
            // 
            // DocumentGrouping
            // 
            this.DocumentGrouping.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DocumentGrouping.Grouping = "DOCGROUPING";
            this.DocumentGrouping.Location = new System.Drawing.Point(0, 0);
            this.DocumentGrouping.Margin = new System.Windows.Forms.Padding(0);
            this.DocumentGrouping.Name = "DocumentGrouping";
            this.DocumentGrouping.Size = new System.Drawing.Size(152, 140);
            this.DocumentGrouping.TabIndex = 9;
            // 
            // pnlGrouping
            // 
            this.pnlGrouping.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlGrouping.Controls.Add(this.ucNavPanel2);
            this.pnlGrouping.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlGrouping.ExpandedHeight = 172;
            this.pnlGrouping.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.pnlGrouping.HeaderColor = System.Drawing.Color.Empty;
            this.pnlGrouping.Location = new System.Drawing.Point(8, 39);
            this.resourceLookup1.SetLookup(this.pnlGrouping, new FWBS.OMS.UI.Windows.ResourceLookupItem("DOCFILTERING", "Document Filtering", ""));
            this.pnlGrouping.Margin = new System.Windows.Forms.Padding(0, 0, 0, 3);
            this.pnlGrouping.Name = "pnlGrouping";
            this.pnlGrouping.Size = new System.Drawing.Size(152, 172);
            this.pnlGrouping.TabIndex = 4;
            this.pnlGrouping.TabStop = false;
            this.pnlGrouping.Text = "Document Filtering";
            // 
            // ucNavPanel2
            // 
            this.ucNavPanel2.Controls.Add(this.DocumentGrouping);
            this.ucNavPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucNavPanel2.Location = new System.Drawing.Point(0, 24);
            this.ucNavPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.ucNavPanel2.Name = "ucNavPanel2";
            this.ucNavPanel2.Padding = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.ucNavPanel2.PanelBackColor = System.Drawing.Color.Empty;
            this.ucNavPanel2.Size = new System.Drawing.Size(152, 141);
            this.ucNavPanel2.TabIndex = 15;
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerExecCommand
            // 
            this.timerExecCommand.Interval = 500;
            this.timerExecCommand.Tick += new System.EventHandler(this.timerExecCommand_Tick);
            // 
            // ucBuiltInDocuments
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this._ucBaseAddinAutoHideControl);
            this.Controls.Add(this.ucSearchControl1);
            this.Controls.Add(this.windowDockingArea1);
            this.Controls.Add(this.windowDockingArea2);
            this.Controls.Add(this._ucBaseAddinUnpinnedTabAreaTop);
            this.Controls.Add(this._ucBaseAddinUnpinnedTabAreaBottom);
            this.Controls.Add(this._ucBaseAddinUnpinnedTabAreaLeft);
            this.Controls.Add(this._ucBaseAddinUnpinnedTabAreaRight);
            this.Name = "ucBuiltInDocuments";
            this.Size = new System.Drawing.Size(896, 488);
            this.Controls.SetChildIndex(this._ucBaseAddinUnpinnedTabAreaRight, 0);
            this.Controls.SetChildIndex(this._ucBaseAddinUnpinnedTabAreaLeft, 0);
            this.Controls.SetChildIndex(this._ucBaseAddinUnpinnedTabAreaBottom, 0);
            this.Controls.SetChildIndex(this._ucBaseAddinUnpinnedTabAreaTop, 0);
            this.Controls.SetChildIndex(this.windowDockingArea2, 0);
            this.Controls.SetChildIndex(this.windowDockingArea1, 0);
            this.Controls.SetChildIndex(this.ucSearchControl1, 0);
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this._ucBaseAddinAutoHideControl, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.omsDockManager1)).EndInit();
            this.dockableWindow1.ResumeLayout(false);
            this.dockableWindow2.ResumeLayout(false);
            this.windowDockingArea2.ResumeLayout(false);
            this._ucBaseAddinAutoHideControl.ResumeLayout(false);
            this.pnDetails.ResumeLayout(false);
            this.pnlGrouping.ResumeLayout(false);
            this.ucNavPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

        #endregion

        #endregion

        #region Properties

        private string SearchListCode
		{
			get
			{
				return _searchlistcode;
			}
			set
			{
				_searchlistcode = value;
			}
		}

				
		#endregion

		#region IOMSTypeAddin Implementation

		/// <summary>
		/// Allows the calling OMS dialog to connect to the addin for the configurable type object.
		/// </summary>
		/// <param name="obj">OMS Configurable type object to use.</param>
		/// <returns>A flag that tells the dialogthat the connection has been successfull.</returns>
		public override bool Connect(IOMSType obj)
		{
			Debug.WriteLine("ucBuiltInDocuments Connect...");

			// Run firsttime connect.
			FinishFormConstruction();

			_type = obj;

			if (obj != null)
			{
				_file = obj as OMSFile;
				if (_file == null)
					_client = obj as Client;
				
				if (_file != null && _client == null)
					_client = _file.Client;

				_contact = obj as Contact;
				
				if (Session.CurrentSession.IsLoggedIn)
				{
					if (_file != null)
						_searchlistcode = Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.Document);
					else if (_client != null)
						_searchlistcode = Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.DocumentClient);
					else if (_contact != null)
						_searchlistcode = Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.DocumentContact);
				}
			}

			ucSearchControl1.ButtonEnabledRulesApplied -= new EventHandler(ucSearchControl1_ButtonEnabledRulesApplied);
			ucSearchControl1.ButtonEnabledRulesApplied += new EventHandler(ucSearchControl1_ButtonEnabledRulesApplied);

			ucSearchControl1.NewOMSTypeWindow -=new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
			ucSearchControl1.NewOMSTypeWindow +=new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);

			ucSearchControl1.SearchCompleted -= new SearchCompletedEventHandler(ucSearchControl1_SearchCompleted);
			ucSearchControl1.SearchCompleted +=new SearchCompletedEventHandler(ucSearchControl1_SearchCompleted);
			ToBeRefreshed=true;

			Debug.WriteLine("ucBuildInDocuments Grouping Start Init!");
			DocumentGrouping.Initialise();
			if (DocumentGrouping.FoundGroups)
			{
				pnlGrouping.Visible = true;
				DocumentGrouping.GroupingChanged -= new ValueChangedEventHandler(DocumentGrouping_GroupingChanged);
				DocumentGrouping.GroupingChanged += new ValueChangedEventHandler(DocumentGrouping_GroupingChanged);
				DocumentGrouping.FilterBuilt -= new EventHandler(DocumentGrouping_FilterBuilt);
				DocumentGrouping.FilterBuilt += new EventHandler(DocumentGrouping_FilterBuilt);
			}
			else
				pnlGrouping.Visible = false;

			Debug.WriteLine("ucBuildInDocuments Grouping End Init!");

			Debug.WriteLine("ucBuiltInDocuments Connect");


			if (_file != null && this.ParentForm is FWBS.OMS.UI.Windows.frmOMSTypeV2)
			{
                SetDockingFormat();
				_matterFoldersTree.InitializeTreeView(_file);
			}
			else
			{
				RemoveTreeViewFromAddin();
			}

			return true;
		}


		private void RemoveTreeViewFromAddin()
		{
			dockableWindow1.Dispose();
			dockableControlPane1.Dispose();
		}


		private void DocumentGrouping_FilterBuilt(object sender, EventArgs e)
		{
			SetExternalFilter(DocumentGrouping.Filter);
		}


		internal void SetExternalFilter(string filter)
		{
			this.Cursor = Cursors.WaitCursor;
			ucSearchControl1.ExternalFilter = filter;
			this.Cursor = Cursors.Arrow;
		}


		private Dictionary<string, List<object>> populatedGroups = new Dictionary<string, List<object>>();

		void DocumentGrouping_GroupingChanged(object sender, ValueChangedEventArgs e)
		{
			string column = e.ProposedValue as string;

			if (populatedGroups.ContainsKey(column))
			{
				DocumentGrouping.SetGroups(populatedGroups[column], column);
				return;
			}

			List<object> groups = GetGroupElements(column);

			populatedGroups.Add(column, groups);
			DocumentGrouping.SetGroups(groups, column);

		}

		internal List<object> GetGroupElements(string column)
		{
			List<object> groups = new List<object>();

			if (ucSearchControl1.DataTable.Columns.Contains(column))
			{
				foreach (DataRow row in ucSearchControl1.DataTable.Rows)
				{
					object value = row[column];

					if (value == DBNull.Value)
						value = null;

					if (!groups.Contains(value))
					{
						groups.Add(value);
					}
				}
			}
			//Add a Sort to the group as this was rendering in the order returned by the search list, and not alpha.  - WI 2998
			groups.Sort();
			return groups;
		}


		/// <summary>
		/// Refreshes the addin visual look and data contents.
		/// </summary>
		public override void RefreshItem()
		{
			if (ToBeRefreshed)
			{
				if (ucSearchControl1.SearchList == null || !ucSearchControl1.SearchList.SearchListType.Equals(_searchlistcode) || ucSearchControl1.SearchList.Parent != _type)
				{
					Debug.WriteLine("ucBuiltInDocuments SetSearchListType");
					ucSearchControl1.SetSearchListType(_searchlistcode, _type, parameters);
					Debug.WriteLine("ucBuiltInDocuments End SetSearchListType");
				}

				Debug.WriteLine("ucBuiltInDocuments ShowPanelButtons");
				ucSearchControl1.ShowPanelButtons();
				Debug.WriteLine("ucBuiltInDocuments End ShowPanelButtons");
				if (ucSearchControl1.SearchList.Style == SearchEngine.SearchListStyle.Search)
				{
					ucSearchControl1_SearchCompleted(ucSearchControl1, new SearchCompletedEventArgs(0, string.Empty));
				}
				else
				{
					Debug.WriteLine("ucBuiltInDocuments Search");
					ucSearchControl1.Search();
					Debug.WriteLine("ucBuiltInDocuments end Search");
				}
				Debug.WriteLine("ucBuiltInDocuments NavCommands");
				navCommands.Refresh();
				Debug.WriteLine("ucBuiltInDocuments and Navcommands");

			}

			Debug.WriteLine("ucBuiltInDocuments AttachCustomButtons");
			AttachCustomButtons();
			Debug.WriteLine("ucBuiltInDocuments End AttachCustomButtons");
			
			ToBeRefreshed=false;
		}

		private void AttachCellDisplayEvents()
		{
			if (_highlightOptions.Enabled && (_highlightOptions.HighlightHidden || _highlightOptions.HighlightVisible))
			{
                foreach (DataGridViewColumn col in ucSearchControl1.Columns)
                {
                    var column = col as IBeforeCellDisplayable;
                    if (column != null)
                    {
                        column.BeforeCellDisplayEvent += col_BeforeCellDisplayEvent;
                    }
                }
            }
		}

		void col_BeforeCellDisplayEvent(object sender, Common.UI.Windows.CellDisplayEventArgs e)
		{
			if (e.Row.Row.Table.Columns.Contains("SecurityOptions"))
			{
				object obj = e.Row["SecurityOptions"];
				if (obj == null)
					return;

				FWBS.OMS.Security.SecurityOptions options = (FWBS.OMS.Security.SecurityOptions)obj;
				if (options.HasFlag(FWBS.OMS.Security.SecurityOptions.IsExternallyVisible) && _highlightOptions.HighlightVisible)
				{
					e.BackColor = _highlightOptions.VisibleColor;
				}
				
				if (!options.HasFlag(FWBS.OMS.Security.SecurityOptions.IsExternallyVisible) && _highlightOptions.HighlightHidden)
				{
					e.BackColor = _highlightOptions.HiddenColor;
				}
			}
		}
		
		public override void SelectItem()
		{
			if (ucSearchControl1.SearchList == null)
			{
				ucSearchControl1.SetSearchList(_searchlistcode,false,_type, null);
			}

			AttachCustomButtons();

			if (ucSearchControl1.SearchList.Style == FWBS.OMS.SearchEngine.SearchListStyle.List)
			{
				ucSearchControl1.Search();
			}

			if (ucSearchControl1.SearchList != null)
				ucSearchControl1.ShowPanelButtons();
			
			BuildExtendedButtons();

		}

		private void AttachCustomButtons()
		{
			_inoutdoc = SetButtonStyle("cmdInOut", ToolBarButtonStyle.ToggleButton);
			_indoc = SetButtonStyle("cmdInward", ToolBarButtonStyle.ToggleButton); 
			_outdoc = SetButtonStyle("cmdOutward", ToolBarButtonStyle.ToggleButton);
			if (_inoutdoc != null)
				_inoutdoc.Pushed = true;

			try
			{
				_localall = SetButtonStyle("cmdLocalAll", ToolBarButtonStyle.ToggleButton);
				_localchanged = SetButtonStyle("cmdLocalChanged", ToolBarButtonStyle.ToggleButton);
				_localunchanged = SetButtonStyle("cmdLocalUnchanged", ToolBarButtonStyle.ToggleButton);
				if (_localall != null)
					_localall.Pushed = true;
			}
			catch { }
		}

		private OMSToolBarButton SetButtonStyle(string name, ToolBarButtonStyle style)
		{
			OMSToolBarButton but = ucSearchControl1.GetOMSToolBarButton(name);
			if (but == null)
				return null;

			but.Style = style;
			return but;
		}

		#endregion

		#region Actions

		private bool runCommandExecuting = false;
		private string RunCommand(string command)
		{
			List<OMSDocument> docs = new List<OMSDocument>();
			System.Text.StringBuilder exmsg = new System.Text.StringBuilder();
			System.Text.StringBuilder infomsg = new System.Text.StringBuilder();
			DocumentAddinCommand cmd = null;
			DocumentAddinPreCommand precmd = null;

			switch (command.ToUpperInvariant())
			{
				case  "CMDLOCALOPEN":
					precmd = new DocumentAddinPreCommand(LocalOpenCommand);
					break;
				case "CMDRECEIVE":
					precmd = new DocumentAddinPreCommand(ReceiptCommand);
					break;
				case "CMDSMS":
					precmd = new DocumentAddinPreCommand(SMSCommand);
					break;
				case "CMDLOCALALL":
					precmd = new DocumentAddinPreCommand(LocalAllFilterCommand);
					break;
				case "CMDLOCALUNCHANGED":
					precmd = new DocumentAddinPreCommand(LocalUnchangedFilterCommand);
					break;
				case "CMDLOCALCHANGED":
					precmd = new DocumentAddinPreCommand(LocalChangedFilterCommand);
					break;
				case "CMDINOUT":
					precmd = new DocumentAddinPreCommand(InOutFilterCommand);
					break;
				case "CMDOUTWARD":
					precmd = new DocumentAddinPreCommand(OutwardFilterCommand);
					break;
				case "CMDINWARD":
					precmd = new DocumentAddinPreCommand(InwardFilterCommand);
					break;
			}

			if (precmd == null)
			{

                if (CommandRequiresDocuments(command))
                {
                    if (ucSearchControl1.SearchList.ResultCount > 0)
                    {
                        try
                        {
                            Common.KeyValueCollection[] ret = ucSearchControl1.SelectedItems;

                            for (int ctr = 0; ctr < ret.Length; ctr++)
                            {
                                Common.KeyValueCollection val = ret[ctr];
                                try
                                {
                                    docs.Add(OMSDocument.GetUncachedDocument((long)val["DOCID"].Value));
                                }
                                catch (Exception ex)
                                {
                                    exmsg.AppendLine(ex.Message);
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            exmsg.AppendLine(ex.Message);
                        }
                    } 
                }

                switch (command.ToUpperInvariant())
				{
					case "CMDCOPY":
						cmd = new DocumentAddinCommand(CopyCommand);
						break;
					case "CMDMOVE":
						cmd = new DocumentAddinCommand(MoveCommand);
						break;
					case "CMDOPEN":
						cmd = new DocumentAddinCommand(OpenCommand);
						break;
					case "CMDOPENVERSION":
						cmd = new DocumentAddinCommand(OpenVersionCommand);
						break;
					case "CMDOPEN&CLOSE":
						cmd = new DocumentAddinCommand(OpenAndCloseCommand);
						closingForm = true;
						break;
					case "CMDOPENVERSION&CLOSE":
						cmd = new DocumentAddinCommand(OpenVersionAndCloseCommand);
						break;
					case "CMDPROPERTIES":
						cmd = new DocumentAddinCommand(PropertiesCommand);
						break;
					case "CMDPRINT":
					 cmd = new DocumentAddinCommand(PrintCommand);                 
						break;
					case "CMDBULKPRINT":
						cmd = new DocumentAddinCommand(BulkPrintCommand);   
						break;   					

					case "CMDDELETE":
						cmd = new DocumentAddinCommand(DeleteCommand);
						break;
					case "CMDEMAIL":
						cmd = new DocumentAddinCommand(EmailCommand);
						break;
					case "CMDEMAILPDF":
						cmd = new DocumentAddinCommand(EmailCommandPDF);
						break;
					case "CMDEMAILVERSIONPDF":
						cmd = new DocumentAddinCommand(EmailVersionCommandPDF);
						break;
					case "CMDEMAILVERSION":
						cmd = new DocumentAddinCommand(EmailVersionCommand);
						break;
					case "CMDSENDLINK":
						cmd = new DocumentAddinCommand(SendLinkCommand);
						break;
					case "CMDUNDOCHECKOUT":
						cmd = new DocumentAddinCommand(UndoCheckOutCommand);
						break;
					case "CMDCHECKOUT":
						cmd = new DocumentAddinCommand(CheckOutCommand);
						break;
					case "CMDEXPORT":
						cmd = new DocumentAddinCommand(ExportCommand);
						break;
					case "CMDIMPORT":
						cmd = new DocumentAddinCommand(ImportCommand);
						break;
					case "CMDIMPORTVER":
						cmd = new DocumentAddinCommand(ImportVersionCommand);
						break;
					case "CMDCHECKIN":
						cmd = new DocumentAddinCommand(CheckInCommand);
						break;
					case "CMDAUTHORISE":
						cmd = new DocumentAddinCommand(AuthoriseCommand);
						break;
					case "CMDSHOWEXT":
						cmd = new DocumentAddinCommand(ShowExternallyCommand);
						break;
					case "CMDHIDEEXT":
						cmd = new DocumentAddinCommand(HideExternallyCommand);
						break;
					case "CMDCREATEPDFBUNDLE":
						cmd = new DocumentAddinCommand(CreatePDFBundleCommand);
						break;
					case "CMDVIEWBUNDLES":
						cmd = new DocumentAddinCommand(ViewPDFBundlesCommand);
						break;
                    case "CMDHIGHQUPLOAD":
                        cmd = new DocumentAddinCommand(HighQUploadCommand);
                        break;
                }
			}

			if (precmd != null)
				precmd(exmsg, infomsg);

			if (cmd != null)
				cmd(docs, exmsg, infomsg);

			if (infomsg.Length > 0)
			{
				MessageBox.Show(infomsg.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			runCommandExecuting = true;
			timerExecCommand.Enabled = true;
			return exmsg.ToString();
		}

        private bool CommandRequiresDocuments(string commandName)
        {
            return (_file == null) || !commandName.Equals("CMDIMPORT", StringComparison.OrdinalIgnoreCase);
        }

		#endregion

		#region Commands

		private delegate void DocumentAddinPreCommand(System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg);
		private delegate void DocumentAddinCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg);

		private void LocalOpenCommand(System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			if (ucSearchControl1.SearchList.ResultCount > 0)
			{
				Common.KeyValueCollection[] ret = ucSearchControl1.SelectedItems;

				for (int ctr = 0; ctr < ret.Length; ctr++)
				{
					try
					{
						string file = Convert.ToString(ret[ctr]["FileLocalPath"].Value);
						if (System.IO.File.Exists(file) == false)
							throw new System.IO.FileNotFoundException("File does not exist.", file);
					    var args = string.Format("OPEN \"{0}\"", file);
					    Services.ProcessStart("OMS.UTILS.EXE", args, InputValidation.ValidateOpenFileInput);
					}
					catch (Exception ex)
					{
						exmsg.AppendLine(ex.Message);
					}
				}
			}
		}

        private void ReceiptCommand(System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			//this should not be needed as the button should not be enabled if the code in ActionsEnabled is firing correctly
			if (_file != null)
			{
				Associate assoc = _file.DefaultAssociate;
				PrecedentJob job = new PrecedentJob(Precedent.GetDefaultPrecedent("RECEIPT", assoc));
				job.Associate = assoc;
				Services.ProcessJob(null, job);
				if (job.HasError)
					exmsg.AppendLine(job.ErrorMessage);
			}
		}

		private void SMSCommand(System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			//this should not be needed as the button should not be enabled if the code in ActionsEnabled is firing correctly
			if (_file != null)
			{
				Associate assoc = _file.DefaultAssociate;
				PrecedentJob job = new PrecedentJob(Precedent.GetDefaultPrecedent("SMS", assoc));
				job.Associate = assoc;
				Services.ProcessJob(null, job);
				if (job.HasError)
					exmsg.AppendLine(job.ErrorMessage);
			}
		}

		private void LocalAllFilterCommand(System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			_localunchanged.Pushed = false;
			_localall.Pushed = true;
			_localchanged.Pushed = false;
			ucSearchControl1.ExternalFilter = "";
		}
		private void LocalUnchangedFilterCommand(System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{

			_localunchanged.Pushed = true;
			_localall.Pushed = false;
			_localchanged.Pushed = false;
			ucSearchControl1.ExternalFilter = "[HasChanged] = false";
		}

		private void LocalChangedFilterCommand(System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			_localunchanged.Pushed = false;
			_localall.Pushed = false;
			_localchanged.Pushed = true;
			ucSearchControl1.ExternalFilter = "[HasChanged] = true";
		}

		private void InOutFilterCommand(System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			_inoutdoc.Pushed = true;
			_indoc.Pushed = false;
			_outdoc.Pushed = false;
			ucSearchControl1.ExternalFilter = "";
		}

		private void OutwardFilterCommand(System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			_inoutdoc.Pushed = false;
			_outdoc.Pushed = true;
			_indoc.Pushed = false;
			ucSearchControl1.ExternalFilter = "[docdirection] = 0";
		}

		 private void InwardFilterCommand(System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		 {
			 _indoc.Pushed = true;
			 _inoutdoc.Pushed = false;
			 _outdoc.Pushed = false;
			 ucSearchControl1.ExternalFilter = "[docdirection] = 1";
		 }

		private void OpenAndCloseCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			bool success = false;
			foreach (OMSDocument doc in docs)
			{
				if (doc != null)
				{
					try
					{
						FWBS.OMS.DocumentManagement.Storage.IStorageItemLockable lockableitem = doc.GetStorageProvider().GetLockableItem(doc);

						User checkedoutby = lockableitem.CheckedOutBy;
						if (checkedoutby != null && checkedoutby.ID != Session.CurrentSession.CurrentUser.ID)
						{
							infomsg.AppendLine(Session.CurrentSession.Resources.GetMessage("MSGDOCCHECKOUT", "Document %1% is already checked out by '%2%'", String.Empty, doc.DisplayID, checkedoutby.FullName).Text);
						}

						UnloadPreview();
						if (Services.OpenDocument(doc, DocOpenMode.Edit))
							success = true;
					}
					catch (Exception ex)
					{
						exmsg.AppendLine(ex.Message);
					}
				}
			}

			FWBS.OMS.UI.Windows.Services.CheckForUnsupportedFiles(docs, true);

			if (success)
			{
				if (this.Parent != null && this.ParentForm != null)
					this.ParentForm.Close();
			}
			else
				LoadPreview();
		}


		private void OpenVersionAndCloseCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			bool success = false;
			bool filetypecheck = false;

			if (docs.Count > 0)
			{
				try
				{
					FWBS.OMS.DocumentManagement.Storage.IStorageItemLockable lockableitem = docs[0].GetStorageProvider().GetLockableItem(docs[0]);

					User checkedoutby = lockableitem.CheckedOutBy;
					if (checkedoutby != null && checkedoutby.ID != Session.CurrentSession.CurrentUser.ID)
					{
						infomsg.AppendLine(Session.CurrentSession.Resources.GetMessage("MSGDOCCHECKOUT", "Document %1% is already checked out by '%2%'", String.Empty, docs[0].DisplayID, checkedoutby.FullName).Text);
					}
					else
						filetypecheck = true;

					UnloadPreview();
					if (Services.OpenDocument(docs[0], DocOpenMode.Edit, false))
						success = true;
				}
				catch (Exception ex)
				{
					exmsg.AppendLine(ex.Message);
				}

				if (filetypecheck)
				{
					List<OMSDocument> docList = new List<OMSDocument>() { docs[0] };
					FWBS.OMS.UI.Windows.Services.CheckForUnsupportedFiles(docList, true);
				}

				if (success)
				{
					if (this.Parent != null && this.ParentForm != null)
						this.ParentForm.Close();
				}
				else
					LoadPreview();
			}
		}


		private void PropertiesCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			OMSDocument doc = docs[0];
			if (doc == null)
				return;

			OnNewOMSTypeWindow(this, new NewOMSTypeWindowEventArgs(doc));

		}
		private void PrintCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			UnloadPreview();
			foreach (OMSDocument doc in docs)
				if (doc != null) Services.OpenDocument(doc, DocOpenMode.Print);
	   }

		private void BulkPrintCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			UnloadPreview();

			int bulkcopies = 1;  // as default value
			bulkcopies = Common.ConvertDef.ToInt32(InputBox.Show(Session.CurrentSession.Resources.GetMessage("PRINTCOPIES", "How many copies would you like to print?", "").Text, "", bulkcopies.ToString()), 0);
			if (bulkcopies == 0) return;

			foreach (OMSDocument doc in docs)
			{
				if (doc != null) Services.OpenDocument(doc, DocOpenMode.Print, true, true, bulkcopies);
			}            
		}


		private void DeleteCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
            var deletedDocs = 0;
			if (MessageBox.Show(this.ParentForm, Session.CurrentSession.Resources.GetMessage("4009", "Are you sure that you would like to delete the specified documents?", ""), null, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
			{
				foreach (OMSDocument doc in docs)
				{
					try
					{
						if (doc != null)
						{
							doc.Delete();
                            deletedDocs++;                        
                        }
					}
					catch (Exception ex)
					{
						exmsg.AppendLine(ex.Message);
					}
				}
				ucSearchControl1.Search(deletedDocs);
			}
		}

		private void PreviewCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			OMSDocument doc = docs[0];
			documentPreviewer1.Connect(doc);
			documentPreviewer1.RefreshItem();
		}


		private void EmailCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			try
			{
				Services.SendDocViaEmail(this.ParentForm, docs.ToArray(), null);
			}
			catch (Exception ex)
			{
				exmsg.AppendLine(ex.Message);
			}
		}


		private void SendLinkCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{

			try
			{              
				string linkstring = null;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();
				string datainfo = FWBS.OMS.Session.CurrentSession.CurrentDatabase.DatabaseName;
				string bodystring = FWBS.OMS.CodeLookup.GetLookup("SENDDOCLINK", "ELINKBODY");                
				bodystring = bodystring.Replace("%DBINFO%", datainfo)+ "<br>";             
				sb.AppendLine(bodystring.ToString());
				string linkdesc = FWBS.OMS.CodeLookup.GetLookup("SENDDOCLINK", "ELINKDESC");

				foreach (OMSDocument doc in docs)
				{
					string docpath = doc.DisplayID + "." + doc.GetLatestVersion().Label;
					string docdesc= doc.Description;                    
					linkstring = "<a href=" + "omsmc:opendocument%20" + docpath + ">" + linkdesc.Replace("%DOCDESC%", docdesc) +"</a>" + "<br>";                     
					sb.AppendLine(linkstring.ToString());                     
				}

			  
				bodystring = sb.ToString();
				string subject = FWBS.OMS.CodeLookup.GetLookup("SENDDOCLINK", "ELINKSUB");

				//have to send a default associate otherwise it comes up with screen to pick one

				OMSDocument doc1 = docs[0];
				Associate assoc = doc1.OMSFile.DefaultAssociate;            
				
			   Services.SendDocLinkViaEmail(this.ParentForm, assoc,bodystring, subject);                       
				
				}              
					  
			catch (Exception ex)
			{
				exmsg.AppendLine(ex.Message);
			}

		}



		private void EmailCommandPDF(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			try
			{
				Services.SendDocViaEmail(this.ParentForm, docs.ToArray(), null,true);
			}
			catch (Exception ex)
			{
				exmsg.AppendLine(ex.Message);
			}
		}

		private void EmailVersionCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			try
			{
				DocumentVersionPicker picker = new DocumentVersionPicker();
				picker.Title = Session.CurrentSession.Resources.GetResource("MSGEMAILDOC", "Select Documents to Email", "").Text;
				picker.Document = docs[0];
				picker.MultiSelect = true;
				IStorageItemVersion[] selected = picker.Show(this.ParentForm);
				if (selected == null)
					return;

				Services.SendDocViaEmail(this.ParentForm, selected, null);
			}
			catch (Exception ex)
			{
				exmsg.AppendLine(ex.Message);
			}
		}

		private void EmailVersionCommandPDF(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			try
			{
				DocumentVersionPicker picker = new DocumentVersionPicker();
				picker.Title = Session.CurrentSession.Resources.GetResource("MSGEMAILDOC", "Select Documents to Email", "").Text;
				picker.Document = docs[0];
				picker.MultiSelect = true;
				IStorageItemVersion[] selected = picker.Show(this.ParentForm);
				if (selected == null)
					return;

				Services.SendDocViaEmail(this.ParentForm, selected, null, true);
			}
			catch (Exception ex)
			{
				exmsg.AppendLine(ex.Message);
			}
		}

		

		private void UndoCheckOutCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			try
			{
				UndoCheckout(docs.ToArray());
				ucSearchControl1.Search();
			}
			catch (Exception ex)
			{
				exmsg.AppendLine(ex.Message);
			}
		}
		private void CheckOutCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			try
			{
				Checkout(docs.ToArray());
				ucSearchControl1.Search();
			}
			catch (Exception ex)
			{
				exmsg.AppendLine(ex.Message);
			}
		}

		private void ExportCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			try
			{
				ExportDocumentCommand export = new ExportDocumentCommand();
                export.Documents.AddRange(docs.ToArray());
				export.Owner = this;
                FWBS.OMS.Commands.ExecuteResult res = export.Execute();

				foreach (Exception ex in res.Errors)
					exmsg.AppendLine(ex.Message);

                if (res.Status != Commands.CommandStatus.Canceled)
                {
                    int exportTotal = docs.Count - res.Errors.Count - export.ExistingDocumentsCount;
                    if (exportTotal == 0)
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation("DOCEXPTOTAL4", "No documents have been exported.");
                    else
                        FWBS.OMS.UI.Windows.MessageBox.ShowInformation("DOCEXPTOTAL3", "%1% document%2% successfully exported.", Convert.ToString(exportTotal), exportTotal > 1 ? "s were" : " was");
                }
			}
			catch (Exception ex)
			{
				exmsg.AppendLine(ex.Message);
			}
		}

		private void ImportCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			ImportDocumentCommand import = new ImportDocumentCommand();
			import.AllowUI = true;
			import.Owner = this;
			import.ToFile = _file;
			import.AutoConfirmFile = _file != null;

			Associate assoc = null;
			if (docs.Count == 0)
				assoc = _file.DefaultAssociate;
			else
				assoc = docs[0].OMSFile.DefaultAssociate;

			if (assoc == null)
			{
				throw new Exception("Could not determine OMSFile or Default Associate.  This is not supported from this area.");
			}

			if ((Session.CurrentSession.CurrentUser.UseDefaultAssociate == FWBS.Common.TriState.Null && Session.CurrentSession.UseDefaultAssociate == false) || Session.CurrentSession.CurrentUser.UseDefaultAssociate == FWBS.Common.TriState.False)
				import.ToAssociate =null;                
			else
				import.ToAssociate = assoc;
			
			
			FWBS.OMS.Commands.ExecuteResult res = import.Execute();

			foreach (Exception ex in res.Errors)
				exmsg.AppendLine(ex.Message);

            if (res.Status != Commands.CommandStatus.Canceled)
    			ucSearchControl1.Search();
		}

		private void ImportVersionCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			ImportDocumentCommand import = new ImportDocumentCommand();
			import.AllowUI = true;

			SaveSettings settings = SaveSettings.Default;

			VersionStoreSettings vers = new VersionStoreSettings();
			vers.SaveItemAs = VersionStoreSettings.StoreAs.NewMajorVersion;

			settings.StorageSettings.Add(vers);

			if (docs.Count <= 0)
			{
				exmsg.AppendLine("No parent document found");
				return;
			}

			import.Settings = settings;
			import.Parent = docs[0];
			import.Owner = this;
			FWBS.OMS.Commands.ExecuteResult res = import.Execute();

			foreach (Exception ex in res.Errors)
				exmsg.AppendLine(ex.Message);


		}

		private void CheckInCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			CheckInDocumentCommand checkin = new CheckInDocumentCommand();
			checkin.AllowUI = true;
			checkin.Docs.AddRange(docs.ToArray());
			checkin.Owner = this;
			checkin.ApplyDefaultVersioning = false;

			FWBS.OMS.Commands.ExecuteResult res = checkin.Execute();

            if (res.Status == Commands.CommandStatus.Success && res.Errors.Count == 0)
                ucSearchControl1.Search();

            foreach (Exception ex in res.Errors)
				exmsg.AppendLine(ex.Message);

		}
		private void OpenVersionCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			bool filetypecheck = false;
			if (docs.Count > 0)
			{
				try
				{
					FWBS.OMS.DocumentManagement.Storage.IStorageItemLockable lockableitem = docs[0].GetStorageProvider().GetLockableItem(docs[0]);

					User checkedoutby = lockableitem.CheckedOutBy;
					if (checkedoutby != null && checkedoutby.ID != Session.CurrentSession.CurrentUser.ID)
					{
						infomsg.AppendLine(Session.CurrentSession.Resources.GetMessage("MSGDOCCHECKOUT", "Document %1% is already checked out by '%2%'", String.Empty, docs[0].DisplayID, checkedoutby.FullName).Text);
					}
					else
						filetypecheck = true;

					UnloadPreview();
					if (Services.OpenDocument(docs[0], DocOpenMode.Edit, false))
						LoadPreview();

					if (filetypecheck)
					{
						List<OMSDocument> docList = new List<OMSDocument>() { docs[0] };
						FWBS.OMS.UI.Windows.Services.CheckForUnsupportedFiles(docList, true);
					}
				}
				catch (Exception ex)
				{
					exmsg.AppendLine(ex.Message);
				}
				finally
				{
				}

			}
		}
		private void CopyCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			FWBS.OMS.UI.Windows.DocumentManagement.CopyDocumentCommand cmd = new FWBS.OMS.UI.Windows.DocumentManagement.CopyDocumentCommand();
			cmd.AllowUI = true;
			cmd.Documents.AddRange(docs);
			cmd.DisplaySaveWizard = docs.Count == 1;
			cmd.DisplayAssociatePicker = Commands.DisplayWhen.ValueNotSpecified;
			cmd.Owner = this.ParentForm;
			cmd.ContinueOnError = true;

			try
			{
				Commands.ExecuteResult res = cmd.Execute();

				if (res.Status != Commands.CommandStatus.Canceled)
				{
					foreach (Exception ex in res.Errors)
					{
						exmsg.AppendLine(ex.Message);
					}
				}
			}
			catch (Exception ex)
			{
				exmsg.AppendLine(ex.Message);
			}

			ucSearchControl1.Search();
		}

		private void MoveCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			FWBS.OMS.UI.Windows.DocumentManagement.MoveDocumentCommand cmd = new FWBS.OMS.UI.Windows.DocumentManagement.MoveDocumentCommand();
			cmd.AllowUI = true;
			cmd.Documents.AddRange(docs);
			cmd.DisplayAssociatePicker = Commands.DisplayWhen.ValueNotSpecified;
			cmd.Owner = this.ParentForm;
			cmd.ContinueOnError = true;

			try
			{
				Commands.ExecuteResult res = cmd.Execute();

				if (res.Status != Commands.CommandStatus.Canceled)
				{
					foreach (Exception ex in res.Errors)
					{
						exmsg.AppendLine(ex.Message);
					}
				}
			}
			catch (Exception ex)
			{
				exmsg.AppendLine(ex.Message);
			}

			ucSearchControl1.Search();
		}

		private void OpenCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			bool filetypecheck = false;

			foreach (OMSDocument doc in docs)
			{
				try
				{
					if (doc != null)
					{
						FWBS.OMS.DocumentManagement.Storage.IStorageItemLockable lockableitem = doc.GetStorageProvider().GetLockableItem(doc);

						User checkedoutby = lockableitem.CheckedOutBy;
						if (checkedoutby != null && checkedoutby.ID != Session.CurrentSession.CurrentUser.ID)
						{
							infomsg.AppendLine(Session.CurrentSession.Resources.GetMessage("MSGDOCCHECKOUT", "Document %1% is already checked out by '%2%'", String.Empty, doc.DisplayID, checkedoutby.FullName).Text);
						}
						else
							filetypecheck = true;
						timer1.Enabled = false;
						UnloadPreview();
						if (Services.OpenDocument(doc, DocOpenMode.Edit))
							LoadPreview();
					}
				}
				catch (Exception ex)
				{
					exmsg.AppendLine(ex.Message);
				}
			}

			if(filetypecheck)
				FWBS.OMS.UI.Windows.Services.CheckForUnsupportedFiles(docs, true);

			bool minimise = false;
			switch (Session.CurrentSession.CurrentUser.MinimiseWindowOnOpen)
			{
				case FWBS.Common.TriState.True:
					minimise = true;
					break;
				case FWBS.Common.TriState.Null:
					minimise = Session.CurrentSession.MinimiseWindowOnOpen;
					break;
				default:
					minimise = false;
					break;
			}
			if (minimise)
			{
				if (this.ParentForm != null)
					this.ParentForm.WindowState = FormWindowState.Minimized;
			}

		}
		private void AuthoriseCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			if (docs.Count <= 0)
			{
				exmsg.AppendLine("No parent document found");
				return;
			}

			SendToAuthoriseCommand sendToAuth = new SendToAuthoriseCommand();
			sendToAuth.AllowUI = true;
			sendToAuth.ContinueOnError = false;
			sendToAuth.Item = docs[0];
			FWBS.OMS.Commands.ExecuteResult res = sendToAuth.Execute();
			foreach (Exception ex in res.Errors)
				exmsg.AppendLine(ex.Message);
		}

		private void ShowExternallyCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			SetVisibility(true, docs, exmsg, infomsg);
		}

		private void HideExternallyCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			SetVisibility(false, docs, exmsg, infomsg);
		}

		private void ViewPDFBundlesCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			OMSFile file = ucSearchControl1.SearchList.Parent as OMSFile;

			KeyValueCollection kvc = new KeyValueCollection();
			kvc.Add("fileID", file.ID);

			FWBS.OMS.UI.Windows.Services.ShowOMSItem("SFIDOCBUNDLESV", null, EnquiryEngine.EnquiryMode.Add, kvc);
		}

        private void HighQUploadCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
        {
            var highQ = HighQAdapter.GetHighQ();
            var command = new HighQDocumentUploadCommand(highQ)
            {
                Documents = docs
            };
            
            try
            {
                var rootFolder = highQ.GetRootFolderId(docs[0].ID);
                command.TargetFolderId = highQ.GetTargetFolderId(rootFolder, this);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            ucSearchControl1.EnableTimer();
            FWBS.OMS.Commands.ExecuteResult res = null;
            System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                res = command.Execute();
            }).ContinueWith(t =>
            {
                this.BeginInvoke((Action)delegate
                {
                    ucSearchControl1.HideProgress();

                    foreach (Exception ex in res.Errors)
                    {
                        exmsg.AppendLine(ex.Message);
                    }

                    var message = exmsg.ToString();
                    if (!string.IsNullOrEmpty(message))
                    {
                        FWBS.OMS.UI.Windows.MessageBox.Show(exmsg.ToString(), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (res.Status == Commands.CommandStatus.Success)
                    {
                        ucSearchControl1.Search();
                    }
                });
            });
        }

        private void CreatePDFBundleCommand(List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			CreatePDFBundle();
		}

		private void SetVisibility(bool visible, List<OMSDocument> docs, System.Text.StringBuilder exmsg, System.Text.StringBuilder infomsg)
		{
			FWBS.OMS.UI.DocumentManagement.SetDocumentExternalVisibilityCommand cmd = new UI.DocumentManagement.SetDocumentExternalVisibilityCommand();
			cmd.Documents.AddRange(docs);
			cmd.Visible = visible;
			cmd.ContinueOnError = true;

			try
			{
				Commands.ExecuteResult res = cmd.Execute();

				if (res.Status != Commands.CommandStatus.Canceled)
				{
					foreach (Exception ex in res.Errors)
					{
						exmsg.AppendLine(ex.Message);
					}
				}
			}
			catch (Exception ex)
			{
				exmsg.AppendLine(ex.Message);
			}

			ucSearchControl1.Search();
		}
		#endregion

		#region Methods

		private void SetDockingFormat()
		{
			var dockableControlConfiguration = new DockableControlConfiguration();

			dockableControlConfiguration.SetDockManagerStyle(omsDockManager1, new DockManagerSettings()
			{
				BackColor = Color.Transparent,
				BorderColor = Color.Transparent
			});

			SetupDockableControlPane(dockableControlPane1, Session.CurrentSession.Resources.GetResource("DOCFOLDERS", "Document Folders", "").Text, dockableControlConfiguration);
			SetupDockableControlPane(dockableControlPane2, Session.CurrentSession.Resources.GetResource("DOCPREVIEW", "Document Preview", "").Text, dockableControlConfiguration);
		}


		private void SetupDockableControlPane(Infragistics.Win.UltraWinDock.DockablePaneBase pane, string paneText, DockableControlConfiguration dockableControlConfiguration)
		{
			dockableControlConfiguration.SetDockPanelStyle(pane, new DockPanelSettings()
			{
				TabSettings = new DockPanelTabSettings
				{
					TextTab = paneText,
					BackColor = ColorTranslator.FromHtml("#005A84"),
					BackColor2 = ColorTranslator.FromHtml("#005A84")
				},
				CaptionSettings = new DockPanelCaptionSettings
				{
					BackColor = ColorTranslator.FromHtml("#005A84")
				}
			});
		}


		#region CreatePDFBundle
		private void CreatePDFBundle()
		{
			FWBS.OMS.OMSFile file = ucSearchControl1.SearchList.Parent as OMSFile;

			KeyValueCollection kvc = new KeyValueCollection();
			kvc.Add("FILEID", file.ID);
			kvc.Add("DOCUMENTIDS", GetDocumentIDs());
			kvc.Add("RECREATEBUNDLE", false);

			FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("SFIDOCBUNDLE", file, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, kvc);

		}
		#endregion CreatePDFBundle


		#region GetDocumentIDs
		private List<long> GetDocumentIDs()
		{
			List<long> documentIDs = new List<long>();

			foreach (KeyValueCollection kvc in ucSearchControl1.SelectedItems)
			{
				documentIDs.Add(Convert.ToInt64(kvc["docid"].Value));
			}

			return documentIDs;
		}
		#endregion GetDocumentIDs


		public void UnloadPreview()
		{
			documentPreviewer1.UnloadPreview();
		}

		public void LoadPreview()
		{
			if(documentPreviewer1.Visible)
				documentPreviewer1.LoadPreview();
		}

        private void BuildIconColumn()
        {
            if (ucSearchControl1.Columns.Count == 0)
                return;

            DataGridViewLabelColumn icon_col = ucSearchControl1.Columns[0] as DataGridViewLabelColumn;
            if (icon_col == null)
                return;

            DataTable dt = ucSearchControl1.DataTable;
            if (dt == null)
                return;

            if (dt.Columns.Contains("#icon#") == false)
                dt.Columns.Add("#icon#", typeof(string));

            if (dt.Columns.Contains("docextension") && dt.Columns.Contains("doccheckedoutby"))
            {
                foreach (DataRow r in dt.Rows)
                {
                    string file;
                    string ext = Convert.ToString(r["docextension"]).TrimStart('.');

                    if (r["doccheckedoutby"] == DBNull.Value)
                    {
                        file = String.Format("checkedin.{0}", ext);
                    }
                    else if (Convert.ToInt32(r["doccheckedoutby"]) == Session.CurrentSession.CurrentUser.ID)
                    {
                        file = String.Format("checkedout.{0}", ext);
                    }
                    else
                    {
                        file = String.Format("locked.{0}", ext);
                    }

                    r["#icon#"] = file;

                    if (icons.Images.ContainsKey(file) == false)
                    {
                        Image icon = FWBS.Common.IconReader.GetFileIcon(file, FWBS.Common.IconReader.IconSize.Small, false).ToBitmap();

                        if (file.Contains("checkedout"))
                        {
                            using (Graphics g = Graphics.FromImage(icon))
                            {
                                try
                                {
                                    g.DrawImage(imageList1.Images["Tick"], 0, 0);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                        }
                        else if (file.Contains("locked"))
                        {
                            using (Graphics g = Graphics.FromImage(icon))
                            {
                                try
                                {
                                    g.DrawImage(imageList1.Images["Person"], 0, 0);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                        }

                        if (file.StartsWith("highq"))
                        {
                            using (Graphics g = Graphics.FromImage(icon))
                            {
                                try
                                {
                                    g.DrawImage(imageList1.Images["HighQ"], 0, 0);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                        }

                        icons.Images.Add(file, icon);
                    }
                }
            }

            icon_col.ImageList = icons;
            icon_col.ImageColumn = "#icon#";
        }

        private void BuildExtendedButtons()
		{
			OMSToolBarButton btnOpen = ucSearchControl1.GetOMSToolBarButton("cmdOpen");
			OMSToolBarButton btnOpenClose = ucSearchControl1.GetOMSToolBarButton("cmdOpen&Close");

			//If the open button does not have a parent code then build the menu
			//structure for the versioning, otherwise let the config sort it out.
			if (btnOpen != null)
			{
				if (!String.IsNullOrEmpty(btnOpen.ParentCode))
					return;
			}

			if (btnOpen != null && btnOpen.Style != ToolBarButtonStyle.DropDownButton)
			{
				btnOpen.Style = ToolBarButtonStyle.DropDownButton;
				btnOpen.DropDownMenu = mnuOpenDoc;
			}

			if (btnOpenClose != null)
			{
				btnOpenClose.Visible = false;
			}

		}


		private void SetDocumentDetails()
		{
			ucNavRichText1.ControlRich.Clear();
            if (ucSearchControl1.dgSearchResults.CurrentRowIndex >= 0)
            {
				var documentDetailsData = GetDocumentDetailsPanelData();
                var documentDetailsFormatting = new Version2DocumentDetails();
                documentDetailsFormatting.Set(ucNavRichText1, ucSearchControl1, documentDetailsData);
			}
            ucNavRichText1.Refresh();
		}


		private Dictionary<string, string> GetDocumentDetailsPanelData()
		{
            var currRowIndex = ucSearchControl1.dgSearchResults.CurrentRowIndex;
            var documentDetailsData = new Dictionary<string, string> 
			{
				{ _docref, Convert.ToString(ucSearchControl1.DataTable.DefaultView[currRowIndex]["DOCID"]) },
				{ _ourref, Convert.ToString(ucSearchControl1.DataTable.DefaultView[currRowIndex]["clNo"]) + "/" + Convert.ToString(ucSearchControl1.DataTable.DefaultView[currRowIndex]["fileNo"]) },
				{ _doctype, Convert.ToString(ucSearchControl1.DataTable.DefaultView[currRowIndex]["docTypedesc"]) },
				{ _created, Convert.ToString(((DateTime)ucSearchControl1.DataTable.DefaultView[currRowIndex]["Created"]).ToLocalTime()) },
				{ _createdby, Convert.ToString(ucSearchControl1.DataTable.DefaultView[currRowIndex]["CrByFullName"]) }
			};
			return documentDetailsData;
		}

		private void ActionsEnabled()
		{
			bool enabled = ucSearchControl1.SearchList.ResultCount > 0;
			SetButtonState("cmdOpen", enabled);
			SetButtonState("cmdOpen&Close", enabled);
			SetButtonState("cmdPrint", enabled);
			SetButtonState("cmdProperties", enabled);
			SetButtonState("cmdPreview", enabled);
			SetButtonState("cmdUndoCheckout", enabled);
			SetButtonState("cmdCheckout", enabled);
			//DMB added 10/2/2004 as these are only valid if a file has been selected
			SetButtonState("cmdSMS",_file != null);
			SetButtonState("cmdReceive",_file != null);

		}

		private void SetButtonState(string name, bool enabled)
		{
			OMSToolBarButton btn = ucSearchControl1.GetOMSToolBarButton(name);

			if (btn == null)
				return;

			btn.Enabled = enabled;
		}


		private void UndoCheckout(OMSDocument[] docs)
		{
			if (docs.Length > 0)
			{
				if (MessageBox.ShowYesNoQuestion("MSGUNDOCHNGSQ1", "Are you sure you want to undo? You will lose any changes that have been made") == DialogResult.Yes)
				{
					System.Text.StringBuilder errors = new System.Text.StringBuilder(); 

					foreach (OMSDocument doc in docs)
					{
						IStorageItemLockable lockdoc = doc.GetStorageProvider().GetLockableItem(doc);

						if (lockdoc.CanUndo)
							lockdoc.UndoCheckOut();
						else
							errors.AppendLine(doc.DisplayID+" - "+doc.Description);


						doc.GetStorageProvider().Fetch(doc, true, FWBS.Common.TriState.True);
									  
					}

					if (errors.Length == 0)
						MessageBox.ShowInformation("MSGUNDOCHECKOUT", "The changes to the document have successfully been undone.");
					else
						MessageBox.ShowInformation("MSGUNDOCKTFAIL", "The following document changes could not be undone:" + Environment.NewLine + "%1%", errors.ToString());

				}
			}

		}

		private void Checkout(OMSDocument[] docs)
		{
			if (docs.Length <= 0)
				return;

			System.Text.StringBuilder exceptions = new System.Text.StringBuilder();

			foreach (OMSDocument doc in docs)
			{
				try
				{
					FetchResults results = doc.GetStorageProvider().Fetch(doc, true, FWBS.Common.TriState.True);

					IStorageItemLockable lockdoc = doc.GetStorageProvider().GetLockableItem(doc);
					lockdoc.CheckOut(results.LocalFile);
				}
				catch (Exception ex)
				{
					if (exceptions.Length == 0)
						exceptions.AppendLine(Session.CurrentSession.Resources.GetResource("CHKOUTFAIL", "The following documents could not be checked out:","").Text);

					exceptions.AppendLine(doc.DisplayID+"-"+ ex.Message);
				}
			}

			List<OMSDocument> docList = new List<OMSDocument>(docs);
			FWBS.OMS.UI.Windows.Services.CheckForUnsupportedFiles(docList, false);

			if (exceptions.Length > 0)
				throw new Exception(exceptions.ToString());

		}

		#endregion

		#region Captured Events

		private void ucSearchControl1_ButtonEnabledRulesApplied(object sender, EventArgs e)
		{
			BuildExtendedButtons();
		}


		private void MenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				string exmsg = RunCommand(Convert.ToString(((MenuItem)sender).Tag));

				if (exmsg != "")
				{
					MessageBox.Show(exmsg, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
		}

		/// <summary>
		/// Captures and overrides the search lists button commands.
		/// </summary>
		/// <param name="sender">The search list control sending the request.</param>
		/// <param name="e">The event arguments that specify which button has bee pressed.</param>
		private void ucSearchControl1_SearchButtonCommands(object sender, FWBS.OMS.UI.Windows.SearchButtonEventArgs e)
		{
			try
            {
                string exmsg = RunCommand(e.ButtonName);

				if (exmsg != "")
				{
					MessageBox.Show(exmsg, "", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
		}

		/// <summary>
		/// Captures the document search lists hover event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ucSearchControl1_ItemHover(object sender, SearchItemHoverEventArgs e)
		{
			if (runCommandExecuting)
			{
				runCommandExecuting = false;
				timerExecCommand.Enabled = false;
				return;
			}

			if (!closingForm)
			{
				try
				{
					//Moved out of if statement as caused regression bug 5737

					SetDocumentDetails();
					OnDocumentSelecting();
					

					//Check pane exists and IsInView to resolve issue when users change DoubleClick on SCHDOCTRANS to cmdOpen&Close 492058137225154 - Bug 2427
					Infragistics.Win.UltraWinDock.DockableControlPane pane = omsDockManager1.PaneFromControl(documentPreviewer1);
					if (pane != null && pane.IsInView)
					{
						SetDocumentToBePreviewed();
					}
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
			}

		}

        bool pinnedState;
		bool previewHidden = true;
		DockableControlPane previewpane;

		private void SetDocumentToBePreviewed()
		{
			previewpane = omsDockManager1.PaneFromControl(documentPreviewer1);
            if (ucSearchControl1.dgSearchResults.CurrentRowIndex >= 0)
            {
                if(this.ParentForm is FWBS.OMS.UI.Windows.frmOMSTypeV2)
                {
                    if(previewHidden)
                        MakeMatterPreviewPaneVisible(previewpane);
                }
                else
                {
                    MakeDocumentPickerPreviewPaneVisible(previewpane);
                }
					
                try
                {
                    Common.KeyValueCollection ret = ucSearchControl1.CurrentItem();
                    //CM WI:8230 - Do not show preview if user has a deny permission on the view
                    if (!AllowDocumentPreview(Convert.ToInt64(ret["DOCID"].Value)))
                    {
                        documentPreviewer1.Connect(null);
                        documentPreviewer1.SetError(Session.CurrentSession.Resources.GetMessage("DOCPRVCHKSEC", "You do not have permissions to view this document", "").Text);
                        return;
                    }
                    //CM WI:5321 - DMS Archiving - Do not show preview if document has been deleted
                    if (ret != null && ret.Contains("docFlags"))
                    {
                        if ((ConvertDef.ToInt16(ret["docFlags"].Value, 0) & 0x0001) != 0)
                        {
                            documentPreviewer1.Connect(null);
                            documentPreviewer1.SetError(Session.CurrentSession.Resources.GetMessage("DOCPRVDOCDEL", "Document has been deleted", "").Text);
                            return;
                        }
                    }

                    Infragistics.Win.UltraWinDock.DockableControlPane pane = omsDockManager1.PaneFromControl(documentPreviewer1);

                    //Added for Bug 2427 - not really needed in the end (see ucSearchControl1_ItemHover), but leave for now as good practice anyway
                    if (pane != null)
                    {
                        if (pane.IsInView)
                            documentPreviewer1.Connect(OMSDocument.GetDocument(Convert.ToInt64(ret["DOCID"].Value)));
                        else
                            documentPreviewer1.Connect(null);

                        if (pane.Pinned)
                            ResetTimer();
                    }
						
                }
                catch (Security.SecurityException ex)
                {
                    documentPreviewer1.SetError(ex.Message);
                }
					
					
            }
            else
            {
                if(this.ParentForm is FWBS.OMS.UI.Windows.frmOMSTypeV2)
                {
                    MakeMatterPreviewPaneInvisible(previewpane);
                }
                else
                {
                    MakeDocumentPickerPreviewPaneInvisible(previewpane);
                }
            }
		}


		private void MakeMatterPreviewPaneVisible(Infragistics.Win.UltraWinDock.DockableControlPane previewpane)
		{
			this.omsDockManager1.PaneDisplayed -= new Infragistics.Win.UltraWinDock.PaneDisplayedEventHandler(this.omsDockManager1_PaneDisplayed);
            this.SuspendLayout();
            omsDockManager1.Visible = false;
            previewpane.Pinned = pinnedState;
            previewHidden = false;
            previewpane.Show();
            omsDockManager1.Visible = true;
            this.ResumeLayout();
            this.omsDockManager1.PaneDisplayed += new Infragistics.Win.UltraWinDock.PaneDisplayedEventHandler(this.omsDockManager1_PaneDisplayed);
        }


        private void MakeDocumentPickerPreviewPaneVisible(Infragistics.Win.UltraWinDock.DockableControlPane previewpane)
		{
			omsDockManager1.Visible = true;
			previewpane.Show();
		}


		private void MakeMatterPreviewPaneInvisible(Infragistics.Win.UltraWinDock.DockableControlPane previewpane)
		{
            pinnedState = previewpane.Pinned;
            previewHidden = true;
			omsDockManager1.Visible = true;
			previewpane.Close();
            previewpane.Pinned = pinnedState;
            documentPreviewer1.Connect(null);
		}


		private void MakeDocumentPickerPreviewPaneInvisible(Infragistics.Win.UltraWinDock.DockableControlPane previewpane)
		{
			omsDockManager1.Visible = true;
			previewpane.Close();
			documentPreviewer1.Connect(null);
		}


		private static bool AllowDocumentPreview(long DocID)
		{
			object retval = (Session.CurrentSession.CurrentConnection.ExecuteSQL(string.Format("select CAST(1 AS BIT) from [config].[searchDocumentAccess] ('Document',{0})", DocID), null).Rows[0][0]);
			if (retval is DBNull || retval == null)
				throw new NullReferenceException("Unable to get Allow Document Preview Function");
			else
				return (Boolean)retval;
		}


		private void ResetTimer()
		{
			timer1.Enabled = false;
			timer1.Enabled = true;
		}


		private void ucSearchControl1_ItemHovered(object sender, EventArgs e)
		{
			try
			{
                var searchControl = (ucSearchControl)sender;

                if (searchControl.SelectedItems.Length > 1)
                {
                    _matterFoldersTree.UnselectFolders();
                }
                else
                {
                    var keyValueCollectionItem = searchControl.CurrentItem();
                    if (keyValueCollectionItem != null)
                    {
                        var keyValueItem = keyValueCollectionItem["docFolderGuid"];
                        if (keyValueItem != null)
                        {
                            var guid = Guid.Empty;
                            Guid.TryParse(Convert.ToString(keyValueItem.Value), out guid);
                            _matterFoldersTree.SelectedFolderGuid = guid;
                        }
                    }
                }

                OnDocumentSelected();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}

		}

		private void ucSearchControl1_SearchCompleted(object sender, SearchCompletedEventArgs e)
		{
			OnDocumentsRefreshed();

			populatedGroups.Clear();
			DocumentGrouping.RefreshNodes();

			ActionsEnabled();
			BuildIconColumn();
		}

        private Bitmap GetDocIconColumnHeaderImage()
        {
            int size = LogicalToDeviceUnits(16);
            Bitmap image = Images.GetCoolButton(0, (Images.IconSize)size).ToBitmap();
            if (image.Width != size)
            {
                image = new Bitmap(image, new Size(size, size));
            }
            return image;
        }

		private void ucSearchControl1_SearchListLoad(object sender, System.EventArgs e)
		{
            if (ucSearchControl1.Columns.Count > 0)
            {
                DataGridViewLabelColumn iconColumn = ucSearchControl1.Columns[0] as DataGridViewLabelColumn;
                if (iconColumn != null && string.IsNullOrEmpty(iconColumn.HeaderText))
                {
                    iconColumn.HeaderCell = new DataGridViewColumnHeaderImageCell()
                    {
                        Image = GetDocIconColumnHeaderImage(),
                        Style = new DataGridViewCellStyle
                        {
                            Padding = new Padding(LogicalToDeviceUnits(4)),
                            Alignment = ucSearchControl1.RightToLeft == RightToLeft.Yes ? DataGridViewContentAlignment.MiddleRight : DataGridViewContentAlignment.MiddleLeft
                        }
                    };
                    iconColumn.MinimumWidth = LogicalToDeviceUnits(24);
                }
            }

			_matterFoldersTree.ClearCheckedFolders();
			previewHidden = true;
			omsDockManager1.Visible = false;
			ActionsEnabled();
			bool sys = FWBS.OMS.Security.SecurityManager.CurrentManager.IsGranted(new SystemPermission(StandardPermissionType.DeleteDocument));
			if (sys == false && ucSearchControl1.GetOMSToolBarButton("cmdDelete") != null)
			{
				ucSearchControl1.GetOMSToolBarButton("cmdDelete").Visible = false;
				ucSearchControl1.GetOMSToolBarButton("cmdDelete").PanelButtonVisible = false;
			}

			AttachCellDisplayEvents();
		}

		#endregion

		#region IDocumentsAddin Members

		private DocumentAddinHost host;
		
		public void InitialiseHost(DocumentAddinHost host)
		{
			this.host = host;
		}

		public OMSDocument[] SelectedDocuments
		{
			get
			{
				List<OMSDocument> docs = new List<OMSDocument>();

				if (ucSearchControl1.SearchList.ResultCount > 0)
				{
					Common.KeyValueCollection[] ret = ucSearchControl1.SelectedItems;

					for (int ctr = 0; ctr < ret.Length; ctr++)
					{
						Common.KeyValueCollection val = ret[ctr];
						try
						{
							docs.Add(OMSDocument.GetDocument((long)val["DOCID"].Value));
						}
						catch
						{
						}
					}

				}

				return docs.ToArray();
			}
		}

		public DocumentPickerType DefaultView
		{
			get
			{
				return DocumentPickerType.LatestUpdate;
			}
		}

		public string[] SelectedDocumentIds
		{
			get 
			{
				List<string> list = new List<string>();
				foreach (FWBS.Common.KeyValueCollection sel  in ucSearchControl1.SelectedItems)
				{
					list.Add(Convert.ToString(sel["DOCID"].Value));
				}
				return list.ToArray();
			}
		}

		public int DocumentCount
		{
			get
			{
				return ucSearchControl1.SearchList.ResultCount;
			}
		}

		public string GetCurrentDocumentDetailsAsRTF()
		{
			return ucNavRichText1.ControlRich.Rtf;
		}



		public bool SupportsView(DocumentPickerType view)
		{
			switch (view)
			{
				case DocumentPickerType.CheckedOut:
				case DocumentPickerType.Client:
				case DocumentPickerType.File:
				case DocumentPickerType.Latest:
                case DocumentPickerType.LatestOpen:
                case DocumentPickerType.Local:
				case DocumentPickerType.LatestUpdate:
				case DocumentPickerType.Search:
					return true;
			}
			return false;
		}

		public void ShowView(DocumentPickerType view, IOMSType obj)
		{
			switch (view)
			{
				case DocumentPickerType.CheckedOut:
					this.SearchListCode = Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.DocumentCheckedOut);
					break;
				case DocumentPickerType.Client:
					this.SearchListCode = Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.DocumentClient);
					break;
				case DocumentPickerType.File:
					this.SearchListCode = Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.Document);
					break;
				case DocumentPickerType.Latest:
					this.SearchListCode = Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.DocumentLast);
					break;
                case DocumentPickerType.LatestOpen:
                    this.SearchListCode = Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.DocumentLatestOpened);
                    break;
                case DocumentPickerType.LatestUpdate:
					this.SearchListCode = Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.DocumentLatestUpdate);
					break;
				case DocumentPickerType.Local:
					this.SearchListCode = Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.DocumentLocal);
					break;
				case DocumentPickerType.Search:
					this.SearchListCode = Session.CurrentSession.DefaultSystemSearchListGroups(SystemSearchListGroups.DocumentAll);
					break;
				default:
					return;
			}

			this.Connect(obj);
			this.RefreshItem();

		}

		#endregion

		private void ucSearchControl1_SearchTypeChanged(object sender, EventArgs e)
		{
			populatedGroups.Clear();
			DocumentGrouping.Clear();
			documentPreviewer1.Connect(null);
			documentPreviewer1.RefreshItem();
		}

		private void omsDockManager1_PaneDisplayed(object sender, Infragistics.Win.UltraWinDock.PaneDisplayedEventArgs e)
		{
			if (e.Pane == omsDockManager1.PaneFromControl(documentPreviewer1))
			{
				SetDocumentToBePreviewed();
				documentPreviewer1.RefreshItem();
			}
		}

		private void omsDockManager1_PaneHidden(object sender, Infragistics.Win.UltraWinDock.PaneHiddenEventArgs e)
		{
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			timer1.Enabled = false;
			documentPreviewer1.RefreshItem();
			
		}

		private void timerExecCommand_Tick(object sender, EventArgs e)
		{
			runCommandExecuting = false;
			timerExecCommand.Enabled = false;
		}

		private void ucSearchControl1_FilterChanged(object sender, EventArgs e)
		{
			OnDocumentSelecting();
			SetDocumentToBePreviewed();
		}

		private void ucSearchControl1_SearchCompleted_1(object sender, SearchCompletedEventArgs e)
		{
			OnDocumentSelecting();
			SetDocumentToBePreviewed();
		}

        private void UpdateSearchListResultSet(string folderGUIDList)
        {
            KeyValueCollection kvc = new KeyValueCollection();
            kvc.Add("folderGUIDList", folderGUIDList);
            kvc.Add("FILEID", _file.ID);
            this.ucSearchControl1.SearchList.ChangeParameters(kvc);
            this.ucSearchControl1.Search();
        }

        private void dgSearchResults_DragOver(object sender, DragEventArgs e)
		{
			e.Effect = DragDropEffects.Move;
		}


		private void dgSearchResults_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button.Equals(MouseButtons.Left))
				mouseDownPoint = e.Location;
		}


		private void dgSearchResults_MouseMove(object sender, MouseEventArgs e)
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

        private void _matterFoldersTree_NodeCheckedChanged(object sender, NodeCheckedChangedEventArgs e)
        {
            UpdateSearchListResultSet(e.CheckedFolderList);
        }

        #region DragDrop

        private void MatterFoldersTreeFoldersDrop(object sender, FoldersDropEventArgs e)
        {
            switch (e.FoldersDropType)
            {
                case FoldersDropType.Id:
                {
                    ProcessObjectDragAndDrop(ProcessIdDrag, sender, e);
                    break;
                }
                case FoldersDropType.ExternalFile:
                {
                    new FileActivity(_file, StatusManagement.Activities.FileStatusActivityType.DocumentModification).Check();
                    ProcessObjectDragAndDrop(ProcessExternalFileDrag, sender, e);
                    break;
                }
            }
        }

        private void ProcessObjectDragAndDrop(Action<object, FoldersDropEventArgs> action, object sender, FoldersDropEventArgs e)
        {
            action(sender, e);
        }

        private void ProcessIdDrag(object sender, FoldersDropEventArgs e)
        {
            string passeddata = e.DragEventArgs.Data.GetData(DataFormats.Text).ToString();
            if (passeddata.Length > 0)
                UpdateDocumentFolderGUID(passeddata, sender, e);
        }

        private void ProcessExternalFileDrag(object sender, FoldersDropEventArgs e)
        {
            string[] files = (string[])e.DragEventArgs.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 0)
            {
                SaveFilesToDocumentStore(sender, e, files);
            }
        }

        private void UpdateDocumentFolderGUID(string docIDs, object sender, FoldersDropEventArgs e)
        {
            var repository = new DocumentFolderRepositoryXML();

            Dictionary<Guid, List<string>> docs = new Dictionary<Guid, List<string>>();

            XElement docIDsXml = new XElement("items");
            foreach (var docID in docIDs.Split(','))
            {
                docIDsXml.Add(new XElement("item", docID));
            }

            repository.AssignForlderGUIDBatch(docIDsXml.ToString(), e.DestinationNodeGuid, false, Session.CurrentSession.CurrentUser.ID);
        }

        private void SaveFilesToDocumentStore(object sender, FoldersDropEventArgs e, string[] files)
        {
            var validation = FilesValidForSaving(files, new ValidationResult());

            if (validation.Success)
            {
                this.BeginInvoke((Action)delegate()
                {
                    try
                    {
                        if (files.Length == 1)
                        {
                            ProcessSingleDocumentSave(files[0], e.DestinationNodeGuid, (e.DragEventArgs.KeyState & 0x20) != 0);
                        }
                        else
                        {
                            ProcessMultipleDocumentSave(files, e.DestinationNodeGuid);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ParentForm, ex);
                    }
                });
            }
            else
            {
                OuputErrorMessages(validation);
            }
        }

        private ValidationResult FilesValidForSaving(string[] files, ValidationResult validation)
        {
            var directorySaveErrorMessage = Session.CurrentSession.Resources.GetResource("UCBUILTIN_5", "Unable to save directories/folders.", "").Text;
            var invalidFileTypeErrorMessage = Session.CurrentSession.Resources.GetResource("UCBUILTIN_4", "An invalid file type has been found in the file(s) chosen to save to the Matter.", "").Text;

            foreach (string file in files)
            {
                if (FileIsDirectory(file))
                {
                    validation.Success = false;
                    AddErrorMessage(directorySaveErrorMessage, validation);
                }

                if (!FileExtensionIsValid(file))
                {
                    validation.Success = false;
                    AddErrorMessage(invalidFileTypeErrorMessage, validation);
                }
            }

            return validation;
        }

        private void AddErrorMessage(string errorMessage, ValidationResult validation)
        {
            if (validation.ProblemsFound.IndexOf(errorMessage) == -1)
            {
                validation.ProblemsFound.Add(errorMessage);
            }
        }

        private bool FileIsDirectory(string file)
        {
            FileAttributes attributes = File.GetAttributes(file);

            return ((attributes & FileAttributes.Directory) == FileAttributes.Directory);
        }

        private bool FileExtensionIsValid(string file)
        {
            return FWBS.OMS.DocumentManagement.Storage.StorageManager.CurrentManager.IsValidFileExtension(file);
        }

        private void OuputErrorMessages(ValidationResult validationResult)
        {
            var errorMessages = new StringBuilder("Unable to continue with save because:\n\n");

            foreach (var problem in validationResult.ProblemsFound)
            {
                errorMessages.AppendLine(string.Format("- {0}", problem));
            }

            throw new Exception(errorMessages.ToString());
        }

        private bool InvalidFileExtensionFound(string [] files)
        {
            foreach (string f in files)
            {
                if (FWBS.OMS.DocumentManagement.Storage.StorageManager.CurrentManager.IsValidFileExtension(f))
                    return true;
            }
            return false;
        }

        private void ProcessSingleDocumentSave(string file, Guid folderGuid, bool alt)
        {
            if (folderGuid != Guid.Empty)
            {
                FWBS.OMS.UI.Windows.ShellOMS _appcontroller = new FWBS.OMS.UI.Windows.ShellOMS();
                Apps.ApplicationManager.CurrentManager.InitialiseInstance("SHELL", _appcontroller);
                using (FWBS.OMS.UI.Windows.ShellFile sf = new FWBS.OMS.UI.Windows.ShellFile(new FileInfo(file)))
                {
                    SaveSettings saveSettings = new SaveSettings()
                    {
                        UseDefaultAssociate = true,
                        Mode = PrecSaveMode.Quick,
                        TargetAssociate = _file.DefaultAssociate,
                        FolderGuid = folderGuid,
                        AllowRelink = false
                    };

                    if (BulkDocumentImportTools.CheckForDocIDVariable(_appcontroller, sf) && BulkDocumentImportTools.CheckDocForCompanyID(_appcontroller, sf))
                    {
                        if (UserWantsToUseExistingProfileInformation())
                        {
                            var settings = SaveSettings.Default;

                            if (QuickSaveEnabled())
                            {
                                settings.Mode = PrecSaveMode.Quick;
                            }

                            _appcontroller.Save(sf, settings);
                        }
                        else
                        {
                            _appcontroller.SaveAs((object)sf, false, saveSettings, (Action<SaveSettings>)null);
                        }
                    }
                    else
                    {
                        if (alt && _file.Associates.Count > 1)
                        {
                            Associate assoc = Services.Searches.PickAssociate(ParentForm, _file, true);
                            if (assoc != null)
                                saveSettings.TargetAssociate = assoc;
                        }

                        _appcontroller.SaveAs((object)sf, false, saveSettings, (Action<SaveSettings>)null);
                    }
                }
            }
        }

        private bool QuickSaveEnabled()
        {
            string additionalSaveCommands = Session.CurrentSession.CurrentUser.AdditionalDocumentSaveCommands;

            if (string.IsNullOrEmpty(additionalSaveCommands))
                additionalSaveCommands = Session.CurrentSession.AdditionalDocumentSaveCommands;

            return additionalSaveCommands.Contains("QUICK");
        }

        private bool UserWantsToUseExistingProfileInformation()
        {
            return MessageBox.Show(ParentForm,
                       Session.CurrentSession.Resources.GetResource("UCBUILTIN_2", "The document has been saved already, would you like to save it using the existing profile information?", "").Text,
                       Session.CurrentSession.Resources.GetResource("UCBUILTIN_3", "Document Save", "").Text,
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void ProcessMultipleDocumentSave(string[] files, Guid folderGuid)
        {
            FWBS.OMS.UI.Windows.ShellOMS _appcontroller = new FWBS.OMS.UI.Windows.ShellOMS();
            Apps.ApplicationManager.CurrentManager.InitialiseInstance("SHELL", _appcontroller);

            BulkDocumentImportTools bulkTools = new BulkDocumentImportTools(files, _appcontroller);
            bulkTools.SaveMultipleDocuments(_file.ID, folderGuid);
        }

        private void bulkTools_AfterMultipleDocumentSave(object sender, BulkDocumentImportTools.AfterMultipleDocumentSaveArgs e)
        { }

        #endregion

    }


    #region Version2DocumentDetails
    internal class Version2DocumentDetails
	{
		public void Set(ucNavRichText ucNavRichText1, ucSearchControl ucSearchControl1, Dictionary<string, string> documentDetailsData)
		{
			ucNavRichText1.ControlRich.Font = new Font(CurrentUIVersion.Font, CurrentUIVersion.FontSize, FontStyle.Regular);

			foreach (var documentDetail in documentDetailsData)
			{
				AddDocumentDetailsHeaderItem(ucNavRichText1, documentDetail.Key);
				AddDocumentDetailsData(ucNavRichText1, documentDetail.Value);
			}
		}


		private void AddDocumentDetailsHeaderItem(ucNavRichText ucNavRichText1, string header)
		{
			ucNavRichText1.ControlRich.SelectionColor = Color.Black;
			ucNavRichText1.ControlRich.AppendText(header + Environment.NewLine);
		}


		private void AddDocumentDetailsData(ucNavRichText ucNavRichText1, string value)
		{
			ucNavRichText1.ControlRich.SelectionColor = Color.FromArgb(102, 102, 102);
			ucNavRichText1.ControlRich.AppendText(value + Environment.NewLine + Environment.NewLine);
		}
	}
	#endregion Version2DocumentDetails

}


