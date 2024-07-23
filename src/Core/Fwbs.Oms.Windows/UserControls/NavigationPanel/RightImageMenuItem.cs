using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public partial class RightImageMenuItem : Button
    {
        public RightImageMenuItem()
        {
            InitializeComponent();

            chevron.Text = "";
            PreferredHeight = 40;
        }

        public int PreferredHeight { get; set; }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);

            this.Width = LogicalToDeviceUnits(85);
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            var size = TextRenderer.MeasureText(this.Text, this.Font);
            var newWidth = size.Width + LogicalToDeviceUnits(4) + chevron.Width;
            var newHeight = LogicalToDeviceUnits(PreferredHeight);
            return new Size(newWidth, newHeight);
        }

        private void chevron_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void chevron_MouseHover(object sender, EventArgs e)
        {
            this.BackColor = Color.FromArgb(229, 229, 229);
        }

        private void chevron_MouseLeave(object sender, EventArgs e)
        {
            this.BackColor = Color.Transparent;
        }
    }
}
