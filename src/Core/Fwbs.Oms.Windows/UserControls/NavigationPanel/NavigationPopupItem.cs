using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public partial class NavigationPopupItem : Label, IHorizontalItem
    {
        public event EventHandler onClicked;

        public NavigationPopupItem()
        {
            InitializeComponent();
        }

        public string Code { get; set; }

        private void Item_Clicked(object sender, System.EventArgs e)
        {
            ToolStripDropDown dropDown = Parent?.Parent as ToolStripDropDown;
            if (dropDown != null)
            {
                dropDown.Close(ToolStripDropDownCloseReason.ItemClicked);
            }
            onClicked?.Invoke(this, e);
        }

        private void NavigationPopupItem_MouseHover(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(208, 224, 242);
        }

        private void NavigationPopupItem_MouseLeave(object sender, EventArgs e)
        {
            BackColor = Color.White;
        }
    }
}
