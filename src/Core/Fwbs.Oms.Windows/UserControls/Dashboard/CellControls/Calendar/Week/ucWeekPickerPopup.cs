using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    internal partial class ucWeekPickerPopup : UserControl
    {
        public ucWeekPickerPopup()
        {
            InitializeComponent();
        }

        public EventHandler<DateTime> WeekClicked;

        public void SetWeeks(List<DateHelper.WeekInfo> weeks)
        {
            this.flpContainer.Controls.Clear();
            var maxWidth = 0;
            var items = new List<ucWeekPickerPopupItem>();
            foreach (var week in weeks)
            {
                var item = new ucWeekPickerPopupItem();
                item.SetWeek(week);
                item.ItemClicked += OnItemClick;
                this.flpContainer.Controls.Add(item);
                items.Add(item);

                var label = item.Title;
                var size = TextRenderer.MeasureText(label, item.Font);
                maxWidth = Math.Max(maxWidth, size.Width);
            }

            var height = this.flpContainer.Controls.Sum<Control>(cntrl => cntrl.Height);
            this.MinimumSize = new Size(this.flpContainer.Size.Width, height);

            maxWidth = 96 * maxWidth / DeviceDpi;
            var padding = (this.Width - maxWidth) / 2;
            foreach (var item in items)
            {
                item.Padding = new Padding(padding, 0, 0, 0);
            }
        }

        public void ChangeDpi(float dpi)
        {
            this.Scale(new System.Drawing.SizeF(dpi / 96, dpi / 96));
        }

        private void OnItemClick(object sender, DateTime e)
        {
            WeekClicked?.Invoke(this, e);
        }
    }
}
