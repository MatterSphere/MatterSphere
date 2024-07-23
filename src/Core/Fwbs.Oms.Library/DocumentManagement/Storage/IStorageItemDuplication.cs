namespace FWBS.OMS.DocumentManagement.Storage
{
    public interface IStorageItemDuplication : IStorageItem
    {
        IStorageItemDuplication CheckForDuplicate();
        void GenerateChecksum(string value);

        bool AllowDuplication { get;set;}
        string Checksum { get;set;}


    }
}
