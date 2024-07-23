using System;
namespace FWBS.OMS.DocumentManagement.Storage
{
    public interface ILocalDocumentCache
    {
        System.Data.DataTable GetLocalDocumentInfo();
        System.IO.FileInfo GetLocalFile(IStorageItem item, out DateTime? cachedDate);
        System.Data.DataTable GetLocalPrecedentInfo();
        bool HasChanged(System.IO.FileInfo file);
        bool HasChanged(IStorageItem item);
        bool IsCheckedOut(System.IO.FileInfo file);
        bool IsCheckedOut(IStorageItem item);
        bool IsDifferentToServer(IStorageItem item);

        System.IO.DirectoryInfo LocalDocumentDirectory { get; }
        System.IO.DirectoryInfo LocalPrecedentDirectory { get; }
        void Remove(System.IO.FileInfo file);
        void Save();
        void Set(IStorageItem item, System.IO.FileInfo localFile, bool force);
    }
}
