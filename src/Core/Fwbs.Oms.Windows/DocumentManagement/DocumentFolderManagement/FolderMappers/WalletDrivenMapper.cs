using System;
using System.Data;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    public class WalletDrivenMapper : IFolderMapper
    {
        long fileID;
        DataTable dtWalletFolders;
        Guid correspondenceGuid;
        Guid emailGuid;


        public WalletDrivenMapper(long FileID, DataTable dtFolders, Guid correspondenceGuid, Guid emailGuid)
        {
            fileID = FileID;
            dtWalletFolders = dtFolders.Copy();
            this.correspondenceGuid = correspondenceGuid;
            this.emailGuid = emailGuid;
        }


        public void Map()
        {
            DocumentFolderRepositoryXML repository = new DocumentFolderRepositoryXML();
            repository.WalletDrivenMappingToFolders(fileID, dtWalletFolders);
            repository.AutoAssignFolderGUID(fileID, correspondenceGuid, emailGuid);
        }
    }
}
