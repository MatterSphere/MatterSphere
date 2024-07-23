using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls.UI;


namespace MSCacheMgr
{
    static class CacheManager
    {
        internal static int CountNumberOfFileToDelete(IEnumerable<RadTreeNode> selectedNodes)
        {
            int totalNumberOfFiles = 0;

            try
            {
                foreach (RadTreeNode node in selectedNodes)
                {
                    DirectoryInfo dir = new DirectoryInfo(((CacheFolderTagData)node.Tag).CacheDirectoryPath);
                    totalNumberOfFiles += dir.GetFiles("*.*", SearchOption.AllDirectories).Length;
                }
            }
            catch
            {
                throw new ApplicationException(Properties.Resources.Error_calculating_the_number_of_files_to_delete);
            }

            return totalNumberOfFiles;
        }


        internal static void DeleteSelectedCacheFolders(IEnumerable<RadTreeNode> selectedNodes, bool writeEvents, ProgressBar progressBar)
        {
            progressBar.Value = 0;
            progressBar.Maximum = CountNumberOfFileToDelete(selectedNodes); 
            List<RadTreeNode> deletedNodes = new List<RadTreeNode>();

            foreach (RadTreeNode node in selectedNodes)
            {
                try
                {
                    CacheFolderTagData data = (CacheFolderTagData)node.Tag;
                    DirectoryInfo dir = new DirectoryInfo(data.CacheDirectoryPath);
                    FileInfo[] files = dir.GetFiles("*.*", SearchOption.AllDirectories);

                    DeleteFiles(files, data.ConnectionText, node.Text, dir, writeEvents, progressBar);
                    RemoveLocalDocumentCacheXML(node.Text);
                    if (CheckDirectoryRemoval(dir))
                        deletedNodes.Add(node);
                }
                catch {
                }
            }
            TreeManager.RemoveNodeFromTreeView(deletedNodes);
        }


        private static void DeleteFiles(FileInfo[] files, string connectionText, string cacheFolderName, DirectoryInfo cacheFolder, bool writeEvents, ProgressBar progressBar)
        {
            foreach (FileInfo file in files)
            {
                DeleteFile(file, connectionText, cacheFolderName, cacheFolder, writeEvents, progressBar);
            }
        }


        private static void DeleteFile(FileInfo file, string connectionText, string cacheFolderName, DirectoryInfo cacheFolder, bool writeEvents, ProgressBar progressBar)
        {
            progressBar.PerformStep();
            Application.DoEvents();
            ProcessFileDeletion(file, connectionText, cacheFolderName, cacheFolder, writeEvents);
        }


        private static void ProcessFileDeletion(FileInfo file, string connectionText, string cacheFolderName, DirectoryInfo cacheFolder, bool writeEvents)
        {
            try
            {
                file.Delete();
                if (writeEvents)
                    Logging.LogEvent(file.FullName + " Deleted.", EventLogEntryType.Information);
                Results.CreateListViewItem(connectionText, cacheFolderName, file.Directory.FullName, file.FullName, cacheFolder);
            }
            catch (Exception ex)
            {
                if (writeEvents)
                    Logging.LogEvent("Error deleting " + file.FullName + " " + ex.Message, EventLogEntryType.Error);
                Results.CreateListViewItem(connectionText, cacheFolderName, file.Directory.FullName, file.FullName, cacheFolder, false);
            }
        }

        private static void RemoveLocalDocumentCacheXML(string cacheFolderName)
        {
            if (cacheFolderName.Equals("Documents", StringComparison.InvariantCultureIgnoreCase))
            {
                bool removeLocalDocumentCache = FWBS.Common.ConvertDef.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["RemoveLocalDocumentCacheXML"], false);
                if (!removeLocalDocumentCache)
                {
                    try
                    {   // Resynchronize LocalDocumentCache with deleted files
                        using (var localDocumentCache = new FWBS.OMS.DocumentManagement.Storage.LocalDocumentCache()) { }
                    }
                    catch
                    {
                        removeLocalDocumentCache = true;
                    }
                }

                if (removeLocalDocumentCache)
                {
                    string localDocumentCacheXml = Path.Combine(AppData.RootCachePath, FWBS.OMS.DocumentManagement.Storage.LocalDocumentCache.DatabaseName);
                    if (File.Exists(localDocumentCacheXml))
                        File.Delete(localDocumentCacheXml);
                }
            }
        }


        private static bool CheckDirectoryRemoval(DirectoryInfo directoryInfo)
        {
            if(directoryInfo.Exists)
            {
                directoryInfo.Delete(true);
                CheckParentDirectoryForFolders(directoryInfo);
                return true;
            }
            return false;
        }


        private static void CheckParentDirectoryForFolders(DirectoryInfo directory)
        {
            if (!directory.Parent.EnumerateDirectories().Any())
                directory.Parent.Parent.Delete(true);
        }


        internal static void ReRunFileDeletion(ListView lstResults, bool writeEvents, ProgressBar progressBar)
        {
            try
            {
                progressBar.Value = 0;
                ListView tempListView = new ListView();
                foreach (ListViewItem item in lstResults.Items)
                {
                    if (!((ListViewItemTagData)item.Tag).Deleted)
                        tempListView.Items.Add((ListViewItem)item.Clone());
                }
                progressBar.Maximum = tempListView.Items.Count;
                lstResults.Items.Clear();

                ProcessClonedListViewItems(writeEvents, tempListView, progressBar);
                TreeManager.RebuildTreeView();
                tempListView.Dispose();
            }
            catch {
            }
        }

        /// <summary>
        /// itm.SubItems[0].Text = connection name
        /// itm.SubItems[1].Text = cache folder name
        /// itm.SubItems[2].Text = file path
        /// </summary>
        private static void ProcessClonedListViewItems(bool writeEvents, ListView tempListView, ProgressBar progressBar)
        {
            try
            {
                foreach (ListViewItem itm in tempListView.Items)
                {
                    FileInfo fileinfo = new FileInfo(itm.SubItems[2].Text);
                    DeleteFile(fileinfo, itm.SubItems[0].Text, itm.SubItems[1].Text, ((ListViewItemTagData)itm.Tag).CacheDirectory, writeEvents, progressBar);
                }

                foreach(ListViewItem itm in tempListView.Items)
                {
                    RemoveLocalDocumentCacheXML(itm.SubItems[1].Text);
                    CheckDirectoryRemoval(((ListViewItemTagData)itm.Tag).CacheDirectory);
                }
            }
            catch {
            }
        }

    }
}