using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace FWBS.OMS.DocumentManagement.Storage
{
    public class LocalDocumentCache : Caching.ICacheable, IDisposable, ILocalDocumentCache
    {

        #region Backward Compatibility

        private const string OldDatabaseName = "LocalDocuments.xml";

        private static FileInfo GetOldCacheDatabase()
        {
            DirectoryInfo dir = new DirectoryInfo(Global.GetAppDataPath());
            return new FileInfo(Path.Combine(dir.FullName, OldDatabaseName));
        }

        private FileInfo olddb;
        private FileInfo OldDatabase
        {
            get
            {
                if (olddb == null)
                    olddb = GetOldCacheDatabase();
                return olddb;
            }
        }

        #endregion

        #region Fields

        private System.Threading.Mutex mutex;
        private DataSet localDocuments;
        private FileInfo db;
        public const string DatabaseName = "LocalDocumentCache.xml";
        private bool isloaded;
        private bool isdirty;
        private DateTime lastModified;


        #endregion

        #region Constructors

        public LocalDocumentCache()
        {
            mutex = new System.Threading.Mutex(false, @"Local\MatterSphere-Mutex-" + DatabaseName);
            Load();

            Session.CurrentSession.Disconnected += new EventHandler(OMS_Disconnected);
            Session.CurrentSession.Connected += new EventHandler(OMS_Connected);

        }

        private void OMS_Connected(object sender, EventArgs e)
        {
            Load();
        }

        private void OMS_Disconnected(object sender, EventArgs e)
        {
            isloaded = false;
        }

        #endregion

        #region Static

        [Obsolete("Please use StorageManager.CurrentManager.LocalDocuments")]
        public static ILocalDocumentCache Cache
        {
            get
            {
                return StorageManager.CurrentManager.LocalDocuments;
            }
        }

        #endregion

        #region Properties

        public int LimitSize
        {
            get
            {
                FWBS.Common.ApplicationSetting setting = new FWBS.Common.ApplicationSetting(Global.ApplicationKey, String.Empty, String.Empty, "LocalDocumentSizeLimit", 100000000);
                int val = Common.ConvertDef.ToInt32(setting.GetSetting(), 100000000);
                return val;
            }
        }

        public bool IsLoaded
        {
            get
            {
                LocalDatabase.Refresh();
                return isloaded && LocalDatabase.LastWriteTime == lastModified;
            }
        }

        public bool IsDirty
        {
            get
            {
                return isdirty;
            }
        }

        public DirectoryInfo LocalDocumentDirectory
        {
            get
            {
                if (Session.CurrentSession.IsLoggedIn)
                {
                    DirectoryInfo dir = new DirectoryInfo(Path.Combine(Global.GetDBAppDataPath(), "Documents"));
                    if (!dir.Exists) dir.Create();
                    return dir;
                }
                else
                    return null;
            }

        }

        public DirectoryInfo LocalPrecedentDirectory
        {
            get
            {
                if (Session.CurrentSession.IsLoggedIn)
                {
                    DirectoryInfo dir = new DirectoryInfo(Path.Combine(Global.GetDBAppDataPath(), "Precedents"));
                    if (!dir.Exists) dir.Create();
                    return dir;
                }
                else
                    return null;
            }

        }

        private FileInfo LocalDatabase
        {
            get
            {
                if (db == null)
                    db = GetLocalCacheDatabase();
                return db;
            }
        }

        #endregion

        #region Methods

        private string GetItemFilter(IStorageItem item)
        {
            ValidateItem(item);

            DocumentVersion version = item as DocumentVersion;
            Precedent prec = item as Precedent;
            PrecedentVersion precVersion = item as PrecedentVersion;
            if (version != null)
                return String.Format("(instance_id = '{0}' or instance_id is null) and (server = '{1}' or server is null) and (database = '{2}' or database is null) and verid = '{3}'", Session.CurrentSession._multidb.Number, Session.CurrentSession._multidb.Server, Session.CurrentSession._multidb.DatabaseName, version.Id);
            if (prec != null)
                return String.Format("(instance_id = '{0}' or instance_id is null) and (server = '{1}' or server is null) and (database = '{2}' or database is null) and id = '{3}'", Session.CurrentSession._multidb.Number, Session.CurrentSession._multidb.Server, Session.CurrentSession._multidb.DatabaseName, prec.ID);
            else if (precVersion != null)
                return String.Format("(instance_id = '{0}' or instance_id is null) and (server = '{1}' or server is null) and (database = '{2}' or database is null) and verid = '{3}'", Session.CurrentSession._multidb.Number, Session.CurrentSession._multidb.Server, Session.CurrentSession._multidb.DatabaseName, precVersion.Id);
            return null;
        }

        private void ValidateItem(IStorageItem item)
        {
            DocumentVersion version = item as DocumentVersion;
            Precedent prec = item as Precedent;
            PrecedentVersion precVersion = item as PrecedentVersion;

            if (version == null && prec == null && precVersion == null)
                throw new ArgumentException(Session.CurrentSession.Resources.GetMessage("STRITMNDSPEC", "Storage item needs to be specified for the local document cache filter.", "").Text);
        }

        private string GetInstanceFilter()
        {
            return String.Format("(server = '{0}' or server is null) and (database = '{1}' or database is null)", Session.CurrentSession._multidb.Server, Session.CurrentSession._multidb.DatabaseName);
        }

        private bool IsCurrentInstance(DataRow cachedRow)
        {
            if (!Session.CurrentSession.IsLoggedIn)
                return false;

            if ((cachedRow["instance_id"] == DBNull.Value || Convert.ToInt32(cachedRow["instance_id"]) == Session.CurrentSession._multidb.Number)
                && (cachedRow["server"] == DBNull.Value || Convert.ToString(cachedRow["server"]) == Session.CurrentSession._multidb.Server)
                && (cachedRow["database"] == DBNull.Value || Convert.ToString(cachedRow["database"]) == Session.CurrentSession._multidb.DatabaseName))
            {
                return true;
            }
            else
                return false;
        }

        public FileInfo GetLocalFile(IStorageItem item, out DateTime? cachedDate)
        {
            ValidateItem(item);

            if (!IsLoaded)
                InternalLoad();

            DocumentVersion version = item as DocumentVersion;
            Precedent prec = item as Precedent;
            PrecedentVersion precVersion = item as PrecedentVersion;

            DataView vw = null;

            cachedDate = null;
            if (version != null)
                vw = new DataView(localDocuments.Tables["Documents"]);
            if (prec != null)
                vw = new DataView(localDocuments.Tables["Precedents"]);
            else if (precVersion != null)
                vw = new DataView(localDocuments.Tables["Precedents"]);

            if (vw != null)
            {
                vw.RowFilter = GetItemFilter(item);
                if (vw.Count > 0)
                {
                    string fp = Convert.ToString(vw[0]["filelocalpath"]);

                    if (fp.Length > 0)
                    {
                        if (File.Exists(fp) == false)
                            return null;

                        try
                        {
                            using (System.IO.File.Open(fp, FileMode.Open)) { }
                        }
                        catch { return null; }

                        DateTime cached;
                        if (vw[0]["CacheDate"] == DBNull.Value)
                            cached = DateTime.MinValue.ToLocalTime();
                        else
                            cached = Convert.ToDateTime(vw[0]["CacheDate"]);

                        System.IO.FileInfo local = new FileInfo(fp);
                        local.LastAccessTimeUtc = DateTime.UtcNow;
                        cachedDate = cached;

                        //Make sure the latest checkout information is set in the cache.
                        Set(item, null, false);

                        return local;
                    }
                }

            }

            return null;
        }

        public bool HasChanged(FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (!IsLoaded)
                InternalLoad();

            DataView vw = new DataView(localDocuments.Tables["Documents"]);
            vw.RowFilter = String.Format("FileLocalPath = '{0}'", file.FullName.Replace("'", "''"));
            if (vw.Count > 0)
            {
                if (vw[0]["FileModified"] == DBNull.Value)
                    return false;

                DateTime lastmodified = DateTime.SpecifyKind(Convert.ToDateTime(vw[0]["FileModified"]), DateTimeKind.Utc);

                DateTime filelastmodified = file.LastWriteTimeUtc;
                if (lastmodified >= filelastmodified)
                    return false;
                else
                    return true;

            }

            vw = new DataView(localDocuments.Tables["Precedents"]);
            vw.RowFilter = String.Format("FileLocalPath = '{0}'", file.FullName.Replace("'", "''"));
            if (vw.Count > 0)
            {
                if (vw[0]["FileModified"] == DBNull.Value)
                    return false;

                DateTime lastmodified = DateTime.SpecifyKind(Convert.ToDateTime(vw[0]["FileModified"]), DateTimeKind.Utc);

                DateTime filelastmodified = file.LastWriteTimeUtc;
                if (lastmodified >= filelastmodified)
                    return false;
                else
                    return true;

            }

            return false;
        }

        public bool IsCheckedOut(FileInfo file)
        {
            if (!Session.CurrentSession.IsLoggedIn)
                return false;

            if (file == null)
                throw new ArgumentNullException("file");

            if (!IsLoaded)
                InternalLoad();

            DataView vw = new DataView(localDocuments.Tables["Documents"]);
            vw.RowFilter = String.Format("FileLocalPath = '{0}'", file.FullName.Replace("'", "''"));
            if (vw.Count > 0)
            {
                if (!Convert.IsDBNull(vw[0]["doccheckedoutby"]) && IsCurrentInstance(vw[0].Row))
                {
                    int by = Convert.ToInt32(vw[0]["doccheckedoutby"]);
                    return (by == Session.CurrentSession.CurrentUser.ID);
                }
            }

            return false;
        }

        public bool HasChanged(IStorageItem item)
        {
            ValidateItem(item);

            if (!IsLoaded)
                InternalLoad();

            DocumentVersion version = item as DocumentVersion;
            Precedent prec = item as Precedent;
            PrecedentVersion precVersion = item as PrecedentVersion;

            DataView vw = new DataView(localDocuments.Tables["Documents"]);

            if (version != null)
                vw = new DataView(localDocuments.Tables["Documents"]);
            if (prec != null)
                vw = new DataView(localDocuments.Tables["Precedents"]);
            else if (precVersion != null)
                vw = new DataView(localDocuments.Tables["Precedents"]);

            if (vw != null)
            {
                vw.RowFilter = GetItemFilter(item);
                if (vw.Count > 0)
                {
                    if (vw[0]["FileModified"] == DBNull.Value)
                        return false;

                    DateTime lastmodified = DateTime.SpecifyKind(Convert.ToDateTime(vw[0]["FileModified"]), DateTimeKind.Utc);

                    string fp = Convert.ToString(vw[0]["FileLocalPath"]);
                    if (File.Exists(fp) == false)
                        return false;

                    DateTime filelastmodified = File.GetLastWriteTimeUtc(fp);
                    if (lastmodified >= filelastmodified)
                        return false;
                    else
                        return true;

                }
            }

            return false;
        }


        public bool IsDifferentToServer(IStorageItem item)
        {
            ValidateItem(item);

            if (!IsLoaded)
                InternalLoad();

            DocumentVersion version = item as DocumentVersion;
            Precedent prec = item as Precedent;
            PrecedentVersion precVersion = item as PrecedentVersion;

            DateTime? updated = null;
            DataView vw = null;

            if (version != null)
            {
                vw = new DataView(localDocuments.Tables["Documents"]);
                updated = version.LastUpdated;
                if (updated == null)
                    updated = version.Created;
            }
            if (prec != null)
            {
                vw = new DataView(localDocuments.Tables["Precedents"]);
                if (prec.TrackingStamp.Updated.IsNull)
                    updated = null;
                else
                    updated = (DateTime)prec.TrackingStamp.Updated;

                if (updated == null)
                {
                    if (prec.TrackingStamp.Created.IsNull)
                        updated = null;
                    else
                        updated = (DateTime)prec.TrackingStamp.Created;
                }
            }
            else if (precVersion != null)
            {
                vw = new DataView(localDocuments.Tables["Precedents"]);
                updated = precVersion.LastUpdated;
                if (updated == null)
                    updated = precVersion.Created;
            }


            if (updated != null)
            {
                if (vw != null)
                {
                    vw.RowFilter = GetItemFilter(item);
                    if (vw.Count > 0)
                    {
                        if (!Convert.IsDBNull(vw[0]["CacheDate"]))
                        {
                            DateTime cached = Convert.ToDateTime(vw[0]["CacheDate"]);
                            if (cached.ToFileTimeUtc() >= updated.Value.ToFileTimeUtc())
                                return false;
                        }
                    }
                }
            }

            return true;
        }


        public bool IsCheckedOut(IStorageItem item)
        {
            ValidateItem(item);

            if (!IsLoaded)
                InternalLoad();

            DocumentVersion version = item as DocumentVersion;
            if (version != null)
            {
                DataView vw = new DataView(localDocuments.Tables["Documents"]);
                vw.RowFilter = GetItemFilter(version);
                if (vw.Count > 0)
                {
                    if (!Convert.IsDBNull(vw[0]["doccheckedoutby"]))
                    {
                        int by = Convert.ToInt32(vw[0]["doccheckedoutby"]);
                        return (by == Session.CurrentSession.CurrentUser.ID);
                    }
                }
            }

            return false;
        }

        private long GetLocalCacheSize()
        {
            if (!Session.CurrentSession.IsLoggedIn)
                return 0;
            else
            {
                long total = 0;
                foreach (FileInfo file in LocalDocumentDirectory.GetFiles())
                {
                    if (file.Name == DatabaseName)
                        continue;
                    total += file.Length;
                }

                foreach (FileInfo file in LocalPrecedentDirectory.GetFiles())
                {
                    if (file.Name == DatabaseName)
                        continue;
                    total += file.Length;
                }

                return total;
            }
        }

        internal void CleanUp()
        {
            if (!IsLoaded)
                InternalLoad();

            CleanUpFiles();
            SyncDatabaseWithFiles();
            SyncFilesWithDatabase();
            Save();
        }

        private void CleanUpFiles()
        {
            if (!IsLoaded)
                InternalLoad();

            if (Session.CurrentSession.IsLoggedIn)
            {
                long currentSize = GetLocalCacheSize();

                List<FileInfo> files = new List<FileInfo>(LocalPrecedentDirectory.GetFiles());
                files.AddRange(LocalDocumentDirectory.GetFiles());
                files.Sort(new Comparison<FileInfo>(this.Sort));

                int filecount = files.Count;
                while (currentSize > LimitSize && files.Count > 1)
                {
                    FileInfo file = files[0];
                    if (IsCheckedOut(file) || HasChanged(file))
                    {
                        files.Remove(file);
                    }
                    else
                    {
                        try
                        {
                            if (file.LastAccessTimeUtc.AddSeconds(20) < DateTime.UtcNow)
                            {
                                file.Delete();
                                currentSize -= file.Length;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine(ex);
                        }
                        finally
                        {
                            files.Remove(file);
                        }
                    }
                }
            }
        }


        private int Sort(FileInfo a, FileInfo b)
        {
            DateTime aTimeUtc = a.CreationTimeUtc > a.LastWriteTimeUtc ? a.CreationTimeUtc : a.LastWriteTimeUtc;
            DateTime bTimeUtc = b.CreationTimeUtc > b.LastWriteTimeUtc ? b.CreationTimeUtc : b.LastWriteTimeUtc;
            return aTimeUtc.CompareTo(bTimeUtc);
        }

        private void SyncFilesWithDatabase()
        {
            if (!IsLoaded)
                Load();

            if (Session.CurrentSession.IsLoggedIn)
            {
                SyncDocumentFilesWithDatabase(0);
                SyncDocumentFilesWithDatabase(1);
            }
        }

        private void SyncDocumentFilesWithDatabase(byte type)
        {
            FileInfo[] files;
            DataView vw;

            if (type == 0)
            {
                files = LocalDocumentDirectory.GetFiles();
                vw = new DataView(localDocuments.Tables["Documents"]);
            }
            else
            {
                files = LocalPrecedentDirectory.GetFiles();
                vw = new DataView(localDocuments.Tables["Precedents"]);
            }

            foreach (FileInfo f in files)
            {
                if (f.Name == DatabaseName)
                    continue;

                vw.RowFilter = String.Format("FileLocalPath = '{0}'", f.FullName.Replace("'", "''"));
                if (vw.Count == 0)
                {
                    try
                    {
                        f.Delete();
                    }
                    catch { }
                    isdirty = true;
                }
            }
        }

        private void SyncDatabaseWithFiles()
        {
            if (!IsLoaded)
                InternalLoad();

            SyncDatabaseWithFiles(0);
            SyncDatabaseWithFiles(1);
        }

        private void SyncDatabaseWithFiles(byte type)
        {
            DataTable dt;
            if (type == 0)
                dt = localDocuments.Tables["Documents"];
            else
                dt = localDocuments.Tables["Precedents"];

            for (int ctr = dt.Rows.Count - 1; ctr >= 0; ctr--)
            {
                DataRow r = dt.Rows[ctr];
                string file = Convert.ToString(r["filelocalpath"]);
                if (file.Length == 0 || System.IO.File.Exists(file) == false)
                {
                    r.Delete();
                    isdirty = true;
                }
            }

            dt.AcceptChanges();
        }


        public void Remove(FileInfo file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            if (!IsCheckedOut(file))
            {
                DataView vw = new DataView(localDocuments.Tables["Documents"]);
                vw.RowFilter = String.Format("FileLocalPath = '{0}'", file.FullName.Replace("'", "''"));
                for (int ctr = vw.Count - 1; ctr >= 0; ctr--)
                {
                    vw[ctr].Delete();
                    isdirty = true;
                }

                vw = new DataView(localDocuments.Tables["Precedents"]);
                vw.RowFilter = String.Format("FileLocalPath = '{0}'", file.FullName.Replace("'", "''"));
                for (int ctr = vw.Count - 1; ctr >= 0; ctr--)
                {
                    vw[ctr].Delete();
                    isdirty = true;
                }
            }
        }

        private void Remove(IStorageItem item)
        {
            ValidateItem(item);

            if (!IsLoaded)
                InternalLoad();

            DataView vw = null;
            DocumentVersion version = item as DocumentVersion;
            Precedent prec = item as Precedent;
            PrecedentVersion precVersion = item as PrecedentVersion;

            if (version != null)
            {
                vw = new DataView(localDocuments.Tables["Documents"]);

            }
            if (prec != null)
            {
                vw = new DataView(localDocuments.Tables["Precedents"]);
            }
            else if (precVersion != null)
            {
                vw = new DataView(localDocuments.Tables["Precedents"]);
            }
            if (vw != null)
            {
                vw.RowFilter = GetItemFilter(item);

                if (vw.Count > 0)
                {
                    string file = Convert.ToString(vw[0]["filelocalpath"]);
                    if (file.Length > 0 && System.IO.File.Exists(file))
                    {
                        try
                        {
                            System.IO.File.Delete(file);
                        }
                        catch { }
                    }

                    isdirty = true;
                }
            }

        }

        private void SetCacheValue<T>(System.Data.DataRow row, string name, object value)
            where T : System.IComparable
        {
            if (row[name] == DBNull.Value)
            {
                if (value != null && value != DBNull.Value)
                    row[name] = value;
            }
            else
            {
                if (value == null || value == DBNull.Value)
                    row[name] = DBNull.Value;
                else
                {
                    T oldval = (T)row[name];
                    T newval = (T)value;

                    if (newval.CompareTo(oldval) != 0)
                        row[name] = value;
                }
            }

        }

        private void SetCacheValue<T>(System.Data.DataRowView row, string name, object value)
            where T : IComparable
        {
            SetCacheValue<T>(row.Row, name, value);
        }



        protected DataRow SetDocumentInfo(OMSDocument doc, System.IO.FileInfo localFile, bool force)
        {
            DataRow nr = null;


            //Update shared data over all documents of the same docid.
            DataTable document = localDocuments.Tables["Documents"];
            DataView vwdocs = new DataView(document);
            vwdocs.RowFilter = String.Format("docid = '{0}'", doc.ID);

            foreach (DataRowView docrow in vwdocs)
            {
                SetCacheValue<string>(docrow, "docdesc", doc.Description);

                SetCacheValue<string>(docrow, "doctype", FWBS.OMS.CodeLookup.GetLookup("DOCTYPE",doc.DocumentType));
                SetCacheValue<string>(docrow, "doctypedesc", FWBS.OMS.CodeLookup.GetLookup("DOCTYPE",doc.DocumentType));

                SetCacheValue<string>(docrow, "docidext", doc.ExternalId);

                SetCacheValue<string>(docrow, "crbyfullname", doc.TrackingStamp.CreatedBy);

                SetCacheValue<long>(docrow, "clid", doc.ClientID);
                SetCacheValue<string>(docrow, "clno", doc.OMSFile.Client.ClientNo);
                SetCacheValue<string>(docrow, "clname", doc.OMSFile.Client.ClientName);

                SetCacheValue<long>(docrow, "fileid", doc.OMSFileID);
                SetCacheValue<string>(docrow, "fileno", doc.OMSFile.FileNo);
                SetCacheValue<string>(docrow, "filedesc", doc.OMSFile.FileDescription);
                SetCacheValue<string>(docrow, "ClientFileNo", String.Format("{0}/{1}", docrow["clno"], docrow["fileno"]));

                SetCacheValue<long>(docrow, "associd", doc.AssocID);
                SetCacheValue<string>(docrow, "assocname", doc.Associate.ContactName);

                IStorageItemLockable lockdoc = doc.GetStorageProvider().GetLockableItem(doc);
                User checkedoutby = lockdoc.CheckedOutBy;
                if (checkedoutby != null)
                {
                    SetCacheValue<int>(docrow, "docCheckedOutBy", checkedoutby.ID);
                    SetCacheValue<string>(docrow, "docCheckedOutByName", checkedoutby.FullName);
                    SetCacheValue<DateTime>(docrow, "docCheckedOut", doc.GetExtraInfo("doccheckedout"));
                    SetCacheValue<string>(docrow, "docCheckedOutLocation", lockdoc.CheckedOutLocation);

                    Common.ApplicationSetting settings = new Common.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "", "OfflineDocuments", -1);
                    if (Convert.ToInt32(settings.GetSetting()) < 0)
                        settings.SetSetting(1);

                }
                else
                {
                    SetCacheValue<int>(docrow, "docCheckedOutBy", null);
                    SetCacheValue<string>(docrow, "docCheckedOutByName", null);
                    SetCacheValue<DateTime>(docrow, "docCheckedOut", null);
                    SetCacheValue<string>(docrow, "docCheckedOutLocation", null);
                }

                if (docrow.Row.RowState != DataRowState.Unchanged)
                    isdirty = true;
            }

            return nr;
        }

        protected DataRow SetDocumentVersionInfo(DocumentVersion version, System.IO.FileInfo localFile, bool force)
        {
            DataRow nr = null;


            OMSDocument doc = version.ParentDocument;

            DataTable document = localDocuments.Tables["Documents"];
            DataView vw = new DataView(document);
            vw.RowFilter = GetItemFilter(version);

            if (vw.Count > 0)
                nr = vw[0].Row;
            else
                nr = document.NewRow();


            SetCacheValue<int>(nr, "instance_id", Session.CurrentSession._multidb.Number);
            SetCacheValue<string>(nr, "server", Session.CurrentSession._multidb.Server);
            SetCacheValue<string>(nr, "database", Session.CurrentSession._multidb.DatabaseName);

            SetCacheValue<long>(nr, "docid", doc.ID);
            SetCacheValue<Guid>(nr, "verid", version.Id);
            SetCacheValue<string>(nr, "verlabel", version.Label);

            SetCacheValue<string>(nr, "docextension", version.Extension);

            if (version.Created == null)
                SetCacheValue<DateTime>(nr, "created", doc.GetExtraInfo("created"));
            else
                SetCacheValue<DateTime>(nr, "created", version.Created);

            if (force)
            {
                if (version.LastUpdated == null)
                    SetCacheValue<DateTime>(nr, "updated", doc.GetExtraInfo("updated"));
                else
                    SetCacheValue<DateTime>(nr, "updated", version.LastUpdated);
            }

            if (localFile != null)
            {
                DataView duplicates = new DataView(document);
                duplicates.RowFilter = String.Format("FileLocalPath = '{0}'", localFile.FullName.Replace("'", "''"));
                for (int ctr = duplicates.Count - 1; ctr >= 0; ctr--)
                {
                    if (duplicates[ctr].Row != nr)
                        duplicates[ctr].Delete();
                }

                SetCacheValue<string>(nr, "FileLocalPath", localFile.FullName);
                SetCacheValue<string>(nr, "FileName", localFile.Name);
                if (localFile.Extension.StartsWith("."))
                    SetCacheValue<string>(nr, "FileExtension", localFile.Extension.Substring(1));
                else
                    SetCacheValue<string>(nr, "FileExtension", localFile.Extension);
                SetCacheValue<long>(nr, "FileSize", localFile.Length);

                if (force || nr["FileModified"] == DBNull.Value)
                    SetCacheValue<DateTime>(nr, "FileModified", DateTime.UtcNow.AddSeconds(5));
            }

            if (force || nr["CacheDate"] == DBNull.Value)
                SetCacheValue<DateTime>(nr, "CacheDate", System.DateTime.Now);

            if (nr.RowState == DataRowState.Detached)
                document.Rows.Add(nr);

            SetDocumentInfo(doc, localFile, force);

            if (nr.RowState != DataRowState.Unchanged)
                isdirty = true;

            return nr;
        }

        protected DataRow SetPrecedentInfo(Precedent prec, System.IO.FileInfo localFile, bool force)
        {
            DataRow nr = null;

            DataTable precedent = localDocuments.Tables["Precedents"];
            DataView vw = new DataView(precedent);
            vw.RowFilter = GetItemFilter(prec);
            

            if (vw.Count > 0)
                nr = vw[0].Row;
            else
                nr = precedent.NewRow();

            SetCacheValue<int>(nr, "instance_id", Session.CurrentSession._multidb.Number);
            SetCacheValue<string>(nr, "server", Session.CurrentSession._multidb.Server);
            SetCacheValue<string>(nr, "database", Session.CurrentSession._multidb.DatabaseName);

            SetCacheValue<long>(nr, "id", prec.ID);
            SetCacheValue<string>(nr, "type", prec.PrecedentType);
            SetCacheValue<string>(nr, "title", prec.Description);

            if (prec.TrackingStamp.Created.IsNull)
                SetCacheValue<DateTime>(nr, "Created", DBNull.Value);
            else
                SetCacheValue<DateTime>(nr, "Created", (DateTime)prec.TrackingStamp.Created);

            if (force)
            {
                if (prec.TrackingStamp.Updated.IsNull)
                    SetCacheValue<DateTime>(nr, "Updated", DBNull.Value);
                else
                    SetCacheValue<DateTime>(nr, "Updated", (DateTime)prec.TrackingStamp.Updated);
            }

            SetCacheValue<string>(nr, "CreatedBy", prec.TrackingStamp.CreatedBy);

            if (localFile != null)
            {
                DataView duplicates = new DataView(precedent);
                duplicates.RowFilter = String.Format("FileLocalPath = '{0}'", localFile.FullName.Replace("'", "''"));
                for (int ctr = duplicates.Count - 1; ctr >= 0; ctr--)
                {
                    if (duplicates[ctr].Row != nr)
                        duplicates[ctr].Delete();
                }

                SetCacheValue<string>(nr, "FileLocalPath", localFile.FullName);
                SetCacheValue<string>(nr, "FileName", localFile.Name);
                if (localFile.Extension.StartsWith("."))
                    SetCacheValue<string>(nr, "FileExtension", localFile.Extension.Substring(1));
                else
                    SetCacheValue<string>(nr, "FileExtension", localFile.Extension);
                SetCacheValue<long>(nr, "FileSize", localFile.Length);

                if (force || nr["FileModified"] == DBNull.Value)
                    SetCacheValue<DateTime>(nr, "FileModified", DateTime.UtcNow.AddSeconds(5));
            }

            if (force || nr["CacheDate"] == DBNull.Value)
                SetCacheValue<DateTime>(nr, "CacheDate", System.DateTime.Now);

            if (nr.RowState == DataRowState.Detached)
                precedent.Rows.Add(nr);

            if (nr.RowState != DataRowState.Unchanged)
                isdirty = true;

            return nr;
        }

        private DataRow SetPrecedentVersionInfo(PrecedentVersion version, FileInfo localFile, bool force)
        {
            DataRow nr = null;
            
            Precedent prec = version.ParentDocument;

            DataTable precedent = localDocuments.Tables["Precedents"];
            DataView vw = new DataView(precedent);
            vw.RowFilter = GetItemFilter(version);

            if (vw.Count > 0)
                nr = vw[0].Row;
            else
                nr = precedent.NewRow();


            SetCacheValue<int>(nr, "instance_id", Session.CurrentSession._multidb.Number);
            SetCacheValue<string>(nr, "server", Session.CurrentSession._multidb.Server);
            SetCacheValue<string>(nr, "database", Session.CurrentSession._multidb.DatabaseName);

            SetCacheValue<long>(nr, "id", prec.ID);
            SetCacheValue<Guid>(nr, "verid", version.Id);
            SetCacheValue<string>(nr, "verlabel", version.Label);

            if (version.Created == null)
                SetCacheValue<DateTime>(nr, "created", prec.GetExtraInfo("created"));
            else
                SetCacheValue<DateTime>(nr, "created", version.Created);

            if (force)
            {
                if (version.LastUpdated == null)
                    SetCacheValue<DateTime>(nr, "updated", prec.GetExtraInfo("updated"));
                else
                    SetCacheValue<DateTime>(nr, "updated", version.LastUpdated);
            }

            if (localFile != null)
            {
                DataView duplicates = new DataView(precedent);
                duplicates.RowFilter = String.Format("FileLocalPath = '{0}'", localFile.FullName.Replace("'", "''"));
                for (int ctr = duplicates.Count - 1; ctr >= 0; ctr--)
                {
                    if (duplicates[ctr].Row != nr)
                        duplicates[ctr].Delete();
                }

                SetCacheValue<string>(nr, "FileLocalPath", localFile.FullName);
                SetCacheValue<string>(nr, "FileName", localFile.Name);
                if (localFile.Extension.StartsWith("."))
                    SetCacheValue<string>(nr, "FileExtension", localFile.Extension.Substring(1));
                else
                    SetCacheValue<string>(nr, "FileExtension", localFile.Extension);
                SetCacheValue<long>(nr, "FileSize", localFile.Length);

                if (force || nr["FileModified"] == DBNull.Value)
                    SetCacheValue<DateTime>(nr, "FileModified", DateTime.UtcNow.AddSeconds(5));
            }

            if (force || nr["CacheDate"] == DBNull.Value)
                SetCacheValue<DateTime>(nr, "CacheDate", System.DateTime.Now);

            if (nr.RowState == DataRowState.Detached)
                precedent.Rows.Add(nr);

            UpdatePrecedentInfo(prec, localFile, force);

            if (nr.RowState != DataRowState.Unchanged)
                isdirty = true;

            return nr;
        }

        protected DataRow UpdatePrecedentInfo(Precedent prec, System.IO.FileInfo localFile, bool force)
        {
            DataRow nr = null;

            //Update shared data over all precedents of the same precid.
            DataTable precedent = localDocuments.Tables["Precedents"];
            DataView vw = new DataView(precedent);
            vw.RowFilter = String.Format("id = '{0}'", prec.ID);

            foreach (DataRowView docrow in vw)
            {
                SetCacheValue<string>(docrow, "type", prec.PrecedentType);
                SetCacheValue<string>(docrow, "title", prec.Description);
            }
            return nr;
        }

        public virtual void Set(IStorageItem item, System.IO.FileInfo localFile, bool force)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (localFile != null)
            {
                if (!System.IO.File.Exists(localFile.FullName))
                    return;

            }

            if (!IsLoaded)
                InternalLoad();

            OMSDocument doc = item as OMSDocument;
            DocumentVersion version = item as DocumentVersion;
            Precedent prec = item as Precedent;
            PrecedentVersion precVersion = item as PrecedentVersion;

            DataRow nr = null;

            //Stops caching files outside of the designated caching directory
            if (doc != null)
            {
                if (localFile != null && localFile.Directory.FullName.ToUpperInvariant() != LocalDocumentDirectory.FullName.ToUpperInvariant())
                    return;

                nr = SetDocumentInfo(doc, localFile, force);
            }
            if (version != null)
            {
                if (localFile != null && localFile.Directory.FullName.ToUpperInvariant() != LocalDocumentDirectory.FullName.ToUpperInvariant())
                    return;

                nr = SetDocumentVersionInfo(version, localFile, force);
            }
            if (prec != null)
            {
                if (localFile != null && localFile.Directory.FullName.ToUpperInvariant() != LocalPrecedentDirectory.FullName.ToUpperInvariant())
                    return;

                nr = SetPrecedentInfo(prec, localFile, force);
            }
            else if (precVersion != null)
            {
                if (localFile != null && localFile.Directory.FullName.ToUpperInvariant() != LocalPrecedentDirectory.FullName.ToUpperInvariant())
                    return;

                nr = SetPrecedentVersionInfo(precVersion, localFile, force);
            }


            if (isdirty)
            {
                Save();
                if (nr != null)
                    nr.AcceptChanges();
            }
        }

 
        public void Save()
        {
            if (!isloaded)
                InternalLoad();

            if (IsDirty)
            {
                WriteXml();
            }
        }

        private void WriteXml()
        {
            try
            {
                mutex.WaitOne();
                localDocuments.WriteXml(LocalDatabase.FullName, XmlWriteMode.WriteSchema);
                isdirty = false;
                LocalDatabase.Refresh();
                lastModified = LocalDatabase.LastWriteTime;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        private void ReadXml()
        {
            try
            {
                mutex.WaitOne();
                localDocuments.Clear();
                localDocuments.ReadXml(LocalDatabase.FullName, XmlReadMode.ReadSchema);
                lastModified = LocalDatabase.LastWriteTime;
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        private void Load()
        {
            InternalLoad();
            CleanUp();
        }

        protected void InternalLoad()
        {
            if (!IsLoaded)
            {
                Build();
            }

            if (!File.Exists(LocalDatabase.FullName) && Session.CurrentSession.IsLoggedIn)
            {
                DirectoryInfo dir = LocalDocumentDirectory;
                System.IO.FileInfo legacy = new FileInfo(Path.Combine(dir.FullName, DatabaseName));
                if (System.IO.File.Exists(legacy.FullName))
                {
                    legacy.MoveTo(LocalDatabase.FullName);
                }
            }

            if (!System.IO.File.Exists(LocalDatabase.FullName))
            {
                //UTCFIX: DM - 14/12/06
                //Got to change the file name to allow date conversion for UTC issue.
                if (System.IO.File.Exists(OldDatabase.FullName))
                {
                    using (DataSet oldcache = new DataSet())
                    {
                        oldcache.ReadXml(OldDatabase.FullName, XmlReadMode.ReadSchema);
                        foreach (DataTable dt in oldcache.Tables)
                        {
                            foreach (DataColumn col in dt.Columns)
                            {
                                if (col.DataType == typeof(DateTime))
                                {
                                    foreach (DataRow r in dt.Rows)
                                    {
                                        if (r[col] != DBNull.Value)
                                        {
                                            DateTime dte = (DateTime)r[col];
                                            dte = DateTime.SpecifyKind(dte, DateTimeKind.Local);
                                            r[col] = dte.ToUniversalTime();
                                        }
                                    }
                                }
                            }


                        }
                        oldcache.AcceptChanges();
                        localDocuments.Merge(oldcache, false, MissingSchemaAction.Ignore);



                    }

                }

                WriteXml();

                if (System.IO.File.Exists(OldDatabase.FullName))
                {
                    try
                    {
                        System.IO.File.Delete(OldDatabase.FullName);
                    }
                    catch { }
                }
            }

            if (System.IO.File.Exists(LocalDatabase.FullName))
            {
                try
                {
                    ReadXml();
                }
                catch
                {
                    if (System.IO.File.Exists(LocalDatabase.FullName))
                    {
                        try
                        {
                            LocalDatabase.Delete();
                        }
                        catch { }
                    }
                    WriteXml();
                }
            }

            isloaded = true;
        }

        private DataTable CheckForLocalDocumentChanges(DataTable dt)
        {
            if (dt.Columns.Contains("HasChanged") == false)
                dt.Columns.Add("HasChanged", typeof(bool));

            for (int ctr = dt.Rows.Count - 1; ctr >= 0; ctr--)
            {
                DataRow r = dt.Rows[ctr];

                if (r.RowState != DataRowState.Deleted)
                {
                    r["HasChanged"] = false;

                    if (r["FileModified"] != DBNull.Value)
                    {
                        DateTime lastmodified = DateTime.SpecifyKind(Convert.ToDateTime(r["FileModified"]), DateTimeKind.Utc);
                        string file = Convert.ToString(r["FileLocalPath"]);

                        if (File.Exists(file))
                        {
                            DateTime filelastmodified = File.GetLastWriteTimeUtc(file);
                            if (lastmodified < filelastmodified)
                                r["hasChanged"] = true;
                        }
                    }
                }

            }

            dt.AcceptChanges();
            return dt;
        }

        public DataTable GetLocalDocumentInfo()
        {
            if (!IsLoaded)
            {
                Load();
            }

            DataTable dt;

            if (Session.CurrentSession.IsLoggedIn)
            {
                DataView vw = new DataView(localDocuments.Tables["Documents"]);
                vw.RowFilter = GetInstanceFilter();
                dt = vw.ToTable();
            }
            else
                dt = localDocuments.Tables["Documents"].Copy();

            if (!dt.Columns.Contains("docidext"))
            {
                dt.Columns.Add("docidext", typeof(string));
                dt.AcceptChanges();
            }


            CheckForLocalDocumentChanges(dt);

            return dt;

        }

        public DataTable GetLocalPrecedentInfo()
        {
            if (!IsLoaded)
            {
                Load();
            }

            DataTable dt;

            if (Session.CurrentSession.IsLoggedIn)
            {
                DataView vw = new DataView(localDocuments.Tables["Precedents"]);
                vw.RowFilter = GetInstanceFilter();
                dt = vw.ToTable();
            }
            else
                dt = localDocuments.Tables["Precedents"].Copy();

            CheckForLocalDocumentChanges(dt);

            return dt;

        }


        #endregion

        #region Private Methods

        private void Build()
        {
            if (localDocuments == null)
                localDocuments = new DataSet();

            localDocuments.DataSetName = "LocalDatabase";

            DataTable document = null;
            if (localDocuments.Tables.Contains("Documents"))
                document = localDocuments.Tables["Documents"];
            else
            {
                document = new System.Data.DataTable("Documents");
                localDocuments.Tables.Add(document);
            }

            if (!document.Columns.Contains("verid"))
                document.Columns.Add("verid", typeof(Guid));

            if (document.PrimaryKey.Length > 0)
                document.PrimaryKey = new DataColumn[0];

            if (!document.Columns.Contains("instance_id"))
                document.Columns.Add("instance_id", typeof(int));

            if (!document.Columns.Contains("server"))
                document.Columns.Add("server", typeof(string));

            if (!document.Columns.Contains("database"))
                document.Columns.Add("database", typeof(string));

            if (!document.Columns.Contains("docid"))
                document.Columns.Add("docid", typeof(long));

            if (!document.Columns.Contains("doctype"))
                document.Columns.Add("doctype", typeof(string));

            if (!document.Columns.Contains("doctypedesc"))
                document.Columns.Add("doctypedesc", typeof(string));

            if (!document.Columns.Contains("created"))
                document.Columns.Add("created", typeof(DateTime)).DateTimeMode = DataSetDateTime.Utc;

            if (!document.Columns.Contains("updated"))
                document.Columns.Add("updated", typeof(DateTime)).DateTimeMode = DataSetDateTime.Utc;

            if (!document.Columns.Contains("crbyfullname"))
                document.Columns.Add("crbyfullname", typeof(string));

            if (!document.Columns.Contains("verlabel"))
                document.Columns.Add("verlabel", typeof(string));

            if (!document.Columns.Contains("clid"))
                document.Columns.Add("clid", typeof(long));

            if (!document.Columns.Contains("clno"))
                document.Columns.Add("clno", typeof(string));

            if (!document.Columns.Contains("ClientFileNo"))
                document.Columns.Add("ClientFileNo", typeof(string));

            if (!document.Columns.Contains("clname"))
                document.Columns.Add("clname", typeof(string));

            if (!document.Columns.Contains("fileid"))
                document.Columns.Add("fileid", typeof(long));

            if (!document.Columns.Contains("fileno"))
                document.Columns.Add("fileno", typeof(string));

            if (!document.Columns.Contains("filedesc"))
                document.Columns.Add("filedesc", typeof(string));

            if (!document.Columns.Contains("associd"))
                document.Columns.Add("associd", typeof(long));

            if (!document.Columns.Contains("assocname"))
                document.Columns.Add("assocname", typeof(string));

            if (!document.Columns.Contains("docdesc"))
                document.Columns.Add("docdesc", typeof(string));

            if (!document.Columns.Contains("docextension"))
                document.Columns.Add("docextension", typeof(string));

            if (!document.Columns.Contains("docCheckedOutBy"))
                document.Columns.Add("docCheckedOutBy", typeof(int));

            if (!document.Columns.Contains("docCheckedOutByName"))
                document.Columns.Add("docCheckedOutByName", typeof(string));

            if (!document.Columns.Contains("docCheckedOut"))
                document.Columns.Add("docCheckedOut", typeof(DateTime)).DateTimeMode = DataSetDateTime.Utc;

            if (!document.Columns.Contains("docCheckedOutLocation"))
                document.Columns.Add("docCheckedOutLocation", typeof(string));

            if (!document.Columns.Contains("FileLocalPath"))
                document.Columns.Add("FileLocalPath", typeof(string));

            if (!document.Columns.Contains("FileName"))
                document.Columns.Add("FileName", typeof(string));

            if (!document.Columns.Contains("FileExtension"))
                document.Columns.Add("FileExtension", typeof(string));

            if (!document.Columns.Contains("FileSize"))
                document.Columns.Add("FileSize", typeof(long));

            if (!document.Columns.Contains("CacheDate"))
                document.Columns.Add("CacheDate", typeof(DateTime)).DateTimeMode = DataSetDateTime.Utc;

            if (!document.Columns.Contains("FileModified"))
                document.Columns.Add("FileModified", typeof(DateTime)).DateTimeMode = DataSetDateTime.Utc;

            if (!document.Columns.Contains("docidext"))
                document.Columns.Add("docidext", typeof(string));

            //Precedent cache.
            DataTable precedent = null;
            if (localDocuments.Tables.Contains("Precedents"))
                precedent = localDocuments.Tables["Precedents"];
            else
            {
                precedent = new System.Data.DataTable("Precedents");
                localDocuments.Tables.Add(precedent);
            }

            if (precedent.PrimaryKey.Length > 0)
                precedent.PrimaryKey = new DataColumn[0];

            if (!precedent.Columns.Contains("Instance_Id"))
                precedent.Columns.Add("Instance_Id", typeof(int));

            if (!precedent.Columns.Contains("Server"))
                precedent.Columns.Add("Server", typeof(string));

            if (!precedent.Columns.Contains("Database"))
                precedent.Columns.Add("Database", typeof(string));

            if (!precedent.Columns.Contains("Id"))
                precedent.Columns.Add("Id", typeof(long));

            if (!precedent.Columns.Contains("Type"))
                precedent.Columns.Add("Type", typeof(string));

            if (!precedent.Columns.Contains("Title"))
                precedent.Columns.Add("Title", typeof(string));

            if (!precedent.Columns.Contains("Created"))
                precedent.Columns.Add("Created", typeof(DateTime)).DateTimeMode = DataSetDateTime.Utc;

            if (!precedent.Columns.Contains("Updated"))
                precedent.Columns.Add("Updated", typeof(DateTime)).DateTimeMode = DataSetDateTime.Utc;

            if (!precedent.Columns.Contains("CreatedBy"))
                precedent.Columns.Add("CreatedBy", typeof(string));

            if (!precedent.Columns.Contains("FileLocalPath"))
                precedent.Columns.Add("FileLocalPath", typeof(string));

            if (!precedent.Columns.Contains("FileName"))
                precedent.Columns.Add("FileName", typeof(string));

            if (!precedent.Columns.Contains("FileExtension"))
                precedent.Columns.Add("FileExtension", typeof(string));

            if (!precedent.Columns.Contains("FileSize"))
                precedent.Columns.Add("FileSize", typeof(long));

            if (!precedent.Columns.Contains("CacheDate"))
            {
                precedent.Columns.Add("CacheDate", typeof(DateTime)).DateTimeMode = DataSetDateTime.Utc;
            }

            if (!precedent.Columns.Contains("FileModified"))
            {
                precedent.Columns.Add("FileModified", typeof(DateTime)).DateTimeMode = DataSetDateTime.Utc;
            }

            if (!precedent.Columns.Contains("verid"))
                precedent.Columns.Add("verid", typeof(Guid));

            if (!precedent.Columns.Contains("verlabel"))
                precedent.Columns.Add("verlabel", typeof(string));

            isdirty = true;
        }

        private static FileInfo GetLocalCacheDatabase()
        {
            Common.Directory dir = Global.GetAppDataPath();
            return new FileInfo(Path.Combine(dir, DatabaseName));
        }

        #endregion

        #region ICacheable Members

        void Caching.ICacheable.Clear()
        {
            Session.CurrentSession.Disconnected -= new EventHandler(OMS_Disconnected);
            Session.CurrentSession.Connected -= new EventHandler(OMS_Connected);
            
            if (localDocuments != null)
            {
                localDocuments.Dispose();
                localDocuments = null;
            }

            if (mutex != null)
            {
                mutex.Dispose();
                mutex = null;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            ((Caching.ICacheable)this).Clear();
        }

        #endregion


    }
}
