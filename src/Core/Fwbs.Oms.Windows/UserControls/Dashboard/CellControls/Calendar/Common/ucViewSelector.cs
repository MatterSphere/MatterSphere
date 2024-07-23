using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common
{
    internal class ucViewSelector : System.Windows.Forms.RadioButton
    {
        private const int _borderSize = 20;
        private const int _markerSize = 10;
        private const int _padding = 16;
        private readonly Color _baseColor = Color.FromArgb(51, 51, 51);
        private readonly Color _selectionColor = Color.FromArgb(0, 120, 212);

        public ucViewSelector()
        {
            BackColor = Color.White;
            this.Padding = new Padding(_padding, 0,0,0);
        }

        public int PreferredWidth
        {
            get
            {
                return LogicalToDeviceUnits(this.Size.Width + _borderSize);
            }
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            var borderSize = LogicalToDeviceUnits(_borderSize);
            var markerSize = LogicalToDeviceUnits(_markerSize);
            var borderY = Convert.ToInt32((this.Height - borderSize) / 2F);
            var markerX = Convert.ToInt32((borderSize - markerSize) / 2F);
            var markerY = Convert.ToInt32((this.Height - markerSize) / 2F);
            var borderThickness = LogicalToDeviceUnits(1);
            pevent.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            using (var brush = new SolidBrush(this.BackColor))
            {
                pevent.Graphics.FillRectangle(brush, 0, 0, LogicalToDeviceUnits(2 * _padding), Height);
            }

            if (Checked)
            {
                using (var pen = new Pen(_selectionColor, borderThickness))
                {
                    pevent.Graphics.DrawEllipse(pen, borderThickness, borderY, borderSize, borderSize);
                }

                using (var blueBrush = new SolidBrush(_selectionColor))
                {
                    pevent.Graphics.FillEllipse(blueBrush, markerX + borderThickness, markerY, markerSize, markerSize);
                }
            }
            else
            {
                using (var pen = new Pen(_baseColor, borderThickness))
                {
                    pevent.Graphics.DrawEllipse(pen, borderThickness, borderY, borderSize, borderSize);
                }
            }
        }
    }
}
