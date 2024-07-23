using System;
using System.Collections;
using System.Windows.Forms;


namespace FWBS.OMS.UI.Windows.Reports
{
    class ReportsTreeNavigationActions : TreeNavigationActions
    {
        private SortedList Windows = new SortedList();

        public ReportsTreeNavigationActions()
        {
            this.ParentKey = "REPORTS";
            this.Title = FWBS.OMS.Branding.APPLICATION_NAME + " Reports";
        }

        public override object Action(string ActionCmd, string ActionLabel)
        {
            var _power = Session.CurrentSession.CurrentPowerUserSettings;
            string filter = "";
            string ecmd = ActionCmd;
            object returnval = null;
            if (ecmd.IndexOf("|") > -1)
            {
                filter = ecmd.Substring(ecmd.IndexOf("|") + 1);
                ecmd = ecmd.Substring(0, ecmd.IndexOf("|"));
            }

            if (_power != null)
            {
                if (_power.CanRunAction(ActionCmd) == false)
                {
                    throw new FWBS.OMS.Security.PermissionsException("EXPERMDENIED2", "Permission Denied", true);
                }
            }

            try
            {
                var result = false;
                returnval = MacroCommands(ecmd, filter, out result);
                if (result)
                {
                    return returnval;
                }
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
            finally
            {
            }
            return returnval;
        }


        public override Control MacroCommands(string ecmd, string filter, out bool result)
        {
            result = false;
            if (ecmd == "MENUSCRIPT")
            {
                using (FWBS.OMS.Script.MenuScriptAggregator agg = new FWBS.OMS.Script.MenuScriptAggregator())
                {
                    agg.Invoke(filter);
                    result = true;
                }
            }
            return null;
        }


        public override Control ConstructAdminElement(string filter, Control frmparent, string ecmd)
        {
            Control returnval = null;
            try
            {
                if (ecmd == "RS")
                {
                    frmIntReportRS frmReport1 = new frmIntReportRS(this);
                    frmReport1.Show();
                    frmReport1.ucReportsView1.OpenReport(FWBS.OMS.ReportingServices.SSRSConnect.ReportServerIP, filter, null, null);
                    frmReport1.BringToFront();
                }
                else
                {
                    if (ecmd != "AKC")
                    {
                        object parent = null;
                        ucReportsView reportsView = new ucReportsView() { Dock = DockStyle.Fill };
                        reportsView.Padding = new Padding(4);

                        FWBS.OMS.Report report = FWBS.OMS.Report.GetReport(ecmd, null, null);
                        if (report.SearchList.ParentTypeRequired.ToUpper() == "FWBS.OMS.CLIENT")
                        {
                            FWBS.OMS.UI.Windows.Services.SelectClient();
                            parent = Session.CurrentSession.CurrentClient;
                        }
                        else if (report.SearchList.ParentTypeRequired.ToUpper() == "FWBS.OMS.OMSFILE")
                        {
                            FWBS.OMS.UI.Windows.Services.SelectFile();
                            parent = Session.CurrentSession.CurrentFile;
                        }

                        report = FWBS.OMS.Report.GetReport(ecmd, parent, null);
                        reportsView.Report = report;
                        returnval = reportsView;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }

            return returnval;
        }

        public override bool IsApplicationsPanelVisible
        {
            get
            {
                return false;
            }
        }

        public override bool IsSystemUpdatePanelVisible
        {
            get
            {
                return false;
            }
        }
    }
}
