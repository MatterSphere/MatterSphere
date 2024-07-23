using System;
using System.Diagnostics;

namespace MSCacheMgr
{
    static class Logging
    {
        internal static void LogEvent(string message, EventLogEntryType type)
        {
            try
            {
                using (EventLog evt = new EventLog("Application", Environment.MachineName, "MSCacheMgr"))
                    evt.WriteEntry(message, type);
            }
            catch { }
        }
    }
}
