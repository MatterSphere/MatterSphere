using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public partial class ucHorizontalNavigationPanel : UserControl
    {
        public event EventHandler SelectedItemChanged;

        HorizontalNavPanelPopup _popup;
        NavigationPopupContainer _container;
        private List<ucHorizontalNavigationPanelItem> _items;
        private List<NavigationPopupItem> _contextMenuItems;
        private NavigationPopup _builder;

        public ucHorizontalNavigationPanel()
        {
            this.Visible = false;
            InitializeComponent();
            
            _popup = new HorizontalNavPanelPopup();
            _container = new NavigationPopupContainer(_popup);
            _items = new List<ucHorizontalNavigationPanelItem>();
            _contextMenuItems = new List<NavigationPopupItem>();
            _builder = new NavigationPopup();
        }

        [DefaultValue(null)]
        public string SelectedItem { get; set; }

        public void AddItems(List<ucHorizontalNavigationPanelItem> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].onClicked += SelectItem;
                FlowLayoutPanel.Controls.Add(items[i]);
                _items.Add(items[i]);
            }
        }

        public void SelectItem(string code)
        {
            var item = _items.FirstOrDefault(it => it.Code == code);
            if (item == null)
            {
                return;
            }

            SelectItem(item);
        }

        public void SetPopupItems(List<NavigationPopupItem> items)
        {
            _contextMenuItems.Clear();
            foreach (var item in items)
            {
                item.onClicked += PopupSelectedItemChanged;
                _contextMenuItems.Add(item);
            }
        }

        private void PopupSelectedItemChanged(object sender, System.EventArgs e)
        {
            ResetSelections();
            var item = sender as IHorizontalItem;
            if (item != null)
            {
                SelectedItem = item.Code;
                SelectItemChanged(sender, e);
            }
        }

        private void SelectItemChanged(object sender, System.EventArgs e)
        {
            SelectedItemChanged?.Invoke(sender, e);
        }

        private void SelectItem(object sender, System.EventArgs e)
        {
            var item = (ucHorizontalNavigationPanelItem)sender;
            
            SelectItem(item);
        }

        private void SelectItem(ucHorizontalNavigationPanelItem item)
        {
            SelectedItem = item.Code;
            ResetSelections();
            item.SelectItem();
            SelectItemChanged(this, EventArgs.Empty);
        }

        private void ResetSelections()
        {
            foreach (var item in _items)
            {
                item.UnselectItem();
            }
        }

        private void moreButton_Click(object sender, EventArgs e)
        {
            UpdatePopupItems();
            _popup.AddItems(_contextMenuItems);
            _container.Show(this.moreButton);
        }

        private void Size_Changed(object sender, EventArgs e)
        {
            if (this.Enabled)
            {
                UpdateItems();
            }
        }

        private void UpdateItems()
        {
            var info = _builder.UpdateItems(this.FlowLayoutPanel);
            this.FlowLayoutPanel.Visible = _items.Count > 0 && this.Width > _items[0].Width + moreButton.Width;
            moreButton.Visible = !this.FlowLayoutPanel.Visible || info.PopupMenuItems > 0;
        }

        private void UpdatePopupItems()
        {
            _builder.Build(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            SetLines(e);
        }

        private void SetLines(PaintEventArgs e)
        {
            using (var pen = new Pen(Color.FromArgb(189, 189, 189), this.Padding.Bottom))
            {
                e.Graphics.DrawLine(pen, 0, Height - this.Padding.Bottom, Width, Height - this.Padding.Bottom);
            }

            using (var pen = new Pen(Color.FromArgb(189, 189, 189), this.Padding.Top))
            {
                e.Graphics.DrawLine(pen, 0, 0, Width, 0);
            }
        }

        private void ucHorizontalNavigationPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                UpdateItems();
            }
        }

        private void FlowLayoutPanel_VisibleChanged(object sender, EventArgs e)
        {
            if (FlowLayoutPanel.Visible)
            {
                FlowLayoutPanel.SuspendLayout();
                foreach (Control control in FlowLayoutPanel.Controls)
                {
                    control.Visible = true;
                }

                FlowLayoutPanel.ResumeLayout();
            }
        }
    }
}
