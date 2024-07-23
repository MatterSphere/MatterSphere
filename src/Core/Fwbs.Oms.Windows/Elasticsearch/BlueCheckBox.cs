using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Elasticsearch
{
    public class BlueCheckBox : CheckBox
    {
        public BlueCheckBox()
        {
            BackColor = Color.White;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            var boxSide = LogicalToDeviceUnits(12);
            var y = Convert.ToInt32((this.Height - boxSide) / 2F);

            pevent.Graphics.FillRectangle(new SolidBrush(this.BackColor), 0, 0, Width, Height);
            if (Checked)
            {
                using (var blueBrush = new SolidBrush(Color.FromArgb(21, 101, 192)))
                using (var checkMarkFont = new Font("Segoe UI Symbol", 6, FontStyle.Regular))
                {
                    pevent.Graphics.FillRectangle(blueBrush, 0, y, boxSide, boxSide);
                    pevent.Graphics.DrawString("", checkMarkFont, Brushes.White,
                        new Rectangle(0, y, boxSide, boxSide));
                }
            }
            else
            {
                using (var pen = new Pen(Color.FromArgb(51, 51, 51), 1))
                {
                    pevent.Graphics.DrawRectangle(pen, 0, y, boxSide - 1, boxSide - 1);
                }
            }

            using (var textBrush = new SolidBrush(Color.FromArgb(51, 51, 51)))
            {
                using (var stringFormat = new StringFormat(StringFormatFlags.NoWrap) { LineAlignment = StringAlignment.Center })
                {
                    pevent.Graphics.DrawString(
                        Text,
                        Font,
                        textBrush,
                        new Rectangle(
                            boxSide + LogicalToDeviceUnits(10),
                            0,
                            this.Width - boxSide - LogicalToDeviceUnits(10),
                            this.Height - LogicalToDeviceUnits(1)),
                        stringFormat);
                }
            }
        }
    }
}
