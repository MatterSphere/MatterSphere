using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public class PackageDeployAutomation
    {
        public static List<ListViewItem> GetPackages(string[] paths)
        {
            List<ListViewItem> packages = new List<ListViewItem>();

            foreach (string pathString in paths)
            {
                FileInfo file = new FileInfo(pathString);

                if (!file.Exists)
                {
                    Console.WriteLine("[Automation] - Package file '" + file.FullName + "' is not exists");
                    continue;
                }

                var item = new ListViewItem() { Text = file.Name, Tag = file };
                item.SubItems.Add(file.Name);
                item.SubItems.Add("");
                item.SubItems.Add("");

                packages.Add(item);
            }

            return packages;
        }

        public static void ProcessPackages(List<ListViewItem> packages)
        {
            List<ListViewItem> failedPackages = new List<ListViewItem>();

            PackageDeploy packageDeploy = PackageDeployFactory.Create4Automation();
            packageDeploy.ActionAfterInstall = ActionAfterInstall;

            foreach (ListViewItem item in packages)
            {
                object ret;
                FileInfo packageFileInfo = (FileInfo)item.Tag;
                string filename = packageFileInfo.FullName;

                Console.WriteLine("[Automation] - '" + packageFileInfo.Name + "' package is installing" );

                SystemUpdateReturnStates status = packageDeploy.ProcessManifest(null, filename, false, false, false, false, out ret, false);

                Exception ex = ret as Exception;
                if (ex != null)
                {
                    if (ex is PackageException)
                    {
                        failedPackages.Add(item);
                        continue;
                    }
                    item.SubItems[2].Tag = ret;
                    item.SubItems[2].Text = ex.Message;

                    Console.WriteLine("[Automation] - '" + packageFileInfo.Name + "' package installation error. Error: " + ex);
                }
                else
                {
                    item.SubItems[2].Text = Convert.ToString(ret);
                }

                if (status == SystemUpdateReturnStates.Failed)
                {
                    Console.WriteLine("[Automation] - '" + packageFileInfo.Name +"' package installation failed. Details: " + Convert.ToString(ret));
                }

                if (status == SystemUpdateReturnStates.Success)
                {
                    item.ImageIndex = 1;
                    item.Checked = false;

                    Console.WriteLine("[Automation] - '" + packageFileInfo.Name + "' package is succesfully installed");
                }

                if (status == SystemUpdateReturnStates.Cancel)
                {
                    return;
                }
            }

            if (failedPackages.Count > 0)
            {
                ProcessPackages(failedPackages);
            }
        }

        private static void ActionAfterInstall(IWin32Window owner, OMS.Design.Import.Global import)
        {
            Console.WriteLine("[Automation] - Package has 'AfterInstall' wizard which cannot be launched under current mode. AfterInstall=" + import.TreeView.AfterInstall);
        }
    }
}
