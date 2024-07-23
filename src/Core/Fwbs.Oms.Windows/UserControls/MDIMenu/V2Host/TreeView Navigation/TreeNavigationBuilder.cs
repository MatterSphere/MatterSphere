using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.Data;
using FWBS.OMS.UI.Windows.Admin;
using Infragistics.Win.UltraWinTabControl;
using Telerik.WinControls.UI;
using swf = System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    internal class TreeNavigationBuilder
	{
		private RadTreeView radTreeView1;
		private UltraTabControl tabcontrol;
		private TreeNavigationActions actions;
		int consoleID;
		bool nodeAdded = false;
		RadTreeNode backupNode;
		System.Data.DataTable dtNodes;

		Telerik.WinControls.UI.RadContextMenu sectionMenu = new Telerik.WinControls.UI.RadContextMenu();
		Telerik.WinControls.UI.RadContextMenu parentMenu = new Telerik.WinControls.UI.RadContextMenu();
		Telerik.WinControls.UI.RadContextMenu childMenu = new Telerik.WinControls.UI.RadContextMenu();

		const string GroupWizard = "sADDTVGROUP";
		const string NodeWizard = "sADDTREENODE";
		const string constDeleteParent = "DDELPTVNODE";
		const string constDeleteChild = "DDELTVCNODE";
		const string constRemoveFromFav = "DRemoveAKFav";
		const string constCheckAKFavs = "DCheckAKFavs";

        const string constFullSDK = "AMUSDK"; //top of the SDK group

        const string constExtendedDataSDK = "-1718510834"; //Extended Data branch
        const string constFullSDKHeader = "AMUSDKHDR"; //Framework SDK branch
        const string constReportSDK = "AMURPT"; //Reports SDK branch
        const string constFMDes = "AMFMDES";
        const string constFMDes2 = "AMFMDES2";
        const string constAMUVCONTROL = "AMUVCONTROL";
        const string constAMFMTEAM = "AMFMTEAM";

        const string constSystemScripts = "AMUSYSSCRPT";
		const string constScreenDesigner = "AMUDESIG";

		private string parentKey;
		private ITreeViewNavigationHost parent;

        List<string> sdkLicenseRequiredCodes = null;
        List<string> extendedDataLicenseRequiredCodes = null;

        public TreeNavigationBuilder(ITreeViewNavigationHost Parent)
		{
            sdkLicenseRequiredCodes = new List<string>
            {
                constFullSDKHeader
                , constExtendedDataSDK
                , constReportSDK
                , constFMDes
                , constFMDes2
                , constAMUVCONTROL
                , constAMFMTEAM
            };

            extendedDataLicenseRequiredCodes = new List<string>
            {
                constExtendedDataSDK
            };

            parent = Parent;
			radTreeView1 = parent.TreeView;
			tabcontrol = parent.TabControl;
			actions = parent.Actions;
			actions.PackageInstalled += new EventHandler(actions_PackageInstalled);
			this.parentKey = parent.Actions.ParentKey;
		}

		void actions_PackageInstalled(object sender, EventArgs e)
		{
			RefreshTreeView();
		}

		public void ConstructTree()
		{
			ResetTreeViewControl();
			SetupTreeView(false);
		}

		internal void SetupTreeView(bool fetchdata)
		{
			SetupContextMenus();
			if (fetchdata)
			{
				dtNodes = GetTreeNodeData();
				SetupTreeEvents();
			}
			GetMasterNodes(dtNodes);
			if (radTreeView1.Nodes.Count > 0)
				radTreeView1.SelectedNode = radTreeView1.Nodes[0];
            try
            {
                radTreeView1.CollapseAll();
            }
            catch { } //Swallow the error occuring within the RadTreeView control as the error is happening within here, and swallowing the error doesnt seem to cause any harm - may need more looking into at a later date; but closing and re-opening OMSAdmin.exe will be best course of action if any further issues.
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
			SetupSectionContextMenu();
			SetupParentContextMenu();
			SetupChildContextMenu();
		}

		private void SetupSectionContextMenu()
		{
			if (sectionMenu.Items.Count == 0)
			{
				sectionMenu.Items.Add(CreateAddSection("NEWSECTION"));
				sectionMenu.Items.Add(CreateEditSection("EDITSECTION"));
				sectionMenu.Items.Add(RemoveSection("DELETESECTION"));
				sectionMenu.Items.Add(CreateAddParent("ADDNEWGRP"));
			}
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

		private RadMenuItem CreateAddSection(string lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem addsection = new Telerik.WinControls.UI.RadMenuItem();
			addsection.Text = ResourceLookup.GetLookupText(lookupText); //"Add New Section"
			addsection.MouseUp += new MouseEventHandler(addsection_MouseUp);
			return addsection;
		}


        private RadMenuItem CreateEditSection(string lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem editsection = new Telerik.WinControls.UI.RadMenuItem();
			editsection.Text = ResourceLookup.GetLookupText(lookupText); //"Edit Section"
			editsection.MouseUp += new MouseEventHandler(editsection_MouseUp);
			return editsection;
		}


		private RadMenuItem RemoveSection(string lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem removesection = new Telerik.WinControls.UI.RadMenuItem();
			removesection.Text = ResourceLookup.GetLookupText(lookupText); //"Remove Section"
			removesection.MouseUp += new MouseEventHandler(deletesection_MouseUp);
            removesection.KeyPress += Removesection_KeyPress;
			return removesection;
		}


        private RadMenuItem CreateAddParent(string  lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem addparent = new Telerik.WinControls.UI.RadMenuItem();
			addparent.Text = ResourceLookup.GetLookupText(lookupText); //"Add New Group"
			addparent.MouseUp += new MouseEventHandler(addparent_MouseUp);
            addparent.KeyPress += Addparent_KeyPress;
			return addparent;
		}

        

        private RadMenuItem CreateEditParent(string lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem editparent = new Telerik.WinControls.UI.RadMenuItem();
			editparent.Text = ResourceLookup.GetLookupText(lookupText); //"Edit Group"
			editparent.MouseUp += new MouseEventHandler(editparent_MouseUp);
            editparent.KeyPress += Editparent_KeyPress;
			return editparent;
		}


        private RadMenuItem AddNewMemberToParent(string lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem AddMemberToParent = new Telerik.WinControls.UI.RadMenuItem();
			AddMemberToParent.Text = ResourceLookup.GetLookupText(lookupText); //"Add New Group Member"
			AddMemberToParent.MouseUp += new MouseEventHandler(AddMemberToParent_MouseUp);
            AddMemberToParent.KeyPress += AddMemberToParent_KeyPress;
			return AddMemberToParent;
		}


        private RadMenuItem RemoveParent(string lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem removeparent = new Telerik.WinControls.UI.RadMenuItem();
			removeparent.Text = ResourceLookup.GetLookupText(lookupText); //"Remove Group"
			removeparent.MouseUp += new MouseEventHandler(removeparent_MouseUp);
            removeparent.KeyPress += Removeparent_KeyPress;
			return removeparent;
		}


        private RadMenuItem MoveParent(string lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem moveparent = new Telerik.WinControls.UI.RadMenuItem();
			moveparent.Text = ResourceLookup.GetLookupText(lookupText); //"Move Group"
			moveparent.MouseUp += new MouseEventHandler(moveparent_MouseUp);
            moveparent.KeyPress += Moveparent_KeyPress;
			return moveparent;
		}


        private void SetupChildContextMenu()
		{
			if (childMenu.Items.Count == 0)
			{
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
            addchild.KeyPress += Addchild_KeyPress;
			return addchild;
		}


        private RadMenuItem EditChild(string lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem editchild = new Telerik.WinControls.UI.RadMenuItem();
			editchild.Text = ResourceLookup.GetLookupText(lookupText); //"Edit Group Member";
			editchild.MouseUp += new MouseEventHandler(editchild_MouseUp);
            editchild.KeyPress += new KeyPressEventHandler(Editchild_KeyPress);
			return editchild;
		}


        private RadMenuItem RemoveChild(string lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem removechild = new Telerik.WinControls.UI.RadMenuItem();
			removechild.Text = ResourceLookup.GetLookupText(lookupText); //"Remove Group Member";
			removechild.MouseUp += new MouseEventHandler(removechild_MouseUp);
            removechild.KeyPress += Removechild_KeyPress;
			return removechild;
		}


        private RadMenuItem AddChildToFav(string lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem addchildtofav = new Telerik.WinControls.UI.RadMenuItem();
			addchildtofav.Text = ResourceLookup.GetLookupText(lookupText); //"Add to Favourites";
			addchildtofav.MouseUp += new MouseEventHandler(addchildtofav_MouseUp);
            addchildtofav.KeyPress += Addchildtofav_KeyPress;
			return addchildtofav;
		}

        

        private RadMenuItem MoveChildNode(string lookupText)
		{
			Telerik.WinControls.UI.RadMenuItem movechildnode = new Telerik.WinControls.UI.RadMenuItem();
			movechildnode.Text = ResourceLookup.GetLookupText(lookupText); //"Move Node";
			movechildnode.MouseUp += new MouseEventHandler(movechildnode_MouseUp);
            movechildnode.KeyPress += Movechildnode_KeyPress;
			return movechildnode;
		}


        #region Tree Element Creation

        private void GetMasterNodes(System.Data.DataTable dt)
		{
			DataRow[] masters = dt.Select("admnuName = '" + parentKey + "' and admnuSearchListCode like 'AKC|%'");
			for (int i = 0; i < masters.Length; i++)
			{
				CreateParentTreeNode(dt, masters[i], true);
			}
		}

		private void CreateParentTreeNode(System.Data.DataTable dt, DataRow r, bool isMaster)
		{
			if(CheckUserHasAccessToNodeAtCreation(r))
			{
				RadTreeNode parent = CreateParentNode(r, isMaster);
				ProcessChildNodes(dt,r,parent);
				return;
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

		private RadTreeNode CreateParentNode(DataRow r, bool isMaster)
		{
			RadTreeNode node = TreeViewNavigation.TreeViewFormatter.NewTreeNode();
			node.Text = Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", Convert.ToString(r["admnuCode"])),true);
			node.Tag = CreateTreeNodeTag(r);
			if (isMaster)
				node.ContextMenu = sectionMenu;
			else
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
				RadTreeNode node = TreeViewNavigation.TreeViewFormatter.NewTreeNode();
				node.Text = Session.CurrentSession.Terminology.Parse(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", Convert.ToString(r["admnuCode"])),true);
				node.Tag = CreateTreeNodeTag(r);
				parent.Nodes.Add(node);

				DataRow[] nextlevel = GetChildNodes(dt, Convert.ToInt64(r["admnuID"]));
				if (nextlevel.Length != 0 || string.IsNullOrWhiteSpace(Convert.ToString(r["admnuSearchListCode"])))
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


		public void AutoSelectNode(long nodeID)
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
				   && node.Tag as string == string.Empty
				   && !node.Enabled;
		}

		
		private bool ParentIsTargetNode(RadTreeNode parentNode, long targetNodeID)
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
            this.radTreeView1.NodeExpandedChanging -= treeView_NodeExpandedChanging;
            this.radTreeView1.NodeExpandedChanging += treeView_NodeExpandedChanging;

            this.radTreeView1.KeyPress -= new KeyPressEventHandler(treeView_KeyPress);
			this.radTreeView1.KeyPress += new KeyPressEventHandler(treeView_KeyPress);

            this.radTreeView1.MouseDown -= new MouseEventHandler(treeView_MouseDown);
            this.radTreeView1.MouseDown += new MouseEventHandler(treeView_MouseDown);

            this.radTreeView1.MouseClick -= new MouseEventHandler(treeView_MouseClick);
			this.radTreeView1.MouseClick += new MouseEventHandler(treeView_MouseClick);

			this.radTreeView1.SelectedNodeChanging -= this.treeView_SelectedNodeChanging;
			this.radTreeView1.SelectedNodeChanging += this.treeView_SelectedNodeChanging;

            this.radTreeView1.NodeFormatting -= TreeViewNavigation.TreeViewFormatter.NodeFormatting;
            this.radTreeView1.NodeFormatting += TreeViewNavigation.TreeViewFormatter.NodeFormatting;
		}

        private void treeView_NodeExpandedChanging(object sender, RadTreeViewCancelEventArgs e)
        {
            var node = ((RadTreeViewElement)sender).SelectedNode;

            if (node == null)
            {
                return;
            }

            if (!NodeHasChildren(node))
            {
                return;
            }

            if (!CheckSDKAccess(node))
            {
                e.Cancel = true;
            }
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
                var node = ((RadTreeView)sender).SelectedNode;

                if (node == null)
                {
                    return;
                }

                var tagData = GetTagNodeData(node);

                if (string.IsNullOrEmpty(tagData.objCode))
                {
                    return;
                }

                if (!CheckSDKAccess(node))
                {
                    return;
                }

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
						AddSectionToTreeView();
				}
			}
		}


        private void treeView_MouseDown(object sender, MouseEventArgs e)
        {
            TreeNodeTagData clickedNodeTag = null;
            if (e.Clicks == 1 && this.radTreeView1.Nodes.Count > 0 && this.radTreeView1.GetNodeAt(e.X, e.Y) != null)
            {
                RadTreeNode clickedNode = this.radTreeView1.GetNodeAt(e.X, e.Y);
                radTreeView1.SelectedNode = clickedNode;
                if (clickedNode.Tag != null)
                    clickedNodeTag = clickedNode.Tag as TreeNodeTagData;

                if (clickedNodeTag != null)
                {
                    var nodeCode = clickedNodeTag.objDesc;
                    if (!nodeCode.StartsWith("AKC") && !string.IsNullOrWhiteSpace(nodeCode))
                    {
                        if (e.Button == MouseButtons.Left)
                        {
                            try
                            {
                                if (CheckSDKAccess(clickedNode))
                                {
                                    ExecuteFunction();
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorBox.Show(ex);
                            }
                        }
                    }
                    else if (!nodeCode.StartsWith("AKC") && string.IsNullOrWhiteSpace(nodeCode))
                    {
                        CheckSDKAccess(clickedNode);
                    }
                }
            }
        }


		private void ExecuteFunction()
		{
            try
            {
                actions.TabAdded -= new EventHandler(actions_TabAdded);
                actions.TabAdded += new EventHandler(actions_TabAdded);

                TreeNodeTagData data = (TreeNodeTagData)radTreeView1.SelectedNode.Tag;
                if (data != null && !string.IsNullOrWhiteSpace(data.objCode))
                {
                    if (CheckUserHasAccess(data.objRoles, data.objCode))
                        actions.DisplaySelectedFunction(data);
                    else
                        swf.MessageBox.Show(ResourceLookup.GetLookupText("NOTNODEACCESS"), ResourceLookup.GetLookupText("MSMSGCAPTION"), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch(Exception ex)
            {
                FWBS.OMS.UI.Windows.MessageBox.Show(ex);
            }
		}

		void actions_TabAdded(object sender, EventArgs e)
		{
			AddToLast10(this.radTreeView1.SelectedNode);
			OnAddedToLast10(EventArgs.Empty);
		}

		public event EventHandler AddedToLast10;

		private void OnAddedToLast10(EventArgs e)
		{
			if (AddedToLast10 != null)
				AddedToLast10(this, e);
		}

		#region Section-related menu items

		private void addsection_MouseUp(object sender, MouseEventArgs e)
		{
			AddSectionToTreeView();
		}

		private void editsection_MouseUp(object sender, MouseEventArgs e)
		{
			EditTreeViewSection();
		}

        private void Removesection_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                DeleteTreeViewSection();
            }
        }


        private void deletesection_MouseUp(object sender, MouseEventArgs e)
		{
			DeleteTreeViewSection();
		}

		private void AddSectionToTreeView()
		{
			string _addconsole = Session.CurrentSession.DefaultSystemForm(SystemForms.ConsoleItem);
			FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection();
			kvc.Add("admnuParent", 1);
			kvc.Add("MENUNAME", parentKey);
			var add = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(_addconsole, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, kvc);
			if (add != null)
				RefreshTreeView();
		}

		private void EditTreeViewSection()
		{
			if (!IsSystemTreeNode(ResourceLookup.GetLookupText("NOEDITTREENODE")) && CheckUserHasAdminAccess())
			{
				FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "admnuID", GetNodeID() }, {"admnuSearchListCode", GetNodeCode() }, { "parentKey", parentKey } };
				var edit = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(Session.CurrentSession.DefaultSystemForm(FWBS.OMS.SystemForms.ConsoleItem), null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, kvc);
				if (edit != null)
					RefreshTreeView();
			}
		}

		private void DeleteTreeViewSection()
		{
			if (!IsSystemTreeNode(ResourceLookup.GetLookupText("NODELTREENODE")) && CheckUserHasAdminAccess())
			{
				if (swf.MessageBox.Show(ResourceLookup.GetLookupText("DELTVSECTMSG"), ResourceLookup.GetLookupText("MSMSGCAPTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					if(FindNodeToRemove(GetNodeID()))
						RefreshTreeView();
				}
			}
		}

		#endregion

		#region Refresh TreeView

		public void RefreshTreeView()
		{
			ResetTreeViewControl();
			SetupTreeView(true);
		}

		public void RefreshTreeViewPostSearch()
		{
			MakeAllTreeNodesVisible();
			radTreeView1.CollapseAll();
		}

		private void MakeAllTreeNodesVisible()
		{
			RadTreeNodeCollection nodes = radTreeView1.Nodes;
			foreach (RadTreeNode n in nodes)
			{
				SetNodeToOriginalState(n);
				MakeNodeVisible(n);
			}
		}

		private void MakeNodeVisible(RadTreeNode treeNode)
		{
		   foreach (RadTreeNode rtn in treeNode.Nodes)
		   {
			   SetNodeToOriginalState(rtn);
			   MakeNodeVisible(rtn);
		   }
		}

		public void RemoveSearchMatches()
		{
			RadTreeNodeCollection nodes = radTreeView1.Nodes;
			foreach (RadTreeNode n in nodes)
			{
				SetNodeToOriginalState(n);
				RemoveChildSearchMatches(n);
			}
		}

		private void RemoveChildSearchMatches(RadTreeNode treeNode)
		{
			foreach (RadTreeNode rtn in treeNode.Nodes)
			{
				SetNodeToOriginalState(rtn);
				RemoveChildSearchMatches(rtn);
			}
		}

		private void SetNodeToOriginalState(RadTreeNode n)
		{
            n.ForeColor = Color.Black;
            n.Visible = true;
        }

        #endregion

        #region Group-related menu items

        private void Addparent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                AddParentGroupToTreeView();
            }
        }
        
        private void addparent_MouseUp(object sender, MouseEventArgs e)
		{
			AddParentGroupToTreeView();
		}

		private void AddParentGroupToTreeView()
		{
            if (!CanExecuteFunction(radTreeView1.SelectedNode))
            {
                return;
            }

            FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "consoleID", GetNodeID() }, {"parentKey", parentKey} };
			var add = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(GroupWizard, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, kvc);
			if (add != null)
			{
				ResetTreeViewControl();
				SetupTreeView(true);
			}
		}

		private void editparent_MouseUp(object sender, MouseEventArgs e)
        {
            EditParent();
        }

        private void Editparent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                EditParent();
            }
        }


        private void EditParent()
        {
            if (!CanExecuteFunction(radTreeView1.SelectedNode))
            {
                return;
            }

            if (!IsSystemTreeNode(ResourceLookup.GetLookupText("NOEDITTREENODE")) && CheckUserHasAdminAccess())
            {
                FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "ID", GetNodeID() }, { "parentKey", parentKey } };
                var edit = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(GroupWizard, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, kvc);
                if (edit != null)
                {
                    ResetTreeViewControl();
                    SetupTreeView(true);
                }
            }
        }


        private void AddMemberToParent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                AddMemberToParent();
            }
        }


        private void AddMemberToParent_MouseUp(object sender, MouseEventArgs e)
        {
            AddMemberToParent();
        }


        private void AddMemberToParent()
        {
            if (!CanExecuteFunction(radTreeView1.SelectedNode))
            {
                return;
            }

            if (CheckUserHasAdminAccess())
            {
                FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "consoleID", consoleID }, { "parentNode", false }, { "parentID", GetNodeID() }, { "parentKey", parentKey } };
                var edit = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(NodeWizard, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, kvc);
                if (edit != null)
                {
                    ResetTreeViewControl();
                    SetupTreeView(true);
                }
            }
        }

   
        private void Removeparent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                RemoveParent();
            }
        }


        private void removeparent_MouseUp(object sender, MouseEventArgs e)
        {
            RemoveParent();
        }


        private void RemoveParent()
        {
            if (!CanExecuteFunction(radTreeView1.SelectedNode))
            {
                return;
            }

            if (!IsSystemTreeNode(ResourceLookup.GetLookupText("NODELTREENODE")) && CheckUserHasAdminAccess())
            {
                if (swf.MessageBox.Show(ResourceLookup.GetLookupText("DELTNGRPMSG"), ResourceLookup.GetLookupText("MSMSGCAPTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    FindNodeToRemove(GetNodeID());
                }

            }
        }

        private void Moveparent_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                MoveParent();
            }
        }


        private void moveparent_MouseUp(object sender, EventArgs e)
        {
            MoveParent();
        }


        private void MoveParent()
        {
            if (!CanExecuteFunction(radTreeView1.SelectedNode))
            {
                return;
            }

            NodeMovementTool(true);
        }

        #endregion

        private void Addchild_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                AddChild();
            }
        }


        private void addchild_MouseUp(object sender, EventArgs e)
        {
            AddChild();
        }


        private void AddChild()
        {
            if (!CanExecuteFunction(radTreeView1.SelectedNode))
            {
                return;
            }

            if (CheckUserHasAdminAccess())
            {
                FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "parentNode", false }, { "parentID", GetNodeID() }, { "parentKey", parentKey } };
                var add = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(NodeWizard, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Add, kvc);
                if (add != null)
                {
                    ResetTreeViewControl();
                    SetupTreeView(true);
                }
            }
        }

        private void Editchild_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                EditChild();
            }
        }


        private void editchild_MouseUp(object sender, MouseEventArgs e)
        {
            EditChild();
        }


        private void EditChild()
        {
            if (!CanExecuteFunction(radTreeView1.SelectedNode))
            {
                return;
            }

            if (!IsSystemTreeNode(ResourceLookup.GetLookupText("NOEDITTREENODE")) && CheckUserHasAdminAccess())
            {
                FWBS.Common.KeyValueCollection kvc = new FWBS.Common.KeyValueCollection() { { "id", GetNodeID() }, { "parentNode", false }, { "parentID", GetParentNodeID() }, { "parentKey", parentKey } };
                var edit = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(NodeWizard, null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, kvc);
                if (edit != null)
                {
                    try
                    {
                        UpdateUserFavourites(CreateUpdateKVC((DataTable)edit));
                        ResetTreeViewControl();
                        SetupTreeView(true);
                        OnAdminKitTreeNodeEdited();
                    }
                    catch (Exception ex)
                    {
                        ErrorBox.Show(ex);
                    }
                }
            }
        }


        private FWBS.Common.KeyValueCollection CreateUpdateKVC(DataTable dt)
        {
            FWBS.Common.KeyValueCollection kvcFavUpdate = new FWBS.Common.KeyValueCollection() {
                { "id", Convert.ToInt64(dt.Rows[0]["ID"]) }
                , { "objectCode", Convert.ToString(dt.Rows[0]["tvNodeObjCode"])}
                , { "description", Convert.ToString(dt.Rows[0]["Description"]) }
                , { "roles", Convert.ToString(dt.Rows[0]["tvNodeRoles"]) }
            };
            return kvcFavUpdate;
        }


        private void UpdateUserFavourites(FWBS.Common.KeyValueCollection updateKVC)
        {
            try
            {
                FWBS.OMS.Favourites.UpdateUserFavourites(updateKVC);
            }
            catch (Exception ex)
            {
                throw new Exception(Session.CurrentSession.Resources.GetResource("AKFAVUPDATE1", "Updating the Admin Kit Favourites failed.", "").Text, ex);
            }
        }


        public event EventHandler AdminKitTreeNodeEdited;

        private void OnAdminKitTreeNodeEdited()
        {
            if (AdminKitTreeNodeEdited != null)
                AdminKitTreeNodeEdited(this, EventArgs.Empty);
        }


        private void Removechild_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                RemoveChild();
            }
        }


        private void removechild_MouseUp(object sender, EventArgs e)
        {
            RemoveChild();
        }


        private void RemoveChild()
        {
            if (!CanExecuteFunction(radTreeView1.SelectedNode))
            {
                return;
            }

            if (!IsSystemTreeNode(ResourceLookup.GetLookupText("NODELTREENODE")) && CheckUserHasAdminAccess())
            {
                if (swf.MessageBox.Show(ResourceLookup.GetLookupText("DELTVCHILDMSG"), ResourceLookup.GetLookupText("MSMSGCAPTION"), MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    long nodeID = GetNodeID();
                    RunDataList(constDeleteChild, new FWBS.Common.KeyValueCollection() { { "ID", nodeID } });
                    FindNodeToRemove(nodeID);
                    RemoveFromLast10AndFavourites(nodeID);
                    parent.RefreshFavorites();
                    parent.RefreshLast10();
                }
            }
        }


        private void Addchildtofav_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                AddToFavourites();
            }
        }


        private void addchildtofav_MouseUp(object sender, EventArgs e)
		{
			AddToFavourites();
		}


		private void AddToFavourites()
		{
            if (!CanExecuteFunction(radTreeView1.SelectedNode))
            {
                return;
            }

            TreeNodeTagData tag = (TreeNodeTagData)radTreeView1.SelectedNode.Tag;
			FWBS.OMS.Favourites fav = new FWBS.OMS.Favourites(parentKey + "FAV");
			if (fav.Count < 10)
			{
				if (!CheckForExistingFavourite(tag.objID))
				{
					fav.AddFavourite(tag.objCode, "1", new string[] { Convert.ToString(tag.objID) + ";" + Convert.ToString(consoleID), tag.objDesc, tag.objRoles, "0" });
					fav.Update();
					swf.MessageBox.Show(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", "AKMYFAVADDMSG"), FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", "AKMYFAVCAP"), MessageBoxButtons.OK, MessageBoxIcon.Information);
					parent.RefreshFavorites();
				}
			}
			else
			{
				swf.MessageBox.Show(FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", "AKMYFAVMAXMSG"), FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", "AKMYFAVCAP"), MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}


        private static void UserNeedsSDKLicenseToAddToFavourites()
        {
            swf.MessageBox.Show(
                                Session.CurrentSession.Resources.GetResource("LIC_SDKFAVERR", "You must have the Full SDK license to execute this action.", "").Text,
                                Session.CurrentSession.Resources.GetResource("LIC_CANTPROCEED", "Cannot proceed with this action...", "").Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
        }


        private static void UserNeedsExtendedDataOrSDKLicenseToAddToFavourites()
        {
            swf.MessageBox.Show(
                                Session.CurrentSession.Resources.GetResource("LIC_EXTFAVERR", "You must have the Extended Data or SDK license to execute this action.", "").Text,
                                Session.CurrentSession.Resources.GetResource("LIC_CANTPROCEED", "Cannot proceed with this action...", "").Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
        }


        private void Movechildnode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                NodeMovementTool(false);
            }
        }


        private void movechildnode_MouseUp(object sender, EventArgs e)
		{
			NodeMovementTool(false);
		}


		private void NodeMovementTool(bool allowAddToSection)
		{
			TreeNodeTagData tag = (TreeNodeTagData)this.radTreeView1.SelectedNode.Tag;
			if (!ConvertDef.ToBoolean(tag.objSystem,false))
				ActivateNodeMovementTool(tag, allowAddToSection);
			else
				swf.MessageBox.Show(FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "TNNOMOVE"), FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "NODEMOVEDCAP"), MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ActivateNodeMovementTool(TreeNodeTagData tag, Boolean allowAddToSection)
		{
			ucAdminKitNodeMover mover = new ucAdminKitNodeMover(allowAddToSection, parentKey);
            var result = mover.ShowDialog(this.radTreeView1.TopLevelControl);
			if (result == DialogResult.OK)
			{
				ApplyNodeMovement(mover, tag.objID);
				RefreshTreeView();
			}
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
			RePointNodeParent(moverID);
			swf.MessageBox.Show(FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "NODEMOVED"), FWBS.OMS.CodeLookup.GetLookup("RESOURCE", "NODEMOVEDCAP"), MessageBoxButtons.OK, MessageBoxIcon.Information);
		}


		private void RePointNodeParent(long nodeIDToMove)
		{
			foreach(RadTreeNode node in radTreeView1.Nodes)
			{
				if (GetNodeID(node) == nodeIDToMove)
				{
					node.Remove();
					return;
				}
				else
				{
					FindChildNodeToRepoint(node, nodeIDToMove);
				}
			}
		}


		private void FindChildNodeToRepoint(RadTreeNode node, long nodeIDToMove)
		{
			foreach (RadTreeNode childnode in node.Nodes)
			{
				if (GetNodeID(childnode) == nodeIDToMove)
				{
					childnode.Remove();
					return;
				}
				else
				{
					FindChildNodeToRepoint(childnode, nodeIDToMove);
				}
			}
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

		internal bool FindNodeToRemove(long searchnodeID)
		{
			bool result = false;
			RadTreeNode parent;

			for (int i = 0; i < radTreeView1.Nodes.Count; i++)
			{
				long topNodeID = GetNodeID(radTreeView1.Nodes[i]);
				if (topNodeID == searchnodeID)
				{
					TrimTree(radTreeView1.Nodes[i].Nodes);
					radTreeView1.Nodes[i].Remove();
					RunDataList(constDeleteParent, new FWBS.Common.KeyValueCollection() { { "ID", topNodeID } });
					result = true;
				}
				else
				{
					parent = radTreeView1.Nodes[i];
					for (int j = 0; j < parent.Nodes.Count; j++)
					{
						bool found = false;
						CheckChildNodes(parent.Nodes[j], searchnodeID, out found);
						if (found)
						{
							result = true;
							break;
						}
					}
				}
			}
			return result;
		}

		private void CheckChildNodes(RadTreeNode Node, long searchNodeID, out bool found)
		{
			long nodeID = GetNodeID(Node);
			if (nodeID == searchNodeID)
			{
				if (Node.Nodes.Count > 0)
				{
					TrimTree(Node.Nodes);
					RemoveParentNode(nodeID);
				}
				else
				{
					RemoveParentNode(nodeID);
				}
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
					RemoveParentNode(nodeID);
				}
			}
		}

		private void RemoveParentNode(long nodeID)
		{
			RunDataList(constDeleteParent, new FWBS.Common.KeyValueCollection() { { "ID", nodeID } });
			RemoveFromLast10AndFavourites(nodeID);
		}


		private long GetNodeID()
		{
			TreeNodeTagData tag = (TreeNodeTagData)this.radTreeView1.SelectedNode.Tag;
			return tag.objID;
		}

		private string GetNodeCode()
		{
			TreeNodeTagData tag = (TreeNodeTagData)this.radTreeView1.SelectedNode.Tag;
			return tag.objCode;
		}

		private long GetNodeID(RadTreeNode node)
		{
            TreeNodeTagData tag = node.Tag as TreeNodeTagData;
            return (tag != null) ? tag.objID : -1;
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
			KeyValueCollection kvc = new KeyValueCollection() {{"nodeID",nodeID}, {"userID", Session.CurrentSession.CurrentUser.ID}};
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

		#region TreeView Search

		public void TreeViewSearch(string searchstring)
		{
			searchstring = Session.CurrentSession.Terminology.Parse(searchstring, false);
			RadTreeNodeCollection nodes = radTreeView1.Nodes;
			foreach (RadTreeNode n in nodes)
			{
				if (PerformNodeChecks(n, searchstring))
					SetMatchingNodeVisible(n);
				else
					n.Visible = false;
				TreeViewSearchChildren(n, searchstring);
			}
		}

		private void TreeViewSearchChildren(RadTreeNode treeNode, string searchstring)
		{
			foreach (RadTreeNode rtn in treeNode.Nodes)
			{
				if (PerformNodeChecks(rtn, searchstring))
				{
					SetMatchingNodeVisible(rtn);
					ExpandParentNodes(rtn);
				}
				else
					rtn.Visible = false;
				TreeViewSearchChildren(rtn, searchstring);
			}
		}

		private bool PerformNodeChecks(RadTreeNode n, string searchstring)
		{
            TreeNodeTagData tag = n.Tag as TreeNodeTagData;
            if (tag == null)
				return false;

			if (!tag.objCode.Contains("AKC"))
			{
				return PerformAdvancedSearch(searchstring, tag, n);
			}
			else
				return false;
		}

		private bool PerformStandardSearch(string searchstring, TreeNodeTagData tag, RadTreeNode n)
		{
			if (GetNodeText(n).Contains(searchstring) && !string.IsNullOrWhiteSpace(tag.objCode))
				return true;
			else
				return false;
		}

		private bool PerformAdvancedSearch(string searchstring, TreeNodeTagData tag, RadTreeNode n)
		{
			if (!string.IsNullOrWhiteSpace(tag.objCode) && (GetNodeText(n).Contains(searchstring) || tag.objCode.ToUpper().Contains(searchstring)))
				return true;
			else
				return false;
		}

		private void SetMatchingNodeVisible(RadTreeNode n)
		{
			n.ForeColor = Color.CornflowerBlue;
			n.Visible = true;
		}

		private string GetNodeText(RadTreeNode n)
		{
            TreeNodeTagData tag = n.Tag as TreeNodeTagData;
            return (tag != null) ? Session.CurrentSession.Terminology.Parse(tag.objText, true).ToUpper() : string.Empty;
		}

		private void ExpandParentNodes(RadTreeNode n)
		{
			while (n != null)
			{
				n.Expand();
				n.Visible = true;
				n = n.Parent;
			}
		}


		#endregion

		#region Last 10 & Favourites

		public void AddToLast10(RadTreeNode node)
        {            
            bool addlast10item = CanAddToLast10(node);

            if (addlast10item)
            {
                string favType = parentKey + "LAST10";
                TreeNodeTagData tag = (TreeNodeTagData)node.Tag;
                if (CheckForValidFunction(tag))
                {
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

                        if (Convert.ToString(check.Rows[0]["result"]) == "0")
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
                        ErrorBox.Show(e);
                    }
                }
            }
        }


        private bool CanExecuteFunction(RadTreeNode node)
        {
            bool result = true;

            if (!IsNodeInSDK(node))
            {
                return true;
            }

            if (IsNodeInExtendedData(node) && (!IsLicensed("SDKEXD")))
            {
                if (IsLicensed("SDKALL"))
                {
                    return true;
                }
            
                UserNeedsExtendedDataOrSDKLicenseToAddToFavourites();
                return false;
            }

            if (IsNodeInSDK(node) && !IsLicensed("SDKALL"))
            {
                if (IsNodeInExtendedData(node) && IsLicensed("SDKEXD"))
                {
                    return true;
                }

                UserNeedsSDKLicenseToAddToFavourites();
                return false;
            }

            return result;
        }


        private bool CanAddToLast10(RadTreeNode node)
        {
            bool result = false;

            if (node == null)
            {
                return false;
            }

            if (!IsNodeInSDK(node))
            {
                return true;
            }

            if (IsNodeInSDK(node) && IsLicensed("SDKALL"))
            {
                result = true;
            }

            if (IsNodeInExtendedData(node) && (IsLicensed("SDKEXD") || IsLicensed("SDKALL")))
            {
                result = true;
            }

            return result;
        }


        private bool CheckForValidFunction(TreeNodeTagData tag)
		{
			return (!string.IsNullOrWhiteSpace(tag.objCode) && !tag.objCode.Contains("AKC"));
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
				ErrorBox.Show(ex);
			}
		}

		private void RemoveFromLast10AndFavourites(long nodeID)
		{
			RunDataList(constRemoveFromFav, new FWBS.Common.KeyValueCollection() {
											{ "nodeID", Convert.ToString(nodeID) },
											{ "usrID", FWBS.OMS.Session.CurrentSession.CurrentUser.ID },
											{ "type", parentKey } });
			RemoveRelevantEntries(nodeID);
		}


		private void RemoveRelevantEntries(long nodeID)
		{
			if (parentKey == "ADMIN")
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


		private bool CheckUserHasAdminAccess()
		{
			if (IsInRoles("ADMIN", FWBS.OMS.Session.CurrentSession.CurrentUser))
				return true;
			else
				return false;
		}


		private bool CheckUserHasAccess(object objRoles, object objCode)
		{
			string[] nodeRoles = Convert.ToString(objRoles).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (nodeRoles.Length == 0)
				return true;

			User user = Session.CurrentSession.CurrentUser;
			foreach (string role in nodeRoles)
			{
				if (IsInRoles(role, user))
					return true;
			}

			if (IsInRoles("POWER", user))
			{
				Power power = Session.CurrentSession.CurrentPowerUserSettings;
				if (power != null && power.IsConfigured)
					return power.CanRunAction(Convert.ToString(objCode));
			}

			return false;
		}


		private bool CheckUserHasAccessToNodeAtCreation(DataRow r)
		{
			return CheckUserHasAccess(r["admnuRoles"], r["admnuSearchListCode"]);
		}


		private bool IsInRoles(string role, FWBS.OMS.User u)
		{
			if (string.IsNullOrEmpty(role))
				return true;

			string[] vals = u.Roles.Split(',');
			return Array.IndexOf(vals, role) > -1;
		}


        private TreeNodeTagData GetTagNodeData(RadTreeNode node)
        {
            return (TreeNodeTagData)node.Tag;
        }


        private bool IsNodeInSDK(RadTreeNode node)
        {
            var nodeData = GetTagNodeData(node);
            return (sdkLicenseRequiredCodes.Contains(nodeData.objDesc) || IsSDKChildNode(node));    
        }


        private bool IsNodeInExtendedData(RadTreeNode node)
        {
            var nodeData = GetTagNodeData(node);
            return (extendedDataLicenseRequiredCodes.Contains(nodeData.objDesc) || IsExtendedDataChildNode(node));
        }


        internal bool CheckSDKAccess(RadTreeNode node)
		{
            bool result = false;

            var nodeData = GetTagNodeData(node);

            try
			{
                if (FWBS.OMS.UI.Windows.frmMain.PartnerAccess || !IsNodeInSDK(node))
                {
                    result = true;
                }
                else
                {
                    if(nodeData.objDesc != constExtendedDataSDK && !IsExtendedDataChildNode(node))
                    {
                        if (!IsLicensed("SDKALL"))
                        {
                            result = GetPartnerAccessPasswordFromUser("Full SDK Access Granted", "3E MatterSphere Framework SDK");
                        }
                        else
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        if (!FWBS.OMS.UI.Windows.frmMain.PartnerAccess)
                        {
                            if (IsLicensed("SDKALL"))
                            {
                                result = true;
                            }
                            else
                            {
                                if (!IsLicensed("SDKEXD"))
                                {
                                    result = GetPartnerAccessPasswordFromUser("Extended SDK Access Granted", "Extended Data SDK");
                                }
                                else
                                {
                                    result = true;
                                }
                            }
                        }
                        else
                        {
                            result = true;
                        }
                    }
                }
			}
			catch (Exception ex)
			{
				ErrorBox.Show(ex);
			}

            return result;
        }


        private bool GetPartnerAccessPasswordFromUser(string logMessage, string licensemissing)
        {
            bool result = ActivatePasswordRequestDialog(logMessage, licensemissing);

            if (result)
            {
                FWBS.OMS.UI.Windows.frmMain.PartnerAccess = true;
            }

            return result;
        }


        private bool IsSDKChildNode(RadTreeNode node)
        {
             return IsChildNode(node, sdkLicenseRequiredCodes);
        }


        private bool IsExtendedDataChildNode(RadTreeNode node)
        {
            return IsChildNode(node, extendedDataLicenseRequiredCodes);
        }


        private bool IsChildNode(RadTreeNode node, List<string> licenseRequiredCodes)
        {
            bool result = false;

            while (node != null)
            {
                var nodeData = GetTagNodeData(node);

                if (licenseRequiredCodes.Contains(nodeData.objDesc))
                {
                    result = true;
                    break;
                }
                else
                {
                    node = node.Parent;
                }
            }

            return result;
        }


        private bool IsLicensed(string licenseCode)
        {
            return Session.CurrentSession.IsLicensedFor(licenseCode);
        }


		private bool ActivatePasswordRequestDialog(string logmessage, string licensemissing)
        {
            bool result = false;
            using (FWBS.OMS.UI.Windows.Admin.frmPasswordRequest pas = new FWBS.OMS.UI.Windows.Admin.frmPasswordRequest(licensemissing))
            {
                var check = pas.ShowDialog();
                if (check == DialogResult.OK)
                {
                    CreateCaptainsLogEntry(logmessage);
                    result = true;
                }
                else
                {
                    radTreeView1.SelectedNode = null;
                }
            }
            return result;
		}


		private void CreateCaptainsLogEntry(string logmessage)
		{
			FWBS.OMS.Logging.CaptainsLog.CreateEntry(8, logmessage, null, "", false);
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

		#endregion	


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
			this.objText = FWBS.OMS.CodeLookup.GetLookup("ADMINMENU", objDesc);
		}
		
  
		internal long objID { get; set; }
		internal string objDesc { get; set; }
		internal string objCode { get; set; }
		internal bool objSystem { get; set; }
		internal string objRoles { get; set; }
		internal string objText { get; set; }
	}

	#endregion		

}