using System;
using System.Drawing;
using FWBS.OMS.Dashboard;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders;
using Infragistics.Win.Misc;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls
{
    public abstract class DashboardCell
    {
        protected string _defaultDescription;
        protected ICellBuilder _builder;

        protected DashboardCell(Guid id, DashboardConfigProvider.DashboardItemInfo itemInfo)
        {
            Id = id;
            Code = itemInfo.Code;
            Priority = itemInfo.Priority;
            MinimalSize = itemInfo.MinimalSize;
            Size = itemInfo.MinimalSize;
        }

        public Guid Id { get; set; }
        public string DashboardCode { get; set; }
        public string Code { get; protected set; }
        public int Priority { get; private set; }
        public Size MinimalSize { get; private set; }
        public Size Size { get; set; }

        public void Build(ucCellContainer container, DashboardCell cell)
        {
            _builder.Build(container, cell);
        }

        public ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title)
        {
            return _builder.CreateWindowContent(cell, omsObjectCode, title);
        }

        public virtual ActionMenuBuilder CreateActionMenuBuilder(UltraPeekPopup popup)
        {
            return new ActionMenuBuilder(popup);
        }

        public virtual string Description
        {
            get
            {
                return CodeLookup.GetLookup("DASHBOARD", Code, _defaultDescription);
            }
        }

        public abstract DashboardCellEnum GetCellType();
    }
}
