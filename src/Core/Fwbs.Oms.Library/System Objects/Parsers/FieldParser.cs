using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FWBS.OMS
{
    using Fwbs.Framework.ComponentModel.Composition;
    using Parsers;

    /// <summary>
    /// 8000 This is a class that is used to parse string field parameters into data from the
    /// OMS system.  This will allow for enquiry forms, search lists and precedents (or any
    /// other object wishing to use this) to retrieve information from specified objects 
    /// within the system.
    /// </summary>
    public class FieldParser
    {
        #region Fields

        public string FieldPrefix = "%";
        public string FieldSuffix = "%";

        private readonly ParserContextFactory factory = new ParserContextFactory();
        private Parsers.ParserContext _context;

        /// <summary>
        /// A collection of parameters which can be used to replace field values
        /// of custom information.
        /// </summary>
        private Common.KeyValueCollection _params = null;

        /// <summary>
        /// A dataset full of valid fields and macros.
        /// </summary>
        private static System.Data.DataSet _fields = null;

        #endregion

        #region Constants

        public const string FieldPrefixRequiredPrompt = "!@";
        public const string FieldPrefixPrompt = "!";
        public const string FieldPrefixSpecificData = "@@";
        public const string FieldPrefixClient = "CL";
        public const string FieldPrefixFeeEarner = "FEE";
        public const string FieldPrefixUser = "USR";
        public const string FieldPrefixAdditionalClient = "CAD";
        public const string FieldPrefixContactCompany = "COMP";
        public const string FieldPrefixContactIndividual = "IND";
        public const string FieldPrefixContact = "CONT";
        public const string FieldPrefixAdditionalFile = "FAD";
        public const string FieldPrefixFile = "FILE";
        public const string FieldPrefixPhase = "PH";
        public const string FieldPrefixRegInfo = "REG";
        public const string FieldPrefixBranchInfo = "BR";
        public const string FieldPrefixFunding = "FT";
        public const string FieldPrefixAssociate = "ASSOC";
        public const string FieldPrefixDocument = "DOC";
        public const string FieldPrefixDocumentVersion = "VER";
        public const string FieldPrefixPrecedent = "PREC";

        public const string FieldPrefixExtendedData = "$$";
        public const string FieldPrefixMacro = "#";
        public const string FieldPrefixReflection = "~";
        public const string FieldPrefixLookup = "LOOKUP";
        public const string FieldPrefixResource = "RESOURCE";
        public const string FieldPrefixMessage = "MESSAGE";
        public const string FieldPrefixAppointment = "APP";
        public const string FieldPrefixDateTime = "DATETIME";
        public const string FieldPrefixDataList = "DS_";
        public const string FieldPrefixSearchList = "SCH_";
        public const char FieldSeperator = ';';
        public const char FieldFilterJoin = '+';

        public const string NonMergeableSpecificDataPrefix = "__";
        #endregion

        #region Events

        /// <summary>
        /// An event that gets raised when UI intervention is needed to fill in information.
        /// </summary>
        public event FieldParserPromptHandler Prompt = null;


        /// <summary>
        /// An event that gets raised when UI intervention is needed to pick an object from a list.
        /// </summary>
        public event FieldParserListPromptHandler ListPrompt = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Using the default constructor will only take fields from the current session state.
        /// </summary>
        public FieldParser()
        {
            _context = (ParserContext)factory.CreateContext(null);

            GetFields();

            _context.Session.Container.Compose(this, true);

        }

        /// <summary>
        /// Creates an instance of the field parser with an additional object to extract information from.
        /// </summary>
        /// <param name="additionalObject">An extra object to extract information from.</param>
        public FieldParser(object additionalObject)
            : this()
        {
            ChangeObject(additionalObject);
        }


        #endregion

        #region Event Methods

        /// <summary>
        /// Raises the prompt event with the specified arguments.
        /// </summary>
        /// <param name="e">FieldParserPromptEventArgs event arguments.</param>
        protected void OnPrompt(FieldParserPromptEventArgs e)
        {
            if (Prompt != null)
                Prompt(this, e);
        }

        /// <summary>
        /// Raises the list prompt event with the specified arguments.
        /// </summary>
        protected void OnListPrompt(FieldParserListPromptEventArgs e)
        {
            if (ListPrompt != null)
                ListPrompt(this, e);
        }

        #endregion

        #region Parsers

        /// <summary>
        /// Parses the field name and returns back the data associated with it returning back
        /// some field name not found text if the field does not exist.
        /// </summary>
        /// <param name="fieldName">The field name to parse.</param>
        /// <returns>Data associated with the field.</returns>
        public object Parse(string fieldName)
        {
            return Parse(fieldName, false, null, false);
        }

        /// <summary>
        /// Parses the field name and returns back the data associated with it returning back
        /// some field name not found text if the field does not exist.
        /// </summary>
        /// <param name="formatting">Formats the return value.</param>
        /// <param name="fieldName">The field name to parse.</param>
        /// <returns>Data associated with the field.</returns>
        public object Parse(bool formatting, string fieldName)
        {
            return Parse(fieldName, false, null, formatting);
        }

        /// <summary>
        /// Parses the field name and returns back the data associated with it returning back
        /// a field name not found exception if the parameter is set to true if the field does 
        /// not exist.
        /// </summary>
        /// <param name="fieldName">The field name to parse.</param>
        /// <param name="throwException">If true an exception will be thrown.</param>
        /// <returns>Data associated with the field.</returns>
        public object Parse(string fieldName, bool throwException)
        {
            return Parse(fieldName, throwException, null, false);
        }

        /// <summary>
        /// Parses the field name and returns back the data associated with it returning back
        /// the specified default value if the field does not exist.
        /// </summary>
        /// <param name="fieldName">The field name to parse.</param>
        /// <param name="defaultValue">The default value to return if the field does not exist.</param>
        /// <returns>Data associated with the field.</returns>
        public object Parse(string fieldName, object defaultValue)
        {
            return Parse(fieldName, false, defaultValue, false);
        }

        /// <summary>
        /// Parses a string that converts legacy date time macros from input boxes.
        /// </summary>
        /// <param name="text">The text to convert.</param>
        /// <returns>Converted text, if any.</returns>
        private string ParseDateMacro(string text)
        {
            switch (text)
            {
                case "NOW":
                    return "%#NOW%";
                case "NOWSHORT":
                    return "%#NOWSHORT%";
                case "TODAY":
                    return "%#TODAY%";
                case "LONGDATETIME":
                    return "%#LONGDATETIME%";
                case "LONGDATETIMEMASK":
                    return "%#LONGDATETIME%";
                case "DATETIME":
                    return "%#DATETIME%";
                case "DATE":
                    return "%#DATE%";
                case "LONGTIME":
                    return "%#LONGTIME%";
                case "LONGDATE":
                    return "%#LONGDATE%";
                case "TIME":
                    return "%#TIME%";
                default:
                    return text;
            }
        }

        /// <summary>
        /// Parses the fieldname passed and returns back the data needed by the calling client.
        /// </summary>
        /// <param name="fieldName">The field nameto parse.</param>
        /// <param name="throwException">Throws an exception if the field does not exist.</param>
        /// <param name="defaultValue">Returns a default value if the field does not exist.</param>
        /// <param name="formatting">Uses formatting if specified.</param>
        /// <returns>The data the field name is matched up with.</returns>
        private object Parse(string fieldName, bool throwException, object defaultValue, bool formatting)
        {
            var context = (ParserContext)_context.Clone();



            string format = "";

            //Return value.
            object ret = "";

            //Potential error message to use if an exception gets raised some where.
            Exception exmsg = null;

            //Field conversion routine for common fields.
            fieldName = FieldToActualFieldConversion(".", fieldName, false, ref format).ToUpper();


            //STEP:		1 - CUSTOM NAMED PARAMETERS
            //******************************************************************************************************************
            if (_params != null && _params.Contains(fieldName))
            {
                ret = _params[fieldName].Value;
                goto Result;
            }


            //STEP:		2 -	REQUIRED PROMPTED FIELD
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixRequiredPrompt))
            {
                string cdcode = fieldName.Substring(FieldPrefixRequiredPrompt.Length);
                string message = context.Session.Resources.GetMessage(cdcode, "", "").Text;
                if (message.IndexOf('~') > -1) message = message.Replace("~", "");
                FieldParserPromptEventArgs prompt = new FieldParserPromptEventArgs(fieldName, true, message);
                OnPrompt(prompt);
                if (prompt.Result == null || Convert.ToString(prompt.Result).Trim() == "")
                {
                    exmsg = new OMSException2("REQUIREDINFO", "You Must enter a Value this is a Required Field!");
                    goto MissingField;

                }

                ret = ParseString(ParseDateMacro(Convert.ToString(prompt.Result)));
                goto Result;
            }

            //STEP:		3 - PROMPTED FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixPrompt))
            {
                string[] pars = fieldName.Split(FieldSeperator);

                //A list prompt is queried if there is an array of items within the fieldName.
                if (pars.Length > 0)
                {
                    /* Field Parameters
                     * ****************
                     * 0 = Search List Code / ASSOC	/ APPOINTMENT
                     * 1 = Additional filter to pass to list.
                     * 2 = Key value to link to other fields associated to the same item (state key value).
                     * 3 = Code lookup code or straight forward message.
                     * 4 = Field name 
                    */
                    string code = pars[0].Substring(FieldPrefixPrompt.Length);

                    if (pars.Length > 1)
                    {
                        string text = "";
                        object[] filter = new object[1];
                        string key = "";
                        string fld = "";

                        filter[0] = "";

                        if (pars.Length >= 5)
                        {
                            filter = pars[1].Split(FieldFilterJoin);
                            key = pars[2];
                            text = pars[3];
                            fld = pars[4];
                        }
                        else
                        {
                            fld = pars[pars.GetUpperBound(0)];
                        }



                        string message = "";
                        if (text != "") message = context.Session.Resources.GetMessage(text, "", "").Text;
                        if (message.IndexOf('~') > -1) message = message.Replace("~", "");

                        if (code == "ASSOCIATE") code = FieldPrefixAssociate;
                        if (code == "APPOINTMENT") code = FieldPrefixAppointment;

                        //DM - 20//1//03 - Quick adaption to code to make sure that there are two filter items for an associate field.
                        if (code == FieldPrefixAssociate && filter.Length == 1)
                        {
                            string[] assocf = new string[2];
                            filter.CopyTo(assocf, 0);
                            assocf[1] = "";
                            filter = assocf;
                        }

                        FieldParserListPromptEventArgs prompt = new FieldParserListPromptEventArgs(fieldName, message, code, key, filter);
                        OnListPrompt(prompt);

                        //Assign the new field name so that the code can go forward and deal with what it needs to.
                        fieldName = fld;

                        //If no result is returned then return empty string.
                        if (prompt.Result == null)
                        {
                            ret = "";
                            goto Result;
                        }

                        //If a key values pair collection is returned (a standard search list)
                        //then return the named pair value using the field name.
                        if (prompt.Result is FWBS.Common.KeyValueCollection)
                        {
                            try
                            {
                                FWBS.Common.KeyValueCollection rets = (FWBS.Common.KeyValueCollection)prompt.Result;
                                if (rets.Contains(fld) == false)
                                    goto MissingField;

                                ret = ((FWBS.Common.KeyValueCollection)prompt.Result)[fld].Value;
                                goto Result;
                            }
                            catch
                            {
                                goto MissingField;
                            }
                        }
                        //If the return value is an associate then sset the internal associate and contact 
                        //variable to the new value and carry on with the field parsing.
                        else if (prompt.Result is Associate)
                        {
                            context.Associate = (Associate)prompt.Result;
                            context.Contact = context.Associate.Contact;
                        }
                        //Any other object will be assigned to the global context.Data and the field parsing
                        //will continue.
                        else
                            context.Data = prompt.Result;

                    }
                    else
                    {
                        //Raise a normail inpur box message.
                        string message = context.Session.Resources.GetMessage(code, "", "").Text;
                        if (message.IndexOf('~') > -1) message = message.Replace("~", "");
                        FieldParserPromptEventArgs prompt = new FieldParserPromptEventArgs(fieldName, false, message);
                        OnPrompt(prompt);
                        ret = ParseString(ParseDateMacro(Convert.ToString(prompt.Result)));
                        goto Result;
                    }

                }

            }

            //STEP:		4 - REFLECTED FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixReflection))
            {
                try
                {
                    string path = fieldName.Substring(FieldPrefixReflection.Length);
                    string[] arrpath = path.Split('.');

                    object val = context.Data;

                    if (arrpath.Length > 0)
                    {
                        if (arrpath[0] == FieldPrefixRegInfo)
                            val = context.Session;
                        else if (arrpath[0] == FieldPrefixBranchInfo)
                            val = context.Session;
                        else if (arrpath[0] == FieldPrefixUser)
                            val = context.User;
                        else if (arrpath[0] == FieldPrefixFeeEarner)
                            val = context.FeeEarner;
                        else if (arrpath[0] == FieldPrefixContact)
                            val = context.Contact;
                        else if (arrpath[0] == FieldPrefixClient)
                            val = context.Client;
                        else if (arrpath[0] == FieldPrefixFile)
                            val = context.File;
                        else if (arrpath[0] == FieldPrefixPhase)
                            val = context.Phase;
                        else if (arrpath[0] == FieldPrefixAssociate)
                            val = context.Associate;
                        else if (arrpath[0] == FieldPrefixDocument)
                            val = context.Document;
                        else if (arrpath[0] == FieldPrefixDocumentVersion)
                            val = context.DocumentVersion;
                        else if (arrpath[0] == FieldPrefixPrecedent)
                            val = context.Precedent;
                        else
                            val = context.Data;

                        arrpath.SetValue("NULL", 0);
                    }


                    ret = ParseProperty(val, arrpath);
                    goto Result;


                }
                catch (Exception ex)
                {
                    exmsg = ex;
                    goto MissingField;
                }
            }


            //STEP:		4a -  PARENT OBJECT
            //******************************************************************************************************************
            if (fieldName == "THIS")
            {
                try
                {
                    ret = context.Data;
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		5 -  SESSION / REGINFO FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixRegInfo))
            {
                try
                {
                    ret = context.Session.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixRegInfo, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }


            //STEP:		6 -  BRANCH FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixBranchInfo))
            {
                try
                {
                    Branch br = context.Session;
                    ret = br.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixBranchInfo, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		7 -  SPECIFIC DATA FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixSpecificData))
            {
                if (fieldName.StartsWith(string.Format("{0}{1}", FieldPrefixSpecificData, NonMergeableSpecificDataPrefix)))
                    ret = context.Session.Resources.GetResource("UNMERGESPECIFIC", "This specific data is secure and cannot be merged", "").Text;
                else
                    ret = context.Session.GetSpecificData(FieldToActualFieldConversion(FieldPrefixSpecificData, fieldName, true, ref format));

                goto Result;
            }


            //STEP:		8 -  USER FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixUser))
            {
                try
                {
                    ret = context.User.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixUser, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }


            //STEP:		9 -  FEE EARNER FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixFeeEarner))
            {
                try
                {
                    ret = GetFeeEarnerContext(context).GetExtraInfo(FieldToActualFieldConversion(FieldPrefixFeeEarner, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }


            //STEP:		10 -  CONTACT FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixContact))
            {
                try
                {
                    ret = context.Contact.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixContact, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		11 -  ADDITIONAL COMPANY CONTACT FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixContactCompany))
            {
                try
                {
                    ret = context.Contact.ExtendedData[Contact.Ext_Company].GetExtendedData(FieldToActualFieldConversion(FieldPrefixContactCompany, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		12 -  ADDITIONAL INDIVIDUAL CONTACT FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixContactIndividual))
            {
                try
                {
                    ret = context.Contact.ExtendedData[Contact.Ext_Individual].GetExtendedData(FieldToActualFieldConversion(FieldPrefixContactIndividual, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		13 -  CLIENT FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixClient))
            {
                try
                {
                    ret = context.Client.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixClient, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		14 -  ADDITIONAL CLIENT FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixAdditionalClient) || fieldName.StartsWith("AD"))
            {
                try
                {
                    ret = context.Client.ExtendedData["ADDCLIENTINFO"].GetExtendedData(FieldToActualFieldConversion(FieldPrefixAdditionalClient, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }


            //STEP:		15 -  FILE / MATTER FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixFile) || fieldName.StartsWith("MAT"))
            {
                try
                {
                    ret = context.File.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixFile, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		16 -  CURRENT FILE PHASE FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixPhase))
            {
                try
                {
                    ret = context.Phase.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixPhase, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		17 -  ADDITIONAL FILE / MATTER FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixAdditionalFile) && fieldName.StartsWith("MAD"))
            {
                try
                {
                    ret = context.File.ExtendedData["ADDFILEINFO"].GetExtendedData(FieldToActualFieldConversion(FieldPrefixAdditionalFile, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		18 -  FUNDING FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixFunding))
            {
                string newName = fieldName.Replace(FieldPrefixFunding, FieldPrefixFile);

                try
                {
                    ret = context.File.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixFile, newName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		19 -  ASSOCIATE FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixAssociate))
            {
                try
                {
                    ret = context.Associate.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixAssociate, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		20 -  DOCUMENT FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixDocument))
            {
                try
                {
                    ret = context.Document.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixDocument, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		21 -  DOCUMENT VERSION FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixDocumentVersion))
            {
                try
                {
                    ret = context.DocumentVersion.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixDocumentVersion, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }


            //STEP:		22 -  PRECEDENT FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixPrecedent))
            {
                try
                {
                    ret = context.Precedent.GetExtraInfo(FieldToActualFieldConversion(FieldPrefixPrecedent, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		23 -  EXTENDED DATA FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixExtendedData))
            {
                try
                {
                    string path = FieldToActualFieldConversion(FieldPrefixExtendedData, fieldName, true, ref format);
                    if (path.StartsWith(FieldPrefixExtendedData))
                        path = path.Substring(FieldPrefixExtendedData.Length);
                    string[] arrpath = path.Split('.');

                    object val = context.Data;

                    if (arrpath.Length > 0)
                    {
                        if (arrpath[0] == FieldPrefixContact)
                            val = context.Contact;
                        else if (arrpath[0] == FieldPrefixClient)
                            val = context.Client;
                        else if (arrpath[0] == FieldPrefixFile)
                            val = context.File;
                        else if (arrpath[0] == FieldPrefixAssociate)
                            val = context.Associate;
                        else if (arrpath[0] == FieldPrefixFeeEarner)
                            val = context.FeeEarner;
                        else if (arrpath[0] == FieldPrefixUser)
                            val = context.User;
                        else if (arrpath[0] == FieldPrefixDocument)
                            val = context.Document;
                        else
                            val = context.Data;
                    }

                    ret = ((Interfaces.IExtendedDataCompatible)val).ExtendedData[arrpath[1]].GetExtendedData(arrpath[2]);

                    goto Result;

                }
                catch (Exception ex)
                {
                    exmsg = ex;
                    goto MissingField;
                }
            }

            //STEP:		24 -  MACROS (Backward compatibility)
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixMacro))
            {
                try
                {
                    string tmp = fieldName.Substring(FieldPrefixMacro.Length);
                    string[] arr = tmp.Split(FieldSeperator);
                    if (arr[0].StartsWith(FieldPrefixLookup) && arr.Length < 3) arr = tmp.Split(' ');

                    //SIGNOFF MACROS
                    if (arr[0].StartsWith("SIGNOFF"))
                    {
                        string[] faithfully_deciders = context.Session.GetSessionConfigSetting("SIGNOFFLIST", @"SIR AND MADAM@@SIR & MADAM@@SIRS@@MADAM@@SIR@@MADAMS@@MESDAMES@@SIR/MADAM@@SIRS/MADAM@@SIRS,@@MADAMS,@@SIR/MADAM,@@SIR & MADAM,@@SIR AND MADAM,@@").ToUpper().Split('@');
                        string salutation = context.Associate.Salutation.Trim().ToUpper();
                        string space = Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;
                        bool use_faithfully = false;

                        if (salutation != String.Empty)
                        {
                            if (Array.IndexOf(faithfully_deciders, salutation) > -1)
                            {
                                use_faithfully = true;
                                ret = context.Session.Resources.GetResource("SIGNOFFSIR", "Yours faithfully", "", false).Text + space;
                            }
                            else
                                ret = context.Session.Resources.GetResource("SIGNOFFNOTSIR", "Yours sincerely", "", false).Text + space;
                        }

                        switch (arr[0])
                        {

                            case "SIGNOFF":
                                if (use_faithfully)
                                { }
                                else
                                {
                                    if (GetFeeEarnerContext(context).AdditionalSignOff.Length > 0)
                                        ret += (GetFeeEarnerContext(context).SignOff + Environment.NewLine + GetFeeEarnerContext(context).AdditionalSignOff);
                                    else
                                        ret += GetFeeEarnerContext(context).SignOff;
                                }
                                break;

                            case "SIGNOFFNONAME":
                                if (use_faithfully)
                                { }
                                else
                                { }
                                break;
                            case "SIGNOFFWITHNAME":
                                if (use_faithfully)
                                {
                                    if (GetFeeEarnerContext(context).AdditionalSignOff.Length > 0)
                                        ret += (GetFeeEarnerContext(context).SignOff + Environment.NewLine + GetFeeEarnerContext(context).AdditionalSignOff);
                                    else
                                        ret += GetFeeEarnerContext(context).SignOff;
                                }
                                else
                                {
                                    if (GetFeeEarnerContext(context).AdditionalSignOff.Length > 0)
                                        ret += (GetFeeEarnerContext(context).SignOff + Environment.NewLine + GetFeeEarnerContext(context).AdditionalSignOff);
                                    else
                                        ret += GetFeeEarnerContext(context).SignOff;
                                }
                                break;
                            case "SIGNOFFFEECOMPUCASE":
                                if (use_faithfully)
                                    ret += context.Session.CompanyName.ToUpper();
                                else
                                {
                                    if (GetFeeEarnerContext(context).AdditionalSignOff.Length > 0)
                                        ret += (GetFeeEarnerContext(context).SignOff + Environment.NewLine + GetFeeEarnerContext(context).AdditionalSignOff);
                                    else
                                        ret += GetFeeEarnerContext(context).SignOff;
                                }
                                break;
                            case "SIGNOFFFEECOMP":
                                if (use_faithfully)
                                    ret += context.Session.CompanyName;
                                else
                                {
                                    if (GetFeeEarnerContext(context).AdditionalSignOff.Length > 0)
                                        ret += (GetFeeEarnerContext(context).SignOff + Environment.NewLine + GetFeeEarnerContext(context).AdditionalSignOff);
                                    else
                                        ret += GetFeeEarnerContext(context).SignOff;
                                }
                                break;

                            case "SIGNOFFFILEFEEWITHNAME":
                                if (use_faithfully)
                                {
                                    if (context.Associate.OMSFile.PrincipleFeeEarner.AdditionalSignOff.Length > 0)
                                        ret += (context.Associate.OMSFile.PrincipleFeeEarner.SignOff + Environment.NewLine + context.Associate.OMSFile.PrincipleFeeEarner.AdditionalSignOff);
                                    else
                                        ret += context.Associate.OMSFile.PrincipleFeeEarner.SignOff;
                                }
                                else
                                {
                                    if (context.Associate.OMSFile.PrincipleFeeEarner.AdditionalSignOff.Length > 0)
                                        ret += (context.Associate.OMSFile.PrincipleFeeEarner.SignOff + Environment.NewLine + context.Associate.OMSFile.PrincipleFeeEarner.AdditionalSignOff);
                                    else
                                        ret += context.Associate.OMSFile.PrincipleFeeEarner.SignOff;
                                }
                                break;
                            case "SIGNOFFFILEFEECOMPUCASE":
                                if (use_faithfully)
                                    ret += context.Session.CompanyName.ToUpper();
                                else
                                {
                                    if (context.Associate.OMSFile.PrincipleFeeEarner.AdditionalSignOff.Length > 0)
                                        ret += (context.Associate.OMSFile.PrincipleFeeEarner.SignOff + Environment.NewLine + context.Associate.OMSFile.PrincipleFeeEarner.AdditionalSignOff);
                                    else
                                        ret += context.Associate.OMSFile.PrincipleFeeEarner.SignOff;
                                }
                                break;
                            case "SIGNOFFFILEFEECOMP":
                                if (use_faithfully)
                                    ret += context.Session.CompanyName;
                                else
                                {
                                    if (context.Associate.OMSFile.PrincipleFeeEarner.AdditionalSignOff.Length > 0)
                                        ret += (context.Associate.OMSFile.PrincipleFeeEarner.SignOff + Environment.NewLine + context.Associate.OMSFile.PrincipleFeeEarner.AdditionalSignOff);
                                    else
                                        ret += context.Associate.OMSFile.PrincipleFeeEarner.SignOff;
                                }
                                break;
                            default:
                                goto MissingField;
                        }
                    }

                    else
                    {
                        switch (arr[0])
                        {
                            case "SALUTATION":	//Possible multi address problems if called after addressee
                                ret = context.Associate.Salutation;
                                break;
                            case "MATTHEIRREF":
                                ret = context.Associate.TheirRef;
                                break;
                            case "OURREF":
                                ret = GetFeeEarnerContext(context).Initials + GetFeeEarnerContext(context).AdditionalReference + "/" + context.User.Initials + "/" + context.Client.ClientNo + "-" + context.File.FileNo;
                                break;

                            case "MATTYPE":
                                ret = context.File.GetOMSType().Description;
                                break;
                            case "ASSOCADDRESS":
                                ret = context.Associate.GetAssocAddress(Environment.NewLine);
                                break;
                            case "ADDRESSEE":
                                goto case "ASSOCADDRESS";
                            case "REF":
                                if (context.Associate.AssocHeading == "")
                                {
                                    FieldParserPromptEventArgs e = new FieldParserPromptEventArgs(fieldName, false, context.Session.Resources.GetResource("PLZHEAD4LET", "Please Enter a Heading for this Letter?", "", false).Text);
                                    OnPrompt(e);
                                    ret = e.Result;
                                }
                                else
                                    ret = context.Associate.AssocHeading;
                                break;
                            case "FEEFULLNAME":
                                ret = GetFeeEarnerContext(context).FullName;
                                break;
                            case "REGVATNUMBER":
                                ret = context.Session.CurrentBranch.VATNumber;
                                break;
                            case "FEEINITS":
                                ret = GetFeeEarnerContext(context).Initials;
                                break;
                            case "USERINITS":
                                ret = context.User.Initials;
                                break;
                            case "USERFULLNAME":
                                ret = context.User.FullName;
                                break;
                            case "REGCOMPANYNAME":
                                ret = context.Session.CompanyName;
                                break;
                            case "REGTELEPHONE":
                                ret = context.Session.CurrentBranch.Telephone;
                                break;
                            case "REGADDRESS":
                                ret = context.Session.CurrentBranch.Address.GetAddressString(Environment.NewLine);
                                break;
                            case "REGFAX":
                                ret = context.Session.CurrentBranch.Fax;
                                break;
                            case "LONGDATETIME":
                                ret = System.DateTime.Now.ToString("f");
                                break;
                            case "LONGDATETIMEMASK":
                                ret = System.DateTime.Now.ToString("f");
                                break;
                            case "DATETIME":
                                ret = System.DateTime.Now.ToString("g");
                                break;
                            case "DATE":
                                ret = System.DateTime.Now.ToString("d");
                                break;
                            case "LONGDATE":
                                ret = System.DateTime.Now.ToString("D");
                                break;
                            case "LONGTIME":
                                ret = System.DateTime.Now.ToString("T");
                                break;
                            case "TIME":
                                ret = System.DateTime.Now.ToString("t");
                                break;
                            //UTCFIX: DM - 30/11/06 - Added UTC compatible field formats.
                            case "UTCLONGDATETIME":
                                ret = System.DateTime.UtcNow.ToString("f");
                                break;
                            case "UTCLONGDATETIMEMASK":
                                ret = System.DateTime.UtcNow.ToString("f");
                                break;
                            case "UTCDATETIME":
                                ret = System.DateTime.UtcNow.ToString("g");
                                break;
                            case "UTCDATE":
                                ret = System.DateTime.UtcNow.ToString("d");
                                break;
                            case "UTCLONGDATE":
                                ret = System.DateTime.UtcNow.ToString("D");
                                break;
                            case "UTCLONGTIME":
                                ret = System.DateTime.UtcNow.ToString("T");
                                break;
                            case "UTCTIME":
                                ret = System.DateTime.UtcNow.ToString("t");
                                break;
                            case "ADDRESSONELINE":
                                string retadd = context.Associate.GetAssocAddress(", ");
                                if (retadd.EndsWith(", "))
                                {
                                    retadd = retadd.Remove(retadd.Length - 2, 2);
                                }
                                ret = retadd;
                                break;
                            case "UI":
                                ret = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
                                break;
                            case "NOW":
                                ret = System.DateTime.Now.ToString("f");
                                break;
                            case "NOWSHORT":
                                ret = System.DateTime.Now.ToString("g");
                                break;
                            case "TODAY":
                                ret = System.DateTime.Now.ToString("D");
                                break;
                            //UTCFIX: DM - 30/11/06 - Added UTC compatible field formats.
                            case "UTCNOW":
                                ret = System.DateTime.UtcNow.ToString("f");
                                break;
                            case "UTCNOWSHORT":
                                ret = System.DateTime.UtcNow.ToString("g");
                                break;
                            case "UTCTODAY":
                                ret = System.DateTime.UtcNow.ToString("D");
                                break;
                            case FieldPrefixLookup:
                                {
                                    string fieldval = Convert.ToString(Parse(arr[1]));
                                    string type = arr[2];
                                    string culture = context.Session.DefaultCultureInfo.Name;
                                    if (context.File == null)
                                    {
                                        if (context.Client != null) culture = context.Client.PreferedLanguage;
                                    }
                                    else
                                        culture = context.File.PreferedLanguage;

                                    ret = CodeLookup.GetLookup(culture, type, fieldval, "");

                                    break;
                                }
                            case FieldPrefixResource:
                                {
                                    string fieldval = Convert.ToString(arr[1]);
                                    string fielddef = Convert.ToString(arr[2]);
                                    ret = context.Session.Resources.GetResource(fieldval, fielddef, "").Text;
                                }
                                break;
                            case FieldPrefixMessage:
                                {
                                    string fieldval = Convert.ToString(arr[1]);
                                    string fielddef = Convert.ToString(arr[2]);
                                    ret = context.Session.Resources.GetMessage(fieldval, fielddef, "").Text;
                                }
                                break;
                            default:
                                goto MissingField;
                        }
                    }
                    goto Result;

                }
                catch (Exception ex)
                {
                    exmsg = ex;
                    goto MissingField;
                }

            }

            //STEP:		25 -  APPOINTMENT FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixAppointment) && context.Data is FWBS.OMS.Appointment)
            {
                try
                {
                    ret = ((Appointment)context.Data).GetExtraInfo(FieldToActualFieldConversion(FieldPrefixAppointment, fieldName, true, ref format));
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		26 -  DATA LIST FIELDS
            //******************************************************************************************************************

            if (fieldName.StartsWith(FieldPrefixDataList))
            {
                try
                {
                    EnquiryEngine.DataLists dl = new EnquiryEngine.DataLists(fieldName.Substring(FieldPrefixDataList.Length));
                    dl.ChangeParent(context.Associate);
                    dl.ChangeParameters(_params);

                    System.Data.DataTable dt = dl.Run() as System.Data.DataTable;
                    ret = dt.DefaultView;

                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }


            }

            //STEP:		27 -  SEARCH LIST FIELDS
            //******************************************************************************************************************
            if (fieldName.StartsWith(FieldPrefixSearchList))
            {
                try
                {

                    SearchEngine.SearchList sch = new SearchEngine.SearchList(fieldName.Substring(FieldPrefixSearchList.Length), context.Associate, _params);
                    System.Data.DataTable dt = sch.Search(false) as System.Data.DataTable;

                    //Only display the display columns.
                    System.Data.DataTable cols = sch.ListView;

                    string[] displayCols = new string[cols.Rows.Count];

                    for (int ctr = 0; ctr < cols.Rows.Count; ctr++)
                    {
                        System.Data.DataRow row = cols.Rows[ctr];
                        displayCols[ctr] = row["lvmapping"].ToString().ToUpper();
                        if (dt.Columns.Contains(displayCols[ctr]))
                        {
                            System.Data.DataColumn c = dt.Columns[displayCols[ctr]];
                            c.Caption = Session.CurrentSession.Terminology.Parse(row["lvdesc"].ToString(), true);

                            try
                            {
                                string fmt = Convert.ToString(row["lvformat"]);
                                switch (fmt)
                                {
                                    case "Number":
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "F");
                                            c.ExtendedProperties.Add("ALIGNMENT", "R");
                                        }
                                        break;
                                    case "Currency":
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "c");
                                            c.ExtendedProperties.Add("ALIGNMENT", "R");
                                        }
                                        break;
                                    case "DateLongLft":
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "D");
                                            c.ExtendedProperties.Add("ALIGNMENT", "L");
                                        }
                                        break;
                                    case "DateLongRgt":
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "D");
                                            c.ExtendedProperties.Add("ALIGNMENT", "R");
                                        }
                                        break;
                                    case "DateTimeLft":
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "g");
                                            c.ExtendedProperties.Add("ALIGNMENT", "L");
                                        }
                                        break;
                                    case "DateTimeRgt":
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "g");
                                            c.ExtendedProperties.Add("ALIGNMENT", "R");
                                        }
                                        break;
                                    case "DateLft":
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "d");
                                            c.ExtendedProperties.Add("ALIGNMENT", "L");
                                        }
                                        break;
                                    case "Date":
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "d");
                                            c.ExtendedProperties.Add("ALIGNMENT", "R");
                                        }
                                        break;
                                    case "DateRgt":
                                        goto case "Date";
                                    case "Time":
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "t");
                                            c.ExtendedProperties.Add("ALIGNMENT", "R");
                                        }
                                        break;
                                    case "TimeLft":
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "t");
                                            c.ExtendedProperties.Add("ALIGNMENT", "L");
                                        }
                                        break;
                                    case "TimeRgt":
                                        goto case "Time";
                                    case "RightAlign":
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "");
                                            c.ExtendedProperties.Add("ALIGNMENT", "R");
                                        }
                                        break;
                                    default:
                                        {
                                            c.ExtendedProperties.Add("FORMAT", "");
                                            c.ExtendedProperties.Add("ALIGNMENT", "R");
                                        }
                                        break;

                                }

                            }
                            catch { }
                        }
                    }

                    for (int ctr = dt.Columns.Count - 1; ctr >= 0; ctr--)
                    {
                        if (Array.IndexOf(displayCols, dt.Columns[ctr].ColumnName.ToUpper()) < 0)
                            dt.Columns.Remove(dt.Columns[ctr]);
                    }

                    ret = dt.DefaultView;
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //STEP:		28 -  FieldParsers
            //******************************************************************************************************************
            if (Parsers != null)
            {
                foreach (var parser in Parsers)
                {
                    if (parser.Value.CanHandle(fieldName))
                    {
                        try
                        {

                            var val = parser.Value.Parse(context, fieldName);

                            ret = val.Value;

                            goto Result;
                        }
                        catch
                        {
                            goto MissingField;
                        }
                    }
                }
            }

            //STEP:		29 -  GLOBAL OBJECT FIELDS
            //******************************************************************************************************************

            if (context.Data is Interfaces.IExtraInfo)
            {
                try
                {
                    ret = ((Interfaces.IExtraInfo)context.Data).GetExtraInfo(fieldName);
                    goto Result;
                }
                catch
                {
                    goto MissingField;
                }
            }

            //Go to the missing field if the code gets this far. This means that there are field matches.
            goto MissingField;
        Result:

            if (formatting)
                return GetFormattedValue(ret, context.NumberFormat, context.DateTimeFormat, format);
            else
                return ret;

        MissingField:
            if (defaultValue == null)
            {
                if (throwException)
                {
                    if (exmsg == null)
                        throw new MissingFieldException(fieldName + " : " + NotFoundText);
                    else
                        throw exmsg;
                }
                else
                {
                    if (exmsg == null)
                        return fieldName + " : " + NotFoundText;
                    else
                        return exmsg.Message;
                }
            }
            else
                return defaultValue;
        }


        /// <summary>
        /// Returns the Fee Earner for field parsing
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static FeeEarner GetFeeEarnerContext(ParserContext context)
        {
            if (FWBS.OMS.Session.CurrentSession.CurrentUser.WorksForMatterHandler)
            {
                //Added to fix issue when then 
                if (context.File == null)
                    return context.FeeEarner;
                else
                    return context.File.PrincipleFeeEarner;
            }
            else
                return context.FeeEarner;
        }

        /// <summary>
        /// Parses a piece of text and replaces any fields that need to be replaced
        /// from a Data Row
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="dr">Data Row to Scan Field</param>
        /// <returns>The original parsed string.</returns>
        public string ParseString(string text, System.Data.DataRow dr)
        {
            string fieldregx = FieldPrefix + "(?<FieldName>.*?)" + FieldSuffix;
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(text, fieldregx, System.Text.RegularExpressions.RegexOptions.Multiline);

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                string key = match.Groups["FieldName"].Value;
                if (dr.Table.Columns.Contains(key) == true)
                {
                    object val = dr[key];
                    if (dr[key] is DateTime)
                    {
                        //UTCFIX: DM - 30/11/06 - Make sure that the default date is returning local.
                        DateTime dteval = Convert.ToDateTime(val);
                        switch (dteval.Kind)
                        {
                            case DateTimeKind.Utc:
                                text = text.Replace(match.Value, dteval.ToLocalTime().ToString("d"));
                                break;
                            default:
                                text = text.Replace(match.Value, dteval.ToString("d"));
                                break;
                        }
                    }
                    else
                        text = text.Replace(match.Value, Convert.ToString(val));
                }
            }

            return text;
        }

        /// <summary>
        /// Parses a string using a replacement default value if the item soes not exist.
        /// </summary>
        public string ParseString(string text, string defaultValue)
        {
            string fieldregx = FieldPrefix + "(?<FieldName>.*?)" + FieldSuffix;
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(text, fieldregx, System.Text.RegularExpressions.RegexOptions.Multiline);

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                string key = match.Groups["FieldName"].Value;
                object val = Parse(key, defaultValue);
                text = text.Replace(match.Value, Convert.ToString(val));
            }

            return text;
        }

        /// <summary>
        /// Parses a piece of text and replaces any fields that need to be replaced.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="replaceFields">Replaces the strings fields if specified.</param>
        /// <param name="throwException"></param>
        /// <param name="fields">The ourputted fields that may be used for replacement.</param>
        /// <returns>The original parsed string.</returns>
        public string ParseString(string text, bool replaceFields, bool throwException, out FWBS.Common.KeyValueCollection fields)
        {
            fields = new FWBS.Common.KeyValueCollection();
            string fieldregx = FieldPrefix + "(?<FieldName>.*?)" + FieldSuffix;
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(text, fieldregx, System.Text.RegularExpressions.RegexOptions.Multiline);

            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                string key = match.Groups["FieldName"].Value;
                if (fields.Contains(key) == false)
                {
                    object val = Parse(key, throwException);
                    fields.Add(key, val);
                    if (replaceFields)
                        text = text.Replace(match.Value, Convert.ToString(val));
                }
            }

            return text;
        }

        /// <summary>
        /// Parses a piece of text and replaces any fields that need to be replaced.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <param name="throwException">Throw Exception</param>
        /// <returns>The original string.</returns>
        public string ParseString(string text, bool throwException)
        {
            FWBS.Common.KeyValueCollection fields;
            return ParseString(text, true, throwException, out fields);
        }

        /// <summary>
        /// Parses a piece of text and replaces any fields that need to be replaced.
        /// </summary>
        /// <param name="text">The text to parse.</param>
        /// <returns>The original string.</returns>
        public string ParseString(string text)
        {
            FWBS.Common.KeyValueCollection fields;
            return ParseString(text, true, false, out fields);
        }

        /// <summary>
        /// Parses the object dot notation to get a property value.
        /// </summary>
        /// <param name="obj">the object to access the property from.</param>
        /// <param name="properties">Dot notation of the property.</param>
        /// <returns></returns>
        private object ParseProperty(object obj, string[] properties)
        {
            Type t = obj.GetType();
            System.Reflection.PropertyInfo prop = null;
            object val = obj;

            for (int ctr = 0; ctr < properties.Length; ctr++)
            {
                if (properties[ctr] == "NULL") continue;
                prop = t.GetProperty(properties[ctr], System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase);
                t = prop.PropertyType;
                val = prop.GetValue(val, null);
            }

            return val;
        }


        /// <summary>
        /// Converts the old OMS2k field names into the new OMS.NET names.
        /// </summary>
        /// <param name="type">The type of object associated.</param>
        /// <param name="fieldName">The field name to compare.</param>
        /// <param name="useAlias">Uses the alias of the field actual field conversion.</param>
        /// <param name="format">Output parameter holding the format style.</param>
        /// <returns>The new fieldname to use.</returns>
        private string FieldToActualFieldConversion(string type, string fieldName, bool useAlias, ref string format)
        {
            System.Data.DataTable dt = GetFields(type);
            dt.DefaultView.RowFilter = "fldold = '" + Common.SQLRoutines.RemoveRubbish(fieldName) + "'";
            if (dt.DefaultView.Count > 0)
            {
                format = Convert.ToString(dt.DefaultView[0]["fldformat"]);
                string alias = Convert.ToString(dt.DefaultView[0]["fldalias"]);
                if (alias == String.Empty || useAlias == false)
                    return Convert.ToString(dt.DefaultView[0]["fldname"]);
                else
                    return alias;
            }
            else
            {
                dt.DefaultView.RowFilter = "fldname = '" + Common.SQLRoutines.RemoveRubbish(fieldName) + "'";
                if (dt.DefaultView.Count > 0)
                {
                    string alias = Convert.ToString(dt.DefaultView[0]["fldalias"]);
                    format = Convert.ToString(dt.DefaultView[0]["fldformat"]);
                    if (alias == String.Empty || useAlias == false)
                        return fieldName;
                    else
                        return alias;

                }
                else
                    return fieldName;
            }
        }



        #endregion

        #region Properties

        [ImportMany(typeof(Parsers.IFieldParser))]
        internal List<Lazy<Parsers.IFieldParser>> Parsers { get; set; }

        /// <summary>
        /// Gets or Sets the user that will be used for user specific fields.
        /// </summary>
        public User CurrentUser
        {
            get
            {
                return _context.User;
            }
            set
            {
                _context.User = value;
                if (_context.User == null)
                    _context.User = Session.CurrentSession.CurrentUser;
            }
        }

        /// <summary>
        /// Gets or Sets the fee earner that will be used for fee earner specific fields.
        /// </summary>
        public FeeEarner CurrentFeeEarner
        {
            get
            {
                return _context.FeeEarner;
            }
            set
            {
                _context.FeeEarner = value;
                if (_context.FeeEarner == null)
                    _context.FeeEarner = Session.CurrentSession.CurrentFeeEarner;
            }
        }

        /// <summary>
        /// Gets the current contact being used to merge.
        /// </summary>
        public Contact CurrentContact
        {
            get
            {
                return _context.Contact;
            }
        }

        /// <summary>
        /// Gets the current client being used to merge.
        /// </summary>
        public Client CurrentClient
        {
            get
            {
                return _context.Client;
            }
        }

        /// <summary>
        /// Gets the current file being used to merge.
        /// </summary>
        public OMSFile CurrentFile
        {
            get
            {
                return _context.File;
            }
        }

        /// <summary>
        /// Gets the current file phase being used to merge.
        /// </summary>
        public FilePhase CurrentFilePhase
        {
            get
            {
                return _context.Phase;
            }
        }

        /// <summary>
        /// Gets the current associate being used to merge.
        /// </summary>
        public Associate CurrentAssociate
        {
            get
            {
                return _context.Associate;
            }
        }

        /// <summary>
        /// Gets the not found text.
        /// </summary>
        protected virtual string NotFoundText
        {
            get
            {
                return _context.Session.Resources.GetResource("NOTFOUND", "Not Found", "").Text;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Changes the underlying additional object to extract info from.
        /// </summary>
        /// <param name="newObject">The new object to use.</param>
        public void ChangeObject(object newObject)
        {
            _context = (ParserContext)factory.CreateContext(newObject);
        }

        /// <summary>
        /// Changes or Sets the parameters collection.
        /// </summary>
        /// <param name="parameters"></param>
        public void ChangeParameters(Common.KeyValueCollection parameters)
        {
            _params = parameters;
        }

        /// <summary>
        /// Builds a CSV merge file for backward compaibility.
        /// </summary>
        /// <param name="addtionalMergeData">Additional merge data.</param>
        /// <returns>A file info object which contains the file location.</returns>
        public System.IO.FileInfo BuildMergeFile(System.Data.DataTable addtionalMergeData)
        {
            System.IO.FileInfo file = Global.GetTempFile();
            System.IO.TextWriter txt = file.CreateText();

            System.Text.StringBuilder names = new System.Text.StringBuilder();
            System.Text.StringBuilder values = new System.Text.StringBuilder();

            if (_context.Associate != null)
            {
                BuildMergeFile(FieldPrefixAssociate, ref names, ref values, _context.Associate.OMSFile.CurrencyFormat, _context.Associate.OMSFile.DateTimeFormat);
            }

            if (_context.Phase != null)
            {
                BuildMergeFile(FieldPrefixPhase, ref names, ref values, _context.Phase.File.CurrencyFormat, _context.Phase.File.DateTimeFormat);
            }

            if (_context.File != null)
            {
                BuildMergeFile(FieldPrefixFile, ref names, ref values, _context.File.CurrencyFormat, _context.File.DateTimeFormat);
            }

            if (_context.Client != null)
            {
                BuildMergeFile(FieldPrefixClient, ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);
            }

            if (_context.Contact != null)
            {
                BuildMergeFile(FieldPrefixContact, ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);
            }

            if (_context.FeeEarner != null)
            {
                BuildMergeFile(FieldPrefixFeeEarner, ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);
            }

            if (_context.User != null)
            {
                BuildMergeFile(FieldPrefixUser, ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);
            }

            if (_context.Session != null)
            {
                BuildMergeFile(FieldPrefixRegInfo, ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);
            }

            if (_context.Document != null)
            {
                BuildMergeFile(FieldPrefixDocument, ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);
            }

            if (_context.DocumentVersion != null)
            {
                BuildMergeFile(FieldPrefixDocumentVersion, ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);
            }

            if (_context.Precedent != null)
            {
                BuildMergeFile(FieldPrefixPrecedent, ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);
            }

            BuildMergeFile(FieldPrefixSpecificData, ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);


            if (_context.Contact != null)
            {
                ExtendedDataList ext = _context.Contact.ExtendedData;
                if (ext != null)
                {
                    int ctr = 0;
                    for (ctr = 0; ctr < ext.Count; ctr++)
                    {
                        try
                        {
                            ExtendedData dat = ext[ctr];
                            BuildMergeFile(dat.GetExtendedDataTable(), FieldPrefixExtendedData + FieldPrefixContact + "." + dat.Code, ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);
                        }
                        catch { }
                    }
                }
            }

            if (_context.Client != null)
            {
                ExtendedDataList ext = _context.Client.ExtendedData;
                if (ext != null)
                {
                    int ctr = 0;
                    for (ctr = 0; ctr < ext.Count; ctr++)
                    {
                        try
                        {
                            ExtendedData dat = ext[ctr];
                            BuildMergeFile(dat.GetExtendedDataTable(), FieldPrefixExtendedData + FieldPrefixClient + "." + dat.Code, ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);
                        }
                        catch
                        { }
                    }
                }
            }

            if (_context.File != null)
            {
                ExtendedDataList ext = _context.File.ExtendedData;
                if (ext != null)
                {
                    int ctr = 0;
                    for (ctr = 0; ctr < ext.Count; ctr++)
                    {
                        try
                        {
                            ExtendedData dat = ext[ctr];
                            BuildMergeFile(dat.GetExtendedDataTable(), FieldPrefixExtendedData + FieldPrefixFile + "." + dat.Code, ref names, ref values, _context.File.CurrencyFormat, _context.File.DateTimeFormat);
                        }
                        catch { }
                    }
                }
            }

            if (_context.Associate != null)
            {
                ExtendedDataList ext = _context.Associate.ExtendedData;
                if (ext != null)
                {
                    int ctr = 0;
                    for (ctr = 0; ctr < ext.Count; ctr++)
                    {
                        try
                        {
                            ExtendedData dat = ext[ctr];
                            BuildMergeFile(dat.GetExtendedDataTable(), FieldPrefixExtendedData + FieldPrefixAssociate + "." + dat.Code, ref names, ref values, _context.Associate.OMSFile.CurrencyFormat, _context.Associate.OMSFile.DateTimeFormat);
                        }
                        catch { }
                    }
                }
            }

            if (_context.Document != null)
            {
                ExtendedDataList ext = _context.Document.ExtendedData;
                if (ext != null)
                {
                    int ctr = 0;
                    for (ctr = 0; ctr < ext.Count; ctr++)
                    {
                        try
                        {
                            ExtendedData dat = ext[ctr];
                            BuildMergeFile(dat.GetExtendedDataTable(), FieldPrefixExtendedData + FieldPrefixDocument + "." + dat.Code, ref names, ref values, _context.Document.Associate.OMSFile.CurrencyFormat, _context.Document.Associate.OMSFile.DateTimeFormat);
                        }
                        catch { }
                    }
                }
            }

            if (addtionalMergeData != null)
            {
                BuildMergeFile(addtionalMergeData, "", ref names, ref values, _context.Session.DefaultCurrencyFormat, _context.Session.DefaultDateTimeFormat);
            }

            names.Length -= 2;
            values.Length -= 2;

            txt.WriteLine(names);
            txt.WriteLine(values);

            txt.Close();

            return file;
        }

        private void BuildMergeFileField(string fldname, object val, ref System.Text.StringBuilder columnNames, ref System.Text.StringBuilder columnValues, System.Globalization.NumberFormatInfo numberFormat, System.Globalization.DateTimeFormatInfo dateFormat)
        {
            if (columnNames.ToString().IndexOf("\"" + fldname + "\"") < 0)
            {

                columnNames.Append("\"");
                columnNames.Append(fldname);
                columnNames.Append("\"");
                columnNames.Append(", ");


                string strval = "";

                if (val != null)
                    strval = Convert.ToString(GetFormattedValue(val, numberFormat, dateFormat, ""));

                if (strval == "")
                    strval = "[__]";


                columnValues.Append("\"");
                columnValues.Append(strval);
                columnValues.Append("\"");
                columnValues.Append(", ");
            }
        }

        /// <summary>
        /// Loop through a data table and adds it to the CSV file.
        /// </summary>
        private void BuildMergeFile(System.Data.DataTable tbl, string prefix, ref System.Text.StringBuilder columnNames, ref System.Text.StringBuilder columnValues, System.Globalization.NumberFormatInfo numberFormat, System.Globalization.DateTimeFormatInfo dateFormat)
        {
            foreach (System.Data.DataColumn col in tbl.Columns)
            {
                string fldname = Convert.ToString(col.ColumnName);
                object val = tbl.Rows[0][col];
                BuildMergeFileField(fldname, val, ref columnNames, ref columnValues, numberFormat, dateFormat);

                fldname = Convert.ToString(prefix + "." + col.ColumnName);
                val = tbl.Rows[0][col];
                BuildMergeFileField(fldname, val, ref columnNames, ref columnValues, numberFormat, dateFormat);
            }
        }

        /// <summary>
        /// Loops through the available fields and get there values.
        /// </summary>
        private void BuildMergeFile(string type, ref System.Text.StringBuilder columnNames, ref System.Text.StringBuilder columnValues, System.Globalization.NumberFormatInfo numberFormat, System.Globalization.DateTimeFormatInfo dateFormat)
        {
            System.Data.DataTable tbl = GetFields(type);

            foreach (System.Data.DataRow row in tbl.Rows)
            {
                string fldname = Convert.ToString(row["fldName"]);
                object val = Parse(fldname, null);
                BuildMergeFileField(fldname, val, ref columnNames, ref columnValues, numberFormat, dateFormat);
            }

            //Loop through each of the old fields of the given type and add them to the
            //mail mergefield.
            System.Data.DataView vw = new System.Data.DataView(tbl);
            vw.RowFilter = "Len(fldOld)>0";
            foreach (System.Data.DataRowView row in vw)
            {
                string fldname = Convert.ToString(row["fldOLD"]).ToUpper();
                object val = Parse(fldname, null);
                BuildMergeFileField(fldname, val, ref columnNames, ref columnValues, numberFormat, dateFormat);
            }

        }


        /// <summary>
        /// Converts the value into the correct format.
        /// </summary>
        /// <param name="val">The value to format.</param>
        /// <param name="numberFormat">The number format to use.</param>
        /// <param name="dateFormat">The date format to use.</param>
        /// <param name="alternative">Alternative formatting value.</param>
        /// <returns>The formatted string.</returns>
        public static object GetFormattedValue(object val, System.Globalization.NumberFormatInfo numberFormat, System.Globalization.DateTimeFormatInfo dateFormat, string alternative)
        {
            //UTCFIX: DM - 30/11/06 - Make sure that Local time is reurned by default.
            if (val is DateTime)
            {
                DateTime dte = (DateTime)val;

                if (dte.Kind != DateTimeKind.Unspecified)
                {
                    if (alternative.StartsWith("UTC-"))
                    {
                        alternative = alternative.Replace("UTC-", "");
                        dte = dte.ToUniversalTime();
                    } // MNW Code added 8/12/2009 - Adding ELSE as the Local Time is not being returned.
                    else
                    {
                        dte = dte.ToLocalTime();
                    }
                }

                return dte.ToString((alternative == "" ? CheckDateTimeFormating(dte) : alternative), dateFormat);

            }
            else if (val is Common.DateTimeNULL)
            {
                Common.DateTimeNULL dte = (Common.DateTimeNULL)val;

                if (dte.Kind != DateTimeKind.Unspecified)
                {
                    if (alternative.StartsWith("UTC-"))
                    {
                        alternative = alternative.Replace("UTC-", "");
                        dte = dte.ToUniversalTime();
                    } // MNW Code added 8/12/2009 - Adding ELSE as the Local Time is not being returned.
                    else
                    {
                        dte = dte.ToLocalTime();
                    }
                }

                return dte.ToString((alternative == "" ? CheckDateTimeFormating(dte) : alternative), dateFormat);
            }
            else if (val is Decimal)
            {
                return ((Decimal)val).ToString((alternative == "" ? "C" : alternative), numberFormat);
            }
            else if (val is bool)
            {
                if (Common.ConvertDef.ToBoolean(val, false))
                    return Session.CurrentSession.Resources.GetResource("YES", "Yes", "").Text;
                else
                    return Session.CurrentSession.Resources.GetResource("NO", "No", "").Text;
            }
            else
                return val;
        }


        private static string CheckDateTimeFormating(Common.DateTimeNULL val)
        {
            return val.IsNull ? string.Empty :
                val.Hour == 0 && val.Minute == 0 && val.Second == 0 ? "D" : "f";
        }


        #endregion

        #region Static Methods

        /// <summary>
        /// Creates a standard field syntax string.
        /// </summary>
        /// <param name="fieldName">The field name to use.</param>
        /// <returns>The field name in how the field parser reads them in.</returns>
        public static string CreateField(string fieldName)
        {
            return fieldName;
        }

        /// <summary>
        /// Creates a prompt field syntax string.
        /// </summary>
        /// <param name="message">The message to use / or code lookup code.</param>
        /// <param name="required">Flags whether the message is required or not.</param>
        /// <returns>The field name in how the field parser reads them in.</returns>
        public static string CreatePromptField(string message, bool required)
        {
            if (required)
                return FieldPrefixRequiredPrompt + message;
            else
                return FieldPrefixPrompt + message;
        }

        /// <summary>
        /// Creates a lookup field.
        /// </summary>
        /// <param name="fieldName">The field name in which the value will be matched with a code lookup.</param>
        /// <param name="codelookupType"></param>
        /// <returns>The field name in how the field parser reads them in.</returns>
        public static string CreateLookupField(string fieldName, string codelookupType)
        {
            return FieldPrefixMacro + FieldPrefixLookup + FieldSeperator + fieldName + FieldSeperator + codelookupType;
        }


        /// <summary>
        /// Creates an associate field.
        /// </summary>
        /// <param name="contactType">The type of contact to filter by.</param>
        /// <param name="assocType">The associate type to filter by.</param>
        /// <param name="key">The key to use to match up the associate field with another.</param>
        /// <param name="message">The prompt message to use.</param>
        /// <param name="fieldName">The field name to acquire.</param>
        /// <returns>The Field Parser's interpretation of the field.</returns>
        public static string CreateAssociateField(string contactType, string assocType, string key, string message, string fieldName)
        {
            return FieldPrefixPrompt + FieldPrefixAssociate + FieldSeperator + contactType + FieldFilterJoin + assocType + FieldSeperator + key + FieldSeperator + message + FieldSeperator + fieldName;
        }

        /// <summary>
        /// Creates an appointment field.
        /// </summary>
        /// <param name="key">The key to use to match up the appointment field with another.</param>
        /// <param name="message">The prompt message to use.</param>
        /// <param name="fieldName">The field name to acquire.</param>
        /// <returns>The Field Parser's interpretation of the field.</returns>
        public static string CreateAppointmentField(string key, string message, string fieldName)
        {
            return FieldPrefixPrompt + FieldPrefixAppointment + FieldSeperator + "" + FieldSeperator + key + FieldSeperator + message + FieldSeperator + fieldName;
        }

        /// <summary>
        /// Creates any extended list field.
        /// </summary>
        /// <param name="list">The search list type to use.</param>
        /// <param name="filter">A filter string to use.</param>
        /// <param name="key">The key to use to match up the field with another of the same list type.</param>
        /// <param name="message">The prompt message to use.</param>
        /// <param name="fieldName">The field name to acquire.</param>
        /// <returns>The Field Parser's interpretation of the field.</returns>
        public static string CreateExtendedListField(string list, string filter, string key, string message, string fieldName)
        {
            return FieldPrefixPrompt + list + FieldSeperator + filter + FieldSeperator + key + FieldSeperator + message + FieldSeperator + fieldName;
        }

        /// <summary>
        /// Gets available fields by type.
        /// </summary>
        /// <param name="fieldType">The field types to list.</param>
        /// <returns>A data table returned.</returns>
        public static System.Data.DataTable GetFields(string fieldType)
        {
            if (_fields == null)
                GetFields();
            if (fieldType.ToLower() != "all")
            {
                if (_fields.Tables.Contains(fieldType))
                    return _fields.Tables[fieldType].Copy();
                else
                    return _fields.Tables[0].Clone();
            }
            else
            {
                System.Data.DataTable tableAll = new System.Data.DataTable();

                foreach (System.Data.DataTable table in _fields.Tables)
                {
                    if (table.TableName != FieldPrefixAppointment && table.TableName != FieldPrefixPhase)
                        tableAll.Merge(table);
                }

                return tableAll.Copy();
            }
        }

        /// <summary>
        /// Gets available fields.
        /// </summary>
        /// <returns>A data set of the fields.</returns>
        static public System.Data.DataSet GetFields()
        {
            Session.CurrentSession.CheckLoggedIn();
            if (_fields == null)
            {
                System.Data.IDataParameter[] pars = new System.Data.IDataParameter[1];
                pars[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.NVarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                _fields = Session.CurrentSession.Connection.ExecuteProcedureDataSet("sprFieldTypes",
                    new string[20]{".",
									FieldPrefixMacro, 
									  FieldPrefixReflection, 
									  FieldPrefixRegInfo, 
									  FieldPrefixBranchInfo,
									  FieldPrefixUser, 
									  FieldPrefixFeeEarner, 
									  FieldPrefixContact, 
									  FieldPrefixContactCompany, 
									  FieldPrefixContactIndividual, 
									  FieldPrefixClient, 
									  FieldPrefixAdditionalClient, 
									  FieldPrefixFile, 
									  FieldPrefixAdditionalFile, 
									  FieldPrefixAssociate, 
									  FieldPrefixSpecificData, 
									  FieldPrefixExtendedData,
										FieldPrefixAppointment,
										FieldPrefixDateTime,
										FieldPrefixLookup
								  }, pars);

                if (_fields.Tables.Count > 20)
                    _fields.Tables[20].TableName = FieldPrefixPhase;

                const string fldName = "fldName";
                for (int i = 0; i < _fields.Tables.Count; i++)
                {
                    System.Data.DataTable table = _fields.Tables[i];
                    try
                    {
                        table.PrimaryKey = new DataColumn[] { table.Columns[fldName] };
                    }
                    catch (ArgumentException ex)
                    {
                        string duplicates = table.AsEnumerable().GroupBy(r => (string)r[fldName]).Where(gr => gr.Count() > 1).Select(gr => gr.Key).Aggregate((s1, s2) => string.Concat(s1, Environment.NewLine, s2));
                        string message = string.Format("{1}{0}Table = [{3}], fldType = '{2}', column = [{4}].{0}Please check and remove duplicated fields:{0}{5}",
                            Environment.NewLine, ex.Message, table.TableName,
                            table.TableName == FieldPrefixSpecificData ? "dbSpecificData" : "dbFields",
                            table.TableName == FieldPrefixSpecificData ? "spLookup" : fldName,
                            duplicates);
                        throw new ArgumentException(message, ex);
                    }
                }
            }
            return _fields;
        }

        /// <summary>
        /// Clears the field list.
        /// </summary>
        static internal void ClearFields()
        {
            if (_fields != null)
            {
                _fields.Dispose();
                _fields = null;
            }
        }


        #endregion



    }


    /// <summary>
    /// The UI prompt delegate.
    /// </summary>
    public delegate void FieldParserPromptHandler(FieldParser sender, FieldParserPromptEventArgs e);


    /// <summary>
    /// The UI prompt delegate.
    /// </summary>
    public delegate void FieldParserListPromptHandler(FieldParser sender, FieldParserListPromptEventArgs e);

    /// <summary>
    /// Extra delegate / event event arguments to be passed to the User interface.
    /// </summary>
    public class FieldParserPromptEventArgs : PromptEventArgs
    {

        private string _field = "";
        private bool _required = false;

        public FieldParserPromptEventArgs(string field, bool required, string message)
            : base(PromptType.InputBox, "", new string[0], message)
        {
            _field = field;
            _required = required;
        }

        public string FieldName
        {
            get
            {
                return _field;
            }
        }

        public bool Required
        {
            get
            {
                return _required;
            }
        }

    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string Message) : base(Message) { }
    }

    public class FieldParserListPromptEventArgs : PromptEventArgs
    {
        private string _field = "";
        private string _key = "";

        public FieldParserListPromptEventArgs(string field, string message, string type, string key, object[] filter)
            : base(PromptType.Search, type, filter, message)
        {
            _field = field;
            _key = key;
        }


        public string FieldName
        {
            get
            {
                return _field;
            }
        }

        public string Key
        {
            get
            {
                return _key;
            }
        }


    }

    /// <summary>
    /// The conditional parser evaluates whether something is true.
    /// </summary>
    public class ConditionalParser
    {
        public class Statement
        {
            #region Fields & Constants

            private const string And = "&";
            private const string Or = "|";

            private Condition First = null;
            private string BitwiseOp = "";
            private Condition Second = null;
            public readonly System.Text.RegularExpressions.Match Match;

            #endregion

            #region Constructors

            private Statement() { }

            internal Statement(System.Text.RegularExpressions.Match match, FieldParser parser)
            {
                Match = match;
                string statement = match.Groups["Conditions"].Value;
                System.Collections.ArrayList conds_temp = new System.Collections.ArrayList();

                string andsplit = " " + And + " ";
                string orsplit = " " + Or + " ";

                if (statement.IndexOf(andsplit) > -1)
                {
                    BitwiseOp = And;
                    First = new Condition(statement.Substring(0, statement.IndexOf(andsplit)), parser);
                    Second = new Condition(statement.Substring(statement.IndexOf(andsplit) + andsplit.Length), parser);
                }
                else
                {
                    if (statement.IndexOf(orsplit) > -1)
                    {
                        BitwiseOp = Or;
                        First = new Condition(statement.Substring(0, statement.IndexOf(orsplit)), parser);
                        Second = new Condition(statement.Substring(statement.IndexOf(orsplit) + orsplit.Length), parser);
                    }
                }

                if (First == null)
                    First = new Condition(statement, parser);

            }

            #endregion

            #region Methods

            public bool Validate()
            {
                if (First == null)
                    return false;

                if (Second == null)
                    return First.Validate();

                switch (BitwiseOp)
                {
                    case And:
                        return (First.Validate() && Second.Validate());
                    case Or:
                        return (First.Validate() || Second.Validate());
                    default:
                        return false;
                }
            }

            #endregion
        }

        public class Condition
        {
            #region Fields & Constants

            private const string Equ = "=";
            private const string LessThan = "<";
            private const string GreaterThan = ">";
            private const string GTET = ">=";
            private const string LTET = "<=";
            private const string NotEquals = "!=";

            private string[] Operators = new string[] { LTET, NotEquals, GTET, "<>", Equ, LessThan, GreaterThan };
            private const string FieldPrefix = "<<<";
            private const string FieldSuffix = ">>>";

            private string Operator = Equ;
            private FieldParser Parser = null;
            private string ComparisonField1 = "";
            private object ComparisonValue1 = null;
            private string ComparisonField2 = "";
            private object ComparisonValue2 = null;

            private const string NULL = "NULL";

            #endregion

            #region Constructors

            public Condition(string condition, FieldParser parser)
            {
                condition = condition.Trim(' ');
                string[] tokens = condition.Split(',');

                if (tokens.Length < 3)
                    throw new OMSException2("ERRCONDIT_3_ARG", "Condition needs at least 3 arguments.");

                Parser = parser;
                if (Parser == null) Parser = new FieldParser();

                if (tokens[0].StartsWith(FieldPrefix) && tokens[0].EndsWith(FieldSuffix))
                    ComparisonField1 = tokens[0].Replace(FieldPrefix, "").Replace(FieldSuffix, "");
                else
                {
                    object val = tokens[0];
                    if (tokens.Length > 3)
                    {
                        Type t = Session.CurrentSession.TypeManager.Load(tokens[3]);
                        if (t != null)
                        {
                            try
                            {
                                val = Convert.ChangeType(val, t);
                            }
                            catch { }
                        }
                    }
                    ComparisonValue1 = val;
                }

                if (tokens[2].StartsWith(FieldPrefix) && tokens[2].EndsWith(FieldSuffix))
                    ComparisonField2 = tokens[2].Replace(FieldPrefix, "").Replace(FieldSuffix, "");
                else
                {
                    object val = tokens[2];
                    if (tokens.Length > 3)
                    {
                        Type t = Session.CurrentSession.TypeManager.Load(tokens[3]);
                        if (t != null)
                        {
                            try
                            {
                                val = Convert.ChangeType(val, t);
                            }
                            catch { }
                        }
                    }
                    ComparisonValue2 = val;
                }

                if (Array.IndexOf(Operators, tokens[1]) >= 0)
                    Operator = tokens[1];
                else
                    Operator = Equ;

            }

            #endregion

            #region Methods

            public bool Validate()
            {
                if (ComparisonField1 != String.Empty)
                {
                    try
                    {
                        ComparisonValue1 = Parser.Parse(ComparisonField1, true);
                    }
                    catch
                    {
                        ComparisonValue1 = "";
                    }
                }
                if (ComparisonField2 != String.Empty)
                {
                    try
                    {
                        ComparisonValue2 = Parser.Parse(ComparisonField2, true);
                    }
                    catch
                    {
                        ComparisonValue2 = "";
                    }
                }

                if (ComparisonValue1 is String)
                    ComparisonValue1 = Convert.ToString(ComparisonValue1).Trim();
                if (ComparisonValue2 is String)
                    ComparisonValue2 = Convert.ToString(ComparisonValue2).Trim();

                switch (Operator)
                {
                    case Equ:
                        return ValidateEquals();
                    case NotEquals:
                        return !(ValidateEquals());
                    case "<>":
                        goto case NotEquals;
                    case GreaterThan:
                        return ValidateGreaterThan();
                    case LessThan:
                        return ValidateLessThan();
                    case GTET:
                        return (ValidateEquals() || ValidateGreaterThan());
                    case LTET:
                        return (ValidateEquals() || ValidateLessThan());
                }

                return false;
            }

            #endregion

            #region Private Methods

            private bool ValidateEquals()
            {
                if (ComparisonValue1 == null && ComparisonValue2 == null)
                    return true;
                if (ComparisonValue1 == null)
                    return false;
                else
                {
                    return ComparisonValue1.Equals(ComparisonValue2);
                }
            }

            private bool ValidateGreaterThan()
            {
                if (ValidateEquals())
                    return false;

                IComparable comp1 = ComparisonValue1 as IComparable;
                IComparable comp2 = ComparisonValue1 as IComparable;

                if (comp1 == null || comp2 == null)
                    return false;
                else
                    return (comp1.CompareTo(comp2) > 0);
            }


            private bool ValidateLessThan()
            {
                if (ValidateEquals())
                    return false;

                IComparable comp1 = ComparisonValue1 as IComparable;
                IComparable comp2 = ComparisonValue1 as IComparable;

                if (comp1 == null || comp2 == null)
                    return false;
                else
                    return (comp1.CompareTo(comp2) < 0);
            }

            #endregion
        }

        #region Constants

        private const string StatementIF = "[IF";
        public string NewLine = Environment.NewLine;

        #endregion

        #region Fields

        private FieldParser _parser = null;

        #endregion

        public ConditionalParser()
        {
        }

        public ConditionalParser(FieldParser parser)
        {
            _parser = parser;
        }

        public string ParseString(string text)
        {
            Statement[] state;
            return ParseString(text, true, out state);
        }

        public string ParseString(string text, bool apply, out Statement[] statements)
        {
            System.Collections.ArrayList conds = new System.Collections.ArrayList();
            string fieldregx = System.Text.RegularExpressions.Regex.Escape(StatementIF) + @"(?<Conditions>.*?)](?<TrueValue>.*?)" + System.Text.RegularExpressions.Regex.Escape(NewLine);
            System.Text.RegularExpressions.MatchCollection matches = System.Text.RegularExpressions.Regex.Matches(text, fieldregx, System.Text.RegularExpressions.RegexOptions.Multiline);

            System.Collections.ArrayList states = new System.Collections.ArrayList();
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                if (match.Success)
                {
                    states.Add(new Statement(match, _parser));
                }
            }

            statements = new Statement[states.Count];
            states.CopyTo(statements);

            if (apply)
            {
                foreach (Statement s in states)
                {
                    if (s.Validate())
                    {
                        string rep = s.Match.Value.Substring(0, s.Match.Groups["TrueValue"].Index - s.Match.Index);
                        text = text.Replace(rep, "");
                    }
                    else
                    {
                        text = text.Replace(s.Match.Value, "");
                    }
                }

            }
            return text;
        }
    }

}
