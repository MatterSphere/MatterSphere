using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MSEWSClass
{
    class MSEWSLogging
    {
        private string logFileLocation;
        private string EventLogSource;
        private const int EventLogError = 1;
        private const int EventLogInformation = 2;
        private const int EventLogErrorCode = 234;
        private const string EventLogType = "Application";

        public MSEWSLogging(string logFile)
        {
            EventLogSource = logFile;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            path = Path.Combine(path, "Thomson Reuters Elite", "MSEWS");
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
            if (MSEWSConfiguration.GetConfigurationItemBool("LogToFileEnabled"))
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
                    CreateEventLogEntry(ex.Message,1); 
                }
            }
            if (MSEWSConfiguration.GetConfigurationItemBool("LogAllToEventLog"))
            {
                CreateEventLogEntry(LogEntry,2);
            }
        }

        public void CreateErrorEntry(string LogEntry)
        {
            CreateLogEntry("Error: " + LogEntry);
            CreateEventLogEntry(LogEntry,1);
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
