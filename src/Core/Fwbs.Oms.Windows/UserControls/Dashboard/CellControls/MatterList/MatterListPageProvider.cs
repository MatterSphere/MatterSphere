using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.MatterList
{
    internal class MatterListPageProvider : IPageProvider
    {
        private const string MATTER_LIST_OMS_CODE = "SCHFEEFILELIST";
        private const string MATTER_LIST_TITLE_CODE = "FLLIST";
        private const string MATTERS_FOR_REVIEW_OMS_CODE = "SCHFEEREVMGR";
        private const string MATTERS_FOR_REVIEW_TITLE_CODE = "FLFORRVW";

        public MatterListPageProvider()
        {
            Headers = new[]
            {
                MatterListDashboardItem.MatterListPageEnum.MattersForReview.ToString(),
                MatterListDashboardItem.MatterListPageEnum.MatterList.ToString()
            };
        }

        public string[] Headers { get; }

        public BaseContainerPage GetPage(string header)
        {
            if (header == MatterListDashboardItem.MatterListPageEnum.MatterList.ToString())
            {
                return CreateMatterListPage();
            }

            if (header == MatterListDashboardItem.MatterListPageEnum.MattersForReview.ToString())
            {
                return CreateMattersForReviewPage();
            }

            throw new ArgumentOutOfRangeException();
        }

        public PageDetails GetDetails(string header)
        {
            if (header == MatterListDashboardItem.MatterListPageEnum.MatterList.ToString())
            {
                return new PageDetails(MATTER_LIST_OMS_CODE, CodeLookup.GetLookup("DASHBOARD", MATTER_LIST_TITLE_CODE, "Matter List"));
            }

            if (header == MatterListDashboardItem.MatterListPageEnum.MattersForReview.ToString())
            {
                return new PageDetails(MATTERS_FOR_REVIEW_OMS_CODE, CodeLookup.GetLookup("DASHBOARD", MATTERS_FOR_REVIEW_TITLE_CODE, "Matters for Review"));
            }

            throw new ArgumentOutOfRangeException();
        }

        private BaseContainerPage CreateMatterListPage()
        {
            return new MatterListDashboardItem(MatterListDashboardItem.MatterListPageEnum.MatterList)
            {
                Code = MatterListDashboardItem.MatterListPageEnum.MatterList.ToString(),
                Title = CodeLookup.GetLookup("DASHBOARD", MATTER_LIST_TITLE_CODE, "Matter List"),
                Dock = DockStyle.Fill
            };
        }

        private BaseContainerPage CreateMattersForReviewPage()
        {
            return new MatterListDashboardItem(MatterListDashboardItem.MatterListPageEnum.MattersForReview)
            {
                Code = MatterListDashboardItem.MatterListPageEnum.MattersForReview.ToString(),
                Title = CodeLookup.GetLookup("DASHBOARD", MATTERS_FOR_REVIEW_TITLE_CODE, "Matters for Review"),
                Dock = DockStyle.Fill,
                FilterOptions = new List<ActionItem>
                {
                    new ActionItem(CodeLookup.GetLookup("CCSTATUS", "WITHIN7", "Within 7 Days"), "WITHIN7"),
                    new ActionItem(CodeLookup.GetLookup("CCSTATUS", "OD", "Overdue"), "OD"),
                    new ActionItem(CodeLookup.GetLookup("CCSTATUS", "OVER7", "Over 7 Days"), "OVER7"),
                    new ActionItem(CodeLookup.GetLookup("CCSTATUS", "ALL", "All"), "ALL")
                },

                DefaultFilter = CodeLookup.GetLookup("CCSTATUS", "OD", "Overdue")
            };
        }
    }
}
