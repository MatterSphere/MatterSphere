using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public partial class ucScreen : ucEditBase2
    {
        private FWBS.OMS.UI.Windows.EnquiryForm enquiryControl1;
        private FWBS.OMS.EnquiryEngine.EnquiryMode mode;
        private string code;

        public ucScreen()
        {
            InitializeComponent();
        }

        public ucScreen(IMainParent mainparent, Control editparent, FWBS.Common.KeyValueCollection Params)
            : base(mainparent, editparent, Params)
		{
            // This call is required by the Windows Form Designer.
			InitializeComponent();
            enquiryControl1 = new FWBS.OMS.UI.Windows.EnquiryForm();
            enquiryControl1.Dock = DockStyle.Fill;
            tpEdit.Controls.Add(enquiryControl1);
            enquiryControl1.BringToFront();
            if (Params["Mode"] == null)
                mode = FWBS.OMS.EnquiryEngine.EnquiryMode.Add;
            else if (Convert.ToString(Params["Mode"].Value) == "E")
                mode = FWBS.OMS.EnquiryEngine.EnquiryMode.Edit;
            else if (Convert.ToString(Params["Mode"].Value) == "S")
            {
                mode = FWBS.OMS.EnquiryEngine.EnquiryMode.Search;
                tbcEdit.Visible = false;
            }
            else
                mode = FWBS.OMS.EnquiryEngine.EnquiryMode.Add;
            code = Convert.ToString(Params["Code"].Value);
            this.ParentChanged += new EventHandler(ucScreen_ParentChanged);
        }

        private void ucScreen_ParentChanged(object sender, EventArgs e)
        {
            if (code != null && Parent != null)
            {
                LoadSingleItem(code);
            }
            ShowEditor();
        }

        protected override void LoadSingleItem(string Code)
        {
            EnquiryEngine.Enquiry enquiry = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiry(Code, null, mode, new FWBS.Common.KeyValueCollection());
            enquiryControl1.Enquiry = enquiry;
            enquiryControl1.Dirty += new EventHandler(enquiryControl1_Dirty);
            labSelectedObject.Text = enquiryControl1.Enquiry.EnquiryName;
        }

        private void enquiryControl1_Dirty(object sender, EventArgs e)
        {
            this.IsDirty = true;
        }

        protected override bool UpdateData()
        {
            try
            {
                enquiryControl1.UpdateItem();
                MessageBox.ShowInformation("DATASAVED", "Data Saved Successfully");
                return true;
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
                return false;
            }
        }

        public override bool IsDirty
        {
            get
            {
                return base.IsDirty && tbcEdit.Visible;
            }
            set
            {
                base.IsDirty = value;
            }
        }

        protected override void ShowList()
        {
            ParentForm.Close();
        }

        // ************************************************************************************************
        //
        // CLOSE
        //
        // ************************************************************************************************

        protected override void CloseAndReturnToList()
        {
            this.HostingTab.Dispose();
        }

        // ************************************************************************************************ 



    }
}
