using System;
using System.Diagnostics;

namespace FWBS.OMS.OMSEXPORT
{
    public class OMSExportENT : OMSExportBase, IDisposable
    {

        #region Fields
        /// <summary>
        /// Convert to Local Time flag
        /// </summary>
        private bool _convertToLocalTime = false;
        /// <summary>
        /// Used when calling registry read function 
        /// </summary>
        private const string APPNAME = "ENT";
        /// <summary>
        /// Monitor if logged in to ENT
        /// </summary>
        private bool _loggedIn = false;

        private enterpriseTestWebService.TestWebService _entTestWebService;
        private enterpriseClientLoadService.ClientLoadService _entClientLoadService;
        private enterpriseClientService.ClientService _entClientService;
        private enterpriseMatterLoadService.MatterLoadService _entMatterLoadService;
        private enterpriseMatterService.MatterService _entMatterService;
        private enterpriseClientLoadService.user _clientUser;
        private enterpriseMatterLoadService.user _matterUser;
        private enterpriseTimeCard.TimeCard _entTimeCard;
        private enterpriseTimeCard.user _timeCardUser;
        
        #endregion Fields


        /// <summary>
        /// Logs into Enterprise and sets cookie containers on all Enterprise objects
        /// </summary>
        private void LoginToENT()
        {
            RaiseStatusEvent("Logging into Enterprise system.");
            string baseURL = "??";

            try
            {
                _entTestWebService = new enterpriseTestWebService.TestWebService();
                _entClientLoadService = new enterpriseClientLoadService.ClientLoadService();
                _entClientService = new enterpriseClientService.ClientService();
                _entMatterLoadService = new enterpriseMatterLoadService.MatterLoadService();
                _entMatterService = new enterpriseMatterService.MatterService();
                _clientUser = new enterpriseClientLoadService.user();
                _matterUser = new enterpriseMatterLoadService.user();
                _entTimeCard = new enterpriseTimeCard.TimeCard();
                _timeCardUser = new enterpriseTimeCard.user();
                
                RaiseStatusEvent("Instantiated Enterprise services ");

                baseURL = StaticLibrary.GetSetting("BaseURL", APPNAME, "");

                //if it is set up with a trailing backslash then strip off
                if (baseURL.LastIndexOf(@"/") == baseURL.Length - 1)
                    baseURL = baseURL.Remove(baseURL.Length - 1, 1);

                //set the urls's of all the web services
                _entClientLoadService.Url = baseURL + @"/ClientLoadService.asmx";
                _entClientService.Url = baseURL + @"/ClientService.asmx";
                _entMatterLoadService.Url = baseURL + @"/MatterLoadService.asmx";
                _entMatterService.Url = baseURL + @"/MatterService.asmx";
                _entTimeCard.Url = baseURL + @"/TimeCard.asmx";
                _entTestWebService.Url = baseURL + @"/testwebservice.asmx";

                Debug.WriteLine("testing auth service", "OMSEXPORT");

                //Test to see if we are logged in properly
                string test = _entTestWebService.TestDatabaseConnection();
                string expectedTestResponse = "Test Successful";

                if (test.ToUpper() != expectedTestResponse.ToUpper())
                {        //not logged in properly
                        throw new Exception(string.Format("Test not successful.  Please check settings [TestResponse : {0}]", test));
                }

                RaiseStatusEvent("Logged into Enterprise system.");
                _loggedIn = true;
            }
            catch (Exception ex)
            {
                base.LogEntry(ex.Message + Environment.NewLine + ex.StackTrace, System.Diagnostics.EventLogEntryType.Error);

                throw new ApplicationException("Error Logging in to Enterprise : " + baseURL + ex.Message, ex);

            }
        }


        protected override void InitialiseProcess()
        {
            //set the convert to local time flag
            _convertToLocalTime = StaticLibrary.GetBoolSetting("ConvertToLocalTime", "ENT", false);
        }

        protected override void FinaliseProcess()
        {
            try
            {
                //Log out of Enterprise
                RaiseStatusEvent("Logging out of Enterprise");
                LogoutOfEnterprise();
            }
            catch { }
        }

        void LogoutOfEnterprise()
        {
            try
			{
				
				if(_loggedIn)
				{
				    //null all web service variables
				    _entClientLoadService = null;
				    _entClientService= null;
				    _entMatterLoadService = null;
				    _entMatterService= null;
				    _entTestWebService= null;
                    _entTimeCard = null;

                }			
				//finally set flag
				_loggedIn = false;
			}
			catch(Exception ex)
			{
				base.LogEntry("LogoutOfEnterprise: " + ex.Message,System.Diagnostics.EventLogEntryType.Error);
			}
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

        protected override object ExportClient(System.Data.DataRow row)
        {
            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToENT();

            try
            {
                //Collect data from columns
                long clID = Convert.ToInt64(row["CLID"]);

                string[] UserIDs = new string[1];
                UserIDs[0] = Convert.ToString(StaticLibrary.GetSetting("ServiceUserID", APPNAME, ""));
                _clientUser.Text = UserIDs;

                enterpriseClientLoadService.ClientLoad[] clientArray = new enterpriseClientLoadService.ClientLoad[1];

                clientArray[0] = new enterpriseClientLoadService.ClientLoad();

                //Now set the properties
                //Loop through all properties for clientArray[0] and set value if a column exists in the data table
                foreach (var prop in clientArray[0].GetType().GetProperties())
                {
                    //if the column name exists as a property, and the type is string (all but udf at time of coding)
                    if (row.Table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value && prop.PropertyType == typeof(string))
                        prop.SetValue(clientArray[0], row[prop.Name], null);
                }
                //TODO - set other properties -what about null values?  does model take over?

                //Get UDF information
                //All columns must begin UDF_ then the name of column in Enterprise
                int udfCount = 0;
                foreach (System.Data.DataColumn col in row.Table.Columns)
                {
                    if(col.ColumnName.ToUpper().StartsWith("UDF_"))
                        udfCount ++;
                }

                if (udfCount > 0)
                {
                    enterpriseClientLoadService.Udf[] clientUdf = new enterpriseClientLoadService.Udf[udfCount];

                    for (int i = 0; i < clientUdf.Length; i++)
                    {
                        clientUdf[i] = new enterpriseClientLoadService.Udf();
                    }

                    int thisUdfIndex = 0;
                    foreach (System.Data.DataColumn col in row.Table.Columns)
                    {
                        if (col.ColumnName.ToUpper().StartsWith("UDF_"))
                        {
                            clientUdf[thisUdfIndex].label = col.ColumnName.Substring(4);
                            clientUdf[thisUdfIndex].val = Convert.ToString(row[col]);
                            thisUdfIndex++;
                        }
                    }

                    clientArray[0].udf = clientUdf;
                }

                _entClientLoadService.userValue = _clientUser;
                string returnValue = _entClientLoadService.Create(clientArray);

                WriteFile(returnValue, "ClientExport",clID);

                string clNum = ExtractLoadResult(returnValue,LoadType.Client);
                if (clNum != "")
                {
                    //Client has been created
                    //but if errored as well (eg lookup incorrect and "MODEL" data assumed by Enterprise)
                    if (returnValue.IndexOf("Error") > 0)
                    {
                        string sql = GetExportMappingSQL("Enterprise",ExportMappingEntityType.Client,clID.ToString(),clNum);
                        ExecuteSQL(sql);
                        throw new Exception("Client created, but validation errors occured : " + Environment.NewLine + Environment.NewLine + returnValue);
                    }
                    else
                    {
                        //Client created with no errors in returned string
                        return clNum;
                    }
                }
                else
                {
                    //how did we get in this state?
                    //no client number can cause this - anything else?
                    throw new Exception(returnValue);
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
            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToENT();

            try
            {
                //Collect data from columns
                long clID = Convert.ToInt64(row["CLID"]);

                string[] UserIDs = new string[1];
                UserIDs[0] = Convert.ToString(StaticLibrary.GetSetting("ServiceUserID", APPNAME, ""));
                _clientUser.Text = UserIDs;

                enterpriseClientLoadService.ClientLoad[] clientArray = new enterpriseClientLoadService.ClientLoad[1];

                clientArray[0] = new enterpriseClientLoadService.ClientLoad();

                //Now set the properties
                //Loop through all properties for clientArray[0] and set value if a column exists in the data table
                foreach (var prop in clientArray[0].GetType().GetProperties())
                {
                    //if the column name exists as a property, and the type is string (all but udf at time of coding)
                    if (row.Table.Columns.Contains(prop.Name) && row[prop.Name]!=DBNull.Value && prop.PropertyType == typeof(string))
                        prop.SetValue(clientArray[0],row[prop.Name],null);
                }
                //TODO - set other properties -what about null values?  does model take over?  Question is with DP.

                //Get UDF information
                //All columns must begin UDF_ then the name of column in Enterprise
                int udfCount = 0;
                foreach (System.Data.DataColumn col in row.Table.Columns)
                {
                    if (col.ColumnName.ToUpper().StartsWith("UDF_"))
                        udfCount++;
                }

                
                if (udfCount > 0)
                {
                    enterpriseClientLoadService.Udf[] clientUdf = new enterpriseClientLoadService.Udf[udfCount];

                    for (int i = 0; i < clientUdf.Length; i++)
                    {
                        clientUdf[i] = new enterpriseClientLoadService.Udf();
                    }

                    int thisUdfIndex = 0;
                    foreach (System.Data.DataColumn col in row.Table.Columns)
                    {
                        if (col.ColumnName.ToUpper().StartsWith("UDF_"))
                        {
                            clientUdf[thisUdfIndex].label = col.ColumnName.Substring(4);
                            clientUdf[thisUdfIndex].val = Convert.ToString(row[col]);
                            thisUdfIndex++;
                        }
                    }

                    clientArray[0].udf = clientUdf;
                }

                _entClientLoadService.userValue = _clientUser;
                string returnValue = _entClientLoadService.Create(clientArray);

                WriteFile(returnValue, "ClientUpdate", clID);

                //TODO - evaluate this with Elite - for example, clientname could be updated, but another validation has failed... is there an option to rollback all?
                if (returnValue.IndexOf("Error") > 0)
                    throw new Exception(returnValue);
                else
                    return true;

            }
            catch (Exception ex)
            {
                string s = ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        protected override object ExportContact(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdateContact(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override object ExportMatter(System.Data.DataRow row)
        {
            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToENT();

            try
            {
                //Collect data from columns
                long fileID = Convert.ToInt64(row["FILEID"]);

                string[] UserIDs = new string[1];
                UserIDs[0] = Convert.ToString(StaticLibrary.GetSetting("ServiceUserID", APPNAME, ""));
                _matterUser.Text = UserIDs;

                enterpriseMatterLoadService.MatterLoad[] matterArray = new enterpriseMatterLoadService.MatterLoad[1];

                matterArray[0] = new enterpriseMatterLoadService.MatterLoad();

                //Now set the properties
                //Loop through all properties for matterArray[0] and set value if a column exists in the data table
                foreach (var prop in matterArray[0].GetType().GetProperties())
                {
                    //if the column name exists as a property, and the type is string (all but udf at time of coding)
                    if (row.Table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value && prop.PropertyType == typeof(string))
                        prop.SetValue(matterArray[0], row[prop.Name], null);
                }
                //TODO - set other properties -what about null values?  does model take over?

                //Get UDF information
                //All columns must begin UDF_ then the name of column in Enterprise
                int udfCount = 0;
                foreach (System.Data.DataColumn col in row.Table.Columns)
                {
                    if (col.ColumnName.ToUpper().StartsWith("UDF_"))
                        udfCount++;
                }

                if (udfCount > 0)
                {
                    enterpriseMatterLoadService.Udf[] matterUdf = new enterpriseMatterLoadService.Udf[udfCount];

                    for (int i = 0; i < matterUdf.Length; i++)
                    {
                        matterUdf[i] = new enterpriseMatterLoadService.Udf();
                    }

                    int thisUdfIndex = 0;
                    foreach (System.Data.DataColumn col in row.Table.Columns)
                    {
                        if (col.ColumnName.ToUpper().StartsWith("UDF_"))
                        {
                            matterUdf[thisUdfIndex].label = col.ColumnName.Substring(4);
                            matterUdf[thisUdfIndex].val = Convert.ToString(row[col]);
                            thisUdfIndex++;
                        }
                    }

                    matterArray[0].udf = matterUdf;
                }

                _entMatterLoadService.userValue = _matterUser;
                //TODO
                string returnValue = _entMatterLoadService.Create(matterArray,System.DateTime.Now.Date.ToString("s"));

                WriteFile(returnValue, "MatterExport",fileID);

                string matNum = ExtractLoadResult(returnValue, LoadType.Matter);
                if (matNum != "")
                {
                    //Matter has been created
                    //but if errored as well (eg lookup incorrect and "MODEL" data assumed by Enterprise)
                    if (returnValue.IndexOf("Error") > 0)
                    {
                        string sql = GetExportMappingSQL("Enterprise", ExportMappingEntityType.File, fileID.ToString(), matNum);
                        ExecuteSQL(sql); throw new Exception("File created, but validation errors occured : " + Environment.NewLine + Environment.NewLine + returnValue);
                    }
                    else
                    {
                        //Client created with no errors in returned string
                        return matNum;
                    }
                }
                else
                {
                    //how did we get in this state?
                    //no client number can cause this - anything else?
                    throw new Exception(returnValue);
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
            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToENT();

            try
            {
                //Collect data from columns
                long fileID = Convert.ToInt64(row["FILEID"]);

                string[] UserIDs = new string[1];
                UserIDs[0] = Convert.ToString(StaticLibrary.GetSetting("ServiceUserID", APPNAME, ""));
                _matterUser.Text = UserIDs;

                enterpriseMatterLoadService.MatterLoad[] matterArray = new enterpriseMatterLoadService.MatterLoad[1];

                matterArray[0] = new enterpriseMatterLoadService.MatterLoad();

                //Now set the properties
                //Loop through all properties for clientArray[0] and set value if a column exists in the data table
                foreach (var prop in matterArray[0].GetType().GetProperties())
                {
                    //if the column name exists as a property, and the type is string (all but udf at time of coding)
                    if (row.Table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value && prop.PropertyType == typeof(string))
                        prop.SetValue(matterArray[0], row[prop.Name], null);
                }
                //TODO - set other properties -what about null values?  does model take over?  Question is with DP.

                //Get UDF information
                //All columns must begin UDF_ then the name of column in Enterprise
                int udfCount = 0;
                foreach (System.Data.DataColumn col in row.Table.Columns)
                {
                    if (col.ColumnName.ToUpper().StartsWith("UDF_"))
                        udfCount++;
                }


                if (udfCount > 0)
                {
                    enterpriseMatterLoadService.Udf[] matterUdf = new enterpriseMatterLoadService.Udf[udfCount];

                    for (int i = 0; i < matterUdf.Length; i++)
                    {
                        matterUdf[i] = new enterpriseMatterLoadService.Udf();
                    }
                    
                    int thisUdfIndex = 0;
                    foreach (System.Data.DataColumn col in row.Table.Columns)
                    {
                        if (col.ColumnName.ToUpper().StartsWith("UDF_"))
                        {
                            matterUdf[thisUdfIndex].label = col.ColumnName.Substring(4);
                            matterUdf[thisUdfIndex].val = Convert.ToString(row[col]);
                            thisUdfIndex++;
                        }
                    }

                    matterArray[0].udf = matterUdf;
                }

                _entMatterLoadService.userValue = _matterUser;
                string returnValue = _entMatterLoadService.Create(matterArray, System.DateTime.Now.Date.ToString("s"));

                WriteFile(returnValue, "MatterUpdate",fileID);

                //TODO - evaluate this with Elite
                if (returnValue.IndexOf("Error") > 0)
                    throw new Exception(returnValue);
                else
                    return true;

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
            //check if logged in and then login if not
            if (!_loggedIn)
                LoginToENT();

            try
            {
                long timeID = Convert.ToInt64(row["ID"]);

                string[] UserIDs = new string[1];
                UserIDs[0] = Convert.ToString(StaticLibrary.GetSetting("ServiceUserID", APPNAME, ""));
                string dateFormat = Convert.ToString(StaticLibrary.GetSetting("DateFormat", APPNAME, ""));
                _timeCardUser.Text = UserIDs;

                enterpriseTimeCard.TimeLoadFormat1[] timeArray = new enterpriseTimeCard.TimeLoadFormat1[1];
                timeArray[0] = new enterpriseTimeCard.TimeLoadFormat1();

                //Now set the properties
                //Loop through all properties for clientArray[0] and set value if a column exists in the data table
                foreach (var prop in timeArray[0].GetType().GetProperties())
                {
                    //if the column name exists as a property, and the type is string (all but udf at time of coding)
                    if (row.Table.Columns.Contains(prop.Name) && row[prop.Name] != DBNull.Value && prop.PropertyType == typeof(string))

                        switch (prop.Name.ToUpper())
                        {
                            case "DATE":
                                prop.SetValue(timeArray[0], Convert.ToDateTime(row[prop.Name]).ToString(dateFormat), null);
                                break;
                            case "AMOUNT":
                                prop.SetValue(timeArray[0], Convert.ToString((row[prop.Name])), null);
                                break;
                            case "HOUR":
                                prop.SetValue(timeArray[0], Convert.ToString((row[prop.Name])), null);
                                break;
                            default:
                                prop.SetValue(timeArray[0], row[prop.Name], null);
                                break;
                        }
                }

                _entTimeCard.userValue = _timeCardUser;

                enterpriseTimeCard.TimeLoadResult returnValue = _entTimeCard.Create1(timeArray[0], UserIDs[0], "", "N");
                WriteFile(returnValue.report, "TIME", timeID);    
                
                //TODO - evaluate this with Elite
                if (!String.IsNullOrEmpty(returnValue.error))
                    throw new Exception(returnValue.error);
                if (string.IsNullOrEmpty(returnValue.timecard_tindex))
                    throw new Exception("No Error was returned, however no Time Index has been returned.  This could be the date format being passed to the web service");
                else
                    return returnValue.timecard_tindex;
       
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
            throw new NotImplementedException();
        }

        protected override string ExportObject
        {
            get
            {
                return APPNAME;
            }
        }

        private enum LoadType { Client, Matter }

        private string ExtractLoadResult(string resultString, LoadType loadType)
        {
            string CheckForStart = "";
            switch (loadType)
            {
                case LoadType.Client:
                    CheckForStart = "\n\nInserting client ";
                    break;
                case LoadType.Matter:
                    CheckForStart = "\n\nInserting matter ";
                    break;
            }

            string CheckForEnd = "\n\nNumber of lines read:";

            if (resultString.IndexOf(CheckForStart) < 0)
                throw new Exception(string.Format("Cannot find start string '{0}'", CheckForStart));
            if (resultString.IndexOf(CheckForEnd) < 0)
                throw new Exception(string.Format("Cannot find end string '{0}'", CheckForEnd));

            string temp = resultString.Substring(resultString.IndexOf(CheckForStart) + CheckForStart.Length);
            temp = temp.Substring(0, temp.IndexOf(CheckForEnd));
            temp = temp.Trim();
            return temp;
        }

        void WriteFile(string text, string type, long ID)
        {
            bool _debug = true;
            string location = @"c:\temp\debug\Enterprise";
            if (_debug)
            {
                if (!location.EndsWith(@"\"))
                    location += @"\";


                string path = location;
                string ticks = System.DateTime.Now.Ticks.ToString();

                System.IO.StreamWriter swSent = new System.IO.StreamWriter(path + type + "-" + ID.ToString() + "-Response-" + ticks + ".txt");
                swSent.Write(text);
                swSent.Flush();
                swSent.Close();
            }
        }
    }
}
