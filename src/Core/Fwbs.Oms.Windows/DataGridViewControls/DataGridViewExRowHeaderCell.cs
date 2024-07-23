using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.DataGridViewControls
{
    public class DataGridViewExRowHeaderCell : DataGridViewRowHeaderCell
    {
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates cellState, object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle, DataGridViewPaintParts paintParts)
        {
            if (DataGridView?.Columns?.Count > 0)
            {
                var column = DataGridView.Columns[0];
                column?.ProcessBeforeCellDisplayEvent(cellStyle, rowIndex, ref formattedValue);
            }
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue, errorText, cellStyle, advancedBorderStyle, paintParts);
        }
    }
}
