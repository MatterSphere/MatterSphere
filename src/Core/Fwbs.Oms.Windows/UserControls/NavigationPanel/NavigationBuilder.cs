using System.Collections.Generic;
using System.Linq;

namespace FWBS.Common.UI.Windows
{
    internal abstract class NavigationBuilder
    {
        private const string _commonGroupCode = "COMMON";

        protected NavigationBuilder(Dictionary<string, GroupData> groups, List<TabData> tabs)
        {
            Groups = groups;
            Tabs = tabs;
        }

        protected Dictionary<string, GroupData> Groups { get; set; }
        protected List<TabData> Tabs { get; set; }

        public abstract void Build(string currentItem);

        protected void FillGroups()
        {
            var commonGroup = new GroupData(null);
            foreach (var tab in Tabs)
            {
                if (Groups.ContainsKey(tab.Group))
                {
                    Groups[tab.Group].AddItem(tab.Code, tab.Description, tab.ImageIndex);
                }
                else
                {
                    commonGroup.AddItem(tab.Code, tab.Description, tab.ImageIndex);
                }
            }

            if (commonGroup.Items.Any())
            {
                Groups.Add(_commonGroupCode, commonGroup);
            }
        }
    }
}
