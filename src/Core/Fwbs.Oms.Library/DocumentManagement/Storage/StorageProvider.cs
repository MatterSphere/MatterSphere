using System;
using System.IO;

namespace FWBS.OMS.DocumentManagement.Storage
{
    using Document = OMSDocument;

    /// <summary>
    /// An base abstract storage location provider that other providers must derive from.
    /// This class enables the redirection of store items, be it documents or precedents,
    /// when storing or fetching the items from where-ever they might be.
    /// </summary>
    public abstract class StorageProvider
    {
        #region Fields

        private StorageProviderService service;

        #endregion

        #region Properties

        public string Name
        {
            get
            {
                return GetType().Name;
            }
        }

        private short id;
        public short Id
        {
            get
            {
                return id;
            }
            internal set
            {
                id = value;
            }
        }

        private PrecSaveMode savemode;
        public PrecSaveMode SaveMode
        {
            get
            {
                return savemode;
            }
            set
            {
                savemode = value;
            }
        }


        #endregion

        #region Fetch

        public FetchResults Fetch(IStorageItem item)
        {
            return Fetch(item, true);
        }

        public FetchResults Fetch(IStorageItem item, bool throwOnError)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return Fetch(item, throwOnError, item.GetSettings());
        }

        public FetchResults Fetch(IStorageItem item, bool throwOnError, StorageSettingsCollection settings)
        {
            return Fetch(item, throwOnError, settings, Common.TriState.Null);
        }

        public FetchResults Fetch(IStorageItem item, bool throwOnError, Common.TriState force)
        {
            return Fetch(item, throwOnError, item.GetSettings(), force);
        }

        public FetchResults Fetch(IStorageItem item, bool throwOnError, StorageSettingsCollection settings, Common.TriState force,bool SpecificVersion = false )
        {
            if (item == null)
                throw new ArgumentNullException("item");

            //DM - 04/12/06 - Refresh the document header if the latest version is required.
            //DM - 08/06/07 - Refresh the precedent header record just before opening.
            FWBS.OMS.Interfaces.IUpdateable refreshable = item as FWBS.OMS.Interfaces.IUpdateable;
            if (refreshable != null)
                refreshable.Refresh(true);

            if (settings == null || settings.Count == 0)
                settings = GetDefaultFetchSettings(item);


            IStorageItem original = item;

            CheckFeature(item, StorageFeature.Retrieving);

            try
            {
                BeforeFetch(ref item, settings, SpecificVersion);


                FetchResults results = GetExistingLocalFile(item, settings, force);
                if (results == null)
                {
                    results = InternalFetch(item, settings);
                    //DMB 16/6/2006 Need to do a Null check now to prevent our favourite Object ref Error
                    if (results == null)
                        throw GetMissingStorageItemException(item);

                    results.IsLocalCopy = false;
                    results.CachedDate = null;
                }

                AfterFetch(results, settings);

                original.ClearSettings();


                try
                {
                    if ((results.LocalFile.Attributes & System.IO.FileAttributes.ReadOnly) == System.IO.FileAttributes.ReadOnly)
                        results.LocalFile.Attributes &= ~System.IO.FileAttributes.ReadOnly;
                }
                catch (System.Security.SecurityException)
                {
                }


                if (results.IsLocalCopy)
                    StorageManager.CurrentManager.LocalDocuments.Set(item, results.LocalFile, false);
                else
                {
                    results.Item.AddActivity("OPENED", null);
                    StorageManager.CurrentManager.LocalDocuments.Set(item, results.LocalFile, true);
                }

                return results;
            }
            catch (StorageException)
            {
                if (throwOnError)
                    throw;
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (throwOnError)
                    throw new StorageException("SPFETCHEX1", "There was a problem fetching specified item '%1%'." + Environment.NewLine + Environment.NewLine + "%2%", ex, item.DisplayID, ex.Message);
                else
                    return null;
            }
        }

        protected virtual FetchResults GetExistingLocalFile(IStorageItem item, StorageSettingsCollection settings, Common.TriState force)
        {
            DateTime? cached;
            System.IO.FileInfo local = StorageManager.CurrentManager.LocalDocuments.GetLocalFile(item, out cached);
            bool difftoserver = StorageManager.CurrentManager.LocalDocuments.IsDifferentToServer(item);
            bool haschanged = StorageManager.CurrentManager.LocalDocuments.HasChanged(item);
            bool uselocal = false;

            if (local != null)
            {
                if (System.IO.File.Exists(local.FullName))
                {

                    if (force == FWBS.Common.TriState.Null)
                    {


                        if (StorageManager.CurrentManager.LocalDocuments.IsCheckedOut(item) == false && StorageManager.CurrentManager.LocalDocuments.HasChanged(item) == false)
                        {
                            if (difftoserver)
                            {
                                if (IsConnected(item))
                                    force = Common.TriState.True;
                                else
                                    force = Common.TriState.False;
                            }
                            else
                                force = Common.TriState.False;
                        }
                        else
                            force = Common.TriState.False;

                    }
                }
            }

            if (force == Common.TriState.False)
                uselocal = true;

            if (!uselocal)
                return null;
            else
            {
                FetchResults results = new FetchResults(item, local);
                results.IsLocalCopy = true;
                results.HasChanged = haschanged;
                results.CachedDate = cached;
                results.NewerExists = difftoserver;
                return results;
            }
        }

        protected virtual void BeforeFetch(ref IStorageItem item, StorageSettingsCollection settings)
        {
            BeforeFetch(ref item, settings, false);   
        }

        protected virtual void BeforeFetch(ref IStorageItem item, StorageSettingsCollection settings,bool SpecifcVersion = false )
        {
            //Make sure a OMSDocument is never returned.
            OMSDocument doc = item as OMSDocument;
            if (doc != null)
            {
                item = doc.GetLatestVersion();
            }

            IStorageItemVersionable original_versionable = item as IStorageItemVersionable;
            IStorageItemVersion original_version = item as IStorageItemVersion;
            IStorageItem original_item = item;

            if (Supports(StorageFeature.Versioning, item))
            {
                VersionFetchSettings versettings = GetSettings<VersionFetchSettings>(settings);

                if (SpecifcVersion == true)
                    versettings.Version = VersionFetchSettings.FetchAs.Specific;


                if (original_versionable != null)
                {
                    switch (versettings.Version)
                    {
                        case VersionFetchSettings.FetchAs.Current:
                            {
                                item = original_item;
                            }
                            break;
                        case VersionFetchSettings.FetchAs.Latest:
                            {
                                //If the document object was given then the latest version was already got from the top of the method.
                                if (doc == null)
                                    item = original_versionable.GetLatestVersion();
                            }
                            break;
                        case VersionFetchSettings.FetchAs.Specific:

                            if (SpecifcVersion == true)
                                item = original_versionable.GetVersion(item.ToString());  
                            else
                            item = original_versionable.GetVersion(versettings.VersionLabel);
                            break;
                    }
                }

            }
            else
            {
                if (original_version != null)
                {
                    item = original_version;
                }
            }

        }

        protected virtual void AfterFetch(FetchResults results, StorageSettingsCollection settings)
        {
            //Check of lockable support/
            IStorageItemLockable lockable = GetLockableItem(results.Item);

            if (Supports(StorageFeature.Locking, results.Item))
            {
                if (lockable != null)
                {
                    LockableFetchSettings locksettings = GetSettings<LockableFetchSettings>(settings);
                    if (locksettings.CheckOut)
                    {
                        lockable.CheckOut(results.LocalFile);
                    }
                }
            }
        }

        protected abstract FetchResults InternalFetch(IStorageItem item, StorageSettingsCollection settings);

        protected virtual StorageSettingsCollection GetDefaultFetchSettings(IStorageItem item)
        {
            StorageSettingsCollection settings = item.GetSettings();
            if (settings == null)
                settings = new StorageSettingsCollection();

            if (Supports(StorageFeature.Versioning, item))
            {
                VersionFetchSettings versettings = GetSettings<VersionFetchSettings>(settings);
                versettings.Version = VersionFetchSettings.FetchAs.Latest;
            }

            if (Supports(StorageFeature.Locking, item))
            {
                LockableFetchSettings locksettings = GetSettings<LockableFetchSettings>(settings);
                locksettings.CheckOut = false;
            }

            return settings;
        }

        public bool GetCheckoutOption(IStorageItem item)
        {
            if (item == null)
                return false;

            if (Supports(StorageFeature.Locking, item))
            {
                //Get the default versioning settings for a document.
                Document document = item as Document;
                DocumentVersion version = item as DocumentVersion;

                Precedent precedent = item as Precedent;
                PrecedentVersion precedentVersion = item as PrecedentVersion;

                IStorageItemLockable lockable = GetLockableItem(item);

                if (!item.IsNew)
                {

                    string val = string.Empty;

                    if (document != null)
                    {
                        val = document.OMSFile.CurrentFileType.DocumentLocking;
                    }

                    if (version != null)
                    {
                        val = version.ParentDocument.OMSFile.CurrentFileType.DocumentLocking;
                    }

                    if (precedent != null)
                    {
                        val = Session.CurrentSession.DocumentLocking;
                    } 

                    if (val == String.Empty)
                        val = Session.CurrentSession.DocumentLocking;

                    if (precedent != null)
                    {
                        switch (val)
                        {
                            case "S":
                            {
                                if (lockable != null)
                                {
                                    if (!lockable.IsCheckedOut)
                                        return true;
                                }
                            }
                                break;
                        }
                    }
                    else
                    {
                        switch (val)
                        {
                            case "E":
                            {
                                if (lockable != null)
                                {
                                    if (!lockable.IsCheckedOut)
                                        return true;
                                }
                            }
                                break;
                        }
                    }

                }

            }

            return false;
        }


        #endregion

        #region Register

        public RegisterResult Register(IStorageItem item)
        {
            return Register(item, null, true);
        }

        public RegisterResult Register(IStorageItem item, object tag)
        {
            return Register(item, tag, true);
        }

        public RegisterResult Register(IStorageItem item, object tag, bool throwOnError)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            IStorageItem original = item;

            CheckFeature(item, StorageFeature.Register);

            try
            {
                RegisterResult results = InternalRegister(item, tag);

                return results;
            }
            catch (StorageException)
            {
                if (throwOnError)
                    throw;
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (throwOnError)
                    throw new StorageException("SPREGISTEREX",
@"There is a problem registering the document '%1%'. The storage location
that is attempting to assign an alternate document id is currently unavailable.  You may 
wish to store your document on your local computer until the storage location becomes 
available and then re-save your document.  
Please inform your System Administrator.", ex, item.DisplayID);
                else
                    return null;
            }
        }


        protected virtual RegisterResult InternalRegister(IStorageItem item, object tag)
        {
            return null;
        }

        #endregion


        #region Store

        public StoreResults Store(IStorageItem item, System.IO.FileInfo source)
        {
            return Store(item, source, (FWBS.OMS.Interfaces.IOMSApp)null);
        }

        public StoreResults Store(IStorageItem item, System.IO.FileInfo source, FWBS.OMS.Interfaces.IOMSApp app)
        {
            return Store(item, source, null, true, app);
        }

        public StoreResults Store(IStorageItem item, System.IO.FileInfo source, object tag)
        {
            return Store(item, source, tag, null);
        }
        public StoreResults Store(IStorageItem item, System.IO.FileInfo source, object tag, FWBS.OMS.Interfaces.IOMSApp app)
        {
            return Store(item, source, tag, true, app);
        }

        public StoreResults Store(IStorageItem item, System.IO.FileInfo source, object tag, bool throwOnError)
        {
            return Store(item, source, tag, throwOnError, (FWBS.OMS.Interfaces.IOMSApp)null);
        }

        public StoreResults Store(IStorageItem item, System.IO.FileInfo source, object tag, bool throwOnError, FWBS.OMS.Interfaces.IOMSApp app)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return Store(item, source, tag, throwOnError, item.GetSettings(), app);
        }


        public StoreResults Store(IStorageItem item, System.IO.FileInfo source, object tag, bool throwOnError, StorageSettingsCollection settings)
        {
            return Store(item, source, tag, throwOnError, settings, null);
        }


        public StoreResults Store(IStorageItem item, System.IO.FileInfo source, object tag, bool throwOnError, StorageSettingsCollection settings, FWBS.OMS.Interfaces.IOMSApp app)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (source == null)
                throw new ArgumentNullException("source");

            IStorageItem original = item;

            //Refresh the file info object so that the exists property is up to date.
            source.Refresh();

            if (settings == null || settings.Count == 0)
                settings = GetDefaultStoreSettings(item);

            CheckFeature(item, StorageFeature.Storing);

            StorageManager.CurrentManager.ValidateFileExtension(source.Extension);

            try
            {
                BeforeStore(ref item, source, settings, tag);

                StoreResults results = InternalStore(item, source, tag, settings, app);

                if (results == null)
                {
                    var document = ((DocumentVersion)item).ParentDocument;
                    document.CancelOnFailedSave();

                }

                AfterStore(results, settings);

                original.ClearSettings();

                bool isnew = results.Item.IsNew;

                //The InternalStore has been ran successully so accept the document.
                results.Item.Accepted = true;

                results.Update();

                if (isnew)
                    results.Item.AddActivity("SAVED", "NEW");
                else
                    results.Item.AddActivity("SAVED", "EDITED");

                StorageManager.CurrentManager.LocalDocuments.Set(item, source, true);

                return results;
            }
            catch (StorageException)
            {
                if (throwOnError)
                    throw;
                else
                    return null;
            }
            catch (Exception ex)
            {
                if (throwOnError)
                    throw new StorageException("SPNICESTOREEX1",
@"There is a problem storing document '%1%'. The storage location %3% 
is attempting to access to store your document is currently unavailable.  You may 
wish to store your document on your local computer until the storage location becomes 
available and then re-save your document to %3%.  
Please inform your System Administrator." + Environment.NewLine + Environment.NewLine + "%2%", ex, item.DisplayID, ex.Message, FWBS.OMS.Global.ApplicationName);
                else
                    return null;
            }
        }

        protected virtual void BeforeStore(ref IStorageItem item, System.IO.FileInfo source, StorageSettingsCollection settings, object tag)
        {
            //Check for versioning support and apply it.
            IStorageItemVersionable original_versionable = item as IStorageItemVersionable;
            IStorageItemVersion original_version = item as IStorageItemVersion;
            IStorageItem original_item = item;

            //Check if versioning on the incoming storage item is supported first.
            if (Supports(StorageFeature.Versioning, item))
            {
                VersionStoreSettings versettings = GetSettings<VersionStoreSettings>(settings);

                if (original_versionable != null)
                {
                    //If the storage item is new then carry on, the versioning settings will NOT apply.
                    //This fixes the issue that MW had when defaulting to a new Major Version.
                    if (original_item.IsNew)
                    {
                        //Only construct a new version if the incoming item is not a verion derivative.
                        if (original_version == null)
                        {
                            item = original_versionable.CreateVersion();
                            item.Preview = original_item.Preview;

                            if (item is IStorageItemDuplication && original_item is IStorageItemDuplication)
                            {
                                ((IStorageItemDuplication)item).Checksum = ((IStorageItemDuplication)original_item).Checksum;
                            }

                            original_versionable.OnNewVersion(new NewVersionEventArgs(null, (IStorageItemVersion)item, source, tag));

                        }
                        else
                            item = original_item;
                    }
                    else
                    {
                        //If the item is being resaved then use the system setting.
                        switch (versettings.SaveItemAs)
                        {
                            case VersionStoreSettings.StoreAs.NewMajorVersion:
                                {
                                    //Always creates a new version of the document and sets it to be the current version.
                                    item = original_versionable.CreateVersion();
                                    item.Preview = original_item.Preview;

                                    if (item is IStorageItemDuplication && original_item is IStorageItemDuplication)
                                    {
                                        ((IStorageItemDuplication)item).Checksum = ((IStorageItemDuplication)original_item).Checksum;
                                    }

                                    original_versionable.OnNewVersion(new NewVersionEventArgs(original_version, (IStorageItemVersion)item, source, tag));

                                }
                                break;
                            case VersionStoreSettings.StoreAs.NewVersion:
                                {
                                    //Always creates a new version of the document and sets it to be the current version.
                                    item = original_versionable.CreateVersion(original_version);
                                    item.Preview = original_item.Preview;

                                    if (item is IStorageItemDuplication && original_item is IStorageItemDuplication)
                                    {
                                        ((IStorageItemDuplication)item).Checksum = ((IStorageItemDuplication)original_item).Checksum;
                                    }

                                    original_versionable.OnNewVersion(new NewVersionEventArgs(original_version, (IStorageItemVersion)item, source, tag));

                                }
                                break;
                            case VersionStoreSettings.StoreAs.NewSubVersion:
                                {
                                    //A new branched version will only set the current version of the document if the original
                                    item = original_versionable.CreateSubVersion(original_version);
                                    item.Preview = original_item.Preview;

                                    if (item is IStorageItemDuplication && original_item is IStorageItemDuplication)
                                    {
                                        ((IStorageItemDuplication)item).Checksum = ((IStorageItemDuplication)original_item).Checksum;
                                    }

                                    original_versionable.OnNewVersion(new NewVersionEventArgs(original_version, (IStorageItemVersion)item, source, tag));

                                }
                                break;
                            case VersionStoreSettings.StoreAs.OriginalOverwrite:
                                {
                                    item = original_item;
                                }
                                break;
                        }
                    }
                }
                
                
                IStorageItemVersion ver = item as IStorageItemVersion;
                if (ver != null)
                {
                    if (!QuickSaveUsedOnExistingDocument(original_item))
                    {
                        ver.Comments = versettings.Comments;
                        ver.Status = versettings.Status;
                    }
                }

            }
            else
            {
                if ((item is PrecedentVersion | item is Precedent) && !Session.CurrentSession.EnablePrecedentVersioning)
                {
                    //Versioning disabled for precedents if the precedent versioning switch is disabled
                }
                else
                {
                    //Make sure it is an object that allows versions. I.E not precedents
                    if (original_versionable != null)
                    {
                        //Make sure that when versioning is not supported always use the latest version
                        //object which should be automatically created.
                        item = original_versionable.GetLatestVersion();
                    }
                }
            }


            //Check of lockable support/
            IStorageItemLockable lockable = GetLockableItem(item);

            if (Supports(StorageFeature.Locking, item))
            {
                if (lockable != null)
                {
                    if (item.IsNew == false)
                    {
                        if (lockable.IsCheckedOutByAnother)
                        {
                            throw new StorageItemCheckedOutException(lockable.CheckedOutBy.FullName, lockable.CheckedOutTime.Value);
                        }
                    }
                }
            }


        }
        
        private bool QuickSaveUsedOnExistingDocument(IStorageItem original_item)
        {
            return (SaveMode == PrecSaveMode.Quick && !original_item.IsNew);
        }
        
        protected virtual void AfterStore(StoreResults results, StorageSettingsCollection settings)
        {
            //Check for versioning support and apply it.
            IStorageItem item = results.Item;
            IStorageItemVersionable original_versionable = item as IStorageItemVersionable;
            IStorageItemVersion original_version = item as IStorageItemVersion;
            IStorageItem original_item = item;

            if (Supports(StorageFeature.Versioning, item))
            {
                VersionStoreSettings versettings = GetSettings<VersionStoreSettings>(settings);


                if (versettings.MarkAsLatest)
                {

                    if (original_version == null && original_item.IsNew)
                    {
                        original_versionable.SetLatestVersion((IStorageItemVersion)item);
                    }
                    else
                    {

                        switch (versettings.SaveItemAs)
                        {
                            case VersionStoreSettings.StoreAs.NewMajorVersion:
                                {
                                    original_versionable.SetLatestVersion((IStorageItemVersion)item);
                                }
                                break;
                            case VersionStoreSettings.StoreAs.NewVersion:
                                {
                                    original_versionable.SetLatestVersion((IStorageItemVersion)item);
                                }
                                break;
                            case VersionStoreSettings.StoreAs.NewSubVersion:
                                {
                                    original_versionable.SetLatestVersion((IStorageItemVersion)item);
                                }
                                break;
                            case VersionStoreSettings.StoreAs.OriginalOverwrite:
                                {
                                    original_versionable.SetLatestVersion((IStorageItemVersion)item);
                                }
                                break;
                        }
                    }
                }

            }
            else
            {
                if ((item is PrecedentVersion | item is Precedent) && !Session.CurrentSession.EnablePrecedentVersioning)
                {
                    //Versioning disabled for precedents if the precedent versioning switch is disabled
                }
                else
                {
                    //Make sure it is an object that allows versions. I.E not precedents
                    if (original_versionable != null)
                    {
                        //If versioning is not supported still set the latest version of the main document
                        original_versionable.SetLatestVersion((IStorageItemVersion)item);
                    }
                }
            }

            //Check of lockable support/
            IStorageItemLockable lockable = GetLockableItem(item);

            if (Supports(StorageFeature.Locking, item))
            {
                if (lockable != null)
                {
                    LockableStoreSettings locksettings = GetSettings<LockableStoreSettings>(settings);
                    if (locksettings.CheckIn)
                    {
                        lockable.CheckIn();
                    }
                    else if (!lockable.IsCheckedOut)
                    {
                        if (results.LocalFile != null)
                            lockable.CheckOut(results.LocalFile);
                    }
                }
            }

        }

        public void CheckSetting(StoreResults results, FileInfo fileInfo)
        {
            IStorageItem item = results.Item;
            IStorageItemLockable lockable = GetLockableItem(item);

            if (Supports(StorageFeature.Locking, item) && lockable != null)
            {
                if (lockable.IsCheckedOut && fileInfo != null)
                {
                    lockable.UpdateCheckedOutLocation(fileInfo);
                }
            }
        }

        protected virtual StoreResults InternalStore(IStorageItem item, System.IO.FileInfo source, object tag, StorageSettingsCollection settings, FWBS.OMS.Interfaces.IOMSApp app)
        {
            return InternalStore(item, source, tag, settings);
        }

        protected abstract StoreResults InternalStore(IStorageItem item, System.IO.FileInfo source, object tag, StorageSettingsCollection settings);

        protected virtual StorageSettingsCollection GetDefaultStoreSettings(IStorageItem item)
        {
            StorageSettingsCollection settings = item.GetSettings();
            if (settings == null)
                settings = new StorageSettingsCollection();

            if (Supports(StorageFeature.Versioning, item))
            {
                VersionStoreSettings versettings = GetSettings<VersionStoreSettings>(settings);

                //Get the default versioning settings for a document.
                Document document = item as Document;
                IStorageItemVersion version = item as IStorageItemVersion;
                IStorageItemVersionable versionable = item as IStorageItemVersionable;

                Precedent prec = item as Precedent;
                if (prec != null)
                {
                    version = item as IStorageItemVersion;
                    versionable = item as IStorageItemVersionable;

                    string val = String.Empty;

                    // just assign these to see what happen
                    Precedent lastprec = prec;
                    Precedent baseprec = prec;

                    versettings.CanOverwrite = false;

                    if (lastprec != null)
                        val = lastprec.DocumentVersioning;

                    if (val == String.Empty)
                    {
                        if (baseprec != null)
                            val = baseprec.DocumentVersioning;
                    }

                    versettings.CanOverwrite = true;

                    switch (val)
                    {
                        case "O":
                            {
                                versettings.SaveItemAs = VersionStoreSettings.StoreAs.OriginalOverwrite;
                            }
                            break;
                        case "N":
                            {
                                versettings.SaveItemAs = VersionStoreSettings.StoreAs.NewVersion;
                            }
                            break;
                        case "M":
                            {
                                versettings.SaveItemAs = VersionStoreSettings.StoreAs.NewMajorVersion;
                            }
                            break;
                        case "V":
                            {
                                versettings.SaveItemAs = VersionStoreSettings.StoreAs.NewSubVersion;
                            }
                            break;
                        default:
                            goto case "O";

                    }
                    
                    //Set the MarkAsLatest setting based on whether the current working document is the latest version already.
                    if (version == null)
                    {
                        if (versionable != null)
                        {
                            version = versionable.GetWorkingVersion();
                        }
                    }

                    if (version != null)
                    {
                        versettings.MarkAsLatest = version.IsLatestVersion;
                        versettings.Status = version.Status;
                        if (versettings.SaveItemAs == VersionStoreSettings.StoreAs.OriginalOverwrite)
                            versettings.Comments = version.Comments;
                    }
                }
               
               
                if (document == null && prec == null)
                {
                    DocumentVersion vers = item as DocumentVersion;
                    if (vers != null)
                        document = vers.ParentDocument;
                }


                if (document != null)
                {
                    string val = String.Empty;

                    Precedent lastprec = document.LastPrecedent;
                    Precedent baseprec = document.BasePrecedent;

                    versettings.CanOverwrite = false;

                    if (lastprec != null)
                        val = lastprec.DocumentVersioning;

                    if (val == String.Empty)
                    {
                        if (baseprec != null)
                            val = baseprec.DocumentVersioning;
                    }

                    if (val == String.Empty)
                        val = document.OMSFile.CurrentFileType.DocumentVersioning;

                    if (val == String.Empty)
                    {
                        val = Session.CurrentSession.DocumentVersioning;
                        versettings.CanOverwrite = true;
                    }

                    switch (val)
                    {
                        case "O":
                            {
                                versettings.SaveItemAs = VersionStoreSettings.StoreAs.OriginalOverwrite;
                            }
                            break;
                        case "N":
                            {
                                versettings.SaveItemAs = VersionStoreSettings.StoreAs.NewVersion;
                            }
                            break;
                        case "M":
                            {
                                versettings.SaveItemAs = VersionStoreSettings.StoreAs.NewMajorVersion;
                            }
                            break;
                        case "V":
                            {
                                versettings.SaveItemAs = VersionStoreSettings.StoreAs.NewSubVersion;
                            }
                            break;
                        default:
                            goto case "O";

                    }
                }

                //Set the MarkAsLatest setting based on whether the current working document is the latest version already.
                if (version == null)
                {
                    if (versionable != null)
                    {
                        version = versionable.GetWorkingVersion();
                    }
                }

                if (version != null)
                {
                    versettings.MarkAsLatest = version.IsLatestVersion;
                    versettings.Status = version.Status;
                    if (versettings.SaveItemAs == VersionStoreSettings.StoreAs.OriginalOverwrite)
                        versettings.Comments = version.Comments;
                }

            }

            IStorageItemLockable lockable = GetLockableItem(item);

            if (Supports(StorageFeature.Locking, item))
            {
                if (item.IsNew)
                {
                    if (CheckOut(item, true))
                    {
                        LockableStoreSettings locksettings = GetSettings<LockableStoreSettings>(settings);
                        locksettings.CheckIn = false;
                    }

                }
                else
                {
                    LockableStoreSettings locksettings = GetSettings<LockableStoreSettings>(settings);

                    if (lockable != null)
                    {
                        User checkedoutby = lockable.CheckedOutBy;
                        if (lockable.IsCheckedOut)
                        {
                            OMSDocument document = item as OMSDocument;
                            DocumentVersion vers = item as DocumentVersion;

                            if (vers != null)
                                document = vers.ParentDocument;

                            bool keepCheckedout = true;
                            if (document != null)
                                keepCheckedout = document.ContinueAfterSave;

                            if (checkedoutby != null && checkedoutby.ID == Session.CurrentSession.CurrentUser.ID && !keepCheckedout)
                                locksettings.CheckIn = true;
                            else
                                locksettings.CheckIn = false;
                        }
                        else
                            locksettings.CheckIn = true;
                    }
                    else
                        locksettings.CheckIn = false;
                }
            }

            return settings;
        }

        /// <summary>
        /// checks to see if with exclusive locking turned on if the document should be checked out
        /// </summary>
        /// <param name="item"></param>
        /// <param name="considerContinueAfterSave"></param>
        /// <returns></returns>
        private static bool CheckOut(IStorageItem item, bool considerContinueAfterSave)
        {
            //if exclusive locking
            OMSDocument doc = item as OMSDocument;
            if (doc == null)
            {
                DocumentVersion vers = item as DocumentVersion;
                if (vers == null)
                    return false;

                doc = vers.ParentDocument;
            }
            

            if (considerContinueAfterSave && !doc.ContinueAfterSave)
                return false;

            FileType type = new FileType(doc.OMSFile.FileTypeCode);
            bool checkOut = false;
            switch (type.DocumentLocking)
            {
                case "":
                    checkOut = Session.CurrentSession.DocumentLocking == "E";
                    break;
                case "E":
                    checkOut = true;
                    break;
            }
            return checkOut;
        }

        #endregion

        #region Purge

        public void Purge(IStorageItem item)
        {
            Purge(item, true);
        }

        public void Purge(IStorageItem item, bool throwOnError)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            Purge(item, throwOnError, item.GetSettings());
        }

        public void Purge(IStorageItem item, bool throwOnError, StorageSettingsCollection settings)
        {
            if (item == null)
                throw new ArgumentNullException("item");


            IStorageItem original = item;

            if (settings == null || settings.Count == 0)
                settings = GetDefaultPurgeSettings(item);

            CheckFeature(item, StorageFeature.Purging);

            try
            {
                BeforePurge(ref item, settings);

                InternalPurge(item, settings);

                AfterPurge(item, settings);

                original.ClearSettings();

                item.AddActivity("PURGED", null);

            }
            catch (StorageException)
            {
                if (throwOnError)
                    throw;
            }
            catch (Exception ex)
            {
                if (throwOnError)
                    throw new StorageException("SPPURGEEX", "There was a problem purging specified item '%1%'.", ex, item.DisplayID);
            }
        }

        protected virtual void BeforePurge(ref IStorageItem item, StorageSettingsCollection settings)
        {
        }

        protected virtual void AfterPurge(IStorageItem item, StorageSettingsCollection settings)
        {
        }

        protected virtual void InternalPurge(IStorageItem item, StorageSettingsCollection settings)
        {
            throw GetNotImplementedFeatureException(StorageFeature.Purging);
        }


        protected virtual StorageSettingsCollection GetDefaultPurgeSettings(IStorageItem item)
        {
            StorageSettingsCollection settings = item.GetSettings();
            if (settings == null)
                return new StorageSettingsCollection();
            else
                return settings;
        }

        #endregion

        #region Feature Checking

        public void CheckFeature(IStorageItem item, StorageFeature feature)
        {
            if (!Supports(feature, item))
                throw GetUnsupportedFeatureException(feature);
        }

        protected virtual bool SupportsBuiltinFeature(StorageFeature feature)
        {
            switch (feature)
            {
                case StorageFeature.Retrieving:
                    return true;
                case StorageFeature.Storing:
                    return true;
                default:
                    return false;
            }
        }

        protected virtual bool HasCustomFeatureImplementation(StorageFeature feature)
        {
            return false;
        }

        public bool Supports(StorageFeature feature)
        {
            if (StorageManager.CurrentManager.IsFeatureImplemented(feature))
            {
                if (SupportsBuiltinFeature(feature))
                    return true;

                if (HasCustomFeatureImplementation(feature))
                    return true;
            }

            return false;
        }

        public bool Supports(StorageFeature feature, IStorageItem item)
        {
            if (item == null)
            {
                if (!Supports(feature))
                    return false;
            }
            else
            {

                if (!item.Supports(feature))
                    return false;

                if (!Supports(feature))
                    return false;
            }
            return true;
        }

        public virtual SupportedStorageFeatures Supports()
        {
            return (Supports((IStorageItem)null));

        }

        public virtual SupportedStorageFeatures Supports(IStorageItem item)
        {
            SupportedStorageFeatures supportedFeatures = new SupportedStorageFeatures();

            supportedFeatures.AllowOverwrite = Supports(StorageFeature.AllowOverwrite, item);
            supportedFeatures.CreateSubVersion = Supports(StorageFeature.CreateSubVersion, item);
            supportedFeatures.CreateVersion = Supports(StorageFeature.CreateVersion, item);
            supportedFeatures.DuplicateChecking = Supports(StorageFeature.DuplicateChecking, item);
            supportedFeatures.Locking = Supports(StorageFeature.Locking, item);
            supportedFeatures.Purging = Supports(StorageFeature.Purging, item);
            supportedFeatures.Register = Supports(StorageFeature.Register, item);
            supportedFeatures.Retrieving = Supports(StorageFeature.Retrieving, item);
            supportedFeatures.Storing = Supports(StorageFeature.Storing, item);
            supportedFeatures.Versioning = Supports(StorageFeature.Versioning, item);

            return supportedFeatures;

        }

        #endregion

        #region Methods

        public virtual bool IsConnected(IStorageItem item)
        {
            if (service == null)
                return Session.CurrentSession.IsLoggedIn;

            if (item is Precedent)
            {
                if (service.PrecedentService == null)
                    return service.IsConnected;
                else
                    return service.PrecedentService.IsConnected;
            }
            else if (item is Document)
            {
                if (service.DocumentService == null)
                    return service.IsConnected;
                else
                    return service.DocumentService.IsConnected;
            }
            else if (item is DocumentVersion)
            {
                if (service.DocumentService == null)
                    return service.IsConnected;
                else
                    return service.DocumentService.IsConnected;
            }
            else
                return false;
        }


        public void InitialiseService()
        {
            service = CreateService();

            if (service != null)
            {
                Connectivity.ConnectivityManager.CurrentManager.Add(service);
            }
        }

        public virtual IStorageItemLockable GetLockableItem(IStorageItem item)
        {
            return item as IStorageItemLockable;
        }

        protected abstract StorageProviderService CreateService();

        public StorageSettingsCollection GetDefaultSettings(IStorageItem item, SettingsType type)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            StorageSettingsCollection settings = null;

            switch (type)
            {
                case SettingsType.Store:
                    settings = GetDefaultStoreSettings(item);
                    break;
                case SettingsType.Fetch:
                    settings = GetDefaultFetchSettings(item);
                    break;
                case SettingsType.Purge:
                    settings = GetDefaultPurgeSettings(item);
                    break;
            }

            if (settings == null)
                settings = new StorageSettingsCollection();

            return settings;
        }

        protected TSettings GetSettings<TSettings>(StorageSettingsCollection settings)
            where TSettings : StorageSettings
        {
            return settings.GetSettings<TSettings>();
        }


        protected virtual StorageException GetMissingStorageItemException(IStorageItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            return new StorageException("S_ITEM_MISSING", "The specified item '%1%' (%2%) does not exist within provider '%3%'.", null, item.Name, item.DisplayID, this.Name);
        }

        protected virtual NotSupportedException GetUnsupportedStorageItemException(IStorageItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            
            return new NotSupportedException(Session.CurrentSession.Resources.GetMessage("STRGTMNTSPP", "Storage item type ''%1%''is not supported by provider type ''%2%''", "", item.GetType().Name, Name).Text);
        }

        protected NotSupportedException GetUnsupportedFeatureException(StorageFeature feature)
        {
            throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("PRVTPDSNTSPFTR", "The provider type ''%1%'' does not support feature ''%2%''", "", Name, feature.ToString()).Text);
        }

        protected NotSupportedException GetNotImplementedFeatureException(StorageFeature feature)
        {
            throw new NotImplementedException(Session.CurrentSession.Resources.GetMessage("PRVTPSPBTNIMFTR", "The provider type ''%1%'' supports but has not implemented feature ''%2%''", "", Name, feature.ToString()).Text);
        }

        protected NotSupportedException GetUnsupportedFeatureException(IStorageItem item, StorageFeature feature)
        {
            throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("STRGTMDNSPFT", "The storage item type ''%1%'' does not support feature ''%2%''", "", item.GetType().Name, feature.ToString()).Text);
        }

        protected abstract string GenerateToken(IStorageItem item);


        public virtual System.IO.FileInfo GetLocalFile(IStorageItem item)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            System.IO.FileInfo local = item.GetIdealLocalFile();
            return GetLocalFile(local);

        }

        public System.IO.FileInfo GetLocalFile(System.IO.FileInfo localFile)
        {

            System.IO.FileInfo file = localFile;

            int ctr = 0;

            while (System.IO.File.Exists(file.FullName))
            {
                try
                {
                    if (file.IsReadOnly)
                        file.Attributes -= System.IO.FileAttributes.ReadOnly;
                    file.Delete();
                }
                catch (System.IO.IOException)
                {
                    ctr++;
                    string alternative = System.IO.Path.Combine(localFile.DirectoryName, String.Format("{0}-{1}{2}", System.IO.Path.GetFileNameWithoutExtension(localFile.FullName), ctr, localFile.Extension));
                    file = new System.IO.FileInfo(alternative);
                }
                catch (UnauthorizedAccessException)
                {
                    ctr++;
                    string alternative = System.IO.Path.Combine(localFile.DirectoryName, String.Format("{0}-{1}{2}", System.IO.Path.GetFileNameWithoutExtension(localFile.FullName), ctr, localFile.Extension));
                    file = new System.IO.FileInfo(alternative);
                }

            }

            if (file.Directory.Exists == false)
                file.Directory.Create();

            return file;
        }


        public override string ToString()
        {
            return Name;
        }

        public virtual void ValidateStoreSettings(IStorageItem item)
        {
            StorageSettingsCollection settings = item.GetSettings();

            if (settings == null)
                settings = this.GetDefaultSettings(item, SettingsType.Store);

            settings.ValidateSettings(item.IsNew);
        }

        #endregion
    }
}
