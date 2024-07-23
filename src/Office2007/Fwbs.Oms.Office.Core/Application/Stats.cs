using System;
using System.Collections.Generic;

namespace Fwbs.Framework
{

    public static class Stats
    {
        private static readonly Dictionary<Type, int> types = new Dictionary<Type, int>();


        
        public static void Increment(Type type)
        {
            if (type == null)
                return;

            lock(types)
            {
                if (!types.ContainsKey(type))
                    types.Add(type, 0);
                types[type]++;
            }
        }

    
        public static void Decrement(Type type)
        {
            if (type == null)
                return;

            lock (types)
            {
                if (types.ContainsKey(type))
                    types[type]--;
                else
                { }
            }
        }

        internal static int GetCount(Type type)
        {
            if (type == null)
                return 0;
            lock (types)
            {
                if (types.ContainsKey(type))
                    return types[type];

                return 0;
            }
        }
    }
}
