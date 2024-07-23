#region References
using System;
using System.Diagnostics;
#endregion References

namespace ELPLServicesLibrary
{
    class CommonMethods
    {
        #region Constants
        const string EVENT_SOURCE = "ELPLServicesLibrary.dll";
        #endregion Constants

        #region Methods

        #region LogMessage
        public static void LogMessage(string message, EventLogEntryType logEntryType)
        {
            try
            {
                using (EventLog evt = new EventLog("Application", Environment.MachineName, EVENT_SOURCE))
                    evt.WriteEntry(message, logEntryType);
            }
            catch { }
        }
        #endregion LogMessage

        #endregion Methods
    }
}
