using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.Common.UI;
using FWBS.OMS.Security.Permissions;
using FWBS.OMS.UI.Windows.DocumentManagement.Addins;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// 35000 The main precedent manager library User Control
    /// </summary>
    public class ucPrecedent : ucBaseAddin
	{
		#region Events
		public event EventHandler SelectedItemDoubleClick;

		private void OnSelectedItemDoubleClick()
		{
            if (SelectedItemDoubleClick != null)
            {
                SelectedItemDoubleClick(this, EventArgs.Empty);
            }
            else
            {
                if (!_isAddin) ParentForm.DialogResult = DialogResult.OK;
                btnContinue_Click(this, EventArgs.Empty);
            }
		}

        public event EventHandler SearchCompleted;

        private void OnSearchCompleted()
        {
            SearchCompleted?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Control Fields
        private System.Windows.Forms.Timer timFilterDown;
		private TabControl tcontrol;
		private FWBS.OMS.UI.Windows.ucSearchControl ucPrecList;
        private FWBS.OMS.UI.Windows.ucSearchControl ucPrecFav;    
        private FWBS.OMS.UI.Windows.EnquiryForm enqPrecHeader;
		private System.Windows.Forms.TabPage tpList;
		private Windows.ucNavCmdButtons ucNavCmdButtonSelectAssociate;
		private System.ComponentModel.IContainer components;
        private System.Windows.Forms.Panel pnlList;
		private System.Windows.Forms.Panel pnlTop;
		private System.Windows.Forms.Panel pnlMainLst;
		private System.Windows.Forms.TabPage tpJoblist;
		private System.Windows.Forms.Button btnContinue;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnEdit;
		private System.Windows.Forms.Panel pnlBuilder;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.Label lblSearching;
		private FWBS.OMS.UI.Windows.ucJoblistRepeaterContainer ucJoblist1;
		private System.Windows.Forms.Label labWaiting;
		private System.Windows.Forms.Timer timRunOnce;
		private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.TabPage tpFav;
        #endregion

        #region Fields
        /// <summary>
        /// Last 10 Storage
        /// </summary>
        private Favourites _activeSearchLastTen;
        /// <summary>
        /// Precedent Favourites
        /// </summary>
        private Favourites _precFavourites;
        /// <summary>
		/// Force show the Library Category
		/// </summary>
		private bool _showlibrary = false;
		/// <summary>
		/// If any Control is missing from the Precedent Enquiry Form then simply assign any settings
		/// to this object
		/// </summary>
		private FWBS.Common.UI.Windows.eLabel2 _default = new FWBS.Common.UI.Windows.eLabel2();
        /// <summary>
        /// Reference to the ComboBox Control
        /// </summary>
        private FWBS.Common.UI.Windows.eXPComboBox cLast10 = null;
        /// <summary>
		/// Link to QuickSearch Control on the Enquiry Form
		/// </summary>
		private IBasicEnquiryControl2 cQuickSearch = null;
		/// <summary>
		/// Link to PrecedentType Control on the Enquiry Form
		/// </summary>
		private IBasicEnquiryControl2 cType = null;
		/// <summary>
		/// Link to Precedent Catagory Control on the Enquiry Form
		/// </summary>
		private IBasicEnquiryControl2 cCat = null;
		/// <summary>
		/// Link to Precedent Sub Catagory Control on the Enquiry Form
		/// </summary>
		private IBasicEnquiryControl2 cSubCat = null;
        /// <summary>
        /// Link to Precedent Minor Catagory Control on the Enquiry Form
        /// </summary>
        private IBasicEnquiryControl2 cMinorCat = null;
        /// <summary>
        /// Link to Addressee Control on the Enquiry Form
        /// </summary>
        private IBasicEnquiryControl2 cContactType = null;
		/// <summary>
		/// Link to Addressee Control on the Enquiry Form
		/// </summary>
		private IBasicEnquiryControl2 cAssocType = null;
		/// <summary>
		/// Link to Stage Control on the Enquiry Form (NOTE Invisible)
		/// </summary>
		private IBasicEnquiryControl2 cStage = null;
		/// <summary>
		/// Link to Library Control on the Enquiry Form
		/// </summary>
		private IBasicEnquiryControl2 cLibrary = null;

		/// <summary>
		/// Link to language Control on the Enquiry Form
		/// </summary>
		private IBasicEnquiryControl2 cLang = null;

		/// <summary>
		/// Disables the Search from running again while its searching.
		/// </summary>
		private bool search = false;

		/// <summary>
		/// Forces the search to happen even if the form is invisible.
		/// </summary>
		private bool force = false;

		/// <summary>
		/// The current file being used to determine the look of the precedent library form.
		/// </summary>
		private FWBS.OMS.Associate _assoc;
			
		/// <summary>
		/// The default precedent type being filtered by.
		/// </summary>
		private string _activetype = "";
		
		/// <summary>
		/// The OMS Application used for the precedent manager.
		/// </summary>
		private FWBS.OMS.Interfaces.IOMSApp _app = null;
		/// <summary>
		/// Link to the Reset Button
		/// </summary>
		private System.Windows.Forms.Button btnReset;
		private System.Windows.Forms.Panel pnlImage;
		private System.Windows.Forms.PictureBox picImage;
		/// <summary>
		/// Link to the chkViewPreview
		/// </summary>
		private System.Windows.Forms.CheckBox chkViewPreview;
		/// <summary>
		/// Show or Hides the Additional Info Tab
		/// </summary>
		private bool _additionalinfovisible = true;
		/// <summary>
		/// Show or Hides the Job List Tab
		/// </summary>
		private bool _joblistvisible = true;
		/// <summary>
		/// Show or Hides the Add Job Button
		/// </summary>
		private bool _addjobvisible = true;
		/// <summary>
		/// Show or Hides the Cancel Button
		/// </summary>
		private bool _btncancelvisible = true;
		/// <summary>
		/// Show or Hides the Edit Button
		/// </summary>
		private bool _btneditvisible = true;
		/// <summary>
		/// Show or Hides the View Button
		/// </summary>
		private bool _btnviewvisible = true;
		/// <summary>
		/// Show or Hides the Print Button
		/// </summary>
		private bool _btnprintvisible = true;
        /// <summary>
        /// Show or Hides the Continue Button
        /// </summary>
        private bool _btncontinuevisible = true;
		private System.Windows.Forms.Button btnView;
		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.Panel pnlSp1;
        private ImageList icons;
	    private ImageList imageList1;
        private omsDockManager omsDockManager1;
        private Infragistics.Win.UltraWinDock.AutoHideControl _ucPrecedentAutoHideControl;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucPrecedentUnpinnedTabAreaBottom;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucPrecedentUnpinnedTabAreaTop;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucPrecedentUnpinnedTabAreaRight;
        private Infragistics.Win.UltraWinDock.UnpinnedTabArea _ucPrecedentUnpinnedTabAreaLeft;
        private Infragistics.Win.UltraWinDock.DockableWindow dockableWindow1;
        private Infragistics.Win.UltraWinDock.DockableControlPane dockableControlPane1;
        private Infragistics.Win.UltraWinDock.DockAreaPane dockAreaPane1;

        private DocumentPreviewAddin documentPreviewer1;
		/// <summary>
		/// Set the Global Precednet Library Filter
		/// </summary>
		private string _library = "";
        private Infragistics.Win.UltraWinDock.WindowDockingArea windowDockingArea1;
        private bool _jobListUpdatePending;
        private Timer timer1;
        private bool _isAddin;
        private int _visibleRowCount = 0;
        private Button btnUnlock;
        private Panel pnlFav;
        private string _selectedLibrary = "";
        private const string constCheckFavs = "DCheckPrecFavs";
        
        #endregion

        #region Constructors & Destructors
        /// <summary>
        /// Internal Contructor so the Precednet cannot be created outside OMS.Library other
        /// than throught the Static Service Object
        /// </summary>
        public ucPrecedent()
		{
			InitializeComponent();
            SetDockingFormat();

            if (!Global.IsInDesignMode() && Session.CurrentSession.IsLoggedIn)
            {
                btnUnlock.Visible = Session.CurrentSession.CurrentUser.IsInRoles("ADMIN");
                picImage.Image = Images.GetAdminMenuList((Images.IconSize)LogicalToDeviceUnits(32)).Images[5];
                _activeSearchLastTen = new Favourites("PRECLAST10");
                _precFavourites = new Favourites("PRECFAV");
                pnlButtons.AutoSize = true;
                pnlButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                PrecedentJobList precedentJobList = Session.CurrentSession.CurrentPrecedentJobList;
                precedentJobList.Added += new EventHandler(UpdateJobList);
                precedentJobList.Moved += new EventHandler(UpdateJobList);
                precedentJobList.Removed += new EventHandler(UpdateJobList);
                precedentJobList.Cleared += new EventHandler(UpdateJobList);
            }
        }

        private void SetDockingFormat()
        {
            var dockableControlConfiguration = new DockableControlConfiguration();

            dockableControlConfiguration.SetDockManagerStyle(omsDockManager1, new DockManagerSettings()
            {
                BackColor = Color.Transparent,
                BorderColor = Color.Transparent
            });

            dockableControlConfiguration.SetDockPanelStyle(dockableControlPane1, new DockPanelSettings()
            {
                TabSettings = new DockPanelTabSettings
                {
                    TextTab = dockableControlPane1.Text,
                    BackColor = ColorTranslator.FromHtml("#005A84"),
                    BackColor2 = ColorTranslator.FromHtml("#005A84")
                },
                CaptionSettings = new DockPanelCaptionSettings
                {
                    BackColor = ColorTranslator.FromHtml("#005A84")
                }
            });
        }

        #region IOMSTypeAddin Implementation

        public override void Initialise(FWBS.OMS.Interfaces.IOMSType obj)
        {
            _isAddin = true;
            ButtonCancelVisible = false;
            try { new SystemPermission(StandardPermissionType.UpdatePrecedent).Check(); }
            catch { ButtonEditVisible = false; }
        }

        public override bool Connect(FWBS.OMS.Interfaces.IOMSType obj)
        {
            if (obj is User)
            {
                SetDefaults(null, "");
                return true;
            }
            return false;
        }

        public override void RefreshItem()
        {
            SetDefaults(null, "");
            ToBeRefreshed = false;
        }

        private void ucNavCmdButtonSelectAssociate_Click(object sender, EventArgs e)
        {
            Associate assoc = Services.SelectAssociate(ParentForm);
            if (assoc != null)
            {
                SetDefaults(null, assoc, "", "");
            }
        }

        #endregion IOMSTypeAddin Implementation

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            picImage.Image = Images.GetAdminMenuList((Images.IconSize)LogicalToDeviceUnits(32)).Images[5];
            base.OnDpiChangedAfterParent(e);
        }

        private void InitSearchLists()
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                if (ucPrecList.SearchList == null)
                {
                    ucPrecList.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.Precedent), false, null, null);
                    ucPrecList.SearchList.ParameterHandler += new SourceEngine.SetParameterHandler(this.SetParam);
                    ucPrecList.SearchCompleted += new SearchCompletedEventHandler(ucPrecList_SearchCompleted);
                    ucPrecList.SearchListLoad += new EventHandler(ucPrecList_SearchListLoad);
                    ucPrecList.SearchTypeChanged += new EventHandler(ucPrecList_SearchTypeChanged);
                    ucPrecList.FilterChanged += new EventHandler(ucPrecList_FilterChanged);
                    ucPrecList.SearchButtonCommands += ucPrecList_SearchButtonCommands;
                    var button = ucPrecList.GetOMSToolBarButton("additionalinfo");
                    if (button != null) button.Visible = _additionalinfovisible;
                }

                if (ucPrecFav.SearchList == null)
                {
                    ucPrecFav.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PrecedentFavourites), false, null, null);
                    ucPrecFav.SearchList.ParameterHandler += new SourceEngine.SetParameterHandler(this.SetParam);
                    ucPrecFav.SearchCompleted += new SearchCompletedEventHandler(ucPrecFav_SearchCompleted);
                    ucPrecFav.SearchListLoad += new EventHandler(ucPrecList_SearchListLoad);
                    ucPrecFav.SearchTypeChanged += new EventHandler(ucPrecList_SearchTypeChanged);
                    ucPrecFav.FilterChanged += new EventHandler(ucPrecList_FilterChanged);
                    ucPrecFav.SearchButtonCommands += ucPrecFav_SearchButtonCommands;
                    var button = ucPrecFav.GetOMSToolBarButton("additionalinfo");
                    if (button != null) button.Visible = _additionalinfovisible;
                }
            }
        }

        private void ucPrecFav_SearchButtonCommands(object sender, SearchButtonEventArgs e)
        {
            if (ucPrecFav.CurrentItem() != null)
            {
                var currentPrecId = (long)ucPrecFav.ReturnValues["PRECID"].Value;

                switch (e.ButtonName)
                {
                    case "removeFromFav":
                        {
                            _precFavourites.RemoveFavourite(ucPrecFav.ReturnValues["FAVID"].Value.ToString());
                            ucPrecFav.Search(true, false, false);
                            break;
                        }
                    case "additionalInfo":
                        {
                            if (currentPrecId > 0)
                            {
                                btnAdditionalInfo_Click(sender, e, currentPrecId);
                            }
                            break;
                        }
                }
            }
        }

        private void ucPrecList_SearchButtonCommands(object sender, SearchButtonEventArgs e)
        {
            if (ucPrecList.CurrentItem() != null)
            {
                var precId = (long)ucPrecList.ReturnValues["PRECID"].Value;

                switch (e.ButtonName)
                {
                    case "addToFavourites":
                        {
                            if (!CheckForExistingFavourite(precId))
                            {
                                var result = _precFavourites.AddFavourite("PRECFAVTAB", string.Empty, new string[] { $"{precId}" });
                                ucPrecFav.Search(true, false, false);
                            }
                            break;
                        }
                    case "additionalInfo":
                        {
                            btnAdditionalInfo_Click(sender, e, precId);
                            break;
                        }
                }
            }
        }

        private bool CheckForExistingFavourite(long precID)
        {
            KeyValueCollection kvc = new KeyValueCollection() { { "precID", precID }, { "userID", Session.CurrentSession.CurrentUser.ID } };
            DataTable dt = RunDataList(constCheckFavs, kvc);
            if (dt != null & dt.Rows.Count > 0)
            {
                MessageBox.Show(ResourceLookup.GetLookupText("AKNOADDTOFAV"), ResourceLookup.GetLookupText("ADMIN"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            else
                return false;
        }

        protected System.Data.DataTable RunDataList(string dataList, FWBS.Common.KeyValueCollection kvc)
        {
            FWBS.OMS.EnquiryEngine.DataLists dl = new FWBS.OMS.EnquiryEngine.DataLists(dataList);
            dl.ChangeParameters(kvc);
            System.Data.DataTable dt = dl.Run(false) as System.Data.DataTable;
            return dt;
        }

        /// <summary>
        /// Replace the Contructor in previous version
        /// Passes the IOMSApp and Associate Object and Precednet Type Code
        /// </summary>
        /// <param name="app">IOMS App</param>
        /// <param name="assocobj">Associate Object</param>
        /// <param name="acttype">Precdent Type e.g. LETTER</param>
        /// <param name="library">Precdent Library</param>
        public void SetDefaults(FWBS.OMS.Interfaces.IOMSApp app, FWBS.OMS.Associate assocobj, string acttype, string library)
		{
            // Orginal Code that runs if registry not set.
            if (assocobj != null) // if not null then assign to private variable
                _assoc = assocobj;
            _library = library;
            _activetype = acttype;
            _app = app;

            InitSearchLists();
            SetDefaultComplete();
		}



		/// <summary>
		/// Replace the Contructor in previous version
		/// Passes the IOMSApp and Precednet Type Code
		/// </summary>
		/// <param name="app">IOMS App</param>
		/// <param name="acttype">Precdent Type e.g. LETTER</param>
		/// <param name="library">Precdent Library</param>
		public void SetDefaults(FWBS.OMS.Interfaces.IOMSApp app, string acttype, string library)
		{
            _assoc = null;
            SetDefaults(app, _assoc, acttype, library);
		}

		/// <summary>
		/// Replace the Contructor in previous version
		/// </summary>
		/// <param name="app">IOMS App</param>
		/// <param name="acttype">Precdent Type e.g. LETTER</param>
		public void SetDefaults(FWBS.OMS.Interfaces.IOMSApp app, string acttype)
		{
            SetDefaults(app, acttype, "");
		}

		private void SetDefaultComplete()
		{
			if (_assoc != null && _assoc.OMSFile.PrecedentLibrary != "" && _library == "")
				_library = _assoc.OMSFile.PrecedentLibrary;

			
			if (enqPrecHeader.Enquiry == null)
				enqPrecHeader.Enquiry = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiry(Session.CurrentSession.DefaultSystemForm(SystemForms.PrecedentSearch), null, FWBS.OMS.EnquiryEngine.EnquiryMode.Search, null);
			
			
	
				//IF LIBRARY IS NOT USED.
				int poschanged = 0;
				int h  = ((Control)cLibrary).Height;
				if ((_showlibrary == false && Session.CurrentSession.CurrentUser.ShowPrecLibraryOption == false) && cLibrary != null)
				{
					poschanged = -h;
					if (Convert.ToString(((Control)cLibrary).Tag) == "")
					{
						((Control)cLibrary).Tag = "HIDE";
						((Control)cLibrary).Visible = false;
						((Control)cType).Top = ((Control)cType).Top - h;
						((Control)cCat).Top = ((Control)cCat).Top - h;
						((Control)cSubCat).Top = ((Control)cSubCat).Top - h;
                        ((Control)cMinorCat).Top = ((Control)cMinorCat).Top - h;
                        ((Control)cContactType).Top = ((Control)cContactType).Top - h;
						((Control)cAssocType).Top = ((Control)cAssocType).Top - h;
						((Control)cLang).Top = ((Control)cLang).Top - h;
						((Control)cStage).Top = ((Control)cStage).Top - h;

					}
				}
				else
				{
					if (Convert.ToString(((Control)cLibrary).Tag) == "HIDE")
					{
						((Control)cLibrary).Tag = "";
						((Control)cLibrary).Visible = true;
						((Control)cType).Top = ((Control)cType).Top + h;
						((Control)cCat).Top = ((Control)cCat).Top + h;
						((Control)cSubCat).Top = ((Control)cSubCat).Top + h;
                        ((Control)cMinorCat).Top = ((Control)cMinorCat).Top + h;
                        ((Control)cContactType).Top = ((Control)cContactType).Top + h;
						((Control)cAssocType).Top = ((Control)cAssocType).Top + h;
						((Control)cLang).Top = ((Control)cLang).Top + h;
						((Control)cStage).Top = ((Control)cStage).Top + h;

					}
				}



				
				//IF LANGUAGE IS NOT USED.
				int poschanged2 = 0;
				h = ((Control)cLang).Height;
				if (Session.CurrentSession.CurrentUser.ShowPrecLanguageOption == false && cLang != null)
				{
					poschanged2 = -h;
					if (Convert.ToString(((Control)cLang).Tag) == "")
					{
						((Control)cLang).Visible = false;
						((Control)cLang).Tag = "HIDE";
						((Control)cStage).Top = ((Control)cStage).Top - h;
					}
				}
				else
				{
					if (Convert.ToString(((Control)cLang).Tag) == "HIDE")
					{
						((Control)cLang).Visible = true;
						((Control)cLang).Tag = "";
						((Control)cStage).Top = ((Control)cStage).Top + h;
					}
				
				}


			
				//IF STAGE IS NOT USED.
				int poschanged3 = 0;
				h = ((Control)cStage).Height;
			if (_assoc != null && _assoc.OMSFile.MilestonePlan != null)
			{
				if (Convert.ToString(((Control)cStage).Tag) == "HIDE")
				{
					((Control)cStage).TabStop = true;
					((Control)cStage).Tag = "";
					((Control)cStage).Visible = true;
				}
				((IListEnquiryControl)cStage).AddItem(_assoc.OMSFile.MilestonePlan.GetMileStoneStages(true));
			}
			else
			{
				poschanged3 = -h;
				if (Convert.ToString(((Control)cStage).Tag) == "")
				{
					((Control)cStage).Visible = false;
					((Control)cStage).Tag = "HIDE";
				}
			}

			pnlTop.Height = LogicalToDeviceUnits(enqPrecHeader.Enquiry.CanvasSize.Height) + poschanged + poschanged2 + poschanged3;
			



			//Once in the load event.
			//***********************
			if (Session.CurrentSession.IsLoggedIn)
			{
				if (_library != "" && cLibrary != null)
					cLibrary.Value = _library;

				if (_activetype != "")
				{
					cType.Value = _activetype;
				}

				if (_assoc != null)
				{
					cQuickSearch.Value = DBNull.Value;
					if (cLibrary != null)
						cLibrary.Value = _assoc.OMSFile.PrecedentLibrary;
					cCat.Value = _assoc.OMSFile.PrecedentCategory;
					cSubCat.Value = _assoc.OMSFile.PrecedentSubCategory;
                    cMinorCat.Value = _assoc.OMSFile.PrecedentMinorCategory;

                    if (_assoc.IsClient)
						cContactType.Value = "CLIENT";
					else
						cContactType.Value = _assoc.Contact.ContactTypeCode;

					cAssocType.Value = _assoc.AssocType;
					cLang.Value = _assoc.OMSFile.PreferedLanguage;

					if (_assoc.OMSFile.MilestonePlan != null)
						cStage.Value = _assoc.OMSFile.MilestonePlan.NextStage;
				}
				else if (_assoc == null)
				{
					object cat = DBNull.Value;
					object subcat = DBNull.Value;
                    object minorcat = DBNull.Value;

                    if (Session.CurrentSession.CurrentFeeEarner.DefaultPrecedentCategory != "")
					{
						cat = Session.CurrentSession.CurrentFeeEarner.DefaultPrecedentCategory;
						if (Session.CurrentSession.CurrentFeeEarner.DefaultPrecedentSubCategory != "")
						{
							subcat = Session.CurrentSession.CurrentFeeEarner.DefaultPrecedentSubCategory;
                            if (Session.CurrentSession.CurrentFeeEarner.DefaultPrecedentMinorCategory != "")
                            {
                                minorcat = Session.CurrentSession.CurrentFeeEarner.DefaultPrecedentMinorCategory;
                            }
                        }
					}
					else
					{
						if (Session.CurrentSession.CurrentUser.DefaultPrecedentCategory != "")
						{
							cat = Session.CurrentSession.CurrentUser.DefaultPrecedentCategory;
							if (Session.CurrentSession.CurrentUser.DefaultPrecedentSubCategory != "")
							{
								subcat = Session.CurrentSession.CurrentUser.DefaultPrecedentSubCategory;
                                if (Session.CurrentSession.CurrentUser.DefaultPrecedentMinorCategory != "")
                                {
                                    subcat = Session.CurrentSession.CurrentUser.DefaultPrecedentMinorCategory;
                                }
                            }
						}
					}

					cCat.Value = cat;
					cSubCat.Value = subcat;
                    cMinorCat.Value = minorcat;
                    cQuickSearch.Value = DBNull.Value;
					cContactType.Value = DBNull.Value;
					cAssocType.Value = DBNull.Value;
					cLang.Value = Session.CurrentSession.DefaultCulture;
					cStage.Value = DBNull.Value;
				}
			

				force = true;

				UpdateJobList(null, EventArgs.Empty);
				tcontrol.SelectedTab = tpList;
				if (_isAddin) return;
				try
				{
					ParentForm.ActiveControl = enqPrecHeader.GetControl("_quickSearch") as Control;
				}
				catch
				{}
				try
				{
					if (btnContinue.Visible)
                        ParentForm.AcceptButton = btnContinue;
				}
				catch
				{}
			}
		}

		public new void Refresh()
		{
            enqPrecHeader.Enquiry.RefreshDataList("_preclib");
			ucPrecList.Search();			
		}


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
                PrecedentJobList precedentJobList = Session.CurrentSession.CurrentPrecedentJobList;
                precedentJobList.Added -= new EventHandler(UpdateJobList);
                precedentJobList.Moved -= new EventHandler(UpdateJobList);
                precedentJobList.Removed -= new EventHandler(UpdateJobList);
                precedentJobList.Cleared -= new EventHandler(UpdateJobList);
                omsDockManager1.Dispose();
				_default.Dispose();
				_default = null;
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucPrecedent));
            this.dockAreaPane1 = new Infragistics.Win.UltraWinDock.DockAreaPane(Infragistics.Win.UltraWinDock.DockedLocation.DockedRight, new System.Guid("41805f65-7924-466f-b595-233b7a5cdaa6"));
            this.dockableControlPane1 = new Infragistics.Win.UltraWinDock.DockableControlPane(new System.Guid("07f2190b-e98d-4e08-ba64-6c9380cf7427"), new System.Guid("00000000-0000-0000-0000-000000000000"), -1, new System.Guid("41805f65-7924-466f-b595-233b7a5cdaa6"), -1);
            this.documentPreviewer1 = new FWBS.OMS.UI.Windows.DocumentManagement.Addins.DocumentPreviewAddin();
            this.ucNavCmdButtonSelectAssociate = new FWBS.OMS.UI.Windows.ucNavCmdButtons();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.lblSearching = new System.Windows.Forms.Label();
            this.labWaiting = new System.Windows.Forms.Label();
            this.btnView = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pnlTop = new System.Windows.Forms.Panel();
            this.enqPrecHeader = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.pnlImage = new System.Windows.Forms.Panel();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.pnlMainLst = new System.Windows.Forms.Panel();
            this.tcontrol = new FWBS.OMS.UI.TabControl();
            this.tpList = new System.Windows.Forms.TabPage();
            this.pnlList = new System.Windows.Forms.Panel();
            this.ucPrecList = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.pnlBuilder = new System.Windows.Forms.Panel();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tpFav = new System.Windows.Forms.TabPage();
            this.pnlFav = new System.Windows.Forms.Panel();
            this.ucPrecFav = new FWBS.OMS.UI.Windows.ucSearchControl();
            this.tpJoblist = new System.Windows.Forms.TabPage();
            this.ucJoblist1 = new FWBS.OMS.UI.Windows.ucJoblistRepeaterContainer();
            this.timFilterDown = new System.Windows.Forms.Timer(this.components);
            this.timRunOnce = new System.Windows.Forms.Timer(this.components);
            this.pnlSp1 = new System.Windows.Forms.Panel();
            this.icons = new System.Windows.Forms.ImageList(this.components);
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this._ucPrecedentAutoHideControl = new Infragistics.Win.UltraWinDock.AutoHideControl();
            this.dockableWindow1 = new Infragistics.Win.UltraWinDock.DockableWindow();
            this.omsDockManager1 = new FWBS.OMS.UI.omsDockManager(this.components);
            this.windowDockingArea1 = new Infragistics.Win.UltraWinDock.WindowDockingArea();
            this._ucPrecedentUnpinnedTabAreaBottom = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucPrecedentUnpinnedTabAreaTop = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucPrecedentUnpinnedTabAreaRight = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this._ucPrecedentUnpinnedTabAreaLeft = new Infragistics.Win.UltraWinDock.UnpinnedTabArea();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.navCommands.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlImage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.pnlMainLst.SuspendLayout();
            this.tcontrol.SuspendLayout();
            this.tpList.SuspendLayout();
            this.pnlList.SuspendLayout();
            this.pnlBuilder.SuspendLayout();
            this.tpFav.SuspendLayout();
            this.pnlFav.SuspendLayout();
            this.tpJoblist.SuspendLayout();
            this._ucPrecedentAutoHideControl.SuspendLayout();
            this.dockableWindow1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.omsDockManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.Location = new System.Drawing.Point(3, 3);
            this.pnlDesign.Size = new System.Drawing.Size(168, 424);
            // 
            // pnlActions
            // 
            this.pnlActions.ExpandedHeight = 46;
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Size = new System.Drawing.Size(152, 46);
            this.pnlActions.Visible = true;
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // navCommands
            // 
            this.navCommands.Controls.Add(this.ucNavCmdButtonSelectAssociate);
            this.navCommands.Size = new System.Drawing.Size(152, 39);
            // 
            // documentPreviewer1
            // 
            this.documentPreviewer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.documentPreviewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.documentPreviewer1.Location = new System.Drawing.Point(0, 18);
            this.documentPreviewer1.Name = "documentPreviewer1";
            this.documentPreviewer1.Padding = new System.Windows.Forms.Padding(5);
            this.documentPreviewer1.Size = new System.Drawing.Size(328, 406);
            this.documentPreviewer1.TabIndex = 0;
            this.documentPreviewer1.ToBeRefreshed = false;
            // 
            // ucNavCmdButtonSelectAssociate
            // 
            this.ucNavCmdButtonSelectAssociate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ucNavCmdButtonSelectAssociate.ImageIndex = -1;
            this.ucNavCmdButtonSelectAssociate.Location = new System.Drawing.Point(5, 12);
            this.resourceLookup1.SetLookup(this.ucNavCmdButtonSelectAssociate, new FWBS.OMS.UI.Windows.ResourceLookupItem("SELASSOCIATE", "Select %ASSOCIATE%", ""));
            this.ucNavCmdButtonSelectAssociate.ModernStyle = true;
            this.ucNavCmdButtonSelectAssociate.Name = "ucNavCmdButtonSelectAssociate";
            this.ucNavCmdButtonSelectAssociate.Size = new System.Drawing.Size(142, 22);
            this.ucNavCmdButtonSelectAssociate.TabIndex = 0;
            this.ucNavCmdButtonSelectAssociate.Text = "Select %ASSOCIATE%";
            this.ucNavCmdButtonSelectAssociate.Click += new System.EventHandler(this.ucNavCmdButtonSelectAssociate_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnContinue.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnContinue.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnContinue.Location = new System.Drawing.Point(8, 1);
            this.resourceLookup1.SetLookup(this.btnContinue, new FWBS.OMS.UI.Windows.ResourceLookupItem("CONTINUE", "&Continue", ""));
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(95, 23);
            this.btnContinue.TabIndex = 6;
            this.btnContinue.Text = "Continue";
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCancel.Location = new System.Drawing.Point(8, 97);
            this.resourceLookup1.SetLookup(this.btnCancel, new FWBS.OMS.UI.Windows.ResourceLookupItem("BTNCANCEL", "Cance&l", ""));
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(95, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cance&l";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnEdit.Location = new System.Drawing.Point(8, 73);
            this.resourceLookup1.SetLookup(this.btnEdit, new FWBS.OMS.UI.Windows.ResourceLookupItem("PRECEDIT", "&Edit %PRECEDENT%", ""));
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(95, 23);
            this.btnEdit.TabIndex = 9;
            this.btnEdit.Text = "Edit Precedent";
            this.btnEdit.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // lblSearching
            // 
            this.lblSearching.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSearching.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblSearching.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblSearching.Location = new System.Drawing.Point(394, 3);
            this.resourceLookup1.SetLookup(this.lblSearching, new FWBS.OMS.UI.Windows.ResourceLookupItem("PRECSEARCHING", "Searching...", ""));
            this.lblSearching.Name = "lblSearching";
            this.lblSearching.Size = new System.Drawing.Size(96, 17);
            this.lblSearching.TabIndex = 10;
            this.lblSearching.Text = "Searching...";
            this.lblSearching.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSearching.Visible = false;
            // 
            // labWaiting
            // 
            this.labWaiting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labWaiting.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.labWaiting.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.labWaiting.Location = new System.Drawing.Point(8, 125);
            this.resourceLookup1.SetLookup(this.labWaiting, new FWBS.OMS.UI.Windows.ResourceLookupItem("PRECWAITING", "Waiting ...", ""));
            this.labWaiting.Name = "labWaiting";
            this.labWaiting.Size = new System.Drawing.Size(96, 17);
            this.labWaiting.TabIndex = 11;
            this.labWaiting.Text = "Waiting ...";
            this.labWaiting.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labWaiting.Visible = false;
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnView.Location = new System.Drawing.Point(8, 25);
            this.resourceLookup1.SetLookup(this.btnView, new FWBS.OMS.UI.Windows.ResourceLookupItem("VIEW", "&View", ""));
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(95, 23);
            this.btnView.TabIndex = 7;
            this.btnView.Text = "View";
            this.btnView.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPrint.Location = new System.Drawing.Point(8, 49);
            this.resourceLookup1.SetLookup(this.btnPrint, new FWBS.OMS.UI.Windows.ResourceLookupItem("PRINT", "&Print", ""));
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(95, 23);
            this.btnPrint.TabIndex = 8;
            this.btnPrint.Text = "Print";
            this.btnPrint.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.enqPrecHeader);
            this.pnlTop.Controls.Add(this.pnlImage);
            this.pnlTop.Controls.Add(this.pnlButtons);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlTop.Location = new System.Drawing.Point(171, 3);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(491, 149);
            this.pnlTop.TabIndex = 6;
            // 
            // enqPrecHeader
            // 
            this.enqPrecHeader.AutoScroll = true;
            this.enqPrecHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enqPrecHeader.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.enqPrecHeader.IsDirty = false;
            this.enqPrecHeader.Location = new System.Drawing.Point(40, 0);
            this.enqPrecHeader.Name = "enqPrecHeader";
            this.enqPrecHeader.Size = new System.Drawing.Size(347, 149);
            this.enqPrecHeader.TabIndex = 1;
            this.enqPrecHeader.ToBeRefreshed = false;
            this.enqPrecHeader.RefreshedControls += new System.EventHandler(this.enqPrecHeader_Refreshed);
            this.enqPrecHeader.Enter += new System.EventHandler(this.enqPrecHeader_Enter);
            // 
            // pnlImage
            // 
            this.pnlImage.Controls.Add(this.picImage);
            this.pnlImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlImage.Location = new System.Drawing.Point(0, 0);
            this.pnlImage.Name = "pnlImage";
            this.pnlImage.Padding = new System.Windows.Forms.Padding(4);
            this.pnlImage.Size = new System.Drawing.Size(40, 149);
            this.pnlImage.TabIndex = 14;
            // 
            // picImage
            // 
            this.picImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.picImage.Location = new System.Drawing.Point(4, 4);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(32, 32);
            this.picImage.TabIndex = 12;
            this.picImage.TabStop = false;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnEdit);
            this.pnlButtons.Controls.Add(this.btnPrint);
            this.pnlButtons.Controls.Add(this.btnView);
            this.pnlButtons.Controls.Add(this.btnContinue);
            this.pnlButtons.Controls.Add(this.labWaiting);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlButtons.Location = new System.Drawing.Point(387, 0);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(104, 149);
            this.pnlButtons.TabIndex = 13;
            // 
            // pnlMainLst
            // 
            this.pnlMainLst.Controls.Add(this.lblSearching);
            this.pnlMainLst.Controls.Add(this.tcontrol);
            this.pnlMainLst.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMainLst.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.pnlMainLst.Location = new System.Drawing.Point(171, 160);
            this.pnlMainLst.Name = "pnlMainLst";
            this.pnlMainLst.Size = new System.Drawing.Size(491, 267);
            this.pnlMainLst.TabIndex = 8;
            // 
            // tcontrol
            // 
            this.tcontrol.Controls.Add(this.tpList);
            this.tcontrol.Controls.Add(this.tpFav);
            this.tcontrol.Controls.Add(this.tpJoblist);
            this.tcontrol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcontrol.HotTrack = true;
            this.tcontrol.Location = new System.Drawing.Point(0, 0);
            this.tcontrol.Name = "tcontrol";
            this.tcontrol.SelectedIndex = 0;
            this.tcontrol.Size = new System.Drawing.Size(491, 267);
            this.tcontrol.TabIndex = 2;
            this.tcontrol.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tcontrol_Selecting);
            // 
            // tpList
            // 
            this.tpList.Controls.Add(this.pnlList);
            this.tpList.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpList, new FWBS.OMS.UI.Windows.ResourceLookupItem("PRECLIST", "List", ""));
            this.tpList.Name = "tpList";
            this.tpList.Size = new System.Drawing.Size(483, 239);
            this.tpList.TabIndex = 0;
            this.tpList.Text = "List";
            this.tpList.UseVisualStyleBackColor = true;
            // 
            // pnlList
            // 
            this.pnlList.Controls.Add(this.ucPrecList);
            this.pnlList.Controls.Add(this.pnlBuilder);
            this.pnlList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlList.Location = new System.Drawing.Point(0, 0);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(483, 239);
            this.pnlList.TabIndex = 1;
            // 
            // ucPrecList
            // 
            this.ucPrecList.AutoJumpToQuickSearch = false;
            this.ucPrecList.BackColor = System.Drawing.Color.White;
            this.ucPrecList.BackGroundColor = System.Drawing.Color.White;
            this.ucPrecList.cmdActive = this.btnCancel;
            this.ucPrecList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPrecList.DoubleClickAction = "None";
            this.ucPrecList.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucPrecList.GraphicalPanelVisible = true;
            this.ucPrecList.Location = new System.Drawing.Point(0, 0);
            this.ucPrecList.Margin = new System.Windows.Forms.Padding(0);
            this.ucPrecList.Name = "ucPrecList";
            this.ucPrecList.NavCommandPanel = null;
            this.ucPrecList.Padding = new System.Windows.Forms.Padding(5);
            this.ucPrecList.SearchListCode = "";
            this.ucPrecList.SearchListType = "";
            this.ucPrecList.SearchPanelVisible = false;
            this.ucPrecList.Size = new System.Drawing.Size(403, 239);
            this.ucPrecList.TabIndex = 4;
            this.ucPrecList.ToBeRefreshed = false;
            this.ucPrecList.TypeSelectorVisible = false;
            this.ucPrecList.ItemHover += new FWBS.OMS.UI.Windows.SearchItemHoverEventHandler(this.ucPrecList_ItemHover);
            this.ucPrecList.SelectedItemDoubleClick += new System.EventHandler(this.ucPrecList_SelectedItemDoubleClick);
            // 
            // pnlBuilder
            // 
            this.pnlBuilder.Controls.Add(this.btnUnlock);
            this.pnlBuilder.Controls.Add(this.btnAdd);
            this.pnlBuilder.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlBuilder.Location = new System.Drawing.Point(403, 0);
            this.pnlBuilder.Name = "pnlBuilder";
            this.pnlBuilder.Padding = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.pnlBuilder.Size = new System.Drawing.Size(80, 239);
            this.pnlBuilder.TabIndex = 1;
            // 
            // btnUnlock
            // 
            this.btnUnlock.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUnlock.Enabled = false;
            this.btnUnlock.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnUnlock.Location = new System.Drawing.Point(4, 31);
            this.resourceLookup1.SetLookup(this.btnUnlock, new FWBS.OMS.UI.Windows.ResourceLookupItem("PRECUNLOCK", "Unlock", ""));
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(72, 23);
            this.btnUnlock.TabIndex = 1;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnAdd.Location = new System.Drawing.Point(4, 8);
            this.resourceLookup1.SetLookup(this.btnAdd, new FWBS.OMS.UI.Windows.ResourceLookupItem("PRECADD++", "Add (&+)", ""));
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(72, 23);
            this.btnAdd.TabIndex = 0;
            this.btnAdd.Text = "Add (&+)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tpFav
            // 
            this.tpFav.Controls.Add(this.pnlFav);
            this.tpFav.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpFav, new FWBS.OMS.UI.Windows.ResourceLookupItem("FAVS", "Favourites", ""));
            this.tpFav.Name = "tpFav";
            this.tpFav.Size = new System.Drawing.Size(483, 239);
            this.tpFav.TabIndex = 3;
            this.tpFav.Text = "Favourites";
            this.tpFav.UseVisualStyleBackColor = true;
            // 
            // pnlFav
            // 
            this.pnlFav.Controls.Add(this.ucPrecFav);
            this.pnlFav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlFav.Location = new System.Drawing.Point(0, 0);
            this.pnlFav.Name = "pnlFav";
            this.pnlFav.Size = new System.Drawing.Size(483, 239);
            this.pnlFav.TabIndex = 0;
            // 
            // ucPrecFav
            // 
            this.ucPrecFav.AutoJumpToQuickSearch = false;
            this.ucPrecFav.BackColor = System.Drawing.Color.White;
            this.ucPrecFav.BackGroundColor = System.Drawing.Color.White;
            this.ucPrecFav.cmdActive = this.btnCancel;
            this.ucPrecFav.DisplayResultsCaption = false;
            this.ucPrecFav.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucPrecFav.DoubleClickAction = "None";
            this.ucPrecFav.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ucPrecFav.GraphicalPanelVisible = true;
            this.ucPrecFav.Location = new System.Drawing.Point(0, 0);
            this.ucPrecFav.Margin = new System.Windows.Forms.Padding(0);
            this.ucPrecFav.Name = "ucPrecFav";
            this.ucPrecFav.NavCommandPanel = null;
            this.ucPrecFav.Padding = new System.Windows.Forms.Padding(5);
            this.ucPrecFav.SearchListCode = "";
            this.ucPrecFav.SearchListType = "";
            this.ucPrecFav.SearchPanelVisible = false;
            this.ucPrecFav.Size = new System.Drawing.Size(483, 239);
            this.ucPrecFav.TabIndex = 4;
            this.ucPrecFav.ToBeRefreshed = false;
            this.ucPrecFav.TypeSelectorVisible = false;
            this.ucPrecFav.ItemHover += new FWBS.OMS.UI.Windows.SearchItemHoverEventHandler(this.ucPrecList_ItemHover);
            this.ucPrecFav.SelectedItemDoubleClick += new System.EventHandler(this.ucPrecList_SelectedItemDoubleClick);
            // 
            // tpJoblist
            // 
            this.tpJoblist.Controls.Add(this.ucJoblist1);
            this.tpJoblist.Location = new System.Drawing.Point(4, 24);
            this.resourceLookup1.SetLookup(this.tpJoblist, new FWBS.OMS.UI.Windows.ResourceLookupItem("JOBLIST", "Job List", ""));
            this.tpJoblist.Name = "tpJoblist";
            this.tpJoblist.Padding = new System.Windows.Forms.Padding(2);
            this.tpJoblist.Size = new System.Drawing.Size(483, 239);
            this.tpJoblist.TabIndex = 2;
            this.tpJoblist.Text = "Job List";
            this.tpJoblist.UseVisualStyleBackColor = true;
            // 
            // ucJoblist1
            // 
            this.ucJoblist1.AutoScroll = true;
            this.ucJoblist1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucJoblist1.Location = new System.Drawing.Point(2, 2);
            this.ucJoblist1.Name = "ucJoblist1";
            this.ucJoblist1.Padding = new System.Windows.Forms.Padding(3);
            this.ucJoblist1.SelectorRepeaterType = typeof(FWBS.OMS.UI.Windows.ucPrecJobSelector);
            this.ucJoblist1.ShowToolBar = true;
            this.ucJoblist1.Size = new System.Drawing.Size(479, 235);
            this.ucJoblist1.TabIndex = 4;
            // 
            // timFilterDown
            // 
            this.timFilterDown.Interval = 500;
            this.timFilterDown.Tick += new System.EventHandler(this.timFilterDown_Tick);
            // 
            // timRunOnce
            // 
            this.timRunOnce.Tick += new System.EventHandler(this.timRunOnce_Tick);
            // 
            // pnlSp1
            // 
            this.pnlSp1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSp1.Location = new System.Drawing.Point(171, 152);
            this.pnlSp1.Name = "pnlSp1";
            this.pnlSp1.Size = new System.Drawing.Size(491, 8);
            this.pnlSp1.TabIndex = 9;
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
            // 
            // _ucPrecedentAutoHideControl
            // 
            this._ucPrecedentAutoHideControl.Controls.Add(this.dockableWindow1);
            this._ucPrecedentAutoHideControl.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._ucPrecedentAutoHideControl.Location = new System.Drawing.Point(629, 3);
            this._ucPrecedentAutoHideControl.Name = "_ucPrecedentAutoHideControl";
            this._ucPrecedentAutoHideControl.Owner = this.omsDockManager1;
            this._ucPrecedentAutoHideControl.Size = new System.Drawing.Size(33, 424);
            this._ucPrecedentAutoHideControl.TabIndex = 14;
            // 
            // dockableWindow1
            // 
            this.dockableWindow1.Controls.Add(this.documentPreviewer1);
            this.dockableWindow1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dockableWindow1.Location = new System.Drawing.Point(5, 0);
            this.dockableWindow1.Name = "dockableWindow1";
            this.dockableWindow1.Owner = this.omsDockManager1;
            this.dockableWindow1.Size = new System.Drawing.Size(328, 424);
            this.dockableWindow1.TabIndex = 16;
            // 
            // omsDockManager1
            // 
            this.omsDockManager1.AnimationSpeed = Infragistics.Win.UltraWinDock.AnimationSpeed.StandardSpeedPlus3;
            this.omsDockManager1.DefaultPaneSettings.AllowClose = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.DefaultPaneSettings.AllowDockAsTab = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.DefaultPaneSettings.AllowDragging = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.DefaultPaneSettings.AllowFloating = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.DefaultPaneSettings.AllowMaximize = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.DefaultPaneSettings.AllowMinimize = Infragistics.Win.DefaultableBoolean.False;
            dockAreaPane1.ChildPaneStyle = Infragistics.Win.UltraWinDock.ChildPaneStyle.VerticalSplit;
            dockableControlPane1.Control = this.documentPreviewer1;
            dockableControlPane1.FlyoutSize = new System.Drawing.Size(328, -1);
            dockableControlPane1.Key = "PRECPREVIEW";
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
            dockableControlPane1.Text = "Precedent Preview";
            dockAreaPane1.Panes.AddRange(new Infragistics.Win.UltraWinDock.DockablePaneBase[] {
            dockableControlPane1});
            dockAreaPane1.Size = new System.Drawing.Size(328, 488);
            this.omsDockManager1.DockAreas.AddRange(new Infragistics.Win.UltraWinDock.DockAreaPane[] {
            dockAreaPane1});
            this.omsDockManager1.HostControl = this;
            this.omsDockManager1.SettingsKey = "";
            this.omsDockManager1.ShowCloseButton = false;
            this.omsDockManager1.ShowDisabledButtons = false;
            this.omsDockManager1.UseOsThemes = Infragistics.Win.DefaultableBoolean.False;
            this.omsDockManager1.PaneDisplayed += new Infragistics.Win.UltraWinDock.PaneDisplayedEventHandler(this.omsDockManager1_PaneDisplayed);
            // 
            // windowDockingArea1
            // 
            this.windowDockingArea1.Location = new System.Drawing.Point(0, 0);
            this.windowDockingArea1.Name = "windowDockingArea1";
            this.windowDockingArea1.Owner = this.omsDockManager1;
            this.windowDockingArea1.Size = new System.Drawing.Size(333, 488);
            this.windowDockingArea1.TabIndex = 15;
            this.windowDockingArea1.Visible = false;
            // 
            // _ucPrecedentUnpinnedTabAreaBottom
            // 
            this._ucPrecedentUnpinnedTabAreaBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._ucPrecedentUnpinnedTabAreaBottom.Location = new System.Drawing.Point(3, 427);
            this._ucPrecedentUnpinnedTabAreaBottom.Name = "_ucPrecedentUnpinnedTabAreaBottom";
            this._ucPrecedentUnpinnedTabAreaBottom.Owner = this.omsDockManager1;
            this._ucPrecedentUnpinnedTabAreaBottom.Size = new System.Drawing.Size(659, 0);
            this._ucPrecedentUnpinnedTabAreaBottom.TabIndex = 13;
            // 
            // _ucPrecedentUnpinnedTabAreaTop
            // 
            this._ucPrecedentUnpinnedTabAreaTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._ucPrecedentUnpinnedTabAreaTop.Location = new System.Drawing.Point(3, 3);
            this._ucPrecedentUnpinnedTabAreaTop.Name = "_ucPrecedentUnpinnedTabAreaTop";
            this._ucPrecedentUnpinnedTabAreaTop.Owner = this.omsDockManager1;
            this._ucPrecedentUnpinnedTabAreaTop.Size = new System.Drawing.Size(659, 0);
            this._ucPrecedentUnpinnedTabAreaTop.TabIndex = 12;
            // 
            // _ucPrecedentUnpinnedTabAreaRight
            // 
            this._ucPrecedentUnpinnedTabAreaRight.Dock = System.Windows.Forms.DockStyle.Right;
            this._ucPrecedentUnpinnedTabAreaRight.Location = new System.Drawing.Point(662, 3);
            this._ucPrecedentUnpinnedTabAreaRight.Name = "_ucPrecedentUnpinnedTabAreaRight";
            this._ucPrecedentUnpinnedTabAreaRight.Owner = this.omsDockManager1;
            this._ucPrecedentUnpinnedTabAreaRight.Size = new System.Drawing.Size(21, 424);
            this._ucPrecedentUnpinnedTabAreaRight.TabIndex = 11;
            // 
            // _ucPrecedentUnpinnedTabAreaLeft
            // 
            this._ucPrecedentUnpinnedTabAreaLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this._ucPrecedentUnpinnedTabAreaLeft.Location = new System.Drawing.Point(3, 3);
            this._ucPrecedentUnpinnedTabAreaLeft.Name = "_ucPrecedentUnpinnedTabAreaLeft";
            this._ucPrecedentUnpinnedTabAreaLeft.Owner = this.omsDockManager1;
            this._ucPrecedentUnpinnedTabAreaLeft.Size = new System.Drawing.Size(0, 424);
            this._ucPrecedentUnpinnedTabAreaLeft.TabIndex = 10;
            // 
            // ucPrecedent
            // 
            this.Controls.Add(this._ucPrecedentAutoHideControl);
            this.Controls.Add(this.pnlMainLst);
            this.Controls.Add(this.pnlSp1);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this._ucPrecedentUnpinnedTabAreaBottom);
            this.Controls.Add(this._ucPrecedentUnpinnedTabAreaTop);
            this.Controls.Add(this._ucPrecedentUnpinnedTabAreaRight);
            this.Controls.Add(this._ucPrecedentUnpinnedTabAreaLeft);
            this.Controls.Add(this.windowDockingArea1);
            this.resourceLookup1.SetLookup(this, new FWBS.OMS.UI.Windows.ResourceLookupItem("PRECFORM", "%PRECEDNET% Library", ""));
            this.Name = "ucPrecedent";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Size = new System.Drawing.Size(686, 430);
            this.Load += new System.EventHandler(this.ucPrecedent_Load);
            this.VisibleChanged += new System.EventHandler(this.ucPrecedent_VisibleChanged);
            this.ParentChanged += new System.EventHandler(this.ucPrecedent_ParentChanged);
            this.Controls.SetChildIndex(this.windowDockingArea1, 0);
            this.Controls.SetChildIndex(this._ucPrecedentUnpinnedTabAreaLeft, 0);
            this.Controls.SetChildIndex(this._ucPrecedentUnpinnedTabAreaRight, 0);
            this.Controls.SetChildIndex(this._ucPrecedentUnpinnedTabAreaTop, 0);
            this.Controls.SetChildIndex(this._ucPrecedentUnpinnedTabAreaBottom, 0);
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.pnlTop, 0);
            this.Controls.SetChildIndex(this.pnlSp1, 0);
            this.Controls.SetChildIndex(this.pnlMainLst, 0);
            this.Controls.SetChildIndex(this._ucPrecedentAutoHideControl, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.pnlActions.PerformLayout();
            this.navCommands.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlImage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.pnlMainLst.ResumeLayout(false);
            this.tcontrol.ResumeLayout(false);
            this.tpList.ResumeLayout(false);
            this.pnlList.ResumeLayout(false);
            this.pnlBuilder.ResumeLayout(false);
            this.tpFav.ResumeLayout(false);
            this.pnlFav.ResumeLayout(false);
            this.tpJoblist.ResumeLayout(false);
            this._ucPrecedentAutoHideControl.ResumeLayout(false);
            this.dockableWindow1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.omsDockManager1)).EndInit();
            this.ResumeLayout(false);

		}
        #endregion

        #endregion

        #region Event Handlers


        private void ucPrecedent_Load(object sender, System.EventArgs e)
        {
            if (Global.IsInDesignMode())
                return;

            omsDockManager1.PaneFromControl(documentPreviewer1).Text = Session.CurrentSession.Resources.GetResource("PRECPREVIEW", "%PRECEDENT% Preview", "").Text;
        }

        private void ucPrecList_SearchListLoad(object sender, System.EventArgs e)
        {
            omsDockManager1.Visible = false;
        }

        private void ucPrecList_SearchTypeChanged(object sender, EventArgs e)
        {
            documentPreviewer1.Connect(null);
            documentPreviewer1.RefreshItem();
        }

        private void ucPrecList_SearchCompleted(object sender, SearchCompletedEventArgs e)
        {
            BuildIconColumn(ucPrecList);

            SetPrecedentToBePreviewed();

            SetDataForButtonEnablement();
        }

        private void ucPrecFav_SearchCompleted(object sender, SearchCompletedEventArgs e)
        {
            BuildIconColumn(ucPrecFav);

            SetPrecedentToBePreviewed();

            SetDataForButtonEnablement();
        }

        private void ucPrecList_FilterChanged(object sender, EventArgs e)
        {
            SetPrecedentToBePreviewed();

            SetDataForButtonEnablement();
        }

        private void SetDataForButtonEnablement()
        {
            _visibleRowCount = ucPrecList.dgSearchResults.VisibleRowCount;
            _selectedLibrary = Convert.ToString(cLibrary.Value);
            OnSearchCompleted();
        }

        /// <summary>
        /// Used by the Search List to get the Parameters from the Precdent Enquiry Form
        /// </summary>
        /// <param name="name">Name of Control on the Enquiry Form</param>
        /// <param name="value">The Return Value</param>
        private void SetParam(string name, out object value)
        {
            try
            {
                value = GetEnquiryControl(enqPrecHeader.GetControl(name)).Value;
            }
            catch
            {
                if (name.Equals("_usrid"))
                {
                    value = Session.CurrentSession.CurrentUser.ID.ToString();
                }
                else if (name.Equals("FavOnly"))
                {
                    value = true;
                }
                else
                {
                    value = "";
                }
            }
        }


		/// <summary>
		/// Captures the precedent edit button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnAction_Click(object sender, System.EventArgs e)
		{
            try
            {
                Common.KeyValueCollection itm = this.SelectedItem;

                if (itm == null || itm["precid"].Value == null)
                {
                    ResourceItem res = Session.CurrentSession.Resources.GetMessage("MSGMUSTSELPREC", "You must select a %PRECEDENT%.", "");
                    MessageBox.Show(res, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (!_isAddin) ParentForm.DialogResult = DialogResult.None;
                    return;
                }

                Precedent precobj = FWBS.OMS.Precedent.GetPrecedent(Convert.ToInt64(itm["precid"].Value));                
                UnloadPreview();
                RunButtonAction(sender, precobj);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

          
		/// <summary>
		/// Fired as you move up and down the Search List
		/// </summary>
		/// <param name="sender">The Sender Object</param>
		/// <param name="e">Empty EventArgs</param>
		private void ucPrecList_ItemHover(object sender, FWBS.OMS.UI.Windows.SearchItemHoverEventArgs e)
		{
			try
			{
				if (e.ItemList != null)
				{
                    SetPrecedentToBePreviewed();
				    EnableUnlockBtn(e.ItemList);
				}
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ParentForm, ex);
			}
		}

	    private void EnableUnlockBtn(KeyValueCollection itemList)
	    {
	        if (pnlBuilder.Visible && btnUnlock.Visible && itemList["precCheckedOut"] != null)
	        {
                btnUnlock.Enabled = itemList["precCheckedOut"].Value != DBNull.Value;
	        }
	    }

	    /// <summary>
		/// Add the Precednet to the Precedent Job List
		/// </summary>
		/// <param name="sender">The Sender Object</param>
		/// <param name="e">Empty EventArgs</param>
		private void btnAdd_Click(object sender, System.EventArgs e)
		{
            try
            {
                Common.KeyValueCollection itm = this.SelectedItem;
                if (itm == null || itm["precid"].Value == null)
                {
                    ResourceItem res = Session.CurrentSession.Resources.GetMessage("MSGMUSTSELPREC", "You must select a %PRECEDENT%.", "");
                    MessageBox.Show(res, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (!_isAddin) ParentForm.DialogResult = DialogResult.None;
                    return;
                }
                Precedent precobj = FWBS.OMS.Precedent.GetPrecedent(Convert.ToInt64(itm["precid"].Value));

                if (_assoc == null)
                {
                    OMSFile of = FWBS.OMS.UI.Windows.Services.SelectFile();
                    if (of == null) return;
                    _assoc = of.DefaultAssociate;
                    if (_assoc == null) { return; }
                }

                if (precobj.IsMultiPrecedent)
                {
                    precobj.GenerateJobList(_assoc.OMSFile);
                }
                else
                {                  
                    PrecedentJob pj = new PrecedentJob(precobj);
                    pj.AsNewTemplate = true;
                    pj.Associate = pj.GetBestFitAssociate(_assoc);
                    if (pj.Associate == null)
                        return;
                    FWBS.OMS.Session.CurrentSession.CurrentPrecedentJobList.Add(pj);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

	    private void btnUnlock_Click(object sender, EventArgs e)
	    {
	        try
	        {
	            Common.KeyValueCollection itm = this.SelectedItem;
	            if (itm == null || itm["precid"].Value == null)
	            {
	                ResourceItem res = Session.CurrentSession.Resources.GetMessage("MSGMUSTSELPREC", "You must select a %PRECEDENT%.", "");
	                MessageBox.Show(res, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                    if (!_isAddin) ParentForm.DialogResult = DialogResult.None;
	                return;
	            }
	            Precedent precobj = FWBS.OMS.Precedent.GetPrecedent(Convert.ToInt64(itm["precid"].Value));
	            if (!precobj.IsCheckedOutByCurrentUser)
	            {
	                string message = "The precedent is checked out by another user ({0}).\r\nAre you sure you want to undo?";

                    DialogResult dialogResult = MessageBox.Show(string.Format(message, precobj.CheckedOutBy), "Undo Checkout", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
	                if (dialogResult != DialogResult.Yes)
	                    return;
	            }

	            LockState ls = new LockState();
	            string id = precobj.ID.ToString();
	            bool isOpen = ls.CheckForOpenObjects(id, LockableObjects.Precedent);
	            if (isOpen)
	            {
	                ls.UnlockPrecedentObject(id);
	            }

                precobj.UndoCheckOut();
	            btnUnlock.Enabled = false;
            }
	        catch (Exception exception)
	        {
	            ErrorBox.Show(ParentForm, exception);
            }
        }


        /// <summary>
        /// Closes the Precedent Manager and start the Process
        /// </summary>
        /// <param name="sender">The Sender Object</param>
        /// <param name="e">Empty EventArgs</param>
        private void btnContinue_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (FWBS.OMS.Session.CurrentSession.CurrentPrecedentJobList.LiveCount == 0)
                {
                    // Add Currently selected job
                    Common.KeyValueCollection itm = this.SelectedItem;
                    if (itm == null || itm["precid"].Value == null)
                    {
                        ResourceItem res = Session.CurrentSession.Resources.GetMessage("MSGMUSTSELPREC", "You must select a %PRECEDENT%.", "");
                        MessageBox.Show(res, "", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);
                        if (!_isAddin) ParentForm.DialogResult = DialogResult.None;
                        return;
                    }

                    var builder = new PrecedentBuilder(this.ParentForm, _assoc);
                    builder.Build(Convert.ToInt64(itm["precid"].Value));
                }
                else if (!_isAddin)
                {
                    ParentForm.DialogResult = DialogResult.OK;
                }

                if (_isAddin)
                {
                    if (Session.CurrentSession.CurrentPrecedentJobList.Count > 0)
                    {
                        UnloadPreview();
                        Services.ProcessJobList(_app);
                        LoadPreview();
                    }
                }
                else if (ParentForm.DialogResult == DialogResult.OK)
                {
                    // Check the Current Item is correct
                    UnloadPreview();
                    ParentForm.Close();
                    Services.ProcessJobList(_app);
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

		/// <summary>
		/// Captures the combo box language change event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cLang_Changed(object sender, EventArgs e)
		{
			StartTimer();
		}

		private void cAssocType_Changed(object sender, EventArgs e)
		{
			StartTimer();
		}

        /// <summary>
        /// Filters the Precedent Catagory by the Precedent Type
        /// Starts the Search Timer
        /// </summary>
        /// <param name="sender">The Sender Object</param>
        /// <param name="e">Empty EventArgs</param>
        private void cmbType_Changed(object sender, EventArgs e)
		{
			StartTimer();
        }

		/// <summary>
		/// Filters the Precedent Sub Catagory by the Precedent Catagory 
		/// Starts the Search Timer
		/// </summary>
		/// <param name="sender">The Sender Object</param>
		/// <param name="e">Empty EventArgs</param>
		private void cmbCat_Changed(object sender, EventArgs e)
		{
			StartTimer();
		}

		/// <summary>
		/// Starts the Search Timer
		/// </summary>
		/// <param name="sender">The Sender Object</param>
		/// <param name="e">Empty EventArgs</param>
		private void cmbSubType_Changed(object sender, EventArgs e)
		{
            StartTimer();
		}

		/// <summary>
		/// Fires the Search Method on the Search Control
		/// </summary>
		/// <param name="sender">The Sender Object</param>
		/// <param name="e">Empty EventArgs</param>
		private void timFilterDown_Tick(object sender, System.EventArgs e)
		{
			if (Session.CurrentSession.IsLoggedIn)
			{
				this.Cursor = Cursors.WaitCursor;
				timFilterDown.Enabled = false;
				search= true;
				lblSearching.Visible = true;
				Application.DoEvents();
				ucPrecList.Search(false,true,false);
                ucPrecFav.Search(false, true, false);
                lblSearching.Visible = false;
				search=false;
				this.Cursor = Cursors.Default;
            }
		}

		/// <summary>
		/// Private Method to Start the Search Timer
		/// </summary>
		private void StartTimer()
		{
			// Do no Start if the Searching or the Form is not Visible
			if (search==false && this.Visible==true)
			{
				timFilterDown.Enabled = false;
				timFilterDown.Interval = 500;
				timFilterDown.Enabled = true;
			}
			else
			{
				timFilterDown.Enabled = false;
            }
		}

		/// <summary>
		/// Run Once Timer use to set values after the form becomes visible override any other events
		/// </summary>
		/// <param name="sender">this</param>
		/// <param name="e">Empty</param>
		private void timRunOnce_Tick(object sender, System.EventArgs e)
		{
			timRunOnce.Enabled=false;
			if (((Control)cQuickSearch).Parent != null && ((Control)cQuickSearch).Visible==true)
				((Control)cQuickSearch).Focus();
			tcontrol.SelectedTab = tpList;

		}

		/// <summary>
		/// The Set Propeties Method
		/// </summary>
		/// <param name="sender">this</param>
		/// <param name="e">Empty</param>
		private void ucPrecedent_ParentChanged(object sender, System.EventArgs e)
		{
            if (this.Parent == null)
                return;

			if (!_joblistvisible && this.tcontrol.Contains(tpJoblist))
				this.tcontrol.Controls.Remove(this.tpJoblist);
			else if (_joblistvisible && !this.tcontrol.Contains(tpJoblist))
				this.tcontrol.Controls.Add(this.tpJoblist);
			pnlBuilder.Visible = _addjobvisible;
			btnEdit.Visible = _btneditvisible;
			btnCancel.Visible = _btncancelvisible;
			btnContinue.Visible = _btncontinuevisible;
			btnPrint.Visible = _btnprintvisible;
			btnView.Visible = _btnviewvisible;
			try
			{
                if (!_isAddin && ParentForm != null)
                {
                    if (btnContinue.Visible)
                        ParentForm.AcceptButton = btnContinue;
                    if (btnCancel.Visible)
                        ParentForm.CancelButton = btnCancel;
                }
			}
			catch
			{}
		}

		/// <summary>
		/// Fires the OnSelectedItemDoubleClick if when the Precedent it Double Clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ucPrecList_SelectedItemDoubleClick(object sender, System.EventArgs e)
		{
			OnSelectedItemDoubleClick();
		}

		/// <summary>
		/// Set focus to the Enquiry form if the User Control becomes visible 
		/// </summary>
		/// <param name="sender">this</param>
		/// <param name="e">empty</param>
		private void enqPrecHeader_Enter(object sender, System.EventArgs e)
		{
            enqPrecHeader.Focus();
		}

		private void enqPrecHeader_Refreshed(object sender, System.EventArgs e)
		{
			enqPrecHeader.AutoScroll=false;
			cQuickSearch = enqPrecHeader.GetIBasicEnquiryControl2("_quickSearch",EnquiryControlMissing.Exception);
            cLast10 = enqPrecHeader.GetControl("_quickSearch", EnquiryControlMissing.Exception) as FWBS.Common.UI.Windows.eXPComboBox;
            if (cLast10 != null)
            {
                ((ComboBox)cLast10.Control).Validated += new EventHandler(cLast10_Validated);
            }
			
			cType = enqPrecHeader.GetIBasicEnquiryControl2("_prectype",EnquiryControlMissing.Exception);
			cCat = enqPrecHeader.GetIBasicEnquiryControl2("_preccat",EnquiryControlMissing.Exception);
			cSubCat = enqPrecHeader.GetIBasicEnquiryControl2("_precsubcat",EnquiryControlMissing.Exception);
            cMinorCat = enqPrecHeader.GetIBasicEnquiryControl2("_precminorcat", EnquiryControlMissing.Exception);
            cContactType = enqPrecHeader.GetIBasicEnquiryControl2("_conttype",EnquiryControlMissing.Exception);
			cStage = enqPrecHeader.GetIBasicEnquiryControl2("_stage",EnquiryControlMissing.Exception);
			chkViewPreview = enqPrecHeader.GetControl("_preview",EnquiryControlMissing.Exception) as CheckBox;
            btnReset = enqPrecHeader.GetControl("_reset",EnquiryControlMissing.Exception) as Button;
			cLibrary = enqPrecHeader.GetIBasicEnquiryControl2("_preclib");
			cLang = enqPrecHeader.GetIBasicEnquiryControl2("_language");
			cAssocType = enqPrecHeader.GetIBasicEnquiryControl2("_assoctype");


            if (chkViewPreview != null)
                chkViewPreview.Visible = false;

            // Link the Search List Quick Filter Control to the Control within the 
            // Enquiry Form with in the QuickSearch Control
            if (cLast10 == null)
            {
                ucPrecList.QuickFilterContol = cQuickSearch.Control as Control;
                ucPrecFav.QuickFilterContol = cQuickSearch.Control as Control;
            }
            else
            {
                ucPrecList.QuickFilterContol = cLast10.Control as Control;
                ucPrecFav.QuickFilterContol = cLast10.Control as Control;
                for (int i = 0; i < _activeSearchLastTen.Count; i++)
                    cLast10.Items.Add(_activeSearchLastTen.Description(i));
            }

            if (cLibrary != null) cLibrary.ActiveChanged +=new EventHandler(cmbType_Changed);
			
			cType.ActiveChanged +=new EventHandler(cmbType_Changed);
			cCat.ActiveChanged +=new EventHandler(cmbCat_Changed);
			cSubCat.ActiveChanged +=new EventHandler(cmbSubType_Changed);
            cMinorCat.ActiveChanged += new EventHandler(cmbSubType_Changed);

            cContactType.ActiveChanged +=new EventHandler(cmbSubType_Changed);
			cStage.ActiveChanged +=new EventHandler(cmbSubType_Changed);
			if (cLang != null) cLang.ActiveChanged +=new EventHandler(cLang_Changed);
			if (cAssocType != null) cAssocType.ActiveChanged +=new EventHandler(cAssocType_Changed);

		}


		private void ucPrecedent_VisibleChanged(object sender, System.EventArgs e)
		{
			if (this.Visible && (ucPrecList.dgSearchResults.DataSource == null || force == true))
			{
				timFilterDown_Tick(sender,e);
				force = false;
			}
		}

        private void cLast10_Validated(object sender, EventArgs e)
        {
            try
            {
                string value = ((ComboBox)cLast10.Control).Text;
                if (cLast10.Items.IndexOf(value) == -1)
                {
                    cLast10.Items.Add(value);
                    _activeSearchLastTen.AddFavourite(Convert.ToString(value), "");
                    if (_activeSearchLastTen.Count > 10)
                        _activeSearchLastTen.RemoveFavourite(0);
                    _activeSearchLastTen.Update();
                }
            }
            catch
            { }
        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            if (!_isAddin) ParentForm.Close();
        }

        #endregion

		#region Properties

        public bool ShowLibraryCategory
        {
            get
            {
                return _showlibrary;
            }
            set
            {
                _showlibrary = value;
            }
        }

		[LocCategory("Visible")]
		public bool AdditionalInfoVisible
		{
			get
			{
				return _additionalinfovisible;
			}
			set
			{
				_additionalinfovisible = value;
				if (this.Parent != null)
					ucPrecedent_ParentChanged(this,EventArgs.Empty);
			}
		}

		[LocCategory("Visible")]
		public bool JobListVisible
		{
			get
			{
				return _joblistvisible;
			}
			set
			{
				_joblistvisible = value;
				if (this.Parent != null)
					ucPrecedent_ParentChanged(this,EventArgs.Empty);
			}
		}

		[LocCategory("Visible")]
		public bool AddJobVisible
		{
			get
			{
				return _addjobvisible;
			}
			set
			{
				_addjobvisible = value;
				if (this.Parent != null)
					ucPrecedent_ParentChanged(this,EventArgs.Empty);
			}
		}

		[LocCategory("Visible")]
		public bool ButtonContinueVisible
		{
			get
			{
				return _btncontinuevisible;
			}
			set
			{
				_btncontinuevisible = value;
				if (this.Parent != null)
					ucPrecedent_ParentChanged(this,EventArgs.Empty);
			}
		}

		
		[LocCategory("Visible")]
		public bool ButtonViewVisible
		{
			get
			{
				return _btnviewvisible;
			}
			set
			{
				_btnviewvisible = value;
				if (this.Parent != null)
					ucPrecedent_ParentChanged(this,EventArgs.Empty);
			}
		}

		
		[LocCategory("Visible")]
		public bool ButtonPrintVisible
		{
			get
			{
				return _btnprintvisible;
			}
			set
			{
				_btnprintvisible = value;
				if (this.Parent != null)
					ucPrecedent_ParentChanged(this,EventArgs.Empty);
			}
		}

		[LocCategory("Visible")]
		public bool ButtonCancelVisible
		{
			get
			{
				return _btncancelvisible;
			}
			set
			{
				_btncancelvisible = value;
				if (this.Parent != null)
					ucPrecedent_ParentChanged(this,EventArgs.Empty);
			}
		}

		[LocCategory("Visible")]
		public bool ButtonEditVisible
		{
			get
			{
				return _btneditvisible;
			}
			set
			{
				_btneditvisible = value;
				if (this.Parent != null)
					ucPrecedent_ParentChanged(this,EventArgs.Empty);
			}
		}

		/// <summary>
		/// The Currently Selected Precedent
		/// </summary>
		[Browsable(false)]
		public FWBS.Common.KeyValueCollection SelectedItem
		{
			get
			{
				ucSearchControl precList = (tcontrol.SelectedTab != tpFav) ? ucPrecList : ucPrecFav;
				return precList.CurrentItem();
			}
		}

        [Browsable(false)]
        public int VisibleRows
        {
            get
            {
                return _visibleRowCount;
            }
        }

        [Browsable(false)]
        public string SelectedLibrary
        {
            get
            {
                return _selectedLibrary;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Private Method to turn a Control into a IBaseEnquiryControl2 Object
        /// </summary>
        /// <param name="ObjectControl"></param>
        /// <returns></returns>
        private FWBS.Common.UI.IBasicEnquiryControl2 GetEnquiryControl(object ObjectControl)
        {
            return ObjectControl as FWBS.Common.UI.IBasicEnquiryControl2;
        }


        /// <summary>
        /// Update the GUI with the current job list.
        /// </summary>
        private void UpdateJobList(object sender, EventArgs e)
        {
            string text = ResourceLookup.GetLookupText("PRECJOBLIST");
            //Initial resource lookup event kicking in on first text set.
            tpJoblist.Text = "";

            if (Session.CurrentSession.CurrentPrecedentJobList.LiveCount == 0)
            {
                // Nothing todo here so clear down the list.
                tpJoblist.Text = text;
            }
            else
            {
                tpJoblist.Text = string.Format("{0} ({1})", text, Session.CurrentSession.CurrentPrecedentJobList.LiveCount);
            }

            if (tcontrol.SelectedTab != tpJoblist)
            {
                _jobListUpdatePending = true;
            }
            else if (Functions.GetActiveWindow() != this.ParentForm?.Handle)
            {
                ucJoblist1.RefreshJobList();
                _jobListUpdatePending = false;
            }
        }

        private void tcontrol_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_jobListUpdatePending && e.TabPage == tpJoblist)
            {
                ucJoblist1.RefreshJobList();
                _jobListUpdatePending = false;
            }
        }

        private void BuildIconColumn(ucSearchControl searchControl)
        {
            if (searchControl.Columns.Count == 0)
                return;

            var icon_col = searchControl.Columns[0] as DataGridViewLabelColumn;
            if (icon_col == null)
                return;

            var dt = searchControl.DataTable;
            if (dt == null)
                return;

            if (!dt.Columns.Contains("precextension"))
                return;

            if (!dt.Columns.Contains("#icon#"))
                dt.Columns.Add("#icon#", typeof(string));

            if (dt.Columns.Contains("precextension") && dt.Columns.Contains("preccheckedoutby"))
            {
                foreach (DataRow r in dt.Rows)
                {
                    string file;
                    string ext = Convert.ToString(r["precextension"]).TrimStart('.');

                    if (r["preccheckedoutby"] == DBNull.Value)
                    {
                        file = string.Format("checkedin.{0}", ext);
                    }
                    else if (Convert.ToInt32(r["preccheckedoutby"]) == Session.CurrentSession.CurrentUser.ID)
                    {
                        file = string.Format("checkedout.{0}", ext);
                    }
                    else
                    {
                        file = string.Format("locked.{0}", ext);
                    }

                    r["#icon#"] = file;

                    if (!icons.Images.ContainsKey(file))
                    {
                        Image icon = FWBS.Common.IconReader.GetFileIcon(file, FWBS.Common.IconReader.IconSize.Small, false).ToBitmap();

                        if (file.StartsWith("checkedout"))
                        {
                            using (Graphics g = Graphics.FromImage(icon))
                            {
                                try
                                {
                                    g.DrawImage(imageList1.Images[0], 0, 0);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                            }
                        }

                        else if (file.StartsWith("locked"))
                        {
                            using (Graphics g = Graphics.FromImage(icon))
                            {
                                try
                                {
                                    g.DrawImage(imageList1.Images[1], 0, 0);
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

        private void RunButtonAction(object sender, Precedent precobj)
        {
            if (sender == btnEdit)
            {
                if(!CheckPrecedentLocking(precobj))
                {
                    if (Services.OpenPrecedent(precobj, DocOpenMode.Edit, !(Session.CurrentSession.EnablePrecedentVersioning)) && !_isAddin)
                        this.ParentForm.Close();
                    else
                        LoadPreview();
                }
            }
            else if (sender == btnView)
            {
                if (Services.OpenPrecedent(precobj, DocOpenMode.View))
                    LoadPreview();
            }
            else if (sender == btnPrint)
            {
                if (Services.OpenPrecedent(precobj, DocOpenMode.Print))
                    LoadPreview();
            }
        }

        private bool CheckPrecedentLocking(Precedent prec)
        {
            string appCode = GetAppCode(prec);
            string precID = Convert.ToString(prec.ID);
            if (appCode == "OUTLOOK" || appCode == "WORD" || appCode == "EXCEL")
            {
                if (Session.CurrentSession.ObjectLocking)
                {
                    LockState ls = new LockState();
                    if (ls.CheckObjectLockState(precID, LockableObjects.Precedent))
                        return true;
                    else
                    {
                        ls.LockPrecedentObject(precID);
                        ls.MarkObjectAsOpen(precID, LockableObjects.Precedent);
                    }
                }
            }
            return false;
        }

        private string GetAppCode(Precedent prec)
        {
            Apps.RegisteredApplication app = prec.PrecProgType;
            if (app.Code == "SHELL")
            {
                FWBS.OMS.DocumentManagement.Storage.IStorageItem item = prec;
                app = Apps.ApplicationManager.CurrentManager.GetRegisteredApplicationByExtension(item.Extension);
                if (app == null)
                    app = prec.PrecProgType;
                else
                {
                    prec.PrecProgType = app;
                    prec.Update();
                }
            }
            return prec.PrecProgType.Code;
        }


        #endregion
        
        #region Previewer
              

        private void omsDockManager1_PaneDisplayed(object sender, Infragistics.Win.UltraWinDock.PaneDisplayedEventArgs e)
        {
            if (e.Pane == omsDockManager1.PaneFromControl(documentPreviewer1))
            {
                SetPrecedentToBePreviewed();

                documentPreviewer1.RefreshItem();
            }
        }

        private void SetPrecedentToBePreviewed()
        {
            ucSearchControl searchControl = null;

            if (tcontrol.SelectedTab == tpList)
            {
                searchControl = ucPrecList;
            }
            else if (tcontrol.SelectedTab == tpFav)
            {
                searchControl = ucPrecFav;
            }

            if (searchControl?.dgSearchResults?.CurrentRowIndex >= 0)
            {
                omsDockManager1.Visible = true;
                try
                {
                    Common.KeyValueCollection ret = searchControl.CurrentItem();
                    Infragistics.Win.UltraWinDock.DockableControlPane pane = omsDockManager1.PaneFromControl(documentPreviewer1);

                    if (pane.IsInView)
                        documentPreviewer1.Connect(Precedent.GetPrecedent(Convert.ToInt64(ret["PRECID"].Value)));
                    else
                        documentPreviewer1.Connect(null);

                    if (pane.Pinned)
                        ResetTimer();
                }
                catch (Security.SecurityException ex)
                {
                    documentPreviewer1.SetError(ex.Message);
                }
            }
            else
            {
                omsDockManager1.Visible = false;
                documentPreviewer1.Connect(null);
            }
        }

        private void UnloadPreview()
        {
            documentPreviewer1.UnloadPreview();
        }
        private void LoadPreview()
        {
            if (documentPreviewer1.Visible)
                documentPreviewer1.LoadPreview();
        }

        private void ResetTimer()
        {
            timer1.Enabled = false;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            documentPreviewer1.RefreshItem();

        }

        #endregion

        private void btnAdditionalInfo_Click(object sender, EventArgs e, long precId)
        {
            FWBS.Common.KeyValueCollection keys = new FWBS.Common.KeyValueCollection();
            keys.Add("PRECID", precId);
            //Changed to make all prec types appear when editing precedent.
            keys.Add("APP", DBNull.Value);

            Services.ShowOMSItem(Session.CurrentSession.DefaultSystemForm(SystemForms.PrecedentEdit), null, EnquiryEngine.EnquiryMode.Edit, keys);
            ucPrecList.Search(true, false, false);
            ucPrecFav.Search(true, false, false);
        }
    }
}
