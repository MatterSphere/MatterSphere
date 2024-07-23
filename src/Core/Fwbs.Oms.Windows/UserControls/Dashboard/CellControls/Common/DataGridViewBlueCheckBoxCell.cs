using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    public class DataGridViewBlueCheckBoxCell : DataGridViewCheckBoxCell
    {
        public bool IsChecked
        {
            get
            {
                return FWBS.Common.ConvertDef.ToBoolean(Value, false);
            }
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            var selectionColor = Color.FromArgb(208, 224, 242);
            var highlightColor = OwningRow.DataGridView.DefaultCellStyle.SelectionBackColor;
            var unHighlightColor = OwningRow.DataGridView.DefaultCellStyle.BackColor;

            bool isChecked = FWBS.Common.ConvertDef.ToBoolean(formattedValue, false);
            if (isChecked)
            {
                this.OwningRow.DefaultCellStyle.BackColor = selectionColor;
                this.OwningRow.DefaultCellStyle.SelectionBackColor = selectionColor;
            }
            else
            {
                this.OwningRow.DefaultCellStyle.BackColor = unHighlightColor;
                this.OwningRow.DefaultCellStyle.SelectionBackColor = highlightColor;
            }

            var boxSide = LogicalToDevice(12);
            var x = Convert.ToInt32(cellBounds.X + (cellBounds.Width - boxSide) / 2F);
            var y = Convert.ToInt32(cellBounds.Y + (cellBounds.Height - boxSide) / 2F);

            if ((paintParts & DataGridViewPaintParts.Background) == DataGridViewPaintParts.Background)
            {
                var backColor = (cellState & DataGridViewElementStates.Selected) != 0
                    ? cellStyle.SelectionBackColor
                    : cellStyle.BackColor;
                SolidBrush cellBackground = new SolidBrush(backColor);
                graphics.FillRectangle(cellBackground, cellBounds);
                cellBackground.Dispose();
            }

            if ((paintParts & DataGridViewPaintParts.Border) == DataGridViewPaintParts.Border)
            {
                PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            }
            
            if (isChecked)
            {
                using (var blueBrush = new SolidBrush(Color.FromArgb(21, 101, 192)))
                using (var checkMarkFont = new Font("Segoe UI Symbol", 6, FontStyle.Regular))
                {
                    graphics.FillRectangle(blueBrush, x, y, boxSide, boxSide);
                    graphics.DrawString("", checkMarkFont, Brushes.White,
                        new Rectangle(x, y, boxSide, boxSide));
                }
            }
            else
            {
                using (var pen = new Pen(Color.FromArgb(51, 51, 51), 1))
                {
                    graphics.DrawRectangle(pen, x, y, boxSide, boxSide);
                }
            }
        }

        private int LogicalToDevice(int value)
        {
            return this.OwningRow.DataGridView.LogicalToDeviceUnits(value);
        }
    }
}
