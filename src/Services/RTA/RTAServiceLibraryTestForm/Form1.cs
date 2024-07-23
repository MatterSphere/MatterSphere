using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Windows.Forms;
using RTAServicesLibrary;

namespace RTAServiceLibraryTestForm
{
    public partial class Form1 : Form
    {
        RTAServices1 rtaServices1;
        RTAServices2 rtaServices2;


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

            rtaServices1 = CreateService<RTAServices1>();
            rtaServices2 = CreateService<RTAServices2>();
        }

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

        private void btnSetRTAServices1_Click(object sender, EventArgs e)
        {
            rtaServices1 = CreateService<RTAServices1>();
            rtaServices2 = CreateService<RTAServices2>();
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
                var notifications = rtaServices1.GetNotificationsList();
                dgNotifications.DataSource = notifications;
                lblRowCountValue.Text = dgNotifications.Rows.Count.ToString();
            });
        }


        private void btnGetClaim_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var claimID = txtClaimID.Text;
                var claimData = rtaServices1.GetClaim(claimID);
                txtClaimXML.Text = claimData;
            });
        }


        private void btnGetTransferredClaimData_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var claimID = txtClaimID.Text;
                var claimData = txtClaimXML.Text;
                var getClaimTransferData = new ClaimsPortal.BulkTransfer.GetClaimTransferData("RTA");
                var transferDataObjects = getClaimTransferData.Get(claimID, claimData);
                WriteToTransferredClaimsTextBox(transferDataObjects);
            });
        }


        private void btnGetTransferredNotifications_Click(object sender, EventArgs e)
        {
            Execute(() => 
            {
                var transferNotifications = new ClaimsPortal.BulkTransfer.Notifications("RTA");
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


        private void btnGetInsurer_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var results = rtaServices1.SeachCompensators(txtInsurer.Text, null);
                dgInsurers.DataSource = results;
            });
        }

        
        private void btnSubmitISP_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var ispr = GatherDataForISP();
                var claimInfo = rtaServices2.AddInterimSPFRequest<RTAServicesLibraryV5.InterimSettlementPackRequest>("88D32330-A121-11E6-9212-005056800B3C", "0000000000153928", ispr);
            });
        }


        private RTAServicesLibraryV5.InterimSettlementPackRequest GatherDataForISP()
        {
            var ispr = new RTAServicesLibraryV5.InterimSettlementPackRequest();
            ispr.ClaimantLosses = GetClaimantLosses();
            ispr.ClaimantRepresentative = GetClaimantRepresentativeData();
            ispr.InterimPayment = GetInterimPaymentData();
            ispr.MedicalReport = GetMedicalReportData();
            ispr.StatementOfTruth = GetStatementOfTruthData();
            return ispr;
        }


        private RTAServicesLibraryV5.InterimSettlementPackRequestStatementOfTruth GetStatementOfTruthData()
        {
            var statementOfTruth = new RTAServicesLibraryV5.InterimSettlementPackRequestStatementOfTruth();
            statementOfTruth.RetainedSignedCopy = RTAServicesLibraryV5.C00_YNFlag.Item1;
            statementOfTruth.SignatoryType = RTAServicesLibraryV5.C16_SignatoryType.S;
            return statementOfTruth;
        }


        private RTAServicesLibraryV5.InterimSettlementPackRequestMedicalReport GetMedicalReportData()
        {
            var medicalData = new RTAServicesLibraryV5.InterimSettlementPackRequestMedicalReport();
            medicalData.MedicalReportStage2_1 = RTAServicesLibraryV5.C22_MedicalReport.Item1;
            return medicalData;
        }


        private RTAServicesLibraryV5.InterimSettlementPackRequestInterimPayment GetInterimPaymentData()
        {
            var interimPayment = new RTAServicesLibraryV5.InterimSettlementPackRequestInterimPayment();
            var claimantRequest = new RTAServicesLibraryV5.CT_A2A_ClaimantRequestForInterimPayment();
            claimantRequest.ReasonsForInterimPaymentRequest = "Want some money to tide us over";
            interimPayment.ClaimantRequestForInterimPayment = claimantRequest;
            return interimPayment;
        }


        private RTAServicesLibraryV5.InterimSettlementPackRequestClaimantRepresentative GetClaimantRepresentativeData()
        {
            var claimantRepresentative = new RTAServicesLibraryV5.InterimSettlementPackRequestClaimantRepresentative();
            
            var companyDetails = new RTAServicesLibraryV5.CT_A2A_CompanyDetails();
            companyDetails.ContactName = "Dan";
            companyDetails.ContactSurname = "Stonebanks";
            companyDetails.EmailAddress = "Dan@Stonebanks.com";
            companyDetails.ReferenceNumber = "DEF45454546";
            companyDetails.TelephoneNumber = "01654788736";
            claimantRepresentative.CompanyDetails = companyDetails;

            var medCoCase = new RTAServicesLibraryV5.CT_A2A_MedCoCase();
            medCoCase.SoftTissue = RTAServicesLibraryV5.C00_YNFlag.Item1;
            medCoCase.MedCoCaseID = "111/1";
            claimantRepresentative.MedCoCase = medCoCase;

            return claimantRepresentative;
        }


        private RTAServicesLibraryV5.CT_A2A_ClaimantLosses_Interim[] GetClaimantLosses()
        {
            var claimantLosses = new RTAServicesLibraryV5.CT_A2A_ClaimantLosses_Interim[2];

            var generalLosses = new RTAServicesLibraryV5.CT_A2A_ClaimantLosses_Interim();
            generalLosses.Comments = "General losses";
            generalLosses.EvidenceAttached = RTAServicesLibraryV5.C00_YNFlag.Item1;
            generalLosses.GrossValueClaimed = 1500;
            generalLosses.ItemBeingPursued = RTAServicesLibraryV5.C00_YNFlag.Item1;
            generalLosses.ItemBeingPursuedSpecified = true;
            generalLosses.LossType = RTAServicesLibraryV5.C18_LossType_A2A_R3.Item11;
            generalLosses.PercContribNegDeductions = 0;
            claimantLosses[0] = generalLosses;

            var someOtherLoss = new RTAServicesLibraryV5.CT_A2A_ClaimantLosses_Interim();
            someOtherLoss.Comments = "Some other loss";
            someOtherLoss.EvidenceAttached = RTAServicesLibraryV5.C00_YNFlag.Item1;
            someOtherLoss.GrossValueClaimed = 1000;
            someOtherLoss.ItemBeingPursued = RTAServicesLibraryV5.C00_YNFlag.Item1;
            someOtherLoss.ItemBeingPursuedSpecified = true;
            someOtherLoss.LossType = RTAServicesLibraryV5.C18_LossType_A2A_R3.Item3;
            someOtherLoss.PercContribNegDeductions = 0;
            claimantLosses[1] = someOtherLoss;

            return claimantLosses;
        }


        private void btnSearchClaims_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var versionStamp = rtaServices1.GetSystemProcessVersion(txtClaimID.Text, rtaServices1);
            });
        }

        private void btnSearchCompensators_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var results = rtaServices1.SearchCompensatorsByInsurerIndex(txtCompensator.Text, null);
                dgCompensators.DataSource = results;
            });
        }

        private void btnGetOrganisation_Click(object sender, EventArgs e)
        {
            Execute(() =>
            {
                var selectedCell = dgCompensators.SelectedCells[0].Value.ToString();

                if (selectedCell != null)
                {
                    var results = rtaServices1.GetOrganisation(selectedCell);
                    dgCompensators.DataSource = results;
                }
            });
        } 
    }
}