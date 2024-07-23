using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RTAServicesLibrary;
using RTAServicesLibrary.PIPService;

namespace RTATestProject
{
    /// <summary>
    /// Summary description for Stage2Test
    /// </summary>
    [TestClass]
    public class Stage2Test
    {
        #region Private Data

        private const string RTA_URL_TEST = "http://piptesta2a.crif.com/PIP.WS/PIPWS?WSDL";

        //  Test Claim Data
        private const string Activity_Engine_Guid = "6FF8F890-25DA-11DF-8E47-0000D4074612";
        private const string Application_Id = "0000000000001126";

        //  Organisation
        private const string ORG_CR = "Claimant Representative";
        private const string ORG_CM = "Defendant Insurer";

        //  CR - A2A Login Details
        private const string CR_A2A_LOGIN_USERNAME = "Username1";
        private const string CR_A2A_LOGIN_PASSWORD = "Password1";
        private const string CR_A2A_LOGIN_ASUSER = "Username1-1";

        //  COMP - A2A Login Details
        private const string COMP_A2A_LOGIN_USERNAME = "Username2";
        private const string COMP_A2A_LOGIN_PASSWORD = "Password2";
        private const string COMP_A2A_LOGIN_ASUSER = "Username2-2";

        //  Other Data
        private const string HOSPITAL_NAME = "Northampton";
        private const string HOSPITAL_POSTCOSE = "NN1 2AB";

        //  Add Claim Status
        private string ActivityEngineGuid = string.Empty;
        private string ApplicationID = string.Empty;
        private string PhaseCacheID = string.Empty;
        private string PhaseCacheName = string.Empty;

        //  Phase Cache ID 
        private const string PHASECACHEID_CLAIMSUBMITTED = "ClaimSubmitted";
        private const string PHASECACHEID_ARTICLE75DECISION = "Article75Decision";
        private const string PHASECACHEID_LIABILITYDECISION = "LiabilityDecision";
        private const string PHASECACHEID_LIABILITYADMITTED = "LiabilityAdmitted";

        #endregion Private Data

        public Stage2Test()
        {
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        /// <summary>
        /// Get Interim Settlement Pack Request
        /// </summary>
        /// <returns></returns>
        private InterimSettlementPackRequest GetInterimSettlementPackRequest()
        {
            InterimSettlementPackRequest request = new InterimSettlementPackRequest();

            //  Claimant Representative
            InterimSettlementPackRequestClaimantRepresentative representative = new InterimSettlementPackRequestClaimantRepresentative();
            CT_A2A_CompanyDetails companyDetails = new CT_A2A_CompanyDetails();
            companyDetails.ContactMiddleName = "";
            companyDetails.ContactName = "";
            companyDetails.ContactSurname = "";
            companyDetails.EmailAddress = "";
            companyDetails.ReferenceNumber = "";
            companyDetails.TelephoneNumber = "";
            representative.CompanyDetails = companyDetails;
            request.ClaimantRepresentative = representative;

            //  Medical Report
            InterimSettlementPackRequestMedicalReport report = new InterimSettlementPackRequestMedicalReport();
            report.MedicalReportStage2_1 = C22_MedicalReport.Item0;
            request.MedicalReport = report;

            //  Interim Payment
            InterimSettlementPackRequestInterimPayment InterimPayment = new InterimSettlementPackRequestInterimPayment();
            CT_A2A_ClaimantRequestForInterimPayment requestForInterimPayment = new CT_A2A_ClaimantRequestForInterimPayment();
            requestForInterimPayment.ReasonsForInterimPaymentRequest = "";
            InterimPayment.ClaimantRequestForInterimPayment = requestForInterimPayment;
            request.InterimPayment = InterimPayment;

            //  Claimant Losses
            CT_A2A_ClaimantLosses_Interim[] toDateList = new CT_A2A_ClaimantLosses_Interim[1];
            CT_A2A_ClaimantLosses_Interim toDate = new CT_A2A_ClaimantLosses_Interim();
            toDate.LossType = C18_LossType.Item0;
            toDate.EvidenceAttached = RTAServicesLibrary.C00_YNFlag.Item1;
            toDate.Comments = "";
            toDate.GrossValueClaimed = 1;
            toDate.PercContribNegDeductions = 1;
            toDateList[0] = toDate;
            request.ClaimantLosses = toDateList;

            //  Statement Of Truth
            InterimSettlementPackRequestStatementOfTruth statementOfTruth = new InterimSettlementPackRequestStatementOfTruth();
            statementOfTruth.RetainedSignedCopy = RTAServicesLibrary.C00_YNFlag.Item1;
            statementOfTruth.SignatoryType = RTAServicesLibrary.C16_SignatoryType.S;
            request.StatementOfTruth = statementOfTruth;

            return request;
        }

        /// <summary>
        /// Get Interim Settlement Pack Response
        /// </summary>
        /// <returns></returns>
        private InterimSettlementPackResponse GetInterimSettlementPackResponse()
        {
            InterimSettlementPackResponse response = new InterimSettlementPackResponse();

            return response;
        }

        /// <summary>
        /// Get Services As Claimant
        /// </summary>
        /// <returns></returns>
        private RTAServices2 GetServicesAsClaimant()
        {
            RTAServices2 services = new RTAServices2(RTA_URL_TEST, CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER);
            return services;
        }

        /// <summary>
        /// Get Services As Defendant
        /// </summary>
        /// <returns></returns>
        private RTAServices2 GetServicesAsDefendant()
        {
            RTAServices2 services = new RTAServices2(RTA_URL_TEST, COMP_A2A_LOGIN_USERNAME, COMP_A2A_LOGIN_PASSWORD, COMP_A2A_LOGIN_ASUSER);
            return services;
        }

        [TestMethod]
        public void SetInterimPaymentNeeded()
        {
            bool isInterimPaymentNeeded = false;

            RTAServices2 services = GetServicesAsClaimant();

            claimInfo info = services.SetInterimPaymentNeeded(Activity_Engine_Guid, Application_Id, isInterimPaymentNeeded);
            Assert.AreNotEqual(null, info, "Set Interim Payment Needed return null");
            Assert.AreNotEqual(0, info.applicationId, "SetInterimPaymentNeeded returned Application ID not valid");
        }

        [TestMethod]
        public void AddInterimSettlementPackRequest()
        {
            RTAServices2 services = GetServicesAsClaimant();

            InterimSettlementPackRequest request = GetInterimSettlementPackRequest();

            claimInfo info = services.AddInterimSPFRequest(Activity_Engine_Guid, Application_Id, request);
            Assert.AreNotEqual(null, info, "Add Interim Settlement Pack Request return null");
            Assert.AreNotEqual(0, info.applicationId, "AddInterimSettlementPackRequest returned Application ID not valid");
        }

        [TestMethod]
        public void AddInterimSettlementPackResponse()
        {
            RTAServices2 services = GetServicesAsDefendant();

            InterimSettlementPackResponse response = GetInterimSettlementPackResponse();

            claimInfo info = services.AddInterimSPFResponse(Activity_Engine_Guid, Application_Id, response);
            Assert.AreNotEqual(null, info, "Add Interim Settlement Pack Response return null");
            Assert.AreNotEqual(0, info.applicationId, "AddInterimSettlementPackResponse returned Application ID not valid");
        }

        [TestMethod]
        public void SetStage2_1Payment()
        {
            bool isStage21Paid = false;

            RTAServices2 services = GetServicesAsClaimant();

            claimInfo info = services.SetStage21Payments(Activity_Engine_Guid, Application_Id, isStage21Paid);
            Assert.AreNotEqual(null, info, "Set Stage 2.1 Payment return null");
            Assert.AreNotEqual(0, info.applicationId, "SetStage2_1Payment returned Application ID not valid");
        }

        [TestMethod]
        public void AcceptPartialInterimPayment()
        {
            bool isPartialInterimPaymentAccepted = false;

            RTAServices2 services = GetServicesAsClaimant();

            claimInfo info = services.AcceptPartialInterimPayment(Activity_Engine_Guid, Application_Id, isPartialInterimPaymentAccepted);
            Assert.AreNotEqual(null, info, "Accept Partial Interim Payment return null");
            Assert.AreNotEqual(0, info.applicationId, "AcceptPartialInterimPayment returned Application ID not valid");
        }

        [TestMethod]
        public void AcknowledgePartialPaymentDecision()
        {
            RTAServices2 services = GetServicesAsDefendant();

            claimInfo info = services.AcknowledgePartialPaymentDecision(Activity_Engine_Guid, Application_Id);
            Assert.AreNotEqual(null, info, "Acknowledge Partial Payment Decision return null");
            Assert.AreNotEqual(0, info.applicationId, "AcknowledgePartialPaymentDecision returned Application ID not valid");
        }
    }
}
