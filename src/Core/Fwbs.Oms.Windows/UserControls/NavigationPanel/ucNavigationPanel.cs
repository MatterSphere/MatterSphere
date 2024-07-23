using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public partial class ucNavigationPanel : UserControl
    {
        public event CancelEventHandler SelectedItemChanged;
        public event EventHandler<NavigationPanelStateChangedEventArgs> NavigationPanelStateChanged;

        protected virtual void OnNavigationPanelStateChanged(NavigationPanelStateChangedEventArgs e)
        {
            NavigationPanelStateChanged?.Invoke(this, e);
        }

        private State _state;
        private const int CompactSize = 56;

        private List<ucNavigationItem> _labels;
        private List<ucNavigationItem> _groups;

        public ucNavigationPanel()
        {
            this.Visible = false;
            InitializeComponent();

            _state = State.Normal;
            _labels = new List<ucNavigationItem>();
            _groups = new List<ucNavigationItem>();
        }

        private int NormalSize => LogicalToDeviceUnits(280);

        public ImageList Images { get; private set; }

        [DefaultValue(null)]
        public string SelectedItem { get; set; }
        
        public void AddGroup(ucNavigationItem group, List<ucNavigationItem> items)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                items[i].onClicked += SelectItem;
                MainPanel.Controls.Add(items[i]);
                items[i].Dock = DockStyle.Top;
                _labels.Add(items[i]);
            }
            
            MainPanel.Controls.Add(group);
            group.Dock = DockStyle.Top;
            _groups.Add(group);
        }

        public void ScalePanel()
        {
            var dpiRatio = DeviceDpi / 96f;
            MainPanel.Scale(new SizeF(dpiRatio, dpiRatio));
        }

        public void LoadIcons()
        {
            Images = FWBS.OMS.UI.Windows.Images.GetNavigationIcons(DeviceDpi);
        }

        public void SetNavigationIcon()
        {
            MenuImage.Image = FWBS.OMS.UI.Windows.Images.GetNavigationIcon(DeviceDpi);
        }

        public Image GetGroupItemIcon()
        {
            return FWBS.OMS.UI.Windows.Images.GetNavigationGroupItemIcon(DeviceDpi);
        }

        public void ClearNavigationItems()
        {
            MainPanel.Controls.Clear();
            _labels.Clear();
            _groups.Clear();
        }

        public void SelectItem(string code, bool withNotification = true)
        {
            var label = _labels.FirstOrDefault(item => item.Code == code);
            if (label == null)
            {
                SelectedItem = null;
                ResetSelection();
                return;
            }

            SelectItem(label, withNotification);
        }

        public bool IsSearchPageOpened
        {
            get { return SelectedItem == null; }
        }

        public void InitState(bool expand)
        {
            var newState = expand ? State.Normal : State.Compact;
            if (_state != newState)
            {
                _state = newState;
                ApplyNewState();
            }
        }

        private void ResetSelection()
        {
            foreach (var label in _labels)
            {
                label.UnselectItem();
            }
        }

        private void SelectItem(object sender, System.EventArgs e)
        {
            var label = (ucNavigationItem) sender;
            SelectItem(label, true);
        }

        private void SelectItem(ucNavigationItem label, bool withNotification)
        {
            string currentItem = SelectedItem;
            SelectedItem = label.Code;
            ResetSelection();
            label.SelectItem();

            if (withNotification)
            {
                CancelEventArgs args = new CancelEventArgs();
                SelectedItemChanged?.Invoke(this, args);
                if (args.Cancel)
                    SelectItem(currentItem, false);
            }
        }

        private void ChangeState()
        {
            switch (_state)
            {
                case State.Normal:
                    _state = State.Compact;
                    break;
                case State.Compact:
                    _state = State.Normal;
                    break;
            }

            ApplyNewState();
        }

        private void ApplyNewState()
        {
            switch (_state)
            {
                case State.Normal:
                    this.Width = NormalSize;
                    MainPanel.AutoScroll = true;
                    break;
                case State.Compact:
                    MainPanel.AutoScroll = false;
                    this.Width = LogicalToDeviceUnits(CompactSize);
                    break;
            }

            NavigationLabel.Visible = _state == State.Normal;

            foreach (var group in _groups)
            {
                group.ChangeGroupItemState(_state == State.Compact);
            }

            foreach (var label in _labels)
            {
                label.ChangePageItemState(_state == State.Compact);
            }
        }

        protected override void OnDpiChangedAfterParent(EventArgs e)
        {
            base.OnDpiChangedAfterParent(e);
            
            var images = FWBS.OMS.UI.Windows.Images.GetNavigationIcons(DeviceDpi);
            foreach (var label in _labels)
            {
                label.UpdateImage(images);
            }

            var navigationPanelIcon = FWBS.OMS.UI.Windows.Images.GetNavigationGroupItemIcon(DeviceDpi);
            foreach (var group in _groups)
            {
                group.UpdateImage(navigationPanelIcon);
            }

            MenuImage.Image = FWBS.OMS.UI.Windows.Images.GetNavigationIcon(DeviceDpi);
        }

        private void MenuImage_Click(object sender, System.EventArgs e)
        {
            ChangeState();
            OnNavigationPanelStateChanged(new NavigationPanelStateChangedEventArgs { Expand = _state == State.Normal });
        }

        private enum State
        {
            Normal = 1,
            Compact = 2
        }
    }

    public class NavigationPanelStateChangedEventArgs : EventArgs
    {
        public bool Expand { get; set; }
    }
}
