using System.Windows.Forms;

namespace FWBS.OMS.UI.DataGridViewControls
{
    public class DataGridViewExRow : DataGridViewRow
    {
        public DataGridViewExRow()
        {
            HeaderCell = new DataGridViewExRowHeaderCell();
        }

        public override DataGridViewAdvancedBorderStyle AdjustRowHeaderBorderStyle(
            DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStyleInput,
            DataGridViewAdvancedBorderStyle dataGridViewAdvancedBorderStylePlaceHolder,
            bool singleVerticalBorderAdded,
            bool singleHorizontalBorderAdded,
            bool isFirstDisplayedRow,
            bool isLastDisplayedRow)
        {
            dataGridViewAdvancedBorderStylePlaceHolder.Top = dataGridViewAdvancedBorderStyleInput.Top;
            dataGridViewAdvancedBorderStylePlaceHolder.Right = dataGridViewAdvancedBorderStyleInput.Right;
            dataGridViewAdvancedBorderStylePlaceHolder.Left = dataGridViewAdvancedBorderStyleInput.Left;
            dataGridViewAdvancedBorderStylePlaceHolder.Bottom = DataGridViewAdvancedCellBorderStyle.Single;

            return dataGridViewAdvancedBorderStylePlaceHolder;
        }
    }
}
