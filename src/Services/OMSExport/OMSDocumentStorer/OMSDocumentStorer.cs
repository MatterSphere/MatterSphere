using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace FWBS.OMS.OMSEXPORT
{
    using OMS;
    using UI.DocumentManagement.DocumentFolderManagement;

    public class OMSDocumentStorer : IDisposable
    {
        private DirectoryInfo _tempDirectory;
        private Dictionary<long, Guid> _cachedMatterFolders;

        static OMSDocumentStorer()
        {
            typeof(Session).GetField("_installLocation", BindingFlags.NonPublic | BindingFlags.Instance)
                .SetValue(Session.CurrentSession, AppDomain.CurrentDomain.BaseDirectory);

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                if (args.Name.StartsWith("OMS.Library", StringComparison.InvariantCultureIgnoreCase))
                    return Assembly.GetExecutingAssembly();

                return null;
            };
        }

        public OMSDocumentStorer()
        {
            Connect();
            _cachedMatterFolders = new Dictionary<long, Guid>();
            _tempDirectory = new DirectoryInfo(Path.Combine(Global.GetTempPath().FullName, Assembly.GetExecutingAssembly().GetCustomAttribute<GuidAttribute>().Value));
            _tempDirectory.Create();
        }

        public void Dispose()
        {
            try
            {
                Session.CurrentSession.Disconnect();
                _tempDirectory.Delete(true);
            }
            catch (Exception ex)
            {
                LogException(ex, false);
            }
        }

        private void Connect()
        {
            Data.DatabaseSettings settings;
            string SQLServerName = StaticLibrary.GetSetting("OMSSQLServer", "", "");
            string SQLDatabaseName = StaticLibrary.GetSetting("OMSSQLDatabase", "", "");
            string LoginType = StaticLibrary.GetSetting("OMSLoginType", "", "");

            try
            {
                Session.CurrentSession.APIConsumer = Assembly.GetExecutingAssembly();
                Session.CurrentSession.Connect(); // Try to connect to existing session

                settings = Session.CurrentSession.CurrentDatabase;
                if (!settings.Server.Equals(SQLServerName, StringComparison.CurrentCultureIgnoreCase) ||
                    !settings.DatabaseName.Equals(SQLDatabaseName, StringComparison.CurrentCultureIgnoreCase))
                {
                    Dispose();
                    throw new InvalidOperationException("OMSExport and MatterSphere database mismatch!");
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception)
            {
                if (SQLServerName == "" || SQLDatabaseName == "" || LoginType == "")
                    throw new Exception("Database setting are not configured");

                string username = LoginType == "SQL" ? StaticLibrary.GetSetting("OMSSQLUID", "", "") : Environment.UserName;
                string password = LoginType == "SQL" ? StaticLibrary.GetSetting("OMSSQLPWD", "", "") : string.Empty;

                Data.DatabaseConnections connections = new Data.DatabaseConnections("OMSEXPORT", "OMS", "2.0");
                settings = connections.CreateDatabaseSettings();
                settings.Server = SQLServerName;
                settings.DatabaseName = SQLDatabaseName;
                settings.LoginType = LoginType;

                Session.CurrentSession.LogOn(settings, username, password, true);
            }
        }

        public bool ImportDocument(int mattExtId, string mattExtTxtId, string attachmentId, string fileName, string fileDescription, byte[] fileData)
        {
            bool result = (GetDocumentID(attachmentId) != 0);
            if (!result)
            {
                OMSFile matter = GetMatter(mattExtId, mattExtTxtId);
                if (matter != null)
                {
                    string filePath = Path.Combine(_tempDirectory.FullName, fileName);
                    File.WriteAllBytes(filePath, fileData);

                    Guid folderGuid = GetOrCreateInvoiceFolder(matter);
                    result = SaveDocumentToMatterSphere(matter, filePath, fileDescription, attachmentId, folderGuid) != 0;
                }
            }
            return result;
        }

        public bool DeleteDocument(string attachmentId)
        {
            OMSDocument doc = null;
            try
            {
                long docID = GetDocumentID(attachmentId);
                if (docID != 0)
                {
                    doc = OMSDocument.GetDocument(docID);
                    doc.Delete(false, DateTime.Now.AddDays(Session.CurrentSession.DeletionRetentionPeriod), true);
                }
                return true;
            }
            catch (Exception ex)
            {
                LogException(ex, doc != null);
            }
            return false;
        }

        private OMSFile GetMatter(int mattExtId, string mattExtTxtId)
        {
            OMSFile matter = null;
            try
            {
                var connection = Session.CurrentSession.CurrentConnection;
                Data.ExecuteParameters pars = new Data.ExecuteParameters
                {
                    CommandType = CommandType.Text,
                    Sql = "SELECT TOP 1 fileID FROM dbFile WITH (NOLOCK) WHERE fileExtLinkID = @extID OR fileExtLinkTxtID = @extTxtID"
                };
                pars.Parameters.Add(connection.CreateParameter("extID", mattExtId));
                pars.Parameters.Add(connection.CreateParameter("extTxtID", mattExtTxtId));

                long fileID = Common.ConvertDef.ToInt64(connection.ExecuteScalar(pars), 0);
                if (fileID != 0)
                    matter = OMSFile.GetFile(fileID);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return matter;
        }

        private long GetDocumentID(string docIDExt)
        {
            long docID = 0;
            try
            {
                var connection = Session.CurrentSession.CurrentConnection;
                Data.ExecuteParameters pars = new Data.ExecuteParameters
                {
                    CommandType = CommandType.Text,
                    Sql = "SELECT TOP 1 docID FROM dbDocument WITH (NOLOCK) WHERE docIDExt = @docIDExt"
                };
                pars.Parameters.Add(connection.CreateParameter("docIDExt", docIDExt));
                docID = Common.ConvertDef.ToInt64(connection.ExecuteScalar(pars), 0);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return docID;
        }

        private Guid GetOrCreateInvoiceFolder(OMSFile matter)
        {
            Guid folderGuid;
            if (_cachedMatterFolders.TryGetValue(matter.ID, out folderGuid))
                return folderGuid;

            var treeView = new Telerik.WinControls.UI.RadTreeView(); // Fake tree mock
            MatterDocumentFolderBuilderXML dfsBuilder = new MatterDocumentFolderBuilderXML();
            dfsBuilder.Build(matter.ID, treeView, matter.FileNo, false);
            folderGuid = dfsBuilder.InvoiceGUID;

            if (folderGuid == Guid.Empty)
            {
                DMTreeViewManager dmtvManager = new DMTreeViewManager(treeView);
                folderGuid = dmtvManager.AddInvoiceFolder();

                MatterDocumentFolderSaverXML dfsSaver = new MatterDocumentFolderSaverXML();
                dfsSaver.Save(matter.ID, treeView);
            }

            _cachedMatterFolders.Add(matter.ID, folderGuid);
            return folderGuid;
        }

        private long SaveDocumentToMatterSphere(OMSFile file, string fullFilePath, string docDescription, string externalId, Guid folderGuid)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fullFilePath);
                string extension = fileInfo.Extension.Trim('.');
                //Construct a new document
                OMSDocument docNew = new OMSDocument(file.DefaultAssociate, docDescription, Precedent.GetDefaultPrecedent("SHELL", null), null, 0, DocumentDirection.In, extension, -1, null, folderGuid);
                docNew.ExternalId = externalId;
                docNew.AlternateDescription = fileInfo.Name;
                docNew.TimeRecords.SkipTime = true;
                //Set document version as the latest version
                DocumentManagement.Storage.IStorageItemVersionable versionable = docNew;
                DocumentManagement.Storage.IStorageItemVersion version = versionable.GetLatestVersion();
                versionable.SetWorkingVersion(version);
                //Update document to create initial row
                docNew.Update();
                //construct storage provider
                DocumentManagement.Storage.StorageProvider sp = docNew.GetStorageProvider();
                //Save File to destination
                sp.Store(version, fileInfo);
                //Delete temp file
                try { fileInfo.Delete(); } catch { }
                //Return ID number
                return docNew.ID;
            }
            catch (Exception ex)
            {
                LogException(ex);
                return 0; // return 0 if it drops out
            }
        }

        private static void LogException(Exception ex, bool error = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MatterSphere Error").AppendLine(ex.Message);
            if (ex.InnerException != null)
                sb.AppendLine("Inner Exception").AppendLine(ex.InnerException.Message);
            StaticLibrary.LogErrorMessage(sb.ToString(), error);
        }
    }
}
