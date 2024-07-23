using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    public class DashboardItemBuilder
    {
        public void Build(DashboardCell cell, Point location, Size size, ucDashboard dashboard, bool setContent, object parent = null)
        {
            var container = CreateContainer(cell, location, size, dashboard.IsConfigurationMode);
            container.Visible = false;
            container.FavoritesProvider = dashboard.FavoritesProvider;
            cell.DashboardCode = dashboard.Code;
            cell.Size = size;
            cell.Build(container, cell);
            dashboard.ClearCellsForDashboardItem(container);
            if (!dashboard.IsConfigurationMode)
            {
                container.NewOMSTypeWindow += dashboard.OnNewOMSTypeWindow;
            }
            container.Closing += dashboard.CloseCell;
            container.Maximizing += dashboard.MaximizeCell;
            container.Minimizing += dashboard.MinimizeCell;
            container.ResizeClicked += dashboard.GetResizeOptions;
            container.Resizing += dashboard.Resizing;
            dashboard.InsertDashboardItem(container);

            if (setContent)
            {
                try
                {
                    container.SetContent(parent);
                }
                catch
                {
                    FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ITEMNOTADDED","Item '%1%' could not be added. Please contact your System Administrator.", "", cell.Code), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    dashboard.CloseCell(container);
                }
            }

            container.Visible = true;
        }

        public ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title)
        {
            return cell.CreateWindowContent(cell, omsObjectCode, title);
        }

        public void Build(IEnumerable<DashboardConfigProvider.BuilderItem> items, ucDashboard dashboard, object parent = null)
        {
            foreach (var item in items)
            {
                if (item.ItemInfo != null && string.IsNullOrEmpty(item.ItemInfo.SourceCode) && item.ItemInfo.ObjectType != OMSObjectTypes.Addin)
                {
                    FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ITEMNOTADDED", "Item '%1%' could not be added. Please contact your System Administrator.", "", item.ItemInfo.Code), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                var cell = CreateDashboardCell(item.UserSettings.DashboardType, item.UserSettings, item.ItemInfo);
                Build(cell, item.UserSettings.Location, item.UserSettings.Size, dashboard, true, parent);
            }
        }

        #region Private methods

        private DashboardCell CreateDashboardCell(string dashboardType, DashboardConfigProvider.UserSettings userSettings, DashboardConfigProvider.DashboardItemInfo itemInfo)
        {
            var dashboardCellType = DashboardCellConverter.GetDashboardCellType(dashboardType);
            switch (dashboardCellType)
            {
                case DashboardCellEnum.OMSObject:
                    switch (itemInfo.ObjectType)
                    {
                        case OMSObjectTypes.List:
                            return new SearchListDashboardCell(userSettings.Id, itemInfo);
                        case OMSObjectTypes.Enquiry:
                            return new EnquiryFormDashboardCell(userSettings.Id, itemInfo);
                        case OMSObjectTypes.Addin:
                            return new AddinDashboardCell(userSettings.Id, itemInfo);
                        default:
                            throw new ArgumentException($"The ObjectType is {itemInfo.ObjectType}, but it should be List or Enquiry");
                    }
                case DashboardCellEnum.KeyDatesAndCalendar:
                case DashboardCellEnum.Calendar:
                    return new KeyDatesDashboardCell(userSettings.Id, itemInfo);
                case DashboardCellEnum.Matters:
                    return new MatterListDashboardCell(userSettings.Id, itemInfo);
                case DashboardCellEnum.RecentsAndFavorites:
                    return new FavoritesDashboardCell(userSettings.Id, itemInfo);
                default:
                    throw new NotImplementedException();
            }
        }

        private ucCellContainer CreateContainer(DashboardCell cell, Point location, Size size, bool isConfigurationMode)
        {
            return new ucCellContainer(isConfigurationMode)
            {
                Dock = DockStyle.Fill,
                DashboardCell = cell,
                CellLocation = location,
                CellSize = size.IsEmpty ? cell.MinimalSize : size
            };
        }

        #endregion
    }
}
