using System;
using System.Data;
using System.IO;
using FWBS.Common;

namespace FWBS.OMS.Caching
{
    public sealed class DataSetLocalCache : BaseLocalDataCache
    {
        #region Fields

        private DataSet cache = null;

        #endregion

        #region Constructors

        public DataSetLocalCache()
            : base(LocalDataCacheType.Default)
        {
        }

        public DataSetLocalCache(LocalDataCacheType type)
            : base(type)
        {
        }

        #endregion

        #region Methods

        public void Set(string type, string additionalData, string id, DataSet ds)
        {
            if (String.IsNullOrEmpty(type))
                throw new ArgumentNullException("type");
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            cache = ds.Copy();

            Add(type, cache);

            if (CacheType == LocalDataCacheType.Full)
            {
                this.Save(type, additionalData, id, ds);
            }
        }

        private void Add(string name, DataSet ds)
        {
            if (CacheType == LocalDataCacheType.Full)
            {
                cache.DataSetName = name;
            }
        }

        public DataSet Get(string type, string additionalData, string id)
        {
            if (String.IsNullOrEmpty(type))
                throw new ArgumentNullException("type");
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");

            if (CacheType == LocalDataCacheType.Full)
            {
                if (cache != null)
                {
                    return cache;
                }
            }

            cache = Load(type, additionalData, id);

            if (cache != null)
            {
                Add(type, cache);
                return cache;
            }
            
            return null;

        }

        public override void Clear()
        {
            if (cache != null)
            {
                cache.Clear();
                cache.Dispose();
                cache = null;
            }

            base.Clear();
        }


        private FileInfo GetCacheFile(string type, string id)
        {
            var fi = new FileInfo(Path.Combine(LocalDirectory, String.Format("{0}-{1}.xml", type, GenerateHash(id))));
            if (!fi.Directory.Exists)
                fi.Directory.Create();
            return fi;
        }

        private void Save(string type, string additionalInfo, string id, DataSet ds)
        {
            var file = GetCacheFile(type, id);
            if (file.Exists)
                file.Delete();

            var lastUpdated = GetLastUpdate(type, additionalInfo);

            if (lastUpdated != null)
                cache.ExtendedProperties["LastUpdate"] = lastUpdated.Value.ToBinary();
            else
                cache.ExtendedProperties["LastUpdate"] = null;

            cache.DataSetName = type;
            cache.WriteXml(file.FullName, XmlWriteMode.WriteSchema);
        }

        private DataSet Load(string type, string additionalInfo, string id)
        {
            DataSet ds = null;

            var filename = GetCacheFile(type, id);

            if (filename.Exists)
            {
                using (FileStream file = new FileStream(filename.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    ds = new DataSet();
                    ds.ReadXml(file, XmlReadMode.ReadSchema);
                    ds.DataSetName = type;
                }
            }

            if (ds == null)
                return null;

            var lastUpdated = GetLastUpdate(type, additionalInfo);
            
            var local = Convert.ToString(ds.ExtendedProperties["LastUpdate"]);

            if (String.IsNullOrEmpty(local) && lastUpdated == null)
                return ds;
            else if (ConvertDef.ToInt64(local, DateTime.MinValue.ToBinary()) == lastUpdated.Value.ToBinary())
                return ds;
            else
                return null;
        }



        #endregion


    }


}
