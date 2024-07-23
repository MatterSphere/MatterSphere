namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    public interface IPageProvider
    {
        string[] Headers { get; }
        BaseContainerPage GetPage(string header);
        PageDetails GetDetails(string header);
    }
}
