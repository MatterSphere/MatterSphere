using System;
using System.Data;
using System.Diagnostics;
using System.IO;

namespace FWBS.Common
{

    /// <summary>
    /// Writes event logs to the event log or custom xml file.
    /// </summary>
    public class EventLogger
	{
		/// <summary>
		/// Log object can be an event log or file.
		/// </summary>
		object _log = null;

		/// <summary>
		/// Level category.
		/// </summary>
		short _level = 0;

		/// <summary>
		/// Maximum of entries to store.
		/// </summary>
		int _max = 100;

		/// <summary>
		/// Log name.
		/// </summary>
		string _logName = "";

		/// <summary>
		/// Source project.
		/// </summary>
		string _source = "";

		/// <summary>
		/// Log file path.
		/// </summary>
		FilePath _file;

		/// <summary>
		/// Output the stack trace.
		/// </summary>
		bool _stack = false;

		private EventLogger(){}

		/// <summary>
		/// Created an event log object with the specified category level etc...
		/// </summary>
		/// <param name="source">Event source.</param>
		/// <param name="logName">Event log name.</param>
		/// <param name="level">Category Level</param>
        /// <param name="stackTrace">Stack trace.</param>
		public EventLogger(string source, string logName, short level, bool stackTrace)
		{
			EventLog log;

			//MNW: Added Try loop to stop Web Site Failing.
			try
			{
				_source = source;
				_logName = logName;
				if (!EventLog.SourceExists(source))
					EventLog.CreateEventSource(source, logName);
				log = new EventLog(logName);
				log.Source = source;
				_level = level;
				_stack = stackTrace;
				_log = log;
			}
			catch{}
			finally {}
		}

		/// <summary>
		/// Creates a log based on a filepath location.
		/// </summary>
		/// <param name="file">Log file.</param>
		/// <param name="source">Project source</param>
		/// <param name="logName">Log name.</param>
		/// <param name="level">Level of tracing.</param>
        /// <param name="stackTrace">Stack trace</param>
		/// <param name="maximum">Maximum number of log entries.</param>
		public EventLogger(FilePath file, string source, string logName, short level, int maximum, bool stackTrace)
		{
			//Build the table that holds the info before writing to file.
			DataSet ds = new DataSet("EventLogger");
			DataTable dt = ds.Tables.Add(logName);
            //UTCFIX: DM - 30/11/06 - Make data column rollback to UTC.
			dt.Columns.Add("DateTime", typeof(DateTime)).DateTimeMode = DataSetDateTime.Utc;
			dt.Columns.Add("Type", typeof(string));
			dt.Columns.Add("Level", typeof(short));
			dt.Columns.Add("Message", typeof(string));
			dt.Columns.Add("Source", typeof(string));
			dt.Columns.Add("StackMethod", typeof(string));
			dt.Columns.Add("User", typeof(string));
			dt.Columns.Add("Machine", typeof(string));
		
			_source = source;
			_logName = logName;
			_level = level;
			_max = maximum;
			_file = file;
			_stack = stackTrace;
			
			if (File.Exists(file))
				ds.ReadXml(file, XmlReadMode.IgnoreSchema);
			else
			{
				Directory dir = Path.GetDirectoryName(file);
				System.IO.Directory.CreateDirectory(dir);
			}

			_log = ds;
		}


		/// <summary>
		/// Writes to the specified log format.
		/// </summary>
		/// <param name="message">Message to write.</param>
		/// <param name="level">Category level.</param>
		/// <param name="type">Event Type.</param>
		public void Write(string message, short level, EventLogEntryType type)
		{
					
			if (level <= _level)
			{
				string stackInfo = "";
				

				//Get the stack trace if available.
				if (_stack)
				{
					System.Text.StringBuilder sb = new System.Text.StringBuilder();
					StackTrace stack = new StackTrace();
					for (int ctr = 0; ctr < stack.FrameCount; ctr++)
					{
						sb = sb.Append (stack.GetFrame(ctr).GetMethod().Name);
						sb = sb.Append (", ");
					}
					stackInfo = sb.ToString();
				}

				// if the log item is an event log type.
				if (_log is EventLog)
					((EventLog) _log).WriteEntry("Stack: " + stackInfo + " - Message: " + message, type, 0, level);

				//If the log item is a data set.
				if (_log is DataSet)
				{
					DataSet ds = (DataSet)_log;
					int del = ds.Tables[_logName].Rows.Count - (_max - 1);
					if (del > 0)
					{
						for (int ctr = 0; ctr < del; ctr++)
							ds.Tables[_logName].Rows.RemoveAt(0);
					}

					object [] row =  {DateTime.Now,  type.ToString(), level, message, _source, stackInfo, Environment.UserName , Environment.MachineName};

					
					ds.Tables[_logName].Rows.Add(row);
					ds.AcceptChanges();
					ds.WriteXml(_file, XmlWriteMode.IgnoreSchema);


				}

				Trace.WriteLine(message, "FWBS");

			}
		}

		public void Write(string message, short level)
		{
			Write(message, level, EventLogEntryType.Information);
		}

		public void Write(string message)
		{
			Write(message, 0, EventLogEntryType.Information);
		}

	}
}
