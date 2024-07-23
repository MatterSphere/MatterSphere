using System;
using MsAdUsersSyncService.MSADSync;

namespace MsAdUsersSyncService.Web
{
    [Serializable]
    public class ConfigInfo
    {
        public ConfigInfo()
        {
            
        }
        public int ServiceTimerLength { get; set; }
        public bool LogToFileEnabled { get; set; }
        public bool LogAllToEventLog { get; set; }
        public string MatterSphereServer { get; set; }
        public string MatterSphereDatabase { get; set; }
        public string MatterSphereLoginType { get; set; }
        public TimeSpan DoNotProcessStart1 { get; set; }
        public TimeSpan DoNotProcessEnd1 { get; set; }
        public TimeSpan DoNotProcessStart2 { get; set; }
        public TimeSpan DoNotProcessEnd2 { get; set; }
        public bool DoNotProcessEnabled { get; set; }
        public string NetbiosSourceUserName { get; set; }
        public string NetbiosTargetUserName { get; set; }

        public void LoadSettings()
        {
            ServiceTimerLength = Convert.ToInt32(Config.GetConfigurationItem("ServiceTimerLength"));
            LogAllToEventLog = bool.Parse(Config.GetConfigurationItem("LogAllToEventLog"));
            LogToFileEnabled = bool.Parse(Config.GetConfigurationItem("LogToFileEnabled"));
            MatterSphereServer = Config.GetConfigurationItem("MatterSphereServer");
            MatterSphereDatabase = Config.GetConfigurationItem("MatterSphereDatabase");
            MatterSphereLoginType = Config.GetConfigurationItem("MatterSphereLoginType");
            DoNotProcessStart1 =  TimeSpan.Parse(Config.GetConfigurationItem("DoNotProcessStart1"));
            DoNotProcessEnd1 = TimeSpan.Parse(Config.GetConfigurationItem("DoNotProcessEnd1"));
            DoNotProcessStart2 = TimeSpan.Parse(Config.GetConfigurationItem("DoNotProcessStart2"));
            DoNotProcessEnd2 = TimeSpan.Parse(Config.GetConfigurationItem("DoNotProcessEnd2"));
            DoNotProcessEnabled = bool.Parse(Config.GetConfigurationItem("DoNotProcessEnabled"));
            NetbiosSourceUserName = Config.GetConfigurationItem("NetbiosSourceUserName");
            NetbiosTargetUserName = Config.GetConfigurationItem("NetbiosTargetUserName");
        }

        public void SaveSettings()
        {
            Config.SetConfigurationItem("ServiceTimerLength", ServiceTimerLength.ToString());
            Config.SetConfigurationItem("LogAllToEventLog", LogAllToEventLog.ToString());
            Config.SetConfigurationItem("LogToFileEnabled", LogToFileEnabled.ToString());
            Config.SetConfigurationItem("MatterSphereServer", MatterSphereServer);
            Config.SetConfigurationItem("MatterSphereDatabase", MatterSphereDatabase);
            Config.SetConfigurationItem("MatterSphereLoginType", MatterSphereLoginType);
            Config.SetConfigurationItem("DoNotProcessStart1", DoNotProcessStart1.ToString());
            Config.SetConfigurationItem("DoNotProcessEnd1", DoNotProcessEnd1.ToString());
            Config.SetConfigurationItem("DoNotProcessStart2", DoNotProcessStart2.ToString());
            Config.SetConfigurationItem("DoNotProcessEnd2", DoNotProcessEnd2.ToString());
            Config.SetConfigurationItem("DoNotProcessEnabled", DoNotProcessEnabled.ToString());
            Config.SetConfigurationItem("NetbiosSourceUserName", NetbiosSourceUserName);
            Config.SetConfigurationItem("NetbiosTargetUserName", NetbiosTargetUserName);
        }
    }
}