using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.Security.Permissions;


namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A custom advanced enquiry control that enables the assigning and creation of
    /// address for specified entities within the system.
    /// </summary>
    public class eAddressSelector : eSelectorBase
	{
		#region Fields
		/// <summary>
		/// This is the current address object.
		/// </summary>
		private Address _currentAddress = null;
		
		/// <summary>
		/// This is the Contact Object
		/// </summary>
		private Contact _cont = null;

		/// <summary>
		/// Enquiryform parent object.
		/// </summary>
		private object _obj = null;

		/// <summary>
		/// Uses the current address object itself for the control value.  Otherwise the
		/// address id is used.
		/// </summary>
		private bool _useAddressObject = false;

		/// <summary>
		/// This flag is used to include a default at the top of the address list.
		/// </summary>
		private bool _includeDefault = true;

        /// <summary>
        /// Enquiryform to clear when removed
        /// </summary>
        private EnquiryForm efo = null;
		#endregion

		#region Controls Specific Controls
		private System.Windows.Forms.Label lbladdress;
		private System.Windows.Forms.Panel pnl;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Creates the address control, and if logged into the system retrieves the list 
		/// of countries from the database to display in the countries combo box.
		/// </summary>
		public eAddressSelector() : base()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
            this.pnl = new System.Windows.Forms.Panel();
            this.lbladdress = new System.Windows.Forms.Label();
            this.pnlContainer.SuspendLayout();
            this.pnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboInfoSelector
            // 
            this.cboInfoSelector.Size = new System.Drawing.Size(0, 21);
            // 
            // pnl
            // 
            this.pnl.Controls.Add(this.lbladdress);
            this.pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl.Location = new System.Drawing.Point(150, 23);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(150, 49);
            this.pnl.TabIndex = 0;
            // 
            // lbladdress
            // 
            this.lbladdress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbladdress.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lbladdress.Location = new System.Drawing.Point(0, 0);
            this.lbladdress.Name = "lbladdress";
            this.lbladdress.Size = new System.Drawing.Size(150, 49);
            this.lbladdress.TabIndex = 2;
            this.lbladdress.UseMnemonic = false;
            // 
            // eAddressSelector
            // 
            this.Controls.Add(this.pnl);
            this.FindLinkVisible = false;
            this.Name = "eAddressSelector";
            this.Size = new System.Drawing.Size(300, 72);
            this.Leave += new System.EventHandler(this.eAddressSelector_Leave);
            this.ParentChanged += new System.EventHandler(this.eAddressSelector_ParentChanged);
            this.Controls.SetChildIndex(this.pnlContainer, 0);
            this.Controls.SetChildIndex(this.pnl, 0);
            this.pnlContainer.ResumeLayout(false);
            this.pnlContainer.PerformLayout();
            this.pnl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region IBasicEnquiryControl2 Implementation
		/// <summary>
		/// Gets or Sets the caption width of a control, leaving the rest of the width of the control
		/// to be the width of the internal editing control.
		/// </summary>
		[DefaultValue(150)]
		[Category("Layout")]
		public override int CaptionWidth
		{
			get
			{
				return base.CaptionWidth;
			}
			set
			{
				base.CaptionWidth = value;
			}
		}

		/// <summary>
		/// Gets or Sets the controls value. This must be overriden by derived classes to make their
		/// own representation of the value using the internal editing control..
		/// </summary>
		[Browsable(false)]
		public override object Value
		{
			get
			{
				try
				{
                    if (_useAddressObject)
                        return _currentAddress;
                    else
                    {
                        if (_currentAddress != null)
                            return _currentAddress.ID;
                        else
                            return DBNull.Value;
                    }
				}
				catch
				{
					return DBNull.Value;
				}
			}
			set
			{
                bool changed = false;

                if (value is Address)
                {
                    if (_currentAddress == null)
                        changed = true;
                    else
                        changed = _currentAddress.ID != ((Address)value).ID;

                    _currentAddress = value as Address;
                }
                else if (value != null)
                {
                    if (_currentAddress == null)
                        changed = true;
                    else
                        changed = _currentAddress.ID != Convert.ToInt64(value);

                    _currentAddress = Address.GetAddress(Convert.ToInt64(value));
                }

				try
				{
					if (_cont != null)
					{
						if (value is DBNull || value == null)
						{
						}
						else
						{
							if (_useAddressObject)
							{
								Address add = (Address)value;
								if (add.ID != Convert.ToInt64(Combo.SelectedValue))
								{
									RefreshControl(_cont, add, changed);
									if (_cont.HasAddress(add))
										Combo.SelectedValue = add.ID;
								}
							}
							else
							{
								if (Convert.ToInt64(value) != Convert.ToInt64(Combo.SelectedValue))
								{
									Address add =  Address.GetAddress(Convert.ToInt64(value));
									RefreshControl(_cont, add, changed);
									if (_cont.HasAddress(add))
										Combo.SelectedValue = value;

								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					ErrorBox.Show(ParentForm, ex);
				}
			}
		}
		#endregion

		#region Captured Events
        private void CheckPermissions()
        {
            var cont = _obj as Contact;
            if (cont != null)
            {
                new ContactPermission(cont, StandardPermissionType.Update).Check();
                new SystemPermission(StandardPermissionType.UpdateContact).Check();
            }

            var client = _obj as Client;
            if (client != null)
            {
                new ClientPermission(client, StandardPermissionType.Update).Check();
                new SystemPermission(StandardPermissionType.UpdateClient).Check();
            }
        }

		protected override void Edit(object sender, LinkLabelLinkClickedEventArgs e)
		{
            try
            {
                CheckPermissions();

                if (_obj is FWBS.OMS.Client)
                {
                    // MNW Remove ability to support Add/Edit/Delete
                    MessageBox.ShowInformation("CLEDITADDNOT", "You Can't Add/Edit/Del a Default Address from the %CLIENT%", null);
                    return;
                }
                
                Address a = null;
                if (Convert.ToInt64(Combo.SelectedValue) == 0)
                {
                    a = Address.GetAddress(_cont.DefaultAddress.ID);
                    a.AddType = "";
                }
                else
                {
                    a = Address.GetAddress(Convert.ToInt64(Combo.SelectedValue));
                    a.AddType = Convert.ToString(((DataRowView)Combo.SelectedItem)["contCode"]);
                }
                FWBS.OMS.Address adobj = FWBS.OMS.UI.Windows.Services.Wizards.CreateAddress(a, true);
                if (adobj != null)
                {
                    bool edit = true;
                    if (a.Line1 != "" && a.Line1 != adobj.Line1) edit = false;
                    if (a.Line2 != "" && a.Line2 != adobj.Line2) edit = false;
                    if (a.Line3 != "" && a.Line3 != adobj.Line3) edit = false;
                    if (a.Line4 != "" && a.Line4 != adobj.Line4) edit = false;
                    if (a.Line5 != "" && a.Line5 != adobj.Line5) edit = false;
                    if (a.Postcode != "" && a.Postcode != adobj.Postcode) edit = false;
                    if (edit)
                    {
                        a.Line1 = adobj.Line1;
                        a.Line2 = adobj.Line2;
                        a.Line3 = adobj.Line3;
                        a.Line4 = adobj.Line4;
                        a.Line5 = adobj.Line5;
                        a.Postcode = adobj.Postcode;
                        a.CountryID = adobj.CountryID;
                        if (a.AddType != adobj.AddType && Convert.ToInt64(Combo.SelectedValue) != 0)
                        {
                            _cont.ChangeAddressType(a.ID, adobj.AddType);

                        }
                        a.Update();

                        this.RefreshControl(_cont, a, true);

                        this.OnActiveChanged(); //sets the dirty flag
                        this.OnChanged(); //updates business layer
                        if (this.Parent is EnquiryForm)
                            ((EnquiryForm)this.Parent).IsDirty = true;
                        Info_Changed(this, EventArgs.Empty);
                    }
                    else
                    {
                        SearchEngine.SearchList _links = _cont.LinkedAddress(a, "SCHCONADD");
                        if (_links.ResultCount > 0)
                        {
                            string result = FWBS.OMS.UI.Windows.Services.Wizards.MultiLinkedAddressResolver(a, _cont);
                            if (result == "CANCEL")
                                return;
                            else if (result == "MODIFYONE")
                            {
                                //add the new address
                                adobj.Update();
                                //add to the contacts addresses
                                _cont.AddAddress(adobj);
                                _cont.RemoveAddress(a);
                                this.RefreshControl(_cont, adobj);
                                this.OnActiveChanged();

                                this.OnChanged(); //updates business layer
                                if (this.Parent is EnquiryForm)
                                    ((EnquiryForm)this.Parent).IsDirty = true;
                                Info_Changed(this, EventArgs.Empty);
                            }
                            else
                            {
                                a.Line1 = adobj.Line1;
                                a.Line2 = adobj.Line2;
                                a.Line3 = adobj.Line3;
                                a.Line4 = adobj.Line4;
                                a.Line5 = adobj.Line5;
                                a.Postcode = adobj.Postcode;
                                a.CountryID = adobj.CountryID;
                                if (a.AddType != adobj.AddType && Convert.ToInt64(Combo.SelectedValue) != 0)
                                {
                                    _cont.ChangeAddressType(a.ID, adobj.AddType);

                                }
                                a.Update();

                                this.RefreshControl(_cont, a, true);

                                this.OnActiveChanged(); //sets the dirty flag
                                this.OnChanged(); //updates business layer
                                if (this.Parent is EnquiryForm)
                                    ((EnquiryForm)this.Parent).IsDirty = true;
                                Info_Changed(this, EventArgs.Empty);
                            }
                        }
                        else
                        {
                            a.Line1 = adobj.Line1;
                            a.Line2 = adobj.Line2;
                            a.Line3 = adobj.Line3;
                            a.Line4 = adobj.Line4;
                            a.Line5 = adobj.Line5;
                            a.Postcode = adobj.Postcode;
                            a.CountryID = adobj.CountryID;
                            if (a.AddType != adobj.AddType && Convert.ToInt64(Combo.SelectedValue) != 0)
                            {
                                _cont.ChangeAddressType(a.ID, adobj.AddType);

                            }
                            a.Update();

                            this.RefreshControl(_cont, a, true);
                            this.OnActiveChanged(); //sets the dirty flag
                            this.OnChanged(); //updates business layer
                            if (this.Parent is EnquiryForm)
                                ((EnquiryForm)this.Parent).IsDirty = true;
                            Info_Changed(this, EventArgs.Empty);
                        }
                    }
                    DisplayAddress();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

        protected override int PreferredHeight()
        {
            return base.PreferredHeight() + pnl.Height;
        }

		/// <summary>
		/// Captures the combo box change event.
		/// </summary>
		/// <param name="sender">The address selector combo box control.</param>
		/// <param name="e">Empty event arguments.</param>
		protected override void Info_Changed(object sender, System.EventArgs e)
		{
			if (Combo.SelectedValue == null) return;
            Reload(_cont, Convert.ToInt64(Combo.SelectedValue));
            this.OnActiveChanged();
            this.OnChanged();
			DisplayAddress();
            this.EditLinkEnabled = Convert.ToInt64(Combo.SelectedValue) != 0;
        }
		
		/// <summary>
		/// Adds an address to the contact.
		/// </summary>
		/// <param name="sender">The add button.</param>
		/// <param name="e">Empty event arguments.</param>
		protected override void Add (object sender, LinkLabelLinkClickedEventArgs e)
		{
            try
            {
                CheckPermissions();

                if (_obj is FWBS.OMS.Client)
                {
                    // MNW Remove ability to support Add/Edit/Delete
                    MessageBox.ShowInformation("CLEDITADDNOT", "You Can't Add/Edit/Del a Default Address from the %CLIENT%", null);
                    return;
                }

                FWBS.OMS.Address adobj = FWBS.OMS.UI.Windows.Services.Wizards.CreateAddress(true);
                if (adobj != null)
                {
                    _cont.AddAddress(adobj);
                    _cont.Update();
                    this.RefreshControl(_cont, adobj);
                    this.OnActiveChanged();
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }

		}

		/// <summary>
		/// Removes an address from the contacts address list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void Remove(object sender, LinkLabelLinkClickedEventArgs e)
		{
            try
            {
                CheckPermissions();

                if (_obj is FWBS.OMS.Client)
                {
                    // MNW Remove ability to support Add/Edit/Delete
                    MessageBox.ShowInformation("CLEDITADDNOT", "You Can't Add/Edit/Del a Default Address from the %CLIENT%", null);
                    return;
                }
                try
                {
                    if (Combo.SelectedValue != null)
                    {
                        Address add = Address.GetAddress(Convert.ToInt64(Combo.SelectedValue));

                        if (add != null)
                        {
                            var addressRow = Combo.SelectedItem as DataRowView;
                            if (addressRow != null)
                            {
                                var type = addressRow["contCode"] as string;
                                add.AddType = type;
                            }

                            if (_cont.RemoveAddress(add))
                            {
                                Reload(_cont, Convert.ToInt64(Combo.SelectedValue));
                            }
                            this.OnActiveChanged();
                            this.OnChanged();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Reload(_cont, Convert.ToInt64(Combo.SelectedValue));
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
		}

		/// <summary>
		/// Finds an existing address to add to the contact.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void Find(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				Address add = Services.Searches.FindAddress();
				if (add != null)
				{
					if (_cont.AddAddress(add))
					{
						RefreshControl(_cont, add);
					}
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
		[DefaultValue(false)]
		public bool UseValueAsAddress
		{
			get
			{
				return _useAddressObject;
			}
			set
			{
				_useAddressObject = value;
			}
		}

		/// <summary>
		/// Gets or Sets a flag that determines a default address entry which interprets a DBNull will be added.
		/// </summary>
		[DefaultValue(true)]
		public bool IncludeDefault
		{
			get
			{
				return _includeDefault;
			}
			set
			{
				_includeDefault = value;
			}
		}
		#endregion

		#region Methods
		public void Reload(Contact contact)
		{
			if (_currentAddress == null || _cont == null || contact.ID != _cont.ID)
				this.RefreshControl(contact , Address.Null);
		}

		public void Reload(Associate associate)
		{
			if (associate != null)
			{
				Contact contact = associate.Contact;
				_obj = associate;
				this.RefreshControl(contact , Address.Null);
			}
		}

		public void Reload(long contid)
		{
			Contact cont = Contact.GetContact(contid);
			this.RefreshControl(cont , Address.Null);
		}

		/// <summary>
		/// Used to Reload the ContID Field and Refresh
		/// </summary>
		public void Reload(long contid, long addid)
		{
			Contact cont = Contact.GetContact(contid);
			if (_currentAddress != null)
				if (_currentAddress.ID == addid) return;
			this.RefreshControl(cont , Address.GetAddress(addid));
		}

		/// <summary>
		/// Refreshes the contact and address id.
		/// </summary>
		/// <param name="contact">Contact object.</param>
		/// <param name="addid">Prefered address.</param>
		public void Reload(Contact contact, long addid)
		{
			if (_currentAddress != null)
				if (_currentAddress.ID == addid) return;

			this.RefreshControl(contact, Address.GetAddress(addid));
		}

		/// <summary>
		/// Refreshes the contact and address.
		/// </summary>
		/// <param name="contact">Contact object.</param>
		/// <param name="address">Prefered address.</param>
		public void Reload(Contact contact, Address address)
		{
			if (_currentAddress != null)
				if (_currentAddress.ID == address.ID) return;

			this.RefreshControl(contact, address);
		}
		
		/// <summary>
		/// Refreshes the controls data.
		/// </summary>
		/// <param name="contact">Contact object</param>
		/// <param name="address">Address object</param>
		private void RefreshControl(Contact contact, Address address)
		{
			this.RefreshControl(contact,address,false);
		}

		/// <summary>
		/// Refreshes the controls data. ocerload added DMB 13/2/2004
		/// </summary>
		/// <param name="contact">Contact Object</param>
		/// <param name="address">Address Object</param>
		/// <param name="force">Flag to force a refres</param>
		private void RefreshControl(Contact contact, Address address, bool force)
		{
			try
			{
				//Fetch the contact if the contact object is null or the contact id has changed.
				if (contact != null && address != null)
				{	
					if (_cont == null || contact.ID != _cont.ID || _currentAddress == null || _currentAddress.ID != address.ID || force == true)
					{
						_cont = contact;
						_currentAddress = address;
						Combo.BeginUpdate();
						DataView dv = _cont.GetAddresses(_includeDefault);
						//make sure it only displays active records
						if(dv.RowFilter =="")
							dv.RowFilter = "contactive = true";
						else
                            dv.RowFilter += " and contactive = true";

						Combo.DataSource = dv;
						Combo.DisplayMember = "ContTypeDesc";
						Combo.ValueMember = "contaddID";

						if (!_cont.HasAddress(_currentAddress))
							_currentAddress = _cont.DefaultAddress;

						Combo.SelectedValue = _currentAddress.ID;
						
						DisplayAddress();

						Combo.EndUpdate();

                        this.EditLinkEnabled = Convert.ToInt64(Combo.SelectedValue) != 0;
					}
				}
			}
			catch (Exception ex)
			{
				throw new OMSException2("ERRCONTIDISNULL", "CONTID is null in creating Contact Record", ex);
			}
		}

		/// <summary>
		/// Displays the address below the addres picker combo box.
		/// </summary>
		private void DisplayAddress()
		{
			if (_currentAddress != null)
			{
				if (_currentAddress == Address.Null)
				{
					if (_obj is Client)
					{
						lbladdress.Text = ((Client)_obj).DefaultContact.DefaultAddress.GetAddressString();
					}
					else if (_obj is Associate)
					{
						Associate assoc = (Associate)_obj;
                        if (assoc.IsClient)
                        {
                            if (assoc.DefaultAddressSetting == false)
                                lbladdress.Text = assoc.OMSFile.Client.DefaultAddress.ToString();
                            else
                                lbladdress.Text = assoc.Contact.DefaultAddress.ToString();
                        }
                        else
                            lbladdress.Text = assoc.Contact.DefaultAddress.GetAddressString();
					}
					else
						lbladdress.Text = _cont.DefaultAddress.GetAddressString();
				}
				else
					lbladdress.Text = _currentAddress.GetAddressString();
			}
		}

		private void eAddressSelector_Leave(object sender, System.EventArgs e)
		{
			this.OnChanged();
		}
		#endregion

		private void Enquiry_Refreshed(object sender, EventArgs e)
		{
            RefreshControl(_cont, _currentAddress, true);
		}

        private void eAddressSelector_ParentChanged(object sender, EventArgs e)
        {
            Contact cont = null;
            long contid = 0;
            
            //Try and find the contact id from the enquiry form that the control is sitting on.
            if (this.Parent is EnquiryForm)
            {
                // if not business mapped try and get the CONTID field through IEnquiryCompatible interface getextrainfo
                // else try and get from Source.
                try
                {
                    efo = (EnquiryForm)this.Parent;
                    efo.Enquiry.Refreshed -= new EventHandler(Enquiry_Refreshed);
                    efo.Enquiry.Refreshed += new EventHandler(Enquiry_Refreshed);
                    if (efo.Enquiry.InDesignMode)
                    {
                        // Design Mode don't render
                        lbladdress.Text = "Line 1" + Environment.NewLine + "Line 2" + Environment.NewLine + "Line 3" + Environment.NewLine + "Line 4" + Environment.NewLine + "Line 5" + Environment.NewLine + "PostCode/ZipCode";
                        return;
                    }

                    if (efo.Enquiry.Object is Contact)
                    {
                        cont = efo.Enquiry.Object as Contact;
                        _obj = cont;
                    }
                    else if (efo.Enquiry.Object is Client)
                    {
                        _obj = efo.Enquiry.Object as Client;
                    }
                    else if (efo.Enquiry.Object is Associate)
                    {
                        _obj = efo.Enquiry.Object as Associate;
                    }

                    // If the Enquiry Object is not a Contact then test the Object to see if it has a Field called ContID
                    if (cont == null)
                    {
                        // Is the Object on the Enquiry IEnquiryCompatible
                        if (efo.Enquiry.Object is FWBS.OMS.Interfaces.IEnquiryCompatible)
                        {
                            try
                            {
                                contid = (long)((FWBS.OMS.Interfaces.IEnquiryCompatible)efo.Enquiry.Object).GetExtraInfo("CONTID");
                            }
                            catch
                            {
                                // If that fails then try the CLDefaultContact field the Client Object
                                if (contid == 0)
                                {
                                    try
                                    {
                                        contid = (long)((FWBS.OMS.Interfaces.IEnquiryCompatible)efo.Enquiry.Object).GetExtraInfo("CLDefaultContact");
                                    }
                                    catch
                                    {
                                        // If this fails try another Field Type
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

                if (cont == null && contid != 0)
                {
                    cont = Contact.GetContact(contid);
                }

                if (cont != null && _currentAddress != null)
                    this.RefreshControl(cont, _currentAddress);
                else if (cont != null)
                    this.RefreshControl(cont, cont.DefaultAddress);
 
            }
            else if (Parent == null && efo != null)
            {
                efo.Enquiry.Refreshed -= new EventHandler(Enquiry_Refreshed);
            }
        }
	}
}