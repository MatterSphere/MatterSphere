namespace FWBS.OMS.DocumentManagement.Storage
{
    public interface IStorageItem
    {
        string Name { get;}
        string Pointer { get;}
        string DisplayID { get;}
        string Extension { get;}
        string Token { get;set;}
        string Preview { get;set;}
        bool Accepted { get;set;}

        System.Drawing.Icon GetIcon();

        void ChangeStorage(StorageProvider provider, bool transfer);
        StorageProvider GetStorageProvider();
        void ApplySettings(StorageSettingsCollection settings);
        StorageSettingsCollection GetSettings();
        void ClearSettings();

        IStorageItemType GetItemType();
        IStorageItem GetConflict();

        System.IO.FileInfo GetIdealLocalFile();
        
        bool Supports(StorageFeature feature);

        bool IsConflicting { get;}
        bool IsNew { get;}
        bool IsDirty { get;}

        void Update();

        void AddActivity(string type, string code);
        void AddActivity(string type, string code, string data);
        System.Data.DataTable GetActivities();
    }
}
