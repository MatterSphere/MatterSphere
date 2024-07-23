namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders
{
    public interface ICellBuilder
    {
        void Build(ucCellContainer container, DashboardCell cell);
        ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title);
    }
}
