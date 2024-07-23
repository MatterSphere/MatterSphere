using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public class ReportAutomationParams : IWin32Window
    {
        public static Queue<ReportAutomationParams> Reports = new Queue<ReportAutomationParams>();

        public Factory.OMSObjectFactoryItem Parent { get; set; }

        public string ReportCode { get; set; }

        public int Copies { get; set; }

        public Windows.Reports.aPageMargins? Margins { get; set; }

        public Common.KeyValueCollection Param { get; set; }

        public string PrinterName { get; set; }

        public Windows.Reports.aExportFormatType? ExportFormatType { get; set; }

        public string Destination { get; set; }

        public IntPtr Handle { get; set; }

        public Guid ID { get; set; }
    }
}
