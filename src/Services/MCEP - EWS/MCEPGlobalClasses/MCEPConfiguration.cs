using System;

namespace MCEPGlobalClasses
{
    public class MCEPConfiguration
    {
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

                if ((timeNow > start1 && timeNow < end1) || (timeNow >start2 && timeNow < end2))
                {
                    AllowProcessToRun = false;
                }
             }
            return AllowProcessToRun;
        }
    }
}
