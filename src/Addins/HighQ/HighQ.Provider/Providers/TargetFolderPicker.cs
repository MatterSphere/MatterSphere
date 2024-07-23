using System;
using System.Windows.Forms;
using FWBS.OMS.HighQ.Interfaces;
using FWBS.OMS.HighQ.Models;
using FWBS.OMS.HighQ.Models.Response;
using FWBS.OMS.HighQ.UserControls;

namespace FWBS.OMS.HighQ.Providers
{
    internal class TargetFolderPicker : ITargetFolderPicker
    {
        public int GetTargetFolderId(FolderInfoResponse rootFolderInfo, FolderItem[] folders, IHighQProvider provider, IWin32Window owner = null)
        {
            using (var form = new FolderTreeWindow())
            {
                form.SetFolders(rootFolderInfo, folders, provider);
                form.ShowDialog(owner);

                if (form.DialogResult != DialogResult.OK)
                {
                    throw new OperationCanceledException();
                }

                return form.SelectedFolderId.Value;
            }
        }
    }
}
