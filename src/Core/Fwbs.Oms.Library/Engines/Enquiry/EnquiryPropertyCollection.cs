using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace FWBS.OMS.EnquiryEngine
{
    public sealed class EnquiryPropertyCollection : IEnumerable<EnquiryProperty>
    {
        public EnquiryPropertyCollection(Type type, bool all)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            Populate(type, all);
        }

        public EnquiryPropertyCollection(object obj, bool all)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            Populate(obj, all);
        }

        private Dictionary<string, EnquiryProperty> props = new Dictionary<string, EnquiryProperty>();


        public EnquiryProperty this[string name]
        {
            get
            {
                EnquiryProperty prop;
                if (props.TryGetValue(name, out prop))
                    return prop;
                else
                    return null;
            }
        }

        public int Count
        {
            get
            {
                return props.Count;
            }
        }


        #region IEnumerable<EnquiryProperty> Members

        public IEnumerator<EnquiryProperty> GetEnumerator()
        {
            return props.Values.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return props.Values.GetEnumerator();
        }

        #endregion

        private void Populate(Type type, bool all)
        {
            props.Clear();

            ICustomTypeDescriptor td = new LookupTypeDescriptor(type);
            PropertyDescriptorCollection tdprops = td.GetProperties(null);
            MemberInfo[] rprops = type.FindMembers(MemberTypes.Property, Enquiry.MemberBinding, new MemberFilter(Enquiry.MemberFilter), all);


            foreach (PropertyDescriptor tdprop in tdprops)
            {
                if (!props.ContainsKey(tdprop.Name))
                {
                    Attribute[] attrs = new Attribute[tdprop.Attributes.Count];
                    tdprop.Attributes.CopyTo(attrs, 0);
                    if (all || Enquiry.HasEnquiryUsageAttribute(attrs))
                    {
                        props.Add(tdprop.Name, new EnquiryProperty(null, null, tdprop));
                    }
                }
            }

            foreach (PropertyInfo pi in rprops)
            {
                if (!props.ContainsKey(pi.Name))
                    props.Add(pi.Name, new EnquiryProperty(null, pi, null));
            }
        }

        private void Populate(object obj, bool all)
        {
            ICustomTypeDescriptor td = new LookupTypeDescriptor(obj);
            PropertyDescriptorCollection tdprops = td.GetProperties(null);
            MemberInfo[] rprops = obj.GetType().FindMembers(MemberTypes.Property, Enquiry.MemberBinding, new MemberFilter(Enquiry.MemberFilter), all);


            foreach (PropertyDescriptor tdprop in tdprops)
            {
                if (!props.ContainsKey(tdprop.Name))
                {
                    Attribute[] attrs = new Attribute[tdprop.Attributes.Count];
                    tdprop.Attributes.CopyTo(attrs, 0);
                    if (all || Enquiry.HasEnquiryUsageAttribute(attrs))
                    {
                        props.Add(tdprop.Name, new EnquiryProperty(obj, null, tdprop));
                    }
                }
            }

            foreach (PropertyInfo pi in rprops)
            {
                if (!props.ContainsKey(pi.Name))
                    props.Add(pi.Name, new EnquiryProperty(obj, pi, null));
            }
        }

    }
}
