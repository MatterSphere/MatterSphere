using System;
using System.ComponentModel;
using FWBS.Common.UI;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// An intelligent associate wizard form.
    /// </summary>
    internal class frmAssociateRemoteWizard : frmWizard
	{
		#region Fields

		/// <summary>
		/// The file to be manipulated.
		/// </summary>
		private OMSFile _file = null;

		/// <summary>
		/// The associate object that is to be created.
		/// </summary>
		private Associate _associate = null;

		/// <summary>
		/// A reference to the contact search screen.
		/// </summary>
		private ucSearchControl _search = null;

		/// <summary>
		/// A flag to determine whether to show the search screen.
		/// </summary>
		private bool _showSearch = true;
		
		private IBasicEnquiryControl2 _txtemail = null;

		private IBasicEnquiryControl2 _txtusername = null;

		private IBasicEnquiryControl2 _pwdpassword = null;

		private IBasicEnquiryControl2 _txtsmsnumber = null;

		private IBasicEnquiryControl2 _chkremaccsec1 = null;

		private IBasicEnquiryControl2 _chkremaccsec2 = null;

		private IBasicEnquiryControl2 _chkremaccsec3 = null;
		
		private IBasicEnquiryControl2 _chkremaccsec4 = null;
		
		private IBasicEnquiryControl2 _chkremaccsec5 = null;

		private IBasicEnquiryControl2 _chkremaccinformchan = null;




		#endregion

		#region Constructors & Destructors

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Default constructor not used.
		/// </summary>
		private frmAssociateRemoteWizard()
		{
		}

		/// <summary>
		/// Creates a new associate with the specified file and contact.
		/// </summary>
		/// <param name="file">The oms file to be associated.</param>
		/// <param name="contact">The contact to be associated to the file.</param>
		/// <param name="param">Extra parameters to be passed if need be.</param>
		public frmAssociateRemoteWizard(OMSFile file, Common.KeyValueCollection param) : this()
		{
			_file = file;
			enquiryForm1.Enquiry = Enquiry.GetEnquiry(Session.CurrentSession.DefaultSystemForm(SystemForms.RemoteAccountWizard), _file, EnquiryMode.Add, false, param);
			enquiryForm1.Finishing +=new CancelEventHandler(enquiryForm1_Finishing);
			_associate = enquiryForm1.Enquiry.Object as Associate;
			_search = enquiryForm1.GetControl("_search",EnquiryControlMissing.Exception) as ucSearchControl;
			if (_search != null)
			{
				_search.ItemSelected += new EventHandler(this._search_ItemSelected);
				_search.StateChanged += new SearchStateChangedEventHandler(this._search_StateChanged);
			}

			_txtemail = enquiryForm1.GetIBasicEnquiryControl2("txtEmail",EnquiryControlMissing.Exception);
			_txtusername = enquiryForm1.GetIBasicEnquiryControl2("txtUserName",EnquiryControlMissing.Exception);
			_pwdpassword = enquiryForm1.GetIBasicEnquiryControl2("pwdPassword",EnquiryControlMissing.Exception);
			_txtsmsnumber = enquiryForm1.GetIBasicEnquiryControl2("txtSMSNumber",EnquiryControlMissing.Exception);
			_chkremaccsec1 = enquiryForm1.GetIBasicEnquiryControl2("chkRemAccSec1",EnquiryControlMissing.Exception);
			_chkremaccsec2 = enquiryForm1.GetIBasicEnquiryControl2("chkRemAccSec2",EnquiryControlMissing.Exception);
			_chkremaccsec3 = enquiryForm1.GetIBasicEnquiryControl2("chkRemAccSec3",EnquiryControlMissing.Exception);
			_chkremaccsec4 = enquiryForm1.GetIBasicEnquiryControl2("chkRemAccSec4",EnquiryControlMissing.Exception);
			_chkremaccsec5 = enquiryForm1.GetIBasicEnquiryControl2("chkRemAccSec5",EnquiryControlMissing.Exception);
			_chkremaccinformchan = enquiryForm1.GetIBasicEnquiryControl2("chkRemAccInformChan",EnquiryControlMissing.Exception);
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
		#endregion

		#region Methods

		/// <summary>
		/// Validates the contact for the associate by updating the wizard information with information.
		/// </summary>
		private void ValidateContact()
		{
		}

		#endregion

		#region Captured Events
		/// <summary>
		/// This method gets run after an item has been selected from the search list.
		/// </summary>
		/// <param name="sender">Search control</param>
		/// <param name="e">Empty event arguments.</param>
		private void _search_ItemSelected(object sender, System.EventArgs e)
		{
			try
			{
				_associate = Associate.GetAssociate(Convert.ToInt64(_search.ReturnValues["associd"].Value));

				enquiryForm1.GetIBasicEnquiryControl2("contID").Value=_associate.Contact.ID;

				_txtusername.Value = _associate.Contact.ExtendedData["EXTREMACCCONT"].GetExtendedData("proUserName");
				if (Convert.ToString(_txtusername.Value) == "")
					_txtusername.Value = _associate.Contact.Name;

				_txtemail.Value = _associate.Contact.ExtendedData["EXTREMACCCONT"].GetExtendedData("proEmail");
				if (Convert.ToString(_txtemail.Value) == "")
					_txtemail.Value = _associate.DefaultEmail;

				_txtsmsnumber.Value = _associate.Contact.ExtendedData["EXTREMACCCONT"].GetExtendedData("proSMSNumber");
				if (Convert.ToString(_txtsmsnumber.Value) == "")
					_txtsmsnumber.Value = _associate.DefaultMobile;

				_pwdpassword.Value = _associate.Contact.ExtendedData["EXTREMACCCONT"].GetExtendedData("proPassword");
				_chkremaccinformchan.Value = _associate.Contact.ExtendedData["EXTREMACCCONT"].GetExtendedData("proInformSMS");

				string secvalue = Convert.ToString(_associate.Contact.ExtendedData["EXTREMACCCONT"].GetExtendedData("proDefSecSetting"));
				if (secvalue == "") secvalue = Convert.ToString((Int64)_file.CurrentFileType.RemoteAccSettings,2);
				if (secvalue == "0") secvalue = "00000";
				if (secvalue.Substring(0,1) == "1") _chkremaccsec1.Value = true;
				if (secvalue.Substring(1,1) == "1") _chkremaccsec2.Value = true;
				if (secvalue.Substring(2,1) == "1") _chkremaccsec3.Value = true;
				if (secvalue.Substring(3,1) == "1") _chkremaccsec4.Value = true;
				if (secvalue.Substring(4,1) == "1") _chkremaccsec5.Value = true;
	

			}
			catch (Exception ex)
			{
				_associate = null;
				ErrorBox.Show(this, ex);
			}
			finally
			{
				btnNext.Enabled = (_associate != null);
				ValidateContact();
			}
		}

		/// <summary>
		/// Captures the search state changed.  This is used to set the different accept keys
		/// depending on the state.
		/// </summary>
		/// <param name="sender">Search control.</param>
		/// <param name="e">Search state event arguments.</param>
		private void _search_StateChanged(object sender, FWBS.OMS.UI.Windows.SearchStateEventArgs e)
		{
			if (e.State == SearchState.Search && _search.cmdSearch != null)
			{
				this.AcceptButton = _search.cmdSearch;
			}
			else if (e.State == SearchState.Select && _search.cmdSelect != null)
			{
				this.AcceptButton = btnNext;
			}
		}

		/// <summary>
		/// Captures the enquiry form wizard before page change event.
		/// </summary>
		/// <param name="sender">The enqiry form reference.</param>
		/// <param name="e">Page changing event arguments.</param>
		protected override void enquiryForm1_PageChanging(object sender, FWBS.OMS.UI.Windows.PageChangingEventArgs e)
		{
			base.enquiryForm1_PageChanging(sender, e);

			//Skip the search screen if a contact was originally specified.
			if (e.PageType == EnquiryPageType.Start && e.Direction == EnquiryPageDirection.Next)
			{
				if (_search == null || _showSearch == false)
				{
					enquiryForm1.GotoPage((short)(e.PageNumber + 2), false);
					e.Cancel = true;
					return;
				}
				else
				{
					btnNext.Enabled = (_associate != null);

					if (_associate == null)
					{
						if (_search != null)
						{
							if (_search.SearchList == null)
								_search.SetSearchList(Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.RemoteAssociates),_file,null);
							_search.SearchButtonCommands +=new SearchButtonEventHandler(_search_SearchButtonCommands);
						}
					}
				}
			}
			//Skip the search screen if a contact was originally specified.
			else if (e.Direction == EnquiryPageDirection.Back && e.PageNumber == 1)
			{
				if (_search == null || _showSearch == false)
				{
					enquiryForm1.GotoPage((short)(e.PageNumber - 2), false);
					e.Cancel = true;
					return;
				}
			}
			//Select the item from the contact search list ready for the next stage.
			else if (e.PageType == EnquiryPageType.Enquiry && e.Direction == EnquiryPageDirection.Next && e.PageNumber == 0)
			{
				if (_search != null && _showSearch == true)
				{
					_search.SelectRowItem();
					
					if (_associate != null)
					{
						if (_associate == null)
						{
							e.Cancel = true;
							return;
						}
						else
						{
						}
					}
					else
					{
						e.Cancel = true;
						return;
					}
				}
			}
		}

		/// <summary>
		/// Captures the after page change of the wizard.
		/// </summary>
		/// <param name="sender">The enqiry form reference.</param>
		/// <param name="e">Page changed event arguments.</param>
		protected override void enquiryForm1_PageChanged(object sender, FWBS.OMS.UI.Windows.PageChangedEventArgs e)
		{
			base.enquiryForm1_PageChanged(sender, e);

			//Make sure the header panel is visible.
            ShowHeaderPanel(_search == null || !_search.Visible);

			if (_search != null && _search.Visible)
			{
				this.AcceptButton = _search.cmdSearch;
			}

			if (e.PageName == "REMOTE")
			{
				if (enquiryForm1.GetControl("txtEmail") is eEmailSelector)
					((eEmailSelector)enquiryForm1.GetControl("txtEmail")).Reload(_associate.Contact);
			}

		}

	
		private void _search_SearchButtonCommands(object sender, SearchButtonEventArgs e)
		{

		}

		private void enquiryForm1_Finishing(object sender, CancelEventArgs e)
		{
			_associate = Associate.GetAssociate(Convert.ToInt64(_search.ReturnValues["associd"].Value));

			string secvalue = Convert.ToString(_associate.Contact.ExtendedData["EXTREMACCCONT"].GetExtendedData("proDefSecSetting"));
			if (secvalue == "") 
			{
				if (Convert.ToBoolean(_chkremaccsec1.Value)) secvalue += "1"; else secvalue += "0";
				if (Convert.ToBoolean(_chkremaccsec2.Value)) secvalue += "1"; else secvalue += "0";
				if (Convert.ToBoolean(_chkremaccsec3.Value)) secvalue += "1"; else secvalue += "0";
				if (Convert.ToBoolean(_chkremaccsec4.Value)) secvalue += "1"; else secvalue += "0";
				if (Convert.ToBoolean(_chkremaccsec5.Value)) secvalue += "1"; else secvalue += "0";
				_associate.Contact.ExtendedData["EXTREMACCCONT"].SetExtendedData("proDefSecSetting",secvalue);
			}

			_associate.Contact.ExtendedData["EXTREMACCCONT"].SetExtendedData("proUserName",_txtusername.Value);
			_associate.Contact.ExtendedData["EXTREMACCCONT"].SetExtendedData("proEmail",_txtemail.Value);
			_associate.Contact.ExtendedData["EXTREMACCCONT"].SetExtendedData("proSMSNumber",_txtsmsnumber.Value);
			_associate.Contact.ExtendedData["EXTREMACCCONT"].SetExtendedData("proInformSMS",_chkremaccinformchan.Value);
			_associate.Contact.ExtendedData["EXTREMACCCONT"].SetExtendedData("proPassword",_pwdpassword.Value);

			_associate.Contact.Update();
		}
		#endregion
	}
}
