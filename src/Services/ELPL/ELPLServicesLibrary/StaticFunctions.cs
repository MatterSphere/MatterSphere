using System;
using System.Data;
using System.Text.RegularExpressions;
using ELPLServicesLibrary.PIPService;

namespace ELPLServicesLibrary
{
    public class StaticFunctions 
    {
        #region Constructor

        /// <summary>
        /// Static Functions Constructor
        /// </summary>
        public StaticFunctions()
        {
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Populate User
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static claim PopulateClaim(DataRow row)
        {
            claim claim = new claim();

            claim.activityEngineGuid = (!string.IsNullOrEmpty(Convert.ToString(row["ActivityEngineGuid"]))) ? Convert.ToString(row["ActivityEngineGuid"]) : "";
            claim.applicationId = (!string.IsNullOrEmpty(Convert.ToString(row["ApplicationID"]))) ? Convert.ToString(row["ApplicationID"]) : "";
            claim.phaseCacheId = (!string.IsNullOrEmpty(Convert.ToString(row["PhaseCacheID"]))) ? Convert.ToString(row["PhaseCacheID"]) : "";
            claim.phaseCacheName = (!string.IsNullOrEmpty(Convert.ToString(row["PhaseCacheName"]))) ? Convert.ToString(row["PhaseCacheName"]) : "";
            claim.currentUserID = (!string.IsNullOrEmpty(Convert.ToString(row["CurrentUserID"]))) ? Convert.ToString(row["CurrentUserID"]) : "";
            claim.creationTime = DateFieldValue(row, "CreationTime");

            return claim;
        }

        #endregion Public Methods

        #region Validation Methods

        /// <summary>
        /// National Insurance Number Validation
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool NationalInsuraneNumberValidation(string number)
        {
            //  Characters 1-2 must be in the range AA-ZZ
            //  Character 1 must not be D, F, I, Q, U, V
            //  Character 2 must not be D, F, I, O, Q, U, V
            //  Characters 1-2 must not be one of the following combinations: FY; GB; NK; TN; TT; ZZ.
            //  Characters 3-6 must be in the range 0000 – 9999
            //  Characters 7-8 must be in the range 00 – 99
            //  Character 9 must be in the range A – D

            if (number.StartsWith("FY", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if (number.StartsWith("GB", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if (number.StartsWith("NK", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if (number.StartsWith("TN", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if (number.StartsWith("TT", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            else if (number.StartsWith("ZZ", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            Regex rgx = new Regex("^[A-CEGHJ-PRSTW-Z]{1}[A-CEGHJ-NPRSTW-Z]{1}[0-9]{6}[A-D]{0,1}$", RegexOptions.IgnoreCase);

            if (rgx.IsMatch(number))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// Email Address Validation
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool EmailAddressValidation(string email)
        {
            Regex rgx = new Regex(@"^(?("")("".+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$", RegexOptions.IgnoreCase);

            if (rgx.IsMatch(email))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Reference Number Validation
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static bool ReferenceNumberValidation(string number)
        {
            Regex rgx = new Regex(@"[|¦#$,£~\^`\[\]\{\}_€¬]", RegexOptions.IgnoreCase);

            if (rgx.IsMatch(number))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// PostCode Valdation
        /// </summary>
        /// <param name="postcode"></param>
        /// <returns></returns>
        public static bool PostCodeValidation(string postcode)
        {
            if (postcode.IndexOf(" ") == -1) // must be separated by a space
            {
                return false;
            }

            if (postcode.Length > 9)
            {
                return false;
            }

            if (!isFirstPartOfPostCodeValid(postcode.Substring(0, postcode.IndexOf(" "))))
            {
                return false;
            }

            if (!isSecondPartOfPostCodeValid(postcode.Remove(0, postcode.IndexOf(" ") + 1)))
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// second part of the post code must be in one of these formats:
        /// NAA
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool isSecondPartOfPostCodeValid(string secondPart)
        {
            bool firstCharIsNumeric = false;
            bool secondCharIsNumeric = false;
            bool thirdCharIsNumeric = false;

            switch (secondPart.Length)
            {
                case 3:
                    firstCharIsNumeric = IsNumeric(secondPart[0]);
                    secondCharIsNumeric = IsNumeric(secondPart[1]);
                    thirdCharIsNumeric = IsNumeric(secondPart[2]);

                    // NAA
                    if (firstCharIsNumeric && !secondCharIsNumeric && !thirdCharIsNumeric)
                    {
                        return true;
                    }

                    return false;

                default:
                    return false;
            }
        }

        
        /// <summary>
        /// first part of the post code must be in one of these formats:
        /// AN
        /// AAN
        /// AANA
        /// AANN
        /// ANA
        /// ANN
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private static bool isFirstPartOfPostCodeValid(string firstPart)
        {
            bool firstCharIsNumeric = false;
            bool secondCharIsNumeric = false;
            bool thirdCharIsNumeric = false;
            bool fourthCharIsNumeric = false;

            switch (firstPart.Length)
            {
                case 2: // AN
                    firstCharIsNumeric = IsNumeric(firstPart[0]);
                    secondCharIsNumeric = IsNumeric(firstPart[1]);

                    if (!firstCharIsNumeric && secondCharIsNumeric)
                    {
                        return true;
                    }

                    return false;

                case 3:
                    firstCharIsNumeric = IsNumeric(firstPart[0]);
                    secondCharIsNumeric = IsNumeric(firstPart[1]);
                    thirdCharIsNumeric = IsNumeric(firstPart[2]);

                    // AAN
                    if ((!firstCharIsNumeric && !secondCharIsNumeric && thirdCharIsNumeric)     // AAN
                        || (!firstCharIsNumeric && secondCharIsNumeric && !thirdCharIsNumeric)  // ANA
                        || (!firstCharIsNumeric && secondCharIsNumeric && thirdCharIsNumeric))  // ANN
                    {
                        return true;
                    }

                    return false;

                case 4:
                    firstCharIsNumeric = IsNumeric(firstPart[0]);
                    secondCharIsNumeric = IsNumeric(firstPart[1]);
                    thirdCharIsNumeric = IsNumeric(firstPart[2]);
                    fourthCharIsNumeric = IsNumeric(firstPart[3]);


                    if ((!firstCharIsNumeric && !secondCharIsNumeric && thirdCharIsNumeric && !fourthCharIsNumeric)     // AANA
                        || (!firstCharIsNumeric && !secondCharIsNumeric && thirdCharIsNumeric && fourthCharIsNumeric))  // AANN
                    {
                        return true;
                    }

                    return false;

                default:
                    return false;
            }
        }


        private static bool IsNumeric(object value)
        {
            try
            {
                int i = Convert.ToInt32(value.ToString());
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }


        /// <summary>
        /// Vehicle Registration Validation
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        public static bool VehicleRegistrationValidation(string registration)
        {
            Regex rgx = new Regex(@"^([A-Z]{3}\s?(\d{3}|\d{2}|d{1})\s?[A-Z])|([A-Z]\s?(\d{3}|\d{2}|\d{1})\s?[A-Z]{3})|(([A-HK-PRSVWY][A-HJ-PR-Y])\s?([0][2-9]|[1-9][0-9])\s?[A-HJ-PR-Z]{3})$", RegexOptions.IgnoreCase);

            if (rgx.IsMatch(registration))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion Validation Methods

        #region Conversion Methods

        /// <summary>
        /// Int32 Field Value
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static int Int32FieldValue(DataRow row, string fieldName)
        {
            if (row[fieldName] != DBNull.Value)
            {
                return Convert.ToInt32(row[fieldName]);
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Date Field Value
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static DateTime DateFieldValue(DataRow row, string fieldName)
        {
            if (row[fieldName] != DBNull.Value)
            {
                return Convert.ToDateTime(row[fieldName]);
            }
            else
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Bool Field Value
        /// </summary>
        /// <param name="row"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static bool BoolFieldValue(DataRow row, string fieldName)
        {
            if (row[fieldName] != DBNull.Value)
            {
                return Convert.ToBoolean(row[fieldName]);
            }
            else
            {
                return false;
            }
        }

        #endregion Conversion Methods
    }
}
