using System.Windows.Forms;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.UI.Windows;
using FWBS.OMS.UI.Windows.Interfaces;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    public class ContentContainer
    {
        public ContentContainer(string code, ucSearchControl searchList)
        {
            Code = code;
            SearchList = searchList;
        }

        public ContentContainer(string code, EnquiryForm enquiryForm)
        {
            Code = code;
            EnquiryForm = enquiryForm;
        }

        public ContentContainer(string code, IOMSTypeAddin omsTypeAddin)
        {
            Code = code;
            OMSTypeAddin = omsTypeAddin;
        }

        public string Code { get; set; }
        public ucSearchControl SearchList { get; set; }
        public EnquiryForm EnquiryForm { get; set; }
        public IOMSTypeAddin OMSTypeAddin { get; set; }
        public string Title { get; set; }

        public void UpdateContent(Control parent)
        {
            if (SearchList != null)
            {
                SearchList.SetSearchList(Code, true, parent, new FWBS.Common.KeyValueCollection());
                return;
            }

            if (EnquiryForm != null)
            {
                EnquiryForm.Enquiry = Enquiry.GetEnquiry(Code, parent, EnquiryMode.None, true, null);
                return;
            }
        }

        public Control GetContent()
        {
            if (SearchList != null)
            {
                SearchList.Dock = DockStyle.Fill;
                return SearchList;
            }

            if (EnquiryForm != null)
            {
                EnquiryForm.Dock = DockStyle.Fill;
                return EnquiryForm;
            }

            if (OMSTypeAddin != null)
            {
                var addin = (Control) OMSTypeAddin;
                addin.Dock = DockStyle.Fill;
                return addin;
            }

            return null;
        }
    }
}
