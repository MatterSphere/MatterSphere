using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.OMS;
using FWBS.OMS.Interfaces;

namespace FWBS.Common.UI.Windows
{
    internal class NavigationPanelFactory
    {
        private const string ADVANCED_SECURITY_CODE = "ADVSECURITY";
        private readonly string ADVANCED_SECURITY_DESCRIPTION = Session.CurrentSession.Resources.GetResource("SECURITY", "Security", "").Text;
        private const int ADVANCED_SECURITY_ICON = 23;

        private readonly ucNavigationPanel _mainPanel;
        private readonly ucHorizontalNavigationPanel _subPanel;

        public NavigationPanelFactory(ucNavigationPanel mainPanel)
        {
            _mainPanel = mainPanel;
        }

        public NavigationPanelFactory(ucHorizontalNavigationPanel subPanel)
        {
            _subPanel = subPanel;
        }

        public NavigationBuilder CreateBuilder(
            Dictionary<TabPage, OMSType.Tab> tabContents,
            System.Windows.Forms.TabControl tcEnquiryPages,
            int advancedSecurityTabIndex, OMSType omsType)
        {
            var groups = CreateGroupList(tabContents);
            var tabs = CreateTabDataList(tcEnquiryPages, tabContents);
            CheckAdvancedSecurityTab(advancedSecurityTabIndex, tabs);

            if (omsType is CommandCentreType)
            {
                return new NavigationPanelBuilder(_mainPanel, groups, tabs);
            }

            return new HorizontalMenuBuilder(_subPanel, groups, tabs); 
        }

        private Dictionary<string, GroupData> CreateGroupList(Dictionary<TabPage, OMSType.Tab> tabContents)
        {
            var groups = new Dictionary<string, GroupData>();

            foreach (var tp in tabContents)
            {
                if (!string.IsNullOrEmpty(tp.Key.ToString()))
                {
                    if (!groups.ContainsKey(tp.Value.Group))
                    {
                        groups.Add(tp.Value.Group, new GroupData(tp.Value.Group));
                    }
                }
            }

            return groups;
        }

        private List<TabData> CreateTabDataList(System.Windows.Forms.TabControl tcEnquiryPages, Dictionary<TabPage, OMSType.Tab> tabContents)
        {
            var tabProperties = new List<TabData>();

            foreach (TabPage tab in tcEnquiryPages.TabPages)
            {
                foreach (KeyValuePair<TabPage, OMSType.Tab> tp in tabContents)
                {
                    if (tab.Name == tp.Key.Name)
                    {
                        var tabData = new TabData(
                            tp.Key.Name,
                            tab.Text,
                            tp.Value.Group = tp.Value.Group,
                            tab.ImageIndex);
                        tabProperties.Add(tabData);
                        break;
                    }
                }
            }

            return tabProperties;
        }

        private void CheckAdvancedSecurityTab(int advancedSecurityTabIndex, List<TabData> tabs)
        {
            if (advancedSecurityTabIndex != -1)
            {
                var advancedSecurityTab = new TabData(ADVANCED_SECURITY_CODE, ADVANCED_SECURITY_DESCRIPTION,
                    ADVANCED_SECURITY_CODE, ADVANCED_SECURITY_ICON);
                tabs.Add(advancedSecurityTab);
            }
        }
    }
}
