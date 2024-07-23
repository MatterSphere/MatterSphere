using System;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    using Telerik.WinControls.UI;

    class DMTreeViewManager
    {
        private RadTreeView _treeView;

        internal DMTreeViewManager(RadTreeView treeView)
        {
            _treeView = treeView;
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
                return string.Empty;
        }

        internal Guid AddInvoiceFolder()
        {
            Guid guid = Guid.NewGuid();
            RadTreeNode newNode = new RadTreeNode(CodeLookup.GetLookup("DFLDR_MATTER", "INVOICE"));
            newNode.Tag = new DMTreeNodeTagData() { system = true, systemID = "Invoice", folderCode = "INVOICE", folderGUID = guid };
            _treeView.Nodes[0].Nodes.Add(newNode);
            return guid;
        }
    }
}
