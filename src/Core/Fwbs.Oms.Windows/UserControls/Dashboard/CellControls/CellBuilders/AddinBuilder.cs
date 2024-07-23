using System;
using FWBS.OMS.UI.Windows.Interfaces;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders
{
    internal class AddinBuilder : ICellBuilder
    {
        public void Build(ucCellContainer container, DashboardCell cell)
        {
            OmsObject omsObject = new OmsObject(cell.Code);
            Type type = Windows.Global.CreateType(omsObject.Windows, omsObject.Assembly);
            ICellBuilder builder = (ICellBuilder)type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
            builder.Build(container, cell);
        }

        public ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title)
        {
            OmsObject omsObject = new OmsObject(omsObjectCode);
            Type type = Windows.Global.CreateType(omsObject.Windows, omsObject.Assembly);
            IOMSTypeAddin addin = (IOMSTypeAddin)type.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, null, null);
            addin.Connect(Session.CurrentSession.CurrentUser);
            if (addin.ToBeRefreshed)
            {
                addin.RefreshItem();
                addin.ToBeRefreshed = false;
            }

            return new ContentContainer(omsObjectCode, addin)
            {
                Title = title
            };
        }
    }
}
