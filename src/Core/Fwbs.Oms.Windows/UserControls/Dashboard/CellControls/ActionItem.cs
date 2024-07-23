namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    public class ActionItem
    {
        public ActionItem(string title, string code)
        {
            Title = title;
            Code = code;
        }

        public string Title { get; set; }
        public string Code { get; set; }
    }
}
