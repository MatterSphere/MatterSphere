using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using FWBS.OMS.Data;

namespace FWBS.OMS.UI.DocumentManagement.DocumentFolderManagement
{
    public class DocumentFolderRepositoryXML : IDocumentFolderRepository
    { 
        public void Save(string tableName, long id, string xml, string procedureName)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("tablename", tableName));
            parList.Add(connection.CreateParameter("id", id));
            parList.Add(connection.CreateParameter("treeXML", xml));
            connection.ExecuteProcedure(procedureName, parList);
        }


        public void Save(string description, string tableName, string xml, string procedureName)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("description", description)); 
            parList.Add(connection.CreateParameter("tablename", tableName));
            parList.Add(connection.CreateParameter("treeXML", xml));
            parList.Add(connection.CreateParameter("userID", Session.CurrentSession.CurrentUser.ID));
            connection.ExecuteProcedure(procedureName, parList);
        }


        public string Get(string tableName, long id, string procedureName)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("id", id));
            parList.Add(connection.CreateParameter("tablename", tableName));
            DataTable dt = connection.ExecuteProcedure(procedureName, parList);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToString(dt.Rows[0]["treeXML"]);
            }
            else
                return null;
        }

        public string Get(string tableName, string code, string procedureName)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("code", code));
            parList.Add(connection.CreateParameter("tablename", tableName));
            DataTable dt = connection.ExecuteProcedure(procedureName, parList);
            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToString(dt.Rows[0]["treeXML"]);
            }
            else
                return null;
        }


        public string CreateDefaultTreeXML()
        {
            return new XDocument(new XElement("TreeView", 
                                      new XElement("node",
                                            new XElement("node",
                                                new XAttribute("SystemID", "Correspondence"),
                                                new XAttribute("system", true),
                                                new XAttribute("FolderCode", "GENERAL"),
                                                new XAttribute("FolderGUID", Guid.NewGuid())),
                                            new XElement("node",
                                                new XAttribute("SystemID", "Email"),
                                                new XAttribute("system", true),
                                                new XAttribute("FolderCode", "EMAIL"),
                                                new XAttribute("FolderGUID", Guid.NewGuid()))))).ToString();

        }


        public void AutoAssignFolderGUID(long id, Guid correspondenceFolderGUID, Guid emailFolderGUID)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("id", id));
            parList.Add(connection.CreateParameter("correspondenceFolderGUID", correspondenceFolderGUID));
            parList.Add(connection.CreateParameter("emailFolderGUID", emailFolderGUID));
            DataTable dt = connection.ExecuteProcedure("sprAutoAssignFolderGUID", parList);
        }

        public void AssignForlderGUIDBatch(string docIDsXml, Guid folderGUID, bool includeRelatedDocuments = true, int? updatedBy = null)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("docIDs", docIDsXml));
            parList.Add(connection.CreateParameter("folderGUID", folderGUID));
            parList.Add(connection.CreateParameter("includeRelatedDocuments", includeRelatedDocuments));
            if (updatedBy.HasValue) parList.Add(connection.CreateParameter("updatedBy", updatedBy.Value));
            parList.Add(connection.CreateParameter("result", 0));
            DataTable dt = connection.ExecuteProcedure("sprAssignFolderGUIDBatch", parList);
        }

        public void DeleteFolderTreeTemplate(long id)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("id", id));
            DataTable dt = connection.ExecuteProcedure("sprDeleteFileFolderTreeTemplate", parList);
        }

        public void DeleteFolderTreeXML(long id)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("id", id));
            DataTable dt = connection.ExecuteProcedure("[sprDeleteFileFolderTreeXML]", parList);
        }

        public bool CheckFolderForDocuments(Guid folderGUID, bool includeDeleted = false)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("folderGUID", folderGUID));
            parList.Add(connection.CreateParameter("includeDeleted", includeDeleted));
            DataTable dt = connection.ExecuteProcedure("sprCheckForDocumentsWithGUID", parList);
            if (dt != null & dt.Rows.Count > 0)
                return true;
            else
                return false;
        }


        public DataTable GetFileDocumentsWithoutGUIDs(long fileID)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("fileID", fileID));
            DataTable dt = connection.ExecuteProcedure("sprGetFileDocumentsWithoutGUIDs", parList);
            if (dt != null & dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }

        public DataTable GetFilesWithoutFolderTreeByType(string fileType)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("@fileType", fileType));
            return connection.ExecuteSQL("select fileId, fileType from config.dbFile f where not exists(select 1 from dbo.dbFileFolderTreeData where id = f.fileID) and fileType = @fileType", parList);
        }

        internal DataTable GetWalletCodesForCreationAsFolders(long fileID, string culture)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("fileID", fileID));
            parList.Add(connection.CreateParameter("UI", culture));
            DataTable dt = connection.ExecuteProcedure("GetMatterDocumentWalletCodes", parList);
            if (dt != null & dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }


        internal void WalletDrivenMappingToFolders(long fileID, DataTable dt)
        {
            IConnection con = Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(con.CreateParameter("fileID", fileID));
            parList.Add(con.CreateParameter("foldercodetable", dt));
            con.ExecuteProcedure("MapDocumentsToFoldersCreatedFromWallets", parList);
        }
    }
}
