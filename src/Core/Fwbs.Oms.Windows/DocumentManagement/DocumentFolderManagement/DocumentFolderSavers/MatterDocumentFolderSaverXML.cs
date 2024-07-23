using FWBS.OMS.UI.DocumentManagement.Addins;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    public class MatterDocumentFolderSaverXML : IDocumentFolderSaver
    {
        readonly string tableName = "dbFileFolderTreeData";
        readonly string procedureName = "sprStoreFileFolderTree";

        public void Save(long id, RadTreeView treeView)
        {
            var dmtvManager = new DMTreeViewXMLManager(treeView);
            var xml = dmtvManager.SerializeTreeView();
            new DocumentFolderRepositoryXML().Save(tableName, id, xml, procedureName);
        }

        public void Save(string description, RadTreeView treeView) { }
    }
}
