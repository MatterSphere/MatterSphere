using System;
using System.Data;

namespace FWBS.OMS.UI.Windows
{
    public class BeforeSelectionChangeEventArgs : EventArgs
    {
        public bool Cancel { get; set; }
        public DataRow[] DataRows { get; set; }
        public Direction Direction { get; set; }
    }   
}
