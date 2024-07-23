using System;
using System.Windows.Forms;
using FWBS.OMS;

namespace QuickClientCreation
{
    public partial class Actions : Form
    {
        #region Constructors
        public Actions()
        {
            InitializeComponent();

            if (!Session.CurrentSession.IsLoggedIn)
            {
                if (!CheckLogin())
                {
                    Application.Exit();
                    return;
                }
            }
            SetLoggedInAppearance();

        }
        #endregion
        #region Methods
        private void SetLoggedInAppearance()
        {
            string ConnectedString = Session.CurrentSession.Resources.GetResource("QIKCLICONMSG","You are currently logged in as {0}","").Text;

            lblConnectionInfo.Text = string.Format(ConnectedString, Session.CurrentSession.CurrentUser.FullName);

            btnConnect.Text = Session.CurrentSession.Resources.GetResource("DISCONNECT", "&Disconnect", "").Text;
            btnCreateClient.Text = Session.CurrentSession.Resources.GetResource("CREATECLIENT", "Create %CLIENT%", "").Text;
            btnCreateMatter.Text = Session.CurrentSession.Resources.GetResource("CREATEFILE", "Create %FILE%", "").Text;

            this.Text = Session.CurrentSession.Resources.GetResource("QIKCLIMAT", this.Text, "").Text;

            TerminologyParse();

            SetButtonState(true);
        }

        private void TerminologyParse()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (!string.IsNullOrEmpty(ctrl.Text))
                    ctrl.Text = Session.CurrentSession.Terminology.Parse(ctrl.Text, false);
            }

            this.Text = Session.CurrentSession.Terminology.Parse(this.Text, false);
        }

        private void SetButtonState(bool enabled)
        {
            btnCreateClient.Enabled = enabled;
            btnCreateMatter.Enabled = enabled;
        }

        public static bool CheckLogin()
        {
            //Can the loign screen not check the licences?
            if (!FWBS.OMS.UI.Windows.Services.CheckLogin())
                return false;

            if (!(FWBS.OMS.Session.CurrentSession.IsLicensedFor("QUICKCR") || FWBS.OMS.Session.CurrentSession.IsLicensedFor("SYSTEM")))
            {
                FWBS.OMS.UI.Windows.MessageBox.Show(Session.CurrentSession.Resources.GetResource("QIKCLILICMSG", "You are not licensed for Quick Client Creation", ""));

                FWBS.OMS.UI.Windows.Services.OMS.Disconnect();
                return CheckLogin();
            }

            return true;
        }
        #endregion
        #region Button Handlers
        private void btnConnect_Click(object sender, EventArgs e)
        {

            if (Session.OMS.IsLoggedIn)
            {
                this.lblConnectionInfo.Text = Session.CurrentSession.Resources.GetResource("QIKCLIDISMSG", "Please connect to perform an action", "").Text;
                FWBS.OMS.UI.Windows.Services.OMS.Disconnect();
            }
            else
            {
                if (CheckLogin())
                {
                    SetLoggedInAppearance();
                    return;
                }
            }
            
            this.btnConnect.Text = FWBS.OMS.Session.CurrentSession.RegistryRes("Connect", "&Connect");
            SetButtonState(false);
        }

        private void btnCreateClient_Click(object sender, EventArgs e)
        {
            try
            {
                FWBS.OMS.UI.Windows.Services.Wizards.CreateClient(true);
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }

        private void btnCreateMatter_Click(object sender, EventArgs e)
        {
            try
            {
                FWBS.OMS.UI.Windows.Services.Wizards.CreateFile(true);
            }
            catch (Exception ex)
            {
                FWBS.OMS.UI.Windows.ErrorBox.Show(ex);
            }
        }
        #endregion
    }
}
