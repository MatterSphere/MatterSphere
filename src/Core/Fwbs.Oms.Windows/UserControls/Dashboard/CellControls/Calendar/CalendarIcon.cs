using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    public class CalendarIcon : PictureBox
    {
        public CalendarIcon()
        {
            SetIcon();
        }

        public int Day { get; set; }

        private void SetIcon()
        {
            this.Image = FWBS.OMS.UI.Windows.Images.GetCommonIcon(DeviceDpi, "calendar");
        }

        #region Overrides

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);

            SetIcon();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (Font font = new Font("Segoe UI", 11.25F))
            {
                var rectangle = new Rectangle(
                    this.ClientRectangle.X,
                    this.ClientRectangle.Y+LogicalToDeviceUnits(6),
                    this.ClientRectangle.Width,
                    this.ClientRectangle.Height - LogicalToDeviceUnits(6));
                TextRenderer.DrawText(e.Graphics, Day.ToString(), font, rectangle, Color.FromArgb(51, 51, 51));
            }
        }

        #endregion
    }
}
