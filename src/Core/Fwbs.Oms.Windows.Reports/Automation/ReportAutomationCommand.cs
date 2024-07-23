using System;
using System.Linq;
using FWBS.OMS.UI.Factory;
using FWBS.OMS.UI.Windows.Reports;

namespace FWBS.OMS.UI.Windows
{
    public class ReportAutomationCommand : MarshalByRefObject
    {
        public Guid Print(string reportName, FWBS.Common.KeyValueCollection param, OMSObjectFactoryItem parent, int copies, aPageMargins? margins, string printerName, IntPtr owner)
        {
            Guid id = Guid.NewGuid();
            ReportAutomationParams.Reports.Enqueue(new ReportAutomationParams() { ReportCode = reportName, Param = param, Parent = parent, Copies = copies, Margins = margins, PrinterName = printerName, Handle = owner, ID = id });
            return id;
        }

        public Guid Export(string reportName, FWBS.Common.KeyValueCollection param, OMSObjectFactoryItem parent, aExportFormatType exportFormatType, string destination, IntPtr owner)
        {
            Guid id = Guid.NewGuid();
            ReportAutomationParams.Reports.Enqueue(new ReportAutomationParams() { ReportCode = reportName, Param = param, Parent = parent, ExportFormatType = exportFormatType, Destination = destination, Handle = owner, ID = id });
            return id;
        }

        public bool IsAutomationActive(Guid id)
        {
            return (ReportAutomationParams.Reports.FirstOrDefault(n => n.ID == id) != null);
        }
    }
}
