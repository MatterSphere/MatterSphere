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
    /// Summary description for Stage1Test
    /// </summary>
    [TestClass]
    public class Stage1Test
    {
        #region Private Data

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

        #endregion Private Data

        public Stage1Test()
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
        public void AddClaim()
        {
            RTAServices1 services = GetServicesAsClaimant();

            DocumentInput doc = GetClaimantCNF();
        }

        [TestMethod]
        public void GetClaim()
        {
            Assert.AreNotEqual(null, ApplicationID, "Application ID not set");

            RTAServices1 services = GetServicesAsDefendant();
            string claimString = services.GetClaim(ApplicationID);
            Assert.AreNotEqual(null, claimString, "Get Claim return null");
            Assert.AreNotEqual(string.Empty, claimString, "Get Claim return an empty string");
        }

        [TestMethod]
        public void AcceptClaim()
        {
            Assert.AreNotEqual(null, ApplicationID, "Application ID not set");

            RTAServices1 services = GetServicesAsDefendant();
            claimInfo info = services.AcceptClaim(ActivityEngineGuid, ApplicationID);
            Assert.AreNotEqual(null, info, "Accept Claim return null");
            Assert.AreEqual(Application_Id, info.applicationId, "AcceptClaim returned Application ID not valid");
            Assert.AreEqual(PHASECACHEID_ARTICLE75DECISION, info.phaseCacheId, "Phase Cache ID not equal to Article 75 Decision");
        }

        [TestMethod]
        public void ApplyArticle75()
        {
            Assert.AreNotEqual(null, ApplicationID, "Application ID not set");

            RTAServices1 services = GetServicesAsDefendant();
            bool isArticle75 = false;

            claimInfo info = services.ApplyArticle75(ActivityEngineGuid, ApplicationID, isArticle75);
            Assert.AreNotEqual(null, info, "Apply Article 75 return null");
            Assert.AreEqual(Application_Id, info.applicationId, "ApplyArticle75 returned Application ID not valid");
            Assert.AreEqual(PHASECACHEID_LIABILITYDECISION, info.phaseCacheId, "Phase Cache ID not equal to Liability Decision");
        }

        [TestMethod]
        public void SendLiabilityDesision()
        {
            Assert.AreNotEqual(null, ApplicationID, "Application ID not set");

            RTAServices1 services = GetServicesAsDefendant();
            RTAServicesLibrary.InsurerResponseA2A insurer = GetLiabilityDecision();

            claimInfo info = services.SendLiabilityDesision(ActivityEngineGuid, ApplicationID, insurer);
            Assert.AreNotEqual(null, info, "Send Liability Desision return null");
            Assert.AreNotEqual(0, info.applicationId, "SendLiabilityDesision returned Application ID not valid");
            Assert.AreEqual(PHASECACHEID_LIABILITYADMITTED, info.phaseCacheId, "Phase Cache ID not equal to Liability Admitted");
        }

        /// <summary>
        /// Get Claimant CNF
        /// </summary>
        /// <returns></returns>
        private DocumentInput GetClaimantCNF()
        {
            //  This part of the CNF is completed by the Claimants Representative
            DocumentInput doc = new DocumentInput();

            //  ApplicationData
            DocumentInputApplicationData appData = new DocumentInputApplicationData();
            CT_INPUT_Claim_Details claimDetails = new CT_INPUT_Claim_Details();
            claimDetails.RetainedCopy = C00_YNFlag.Item1;
            claimDetails.Signatory = C16_SignatoryType.C;
            appData.ClaimDetails = claimDetails;
            doc.ApplicationData = appData;

            //  ClaimAndClaimantDetails - ClaimantRepresentative
            DocumentInputClaimAndClaimantDetails claimAndClaimantDetails = new DocumentInputClaimAndClaimantDetails();
            DocumentInputClaimAndClaimantDetailsClaimantRepresentative representative = new DocumentInputClaimAndClaimantDetailsClaimantRepresentative();
            CT_INPUT_ClaimantRepresentative_CompanyDetails companyDetails = new CT_INPUT_ClaimantRepresentative_CompanyDetails();
            companyDetails.CompanyName = "FWBS";
            companyDetails.ContactName = "Alan";
            companyDetails.ContactSurname = "Greaves";
            companyDetails.ReferenceNumber = "12345678";
            companyDetails.TelephoneNumber = "01604 372828";

            CT_INPUT_Address crAddress = new CT_INPUT_Address();
            crAddress.Street1 = "12 Gold Street";
            crAddress.City = "Northampton";
            crAddress.Country = "England";
            crAddress.PostCode = "NN1 6AF";
            crAddress.AddressType = C01_AddressType.A;
            crAddress.HouseName = "A House";

            companyDetails.Address = crAddress;
            representative.CompanyDetails = companyDetails;
            claimAndClaimantDetails.ClaimantRepresentative = representative;

            //  DefendantDetails 
            CT_INPUT_DefendantDetails defendantDetails = new CT_INPUT_DefendantDetails();
            defendantDetails.DefedantAge = "22";
            defendantDetails.DefendandDescription = "Six feet male";
            defendantDetails.DefendantDetailsObtained = "From Defendant";
            defendantDetails.DefendantStatus = C09_SubjectStatus.P;
            defendantDetails.PolicyNumberReference = "12345678";

            CT_INPUT_Defendant_PersonalDetails defendantPersonal = new CT_INPUT_Defendant_PersonalDetails();
            defendantPersonal.Sex = C08_Sex.M;
            defendantPersonal.SexSpecified = true;
            defendantPersonal.Surname = "Thomas";

            CT_INPUT_Address personalAddress = new CT_INPUT_Address();
            personalAddress.Street1 = "11 High Street";
            personalAddress.City = "Northampton";
            personalAddress.Country = "England";
            personalAddress.AddressType = C01_AddressType.A;
            personalAddress.HouseName = "The Times";
            defendantPersonal.Address = personalAddress;
            defendantDetails.PersonalDetails = defendantPersonal;

            CT_INPUT_Vehicle vehicle = new CT_INPUT_Vehicle();
            vehicle.VRN = "KP05 FAB";
            defendantDetails.Vehicle = vehicle;

            CT_INPUT_InsurerInformation insurerInformation = new CT_INPUT_InsurerInformation();
            insurerInformation.InsurerType = C13_InsurerType.I;

            //  For testing A2A
            insurerInformation.InsurerName = "FWBS COMP";
            insurerInformation.InsurerOrganizationID = "COMP009";
            insurerInformation.InsurerOrganizationPath = "/C00009";

            defendantDetails.InsurerInformation = insurerInformation;

            CT_INPUT_Defendant_CompanyDetails defendantCompanyDetails = new CT_INPUT_Defendant_CompanyDetails();
            defendantCompanyDetails.CompanyName = "FWBS COMP";

            CT_INPUT_Address defendantCompanyAddress = new CT_INPUT_Address();
            defendantCompanyAddress.Street1 = "76 Pine Lane";
            defendantCompanyAddress.City = "Northampton";
            defendantCompanyAddress.Country = "England";
            defendantCompanyAddress.AddressType = C01_AddressType.A;
            defendantCompanyDetails.Address = defendantCompanyAddress;

            defendantDetails.CompanyDetails = defendantCompanyDetails;
            claimAndClaimantDetails.DefendantDetails = defendantDetails;


            //  Claimant Details
            CT_INPUT_ClaimantDetails claimantDetails = new CT_INPUT_ClaimantDetails();
            claimantDetails.ChildClaim = C00_YNFlag.Item0;
            claimantDetails.Occupation = "Builder";

            CT_Vehicle claimantVehice = new CT_Vehicle();
            claimantDetails.Vehicle = claimantVehice;

            claimantDetails.NINComment = "comment";

            CT_INPUT_Claimant_PersonalDetails claimantPersonalDetails = new CT_INPUT_Claimant_PersonalDetails();
            claimantPersonalDetails.DateOfBirth = Convert.ToDateTime("22/02/1985");
            claimantPersonalDetails.Name = "Tom";
            claimantPersonalDetails.Surname = "Watts";
            claimantPersonalDetails.TitleType = C02_TitleType.Item1;

            CT_INPUT_Address claimantPersonalDetailsAddress = new CT_INPUT_Address();
            claimantPersonalDetailsAddress.HouseName = "Home";
            claimantPersonalDetailsAddress.Street1 = "5 Kingsthore Road";
            claimantPersonalDetailsAddress.City = "Northampton";
            claimantPersonalDetailsAddress.Country = "England";
            claimantPersonalDetailsAddress.AddressType = C01_AddressType.A;
            claimantPersonalDetails.Address = claimantPersonalDetailsAddress;
            claimantDetails.PersonalDetails = claimantPersonalDetails;
            claimAndClaimantDetails.ClaimantDetails = claimantDetails;

            doc.ClaimAndClaimantDetails = claimAndClaimantDetails;


            //  MedicalDetails 
            DocumentInputMedicalDetails medicalDetails = new DocumentInputMedicalDetails();
            CT_INPUT_Injury injury = new CT_INPUT_Injury();
            injury.HospitalAttendance = C00_YNFlag.Item1;
            injury.InjurySustainedDescription = "Wiplash";
            injury.MedicalAttentionSeeking = C00_YNFlag.Item1;
            injury.TimeOffRequired = C00_YNFlag.Item1;

            injury.BoneInjury = C00_YNFlag.Item1;
            injury.BoneInjurySpecified = true;
            injury.DaysNumber = "3";
            injury.MedicalAttentionFirstDate = DateTime.Now;
            injury.MedicalAttentionFirstDateSpecified = true;
            injury.Other = C00_YNFlag.Item1;
            injury.OtherSpecified = true;
            injury.OvernightDetention = C00_YNFlag.Item1;
            injury.OvernightDetentionSpecified = true;
            injury.SoftTissue = C00_YNFlag.Item1;
            injury.SoftTissueSpecified = true;
            injury.StillOffWork = C00_YNFlag.Item0;
            injury.StillOffWorkSpecified = true;
            injury.TimeOffPeriod = "3";
            injury.Whiplash = C00_YNFlag.Item1;
            injury.WhiplashSpecified = true;
            medicalDetails.Injury = injury;

            CT_INPUT_Hospital[] hospitals = new CT_INPUT_Hospital[1];
            CT_INPUT_Hospital hospital = new CT_INPUT_Hospital();
            hospital.HospitalName = "Hospital name";
            hospital.PostCode = "DG3 5VN";
            hospital.HospitalType = C07_HospitalType.Item0;

            CT_HospitalAddress hospitalAddress = new CT_HospitalAddress();
            hospitalAddress.AddressLine1 = "Hospital address1";
            hospitalAddress.AddressLine2 = "Hospital address2";
            hospitalAddress.AddressLine3 = "Hospital address3";
            hospitalAddress.AddressLine4 = "Hospital address4";
            hospital.HospitalAddress = hospitalAddress;

            hospitals[0] = hospital;
            medicalDetails.Hospital = hospitals;

            CT_INPUT_Rehabilitation rehabilitation = new CT_INPUT_Rehabilitation();
            rehabilitation.RehabilitationUndertaken = C03_RehabilitationRecommended.Item0;
            rehabilitation.RehabilitationDetails = "Full details on rahabilitaion needs claimant has arising out of the accident";
            rehabilitation.RehabilitationNeeds = C00_YNFlag.Item1;
            rehabilitation.RehabilitationNeedsSpecified = true;
            rehabilitation.RehabilitationTreatment = "tails of the rehabilitation treatment recommended and any treatment provided including name of provider";

            medicalDetails.Rehabilitation = rehabilitation;
            doc.MedicalDetails = medicalDetails;

            //  RepairsAndAlternativeVehicleProvision
            DocumentInputRepairsAndAlternativeVehicleProvision repairsAndAlternativeVehicleProvision = new DocumentInputRepairsAndAlternativeVehicleProvision();
            CT_INPUT_Repairs repairs = new CT_INPUT_Repairs();
            repairs.ClaiimingDamageOwnVehicle = C00_YNFlag.Item1;
            repairs.DetailsOfTheInsurance = C04_DetailsOfTheInsurance.Item0;
            repairs.ThroughClaimantInsurer = C00_YNFlag.Item1;
            repairs.TotalLoss = C14_YNDK.Item1;

            repairs.ContactDetails = "Contact for  the defendants insurer";
            repairs.DefendantInsInspection = C00_YNFlag.Item1;
            repairs.Location = "Location for  the defendants insurer";
            repairs.OtherDetails = "other details of the insurance";
            repairs.RepairsPosition = C05_RepairsPosition.Item3;
            repairs.ThroughAlternatieCompany = C00_YNFlag.Item1;

            CT_INPUT_AlternativeCompany alternativeCompany = new CT_INPUT_AlternativeCompany();
            alternativeCompany.Address = "Alternative company address";
            alternativeCompany.CompanyName = "Alternative company";
            alternativeCompany.ReferenceNumber = "123";
            alternativeCompany.TelephoneNumber = "12345678";
            repairs.AlternativeCompany = alternativeCompany;
            repairsAndAlternativeVehicleProvision.Repairs = repairs;

            //  AlternativeVehicleProvision 
            CT_INPUT_AlternativeVehicleProvision alternativeVehicleProvision = new CT_INPUT_AlternativeVehicleProvision();
            alternativeVehicleProvision.AVProvided = C00_YNFlag.Item1;
            alternativeVehicleProvision.AVRequiredByCL = C00_YNFlag.Item1;
            alternativeVehicleProvision.AVRequiredByCR = C00_YNFlag.Item0;
            alternativeVehicleProvision.ClaimantEntitled = C00_YNFlag.Item1;

            CT_INPUT_Provider provider = new CT_INPUT_Provider();
            provider.ProviderName = "A Provider";
            provider.ReferenceNumber = "123456789";
            provider.StartDate = Convert.ToDateTime("2009-12-11");
            provider.EndDate = "11/12/2009";
            provider.ProviderAddress = "Provider Address";

            CT_Vehicle providerVehicle = new CT_Vehicle();
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
            DocumentInputAccidentData accidentData = new DocumentInputAccidentData();
            CT_INPUT_AccidentDetails accidentDetails = new CT_INPUT_AccidentDetails();
            accidentDetails.AccidentDate = Convert.ToDateTime("12-12-2009");
            accidentDetails.AccidentDescription = "Two car shunt";
            accidentDetails.AccidentLocation = "Bridge Street, Northampton";
            accidentDetails.AccidentTime = "12:02";
            accidentDetails.ClaimantType = C06_ClaimantType.Item1;
            accidentDetails.OccupantsNumber = "1";
            accidentDetails.Seatbelt = C11_SeatbeltWearing.Item1;

            CT_INPUT_Driver_PersonalDetails driverPersonalDetails = new CT_INPUT_Driver_PersonalDetails();
            driverPersonalDetails.Surname = "William Thomas";
            driverPersonalDetails.MiddleName = "A";
            driverPersonalDetails.Name = "William";

            CT_INPUT_Address driverPersonalDetailsAddress = new CT_INPUT_Address();
            driverPersonalDetailsAddress.Street1 = "10 Silver Street";
            driverPersonalDetailsAddress.City = "Northampton";
            driverPersonalDetailsAddress.Country = "UK";
            driverPersonalDetailsAddress.AddressType = C01_AddressType.A;
            driverPersonalDetailsAddress.HouseName = "The Trees";
            driverPersonalDetailsAddress.District = "Abington";
            driverPersonalDetailsAddress.HouseNumber = "10";
            driverPersonalDetails.Address = driverPersonalDetailsAddress;
            accidentDetails.Driver = driverPersonalDetails;

            CT_INPUT_VehicleOwner_PersonalDetails vehicleOwnerPersonalDetails = new CT_INPUT_VehicleOwner_PersonalDetails();

            CT_INPUT_Address vehicleOwnerPersonalDetailsAddress = new CT_INPUT_Address();
            vehicleOwnerPersonalDetailsAddress.Street1 = "10 Silver Street";
            vehicleOwnerPersonalDetailsAddress.City = "Northampton";
            vehicleOwnerPersonalDetailsAddress.Country = "UK";
            vehicleOwnerPersonalDetailsAddress.AddressType = C01_AddressType.A;
            vehicleOwnerPersonalDetailsAddress.HouseName = "The Trees";
            vehicleOwnerPersonalDetailsAddress.District = "Abington";
            vehicleOwnerPersonalDetailsAddress.HouseNumber = "10";
            vehicleOwnerPersonalDetails.Address = vehicleOwnerPersonalDetailsAddress;
            accidentDetails.Owner = vehicleOwnerPersonalDetails;

            //  Insurer Details
            CT_INPUT_Insurance_CompanyDetails insuranceCompanyDetails = new CT_INPUT_Insurance_CompanyDetails();
            CT_INPUT_Address insuranceCompanyDetailsAddress = new CT_INPUT_Address();
            insuranceCompanyDetailsAddress.Street1 = "77 The Mews";
            insuranceCompanyDetailsAddress.City = "Northampton";
            insuranceCompanyDetailsAddress.Country = "UK";
            insuranceCompanyDetailsAddress.AddressType = C01_AddressType.A;
            insuranceCompanyDetailsAddress.HouseName = "My Home";
            insuranceCompanyDetailsAddress.District = "Up Town";
            insuranceCompanyDetailsAddress.HouseNumber = "33";
            insuranceCompanyDetails.Address = insuranceCompanyDetailsAddress;
            accidentDetails.InsuranceCompanyInformatiion = insuranceCompanyDetails;

            CT_WeatherConditions weather = new CT_WeatherConditions();
            weather.Fog = C00_YNFlag.Item0;
            weather.Ice = C00_YNFlag.Item0;
            weather.Other = C00_YNFlag.Item0;
            weather.Rain = C00_YNFlag.Item0;
            weather.Snow = C00_YNFlag.Item1;
            weather.Sun = C00_YNFlag.Item0;
            accidentDetails.WeatherConditions = weather;

            CT_RoadConditions roadConditions = new CT_RoadConditions();
            roadConditions.Dry = C00_YNFlag.Item0;
            roadConditions.Ice = C00_YNFlag.Item0;
            roadConditions.Mud = C00_YNFlag.Item1;
            roadConditions.Oil = C00_YNFlag.Item0;
            roadConditions.Other = C00_YNFlag.Item0;
            roadConditions.Snow = C00_YNFlag.Item0;
            roadConditions.Wet = C00_YNFlag.Item0;
            accidentDetails.RoadConditions = roadConditions;

            CT_AccidentCircumstances accidentCircumstances = new CT_AccidentCircumstances();
            accidentCircumstances.AccidCarPark = C00_YNFlag.Item0;
            accidentCircumstances.AccidChangingLanes = C00_YNFlag.Item0;
            accidentCircumstances.AccidRoundabout = C00_YNFlag.Item0;
            accidentCircumstances.ConcertinaCollision = C00_YNFlag.Item0;
            accidentCircumstances.Other = C00_YNFlag.Item0;
            accidentCircumstances.VhclHitInRear = C00_YNFlag.Item0;
            accidentCircumstances.VhclHitSideRoad = C00_YNFlag.Item0;
            accidentCircumstances.VhclHitWhilstParked = C00_YNFlag.Item1;
            accidentDetails.AccidentCircumstances = accidentCircumstances;

            CT_PoliceDetails police = new CT_PoliceDetails();
            police.ReferenceNumber = "123";
            police.ReportingOfficerName = "Reporting Officer Name";
            police.StationAddress = "Police Station Address";
            police.StationName = "Police Station Name";
            accidentDetails.PoliceDetails = police;

            accidentData.AccidentDetails = accidentDetails;

            CT_INPUT_BusCoach busCoach = new CT_INPUT_BusCoach();
            busCoach.BussOrCoach = C00_YNFlag.Item0;
            busCoach.Evidence = C00_YNFlag.Item0;
            busCoach.VehicleDescription = "A Big Bus";
            busCoach.Comments = "Some Comments";
            busCoach.DriverDescription = "Description of the driver =drunk";
            busCoach.DriverID = "123";
            busCoach.DriverName = "Driver Name";
            busCoach.NumberOfPassengers = "2";
            accidentData.BusCoach = busCoach;

            doc.AccidentData = accidentData;

            DocumentInputLiabilityFunding liabilityFunding = new DocumentInputLiabilityFunding();
            CT_INPUT_Liability liability = new CT_INPUT_Liability();
            liability.DefendantResponsability = "Defendant Responsability";
            liabilityFunding.Liability = liability;

            CT_INPUT_Funding funding = new CT_INPUT_Funding();
            funding.ConsideredFreeLegalExpIns = C00_YNFlag.Item0;
            funding.FundingUndertaken = C00_YNFlag.Item1;
            funding.AgreementDate = DateTime.Now;
            funding.Comments = "Section M - Other relevant information";
            funding.ConditionalFeeDate = DateTime.Now;
            funding.ICAddress = "Address of Insurance Company";
            funding.ICName = "Insurance Company Name";
            funding.IncreasingPoint = "At which point is an increased premium payable?";
            funding.LevelOfCover = "Level of cover";
            funding.MembershipOrganisation = C00_YNFlag.Item1;
            funding.Ohter = C00_YNFlag.Item1;
            funding.OrganizationName = "Organisation Name";
            funding.OtherDetails = "Other details";
            funding.PolicyDate = DateTime.Now;
            funding.PolicyNumber = "123";
            funding.PremiumsStaged = C00_YNFlag.Item0;
            funding.Section29 = C00_YNFlag.Item1;
            funding.Section58 = C00_YNFlag.Item1;
            liabilityFunding.Funding = funding;

            doc.LiabilityFunding = liabilityFunding;

            return doc;
        }

        /// <summary>
        /// Get Liability Decision
        /// </summary>
        /// <returns></returns>
        private InsurerResponseA2A GetLiabilityDecision()
        {
            //  InsurerResponseA2A 
            InsurerResponseA2A insurer = new InsurerResponseA2A();
            insurer.Capacity = RTAServicesLibrary.C15_Capacity.Item3;
            insurer.OtherCapacity = "";

            //  LiabilityCausation 
            CT_A2A_LiabilityCausation liabilityCausation = new CT_A2A_LiabilityCausation();
            liabilityCausation.LiabilityDecision = RTAServicesLibrary.C17_LiabilityDecision.A;
            liabilityCausation.NoAuthority = RTAServicesLibrary.C00_YNFlag.Item0;
            liabilityCausation.NoAuthoritySpecified = true;
            liabilityCausation.UnadmittedLiabilityReasons = "";

            //  DefendantAdmits 
            CT_DefendantAdmits defendantAdmits = new CT_DefendantAdmits();
            defendantAdmits.AccidentOccurred = RTAServicesLibrary.C00_YNFlag.Item1;
            defendantAdmits.AccidentOccurredSpecified = true;
            defendantAdmits.CausedByDefendant = RTAServicesLibrary.C00_YNFlag.Item1;
            defendantAdmits.CausedByDefendantSpecified = true;
            defendantAdmits.CausedSomeLossToTheClaimant = RTAServicesLibrary.C00_YNFlag.Item1;
            defendantAdmits.CausedSomeLossToTheClaimantSpecified = true;
            liabilityCausation.DefendantAdmits = defendantAdmits;
            insurer.LiabilityCausation = liabilityCausation;

            //  Provided Services 
            CT_A2A_ProvidedServices providedServices = new CT_A2A_ProvidedServices();
            providedServices.AltVchlProvided = RTAServicesLibrary.C00_YNFlag.Item0;
            providedServices.AltVhclDetails = "";
            providedServices.PreparedToProvideReabilitation = RTAServicesLibrary.C00_YNFlag.Item0;
            providedServices.ReabilitationDetails = "";
            providedServices.ReabilitationProvided = RTAServicesLibrary.C00_YNFlag.Item0;
            providedServices.RepairsProvided = RTAServicesLibrary.C00_YNFlag.Item0;
            providedServices.RepairsDetails = "";
            insurer.ProvidedServices = providedServices;

            //  Defendants Insurer 
            CT_A2A_InsurerResponse_CompanyDetails companyDetails = new CT_A2A_InsurerResponse_CompanyDetails();
            companyDetails.ContactMiddleName = "";
            companyDetails.ContactName = "Fred";
            companyDetails.ContactSurname = "Smith";
            companyDetails.EmailAddress = "Fred@Smith.co.uk";
            companyDetails.ReferenceNumber = "6473828";
            companyDetails.TelephoneNumber = "01604 263738";
            RTAServicesLibrary.CT_INPUT_Address address = new RTAServicesLibrary.CT_INPUT_Address();
            address.AddressType = RTAServicesLibrary.C01_AddressType.A;
            address.City = "Northampton";
            address.Country = "UK";
            address.County = "";
            address.District = "";
            address.HouseName = "";
            address.HouseNumber = "77";
            address.PostCode = "";
            address.Street1 = "The Mews";
            address.Street2 = "";
            companyDetails.Address = address;
            insurer.ProvidedServices.DefendantsInsurer = companyDetails;

            return insurer;
        }
    }
}
