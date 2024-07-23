using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders
{
    internal class SearchListBuilder : ICellBuilder
    {
        public void Build(ucCellContainer container, DashboardCell cell)
        {
            var searchListCell = cell as SearchListDashboardCell;
            var searchControl = new ucSearchControl();
            searchControl.NewOMSTypeWindow += container.OnNewOMSTypeWindow;
            container.QueryChanged += searchControl.OnQueryChanged;
            container.InsertSearchList(searchControl, searchListCell.SourceCode);
            container.InsertTitle(cell.Description);
            container.HideFilterButton();
            container.HideFullScreenButton();
        }

        public ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title)
        {
            var searchListCell = cell as SearchListDashboardCell;
            var searchControl = new ucSearchControl();
            return new ContentContainer(searchListCell.SourceCode, searchControl)
            {
                Title = cell.Description
            };
        }
    }
}
