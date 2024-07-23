using System;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    internal class FavoritesDashboardCell : DashboardCell
    {
        public FavoritesDashboardCell(Guid id, DashboardConfigProvider.DashboardItemInfo itemInfo) : base(id, itemInfo)
        {
            _defaultDescription = "Recents / Favorites";
            Code = DashboardCellConverter.GetDashboardCellCode(DashboardCellEnum.RecentsAndFavorites);

            _builder = new FavoritesBuilder();
        }

        public override DashboardCellEnum GetCellType()
        {
            return DashboardCellEnum.RecentsAndFavorites;
        }
    }
}
