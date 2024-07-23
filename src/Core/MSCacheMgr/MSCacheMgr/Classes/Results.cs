using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MSCacheMgr
{
    static class Results
    {
        private static ListView ListView;

        internal static void ConfigureListViewControl(ListView listView)
        {
            ListView = listView;
        }


        internal static void CreateListViewItem(string connection, string folder, string directorypath, string filepath, DirectoryInfo cacheFolder, bool deleted = true)
        {
            string[] row = { connection, folder, filepath };
            ListViewItem itm = new ListViewItem(row);
            if (!deleted)
            {
                itm.ForeColor = Color.Red;
                itm.Group = ListView.Groups[0];
            }
            else
            {
                itm.Group = ListView.Groups[1];
            }
            itm.Tag = new ListViewItemTagData() { CacheDirectory = cacheFolder, FileDirectoryPath = directorypath, Deleted = deleted };

            ListView.Items.Add(itm);
        }


        internal static void OpenListViewItemFileLocation()
        {
            try
            {
                if (Directory.Exists(((ListViewItemTagData)ListView.SelectedItems[0].Tag).FileDirectoryPath))
                    Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Windows), "Explorer.exe"), ((ListViewItemTagData)ListView.SelectedItems[0].Tag).FileDirectoryPath);
                else
                    MessageBox.Show(Properties.Resources.Directory_not_exists, Properties.Resources.Directory_Access, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }
    }
}
