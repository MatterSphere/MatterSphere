using FWBS.OMS.UI.UserControls.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders;

namespace FWBS.OMS.FinancialTile
{
    internal class Builder : ICellBuilder
    {
        public void Build(ucCellContainer container, DashboardCell cell)
        {
            container.HideFullScreenButton();
            container.HideAddNewButton();
            container.HideDeleteButton();
            var provider = new PageProvider();
            container.InsertDefaultItem(provider);
            container.PopOutVisible = false;
        }

        public ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title)
        {
            return null;
        }
    }
}
