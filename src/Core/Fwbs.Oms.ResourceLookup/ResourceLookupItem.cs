using System;
using System.ComponentModel;


namespace FWBS.OMS.UI.Windows
{

    [TypeConverter(typeof(ResourceLookupItemConverter))]
    [Serializable()]
    public class ResourceLookupItem
    {
        private string _code = "";
        private string _description = "";
        private string _help = "";

        public ResourceLookupItem(string Code, string Description, string Help)
        {
            _code = Code;
            _description = Description;
            _help = Help;
        }

        public string Code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string Help
        {
            get
            {
                return _help;
            }
            set
            {
                _help = value;
            }
        }

        public override string ToString()
        {
            return _description;
        }

    }

}
