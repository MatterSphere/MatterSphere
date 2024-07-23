using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.OMS.UI.Factory;

namespace FWBS.OMS.UI.Windows
{
    public class ReportStartupParams : IWin32Window
    {
        public static Queue<ReportStartupParams> Reports = new Queue<ReportStartupParams>();

        public OMSObjectFactoryItem Parent
        {
            get; set;
        }

        public string ReportCode
        {
            get; set;
        }

        public FWBS.Common.KeyValueCollection Param
        {
            get; set; 
        }
            
        public bool RunNow
        {
            get; set;
        }
            
        public bool PrintNow
        {
            get; set;
        }

        public IntPtr Handle
        {
            get;
            set;
        }
    }
}
