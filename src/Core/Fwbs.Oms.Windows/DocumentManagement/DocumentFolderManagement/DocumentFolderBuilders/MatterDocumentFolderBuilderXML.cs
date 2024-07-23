using System;
using System.Data;
using FWBS.OMS.UI.DocumentManagement.Addins;
using FWBS.OMS.UI.Windows;
using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    public class MatterDocumentFolderBuilderXML : IDocumentFolderBuilder
    {
        protected RadTreeView mappingTreeView;
        protected string tableName = "dbFileFolderTreeData";
        protected string storePprocedureName = "sprStoreFileFolderTree";
        protected string buildProcedureName = "sprRetrieveFileFolderTree";

        private IFolderMapper mapper;
        internal Guid CorrespondenceGUID { get; private set; }
        internal Guid EmailGUID { get; private set; }
        internal Guid InvoiceGUID { get; private set; }

        public void Build(long id, RadTreeView treeView, string rootString, bool useCheckBox)
        {
            try
            {
                DMTreeViewXMLManager dmtvManager = new DMTreeViewXMLManager(treeView);
                string xml = new DocumentFolderRepositoryXML().Get(tableName, id, buildProcedureName);
                DataTable documentsWithoutFolderGuids = new DocumentFolderRepositoryXML().GetFileDocumentsWithoutGUIDs(id);

                if (MatterHasFolderData(xml))
                {
                    dmtvManager.DeserialiseXML(xml, rootString, useCheckBox);
                    CorrespondenceGUID = new Guid(dmtvManager.CheckTreeForSystemNode(xml, "Correspondence"));
                    EmailGUID = new Guid(dmtvManager.CheckTreeForSystemNode(xml, "Email"));
                    InvoiceGUID = Common.ConvertDef.ToGuid(dmtvManager.CheckTreeForSystemNode(xml, "Invoice"), Guid.Empty);
                    mapper = new TemplateFolderMapper(CorrespondenceGUID, EmailGUID, treeView, id);
                }
                else
                {
                    mapper = CreateMatterTreeView(id, rootString, dmtvManager);
                }

                mappingTreeView = treeView;

                if (documentsWithoutFolderGuids != null && documentsWithoutFolderGuids.Rows.Count > 0)
                {
                    mapper.Map();
                }
            }
            catch(Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }


        private bool MatterHasFolderData(string xml)
        {
            return !string.IsNullOrWhiteSpace(xml);
        }


        private IFolderMapper CreateMatterTreeView(long id, string rootString, DMTreeViewXMLManager dmtvManager)
        {
            IFolderMapper mapper;
            dmtvManager.AssignTreeStructureToMatter(id, rootString, out mapper);
            DocumentFolderRepositoryXML repository = new DocumentFolderRepositoryXML();

            string saveXML = dmtvManager.SerializeTreeView();
            repository.Save(tableName, id, saveXML, storePprocedureName);

            return mapper;
        }


        public void Build(string code, RadTreeView treeView, string rootString)
        {
            throw new NotImplementedException();
        }
    }

}
