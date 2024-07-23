using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public partial class ImageListBox : ListBox
    {
        private string _imageColumnName = "";
        private ImageList _imgList;
        private int ImageOffsetVariable => this.LogicalToDeviceUnits(18);
        private int ImageSizeVariable => this.LogicalToDeviceUnits(16);
        private int ItemMinimalHeight => this.LogicalToDeviceUnits(18);

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (this.DataSource is DataTable || this.DataSource is DataView)
            {
                if (this.Items.Count == 0)
                    return;

                DataRowView row = this.Items[e.Index] as DataRowView;
                if (row == null)
                    return;

                // Draw the background of the ListBox control for each item.
                e.DrawBackground();

                int p = -1;
                try
                {
                    p = Convert.ToInt32(row[_imageColumnName]);
                    if (p >= _imgList.Images.Count) p = -1;
                    if (p < -1) p = -1;
                }
                catch (Exception)
                {
                    p = -1;
                }
                // Draw the current item text based on the current Font and the custom brush settings.
                if (p != -1)
                {
                    var point = new PointF
                    {
                        X = e.Bounds.Location.X + 1,
                        Y = e.Bounds.Y + Convert.ToInt32((e.Bounds.Height - ImageSizeVariable) / 2)
                    };
                    e.Graphics.DrawImage(_imgList.Images[p], new RectangleF(point, new SizeF(ImageSizeVariable, ImageSizeVariable)));
                }

                var myBounds = e.Bounds;
                myBounds.Offset(ImageOffsetVariable, 0);
                var brush = (e.State & DrawItemState.Selected) == DrawItemState.Selected
                    ? SystemBrushes.HighlightText
                    : SystemBrushes.ControlText;

                using (var stringFormat = new StringFormat(StringFormat.GenericDefault){ LineAlignment = StringAlignment.Center })
                {
                    e.Graphics.DrawString(Convert.ToString(row[this.DisplayMember]), e.Font, brush, myBounds, stringFormat);
                }

                // If the ListBox has focus, draw a focus rectangle around the selected item.
                e.DrawFocusRectangle();
            }
            else
                this.DrawMode = DrawMode.Normal;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            ValidateItemHeight();
        }

        [Category("Design")]
        public string ImageColumnName
        {
            get
            {
                return _imageColumnName;
            }
            set
            {
                _imageColumnName = value;
            }
        }

        /// <summary>
        /// Gets or Sets the bound image list associated with the column.
        /// </summary>
        [Category("Design")]
        [DefaultValue(null)]
        public ImageList ImageList
        {
            get
            {
                return _imgList;
            }
            set
            {
                _imgList = value;
                this.DrawMode = value != null ? DrawMode.OwnerDrawFixed : DrawMode.Normal;
                ValidateItemHeight();
            }
        }

        private void ValidateItemHeight()
        {
            if (this.DrawMode == DrawMode.OwnerDrawFixed)
            {
                using (var g = this.CreateGraphics())
                {
                    var measuredItemHeight = Convert.ToInt32(g.MeasureString("T", this.Font).Height);
                    this.ItemHeight = measuredItemHeight < ItemMinimalHeight ? ItemMinimalHeight : measuredItemHeight;
                }
            }
        }
    }
}
