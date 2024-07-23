using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace MSCacheMgr
{
    class TreeManager
    {
        #region Fields

        private static FWBS.OMS.Data.DatabaseConnections dbConnections;

        private static List<CacheFolderTagData> cachedConnections;

        private static DirectoryInfo root;

        private static RadTreeView treeView;

        #endregion


        private static void InitialiseData()
        {
            cachedConnections = new List<CacheFolderTagData>();
            dbConnections = AppData.DbConnections;
            root = new DirectoryInfo(AppData.RootCachePath);

            if (root.Exists)
                ReadAllCaches();
        }


        internal static void BuildConnectionFolderTree(RadTreeView radTreeView)
        {
            treeView = radTreeView;
            try
            {
                InitialiseData();
                int conCount = dbConnections.Count;
                RadTreeNode rootNode = CreateRootNode(GetRootNodeText(conCount));

                for (int i = 0; i < conCount; i++)
                {
                    CreateConnectionNode(rootNode, dbConnections[i]);
                }

                conCount = cachedConnections.Count;
                if (conCount > 0)
                {
                    rootNode = CreateRootNode(GetAbandonedNodeText(conCount));
                    for (int i = 0; i < conCount; i++)
                    {
                        CreateConnectionNode(rootNode, cachedConnections[i]);
                    }
                    cachedConnections.Clear();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, Properties.Resources.TreeView_Initialisation_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            treeView.ExpandAll(treeView.Nodes);
        }


        private static void ReadAllCaches()
        {
            foreach (DirectoryInfo id in root.EnumerateDirectories())
            {
                foreach (DirectoryInfo server in id.EnumerateDirectories())
                {
                    foreach (DirectoryInfo database in server.EnumerateDirectories())
                    {
                        var item = new CacheFolderTagData() { CacheDirectoryPath = database.FullName, ConnectionText = $"{server.Name} / {database.Name}" };
                        cachedConnections.Add(item);
                    }
                }
            }
        }


        private static string GetRootNodeText(int conCount)
        {
            switch (conCount)
            {
                case 0:
                    return Properties.Resources.No_Existing_Connections;
                case 1:
                    return Properties.Resources.Existing_Connection;
                default:
                    return Properties.Resources.Existing_Connections;
            }
        }

        private static string GetAbandonedNodeText(int conCount)
        {
            return conCount == 1
                ? Properties.Resources.Abandoned_Connection
                : Properties.Resources.Abandoned_Connections;
        }

        private static RadTreeNode CreateRootNode(string rootNodeText)
        {
            RadTreeNode rootNode = new RadTreeNode(rootNodeText);
            treeView.Nodes.Add(rootNode);
            return rootNode;
        }


        private static void CreateConnectionNode(RadTreeNode rootNode, FWBS.OMS.Data.DatabaseSettings connection)
        {
            DirectoryInfo connectionFolder = new DirectoryInfo(AppData.GetDBAppDataPath(connection));
            RadTreeNode connectionNode = new RadTreeNode(connection.Description);
            rootNode.Nodes.Add(connectionNode);

            int i = cachedConnections.FindIndex(c => c.CacheDirectoryPath.Equals(connectionFolder.FullName));
            if (i >= 0)
            {
                cachedConnections.RemoveAt(i);
                DrawCacheFolderNodes(connectionNode, connectionFolder.EnumerateDirectories());
            }
        }

        
        private static void CreateConnectionNode(RadTreeNode rootNode, CacheFolderTagData abandoned)
        {
            DirectoryInfo connectionFolder = new DirectoryInfo(abandoned.CacheDirectoryPath);
            RadTreeNode connectionNode = new RadTreeNode(abandoned.ConnectionText) { Tag = abandoned };
            rootNode.Nodes.Add(connectionNode);
            DrawCacheFolderNodes(connectionNode, connectionFolder.EnumerateDirectories());
        }


        private static void DrawCacheFolderNodes(RadTreeNode connectionNode, IEnumerable<DirectoryInfo> cacheFolders)
        {
            foreach (DirectoryInfo folder in cacheFolders)
            {
                RadTreeNode cacheFolder = new RadTreeNode(folder.Name);
                cacheFolder.CheckType = CheckType.CheckBox;
                cacheFolder.Tag = new CacheFolderTagData() { CacheDirectoryPath = folder.FullName, ConnectionText = connectionNode.Text };
                connectionNode.Nodes.Add(cacheFolder);
            }
        }


        internal static int NumberOfSelectedNodes()
        {
            IEnumerable<RadTreeNode> selectedNodes = CollectSelectedNodes(treeView.Nodes);
            return selectedNodes.Count();
        }


        public static IEnumerable<RadTreeNode> CollectSelectedNodes(RadTreeNodeCollection nodes)
        {
            try
            {
                foreach (RadTreeNode node in nodes)
                {
                    if (node.Checked)
                    {
                        yield return node;
                    }

                    foreach (var child in CollectSelectedNodes(node.Nodes))
                        if (child.Checked)
                        {
                            yield return child;
                        }
                }
            }
            finally { }
        }


        public static void RemoveNodeFromTreeView(List<RadTreeNode> deletedNodes)
        {
            foreach(RadTreeNode node in deletedNodes)
            {
                RadTreeNode parentNode = node.Parent;
                DeleteRadTreeNodeFromTree(node);
                CheckForParentNodeRemoval(parentNode);
            }
        }


        private static void CheckForParentNodeRemoval(RadTreeNode parentNode)
        {
            if (parentNode.Nodes.Count == 0 && parentNode.Tag != null)
            {
                DeleteRadTreeNodeFromTree(parentNode);
                ResetAbandonedRootNode();
            }
        }


        private static void DeleteRadTreeNodeFromTree(RadTreeNode node)
        {
            treeView.SelectedNode = node;
            treeView.SelectedNode.Remove();
            treeView.Refresh();
        }


        public static void RebuildTreeView()
        {
            treeView.Nodes.Clear();
            BuildConnectionFolderTree(treeView);
        }


        private static void ResetAbandonedRootNode()
        {
            if (treeView.Nodes.Count > 1)
            {
                RadTreeNode rootNode = treeView.Nodes[1];
                if (rootNode.Nodes.Count > 0)
                {
                    rootNode.Text = GetAbandonedNodeText(rootNode.Nodes.Count);
                    treeView.Refresh();
                }
                else
                {
                    DeleteRadTreeNodeFromTree(rootNode);
                }
            }
        }
    }
}
