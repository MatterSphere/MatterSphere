using System;
using System.Windows.Forms;

namespace MCEPControlForm
{
    public partial class MCEPControl : Form
    {
        public MCEPControl()
        {
            InitializeComponent();
        }

        private void btnRunProfiler_Click(object sender, EventArgs e)
        {
            MCEPGlobalClasses.MCEPProfilerClass profiler = new MCEPGlobalClasses.MCEPProfilerClass();
            profiler.RunProcess();
            profiler = null;
        }

        private void btnRunStorer_Click(object sender, EventArgs e)
        {
            MCEPGlobalClasses.MCEPStorerClass storer = new MCEPGlobalClasses.MCEPStorerClass();
            storer.RunProcess();
            storer = null;
        }

        private void btnEnableManualRun_Click(object sender, EventArgs e)
        {
            btnRunProfiler.Visible = true;
            btnRunStorer.Visible = true;
        }

        private void btnImportMissingUsers_Click(object sender, EventArgs e)
        {
            try
            {
                MCEPGlobalClasses.MCEPAdminRoutines admin = new MCEPGlobalClasses.MCEPAdminRoutines();
                admin.ImportMissingUsers(txtDefaultFolderName.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, btnImportMissingUsers.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditUsers_Click(object sender, EventArgs e)
        {
            MCEPUserForm userForm = new MCEPUserForm();
            userForm.Show();
        }


    }
}
