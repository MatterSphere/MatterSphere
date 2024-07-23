using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using FWBS.OMS.OMSEXPORT.ResetFlagCommands;

namespace FWBS.OMS.OMSEXPORT
{
    public enum ProcessExecutionResult { Success, Interface, Failure}

    public class OMSExportE3E : OMSExportBase, IDisposable
    {
        #region Constants

        const string GetMattDateOQL = @"<QUERY xmlns=""http://elite.com/schemas/query"">
 <SELECT ID=""SelectStatement"" Class=""NextGen.Framework.OQL.Symbols.SelectStatement"">
  <OQL_CONTEXT Class=""NextGen.Framework.Managers.ObjectMgr.ExContextProvider"">
    <NODEMAP ID=""Node#1"" QueryID=""MattDate"" Class=""NextGen.Application.Query.MattDate"" Assembly=""NextGen.Archetype.Matter"" />
  </OQL_CONTEXT>
  <SELECT_LIST>
    <SINGLE_SELECT Union=""Distinct"">
      <NODE NodeID=""Node#1"" />
      <VALUES>
        <VALUE>
          <LEAF QueryID=""MattDateID"">
            <NODE NodeID=""Node#1"" />
          </LEAF>
        </VALUE>
        <VALUE>
          <LEAF QueryID=""EffStart"">
            <NODE NodeID=""Node#1"" />
          </LEAF>
        </VALUE>
      </VALUES>
      <WHERE>
        <X_AND_Y>
          <X>
            <X_IS_EQUAL_TO_Y>
              <X>
                <LEAF QueryID=""MatterLkUp"">
                  <NODE NodeID=""Node#1"" />
                </LEAF>
              </X>
              <Y>
                <INT_NUM Value=""{0}"" />
              </Y>
            </X_IS_EQUAL_TO_Y>
          </X>
          <Y>
            <X_IS_EQUAL_TO_Y>
              <X>
                <LEAF QueryID=""EffStart"">
                  <NODE NodeID=""Node#1"" />
                </LEAF>
              </X>
              <Y>
                <UNICODE_STRING Value=""{1}"" />
              </Y>
            </X_IS_EQUAL_TO_Y>
          </Y>
        </X_AND_Y>
      </WHERE>
    </SINGLE_SELECT>
  </SELECT_LIST>
</SELECT>
</QUERY>";
        const string GetCliDateOQL = @"<QUERY xmlns=""http://elite.com/schemas/query"">
<SELECT ID=""SelectStatement"" Class=""NextGen.Framework.OQL.Symbols.SelectStatement"" xmlns=""http://elite.com/schemas/query"">
  <OQL_CONTEXT Class=""NextGen.Framework.Managers.ObjectMgr.ExContextProvider"">
    <NODEMAP ID=""Node#1"" QueryID=""CliDate"" Class=""NextGen.Application.Query.CliDate"" Assembly=""NextGen.Archetype.Client"" />
  </OQL_CONTEXT>
  <SELECT_LIST>
    <SINGLE_SELECT Union=""Distinct"">
      <NODE NodeID=""Node#1"" />
      <VALUES>
        <VALUE>
          <LEAF QueryID=""ClientLkUp"">
            <NODE NodeID=""Node#1"" />
          </LEAF>
        </VALUE>
      </VALUES>
      <WHERE>
        <X_AND_Y>
          <X>
            <X_IS_EQUAL_TO_Y>
              <X>
                <LEAF QueryID=""ClientLkUp"">
                  <NODE NodeID=""Node#1"" />
                </LEAF>
              </X>
              <Y>
                <INT_NUM Value=""{0}"" />
              </Y>
            </X_IS_EQUAL_TO_Y>
          </X>
          <Y>
            <X_IS_EQUAL_TO_Y>
              <X>
                <LEAF QueryID=""EffStart"">
                  <NODE NodeID=""Node#1"" />
                </LEAF>
              </X>
              <Y>
                <UNICODE_STRING Value=""{1}"" />
              </Y>
            </X_IS_EQUAL_TO_Y>
          </Y>
        </X_AND_Y>
      </WHERE>
    </SINGLE_SELECT>
  </SELECT_LIST>
</SELECT>
</QUERY>";
        const string GetSiteIndexOQL = @"<QUERY xmlns=""http://elite.com/schemas/query"">
<SELECT ID=""SelectStatement"" Class=""NextGen.Framework.OQL.Symbols.SelectStatement"">
  <OQL_CONTEXT Class=""NextGen.Framework.Managers.ObjectMgr.ExContextProvider"">
    <NODEMAP ID=""Node#1"" QueryID=""Address"" Class=""NextGen.Application.Query.Address"" Assembly=""NextGen.Archetype.Address"" />
    <NODEMAP ID=""Node#2"" QueryID=""Site"" Class=""NextGen.Application.Query.Site"" Assembly=""NextGen.Archetype.Site"" />
    <NODEMAP ID=""Node#3"" QueryID=""Relate1"">
      <NODE NodeID=""Node#2"" />
    </NODEMAP>
  </OQL_CONTEXT>
  <SELECT_LIST>
    <SINGLE_SELECT Union=""Distinct"">
      <NODE NodeID=""Node#1"" />
      <VALUES>
        <VALUE>
          <LEAF QueryID=""SiteIndex"">
            <NODE NodeID=""Node#2"" />
          </LEAF>
        </VALUE>
        <VALUE>
          <LEAF QueryID=""AddrIndex"">
            <NODE NodeID=""Node#1"" />
          </LEAF>
        </VALUE>
        <VALUE>
          <LEAF QueryID=""Street"">
            <NODE NodeID=""Node#1"" />
          </LEAF>
        </VALUE>
        <VALUE>
          <LEAF QueryID=""Description"">
            <NODE NodeID=""Node#2"" />
          </LEAF>
        </VALUE>
      </VALUES>
      <JOINS>
        <INNER_JOIN>
          <ON_CONDITION>
            <X_IS_EQUAL_TO_Y>
              <X>
                <LEAF QueryID=""AddrIndex"">
                  <NODE NodeID=""Node#1"" />
                </LEAF>
              </X>
              <Y>
                <LEAF QueryID=""Address"">
                  <NODE NodeID=""Node#2"" />
                </LEAF>
              </Y>
            </X_IS_EQUAL_TO_Y>
          </ON_CONDITION>
        </INNER_JOIN>
      </JOINS>
      <WHERE>
        <X_AND_Y>
          <X>
            <X_IS_EQUAL_TO_Y>
              <X>
                <LEAF QueryID=""SbjEntity"">
                  <NODE NodeID=""Node#3"" />
                </LEAF>
              </X>
              <Y>
                <INT_NUM Value=""{0}"" />
              </Y>
            </X_IS_EQUAL_TO_Y>
          </X>
          <Y>
            <X_IS_EQUAL_TO_Y>
              <X>
                <LEAF QueryID=""IsDefault"">
                  <NODE NodeID=""Node#2"" />
                </LEAF>
              </X>
              <Y>
                <BOOLEAN_VALUE Value=""1"" />
              </Y>
            </X_IS_EQUAL_TO_Y>
          </Y>
        </X_AND_Y>
      </WHERE>
    </SINGLE_SELECT>
  </SELECT_LIST>
</SELECT>
</QUERY>
";

        #endregion

        private string _lastCancellationSent = "";
        private string _lastCancellationResponse = "";

        #region Fields

        private TransactionService.TransactionService _transSvs;
        /// <summary>
        /// Used when calling registry read function 
        /// </summary>
        private const string APPNAME = "E3E";
        /// <summary>
        /// Monitor if logged in to E3E
        /// </summary>
        private bool _loggedIn = false;

        /// <summary>
        /// Custom property for Maurice Blackburn to cancel process using their CancelProcess Process
        /// </summary>
        private bool _CancelProcess = false;

        /// <summary>
        /// Custom property for running matter export conflict check CftWFNewBizIntake
        /// </summary>
        private bool _matterExportPostProcess = false;

        /// <summary>
        /// Custom property to reset need export flag in database in case of failure.
        /// </summary>
        private bool _ResetFlagInCaseOfFailure = true;

        /// <summary>
        /// Convert to Local Time flag
        /// </summary>
        private bool _convertToLocalTime = false;

        /// <summary>
        /// Debug Success Location
        /// </summary>
        private string _debugSuccessLocation = "";

        /// <summary>
        /// Debug Failed Location
        /// </summary>
        private string _debugFailedLocation = "";

        #endregion

        protected override string dataFormat => "xml";

        #region E3E Specific Functions

        /// <summary>
        /// Logs into E3E and sets cookie containers on all E3E objects
        /// </summary>
        private void LoginToE3E()
        {
            RaiseStatusEvent("Logging into E3E system.");

            try
            {
                bool isNewEndpoint = Convert.ToBoolean(StaticLibrary.GetSetting("NewEndpoint", APPNAME, "False"));
                string relativeUrl = isNewEndpoint ?
                       StaticLibrary.GetSetting(ApiRelativeUrlTransService3xKey, "", ApiRelativeUrlTransService3xDefault) :
                       StaticLibrary.GetSetting(ApiRelativeUrlTransService2xKey, "", ApiRelativeUrlTransService2xDefault);

                string baseURL = StaticLibrary.GetSetting("BaseURL", APPNAME, "");
                _CancelProcess = Convert.ToBoolean(StaticLibrary.GetSetting("CancelProcess", APPNAME, "False"));
                _matterExportPostProcess = Convert.ToBoolean(StaticLibrary.GetSetting("MatterConflictCheck", APPNAME, "True"));
                _ResetFlagInCaseOfFailure = Convert.ToBoolean(StaticLibrary.GetSetting("ResetFlagInCaseOfFailure", APPNAME, "False"));
                _debugMode = Convert.ToBoolean(StaticLibrary.GetSetting("Debug", APPNAME, "False"));
                _debugSuccessLocation = StaticLibrary.GetSetting("SuccessLocation", APPNAME, "");
                _debugFailedLocation = StaticLibrary.GetSetting("FailedLocation", APPNAME, "");


                //if any values are missing throw an exception
                if (baseURL == "")
                    throw new ApplicationException("E3E settings not configured.  Please check your settings.");

                if (!baseURL.EndsWith(@"/"))
                    baseURL += @"/";

                if (_transSvs == null)
                {
                    _transSvs = new TransactionService.TransactionService();
                    _transSvs.Credentials = DefaultCredentials;
                    _transSvs.Timeout = System.Threading.Timeout.Infinite;
                }

                //set the urls's of all the web services
                _transSvs.Url = baseURL + relativeUrl;
                
                //Test to see if we are logged in properly
                //TODO - is there a good test for this?
                string test = _transSvs.SoapVersion.ToString();
                

                RaiseStatusEvent("Logged into E3E system.");
                _loggedIn = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Logging in to E3E: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Logs out of the CMS system, use at the end of a run
        /// </summary>
        private void LogoutOfE3E()
        {
            try
            {

                if (_loggedIn)
                {
                    //null all web service variables
                    _transSvs = null;
                }

                //finally set flag
                _loggedIn = false;
            }
            catch (Exception ex)
            {
                base.LogEntry("LogoutOfE3E: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        #endregion


        /// <summary>
        /// Nothing to implement here for this onject
        /// Most is the first time any work needs doing
        /// </summary>
        protected override void InitialiseProcess()
        {
            //set the convert to local time flag
            _convertToLocalTime = StaticLibrary.GetBoolSetting("ConvertToLocalTime", "E3E", false);
        }

        /// <summary>
        /// performa any finishing off
        /// </summary>
        protected override void FinaliseProcess()
        {
            try
            {
                //Log out of CMS
                RaiseStatusEvent("Logging out of E3E");
                LogoutOfE3E();
            }
            catch { }
        }

        protected override string ExportLookup(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override int ExportUser(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override int ExportFeeEarner(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Exports an individual client record
        /// </summary>
        /// <param name="row">Data row from client query</param>
        /// <returns>the UNO of the inserted client record</returns>
        protected override object ExportClient(System.Data.DataRow row)
        {
            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToE3E();

            try
            {
                //Collect data from columns
                long contID = Convert.ToInt64(row["DEFAULTCONTACTID"]);
                long clID = Convert.ToInt64(row["CLID"]);
                string entityID = Convert.ToString(row["ENTITYINDEX"]);
                string xmlToSend = Convert.ToString(row["Client"]);
                
                if (xmlToSend == "")
                    throw new Exception("XML not supplied");
                
                //Replace %INVOICESITE% 
                if (xmlToSend.IndexOf("%INVOICESITE%") > 0)
                {
                    int siteIndex = GetEntityDefaultSiteIndex(Convert.ToInt32(entityID));

                    if (siteIndex > 0)
                    {
                        xmlToSend = xmlToSend.Replace("%INVOICESITE%", siteIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));

                        string responseClient = _transSvs.ExecuteProcess(xmlToSend, 1);

                        string ArcheType = "Client";
                        string Action = "Export" + ArcheType;
                        //Now build XML document
                        System.Xml.XmlDocument xdClient = new System.Xml.XmlDocument();
                        xdClient.LoadXml(responseClient);

                        //Check Result
                        switch (CheckResult(xdClient))
                        {
                            case ProcessExecutionResult.Success:
                                //If OK, then we just need to get the ID back and store it
                                //Write to disk
                                WriteFile(Action, clID, xmlToSend, responseClient, _debugSuccessLocation);
                                return GetKeyValue(xdClient, ArcheType);
                            case ProcessExecutionResult.Interface:
                                //Write to disk
                                WriteFile(Action, clID, xmlToSend, responseClient, _debugFailedLocation);

                                List<SqlParameter> parList = new List<SqlParameter>();
                                SqlParameter parCLID = new SqlParameter("CLID", clID);
                                parList.Add(parCLID);
                                ExecuteSQL("UPDATE DBCLIENT SET CLEXTID = -1 , CLNEEDEXPORT = 0 WHERE CLID = @CLID", parList);
                                //Should process be cancelled?
                                if (_CancelProcess)
                                    CancelProcess(GetProcess(xdClient), GetProcessItemId(xdClient),clID,Action);
                                throw new Exception(GetErrors(xmlToSend, responseClient, Action,clID));
                            case ProcessExecutionResult.Failure:
                                //Complete failure so try again next time or NOT?!
                                if (_ResetFlagInCaseOfFailure)
                                {
                                    new ResetNeedExportFlagForClientCommand(clID, this).Execute();
                                }
                                //Write to disk
                                WriteFile(Action, clID, xmlToSend, responseClient, _debugFailedLocation);
                                throw new Exception(xdClient.InnerXml);
                            default:
                                throw new Exception("Unknown ProcessExecutionResult");
                        }
                    }
                    else
                    {
                        List<SqlParameter> parList = new List<SqlParameter>();
                        SqlParameter parCLID = new SqlParameter("CLID", clID);
                        parList.Add(parCLID);
                        ExecuteSQL("UPDATE DBCLIENT SET CLEXTID = -1 , CLNEEDEXPORT = CLNEEDEXPORT WHERE CLID = @CLID",parList);
                        throw new Exception("Could not get default site index for entity");
                    }
                }
                else
                {
                    List<SqlParameter> parList = new List<SqlParameter>();
                    SqlParameter parCLID = new SqlParameter("CLID", clID);
                    parList.Add(parCLID);
                    ExecuteSQL("UPDATE DBCLIENT SET CLEXTID = -1 , CLNEEDEXPORT = CLNEEDEXPORT WHERE CLID = @CLID" , parList);
                    throw new Exception("Expected placeholder missing in XML (%INVOICESITE%)");
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        protected override bool UpdateClient(System.Data.DataRow row)
        {
            string Action = "UpdateClient";

            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToE3E();

            try
            {
                //Only updates a client - does not update entity information
                string xmlToSend = Convert.ToString(row["Client"]);
                long clid = Convert.ToInt64(row["CLID"]);
                int entityID = Convert.ToInt32(row["ENTITYINDEX"]);
                long ClientIndex = Convert.ToInt32(row["CLIENTINDEX"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                //Replace %INVOICESITE% 
                if (xmlToSend.IndexOf("%INVOICESITE%") > 0)
                {
                    int siteIndex = GetEntityDefaultSiteIndex(Convert.ToInt32(entityID));

                    if (siteIndex > 0)
                    {
                        xmlToSend = xmlToSend.Replace("%INVOICESITE%", siteIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    }
                    else
                        throw new Exception("Could not get default site index for entity");
                }

                //If XML contains {0} we know the EffectiveDated flag has been set
                if (xmlToSend.IndexOf("{0}") > 0)
                {
                    //check if there is an effective dated row for the
                    string effectiveDate = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy"); //Use this date for now?  we dont know what time zone the change was made in - do we need to worry about this?
                    string thisCliDateXOQL = string.Format(GetCliDateOQL, ClientIndex, effectiveDate);
                    string returnString = _transSvs.GetArchetypeData(thisCliDateXOQL);
                    if (!string.IsNullOrEmpty(returnString))
                    {
                        //A row was returned, so must be edit mode
                        xmlToSend = string.Format(xmlToSend, "Edit", @"KeyField=""EffStart"" KeyValue=""" + effectiveDate + @"""", effectiveDate);
                    }
                    else
                    {
                        //Adding a new effective date for the client
                        xmlToSend = string.Format(xmlToSend, "Add", "", effectiveDate);
                    }
                }



                string response = _transSvs.ExecuteProcess(xmlToSend, 1);

                //TODO - how best to handle this?  Class around object - different to other objects?
                System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
                xd.LoadXml(response);

                switch (CheckResult(xd))
                {
                    case ProcessExecutionResult.Success:
                        //If OK, then we just need to get the ID back and store it
                        //Write to disk
                        WriteFile(Action, clid, xmlToSend, response, _debugSuccessLocation);
                        return true;
                    case ProcessExecutionResult.Interface:
                        //Has been sent, but is in draft, store as -1 as we have lost control now
                        //Write to disk
                        WriteFile(Action, clid, xmlToSend, response, _debugFailedLocation);
                        if (_CancelProcess)
                            CancelProcess(GetProcess(xd), GetProcessItemId(xd),clid,Action);
                        throw new Exception(GetErrors(xmlToSend, response, Action, clid));
                    case ProcessExecutionResult.Failure:
                        //Complete failure so try again next time or NOT;
                        if (_ResetFlagInCaseOfFailure)
                        {
                            new ResetNeedExportFlagForClientCommand(clid, this).Execute();
                        }
                        WriteFile(Action, clid, xmlToSend, response, _debugFailedLocation);
                        throw new Exception(xd.InnerXml);
                    default:
                        throw new Exception("Unknown ProcessExecutionResult");
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        protected override object ExportMatter(System.Data.DataRow row)
        {
            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToE3E();

            try
            {
                string ArcheType = "Matter";
                string Action = "Export" + ArcheType;
                long fileID = Convert.ToInt64(row["FILEID"]);
                string xmlToSend = Convert.ToString(row[ArcheType]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                string response = _transSvs.ExecuteProcess(xmlToSend, 1);

                System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
                xd.LoadXml(response);

               

                //Check Result
                switch (CheckResult(xd))
                {
                    case ProcessExecutionResult.Success:
                        //Write to disk
                        WriteFile(Action, fileID, xmlToSend, response, _debugSuccessLocation);
                        //Perform post process - but the MattIndex hasnt been populated to the database yet, so have to get from the result
                        int mattIndex = GetKeyValue(xd, ArcheType);
                        if (!_matterExportPostProcess)
                            return mattIndex;

                        try
                        {
                            /*  Wrap in a try catch, as if the matter succeeds, then we still want to report a success, but we do need to log a post process failure.  
                             *
                             *  Event log is place to log for now
                             * 
                             *  TODO: where else to log more effectively
                            */

                            //Create Parameters
                            SqlParameter parFileID = new SqlParameter("FILEID", fileID);
                            SqlParameter parMattIndex = new SqlParameter("MATTINDEX", mattIndex);
                            SqlParameter parSpecial = new SqlParameter("SPECIAL", "0");
                            SqlParameter parProcessType = new SqlParameter("PROCESS_TYPE", APPNAME);

                            List<SqlParameter> parList = new List<SqlParameter>();
                            parList.Add(parFileID);
                            parList.Add(parMattIndex);
                            parList.Add(parSpecial);
                            parList.Add(parProcessType);
                            DataTable postProcess = GetDataTable("SELECT DBO.GETE3EMATTEREXPORTPOSTPROCESSESXML ( @FILEID , @MATTINDEX , @SPECIAL , @PROCESS_TYPE )", parList);
                            PostProcessInstructions(Action, fileID, Convert.ToString(postProcess.Rows[0][0]));
                        }
                        catch (Exception ex)
                        {
                            LogEntry(string.Format("ExportMatterPostProcessInstructions : Error sending PostProcessesInstructions : {0}", ex.Message), System.Diagnostics.EventLogEntryType.Error);
                        } 
                        //If OK, then we just need to get the ID back and store it
                        return mattIndex;
                    case ProcessExecutionResult.Interface:
                        //Write to disk
                        WriteFile(Action, fileID, xmlToSend, response, _debugFailedLocation);

                        List<SqlParameter> parList2 = new List<SqlParameter>();
                        SqlParameter parFILEID = new SqlParameter("FILEID", fileID);
                        parList2.Add(parFILEID);
                        ExecuteSQL("UPDATE DBFILE SET FILEEXTLINKID = -1 , FILENEEDEXPORT = 0 WHERE FILEID = @FILEID", parList2);
                        //Should process be cancelled?
                        if (_CancelProcess)
                            CancelProcess(GetProcess(xd), GetProcessItemId(xd),fileID,Action);
                        throw new Exception(GetErrors(xmlToSend, response, Action, fileID));
                    case ProcessExecutionResult.Failure:
                        //Write to disk
                        WriteFile(Action, fileID, xmlToSend, response, _debugFailedLocation);
                        //Complete failure so try again next time or NOT;
                        if (_ResetFlagInCaseOfFailure)
                        {
                            new ResetNeedExportFlagForMatterCommand(fileID, this).Execute();
                        }
                        throw new Exception(xd.InnerXml);
                    default:
                        throw new Exception("Unknown ProcessExecutionResult");
                }

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        protected override bool UpdateMatter(System.Data.DataRow row)
        {
            string Action = "UpdateMatter";

            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToE3E();

            try
            {
                string xmlToSend = Convert.ToString(row["Matter"]);
                long fileID = Convert.ToInt64(row["FILEID"]);
                long MattIndex = Convert.ToInt64(row["MATTINDEX"]);

                

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                //If XML contains {0} we know the EffectiveDated flag has been set
                if (xmlToSend.IndexOf("{0}") > 0)
                {
                    //check if there is an effective dated row for the
                    string effectiveDate = DateTime.UtcNow.Date.ToString("dd-MMM-yyyy"); //Use this date for now?  we dont know what time zone the change was made in - do we need to worry about this?
                    string thisMattDateXOQL = string.Format(GetMattDateOQL, MattIndex, effectiveDate);
			        string returnString = _transSvs.GetArchetypeData(thisMattDateXOQL);
                    if (!string.IsNullOrEmpty(returnString))
                    {
                        //A row was returned, so must be edit mode
                        xmlToSend = string.Format(xmlToSend, "Edit", @"KeyField=""EffStart"" KeyValue=""" + effectiveDate + @"""", effectiveDate);
                    }
                    else
                    {
                        //Adding a new effective date for the matter
                        xmlToSend = string.Format(xmlToSend, "Add", "", effectiveDate);
                    }
                }

                string response = _transSvs.ExecuteProcess(xmlToSend, 1);
                
                //TODO - how best to handle this?  Class around object - different to other objects?
                System.Xml.XmlDocument xd = new System.Xml.XmlDocument();
                xd.LoadXml(response);

                //Check Result
                switch (CheckResult(xd))
                {
                    case ProcessExecutionResult.Success:
                        //Write to disk
                        WriteFile(Action, fileID, xmlToSend, response, _debugSuccessLocation);//If OK, then we just need to get the ID back and store it
                        return true;
                    case ProcessExecutionResult.Interface:
                        //Write to disk
                        WriteFile(Action, fileID, xmlToSend, response, _debugFailedLocation);
                        //Has been sent, but is in draft, store as -1 as we have lost control now
                        if(_CancelProcess)
                            CancelProcess(GetProcess(xd),GetProcessItemId(xd),fileID,Action);
                        throw new Exception(GetErrors(xmlToSend, response, Action, fileID));
                    case ProcessExecutionResult.Failure:
                        //Write to disk
                        WriteFile("UpdateMatter", fileID, xmlToSend, response, _debugFailedLocation);
                        //Complete failure so try again next time;
                        throw new Exception(xd.InnerXml);
                    default:
                        throw new Exception("Unknown ProcessExecutionResult");
                }

    
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        protected override object ExportTimeRecord(System.Data.DataRow row)
        {
            System.Threading.Thread.Sleep(10);

            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToE3E();

            try
            {
                //What if a different archetype needs to be used?
                string ArcheType = "TimeCardPending";
                string Action = "Export" + ArcheType;

                string xmlToSend = Convert.ToString(row[ArcheType]);
                long timeID = Convert.ToInt64(row["ID"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                string response = _transSvs.ExecuteProcess(xmlToSend, 1);
                
                //TODO - how best to handle this?  Class around object - different to other objects?
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(response);

                //Check Result
                switch (CheckResult(xd))
                {
                    case ProcessExecutionResult.Success:
                        //If OK, then we just need to get the ID back and store it
                        //Write to disk
                        WriteFile(Action, timeID, xmlToSend, response, _debugSuccessLocation);
                        return GetKeyValue(xd, ArcheType);
                    case ProcessExecutionResult.Interface:
                        //Write to disk
                        WriteFile(Action, timeID, xmlToSend, response, _debugFailedLocation);

                        List<SqlParameter> parList = new List<SqlParameter>();
                        SqlParameter parTIMEID = new SqlParameter("TIMEID", timeID);
                        parList.Add(parTIMEID);
                        ExecuteSQL("UPDATE DBTIMELEDGER SET TIMETRANSFERREDID = -1 WHERE ID = @TIMEID", parList);
                        //Should we cancel?
                        if(_CancelProcess)
                            CancelProcess(GetProcess(xd), GetProcessItemId(xd),timeID,Action);
                        throw new Exception(GetErrors(xmlToSend, response, Action, timeID));
                    case ProcessExecutionResult.Failure:
                        //Write to disk
                        WriteFile(Action + ArcheType, timeID, xmlToSend, response, _debugFailedLocation);
                        //Complete failure so try again next time;
                        throw new Exception(xd.InnerXml);
                    default:
                        throw new Exception ("Unknown ProcessExecutionResult");
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        protected override object ExportFinancialRecord(System.Data.DataRow row)
        {
            System.Threading.Thread.Sleep(10);

            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToE3E();
              
            try
            {
                //What if a different archetype needs to be used?
                string ArcheType = "CostCardPending";
                string Action = "Export" + ArcheType;

                string xmlToSend = Convert.ToString(row[ArcheType]);
                long finLogID = Convert.ToInt64(row["ID"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                string response = _transSvs.ExecuteProcess(xmlToSend, 1);

                //TODO - how best to handle this?  Class around object - different to other objects?
                XmlDocument xd = new XmlDocument();
                xd.LoadXml(response);

                //Check Result
                switch (CheckResult(xd))
                {
                    case ProcessExecutionResult.Success:
                        //If OK, then we just need to get the ID back and store it
                        //Write to disk
                        WriteFile(Action, finLogID, xmlToSend, response, _debugSuccessLocation);
                        return GetKeyValue(xd, ArcheType);
                    case ProcessExecutionResult.Interface:
                        //Write to disk
                        WriteFile(Action, finLogID, xmlToSend, response, _debugFailedLocation);
                        //*** Do finance entries go into a draft table or not?
                        List<SqlParameter> parList = new List<SqlParameter>();
                        SqlParameter parFINLOGID = new SqlParameter("FINLOGID", finLogID);
                        parList.Add(parFINLOGID);
                        ExecuteSQL("UPDATE DBFINANCIALLEDGER SET FINEXTID = -1 WHERE FINLOGID = @FINLOGID", parList); 
                        //Should we cancel?
                        if (_CancelProcess)
                            CancelProcess(GetProcess(xd), GetProcessItemId(xd), finLogID, Action);
                        throw new Exception(GetErrors(xmlToSend, response, Action, finLogID));
                    case ProcessExecutionResult.Failure:
                        //Write to disk
                        WriteFile(Action + ArcheType, finLogID, xmlToSend, response, _debugFailedLocation);
                        //Complete failure so try again next time;
                        throw new Exception(xd.InnerXml);
                    default:
                        throw new Exception("Unknown ProcessExecutionResult");
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        protected override string ExportObject
        {
            get
            {
                return APPNAME;
            }
        }

        private Int32 GetEntityDefaultSiteIndex(Int32 EntityIndex)
        {
            Int32 siteIndex = 0;
            string sfeOQL = GetSiteIndexOQL;
            //put the entered value in the XOQL
            sfeOQL = string.Format(sfeOQL, EntityIndex);
            try
            {
                string pgagiVals = _transSvs.GetArchetypeData(sfeOQL);
                if (!string.IsNullOrEmpty(pgagiVals))
                {
                    Dictionary<string, string> values = ReadSimpleXMLData(pgagiVals, "Address");
                    //Wrap in try catches as they dont always return data
                    if (values.ContainsKey("SiteIndex"))
                        siteIndex = Convert.ToInt32(values["SiteIndex"]);
                }
            }
            catch (Exception ex)
            {
                string a = ex.Message;
            }

            return siteIndex;
        }

        private static Dictionary<string, string> ReadSimpleXMLData(string xmlData, string rowNodeName)
        {
            System.IO.StringReader s = new System.IO.StringReader(xmlData);
            System.Xml.XmlReader xr = System.Xml.XmlReader.Create(s);
            return ReadSimpleXMLData(xr, rowNodeName);
        }
        private static Dictionary<string, string> ReadSimpleXMLData(System.Xml.XmlReader xr, string rowNodeName)
        {
            //<Data>
            //  <Timekeeper>
            //    <TkprIndex>416</TkprIndex>
            //    <DisplayName>Aaron Riley</DisplayName>
            //  </Timekeeper>
            //</Data>
            Dictionary<string, string> values = new Dictionary<string, string>();
            bool isInRow = false;
            string attName = null;
            while (xr.Read())
            {
                switch (xr.NodeType)
                {
                    case System.Xml.XmlNodeType.Element:
                        if (xr.Name == rowNodeName)
                        {
                            isInRow = true;
                        }
                        else
                        {
                            attName = xr.Name;
                        }
                        break;
                    case System.Xml.XmlNodeType.Text:
                        if (isInRow) values.Add(attName, xr.Value);
                        break;
                    case System.Xml.XmlNodeType.EndElement:
                        if (xr.Name == rowNodeName)
                        {
                            isInRow = false;
                            return values;
                        }
                        break;
                }
            }
            return values;
        }

        ProcessExecutionResult CheckResult(XmlDocument ProcessExecutionResultsXml)
        {
            //Cater for difference in NxBizTalkEntityOrgLoad
            string checkAttribute = "Result";
            try
            {

                switch (Convert.ToString(ProcessExecutionResultsXml.SelectSingleNode("ProcessExecutionResults").Attributes[checkAttribute].Value).ToUpper())
                {
                    case "SUCCESS":
                        if (Convert.ToString(ProcessExecutionResultsXml.SelectSingleNode("ProcessExecutionResults").Attributes["Process"].Value) == "NxBizTalkEntityOrgLoad")
                        {
                            checkAttribute = "OutputId";
                            switch (Convert.ToString(ProcessExecutionResultsXml.SelectSingleNode("ProcessExecutionResults").Attributes[checkAttribute].Value).ToUpper())
                            {
                                case "SUCCESS":
                                    return ProcessExecutionResult.Success;
                                case "INTERFACE":
                                    return ProcessExecutionResult.Interface;
                                case "FAILURE": //Treat the same as an Interface for EntityOrg - needs to be validated!
                                    return ProcessExecutionResult.Interface;
                                default:
                                    throw new Exception("Unknown Result Code from ProcessExecutionResultsXml");
                            }
                        }
                        else if (ProcessExecutionResultsXml.SelectSingleNode("ProcessExecutionResults/DATA_ERRORS") != null)
                            return ProcessExecutionResult.Failure;
                        else
                            return ProcessExecutionResult.Success;
                    case "INTERFACE":
                        return ProcessExecutionResult.Interface;
                    case "FAILURE":
                        return ProcessExecutionResult.Failure;
                    default:
                        throw new Exception("Unknown Result Code from ProcessExecutionResultsXml");
                }
            }
            catch
            {
                //Might need to do a bit more work around here.
                return ProcessExecutionResult.Failure;
            }
        }

        //Build up a new string for errors compiled of as much informaton in message as possible
        string GetErrors(string xmlSent, string xmlResponse, string actionDescription, long internalID)
        {
            try
            {
                System.IO.StringWriter s = new System.IO.StringWriter();
                System.Xml.XmlWriterSettings settings = new System.Xml.XmlWriterSettings();
                settings.Indent = true;
                System.Xml.XmlWriter errorPacket = System.Xml.XmlWriter.Create(s, settings);

                errorPacket.WriteStartElement("ErrorMessageInfo");

                errorPacket.WriteStartElement("Information");
                errorPacket.WriteAttributeString("ActionDescription",actionDescription);
                errorPacket.WriteAttributeString("InternalID",internalID.ToString(System.Globalization.CultureInfo.InvariantCulture));
                errorPacket.WriteEndElement();  //Information
                

                //Sent
                errorPacket.WriteStartElement("Sent");
                errorPacket.WriteRaw(xmlSent);

                errorPacket.WriteEndElement();  //Sent

                //Response
                errorPacket.WriteStartElement("Response");
                errorPacket.WriteRaw(xmlResponse);

                errorPacket.WriteEndElement();  //Response

                if (_CancelProcess)
                {
                    //Sent
                    errorPacket.WriteStartElement("Cancellation_Sent");
                    errorPacket.WriteRaw(_lastCancellationSent);

                    errorPacket.WriteEndElement();  //Cancellation_Sent

                    //Response
                    errorPacket.WriteStartElement("Cancellation_Response");
                    errorPacket.WriteRaw(_lastCancellationResponse);

                    errorPacket.WriteEndElement();  //Cancellation_Response

                }
                errorPacket.WriteEndElement();  //ErrorMessageInfo

                errorPacket.Flush();

                string retvalue = s.ToString();
                s.Close();

                return retvalue;
            }
            catch (Exception ex)
            {
                return "Unable to get error - review Debug file if available [" + ex.Message + "]";
            }
        }

        string GetProcessItemId(XmlDocument ProcessExecutionResultsXml)
        {
            try
            {
                return Convert.ToString(ProcessExecutionResultsXml.SelectSingleNode("ProcessExecutionResults").Attributes["ProcessItemId"].Value).ToUpper();
            }
            catch
            {
                throw new Exception("Unable to get ProcessItemId from result");
            }
        }
        string GetProcess(XmlDocument ProcessExecutionResultsXml)
        {
            try
            {
                return Convert.ToString(ProcessExecutionResultsXml.SelectSingleNode("ProcessExecutionResults").Attributes["Process"].Value).ToUpper();
            }
            catch
            {
                throw new Exception("Unable to get Process from result");
            }
        }

        void CancelProcess(string procCode, string procItemId, long ID, string type)
        {
            //Reset first incase stored anywhere
            _lastCancellationSent = "";
            _lastCancellationResponse = "";

            string xml = @"<MS_CancelProcess_Srv xmlns=""http://elite.com/schemas/transaction/process/write/MS_CancelProcess_Srv"">
 <Initialize xmlns=""http://elite.com/schemas/transaction/object/write/MS_CancelProcess"">
  <Add>
   <MS_CancelProcess>
	<Attributes>
	 <MS_procCode>{0}</MS_procCode>
	 <MS_procItemId>{1}</MS_procItemId>
	</Attributes>
   </MS_CancelProcess>
  </Add>
 </Initialize>
</MS_CancelProcess_Srv>";
            xml = string.Format(xml, procCode, procItemId);
            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToE3E(); 
            
            string response = _transSvs.ExecuteProcess(xml,1);

            //TODO : Review - Send through 0 as ID as we dont know what this was really
            WriteFile("Cancel-" + type, ID,xml, response, _debugFailedLocation);

            _lastCancellationSent = xml;
            _lastCancellationResponse = response;
        }

        Int32 GetKeyValue(XmlDocument ProcessExecutionResultsXml, string Archetype)
        {
            return Convert.ToInt32(ProcessExecutionResultsXml.SelectSingleNode("ProcessExecutionResults").SelectSingleNode("Keys").SelectSingleNode(Archetype).Attributes["KeyValue"].Value);
        }

        string GetKeyValueString(XmlDocument ProcessExecutionResultsXml, string Archetype)
        {
            return Convert.ToString(ProcessExecutionResultsXml.SelectSingleNode("ProcessExecutionResults").SelectSingleNode("Keys").SelectSingleNode(Archetype).Attributes["KeyValue"].Value);
        }


        /// <summary>
        /// Exports an individual client record
        /// </summary>
        /// <param name="row">Data row from contact query</param>
        /// <returns>the UNO of the inserted contact record</returns>
        protected override object ExportContact(System.Data.DataRow row)
        {
            string Action = "ExportContact";

            //check if logged in and then login if not
            if (!_loggedIn)
               LoginToE3E();

            try
            {
                //Collect data from columns
                string entityType = Convert.ToString(row["ENTITYTYPE"]);
                long contID = Convert.ToInt64(row["CONTID"]);
                string xmlToSend = Convert.ToString(row["ENTITYXML"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                //Store response
                string responseEntity = "";
                //Get the entity ID back
                string entityID = "";

                //Now create the entity
                responseEntity = _transSvs.ExecuteProcess(xmlToSend, 1);

                System.Xml.XmlDocument xdEntity = new System.Xml.XmlDocument();
                xdEntity.LoadXml(responseEntity);

                //Check Result
                switch (CheckResult(xdEntity))
                {
                    case ProcessExecutionResult.Success:
                        //If OK, then we just need to get the ID back and store it
                        //Write to disk
                        WriteFile(Action, contID, xmlToSend, responseEntity, _debugSuccessLocation);
                        entityID = Convert.ToString(GetKeyValue(xdEntity, entityType));
                        return entityID;
                    case ProcessExecutionResult.Interface:
                        //Write to disk
                        WriteFile(Action, contID, xmlToSend, responseEntity, _debugFailedLocation);

                        List<SqlParameter> parList = new List<SqlParameter>();
                        SqlParameter parCONTID = new SqlParameter("CONTID", contID);
                        parList.Add(parCONTID);
                        ExecuteSQL("UPDATE DBCONTACT SET CONTNEEDEXPORT = 0 , CONTEXTID = -1 WHERE CONTID = @CONTID" , parList);
                        //Should process be cancelled?
                        if (_CancelProcess)
                            CancelProcess(GetProcess(xdEntity), GetProcessItemId(xdEntity),contID,Action);

                        //And finally throw an exception so to halt process
                        throw new Exception(GetErrors(xmlToSend, responseEntity, Action,contID));
                    case ProcessExecutionResult.Failure:
                        //Write to disk
                        WriteFile(Action, contID, xmlToSend, responseEntity, _debugFailedLocation);
                        //Complete failure so try again next time or NOT;
                        if (_ResetFlagInCaseOfFailure)
                        {
                            new ResetNeedExportFlagForContactCommand(contID, this).Execute();
                        }
                        throw new Exception(xdEntity.InnerXml);
                    default:
                        throw new Exception("Unknown ProcessExecutionResult");
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }
        protected override bool UpdateContact(System.Data.DataRow row)
        {
            string Action = "UpdateContact";

            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToE3E();

            try
            {
                //Collect data from columns
                string entityType = Convert.ToString(row["ENTITYTYPE"]);
                long contID = Convert.ToInt64(row["CONTID"]);
                string xmlToSend = Convert.ToString(row["ENTITYXML"]);
                int entityIndex = Convert.ToInt32(row["ENTITYINDEX"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");
                
                //Store response
                string responseEntity = "";
                //Get the entity ID back
                string entityID = "";

                int siteIndex = GetEntityDefaultSiteIndex(entityIndex);

                xmlToSend = xmlToSend.Replace("%SITEINDEX%", siteIndex.ToString(System.Globalization.CultureInfo.InvariantCulture));

                //Now create the entity
                responseEntity = _transSvs.ExecuteProcess(xmlToSend, 1);
                

                System.Xml.XmlDocument xdEntity = new System.Xml.XmlDocument();
                xdEntity.LoadXml(responseEntity);

                //Check Result
                switch (CheckResult(xdEntity))
                {
                    case ProcessExecutionResult.Success:
                        //Write to disk
                        WriteFile(Action, contID, xmlToSend, responseEntity, _debugSuccessLocation);
                        //If OK, then we just need to get the ID back and store it
                        entityID = Convert.ToString(GetKeyValue(xdEntity, entityType));
                        return true;
                    case ProcessExecutionResult.Interface:
                        //Write to disk
                        WriteFile(Action, contID, xmlToSend, responseEntity, _debugFailedLocation);
                        if (_CancelProcess)
                            CancelProcess(GetProcess(xdEntity), GetProcessItemId(xdEntity),contID,Action);
                        //And finally throw an exception so to halt process
                        throw new Exception(GetErrors(xmlToSend, responseEntity, Action, contID));
                    case ProcessExecutionResult.Failure:
                        //Write to disk
                        WriteFile(Action, contID, xmlToSend, responseEntity, _debugFailedLocation);
                        //Complete failure so try again next time or NOT;
                        if (_ResetFlagInCaseOfFailure)
                        {
                            new ResetNeedExportFlagForContactCommand(contID, this).Execute();
                        }
                        throw new Exception(xdEntity.InnerXml);
                    default:
                        throw new Exception("Unknown ProcessExecutionResult");
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        void PostProcessInstructions(string action, long id, string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XmlDocument processes = new XmlDocument();
                processes.LoadXml(xml);

                System.Xml.XmlElement processesRoot = processes.DocumentElement;
                System.Xml.XmlNodeList process = processesRoot.SelectNodes("process");
                foreach (System.Xml.XmlNode instruction in process)
                {
                    System.Diagnostics.Debug.WriteLine(instruction.InnerXml);
                    //Just send to 3E for now...  dont expect anything back.
                    string result = _transSvs.ExecuteProcess(instruction.InnerXml, 1);
                    //But write the result
                    XmlDocument resultXml = new XmlDocument();
                    resultXml.LoadXml(result);
                    switch (CheckResult(resultXml))
                    {
                        case ProcessExecutionResult.Success:
                            if (instruction.Attributes["expectedResult"].Value == "Success")
                            {
                                ProcessInstruction(action, id, instruction, result, resultXml);
                            }
                            else
                                WriteFile(string.Format("PostProcessAction(SUCCESS)-{0}", action), id, instruction.InnerXml, result, _debugFailedLocation);
                            break;
                        case ProcessExecutionResult.Interface:
                            if (instruction.Attributes["expectedResult"].Value == "Interface")
                            {
                                ProcessInstruction(action, id, instruction, result, resultXml);
                            }
                            else
                                WriteFile(string.Format("PostProcessAction(INTERFACE)-{0}", action), id, instruction.InnerXml, result, _debugFailedLocation);
                            break;
                        case ProcessExecutionResult.Failure:
                            WriteFile(string.Format("PostProcessAction(FAILURE)-{0}", action), id, instruction.InnerXml, result, _debugFailedLocation);
                            break;
                    }
                }
            }
        }

        private void ProcessInstruction(string action, long id, System.Xml.XmlNode instruction, string result, XmlDocument resultXml)
        {
            WriteFile(string.Format("PostProcessAction(ProcessInstruction)-{0}", action), id, instruction.InnerXml, result, _debugSuccessLocation);
            if (instruction.Attributes["matterSphereUpdate"] != null)
            {
                List<SqlParameter> parList = new List<SqlParameter>();

                string expectedResultType = instruction.Attributes["expectedResultType"].Value;
                string keyValue = instruction.Attributes["keyValue"].Value;
                string command = instruction.Attributes["matterSphereUpdate"].Value;

                if (Convert.ToBoolean(instruction.Attributes["getProcessResult"].Value))
                {
                    SqlParameter par;
                    switch (expectedResultType.ToUpper())
                    {
                        case "STRING":
                            par = new SqlParameter("PROCESSRESULT", GetKeyValueString(resultXml, keyValue));
                            break;
                        case "INT32":
                            par = new SqlParameter("PROCESSRESULT", GetKeyValue(resultXml, keyValue));
                            break;
                        default:
                            throw new Exception(string.Format("PostProcessInstructions error : expectedResultType of {0} not supported", expectedResultType));
                    }


                    parList.Add(par);
                }

                ExecuteSQL(command, parList);
            }
        }
    }
}
