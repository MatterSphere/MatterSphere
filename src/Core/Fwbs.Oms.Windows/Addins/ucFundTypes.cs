using System;
using System.ComponentModel;
using System.Windows.Forms;
using FWBS.Common.UI;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Summary description for ucFundTypes.
    /// </summary>
    public class ucFundTypes : ucBaseAddin
    {
        private OMSFile _omsfile = null;
        private IBasicEnquiryControl2 cboFileLACategory = null;
        private IBasicEnquiryControl2 curFileCreditLimit = null;
        private IBasicEnquiryControl2 curFileOriginalLimi = null;
        private IBasicEnquiryControl2 speFileWarningPerc = null;
        private IBasicEnquiryControl2 txtFileFundRef = null;
        private IBasicEnquiryControl2 dteFileAgreementDat = null;
        private IBasicEnquiryControl2 speFileBanding = null;
        private IBasicEnquiryControl2 curFileRatePerUnit = null;
        private IBasicEnquiryControl2 cboFileFranCode = null;
        private IBasicEnquiryControl2 curFileEstimate = null;
        private IBasicEnquiryControl2 curFileLastEstimate = null;
        private EnquiryForm _parent = null;

        private FWBS.OMS.UI.Windows.EnquiryForm enquiryForm1;
        private FWBS.Common.UI.Windows.eComboBox2 cmbFundTypes;

        public ucFundTypes()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                enquiryForm1.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code
        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.enquiryForm1 = new FWBS.OMS.UI.Windows.EnquiryForm();
            this.cmbFundTypes = new FWBS.Common.UI.Windows.eComboBox2();
            this.pnlDesign.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlDesign
            // 
            this.pnlDesign.Location = new System.Drawing.Point(8, 8);
            this.pnlDesign.Size = new System.Drawing.Size(168, 474);
            // 
            // pnlActions
            // 
            this.resourceLookup1.SetLookup(this.pnlActions, new FWBS.OMS.UI.Windows.ResourceLookupItem("Actions", "Actions", ""));
            this.pnlActions.Controls.SetChildIndex(this.navCommands, 0);
            // 
            // enquiryForm1
            // 
            this.enquiryForm1.AutoScroll = true;
            this.enquiryForm1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.enquiryForm1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.enquiryForm1.IsDirty = false;
            this.enquiryForm1.Location = new System.Drawing.Point(176, 34);
            this.enquiryForm1.Name = "enquiryForm1";
            this.enquiryForm1.Size = new System.Drawing.Size(656, 448);
            this.enquiryForm1.TabIndex = 10;
            this.enquiryForm1.ToBeRefreshed = false;
            this.enquiryForm1.Rendered += new System.EventHandler(this.enquiryForm1_Rendered);
            // 
            // cmbFundTypes
            // 
            this.cmbFundTypes.CaptionWidth = 280;
            this.cmbFundTypes.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbFundTypes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cmbFundTypes.IsDirty = false;
            this.cmbFundTypes.Location = new System.Drawing.Point(176, 8);
            this.resourceLookup1.SetLookup(this.cmbFundTypes, new FWBS.OMS.UI.Windows.ResourceLookupItem("SelectFundTypes", "Please select the Funding Information for this %FILE% : ", ""));
            this.cmbFundTypes.MaxLength = 0;
            this.cmbFundTypes.Name = "cmbFundTypes";
            this.cmbFundTypes.Padding = new System.Windows.Forms.Padding(50, 0, 0, 0);
            this.cmbFundTypes.Size = new System.Drawing.Size(656, 26);
            this.cmbFundTypes.TabIndex = 4;
            this.cmbFundTypes.TabStop = true;
            this.cmbFundTypes.Text = "Please select the Funding Information for this %FILE% : ";
            this.cmbFundTypes.ActiveChanged += new System.EventHandler(this.cmbFundTypes_Changed);
            // 
            // ucFundTypes
            // 
            this.Controls.Add(this.enquiryForm1);
            this.Controls.Add(this.cmbFundTypes);
            this.Name = "ucFundTypes";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Load += new System.EventHandler(this.ucFundTypes_Load);
            this.ParentChanged += new System.EventHandler(this.ucFundTypes_ParentChanged);
            this.Controls.SetChildIndex(this.pnlDesign, 0);
            this.Controls.SetChildIndex(this.cmbFundTypes, 0);
            this.Controls.SetChildIndex(this.enquiryForm1, 0);
            this.pnlDesign.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        #endregion

        public override void RefreshItem()
        {
            if (_omsfile != null)
            {
                try
                {
                    if (ToBeRefreshed)
                    {
                        if (_omsfile.FundingType != null && enquiryForm1.Enquiry == null)
                        {
                            string enquiryCode = Session.CurrentSession.DefaultSystemForm(SystemForms.FileFunding);
                            if (cmbFundTypes.CaptionTop) enquiryCode = "STP" + enquiryCode.Substring(3);
                            enquiryForm1.Dirty -= new EventHandler(OnDirty);
                            enquiryForm1.Enquiry = EnquiryEngine.Enquiry.GetEnquiry(enquiryCode, null, _omsfile, null);
                            enquiryForm1.Dirty += new EventHandler(OnDirty);
                            cmbFundTypes.Value = _omsfile.FundingType.Code;
                            cmbFundTypes_Changed(this, EventArgs.Empty);
                            enquiryForm1.IsDirty = false;
                        }
                        else
                        {
                            enquiryForm1.RefreshItem();
                        }
                    }
                    ToBeRefreshed = false;
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ParentForm, ex);
                }
            }
            base.RefreshItem();
        }

        public override bool Connect(FWBS.OMS.Interfaces.IOMSType obj)
        {
            enquiryForm1.NewOMSTypeWindow -= new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
            enquiryForm1.NewOMSTypeWindow += new NewOMSTypeWindowEventHandler(this.OnNewOMSTypeWindow);
            _omsfile = obj as OMSFile;
            if (obj == null)
                return false;
            else
            {
                ToBeRefreshed = true;
                return true;
            }
        }

        public override void UpdateItem()
        {
            if (Convert.ToString(cmbFundTypes.Value) == "LEGALAID" && Common.ConvertDef.ToInt16(cboFileLACategory.Value, 0) == 0)
                throw new OMSException2("REQLACAT", "The Legal Aid Category is Required", "", new Exception());
            else
                enquiryForm1.UpdateItem();
        }

        [Browsable(false)]
        public override bool IsDirty
        {
            get
            {
                return enquiryForm1.IsDirty;
            }
        }

        [Category("OMS Appearance")]
        [DefaultValue(false)]
        public virtual bool CaptionTop
        {
            get
            {
                return cmbFundTypes.CaptionTop;
            }
            set
            {
                if (cmbFundTypes.CaptionTop != value)
                {
                    int height = value ? 52 : 26;
                    cmbFundTypes.CaptionTop = value;
                    cmbFundTypes.Height = (Parent != null) ? LogicalToDeviceUnits(height) : height;
                    if (!value) cmbFundTypes.CaptionWidth = 280;
                }
            }
        }

        private void enquiryForm1_Rendered(object sender, System.EventArgs e)
        {
            FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "UI", Session.CurrentSession.CurrentUser.PreferedCulture }, { "FILEID", _omsfile.ID } };
            cmbFundTypes.AddItem(FWBS.OMS.UI.Windows.Services.RunDataList("DFUNDACT+FILE", kvc));
            
            cboFileLACategory = enquiryForm1.GetIBasicEnquiryControl2("cboFileLACategory", EnquiryControlMissing.Exception);
            curFileCreditLimit = enquiryForm1.GetIBasicEnquiryControl2("curFileCreditLimit", EnquiryControlMissing.Exception);
            curFileOriginalLimi = enquiryForm1.GetIBasicEnquiryControl2("curFileOriginalLimi", EnquiryControlMissing.Exception);
            speFileWarningPerc = enquiryForm1.GetIBasicEnquiryControl2("speFileWarningPerc", EnquiryControlMissing.Exception);
            txtFileFundRef = enquiryForm1.GetIBasicEnquiryControl2("txtFileFundRef", EnquiryControlMissing.Exception);
            dteFileAgreementDat = enquiryForm1.GetIBasicEnquiryControl2("dteFileAgreementDat", EnquiryControlMissing.Exception);
            speFileBanding = enquiryForm1.GetIBasicEnquiryControl2("speFileBanding", EnquiryControlMissing.Exception);
            curFileRatePerUnit = enquiryForm1.GetIBasicEnquiryControl2("curFileRatePerUnit", EnquiryControlMissing.Exception);
            cboFileFranCode = enquiryForm1.GetIBasicEnquiryControl2("cboFileFranCode", EnquiryControlMissing.Exception);
            curFileEstimate = enquiryForm1.GetIBasicEnquiryControl2("curFileEstimate", EnquiryControlMissing.Exception);
            curFileLastEstimate = enquiryForm1.GetIBasicEnquiryControl2("curFileLastEstimate", EnquiryControlMissing.Exception);

            if (_omsfile.FundingType != null)
            {
                dteFileAgreementDat.Text = FWBS.OMS.CodeLookup.GetLookup("FTAGREEMENT", _omsfile.FundingType.AgreementCode, "");
                txtFileFundRef.Text = Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("FTREFDESC", _omsfile.FundingType.RefCode, ""), true);
                curFileCreditLimit.Text = FWBS.OMS.CodeLookup.GetLookup("FTCLDESC", _omsfile.FundingType.CreditLimitCode, "");
            }

            if (FWBS.OMS.Session.CurrentSession.UseExternalBalances == true)
            {
                string caption = FWBS.OMS.Session.CurrentSession.Resources.GetResource("EXTBALINFO", "Some information on this screen has been gathered from an external accounts system and may be readonly.", "").Text;
                curFileCreditLimit.ReadOnly = true;
                curFileLastEstimate.ReadOnly = true;
                if (String.IsNullOrEmpty(caption) == false)
                {
                    if (enquiryForm1.Controls.ContainsKey("extbal") == false)
                    {
                        Label extbal = new Label();
                        extbal.Dock = DockStyle.Bottom;
                        extbal.AutoSize = true;
                        extbal.Name = "extbal";
                        extbal.Text = caption;
                        enquiryForm1.Controls.Add(extbal);
                    }
                }
            }
        }

        private void cmbFundTypes_Changed(object sender, System.EventArgs e)
        {
            try
            {
                bool refresh = false;
                if (cmbFundTypes.Value != DBNull.Value)
                {
                    if (_omsfile.FundingType == null) refresh = true;
                    _omsfile.FundingType = FWBS.OMS.FundType.GetFundType(Convert.ToString(cmbFundTypes.Value), _omsfile.ISOCode);
                    dteFileAgreementDat.Text = FWBS.OMS.CodeLookup.GetLookup("FTAGREEMENT", _omsfile.FundingType.AgreementCode, "Agreement Date :");
                    txtFileFundRef.Text = Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("FTREFDESC", _omsfile.FundingType.RefCode, "Reference :"), true);
                    curFileCreditLimit.Text = FWBS.OMS.CodeLookup.GetLookup("FTCLDESC", _omsfile.FundingType.CreditLimitCode, "Credit Limit :");

                    if (_omsfile.FundingType.LegalAidCharged)
                    //Show Legal Aid Items,and hide Private Specific items.
                    //May need to move controls around
                    {
                        //Legal Aid Files
                        ((Control)cboFileLACategory).Visible = true;
                        ((Control)cboFileFranCode).Visible = true;
                        ((Control)speFileBanding).Visible = false;
                        ((Control)curFileRatePerUnit).Visible = false;
                        enquiryForm1.GetControl("LegalAidLine", EnquiryControlMissing.Create).Visible = true;
                    }
                    else
                    {
                        //Non-Legal Aid Files
                        cboFileLACategory.Value = DBNull.Value;
                        ((Control)cboFileLACategory).Visible = false;
                        ((Control)cboFileFranCode).Visible = false;
                        ((Control)speFileBanding).Visible = true;
                        enquiryForm1.GetControl("LegalAidLine", EnquiryControlMissing.Create).Visible = false;
                        ((Control)curFileRatePerUnit).Visible = true;
                    }

                    if (refresh) RefreshItem();
                }
            }
            catch (Exception ex)
            {
                cmbFundTypes.Value = _omsfile.FundingType.Code;
                ErrorBox.Show(ParentForm, ex);
            }

        }

        private void ucFundTypes_ParentChanged(object sender, System.EventArgs e)
        {
            if (Parent is FWBS.OMS.UI.Windows.EnquiryForm)
            {
                _parent = Parent as EnquiryForm;
                if (_parent.Enquiry.Object is OMSFile)
                {
                    Connect((OMSFile)_parent.Enquiry.Object);
                    RefreshItem();
                }
                _parent.Updating += new CancelEventHandler(_parent_Updating);
            }
            else
                cmbFundTypes.Enabled = Session.CurrentSession.CurrentUser.IsInRoles("CHANGEFUNDING");

        }

        private void _parent_Updating(object sender, CancelEventArgs e)
        {
            if (Convert.ToString(cmbFundTypes.Value) == "LEGALAID" && Common.ConvertDef.ToInt16(cboFileLACategory.Value, 0) == 0)
            {
                ErrorBox.Show(ParentForm, new OMSException2("REQLACAT", "The Legal Aid Category is Required", this.Name, new Exception()));
                _parent.GotoControl(this.Name);
                e.Cancel = true;
            }
        }

        private void ucFundTypes_Load(object sender, System.EventArgs e)
        {
            cmbFundTypes.Text = Session.CurrentSession.Resources.GetResource("FUNDINGPICK", "Please select the Funding Information for this %FILE% : ", "", true).Text;
        }
    }
}
