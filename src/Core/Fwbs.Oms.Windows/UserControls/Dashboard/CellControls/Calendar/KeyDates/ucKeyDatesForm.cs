using System;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.KeyDates
{
    public class ucKeyDatesForm : ucOmsItem
    {
        private DateWizard _currentWizard = null;
        private OMSFile _file = null;
        private FWBS.Common.UI.IBasicEnquiryControl2 _group;
        private FWBS.Common.UI.IBasicEnquiryControl2 _type;
        private FWBS.Common.UI.IBasicEnquiryControl2 _feeEarner;
        private FWBS.Common.UI.IBasicEnquiryControl2 _notes;
        private FWBS.Common.UI.Windows.eLabel2 _caption;
        private DateWizardForm _dateWiz;

        internal ucKeyDatesForm(string code, object parent, EnquiryMode mode, bool offline, FWBS.Common.KeyValueCollection param) : base(code, parent, mode, offline, param)
        {
        }

        protected override void Setup(string enquiryCode, EnquiryMode mode, object parent, FWBS.Common.KeyValueCollection param)
        {
            base.Setup(enquiryCode, mode, parent, param);
            _file = parent as OMSFile;
            InitializeControls();
        }

        public override void UpdateItem()
        {
            _dateWiz.UpdateItem();
            base.UpdateItem();
        }

        private void InitializeControls()
        {
            _group = enquiryForm1.GetIBasicEnquiryControl2("_group", EnquiryControlMissing.Exception);
            _type = enquiryForm1.GetIBasicEnquiryControl2("_type", EnquiryControlMissing.Exception);
            _feeEarner = enquiryForm1.GetIBasicEnquiryControl2("_feeEarner", EnquiryControlMissing.Exception);
            _notes = enquiryForm1.GetIBasicEnquiryControl2("_notes", EnquiryControlMissing.Exception);
            _dateWiz = enquiryForm1.GetControl("_dateWizard", EnquiryControlMissing.Exception) as DateWizardForm;
            _caption = enquiryForm1.GetControl("_caption", EnquiryControlMissing.Exception) as FWBS.Common.UI.Windows.eLabel2;
            
            _group.ActiveChanged += new EventHandler(_type_Changed);
            _type.ActiveChanged += new EventHandler(_type_Changed);
            _feeEarner.ActiveChanged += new EventHandler(_feeEarner_Changed);
        }

        private void _type_Changed(object sender, EventArgs e)
        {
            if (_dateWiz != null)
            {
                if (_type == null || _group == null || _type.Value == DBNull.Value || _group.Value == DBNull.Value)
                {
                    _dateWiz.Visible = false;
                    _caption.Visible = false;
                }
                else
                {
                    _dateWiz.Visible = true;
                    _caption.Visible = true;
                }

                if (_dateWiz.Visible)
                {
                    _dateWiz.DateWizard = BuildDateWizard();
                }
            }
        }

        private void _feeEarner_Changed(object sender, EventArgs e)
        {
            if (_currentWizard != null && _feeEarner.Value != DBNull.Value)
            {
                var feeEarnerField = typeof(DateWizard).GetField("_feeEarner", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                if (feeEarnerField != null)
                    feeEarnerField.SetValue(_currentWizard, GetFeeEarner());
            }
        }

        private DateWizard BuildDateWizard()
        {
            var code = Convert.ToString(_type.Value);
            var feeEarner = GetFeeEarner();
            var notes = Convert.ToString(_notes?.Value);

            return InitializeForm(code, feeEarner, notes);
        }

        private FeeEarner GetFeeEarner()
        {
            FeeEarner fee = Session.CurrentSession.CurrentFeeEarner;
            int feeid = fee.ID;
            if (_feeEarner != null && _feeEarner.Value != DBNull.Value)
            {
                feeid = Convert.ToInt32(_feeEarner.Value);
            }

            if (feeid != fee.ID)
            {
                fee = FeeEarner.GetFeeEarner(feeid);
            }

            return fee;
        }

        private DateWizard InitializeForm(string code, FeeEarner fee, string notes)
        {
            if (code != "" && _file != null && (_currentWizard == null || _currentWizard.Code != code))
            {
                _currentWizard = DateWizard.GetDateWizard(code, _file, fee);
                _currentWizard.CreateNotes(notes);
                if (_caption != null)
                {
                    _caption.Text = _currentWizard.Description;
                }
            }

            return _currentWizard;
        }
    }
}
