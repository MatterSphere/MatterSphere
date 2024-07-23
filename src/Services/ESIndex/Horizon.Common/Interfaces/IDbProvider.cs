using System;
using System.Collections.Generic;
using Horizon.Common.Models.Common;
using Horizon.Common.Models.Repositories.Blacklist;
using Horizon.Common.Models.Repositories.IndexProcess;
using Horizon.Common.Models.Repositories.IndexReport;
using Horizon.Common.Models.Repositories.IndexStructure;
using Horizon.Common.Models.Repositories.ProcessingStatus;

namespace Horizon.Common.Interfaces
{
    public interface IDbProvider
    {
        KeyValuePair<ResponseStatus, List<IndexInfo>> GetIndices();
        KeyValuePair<ResponseStatus, List<IndexEntity>> GetIndexEntities(short indexId);
        KeyValuePair<ResponseStatus, List<IndexFieldRow>> GetIndexFields(short entityId);
        ResponseStatus DeleteIndexField(short entityId, string field);
        KeyValuePair<ResponseStatus, bool> CheckExtendedData(string tableName, string pkFieldName);
        KeyValuePair<ResponseStatus, List<TableField>> GetTableFields(string tableName);
        KeyValuePair<ResponseStatus, bool> AddField(IndexField field);
        KeyValuePair<ResponseStatus, bool> UpdateIndexField(IndexField field);
        KeyValuePair<ResponseStatus, List<string>> GetCodeLookupGroups();
        KeyValuePair<ResponseStatus, Dictionary<string, string>> GetFacetableCodeLookups();
        KeyValuePair<ResponseStatus, bool> SetFacetableCodeLookup(string cdCode, string cdDesc, bool createNew);
        ResponseStatus ChangeIndexingEnabling(short entityId, bool enable);

        KeyValuePair<ResponseStatus, List<IReportItem>> GetDocumentBuckets(ContentableEntityTypeEnum entityType);
        KeyValuePair<ResponseStatus, List<DocumentErrorBucket>> GetDocumentErrorBuckets(string extension,
            ContentableEntityTypeEnum entityType);
        KeyValuePair<ResponseStatus, List<DocumentError>> GetDocumentErrors(string extension, string errorCode,
            int page, int pageSize, ContentableEntityTypeEnum entityType);
        KeyValuePair<ResponseStatus, List<EntityProcessItem>> GetActualProcessDetail(int seconds);
        KeyValuePair<ResponseStatus, long> GetQueueLength(string queue);

        KeyValuePair<ResponseStatus, List<BlacklistItem>> GetBlacklist();
        ResponseStatus AddBlacklistItem(BlacklistItem item);
        ResponseStatus RemoveBlacklistGroup(string extension);
        ResponseStatus RemoveBlacklistItem(string extension, string metadata = null, string encoding = null);
        KeyValuePair<ResponseStatus, List<string>> GetExtensionsForReindexing();
        ResponseStatus AddExtensionForReindexing(string extension);
        ResponseStatus ReindexAllFailedDocuments();
        KeyValuePair<ResponseStatus, IndexSettings> GetIndexSettings();
        ResponseStatus SaveIndexSettings(IndexSettings settings);

        KeyValuePair<ResponseStatus, List<ProcessHistoryItem>> GetProcessHistory(DateTime dateFrom, DateTime dateTo);
        KeyValuePair<ResponseStatus, List<ProcessHistoryItemDetail>> GetProcessHistoryDetail(long processId);
        KeyValuePair<ResponseStatus, List<ErrorCodeItem>> GetErrorCodes(int processId);
        KeyValuePair<ResponseStatus, List<DocumentTypeItem>> GetDocumentTypes(int processId, string errorCode);
        KeyValuePair<ResponseStatus, List<DocumentItem>> GetDocuments(int processId, string errorCode, string extension, int page, int pageSize);
        KeyValuePair<ResponseStatus, List<DocumentErrorInfo>> GetDocumentErrorsReport(int processId);
    }
}
