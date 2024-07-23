using System;

namespace FWBS.OMS.Design
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DataListAttribute : Attribute
    {
        private string _code = String.Empty;
        private string _display = String.Empty;
        private string _value = string.Empty;
        private bool _parse = false;
        private object _notset = null;
        private bool _useNull = false;
        private string _orderby = String.Empty;

        public DataListAttribute(string code)
        {
            _code = code;
        }

        public string Code
        {
            get
            {
                return _code;
            }
        }

        public string DisplayMember
        {
            get
            {
                return _display;
            }
            set
            {
                if (value == null) value = String.Empty;
                _display = value;
            }
        }

        public string ValueMember
        {
            get
            {
                return _value;
            }
            set
            {
                if (value == null) value = String.Empty;
                _value = value;
            }
        }

        public bool Parse
        {
            get
            {
                return _parse;
            }
            set
            {
                _parse = value;
            }
        }

        public bool UseNull
        {
            get
            {
                return _useNull;
            }
            set
            {
                _useNull = value;
            }
        }

        public object NullValue
        {
            get
            {
                return _notset;
            }
            set
            {
                _notset = value;
            }
        }

        public string OrderBy
        {
            get
            {
                return _orderby;
            }
            set
            {
                if (value == null) value = String.Empty;
                _orderby = value;
            }
        }
    }
}
