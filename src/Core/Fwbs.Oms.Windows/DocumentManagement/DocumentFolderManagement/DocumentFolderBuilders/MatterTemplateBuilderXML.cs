using FWBS.OMS.UI.DocumentManagement.Addins;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    class MatterTemplateBuilderXML : IDocumentFolderBuilder
    {
        protected string tableName = "dbFileFolderTreeTemplates";
        protected string procedureName = "sprRetrieveFileFolderTemplate";

        public void Build(string code, RadTreeView treeView, string rootString)
        {
            DMTreeViewXMLManager dmtvManager = new DMTreeViewXMLManager(treeView);
            string xml = new DocumentFolderRepositoryXML().Get(tableName, code, procedureName);
            if (!string.IsNullOrWhiteSpace(xml))
            {
                dmtvManager.DeserialiseXML(xml, rootString, false);
            }
            else
            {
                MatterTemplateSaverXML saverXML = new MatterTemplateSaverXML();
                xml = new DocumentFolderRepositoryXML().CreateDefaultTreeXML();
                dmtvManager.DeserialiseXML(xml, rootString, false);
                IDocumentFolderSaver saver = DocumentFolderFactory.GetSaver(saverXML.GetType());
                saver.Save(code, treeView);
            }
        }

        public void Build(long ID, RadTreeView treeView, string rootString, bool useCheckBox)
        {
            
        }
    }
}
