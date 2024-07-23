using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day
{
    internal class ucDayPicker : ucCalendarPickerBase
    {
        private DateTimeFormatInfo _info;

        public ucDayPicker() : base()
        {
            _info = Session.CurrentSession.DefaultCultureInfo.DateTimeFormat;
            SetForeColor(Color.FromArgb(0, 120, 212));
        }

        public override void SetTitle(DateTime date)
        {
            Text = $"{_info.GetDayName(date.DayOfWeek)} {date.Day}";
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var size = TextRenderer.MeasureText(this.Text, this.Font);
            var newWidth = size.Width + LogicalToDeviceUnits(4) + chevron.Width;
            var newHeight = this.Height;
            return new Size(newWidth, newHeight);
        }
    }
}
