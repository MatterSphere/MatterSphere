using System.Windows.Forms;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    public partial class ucOmsItem : UserControl
    {
        public ucOmsItem()
        {
            InitializeComponent();
        }

        internal ucOmsItem(string code, object parent, EnquiryMode mode, bool offline, FWBS.Common.KeyValueCollection param) : this()
        {
            enquiryForm1.Enquiry = Enquiry.GetEnquiry(code, parent, mode, offline, param);
            Setup(code, mode, parent, param);
        }

        public string Description
        {
            get { return enquiryForm1.Description; }
        }

        public virtual void UpdateItem()
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                enquiryForm1.UpdateItem();
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        public bool IsDirty
        {
            get { return enquiryForm1.IsDirty; }
        }
        
        protected virtual void Setup(string enquiryCode, EnquiryMode mode, object parent, FWBS.Common.KeyValueCollection param)
        {
            enquiryForm1.SetCanvasSize();
        }
    }
}
