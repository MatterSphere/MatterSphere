using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    public sealed partial class ucOMSTypeEmbeded : UserControl, Interfaces.IOMSTypeWindow
	{
        #region Control Fields
		/// <summary>
		/// The Display Type that Contains the Search Window
		/// </summary>
        private IEmbeddedOMSTypeDisplay omsTypeWithSearch = null;

        /// <summary>
        /// Holds the display order of object display controls.
        /// </summary>
        private DisplayCollection<IEmbeddedDisplay> _displayCollection;

        /// <summary>
        /// Returns the Last OMS Type Display 
        /// </summary>
        private IEmbeddedOMSTypeDisplay lastDisplay;

		/// <summary>
		/// Returns the Command Centre OMS Type Display 
		/// </summary>
        private Interfaces.IOMSTypeDisplay cmdCentre;

        #endregion

        private readonly PageManager<IEmbeddedDisplay> _pageManager;

        #region Constructors

        /// <summary>
        /// Default contructor.
        /// </summary>
        public ucOMSTypeEmbeded()
        {
            InitializeComponent();

            tbLeft.ImageList = FWBS.OMS.UI.Windows.Images.Windows8();
            tbRight.ImageList = FWBS.OMS.UI.Windows.Images.Windows8();

            _displayCollection = new DisplayCollection<IEmbeddedDisplay>();
            _pageManager = new PageManager<IEmbeddedDisplay>(null, _displayCollection);
            FWBS.OMS.UI.Windows.Services.OMSTypeRefresh += new EventHandler(cmdRefresh_Click);
            if (Session.CurrentSession.IsLoggedIn)
            {
                cmdCmdCentre.Visible = Session.CurrentSession.IsPackageInstalled("COMMANDCENTRE");
                cmdSearch.Visible = (!Session.CurrentSession.CurrentBranch.DisableSearchButton);
            }
            Sp2.Visible = cmdCmdCentre.Visible;

            this.ucOMSTypeDefault.CmdButtonsActions(cmdSave_Click, cmdCancel_Click);
        }
		#endregion

		#region Methods

		/// <summary>
		/// Captures any new dialog window request from the child user controls.
		/// </summary>
		/// <param name="sender">Display control instance.</param>
		/// <param name="e">NewOMSTypeWindowEventArgs event arguments.</param>
		private void ucOMSTypeDefault_NewOMSTypeWindow(object sender, FWBS.OMS.UI.Windows.NewOMSTypeWindowEventArgs e)
		{
            IEmbeddedOMSTypeDisplay active = _displayCollection.LastDisplay;
            active.Enabled = false;
            stBar.Panels[0].Text = "";

			MenuItem mnui = new MenuItem();
            mnui.Text = active.ObjectTypeDescription;
			this.cmenu.MenuItems.Add(_displayCollection.Count - 1,mnui);
			mnui.Click += new System.EventHandler(this.pnlSelect_Click);

            var display = new ucOMSTypeDisplayV2()
            {
                Location = ucOMSTypeDefault.Location,
                Size = ucOMSTypeDefault.Size
            };

            display.SuspendLayout();
            display.TabPositions = this.TabPosition;
            display.AlertsVisible = this.AlertsVisible;
            //Set the default properties of the new display item and make it visible.
            display.ipc_BackColor = System.Drawing.Color.Gray;
            display.ipc_Width = 157;

            display.Dirty += new EventHandler(ucOMSTypeDefault_Dirty);
            display.NewOMSTypeWindow += new FWBS.OMS.UI.Windows.NewOMSTypeWindowEventHandler(this.ucOMSTypeDefault_NewOMSTypeWindow);
            display.SearchCompleted += new EventHandler(ucOMSTypeDefault_SearchCompleted);
            display.SearchManagerVisibleChanged += new EventHandler(active_SearchManagerVisibleChanged);

            pnlMain.Controls.Add((Control)display);
            display.Open(e.OMSObject, e.OMSType);
            display.BringToFront();
            display.Dock = DockStyle.Fill;
            display.ResumeLayout();

			Global.RightToLeftControlConverter((Control)display, ParentForm);
			_displayCollection.Add(display);
			display.ipc_Visible = true;
			SetNavigationButtonState();

		    display.Visible=true;
            active.Visible = false;

            if (_displayCollection.LastDisplay == omsTypeWithSearch && cmdSearch.Pushed)
			{
				cmdSearch.Enabled=false;
			}

			cmdSave.Enabled=true;
			//added DMB 2/2/2004
			cmdRefresh.Enabled=true;
			lastDisplay = display;
			if (e.OMSType is FWBS.OMS.CommandCentreType)
			{
				cmdCentre = (IOMSTypeDisplay)lastDisplay;
				cmdCmdCentre.Enabled = false;
                _pageManager.ShowPage(ViewEnum.Default);
            }
        }


		/// <summary>
		/// Disables the navigational buttons if there is only one object display control available.
		/// </summary>
		private void SetNavigationButtonState()
		{
			if (_displayCollection.Count > 1 || (cmdSearch.Pushed && cmdSearch.Visible))
				cmdBack.Enabled = true;
			else
				cmdBack.Enabled = false;
		}


		/// <summary>
		/// Disposes of the active display control.
		/// </summary>
		/// <param name="sender">Back button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdBack_Click(object sender, System.EventArgs e)
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
					ErrorBox.Show(ParentForm, ex);
				}
				finally
				{
					Cursor = Cursors.Default;
				}
			}
		}


		/// <summary>
		/// Saves all the open display control and their contents and keeps the form open.
		/// This is similar to an apply button.
		/// </summary>
		/// <param name="sender">Save button.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdSave_Click(object sender, System.EventArgs e)
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
                    ErrorBox.Show(ParentForm, ex, frmOMSType.savetrace);
			}
			finally
			{
				Cursor = Cursors.Default;
			}

		}

        private void cmdCancel_Click(object sender, System.EventArgs e)
        {
            Cancel();
        }

        /// <summary>
        /// Refreshes the whole dialog form.
        /// </summary>
        /// <param name="sender">Refresh button.</param>
        /// <param name="e">Empty event arguments.</param>
        private void cmdRefresh_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;

				Refresh();
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
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
				if (e.Button == cmdSave) cmdSave_Click(sender,e);
				if (e.Button == cmdCmdCentre)
				{
					ShowCommandCentre();
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
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

        private void ucOMSTypeDefault_SearchCompleted(object sender, EventArgs e)
        {
        }
        
        private void ParentForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !CanClose(false);
        }
        
        /// <summary>
        /// Raise the Close Method which will validate if the Form Can be closed or if its dirty
        /// </summary>
        /// <returns>Returns True if the form should not be closed</returns>
        public bool CanClose(bool cantCancel)
        {
            try
            {
                if (Session.CurrentSession.IsLoggedIn)
                {
                    UpdateBinding();
                    
                    Cursor = Cursors.WaitCursor;
                    for (int ctr = _displayCollection.Count - 1; ctr >= 0; ctr--)
                    {
                        IEmbeddedOMSTypeDisplay display = _displayCollection[ctr];

                        //Check for dirty data before cancelling.
                        if (display.IsDirty)
                        {
                            DialogResult res = MessageBox.Show(Session.CurrentSession.Resources.GetMessage("DIRTYDATAPRM", "Changes have been detected to %1%, would you like to save?", "", display.ObjectTypeDescription),
                                FWBS.OMS.Global.ApplicationName, cantCancel ? MessageBoxButtons.YesNo : MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

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
                                        ErrorBox.Show(ParentForm, exe);
                                        return false;
                                    }
                                    break;
                                case DialogResult.No:
                                    display.CancelItem();
                                    cmdBack_Click(this, EventArgs.Empty);
                                    break;
                                case DialogResult.Cancel:
                                    {
                                        return false;
                                    }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
                return false;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
            return true;
        }


        private void UpdateBinding()
        {
            int timeout = 0;
            do
            {
                Blur.Focus();
                Application.DoEvents();
            } while (Blur.Focused == false && timeout++ < 1000);
        }


		private void ucOMSTypeEmbeded_Load(object sender, System.EventArgs e)
		{
            if (OMS.Session.CurrentSession.IsLoggedIn)
            {
                cmdCmdCentre.Text = Session.CurrentSession.Resources.GetResource("CmdCentre", "&Command Centre", "").Text;
                cmdBack.Text = Session.CurrentSession.Resources.GetResource("CMDBACK", "&Back", "").Text;
                cmdRefresh.Text = Session.CurrentSession.Resources.GetResource("CMDREFRESH", "&Refresh", "").Text;
                cmdSave.Text = Session.CurrentSession.Resources.GetResource("CMDSAVE", "&Save", "").Text;
            }
            
			ucOMSTypeDefault.SearchManagerVisibleChanged += new EventHandler(active_SearchManagerVisibleChanged);
			if (_style != SearchManager.None)
			{
				ucOMSTypeDefault.ShowSearchManager(_style);
			}

            int _pnlTBRight = LogicalToDeviceUnits(5);
            foreach (ToolBarButton tb in tbRight.Buttons)
            {
                if (tb.Visible)
                    _pnlTBRight = _pnlTBRight + tb.Rectangle.Width;
            }
            int _pnlTBLeft = LogicalToDeviceUnits(5);
            foreach (ToolBarButton tb in tbLeft.Buttons)
            {
                if (tb.Visible)
                    _pnlTBLeft = _pnlTBLeft + tb.Rectangle.Width;
            }
            pnlTBRight.Width = _pnlTBRight;
            pnlTBLeft.Width = _pnlTBLeft;
            pnlTop.Height = tbLeft.Height + LogicalToDeviceUnits(2);
		}


		private void active_SearchManagerVisibleChanged(object sender, EventArgs e)
		{
            cmdSearch.Pushed = ((IEmbeddedOMSTypeDisplay)sender).SearchManagerVisible || ((IEmbeddedOMSTypeDisplay)sender).ElasticsearchVisible;
            cmdSave.Enabled = !cmdSearch.Pushed;
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
			IEmbeddedOMSTypeDisplay active = _displayCollection.LastDisplay;
			active.SetTabPage(name);
		}


        public void GotoTab(string Code)
        {
            IEmbeddedOMSTypeDisplay active = _displayCollection.LastDisplay;
            active.GotoTab(Code);
        }


        public IOMSItem GetTabsOMSItem(string Code)
        {
            IEmbeddedOMSTypeDisplay active = _displayCollection.LastDisplay;
            return active.GetTabsOMSItem(Code);
        }


        public void Back()
        {
            if (cmdBack.Enabled)
            {
                try
                {
                    //added DMB 10/2/2004 to allow back button to work when search screen is shown
                    if (cmdSearch.Pushed && (cmdSearch.Visible && ((IEmbeddedOMSTypeDisplay)_displayCollection.LastDisplay).SearchManagerVisible || _displayCollection.LastDisplay.ElasticsearchVisible))
                    {
                        cmdSearch.Pushed = false;
                        ShowSearch();
                        return;
                    }

                    Cursor = Cursors.WaitCursor;
                    if (_displayCollection != null)
                    {
                        IEmbeddedDisplay active = _displayCollection.LastDisplay;
                        
                        //cmdSearch.Pushed = false;

                        if (((IEmbeddedOMSTypeDisplay)active).Object != null)
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
                        _displayCollection.Remove(active);
                        _displayCollection.LastDisplay.Visible = true;
                        _pageManager.SetViewType(ViewEnum.Default);

                        cmdBack.Enabled = false;
                        // If the Active Display is the Command Centre then on Back Enabled Button
                        if (active == cmdCentre) cmdCmdCentre.Enabled = true;

                        active.Dirty -= new EventHandler(ucOMSTypeDefault_Dirty);
                        active.NewOMSTypeWindow -= new FWBS.OMS.UI.Windows.NewOMSTypeWindowEventHandler(this.ucOMSTypeDefault_NewOMSTypeWindow);
                        active.SearchCompleted -= new EventHandler(ucOMSTypeDefault_SearchCompleted);
                        active.SearchManagerVisibleChanged -= new EventHandler(active_SearchManagerVisibleChanged);

                        active.Parent.Controls.Remove(active as Control);
                        active.Dispose();

                        active = _displayCollection.LastDisplay;
                        active.Enabled = true;
                        if (active == omsTypeWithSearch)
                        {
                            if (ucOMSTypeDefault.Object != null) cmdSearch.Enabled = true;
                            cmdSearch.Pushed = Convert.ToBoolean(cmdSearch.Tag);
                        }
                        Text = active.ObjectTypeDescription;
                        if (((IEmbeddedOMSTypeDisplay)active).Object != null)
                        {
                            ((IEmbeddedOMSTypeDisplay)active).Object.SetCurrentSessions();
                        }

                        cmdSearch.Pushed = ((IEmbeddedOMSTypeDisplay)active).SearchManagerVisible || active.ElasticsearchVisible;
                        cmdSave.Enabled = !cmdSearch.Pushed;
                        ((Control)active).Focus();

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
                UpdateBinding();
                Cursor = Cursors.WaitCursor;
                frmOMSType.savetrace.Clear();
				for (int ctr = _displayCollection.Count - 1; ctr >= 0; ctr-- )
				{
					IEmbeddedOMSTypeDisplay display = _displayCollection[ctr];
					if (display.Object != null)
					{
                        frmOMSType.savetrace.Add(display.ObjectTypeDescription);
						display.UpdateItem();
						display.RefreshItem(true);
					}
				}	
		
                //UTCFIX: DM - 30/11/06 - No fix required local time displayed as it should.
				stBar.Panels[0].Text = Session.CurrentSession.Resources.GetResource("INFOSAVEDTIME", "Last Saved at %1% ...","", DateTime.Now.ToLongTimeString()).Text;
                this.IsDirty = false;
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
                UpdateBinding(); 
                Cursor = Cursors.WaitCursor;
				for (int ctr = _displayCollection.Count - 1; ctr >= 0; ctr-- )
				{
                    IEmbeddedOMSTypeDisplay display = _displayCollection[ctr];
					
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
                                Cancel();
								break;
							case DialogResult.Cancel:
								return;
						}				
					}
					display.RefreshItem();
                    IsDirty = false;
				}			
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}


		public void Cancel()
		{
            for (int ctr = _displayCollection.Count - 1; ctr >= 0; ctr--)
            {
                IEmbeddedOMSTypeDisplay display = _displayCollection[ctr];
                if (display.Object != null)
                {
                    display.CancelItem();
                }
            }
            this.IsDirty = false;
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
                        ((IEmbeddedOMSTypeDisplay)_displayCollection.LastDisplay).SearchManagerVisible = true;
                    }
                    else if (Session.CurrentSession.SearchButtonUseSearchManager == false && cmdSearch.Pushed)
                    {
                        _displayCollection.LastDisplay.ElasticsearchVisible = true;
                    }
                    else
                    {
                        if (((IEmbeddedOMSTypeDisplay)_displayCollection.LastDisplay).SearchManagerVisible)
                            ((IEmbeddedOMSTypeDisplay)_displayCollection.LastDisplay).SearchManagerVisible = false;
                        if (_displayCollection.LastDisplay.ElasticsearchVisible)
                            _displayCollection.LastDisplay.ElasticsearchVisible = false;
                    }
                }
                else
                {
                    ((IEmbeddedOMSTypeDisplay)_displayCollection.LastDisplay).SearchManagerVisible = cmdSearch.Pushed;
                }
                
                if (cmdSearch.Pushed)
                {
                    omsTypeWithSearch = _displayCollection.LastDisplay;
                    if (omsTypeWithSearch.ElasticsearchVisible == false)
                        omsTypeWithSearch.RefreshSearchManager();
                    else
                        omsTypeWithSearch.RefreshElasticsearch();
                }
                else
                {
                    omsTypeWithSearch = null;
                }
				cmdSave.Enabled=!cmdSearch.Pushed;
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

        #region Properties
        private bool refreshButtonVisible = true;
        [Category("Buttons")]
        [Description("Show or hide the refresh button")]
        public bool RefreshButtonVisible 
        {
            get
            {
                return refreshButtonVisible;
            }
            set
            {
                refreshButtonVisible = value;
                if (Parent != null)
                    ucOMSTypeEmbeded_ParentChanged(this, EventArgs.Empty);
            }
        }

        private bool backButtonVisible = true;
        [Category("Buttons")]
        [Description("Show or hide the back button")]
        public bool BackButtonVisible
        {
            get
            {
                return backButtonVisible;
            }
            set
            {
                backButtonVisible = value;
                if (Parent != null)
                    ucOMSTypeEmbeded_ParentChanged(this, EventArgs.Empty);
            }
        }

        private bool commandCentreButtonVisible = true;
        [Category("Buttons")]
        [Description("Show or hide the Command Centre button")]
        public bool CommandCentreButtonVisible
        {
            get
            {
                return commandCentreButtonVisible;
            }
            set
            {
                commandCentreButtonVisible = value;
                if (Parent != null)
                    ucOMSTypeEmbeded_ParentChanged(this, EventArgs.Empty);
            }
        }

        private bool searchButtonVisible = true;
        [Category("Buttons")]
        [Description("Show or hide the Search button")]
        public bool SearchButtonVisible
        {
            get
            {
                return searchButtonVisible;
            }
            set
            {
                searchButtonVisible = value;
                if (Parent != null)
                    ucOMSTypeEmbeded_ParentChanged(this, EventArgs.Empty);
            }
        }

        private bool saveButtonVisible = true;
        [Category("Buttons")]
        [Description("Show or hide the Save button")]
        public bool SaveButtonVisible
        {
            get
            {
                return saveButtonVisible;
            }
            set
            {
                saveButtonVisible = value;
                ucOMSTypeEmbeded_ParentChanged(this, EventArgs.Empty);
            }
        }
        
        private bool infoButtonVisible = true;
        [Category("Buttons")]
        [Description("Show or hide the Info button")]
        public bool InfoButtonVisible
        {
            get
            {
                return infoButtonVisible;
            }
            set
            {
                infoButtonVisible = value;
                if (Parent != null)
                    ucOMSTypeEmbeded_ParentChanged(this, EventArgs.Empty);
            }
        }

        private bool toolBarVisible = true;
        [Description("Show or hide the ToolBar")]
        [Category("Toolbar")]
        public bool ToolBarVisible
        {
            get
            {
                return toolBarVisible;
            }
            set
            {
                toolBarVisible = value;
                if (Parent != null)
                {
                    pnlTop.Visible = value;
                }
            }
        }

        private bool leftToolBarVisible = true;
        [Category("Toolbar")]
        [Description("Show or hide the Left Tool Bar")]
        public bool LeftToolBarVisible
        {
            get
            {
                return leftToolBarVisible;
            }
            set
            {
                leftToolBarVisible = value;
                if (Parent != null)
                    pnlTBLeft.Visible = value;
            }
        }

        private bool statusBarVisible = true;
        [Category("Appearance")]
        [Description("Show or hide the Status Bar")]
        public bool StatusBarVisible
        {
            get
            {
                return statusBarVisible;
            }
            set
            {
                statusBarVisible = value;
                if (Parent != null)
                    stBar.Visible = value;
            }
        }

        private bool rightToolBarVisible = true;

        [Category("Toolbar")]
        [Description("Show or hide the Right Tool Bar")]
        public bool RightToolBarVisible
        {
            get
            {
                return rightToolBarVisible;
            }
            set
            {
                rightToolBarVisible = value;
                if (Parent != null)
                    pnlTBRight.Visible = value;
            }
        }

        private bool infoPanelVisible = true;
        [Category("Info Panel")]
        [Description("Show or hide the Info Panel")]
        public bool InfoPanelVisible
        {
            get
            {
                return infoPanelVisible;
            }
            set
            {
                infoPanelVisible = value;
                if (Parent != null)
                {
                    ucOMSTypeDefault.InformationPanelVisible = value;
                    foreach (var i in _displayCollection)
                        i.InformationPanelVisible = value;
                }
            }
        }

        private TabAlignment tabPosition = TabAlignment.Top;
        [Category("Appearance")]
        [Description("Show or hide the Info Panel")]
        public TabAlignment TabPosition
        {
            get
            {
                return tabPosition;
            }
            set
            {
                tabPosition = value;
                if (Parent != null)
                {
                    ucOMSTypeDefault.TabPositions = value;
                    foreach (var i in _displayCollection)
                        i.TabPositions = value;
                }
            }
        }

        private bool autoLoad = true;
        [Category("OMS")]
        [Description("Auto Load from Enquiry Form")]
        public bool AutoLoad
        {
            get
            {
                return autoLoad;
            }
            set
            {
                autoLoad = value;
            }
        }

        private bool alertsVisible = true;
        [Category("Appearance")]
        [Description("Show or hide the Alerts")]
        public bool AlertsVisible
        {
            get
            {
                return alertsVisible;
            }
            set
            {
                alertsVisible = value;
                if (Parent != null)
                {
                    ucOMSTypeDefault.AlertsVisible = value;
                    foreach (var i in _displayCollection)
                        i.AlertsVisible = value;
                }
            }
        }

        private bool isdirty = false;

        [Browsable(false)]
        public bool IsDirty
        {
            get
            {
                return isdirty;
            }
            private set 
            {
                if (isdirty != value)
                {
                    isdirty = value;
                    OnIsDirty();
                }
            }
        }

        private void OnIsDirty()
        {
            if (Dirty != null)
                Dirty(this, EventArgs.Empty);
        }

        public event EventHandler Dirty;

        private Form parentform = null;
        private EnquiryForm enquiryform = null;

        private void ucOMSTypeEmbeded_ParentChanged(object sender, EventArgs e)
        {
            // Set Properties
            if (Parent != null)
            {
                foreach (var i in _displayCollection)
                {
                    i.TabPositions = TabPosition;
                    i.AlertsVisible = AlertsVisible;
                    i.InformationPanelVisible = InfoPanelVisible;
                }

                if (ucOMSTypeDefault != null)
                {
                    ucOMSTypeDefault.TabPositions = TabPosition;
                    ucOMSTypeDefault.AlertsVisible = AlertsVisible;
                    ucOMSTypeDefault.InformationPanelVisible = InfoPanelVisible;
                    ucOMSTypeDefault.Dirty += new EventHandler(ucOMSTypeDefault_Dirty);
                }

                pnlTBRight.Visible = RightToolBarVisible;
                pnlTBLeft.Visible = LeftToolBarVisible;
                cmdSearch.Visible = SearchButtonVisible;
                cmdCmdCentre.Visible = CommandCentreButtonVisible;
                Sp2.Visible = CommandCentreButtonVisible;
                sp1.Visible = InfoButtonVisible;
                cmdBack.Visible = BackButtonVisible;
                cmdRefresh.Visible = RefreshButtonVisible;
                pnlTop.Visible = ToolBarVisible;
                stBar.Visible = StatusBarVisible;
                pnlTBRight.Visible = SaveButtonVisible;

                int _pnlTBRight = 5;
                foreach (ToolBarButton tb in tbRight.Buttons)
                {
                    if (tb.Visible)
                        _pnlTBRight = _pnlTBRight + tb.Rectangle.Width;
                }
                int _pnlTBLeft = 5;
                foreach (ToolBarButton tb in tbLeft.Buttons)
                {
                    if (tb.Visible)
                        _pnlTBLeft = _pnlTBLeft + tb.Rectangle.Width;
                }
                pnlTBRight.Width = _pnlTBRight;
                pnlTBLeft.Width = _pnlTBLeft;
            }

            // Capture the ParentForms Close Events
            if (ParentForm != null)
            {
                parentform = ParentForm;
                this.ParentForm.FormClosing += new FormClosingEventHandler(ParentForm_Closing);
            }
            else if (parentform != null)
            {
                this.parentform.FormClosing -= new FormClosingEventHandler(ParentForm_Closing);
                this.parentform = null;
            }

            
            if (enquiryform != null && Parent == null)
            {
                enquiryform.Cancelled -= new EventHandler(enquiryform_Cancelled);
                enquiryform.Updating -= new CancelEventHandler(enquiryform_Updating);
                enquiryform = null;
            }
            else
            {
                enquiryform = Parent as EnquiryForm;
                if (enquiryform != null)
                {
                    if (this.parentform != null)
                        this.parentform.FormClosing -= new FormClosingEventHandler(ParentForm_Closing);
                    enquiryform.Updating += new CancelEventHandler(enquiryform_Updating);
                    enquiryform.Cancelled += new EventHandler(enquiryform_Cancelled);
                    if (enquiryform.Enquiry == null || autoLoad == false) return;
                    FWBS.OMS.Interfaces.IOMSType iobj = enquiryform.Enquiry.Object as FWBS.OMS.Interfaces.IOMSType;
                    if (iobj == null) return;
                    Connect(iobj);
                }
            }
        }

        private void enquiryform_Cancelled(object sender, EventArgs e)
        {
            Cancel();
        }

        private void ucOMSTypeDefault_Dirty(object sender, EventArgs e)
        {
            if (enquiryform != null)
                enquiryform.IsDirty = true;
            this.IsDirty = true;
        }

        private void enquiryform_Updating(object sender, CancelEventArgs e)
        {
            Save();
        }

        public void Connect(FWBS.OMS.Interfaces.IOMSType obj)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                ucOMSTypeDefault.Open(obj, obj.GetOMSType());
                Text = ucOMSTypeDefault.ObjectTypeDescription;
                _displayCollection.Add(ucOMSTypeDefault);
                SetNavigationButtonState();
                lastDisplay = ucOMSTypeDefault;
                cmdBack.Enabled = false;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        #endregion
    }
}
