using System.IO;

namespace FWBS.OMS.DocumentManagement.Storage
{
    public interface IStorageItemProvider
    {
        FileInfo GeneratePath(IStorageItem item, out string token);
        string GenerateToken(IStorageItem item);
        FileInfo FetchStorageItem(IStorageItem item);
    }

    public interface IStorageItemProvider2: IStorageItemProvider
    {
        FileInfo GeneratePath(IStorageItem item, FileInfo source, out string token);
    }
}