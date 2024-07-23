using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Calendar
{
    internal partial class ucCalendarPickerPopup : UserControl
    {
        protected DateTime _firstItem;
        protected DateTime _currentDate;
        private const string _up = "";
        private const string _down = "";

        public ucCalendarPickerPopup()
        {
            InitializeComponent();
            
            btnUp.Text = _up;
            btnDown.Text = _down;

            var controls = GetAll(this);
            foreach (var control in controls)
            {
                control.MouseWheel += OnMouseWheel;
            }

            btnFirstItem.ItemClicked += OnItemClicked;
            btnSecondItem.ItemClicked += OnItemClicked;
            btnThirdItem.ItemClicked += OnItemClicked;
            btnFourthItem.ItemClicked += OnItemClicked;
            btnFifthItem.ItemClicked += OnItemClicked;
        }

        public EventHandler<DateTime> DayClicked;

        public void ChangeDpi(float dpi)
        {
            this.Scale(new System.Drawing.SizeF(dpi / 96, dpi / 96));
        }

        protected void SetDate(ucCalendarPickerPopupItem item, DateTime date)
        {
            item.SetDate(date, date == _currentDate);
        }

        protected void SetPaddings()
        {
            var maxWidth = 96 * GetLabelWidth(btnFirstItem) / DeviceDpi;
            var padding = (this.Width - maxWidth) / 2;
            btnFirstItem.Padding = new Padding(padding, 0, 0, 0);
            btnSecondItem.Padding = new Padding(padding, 0, 0, 0);
            btnThirdItem.Padding = new Padding(padding, 0, 0, 0);
            btnFourthItem.Padding = new Padding(padding, 0, 0, 0);
            btnFifthItem.Padding = new Padding(padding, 0, 0, 0);
        }

        protected virtual int GetLabelWidth(ucCalendarPickerPopupItem item)
        {
            return 0;
        }

        #region UI events

        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
            {
                NextItem();
            }
            else
            {
                PrevItem();
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            PrevItem();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            NextItem();
        }

        private void OnItemClicked(object sender, DateTime e)
        {
            DayClicked?.Invoke(this, e);
        }

        #endregion

        #region Private methods

        protected virtual void PrevItem()
        {
            throw new NotImplementedException();
        }

        protected virtual void NextItem()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Control> GetAll(Control control)
        {
            var controls = control.Controls.Cast<Control>();
            return controls.SelectMany(ctrl => GetAll(ctrl)).Concat(controls);
        }

        #endregion
    }
}
