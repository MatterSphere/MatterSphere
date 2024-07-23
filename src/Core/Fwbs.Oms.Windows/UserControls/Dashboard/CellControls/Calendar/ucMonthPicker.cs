using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    internal class ucMonthPicker : ucCalendarPickerBase
    {
        private Size _size;

        public ucMonthPicker() : base()
        {
            SetForeColor(Color.FromArgb(51, 51, 51));
            SetSize();
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return _size;
        }

        public override void SetTitle(DateTime date)
        {
            Text = $"{date.ToString("MMMM")} {date.Year}";
        }

        private void SetSize()
        {
            var maxWidth = 0;
            var date = new DateTime(2020, 1, 1);
            for (int i = 0; i < 12; i++)
            {
                var label = $"{date.AddMonths(i).ToString("MMMM")} {date.Year}";
                var size = TextRenderer.MeasureText(label, this.Font);
                maxWidth = Math.Max(maxWidth, size.Width);
            }

            var width = maxWidth + this.chevron.Width + LogicalToDeviceUnits(5);
            _size = new Size(width, this.Height);
        }
    }
}
