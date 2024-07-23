using System;

namespace FWBS.OMS
{
    static class Global
    {
        private static readonly string _appname = Branding.GetApplicationName();

        public static string ApplicationName
        {
            get { return _appname; }
        }

        public static string ApplicationKey
        {
            get { return Common.Global.ApplicationKey; }
        }

        public static string VersionKey
        {
            get { return Common.Global.VersionKey; }
        }

        public static string RegistryRes(string code, string defaultValue)
        {
            string culture = Convert.ToString(new Common.Reg.ApplicationSetting(ApplicationKey, VersionKey, "UICulture", "OverrideUI").GetSetting(System.Threading.Thread.CurrentThread.CurrentUICulture.Name));
            return Convert.ToString(new Common.Reg.ApplicationSetting(ApplicationKey, VersionKey, "UICulture\\" + culture, code).GetSetting(defaultValue));
        }
    }
}
