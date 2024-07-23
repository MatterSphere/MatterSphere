using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.CellBuilders
{
    internal class EnquiryFormBuilder : ICellBuilder
    {
        public void Build(ucCellContainer container, DashboardCell cell)
        {
            var enquiry = cell as EnquiryFormDashboardCell;
            var form = new EnquiryForm();
            container.InsertEnquiryForm(form, enquiry.SourceCode);
            container.InsertTitle(cell.Description);
            container.HideFilterButton();
        }

        public ContentContainer CreateWindowContent(DashboardCell cell, string omsObjectCode, string title)
        {
            var enquiryFormCell = cell as EnquiryFormDashboardCell;
            var form = new EnquiryForm();
            return new ContentContainer(enquiryFormCell.SourceCode, form)
            {
                Title = cell.Description
            };
        }
    }
}
