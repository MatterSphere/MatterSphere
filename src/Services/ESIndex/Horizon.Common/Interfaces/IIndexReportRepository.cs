using System.Collections.Generic;
using Horizon.Common.Models.Common;
using Horizon.Common.Models.Repositories.IndexReport;

namespace Horizon.Common.Interfaces
{
    public interface IIndexReportRepository
    {
        IEnumerable<IReportItem> GetDocumentBuckets(ContentableEntityTypeEnum contentableEntityType);
        IEnumerable<DocumentErrorBucket> GetDocumentErrorBuckets(string extension, ContentableEntityTypeEnum entityType);
        IEnumerable<DocumentError> GetDocumentErrors(string extension, string errorCode, int page, int pageSize, ContentableEntityTypeEnum entityType);
        
        IEnumerable<EntityProcessItem> GetActualProcessDetail(int seconds);
        long GetQueueLength(string queue);
    }
}
