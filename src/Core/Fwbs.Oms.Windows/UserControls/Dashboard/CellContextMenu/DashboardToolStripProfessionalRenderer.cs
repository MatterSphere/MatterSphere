using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    internal class DashboardToolStripProfessionalRenderer : ToolStripProfessionalRenderer
    {

        internal DashboardToolStripProfessionalRenderer() : base(new DashboardColorTable())
        {
            
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            int enlargeX = ((DashboardToolStripMenuItem)e.Item).UcEmptyCell.LogicalToDeviceUnits(-2);
            int enlargeY = ((DashboardToolStripMenuItem)e.Item).UcEmptyCell.LogicalToDeviceUnits(-4);
            
            var r = new Rectangle(e.ArrowRectangle.Location, ((DashboardToolStripMenuItem)e.Item).UcEmptyCell.LogicalToDeviceUnits(new Size(10, 20)));
            r.Inflate(enlargeX, enlargeY);
            
            using (var pen = new Pen(e.Item.ForeColor))
            {
                e.Graphics.DrawLines(pen, new Point[]{
                    new Point(r.Left, r.Top),
                    new Point(r.Right, r.Top + r.Height / 2),
                    new Point(r.Left, r.Top+ r.Height)});
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            int offset = ((DashboardToolStripMenuItem) e.Item).UcEmptyCell.LogicalToDeviceUnits(12);
            e.TextRectangle = new Rectangle(e.Item.ContentRectangle.X + offset, e.Item.ContentRectangle.Y, e.Item.ContentRectangle.Width - offset, e.Item.ContentRectangle.Height);
            
            base.OnRenderItemText(e);
        }

        private class DashboardColorTable : ProfessionalColorTable
        {

            public override Color MenuItemBorder
            {
                get { return Color.FromArgb(237, 243, 250); }
            }

            public override Color MenuItemSelected
            {
                get { return Color.FromArgb(237, 243, 250); }
            }

            public override Color ToolStripDropDownBackground
            {
                get { return Color.White; }
            }

            public override Color ImageMarginGradientBegin
            {
                get { return Color.White; }
            }

            public override Color ImageMarginGradientMiddle
            {
                get { return Color.White; }
            }

            public override Color ImageMarginGradientEnd
            {
                get { return Color.White; }
            }

            public override Color MenuBorder
            {
                get { return Color.White; }
            }

        }

    }
}
