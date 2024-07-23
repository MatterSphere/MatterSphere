namespace FWBS.OMS.HighQ.Models
{
    internal class DocumentDetails
    {
        public DocumentDetails(long docId, string path, string description, int hqFolderId)
        {
            DocumentId = docId;
            Path = path;
            Description = description;
            HQFolderId = hqFolderId;
        }

        public long DocumentId { get; }
        public string Path { get; }
        public string Description { get; }
        public int HQFolderId { get; }
    }
}
