#region References
using System;
using System.Diagnostics;
#endregion

namespace FWBS.Logging
{
	public class TraceLogger
	{
		#region Constants
		//
		// Registry entries for logging confuguration
		//	trace source name is the section containing the values
		//
		private const string LOGGING_KEYNAME = "Logging";
		private const string LOGGING_TRACELEVEL_VALUE = "TraceLevel";
		private const string LOGGING_DIRECTORY_VALUE = "Directory";
		private const string LOGGING_TEMPLATE_VALUE = "Template";
		#endregion

		#region Properties
		private int TraceID { get; set; }
		private TraceSource TracingSource { get; set; }
		#endregion

		#region Constructors
		private TraceLogger()
		{
			this.TracingSource = new TraceSource("FWBS");
		}

		public TraceLogger(string traceSourceName)
			: this(traceSourceName, 0)
		{ }

		public TraceLogger(string traceSourceName, int id)
		{
			this.TracingSource = new TraceSource(traceSourceName);
			this.TraceID = id;

			this.CheckRegistrySettings(this.TracingSource);
		}

		public TraceLogger(TraceSource ts)
			: this(ts, 0)
		{ }

		public TraceLogger(TraceSource ts, int id)
		{
			this.TracingSource = ts;
			this.TraceID = id;

			this.CheckRegistrySettings(this.TracingSource);
		}
		#endregion

		#region TraceEvent methods
		public void TraceEvent(TraceEventType eventType, string message)
		{
			this.TracingSource.TraceEvent(eventType, this.TraceID, message);
		}

		public void TraceEvent(TraceEventType eventType, int id, string message)
		{
			this.TracingSource.TraceEvent(eventType, id, message);
		}

		public void TraceEvent(TraceEventType eventType, int id, string format, object[] args)
		{
			this.TracingSource.TraceEvent(eventType, id, format, args);
		}

		public void TraceEvent(TraceEventType eventType, string format, object[] args)
		{
			this.TracingSource.TraceEvent(eventType, this.TraceID, format, args);
		}
		#endregion

		#region Critical, Error, Warning, Information, Verbose
		#region Critical
		public void TraceCritical(string message)
		{
			this.TracingSource.TraceEvent(TraceEventType.Critical, this.TraceID, message);
		}

		public void TraceCritical(string format, object[] args)
		{
			this.TracingSource.TraceEvent(TraceEventType.Critical, this.TraceID, format, args);
		}

		public void TraceCritical(int id, string message)
		{
			this.TracingSource.TraceEvent(TraceEventType.Critical, id, message);
		}

		public void TraceCritical(int id, string format, object[] args)
		{
			this.TracingSource.TraceEvent(TraceEventType.Critical, id, format, args);
		}
		#endregion

		#region Error		
		public void TraceError(string message)
		{
			this.TracingSource.TraceEvent(TraceEventType.Error, this.TraceID, message);
		}

		public void TraceError(string format, object[] args)
		{
			this.TracingSource.TraceEvent(TraceEventType.Error, this.TraceID, format, args);
		}

		public void TraceError(int id, string message)
		{
			this.TracingSource.TraceEvent(TraceEventType.Error, id, message);
		}

		public void TraceError(int id, string format, object[] args)
		{
			this.TracingSource.TraceEvent(TraceEventType.Error, id, format, args);
		}
		#endregion

		#region Warning
		public void TraceWarning(string message)
		{
			this.TracingSource.TraceEvent(TraceEventType.Warning, this.TraceID, message);
		}

		public void TraceWarning(string format, object[] args)
		{
			this.TracingSource.TraceEvent(TraceEventType.Warning, this.TraceID, format, args);
		}

		public void TraceWarning(int id, string message)
		{
			this.TracingSource.TraceEvent(TraceEventType.Warning, id, message);
		}

		public void TraceWarning(int id, string format, object[] args)
		{
			this.TracingSource.TraceEvent(TraceEventType.Warning, id, format, args);
		}
		#endregion

		#region Information
		public void TraceInformation(string message)
		{
			this.TracingSource.TraceEvent(TraceEventType.Information, this.TraceID, message);
		}

		public void TraceInformation(string format, object[] args)
		{
			this.TracingSource.TraceEvent(TraceEventType.Information, this.TraceID, format, args);
		}

		public void TraceInformation(int id, string message)
		{
			this.TracingSource.TraceEvent(TraceEventType.Information, id, message);
		}

		public void TraceInformation(int id, string format, object[] args)
		{
			this.TracingSource.TraceEvent(TraceEventType.Information, id, format, args);
		}
		#endregion

		#region Verbose
		public void TraceVerbose(string message)
		{
			this.TracingSource.TraceEvent(TraceEventType.Verbose, this.TraceID, message);
		}

		public void TraceVerbose(string format, object[] args)
		{
			this.TracingSource.TraceEvent(TraceEventType.Verbose, this.TraceID, format, args);
		}

		public void TraceVerbose(int id, string message)
		{
			this.TracingSource.TraceEvent(TraceEventType.Verbose, id, message);
		}

		public void TraceVerbose(int id, string format, object[] args)
		{
			this.TracingSource.TraceEvent(TraceEventType.Verbose, id, format, args);
		}
		#endregion
		#endregion

		#region Close
		public void Close()
		{
			this.TracingSource.Close();
		}
		#endregion

		#region Check registry
		private void CheckRegistrySettings(TraceSource src)
		{
			try
			{
				// create new registry config object
				FWBS.Configuration.RegistryConfig config = new Configuration.RegistryConfig(LOGGING_KEYNAME);
				// is there a section with the TraceSource's name?
				if (config.HasSection(src.Name))
				{
					// get values
					string level = config.GetValue(src.Name, LOGGING_TRACELEVEL_VALUE, "Off");
					string directory = config.GetValue(src.Name, LOGGING_DIRECTORY_VALUE, string.Empty);
					string template = config.GetValue(src.Name, LOGGING_TEMPLATE_VALUE, string.Empty);

					// convert trace level string to its enum
					SourceLevels levelResult;
					if (System.Enum.TryParse<SourceLevels>(level, out levelResult))
					{
						// set template
						if (string.IsNullOrWhiteSpace(template))
						{
							template = src.Name + "-{DateTime:yyyy-MM-dd}.log";
						}
						// set directory
						if (string.IsNullOrWhiteSpace(directory))
						{
							// default directory
							directory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\";
						}
						else
						{
							if (directory[directory.Length - 1] != '\\')
							{
								directory += @"\";
							}
						}

						// check access to directory by creating a tmp file and deleting it
						string tmpFilename = DateTime.Now.Ticks.ToString() + ".tmp";
						System.IO.FileStream fs = System.IO.File.Create(directory + tmpFilename);
						fs.Close();
						System.IO.File.Delete(directory + tmpFilename);

						// set level
						src.Switch.Level = levelResult;
						// create listener
						src.Listeners.Add(new RollingFileTraceListener(directory + template));
					}
				}
			}
			catch (Exception)
			{
				;	// an error has occurred, do nothing
			}
		}
		#endregion
	}
}
