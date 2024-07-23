using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    internal partial class ucCalendarMultiItem : UserControl
    {
        public ucCalendarMultiItem()
        {
            InitializeComponent();

            lblMeetings.Text = CodeLookup.GetLookup("DASHBOARD", "MTNGS", "Meetings");
        }

        public ucCalendarMultiItem(int count) : this()
        {
            lblNumber.Text = count.ToString();
        }
    }
}
