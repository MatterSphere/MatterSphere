using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls
{
    internal class ucToggleButton : Control
    {
        private const int UNIT_SIZE = 10;
        private readonly Color _baseColor = Color.FromArgb(51, 51, 51);
        private readonly Color _pickerColor = Color.FromArgb(94, 94, 97);
        private readonly Color _mainColor = Color.FromArgb(13, 118, 207);
        
        public ucToggleButton()
        {
            this.SetStyle(ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        public event EventHandler CheckedChanged;

        private int TotalWidth
        {
            get { return 4 * UNIT_SIZE * DeviceDpi / 96; }
        }

        private int Radius
        {
            get { return UNIT_SIZE * DeviceDpi / 96; }
        }

        private int SmallRadius
        {
            get { return UNIT_SIZE * DeviceDpi / 160; }
        }

        private int Diameter
        {
            get { return Radius * 2; }
        }

        private int SmallDiameter
        {
            get { return SmallRadius * 2; }
        }

        private bool _checked;
        public bool Checked
        {
            get { return _checked; }
            set
            {
                if (value != _checked)
                {
                    _checked = value;
                    CheckedChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private int GetLeftPadding(PaintEventArgs e)
        {
            return (Width - TotalWidth) / 2;
        }

        private int GetTopPadding(PaintEventArgs e)
        {
            return (Height - Diameter) / 2;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            using (var brush = new SolidBrush(this.BackColor))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }

            if (Checked)
            {
                DrawCheckedContainer(e);
                DrawCheckedPicker(e);
            }
            else
            {
                DrawUncheckedContainer(e);
                DrawUncheckedPicker(e);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            Checked = !Checked;
            this.Invalidate();
        }

        private void DrawCheckedContainer(PaintEventArgs e)
        {
            using (var brush = new SolidBrush(_mainColor))
            {
                e.Graphics.FillRectangle(brush,
                    GetLeftPadding(e) + Radius,
                    GetTopPadding(e),
                    TotalWidth - Diameter,
                    Diameter);

                e.Graphics.FillEllipse(brush,
                    GetLeftPadding(e),
                    (Height - Diameter) / 2,
                    Diameter,
                    Diameter);
                e.Graphics.FillEllipse(brush,
                    GetLeftPadding(e) + TotalWidth - Diameter,
                    (Height - Diameter) / 2,
                    Diameter,
                    Diameter);
            }
        }

        private void DrawCheckedPicker(PaintEventArgs e)
        {
            e.Graphics.FillEllipse(Brushes.White,
                GetLeftPadding(e) + TotalWidth - Diameter + (Diameter - SmallDiameter) / 2,
                GetTopPadding(e) + (Diameter - SmallDiameter) / 2,
                SmallDiameter,
                SmallDiameter);
        }

        private void DrawUncheckedContainer(PaintEventArgs e)
        {
            using (var pen = new Pen(_baseColor, LogicalToDeviceUnits(1)))
            {
                e.Graphics.DrawArc(pen,
                    GetLeftPadding(e),
                    GetTopPadding(e),
                    Diameter,
                    Diameter,
                    90,
                    180);

                e.Graphics.DrawArc(pen,
                    GetLeftPadding(e) + TotalWidth - Diameter,
                    GetTopPadding(e),
                    Diameter,
                    Diameter,
                    270,
                    180);

                e.Graphics.DrawLine(pen,
                    GetLeftPadding(e) + Radius,
                    GetTopPadding(e),
                    GetLeftPadding(e) + TotalWidth - Radius,
                    GetTopPadding(e));

                e.Graphics.DrawLine(pen,
                    GetLeftPadding(e) + Radius,
                    GetTopPadding(e) + Diameter,
                    GetLeftPadding(e) + TotalWidth - Radius,
                    GetTopPadding(e) + Diameter);
            }
        }

        private void DrawUncheckedPicker(PaintEventArgs e)
        {
            using (var brush = new SolidBrush(_pickerColor))
            {
                e.Graphics.FillEllipse(brush,
                    GetLeftPadding(e) + (Diameter - SmallDiameter) / 2,
                    GetTopPadding(e) + (Diameter - SmallDiameter) / 2,
                    SmallDiameter,
                    SmallDiameter);
            }
        }
    }
}
