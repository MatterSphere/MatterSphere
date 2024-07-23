using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using FWBS.OMS.OMSEXPORT.ResetFlagCommands;

namespace FWBS.OMS.OMSEXPORT
{
    public class OMSExportC3E : OMSExportBase, ITokenStorageProvider, IDisposable
    {
        #region Fields

        private ApiRequester _requester;

        /// <summary>
        /// Used when calling registry read function 
        /// </summary>
        private const string APPNAME = "C3E";
        /// <summary>
        /// Monitor if logged in to 3E
        /// </summary>
        private bool _loggedIn = false;

        /// <summary>
        /// Custom property for running matter export conflict check CftWFNewBizIntake
        /// </summary>
        private bool _matterExportPostProcess = false;

        /// <summary>
        /// Custom property to reset need export flag in database in case of failure.
        /// </summary>
        private bool _ResetFlagInCaseOfFailure = true;

        /// <summary>
        /// Debug Success Location
        /// </summary>
        private string _debugSuccessLocation = "";

        /// <summary>
        /// Debug Failed Location
        /// </summary>
        private string _debugFailedLocation = "";

        #endregion

        protected override string dataFormat => "json";

        #region 3E Cloud Specific Functions

        /// <summary>
        /// Logs into 3E Cloud and sets cookie containers on all 3E objects
        /// </summary>
        private void LoginToC3E()
        {
            RaiseStatusEvent("Logging into 3E Cloud system.");

            try
            {
                var env = StaticLibrary.GetSetting("AADEnv", APPNAME, "");
                string relativeUrl = string.IsNullOrEmpty(env) ?
                        StaticLibrary.GetSetting(ApiRelativeUrlOnPremKey, "", ApiRelativeUrlOnPremDefault) :
                        StaticLibrary.GetSetting(ApiRelativeUrlCloudKey, "", ApiRelativeUrlCloudDefault);
                

                string baseURL = StaticLibrary.GetSetting("BaseURL", APPNAME, "");
                var aadParams = new KeyValuePair<string, string>(
                    StaticLibrary.GetSetting("AADEnv", APPNAME, ""), 
                    StaticLibrary.GetSetting("AADConfig", APPNAME, ""));

                _matterExportPostProcess = Convert.ToBoolean(StaticLibrary.GetSetting("MatterConflictCheck", APPNAME, "False"));
                _ResetFlagInCaseOfFailure = Convert.ToBoolean(StaticLibrary.GetSetting("ResetFlagInCaseOfFailure", APPNAME, "False"));
                _debugMode = Convert.ToBoolean(StaticLibrary.GetSetting("Debug", APPNAME, "False"));
                _debugSuccessLocation = StaticLibrary.GetSetting("SuccessLocation", APPNAME, "");
                _debugFailedLocation = StaticLibrary.GetSetting("FailedLocation", APPNAME, "");

                if (baseURL == "")
                    throw new ApplicationException("3E Cloud settings not configured.  Please check your settings.");

                if (_requester == null)
                {
                    var apiUrl = baseURL.TrimEnd('/') + "/" + relativeUrl;

                    _requester = new ApiRequester(apiUrl, aadParams, this, DefaultCredentials, _debugMode);
                }

                var infoResponse = _requester.Info();
                if (!infoResponse.Success)
                    throw new Exception(infoResponse.ErrorMessage);

                RaiseStatusEvent("Logged into 3E Cloud system.");
                _loggedIn = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Logging in to 3E Cloud: " + ex.Message, ex);
            }
        }


        /// <summary>
        /// Logs out of the C3E system, use at the end of a run
        /// </summary>
        private void LogoutOfC3E()
        {
            try
            {
                if (_loggedIn)
                {
                    //null all web service variables
                    _requester = null;
                }

                //finally set flag
                _loggedIn = false;
            }
            catch (Exception ex)
            {
                base.LogEntry("LogoutOfC3E: " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
            }
        }

        #endregion


        /// <summary>
        /// Nothing to implement here for this object
        /// Most is the first time any work needs doing
        /// </summary>
        protected override void InitialiseProcess()
        {
        }

        /// <summary>
        /// perform any finishing off
        /// </summary>
        protected override void FinaliseProcess()
        {
            try
            {
                //Log out of C3E
                RaiseStatusEvent("Logging out of 3E Cloud");
                LogoutOfC3E();
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
            string Action = "ExportClient";

            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToC3E();

            try
            {
                //Collect data from columns
                long contID = Convert.ToInt64(row["DEFAULTCONTACTID"]);
                long clID = Convert.ToInt64(row["CLID"]);
                string jsonToSend, xmlToSend = Convert.ToString(row["Client"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                //Replace %INVOICESITE% 
                if (xmlToSend.IndexOf("%INVOICESITE%") > 0)
                {
                    var attributes = GetEntityDefaultIndexes(contID, row["ENTITYINDEX"], row["ENTITYID"]);
                    if (attributes != null)
                    {
                        jsonToSend = Models.XmlToJson.ClientRequest(xmlToSend, attributes);
                        Models.GenericResponse response = _requester.CreateClient(jsonToSend);

                        if (response.Success)
                        {
                            //If OK, then we just need to get the ID back and store it
                            //Write to disk
                            WriteFile(Action, clID, jsonToSend, response.Dump, _debugSuccessLocation);
                            return response.GetItemID();
                        }
                        else
                        {
                            //Write to disk
                            WriteFile(Action, clID, jsonToSend, response.Dump, _debugFailedLocation);
                            //Complete failure so try again next time or NOT;
                            if (_ResetFlagInCaseOfFailure)
                            {
                                new ResetNeedExportFlagForClientCommand(clID, this).Execute();
                            }
                            throw new Exception(GetErrors(jsonToSend, response.ErrorMessage, Action, clID));
                        }
                    }
                    else
                    {
                        List<SqlParameter> parList = new List<SqlParameter>();
                        SqlParameter parCLID = new SqlParameter("CLID", clID);
                        parList.Add(parCLID);
                        ExecuteSQL("UPDATE DBCLIENT SET CLNEEDEXPORT = CLNEEDEXPORT, CLEXTID = -1 WHERE CLID = @CLID", parList);
                        throw new Exception("Could not get default site index for entity");
                    }
                }
                else
                {
                    List<SqlParameter> parList = new List<SqlParameter>();
                    SqlParameter parCLID = new SqlParameter("CLID", clID);
                    parList.Add(parCLID);
                    ExecuteSQL("UPDATE DBCLIENT SET CLNEEDEXPORT = CLNEEDEXPORT, CLEXTID = -1 WHERE CLID = @CLID", parList);
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
                LoginToC3E();

            try
            {
                //Only updates a client - does not update entity information
                long clID = Convert.ToInt64(row["CLID"]);
                string jsonToSend, xmlToSend = Convert.ToString(row["Client"]);
                object clientIndex = row["CLIENTINDEX"];
                object clientID = row["CLIENTID"];

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                //Replace %INVOICESITE% 
                if (xmlToSend.IndexOf("%INVOICESITE%") > 0)
                {
                    long contID = Convert.ToInt64(row["DEFAULTCONTACTID"]);
                    var attributes = GetEntityDefaultIndexes(contID, row["ENTITYINDEX"], row["ENTITYID"]);
                    if (attributes != null)
                    {
                        xmlToSend = xmlToSend.Replace("%INVOICESITE%", attributes.SiteIndex);
                    }
                    else
                        throw new Exception("Could not get default site index for entity");
                }

                Models.FindClientAttribute clattributes = null;
                //If XML contains {0} we know the EffectiveDated flag has been set
                if (DBNull.Value.Equals(clientID) || xmlToSend.IndexOf("{0}") > 0)
                {
                    //Use this date for now?  we dont know what time zone the change was made in - do we need to worry about this?
                    clattributes = GetClientDefaultIndexes(clID, clientIndex, clientID, DateTime.UtcNow.Date);
                    if (clattributes == null)
                    {
                        throw new Exception("Unable to resolve Indexes for Client = " + clID.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    }

                    if (xmlToSend.IndexOf("{0}") > 0)
                    {
                        //check if there is an effective dated row for the
                        if (!string.IsNullOrEmpty(clattributes.CliDateID))
                        {
                            //A row was returned, so must be edit mode
                            xmlToSend = string.Format(xmlToSend, "Edit", @"KeyField=""EffStart"" KeyValue=""" + clattributes.EffStart + @"""", clattributes.EffStart);
                        }
                        else
                        {
                            //Adding a new effective date for the client
                            xmlToSend = string.Format(xmlToSend, "Add", "", clattributes.EffStart);
                        }
                    }
                }
                else
                {
                    clattributes = new Models.FindClientAttribute()
                    {
                        ClientID = Convert.ToString(clientID),
                        ClientIndex = Convert.ToString(clientIndex, System.Globalization.CultureInfo.InvariantCulture)
                    };
                }

                jsonToSend = Models.XmlToJson.ClientRequest(xmlToSend, clattributes);
                Models.GenericResponse response = _requester.UpdateClient(jsonToSend);

                if (response.Success)
                {
                    //If OK, then we just need to get the ID back and store it
                    //Write to disk
                    WriteFile(Action, clID, jsonToSend, response.Dump, _debugSuccessLocation);
                    return true;
                }
                else
                {
                    //Write to disk
                    WriteFile(Action, clID, jsonToSend, response.Dump, _debugFailedLocation);
                    if (response.IsRecordLocked())
                        return false;

                    //Complete failure so try again next time or NOT;
                    if (_ResetFlagInCaseOfFailure)
                    {
                        new ResetNeedExportFlagForClientCommand(clID, this).Execute();
                    }
                    throw new Exception(GetErrors(jsonToSend, response.ErrorMessage, Action, clID));
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
            string Action = "ExportMatter";

            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToC3E();

            try
            {
                long fileID = Convert.ToInt64(row["FILEID"]);
                string jsonToSend, xmlToSend = Convert.ToString(row["Matter"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                Models.FindClientAttribute attributes = null;
                if (xmlToSend.IndexOf("%CLIENT%") > 0)
                {
                    attributes = GetClientDefaultIndexes(Convert.ToInt64(row["CLID"]), DBNull.Value, row["CLIENTID"], DateTime.UtcNow.Date);
                }

                jsonToSend = Models.XmlToJson.MatterRequest(xmlToSend, attributes);
                Models.GenericResponse response = _requester.CreateMatter(jsonToSend);

                if (response.Success)
                {
                    //Write to disk
                    WriteFile(Action, fileID, jsonToSend, response.Dump, _debugSuccessLocation);
                    string mattExtTxtId = response.GetItemID();
                    //Perform post process - but the MattIndex hasnt been populated to the database yet, so have to get from the result
                    if (_matterExportPostProcess)
                    {
                        ExportMatterPostProcess(Action, fileID, mattExtTxtId);
                    }
                    return mattExtTxtId;
                }
                else
                {
                    //Write to disk
                    WriteFile(Action, fileID, jsonToSend, response.Dump, _debugFailedLocation);
                    //Complete failure so try again next time or NOT;
                    if (_ResetFlagInCaseOfFailure)
                    {
                        new ResetNeedExportFlagForMatterCommand(fileID, this).Execute();
                    }
                    throw new Exception(GetErrors(jsonToSend, response.ErrorMessage, Action, fileID));
                }

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        private void ExportMatterPostProcess(string Action, long fileID, string mattExtTxtId)
        {
            /* Wrap in a try catch, as if the matter succeeds, then we still want to report a success, but we do need to log a post process failure.  
            *  Event log is place to log for now
            *  TODO: where else to log more effectively
            */
            try
            {
                var matterResponse = _requester.GetMatter(mattExtTxtId);
                if (!matterResponse.Success)
                    throw new Exception(matterResponse.ErrorMessage);

                //Create Parameters
                SqlParameter parFileID = new SqlParameter("FILEID", fileID);
                SqlParameter parMattIndex = new SqlParameter("MATTINDEX", Convert.ToInt32(matterResponse.GetMatterIndex()));
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
        }

        protected override bool UpdateMatter(System.Data.DataRow row)
        {
            string Action = "UpdateMatter";

            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToC3E();

            try
            {
                long fileID = Convert.ToInt64(row["FILEID"]);
                object mattIndex = row["MATTINDEX"];
                object mattID = row["MATTID"];
                string jsonToSend, xmlToSend = Convert.ToString(row["Matter"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                Models.FindMatterAttribute mattAtributes = null;
                //If XML contains {0} we know the EffectiveDated flag has been set
                if (DBNull.Value.Equals(mattID) || xmlToSend.IndexOf("{0}") > 0)
                {
                    mattAtributes = GetMatterDefaultIndexes(fileID, mattIndex, mattID, DateTime.UtcNow.Date);
                    if (mattAtributes == null)
                    {
                        throw new Exception("Unable to resolve Indexes for Matter = " + fileID.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    }

                    if (xmlToSend.IndexOf("{0}") > 0)
                    {
                        //check if there is an effective dated row for the
                        if (!string.IsNullOrEmpty(mattAtributes.MattDateID))
                        {
                            //A row was returned, so must be edit mode
                            xmlToSend = string.Format(xmlToSend, "Edit", @"KeyField=""EffStart"" KeyValue=""" + mattAtributes.EffStart + @"""", mattAtributes.EffStart);
                        }
                        else
                        {
                            //Adding a new effective date for the matter
                            xmlToSend = string.Format(xmlToSend, "Add", "", mattAtributes.EffStart);
                        }
                    }
                }
                else
                {
                    mattAtributes = new Models.FindMatterAttribute()
                    {
                        MatterID = Convert.ToString(mattID),
                        MattIndex = Convert.ToString(mattIndex, System.Globalization.CultureInfo.InvariantCulture)
                    };
                }

                jsonToSend = Models.XmlToJson.MatterRequest(xmlToSend, mattAtributes);
                Models.GenericResponse response = _requester.UpdateMatter(jsonToSend);

                if (response.Success)
                {
                    //Write to disk
                    WriteFile(Action, fileID, jsonToSend, response.Dump, _debugSuccessLocation);//If OK, then we just need to get the ID back and store it
                    return true;
                }
                else
                {
                    //Write to disk
                    WriteFile(Action, fileID, jsonToSend, response.Dump, _debugFailedLocation);
                    if (response.IsRecordLocked())
                        return false;

                    //Complete failure so try again next time or NOT;
                    if (_ResetFlagInCaseOfFailure)
                    {
                        new ResetNeedExportFlagForClientCommand(fileID, this).Execute();
                    }
                    throw new Exception(GetErrors(jsonToSend, response.ErrorMessage, Action, fileID));
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
            string Action = "ExportTimeCardPending";
            System.Threading.Thread.Sleep(10);

            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToC3E();

            try
            {
                long timeID = Convert.ToInt64(row["ID"]);
                string jsonToSend, xmlToSend = Convert.ToString(row["TimeCardPending"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                Models.FindMatterAttribute attributes = null;
                if (xmlToSend.IndexOf("%MATTER%") > 0)
                {
                    attributes = GetMatterDefaultIndexes(Convert.ToInt64(row["FILEID"]), row["MATTINDEX"], row["MATTID"], DateTime.UtcNow.Date);
                }

                jsonToSend = Models.XmlToJson.TimeRequest(xmlToSend, attributes);
                Models.GenericResponse response = _requester.CreateTimeCard(jsonToSend);

                if (response.Success)
                {
                    //Write to disk
                    WriteFile(Action, timeID, jsonToSend, response.Dump, _debugSuccessLocation);
                    return response.GetItemID();
                }
                else
                {
                    //Write to disk
                    WriteFile(Action, timeID, jsonToSend, response.Dump, _debugFailedLocation);
                    //Complete failure so try again next time;
                    throw new Exception(GetErrors(jsonToSend, response.ErrorMessage, Action, timeID));
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
            string Action = "ExportCostCardPending";
            System.Threading.Thread.Sleep(10);

            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToC3E();

            try
            {
                long finLogID = Convert.ToInt64(row["ID"]);
                string jsonToSend, xmlToSend = Convert.ToString(row["CostCardPending"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                Models.FindMatterAttribute attributes = null;
                if (xmlToSend.IndexOf("%MATTER%") > 0)
                {
                    attributes = GetMatterDefaultIndexes(Convert.ToInt64(row["FILEID"]), row["MATTINDEX"], row["MATTID"], DateTime.UtcNow.Date);
                }

                jsonToSend = Models.XmlToJson.CostCardRequest(xmlToSend, attributes);
                Models.GenericResponse response = _requester.CreateCostCard(jsonToSend);

                if (response.Success)
                {
                    //Write to disk
                    WriteFile(Action, finLogID, jsonToSend, response.Dump, _debugSuccessLocation);
                    return response.GetItemID();
                }
                else
                {
                    //Write to disk
                    WriteFile(Action, finLogID, jsonToSend, response.Dump, _debugFailedLocation);
                    //Complete failure so try again next time;
                    throw new Exception(GetErrors(jsonToSend, response.ErrorMessage, Action, finLogID));
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

        /// <summary>
        /// Resolves Entity and Site attributes for a given Contact
        /// </summary>
        /// <param name="contID">MSP contID</param>
        /// <param name="extId">MSP contExtID / 3E EntityIndex</param>
        /// <param name="extTxtId">MSP contExtTxtID / 3E EntityID</param>
        /// <returns>Contact attributes</returns>
        private Models.FindContactAttribute GetEntityDefaultIndexes(long contID, object extId, object extTxtId)
        {
            int contExtId = DBNull.Value.Equals(extId) ? 0 : Convert.ToInt32(extId);
            string contExtTxtId = Convert.ToString(extTxtId);
            Models.FindContactAttribute attributes = null;

            var findRequest = string.IsNullOrEmpty(contExtTxtId)
                ? new Models.FindContactRequest(contExtId)
                : new Models.FindContactRequest(contExtTxtId);

            var findResponse = _requester.FindContact(findRequest);
            if (findResponse.Success && findResponse.RowCount == 1)
            {
                attributes = findResponse.Rows[0].Attributes;

                if (contExtId == 0 || string.IsNullOrEmpty(contExtTxtId))
                {
                    contExtId = Convert.ToInt32(attributes.EntIndex);
                    contExtTxtId = attributes.EntityID;

                    List<SqlParameter> parList = new List<SqlParameter>();
                    parList.Add(new SqlParameter("CONTID", contID));
                    parList.Add(new SqlParameter("CONTEXTID", contExtId));
                    parList.Add(new SqlParameter("CONTEXTTXTID", contExtTxtId));
                    string sql = "UPDATE dbContact SET contNeedExport = contNeedExport, contExtID = @CONTEXTID, contExtTxtID = @CONTEXTTXTID WHERE contID = @CONTID";
                    ExecuteSQL(sql, parList);
                }
            }
            else
            {
                WriteFile("FindContact", contID, Newtonsoft.Json.JsonConvert.SerializeObject(findRequest), findResponse.Dump, _debugFailedLocation);
            }

            return attributes;
        }

        /// <summary>
        /// Resolves attributes for a given Client
        /// </summary>
        /// <param name="clID">MSP clID</param>
        /// <param name="extId">MSP clExtID / 3E ClientIndex</param>
        /// <param name="extTxtId">MSP clExtTxtID / 3E ClientID</param>
        /// <param name="effectiveDate">Desired effective date</param>
        /// <returns>Client attributes</returns>
        private Models.FindClientAttribute GetClientDefaultIndexes(long clID, object extId, object extTxtId, DateTime effectiveDate)
        {
            int clExtId = DBNull.Value.Equals(extId) ? 0 : Convert.ToInt32(extId);
            string clExtTxtId = Convert.ToString(extTxtId);
            Models.FindClientAttribute attributes = null;

            var clientResponse = string.IsNullOrEmpty(clExtTxtId)
                ? _requester.GetClient(clExtId)
                : _requester.GetClient(clExtTxtId);

            if (clientResponse.Success && clientResponse.Rows.Length == 1)
            {
                attributes = new Models.FindClientAttribute()
                {
                    ClientID = clientResponse.GetClientID(),
                    ClientIndex = clientResponse.GetClientIndex(),
                    CliDateID = clientResponse.GetEffectiveCliDateID(effectiveDate.ToString("yyyy-MM-dd")),
                    EffStart = effectiveDate.ToString("yyyy-MM-dd")
                };

                if (clExtId == 0 || string.IsNullOrEmpty(clExtTxtId))
                {
                    clExtId = Convert.ToInt32(attributes.ClientIndex);
                    clExtTxtId = attributes.ClientID;

                    List<SqlParameter> parList = new List<SqlParameter>();
                    parList.Add(new SqlParameter("CLID", clID));
                    parList.Add(new SqlParameter("CLEXTID", clExtId));
                    parList.Add(new SqlParameter("CLEXTTXTID", clExtTxtId));
                    string sql = "UPDATE dbClient SET clNeedExport = clNeedExport, clExtID = @CLEXTID, clExtTxtID = @CLEXTTXTID WHERE clID = @CLID";
                    ExecuteSQL(sql, parList);
                }
            }
            else
            {
                WriteFile("GetClient", clID, Newtonsoft.Json.JsonConvert.SerializeObject(string.IsNullOrEmpty(clExtTxtId) ? (object)clExtId : clExtTxtId), clientResponse.Dump, _debugFailedLocation);
            }

            return attributes;
        }

        private Models.FindMatterAttribute GetMatterDefaultIndexes(long mattID, object extId, object extTxtId, DateTime effectiveDate)
        {
            int mattExtId = DBNull.Value.Equals(extId) ? 0 : Convert.ToInt32(extId);
            string mattExtTxtId = Convert.ToString(extTxtId);
            Models.FindMatterAttribute attributes = null;

            var matterResponse = string.IsNullOrEmpty(mattExtTxtId)
                ? _requester.GetMatter(mattExtId)
                : _requester.GetMatter(mattExtTxtId);

            if (matterResponse.Success && matterResponse.Rows.Length == 1)
            {
                attributes = new Models.FindMatterAttribute()
                {
                    MatterID = matterResponse.GetMatterID(),
                    MattIndex = matterResponse.GetMatterIndex(),
                    MattDateID = matterResponse.GetEffectiveMattDateID(effectiveDate.ToString("yyyy-MM-dd")),
                    EffStart = effectiveDate.ToString("yyyy-MM-dd")
                };

                if (mattExtId == 0 || string.IsNullOrEmpty(mattExtTxtId))
                {
                    mattExtId = Convert.ToInt32(attributes.MattIndex);
                    mattExtTxtId = attributes.MatterID;

                    List<SqlParameter> parList = new List<SqlParameter>();
                    parList.Add(new SqlParameter("FILEID", mattID));
                    parList.Add(new SqlParameter("FILEEXTID", mattExtId));
                    parList.Add(new SqlParameter("FILEEXTTXTID", mattExtTxtId));
                    string sql = "UPDATE dbFile SET fileNeedExport = fileNeedExport, fileExtLinkID = @FILEEXTID, fileExtLinkTxtID = @FILEEXTTXTID WHERE fileID = @FILEID";
                    ExecuteSQL(sql, parList);
                }
            }
            else
            {
                WriteFile("GetMatter", mattID, Newtonsoft.Json.JsonConvert.SerializeObject(string.IsNullOrEmpty(mattExtTxtId) ? (object)mattExtId : mattExtTxtId), matterResponse.Dump, _debugFailedLocation);
            }

            return attributes;
        }


        //Build up a new string for errors compiled of as much informaton in message as possible
        string GetErrors(string xmlSent, string xmlResponse, string actionDescription, long internalID)
        {
            try
            {
                System.IO.StringWriter s = new System.IO.StringWriter();
                XmlWriterSettings settings = new XmlWriterSettings() { Indent = true };
                XmlWriter errorPacket = XmlWriter.Create(s, settings);

                errorPacket.WriteStartElement("ErrorMessageInfo");

                errorPacket.WriteStartElement("Information");
                errorPacket.WriteAttributeString("ActionDescription",actionDescription);
                errorPacket.WriteAttributeString("InternalID",internalID.ToString());
                errorPacket.WriteEndElement();  //Information
                

                //Sent
                errorPacket.WriteStartElement("Sent");
                errorPacket.WriteRaw(xmlSent);
                errorPacket.WriteEndElement();  //Sent

                //Response
                errorPacket.WriteStartElement("Response");
                errorPacket.WriteRaw(xmlResponse);
                errorPacket.WriteEndElement();  //Response

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
               LoginToC3E();

            try
            {
                //Collect data from columns
                long contID = Convert.ToInt64(row["CONTID"]);
                string entityType = Convert.ToString(row["ENTITYTYPE"]);
                string jsonToSend, xmlToSend = Convert.ToString(row["ENTITYXML"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                Models.GenericResponse response = null;
                switch (entityType)
                {
                    case "EntityPerson":
                        jsonToSend = Models.XmlToJson.PersonalContactRequest(xmlToSend);
                        response = _requester.CreatePersonContact(jsonToSend);
                        break;
                    case "EntityOrg":
                        jsonToSend = Models.XmlToJson.OrganizationContactRequest(xmlToSend);
                        response = _requester.CreateOrgContact(jsonToSend);
                        break;
                    default:
                        throw new Exception("Unknown entityType: " + entityType);
                }

                if (response.Success)
                {
                    //If OK, then we just need to get the ID back and store it
                    //Write to disk
                    WriteFile(Action, contID, jsonToSend, response.Dump, _debugSuccessLocation);
                    return response.GetItemID();
                }
                else
                {
                    //Write to disk
                    WriteFile(Action, contID, jsonToSend, response.Dump, _debugFailedLocation);
                    //Complete failure so try again next time or NOT;
                    if (_ResetFlagInCaseOfFailure)
                    {
                        new ResetNeedExportFlagForContactCommand(contID, this).Execute();
                    }
                    throw new Exception(GetErrors(jsonToSend, response.ErrorMessage, Action, contID));
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
                LoginToC3E();

            try
            {
                //Collect data from columns
                long contID = Convert.ToInt64(row["CONTID"]);
                string entityType = Convert.ToString(row["ENTITYTYPE"]);
                string jsonToSend, xmlToSend = Convert.ToString(row["ENTITYXML"]);

                if (xmlToSend == "")
                    throw new Exception("XML not supplied");

                // Need to resolve %ENTITYID%, %RELATEID%, %SITEID%
                var attributes = GetEntityDefaultIndexes(contID, row["ENTITYINDEX"], row["ENTITYID"]);
                if (attributes == null)
                {
                    throw new Exception("Unable to resolve Indexes for Contact = " + contID.ToString(System.Globalization.CultureInfo.InvariantCulture));
                }

                Models.GenericResponse response = null;
                switch (entityType)
                {
                    case "EntityPerson":
                        jsonToSend = Models.XmlToJson.PersonalContactRequest(xmlToSend, attributes);
                        response = _requester.UpdateContact(jsonToSend);
                        break;
                    case "EntityOrg":
                        jsonToSend = Models.XmlToJson.OrganizationContactRequest(xmlToSend, attributes);
                        response = _requester.UpdateContact(jsonToSend);
                        break;
                    default:
                        throw new Exception("Unknown entityType: " + entityType);
                }

                if (response.Success)
                {
                    //If OK, then we just need to get the ID back and store it
                    //Write to disk
                    WriteFile(Action, contID, jsonToSend, response.Dump, _debugSuccessLocation);
                    return true;
                }
                else
                {
                    //Write to disk
                    WriteFile(Action, contID, jsonToSend, response.Dump, _debugFailedLocation);
                    if (response.IsRecordLocked())
                        return false;
                    
                    //Complete failure so try again next time or NOT;
                    if (_ResetFlagInCaseOfFailure)
                    {
                        new ResetNeedExportFlagForContactCommand(contID, this).Execute();
                    }
                    throw new Exception(GetErrors(jsonToSend, response.ErrorMessage, Action, contID));
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }


        private void PostProcessInstructions(string action, long id, string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XmlDocument processes = new XmlDocument();
                processes.LoadXml(xml);

                XmlElement processesRoot = processes.DocumentElement;
                XmlNodeList process = processesRoot.SelectNodes("process");
                foreach (XmlNode instruction in process)
                {
                    if (instruction.FirstChild.LocalName != "CftWFNewBizIntake")
                    {
                        LogEntry(string.Format("PostProcessInstructions : Unsupported request type: {0}", instruction.FirstChild.LocalName), System.Diagnostics.EventLogEntryType.Warning);
                        continue;
                    }

                    // Resolve Entity index
                    XmlNodeList entities = instruction.SelectNodes(".//*[name()='NBI:Entity']");
                    foreach (XmlNode entity in entities)
                    {
                        if (string.IsNullOrEmpty(entity.InnerText) && !string.IsNullOrEmpty(entity.Attributes["extId"]?.Value))
                        {
                            entity.InnerText = GetEntityDefaultIndexes(Convert.ToInt64(entity.Attributes["Id"].Value), DBNull.Value, entity.Attributes["extId"].Value).EntIndex;
                        }
                    }

                    // Resolve submitter NetworkAlias into UserId
                    XmlNode submitter = instruction.SelectSingleNode(".//*[name()='NBI:Submitter']");
                    if (submitter.InnerText != "ERROR")
                    {
                        var userResponse = _requester.GetUserInfo(submitter.InnerText);
                        if (userResponse.Success)
                        {
                            submitter.InnerText = userResponse.User.UserId;
                        }
                        else
                        {
                            var meResponse = _requester.GetMeInfo();
                            if (meResponse.Success)
                                submitter.InnerText = meResponse.UserId;
                        }
                    }

                    System.Diagnostics.Debug.WriteLine(instruction.InnerXml);
                    //Just send to 3E for now...  dont expect anything back.
                    string jsonToSend = Models.XmlToJson.NBISearchRequest(instruction.InnerXml);
                    Models.GenericResponse response = _requester.CreateNBISearch(jsonToSend);
                    //But write the result
                    if (response.Success)
                    {
                        if (instruction.Attributes["expectedResult"].Value == "Success" || instruction.Attributes["expectedResult"].Value == "Interface")
                        {
                            WriteFile(string.Format("PostProcessAction(ProcessInstruction)-{0}", action), id, jsonToSend, response.Dump, _debugSuccessLocation);
                            ProcessInstruction(action, id, instruction, response);
                        }
                        else
                        {
                            WriteFile(string.Format("PostProcessAction({1})-{0}", action, instruction.Attributes["expectedResult"].Value.ToUpper()), id, jsonToSend, response.Dump, _debugFailedLocation);
                        }
                    }
                    else
                    {
                        WriteFile(string.Format("PostProcessAction(FAILURE)-{0}", action), id, jsonToSend, response.Dump, _debugFailedLocation);
                        throw new Exception(response.ErrorMessage);
                    }
                }
            }
        }

        private void ProcessInstruction(string action, long id, XmlNode instruction, Models.GenericResponse response)
        {
            if (instruction.Attributes["matterSphereUpdate"] != null)
            {
                List<SqlParameter> parList = new List<SqlParameter>();

                string expectedResultType = instruction.Attributes["expectedResultType"].Value;

                string command = instruction.Attributes["matterSphereUpdate"].Value;
                string itemId = response.GetItemID();

                if (Convert.ToBoolean(instruction.Attributes["getProcessResult"].Value))
                {
                    SqlParameter par;
                    switch (expectedResultType.ToUpper())
                    {
                        case "STRING":
                            par = new SqlParameter("PROCESSRESULT", itemId);
                            break;
                        case "INT32":
                            var nbiSeachResponse = _requester.GetNBISearch(itemId);
                            par = new SqlParameter("PROCESSRESULT", nbiSeachResponse.GetSearchIndex());
                            break;
                        default:
                            throw new Exception(string.Format("PostProcessInstructions error : expectedResultType of {0} not supported", expectedResultType));
                    }

                    parList.Add(par);
                }

                ExecuteSQL(command, parList);
            }
        }

        protected override void CustomProcess()
        {
            if (Convert.ToBoolean(StaticLibrary.GetSetting("ImportInvoices", APPNAME, "False")))
            {
                RaiseStatusEvent("Getting Invoices for import.");

                if (!_loggedIn)
                    LoginToC3E();

                DateTime timeStamp = InvoiceSyncTimeStamp.Load(OMSConnection);
                if (ImportInvoices(ref timeStamp))
                    InvoiceSyncTimeStamp.Save(OMSConnection, timeStamp);
            }
        }

        private bool ImportInvoices(ref DateTime timeStamp)
        {
            int impSucceed = 0, impFailed = 0;
            int delSucceed = 0, delFailed = 0;
            try
            {
                string Action = "FindInvoices";
                var findInvoiceRequest = new Models.FindInvoiceAttachmentRequest(timeStamp);
                string jsonToSend = _debugMode ? Newtonsoft.Json.JsonConvert.SerializeObject(findInvoiceRequest) : null;
                timeStamp = DateTime.UtcNow;

                var response = _requester.FindInvoiceAttachments(findInvoiceRequest);
                if (response.Success)
                {
                    WriteFile(Action, 0, jsonToSend, response.Dump, _debugSuccessLocation);
                    RaiseStatusEvent(string.Format("Got {0} Invoice attachments to process.", response.RowCount));
                    if (response.RowCount > 0)
                    {
                        using (var documentStorer = new OMSDocumentStorer())
                        {
                            foreach (var row in response.Rows)
                            {
                                Models.FindInvoiceAttachmentAttribute att = row.Attributes;
                                if (att.IsReversed == 0)
                                {
                                    byte[] fileData = DownloadInvoiceAttachment(att.InvoiceIndex, att.AttachmentID);
                                    if (fileData != null && fileData.Length != 0)
                                    {
                                        if (documentStorer.ImportDocument(att.MattIndex, att.MatterID, att.AttachmentID, att.FileName, att.Description, fileData))
                                            impSucceed++;
                                        else
                                            impFailed++;
                                    }
                                    else
                                    {
                                        LogEntry(string.Format("Failed to download attachment {0} of invoice {1}.", att.AttachmentID, att.InvoiceNumber), System.Diagnostics.EventLogEntryType.Error);
                                        impFailed++;
                                    }
                                }
                                else
                                {
                                    if (documentStorer.DeleteDocument(att.AttachmentID))
                                        delSucceed++;
                                    else
                                        delFailed++;
                                }
                            }
                        }

                        RaiseStatusEvent(string.Format("{0} of {1} invoice attachments imported, {2} of {3} reversed invoices deleted.",
                            impSucceed, impSucceed + impFailed, delSucceed, delSucceed + delFailed));
                    }
                    return response.RowCount > 0;
                }
                else
                {
                    WriteFile(Action, 0, jsonToSend, response.Dump, _debugFailedLocation);
                    throw new Exception(GetErrors(jsonToSend, response.ErrorMessage, Action, 0));
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        private byte[] DownloadInvoiceAttachment(int invoiceIndex, string attachmentID)
        {
            Models.BaseResponse response = _requester.GetAttachment(attachmentID);
            if (response.Success)
            {
                return response.RawBytes;
            }
            else
            {
                WriteFile("GetAttachment", invoiceIndex, Newtonsoft.Json.JsonConvert.SerializeObject(attachmentID), response.Dump, _debugFailedLocation);
                return null;
            }
        }

        #region ITokenStorageProvider

        private const string SITE_CODE = "3E";

        string ITokenStorageProvider.LoadToken(out DateTime accessTokenExpiresAt)
        {
            string accessToken = null;
            accessTokenExpiresAt = DateTime.MinValue;
            try
            {
                List<SqlParameter> parList = new List<SqlParameter>()
                {
                    new SqlParameter("@siteCode", SITE_CODE),
                    new SqlParameter("@userId", DBNull.Value),
                };
                DataTable dataTable = GetDataTable("EXEC [dbo].[GetTokens] @siteCode, @userId", parList);
                if (dataTable.Rows.Count > 0)
                {
                    accessToken = Encoding.UTF8.GetString(
                        EncryptionV2.Decrypt(Convert.FromBase64String(dataTable.Rows[0]["accessToken"].ToString()), string.Format("{0}@{1}", typeof(DBNull).Name, SITE_CODE)));
                    accessTokenExpiresAt = DateTime.SpecifyKind(Convert.ToDateTime(dataTable.Rows[0]["accessTokenExpiresAt"]), DateTimeKind.Utc);
                }
            }
            catch { }
            return accessToken;
        }

        void ITokenStorageProvider.StoreToken(string accessToken, DateTime accessTokenExpiresAt)
        {
            try
            {
                List<SqlParameter> parList = new List<SqlParameter>()
                {
                    new SqlParameter("@siteCode", SITE_CODE),
                    new SqlParameter("@userId", DBNull.Value),
                    new SqlParameter("@accessToken",
                        Convert.ToBase64String(EncryptionV2.Encrypt(Encoding.UTF8.GetBytes(accessToken), string.Format("{0}@{1}", typeof(DBNull).Name, SITE_CODE)))),
                    new SqlParameter("@accessTokenExpiresAt", accessTokenExpiresAt)
                };
                ExecuteSQL("EXEC [dbo].[SetTokens] @siteCode, @userId, @accessToken, @accessTokenExpiresAt", parList);
            }
            catch { }
        }

        #endregion
    }
}
