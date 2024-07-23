using System;
using System.Configuration;
using System.Xml;

namespace DocumentArchivingClass
{
    public class DocumentArchivingConfiguration
    {
        public static bool SetConfigurationItem(string ConfigurationName, string ConfigurationValue)
        {
            bool updated = false;
            string configsetting = System.Configuration.ConfigurationManager.AppSettings.Get(ConfigurationName);
            if (configsetting != ConfigurationValue)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Environment.CurrentDirectory + "\\DocArchive.Config");
                if (xmlDoc.DocumentElement.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                    {
                        if (node.Attributes[0].Value.Equals(ConfigurationName))
                        {
                            node.Attributes[1].Value = ConfigurationValue;
                        }
                    }
                }
                xmlDoc.Save(Environment.CurrentDirectory + "\\DocArchive.Config");
                ConfigurationManager.RefreshSection("appSettings");
                string configsetting2 = System.Configuration.ConfigurationManager.AppSettings.Get(ConfigurationName);
                updated = true;
            }
            return updated;
        }
        public static bool SetConfigurationItemBool(string ConfigurationName, bool ConfigurationValue)
        {
            bool updated = false;
            string configsetting = System.Configuration.ConfigurationManager.AppSettings.Get(ConfigurationName);
            bool boolconfigsetting = Convert.ToBoolean(configsetting);
            if (boolconfigsetting != ConfigurationValue)
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Environment.CurrentDirectory + "\\DocArchive.Config");
                if (xmlDoc.DocumentElement.Name.Equals("appSettings"))
                {
                    foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
                    {
                        if (node.Attributes[0].Value.Equals(ConfigurationName))
                        {
                            node.Attributes[1].Value = Convert.ToString(ConfigurationValue);
                        }
                    }
                }
                xmlDoc.Save(Environment.CurrentDirectory + "\\DocArchive.Config");
                ConfigurationManager.RefreshSection("appSettings");
                string configsetting2 = System.Configuration.ConfigurationManager.AppSettings.Get(ConfigurationName);
                updated = true;
            }
            return updated;
        }
        public static string GetConfigurationItem(string ConfigurationName)
        {
            string configsetting;
            try
            {
                configsetting = System.Configuration.ConfigurationManager.AppSettings.Get(ConfigurationName);
            }
            catch (Exception)
            {
                configsetting = string.Empty;
            }

            return configsetting;
        }
        public static bool GetConfigurationItemBool(string ConfigurationName)
        {
            string configsetting;
            bool boolconfigsetting;
            try
            {
                configsetting = System.Configuration.ConfigurationManager.AppSettings.Get(ConfigurationName);
                boolconfigsetting = Convert.ToBoolean(configsetting);
            }
            catch (Exception)
            {
                boolconfigsetting = false;
            }

            return boolconfigsetting;
        }
        public static bool CheckDoNotProcessTimes()
        {
            bool AllowProcessToRun = true;
            if (GetConfigurationItemBool("DoNotProcessEnabled"))
            {
                TimeSpan start1 = TimeSpan.Parse(GetConfigurationItem("DoNotProcessStart1"));
                TimeSpan end1 = TimeSpan.Parse(GetConfigurationItem("DoNotProcessEnd1"));
                TimeSpan start2 = TimeSpan.Parse(GetConfigurationItem("DoNotProcessStart2"));
                TimeSpan end2 = TimeSpan.Parse(GetConfigurationItem("DoNotProcessEnd2"));
                TimeSpan timeNow = DateTime.Now.TimeOfDay;

                if ((timeNow > start1 && timeNow < end1) || (timeNow > start2 && timeNow < end2))
                {
                    AllowProcessToRun = false;
                }
            }
            return AllowProcessToRun;
        }
    }
}
