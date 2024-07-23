using FWBS.OMS.Security;
using FWBS.OMS.Security.Permissions;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.MatterList;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders
{
    internal class MatterListBuilder : ICellBuilder
    {
        public void Build(ucCellContainer container, DashboardCell cell)
        {
            if (Session.CurrentSession.CurrentBranch.HideAddNewForMatterListTile)
            {
                container.HideAddNewButton();
            }
            else
            {
                try
                {
                    new SystemPermission(StandardPermissionType.CreateFile).Check();
                }
                catch (PermissionsException)
                {
                    container.HideAddNewButton();
                }
            }

            container.HideDeleteButton();
            container.HideFullScreenButton();
            var provider = new MatterListPageProvider();
            container.InsertDefaultItem(provider);
        }

        public ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title)
        {
            var searchControl = new ucSearchControl();
            return new ContentContainer(omsObjectCode, searchControl)
            {
                Title = title
            };
        }
    }
}
