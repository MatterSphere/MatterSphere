using System;
using System.Data;
using System.Data.SqlClient;
using Miles33.Precedent.TimeRecordingBL;

namespace FWBS.OMS.OMSEXPORT
{
    /// <summary>
    /// A Class to export to Miles33
    /// </summary>
    public class OMSExportMIL : OMSExportBase, IDisposable
    {
        //  Constructor calls base contstructor
        public OMSExportMIL()
            : base()
        {
        }

        #region Fields

        //  Get the Indigo values from the registry
        string companyNo = StaticLibrary.GetSetting("CompanyNo", APPNAME, "");
        string branchNo = StaticLibrary.GetSetting("BranchNo", APPNAME, "");
        string computer = StaticLibrary.GetSetting("Computer", APPNAME, "");
        string userCode = StaticLibrary.GetSetting("UserCode", APPNAME, "");
        string programName = StaticLibrary.GetSetting("ProgramName", APPNAME, "");
        string version = StaticLibrary.GetSetting("Version", APPNAME, "");
        string message = StaticLibrary.GetSetting("Message", APPNAME, "");
        private SqlConnection _MILcnn;

        /// <summary>
        /// Used when calling registry read function 
        /// </summary>
        private const string APPNAME = "MIL";

        /// <summary>
        /// Flag to indicate if the time values should be converted to local time.
        /// </summary>
        private bool _convertToLocalTime = false;

        /// <summary>
        /// Monitor if we are logged into CMS
        /// </summary>
        private bool _loggedIn = false;
        private bool _Miles33TimeloggedIn = false;
       
        #endregion

        /// <summary>
        /// Get Program Log ID
        /// </summary>
        /// <returns></returns>
        public int GetProgramLogID()
        {
            try
            {
                return 0;
            }
            catch (Exception ex)
            {
                string s = "InsertLogRecord: " + ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        /// <summary>
        /// Opens a connection to the Miles 33 Database
        /// </summary>
        private void OpenIGOConnection()
        {
            // Open a connection to the OMS Database and check for the table fdIndigoPeriods
            RaiseStatusEvent("Opening MIL Database connection.");
            _MILcnn = new SqlConnection(StaticLibrary.MILConnectionString());
            _MILcnn.Open();
            RaiseStatusEvent("Database connection open.");
        }

        #region OMSExportBase Members

        /// <summary>
        /// Initialise Process
        /// </summary>
        protected override void InitialiseProcess()
        {
            try
            {
                //  Set the convert to local time flag
                _convertToLocalTime = StaticLibrary.GetBoolSetting("ConvertToLocalTime", "MIL", false);
            }
            catch (Exception ex)
            {
                string s = "Initialisation: " + ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        /// <summary>
        /// Finalise Process
        /// </summary>
        protected override void FinaliseProcess()
        {
            try
            {
                //  Log out of MIL
                RaiseStatusEvent("Logging out of MIL");
            }
            catch (Exception ex)
            {
                string s = "Update Log Record: " + ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }

        /// <summary>
        /// Export Lookup for MIL
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override string ExportLookup(DataRow row)
        {
            return "";
        }

        /// <summary>
        /// Exports an individual client record
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override object ExportClient(System.Data.DataRow row)
        {
            throw new ApplicationException("Clients not supported in this release.");
        }
        protected override object ExportContact(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdateContact(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Export Financial Record
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override object ExportFinancialRecord(DataRow row)
        {
            throw new ApplicationException("Financials not supported in this release.");
        }

        /// <summary>
        /// Export Fee Earner
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override int ExportFeeEarner(DataRow row)
        {
            throw new ApplicationException("Fee Earners not supported in this release.");
        }

        /// <summary>
        /// Export Time Record
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override object ExportTimeRecord(DataRow row)
        {
            //    Check if logged in?
            if (!_Miles33TimeloggedIn)
            {
                //  If not, login
                LoginToMiles33TimeRecording();
            }

            try
            {
                if (_Miles33TimeloggedIn)
                {
                    RaiseStatusEvent("Logged in to Miles 33 processing time record");
                    TimeStaticData.Refresh();
                    TimeClient timeClient = new TimeClient(Convert.ToString(row["FeID"]), Convert.ToString(row["DueDate"]));
                    RaiseStatusEvent("Refreshing Fee Earner");
                    timeClient.RefreshFeeEarner();
                    RaiseStatusEvent("Loading timesheet");
                    timeClient.TimeSheet.LoadTimesheet();
                    WorkingMatter wm = timeClient.WorkingMatters.LoadMatter(Convert.ToString(row["MatterID"]));
                    TimeLine timeLine = timeClient.TimeSheet.NewTimeLine(true);

                    RaiseStatusEvent("Populating timeline");
                    timeLine.PopulateFromWorkingMatter(ref wm);
                    timeLine.ActivityType = Convert.ToString(row["ActivityType"]);
                    timeLine.ItemDescription = Convert.ToString(row["ItemDescription"]);
                    
                    string lit = Convert.ToString(row["LitigationStageID"]);
                    if (!string.IsNullOrEmpty(lit))
                        timeLine.LitigationStageID = lit;
                    /*
                     * 09/02/2011
                     * Hi Dave, 
                       It looks like the Time API is defaulting the bill event in scenarios where the schedule is defined with just a single bill event.
                       A workaround would be to change the code:
                    * */
                    string eventID = Convert.ToString(row["EventID"]);
                    if (!string.IsNullOrEmpty(eventID))
                        timeLine.EventID = eventID;   // may not be passed if not a Billing Event ID
                    else
                        timeLine.EventID = "";

                    string la = Convert.ToString(row["LAStructure"]);
                    if(! string.IsNullOrEmpty(la))
                        timeLine.LAStructure = la;

                    RaiseStatusEvent("Updating Time Quantity");
                    timeLine.UpdateTimeQty("", Convert.ToString(row["Units"]));
                    timeLine.UpdateTimeValue();
                    timeLine.Validate();
                    timeClient.TimeSheet.SaveData(true, false);

                    string timeline = Convert.ToString(timeLine.UniqueID);

                    return timeline;
                }
                else
                {
                    return "Failed Export";
                }
            }
            catch (Exception ex)
            {
                string s = "ExportTimeRecord: " + ex.Message;
                Exception enew = new Exception(s);
                throw enew;
            }
        }


        /// <summary>
        /// Export Matter
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override object ExportMatter(DataRow row)
        {
            
            throw new ApplicationException("Matters not supported in this release.");
        }



        /// <summary>
        /// Update Matter
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override bool UpdateMatter(DataRow row)
        {
            throw new ApplicationException("Matters not supported in this release.");
        }



        /// <summary>
        /// Update Client
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override bool UpdateClient(DataRow row)
        {
            throw new ApplicationException("Clients not supported in this release.");
        }

        /// <summary>
        /// Export User
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        protected override int ExportUser(DataRow row)
        {
            throw new ApplicationException("Users not supported in this release.");
            //return 0;
        }

        /// <summary>
        /// Export Object
        /// </summary>
        protected override string ExportObject
        {
            get
            {
                return APPNAME;
            }
        }



        #endregion OMSExportBase Members

        /// <summary>
        /// Login to PFW
        /// </summary>
        private void LoginToMiles33()
        {
            try
            {
                RaiseStatusEvent("Setting Miles 33 Connection properties");
                //  Create and Setup login details
                MilesDataStore.LoginManager.ThreadLoginManager().LoginName = StaticLibrary.GetSetting("MILLogin", APPNAME, "");
                MilesDataStore.LoginManager.ThreadLoginManager().Password = StaticLibrary.GetSetting("MILPassword", APPNAME, "");
                MilesDataStore.LoginManager.ThreadLoginManager().Server = StaticLibrary.GetSetting("MILServer", APPNAME, "");
                
                //  Get Oracle client connection version
                string oracleType = StaticLibrary.GetSetting("OrcaleConnectionType", APPNAME, "");

                if (oracleType.Equals("Oracle 10 OLEDB"))
                {
                    //  Oracle client version 10
                    MilesDataStore.LoginManager.ThreadLoginManager().ConnectionType = MilesDataStore.LoginManager.OracleConnectionType.Oracle10OLEDB;
                }
                else
                {
                    //  Else default to Oracle client version 7
                    MilesDataStore.LoginManager.ThreadLoginManager().ConnectionType = MilesDataStore.LoginManager.OracleConnectionType.Oracle7OLEDB;
                }

                //  Use Test Connection for now
                RaiseStatusEvent("Logging into Miles 33....");
                _loggedIn = MilesDataStore.LoginManager.ThreadLoginManager().TestConnection();

                _loggedIn = true;

                RaiseStatusEvent("Logged into Miles 33 system.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Logging in to Miles 33: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Login To Miles33 Time Recording
        /// </summary>
        private void LoginToMiles33TimeRecording()
        {
            //  This Login is for the Time Record functions
            try
            {
                RaiseStatusEvent("Setting Miles 33 time export connection settings"); 
                Miles33.Precedent.PrecDAL.LoginManager.Oracle10g = true;
                Miles33.Precedent.PrecDAL.LoginManager.ThreadLoginManager().LoginName = StaticLibrary.GetSetting("MILLogin", APPNAME, "");
                Miles33.Precedent.PrecDAL.LoginManager.ThreadLoginManager().Password = StaticLibrary.GetSetting("MILPassword", APPNAME, "");
                Miles33.Precedent.PrecDAL.LoginManager.ThreadLoginManager().Server = StaticLibrary.GetSetting("MILServer", APPNAME, "");

                _Miles33TimeloggedIn = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Setting Miles 33 Time Recording connection settings: " + ex.Message, ex);
            }
        }
    }
}
