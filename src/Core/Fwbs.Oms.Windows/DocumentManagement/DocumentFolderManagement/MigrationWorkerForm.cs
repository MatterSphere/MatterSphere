using System;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.DocumentManagement.Addins
{
    partial class DMTreeViewXMLManager
    {
        public static bool RunMigration(IWin32Window owner, FileType fileType)
        {
            if (fileType.MigrateWalletsToFoldersOnSave)
            {
                var documentFolderRepository = new DocumentFolderRepositoryXML();
                var dataTable = documentFolderRepository.GetFilesWithoutFolderTreeByType(fileType.Code);
                if (dataTable.Rows.Count > 0)
                {
                    using (var migrationWorker = new MigrationWorkerForm(dataTable))
                    {
                        migrationWorker.ShowDialog(owner);
                    }
                    return true;
                }

                fileType.MigrateWalletsToFoldersOnSave = false;
            }
            return false;
        }
    }

    class MigrationWorkerForm : frmProgress
    {
        private DataTable _dataTable;

        private BackgroundWorker _backgroundWorker;

        private readonly string _labelText;

        internal MigrationWorkerForm(DataTable dataTable)
        {
            _dataTable = dataTable;
            CanCancel = true;
            CancelMessageBoxText = Session.CurrentSession.Resources
                .GetMessage("MSGTXTCNCLMGR",
                    "Are you sure you want to stop this operation? Progress saved. Migration can be resumed later.",
                    "").Text;
            CancelMessageBoxTitle = Session.CurrentSession.Resources
                .GetMessage("MSGTTLCNCLMGR", "Wallets to folders migration", "")
                .Text;
            var rowCount = _dataTable.Rows.Count;
            ProgressBar.Maximum = rowCount;
            _labelText = Session.CurrentSession.Resources.GetMessage("PRGRMGRWTFDLBL", "Migrating: %1% of %2%.", string.Empty, "{0}", rowCount.ToString()).Text;
            _backgroundWorker = new BackgroundWorker { WorkerReportsProgress = true };
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += _backgroundWorker_ProgressChanged;
            _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _backgroundWorker.RunWorkerAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_backgroundWorker != null)
                {
                    _backgroundWorker.Dispose();
                    _backgroundWorker = null;
                }
                if (_dataTable != null)
                {
                    _dataTable.Dispose();
                    _dataTable = null;
                }
            }
            base.Dispose(disposing);
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            using (var treeView = new Telerik.WinControls.UI.RadTreeView { ImageList = DMTreeViewManager.DocumentFolderImageList() })
            {
                var rowCount = _dataTable.Rows.Count;
                for (int i = 0; i < rowCount && !Cancel; i++)
                {
                    _backgroundWorker.ReportProgress(0, i + 1);
                    var fileId = Convert.ToInt64(_dataTable.Rows[i]["fileId"].ToString());
                    var builder = DocumentFolderFactory.GetBuilder(typeof(OMSFile));
                    builder.Build(fileId, treeView, string.Empty, false);
                    treeView.Nodes.Clear();
                }
            }
        }

        private void _backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var counter = Convert.ToInt32(e.UserState);
            Label = string.Format(_labelText, counter);
            ProgressBar.Value = counter;
        }

        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Close();
        }
    }
}
