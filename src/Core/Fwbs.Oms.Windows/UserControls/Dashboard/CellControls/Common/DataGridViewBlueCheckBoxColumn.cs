using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    internal class DataGridViewBlueCheckBoxColumn : DataGridViewCheckBoxColumn
    {
        public DataGridViewBlueCheckBoxColumn()
        {
            this.CellTemplate = new DataGridViewBlueCheckBoxCell();
            HeaderText = "☐";
            HeaderCell.Style.Padding = new Padding(2,0,0,0);
            HeaderCell.Style.Font = new Font("Segoe UI Symbol", 14F);
            HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
    }
}
