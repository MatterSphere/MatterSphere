using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Day
{
    internal class ucDayPickerPopup : ucCalendarPickerPopup
    {
        public ucDayPickerPopup(DateTime date)
        {
            _currentDate = DateTime.Today;
            _firstItem = date == _currentDate
                ? GetFirstItem()
                : date;

            btnFirstItem.ItemView = ucCalendarPickerPopupItem.View.Day;
            btnSecondItem.ItemView = ucCalendarPickerPopupItem.View.Day;
            btnThirdItem.ItemView = ucCalendarPickerPopupItem.View.Day;
            btnFourthItem.ItemView = ucCalendarPickerPopupItem.View.Day;
            btnFifthItem.ItemView = ucCalendarPickerPopupItem.View.Day;

            SetDates();
            SetPaddings();
        }

        public ucDayPickerPopup() : this(DateTime.Today)
        {
            
        }

        protected override void PrevItem()
        {
            var day = _firstItem.AddDays(-1);
            if (day.Month != _firstItem.Month)
            {
                return;
            }

            _firstItem = day;
            SetDates();
        }

        protected override void NextItem()
        {
            if (_firstItem.AddDays(5).Month != _firstItem.Month)
            {
                return;
            }

            _firstItem = _firstItem.AddDays(1);
            SetDates();
        }

        private void SetDates()
        {
            SetDate(btnFirstItem, _firstItem);
            SetDate(btnSecondItem, _firstItem.AddDays(1));
            SetDate(btnThirdItem, _firstItem.AddDays(2));
            SetDate(btnFourthItem, _firstItem.AddDays(3));
            SetDate(btnFifthItem, _firstItem.AddDays(4));
        }

        private DateTime GetFirstItem()
        {
            var center = _currentDate;
            for (int i = 1; i <= 2; i++)
            {
                if (center.AddDays(-i).Month != center.Month)
                {
                    return center.AddDays(-(i-1));
                }
            }

            for (int i = 1; i <= 2; i++)
            {
                if (center.AddDays(i).Month != center.Month)
                {
                    return center.AddDays(-(5 - i));
                }
            }

            return center.AddDays(-2);
        }

        protected override int GetLabelWidth(ucCalendarPickerPopupItem item)
        {
            var maxWidth = 0;
            var date = DateTime.Today;
            for (int i = 0; i < 7; i++)
            {
                var label = btnFirstItem.GetDayLabel(date.AddDays(i));
                var size = TextRenderer.MeasureText(label, btnFirstItem.Font);
                maxWidth = Math.Max(maxWidth, size.Width);
            }

            return maxWidth;
        }
    }
}
