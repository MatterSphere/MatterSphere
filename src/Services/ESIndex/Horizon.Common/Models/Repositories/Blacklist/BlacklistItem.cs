namespace Horizon.Common.Models.Repositories.Blacklist
{
    public class BlacklistItem
    {
        public BlacklistItem(string extension)
        {
            Extension = extension.ToLower().Trim();
        }

        public BlacklistItem(string extension, string metadata, string encoding, long? maxSize = null) : this(extension)
        {
            Metadata = metadata != null ? metadata.Trim() : string.Empty;
            Encoding = encoding != null ? encoding.Trim() : string.Empty;
            MaxSize = maxSize == 0 ? null : maxSize;
        }

        public string Extension { get; private set; }
        public string Metadata { get; private set; }
        public string Encoding { get; private set; }
        public long? MaxSize { get; private set; }

        public bool FullExtension
        {
            get
            {
                return string.IsNullOrWhiteSpace(Metadata)
                       && string.IsNullOrWhiteSpace(Encoding)
                       && MaxSize == null;
            }
        }
    }
}
