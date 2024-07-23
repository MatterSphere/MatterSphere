using System;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    public static class DocumentFolderFactory
    {
            public static IDocumentFolderSaver GetSaver(Type omsType)
            {
                if (omsType == typeof(OMSFile)) return (IDocumentFolderSaver)new MatterDocumentFolderSaverXML();
                if (omsType == typeof(Client)) return (IDocumentFolderSaver)new ClientDocumentFolderSaverXML();
                if (omsType == typeof(MatterTemplateSaverXML)) return (IDocumentFolderSaver)new MatterTemplateSaverXML();
                return null;
            }

            public static IDocumentFolderBuilder GetBuilder(Type omsType)
            {
                if (omsType == typeof(OMSFile)) return (IDocumentFolderBuilder)new MatterDocumentFolderBuilderXML();
                if (omsType == typeof(Client)) return (IDocumentFolderBuilder)new ClientDocumentFolderBuilderXML();
                if (omsType == typeof(MatterTemplateBuilderXML)) return (IDocumentFolderBuilder)new MatterTemplateBuilderXML();
                return null;
            }
    }
}
