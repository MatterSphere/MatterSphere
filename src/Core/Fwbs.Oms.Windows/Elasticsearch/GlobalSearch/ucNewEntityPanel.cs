using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.UI.Elasticsearch.GlobalSearch
{
    public partial class ucNewEntityPanel : UserControl
    {
        public event NewOMSTypeWindowEventHandler _omsTypeWindow;

        public ucNewEntityPanel()
        {
            InitializeComponent();
        }

        private void btnNewContact_Click(object sender, System.EventArgs e)
        {
            FWBS.OMS.Contact contact = FWBS.OMS.UI.Windows.Services.Wizards.CreateContact(true);
            if (contact != null)
            {
                NewOMSTypeWindowEventArgs eva = new NewOMSTypeWindowEventArgs(contact);
                OpenOMSTypeWindow(this, eva);
            }
        }

        private void btnNewClient_Click(object sender, System.EventArgs e)
        {
            FWBS.OMS.Client client = FWBS.OMS.UI.Windows.Services.Wizards.CreateClient(true);
            if (client != null)
            {
                NewOMSTypeWindowEventArgs eva = new NewOMSTypeWindowEventArgs(client);
                OpenOMSTypeWindow(this, eva);
            }
        }

        private void OpenOMSTypeWindow(object sender, NewOMSTypeWindowEventArgs e)
        {
            if (_omsTypeWindow != null)
            {
                _omsTypeWindow(this, e);
            }
            else
            {
                FWBS.OMS.UI.Windows.Services.ShowOMSType(e.OMSObject, e.DefaultPage);
            }
                
        }
    }
}
