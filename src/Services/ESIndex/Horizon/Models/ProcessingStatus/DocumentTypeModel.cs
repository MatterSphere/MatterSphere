using Horizon.Common.Models.Repositories.ProcessingStatus;

namespace Horizon.Models.ProcessingStatus
{
    public class DocumentTypeModel
    {
        public DocumentTypeModel(DocumentTypeItem item)
        {
            Extension = item.Extension;
            Count = item.Count;
        }

        public string Extension { get; set; }
        public int Count { get; set; }
    }
}
