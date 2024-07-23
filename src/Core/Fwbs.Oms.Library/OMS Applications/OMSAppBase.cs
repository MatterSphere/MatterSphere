using System;

namespace FWBS.OMS.Interfaces
{
    using System.Globalization;
    using DocumentManagement;
    using DocumentManagement.Storage;
    using Document = OMSDocument;


    public abstract class IOMSApp
    {
        #region Fields

        /// <summary>
        /// A collection of refreshed fields.
        /// </summary>
        private Common.KeyValueCollection _refreshedfields = new Common.KeyValueCollection();

        /// <summary>
        /// A flag indicating that the refresh method has been executed.
        /// </summary>
        private bool _refreshing = false;


        #endregion

        #region Document Property Constants

        public const string EDITION = "EDITION";
        public const string CLIENT = "CLIENTID";
        public const string FILE = "FILEID";
        public const string ASSOCIATE = "ASSOCID";
        public const string PHASE = "PHASEID";
        public const string DOCUMENT = "DOCID";
        public const string DOCUMENT_EXTERNAL = "DOCIDEX";
        public const string ISPREC = "ISPREC";
        public const string TEXTONLY = "TEXTONLY";
        public const string COMPANY = "COMPANYID";
        public const string DATAKEY = "DATAKEY";
        public const string CUSTOMFORM = "CUSTOMFORM";
        public const string VIEWMODE = "VIEWMODE";
        public const string BRANCH = "SERIALNO";
        public const string BASEPRECEDENT = "BASEPRECID";
        public const string PRECEDENT = "PRECID";
        public const string TYPE = "BASEPRECTYPE";
        public const string PRIVATE = "ISPRIVATE";
        public const string RETENTION_POLICY = "RETENTIONPOLICY";
        public const string RETENTION_PERIOD = "RETENTIONPERIOD";
        public const string BILLNO = "BILLNO";
        public const string DICID = "DICID";
        public const string DOCUMENT_VERSION = "VERSIONID";
        public const string DOCUMENT_LABEL = "VERSIONLABEL";
        public const string AUTHORISE_DOCUMENT = "AUTHORISEDOCID";
        public const string AUTHORISE_DOCUMENT_VERSION = "AUTHORISEVERSIONID";
        public const string AUTHORISE_RETURN = "AUTHORISERETURN";
        public const string PROPTAG_CUSTOM_PROPERTY = "{00020329-0000-0000-C000-000000000046}";


        private const string MATTER = "MATTERID";
        private const string DOCREF = "DOCREF";

        public const string PROCESSING = "PROCESSING";

        public const string TASKID = "TASKID";
        public const string APPID = "APPID";

        public const string SEP = "_#_";
        public const string RELINK = "RELINK";

        //Precedent Version
        public const string PRECEDENT_VERSION = "VERSIONID";
        public const string PRECEDENT_LABEL = "VERSIONLABEL";

        #endregion

        #region Events

        public event EventHandler JobProcessed = null;
        public event FWBS.OMS.Interfaces.OMSAppSavedEventHandler Saved = null;
        public event EventHandler DocumentClosed;


        #endregion

        #region Event Methods

        protected virtual void OnJobProcessed()
        {
            try
            {
                if (JobProcessed != null)
                {
                    JobProcessed(this, EventArgs.Empty);
                }
            }
            catch { }
        }

        protected virtual void OnSaved(FWBS.OMS.Interfaces.OMSAppSavedEventArgs e)
        {
            if (Saved != null)
            {
                Saved(this, e);
            }
        }

        protected virtual void OnDocumentClosed()
        {

            EventHandler ev = DocumentClosed;
            if (ev != null)
                ev(this, EventArgs.Empty);

        }

        #endregion

        #region Document Variable Methods

        /// <summary>
        /// Used to save the document to local save - in OMSApp calls internal Save
        /// </summary>
        /// <param name="obj"></param>
        public abstract void LocalSave(object obj);


        public abstract void RemoveDocVariable(object obj, string varName);

        protected void DisplayDocVariables(object obj)
        {
            CheckObjectIsDoc(ref obj);
            System.Text.StringBuilder val = new System.Text.StringBuilder();

            for (int ctr = 0; ctr < GetFieldCount(obj); ctr++)
            {
                val.Append(GetFieldName(obj, ctr));
                val.Append(": ");
                val.Append(Convert.ToString(GetFieldValue(obj, ctr)));
                val.Append(Environment.NewLine);
            }

            DisplayDocVariables(obj, val.ToString());
        }


        protected abstract void DisplayDocVariables(object obj, string val);

        public string GetDocVariable(object obj, string varName, string defVal)
        {
            try
            {
                object ret = GetDocVariable(obj, varName);
                if (ret == null)
                    return defVal;
                else
                    return Convert.ToString(ret);
            }
            catch
            {
                return defVal;
            }
        }




        public long GetDocVariable(object obj, string varName, long defVal)
        {
            try
            {
                object ret = GetDocVariable(obj, varName);
                if (ret == null)
                    return defVal;
                else
                    return Convert.ToInt64(ret);
            }
            catch
            {
                return defVal;
            }
        }

        public bool GetDocVariable(object obj, string varName, bool defVal)
        {
            object ret = null;
            try
            {
                ret = GetDocVariable(obj, varName);
                if (ret == null)
                    return defVal;
                else
                    return Convert.ToBoolean(ret);
            }
            catch
            {
                if (Convert.ToString(ret) == "-1")
                    return true;

                return defVal;
            }
        }

        public bool SetDocVariable(object obj, string varName, string val)
        {
            return SetDocVariable(obj, varName, (object)val);
        }

        public bool SetDocVariable(object obj, string varName, long val)
        {
            return SetDocVariable(obj, varName, (object)val);
        }

        public bool SetDocVariable(object obj, string varName, bool val)
        {
            return SetDocVariable(obj, varName, (object)val);
        }


        protected abstract object GetDocVariable(object obj, string varName);


        public abstract bool HasDocVariable(object obj, string varName);

        protected abstract bool SetDocVariable(object obj, string varName, object val);

        



        protected void AttachPrecedentVars(object obj, Precedent prec)
        {

            if (GetDocVariable(obj, COMPANY, -1) != Session.CurrentSession.CompanyID)
            {
                SetDocVariable(obj, COMPANY, Session.CurrentSession.CompanyID);
            }
            if (GetDocVariable(obj, DATAKEY, String.Empty) != Session.CurrentSession.DataKey)
            {
                SetDocVariable(obj, DATAKEY, Session.CurrentSession.DataKey);
            }
            if (GetDocVariable(obj, BRANCH, -1) != Session.CurrentSession.SerialNumber)
            {
                SetDocVariable(obj, BRANCH, Session.CurrentSession.SerialNumber);
            }


            if (IsPrecedent(obj) == false)
            {
                SetDocVariable(obj, ISPREC, true);
            }

            if (GetDocVariable(obj, PRECEDENT, -1) != prec.ID)
            {
                SetDocVariable(obj, PRECEDENT, prec.ID);
            }

            if (HasDocVariable(obj, TEXTONLY) == false || GetDocVariable(obj, TEXTONLY, false) != prec.TextOnly)
            {
                SetDocVariable(obj, TEXTONLY, prec.TextOnly);
            }

        }

        #region "PrecedentVersion variable methods"

        protected void AttachPrecedentVersionVars(object obj, Precedent prec, PrecedentVersion version)
        {
            //If a particular version is passed then set the document variable.
            if (version == null)
            {
                if (HasDocVariable(obj, PRECEDENT_VERSION)) RemoveDocVariable(obj, PRECEDENT_VERSION);
                if (HasDocVariable(obj, PRECEDENT_LABEL)) RemoveDocVariable(obj, PRECEDENT_LABEL);
            }
            else
            {
                Guid stored;
                try
                {
                    stored = new Guid(GetDocVariable(obj, PRECEDENT_VERSION, Guid.Empty.ToString()));
                }
                catch (FormatException)
                {
                    stored = Guid.Empty;
                }
                if (version.Id.Equals(stored) == false)
                    SetDocVariable(obj, PRECEDENT_VERSION, version.Id.ToString());

                if (version.Label.Equals(GetDocVariable(obj, PRECEDENT_LABEL, String.Empty)) == false)
                    SetDocVariable(obj, PRECEDENT_LABEL, version.Label);
            }          
        }

        protected void DetachPrecedentVersionVars(object obj)
        {
            RemoveDocVariable(obj, PRECEDENT_VERSION);
            RemoveDocVariable(obj, PRECEDENT_LABEL);
        }

        #endregion

        protected void DettachPrecedentVars(object obj)
        {
            RemoveDocVariable(obj, COMPANY);
            RemoveDocVariable(obj, DATAKEY);
            RemoveDocVariable(obj, BRANCH);
            RemoveDocVariable(obj, EDITION);
            RemoveDocVariable(obj, ISPREC);
            RemoveDocVariable(obj, PRECEDENT);
            RemoveDocVariable(obj, TEXTONLY);
        }

        public void DettachDocumentVars(object obj)
        {
            RemoveDocVariable(obj, COMPANY);
            RemoveDocVariable(obj, DATAKEY);
            RemoveDocVariable(obj, VIEWMODE);
            RemoveDocVariable(obj, CUSTOMFORM);
            RemoveDocVariable(obj, BRANCH);
            RemoveDocVariable(obj, EDITION);
            RemoveDocVariable(obj, CLIENT);
            RemoveDocVariable(obj, FILE);
            RemoveDocVariable(obj, ASSOCIATE);
            RemoveDocVariable(obj, PHASE);
            RemoveDocVariable(obj, DOCUMENT);
            RemoveDocVariable(obj, DOCUMENT_EXTERNAL);
            RemoveDocVariable(obj, DOCUMENT_VERSION);
            RemoveDocVariable(obj, DOCUMENT_LABEL);
            RemoveDocVariable(obj, PRECEDENT);
            RemoveDocVariable(obj, BASEPRECEDENT);
            RemoveDocVariable(obj, TYPE);
            RemoveDocVariable(obj, RETENTION_POLICY);
            RemoveDocVariable(obj, RETENTION_PERIOD);

            DettachCustomDocumentVars(obj);
        }

        protected virtual void DettachCustomDocumentVars(object obj)
        {
        }

        protected Common.KeyValueCollection CopyVariables(object obj)
        {
            Common.KeyValueCollection col = new Common.KeyValueCollection();
            foreach (string val in FieldExclusionList)
            {
                if (HasDocVariable(obj, val))
                    col.Add(val, GetDocVariable(obj, val));
            }
            return col;
        }

        protected void ApplyVariables(object obj, Common.KeyValueCollection col)
        {
            for (int ctr = 0; ctr < col.Count; ctr++)
            {
                Common.KeyValueItem val = col[ctr];
                if (val != null)
                    SetDocVariable(obj, val.Key, val.Value);
            }
        }

        public Associate AttachDocumentVars(object obj, bool useDefaultAssociate, Associate assoc)
        {
            assoc = SelectAssociate(obj, useDefaultAssociate, assoc);

            //DM - 21/09/04 - I added this as an object ref error was being thrown if the associate was cancelled.
            if (assoc == null)
                return null;

            if (assoc == Associate.Private)
            {
                SetDocVariable(obj, PRIVATE, true);
                return assoc;
            }

            //DM - 13/05/04 - Make sure the document variable is there.
            if (HasDocVariable(obj, DOCUMENT) == false)
                SetDocVariable(obj, DOCUMENT, "");

            if (HasDocVariable(obj, PRIVATE)) RemoveDocVariable(obj, PRIVATE);
            if (FWBS.OMS.Session.CurrentSession.CompanyID != GetDocVariable(obj, COMPANY, -1)) SetDocVariable(obj, COMPANY, FWBS.OMS.Session.CurrentSession.CompanyID);
            if (FWBS.OMS.Session.CurrentSession.DataKey != GetDocVariable(obj, DATAKEY, String.Empty)) SetDocVariable(obj, DATAKEY, FWBS.OMS.Session.CurrentSession.DataKey);
            if (FWBS.OMS.Session.CurrentSession.SerialNumber != GetDocVariable(obj, BRANCH, -1)) SetDocVariable(obj, BRANCH, FWBS.OMS.Session.CurrentSession.SerialNumber);
            if (FWBS.OMS.Session.CurrentSession.Edition != GetDocVariable(obj, EDITION, "")) SetDocVariable(obj, EDITION, FWBS.OMS.Session.CurrentSession.Edition);
            if (assoc.OMSFile.Client.ClientID != GetDocVariable(obj, CLIENT, -1)) SetDocVariable(obj, CLIENT, assoc.OMSFile.Client.ClientID);
            if (assoc.OMSFile.ID != GetDocVariable(obj, FILE, -1)) SetDocVariable(obj, FILE, assoc.OMSFile.ID);
            if (assoc.ID != GetDocVariable(obj, ASSOCIATE, -1)) SetDocVariable(obj, ASSOCIATE, assoc.ID);

            return assoc;

        }

        public void AttachDocumentVars(object obj, Document doc, DocumentVersion version)
        {
            Associate assoc = doc.Associate;


            if (doc.ID != GetDocVariable(obj, DOCUMENT, -1))
                SetDocVariable(obj, DOCUMENT, doc.ID);

            if (doc.ID == GetDocVariable(obj, DOCUMENT, -1))
            {
                if (HasDocVariable(obj, PRIVATE)) RemoveDocVariable(obj, PRIVATE);
                if (FWBS.OMS.Session.CurrentSession.CompanyID != GetDocVariable(obj, COMPANY, -1)) SetDocVariable(obj, COMPANY, FWBS.OMS.Session.CurrentSession.CompanyID);
                if (FWBS.OMS.Session.CurrentSession.DataKey != GetDocVariable(obj, DATAKEY, String.Empty)) SetDocVariable(obj, DATAKEY, FWBS.OMS.Session.CurrentSession.DataKey);
                if (FWBS.OMS.Session.CurrentSession.SerialNumber != GetDocVariable(obj, BRANCH, -1)) SetDocVariable(obj, BRANCH, FWBS.OMS.Session.CurrentSession.SerialNumber);
                if (FWBS.OMS.Session.CurrentSession.Edition != GetDocVariable(obj, EDITION, "")) SetDocVariable(obj, EDITION, FWBS.OMS.Session.CurrentSession.Edition);
                if (assoc.OMSFile.Client.ClientID != GetDocVariable(obj, CLIENT, -1)) SetDocVariable(obj, CLIENT, assoc.OMSFile.Client.ClientID);
                if (assoc.OMSFile.ID != GetDocVariable(obj, FILE, -1)) SetDocVariable(obj, FILE, assoc.OMSFile.ID);
                if (assoc.ID != GetDocVariable(obj, ASSOCIATE, -1)) SetDocVariable(obj, ASSOCIATE, assoc.ID);
                if (doc.ExternalId != GetDocVariable(obj, DOCUMENT_EXTERNAL, "")) SetDocVariable(obj, DOCUMENT_EXTERNAL, doc.ExternalId);
                if (doc.DocumentType != GetDocVariable(obj, TYPE, "")) SetDocVariable(obj, TYPE, doc.DocumentType);

                //Set the phase id to the document (if applicable),
                object phaseid = null;
                try { phaseid = doc.GetExtraInfo("phID"); }
                catch { };
                if (phaseid == null || phaseid == DBNull.Value)
                {
                    if (HasDocVariable(obj, PHASE)) RemoveDocVariable(obj, PHASE);
                }
                else
                    if (phaseid.Equals(GetDocVariable(obj, PHASE, -1)) == false) SetDocVariable(obj, PHASE, phaseid);


                Precedent baseprec = doc.BasePrecedent;
                if (baseprec == null)
                {
                    if (HasDocVariable(obj, BASEPRECEDENT)) RemoveDocVariable(obj, BASEPRECEDENT);
                }
                else
                    if (baseprec.ID != GetDocVariable(obj, BASEPRECEDENT, -1)) SetDocVariable(obj, BASEPRECEDENT, baseprec.ID);

                Precedent lastprec = doc.LastPrecedent;
                if (lastprec == null)
                {
                    if (HasDocVariable(obj, PRECEDENT)) RemoveDocVariable(obj, PRECEDENT);
                }
                else
                    if (lastprec.ID != GetDocVariable(obj, PRECEDENT, -1)) SetDocVariable(obj, PRECEDENT, lastprec.ID);

                //If a particular version is passed then set the document variable.
                if (version == null)
                {
                    if (HasDocVariable(obj, DOCUMENT_VERSION)) RemoveDocVariable(obj, DOCUMENT_VERSION);
                    if (HasDocVariable(obj, DOCUMENT_LABEL)) RemoveDocVariable(obj, DOCUMENT_LABEL);
                }
                else
                {
                    Guid stored;
                    try
                    {
                        stored = new Guid(GetDocVariable(obj, DOCUMENT_VERSION, Guid.Empty.ToString()));
                    }
                    catch (FormatException)
                    {
                        stored = Guid.Empty;
                    }
                    if (version.Id.Equals(stored) == false)
                        SetDocVariable(obj, DOCUMENT_VERSION, version.Id.ToString());

                    if (version.Label.Equals(GetDocVariable(obj, DOCUMENT_LABEL, String.Empty)) == false)
                        SetDocVariable(obj, DOCUMENT_LABEL, version.Label);
                }

                AttachCustomDocumentVars(obj, doc, version);
            }


        }

        protected virtual void AttachCustomDocumentVars(object obj, Document doc, DocumentVersion version)
        {
        }

        protected virtual bool SupportsOleProperties(object obj)
        {
            return false;
        }


        protected void AttachOutboundDocumentVars(object obj, IStorageItem item)
        {
            DocumentVersion version = item as DocumentVersion;
            Document doc = item as Document;
            if (version != null)
                doc = version.ParentDocument;

            OutboundPropertyNames obprops = new OutboundPropertyNames();


            long docid;
            if (doc == null)
                docid = Convert.ToInt64(GetDocVariable(obj, DOCUMENT, -1));
            else
                docid = doc.ID;

            if (docid != GetDocVariable(obj, obprops.DOCID_COMPANY, -1))
                SetDocVariable(obj, obprops.DOCID_COMPANY, docid);

            if (docid != GetDocVariable(obj, obprops.DOCID_DATAKEY, -1))
                SetDocVariable(obj, obprops.DOCID_DATAKEY, docid);

            if (docid != GetDocVariable(obj, obprops.DOCID_SERIAL, -1))
                SetDocVariable(obj, obprops.DOCID_SERIAL, docid);


            Guid versionid;
            if (version == null)
            {
                try
                {
                    versionid = new Guid(GetDocVariable(obj, DOCUMENT_VERSION, Guid.Empty.ToString()));
                }
                catch (FormatException)
                {
                    versionid = Guid.Empty;
                }
            }
            else
                versionid = version.Id;

            if (versionid != Guid.Empty)
            {
                Guid stored;

                try
                {
                    stored = new Guid(GetDocVariable(obj, obprops.VERID_COMPANY, Guid.Empty.ToString()));
                }
                catch (FormatException)
                {
                    stored = Guid.Empty;
                }
                if (versionid.Equals(stored) == false)
                    SetDocVariable(obj, obprops.VERID_COMPANY, versionid.ToString());
                try
                {
                    stored = new Guid(GetDocVariable(obj, obprops.VERID_DATAKEY, Guid.Empty.ToString()));
                }
                catch (FormatException)
                {
                    stored = Guid.Empty;
                }
                if (versionid.Equals(stored) == false)
                    SetDocVariable(obj, obprops.VERID_DATAKEY, versionid.ToString());
            }

        }

        protected void DetachOutboundDocumentVars(object obj)
        {

            OutboundPropertyNames obprops = new OutboundPropertyNames();

            RemoveDocVariable(obj, obprops.DOCID_COMPANY);
            RemoveDocVariable(obj, obprops.DOCID_DATAKEY);
            RemoveDocVariable(obj, obprops.DOCID_SERIAL);
            RemoveDocVariable(obj, obprops.VERID_COMPANY);
            RemoveDocVariable(obj, obprops.VERID_DATAKEY);
        }


        public static void AttachOutboundDocumentVars(System.IO.FileInfo file, IStorageItem item)
        {
            DocumentVersion version = item as DocumentVersion;
            Document doc = item as Document;
            if (version != null)
                doc = version.ParentDocument;

            OutboundPropertyNames obprops = new OutboundPropertyNames();

            using (System.IO.OleFileInfo ole = new System.IO.OleFileInfo(file.FullName))
            {
                long docid;
                if (doc == null)
                    docid = Convert.ToInt64(ole.GetProperty(DOCUMENT));
                else
                    docid = doc.ID;


                ole.SetProperty(obprops.DOCID_COMPANY, docid);
                ole.SetProperty(obprops.DOCID_DATAKEY, docid);
                ole.SetProperty(obprops.DOCID_SERIAL, docid);


                Guid versionid;
                if (version == null)
                {
                    try
                    {
                        versionid = new Guid(Convert.ToString(ole.GetProperty(DOCUMENT_VERSION)));
                    }
                    catch
                    {
                        versionid = Guid.Empty;
                    }
                }
                else
                    versionid = version.Id;

                if (versionid != Guid.Empty)
                {
                    ole.SetProperty(obprops.VERID_COMPANY, versionid.ToString());
                    ole.SetProperty(obprops.VERID_DATAKEY, versionid.ToString());
                }

                ole.Save();
            }
        }

        public static void DetachOutboundDocumentVars(System.IO.FileInfo file)
        {
            OutboundPropertyNames obprops = new OutboundPropertyNames();

            using (System.IO.OleFileInfo ole = new System.IO.OleFileInfo(file.FullName))
            {
                ole.RemoveProperty(obprops.DOCID_COMPANY);
                ole.RemoveProperty(obprops.DOCID_DATAKEY);
                ole.RemoveProperty(obprops.DOCID_SERIAL);
                ole.RemoveProperty(obprops.VERID_COMPANY);
                ole.RemoveProperty(obprops.VERID_DATAKEY);

                ole.Save();
            }
        }

        /// <summary>
        /// Used to look at the current controlling application to try and 
        /// get the current Associate relating to the object on screen, if word
        /// the activedocument will be checked if there is a file relevant to filter
        /// the precedents down.
        /// </summary>
        /// <returns>Object Created</returns>
        public virtual Associate GetCurrentAssociate(object obj)
        {
            CheckObjectIsDoc(ref obj);

            //Check if document variable exists for Associate 
            //If so send AssocObject through the channels
            if (HasDocVariable(obj, ASSOCIATE))
            {
                long associd = GetDocVariable(obj, ASSOCIATE, 0);
                if (associd != 0)
                {
                    try
                    {
                        return FWBS.OMS.Associate.GetAssociate(associd);
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    return null;

            }
            else
                return null;
        }

        public Precedent GetCurrentPrecedent(object obj)
        {
            CheckObjectIsDoc(ref obj);

            if (IsCompanyDocument(obj))
            {
                if (HasDocVariable(obj, PRECEDENT))
                {
                    return FWBS.OMS.Precedent.GetPrecedent(GetDocVariable(obj, PRECEDENT, 0));
                }
            }

            return null;

        }
        /// <summary>
        /// Used to look at the current controlling application to try and 
        /// get the current Document object relating to the object on screen.
        /// </summary>
        /// <returns>Object Created</returns>
        public Document GetCurrentDocument(object obj)
        {
            CheckObjectIsDoc(ref obj);

            //Check if document variable exists for Associate 
            //If so send AssocObject through the channels
            if (IsCompanyDocument(obj))
            {
                if (HasDocVariable(obj, DOCUMENT))
                {
                    long docid = GetDocVariable(obj, DOCUMENT, 0);
                    if (docid != 0)
                    {
                        try
                        {
                            return Document.GetDocument(docid);
                        }
                        catch (Security.SecurityException)
                        {
                            throw;
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Used to look at the current controlling application to try and 
        /// get the current Document version relating to the object on screen.
        /// </summary>
        public DocumentVersion GetCurrentDocumentVersion(object obj)
        {
            CheckObjectIsDoc(ref obj);

            IStorageItemVersionable doc = GetCurrentDocument(obj) as IStorageItemVersionable;
            if (doc != null)
            {
                if (HasDocVariable(obj, DOCUMENT_VERSION))
                {
                    Guid id;
                    try
                    {
                        id = new Guid(GetDocVariable(obj, DOCUMENT_VERSION, Guid.Empty.ToString()));
                    }
                    catch (FormatException)
                    {
                        id = Guid.Empty;
                    }

                    if (id != Guid.Empty)
                    {
                        try
                        {
                            return doc.GetVersion(id) as DocumentVersion;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// Used to look at the current controlling application to try and 
        /// get the current Document version relating to the object on screen.
        /// </summary>
        public PrecedentVersion GetCurrentPrecedentVersion(object obj)
        {
            CheckObjectIsDoc(ref obj);

            IStorageItemVersionable doc = GetCurrentPrecedent(obj) as IStorageItemVersionable;
            if (doc != null)
            {
                if (HasDocVariable(obj, PRECEDENT_VERSION))
                {
                    Guid id;
                    try
                    {
                        id = new Guid(GetDocVariable(obj, PRECEDENT_VERSION, Guid.Empty.ToString()));
                    }
                    catch (FormatException)
                    {
                        id = Guid.Empty;
                    }

                    if (id != Guid.Empty)
                    {
                        try
                        {
                            return doc.GetVersion(id) as PrecedentVersion;
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return null;
        }

        public bool IsCompanyDocument(object obj)
        {
            string datakey = GetDocVariable(obj, DATAKEY, "");

            //Take into account the new DataKey (if available)
            if (GetDocVariable(obj, COMPANY, 0) == Session.CurrentSession.CompanyID
                && (String.IsNullOrEmpty(datakey)
                || datakey.ToUpperInvariant().Equals(Session.CurrentSession.DataKey.ToUpperInvariant())))

                return true;
            else
                return false;
        }

        public bool IsStoredDocument(object obj)
        {
            return IsCompanyDocument(obj) && GetDocVariable(obj, DOCUMENT, 0) != 0;
        }

        protected bool IsBranchDocument(object obj)
        {

            if (GetDocVariable(obj, BRANCH, 0) == Session.CurrentSession.SerialNumber)
                return true;
            else
                return false;
        }

        protected bool IsProcessing(object obj)
        {
            return GetDocVariable(obj, PROCESSING, false);
        }

        public bool IsPrecedent(object obj)
        {
            return GetDocVariable(obj, ISPREC, false);
        }

        /// <summary>
        /// Get the Applications Active Document Type, IE LETTERHEAD
        /// </summary>
        /// <param name="obj">Object Created</param>
        /// <returns>String Representing the PRECTYPE</returns>
        public virtual string GetActiveDocType(object obj)
        {
            CheckObjectIsDoc(ref obj);
            if (HasDocVariable(obj, TYPE))
                return GetDocVariable(obj, TYPE, "");
            else
                return "";
        }

        public virtual int GetDocumentCount()
        {
            return 1;
        }

        #endregion

        #region Field Routines

        public virtual string[] FieldExclusionList
        {
            get
            {
                OutboundPropertyNames obprops = new OutboundPropertyNames();

                return new string[]
                {

                    EDITION,
                    COMPANY,
                    DATAKEY,
                    BRANCH,

                    CLIENT,
                    FILE,
                    MATTER,
                    ASSOCIATE,
                    PHASE,

                    TYPE,
                    ISPREC,
                    BASEPRECEDENT,
                    PRECEDENT,
                    TEXTONLY,

                    DOCUMENT,
                    DOCUMENT_EXTERNAL,
                    DOCUMENT_VERSION,
                    DOCUMENT_LABEL,
                    DOCREF,

                    PROCESSING,
                    PRIVATE,

                    RETENTION_POLICY,
                    RETENTION_PERIOD,
                    BILLNO,
                    DICID,
                    VIEWMODE,
                    CUSTOMFORM,

                    obprops.DOCID_COMPANY,
                    obprops.DOCID_DATAKEY,
                    obprops.DOCID_SERIAL,
                    obprops.VERID_COMPANY,
                    obprops.VERID_DATAKEY,

                    PRECEDENT_VERSION,
                    PRECEDENT_LABEL
                };

                
            }
        }

        protected virtual bool CanRefreshDocumentFields(object obj)
        {
            return !HasDocVariable(obj, "DONTREFRESHDOCUMENTFIELDS") && DocumentManager.RefreshDocumentFields;
        }

        public virtual void AddField(object obj, string name)
        {
            CheckObjectIsDoc(ref obj);
            if (HasDocVariable(obj, name) == false)
                SetDocVariable(obj, name, "");
            ScreenRefresh();
        }

        public virtual void DeleteField(object obj, string name)
        {
            CheckObjectIsDoc(ref obj);
            if (Array.IndexOf(FieldExclusionList, name.ToUpper()) < 0)
                RemoveDocVariable(obj, name);
            ScreenRefresh();
        }

        public bool RelinkFields(object obj)
        {
            var ver = GetCurrentDocumentVersion(obj);

            return RelinkFields(obj, ver, null);
        }

        private bool RelinkFields(object obj, DocumentVersion version, string[] fields)
        {
            try
            {
                bool update = true;

                long baseprecid = GetDocVariable(obj, BASEPRECEDENT, 0);
                long precid = GetDocVariable(obj, PRECEDENT, 0);

                Precedent prec = null;

                if (precid != 0)
                {
                    prec = Precedent.GetPrecedent(precid);
                    UpdateDocFields(obj, prec, version, fields);
                    update = false;
                }

                if (baseprecid != 0 && update)
                {
                    prec = Precedent.GetPrecedent(baseprecid);
                    UpdateDocFields(obj, prec, version, fields);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected void RefreshDocumentFields(object obj, DocumentVersion version)
        {
            var fields = GetFieldNames(obj);

            var fieldstorefresh = Array.FindAll(fields, f =>
            {
                var field = f.ToUpperInvariant();

                if (field.StartsWith(FieldParser.FieldPrefixReflection))
                    field = field.Substring(FieldParser.FieldPrefixReflection.Length);

                return field.StartsWith(FieldParser.FieldPrefixDocument)
                    || field.StartsWith(FieldParser.FieldPrefixDocumentVersion)
                    || field.StartsWith(FieldParser.FieldPrefixPrecedent);

            });

            if (fieldstorefresh.Length > 0)
            {
                RefreshFields(obj, version, fieldstorefresh);
                string preview = ExtractPreview(obj);
                version.Preview = preview;
                version.GenerateChecksum(preview);
                version.Update();
            }
        }

        public bool RefreshFields(object obj)
        {
            var ver = GetCurrentDocumentVersion(obj);

            return RefreshFields(obj, ver, null);
        }

        private bool RefreshFields(object obj, DocumentVersion version, string[] fieldsToRefresh)
        {
            try
            {
                _refreshing = true;
                bool ret = RelinkFields(obj, version, fieldsToRefresh);
                _refreshing = false;
                return ret;
            }
            catch
            {
                return false;
            }
            finally
            {
                _refreshedfields.Clear();
            }
        }

        public abstract void CheckFields(object obj);

        public virtual void InsertTextIntoDocument(object obj, string text)
        {
            
        }

        public abstract int GetFieldCount(object obj);

        public virtual string[] GetFieldNames(object obj)
        {
            string[] fields = new string[GetFieldCount(obj)];
            for (int ctr = 0; ctr < fields.Length; ctr++)
            {
                fields[ctr] = GetFieldName(obj, ctr).ToUpper();
            }
            return fields;
        }

        public abstract string GetFieldName(object obj, int index);

        public abstract object GetFieldValue(object obj, int index);

        public abstract object GetFieldValue(object obj, string name);

        public abstract void SetFieldValue(object obj, int index, object val);

        public abstract void SetFieldValue(object obj, string name, object val);

        private void UpdateDocFields(object obj, Precedent prec)
        {
            UpdateDocFields(obj, prec, null, null);
        }

        private void UpdateDocFields(object obj, Precedent prec, DocumentVersion version, string[] fields)
        {
            //Load up the current document and create the FILE Object
            var assoc = GetCurrentAssociate(obj);

            var preclink = new PrecedentLink(prec, assoc);

            UpdateDocFields(obj, preclink, version, fields);

        }

        public virtual string ConvertFromLegalFieldName(string fieldName)
        {
            return fieldName;
        }

        public virtual string ConvertToLegalFieldName(string fieldName)
        {
            return fieldName;
        }


        #endregion

        #region IOMSApp Members

        public virtual Apps.IOMSAppAddin GetAddin(string name)
        {
            return null;
        }

        public virtual string GetOpenFileFilter()
        {
            return string.Empty;
        }

        public abstract ProcessJobStatus ProcessJob(PrecedentJob precjob);
        public abstract object Open(OMSDocument document, DocOpenMode mode);
        public abstract object Open(Precedent precedent, DocOpenMode mode);
        public abstract object Open(OMSDocument document, DocOpenMode mode,bool bulkprint,int noofcopies);
        public abstract object Open(DocumentManagement.DocumentVersion version, DocOpenMode mode);
        public abstract object Open(System.IO.FileInfo file);
        public abstract void Close(object obj);
        public abstract void Print(object obj);
        public abstract void PrintQuick(object obj);
        public abstract bool Save(object obj);
        public abstract bool SaveQuick(object obj);
        public abstract bool SaveAs(object obj, bool asPrecedent);
        public abstract string DefaultDocType { get; }
        public abstract int GetDocEditingTime(object obj);
        public abstract string DefaultPrecedentType { get; }
        public abstract string ExtractPreview(object obj);

        public virtual bool? WillHandleRunCommand(object obj, string cmd)
        {
            return true;
        }

        public abstract void RunCommand(object obj, string cmd);

        public virtual object RunAddinCommand(string name, string command, object obj)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            var current = Apps.ApplicationManager.CurrentManager.CurrentApplication;

            var external = current.GetAddin(name);

            if (external == null)
                throw new InvalidOperationException(String.Format(CultureInfo.InvariantCulture, "There is no external addin of the name '{0}' associated with the OMS application '{1}'", name, current.ToString()));

            return external.RunCommand(command, obj);
        }

        public abstract object TemplateStart(object obj, string precName, Associate assoc);
        public abstract object TemplateStart(object obj, Precedent prec, Associate assoc);
        public abstract void ActivateApplication();
        public abstract void ActivateDocument(object obj);


        #endregion

        #region Methods

        protected abstract Associate SelectAssociate(object obj, bool useDefaultAssociate, Associate assoc);
        protected abstract void CheckObjectIsDoc(ref object obj);
        protected abstract void ScreenRefresh();
        protected abstract void UpdateDocFields(object obj, PrecedentLink preclink);
        protected abstract void UpdateDocFields(object obj, PrecedentLink preclink, DocumentVersion version, string[] fields);
        protected abstract bool IsReadOnly(object obj);

        #endregion

        #region Properties

        protected Common.KeyValueCollection RefreshedFields
        {
            get
            {
                return _refreshedfields;
            }
        }

        protected bool Refreshing
        {
            get
            {
                return _refreshing;
            }
        }

        public DocumentManagementMode DocumentManagementMode
        {
            get
            {
                return DocumentManager.Mode;
            }
        }

        #endregion

        #region Static



        static public Document GetDocument(System.IO.FileInfo file)
        {
            IStorageItem item = GetDocumentItem(file);

            Document doc = item as Document;
            DocumentVersion version = item as DocumentVersion;

            if (version != null)
                doc = version.ParentDocument;

            return doc;

        }

        static public DocumentVersion GetDocumentVersion(System.IO.FileInfo file)
        {
            return GetDocumentItem(file) as DocumentVersion;
        }

        static private IStorageItem GetDocumentItem(System.IO.FileInfo file)
        {
            long docidex;
            Guid versionidex;

            long docid;
            long companyid;
            long serialno;
            string datakey;
            Guid versionid;

            using (System.IO.OleFileInfo ole = new System.IO.OleFileInfo(file.FullName))
            {
                OutboundPropertyNames obprops = new OutboundPropertyNames();


                docidex = GetDocumentProperty<long>(ole, obprops.DOCID_DATAKEY, -1);
                if (docidex == -1)
                    docidex = GetDocumentProperty<long>(ole, obprops.DOCID_COMPANY, -1);

                versionidex = GetDocumentProperty<Guid>(ole, obprops.VERID_DATAKEY, Guid.Empty);
                if (versionidex == Guid.Empty)
                    versionidex = GetDocumentProperty<Guid>(ole, obprops.VERID_COMPANY, Guid.Empty);

                docid = GetDocumentProperty<long>(ole, DOCUMENT, -1);
                companyid = GetDocumentProperty<long>(ole, COMPANY, -1);
                datakey = GetDocumentProperty<string>(ole, DATAKEY, String.Empty);
                serialno = GetDocumentProperty<long>(ole, BRANCH, -1);
                versionid = GetDocumentProperty<Guid>(ole, DOCUMENT_VERSION, Guid.Empty);
            }

            Document doc = null;

            if (docidex != -1)
            {
                doc = Document.GetDocument(docidex);
                if (versionidex == Guid.Empty)
                    return doc;
                else
                    return doc.GetVersion(versionidex);
            }
            else if (companyid == Session.CurrentSession.CompanyID &&
                (String.IsNullOrEmpty(datakey) || datakey.ToUpperInvariant().Equals(Session.CurrentSession.DataKey.ToUpperInvariant())))
            {
                doc = Document.GetDocument(docid);
                if (versionid == Guid.Empty)
                    return doc;
                else
                    return doc.GetVersion(versionid);
            }

            return null;
        }

        public static T GetDocumentProperty<T>(System.IO.OleFileInfo file, string name, T def)
        {
            try
            {
                //Add the global prefix
                string globalname = Session.CurrentSession.GetExternalDocumentPropertyName(name);
                T ret;

                if (typeof(T) == typeof(Guid))
                {
                    try
                    {
                        var val = Convert.ToString(file.GetProperty(globalname));

                        if (String.IsNullOrEmpty(val) || val.Length < 32)
                            ret = def;
                        else
                            ret = (T)(object)new Guid(val);
                    }
                    catch (FormatException)
                    {
                        ret = (T)(object)Guid.Empty;
                    }
                }
                else
                {
                    var val = file.GetProperty(globalname);

                    if (val == null)
                        ret = def;
                    else
                        ret = (T)Convert.ChangeType(val, typeof(T));
                }

                if (ret == null)
                    return def;
                else
                    return ret;
            }
            catch
            {
                return def;
            }
        }

        public static bool HasDocumentProperty(System.IO.OleFileInfo file, string name)
        {
            //Add the global prefix
            string globalname = Session.CurrentSession.GetExternalDocumentPropertyName(name);
            return file.HasProperty(globalname);
        }

        public static void SetDocumentProperty<T>(System.IO.OleFileInfo file, string name, T value)
        {
            //Add the global prefix
            string globalname = Session.CurrentSession.GetExternalDocumentPropertyName(name);
            file.SetProperty(globalname, value);
        }

        public static void RemoveDocumentProperty(System.IO.OleFileInfo file, string name)
        {
            //Add the global prefix
            string globalname = Session.CurrentSession.GetExternalDocumentPropertyName(name);
            file.RemoveProperty(globalname);
        }

        [Obsolete("Please use AttachDocumentProperties")]
        public static void AttachDocumentVars(System.IO.FileInfo file, Document doc, DocumentVersion version)
        {
            AttachDocumentProperties(file, doc, version);
        }

        public static void AttachDocumentProperties(System.IO.FileInfo file, Document doc, DocumentVersion version)
        {
            Associate assoc = doc.Associate;

            using (System.IO.OleFileInfo obj = new System.IO.OleFileInfo(file.FullName))
            {
                if (doc.ID != GetDocumentProperty<long>(obj, DOCUMENT, -1))
                    SetDocumentProperty<long>(obj, DOCUMENT, doc.ID);

                if (doc.ID == GetDocumentProperty<long>(obj, DOCUMENT, -1))
                {
                    if (HasDocumentProperty(obj, PRIVATE)) RemoveDocumentProperty(obj, PRIVATE);
                    if (FWBS.OMS.Session.CurrentSession.CompanyID != GetDocumentProperty<long>(obj, COMPANY, -1)) SetDocumentProperty<long>(obj, COMPANY, FWBS.OMS.Session.CurrentSession.CompanyID);
                    if (FWBS.OMS.Session.CurrentSession.DataKey != GetDocumentProperty<string>(obj, DATAKEY, "")) SetDocumentProperty<string>(obj, DATAKEY, FWBS.OMS.Session.CurrentSession.DataKey);
                    if (FWBS.OMS.Session.CurrentSession.SerialNumber != GetDocumentProperty<long>(obj, BRANCH, -1)) SetDocumentProperty<long>(obj, BRANCH, FWBS.OMS.Session.CurrentSession.SerialNumber);
                    if (FWBS.OMS.Session.CurrentSession.Edition != GetDocumentProperty<string>(obj, EDITION, "")) SetDocumentProperty<string>(obj, EDITION, FWBS.OMS.Session.CurrentSession.Edition);
                    if (assoc.OMSFile.Client.ClientID != GetDocumentProperty<long>(obj, CLIENT, -1)) SetDocumentProperty<long>(obj, CLIENT, assoc.OMSFile.Client.ClientID);
                    if (assoc.OMSFile.ID != GetDocumentProperty<long>(obj, FILE, -1)) SetDocumentProperty<long>(obj, FILE, assoc.OMSFile.ID);
                    if (assoc.ID != GetDocumentProperty<long>(obj, ASSOCIATE, -1)) SetDocumentProperty<long>(obj, ASSOCIATE, assoc.ID);
                    if (doc.ExternalId != GetDocumentProperty<string>(obj, DOCUMENT_EXTERNAL, "")) SetDocumentProperty<string>(obj, DOCUMENT_EXTERNAL, doc.ExternalId);
                    if (doc.DocumentType != GetDocumentProperty<string>(obj, TYPE, "")) SetDocumentProperty<string>(obj, TYPE, doc.DocumentType);

                    //Set the phase id to the document (if applicable),
                    object phaseid = null;
                    try { phaseid = doc.GetExtraInfo("phID"); }
                    catch { };
                    if (phaseid == null || phaseid == DBNull.Value)
                    {
                        if (HasDocumentProperty(obj, PHASE)) RemoveDocumentProperty(obj, PHASE);
                    }
                    else
                        if (phaseid.Equals(GetDocumentProperty<int>(obj, PHASE, -1)) == false) SetDocumentProperty<int>(obj, PHASE, Convert.ToInt32(phaseid));


                    Precedent baseprec = doc.BasePrecedent;
                    if (baseprec == null)
                    {
                        if (HasDocumentProperty(obj, BASEPRECEDENT)) RemoveDocumentProperty(obj, BASEPRECEDENT);
                    }
                    else
                        if (baseprec.ID != GetDocumentProperty<long>(obj, BASEPRECEDENT, -1)) SetDocumentProperty<long>(obj, BASEPRECEDENT, baseprec.ID);

                    Precedent lastprec = doc.LastPrecedent;
                    if (lastprec == null)
                    {
                        if (HasDocumentProperty(obj, PRECEDENT)) RemoveDocumentProperty(obj, PRECEDENT);
                    }
                    else
                        if (lastprec.ID != GetDocumentProperty<long>(obj, PRECEDENT, -1)) SetDocumentProperty<long>(obj, PRECEDENT, lastprec.ID);

                    //If a particular version is passed then set the document variable.
                    if (version == null)
                    {
                        if (HasDocumentProperty(obj, DOCUMENT_VERSION)) RemoveDocumentProperty(obj, DOCUMENT_VERSION);
                        if (HasDocumentProperty(obj, DOCUMENT_LABEL)) RemoveDocumentProperty(obj, DOCUMENT_LABEL);
                    }
                    else
                    {
                        Guid stored;
                        try
                        {
                            stored = new Guid(GetDocumentProperty<string>(obj, DOCUMENT_VERSION, Guid.Empty.ToString()));
                        }
                        catch (FormatException)
                        {
                            stored = Guid.Empty;
                        }
                        if (version.Id.Equals(stored) == false)
                            SetDocumentProperty<string>(obj, DOCUMENT_VERSION, version.Id.ToString());

                        if (version.Label.Equals(GetDocumentProperty<string>(obj, DOCUMENT_LABEL, String.Empty)) == false)
                            SetDocumentProperty<string>(obj, DOCUMENT_LABEL, version.Label);
                    }
                }

                obj.Save();
            }
        }

        #endregion

        private sealed class OutboundPropertyNames
        {
            public readonly string DOCID_COMPANY;
            public readonly string DOCID_DATAKEY;
            public readonly string DOCID_SERIAL;
            public readonly string VERID_COMPANY;
            public readonly string VERID_DATAKEY;



            public OutboundPropertyNames()
            {
                DOCID_COMPANY = String.Format("{0}_{1}", DOCUMENT, Session.CurrentSession.CompanyID);
                DOCID_DATAKEY = String.Format("{0}_{1}_{2}", DOCUMENT, Session.CurrentSession.CompanyID, Session.CurrentSession.DataKey);
                DOCID_SERIAL = String.Format("{0}_{1}", DOCUMENT, Session.CurrentSession.SerialNumber);
                VERID_COMPANY = String.Format("{0}_{1}", DOCUMENT_VERSION, Session.CurrentSession.CompanyID);
                VERID_DATAKEY = String.Format("{0}_{1}_{2}", DOCUMENT_VERSION, Session.CurrentSession.CompanyID, Session.CurrentSession.DataKey);
            }
        }
    }



}
