using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// A wizard that displays a datewizard configuration object.  The date wizard could be
    /// based on a system enquiry form with script, or from a database driven auto rendered
    /// wizard control.
    /// </summary>
    internal class frmDateWizard : frmWizard
	{
		#region Fields

		/// <summary>
		/// The current date wizard being used.
		/// </summary>
		private DateWizard _currentWizard = null;

		/// <summary>
		/// The file asosciated to the date wizard.
		/// </summary>
		private OMSFile _file = null;

		#endregion
		
		#region Control Fields


		private FWBS.Common.UI.IBasicEnquiryControl2 _group;
		private FWBS.Common.UI.IBasicEnquiryControl2 _type;
		private FWBS.Common.UI.IBasicEnquiryControl2 _feeEarner;
		private FWBS.Common.UI.IBasicEnquiryControl2 _notes;
		private FWBS.Common.UI.Windows.eCaptionLine _caption;
		private FWBS.OMS.UI.Windows.DateWizardForm _dateWiz;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Contructors & Destrucotrs

		/// <summary>
		/// Default contructor.
		/// </summary>
		private frmDateWizard(){}

		/// <summary>
		/// Creates a date wizard form.
		/// </summary>
		/// <param name="file">The file that the date will be created as.</param>
		public frmDateWizard(OMSFile file)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//Fetch the header form and the controls needed to be used from it.
			enquiryForm1.Enquiry = EnquiryEngine.Enquiry.GetEnquiry(Session.CurrentSession.DefaultSystemForm(SystemForms.DateWizard), file, EnquiryEngine.EnquiryMode.Add, true, null);
			_group = enquiryForm1.GetIBasicEnquiryControl2("_group",EnquiryControlMissing.Exception);
			_type = enquiryForm1.GetIBasicEnquiryControl2("_type",EnquiryControlMissing.Exception);
			_feeEarner = enquiryForm1.GetIBasicEnquiryControl2("_feeEarner",EnquiryControlMissing.Exception);
			_notes = enquiryForm1.GetIBasicEnquiryControl2("_notes",EnquiryControlMissing.Exception);
			_dateWiz = enquiryForm1.GetControl("_dateWizard",EnquiryControlMissing.Exception) as DateWizardForm;
			_caption = enquiryForm1.GetControl("_caption",EnquiryControlMissing.Exception) as FWBS.Common.UI.Windows.eCaptionLine;
			_file = file;

			if (_group == null)
			{
				throw new OMSException2("ERDATEWIZGRPMIS", "The date wizard group list is not available.");
			}
			else
				_group.ActiveChanged += new EventHandler(_type_Changed);
			
			if (_type == null)
			{
    			throw new OMSException2("ERDATEWIZTYPMIS", "The date wizard type list is not available.");
			}
			else
				_type.ActiveChanged +=new EventHandler(_type_Changed);

			

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
            // enquiryForm1
            // 
            this.enquiryForm1.Finishing += new System.ComponentModel.CancelEventHandler(this.enquiryForm1_Finishing);
            // 
            // frmDateWizard
            // 
            this.Name = "frmDateWizard";
            this.Text = "frmDateWizard";
		}
		#endregion

		#endregion

		#region Captured Events


		/// <summary>
		/// Captures the enquiry form after page changed event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected override void enquiryForm1_PageChanged(object sender, PageChangedEventArgs e)
		{
			base.enquiryForm1_PageChanged (sender, e);

			if (_dateWiz != null)
			{
				if (_dateWiz.Visible)
				{
                    FeeEarner fee = Session.CurrentSession.CurrentFeeEarner;
					int feeid = fee.ID;
					if (_feeEarner != null)
					{
						if (_feeEarner.Value != DBNull.Value)
							feeid = Convert.ToInt32(_feeEarner.Value);

					}

					if (feeid != fee.ID)
					{
						fee = FeeEarner.GetFeeEarner(feeid);
					}

					string code = Convert.ToString(_type.Value);
					if (code != "" && (_currentWizard == null || _currentWizard.Code != code))
					{
						_currentWizard = DateWizard.GetDateWizard(code, _file, fee);
						if (_caption != null) _caption.Text = _currentWizard.Description;
					}
					_dateWiz.DateWizard = _currentWizard;
				}
			}

			if (e.PageType == EnquiryPageType.Start)
			{
				enquiryForm1.ActionNext.Enabled = true;
			}
			else
			{
				if (((Control)_type).Visible)
				{				   
					if (_type == null || _group == null || _type.Value == DBNull.Value || _group.Value == DBNull.Value)
						enquiryForm1.ActionNext.Enabled = false;
					else
						enquiryForm1.ActionNext.Enabled = true;
				}
			}
		}

		/// <summary>
		/// Captures the wizards finished event so that the keydates can be created.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void enquiryForm1_Finishing(object sender, CancelEventArgs e)
		{
			try
			{
                _dateWiz.CreateWizardNotes(_notes?.Value.ToString());
                _dateWiz.UpdateItem();
			}
			catch (Exception ex)
			{
				e.Cancel = true;
				ErrorBox.Show(this, ex);
			}
		}

		/// <summary>
		/// Captures the group and type change values.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void _type_Changed(object sender, EventArgs e)
		{
			if (_type == null || _group == null || _type.Value == DBNull.Value || _group.Value == DBNull.Value)
				enquiryForm1.ActionNext.Enabled = false;
			else
				enquiryForm1.ActionNext.Enabled = true;
		}

		#endregion


	}
}
