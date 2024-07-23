using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    internal class CellMenuBuilder
    {
        private List<DashboardCell> _cellItems;

        private readonly Dictionary<OMSObjectTypes, List<DashboardCell>> _omsObjectTypeDashboardCellDictionary;

        private readonly string _searchListRootMenuText = "Search Lists";

        internal CellMenuBuilder(List<DashboardConfigProvider.DashboardItemInfo> dashboardItems, List<string> excludedItems)
        {
            _cellItems = new List<DashboardCell>();
            _omsObjectTypeDashboardCellDictionary = new Dictionary<OMSObjectTypes, List<DashboardCell>>
            {
                {OMSObjectTypes.List, new List<DashboardCell>()}
            };

            var dashboardOmsObjectItems = dashboardItems
                .Where(cell => excludedItems.All(ei => ei != cell.Code)).ToList();

            if (dashboardOmsObjectItems.Any())
            {
                foreach (var item in dashboardOmsObjectItems)
                {
                    if (!Session.CurrentSession.CurrentUser.IsInRoles(item.UserRoles))
                    {
                        continue;
                    }

                    OmsObjectDashboardCell subItem = null;
                    switch (item.ObjectType)
                    {
                        case OMSObjectTypes.List:
                            subItem = new SearchListDashboardCell(Guid.Empty, item);
                            break;
                        case OMSObjectTypes.Enquiry:
                            _cellItems.Add(new EnquiryFormDashboardCell(Guid.Empty, item));
                            break;
                        case OMSObjectTypes.Addin:
                            switch (item.Code)
                            {
                                case TileCodes.KEY_DATES_AND_CALENDAR:
                                    _cellItems.Add(new KeyDatesDashboardCell(Guid.Empty, item));
                                    break;
                                case TileCodes.MATTER_LIST:
                                    _cellItems.Add(new MatterListDashboardCell(Guid.Empty, item));
                                    break;
                                case TileCodes.RECENTS_AND_FAVORITES:
                                    _cellItems.Add(new FavoritesDashboardCell(Guid.Empty, item));
                                    break;
                                default:
                                    _cellItems.Add(new AddinDashboardCell(Guid.Empty, item));
                                    break;
                            }
                            break;
                    }

                    if (subItem != null)
                    {
                        _omsObjectTypeDashboardCellDictionary[subItem.OmsObjectType].Add(subItem);
                    }
                }
            }

            _cellItems = _cellItems.Where(cell => excludedItems.All(ei => ei != cell.Code)).OrderBy(cell => cell.Priority).ThenBy(cell => cell.Description).ToList();
            _omsObjectTypeDashboardCellDictionary[OMSObjectTypes.List] = _omsObjectTypeDashboardCellDictionary[OMSObjectTypes.List].OrderBy(cell => cell.Priority).ThenBy(cell => cell.Description).ToList();

            _searchListRootMenuText = CodeLookup.GetLookup("DASHBOARD", "SEARCHLISTS", "Search Lists");
        }

        #region Methods

        internal void Build(ContextMenuStrip menu, ucEmptyCell cell)
        {
            menu.Items.Clear();

            foreach (var cellItem in _cellItems)
            {
                var menuItem = new DashboardToolStripMenuItem(cell, cellItem);
                menu.Items.Add(menuItem);
            }

            BuildSubMenus(menu, cell);
        }

        internal void AddItem(DashboardCell cell)
        {
            SearchListDashboardCell searchListDashboardCell = cell as SearchListDashboardCell;
            if (searchListDashboardCell != null)
            {
                _omsObjectTypeDashboardCellDictionary[searchListDashboardCell.OmsObjectType].Add(searchListDashboardCell);
                _omsObjectTypeDashboardCellDictionary[searchListDashboardCell.OmsObjectType] = _omsObjectTypeDashboardCellDictionary[searchListDashboardCell.OmsObjectType].OrderBy(c => c.Priority)
                    .ThenBy(c => c.Description).ToList();
                return;
            }

            _cellItems.Add(cell);
            _cellItems = _cellItems.OrderBy(c => c.Priority).ToList();
        }

        internal void RemoveItem(DashboardCell cell)
        {
            SearchListDashboardCell searchListDashboardCell = cell as SearchListDashboardCell;
            if (searchListDashboardCell != null)
            {
                _omsObjectTypeDashboardCellDictionary[searchListDashboardCell.OmsObjectType].Remove(searchListDashboardCell);
                return;
            }

            _cellItems.Remove(cell);
        }

        private void BuildSubMenus(ContextMenuStrip menu, ucEmptyCell cell)
        {
            if (_omsObjectTypeDashboardCellDictionary[OMSObjectTypes.List].Any())
            {
                var searchListRootMenuItem = new DashboardToolStripMenuItem(cell, _searchListRootMenuText);
                BuildSubMenu(searchListRootMenuItem, _omsObjectTypeDashboardCellDictionary[OMSObjectTypes.List], cell);
                menu.Items.Add(searchListRootMenuItem);
            }
        }

        private void BuildSubMenu(DashboardToolStripMenuItem root, List<DashboardCell> dashboardCells, ucEmptyCell ucEmptyCell)
        {
            foreach (var cellItem in dashboardCells)
            {
                var menuItem = new DashboardToolStripMenuItem(ucEmptyCell, cellItem);
                root.DropDownItems.Add(menuItem);
            }
        }
        
        #endregion
    }
}
