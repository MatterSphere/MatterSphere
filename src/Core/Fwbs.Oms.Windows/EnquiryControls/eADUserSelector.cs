using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public partial class eADUserSelector : UserControl
    {
        public eADUserSelector()
        {
            InitializeComponent();
        }


        private frmHourGlass _busyDialog;

        public eCLCollectionSelector CollectionSelector { get; set; }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var de = (Parent as EnquiryForm)?.Enquiry?.InDesignMode == true ? null : new DirectoryEntry();

            //Read the root:
            if (de != null)
            {
                tvGroups.Nodes.Clear();

                TreeNode rootNode = tvGroups.Nodes.Add(GetNodeName(de.Name));
                rootNode.Tag = de;
                tvGroups.SelectedNode = rootNode;
            }
        }



        private static string GetNodeName(string val)
        {
            if (val.Length < 3 || val[2] != '=')
                return val;

            return val.Remove(0, 3);
        }
        
        private void PopulateChildNodes(List<DirectoryEntry> entries)
        {
            TreeNode selectedNode = tvGroups.SelectedNode;
            if (selectedNode == null || selectedNode.Nodes.Count > 0)
                return;

            tvGroups.BeginUpdate();

            foreach (DirectoryEntry de in entries)
            {
                TreeNode childNode = selectedNode.Nodes.Add(GetNodeName(de.Name));
                childNode.Tag = de;
            }

            selectedNode.Expand();
            tvGroups.EndUpdate();
        }
        
        private void DisposeTreeNodes(TreeNode treeNode)
        {
            TreeNodeCollection treeNodes = (treeNode == null) ? tvGroups.Nodes : treeNode.Nodes;

            foreach (TreeNode tn in treeNodes)
            {
                DirectoryEntry de = tn.Tag as DirectoryEntry;
                if (de != null)
                {
                    de.Dispose();
                    tn.Tag = null;
                }
                DisposeTreeNodes(tn);
            }
        }

        private void tvGroups_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (tvGroups.SelectedNode == null)
                return;

            PopulateUsers(new DataTable());
            backgroundWorker.RunWorkerAsync(e.Node.Tag);
            ShowBusyDialog(true);
        }

        private void ShowBusyDialog(bool show)
        {
            if (show)
            {
                _busyDialog = new frmHourGlass(this);
                _busyDialog.ShowDialog(ParentForm);
            }
            else if (_busyDialog != null)
            {
                _busyDialog.Close();
                _busyDialog = null;
            }
        }

        private void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            FWBS.Common.ActiveDirectoryInfo info = new FWBS.Common.ActiveDirectoryInfo();
            string overrideDomainName = Convert.ToString(Session.CurrentSession.GetSpecificData("ADSECDOMAIN"));
            if (!string.IsNullOrEmpty(overrideDomainName))
                info.OverrideDomain = overrideDomainName;

            List<DirectoryEntry> otherEntries;
            e.Result = info.GetUsersInDirEx((DirectoryEntry)e.Argument, out otherEntries);

            if (otherEntries.Count > 0)
                tvGroups.BeginInvoke(new Action<List<DirectoryEntry>>(PopulateChildNodes), otherEntries);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            ShowBusyDialog(false);
            if (e.Error != null)
            {
                ErrorBox.Show(ParentForm, e.Error);
            }
            else
            {
                DataTable result = e.Result as DataTable;
                if (result != null)
                    PopulateUsers(result);
            }
        }

        private void PopulateUsers(DataTable tblUsers)
        {
            if (CollectionSelector != null)
            {
                CollectionSelector.Value = DBNull.Value;
                CollectionSelector.AddItem(tblUsers);
            }
        }


    }
}
