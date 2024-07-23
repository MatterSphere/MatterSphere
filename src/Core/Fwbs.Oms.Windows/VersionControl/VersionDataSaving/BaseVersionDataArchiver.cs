using System;
using System.Data;
using System.Text;
using System.Xml;
using FWBS.Common;
using FWBS.OMS.Data;

namespace FWBS.OMS.UI.Windows
{
    public interface IVersionDataArchiver
    {
        string Code { get; }
        long Version { get; }
        Guid VersionID { get; }
        DataSet VersionDataToSave { get; }
        string Destination { get; set; }
        string Comments { get; set; }
        VersionDataXMLHeader HeaderXML { get; set; }
        void SaveData(DataSet versionDataToSave, string code, long version, Guid versionID, bool suppressComments, bool processLinks);
        void UpdateXMLHeaderLinkData(LinkedItem linkedItem);
    }


    public abstract class BaseVersionDataArchiver : IVersionDataArchiver
    {
        public event EventHandler Saved;

        DataSet versionDataToSave;
        VersionDataSaver versionDataSaver;
        Guid versionID;
        string code;
        string comments = string.Empty;
        long version;
        string destination;
        VersionDataXMLHeader versionDataXMLHeader;
        private bool processlinks;


         public VersionDataXMLHeader HeaderXML
        {
            get 
            { 
                return versionDataXMLHeader; 
            }
            set 
            { 
                versionDataXMLHeader = value; 
            }
        }


        private string VersionDataXMLHeaderAsString
        {
            get
            {
                var stringBuilder = new StringBuilder();
                XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
                versionDataXMLHeader.Save(xmlWriter);
                return stringBuilder.ToString();
            }
        }


        public string Code
        {
            get
            {
                return code;
            }
        }


        public long Version
        {
            get
            {
                return version;
            }
        }


        public Guid VersionID
        {
            get
            {
                return versionID;
            }
        }


        public DataSet VersionDataToSave
        {
            get
            {
                return versionDataToSave;
            }
        }


        public string Destination
        {
            get
            {
                return destination;
            }
            set
            {
                destination = value;
            }
        }


        public string Comments
        {
            get
            {
                return comments;
            }
            set
            {
                comments = value;
            }
        }


        public void OnSaved()
        {
            if (Saved != null)
                Saved(this, EventArgs.Empty);
        }


        public virtual void SaveData(DataSet versionDataToSave, string code, long version, Guid versionID, bool suppressComments = false, bool processLinks = true)
        {
            this.versionDataToSave = versionDataToSave;
            this.versionID = versionID;
            this.code = code;
            this.version = version;
            this.processlinks = processLinks;

            if (suppressComments)
                ProceedWithSaveWithoutComments(code, version);
            else
                RequestCommentsFromUserThenSave(code, version);
        }


        private void ProceedWithSaveWithoutComments(string code, long version)
        {
            DataTable results;
            if(versionDataXMLHeader == null)
                CreateDataXMLHeader(code, version);
            else
            {
                UpdateXMLHeaderLinkData(new LinkedItem { Code = code, 
                                                         Version = Convert.ToInt32(version), 
                                                         Destination = destination, 
                                                         VersionID = versionID });
            }

          
            if (!VersionDataSaver.VersionDataExistsInDB(destination, code, version, out results))
            {
                SaveVersionData();
                VersionDataSaver.VersionDataExistsInDB(destination, code, version, out results);
            }

            var idInVersionTable = ConvertDef.ToInt64(results.Rows[0]["VersionID"], 0);

            OnIndividualLinkSaved(new LinkItemSavedEventArgs { tableName = destination, versionID = idInVersionTable });
        }


        private void RequestCommentsFromUserThenSave(string code, long version)
        {
            var runSaveProcess = new Action(() =>
            {
                DataTable results = null;
                CreateDataXMLHeader(code, version);
                
                if (!VersionDataSaver.VersionDataExistsInDB(destination, code, version, out results))
                {
                    SaveVersionData();
                    VersionDataSaver.VersionDataExistsInDB(destination, code, version, out results);
                }

                var idInVersionTable = ConvertDef.ToInt64(results.Rows[0]["VersionID"], 0);

                OnIndividualLinkSaved(new LinkItemSavedEventArgs { tableName = destination, versionID = idInVersionTable });
                if (processlinks)
                    ProcessLinkedItems();
            });

            var saveWithVersionForm = new FWBS.OMS.UI.Windows.frmSaveWithVersionData(destination, code, version, this);

            saveWithVersionForm.Closing += (s, e) =>
            {
                if (!e.CancelClicked)
                {
                    this.comments = e.Comments;
                    runSaveProcess();
                    OnSaved();
                }
            };

            saveWithVersionForm.Show();
        }

        private void CreateDataXMLHeader(string code, long version)
        {
            versionDataXMLHeader = new VersionDataXMLHeader();
            InsertHeaderData();
            UpdateXMLHeaderLinkData(new LinkedItem
            {
                Code = code,
                Version = Convert.ToInt32(version),
                Destination = destination,
                VersionID = versionID
            });
        }


        public abstract void ProcessLinkedItems();


        private void SaveVersionData()
        {
            versionDataSaver = new VersionDataSaver(new VersionDataToXML(FWBS.OMS.Session.CurrentSession.CurrentConnection));

            var versionDataSaveArgs = new VersionDataSaveArgs()
            {
                Code = code,
                CurrentVersion = version,
                DataSetToSave = versionDataToSave,
                Destination = destination,
                Comments = comments,
            };

            versionDataSaver.SaveVersionData(versionDataSaveArgs);
        }


        public event EventHandler<LinkItemSavedEventArgs> IndividualLinkSaved;


        private void OnIndividualLinkSaved(LinkItemSavedEventArgs e)
        {
            if (IndividualLinkSaved != null)
            {
                IndividualLinkSaved(this, e);
            }
        }


        private void InsertHeaderData()
        {
            while (VersionGuidExists(versionID))
            {
                versionID = Guid.NewGuid();
            }

            var sql = @"insert into dbVersionDataHeader(versionID, versionLinks, versionComments, Created, CreatedBy)
                        values(@versionID, @versionLinks, @versionComments, @created, @createdBy)";

            var headerData = new
            {
                ID = versionID,
                Comments = comments,
                Created = System.DateTime.Now,
                CreatedBy = FWBS.OMS.Session.CurrentSession.CurrentUser.ID,
                Links = VersionDataXMLHeaderAsString
            };

            IDataParameter[] parameters = new IDataParameter[5];
            parameters[0] = Session.CurrentSession.CurrentConnection.CreateParameter("versionID", headerData.ID);
            parameters[1] = Session.CurrentSession.CurrentConnection.CreateParameter("versionComments", headerData.Comments);
            parameters[2] = Session.CurrentSession.CurrentConnection.CreateParameter("created", headerData.Created);
            parameters[3] = Session.CurrentSession.CurrentConnection.CreateParameter("createdBy", headerData.CreatedBy);
            parameters[4] = Session.CurrentSession.CurrentConnection.CreateParameter("versionLinks", headerData.Links);

            ExecuteSQL(sql, parameters);
        }


        private bool VersionGuidExists(Guid versionID)
        {
            var sql = @"select * from dbVersionDataHeader where versionID = @id";

            IDataParameter[] parameters = new IDataParameter[1];
            parameters[0] = Session.CurrentSession.CurrentConnection.CreateParameter("id", versionID);

            var results = ExecuteSQL(sql, parameters);

            if (results != null && results.Rows.Count > 0)
            {
                return true;
            }

            return false;
        }


        private DataTable ExecuteSQL(string sql, IDataParameter[] parameters)
        {
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            return connection.ExecuteSQL(sql, parameters);
        }


        public void UpdateXMLHeaderLinkData(LinkedItem linkedItem)
        {
            if (NodeExists(linkedItem))
            {
                return;
            }

            var nodeBuilder = new LinkItemNodeBuilder(HeaderXML);
            var linkItemNode = nodeBuilder.BuildLinkItemNode(linkedItem);
            nodeBuilder.AddLinkItemNodeToVersionHeaderXML(linkItemNode);

            SaveHeaderXMLToDB(HeaderXML, linkedItem.VersionID);
        }


        private bool NodeExists(LinkedItem linkedItem)
        {
            var linkItems = HeaderXML.GetElementsByTagName("LinkItem");
            foreach (XmlNode linkItem in linkItems)
            {
                var linkItemChildren = linkItem.ChildNodes;

                if (linkItemChildren[0].InnerText == linkedItem.Code
                && linkItemChildren[1].InnerText == linkedItem.Version.ToString()
                && linkItemChildren[2].InnerText == linkedItem.Destination)
                {
                    return true;
                }
            }

            return false;
        }


        private void SaveHeaderXMLToDB(VersionDataXMLHeader versionDataXMLHeader, Guid versionID)
        {
            var stringBuilder = new StringBuilder();
            XmlWriter xmlWriter = XmlWriter.Create(stringBuilder);
            versionDataXMLHeader.Save(xmlWriter);

            var sql = @"update dbVersionDataHeader set versionLinks = @versionLinks where versionID = @versionID";

            IDataParameter[] parameters = new IDataParameter[2];
            parameters[0] = Session.CurrentSession.CurrentConnection.CreateParameter("versionID", versionID);
            parameters[1] = Session.CurrentSession.CurrentConnection.CreateParameter("versionLinks", stringBuilder.ToString());

            ExecuteSQL(sql, parameters);
        }
    }

}
