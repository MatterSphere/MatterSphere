using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.DataGridViewControls
{
    public class DataGridViewExColumnHeaderCell : DataGridViewColumnHeaderCell
    {
        public bool HeaderPressed { get; set; }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
            if (HeaderPressed)
            {
                var bounds = cellBounds;
                var borderSize = DataGridView?.LogicalToDeviceUnits(1) ?? 1;
                bounds.Width -= borderSize;
                bounds.Height -= borderSize;
                using (var pen = new Pen(Color.FromArgb(41, 86, 154)))
                {
                    graphics.DrawRectangle(pen, bounds);
                }
            }
        }

        protected override void OnMouseDown(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                HeaderPressed = true;
                this.Style.BackColor = Color.FromArgb(208, 224, 242);
                this.Style.SelectionBackColor = this.Style.BackColor;
            }
        }

        protected override void OnMouseUp(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseUp(e);
            HeaderPressed = false;
            var columns = DataGridView?.Columns;
            if (columns != null)
            {
                foreach(DataGridViewColumn column in columns)
                {
                    var columnHeader = column.HeaderCell as DataGridViewExColumnHeaderCell;
                    if (columnHeader != null)
                    {
                        columnHeader.HeaderPressed = false;
                        columnHeader.Style.BackColor = Color.White;
                        columnHeader.Style.SelectionBackColor = columnHeader.Style.BackColor;
                    }
                }
            }
        }
    }
}
