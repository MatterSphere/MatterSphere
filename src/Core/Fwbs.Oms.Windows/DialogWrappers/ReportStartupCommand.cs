using System;
using FWBS.OMS.UI.Factory;

namespace FWBS.OMS.UI.Windows
{
    public class ReportingStartupCommand : MarshalByRefObject
    {
        public void Execute(OMSObjectFactoryItem parent, string reportcode, FWBS.Common.KeyValueCollection param, bool runnow, bool printnow, IntPtr handle)
        {
            ReportStartupParams.Reports.Enqueue(new ReportStartupParams() { Parent = parent, ReportCode = reportcode, PrintNow = printnow, RunNow = runnow, Param = param, Handle = handle });
        }
    }
}
