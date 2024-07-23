using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public interface IeXPFrame
    {
        int Width { get; set; }
        int Height { get; set; }
        string Text { get; set; }
        ExtColor FontColor { get; set; }
        ExtColor FrameForeColor { get; set; }
        System.Drawing.Font Font { get; set; }
        System.Windows.Forms.RightToLeft RightToLeft { get; set; }
        int LogicalToDeviceUnits(int value);
        void PainteXPFrame(System.Windows.Forms.PaintEventArgs e);
    }

    public enum eXPFrameType
    {
        eXPFrame,
        eXPFrame2
    }

    class eXPFramePainter
    {
        private eXPFrameType frametype;
        private IeXPFrame frame;
        private ExtColor backcolor;
        private string text;
        private Color linecolor;
        private Color headerbackcolor;
        private ExtColor themecolor;
        private System.Drawing.Color basebackcolor;

        public eXPFramePainter(IeXPFrame Frame, string Text, ExtColor BackColor, ExtColor ThemeColor, System.Drawing.Color BaseBackColor, eXPFrameType FrameType)
        {
            text = Text;
            frame = Frame;
            backcolor = BackColor;
            frametype = FrameType;
            themecolor = ThemeColor;
            basebackcolor = BaseBackColor;
        }

        public eXPFramePainter(IeXPFrame Frame, string Text, ExtColor BackColor, ExtColor ThemeColor, System.Drawing.Color BaseBackColor, eXPFrameType FrameType, Color LineColor, Color HeaderBackColour):this(Frame, Text, BackColor, ThemeColor, BaseBackColor, FrameType)
        {
            linecolor = LineColor;
            headerbackcolor = HeaderBackColour;
        }

        public void PainteXPFrame(System.Windows.Forms.PaintEventArgs e)
        {
            if (String.IsNullOrEmpty(frame.Text) == false)
            {
                using (SolidBrush b1 = new SolidBrush(frame.FontColor.Color))
                {
                    using (Pen p1 = new Pen(frame.FrameForeColor.Color, 1))
                    {
                        using (Font f = new Font(frame.Font, FontStyle.Bold))
                        {
                            RectangleF textsize = new RectangleF(new PointF(0, 0), e.Graphics.MeasureString(frame.Text, frame.Font));
                            var Y = Convert.ToInt32(textsize.Height / 2);
                            DetermineFrameType(f, p1, b1, Y, textsize, e);
                        }
                    }
                }
            }
        }

        private void DetermineFrameType(Font f, Pen p1, SolidBrush b1, int Y, RectangleF textsize, System.Windows.Forms.PaintEventArgs e)
        {
            if (frametype == eXPFrameType.eXPFrame)
            {
                if (frame.RightToLeft == RightToLeft.Yes)
                    DrawLegacyFrameRTL(f, p1, b1, Y, textsize, e);
                else
                    DrawLegacyFrameLTR(f, p1, b1, Y, textsize, e);
            }
            else
            {
                if (frame.RightToLeft == RightToLeft.Yes)
                    DrawFrameVersion7RTL(f, b1, e);
                else
                    DrawFrameVersion7LTR(f, b1, e);
            }
        }

        private StringFormat SetupStringFormat(System.Windows.Forms.PaintEventArgs e)
        {
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.Trimming = StringTrimming.EllipsisWord;
            sf.FormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.DisplayFormatControl;
            return sf;
        }

        private void DrawFrameVersion7LTR(Font f, SolidBrush b1, System.Windows.Forms.PaintEventArgs e)
        {
            using (SolidBrush sb1 = new SolidBrush(headerbackcolor))
            {
                using (Pen pen2 = new Pen(linecolor, 1))
                {
                    using (Pen pen1 = new Pen(linecolor, 1))
                    {
                        var height = Convert.ToInt32(e.Graphics.MeasureString(frame.Text, f).Height + frame.LogicalToDeviceUnits(6));
                        e.Graphics.DrawRectangle(pen1, new Rectangle(0, 0, frame.Width - 1, frame.Height - 1));
                        e.Graphics.FillRectangle(sb1, new Rectangle(1, 1, frame.Width - 2, height));
                        e.Graphics.DrawRectangle(pen2, new Rectangle(0, 0, frame.Width - 1, height));
                        e.Graphics.DrawString(frame.Text, f, b1, new Point(frame.LogicalToDeviceUnits(2), frame.LogicalToDeviceUnits(3)));
                    }
                }
            }
        }

        private void DrawFrameVersion7RTL(Font f, SolidBrush b1, System.Windows.Forms.PaintEventArgs e)
        {
            using (SolidBrush sb1 = new SolidBrush(headerbackcolor))
            {
                using (Pen pen2 = new Pen(linecolor, 1))
                {
                    using (Pen pen1 = new Pen(linecolor, 1))
                    {
                        var height = Convert.ToInt32(e.Graphics.MeasureString(frame.Text, f).Height + frame.LogicalToDeviceUnits(6));
                        e.Graphics.DrawRectangle(pen1, new Rectangle(0, 0, frame.Width - 1, frame.Height - 1));
                        e.Graphics.FillRectangle(sb1, new Rectangle(1, 1, frame.Width - 2, height));
                        e.Graphics.DrawRectangle(pen2, new Rectangle(0, 0, frame.Width - 1, height));
                        e.Graphics.DrawString(frame.Text, f, b1, new Point(frame.Width - frame.LogicalToDeviceUnits(2), frame.LogicalToDeviceUnits(3)), new StringFormat(StringFormatFlags.DirectionRightToLeft));
                    }
                }
            }
        }

        private void DrawLegacyFrameLTR(Font f, Pen p1, SolidBrush b1, int Y, RectangleF textsize, System.Windows.Forms.PaintEventArgs e)
        {
            var width = frame.Width - (Convert.ToInt32(textsize.Width)) - frame.LogicalToDeviceUnits(15);
            e.Graphics.DrawString(frame.Text, f, b1, new Point(0, 0));
            if (themecolor.CurrentTheme == ExtColorTheme.Classic)
                ControlPaint.DrawBorder3D(e.Graphics, Convert.ToInt32(textsize.Right), Y, width, Y, Border3DStyle.Etched, Border3DSide.Top);
            else
                e.Graphics.DrawLine(p1, new Point(Convert.ToInt32(textsize.Right) + frame.LogicalToDeviceUnits(15), Y), new Point(frame.Width, Y));
        }

        private void DrawLegacyFrameRTL(Font f, Pen p1, SolidBrush b1, int Y, RectangleF textsize, System.Windows.Forms.PaintEventArgs e)
        {
            var width = frame.Width - (Convert.ToInt32(textsize.Width)) - frame.LogicalToDeviceUnits(15);
            e.Graphics.DrawString(frame.Text, f, b1, new Point(frame.Width, 0), new StringFormat(StringFormatFlags.DirectionRightToLeft));
            if (themecolor.CurrentTheme == ExtColorTheme.Classic)
                ControlPaint.DrawBorder3D(e.Graphics, 0, Y, width, Y, Border3DStyle.Etched, Border3DSide.Top);
            else
                e.Graphics.DrawLine(p1, new Point(0, Y), new Point(width, Y));
        }
        
        private void DrawRadioBox(Graphics e, Rectangle f2)
        {
            if (themecolor.CurrentTheme == ExtColorTheme.Classic || themecolor.CurrentTheme == ExtColorTheme.None)
                DrawSquareRadioBox(e, f2);
            else
                DrawRoundRadioBox(e, f2);
        }

        private void DrawSquareRadioBox(Graphics e, Rectangle f2)
        {
            using (SolidBrush backc = new SolidBrush(basebackcolor))
            {
                using (SolidBrush fillc = new SolidBrush(backcolor.Color))
                {
                    if (basebackcolor != backcolor.Color)
                        e.FillRectangle(fillc, f2);
                    ControlPaint.DrawBorder3D(e, f2, Border3DStyle.Etched);
                }
            }
        }

        private void DrawRoundRadioBox(Graphics e, Rectangle f2)
        {
            using (Pen frac = new Pen(themecolor.Color, 1))
            {
                using (SolidBrush backc = new SolidBrush(basebackcolor))
                {
                    using (SolidBrush fillc = new SolidBrush(backcolor.Color))
                    {
                        DrawRoundedRectangle(frac, backc, fillc, e, f2);
                    }
                }
            }
        }

        private void DrawRoundedRectangle(Pen frac, SolidBrush backc, SolidBrush fillc, Graphics e, Rectangle f2)
        {
            if (basebackcolor != backcolor.Color)
            {
                Rectangle f3 = Rectangle.Inflate(f2, -3, 0);
                Rectangle f4 = Rectangle.Inflate(f2, 0, -3);
                e.FillPie(fillc, new Rectangle(f2.X, f2.Y, 10, 10), 180, 90); // TL
                e.FillPie(fillc, new Rectangle(f2.Right - 10, f2.Y, 10, 10), 270, 90); // TR
                e.FillPie(fillc, new Rectangle(f2.X, (f2.Bottom - 10), 10, 10), 90, 90); // BL
                e.FillPie(fillc, new Rectangle(f2.Right - 10, (f2.Bottom - 10), 10, 10), 0, 90); // BR
                e.FillRectangle(fillc, f3);
                e.FillRectangle(fillc, f4);
            }

            e.DrawArc(frac, new Rectangle(f2.X, f2.Y, 10, 10), 180, 90); // TL
            e.DrawLine(frac, new Point(f2.X, f2.Y + 5), new Point(f2.X, f2.Bottom - 5));
            e.DrawArc(frac, new Rectangle(f2.Right - 10, f2.Y, 10, 10), 270, 90); // TR
            e.DrawLine(frac, new Point(f2.X + 5, f2.Y), new Point(f2.Right - 5, f2.Y));
            e.DrawLine(frac, new Point(f2.Right, f2.Y + 6), new Point(f2.Right, f2.Bottom - 5));
            e.DrawArc(frac, new Rectangle(f2.X, (f2.Bottom - 10), 10, 10), 90, 90); // BL
            e.DrawArc(frac, new Rectangle(f2.Right - 10, (f2.Bottom - 10), 10, 10), 0, 90); // BR
            e.DrawLine(frac, new Point(f2.X + 5, f2.Bottom), new Point(f2.Right - 5, f2.Bottom));
        }
    }
}
