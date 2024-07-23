using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.Data;
using FWBS.OMS.Security.Permissions;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A custom advanced enquiry control that enables the assigning and creation of
    /// email addresses.
    /// </summary>
    public class eEmailSelector: eSelectorBase
	{

		#region Fields

		/// <summary>
		/// If True then do not Change Dirty
		/// </summary>
		private bool _disabledirty = false;
		/// <summary>
		/// This is the Contact Object
		/// </summary>
		private Contact _cont = null;

		/// <summary>
		/// The email returned.
		/// </summary>
		private string _email = "";

		/// <summary>
		/// The location of the email.
		/// </summary>
		private string _location = "HOME";

        private bool isAssociate = false;

		#endregion

		#region Constructors

		/// <summary>
		/// Creates the address control, and if logged into the system retrieves the list 
		/// of countries from the databse to display in the countries combo box.
		/// </summary>
		public eEmailSelector() : base()
		{
			InitializeComponent();
            this.ParentChanged += new EventHandler(eNumberSelector_ParentChanged);
        }

        EnquiryForm enq;
        Associate ass;
        bool assContactUpdated;

        void eNumberSelector_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent is EnquiryForm)
            {
                enq = this.Parent as EnquiryForm;
                enq.Enquiry.Refreshed -= new EventHandler(Enquiry_Refreshed);
                enq.Enquiry.Refreshed += new EventHandler(Enquiry_Refreshed);
                if (enq.Enquiry != null)
                    if (enq.Enquiry.Object is Associate)
                    {
                        isAssociate = true;
                        ass = enq.Enquiry.Object as Associate;
                        if (ass.Contact != null)
                        {
                            assContactUpdated = true;
                            ass.Contact.Updated -= new EventHandler(Enquiry_Refreshed);
                            ass.Contact.Updated += new EventHandler(Enquiry_Refreshed);
                        }
                    }
            }
            else if (this.Parent == null && enq != null)
            {
                if (ass != null)
                {
                    if (assContactUpdated && ass.Contact != null)
                        ass.Contact.Updated -= new EventHandler(Enquiry_Refreshed);
                    assContactUpdated = false;
                }
                enq.Enquiry.Refreshed -= new EventHandler(Enquiry_Refreshed);
                enq = null;
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (enq != null)
                {
                    enq.Enquiry.Refreshed -= new EventHandler(Enquiry_Refreshed);
                    enq = null;
                }
            }
            base.Dispose(disposing);
        }

        void Enquiry_Refreshed(object sender, EventArgs e)
        {
            if (this.Parent is EnquiryForm)
            {
                EnquiryForm efo = (EnquiryForm)this.Parent;
                if (efo.Enquiry.InDesignMode)
                {
                    return;
                }

                if (efo.Enquiry.Object is Associate)
                {
                    Associate assoc = efo.Enquiry.Object as Associate;
                    _cont = assoc.Contact;
                    _disabledirty = true;
                    _cont.Refresh(); //hopefully this will trigger the refresh well enough to handle the other objects
                    RefreshControl(_cont, _location, _email,true);
                    Value = assoc.DefaultEmail;
                    _disabledirty = false;
                    return;
                }
                else if (efo.Enquiry.Object is Contact)
                    _cont = efo.Enquiry.Object as Contact;

            }

            _disabledirty = true;
            RefreshControl(_cont, _location, _email);
            _disabledirty = false;

        }
	
		#endregion

		#region IBasicEnquiryControl2 Implementation

		/// <summary>
		/// Gets or Sets the controls value.  This must be overriden by derived classes to make their
		/// own representation of the value using the internal editing control..
		/// </summary>
		[Browsable(false)]
		public override object Value
		{
			get
			{
				if (_email == String.Empty)
					return DBNull.Value;
				else
					return _email;
			}
			set
			{
				try
				{
                    if (_cont == null)
                        Initialise(Convert.ToString(value));
                    else
                        Combo.SelectedValue = value;

				}
				catch
				{
				}
			}
		}

		#endregion

		#region Captured Events

		protected override void Info_Changed (object sender, System.EventArgs e)
		{
			StoreControl(_cont, _location, Convert.ToString(Combo.SelectedValue));
			this.OnActiveChanged();
            this.OnChanged();
		}

		private void Initialise(string value)
		{
			Contact cont = null;
			Associate assoc = null;
			long contid = 0;

			//Try and find the contact id from the enquiry form that the control is sitting on.
			if (this.Parent is EnquiryForm)
			{
				
				// if not business mapped try and get the CONTID field through IEnquiryCompatible interface getextrainfo
				// else try and get from Source.
				try
				{
					EnquiryForm efo = (EnquiryForm)this.Parent;
					if (efo.Enquiry.InDesignMode)
					{
						return;
					}


					if (efo.Enquiry.Object is Contact)
					{
						cont = efo.Enquiry.Object as Contact;
					}

					if (efo.Enquiry.Object is OMSDocument)
					{
						OMSDocument doc= efo.Enquiry.Object as OMSDocument;
						assoc = doc.Associate;
						cont = assoc.Contact;
					}

					if (efo.Enquiry.Object is Associate)
					{
						assoc = efo.Enquiry.Object as Associate;
						cont = assoc.Contact;
					}

					if (cont == null)
					{
						if (efo.Enquiry.Object is FWBS.OMS.Interfaces.IEnquiryCompatible)
						{
							try
							{
								contid = (long)((FWBS.OMS.Interfaces.IEnquiryCompatible)efo.Enquiry.Object).GetExtraInfo("CONTID");
							}
							catch 
							{
								if (contid == 0)
								{
									try
									{
										contid = (long)((FWBS.OMS.Interfaces.IEnquiryCompatible)efo.Enquiry.Object).GetExtraInfo("CLDefaultContact");
									}
									catch
									{
										contid = (long)((FWBS.OMS.Interfaces.IEnquiryCompatible)efo.Enquiry.Object).GetExtraInfo("ContDefaultContact");
									}
								}
							}
						}
						else
						{
							contid = (long)efo.Enquiry.Source.Tables["DATA"].Rows[0]["CONTID"];
						}
					}
				}
				catch
				{}
			}

			if (contid != 0)
			{
				cont = Contact.GetContact(contid);
			}

            string defemail = value;

            if (cont != null)
            {
                if (String.IsNullOrEmpty(value) && assoc == null) defemail = cont.GetEmail(_location, 0);
                _disabledirty = true;
                this.RefreshControl(cont, _location, defemail);
                _disabledirty = false;
            }

		}

		protected override void Edit(object sender, LinkLabelLinkClickedEventArgs e)
		{
            try
            {
                CheckPermissions();
                DataRowView row = cboInfoSelector.SelectedItem as DataRowView;
                FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
                param.Add("LOCATION", row["contCode"]);
                param.Add("EMAIL", row["contEmail"]);
                DataTable data = FWBS.OMS.UI.Windows.Services.ShowOMSItem(this.ParentForm, Session.CurrentSession.DefaultSystemForm(SystemForms.EmailEdit), null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, false, param) as DataTable;
                if (data != null)
                {
                    _cont.ChangeEmail(Convert.ToString(row.Row["contEmail"]), Convert.ToString(row.Row["contCode"]), Convert.ToString(data.Rows[0]["_email"]), Convert.ToString(data.Rows[0]["_location"]));
                    if (ExecuteAssociateUpdateProcess(Convert.ToString(param["EMAIL"].Value), Convert.ToString(param["LOCATION"].Value), Convert.ToString(data.Rows[0]["_email"]), Convert.ToString(data.Rows[0]["_location"])))
                        _cont.Update();
                    this.RefreshControl(_cont, _location, Convert.ToString(data.Rows[0]["_email"]), true);
                    this.Value = Convert.ToString(data.Rows[0]["_email"]);
                    this.OnActiveChanged();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

        private void CheckPermissions()
        {
            if (_cont.IsNew)
                return;

            new ContactPermission(_cont, StandardPermissionType.Update).Check();
            new SystemPermission(StandardPermissionType.UpdateContact).Check();
        }

        /// <summary>
        /// initiate the Associate process - criteria checks and, if appropriate, Associate update(s)
        /// </summary>
        /// <param name="originalAddress"></param>
        /// <param name="originalLocation"></param>
        /// <param name="newAddress"></param>
        /// <param name="newLocation"></param>
        /// <returns>True if associate email was updated, False otherwise</returns>
        private bool ExecuteAssociateUpdateProcess(string originalAddress, string originalLocation, string newAddress, string newLocation)
        {
            if (HasEmailBeenEdited(originalAddress, newAddress))
            {
                if (CheckIfParentIsAContact())
                {
                    if (CheckForAssociateEmailLinks(originalAddress))
                    {
                        return ActivateEmailAddressResolver(originalAddress, newAddress);
                    }
                }
            }
            return false;
        }


        private bool HasEmailBeenEdited(string originalAddress, string newAddress)
        {
            return (originalAddress.Trim() != newAddress.Trim());
        }


        private bool CheckIfParentIsAContact()
        {
            EnquiryForm efo = (EnquiryForm)this.Parent;
            return (efo.Enquiry.Object is Contact);
        }


        private bool CheckForAssociateEmailLinks(string originalAddress)
        {
            SearchEngine.SearchList _links = _cont.LinkedEmailAddress(originalAddress, "LUpdAssocEmail");
            return (_links.ResultCount > 0);
        }


        private bool ActivateEmailAddressResolver(string originalAddress, string newAddress)
        {
            string result = FWBS.OMS.UI.Windows.Services.Wizards.MultiLinkedEmailAddressResolver(originalAddress, _cont);
            return !string.IsNullOrWhiteSpace(result) &&
                UpdateLinkedAssociateEmailAddresses(newAddress, result);
        }


        private bool UpdateLinkedAssociateEmailAddresses(string emailAddress, string assocIDs)
        {
            try
            {
                IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                List<IDataParameter> parList = new List<IDataParameter>();
                parList.Add(connection.CreateParameter("emailAddress", emailAddress.Trim()));
                parList.Add(connection.CreateParameter("assocIDs", assocIDs));
                System.Data.DataTable dt = connection.ExecuteProcedure("sprUpdateAssociateEmailAddresses", parList);
                return true;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
            return false;
        }


		/// <summary>
		/// Adds an email to the contact.
		/// </summary>
		/// <param name="sender">The add button.</param>
		/// <param name="e">Empty event arguments.</param>
		protected override void Add (object sender, LinkLabelLinkClickedEventArgs e)
		{
            try
            {
                CheckPermissions();
                ContactEmail email = FWBS.OMS.UI.Windows.Services.Wizards.CreateEmail(_location);
                if (email != null)
                {

                    if (email.Location == string.Empty)
                    {
                        _cont.AddDefaultEmail(email.Email, email.Location);
                    }
                    else
                    {
                        _cont.AddEmail(email.Email, email.Location);
                    }

                    this.RefreshControl(_cont, _location, email.Email, true);
                    _email = email.Email;
                    this.OnActiveChanged();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

		/// <summary>
		/// Removes a email from the contacts list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void Remove(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
                CheckPermissions();
                DialogResult result = DialogResult.Yes;
                if (ass != null)
                    result = MessageBox.ShowYesNoQuestion("AREYOUSUREASS", "Are you sure you want to Remove this %1% from the Associate?", "email");
                else if (_cont != null)
                    result = MessageBox.ShowYesNoQuestion("AREYOUSURE", "Are you sure you wish to Delete?", "");
                
                if (Combo.SelectedValue != null && result == DialogResult.Yes)
				{
                    //obtain the currently selected location
					DataRowView row = cboInfoSelector.SelectedItem as DataRowView;
					string location = Convert.ToString(row["contCode"]);
					string email = Convert.ToString(row["contEmail"]);
					
					_email = "";
					if (email != "")
					{
                        if (isAssociate)
                        {
                            Combo.SelectedIndex = -1;
                            Combo.SelectedIndex = -1;
                            _email = String.Empty;
                            email = String.Empty;
                        }
                        else
                        {
                            if (_cont.RemoveEmail(email, location))
                            {
                                Reload(_cont);
                                RefreshControl(_cont, _location, "", true);
                                if (_cont.GetEmails(location).Count == 0)
                                    Combo.SelectedValue = String.Empty;
                                else
                                    Combo.SelectedIndex = 0;
                            }
                        }
					}
                    this.OnActiveChanged();
                    this.OnChanged();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex);
			}
		}


		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the address object as the return value of the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue("HOME")]
		public string EmailLocation
		{
			get
			{
				return _location;
			}
			set
			{
				_location = value;
			}
		}

		#endregion

		#region Methods

		public void Reload(Contact contact)
		{
			if (_cont == null || contact.ID != _cont.ID)
				this.RefreshControl(contact , _location, contact.GetEmail(_location, 0));
			base.UIUpdate();
		}

		private void InitializeComponent()
		{
            this.pnlContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboInfoSelector
            // 
            this.cboInfoSelector.Size = new System.Drawing.Size(0, 21);
            // 
            // eEmailSelector
            // 
            this.FindLinkVisible = false;
            this.Name = "eEmailSelector";
            this.Leave += new System.EventHandler(this.eEmailSelector_Leave);
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		public void Reload(long contid)
		{
			Contact cont = Contact.GetContact(contid);
			this.RefreshControl(cont , _location,  cont.GetEmail(_location, 0));
		}

		public void Reload(long contid, string email)
		{
			Contact cont = Contact.GetContact(contid);
			if (_cont == null) _cont = cont;
			if (_email == email) return;
			this.RefreshControl(cont , _location, email);
		}
		public void Reload(Contact contact, string email)
		{
			if (_cont == null) _cont = contact;
			if (_email == email) return;
			this.RefreshControl(contact, _location, email);
		}

        private void RefreshControl(Contact contact, string location, string email)
        {
            RefreshControl(contact, location, email, false);
        }

        /// <summary>
		/// Refreshes the controls data.
		/// </summary>
		private void RefreshControl(Contact contact, string location, string email, bool force)
		{
			try
			{
				//Fetch the contact if the contact object is null or the contact id has changed.
				if (contact != null)
				{
                    if (_cont == null || contact.ID != _cont.ID || _email != email || _location != location || force == true)
					{
						_cont = contact;
						_email = email;

						Combo.BeginUpdate();
						Combo.DataSource = _cont.GetEmails(location).ToTable();
						if (_location == "")
							Combo.DisplayMember = "DisplayEmail";
						else
							Combo.DisplayMember = "contEmail";
						Combo.ValueMember = "contEmail";

                        if (_email != null)
                            Combo.SelectedValue = _email;
                        else
                            Combo.SelectedIndex = -1;
						Combo.EndUpdate();

						if (!_disabledirty)
							this.OnActiveChanged();
					}
				}
			
			}
			catch (Exception ex)
			{
                throw new OMSException2("ERRCONTIDISNULL", "CONTID is null in creating Contact Record", ex);
			}
			
			
		}


		/// <summary>
		/// Stores the controls data.
		/// </summary>
		private void StoreControl(Contact contact, string location, string email)
		{
			try
			{
				//Fetch the contact if the contact object is null or the contact id has changed.
				if (contact != null)
				{	
					if (_cont == null || contact.ID != _cont.ID || _email != email)
					{
						_cont = contact;
						_email = email;
	
		
						if (!_cont.HasEmail(_email, _location))
							_email = _cont.GetEmail(_location, 0);
					}
				}
			
			}
			catch (Exception ex)
			{
                throw new OMSException2("ERRCONTIDISNULL", "CONTID is null in creating Contact Record", ex);
            }
			
			
		}

		
		private void eEmailSelector_Leave(object sender, System.EventArgs e)
		{
			this.OnChanged();
		}


		#endregion
	}
}
