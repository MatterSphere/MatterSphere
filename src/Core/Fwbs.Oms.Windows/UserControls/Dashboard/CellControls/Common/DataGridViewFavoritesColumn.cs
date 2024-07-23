using System.Drawing;
using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common
{
    internal class DataGridViewFavoritesColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewFavoritesColumn()
        {
            MinimumWidth = 20;
            HeaderText = "\u2605";
            DefaultCellStyle.Font = new Font("Segoe UI Symbol", 14F);
            DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            HeaderCell.Style.Font = new Font("Segoe UI Symbol", 14F);
            HeaderCell.Style.ForeColor = Color.FromArgb(255, 185, 0);
            HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.CellTemplate = new DataGridViewFavoritesCell();
        }
    }

    internal class DataGridViewRecentsColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewRecentsColumn()
        {
            MinimumWidth = 20;
            HeaderCell.Style.Font = new Font("Segoe UI Symbol", 12F);
        }
    }
}
