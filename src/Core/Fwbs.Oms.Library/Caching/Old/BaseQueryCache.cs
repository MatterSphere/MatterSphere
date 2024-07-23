using System.Data;

namespace FWBS.OMS.Caching
{
    public abstract class BaseQueryCache : IQueryCache
    {
        #region IQueryCache Members

        public abstract bool Handles(FWBS.OMS.Data.ExecuteTableEventArgs args);

        public virtual bool Handles(FWBS.OMS.Data.ExecuteDataSetEventArgs args)
        {
            return false;
        }

        protected abstract DataTableCache Cache { get;}

        public void Clear(string name, CacheSearch search)
        {
            Cache.Remove(name, search);
        }

        public virtual System.Data.DataTable GetData(FWBS.OMS.Data.ExecuteTableEventArgs args)
        {
            if (Handles(args))
            {
                string cachename = GetName(args);
                DataTable dt = Cache.Get(cachename);
                if (dt != null)
                {
                    return dt.Copy();
                }
            }
            return null;
        }

        public virtual System.Data.DataSet GetData(FWBS.OMS.Data.ExecuteDataSetEventArgs args)
        {
            if (Handles(args))
            {
                //TODO: Get DataSet
                return null;
            }
            return null;
        }

        public virtual void SetData(FWBS.OMS.Data.ExecuteTableEventArgs args)
        {
            if (Handles(args))
            {
                string cachename = GetName(args);
                Cache.Set(cachename, args.Data);
            }
        }

        public virtual void SetData(FWBS.OMS.Data.ExecuteDataSetEventArgs args)
        {
            //TODO: Set DataSet
            return;
        }

        protected virtual string GetName(FWBS.OMS.Data.ExecuteEventArgs args)
        {
            return DataTableLocalQueryCache.GetNameFromArgs(args);
        }

        #endregion
    }
}
