using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public class DataGridViewEx : DataGridView
    {
        private const string _ascending = "\u0068";
        private const string _descending = "\u0069";
        public const int MinimumColumnWidth = 2; 
        private SortOrder _sortOrder;

        /// <summary>
        /// Static fake DataGrid is used for backward compatibility
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        private static DataGrid _fakeGrid;

        /// <summary>
        /// Label that displays caption content
        /// </summary>
        private Label _captionLabel;

        private DataGridViewSelectedRowCollection _alternativeSelection;

        public DataGridViewEx()
        {
            this.RowTemplate = new DataGridViewControls.DataGridViewExRow();
        }

        public event EventHandler<SortDataEventArgs> SortChanged;

        [Browsable(false)]
        [DefaultValue(null)]
        public new DataGridViewColumn SortedColumn { get; set; }

        /// <summary>
        /// Gets or sets index of the row that currently has focus.
        /// (Adaptive property, consistent with DataGrid.CurrentRowIndex)
        /// </summary>
        [Browsable(false)]
        [DefaultValue(-1)]
        public int CurrentRowIndex
        {
            get
            {
                return this.CurrentRow?.Index ?? -1;
            }
            set
            {
                ClearSelection();
                DataGridViewRow currentRow = this.Rows[value];
                this.CurrentCell = currentRow.Cells.FirstOrDefault<DataGridViewCell>(c => c.Visible);
                currentRow.Selected = true;
            }
        }

        /// <summary>
        /// Gets the number of rows visible.
        /// (Adaptive property, consistent with DataGrid.VisibleRowCount)
        /// </summary>
        [Browsable(false)]
        public int VisibleRowCount => this.DisplayedRowCount(true);

        /// <summary>
        /// Created for support compiling old SearchList scripts that contains getters/setters DataGrid.TableStyles
        /// (Empty property, consistent with DataGrid.TableStyles)
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public GridTableStylesCollection TableStyles
        {
            get
            {
                if (_fakeGrid == null)
                {
                    _fakeGrid = new DataGrid();
                    _fakeGrid.TableStyles.Add(new DataGridTableStyle());
                }

                return _fakeGrid.TableStyles;
            }
        }

        /// <summary>
        /// Label associated with DataGridView
        /// </summary>
        public Label CaptionLabel
        {
            get
            {
                return _captionLabel;
            }

            set
            {
                _captionLabel = value;
            }
        }

        /// <summary>
        /// Displays whether caption is visible
        /// </summary>
        [DefaultValue(false)]
        public bool CaptionVisible
        {
            get
            {
                return _captionLabel != null ? _captionLabel.Visible : false;
            }
            set
            {
                if (_captionLabel != null)
                {
                    _captionLabel.Visible = value;
                }
            }
        }

        /// <summary>
        /// Access content of CaptionLabel
        /// </summary>
        [DefaultValue("")]
        public string CaptionText
        {
            get
            {
                return _captionLabel != null ? _captionLabel.Text : string.Empty;
            }
            set
            {
                if (_captionLabel != null)
                {
                    _captionLabel.Text = value;
                }
            }
        }

        public override DataGridViewAdvancedBorderStyle AdjustedTopLeftHeaderBorderStyle
        {
            get
            {
                return new DataGridViewAdvancedBorderStyle()
                {
                    Top = DataGridViewAdvancedCellBorderStyle.None,
                    Left = DataGridViewAdvancedCellBorderStyle.None,
                    Right = DataGridViewAdvancedCellBorderStyle.None,
                    Bottom = DataGridViewAdvancedCellBorderStyle.Single
                };
            }
        }

        public override DataGridViewAdvancedBorderStyle AdjustColumnHeaderBorderStyle(
            DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyleInput,
            DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceHolder,
            bool firstDisplayedColumn, bool lastVisibleColumn)
        {
            dataGridViewAdvancedBorderStylePlaceHolder.Left = DataGridViewAdvancedCellBorderStyle.None;
            dataGridViewAdvancedBorderStylePlaceHolder.Right = DataGridViewAdvancedCellBorderStyle.None;
            dataGridViewAdvancedBorderStylePlaceHolder.Top = DataGridViewAdvancedCellBorderStyle.None;
            dataGridViewAdvancedBorderStylePlaceHolder.Bottom = DataGridViewAdvancedCellBorderStyle.Single;
            return dataGridViewAdvancedBorderStylePlaceHolder;
        }

        protected override void OnCellMouseDown(DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == -1) return;

            _alternativeSelection = null;

            if (e.RowIndex >= 0 && MultiSelect && SelectionMode == DataGridViewSelectionMode.FullRowSelect
                && e.Button == MouseButtons.Left && (ModifierKeys & (Keys.Control | Keys.Shift)) == 0
                && SelectedRows.Count > 1 && Rows[e.RowIndex].Selected)
            {
                _alternativeSelection = SelectedRows;
            }

            base.OnCellMouseDown(e);

            if (_alternativeSelection != null)
            {
                var defaultSelection = SelectedRows;
                foreach (DataGridViewRow row in _alternativeSelection)
                    row.Selected = true;

                _alternativeSelection = defaultSelection;
            }
        }

        protected override void OnCellMouseUp(DataGridViewCellMouseEventArgs e)
        {
            if (_alternativeSelection != null)
            {
                ClearSelection();

                foreach (DataGridViewRow row in _alternativeSelection)
                    row.Selected = true;

                _alternativeSelection = null;
            }

            base.OnCellMouseUp(e);
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            base.ScaleControl(factor, specified);

            if (factor.Width != 1 && (specified & BoundsSpecified.Size) != 0)
            {
                if (RowHeadersWidthSizeMode == DataGridViewRowHeadersWidthSizeMode.DisableResizing)
                {
                    RowHeadersWidth = Convert.ToInt32(RowHeadersWidth * factor.Width);
                }

                if (AutoSizeRowsMode == DataGridViewAutoSizeRowsMode.None)
                {
                    var rowHeight = Convert.ToInt32(RowTemplate.Height * factor.Height);
                    RowTemplate.MinimumHeight = Convert.ToInt32(RowTemplate.MinimumHeight * factor.Height);
                    RowTemplate.Height = rowHeight;
                }

                if (ColumnHeadersHeightSizeMode == DataGridViewColumnHeadersHeightSizeMode.DisableResizing)
                {
                    ColumnHeadersHeight = Convert.ToInt32(ColumnHeadersHeight * factor.Height);
                }

                if (AutoSizeColumnsMode == DataGridViewAutoSizeColumnsMode.None)
                {
                    foreach (DataGridViewColumn column in Columns)
                    {
                        if (column.AutoSizeMode == DataGridViewAutoSizeColumnMode.NotSet)
                        {
                            column.Width = Convert.ToInt32(column.Width * factor.Width);
                        }
                    }
                }

                OnDataSourceChanged(EventArgs.Empty);
            }
        }
        
        
        protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.RowIndex == -1 && e.ColumnIndex >= 0 &&
                Columns[e.ColumnIndex].SortMode != DataGridViewColumnSortMode.NotSortable)
            {
                SortDataSource(Columns[e.ColumnIndex]);
            }

            base.OnCellMouseClick(e);
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == -1 && SortedColumn?.Index == e.ColumnIndex)
            {
                e.PaintBackground(e.CellBounds, false);
                e.Paint(e.CellBounds, DataGridViewPaintParts.ContentForeground);

                var order = SortedColumn.SortMode == DataGridViewColumnSortMode.Programmatic
                    ? _sortOrder
                    : SortedColumn.HeaderCell.SortGlyphDirection;
                if (order != SortOrder.None)
                {
                    int textWidth = Math.Max(TextRenderer.MeasureText(Convert.ToString(e.FormattedValue), e.CellStyle.Font).Width, LogicalToDeviceUnits(1));
                    int offset = e.CellBounds.X + textWidth + e.CellStyle.Padding.Left;
                    using (var font = new Font("Wingdings 3", e.CellStyle.Font.SizeInPoints, FontStyle.Regular, GraphicsUnit.Point, 2))
                    {
                        TextRenderer.DrawText(e.Graphics, order == SortOrder.Ascending ? _ascending : _descending,
                            font,
                            new Rectangle(offset, e.CellBounds.Y, e.CellBounds.Width - textWidth - e.CellStyle.Padding.Left, e.CellBounds.Height),
                            Color.FromArgb(51, 51, 51), TextFormatFlags.VerticalCenter | TextFormatFlags.Left);
                    }
                }
                e.Handled = true;
            }
            else
            {
                base.OnCellPainting(e);
            }
        }
        
        public void ShowSorting(string columnName, SortOrder order)
        {
            foreach (DataGridViewColumn gridColumn in this.Columns)
            {
                if (gridColumn.DataPropertyName == columnName)
                {
                    SetSorting(gridColumn, order);
                    break;
                }
            }
        }
        
        private void SetSorting(DataGridViewColumn column, SortOrder order)
        {
            SortedColumn = column;

            if (column.SortMode == DataGridViewColumnSortMode.Programmatic)
            {
                column.HeaderCell.SortGlyphDirection = order;
                _sortOrder = order;
            }
        }

        private void SortDataSource(DataGridViewColumn column)
        {
            SetSorting(column);
            _sortOrder = column.HeaderCell.SortGlyphDirection;
            if (column.SortMode == DataGridViewColumnSortMode.Programmatic)
            {
                SortChanged?.Invoke(this, new SortDataEventArgs(column.DataPropertyName, column.HeaderCell.SortGlyphDirection));
            }

            column.HeaderCell.SortGlyphDirection = _sortOrder;
        }

        private void SetSorting(DataGridViewColumn column)
        {
            if (column.SortMode == DataGridViewColumnSortMode.Programmatic)
            {
                if (column == SortedColumn)
                {
                    _sortOrder = _sortOrder == SortOrder.Ascending
                        ? SortOrder.Descending
                        : SortOrder.Ascending;
                }
                else
                {
                    _sortOrder = SortOrder.Ascending;
                    if (SortedColumn != null)
                        SortedColumn.HeaderCell.SortGlyphDirection = SortOrder.None;
                }

                column.HeaderCell.SortGlyphDirection = _sortOrder;
            }
            
            SortedColumn = column;
        }

        public class SortDataEventArgs : EventArgs
        {
            public SortDataEventArgs(string column, SortOrder order)
            {
                Column = column;
                Order = order;
            }

            public string Column { get; }
            public SortOrder Order { get; }
        }
    }
}
