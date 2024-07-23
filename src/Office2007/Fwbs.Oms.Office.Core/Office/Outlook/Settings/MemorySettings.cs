namespace Fwbs.Office.Outlook
{
    public class MemorySettings
    {
        public bool AutoGarbageCollection { get; internal set; }

        public int MultipleItemChunkSize { get; internal set; }

        public bool NewMailEventEnabled { get; internal set; }

        public bool MultipleItemWarningEnabled { get; internal set; }

        public int MultipleItemWarningSize { get; internal set; }

        public string MultipleItemWarningMessage { get; internal set; }

        public MemorySettings Clone()
        {
            return (MemorySettings)MemberwiseClone();
        }
    }
}
