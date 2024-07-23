using Telerik.WinControls.UI;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    public interface IDocumentFolderBuilder
    {
        void Build(long ID, RadTreeView treeView, string rootString, bool useCheckBox);
        void Build(string code, RadTreeView treeView, string rootString);
    }

    public interface IDocumentFolderSaver
    {
        void Save(long ID, RadTreeView treeView);
        void Save(string description, RadTreeView treeView);
    }

    public interface IDocumentFolderRepository
    {
        string Get(string tableName, long id, string procedureName);
        string Get(string tableName, string code, string procedureName);
        void Save(string tableName, long id, string xml, string procedureName);
        void Save(string description, string tableName, string xml, string procedureName);
    }


}
