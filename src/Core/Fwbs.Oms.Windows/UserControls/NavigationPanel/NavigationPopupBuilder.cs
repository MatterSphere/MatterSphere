using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.Common.UI.Windows
{
    public class NavigationPopup
    {
        public void Build(ucHorizontalNavigationPanel panel)
        {
            if (!panel.FlowLayoutPanel.Controls.Any())
            {
                return;
            }

            var info = UpdateItems(panel.FlowLayoutPanel);
            var items = GetItems(info.HorizontalItems, panel.FlowLayoutPanel);
            var menuItems = items.Select(lbl => new NavigationPopupItem
            {
                Text = lbl.Text,
                Code = lbl.Code
            }).ToList();

            panel.SetPopupItems(menuItems);
        }

        public PanelInfo UpdateItems(FlowLayoutPanel panel)
        {
            if (!panel.Controls.Any())
            {
                return new PanelInfo();
            }

            if (!panel.Visible)
            {
                return new PanelInfo(0, panel.Controls.Count);
            }

            if (panel.Controls.Count == 1)
            {
                panel.Controls[0].Visible = true;
                return new PanelInfo(1, 0);
            }

            Control prevControl = panel.Controls[0];
            int index = 0;
            for (int i = 1; i < panel.Controls.Count; i++)
            {
                panel.Controls[i].Visible = true;
                if (prevControl.Left >= panel.Controls[i].Left)
                {
                    index = i;
                    break;
                }

                prevControl = panel.Controls[i];

                if (i == panel.Controls.Count - 1)
                {
                    index = panel.Controls.Count;
                }
            }

            for (int i = index; i < panel.Controls.Count; i++)
            {
                panel.Controls[i].Visible = false;
            }

            return new PanelInfo(index, panel.Controls.Count - index);
        }

        private List<IHorizontalItem> GetItems(int index, FlowLayoutPanel panel)
        {
            var items = new List<IHorizontalItem>();
            for (int i = index; i < panel.Controls.Count; i++)
            {
                var item = panel.Controls[i] as IHorizontalItem;
                items.Add(item);
            }

            return items;
        }

        public class PanelInfo
        {
            public PanelInfo()
            {

            }

            public PanelInfo(int horizontalItems, int popupMenuItems)
            {
                HorizontalItems = horizontalItems;
                PopupMenuItems = popupMenuItems;
            }

            public int HorizontalItems { get; set; }
            public int PopupMenuItems { get; set; }
        }
    }
}
