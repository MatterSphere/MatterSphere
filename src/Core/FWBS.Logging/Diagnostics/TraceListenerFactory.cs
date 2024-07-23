#region References
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
#endregion

namespace FWBS.Logging
{
    public class TraceListenerFactory
	{
		#region Fields
		private static ConcurrentDictionary<Type, object> traceListeners = new ConcurrentDictionary<Type, object>();
		#endregion

		#region Constructor - class
		static TraceListenerFactory()
		{
			// add rolling file listener as default
			TraceListenerFactory.traceListeners[typeof(RollingFileTraceListener)] = new RollingFileTraceListener();
		}
		#endregion

		#region Constructor - instance
		private TraceListenerFactory()
		{
		}
		#endregion

		#region SharedTraceListener
		static public TraceListener SharedTraceListener(Type listenerType)
		{
			TraceListener tl = null;
			try
			{
				if (!TraceListenerFactory.traceListeners.ContainsKey(listenerType))
				{
					object obj  = Activator.CreateInstance(listenerType);
					if (obj != null)
					{
						tl = obj as TraceListener;
					}
				}
			}
			catch (Exception)
			{
				tl = null;
			}

			return tl;
		}
		#endregion
	}
}
