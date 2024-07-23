using System;
using System.Windows.Forms;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A specialised wizard form that will create a new contact.
    /// </summary>
    internal class frmFileWizard : frmOMSTypeWizard
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
		/// The return contact object used from the wizard.
		/// </summary>
		private OMSFile _obj = null;

		/// <summary>
		/// The oms type object to manipulate once on is chosen.
		/// </summary>
		private FileType _omsType = null;
		
		/// <summary>
		/// The default contact to use when creating a client.
		/// </summary>
		private Client _defClient = null;

		/// <summary>
		/// The department combo box.
		/// </summary>
		private IBasicEnquiryControl2 _dept = null;

		/// <summary>
		/// A checkbox which accepts whether to display the file after it has been created.
		/// </summary>
		private IBasicEnquiryControl2 _view = null;

		/// <summary>
		/// A date picker which accepts a file review date.
		/// </summary>
		private IBasicEnquiryControl2 _reviewDate = null;

		/// <summary>
		/// A variable which decides to view the file after use or not.
		/// </summary>
		private bool _viewFile = false;


		#endregion

		#region Contructors & Destructors

		/// <summary>
		/// Default contructor not used.
		/// </summary>
		private frmFileWizard() : base()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

		}

		
		/// <summary>
		/// Creates an OMS File by running the file take on wizard.
		/// </summary>
		/// <param name="defaultClient">The default client to use.</param>
		/// <param name="param">Parameters that may be needed to replace items in the enquiry to run properly.</param>
		/// <param name="wizardStyle">Parameter indicating Wizard style.</param>
		public frmFileWizard(Client defaultClient, Common.KeyValueCollection param, WizardStyle wizardStyle = WizardStyle.Dialog) : base(wizardStyle)
		{
            InitializeEnquiryForm(SystemForms.FileTypePicker, defaultClient, true, param);

			//Get the conflict searcher control.
			_type =  enquiryForm1.GetIBasicEnquiryControl2("_type",EnquiryControlMissing.Exception);
			
			//Type selector.
			if (_type != null)
			{
				_type.ActiveChanged += new EventHandler(this.TypeChanged);
			}

			_defClient = defaultClient;
			_dept = enquiryForm1.GetIBasicEnquiryControl2("_dept",EnquiryControlMissing.Exception);
			Control clientNo = enquiryForm1.GetControl("_clientNo");
			Control clientDesc = enquiryForm1.GetControl("_clientDesc",EnquiryControlMissing.Exception);
			
			if (clientNo != null)
			{
				clientNo.Text = defaultClient.ClientNo + " (" + defaultClient.ClientTypeDescription + ")";
			}

			IBasicEnquiryControl2 cldesc = clientDesc as IBasicEnquiryControl2;
			if (cldesc != null)
			{
				cldesc.Value = defaultClient.ClientDescription;
			}

			//Gets a department combo.
			if (_dept != null)
			{
				Control dept = (Control)_dept;
				dept.VisibleChanged += new EventHandler(this.VisibilityChange);
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
            // frmFileWizard
            // 
            this.Name = "frmFileWizard";
            this.Text = "File Takeon Wizard";
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

				//If the wizard is on its last custom page then show the associated enquiry form.
				if (e.PageType == EnquiryPageType.Custom && e.Direction == EnquiryPageDirection.Next)
				{
					if (_obj != null)
					{
						foreach (SearchCompletedEventArgs conflicts in _conflictSearches)
						{
							_obj.ApplyConflictSearch(conflicts.Count, conflicts.Criteria);
						}

						if (Session.CurrentSession.IsPackageInstalled("SECURITY"))
						{
                            //UTCFIX: DM - 30/11/06
							if (_omsType.SecurityAudit && _obj.Client.TrackingStamp.Created.ToLocalTime().ToShortDateString() != DateTime.Today.ToShortDateString())
							{
								object seccheck = null;
								foreach (FWBS.OMS.Contact _contact in _obj.Client.Contacts)
								{
									if (_contact.CurrentContactType.GeneralType == OMSTypeContactGeneralType.Individual)
									{
										try
										{
											Common.KeyValueCollection _params = new FWBS.Common.KeyValueCollection();
											_params.Add("OMSFILE",_obj);
											seccheck = Services.Wizards.GetWizard(this,Session.CurrentSession.DefaultSystemForm(SystemForms.SecurityCheck),_contact,EnquiryEngine.EnquiryMode.Edit,false,_params,Modal || !TopLevel);
										}
										catch
										{
											// If the Wizard Can't be found break
											break;
										}
									}
								}
							}
						}
					}

				}
				//Run the base forms event first.
				base.enquiryForm1_PageChanged(sender, e);

				if (IsConflictSearchVisible())
				{
					this.Text = Session.CurrentSession.Resources.GetResource("FILECONFLICTSCH","Conflict Search","").Text;

					//If the search form has the client type search control
					//then set the default value to the client type chosen.
                    Common.KeyValueCollection defaults = new Common.KeyValueCollection();
                    defaults.Add("@CLTYPE", _obj.Client.ClientTypeCode);
                    defaults.Add("@FILETYPE", _omsType.Code);
                    SetConflictSearchDefaults(defaults);
				}
				else
					this.Text = labWelcome.Text;
			}
			catch (Exception ex)
			{
				ErrorBox.Show(this, ex);
			}
			finally
			{
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
						enquiryForm1.req.SetError((Control)_type,"");
						enquiryForm1.err.SetError((Control)_type,Session.CurrentSession.Resources.GetResource("REQFIELD","This field is required.","").Text);
						e.Cancel = true;
						return;
					}

					bool objchanged = false;

					if (_omsType == null || _omsType.Code != type) 
					{
						_omsType = FileType.GetFileType(type);
						_obj = (OMSFile)_omsType.CreateObject(new object [1]{_defClient});
						objchanged = true;
					}

                    InitializeCustomForm(_omsType.Wizard, SystemForms.FileWizard, _obj, objchanged);

					_view = custom.GetIBasicEnquiryControl2("_view");
					_reviewDate = custom.GetIBasicEnquiryControl2("_reviewDate");

					if (_omsType.SearchOnCreate)
					{
                        SetConflictSearchList(SystemSearchListGroups.ClientConflict, _defClient);
					}
					else
					{
						enquiryForm1.GotoPage((short)(e.PageNumber + 2), false);
						e.Cancel = true;
						return;
					}

					if (_reviewDate != null)
						((Control)_reviewDate).Visible = _omsType.EnableFileReview;

				}

				catch (Exception ex)
				{
					e.Cancel = true;
					ErrorBox.Show(this, ex);
				}
			}
			else if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Back && e.PageNumber == 1)
			{
				if (_omsType.SearchOnCreate)
				{
                    SetConflictSearchList(SystemSearchListGroups.ClientConflict, _defClient);
				}
				else
				{
					enquiryForm1.GotoPage((short)(e.PageNumber - 2), false);
					e.Cancel = true;
					return;
				}
			}
			else if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Next && e.PageNumber == 1)
			{
				if (_conflictSearches.Count == 0)
				{
					//Make sure that the user is aware that a conflict search is required.
					if (MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("4003","Are you sure that you do not want to check to see if the conflicting party exists before continuing?","") , FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
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
						if (MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("4004","Are you sure that you would like to continue with creating the %CLIENT%, even if there were %1% %FILES% found within the last search made?","", sch.Count.ToString()), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
						{
							e.Cancel = true;
							return;
						}
					}
				}

			}
			else if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Next && e.PageNumber == 2)
			{
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
			this.btnFinished.Enabled=false;
		}

		/// <summary>
		/// This method gets run after an item has been selected from the conflict search list.
		/// </summary>
		/// <param name="sender">Conflict Search control</param>
		/// <param name="returnValues">Conflict search return values.</param>
		protected override void OnConflictItemSelected(object sender, Common.KeyValueCollection returnValues)
		{
			_obj = OMSFile.GetFile(Convert.ToInt64(returnValues["fileid"]));
            this.Tag = _obj;
			this.DialogResult = TopLevel ? DialogResult.OK : DialogResult.Yes;
            enquiryForm1_Finished(sender, EventArgs.Empty);
		}

		
		/// <summary>
		/// This method gets run the department of type combo get made visible.
		/// </summary>
		/// <param name="sender">Departnemt or type combo.</param>
		/// <param name="e">Empty event arguments.</param>
		private void VisibilityChange(object sender, System.EventArgs e)
		{
			if (sender == _dept)
			{
				Control dept = (Control)_dept;
				if (dept.Visible)
				{
					try
					{
						_dept.Value = Session.CurrentSession.CurrentFeeEarner.DefaultDepartment;
					}
					catch(Exception ex)
					{
						throw new OMSException2("ERRNOFE","Your user settings 'Work For' have not been set.","",ex);
					}
					dept.VisibleChanged -= new EventHandler(this.VisibilityChange);
					if (_type != null)
					{
						Control type = (Control)_type;
						//Set the default file type.
						if (type.Visible)
						{
							_type.Value = Session.CurrentSession.CurrentFeeEarner.DefaultFileType;
						}
					}
					
				}

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
            if (TopLevel)
            {
                MessageBox.Show(this, Session.CurrentSession.Resources.GetMessage("4005", "Your New %FILE% Number for '%2% (%3%)' is : %1%", "", true, OMSFile.FileNo, OMSFile.Client.ClientName, OMSFile.Client.ClientNo), null, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                if (_view != null)
                {
                    _viewFile = Common.ConvertDef.ToBoolean(_view.Value, false);
                }
            }
            base.custom_Finished(sender, e);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the contact returned from the wizard.
		/// </summary>
		public OMSFile OMSFile
		{
			get
			{
				return _obj;
			}
		}

		/// <summary>
		/// Gets the view file property.
		/// </summary>
		public bool ViewFile
		{
			get
			{
				return _viewFile;
			}
		}


		#endregion
	}
}