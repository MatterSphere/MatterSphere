using System.IO;


namespace MSCacheMgr
{
    class CacheFolderTagData
    {
        public string CacheDirectoryPath { get; set; }
        public string ConnectionText { get; set; }
    }


    class ListViewItemTagData
    {
        public DirectoryInfo CacheDirectory { get; set; }
        public string FileDirectoryPath { get; set; }
        public bool Deleted { get; set; }
    }
}
