using System;
using System.Windows.Forms;

namespace DocumentArchivingForm
{
    public partial class DocumentArchivingForm : Form
    {
        public DocumentArchivingForm()
        {
            InitializeComponent();
        }

        private void btnRunProcess_Click(object sender, EventArgs e)
        {
            DocumentArchivingClass.DocumentArchiving docArchive = new DocumentArchivingClass.DocumentArchiving();
            docArchive.RunProcess();
            docArchive = null;
            MessageBox.Show(this, "Process Complete");
        }
    }
}
