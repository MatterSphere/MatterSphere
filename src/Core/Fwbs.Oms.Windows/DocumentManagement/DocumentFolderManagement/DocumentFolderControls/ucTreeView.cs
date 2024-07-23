using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement.DocumentFolderControls
{
    public partial class ucTreeView : UserControl
    {

        private ucTreeViewArgs _treeViewArgs;

        public EventHandler Close;
        public event EventHandler Apply;

        private void OnApply()
        {
            _treeViewArgs.SelectedFolder = ucMatterFoldersTree.SelectedFolderGuid;
            _treeViewArgs.SelectedFolderText = ucMatterFoldersTree.GetFolderDescriptionByGuid(ucMatterFoldersTree.SelectedFolderGuid);

            if (Apply != null)
            {
                Apply(this, EventArgs.Empty);
            }
        }

        private void OnClose()
        {
            if (Close != null)
                Close(this, EventArgs.Empty);
        }

        public ucTreeView(ucTreeViewArgs treeViewArgs)
        {
            InitializeComponent();
            _treeViewArgs = treeViewArgs;
            SetupControlUI();
        }
        
        private void SetupControlUI()
        {
            this.chkApplyToAll.Visible = _treeViewArgs.IncludeApplyToAllCheckBox;
            if (!chkApplyToAll.Visible)
            {
                this.btnClose.Dispose();
                this.chkApplyToAll.Dispose();
                this.btnApply.Dispose();
                this.pnlOptions.Dispose();
            }

            SetupTreeView();

            if (!string.IsNullOrWhiteSpace(_treeViewArgs.ApplyToAllText))
                this.chkApplyToAll.Text = _treeViewArgs.ApplyToAllText;
        }


        private void SetupTreeView()
        {
            ucMatterFoldersTree.InitializeTreeView(_treeViewArgs.OMSFile);
            ucMatterFoldersTree.SelectedFolderGuid = _treeViewArgs.SelectedFolder;
        }

        public string GetFolderDescriptionByGuid(Guid guid)
        {
            return ucMatterFoldersTree.GetFolderDescriptionByGuid(guid);
        }

        private void chkApplyToAll_Click(object sender, EventArgs e)
        {
            _treeViewArgs.ApplyToAllValue = this.chkApplyToAll.Checked;
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            OnApply();
        }

        private void UcMatterFoldersTreeSelectedNodeChangedEvent(object sender, System.EventArgs e)
        {
            this.btnApply.Enabled = !(ucMatterFoldersTree.SelectedFolderGuid == Guid.Empty);
        }

    }
}