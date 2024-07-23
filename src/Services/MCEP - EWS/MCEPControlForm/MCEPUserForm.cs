using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace MCEPControlForm
{
    public partial class MCEPUserForm : Form
    {
        public MCEPUserForm()
        {
            InitializeComponent();
        }
        private MCEPGlobalClasses.MCEPAdminRoutines admin;
        private void MCEPUserForm_Load(object sender, EventArgs e)
        {
            try
            {
                SetDataGridProperties();
                btnRefreshList.Enabled = true;
                btnSave.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetDataGridProperties()
        {
            if (admin == null)
            {
                admin = new MCEPGlobalClasses.MCEPAdminRoutines();
            }
            
            List<MCEPGlobalClasses.MCEPUser> users = admin.GetUsers();
            BindingList<MCEPGlobalClasses.MCEPUser> usersB = new BindingList<MCEPGlobalClasses.MCEPUser>(users);
            dgUsers.DataSource = usersB;

            dgUsers.Columns["UserRowUpdated"].Visible = false;
            dgUsers.Columns["UserID"].ReadOnly = true;
            dgUsers.Columns["UserLastRan"].ReadOnly = true;
            dgUsers.Columns["UserCreated"].ReadOnly = true;
            dgUsers.Columns["UserUpdated"].ReadOnly = true;
            foreach (DataGridViewColumn column in dgUsers.Columns)
            {
                dgUsers.Columns[column.Name].SortMode = DataGridViewColumnSortMode.Automatic;
            }
        }

        private void btnRefreshList_Click(object sender, EventArgs e)
        {
            dgUsers.DataSource = null;
            btnRefreshList.Enabled = false;
            btnSave.Enabled = false;
            MCEPUserForm_Load(this, e);
        }

        private void dgUsers_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgUsers_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgUsers.Rows)
            {
                MCEPGlobalClasses.MCEPUser user = (MCEPGlobalClasses.MCEPUser)row.DataBoundItem;
                if (user.UserRowUpdated)
                {
                    if (admin == null) admin = new MCEPGlobalClasses.MCEPAdminRoutines();
                    admin.UpdateUserRecord(user);
                }
            }
        }
    }
}
