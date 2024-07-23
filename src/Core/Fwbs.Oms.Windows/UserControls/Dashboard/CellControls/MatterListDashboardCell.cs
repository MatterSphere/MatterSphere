using System;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    internal class MatterListDashboardCell : DashboardCell
    { 
        public MatterListDashboardCell(Guid id, DashboardConfigProvider.DashboardItemInfo itemInfo) : base(id, itemInfo)
        {
            _defaultDescription = "Matter List / Matters for Review";
            Code = DashboardCellConverter.GetDashboardCellCode(DashboardCellEnum.Matters);

            _builder = new MatterListBuilder();
        }

        public override DashboardCellEnum GetCellType()
        {
            return DashboardCellEnum.Matters;
        }
    }
}
