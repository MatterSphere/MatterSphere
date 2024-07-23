using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.FinancialTile
{
    partial class FinancialDashboardItem : BaseContainerPage
    {
        private const string CLIENT_NO_COLUMN = "clmClientNo";
        private const string CLIENT_NAME_COLUMN = "clmClientName";
        private const string CLIENT_COLUMN = "clmClient";
        private const string WIP_HOURS_COLUMN = "clmWIPHours";
        private const string WIP_TIME_COLUMN = "clmWIPTime";
        private const string WIP_COSTS_COLUMN = "clmWIPCosts";
        private const string WIP_CHARGES_COLUMN = "clmWIPCharges";
        private const string WIP_TOTAL_COLUMN = "clmWIPTotal";

        private int _currentPage = 1;
        private DataProvider _dataProvider;
        private string _sortedColumn;
        private SortOrder _sortOrder;
        private CancellationTokenSource _cancellationTokenSource;

        public FinancialDashboardItem(FinancialPageEnum page, DataProvider dataProvider)
        {
            InitializeComponent();

            _cancellationTokenSource = new CancellationTokenSource();
            FinancialPage = page;
            _sortOrder = SortOrder.Ascending;
            _dataProvider = dataProvider;

            SetColumns();
            ucDataView.SortChanged += OnSortChanged;
        }

        public FinancialPageEnum FinancialPage { get; set; }

        private void OnSortChanged(object sender, DataGridViewEx.SortDataEventArgs e)
        {
            _sortOrder = e.Order;

            switch (e.Column)
            {
                case CLIENT_COLUMN:
                    _sortedColumn = null;
                    break;
                case WIP_HOURS_COLUMN:
                    _sortedColumn = "WIPHours";
                    break;
                case WIP_TIME_COLUMN:
                    _sortedColumn = "WIPTime";
                    break;
                case WIP_COSTS_COLUMN:
                    _sortedColumn = "WIPCosts";
                    break;
                case WIP_CHARGES_COLUMN:
                    _sortedColumn = "WIPCharges";
                    break;
                case WIP_TOTAL_COLUMN:
                    _sortedColumn = "WIPTotal";
                    break;
            }

            _currentPage = 1;
            ResetPages();
            UpdateData();
        }

        #region BaseContainerPage

        public override void UpdateData(bool withScale = false)
        {
            if (_dataProvider == null)
            {
                return;
            }

            ChangeEnable(false);
            var cancellationToken = _cancellationTokenSource.Token;
            switch (FinancialPage)
            {
                case FinancialPageEnum.Clients:
                    _dataProvider.GetClientFinancialItemsAsync(_sortedColumn, _sortOrder, _currentPage, PageSize, ProcessData, ErrorHandle, cancellationToken);
                    break;
                case FinancialPageEnum.Matters:
                    _dataProvider.GetMatterFinancialItemsAsync(_sortedColumn, _sortOrder, _currentPage, PageSize, ProcessData, ErrorHandle, cancellationToken);
                    break;
            }
        }

        private void ErrorHandle(Exception e)
        {
            EnableActions();
            ErrorBox.Show(e);
        }

        internal void ProcessData(List<FinancialRow> items, int total)
        {
            BeginInvoke((MethodInvoker)(() =>
            {
                OnQueryCompleted(total);

                var table = new DataTable();
                foreach (DataGridViewColumn column in ucDataView.dataGridView.Columns)
                {
                    table.Columns.Add(column.Name);
                }

                for (int i = 0; i < items.Count; i++)
                {
                    table.Rows.Add(table.NewRow());

                    table.Rows[i][CLIENT_NO_COLUMN] = items[i].ClientNumber;
                    table.Rows[i][CLIENT_NAME_COLUMN] = items[i].ClientName;
                    table.Rows[i][CLIENT_COLUMN] = $"{items[i].ClientNumber}-{items[i].ClientName}";
                    table.Rows[i][WIP_HOURS_COLUMN] = items[i].WIPHours;
                    table.Rows[i][WIP_TIME_COLUMN] = items[i].WIPTime;
                    table.Rows[i][WIP_COSTS_COLUMN] = items[i].WIPCosts;
                    table.Rows[i][WIP_CHARGES_COLUMN] = items[i].WIPCharges;
                    table.Rows[i][WIP_TOTAL_COLUMN] = items[i].WIPTotal;
                }
                ChangeEnable(true);
                ucDataView.dataGridView.AutoGenerateColumns = false;
                var bindingSource = new BindingSource { DataSource = table };
                ucDataView.dataGridView.DataSource = bindingSource;
                ucDataView.Refresh();
            }));
        }

        public override void ChangePage(object sender, int e)
        {
            _currentPage = e;
            UpdateData();
        }

        public override void StartSearch(object sender, string e)
        {
            var query = e.Trim().ToLower();

            CurrencyManager currencyManager = (CurrencyManager)BindingContext[ucDataView.dataGridView.DataSource];
            currencyManager.SuspendBinding();

            for (int rowIndex = 0; rowIndex < ucDataView.dataGridView.RowCount; rowIndex++)
            {
                ucDataView.dataGridView.Rows[rowIndex].Visible = true;
            }

            for (int rowIndex = 0; rowIndex < ucDataView.dataGridView.RowCount; rowIndex++)
            {
                if (ucDataView.dataGridView.Rows[rowIndex].Cells[CLIENT_COLUMN].Value.ToString().ToLower().Contains(query))
                {
                    ucDataView.dataGridView.Rows[rowIndex].Visible = true;
                }
                else
                {
                    ucDataView.dataGridView.Rows[rowIndex].Visible = false;
                }
            }

            currencyManager.ResumeBinding();
        }

        #endregion

        #region Private methods

        private void SetColumns()
        {
            var columns = new List<ColumnData>
            {
                new ColumnData(CLIENT_NO_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn),
                new ColumnData(CLIENT_NAME_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn),
                new ColumnData(CLIENT_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "DSCRPTN", "Description"))
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DefaultPadding = true,
                    MinimumWidth = 100
                },
                new ColumnData(WIP_HOURS_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "WIPHOURS", "WIP Hours"))
                {
                    Resizable = true,
                    Sortable = true,
                    Width = 100
                },
                new ColumnData(WIP_TIME_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "WIPTIME", "WIP Time"))
                {
                    Resizable = true,
                    Sortable = true,
                    Width = 100
                },
                new ColumnData(WIP_COSTS_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "WIPCOSTS", "WIP Costs"))
                {
                    Resizable = true,
                    Sortable = true,
                    Width = 100
                },
                new ColumnData(WIP_CHARGES_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "WIPCHARGES", "WIP Charges"))
                {
                    Resizable = true,
                    Sortable = true,
                    Width = 100
                },
                new ColumnData(WIP_TOTAL_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "WIPTOTAL", "WIP Total"))
                {
                    Resizable = true,
                    Sortable = true,
                    Width = 100
                }
            };
            
            ucDataView.SetColumns(columns);
        }

        #endregion

        public enum FinancialPageEnum
        {
            Clients,
            Matters
        }
    }
}
