namespace FWBS.OMS.Utils.Commands
{
    public class ViewReportCommand : RunCommand
    {
        public override string Name
        {
            get { return "VIEWREPORT"; }
        }

        public override void Execute(MainWindow main)
        {
            FWBS.OMS.UI.Windows.Services.Reports.OpenReport(Param);
        }
    }
}
