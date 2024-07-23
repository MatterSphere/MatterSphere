using System;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// An intelligent associate wizard form.
    /// </summary>
    internal class frmAssociateWizard : frmOMSTypeWizard
    {
        #region Fields

        /// <summary>
        /// The file to be manipulated.
        /// </summary>
        private OMSFile _file = null;

		/// <summary>
		/// The contact that is being associated to a file.
		/// </summary>
		private Contact _contact = null;

		/// <summary>
		/// The associate object that is to be created.
		/// </summary>
		private Associate _associate = null;

		/// <summary>
		/// The contact info display control.
		/// </summary>
		private IBasicEnquiryControl2 _contactInfo = null;

		/// <summary>
		/// The associate heading type.
		/// </summary>
		private IListEnquiryControl _type = null;

		/// <summary>
		/// Quick heading combo box.
		/// </summary>
		private IBasicEnquiryControl2 _quickHeading = null;

		/// <summary>
		/// Heading multiline text box.
		/// </summary>
		private IBasicEnquiryControl2 _heading = null;

		/// <summary>
		/// The cntact address selector.
		/// </summary>
		private eAddressSelector _addresses = null;

		/// <summary>
		/// A flag to determine whether to show the search screen.
		/// </summary>
		private bool _showSearch = true;

		/// <summary>
		/// Contact Name Control
		/// </summary>
		private IBasicEnquiryControl2 _contactname = null;

        /// <summary>
		/// The oms type object to manipulate once on is chosen.
		/// </summary>
		private AssociateType _omsType = null;

        /// <summary>
        /// The return contact object used from the wizard.
        /// </summary>
        private Associate _obj = null;

        private bool _offline;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor not used.
		/// </summary>
		private frmAssociateWizard() : base()
		{
		}

		/// <summary>
		/// Creates a new  associate wizard foirm specifying the file to be associate to.
		/// </summary>
		/// <param name="file">The oms file to be associated.</param>
		/// <param name="param">Extra parameters to be passed if need be.</param>
		public frmAssociateWizard(OMSFile file, Common.KeyValueCollection param) : this(file, null, false, param)
		{
		}

		/// <summary>
		/// Creates a new associate with the specified file and contact.
		/// </summary>
		/// <param name="file">The oms file to be associated.</param>
		/// <param name="contact">The contact to be associated to the file.</param>
		/// <param name="param">Extra parameters to be passed if need be.</param>
		public frmAssociateWizard(OMSFile file, Contact contact, bool offline, Common.KeyValueCollection param) : this()
        {
            SetupWizard(file, contact, offline, param);
        }

        private void SetupWizard(OMSFile file, Contact contact, bool offline, Common.KeyValueCollection param)
        {
            _file = file;
            _contact = contact;
            InitializeEnquiryForm(SystemForms.AssociateWizard, file, offline, param);
            _associate = enquiryForm1.Enquiry.Object as Associate;

            _type = enquiryForm1.GetControl("_type", EnquiryControlMissing.Exception) as IListEnquiryControl;
            SetControlValues(enquiryForm1);

            if (_contact != null)
            {
                _showSearch = false;
                ValidateContact();
            }
        }

        public frmAssociateWizard(OMSFile file, Contact contact, bool offline, WizardStyle wizardStyle = WizardStyle.Dialog) : base(wizardStyle)
        {
            _file = file;
            _contact = contact;
            _offline = offline;
            InitializeEnquiryForm(SystemForms.AssociateTypePicker, file, offline, null);
            EnableFinish(false);

            _associate = enquiryForm1.Enquiry.Object as Associate;
            _type = enquiryForm1.GetControl("_type", EnquiryControlMissing.Exception) as IListEnquiryControl;
            //Type selector.
            if (_type != null)
            {
                _type.ActiveChanged += new EventHandler(this.TypeChanged);
            }

            if (_contact != null)
            {
                ValidateContact();
                _showSearch = false;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (wizardStyle == WizardStyle.InPlace)
            {
                UpdateFinishButtonText(Session.CurrentSession
                    .Resources.GetResource("CREATEENTITY", "Create %1%", string.Empty, true, "%ASSOCIATE%").Text);
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
        #endregion

        #region Properties

        public Associate Associate { get { return _obj; } }

        #endregion

        #region Methods

        /// <summary>
        /// Validates the contact for the associate by updating the wizard information with information.
        /// </summary>
        private void ValidateContact()
		{
			if (_contact != null)
            {
                _associate.SetAssociateInfo(_file, _contact, "");
                if (_contactname != null)
                    enquiryForm1.Enquiry.SetValue("_contactName", _contact.Name);

                if (_contactInfo != null) enquiryForm1.Enquiry.SetValue("_contactInfo", _contact.ContactDescription);
                if (_type != null)
                {
                    System.Data.DataTable dt = _associate.GetAssociateTypes();

                    _type.AddItem(dt, "typecode", "typedesc");
                    if (_associate.IsClient)
                        enquiryForm1.Enquiry.SetValue("_type", "CLIENT");
                    else
                        enquiryForm1.Enquiry.SetValue("_type", DBNull.Value);

                    foreach(Control ctrl in enquiryForm1.Controls)
                    {
                        if (ctrl is eAddressSelector)
                            ((eAddressSelector)ctrl).Reload(_contact);
                        else if (ctrl is eNumberSelector)
                            ((eNumberSelector)ctrl).Reload(_contact);
                        else if (ctrl is eEmailSelector)
                            ((eEmailSelector)ctrl).Reload(_contact);
                    }
                }
            }
        }

        private void TypeChanged(object sender, EventArgs e)
        {
            if (enquiryForm1.PageNumber > -1)
            {
                btnNext.Enabled = (_type.Value != DBNull.Value);
            }
        }

        private void SetControlValues(EnquiryForm enquiryForm)
	    {
            _contactInfo = enquiryForm.GetIBasicEnquiryControl2("_contactInfo");
            _addresses = enquiryForm.GetControl("_addresses", EnquiryControlMissing.Exception) as eAddressSelector;
            _quickHeading = enquiryForm.GetIBasicEnquiryControl2("_quickHeading", EnquiryControlMissing.Exception);
            _heading = enquiryForm.GetIBasicEnquiryControl2("_heading", EnquiryControlMissing.Exception);
            _contactname = enquiryForm.GetIBasicEnquiryControl2("_contactName");
        }

        #endregion
        
        #region Captured Events

        /// <summary>
        /// Captures the quick heading combo change event so that the assoc heading can be set.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _quickHeading_ActiveChanged(object sender, System.EventArgs e)
		{
            string quickHeading = Convert.ToString(_quickHeading.Value);
            string heading = Convert.ToString(_heading.Value);
            if (heading.Length == 0)
            {
                _heading.Value = quickHeading;
            }
            else
            {
                string[] headings = heading.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (!headings.Any(h => h.StartsWith(quickHeading.TrimEnd(), StringComparison.CurrentCultureIgnoreCase)))
                {
                    heading += (Environment.NewLine + quickHeading);
                    if (heading.Length <= 255)
                    {
                        _heading.Value = heading;
                    }
                }
            }
		}

		/// <summary>
		/// This method gets run after an item has been selected from the search list.
		/// </summary>
		/// <param name="sender">Conflict Search control</param>
		/// <param name="returnValues">Conflict search return values.</param>
		protected override void OnConflictItemSelected(object sender, Common.KeyValueCollection returnValues)
		{
			try
			{
				_contact = Contact.GetContact(Convert.ToInt64(returnValues["contid"].Value));
			}
			catch (Exception ex)
			{
				_contact = null;
				ErrorBox.Show(this, ex);
			}
			finally
			{
				btnNext.Enabled = (_contact != null);
                EnableFinish(false);
                ValidateContact();
			}
		}


        private void FillquickHeading(IBasicEnquiryControl2 quickHeading)
        {
            if (quickHeading != null)
            {
                quickHeading.ActiveChanged -= new EventHandler(_quickHeading_ActiveChanged);
                quickHeading.ActiveChanged += new EventHandler(_quickHeading_ActiveChanged);

                if (quickHeading is IListEnquiryControl)
                {
                    ((IListEnquiryControl)quickHeading).AddItem(_file.GetAssociateHeadings());
                    quickHeading.Value = string.Empty;
                }
            }

        }

		/// <summary>
		/// Captures the enquiry form wizard before page change event.
		/// </summary>
		/// <param name="sender">The enqiry form reference.</param>
		/// <param name="e">Page changing event arguments.</param>
		protected override void enquiryForm1_PageChanging(object sender, FWBS.OMS.UI.Windows.PageChangingEventArgs e)
		{
			base.enquiryForm1_PageChanging(sender, e);

			//Skip the search screen if a contact was originally specified.
			if (e.PageType == EnquiryPageType.Start && e.Direction == EnquiryPageDirection.Next)
			{
				if (_conflict == null || _showSearch == false)
				{
					enquiryForm1.GotoPage((short)(e.PageNumber + 2), false);

                     if (_contact != null) 
                     { 
                        FillquickHeading(_quickHeading);                         
                     } 


					e.Cancel = true;
					return;
				}
				else
				{
					enquiryForm1.ActionNext = null;
					btnNext.Enabled = (_contact != null);
					if (btnNext.Enabled)
						enquiryForm1.ActionNext = btnNext;
					if (_contact == null)
					{
						if (_conflict != null)
						{
							_conflict.SearchButtonCommands -= new SearchButtonEventHandler(_search_SearchButtonCommands);
                            _conflict.SearchCompleted -= new SearchCompletedEventHandler(_search_SearchCompleted);
							SetConflictSearchList(SystemSearchListGroups.ContactAssociate, _file);
                            _conflict.SearchButtonCommands += new SearchButtonEventHandler(_search_SearchButtonCommands);
                            _conflict.SearchCompleted += new SearchCompletedEventHandler(_search_SearchCompleted);
						}
					}
				}
			}
				//Skip the search screen if a contact was originally specified.
			else if (e.Direction == EnquiryPageDirection.Back && e.PageNumber == 1)
			{
				if (_conflict == null || _showSearch == false)
				{
					enquiryForm1.GotoPage((short)(e.PageNumber - 2), false);
					e.Cancel = true;
					return;
				}
				else
                    ShowHeaderPanel(false);

			}
				//Select the item from the contact search list ready for the next stage.
			else if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Next && e.PageNumber == 0)
			{
				if (_conflict != null && _showSearch == true)
				{
					_conflict.SelectRowItem();
					
					if (_contact != null)
					{
                        FillquickHeading(_quickHeading);
					}
					else
					{
						e.Cancel = true;
						return;
					}
				}
                //Added DMB 9/2/2004 Next button was disabled following adding of a new contact
                btnNext.Enabled = _type.Value != DBNull.Value;
                if (btnNext.Enabled)
					enquiryForm1.ActionNext = btnNext;
			}
            else if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Next && e.PageName == "ADDINFO")
            {
                try
                {
                    //Get the current value of the type list control.
                    string type = Convert.ToString(_type.Value);

                    //Depending on the oms type object, get their type describing
                    //data so that thw wizard enquiry code can be found for that
                    //specific type.
                    if (type == String.Empty)
                    {
                        e.Cancel = true;
                        return;
                    }

                    bool objchanged = false;
                    if (_omsType == null || _omsType.Code != type || _obj?.Contact != _contact)
                    {
                        _omsType = AssociateType.GetAssociateType(type);
                        _obj = (Associate)_omsType.CreateObject(new object[] { _contact, _file});
                        objchanged = true;
                    }

                    InitializeCustomForm(_omsType.Wizard, SystemForms.AssociateWizard, _obj, objchanged);
                    custom.Enquiry.Offline = _offline;

                    SetControlValues(custom);
                    FillquickHeading(_quickHeading);
                    ValidateContact();
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    ErrorBox.Show(this, ex);
                }
            }
        }

        /// <summary>
        /// Captures the after page change of the wizard.
        /// </summary>
        /// <param name="sender">The enqiry form reference.</param>
        /// <param name="e">Page changed event arguments.</param>
        protected override void enquiryForm1_PageChanged(object sender, FWBS.OMS.UI.Windows.PageChangedEventArgs e)
		{
			base.enquiryForm1_PageChanged(sender, e);
			//Make sure the header panel is visible.
            ShowHeaderPanel(!IsConflictSearchVisible());

            if (IsConflictSearchVisible())
			{
				this.AcceptButton = _conflict.cmdSearch;
			}

			//DB 15/1/2004 Fix with catch-all for wizard when going back to start screen sometimes next button is still disabled
			if (e.PageNumber == -1)
			{
				btnNext.Enabled = true;
				enquiryForm1.ActionNext = btnNext;
			}
            if (e.PageName.ToUpper() == "ADDINFO" && _type.Value == DBNull.Value)
            {
                btnNext.Enabled = false;
            }

            if (e.PageName.ToUpper() == "SEARCH" || e.PageName.ToUpper() == "ADDINFO")
                EnableFinish(false);
        }

	
		private void _search_SearchButtonCommands(object sender, SearchButtonEventArgs e)
		{
			if (e.ButtonName == "cmdAddContact")
			{
				_contact = Services.Wizards.CreateContact(true, null, this, Modal || !TopLevel);
                if (_contact != null)
                {
                    _contact.Refresh(true);
                    ValidateContact();

                    if (_conflict != null)
                        _conflict.SearchList.ClearResults();

                    enquiryForm1.ActionNext = btnNext;
                    enquiryForm1.NextPage();
                }
			}
		}


		private void _search_SearchCompleted(object sender, SearchCompletedEventArgs e)
		{
			btnNext.Enabled = (_conflict.SearchList.ResultCount > 0);
			if (btnNext.Enabled)
				enquiryForm1.ActionNext = btnNext;
		}

        /// <summary>
        /// Captures the finished event of the enquiry form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void custom_Finished(object sender, EventArgs e)
        {
            this.Tag = _obj;
            base.custom_Finished(sender, e);
        }

		#endregion

	}
}
