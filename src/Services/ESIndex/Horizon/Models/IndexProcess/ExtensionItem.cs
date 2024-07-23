namespace Horizon.Models.IndexProcess
{
    public class ExtensionItem
    {
        public ExtensionItem(string extension, string metadata, string encoding, long? maxSize)
        {
            Extension = extension.ToLower().Trim();
            Metadata = metadata != null ? metadata.Trim() : string.Empty;
            Encoding = encoding != null ? encoding.Trim() : string.Empty;
            MaxSize = maxSize == 0 ? null : maxSize;
        }

        public string Extension { get; set; }
        public string Metadata { get; set; }
        public string Encoding { get; set; }
        public long? MaxSize { get; set; }
    }
}
