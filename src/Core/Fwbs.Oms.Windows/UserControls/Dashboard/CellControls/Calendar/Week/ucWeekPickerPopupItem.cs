using System;
using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar.Week
{
    internal partial class ucWeekPickerPopupItem : UserControl
    {
        private DateHelper.WeekInfo _weekInfo;

        public ucWeekPickerPopupItem()
        {
            InitializeComponent();
        }

        public EventHandler<DateTime> ItemClicked;

        public string Title
        {
            get { return this.lblTitle.Text; }
        }

        public void SetWeek(DateHelper.WeekInfo weekInfo)
        {
            _weekInfo = weekInfo;
            this.lblTitle.Text = GetLabel(_weekInfo);
        }

        private string GetLabel(DateHelper.WeekInfo weekInfo)
        {
            var week = CodeLookup.GetLookup("DASHBOARD", "WKUPCS", "Week");
            return $"{week} {weekInfo.WeekNumber} ({weekInfo.Interval})";
        }

        #region UI events

        private void lblTitle_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.White;
        }

        private void lblTitle_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(237, 243, 250);
        }

        private void lblTitle_MouseClick(object sender, MouseEventArgs e)
        {
            ItemClicked.Invoke(this, _weekInfo.Start < _weekInfo.FirstMonthDay ? _weekInfo.FirstMonthDay : _weekInfo.Start);
        }

        #endregion
    }
}
