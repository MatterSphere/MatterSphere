using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    internal class DataGridViewFavoritesCell : DataGridViewTextBoxCell
    {
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            formattedValue = ConvertValue(value);
            cellStyle.ForeColor = GetForeColor(value);
            cellStyle.SelectionForeColor = cellStyle.ForeColor;
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue,
                errorText, cellStyle, advancedBorderStyle, paintParts);
        }

        protected override void OnMouseEnter(int rowIndex)
        {
            base.OnMouseEnter(rowIndex);
            this.DataGridView.Cursor = Cursors.Hand;
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);
            this.DataGridView.Cursor = Cursors.Default;
        }

        private string ConvertValue(object value)
        {
            return value.ToString() == Boolean.TrueString ? "\u2605" : "\u2606"; // "black star" or "white star"
        }

        private Color GetForeColor(object value)
        {
            return value.ToString() == "True"
                ? System.Drawing.Color.FromArgb(255, 185, 0)
                : System.Drawing.Color.FromArgb(51, 51, 51);
        }
    }
}
