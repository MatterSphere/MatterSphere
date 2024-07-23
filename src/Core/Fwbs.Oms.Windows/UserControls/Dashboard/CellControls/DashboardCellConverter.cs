using System;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    internal class DashboardCellConverter
    {
        public static DashboardCellEnum GetDashboardCellType(string dashboardCellType)
        {
            switch (dashboardCellType)
            {
                case TileCodes.OMS_OBJECT:
                    return DashboardCellEnum.OMSObject;
                case TileCodes.FINANCIALS:
                    return DashboardCellEnum.Financials;
                case TileCodes.KEY_DATES_AND_CALENDAR:
                    return KeyDatesDashboardCell.CanShowKeyDatesTile
                        ? DashboardCellEnum.KeyDatesAndCalendar
                        : DashboardCellEnum.Calendar;
                case TileCodes.MATTER_LIST:
                    return DashboardCellEnum.Matters;
                case TileCodes.RECENTS_AND_FAVORITES:
                    return DashboardCellEnum.RecentsAndFavorites;
                default:
                    throw new ArgumentException("The unknown dashboard cell type");
            }
        }

        public static string GetDashboardCellCode(DashboardCellEnum dashboardCellType)
        {
            switch (dashboardCellType)
            {
                case DashboardCellEnum.OMSObject:
                    return TileCodes.OMS_OBJECT;
                case DashboardCellEnum.Financials:
                    return TileCodes.FINANCIALS;
                case DashboardCellEnum.KeyDatesAndCalendar:
                    return TileCodes.KEY_DATES_AND_CALENDAR;
                case DashboardCellEnum.Calendar:
                    return TileCodes.CALENDAR;
                case DashboardCellEnum.Matters:
                    return TileCodes.MATTER_LIST;
                case DashboardCellEnum.RecentsAndFavorites:
                    return TileCodes.RECENTS_AND_FAVORITES;
                default:
                    return null;
            }
        }
    }
}
