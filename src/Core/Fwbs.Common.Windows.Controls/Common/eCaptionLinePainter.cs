using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{

    public interface IeCaptionLine
    {
        string Text { get; set; }
        ExtColor FontColor { get; set; }
        ExtColor FrameForeColor { get; set; }
        Font Font { get; set; }
        bool FontStyleBold { get; set; }

        void PaintCaptionLine(System.Windows.Forms.PaintEventArgs e);
        void DrawCaptionLine(DrawingParameters pars);
    }


    internal class eCaptionLinePainter
    {
        private IeCaptionLine captionline;

        public eCaptionLinePainter(IeCaptionLine CaptionLine)
        {
            captionline = CaptionLine;
        }

        public void PaintCaptionLine(System.Windows.Forms.PaintEventArgs e)
        {
            DrawingParameters pars = new DrawingParameters();
            pars.b1 = new SolidBrush(captionline.FontColor.Color);
            pars.p1 = new Pen(captionline.FrameForeColor.Color,1);
            pars.f = new Font(captionline.Font, captionline.FontStyleBold ? FontStyle.Bold : FontStyle.Regular);
            pars.textsize = new Rectangle(Point.Empty, Size.Ceiling(e.Graphics.MeasureString(captionline.Text, pars.f)));
            pars.args = e;
            captionline.DrawCaptionLine(pars);
            pars.Dispose();
        }
    }


    public struct DrawingParameters : System.IDisposable
    {
        public SolidBrush b1 { get; set; }
        public SolidBrush b2 { get; set; }
        public Pen p1 { get; set; }
        public Font f { get; set; }
        public Rectangle textsize { get; set; }
        public PaintEventArgs args { get; set; }

        public void Dispose()
        {
            b1.Dispose();
            b2?.Dispose();
            p1.Dispose();
            f.Dispose();
        }
    }

}
