using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// This form holds the user controls that build the OMSType object display dialog.
    /// </summary>
    internal class frmOMSType : frmDialog, Interfaces.IOMSTypeWindow, Interfaces.IfrmOMSType
	{
		#region Control Fields

		private System.ComponentModel.IContainer components = null;
        private ucOMSTypeDisplay ucOMSTypeDefault;
		private SearchManager _style = SearchManager.None;

		#endregion

		#region Fields

		/// <summary>
		/// The Display Type that Contains the Search Window
		/// </summary>
        private ucOMSTypeDisplay _omstypewithsearch = null;

		/// <summary>
		/// Holds the display order of object display controls.
		/// </summary>
        private List<ucOMSTypeDisplay> _displayOrder = null;

		/// <summary>
		/// Returns the Last OMS Type Display 
		/// </summary>
        private ucOMSTypeDisplay lastdisplay;

		/// <summary>
		/// Returns the Command Centre OMS Type Display 
		/// </summary>
        private Interfaces.IOMSTypeDisplay CmdCentre;

		private System.Windows.Forms.ContextMenu cmenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private FWBS.OMS.UI.StatusBar stBar;
		private System.Windows.Forms.ToolBarButton cmdSearch;
		private System.Windows.Forms.ToolBarButton cmdCmdCentre;
		private System.Windows.Forms.ToolBarButton sp1;
		private System.Windows.Forms.ToolBarButton Sp2;
        private System.Windows.Forms.StatusBarPanel statusBarPanel1;

		public static ArrayList savetrace = new ArrayList();

		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Default contructor.
		/// </summary>
        private frmOMSType() : base()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            this.Activated += new EventHandler(frmOMSType_Activated);
            _displayOrder = new List<ucOMSTypeDisplay>();
            FWBS.OMS.UI.Windows.Services.OMSTypeRefresh += new EventHandler(cmdRefresh_Click);
            cmdCmdCentre.Visible = Session.CurrentSession.IsPackageInstalled("COMMANDCENTRE");
            cmdSearch.Visible = (!Session.CurrentSession.CurrentBranch.DisableSearchButton);
            Sp2.Visible = cmdCmdCentre.Visible;
        }

        protected override void SetResources()
        {
            cmdBack.Text = Session.CurrentSession.Resources.GetResource("CMDBACK", "&Back", "").Text;
            cmdRefresh.Text = Session.CurrentSession.Resources.GetResource("CMDREFRESH", "&Refresh", "").Text;
            cmdSave.Text = Session.CurrentSession.Resources.GetResource("CMDSAVE", "&Save", "").Text;
            cmdCancel.Text = Session.CurrentSession.Resources.GetResource("CMDCANCEL", "Cance&l", "").Text;
            cmdOK.Text = Session.CurrentSession.Resources.GetResource("OMSTOK", "&OK", "").Text;
            this.cmdSearch.Text = Session.CurrentSession.Resources.GetResource("cmdSearch", "Se&arch", "").Text;
            this.cmdCmdCentre.Text = Session.CurrentSession.Resources.GetResource("CmdCentre", "&Command Centre", "").Text;
        }

        private void frmOMSType_Activated(object sender, EventArgs e)
        {
            try
            {
                if (_displayOrder.Count > 0 && _displayOrder[_displayOrder.Count - 1] != null && _displayOrder[_displayOrder.Count - 1].Object != null)
                {
                    Trace.WriteLine("Before");
                    if (Session.CurrentSession.CurrentClient != null)
                        Trace.WriteLine("CC: " + Session.CurrentSession.CurrentClient.ClientNo);
                    if (Session.CurrentSession.CurrentFile != null)
                        Trace.WriteLine("CF: " + Session.CurrentSession.CurrentFile.Client.ClientNo + "/" + Session.CurrentSession.CurrentFile.FileNo);
                    if (Session.CurrentSession.CurrentAssociate != null)
                        Trace.WriteLine("CA: " + Session.CurrentSession.CurrentAssociate.Salutation + "/" + Session.CurrentSession.CurrentAssociate.Contact.Name);
                    if (Session.CurrentSession.CurrentContact != null)
                        Trace.WriteLine("CC: " + Session.CurrentSession.CurrentContact.Name);
                    _displayOrder[_displayOrder.Count - 1].Object.SetCurrentSessions();
                    Trace.WriteLine("After");
                    if (Session.CurrentSession.CurrentClient != null)
                        Trace.WriteLine("CC: " + Session.CurrentSession.CurrentClient.ClientNo);
                    if (Session.CurrentSession.CurrentFile != null)
                        Trace.WriteLine("CF: " + Session.CurrentSession.CurrentFile.Client.ClientNo + "/" + Session.CurrentSession.CurrentFile.FileNo);
                    if (Session.CurrentSession.CurrentAssociate != null)
                        Trace.WriteLine("CA: " + Session.CurrentSession.CurrentAssociate.Salutation + "/" + Session.CurrentSession.CurrentAssociate.Contact.Name);
                    if (Session.CurrentSession.CurrentContact != null)
                        Trace.WriteLine("CC: " + Session.CurrentSession.CurrentContact.Name);
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("frmOMSType_Activated:" + ex.Message);
            }
        }

		/// <summary>
		/// Constructs an OMS form as a search manager.
		/// </summary>
		public frmOMSType(SearchManager Style) : this()
		{
			cmdSearch.Pushed=true;
			cmdSearch.Visible=false;
			cmdSave.Enabled=false;
			cmdCancel.Enabled=false;
			//added DMB 2/2/2004 
			cmdRefresh.Enabled=false;
			this.FormStorageID = Style.ToString();
			_style = Style;
			_displayOrder.Add(ucOMSTypeDefault);
			ucOMSTypeDefault.SearchManagerCloseVisible = false;
			SetNavigationButtonState();
		}

		/// <summary>
		/// Constructs an OMS forms for a users command centre configuration.
		/// </summary>
		public frmOMSType(FWBS.OMS.User user) : this(user, user.CommandCentre)
		{
		}

		/// <summary>
		/// Constructs the dialog form with a base configurable object.
		/// </summary>
		/// <param name="obj">Configurable type object.</param>
		public frmOMSType(FWBS.OMS.Interfaces.IOMSType obj) : this(obj, null)
		{
		}
		
		/// <summary>
		/// Constructs the dialog form with a base configurable object.
		/// </summary>
		internal frmOMSType(FWBS.OMS.Interfaces.IOMSType obj, OMSType omst) : this()
		{
			try
			{
				this.FormStorageID = obj.GetType().Name;
				Cursor = Cursors.WaitCursor;
				ucOMSTypeDefault.Open(obj, omst);
				Icon = ucOMSTypeDefault.ObjectTypeIcon;
				Text = ucOMSTypeDefault.ObjectTypeCaption;
				_displayOrder.Add(ucOMSTypeDefault);
				SetNavigationButtonState();
				lastdisplay = ucOMSTypeDefault;
				if (omst is FWBS.OMS.CommandCentreType)
				{
					cmdCmdCentre.Enabled = false;
				}
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
            try
            {
                if (disposing)
                {
                    if (_displayOrder != null)
                    {
                        foreach (var i in _displayOrder)
                        {
                            UnattachDisplayEvents(i);
                            i.Dispose();
                        }
                    }
                    _displayOrder = null;
                    lastdisplay = null;
                    FWBS.OMS.UI.Windows.Services.OMSTypeRefresh -= new EventHandler(cmdRefresh_Click);
                    if (components != null)
                    {
                        components.Dispose();
                    }
                    if (_displayOrder != null)
                    {
                        _displayOrder.Clear();
                        _displayOrder = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
		}

        private void UnattachDisplayEvents(ucOMSTypeDisplay i)
        {
            if (i != null)
            {
                i.NewOMSTypeWindow -= new FWBS.OMS.UI.Windows.NewOMSTypeWindowEventHandler(this.ucOMSTypeDefault_NewOMSTypeWindow);
                i.SearchManagerVisibleChanged -= new EventHandler(active_SearchManagerVisibleChanged);
            }
        }

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOMSType));
            this.ucOMSTypeDefault = new FWBS.OMS.UI.Windows.ucOMSTypeDisplay();
            this.cmenu = new System.Windows.Forms.ContextMenu();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.stBar = new FWBS.OMS.UI.StatusBar();
            this.statusBarPanel1 = new System.Windows.Forms.StatusBarPanel();
            this.cmdSearch = new System.Windows.Forms.ToolBarButton();
            this.cmdCmdCentre = new System.Windows.Forms.ToolBarButton();
            this.sp1 = new System.Windows.Forms.ToolBarButton();
            this.Sp2 = new System.Windows.Forms.ToolBarButton();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).BeginInit();
            this.SuspendLayout();
            // 
            // ucFormStorage1
            // 
            this.ucFormStorage1.DefaultPercentageHeight = 90;
            this.ucFormStorage1.DefaultPercentageWidth = 90;
            this.ucFormStorage1.Version = ((long)(3));
            // 
            // tbLeft
            // 
            this.tbLeft.Buttons.AddRange(new System.Windows.Forms.ToolBarButton[] {
            this.cmdSearch,
            this.sp1,
            this.cmdCmdCentre,
            this.Sp2});
            this.tbLeft.Size = new System.Drawing.Size(182, 42);
            this.tbLeft.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbDialogs_ButtonClick);
            // 
            // tbRight
            // 
            this.tbRight.Size = new System.Drawing.Size(234, 42);
            this.tbRight.ButtonClick += new System.Windows.Forms.ToolBarButtonClickEventHandler(this.tbDialogs_ButtonClick);
            // 
            // pnlTop
            // 
            this.pnlTop.Location = new System.Drawing.Point(0, 2);
            this.pnlTop.Size = new System.Drawing.Size(742, 42);
            this.pnlTop.Controls.SetChildIndex(this.quickOK, 0);
            this.pnlTop.Controls.SetChildIndex(this.quickCancel, 0);
            // 
            // cmdBack
            // 
            this.cmdBack.DropDownMenu = this.cmenu;
            // 
            // quickOK
            // 
            this.quickOK.Size = new System.Drawing.Size(19, 32);
            // 
            // quickCancel
            // 
            this.quickCancel.Size = new System.Drawing.Size(19, 32);
            // 
            // ucOMSTypeDefault
            // 
            this.ucOMSTypeDefault.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("ucOMSTypeDefault.BackgroundImage")));
            this.ucOMSTypeDefault.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ucOMSTypeDefault.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucOMSTypeDefault.InfoPanelCloseVisible = true;
            this.ucOMSTypeDefault.InformationPanelVisible = true;
            this.ucOMSTypeDefault.ipc_BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(212)))), ((int)(((byte)(243)))));
            this.ucOMSTypeDefault.ipc_Visible = true;
            this.ucOMSTypeDefault.ipc_Width = 157;
            this.ucOMSTypeDefault.Location = new System.Drawing.Point(0, 44);
            this.ucOMSTypeDefault.Name = "ucOMSTypeDefault";
            this.ucOMSTypeDefault.Padding = new System.Windows.Forms.Padding(5);
            this.ucOMSTypeDefault.SearchManagerCloseVisible = true;
            this.ucOMSTypeDefault.SearchManagerVisible = false;
            this.ucOMSTypeDefault.SearchText = null;
            this.ucOMSTypeDefault.Size = new System.Drawing.Size(742, 424);
            this.ucOMSTypeDefault.TabIndex = 0;
            this.ucOMSTypeDefault.TabPositions = System.Windows.Forms.TabAlignment.Top;
            this.ucOMSTypeDefault.ElasticsearchVisible = false;
            this.ucOMSTypeDefault.ToBeRefreshed = false;
            this.ucOMSTypeDefault.NewOMSTypeWindow += new FWBS.OMS.UI.Windows.NewOMSTypeWindowEventHandler(this.ucOMSTypeDefault_NewOMSTypeWindow);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = -1;
            this.menuItem1.Text = "";
            // 
            // stBar
            // 
            this.stBar.Location = new System.Drawing.Point(0, 468);
            this.stBar.Name = "stBar";
            this.stBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
            this.statusBarPanel1});
            this.stBar.ShowPanels = true;
            this.stBar.Size = new System.Drawing.Size(742, 22);
            this.stBar.TabIndex = 10;
            // 
            // statusBarPanel1
            // 
            this.statusBarPanel1.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Contents;
            this.statusBarPanel1.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.None;
            this.statusBarPanel1.Name = "statusBarPanel1";
            this.statusBarPanel1.Width = 10;
            // 
            // cmdSearch
            // 
            this.cmdSearch.ImageIndex = 10;
            this.cmdSearch.Name = "cmdSearch";
            this.cmdSearch.Style = System.Windows.Forms.ToolBarButtonStyle.ToggleButton;
            // 
            // cmdCmdCentre
            // 
            this.cmdCmdCentre.ImageIndex = 27;
            this.cmdCmdCentre.Name = "cmdCmdCentre";
            // 
            // sp1
            // 
            this.sp1.Name = "sp1";
            this.sp1.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // Sp2
            // 
            this.Sp2.Name = "Sp2";
            this.Sp2.Style = System.Windows.Forms.ToolBarButtonStyle.Separator;
            // 
            // frmOMSType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(742, 490);
            this.Controls.Add(this.ucOMSTypeDefault);
            this.Controls.Add(this.stBar);
            this.Name = "frmOMSType";
            this.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frmOMSType_Closing);
            this.Load += new System.EventHandler(this.frmOMSType_Load);
            this.Controls.SetChildIndex(this.stBar, 0);
            this.Controls.SetChildIndex(this.pnlTop, 0);
            this.Controls.SetChildIndex(this.ucOMSTypeDefault, 0);
            this.pnlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.statusBarPanel1)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion
	
		#endregion

		#region Methods


		/// <summary>
		/// Captures any new dialog window request from the child user controls.
		/// </summary>
		/// <param name="sender">Display control instance.</param>
		/// <param name="e">NewOMSTypeWindowEventArgs event arguments.</param>
		private void ucOMSTypeDefault_NewOMSTypeWindow(object sender, FWBS.OMS.UI.Windows.NewOMSTypeWindowEventArgs e)
		{
            _displayOrder[_displayOrder.Count - 1].Enabled = false;
            stBar.Panels[0].Text = "";

			MenuItem mnui = new MenuItem();
            mnui.Text = _displayOrder[_displayOrder.Count - 1].ObjectTypeDescription;
			this.cmenu.MenuItems.Add(_displayOrder.Count - 1,mnui);
			mnui.Click += new System.EventHandler(this.pnlSelect_Click);

            FWBS.OMS.UI.Windows.ucOMSTypeDisplay display = new FWBS.OMS.UI.Windows.ucOMSTypeDisplay()
            {
                Location = ucOMSTypeDefault.Location,
                Size = ucOMSTypeDefault.Size
            };

            display.SuspendLayout();
			display.SearchManagerVisibleChanged +=new EventHandler(active_SearchManagerVisibleChanged);
            display.NewOMSTypeWindow += new FWBS.OMS.UI.Windows.NewOMSTypeWindowEventHandler(this.ucOMSTypeDefault_NewOMSTypeWindow);

			//Set the default properties of the new display item and make it visible.
			display.ipc_BackColor = System.Drawing.Color.Gray;
			display.ipc_Width = 157;

			this.Controls.Add(display);
			display.Open(e.OMSObject, e.OMSType);
			display.BringToFront();
            display.Dock = DockStyle.Fill;
            display.ResumeLayout();

			Icon = display.ObjectTypeIcon;
			Text = display.ObjectTypeCaption;

			_displayOrder.Add(display);
            Global.RightToLeftControlConverter(display, this);
            display.ipc_Visible = true;
			SetNavigationButtonState();

		    display.Visible=true;

            if (_displayOrder[_displayOrder.Count - 1] == _omstypewithsearch && cmdSearch.Pushed)
			{
				cmdSearch.Enabled=false;
			}

			cmdSave.Enabled=true;
			cmdCancel.Enabled=true;
			//added DMB 2/2/2004
			cmdRefresh.Enabled=true;
			lastdisplay = display;
			if (e.OMSType is FWBS.OMS.CommandCentreType)
			{
				CmdCentre = lastdisplay;
				cmdCmdCentre.Enabled = false;
			}
            display.GotoDefaultPage();
		}

		/// <summary>
		/// Disables the navigational buttons if there is only one object display control available.
		/// </summary>
		private void SetNavigationButtonState()
		{
			if (_displayOrder.Count > 1 || (cmdSearch.Pushed && cmdSearch.Visible))
				cmdBack.Enabled = true;
			else
				cmdBack.Enabled = false;
		}

		/// <summary>
		/// Disposes of the active display control.
		/// </summary>
		/// <param name="sender">Back button.</param>
		/// <param name="e">Empty event arguments.</param>
		protected void cmdBack_Click(object sender, System.EventArgs e)
		{
			if (cmdBack.Enabled)
			{
				try
				{		
					Cursor = Cursors.WaitCursor;

					Back();
				}
				catch(Exception ex)
				{
					ErrorBox.Show(this, ex);
				}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}

		/// <summary>
		/// Saves all of the current display controls and their content and closes the form.
		/// </summary>
		/// <param name="sender">OK Button.</param>
		/// <param name="e">Empty event arguments.</param>
		protected void cmdOK_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				for (int ctr = _displayOrder.Count - 1; ctr >= 0; ctr-- )
				{
					ucOMSTypeDisplay display = _displayOrder[ctr];
					display.UpdateItem();
				}
				this.DialogResult = DialogResult.OK;

                if (ucFormStorage1 != null)
                {
                    ucFormStorage1.SaveNow();
                }
                
                this.Close();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		
		}


		/// <summary>
		/// Saves all the open display control and their contents and keeps the form open.
		/// This is similar to an apply button.
		/// </summary>
		/// <param name="sender">Save button.</param>
		/// <param name="e">Empty event arguments.</param>
		protected void cmdSave_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
			
				Save();
			}
			catch (Exception ex)
			{
				// DMB 24/02/2004 added a check to see if this was called by pressing the button or called
				// from another function
				if(sender is frmOMSType)
					throw ex;	
				else
					ErrorBox.Show(this, ex, frmOMSType.savetrace);

			}
			finally
			{
				Cursor = Cursors.Default;
			}

		}

		/// <summary>
		/// Refreshes the whole dialog form.
		/// </summary>
		/// <param name="sender">Refresh button.</param>
		/// <param name="e">Empty event arguments.</param>
		protected void cmdRefresh_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				Refresh();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Cancels any changes for the whole of the dialog form.
		/// </summary>
		/// <param name="sender">Cancel button.</param>
		/// <param name="e">Empty event arguments.</param>
		protected void cmdCancel_Click(object sender, System.EventArgs e)
		{
			Cancel();
		}

		/// <summary>
		/// Captures all the toolbar buttons clicks.
		/// </summary>
		/// <param name="sender">Toolbar button.</param>
		/// <param name="e">Tool bar button event arguments.</param>
		private void tbDialogs_ButtonClick(object sender, System.Windows.Forms.ToolBarButtonClickEventArgs e)
		{
			try
			{
				pnlTop.Focus();
				if (e.Button == cmdSearch) 
				{
					ShowSearch();
				}
				if (e.Button == cmdRefresh) cmdRefresh_Click(sender,e);
				if (e.Button == cmdBack) cmdBack_Click(sender,e);
				if (e.Button == cmdOK) cmdOK_Click(sender,e);
				if (e.Button == cmdSave) cmdSave_Click(sender,e);
				if (e.Button == cmdCancel) cmdCancel_Click(sender,e);
				if (e.Button == cmdCmdCentre)
				{
					ShowCommandCentre();
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this,ex);
			}
		}


		/// <summary>
		/// Back Button Drop Menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pnlSelect_Click(object sender, System.EventArgs e)
		{
			int b = cmenu.MenuItems.Count - (((MenuItem)sender).Index );
			for (int a = 0;a < b;a++)
			{
				this.cmdBack_Click(sender,e);
				Application.DoEvents();
			}

		}

		#endregion

		#region Private Events

        private void frmOMSType_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            quickCancel.Focus();
            Services.BackForwardMouse.BackButtonClicked -= new EventHandler(BackForwardMouse_BackButtonClicked);
            try
            {
                if (Session.CurrentSession.IsLoggedIn)
                {
                    Cursor = Cursors.WaitCursor;
                    for (int ctr = _displayOrder.Count - 1; ctr >= 0; ctr--)
                    {
                        ucOMSTypeDisplay display = _displayOrder[ctr];

                        //Check for dirty data before cancelling.
                        if (display.IsDirty)
                        {
                            DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAPRM", "Changes have been detected to %1%, would you like to save?", "", display.ObjectTypeDescription), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                            switch (res)
                            {
                                case DialogResult.Yes:
                                    // DMB 24/02/2004 
                                    // Added a try catch around this to catch save errors and stop form closing
                                    try
                                    {
                                        cmdSave_Click(this, EventArgs.Empty);
                                        cmdBack_Click(this, EventArgs.Empty);
                                    }
                                    catch (Exception exe)
                                    {
                                        ErrorBox.Show(this, exe);
                                        e.Cancel = true;
                                    }
                                    break;
                                case DialogResult.No:
                                    display.CancelItem();
                                    cmdBack_Click(this, EventArgs.Empty);
                                    break;
                                case DialogResult.Cancel:
                                    {
                                        e.Cancel = true;
                                        return;
                                    }
                            }
                        }
                    }
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
            finally
            {
                
                Cursor = Cursors.Default;
                
                this.Dispose();
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();

            }
        }

		private void BackForwardMouse_BackButtonClicked(object sender, EventArgs e)
		{
			if (cmdBack.Enabled)
				cmdBack_Click(this,e);
		}

		private void frmOMSType_Load(object sender, System.EventArgs e)
		{
			Services.BackForwardMouse.BackButtonClicked +=new EventHandler(BackForwardMouse_BackButtonClicked);
			ucOMSTypeDefault.SearchManagerVisibleChanged += new EventHandler(active_SearchManagerVisibleChanged);
            
            if (_style != SearchManager.None)
			{
				ucOMSTypeDefault.ShowSearchManager(_style);
			}
			ucOMSTypeDefault.TabControlFocus();
		}

		private void active_SearchManagerVisibleChanged(object sender, EventArgs e)
		{
            cmdSearch.Pushed = ((ucOMSTypeDisplay)sender).SearchManagerVisible || ((ucOMSTypeDisplay)sender).ElasticsearchVisible;
            cmdSave.Enabled = !cmdSearch.Pushed;
            cmdCancel.Enabled = !cmdSearch.Pushed;
            cmdRefresh.Enabled = !cmdSearch.Pushed;
		}
		#endregion

		#region IOMSTypeWindow Members

		/// <summary>
		/// Sets a specificly named tab page.
		/// </summary>
		/// <param name="name">The code name of the page to set.</param>
		public void SetTabPage(string name)
		{
			ucOMSTypeDisplay active = _displayOrder[_displayOrder.Count - 1];
			active.SetTabPage(name);
		}

        public void GotoTab(string Code)
        {
            ucOMSTypeDisplay active = _displayOrder[_displayOrder.Count - 1];
            active.GotoTab(Code);
        }

        public IOMSItem GetTabsOMSItem(string Code)
        {
            ucOMSTypeDisplay active = _displayOrder[_displayOrder.Count - 1];
            return active.GetTabsOMSItem(Code);
        }

        public void Back()
        {
            if (cmdBack.Enabled)
            {
                try
                {
                    Cursor = Cursors.WaitCursor;
                    if (_displayOrder != null)
                    {
                        //added DMB 10/2/2004 to allow back button to work when search screen is shown
                        if (cmdSearch.Pushed && cmdSearch.Visible && (_displayOrder[_displayOrder.Count - 1].SearchManagerVisible || _displayOrder[_displayOrder.Count - 1].ElasticsearchVisible))
                        {
                            cmdSearch.Pushed = false;
                            ShowSearch();
                            return;
                        }

                        ucOMSTypeDisplay active = _displayOrder[_displayOrder.Count - 1];
                        
                        cmdSearch.Pushed = false;

                        if (active.Object != null)
                        {
                            //Check dirty data before going back through the history.
                            if (active.IsDirty)
                            {
                                DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAPRM", "Changes have been detected to %1%, would you like to save?", "", active.ObjectTypeDescription), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                                switch (res)
                                {
                                    case DialogResult.Yes:
                                        active.UpdateItem();
                                        break;
                                    case DialogResult.No:
                                        active.CancelItem();
                                        break;
                                    case DialogResult.Cancel:
                                        return;
                                }
                            }
                        }

                        this.cmenu.MenuItems.RemoveAt(this.cmenu.MenuItems.Count - 1);
                        _displayOrder.Remove(active);

                        cmdBack.Enabled = false;
                        // If the Active Display is the Command Centre then on Back Enabled Button
                        if (active == CmdCentre) cmdCmdCentre.Enabled = true;

                        UnattachDisplayEvents(active);
                        active.Parent.Controls.Remove(active);
                        active.Dispose();

                        active = _displayOrder[_displayOrder.Count - 1];
                        active.Enabled = true;
                        if (active == _omstypewithsearch)
                        {
                            if (ucOMSTypeDefault.Object != null) cmdSearch.Enabled = true;
                            cmdSearch.Pushed = Convert.ToBoolean(cmdSearch.Tag);
                        }
                        Text = active.ObjectTypeCaption;
                        if (active.Object != null)
                        {
                            Icon = active.ObjectTypeIcon;
                            active.Object.SetCurrentSessions();
                        }
                        
                        cmdSearch.Pushed = active.SearchManagerVisible || active.ElasticsearchVisible;
                        cmdSave.Enabled = !cmdSearch.Pushed;
                        cmdCancel.Enabled = !cmdSearch.Pushed;
                        active.Focus();

                        SetNavigationButtonState(); 
                        stBar.Panels[0].Text = "";
                    }
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

		public void Save()
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				savetrace.Clear();
				for (int ctr = _displayOrder.Count - 1; ctr >= 0; ctr-- )
				{
					ucOMSTypeDisplay display = _displayOrder[ctr];
					if (display.Object != null)
					{
						savetrace.Add(display.ObjectTypeDescription);
						display.UpdateItem();
						display.RefreshItem(true);
					}
				}	
		
                //UTCFIX: DM - 30/11/06 - No fix required local time displayed as it should.
				stBar.Panels[0].Text = Session.CurrentSession.Resources.GetResource("INFOSAVEDTIME", "Last Saved at %1% ...","", DateTime.Now.ToLongTimeString()).Text;
			}
			finally
			{
				Cursor = Cursors.Default;
			}

		}

		new public void Refresh()
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				for (int ctr = _displayOrder.Count - 1; ctr >= 0; ctr-- )
				{
					ucOMSTypeDisplay display = _displayOrder[ctr];
					
					//Check for dirty data before refreshing the data.
					if (display.IsDirty)
					{
						DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAPRM", "Changes have been detected to %1%, would you like to save?","",display.ObjectTypeDescription), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
			
						switch (res)
						{
							case DialogResult.Yes:
								cmdSave_Click(this, EventArgs.Empty);
								break;
							case DialogResult.No:
								display.CancelItem();
								break;
							case DialogResult.Cancel:
								return;
						}				
					}
					display.RefreshItem();
				}			
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		public void Cancel()
		{
			this.Close();
		}

		public void ShowCommandCentre()
		{
			NewOMSTypeWindowEventArgs r = new NewOMSTypeWindowEventArgs(Session.CurrentSession.CurrentUser, Session.CurrentSession.CurrentUser.CommandCentre, "");
			ucOMSTypeDefault_NewOMSTypeWindow(null,r);
		}
        
        public void ShowSearch()
		{
			try
			{
                if (Session.CurrentSession.IsSearchConfigured)
                {
                    if (Session.CurrentSession.SearchButtonUseSearchManager && cmdSearch.Pushed)
                    {
                        _displayOrder[_displayOrder.Count - 1].SearchManagerVisible = true;
                    }
                    else if (Session.CurrentSession.SearchButtonUseSearchManager == false && cmdSearch.Pushed)
                    {
                        _displayOrder[_displayOrder.Count - 1].ElasticsearchVisible = true;
                    }
                    else
                    {
                        if (_displayOrder[_displayOrder.Count - 1].SearchManagerVisible)
                            _displayOrder[_displayOrder.Count - 1].SearchManagerVisible = false;
                        if (_displayOrder[_displayOrder.Count - 1].ElasticsearchVisible)
                            _displayOrder[_displayOrder.Count - 1].ElasticsearchVisible = false;
                    }
                }
                else
                {
                    _displayOrder[_displayOrder.Count - 1].SearchManagerVisible = cmdSearch.Pushed;
                }
                
                if (cmdSearch.Pushed)
                {
                    _omstypewithsearch = _displayOrder[_displayOrder.Count - 1];
                    if (_omstypewithsearch.ElasticsearchVisible == false)
                        _omstypewithsearch.RefreshSearchManager();
                    else
                        _omstypewithsearch.RefreshElasticsearch();
                }
                else
                {
                    _omstypewithsearch = null;
                }
				cmdSave.Enabled=!cmdSearch.Pushed;
				cmdCancel.Enabled=!cmdSearch.Pushed;
			}
			catch (Exception ex)
			{
				cmdSearch.Pushed = false;
				throw ex;
			}

			//added DMB 10/2/2004
            SetNavigationButtonState();
		}
		#endregion
	}
}

