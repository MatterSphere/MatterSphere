using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MsAdUsersSyncService.MSADSync
{
    public class Logging
    {
        private readonly string _logFileLocation;
        private readonly string _eventLogSource;
        private const int EventLogError = 1;
        private const int EventLogInformation = 2;
        private const int EventLogErrorCode = 234;
        private const string EventLogType = "Application";

        public Logging(string logFile)
        {
            _eventLogSource = logFile;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            path = Path.Combine(path, "3E MatterSphere", "ADSync");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = logFile + ".log";
            string filepath = Path.Combine(path, filename);
            if (!File.Exists(filepath))
            {
                using (FileStream fs = File.Create(filepath)) { }

            }
            _logFileLocation = filepath;
        }

        public void CreateLogEntry(string LogEntry)
        {
            if (Config.GetConfigurationItemBool("LogToFileEnabled"))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Convert.ToString(DateTime.Now));
                    sb.Append(" - ");
                    sb.Append(LogEntry);
                    using (StreamWriter sw = File.AppendText(_logFileLocation))
                    {
                        sw.WriteLine(sb.ToString());
                    }
                }
                catch (Exception ex)
                {
                    CreateEventLogEntry(ex.Message, 1);
                }
            }
            if (Config.GetConfigurationItemBool("LogAllToEventLog"))
            {
                CreateEventLogEntry(LogEntry, 2);
            }
        }

        public void CreateErrorEntry(string LogEntry)
        {
            CreateLogEntry("Error: " + LogEntry);
            CreateEventLogEntry(LogEntry, 1);
        }

        private void CreateEventLogEntry(string sEvent, int sType)
        {
            if (!EventLog.SourceExists(_eventLogSource))
            {
                EventLog.CreateEventSource(_eventLogSource, EventLogType);
            }
            if (sType == EventLogError)
            {
                EventLog.WriteEntry(_eventLogSource, sEvent, EventLogEntryType.Error, EventLogErrorCode);
            }
            else
            {
                EventLog.WriteEntry(_eventLogSource, sEvent, EventLogEntryType.Information);
            }
        }
    }
}
