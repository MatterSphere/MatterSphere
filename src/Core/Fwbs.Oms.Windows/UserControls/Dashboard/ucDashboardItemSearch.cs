using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard
{
    public partial class ucDashboardItemSearch : UserControl
    {
        public ucDashboardItemSearch()
        {
            InitializeComponent();
            SetIcons();
        }

        public EventHandler Closed;
        public EventHandler<string> QueryChanged;
        public EventHandler<string> SearchCalled;

        public void ShowSearch()
        {
            searchBox.Text = null;
            this.Visible = true;
            searchBox.Focus();
        }

        private void SetIcons()
        {
            resetIcon.Image = FWBS.OMS.UI.Windows.Images.GetResetIcon(DeviceDpi);
            searchIcon.Image = FWBS.OMS.UI.Windows.Images.GetSearchIcon(DeviceDpi);
        }

        private void resetIcon_DpiChangedAfterParent(object sender, System.EventArgs e)
        {
            SetIcons();
        }

        private void resetIcon_Click(object sender, System.EventArgs e)
        {
            searchBox.Text = null;
            Closed?.Invoke(this, e);
        }

        private void searchBox_TextChanged(object sender, EventArgs e)
        {
            QueryChanged?.Invoke(this, searchBox.Text?.Trim().ToLower());
        }

        private void searchBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                SearchCalled?.Invoke(this, searchBox.Text?.Trim().ToLower());
            }
        }
    }
}
