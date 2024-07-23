using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.Common.UI.Windows
{
    public partial class HorizontalNavPanelPopup : UserControl
    {
        public HorizontalNavPanelPopup()
        {
            InitializeComponent();
        }

        public void AddItems(List<NavigationPopupItem> items)
        {
            Controls.Clear();
            if (!items.Any())
            {
                return;
            }
            
            for (int i = items.Count - 1; i >= 0; i--)
            {
                Controls.Add(items[i], true);
            }

            var maxSize = GetMaxSize(items);
            MinimumSize = new Size(maxSize.Width + LogicalToDeviceUnits(30), 0);
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);

            var items = new List<NavigationPopupItem>();
            foreach (var control in Controls)
            {
                items.Add(control as NavigationPopupItem);
            }

            var maxSize = GetMaxSize(items);
            MinimumSize = new Size(maxSize.Width + LogicalToDeviceUnits(30), 0);
        }

        private Size GetMaxSize(List<NavigationPopupItem> items)
        {
            var maxWidth = 0;
            var maxHeight = 0;
            foreach (var item in items)
            {
                var size = TextRenderer.MeasureText(item.Text, item.Font);
                maxWidth = System.Math.Max(size.Width, maxWidth);
                maxHeight = System.Math.Min(size.Height, maxHeight);
            }

            return new Size(maxWidth, maxHeight);
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if ((keyData & Keys.Alt) == Keys.Alt)
            {
                if ((keyData & Keys.F4) == Keys.F4)
                {
                    this.Parent.Hide();
                    return true;
                }
            }


            if ((keyData & Keys.Enter) == Keys.Enter)
            {
                if (this.ActiveControl is Button)
                {
                    (this.ActiveControl as Button).PerformClick();
                    return true;
                }
            }

            return base.ProcessDialogKey(keyData);
        }
    }
}
