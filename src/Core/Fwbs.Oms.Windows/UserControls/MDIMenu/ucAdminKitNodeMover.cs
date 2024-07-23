using System;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using swf = System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public partial class ucAdminKitNodeMover : BaseForm
    {

        #region variables

        private long sectionid = 0;
        private long selectednodeid = 0; 
        System.Data.DataTable dtSections;
        private string mainparent = "";
        private const string dlMenuItems = "DAdminMenu";

        #endregion

        #region constructors


        public bool AddToSection
        {
            get { return ((System.Windows.Forms.CheckBox)this.chkSection).Checked; }
        }


        public long SelectedNodeID
        {
            get { return selectednodeid; }
            set { selectednodeid = value; }
        }


        public long sectionID
        {
            get { return sectionid; }
            set { sectionid = value; }
        }


        public ucAdminKitNodeMover(bool AddToSection, string MainParent)
        {
            mainparent = MainParent;
            InitializeComponent();
            BuildSectionList();
            AllowAdditionToSection(AddToSection);

            lblTitle.Text = Session.CurrentSession.Resources.GetResource("MOVERTITLE", "Select the section from the left-hand panel, then the destination node from the right-hand panel.","").Text;
            chkSection.Text = Session.CurrentSession.Resources.GetResource("CHKSECTION", "Add to selected Section", "").Text;
        }


        #endregion

        #region Section List Setup

        private void BuildSectionList()
        {
            dtSections = GetTreeNodeData(dlMenuItems);
            CreateSectionListRootNode();
            CreateSectionNodes(dtSections);
            SetupSectionListEvents();
        }


        private System.Data.DataTable GetTreeNodeData(string dlName)
        {
            Common.KeyValueCollection kvc = new Common.KeyValueCollection();
            kvc.Add("name", mainparent);
            System.Data.DataTable dt = RunDataList(dlName, kvc);
            return dt;
        }


        private void CreateSectionListRootNode()
        {
            RadTreeNode node = TreeViewNavigation.TreeViewFormatter.NewTreeNode();
            if(mainparent == "ADMIN")
                node.Text = Session.CurrentSession.Resources.GetResource("ADESECTIONS", "Admin Kit Sections", "").Text;
            else
                node.Text = Session.CurrentSession.Resources.GetResource("REPSECTIONS", "3E MatterSphere Report Sections", "").Text;
            this.SectionList.Nodes.Add(node);
            this.SectionList.SelectedNode = node;
        }


        private void CreateSectionNodes(System.Data.DataTable dt)
        {
            DataRow[] sections = dt.Select("admnuParent = 1 and admnuSearchListCode <> 'LI'");
            for (int i = 0; i < sections.Length; i++)
            {
                RadTreeNode node = CreateTreeNode(sections[i]);
                this.SectionList.SelectedNode.Nodes.Add(node);
            }
            this.SectionList.ExpandAll();
        }


        private RadTreeNode CreateTreeNode(DataRow r)
        {
            RadTreeNode node = TreeViewNavigation.TreeViewFormatter.NewTreeNode();
            node.Text = Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", Convert.ToString(r["admnuCode"])), true);
            node.Tag = CreateTreeNodeTag(r);
            return node;
        }


        private TreeNodeTagData CreateTreeNodeTag(DataRow r)
        {
            TreeNodeTagData tag = new TreeNodeTagData(Convert.ToInt64(r["admnuID"]), Convert.ToString(r["admnuCode"]), Convert.ToString(r["admnuSearchListCode"]),
                                      Convert.ToBoolean(r["admnuSystem"]), Convert.ToString(r["admnuRoles"]));
            return tag;
        }

        #endregion

        #region Section List Events

        private void SetupSectionListEvents()
        {
            this.SectionList.MouseUp -= new MouseEventHandler(SectionList_MouseUp);
            this.SectionList.MouseUp += new MouseEventHandler(SectionList_MouseUp);
            this.SectionList.NodeFormatting -= TreeViewNavigation.TreeViewFormatter.NodeFormatting;
            this.SectionList.NodeFormatting += TreeViewNavigation.TreeViewFormatter.NodeFormatting;
        }

        private void SectionList_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.SectionList.Nodes.Count > 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    TreeNodeTagData tag = (TreeNodeTagData)this.SectionList.SelectedNode.Tag;
                    if (this.SectionTreeView.Nodes != null && this.SectionTreeView.Nodes.Count > 0)
                        this.SectionTreeView.Nodes.Clear();
                    if (tag != null)
                    {
                        CreateSectionTreeView(tag.objID);
                        sectionid = tag.objID;
                    }
                }
            }
        }



        #endregion

        #region TreeView Creation

        private void CreateSectionTreeView(long sectionID)
        {
            BuildTreeView(sectionID);
            SetupSectionTreeViewEvents();
            SetSelectedTreeViewNodeID();
            if (this.SectionTreeView.Nodes.Count > 0)
                this.SectionTreeView.SelectedNode = this.SectionTreeView.Nodes[0];
        }


        private void BuildTreeView(long sectionID)
        {
            System.Data.DataTable dtNodes = GetTreeNodeData(dlMenuItems);
            DataRow[] parents = dtNodes.Select("admnuParent = " + sectionID);
            for (int i = 0; i < parents.Length; i++)
            {
                CreateParentTreeNode(dtNodes, parents[i]);
            }
        }


        private void CreateParentTreeNode(System.Data.DataTable dt, DataRow r)
        {
            RadTreeNode parent = CreateParentNode(r);
            DataRow[] children = GetChildNodes(dt, Convert.ToInt32(r["admnuID"]));
            if (children.Length > 0)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    CreateChildTreeNode(dt, children[i], parent);
                }
                AddBlankTreeNode();
            }
        }


        private RadTreeNode CreateParentNode(DataRow r)
        {
            RadTreeNode node = TreeViewNavigation.TreeViewFormatter.NewTreeNode();
            node.Text = Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", Convert.ToString(r["admnuCode"])), true);
            node.Tag = CreateTreeNodeTag(r);
            this.SectionTreeView.Nodes.Add(node);
            return node;
        }


        private DataRow[] GetChildNodes(System.Data.DataTable dt, long parentID)
        {
            DataRow[] foundRows = dt.Select("admnuParent = " + parentID, "admnuOrder ASC");
            return foundRows;
        }


        private void CreateChildTreeNode(System.Data.DataTable dt, DataRow r, RadTreeNode parent)
        {
            this.SectionTreeView.SelectedNode = parent;
            RadTreeNode node = TreeViewNavigation.TreeViewFormatter.NewTreeNode();
            node.Text = Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", Convert.ToString(r["admnuCode"])), true);
            node.Tag = CreateTreeNodeTag(r);
            this.SectionTreeView.SelectedNode.Nodes.Add(node);

            DataRow[] nextlevel = GetChildNodes(dt, Convert.ToInt64(r["admnuID"]));
            if (nextlevel.Length != 0)
            {
                foreach (DataRow kr in nextlevel)
                {
                    if (Convert.ToInt64(r["admnuID"]) != 1)
                    {
                        CreateChildTreeNode(dt, kr, node);
                    }
                }
            }
        }


        private void AddBlankTreeNode()
        {
            RadTreeNode node = TreeViewNavigation.TreeViewFormatter.NewTreeNode();
            node.Text = string.Empty;
            node.Enabled = false;
            node.Tag = string.Empty;
            this.SectionTreeView.Nodes.Add(node);
        }

        #endregion

        #region Section TreeView Events


        private void SetupSectionTreeViewEvents()
        {
            this.SectionTreeView.MouseUp -= new MouseEventHandler(SectionTreeView_MouseUp);
            this.SectionTreeView.MouseUp += new MouseEventHandler(SectionTreeView_MouseUp);
            this.SectionTreeView.NodeFormatting -= TreeViewNavigation.TreeViewFormatter.NodeFormatting;
            this.SectionTreeView.NodeFormatting += TreeViewNavigation.TreeViewFormatter.NodeFormatting;
        }


        private void SectionTreeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.SectionTreeView.Nodes.Count > 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    SetSelectedTreeViewNodeID();
                }
            }
        }


        private void SetSelectedTreeViewNodeID()
        {
            if (this.SectionTreeView.SelectedNode != null)
            {
                TreeNodeTagData tag = (TreeNodeTagData)this.SectionTreeView.SelectedNode.Tag;
                selectednodeid = tag.objID;
            }
        }


        #endregion

        #region Button Handler

        private void btnMoveMode_Click(object sender, EventArgs e)
        {
            if (sectionid == 0)
            {
                swf.MessageBox.Show(ResourceLookup.GetLookupText("NODEMVRNOCONS"), ResourceLookup.GetLookupText("NODEMVRHEADER"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (!this.chkSection.Checked && selectednodeid == 0)
            {
                swf.MessageBox.Show(ResourceLookup.GetLookupText("NODEMVRNONODE"), ResourceLookup.GetLookupText("NODEMVRHEADER"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        #endregion

        #region Infrastructure

        private bool IsInRoles(string role, FWBS.OMS.User u)
        {
            if (string.IsNullOrEmpty(role))
                return true;

            string[] vals = u.Roles.Split(',');
            return Array.IndexOf(vals, role) > -1;
        }


        protected System.Data.DataTable RunDataList(string dataList, FWBS.Common.KeyValueCollection kvc)
        {
            FWBS.OMS.EnquiryEngine.DataLists dl = new FWBS.OMS.EnquiryEngine.DataLists(dataList);
            dl.ChangeParameters(kvc);
            System.Data.DataTable dt = dl.Run(false) as System.Data.DataTable;
            return dt;
        }


        protected System.Data.DataTable RunDataList(string dataList)
        {
            FWBS.OMS.EnquiryEngine.DataLists dl = new FWBS.OMS.EnquiryEngine.DataLists(dataList);
            System.Data.DataTable dt = dl.Run(false) as System.Data.DataTable;
            return dt;
        }


        private void AllowAdditionToSection(bool addtosection)
        {
            this.chkSection.Visible = addtosection;
        }


        #endregion

    }
    
}
