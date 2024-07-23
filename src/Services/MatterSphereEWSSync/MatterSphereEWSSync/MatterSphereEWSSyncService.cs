using System;
using System.Diagnostics;
using System.ServiceProcess;

namespace MatterSphereEWSSync
{
    public partial class MatterSphereEWSSyncService : ServiceBase
    {
        public MatterSphereEWSSyncService()
        {
            InitializeComponent();
        }

        private string EventLogSource = "MatterSphere EWS Sync - Service Level";
        private const int EventLogError = 1;
        private const int EventLogInformation = 2;
        private const int EventLogErrorCode = 234;
        private const string EventLogType = "Application";
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

        private MatterSphereEWS.MatterSphereEWSFE MSews;
        private void GetClass()
        {
            if (MSews == null)
            {
                MSews = new MatterSphereEWS.MatterSphereEWSFE();
            }
        }
        private void UnloadClass()
        {
            if (MSews != null)
            {
                MSews = null;
            }
        }

        private void tmrTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                tmrTimer.Enabled = false;
                GetClass();
                if (MatterSphereEWS.Config.CheckDoNotProcessTimes())
                {
                    MSews.RunProcess();
                }
                bool shouldContinue = !MSews.IsCancellationRequested;
                UnloadClass();
                GC.Collect();
                tmrTimer.Enabled = shouldContinue;
            }
            catch (Exception ex)
            {
                CreateEventLogEntry("Error During Timer Run. See Next Event Log Entry for Details", 1);
                CreateEventLogEntry(ex.Message, 1);
            }
        }

        protected override void OnStart(string[] args)
        {
            tmrTimer.Interval = Convert.ToDouble(MatterSphereEWS.Config.GetConfigurationItem("ServiceTimerLength"));
            tmrTimer.AutoReset = true;
            tmrTimer.Start();
        }

        protected override void OnStop()
        {
            tmrTimer.Stop();
            var msews = MSews;
            if (msews != null)
            {
                msews.IsCancellationRequested = true;
            }
            tmrTimer.Enabled = false;
        }
    }
}
