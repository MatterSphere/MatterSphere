using System.Reflection;

namespace MSCacheMgr
{
    using FWBS.OMS;

    static class AppData
    {
        public static string RootCachePath
        {
            get
            {
                return Global.GetAppDataPath();
            }
        }

        public static FWBS.OMS.Data.DatabaseConnections DbConnections
        {
            get
            {
                return new FWBS.OMS.Data.DatabaseConnections(Global.ApplicationName, Global.ApplicationKey, Global.VersionKey);
            }
        }

        public static string GetDBAppDataPath(FWBS.OMS.Data.DatabaseSettings dbSettings)
        {
            FieldInfo multidb = typeof(Session).GetField("_multidb", BindingFlags.NonPublic | BindingFlags.Instance);
            multidb.SetValue(Session.CurrentSession, dbSettings);
            string dbAppDataPath = Global.GetDBAppDataPath();
            multidb.SetValue(Session.CurrentSession, null);
            return dbAppDataPath.TrimEnd('\\');
        }
    }
}
