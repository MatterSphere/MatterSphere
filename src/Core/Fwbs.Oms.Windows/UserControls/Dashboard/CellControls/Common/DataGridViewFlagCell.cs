using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    public class DataGridViewFlagCell : DataGridViewTextBoxCell
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

        private string ConvertValue(object value)
        {
            var status = GetTaskStatusEnum(value);
            switch (status)
            {
                case TaskStatusEnum.PastDue:
                    return "⚑";
                case TaskStatusEnum.DueSoon:
                    return "⚑";
                default:
                    return "⚐";
            }
        }

        private Color GetForeColor(object value)
        {
            var status = GetTaskStatusEnum(value);
            switch (status)
            {
                case TaskStatusEnum.PastDue:
                    return System.Drawing.Color.FromArgb(247, 143, 143);
                case TaskStatusEnum.DueSoon:
                    return System.Drawing.Color.FromArgb(255, 185, 0);
                default:
                    return System.Drawing.Color.FromArgb(51, 51, 51);
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
