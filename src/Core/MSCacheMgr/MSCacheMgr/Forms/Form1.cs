using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace MSCacheMgr
{
    partial class Form1 : Form
    {
        private readonly List<TabPage> hiddenPages = new List<TabPage>();

        public Form1()
        {
            InitializeComponent();
            SetupApplication();
        }

        [HandleProcessCorruptedStateExceptions]
        private void SetupApplication()
        {
            try
            {
                MSCacheMgr.Results.ConfigureListViewControl(lstResults);
                TreeManager.BuildConnectionFolderTree(radTreeView);
                SetWarningMessageVisibility();
                HideResultsTab();
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(this, ex);
            }
        }

        private void btnClearCache_Click(object sender, EventArgs e)
        {
            ShowResultsTab();
            lstResults.Items.Clear();
            try
            {
                CacheManager.DeleteSelectedCacheFolders(TreeManager.CollectSelectedNodes(this.radTreeView.Nodes), this.chkWriteEvents.Checked, progressBar);
                ResetUIElements();
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(this, ex);
            }
        }


        private void radTreeView_NodeCheckedChanged(object sender, TreeNodeCheckedEventArgs e)
        {
            SetClearCacheButtonEnabledState();
            SetWarningMessageVisibility();
        }


        private void lstResults_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                if(lstResults.FocusedItem.Bounds.Contains(e.Location))
                {
                    if(lstResults.FocusedItem.Group == lstResults.Groups["FailedToDelete"])
                    {
                        contextMenuStrip.Show(Cursor.Position);
                    }
                }
            }
            SetFileLocationButtonEnabledStateOnListItemClick();
        }


        private void tsBtnFileLocation_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstResults.FocusedItem.Group == lstResults.Groups["FailedToDelete"])
                {
                    MSCacheMgr.Results.OpenListViewItemFileLocation();
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(this, ex);
            }
        }


        private void openFileLocationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                MSCacheMgr.Results.OpenListViewItemFileLocation();
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(this, ex);
            }
        }


        private void tsBtnReRun_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstResults.Groups["FailedToDelete"].Items.Count > 0)
                {
                    CacheManager.ReRunFileDeletion(lstResults, chkWriteEvents.Checked, progressBar);
                    ResetUIElements();
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(this, ex);
            }
        }


        private void rerunDeletionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CacheManager.ReRunFileDeletion(lstResults, chkWriteEvents.Checked, progressBar);
                ResetUIElements();
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(this, ex);
            }
        }


        private void HideResultsTab()
        {
            hiddenPages.Add(tbCacheMgr.TabPages[1]);
            tbCacheMgr.TabPages.RemoveAt(1);
        }


        private void ShowResultsTab()
        {
            if (hiddenPages.Count > 0)
            {
                tbCacheMgr.TabPages.Add(hiddenPages[0]);
                hiddenPages.Clear();
            }
            tbCacheMgr.SelectedTab = tbCacheMgr.TabPages[1];
        }


        private void ResetUIElements()
        {
            SetResultsToolbarButtonEnabledState();
            SetClearCacheButtonEnabledState();
            SetWarningMessageVisibility();
        }


        private void SetClearCacheButtonEnabledState()
        {
            btnClearCache.Enabled = TreeManager.NumberOfSelectedNodes() > 0;
        }


        private void SetWarningMessageVisibility()
        {
            IEnumerable<RadTreeNode> selectedNodes = TreeManager.CollectSelectedNodes(radTreeView.Nodes);
            pnlWarningBaseBorder.Visible = selectedNodes.Any(n => n.Text.Equals("Documents", StringComparison.InvariantCultureIgnoreCase));
        }


        private void SetResultsToolbarButtonEnabledState()
        {
            tsBtnReRun.Enabled = tsBtnFileLocation.Enabled = lstResults.Groups["FailedToDelete"].Items.Count != 0;
        }


        private void SetFileLocationButtonEnabledStateOnListItemClick()
        {
            tsBtnFileLocation.Enabled = (lstResults.FocusedItem.Group == lstResults.Groups["FailedToDelete"]);
        }
    }
}
