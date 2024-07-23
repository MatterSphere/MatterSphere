using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    internal partial class ucCalendarPickerPopupItem : UserControl
    {
        private DateTime _date;
        private readonly Font _defaultFont = new Font("Segoe UI", 9F);
        private readonly Font _selectedFont = new Font("Segoe UI Semibold", 9F);

        public ucCalendarPickerPopupItem()
        {
            InitializeComponent();
        }

        public EventHandler<DateTime> ItemClicked;

        public View ItemView { get; set; }
        public string Title
        {
            get
            {
                return lblTitle.Text;
            }
            set
            {
                lblTitle.Text = value;
            }
        }

        public void SetDate(DateTime date, bool currentDate = false)
        {
            _date = date;

            switch (ItemView)
            {
                case View.Day:
                    Title = GetDayLabel(date);
                    break;
                case View.Month:
                    Title = GetMonthLabel(date);
                    break;
            }
            
            lblTitle.Font = currentDate ? _selectedFont : _defaultFont;
        }

        public string GetMonthLabel(DateTime date)
        {
            return $"{date.ToString("MMMM")}  {date.Year}";
        }

        public string GetDayLabel(DateTime date)
        {
            var info = Session.CurrentSession.DefaultCultureInfo.DateTimeFormat;
            return $"{info.GetDayName(date.DayOfWeek)}  {date.Day}";
        }
        
        #region UI events

        private void PopupItem_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(237, 243, 250);
        }

        private void PopupItem_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void PopupItem_MouseClick(object sender, MouseEventArgs e)
        {
            ItemClicked?.Invoke(this, _date);
        }

        #endregion

        internal enum View
        {
            Day =1,
            Month = 2
        }
    }
}
