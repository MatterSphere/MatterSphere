using System;
using System.Windows.Forms;

namespace TestApp
{
    public partial class frmTest : Form
    {
        string service = "MatterSphereBundlerService";

        public frmTest()
        {
            InitializeComponent();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            string server = txtServer.Text;
            string port = txtPort.Text;

            string Uri = string.Format("http://{0}:{1}/{2}", server, port, service);

            System.ServiceModel.BasicHttpBinding bhb = new System.ServiceModel.BasicHttpBinding();
            System.ServiceModel.EndpointAddress epa = new System.ServiceModel.EndpointAddress(Uri);

            try
            {
                MatterSphereBundlerService.MatterSphereBundlerServiceClient bsc = new MatterSphereBundlerService.MatterSphereBundlerServiceClient(bhb, epa);

                MessageBox.Show(bsc.TestConnection(), string.Format("Version : {0}", bsc.ServiceVersion()));
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnPickInstructionFile_Click(object sender, EventArgs e)
        {
            string server = txtServer.Text;
            string port = txtPort.Text;
            
            string Uri = string.Format("http://{0}:{1}/{2}", server, port, service);

            System.ServiceModel.BasicHttpBinding bhb = new System.ServiceModel.BasicHttpBinding();
            System.ServiceModel.EndpointAddress epa = new System.ServiceModel.EndpointAddress(Uri);

            try
            {
                System.Windows.Forms.OpenFileDialog ofd = new OpenFileDialog();
                ofd.DefaultExt = ".xml";
                ofd.Filter = "xml instruction file|*.xml";
                ofd.ShowDialog();

                if (ofd.FileName != "")
                {
                    MatterSphereBundlerService.MatterSphereBundlerServiceClient bsc = new MatterSphereBundlerService.MatterSphereBundlerServiceClient(bhb, epa);
                    bsc.ProcessBundles(ofd.FileName,txtTestKey.Text);
                    MessageBox.Show("instruction sent to service");
                }
                else
                {
                    MessageBox.Show("cancelled");
                }

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void frmTest_Load(object sender, EventArgs e)
        {
            txtServer.Text = Environment.MachineName;
        }

        private void btnValidateKey_Click(object sender, EventArgs e)
        {
            string server = txtServer.Text;
            string port = txtPort.Text;

            string Uri = string.Format("http://{0}:{1}/{2}", server, port, service);

            System.ServiceModel.BasicHttpBinding bhb = new System.ServiceModel.BasicHttpBinding();
            System.ServiceModel.EndpointAddress epa = new System.ServiceModel.EndpointAddress(Uri);

            try
            {
                MatterSphereBundlerService.MatterSphereBundlerServiceClient bsc = new MatterSphereBundlerService.MatterSphereBundlerServiceClient(bhb, epa);
                if(bsc.ValidateKey(txtTestKey.Text))
                    MessageBox.Show("Key matched to server");
                else
                    MessageBox.Show("Invalid Key");
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
    }
}
