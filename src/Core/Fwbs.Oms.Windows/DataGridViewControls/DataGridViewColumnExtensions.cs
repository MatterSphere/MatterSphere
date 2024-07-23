using System.Drawing;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.DataGridViewControls
{
    internal static class DataGridViewColumnExtensions
    {
        internal static void ProcessBeforeCellDisplayEvent(this DataGridViewColumn column, DataGridViewCellStyle cellStyle, int rowIndex, ref object formattedValue)
        {
            var dataGridView = column?.DataGridView;
            var source = dataGridView?.DataSource != null
                    ? dataGridView.BindingContext[dataGridView.DataSource, dataGridView.DataMember] as CurrencyManager
                    : null;

            var owningColumn = column as IBeforeCellDisplayable;
            if (source != null && owningColumn != null && dataGridView != null)
            {
                Common.UI.Windows.CellDisplayEventArgs ee =
                    owningColumn.OnBeforeCellDisplayEvent(rowIndex, source, (string)formattedValue, column.DataPropertyName);

                if (ee != null)
                {
                    if (source.Position != rowIndex && (dataGridView.Rows[rowIndex].Selected == false || dataGridView.MultiSelect == false))
                    {
                        if (ee.BackColor != Color.Empty)
                            cellStyle.BackColor = ee.BackColor;
                        if (ee.ForeColor != Color.Empty)
                            cellStyle.ForeColor = ee.ForeColor;
                    }
                    formattedValue = ee.Text;
                }
            }
        }
    }
}
