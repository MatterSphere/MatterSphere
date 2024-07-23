using System.Windows.Forms;

namespace FWBS.OMS.UI.Elasticsearch
{
    public class DataGridViewHighlightsColumn : DataGridViewTextBoxColumn
    {
        public DataGridViewHighlightsColumn()
        {
            CellTemplate = new DataGridViewHighlightsCell();
        }
    }
}
