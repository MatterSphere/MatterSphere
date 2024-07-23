using System;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    internal class EnquiryFormDashboardCell : OmsObjectDashboardCell
    {
        public EnquiryFormDashboardCell(Guid id, DashboardConfigProvider.DashboardItemInfo itemInfo) : base(id, itemInfo)
        {
            _builder = new EnquiryFormBuilder();
        }
    }
}
