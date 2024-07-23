using System;
using System.Configuration;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using ELPLServicesLibrary;

namespace ELPLTestForm
{
    public partial class ELPLTestForm : Form
    {
        #region Constructors

        #region Form1
        public ELPLTestForm()
        {
            InitializeComponent();

            URL = ConfigurationManager.AppSettings["URL"];
            UserName = ConfigurationManager.AppSettings["UserName"];
            Password = ConfigurationManager.AppSettings["Password"];
            AsUser = ConfigurationManager.AppSettings["AsUser"];
        }
        #endregion Form1

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

        private ELPLServices1 CreateService()
        {
            ELPLLoginDetails loginDetails = new ELPLLoginDetails
            {
                Url = URL,
                UserName = UserName,
                UserPassword = Password,
                AsUser = AsUser
            };
            return new ELPLServices1(loginDetails, new TokenStorageProvider());
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
            try
            {
                var elplServices1 = CreateService();
                var claimInfo = elplServices1.AcknowledgeRejectedClaim(txtActivityEngineGuid.Text, txtClaimID.Text);
                
                var output = new StringBuilder("Acknowledged Rejected Claim. Claim Info data is now:\n\n");
                output.AppendLine("Activity Engine Guid: " + claimInfo.activityEngineGuid);
                output.AppendLine("Claim ID: " + claimInfo.applicationId);
                output.AppendLine("Phase Cache Name: " + claimInfo.phaseCacheName);
                output.AppendLine("Phase Cache ID: " + claimInfo.phaseCacheId);
                MessageBox.Show(this, output.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion Methods

        
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


        private void btnGetRejectedReason_Click(object sender, EventArgs e)
        {
            try
            {
                var elplServices1 = CreateService();
                var claimXML = elplServices1.GetClaim(txtClaimID.Text);
                var rejectionCode = GetRejectionReasonCodeFromClaimXML(claimXML);
                MessageBox.Show(this, "Your rejection reason code is... " + rejectionCode);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private string GetRejectionReasonCodeFromClaimXML(string claimXML)
        {
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(claimXML);

            var xmlNodeList = doc.GetElementsByTagName("Rejection");

            foreach (XmlNode xmlNode in xmlNodeList)
            {
                foreach (XmlNode attribute in xmlNode.Attributes)
                {
                    if (attribute.Name == "RejectionReasonCode")
                    {
                        return attribute.Value;
                    }
                }
            }

            return "-1";
        }
    }
}
