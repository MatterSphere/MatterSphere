using System;
using System.Windows.Forms;
using FWBS.OMS.HighQ.Interfaces;
using FWBS.OMS.HighQ.Models;
using FWBS.OMS.HighQ.Models.Response;

namespace FWBS.OMS.HighQ.UserControls
{
    internal partial class FolderTreeWindow : Form
    {
        private IHighQProvider _provider;
        private int? _selectedFolderId;

        public FolderTreeWindow()
        {
            InitializeComponent();

            this.Text = Session.CurrentSession.Resources.GetResource("HQFLDRTREE", "HighQ Folders", string.Empty).Text;
            this.btnUpload.Text = Session.CurrentSession.Resources.GetResource("UPLOAD", "Upload", string.Empty).Text;
            this.btnCancel.Text = Session.CurrentSession.Resources.GetResource("CANCEL", "Cancel", string.Empty).Text;

            var images = new FWBS.OMS.HighQ.Providers.Images();
            radTreeView.ImageList = new ImageList();
            radTreeView.ImageList.Images.Add(images.GetFolderImage(this.DeviceDpi));
        }

        public void SetFolders(FolderInfoResponse rootFolderInfo, FolderItem[] folders, IHighQProvider provider)
        {
            _selectedFolderId = rootFolderInfo.Id;
            _provider = provider;
            
            radTreeView.Nodes.Add(rootFolderInfo.Id.ToString(), rootFolderInfo.Name, 0);
            radTreeView.Nodes[0].Expanded = true;
            radTreeView.Nodes[0].Selected = true;

            foreach (var folder in folders)
            {
                radTreeView.Nodes[0].Nodes.Add(folder.Id.ToString(), folder.Name, 0);
            }
        }

        public int? SelectedFolderId { get; private set; }

        private void btnUpload_Click(object sender, System.EventArgs e)
        {
            if (_selectedFolderId.HasValue)
            {
                var selectedFolder = _provider.GetFolderInfo(_selectedFolderId.Value);
                if (selectedFolder.Permission.AddEditAllFiles)
                {
                    SelectedFolderId = _selectedFolderId;
                    DialogResult = DialogResult.OK;
                }
                else
                {
                    var message = Session.CurrentSession.Resources.GetResource("ACCESSDENIED", "Access denied.", string.Empty).Text;
                    var caption = Session.CurrentSession.Resources.GetResource("ERRORMESSAGE", "Error Message", string.Empty).Text;
                    MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void radTreeView1_NodeMouseClick(object sender, Telerik.WinControls.UI.RadTreeViewEventArgs e)
        {
            int id;
            Int32.TryParse(e.Node.Name, out id);
            _selectedFolderId = id;

            if (e.Node.Expanded)
            {
                e.Node.Expanded = false;
            }
            else
            {
                e.Node.Nodes.Clear();

                var folders = _provider.GetFolders(id);
                foreach (var folder in folders)
                {
                    e.Node.Nodes.Add(folder.Id.ToString(), folder.Name, 0);
                }

                e.Node.Expanded = true;
            }
        }
    }
}
