using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.Security.Permissions;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A custom advanced enquiry control that enables the assigning and creation of
    /// telephone numbers.
    /// </summary>
    public class eNumberSelector: eSelectorBase
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
		/// The email / number returned.
		/// </summary>
		private string _number = "";

		/// <summary>
		/// The first type of number.
		/// </summary>
		private string _numberType = "TELEPHONE";

		/// <summary>
		/// The second type of number.
		/// </summary>
		private string _location = "";

        private bool isAssociate = false;

		#endregion
		
		#region Constructors

		/// <summary>
		/// Creates the address control, and if logged into the system retrieves the list 
		/// of countries from the databse to display in the countries combo box.
		/// </summary>
		public eNumberSelector() : base()
		{
			InitializeComponent();
            this.ParentChanged += new EventHandler(eNumberSelector_ParentChanged);
		}

        EnquiryForm enq;
        Associate ass;

        private bool assContactUpdated = false;
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
                enq.Enquiry.Refreshed -= new EventHandler(Enquiry_Refreshed);
                enq = null;
                if (ass != null)
                {
                    if (assContactUpdated && ass.Contact != null)
                        ass.Contact.Updated -= new EventHandler(Enquiry_Refreshed);
                    assContactUpdated = false;
                }
            }

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
                    switch (_numberType)
                    {
                        case "TELEPHONE":
                            _cont.Refresh(); //hopefully this will trigger the refresh well enough to handle the other objects
                            RefreshControl(_cont, _numberType, _location, assoc.DefaultTelephoneNumber, true);
                            Value = assoc.DefaultTelephoneNumber;
                            break;
                        case "FAX":
                            RefreshControl(_cont, _numberType, _location, assoc.DefaultFaxNumber, true);
                            Value = assoc.DefaultFaxNumber;
                            break;
                        case "MOBILE":
                            RefreshControl(_cont, _numberType, _location, assoc.DefaultMobile, true);
                            Value = assoc.DefaultMobile;
                            break;
                        default:
                            RefreshControl(_cont, _numberType, _location, null, true);
                            break;
                    }
                    _disabledirty = false;
                    return;
                }
                else if (efo.Enquiry.Object is Contact)
                    _cont = efo.Enquiry.Object as Contact;
                
            }

            _disabledirty = true;
            RefreshControl(_cont, _numberType, _location, _number, true);
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
				if (_number == String.Empty)
					return DBNull.Value;
				else
					return _number;
			}
			set
			{
                try
                {
                    if (_cont == null)
                        Initialise(Convert.ToString(value));
                    else
                    {
                        Combo.SelectedValue = value;
                        _number = Convert.ToString(value);
                    }
                }
                catch
                {
                }
			}
		}

		#endregion

		#region Captured Events

		protected override void Info_Changed(object sender, System.EventArgs e)
		{
            StoreControl(_cont, _numberType, _location, Convert.ToString(Combo.SelectedValue));
            OnActiveChanged();
            OnChanged();
		}

		private void eNumberelector_Load(object sender, System.EventArgs e)
		{
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
                        OMSDocument doc = efo.Enquiry.Object as OMSDocument;
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
                { }
            }

            if (cont == null && contid != 0)
            {
                cont = Contact.GetContact(contid);
            }

            string defnumber = value;

            if (cont != null)
            {
                if (String.IsNullOrEmpty(value) && assoc == null) defnumber = cont.GetNumber(_numberType, _location, 0);
                _disabledirty = true;
                this.RefreshControl(cont, _numberType, _location, defnumber);
                _disabledirty = false;
            }

            UIUpdate();
        }

		protected override void Edit(object sender, LinkLabelLinkClickedEventArgs e)
		{
            try
            {
                CheckPermissions();
                DataRowView row = cboInfoSelector.SelectedItem as DataRowView;
                FWBS.Common.KeyValueCollection param = new FWBS.Common.KeyValueCollection();
                param.Add("TYPE", row["contCode"]);
                param.Add("LOCATION", row["contExtraCode"]);
                param.Add("NUMBER", row["contNumber"]);
                DataTable data = FWBS.OMS.UI.Windows.Services.ShowOMSItem(this.ParentForm, Session.CurrentSession.DefaultSystemForm(SystemForms.TelephoneNumberEdit), null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, false, param) as DataTable;
                if (data != null)
                {
                    _cont.ChangeNumber(Convert.ToString(row["contNumber"]), Convert.ToString(row["contCode"]), Convert.ToString(row["contExtraCode"]), Convert.ToString(data.Rows[0]["_number"]), Convert.ToString(data.Rows[0]["_type"]), Convert.ToString(data.Rows[0]["_location"]));
                    this.RefreshControl(_cont, _numberType, _location, Convert.ToString(data.Rows[0]["_number"]), true);
                    this.Value = Convert.ToString(data.Rows[0]["_number"]);
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
		/// Adds an telephone number to the contact.
		/// </summary>
		/// <param name="sender">The add button.</param>
		/// <param name="e">Empty event arguments.</param>
		protected override void Add(object sender, LinkLabelLinkClickedEventArgs e)
		{
            try
            {
                CheckPermissions();
                ContactNumber num = FWBS.OMS.UI.Windows.Services.Wizards.CreateNumber(this.ParentForm, _numberType, _location);
                if (num != null)
                {
                    if (num.Location == string.Empty)
                    {
                        _cont.AddDefaultNumber(num.Number, num.Type, num.Location);
                    }
                    else
                    {
                        _cont.AddNumber(num.Number, num.Type, num.Location);
                    }
                    this.RefreshControl(_cont, _numberType, _location, num.Number, true);
                    _number = num.Number;
                    this.OnActiveChanged();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

		/// <summary>
		/// Removes a number from the contacts list.
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
                    result = MessageBox.ShowYesNoQuestion("AREYOUSUREASS", "Are you sure you want to Remove this %1% from the Associate?", "number");
                else if (_cont != null)
                    result = MessageBox.ShowYesNoQuestion("AREYOUSURE", "Are you sure you wish to Delete?");
                
                if (Combo.SelectedValue != null && result == DialogResult.Yes)
				{
                    //DMB 4/2/2004 _location does not appear to be tracked so added code to capture
					DataRowView row = cboInfoSelector.SelectedItem as DataRowView;
					string type = Convert.ToString(row["contCode"]);
					string location = Convert.ToString(row["contExtraCode"]);
					string number = Convert.ToString(row["contNumber"]);

					if (String.IsNullOrEmpty(number) == false)
					{
                        if (isAssociate)
                        {
                            Combo.SelectedIndex = -1;
                            Combo.SelectedIndex = -1;
                            number = String.Empty;
                            _number = String.Empty;
                        }
                        else if (_cont.RemoveNumber(type, location, number))
						{
                            Reload(_cont);
                            RefreshControl(_cont, _numberType, _location, "", true);
                            if (_cont.GetNumbers(type).Count == 0)
                                Combo.SelectedValue = String.Empty;
                            else
                                Combo.SelectedIndex = 0;

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
		[DefaultValue("TELEPHONE")]
		public string NumberType
		{
			get
			{
				return _numberType;
			}
			set
			{
				_numberType = value;
			}
		}

		/// <summary>
		/// Gets or Sets the address object as the return value of the control.
		/// </summary>
		[Browsable(true)]
		[DefaultValue("HOME")]
		public string NumberLocation
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
				this.RefreshControl(contact , _numberType, _location, contact.GetNumber(_numberType, _location, 0));
			UIUpdate();
		}

		private void InitializeComponent()
		{
            this.pnlContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboInfoSelector
            // 
            this.cboInfoSelector.Size = new System.Drawing.Size(126, 21);
            // 
            // eNumberSelector
            // 
            this.FindLinkVisible = false;
            this.Name = "eNumberSelector";
            this.Load += new System.EventHandler(this.eNumberelector_Load);
            this.Leave += new System.EventHandler(this.eNumberSelector_Leave);
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		public void Reload(long contid)
		{
			Contact cont = Contact.GetContact(contid);
			this.RefreshControl(cont , _numberType, _location,  cont.GetNumber(_numberType, _location, 0));
			UIUpdate();
		}

		public void Reload(long contid, string number)
		{
			Contact cont = Contact.GetContact(contid);
			if (_cont == null) _cont = cont;
			if (_number == number) return;
			this.RefreshControl(cont , _numberType, _location, number);
			UIUpdate();
		}
		public void Reload(Contact contact, string number)
		{
			if (_cont == null) _cont = contact;
			if (_number == number) return;
			this.RefreshControl(contact, _numberType, _location, number);
			UIUpdate();
		}
	
		private void StoreControl(Contact contact, string numberType, string location, string number)
		{
			try
			{
				//Fetch the contact if the contact object is null or the contact id has changed.
				if (contact != null)
				{	
					if (_cont == null || contact.ID != _cont.ID || _number != number)
					{
						_cont = contact;
						_number = number;
		
						if (!_cont.HasNumber(_number, _numberType, _location))
							_number = _cont.GetNumber(_numberType, _location, 0);
					}
				}
			
			}
			catch (Exception ex)
			{
                throw new OMSException2("ERRCONTIDISNULL", "CONTID is null in creating Contact Record", ex);
            }
		}

        private void RefreshControl(Contact contact, string numberType, string location, string number)
        {
            RefreshControl(contact, numberType, location, number, false);
        }

		/// <summary>
		/// Refreshes the controls data.
		/// </summary>
		private void RefreshControl(Contact contact, string numberType, string location, string number, bool force)
		{
			try
			{
				//Fetch the contact if the contact object is null or the contact id has changed.
				if (contact != null) 
				{	
					if (_cont == null || contact.ID != _cont.ID || _number != number || force)
					{
						_cont = contact;
						_number = number;
						Combo.BeginUpdate();

                        DataTable dt = _cont.GetNumbers(numberType, location).ToTable();
                        Combo.DataSource = dt;
						if (_location == "")
							Combo.DisplayMember = "DisplayNumber";
						else
							Combo.DisplayMember = "contNumber";
						Combo.ValueMember = "contNumber";
		
                        if (_number != null)
                            Combo.SelectedValue = _number;
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


		private void eNumberSelector_Leave(object sender, System.EventArgs e)
		{
			OnChanged();
		}

		#endregion

	}
}
