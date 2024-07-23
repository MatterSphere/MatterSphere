using System;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    internal class ucWeekPicker : ucCalendarPickerBase
    {
        private DateHelper _dateHelper;

        public ucWeekPicker() : base()
        {
            _dateHelper = new DateHelper();
            SetForeColor(Color.FromArgb(0, 120, 212));
        }

        public override void SetTitle(DateTime date)
        {
            var weekNumberOfMonth = _dateHelper.GetWeekNumberOfMonth(date);
            var weekDays = _dateHelper.GetWeekDays(date);
            var week = CodeLookup.GetLookup("DASHBOARD", "WKUPCS", "Week");
            Text = $"{week} {weekNumberOfMonth} ({weekDays[0].Day} - {weekDays[6].Day})";
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
