using System;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    internal class SearchListDashboardCell : OmsObjectDashboardCell
    {
        public SearchListDashboardCell(Guid id, DashboardConfigProvider.DashboardItemInfo itemInfo) : base(id, itemInfo)
        {
            _builder = new SearchListBuilder();
        }

        public override OMSObjectTypes OmsObjectType
        {
            get { return OMSObjectTypes.List; }
        }
    }
}
