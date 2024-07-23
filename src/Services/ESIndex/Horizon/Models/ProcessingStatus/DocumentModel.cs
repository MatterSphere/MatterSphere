using Horizon.Common.Models.Repositories.ProcessingStatus;

namespace Horizon.Models.ProcessingStatus
{
    public class DocumentModel
    {
        public DocumentModel(DocumentItem item, int number)
        {
            Id = Id;
            Number = number;
            Name = item.Name;
            Path = item.Path;
            Size = item.Size;
            Error = item.Error;
        }

        public long Id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public long Size { get; set; }
        public string Error { get; set; }
    }
}
