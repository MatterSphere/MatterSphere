using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using FWBS.Common.UI;
using FWBS.Common.UI.Windows.Common;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A custom advanced enquiry control that enables the assigning and creation of
    /// address for specified entities within the system.
    /// </summary>
    public class eAddress : FormRendererBase, IBasicEnquiryControl2, IUsesRequiredStars
	{

		#region Events

		/// <summary>
		/// The changed event is used to determine when a major change has happended within the
		/// user control.  This will tend to be used when the internal editing control has changed
		/// in some way or another.
		/// </summary>
		[Category("Action")]
		public event EventHandler Changed;

		/// <summary>
		/// The changed event is used to determine when a major change has happended within the
		/// user control.  This will tend to be used when the internal editing control has changed
		/// in some way or another.
		/// </summary>
		[Category("Action")]
		public event EventHandler ActiveChanged;

		#endregion

		#region Design Mode
		[Browsable(false)]
		public bool omsDesignMode
		{
			get
			{
				return InDesignMode;
			}
			set
			{
                InDesignMode = value;
                if (value)
                {
                    foreach (Control ctrl in this.Controls)
                    {
                        if (ctrl is IBasicEnquiryControl2)
                            ((IBasicEnquiryControl2)ctrl).omsDesignMode = value;
                    }
                }
			}
		}
		#endregion

		#region Fields

		/// <summary>
		/// Marks the editing control as required.
		/// </summary>
		private bool _required = false;

		/// <summary>
		/// Stores how wide the caption should be.
		/// </summary>
		private int _captionWidth = -1;

        /// <summary>
        /// Stores whether the caption should be on top.
        /// </summary>
        private bool _captionTop = false;

        /// <summary>
        /// Stores the height of an item in this control.
        /// </summary>
        private int _itemHeight;

        /// <summary>
        /// Stores the amount of vertical spacing between items.
        /// </summary>
        private int _lineSpacing;

		/// <summary>
		/// The country combo box that is used to pick a country and change the address
		/// format depending on the country chosen.
		/// // DMB 20/02/2004 changed control type to xpcombo
		/// </summary>
		private FWBS.Common.UI.Windows.eXPComboBox cboCountry;

		/// <summary>
		/// Leach Parent for Enquiry Form
		/// </summary>
		private EnquiryForm _parent = null;

		/// <summary>
		/// The y co-ordinate used to dynamically place each addres line one after another.
		/// </summary>
		private int ycoord = 0;

		/// <summary>
		/// This is the current address object.
		/// </summary>
		private object _currentAddress = null;
		
		/// <summary>
		/// Holds a value that indicates whether the control is currently read only.
		/// </summary>
		private bool _readOnly = false;

		/// <summary>
		/// The data table to bind to.
		/// </summary>
		private DataTable dt = null;

		/// <summary>
		/// Last Country Selected
		/// </summary>
		private int _lastcountry = 0;

		/// <summary>
		/// The Last Send Object
		/// </summary>
		private object _value = null;

		private bool _isdirty = false;

		/// <summary>
		/// Creates or Updates the Address Imediately after Lost Focus
		/// </summary>
		private bool _imediatecreate = true;

		#endregion

		#region Properties
		[Browsable(true)]
		public object Control
		{
			get
			{
				return null;
			}
		}

		[Category("Defaults")]
		public string AddressType
		{
			get
			{
				FWBS.OMS.Address add = _currentAddress as Address;
				if (add != null)
					return add.AddType;
				else
					return "ERROR";
			}
			set
			{
				FWBS.OMS.Address add = _currentAddress as Address;
				if (add != null)
					add.AddType = value;
			}
		}
		#endregion
		
		#region Constructors


		/// <summary>
		/// Creates the address control, and if logged into the system retrieves the list 
		/// of countries from the databse to display in the countries combo box.
		/// </summary>
		public eAddress() : base()
		{
			InitializeComponent();
			if (FWBS.OMS.EnquiryEngine.Enquiry.Exists("SCRADDTEMPLATE") == false)
			{
				ErrorBox.Show(ParentForm, new OMSException2("ERRADDTMPMIS","Error Screen 'SCRADDTEMPLATE' is missing",new Exception(),false));
				return;
			}
			TabIndex = 0;
			DataTable countries = Session.CurrentSession.GetCountries();
			countries.TableName = "DSCOUNTRIES";
			AddList(countries);
			this.Controls.Add(cboCountry);
            _itemHeight = cboCountry.PreferredHeight;

            if (FWBS.OMS.Session.CurrentSession.IsLoggedIn)
            {
                this.RightToLeft = FWBS.OMS.Session.CurrentSession.CurrentUser.RightToLeft ? RightToLeft.Yes : RightToLeft.No;
            }
            Render(Session.CurrentSession.Address.CountryID);
            Value = DBNull.Value;
            cboCountry.ActiveChanged -= this.ChangedEvent;
            cboCountry.ActiveChanged += this.ChangedEvent;
            
            MethodInfo EnableRequiredScaling = typeof(ContainerControl).GetMethod("EnableRequiredScaling", BindingFlags.NonPublic | BindingFlags.Instance);
            if (EnableRequiredScaling != null)
                EnableRequiredScaling.Invoke(this, new object[] { this, true });
		}

		private void InitializeComponent()
		{
            this.cboCountry = new FWBS.Common.UI.Windows.eXPComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.req)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.err)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._lists)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._questions)).BeginInit();
            this.SuspendLayout();
            // 
            // cboCountry
            // 
            this.cboCountry.ActiveSearchEnabled = true;
            this.cboCountry.CaptionWidth = 179;
            this.cboCountry.IsDirty = false;
            this.cboCountry.Location = new System.Drawing.Point(607, 17);
            this.cboCountry.MaxLength = 0;
            this.cboCountry.Name = "cboCountry";
            this.cboCountry.Size = new System.Drawing.Size(300, 23);
            this.cboCountry.TabIndex = 0;
            this.cboCountry.Text = "cboCountry";
            // 
            // eAddress
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "eAddress";
            this.Size = new System.Drawing.Size(328, 168);
            this.Rendered += new System.EventHandler(this.eAddress_Rendered);
            this.ValueChanged += new System.EventHandler(this.eAddress_ValueChanged);
            this.FontChanged += new System.EventHandler(this.eAddress_FontChanged);
            this.VisibleChanged += new System.EventHandler(this.eAddress_VisibleChanged);
            this.Leave += new System.EventHandler(this.eAddress_Leave);
            this.ParentChanged += new System.EventHandler(this.eAddress_ParentChanged);
            ((System.ComponentModel.ISupportInitialize)(this.req)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.err)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._lists)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._questions)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		#region IBasicEnquiryControl2 Implementation


		/// <summary>
		/// Gets whether the current control can be stretched by its Y co-ordinate.
		/// This is a design mode property and is set to true.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(true)]
		public bool LockHeight 
		{
			get
			{
				return false;
			}
		}

		/// <summary>
		/// Gets or Sets the control as required.  This is then used by the rendering form to display the
		/// control as required by its own definition.
		/// </summary>
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool Required 
		{
			get
			{
				return _required;
			}
			set
			{
				_required = value;
			}
		}


		/// <summary>
		/// Gets or Sets the editable format of the control.  By default the whole control toggles it's enable property.
		/// </summary>
		[DefaultValue(false)]
		[Category("Behavior")]
		public bool ReadOnly 
		{
			get
			{
				return _readOnly;
			}
			set
			{
				foreach (Control ctrl in this.Controls)
				{
					if (ctrl is FWBS.Common.UI.IBasicEnquiryControl2)
						((FWBS.Common.UI.IBasicEnquiryControl2)ctrl).ReadOnly = value;
					else
						ctrl.Enabled = !value;

				}
				_readOnly = value;

				OnEnabledChanged(EventArgs.Empty);
			}
		}


		/// <summary>
		/// Gets or Sets the caption width of a control, leaving the rest of the width of the control
		/// to be the width of the internal editing control.
		/// </summary>
		[DefaultValue(150)]
        [Browsable(false)]
		public int CaptionWidth
		{
			get
			{
				return _captionWidth;
			}
			set
			{
                int captionWidth = _captionTop ? 0 : value;
                if (_captionWidth != captionWidth)
				{
                    _captionWidth = captionWidth;

                    foreach (Control ctrl in Controls)
					{
                        if (ctrl is IBasicEnquiryControl2)
                            ((IBasicEnquiryControl2)ctrl).CaptionWidth = value;
                    }
				}
			}
		}

        /// <summary>
        /// Gets or Set a bool for the Caption location - on the top or not
        /// </summary>
        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public bool CaptionTop
        {
            get
            {
                return _captionTop;
            }
            set
            {
                if (_captionTop != value)
                {
                    _captionTop = value;
                    _captionWidth = value ? 0 : Common.UI.Windows.eBase2.DefaultCaptionWidth;

                    foreach (Control ctrl in Controls)
                    {
                        if (ctrl is IBasicEnquiryControl2)
                            ((IBasicEnquiryControl2)ctrl).CaptionTop = value;
                    }

                    _itemHeight = cboCountry.PreferredHeight;
                    if (omsDesignMode)
                    {
                        Height = RepositionControls();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of vertical spacing between items.
        /// </summary>
        [Category("OMS Appearance")]
        [DefaultValue(0)]
        public int LineSpacing
        {
            get
            {
                return _lineSpacing;
            }
            set
            {
                if (_lineSpacing != value)
                {
                    _lineSpacing = Math.Max(value, 0);

                    if (omsDesignMode)
                    {
                        Height = RepositionControls();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the amount of vertical spacing between items.
        /// </summary>
        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public bool EnableCueText
        {
            get; set;
        }

		/// <summary>
		/// Gets or Sets the controls value.  This must be overriden by derived classes to make their
		/// own representation of the value using the internal editing control..
		/// </summary>
		public object Value
		{
			get
			{
				//if the control has been set as a required field check that the required fields within this control are set
				if(Required)
				{
					//loop all controls and if a required field is null return null
					foreach(Control ctrl in this.Controls)
					{
						DataRow row  = this.GetSettings(ctrl);
						if (row != null)
							if(Convert.ToBoolean(row["qurequired"]) == true)
								if(ctrl is IBasicEnquiryControl2)
									if(((IBasicEnquiryControl2)ctrl).Value == DBNull.Value || Convert.ToString(((IBasicEnquiryControl2)ctrl).Value) == "") 
										return DBNull.Value;
						
					}
					//if we get this far then no required fields are null
					return _currentAddress;
				}
				else
				{
					//DMB 28/1/2004 might have to keep an eye on this in case _current address is null and it causes problems
					return _currentAddress;
				}
			}
			set
			{
				if (_value != value)
				{
					_value = value;
					if (value is Address || value is DataTable)
					{
						if ((_currentAddress != value) && (value != null))
						{
							_currentAddress = value;
							BindControls();
						}
					}
                    else if (value is DBNull || value == null)
                    {
                        _currentAddress = new Address();
                        BindControls();
                    }
					else if (FWBS.Common.ConvertDef.ToInt64(value,0) != 0)
					{
						_currentAddress = Address.GetAddress((long)value);
						BindControls();
					}
					if(this.Parent != null)
					{
						OnActiveChanged();
						OnChanged();
					}

					if (_value is Address)
					{
						if (((Address)_value).IsNew == false) 
							if (this.ReadOnly == false) this.ReadOnly = true;
						else
							if (this.ReadOnly) this.ReadOnly = false;

					}
					else
						if (this.ReadOnly) this.ReadOnly = false;
				
				}

			}
		}

		[Browsable(false)]
		public bool IsDirty
		{
			get
			{
				return _isdirty;
			}
			set
			{
				_isdirty = value;
			}
		}

		[LocCategory("Data")]
		public bool ImediateCreate
		{
			get
			{
				return _imediatecreate;
			}
			set
			{
				_imediatecreate = value;
			}
		}

		/// <summary>
		/// Executes the changed event.
		/// </summary>
		public void OnActiveChanged()
		{
			if (ActiveChanged != null)
				ActiveChanged(this, EventArgs.Empty);
			IsDirty = true;
		}

		public void OnActiveChanged(object sender)
		{
			if (ActiveChanged != null)
				ActiveChanged(sender, EventArgs.Empty);
			IsDirty = true;
		}

		public void OnChanged()
		{
			
			if (Changed!= null && IsDirty)
			{
				Changed(this, EventArgs.Empty);
				if (_imediatecreate && _parent == null)
				{
					FWBS.OMS.Address add = _currentAddress as Address;
					if (add != null) add.Update();
				}
			}
			IsDirty = false;
		}

		#endregion

		#region Render Methods


		/// <summary>
		/// Renders all the controls on address rendering form by the country identifier
		/// passed.
		/// </summary>
		/// <param name="countryID">Unique country identifier.</param>
		private void Render(int countryID)
		{
			if (_lastcountry != countryID)
			{
                try
                {
                    _questions = Address.GetAddressFormat(countryID);
                }
                catch (Exception)
                {
                    //role back to englands descriptions
                    _questions = Address.GetAddressFormat(223);
                }
			
				foreach (Control ctrl in Controls)
				{
                    req.SetError(ctrl, "");
                    ctrl.Visible = false;
					ctrl.DataBindings.Clear();
				}

				RenderControls(false);

                if (Parent == null)
                    Height = ycoord;

				ycoord = 0;

				cboCountry.Value = countryID;
				_lastcountry = countryID;
			}
		}

		/// <summary>
		/// Overriden render control method from the base rendering form to apply 
		/// extra property settings per question asked.
		/// </summary>
		/// <param name="ctrl">Reference to the newly created / or existing control.</param>
		/// <param name="settings">Settings data row that stores the information.</param>
		public override void RenderControl(ref Control ctrl, DataRow settings)
		{
			base.RenderControl(ref ctrl, settings);
			
			ctrl.Tag = settings["qufieldname"];
			
			if (ctrl is IBasicEnquiryControl2)
			{
				//If no caption width is supplied then use the parent questions caption width.
				IBasicEnquiryControl2 basic = (IBasicEnquiryControl2)ctrl;
                if (_captionTop)
                {
                    basic.CaptionTop = _captionTop;
                }
                else if (_captionWidth < 0)
                {
                    basic.CaptionWidth = (int)settings["qucaptionwidth"];
                }
                else
                {
                    basic.CaptionWidth = _captionWidth;
                }

				basic.ReadOnly = _readOnly;
                basic.ActiveChanged -= new EventHandler(eAddress_ValueChanged);
                basic.ActiveChanged += new EventHandler(eAddress_ValueChanged);
                if (basic.Control is Control)
                {
                    ((Control)basic.Control).KeyUp -= new KeyEventHandler(Control_KeyUp);
                    ((Control)basic.Control).KeyUp += new KeyEventHandler(Control_KeyUp);
                }
			}
			else
			{
				ctrl.Enabled = !_readOnly;
                ctrl.KeyUp -= new KeyEventHandler(Control_KeyUp);
                ctrl.KeyUp += new KeyEventHandler(Control_KeyUp);
			}

            if (AutoScaleMode == AutoScaleMode.Inherit && DeviceDpi != 96)
            {
                float scaleFactor = DeviceDpi / 96F;
                ctrl.Scale(new SizeF(scaleFactor, scaleFactor));
            }

            SetChildControlPos(ctrl, ycoord);
            Controls.SetChildIndex(ctrl, 0);
            ycoord += ctrl.Height;
            ycoord += ConvertUnits(_lineSpacing);

            BindControls(ctrl);
		}

        private void eAddress_Rendered(object sender, EventArgs e)
        {
            this.AutoScaleMode = AutoScaleMode.Inherit;
        }

        private void SetChildControlPos(Control ctrl, int y)
        {
            ctrl.Anchor = AnchorStyles.None;
            ctrl.Size = new Size(Width - (_captionTop ? 0 : err.Icon.Width), _itemHeight);
            ctrl.Location = new Point(RightToLeft == RightToLeft.Yes ? Width - ctrl.Width : 0, y);
            ctrl.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        }

        private int RepositionControls()
        {
            int y = 0;
            SuspendLayout();
            for (int i = Controls.Count - 1; i >= 0; i--)
            {
                Control ctrl = Controls[i];
                if (ctrl is IBasicEnquiryControl2 && ctrl.Visible)
                {
                    SetChildControlPos(ctrl, y);
                    y += ctrl.Height;
                    y += LogicalToDeviceUnits(_lineSpacing);
                }
            }
            y -= LogicalToDeviceUnits(_lineSpacing);
            ResumeLayout();
            return y;
        }

        private void DisableCueText()
        {
            object control;
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is IBasicEnquiryControl2)
                {
                    control = ((IBasicEnquiryControl2)ctrl).Control;
                    if (control is CueTextBox)
                        ((CueTextBox)control).CueText = string.Empty;
                    else if (control is CueComboBox)
                        ((CueComboBox)control).CueText = string.Empty;
                }
            }
        }

		#endregion

		#region Methods
		/// <summary>
		/// Captures the changed event of the countries combobox to re-render the controls.
		/// </summary>
		/// <param name="sender">Countries combo box reference.</param>
		/// <param name="e">Empty event arguments.</param>
		private void ChangedEvent(object sender, EventArgs e)
		{
			if (cboCountry.Value is int && _lastcountry != (int)cboCountry.Value)
			{
                Action action = delegate ()
                {
                    cboCountry.ActiveChanged -= this.ChangedEvent;
                    cboCountry.Enabled = false;
                    Render((int)cboCountry.Value);
                    cboCountry.Enabled = true;
                    cboCountry.Focus();
                    cboCountry.ActiveChanged += this.ChangedEvent;
                };
                if (sender != this)
                    BeginInvoke(action);
                else
                    action();
			}
		}


		/// <summary>
		/// Binds the controls to the underlying address object data source.
		/// </summary>
		private void BindControls()
		{
			if (_currentAddress != null)
			{
                foreach (Control ctrl in this.Controls)
				{
					BindControls(ctrl);
				}
			}
			ChangedEvent(this, EventArgs.Empty);
		}

        /// <summary>
        /// Update the Data by event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ictrl_Changed(object sender, EventArgs e)
        {
            IBasicEnquiryControl2 ictrl = sender as IBasicEnquiryControl2;
            Control ctrl = sender as Control;
            if (ictrl != null)
            {
                if (_currentAddress is DataTable)
                    dt = (DataTable)_currentAddress;
                else
                    dt = ((Address)_currentAddress).GetDataTable();
                if (Convert.ToString(dt.Rows[0][ctrl.Tag.ToString()]) != Convert.ToString(ictrl.Value)) 
                    dt.Rows[0][ctrl.Tag.ToString()] = ictrl.Value;

                OnChanged();
            }
        }



		/// <summary>
		/// Binds the specified control to the underlying address object data source.
		/// </summary>
		/// <param name="ctrl">Control to bind.</param>
		private void BindControls(Control ctrl)
		{
            if (_currentAddress != null)
            {
                if (_currentAddress is DataTable)
                    dt = (DataTable)_currentAddress;
                else
                    dt = ((Address)_currentAddress).GetDataTable();

                IBasicEnquiryControl2 ictrl = ctrl as IBasicEnquiryControl2;
                if (ictrl != null)
                {
                    ictrl.Changed -= new EventHandler(ictrl_Changed);
                    ictrl.Value = dt.Rows[0][ctrl.Tag.ToString()];
                    ictrl.Changed += new EventHandler(ictrl_Changed);
                }
            }
		}

		/// <summary>
		/// End the edit ont the lost focus of the control.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void eAddress_Leave(object sender, System.EventArgs e)
		{
			try
			{
				this.BindingContext[dt].EndCurrentEdit();
			}
			catch
			{}
			OnChanged();
		}

		private void eAddress_ValueChanged(object sender, System.EventArgs e)
		{
			OnActiveChanged(sender);
        }

		private void eAddress_ParentChanged(object sender, System.EventArgs e)
		{
			if (_imediatecreate)
			{
				_parent = this.Parent as EnquiryForm;
				if (_parent != null)
				{
					_parent.Updating +=new CancelEventHandler(EnquiryForm_Updating);
				}
			}
		}

        private void eAddress_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible && Parent != null)
            {
                _itemHeight = cboCountry.PreferredHeight;
                Height = RepositionControls();

                if (!omsDesignMode && !EnableCueText)
                    DisableCueText();
            }
        }

        private void eAddress_FontChanged(object sender, EventArgs e)
        {
            _itemHeight = cboCountry.PreferredHeight;
            if (omsDesignMode)
            {
                Height = RepositionControls();
            }
        }

		private void EnquiryForm_Updating(object sender, CancelEventArgs e)
		{
			if (_imediatecreate && _parent != null)
			{
				FWBS.OMS.Address add = _currentAddress as Address;
				if (add != null) add.Update();
				OnActiveChanged();
				OnChanged();
			}
		}

		private void Control_KeyUp(object sender, KeyEventArgs e)
		{
			try
			{
				if (Session.CurrentSession.IsLicensedFor("POSTCODE"))
				{
					Cursor = Cursors.WaitCursor;

					switch (e.KeyCode)
					{
						case Keys.F8:
						{
							if (dt.Rows.Count > 0)
							{
								IBasicEnquiryControl2 ctrl = sender as IBasicEnquiryControl2;
								string val = "";
								if (ctrl == null)
									val = ((Control)sender).Text;
								else
									val = Convert.ToString(ctrl.Value);
								
								string [] pc = Convert.ToString(val).Split(' ');
								if (pc.Length > 1 && Session.CurrentSession.IsPackageInstalled("POSTCODE"))
								{
									Address add = Address.GetAddress(String.Join("", pc, 1, pc.Length - 1), pc[0]);

                                    if (add != null)
                                    {
                                        //preserve the original country set in the combobox
                                        int countryid = FWBS.Common.ConvertDef.ToInt32(dt.Rows[0]["addCountry"], 0);
                                        if (countryid >= 0)
                                            add.CountryID = countryid;

                                        Value = add;
                                    }
								}
							}
						
						}
							break;
						case Keys.F9:
						{
							if (Session.CurrentSession.IsPackageInstalled("POSTCODE"))
							{
								Address add = Services.Searches.FindAddressEx(this.ParentForm);
								if (add != null)
									Value = add;
							}
						}
							break;
					}
				}
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


		#endregion

		#region IUsesRequiredStars Members

		/// <summary>
		/// switches in the required field validation icons 
		/// </summary>
		/// <param name="on"></param>
		public void ErrorIconsOn(bool on)
		{
			if(Required)
			{
				foreach(Control ctrl in this.Controls)
				{
					DataRow row  = this.GetSettings(ctrl);
					
					// DMB 20/02/2004 Need a null check here
					if(row != null)
					{
						if(Convert.ToBoolean(row["qurequired"]) == true && ctrl is IBasicEnquiryControl2 
							&& (((IBasicEnquiryControl2)ctrl).Value == DBNull.Value || Convert.ToString(((IBasicEnquiryControl2)ctrl).Value) == ""))
						{
							if(on)
							{
								err.SetError(ctrl,_reqFieldRenderer.Description);
								req.SetError(ctrl,"");
                            }
							else
							{
                                _reqFieldRenderer.MarkRequiredControl(ctrl);
                                err.SetError(ctrl, "");
                            }
						}
					}
				}
			}
			else
			{
				//switch off error text if this object is not a required control
				foreach(Control ctrl in this.Controls)
				{
					err.SetError(ctrl,"");
					req.SetError(ctrl,"");
                }
			}
		}

		public void RequiredIconsOn(bool on)
		{
			// TODO:  Add eAddress.ErrorIconsOn implementation
		}

		#endregion

		
	}
}
