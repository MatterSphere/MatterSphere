using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.Common.UI;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A specialised wizard form that will create a new contact.
    /// </summary>
    internal class frmClientTakeonWizard : frmOMSTypeWizard
    {
        #region Fields

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        /// <summary>
        /// The return contact object used from the wizard.
        /// </summary>
        private Client _obj = null;

        /// <summary>
        /// The oms type object to manipulate once on is chosen.
        /// </summary>
        private ClientType _omsType = null;

        /// <summary>
        /// The default contact to use when creating a client.
        /// </summary>
        private Contact _defContact = null;

        /// <summary>
        /// A flag to indicate the client takeon is a pre client.
        /// </summary>
        private bool _preclient = false;


        /// <summary>
        /// A variable which decides to view the client after use or not.
        /// </summary>
        private bool _viewClient = false;

        #region Required Controls

        /// <summary>
        /// A type picker list.
        /// </summary>
        private IBasicEnquiryControl2 _type = null;

        /// <summary>
        /// Holds a number of contacts when creating a client object.
        /// </summary>
        private FWBS.OMS.UI.Windows.ucSelectorRepeaterContainer _contactSelector = null;

        /// <summary>
        /// A reference to the preclient info control.
        /// </summary>
        private IBasicEnquiryControl2 _preclientfiledesc = null;

        /// <summary>
        /// A reference to the preclient info control.
        /// </summary>
        private IBasicEnquiryControl2 _preclientdept = null;

        /// <summary>
        /// A reference to the preclient info control.
        /// </summary>
        private IBasicEnquiryControl2 _preclientfiletype = null;

        /// <summary>
        /// A checkbox which accepts whether to display the client after it has been created.
        /// </summary>
        private IBasicEnquiryControl2 _view = null;

        #endregion


        #endregion

        #region Contructors & Destructors

        /// <summary>
        /// Default contructor not used.
        /// </summary>
        private frmClientTakeonWizard()
            : base()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

        }

        /// <summary>
        /// Creates a new intance of the client wizard form.
        /// </summary>
        /// <param name="defaultContact">The default contact to use.</param>
        /// <param name="param">Any optional parameters needed to populate values.</param>
        /// <param name="wizardStyle">Parameter indicating Wizard style.</param>
        public frmClientTakeonWizard(Contact defaultContact, bool asPreClient, bool quick, Common.KeyValueCollection param, WizardStyle wizardStyle = WizardStyle.Dialog) : base(wizardStyle)
        {
            InitializeEnquiryForm(asPreClient ? SystemForms.PreClientType : SystemForms.ClientTypePicker, defaultContact, true, param);

            enquiryForm1.ChangeParentFormSize = false;
            //Get the conflict searcher control.
            _type = enquiryForm1.GetControl("_type") as IBasicEnquiryControl2;
            _contactSelector = enquiryForm1.GetControl("_contactSelector") as ucSelectorRepeaterContainer;

            //Fetch a reference to the preclient relevant controls.
            _preclientfiledesc = enquiryForm1.GetControl("_preclientfiledesc") as IBasicEnquiryControl2;
            _preclientdept = enquiryForm1.GetControl("_preclientdept") as IBasicEnquiryControl2;
            _preclientfiletype = enquiryForm1.GetControl("_preclientfiletype") as IBasicEnquiryControl2;

            //Type selector.
            if (_type != null)
            {
                _type.Changed += new EventHandler(this.TypeChanged);
            }

            if (_contactSelector != null)
                _contactSelector.SelectorRepeaterType = typeof(ucContactSelector);

            _preclient = asPreClient;
            _defContact = defaultContact;
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

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // frmClientTakeonWizard
            // 
            this.Name = "frmClientTakeonWizard";
            this.Text = "frmClientTakeonWizard";
        }
        #endregion

        #endregion

        #region Captured Events

        /// <summary>
        /// Captures the enquiry forms after page change event which passes current page information.
        /// </summary>
        /// <param name="sender">Enquiry form object that raised the event.</param>
        /// <param name="e">Specifies the page number, page type and direction the wizard is going.</param>
        protected override void enquiryForm1_PageChanged(object sender, FWBS.OMS.UI.Windows.PageChangedEventArgs e)
        {
            try
            {
                ShowHeaderPanel(e.PageName != "SEARCH");

                // ******************************************************************************************
                // Used for the Original Client Takeon the Contact Selector
                // this was on the First Enquiry Form
                // ******************************************************************************************
                if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Next && e.PageName == "CONTACTS")
                {
                    if (_contactSelector != null)
                    {
                        if (_contactSelector != null && _contactSelector.Visible)
                            _contactSelector.Select(true);

                        if (_contactSelector.DefaultObject == null)
                        {
                            //Ask if the user wants to create a new contact for the client if one has not already been sepcified.
                            if (MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("4002", "Would you like to create a new contact to associate to a new %CLIENT%?", ""), null, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                                _contactSelector.RunMethod(SelectorRepeaterMethods.Assign);
                            else
                                _contactSelector.RunMethod(SelectorRepeaterMethods.Find);

                        }
                    }
                }
                //If the wizard is on its last custom page then show the associated enquiry form.
                else if (e.PageType == EnquiryPageType.Custom && e.Direction == EnquiryPageDirection.Next)
                {
                    // ******************************************************************************************
                    // Used for the Original Client Takeon the Contact Selector
                    // ******************************************************************************************
                    if (_obj != null && _contactSelector != null)
                    {
                        _obj.ClearContacts();
                        _obj.DefaultContact = ((ClientContactLink)_contactSelector.DefaultObject).Contact;
                        foreach (ClientContactLink cont in _contactSelector.OtherObjects)
                        {
                            _obj.AddContact(cont);
                        }
                    }

                    // ******************************************************************************************
                    // Used when the Client Name was set on the First Enquiry Form
                    // ******************************************************************************************
                    if (enquiryForm1.GetIBasicEnquiryControl2("ClientName", EnquiryControlMissing.None) != null)
                    {
                        try
                        {
                            _obj.ClientName = Convert.ToString(enquiryForm1.GetIBasicEnquiryControl2("ClientName").Value);
                        }
                        catch
                        {
                            // Not the Quick Client Takeon Form
                        }
                    }
                }

                //Run the base forms event first.
                base.enquiryForm1_PageChanged(sender, e);

                //Make sure that the conflict search has its search button pressed by default.
                //Change the form caption to Client Conflict search as well.
                if (IsConflictSearchVisible())
                {
                    this.Text = Session.CurrentSession.Resources.GetResource("CLCONFLICTSCH", "%CLIENT% Conflict Search", "").Text;

                    //If the search form has the client type search control
                    //then set the default value to the client type chosen.
                    Common.KeyValueCollection defaults = new Common.KeyValueCollection();
                    defaults.Add("@CLTYPE", _omsType.Code); //"cboCLType"
                    SetConflictSearchDefaults(defaults);
                }
                else
                    this.Text = labWelcome.Text;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }

        }

        /// <summary>
        /// The event that gets captures before a page change.
        /// </summary>
        /// <param name="sender">Enquiry form.</param>
        /// <param name="e">Before page change event arguments.</param>
        protected override void enquiryForm1_PageChanging(object sender, FWBS.OMS.UI.Windows.PageChangingEventArgs e)
        {
            base.enquiryForm1_PageChanging(sender, e);

            if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Next && e.PageNumber == 0)
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

                    if (_omsType == null || _omsType.Code != type)
                    {
                        _omsType = ClientType.GetClientType(type);
                        _obj = (Client)_omsType.CreateObject(new object[2] { _defContact, _preclient });
                        objchanged = true;
                    }

                    if (_contactSelector != null)
                    {
                        _contactSelector.Clear();
                        _contactSelector.DefaultObject = _obj.DefaultContact;
                        foreach (Contact cont in _obj.Contacts)
                        {
                            if (cont.ID != _obj.DefaultContact.ID)
                                _contactSelector.Add(cont);
                        }
                        _contactSelector.SetCountBounds(_omsType.MinimumContactCount, _omsType.MaximumContactCount);
                    }

                    InitializeCustomForm(_omsType.Wizard, SystemForms.ClientWizard, _obj, objchanged);

                    _view = custom.GetIBasicEnquiryControl2("_view");



                    if (_omsType.SearchOnCreate)
                    {
                        SetConflictSearchList(SystemSearchListGroups.Client, _defContact);
                    }
                    else
                    {
                        enquiryForm1.GotoPage("CUSTOM", false);
                        ShowHeaderPanel(true);
                        e.Cancel = true;
                        return;
                    }

                }

                catch (Exception ex)
                {
                    e.Cancel = true;
                    ErrorBox.Show(this, ex);
                }
            }
            else if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Next && e.PageNumber == 1)
            {
                if (_conflictSearches.Count == 0)
                {
                    //Make sure that the user is aware that a conflict search is required.
                    if (MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("4000", "Are you sure that you do not want to check to see if the %CLIENT% exists before continuing?", ""), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }

                }
                else
                {
                    SearchCompletedEventArgs sch = _conflictSearches[_conflictSearches.Count - 1] as SearchCompletedEventArgs;
                    if (sch != null && sch.Count > 0)
                    {
                        //Make sure that the user is aware that there were entries within the conflict search.
                        if (MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("4001", "Are you sure that you would like to continue with creating the %CLIENT%, even if there were '%1%' %CLIENTS% found within the last search made? ", "", sch.Count.ToString()), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
                ShowHeaderPanel(true);
            }
            else if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Next && e.PageNumber == 2)
            {
                if (_contactSelector != null)
                {
                    try
                    {
                        _contactSelector.ValidateObjects();
                    }
                    catch (Exception ex)
                    {
                        e.Cancel = true;
                        ErrorBox.Show(this, ex);
                    }
                }
            }
        }


        /// <summary>
        /// Captures the cancelled event of the wizard enquiry form.
        /// </summary>
        /// <param name="sender">The cancel button form reference.</param>
        /// <param name="e">Empty event arguments.</param>
        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            _obj = null;
        }

        /// <summary>
        /// Captures the changed event of the type enquiry control listing the 
        /// valid object types.  If the value is NULL disable the next and finish buttons,
        /// otherwise allow the next and merge the wizard with that particular chosen type
        /// uses.
        /// </summary>
        /// <param name="sender">Type listing control.</param>
        /// <param name="e">Empty event arguments.</param>
        private void TypeChanged(object sender, EventArgs e)
        {
            //Disable the next button if the value chosen from the type list is
            //DBNull.
            if (enquiryForm1.PageNumber > -1)
                btnNext.Enabled = (_type.Value != DBNull.Value);
            btnFinished.Enabled = false;

        }

        /// <summary>
        /// This method gets run after an item has been selected from the conflict search list.
        /// </summary>
        /// <param name="sender">Conflict Search control</param>
        /// <param name="returnValues">Conflict search return values.</param>
        protected override void OnConflictItemSelected(object sender, Common.KeyValueCollection returnValues)
        {
            _obj = Client.GetClient(Convert.ToInt64(returnValues["clid"].Value));
            this.Tag = _obj;
            this.DialogResult = TopLevel ? DialogResult.OK : DialogResult.Yes;
            this.Hide();
            if (wizardStyle == WizardStyle.Dialog && MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("CREATEFILE", "Would you like to create a %FILE% for this %CLIENT%?", "", true), null, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                _viewClient = false;
                Services.Wizards.CreateFile(Client, this, Modal);
            }
            enquiryForm1_Finished(sender, EventArgs.Empty);
        }


        /// <summary>
        /// Captures the finishing event of the enquiry form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void custom_Finishing(object sender, CancelEventArgs e)
        {
            try
            {
                if (_obj.ClientName == "")
                {
                    custom.ShowMissingField(custom.GetControl("ClientName", EnquiryControlMissing.Exception), Session.CurrentSession.Resources.GetResource("REQFIELD", "This field is required.", "").Text);
                    ValidatedField field = new ValidatedField("ClientName", custom.GetControl("ClientName", EnquiryControlMissing.Exception).Text, "");
                    ErrorBox.Show(this, new EnquiryValidationFieldException(HelpIndexes.EnquiryRequiredField, field));
                    e.Cancel = true;
                    return;
                }

                eContactPassiveSearch ps = null;
                ps = custom.GetControl("DefaultContact") as eContactPassiveSearch;
                // If not on the First Enquiry Form then Check the Custom Form
                if (ps == null) ps = custom.GetControl("DefaultContact") as eContactPassiveSearch;
                if (ps != null)
                {
                    if (ps.IsContact)
                    {
                        if (ps.ContactTypeName == "") ps.ContactTypeName = _omsType.ContactType;

                        Contact n = ps.Contact;

                        if (n != null)
                        {
                            if (ps.RelationshipControl != "")
                            {
                                IBasicEnquiryControl2 ibRelationship = custom.GetIBasicEnquiryControl2(ps.RelationshipControl, EnquiryControlMissing.None);
                                if (ibRelationship != null && ibRelationship.Value != DBNull.Value)
                                    _obj.SetDefaultContactAndRelationShip(n, Convert.ToString(ibRelationship.Value));
                                else
                                    _obj.DefaultContact = n;
                            }
                            else
                                _obj.DefaultContact = n;

                            ps.ErrorsOff();
                        }
                    }
                    else
                    {
                        custom.GotoControl("DefaultContact");
                        ps.ErrorsOn();
                        e.Cancel = true;
                        return;
                    }
                }

                eContactPassiveSearch psa = null;
                for (int i = 1; i < 10; i++)
                {
                    psa = custom.GetControl("Contact" + i.ToString()) as eContactPassiveSearch;
                    if (psa != null)
                    {
                        IBasicEnquiryControl2 pa = custom.GetIBasicEnquiryControl2(psa.IsAddressSameAsPrinciple);
                        if (pa != null && Convert.ToBoolean(pa.Value) == true) psa.AddressControl.Value = ps.AddressControl.Value;
                        if (psa.IsContact)
                        {
                            if (psa.ContactTypeName == "") psa.ContactTypeName = _omsType.ContactType;
                            if (psa.Contact != null)
                            {
                                ClientContactLink cllink = new ClientContactLink(_obj, psa.Contact);

                                if (Convert.ToString(psa.RelationshipControl) != "")
                                {
                                    IBasicEnquiryControl2 ibRelationship = custom.GetIBasicEnquiryControl2(psa.RelationshipControl, EnquiryControlMissing.None);
                                    if (ibRelationship != null && ibRelationship.Value != DBNull.Value)
                                    {
                                        cllink.RelationCode = Convert.ToString(ibRelationship.Value);
                                    }
                                }
                                _obj.AddContact(cllink);
                                psa.ErrorsOff();
                            }
                        }
                        else
                        {
                            custom.GotoControl("Contact" + i.ToString());
                            psa.ErrorsOn();
                            e.Cancel = true;
                            break;
                        }
                    }
                }

                //Set the pre client file information.
                if (Client.IsPreClient)
                {
                    if (_preclientfiledesc != null)
                        Client.PreClientFileDesc = Convert.ToString(_preclientfiledesc.Value);
                    if (_preclientdept != null)
                        Client.PreClientFileDepartment = Convert.ToString(_preclientdept.Value);
                    if (_preclientfiletype != null)
                        Client.PreClientFileType = Convert.ToString(_preclientfiletype.Value);
                }
            }
            catch (System.Reflection.TargetInvocationException ex)
            {
                //Captures Advanced Security error which may occur (via an Update method which is accessed through reflection)
                if (ex.InnerException is FWBS.OMS.Security.PermissionsException)
                {
                    ErrorBox.Show(this, ex.InnerException);
                    e.Cancel = true;
                    return;
                }
                else
                {
                    //Placeholder: required if any other reflection methods are used that are not related to advanced security
                    ErrorBox.Show(this, ex);
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }


        /// <summary>
        /// Captures the finished event of the enquiry form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void custom_Finished(object sender, EventArgs e)
        {
            this.Tag = _obj;

            if (Client.IsPreClient)
            {
                MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("4006", "Your New %CLIENT% Number is : %1%", "", true, Client.ClientNo), null, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                base.custom_Finished(sender, e);
                return;
            }

            // Check the resource String CREATEFILE2 is blank if blank don't prompt user.
            var createFileMessage = Session.CurrentSession.Resources.GetMessage("CREATEFILE2", "Your New %CLIENT% Number is : %1% " + Environment.NewLine + Environment.NewLine + "Would you like to create a %FILE% for this %CLIENT%?", "", true, Client.ClientNo);
            if (wizardStyle == WizardStyle.Dialog && !string.IsNullOrWhiteSpace(createFileMessage.Text) &&
                MessageBox.Show(this, createFileMessage, null, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                _viewClient = false;
                Services.Wizards.CreateFile(Client, this, Modal);
                _view = null;
            }

            if (_view != null)
                _viewClient = Common.ConvertDef.ToBoolean(_view.Value, false);

            base.custom_Finished(sender, e);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the contact returned from the wizard.
        /// </summary>
        public Client Client
        {
            get
            {
                return _obj;
            }
        }

        /// <summary>
        /// Gets the view client property.
        /// </summary>
        public bool ViewClient
        {
            get
            {
                return _viewClient;
            }
        }

        #endregion
    }
}