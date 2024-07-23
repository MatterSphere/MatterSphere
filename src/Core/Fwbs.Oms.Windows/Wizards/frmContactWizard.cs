using System;
using System.Windows.Forms;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A specialised wizard form that will create a new contact.
    /// </summary>
    internal class frmContactWizard : frmOMSTypeWizard
	{
		#region Fields

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// A type picker list.
		/// </summary>
		private IBasicEnquiryControl2 _type = null;

		/// <summary>
		/// Holds the sub contact code.
		/// </summary>
		private IBasicEnquiryControl2 _subType = null;

		/// <summary>
		/// The return contact object used from the wizard.
		/// </summary>
		private Contact _obj = null;

		/// <summary>
		/// The oms type object to manipulate once on is chosen.
		/// </summary>
		private ContactType _omsType = null;

		/// <summary>
		/// Override any Search if True
		/// </summary>
		private bool _skipsearch = false;

		#endregion

		#region Contructors & Destructors

		/// <summary>
		/// Default contructor not used.
		/// </summary>
		private frmContactWizard() : base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		/// <summary>
		/// Creates a new intance of the contact wizard form.
		/// </summary>
		/// <param name="param">Any optional parameters needed to populate values.</param>
		/// <param name="wizardStyle">Parameter indicating Wizard style.</param>
		public frmContactWizard(Common.KeyValueCollection param, WizardStyle wizardStyle = WizardStyle.Dialog) : base(wizardStyle)
		{
            InitializeEnquiryForm(SystemForms.ContactTypePicker, null, true, param);

			enquiryForm1.ChangeParentFormSize = false;
			//Get the conflict searcher control.
			_type =  enquiryForm1.GetIBasicEnquiryControl2("_type",EnquiryControlMissing.Exception);
			_subType =  enquiryForm1.GetIBasicEnquiryControl2("_subType",EnquiryControlMissing.Exception);
			
			//Type selector.
			if (_type != null)
			{
				_type.ActiveChanged += new EventHandler(this.TypeChanged);
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            // 
            // frmContactWizard
            // 
            this.Name = "frmContactWizard";
            this.Text = "frmContactWizard";
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
				//Make sure the header panel is visible.
                ShowHeaderPanel(!IsConflictSearchVisible());

				//Run the base forms event first.
				base.enquiryForm1_PageChanged(sender, e);

				//Make sure that the conflict search has its search button pressed by default.
				//Change the form caption to Contact Conflict search as well.
				if (IsConflictSearchVisible())
				{
					this.Text = Session.CurrentSession.Resources.GetResource("CONTCONFLICTSCH","Contact Conflict Search","").Text;
                    SetConflictSearchDefaults(new Common.KeyValueCollection());
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
			if ((e.PageName == "CONTPICK" && e.Direction == EnquiryPageDirection.Back) || (e.PageType == EnquiryPageType.Start && e.Direction == EnquiryPageDirection.Next))
			{
				if (_skipsearch == false)
				{
                    SetConflictSearchList(SystemSearchListGroups.ContactConflict, null);
				}
				else
				{
					if (e.Direction == EnquiryPageDirection.Next)
					{
						enquiryForm1.GotoPage("CONTPICK",false);
						e.Cancel=true;
					}
					else
					{
						enquiryForm1.GotoPage((short)-1,false);
						enquiryForm1.GotoWelcomePage();
						e.Cancel=true;
					}
				}
			}
			else if (e.PageName == "CONFLICT" && e.Direction == EnquiryPageDirection.Next)
			{
				if (_conflictSearches.Count == 0)
				{
					//Make sure that the user is aware that a conflict search is required.
					if (MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("RUSCONTACT","Are you sure that you do not want to check to see if the Contact exists before continuing?","") , FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
					{
						e.Cancel = true;
						return;
					}
				}
			}
			else if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Next && e.PageName == "CONTPICK")
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
						_omsType = ContactType.GetContactType(type);
						_obj = (Contact)_omsType.CreateObject(null);
						if (_subType != null) _obj.AdditionalFilter = Convert.ToString(_subType.Value);
						objchanged = true;
					}

                    InitializeCustomForm(_omsType.Wizard, SystemForms.ContactWizard, _obj, objchanged);
				}

				catch (Exception ex)
				{
					e.Cancel = true;
					ErrorBox.Show(this, ex);
				}
			}
		}

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (wizardStyle == WizardStyle.InPlace)
            {
                UpdateFinishButtonText(Session.CurrentSession
                    .Resources.GetResource("CREATEENTITY", "Create %1%", string.Empty, true, "%CONTACT%").Text);
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
		private void TypeChanged (object sender, EventArgs e)
		{
			//Disable the next button if the value chosen from the type list is
			//DBNull.
			if (enquiryForm1.PageNumber > -1)
				btnNext.Enabled = (_type.Value != DBNull.Value);

			if (_subType != null && _subType is IListEnquiryControl )
				((IListEnquiryControl)_subType).Filter("cdcode", Convert.ToString(_type.Value));
		}

		/// <summary>
		/// This method gets run after an item has been selected from the conflict search list.
		/// </summary>
		/// <param name="sender">Conflict Search control</param>
		/// <param name="returnValues">Conflict search return values.</param>
		protected override void OnConflictItemSelected(object sender, Common.KeyValueCollection returnValues)
		{
			_obj = Contact.GetContact(Convert.ToInt64(returnValues["contid"].Value));
            this.Tag = _obj;
			this.DialogResult = TopLevel ? DialogResult.OK : DialogResult.Yes;
            enquiryForm1_Finished(sender, EventArgs.Empty);
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

		#region Properties

		/// <summary>
		/// Gets the contact returned from the wizard.
		/// </summary>
		public Contact Contact
		{
			get
			{
				return _obj;
			}
		}

		public bool SkipSearch
		{
			get
			{
				return _skipsearch;
			}
			set
			{
				_skipsearch = value;
			}
		}

		#endregion
	}
}
