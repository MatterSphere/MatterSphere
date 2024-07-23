#region References
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
#endregion

namespace FWBS.Logging
{
	public class RollingTextWriter : IDisposable
	{
		#region Fields
		private string _currentPath;						// file path
		private TextWriter _currentWriter;
		private object _fileLock = new object();			// used for thread sync
		private string _filePathTemplate;
		private IFileSystem _fileSystem = new FileSystem();
		#endregion

		#region Constructor
		internal RollingTextWriter(string filePathTemplate)
		{
			_filePathTemplate = filePathTemplate;
		}
		#endregion

		#region Properties
		internal string FilePathTemplate
		{
			get { return _filePathTemplate; }
		}

		internal IFileSystem FileSystem
		{
			get { return _fileSystem; }
			set
			{
				lock (_fileLock)
				{
					_fileSystem = value;
				}
			}
		}
		#endregion

		#region Flush
		internal void Flush()
		{
			lock (_fileLock)
			{
				if (_currentWriter != null)
				{
					_currentWriter.Flush();
				}
			}
		}
		#endregion

		#region Write
		internal void Write(TraceEventCache eventCache, string value)
		{
			string filePath = GetCurrentFilePath(eventCache);
			lock (_fileLock)
			{
				EnsureCurrentWriter(filePath);
				if (_currentWriter != null)
				{
					_currentWriter.Write(value);
				}
			}
		}
		#endregion

		#region WriteLine
		internal void WriteLine(TraceEventCache eventCache, string value)
		{
			string filePath = GetCurrentFilePath(eventCache);
			lock (_fileLock)
			{
				EnsureCurrentWriter(filePath);
				if (_currentWriter != null)
				{
					_currentWriter.WriteLine(value);
				}
			}
		}
		#endregion

		#region EnsureCurrentWriter
		private void EnsureCurrentWriter(string path)
		{
			// NOTE: This is called inside lock(_fileLock)
			if (_currentPath != path)
			{
				if (_currentWriter != null)
				{
					_currentWriter.Close();
					_currentWriter = null;
				}

				var stream = FileSystem.Open(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
				if (stream != null)
				{
					_currentWriter = new StreamWriter(stream);
					_currentPath = path;
				}
			}
		}
		#endregion

		#region GetCurrentFilePath
		private string GetCurrentFilePath(TraceEventCache eventCache)
		{
			var result = StringTemplate.Format(CultureInfo.CurrentCulture, FilePathTemplate,
				delegate(string name, out object value)
				{
					switch (name.ToUpperInvariant())
					{
						case "APPLICATIONNAME":
							value = TraceFormatter.FormatApplicationName();
							break;
						case "DATETIME":
						case "UTCDATETIME":
							value = TraceFormatter.FormatUniversalTime(eventCache);
							break;
						case "LOCALDATETIME":
							value = TraceFormatter.FormatLocalTime(eventCache);
							break;
						case "MACHINENAME":
							value = Environment.MachineName;
							break;
						case "PROCESSID":
							value = TraceFormatter.FormatProcessId(eventCache);
							break;
						case "PROCESSNAME":
							value = TraceFormatter.FormatProcessName();
							break;
						default:
							value = "{" + name + "}";
							return true;
					}
					return true;
				});
			return result;
		}
		#endregion

		#region IDisposable
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_currentWriter != null)
				{
					_currentWriter.Dispose();
				}
			}
		}
		#endregion
	}
}
