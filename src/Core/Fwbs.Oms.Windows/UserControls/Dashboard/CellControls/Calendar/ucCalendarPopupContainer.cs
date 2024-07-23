using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.ContextMenu;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    internal class ucCalendarPopupContainer : ContextMenuPopupContainer
    {
        public ucCalendarPopupContainer() { }

        public ucCalendarPopupContainer(Control control) : base(control)
        {
            this.MinimumSize = control.Size;
        }

        protected override Point GetLocation(Control control, Rectangle area)
        {
            Point location = control.PointToScreen(new Point(area.Left + area.Width, area.Top + area.Height));
            Rectangle screen = System.Windows.Forms.Screen.FromControl(control).WorkingArea;
            if (location.X - Size.Width < screen.Left)
            {
                location.X = screen.Left + Size.Width;
            }

            if (location.Y + Size.Height > (screen.Top + screen.Height))
            {
                location.Y -= Size.Height + area.Height;
            }

            return control.PointToClient(location);
        }

        protected override void ShowPopup(Control control, Point location)
        {
            Show(control, location, ToolStripDropDownDirection.BelowLeft);
        }
    }
}
