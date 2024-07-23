using System;
using System.Data;

namespace FWBS.OMS.Caching
{
    public enum CacheSearch
    {
        Exact = 0,
        StartsWith,
        EndsWith,
        Any
    }

    public sealed class DataTableCache : IDisposable, ICacheable
    {
        #region Fields

        private DataSet cache;

        #endregion

        #region Constructors

        public DataTableCache()
        {
        }

        #endregion

        #region Methods

        public void Set(string name, DataTable dt)
        {
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (dt == null)
                throw new ArgumentNullException("dt");

            if (cache == null)
                cache = new DataSet();

            DataTable copy = dt.Copy();
            copy.TableName = name;

            if (cache.Tables.Contains(name))
            {
                cache.Tables[name].Clear();
                cache.Tables.Remove(name);
            }

            cache.Tables.Add(copy);
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

        public bool Exists(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            if (cache == null)
                return false;

            return cache.Tables.Contains(name);
        }

        public DataTable Get(string name)
        {
            if (cache == null)
                return null;

            if (cache.Tables.Contains(name))
                return cache.Tables[name];
            else
                return null;
        }

        public void Clear()
        {
            if (cache != null)
            {
                cache.Clear();
                cache.Dispose();
                cache = null;
            }
        }

        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Clear();
        }

        #endregion
    }
}
