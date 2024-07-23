using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Data;
using Telerik.WinControls.UI;
using swf = System.Windows.Forms;


namespace FWBS.OMS.UI.Windows.Admin
{
    public partial class ucOMSAdminKitConsole : UserControl
    {

        #region Variables & Constants
    
       
        int consoleID;
        private string filter;
        private int autoNodeID;
        bool nodeAdded = false;
        RadTreeNode backupNode;
        private frmMain frmMain;
        private int autoConsoleID;
        private string consoleparent;
        System.Data.DataTable dtNodes;
        private frmAdminDesktop parent;
        bool sdklicensedialogshown = false;

        Telerik.WinControls.UI.RadContextMenu parentMenu = new Telerik.WinControls.UI.RadContextMenu();
        Telerik.WinControls.UI.RadContextMenu childMenu = new Telerik.WinControls.UI.RadContextMenu();

        const string GroupWizard = "sADDTVGROUP";
        const string NodeWizard = "sADDTREENODE";
        const string constDeleteParent = "DDELPTVNODE";
        const string constDeleteChild = "DDELTVCNODE";
        const string constRemoveFromFav = "DRemoveAKFav";
        const string constCheckAKFavs = "DCheckAKFavs";

        const string constExtendedDataSDK = "-1718510834";
        const string constFullSDK = "AMUSDK";
        const string constReportSDK = "AMURPT";
        const string constSystemScripts = "AMUSYSSCRPT";
        const string constScreenDesigner = "AMUDESIG";
        

        #endregion

        #region Properties

        public Type panel2Object
        {
            get
            {
                if(this.panel2.Controls.Count > 0)
                    return panel2.Controls[0].GetType();
                else
                    return panel2.GetType();
            }
        }

        #endregion

        #region Constructors

        public ucOMSAdminKitConsole(frmMain frmMain, frmAdminDesktop parent)
        {
            this.parent = parent;
            this.parent.FormClosed += new FormClosedEventHandler(parent_FormClosed);
            this.frmMain = frmMain;
            InitializeComponent();
            SetConsoleParentType();
        }


        private void SetConsoleParentType()
        {
            if (frmMain.GetType().ToString() == "FWBS.OMS.UI.Windows.Admin.frmMain")
                consoleparent = "ADMIN";
            else
                consoleparent = "REPORTS";
        }


        private void parent_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.frmMain.OnfrmMainClosing();
        }

        
        public void LoadTreeView(string Filter)
        {
            filter = Filter;
            BuildConsoleWithFilter(filter);
            SetFont();
            parent_ShownFilter();
        }


        public void LoadTreeView(int ConsoleID, int NodeID)
        {
            ResetTreeViewControl();
            autoConsoleID = ConsoleID;
            autoNodeID = NodeID;
            BuildConsoleWithID(ConsoleID);
            this.parent.Text = GetConsoleCodeLookupDesc(ConsoleID);
            SetFont();
            parent_ShownID();
        }


        private string GetConsoleCodeLookupDesc(int consoleID)
        {
            string consoleDescription = "";
            System.Data.DataTable dt = RunDataList("DConsoleList");
            foreach (DataRow r in dt.Rows)
            {
                if (Convert.ToInt32(r["ID"]) == consoleID)
                    consoleDescription = Convert.ToString(r["description"]);
            }
            return consoleDescription;
        }


        private void parent_ShownID()
        {
            AutoSelectNode(autoConsoleID, autoNodeID);
        }


        private void parent_ShownFilter()
        {
            if (this.radTreeView1.Nodes.Count == 0)
                AddParentGroupToTreeView();
        }


        private void BuildConsoleWithFilter(string filter)
        {
            ConstructTree(filter);
        }


        private void BuildConsoleWithID(int ConsoleID)
        {
            consoleID = ConsoleID;
            SetupContextMenus();
            SetupTreeView(ConsoleID);
        }


        private void ConstructTree(string filter)
        {
            ResetTreeViewControl();
            SetupContextMenus();
            SetupTreeView(filter);
            SetFont();
        }


        #endregion

        #region TreeView Setup (Infrastructure)

        private void SetupTreeView(string filter)
        {
            ResetPanelVariables();
            dtNodes = GetTreeNodeData();
            GetParentNodes(dtNodes,filter);
            SetupTreeEvents();
            if(radTreeView1.Nodes.Count > 0)
                radTreeView1.SelectedNode = radTreeView1.Nodes[0];
            CreateWelcomePage(filter);
        }


        private void SetupTreeView(int ConsoleID)
        {
            dtNodes = GetTreeNodeData();
            GetParentNodes(dtNodes, ConsoleID);
            SetupTreeEvents();
            if(radTreeView1.Nodes.Count > 0)
                radTreeView1.SelectedNode = radTreeView1.Nodes[0];
        }


        private System.Data.DataTable GetTreeNodeData()
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("@UI", Session.CurrentSession.CurrentUser.PreferedCulture));
            System.Data.DataTable dt = connection.ExecuteProcedure("GetAdminKitMenuData", parList);
            return dt;
        }


        private void SetupContextMenus()
        {
            radTreeView1.Nodes.Clear();
            SetupParentContextMenu();
            SetupChildContextMenu();
        }


        private void SetupParentContextMenu()
        {
            if (parentMenu.Items.Count == 0)
            {
                parentMenu.Items.Add(CreateAddParent("ADDNEWGRP"));
                parentMenu.Items.Add(CreateEditParent("EDITGRP"));
                parentMenu.Items.Add(AddNewMemberToParent("ADDNEWGRPMEM"));
                parentMenu.Items.Add(RemoveParent("REMOVEGRP"));
                parentMenu.Items.Add(MoveParent("MOVEGRP"));
            }
        }


        private RadMenuItem CreateAddParent(string  lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem addparent = new Telerik.WinControls.UI.RadMenuItem();
            addparent.Text = ResourceLookup.GetLookupText(lookupText); //"Add New Group"
            addparent.MouseUp += new MouseEventHandler(addparent_MouseUp);
            return addparent;
        }


        private RadMenuItem CreateEditParent(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem editparent = new Telerik.WinControls.UI.RadMenuItem();
            editparent.Text = ResourceLookup.GetLookupText(lookupText); //"Edit Group"
            editparent.MouseUp += new MouseEventHandler(editparent_MouseUp);
            return editparent;
        }


        private RadMenuItem AddNewMemberToParent(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem AddMemberToParent = new Telerik.WinControls.UI.RadMenuItem();
            AddMemberToParent.Text = ResourceLookup.GetLookupText(lookupText); //"Add New Group Member"
            AddMemberToParent.MouseUp += new MouseEventHandler(AddMemberToParent_MouseUp);
            return AddMemberToParent;
        }


        private RadMenuItem RemoveParent(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem removeparent = new Telerik.WinControls.UI.RadMenuItem();
            removeparent.Text = ResourceLookup.GetLookupText(lookupText); //"Remove Group"
            removeparent.MouseUp += new MouseEventHandler(removeparent_MouseUp);
            return removeparent;
        }


        private RadMenuItem MoveParent(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem moveparent = new Telerik.WinControls.UI.RadMenuItem();
            moveparent.Text = ResourceLookup.GetLookupText(lookupText); //"Move Group"
            moveparent.MouseUp += new MouseEventHandler(moveparent_MouseUp);
            return moveparent;
        }   


        private void SetupChildContextMenu()
        {
            if (childMenu.Items.Count == 0)
            {
                childMenu.Items.Add(AddChild("ADDNEWGRPMEM"));
                childMenu.Items.Add(EditChild("EDITGRPMEM"));
                childMenu.Items.Add(RemoveChild("REMOVEGRPMEM"));
                childMenu.Items.Add(AddChildToFav("ADDTOFAV"));
                childMenu.Items.Add(MoveChildNode("MOVENODE"));
            }
        }


        private RadMenuItem AddChild(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem addchild = new Telerik.WinControls.UI.RadMenuItem();
            addchild.Text = ResourceLookup.GetLookupText(lookupText); //"Add New Group Member"; 
            addchild.MouseUp += new MouseEventHandler(addchild_MouseUp);
            return addchild;
        }


        private RadMenuItem EditChild(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem editchild = new Telerik.WinControls.UI.RadMenuItem();
            editchild.Text = ResourceLookup.GetLookupText(lookupText); //"Edit Group Member";
            editchild.MouseUp += new MouseEventHandler(editchild_MouseUp);
            return editchild;
        }


        private RadMenuItem RemoveChild(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem removechild = new Telerik.WinControls.UI.RadMenuItem();
            removechild.Text = ResourceLookup.GetLookupText(lookupText); //"Remove Group Member";
            removechild.MouseUp += new MouseEventHandler(removechild_MouseUp);
            return removechild;
        }


        private RadMenuItem AddChildToFav(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem addchildtofav = new Telerik.WinControls.UI.RadMenuItem();
            addchildtofav.Text = ResourceLookup.GetLookupText(lookupText); //"Add to Favourites";
            addchildtofav.MouseUp += new MouseEventHandler(addchildtofav_MouseUp);
            return addchildtofav;
        }


        private RadMenuItem MoveChildNode(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem movechildnode = new Telerik.WinControls.UI.RadMenuItem();
            movechildnode.Text = ResourceLookup.GetLookupText(lookupText); //"Move Node";
            movechildnode.MouseUp += new MouseEventHandler(movechildnode_MouseUp);
            return movechildnode;
        }


        #endregion

        #region Tree Element Creation

        private void GetParentNodes(System.Data.DataTable dt, string filter)
        {
            string sql = "select admnuID as ID, 'AKC|" +  filter + "' as consolecode from dbAdminMenu where admnuSearchListCode = 'AKC|" + filter + "'";
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("filter", filter));
            System.Data.DataTable dtID = connection.ExecuteSQL(sql, parList);
            consoleID = Convert.ToInt32(dtID.Rows[0]["ID"]);
            GetParentNodes(dt, consoleID);
        }


        private void GetParentNodes(System.Data.DataTable dt, int consoleID)
        {
            DataRow[] parents = dt.Select("admnuParent = " + consoleID);
            for (int i = 0; i < parents.Length; i++)
            {
                CreateParentTreeNode(dt, parents[i]);
            }
        }


        private void CreateParentTreeNode(System.Data.DataTable dt, DataRow r)
        {
            bool prevention = true;
            if(CheckUserHasAccessToNodeAtCreation(r))
            {
                RadTreeNode parent = CreateParentNode(r);

                prevention = PreventSDKAccess(Convert.ToString(r["admnuCode"]));
                if(!prevention)
                {
                    ProcessChildNodes(dt,r,parent);
                    return;
                }
                if (prevention && Convert.ToString(r["admnuCode"]) == constFullSDK)
                {
                    if (CheckForScreenDesignerLicense())
                    {
                        DataRow [] designer = GetScreenDesignerNode(dt, Convert.ToInt64(r["admnuID"]));
                        CreateChildTreeNode(dt, designer[0], parent);
                    }
                }
            }
        }


        private void ProcessChildNodes(System.Data.DataTable dt, DataRow r, RadTreeNode parent)
        {
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
            node.Text = Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", Convert.ToString(r["admnuCode"])),true);
            node.Tag = CreateTreeNodeTag(r);
            node.ContextMenu = parentMenu;
            this.radTreeView1.Nodes.Add(node);
            return node;
        }


        private DataRow[] GetChildNodes(System.Data.DataTable dt, long parentID)
        {
            DataRow[] foundRows = dt.Select("admnuParent = " + parentID);
            return foundRows;
        }


        private DataRow [] GetScreenDesignerNode(System.Data.DataTable dt, long parentID)
        {
            DataRow [] designerrow = dt.Select("admnuParent = " + parentID + " and admnuCode = '" + constScreenDesigner + "'");
            return designerrow;
        }
        

        private void CreateChildTreeNode(System.Data.DataTable dt, DataRow r, RadTreeNode parent)
        {
            if(CheckUserHasAccessToNodeAtCreation(r))
            {
                this.radTreeView1.SelectedNode = parent;
                RadTreeNode node = TreeViewNavigation.TreeViewFormatter.NewTreeNode();
                node.Text = Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", Convert.ToString(r["admnuCode"])),true);
                node.Tag = CreateTreeNodeTag(r);
                this.radTreeView1.SelectedNode.Nodes.Add(node);

                DataRow[] nextlevel = GetChildNodes(dt, Convert.ToInt64(r["admnuID"]));
                if (nextlevel.Length != 0)
                {
                    node.ContextMenu = parentMenu;
                    foreach (DataRow kr in nextlevel)
                    {
                        if (Convert.ToInt64(r["admnuID"]) != 1)
                        {
                            CreateChildTreeNode(dt, kr, node);
                        }
                    }
                }
                else
                {
                    node.ContextMenu = childMenu;
                }
            }
        }


        private TreeNodeTagData CreateTreeNodeTag(DataRow r)
        {
            TreeNodeTagData tag = new TreeNodeTagData(Convert.ToInt64(r["admnuID"]), Convert.ToString(r["admnuCode"]), Convert.ToString(r["admnuSearchListCode"]),
                                      ConvertDef.ToBoolean(r["admnuSystem"],false), Convert.ToString(r["admnuRoles"]));
            return tag;
        }


        private void AddBlankTreeNode()
        {
            RadTreeNode node = TreeViewNavigation.TreeViewFormatter.NewTreeNode();
            node.Text = string.Empty;
            node.Enabled = false;
            node.Tag = string.Empty;
            this.radTreeView1.Nodes.Add(node);
        }


        public void AutoSelectNode(int consoleID, int nodeID)
        {
            RadTreeNode parent;

            for (int i = 0; i < radTreeView1.Nodes.Count; i++)
            {
                parent = radTreeView1.Nodes[i];

                if (NodeIsBlankTreeNode(parent))
                {
                    continue;
                }

                if (ParentIsTargetNode(parent, nodeID))
                {
                    OpenTree(parent);
                    return;
                }

                for (int j = 0; j < parent.Nodes.Count; j++)
                {
                    bool found = false;
                    CheckChildNodeIDs(parent.Nodes[j],nodeID,out found);
                    if (found)
                        break;
                }
            }
        }


        private bool NodeIsBlankTreeNode(RadTreeNode node)
        {
            return node.Text == string.Empty 
                   && node.Tag == string.Empty 
                   && !node.Enabled;
        }


        private bool ParentIsTargetNode(RadTreeNode parentNode, int targetNodeID)
        {
            var parentNodeTagData = (TreeNodeTagData)parentNode.Tag;
            long parentNodeID = parentNodeTagData.objID;
            return parentNodeID == targetNodeID;
        }


        private void CheckChildNodeIDs(RadTreeNode parentNode, long searchNodeID, out bool found)
        {
            if (GetNodeID(parentNode) == searchNodeID)
            {
                OpenTree(parentNode);
                found = true;
            }
            else
            {
                found = false;
                for (int i = 0; i < parentNode.Nodes.Count; i++)
                {
                    CheckChildNodeIDs(parentNode.Nodes[i], searchNodeID, out found);
                }
            }
        }


        private void OpenTree(RadTreeNode foundNode)
        {
            if (foundNode != null)
            {
                radTreeView1.SelectedNode = foundNode;
                
                if (NodeHasChildren(foundNode))
                {
                    foundNode.Expanded = true;
                }
                
                ExecuteFunction();
            }
        }


        private static bool NodeHasChildren(RadTreeNode node)
        {
            return node.Nodes.Count > 0;
        }


        private void SetupTreeEvents()
        {
            this.radTreeView1.KeyPress -= new KeyPressEventHandler(treeView_KeyPress);
            this.radTreeView1.KeyPress += new KeyPressEventHandler(treeView_KeyPress);
            this.radTreeView1.MouseUp -= new MouseEventHandler(treeView_MouseUp);
            this.radTreeView1.MouseUp += new MouseEventHandler(treeView_MouseUp);
            this.radTreeView1.MouseClick -= new MouseEventHandler(treeView_MouseClick);
            this.radTreeView1.MouseClick += new MouseEventHandler(treeView_MouseClick);
            this.radTreeView1.SelectedNodeChanging -= new Telerik.WinControls.UI.RadTreeView.RadTreeViewCancelEventHandler(this.treeView_SelectedNodeChanging);
            this.radTreeView1.SelectedNodeChanging += new Telerik.WinControls.UI.RadTreeView.RadTreeViewCancelEventHandler(this.treeView_SelectedNodeChanging);
            this.radTreeView1.NodeFormatting -= TreeViewNavigation.TreeViewFormatter.NodeFormatting;
            this.radTreeView1.NodeFormatting += TreeViewNavigation.TreeViewFormatter.NodeFormatting;
        }

        #endregion

        #region TreeView Events & Support Methods

        private void treeView_SelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            if (!nodeAdded)
            {
                if (e.Node != null)
                {
                    if (Convert.ToString(e.Node.Tag) == string.Empty)
                        e.Cancel = true;
                    else
                        backupNode = radTreeView1.SelectedNode;
                }
            }
            nodeAdded = false;
        }


        private void treeView_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                ExecuteFunction();
            }
        }


        private void treeView_MouseClick(object sender, MouseEventArgs e)
        {
            if (CheckUserHasAdminAccess())
            {
                if (this.radTreeView1.GetNodeAt(e.X, e.Y) == null)
                {
                    if (e.Button == MouseButtons.Right)
                        AddParentGroupToTreeView();
                }
            }
        }


        private void treeView_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.radTreeView1.Nodes.Count > 0 && this.radTreeView1.GetNodeAt(e.X, e.Y) != null)
            {
                if (e.Button == MouseButtons.Left)
                {
                    ExecuteFunction();
                }
            }
        }


        private void ExecuteFunction()
        {
            TreeNodeTagData data = (TreeNodeTagData)radTreeView1.SelectedNode.Tag;
            if (data != null && !string.IsNullOrWhiteSpace(data.objCode) && CheckUserHasAccess((TreeNodeTagData)radTreeView1.SelectedNode.Tag))
            {
                Telerik.WinControls.UI.RadTreeNode node = this.radTreeView1.SelectedNode;
                DisplaySelectedFunction((TreeNodeTagData)node.Tag);
                AddToLast10(node);
            }
        }


        private void addparent_MouseUp(object sender, MouseEventArgs e)
        {
            if (CheckUserHasAdminAccess())
            {
                AddParentGroupToTreeView();
            }
        }


        private void AddParentGroupToTreeView()
        {
            FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "consoleID", consoleID }, {"consoleParent", consoleparent} };
            var add = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(GroupWizard, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, kvc);
            if (add != null)
            {
                ResetTreeViewControl();
                SetupTreeView(filter);
            }
        }


        private void editparent_MouseUp(object sender, MouseEventArgs e)
        {
            if (!IsSystemTreeNode(ResourceLookup.GetLookupText("NOEDITTREENODE")) && CheckUserHasAdminAccess())
            {
                FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "ID", GetNodeID() }, {"consoleParent", consoleparent}};
                var edit = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(GroupWizard, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, kvc);
                if (edit != null)
                {
                    ResetTreeViewControl();
                    SetupTreeView(filter);
                }
            }
        }


        private void AddMemberToParent_MouseUp(object sender, MouseEventArgs e)
        {
            if (CheckUserHasAdminAccess())
            {
                FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "consoleID", consoleID }, { "parent", false }, { "parentID", GetNodeID() }, { "consoleParent", consoleparent } };
                var edit = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(NodeWizard, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, kvc);
                if (edit != null)
                {
                    ResetTreeViewControl();
                    SetupTreeView(filter);
                }
            }
        }

 
        private void removeparent_MouseUp(object sender, MouseEventArgs e)
        {
            if (!IsSystemTreeNode(ResourceLookup.GetLookupText("NODELTREENODE")) && CheckUserHasAdminAccess())
            {
                if (swf.MessageBox.Show(ResourceLookup.GetLookupText("DELTNGRPMSG"), ResourceLookup.GetLookupText("MSMSGCAPTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FindNodeToRemove(GetNodeID());
                }
                    
            }
        }


        private void moveparent_MouseUp(object sender, EventArgs e)
        {
            NodeMovementTool(true);
        }


        private void addchild_MouseUp(object sender, EventArgs e)
        {
            if (CheckUserHasAdminAccess())
            {
                FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "consoleID", consoleID }, { "parent", false }, { "parentID", GetParentNodeID() }, { "consoleParent", consoleparent } };
                var add = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(NodeWizard, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, kvc);
                if (add != null)
                {
                    ResetTreeViewControl();
                    SetupTreeView(filter);
                }
            }
        }


        private void editchild_MouseUp(object sender, MouseEventArgs e)
        {
            if (!IsSystemTreeNode(ResourceLookup.GetLookupText("NOEDITTREENODE")) && CheckUserHasAdminAccess())
            {
                FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "consoleID", consoleID }, { "parent", false }, { "ID", GetNodeID() }, { "consoleParent", consoleparent } };
                var edit = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(NodeWizard, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, kvc);
                if (edit != null)
                {
                    ResetTreeViewControl();
                    SetupTreeView(filter);
                }
            }
        }


        private void removechild_MouseUp(object sender, EventArgs e)
        {
            if (!IsSystemTreeNode(ResourceLookup.GetLookupText("NODELTREENODE")) && CheckUserHasAdminAccess())
            {
                if (swf.MessageBox.Show(ResourceLookup.GetLookupText("DELTVCHILDMSG"), ResourceLookup.GetLookupText("MSMSGCAPTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    long nodeID = GetNodeID();
                    RunDataList(constDeleteChild, new FWBS.Common.KeyValueCollection() { { "ID", nodeID }});
                    FindNodeToRemove(nodeID);
                    RemoveFromLast10AndFavourites(nodeID);
                }
            }
        }


        private void addchildtofav_MouseUp(object sender, EventArgs e)
        {
            TreeNodeTagData tag = (TreeNodeTagData)radTreeView1.SelectedNode.Tag;
            FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(this.frmMain.frmMenu1.ucHome.MenuCode + "FAV");
            if (fav.Count < 10)
            {
                if (!CheckForExistingFavourite(tag.objID))
                {
                    fav.AddFavourite(tag.objCode, "1", new string[] { Convert.ToString(tag.objID) + ";" + Convert.ToString(consoleID), tag.objDesc, tag.objRoles, "0" });
                    fav.Update();
                    swf.MessageBox.Show(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", "AKMYFAVADDMSG"), FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", "AKMYFAVCAP"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                swf.MessageBox.Show(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", "AKMYFAVMAXMSG"), FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", "AKMYFAVCAP"), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void movechildnode_MouseUp(object sender, EventArgs e)
        {
            NodeMovementTool(false);
        }


        private void NodeMovementTool(bool allowAddToConsole)
        {
            TreeNodeTagData tag = (TreeNodeTagData)this.radTreeView1.SelectedNode.Tag;
            if (!ConvertDef.ToBoolean(tag.objSystem,false))
                ActivateNodeMovementTool(tag, allowAddToConsole);
            else
                swf.MessageBox.Show(FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "TNNOMOVE"), FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "NODEMOVEDCAP"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void ActivateNodeMovementTool(TreeNodeTagData tag, Boolean allowAddToSection)
        {
            ucAdminKitNodeMover mover = new ucAdminKitNodeMover(allowAddToSection, consoleparent);
            var result = mover.ShowDialog(parent);
            if (result == DialogResult.OK)
                ApplyNodeMovement(mover, tag.objID);
        }


        private void ApplyNodeMovement(ucAdminKitNodeMover mover, long moverID)
        {
            long id = 0;
            if (!mover.AddToSection)
                id = mover.SelectedNodeID;
            else
                id = mover.sectionID;

            ImplementNodeMovement(mover, moverID, id);
        }


        private void ImplementNodeMovement(ucAdminKitNodeMover mover, long moverID, long id)
        {
            System.Data.DataTable dtID = RunDataList("DMoveNode", new FWBS.Common.KeyValueCollection() { { "moverID", moverID }, { "newParent", id } });

            RunDataList("DUpdateAKFavs", new KeyValueCollection() { { "group", NodeHasChildren(radTreeView1.SelectedNode) }, { "nodeID", moverID }, { "consoleID", mover.sectionID }, { "usrID", FWBS.OMS.Session.CurrentSession.CurrentUser.ID } });
            mover.Dispose();
            FindNodeToMove(moverID);
            swf.MessageBox.Show(FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "NODEMOVED"), FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "NODEMOVEDCAP"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        public void FindNodeToMove(long moverNodeID)
        {
            RadTreeNode parent;

            for (int i = 0; i < radTreeView1.Nodes.Count; i++)
            {
                parent = radTreeView1.Nodes[i];

                long parentID = GetNodeID(parent);
                if (parentID == moverNodeID)
                {
                    parent.Remove();
                }
                else
                {
                    for (int j = 0; j < parent.Nodes.Count; j++)
                    {
                        bool found = false;
                        CheckChildNodesToMove(parent.Nodes[j], moverNodeID, out found);
                        if (found)
                            break;
                    }
                }
            }
        }


        private void CheckChildNodesToMove(RadTreeNode Node, long searchNodeID, out bool found)
        {
            if (GetNodeID(Node) == searchNodeID)
            {
                Node.Remove();
                found = true;
            }
            else
            {
                found = false;
                for (int i = 0; i < Node.Nodes.Count; i++)
                {
                    CheckChildNodes(Node.Nodes[i], searchNodeID, out found);
                }
            }
        }


        internal void FindNodeToRemove(long searchnodeID)
        {
            RadTreeNode parent;

            for (int i = 0; i < radTreeView1.Nodes.Count; i++)
            {
                long topNodeID = GetNodeID(radTreeView1.Nodes[i]);
                if (topNodeID == searchnodeID)
                {
                    TrimTree(radTreeView1.Nodes[i].Nodes);
                    radTreeView1.Nodes[i].Remove();
                    RunDataList(constDeleteParent, new FWBS.Common.KeyValueCollection() { { "ID", topNodeID } });
                }
                else
                {
                    parent = radTreeView1.Nodes[i];
                    for (int j = 0; j < parent.Nodes.Count; j++)
                    {
                        bool found = false;
                        CheckChildNodes(parent.Nodes[j], searchnodeID, out found);
                        if (found)
                            break;
                    }
                }
            }
        }


        private void CheckChildNodes(RadTreeNode Node, long searchNodeID, out bool found)
        {
            if (GetNodeID(Node) == searchNodeID)
            {
                TrimTree(Node.Nodes);
                Node.Remove();
                found = true;
            }
            else
            {
                found = false;
                for (int i = 0; i < Node.Nodes.Count; i++)
                {
                    CheckChildNodes(Node.Nodes[i], searchNodeID, out found);
                }
            }
        }


        private void TrimTree(RadTreeNodeCollection nodes)
        {
            RadTreeNode node = null;
            for (int i = nodes.Count; i > 0; i--)
            {
                node = nodes[i - 1];
                TrimTree(node.Nodes);
                if (node.Nodes.Count == 0)
                {
                    long nodeID = GetNodeID(node);
                    nodes.Remove(node);
                    RunDataList(constDeleteParent, new FWBS.Common.KeyValueCollection() { { "ID", nodeID } });
                    RemoveFromLast10AndFavourites(nodeID);
                }
            }
        }


        private long GetNodeID()
        {
            TreeNodeTagData tag = (TreeNodeTagData)this.radTreeView1.SelectedNode.Tag;
            return tag.objID;
        }

        
        private long GetNodeID(RadTreeNode node)
        {
            if (node.Tag != string.Empty)
            {
                TreeNodeTagData tag = (TreeNodeTagData)node.Tag;
                return tag.objID;
            }
            else
            {
                return -1;
            }
        }
        

        private long GetParentNodeID()
        {
            RadTreeNode parent = radTreeView1.SelectedNode.Parent;
            TreeNodeTagData tag = (TreeNodeTagData)parent.Tag;
            return tag.objID;
        }


        private void ResetTreeViewControl()
        {
            nodeAdded = true;
            radTreeView1.Nodes.Clear();
        }


        private bool IsSystemTreeNode(string msg)
        {
            bool result = true;
            if (radTreeView1.SelectedNode != null)
            {
                TreeNodeTagData tagData = (TreeNodeTagData)radTreeView1.SelectedNode.Tag;
                if (Convert.ToBoolean(tagData.objSystem))
                {
                    swf.MessageBox.Show(msg, ResourceLookup.GetLookupText("MSMSGCAPTION"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }


        private bool CheckForExistingFavourite(long nodeID)
        {
            KeyValueCollection kvc = new KeyValueCollection() {{"nodeID",nodeID}};
            DataTable dt = RunDataList(constCheckAKFavs, kvc);
            if (dt != null & dt.Rows.Count > 0)
            {
                swf.MessageBox.Show(ResourceLookup.GetLookupText("AKNOADDTOFAV"), ResourceLookup.GetLookupText("ADMIN"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            else
                return false;
        }


        #endregion

        #region Function Display Methods

        private void DisplaySelectedFunction(TreeNodeTagData tag)
        {
            string obj = "";
            string cmd = tag.objCode;
            if (cmd.IndexOf("|") > -1)
            {
                obj = cmd.Substring(cmd.IndexOf("|") + 1);
                cmd = cmd.Substring(0, cmd.IndexOf("|"));
            }
            if (CheckDisplayObjectIsDirty())
                return;

            this.frmMain.OnfrmMainClosing();
            ResetPanelVariables();

            var _type = frmMain.ConstructAdminElement(obj, panel2, cmd);
            var console = _type as ucOMSAdminKitConsole;
            if (console != null)
            {
                this.ConstructTree(obj);
                return;
            }

            if (_type == null)
            {
                var result = false;
                frmMain.MacroCommands(cmd, obj, out result);
                if (result)
                    return;

                Type edit = Session.CurrentSession.TypeManager.TryLoad(cmd);
                if (edit != null)
                {
                    ucEditBase2 ctrl = (ucEditBase2)edit.InvokeMember(String.Empty, System.Reflection.BindingFlags.CreateInstance, null, null, null);
                    DetermineDisplayMethod(ctrl);
                    return;
                }

                DisplaySearchList(cmd);
                return;
            }

            DetermineDisplayMethod(_type);

            _type.Dock = DockStyle.Fill;
            _type.BringToFront();
        }


        private bool CheckDisplayObjectIsDirty()
        {
            if (EditBaseIsDirty())
                return true;
            if (EditBase2IsDirty())
                return true;
            if (SearchListIsDirty())
                return true;
            return false;
        }


        private bool EditBaseIsDirty()
        {
            return (parent.editbase != null && !parent.editbase.IsObjectDirty());
        }


        private bool EditBase2IsDirty()
        {
            return (parent.editbase2 != null && !parent.editbase2.IsObjectDirty());
        }


        private bool SearchListIsDirty()
        {
            if (this.panel2.Controls.Count > 0)
            {
                var searchlist = this.panel2.Controls[0] as ucSearchControl;
                if (searchlist != null)
                    return (!this.IsObjectDirty(searchlist));
            }
            return false;
        }


        private void DetermineDisplayMethod(Control _type)
        {
            var ucEditBase2 = _type as ucEditBase2;
            if (ucEditBase2 != null)
                DisplayTypeAsUcEditBase2(ucEditBase2);

            var ucEditBase = _type as ucEditBase;
            if (ucEditBase != null)
                DisplayTypeAsUcEditBase(ucEditBase);
            
            if (ucEditBase == null && ucEditBase2 == null)
                this.panel2.Controls.Add(_type);
        }


        private void DisplayTypeAsUcEditBase(Admin.ucEditBase ucEditBase)
        {
            parent.editbase = ucEditBase;
            this.panel2.Controls.Add(ucEditBase.tpList);
        }


        private void DisplayTypeAsUcEditBase2(ucEditBase2 _type)
        {
            var screen = _type as ucScreen;
            if(screen != null)
            {
                DisplayScreen(screen);
            }
            else
            {
                parent.editbase2 = _type;
                this.panel2.Controls.Add(_type.tpList);
            }
        }


        private void ResetPanelVariables()
        {
            if (panel2.Controls.Count > 0)
            {
                var disposeable = this.panel2.Controls[0] as IDisposable;
                if (disposeable != null)
                {
                    disposeable.Dispose();
                }
            }
            ClearEnquiryForm(this.panel2.Controls);
            this.panel2.Controls.Clear();
            parent.editbase = null;
            parent.editbase2 = null;
        }


        private void ClearEnquiryForm(ControlCollection collection)
        {
            foreach (Control c in collection)
            {
                var enquiryForm = c as EnquiryForm;
                if (enquiryForm != null)
                    enquiryForm.Dispose();
                if (c.Controls.Count > 0)
                    ClearEnquiryForm(c.Controls);
            }
        }


        public bool IsObjectDirty(ucSearchControl search)
        {
            if (search.IsDirty)
            {
                DialogResult dr = System.Windows.Forms.MessageBox.Show(parent, Session.CurrentSession.Resources.GetMessage("DIRTYDATAMSG", "Changes have been detected, would you like to save?", "").Text, "OMS Admin", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        search.UpdateItem();
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ParentForm, ex);
                        return false;
                    }
                }
                if (dr == DialogResult.Cancel) return false;
            }
            return true;
        }


        private void DisplayScreen(ucScreen screen)
        {
            ResetPanelVariables();
            this.panel2.Controls.Add(screen);
            screen.BringToFront();
            panel2.Focus();
        }


        private void DisplaySearchList(string objCode)
        {
            ResetPanelVariables();
            ucSearchControl search = new ucSearchControl();
            this.panel2.Controls.Add(search);
            search.SetSearchList(objCode, null, null);
            search.ShowPanelButtons();
            search.Dock = DockStyle.Fill;
            search.BringToFront();
            search.Search();
        }


        #endregion

        #region Last 10 & Favourites

        public void AddToLast10(RadTreeNode node)
        {
            string favType = this.frmMain.frmMenu1.ucHome.MenuCode + "LAST10";
            TreeNodeTagData tag = (TreeNodeTagData)node.Tag;
            FWBS.OMS.Favourites last10 = new Favourites(favType);
            DataView dv = null;
            try
            {
                dv = last10.GetDataView();
                IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
                List<IDataParameter> parList = new List<IDataParameter>();
                parList.Add(connection.CreateParameter("objID", Convert.ToString(tag.objID) + ";" + Convert.ToString(consoleID)));
                parList.Add(connection.CreateParameter("objDesc", tag.objDesc));
                parList.Add(connection.CreateParameter("favType", favType));
                parList.Add(connection.CreateParameter("userID", Session.CurrentSession.CurrentUser.ID));
                System.Data.DataTable check = connection.ExecuteProcedure("CheckAdminKitLast10", parList);
   
                if(Convert.ToString(check.Rows[0]["result"]) == "0")
                {
                    last10.AddFavourite(tag.objCode, null, new string[] { Convert.ToString(tag.objID) + ";" + Convert.ToString(consoleID), tag.objDesc, tag.objRoles, "-1" });
                }

                dv.Sort = "usrFavObjParam4";
                for (int i = dv.Count - 1; i >= 0; i--)
                {
                    dv[i]["usrFavObjParam4"] = i;
                }
                if (dv.Count > 10)
                {
                    for (int i = dv.Count - 1; i >= 10; i--)
                    {
                        dv.Delete(i);
                    }
                }
                last10.Update();
            }
            catch (Exception e)
            {
                ErrorBox.Show(ParentForm, e);
            }
        }


        private void RemoveFromList(long nodeID, string favType)
        {
            FWBS.OMS.Favourites list = new Favourites(favType);
            DataView dv = null;
            try
            {
                dv = list.GetDataView();
                for (int i = dv.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToString(dv[i]["usrFavObjParam1"]).StartsWith(Convert.ToString(nodeID).Trim() + ";"))
                    {
                        dv.Delete(i);
                    }
                }
                list.Update();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }


        private void RemoveFromLast10AndFavourites(long nodeID)
        {
            RunDataList(constRemoveFromFav, new FWBS.Common.KeyValueCollection() {
                                            { "nodeID", Convert.ToString(nodeID) },
                                            { "usrID", FWBS.OMS.Session.CurrentSession.CurrentUser.ID },
                                            { "type", consoleparent } });
            RemoveRelevantEntries(nodeID);
        }


        private void RemoveRelevantEntries(long nodeID)
        {
            if (consoleparent == "ADMIN")
            {
                RemoveFromList(nodeID, "ADMINLAST10");
                RemoveFromList(nodeID, "ADMINFAV");
            }
            else
            {
                RemoveFromList(nodeID, "REPORTSLAST10");
                RemoveFromList(nodeID, "REPORTSFAV");
            }
            Session.CurrentSession.ClearCache();
        }

        #endregion

        #region General Infrastructure


        private void CreateWelcomePage(string filter)
        {
            Label lblWlecome1 = new Label();
            lblWlecome1.AutoSize = true;
            lblWlecome1.BackColor = System.Drawing.Color.WhiteSmoke;
            lblWlecome1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblWlecome1.Location = new System.Drawing.Point(5, 9);
            lblWlecome1.Name = "label1";
            lblWlecome1.Size = new System.Drawing.Size(256, 15);
            lblWlecome1.TabIndex = 0;

            Label lblWlecome2 = new Label();
            lblWlecome2.AutoSize = true;
            lblWlecome2.BackColor = System.Drawing.Color.WhiteSmoke;
            lblWlecome2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            lblWlecome2.Location = new System.Drawing.Point(5, 36);
            lblWlecome2.Name = "label2";
            lblWlecome2.Size = new System.Drawing.Size(637, 15);
            lblWlecome2.TabIndex = 1;

            Panel pnlWlecome = new Panel();
            pnlWlecome.BackColor = System.Drawing.Color.WhiteSmoke;
            pnlWlecome.Controls.Add(lblWlecome2);
            pnlWlecome.Controls.Add(lblWlecome1);
            pnlWlecome.Dock = System.Windows.Forms.DockStyle.Top;
            pnlWlecome.Location = new System.Drawing.Point(0, 0);
            pnlWlecome.Name = "panel3";
            pnlWlecome.Size = new System.Drawing.Size(1249, 114);
            pnlWlecome.TabIndex = 0;

            DataRow[] foundRow = dtNodes.Select("SUBSTRING(admnuSearchListCode,1,3) LIKE 'AKC%' AND admnuSearchListCode LIKE '%" + filter + "'");
            lblWlecome1.Text = FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", "WELCOMETO")  
                                + " " + FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", Convert.ToString(foundRow[0]["AdmnuCode"]));
            lblWlecome2.Text = FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", "NAVPANELADVICE");

            this.panel2.Controls.Add(pnlWlecome);
        }


        private bool CheckUserHasAdminAccess()
        {
            if (IsInRoles("ADMIN", FWBS.OMS.Session.CurrentSession.CurrentUser))
                return true;
            else
                return false;
        }


        private bool CheckUserHasAccess(TreeNodeTagData tag)
        {
            bool result = false;
            string[] nodeRoles = Convert.ToString(tag.objRoles).Split(',');
            if(nodeRoles.Length > 0)
            {
                foreach (string role in nodeRoles)
                {
                    if (IsInRoles(role, FWBS.OMS.Session.CurrentSession.CurrentUser))
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                result = true;
            }
            if(!result)
                swf.MessageBox.Show(ResourceLookup.GetLookupText("NOTNODEACCESS"),ResourceLookup.GetLookupText("MSMSGCAPTION"),MessageBoxButtons.OK,MessageBoxIcon.Stop);
            return result;
        }


        private bool CheckUserHasAccessToNodeAtCreation(DataRow r)
        {
            bool result = false;
            string[] nodeRoles = Convert.ToString(r["admnuRoles"]).Split(',');
            if (nodeRoles.Length > 0)
            {
                foreach (string role in nodeRoles)
                {
                    if (IsInRoles(role, FWBS.OMS.Session.CurrentSession.CurrentUser))
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                result = true;
            }
            return result;
        }


        private bool IsInRoles(string role, FWBS.OMS.User u)
        {
            if (string.IsNullOrEmpty(role))
                return true;

            string[] vals = u.Roles.Split(',');
            return Array.IndexOf(vals, role) > -1;
        }


        private bool PreventSDKAccess(string code)
        {
            bool fullsdk = false;
            bool preventaccess = true;
            try
            {
                if (code == constFullSDK || code == constReportSDK || code == constSystemScripts || code == constExtendedDataSDK)
                {
                    CheckForLicensing(ref preventaccess, "SDKALL", "Full SDK Access Granted", "3E MatterSphere Framework SDK");
                    fullsdk = !preventaccess;
                }
                if (code == constExtendedDataSDK && !fullsdk)
                {
                    CheckForLicensing(ref preventaccess, "SDKEXD", "Extended SDK Access Granted", "Extended Data SDK");
                }
                if (code != constFullSDK && code != constReportSDK && code != constSystemScripts && code != constExtendedDataSDK)
                {
                    preventaccess = false;
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
            return preventaccess;
        }


        private void CheckForLicensing(ref bool preventaccess, string licenseCode, string LogMessage, string licensemissing)
        {
            if (!Session.CurrentSession.IsLicensedFor(licenseCode))
            {
                if (!frmMain.sdkaccess && !sdklicensedialogshown)
                    ActivatePasswordRequestDialog(ref preventaccess, LogMessage, licensemissing);
                
                if (frmMain.sdkaccess)
                    preventaccess = false;
            }
            else
                preventaccess = false;
        }


        private void ActivatePasswordRequestDialog(ref bool preventaccess, string logmessage, string licensemissing)
        {
            using (FWBS.OMS.UI.Windows.Admin.frmPasswordRequest pas = new FWBS.OMS.UI.Windows.Admin.frmPasswordRequest(licensemissing))
            {
                var check = pas.ShowDialog();
                sdklicensedialogshown = true;
                if (check != DialogResult.Cancel)
                    CreateCaptainsLogEntry(ref preventaccess, logmessage);
            }
        }


        private void CreateCaptainsLogEntry(ref bool preventaccess, string logmessage)
        {
            FWBS.OMS.Logging.CaptainsLog.CreateEntry(8, logmessage, null, "", false);
            frmMain.sdkaccess = true;
            preventaccess = false;
        }


        private bool CheckForScreenDesignerLicense()
        {
            return Session.CurrentSession.IsLicensedFor("SDKSCD");
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


        private void SetFont()
        {
            this.radTreeView1.Font = new Font(CurrentUIVersion.Font, CurrentUIVersion.FontSize);
        }

 
        #endregion


        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            ResetPanelVariables();
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



    }

    #region Tree Node Tag Data Class

    public partial class TreeNodeTagData
    {
        public TreeNodeTagData(long objID, string objDesc, string objectCode, bool objSystem, string objRoles)
        {
            this.objID = objID;
            this.objDesc = objDesc;
            this.objCode = objectCode;
            this.objSystem = objSystem;
            this.objRoles = objRoles;
        }
        
  
        internal long objID { get; set; }
        internal string objDesc { get; set; }
        internal string objCode { get; set; }
        internal bool objSystem { get; set; }
        internal string objRoles { get; set; }
    }

    #endregion

}