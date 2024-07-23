using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MCEPGlobalClasses
{
    class MCEPLogging
    {
        private string logFileLocation;
        private string EventLogSource;
        private const int EventLogError = 1;
        private const int EventLogInformation = 2;
        private const int EventLogErrorCode = 234;
        private const string EventLogType = "Application";

        public MCEPLogging(string logFile)
        {
            EventLogSource = logFile;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            path = Path.Combine(path, "3E MatterSphere", "MCEP");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filename = logFile + ".log";
            string filepath = System.IO.Path.Combine(path, filename);
            if (!File.Exists(filepath))
            {
                using (FileStream fs = File.Create(filepath)) { }
                
            }
            logFileLocation = filepath;
        }

        public void CreateLogEntry(string LogEntry)
        {
            if (MCEPConfiguration.GetConfigurationItemBool("LogToFileEnabled"))
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
            if (MCEPConfiguration.GetConfigurationItemBool("LogAllToEventLog"))
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
