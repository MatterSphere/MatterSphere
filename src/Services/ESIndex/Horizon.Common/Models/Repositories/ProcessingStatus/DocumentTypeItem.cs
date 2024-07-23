namespace Horizon.Common.Models.Repositories.ProcessingStatus
{
    public class DocumentTypeItem
    {
        public DocumentTypeItem(string extension, int count)
        {
            Extension = extension;
            Count = count;
        }

        public string Extension { get; }
        public int Count { get; }
    }
}
