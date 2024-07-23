using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    internal class DataGridViewFlagColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewFlagColumn()
        {
            MinimumWidth = 20;
            HeaderText = "⚑";
            DefaultCellStyle.Font = new Font("Segoe UI Symbol", 14F);
            DefaultCellStyle.Padding = new Padding(2, 0, 0, 6);
            HeaderCell.Style.Font = new Font("Segoe UI Symbol", 14F);
            HeaderCell.Style.ForeColor = Color.FromArgb(247, 143, 143);
            this.CellTemplate = new DataGridViewFlagCell();
        }
    }
}
