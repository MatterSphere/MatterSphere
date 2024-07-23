using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public class DataGridViewColumnHeaderImageCell : System.Windows.Forms.DataGridViewColumnHeaderCell
    {

        private Rectangle _imageBounds;

        private Color _imageBackColor;

        public event EventHandler ImageClick; 

        public Image Image { get; set; }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex, DataGridViewElementStates dataGridViewElementState,
    object value, object formattedValue, string errorText, DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
    DataGridViewPaintParts paintParts)
        {
            base.Paint(graphics, clipBounds, cellBounds, rowIndex, dataGridViewElementState, value, formattedValue,
                errorText, cellStyle, advancedBorderStyle, paintParts);
            
            if (Image != null)
            {
                Rectangle bounds = GetImageBounds(cellBounds, cellStyle.Alignment, cellStyle.Padding);
                using (SolidBrush imageBackground = new SolidBrush(_imageBackColor))
                {
                    graphics.FillRectangle(imageBackground, bounds);
                }

                graphics.DrawImage(Image, bounds);
                _imageBounds = new Rectangle(bounds.X - cellBounds.X,  bounds.Y - cellBounds.Y, bounds.Width, bounds.Height);
            }
        }

        protected override void OnMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (e.Button == MouseButtons.Left && _imageBounds.Contains(e.Location.X, e.Location.Y))
            {
                OnImageClick();
            }
        }

        protected virtual void OnImageClick()
        {
            ImageClick?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnMouseMove(DataGridViewCellMouseEventArgs e)
        {
            base.OnMouseMove(e);
            ImageBackColor = _imageBounds.Contains(e.Location.X, e.Location.Y) ? Color.FromArgb(208, 224, 242) : OwningColumn.DefaultCellStyle.BackColor;
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);
            ImageBackColor = OwningColumn.DefaultCellStyle.BackColor;
        }

        private Rectangle GetImageBounds(Rectangle cellBounds, DataGridViewContentAlignment alignment, Padding cellPadding)
        {
            Size imageSize = Image.Size;
            cellBounds = Rectangle.FromLTRB(cellBounds.Left + cellPadding.Left, cellBounds.Top + cellPadding.Top,
                                            cellBounds.Right - cellPadding.Right, cellBounds.Bottom - cellPadding.Bottom);
            switch (alignment)
            {
                case DataGridViewContentAlignment.TopLeft:
                    return new Rectangle(new Point(cellBounds.X, cellBounds.Y), imageSize);
                case DataGridViewContentAlignment.TopCenter:
                    return new Rectangle(new Point(cellBounds.X + (cellBounds.Width - imageSize.Width) / 2, cellBounds.Y), imageSize);
                case DataGridViewContentAlignment.TopRight:
                    return new Rectangle(new Point(cellBounds.Right - imageSize.Width, cellBounds.Y), imageSize);
                case DataGridViewContentAlignment.MiddleLeft:
                    return new Rectangle(new Point(cellBounds.X, cellBounds.Y + (cellBounds.Height - imageSize.Height) / 2), imageSize);
                case DataGridViewContentAlignment.MiddleCenter:
                    return new Rectangle(new Point(cellBounds.X + (cellBounds.Width - imageSize.Width) / 2, cellBounds.Y + (cellBounds.Height - imageSize.Height) / 2), imageSize);
                case DataGridViewContentAlignment.MiddleRight:
                    return new Rectangle(new Point(cellBounds.Right - imageSize.Width, cellBounds.Y + (cellBounds.Height - imageSize.Height) / 2), imageSize);
                case DataGridViewContentAlignment.BottomLeft:
                    return new Rectangle(new Point(cellBounds.X, cellBounds.Bottom - imageSize.Height), imageSize);
                case DataGridViewContentAlignment.BottomCenter:
                    return new Rectangle(new Point(cellBounds.X + (cellBounds.Width - imageSize.Width) / 2, cellBounds.Bottom - imageSize.Height), imageSize);
                case DataGridViewContentAlignment.BottomRight:
                    return new Rectangle(new Point(cellBounds.Right - imageSize.Width, cellBounds.Bottom - imageSize.Height), imageSize);
                default:
                    return new Rectangle(new Point(cellBounds.X, cellBounds.Y), imageSize);
            }
        }

        private Color ImageBackColor
        {
            set
            {
                if (!value.Equals(_imageBackColor))
                {
                    _imageBackColor = value;
                    DataGridView.InvalidateCell(this);
                }
            }
        }

    }
}
