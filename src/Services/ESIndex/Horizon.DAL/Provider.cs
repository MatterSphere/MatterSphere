using System;
using System.Collections.Generic;
using System.Linq;
using Horizon.Common.Interfaces;
using Horizon.Common.Models.Common;
using Horizon.Common.Models.Repositories.Blacklist;
using Horizon.Common.Models.Repositories.IndexProcess;
using Horizon.Common.Models.Repositories.IndexReport;
using Horizon.Common.Models.Repositories.IndexStructure;
using Horizon.Common.Models.Repositories.ProcessingStatus;
using Horizon.DAL.AdoNetRepository;

namespace Horizon.DAL
{
    public class Provider : IDbProvider
    {
        private readonly string _connection;
        public Provider(string connection)
        {
            _connection = connection;
        }

        private IIndexStructureRepository _indexStructureRepository;
        private IIndexStructureRepository IndexStructureRepository
        {
            get
            {
                if (_indexStructureRepository == null)
                {
                    _indexStructureRepository = new IndexStructureRepository(_connection);
                }

                return _indexStructureRepository;
            }
        }

        private IIndexReportRepository _indexReportRepository;
        private IIndexReportRepository IndexReportRepository
        {
            get
            {
                if (_indexReportRepository == null)
                {
                    _indexReportRepository = new IndexReportRepository(_connection);
                }

                return _indexReportRepository;
            }
        }

        private IIndexProcessRepository _indexProcessRepository;
        private IIndexProcessRepository IndexProcessRepository
        {
            get
            {
                if (_indexProcessRepository == null)
                {
                    _indexProcessRepository = new IndexProcessRepository(_connection);
                }

                return _indexProcessRepository;
            }
        }

        private IProcessingStatusRepository _processingStatusRepository;
        private IProcessingStatusRepository ProcessingStatusRepository
        {
            get
            {
                if (_processingStatusRepository == null)
                {
                    _processingStatusRepository = new ProcessingStatusRepository(_connection);
                }

                return _processingStatusRepository;
            }
        }

        private IIndexTableRepository _indexTableRepository;
        private IIndexTableRepository IndexTableRepository
        {
            get
            {
                if (_indexTableRepository == null)
                {
                    _indexTableRepository = new IndexTableRepository(_connection);
                }

                return _indexTableRepository;
            }
        }

        #region IndexStructureRepository

        public KeyValuePair<ResponseStatus, List<IndexInfo>> GetIndices()
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<IndexInfo>>(new ResponseStatus(), IndexStructureRepository.GetIndices().ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<IndexInfo>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, List<IndexEntity>> GetIndexEntities(short indexId)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<IndexEntity>>(new ResponseStatus(), IndexStructureRepository.GetIndexEntities(indexId).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<IndexEntity>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, List<IndexFieldRow>> GetIndexFields(short entityId)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<IndexFieldRow>>(new ResponseStatus(), IndexStructureRepository.GetIndexFields(entityId).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<IndexFieldRow>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, List<IndexFieldRow>> GetAllIndexFields(short indexId)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<IndexFieldRow>>(new ResponseStatus(), IndexStructureRepository.GetAllIndexFields(indexId).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<IndexFieldRow>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, bool> UpdateIndexField(IndexField field)
        {
            try
            {
                IndexStructureRepository.UpdateIndexField(field);
                return new KeyValuePair<ResponseStatus, bool>(new ResponseStatus(), true);
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, bool>(new ResponseStatus(ex.Message), false);
            }
        }

        public KeyValuePair<ResponseStatus, bool> UpdateCommonIndexField(IndexTypeEnum indexType, IndexField field)
        {
            try
            {
                var indexInfo = GetIndices().Value.FirstOrDefault(index => index.IndexType == indexType);
                IndexStructureRepository.UpdateCommonIndexField(indexInfo.Id, field);
                return new KeyValuePair<ResponseStatus, bool>(new ResponseStatus(), true);
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, bool>(new ResponseStatus(ex.Message), false);
            }
        }

        public ResponseStatus DeleteIndexField(short entityId, string field)
        {
            try
            {
                IndexStructureRepository.DeleteIndexField(entityId, field);
                return new ResponseStatus();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(ex.Message);
            }
        }

        public KeyValuePair<ResponseStatus, bool> CheckExtendedData(string tableName, string pkFieldName)
        {
            try
            {
                var result = IndexStructureRepository.CheckExtendedData(tableName, pkFieldName);
                return new KeyValuePair<ResponseStatus, bool>(new ResponseStatus(), result);
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, bool>(new ResponseStatus(ex.Message), false);
            }
        }

        public KeyValuePair<ResponseStatus, List<TableField>> GetTableFields(string tableName)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<TableField>>(new ResponseStatus(), IndexStructureRepository.GetTableFields(tableName).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<TableField>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, bool> AddField(IndexField field)
        {
            try
            {
                IndexStructureRepository.AddField(field);
                return new KeyValuePair<ResponseStatus, bool>(new ResponseStatus(), true);
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, bool>(new ResponseStatus(ex.Message), false);
            }
        }

        public ResponseStatus ChangeIndexingEnabling(short entityId, bool enable)
        {
            try
            {
                IndexStructureRepository.ChangeIndexingEnabling(entityId, enable);
                return new ResponseStatus();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(ex.Message);
            }
        }

        public KeyValuePair<ResponseStatus, List<string>> GetCodeLookupGroups()
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<string>>(new ResponseStatus(), IndexStructureRepository.GetCodeLookupGroups());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<string>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, Dictionary<string, string>> GetFacetableCodeLookups()
        {
            try
            {
                return new KeyValuePair<ResponseStatus, Dictionary<string, string>>(new ResponseStatus(), IndexStructureRepository.GetFacetableCodeLookups());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, Dictionary<string, string>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, bool> SetFacetableCodeLookup(string cdCode, string cdDesc, bool createNew)
        {
            try
            {
                if (createNew)
                    IndexStructureRepository.CreateFacetableCodeLookup(cdCode, cdDesc);
                else
                    IndexStructureRepository.UpdateFacetableCodeLookup(cdCode, cdDesc);
                return new KeyValuePair<ResponseStatus, bool>(new ResponseStatus(), true);
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, bool>(new ResponseStatus(ex.Message), false);
            }
        }

        public ResponseStatus UpdateSummaryTemplate(IndexEntity entity)
        {
            try
            {
                IndexStructureRepository.UpdateSummaryTemplate(entity);
                return new ResponseStatus();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(ex.Message);
            }
        }

        #endregion

        #region IndexReportRepository
        public KeyValuePair<ResponseStatus, List<IReportItem>> GetDocumentBuckets(ContentableEntityTypeEnum entityType)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<IReportItem>>(new ResponseStatus(), IndexReportRepository.GetDocumentBuckets(entityType).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<IReportItem>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, List<DocumentErrorBucket>> GetDocumentErrorBuckets(string extension, ContentableEntityTypeEnum entityType)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<DocumentErrorBucket>>(new ResponseStatus(), IndexReportRepository.GetDocumentErrorBuckets(extension, entityType).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<DocumentErrorBucket>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, List<DocumentError>> GetDocumentErrors(string extension, string errorCode, int page, int pageSize, ContentableEntityTypeEnum entityType)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<DocumentError>>(new ResponseStatus(), IndexReportRepository.GetDocumentErrors(extension, errorCode, page, pageSize, entityType).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<DocumentError>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, List<EntityProcessItem>> GetActualProcessDetail(int seconds)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<EntityProcessItem>>(new ResponseStatus(), IndexReportRepository.GetActualProcessDetail(seconds).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<EntityProcessItem>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, long> GetQueueLength(string queue)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, long>(new ResponseStatus(), IndexReportRepository.GetQueueLength(queue));
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, long>(new ResponseStatus(ex.Message), 0);
            }
        }

        #endregion

        #region IndexProcessRepository
        public KeyValuePair<ResponseStatus, List<BlacklistItem>> GetBlacklist()
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<BlacklistItem>>(new ResponseStatus(), IndexProcessRepository.GetBlacklist().ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<BlacklistItem>>(new ResponseStatus(ex.Message), null);
            }
        }

        public ResponseStatus AddBlacklistItem(BlacklistItem item)
        {
            try
            {
                IndexProcessRepository.AddBlacklistItem(item);
                return new ResponseStatus();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(ex.Message);
            }
        }

        public ResponseStatus RemoveBlacklistGroup(string extension)
        {
            try
            {
                IndexProcessRepository.RemoveBlacklistGroup(extension);
                return new ResponseStatus();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(ex.Message);
            }
        }

        public ResponseStatus RemoveBlacklistItem(string extension, string metadata = null, string encoding = null)
        {
            try
            {
                IndexProcessRepository.RemoveBlacklistItem(extension, metadata, encoding);
                return new ResponseStatus();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(ex.Message);
            }
        }

        public KeyValuePair<ResponseStatus, List<string>> GetExtensionsForReindexing()
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<string>>(new ResponseStatus(), IndexProcessRepository.GetExtensionsForReindexing().ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<string>>(new ResponseStatus(ex.Message), null);
            }
        }

        public ResponseStatus AddExtensionForReindexing(string extension)
        {
            try
            {
                IndexProcessRepository.AddExtensionForReindexing(extension);
                return new ResponseStatus();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(ex.Message);
            }
        }

        public ResponseStatus ReindexAllFailedDocuments()
        {
            try
            {
                IndexProcessRepository.ReindexAllFailedDocuments();
                return new ResponseStatus();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(ex.Message);
            }
        }

        public KeyValuePair<ResponseStatus, IndexSettings> GetIndexSettings()
        {
            try
            {
                return new KeyValuePair<ResponseStatus, IndexSettings>(new ResponseStatus(), IndexProcessRepository.GetIndexSettings());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, IndexSettings>(new ResponseStatus(ex.Message), null);
            }
        }

        public ResponseStatus SaveIndexSettings(IndexSettings settings)
        {
            try
            {
                var documentFullCopyRequired = IndexTableRepository.GetObjectTypeFullCopyRequired("DOCUMENT");
                var previousIndexSettings = GetIndexSettings().Value;
                if (documentFullCopyRequired)
                {
                    settings.PreviousDocumentDateLimit = previousIndexSettings.PreviousDocumentDateLimit;
                }
                else
                {
                    settings.PreviousDocumentDateLimit = previousIndexSettings.DocumentDateLimit;
                    IndexTableRepository.SetObjectTypeFullCopyRequired("DOCUMENT", true);
                }

                IndexProcessRepository.SaveIndexSettings(settings);
                return new ResponseStatus();
            }
            catch (Exception ex)
            {
                return new ResponseStatus(ex.Message);
            }
        }

        #endregion


        #region ProcessingStatusRepository

        public KeyValuePair<ResponseStatus, List<ProcessHistoryItem>> GetProcessHistory(DateTime dateFrom, DateTime dateTo)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<ProcessHistoryItem>>(new ResponseStatus(), ProcessingStatusRepository.GetProcessHistory(dateFrom, dateTo).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<ProcessHistoryItem>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, List<ProcessHistoryItemDetail>> GetProcessHistoryDetail(long processId)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<ProcessHistoryItemDetail>>(new ResponseStatus(), ProcessingStatusRepository.GetProcessHistoryDetail(processId).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<ProcessHistoryItemDetail>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, List<ErrorCodeItem>> GetErrorCodes(int processId)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<ErrorCodeItem>>(new ResponseStatus(), ProcessingStatusRepository.GetErrorCodes(processId).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<ErrorCodeItem>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, List<DocumentTypeItem>> GetDocumentTypes(int processId, string errorCode)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<DocumentTypeItem>>(new ResponseStatus(), ProcessingStatusRepository.GetDocumentTypes(processId, errorCode).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<DocumentTypeItem>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, List<DocumentItem>> GetDocuments(int processId, string errorCode,
            string extension, int page, int pageSize)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<DocumentItem>>(new ResponseStatus(),
                    ProcessingStatusRepository.GetDocuments(processId, errorCode, extension, page, pageSize).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<DocumentItem>>(new ResponseStatus(ex.Message), null);
            }
        }

        public KeyValuePair<ResponseStatus, List<DocumentErrorInfo>> GetDocumentErrorsReport(int processId)
        {
            try
            {
                return new KeyValuePair<ResponseStatus, List<DocumentErrorInfo>>(new ResponseStatus(),
                    ProcessingStatusRepository.GetDocumentErrorsReport(processId).ToList());
            }
            catch (Exception ex)
            {
                return new KeyValuePair<ResponseStatus, List<DocumentErrorInfo>>(new ResponseStatus(ex.Message), null);
            }
        }

        #endregion
    }
}
