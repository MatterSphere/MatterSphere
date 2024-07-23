using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Favorites
{
    public partial class FavoritesDashboardItem : BaseContainerPage
    {
        private const string RECENTS_COLUMN = "clmRecents";
        private const string FAVORITES_COLUMN = "clmFavorites";
        private const string ID_COLUMN = "clmId";
        private const string CLIENT_ID_COLUMN = "clmClientId";
        private const string FILE_ID_COLUMN = "clmFileId";
        private const string ICON_COLUMN = "clmIcon";
        private const string CLIENT_NO_COLUMN = "clmClientNo";
        private const string FILE_NO_COLUMN = "clmFileNo";
        private const string PRECEDENT_ID_COLUMN = "clmPrecedentId";
        private const string DESCRIPTION_COLUMN = "clmDescription";
        private const string ITEM_TYPE_COLUMN = "clmItemType";
        private const string MODIFIED_COLUMN = "clmModified";
        private const string EXTENSION_COLUMN = "clmExtension";
        private string _query;
        private int _currentPage = 1;
        private string _orderBy;
        private string _sortColumn;
        private SortOrder _sortOrder = SortOrder.Ascending;
        private List<FavoriteRow> _dataGridItems;
        private ImageListProvider _imageListProvider;

        private FavoritesDashboardItem()
        {
            InitializeComponent();

            _imageListProvider = new ImageListProvider();

            ucDataView.CellClicked += CellClickHandler;
            ucDataView.CellDoubleClicked += CellDoubleClickHandler;
            ucDataView.SortChanged += OnSortChanged;
        }

        public FavoritesDashboardItem(FavoritesPage page) : this()
        {
            TilePage = page;

            if (page == FavoritesPage.Recents)
            {
                _orderBy = "OrderForRecent";
                _sortColumn = RECENTS_COLUMN;
            }
            else if (page == FavoritesPage.Favorites)
            {
                _orderBy = "combo";
                _sortColumn = MODIFIED_COLUMN;
            }

            SetColumns();
        }

        public FavoritesPage TilePage { get; private set; }

        private string Filter
        {
            get
            {
                switch (TilePage)
                {
                    case FavoritesPage.Favorites:
                        return "FA";
                    case FavoritesPage.Recents:
                        return "RE";
                    default:
                        return "Unknown";
                }
            }
        }

        #region UI events

        private void CellClickHandler(object sender, DataGridViewCellEventArgs e)
        {
            if (TilePage == FavoritesPage.Recents && e.ColumnIndex == 0 && e.RowIndex < 0)
            {
                if (_sortColumn != RECENTS_COLUMN)
                {
                    DataGridViewEx.SortDataEventArgs sa = new DataGridViewEx.SortDataEventArgs(RECENTS_COLUMN, SortOrder.Ascending);
                    ucDataView.dataGridView.ShowSorting(sa.Column, sa.Order);
                    OnSortChanged(sender, sa);
                }
            }
            else
            {
                RowClick(sender, e.ColumnIndex, e.RowIndex);
            }
        }

        private void CellDoubleClickHandler(object sender, DataGridViewCellEventArgs e)
        {
            RowClick(sender, e.ColumnIndex, e.RowIndex, true);
        }

        private void SetSortOrder(string column, SortOrder order)
        {
            switch (column)
            {
                case RECENTS_COLUMN:
                    _orderBy = "OrderForRecent";
                    order = SortOrder.Ascending;
                    break;
                case DESCRIPTION_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? "combo DESC"
                        : "combo";
                    break;
                case MODIFIED_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? "Updated DESC"
                        : "Updated";
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "Unexpected sort column: " + column);
                    return;
            }
            _sortColumn = column;
            _sortOrder = order;
        }

        private void OnSortChanged(object sender, DataGridViewEx.SortDataEventArgs e)
        {
            _currentPage = 1;
            SetSortOrder(e.Column, e.Order);
            ResetPages();
            UpdateData();
            SaveColumnSettings();
        }

        #endregion

        #region Private methods
        
        private void SetColumns()
        {
            var columns = new List<ColumnData>();

            if (TilePage == FavoritesPage.Favorites)
            {
                columns.Add(new ColumnData(FAVORITES_COLUMN, ColumnTypeEnum.DataGridViewFavoritesColumn, "\u2605"));
            }
            else if (TilePage == FavoritesPage.Recents)
            {
                columns.Add(new ColumnData(RECENTS_COLUMN, ColumnTypeEnum.DataGridViewRecentsColumn, "⏲"));
            }

            columns.Add(new ColumnData(ID_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn));
            columns.Add(new ColumnData(CLIENT_ID_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn));
            columns.Add(new ColumnData(FILE_ID_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn));
            columns.Add(new ColumnData(ITEM_TYPE_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn));
            columns.Add(new ColumnData(ICON_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn));
            columns.Add(new ColumnData(EXTENSION_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn));
            columns.Add(new ColumnData(CLIENT_NO_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn));
            columns.Add(new ColumnData(FILE_NO_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn));
            columns.Add(new ColumnData(PRECEDENT_ID_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn));
            columns.Add(
                new ColumnData(DESCRIPTION_COLUMN, ColumnTypeEnum.DataGridViewCustomTextColumn,
                CodeLookup.GetLookup("DASHBOARD", "DSCRPTN", "Description"), false)
                {
                    AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 100,
                    Sortable = true
                });
            columns.Add(new ColumnData(MODIFIED_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                CodeLookup.GetLookup("DASHBOARD", "DTMDFD", "Date Modified"))
            {
                Resizable = true,
                Sortable = true,
                Width = 110
            });

            ucDataView.SetColumns(columns);
        }

        private void CheckData(string query, string filter, int page, out int total)
        {
            _dataGridItems = DashboardTileDataProvider.GetFavorites(query, filter, _orderBy, page, out total, PageSize);
        }

        private void ChangeFavoriteStatus(DataGridViewCell cell)
        {
            var itemType = cell.OwningRow.Cells[ITEM_TYPE_COLUMN].Value.ToString().ToLower();
            switch (itemType)
            {
                case "client":
                    ProcessClient(cell);
                    break;
                case "file":
                    ProcessFile(cell);
                    break;
                case "precedent":
                    ProcessPrecedent(cell);
                    break;
            }
        }

        private void RowClick(object sender, int columnIndex, int rowIndex, bool doubleClick = false)
        {
            if (columnIndex < 0 || rowIndex < 0)
            {
                return;
            }

            var dataView = sender as DataGridView;
            var cell = dataView.Rows[rowIndex].Cells[columnIndex];

            if (columnIndex == ucDataView.GetColumnIndex(FAVORITES_COLUMN))
            {
                ChangeFavoriteStatus(cell);
            }
            else if (doubleClick || columnIndex == ucDataView.GetColumnIndex(DESCRIPTION_COLUMN))
            {
                var itemType = cell.OwningRow.Cells[ITEM_TYPE_COLUMN].Value.ToString().ToLower();
                switch (itemType)
                {
                    case "client":
                        OpenNewOMSTypeWindow(GetClient(cell));
                        break;
                    case "file":
                        OpenNewOMSTypeWindow(GetOmsFile(cell));
                        break;
                    case "precedent":
                        long precId;
                        Int64.TryParse(cell.OwningRow.Cells[PRECEDENT_ID_COLUMN].Value.ToString(), out precId);
                        var builder = new PrecedentBuilder(this.ParentForm);
                        builder.Build(precId);
                        Windows.Services.ProcessJobList(null);
                        break;
                }
            }
        }

        private void ProcessClient(DataGridViewCell cell)
        {
            var clientNo = cell.OwningRow.Cells[CLIENT_NO_COLUMN].Value.ToString();
            var description = cell.OwningRow.Cells[DESCRIPTION_COLUMN].Value.ToString();

            if (FavoritesProvider.HasClient(clientNo))
            {
                FavoritesProvider.RemoveClientFavorite(clientNo);
                cell.Value = false;
            }
            else
            {
                FavoritesProvider.AddClientFavorite(clientNo, description);
                cell.Value = true;
            }
        }

        private void ProcessFile(DataGridViewCell cell)
        {
            var client = GetClient(cell);
            var file = GetOmsFile(cell);
            if (FavoritesProvider.HasFile(client.ClientNo, file.FileNo))
            {
                FavoritesProvider.RemoveFileFavorite(client.ClientNo, file.FileNo, this);
                cell.Value = false;
            }
            else
            {
                var description = $"{client.ClientNo}/{file.FileNo}: {client.ClientName}{System.Environment.NewLine}{file.FileDescription}";
                FavoritesProvider.AddFileFavorite(client.ClientNo, file.FileNo, description, this);
                cell.Value = true;
            }
        }

        private void ProcessPrecedent(DataGridViewCell cell)
        {
            var precId = Convert.ToInt64(cell.OwningRow.Cells[PRECEDENT_ID_COLUMN].Value.ToString());

            if (FavoritesProvider.HasPrecedent(precId))
            {
                FavoritesProvider.RemovePrecedentFavorite(precId);
                cell.Value = false;
            }
            else
            {
                FavoritesProvider.AddPrecedentFavorite(precId);
                cell.Value = true;
            }
        }

        private FWBS.OMS.OMSFile GetOmsFile(DataGridViewCell cell)
        {
            long fileId;
            Int64.TryParse(cell.OwningRow.Cells[FILE_ID_COLUMN].Value.ToString(), out fileId);
            return FWBS.OMS.OMSFile.GetFile(fileId);
        }

        private FWBS.OMS.Client GetClient(DataGridViewCell cell)
        {
            long clientId;
            Int64.TryParse(cell.OwningRow.Cells[CLIENT_ID_COLUMN].Value.ToString(), out clientId);
            return FWBS.OMS.Client.GetClient(clientId);
        }

        #endregion

        #region BaseContainerPage

        public override void UpdateData(bool withScale = false)
        {
            int total;
            CheckData(_query, Filter, _currentPage, out total);
            OnQueryCompleted(total);

            var table = new DataTable();
            foreach (DataGridViewColumn column in ucDataView.dataGridView.Columns)
            {
                table.Columns.Add(column.Name);
            }

            var extensions = new List<string>();
            for (int i = 0; i < _dataGridItems.Count; i++)
            {
                table.Rows.Add(table.NewRow());

                if (table.Columns.Contains(FAVORITES_COLUMN))
                {
                    table.Rows[i][FAVORITES_COLUMN] = true;
                }
                
                table.Rows[i][ID_COLUMN] = _dataGridItems[i].Id;
                table.Rows[i][CLIENT_ID_COLUMN] = _dataGridItems[i].ClientId;
                table.Rows[i][FILE_ID_COLUMN] = _dataGridItems[i].FileId;
                table.Rows[i][ITEM_TYPE_COLUMN] = _dataGridItems[i].ItemType;
                table.Rows[i][EXTENSION_COLUMN] = _dataGridItems[i].Extension;
                table.Rows[i][CLIENT_NO_COLUMN] = _dataGridItems[i].ClientNo;
                table.Rows[i][FILE_NO_COLUMN] = _dataGridItems[i].FileNo;
                table.Rows[i][PRECEDENT_ID_COLUMN] = _dataGridItems[i].PrecedentId;
                table.Rows[i][DESCRIPTION_COLUMN] = _dataGridItems[i].Combo;
                table.Rows[i][MODIFIED_COLUMN] = _dataGridItems[i].Modified.ToLocalTime().ToShortDateString();
                table.Rows[i][ICON_COLUMN] = _dataGridItems[i].Icon;

                if (!string.IsNullOrEmpty(_dataGridItems[i].Extension))
                {
                    extensions.Add(_dataGridItems[i].Extension);
                }
            }

            extensions = extensions.Distinct().ToList();

            ucDataView.dataGridView.AutoGenerateColumns = false;
            ucDataView.dataGridView.DataSource = table;

            ucDataView.SetImageList(_imageListProvider.BuildImageList(extensions, DeviceDpi), ICON_COLUMN);

            if (ucDataView.dataGridView.SortedColumn == null)
            {
                ucDataView.dataGridView.ShowSorting(_sortColumn, _sortOrder);
            }
        }

        public override void ChangePage(object sender, int e)
        {
            _currentPage = e;
            UpdateData();
        }

        public override void StartSearch(object sender, string e)
        {
            _query = e;
            _currentPage = 1;
            UpdateData();
        }

        protected override void UpdateFileFavorites(object sender, EventArgs e)
        {
            if (this == sender || TilePage != FavoritesPage.Favorites)
            {
                return;
            }

            foreach (DataGridViewRow row in ucDataView.dataGridView.Rows)
            {
                if (row.Cells[ITEM_TYPE_COLUMN].Value.ToString().ToLower() != "file")
                {
                    continue;
                }

                DataGridViewCell cell = row.Cells[FAVORITES_COLUMN];
                cell.Value = FavoritesProvider.HasFile(row.Cells[CLIENT_NO_COLUMN].Value.ToString(),
                    row.Cells[FILE_NO_COLUMN].Value.ToString());
            }
        }

        public override void SetColumnSettings()
        {
            if (DashboardCellGuid != Guid.Empty && !IsConfigurationMode)
            {
                var settings = new DashboardConfigProvider(DashboardCode).GetColumnsSettings(DashboardCellGuid, Code);
                var setting = settings?.FirstOrDefault(s => s.SortOrder != SortOrder.None);
                if (setting != null)
                {
                    SetSortOrder(setting.Name, setting.SortOrder);
                }
            }
        }

        private void SaveColumnSettings()
        {
            if (!IsConfigurationMode)
            {
                var columns = new List<DashboardConfigProvider.ColumnsSettings.Column>();
                var setting = new DashboardConfigProvider.ColumnsSettings.Column() { Name = _sortColumn, Visible = true, SortOrder = _sortOrder };
                columns.Add(setting);

                new DashboardConfigProvider(DashboardCode).UpdateColumnsSettings(DashboardCellGuid, Code, columns);
            }
        }

        #endregion

        public enum FavoritesPage
        {
            Recents,
            Favorites
        }
    }
}
