using System;

namespace Fwbs.Office.Outlook
{
    using Redemption;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public sealed class RedemptionUserProperty : MSOutlook.UserProperty
    {
        #region Fields

        private readonly RedemptionUserProperties parent;
        private readonly string name;
        private MSOutlook.OlUserPropertyType type;
        private int propid;
        private readonly RDOFolderField field;
        private object currentValue;

        #endregion

        #region Constructors

        public RedemptionUserProperty(RedemptionUserProperties parent, string name, int propid, MSOutlook.OlUserPropertyType type, RDOFolderField field)
        {
            if (parent == null)
                throw new ArgumentNullException("parent");
            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            this.parent = parent;
            this.name = name;
            this.propid = propid;
            this.type = type;
            this.field = field;
            this.currentValue = DBNull.Value;
        }

        #endregion

        #region UserProperty Members

        public MSOutlook.Application Application
        {
            get { return parent.Application; }
        }

        public MSOutlook.OlObjectClass Class
        {
            get { return MSOutlook.OlObjectClass.olUserProperty; }
        }

        public void Delete()
        {
            Value = null; //A combination of setting Value to null and parent.Remove are responsible for the deletion of the property
            parent.Remove(this);
        }

        public string Formula
        {
            get
            {
                if (field == null)
                    return String.Empty;
                return field.Formula;
            }
            set
            {
                if (field == null)
                    throw new NotSupportedException();

                field.Formula = value;
            }
        }

        public bool IsUserProperty
        {
            get { return true; }
        }

        public string Name
        {
            get { return name; }
        }

        public object Parent
        {
            get { return parent; }
        }

        public MSOutlook.NameSpace Session
        {
            get { return Application.Session; }
        }

        public MSOutlook.OlUserPropertyType Type
        {
            get
            {
                if (field != null)
                    return field.Type.ToOutlookType();
                else
                {
                    return type;
                }
            }
        }

        public string ValidationFormula
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public string ValidationText
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public object Value
        {
            get
            {
                if (currentValue == DBNull.Value)
                {
                    var pid = Helpers.ToPropId(propid, Type.ToPT());
                    //Setting StorageItem fields do not seem to work on the Redemption.SafeItem equivalent.
                    if (parent.InternalItem.InternalItem is MSOutlook.StorageItem)
                    {
                        currentValue = OfficeObject.GetPropertyEx<object>(parent.InternalItem.RDOItem, "Fields", pid);
                    }
                    else
                    {
                        currentValue = parent.InternalItem.SafeItem.Fields[pid];
                    }
                }
                return currentValue;
            }
            set
            {
                if (object.Equals(Value, value))
                    return;

                var pid = Helpers.ToPropId(propid, Type.ToPT());
                if (parent.InternalItem.InternalItem is MSOutlook.StorageItem)
                {
                    OfficeObject.SetPropertyEx(parent.InternalItem.RDOItem, "Fields", pid, value);
                }
                else
                {
                    if (string.Empty.Equals(value))
                        value = null;

                    parent.InternalItem.SafeItem.Fields[pid] = value;
                }
                currentValue = value;
            }
        }

        public void Printable(bool value)
        {
            var properties = parent.InternalItem.RealUserProperties;

            var property = properties[Name];

            if (property == null && value)
            {
                //Only add the proeprty to outlook user proeprty collection if it has a valid value. This is so that DOCID cannot accidentally be added
                //as a string.
                if (this.Value != null)
                {
                    property = properties.Add(Name, this.Type, false, System.Type.Missing);
                    property.Value = this.Value;
                }
            }

            if (property != null)
                property.Printable(value);

        }

        #endregion
    }
}
