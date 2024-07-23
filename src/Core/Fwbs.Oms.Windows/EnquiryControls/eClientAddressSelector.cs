using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A custom advanced enquiry control that enables the assigning of addresses for a client.
    /// </summary>
    public class eClientAddressSelector : eSelectorBase
	{
		#region Fields
        private EnquiryForm efo = null;
		private Address _currentAddress = null;
		private Client _cl = null;
		private bool _includeDefault = true;
		#endregion

		#region Controls Specific Controls
		private System.Windows.Forms.Label lbladdress;
		private System.Windows.Forms.Panel pnl;
		#endregion
		
		#region Constructors
		/// <summary>
		/// Creates the address control, and if logged into the system retrieves the list 
		/// of countries from the databse to display in the countries combo box.
		/// </summary>
		public eClientAddressSelector() : base()
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
            // pnlContainer
            // 
            this.pnlContainer.Size = new System.Drawing.Size(146, 23);
            // 
            // pnl
            // 
            this.pnl.Controls.Add(this.lbladdress);
            this.pnl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl.Location = new System.Drawing.Point(150, 23);
            this.pnl.Name = "pnl";
            this.pnl.Size = new System.Drawing.Size(146, 49);
            this.pnl.TabIndex = 0;
            // 
            // lbladdress
            // 
            this.lbladdress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbladdress.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.lbladdress.Location = new System.Drawing.Point(0, 0);
            this.lbladdress.Name = "lbladdress";
            this.lbladdress.Size = new System.Drawing.Size(146, 49);
            this.lbladdress.TabIndex = 2;
            // 
            // eClientAddressSelector
            // 
            this.Controls.Add(this.pnl);
            this.FindLinkVisible = false;
            this.Name = "eClientAddressSelector";
            this.Size = new System.Drawing.Size(296, 72);
            this.Leave += new System.EventHandler(this.eAddressSelector_Leave);
            this.ParentChanged += new System.EventHandler(this.eClientAddressSelector_ParentChanged);
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
					return _currentAddress;
				}
				catch
				{
					return DBNull.Value;
				}
			}
			set
			{
				if (value is Address)
					_currentAddress = value as Address;
				
				try
				{
					if (_cl != null)
					{
						if (value is DBNull || value == null)
						{
						}
						else
						{
							Address add = (Address)value;
							if (add.ID != Convert.ToInt64(Combo.SelectedValue))
							{
								RefreshControl(_cl, add);
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
		/// <summary>
		/// Captures the combo box change event.
		/// </summary>
		/// <param name="sender">The address selector combo box control.</param>
		/// <param name="e">Empty event arguments.</param>
		protected override void Info_Changed(object sender, System.EventArgs e)
		{
			if (Combo.SelectedValue == null) return;
			Reload(_cl, Convert.ToInt64(Combo.SelectedValue));
			this.OnChanged();
			this.OnActiveChanged();
			DisplayAddress();
		}
		
		#endregion

		#region Properties
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
		public void Reload(Client client)
		{
			if (_currentAddress == null || _cl == null || client.ClientID != _cl.ClientID)
				this.RefreshControl(client, Address.Null);

			this.DisplayAddress();
		}

		public void Reload(Client client, long addid)
		{
			if (_currentAddress != null)
				if (_currentAddress.ID == addid) return;

			this.RefreshControl(client, Address.GetAddress(addid));
			this.DisplayAddress();
		}

		private void RefreshControl(Client client, Address address)
		{
			this.RefreshControl(client,address,false);
		}

		private void RefreshControl(Client client, Address address, bool force)
		{
			try
			{
				//Fetch the contact if the contact object is null or the contact id has changed.
				if (client != null && address != null)
				{	
					if (_cl == null || client.ClientID != _cl.ClientID || _currentAddress == null || _currentAddress.ID != address.ID || force == true)
					{
						_cl = client;
						_currentAddress = address;
						
						Combo.BeginUpdate();
						DataView dv = _cl.GetAddresses(_includeDefault);
						
						Combo.DataSource = dv;
						Combo.DisplayMember = "addline1";
						Combo.ValueMember = "addid";
						
						Combo.SelectedValue = _currentAddress.ID;

						DisplayAddress();

						Combo.EndUpdate();
					}
				}
			}
			catch (Exception ex)
			{
                throw new OMSException2("ERRCONTIDISNULL", "CONTID is null in creating Contact Record", ex);
            }
		}

		/// <summary>
		/// Displays the address below the address picker combo box.
		/// </summary>
		private void DisplayAddress()
		{
			if (_currentAddress == Address.Null)
			{
				lbladdress.Text = _cl.DefaultContact.DefaultAddress.GetAddressString() + Environment.NewLine + "(" + _cl.DefaultContact.Name + ")";
			}
			else
			{
				lbladdress.Text = _currentAddress.GetAddressString();
				try
				{
					DataRowView r = (DataRowView)cboInfoSelector.SelectedItem;
					lbladdress.Text += Environment.NewLine + "(" + Convert.ToString(r["notes"]) + ")";
				}
				catch{}
			}
		}

		private void eAddressSelector_Leave(object sender, System.EventArgs e)
		{
			this.OnChanged();
		}
		#endregion

		private void Enquiry_Refreshed(object sender, EventArgs e)
		{
			if (_cl != null) 
			{
				_currentAddress = null;
				RefreshControl(_cl,_cl.DefaultAddress);
			}
			DisplayAddress();
		}

		private void cboInfoSelector_DrawItem(object sender, System.Windows.Forms.DrawItemEventArgs e)
		{
			try
			{
				ComboBox cbo = (ComboBox)sender;

				string drawString = "";
				try
				{
					DataView vw = cbo.DataSource as DataView;
					if (vw != null)
					{
						drawString = Convert.ToString(vw[e.Index]["addline1"]);
						string itm = Convert.ToString(vw[e.Index]["addline2"]);
						if (itm != "")
							drawString = drawString + Environment.NewLine + itm;
						itm = Convert.ToString(vw[e.Index]["addline3"]);
						if (itm != "")
							drawString = drawString + Environment.NewLine + itm;
						itm = Convert.ToString(vw[e.Index]["addline4"]);
						if (itm != "")
							drawString = drawString + Environment.NewLine + itm;
						itm = Convert.ToString(vw[e.Index]["addline5"]);
						if (itm != "")
							drawString = drawString + Environment.NewLine + itm;
						itm = Convert.ToString(vw[e.Index]["addpostcode"]);
						if (itm != "")
							drawString = drawString + Environment.NewLine + itm;

					}
					else
						drawString = Convert.ToString(cboInfoSelector.Items[e.Index]);
				}
				catch
				{
				}

                Brush drawBrush ;

                if (e.BackColor.Name == "Highlight")
                    drawBrush = SystemBrushes.HighlightText;
                else
                    drawBrush = SystemBrushes.ControlText;

				e.DrawBackground();

				e.Graphics.DrawString(drawString, this.Font, drawBrush,e.Bounds.X, e.Bounds.Y);
				e.DrawFocusRectangle();
			}
			catch{}
			
		}

		private void cboInfoSelector_MeasureItem(object sender, System.Windows.Forms.MeasureItemEventArgs e)
		{
			try
			{
				int ctr = 0;
				try
				{
					ComboBox cbo = (ComboBox)sender;
					DataView vw = cbo.DataSource as DataView;
					if (vw != null)
					{
						ctr++;
						if (Convert.ToString(vw[e.Index]["addline2"]) != "")
							ctr++;
						if (Convert.ToString(vw[e.Index]["addline3"]) != "")
							ctr++;
						if (Convert.ToString(vw[e.Index]["addline4"]) != "")
							ctr++;
						if (Convert.ToString(vw[e.Index]["addline5"]) != "")
							ctr++;
						if (Convert.ToString(vw[e.Index]["addpostcode"]) != "")
							ctr++;
					}
					else
					{
						ctr++;
					}
				}
				catch
				{
				}
				e.ItemHeight *= ctr;
			}
			catch{}
		}

        protected override int PreferredHeight()
        {
            return base.PreferredHeight() + pnl.Height;
        }

        private void eClientAddressSelector_ParentChanged(object sender, EventArgs e)
        {
            //Try and find the contact id from the enquiry form that the control is sitting on.
            if (this.Parent is EnquiryForm)
            {
                Client cl = null;

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

                    if (efo.Enquiry.Object is Client)
                    {
                        cl = efo.Enquiry.Object as Client;
                    }
                }
                catch { }
                if (cl != null && _currentAddress != null)
                    this.RefreshControl(cl, _currentAddress);
                else if (cl != null)
                    this.RefreshControl(cl, cl.DefaultAddress);
            }
            else if (this.Parent == null && efo != null)
            {
                efo.Enquiry.Refreshed -= new EventHandler(Enquiry_Refreshed);
            }
        }
	}
}