using System;
using System.Diagnostics;
using System.Threading;
using MSIndex.Common;
using MSIndex.Common.Interfaces;
using MSIndex.Common.Models;

namespace MSIndex.Controller
{
    public class Controller
    {
        private readonly IQueueReader _queueReader;
        private readonly IFileStorageProvider _fsProvider;
        private readonly IConverter _converter;
        private readonly IHandlerProvider _handlerProvider;
        private readonly int _interval;

        public Controller(string source, string destination, string queue, string cachePath, int interval)
        {
            _queueReader = new QueueReader(source, queue);
            _fsProvider = new FileStorageProvider(cachePath, source);
            _converter = new Converter();
            _handlerProvider = new HandlerProvider(new DbProvider(destination), new Mapper());
            _interval = interval;
        }

        public void RunProcess(CancellationToken cancellationToken)
        {
            var cache = _fsProvider.ReadData();
            if (cache != null && cache.Length > 0)
            {
                ProcessMessage(cache);
                _fsProvider.ClearCache();
            }

            StartListening(cancellationToken);
        }

        private void ProcessMessage(byte[] message)
        {
            var entities = _converter.Convert(message);
            var entityType = entities.GetEntityType();
            var handler = _handlerProvider.GetHandler(entityType);
            handler.Index(entities.Data.Items);
            SaveApplicationLog(entityType);
        }

        private void StartListening(CancellationToken cancellationToken)
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var message = _queueReader.Read();
                    if (message == null)
                    {
                        cancellationToken.WaitHandle.WaitOne(_interval);
                        continue;
                    }

                    _fsProvider.SaveData(message);
                    ProcessMessage(message);
                    _fsProvider.ClearCache();
                }
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private static void SaveApplicationLog(EntityType entityType)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = "Application";
                eventLog.WriteEntry($"The message {entityType} was processed", EventLogEntryType.Information, 44, 44);
            }
        }
    }
}
