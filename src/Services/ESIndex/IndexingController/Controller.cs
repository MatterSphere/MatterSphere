using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ElasticsearchProvider.Models;
using FileStorageProvider;
using IndexingController.Providers;
using Models.Common;
using Models.DbModels;
using Models.ElasticsearchModels;
using Models.Interfaces;
using NLog;
using QueueReader;
using XmlConverter;

namespace IndexingController
{
    public class Controller
    {
        private readonly Reader _queueReader;
        private readonly ICacheProvider _fsProvider;
        private readonly Converter _converter;
        private IElasticsearchProvider _esProvider;
        private readonly IDbProvider _dbProvider;
        private volatile IBlacklistValidator _blacklistValidator;
        private readonly ElasticsearchProviderBuilder _esBuilder;
        private readonly MessageTypeValidator _validator;
        private readonly IDbLogger _dbLogger;
        private readonly int _interval;
        private List<string> _suggestableFields;
        private ParametersData _parametersData;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly int _timeout;
        private readonly bool _useOcrIndexing;
        private readonly int _ocrTimeout;
        private readonly BlockingCollection<Message> _contentableQueue;
        private readonly BlockingCollection<Message> _noncontentableQueue;
        private readonly ConcurrentQueue<Message> _failedMessages;
        private long _serviceLastTimeChecked; // DateTime ticks
        private ElasticsearchClientParameters _elasticsearchClientParameters;
        private int _processingItems = 0;
        private const int _maxTries = 5;

        public Controller(string conn, string queue, string cachePath, string url, int interval, int timeout, bool useOcrIndexing, int ocrTimeout, int dataIndexShards, int dataIndexReplicas, int userIndexShards, int userIndexReplicas, int workerQueueBound)
        {
            _queueReader = new Reader(conn, queue);
            _fsProvider = new Provider(cachePath, conn);
            _converter = new Converter();
            _dbProvider = new DbProvider.Provider(conn);
            _esBuilder = new ElasticsearchProviderBuilder(_dbProvider, url, 
                new Settings { Shards = dataIndexShards, Replicas = dataIndexReplicas }, 
                new Settings { Shards = userIndexShards, Replicas = userIndexReplicas });
            _validator = new MessageTypeValidator();
            _dbLogger = new DbLogger(_dbProvider);
            _interval = interval;
            _timeout = timeout;
            _useOcrIndexing = useOcrIndexing;
            _ocrTimeout = ocrTimeout;
            _contentableQueue = new BlockingCollection<Message>(workerQueueBound);
            _noncontentableQueue = new BlockingCollection<Message>(workerQueueBound);
            _failedMessages = new ConcurrentQueue<Message>();
        }

        public void SetParameters(ParametersData parameters)
        {
            _parametersData = parameters;
        }

        public void SetElasticsearchClientParameters(ElasticsearchClientParameters elasticsearchClientParameters)
        {
            _elasticsearchClientParameters = elasticsearchClientParameters;
        }

        public void RunProcess(CancellationToken cancellationToken)
        {
            _esProvider = _esBuilder.InitClient(_elasticsearchClientParameters).CreateProvider();
            CheckService();
            PrepareIndices();
            DocumentReader.ContentReaderFactory.Startup();

            Task[] tasks = new Task[2]
            {
                Task.Factory.StartNew(Process, Tuple.Create(_contentableQueue, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default),
                Task.Factory.StartNew(Process, Tuple.Create(_noncontentableQueue, cancellationToken), cancellationToken, TaskCreationOptions.LongRunning, TaskScheduler.Default)
            };
            
            bool isProcess = false;
            ProcessCache(ref isProcess, cancellationToken);
            ProcessDatabase(ref isProcess, cancellationToken);

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ex)
            {
                // TODO: check error handling
                foreach (Exception e in ex.InnerExceptions)
                {
                    if (!(e is OperationCanceledException))
                        _logger.Warn(e);
                }
            }

            DocumentReader.ContentReaderFactory.Shutdown();
        }

        private void ProcessCache(ref bool isProcess, CancellationToken cancellationToken)
        {
            try
            {
                var cache = _fsProvider.ReadMessages();
                if (cache.Any())
                {
                    LogStartProcess(ref isProcess);
                    foreach (Message message in cache)
                    {
                        (message.IsContentable ? _contentableQueue : _noncontentableQueue).Add(message, cancellationToken);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine(ex.Message);
                _contentableQueue.CompleteAdding();
                _noncontentableQueue.CompleteAdding();
            }
        }

        private void ProcessDatabase(ref bool isProcess, CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    Message message = _queueReader.ReadMessage();
                    if (message != null)
                    {
                        _fsProvider.SaveMessage(message);
                        LogStartProcess(ref isProcess);
                        (message.IsContentable ? _contentableQueue : _noncontentableQueue).Add(message, cancellationToken);
                    }
                    else
                    {
                        while (_failedMessages.TryDequeue(out message))
                        {
                            (message.IsContentable ? _contentableQueue : _noncontentableQueue).Add(message, cancellationToken);
                        }
                        if (Interlocked.CompareExchange(ref _processingItems, 0, 0) == 0 && _contentableQueue.Count == 0 && _noncontentableQueue.Count == 0)
                        {
                            LogFinishProcess(ref isProcess);
                            CheckService();
                            _blacklistValidator = null;
                        }
                        cancellationToken.WaitHandle.WaitOne(_interval);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _contentableQueue.CompleteAdding();
                _noncontentableQueue.CompleteAdding();
            }
        }

        private void Process(object state)
        {
            bool taken = false;
            BlockingCollection<Message> queue = ((Tuple<BlockingCollection<Message>, CancellationToken>)state).Item1;
            CancellationToken cancellationToken = ((Tuple<BlockingCollection<Message>, CancellationToken>)state).Item2;

            while (!cancellationToken.IsCancellationRequested && !queue.IsCompleted)
            {
                try
                {
                    Message message;
                    if (taken = queue.TryTake(out message, Timeout.Infinite, cancellationToken))
                    {
                        Interlocked.Increment(ref _processingItems);
                        if (ProcessMessage(message, cancellationToken))
                        {
                            _fsProvider.ClearCache(message);
                        }
                        else
                        {
                            if (message.RetryCount++ < _maxTries)
                            {
                                _failedMessages.Enqueue(message);
                            }
                            else
                            {
                                _fsProvider.FailMessage(message);
                            }
                        }
                    }
                }
                catch (OperationCanceledException ex)
                {
                    Debug.WriteLine(ex.Message);
                    break;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                }
                finally
                {
                    if (taken)
                        Interlocked.Decrement(ref _processingItems);
                }
            }
        }

        private bool IsEmptyMessage(Message item)
        {
            return item == null || item.Data.Length == 0;
        }

        private bool ProcessMessage(Message message, CancellationToken cancellationToken)
        {
            if (IsEmptyMessage(message))
            {
                _logger.Error("The message was empty");
                return true;
            }
            var entities = _converter.Convert(message.Data);
            if (entities.IsEmptyBucket())
            {
                _logger.Error("The message was not valid");
                return true;
            }

            var messageType = _validator.Validate(entities.Bucket);
            var objectType = entities.Bucket.Target == null
                ? entities.Bucket.Entity
                : "RELATIONSHIP";
            var suggestableFields = GetSuggestableFields();
            if (_blacklistValidator == null)
            {
                _blacklistValidator = CreateBlacklistValidator();
            }
            _parametersData.SummaryFieldEnabled = _dbProvider.GetSummarySetting();
            _parametersData.SummaryTemplates = _dbProvider.GetSummaryTemplates();

            var handler = new QueueHandler(
                messageType,
                objectType,
                suggestableFields,
                _esProvider,
                _dbLogger,
                _blacklistValidator,
                _timeout,
                _useOcrIndexing,
                _ocrTimeout);
            handler.SetParameters(_parametersData);
            bool result = false;
            try
            {
                result = handler.ProcessItems(entities.Bucket.Elements, cancellationToken);
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            return result;
        }
        

        private void PrepareIndices()
        {
            if (!_esProvider.CheckDataIndex())
            {
                var dataFields = _dbProvider.GetIndexFields(IndexTypeEnum.Data);
                if (dataFields.Any())
                {
                    var fields = dataFields.Where(field => field.IndexingEnabled)
                        .Select(field => new Field(
                            field.FieldName,
                            field.FieldType,
                            field.Searchable,
                            field.Facetable,
                            field.Suggestable,
                            field.Analyzer)).ToList();
                    _esProvider.CreateDataIndex(fields);
                }
            }

            if (!_esProvider.CheckUserIndex())
            {
                var userFields = _dbProvider.GetIndexFields(IndexTypeEnum.User);
                if (userFields.Any())
                {
                    var fields = userFields.Select(field => new Field(
                        field.FieldName,
                        field.FieldType,
                        field.Searchable,
                        field.Facetable,
                        field.Suggestable,
                        field.Analyzer)).ToList();
                    _esProvider.CreateUserIndex(fields);
                }
            }
        }

        private void LogStartProcess(ref bool isProcess)
        {
            if (!isProcess)
            {
                _dbLogger.StartIndexingProcess();
                _logger.Info("The Indexing process is started");
                isProcess = true;
            }
        }

        private void LogFinishProcess(ref bool isProcess)
        {
            if (isProcess)
            {
                _dbLogger.CompleteIndexingProcess();
                _logger.Info("The Indexing process is completed");
                isProcess = false;
            }
        }

        private List<string> GetSuggestableFields()
        {
            if (_suggestableFields == null)
            {
                _suggestableFields = _dbProvider.GetSuggestableFields();
            }

            return _suggestableFields;
        }

        private IBlacklistValidator CreateBlacklistValidator()
        {
            var items = _dbProvider.GetBlacklist();

            return new BlacklistValidator(items.ToArray());
        }

        private void CheckService()
        {
            if (DateTime.UtcNow.AddMinutes(-5).Ticks < Interlocked.Read(ref _serviceLastTimeChecked))
                return;

            string errorMessage;
            bool result = _esProvider.CheckService(out errorMessage);
            Interlocked.Exchange(ref _serviceLastTimeChecked, DateTime.UtcNow.Ticks);
            if (!result)
            {
                _logger.Error(errorMessage);
                throw new InvalidOperationException(errorMessage);
            }
        }
    }
}
