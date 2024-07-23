using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    public class TemplateFolderMapper : IFolderMapper
    {
        long fileID;
        Guid emailGUID;
        Guid correspondenceGUID;
        RadTreeView mappingTreeView;
        DataTable dtDocuments;

        public TemplateFolderMapper(Guid CorrespondenceGUID, Guid EmailGUID, RadTreeView MappingTreeView, long FileID)
        {
            fileID = FileID;
            emailGUID = EmailGUID;
            correspondenceGUID = CorrespondenceGUID;
            mappingTreeView = MappingTreeView;
        }
      
        public void Map()
        {
            DocumentFolderRepositoryXML repository = new DocumentFolderRepositoryXML();
            dtDocuments = repository.GetFileDocumentsWithoutGUIDs(fileID);
            if (dtDocuments == null || dtDocuments.Rows.Count == 0)
                return;

            bool mapped = false;
            string docWallet = "";
            IEnumerable<WalletMapping> walletMappings = BuildWalletMappingList();

            if (walletMappings.Count() == 0)
            {
                repository.AutoAssignFolderGUID(fileID, correspondenceGUID, emailGUID);
                return;
            }

            Dictionary<Guid, List<string>> docsWithoutRelatedDocs = new Dictionary<Guid, List<string>>();
            Dictionary<Guid, List<string>> docsWithRelatedDocs = new Dictionary<Guid, List<string>>();

            foreach (DataRow r in dtDocuments.Rows)
            {
                docWallet = Convert.ToString(r["DocWallet"]);
                var docId = Convert.ToString(r["docID"]);

                if (!string.IsNullOrWhiteSpace(docWallet))
                {
                    foreach (WalletMapping walletmap in walletMappings)
                    {
                        if (isWalletInWalletMapping(docWallet, walletmap.WalletMappings))
                        {
                            var folderGuid = walletmap.FolderGuid;
                            AddDocsToDictionary(docsWithoutRelatedDocs, folderGuid, docId);

                            mapped = true;
                            break;
                        }
                    }

                    if (!mapped)
                    {
                        var guid = GetGuidForDocsWithoutWallet(r);
                        AddDocsToDictionary(docsWithoutRelatedDocs, guid, docId);
                    }
                }
                else
                {
                    var guid = GetGuidForDocsWithoutWallet(r);
                    AddDocsToDictionary(docsWithRelatedDocs, guid, docId);
                }

                mapped = false;
            }
            
            AssignFolderGUIDBatch(docsWithoutRelatedDocs, repository);
            AssignFolderGUIDBatch(docsWithRelatedDocs, repository);
        }

       

        private void AddDocsToDictionary(Dictionary<Guid, List<string>> docsDictionary, Guid folderGuid, string docId)
        {
            if (!docsDictionary.ContainsKey(folderGuid))
                docsDictionary.Add(folderGuid, new List<string>());

            docsDictionary[folderGuid].Add(docId);
        }

        private Guid GetGuidForDocsWithoutWallet(DataRow r)
        {
            var extension = Convert.ToString(r["docExtension"]);
            var docId = Convert.ToString(r["docID"]);

            Guid guid = (extension.ToUpper() == ".MSG" || extension.ToUpper() == "MSG")
                ? emailGUID
                : correspondenceGUID;

            return guid;
        }

        private void AssignFolderGUIDBatch(Dictionary<Guid, List<string>> guidDocs, DocumentFolderRepositoryXML repository)
        {
            foreach (var guidDocsItem in guidDocs)
            {
                XElement xElement = new XElement("items");

                foreach (var docid in guidDocsItem.Value)
                {
                    xElement.Add(new XElement("item", docid));
                }

                repository.AssignForlderGUIDBatch(xElement.ToString(), guidDocsItem.Key, false);
            }
        }

        private IEnumerable<WalletMapping> BuildWalletMappingList()
        {
            // Build a list of WalletMapping objects from the treeView data saved in dbFileFolderTreeData
            List<WalletMapping> walletMappingList = new List<WalletMapping>();

            DMTreeViewManager dmtvManager = new DMTreeViewManager(mappingTreeView);
            foreach (RadTreeNode n in CollectMappedNodes(mappingTreeView.Nodes))
            {
                if (dmtvManager.GetTagFolderGUID(n) != Guid.Empty && !string.IsNullOrWhiteSpace(dmtvManager.GetTagDocWallets(n)))
                {
                    walletMappingList.Add(new WalletMapping()
                    {
                        FolderGuid = dmtvManager.GetTagFolderGUID(n),
                        WalletMappings = dmtvManager.GetTagDocWallets(n)
                    });
                }
            }

            return walletMappingList;
        }


        private bool isWalletInWalletMapping(string docwallet, string walletmap)
        {
            bool result = false;
            string[] maps = walletmap.Split(',');
            foreach (string map in maps)
            {
                if (map == docwallet)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }


        public static IEnumerable<RadTreeNode> CollectMappedNodes(RadTreeNodeCollection nodes)
        {
            foreach (RadTreeNode node in nodes)
            {
                if (node.Tag != null)
                {
                    yield return node;
                }

                foreach (var child in CollectMappedNodes(node.Nodes))
                    if (child.Tag != null)
                    {
                        yield return child;
                    }
            }
        }

    }
}
