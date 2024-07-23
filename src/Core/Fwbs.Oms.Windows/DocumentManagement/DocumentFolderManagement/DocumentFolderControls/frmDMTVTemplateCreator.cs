using System.Windows.Forms;

namespace FWBS.OMS.UI.DocumentManagement.Addins
{
    public partial class frmDMTVTemplateCreator : FWBS.OMS.UI.Windows.BaseForm
    {

        public frmDMTVTemplateCreator()
        {
            InitializeComponent();
        }

        public bool MigrateWalletsToFoldersOnSave
        {
            get
            {
                return ucTreeViewTemplateCreator.MigrateWalletsToFoldersOnSave;
            }
        }

        public object TemplateID
        {
            get { return this.ucTreeViewTemplateCreator.TemplateID; }
            set { this.ucTreeViewTemplateCreator.TemplateID = value; }
        }

        private void frmDMTVTemplateCreator_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Note: Save selected value on the form closing for now
            this.DialogResult = DialogResult.OK;
        }
    }
}
