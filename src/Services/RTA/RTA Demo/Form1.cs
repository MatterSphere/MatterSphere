using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using RTAServicesLibrary;
using RTAServicesLibrary.PIPService;

namespace RTA_Demo
{
    public partial class Form1 : Form
    {
        //  NOTES:
        //  CR  = Claimant Representative
        //  CM  = MIB / Self Insurer / Insurer 
        //  CNF = Claim Notification Form

        #region Private Fields

        //  Test Claim Data
        private const string Activity_Engine_Guid = "1958feb2-02af-11e0-bc46-0000d407460d";
        private const string Application_Id = "0000000000280333";

        //  A2A System Url
        private const string RTA_URL_TEST = "https://piptesta2a.crif.com/PIP.WSTK/PIPWSTK";
        private const string RTA_URL_LIVE = "https://www.rapidclaimsettlement.org.uk/PIP.WSTK/PIPWSTK";
        private const string A2A_SYSTEM_TEST = "Test System";
        private const string A2A_SYSTEM_LIVE = "Live System";

        //  Organisation
        private const string ORG_CR = "Claimant Representative";
        private const string ORG_CM = "Defendant Insurer";
        private const string ORG_CR2 = "Claimant Representative 2";
        private const string ORG_CM2 = "Defendant Insurer 2";

        private const string CR_A2A_LOGIN_USERNAME = "Username1";
        private const string CR_A2A_LOGIN_PASSWORD = "Password1";
        private const string CR_A2A_LOGIN_ASUSER = "Username1-1";
        private const int CR_A2A_MSP_USERID = 0;

        //  COMP - A2A Login Details
        private const string COMP_A2A_LOGIN_USERNAME = "Username2";
        private const string COMP_A2A_LOGIN_PASSWORD = "Password2";
        private const string COMP_A2A_LOGIN_ASUSER = "Username2-2";
        private const int COMP_A2A_MSP_USERID = 1;

        //  CR2 - A2A Login Details
        private const string CR2_A2A_LOGIN_USERNAME = "Username3";
        private const string CR2_A2A_LOGIN_PASSWORD = "Password3";
        private const string CR2_A2A_LOGIN_ASUSER = "Username3-3";
        private const int CR2_A2A_MSP_USERID = 0; // same as CR_A2A because of ASUSER

        //  COMP2 - A2A Login Details
        private const string COMP2_A2A_LOGIN_USERNAME = "Username4";
        private const string COMP2_A2A_LOGIN_PASSWORD = "Password4";
        private const string COMP2_A2A_LOGIN_ASUSER = "Username4-4";
        private const int COMP2_A2A_MSP_USERID = 3;

        //  CR - LIVE Login Details
        private const string CR_LIVE_LOGIN_USERNAME = "";
        private const string CR_LIVE_LOGIN_PASSWORD = "";
        private const string CR_LIVE_LOGIN_ASUSER = "";

        //  Yes/No Types
        private const string COMBO_NO = "No";
        private const string COMBO_YES = "Yes";

        //  Address Types
        private const string COMBO_ADDRESSTYPE_A = "As input";
        private const string COMBO_ADDRESSTYPE_F = "Business";
        private const string COMBO_ADDRESSTYPE_P = "Personal";

        //  Capacity Types
        private const string COMBO_CAPACITY_CONTRACT = "Insurer in Contract";
        private const string COMBO_CAPACITY_RTA = "RTA Insurer";
        private const string COMBO_CAPACITY_ARTICLE75 = "Article 75 Insurer";
        private const string COMBO_CAPACITY_MIB = "MIB";
        private const string COMBO_CAPACITY_OTHER = "Other";

        //  Sex Types
        private const string COMBO_SEX_MALE = "Male";
        private const string COMBO_SEX_FEMALE = "Female";
        private const string COMBO_SEX_NOTKNOWN = "Not Known";

        //  Insurer Types
        private const string COMBO_INSURERTYPE_INSURER = "Insurer";
        private const string COMBO_INSURERTYPE_SELFINSURED = "Self Insured";
        private const string COMBO_INSURERTYPE_MIB = "MIB";

        //  Claim Signatory
        private const string COMBO_CLAIMANT_SOLICITOR = "Claimant' Solicitor";
        private const string COMBO_CLAIMANT_IN_PERSON = "Claimant in Person";

        //  Total Loss
        private const string COMBO_TOTALLOSS_YES = "Yes";
        private const string COMBO_TOTALLOSS_NO = "No";
        private const string COMBO_TOTALLOSS_DONTKNOW = "Don't Know";

        //  Status
        private const string COMBO_STATUS_PERSONAL = "Personal";
        private const string COMBO_STATUS_BUSINESS = "Business";

        //  Liability Decision
        private const string COMBO_LIABILITY_ADMITTED = "Admitted";
        private const string COMBO_LIABILITY_ADMITTEDWITHNEGLIGENCE = "Admitted with Negligence";
        private const string COMBO_LIABILITY_NOTADMITTED = "Not Admitted";

        //  Payment Decision
        private const string COMBO_PAYMENTDECISION_AC = "AC";
        private const string COMBO_PAYMENTDECISION_NAC = "NAC";

        //  Loss type
        private const string COMBO_LOSSTYPE_POLICYEXCESS = "Policy Excess";
        private const string COMBO_LOSSTYPE_LOSSOFUSE = "Loss of Use";
        private const string COMBO_LOSSTYPE_CARHIRE = "Car Hire";
        private const string COMBO_LOSSTYPE_REPAIRCOSTS = "Repair Costs";
        private const string COMBO_LOSSTYPE_FARES = "Fares (taxis, buses, tube etc)";
        private const string COMBO_LOSSTYPE_MEDICALEXPENSES = "Medical Expenses";
        private const string COMBO_LOSSTYPE_COLTHING = "Colthing";
        private const string COMBO_LOSSTYPE_CARESERVICE = "Care Service";
        private const string COMBO_LOSSTYPE_LOSSEARNINGSCLAIMANT = "Loss of earnings for Claimant";
        private const string COMBO_LOSSTYPE_LOSSEARNINGSEMPLOYER = "Loss of earnings for Employer";
        private const string COMBO_LOSSTYPE_OTHERLOSSES = "Other Losses";
        private const string COMBO_LOSSTYPE_GENERALDAMAGES = "General Damages";

        //  Medical Reports
        private const string COMBO_MEDICALREPORT_0 = "0";
        private const string COMBO_MEDICALREPORT_1 = "1";
        private const string COMBO_MEDICALREPORT_2 = "2";
        private const string COMBO_MEDICALREPORT_3 = "3";
        private const string COMBO_MEDICALREPORT_4 = "4";

        //  Police Reported
        private const string COMBO_POLICE_NO = "No";
        private const string COMBO_POLICE_YES = "Yes";
        private const string COMBO_POLICE_NOTKNOWN = "Not Known";


        //  Default Country
        private const string UNITED_KINGDOM = "United Kingdom";

        public bool IsClaimMIB { get; set; }
        public bool IsCapacityMIB { get; set; }

        private string xmlFilter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";

        private readonly DataInitializer _dataInitializer;

        #endregion Private Fields

        #region Constructor

        public Form1()
        {
            InitializeComponent();

            txtURL.Text = ConfigurationManager.AppSettings["URL"];
            txtUsername.Text = ConfigurationManager.AppSettings["UserName"];
            txtPassword.Text = ConfigurationManager.AppSettings["Password"];
            txtAsUser.Text = ConfigurationManager.AppSettings["AsUser"];

            _dataInitializer = new DataInitializer();

            SetupClaimantCNF();
            SetupSendLiabilityDecision();

            tbClaimApplicationID.Text = Application_Id;
            cmbGetNotifications.Items.Add(ORG_CR);
            cmbGetNotifications.Items.Add(ORG_CM);
            cmbGetNotifications.Items.Add(ORG_CR2);
            cmbGetNotifications.Items.Add(ORG_CM2);
            cmbGetNotifications.SelectedIndex = 0;

            cmbA2ASystem.Items.Add(A2A_SYSTEM_TEST);
            cmbA2ASystem.Items.Add(A2A_SYSTEM_LIVE);
            cmbA2ASystem.SelectedIndex = 0;

            tbSearchHospitalName.Text = "Northampton";
            tbAttachmentGuid.Text = "1B04C1B9-793E-44D6-9370-054A2D637F34";
            tbGetOrganisationPath.Text = "/CR00009";

            SetUpYesNo(cmbStageOnePaymentReceived);
            cmbStageOnePaymentReceived.SelectedIndex = 1;

            //  Stage 2.1
            SetUpStageTwo();
            SetupInterimSettlementPackResponse();

            //  Validation Tests
            tbNationalInsuranceNumber.Text = "AA000100A";
            tbReferenceNumber.Text = "F681";
            tbPostCodeTest.Text = "NN7 3NU";
            tbValdateEmailAddress.Text = "info@fwbs.net";
            tbVehicleRegistrationValidation.Text = "KT08 FLT";

            //  Settlement Pack Request
            SetUpYesNo(cmbSPREvidenceAttached);
            SetUpLossType(cmbSPRLossType);
            SetUpMedicalReport(cmbMedicalReportInculded);
            SetUpYesNo(cmbSPRRetainedCopy);
            cmbSPRRetainedCopy.SelectedIndex = 1;
            SetUpSignatory(cmbSPRSignatory);
            tbSPRAgreementComments.Text = "Comments";
            tbSPRLossesComments.Text = "More Comments";
            cmbSPREvidenceAttached.SelectedIndex = 1;
            tbSPRGrossValueClaimed.Text = "1700";
            tbSPRPercContribHegDeductions.Text = "15";
            tbSPRContactName.Text = "Contact Name";
            tbSPRContactSurname.Text = "Contact Surname";
            tbSPRReferenceNumber.Text = "Reference Number";
            cmbMedicalReportInculded.SelectedIndex = 1;

            //  Settlement Pack Decision
            cmbSPDSettlementDecision.DataSource = DataCollection.SettlementPackDecisionList();
            cmbSPDIsGrossAmountAgreed.DataSource = DataCollection.YesNoList();
            cmbSPDLossType.DataSource = DataCollection.LossTypeList();
            tbSPDAgreementComments.Text = "Comments";
            tbSPDGrossAmount.Text = "1700";
            tbSPDInterimPaymentAmount.Text = "1700";
            tbSPDDefendantRepliesComments.Text = "More Comments";
            tbSPDDefendantRepliesGrossValueClaimed.Text = "1700";
            cmbSPDIsGrossAmountAgreed.SelectedIndex = 1;
            tbSPDPercContribNegDeductions.Text = "15";
            tbSPDRepresentativeMiddlename.Text = "Middlename";
            tbSPDRepresentativeName.Text = "Name";
            tbSPDRepresentativeSurname.Text = "Surname";
            tbSPDRepresentativeEmail.Text = "John@smith.co.uk";
            tbSPDRepresentativeReferenceNumber.Text = "123456789";
            tbSPDRepresentativeTelephoneNumber.Text = "01604 362728";
            tbSPDTotalCurrentTotal.Text = "1700";

            //  Settlement Pack Counter Offer
            tbSPCOAgreementComments.Text = "Comments";
            tbSPCOAgreementGrossAmount.Text = "1800";
            tbSPCOAgreementInterimPaymentAmount.Text = "1700";
            tbSPCOLossesComments.Text = "Losses Comments";
            cmbSPCOLossesEvidenceAttached.DataSource = DataCollection.YesNoList();
            tbSPCOLossesGrossValueClaimed.Text = "1800";
            cmbSPCOLossesLossType.DataSource = DataCollection.LossTypeList();
            tbSPCOLossesPercContribNegDeductions.Text = "15";

            //  Settlement Pack Counter Offer Decision
            tbSPCODAgreementComments.Text = "Comments";
            tbSPCODAgreementGrossAmount.Text = "1000";
            tbSPCODAgreementsInterimPaymentAmount.Text = "1000";
            tbSPCODLossesComments.Text = "Losses Comments";
            tbSPCODLossesGrossValueOffered.Text = "1000";
            cmbSPCODLossesIsGrossAmountAgreed.DataSource = DataCollection.YesNoList();
            cmbSPCODLossesLossType.DataSource = DataCollection.LossTypeList();
            tbSPCODLossesPercContribNegDeductions.Text = "15";
            tbSPCODTotalDeductions.Text = "1000";

            //  Settlement Pack Agreement
            cmbSPAAgreement.DataSource = DataCollection.YesNoList();
        }

        #endregion Constructor

        private T CreateService<T>(string url, string username, string password, string asUser, int? msUserId) where T : RTAServiceBase
        {
            RTALoginDetails loginDetails = new RTALoginDetails
            {
                Url = url,
                UserName = username,
                UserPassword = password,
                AsUser = asUser,
                MatterSphereUserId = msUserId
            };
            TokenStorageProvider tokenStorageProvider = new TokenStorageProvider();
            return (T)Activator.CreateInstance(typeof(T), loginDetails, tokenStorageProvider);
        }

        /// <summary>
        /// SetUp Stage Two
        /// </summary>
        private void SetUpStageTwo()
        {
            SetupInterimSettlementPackRequest();

            cmbSIPNAIsInterimPaymentNeeded.DataSource =  DataCollection.YesNoList();
            cmbSIPNAIsInterimPaymentNeeded.SelectedIndex = 1;
            cmbSIPNStageTwoPaid.DataSource = DataCollection.YesNoList();
            cmbSIPNIsPartialInterimPaymentAccepted.DataSource = DataCollection.YesNoList();

            cmbSetStage21Payments.DataSource = DataCollection.YesNoList();
            cmbAcceptPartialInterimPayment.DataSource = DataCollection.YesNoList();
        }

        /// <summary>
        /// Setup Interim Settlement Pack Request
        /// </summary>
        private void SetupInterimSettlementPackRequest()
        {
            cmbISPREvidenceAttached.DataSource = DataCollection.YesNoList();
            cmbISPRNumberOfMedicalReport.DataSource = DataCollection.MedicalReportList();
            cmbISPRLossType.DataSource = DataCollection.LossTypeList();
            cmbISPRPaymentMoreRequested.DataSource = DataCollection.YesNoList();

            cmbISPRPaymentMoreRequested.SelectedIndex = 1;
            cmbISPRNumberOfMedicalReport.SelectedIndex = 0;
            cmbISPRLossType.SelectedIndex = 2;

            tbISPRTelephoneNumber.Text = "01604 372828";
            tbISPRReferenceNumber.Text = "12345678";
            tbISPREmailAddress.Text = "";
            tbISPRContactSurname.Text = "Greaves";
            tbISPRContactName.Text = "Alan";
            tbISPRContactMiddleName.Text = "M";
            tbISPRComments.Text = "Test";
            tbISPRGrossValueClaimed.Text = "2000";
            tbISPRPercContribNegDeductions.Text = "15";
            tbISPRValueOfInterimRequest.Text = "1700";
            tbISPRReasonsForInterimPaymentRequest.Text = "Car Hire needed";
        }

        /// <summary>
        /// Setup Interim Settlement Pack Response
        /// </summary>
        private void SetupInterimSettlementPackResponse()
        {
            cmbIsGrossAmountAgreed.DataSource = DataCollection.YesNoList();
            cmbIsGrossAmountAgreed.SelectedIndex = 1;
            cmbISPDAddressType.DataSource = DataCollection.AddressTypeList();
            cmbISPDLossType.DataSource = DataCollection.LossTypeList();
            cmbISPDLossType.SelectedIndex = 2;


            //  Defendant Insurer
            tbISPDCompanyName.Text = "FWBS COMP";
            tbISPDContactName.Text = "James";
            tbISPDContactMiddleName.Text = "";
            tbISPDContactSurname.Text = "Brown";
            tbISPDEmailAddress.Text = "james.b@FWBSCOMP.com";
            tbISPDTelephoneNumber.Text = "10604 374848";
            tbISPDPolicyNumber.Text = "1001";
            tbISPDProviderAddress.Text = "";
            tbISPDReferenceNumber.Text = "12345678";

            //  Defendant Insurer Address
            tbISPDHouseName.Text = "";
            tbISPDHouseNumber.Text = "77";
            tbISPDStreet1.Text = "The Mews";
            tbISPDStreet2.Text = "";
            tbISPDDistrict.Text = "";
            tbISPDCity.Text = "Northampton";
            tbISPDCounty.Text = "Northamptonshire";
            tbISPDCountry.Text = "UK";

            //  Defendants Response
            tbISPDAddtionalComments.Text = "";
            tbISPDAmountInDispute.Text = "";
            tbISPDComments.Text = "";
            tbISPDDateOfResponse.Text = DateTime.Now.ToShortDateString();
            tbISPDGrossValueClaimed.Text = "2000";
            tbISPDPercContribNegDeductions.Text = "15";
            tbISPDValueOfferedAfterContrib.Text = "1500";
            tbISPDValueOfInterimPaymentAgreed.Text = "1500";
        }

        /// <summary>
        /// Setup Send Liability Decision
        /// </summary>
        private void SetupSendLiabilityDecision()
        {
            SetUpCapacity(cmbInsurerCapacity);
            cmbInsurerCapacity.SelectedIndex = 3;                       //  Select MIB
            SetUpYesNo(cmbInsurerNoAuthority);
            SetUpYesNo(cmbInsurerAccidentOccurred);
            cmbInsurerAccidentOccurred.SelectedIndex = 1;               //  Yes
            SetUpYesNo(cmbInsurerCausedByDefendant);
            cmbInsurerCausedByDefendant.SelectedIndex = 1;              //  Yes
            SetUpYesNo(cmbInsurerCausedSomeLossToTheClaimant);
            cmbInsurerCausedSomeLossToTheClaimant.SelectedIndex = 1;    //  Yes
            SetUpYesNo(cmbInsurerPreparedToProvideReabilitation);
            SetUpYesNo(cmbInsurerReabilitationProvided);
            SetUpYesNo(cmbInsurerAltVchlProvided);
            SetUpYesNo(cmbInsurerRepairsProvided);
            SetUpAddressType(cmbDefendantInsurerAddressType);
            SetUpLiabilityDecision(cmbInsurerLiabilityDecision);

            //  Liability Details
            tbInsurerOtherCapacity.Text = "";
            tbInsurerUnadmittedLiabilityReasons.Text = "";
            tbInsurerReabilitationDetails.Text = "";
            tbInsurerAltVhclDetails.Text = "";
            tbInsurerRepairsDetails.Text = "";

            //  Defendants Insurer Details
            tbDefendantInsurerContactName.Text = "Fred";
            tbDefendantInsurerContactMiddleName.Text = "";
            tbDefendantInsurerContactSurname.Text = "Smith";
            tbDefendantInsurerTelephone.Text = "01604 263738";
            tbDefendantInsurerEmail.Text = "Fred@Smith.co.uk";
            tbDefendantInsurerReferenceNumber.Text = "6473828";
            tbDefendantInsurerHouseName.Text = "";
            tbDefendantInsurerHouseNumber.Text = "77";
            tbDefendantInsurerStreet.Text = "The Mews";
            tbDefendantInsurerCity.Text = "Northampton";
            tbDefendantInsurerCountry.Text = "UK";

            //  Article 75 Decision
            SetUpYesNo(cmbArticle75Decision);
        }

        /// <summary>
        /// Setup Claimant CNF
        /// </summary>
        private void SetupClaimantCNF()
        {
            SetUpSignatory(cmbClaimantRepresentativeSignatory);
            cmbClaimantRepresentativeSignatory.BackColor = Color.LightYellow;
            SetUpYesNo(cmbClaimantRepresentativeRetainedCopy);
            cmbClaimantRepresentativeRetainedCopy.SelectedIndex = 1;
            cmbClaimantRepresentativeRetainedCopy.BackColor = Color.LightYellow;

            //  ClaimAndClaimantDetails - ClaimantRepresentative
            tbClaimantRepresentativeCompanyName.Text = "FWBS";
            tbClaimantRepresentativeContactName.Text = "Alan";
            tbClaimantRepresentativeContactSurname.Text = "Greaves";
            tbClaimantRepresentativeReferenceNumber.Text = "12345678";
            tbClaimantRepresentativeTelephone.Text = "01604 372828";
            tbClaimantRepresentativeStreet.Text = "12 Gold Street";
            tbClaimantRepresentativeCity.Text = "Northampton";
            tbClaimantRepresentativeCountry.Text = "England";
            tbClaimantRepresentativePostCode.Text = "NN1 6AF";
            SetUpAddressType(cmbClaimantRepresentativeAddressType);
            cmbClaimantRepresentativeAddressType.BackColor = Color.LightYellow;
            tbOICRefferenceNumber.Text = "OIC-123123";
            tbClaimantRepresentativeCompanyName.BackColor = Color.LightYellow;
            tbClaimantRepresentativeContactName.BackColor = Color.LightYellow;
            tbClaimantRepresentativeContactSurname.BackColor = Color.LightYellow;
            tbClaimantRepresentativeReferenceNumber.BackColor = Color.LightYellow;
            tbClaimantRepresentativeTelephone.BackColor = Color.LightYellow;
            tbClaimantRepresentativeStreet.BackColor = Color.LightYellow;
            tbClaimantRepresentativeCity.BackColor = Color.LightYellow;
            tbClaimantRepresentativeCountry.BackColor = Color.LightYellow;
            tbClaimantRepresentativePostCode.BackColor = Color.LightYellow;
            cmbClaimantRepresentativeAddressType.BackColor = Color.LightYellow;

            //  DefendantDetails 
            tbDefedantSurname.Text = "Thomas";
            tbDefendantAge.Text = "22";
            tbDefendantDescription.Text = "Six feet male";
            tbDefendantDetailsObtained.Text = "From Defendant";
            tbDefendantStreet.Text = "11 High Street";
            tbDefendantCity.Text = "Northampton";
            tbDefendantCountry.Text = "England";
            tbDefendantVehicleVRN.Text = "KP05 FAB";
            tbDefendantInsurerName.Text = "FWBS COMP";
            tbDefendantCompanyName.Text = "White's Deliverys";
            tbDefendantCompanyStreet.Text = "76 Pine Lane";
            tbDefendantCompanyCity.Text = "Northampton";
            tbDefendantCompanyCountry.Text = "England";
            SetUpStatus(cmbDefendantStatus);
            cmbDefendantStatus.BackColor = Color.LightYellow;

            SetUpAddressType(cmbDefendantAddressType);
            cmbDefendantAddressType.BackColor = Color.LightYellow;

            SetUpAddressType(cmbDefendantCompanyAddressType);
            cmbDefendantCompanyAddressType.BackColor = Color.LightYellow;

            SetUpSex(cmbDefendantSex);
            cmbDefendantSex.BackColor = Color.LightYellow;


            tbDefedantSurname.BackColor = Color.LightYellow;
            tbDefendantAge.BackColor = Color.LightYellow;
            tbDefendantDescription.BackColor = Color.LightYellow;
            tbDefendantDetailsObtained.BackColor = Color.LightYellow;
            tbDefendantStreet.BackColor = Color.LightYellow;
            tbDefendantCity.BackColor = Color.LightYellow;
            tbDefendantCountry.BackColor = Color.LightYellow;
            tbDefendantVehicleVRN.BackColor = Color.LightYellow;
            tbDefendantInsurerName.BackColor = Color.LightYellow;
            tbDefendantCompanyName.BackColor = Color.LightYellow;
            tbDefendantCompanyStreet.BackColor = Color.LightYellow;
            tbDefendantCompanyCity.BackColor = Color.LightYellow;
            tbDefendantCompanyCountry.BackColor = Color.LightYellow;
            cmbDefendantStatus.BackColor = Color.LightYellow;
            cmbDefendantSex.BackColor = Color.LightYellow;

            //  Claimant Details
            tbClaimantOccupation.Text = "Builder";
            tbClaimantDOB.Text = "1985-02-22";
            tbClaimantName.Text = "Tom";
            tbClaimantSurname.Text = "Watts";
            tbClaimantStreet.Text = "5 Kingsthore Road";
            tbClaimantCity.Text = "Northampton";
            tbClaimantCountry.Text = "England";
            tbClaimantNationalInsuranceNumber.Text = "HT001112A";
            tbClaimantNationalInsuranceNumberComments.Text = string.Empty;
            SetUpAddressType(cmbClaimantAddressType);
            cmbClaimantAddressType.BackColor = Color.LightYellow;

            tbClaimantOccupation.BackColor = Color.LightYellow;
            tbClaimantDOB.BackColor = Color.LightYellow;
            tbClaimantName.BackColor = Color.LightYellow;
            tbClaimantSurname.BackColor = Color.LightYellow;
            tbClaimantStreet.BackColor = Color.LightYellow;
            tbClaimantCity.BackColor = Color.LightYellow;
            tbClaimantCountry.BackColor = Color.LightYellow;

            //  Medical Details
            _dataInitializer.InitializeComboBox_YesNo(cmbMedicalHospitalAttendance, true);
            cmbMedicalHospitalAttendance.SelectedIndex = 1;
            cmbMedicalHospitalAttendance.BackColor = Color.LightYellow;

            SetUpYesNo(cmbMedicalMedicalAttentionSeeking);
            cmbMedicalMedicalAttentionSeeking.BackColor = Color.LightYellow;

            SetUpYesNo(cmbMedicalTimeOffRequired);
            cmbMedicalTimeOffRequired.BackColor = Color.LightYellow;

            SetUpYesNo(cmbClaimantChildClaim);
            cmbClaimantChildClaim.BackColor = Color.LightYellow;

            tbMedicalInjurySustainedDescription.Text = "Wiplash";
            tbMedicalInjurySustainedDescription.BackColor = Color.LightYellow;

            //  Accident Data
            tbAccidentDate.Text = "2021-05-15";
            tbAccidentDescription.Text = "Two car shunt";
            tbAccidentLocation.Text = "Bridge Street, Northampton";
            tbAccidentTime.Text = "12:02";
            tbAccidentOccupantsNumber.Text = "1";
            tbDefendantDriverName.Text = "William Thomas";
            tbVehicleOwnerStreet.Text = "10 Silver Street";
            tbVehicleOwnerCity.Text = "Northampton";
            tbVehicleOwnerCountry.Text = "UK";
            tbDefendantDriverStreet.Text = "10 Silver Street";
            tbDefendantDriverCity.Text = "Northampton";
            tbDefendantDriverCountry.Text = "UK";
            SetUpAddressType(cmbDefendantDriverAddressType);
            cmbDefendantDriverAddressType.BackColor = Color.LightYellow;

            SetUpAddressType(cmbVehicleOwnerAddressType);
            cmbVehicleOwnerAddressType.BackColor = Color.LightYellow;

            tbDefendantDriverStreet.BackColor = Color.LightYellow;
            tbDefendantDriverCity.BackColor = Color.LightYellow;
            tbDefendantDriverCountry.BackColor = Color.LightYellow;

            tbAccidentDate.BackColor = Color.LightYellow;
            tbAccidentDescription.BackColor = Color.LightYellow;
            tbAccidentLocation.BackColor = Color.LightYellow;
            tbAccidentTime.BackColor = Color.LightYellow;
            tbAccidentOccupantsNumber.BackColor = Color.LightYellow;
            tbVehicleOwnerStreet.BackColor = Color.LightYellow;
            tbVehicleOwnerCity.BackColor = Color.LightYellow;
            tbVehicleOwnerCountry.BackColor = Color.LightYellow;
            tbDefendantDriverName.BackColor = Color.LightYellow;

            SetUpYesNo(cmbAccidentPoliceReported);
            cmbAccidentPoliceReported.BackColor = Color.LightYellow;

            //  RepairsAndAlternativeVehicleProvision
            SetUpYesNo(cmbClaimingDamageOwnVehicle);
            cmbClaimingDamageOwnVehicle.BackColor = Color.LightYellow;

            cmbDetailsOfTheInsurance.Items.Add(RTAServicesLibraryV3.C04_DetailsOfTheInsurance.Item0);
            cmbDetailsOfTheInsurance.Items.Add(RTAServicesLibraryV3.C04_DetailsOfTheInsurance.Item1);
            cmbDetailsOfTheInsurance.Items.Add(RTAServicesLibraryV3.C04_DetailsOfTheInsurance.Item2);
            cmbDetailsOfTheInsurance.Items.Add(RTAServicesLibraryV3.C04_DetailsOfTheInsurance.Item3);
            cmbDetailsOfTheInsurance.SelectedIndex = 0;
            cmbDetailsOfTheInsurance.BackColor = Color.LightYellow;

            SetUpYesNo(cmbThroughClaimantInsurer);
            cmbThroughClaimantInsurer.BackColor = Color.LightYellow;

            SetUpTotalLoss(cmbTotalLoss);
            cmbTotalLoss.BackColor = Color.LightYellow;

            SetUpYesNo(cmbAVProvided);
            cmbAVProvided.BackColor = Color.LightYellow;

            SetUpYesNo(cmbAVRequiredByCL);
            cmbAVRequiredByCL.BackColor = Color.LightYellow;

            SetUpYesNo(cmbAVRequiredByCR);
            cmbAVRequiredByCR.BackColor = Color.LightYellow;

            SetUpYesNo(cmbClaimantEntitled);
            cmbClaimantEntitled.BackColor = Color.LightYellow;

            //  Provider
            tbProviderName.Text = "A Provider";
            tbProviderReferenceNumber.Text = "123456789";
            tbProviderStartDate.Text = "2009-12-11";
            tbProviderEndDate.Text = "11/12/2009";

            tbProviderName.BackColor = Color.LightYellow;
            tbProviderReferenceNumber.BackColor = Color.LightYellow;
            tbProviderStartDate.BackColor = Color.LightYellow;
            tbProviderEndDate.BackColor = Color.LightYellow;

            //  Insurer
            tbInsurerStreet.Text = "77 The Mews";
            tbInsurerCity.Text = "Northampton";
            tbInsurerCountry.Text = "UK";
            SetUpAddressType(cmbInsurerAddressType);

            tbInsurerStreet.BackColor = Color.LightYellow;
            tbInsurerCity.BackColor = Color.LightYellow;
            tbInsurerCountry.BackColor = Color.LightYellow;
            cmbInsurerAddressType.BackColor = Color.LightYellow;

            SetUpInsurerType(cmbDefendantInsurerType);
            cmbDefendantInsurerType.BackColor = Color.LightYellow;


            //  Bus or Coach
            SetUpYesNo(cmbBussOrCoach);
            SetUpYesNo(cmbBusOrCoachEvidence);
            tbBusOrCoachVehicleDescription.Text = "A Big Bus";

            cmbBussOrCoach.BackColor = Color.LightYellow;
            cmbBusOrCoachEvidence.BackColor = Color.LightYellow;
            tbBusOrCoachVehicleDescription.BackColor = Color.LightYellow;

            //  Other Partys
            tbOtherPartySurname.Text = "OtherSurname";
            tbOtherPartyStreet.Text = "OtherSteet";
            tbOtherPartyCity.Text = "OtherCity";
            tbOtherPartyCountry.Text = "OtherCountry";
            SetUpAddressType(cmbOtherPartyAddressType);

            tbOtherPartySurname.BackColor = Color.LightYellow;
            tbOtherPartyStreet.BackColor = Color.LightYellow;
            tbOtherPartyCity.BackColor = Color.LightYellow;
            tbOtherPartyCountry.BackColor = Color.LightYellow;
            cmbOtherPartyAddressType.BackColor = Color.LightYellow;

            tbOtherPartyInsurerStreet.Text = "Other Party Insurer Street";
            tbOtherPartyInsurerCity.Text = "Other Party Insurer City";
            tbOtherPartyInsurerCountry.Text = "Other Party Insurer Country";
            SetUpAddressType(cmbOtherPartyInsurerAddressType);

            tbOtherPartyInsurerStreet.BackColor = Color.LightYellow;
            tbOtherPartyInsurerCity.BackColor = Color.LightYellow;
            tbOtherPartyInsurerCountry.BackColor = Color.LightYellow;
            cmbOtherPartyInsurerAddressType.BackColor = Color.LightYellow;

            //  Liability and Funding
            tbDefendantResponsability.Text = "Defendant Responsability";
            tbDefendantResponsability.BackColor = Color.LightYellow;

            SetUpYesNo(cmbConsideredFreeLegalExpIns);
            cmbConsideredFreeLegalExpIns.BackColor = Color.LightYellow;
            SetUpYesNo(cmbFundingUndertaken);
            cmbFundingUndertaken.BackColor = Color.LightYellow;

            // R3 and onwards
            cboClaimValue.SelectedIndex = 0; // Up to £10,000

        }


        #region GetClaimantCNF

        #region V2
        /// <summary>
        /// Get Claimant CNF
        /// </summary>
        /// <returns></returns>
        private RTAServicesLibraryV2.DocumentInput GetClaimantCNF()
        {
            //  This part of the CNF is completed by the Claimants Representative
            RTAServicesLibraryV2.DocumentInput doc = new RTAServicesLibraryV2.DocumentInput();

            try
            {
                //  ApplicationData
                RTAServicesLibraryV2.DocumentInputApplicationData appData = new RTAServicesLibraryV2.DocumentInputApplicationData();
                RTAServicesLibraryV2.CT_INPUT_Claim_Details claimDetails = new RTAServicesLibraryV2.CT_INPUT_Claim_Details();
                claimDetails.RetainedCopy = GetYesNo(cmbClaimantRepresentativeRetainedCopy);
                claimDetails.Signatory = GetSignatory(cmbClaimantRepresentativeSignatory);

                appData.ClaimDetails = claimDetails;
                doc.ApplicationData = appData;

                //  ClaimAndClaimantDetails - ClaimantRepresentative
                RTAServicesLibraryV2.DocumentInputClaimAndClaimantDetails claimAndClaimantDetails = new RTAServicesLibraryV2.DocumentInputClaimAndClaimantDetails();
                RTAServicesLibraryV2.DocumentInputClaimAndClaimantDetailsClaimantRepresentative representative = new RTAServicesLibraryV2.DocumentInputClaimAndClaimantDetailsClaimantRepresentative();
                RTAServicesLibraryV2.CT_INPUT_ClaimantRepresentative_CompanyDetails companyDetails = new RTAServicesLibraryV2.CT_INPUT_ClaimantRepresentative_CompanyDetails();
                companyDetails.CompanyName = tbClaimantRepresentativeCompanyName.Text;
                companyDetails.ContactName = tbClaimantRepresentativeContactName.Text;
                companyDetails.ContactSurname = tbClaimantRepresentativeContactSurname.Text;
                companyDetails.ReferenceNumber = tbClaimantRepresentativeReferenceNumber.Text;
                companyDetails.TelephoneNumber = tbClaimantRepresentativeTelephone.Text;
                companyDetails.EmailAddress = "alan.g@fwbs.net";

                RTAServicesLibraryV2.CT_INPUT_Address crAddress = new RTAServicesLibraryV2.CT_INPUT_Address();
                crAddress.Street1 = tbClaimantRepresentativeStreet.Text;
                crAddress.City = tbClaimantRepresentativeCity.Text;
                crAddress.Country = tbClaimantRepresentativeCountry.Text;
                crAddress.PostCode = tbClaimantRepresentativePostCode.Text;
                crAddress.AddressType = GetAddressType(cmbClaimantRepresentativeAddressType);
                crAddress.HouseName = "A House";

                companyDetails.Address = crAddress;
                representative.CompanyDetails = companyDetails;
                claimAndClaimantDetails.ClaimantRepresentative = representative;

                //  DefendantDetails 
                RTAServicesLibraryV2.CT_INPUT_DefendantDetails defendantDetails = new RTAServicesLibraryV2.CT_INPUT_DefendantDetails();
                defendantDetails.DefedantAge = tbDefendantAge.Text;
                defendantDetails.DefendandDescription = tbDefendantDescription.Text;
                defendantDetails.DefendantDetailsObtained = tbDefendantDetailsObtained.Text;
                defendantDetails.DefendantStatus = GetStatus(cmbDefendantStatus);
                defendantDetails.PolicyNumberReference = "12345678";

                RTAServicesLibraryV2.CT_INPUT_Defendant_PersonalDetails defendantPersonal = new RTAServicesLibraryV2.CT_INPUT_Defendant_PersonalDetails();
                defendantPersonal.Sex = GetSex(cmbDefendantSex);
                defendantPersonal.SexSpecified = true;
                defendantPersonal.Surname = tbDefedantSurname.Text;

                RTAServicesLibraryV2.CT_INPUT_Address personalAddress = new RTAServicesLibraryV2.CT_INPUT_Address();
                personalAddress.Street1 = tbDefendantStreet.Text;
                personalAddress.City = tbDefendantCity.Text;
                personalAddress.Country = tbDefendantCountry.Text;
                personalAddress.AddressType = GetAddressType(cmbDefendantAddressType);
                personalAddress.HouseName = "The Times";
                defendantPersonal.Address = personalAddress;
                defendantDetails.PersonalDetails = defendantPersonal;

                RTAServicesLibraryV2.CT_INPUT_Vehicle vehicle = new RTAServicesLibraryV2.CT_INPUT_Vehicle();
                vehicle.VRN = tbDefendantVehicleVRN.Text;
                defendantDetails.Vehicle = vehicle;

                RTAServicesLibraryV2.CT_INPUT_InsurerInformation insurerInformation = new RTAServicesLibraryV2.CT_INPUT_InsurerInformation();
                insurerInformation.InsurerType = GetInsurerType(cmbDefendantInsurerType);

                //  For testing A2A
                insurerInformation.InsurerName = "FWBS COMP";
                insurerInformation.InsurerOrganizationID = "COMP009";
                insurerInformation.InsurerOrganizationPath = "/C00009";

                defendantDetails.InsurerInformation = insurerInformation;

                RTAServicesLibraryV2.CT_INPUT_Defendant_CompanyDetails defendantCompanyDetails = new RTAServicesLibraryV2.CT_INPUT_Defendant_CompanyDetails();
                defendantCompanyDetails.CompanyName = tbDefendantCompanyName.Text;

                RTAServicesLibraryV2.CT_INPUT_Address defendantCompanyAddress = new RTAServicesLibraryV2.CT_INPUT_Address();
                defendantCompanyAddress.Street1 = tbDefendantCompanyStreet.Text;
                defendantCompanyAddress.City = tbDefendantCompanyCity.Text;
                defendantCompanyAddress.Country = tbDefendantCompanyCountry.Text;
                defendantCompanyAddress.AddressType = GetAddressType(cmbDefendantCompanyAddressType);
                defendantCompanyDetails.Address = defendantCompanyAddress;

                defendantDetails.CompanyDetails = defendantCompanyDetails;
                claimAndClaimantDetails.DefendantDetails = defendantDetails;

                //  Claimant Details
                RTAServicesLibraryV2.CT_INPUT_ClaimantDetails claimantDetails = new RTAServicesLibraryV2.CT_INPUT_ClaimantDetails();
                claimantDetails.ChildClaim = GetYesNo(cmbClaimantChildClaim);
                claimantDetails.Occupation = tbClaimantOccupation.Text;

                RTAServicesLibraryV2.CT_Vehicle claimantVehice = new RTAServicesLibraryV2.CT_Vehicle();
                claimantDetails.Vehicle = claimantVehice;

                //  National Insurance Number
                //  Characters 1-2 must be in the range AA-ZZ
                //  Character 1 must not be D, F, I, Q, U, V
                //  Character 2 must not be D, F, I, O, Q, U, V
                //  Characters 1-2 must not be one of the following combinations: FY; GB; NK; TN; TT; ZZ.
                //  Characters 3-6 must be in the range 0000 – 9999
                //  Characters 7-8 must be in the range 00 – 99
                //  Character 9 must be in the range A – D

                //  Only include one or the other (Not Both)
                claimantDetails.NationalInsuranceNumber = tbClaimantNationalInsuranceNumber.Text;
                if (tbClaimantNationalInsuranceNumber.Text.Equals(string.Empty))
                {
                    if (tbClaimantNationalInsuranceNumberComments.Text.Equals(string.Empty))
                    {
                        claimantDetails.NINComment = "Comments";
                    }
                    else
                    {
                        claimantDetails.NINComment = tbClaimantNationalInsuranceNumberComments.Text;
                    }
                }

                RTAServicesLibraryV2.CT_INPUT_Claimant_PersonalDetails claimantPersonalDetails = new RTAServicesLibraryV2.CT_INPUT_Claimant_PersonalDetails();
                claimantPersonalDetails.DateOfBirth = Convert.ToDateTime(tbClaimantDOB.Text);
                claimantPersonalDetails.Name = tbClaimantName.Text;
                claimantPersonalDetails.Surname = tbClaimantSurname.Text;
                claimantPersonalDetails.TitleType = RTAServicesLibraryV2.C02_TitleType.Item1;

                RTAServicesLibraryV2.CT_INPUT_Address claimantPersonalDetailsAddress = new RTAServicesLibraryV2.CT_INPUT_Address();
                claimantPersonalDetailsAddress.HouseName = "Home";
                claimantPersonalDetailsAddress.Street1 = tbClaimantStreet.Text;
                claimantPersonalDetailsAddress.City = tbClaimantCity.Text;
                claimantPersonalDetailsAddress.Country = tbClaimantCountry.Text;
                claimantPersonalDetailsAddress.AddressType = GetAddressType(cmbClaimantAddressType);
                claimantPersonalDetails.Address = claimantPersonalDetailsAddress;
                claimantDetails.PersonalDetails = claimantPersonalDetails;
                claimAndClaimantDetails.ClaimantDetails = claimantDetails;

                doc.ClaimAndClaimantDetails = claimAndClaimantDetails;


                //  MedicalDetails 
                RTAServicesLibraryV2.DocumentInputMedicalDetails medicalDetails = new RTAServicesLibraryV2.DocumentInputMedicalDetails();
                RTAServicesLibraryV2.CT_INPUT_Injury injury = new RTAServicesLibraryV2.CT_INPUT_Injury();
                injury.HospitalAttendance = GetYesNo(cmbMedicalHospitalAttendance);
                injury.InjurySustainedDescription = tbMedicalInjurySustainedDescription.Text;
                injury.MedicalAttentionSeeking = GetYesNo(cmbMedicalMedicalAttentionSeeking);
                injury.TimeOffRequired = GetYesNo(cmbMedicalTimeOffRequired);

                injury.BoneInjury = RTAServicesLibraryV2.C00_YNFlag.Item1;
                injury.BoneInjurySpecified = true;
                injury.DaysNumber = "3";
                injury.MedicalAttentionFirstDate = DateTime.Now;
                injury.MedicalAttentionFirstDateSpecified = true;
                injury.Other = RTAServicesLibraryV2.C00_YNFlag.Item1;
                injury.OtherSpecified = true;
                injury.OvernightDetention = RTAServicesLibraryV2.C00_YNFlag.Item1;
                injury.OvernightDetentionSpecified = true;
                injury.SoftTissue = RTAServicesLibraryV2.C00_YNFlag.Item1;
                injury.SoftTissueSpecified = true;
                injury.StillOffWork = RTAServicesLibraryV2.C00_YNFlag.Item0;
                injury.StillOffWorkSpecified = true;
                injury.TimeOffPeriod = "3";
                injury.Whiplash = RTAServicesLibraryV2.C00_YNFlag.Item1;
                injury.WhiplashSpecified = true;
                medicalDetails.Injury = injury;

                RTAServicesLibraryV2.CT_INPUT_Hospital[] hospitals = new RTAServicesLibraryV2.CT_INPUT_Hospital[1];
                RTAServicesLibraryV2.CT_INPUT_Hospital hospital = new RTAServicesLibraryV2.CT_INPUT_Hospital();
                hospital.HospitalName = "Hospital name";
                hospital.PostCode = "DG3 5VN";
                hospital.HospitalType = RTAServicesLibraryV2.C07_HospitalType.Item0;

                RTAServicesLibraryV2.CT_HospitalAddress hospitalAddress = new RTAServicesLibraryV2.CT_HospitalAddress();
                hospitalAddress.AddressLine1 = "Hospital address1";
                hospitalAddress.AddressLine2 = "Hospital address2";
                hospitalAddress.AddressLine3 = "Hospital address3";
                hospitalAddress.AddressLine4 = "Hospital address4";
                hospital.HospitalAddress = hospitalAddress;

                hospitals[0] = hospital;
                medicalDetails.Hospital = hospitals;

                RTAServicesLibraryV2.CT_INPUT_Rehabilitation rehabilitation = new RTAServicesLibraryV2.CT_INPUT_Rehabilitation();
                rehabilitation.RehabilitationUndertaken = RTAServicesLibraryV2.C03_RehabilitationRecommended.Item0;
                rehabilitation.RehabilitationDetails = "Full details on rahabilitaion needs claimant has arising out of the accident";
                rehabilitation.RehabilitationNeeds = RTAServicesLibraryV2.C00_YNFlag.Item1;
                rehabilitation.RehabilitationNeedsSpecified = true;
                rehabilitation.RehabilitationTreatment = "tails of the rehabilitation treatment recommended and any treatment provided including name of provider";

                medicalDetails.Rehabilitation = rehabilitation;
                doc.MedicalDetails = medicalDetails;

                //  RepairsAndAlternativeVehicleProvision
                RTAServicesLibraryV2.DocumentInputRepairsAndAlternativeVehicleProvision repairsAndAlternativeVehicleProvision = new RTAServicesLibraryV2.DocumentInputRepairsAndAlternativeVehicleProvision();
                RTAServicesLibraryV2.CT_INPUT_Repairs repairs = new RTAServicesLibraryV2.CT_INPUT_Repairs();
                repairs.ClaiimingDamageOwnVehicle = GetYesNo(cmbClaimingDamageOwnVehicle);
                repairs.DetailsOfTheInsurance = RTAServicesLibraryV2.C04_DetailsOfTheInsurance.Item0;
                repairs.ThroughClaimantInsurer = GetYesNo(cmbThroughClaimantInsurer);
                repairs.TotalLoss = GetTotalLoss(cmbTotalLoss);

                repairs.ContactDetails = "Contact for  the defendants insurer";
                repairs.DefendantInsInspection = RTAServicesLibraryV2.C00_YNFlag.Item1;
                repairs.Location = "Location for  the defendants insurer";
                repairs.OtherDetails = "other details of the insurance";
                repairs.RepairsPosition = RTAServicesLibraryV2.C05_RepairsPosition.Item3;
                repairs.ThroughAlternatieCompany = RTAServicesLibraryV2.C00_YNFlag.Item1;

                RTAServicesLibraryV2.CT_INPUT_AlternativeCompany alternativeCompany = new RTAServicesLibraryV2.CT_INPUT_AlternativeCompany();
                alternativeCompany.Address = "Alternative company address";
                alternativeCompany.CompanyName = "Alternative company";
                alternativeCompany.ReferenceNumber = "123";
                alternativeCompany.TelephoneNumber = "12345678";
                repairs.AlternativeCompany = alternativeCompany;
                repairsAndAlternativeVehicleProvision.Repairs = repairs;

                //  AlternativeVehicleProvision 
                RTAServicesLibraryV2.CT_INPUT_AlternativeVehicleProvision alternativeVehicleProvision = new RTAServicesLibraryV2.CT_INPUT_AlternativeVehicleProvision();
                alternativeVehicleProvision.AVProvided = GetYesNo(cmbAVProvided);
                alternativeVehicleProvision.AVRequiredByCL = GetYesNo(cmbAVRequiredByCL);
                alternativeVehicleProvision.AVRequiredByCR = GetYesNo(cmbAVRequiredByCR);
                alternativeVehicleProvision.ClaimantEntitled = GetYesNo(cmbClaimantEntitled);

                RTAServicesLibraryV2.CT_INPUT_Provider provider = new RTAServicesLibraryV2.CT_INPUT_Provider();
                provider.ProviderName = tbProviderName.Text;
                provider.ReferenceNumber = tbProviderReferenceNumber.Text;
                provider.StartDate = Convert.ToDateTime(tbProviderStartDate.Text);
                provider.EndDate = tbProviderEndDate.Text;
                provider.ProviderAddress = "Provider Address";

                RTAServicesLibraryV2.CT_Vehicle providerVehicle = new RTAServicesLibraryV2.CT_Vehicle();
                providerVehicle.Color = "BlackProvided";
                providerVehicle.EngineSize = "12";
                providerVehicle.Make = "MakeProvided";
                providerVehicle.Model = "ModelBestProvided";
                providerVehicle.VRN = "12341234";
                provider.Vehicle = providerVehicle;

                alternativeVehicleProvision.Provider = provider;
                repairsAndAlternativeVehicleProvision.AlternativeVehicleProvision = alternativeVehicleProvision;
                doc.RepairsAndAlternativeVehicleProvision = repairsAndAlternativeVehicleProvision;

                //  AccidentData 
                RTAServicesLibraryV2.DocumentInputAccidentData accidentData = new RTAServicesLibraryV2.DocumentInputAccidentData();
                RTAServicesLibraryV2.CT_INPUT_AccidentDetails accidentDetails = new RTAServicesLibraryV2.CT_INPUT_AccidentDetails();
                accidentDetails.AccidentDate = Convert.ToDateTime(tbAccidentDate.Text);
                accidentDetails.AccidentDescription = tbAccidentDescription.Text;
                accidentDetails.AccidentLocation = tbAccidentLocation.Text;
                accidentDetails.AccidentTime = tbAccidentTime.Text;
                accidentDetails.ClaimantType = RTAServicesLibraryV2.C06_ClaimantType.Item0;
                accidentDetails.OccupantsNumber = tbAccidentOccupantsNumber.Text;
                accidentDetails.Seatbelt = RTAServicesLibraryV2.C11_SeatbeltWearing.Item1;
                accidentDetails.SeatbeltSpecified = true;
                accidentDetails.DriverIsDefendant = RTAServicesLibraryV2.C00_YNFlag.Item1;
                accidentDetails.DriverIsDefendantSpecified = true;

                RTAServicesLibraryV2.CT_INPUT_Driver_PersonalDetails driverPersonalDetails = new RTAServicesLibraryV2.CT_INPUT_Driver_PersonalDetails();
                driverPersonalDetails.Surname = "John";
                driverPersonalDetails.MiddleName = "A";
                driverPersonalDetails.Name = "William";

                RTAServicesLibraryV2.CT_INPUT_Address driverPersonalDetailsAddress = new RTAServicesLibraryV2.CT_INPUT_Address();
                driverPersonalDetailsAddress.Street1 = tbDefendantDriverStreet.Text;
                driverPersonalDetailsAddress.City = tbDefendantDriverCity.Text;
                driverPersonalDetailsAddress.Country = tbDefendantDriverCountry.Text;
                driverPersonalDetailsAddress.AddressType = GetAddressType(cmbDefendantDriverAddressType);
                driverPersonalDetailsAddress.HouseName = "The Trees";
                driverPersonalDetailsAddress.District = "Abington";
                driverPersonalDetailsAddress.HouseNumber = "10";
                driverPersonalDetails.Address = driverPersonalDetailsAddress;
                accidentDetails.Driver = driverPersonalDetails;

                RTAServicesLibraryV2.CT_INPUT_VehicleOwner_PersonalDetails vehicleOwnerPersonalDetails = new RTAServicesLibraryV2.CT_INPUT_VehicleOwner_PersonalDetails();

                RTAServicesLibraryV2.CT_INPUT_Address vehicleOwnerPersonalDetailsAddress = new RTAServicesLibraryV2.CT_INPUT_Address();

                accidentDetails.Owner = vehicleOwnerPersonalDetails;

                //  Insurer Details
                RTAServicesLibraryV2.CT_INPUT_Insurance_CompanyDetails insuranceCompanyDetails = new RTAServicesLibraryV2.CT_INPUT_Insurance_CompanyDetails();
                RTAServicesLibraryV2.CT_INPUT_Address insuranceCompanyDetailsAddress = new RTAServicesLibraryV2.CT_INPUT_Address();
                accidentDetails.InsuranceCompanyInformatiion = insuranceCompanyDetails;

                RTAServicesLibraryV2.CT_WeatherConditions weather = new RTAServicesLibraryV2.CT_WeatherConditions();
                weather.Fog = RTAServicesLibraryV2.C00_YNFlag.Item0;
                weather.Ice = RTAServicesLibraryV2.C00_YNFlag.Item0;
                weather.Other = RTAServicesLibraryV2.C00_YNFlag.Item0;
                weather.Rain = RTAServicesLibraryV2.C00_YNFlag.Item0;
                weather.Snow = RTAServicesLibraryV2.C00_YNFlag.Item1;
                weather.Sun = RTAServicesLibraryV2.C00_YNFlag.Item0;
                accidentDetails.WeatherConditions = weather;

                RTAServicesLibraryV2.CT_RoadConditions roadConditions = new RTAServicesLibraryV2.CT_RoadConditions();
                roadConditions.Dry = RTAServicesLibraryV2.C00_YNFlag.Item0;
                roadConditions.Ice = RTAServicesLibraryV2.C00_YNFlag.Item0;
                roadConditions.Mud = RTAServicesLibraryV2.C00_YNFlag.Item1;
                roadConditions.Oil = RTAServicesLibraryV2.C00_YNFlag.Item0;
                roadConditions.Other = RTAServicesLibraryV2.C00_YNFlag.Item0;
                roadConditions.Snow = RTAServicesLibraryV2.C00_YNFlag.Item0;
                roadConditions.Wet = RTAServicesLibraryV2.C00_YNFlag.Item0;
                accidentDetails.RoadConditions = roadConditions;

                RTAServicesLibraryV2.CT_AccidentCircumstances accidentCircumstances = new RTAServicesLibraryV2.CT_AccidentCircumstances();
                accidentCircumstances.AccidCarPark = RTAServicesLibraryV2.C00_YNFlag.Item0;
                accidentCircumstances.AccidChangingLanes = RTAServicesLibraryV2.C00_YNFlag.Item0;
                accidentCircumstances.AccidRoundabout = RTAServicesLibraryV2.C00_YNFlag.Item0;
                accidentCircumstances.ConcertinaCollision = RTAServicesLibraryV2.C00_YNFlag.Item0;
                accidentCircumstances.Other = RTAServicesLibraryV2.C00_YNFlag.Item0;
                accidentCircumstances.VhclHitInRear = RTAServicesLibraryV2.C00_YNFlag.Item0;
                accidentCircumstances.VhclHitSideRoad = RTAServicesLibraryV2.C00_YNFlag.Item0;
                accidentCircumstances.VhclHitWhilstParked = RTAServicesLibraryV2.C00_YNFlag.Item1;
                accidentDetails.AccidentCircumstances = accidentCircumstances;

                RTAServicesLibraryV2.CT_PoliceDetails police = new RTAServicesLibraryV2.CT_PoliceDetails();
                police.ReferenceNumber = "123";
                police.ReportingOfficerName = "Reporting Officer Name";
                police.StationAddress = "Police Station Address";
                police.StationName = "Police Station Name";
                accidentDetails.PoliceDetails = police;

                accidentData.AccidentDetails = accidentDetails;

                RTAServicesLibraryV2.CT_INPUT_BusCoach busCoach = new RTAServicesLibraryV2.CT_INPUT_BusCoach();
                busCoach.BussOrCoach = GetYesNo(cmbBussOrCoach);
                busCoach.Evidence = GetYesNo(cmbBusOrCoachEvidence);
                busCoach.VehicleDescription = tbBusOrCoachVehicleDescription.Text;
                busCoach.Comments = "Some Comments";
                busCoach.DriverDescription = "Description of the driver =drunk";
                busCoach.DriverID = "123";
                busCoach.DriverName = "Driver Name";
                busCoach.NumberOfPassengers = "2";
                accidentData.BusCoach = busCoach;

                doc.AccidentData = accidentData;

                RTAServicesLibraryV2.DocumentInputLiabilityFunding liabilityFunding = new RTAServicesLibraryV2.DocumentInputLiabilityFunding();
                RTAServicesLibraryV2.CT_INPUT_Liability liability = new RTAServicesLibraryV2.CT_INPUT_Liability();
                liability.DefendantResponsability = tbDefendantResponsability.Text;
                liabilityFunding.Liability = liability;

                RTAServicesLibraryV2.CT_INPUT_Funding funding = new RTAServicesLibraryV2.CT_INPUT_Funding();
                funding.ConsideredFreeLegalExpIns = GetYesNo(cmbConsideredFreeLegalExpIns);
                funding.FundingUndertaken = GetYesNo(cmbFundingUndertaken);
                funding.AgreementDate = DateTime.Now;
                funding.Comments = "Section M - Other relevant information";
                funding.ConditionalFeeDate = DateTime.Now;
                funding.ICAddress = "Address of Insurance Company";
                funding.ICName = "Insurance Company Name";
                funding.IncreasingPoint = "At which point is an increased premium payable?";
                funding.LevelOfCover = "Level of cover";
                funding.MembershipOrganisation = RTAServicesLibraryV2.C00_YNFlag.Item1;
                funding.Ohter = RTAServicesLibraryV2.C00_YNFlag.Item1;
                funding.OrganizationName = "Organisation Name";
                funding.OtherDetails = "Other details";
                funding.PolicyDate = DateTime.Now;
                funding.PolicyNumber = "123";
                funding.PremiumsStaged = RTAServicesLibraryV2.C00_YNFlag.Item0;
                funding.Section29 = RTAServicesLibraryV2.C00_YNFlag.Item1;
                funding.Section58 = RTAServicesLibraryV2.C00_YNFlag.Item1;
                liabilityFunding.Funding = funding;

                doc.LiabilityFunding = liabilityFunding;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return doc;
        }
        #endregion V2

        #region V3
        /// <summary>
        /// Get Claimant CNF
        /// </summary>
        /// <returns></returns>
        private RTAServicesLibraryV3.DocumentInput GetClaimantCNFV3()
        {
            //  This part of the CNF is completed by the Claimants Representative
            RTAServicesLibraryV3.DocumentInput doc = new RTAServicesLibraryV3.DocumentInput();

            try
            {
                //  ApplicationData
                RTAServicesLibraryV3.DocumentInputApplicationData appData = new RTAServicesLibraryV3.DocumentInputApplicationData();
                RTAServicesLibraryV3.CT_INPUT_Claim_Details claimDetails = new RTAServicesLibraryV3.CT_INPUT_Claim_Details();
                claimDetails.RetainedCopy = GetYesNoV3(cmbClaimantRepresentativeRetainedCopy);
                claimDetails.Signatory = GetSignatoryV3(cmbClaimantRepresentativeSignatory);
                
                appData.ClaimDetails = claimDetails;
                doc.ApplicationData = appData;

                //  ClaimAndClaimantDetails - ClaimantRepresentative
                RTAServicesLibraryV3.DocumentInputClaimAndClaimantDetails claimAndClaimantDetails = new RTAServicesLibraryV3.DocumentInputClaimAndClaimantDetails();
                RTAServicesLibraryV3.DocumentInputClaimAndClaimantDetailsClaimantRepresentative representative = new RTAServicesLibraryV3.DocumentInputClaimAndClaimantDetailsClaimantRepresentative();
                RTAServicesLibraryV3.CT_INPUT_ClaimantRepresentative_CompanyDetails companyDetails = new RTAServicesLibraryV3.CT_INPUT_ClaimantRepresentative_CompanyDetails();
                companyDetails.CompanyName = tbClaimantRepresentativeCompanyName.Text;
                companyDetails.ContactName = tbClaimantRepresentativeContactName.Text;
                companyDetails.ContactSurname = tbClaimantRepresentativeContactSurname.Text;
                companyDetails.ReferenceNumber = tbClaimantRepresentativeReferenceNumber.Text;
                companyDetails.TelephoneNumber = tbClaimantRepresentativeTelephone.Text;
                companyDetails.EmailAddress = "alan.g@fwbs.net";

                RTAServicesLibraryV3.CT_INPUT_Address crAddress = new RTAServicesLibraryV3.CT_INPUT_Address();
                crAddress.Street1 = tbClaimantRepresentativeStreet.Text;
                crAddress.City = tbClaimantRepresentativeCity.Text;
                crAddress.Country = tbClaimantRepresentativeCountry.Text;
                crAddress.PostCode = tbClaimantRepresentativePostCode.Text;
                crAddress.AddressType = GetAddressTypeV3(cmbClaimantRepresentativeAddressType);
                crAddress.HouseName = "A House";

                companyDetails.Address = crAddress;
                representative.CompanyDetails = companyDetails;
                claimAndClaimantDetails.ClaimantRepresentative = representative;

                //  DefendantDetails 
                RTAServicesLibraryV3.CT_INPUT_DefendantDetails defendantDetails = new RTAServicesLibraryV3.CT_INPUT_DefendantDetails();
                defendantDetails.DefedantAge = tbDefendantAge.Text;
                defendantDetails.DefendandDescription = tbDefendantDescription.Text;
                defendantDetails.DefendantDetailsObtained = tbDefendantDetailsObtained.Text;
                defendantDetails.DefendantStatus = GetStatusV3(cmbDefendantStatus);
                defendantDetails.PolicyNumberReference = "12345678";

                RTAServicesLibraryV3.CT_INPUT_Defendant_PersonalDetails defendantPersonal = new RTAServicesLibraryV3.CT_INPUT_Defendant_PersonalDetails();
                defendantPersonal.Sex = GetSexV3(cmbDefendantSex);
                defendantPersonal.SexSpecified = true;
                defendantPersonal.Surname = tbDefedantSurname.Text;

                RTAServicesLibraryV3.CT_INPUT_Address personalAddress = new RTAServicesLibraryV3.CT_INPUT_Address();
                personalAddress.Street1 = tbDefendantStreet.Text;
                personalAddress.City = tbDefendantCity.Text;
                personalAddress.Country = tbDefendantCountry.Text;
                personalAddress.AddressType = GetAddressTypeV3(cmbDefendantAddressType);
                personalAddress.HouseName = "The Times";
                defendantPersonal.Address = personalAddress;
                defendantDetails.PersonalDetails = defendantPersonal;

                RTAServicesLibraryV3.CT_INPUT_Vehicle vehicle = new RTAServicesLibraryV3.CT_INPUT_Vehicle();
                vehicle.VRN = tbDefendantVehicleVRN.Text;
                defendantDetails.Vehicle = vehicle;

                RTAServicesLibraryV3.CT_INPUT_InsurerInformation insurerInformation = new RTAServicesLibraryV3.CT_INPUT_InsurerInformation();
                insurerInformation.InsurerType = GetInsurerTypeV3(cmbDefendantInsurerType);

                //  For testing A2A
                insurerInformation.InsurerName = "FWBS COMP";
                insurerInformation.InsurerOrganizationID = "COMP009";
                insurerInformation.InsurerOrganizationPath = "/C00009";

                defendantDetails.InsurerInformation = insurerInformation;

                RTAServicesLibraryV3.CT_INPUT_Defendant_CompanyDetails defendantCompanyDetails = new RTAServicesLibraryV3.CT_INPUT_Defendant_CompanyDetails();
                defendantCompanyDetails.CompanyName = tbDefendantCompanyName.Text;

                RTAServicesLibraryV3.CT_INPUT_Address defendantCompanyAddress = new RTAServicesLibraryV3.CT_INPUT_Address();
                defendantCompanyAddress.Street1 = tbDefendantCompanyStreet.Text;
                defendantCompanyAddress.City = tbDefendantCompanyCity.Text;
                defendantCompanyAddress.Country = tbDefendantCompanyCountry.Text;
                defendantCompanyAddress.AddressType = GetAddressTypeV3(cmbDefendantCompanyAddressType);
                defendantCompanyDetails.Address = defendantCompanyAddress;

                defendantDetails.CompanyDetails = defendantCompanyDetails;
                claimAndClaimantDetails.DefendantDetails = defendantDetails;

                //  Claimant Details
                RTAServicesLibraryV3.CT_INPUT_ClaimantDetails claimantDetails = new RTAServicesLibraryV3.CT_INPUT_ClaimantDetails();
                claimantDetails.ChildClaim = GetYesNoV3(cmbClaimantChildClaim);
                claimantDetails.Occupation = tbClaimantOccupation.Text;

                RTAServicesLibraryV3.CT_Vehicle claimantVehice = new RTAServicesLibraryV3.CT_Vehicle();
                claimantDetails.Vehicle = claimantVehice;

                //  National Insurance Number
                //  Characters 1-2 must be in the range AA-ZZ
                //  Character 1 must not be D, F, I, Q, U, V
                //  Character 2 must not be D, F, I, O, Q, U, V
                //  Characters 1-2 must not be one of the following combinations: FY; GB; NK; TN; TT; ZZ.
                //  Characters 3-6 must be in the range 0000 – 9999
                //  Characters 7-8 must be in the range 00 – 99
                //  Character 9 must be in the range A – D

                //  Only include one or the other (Not Both)
                claimantDetails.NationalInsuranceNumber = tbClaimantNationalInsuranceNumber.Text;
                if (tbClaimantNationalInsuranceNumber.Text.Equals(string.Empty))
                {
                    if (tbClaimantNationalInsuranceNumberComments.Text.Equals(string.Empty))
                    {
                        claimantDetails.NINComment = "Comments";
                    }
                    else
                    {
                        claimantDetails.NINComment = tbClaimantNationalInsuranceNumberComments.Text;
                    }
                }

                RTAServicesLibraryV3.CT_INPUT_Claimant_PersonalDetails claimantPersonalDetails = new RTAServicesLibraryV3.CT_INPUT_Claimant_PersonalDetails();
                claimantPersonalDetails.DateOfBirth = Convert.ToDateTime(tbClaimantDOB.Text);
                claimantPersonalDetails.Name = tbClaimantName.Text;
                claimantPersonalDetails.Surname = tbClaimantSurname.Text;
                claimantPersonalDetails.TitleType = RTAServicesLibraryV3.C02_TitleType.Item1;

                RTAServicesLibraryV3.CT_INPUT_Address claimantPersonalDetailsAddress = new RTAServicesLibraryV3.CT_INPUT_Address();
                claimantPersonalDetailsAddress.HouseName = "Home";
                claimantPersonalDetailsAddress.Street1 = tbClaimantStreet.Text;
                claimantPersonalDetailsAddress.City = tbClaimantCity.Text;
                claimantPersonalDetailsAddress.Country = tbClaimantCountry.Text;
                claimantPersonalDetailsAddress.AddressType = GetAddressTypeV3(cmbClaimantAddressType);
                claimantPersonalDetails.Address = claimantPersonalDetailsAddress;
                claimantDetails.PersonalDetails = claimantPersonalDetails;
                claimAndClaimantDetails.ClaimantDetails = claimantDetails;

                doc.ClaimAndClaimantDetails = claimAndClaimantDetails;


                //  MedicalDetails 
                RTAServicesLibraryV3.DocumentInputMedicalDetails medicalDetails = new RTAServicesLibraryV3.DocumentInputMedicalDetails();
                RTAServicesLibraryV3.CT_INPUT_Injury injury = new RTAServicesLibraryV3.CT_INPUT_Injury();
                injury.HospitalAttendance = GetYesNoV3(cmbMedicalHospitalAttendance);
                injury.InjurySustainedDescription = tbMedicalInjurySustainedDescription.Text;
                injury.MedicalAttentionSeeking = GetYesNoV3(cmbMedicalMedicalAttentionSeeking);
                injury.TimeOffRequired = GetYesNoV3(cmbMedicalTimeOffRequired);

                injury.BoneInjury = RTAServicesLibraryV3.C00_YNFlag.Item1;
                injury.BoneInjurySpecified = true;
                injury.DaysNumber = "3";
                injury.MedicalAttentionFirstDate = DateTime.Now;
                injury.MedicalAttentionFirstDateSpecified = true;
                injury.Other = RTAServicesLibraryV3.C00_YNFlag.Item1;
                injury.OtherSpecified = true;
                injury.OvernightDetention = RTAServicesLibraryV3.C00_YNFlag.Item1;
                injury.OvernightDetentionSpecified = true;
                injury.SoftTissue = RTAServicesLibraryV3.C00_YNFlag.Item1;
                injury.SoftTissueSpecified = true;
                injury.StillOffWork = RTAServicesLibraryV3.C00_YNFlag.Item0;
                injury.StillOffWorkSpecified = true;
                injury.TimeOffPeriod = "3";
                injury.Whiplash = RTAServicesLibraryV3.C00_YNFlag.Item1;
                injury.WhiplashSpecified = true;
                medicalDetails.Injury = injury;

                RTAServicesLibraryV3.CT_INPUT_Hospital_R3[] hospitals = new RTAServicesLibraryV3.CT_INPUT_Hospital_R3[1];
                RTAServicesLibraryV3.CT_INPUT_Hospital_R3 hospital = new RTAServicesLibraryV3.CT_INPUT_Hospital_R3();
                hospital.HospitalName = "Hospital name";
                hospital.PostCode = "DG3 5VN";
                hospital.HospitalType = RTAServicesLibraryV3.C07_HospitalType_R3.Item0;

                RTAServicesLibraryV3.CT_HospitalAddress hospitalAddress = new RTAServicesLibraryV3.CT_HospitalAddress();
                hospitalAddress.AddressLine1 = "Hospital address1";
                hospitalAddress.AddressLine2 = "Hospital address2";
                hospitalAddress.AddressLine3 = "Hospital address3";
                hospitalAddress.AddressLine4 = "Hospital address4";
                hospital.HospitalAddress = hospitalAddress;

                hospitals[0] = hospital;
                medicalDetails.Hospital = hospitals;

                RTAServicesLibraryV3.CT_INPUT_Rehabilitation rehabilitation = new RTAServicesLibraryV3.CT_INPUT_Rehabilitation();
                rehabilitation.RehabilitationUndertaken = RTAServicesLibraryV3.C03_RehabilitationRecommended.Item0;
                rehabilitation.RehabilitationDetails = "Full details on rahabilitaion needs claimant has arising out of the accident";
                rehabilitation.RehabilitationNeeds = RTAServicesLibraryV3.C00_YNFlag.Item1;
                rehabilitation.RehabilitationNeedsSpecified = true;
                rehabilitation.RehabilitationTreatment = "tails of the rehabilitation treatment recommended and any treatment provided including name of provider";

                medicalDetails.Rehabilitation = rehabilitation;
                doc.MedicalDetails = medicalDetails;

                //  RepairsAndAlternativeVehicleProvision
                RTAServicesLibraryV3.DocumentInputRepairsAndAlternativeVehicleProvision repairsAndAlternativeVehicleProvision = new RTAServicesLibraryV3.DocumentInputRepairsAndAlternativeVehicleProvision();
                RTAServicesLibraryV3.CT_INPUT_Repairs repairs = new RTAServicesLibraryV3.CT_INPUT_Repairs();
                repairs.ClaiimingDamageOwnVehicle = GetYesNoV3(cmbClaimingDamageOwnVehicle);
                repairs.DetailsOfTheInsurance = RTAServicesLibraryV3.C04_DetailsOfTheInsurance.Item0;
                repairs.ThroughClaimantInsurer = GetYesNoV3(cmbThroughClaimantInsurer);
                repairs.TotalLoss = GetTotalLossV3(cmbTotalLoss);

                repairs.ContactDetails = "Contact for  the defendants insurer";
                repairs.DefendantInsInspection = RTAServicesLibraryV3.C00_YNFlag.Item1;
                repairs.Location = "Location for  the defendants insurer";
                repairs.OtherDetails = "other details of the insurance";
                repairs.RepairsPosition = RTAServicesLibraryV3.C05_RepairsPosition.Item3;
                repairs.ThroughAlternatieCompany = RTAServicesLibraryV3.C00_YNFlag.Item1;

                RTAServicesLibraryV3.CT_INPUT_AlternativeCompany alternativeCompany = new RTAServicesLibraryV3.CT_INPUT_AlternativeCompany();
                alternativeCompany.Address = "Alternative company address";
                alternativeCompany.CompanyName = "Alternative company";
                alternativeCompany.ReferenceNumber = "123";
                alternativeCompany.TelephoneNumber = "12345678";
                repairs.AlternativeCompany = alternativeCompany;
                repairsAndAlternativeVehicleProvision.Repairs = repairs;

                //  AlternativeVehicleProvision 
                RTAServicesLibraryV3.CT_INPUT_AlternativeVehicleProvision alternativeVehicleProvision = new RTAServicesLibraryV3.CT_INPUT_AlternativeVehicleProvision();
                alternativeVehicleProvision.AVProvided = GetYesNoV3(cmbAVProvided);
                alternativeVehicleProvision.AVRequiredByCL = GetYesNoV3(cmbAVRequiredByCL);
                alternativeVehicleProvision.AVRequiredByCR = GetYesNoV3(cmbAVRequiredByCR);
                alternativeVehicleProvision.ClaimantEntitled = GetYesNoV3(cmbClaimantEntitled);

                RTAServicesLibraryV3.CT_INPUT_Provider provider = new RTAServicesLibraryV3.CT_INPUT_Provider();
                provider.ProviderName = tbProviderName.Text;
                provider.ReferenceNumber = tbProviderReferenceNumber.Text;
                provider.StartDate = Convert.ToDateTime(tbProviderStartDate.Text);
                provider.EndDate = tbProviderEndDate.Text;
                provider.ProviderAddress = "Provider Address";

                RTAServicesLibraryV3.CT_Vehicle providerVehicle = new RTAServicesLibraryV3.CT_Vehicle();
                providerVehicle.Color = "BlackProvided";
                providerVehicle.EngineSize = "12";
                providerVehicle.Make = "MakeProvided";
                providerVehicle.Model = "ModelBestProvided";
                providerVehicle.VRN = "12341234";
                provider.Vehicle = providerVehicle;

                alternativeVehicleProvision.Provider = provider;
                repairsAndAlternativeVehicleProvision.AlternativeVehicleProvision = alternativeVehicleProvision;
                doc.RepairsAndAlternativeVehicleProvision = repairsAndAlternativeVehicleProvision;

                //  AccidentData 
                RTAServicesLibraryV3.DocumentInputAccidentData accidentData = new RTAServicesLibraryV3.DocumentInputAccidentData();
                RTAServicesLibraryV3.CT_INPUT_AccidentDetails accidentDetails = new RTAServicesLibraryV3.CT_INPUT_AccidentDetails();
                accidentDetails.AccidentDate = Convert.ToDateTime(tbAccidentDate.Text);
                accidentDetails.AccidentDescription = tbAccidentDescription.Text;
                accidentDetails.AccidentLocation = tbAccidentLocation.Text;
                accidentDetails.AccidentTime = tbAccidentTime.Text;
                accidentDetails.ClaimantType = RTAServicesLibraryV3.C06_ClaimantType.Item0;

                accidentDetails.OccupantsNumber = tbAccidentOccupantsNumber.Text;
                accidentDetails.Seatbelt = RTAServicesLibraryV3.C11_SeatbeltWearing.Item1;
                accidentDetails.SeatbeltSpecified = true;
                accidentDetails.DriverIsDefendant = RTAServicesLibraryV3.C00_YNFlag.Item1;
                accidentDetails.DriverIsDefendantSpecified = true;

                RTAServicesLibraryV3.CT_INPUT_Driver_PersonalDetails driverPersonalDetails = new RTAServicesLibraryV3.CT_INPUT_Driver_PersonalDetails();
                driverPersonalDetails.Surname = "John";
                driverPersonalDetails.MiddleName = "A";
                driverPersonalDetails.Name = "William";

                RTAServicesLibraryV3.CT_INPUT_Address driverPersonalDetailsAddress = new RTAServicesLibraryV3.CT_INPUT_Address();
                driverPersonalDetailsAddress.Street1 = tbDefendantDriverStreet.Text;
                driverPersonalDetailsAddress.City = tbDefendantDriverCity.Text;
                driverPersonalDetailsAddress.Country = tbDefendantDriverCountry.Text;
                driverPersonalDetailsAddress.AddressType = GetAddressTypeV3(cmbDefendantDriverAddressType);
                driverPersonalDetailsAddress.HouseName = "The Trees";
                driverPersonalDetailsAddress.District = "Abington";
                driverPersonalDetailsAddress.HouseNumber = "10";
                driverPersonalDetails.Address = driverPersonalDetailsAddress;
                accidentDetails.Driver = driverPersonalDetails;

                RTAServicesLibraryV3.CT_INPUT_VehicleOwner_PersonalDetails vehicleOwnerPersonalDetails = new RTAServicesLibraryV3.CT_INPUT_VehicleOwner_PersonalDetails();

                RTAServicesLibraryV3.CT_INPUT_Address vehicleOwnerPersonalDetailsAddress = new RTAServicesLibraryV3.CT_INPUT_Address();


                accidentDetails.Owner = vehicleOwnerPersonalDetails;

                //  Insurer Details
                RTAServicesLibraryV3.CT_INPUT_Insurance_CompanyDetails insuranceCompanyDetails = new RTAServicesLibraryV3.CT_INPUT_Insurance_CompanyDetails();
                RTAServicesLibraryV3.CT_INPUT_Address insuranceCompanyDetailsAddress = new RTAServicesLibraryV3.CT_INPUT_Address();

                accidentDetails.InsuranceCompanyInformatiion = insuranceCompanyDetails;

                RTAServicesLibraryV3.CT_WeatherConditions weather = new RTAServicesLibraryV3.CT_WeatherConditions();
                weather.Fog = RTAServicesLibraryV3.C00_YNFlag.Item0;
                weather.Ice = RTAServicesLibraryV3.C00_YNFlag.Item0;
                weather.Other = RTAServicesLibraryV3.C00_YNFlag.Item0;
                weather.Rain = RTAServicesLibraryV3.C00_YNFlag.Item0;
                weather.Snow = RTAServicesLibraryV3.C00_YNFlag.Item1;
                weather.Sun = RTAServicesLibraryV3.C00_YNFlag.Item0;
                accidentDetails.WeatherConditions = weather;

                RTAServicesLibraryV3.CT_RoadConditions roadConditions = new RTAServicesLibraryV3.CT_RoadConditions();
                roadConditions.Dry = RTAServicesLibraryV3.C00_YNFlag.Item0;
                roadConditions.Ice = RTAServicesLibraryV3.C00_YNFlag.Item0;
                roadConditions.Mud = RTAServicesLibraryV3.C00_YNFlag.Item1;
                roadConditions.Oil = RTAServicesLibraryV3.C00_YNFlag.Item0;
                roadConditions.Other = RTAServicesLibraryV3.C00_YNFlag.Item0;
                roadConditions.Snow = RTAServicesLibraryV3.C00_YNFlag.Item0;
                roadConditions.Wet = RTAServicesLibraryV3.C00_YNFlag.Item0;
                accidentDetails.RoadConditions = roadConditions;

                RTAServicesLibraryV3.CT_AccidentCircumstances accidentCircumstances = new RTAServicesLibraryV3.CT_AccidentCircumstances();
                accidentCircumstances.AccidCarPark = RTAServicesLibraryV3.C00_YNFlag.Item0;
                accidentCircumstances.AccidChangingLanes = RTAServicesLibraryV3.C00_YNFlag.Item0;
                accidentCircumstances.AccidRoundabout = RTAServicesLibraryV3.C00_YNFlag.Item0;
                accidentCircumstances.ConcertinaCollision = RTAServicesLibraryV3.C00_YNFlag.Item0;
                accidentCircumstances.Other = RTAServicesLibraryV3.C00_YNFlag.Item0;
                accidentCircumstances.VhclHitInRear = RTAServicesLibraryV3.C00_YNFlag.Item0;
                accidentCircumstances.VhclHitSideRoad = RTAServicesLibraryV3.C00_YNFlag.Item0;
                accidentCircumstances.VhclHitWhilstParked = RTAServicesLibraryV3.C00_YNFlag.Item1;
                accidentDetails.AccidentCircumstances = accidentCircumstances;

                RTAServicesLibraryV3.CT_PoliceDetails police = new RTAServicesLibraryV3.CT_PoliceDetails();
                police.ReferenceNumber = "123";
                police.ReportingOfficerName = "Reporting Officer Name";
                police.StationAddress = "Police Station Address";
                police.StationName = "Police Station Name";
                accidentDetails.PoliceDetails = police;

                accidentData.AccidentDetails = accidentDetails;

                RTAServicesLibraryV3.CT_INPUT_BusCoach busCoach = new RTAServicesLibraryV3.CT_INPUT_BusCoach();
                busCoach.BussOrCoach = GetYesNoV3(cmbBussOrCoach);
                busCoach.Evidence = GetYesNoV3(cmbBusOrCoachEvidence);
                busCoach.VehicleDescription = tbBusOrCoachVehicleDescription.Text;
                busCoach.Comments = "Some Comments";
                busCoach.DriverDescription = "Description of the driver =drunk";
                busCoach.DriverID = "123";
                busCoach.DriverName = "Driver Name";
                busCoach.NumberOfPassengers = "2";
                accidentData.BusCoach = busCoach;

                doc.AccidentData = accidentData;

                RTAServicesLibraryV3.DocumentInputLiabilityFunding liabilityFunding = new RTAServicesLibraryV3.DocumentInputLiabilityFunding();
                RTAServicesLibraryV3.CT_INPUT_Liability liability = new RTAServicesLibraryV3.CT_INPUT_Liability();
                liability.DefendantResponsability = tbDefendantResponsability.Text;
                liabilityFunding.Liability = liability;

                RTAServicesLibraryV3.CT_INPUT_Funding funding = new RTAServicesLibraryV3.CT_INPUT_Funding();
                funding.ConsideredFreeLegalExpIns = GetYesNoV3(cmbConsideredFreeLegalExpIns);
                funding.FundingUndertaken = GetYesNoV3(cmbFundingUndertaken);
                funding.AgreementDate = DateTime.Now;
                funding.Comments = "Section M - Other relevant information";
                funding.ConditionalFeeDate = DateTime.Now;
                funding.ICAddress = "Address of Insurance Company";
                funding.ICName = "Insurance Company Name";
                funding.IncreasingPoint = "At which point is an increased premium payable?";
                funding.LevelOfCover = "Level of cover";
                funding.MembershipOrganisation = RTAServicesLibraryV3.C00_YNFlag.Item1;
                funding.Ohter = RTAServicesLibraryV3.C00_YNFlag.Item1;
                funding.OrganizationName = "Organisation Name";
                funding.OtherDetails = "Other details";
                funding.PolicyDate = DateTime.Now;
                funding.PolicyNumber = "123";
                funding.PremiumsStaged = RTAServicesLibraryV3.C00_YNFlag.Item0;
                funding.Section29 = RTAServicesLibraryV3.C00_YNFlag.Item1;
                funding.Section58 = RTAServicesLibraryV3.C00_YNFlag.Item1;
                liabilityFunding.Funding = funding;

                doc.LiabilityFunding = liabilityFunding;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return doc;
        }

        #endregion V3

        #region V7
        /// <summary>
        /// Get Claimant CNF
        /// </summary>
        /// <returns></returns>
        private RTAServicesLibraryV7.DocumentInput GetClaimantCNFV7()
        {
            //  This part of the CNF is completed by the Claimants Representative
            RTAServicesLibraryV7.DocumentInput doc = new RTAServicesLibraryV7.DocumentInput();

            try
            {
                //  ApplicationData
                RTAServicesLibraryV7.DocumentInputApplicationData appData = new RTAServicesLibraryV7.DocumentInputApplicationData();
                RTAServicesLibraryV7.CT_INPUT_Claim_Details claimDetails = new RTAServicesLibraryV7.CT_INPUT_Claim_Details();
                claimDetails.RetainedCopy = GetYesNoV7(cmbClaimantRepresentativeRetainedCopy);
                claimDetails.Signatory = GetSignatoryV7(cmbClaimantRepresentativeSignatory);
                claimDetails.OICReferenceNumber = "OIC-123";
                // claimDetails.ClaimValue

                appData.ClaimDetails = claimDetails;
                doc.ApplicationData = appData;

                //  ClaimAndClaimantDetails - ClaimantRepresentative
                RTAServicesLibraryV7.DocumentInputClaimAndClaimantDetails claimAndClaimantDetails = new RTAServicesLibraryV7.DocumentInputClaimAndClaimantDetails();
                RTAServicesLibraryV7.DocumentInputClaimAndClaimantDetailsClaimantRepresentative representative = new RTAServicesLibraryV7.DocumentInputClaimAndClaimantDetailsClaimantRepresentative();
                RTAServicesLibraryV7.CT_INPUT_ClaimantRepresentative_CompanyDetails companyDetails = new RTAServicesLibraryV7.CT_INPUT_ClaimantRepresentative_CompanyDetails();
                companyDetails.CompanyName = tbClaimantRepresentativeCompanyName.Text;
                companyDetails.ContactName = tbClaimantRepresentativeContactName.Text;
                companyDetails.ContactSurname = tbClaimantRepresentativeContactSurname.Text;
                companyDetails.ReferenceNumber = tbClaimantRepresentativeReferenceNumber.Text;
                companyDetails.TelephoneNumber = tbClaimantRepresentativeTelephone.Text;
                companyDetails.EmailAddress = "alan.g@fwbs.net";

                RTAServicesLibraryV7.CT_INPUT_Address crAddress = new RTAServicesLibraryV7.CT_INPUT_Address();
                crAddress.Street1 = tbClaimantRepresentativeStreet.Text;
                crAddress.City = tbClaimantRepresentativeCity.Text;
                crAddress.Country = tbClaimantRepresentativeCountry.Text;
                crAddress.PostCode = tbClaimantRepresentativePostCode.Text;
                crAddress.AddressType = GetAddressType<RTAServicesLibraryV7.C01_AddressType>(cmbClaimantRepresentativeAddressType);
                crAddress.HouseName = "A House";

                companyDetails.Address = crAddress;
                representative.CompanyDetails = companyDetails;
                var referralSourceInfo = new RTAServicesLibraryV7.CT_INPUT_ReferralSourceInfo();
                referralSourceInfo.ReferralSource = RTAServicesLibraryV7.C32_ReferralSource.N;
                representative.ReferralSourceInfo = referralSourceInfo;
                claimAndClaimantDetails.ClaimantRepresentative = representative;

                //  DefendantDetails 
                RTAServicesLibraryV7.CT_INPUT_DefendantDetails defendantDetails = new RTAServicesLibraryV7.CT_INPUT_DefendantDetails();
                defendantDetails.DefedantAge = tbDefendantAge.Text;
                defendantDetails.DefendandDescription = tbDefendantDescription.Text;
                defendantDetails.DefendantDetailsObtained = tbDefendantDetailsObtained.Text;
                defendantDetails.DefendantStatus = GetStatusV7(cmbDefendantStatus);
                defendantDetails.PolicyNumberReference = "12345678";

                RTAServicesLibraryV7.CT_INPUT_Defendant_PersonalDetails defendantPersonal = new RTAServicesLibraryV7.CT_INPUT_Defendant_PersonalDetails();
                defendantPersonal.Sex = GetSexV7(cmbDefendantSex);
                defendantPersonal.SexSpecified = true;
                defendantPersonal.Surname = tbDefedantSurname.Text;

                RTAServicesLibraryV7.CT_INPUT_Address personalAddress = new RTAServicesLibraryV7.CT_INPUT_Address();
                personalAddress.Street1 = tbDefendantStreet.Text;
                personalAddress.City = tbDefendantCity.Text;
                personalAddress.Country = tbDefendantCountry.Text;
                personalAddress.AddressType = GetAddressType<RTAServicesLibraryV7.C01_AddressType>(cmbDefendantAddressType);
                personalAddress.HouseName = "The Times";
                defendantPersonal.Address = personalAddress;
                defendantDetails.PersonalDetails = defendantPersonal;

                RTAServicesLibraryV7.CT_INPUT_Vehicle vehicle = new RTAServicesLibraryV7.CT_INPUT_Vehicle();
                vehicle.VRN = tbDefendantVehicleVRN.Text;
                defendantDetails.Vehicle = vehicle;

                RTAServicesLibraryV7.CT_INPUT_InsurerInformation insurerInformation = new RTAServicesLibraryV7.CT_INPUT_InsurerInformation();
                insurerInformation.InsurerType = GetInsurerTypeV7(cmbDefendantInsurerType);

                //  For testing A2A
                insurerInformation.InsurerName = "FWBS COMP";
                insurerInformation.InsurerOrganizationID = "COMP009";
                insurerInformation.InsurerOrganizationPath = "/C00009";

                defendantDetails.InsurerInformation = insurerInformation;

                RTAServicesLibraryV7.CT_INPUT_Defendant_CompanyDetails defendantCompanyDetails = new RTAServicesLibraryV7.CT_INPUT_Defendant_CompanyDetails();
                defendantCompanyDetails.CompanyName = tbDefendantCompanyName.Text;

                RTAServicesLibraryV7.CT_INPUT_Address defendantCompanyAddress = new RTAServicesLibraryV7.CT_INPUT_Address();
                defendantCompanyAddress.Street1 = tbDefendantCompanyStreet.Text;
                defendantCompanyAddress.City = tbDefendantCompanyCity.Text;
                defendantCompanyAddress.Country = tbDefendantCompanyCountry.Text;
                defendantCompanyAddress.AddressType = GetAddressType<RTAServicesLibraryV7.C01_AddressType>(cmbDefendantCompanyAddressType);
                defendantCompanyDetails.Address = defendantCompanyAddress;

                defendantDetails.CompanyDetails = defendantCompanyDetails;
                claimAndClaimantDetails.DefendantDetails = defendantDetails;

                //  Claimant Details
                RTAServicesLibraryV7.CT_INPUT_ClaimantDetails claimantDetails = new RTAServicesLibraryV7.CT_INPUT_ClaimantDetails();
                claimantDetails.ChildClaim = GetYesNoV7(cmbClaimantChildClaim);
                claimantDetails.Occupation = tbClaimantOccupation.Text;
                claimantDetails.AskCUEPIReference = "ASKCUEPI0123456789";
                RTAServicesLibraryV7.CT_Vehicle claimantVehice = new RTAServicesLibraryV7.CT_Vehicle();
                claimantDetails.Vehicle = claimantVehice;

                //  National Insurance Number
                //  Characters 1-2 must be in the range AA-ZZ
                //  Character 1 must not be D, F, I, Q, U, V
                //  Character 2 must not be D, F, I, O, Q, U, V
                //  Characters 1-2 must not be one of the following combinations: FY; GB; NK; TN; TT; ZZ.
                //  Characters 3-6 must be in the range 0000 – 9999
                //  Characters 7-8 must be in the range 00 – 99
                //  Character 9 must be in the range A – D

                //  Only include one or the other (Not Both)
                claimantDetails.NationalInsuranceNumber = tbClaimantNationalInsuranceNumber.Text;
                if (tbClaimantNationalInsuranceNumber.Text.Equals(string.Empty))
                {
                    if (tbClaimantNationalInsuranceNumberComments.Text.Equals(string.Empty))
                    {
                        claimantDetails.NINComment = "Comments";
                    }
                    else
                    {
                        claimantDetails.NINComment = tbClaimantNationalInsuranceNumberComments.Text;
                    }
                }

                RTAServicesLibraryV7.CT_INPUT_Claimant_PersonalDetails claimantPersonalDetails = new RTAServicesLibraryV7.CT_INPUT_Claimant_PersonalDetails();
                claimantPersonalDetails.DateOfBirth = Convert.ToDateTime(tbClaimantDOB.Text);
                claimantPersonalDetails.Name = tbClaimantName.Text;
                claimantPersonalDetails.Surname = tbClaimantSurname.Text;
                claimantPersonalDetails.TitleType = RTAServicesLibraryV7.C02_TitleType.Item1;

                RTAServicesLibraryV7.CT_INPUT_Address claimantPersonalDetailsAddress = new RTAServicesLibraryV7.CT_INPUT_Address();
                claimantPersonalDetailsAddress.HouseName = "Home";
                claimantPersonalDetailsAddress.Street1 = tbClaimantStreet.Text;
                claimantPersonalDetailsAddress.City = tbClaimantCity.Text;
                claimantPersonalDetailsAddress.Country = tbClaimantCountry.Text;
                claimantPersonalDetailsAddress.AddressType = GetAddressType<RTAServicesLibraryV7.C01_AddressType>(cmbClaimantAddressType);
                claimantPersonalDetails.Address = claimantPersonalDetailsAddress;
                claimantDetails.PersonalDetails = claimantPersonalDetails;
                claimAndClaimantDetails.ClaimantDetails = claimantDetails;

                doc.ClaimAndClaimantDetails = claimAndClaimantDetails;


                //  MedicalDetails 
                RTAServicesLibraryV7.DocumentInputMedicalDetails medicalDetails = new RTAServicesLibraryV7.DocumentInputMedicalDetails();
                RTAServicesLibraryV7.CT_INPUT_Injury injury = new RTAServicesLibraryV7.CT_INPUT_Injury();
                injury.HospitalAttendance = GetYesNoV7(cmbMedicalHospitalAttendance);
                injury.InjurySustainedDescription = tbMedicalInjurySustainedDescription.Text;
                injury.MedicalAttentionSeeking = GetYesNoV7(cmbMedicalMedicalAttentionSeeking);
                injury.TimeOffRequired = GetYesNoV7(cmbMedicalTimeOffRequired);

                injury.BoneInjury = RTAServicesLibraryV7.C00_YNFlag.Item1;
                injury.BoneInjurySpecified = true;
                injury.DaysNumber = "3";
                injury.MedicalAttentionFirstDate = DateTime.Now;
                injury.MedicalAttentionFirstDateSpecified = true;
                injury.Other = RTAServicesLibraryV7.C00_YNFlag.Item1;
                injury.OtherSpecified = true;
                injury.OvernightDetention = RTAServicesLibraryV7.C00_YNFlag.Item1;
                injury.OvernightDetentionSpecified = true;
                injury.SoftTissue = RTAServicesLibraryV7.C00_YNFlag.Item1;
                injury.SoftTissueSpecified = true;
                injury.StillOffWork = RTAServicesLibraryV7.C00_YNFlag.Item0;
                injury.StillOffWorkSpecified = true;
                injury.TimeOffPeriod = "3";
                injury.Whiplash = RTAServicesLibraryV7.C00_YNFlag.Item1;
                injury.WhiplashSpecified = true;
                medicalDetails.Injury = injury;

                RTAServicesLibraryV7.CT_INPUT_Hospital_R3[] hospitals = new RTAServicesLibraryV7.CT_INPUT_Hospital_R3[1];
                RTAServicesLibraryV7.CT_INPUT_Hospital_R3 hospital = new RTAServicesLibraryV7.CT_INPUT_Hospital_R3();
                hospital.HospitalName = "Hospital name";
                hospital.PostCode = "DG3 5VN";
                hospital.HospitalType = RTAServicesLibraryV7.C07_HospitalType_R3.Item0;

                RTAServicesLibraryV7.CT_HospitalAddress hospitalAddress = new RTAServicesLibraryV7.CT_HospitalAddress();
                hospitalAddress.AddressLine1 = "Hospital address1";
                hospitalAddress.AddressLine2 = "Hospital address2";
                hospitalAddress.AddressLine3 = "Hospital address3";
                hospitalAddress.AddressLine4 = "Hospital address4";
                hospital.HospitalAddress = hospitalAddress;

                hospitals[0] = hospital;
                medicalDetails.Hospital = hospitals;

                RTAServicesLibraryV7.CT_INPUT_Rehabilitation rehabilitation = new RTAServicesLibraryV7.CT_INPUT_Rehabilitation();
                rehabilitation.RehabilitationUndertaken = RTAServicesLibraryV7.C03_RehabilitationRecommended.Item0;
                rehabilitation.RehabilitationDetails = "Full details on rahabilitaion needs claimant has arising out of the accident";
                rehabilitation.RehabilitationNeeds = RTAServicesLibraryV7.C00_YNFlag.Item1;
                rehabilitation.RehabilitationNeedsSpecified = true;
                rehabilitation.RehabilitationTreatment = "tails of the rehabilitation treatment recommended and any treatment provided including name of provider";

                medicalDetails.Rehabilitation = rehabilitation;
                doc.MedicalDetails = medicalDetails;

                //  RepairsAndAlternativeVehicleProvision
                RTAServicesLibraryV7.DocumentInputRepairsAndAlternativeVehicleProvision repairsAndAlternativeVehicleProvision = new RTAServicesLibraryV7.DocumentInputRepairsAndAlternativeVehicleProvision();
                RTAServicesLibraryV7.CT_INPUT_Repairs repairs = new RTAServicesLibraryV7.CT_INPUT_Repairs();
                repairs.ClaiimingDamageOwnVehicle = GetYesNoV7(cmbClaimingDamageOwnVehicle);
                repairs.DetailsOfTheInsurance = RTAServicesLibraryV7.C04_DetailsOfTheInsurance.Item0;
                repairs.ThroughClaimantInsurer = GetYesNoV7(cmbThroughClaimantInsurer);
                repairs.TotalLoss = GetTotalLossV7(cmbTotalLoss);

                repairs.ContactDetails = "Contact for  the defendants insurer";
                repairs.DefendantInsInspection = RTAServicesLibraryV7.C00_YNFlag.Item1;
                repairs.Location = "Location for  the defendants insurer";
                repairs.OtherDetails = "other details of the insurance";
                repairs.RepairsPosition = RTAServicesLibraryV7.C05_RepairsPosition.Item3;
                repairs.ThroughAlternatieCompany = RTAServicesLibraryV7.C00_YNFlag.Item1;

                RTAServicesLibraryV7.CT_INPUT_AlternativeCompany alternativeCompany = new RTAServicesLibraryV7.CT_INPUT_AlternativeCompany();
                alternativeCompany.Address = "Alternative company address";
                alternativeCompany.CompanyName = "Alternative company";
                alternativeCompany.ReferenceNumber = "123";
                alternativeCompany.TelephoneNumber = "12345678";
                repairs.AlternativeCompany = alternativeCompany;
                repairsAndAlternativeVehicleProvision.Repairs = repairs;

                //  AlternativeVehicleProvision 
                RTAServicesLibraryV7.CT_INPUT_AlternativeVehicleProvision alternativeVehicleProvision = new RTAServicesLibraryV7.CT_INPUT_AlternativeVehicleProvision();
                alternativeVehicleProvision.AVProvided = GetYesNoV7(cmbAVProvided);
                alternativeVehicleProvision.AVRequiredByCL = GetYesNoV7(cmbAVRequiredByCL);
                alternativeVehicleProvision.AVRequiredByCR = GetYesNoV7(cmbAVRequiredByCR);
                alternativeVehicleProvision.ClaimantEntitled = GetYesNoV7(cmbClaimantEntitled);

                RTAServicesLibraryV7.CT_INPUT_Provider provider = new RTAServicesLibraryV7.CT_INPUT_Provider();
                provider.ProviderName = tbProviderName.Text;
                provider.ReferenceNumber = tbProviderReferenceNumber.Text;
                provider.StartDate = Convert.ToDateTime(tbProviderStartDate.Text);
                provider.EndDate = tbProviderEndDate.Text;
                provider.ProviderAddress = "Provider Address";

                RTAServicesLibraryV7.CT_Vehicle providerVehicle = new RTAServicesLibraryV7.CT_Vehicle();
                providerVehicle.Color = "BlackProvided";
                providerVehicle.EngineSize = "12";
                providerVehicle.Make = "MakeProvided";
                providerVehicle.Model = "ModelBestProvided";
                providerVehicle.VRN = "12341234";
                provider.Vehicle = providerVehicle;

                alternativeVehicleProvision.Provider = provider;
                repairsAndAlternativeVehicleProvision.AlternativeVehicleProvision = alternativeVehicleProvision;
                doc.RepairsAndAlternativeVehicleProvision = repairsAndAlternativeVehicleProvision;

                //  AccidentData 
                RTAServicesLibraryV7.DocumentInputAccidentData accidentData = new RTAServicesLibraryV7.DocumentInputAccidentData();
                RTAServicesLibraryV7.CT_INPUT_AccidentDetails accidentDetails = new RTAServicesLibraryV7.CT_INPUT_AccidentDetails();
                accidentDetails.AccidentDate = Convert.ToDateTime(tbAccidentDate.Text);
                accidentDetails.AccidentDescription = tbAccidentDescription.Text;
                accidentDetails.AccidentLocation = tbAccidentLocation.Text;
                accidentDetails.AccidentTime = tbAccidentTime.Text;
                accidentDetails.ClaimantType = RTAServicesLibraryV7.C06_ClaimantType.Item0;
                accidentDetails.OccupantsNumber = tbAccidentOccupantsNumber.Text;
                accidentDetails.Seatbelt = RTAServicesLibraryV7.C11_SeatbeltWearing.Item1;
                accidentDetails.SeatbeltSpecified = true;
                accidentDetails.DriverIsDefendant = RTAServicesLibraryV7.C00_YNFlag.Item1;
                accidentDetails.DriverIsDefendantSpecified = true;

                RTAServicesLibraryV7.CT_INPUT_Driver_PersonalDetails driverPersonalDetails = new RTAServicesLibraryV7.CT_INPUT_Driver_PersonalDetails();
                driverPersonalDetails.Surname = "John";
                driverPersonalDetails.MiddleName = "A";
                driverPersonalDetails.Name = "William";

                RTAServicesLibraryV7.CT_INPUT_Address driverPersonalDetailsAddress = new RTAServicesLibraryV7.CT_INPUT_Address();
                driverPersonalDetailsAddress.Street1 = tbDefendantDriverStreet.Text;
                driverPersonalDetailsAddress.City = tbDefendantDriverCity.Text;
                driverPersonalDetailsAddress.Country = tbDefendantDriverCountry.Text;
                driverPersonalDetailsAddress.AddressType = GetAddressType<RTAServicesLibraryV7.C01_AddressType>(cmbDefendantDriverAddressType);
                driverPersonalDetailsAddress.HouseName = "The Trees";
                driverPersonalDetailsAddress.District = "Abington";
                driverPersonalDetailsAddress.HouseNumber = "10";
                driverPersonalDetails.Address = driverPersonalDetailsAddress;
                accidentDetails.Driver = driverPersonalDetails;

                RTAServicesLibraryV7.CT_INPUT_VehicleOwner_PersonalDetails vehicleOwnerPersonalDetails = new RTAServicesLibraryV7.CT_INPUT_VehicleOwner_PersonalDetails();

                RTAServicesLibraryV7.CT_INPUT_Address vehicleOwnerPersonalDetailsAddress = new RTAServicesLibraryV7.CT_INPUT_Address();
                accidentDetails.Owner = vehicleOwnerPersonalDetails;

                //  Insurer Details
                RTAServicesLibraryV7.CT_INPUT_Insurance_CompanyDetails insuranceCompanyDetails = new RTAServicesLibraryV7.CT_INPUT_Insurance_CompanyDetails();
                RTAServicesLibraryV7.CT_INPUT_Address insuranceCompanyDetailsAddress = new RTAServicesLibraryV7.CT_INPUT_Address();
                accidentDetails.InsuranceCompanyInformatiion = insuranceCompanyDetails;

                RTAServicesLibraryV7.CT_WeatherConditions weather = new RTAServicesLibraryV7.CT_WeatherConditions();
                weather.Fog = RTAServicesLibraryV7.C00_YNFlag.Item0;
                weather.Ice = RTAServicesLibraryV7.C00_YNFlag.Item0;
                weather.Other = RTAServicesLibraryV7.C00_YNFlag.Item0;
                weather.Rain = RTAServicesLibraryV7.C00_YNFlag.Item0;
                weather.Snow = RTAServicesLibraryV7.C00_YNFlag.Item1;
                weather.Sun = RTAServicesLibraryV7.C00_YNFlag.Item0;
                accidentDetails.WeatherConditions = weather;

                RTAServicesLibraryV7.CT_RoadConditions roadConditions = new RTAServicesLibraryV7.CT_RoadConditions();
                roadConditions.Dry = RTAServicesLibraryV7.C00_YNFlag.Item0;
                roadConditions.Ice = RTAServicesLibraryV7.C00_YNFlag.Item0;
                roadConditions.Mud = RTAServicesLibraryV7.C00_YNFlag.Item1;
                roadConditions.Oil = RTAServicesLibraryV7.C00_YNFlag.Item0;
                roadConditions.Other = RTAServicesLibraryV7.C00_YNFlag.Item0;
                roadConditions.Snow = RTAServicesLibraryV7.C00_YNFlag.Item0;
                roadConditions.Wet = RTAServicesLibraryV7.C00_YNFlag.Item0;
                accidentDetails.RoadConditions = roadConditions;

                RTAServicesLibraryV7.CT_AccidentCircumstances accidentCircumstances = new RTAServicesLibraryV7.CT_AccidentCircumstances();
                accidentCircumstances.AccidCarPark = RTAServicesLibraryV7.C00_YNFlag.Item0;
                accidentCircumstances.AccidChangingLanes = RTAServicesLibraryV7.C00_YNFlag.Item0;
                accidentCircumstances.AccidRoundabout = RTAServicesLibraryV7.C00_YNFlag.Item0;
                accidentCircumstances.ConcertinaCollision = RTAServicesLibraryV7.C00_YNFlag.Item0;
                accidentCircumstances.Other = RTAServicesLibraryV7.C00_YNFlag.Item0;
                accidentCircumstances.VhclHitInRear = RTAServicesLibraryV7.C00_YNFlag.Item0;
                accidentCircumstances.VhclHitSideRoad = RTAServicesLibraryV7.C00_YNFlag.Item0;
                accidentCircumstances.VhclHitWhilstParked = RTAServicesLibraryV7.C00_YNFlag.Item1;
                accidentDetails.AccidentCircumstances = accidentCircumstances;

                RTAServicesLibraryV7.CT_PoliceDetails police = new RTAServicesLibraryV7.CT_PoliceDetails();
                police.ReferenceNumber = "123";
                police.ReportingOfficerName = "Reporting Officer Name";
                police.StationAddress = "Police Station Address";
                police.StationName = "Police Station Name";
                accidentDetails.PoliceDetails = police;

                accidentData.AccidentDetails = accidentDetails;

                RTAServicesLibraryV7.CT_INPUT_BusCoach busCoach = new RTAServicesLibraryV7.CT_INPUT_BusCoach();
                busCoach.BussOrCoach = GetYesNoV7(cmbBussOrCoach);
                busCoach.Evidence = GetYesNoV7(cmbBusOrCoachEvidence);
                busCoach.VehicleDescription = tbBusOrCoachVehicleDescription.Text;
                busCoach.Comments = "Some Comments";
                busCoach.DriverDescription = "Description of the driver =drunk";
                busCoach.DriverID = "123";
                busCoach.DriverName = "Driver Name";
                busCoach.NumberOfPassengers = "2";
                accidentData.BusCoach = busCoach;

                doc.AccidentData = accidentData;

                RTAServicesLibraryV7.DocumentInputLiabilityFunding liabilityFunding = new RTAServicesLibraryV7.DocumentInputLiabilityFunding();
                RTAServicesLibraryV7.CT_INPUT_Liability liability = new RTAServicesLibraryV7.CT_INPUT_Liability();
                liability.DefendantResponsability = tbDefendantResponsability.Text;
                liabilityFunding.Liability = liability;

                RTAServicesLibraryV7.DocumentInputLiabilityFundingFunding funding = new RTAServicesLibraryV7.DocumentInputLiabilityFundingFunding();
                funding.Comments = "Section M - Other relevant information";

                liabilityFunding.Funding = funding;

                doc.LiabilityFunding = liabilityFunding;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return doc;
        }

        #endregion V3

        #endregion GetClaimantCNF

        /// <summary>
        /// Save File
        /// </summary>
        private void SaveFile(byte[] buffer, string filePath)
        {
            Stream outStream = File.OpenWrite(filePath);
            outStream.Write(buffer, 0, buffer.Length);
            outStream.Close();
        }

        #region Private Methods

        /// <summary>
        /// Display Claim Data
        /// </summary>
        /// <param name="info"></param>
        private void DisplayClaimData(claimInfo info)
        {
            tbClaimApplicationID.Text = info.applicationId;
            tbClaimActivityEngineGuid.Text = info.activityEngineGuid;
            tbClaimPhaseCacheID.Text = info.phaseCacheId;
            tbClaimPhaseCacheName.Text = info.phaseCacheName;
        }

        /// <summary>
        /// Activity Engine Guid Empty
        /// </summary>
        /// <returns></returns>
        private bool ActivityEngineGuidEmpty()
        {
            if (tbClaimActivityEngineGuid.Text == string.Empty)
            {
                MessageBox.Show("Please enter an Activity Engine Guid");
                tbClaimActivityEngineGuid.Focus();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Application ID Empty
        /// </summary>
        /// <returns></returns>
        private bool ApplicationIDEmpty()
        {
            if (tbClaimApplicationID.Text == string.Empty)
            {
                MessageBox.Show("Please enter an Application ID");
                tbClaimApplicationID.Focus();
                return true;
            }

            return false;
        }

        /// <summary>
        /// SetUp a Yes No Combobox
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpYesNo(ComboBox combobox)
        {
            //  C00_YNFlag.Item0 = No
            //  C00_YNFlag.Item1 = Yes

            combobox.Items.Add(COMBO_NO);
            combobox.Items.Add(COMBO_YES);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// Get Yes/No value from Combobox
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV2.C00_YNFlag GetYesNo(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_YES:
                    return RTAServicesLibraryV2.C00_YNFlag.Item1;
                case COMBO_NO:
                    return RTAServicesLibraryV2.C00_YNFlag.Item0;
                default:
                    return RTAServicesLibraryV2.C00_YNFlag.Item0;
            }
        }

        private RTAServicesLibraryV3.C00_YNFlag GetYesNoV3(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_YES:
                    return RTAServicesLibraryV3.C00_YNFlag.Item1;
                case COMBO_NO:
                    return RTAServicesLibraryV3.C00_YNFlag.Item0;
                default:
                    return RTAServicesLibraryV3.C00_YNFlag.Item0;
            }
        }

        private RTAServicesLibraryV7.C00_YNFlag GetYesNoV7(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_YES:
                    return RTAServicesLibraryV7.C00_YNFlag.Item1;
                case COMBO_NO:
                    return RTAServicesLibraryV7.C00_YNFlag.Item0;
                default:
                    return RTAServicesLibraryV7.C00_YNFlag.Item0;
            }
        }

        #region Sex

        /// <summary>
        /// SetUp Sex
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpSex(ComboBox combobox)
        {
            combobox.Items.Add(COMBO_SEX_MALE);
            combobox.Items.Add(COMBO_SEX_FEMALE);
            combobox.Items.Add(COMBO_SEX_NOTKNOWN);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// Get Sex
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV2.C08_Sex GetSex(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_SEX_MALE:
                    return RTAServicesLibraryV2.C08_Sex.M;
                case COMBO_SEX_FEMALE:
                    return RTAServicesLibraryV2.C08_Sex.F;
                case COMBO_SEX_NOTKNOWN:
                    return RTAServicesLibraryV2.C08_Sex.N;
                default:
                    return RTAServicesLibraryV2.C08_Sex.N;
            }
        }


        /// <summary>
        /// Get Sex
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV3.C08_Sex GetSexV3(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_SEX_MALE:
                    return RTAServicesLibraryV3.C08_Sex.M;
                case COMBO_SEX_FEMALE:
                    return RTAServicesLibraryV3.C08_Sex.F;
                case COMBO_SEX_NOTKNOWN:
                    return RTAServicesLibraryV3.C08_Sex.N;
                default:
                    return RTAServicesLibraryV3.C08_Sex.N;
            }
        }

        /// <summary>
        /// Get Sex
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV7.C08_Sex GetSexV7(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_SEX_MALE:
                    return RTAServicesLibraryV7.C08_Sex.M;
                case COMBO_SEX_FEMALE:
                    return RTAServicesLibraryV7.C08_Sex.F;
                case COMBO_SEX_NOTKNOWN:
                    return RTAServicesLibraryV7.C08_Sex.N;
                default:
                    return RTAServicesLibraryV7.C08_Sex.N;
            }
        }

        #endregion Sex


        #region Signatory

        /// <summary>
        /// SetUp Signatory
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpSignatory(ComboBox combobox)
        {
            combobox.Items.Add(COMBO_CLAIMANT_SOLICITOR);
            combobox.Items.Add(COMBO_CLAIMANT_IN_PERSON);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// Get Signatory
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV2.C16_SignatoryType GetSignatory(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_CLAIMANT_SOLICITOR:
                    return RTAServicesLibraryV2.C16_SignatoryType.S;
                case COMBO_CLAIMANT_IN_PERSON:
                    return RTAServicesLibraryV2.C16_SignatoryType.C;
                default:
                    return RTAServicesLibraryV2.C16_SignatoryType.S;
            }
        }


        /// <summary>
        /// Get Signatory
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV3.C16_SignatoryType GetSignatoryV3(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_CLAIMANT_SOLICITOR:
                    return RTAServicesLibraryV3.C16_SignatoryType.S;
                case COMBO_CLAIMANT_IN_PERSON:
                    return RTAServicesLibraryV3.C16_SignatoryType.C;
                default:
                    return RTAServicesLibraryV3.C16_SignatoryType.S;
            }
        }

        /// <summary>
        /// Get Signatory
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV7.C16_SignatoryType GetSignatoryV7(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_CLAIMANT_SOLICITOR:
                    return RTAServicesLibraryV7.C16_SignatoryType.S;
                case COMBO_CLAIMANT_IN_PERSON:
                    return RTAServicesLibraryV7.C16_SignatoryType.C;
                default:
                    return RTAServicesLibraryV7.C16_SignatoryType.S;
            }
        }
        #endregion Signatory


        #region ClaimValue - V3+

        private RTAServicesLibraryV3.C29_ClaimValue GetClaimValue(ComboBox comboBox)
        {
            switch (comboBox.SelectedItem.ToString())
            {
                case "1":
                    return RTAServicesLibraryV3.C29_ClaimValue.Item1;
                case "2":
                    return RTAServicesLibraryV3.C29_ClaimValue.Item2;
                default:
                    return RTAServicesLibraryV3.C29_ClaimValue.Item1;
            }
        }

        #endregion ClaimValue - V3+


        #region Address

        /// <summary>
        /// SetUp an Address Type Combobox
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpAddressType(ComboBox combobox)
        {
            //  C01_AddressType.A = As input
            //  C01_AddressType.F = 
            //  C01_AddressType.P = Personal

            combobox.Items.Add(COMBO_ADDRESSTYPE_A);
            combobox.Items.Add(COMBO_ADDRESSTYPE_F);
            combobox.Items.Add(COMBO_ADDRESSTYPE_P);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// Get AddressType value from Combobox
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV2.C01_AddressType GetAddressType(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_ADDRESSTYPE_A:
                    return RTAServicesLibraryV2.C01_AddressType.A;
                case COMBO_ADDRESSTYPE_F:
                    return RTAServicesLibraryV2.C01_AddressType.F;
                case COMBO_ADDRESSTYPE_P:
                    return RTAServicesLibraryV2.C01_AddressType.P;
                default:
                    return RTAServicesLibraryV2.C01_AddressType.A;
            }
        }


        /// <summary>
        /// Get AddressType value from Combobox
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV3.C01_AddressType GetAddressTypeV3(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_ADDRESSTYPE_A:
                    return RTAServicesLibraryV3.C01_AddressType.A;
                case COMBO_ADDRESSTYPE_F:
                    return RTAServicesLibraryV3.C01_AddressType.F;
                case COMBO_ADDRESSTYPE_P:
                    return RTAServicesLibraryV3.C01_AddressType.P;
                default:
                    return RTAServicesLibraryV3.C01_AddressType.A;
            }
        }

        /// <summary>
        /// Get AddressType value from Combobox
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private T_C01_AddressType GetAddressType<T_C01_AddressType>(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_ADDRESSTYPE_A:
                    return (T_C01_AddressType)(object)RTAServicesLibraryV7.C01_AddressType.A;
                case COMBO_ADDRESSTYPE_F:
                    return (T_C01_AddressType)(object)RTAServicesLibraryV7.C01_AddressType.F;
                case COMBO_ADDRESSTYPE_P:
                    return (T_C01_AddressType)(object)RTAServicesLibraryV7.C01_AddressType.P;
                default:
                    return (T_C01_AddressType)(object)RTAServicesLibraryV7.C01_AddressType.A;
            }
        }

        #endregion Address


        #region TotalLoss

        /// <summary>
        /// SetUp Total Loss
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpTotalLoss(ComboBox combobox)
        {
            combobox.Items.Add(COMBO_TOTALLOSS_YES);
            combobox.Items.Add(COMBO_TOTALLOSS_NO);
            combobox.Items.Add(COMBO_TOTALLOSS_DONTKNOW);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// Get Total Loss
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV2.C14_YNDK GetTotalLoss(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_TOTALLOSS_YES:
                    return RTAServicesLibraryV2.C14_YNDK.Item0;
                case COMBO_TOTALLOSS_NO:
                    return RTAServicesLibraryV2.C14_YNDK.Item1;
                case COMBO_TOTALLOSS_DONTKNOW:
                    return RTAServicesLibraryV2.C14_YNDK.Item2;
                default:
                    return RTAServicesLibraryV2.C14_YNDK.Item2;
            }
        }

        /// <summary>
        /// Get Total Loss
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV3.C14_YNDK GetTotalLossV3(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_TOTALLOSS_YES:
                    return RTAServicesLibraryV3.C14_YNDK.Item0;
                case COMBO_TOTALLOSS_NO:
                    return RTAServicesLibraryV3.C14_YNDK.Item1;
                case COMBO_TOTALLOSS_DONTKNOW:
                    return RTAServicesLibraryV3.C14_YNDK.Item2;
                default:
                    return RTAServicesLibraryV3.C14_YNDK.Item2;
            }
        }

        /// <summary>
        /// Get Total Loss
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV7.C14_YNDK GetTotalLossV7(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_TOTALLOSS_YES:
                    return RTAServicesLibraryV7.C14_YNDK.Item0;
                case COMBO_TOTALLOSS_NO:
                    return RTAServicesLibraryV7.C14_YNDK.Item1;
                case COMBO_TOTALLOSS_DONTKNOW:
                    return RTAServicesLibraryV7.C14_YNDK.Item2;
                default:
                    return RTAServicesLibraryV7.C14_YNDK.Item2;
            }
        }

        #endregion TotalLoss


        #region Status
        /// <summary>
        /// SetUp Status
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpStatus(ComboBox combobox)
        {
            combobox.Items.Add(COMBO_STATUS_PERSONAL);
            combobox.Items.Add(COMBO_STATUS_BUSINESS);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// Get Status
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV2.C09_SubjectStatus GetStatus(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_STATUS_PERSONAL:
                    return RTAServicesLibraryV2.C09_SubjectStatus.P;
                case COMBO_STATUS_BUSINESS:
                    return RTAServicesLibraryV2.C09_SubjectStatus.B;
                default:
                    return RTAServicesLibraryV2.C09_SubjectStatus.P;
            }
        }


        /// <summary>
        /// Get Status
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV3.C09_SubjectStatus GetStatusV3(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_STATUS_PERSONAL:
                    return RTAServicesLibraryV3.C09_SubjectStatus.P;
                case COMBO_STATUS_BUSINESS:
                    return RTAServicesLibraryV3.C09_SubjectStatus.B;
                default:
                    return RTAServicesLibraryV3.C09_SubjectStatus.P;
            }
        }

        /// <summary>
        /// Get Status
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV7.C09_SubjectStatus GetStatusV7(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_STATUS_PERSONAL:
                    return RTAServicesLibraryV7.C09_SubjectStatus.P;
                case COMBO_STATUS_BUSINESS:
                    return RTAServicesLibraryV7.C09_SubjectStatus.B;
                default:
                    return RTAServicesLibraryV7.C09_SubjectStatus.P;
            }
        }

        #endregion Status


        #region Insurer
        /// <summary>
        /// SetUp Insurer Type
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpInsurerType(ComboBox combobox)
        {
            combobox.Items.Add(COMBO_INSURERTYPE_INSURER);
            combobox.Items.Add(COMBO_INSURERTYPE_SELFINSURED);
            combobox.Items.Add(COMBO_INSURERTYPE_MIB);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// Get Insurer Type
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV2.C13_InsurerType GetInsurerType(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_INSURERTYPE_INSURER:
                    return RTAServicesLibraryV2.C13_InsurerType.I;
                case COMBO_INSURERTYPE_MIB:
                    return RTAServicesLibraryV2.C13_InsurerType.M;
                case COMBO_INSURERTYPE_SELFINSURED:
                    return RTAServicesLibraryV2.C13_InsurerType.S;
                default:
                    return RTAServicesLibraryV2.C13_InsurerType.I;
            }
        }

        /// <summary>
        /// Get Insurer Type
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV3.C13_InsurerType_R3 GetInsurerTypeV3(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_INSURERTYPE_INSURER:
                    return RTAServicesLibraryV3.C13_InsurerType_R3.I;
                case COMBO_INSURERTYPE_MIB:
                    return RTAServicesLibraryV3.C13_InsurerType_R3.M;
                default:
                    return RTAServicesLibraryV3.C13_InsurerType_R3.I;
            }
        }

        /// <summary>
        /// Get Insurer Type
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV7.C13_InsurerType_R3 GetInsurerTypeV7(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_INSURERTYPE_INSURER:
                    return RTAServicesLibraryV7.C13_InsurerType_R3.I;
                case COMBO_INSURERTYPE_MIB:
                    return RTAServicesLibraryV7.C13_InsurerType_R3.M;
                default:
                    return RTAServicesLibraryV7.C13_InsurerType_R3.I;
            }
        }

        #endregion Insurer


        #region Capacity
        /// <summary>
        /// SetUp Capacity
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpCapacity(ComboBox combobox)
        {
            combobox.Items.Add(COMBO_CAPACITY_CONTRACT);
            combobox.Items.Add(COMBO_CAPACITY_RTA);
            combobox.Items.Add(COMBO_CAPACITY_ARTICLE75);
            combobox.Items.Add(COMBO_CAPACITY_MIB);
            combobox.Items.Add(COMBO_CAPACITY_OTHER);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// Get Capacity
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV2.C15_Capacity GetCapacity(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_CAPACITY_CONTRACT:
                    return RTAServicesLibraryV2.C15_Capacity.Item0;
                case COMBO_CAPACITY_RTA:
                    return RTAServicesLibraryV2.C15_Capacity.Item1;
                case COMBO_CAPACITY_ARTICLE75:
                    return RTAServicesLibraryV2.C15_Capacity.Item2;
                case COMBO_CAPACITY_MIB:
                    return RTAServicesLibraryV2.C15_Capacity.Item3;
                case COMBO_CAPACITY_OTHER:
                    return RTAServicesLibraryV2.C15_Capacity.Item4;
                default:
                    return RTAServicesLibraryV2.C15_Capacity.Item4;
            }
        }


        /// <summary>
        /// Get Capacity
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV3.C15_Capacity GetCapacityV3(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_CAPACITY_CONTRACT:
                    return RTAServicesLibraryV3.C15_Capacity.Item0;
                case COMBO_CAPACITY_RTA:
                    return RTAServicesLibraryV3.C15_Capacity.Item1;
                case COMBO_CAPACITY_ARTICLE75:
                    return RTAServicesLibraryV3.C15_Capacity.Item2;
                case COMBO_CAPACITY_MIB:
                    return RTAServicesLibraryV3.C15_Capacity.Item3;
                case COMBO_CAPACITY_OTHER:
                    return RTAServicesLibraryV3.C15_Capacity.Item4;
                default:
                    return RTAServicesLibraryV3.C15_Capacity.Item4;
            }
        }
        #endregion Capacity


        #region Liability

        /// <summary>
        /// SetUp Liability Decision
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpLiabilityDecision(ComboBox combobox)
        {
            combobox.Items.Add(COMBO_LIABILITY_ADMITTED);
            combobox.Items.Add(COMBO_LIABILITY_ADMITTEDWITHNEGLIGENCE);
            combobox.Items.Add(COMBO_LIABILITY_NOTADMITTED);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// Get Liability Decision
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV2.C17_LiabilityDecision GetLiabilityDecision(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_LIABILITY_ADMITTED:
                    return RTAServicesLibraryV2.C17_LiabilityDecision.A;
                case COMBO_LIABILITY_ADMITTEDWITHNEGLIGENCE:
                    return RTAServicesLibraryV2.C17_LiabilityDecision.AN;
                case COMBO_LIABILITY_NOTADMITTED:
                    return RTAServicesLibraryV2.C17_LiabilityDecision.N;
                default:
                    return RTAServicesLibraryV2.C17_LiabilityDecision.N;
            }
        }

        /// <summary>
        /// Get Liability Decision
        /// </summary>
        /// <param name="combobox"></param>
        /// <returns></returns>
        private RTAServicesLibraryV3.C17_LiabilityDecision GetLiabilityDecisionV3(ComboBox combobox)
        {
            switch (combobox.SelectedItem.ToString())
            {
                case COMBO_LIABILITY_ADMITTED:
                    return RTAServicesLibraryV3.C17_LiabilityDecision.A;
                case COMBO_LIABILITY_ADMITTEDWITHNEGLIGENCE:
                    return RTAServicesLibraryV3.C17_LiabilityDecision.AN;
                case COMBO_LIABILITY_NOTADMITTED:
                    return RTAServicesLibraryV3.C17_LiabilityDecision.N;
                default:
                    return RTAServicesLibraryV3.C17_LiabilityDecision.N;
            }
        }

        #endregion Liability

        /// <summary>
        /// SetUp Payment Decision
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpPaymentDecision(ComboBox combobox)
        {
            combobox.Items.Add(COMBO_PAYMENTDECISION_AC);
            combobox.Items.Add(COMBO_PAYMENTDECISION_NAC);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// SetUp Loss Type
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpLossType(ComboBox combobox)
        {
            combobox.Items.Add(COMBO_LOSSTYPE_POLICYEXCESS);
            combobox.Items.Add(COMBO_LOSSTYPE_LOSSOFUSE);
            combobox.Items.Add(COMBO_LOSSTYPE_CARHIRE);
            combobox.Items.Add(COMBO_LOSSTYPE_REPAIRCOSTS);
            combobox.Items.Add(COMBO_LOSSTYPE_FARES);
            combobox.Items.Add(COMBO_LOSSTYPE_MEDICALEXPENSES);
            combobox.Items.Add(COMBO_LOSSTYPE_COLTHING);
            combobox.Items.Add(COMBO_LOSSTYPE_CARESERVICE);
            combobox.Items.Add(COMBO_LOSSTYPE_LOSSEARNINGSCLAIMANT);
            combobox.Items.Add(COMBO_LOSSTYPE_LOSSEARNINGSEMPLOYER);
            combobox.Items.Add(COMBO_LOSSTYPE_OTHERLOSSES);
            combobox.Items.Add(COMBO_LOSSTYPE_GENERALDAMAGES);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// SetUp Medical Report
        /// </summary>
        /// <param name="combobox"></param>
        private void SetUpMedicalReport(ComboBox combobox)
        {
            combobox.Items.Add(COMBO_MEDICALREPORT_0);
            combobox.Items.Add(COMBO_MEDICALREPORT_1);
            combobox.Items.Add(COMBO_MEDICALREPORT_2);
            combobox.Items.Add(COMBO_MEDICALREPORT_3);
            combobox.Items.Add(COMBO_MEDICALREPORT_4);
            combobox.SelectedIndex = 0;
        }

        /// <summary>
        /// Send Form Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendForm_Click(object sender, EventArgs e)
        {
            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                ForceClaimantRepresentative();

                RTAServicesLibraryV2.DocumentInput doc = null;
                RTAServicesLibraryV3.DocumentInput docV3 = null;
                claimInfo info = null;

                RTAServices1 services = CreateService<RTAServices1>(GetSystemString(), CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER, CR_A2A_MSP_USERID);

                switch (services.GetSystemProcessVersionReleaseCode(services.GetSystemProcessVersion(tbClaimApplicationID.Text, services)))
                {
                    case "R2":
                        doc = GetClaimantCNF();
                        info = services.AddClaim(doc);
                        break;

                    case "R3":
                        docV3 = GetClaimantCNFV3();
                        info = services.AddClaim(docV3);
                        break;
                    case "R7":
                        var docV7 = GetClaimantCNFV7();
                        info = services.AddClaim(docV7);
                        break;

                }

                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        }

        /// <summary>
        /// Exit Button Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Get Active Claims
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetActiveClaims_Click(object sender, EventArgs e)
        {
            RTAServices1 services = CreateService<RTAServices1>(GetSystemString(), CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER, CR_A2A_MSP_USERID);

            try
            {
                dataGridViewClaims.DataSource = services.GetClaimsList();
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Combobox Insurer Reabilitation Provided Selected Index Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbInsurerReabilitationProvided_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbInsurerReabilitationProvided.SelectedItem.Equals(COMBO_YES))
            {
                tbInsurerReabilitationDetails.Enabled = true;
            }
            else
            {
                tbInsurerReabilitationDetails.Enabled = false;
            }
        }

        #endregion Private Methods

        /// <summary>
        /// Generate XML from CNF
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bntGetCNFXML_Click(object sender, EventArgs e)
        {
            string xml = "";

            try
            {
                RTAServices1 services = CreateService<RTAServices1>(GetSystemString(), CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER, CR_A2A_MSP_USERID);
                switch (services.GetSystemProcessVersionReleaseCode(services.GetSystemProcessVersion(tbClaimApplicationID.Text, services)))
                { 
                    case "R2":
                        RTAServicesLibraryV2.DocumentInput docV2 = GetClaimantCNF();
                        xml = services.GenerateClaimantNotificationFormXml(docV2);
                        break;

                    case "R3":
                        RTAServicesLibraryV3.DocumentInput docV3 = GetClaimantCNFV3();
                        xml = services.GenerateClaimantNotificationFormXml(docV3);
                        break;
                }
            
                tbGetCNFXML.Text = xml;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Get Claim List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetClaimList_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                dataGridViewGetClaimsList.DataSource = services.GetClaimsList();
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        ///    get Claim Status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtnGetClaimStatus_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                claimInfo info = services.GetClaimsStatus(tbClaimApplicationID.Text);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Get Services
        /// </summary>
        /// <returns></returns>
        private RTAServices1 GetServices()
        {
            switch (cmbGetNotifications.SelectedItem.ToString())
            {
                case ORG_CR:
                    return CreateService<RTAServices1>(GetSystemString(), CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER, CR_A2A_MSP_USERID);

                case ORG_CM:
                    return CreateService<RTAServices1>(GetSystemString(), COMP_A2A_LOGIN_USERNAME, COMP_A2A_LOGIN_PASSWORD, COMP_A2A_LOGIN_ASUSER, COMP_A2A_MSP_USERID);

                case ORG_CR2:
                    return CreateService<RTAServices1>(GetSystemString(), CR2_A2A_LOGIN_USERNAME, CR2_A2A_LOGIN_PASSWORD, CR2_A2A_LOGIN_ASUSER, CR2_A2A_MSP_USERID);

                case ORG_CM2:
                    return CreateService<RTAServices1>(GetSystemString(), COMP2_A2A_LOGIN_USERNAME, COMP2_A2A_LOGIN_PASSWORD, COMP2_A2A_LOGIN_ASUSER, COMP2_A2A_MSP_USERID);

                default:
                    return CreateService<RTAServices1>(GetSystemString(), CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER, CR_A2A_MSP_USERID);
            }
        }

        /// <summary>
        /// Force Claimant Representative
        /// </summary>
        private void ForceClaimantRepresentative()
        {
            cmbGetNotifications.SelectedIndex = 0;
            cmbGetNotifications.Update();
        }

        /// <summary>
        /// Force Defendant Insurer
        /// </summary>
        private void ForceDefendantInsurer()
        {
            cmbGetNotifications.SelectedIndex = 1;
            cmbGetNotifications.Update();
        }

        /// <summary>
        /// Get System String
        /// </summary>
        /// <returns></returns>
        private string GetSystemString()
        {
            switch (cmbA2ASystem.SelectedItem.ToString())
            {
                case A2A_SYSTEM_TEST:
                    return RTA_URL_TEST;

                case A2A_SYSTEM_LIVE:
                    return RTA_URL_LIVE;

                default:
                    return RTA_URL_TEST;
            }
        }

        /// <summary>
        /// Get Services Stage 2
        /// </summary>
        /// <returns></returns>
        private RTAServices2 GetServicesStage2()
        {
            switch (cmbGetNotifications.SelectedItem.ToString())
            {
                case ORG_CR:
                    return CreateService<RTAServices2>(GetSystemString(), CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER, CR_A2A_MSP_USERID);

                case ORG_CM:
                    return CreateService<RTAServices2>(GetSystemString(), COMP_A2A_LOGIN_USERNAME, COMP_A2A_LOGIN_PASSWORD, COMP_A2A_LOGIN_ASUSER, COMP_A2A_MSP_USERID);

                default:
                    return CreateService<RTAServices2>(GetSystemString(), CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER, CR_A2A_MSP_USERID);
            }
        }

        /// <summary>
        /// Get Notifications List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetNotificationsList_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                dataGridViewNotifications.DataSource = null;

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                DataTable table = services.GetNotificationsList();

                if (table != null)
                {
                    dataGridViewNotifications.DataSource = table;
                }
                else
                {
                    MessageBox.Show("No Notifications Found");
                }

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Get Hospitals List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbnGetHospitalsList_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                hospital[] hospitals = services.GetHospitalsList(tbSearchHospitalName.Text, "");

                if (hospitals != null)
                {
                }
                else
                {
                    MessageBox.Show("No Hospitals Found");
                }

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Get Organisation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetOrganisation_Click(object sender, EventArgs e)
        {
            if (tbGetOrganisationPath.Text.Equals(string.Empty))
            {
                tbGetOrganisationPath.Focus();
                MessageBox.Show("Please enter an Organisation Path");
                return;
            }

            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                DataTable table = services.GetOrganisation(tbGetOrganisationPath.Text);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                DataRow row = table.Rows[0];

                if (row != null)
                {
                    tbGetOrganisationID.Text = (!string.IsNullOrEmpty(Convert.ToString(row["organisationId"]))) ? Convert.ToString(row["organisationId"]) : "";
                    tbGetOrganisationName.Text = (!string.IsNullOrEmpty(Convert.ToString(row["organisationName"]))) ? Convert.ToString(row["organisationName"]) : "";
                    tbGetOrganisationPath.Text = (!string.IsNullOrEmpty(Convert.ToString(row["organisationPath"]))) ? Convert.ToString(row["organisationPath"]) : "";
                    tbGetOrganisationHouseName.Text = (!string.IsNullOrEmpty(Convert.ToString(row["houseName"]))) ? Convert.ToString(row["houseName"]) : "";
                    tbGetOrganisationHouseNumber.Text = (!string.IsNullOrEmpty(Convert.ToString(row["houseNumber"]))) ? Convert.ToString(row["houseNumber"]) : "";
                    tbGetOrganisationStreet1.Text = (!string.IsNullOrEmpty(Convert.ToString(row["street1"]))) ? Convert.ToString(row["street1"]) : "";
                    tbGetOrganisationStreet2.Text = (!string.IsNullOrEmpty(Convert.ToString(row["street2"]))) ? Convert.ToString(row["street2"]) : "";
                    tbGetOrganisationDistrict.Text = (!string.IsNullOrEmpty(Convert.ToString(row["district"]))) ? Convert.ToString(row["district"]) : "";
                    tbGetOrganisationCity.Text = (!string.IsNullOrEmpty(Convert.ToString(row["city"]))) ? Convert.ToString(row["city"]) : "";
                    tbGetOrganisationCounty.Text = (!string.IsNullOrEmpty(Convert.ToString(row["county"]))) ? Convert.ToString(row["county"]) : "";
                    tbGetOrganisationCountry.Text = (!string.IsNullOrEmpty(Convert.ToString(row["country"]))) ? Convert.ToString(row["country"]) : "";
                    tbGetOrganisationPostCode.Text = (!string.IsNullOrEmpty(Convert.ToString(row["postCode"]))) ? Convert.ToString(row["postCode"]) : "";
                    tbGetOrganisationAddressType.Text = (!string.IsNullOrEmpty(Convert.ToString(row["addressType"]))) ? Convert.ToString(row["addressType"]) : "";
                }
                else
                {
                    MessageBox.Show("Organisation Not Found?");
                }
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Get Printable Document List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetPrintableDocumentList_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                attachment[] atts = services.GetPrintableDocumentsList(Application_Id);
                dataGridViewGetPrintableDocument.DataSource = atts;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Get Document
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetDocument_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                attachment att = services.GetPrintableDocument(tbPrintableDocumentId.Text);

                StringBuilder sb = new StringBuilder();
                sb.Append(@"C:\Temp\");
                sb.Append(att.dataAttachmentTitle);
                sb.Append(".PDF");

                SaveFile(att.dataAttachmentFileZip, sb.ToString());

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                MessageBox.Show("Saved File: " + sb.ToString());
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Get Attachments List
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetAttachments_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            if (ApplicationIDEmpty())
                return;

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                dataGridViewAttachments.DataSource = null;

                attachment[] atts = services.GetAttachmentsList(tbClaimApplicationID.Text);

                if (atts != null)
                {
                    dataGridViewAttachments.DataSource = atts;
                }
                else
                {
                    MessageBox.Show("No Attachments Found");
                }

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Get File Path
        /// </summary>
        /// <param name="fileTypeFilter"></param>
        /// <returns></returns>
        private string GetFilePath(string fileTypeFilter)
        {
            string result = "";
            openFileDialog1.InitialDirectory = "";
            openFileDialog1.Filter = fileTypeFilter;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.CheckFileExists = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                result = openFileDialog1.FileName;
            }

            return result;
        }

        /// <summary>
        /// Load Browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadBrowse_Click(object sender, EventArgs e)
        {
            tbFilePathLoad.Text = GetFilePath("");
        }

        /// <summary>
        /// Add Attachment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddAttachment_Click(object sender, EventArgs e)
        {
            if (tbFilePathLoad.Text == string.Empty)
            {
                MessageBox.Show("Please enter a file to attach");
                tbFilePathLoad.Focus();
                return;
            }

            if (tbClaimApplicationID.Text == string.Empty)
            {
                MessageBox.Show("Please enter an Application ID");
                tbClaimApplicationID.Focus();
                return;
            }

            if (File.Exists(tbFilePathLoad.Text))
            {
                RTAServices1 services = GetServices();

                try
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                    string result = services.AddAttachment(tbClaimApplicationID.Text, tbFilePathLoad.Text, "Test 1.doc", "Test 1.doc", "Test Document 1.doc");

                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                    tbAttachmentResultID.Text = result;
                }
                catch (Exception ex)
                {
                    switch (services.ErrorResponseCode)
                    {
                        case RTAServicesLibrary.PIPService.responseCode.Failure:
                            MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                            break;
                        case RTAServicesLibrary.PIPService.responseCode.Error:
                            MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                            break;
                        default:
                            MessageBox.Show(ex.Message);
                            break;
                    }
                }
            }
            else
            {
                tbFilePathLoad.Focus();
                MessageBox.Show("File not found?");
            }
        }

        /// <summary>
        /// Get Attachment
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetAttachment_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                attachment att = services.GetAttachment(tbAttachmentGuid.Text);

                if (att != null)
                {
                    string filePath = services.SaveAttachmentFileToPath(att, @"C:\Temp\");
                    MessageBox.Show("File Saved to Path:" + filePath);
                }

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Get Branches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetBranches_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                organisationInfo[] orgs = services.GetBranchesList();

                dataGridViewGetBranches.DataSource = orgs;

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Search Claims
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchClaims_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                searchClaims claim = new searchClaims();
                searchClaimCriteria critera = new searchClaimCriteria();
                claim.searchClaimCriteria = critera;
                critera.applicationId = "";
                critera.branchPath = "";
                critera.CMReferenceNumber = "";
                critera.CRReferenceNumber = "";
                critera.phaseCacheId = "";
                critera.sortField = sortField.ApplicationId;
                critera.sortFieldSpecified = true;
                critera.sortOrder = sortOrder.Asc;
                critera.sortOrderSpecified = true;
                critera.startDateFrom = DateTime.Now;
                critera.startDateFromSpecified = true;
                critera.startDateTo = DateTime.Now;
                critera.startDateToSpecified = true;


                claim[] claims = services.SearchClaims(claim);

                dataGridViewSearchClaims.DataSource = claims;

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Search Organisation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchOrganisation_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                dataGridViewSearchOrganisation.DataSource = services.SeachCompensators(tbSearchOrganisationName.Text, "");
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Send Liability Decision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendLiabilityDecision_Click(object sender, EventArgs e)
        {
            ForceDefendantInsurer();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                RTAServicesLibraryV2.InsurerResponseA2A insurer = GetLiabilityDecision();
                claimInfo info = services.SendLiabilityDecision(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, insurer);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Get Liability Decision
        /// </summary>
        /// <returns></returns>
        private RTAServicesLibraryV2.InsurerResponseA2A GetLiabilityDecision()
        {
            //  InsurerResponseA2A 
            RTAServicesLibraryV2.InsurerResponseA2A insurer = new RTAServicesLibraryV2.InsurerResponseA2A();
            insurer.Capacity = GetCapacity(cmbInsurerCapacity);
            insurer.OtherCapacity = tbInsurerOtherCapacity.Text;

            //  LiabilityCausation 
            RTAServicesLibraryV2.CT_A2A_LiabilityCausation liabilityCausation = new RTAServicesLibraryV2.CT_A2A_LiabilityCausation();
            liabilityCausation.LiabilityDecision = GetLiabilityDecision(cmbInsurerLiabilityDecision);
            liabilityCausation.NoAuthority = (RTAServicesLibraryV2.C00_YNFlag)GetYesNo(cmbInsurerNoAuthority);
            liabilityCausation.NoAuthoritySpecified = true;
            liabilityCausation.UnadmittedLiabilityReasons = tbInsurerUnadmittedLiabilityReasons.Text;

            //  DefendantAdmits 
            RTAServicesLibraryV2.CT_DefendantAdmits defendantAdmits = new RTAServicesLibraryV2.CT_DefendantAdmits();
            defendantAdmits.AccidentOccurred = (RTAServicesLibraryV2.C00_YNFlag)GetYesNo(cmbInsurerAccidentOccurred);
            defendantAdmits.AccidentOccurredSpecified = true;
            defendantAdmits.CausedByDefendant = (RTAServicesLibraryV2.C00_YNFlag)GetYesNo(cmbInsurerCausedByDefendant);
            defendantAdmits.CausedByDefendantSpecified = true;
            defendantAdmits.CausedSomeLossToTheClaimant = (RTAServicesLibraryV2.C00_YNFlag)GetYesNo(cmbInsurerCausedSomeLossToTheClaimant);
            defendantAdmits.CausedSomeLossToTheClaimantSpecified = true;
            liabilityCausation.DefendantAdmits = defendantAdmits;
            insurer.LiabilityCausation = liabilityCausation;

            //  Provided Services 
            RTAServicesLibraryV2.CT_A2A_ProvidedServices providedServices = new RTAServicesLibraryV2.CT_A2A_ProvidedServices();
            providedServices.AltVchlProvided = (RTAServicesLibraryV2.C00_YNFlag)GetYesNo(cmbInsurerAltVchlProvided);
            providedServices.AltVhclDetails = tbInsurerAltVhclDetails.Text;
            providedServices.PreparedToProvideReabilitation = (RTAServicesLibraryV2.C00_YNFlag)GetYesNo(cmbInsurerPreparedToProvideReabilitation);
            providedServices.ReabilitationDetails = tbInsurerReabilitationDetails.Text;
            providedServices.ReabilitationProvided = (RTAServicesLibraryV2.C00_YNFlag)GetYesNo(cmbInsurerReabilitationProvided);
            providedServices.RepairsProvided = (RTAServicesLibraryV2.C00_YNFlag)GetYesNo(cmbInsurerRepairsProvided);
            providedServices.RepairsDetails = tbInsurerRepairsDetails.Text;
            insurer.ProvidedServices = providedServices;

            //  Defendants Insurer 
            RTAServicesLibraryV2.CT_A2A_InsurerResponse_CompanyDetails companyDetails = new RTAServicesLibraryV2.CT_A2A_InsurerResponse_CompanyDetails();
            companyDetails.ContactMiddleName = tbDefendantInsurerContactMiddleName.Text;
            companyDetails.ContactName = tbDefendantInsurerContactName.Text;
            companyDetails.ContactSurname = tbDefendantInsurerContactSurname.Text;
            companyDetails.EmailAddress = tbDefendantInsurerEmail.Text;
            companyDetails.ReferenceNumber = tbDefendantInsurerReferenceNumber.Text;
            companyDetails.TelephoneNumber = tbDefendantInsurerTelephone.Text;
            RTAServicesLibraryV2.CT_INPUT_Address address = new RTAServicesLibraryV2.CT_INPUT_Address();
            address.AddressType = (RTAServicesLibraryV2.C01_AddressType)GetAddressType(cmbDefendantInsurerAddressType);
            //address.AddressTypeSpecified = true;
            address.City = tbDefendantInsurerCity.Text;
            address.Country = tbDefendantInsurerCountry.Text;
            address.County = "";
            address.District = "";
            address.HouseName = tbDefendantInsurerHouseName.Text;
            address.HouseNumber = tbDefendantInsurerHouseNumber.Text;
            address.PostCode = "";
            address.Street1 = tbDefendantInsurerStreet.Text;
            address.Street2 = "";
            companyDetails.Address = address;
            insurer.ProvidedServices.DefendantsInsurer = companyDetails;

            return insurer;
        }

        /// <summary>
        /// Accept Claim
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAcceptClaim_Click(object sender, EventArgs e)
        {
            ForceDefendantInsurer();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                claimInfo info = services.AcceptClaim(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Article 75 Decision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnArticle75Decision_Click(object sender, EventArgs e)
        {
            ForceDefendantInsurer();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices1 services = GetServices();

            try
            {
                bool isArticle75 = false;

                if (GetYesNo(cmbArticle75Decision) == RTAServicesLibraryV2.C00_YNFlag.Item1)
                {
                    isArticle75 = true;
                }

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                claimInfo info = services.ApplyArticle75(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, isArticle75);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Send Request Interim Settlement Pack Request Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendRequestInterimSettlementPackRequest_Click(object sender, EventArgs e)
        {
            ForceClaimantRepresentative();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServicesLibraryV2.InterimSettlementPackRequest request = new RTAServicesLibraryV2.InterimSettlementPackRequest();

            //  ClaimantRepresentative
            RTAServicesLibraryV2.InterimSettlementPackRequestClaimantRepresentative representative = new RTAServicesLibraryV2.InterimSettlementPackRequestClaimantRepresentative();
            RTAServicesLibraryV2.CT_A2A_CompanyDetails companyDetails = new RTAServicesLibraryV2.CT_A2A_CompanyDetails();
            companyDetails.ContactMiddleName = tbISPRContactMiddleName.Text;
            companyDetails.ContactName = tbISPRContactName.Text;
            companyDetails.ContactSurname = tbISPRContactSurname.Text;
            companyDetails.EmailAddress = tbISPREmailAddress.Text;
            companyDetails.ReferenceNumber = tbISPRReferenceNumber.Text;
            companyDetails.TelephoneNumber = tbISPRTelephoneNumber.Text;
            representative.CompanyDetails = companyDetails;
            request.ClaimantRepresentative = representative;

            //  InterimPayment
            RTAServicesLibraryV2.InterimSettlementPackRequestInterimPayment InterimPayment = new RTAServicesLibraryV2.InterimSettlementPackRequestInterimPayment();
            RTAServicesLibraryV2.CT_A2A_ClaimantRequestForInterimPayment requestForInterimPayment = new RTAServicesLibraryV2.CT_A2A_ClaimantRequestForInterimPayment();
            requestForInterimPayment.ReasonsForInterimPaymentRequest = tbISPRReasonsForInterimPaymentRequest.Text;
            InterimPayment.ClaimantRequestForInterimPayment = requestForInterimPayment;
            request.InterimPayment = InterimPayment;

            //  MedicalReport
            RTAServicesLibraryV2.InterimSettlementPackRequestMedicalReport report = new RTAServicesLibraryV2.InterimSettlementPackRequestMedicalReport();
            report.MedicalReportStage2_1 = DataCollection.MedicalReportValue(cmbISPRNumberOfMedicalReport.SelectedItem.ToString());
            request.MedicalReport = report;

            //  ClaimantLosses
            RTAServicesLibraryV2.CT_A2A_ClaimantLosses_Interim[] toDateList = new RTAServicesLibraryV2.CT_A2A_ClaimantLosses_Interim[1];
            RTAServicesLibraryV2.CT_A2A_ClaimantLosses_Interim toDate = new RTAServicesLibraryV2.CT_A2A_ClaimantLosses_Interim();
            toDate.LossType = (RTAServicesLibraryV2.C18_LossType)DataCollection.LossTypeValue(cmbISPRLossType.SelectedItem.ToString());
            toDate.EvidenceAttached = (RTAServicesLibraryV2.C00_YNFlag)DataCollection.YesNoValueV3(cmbISPREvidenceAttached.SelectedItem.ToString());
            toDate.Comments = tbISPRComments.Text;
            toDate.GrossValueClaimed = Convert.ToInt16(tbISPRGrossValueClaimed.Text);
            toDate.PercContribNegDeductions = Convert.ToInt16(tbISPRPercContribNegDeductions.Text);
            toDateList[0] = toDate;
            request.ClaimantLosses = toDateList;

            //  StatementOfTruth
            RTAServicesLibraryV2.InterimSettlementPackRequestStatementOfTruth statementOfTruth = new RTAServicesLibraryV2.InterimSettlementPackRequestStatementOfTruth();
            statementOfTruth.RetainedSignedCopy = RTAServicesLibraryV2.C00_YNFlag.Item1;
            statementOfTruth.SignatoryType = RTAServicesLibraryV2.C16_SignatoryType.S;
            request.StatementOfTruth = statementOfTruth;

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                claimInfo info = services.AddInterimSPFRequest(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, request);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Add Interim Settlement Pack Response Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddInterimSettlementPackResponse_Click(object sender, EventArgs e)
        {
            ForceDefendantInsurer();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServicesLibraryV2.InterimSettlementPackResponse response = new RTAServicesLibraryV2.InterimSettlementPackResponse();

            //  Defendant Insurer
            RTAServicesLibraryV2.InterimSettlementPackResponseDefendantRepresentative defendantRepresentative = new RTAServicesLibraryV2.InterimSettlementPackResponseDefendantRepresentative();
            RTAServicesLibraryV2.CT_A2A_CompanyDetails companyDetails = new RTAServicesLibraryV2.CT_A2A_CompanyDetails();
            companyDetails.ContactMiddleName = tbISPDContactMiddleName.Text;
            companyDetails.ContactName = tbISPDContactName.Text;
            companyDetails.ContactSurname = tbISPDContactSurname.Text;
            companyDetails.EmailAddress = tbISPDEmailAddress.Text;
            companyDetails.ReferenceNumber = tbISPDReferenceNumber.Text;
            companyDetails.TelephoneNumber = tbISPDTelephoneNumber.Text;
            defendantRepresentative.DefendantsInsurer = companyDetails;
            response.DefendantRepresentative = defendantRepresentative;

            //  Interim Payment
            RTAServicesLibraryV2.InterimSettlementPackResponseInterimPayment interimPayment = new RTAServicesLibraryV2.InterimSettlementPackResponseInterimPayment();
            RTAServicesLibraryV2.CT_A2A_DefendantResponseToInterimPaymentRequest paymentRequest = new RTAServicesLibraryV2.CT_A2A_DefendantResponseToInterimPaymentRequest();
            paymentRequest.AdditionalComments = tbISPDAddtionalComments.Text;
            interimPayment.DefendantResponsesToInterimPaymentRequest = paymentRequest;
            response.InterimPayment = interimPayment;

            RTAServicesLibraryV2.CT_A2A_DefendantLosses[] losses = new RTAServicesLibraryV2.CT_A2A_DefendantLosses[1];
            RTAServicesLibraryV2.CT_A2A_DefendantLosses lose = new RTAServicesLibraryV2.CT_A2A_DefendantLosses();
            lose.Comments = tbISPDComments.Text;
            lose.GrossValueOffered = Convert.ToInt16(tbISPDGrossValueClaimed.Text);
            lose.IsGrossAmountAgreed = (RTAServicesLibraryV2.C00_YNFlag)DataCollection.YesNoValueV3(cmbIsGrossAmountAgreed.SelectedItem.ToString());
            lose.LossType = (RTAServicesLibraryV2.C18_LossType_R2)DataCollection.LossTypeValue(cmbISPDLossType.SelectedItem.ToString());
            lose.PercContribNegDeductions = Convert.ToInt16(tbISPDPercContribNegDeductions.Text);
            losses[0] = lose;
            response.DefendantReplies = losses;

            RTAServicesLibraryV2.InterimSettlementPackResponseTotal total = new RTAServicesLibraryV2.InterimSettlementPackResponseTotal();
            RTAServicesLibraryV2.InterimSettlementPackResponseTotalLossesTotal lossesTotal = new RTAServicesLibraryV2.InterimSettlementPackResponseTotalLossesTotal();
            lossesTotal.CRUDeductions = 1;
            total.LossesTotal = lossesTotal;
            response.Total = total;

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                claimInfo info = services.AddInterimSPFResponse(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, response);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// SIPD Send Payment Decision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSIPDSendPaymentDecision_Click(object sender, EventArgs e)
        {
            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                bool isInterimPaymentNeeded = false;

                if (GetYesNo(cmbSIPNAIsInterimPaymentNeeded) == RTAServicesLibraryV2.C00_YNFlag.Item1)
                {
                    isInterimPaymentNeeded = true;
                }

                claimInfo info = services.SetInterimPaymentNeeded(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, isInterimPaymentNeeded);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Stage 2.1 Paid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSIPNStageTwoPaid_Click(object sender, EventArgs e)
        {
            ForceClaimantRepresentative();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                bool isStage21Paid = false;

                if (GetYesNo(cmbSIPNAIsInterimPaymentNeeded) == RTAServicesLibraryV2.C00_YNFlag.Item1)
                {
                    isStage21Paid = true;
                }

                claimInfo info = services.SetInterimPaymentNeeded(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, isStage21Paid);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Is Partial Interim Payment Accepted
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSIPNIsPartialInterimPaymentAccepted_Click(object sender, EventArgs e)
        {
            ForceClaimantRepresentative();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                bool isPartialInterimPaymentAccepted = true;

                if (GetYesNo(cmbSIPNIsPartialInterimPaymentAccepted) == RTAServicesLibraryV2.C00_YNFlag.Item1)
                {
                    isPartialInterimPaymentAccepted = true;
                }

                claimInfo info = services.AcceptPartialInterimPayment(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, isPartialInterimPaymentAccepted);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                switch (services.ErrorResponseCode)
                {
                    case RTAServicesLibrary.PIPService.responseCode.Failure:
                        MessageBox.Show("Failure:" + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    case RTAServicesLibrary.PIPService.responseCode.Error:
                        MessageBox.Show("Error: " + services.ErrorMessage + " Trace: " + services.ErrorTrace);
                        break;
                    default:
                        MessageBox.Show(ex.Message);
                        break;
                }
            }
        }

        /// <summary>
        /// Acknowledge Partial Payment Decision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSIPNSendAcknowledgement_Click(object sender, EventArgs e)
        {
            ForceDefendantInsurer();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                claimInfo info = services.AcknowledgePartialPaymentDecision(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Fraud Stated
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFraudStated_Click(object sender, EventArgs e)
        {
            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                claimInfo info = services.StateFraud(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, tbFraudReasonCode.Text, tbFraudReasonDescription.Text);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Stage One Payment Received
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStageOnePaymentReceived_Click(object sender, EventArgs e)
        {
            ForceClaimantRepresentative();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                bool isStage1Paid = false;

                if (GetYesNo(cmbStageOnePaymentReceived) == RTAServicesLibraryV2.C00_YNFlag.Item1)
                {
                    isStage1Paid = true;
                }

                claimInfo info = services.SetStage1Payment(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, isStage1Paid);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Get Stage 2 Claim Status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetStage2ClaimStatus_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                claimInfo info = services.GetClaimsStatus(tbClaimApplicationID.Text);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRemoveNotes_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                services.RemoveNotification(tbNotes.Text);

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Set Stage 21 Payments Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetStage21Payments_Click(object sender, EventArgs e)
        {
            ForceClaimantRepresentative();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                bool isStage21Paid = false;

                if (GetYesNo(cmbSIPNStageTwoPaid) == RTAServicesLibraryV2.C00_YNFlag.Item1)
                {
                    isStage21Paid = true;
                }

                claimInfo info = services.SetStage21Payments(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, isStage21Paid);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Accept Partial Interim Payment Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAcceptPartialInterimPayment_Click(object sender, EventArgs e)
        {
            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                bool isPartialInterimPaymentAccepted = false;

                if (GetYesNo(cmbAcceptPartialInterimPayment) == RTAServicesLibraryV2.C00_YNFlag.Item1)
                {
                    isPartialInterimPaymentAccepted = true;
                }

                claimInfo info = services.AcceptPartialInterimPayment(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, isPartialInterimPaymentAccepted);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Validate National Insurance Number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNationalInsuranceNumber_Click(object sender, EventArgs e)
        {
            if (StaticFunctions.NationalInsuraneNumberValidation(tbNationalInsuranceNumber.Text))
            {
                lblNationalInsuranceNumber.Text = "Valid National Insurance";
            }
            else
            {
                lblNationalInsuranceNumber.Text = "Invalid National Insurance";
            }
        }

        /// <summary>
        /// Validate Reference Number Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValidateReferenceNumber_Click(object sender, EventArgs e)
        {
            if (StaticFunctions.ReferenceNumberValidation(tbReferenceNumber.Text))
            {
                lblReferenceNumber.Text = "Valid Reference Number";
            }
            else
            {
                lblReferenceNumber.Text = "Invalid Reference Number";
            }
        }

        /// <summary>
        /// Validate PostCode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValidatePostCode_Click(object sender, EventArgs e)
        {
            if (StaticFunctions.PostCodeValidation(tbPostCodeTest.Text))
            {
                lblPostCodeTest.Text = "Valid PostCode";
            }
            else
            {
                lblPostCodeTest.Text = "Invalid PostCode";
            }
        }

        /// <summary>
        /// Validate Email Address Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnValidateEmailAddress_Click(object sender, EventArgs e)
        {
            if (StaticFunctions.EmailAddressValidation(tbValdateEmailAddress.Text))
            {
                lblEmailAddressValidation.Text = "Valid Email Address";
            }
            else
            {
                lblEmailAddressValidation.Text = "Invalid Email Address";
            }
        }

        /// <summary>
        /// Vehicle Registration Validation Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVehicleRegistrationValidation_Click(object sender, EventArgs e)
        {
            if (StaticFunctions.VehicleRegistrationValidation(tbVehicleRegistrationValidation.Text))
            {
                lblVehicleRegistrationValdaition.Text = "Valid Vehicle Registration";
            }
            else
            {
                lblVehicleRegistrationValdaition.Text = "Invalid Vehicle Registration";
            }
        }

        /// <summary>
        /// Load CNF XML File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchLoadCNFXMLFile_Click(object sender, EventArgs e)
        {
            tbLoadCNFXMLFile.Text = GetFilePath(xmlFilter);
        }

        /// <summary>
        /// Send CNF XML File
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendLoadCNFXMLFile_Click(object sender, EventArgs e)
        {
            RTAServicesLibraryV2.DocumentInput doc = GetClaimantCNF();
            RTAServices1 services = CreateService<RTAServices1>(GetSystemString(), CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER, CR_A2A_MSP_USERID);

            if (tbLoadCNFXMLFile.Text == string.Empty)
            {
                MessageBox.Show("Please load an XML file");
                tbLoadCNFXMLFile.Focus();
                return;
            }

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                XmlTextReader reader = new XmlTextReader(tbLoadCNFXMLFile.Text);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);

                string xml = xmlDoc.InnerXml;

                claimInfo info = services.AddXMLClaim(xml);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReassignToAnotherCM_Click(object sender, EventArgs e)
        {
            RTAServices1 services = GetServices();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            if (tbReassignToAnotherCM.Text == string.Empty)
            {
                MessageBox.Show("Please enter a CM Path to reassign claim");
                tbReassignToAnotherCM.Focus();
                return;
            }

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                claimInfo info = services.ReassignToAnotherCM(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, tbReassignToAnotherCM.Text);
                DisplayClaimData(info);

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        /// <summary>
        /// Add Stage 2 SPF Request
        /// </summary>
        private void btnAddStage2SPFRequest(object sender, EventArgs e)
        {
            ForceClaimantRepresentative();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServicesLibraryV2.Stage2SettlementPackRequest pack = null;
            RTAServicesLibraryV3.Stage2SettlementPackRequest packV3 = null;

            RTAServices1 RTA = CreateService<RTAServices1>(GetSystemString(), CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER, CR_A2A_MSP_USERID);
            switch (RTA.GetSystemProcessVersionReleaseCode(RTA.GetSystemProcessVersion(tbClaimApplicationID.Text, RTA)))
            {
                case "R2":
                    pack = new RTAServicesLibraryV2.Stage2SettlementPackRequest();

                    //  Agreement Data
                    RTAServicesLibraryV2.Stage2SettlementPackRequestAgreementData agreementData = new RTAServicesLibraryV2.Stage2SettlementPackRequestAgreementData();
                    agreementData.Comments = tbSPRAgreementComments.Text;
                    pack.AgreementData = agreementData;

                    //  Claimant Losses
                    RTAServicesLibraryV2.CT_A2A_ClaimantLosses[] claimantLosses = new RTAServicesLibraryV2.CT_A2A_ClaimantLosses[1];
                    RTAServicesLibraryV2.CT_A2A_ClaimantLosses losses = new RTAServicesLibraryV2.CT_A2A_ClaimantLosses();
                    losses.Comments = tbSPRLossesComments.Text;
                    losses.EvidenceAttached = (RTAServicesLibraryV2.C00_YNFlag)DataCollection.YesNoValueV3(cmbSPREvidenceAttached.SelectedItem.ToString());
                    losses.GrossValueClaimed = Convert.ToInt32(tbSPRGrossValueClaimed.Text);
                    losses.LossType = (RTAServicesLibraryV2.C18_LossType_R2)DataCollection.LossTypeValue(cmbSPRLossType.SelectedItem.ToString());
                    losses.PercContribNegDeductions = Convert.ToInt32(tbSPRPercContribHegDeductions.Text);
                    claimantLosses[0] = losses;
                    pack.ClaimantLosses = claimantLosses;

                    //  Claimant Representative
                    RTAServicesLibraryV2.Stage2SettlementPackRequestClaimantRepresentative claimantRepresentative = new RTAServicesLibraryV2.Stage2SettlementPackRequestClaimantRepresentative();
                    claimantRepresentative.ContactMiddleName = "";
                    claimantRepresentative.ContactName = tbSPRContactName.Text;
                    claimantRepresentative.ContactSurname = tbSPRContactSurname.Text;
                    claimantRepresentative.EmailAddress = "";
                    claimantRepresentative.ReferenceNumber = tbSPRReferenceNumber.Text;
                    claimantRepresentative.TelephoneNumber = "01604 273839";
                    pack.ClaimantRepresentative = claimantRepresentative;

                    //  Medical Report
                    RTAServicesLibraryV2.Stage2SettlementPackRequestMedicalReport medicalReport = new RTAServicesLibraryV2.Stage2SettlementPackRequestMedicalReport();
                    RTAServicesLibraryV2.C22_MedicalReport report = new RTAServicesLibraryV2.C22_MedicalReport();
                    report = (RTAServicesLibraryV2.C22_MedicalReport)DataCollection.MedicalReportValue(cmbMedicalReportInculded.SelectedItem.ToString());
                    medicalReport.MedicalReportStage2 = report;
                    pack.MedicalReport = medicalReport;

                    //  Statement Of Truth
                    RTAServicesLibraryV2.Stage2SettlementPackRequestStatementOfTruth truth = new RTAServicesLibraryV2.Stage2SettlementPackRequestStatementOfTruth();
                    truth.RetainedSignedCopy = (RTAServicesLibraryV2.C00_YNFlag)DataCollection.YesNoValue(cmbSPRRetainedCopy.SelectedItem.ToString());
                    truth.SignatoryType = (RTAServicesLibraryV2.C16_SignatoryType)DataCollection.SignatoryTypeValueV3(cmbSPRSignatory.SelectedItem.ToString());
                    pack.StatementOfTruth = truth;
                    break;


                case "R3":
                    packV3 = new RTAServicesLibraryV3.Stage2SettlementPackRequest();

                    //  Agreement Data
                    RTAServicesLibraryV3.Stage2SettlementPackRequestAgreementData agreementDataV3 = new RTAServicesLibraryV3.Stage2SettlementPackRequestAgreementData();
                    agreementDataV3.Comments = tbSPRAgreementComments.Text;
                    packV3.AgreementData = agreementDataV3;

                    //  Claimant Losses
                    RTAServicesLibraryV3.CT_A2A_ClaimantLosses[] claimantLossesV3 = new RTAServicesLibraryV3.CT_A2A_ClaimantLosses[1];
                    RTAServicesLibraryV3.CT_A2A_ClaimantLosses lossesV3 = new RTAServicesLibraryV3.CT_A2A_ClaimantLosses();
                    lossesV3.Comments = tbSPRLossesComments.Text;
                    lossesV3.EvidenceAttached = (RTAServicesLibraryV3.C00_YNFlag)DataCollection.YesNoValueV3(cmbSPREvidenceAttached.SelectedItem.ToString());
                    lossesV3.GrossValueClaimed = Convert.ToInt32(tbSPRGrossValueClaimed.Text);
                    lossesV3.LossType = (RTAServicesLibraryV3.C18_LossType_A2A_R3)DataCollection.LossTypeValue(cmbSPRLossType.SelectedItem.ToString());
                    lossesV3.PercContribNegDeductions = Convert.ToInt32(tbSPRPercContribHegDeductions.Text);
                    claimantLossesV3[0] = lossesV3;
                    packV3.ClaimantLosses = claimantLossesV3;

                    //  Claimant Representative
                    RTAServicesLibraryV3.Stage2SettlementPackRequestClaimantRepresentative claimantRepresentativeV3 = new RTAServicesLibraryV3.Stage2SettlementPackRequestClaimantRepresentative();
                    claimantRepresentativeV3.ContactMiddleName = "";
                    claimantRepresentativeV3.ContactName = tbSPRContactName.Text;
                    claimantRepresentativeV3.ContactSurname = tbSPRContactSurname.Text;
                    claimantRepresentativeV3.EmailAddress = "";
                    claimantRepresentativeV3.ReferenceNumber = tbSPRReferenceNumber.Text;
                    claimantRepresentativeV3.TelephoneNumber = "01604 273839";
                    packV3.ClaimantRepresentative = claimantRepresentativeV3;

                    //  Medical Report
                    RTAServicesLibraryV3.Stage2SettlementPackRequestMedicalReport medicalReportV3 = new RTAServicesLibraryV3.Stage2SettlementPackRequestMedicalReport();
                    RTAServicesLibraryV3.C22_MedicalReport reportV3 = new RTAServicesLibraryV3.C22_MedicalReport();
                    reportV3 = (RTAServicesLibraryV3.C22_MedicalReport)DataCollection.MedicalReportValue(cmbMedicalReportInculded.SelectedItem.ToString());
                    medicalReportV3.MedicalReportStage2 = reportV3;
                    packV3.MedicalReport = medicalReportV3;

                    //  Statement Of Truth
                    RTAServicesLibraryV3.Stage2SettlementPackRequestStatementOfTruth truthV3 = new RTAServicesLibraryV3.Stage2SettlementPackRequestStatementOfTruth();
                    truthV3.RetainedSignedCopy = (RTAServicesLibraryV3.C00_YNFlag)DataCollection.YesNoValue(cmbSPRRetainedCopy.SelectedItem.ToString());
                    truthV3.SignatoryType = (RTAServicesLibraryV3.C16_SignatoryType)DataCollection.SignatoryTypeValueV3(cmbSPRSignatory.SelectedItem.ToString());
                    packV3.StatementOfTruth = truthV3;
                    break;

            }

         

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                claimInfo info = services.AddStage2SPFRequest(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, pack);

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private RTAServicesLibraryV3.Stage2SettlementPackRequest getStage2SettlementPackV3()
        {
            RTAServicesLibraryV3.Stage2SettlementPackRequest pack = new RTAServicesLibraryV3.Stage2SettlementPackRequest();

            //  Agreement Data
            RTAServicesLibraryV3.Stage2SettlementPackRequestAgreementData agreementData = new RTAServicesLibraryV3.Stage2SettlementPackRequestAgreementData();
            agreementData.Comments = tbSPRAgreementComments.Text;
            pack.AgreementData = agreementData;

            //  Claimant Losses
            RTAServicesLibraryV3.CT_A2A_ClaimantLosses[] claimantLosses = new RTAServicesLibraryV3.CT_A2A_ClaimantLosses[1];
            RTAServicesLibraryV3.CT_A2A_ClaimantLosses losses = new RTAServicesLibraryV3.CT_A2A_ClaimantLosses();
            losses.Comments = tbSPRLossesComments.Text;
            losses.EvidenceAttached = (RTAServicesLibraryV3.C00_YNFlag)DataCollection.YesNoValueV3(cmbSPREvidenceAttached.SelectedItem.ToString());
            losses.GrossValueClaimed = Convert.ToInt32(tbSPRGrossValueClaimed.Text);
            losses.LossType = (RTAServicesLibraryV3.C18_LossType_A2A_R3)DataCollection.LossTypeValue(cmbSPRLossType.SelectedItem.ToString());
            losses.PercContribNegDeductions = Convert.ToInt32(tbSPRPercContribHegDeductions.Text);
            claimantLosses[0] = losses;
            pack.ClaimantLosses = claimantLosses;

            //  Claimant Representative
            RTAServicesLibraryV3.Stage2SettlementPackRequestClaimantRepresentative claimantRepresentative = new RTAServicesLibraryV3.Stage2SettlementPackRequestClaimantRepresentative();
            claimantRepresentative.ContactMiddleName = "";
            claimantRepresentative.ContactName = tbSPRContactName.Text;
            claimantRepresentative.ContactSurname = tbSPRContactSurname.Text;
            claimantRepresentative.EmailAddress = "";
            claimantRepresentative.ReferenceNumber = tbSPRReferenceNumber.Text;
            claimantRepresentative.TelephoneNumber = "01604 273839";
            pack.ClaimantRepresentative = claimantRepresentative;

            //  Medical Report
            RTAServicesLibraryV3.Stage2SettlementPackRequestMedicalReport medicalReport = new RTAServicesLibraryV3.Stage2SettlementPackRequestMedicalReport();
            RTAServicesLibraryV3.C22_MedicalReport report = new RTAServicesLibraryV3.C22_MedicalReport();
            report = (RTAServicesLibraryV3.C22_MedicalReport)DataCollection.MedicalReportValue(cmbMedicalReportInculded.SelectedItem.ToString());
            medicalReport.MedicalReportStage2 = report;
            pack.MedicalReport = medicalReport;

            //  Statement Of Truth
            RTAServicesLibraryV3.Stage2SettlementPackRequestStatementOfTruth truth = new RTAServicesLibraryV3.Stage2SettlementPackRequestStatementOfTruth();
            truth.RetainedSignedCopy = (RTAServicesLibraryV3.C00_YNFlag)DataCollection.YesNoValue(cmbSPRRetainedCopy.SelectedItem.ToString());
            truth.SignatoryType = (RTAServicesLibraryV3.C16_SignatoryType)DataCollection.SignatoryTypeValueV3(cmbSPRSignatory.SelectedItem.ToString());
            pack.StatementOfTruth = truth;
            return pack;
        }


        /// <summary>
        /// Add Stage 2 SPF Counter Offer By CR
        /// </summary>
        private void btnAddStage2SPFCounterOfferByCR()
        {
            RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCR offer = new RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCR();

            //  Agreement Data
            RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCRAgreementData data = new RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCRAgreementData();
            RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCRAgreementDataFinalAgreementDetails finalDetails = new RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCRAgreementDataFinalAgreementDetails();
            RTAServicesLibraryV2.CT_A2A_AgreementDetails agreement = new RTAServicesLibraryV2.CT_A2A_AgreementDetails();
            agreement.Comments = "";
            agreement.GrossAmount = 1;
            agreement.InterimPaymentAmount = 1;
            finalDetails.AgreementDetails = agreement;
            data.FinalAgreementDetails = finalDetails;
            offer.AgreementData = data;

            //  Claimant Losses
            RTAServicesLibraryV2.CT_A2A_ClaimantLosses[] claimantLosses = new RTAServicesLibraryV2.CT_A2A_ClaimantLosses[1];
            RTAServicesLibraryV2.CT_A2A_ClaimantLosses losses = new RTAServicesLibraryV2.CT_A2A_ClaimantLosses();
            losses.Comments = "";
            losses.EvidenceAttached = RTAServicesLibraryV2.C00_YNFlag.Item0;
            losses.GrossValueClaimed = 1;
            losses.LossType = RTAServicesLibraryV2.C18_LossType_R2.Item0;
            losses.PercContribNegDeductions = 1;
            claimantLosses[0] = losses;
            offer.ClaimantLosses = claimantLosses;

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;

                claimInfo info = services.AddStage2SPFCounterOfferByCR(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, offer);

                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        /// <summary>
        /// Add CPPF Request
        /// </summary>
        private void btnAddCPPFRequest()
        {
            MessageBox.Show("Not yet working. Still to be implemented.");
        }

        /// <summary>
        /// Claim Status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbClaimStatus_Click(object sender, EventArgs e)
        {
            if (ApplicationIDEmpty())
                return;

            RTAServices1 services = CreateService<RTAServices1>(GetSystemString(), CR_A2A_LOGIN_USERNAME, CR_A2A_LOGIN_PASSWORD, CR_A2A_LOGIN_ASUSER, CR_A2A_MSP_USERID);

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                claimInfo info = services.GetClaimsStatus(tbClaimApplicationID.Text);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Settlement Pack Decision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSPDSettlementPackDecision_Click(object sender, EventArgs e)
        {
            ForceDefendantInsurer();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;



            RTAServicesLibraryV2.Stage2SettlementPackResponse response = new RTAServicesLibraryV2.Stage2SettlementPackResponse();


            //  Agreement Data
            RTAServicesLibraryV2.Stage2SettlementPackResponseAgreementData agreement = new RTAServicesLibraryV2.Stage2SettlementPackResponseAgreementData();
            RTAServicesLibraryV2.CT_A2A_FinalAgreementDetails final = new RTAServicesLibraryV2.CT_A2A_FinalAgreementDetails();
            RTAServicesLibraryV2.CT_A2A_AgreementDetails details = new RTAServicesLibraryV2.CT_A2A_AgreementDetails();
            details.Comments = tbSPDAgreementComments.Text;
            details.GrossAmount = Convert.ToInt32(tbSPDGrossAmount.Text);
            details.InterimPaymentAmount = Convert.ToInt32(tbSPDInterimPaymentAmount.Text);
            final.AgreementDetails = details;
            final.SettlementPackDecision = (RTAServicesLibraryV2.C20_SettlementPackDecision)DataCollection.SettlementPackDecisionValue(cmbSPDSettlementDecision.SelectedItem.ToString());
            agreement.FinalAgreementDetails = final;
            response.AgreementData = agreement;

            //  Defendant Replies
            RTAServicesLibraryV2.CT_A2A_CurrentDefendantResponse[] defendantResponse = new RTAServicesLibraryV2.CT_A2A_CurrentDefendantResponse[1];
            RTAServicesLibraryV2.CT_A2A_CurrentDefendantResponse currentResponse = new RTAServicesLibraryV2.CT_A2A_CurrentDefendantResponse();
            currentResponse.Comments = tbSPDDefendantRepliesComments.Text;
            currentResponse.GrossValueClaimed = Convert.ToInt32(tbSPDDefendantRepliesGrossValueClaimed.Text);
            currentResponse.IsGrossAmountAgreed = (RTAServicesLibraryV2.C00_YNFlag)DataCollection.YesNoValueV3(cmbSPDIsGrossAmountAgreed.SelectedItem.ToString());
            currentResponse.LossType = (RTAServicesLibraryV2.C18_LossType_R2)DataCollection.LossTypeValue(cmbSPDLossType.SelectedItem.ToString());
            currentResponse.PercContribNegDeductions = Convert.ToInt32(tbSPDPercContribNegDeductions.Text);
            defendantResponse[0] = currentResponse;
            response.DefendantReplies = defendantResponse;

            //  Defendant Representative
            RTAServicesLibraryV2.Stage2SettlementPackResponseDefendantRepresentative representative = new RTAServicesLibraryV2.Stage2SettlementPackResponseDefendantRepresentative();
            RTAServicesLibraryV2.CT_A2A_DefendantsInsurer insurer = new RTAServicesLibraryV2.CT_A2A_DefendantsInsurer();
            insurer.ContactMiddleName = tbSPDRepresentativeMiddlename.Text;
            insurer.ContactName = tbSPDRepresentativeName.Text;
            insurer.ContactSurname = tbSPDRepresentativeSurname.Text;
            insurer.EmailAddress = tbSPDRepresentativeEmail.Text;
            insurer.ReferenceNumber = tbSPDRepresentativeReferenceNumber.Text;
            insurer.TelephoneNumber = tbSPDRepresentativeTelephoneNumber.Text;
            representative.DefendantsInsurer = insurer;
            response.DefendantRepresentative = representative;

            //  Total
            RTAServicesLibraryV2.Stage2SettlementPackResponseTotal total = new RTAServicesLibraryV2.Stage2SettlementPackResponseTotal();
            RTAServicesLibraryV2.CT_A2A_CurrentTotal currentTotal = new RTAServicesLibraryV2.CT_A2A_CurrentTotal();
            currentTotal.CRUDeductions = Convert.ToInt32(tbSPDTotalCurrentTotal.Text);
            total.CurrentTotal = currentTotal;
            response.Total = total;


            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                claimInfo info = services.AddStage2SPFResponse(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, response);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Send Counter Offer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSPCOSendCounterOffer_Click(object sender, EventArgs e)
        {
            ForceClaimantRepresentative();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCR offer = new RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCR();

            //  Agreement Data
            RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCRAgreementData agreement = new RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCRAgreementData();
            RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCRAgreementDataFinalAgreementDetails finalAgreement = new RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCRAgreementDataFinalAgreementDetails();
            RTAServicesLibraryV2.CT_A2A_AgreementDetails details = new RTAServicesLibraryV2.CT_A2A_AgreementDetails();
            details.Comments = tbSPCOAgreementComments.Text;
            details.GrossAmount = Convert.ToInt32(tbSPCOAgreementGrossAmount.Text);
            details.InterimPaymentAmount = Convert.ToInt32(tbSPCOAgreementInterimPaymentAmount.Text);
            finalAgreement.AgreementDetails = details;
            agreement.FinalAgreementDetails = finalAgreement;
            offer.AgreementData = agreement;

            //  Claimant Losses
            RTAServicesLibraryV2.CT_A2A_ClaimantLosses[] losses = new RTAServicesLibraryV2.CT_A2A_ClaimantLosses[1];
            RTAServicesLibraryV2.CT_A2A_ClaimantLosses loss = new RTAServicesLibraryV2.CT_A2A_ClaimantLosses();
            loss.Comments = tbSPCOLossesComments.Text;
            loss.EvidenceAttached = (RTAServicesLibraryV2.C00_YNFlag)DataCollection.YesNoValueV3(cmbSPCOLossesEvidenceAttached.SelectedItem.ToString());
            loss.GrossValueClaimed = Convert.ToInt32(tbSPCOLossesGrossValueClaimed.Text);
            loss.LossType = (RTAServicesLibraryV2.C18_LossType_R2)DataCollection.LossTypeValue(cmbSPCOLossesLossType.SelectedItem.ToString());
            loss.PercContribNegDeductions = Convert.ToInt32(tbSPCOLossesPercContribNegDeductions.Text);
            losses[0] = loss;
            offer.ClaimantLosses = losses;

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                claimInfo info = services.AddStage2SPFCounterOfferByCR(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, offer);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Counter Offer Decision
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCounterOfferDecision_Click(object sender, EventArgs e)
        {
            ForceDefendantInsurer();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCM offer = new RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCM();

            //  Agreement Data
            RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCMAgreementData agreement = new RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCMAgreementData();
            RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCMAgreementDataFinalAgreementDetails finalAgreement = new RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCMAgreementDataFinalAgreementDetails();
            RTAServicesLibraryV2.CT_A2A_AgreementDetails details = new RTAServicesLibraryV2.CT_A2A_AgreementDetails();
            details.Comments = tbSPCODAgreementComments.Text;
            details.GrossAmount = Convert.ToInt32(tbSPCODAgreementGrossAmount.Text);
            details.InterimPaymentAmount = Convert.ToInt32(tbSPCODAgreementsInterimPaymentAmount.Text);
            finalAgreement.AgreementDetails = details;
            agreement.FinalAgreementDetails = finalAgreement;
            offer.AgreementData = agreement;

            //  Defendant Replies
            RTAServicesLibraryV2.CT_A2A_DefendantLosses[] losses = new RTAServicesLibraryV2.CT_A2A_DefendantLosses[1];
            RTAServicesLibraryV2.CT_A2A_DefendantLosses loss = new RTAServicesLibraryV2.CT_A2A_DefendantLosses();
            loss.Comments = tbSPCODLossesComments.Text;
            loss.GrossValueOffered = Convert.ToInt32(tbSPCODLossesGrossValueOffered.Text);
            loss.IsGrossAmountAgreed = (RTAServicesLibraryV2.C00_YNFlag)DataCollection.YesNoValueV3(cmbSPCODLossesIsGrossAmountAgreed.SelectedItem.ToString());
            loss.LossType = (RTAServicesLibraryV2.C18_LossType_R2)DataCollection.LossTypeValue(cmbSPCODLossesLossType.SelectedItem.ToString());
            loss.PercContribNegDeductions = Convert.ToInt32(tbSPCODLossesPercContribNegDeductions.Text);
            losses[0] = loss;
            offer.DefendantReplies = losses;

            //  Total
            RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCMTotal total = new RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCMTotal();
            RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCMTotalCurrentTotal currentTotal = new RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCMTotalCurrentTotal();
            currentTotal.CRUDeductions = Convert.ToInt32(tbSPCODTotalDeductions.Text);
            total.CurrentTotal = currentTotal;
            offer.Total = total;


            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                claimInfo info = services.AddStage2SPFCounterOfferByCM(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, offer);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Settlement Pack Agreed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSettlementPackAgreed_Click(object sender, EventArgs e)
        {
            ForceClaimantRepresentative();

            if (ActivityEngineGuidEmpty())
                return;

            if (ApplicationIDEmpty())
                return;

            RTAServices2 services = GetServicesStage2();

            bool isSettlementPackAgreed = false;

            if (GetYesNo(cmbSPAAgreement) == RTAServicesLibraryV2.C00_YNFlag.Item1)
            {
                isSettlementPackAgreed = true;
            }

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                claimInfo info = services.SetStage2SPFAgreementDecision(tbClaimActivityEngineGuid.Text, tbClaimApplicationID.Text, isSettlementPackAgreed);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                DisplayClaimData(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ApplicationIDEmpty())
                return;

            RTAServices1 services = GetServices();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                string info = services.GetClaim(tbClaimApplicationID.Text);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                tbGetCNFXML.Text = info;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Get Interim Settlement Pack
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetInterimSettlementPack_Click(object sender, EventArgs e)
        {
            if (ApplicationIDEmpty())
                return;

            RTAServices2 services = GetServicesStage2();

            try
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                RTAServicesLibraryV2.InterimSettlementPack data = services.GetClaimInterimSettlementPack(tbClaimApplicationID.Text);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Button Get Version Clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetVersion_Click(object sender, EventArgs e)
        {
            string versionStamp = "";
            string releaseNumber = "";

            try
            {
                Cursor = Cursors.WaitCursor;

                RTAServices1 services = CreateService<RTAServices1>(txtURL.Text, txtUsername.Text, txtPassword.Text, txtAsUser.Text, null);
                versionStamp = services.GetSystemProcessVersion(tbClaimApplicationID.Text, services);
                releaseNumber = services.GetSystemProcessVersionReleaseCode(versionStamp);
                tbVersion.Text = string.Format("{0} ({1})", releaseNumber, versionStamp);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                Cursor = Cursors.Default;
            }
        }

    }
}
