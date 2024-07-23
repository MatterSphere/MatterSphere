using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.OMS.UI.DataGridViewControls;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    public partial class ucDataView : UserControl
    {
        private System.Drawing.Font headerFont = new System.Drawing.Font("Segoe UI Semibold", 9F);

        private bool _contentClicked;

        public ucDataView()
        {
            InitializeComponent();

            dataGridView.SortChanged += OnSortChange;
        }

        public event EventHandler<DataGridViewCellEventArgs> CellClicked;
        public event EventHandler<DataGridViewCellEventArgs> CellDoubleClicked;
        public event EventHandler<DataGridViewCellEventArgs> CellValueChanged;
        public event EventHandler<DataGridViewEx.SortDataEventArgs> SortChanged;

        public void SetColumns(IEnumerable<ColumnData> columns)
        {
            var dataViewColumns = new List<DataGridViewColumn>();
            foreach (var column in columns)
            {
                DataGridViewColumn dataViewColumn = null;
                switch (column.ColumnType)
                {
                    case ColumnTypeEnum.DataGridViewFavoritesColumn:
                        dataViewColumn = new DataGridViewFavoritesColumn
                        {
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                        };
                        break;
                    case ColumnTypeEnum.DataGridViewRecentsColumn:
                        dataViewColumn = new DataGridViewRecentsColumn
                        {
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                        };
                        break;
                    case ColumnTypeEnum.DataGridViewTextBoxColumn:
                        dataViewColumn = new DataGridViewTextBoxColumn();
                        dataViewColumn.HeaderCell.Style.Font = headerFont;

                        if (!column.DefaultPadding)
                        {
                            dataViewColumn.DefaultCellStyle.Padding = new Padding(15, 0, 0, 0);
                            dataViewColumn.HeaderCell.Style.Padding = new Padding(15, 0, 0, 0);
                        }
                        break;
                    case ColumnTypeEnum.DataGridViewCustomTextColumn:
                        dataViewColumn = new DataGridViewCustomTextColumn();
                        dataViewColumn.HeaderCell.Style.Font = headerFont;
                        break;
                    case ColumnTypeEnum.DataGridViewImageColumn:
                        dataViewColumn = new DataGridViewImageColumn();
                        dataViewColumn.HeaderCell.Style.Font = headerFont;
                        dataViewColumn.CellTemplate = new DataGridViewCustomImageCell();
                        break;
                    case ColumnTypeEnum.DataGridViewActionsColumn:
                        dataViewColumn = new DataGridViewActionsColumn
                        {
                            AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader,
                            FlatStyle = System.Windows.Forms.FlatStyle.Flat,
                            Text = "...",
                            Resizable = System.Windows.Forms.DataGridViewTriState.False,
                            UseColumnTextForButtonValue = true
                        };
                        dataViewColumn.HeaderCell.Style.Font = headerFont;
                        break;
                    case ColumnTypeEnum.DataGridViewFlagColumn:
                        dataViewColumn = new DataGridViewFlagColumn
                        {
                            AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader
                        };
                        break;
                    case ColumnTypeEnum.DataGridViewTaskStatusColumn:
                        dataViewColumn = new DataGridViewTaskStatusColumn();
                        dataViewColumn.HeaderCell.Style.Font = headerFont;
                        break;
                    case ColumnTypeEnum.DataGridViewBlueCheckBoxColumn:
                        dataViewColumn = new DataGridViewBlueCheckBoxColumn();
                        break;
                    default:
                        throw new NotImplementedException();
                }

                dataViewColumn.Resizable = column.Resizable ? DataGridViewTriState.True : DataGridViewTriState.False;

                if (!string.IsNullOrWhiteSpace(column.Title))
                {
                    dataViewColumn.HeaderText = column.Title;
                }
                
                dataViewColumn.DataPropertyName = column.Name;
                dataViewColumn.SortMode = column.Sortable
                    ? DataGridViewColumnSortMode.Programmatic
                    : DataGridViewColumnSortMode.NotSortable;
                dataViewColumn.Name = column.Name;
                dataViewColumn.ReadOnly = column.ReadOnly;
                dataViewColumn.Visible = column.Visible;

                if (column.TextAlignment.HasValue)
                {
                    switch (column.TextAlignment.Value)
                    {
                        case TextAlignmentEnum.Left:
                            dataViewColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                            break;
                        case TextAlignmentEnum.Center:
                            dataViewColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            break;
                        case TextAlignmentEnum.Right:
                            dataViewColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            break;
                    }
                }

                if (column.Width.HasValue)
                {
                    dataViewColumn.Width = column.Width.Value;
                }

                if (column.MinimumWidth.HasValue)
                {
                    dataViewColumn.MinimumWidth = column.MinimumWidth.Value;
                }

                if (column.AutoSizeMode.HasValue)
                {
                    dataViewColumn.AutoSizeMode = column.AutoSizeMode.Value;
                }

                dataViewColumns.Add(dataViewColumn);
            }

            this.dataGridView.Columns.AddRange(dataViewColumns.ToArray());
        }

        public int GetColumnIndex(string name)
        {
            return dataGridView?.Columns[name] == null
                   ? -1
                   : dataGridView.Columns[name].Index;
        }

        public void SetImageList(ImageList images, string imageColumn)
        {
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                var labelColumn = column as DataGridViewCustomTextColumn;
                if (labelColumn != null)
                {
                    labelColumn.ImageList = images;
                    labelColumn.ImageColumn = imageColumn;
                }
            }
        }

        private void OnSortChange(object sender, DataGridViewEx.SortDataEventArgs e)
        {
            SortChanged?.Invoke(this, e);
        }

        #region UI events

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // reset flag, because we don't know yet where is clicked
            _contentClicked = false;
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // set flag to handle it later in double-clicks
            _contentClicked = true;

            Type columnType = ((DataGridView)sender).Columns[e.ColumnIndex].GetType();

            // handle single-clicks on certain column types only
            if (columnType == typeof(DataGridViewCustomTextColumn) ||
                columnType == typeof(DataGridViewFavoritesColumn) ||
                columnType == typeof(DataGridViewRecentsColumn) ||
                columnType == typeof(DataGridViewImageColumn))
            {
                CellClicked?.Invoke(sender, e);
            }
        }

        private void dataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var columnType = ((DataGridView)sender).Columns[e.ColumnIndex].GetType();

            // handle double-clicks outside the text area but within a cell area
            // handle double-clicks for a regular text columns (it is default behavior of each MSP data lists)
            if ((!_contentClicked && columnType == typeof(DataGridViewCustomTextColumn)) ||
                columnType == typeof(DataGridViewRecentsColumn) ||
                columnType == typeof(DataGridViewTextBoxColumn))
            {
                CellDoubleClicked?.Invoke(sender, e);
            }
        }

        private void dataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            CellValueChanged?.Invoke(sender, e);
        }

        private void dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView.IsCurrentCellDirty)
            {
                dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            var cell = dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (cell is DataGridViewFlagCell)
            {
                TaskStatusEnum status;
                Enum.TryParse(Convert.ToString(cell.Value), out status);
                cell.ToolTipText = TaskStatus.GetLabel(status);
            }
            else if (cell is DataGridViewFavoritesCell)
            {
                cell.ToolTipText = Convert.ToString(cell.Value) == Boolean.TrueString
                    ? CodeLookup.GetLookup("DASHBOARD", "FVRT", "Favorite")
                    : CodeLookup.GetLookup("DASHBOARD", "NTFVRT", "Not Favorite");
            }
        }

        #endregion

        public enum TextAlignmentEnum
        {
            Left,
            Center,
            Right
        }
    }
}
