using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    internal class ucMonthPickerPopup : ucCalendarPickerPopup
    {
        public ucMonthPickerPopup()
        {
            _currentDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            _firstItem = _currentDate.AddMonths(-2);

            btnFirstItem.ItemView = ucCalendarPickerPopupItem.View.Month;
            btnSecondItem.ItemView = ucCalendarPickerPopupItem.View.Month;
            btnThirdItem.ItemView = ucCalendarPickerPopupItem.View.Month;
            btnFourthItem.ItemView = ucCalendarPickerPopupItem.View.Month;
            btnFifthItem.ItemView = ucCalendarPickerPopupItem.View.Month;

            SetDates();
            SetPaddings();
        }

        protected override void PrevItem()
        {
            _firstItem = _firstItem.AddMonths(-1);
            SetDates();
        }

        protected override void NextItem()
        {
            _firstItem = _firstItem.AddMonths(1);
            SetDates();
        }

        private void SetDates()
        {
            SetDate(btnFirstItem, _firstItem);
            SetDate(btnSecondItem, _firstItem.AddMonths(1));
            SetDate(btnThirdItem, _firstItem.AddMonths(2));
            SetDate(btnFourthItem, _firstItem.AddMonths(3));
            SetDate(btnFifthItem, _firstItem.AddMonths(4));
        }

        protected override int GetLabelWidth(ucCalendarPickerPopupItem item)
        {
            var maxWidth = 0;
            for (int i = 1; i < 13; i++)
            {
                var month = new DateTime(2020, i, 1);
                var label = item.GetMonthLabel(month);
                var size = TextRenderer.MeasureText(label, item.Font);
                maxWidth = Math.Max(maxWidth, size.Width);
            }

            return maxWidth;
        }
    }
}
