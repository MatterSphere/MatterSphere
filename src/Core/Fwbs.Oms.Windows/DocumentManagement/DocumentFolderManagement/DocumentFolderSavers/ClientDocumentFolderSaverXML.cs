using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    public class ClientDocumentFolderSaverXML : IDocumentFolderSaver
    {
        public void Save(long id, RadTreeView treeView)
        {
            /*It needs to be decided whether or not users will be allowed 
             * to amend Matter trees within the Client-level document treeview*/
        }

        public void Save(string description, RadTreeView treeView) { }
    }
}
