using System;
using FWBS.OMS.Dashboard;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    internal class OmsObjectDashboardCell : DashboardCell
    {
        public OmsObjectDashboardCell(Guid id, DashboardConfigProvider.DashboardItemInfo itemInfo) : base(id, itemInfo)
        {
            SourceCode = itemInfo.SourceCode;
        }

        public string SourceCode { get; protected set; }

        public override string Description
        {
            get
            {
                return Session.CurrentSession.Terminology.Parse(CodeLookup.GetLookup("OMSOBJECT", Code, Code), true);
            }
        }

        public virtual OMSObjectTypes OmsObjectType { get; }

        public override DashboardCellEnum GetCellType()
        {
            return DashboardCellEnum.OMSObject;
        }

    }
}
