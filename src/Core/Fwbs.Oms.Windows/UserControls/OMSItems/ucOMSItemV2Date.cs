using System;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{
    public partial class ucOMSItemV2Date : ucOMSItemV2
    {
        /// <summary>
		/// The current date wizard being used.
		/// </summary>
		private DateWizard _currentWizard = null;

        /// <summary>
        /// The file asosciated to the date wizard.
        /// </summary>
        private OMSFile _file = null;

        private Common.UI.IBasicEnquiryControl2 _group;
        private Common.UI.IBasicEnquiryControl2 _type;
        private Common.UI.IBasicEnquiryControl2 _feeEarner;
        private Common.UI.IBasicEnquiryControl2 _notes;
        private Common.UI.Windows.eLabel2 _caption;
        private DateWizardForm _dateWiz;

        internal ucOMSItemV2Date(string code, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param) : base(code, parent, mode, offline, param)
        {
        }

        internal ucOMSItemV2Date(string code, object parent, OMS.Interfaces.IEnquiryCompatible obj, bool offline, Common.KeyValueCollection param) : base(code, parent, obj, offline, param)
        {
        }

        public override bool IsDirty
        {
            get
            {
                return _dateWiz.EnquiryForm.IsDirty || base.IsDirty;
            }
        }

        public override void UpdateItem()
        {
            _dateWiz.UpdateItem();
            base.UpdateItem();
        }

        public override void RefreshItem()
        {
            base.RefreshItem();
            InitializeControls();
            _currentWizard = null;
        }

        protected override void Setup(string enquiryCode, EnquiryMode mode, object parent, Common.KeyValueCollection param)
        {
            base.Setup(enquiryCode, mode, parent, param);
            _file = parent as OMSFile;
            InitializeControls();     
        }

        private void InitializeControls()
        {
            _group = enquiryForm1.GetIBasicEnquiryControl2("_group", EnquiryControlMissing.Exception);
            _type = enquiryForm1.GetIBasicEnquiryControl2("_type", EnquiryControlMissing.Exception);
            _feeEarner = enquiryForm1.GetIBasicEnquiryControl2("_feeEarner", EnquiryControlMissing.Exception);
            _notes = enquiryForm1.GetIBasicEnquiryControl2("_notes", EnquiryControlMissing.Exception);
            _dateWiz = enquiryForm1.GetControl("_dateWizard", EnquiryControlMissing.Exception) as DateWizardForm;
            _caption = enquiryForm1.GetControl("_caption", EnquiryControlMissing.Exception) as Common.UI.Windows.eLabel2;

            _dateWiz.EnquiryForm.Dirty += ValidateFormCompleted;
            _group.ActiveChanged += new EventHandler(_type_Changed);
            _type.ActiveChanged += new EventHandler(_type_Changed);
            _feeEarner.ActiveChanged += new EventHandler(_feeEarner_Changed);
        }

        /// <summary>
        /// Captures the group and type change values.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _type_Changed(object sender, EventArgs e)
        {
            if (_dateWiz != null)
            {
                if (_type == null
                    || _group == null
                    || _type.Value == DBNull.Value
                    || _group.Value == DBNull.Value)
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
            return InitializeForm(code, feeEarner);
        }

        private DateWizard InitializeForm(string code, FeeEarner fee)
        {
            if (code != "" && _file != null && (_currentWizard == null || _currentWizard.Code != code))
            {
                _currentWizard = DateWizard.GetDateWizard(code, _file, fee);
                if (_caption != null)
                {
                    _caption.Text = _currentWizard.Description;
                }
            }
            return _currentWizard;
        }

        private FeeEarner GetFeeEarner()
        {
            FeeEarner fee = Session.CurrentSession.CurrentFeeEarner;
            int feeid = fee.ID;
            if (_feeEarner != null)
            {
                if (_feeEarner.Value != DBNull.Value)
                {
                    feeid = Convert.ToInt32(_feeEarner.Value);
                }
            }
            if (feeid != fee.ID)
            {
                fee = FeeEarner.GetFeeEarner(feeid);
            }
            return fee;
        }

        protected override void ValidateFormCompleted(object sender, EventArgs e)
        {
            if (_dateWiz.EnquiryForm.RequiredFieldsComplete())
            {
                base.ValidateFormCompleted(sender, e);
            }
            else
            {
                DisableFinish();
            }
        }

        protected override void btnAdd_Click(object sender, EventArgs e)
        {
            base.btnAdd_Click(sender, e);
            try
            {
                _dateWiz.CreateWizardNotes(Convert.ToString(_notes?.Value));
                _dateWiz.UpdateItem();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }
    }
}
