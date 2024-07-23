using System;
using System.ComponentModel;
using System.Windows.Forms;


namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A custom advanced enquiry control that enables the selecting of contacts
    /// for specified entities within the system.
    /// </summary>
    public class eContactSelector : eSelectorBase
	{
		#region Fields
        private EnquiryForm efo = null;

		/// <summary>
		/// This is the current address object.
		/// </summary>
		private Contact _currentContact = null;
		
		/// <summary>
		/// This is the Client Object
		/// </summary>
		private Client _client;
		#endregion

		#region Control Specific Controls
		private System.Windows.Forms.Label lblInfo;
		private System.Windows.Forms.Panel pnl;
		#endregion

		#region Constructors
		/// <summary>
		/// Creates the address control, and if logged into the system retrieves the list 
		/// of countries from the databse to display in the countries combo box.
		/// </summary>
		public eContactSelector() : base()
		{
			InitializeComponent();
		}

		private void InitializeComponent()
		{
            this.lblInfo = new System.Windows.Forms.Label();
            this.pnl = new System.Windows.Forms.Panel();
            this.pnlContainer.SuspendLayout();
            this.pnl.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboInfoSelector
            // 
            this.cboInfoSelector.Size = new System.Drawing.Size(0, 21);
            // 
            // pnlContainer
            // 
            this.pnlContainer.Size = new System.Drawing.Size(178, 23);
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfo.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(178, 97);
            this.lblInfo.TabIndex = 1;
            // 
            // pnl
            // 
            this.pnl.Controls.Add(this.lblInfo);
            this.pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl.Location = new System.Drawing.Point(150, 23);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(178, 97);
            this.pnl.TabIndex = 4;
            // 
            // eContactSelector
            // 
            this.Controls.Add(this.pnl);
            this.FindLinkVisible = false;
            this.Name = "eContactSelector";
            this.Size = new System.Drawing.Size(328, 120);
            this.Leave += new System.EventHandler(this.eContactSelector_Leave);
            this.ParentChanged += new System.EventHandler(this.eContactSelector_ParentChanged);
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
		/// Gets or Sets the controls value.  This must be overriden by derived classes to make their
		/// own representation of the value using the internal editing control..
		/// </summary>
		public override object Value
		{
			get
			{
				try
				{
					return _currentContact;
				}
				catch
				{
					return DBNull.Value;
				}
			}
			set
			{
				try
				{
					if (_client != null)
					{
						if (value is DBNull || value == null)
						{
						}
						else
						{
							if (Convert.ToInt64(value) != Convert.ToInt64(Combo.SelectedValue))
							{
								Contact cont =  Contact.GetContact(Convert.ToInt64(value));
								RefreshControl(_client, cont);
								if (_client.HasContact(cont))
									Combo.SelectedValue = value;
							}
						}
					}
				}
				catch
				{
				}
			}
		}

		/// <summary>
		/// Executes the changed event.
		/// </summary>
		public override void OnChanged()
		{
            if (this.Parent != null)
            {
                // Check Parent form for any other controls relating and refresh
                foreach (Control ctrl in this.Parent.Controls)
                {
                    if (ctrl is FWBS.OMS.UI.Windows.eAddressSelector)
                    {
                        eAddressSelector s = (eAddressSelector)ctrl;
                        s.Reload(_currentContact);
                        s.OnChanged();
                    }
                    else if (ctrl is FWBS.OMS.UI.Windows.eClientAddressSelector)
                    {
                        base.OnChanged();

                        eClientAddressSelector s = (eClientAddressSelector)ctrl;
                        if ((s.Value as Address) == Address.Null)
                        {
                            s.Reload(_client);
                            s.OnChanged();
                        }
                    }
                }
            }
			base.OnChanged();
		}
		#endregion

		#region Methods
		public void Reload(Client client)
		{
			if (_currentContact == null || _client == null || client.ClientID != _client.ClientID)
				this.RefreshControl(client, client.DefaultContact);
		}

		public void Reload(long clid)
		{
			Client client = Client.GetClient(clid);
			this.RefreshControl(client, client.DefaultContact);
		}

		public void Reload(long clid, long contid)
		{
			Client client = Client.GetClient(clid);
			if (_currentContact != null)
				if (_currentContact.ID == contid) return;
			this.RefreshControl(client , Contact.GetContact(contid));
		}

		public void Reload(Client client, long contid)
		{
			if (_currentContact != null)
				if (_currentContact.ID == contid) return;

			this.RefreshControl(client, Contact.GetContact(contid));
		}
		
		/// <summary>
		/// Refreshes the controls data.
		/// </summary>
		private void RefreshControl(Client client, Contact contact)
		{
			try
			{
				//Fetch the contact if the contact object is null or the contact id has changed.
				if (client != null && contact != null)
				{	
					if (_client == null || client.ClientID != _client.ClientID || _currentContact== null || _currentContact.ID != contact.ID)
					{
						_client = client;
						_currentContact = contact;
						Combo.BeginUpdate();
						Combo.DataSource =  _client.GetContacts();
						Combo.DisplayMember = "contName";
						Combo.ValueMember = "contID";
						_currentContact = contact;

						if (!_client.HasContact(_currentContact))
							_currentContact = _client.DefaultContact;

						Combo.SelectedValue = _currentContact.ID;
						lblInfo.Text = _currentContact.Name;
						Combo.EndUpdate();
						//this.OnChanged();
					}
				}
			}
			catch (Exception ex)
			{
				throw new OMSException2("ERRCLIENTISNULL", "CLIENT is null in creating Client Record", ex);
			}
		}
		#endregion

		#region Captured Events
		/// <summary>
		/// Finds an existing contact.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void Find(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				Contact cont = Services.Searches.FindContact();
				if (cont != null)
				{
					_client.AddContact(new ClientContactLink(_client, cont));
					RefreshControl(_client, cont);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex);
			}
		}

		/// <summary>
		/// Removes a contact from the list.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void Remove(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				if (Combo.SelectedValue != null)
				{
					Contact cont = Contact.GetContact(Convert.ToInt64(Combo.SelectedValue));
					if (cont != null)
					{
						if (MessageBox.Show(FWBS.OMS.UI.Windows.ResourceLookup.GetLookupText("RUSUREREMCONT","Are you sure you wish to remove this Contact?" + Environment.NewLine + "%1%","",false,cont.Name), FWBS.OMS.Branding.APPLICATION_NAME, MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.Yes)
						{	
							_client.RemoveContact(cont);
							Reload(_client, Convert.ToInt64(Combo.SelectedValue));
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex);
			}
			finally
			{
				Reload(_client, Convert.ToInt64(Combo.SelectedValue));
			}
		}

		/// <summary>
		/// Adds a new contact by activating the wizard.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void Add(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				Contact cont = Services.Wizards.CreateContact();
				if (cont != null)
				{
					_client.AddContact(new ClientContactLink(_client, cont));
					RefreshControl(_client, cont);
					this.OnActiveChanged();
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex);
			}
		}

		protected override void Edit(object sender, LinkLabelLinkClickedEventArgs e)
		{
			if (this.Parent is EnquiryForm)
			{
				EnquiryForm enq = this.Parent as EnquiryForm;
				enq.OnNewOMSTypeWindow(new NewOMSTypeWindowEventArgs(FWBS.OMS.Contact.GetContact(Convert.ToInt64(Combo.SelectedValue))));
			}
		}

		/// <summary>
		/// Captures the combo box change event.
		/// </summary>
		/// <param name="sender">The address selector combo box control.</param>
		/// <param name="e">Empty event arguments.</param>
		protected override void Info_Changed (object sender, System.EventArgs e)
		{
			if(_currentContact != null)
			{
				if(_currentContact.IsDirty)
					_currentContact.Update();
			}
			if (Combo.SelectedValue == null) return;
			Reload(_client, Convert.ToInt64(Combo.SelectedValue));
			this.OnActiveChanged();
			this.OnChanged();
        }

		private void eContactSelector_Leave(object sender, System.EventArgs e)
		{
			if (this.omsDesignMode == false)
				this.OnChanged();		
		}
        #endregion

        protected override int PreferredHeight()
        {
            return base.PreferredHeight() + pnl.Height;
        }

        private void eContactSelector_ParentChanged(object sender, EventArgs e)
        {
            if (this.Parent is EnquiryForm)
            {
                Client cl = null;
                long clid = 0;

                // if not business mapped try and get the CONTID field through IEnquiryCompatible interface getextrainfo
                // else try and get from Source.
                try
                {
                    efo = (EnquiryForm)this.Parent;
                    if (efo.Enquiry.InDesignMode)
                    {
                        // Design Mode don't render
                        lblInfo.Text = "Contact Info 1" + Environment.NewLine + "Contact Info 2";
                        return;
                    }

                    if (efo.Enquiry.Object is Client)
                        cl = efo.Enquiry.Object as Client;

                    if (cl == null)
                    {
                        if (efo.Enquiry.Object is FWBS.OMS.Interfaces.IEnquiryCompatible)
                        {
                            try
                            {
                                clid = (long)((FWBS.OMS.Interfaces.IEnquiryCompatible)efo.Enquiry.Object).GetExtraInfo("CLID");
                            }
                            catch
                            {
                            }
                        }
                        else
                        {
                            clid = (long)efo.Enquiry.Source.Tables["DATA"].Rows[0]["CLID"];
                        }
                    }
                }
                catch
                {
                }

                if (cl == null && clid != 0)
                {
                    cl = Client.GetClient(clid);
                }

                if (cl != null)
                    this.RefreshControl(cl, cl.DefaultContact);
            }
            else if (efo != null && Parent == null)
            {

            }
        }
	}
}