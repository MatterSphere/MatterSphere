using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    public class EditFormParameters
    {
        public EditFormParameters(string code, EnquiryMode mode, FWBS.Common.KeyValueCollection param)
        {
            Code = code;
            EnquiryMode = mode;
            Parameters = param;
        }

        public string Code { get; set; }
        public EnquiryMode EnquiryMode { get; set; }
        public FWBS.Common.KeyValueCollection Parameters { get; set; }
    }
}
