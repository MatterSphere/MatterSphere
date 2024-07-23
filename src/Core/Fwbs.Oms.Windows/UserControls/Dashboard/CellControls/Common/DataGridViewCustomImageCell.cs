using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    internal class DataGridViewCustomImageCell : DataGridViewImageCell
    {
        protected override void OnMouseEnter(int rowIndex)
        {
            base.OnMouseEnter(rowIndex);
            this.DataGridView.Cursor = Cursors.Hand;
        }

        protected override void OnMouseLeave(int rowIndex)
        {
            base.OnMouseLeave(rowIndex);
            this.DataGridView.Cursor = Cursors.Default;
        }
    }
}
