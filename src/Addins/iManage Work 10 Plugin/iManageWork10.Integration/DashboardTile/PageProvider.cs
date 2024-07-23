using System;
using System.Windows.Forms;
using FWBS.OMS;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace iManageWork10.Integration.DashboardTile
{
    class PageProvider : IPageProvider
    {
        private const string iMANAGE_CODE = "iMANAGEHEADER";

        public PageProvider()
        {
            Headers = new[]
            {
                iMANAGE_CODE
            };
        }

        public string[] Headers { get; }

        public BaseContainerPage GetPage(string header)
        {
            return new uciManageDashboardItem()
            {
                Code = iMANAGE_CODE,
                Title = CodeLookup.GetLookup("DASHBOARD", iMANAGE_CODE, "iManage"),
                Dock = DockStyle.Fill,
                HideBottomPanel = true,
                HideSearchButton = true
            };
        }

        public PageDetails GetDetails(string header)
        {
            switch (header)
            {
                case iMANAGE_CODE:
                    return new PageDetails(iMANAGE_CODE, CodeLookup.GetLookup("DASHBOARD", iMANAGE_CODE, "iManage"));
            }

            throw new ArgumentOutOfRangeException();
        }

    }
}
