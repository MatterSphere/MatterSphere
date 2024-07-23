namespace Fwbs.Office.Outlook
{
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public partial class OutlookReport :
        OutlookItem
        , MSOutlook.ReportItem
    {

        #region Fields

        private MSOutlook._ReportItem  report;

        #endregion

        #region Constructors

        public OutlookReport(MSOutlook.ReportItem report)
            : base(report)
        {
            this.report = report;
        }

        #endregion

        new private MSOutlook._ReportItem InternalItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();
                return report;
            }
        }

        #region Redemption

        new protected internal Redemption.SafeReportItem SafeItem
        {
            get
            {
                return (Redemption.SafeReportItem)base.SafeItem;
            }
        }

        new protected internal Redemption.RDOReportItem RDOItem
        {
            get
            {
                return (Redemption.RDOReportItem)base.RDOItem;
            }
        }

        #endregion


        #region _ReportItem Members


        MSOutlook.Application MSOutlook._ReportItem.Application
        {
            get { return Application; }
        }

        #endregion
    }
}
