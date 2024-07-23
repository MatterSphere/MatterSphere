using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Breadcrumbs
{
    internal class BreadCrumbsPanel : FlowLayoutPanel
    {
        public BreadCrumbsPanel()
        {
            DoubleBuffered = true;
            AutoScroll = false;
            HorizontalScroll.Maximum = 0;
            HorizontalScroll.Visible = false;
            VerticalScroll.Maximum = 0;
            VerticalScroll.Visible = false;
            AutoScroll = true;
        }

        public void EnsureLastVisible()
        {
            if (Controls.Count > 0)
            {
                Control activeControl = Controls[Controls.Count - 1];
                Point scrollLocation = ScrollToControl(activeControl);
                SetScrollState(ScrollStateUserHasScrolled, false);
                SetDisplayRectLocation(scrollLocation.X, scrollLocation.Y);
            }
        }
    }
}
