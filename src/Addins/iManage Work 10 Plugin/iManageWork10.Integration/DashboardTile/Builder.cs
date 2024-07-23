using FWBS.OMS.UI.UserControls.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders;

namespace iManageWork10.Integration.DashboardTile
{
    class Builder : ICellBuilder
    {
        public void Build(ucCellContainer container, DashboardCell cell)
        { 
            container.HideFullScreenButton();
            container.HideAddNewButton();
            container.HideDeleteButton();
            container.HideBottomPanel();
            container.HideFilterButton();
            container.InsertDefaultItem(new PageProvider());
            container.PopOutVisible = false;
        }

        public ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title)
        {
            return null;
        }
    }
}
