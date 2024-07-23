using System;
using System.Diagnostics;
using NLog;
using ILogger = Models.Interfaces.ILogger;

namespace IndexingController.Models
{
    public class AppLogger : ILogger
    {
        private static readonly string WINEVENTLOG_LOG = "ESIndexToolService";
        private static readonly string WINEVENTLOG_SRC = "3E MatterSphere Elasticsearch Index Tool Service";
        private static readonly int MAX_MESSAGE_LENGTH = 31820;
        private readonly Logger _logger;
        private readonly EventLog _eventLog;
        
        public AppLogger(Logger logger)
        {
            _logger = logger;
            if (!EventLog.SourceExists(WINEVENTLOG_SRC))
            {
                EventLog.CreateEventSource(WINEVENTLOG_SRC, WINEVENTLOG_LOG);
            }
            _eventLog = new EventLog(WINEVENTLOG_LOG, ".", WINEVENTLOG_SRC);
        }

        public void Info(string message)
        {
            LogToConsole(message);
            _logger.Info(message);
            WriteMessageToEventLogger(message, EventLogEntryType.Information);
        }

        public void Error(string message)
        {
            LogToConsole(message);
            _logger.Error(message);
            WriteMessageToEventLogger(message, EventLogEntryType.Error);
        }

        private void WriteMessageToEventLogger(string message, EventLogEntryType entryType)
        {
            if (message.Length > MAX_MESSAGE_LENGTH)
            {
                var newLineIndex = message.LastIndexOf(Environment.NewLine, MAX_MESSAGE_LENGTH, MAX_MESSAGE_LENGTH);
                var messageToLog = message.Substring(0, newLineIndex);
                _eventLog.WriteEntry(messageToLog, entryType);
                WriteMessageToEventLogger(message.Substring(newLineIndex + Environment.NewLine.Length), entryType);
            }
            else
            {
                _eventLog.WriteEntry(message, entryType);
            }
        }

        [Conditional("DEBUG")]
        private void LogToConsole(string message)
        {
            Console.WriteLine(DateTime.Now.ToString(@"%hh\:%mm\:%ss\.fff"));
            Console.WriteLine(message);
        }
    }
}
