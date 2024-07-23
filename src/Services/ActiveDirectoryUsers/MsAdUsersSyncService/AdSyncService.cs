using System;
using System.Diagnostics;
using System.Threading;
using System.Timers;
using MsAdUsersSyncService.MSADSync;
using MsAdUsersSyncService.Web;
using Nancy.Hosting.Self;
using Topshelf;
using Timer = System.Timers.Timer;

namespace MsAdUsersSyncService
{
    public class AdSyncService : ServiceControl
    {
        private readonly Timer _timer = new Timer() {Enabled = false};
        private string EventLogSource = "MatterSphere AD Sync - Service Level";
        private const int EventLogError = 1;
        private const int EventLogInformation = 2;
        private const int EventLogErrorCode = 234;
        private const string EventLogType = "Application";
        private Uri Uri;
        private ADMS _adMs;
        private readonly ManualResetEvent _manualResetEvent = new ManualResetEvent(false);

        private NancyHost _nancyHost;
        public bool Start(HostControl hostControl)
        {
            Uri = new Uri(string.Format("http://localhost:{0}", Config.GetConfigurationItem("Port")));
            HostConfiguration hostConfiguration = new HostConfiguration();
            hostConfiguration.UrlReservations.CreateAutomatically = true;

            _nancyHost = new NancyHost(Uri, new Bootstrapper(), hostConfiguration);

            _nancyHost.Start();
            Console.WriteLine("Web server running...");

            Process.Start(Uri.ToString());

            Console.WriteLine("The service has started");

            _timer.Interval = Convert.ToDouble(Config.GetConfigurationItem("ServiceTimerLength"));
            _timer.AutoReset = true;
            _timer.Elapsed += TimerOnElapsed;
            _timer.Start();
            return true;
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

        private void GetClass()
        {
            if (_adMs == null)
            {
                _adMs = new ADMS();
            }
        }
        private void UnloadClass()
        {
            _adMs = null;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                _manualResetEvent.Reset();
                _timer.Enabled = false;
                GetClass();
                if (Config.CheckDoNotProcessTimes())
                {
                    _adMs.RunProcess();
                }

                UnloadClass();
                _timer.Enabled = true;
            }
            catch (Exception ex)
            {
                CreateEventLogEntry("Error During Timer Run. See Next Event Log Entry for Details", 1);
                CreateEventLogEntry(ex.Message, 1);
            }
            finally
            {
                _manualResetEvent.Set();
            }
        }
        
        public bool Stop(HostControl hostControl)
        {
            Console.WriteLine("The service has been stopped");

            while (!_manualResetEvent.WaitOne())
            {
                Thread.Sleep(1000);
            }

            _timer.Enabled = false;
            _timer.Stop();
            _nancyHost.Stop();
            return true;
        }
    }
}