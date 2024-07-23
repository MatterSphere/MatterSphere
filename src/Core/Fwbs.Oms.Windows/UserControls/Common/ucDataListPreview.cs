using System.Windows.Forms;

namespace FWBS.OMS.UI.UserControls.Common
{
    public partial class ucDataListPreview : UserControl
    {
        public ucDataListPreview()
        {
            InitializeComponent();
        }

        public object DataSource
        {
            get { return GridView.DataSource; }

            set
            {
                GridView.DataSource = value;

                foreach (DataGridViewColumn column in GridView.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

                if (this.Width > TotalWidth)
                {
                    GridView.Columns[GridView.Columns.Count - 1].Width += Width - TotalWidth;
                }
            }
        }

        public int TotalWidth
        {
            get
            {
                var width = 0;
                foreach (DataGridViewColumn column in GridView.Columns)
                {
                    width += column.Width;
                }

                return width;
            }
        }
    }
}
