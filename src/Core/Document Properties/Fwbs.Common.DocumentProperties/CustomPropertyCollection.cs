using System;
using System.Collections.Generic;

namespace Fwbs.Documents
{
    public sealed class CustomPropertyCollection : IEnumerable<CustomProperty>
    {
        private readonly List<CustomProperty> propsbyindex = new List<CustomProperty>();
        private readonly Dictionary<string, CustomProperty> props = new Dictionary<string, CustomProperty>();


        #region Methods

        public CustomProperty Add(string name)
        {
            return Add(null, name);
        }

        public CustomProperty Add(string ns, string name)
        {
            UCase(ref ns);
            UCase(ref name);
            CustomProperty prop = new CustomProperty(this, ns, name);
            props.Add(prop.FullName, prop);
            propsbyindex.Add(prop);
            return prop;
        }

        public bool Contains(string name)
        {
            UCase(ref name);
            return props.ContainsKey(name);
        }

        internal void Clear()
        {
            props.Clear();
            propsbyindex.Clear();
        }

        public void Delete(string name)
        {
            UCase(ref name);
            CustomProperty prop = this[name];
            prop.Delete();
        }


        private static void UCase(ref string name)
        {
            if (name == null)
                name = String.Empty;
            else
                name = name.ToUpperInvariant();
        }

        #endregion

        #region Properties

        public bool HasChanged
        {
            get
            {
                foreach (CustomProperty prop in propsbyindex)
                {
                    if (prop.HasChanged)
                        return true;
                }

                return false;
            }
        }

        public void Accept()
        {
            for (int ctr = propsbyindex.Count - 1; ctr >= 0; ctr--)
            {
                CustomProperty prop = propsbyindex[ctr];
                if (prop.IsDeleted)
                {
                    propsbyindex.Remove(prop);
                    props.Remove(prop.FullName);
                }
                prop.Accept();
            }
        }

        public void Cancel()
        {
            foreach(CustomProperty prop in propsbyindex)
            {
                prop.Cancel();
            }
        }

        public int Count
        {
            get
            {
                return propsbyindex.Count;
            }
        }

        #endregion

        #region Indexers

        public CustomProperty this[string name]
        {
            get
            {
                UCase(ref name);

                if (!props.ContainsKey(name))
                {
                    Add(name).Accept();
                }
                return props[name];
            }
        }

        public CustomProperty this[int index]
        {
            get
            {
                return propsbyindex[index];
            }
        }

        #endregion

        #region IEnumerable<CustomProperty> Members

        public IEnumerator<CustomProperty> GetEnumerator()
        {
            return propsbyindex.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
