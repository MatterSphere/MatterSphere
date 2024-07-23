using System.Windows.Forms;
using FWBS.OMS.HighQ.Models;
using FWBS.OMS.HighQ.Models.Response;

namespace FWBS.OMS.HighQ.Interfaces
{
    internal interface ITargetFolderPicker
    {
        int GetTargetFolderId(FolderInfoResponse rootFolderInfo, FolderItem[] folders, IHighQProvider provider, IWin32Window owner = null);
    }
}
