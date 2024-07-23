using System;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.Admin
{
    public partial class frmTestOMSTypeBrowser : Form
    {
        OMSFile f = null;
        public frmTestOMSTypeBrowser()
        {
            InitializeComponent();
            f = FWBS.OMS.UI.Windows.Services.SelectFile();
            ucOMSTypeBrowser1.Connect(f);
            ucOMSTypeBrowser1.BrowserSelectedValue = "FILE";

        }

        private void ucOMSTypeBrowser1_BrowserChanged(object sender, EventArgs e)
        {
            switch (ucOMSTypeBrowser1.BrowserSelectedValue)
            {
                case "CLIENT":
                    ucOMSTypeBrowser1.Connect(f.Client);
                    break;
                case "FILE":
                    ucOMSTypeBrowser1.Connect(f);
                    break;
                default:
                    break;
            }
        }

        private void ucOMSTypeBrowser1_DefaultClick(object sender, EventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("Test");
        }
    }
}
