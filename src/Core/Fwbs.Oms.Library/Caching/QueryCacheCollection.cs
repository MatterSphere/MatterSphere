using System;
using System.Collections.Generic;

namespace FWBS.OMS.Caching
{
    public sealed class QueryCacheCollection : IEnumerable<IQueryCache>
    {
        private Dictionary<Type, IQueryCache> list = new Dictionary<Type, IQueryCache>();

        public void Add<T>(T obj)
            where T : IQueryCache
        {
            if (!list.ContainsKey(typeof(T)))
                list.Add(typeof(T), obj);
        }

        public void Add<T>()
    where T : IQueryCache, new()
        {
            if (!list.ContainsKey(typeof(T)))
                list.Add(typeof(T), Activator.CreateInstance<T>());
        }

        public T Get<T>()
             where T : IQueryCache
        {
            if (list.ContainsKey(typeof(T)))
                return (T)list[typeof(T)];
            else
                return default(T);
        }

        public void Clear()
        {
            list.Clear();
        }

        #region IEnumerable<IQueryCache> Members

        public IEnumerator<IQueryCache> GetEnumerator()
        {
            return list.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return list.Values.GetEnumerator();
        }

        #endregion
    }
}
