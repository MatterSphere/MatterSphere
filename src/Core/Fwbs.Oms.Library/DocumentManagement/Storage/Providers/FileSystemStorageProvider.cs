using System;
using System.Diagnostics;
using System.IO;
using Document = FWBS.OMS.OMSDocument;

namespace FWBS.OMS.DocumentManagement.Storage.Providers
{
    public sealed class FileSystemStorageProvider : StorageProvider
    {
        #region Store

        protected override StoreResults InternalStore(IStorageItem item, FileInfo source, object tag, StorageSettingsCollection settings)
        {
            IStorageItemProvider provider = GetProvider(item);

            string token;

            FileInfo destination = provider is IStorageItemProvider2 
                ? ((IStorageItemProvider2)provider).GeneratePath(item, source, out token) 
                : provider.GeneratePath(item, out token);

            try
            {
                if (!destination.Directory.Exists)
                    destination.Directory.Create();

                source.CopyTo(destination.FullName, true);

                return new StoreResults(item, token, source);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private IStorageItemProvider GetProvider(IStorageItem item)
        {
            if (item is Document)
            {
                return new DocumentProvider();
            }
            else if (item is Precedent)
            {
                return new PrecedentProvider();
            }
            else if (item is DocumentVersion)
            {
                return new DocumentVersionProvider();
            }
            else if (item is PrecedentVersion)
            { 
                return new PrecedentVersionProvider();
            }
            throw GetUnsupportedStorageItemException(item);
        }

        protected override string GenerateToken(IStorageItem item)
        {
            IStorageItemProvider provider = GetProvider(item);
            return provider.GenerateToken(item);
        }

        #endregion

        #region Fetch

        protected override FetchResults InternalFetch(IStorageItem item, StorageSettingsCollection settings)
        {
            try
            {
				//Check if the path locates the file directly.
                if (String.IsNullOrEmpty(item.Token) == false && System.IO.File.Exists(item.Token) == false)
                {
                    //Get the store items virtual path.
                    System.IO.FileInfo file = new System.IO.FileInfo(item.Token);

                    var provider = GetProvider(item);
                    file = provider.FetchStorageItem(item);

                    if (file == null || file.Exists == false)
                        return null;

                    FileInfo local = GetLocalFile(item);

                    Debug.WriteLine($"{this.GetType().Name} / InternalFetch");
                    Debug.WriteLine($"File.Copy({file.FullName}, {local.FullName});");

                    File.Copy(file.FullName, local.FullName);

                    return new FetchResults(item, local);
                }

                if (File.Exists(item.Token)) 
                {
                    System.IO.FileInfo file = new System.IO.FileInfo(item.Token);

                    System.IO.FileInfo local = GetLocalFile(item);

                    System.IO.File.Copy(file.FullName, local.FullName);

                    return new FetchResults(item,local);
                }
                return null;
            }
			catch
			{
				throw;
			}
        }

        #endregion

        #region Purge

        protected override void InternalPurge(IStorageItem item, StorageSettingsCollection settings)
        {
            var provider = GetProvider(item);
            FileInfo file = provider.FetchStorageItem(item);
            if (file != null && file.Exists)
                file.Delete();
        }

        #endregion

        #region Methods

        protected override StorageProviderService CreateService()
        {
            return new FileSystemStorageProviderService();
        }

        protected override bool SupportsBuiltinFeature(StorageFeature feature)
        {
            switch (feature)
            {
                case StorageFeature.Versioning:
                case StorageFeature.AllowOverwrite:
                case StorageFeature.CreateSubVersion:
                case StorageFeature.CreateVersion:
                    return true;
                case StorageFeature.Locking:
                    return true;
            }

            return base.SupportsBuiltinFeature(feature);
        }

        #endregion
    }
}
