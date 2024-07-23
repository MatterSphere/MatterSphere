using FWBS.OMS.UI.DocumentManagement.Addins;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    class MatterTemplateSaverXML : IDocumentFolderSaver
    {
        readonly string tableName = "dbFileFolderTreeTemplates";
        readonly string procedureName = "sprStoreNewFileFolderTreeTemplate";

        public void Save(string templateDesc, RadTreeView treeView)
        {
            var dmtvManager = new DMTreeViewXMLManager(treeView);
            var xml = dmtvManager.SerializeTreeView();
            new DocumentFolderRepositoryXML().Save(templateDesc, tableName, xml, procedureName);
        }

        public void Save(long id, RadTreeView treeView)
        {
            var dmtvManager = new DMTreeViewXMLManager(treeView);
            var xml = dmtvManager.SerializeTreeView();
            new DocumentFolderRepositoryXML().Save(tableName, id, xml, procedureName);
        }


    }
}
