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
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class GeneralUnitTest
    {

        private const string RTA_URL_TEST = "http://piptesta2a.crif.com/PIP.WS/PIPWS?wsdl";

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
        
        public GeneralUnitTest()
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
        /// Get Services As Claimant
        /// </summary>
        /// <returns></returns>
        private RTAServices1 GetServicesAsClaimant()
        {
            RTAServices1 services = new RTAServices1(RTA_URL_TEST, CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER);
            return services;
        }

        /// <summary>
        /// Get Services As Defendant
        /// </summary>
        /// <returns></returns>
        private RTAServices1 GetServicesAsDefendant()
        {
            RTAServices1 services = new RTAServices1(RTA_URL_TEST, COMP_A2A_LOGIN_USERNAME, COMP_A2A_LOGIN_PASSWORD, COMP_A2A_LOGIN_ASUSER);
            return services;
        }

        [TestMethod]
        public void GetClaimsStatus()
        {
            RTAServices1 services = GetServicesAsClaimant();
            claimInfo info = services.GetClaimsStatus(Application_Id);

            Assert.AreNotEqual(null, info, "Get Cliam Status return null");
            Assert.AreEqual(Application_Id, info.applicationId, "GetClaimsStatus returned Application ID not valid"); 
        }

        [TestMethod]
        public void GetHospitalsList()
        {
            RTAServices1 services = GetServicesAsClaimant();
            hospital[] hospitals = services.GetHospitalsList(HOSPITAL_NAME, "");

            Assert.AreNotEqual(null, hospitals, "Hospital List is null");

            List<RTAServicesLibrary.PIPService.hospital> listHospitals = hospitals.ToList<RTAServicesLibrary.PIPService.hospital>();
            Assert.AreNotEqual(0, listHospitals.Count, "No Hospitals returned");
        }

        [TestMethod]
        public void GetNotificationsList()
        {
            //  Login as Claimant Representative
            RTAServices1 services = GetServicesAsClaimant();
        }

        [TestMethod]
        public void SeachCompensators()
        {
            RTAServices1 services = GetServicesAsClaimant();
        }

        [TestMethod]
        public void GetAttachmentsList()
        {
            RTAServices1 services = GetServicesAsClaimant();

            attachment[] atts = services.GetAttachmentsList(Application_Id);
            Assert.AreNotEqual(null, atts, "GetAttachmentsList return null");

            List<attachment> attachments = atts.ToList<attachment>();
            Assert.AreNotEqual(0, attachments.Count, "GetAttachmentsList returned no Attachments");
        }

        [TestMethod]
        public void GetAttachment()
        {
            RTAServices1 services = GetServicesAsClaimant();

            string attachmentGuid = "";

            attachment att = services.GetAttachment(attachmentGuid);
            Assert.AreNotEqual(null, att, "GetAttachment return null");
        }

        [TestMethod]
        public void GetClaimList()
        {
            RTAServices1 services = GetServicesAsClaimant();
        }
    }
}
