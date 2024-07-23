using System;
using System.Configuration;
using System.Xml;

namespace MsAdUsersSyncService.MSADSync
{
    public class Config
    {
        public static bool SetConfigurationItem(string configurationName, string configurationValue)
        {
            bool updated = false;

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string configSetting = ConfigurationManager.AppSettings.Get(configurationName);

            if (!configSetting.Equals(configurationValue))
            {
                config.AppSettings.Settings[configurationName].Value = configurationValue;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
                updated = true;
            }
            return updated;
        }
        public static bool SetConfigurationItemBool(string configurationName, bool configurationValue)
        {
            bool updated = false;
            string configsetting = System.Configuration.ConfigurationManager.AppSettings.Get(configurationName);
            bool boolconfigsetting = Convert.ToBoolean(configsetting);
            if (boolconfigsetting != configurationValue)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Environment.CurrentDirectory + "\\ADSync.Config");
                if (xmlDoc.DocumentElement.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                    {
                        if (node.Attributes[0].Value.Equals(configurationName))
                        {
                            node.Attributes[1].Value = Convert.ToString(configurationValue);
                        }
                    }
                }
                xmlDoc.Save(Environment.CurrentDirectory + "\\ADSync.Config");
                ConfigurationManager.RefreshSection("appSettings");
                string configsetting2 = System.Configuration.ConfigurationManager.AppSettings.Get(configurationName);
                updated = true;
            }
            return updated;
        }
        public static string GetConfigurationItem(string configurationName)
        {
            var logging = new Logging("MatterSphere AD Sync");
            try
            {
                var settings = ConfigurationManager.AppSettings;
                if (settings != null)
                {
                    string value = settings.Get(configurationName);
                    logging.CreateLogEntry(string.Format("{0} = {1}", configurationName, value));
                    return value;
                }
                else
                {
                    throw new NullReferenceException("Settings = null");
                }
            }
            catch (Exception e)
            {
                logging.CreateErrorEntry(e.Message);
                return e.Message;
            }
        }
        public static bool GetConfigurationItemBool(string configurationName)
        {
            try
            {
                var configsetting = ConfigurationManager.AppSettings.Get(configurationName);
                return Convert.ToBoolean(configsetting);
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool CheckDoNotProcessTimes()
        {
            bool allowProcessToRun = true;
            if (GetConfigurationItemBool("DoNotProcessEnabled"))
            {
                TimeSpan start1 = TimeSpan.Parse(GetConfigurationItem("DoNotProcessStart1"));
                TimeSpan end1 = TimeSpan.Parse(GetConfigurationItem("DoNotProcessEnd1"));
                TimeSpan start2 = TimeSpan.Parse(GetConfigurationItem("DoNotProcessStart2"));
                TimeSpan end2 = TimeSpan.Parse(GetConfigurationItem("DoNotProcessEnd2"));
                TimeSpan timeNow = DateTime.Now.TimeOfDay;

                if ((timeNow > start1 && timeNow < end1) || (timeNow > start2 && timeNow < end2))
                {
                    allowProcessToRun = false;
                }
            }
            return allowProcessToRun;
        }
    }
}
