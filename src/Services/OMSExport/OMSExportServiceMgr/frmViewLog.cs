using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FWBS.OMS.OMSEXPORT
{
    public partial class frmViewLog : Form
    {
        public frmViewLog()
        {
            InitializeComponent();
        }


        private void frmViewLog_Load(object sender, EventArgs e)
        {
            try
            {

                //populates the grid
                PopulateGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Form Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Priovate methods
        
        private void PopulateGrid()
        {

            using (SqlConnection cnn = new SqlConnection(StaticLibrary.OMSConnectionString()))
            {
                string strSQL = "select top 100 datelogged,Entity,ErrorMessage from dbo.fdexportservicelog order by logid desc";
                SqlCommand cmd = new SqlCommand(strSQL, cnn);
                DataTable tbl = new DataTable();
                cnn.Open();
                SqlDataAdapter adptUrl = new SqlDataAdapter(cmd);
                adptUrl.Fill(tbl);
                dgLog.DataSource = tbl;

                dgLog.Columns[0].Width = 100;
                dgLog.Columns[0].HeaderText = "Date";
                dgLog.Columns[1].Width = 60;
                dgLog.Columns[1].HeaderText = "Type";
                dgLog.Columns[2].Width = dgLog.Width - 200;
                dgLog.Columns[2].HeaderText = "Message - Double Click to Read";

            }
            
            
        }

        #endregion


        /// <summary>
        /// Displays the full error message in a message box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgLog_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex >= 0)
            {
                DataGridViewCell cell = ((DataGridView)sender).Rows[e.RowIndex].Cells[e.ColumnIndex];
                MessageBox.Show(Convert.ToString(cell.Value));
            }
        }
    }
}