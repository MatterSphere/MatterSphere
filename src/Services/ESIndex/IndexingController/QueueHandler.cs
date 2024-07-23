using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Common.Enums;
using IndexingController.Builders;
using IndexingController.Models;
using IndexingController.Models.Logging;
using Models.Common;
using Models.DbModels;
using Models.ElasticsearchModels;
using Models.Interfaces;
using Models.MSBaseEntities;
using NLog;
using ILogger = Models.Interfaces.ILogger;

namespace IndexingController
{
    public class QueueHandler
    {
        private readonly Decorator _decorator;
        private readonly MessageTypeEnum _messageType;
        private readonly IElasticsearchProvider _esProvider;
        private readonly string _objectType;
        private readonly IDbLogger _dbLogger;
        private readonly ILogger _appLogger;

        private long _messageSize;
        private int _successDocuments;
        private int _failedDocuments;
        private int _contentFailedDocuments;
        private ConcurrentQueue<ExpandoObject> _queue;
        private ConcurrentQueue<DocumentLog> _documentLogs;
        private ConcurrentBag<IndexingProcessResult> _documentsIndexingResults;
        private List<string> _suggestableFields;
        private ParametersData _parametersData;

        public QueueHandler(MessageTypeEnum messageType, string objectType, List<string> suggestableFields, IElasticsearchProvider esProvider, IDbLogger dbLogger, IBlacklistValidator validator, int timeout, bool useOcrIndexing, int ocrTimeout)
        {
            _messageType = messageType;
            _objectType = objectType;
            _suggestableFields = suggestableFields;
            _appLogger = new AppLogger(LogManager.GetCurrentClassLogger());
            var factory = new BuilderFactory(validator, _appLogger, timeout, useOcrIndexing, ocrTimeout);
            _decorator = factory.CreateBuilder(messageType);
            _esProvider = esProvider;
            _dbLogger = dbLogger;
        }

        public void SetParameters(ParametersData parameters)
        {
            _parametersData = parameters;
        }

        public bool ProcessItems(List<ExpandoObject> items, CancellationToken cancellationToken)
        {
            IndexingProcessResult result = null;
            switch (_messageType)
            {
                case MessageTypeEnum.Contentable:
                    _queue = new ConcurrentQueue<ExpandoObject>(items);
                    _documentLogs = new ConcurrentQueue<DocumentLog>();
                    _documentsIndexingResults = new ConcurrentBag<IndexingProcessResult>();
                    result = RunProcessing(cancellationToken);
                    break;
                case MessageTypeEnum.Uncontentable:
                case MessageTypeEnum.Users:
                    result = UncontentableDocumentsProcess(items);
                    break;
                case MessageTypeEnum.Relation:
                    result = RelationDocumentsProcess(items);
                    break;
            }
            return result?.CompletedSuccessfully == true;
        }

        #region Contentable
        private IndexingProcessResult RunProcessing(CancellationToken cancellationToken)
        {
            var messageWatch = Stopwatch.StartNew();

            SplitProcess(cancellationToken);

            if (!_documentLogs.IsEmpty)
            {
                _dbLogger.LogDocumentIndex(_objectType, _documentLogs.ToArray());
            }

            messageWatch.Stop();
            var messageLog = new MessageLog(
                messageWatch.Elapsed,
                _objectType,
                _successDocuments,
                _failedDocuments,
                _contentFailedDocuments,
                _messageSize);
            _dbLogger.LogMessageProcess(messageLog);

            return _documentsIndexingResults.FirstOrDefault(result => result.ExceptionOccurred) 
                ?? _documentsIndexingResults.FirstOrDefault(result => result.CompletedSuccessfully);
        }

        private void SplitProcess(CancellationToken cancellationToken)
        {
            int threadCount = Math.Min(_parametersData.ThreadsCount, _queue.Count);
            Thread[] threads = new Thread[threadCount];
            for (int i = 0; i < threadCount; i++)
            {
                threads[i] = new Thread(ContentableDocumentsThreadProc);
            }

            foreach (Thread thread in threads)
            {
                thread.Start(cancellationToken);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }

        private void ContentableDocumentsThreadProc(object token)
        {
            try
            {
                ContentableDocumentsProcess((CancellationToken)token);
            }
            catch (Exception ex)
            {
                _appLogger.Error(ex.ToString());
            }
        }

        private void ContentableDocumentsProcess(CancellationToken cancellationToken)
        {
            long bulkSize = 0;
            var entities = new List<EntityInfoBatch>();
            ExpandoObject item;
            
            while (!cancellationToken.IsCancellationRequested && _queue.TryDequeue(out item))
            {
                var documentKey = GetEntityKey(item);
                if (_parametersData.UseExtendedLogs)
                {
                    _appLogger.Info($"The document {documentKey} began to be read");
                }
                
                var entityInfoBatch = GetEntityInfo(item);
                if (_parametersData.UseExtendedLogs)
                {
                    _appLogger.Info($"The document {documentKey} has been read");
                }

                if (entityInfoBatch.HasContentReadingError)
                {
                    Interlocked.Increment(ref _contentFailedDocuments);
                }

                if (entityInfoBatch.ProcessError)
                {
                    if (entityInfoBatch.DocumentLog != null)
                    {
                        _documentLogs.Enqueue(entityInfoBatch.DocumentLog);
                    }
                    continue;
                }

                if (entityInfoBatch.Deleted)
                {
                    bool docIsDeleted = _esProvider.Delete(entityInfoBatch.EntityInfo.Key);

                    if (_parametersData.UseExtendedLogs)
                    {
                        var msg = docIsDeleted
                            ? $"The document {documentKey} has been deleted from indexing."
                            : $"An attempt to delete the document {documentKey} from indexing was failed.";
                        _appLogger.Info(msg);

                    }

                    continue;
                }

                if (cancellationToken.IsCancellationRequested)
                    break;

                entities.Add(entityInfoBatch);
                bulkSize += entityInfoBatch.Size;
                Interlocked.Add(ref _messageSize, entityInfoBatch.Size);

                if (bulkSize >= _parametersData.MaxBulkSize || entities.Count >= _parametersData.MaxDocumentsCount)
                {
                    _documentsIndexingResults.Add(ProcessDocuments(entities));
                    bulkSize = 0;
                    entities.Clear();
                }
            }

            if (entities.Count > 0 && !cancellationToken.IsCancellationRequested)
            {
                _documentsIndexingResults.Add(ProcessDocuments(entities));
            }
        }

        private Guid GetEntityKey(ExpandoObject model)
        {
            var key = Guid.Empty;
            var propertyDictionary = model as IDictionary<string, object>;
            foreach (var modelProperty in propertyDictionary)
            {
                if (modelProperty.Key == "id")
                {
                    key = new Guid(modelProperty.Value.ToString());
                    break;
                }
            }

            return key;
        }
        
        private EntityInfoBatch GetEntityInfo(ExpandoObject message)
        {
            var documentWatch = Stopwatch.StartNew();
            var entityInfo = _decorator.CreateDocument(_objectType, CreateSummary(message));
            documentWatch.Stop();

            DocumentLog documentLog = null;
            if (entityInfo.Id != 0 && !entityInfo.Deleted)
            {
                documentLog = new DocumentLog(entityInfo.Id, entityInfo.FileName, documentWatch.Elapsed, entityInfo.Size);

                if (entityInfo.HasError)
                {
                    documentLog.SetError(entityInfo.DocumentProcessErrorType, entityInfo.Error);
                }
            }

            return new EntityInfoBatch(entityInfo, documentLog)
            {
                ProcessError = entityInfo.ProcessError
            };
        }

        private IndexingProcessResult ProcessDocuments(List<EntityInfoBatch> entities)
        {
            var resultLog = new IndexingProcessResult()
            {
                ProcessingItems = entities,
                ProcessedObjectType = _objectType,
                Params = _parametersData
            };

            var watch = Stopwatch.StartNew();
            try
            {
                var result = IndexDocuments(entities);
                resultLog.Response = result;
            }
            catch(Exception e)
            {
                resultLog.Exception = e;
            }
            finally
            {
                watch.Stop();
                resultLog.Elapsed = watch.Elapsed;
            }

            _appLogger.Log(resultLog);
            Interlocked.Add(ref _successDocuments, resultLog.NumberOfItemsSucceeded);
            Interlocked.Add(ref _failedDocuments, resultLog.NumberOfItemsFailed);

            foreach (var entity in entities)
            {
                if (entity.DocumentLog != null)
                {
                    _documentLogs.Enqueue(entity.DocumentLog);
                }
            }
            return resultLog;
        }

        private ElasticsearchResponse IndexDocuments(List<EntityInfoBatch> entities)
        {
            var documents = new List<IndexDocument>();
            
            foreach (var entity in entities)
            {
                string error;
                var document = CreateIndexDocument(entity.EntityInfo, out error);
                if (document != null)
                {
                    documents.Add(document);
                }
                else
                {
                    entity.DocumentLog?.SetError(DocumentProcessErrorTypeEnum.IndexingError, error);
                }
            }

            var response = _esProvider.BulkIndex(MessageTypeEnum.Contentable, documents);
            if (response.HasErrors && response.Logs != null)
            {
                foreach (var entity in entities)
                {
                    ProcessingItemLog logItem;
                    if (response.Logs.TryGetValue(entity.EntityInfo.Key, out logItem) && !logItem.IsSucceeded)
                    {
                        entity.DocumentLog.SetError(DocumentProcessErrorTypeEnum.IndexingError,
                            "The error in Indexing process");
                    }
                }
            }

            return response;
        }

        private IndexDocument CreateIndexDocument(EntityInfo info, out string error)
        {
            try
            {
                error = null;
                return CreateIndexDocument(info.Model, _suggestableFields);
            }
            catch (Exception e)
            {
                error = e.Message;
            }

            return null;
        }

        #endregion

        private IndexingProcessResult UncontentableDocumentsProcess(List<ExpandoObject> items)
        {
            var resultLog = new IndexingProcessResult()
            {
                ProcessingItems = items,
                ProcessedObjectType = _objectType,
                Params = _parametersData
            };

            var watch = Stopwatch.StartNew();
            try
            {
                var entities = items.Select(item => _decorator.CreateEntity(_objectType, item))
                    .Select(entity => CreateSummary(entity))
                    .Select(doc => CreateIndexDocument(doc, _suggestableFields)).ToList();
                var result = _esProvider.BulkIndex(_messageType, entities);
                resultLog.Response = result;
            }
            catch (Exception e)
            {
                resultLog.Exception = e;
            }
            finally
            {
                watch.Stop();
                resultLog.Elapsed = watch.Elapsed;
            }

            _dbLogger.LogMessageProcess(resultLog.ToMessageLog());
            _appLogger.Log(resultLog);
            return resultLog;
        }

        private ExpandoObject CreateSummary(ExpandoObject entity)
        {
            var dict = entity as IDictionary<string, object>;
            if (_parametersData.SummaryFieldEnabled)
            {
                var summary = _parametersData.SummaryTemplates[_objectType];
                foreach (var entry in dict)
                {
                    summary = summary.Replace($"{{{entry.Key}}}", entry.Value.ToString());
                }
                dict["summary"] = new Regex(@"{(\w|[-])+}").Replace(summary, string.Empty);
            }
            else
            {
                dict["summary"] = string.Empty;
            }
            return dict as ExpandoObject;
        }

        private IndexingProcessResult RelationDocumentsProcess(List<ExpandoObject> items)
        {
            var resultLog = new IndexingProcessResult()
            {
                ProcessingItems = items,
                ProcessedObjectType = _objectType,
                Params = _parametersData
            };

            var watch = Stopwatch.StartNew();
            try
            {
                var relations = items.SelectMany(item => _decorator.CreateRelationships(item))
                    .Select(doc => CreateIndexDocument(doc, _suggestableFields)).ToList();
                var result = _esProvider.BulkIndex(_messageType, relations);
                resultLog.Response = result;
                
            }
            catch (Exception e)
            {
                resultLog.Exception = e;
            }
            finally
            {
                watch.Stop();
                resultLog.Elapsed = watch.Elapsed;
            }

            _dbLogger.LogMessageProcess(resultLog.ToMessageLog());
            _appLogger.Log(resultLog);
            return resultLog;
        }
        

        private IndexDocument CreateIndexDocument(ExpandoObject document, List<string> suggestableFields)
        {
            var suggests = new Dictionary<string, string>();
            var properties = document as IDictionary<string, object>;
            var fields = suggestableFields.Distinct().ToList();
            foreach (var field in fields)
            {
                if (properties.ContainsKey(field))
                {
                    suggests.Add(field, properties[field].ToString());
                }
            }

            return new IndexDocument(document, suggests);
        }
        
        #region Classes

        private class EntityInfoBatch
        {
            public EntityInfoBatch(EntityInfo entityInfo, DocumentLog documentLog)
            {
                EntityInfo = entityInfo;
                DocumentLog = documentLog;
            }

            public EntityInfo EntityInfo { get; }
            public DocumentLog DocumentLog { get; }
            public bool ProcessError { get; set; }

            public long Size
            {
                get { return EntityInfo.Size ?? 0; }
            }

            public bool Deleted
            {
                get { return EntityInfo.Deleted; }
            }

            public bool HasLog
            {
                get { return DocumentLog != null; }
            }

            public bool HasContentReadingError
            {
                get { return HasLog && EntityInfo.ContentReadingError; }
            }
        }

        #endregion
    }
}
