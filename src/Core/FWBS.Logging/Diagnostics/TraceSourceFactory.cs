#region References
using System;
using System.Diagnostics;
#endregion

namespace FWBS.Logging
{
	public class TraceSourceFactory
	{
		#region Fields
		private static TraceSource traceSource = null;
		#endregion

		#region Constructor - instance
		private TraceSourceFactory()
		{
		}
		#endregion

		#region SharedTraceSource
		public static TraceSource SharedTraceSource()
		{
			try
			{
				if (TraceSourceFactory.traceSource == null)
				{
					TraceSourceFactory.traceSource = new TraceSource("FWBS_SharedTraceSource");
				}

				return TraceSourceFactory.traceSource;
			}
			catch (Exception)
			{
				;
			}

			return null;
		}
		#endregion

		#region CreateTraceSource
		public static TraceLogger CreateTraceSource(string traceSourceName, int id)
		{
			return new TraceLogger(traceSourceName, id);
		}

		public static TraceLogger CreateTraceSource(string traceSourceName)
		{
			return new TraceLogger(traceSourceName);
		}

		public static TraceLogger CreateTraceSource(TraceSource ts)
		{
			return new TraceLogger(ts);
		}

		public static TraceLogger CreateTraceSource(TraceSource ts, int id)
		{
			return new TraceLogger(ts, id);
		}
		#endregion
	}
}
