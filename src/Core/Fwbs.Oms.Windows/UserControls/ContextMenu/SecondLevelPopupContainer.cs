using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.ContextMenu
{
    public class SecondLevelPopupContainer : ContextMenuPopupContainer
    {
        public SecondLevelPopupContainer(Control control) : base(control)
        {

        }

        protected override Point GetLocation(Control control, Rectangle area)
        {
            Point location = control.PointToScreen(new Point(area.Left + area.Width, area.Top + area.Height));
            Rectangle screen = System.Windows.Forms.Screen.FromControl(control).WorkingArea;
            if (location.X + Size.Width > screen.Right)
            {
                location.X -= control.Width;
            }
            else
            {
                location.X += Size.Width;
            }

            if (location.Y - control.Height + Size.Height > screen.Bottom)
            {
                location.Y = screen.Bottom - Size.Height;
            }
            else
            {
                location.Y -= control.Height;
            }
            
            return control.PointToClient(location);
        }

        protected override void ShowPopup(Control control, Point location)
        {
            Show(control, location, ToolStripDropDownDirection.BelowLeft);
        }
    }
}
