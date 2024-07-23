using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.DataGridViewControls;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    internal class DataGridViewCustomTextCell : DataGridViewTextBoxCell
    {
        #region Override Methods

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            formattedValue = this.OwningLabelColumn.GetText(value);
            OwningLabelColumn.ProcessBeforeCellDisplayEvent(cellStyle, rowIndex, ref formattedValue);
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, cellState, value, formattedValue,
                errorText, cellStyle, advancedBorderStyle, paintParts);

            if (OwningLabelColumn.HasImages)
            {
                PaintImage(graphics, cellBounds, cellStyle.Padding, rowIndex);
            }
        }

        private void PaintImage(Graphics graphics, Rectangle cellBounds, Padding cellPadding, int rowIndex)
        {
            int imageSize = DataGridView.LogicalToDeviceUnits(16);

            int imageIndex = -1;
            if (OwningLabelColumn.ImageColumn != string.Empty)
            {
                DataTable dataSource = DataGridView.DataSource as DataTable;
                if (dataSource != null)
                {
                    try
                    {
                        object idx = dataSource.DefaultView[rowIndex][OwningLabelColumn.ImageColumn];
                        if (idx != DBNull.Value)
                        {
                            if (!string.IsNullOrEmpty(idx as string))
                            {
                                imageIndex = OwningLabelColumn.ImageList.Images.IndexOfKey((string)idx);
                            }
                            else if (idx is int)
                            {
                                imageIndex = (int)idx;
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else
            {
                imageIndex = OwningLabelColumn.ImageIndex;
            }

            Rectangle rect;
            if (DataGridView.RightToLeft == RightToLeft.Yes)
            {
                rect = new Rectangle(cellBounds.Right - cellPadding.Right + 5, cellBounds.Top + (cellBounds.Height - imageSize) / 2, imageSize, imageSize);
            }
            else
            {
                rect = new Rectangle(cellBounds.Left + cellPadding.Left - imageSize - 5, cellBounds.Top + (cellBounds.Height - imageSize) / 2, imageSize, imageSize);
            }

            if (imageIndex >= 0 && imageIndex < OwningLabelColumn.ImageList.Images.Count)
            {
                OwningLabelColumn.ImageList.Draw(graphics, rect.Location, imageIndex);
            }
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

        #endregion

        #region Properties

        private DataGridViewCustomTextColumn OwningLabelColumn
        {
            get
            {
                return (DataGridViewCustomTextColumn)OwningColumn;
            }
        }

        #endregion
    }
}
