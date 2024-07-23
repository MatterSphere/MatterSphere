using FWBS.OMS.UI.UserControls.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.FileManagement.Addins.Dashboard
{
    public class TasksBuilder : ICellBuilder
    {
        public void Build(ucCellContainer container, DashboardCell cell)
        {
            container.HideFullScreenButton();
            var provider = new TasksPageProvider();
            container.InsertDefaultItem(provider);
        }

        public ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title)
        {
            var searchControl = new ucSearchControl();
            return new ContentContainer(omsObjectCode, searchControl)
            {
                Title = title
            };
        }
    }
}
