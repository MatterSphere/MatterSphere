namespace FWBS.OMS.DocumentManagement.Storage
{
    public interface IStorageItemType
    {
        string Code { get;}
        string Extension { get;}
        StorageProvider GetDefaultStorageProvider();
        bool Supports(StorageFeature feature);
    }
}
