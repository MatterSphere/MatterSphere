using System;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    public class DefaultTreeMapper : IFolderMapper
    {
        long fileID;
        Guid emailGUID;
        Guid correspondenceGUID;

        public DefaultTreeMapper(long id, Guid CorrespondenceGUID, Guid EmailGUID)
        {
            fileID = id;
            emailGUID = EmailGUID;
            correspondenceGUID = CorrespondenceGUID;
        }

        public void Map()
        {
            DocumentFolderRepositoryXML repository = new DocumentFolderRepositoryXML();
            repository.AutoAssignFolderGUID(fileID, new Guid(Convert.ToString(correspondenceGUID)), new Guid(Convert.ToString(emailGUID)));
        }
    }
}
