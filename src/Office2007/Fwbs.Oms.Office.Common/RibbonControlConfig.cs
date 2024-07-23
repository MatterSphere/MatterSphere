using System;

namespace Fwbs.Oms.Office.Common
{

    internal class RibbonControlConfig : FWBS.OMS.Script.MenuItem
    {
        internal RibbonControlConfig() { }

        private string size = String.Empty;
        public string Size
        {
            get
            {
                return size;
            }
            internal set
            {
                size = value;
            }
        }

        private string res = String.Empty;
        public string ResourceId
        {
            get
            {
                return res;
            }
            internal set
            {
                res = value;
            }
        }
    }
}
