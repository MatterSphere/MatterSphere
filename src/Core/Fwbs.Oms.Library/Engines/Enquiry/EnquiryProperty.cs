using System;
using System.ComponentModel;
using System.Reflection;

namespace FWBS.OMS.EnquiryEngine
{
    public sealed class EnquiryProperty
    {

        internal EnquiryProperty(object obj, PropertyInfo propinfo, PropertyDescriptor propdesc)
        {
            this.obj = obj;
            this.propinfo = propinfo;
            this.propdesc = propdesc;
        }

        private object obj;
        private PropertyInfo propinfo;
        private PropertyDescriptor propdesc;

        public string Name
        {
            get
            {
                if (propinfo == null && propdesc == null)
                    return String.Empty;

                if (propinfo == null)
                    return propdesc.Name;

                return propinfo.Name;
            }
        }

        public Type PropertyType
        {
            get
            {
                if (!IsValid)
                    return typeof(string);

                if (propinfo == null)
                    return propdesc.PropertyType;

                return propinfo.PropertyType;
            }
        }

        public bool IsValid
        {
            get
            {
                return !(propinfo == null && propdesc == null);
            }
        }

        public void SetValue(object val)
        {
            if (!IsValid)
                throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("PRPDSNTEXT", "Property does not exist", "").Text);
            if (obj == null)
                throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("NOINSTSET", "No instance set.", "").Text);

            if (propinfo == null)
                propdesc.SetValue(obj, val);
            else
                propinfo.SetValue(obj, val, null);
        }

        public object GetValue()
        {
            if (!IsValid)
                throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("PRPDSNTEXT", "Property does not exist", "").Text);
            if (obj == null)
                throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("NOINSTSET", "No instance set.", "").Text);

            if (propinfo == null)
                return propdesc.GetValue(obj);
            else
                return propinfo.GetValue(obj, null);
        }

        public bool CanWrite
        {
            get
            {
                if (!IsValid)
                    return false;

                if (propinfo == null)
                    return !propdesc.IsReadOnly;
                else
                    return propinfo.CanWrite;
            }
        }

        public bool CanRead
        {
            get
            {
                if (!IsValid)
                    return false;

                if (propinfo == null)
                    return true;
                else
                    return propinfo.CanRead;
            }
        }
    }
}
