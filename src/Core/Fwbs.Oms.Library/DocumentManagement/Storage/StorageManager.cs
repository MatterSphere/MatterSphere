using System;
using System.Collections.Generic;
using System.Text;

namespace FWBS.OMS.DocumentManagement.Storage
{
    /// <summary>
    /// A storage manager which controls all of the persting and retrieval of documents
    /// using registered document comparers, mergers and storagte providers.
    /// </summary>
    public sealed class StorageManager : IDisposable, IService
    {
        #region Fields

        private static StorageManager currentManager;

        private System.Data.DataTable providers;
        private Dictionary<short, StorageProvider> loadedproviders;
        private Dictionary<short, Exception> providerexceptions = new Dictionary<short,Exception>();

        private bool loaded = false;
  
        #endregion

        #region Constructors

        private StorageManager()
        {
        }

        #endregion

        #region Properties

        [Obsolete("Please use the LocalDocuments property")]
        public static System.Data.DataTable GetLocalDocuments()
        {
            return CurrentManager.LocalDocuments.GetLocalDocumentInfo();
        }

        public static System.Data.DataTable GetLocalDocuments(bool checkedOut, bool changed, object documentType)
        {
            
            System.Data.DataTable localDocuments = CurrentManager.LocalDocuments.GetLocalDocumentInfo();
            List<string> filterProps = new List<string>();
            
            if (changed)
                filterProps.Add("HasChanged");

            if (checkedOut)
                filterProps.Add("Len([docCheckedOutByName]) > 0");

            if (!(documentType is DBNull) && documentType != null)
                filterProps.Add(string.Format("doctype = '{0}'", documentType));

            StringBuilder filter = new StringBuilder();
            bool first = true;

            foreach (var property in filterProps)
            {
                if (first)
                    first = false;
                else
                    filter.Append(" and ");

                filter.Append(property);

                
            }

            System.Data.DataView localView = localDocuments.DefaultView;
            localView.RowFilter = filter.ToString();

            return localView.ToTable();
        }


        [Obsolete("Please use the LocalDocuments property")]
        public static System.Data.DataTable GetLocalPrecedents()
        {
            return CurrentManager.LocalDocuments.GetLocalPrecedentInfo();
        }

        public static StorageManager CurrentManager
        {
            get
            {
                if (currentManager == null)
                {
                    currentManager = new StorageManager();
                }
                return currentManager;
            }
        }

   
        public StorageProvider GetStorageProvider(short id)
        {
            if (loadedproviders == null)
                loadedproviders = new Dictionary<short, StorageProvider>();

            //Strange StorageProvider '0' does not exist.
            //Will rebuild the provider list if not exists and then check again.
            if (loadedproviders.ContainsKey(id) == false)
            {
                InternalLoad();
                loaded = true;

                if (providerexceptions.ContainsKey(id))
                {
                    throw new StorageLocationProviderItemNotInstalledException(providerexceptions[id], id.ToString());
                }

                if (loadedproviders.ContainsKey(id) == false)
                {
                    throw new StorageLocationProviderItemNotInstalledException(id.ToString());
                }

    
            }

            return loadedproviders[id];
            
        }

        public System.Data.DataTable GetRegisteredProviders()
        {
            if (providers == null)
            {
                System.Data.IDataParameter [] pars = new System.Data.IDataParameter[1];
                pars[0] = Session.CurrentSession.Connection.CreateParameter("UI", System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
                providers = Session.CurrentSession.Connection.ExecuteSQLTable("select spid, dbo.GetCodeLookupDesc('SLP', spcode, @UI) as spdesc, sptype2 from dbstorageprovider", "PROVIDERS", pars);
            }
            return providers.Copy();
        }


        public bool IsInstalled<T>() where T : StorageProvider
        {
            if (loadedproviders == null)
                loadedproviders = new Dictionary<short, StorageProvider>();

            foreach (StorageProvider prov in loadedproviders.Values)
            {
                if (prov.GetType() == typeof(T))
                    return true;
            }

            return false;
        }

        public bool IsFeatureImplemented(StorageFeature feature)
        {
            switch (feature)
            {
                case StorageFeature.Retrieving:
                    return true;
                case StorageFeature.Storing:
                    return true;
                case StorageFeature.Locking:
                    return true;
                case StorageFeature.Versioning:
                    return true;
                case StorageFeature.DuplicateChecking:
                    return true;
                case StorageFeature.Register:
                    return true;
                case StorageFeature.AllowOverwrite:
                case StorageFeature.CreateSubVersion:
                case StorageFeature.CreateVersion:
                    return true;
                default:
                    return false;
            }
        }

        private ILocalDocumentCache cache;
        public ILocalDocumentCache LocalDocuments
        {
            get
            {
                if (cache == null)
                    cache = new LocalDocumentCache();

                return cache;
            }
        }

        public void SetCachingMechanism(ILocalDocumentCache cache)
        {
            this.cache = cache;
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Unload();

                var cachedisp = cache as IDisposable;
                if (cachedisp != null)
                {
                    cachedisp.Dispose();
                    cache = null;
                }
            }



            loaded = false;
        }

        #endregion

        #region IService

        public bool IsLoaded
        {
            get
            {
                return loaded;
            }
        }

        public void Load()
        {
            if (!loaded && Session.CurrentSession.IsLoggedIn)
            {
                Session.CurrentSession.CheckLoggedIn();

                InternalLoad();

                loaded = true;
            }          
        }

        private void InternalLoad()
        {
            System.Data.DataTable dt = GetRegisteredProviders();

            if (loadedproviders == null)
                loadedproviders = new Dictionary<short, StorageProvider>();
            else
                loadedproviders.Clear();

            providerexceptions.Clear();

            foreach (System.Data.DataRow r in dt.Rows)
            {
                string spt = Convert.ToString(r["sptype2"]);
                //This still seems to be throwing an error if it finds the storage provider
                //but that storage provider has dependency issues.
                Type t = null;

                try
                {
                    t = Session.CurrentSession.TypeManager.Load(spt);
                    if (t != null)
                    {
                        StorageProvider provider = Session.CurrentSession.TypeManager.Create(t) as StorageProvider;
                        if (provider != null)
                        {
                            provider.Id = Convert.ToInt16(r["spid"]);
                            loadedproviders.Add(provider.Id, provider);
                            provider.InitialiseService();
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(spt))
                                //TEMP: Temporary code to help resolving the random Storage Provider '0' is not installed error.
                                System.Diagnostics.Trace.WriteLine(String.Format(@"Storage Provider Type Cannot Be Constructed: '{0}':", spt), "STORAGE PROVIDER MANAGER");
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(spt))
                            //TEMP: Temporary code to help resolving the random Storage Provider '0' is not installed error.
                            System.Diagnostics.Trace.WriteLine(String.Format(@"Storage Provider Type Cannot Be Found: '{0}':", spt), "STORAGE PROVIDER MANAGER");
                    }
                }
                catch (Exception ex)
                {
                    t = null;
                    providerexceptions.Add(Convert.ToInt16(r["spid"]), ex);
                }
            }
        }


        /// <summary>
        /// Clears any cached information.
        /// </summary>
        public void Unload()
        {
            try
            {

                if (loaded)
                {
                    if (cache != null)
                        cache.Save();

                    if (providers != null)
                    {
                        providers.Dispose();
                        providers = null;
                    }

                    if (loadedproviders != null)
                    {
                        foreach (StorageProvider m in loadedproviders.Values)
                        {
                            IDisposable disp = m as IDisposable;
                            if (disp != null)
                                disp.Dispose();
                        }
                        loadedproviders.Clear();
                        providerexceptions.Clear();

                        //TEMP: Temporary code to help resolving the random Storage Provider '0' is not installed error.
                        System.Diagnostics.Trace.WriteLine(@"Storage Provider Manager Unloaded", "STORAGE PROVIDER MANAGER");
 
                    }

                    currentManager = null;
                }
            }
            finally
            {
                loaded = false;
            }
        }


        public bool IsValidFileExtension(string file)
        {
            if (file == null)
                return false;
            string extension = System.IO.Path.GetExtension(file);
            if (extension == null)
                extension = String.Empty;

            extension = extension.ToUpper();

            string[] illegalextensions = Session.CurrentSession.IllegalFileExtensions.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
            if (extension.StartsWith("."))
                extension = extension.Substring(1);

            if (Array.IndexOf<string>(illegalextensions, extension) > -1)
                return false;
            else
                return true;
        }

        public void ValidateFileExtension(string file)
        {
            if (!IsValidFileExtension(file))
            {
                string extension = System.IO.Path.GetExtension(file);
                if (extension == null)
                    extension = String.Empty;
                throw new IllegalStorageItemException(extension);
            }
        }

        #endregion


    }
}
