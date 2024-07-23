using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.Common.UI;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for frmCDSWizard.
    /// </summary>
    internal class frmUFNWizard : FWBS.OMS.UI.Windows.frmWizard
	{
		#region Fields
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		IBasicEnquiryControl2 _xpcNewUFN = null;
		IBasicEnquiryControl2 _xpcIsLeadUFN = null;
		FWBS.OMS.UI.Windows.ucSearchControl _slFindUFN = null;
		FWBS.OMS.UI.Windows.ucSearchControl _slNotLead = null;
		FWBS.OMS.OMSFile _file = null;
		FWBS.OMS.UFN _ufn = null;
		Control _dpDateUFNFrom = null;

		#endregion

		#region Constructors
		public frmUFNWizard(OMSFile file)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			_file = file;
			_ufn = new FWBS.OMS.UFN(file);
			enquiryForm1.Enquiry = Enquiry.GetEnquiry(Session.CurrentSession.DefaultSystemForm(FWBS.OMS.SystemForms.UFNInformation),null,_ufn,new FWBS.Common.KeyValueCollection());
			enquiryForm1.PageChanging +=new PageChangingEventHandler(enquiryForm_PageChanging);
			enquiryForm1.PageChanged +=new PageChangedEventHandler(enquiryForm1_PageChanged);
			enquiryForm1.Finishing +=new CancelEventHandler(ufnWizard_Finishing);

			_ufn = enquiryForm1.Enquiry.Object as FWBS.OMS.UFN;

			_xpcNewUFN = enquiryForm1.GetIBasicEnquiryControl2("xpcNewUFN");
			_xpcNewUFN.ActiveChanged +=new EventHandler(_xpcNewUFN_ActiveChanged);
			_xpcIsLeadUFN = enquiryForm1.GetIBasicEnquiryControl2("xpcIsLeadUFN");
			_xpcIsLeadUFN.ActiveChanged +=new EventHandler(_xpcNewUFN_ActiveChanged);

			_slFindUFN = enquiryForm1.GetControl("slFindUFN") as FWBS.OMS.UI.Windows.ucSearchControl;
			_slNotLead = enquiryForm1.GetControl("slNotLead") as FWBS.OMS.UI.Windows.ucSearchControl;
			_slFindUFN.SearchCompleted +=new SearchCompletedEventHandler(slFindUFN_SearchCompleted);
			_slNotLead.SearchCompleted +=new SearchCompletedEventHandler(slNotLead_SearchCompleted);
			_dpDateUFNFrom = enquiryForm1.GetControl("dpDateUFNFrom");
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            // 
            // frmUFNWizard
            // 
            this.Name = "frmUFNWizard";
            this.Text = "frmCDSWizard";
		}
		#endregion

		#region Private


		private void ufnWizard_Finishing(object sender, CancelEventArgs e)
		{
			if (Convert.ToString(_xpcNewUFN.Value).ToUpper() == "NO")
			{
				enquiryForm1.GetSettings(_dpDateUFNFrom)["quRequired"] = false;
			}
		}

		private void enquiryForm_PageChanged(object sender, PageChangingEventArgs e)
		{	
		}

		private void enquiryForm_PageChanging(object sender, PageChangingEventArgs e)
		{	
			if (e.PageName == "START" && Convert.ToString(_xpcNewUFN.Value).ToUpper() == "NO" && e.Direction == EnquiryPageDirection.Next)
			{
				if (_slFindUFN.SearchList == null)
				{
					_slFindUFN.SetSearchList("SCHFILUFNCLI",_file,new FWBS.Common.KeyValueCollection());
					btnNext.Enabled = false;
				}
				else
					_slFindUFN.Search();
				enquiryForm1.GotoPage("FIND",false);
				e.Cancel=true;
			}
			else if (e.PageName == "FIND" && e.Direction == EnquiryPageDirection.Next)
			{
				_slFindUFN.SelectRowItem();
				_ufn.HeadUFNCode = Convert.ToString(_slFindUFN.ReturnValues[0].Value);
				_ufn.UFNCode = _ufn.HeadUFNCode;
			}
			else if (e.PageName == "NEW" && Convert.ToString(_xpcIsLeadUFN.Value).ToUpper() == "YES" && e.Direction == EnquiryPageDirection.Next)
			{
				enquiryForm1.GotoPage("LAST",false);
				e.Cancel=true;
			}
			else if (e.PageName == "NEW" && Convert.ToString(_xpcIsLeadUFN.Value).ToUpper() == "NO" && e.Direction == EnquiryPageDirection.Next)
			{
				if (_slNotLead.SearchList == null)
				{
					_slNotLead.SetSearchList("SCHFILUFNOCLI",_file,new FWBS.Common.KeyValueCollection());
					btnNext.Enabled = false;
				}
				else
					_slNotLead.Search();
			}
			else if (e.PageName == "NOTLEAD" && e.Direction == EnquiryPageDirection.Next)
			{
				_slNotLead.SelectRowItem();
				_ufn.HeadUFNCode = Convert.ToString(_slNotLead.ReturnValues[0].Value);
				enquiryForm1.GotoPage("LAST",false);
				e.Cancel=true;
			}
		}

		private void _xpcNewUFN_ActiveChanged(object sender, EventArgs e)
		{
			enquiryForm1.ClearForwardHistory();

		}
		#endregion

		private void slNotLead_SearchCompleted(object sender, SearchCompletedEventArgs e)
		{
			btnNext.Enabled = (_slNotLead.SearchList.ResultCount > 0);
		}

		private void slFindUFN_SearchCompleted(object sender, SearchCompletedEventArgs e)
		{
			btnNext.Enabled = (_slFindUFN.SearchList.ResultCount > 0);
		}
	}
}
