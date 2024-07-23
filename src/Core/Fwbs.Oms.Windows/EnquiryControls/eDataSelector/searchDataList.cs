using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows
{
    public partial class searchDataList : BaseForm
    {
        public object Value { get; set; }
        public string Description { get; set; }
        public DataTable DataSource { get; set; }

        private string _displayColumn = string.Empty;
        private string _filterString = string.Empty;

        public searchDataList(DataTable dataSource, string filterString)
        {
            InitializeComponent();
            this.dgvDataList.RowTemplate.Height = LogicalToDeviceUnits(22);
            this.SetDesktopLocation(Cursor.Position.X, Cursor.Position.Y);
            if (dataSource != null)
            {
                this.dgvDataList.DataSource = dataSource;
                this.dgvDataList.Columns[0].Visible = false;

                for (int i = 2; i < this.dgvDataList.ColumnCount; i++)
                {
                    this.dgvDataList.Columns[i].Visible = false;
                }

                this.dgvDataList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                _displayColumn = dataSource.Columns[1].ColumnName;
                _filterString = filterString;
                this.txtSearch.Select();
            }
        }

        public searchDataList()
        {
            InitializeComponent();
        }

        private void selectCurrentValue(string currValue)
        {
            DataGridViewRow row = this.dgvDataList.Rows.Cast<DataGridViewRow>()
                .Where(r => Convert.ToString(r.Cells[0].Value).Trim().Equals(currValue, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            if (row != null)
            {
                this.dgvDataList.Rows[row.Index].Selected = true;
                this.dgvDataList.FirstDisplayedScrollingRowIndex = row.Index;
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            closeForm(this.dgvDataList.SelectedRows[0].Index);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            (this.dgvDataList.DataSource as DataTable).DefaultView.RowFilter = string.Format("{0}{1} LIKE '%{2}%'", (string.IsNullOrEmpty(_filterString) ? "" : _filterString + " AND "), _displayColumn, txtSearch.Text);
            this.btnSelect.Enabled = (this.dgvDataList.RowCount > 0);
        }

        private void dgvDataList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            closeForm(e.RowIndex);
        }

        private void closeForm(int row)
        {
            this.Value = this.dgvDataList.Rows[row].Cells[0].Value;
            this.Description = Convert.ToString(this.dgvDataList.Rows[row].Cells[1].Value);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.dgvDataList.RowCount > 0)
            {
                selectCurrentValue(Convert.ToString(Value).Trim());
            }
            else
            {
                this.btnSelect.Enabled = false;
                this.txtSearch.Enabled = false;
            }
        }
    }
}
