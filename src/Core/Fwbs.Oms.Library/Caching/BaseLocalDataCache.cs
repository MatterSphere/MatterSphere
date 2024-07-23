using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using FWBS.Common;

namespace FWBS.OMS.Caching
{
    public enum LocalDataCacheType
    {
        Default = 0,
        Full,
        Active,
    }

    public abstract class BaseLocalDataCache : IDisposable, ICacheable
    {
        #region Fields

        private DirectoryInfo _localstoragecachefolder;
        private Dictionary<string, DateTime?> _lastupdatedcache = new Dictionary<string, DateTime?>(StringComparer.OrdinalIgnoreCase);
        private LocalDataCacheType _type;
        private static List<MonitoredCacheItem> _itemstomonitor;

        #endregion

        #region Constructors

        public BaseLocalDataCache(LocalDataCacheType type)
        {
            _lastupdatedcache = new Dictionary<string, DateTime?>();
            _localstoragecachefolder = new DirectoryInfo(System.IO.Path.Combine(System.IO.Path.Combine(Global.GetCachePath(), "Queries")));
            if (_localstoragecachefolder.Exists == false)
                _localstoragecachefolder.Create();

            switch (type)
            {
                case LocalDataCacheType.Default:
                    {
                        if (EnableFullQueryCaching)
                            _type = LocalDataCacheType.Full;
                        else
                            _type = LocalDataCacheType.Active;
                    }
                    break;
                case LocalDataCacheType.Full:
                    _type = LocalDataCacheType.Full;
                    break;
                case LocalDataCacheType.Active:
                    _type = LocalDataCacheType.Active;
                    break;
                default:
                    throw new NotSupportedException(String.Format(Session.CurrentSession.Resources.GetMessage("MSGTPISNTSUPP", "''%0%'' is not supported.", "", type.ToString()).Text));
            }
        }

        #endregion

        #region Static

        private bool EnableFullQueryCaching
        {
            get
            {
                var reg = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Memory", "EnableFullQueryCaching", true);

                return reg.ToBoolean();
            }
        }

        protected static string GenerateHash(string text)
        {
            var enc = new System.Text.UnicodeEncoding();
            var data = enc.GetBytes(text);

            using (var sha1 = new System.Security.Cryptography.Crc32())
            {
                var buff = new System.Text.StringBuilder();
                var hash = sha1.ComputeHash(data);
                foreach (byte hashByte in hash)
                {
                    buff.Append(hashByte.ToString("x2"));
                }
                return buff.ToString();
            }

        }

        #endregion

        #region ICacheable

        public virtual void Clear()
        {
            if (_itemstomonitor != null)
            {
                _itemstomonitor.Clear();
                _itemstomonitor = null;
            }
            _lastupdatedcache.Clear();
        }

        #endregion

        #region Properties

        public LocalDataCacheType CacheType
        {
            get
            {
                return _type;
            }
        }

        protected string LocalDirectory
        {
            get
            {
                return _localstoragecachefolder.FullName;
            }
        }

        #endregion

        #region Methods

        protected DateTime? GetLastUpdate(string type, string additionalInfo)
        {
            DateTime? lastupdated;

            string key = string.Format("{0},{1}", type, additionalInfo);

            if (_lastupdatedcache.TryGetValue(key, out lastupdated))
                return lastupdated;

            var monitor = GetMonitoredItem(type, additionalInfo);

            if (monitor == null)
                return null;

            return monitor.LastUpdated;
        }


        private MonitoredCacheItem GetMonitoredItem(string type, string additionalInfo)
        {
            switch (CacheType)
            {
                case LocalDataCacheType.Full:
                    {
                        if (_itemstomonitor == null)
                        {
                            _itemstomonitor = new List<MonitoredCacheItem>();

                            var monitor = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT TableName, Category, LastUpdated FROM dbTableMonitor", "Monitor", null);
                            foreach (DataRow item in monitor.Rows)
                            {
                                _itemstomonitor.Add(new MonitoredCacheItem(Convert.ToString(item["TableName"]), Convert.ToString(item["Category"]), Convert.ToDateTime(item["LastUpdated"])));
                            }
                        }

                        return _itemstomonitor.FirstOrDefault<MonitoredCacheItem>(n => n.Type.Equals(type, StringComparison.InvariantCultureIgnoreCase) && (n.AdditionalInfo ?? String.Empty).Equals(additionalInfo ?? String.Empty, StringComparison.OrdinalIgnoreCase));

                    }
                case LocalDataCacheType.Active:
                    {

                        var parameters = new List<IDataParameter>();

                        string key = string.Format("{0},{1}", type, additionalInfo);

                        parameters.Add(Session.CurrentSession.Connection.AddParameter("Type", type));
                        parameters.Add(Session.CurrentSession.Connection.AddParameter("AdditionalInfo", additionalInfo));

                        using (var dt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT LastUpdated FROM dbTableMonitor WHERE TableName = @Type AND Category = @AdditionalInfo", "Monitor", parameters.ToArray()))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                var updated = dt.Rows.First<DataRow>()["LastUpdated"];
                               
                                if (updated != DBNull.Value)
                                {
                                    _lastupdatedcache[key] = (DateTime)updated;
                                    return new MonitoredCacheItem(type, additionalInfo, (DateTime)updated);                                    
                                }

                            }
                        }

                        return null;
                    }
                default:
                    return null;
            }
        }



        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Clear();
        }

        #endregion

        private sealed class MonitoredCacheItem
        {
            public string Type { get; private set; }
            public string AdditionalInfo { get; private set; }
            public DateTime LastUpdated { get; private set; }

            public MonitoredCacheItem(string type, string additionalInfo, DateTime lastUpdated)
            {
                Type = type;
                AdditionalInfo = additionalInfo ?? String.Empty;
                LastUpdated = lastUpdated;
            }
        }

    }


}
