using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Common
{
    internal partial class ucCalendarDayHeader : UserControl
    {
        public ucCalendarDayHeader()
        {
            InitializeComponent();
        }

        public void SetTitle(DayOfWeek dayOfWeek)
        {
            if (Session.CurrentSession.IsConnected)
            {
                var info = Session.CurrentSession.DefaultCultureInfo.DateTimeFormat;
                Title.Text = info.GetDayName(dayOfWeek).Substring(0,1);
            }
        }
    }
}
