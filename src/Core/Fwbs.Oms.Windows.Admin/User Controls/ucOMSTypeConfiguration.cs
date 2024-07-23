using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.Data;
using Telerik.WinControls.UI;
using swf = System.Windows.Forms;


namespace FWBS.OMS.UI.Windows.Admin
{
    public partial class ucOMSTypeConfiguration : UserControl
    {
        bool nodeAdded = false;
        ArrayList FirstNodeTabCodes = new ArrayList();
        private frmMain frmMain;
        private string filter;
        Telerik.WinControls.UI.RadContextMenu parentMenu = new Telerik.WinControls.UI.RadContextMenu();
        Telerik.WinControls.UI.RadContextMenu childMenu = new Telerik.WinControls.UI.RadContextMenu();

        const string CannotRemoveTreeNodeMsg = "You cannot remove the selected item as it is part of the core 3E MatterSphere system.\n\nYou can only remove custom elements.";
        const string CannotRemoveTreeNodeMsgHeader = "System Functionality";
        const string CannotEditTreeNodeMsg = "You cannot edit the selected item as it is part of the core 3E MatterSphere system.\n\nYou can only edit custom elements.";



        public ucOMSTypeConfiguration(string Filter)
        {
            filter = Filter;
            InitializeComponent();
            SetupContextMenus();
            SetupTreeView(filter);
            this.Dock = DockStyle.Fill;
        }


        public ucOMSTypeConfiguration(frmMain frmMain, string Filter) : this(Filter)
        {
            filter = Filter;
            this.frmMain = frmMain;
        }

        #region TreeView Setup (Infrastructure)

        private void SetupTreeView(string filter)
        {
            System.Data.DataTable dtNodes = GetTreeNodeData(filter);
            GetParentNodes(dtNodes);
            SetupTreeEvents();
        }


        private System.Data.DataTable GetTreeNodeData(string filter)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("filter", filter));
            System.Data.DataTable dt = connection.ExecuteProcedure("sprAdminTreeNodeData", parList);
            return dt;
        }


        private void SetupContextMenus()
        {
            SetupParentContextMenu();
            SetupChildContextMenu();
        }


        private void SetupParentContextMenu()
        {
            Telerik.WinControls.UI.RadMenuItem addparent = new Telerik.WinControls.UI.RadMenuItem();
            addparent.Text = "Add New Group";
            addparent.MouseDown += new MouseEventHandler(addparent_MouseDown);

            Telerik.WinControls.UI.RadMenuItem editparent = new Telerik.WinControls.UI.RadMenuItem();
            editparent.Text = "Edit Group";
            editparent.MouseDown += new MouseEventHandler(editparent_MouseDown);

            Telerik.WinControls.UI.RadMenuItem AddMemberToParent = new Telerik.WinControls.UI.RadMenuItem();
            AddMemberToParent.Text = "Add New Group Member";
            AddMemberToParent.MouseDown += new MouseEventHandler(AddMemberToParent_MouseDown);


            Telerik.WinControls.UI.RadMenuItem removeparent = new Telerik.WinControls.UI.RadMenuItem();
            removeparent.Text = "Remove Group";
            removeparent.MouseDown += new MouseEventHandler(removeparent_MouseDown);               
            
            parentMenu.Items.Add(addparent);
            parentMenu.Items.Add(editparent);
            parentMenu.Items.Add(AddMemberToParent);
            parentMenu.Items.Add(removeparent);
        }


        private void SetupChildContextMenu()
        {
            Telerik.WinControls.UI.RadMenuItem addchild = new Telerik.WinControls.UI.RadMenuItem();
            addchild.Text = "Add New Group Member";
            addchild.MouseDown += new MouseEventHandler(addchild_MouseDown);

            Telerik.WinControls.UI.RadMenuItem editchild = new Telerik.WinControls.UI.RadMenuItem();
            editchild.Text = "Edit Group Member";
            editchild.MouseDown += new MouseEventHandler(editchild_MouseDown);

            Telerik.WinControls.UI.RadMenuItem removechild = new Telerik.WinControls.UI.RadMenuItem();
            removechild.Text = "Remove Group Member";
            removechild.MouseDown += new MouseEventHandler(removechild_MouseDown);

            childMenu.Items.Add(addchild);
            childMenu.Items.Add(editchild);
            childMenu.Items.Add(removechild);
        }

        #endregion

        #region Tree Element Creation

        private void GetParentNodes(System.Data.DataTable dt)
        {
            DataRow[] parents = dt.Select("tvParent = 1");
            for (int i = 0; i < parents.Length; i++)
            {
                CreateParentTreeNode(dt, parents[i]);
            }
        }


        private DataRow[] GetChildNodes(System.Data.DataTable dt, long parentID)
        {
            DataRow[] foundRows = dt.Select("tvNodeParentID = " + parentID, "tvNodePosition ASC");
            return foundRows;
        }


        private void CreateParentTreeNode(System.Data.DataTable dt, DataRow r)
        {
            RadTreeNode parent = CreateParentNode(r);
            DataRow[] children = GetChildNodes(dt, Convert.ToInt64(r["ID"]));
            if (children.Length > 0)
            {
                for (int i = 0; i < children.Length; i++)
                {
                    CreateChildTreeNode(children[i], parent);
                }
                AddBlankTreeNode();
            }
        }


        private RadTreeNode CreateParentNode(DataRow r)
        {
            RadTreeNode node = new RadTreeNode();
            node.Text = FWBS.OMS.CodeLookup.GetLookup("ADMTVNODES", Convert.ToString(r["tvNodeDesc"]));
            node.Tag = CreateTreeNodeTag(r);
            node.ContextMenu = parentMenu;
            this.radTreeView1.Nodes.Add(node);
            return node;
        }


        private void CreateChildTreeNode(DataRow r, RadTreeNode parent)
        {
            this.radTreeView1.SelectedNode = parent;
            RadTreeNode node = new RadTreeNode();
            node.ForeColor = System.Drawing.Color.Black;
            node.Text = FWBS.OMS.CodeLookup.GetLookup("ADMTVNODES", Convert.ToString(r["tvNodeDesc"]));
            node.Tag = CreateTreeNodeTag(r);
            node.ContextMenu = childMenu;
            this.radTreeView1.SelectedNode.Nodes.Add(node);
        }


        private OTCTreeNodeTagData CreateTreeNodeTag(DataRow r)
        {
            OTCTreeNodeTagData tag = new OTCTreeNodeTagData(Convert.ToInt64(r["ID"]), Convert.ToString(r["tvNodeObjCode"]),
                                      Convert.ToString(r["tvNodeObjCategory"]), Convert.ToString(r["tvNodeObjType"]), Convert.ToBoolean(r["tvNodeSystem"]));
            return tag;
        }


        private void AddBlankTreeNode()
        {
            RadTreeNode node = new RadTreeNode();
            node.Text = string.Empty;
            node.Enabled = false;
            node.Tag = string.Empty;
            this.radTreeView1.Nodes.Add(node);
        }


        private void SetupTreeEvents()
        {
            this.radTreeView1.MouseUp -= new MouseEventHandler(treeNavigation_MouseUp);
            this.radTreeView1.MouseUp += new MouseEventHandler(treeNavigation_MouseUp);
            this.radTreeView1.MouseDown -= new MouseEventHandler(treeNavigation_MouseDown);
            this.radTreeView1.MouseDown += new MouseEventHandler(treeNavigation_MouseDown);
            this.radTreeView1.SelectedNodeChanging -= new Telerik.WinControls.UI.RadTreeView.RadTreeViewCancelEventHandler(this.treeNavigation_SelectedNodeChanging);
            this.radTreeView1.SelectedNodeChanging += new Telerik.WinControls.UI.RadTreeView.RadTreeViewCancelEventHandler(this.treeNavigation_SelectedNodeChanging);
        }

        #endregion

        #region TreeView Events

        private void treeNavigation_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.radTreeView1.Nodes.Count > 0)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (radTreeView1.SelectedNode.Tag != null)
                    {
                        Telerik.WinControls.UI.RadTreeNode node = this.radTreeView1.SelectedNode;
                        ChangeEnquiryForm((OTCTreeNodeTagData)node.Tag);
                    }
                }
            }
        }


        private void treeNavigation_SelectedNodeChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            if (!nodeAdded)
            {
                if (radTreeView1 != null)
                {
                    if (radTreeView1.Nodes != null && radTreeView1.Nodes.Count > 0)
                    {
                        if (string.IsNullOrWhiteSpace(e.Node.Text))
                        {
                            e.Cancel = true;
                        }
                    }
                }
            }
            nodeAdded = false;
        }


        private void treeNavigation_MouseDown(object sender, MouseEventArgs e)
        {
            if(this.radTreeView1.Nodes.Count == 0)
            {
                if(e.Button == MouseButtons.Right)
                {
                    if (swf.MessageBox.Show(Session.CurrentSession.Resources.GetResource("ADDGRTOTREE", "Would you like to add a group to the tree?", "").Text, Session.CurrentSession.Resources.GetResource("EMPTYTREE", "Empty Tree", "").Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        AddParentGroupToTreeView();
                    }
                }
            }
        }


        private void addparent_MouseDown(object sender, MouseEventArgs e)
        {
            AddParentGroupToTreeView();
        }


        private void AddParentGroupToTreeView()
        {
            FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "filter", filter }, { "parent", true } };
            var add = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("sADDTREENODE", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, kvc);
            if (add != null)
                ControlTreeViewReset();
        }


        private void editparent_MouseDown(object sender, MouseEventArgs e)
        {
            if (!IsSystemTreeNode(CannotEditTreeNodeMsg))
            {
                FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "filter", filter }, { "parent", true }, { "ID", GetNodeID() } };
                var edit = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("sADDTREENODE", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, kvc);
                if (edit != null)
                    ControlTreeViewReset();
            }
        }


        private void AddMemberToParent_MouseDown(object sender, MouseEventArgs e)
        {
            FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "filter", filter }, { "parent", false }, {"parentID", GetNodeID() } };
            var edit = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("sADDTREENODE", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, kvc);
            if (edit != null)
                ControlTreeViewReset();
        }

 
        private void removeparent_MouseDown(object sender, MouseEventArgs e)
        {
            if (!IsSystemTreeNode(CannotRemoveTreeNodeMsg))
            {
                if (swf.MessageBox.Show(Session.CurrentSession.Resources.GetResource("DELETEGRMSG", "Deleting a group will remove all elements within the group. Are you sure you wish to continue?", "").Text, Session.CurrentSession.Resources.GetResource("DELETEGROUP", "Delete Group", "").Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { {"ID", GetNodeID() } };
                    RunDataList("DDELPTVNODE", kvc);
                    ControlTreeViewReset();               
                }
                    
            }
        }


        private void addchild_MouseDown(object sender, EventArgs e)
        {
            FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "filter", filter }, { "parentNode", false }, {"parentID", GetParentNodeID() } };
            var add = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("sADDTREENODE", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, kvc);
            if (add != null)
                ControlTreeViewReset();
        }


        private void editchild_MouseDown(object sender, MouseEventArgs e)
        {
            if (!IsSystemTreeNode(CannotEditTreeNodeMsg))
            {
                FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "filter", filter }, { "parentNode", false }, { "ID", GetNodeID() } };
                var edit = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("sADDTREENODE", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, kvc);
                if (edit != null)
                    ControlTreeViewReset();
            }
        }


        private void removechild_MouseDown(object sender, EventArgs e)
        {
            if (!IsSystemTreeNode(CannotRemoveTreeNodeMsg))
            {
                if (swf.MessageBox.Show(Session.CurrentSession.Resources.GetResource("DELMEMBERMSG", "Are you sure you wish to delete the selected element?", "").Text, Session.CurrentSession.Resources.GetResource("DELETEMEMBER", "Delete Group Member", "").Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "ID", GetNodeID() } };
                    RunDataList("DDELTVCNODE", kvc);
                    ControlTreeViewReset();
                }
                    
            }
        }


        private long GetNodeID()
        {
            OTCTreeNodeTagData tag = (OTCTreeNodeTagData)this.radTreeView1.SelectedNode.Tag;
            return tag.objID;
        }

        
        private long GetParentNodeID()
        {
            RadTreeNode parent = radTreeView1.SelectedNode.Parent;
            OTCTreeNodeTagData tag = (OTCTreeNodeTagData)parent.Tag;
            return tag.objID;
        }


        private void ControlTreeViewReset()
        {
            nodeAdded = true;
            radTreeView1.Nodes.Clear();
            SetupTreeView(filter);
        }


        private bool IsSystemTreeNode(string msg)
        {
            bool result = true;
            if (radTreeView1.SelectedNode != null)
            {
                OTCTreeNodeTagData tagData = (OTCTreeNodeTagData)radTreeView1.SelectedNode.Tag;
                if (Convert.ToBoolean(tagData.objSystem))
                {
                    swf.MessageBox.Show(msg, CannotRemoveTreeNodeMsgHeader, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            return result;
        }


        #endregion

        #region Item Display Methods

        private void ChangeEnquiryForm(OTCTreeNodeTagData tag)
        {
            switch(tag.objCategory)
            {
                case "Form":
                    DisplayEnquiryForm(tag.objCode);
                    break;
                case "List":

                    break;
                case "OMSType":
                    DisplayOMSType(tag.objType);
                    break;
                case "EditWizard":
                    FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, null);
                    break;
                case "AddWizard":
                    FWBS.OMS.UI.Windows.Services.Wizards.GetWizard("", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, null);
                    break;
            }
        }

        private void DisplayOMSType(string typeName)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            FWBS.Common.KeyValueCollection Params = new FWBS.Common.KeyValueCollection();
            Params.Add("Type", typeName);
            FWBS.OMS.UI.Windows.Admin.ucOMSType _type = new FWBS.OMS.UI.Windows.Admin.ucOMSType(this.frmMain, this.splitContainer1.Panel2, Params);
            _type.Dock = DockStyle.Fill;
            _type.Initialise(frmMain, this.splitContainer1.Panel2, new FWBS.Common.KeyValueCollection());
            _type.tpList.BringToFront();
            this.splitContainer1.Panel2.Controls.Add(_type);
            _type.BringToFront();
        }


        private void DisplayEnquiryForm(string objCode)
        {
            this.splitContainer1.Panel2.Controls.Clear();
            EnquiryForm enquiryForm1 = new EnquiryForm();
            this.splitContainer1.Panel2.Controls.Add(enquiryForm1);
            enquiryForm1.Dock = DockStyle.Fill;
            enquiryForm1.Enquiry = FWBS.OMS.EnquiryEngine.Enquiry.GetEnquiry(objCode, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, null);
            enquiryForm1.BringToFront();
        }

        #endregion

        #region General Infrastructure

        protected System.Data.DataTable RunDataList(string dataList, FWBS.Common.KeyValueCollection kvc)
        {
            FWBS.OMS.EnquiryEngine.DataLists dl = new FWBS.OMS.EnquiryEngine.DataLists(dataList);
            dl.ChangeParameters(kvc);
            System.Data.DataTable dt = dl.Run(false) as System.Data.DataTable;
            return dt;
        }

        #endregion


    }





    internal partial class OTCTreeNodeTagData
    {
        public OTCTreeNodeTagData(long objID, string objectCode, string objectCategory, string objectType, bool objSystem)
        {
            this.objID = objID;
            this.objCode = objectCode;
            this.objCategory = objectCategory;
            this.objType = objectType;
            this.objSystem = objSystem;
        }

        internal string objCode { get; set; }
        internal string objCategory { get; set; }
        internal string objType { get; set; }
        internal long objID { get; set; }
        internal bool objSystem { get; set; }
    }
}