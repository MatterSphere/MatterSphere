using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.Windows.TreeViewNavigation
{
    #region TabData
    public class TabData
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public string Group { get; set; }
    }
    #endregion TabData


    #region TreeViewFormatter
    /// <summary>
    /// Tree view formatter helper to apply common node formatting to RadTreeView controls.
    /// </summary>
    public static class TreeViewFormatter
    {
        private static readonly bool UseFont = false;
        private static readonly Font NormFont = new Font("Segoe UI", 9F, FontStyle.Regular);
        private static readonly Font BoldFont = new Font("Segoe UI", 9F, FontStyle.Bold);

        public static RadTreeNode NewTreeNode()
        {
            return new RadTreeNode()
            {
                TextAlignment = UseFont ? ContentAlignment.MiddleLeft : ContentAlignment.TopLeft,
                ForeColor = Color.Black
            };
        }

        public static void NodeFormatting(object sender, TreeNodeFormattingEventArgs e)
        {
            e.NodeElement.ResetValue(LightVisualElement.BorderColorProperty, Telerik.WinControls.ValueResetFlags.Local);
            e.NodeElement.ResetValue(LightVisualElement.BackColorProperty, Telerik.WinControls.ValueResetFlags.Local);

            if (UseFont)
            {
                e.NodeElement.ContentElement.Font = (e.NodeElement.IsRootNode || e.NodeElement.IsSelected)
                    ? BoldFont
                    : NormFont;
            }
            else
            {
                e.NodeElement.ContentElement.Text = (e.NodeElement.IsRootNode || e.NodeElement.IsSelected)
                    ? string.Format(@"<html><strong>{0}</strong>", e.Node.Text)
                    : e.Node.Text;
            }

            if (e.NodeElement.IsMouseOver)
            {
                e.NodeElement.BackColor = Color.AliceBlue;
            }
            else if (e.NodeElement.IsSelected)
            {
                e.NodeElement.BackColor = Color.LightGray;
                e.NodeElement.BorderColor = Color.White;
            }
        }
    }
    #endregion TreeViewFormatter

    #region TreeViewBuilder
    public class TreeViewBuilder
    {
        #region Controls

        private TabControl tcEnquiryPages;
        private RadTreeView treeNavigation;
        private Dictionary<TabPage, OMSType.Tab> tabcontents;

        #endregion Controls

        #region Members

        private string _defaultPage = null;
        private int advancedSecurityTabIndex = -1;
        ArrayList FirstNodeTabCodes = new ArrayList();
        private const string ADVANCED_SECURITY_TREE_NODE_CODE = "ADVSECURITY";
        private const string ADVANCED_SECURITY_TREE_NODE_DESCRIPTION = "Security";

        #endregion Members

        #region Events

        public event EventHandler AfterBuild;
        public event EventHandler BeforeBuild;

        #endregion Events

        #region Constructors

        public TreeViewBuilder(RadTreeView treeNavigation, Dictionary<TabPage, OMSType.Tab> tabcontents
                               , TabControl tcEnquiryPages, FWBS.OMS.Interfaces.IOMSType obj, int advancedSecurityTabIndex = -1)
            : this(treeNavigation, tabcontents, tcEnquiryPages)
        {
            this.advancedSecurityTabIndex = advancedSecurityTabIndex;

            if (obj.DefaultTab != null)
                _defaultPage = obj.DefaultTab;
        }


        public TreeViewBuilder(RadTreeView treeNavigation, Dictionary<TabPage, OMSType.Tab> tabcontents, TabControl tcEnquiryPages)
        {
            this.treeNavigation = treeNavigation;
            this.tabcontents = tabcontents;
            this.tcEnquiryPages = tcEnquiryPages;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Build the navigation tree for display in the Navigation Panel
        /// </summary>
        /// <returns></returns>
        public bool Build()
        {
            try
            {
                OnBeforeBuild();

                this.treeNavigation.NodeFormatting -= TreeViewFormatter.NodeFormatting;
                this.treeNavigation.NodeFormatting += TreeViewFormatter.NodeFormatting;
                ProcessGroupCreation();
                ProcessNodeCreation();
                CheckForAdvancedSecurity();
                treeNavigation.ExpandAll();
                SetStartingSelectedNode();

                OnAfterBuild();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
                return false;
            }

            return true;
        }


        private void OnBeforeBuild()
        {
            if (BeforeBuild != null)
            {
                BeforeBuild(this, EventArgs.Empty);
            }
        }


        private void ProcessGroupCreation()
        {
            Dictionary<string, string> groups = GetDistinctListOfGroups(new Dictionary<string, string>());

            if (groups != null && groups.Count > 0)
            {
                CreateTreeNavigationMainGroups(groups);
            }
        }


        /// <summary>
        /// Builds a Dictionary of distinct groups assigned to tabs based on the current OMSType's tab configuration
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetDistinctListOfGroups(Dictionary<string, string> groups)
        {
            foreach (KeyValuePair<TabPage, OMSType.Tab> tp in tabcontents)
            {
                if (!string.IsNullOrEmpty(tp.Key.ToString()))
                {
                    if (string.IsNullOrEmpty(tp.Value.Group))
                    {
                        continue;
                    }

                    if (!groups.ContainsKey(tp.Value.Group))
                    {
                        groups.Add(tp.Value.Group, FWBS.OMS.CodeLookup.GetLookup("DLGGROUPCAPTION", tp.Value.Group));
                    }
                }
            }

            return groups;
        }


        /// <summary>
        /// Create the tree view main group nodes as set in the Configurable Types 
        /// section of the Admin Kit.
        /// </summary>
        /// <param name="groups"></param>
        private void CreateTreeNavigationMainGroups(Dictionary<string, string> groups)
        {
            foreach (KeyValuePair<string, string> group in groups)
            {
                RadTreeNode treeViewGroup = TreeViewFormatter.NewTreeNode();
                treeViewGroup.Text = GetGroupHeaderText(group.Key);
                treeViewGroup.Tag = group.Key;

                treeNavigation.Nodes.Add(treeViewGroup);
                AddBlankParentNodeToTreeView();
            }
        }


        private static string GetGroupHeaderText(string groupHeaderDescription)
        {
            return Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("DLGGROUPCAPTION", groupHeaderDescription), true);
        }


        internal void AddBlankParentNodeToTreeView()
        {
            RadTreeNode node = TreeViewFormatter.NewTreeNode();
            node.Text = string.Empty;
            node.Enabled = false;
            treeNavigation.Nodes.Add(node);
        }


        private void ProcessNodeCreation()
        {
            var tabDataForTreeNavigation = GetTabDataForTreeNavigation();
            CreateTreeNodes(tabDataForTreeNavigation);
        }

        internal RadTreeNode CreateChildTreeNode(TabData tabData)
        {
            foreach (RadTreeNode currentNode in treeNavigation.Nodes)
            {
                if (Convert.ToString(currentNode.Name) == tabData.Group)
                {
                    return CreateChildNode(tabData, currentNode);
                }
            }
            return null;
        }

        private void CreateTreeNodes(List<TreeViewNavigation.TabData> tabDataForTreeNavigation)
        {
            foreach (TreeViewNavigation.TabData tab in tabDataForTreeNavigation)
            {
                bool isGroupExists = false;

                foreach (RadTreeNode currentNode in treeNavigation.Nodes)
                {
                    if (Convert.ToString(currentNode.Tag) == tab.Group)
                    {
                        CreateChildNode(tab, currentNode);
                        isGroupExists = true;
                        break;
                    }
                }

                if (!isGroupExists)
                {
                    CreateParentNode(tab);
                }
            }
            UpdateParentNodesWithTabCodes();
        }

        internal RadTreeNode CreateChildNode(TreeViewNavigation.TabData tab, RadTreeNode currentNode)
        {
            return CreateChildNode(tab, currentNode, true);
        }

        internal RadTreeNode CreateChildNode(TreeViewNavigation.TabData tab, RadTreeNode currentNode, bool SetSelectedNode)
        {
            if (SetSelectedNode)
                treeNavigation.SelectedNode = currentNode;

            RadTreeNode node = TreeViewFormatter.NewTreeNode();
            node.Text = tab.Description;
            node.Tag = tab.Code;

            if (SetSelectedNode)
            {
                treeNavigation.SelectedNode.Nodes.Add(node);
            }
            else
            {
                currentNode.Nodes.Add(node);
            }

            RadTreeNode treeNode = SetSelectedNode ? treeNavigation.SelectedNode : currentNode;

            if (FirstChildNodeInGroup(treeNode))
            {
                FirstNodeTabCodes.Add(tab.Code);
            }
            return node;
        }

        private bool FirstChildNodeInGroup(RadTreeNode treeNode)
        {
            return treeNode.Nodes.Count == 1;
        }


        internal RadTreeNode CreateParentNode(TreeViewNavigation.TabData tab)
        {
            RadTreeNode node = TreeViewFormatter.NewTreeNode();
            node.Text = tab.Description;
            node.Tag = tab.Code;

            treeNavigation.Nodes.Add(node);
            return node;
        }



        private void UpdateParentNodesWithTabCodes()
        {
            int i = 0;
            foreach (RadTreeNode currentNode in treeNavigation.Nodes)
            {
                if (currentNode.Nodes.Count > 0)
                {
                    currentNode.Name = Convert.ToString(currentNode.Tag);
                    currentNode.Tag = Convert.ToString(FirstNodeTabCodes[i]);
                    i++;
                }
            }
        }


        private List<TreeViewNavigation.TabData> GetTabDataForTreeNavigation()
        {
            var tabPropertiesList = new List<TreeViewNavigation.TabData>();

            foreach (TabPage tab in tcEnquiryPages.TabPages)
            {
                foreach (KeyValuePair<TabPage, OMSType.Tab> tp in tabcontents)
                {
                    if (tab.Name == tp.Key.Name) // e.g. SCRFILMAIN
                    {
                        var tpftn = new TreeViewNavigation.TabData();
                        tpftn.Code = tp.Key.Name;
                        tpftn.Description = tab.Text;
                        tpftn.Group = (tp.Value.Group == string.Empty) ? tp.Key.Name : tp.Value.Group;

                        tabPropertiesList.Add(tpftn);
                        break;
                    }
                }
            }

            return tabPropertiesList;
        }


        private void CheckForAdvancedSecurity()
        {
            if (AdvancedSecurityTabExists())
            {
                CreateAdvancedSecurityTreeNode();
            }
        }


        private bool AdvancedSecurityTabExists()
        {
            return advancedSecurityTabIndex != -1;
        }


        private void CreateAdvancedSecurityTreeNode()
        {
            var advancedSecurityTab = new TreeViewNavigation.TabData() { Code = ADVANCED_SECURITY_TREE_NODE_CODE, Description = ADVANCED_SECURITY_TREE_NODE_DESCRIPTION, Group = ADVANCED_SECURITY_TREE_NODE_CODE };
            CreateParentNode(advancedSecurityTab);
        }


        private void SetStartingSelectedNode()
        {
            if (string.IsNullOrWhiteSpace(_defaultPage))
            {
                if (treeNavigation.Nodes.Count > 0)
                    treeNavigation.SelectedNode = treeNavigation.Nodes[0];
            }
            else
                treeNavigation.SelectedNode = FindNode(_defaultPage);
        }


        private Telerik.WinControls.UI.RadTreeNode FindNode(string obj)
        {
            foreach (Telerik.WinControls.UI.RadTreeNode n in treeNavigation.Nodes)
            {
                if (n.Nodes.Count == 0 || n.Nodes == null)
                {
                    if (Convert.ToString(n.Tag) == obj)
                        return n;
                }
                else
                {
                    foreach (Telerik.WinControls.UI.RadTreeNode node in n.Nodes)
                    {
                        if (Convert.ToString(node.Tag) == obj)
                            return node;
                    }
                }
            }
            return treeNavigation.Nodes[0];
        }


        private void OnAfterBuild()
        {
            if (AfterBuild != null)
            {
                AfterBuild(this, EventArgs.Empty);
            }
        }

        #endregion Methods
    }
    #endregion TreeViewBuilder
}

