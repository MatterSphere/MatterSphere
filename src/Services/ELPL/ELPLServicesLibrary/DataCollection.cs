using System.Collections.Generic;

namespace ELPLServicesLibrary
{
    class DataCollection
    {

        #region Private Fields

        //  Yes No - C00_YNFlag
        private const string YESNO_NO = "No";
        private const string YESNO_YES = "Yes";

        //  Address Type - C01_AddressType
        private const string ADDRESSTYPE_ASADDRESS = "As Address";
        private const string ADDRESSTYPE_BUSINESS = "Business";
        private const string ADDRESSTYPE_PERSONAL = "Personal";

        //  Title Type - C02_TitleType
        private const string TITLETYPE_MR = "Mr";
        private const string TITLETYPE_MRS = "Mrs";
        private const string TITLETYPE_MS = "Ms";
        private const string TITLETYPE_MISS = "Miss";
        private const string TITLETYPE_OTHER = "Other";

        //  Rehabilitation Recommended - C03_RehabilitationRecommended
        private const string REHABILITATIONRECOMMENDED_YES = "Yes";
        private const string REHABILITATIONRECOMMENDED_NO = "No";
        private const string REHABILITATIONRECOMMENDED_MEDICAL = "Medical Professional not seen";

        //  Details Of The Insurance - C04_DetailsOfTheInsurance
        private const string DETAILSOFTHEINSURANCE_COMPREHENSIVE = "Comprehensive";
        private const string DETAILSOFTHEINSURANCE_THIRDPARTYFIEANDTHEFT = "Third party fire and theft";
        private const string DETAILSOFTHEINSURANCE_THIRDPARTYONLY = "Third party only";
        private const string DETAILSOFTHEINSURANCE_OTHER = "Other";

        //  Repairs Position - C05_RepairsPosition
        private const string REPAIRSPOSITION_COMPLETE = "Complete";
        private const string REPAIRSPOSITION_AUTHORISED = "Authorised";
        private const string REPAIRSPOSITION_NOTYETAUTHORISED = "Not yet authorised";
        private const string REPAIRSPOSITION_NOTKNOWN = "Not known";

        //  Claimant Type - C06_ClaimantType
        private const string CLAIMANTTYPE_DRIVER = "The driver";
        private const string CLAIMANTTYPE_OWNER = "The owner of the vehicle but not driving";
        private const string CLAIMANTTYPE_PASSENGER = "A passenger in a vehicle owned by someone else";
        private const string CLAIMANTTYPE_PEDESTRIAN = "A pedestrian";
        private const string CLAIMANTTYPE_CYCLIST = "A cyclist";
        private const string CLAIMANTTYPE_MOTORCYCLIST = "A motorcyclist";
        private const string CLAIMANTTYPE_OTHER = "Other";

        //  Hospital Type - C07_HospitalType
        private const string HOSPITALTYPE_NHS = "NHS";
        private const string HOSPITALTYPE_PRIVATE = "Private";

        //  Sex Types - C08_Sex
        private const string SEX_MALE = "Male";
        private const string SEX_FEMALE = "Female";
        private const string SEX_NOTKNOWN = "Not Known";

        //  Subject Status - C09_SubjectStatus
        private const string STATUS_PERSONAL = "Personal";
        private const string STATUS_BUSINESS = "Business";

        //  Other Party - C10_OtherParty
        private const string OTHERPARTY_WITNESS = "Witness";
        private const string OTHERPARTY_OTHER = "Other";

        //  Seatbelt Wearing - C11_SeatbeltWearing
        private const string SEATBELTWEARING_NO = "No";
        private const string SEATBELTWEARING_YES = "Yes";
        private const string SEATBELTWEARING_NOTKNOWN = "Not known";

        //  Insurer Types - C13_InsurerType
        private const string INSURERTYPE_INSURER = "Insurer";
        private const string INSURERTYPE_SELFINSURED = "Self Insured";
        private const string INSURERTYPE_MIB = "MIB";

        //  Total Loss - C14_YNDK
        private const string TOTALLOSS_YES = "Yes";
        private const string TOTALLOSS_NO = "No";
        private const string TOTALLOSS_DONTKNOW = "Don't Know";

        //  Capacity Types - C15_Capacity
        private const string CAPACITYTYPE_CONTRACT = "Insurer in Contract";
        private const string CAPACITYTYPE_RTA = "RTA Insurer";
        private const string CAPACITYTYPE_ARTICLE75 = "Article 75 Insurer";
        private const string CAPACITYTYPE_MIB = "MIB";
        private const string CAPACITYTYPE_OTHER = "Other";

        //  Signatory Type - C16_SignatoryType
        private const string SIGNATORYTYPE_CLAIMANT_SOLICITOR = "Claimant Solicitor";
        private const string SIGNATORYTYPE_CLAIMANT_IN_PERSON = "Claimant in Person";

        //  Liability Decision - C17_LiabilityDecision
        private const string LIABILITY_ADMITTED = "Admitted";
        private const string LIABILITY_ADMITTEDWITHNEGLIGENCE = "Admitted with Negligence";
        private const string LIABILITY_NOTADMITTED = "Not Admitted";

        //  Loss Type - C18_LossType
        private const string LOSSTYPE_POLICYEXCESS = "Policy Excess";
        private const string LOSSTYPE_LOSSOFUSE = "Loss of Use";
        private const string LOSSTYPE_CARHIRE = "Car Hire";
        private const string LOSSTYPE_REPAIRCOSTS = "Repair Costs";
        private const string LOSSTYPE_FARES = "Fares (taxis, buses, tube etc)";
        private const string LOSSTYPE_MEDICALEXPENSES = "Medical Expenses";
        private const string LOSSTYPE_CLOTHING = "Clothing";
        private const string LOSSTYPE_CARESERVICE = "Care Service";
        private const string LOSSTYPE_LOSSEARNINGSCLAIMANT = "Loss of earnings for Claimant";
        private const string LOSSTYPE_LOSSEARNINGSEMPLOYER = "Loss of earnings for Employer";
        private const string LOSSTYPE_OTHERLOSSES = "Other Losses";
        private const string LOSSTYPE_GENERALDAMAGES = "General Damages";

        private const string LOSSTYPE_DISADVANTAGELABOURMARKET = "Disadvantage on the Labour Market"; // Added for V3
        private const string LOSSTYPE_LOSSCONGENIALEMPLOYMENT = "Loss of Congenial Employment"; // Added for V3
        private const string LOSSTYPE_FUTURELOSSES = "Future Losses"; // Added for V3

        //  Settlement Pack Decision - C20_SettlementPackDecision
        private const string SETTLEMENTPACKDECISION_CONFIRM = "Confirm";
        private const string SETTLEMENTPACKDECISION_COUNTEROFFER = "Counter Offer";
        private const string SETTLEMENTPACKDECISION_REPUDIATE = "Repudiate";

        //  Medical Reports - C22_MedicalReport
        private const string MEDICALREPORT_0 = "0";
        private const string MEDICALREPORT_1 = "1";
        private const string MEDICALREPORT_2 = "2";
        private const string MEDICALREPORT_3 = "3";
        private const string MEDICALREPORT_4 = "4";

        #endregion Private Fields


        #region Constructor

        public DataCollection()
        {
        }

        #endregion Constructor


        #region C00_YNFlag
        /// <summary>
        /// Yes/No List
        /// </summary>
        /// <returns></returns>
        static public List<string> YesNoList()
        {
            List<string> list = new List<string>();
            list.Add(YESNO_NO);
            list.Add(YESNO_YES);
            return list;
        }

        /// <summary>
        /// Yes/No Value
        /// </summary>
        /// <param name="yesno"></param>
        /// <returns></returns>
        static public C00_YNFlag YesNoValue(string yesno)
        {
            switch (yesno)
            {
                case YESNO_NO:
                    return C00_YNFlag.Item0;
                case YESNO_YES:
                    return C00_YNFlag.Item1;
                default:
                    return C00_YNFlag.Item0;
            }
        }
        #endregion C00_YNFlag


        #region C01_AddressType
        /// <summary>
        /// Address Type List
        /// </summary>
        /// <returns></returns>
        static public List<string> AddressTypeList()
        {
            List<string> list = new List<string>();
            list.Add(ADDRESSTYPE_ASADDRESS);
            list.Add(ADDRESSTYPE_BUSINESS);
            list.Add(ADDRESSTYPE_PERSONAL);
            return list;
        }

        /// <summary>
        /// Address Type Value
        /// </summary>
        /// <param name="addressType"></param>
        /// <returns></returns>
        static C01_AddressType AddressTypeValue(string addressType)
        {
            switch (addressType)
            {
                case ADDRESSTYPE_ASADDRESS:
                    return C01_AddressType.A;
                case ADDRESSTYPE_BUSINESS:
                    return C01_AddressType.F;
                case ADDRESSTYPE_PERSONAL:
                    return C01_AddressType.P;
                default:
                    return C01_AddressType.A;
            }
        }
        #endregion C01_AddressType


        #region C02_TitleType
        /// <summary>
        /// Title Type List
        /// </summary>
        /// <returns></returns>
        static public List<string> TitleTypeList()
        {
            List<string> list = new List<string>();
            list.Add(TITLETYPE_MR);
            list.Add(TITLETYPE_MRS);
            list.Add(TITLETYPE_MS);
            list.Add(TITLETYPE_MISS);
            list.Add(TITLETYPE_OTHER);
            return list;
        }

        /// <summary>
        /// Title Type Value
        /// </summary>
        /// <param name="titleType"></param>
        /// <returns></returns>
        static public C02_TitleType TitleTypeValue(string titleType)
        {
            switch (titleType)
            {
                case TITLETYPE_MR:
                    return C02_TitleType.Item1;
                case TITLETYPE_MRS:
                    return C02_TitleType.Item2;
                case TITLETYPE_MS:
                    return C02_TitleType.Item3;
                case TITLETYPE_MISS:
                    return C02_TitleType.Item4;
                case TITLETYPE_OTHER:
                    return C02_TitleType.Item5;
                default:
                    return C02_TitleType.Item1;
            }
        }

        #endregion C02_TitleType


        #region C03_RehabilitationRecommended
        /// <summary>
        /// Rehabilitation Recommended List
        /// </summary>
        /// <returns></returns>
        static public List<string> RehabilitationRecommendedList()
        {
            List<string> list = new List<string>();
            list.Add(REHABILITATIONRECOMMENDED_YES);
            list.Add(REHABILITATIONRECOMMENDED_NO);
            list.Add(REHABILITATIONRECOMMENDED_MEDICAL);
            return list;
        }

        /// <summary>
        /// Rehabilitation Recommended Value
        /// </summary>
        /// <param name="recommended"></param>
        /// <returns></returns>
        static public C03_RehabilitationRecommended RehabilitationRecommendedValue(string recommended)
        {
            switch (recommended)
            {
                case REHABILITATIONRECOMMENDED_YES:
                    return C03_RehabilitationRecommended.Item0;
                case REHABILITATIONRECOMMENDED_NO:
                    return C03_RehabilitationRecommended.Item1;
                case REHABILITATIONRECOMMENDED_MEDICAL:
                    return C03_RehabilitationRecommended.Item3;
                default:
                    return C03_RehabilitationRecommended.Item0;
            }
        }
        #endregion C03_RehabilitationRecommended


        #region C04_DetailsOfTheInsurance
        /// <summary>
        /// Details Of The Insurance List
        /// </summary>
        /// <returns></returns>
        static public List<string> DetailsOfTheInsuranceList()
        {
            List<string> list = new List<string>();
            list.Add(DETAILSOFTHEINSURANCE_COMPREHENSIVE);
            list.Add(DETAILSOFTHEINSURANCE_THIRDPARTYFIEANDTHEFT);
            list.Add(DETAILSOFTHEINSURANCE_THIRDPARTYONLY);
            list.Add(DETAILSOFTHEINSURANCE_OTHER);
            return list;
        }

        /// <summary>
        /// Details Of The Insurance Value
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        static public C04_DetailsOfTheInsurance DetailsOfTheInsuranceValue(string details)
        {
            switch (details)
            {
                case DETAILSOFTHEINSURANCE_COMPREHENSIVE:
                    return C04_DetailsOfTheInsurance.Item0;
                case DETAILSOFTHEINSURANCE_THIRDPARTYFIEANDTHEFT:
                    return C04_DetailsOfTheInsurance.Item1;
                case DETAILSOFTHEINSURANCE_THIRDPARTYONLY:
                    return C04_DetailsOfTheInsurance.Item2;
                case DETAILSOFTHEINSURANCE_OTHER:
                    return C04_DetailsOfTheInsurance.Item3;
                default:
                    return C04_DetailsOfTheInsurance.Item0;
            }
        }

        #endregion C04_DetailsOfTheInsurance


        #region C05_RepairsPosition
        /// <summary>
        /// Repairs Position List
        /// </summary>
        /// <returns></returns>
        static public List<string> RepairsPositionList()
        {
            List<string> list = new List<string>();
            list.Add(REPAIRSPOSITION_COMPLETE);
            list.Add(REPAIRSPOSITION_AUTHORISED);
            list.Add(REPAIRSPOSITION_NOTYETAUTHORISED);
            list.Add(REPAIRSPOSITION_NOTKNOWN);
            return list;
        }

        /// <summary>
        /// Repairs Position Value
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        static public C05_RepairsPosition RepairsPositionValue(string position)
        {
            switch (position)
            {
                case REPAIRSPOSITION_COMPLETE:
                    return C05_RepairsPosition.Item0;
                case REPAIRSPOSITION_AUTHORISED:
                    return C05_RepairsPosition.Item1;
                case REPAIRSPOSITION_NOTYETAUTHORISED:
                    return C05_RepairsPosition.Item2;
                case REPAIRSPOSITION_NOTKNOWN:
                    return C05_RepairsPosition.Item3;
                default:
                    return C05_RepairsPosition.Item0;
            }
        }
        #endregion C05_RepairsPosition


        #region C07_HospitalType
        /// <summary>
        /// Hospital Type List
        /// </summary>
        /// <returns></returns>
        static public List<string> HospitalTypeList()
        {
            List<string> list = new List<string>();
            list.Add(HOSPITALTYPE_NHS);
            list.Add(HOSPITALTYPE_PRIVATE);
            return list;
        }

        /// <summary>
        /// Hospital Type Value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public C07_HospitalType HospitalTypeValue(string type)
        {
            switch (type)
            {
                case HOSPITALTYPE_NHS:
                    return C07_HospitalType.Item0;
                case HOSPITALTYPE_PRIVATE:
                    return C07_HospitalType.Item1;
                default:
                    return C07_HospitalType.Item0;
            }
        }
        #endregion C07_HospitalType


        #region C09_SubjectStatus
        /// <summary>
        /// Subject Status List
        /// </summary>
        /// <returns></returns>
        static public List<string> SubjectStatusList()
        {
            List<string> list = new List<string>();
            list.Add(STATUS_PERSONAL);
            list.Add(STATUS_BUSINESS);
            return list;
        }

        /// <summary>
        /// Subject Status Value
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        static public C09_SubjectStatus SubjectStatusValue(string status)
        {
            switch (status)
            {
                case STATUS_PERSONAL:
                    return C09_SubjectStatus.P;
                case STATUS_BUSINESS:
                    return C09_SubjectStatus.B;
                default:
                    return C09_SubjectStatus.P;
            }
        }
        #endregion C09_SubjectStatus


        #region C14_YNDK (Total Loss)
        /// <summary>
        /// Total Loss List
        /// </summary>
        /// <returns></returns>
        static public List<string> TotalLossList()
        {
            List<string> list = new List<string>();
            list.Add(TOTALLOSS_YES);
            list.Add(TOTALLOSS_NO);
            list.Add(TOTALLOSS_DONTKNOW);
            return list;
        }

        /// <summary>
        /// Total Loss Value
        /// </summary>
        /// <param name="total"></param>
        /// <returns></returns>
        static public C14_YNDK TotalLossValue(string total)
        {
            switch (total)
            {
                case TOTALLOSS_YES:
                    return C14_YNDK.Item0;
                case TOTALLOSS_NO:
                    return C14_YNDK.Item1;
                case TOTALLOSS_DONTKNOW:
                    return C14_YNDK.Item2;
                default:
                    return C14_YNDK.Item2;
            }
        }
        #endregion C14_YNDK (Total Loss)


        #region C16_SignatoryType
        /// <summary>
        /// Signatory Type List
        /// </summary>
        /// <returns></returns>
        static public List<string> SignatoryTypeList()
        {
            List<string> list = new List<string>();
            list.Add(SIGNATORYTYPE_CLAIMANT_SOLICITOR);
            list.Add(SIGNATORYTYPE_CLAIMANT_IN_PERSON);
            return list;
        }

        /// <summary>
        /// Signatory Type Value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public C16_SignatoryType SignatoryTypeValue(string type)
        {
            switch (type)
            {
                case SIGNATORYTYPE_CLAIMANT_SOLICITOR:
                    return C16_SignatoryType.S;
                case SIGNATORYTYPE_CLAIMANT_IN_PERSON:
                    return C16_SignatoryType.C;
                default:
                    return C16_SignatoryType.S;
            }
        }
        #endregion C16_SignatoryType


        #region C17_LiabilityDecision
        /// <summary>
        /// Liability Decision List
        /// </summary>
        /// <returns></returns>
        static public List<string> LiabilityDecisionList()
        {
            List<string> list = new List<string>();
            list.Add(LIABILITY_ADMITTED);
            list.Add(LIABILITY_ADMITTEDWITHNEGLIGENCE);
            list.Add(LIABILITY_NOTADMITTED);
            return list;
        }

        /// <summary>
        /// Liability Decision Value
        /// </summary>
        /// <param name="decision"></param>
        /// <returns></returns>
        static public C17_LiabilityDecision LiabilityDecisionValue(string decision)
        {
            switch (decision)
            {
                case LIABILITY_ADMITTED:
                    return C17_LiabilityDecision.A;
                case LIABILITY_ADMITTEDWITHNEGLIGENCE:
                    return C17_LiabilityDecision.AN;
                case LIABILITY_NOTADMITTED:
                    return C17_LiabilityDecision.N;
                default:
                    return C17_LiabilityDecision.A;
            }
        }
        #endregion C17_LiabilityDecision


        #region C18_LossType
        /// <summary>
        /// Loss Type List
        /// </summary>
        /// <returns></returns>
        static public List<string> LossTypeList()
        {
            List<string> list = new List<string>();
            list.Add(LOSSTYPE_FARES);
            list.Add(LOSSTYPE_MEDICALEXPENSES);
            list.Add(LOSSTYPE_CLOTHING);
            list.Add(LOSSTYPE_CARESERVICE);
            list.Add(LOSSTYPE_LOSSEARNINGSCLAIMANT);
            list.Add(LOSSTYPE_LOSSEARNINGSEMPLOYER);
            list.Add(LOSSTYPE_OTHERLOSSES);
            list.Add(LOSSTYPE_GENERALDAMAGES);
            return list;
        }

        /// <summary>
        /// Loss Type Value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public C18_LossType_R3 LossTypeValue(string type)
        {
            switch (type)
            {
                case LOSSTYPE_FARES:
                    return C18_LossType_R3.Item4;
                case LOSSTYPE_MEDICALEXPENSES:
                    return C18_LossType_R3.Item5;
                case LOSSTYPE_CLOTHING:
                    return C18_LossType_R3.Item6;
                case LOSSTYPE_CARESERVICE:
                    return C18_LossType_R3.Item7;
                case LOSSTYPE_LOSSEARNINGSCLAIMANT:
                    return C18_LossType_R3.Item8;
                case LOSSTYPE_LOSSEARNINGSEMPLOYER:
                    return C18_LossType_R3.Item9;
                case LOSSTYPE_OTHERLOSSES:
                    return C18_LossType_R3.Item10;
                case LOSSTYPE_GENERALDAMAGES:
                    return C18_LossType_R3.Item11;
                case LOSSTYPE_DISADVANTAGELABOURMARKET:
                    return C18_LossType_R3.Item13;
                case LOSSTYPE_LOSSCONGENIALEMPLOYMENT:
                    return C18_LossType_R3.Item14;
                case LOSSTYPE_FUTURELOSSES:
                    return C18_LossType_R3.Item15;
                default:
                    return C18_LossType_R3.Item10;
            }
        }
        #endregion C18_LossType


        #region C20_SettlementPackDecision
        /// <summary>
        /// Settlement Pack Decision List
        /// </summary>
        /// <returns></returns>
        static public List<string> SettlementPackDecisionList()
        {
            List<string> list = new List<string>();
            list.Add(SETTLEMENTPACKDECISION_CONFIRM);
            list.Add(SETTLEMENTPACKDECISION_COUNTEROFFER);
            list.Add(SETTLEMENTPACKDECISION_REPUDIATE);
            return list;
        }

        /// <summary>
        /// Settlement Pack Decision Value
        /// </summary>
        /// <param name="yesno"></param>
        /// <returns></returns>
        static public C20_SettlementPackDecision SettlementPackDecisionValue(string decision)
        {
            switch (decision)
            {
                case SETTLEMENTPACKDECISION_CONFIRM:
                    return C20_SettlementPackDecision.C;
                case SETTLEMENTPACKDECISION_COUNTEROFFER:
                    return C20_SettlementPackDecision.CO;
                case SETTLEMENTPACKDECISION_REPUDIATE:
                    return C20_SettlementPackDecision.R;
                default:
                    return C20_SettlementPackDecision.C;
            }
        }
        #endregion C20_SettlementPackDecision


        #region C22_MedicalReport
        /// <summary>
        /// Medical Report List
        /// </summary>
        /// <returns></returns>
        static public List<string> MedicalReportList()
        {
            List<string> list = new List<string>();
            list.Add(MEDICALREPORT_0);
            list.Add(MEDICALREPORT_1);
            list.Add(MEDICALREPORT_2);
            list.Add(MEDICALREPORT_3);
            list.Add(MEDICALREPORT_4);
            return list;
        }

        /// <summary>
        /// Medical Report Value
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        static public C22_MedicalReport MedicalReportValue(string report)
        {
            switch (report)
            {
                case MEDICALREPORT_0:
                    return C22_MedicalReport.Item0;
                case MEDICALREPORT_1:
                    return C22_MedicalReport.Item1;
                case MEDICALREPORT_2:
                    return C22_MedicalReport.Item2;
                case MEDICALREPORT_3:
                    return C22_MedicalReport.Item3;
                case MEDICALREPORT_4:
                    return C22_MedicalReport.Item4;
                default:
                    return C22_MedicalReport.Item0;
            }
        }
        #endregion C22_MedicalReport
    }
}
