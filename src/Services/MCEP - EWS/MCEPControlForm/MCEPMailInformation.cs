using System;
using System.Windows.Forms;

namespace MCEPControlForm
{
    public partial class MCEPMailInformation : Form
    {
        public MCEPMailInformation()
        {
            InitializeComponent();
        }
        private MCEPGlobalClasses.MCEPStorerClass mcepStorer;

        private void GenerateClassReferences()
        {
            if (mcepStorer == null) mcepStorer = new MCEPGlobalClasses.MCEPStorerClass();
        }
        private void DisposeOfClassReferences()
        {
            if (mcepStorer != null) mcepStorer = null;
        }

        private void btnGetInformation_Click(object sender, EventArgs e)
        {
            try
            {
            string MailItemID = txtStoreID.Text;
            GenerateClassReferences();
            txtMailInformation.Text = mcepStorer.GetMailInformation(MailItemID);
            }
            catch (Exception ex)
            {
                txtMailInformation.Text = ex.Message.ToString();
            }
            finally
            {
                DisposeOfClassReferences();
            }

        }



    }
}
