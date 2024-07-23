using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    internal class DataGridViewTaskStatusColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewTaskStatusColumn()
        {
            this.CellTemplate = new DataGridViewTaskStatusCell();
        }
    }
}
