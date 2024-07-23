using System;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders
{
    internal class CalendarBuilder : ICellBuilder
    {
        public void Build(ucCellContainer container, DashboardCell cell)
        {
            container.HideDeleteButton();
            container.HideFullScreenButton();
            var provider = new CalendarPageProvider();
            container.InsertDefaultItem(provider);
            container.PopOutVisible = false;
        }

        public ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title)
        {
            throw new NotImplementedException();
        }
    }
}
