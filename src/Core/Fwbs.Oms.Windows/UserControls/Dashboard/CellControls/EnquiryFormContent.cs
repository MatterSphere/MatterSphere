using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    public class EnquiryFormContent : IEditFormContent
    {
        private EnquiryForm _enquiryForm;

        public EnquiryFormContent(EnquiryForm enquiryForm)
        {
            _enquiryForm = enquiryForm;
        }
        
        #region IEditFormContent

        public bool AutoScroll
        {
            get { return _enquiryForm.AutoScroll; }
            set { _enquiryForm.AutoScroll = value; }
        }

        public Control Content
        {
            get { return _enquiryForm; }
        }

        public DockStyle Dock
        {
            get { return _enquiryForm.Dock; }
            set { _enquiryForm.Dock = value; }
        }

        public bool Enabled
        {
            get { return _enquiryForm.Enabled; }
            set { _enquiryForm.Enabled = value; }
        }


        public bool IsDirty
        {
            get
            {
                return _enquiryForm.IsDirty;
            }
        }

        public Point Location
        {
            get { return _enquiryForm.Location; }
            set { _enquiryForm.Location = value; }
        }

        public void UpdateContent()
        {
            _enquiryForm.UpdateItem();
        }
        #endregion
    }
}
