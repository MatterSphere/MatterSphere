namespace MSIndex.Common.Interfaces
{
    public interface IFileStorageProvider
    {
        void SaveData(byte[] message);
        byte[] ReadData();
        void ClearCache();
    }
}
