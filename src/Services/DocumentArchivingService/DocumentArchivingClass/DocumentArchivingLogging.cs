using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DocumentArchivingClass
{
    class DocumentArchivingLogging
    {
        private string logFileLocation;
        private string EventLogSource;
        private const int EventLogError = 1;
        private const int EventLogInformation = 2;
        private const int EventLogErrorCode = 234;
        private const string EventLogType = "Application";

        public DocumentArchivingLogging(string logFile)
        {
            EventLogSource = logFile;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            path = Path.Combine(path, "3E MatterSphere", "Document Archiver");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            logFileLocation = Path.Combine(path, logFile + ".log");
            if (!File.Exists(logFileLocation))
            {
                using (FileStream fs = File.Create(logFileLocation)) { }
            }
        }

        public void CreateLogEntry(string LogEntry)
        {
            if (DocumentArchivingConfiguration.GetConfigurationItemBool("LogToFileEnabled"))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Convert.ToString(DateTime.Now));
                    sb.Append(" - ");
                    sb.Append(LogEntry);
                    using (StreamWriter sw = File.AppendText(logFileLocation))
                    {
                        sw.WriteLine(sb.ToString());
                    }
                }
                catch (Exception ex)
                {
                    CreateEventLogEntry(ex.Message, EventLogError); 
                }
            }
            if (DocumentArchivingConfiguration.GetConfigurationItemBool("LogAllToEventLog"))
            {
                CreateEventLogEntry(LogEntry, EventLogInformation);
            }
        }

        public void CreateErrorEntry(string LogEntry)
        {
            CreateLogEntry("Error: " + LogEntry);
            CreateEventLogEntry(LogEntry, EventLogError);
        }

        public void CreateErrorEntry(string logEntry, Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(logEntry).AppendLine(ex.Message);
            if (ex.InnerException != null)
                sb.AppendLine("Inner Exception").AppendLine(ex.InnerException.Message);
            CreateErrorEntry(sb.ToString());
        }

        private void CreateEventLogEntry(string sEvent, int sType)
        {
            if (!EventLog.SourceExists(EventLogSource))
            {
                EventLog.CreateEventSource(EventLogSource, EventLogType);
            }
            if (sType == EventLogError)
            {
                EventLog.WriteEntry(EventLogSource, sEvent, EventLogEntryType.Error, EventLogErrorCode);
            }
            else
            {
                EventLog.WriteEntry(EventLogSource, sEvent, EventLogEntryType.Information);
            }
        }
    }
}
