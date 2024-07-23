using System;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    internal class KeyDatesDashboardCell : DashboardCell
    {
        public KeyDatesDashboardCell(Guid id, DashboardConfigProvider.DashboardItemInfo itemInfo) : base(id, itemInfo)
        {
            _defaultDescription = "Key Dates / Calendar";
            Code = DashboardCellConverter.GetDashboardCellCode(DashboardCellEnum.KeyDatesAndCalendar);

            _builder = new CalendarBuilder();
        }

        public override string Description
        {
            get
            {
                return CanShowKeyDatesTile
                    ? CodeLookup.GetLookup("DASHBOARD",
                        DashboardCellConverter.GetDashboardCellCode(DashboardCellEnum.KeyDatesAndCalendar),
                        _defaultDescription)
                    : CodeLookup.GetLookup("DASHBOARD",
                        DashboardCellConverter.GetDashboardCellCode(DashboardCellEnum.Calendar), "Calendar");
            }
        }

        public override DashboardCellEnum GetCellType()
        {
            return DashboardCellEnum.KeyDatesAndCalendar;
        }

        public static bool CanShowKeyDatesTile
        {
            get
            {
                return Session.CurrentSession.IsPackageInstalled("KEYDATES");
            }
        }
    }
}
