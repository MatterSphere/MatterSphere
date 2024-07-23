using System;

namespace Fwbs.Documents
{
    public class CustomProperty
    {
        internal CustomProperty(CustomPropertyCollection parent, string ns, string name)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");

            if (ns == null)
                ns = String.Empty;

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.ns = ns;
            this.name = name;
        }

        private readonly string ns;
        public string Namespace
        {
            get
            {
                return ns;
            }
        }

        private readonly string name;
        public string Name
        {
            get
            {
                return name;
            }
        }

        public string FullName
        {
            get
            {
                if (String.IsNullOrEmpty(ns))
                    return name;
                else
                    return ns + "." + name;
            }
        }
        

        private object val;
        public object Value
        {
            get
            {
                return val;
            }
            set
            {
                if (value == null || Convert.IsDBNull(value))
                    val = null;
                else
                    val = value;
            }
        }

        private object originalval;

        public bool IsDeleted
        {
            get
            {
                return (val == null);
            }
        }

        public bool HasChanged
        {
            get
            {
                return (originalval != val);
            }
        }

        internal void Cancel()
        {
            val = originalval;
        }

        public void Accept()
        {
            originalval = val;
        }

        public void Delete()
        {
            val = null;
        }

    }

}
