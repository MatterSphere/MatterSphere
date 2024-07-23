using System;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using RTAServicesLibrary;


namespace RTAServiceLibraryTestForm
{
    public partial class RTATestForm : Form
    {
        #region Members

        RTAServices1 rtaServices1 = null;
        RTAServices2 rtaServices2 = null;

        #endregion Members


        #region Properties

        public string URL
        {
            get
            {
                return txtURL.Text;
            }
            private set
            {
                txtURL.Text = value;
            }
        }


        public string UserName
        {
            get
            {
                return txtUserName.Text;
            }
            private set
            {
                txtUserName.Text = value;
            }
        }


        public string Password
        {
            get
            {
                return txtPassword.Text;
            }
            private set
            {
                txtPassword.Text = value;
            }
        }


        public string AsUser
        {
            get
            {
                return txtAsUser.Text;
            }
            private set
            {
                txtAsUser.Text = value;
            }
        }

        #endregion Properties


        #region Constructors

        public RTATestForm()
        {
            InitializeComponent();

            URL = ConfigurationManager.AppSettings["URL"];
            UserName = ConfigurationManager.AppSettings["UserName"];
            Password = ConfigurationManager.AppSettings["Password"];
            AsUser = ConfigurationManager.AppSettings["AsUser"];

            rtaServices1 = CreateService<RTAServices1>();
            rtaServices2 = CreateService<RTAServices2>();
        }

        #endregion Constructors


        #region Events

        #region cmdCheckPostcode_Click
        private void cmdCheckPostcode_Click(object sender, EventArgs e)
        {
            if (!isPostCodeFormatValid(txtPostCode.Text))
            {
                MessageBox.Show(this, "Post Code is in incorrect format.");
            }
            else
            {
                MessageBox.Show(this, "Post Code looks good!");
            }
        }
        #endregion cmdCheckPostcode_Click

        #endregion Events


        #region Methods

        private T CreateService<T>() where T : RTAServiceBase
        {
            RTALoginDetails loginDetails = new RTALoginDetails
            {
                Url = URL,
                UserName = UserName,
                UserPassword = Password,
                AsUser = AsUser
            };
            TokenStorageProvider tokenStorageProvider = new TokenStorageProvider();
            return (T)Activator.CreateInstance(typeof(T), loginDetails, tokenStorageProvider);
        }

        #region isPostCodeFormatValid
        private bool isPostCodeFormatValid(string postCode)
        {
            if (!StaticFunctions.PostCodeValidation(postCode))
            {
                return false;
            }

            return true;
        }
        #endregion isPostCodeFormatValid

        private void btnSendAcknowledgement_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var claimInfo = rtaServices1.AcknowledgeRejectedClaim(txtActivityEngineGuid.Text, txtClaimID.Text);
                OutputClaimInfoData(claimInfo, "Acknowledged Rejected Claim. Claim Info data is now:\n\n");
            });
        }


        private void btnAcknowledgeAllDamagesAgreed_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var claimInfo = rtaServices2.AcknowledgeAllDamagesAgreed(txtClaimID.Text, txtActivityEngineGuid.Text);
                OutputClaimInfoData(claimInfo, "Acknowledged All Damages Agreed. Claim Info data is now:\n\n");
            });
        }


        private void OutputClaimInfoData(RTAServicesLibrary.PIPService.claimInfo claimInfo, string messagePrefix)
        {
            var output = new StringBuilder(messagePrefix);
            output.AppendLine("Activity Engine Guid: " + claimInfo.activityEngineGuid);
            output.AppendLine("Claim ID: " + claimInfo.applicationId);
            output.AppendLine("Phase Cache Name: " + claimInfo.phaseCacheName);
            output.AppendLine("Phase Cache ID: " + claimInfo.phaseCacheId);
            MessageBox.Show(this, output.ToString());
        }


        private void btnGetOrganisation_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var organisation = rtaServices1.GetOrganisation(txtOrganisationCode.Text);
                MessageBox.Show(this, string.Format("Organisation Name: {0}", organisation.Rows[0]["organisationName"]));
            });
        }


        private void Execute(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Methods
    }
}
