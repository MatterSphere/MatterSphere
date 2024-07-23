using System;
using System.Data;
using System.IO;
using System.Linq;
using FWBS.Common;

namespace FWBS.OMS.Caching
{
    public sealed class DataTableLocalCache : BaseLocalDataCache
    {
        #region Fields

        private DataSet cache = new DataSet();
   
        #endregion

        #region Constructors

        public DataTableLocalCache()
            :base(LocalDataCacheType.Default)
        {
        }

        public DataTableLocalCache(LocalDataCacheType type)
            :base(type)
        {
        }

        #endregion

        #region Methods

        public void Set(string type, string additionalInfo, string id, DataTable dt)
        {
            if (String.IsNullOrEmpty(type))
                throw new ArgumentNullException("type");
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");
            if (dt == null)
                throw new ArgumentNullException("dt");

            dt = dt.Copy();

            Add(id, dt);

            if (CacheType == LocalDataCacheType.Full)
            {
                this.Save(type, additionalInfo, id, dt);
            }
        }

        private void Add(string name, DataTable dt)
        {
            dt.TableName = name;

            if (CacheType == LocalDataCacheType.Full)
            {
                if (cache.Tables.Contains(dt))
                    return;

                if (cache.Tables.Contains(name))
                {
                    cache.Tables.Remove(name);
                }

                cache.Tables.Add(dt);
            }
        }

        public void Remove(string name, CacheSearch search)
        {
            if (String.IsNullOrEmpty(name))
                return;

            switch (search)
            {
                case CacheSearch.Exact:
                    {
                        if (cache.Tables.Contains(name))
                        {
                            cache.Tables[name].Clear();
                            cache.Tables.Remove(name);
                        }
                    }
                    break;
                case CacheSearch.Any:
                    {
                        for (int ctr = cache.Tables.Count - 1; ctr > 0; ctr--)
                        {
                            string tablename = cache.Tables[ctr].TableName;
                            if (tablename.IndexOf(name) > -1)
                            {
                                cache.Tables[tablename].Clear();
                                cache.Tables.Remove(tablename);
                            }
                        }

                    }
                    break;
                case CacheSearch.StartsWith:
                    {
                        for (int ctr = cache.Tables.Count - 1; ctr > 0; ctr--)
                        {
                            string tablename = cache.Tables[ctr].TableName;
                            if (tablename.StartsWith(name))
                            {
                                cache.Tables[tablename].Clear();
                                cache.Tables.Remove(tablename);
                            }
                        }

                    }
                    break;
                case CacheSearch.EndsWith:
                    {
                        for (int ctr = cache.Tables.Count - 1; ctr > 0; ctr--)
                        {
                            string tablename = cache.Tables[ctr].TableName;
                            if (tablename.EndsWith(name))
                            {
                                cache.Tables[tablename].Clear();
                                cache.Tables.Remove(tablename);
                            }
                        }

                    }
                    break;
            }
        }


        public DataTable Get(string type, string additionalInfo, string id)
        {
            if (String.IsNullOrEmpty(type))
                throw new ArgumentNullException("type");
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException("id");


            if (CacheType == LocalDataCacheType.Full)
            {
                if (cache.Tables.Contains(id))
                    return cache.Tables[id];
            }

            var dt = Load(type, additionalInfo, id);

            if (dt != null)
            {
                Add(id, dt);
                return dt;
            }

            return null;
        }

        public override void Clear()
        {
            if (cache != null)
            {
                cache.Clear();
            }

            base.Clear();

        }

        #endregion


        #region Local Storage

        private FileInfo GetCacheFile(string type, string id)
        {
            var fi = new FileInfo(Path.Combine(LocalDirectory, String.Format("{0}-{1}.xml", type, GenerateHash(id))));
            if (!fi.Directory.Exists)
                fi.Directory.Create();
            return fi;
        }

        private void Save(string type, string additionalInfo, string id, DataTable dt)
        {
            var file = GetCacheFile(type, id);
            if (file.Exists)
                file.Delete();

            DateTime? lastUpdated = GetLastUpdate(type, additionalInfo);

            if (lastUpdated.HasValue)
                dt.ExtendedProperties["LastUpdate"] = lastUpdated.Value.ToBinary();
            else
                dt.ExtendedProperties["LastUpdate"] = null;

            DataSet ds = new DataSet(type);
            ds.Tables.Add(dt.Copy());
            ds.WriteXml(file.FullName, XmlWriteMode.WriteSchema);
        }

        private DataTable Load(string type, string additionalInfo, string id)
        {
            var file = GetCacheFile(type, id);

            DataTable dt = null;
            DataSet ds = new DataSet();
            if (file.Exists)
            {
                using (var strm = new FileStream(file.FullName,FileMode.Open,FileAccess.Read,FileShare.Read))
                {
                    ds.ReadXml(strm, XmlReadMode.ReadSchema);
                    dt = ds.Tables.First<DataTable>().Copy();
                }
            }
            
            if (dt != null)
            {
   
                var lastUpdated = GetLastUpdate(type, additionalInfo);

                var local = Convert.ToString(dt.ExtendedProperties["LastUpdate"]);

                if (!lastUpdated.HasValue)
                    return dt;

                // An error occured when a Table was cached and did not have a Update Date in the 
                // dbtablemonitor. Then later someone would make a change and the dbTablemonitor would
                // reflect the change. Unfortunalty the local cached was created without the lastupdate
                // and the line below would always return that cache even though the LastUpdate had changed
                // in the dbTableMonitor
                if (String.IsNullOrEmpty(local) && GetLastUpdate(type, additionalInfo) == null)
                    return dt;
                
                if (ConvertDef.ToInt64(local, DateTime.MinValue.ToBinary()) == lastUpdated.Value.ToBinary())
                    return dt;
                
                return null;
            }

            return null;
        }


     

        #endregion



    }


}
