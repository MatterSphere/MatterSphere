using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ELPLServicesLibrary;

namespace ELPLTestForm
{
    public partial class Form1 : Form
    {
        private ELPLServices1 elplServices1;

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
                return txtUsername.Text;
            }
            private set
            {
                txtUsername.Text = value;
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


        public Form1()
        {
            InitializeComponent();

            URL = ConfigurationManager.AppSettings["URL"];
            UserName = ConfigurationManager.AppSettings["UserName"];
            Password = ConfigurationManager.AppSettings["Password"];
            AsUser = ConfigurationManager.AppSettings["AsUser"];

            elplServices1 = CreateService();
        }

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

        private void btnSetRTAServices1_Click(object sender, EventArgs e)
        {
            elplServices1 = CreateService();
        }


        private void Execute(Action action)
        {
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                action();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }


        private void btnGetNotifications_Click(object sender, EventArgs e)
        {
            Execute(() => 
            { 
                var notifications = elplServices1.GetNotificationsList();
                dgNotifications.DataSource = notifications;
                lblRowCountValue.Text = dgNotifications.Rows.Count.ToString();
            });
        }


        private void btnGetClaim_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var claimID = txtClaimID.Text;
                var claimData = elplServices1.GetClaim(claimID);
                txtClaimXML.Text = claimData;
            });
        }


        private void btnGetTransferredClaimData_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var claimID = txtClaimID.Text;
                var claimData = txtClaimXML.Text;
                var getClaimTransferData = new ClaimsPortal.BulkTransfer.GetClaimTransferData("ELPL");
                var transferDataObjects = getClaimTransferData.Get(claimID, claimData);
                WriteToTransferredClaimsTextBox(transferDataObjects);
            });
        }


        private void btnGetTransferredNotifications_Click(object sender, EventArgs e)
        {
            Execute(() => 
            {
                var transferNotifications = new ClaimsPortal.BulkTransfer.Notifications("ELPL");
                var transferDataObjects = transferNotifications.Get((DataTable)(dgNotifications.DataSource));
                WriteToTransferredClaimsTextBox(transferDataObjects);
            });
        }


        private void WriteToTransferredClaimsTextBox(List<ClaimsPortal.BulkTransfer.TransferData> transferDataObjects)
        {
            var message = new StringBuilder();

            foreach (ClaimsPortal.BulkTransfer.TransferData transferData in transferDataObjects)
            {
                message.AppendLine(string.Format("ClaimID: {0}", transferData.ClaimID));
                message.AppendLine(string.Format("Source: {0}", transferData.Source));
                message.AppendLine(string.Format("Destination: {0}", transferData.Destination));
                message.AppendLine(string.Format("Date of Transfer: {0}", transferData.DateOfTransfer));
                message.AppendLine(string.Format("Full message: {0}", transferData.FullMessage));
                message.AppendLine();
            }

            txtTransferredClaimData.Text = message.ToString();
        }

        
        private void btnSearchCompensators_Click(object sender, EventArgs e)
        {
            Execute(() => {
                var results = elplServices1.SearchCompensatorsByInsurerIndex(txtCompensator.Text, null);
                dgCompensators.DataSource = results;
            });
        }


        private void btnGetClaims_Click(object sender, EventArgs e)
        {
            Execute(() => {
                var results = elplServices1.GetClaimsList();
                dgClaims.DataSource = results;
            });
        } 
    }
}