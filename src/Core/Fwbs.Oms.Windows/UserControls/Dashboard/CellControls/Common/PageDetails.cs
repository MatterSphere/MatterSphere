namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    public class PageDetails
    {
        public PageDetails(string code, string title)
        {
            OmsObjectCode = code;
            Title = title;
        }

        public string OmsObjectCode { get; set; }
        public string Title { get; set; }
    }
}
