using System;
using System.Data;

namespace FWBS.OMS.Caching
{
    public abstract class DataSetLocalQueryCache : IQueryCache
    {
        private string type;
        private DataSetLocalCache cache = new DataSetLocalCache();

        public DataSetLocalQueryCache(string type)
        {
            if (String.IsNullOrWhiteSpace(type))
                throw new ArgumentNullException("type");

            this.type = type;
        }

        #region IQueryCache Members

        public virtual bool Handles(Data.ExecuteTableEventArgs args)
        {
            return false;
        }

        public virtual bool Handles(FWBS.OMS.Data.ExecuteDataSetEventArgs args)
        {
            return (args.SQL.IndexOf(Type, StringComparison.InvariantCultureIgnoreCase) > -1);
        }

        public string Key
        {
            get
            {
                return "OMS:" + Type.ToUpperInvariant();
            }
        }

        protected virtual string Type
        {
            get
            {
                return type;
            }
        }

        public void Clear(string name, CacheSearch search)
        {
        }

        public DataSetLocalCache Cache
        {
            get { return cache; }
        }

        public virtual DataTable GetData(Data.ExecuteTableEventArgs args)
        {
            return null;
        }

        public virtual DataSet GetData(Data.ExecuteDataSetEventArgs args)
        {
            if (Handles(args))
            {
                string cachename = GetName(args);
                var ds = Cache.Get(Type, "", cachename);
                if (ds != null)
                {
                    return ds.Copy();
                }
            }
            return null;
        }

        public virtual void SetData(Data.ExecuteTableEventArgs args)
        {
            return;
        }

        public virtual void SetData(FWBS.OMS.Data.ExecuteDataSetEventArgs args)
        {
            if (args == null || args.Data == null)
                return;

            if (Handles(args))
            {
                string cachename = GetName(args);
                Cache.Set(Type, "", cachename, args.Data);
            }
        }

        protected virtual string GetName(FWBS.OMS.Data.ExecuteEventArgs args)
        {
            return DataTableLocalQueryCache.GetNameFromArgs(args);
        }


        #endregion
    }
}
