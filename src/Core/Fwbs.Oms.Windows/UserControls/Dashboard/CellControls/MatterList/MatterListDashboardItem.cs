using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;
using FWBS.OMS.UI.DataGridViewControls;
using FWBS.OMS.UI.UserControls.ContextMenu;
using FWBS.OMS.UI.Windows;
using Infragistics.Win.Misc;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.MatterList
{
    internal partial class MatterListDashboardItem : BaseContainerPage
    {
        private List<MatterRow> _dataGridItems;
        private const string FAVORITES_COLUMN = "clmFavorites";
        private const string CLIENT_ID_COLUMN = "clmClientId";
        private const string FILE_ID_COLUMN = "clmFileId";
        private const string CLIENT_NO_COLUMN = "clmClientNo";
        private const string FILE_NO_COLUMN = "clmFileNo";
        private const string NUMBER_COLUMN = "clmNumber";
        private const string BUDGET_COLUMN = "clmBudget";
        private const string DESCRIPTION_COLUMN = "clmDescription";
        private const string REVIEW_DATE_COLUMN = "clmReviewDate";
        private const string ACTION_COLUMN = "clmActions";

        private const string CLIENT_NO_FIELD = "clNo";
        private const string FILE_NO_FIELD = "fileNo";
        private const string FILE_DESCRIPTION_FIELD = "fileDesc";
        private const string FILE_REVIEW_DATE_FIELD = "fileReviewDate";

        private string _query;
        private string _filter;
        private int _currentPage = 1;
        private string _orderBy;
        private string _sortColumn;
        private SortOrder _sortOrder = SortOrder.Ascending;
        private ContextMenuItemBuilder _cmiBuilder;

        private MatterListDashboardItem()
        {
            InitializeComponent();
            
            _cmiBuilder = new ContextMenuItemBuilder();

            ucDataView.CellClicked += CellClickHandler;
            ucDataView.CellDoubleClicked += CellDoubleClickHandler;
            ucDataView.SortChanged += OnSortChanged;
        }

        public MatterListDashboardItem(MatterListPageEnum page) : this()
        {
            MatterListPage = page;

            if (page == MatterListPageEnum.MattersForReview)
            {
                _filter = "OD";
                _orderBy = FILE_REVIEW_DATE_FIELD;
                _sortColumn = REVIEW_DATE_COLUMN;
            }
            else if (page == MatterListPageEnum.MatterList)
            {
                _filter = null;
                _orderBy = $"{CLIENT_NO_FIELD}, {FILE_NO_FIELD}";
                _sortColumn = NUMBER_COLUMN;
            }

            SetColumns();
        }

        public MatterListPageEnum MatterListPage { get; private set; }

        #region Actions popup
        private IEnumerable<ContextMenuButton> PopulateActionPopup(DataGridViewActionsCell cell)
        {
            var row = cell.OwningRow;

            var buttons = new List<ContextMenuButton>();
            long fileId;
            Int64.TryParse(row.Cells[FILE_ID_COLUMN].Value.ToString(), out fileId);
            var addTimeButton = _cmiBuilder.CreateTextButton(
                title: CodeLookup.GetLookup("DASHBOARD", "ADDTIME", "Add Time"),
                clickHandler: delegate { AddTime(fileId); });
            buttons.Add(addTimeButton);

            long clientId;
            Int64.TryParse(row.Cells[CLIENT_ID_COLUMN].Value.ToString(), out clientId);
            var openClientButton = _cmiBuilder.CreateTextButton(
                title: CodeLookup.GetLookup("DASHBOARD", "VWCLNT", "View Client"),
                clickHandler: delegate { OpenClient(clientId); });
            buttons.Add(openClientButton);

            return buttons;
        }

        private void AddTime(long fileId)
        {
            FWBS.OMS.OMSFile omsFile = FWBS.OMS.OMSFile.GetFile(fileId);
            FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(Session.CurrentSession.DefaultSystemForm(SystemForms.ManualTimeWizard), omsFile, null);
        }

        private void OpenClient(long id)
        {
            FWBS.OMS.Client client = FWBS.OMS.Client.GetClient(id);
            OpenNewOMSTypeWindow(client);
        }

        #endregion

        #region Private methods

        private void SetColumns()
        {
            var columns = new List<ColumnData>
            {
                new ColumnData(FAVORITES_COLUMN, ColumnTypeEnum.DataGridViewFavoritesColumn, "\u2605"),
                new ColumnData(CLIENT_ID_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn),
                new ColumnData(FILE_ID_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn),
                new ColumnData(CLIENT_NO_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn),
                new ColumnData(FILE_NO_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn),
                new ColumnData(NUMBER_COLUMN, ColumnTypeEnum.DataGridViewCustomTextColumn,
                    CodeLookup.GetLookup("DASHBOARD", "FILE", "Matter"))
                {
                    DefaultPadding = true,
                    Resizable = true,
                    Sortable = true,
                    Width = 80
                },
                new ColumnData(BUDGET_COLUMN, ColumnTypeEnum.DataGridViewImageColumn,
                    CodeLookup.GetLookup("DASHBOARD", "BUDGET", "Budget"))
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader,
                    TextAlignment = ucDataView.TextAlignmentEnum.Center,
                    MinimumWidth = 20
                },
                new ColumnData(DESCRIPTION_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "DSCRPTN", "Description"))
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth = 100,
                    Sortable = true
                },
                new ColumnData(REVIEW_DATE_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "RVWDT", "Review Date"))
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    DefaultPadding = true,
                    Sortable = true,
                    Visible = MatterListPage == MatterListPageEnum.MattersForReview
                },
                new ColumnData(ACTION_COLUMN, ColumnTypeEnum.DataGridViewActionsColumn,
                    CodeLookup.GetLookup("DASHBOARD", "ACTNS", "Actions"), false)
            };

            ucDataView.SetColumns(columns);
            ((DataGridViewActionsColumn)ucDataView.dataGridView.Columns[ACTION_COLUMN]).GetActions = PopulateActionPopup;
        }

        private void CheckData(string query, string filter, int page, out int total)
        {
            _dataGridItems = DashboardTileDataProvider.GetMatterList(query, filter, _orderBy, page, out total, PageSize);
        }

        private void OpenFile(DataGridViewCell cell)
        {
            var file = GetOmsFile(cell);
            OpenNewOMSTypeWindow(file);
        }

        private void ChangeFavoriteStatus(DataGridViewCell cell)
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

        #region UI events

        private void CellClickHandler(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var dataView = sender as DataGridView;
            var cell = dataView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (e.ColumnIndex == ucDataView.GetColumnIndex(FAVORITES_COLUMN))
            {
                ChangeFavoriteStatus(cell);
            }
            else if (e.ColumnIndex == ucDataView.GetColumnIndex(BUDGET_COLUMN))
            {
                OpenBudgetPopup(cell);
            }
            else if (e.ColumnIndex == ucDataView.GetColumnIndex(NUMBER_COLUMN))
            {
                OpenFile(cell);
            }
        }

        private void CellDoubleClickHandler(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            var dataView = sender as DataGridView;
            var cell = dataView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            OpenFile(cell);
        }

        private void SetSortOrder(string column, SortOrder order)
        {
            switch (column)
            {
                case NUMBER_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? $"{CLIENT_NO_FIELD} DESC, {FILE_NO_FIELD}"
                        : $"{CLIENT_NO_FIELD}, {FILE_NO_FIELD}";
                    break;
                case DESCRIPTION_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? $"{FILE_DESCRIPTION_FIELD} DESC"
                        : FILE_DESCRIPTION_FIELD;
                    break;
                case REVIEW_DATE_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? $"{FILE_REVIEW_DATE_FIELD} DESC"
                        : FILE_REVIEW_DATE_FIELD;
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

        #region Budget popup

        private UltraPeekPopup _popup;
        private void OpenBudgetPopup(DataGridViewCell cell)
        {
            long omsFileId;
            Int64.TryParse(cell.OwningRow.Cells[FILE_ID_COLUMN].Value.ToString(), out omsFileId);

            var popupContent = new BudgetPopup();
            var builder = new BudgetBuilder(popupContent, FWBS.OMS.OMSFile.GetFile(omsFileId));
            builder.BuildStandardView();
            var k = DeviceDpi / 96F;
            popupContent.Scale(new SizeF(k, k));
            popupContent.FileClicked += PopupFileClickedHandler;

            _popup = new UltraPeekPopup
            {
                ContentMargin = new Padding(0),
                Content = popupContent,
                Appearance = new Infragistics.Win.Appearance { BorderColor = Color.FromArgb(244, 244, 244) }
            };

            _popup.Closed += PopupClosedHandler;

            var point = new Point(
                cell.AccessibilityObject.Bounds.Location.X + cell.ContentBounds.Width + cell.ContentBounds.X,
                cell.AccessibilityObject.Bounds.Location.Y + cell.AccessibilityObject.Bounds.Height / 2);
            _popup.Show(point, Infragistics.Win.Peek.PeekLocation.RightOfItem);
        }

        private void PopupClosedHandler(object sender, EventArgs e)
        {
            var content = _popup.Content as BudgetPopup;
            content.FileClicked -= PopupFileClickedHandler;
        }

        private void PopupFileClickedHandler(object sender, EventArgs e)
        {
            _popup.Close();
        }

        #endregion

        #region BaseContainerPage

        public override void UpdateData(bool withScale = false)
        {
            int total;
            CheckData(_query, _filter, _currentPage, out total);
            OnQueryCompleted(total);

            var calculator = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "calculator");
            FavoritesProvider.UpdateFavoritesFiles();
            var favorites = FavoritesProvider.FavoritesFiles;

            var table = new DataTable();
            foreach (DataGridViewColumn column in ucDataView.dataGridView.Columns)
            {
                table.Columns.Add(column.Name);
            }

            table.Columns[BUDGET_COLUMN].DataType = calculator.GetType();

            for (int i = 0; i < _dataGridItems.Count; i++)
            {
                table.Rows.Add(table.NewRow());

               table.Rows[i][FAVORITES_COLUMN] = favorites.Any(fav =>
                    fav.ClientNo == _dataGridItems[i].ClientNo && fav.FileNo == _dataGridItems[i].FileNo);
                table.Rows[i][BUDGET_COLUMN] = calculator;
                table.Rows[i][NUMBER_COLUMN] = _dataGridItems[i].Number;
                table.Rows[i][DESCRIPTION_COLUMN] = _dataGridItems[i].Description;
                table.Rows[i][CLIENT_ID_COLUMN] = _dataGridItems[i].ClientId;
                table.Rows[i][FILE_ID_COLUMN] = _dataGridItems[i].FileId;
                table.Rows[i][CLIENT_NO_COLUMN] = _dataGridItems[i].ClientNo;
                table.Rows[i][FILE_NO_COLUMN] = _dataGridItems[i].FileNo;
                table.Rows[i][REVIEW_DATE_COLUMN] = _dataGridItems[i].ReviewDate.HasValue
                    ? _dataGridItems[i].ReviewDate.Value.ToLocalTime().ToShortDateString()
                    : string.Empty;
            }

            ucDataView.dataGridView.AutoGenerateColumns = false;
            var bindingSource = new BindingSource {DataSource = table};
            ucDataView.dataGridView.DataSource = bindingSource;

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

        public override void AddNew(object sender, EventArgs e)
        {
            FWBS.OMS.UI.Windows.Services.Wizards.CreateFile(true);
            UpdateData();
        }

        public override void CallAction(object sender, string code)
        {
            _filter = code;
            _currentPage = 1;
            UpdateData();
        }

        protected override void UpdateFileFavorites(object sender, EventArgs e)
        {
            if (this == sender)
            {
                return;
            }

            foreach (DataGridViewRow row in ucDataView.dataGridView.Rows)
            {
                DataGridViewCell cell = row.Cells[FAVORITES_COLUMN];
                cell.Value = FavoritesProvider.FavoritesFiles.Any(it =>
                    it.FileNo == row.Cells[FILE_NO_COLUMN].Value.ToString() &&
                    it.ClientNo == row.Cells[CLIENT_NO_COLUMN].Value.ToString());
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

        public enum MatterListPageEnum
        {
            MatterList,
            MattersForReview
        }
    }
}
