namespace FWBS.OMS.DocumentManagement.Storage
{
    public class SupportedStorageFeatures
    {
        public bool Retrieving{get;set;}
        public bool Storing { get; set; }
        public bool Purging { get; set; }
        public bool AllowOverwrite { get; set; }
        public bool Versioning { get; set; }
        public bool CreateVersion { get; set; }
        public bool CreateSubVersion { get; set; }
        public bool Locking { get; set; }
        public bool DuplicateChecking { get; set; }
        public bool Register { get; set; }
    }
}
