using System;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Favorites
{
    internal class FavoritesPageProvider : IPageProvider
    {
        private const string RECENTS_TITLE_CODE = "RCNTS";
        private const string FAVORITES_TITLE_CODE = "FVRTS";

        public FavoritesPageProvider()
        {
            Headers = new[]
            {
                FavoritesDashboardItem.FavoritesPage.Favorites.ToString(),
                FavoritesDashboardItem.FavoritesPage.Recents.ToString()
            };
        }

        public string[] Headers { get; }

        public BaseContainerPage GetPage(string header)
        {
            if (header == FavoritesDashboardItem.FavoritesPage.Recents.ToString())
            {
                return CreateRecentsPage();
            }

            if (header == FavoritesDashboardItem.FavoritesPage.Favorites.ToString())
            {
                return CreateFavoritesPage();
            }

            throw new ArgumentOutOfRangeException();
        }

        public PageDetails GetDetails(string header)
        {
            if (header == FavoritesDashboardItem.FavoritesPage.Recents.ToString())
            {
                return new PageDetails(null, CodeLookup.GetLookup("DASHBOARD", RECENTS_TITLE_CODE, "Recents"));
            }

            if (header == FavoritesDashboardItem.FavoritesPage.Favorites.ToString())
            {
                return new PageDetails(null, CodeLookup.GetLookup("DASHBOARD", FAVORITES_TITLE_CODE, "Favorites"));
            }

            throw new ArgumentOutOfRangeException();
        }

        private BaseContainerPage CreateRecentsPage()
        {
            return new FavoritesDashboardItem(FavoritesDashboardItem.FavoritesPage.Recents)
            {
                Code = FavoritesDashboardItem.FavoritesPage.Recents.ToString(),
                Title = CodeLookup.GetLookup("DASHBOARD", RECENTS_TITLE_CODE, "Recents"),
                Dock = DockStyle.Fill
            };
        }

        private BaseContainerPage CreateFavoritesPage()
        {
            return new FavoritesDashboardItem(FavoritesDashboardItem.FavoritesPage.Favorites)
            {
                Code = FavoritesDashboardItem.FavoritesPage.Favorites.ToString(),
                Title = CodeLookup.GetLookup("DASHBOARD", FAVORITES_TITLE_CODE, "Favorites"),
                Dock = DockStyle.Fill
            };
        }
    }
}
