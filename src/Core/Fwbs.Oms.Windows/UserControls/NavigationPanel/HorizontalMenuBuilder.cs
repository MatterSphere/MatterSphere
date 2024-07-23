using System.Collections.Generic;
using System.Linq;

namespace FWBS.Common.UI.Windows
{
    internal class HorizontalMenuBuilder : NavigationBuilder
    {
        private readonly ucHorizontalNavigationPanel _panel;

        public HorizontalMenuBuilder(ucHorizontalNavigationPanel panel, Dictionary<string, GroupData> groups, List<TabData> tabs) : base(groups, tabs)
        {
            _panel = panel;
        }

        public override void Build(string currentItem)
        {
            FillGroups();
            var items = Groups.SelectMany(group => group.Value.Items)
                .Select(item => CreateMenuItem(item.Description, item.Code)).ToList();
            _panel.AddItems(items);
            _panel.SelectItem(currentItem);
            _panel.Visible = true;
        }
        
        private ucHorizontalNavigationPanelItem CreateMenuItem(string title, string code)
        {
            var item = new ucHorizontalNavigationPanelItem
            {
                Text = title,
                Code = code
            };

            return item;
        }
    }
}
