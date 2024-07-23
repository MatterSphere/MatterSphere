using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.UI.Windows;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    public class DMTreeViewManager
    {
        RadTreeView _treeView;

        public event EventHandler<EventArgs> FolderTreeChanged;


        public DMTreeViewManager(RadTreeView TreeView)
        {
            _treeView = TreeView;
            _treeView.DragEnded += new RadTreeView.DragEndedHandler(TreeView_DragEnded);
        }


        private void OnFolderTreeChanged()
        {
            if (FolderTreeChanged != null)
                FolderTreeChanged(this, new EventArgs());
        }


        public static ImageList DocumentFolderImageList()
        {
            ImageList treeImageList = new ImageList();
            ImageList coolButtons16Images = FWBS.OMS.UI.Windows.Images.CoolButtons16();
            treeImageList.Images.Add("Folder", coolButtons16Images.Images[58]);
            treeImageList.Images.Add("Document", coolButtons16Images.Images[0]);
            return treeImageList;
        }


        #region TreeView ContextMenu

        private bool _checkbox = true;
        private RadContextMenu treeContextMenu;
        private RadContextMenu rootContextMenu;

        public void SetupTreeContextMenu(bool checkbox)
        {
            //build tree context menu
            _checkbox = checkbox;
            treeContextMenu = new RadContextMenu();
            rootContextMenu = new RadContextMenu();
            SetupContextMenu(treeContextMenu);
            _treeView.SelectedNode = _treeView.Nodes[0];
            SetupRootNodeContextMenu(rootContextMenu);
            if (rootContextMenu.Items.Count > 0)
            {
                _treeView.SelectedNode.ContextMenu = rootContextMenu;
            }
            foreach (RadTreeNode n in _treeView.SelectedNode.Nodes)
            {
                if (treeContextMenu.Items.Count > 0)
                    n.ContextMenu = treeContextMenu;

                if (n.Nodes.Count > 0 && treeContextMenu.Items.Count > 0)
                {
                    foreach (RadTreeNode node in n.Nodes)
                    {
                        AssignContextMenu(node);
                    }
                }
            }
        }

        private void SetupContextMenu(Telerik.WinControls.UI.RadContextMenu treeContextMenu)
        {
            if (treeContextMenu.Items.Count == 0)
            {
                if(Session.CurrentSession.CurrentUser.IsInRoles(new string [] { "AddDMTVFolder","ADMIN"}))
                    treeContextMenu.Items.Add(CreateNewFolder("NEWTVFOLDER"));
                if(Session.CurrentSession.CurrentUser.IsInRoles(new string [] { "EditDMTVFolder","ADMIN"}))
                    treeContextMenu.Items.Add(EditFolder("EDITTVFOLDER"));
                if (Session.CurrentSession.CurrentUser.IsInRoles(new string[] { "DelDMTVFolder", "ADMIN" }))
                    treeContextMenu.Items.Add(DeleteFolder("DELETETVFOLDER"));
            }
        }

        private void SetupRootNodeContextMenu(Telerik.WinControls.UI.RadContextMenu rootContextMenu)
        {
            if (rootContextMenu.Items.Count == 0)
            {
                if (Session.CurrentSession.CurrentUser.IsInRoles(new string[] { "AddDMTVFolder", "ADMIN" }))
                    rootContextMenu.Items.Add(CreateNewFolder("NEWTVFOLDER"));
            }
        }

        private void AssignContextMenu(RadTreeNode node)
        {
            node.ContextMenu = treeContextMenu;
            foreach (RadTreeNode n in node.Nodes)
            {
                if (n.Nodes.Count == 0)
                {
                    if (treeContextMenu.Items.Count > 0)
                    {
                        n.ContextMenu = treeContextMenu;
                    }
                }
                else
                    AssignContextMenu(n);
            }
        }



        private RadMenuItem CreateNewFolder(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem addfolder = new Telerik.WinControls.UI.RadMenuItem();
            addfolder.Text = ResourceLookup.GetLookupText(lookupText);
            addfolder.MouseUp += new MouseEventHandler(addfolder_MouseUp);

            return addfolder;
        }


        private RadMenuItem EditFolder(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem editfolder = new Telerik.WinControls.UI.RadMenuItem();
            editfolder.Text = ResourceLookup.GetLookupText(lookupText);
            editfolder.MouseUp += new MouseEventHandler(editfolder_MouseUp);
            return editfolder;
        }


        private RadMenuItem DeleteFolder(string lookupText)
        {
            Telerik.WinControls.UI.RadMenuItem deletefolder = new Telerik.WinControls.UI.RadMenuItem();
            deletefolder.Text = ResourceLookup.GetLookupText(lookupText);
            deletefolder.MouseUp += new MouseEventHandler(deletefolder_MouseUp);
            return deletefolder;
        }

        private void TreeView_DragEnded(object sender, RadTreeViewDragEventArgs e)
        {
            OnFolderTreeChanged();
        }

        private void addfolder_MouseUp(object sender, MouseEventArgs e)
        {
            if (AddTreeViewFolder(_checkbox, FolderActivities.AddFolder))
                OnFolderTreeChanged();
        }


        private void editfolder_MouseUp(object sender, MouseEventArgs e)
        {
            if (UpdateCodeLookupForFolder(FolderActivities.EditFolder))
                OnFolderTreeChanged();
        }


        private void deletefolder_MouseUp(object sender, MouseEventArgs e)
        {
            if (DeleteTreeViewFolder())
                OnFolderTreeChanged();
        }


        private bool AddTreeViewFolder(bool useCheckBox, FolderActivities activity)
        {
            string code = GetFolderCode(activity);
            if (code != null)
            {
                RadTreeNode newNode = Windows.TreeViewNavigation.TreeViewFormatter.NewTreeNode();
                newNode.Text = CodeLookup.GetLookup("DFLDR_MATTER", code);
                newNode.Image = DocumentFolderImageList().Images[0];
                newNode.Tag = new DMTreeNodeTagData() { system = false, folderGUID = Guid.NewGuid(), folderCode = code };

                if (useCheckBox)
                {
                    newNode.CheckType = CheckType.CheckBox;
                }
                
                newNode.ContextMenu = treeContextMenu;
                _treeView.SelectedNode.Nodes.Add(newNode);
                _treeView.Refresh();
                return true;
            }
            return false;
        }


        public static string CreateNewCodeLookupForFolder(string folderDescription, string codeLookupType)
        {
            CodeLookup newFolderCode = CodeLookup.Create(codeLookupType, folderDescription.GetHashCode().ToString(), folderDescription, null, Session.CurrentSession.CurrentUser.PreferedCulture, true, true, true);
            return newFolderCode.Code;
        }


        private bool UpdateCodeLookupForFolder(FolderActivities activity)
        {
            string code = GetFolderCode(activity);
            if (code != null)
            {
                SetFolderCode(code);
                _treeView.SelectedNode.Text = CodeLookup.GetLookup("DFLDR_MATTER", code);
                _treeView.Refresh();
                return true;
            }
            return false;
        }


        internal string GetFolderCode(FolderActivities activity)
        {
            KeyValueCollection kvc = new KeyValueCollection();

            List<string> nodeDescriptions = GetNodeDescriptionsAtSelectedLevel(activity);
            if (nodeDescriptions.Count > 0)
                kvc.Add("nodesAtLevel", nodeDescriptions);

            DataTable dtFolderCode = (DataTable)FWBS.OMS.UI.Windows.Services.ShowOMSItem("sEditFDesc", null, FWBS.OMS.EnquiryEngine.EnquiryMode.Edit, kvc);
            if (dtFolderCode != null && dtFolderCode.Rows.Count > 0)
            {
                if(!Convert.ToBoolean(dtFolderCode.Rows[0]["DuplicateFolderName"]))
                    return Convert.ToString(dtFolderCode.Rows[0]["cboDescription"]);
            }
            return null;
        }


        private List<string> GetNodeDescriptionsAtSelectedLevel(FolderActivities activity)
        {
            if (activity == FolderActivities.AddFolder)
                return _treeView.SelectedNode.Nodes.Select(n => n.Text).ToList();
            else
                return _treeView.SelectedNode.Parent.Nodes.Select(n => n.Text).ToList();
        }


        public enum FolderActivities
        {
            AddFolder,
            EditFolder,
            DeleteFolder
        }


        private void SetFolderCode(string code)
        {
            DMTreeNodeTagData tag = (DMTreeNodeTagData)_treeView.SelectedNode.Tag;
            tag.folderCode = code;
            _treeView.SelectedNode.Tag = tag;
        }


        internal string GetTagFolderCode(RadTreeNode node)
        {
            DMTreeNodeTagData tag = (DMTreeNodeTagData)node.Tag;
            return tag.folderCode;
        }


        internal void SetTagFolderCode(string code)
        {
            DMTreeNodeTagData tag = (DMTreeNodeTagData)_treeView.SelectedNode.Tag;
            tag.folderCode = code;
        }


        internal Guid GetTagFolderGUID(RadTreeNode node)
        {
            DMTreeNodeTagData tag = (DMTreeNodeTagData)node.Tag;
            if(tag != null)
                return tag.folderGUID;
            else
                return Guid.Empty;
        }

        internal string GetTagDocWallets(RadTreeNode node)
        {
            DMTreeNodeTagData tag = (DMTreeNodeTagData)node.Tag;
            if (tag != null)
                return tag.docWallets;
            else
                return String.Empty;
        }


        private bool DeleteTreeViewFolder()
        {
            if (IsSystemNode(_treeView.SelectedNode))
            {
                System.Windows.Forms.MessageBox.Show(
                                Session.CurrentSession.Resources.GetResource("DMTVMGR_1", "The selected folder is a system folder and cannot be deleted.", "").Text,
                                Session.CurrentSession.Resources.GetResource("DMTVMGR_2", "Document Folder Deletion", "").Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
            }
            
            if (System.Windows.Forms.MessageBox.Show(
                                Session.CurrentSession.Resources.GetResource("DMTVMGR_3", "Are you sure you want to delete the selected folder?", "").Text,
                                Session.CurrentSession.Resources.GetResource("DMTVMGR_2", "Document Folder Deletion", "").Text,
                                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (CheckTreeBranchForSystemFolders(_treeView.SelectedNode))
                {
                    System.Windows.Forms.MessageBox.Show(
                                    Session.CurrentSession.Resources.GetResource("DMTVMGR_5", "The branch you are attempting to delete includes a system folder therefore it cannot be removed.", "").Text,
                                    Session.CurrentSession.Resources.GetResource("DMTVMGR_2", "Document Folder Deletion", "").Text,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                if (CheckTreeBranchForDocuments(_treeView.SelectedNode))
                {
                    System.Windows.Forms.MessageBox.Show(
                                    Session.CurrentSession.Resources.GetResource("DMTVMGR_4", "There are documents in the selected folder or a child folder therefore it cannot be deleted.", "").Text,
                                    Session.CurrentSession.Resources.GetResource("DMTVMGR_2", "Document Folder Deletion", "").Text,
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                _treeView.SelectedNode.Remove();
                _treeView.Refresh();
                return true;
            }

            return false;
        }



        private bool IsSystemNode(RadTreeNode node)
        {
            DMTreeNodeTagData tag = (DMTreeNodeTagData)node.Tag;
            return tag.system;
        }


        private bool CheckTreeBranchForDocuments(RadTreeNode nodeToDelete)
        {
            DocumentFolderRepositoryXML repository = new DocumentFolderRepositoryXML();

            if (NodeHasDocuments(repository, nodeToDelete))
            {
                return true;
            }

            foreach (RadTreeNode node in CollectTreeNodes(nodeToDelete.Nodes))
            {
                if (NodeHasDocuments(repository, node))
                {
                    return true;
                }
            }

            return false;
        }


        private bool NodeHasDocuments(DocumentFolderRepositoryXML repository, RadTreeNode node)
        {
            return repository.CheckFolderForDocuments(GetTagFolderGUID(node));
        }


        private bool CheckTreeBranchForSystemFolders(RadTreeNode node)
        {
            foreach (RadTreeNode childnode in CollectTreeNodes(node.Nodes))
            {
                if (IsSystemNode(childnode))
                {
                    return true;
                }
            }
            return false;
        }


        IEnumerable<RadTreeNode> CollectTreeNodes(RadTreeNodeCollection nodes)
        {
            return StaticCollectTreeNodes(nodes);
        }

        #endregion


        public static TreeViewSystemNodeGuids GetSystemFolderGUIDs(RadTreeView tree)
        {
            var treeViewSystemNodeGuids = new TreeViewSystemNodeGuids();

            foreach (RadTreeNode node in StaticCollectTreeNodes(tree.Nodes))
            {
                if (IsSystemFolder(node))
                {
                    CreateSystemNode(node, treeViewSystemNodeGuids);
                }
            }

            return treeViewSystemNodeGuids;
        }


        private static IEnumerable<RadTreeNode> StaticCollectTreeNodes(RadTreeNodeCollection nodes)
        {
            foreach (RadTreeNode node in nodes)
            {
                yield return node;

                foreach (RadTreeNode child in StaticCollectTreeNodes(node.Nodes))
                    yield return child;
            }
        }


        private static bool IsSystemFolder(RadTreeNode node)
        {
            bool result = false;
            if (node.Tag != null)
            {
                DMTreeNodeTagData tagdata = (DMTreeNodeTagData)node.Tag;
                result = ConvertDef.ToBoolean(tagdata.system, false);
            }
            return result;
        }


        private static void CreateSystemNode(RadTreeNode node, TreeViewSystemNodeGuids treeViewSystemNodeGuids)
        {
            DMTreeNodeTagData tagdata = (DMTreeNodeTagData)node.Tag;

            if (tagdata.folderCode == "EMAIL")
            {
                treeViewSystemNodeGuids.EmailGuid = tagdata.folderGUID;
            }
            else if (tagdata.folderCode == "GENERAL")
            {
                treeViewSystemNodeGuids.CorrespondenceGuid = tagdata.folderGUID;
            }
            else if (tagdata.folderCode == "INVOICE")
            {
                treeViewSystemNodeGuids.InvoiceGuid = tagdata.folderGUID;
            }
        }
    }


    public class TreeViewSystemNodeGuids
    {
        public Guid CorrespondenceGuid { get; set; }
        public Guid EmailGuid { get; set; }
        public Guid InvoiceGuid { get; set; }
    }
}
