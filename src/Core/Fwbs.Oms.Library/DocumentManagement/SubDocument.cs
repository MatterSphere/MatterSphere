using System;
using System.Text;

namespace FWBS.OMS
{
    using DocumentManagement;
    using DocumentManagement.Storage;
    using Document = OMSDocument;
    using SP = FWBS.OMS.DocumentManagement.Storage.StorageProvider;

    /// <summary>
    /// A class that represents a sub document.  This object only works with files.
    /// </summary>
    public class SubDocument
    {
        public enum StoreAs
        {
            None = 0,
            NewDocument,
            NewVersion
        }

        public enum SubDocumentType
        {
            Miscellaneous,
            Attachment
        }

        #region Fields


        private bool saved = false;
        /// <summary>
        /// Display name of the attachment.
        /// </summary>
        private string _displayName = "";

        /// <summary>
        /// File location of the document.
        /// </summary>
        private System.IO.FileInfo _file = null;

        /// <summary>
        /// A flag that specifies whether it will be stored alongside a parent document.
        /// </summary>
        private bool _store = true;

        /// <summary>
        /// A document object that is the parent document.
        /// </summary>
        private OMSDocument _parent = null;


        /// <summary>
        /// The date and time of when the document was received.
        /// </summary>
        private DateTime _authored = DateTime.Now;

        /// <summary>
        /// A flag that indicates that the document already exists within the system.
        /// </summary>
        private bool alreadyexists = false;

        /// <summary>
        /// A document object that has been matched with the incoming doc id.
        /// </summary>
        private DocumentVersion existingdoc;

        /// <summary>
        /// Information collated throughout the matching process.
        /// </summary>
        private StringBuilder information;

        private SubDocumentType type;

        private DocumentDirection direction;

        #endregion

        #region Constructors

        private SubDocument() { }

        /// <summary>
        /// Constructs a sub document.
        /// </summary>
        /// <param name="parent">The parent document object.</param>
        /// <param name="displayName">The display name of the sub document.</param>
        /// <param name="file">The file information object to store.</param>
        public SubDocument(OMSDocument parent, string displayName, System.IO.FileInfo file)
        {

            _parent = parent;
            _displayName = displayName;
            _file = file;

            CheckStatus();
        }

        /// <summary>
        /// Constructs a sub document.
        /// </summary>
        /// <param name="displayName">The display name of the sub document.</param>
        /// <param name="file">The file information object to store.</param>
        public SubDocument(string displayName, System.IO.FileInfo file)
            : this(null, displayName, file)
        {
        }

        #endregion

        #region Properties

        public DocumentDirection Direction
        {
            get
            {
                return direction;
            }
            set
            {
                direction = value;
            }
        }

        public bool AllowStore
        {
            get
            {
                return !(
                DocumentType == SubDocumentType.Attachment
                && Direction == DocumentDirection.Out
                && AlreadyExists
                );
            }

        }

        public SubDocumentType DocumentType
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }

        /// <summary>
        /// Gets or Sets the parent document.
        /// </summary>
        public OMSDocument Parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
                if (_parent != null)
                {
                    switch (_parent.DocumentType)
                    {
                        case "EMAIL":
                            type = SubDocumentType.Attachment;
                            break;
                        default:
                            type = SubDocumentType.Miscellaneous;
                            break;
                    }
                    direction = _parent.Direction;
                }
            }
        }

        /// <summary>
        /// Gets the dipslay name of the sub document.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (_displayName == "" || _displayName == null)
                    return _file.Name;
                else
                    return _displayName;
            }
            set
            {
                _displayName = value;
            }
        }

        /// <summary>
        /// Gets the icon of the sub document.
        /// </summary>
        public System.Drawing.Icon Icon
        {
            get
            {
                return Common.IconReader.GetFileIcon(_file.Name, Common.IconReader.IconSize.Large, false);
            }
        }

        /// <summary>
        /// Gets the icon of the sub document.
        /// </summary>
        public System.Drawing.Icon SmallIcon
        {
            get
            {
                return Common.IconReader.GetFileIcon(_file.Name, Common.IconReader.IconSize.Small, false);
            }
        }

        /// <summary>
        /// Uses the versioning feature of the DM.
        /// </summary>
        public StoreAs StoreAsSetting
        {
            get
            {
                if (AllowStore)
                {
                    if (AlreadyExists)
                    {

                        if (AllowsVersions)
                            return SubDocument.StoreAs.NewVersion;
                        else
                            return SubDocument.StoreAs.NewDocument;

                    }
                    else
                        return SubDocument.StoreAs.NewDocument;
                }
                else
                    return StoreAs.None;
            }
        }

        /// <summary>
        /// Gets or Sets whether the sub document is to be stored.
        /// </summary>
        public bool Store
        {
            get
            {
                return _store;
            }
            set
            {
                _store = value;
            }
        }

        public bool Acknowledged { get; set; }

        public bool IsOwnedByDifferentFile
        {
            get
            {
                if (ExistingDocument == null)
                    return false;

                if (Parent == null)
                    return false;

                return ExistingDocument.ParentDocument.OMSFile.ID != Parent.OMSFileID;
            }
        }


        public bool Saved
        {
            get
            {
                return saved;
            }
            set
            {
                saved = value;
            }
        }


        public bool AlreadyExists
        {
            get
            {
                return (alreadyexists && existingdoc != null);
            }
        }

        /// <summary>
        /// Gets the matched document used from the ole properties of the file.
        /// </summary>
        public DocumentVersion ExistingDocument
        {
            get
            {
                return existingdoc;
            }
        }

        public StringBuilder Information
        {
            get
            {
                if (information == null)
                    information = new StringBuilder();
                return information;
            }
        }

        /// <summary>
        /// Gets the date and time in which the document was received.
        /// </summary>
        public DateTime AuthoredDate
        {
            get
            {
                return _authored;
            }
            set
            {
                _authored = value;
            }
        }

        public bool AllowsVersions
        {
            get
            {
                if (!AlreadyExists)
                    return false;
                else
                {
                    IStorageItem item = existingdoc;
                    return item.Supports(StorageFeature.Versioning);
                }
            }
        }

        public bool MarkAsLatest { get; set; }

        public string Wallet{ get; set; }

        public Guid FolderGUID { get; set; }

        public string To { get; set; }
        [Obsolete("Not Currently Stored To the Database")]
        public string From { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }

        #endregion

        #region Methods

        public bool Resolve()
        {
            CheckStatus();
            return AlreadyExists;
        }

        public void Attach(Document document)
        {
            if (document == null)
                Detach();
            else
                Attach((DocumentVersion)document.GetLatestVersion());
        }
        public void Attach(DocumentVersion version)
        {
            if (version== null)
                Detach();
            else
            {
                alreadyexists = true;
                existingdoc = version;
            }
        }

        public void Detach()
        {
            alreadyexists = false;
            existingdoc = null;
            MarkAsLatest = false;
        }

        private void CheckStatus()
        {
            try
            {
                DocumentVersion version = Interfaces.IOMSApp.GetDocumentVersion(_file);
                if (version != null)
                {
                    Attach(version);
                    return;
                }
                else
                {
                    Document doc = Interfaces.IOMSApp.GetDocument(_file);
                    Attach(doc);
                }
            }
            catch (Exception ex)
            {
                Information.AppendFormat("ERR: {0}", ex.Message);
            }
        }



        /// <summary>
        /// Updates the sub document to the database.
        /// </summary>
        public void Update()
        {
            if (!Saved)
            {
                Validate();

                if (Store && _parent != null)
                {
                    
                    StoreAs val = StoreAsSetting;

                    switch (val)
                    {
                        case StoreAs.NewDocument:
                            SaveAsNewDocument();
                            break;
                        case StoreAs.NewVersion:
                            SaveAsNewVersion();
                            break;
                        case StoreAs.None:
                            SaveAsExistingOutgoingAttachment();
                            break;
                    }

                    Saved = true;

                }
            }
        }


        public void Validate()
        {
            //If incoming document and an attachment is already belonging to a matter that is different
            //from the INCOMING document.

            if (!Store)
                return;

            if (direction != DocumentDirection.In)
                return;

            if (!IsOwnedByDifferentFile)
                return;


            if (Acknowledged)
                return;

            throw new SubDocumentAlreadyOwnedException();

        }



        private void SaveAsExistingOutgoingAttachment()
        {
            SetRegisteredApp(existingdoc.ParentDocument);

            IStorageItemVersionable versionable = existingdoc.BaseVersionalStorageItem;
            versionable.SetWorkingVersion(existingdoc);

            Log(existingdoc);

            existingdoc.Update();
        }


        private void SaveAsNewVersion()
        {
            SetRegisteredApp(existingdoc.ParentDocument);

            SP provider = existingdoc.GetStorageProvider();

            StorageSettingsCollection settings = provider.GetDefaultSettings(existingdoc, SettingsType.Store);

            if (Parent!= null)
                settings.Merge(((IStorageItem)Parent).GetSettings());

            VersionStoreSettings verset = settings.GetSettings<VersionStoreSettings>();
            verset.MarkAsLatest = MarkAsLatest;
            verset.Comments = String.Empty;

            switch (Session.CurrentSession.SubdocumentVersioning)
            {
                case "N":
                    {
                        verset.SaveItemAs = VersionStoreSettings.StoreAs.NewVersion;
                    }
                    break;
                case "V":
                default:
                    {
                        verset.SaveItemAs = VersionStoreSettings.StoreAs.NewSubVersion;
                        if (existingdoc.Label.Split('.').Length - 1 >= Session.CurrentSession.DocumentMaximumRevisionCount)
                        {   //ReachDocMaxRevisionCount
                            verset.SaveItemAs = VersionStoreSettings.StoreAs.NewVersion;
                        }
                    }
                    break;
            }

            IStorageItemVersionable versionable = existingdoc.BaseVersionalStorageItem;

            try
            {
                versionable.NewVersion += new EventHandler<NewVersionEventArgs>(versionable_NewVersion);

                Log(verset);

                provider.Store(existingdoc, _file, _file, true, settings);

                Log(existingdoc);
            }
            finally
            {
                versionable.NewVersion -= new EventHandler<NewVersionEventArgs>(versionable_NewVersion);
            }

        }

        private void SaveAsNewDocument()
        {
            OMSDocument doc = new OMSDocument(_parent.Associate, DisplayName, Precedent.GetDefaultPrecedent("SHELL", _parent.Associate), null, 0, _parent.Direction, _file.Extension, _parent.CurrentStorageProviderID, null);
            SetRegisteredApp(doc);

            try { doc.SetExtraInfo("docParent", _parent.ID); }
            catch { }
            try { doc.SetExtraInfo("phID", _parent.GetExtraInfo("phID")); }
            catch { }
            try { doc.SetExtraInfo("docpassword", _parent.GetExtraInfo("docpassword")); }
            catch { }
            try { doc.SetExtraInfo("docpasswordhint", _parent.GetExtraInfo("docpasswordhint")); }
            catch { }

            if (String.IsNullOrWhiteSpace(FolderGUID.ToString()) || FolderGUID == Guid.Empty)
                doc.FolderGUID = _parent.FolderGUID;
            else
                doc.FolderGUID = FolderGUID;


            doc.Description = DisplayName;
            doc.AuthoredDate = AuthoredDate;
            doc.TimeRecords.SkipTime = true;

            IStorageItemVersionable versionable = doc;
            DocumentVersion version = (DocumentVersion)versionable.GetLatestVersion();
            versionable.SetWorkingVersion(version);

            doc.InternalUpdate();

            SP provider = doc.GetStorageProvider();

            StorageSettingsCollection settings = provider.GetDefaultSettings(doc, SettingsType.Store);

            if (Parent != null)
                settings.Merge(((IStorageItem)Parent).GetSettings());

            VersionStoreSettings verset = settings.GetSettings<VersionStoreSettings>();
            verset.MarkAsLatest = true;
            verset.SaveItemAs = VersionStoreSettings.StoreAs.OriginalOverwrite;
            verset.Comments = String.Empty;

            try
            {
                versionable.NewVersion += new EventHandler<NewVersionEventArgs>(versionable_NewVersion);

                Interfaces.IOMSApp.AttachDocumentProperties(_file, doc, version);

                Log(verset);

                provider.Store(version, _file, _file, true, settings);

                Log(version);
            }
            finally
            {
                versionable.NewVersion -= new EventHandler<NewVersionEventArgs>(versionable_NewVersion);
            }

        }

        private void versionable_NewVersion(object sender, NewVersionEventArgs e)
        {
           Interfaces.IOMSApp.AttachDocumentProperties((System.IO.FileInfo)e.Tag, (Document)e.Version.BaseStorageItem, (DocumentVersion)e.Version);
        }


        private void Log(VersionStoreSettings settings)
        {
            if (_parent == null)
                return;

            switch (type)
            {
                case SubDocumentType.Attachment:
                    {
                        if (this.Direction == DocumentDirection.Out)
                        {
                            if (string.IsNullOrEmpty(settings.Comments))
                                settings.Comments = Session.CurrentSession.Resources.GetMessage("ATTACHEMAILOUT", "Emailed to '%1%'", "", _parent.Associate.ContactName).Text;
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(settings.Comments))
                                settings.Comments = Session.CurrentSession.Resources.GetMessage("ATTACHEMAILIN", "Received by Email from '%1%'", "", _parent.Associate.ContactName).Text;

                        }

                    }
                    break;
            }

        }

        private void Log(DocumentVersion version)
        {
            switch (type)
            {
                case SubDocumentType.Attachment:
                    {
                        if (this.Direction == DocumentDirection.Out)
                        {
                            if (string.IsNullOrEmpty(version.Comments))
                                version.Comments = Session.CurrentSession.Resources.GetMessage("ATTACHEMAILOUT", "Emailed to '%1%'", "", _parent.Associate.ContactName).Text;

                            version.AddActivity("EMAILED", "OUT", _parent.Associate.ContactName);

                            if (!string.IsNullOrEmpty(To))
                                version.AddActivity("EMAILED", "TO", To);
                            if (!string.IsNullOrEmpty(CC))
                                version.AddActivity("EMAILED", "CC", CC);
                            if (!string.IsNullOrEmpty(BCC))
                                version.AddActivity("EMAILED", "BCC", BCC);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(version.Comments))
                                version.Comments = Session.CurrentSession.Resources.GetMessage("ATTACHEMAILIN", "Received by Email from '%1%'", "", _parent.Associate.ContactName).Text;

                            version.AddActivity("EMAILED", "IN", _parent.Associate.ContactName);
                            
                        }

                    }
                    break;
            }
            
        }

        private void SetRegisteredApp(Document doc)
        {
            Apps.RegisteredApplication oldapp = doc.DocProgType;
            Apps.RegisteredApplication newapp = Apps.ApplicationManager.CurrentManager.GetRegisteredApplicationByExtension(_file.Extension);

            if (newapp != null)
            {
                if (oldapp == null)
                    doc.DocProgType = newapp;
                else
                {
                    if (newapp.ID != oldapp.ID)
                        doc.DocProgType = newapp;
                }
            }
        }

        /// <summary>
        /// Returns the string reprentation of the object.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DisplayName;
        }

        #endregion
    }
}
