using System;
using System.Collections.Generic;
using Horizon.Common.Models.Repositories.ProcessingStatus;

namespace Horizon.Common.Interfaces
{
    public interface IProcessingStatusRepository
    {
        IEnumerable<ProcessHistoryItem> GetProcessHistory(DateTime dateFrom, DateTime dateTo);
        IEnumerable<ProcessHistoryItemDetail> GetProcessHistoryDetail(long processId);
        IEnumerable<ErrorCodeItem> GetErrorCodes(int processId);
        IEnumerable<DocumentTypeItem> GetDocumentTypes(int processId, string errorCode);
        IEnumerable<DocumentItem> GetDocuments(int processId, string errorCode, string extension, int page, int pageSize);
        IEnumerable<DocumentErrorInfo> GetDocumentErrorsReport(int processId);
    }
}
