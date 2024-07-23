using System.Collections.Generic;

namespace RTAServicesLibrary
{
    public class DataCollection
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
        static public RTAServicesLibraryV2.C00_YNFlag YesNoValue(string yesno)
        {
            switch (yesno)
            {
                case YESNO_NO:
                    return RTAServicesLibraryV2.C00_YNFlag.Item0;
                case YESNO_YES:
                    return RTAServicesLibraryV2.C00_YNFlag.Item1;
                default:
                    return RTAServicesLibraryV2.C00_YNFlag.Item0;
            }
        }


        /// <summary>
        /// Yes/No Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="yesno"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C00_YNFlag YesNoValueV3(string yesno)
        {
            switch (yesno)
            {
                case YESNO_NO:
                    return RTAServicesLibraryV3.C00_YNFlag.Item0;
                case YESNO_YES:
                    return RTAServicesLibraryV3.C00_YNFlag.Item1;
                default:
                    return RTAServicesLibraryV3.C00_YNFlag.Item0;
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
        static RTAServicesLibraryV2.C01_AddressType AddressTypeValue(string addressType)
        {
            switch (addressType)
            {
                case ADDRESSTYPE_ASADDRESS:
                    return RTAServicesLibraryV2.C01_AddressType.A;
                case ADDRESSTYPE_BUSINESS:
                    return RTAServicesLibraryV2.C01_AddressType.F;
                case ADDRESSTYPE_PERSONAL:
                    return RTAServicesLibraryV2.C01_AddressType.P;
                default:
                    return RTAServicesLibraryV2.C01_AddressType.A;
            }
        }

        /// <summary>
        /// Address Type Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="addressType"></param>
        /// <returns></returns>
        static RTAServicesLibraryV3.C01_AddressType AddressTypeValueV3(string addressType)
        {
            switch (addressType)
            {
                case ADDRESSTYPE_ASADDRESS:
                    return RTAServicesLibraryV3.C01_AddressType.A;
                case ADDRESSTYPE_BUSINESS:
                    return RTAServicesLibraryV3.C01_AddressType.F;
                case ADDRESSTYPE_PERSONAL:
                    return RTAServicesLibraryV3.C01_AddressType.P;
                default:
                    return RTAServicesLibraryV3.C01_AddressType.A;
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
        static public RTAServicesLibraryV2.C02_TitleType TitleTypeValue(string titleType)
        {
            switch (titleType)
            {
                case TITLETYPE_MR:
                    return RTAServicesLibraryV2.C02_TitleType.Item1;
                case TITLETYPE_MRS:
                    return RTAServicesLibraryV2.C02_TitleType.Item2;
                case TITLETYPE_MS:
                    return RTAServicesLibraryV2.C02_TitleType.Item3;
                case TITLETYPE_MISS:
                    return RTAServicesLibraryV2.C02_TitleType.Item4;
                case TITLETYPE_OTHER:
                    return RTAServicesLibraryV2.C02_TitleType.Item5;
                default:
                    return RTAServicesLibraryV2.C02_TitleType.Item1;
            }
        }


        /// <summary>
        /// Title Type Value
        /// </summary>
        /// <param name="titleType"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C02_TitleType TitleTypeValueV3(string titleType)
        {
            switch (titleType)
            {
                case TITLETYPE_MR:
                    return RTAServicesLibraryV3.C02_TitleType.Item1;
                case TITLETYPE_MRS:
                    return RTAServicesLibraryV3.C02_TitleType.Item2;
                case TITLETYPE_MS:
                    return RTAServicesLibraryV3.C02_TitleType.Item3;
                case TITLETYPE_MISS:
                    return RTAServicesLibraryV3.C02_TitleType.Item4;
                case TITLETYPE_OTHER:
                    return RTAServicesLibraryV3.C02_TitleType.Item5;
                default:
                    return RTAServicesLibraryV3.C02_TitleType.Item1;
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
        static public RTAServicesLibraryV2.C03_RehabilitationRecommended RehabilitationRecommendedValue(string recommended)
        {
            switch (recommended)
            {
                case REHABILITATIONRECOMMENDED_YES:
                    return RTAServicesLibraryV2.C03_RehabilitationRecommended.Item0;
                case REHABILITATIONRECOMMENDED_NO:
                    return RTAServicesLibraryV2.C03_RehabilitationRecommended.Item1;
                case REHABILITATIONRECOMMENDED_MEDICAL:
                    return RTAServicesLibraryV2.C03_RehabilitationRecommended.Item3;
                default:
                    return RTAServicesLibraryV2.C03_RehabilitationRecommended.Item0;
            }
        }


        /// <summary>
        /// Rehabilitation Recommended Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="recommended"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C03_RehabilitationRecommended RehabilitationRecommendedValueV3(string recommended)
        {
            switch (recommended)
            {
                case REHABILITATIONRECOMMENDED_YES:
                    return RTAServicesLibraryV3.C03_RehabilitationRecommended.Item0;
                case REHABILITATIONRECOMMENDED_NO:
                    return RTAServicesLibraryV3.C03_RehabilitationRecommended.Item1;
                case REHABILITATIONRECOMMENDED_MEDICAL:
                    return RTAServicesLibraryV3.C03_RehabilitationRecommended.Item3;
                default:
                    return RTAServicesLibraryV3.C03_RehabilitationRecommended.Item0;
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
        static public RTAServicesLibraryV2.C04_DetailsOfTheInsurance DetailsOfTheInsuranceValue(string details)
        {
            switch (details)
            {
                case DETAILSOFTHEINSURANCE_COMPREHENSIVE:
                    return RTAServicesLibraryV2.C04_DetailsOfTheInsurance.Item0;
                case DETAILSOFTHEINSURANCE_THIRDPARTYFIEANDTHEFT:
                    return RTAServicesLibraryV2.C04_DetailsOfTheInsurance.Item1;
                case DETAILSOFTHEINSURANCE_THIRDPARTYONLY:
                    return RTAServicesLibraryV2.C04_DetailsOfTheInsurance.Item2;
                case DETAILSOFTHEINSURANCE_OTHER:
                    return RTAServicesLibraryV2.C04_DetailsOfTheInsurance.Item3;
                default:
                    return RTAServicesLibraryV2.C04_DetailsOfTheInsurance.Item0;
            }
        }


        /// <summary>
        /// Details Of The Insurance Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="details"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C04_DetailsOfTheInsurance DetailsOfTheInsuranceValueV3(string details)
        {
            switch (details)
            {
                case DETAILSOFTHEINSURANCE_COMPREHENSIVE:
                    return RTAServicesLibraryV3.C04_DetailsOfTheInsurance.Item0;
                case DETAILSOFTHEINSURANCE_THIRDPARTYFIEANDTHEFT:
                    return RTAServicesLibraryV3.C04_DetailsOfTheInsurance.Item1;
                case DETAILSOFTHEINSURANCE_THIRDPARTYONLY:
                    return RTAServicesLibraryV3.C04_DetailsOfTheInsurance.Item2;
                case DETAILSOFTHEINSURANCE_OTHER:
                    return RTAServicesLibraryV3.C04_DetailsOfTheInsurance.Item3;
                default:
                    return RTAServicesLibraryV3.C04_DetailsOfTheInsurance.Item0;
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
        static public RTAServicesLibraryV2.C05_RepairsPosition RepairsPositionValue(string position)
        {
            switch (position)
            {
                case REPAIRSPOSITION_COMPLETE:
                    return RTAServicesLibraryV2.C05_RepairsPosition.Item0;
                case REPAIRSPOSITION_AUTHORISED:
                    return RTAServicesLibraryV2.C05_RepairsPosition.Item1;
                case REPAIRSPOSITION_NOTYETAUTHORISED:
                    return RTAServicesLibraryV2.C05_RepairsPosition.Item2;
                case REPAIRSPOSITION_NOTKNOWN:
                    return RTAServicesLibraryV2.C05_RepairsPosition.Item3;
                default:
                    return RTAServicesLibraryV2.C05_RepairsPosition.Item0;
            }
        }


        /// <summary>
        /// Repairs Position Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C05_RepairsPosition RepairsPositionValueV3(string position)
        {
            switch (position)
            {
                case REPAIRSPOSITION_COMPLETE:
                    return RTAServicesLibraryV3.C05_RepairsPosition.Item0;
                case REPAIRSPOSITION_AUTHORISED:
                    return RTAServicesLibraryV3.C05_RepairsPosition.Item1;
                case REPAIRSPOSITION_NOTYETAUTHORISED:
                    return RTAServicesLibraryV3.C05_RepairsPosition.Item2;
                case REPAIRSPOSITION_NOTKNOWN:
                    return RTAServicesLibraryV3.C05_RepairsPosition.Item3;
                default:
                    return RTAServicesLibraryV3.C05_RepairsPosition.Item0;
            }
        }

        #endregion C05_RepairsPosition

        #region C06_ClaimantType

        /// <summary>
        /// Claimant Type List
        /// </summary>
        /// <returns></returns>
        static public List<string> ClaimantTypeList()
        {
            List<string> list = new List<string>();
            list.Add(CLAIMANTTYPE_DRIVER);
            list.Add(CLAIMANTTYPE_OWNER);
            list.Add(CLAIMANTTYPE_PASSENGER);
            list.Add(CLAIMANTTYPE_PEDESTRIAN);
            list.Add(CLAIMANTTYPE_CYCLIST);
            list.Add(CLAIMANTTYPE_MOTORCYCLIST);
            list.Add(CLAIMANTTYPE_OTHER);
            return list;
        }

        /// <summary>
        /// Claimant Type Value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV2.C06_ClaimantType ClaimantTypeValue(string type)
        {
            switch (type)
            {
                case CLAIMANTTYPE_DRIVER:
                    return RTAServicesLibraryV2.C06_ClaimantType.Item0;
                case CLAIMANTTYPE_OWNER:
                    return RTAServicesLibraryV2.C06_ClaimantType.Item1;
                case CLAIMANTTYPE_PASSENGER:
                    return RTAServicesLibraryV2.C06_ClaimantType.Item2;
                case CLAIMANTTYPE_PEDESTRIAN:
                    return RTAServicesLibraryV2.C06_ClaimantType.Item3;
                case CLAIMANTTYPE_CYCLIST:
                    return RTAServicesLibraryV2.C06_ClaimantType.Item4;
                case CLAIMANTTYPE_MOTORCYCLIST:
                    return RTAServicesLibraryV2.C06_ClaimantType.Item5;
                case CLAIMANTTYPE_OTHER:
                    return RTAServicesLibraryV2.C06_ClaimantType.Item6;
                default:
                    return RTAServicesLibraryV2.C06_ClaimantType.Item0;
            }
        }


        /// <summary>
        /// Claimant Type Value
        /// DI:31.07.13
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C06_ClaimantType ClaimantTypeValueV3(string type)
        {
            switch (type)
            {
                case CLAIMANTTYPE_DRIVER:
                    return RTAServicesLibraryV3.C06_ClaimantType.Item0;
                case CLAIMANTTYPE_OWNER:
                    return RTAServicesLibraryV3.C06_ClaimantType.Item1;
                case CLAIMANTTYPE_PASSENGER:
                    return RTAServicesLibraryV3.C06_ClaimantType.Item2;
                case CLAIMANTTYPE_PEDESTRIAN:
                    return RTAServicesLibraryV3.C06_ClaimantType.Item3;
                case CLAIMANTTYPE_CYCLIST:
                    return RTAServicesLibraryV3.C06_ClaimantType.Item4;
                case CLAIMANTTYPE_MOTORCYCLIST:
                    return RTAServicesLibraryV3.C06_ClaimantType.Item5;
                case CLAIMANTTYPE_OTHER:
                    return RTAServicesLibraryV3.C06_ClaimantType.Item6;
                default:
                    return RTAServicesLibraryV3.C06_ClaimantType.Item0;
            }
        }

        #endregion C06_ClaimantType

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
        static public RTAServicesLibraryV2.C07_HospitalType HospitalTypeValue(string type)
        {
            switch (type)
            {
                case HOSPITALTYPE_NHS:
                    return RTAServicesLibraryV2.C07_HospitalType.Item0;
                case HOSPITALTYPE_PRIVATE:
                    return RTAServicesLibraryV2.C07_HospitalType.Item1;
                default:
                    return RTAServicesLibraryV2.C07_HospitalType.Item0;
            }
        }


        /// <summary>
        /// Hospital Type Value
        /// DI:31.07.13
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C07_HospitalType HospitalTypeValueV3(string type)
        {
            switch (type)
            {
                case HOSPITALTYPE_NHS:
                    return RTAServicesLibraryV3.C07_HospitalType.Item0;
                case HOSPITALTYPE_PRIVATE:
                    return RTAServicesLibraryV3.C07_HospitalType.Item1;
                default:
                    return RTAServicesLibraryV3.C07_HospitalType.Item0;
            }
        }

        #endregion C07_HospitalType

        #region C08_Sex

        /// <summary>
        /// Sex List
        /// </summary>
        /// <returns></returns>
        static public List<string> SexList()
        {
            List<string> list = new List<string>();
            list.Add(SEX_MALE);
            list.Add(SEX_FEMALE);
            list.Add(SEX_NOTKNOWN);
            return list;
        }

        /// <summary>
        /// Sex Value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV2.C08_Sex SexValue(string type)
        {
            switch (type)
            {
                case SEX_MALE:
                    return RTAServicesLibraryV2.C08_Sex.M;
                case SEX_FEMALE:
                    return RTAServicesLibraryV2.C08_Sex.F;
                case SEX_NOTKNOWN:
                    return RTAServicesLibraryV2.C08_Sex.N;
                default:
                    return RTAServicesLibraryV2.C08_Sex.N;
            }
        }


        /// <summary>
        /// Sex Value
        /// DI:31.07.13
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C08_Sex SexValueV3(string type)
        {
            switch (type)
            {
                case SEX_MALE:
                    return RTAServicesLibraryV3.C08_Sex.M;
                case SEX_FEMALE:
                    return RTAServicesLibraryV3.C08_Sex.F;
                case SEX_NOTKNOWN:
                    return RTAServicesLibraryV3.C08_Sex.N;
                default:
                    return RTAServicesLibraryV3.C08_Sex.N;
            }
        }

        #endregion C08_Sex

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
        static public RTAServicesLibraryV2.C09_SubjectStatus SubjectStatusValue(string status)
        {
            switch (status)
            {
                case STATUS_PERSONAL:
                    return RTAServicesLibraryV2.C09_SubjectStatus.P;
                case STATUS_BUSINESS:
                    return RTAServicesLibraryV2.C09_SubjectStatus.B;
                default:
                    return RTAServicesLibraryV2.C09_SubjectStatus.P;
            }
        }


        /// <summary>
        /// Subject Status Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C09_SubjectStatus SubjectStatusValueV3(string status)
        {
            switch (status)
            {
                case STATUS_PERSONAL:
                    return RTAServicesLibraryV3.C09_SubjectStatus.P;
                case STATUS_BUSINESS:
                    return RTAServicesLibraryV3.C09_SubjectStatus.B;
                default:
                    return RTAServicesLibraryV3.C09_SubjectStatus.P;
            }
        }

        #endregion C09_SubjectStatus

        #region C10_OtherParty

        /// <summary>
        /// Other Party List
        /// </summary>
        /// <returns></returns>
        static public List<string> OtherPartyList()
        {
            List<string> list = new List<string>();
            list.Add(OTHERPARTY_WITNESS);
            list.Add(OTHERPARTY_OTHER);
            return list;
        }

        /// <summary>
        /// Other Party Value
        /// </summary>
        /// <param name="party"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV2.C10_OtherParty OtherPartyValue(string party)
        {
            switch (party)
            {
                case OTHERPARTY_WITNESS:
                    return RTAServicesLibraryV2.C10_OtherParty.W;
                case OTHERPARTY_OTHER:
                    return RTAServicesLibraryV2.C10_OtherParty.O;
                default:
                    return RTAServicesLibraryV2.C10_OtherParty.O;
            }
        }


        /// <summary>
        /// Other Party Value
        /// DI:31.07.13
        /// </summary>
        /// <param name="party"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C10_OtherParty OtherPartyValueV3(string party)
        {
            switch (party)
            {
                case OTHERPARTY_WITNESS:
                    return RTAServicesLibraryV3.C10_OtherParty.W;
                case OTHERPARTY_OTHER:
                    return RTAServicesLibraryV3.C10_OtherParty.O;
                default:
                    return RTAServicesLibraryV3.C10_OtherParty.O;
            }
        }

        #endregion C10_OtherParty

        #region C11_SeatbeltWearing

        /// <summary>
        /// Seatbelt Wearing List
        /// </summary>
        /// <returns></returns>
        static public List<string> SeatbeltWearingList()
        {
            List<string> list = new List<string>();
            list.Add(SEATBELTWEARING_YES);
            list.Add(SEATBELTWEARING_NO);
            list.Add(SEATBELTWEARING_NOTKNOWN);
            return list;
        }

        /// <summary>
        /// Seatbelt Wearing Value
        /// </summary>
        /// <param name="wearing"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV2.C11_SeatbeltWearing SeatbeltWearingValue(string wearing)
        {
            switch (wearing)
            {
                case SEATBELTWEARING_NO:
                    return RTAServicesLibraryV2.C11_SeatbeltWearing.Item0;
                case SEATBELTWEARING_YES:
                    return RTAServicesLibraryV2.C11_SeatbeltWearing.Item1;
                case SEATBELTWEARING_NOTKNOWN:
                    return RTAServicesLibraryV2.C11_SeatbeltWearing.Item2;
                default:
                    return RTAServicesLibraryV2.C11_SeatbeltWearing.Item2;
            }
        }


        /// <summary>
        /// Seatbelt Wearing Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="wearing"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C11_SeatbeltWearing SeatbeltWearingValueV3(string wearing)
        {
            switch (wearing)
            {
                case SEATBELTWEARING_NO:
                    return RTAServicesLibraryV3.C11_SeatbeltWearing.Item0;
                case SEATBELTWEARING_YES:
                    return RTAServicesLibraryV3.C11_SeatbeltWearing.Item1;
                case SEATBELTWEARING_NOTKNOWN:
                    return RTAServicesLibraryV3.C11_SeatbeltWearing.Item2;
                default:
                    return RTAServicesLibraryV3.C11_SeatbeltWearing.Item2;
            }
        }

        #endregion C11_SeatbeltWearing

        #region C13_InsurerType

        /// <summary>
        /// Insurer Type List
        /// </summary>
        /// <returns></returns>
        static public List<string> InsurerTypeList()
        {
            List<string> list = new List<string>();
            list.Add(INSURERTYPE_INSURER);
            list.Add(INSURERTYPE_SELFINSURED);
            list.Add(INSURERTYPE_MIB);
            return list;
        }

        /// <summary>
        /// Insurer Type Value
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV2.C13_InsurerType InsurerTypeValue(string type)
        {
            switch (type)
            {
                case INSURERTYPE_INSURER:
                    return RTAServicesLibraryV2.C13_InsurerType.I;
                case INSURERTYPE_SELFINSURED:
                    return RTAServicesLibraryV2.C13_InsurerType.S;
                case INSURERTYPE_MIB:
                    return RTAServicesLibraryV2.C13_InsurerType.M;
                default:
                    return RTAServicesLibraryV2.C13_InsurerType.I;
            }
        }


        /// <summary>
        /// Insurer Type Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C13_InsurerType InsurerTypeValueV3(string type)
        {
            switch (type)
            {
                case INSURERTYPE_INSURER:
                    return RTAServicesLibraryV3.C13_InsurerType.I;
                case INSURERTYPE_SELFINSURED:
                    return RTAServicesLibraryV3.C13_InsurerType.S;
                case INSURERTYPE_MIB:
                    return RTAServicesLibraryV3.C13_InsurerType.M;
                default:
                    return RTAServicesLibraryV3.C13_InsurerType.I;
            }
        }

        #endregion C13_InsurerType

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
        static public RTAServicesLibraryV2.C14_YNDK TotalLossValue(string total)
        {
            switch (total)
            {
                case TOTALLOSS_YES:
                    return RTAServicesLibraryV2.C14_YNDK.Item0;
                case TOTALLOSS_NO:
                    return RTAServicesLibraryV2.C14_YNDK.Item1;
                case TOTALLOSS_DONTKNOW:
                    return RTAServicesLibraryV2.C14_YNDK.Item2;
                default:
                    return RTAServicesLibraryV2.C14_YNDK.Item2;
            }
        }


        /// <summary>
        /// Total Loss Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="total"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C14_YNDK TotalLossValueV3(string total)
        {
            switch (total)
            {
                case TOTALLOSS_YES:
                    return RTAServicesLibraryV3.C14_YNDK.Item0;
                case TOTALLOSS_NO:
                    return RTAServicesLibraryV3.C14_YNDK.Item1;
                case TOTALLOSS_DONTKNOW:
                    return RTAServicesLibraryV3.C14_YNDK.Item2;
                default:
                    return RTAServicesLibraryV3.C14_YNDK.Item2;
            }
        }

        #endregion C14_YNDK (Total Loss)

        #region C15_Capacity

        /// <summary>
        /// Capacity List
        /// </summary>
        /// <returns></returns>
        static public List<string> CapacityList()
        {
            List<string> list = new List<string>();
            list.Add(CAPACITYTYPE_CONTRACT);
            list.Add(CAPACITYTYPE_RTA);
            list.Add(CAPACITYTYPE_ARTICLE75);
            list.Add(CAPACITYTYPE_MIB);
            list.Add(CAPACITYTYPE_OTHER);
            return list;
        }

        /// <summary>
        /// Capacity Value
        /// </summary>
        /// <param name="capacity"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV2.C15_Capacity CapacityValue(string capacity)
        {
            switch (capacity)
            {
                case CAPACITYTYPE_CONTRACT:
                    return RTAServicesLibraryV2.C15_Capacity.Item0;
                case CAPACITYTYPE_RTA:
                    return RTAServicesLibraryV2.C15_Capacity.Item1;
                case CAPACITYTYPE_ARTICLE75:
                    return RTAServicesLibraryV2.C15_Capacity.Item2;
                case CAPACITYTYPE_MIB:
                    return RTAServicesLibraryV2.C15_Capacity.Item3;
                case CAPACITYTYPE_OTHER:
                    return RTAServicesLibraryV2.C15_Capacity.Item4;
                default:
                    return RTAServicesLibraryV2.C15_Capacity.Item0;
            }
        }


        /// <summary>
        /// Capacity Value
        /// </summary>
        /// <param name="capacity"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C15_Capacity CapacityValueV3(string capacity)
        {
            switch (capacity)
            {
                case CAPACITYTYPE_CONTRACT:
                    return RTAServicesLibraryV3.C15_Capacity.Item0;
                case CAPACITYTYPE_RTA:
                    return RTAServicesLibraryV3.C15_Capacity.Item1;
                case CAPACITYTYPE_ARTICLE75:
                    return RTAServicesLibraryV3.C15_Capacity.Item2;
                case CAPACITYTYPE_MIB:
                    return RTAServicesLibraryV3.C15_Capacity.Item3;
                case CAPACITYTYPE_OTHER:
                    return RTAServicesLibraryV3.C15_Capacity.Item4;
                default:
                    return RTAServicesLibraryV3.C15_Capacity.Item0;
            }
        }

        #endregion C15_Capacity

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
        static public RTAServicesLibraryV2.C16_SignatoryType SignatoryTypeValue(string type)
        {
            switch (type)
            {
                case SIGNATORYTYPE_CLAIMANT_SOLICITOR:
                    return RTAServicesLibraryV2.C16_SignatoryType.S;
                case SIGNATORYTYPE_CLAIMANT_IN_PERSON:
                    return RTAServicesLibraryV2.C16_SignatoryType.C;
                default:
                    return RTAServicesLibraryV2.C16_SignatoryType.S;
            }
        }


        /// <summary>
        /// Signatory Type Value
        /// DI:31.07.13
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C16_SignatoryType SignatoryTypeValueV3(string type)
        {
            switch (type)
            {
                case SIGNATORYTYPE_CLAIMANT_SOLICITOR:
                    return RTAServicesLibraryV3.C16_SignatoryType.S;
                case SIGNATORYTYPE_CLAIMANT_IN_PERSON:
                    return RTAServicesLibraryV3.C16_SignatoryType.C;
                default:
                    return RTAServicesLibraryV3.C16_SignatoryType.S;
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
        static public RTAServicesLibraryV2.C17_LiabilityDecision LiabilityDecisionValue(string decision)
        {
            switch (decision)
            {
                case LIABILITY_ADMITTED:
                    return RTAServicesLibraryV2.C17_LiabilityDecision.A;
                case LIABILITY_ADMITTEDWITHNEGLIGENCE:
                    return RTAServicesLibraryV2.C17_LiabilityDecision.AN;
                case LIABILITY_NOTADMITTED:
                    return RTAServicesLibraryV2.C17_LiabilityDecision.N;
                default:
                    return RTAServicesLibraryV2.C17_LiabilityDecision.A;
            }
        }


        /// <summary>
        /// Liability Decision Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="decision"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C17_LiabilityDecision LiabilityDecisionValueV3(string decision)
        {
            switch (decision)
            {
                case LIABILITY_ADMITTED:
                    return RTAServicesLibraryV3.C17_LiabilityDecision.A;
                case LIABILITY_ADMITTEDWITHNEGLIGENCE:
                    return RTAServicesLibraryV3.C17_LiabilityDecision.AN;
                case LIABILITY_NOTADMITTED:
                    return RTAServicesLibraryV3.C17_LiabilityDecision.N;
                default:
                    return RTAServicesLibraryV3.C17_LiabilityDecision.A;
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
            list.Add(LOSSTYPE_POLICYEXCESS);
            list.Add(LOSSTYPE_LOSSOFUSE);
            list.Add(LOSSTYPE_CARHIRE);
            list.Add(LOSSTYPE_REPAIRCOSTS);
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
        static public RTAServicesLibraryV2.C18_LossType LossTypeValue(string type)
        {
            switch (type)
            {
                case LOSSTYPE_POLICYEXCESS:
                    return RTAServicesLibraryV2.C18_LossType.Item0;
                case LOSSTYPE_LOSSOFUSE:
                    return RTAServicesLibraryV2.C18_LossType.Item1;
                case LOSSTYPE_CARHIRE:
                    return RTAServicesLibraryV2.C18_LossType.Item2;
                case LOSSTYPE_REPAIRCOSTS:
                    return RTAServicesLibraryV2.C18_LossType.Item3;
                case LOSSTYPE_FARES:
                    return RTAServicesLibraryV2.C18_LossType.Item4;
                case LOSSTYPE_MEDICALEXPENSES:
                    return RTAServicesLibraryV2.C18_LossType.Item5;
                case LOSSTYPE_CLOTHING:
                    return RTAServicesLibraryV2.C18_LossType.Item6;
                case LOSSTYPE_CARESERVICE:
                    return RTAServicesLibraryV2.C18_LossType.Item7;
                case LOSSTYPE_LOSSEARNINGSCLAIMANT:
                    return RTAServicesLibraryV2.C18_LossType.Item8;
                case LOSSTYPE_LOSSEARNINGSEMPLOYER:
                    return RTAServicesLibraryV2.C18_LossType.Item9;
                case LOSSTYPE_OTHERLOSSES:
                    return RTAServicesLibraryV2.C18_LossType.Item10;
                case LOSSTYPE_GENERALDAMAGES:
                    return RTAServicesLibraryV2.C18_LossType.Item11;
                default:
                    return RTAServicesLibraryV2.C18_LossType.Item10;
            }
        }


        /// <summary>
        /// Loss Type Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C18_LossType_R3 LossTypeValueV3(string type)
        {
            switch (type)
            {
                case LOSSTYPE_POLICYEXCESS:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item0;
                case LOSSTYPE_LOSSOFUSE:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item1;
                case LOSSTYPE_CARHIRE:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item2;
                case LOSSTYPE_REPAIRCOSTS:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item3;
                case LOSSTYPE_FARES:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item4;
                case LOSSTYPE_MEDICALEXPENSES:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item5;
                case LOSSTYPE_CLOTHING:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item6;
                case LOSSTYPE_CARESERVICE:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item7;
                case LOSSTYPE_LOSSEARNINGSCLAIMANT:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item8;
                case LOSSTYPE_LOSSEARNINGSEMPLOYER:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item9;
                case LOSSTYPE_OTHERLOSSES:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item10;
                case LOSSTYPE_GENERALDAMAGES:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item11;
                case LOSSTYPE_DISADVANTAGELABOURMARKET:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item13;
                case LOSSTYPE_LOSSCONGENIALEMPLOYMENT:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item14;
                case LOSSTYPE_FUTURELOSSES:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item15;
                default:
                    return RTAServicesLibraryV3.C18_LossType_R3.Item10;
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
        static public RTAServicesLibraryV2.C20_SettlementPackDecision SettlementPackDecisionValue(string decision)
        {
            switch (decision)
            {
                case SETTLEMENTPACKDECISION_CONFIRM:
                    return RTAServicesLibraryV2.C20_SettlementPackDecision.C;
                case SETTLEMENTPACKDECISION_COUNTEROFFER:
                    return RTAServicesLibraryV2.C20_SettlementPackDecision.CO;
                case SETTLEMENTPACKDECISION_REPUDIATE:
                    return RTAServicesLibraryV2.C20_SettlementPackDecision.R;
                default:
                    return RTAServicesLibraryV2.C20_SettlementPackDecision.C;
            }
        }

        /// <summary>
        /// Settlement Pack Decision Value
        /// DI:31.07.13 - Added
        /// </summary>
        /// <param name="yesno"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C20_SettlementPackDecision SettlementPackDecisionValueV3(string decision)
        {
            switch (decision)
            {
                case SETTLEMENTPACKDECISION_CONFIRM:
                    return RTAServicesLibraryV3.C20_SettlementPackDecision.C;
                case SETTLEMENTPACKDECISION_COUNTEROFFER:
                    return RTAServicesLibraryV3.C20_SettlementPackDecision.CO;
                case SETTLEMENTPACKDECISION_REPUDIATE:
                    return RTAServicesLibraryV3.C20_SettlementPackDecision.R;
                default:
                    return RTAServicesLibraryV3.C20_SettlementPackDecision.C;
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


        


        #region MedicalReportValueV2
        /// <summary>
        /// Medical Report Value
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV2.C22_MedicalReport MedicalReportValue(string report)
        {
            switch (report)
            {
                case MEDICALREPORT_0:
                    return RTAServicesLibraryV2.C22_MedicalReport.Item0;
                case MEDICALREPORT_1:
                    return RTAServicesLibraryV2.C22_MedicalReport.Item1;
                case MEDICALREPORT_2:
                    return RTAServicesLibraryV2.C22_MedicalReport.Item2;
                case MEDICALREPORT_3:
                    return RTAServicesLibraryV2.C22_MedicalReport.Item3;
                case MEDICALREPORT_4:
                    return RTAServicesLibraryV2.C22_MedicalReport.Item4;
                default:
                    return RTAServicesLibraryV2.C22_MedicalReport.Item0;
            }
        }
        #endregion MedicalReportValueV2


        /// <summary>
        /// Medical Report Value
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        static public RTAServicesLibraryV3.C22_MedicalReport MedicalReportValueV3(string report)
        {
            switch (report)
            {
                case MEDICALREPORT_0:
                    return RTAServicesLibraryV3.C22_MedicalReport.Item0;
                case MEDICALREPORT_1:
                    return RTAServicesLibraryV3.C22_MedicalReport.Item1;
                case MEDICALREPORT_2:
                    return RTAServicesLibraryV3.C22_MedicalReport.Item2;
                case MEDICALREPORT_3:
                    return RTAServicesLibraryV3.C22_MedicalReport.Item3;
                case MEDICALREPORT_4:
                    return RTAServicesLibraryV3.C22_MedicalReport.Item4;
                default:
                    return RTAServicesLibraryV3.C22_MedicalReport.Item0;
            }
        }

        #endregion C22_MedicalReport
    }
}
