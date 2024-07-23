using System;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Favorites;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders
{
    internal class FavoritesBuilder : ICellBuilder
    {
        public void Build(ucCellContainer container, DashboardCell cell)
        {
            container.HideDeleteButton();
            container.HideAddNewButton();
            container.HideFullScreenButton();
            var provider = new FavoritesPageProvider();
            container.InsertDefaultItem(provider);
            container.PopOutVisible = false;
        }

        public ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title)
        {
            throw new NotImplementedException();
        }
    }
}
