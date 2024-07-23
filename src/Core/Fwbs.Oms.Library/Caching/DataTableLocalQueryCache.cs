using System;
using System.Data;
using System.Linq;
using System.Text;

namespace FWBS.OMS.Caching
{
    public abstract class DataTableLocalQueryCache : IQueryCache
    {
        private DataTableLocalCache cache = new DataTableLocalCache();


        #region IQueryCache Members

        public virtual bool Handles(Data.ExecuteTableEventArgs args)
        {
            return (args.SQL.IndexOf(Type, StringComparison.InvariantCultureIgnoreCase) > -1);
        }

        public virtual bool Handles(FWBS.OMS.Data.ExecuteDataSetEventArgs args)
        {
            return false;
        }

        public virtual string Key
        {
            get
            {
                return "OMS:" + Type.ToUpperInvariant();
            }
        }

        protected abstract string Type{get;}

        public void Clear(string name, CacheSearch search)
        {
            Cache.Remove(name, search);
        }

        public DataTableLocalCache Cache
        {
            get { return cache; }
        }

        public virtual DataTable GetData(Data.ExecuteTableEventArgs args)
        {
            if (Handles(args))
            {
                string cachename = GetName(args);
                var dt = Cache.Get(Type, "", cachename);
                if (dt != null)
                {
                    return dt.Copy();
                }
            }
            return null;
        }

        public virtual DataSet GetData(Data.ExecuteDataSetEventArgs args)
        {
            return null;
        }

        public virtual void SetData(Data.ExecuteTableEventArgs args)
        {
            if (args == null || args.Data == null)
                return;

            if (Handles(args))
            {
                string cachename = GetName(args);
                Cache.Set(Type, "", cachename, args.Data);
            }
        }

        public virtual void SetData(FWBS.OMS.Data.ExecuteDataSetEventArgs args)
        {
            return;
        }

        protected virtual string GetName(FWBS.OMS.Data.ExecuteEventArgs args)
        {
            return DataTableLocalQueryCache.GetNameFromArgs(args);
        }


        internal static string GetNameFromArgs(FWBS.OMS.Data.ExecuteEventArgs args)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(args.SQL);

            //Make sure that the additional parameters are also specified to generate the name.

            var pars = args.Parameters.Union(args.AdditionalParameters).ToArray();

            foreach (IDataParameter par in pars)
            {
                if (par != null)
                {
                    sb.Append("#");
                    sb.Append(par.ParameterName);
                    sb.Append("=");
                    if (par.Value == DBNull.Value || par == null)
                        sb.Append("{NULL}");
                    else
                        sb.Append(Convert.ToString(par.Value));
                }
            }

            return sb.ToString();
        }


        #endregion
    }
}
