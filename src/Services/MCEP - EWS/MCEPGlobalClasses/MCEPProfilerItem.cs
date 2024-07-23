using System;

namespace MCEPGlobalClasses
{
    class MCEPProfilerItem
    {
        public string UserEmail { get; set; }
        public double UserID  { get; set; }
        public string FolderID  { get; set; }
        public double FileID  { get; set; }
        public bool Processed  { get; set; }
        public DateTime ItemCreated  { get; set; }
        public DateTime ItemUpdated { get; set; }
        public string MessageID  { get; set; }
    }
}
