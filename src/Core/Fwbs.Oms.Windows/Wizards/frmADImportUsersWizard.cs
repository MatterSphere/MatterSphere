using System.ComponentModel;
using System.Data;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{
    internal class frmADImportUsersWizard : FWBS.OMS.UI.Windows.frmWizard
	{
		private System.ComponentModel.IContainer components = null;
		private DataTable _dtList = null;
		private DataTable _dtSelected = null;
		
		private FWBS.OMS.UI.Windows.eCLCollectionSelector _colUsers;
		private FWBS.Common.UI.Windows.eLabel2 _lblWait;
        private System.Windows.Forms.ComboBox _cboGroups = null;
        private FWBS.Common.UI.Windows.eXPComboBox _cboFeeEarner = null;
        private FWBS.Common.UI.Windows.eXPComboBox _cboBranch = null;
        private FWBS.Common.UI.Windows.eXPComboBox _cboCurrency = null;


		#region Constructors
		public frmADImportUsersWizard()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
		            
			if(Session.CurrentSession.IsLoggedIn)
                enquiryForm1.Enquiry = 
                    Enquiry.GetEnquiry(Session.CurrentSession.DefaultSystemForm(SystemForms.UserImportWizard),null,EnquiryMode.Add,true, null);
			
			enquiryForm1.Finishing +=new CancelEventHandler(enquiryForm1_Finishing);

			_cboFeeEarner = enquiryForm1.GetControl("cboFeeEarner") as FWBS.Common.UI.Windows.eXPComboBox;

            _cboBranch = enquiryForm1.GetControl("cboBranch") as FWBS.Common.UI.Windows.eXPComboBox;
            _cboCurrency = enquiryForm1.GetControl("cboCurrency") as FWBS.Common.UI.Windows.eXPComboBox;


			_colUsers = enquiryForm1.GetControl("colUsers") as FWBS.OMS.UI.Windows.eCLCollectionSelector;
            _colUsers.DisplayMember = "usrFullName";
            _colUsers.ValueMember = "usrInits";
			
            _lblWait = enquiryForm1.GetControl("lblPleaseWait") as FWBS.Common.UI.Windows.eLabel2;

            eADUserSelector selector = enquiryForm1.GetControl("ucADImport1") as eADUserSelector;

            selector.CollectionSelector = _colUsers;

            _cboGroups = enquiryForm1.GetControl("cboGroups") as System.Windows.Forms.ComboBox;


		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		
		#endregion
		
		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Event Handlers
		
		private void enquiryForm1_Finishing(object sender, CancelEventArgs e)
		{
			if(_cboFeeEarner.SelectedValue != System.DBNull.Value)
			{
				if(_colUsers.Value != null)
				{
					//get datatable of results
					_dtSelected = _colUsers.SelectedItems;

                    if (_dtSelected == null)
                        return;
				
					//populate with fee earner values
					foreach(DataRow row in _dtSelected.Rows)
					{
                        if (_cboFeeEarner != null)
                            row["usrWorksFor"] = (int)_cboFeeEarner.SelectedValue;

                        if (_cboBranch != null)
                            row["brID"] = (int)_cboBranch.SelectedValue;

                        if (_cboCurrency != null)
                            row["usrcurISOCode"] = (string)_cboCurrency.SelectedValue;
                        
					}
				}
			}
		}

		#endregion		

		
		#region Properties
		
		/// <summary>
		/// Returns the datable of users
		/// </summary>
		public DataTable Users
		{
			get
			{
				return _dtSelected;
			}
		}
		
		
		#endregion
		
	
		
	}
}

