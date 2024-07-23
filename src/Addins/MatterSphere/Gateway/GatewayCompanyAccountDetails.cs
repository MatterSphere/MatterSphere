using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using FWBS.MatterSphereIntegration.Gateway;

namespace FWBS.MatterSphereIntegration
{
    public partial class GatewayCompanyAccountDetails : UserControl
    {
        public GatewayCompanyAccountDetails()
        {
            InitializeComponent();

            this.dataGridView1.SelectionChanged += new EventHandler(dataGridView1_SelectionChanged);
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Name", HeaderText = "Name" });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Type", HeaderText = "Type" });
            this.dataGridView1.Columns.Add(new DataGridViewTextBoxColumn() { DataPropertyName = "Value", HeaderText = "Value", MinimumWidth= 150 });
            this.dataGridView1.Columns.Add(new DataGridViewCheckBoxColumn() { DataPropertyName = "Display", HeaderText = "Display" });
        }

        void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.SelectedRows == null || this.dataGridView1.SelectedRows.Count == 0)
            {
                this.button4.Enabled = false;
                return;
            }
            this.button4.Enabled = true;

            var data = this.dataGridView1.SelectedRows[0].DataBoundItem as EntityDetail;

            this.button2.Enabled = data != Details.FirstOrDefault();
            this.button3.Enabled = data != Details.LastOrDefault();
                

        }

        private void ShowProcessing()
        {
            this.progressBar1.Visible = true;
         
            

        }

        private void HideProcessing()
        {
            this.progressBar1.Visible = false;
        }

        private List<EntityDetail> Details = new List<EntityDetail>(); 
        private string endpoint;
        private string passcode;
        private long companyId;
        private string user;

        Timer t;
        public void SetupData(string endpoint, string passcode, long companyId, string user)
        {
            this.endpoint = endpoint;
            this.passcode = passcode;
            this.companyId = companyId;
            this.user = user;

            t = new Timer();
            t.Tick += new EventHandler(t_Tick);
            t.Interval = 100;
            t.Start();
        }

        public void SaveData()
        {
            var d = new AccountDetails();
            d.Details = this.Details.ToArray();

            GatewayLib.StoreAccountDetails(endpoint, passcode, companyId, user, d);
        }

        void t_Tick(object sender, EventArgs e)
        {
            t.Stop();
            try
            {
                ShowProcessing();

                var result = GatewayLib.GetAccountDetails(endpoint, passcode, companyId, user);

                if (!result.Success)
                {
                    MessageBox.Show(result.Message, "Load Account Details Error");
                    return;
                }

                var details = result.Details;


                

                if (details != null)
                    Details.AddRange(details);


                this.dataGridView1.DataSource = Details;


            }
            catch (Exception)
            {
                MessageBox.Show("An Error Occured While trying to access the service. Please Try later", "Mattersphere Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                HideProcessing();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigureAccountDetail form = new ConfigureAccountDetail();
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.dataGridView1.SuspendLayout();
                this.dataGridView1.DataSource = null;
                this.Details.Add(form.Data);
                this.dataGridView1.DataSource = Details;
                

                this.dataGridView1.ResumeLayout();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.dataGridView1.SuspendLayout();

            var selection = this.dataGridView1.SelectedRows[0].DataBoundItem as EntityDetail;

            var index = this.Details.IndexOf(selection);

            this.Details.Remove(selection);
            this.Details.Insert(--index, selection);

            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = Details;

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                row.Selected = false;
            }

            this.dataGridView1.Rows[index].Selected = true;

            this.dataGridView1.ResumeLayout();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.dataGridView1.SuspendLayout();

            var selection = this.dataGridView1.SelectedRows[0].DataBoundItem as EntityDetail;

            var index = this.Details.IndexOf(selection);

            this.Details.Remove(selection);
            this.Details.Insert(++index, selection);

            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = Details;

            foreach (DataGridViewRow row in this.dataGridView1.Rows)
            {
                row.Selected = false;
            }

            this.dataGridView1.Rows[index].Selected = true;

            this.dataGridView1.ResumeLayout();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.dataGridView1.SuspendLayout();

            var selection = this.dataGridView1.SelectedRows[0].DataBoundItem as EntityDetail;
            this.Details.Remove(selection);
         
            this.dataGridView1.DataSource = null;
            this.dataGridView1.DataSource = Details;

            this.dataGridView1.ResumeLayout();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EditSelectedDetail();
        }

        private void EditSelectedDetail()
        {
            if(this.dataGridView1.SelectedRows == null || this.dataGridView1.SelectedRows.Count == 0)
                return;

            ConfigureAccountDetail form = new ConfigureAccountDetail();

            form.Data = (EntityDetail)this.dataGridView1.SelectedRows[0].DataBoundItem;
            if (form.ShowDialog() == DialogResult.OK)
            {
                this.dataGridView1.SuspendLayout();
                this.dataGridView1.DataSource = null;
                this.dataGridView1.DataSource = Details;


                this.dataGridView1.ResumeLayout();
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            this.EditSelectedDetail();
        }

      
    }
}
