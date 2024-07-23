using System;
using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Reports
{
    public static class ExternalServices
    {
        public static Form OpenReport(ReportStartupParams reportsparam)
        {
            var parent = FWBS.OMS.UI.Factory.OMSObjectFactory.Get(reportsparam.Parent);
            return OpenReport(reportsparam.ReportCode, parent, reportsparam.Param, reportsparam.RunNow, reportsparam.PrintNow, Size.Empty, reportsparam, false);
        }

        private static Form OpenReport(string ReportCode, object parent, FWBS.Common.KeyValueCollection param, bool RunNow, bool PrintNow, Size ReportSize, IWin32Window owner, bool modal)
        {
            try
            {
                frmOpenReports fop = new frmOpenReports();
                if (ReportSize != Size.Empty) fop.Size = ReportSize;

                FWBS.OMS.Report _report = FWBS.OMS.Report.GetReport(ReportCode, parent, param);

                if (parent == null)
                {
                    if (_report.SearchList.ParentTypeRequired.ToUpper() == "FWBS.OMS.CLIENT")
                    {
                        FWBS.OMS.UI.Windows.Services.SelectClient();
                        _report.SearchList.ChangeParent(Session.CurrentSession.CurrentClient);
                    }
                    else if (_report.SearchList.ParentTypeRequired.ToUpper() == "FWBS.OMS.OMSFILE")
                    {
                        FWBS.OMS.UI.Windows.Services.SelectFile();
                        _report.SearchList.ChangeParent(Session.CurrentSession.CurrentFile);
                    }
                }

                if (modal)
                {
                    fop.ReportsView.Report = _report;
                    if (RunNow) fop.ReportsView.Run();
                    fop.ShowDialog(owner);
                    return null;
                }
                else
                {
                    fop.Show(owner);
                    Application.DoEvents();
                    fop.ReportsView.Report = _report;
                    if (RunNow) fop.ReportsView.Run();
                    return fop;
                }
            }
            catch (Exception ex)
            {
                throw new OMSException2("ERRRPTNOTFND", "Report '%1%' cannot be found ...", ex, true, ReportCode);
            }
        }
    }
}
