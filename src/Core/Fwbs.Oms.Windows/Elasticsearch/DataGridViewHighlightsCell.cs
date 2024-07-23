using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Elasticsearch
{
    public class DataGridViewHighlightsCell : DataGridViewTextBoxCell
    {
        #region Override Methods

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            var reader = new HighlightsReader();
            var info = reader.Read(formattedValue.ToString());
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, string.Empty,
                errorText, cellStyle, advancedBorderStyle, paintParts);

            foreach (var group in info.Groups)
            {
                var rectangle = new Rectangle
                {
                    Y = cellBounds.Y + cellBounds.Height / 3,
                    Height = cellBounds.Height / 3 + 2
                };

                var beforeSearchWord = info.Output.Substring(0, group.Index);
                var searchWord = info.Output.Substring(group.Index, group.Phrase.Length);
                Size beforeSearchWordSize =
                    TextRenderer.MeasureText(graphics, beforeSearchWord, cellStyle.Font, cellBounds.Size);
                Size searchWordSize = TextRenderer.MeasureText(graphics, searchWord, cellStyle.Font, cellBounds.Size);

                if (beforeSearchWordSize.Width > 5)
                {
                    rectangle.X = cellBounds.X + beforeSearchWordSize.Width - 5;
                    rectangle.Width = searchWordSize.Width - 6;
                }
                else
                {
                    rectangle.X = cellBounds.X + 2;
                    rectangle.Width = searchWordSize.Width - 6;
                }

                var brush = new SolidBrush(Color.FromArgb(216, 235, 255));
                graphics.FillRectangle(brush, rectangle);
                brush.Dispose();
            }
            
            cellStyle.BackColor = Color.Transparent;
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, info.Output,
                errorText, cellStyle, advancedBorderStyle, paintParts);
        }

        #endregion Override Methods
    }
}
