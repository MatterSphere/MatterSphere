using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace FWBS.OMS.DocumentManagement
{
    using System.Data;
    using System.Reflection;
    using DocumentManagement.Storage;
    using FWBS.OMS.Data;

    public static class PrecStatus
    {
        public const string Final = "FINAL";
        public const string Emailed = "EMAILED";
        public const string Draft = "DRAFT";
    }


    public sealed class PrecedentVersion : IStorageItemVersion, IStorageItemVersionable, IStorageItemDuplication, IStorageItemLockable, Interfaces.IExtraInfo
    {

        #region Fields

        private Precedent parent;

        private Guid id;
        private long precid;
        private Guid? parentid;
        private byte depth;

        private int version;
        private string token = String.Empty;
        private string label = String.Empty;
        private string comment = string.Empty;
        private string status = string.Empty;

        private string checksum = null;
        private string preview = null;

        //version header editing.
        private int? createdBy;
        private DateTime? created;
        private int? updatedBy;
        private DateTime? updated;

        private bool isnew;
        private bool isdirty;

        //Storage provider must set this to false when file is saved successfully.
        private bool accepted = false;
        private bool deleted = false;

        private long? scriptVersionID;
        Assembly assembly = null;

        #endregion
        
        #region Constructors
        private PrecedentVersion() { }

        internal PrecedentVersion(Precedent parent, System.Data.DataRow r)
        {
            Refresh(parent, r);

            isnew = false;
            isdirty = false;

        }

        internal PrecedentVersion(Precedent parent)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            id = Guid.NewGuid();
            this.parent = parent;

            GenerateNextVersionNumber();

            isnew = true;
            isdirty = true;
            parentid = null;
        }

        internal PrecedentVersion(PrecedentVersion original, bool subVersion)
        {
            if (original == null)
                throw new ArgumentNullException("original");

            id = Guid.NewGuid();
            parent = original.ParentDocument;

            if (!subVersion)
                parentid = original.parentid;
            else
                parentid = original.id;

            GenerateNextVersionNumber();

            isnew = true;
            isdirty = true;
        }
        #endregion
         
        #region Properties

        public Guid Id
        {
            get
            {
                return id;
            }
        }

        public Guid? ParentId
        {
            get
            {
                return parentid;
            }
        }

        public int Version
        {
            get
            {
                return version;
            }
        }

        public byte Depth
        {
            get
            {
                return depth;
            }
        }

        public bool IsSubVersion
        {
            get
            {
                return parentid.HasValue;
            }
        }

        public string Label
        {
            get
            {
                return label;
            }
            set
            {
                label = value;
            }

        }

        public string Comments
        {
            get
            {
                return comment;
            }
            set
            {
                if (value == null)
                    value = String.Empty;
                if (value != comment)
                {
                    comment = value;
                    isdirty = true;
                }
            }
        }

        public string Status
        {
            get
            {
                return status;
            }
            set
            {
                if (value == null)
                    value = String.Empty;

                status = value;
            }
        }

        public string StatusDescription
        {
            get
            {
                return CodeLookup.GetLookup("DOCSTATUS", Status);
            }
        }

        public Precedent ParentDocument
        {
            get
            {
                return parent;
            }
        }

        internal Storage.IStorageItemLockable BaseLockableStorageItem
        {
            get
            {
                return (Storage.IStorageItemLockable)parent;
            }
        }


        internal Storage.IStorageItemDuplication BaseDuplicationStorageItem
        {
            get
            {
                return (Storage.IStorageItemDuplication)parent;
            }
        }

        public DateTime? Created
        {
            get
            {
                return created;
            }
        }


        public string CreatedBy
        {
            get
            {
                if (createdBy.HasValue)
                    return User.GetUserFullName(createdBy.Value, "n/a");
                else
                    return "n/a";
            }
        }

        public DateTime? LastUpdated
        {
            get
            {
                return updated;
            }
        }

        public string LastUpdatedBy
        {
            get
            {
                if (updatedBy.HasValue)
                    return User.GetUserFullName(updatedBy.Value, "n/a");
                else
                    return "n/a";
            }
        }

        public long? ScriptVersionID
        {
            get
            {
                return scriptVersionID;
            }
            set
            {
                scriptVersionID = value;
            }
        }

        public Storage.IStorageItem BaseStorageItem
        {
            get
            {
                return (Storage.IStorageItem)parent;
            }
        }


        internal Storage.IStorageItemVersionable BaseVersionalStorageItem
        {
            get
            {
                return (Storage.IStorageItemVersionable)parent;
            }
        }


      
        #endregion

        #region Methods

        internal void Refresh(Precedent parent, DataRow r)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            if (r == null)
                throw new ArgumentNullException("r");

            this.parent = parent;

            id = (Guid)(r["verid"]);
            precid = parent.ID;

            version = Convert.ToInt32(r["vernumber"]);
            label = Convert.ToString(r["verlabel"]);
            token = Convert.ToString(r["vertoken"]);
            comment = Convert.ToString(r["vercomments"]);
            depth = Convert.ToByte(r["verdepth"]);
            deleted = Convert.ToBoolean(r["verdeleted"]);
            accepted = !deleted;

            if (r.Table.Columns.Contains("verstatus"))
                status = Convert.ToString(r["verstatus"]);

            if (Convert.IsDBNull(r["verparent"]))
                parentid = null;
            else
                parentid = (Guid)(r["verparent"]);

            if (Convert.IsDBNull(r["createdby"]))
                createdBy = null;
            else
                createdBy = Convert.ToInt32(r["createdby"]);

            if (Convert.IsDBNull(r["updatedby"]))
                updatedBy = null;
            else
                updatedBy = Convert.ToInt32(r["updatedby"]);

            if (Convert.IsDBNull(r["created"]))
                created = null;
            else
                created = Convert.ToDateTime(r["created"]);

            if (Convert.IsDBNull(r["updated"]))
                updated = null;
            else
                updated = Convert.ToDateTime(r["updated"]);

            if (Convert.IsDBNull(r["verScriptVersionID"]))
                scriptVersionID = null;
            else
                scriptVersionID = Convert.ToInt64(r["verScriptVersionID"]);
        }
        
        private void GenerateNextVersionNumber()
        {
            System.Data.DataTable dt = parent.GetVersionsTable(false);

            if (IsSubVersion == false)
            {
                DataView vw = new DataView(dt);
                vw.RowFilter = "verparent is null";
                vw.Sort = "vernumber desc";
                if (vw.Count > 0)
                    version = Convert.ToInt32(vw[0]["vernumber"]) + 1;
                else
                    version = 1;

                depth = 0;
                label = version.ToString();
            }
            else
            {
                DataView parent_vw = new DataView(dt);
                parent_vw.RowFilter = String.Format("verid = '{0}'", parentid.Value);

                DataView same_level_wv = new DataView(dt);
                same_level_wv.RowFilter = String.Format("verparent = '{0}'", parentid.Value);
                same_level_wv.Sort = "vernumber desc";
                if (same_level_wv.Count > 0)
                {
                    version = Convert.ToInt32(same_level_wv[0]["vernumber"]) + 1;
                    depth = Convert.ToByte(same_level_wv[0]["verdepth"]);
                }
                else
                {
                    if (parent_vw.Count > 0)
                    {
                        version = 1;
                        depth = Convert.ToByte(parent_vw[0]["verdepth"]);
                        depth++;
                    }
                    else
                    {
                        version = 1;
                        depth = 1;
                    }
                }
                StringBuilder sb = new StringBuilder();
                sb.Append(version);
                GenerateLabel(dt, parentid, sb);
                label = sb.ToString();
            }


        }

        private void GenerateLabel(DataTable versions, Guid? currentId, StringBuilder sb)
        {
            if (currentId.HasValue)
            {
                DataView vw = new DataView(versions);
                vw.RowFilter = String.Format("verid = '{0}'", currentId.Value);

                if (vw.Count > 0)
                {
                    sb.Insert(0, ".");
                    sb.Insert(0, vw[0]["vernumber"]);

                    if (!Convert.IsDBNull(vw[0]["verparent"]))
                    {
                        currentId = (Guid)(vw[0]["verparent"]);
                        if (currentId.HasValue)
                            GenerateLabel(versions, currentId, sb);
                    }
                }
            }
        }

        internal void Delete()
        {
            isdirty = true;
            deleted = true;
        }

        public override string ToString()
        {
            return label;
        }

        #endregion
        
        #region IStorageItem Members

        public string Name
        {
            get { return String.Format("{0}.{1}", BaseStorageItem.Pointer, Label); }
        }

        public string Pointer
        {
            get
            {
                return String.Format("{0}.{1}", ParentDocument.ID, Label);
            }
        }

        public string DisplayID
        {
            get
            {
                return String.Format("{0}.{1}", ParentDocument.ID, Label);
            }
        }

        public string Extension
        {
            get { return BaseStorageItem.Extension; }
        }

        public string Token
        {
            get
            {
                return token;
            }
            set
            {
                if (value == null)
                    value = String.Empty;
                if (value != token)
                {
                    token = value;
                    isdirty = true;
                }
            }
        }

        public bool Accepted
        {
            get
            {
                return accepted;
            }
            set
            {
                if (value != accepted)
                {
                    accepted = value;

                    isdirty = true;
                }
                if (value && BaseStorageItem.Accepted == false)
                {
                    BaseStorageItem.Accepted = true;
                    isdirty = true;
                }

                if (value && deleted)
                    deleted = false;
            }
        }

        public string Preview
        {
            get
            {
                if (preview == null)
                {
                    if (IsNew == false)
                    {
                        IDataParameter[] pars = new IDataParameter[1];
                        pars[0] = Session.CurrentSession.Connection.CreateParameter("precid", precid);
                        preview = Convert.ToString(Session.CurrentSession.Connection.ExecuteSQLScalar("select precpreview from dbprecedents where precid = @precid", pars));
                    }
                }
                return preview;
            }
            set
            {
                if (Session.CurrentSession.DocumentPreviewEnabled)
                {

                    if (value == null || value == "")
                        preview = String.Empty;
                    else
                        preview = value;

                    isdirty = true;
                }


            }
        }

        public string Checksum
        {
            get
            {
                return checksum;
            }
            set
            {
                if (value != checksum)
                {
                    checksum = value;
                    isdirty = true;
                }
            }
        }

        public void GenerateChecksum(string value)
        {
            if (value == null)
                Checksum = null;
            else
                Checksum = OMSDocument.GenerateChecksum(value);
        }

        public void ChangeStorage(DocumentManagement.Storage.StorageProvider provider, bool transfer)
        {
            throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("MSGCSNTSPONPV", "ChangeStorage is not supported on a Precedent Version, you must change the Storage provider of the primary Precedent.", "").Text);
        }

        StorageSettingsCollection settings = null;
        public StorageSettingsCollection GetSettings()
        {
            if (settings == null)
                settings = BaseStorageItem.GetSettings();
            return settings;
        }

        public void ApplySettings(StorageSettingsCollection settings)
        {
            this.settings = settings;
        }
        public void ClearSettings()
        {
            if (settings != null)
                settings.Clear();
            settings = null;
        }


        public StorageProvider GetStorageProvider()
        {
            return BaseStorageItem.GetStorageProvider();
        }

        public IStorageItemType GetItemType()
        {
            return BaseStorageItem.GetItemType();
        }

        public System.IO.FileInfo GetIdealLocalFile()
        {
            
            System.IO.DirectoryInfo dir = StorageManager.CurrentManager.LocalDocuments.LocalPrecedentDirectory;
            string fp = System.IO.Path.Combine(dir.FullName, String.Format("{0}.{1}", FWBS.Common.FilePath.ExtractInvalidChars(Pointer), FWBS.Common.FilePath.ExtractInvalidChars(Extension)));
            return new System.IO.FileInfo(fp);
        }

        public System.Drawing.Icon GetIcon()
        {
            return Common.IconReader.GetFileIcon(String.Format("test.{0}", Extension), Common.IconReader.IconSize.Small, false);
        }

        bool IStorageItem.Supports(DocumentManagement.Storage.StorageFeature feature)
        {
            return BaseStorageItem.Supports(feature);
        }

        public bool IsLatestVersion
        {
            get
            {
                IStorageItemVersion current = BaseVersionalStorageItem.GetLatestVersion();
                return (current.Id == this.Id);
            }
        }

        IStorageItem IStorageItem.GetConflict()
        {
            return BaseStorageItem.GetConflict();
        }

        public bool IsConflicting
        {
            get
            {
                return BaseStorageItem.IsConflicting;
            }
        }

        public bool IsNew
        {
            get
            {
                return isnew;
            }
        }

        public bool IsDirty
        {
            get
            {
                return isdirty;
            }
        }

        public void Update()
        {
            try
            {
                Session.CurrentSession.Connection.Connect(true);
                Session.CurrentSession.Connection.BeginTransaction();
                BaseStorageItem.Update();
                InternalUpdate();
                Session.CurrentSession.Connection.CommitTransaction();

                //IMPORTANT: Force a refresh of the internal cache of version under the parent document.
                ParentDocument.GetVersionsTable(true);
            }
            catch
            {
                Session.CurrentSession.Connection.RollbackTransaction();
                throw;
            }
            finally
            {
                Session.CurrentSession.Connection.Disconnect();
            }
        }

        private void GetScriptVersionID()
        {
            //Only perform get script version process if there is a script
            if (!this.ParentDocument.HasScript)
                return;

            if (assembly == null)
                assembly = Assembly.Load("OMS.UI, Version=" + Session.CurrentSession.AssemblyVersion.ToString() + ", Culture=neutral, PublicKeyToken=7212801a92a1726d");

            Type scriptArchiver = assembly.GetType("FWBS.OMS.UI.Windows.ScriptVersionDataArchiver");
            var archiverinstance = Activator.CreateInstance(scriptArchiver);
            MethodInfo theMethod = scriptArchiver.GetMethod("SaveData");

            EventInfo eventinfo = scriptArchiver.GetEvent("IndividualLinkSaved");
            Type handlerType = eventinfo.EventHandlerType;
            MethodInfo eventMethod = this.GetType().GetMethod("AssignScriptVersionID");
            Delegate handler = Delegate.CreateDelegate(handlerType, this, eventMethod);
            eventinfo.AddEventHandler(archiverinstance, handler);

            object[] objects = new object[6];
            DataSet versionDataToSave = new DataSet();
            versionDataToSave.Tables.Add(BuildVersionDataTable(this.ParentDocument.Script.Code));
            objects[0] = versionDataToSave;
            objects[1] = this.ParentDocument.Script.Code;
            objects[2] = this.ParentDocument.Script.Version;
            objects[3] = Guid.NewGuid();
            objects[4] = true;
            objects[5] = false;

            theMethod.Invoke(archiverinstance, objects);
        }

        private DataTable BuildVersionDataTable(string scriptcode)
        {
            string sql = @"select * from dbScript where scrCode = @code";
            IConnection connection = FWBS.OMS.Session.CurrentSession.CurrentConnection;
            List<IDataParameter> parList = new List<IDataParameter>();
            parList.Add(connection.CreateParameter("code", scriptcode));
            System.Data.DataTable dt = connection.ExecuteSQL(sql, parList);
            dt.TableName = "dbScript";
            return dt;
        }

        public void AssignScriptVersionID(object sender, LinkItemSavedEventArgs e)
        {
            scriptVersionID = e.versionID;
        }

        internal void InternalUpdate()
        {
            if (isdirty && ParentDocument.IsNew == false)
            {
                try
                {
                    GetScriptVersionID(); 
                    Session.CurrentSession.Connection.Connect(true);
                    Session.CurrentSession.Connection.BeginTransaction();

                    if (isnew)
                    {
                        createdBy = Session.CurrentSession.CurrentUser.ID;
                        created = System.DateTime.Now;

                        string sql = "insert into dbprecedentversion (verid, precid, vernumber, verparent, verdepth, verlabel, vercomments, vertoken, verchecksum, createdby, created, verstatus, verdeleted, verScriptVersionID) values (@id, @precid, @version, @parent, @depth, @label, @comments, @token, @checksum, @createdby, @created, @status, @deleted, @verScriptVersionID)";
                        IDataParameter[] pars = new IDataParameter[14];
                        pars[0] = Session.CurrentSession.Connection.AddParameter("id", id);
                        pars[1] = Session.CurrentSession.Connection.AddParameter("precid", parent.ID);
                        pars[2] = Session.CurrentSession.Connection.AddParameter("version", version);
                        if (parentid.HasValue)
                            pars[3] = Session.CurrentSession.Connection.AddParameter("parent", parentid);
                        else
                            pars[3] = Session.CurrentSession.Connection.AddParameter("parent", DBNull.Value);
                        pars[4] = Session.CurrentSession.Connection.AddParameter("depth", depth);
                        pars[5] = Session.CurrentSession.Connection.AddParameter("label", label);

                        if (String.IsNullOrEmpty(comment))
                            pars[6] = Session.CurrentSession.Connection.AddParameter("comments", DBNull.Value);
                        else
                            pars[6] = Session.CurrentSession.Connection.AddParameter("comments", comment);

                        pars[7] = Session.CurrentSession.Connection.AddParameter("token", token);

                        if (String.IsNullOrEmpty(checksum))
                            pars[8] = Session.CurrentSession.Connection.AddParameter("checksum", DBNull.Value);
                        else
                            pars[8] = Session.CurrentSession.Connection.AddParameter("checksum", checksum);
                        pars[9] = Session.CurrentSession.Connection.AddParameter("createdby", createdBy);
                        pars[10] = Session.CurrentSession.Connection.AddParameter("created", created);

                        if (String.IsNullOrEmpty(status))
                            pars[11] = Session.CurrentSession.Connection.AddParameter("status", DBNull.Value);
                        else
                            pars[11] = Session.CurrentSession.Connection.AddParameter("status", status);

                        pars[12] = Session.CurrentSession.Connection.AddParameter("deleted", (deleted || accepted == false));

                        if (scriptVersionID != null)
                            pars[13] = Session.CurrentSession.Connection.AddParameter("verScriptVersionID", scriptVersionID);
                        else
                            pars[13] = Session.CurrentSession.Connection.AddParameter("verScriptVersionID", DBNull.Value);

                        Session.CurrentSession.Connection.ExecuteSQL(sql, pars);

                        ParentDocument.AddVersion(this);
                    }
                    else
                    {
                        updatedBy = Session.CurrentSession.CurrentUser.ID;
                        updated = System.DateTime.Now;

                        string sql = "update dbprecedentversion set precid = @precid, vernumber = @version, verparent = @parent, verdepth = @depth, verlabel = @label, vercomments = @comments, vertoken = @token, verchecksum = @checksum, updatedBy = @updatedby, updated = @updated, verstatus = @status, verdeleted = @deleted, verScriptVersionID = @verScriptVersionID where verid = @id";
                        IDataParameter[] pars = new IDataParameter[14];
                        pars[0] = Session.CurrentSession.Connection.AddParameter("id", id);
                        pars[1] = Session.CurrentSession.Connection.AddParameter("precid", parent.ID);
                        pars[2] = Session.CurrentSession.Connection.AddParameter("version", version);
                        if (parentid.HasValue)
                            pars[3] = Session.CurrentSession.Connection.AddParameter("parent", parentid);
                        else
                            pars[3] = Session.CurrentSession.Connection.AddParameter("parent", DBNull.Value);
                        pars[4] = Session.CurrentSession.Connection.AddParameter("depth", depth);
                        pars[5] = Session.CurrentSession.Connection.AddParameter("label", label);
                        if (String.IsNullOrEmpty(comment))
                            pars[6] = Session.CurrentSession.Connection.AddParameter("comments", DBNull.Value);
                        else
                            pars[6] = Session.CurrentSession.Connection.AddParameter("comments", comment);

                        pars[7] = Session.CurrentSession.Connection.AddParameter("token", token);
                        if (String.IsNullOrEmpty(checksum))
                            pars[8] = Session.CurrentSession.Connection.AddParameter("checksum", DBNull.Value);
                        else
                            pars[8] = Session.CurrentSession.Connection.AddParameter("checksum", checksum);
                        pars[9] = Session.CurrentSession.Connection.AddParameter("updatedby", updatedBy);
                        pars[10] = Session.CurrentSession.Connection.AddParameter("updated", updated);

                        if (String.IsNullOrEmpty(status))
                            pars[11] = Session.CurrentSession.Connection.AddParameter("status", DBNull.Value);
                        else
                            pars[11] = Session.CurrentSession.Connection.AddParameter("status", status);

                        pars[12] = Session.CurrentSession.Connection.AddParameter("deleted", (deleted || accepted == false));

                        if (scriptVersionID != null)
                            pars[13] = Session.CurrentSession.Connection.AddParameter("verScriptVersionID", scriptVersionID);
                        else
                            pars[13] = Session.CurrentSession.Connection.AddParameter("verScriptVersionID", DBNull.Value);

                        Session.CurrentSession.Connection.ExecuteSQL(sql, pars);
                    }

                    if (deleted)
                    {
                        AddActivity("DELETED", null);
                    }

                    Session.CurrentSession.Connection.CommitTransaction();
                    isnew = false;
                    isdirty = false;
                }
                catch
                {
                    Session.CurrentSession.Connection.RollbackTransaction();
                    throw;
                }
                finally
                {
                    Session.CurrentSession.Connection.Disconnect();
                }
            }
        }

        private DateTime lastActivityCheck = DateTime.MinValue.ToLocalTime();
        private DataTable activities = null;


        public void AddActivity(string action, string subaction)
        {
            AddActivity(action, subaction, null);
        }
        public void AddActivity(string action, string subaction, string data)
        {
            if (String.IsNullOrEmpty(action))
                throw new ArgumentException("action");

            if (ParentDocument.IsNew == false)
            {
                List<IDataParameter> pars = new List<IDataParameter>();
                pars.Add(Session.CurrentSession.Connection.AddParameter("precid", ParentDocument.ID));
                pars.Add(Session.CurrentSession.Connection.AddParameter("version", Id));
                pars.Add(Session.CurrentSession.Connection.AddParameter("action", SqlDbType.NVarChar, 15, action));
                if (String.IsNullOrEmpty(subaction))
                    pars.Add(Session.CurrentSession.Connection.AddParameter("subaction", SqlDbType.NVarChar, 15, DBNull.Value));
                else
                    pars.Add(Session.CurrentSession.Connection.AddParameter("subaction", SqlDbType.NVarChar, 15, subaction));

                pars.Add(Session.CurrentSession.Connection.AddParameter("usrid", Session.CurrentSession.CurrentUser.ID));

                if (String.IsNullOrEmpty(data))
                    pars.Add(Session.CurrentSession.Connection.AddParameter("data", DBNull.Value));
                else
                    pars.Add(Session.CurrentSession.Connection.AddParameter("data", SqlDbType.NVarChar, 100, data));

                if (Session.CurrentSession.IsProcedureInstalled("sprCreatePrecedentAction"))
                    activities = Session.CurrentSession.Connection.ExecuteProcedureTable("sprCreatePrecedentAction", "ACTIVITIES", pars.ToArray());
                else
                    activities = Session.CurrentSession.Connection.ExecuteSQLTable(
    @"insert into dbprecedentlog(precid, verid, logtype, logcode, usrid) values(@precid, @version, @action, @subaction, @usrid);
select * from dbprecedentlog where precid = @precid and verid = @version", "ACTIVITIES", pars.ToArray());

                lastActivityCheck = DateTime.Now;
            }
        }

        System.Data.DataTable IStorageItem.GetActivities()
        {
            if (activities == null || DateTime.Now.Subtract(lastActivityCheck).Minutes > 0)
            {
                IDataParameter[] pars = new IDataParameter[2];
                pars[0] = Session.CurrentSession.Connection.AddParameter("precid", ParentDocument.ID);
                pars[1] = Session.CurrentSession.Connection.AddParameter("verid", Id);

                activities = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbprecedentlog where precid = @precid and verid = @verid", "ACTIVITIES", pars);

                lastActivityCheck = DateTime.Now;
            }

            return activities.Copy();
        }

        #endregion

        #region IStorageItemVersionable Members

        public event EventHandler<NewVersionEventArgs> NewVersion;
        
        public void OnNewVersion(NewVersionEventArgs e)
        {
            if (NewVersion != null)
                NewVersion(this, e);
            BaseVersionalStorageItem.OnNewVersion(e);
        }

        public IStorageItemVersion[] GetVersions()
        {
            System.Data.DataTable dt = parent.GetVersionsTable(false);
            List<PrecedentVersion> versions = new List<PrecedentVersion>();
            List<Guid> visited = new List<Guid>();
            GetVersions(dt, visited, this, versions);
            return versions.ToArray();
        }

        private void GetVersions(DataTable versionsData, List<Guid> visited, PrecedentVersion parent, List<PrecedentVersion> versions)
        {
            using (DataView vw = new DataView(versionsData))
            {
                vw.RowFilter = String.Format("verparent='{0}'", parent.Id);
                vw.Sort = "verdepth asc, vernumber asc";
                foreach (DataRowView r in vw)
                {
                    var version = new PrecedentVersion(ParentDocument, r.Row);
                    if (visited.Contains(version.Id))
                        continue;
                    versions.Add(version);
                    visited.Add(version.Id);
                    GetVersions(versionsData, visited, version, versions);
                }
            }
        }

        public IStorageItemVersion GetVersion(Guid id)
        {
            return BaseVersionalStorageItem.GetVersion(id);
        }

        public IStorageItemVersion GetVersion(string label)
        {
            return BaseVersionalStorageItem.GetVersion(label);
        }

        public void DeleteVersion(IStorageItemVersion version)
        {
            if (version == null)
                throw new ArgumentNullException("version");
            if (version.BaseStorageItem.Pointer != this.BaseStorageItem.Pointer)
                throw new ArgumentException(Session.CurrentSession.Resources.GetMessage("MSGPRPRCMTHSM", "The parent precedent must be the same as the current precedent", "").Text);

            BaseVersionalStorageItem.DeleteVersion(version);
        }

        public IStorageItemVersion CreateVersion()
        {
            return ((IStorageItemVersionable)this.ParentDocument).CreateVersion();
        }

        public IStorageItemVersion CreateVersion(IStorageItemVersion original)
        {
            return ((IStorageItemVersionable)this.ParentDocument).CreateVersion(original);
        }

        public IStorageItemVersion CreateSubVersion(IStorageItemVersion original)
        {
            return ((IStorageItemVersionable)this.ParentDocument).CreateSubVersion(original);
        }

        public IStorageItemVersion GetLatestVersion()
        {
            DocumentManagement.Storage.IStorageItemVersion[] versions = GetVersions();
            if (versions.Length == 0)
            {
                return this;
            }
            else
                return versions[versions.Length - 1];
        }

        public void SetLatestVersion(IStorageItemVersion current)
        {
            if (current == null)
                throw new ArgumentNullException("current");
            if (current.BaseStorageItem.Pointer != this.BaseStorageItem.Pointer)
                throw new ArgumentException(Session.CurrentSession.Resources.GetMessage("MSGPRPRCMTHSM", "The parent precedent must be the same as the current precedent", "").Text);
            if (Session.CurrentSession.EnablePrecedentVersioning)
                BaseVersionalStorageItem.SetLatestVersion(current);
        }

        void IStorageItemVersionable.SetWorkingVersion(IStorageItemVersion version)
        {
            if (version == null)
                throw new ArgumentNullException("version");
            if (version.BaseStorageItem.Pointer != BaseStorageItem.Pointer)
                throw new ArgumentException(Session.CurrentSession.Resources.GetMessage("MSGPRPRCMTHSMWP", "The parent precedent must be the same as the current working precedent", "").Text);
            if (Session.CurrentSession.EnablePrecedentVersioning)
                BaseVersionalStorageItem.SetWorkingVersion(version);
        }

        IStorageItemVersion IStorageItemVersionable.GetWorkingVersion()
        {
            return BaseVersionalStorageItem.GetWorkingVersion();
        }

        #endregion

        #region IStorageItemDuplication Members

        public IStorageItemDuplication CheckForDuplicate()
        {
            return BaseDuplicationStorageItem.CheckForDuplicate();
        }

        public bool AllowDuplication
        {
            get
            {
                return BaseDuplicationStorageItem.AllowDuplication;
            }
            set
            {
                BaseDuplicationStorageItem.AllowDuplication = value;
            }
        }

        #endregion

        #region IStorageItemLockable Members

        public string CheckedOutMachine
        {
            get
            {
                return BaseLockableStorageItem.CheckedOutMachine;
            }
        }

        public string CheckedOutLocation
        {
            get
            {
                return BaseLockableStorageItem.CheckedOutLocation;
            }
        }

        public DateTime? CheckedOutTime
        {
            get { return BaseLockableStorageItem.CheckedOutTime; }
        }

        public int CheckOutDuration
        {
            get { return BaseLockableStorageItem.CheckOutDuration; }
        }

        public bool IsCheckedOut
        {
            get { return BaseLockableStorageItem.IsCheckedOut; }
        }


        public bool IsCheckedOutByAnother
        {
            get { return BaseLockableStorageItem.IsCheckedOutByAnother; }
        }

        public bool IsCheckedOutByCurrentUser
        {
            get { return BaseLockableStorageItem.IsCheckedOutByCurrentUser; }
        }

        public bool CanCheckIn
        {
            get { return BaseLockableStorageItem.CanCheckIn; }
        }

        public bool CanCheckOut
        {
            get { return BaseLockableStorageItem.CanCheckOut; }
        }


        public bool CanUndo
        {
            get { return BaseLockableStorageItem.CanUndo; }
        }

        public User CheckedOutBy
        {
            get { return BaseLockableStorageItem.CheckedOutBy; }
        }

        public void CheckOut(System.IO.FileInfo localFile)
        {
            BaseLockableStorageItem.CheckOut(localFile);
        }

        public void CheckIn()
        {
            BaseLockableStorageItem.CheckIn();
        }

        public void UndoCheckOut()
        {
            BaseLockableStorageItem.UndoCheckOut();
        }

        public void UpdateCheckedOutLocation(FileInfo localFile)
        {
            BaseLockableStorageItem.UpdateCheckedOutLocation(localFile);
        }
        #endregion

        #region IExtraInfo Members

        public void SetExtraInfo(string fieldName, object val)
        {
            throw new NotSupportedException();
        }

        public object GetExtraInfo(string fieldName)
        {
            switch (fieldName.ToUpperInvariant())
            {
                case "VERID":
                    return Id;
                case "VERNUMBER":
                    return Version;
                case "VERLABEL":
                    return Label;
                case "VERTOKEN":
                    return Token;
                case "VERCOMMENTS":
                    return Comments;
            }
            
            throw new ArgumentException(Session.CurrentSession.Resources.GetMessage("CLMDNTTOTBLVRS", "Column ''%1%'' does not belong to table VERSIONS.", "", fieldName).Text);
        }

        public Type GetExtraInfoType(string fieldName)
        {
            try
            {
                switch (fieldName.ToUpperInvariant())
                {
                    case "VERID":
                        return typeof(Guid);
                    case "VERNUMBER":
                        return typeof(int);
                    case "VERLABEL":
                        return typeof(String);
                    case "VERTOKEN":
                        return typeof(String);
                    case "VERCOMMENTS":
                        return typeof(String);

                }
                throw new Exception(Session.CurrentSession.Resources.GetMessage("CPTCLT", "Error Getting Extra Info Field %1% Probably Not Initialized", "", fieldName).Text);
            }
            catch
            {
                throw new Exception(Session.CurrentSession.Resources.GetMessage("CPTCLT", "Error Getting Extra Info Field %1% Probably Not Initialized", "", fieldName).Text);
            }
        }

        public DataSet GetDataset()
        {
            throw new NotSupportedException();
        }

        public DataTable GetDataTable()
        {
            throw new NotSupportedException();
        }

        #endregion
        
    }
}
