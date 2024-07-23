using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public partial class ucMileStoneConfig : ucEditBase2
    {
        private FWBS.OMS.MilestoneConfig _currentobj;
        private string _code;
        
        public ucMileStoneConfig()
        {
            InitializeComponent();
        }

        public ucMileStoneConfig(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params)
            : base(mainparent, editparent, Params)
		{
			// This call is required by the Windows Form Designer.
            InitializeComponent();
			tpEdit.Text = FWBS.OMS.Session.CurrentSession.Resources.GetResource("MILESTONECNG","Milestone Config","").Text;
			tpList.Text = tpEdit.Text;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null)
                Load();

            base.OnParentChanged(e);
        }

        #region Overrides
        protected override string SearchListName
        {
            get
            {
                return "ADMSYSMSTCNFG";
            }
        }
        
        protected override void LoadSingleItem(string Code)
        {
            _code = Code;
            try
            {
                _currentobj = new FWBS.OMS.MilestoneConfig(Code);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, new Exception(Session.CurrentSession.Resources.GetMessage("LOADMILESTCONF", "Error loading Milestone Config click Advanced ...", "").Text, ex));
                return;
            }
            labSelectedObject.Text = _currentobj.Code + " - " + FWBS.OMS.Session.CurrentSession.Resources.GetResource("MILESTONECNG","Milestone Config","").Text;
            enquiryForm1.Enquiry = EnquiryEngine.Enquiry.GetEnquiry("SCRSYSMSCONFIG2", null, _currentobj, null);
            ShowEditor(false);
            this.IsDirty = false;
        }


        protected override bool UpdateData()
        {
            try
            {
                enquiryForm1.UpdateItem();
                return true;
            }
            
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
                return false;
            }
        }

        protected override void NewData()
        {
            _code = InputBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("PLZECODE2", "Please enter a Code for this Package (15 Characters Max)", "").Text);
            if (_code == InputBox.CancelText) return;
            _currentobj = new FWBS.OMS.MilestoneConfig();
            if (_code.Length > 15)
                _currentobj.Code = _code.Substring(0, 15);
            else
                _currentobj.Code = _code;
            labSelectedObject.Text = string.Format("{0} - {1}", _currentobj.Code, ResourceLookup.GetLookupText("MILESTONECNG", "Milestone Config", ""));
            enquiryForm1.Enquiry = EnquiryEngine.Enquiry.GetEnquiry("SCRSYSMSCONFIG2", null, _currentobj, null);
            ShowEditor(true);
            this.IsDirty = false;
        }

        protected override void Clone(string Code)
        {
            _code = InputBox.Show(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("PLZECODE", "Please enter a Code for this Package", "").Text);
            if (_code == InputBox.CancelText) return;
            _currentobj = FWBS.OMS.MilestoneConfig.Clone(Code,_code);
            labSelectedObject.Text = string.Format("{0} - {1}", _currentobj.Code, ResourceLookup.GetLookupText("MILESTONECNG", "Milestone Config", ""));
            enquiryForm1.Enquiry = EnquiryEngine.Enquiry.GetEnquiry("SCRSYSMSCONFIG2", null, _currentobj, null);
            ShowEditor(true);
            this.IsDirty = true;
        }

        protected override void DeleteData(string Code)
        {
            try
            {
                _currentobj = new FWBS.OMS.MilestoneConfig(Code);
                _currentobj.Delete();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, new Exception(Session.CurrentSession.Resources.GetMessage("DELMILESTCONF", "Error Delete Milestone Config ...", "").Text, ex));
                return;
            }
        }

        protected override void CloseAndReturnToList()
        {
            if (base.IsDirty)
            {
                DialogResult? dr = base.IsObjectDirtyDialogResult();
                if (dr != DialogResult.Cancel)
                {
                    base.ShowList();
                }
            }
            else
            {
                base.ShowList();
            }
        }

        #endregion

        private void enquiryForm1_Dirty(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }


    }
}
