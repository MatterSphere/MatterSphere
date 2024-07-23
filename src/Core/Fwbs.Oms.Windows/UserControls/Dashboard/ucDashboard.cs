using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.MatterList;
using FWBS.OMS.UI.Windows;
using FWBS.OMS.UI.Windows.Interfaces;
using static FWBS.OMS.UI.UserControls.Dashboard.CellControls.ucCellContainer;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    public partial class ucDashboard : ucBaseAddin, ICellsContainer, IOMSTypeAddin2
    {
        protected DashboardConfigProvider _dashboardConfigProvider;
        private CellMenuBuilder _menuBuilder;
        private DashboardItemBuilder _itemBuilder;
        private EmptyCellProvider _emptyCellProvider;

        public ucDashboard()
        {
            InitializeComponent();
            FavoritesProvider = new FavoritesProvider();
        }

        public FavoritesProvider FavoritesProvider { get; private set; }
        public int RowsNumber
        {
            get
            {
                return tlpContainer.RowCount;
            }
        }

        public int ColumnsNumber
        {
            get
            {
                return tlpContainer.ColumnCount;
            }
        }

        public string Code { get; set; }

        public bool IsConfigurationMode { get; set; }

        protected virtual object ParentObject
        {
            get
            {
                return Session.CurrentSession.CurrentUser;
            }
        }

        protected virtual DashboardConfigProvider DashboardConfigProvider
        {
            get
            {
                if (_dashboardConfigProvider == null)
                {
                    _dashboardConfigProvider = new DashboardConfigProvider(Code, IsConfigurationMode);
                }

                return _dashboardConfigProvider;
            }
        }

        public override bool Connect(FWBS.OMS.Interfaces.IOMSType obj)
        {
            Build();
            return true;
        }

        public override void RefreshItem()
        {
            Reset();
            Build();
        }

        private void Build()
        {
            var dashboardItems = DashboardConfigProvider.LoadDashboardObjects();
            var userSettings = DashboardConfigProvider.GetUserTilesSettings();
            var userItems = userSettings.Select(item => !string.IsNullOrWhiteSpace(item.UserSettings.OmsObjectCode)
                        ? item.UserSettings.OmsObjectCode
                        : item.UserSettings.DashboardType).ToList();
            _menuBuilder = new CellMenuBuilder(dashboardItems, userItems);
            _itemBuilder = new DashboardItemBuilder();
            _emptyCellProvider = new EmptyCellProvider(_menuBuilder);

            _emptyCellProvider.CreateCells(this);

            int index = 0;
            while (index < userSettings.Count)
            {
                if (!Session.CurrentSession.CurrentUser.IsInRoles(userSettings[index].ItemInfo.UserRoles))
                {
                    DashboardConfigProvider.RemoveItem(userSettings[index].UserSettings.Id);
                    userSettings.Remove(userSettings[index]);
                }
                else
                {
                    index++;
                }
            }

            InitializeDashboardSettings();
            _itemBuilder.Build(userSettings, this, ParentObject);
        }

        public void Reset()
        {
            Windows.Global.RemoveAndDisposeControls(tlpContainer);
            _dashboardConfigProvider = null;
            _menuBuilder = null;
            _itemBuilder = null;
            _emptyCellProvider = null;
        }

        public void AttachInsertHandler(ucEmptyCell cell)
        {
            cell.ItemCreating += InsertItem;
        }

        public void AddEmptyCell(ucEmptyCell cell)
        {
            this.tlpContainer.Controls.Add(cell, cell.CellLocation.X, cell.CellLocation.Y, true);
        }

        public void ClearCellsForDashboardItem(ucCellContainer item)
        {
            for (int i = 0; i < item.CellSize.Width; i++)
            {
                for (int j = 0; j < item.CellSize.Height; j++)
                {
                    var control = tlpContainer.GetControlFromPosition(item.CellLocation.X + i, item.CellLocation.Y + j);

                    var cellContainer = control as ucCellContainer;
                    if (cellContainer != null)
                    {
                        cellContainer.CellSize = new Size(1, 1);
                        tlpContainer.SetColumnSpan(cellContainer, 1);
                    }
                    else
                    {
                        control.Dispose();
                    }
                }
            }
        }

        public void InsertDashboardItem(ucCellContainer item)
        {
            if (item.DashboardCell.Id == Guid.Empty)
            {
                var objCode = item.DashboardCell.Code;
                var cellType = DashboardCellConverter.GetDashboardCellCode(item.DashboardCell.GetCellType());
                var guid = Guid.NewGuid();
                DashboardConfigProvider.AddItem(guid, cellType, objCode, item.CellLocation, item.CellSize);
                item.SetDashboardInformation(guid, Code);
            }

            tlpContainer.Controls.Add(item, item.CellLocation.X, item.CellLocation.Y, true);
            tlpContainer.SetColumnSpan(item, item.CellSize.Width);
            tlpContainer.SetRowSpan(item, item.CellSize.Height);
        }

        public void CloseCell(object sender, EventArgs e)
        {
            var cell = sender as ucCellContainer;
            CloseCell(cell);
        }

        public void CloseCell(ucCellContainer cell)
        {
            if (cell != null)
            {
                DashboardConfigProvider.RemoveItem(cell.DashboardCell.Id);
                RemoveItem(cell);
                _emptyCellProvider.AddEmptyCells(cell.CellLocation, cell.CellSize, this);
            }
        }

        public void MaximizeCell(object sender, EventArgs e)
        {
            var cell = sender as ucCellContainer;
            if (cell != null)
            {
                for (int i = 0; i < tlpContainer.RowCount; i++)
                {
                    if (i != cell.CellLocation.Y)
                    {
                        tlpContainer.RowStyles[i].SizeType = SizeType.Absolute;
                        tlpContainer.RowStyles[i].Height = 0;
                    }
                    else
                    {
                        tlpContainer.RowStyles[i].SizeType = SizeType.Percent;
                        tlpContainer.RowStyles[i].Height = 100;
                    }
                }

                for (int i = 0; i < tlpContainer.ColumnCount; i++)
                {
                    if (i != cell.CellLocation.X)
                    {
                        tlpContainer.ColumnStyles[i].SizeType = SizeType.Absolute;
                        tlpContainer.ColumnStyles[i].Width = 0;
                    }
                    else
                    {
                        tlpContainer.ColumnStyles[i].SizeType = SizeType.Percent;
                        tlpContainer.ColumnStyles[i].Width = 100;
                    }
                }
            }
        }

        public void MinimizeCell(object sender, EventArgs e)
        {
            MinimizeContainerItems();
        }

        public void GetResizeOptions(object sender, ResizeOptionsEventArgs e)
        {
            var cellContainer = sender as ucCellContainer;
            if (cellContainer != null)
            {
                var sizes = GetAvailableSizes(cellContainer.CellLocation, cellContainer);
                e.ResizeOptions = sizes
                    .Where(s => s.Width >= cellContainer.DashboardCell.MinimalSize.Width
                        && s.Height >= cellContainer.DashboardCell.MinimalSize.Height
                        && !(s.Width == cellContainer.DashboardCell.Size.Width && s.Height == cellContainer.DashboardCell.Size.Height))
                    .OrderBy(s => s.Width)
                    .ThenBy(s => s.Height)
                    .ToList();
                e.Handled = true;
            }
        }

        public void Resizing(object sender, ResizingEventArgs e)
        {
            var cellContainer = sender as ucCellContainer;
            if (cellContainer != null)
            {
                tlpContainer.Controls.Remove(cellContainer);
                _emptyCellProvider.AddEmptyCells(cellContainer.CellLocation, cellContainer.CellSize, this);

                cellContainer.CellSize = e.NewSize;
                cellContainer.DashboardCell.Size = e.NewSize;
                cellContainer.CellLocation = GetValidTileLocation(cellContainer.DashboardCell, cellContainer.CellLocation);
                tlpContainer.SetColumnSpan(cellContainer, 1);
                tlpContainer.SetRowSpan(cellContainer, 1);
                ClearCellsForDashboardItem(cellContainer);
                InsertResizedDashboardItem(cellContainer);
            }
        }

        public List<Size> GetAvailableSizes(Point location, Control self = null)
        {
            var result = new List<Size>();
            var mainRowWidth = GetAvailableCellsRightDirection(location, self) + GetAvailableCellsLeftDirection(location, self) - 1;
            for (int i = 1; i <= mainRowWidth; i++)
            {
                result.Add(new Size(i, 1));
            }
            var secondRowLocation = new Point(location.X, 1 - location.Y);
            var secondRowWidth = GetAvailableCellsRightDirection(secondRowLocation, self) + GetAvailableCellsLeftDirection(secondRowLocation, self) - 1;

            for (int i = 1; i <= Math.Min(mainRowWidth, secondRowWidth); i++)
            {
                result.Add(new Size(i, 2));
            }
            return result;
        }

        protected virtual void InitializeDashboardSettings()
        { }

        protected virtual void SetContent()
        {
            for (int i = 0; i < tlpContainer.Controls.Count; i++)
            {
                var container = tlpContainer.Controls[i] as ucCellContainer;

                try
                {
                    container?.SetContent(ParentObject);
                }
                catch
                {
                    FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ITEMNOTADDED", "Item '%1%' could not be added. Please contact your System Administrator.", "", container.DashboardCell.Code), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    CloseCell(container);
                }
            }
        }

        private void MinimizeContainerItems()
        {
            for (int i = 0; i < tlpContainer.RowCount; i++)
            {
                tlpContainer.RowStyles[i].SizeType = SizeType.Percent;
                tlpContainer.RowStyles[i].Height = 100F / tlpContainer.RowCount;
            }

            for (int i = 0; i < tlpContainer.ColumnCount; i++)
            {
                tlpContainer.ColumnStyles[i].SizeType = SizeType.Percent;
                tlpContainer.ColumnStyles[i].Width = 100F / tlpContainer.ColumnCount;
            }
        }

        private void InsertItem(object sender, DashboardCell e)
        {
            var emptyCell = sender as ucEmptyCell;
            if (emptyCell == null)
            {
                throw new ArgumentException("The sender is not an ucEmptyCell");
            }

            if (e is SearchListDashboardCell || e is EnquiryFormDashboardCell)
            {
                var cell = (OmsObjectDashboardCell) e;
                if (string.IsNullOrEmpty(cell.SourceCode))
                {
                    FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetMessage("ITEMNOTADDED", "Item '%1%' could not be added. Please contact your System Administrator.", "", cell.Code), "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            var location = GetValidTileLocation(e, emptyCell.CellLocation);
            _itemBuilder.Build(e, location, e.MinimalSize, this, true, ParentObject);
            _menuBuilder.RemoveItem(e);
        }

        private void InsertResizedDashboardItem(ucCellContainer item)
        {
            DashboardConfigProvider.ResizeItem(item.DashboardCell.Id, item.CellLocation, item.CellSize);
            tlpContainer.Controls.Add(item, item.CellLocation.X, item.CellLocation.Y);
            tlpContainer.SetColumnSpan(item, item.CellSize.Width);
            tlpContainer.SetRowSpan(item, item.CellSize.Height);
        }

        private Point GetValidTileLocation(DashboardCell tileCell, Point location)
        {
            var size = tileCell.Size;
            if (size.Height == 2)
            {
                location.Y = 0;
            }
            if (IsValidLocation(location, size))
            {
                return location;
            }
            location.X--;
            return GetValidTileLocation(tileCell, location);
        }

        private bool IsValidLocation(Point location, Size size)
        {
            if (size.Height == 1)
            {
                var rightDirCells = GetAvailableCellsRightDirection(location);
                return rightDirCells >= size.Width;
            }
            else
            {
                var rightDirCells = GetAvailableCellsRightDirection(location);
                var rightDirCellsSecondRow = GetAvailableCellsRightDirection(new Point(location.X, 1 - location.Y));
                return Math.Min(rightDirCells, rightDirCellsSecondRow) >= size.Width;
            }
        }

        private void RemoveItem(ucCellContainer item)
        {
            this.tlpContainer.Controls.Remove(item);
            item.DashboardCell.Id = Guid.Empty;
            item.DashboardCell.Size = item.DashboardCell.MinimalSize;
            MinimizeContainerItems();
            _menuBuilder.AddItem(item.DashboardCell);
        }


        private int GetAvailableCellsRightDirection(Point location, Control container = null)
        {
            int count = 0;
            for (int i = location.X; i < tlpContainer.ColumnCount; i++)
            {
                if (IsEmptyCell(new Point(i, location.Y)) || IsSelfContainerOccupiedCell(new Point(i, location.Y), container))
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        private int GetAvailableCellsLeftDirection(Point location, Control container = null)
        {
            int count = 0;
            for (int i = location.X; i >= 0; i--)
            {
                if (IsEmptyCell(new Point(i, location.Y)) || IsSelfContainerOccupiedCell(new Point(i, location.Y), container))
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        private bool IsSelfContainerOccupiedCell(Point location, Control container)
        {
            return tlpContainer.GetControlFromPosition(location.X, location.Y) == container;
        }

        private bool IsEmptyCell(Point location)
        {
            return tlpContainer.GetControlFromPosition(location.X, location.Y) is ucEmptyCell;
        }

    }
}
