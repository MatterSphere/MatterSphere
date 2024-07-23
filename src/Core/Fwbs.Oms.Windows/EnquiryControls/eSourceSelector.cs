using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// An enquiry control which will control the source of business marketing parameters
    /// for clients and files.
    /// </summary>
    public class eSourceSelector : FWBS.Common.UI.Windows.eComboBoxList2
	{
		#region Fields
		/// <summary>
		/// Source of business information.
		/// </summary>
		private System.Windows.Forms.Label _lblSourceInfo;

		/// <summary>
		/// Show button that will be used to display a source of business contact.
		/// </summary>
		private System.Windows.Forms.Button _btnDisplayContact;

		private string _value = "";

		private FWBS.OMS.Contact _contact;

		private FWBS.OMS.User _user;

		private string _prevval = "";
		#endregion

		#region Constructors & Destructors
		/// <summary>
		/// Default constructor.
		/// </summary>
		public eSourceSelector()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.VisibleChanged +=new EventHandler(eSourceSelector_VisibleChanged);
            this.ActiveChanged += new EventHandler(eSourceSelector_ActiveChanged);
		}

        void eSourceSelector_ActiveChanged(object sender, EventArgs e)
        {
            _value = Convert.ToString(this.Value);
        }

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this._lblSourceInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _ctrl
            // 
            this._ctrl.Location = new System.Drawing.Point(17, 0);
            this._ctrl.KeyDown += new System.Windows.Forms.KeyEventHandler(this._ctrl_KeyDown);
            // 
            // _lblSourceInfo
            // 
            this._lblSourceInfo.AutoSize = true;
            this._lblSourceInfo.Name = "_lblSourceInfo";
            this._lblSourceInfo.TabIndex = 0;
            this._lblSourceInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this._lblSourceInfo.Visible = false;
            this._lblSourceInfo.DpiChangedAfterParent += _lblSourceInfo_DpiChangedAfterParent;
            // 
            // eSourceSelector
            // 
            this.Controls.Add(this._lblSourceInfo);
            this.ActiveChanged += new System.EventHandler(this.eSourceSelector_Changed);
            this.Controls.SetChildIndex(this._lblSourceInfo, 0);
            this.Controls.SetChildIndex(this._ctrl, 0);
            this.Height = 41;
            this.ResumeLayout(false);
		}
        #endregion
        #endregion

        #region Methods
        /// <summary>
        /// A method that captures the changed event of the combo box.
        /// </summary>
        /// <param name="sender">The current instance of the control.</param>
        /// <param name="e">Empty event arguments.</param>
        private void eSourceSelector_Changed(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				string val = Convert.ToString(Value);
                bool ChangePrev = true;
				ResetInfo();

				object source = SetInfo(val);

				switch (val)
				{
					case "USER":
						User usr = Services.Searches.FindUser();
                        if (usr != null)
                        {
                            if (source is Client)
                            {
                                Client cl = (Client)source;
                                cl.SourceIsUser = usr;
                                _lblSourceInfo.Text = usr.FullName;
                            }
                            else if (source is OMSFile)
                            {
                                OMSFile fl = (OMSFile)source;
                                fl.SourceIsUser = usr;
                                _lblSourceInfo.Text = usr.FullName;
                            }
                            else
                            {
                                _user = usr;
                                _lblSourceInfo.Text = usr.FullName;
                            }
                        }
                        else
                        {
                            ChangePrev = false;
                            if (_prevval.Length == 0)
                                SelectFirstItem();
                            else
                                Value = _prevval;
                        }
						break;
					case "CONTACT":
						Contact cont = Services.Searches.FindContact();
                        if (cont != null)
                        {
                            if (source is Client)
                            {
                                Client cl = (Client)source;
                                cl.SourceIsContact = cont;
                                _lblSourceInfo.Text = cont.Name;
                            }
                            else if (source is OMSFile)
                            {
                                OMSFile fl = (OMSFile)source;
                                fl.SourceIsContact = cont;
                                _lblSourceInfo.Text = cont.Name;
                                fl.PrecedentLibrary = cont.PrecedentLibrary;
                            }
                            else
                            {
                                _contact = cont;
                                _lblSourceInfo.Text = cont.Name;
                            }
                        }
                        else
                        {
                            if (_prevval != "CONTACT")
                                HideDisplayContactButton();

                            ChangePrev = false;
                            if (_prevval.Length == 0)
                                SelectFirstItem();
                            else
                                Value = _prevval;
                        }
						break;
                    default:
                        if (source is Client)
                        {
                            Client cl = (Client)source;
                            cl.SourceIsContact = null;
                            cl.SourceIsUser = null;
                        }
                        else if (source is OMSFile)
                        {
                            OMSFile fl = (OMSFile)source;
                            fl.SourceIsContact = null;
                            fl.SourceIsUser = null;
                        }
                        break;
				}

                if (ChangePrev)
				    _prevval = val;
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Displays the contact that is the source of business.
		/// </summary>
		/// <param name="sender">The show button instance.</param>
		/// <param name="e">Empty event arguments.</param>
		private void cmdShow_Click(object sender, System.EventArgs e)
		{
			try
			{
				Cursor = Cursors.WaitCursor;
				EnquiryForm enq = this.Parent as EnquiryForm;
				object source = null;
				source = enq.Enquiry.Object;
				Contact cont = null;
				if (source is Client)
					cont = ((Client)source).SourceIsContact as Contact;
				else if (source is OMSFile)
					cont = ((OMSFile)source).SourceIsContact as Contact;
				else if (_contact != null)
					cont = _contact;
				if (cont != null)
					Services.ShowContact(cont);
			}
			catch (Exception ex)
			{
				ErrorBox.Show(TopLevelControl, ex);
			}
			finally
			{
				Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Resets the information controls and makes them invisible.
		/// </summary>
		private void ResetInfo()
		{
            HideLabelSourceInfo();
            HideDisplayContactButton();
			_lblSourceInfo.ResetText();
		}

		/// <summary>
		/// Sets the information on the control, if data already exists.
		/// </summary>
		/// <param name="val">The value of the selector combo box.</param>
		/// <param name="isDefaultInitializing">Indicates whether is default initializing.</param>
		/// <returns>The source object.</returns>
		private object SetInfo(string val, bool isDefaultInitializing = false)
		{
//			if (_prevval == "") _prevval = val;

			EnquiryForm enq = this.Parent as EnquiryForm;
			object source = null;

			if (enq != null)
			{
				source = enq.Enquiry.Object;
				switch(val)
				{
					case "USER":
					{
						ShowLabelSourceInfo(isDefaultInitializing);
						User usr = null;
						if (source is Client)
							usr = ((Client)source).SourceIsUser as User;
						else if (source is OMSFile)
							usr = ((OMSFile)source).SourceIsUser as User;
						else if (_user != null)
							usr = _user;
						if (usr != null)
							_lblSourceInfo.Text = usr.FullName;
						break;
					}
					case "CONTACT":
					{
                        ShowDisplayContactButton(isDefaultInitializing);
                        ShowLabelSourceInfo(isDefaultInitializing);
						Contact cont = null;
						if (source is Client)
							cont = ((Client)source).SourceIsContact as Contact;
						else if (source is OMSFile)
							cont = ((OMSFile)source).SourceIsContact as Contact;
						else if (_contact != null)
							cont = _contact;
						if (cont != null)
							_lblSourceInfo.Text = cont.Name;
						break;
					}
				}
			}
			return source;
		}
        #endregion

        #region Properties
        public override object Value
		{
			get
			{
				return base.Value;
			}
			set
			{
				ResetInfo();
                SetInfo(Convert.ToString(value), base.Value == null);
				base.Value = Convert.ToString(value);
				_value = Convert.ToString(value);
			}
		}

		[Browsable(false)]
		public Contact SourceIsContact
		{
			get
			{
				return _contact;
			}
		}

		public User SourceIsUser
		{
			get
			{
				return _user;
			}
		}

        [Browsable(false)]
        public override int PreferredHeight
        {
            get
            {
                return base.PreferredHeight + _ctrl.PreferredSize.Height;
            }
        }

        #endregion

        #region Private
        private void eSourceSelector_VisibleChanged(object sender, EventArgs e)
		{
			if (_value != "") 
                base.Value = _value;
            else
                SelectFirstItem();
		}

		private void _ctrl_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (((ComboBox)_ctrl).DroppedDown == false && (e.KeyCode == Keys.Down || e.KeyCode == Keys.Up))
			{
				((ComboBox)_ctrl).DroppedDown =true;
				e.Handled=true;
			}
		}
		#endregion

        public void SelectFirstItem()
        {
            if (((ComboBox)_ctrl).Items.Count > 0)
                ((ComboBox)_ctrl).SelectedIndex = 0;
        }

        private void ShowDisplayContactButton(bool isDefaultInitializing)
        {
            if (_btnDisplayContact == null)
            {
                InitializeButtonDisplayContact(isDefaultInitializing);
            }
            else
            {
                _btnDisplayContact.Visible = true;
            }
        }

        private void InitializeButtonDisplayContact(bool isBeforeScaling)
        {
            _btnDisplayContact = new Button
            {
                AutoEllipsis = false,
                FlatStyle = System.Windows.Forms.FlatStyle.System,
                Location = Point.Empty,
                Margin = new Padding(0),
                Size = Size.Empty,
                Text = "...",
                Visible = true
            };
            _btnDisplayContact.Click += new System.EventHandler(this.cmdShow_Click);
            _btnDisplayContact.DpiChangedAfterParent += _btnDisplayContact_DpiChangedAfterParent;
            this.Controls.Add(_btnDisplayContact);
            this.Controls.SetChildIndex(_btnDisplayContact, 0);
            this.Controls.SetChildIndex(_ctrl, 0);
            CalculateButtonDisplayContactBounds(isBeforeScaling);
        }

        private void ShowLabelSourceInfo(bool isDefaultInitializing)
        {
            CalculateLabelSourceInfoLocation(isDefaultInitializing);
            _lblSourceInfo.Visible = true;
        }

        public override void SetRTL(Form parentform)
        {
            base.SetRTL(parentform);
            CalculateButtonDisplayContactBounds(false);
            CalculateLabelSourceInfoLocation(false);
        }
        
        private void _btnDisplayContact_DpiChangedAfterParent(object sender, EventArgs e)
        {
            CalculateButtonDisplayContactBounds(false);
        }

        private void _lblSourceInfo_DpiChangedAfterParent(object sender, EventArgs e)
        {
            CalculateLabelSourceInfoLocation(false);
        }

        private void CalculateButtonDisplayContactBounds(bool isDefaultInitializing)
        {
            _btnDisplayContact.Anchor = AnchorStyles.None;
            if (_cmd != null)
            {
                _btnDisplayContact.Size = _cmd.Size;
                _btnDisplayContact.Location = new Point
                {
                    X = _cmd.Location.X,
                    Y = _cmd.Bottom
                };
            }
            else
            {
                int size = !isDefaultInitializing
                    ? Math.Min(_btnDisplayContact.PreferredSize.Height, _ctrl.PreferredSize.Height)
                    : Math.Min(_btnDisplayContact.PreferredSize.Height, _ctrl.PreferredSize.Height) * 96 / DeviceDpi;
                _btnDisplayContact.Size = new Size(size, size);


                int y = !isDefaultInitializing
                    ? _ctrl.Bottom
                    : _ctrl.Top + _ctrl.Height * 96 / DeviceDpi;

                _btnDisplayContact.Location = new Point
                {
                    X = RightToLeft == RightToLeft.Yes
                        ? LogicalToDeviceUnits(CaptionWidth)
                        : this.Width - _btnDisplayContact.Width,
                    Y = y
                };
            }
            _btnDisplayContact.Anchor = (RightToLeft == RightToLeft.Yes) ? (AnchorStyles.Top | AnchorStyles.Left) : (AnchorStyles.Top | AnchorStyles.Right);
            CalculateLabelSourceInfoLocation(isDefaultInitializing);
        }

        private void CalculateLabelSourceInfoLocation(bool isDefaultInitializing)
        {
            if (_btnDisplayContact != null && _btnDisplayContact.Visible)
            {
                int y = !isDefaultInitializing
                    ? _btnDisplayContact.Location.Y +
                      ((_btnDisplayContact.Height - _lblSourceInfo.Height) / 2)
                    : _btnDisplayContact.Location.Y +
                      ((_btnDisplayContact.Height - _lblSourceInfo.Height) / 2) * 96 / DeviceDpi;
                _lblSourceInfo.Location = new Point
                {
                    X = RightToLeft == RightToLeft.Yes 
                    ? LogicalToDeviceUnits(CaptionWidth) + _btnDisplayContact.Width
                    : LogicalToDeviceUnits(CaptionWidth),
                    Y = y
                };
            }
            else
            {
                int y = !isDefaultInitializing
                    ? _ctrl.Bottom
                    : _ctrl.Top + _ctrl.Height * 96 / DeviceDpi;
                _lblSourceInfo.Location = new Point
                {
                    X = _ctrl.Location.X,
                    Y = y
                };
            }
        }

        private void HideDisplayContactButton()
        {
            if (_btnDisplayContact != null)
            {
                this.Controls.Remove(_btnDisplayContact);
                _btnDisplayContact.Dispose();
                _btnDisplayContact = null;
            }
        }

        private void HideLabelSourceInfo()
        {
            _lblSourceInfo.Visible = false;
        }
    }
}