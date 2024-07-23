using System;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceProcess;

namespace MatterSphereBundlerWindowsService
{
    public partial class Main : ServiceBase
    {
        public Main()
        {
            InitializeComponent();
            if (!EventLog.SourceExists(_eventLog.Source))
            {
                EventLog.CreateEventSource(_eventLog.Source, _eventLog.Log);
            }
        }

        ServiceHost host;

        protected override void OnPause()
        {
            base.OnPause();
            StopService();
        }

        protected override void OnContinue()
        {
            base.OnContinue();
            StartService();
        }

        protected override void OnStart(string[] args)
        {
            StartService();
        }

        private void StartService()
        {
            LogMessage("Service Starting...");

            string port = GetSetting("Port", "");

            if (port == "")
            {
                LogErrorMessage("Port not set!", true);
                StopService();
                return;
            } 

            Uri baseAddress = new Uri(string.Format("http://localhost:{0}/MatterSphereBundlerService", port));

            host = new ServiceHost(typeof(MatterSphereBundlerWCFService.MatterSphereBundlerService), baseAddress);
            // Enable metadata publishing.
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = false;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            host.Description.Behaviors.Add(smb);

            // Open the ServiceHost to start listening for messages. Since
            // no endpoints are explicitly configured, the runtime will create
            // one endpoint per base address for each service contract implemented
            // by the service.
            host.Open();
            LogMessage(string.Format("Service Listening at {0}", baseAddress));

            LogMessage("Service Started");
        }

        protected override void OnStop()
        {
            StopService();
        }

        private void StopService()
        {
            LogMessage("Service Stopping");
            host.Close();
            LogMessage("Service Stopped");
        }

        private void LogMessage(string message)
        {
            try
            {
                _eventLog.WriteEntry(message, EventLogEntryType.Information);
            }
            catch { }
        }

        private void LogErrorMessage(string message, bool warning = false)
        {
            try
            {
                _eventLog.WriteEntry(message, warning ? EventLogEntryType.Warning : EventLogEntryType.Error);
            }
            catch { }
        }


        private const string REG_APPLICATION_KEY = "OMS";
        private const string REG_VERSION_KEY = "2.0";
        private const string REG_KEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\FWBS\OMS\2.0\MatterSphereBundlerService";



        /// <summary>
        /// Reads registry setting
        /// </summary>
        /// <param name="KeyName">Registry Key Name</param>
        /// <param name="ApplicationName">Export application or common if left empty</param>
        /// <returns></returns>
        private string GetSetting(string valueName, string defaultValue)
        {
            string val = defaultValue;
            try
            {
                object oVal = Microsoft.Win32.Registry.GetValue(REG_KEY, valueName, "NOTSET");

                if (oVal == null || Convert.ToString(oVal) == "NOTSET") //key doesn't exist or key not set
                {
                    //write the default value instead
                    UpdateSetting(valueName, defaultValue);
                    val = defaultValue;
                }
                else
                {
                    val = Convert.ToString(oVal);
                }

            }
            catch (Exception ex)
            {
                LogErrorMessage(ex.Message);
            }
            return val;
        }

        /// <summary>
        /// Updates registry setting
        /// </summary>
        /// <param name="setting">Settings name</param>
        /// <param name="app">application name</param>
        /// <param name="newValue">value of setting</param>
        private void UpdateSetting(string valueName, object newValue)
        {
            try
            {
                Microsoft.Win32.Registry.SetValue(REG_KEY, valueName, newValue);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Updating Registry " + ex.Message, ex);
            }
        }
    }
}
