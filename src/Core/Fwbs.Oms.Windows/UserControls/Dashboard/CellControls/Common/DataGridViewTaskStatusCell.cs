using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    public class DataGridViewTaskStatusCell : DataGridViewTextBoxCell
    {
        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, string.Empty,
                errorText, cellStyle, advancedBorderStyle, paintParts);

            var rectangle = new Rectangle
            {
                Y = cellBounds.Y + 5,
                Height = cellBounds.Height - 10,
                X = cellBounds.X + 5,
                Width = cellBounds.Width - 10
            };

            var brush = new SolidBrush(GetBackColor(value));
            graphics.FillRectangle(brush, rectangle);
            brush.Dispose();
            
            cellStyle.BackColor = Color.Transparent;
            cellStyle.SelectionBackColor = Color.Transparent;
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, GetLabel(value),
                errorText, cellStyle, advancedBorderStyle, paintParts);
        }

        private string GetLabel(object value)
        {
            var status = GetTaskStatusEnum(value);
            return TaskStatus.GetLabel(status);
        }

        private Color GetBackColor(object value)
        {
            var status = GetTaskStatusEnum(value);
            switch (status)
            {
                case TaskStatusEnum.PastDue:
                    return System.Drawing.Color.FromArgb(253, 227, 229);
                case TaskStatusEnum.DueSoon:
                    return System.Drawing.Color.FromArgb(255, 241, 204);
                default:
                    return System.Drawing.Color.FromArgb(224, 240, 238);
            }
        }

        private TaskStatusEnum GetTaskStatusEnum(object value)
        {
            TaskStatusEnum status;
            Enum.TryParse(value.ToString(), out status);

            return status;
        }
    }
}
