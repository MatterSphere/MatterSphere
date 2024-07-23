using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.DataGridViewControls
{
    public class DataGridViewActionsCell : DataGridViewButtonCell
    {
        /// <summary>
        /// Content width on 100% scaling
        /// </summary>
        private const float _defaultTextSize = 41;

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left)
            {
                var contentSize = TextRenderer.MeasureText((string)Value, OwningActionColumn?.ActionFont);
                var contentArea = new Rectangle(ContentBounds.Location, contentSize);
                if (contentArea.Contains(e.Location))
                {
                    DataGridView.CurrentCell = this;

                    var fontOffset = contentSize.Width / _defaultTextSize;
                    var cellArea = DataGridView.RectangleToScreen(DataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false));
                    var popUpLocation = new Point(cellArea.Left + ContentBounds.Left + (int)(20 * fontOffset), cellArea.Bottom - 13);
                    DataGridView.BeginInvoke((Action)delegate () {
                        OwningActionColumn.ShowActionsPopup(this, popUpLocation);
                    });
                }
            }
        }

        protected override void OnDoubleClick(DataGridViewCellEventArgs e)
        {
            OwningActionColumn.CloseActionsPopup();
            base.OnDoubleClick(e);
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates elementState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            OwningActionColumn.ProcessBeforeCellDisplayEvent(cellStyle, rowIndex, ref formattedValue);
            paintParts = paintParts & ~DataGridViewPaintParts.ContentBackground;
            cellStyle.Font = OwningActionColumn?.ActionFont;
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
        }

        public DataGridViewActionsColumn OwningActionColumn
        {
            get
            {
                return (DataGridViewActionsColumn)OwningColumn;
            }
        }
    }
}
