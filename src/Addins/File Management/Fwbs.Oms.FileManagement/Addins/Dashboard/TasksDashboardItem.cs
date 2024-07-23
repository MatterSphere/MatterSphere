using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Dashboard;
using FWBS.OMS.EnquiryEngine;
using FWBS.OMS.UI.DataGridViewControls;
using FWBS.OMS.UI.UserControls.ContextMenu;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.FileManagement.Addins.Dashboard
{
    public partial class TasksDashboardItem : BaseContainerPage
    {
        private const string SELECTION_COLUMN = "clmSelection";
        private const string FLAG_COLUMN = "clmFlag";
        private const string DUE_COLUMN = "clmDue";
        private const string STATUS_COLUMN = "clmStatus";
        private const string DESCRIPTION_COLUMN = "clmDescription";
        private const string TYPE_COLUMN = "clmType";
        private const string MATTER_COLUMN = "clmMatter";
        private const string CREATED_BY_COLUMN = "clmCreatedBy";
        private const string TEAM_COLUMN = "clmTeam";
        private const string ASSIGNED_TO_COLUMN = "clmAssignedTo";
        private const string ACTIONS_COLUMN = "clmActions";
        private const string FILE_ID_COLUMN = "clmFileId";
        private const string TASK_ID_COLUMN = "clmTaskId";
        private readonly System.Drawing.Color _linkForeColor = System.Drawing.Color.FromArgb(21, 101, 192);
        private List<TaskRow> _dataGridItems;
        private string _query;
        private int _currentPage = 1;
        private List<long> _checkedTaskIds = new List<long>();
        private string _orderBy = "tskDue";
        private string _sortColumn = DUE_COLUMN;
        private SortOrder _sortOrder = SortOrder.Ascending;

        private ContextMenuItemBuilder _cmiBuilder;

        public TasksDashboardItem()
        {
            InitializeComponent();

            SetColumns();

            ucDataView.CellClicked += CellClickHandler;
            ucDataView.CellValueChanged += CellValueChangedHandler;
            ucDataView.CellDoubleClicked += CellDoubleClickHandler;
            ucDataView.SortChanged += OnSortChanged;

            _cmiBuilder = new ContextMenuItemBuilder();
        }

        public TasksPageEnum TasksPage { get; set; }

        #region Actions popup

        private IEnumerable<ContextMenuButton> PopulateActionPopup(DataGridViewActionsCell cell)
        {
            var row = cell.OwningRow;

            var buttons = new List<ContextMenuButton>();
            long taskId;
            Int64.TryParse(row.Cells[TASK_ID_COLUMN].Value.ToString(), out taskId);
            FWBS.OMS.Task task = FWBS.OMS.Task.GetTask(taskId);

            var markAsCompletedButton = _cmiBuilder.CreateTextButton(
                CodeLookup.GetLookup("DASHBOARD", "MARKCOMPL", "Mark As Complete"),
                clickHandler: delegate { CompleteTask(task); },
                enabled: !task.IsCompleted);
            buttons.Add(markAsCompletedButton);

            if (Session.CurrentSession.CurrentUser.IsInRoles("TASKUPDATEALL"))
            {
                var assignButton = _cmiBuilder.CreateTextButton(
                    CodeLookup.GetLookup("DASHBOARD", "ASSIGN", "Assign"),
                    clickHandler: delegate
                    {
                        AssignTask(task);
                    },
                    enabled: !task.IsCompleted);
                buttons.Add(assignButton);

                var unAssignButton = _cmiBuilder.CreateTextButton(
                    title: CodeLookup.GetLookup("DASHBOARD", "UNASSIGN", "Unassign"),
                    clickHandler: delegate { UnAssignTask(task); },
                    enabled: !task.IsCompleted && task.AssignedTo != null);
                buttons.Add(unAssignButton);
            }
            
            var deleteButton = _cmiBuilder.CreateTextButton(
                title: CodeLookup.GetLookup("DASHBOARD", "DELETE", "Delete"),
                clickHandler: delegate { DeleteTask(task); });
            buttons.Add(deleteButton);

            return buttons;
        }

        private void CompleteTask(FWBS.OMS.Task task)
        {
            if (!task.IsCompleted)
            {
                var taskProvider = new TasksProvider();
                taskProvider.Complete(task.ID);
                UpdateData();
            }
        }

        private void AssignTask(FWBS.OMS.Task task)
        {
            if (!task.IsCompleted)
            {
                var taskProvider = new TasksProvider();
                taskProvider.AssignTask(task.ID);
                UpdateData();
            }
        }

        private void UnAssignTask(FWBS.OMS.Task task)
        {
            if (!task.IsCompleted && task.AssignedTo != null)
            {
                var taskProvider = new TasksProvider();
                taskProvider.Unassign(task.ID);
                UpdateData();
            }
        }

        private void DeleteTask(FWBS.OMS.Task task)
        {
            var taskProvider = new TasksProvider();
            taskProvider.Delete(task.ID);
            UpdateData();
        }

        #endregion

        #region Private methods

        private void SetColumns()
        {
            var columns = new List<ColumnData>
            {
                new ColumnData(FILE_ID_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn),
                new ColumnData(TASK_ID_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn),
                new ColumnData(SELECTION_COLUMN, ColumnTypeEnum.DataGridViewBlueCheckBoxColumn, "☐", false)
                {
                    ReadOnly = false,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                },
                new ColumnData(FLAG_COLUMN, ColumnTypeEnum.DataGridViewFlagColumn, "⚑"),
                new ColumnData(DUE_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "DUE", "Due"))
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    DefaultPadding = true,
                    Sortable = true
                },
                new ColumnData(STATUS_COLUMN, ColumnTypeEnum.DataGridViewTaskStatusColumn,
                    CodeLookup.GetLookup("DASHBOARD", "STATUS", "Status"))
                {
                    TextAlignment = ucDataView.TextAlignmentEnum.Center,
                    Width = 70,
                    Sortable = true
                },
                new ColumnData(DESCRIPTION_COLUMN, ColumnTypeEnum.DataGridViewCustomTextColumn,
                    CodeLookup.GetLookup("DASHBOARD", "DSCRPTN", "Description"), false)
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DefaultPadding = true,
                    MinimumWidth = 100,
                    Sortable = true
                },
                new ColumnData(TYPE_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "TYPE", "Type"))
                {
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells,
                    Sortable = true
                },
                new ColumnData(MATTER_COLUMN, ColumnTypeEnum.DataGridViewCustomTextColumn,
                    CodeLookup.GetLookup("DASHBOARD", "FILE", "Matter"))
                {
                    Resizable = true,
                    Sortable = true,
                    Width = 80
                },
                new ColumnData(CREATED_BY_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "CRTDBY", "Created By"))
                {
                    Resizable = true,
                    Sortable = true,
                    Width = 100
                },
                new ColumnData(TEAM_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "TMNAME", "Team Name"))
                {
                    Resizable = true,
                    Sortable = true,
                    Width = 100
                },
                new ColumnData(ASSIGNED_TO_COLUMN, ColumnTypeEnum.DataGridViewTextBoxColumn,
                    CodeLookup.GetLookup("DASHBOARD", "ASSGNDTO", "Assigned To"))
                {
                    Resizable = true,
                    Sortable = true,
                    Width = 100
                },
                new ColumnData(ACTIONS_COLUMN, ColumnTypeEnum.DataGridViewActionsColumn,
                    CodeLookup.GetLookup("DASHBOARD", "ACTNS", "Actions"), false)
            };

            base.Columns = columns.Where(col => col.AllowManage || col.Sortable).ToArray();
            ucDataView.SetColumns(columns);
            ((DataGridViewActionsColumn)ucDataView.dataGridView.Columns[ACTIONS_COLUMN]).GetActions = PopulateActionPopup;
        }

        private void CheckData(string query, int page, out int total)
        {
            _dataGridItems = DashboardTileDataProvider.GetTasks(query, Filter, _orderBy, page, out total, PageSize);
        }

        private string Filter
        {
            get
            {
                switch (TasksPage)
                {
                    case TasksPageEnum.MyTasks:
                        return "MT";
                    case TasksPageEnum.MyTeamsTasks:
                        return "TT";
                    case TasksPageEnum.AllTasks:
                    default:
                        return "AT";
                }
            }
        }

        private TaskStatusEnum GetStatus(string code)
        {
            switch (code)
            {
                case "PASTDUE":
                    return TaskStatusEnum.PastDue;
                case "DUESOON":
                    return TaskStatusEnum.DueSoon;
                case "ONTIME":
                    return TaskStatusEnum.OnTime;
                case "CMPLT":
                    return TaskStatusEnum.Completed;
            }

            throw new ArgumentException($"{code} is not an expected task status code");
        }

        private void OpenFile(DataGridViewCell cell)
        {
            long omsFileId;
            Int64.TryParse(cell.OwningRow.Cells[FILE_ID_COLUMN].Value.ToString(), out omsFileId);
            OpenFile(omsFileId);
        }

        private void OpenTask(DataGridViewCell cell)
        {
            if (ViewMode)
            {
                return;
            }

            ViewMode = true;
            long taskId;
            Int64.TryParse(cell.OwningRow.Cells[TASK_ID_COLUMN].Value.ToString(), out taskId);
            View(taskId);
        }

        private void View(long id)
        {
            var task = Task.GetTask(id);
            base.OnEditFormOpening(
                "DSHBTSKADD",
                EnquiryMode.Edit,
                new KeyValueCollection
                {
                    { "tskid", id }
                });
        }

        private void OpenFile(long id)
        {
            FWBS.OMS.OMSFile file = FWBS.OMS.OMSFile.GetFile(id);
            OpenNewOMSTypeWindow(file);
        }

        private void UpdateDeleteButtonStatus()
        {
            OnDeleteEnableStatusChanged(_checkedTaskIds.Count > 0);
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

            if (e.ColumnIndex == ucDataView.GetColumnIndex(DESCRIPTION_COLUMN))
            {
                OpenTask(cell);
            }
            else if (e.ColumnIndex == ucDataView.GetColumnIndex(MATTER_COLUMN))
            {
                OpenFile(cell);
            }
        }

        private void CellValueChangedHandler(object sender, DataGridViewCellEventArgs e)
        {
            var dataView = sender as DataGridView;
            DataGridViewRow row = dataView.Rows[e.RowIndex];
            if (e.ColumnIndex != row.Cells[SELECTION_COLUMN].ColumnIndex)
            {
                return;
            }

            long taskId = ConvertDef.ToInt64(row.Cells[TASK_ID_COLUMN].Value, 0);
            if (taskId != 0)
            {
                _checkedTaskIds.Remove(taskId);
                if (((DataGridViewBlueCheckBoxCell)row.Cells[SELECTION_COLUMN]).IsChecked)
                    _checkedTaskIds.Add(taskId);
            }

            UpdateDeleteButtonStatus();
        }

        private void CellDoubleClickHandler(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            var dataView = sender as DataGridView;
            var cell = dataView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            OpenTask(cell);
        }

        private void SetSortOrder(string column, SortOrder order)
        {
            switch (column)
            {
                case DUE_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? "tskDue DESC"
                        : "tskDue";
                    break;
                case DESCRIPTION_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? "tskDesc DESC"
                        : "tskDesc";
                    break;
                case TYPE_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? "tsktypedesc DESC"
                        : "tsktypedesc";
                    break;
                case STATUS_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? "tskStatusDesc DESC"
                        : "tskStatusDesc";
                    break;
                case MATTER_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? "clNo DESC, fileNo"
                        : "clNo, fileNo";
                    break;
                case CREATED_BY_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? "CreatedBy DESC"
                        : "CreatedBy";
                    break;
                case TEAM_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? "tskTeamName DESC"
                        : "tskTeamName";
                    break;
                case ASSIGNED_TO_COLUMN:
                    _orderBy = order == SortOrder.Descending
                        ? "AssignedTo DESC"
                        : "AssignedTo";
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
            _checkedTaskIds.Clear();
            ResetPages();
            UpdateData();
            SaveColumnSettings();
        }

        #endregion

        #region BaseContainerPage

        public override void UpdateData(bool withScale = false)
        {
            int total;
            CheckData(_query, _currentPage, out total);
            OnQueryCompleted(total);

            var table = new DataTable();
            foreach (DataGridViewColumn column in ucDataView.dataGridView.Columns)
            {
                table.Columns.Add(column.Name);
            }

            var checkedTaskIds = new List<long>();
            DateTime emptyDueTime = new DateTime(1, 1, 1);

            foreach (TaskRow taskRow in _dataGridItems)
            {
                DataRow dataRow = table.NewRow();
                table.Rows.Add(dataRow);

                var status = GetStatus(taskRow.StatusCode);
                dataRow[FLAG_COLUMN] = status;
                dataRow[DUE_COLUMN] = taskRow.Due == emptyDueTime ? string.Empty : taskRow.Due.ToLocalTime().ToShortDateString();
                dataRow[STATUS_COLUMN] = status;
                dataRow[DESCRIPTION_COLUMN] = taskRow.Description;
                dataRow[TYPE_COLUMN] = taskRow.Type;
                dataRow[MATTER_COLUMN] = taskRow.Matter;
                dataRow[CREATED_BY_COLUMN] = taskRow.CreatedByName;
                dataRow[TEAM_COLUMN] = taskRow.Team;
                dataRow[ASSIGNED_TO_COLUMN] = taskRow.AssignedToName;
                dataRow[FILE_ID_COLUMN] = taskRow.FileId;
                dataRow[TASK_ID_COLUMN] = taskRow.Id;

                bool isChecked = _checkedTaskIds.Contains(taskRow.Id);
                dataRow[SELECTION_COLUMN] = isChecked;
                if (isChecked)
                    checkedTaskIds.Add(taskRow.Id);
            }

            _checkedTaskIds = checkedTaskIds;
            UpdateDeleteButtonStatus();

            ucDataView.dataGridView.AutoGenerateColumns = false;
            var bindingSource = new BindingSource { DataSource = table };
            ucDataView.dataGridView.DataSource = bindingSource;

            if (ucDataView.dataGridView.SortedColumn == null)
            {
                ucDataView.dataGridView.ShowSorting(_sortColumn, _sortOrder);
            }
        }

        public override void ChangePage(object sender, int e)
        {
            _currentPage = e;
            _checkedTaskIds.Clear();
            UpdateData();
        }

        public override void StartSearch(object sender, string e)
        {
            _query = e;
            ChangePage(sender, 1);
        }

        public override void AddNew(object sender, EventArgs e)
        {
            base.OnEditFormOpening("DSHBTSKADD", EnquiryMode.Add, null);
        }

        public override void Delete(object sender, EventArgs e)
        {
            if (_checkedTaskIds.Count == 1)
            {
                FWBS.OMS.Task task = FWBS.OMS.Task.GetTask(_checkedTaskIds[0]);
                DeleteTask(task);
                return;
            }

            if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("QDELETETASKS",
                    "Are you sure that you want to delete the tasks?") == DialogResult.Yes)
            {
                var taskProvider = new TasksProvider();
                taskProvider.Delete(_checkedTaskIds);
                UpdateData();
            }
        }
        
        public override void ChangeColumnVisibility(string name, bool visibility)
        {
            SetColumnVisibility(name, visibility);
            SaveColumnSettings();
        }

        private void SetColumnVisibility(string name, bool visibility)
        {
            ucDataView.dataGridView.Columns[name].Visible = visibility;
            base.Columns.First(col => col.Name == name).Visible = visibility;
        }

        public override void SetColumnSettings()
        {
            if (DashboardCellGuid != Guid.Empty && !IsConfigurationMode)
            {
                var settings = new DashboardConfigProvider(DashboardCode).GetColumnsSettings(DashboardCellGuid, Code);
                if (settings != null)
                {
                    foreach (var setting in settings)
                    {
                        SetColumnVisibility(setting.Name, setting.Visible);
                        if (setting.SortOrder != SortOrder.None)
                            SetSortOrder(setting.Name, setting.SortOrder);
                    }
                }
            }
        }

        private void SaveColumnSettings()
        {
            if (IsConfigurationMode)
                return;

            var columns = new List<DashboardConfigProvider.ColumnsSettings.Column>();
            foreach (var column in base.Columns)
            {
                var setting = new DashboardConfigProvider.ColumnsSettings.Column() { Name = column.Name, Visible = column.Visible };
                if (column.Name == _sortColumn)
                    setting.SortOrder = _sortOrder;

                columns.Add(setting);
            }
            new DashboardConfigProvider(DashboardCode).UpdateColumnsSettings(DashboardCellGuid, Code, columns);
        }

        #endregion

        public enum TasksPageEnum
        {
            MyTasks,
            MyTeamsTasks,
            AllTasks
        }
    }
}
