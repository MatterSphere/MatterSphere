using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.DocumentManagement.Addins
{
    public partial class frmFolderWalletMappingEditor : FWBS.OMS.UI.Windows.BaseForm
    {
        public frmFolderWalletMappingEditor()
        {
            InitializeComponent();
        }


        private void frmFolderWalletMappingEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Note: Save selected value on the form closing for now
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void frmFolderWalletMappingEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
