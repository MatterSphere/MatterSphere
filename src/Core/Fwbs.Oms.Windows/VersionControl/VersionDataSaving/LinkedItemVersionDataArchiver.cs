using System;
using System.Collections.Generic;
using System.Data;
using FWBS.OMS.Data;

namespace FWBS.OMS.UI.Windows
{
    public class LinkedItemVersionDataArchiver<T> where T : IVersionDataArchiver
    {
        private T archiveType;
        private ObjectLinkRetriever objectLinkRetriever;


        internal LinkedItemVersionDataArchiver(T t)
        {
            this.archiveType = t;
            this.objectLinkRetriever = new ObjectLinkRetriever();
        }


        internal void ProcessLinkedItems()
        {
            DataTable links = null;

            if (archiveType is EnquiryFormVersionDataArchiver)
            {
                links = GetLinks(new EnquiryFormComparer(archiveType.Code));
            }

            if (archiveType is ScriptVersionDataArchiver)
            {
                links = GetLinks(new ScriptComparer(archiveType.Code));
            }

            if (archiveType is SearchListVersionDataArchiver)
            {
                links = GetLinks(new SearchListComparer(archiveType.Code));
            }

            if (archiveType is PrecedentVersionDataArchiver)
            {
                links = GetLinks(new PrecedentComparer(archiveType.Code));
            }

            if (archiveType is FileManagementVersionDataArchiver)
            {
                links = GetLinks(new FileManagementComparer(archiveType.Code));
            }

            if (links != null && links.Rows.Count > 1)
            {
                ProcessLinks(links);
            }
        }

 
        private DataTable GetLinks(IObjectComparison objectType)
        {
            var links = objectLinkRetriever.RetrieveObjectLinks(objectType, includeDataLists: false);
            return links;
        }


        private void ProcessLinks(DataTable links)
        {
            if (links.Rows.Count > 1)
            {
                var linkedItems = GetLinkedItemList(links);

                if (linkedItems.Count > 0)
                {
                    foreach (var linkedItem in linkedItems)
                    {
                        switch (linkedItem.Type)
                        {
                            case LinkType.SearchList:
                                ProcessLinkedItem(linkedItem, new SearchListVersionDataArchiver());
                                break;

                            case LinkType.EnquiryForm:
                                ProcessLinkedItem(linkedItem, new EnquiryFormVersionDataArchiver());
                                break;

                            case LinkType.Script:
                                ProcessLinkedItem(linkedItem, new ScriptVersionDataArchiver());
                                break;

                            case LinkType.Precedent:
                                ProcessLinkedItem(linkedItem, new PrecedentVersionDataArchiver());
                                break;

                            case LinkType.FileManagement:
                                ProcessLinkedItem(linkedItem, new FileManagementVersionDataArchiver());
                                break;
                        }
                    }
                }
            }
        }


        private List<LinkedItem> GetLinkedItemList(DataTable links)
        {
            var linkedItems = new List<LinkedItem>();

            foreach (DataRow link in links.Rows)
            {
                var type = Convert.ToString(link["objecttype"]);
                var code = Convert.ToString(link["objectcode"]);
                var linkedItem = new LinkedItem { VersionID = archiveType.VersionID };

                switch (type)
                {
                    case "SearchList":
                        linkedItem.Type = LinkType.SearchList;
                        var searchListLinkItem = GetLinkedItemForSaving(linkedItem, "select * from dbSearchListConfig where schCode = @code", code, "schCode", "schVersion", "dbSearchlistConfig");
                        linkedItems.Add(searchListLinkItem);
                        break;

                    case "EnquiryForm":
                        linkedItem.Type = LinkType.EnquiryForm;
                        var enquiryFormLinkItem = GetLinkedItemForSaving(linkedItem, "select * from dbEnquiry where enqCode = @code", code, "enqCode", "enqVersion", "dbEnquiry", true);
                        linkedItems.Add(enquiryFormLinkItem);
                        break;

                    case "Script":
                        linkedItem.Type = LinkType.Script;
                        var scriptLinkItem = GetLinkedItemForSaving(linkedItem, "select * from dbScript where scrCode = @code", code, "scrCode", "scrVersion", "dbScript");
                        linkedItems.Add(scriptLinkItem);
                        break;

                    case "Precedent":
                        break;

                    case "FileManagement":
                        linkedItem.Type = LinkType.FileManagement;
                        var fileManagementLinkItem = GetLinkedItemForSaving(linkedItem, "select * from dbFileManagementApplication where appCode = @code", code, "appCode", "appVer", "dbFileManagementApplication");
                        linkedItems.Add(fileManagementLinkItem);
                        break;
                }
            }

            return linkedItems;
        }


        private LinkedItem GetLinkedItemForSaving(LinkedItem linkedItem, string sql, string code, string codeFieldName, string versionFieldName, string tableNameOfDataBeingArchived, bool linkedItemIsEnquiryForm = false)
        {
            DataTable data = GetData(sql, code);

            if (data != null && data.Rows.Count > 0)
            {
                data.TableName = tableNameOfDataBeingArchived;

                var dataSet = new DataSet();
                dataSet.Tables.Add(data);

                if (linkedItemIsEnquiryForm)
                {
                    List<DataTable> associatedEnquiryTables = GetAssociatedEnquiryTables(data.Rows[0]["enqID"].ToString());

                    dataSet.Tables.Add(associatedEnquiryTables[0]);
                    dataSet.Tables.Add(associatedEnquiryTables[1]);
                }

                linkedItem.VersionData = dataSet;
                linkedItem.Code = data.Rows[0][codeFieldName].ToString();
                linkedItem.Version = int.Parse(data.Rows[0][versionFieldName].ToString());
                linkedItem.VersionID = archiveType.VersionID;
            }

            return linkedItem;
        }


        private List<DataTable> GetAssociatedEnquiryTables(string enqID)
        {
            var associatedEnquiryTables = new List<DataTable>();

            var enquiryQuestionDataSql = @"select * from dbEnquiryQuestion where enqID in (select enqID from dbEnquiry where enqCode = @code)";
            var enquiryQuestionData = GetData(enquiryQuestionDataSql, enqID);
            enquiryQuestionData.TableName = "dbEnquiryQuestion";
            associatedEnquiryTables.Add(enquiryQuestionData);

            var enquiryPageDataSql = @"select * from dbEnquiryPage where enqID in (select enqID from dbEnquiry where enqCode = @code)";
            var enquiryPageData = GetData(enquiryPageDataSql, enqID);
            enquiryPageData.TableName = "dbEnquiryPage";
            associatedEnquiryTables.Add(enquiryPageData);

            return associatedEnquiryTables;
        }


        private DataTable GetData(string sql, string code)
        {
            IConnection connection = Session.CurrentSession.CurrentConnection;
            IDataParameter[] parameters = new IDataParameter[1];
            parameters[0] = connection.CreateParameter("code", code);
            var results = connection.ExecuteSQL(sql, parameters);
            return results;
        }


        private void ProcessLinkedItem(LinkedItem linkedItem, IVersionDataArchiver versionDataArchiver)
        {
            versionDataArchiver.HeaderXML = archiveType.HeaderXML;
           
            SaveVersionData(versionDataArchiver, linkedItem);
        }


        private void SaveVersionData(IVersionDataArchiver versionDataArchiver, LinkedItem linkedItem)
        {
            versionDataArchiver.SaveData(linkedItem.VersionData, linkedItem.Code, linkedItem.Version, archiveType.VersionID, suppressComments: true, processLinks: true);
        }
    }
}
