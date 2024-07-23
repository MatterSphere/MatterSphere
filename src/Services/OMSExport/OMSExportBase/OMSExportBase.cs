using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace FWBS.OMS.OMSEXPORT
{
	/// <summary>
	/// This is the base abstract class that all export objects must inherit.
	/// </summary>
	public abstract class OMSExportBase: IDatabaseProvider, IDisposable
	{
		#region Fields
		/// <summary>
		/// Registry constants
		/// </summary>
		internal const string REG_APPLICATION_KEY = "OMS";
		internal const string REG_VERSION_KEY = "2.0";
        /// <summary>
        /// Data Base connection variable
        /// </summary>
        SqlConnection _cnn = null;
		/// <summary>
		/// flag to pause application
		/// </summary>
		internal bool _pause = false;
		/// <summary>
		/// flag to stop application
		/// </summary>
		internal bool _stop = false;
		/// <summary>
		/// Flag to indicate service has stopped
		/// </summary>
		protected internal bool _stopped = false;
		/// <summary>
		/// Is full logging enabled
		/// </summary>
		private bool _fullLog;
		/// <summary>
		/// Variables for stats
		/// </summary>
		protected LogCounter _logCounter;
		/// <summary>
		/// Timer to write stats periodically
		/// </summary>
		private System.Timers.Timer _statTimer;
		/// <summary>
		/// How many times each export will fail before moving to the next
		/// </summary>
		private int _exceptionThreashold;
        /// <summary>
        /// Counts up errors so can email admin if greater than 0
        /// </summary>
        private int _errorCounter = 0;
        /// <summary>
        /// Flag to indicate errors arewritten to a new database table
        /// </summary>
        private bool _logToDB = false;

        ///Added thises fields while upgrading to version 2.0 
        ///Allows switching of of individual export items
        private bool _bUser = true;
        private bool _bFeeEarner = true;
        private bool _bExportClient = true;
        private bool _bUpdateClient = true;
        private bool _bExportContact = true;
        private bool _bUpdateContact = true;
        private bool _bExportMatter = true;
        private bool _bUpdateMatter = true;
        private bool _bTime = true;
        private bool _bFinance = true;
        private bool _bLookups = true;
        //fields for custom updates
        private bool _bCustomUpdateTime;
        private string _sCustomUpdateTime;
        private bool _bCustomUpdateFinancials;
        private string _sCustomUpdateFinancials;
        private bool _bCustomUpdateExClients;
        private string _sCustomUpdateExClients;
        private bool _bCustomUpdateUpClients;
        private string _sCustomUpdateUpClients;
        private bool _bCustomUpdateExMatters;
        private string _sCustomUpdateExMatters;
        private bool _bCustomUpdateUpMatters;
        private string _sCustomUpdateUpMatters;
        private string _sLogDays;
        private string _sTrackingColumnType;
        private bool _bCustomUpdateExContacts;
        private string _sCustomUpdateExContacts;
        private bool _bCustomUpdateUpContacts;
        private string _sCustomUpdateUpContacts;

        /// <summary>
        /// debug option
        /// </summary>
        protected bool _debugMode = false;
        protected virtual string dataFormat { get; }

        public const string ApiRelativeUrlTransService2xDefault = "webui/TransactionService.asmx";
        public const string ApiRelativeUrlTransService3xDefault = "web/TransactionService.asmx";
        public const string ApiRelativeUrlOnPremDefault = "web/api/";
        public const string ApiRelativeUrlCloudDefault = "3e/integration/";

        public const string ApiRelativeUrlTransService2xKey = "ApiRelativeUrlTransService2";
        public const string ApiRelativeUrlTransService3xKey = "ApiRelativeUrlTransService3";
        public const string ApiRelativeUrlOnPremKey = "ApiRelativeUrlOnPrem";
        public const string ApiRelativeUrlCloudKey = "ApiRelativeUrlCloud";

        #endregion

        #region EnterpriseSQL

        public enum ExportMappingEntityType { Client, File }
        public string GetExportMappingSQL(string system, ExportMappingEntityType entity, string internalID, string externalID)
        {

            string templateSQL = @"
--SET NOCOUNT ON TO ENSURE ONLY RETURNS ONE ROW FOR UPDATE
SET NOCOUNT ON

DECLARE @SYSTEMID UNIQUEIDENTIFIER
DECLARE @ENTITYID UNIQUEIDENTIFIER
DECLARE @ERRORMESSAGE NVARCHAR ( 100 ) 

DECLARE @INTEGRATIONSYSTEMNAME NVARCHAR ( 50 )
DECLARE @INTEGRATIONENTITYNAME NVARCHAR ( 50 )

SET @INTEGRATIONSYSTEMNAME = '{0}' 
SET @INTEGRATIONENTITYNAME = '{1}'

IF ( SELECT COUNT ( * ) FROM FDDBINTEGRATIONSYSTEM WHERE NAME = @INTEGRATIONSYSTEMNAME ) = 0
BEGIN 
	SET @ERRORMESSAGE = 'Integration System not set up for ' + @INTEGRATIONSYSTEMNAME 
	RAISERROR ( @ERRORMESSAGE , 15 , 1 )
	RETURN
END 

IF ( SELECT COUNT ( * ) FROM FDDBINTEGRATIONENTITY WHERE NAME = @INTEGRATIONENTITYNAME ) = 0
BEGIN 
	SET @ERRORMESSAGE =  'Integration Entity not set up for ' + @INTEGRATIONENTITYNAME
	RAISERROR ( @ERRORMESSAGE , 15 , 1 )
	RETURN
END 

SELECT @SYSTEMID = ID FROM FDDBINTEGRATIONSYSTEM WHERE NAME = @INTEGRATIONSYSTEMNAME 
SELECT @ENTITYID = ID FROM FDDBINTEGRATIONENTITY WHERE NAME = @INTEGRATIONENTITYNAME

IF ( SELECT COUNT ( * ) FROM FDDBINTEGRATIONMAPPING WHERE ENTITYID = @ENTITYID AND SYSTEMID = @SYSTEMID AND INTERNALID = '{2}' ) > 0
BEGIN
	SET @ERRORMESSAGE =  'Integration Mapping table already populated for Internal ID {2}'
	RAISERROR ( @ERRORMESSAGE , 15 , 1 )
	RETURN
END 


--BUILD SQL TO CREATE ROW
INSERT INTO	FDDBINTEGRATIONMAPPING
(
	SYSTEMID
	, ENTITYID
	, INTERNALID
	, EXTERNALID
)

SELECT 
	@SYSTEMID
	, @ENTITYID
	, '{2}'
	, '{3}'

--SET NOCOUNT OFF TO ENSURE ONLY RETURNS ONE ROW FOR UPDATE
SET NOCOUNT OFF
";

            switch (entity)
            {
                case ExportMappingEntityType.Client:
                    templateSQL += "UPDATE DBCLIENT SET CLNEEDEXPORT = 0 WHERE CLID = {2};";
                    break;
                case ExportMappingEntityType.File:
                    templateSQL += "UPDATE DBFILE SET FILENEEDEXPORT = 0 WHERE FILEID = {2};";
                    break;
            }

            return (string.Format(templateSQL,system,entity.ToString(),internalID,externalID));

        }
        #endregion

        #region Events

        //event for status messages
		public delegate void StatusEventHandler(object sender, StatusEventArgs e);
		public event StatusEventHandler OnStatusChange;

		#endregion

		#region Constructor
		
		public OMSExportBase()
            : this (new SqlConnection(StaticLibrary.OMSConnectionString()))
		{			
		}

	    public OMSExportBase(SqlConnection connection)           
	    {
            //sets flag to indicate if admin email is switched on
            try
            {
                //set up stats timer but do not make it active yet
                _statTimer = new System.Timers.Timer(10000);
                _statTimer.Elapsed += new System.Timers.ElapsedEventHandler(_statTimer_Elapsed);

                //get a new logcounter with intial values read from any existing log file
                _logCounter = new LogCounter();

                // finally start the timer that writes the stats down
                _statTimer.Start();
            }
            catch { }
            _cnn = connection;
	    }


        #endregion

        #region Properties

        /// <summary>
        /// Used to query at the end of a RunProcess to see if Admin needs to be emailed
        /// </summary>
        public int ErrorCount
        {
            get
            {
                return _errorCounter;
            }
        }
		
		/// <summary>
		/// Boolean flag to indicate service has stopped
		/// </summary>
		public bool Stopped
		{
			get
			{
				return _stopped;
			}
		}


		/// <summary>
		/// Flag to indicate appication has paused
		/// </summary>
		public bool Paused
		{
			get
			{
				return _pause;
			}
		}

		/// <summary>
		/// Default Credentials
		/// </summary>
		public static System.Net.ICredentials DefaultCredentials { get; set; } = System.Net.CredentialCache.DefaultCredentials;

		/// <summary>
		/// Gets number of minutes to pause between iterations
		/// </summary>
		public int PauseInterval
		{
			get
			{
				int val = 1;
				try
				{
                    string valStr = StaticLibrary.GetSetting("PauseInterval", "", "1");
					val = Convert.ToInt32(valStr);
				}
				catch(Exception ex)
				{
					LogEntry("Pause Interval Registry Setting " + ex.Message,EventLogEntryType.Error);
				}
				return val;
			}
		}
		
		/// <summary>
		/// Checks if full event logging is enabled
		/// </summary>
		public bool FullLoggingEnabled
		{
			get
			{
                string val = FWBS.OMS.OMSEXPORT.StaticLibrary.GetSetting("DetailedLogging", "", "False");
				
				if(val.ToUpper() == "FALSE")
					return false;
				else
					return true;
			}
		}

        		
		#endregion

		#region Public methods

			
		/// <summary>
		/// Sets stop variables
		/// </summary>
		public void Stop()
		{
			_stop = true;
			_pause = false;
			_statTimer.Enabled = false;

		}
		
		/// <summary>
		/// sets pause variables
		/// </summary>
		public void Pause()
		{
			_stop = false;
			_pause = true;
			_statTimer.Enabled = false;
		}

		/// <summary>
		/// sets start variables
		/// </summary>
		public void Start()
		{
			_stop = false;
			_stopped = false;
			_pause = false;
			_statTimer.Enabled = true;
		}
		
		/// <summary>
		/// The main export process
		/// </summary>
		public void RunProcess()
		{
			try
			{
				RaiseStatusEvent("Beginning RunProcess Procedure.");
				
				//Get the variables for logging etc
				_fullLog = FullLoggingEnabled;
				
                //added ver 2.0
                _bUser = StaticLibrary.GetBoolSetting("ExportUsers","",false);
                _bFeeEarner = StaticLibrary.GetBoolSetting("ExportFeeEarners", "", false);
                _bExportClient = StaticLibrary.GetBoolSetting("ExportClients","",true);
                _bUpdateClient = StaticLibrary.GetBoolSetting("UpdateClients", "",true);
                _bExportMatter = StaticLibrary.GetBoolSetting("ExportMatters", "",true);
                _bUpdateMatter = StaticLibrary.GetBoolSetting("UpdateMatters", "",true);
                _bTime = StaticLibrary.GetBoolSetting("ExportTime","",true);
                _bFinance = false;
                _bLookups = StaticLibrary.GetBoolSetting("ExportLookups", "", false);
                //### Need bool variables to indicate custom update fields and strings to hold update
                _bCustomUpdateTime = StaticLibrary.GetBoolSetting("CustomUpdateTime","",false);
                _sCustomUpdateTime = StaticLibrary.GetSetting("CustomUpdateTimeScript", "", "");
                _bCustomUpdateFinancials = false;
                _sCustomUpdateFinancials = StaticLibrary.GetSetting("CustomUpdateFinancialsScript", "", "");
                _bCustomUpdateExClients = StaticLibrary.GetBoolSetting("CustomUpdateExClients", "", false);
                _sCustomUpdateExClients = StaticLibrary.GetSetting("CustomUpdateExClientsScript", "", "");
                _bCustomUpdateUpClients = StaticLibrary.GetBoolSetting("CustomUpdateUpClients", "", false);
                _sCustomUpdateUpClients = StaticLibrary.GetSetting("CustomUpdateUpClientsScript", "", "");
                _bCustomUpdateExMatters = StaticLibrary.GetBoolSetting("CustomUpdateExMatters", "", false);
                _sCustomUpdateExMatters = StaticLibrary.GetSetting("CustomUpdateExMattersScript", "", "");
                _bCustomUpdateUpMatters = StaticLibrary.GetBoolSetting("CustomUpdateUpMatters", "", false);
                _sCustomUpdateUpMatters = StaticLibrary.GetSetting("CustomUpdateUpMattersScript", "", "");
                //for deletion of old entries from fdExportServiceLog
                _sLogDays = StaticLibrary.GetSetting("LogDays", "", "14");
                _sTrackingColumnType = StaticLibrary.GetSetting("TrackingColumnType", "", "");
                //Added for Contact Support
                _bExportContact = StaticLibrary.GetBoolSetting("ExportContacts", "", ExportObject == "E3E" || ExportObject == "C3E");
                _bUpdateContact = StaticLibrary.GetBoolSetting("UpdateContacts", "", ExportObject == "E3E" || ExportObject == "C3E");
                _bCustomUpdateExContacts = StaticLibrary.GetBoolSetting("CustomUpdateExContacts", "", false);
                _sCustomUpdateExContacts = StaticLibrary.GetSetting("CustomUpdateExContactsScript", "", "");
                _bCustomUpdateUpContacts = StaticLibrary.GetBoolSetting("CustomUpdateUpContacts", "", false);
                _sCustomUpdateUpContacts = StaticLibrary.GetSetting("CustomUpdateUpContactsScript", "", "");
                //to change way admins are emails counts errors and if more than 1 will email
                //this will be read at the end of the run by the main service.
                _errorCounter = 0;
                // logs to new errorlog table 
                _logToDB = StaticLibrary.GetBoolSetting("LogToDB", "", false);
                if (_logToDB)
                    ClearLogTable();
                try
				{
					_exceptionThreashold = Convert.ToInt32(FWBS.OMS.OMSEXPORT.StaticLibrary.GetSetting("ExceptionThreashold","","100"));
				}
				catch
				{
					_exceptionThreashold = 5;
				}
				
				//implemented by the sub class performas any initial setup
				InitialiseProcess();

				//use same table variable throughout
				DataTable dtData;
                DataTable newTable;
				//used to construnct dynamic statements based on export object
				string statement;	
								
				if(CheckStop())
					return;

                if (_bLookups)
                {
                    //Lookup codes each export object will be treated differently
                    RaiseStatusEvent("Getting Lookups for Export");
                    statement = "exec spr" + ExportObject + "ExportLookups";
                    dtData = GetDataTable(statement);

                    if (dtData != null)
                    {
                        //added for UTC support
                        newTable = dtData.Clone();
                        
                        //make sure all date time columns are UTC aware
                        foreach (DataColumn col in newTable.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Utc;
                            }
                        }

                        newTable.Merge(dtData, false, MissingSchemaAction.Ignore);
                        newTable.AcceptChanges();
                                                
                        RaiseStatusEvent("Exporting Lookups");
                        ExportLookups(newTable);
                        dtData = null;
                        newTable = null;
                    }
                }

                if (CheckStop())
                    return;

                if (_bUser) //Users/Fee earners
                {
                    //Locate feeEarners
                    RaiseStatusEvent("Getting users for export.");
                    statement = "exec spr" + ExportObject + "ExportUsers";
                    dtData = GetDataTable(statement);

                    //iterate clients adding to CMS system
                    if (dtData != null)
                    {
                        RaiseStatusEvent("Exporting users.");

                        //added for UTC support
                        newTable = dtData.Clone();

                        //make sure all date time columns are UTC aware
                        foreach (DataColumn col in newTable.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Utc;
                            }
                        }

                        newTable.Merge(dtData, false, MissingSchemaAction.Ignore);
                        newTable.AcceptChanges();
                        
                        ExportUsers(newTable);
                        dtData = null;
                        newTable = null;
                    }
                }
				
				if(CheckStop())
					return;


                if (_bUser) //Users/Fee earners
                {
                    //Locate feeEarners
                    RaiseStatusEvent("Getting FeeEarners for export.");
                    statement = "exec spr" + ExportObject + "ExportFeeEarners";
                    dtData = GetDataTable(statement);

                    //iterate clients adding to CMS system
                    if (dtData != null)
                    {
                        RaiseStatusEvent("Exporting Fee Earners.");

                        //added for UTC support
                        newTable = dtData.Clone();

                        //make sure all date time columns are UTC aware
                        foreach (DataColumn col in newTable.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Utc;
                            }
                        }

                        newTable.Merge(dtData, false, MissingSchemaAction.Ignore);
                        newTable.AcceptChanges();

                        ExportFeeEarners(newTable);
                        dtData = null;
                        newTable = null;
                    }
                }

                if (CheckStop())
                    return;

                if (_bExportContact)
                {
                    //Get new client data
                    RaiseStatusEvent("Getting Contacts for export.");
                    statement = "exec spr" + ExportObject + "ExportContacts";
                    dtData = GetDataTable(statement);

                    //iterate clients adding to CMS system
                    if (dtData != null)
                    {
                        RaiseStatusEvent("Exporting Contacts.");

                        //added for UTC support
                        newTable = dtData.Clone();

                        //make sure all date time columns are UTC aware
                        foreach (DataColumn col in newTable.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Utc;
                            }
                        }

                        newTable.Merge(dtData, false, MissingSchemaAction.Ignore);
                        newTable.AcceptChanges();

                        ExportContacts(newTable);
                        dtData = null;
                        newTable = null;
                    }
                }
                if (CheckStop())
                    return;

                if (_bUpdateContact)
                {
                    //get update client data
                    RaiseStatusEvent("Getting Contacts for update.");
                    statement = "exec spr" + ExportObject + "UpdateContacts";
                    dtData = GetDataTable(statement);

                    //iterate Contacts updating any neccessary
                    if (dtData != null)
                    {
                        RaiseStatusEvent("Updating existing Contacts");

                        //added for UTC support
                        newTable = dtData.Clone();

                        //make sure all date time columns are UTC aware
                        foreach (DataColumn col in newTable.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Utc;
                            }
                        }

                        newTable.Merge(dtData, false, MissingSchemaAction.Ignore);
                        newTable.AcceptChanges();

                        UpdateContacts(newTable);
                        dtData = null;
                        newTable = null;
                    }
                }

                if (CheckStop())
                    return;


                if (_bExportClient) //Client Exports
                {
                    //Get new client data
                    RaiseStatusEvent("Getting Clients for export.");
                    statement = "exec spr" + ExportObject + "ExportClients";
                    dtData = GetDataTable(statement);

                    //iterate clients adding to CMS system
                    if (dtData != null)
                    {
                        RaiseStatusEvent("Exporting Clients.");

                        //added for UTC support
                        newTable = dtData.Clone();

                        //make sure all date time columns are UTC aware
                        foreach (DataColumn col in newTable.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Utc;
                            }
                        }

                        newTable.Merge(dtData, false, MissingSchemaAction.Ignore);
                        newTable.AcceptChanges();

                        ExportClients(newTable);
                        dtData = null;
                        newTable = null;
                    }
                }								
				if(CheckStop())
					return;

                if (_bUpdateClient)
                {
                    //get update client data
                    RaiseStatusEvent("Getting Clients for update.");
                    statement = "exec spr" + ExportObject + "UpdateClients";
                    dtData = GetDataTable(statement);

                    //iterate clients updating any neccessary
                    if (dtData != null)
                    {
                        RaiseStatusEvent("Updating existing clients");

                        //added for UTC support
                        newTable = dtData.Clone();

                        //make sure all date time columns are UTC aware
                        foreach (DataColumn col in newTable.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Utc;
                            }
                        }

                        newTable.Merge(dtData, false, MissingSchemaAction.Ignore);
                        newTable.AcceptChanges();
                        
                        UpdateClients(newTable);
                        dtData = null;
                        newTable = null;
                    }
                }
								
				if(CheckStop())
					return;

                if (_bExportMatter)
                {
                    //get new file data
                    RaiseStatusEvent("Getting Matters for export.");
                    statement = "exec spr" + ExportObject + "ExportMatters";
                    dtData = GetDataTable(statement);

                    //iterate files adding to CMS system
                    if (dtData != null)
                    {
                        RaiseStatusEvent("Exporting Matters.");

                        //added for UTC support
                        newTable = dtData.Clone();

                        //make sure all date time columns are UTC aware
                        foreach (DataColumn col in newTable.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Utc;
                            }
                        }

                        newTable.Merge(dtData, false, MissingSchemaAction.Ignore);
                        newTable.AcceptChanges();

                        ExportMatters(newTable);
                        dtData = null;
                        newTable = null;
                    }
                }
								
				if(CheckStop())
					return;

                if (_bUpdateMatter)
                {
                    //get update file data
                    RaiseStatusEvent("Getting Matters for update.");
                    statement = "exec spr" + ExportObject + "UpdateMatters";
                    dtData = GetDataTable(statement);

                    //iterate update file data
                    if (dtData != null)
                    {
                        RaiseStatusEvent("Updating Matters");

                        //added for UTC support
                        newTable = dtData.Clone();

                        //make sure all date time columns are UTC aware
                        foreach (DataColumn col in newTable.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Utc;
                            }
                        }

                        newTable.Merge(dtData, false, MissingSchemaAction.Ignore);
                        newTable.AcceptChanges();

                        UpdateMatters(newTable);
                        dtData = null;
                        newTable = null;
                    }
                }
							
				if(CheckStop())
					return;

                if (_bTime)
                {
                    //get time entry data
                    RaiseStatusEvent("Getting Time entries for export.");
                    statement = "exec spr" + ExportObject + "ExportTime";
                    dtData = GetDataTable(statement);

                    //Export time recording entries
                    if (dtData != null)
                    {
                        RaiseStatusEvent("Exporting time entries.");

                        //added for UTC support
                        newTable = dtData.Clone();

                        //make sure all date time columns are UTC aware
                        foreach (DataColumn col in newTable.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Utc;
                            }
                        }

                        newTable.Merge(dtData, false, MissingSchemaAction.Ignore);
                        newTable.AcceptChanges();

                        ExportTime(newTable);
                        dtData = null;
                        newTable = null;
                    }
                }

				if(CheckStop())
					return;

                if (_bFinance)
                {
                    //FINANCIALS
                    //Export Finance logger entries
                    RaiseStatusEvent("Getting Financial entries for export.");
                    statement = "exec spr" + ExportObject + "ExportFinancials";
                    dtData = GetDataTable(statement);

                    //Export time recording entries
                    if (dtData != null)
                    {
                        RaiseStatusEvent("Exporting financial entries.");

                        //added for UTC support
                        newTable = dtData.Clone();

                        //make sure all date time columns are UTC aware
                        foreach (DataColumn col in newTable.Columns)
                        {
                            if (col.DataType == typeof(DateTime))
                            {
                                col.DateTimeMode = DataSetDateTime.Utc;
                            }
                        }

                        newTable.Merge(dtData, false, MissingSchemaAction.Ignore);
                        newTable.AcceptChanges();

                        ExportFinancials(newTable);
                        dtData = null;
                        newTable = null;
                    }
                }

                if(CheckStop())
                    return;

                CustomProcess();
			}

			catch(Exception ex)
			{
				LogEntry("Run Process Procedure Error: " + ex.Message,EventLogEntryType.Error);                
			}
			finally
			{
				//implemented in the sub class
				FinaliseProcess();
				//finally close the connection
				CloseOMSConnection();
				RaiseStatusEvent("Process Complete.");

				//this is important as it flags the service controller that 
				//it is safe to reurn to the service control manager
				if(CheckStop())
					_stopped =true;
			}
		}

        private void ClearLogTable()
        {
            //If _sLogDays is 0, then logs are not cleared
            if (_sLogDays == "0")
                return;

            if (Convert.ToInt64(StaticLibrary.GetSetting("LogLastClearedTicks", ExportObject, "0")) == 0)
            {
                //Reg key does not exist so set to today
                StaticLibrary.UpdateSetting("LogLastClearedTicks", ExportObject, System.DateTime.UtcNow.Date.Ticks);
            }
            else
            {
                long logLastClearedTicks = Convert.ToInt64(StaticLibrary.GetSetting("LogLastClearedTicks", ExportObject, "0"));
                TimeSpan t = new TimeSpan(System.DateTime.UtcNow.Date.Ticks-logLastClearedTicks);
                //If last cleared over a day ago, then run the delete based on the _sLogDays setting
                if (t.Days>0)
                {
                    try
                    {
                        List<SqlParameter> parList = new List<SqlParameter>();
                        SqlParameter parApplication = new SqlParameter("APPLICATION", ExportObject);
                        SqlParameter parLogDays = new SqlParameter("LOGDAYS",_sLogDays);
                        parList.Add(parApplication);
                        parList.Add(parLogDays);
                        ExecuteSQL("DELETE FROM FDEXPORTSERVICELOG WHERE APPLICATION = @APPLICATION AND DATEDIFF ( d , DATELOGGED , GETUTCDATE () ) > @LOGDAYS", parList);
                        RaiseStatusEvent(string.Format("Deleted from FDEXPORTSERVICELOG for Application '{0}' and where logs older than {1} days.", ExportObject , _sLogDays));
                        StaticLibrary.UpdateSetting("LogLastClearedTicks", ExportObject, System.DateTime.UtcNow.Date.Ticks);
                    }
                    catch (Exception ex)
                    {
                        RaiseStatusEvent("Problem deleting from FDEXPORTSERVICELOG " + ex.Message);
                    }
                }
            }
        }


		
		#endregion

		#region Protected members
			
		/// <summary>
		/// Checks if a stop has been called
		/// </summary>
		/// <returns></returns>
		protected internal bool CheckStop()
		{
			//check if it has beenasked to pause
			while(_pause)
			{
				//need to call sleep for a second
				Thread.Sleep(1000);
			}
			//if we are in debug mode pause for 5 seconds
			if(_fullLog)
			{	
				Thread.Sleep(1000);
			}

			if(_stop)
			{
				return true;
			}
			else
				return false;
		}

		
		/// <summary>
		/// General procedure for raising status events
		/// </summary>
		/// <param name="message">the message to raise in the event args</param>
		protected internal void RaiseStatusEvent(string message)
		{
			//raise the event
			if(OnStatusChange != null)
			{
				try
				{

					//create an instance of the event arguments
					StatusEventArgs e = new StatusEventArgs(message);
					OnStatusChange(this,e);
				}
				catch{}
			}
		}
		
        /// <summary>
		/// Opens a connection to the OMS database
		/// </summary>
		/// <returns>True if successful</returns>
		protected internal void OpenOMSConnection()
        {
            if (_cnn.State == ConnectionState.Open)
                return;
            try
            {
                RaiseStatusEvent("Opening Database connection.");
                _cnn.Open();
                RaiseStatusEvent("Database connection open.");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
	
		/// <summary>
		/// Closes the active connection the the database
		/// </summary>
		protected internal void CloseOMSConnection()
		{	
			try
			{
				//only continue if it is not null
				if(_cnn != null)
				{
					try
					{
						if(_cnn.State == ConnectionState.Open)
							_cnn.Close();
					}
					catch{}
				}				
				RaiseStatusEvent("Database connection closed.");
			}
			catch{}
		}
		
		
		/// <summary>
		/// Executes a query against the OMS database
		/// </summary>
		/// <param name="SQL">Execute statement</param>
		/// <returns>The number of rows affected</returns>
		protected internal int ExecuteSQL(string SQL)
		{
			SqlCommand cmd = new SqlCommand(SQL,OMSConnection);
			int retVal = cmd.ExecuteNonQuery();
			//try and prevent an open Datareader error periodically happening
			//UNTESTED
			if(OMSConnection.State== ConnectionState.Open)
                OMSConnection.Close();
			return retVal;
		}

        /// <summary>
        /// Executes a query against the OMS database
        /// </summary>
        /// <param name="SQL">Execute statement</param>
        /// <param name="parameters">List of parameters for execution statement</param>
        /// <returns>The number of rows affected</returns>
        public int ExecuteSQL(string SQL, IEnumerable<SqlParameter> parameters)
        {
            var cmd = CreateSqlCommand(SQL, parameters);

            int retVal = cmd.ExecuteNonQuery();
            //try and prevent an open Datareader error periodically happening
            //UNTESTED
            if (OMSConnection.State == ConnectionState.Open)
                OMSConnection.Close();
            return retVal;
        }

	    public virtual SqlCommand CreateSqlCommand(string SQL, IEnumerable<SqlParameter> parameters)
	    {
	        SqlCommand cmd = new SqlCommand(SQL, OMSConnection);

	        foreach (SqlParameter param in parameters)
	        {
	            cmd.Parameters.Add(param);
	        }
	        return cmd;
	    }


	    /// <summary>
		/// Returns the first datatable within the passed sql statement
		/// </summary>
		/// <param name="SQL">SQL query</param>
		/// <returns>DataTable of results</returns>
		protected internal DataTable GetDataTable(string SQL)
		{
			DataTable dtRet = new DataTable();
			SqlCommand cmd = new SqlCommand(SQL,OMSConnection);
			SqlDataAdapter adpt = new SqlDataAdapter();
								
			adpt.SelectCommand = cmd; 
			
			adpt.Fill(dtRet);

			//try and prevent an open Datareader error periodically happening
			//UNTESTED
			OMSConnection.Close();

			return dtRet;
		}
        /// <summary>
        /// Returns the first datatable within the passed sql statement
        /// </summary>
        /// <param name="SQL">SQL query</param>
        /// <param name="parameters">List of parameters for execution statement</param>
        /// <returns>DataTable of results</returns>
        protected internal DataTable GetDataTable(string SQL, List<SqlParameter> parameters)
        {
            DataTable dtRet = new DataTable();
            SqlCommand cmd = new SqlCommand(SQL, OMSConnection);

            foreach (SqlParameter param in parameters)
            {
                cmd.Parameters.Add(param);
            }

            SqlDataAdapter adpt = new SqlDataAdapter();
            

            adpt.SelectCommand = cmd;



            adpt.Fill(dtRet);

            //try and prevent an open Datareader error periodically happening
            //UNTESTED
            OMSConnection.Close();

            return dtRet;
        }

        /// <summary>
        /// Exports Lookup data
        /// </summary>
        /// <param name="dtData">Table of Lookup Codes</param>
        protected void ExportLookups(DataTable dtData)
        {
            int exc = 0;
            int count = 0;

            //first check we have some rows to process
            if (dtData.Rows.Count < 1)
            {
                RaiseStatusEvent("0 Lookup records to export.");
                return;
            }
            
            //now iterate each row
            foreach (DataRow row in dtData.Rows)
            {
                if (CheckStop())
                    return;

                try
                {
                    //Not sure exactly how to implement at this time 12/06/2006
                    _logCounter.LookupsAdded += 1;
                }
                
                catch (Exception ex) //do something with each exception
                {
                    
                    _logCounter.Errors += 1;
                    LogEntry("ExportLookups:" + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    
                    exc++; //exception threshold counter
                    _errorCounter++;
                    if (exc >= _exceptionThreashold)
                    {
                        LogEntry("ExportLookups: Exception Threashold of " + Convert.ToString(exc) + " Reached", System.Diagnostics.EventLogEntryType.Warning);
                        CloseOMSConnection();
                        return;
                    }

                    if (_logToDB)
                    {
                        StaticLibrary.LogToDatabase(OMSConnection, ex.Message, "LOOKUP","", "I");
                    }
                }
            }
            RaiseStatusEvent(Convert.ToString(count) + " Lookup codes exported.");

        }
        

		/// <summary>
		/// Doesn't actually export as it just has to find them 
		/// </summary>
		/// <param name="?"></param>
		protected void ExportUsers(DataTable dtData)
		{	
			int exc = 0; //exception counter
            int count = 0; //export counter
			
			//first check we have some rows to process
			if(dtData.Rows.Count < 1)
			{
				RaiseStatusEvent("0 User records to export.");
				return;
			}
					
			//now iterate each row
			foreach(DataRow row in dtData.Rows)
			{
				if(CheckStop())
					return;

                string Usr = "";

                try
                {
                    Usr = Convert.ToString(row["usrID"]);
                    int iUno = ExportUser(row);
                    
                    //update OMS fee earner if located
                    if (iUno != 0)
                    {
                        string sUno = Convert.ToString(iUno);
                        //Parameters
                        List<SqlParameter> parList = new List<SqlParameter>();
                        SqlParameter parUSRID = new SqlParameter("USRID", Usr);
                        SqlParameter parUSREXTID = new SqlParameter("USREXTID", sUno);

                        parList.Add(parUSRID);
                        parList.Add(parUSREXTID);

                        string sql = "update dbuser set usrExtID = @USREXTID WHERE USRID = @USRID";

                        int rows = ExecuteSQL(sql, parList);

                        if (rows < 1)
                            throw new Exception("Update of user failed with feeusrID of " + Usr + " and external ID of " + sUno);

                        count++;
                        _logCounter.UsersAdded += 1;
                    }
                }
                
                catch (Exception ex) //do something with each exception
                {
                    _logCounter.Errors += 1;
                    exc++;
                    _errorCounter++;
                    
                    LogEntry("ExportUsers:" + " ID:" + Usr + " " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    if (exc >= _exceptionThreashold)
                    {
                        LogEntry("ExportUsers: Exception Threashold of " + Convert.ToString(exc) + " Reached", System.Diagnostics.EventLogEntryType.Warning);
                        CloseOMSConnection();
                        return;
                    }

                    if (_logToDB)
                    {
                        StaticLibrary.LogToDatabase(OMSConnection, ex.Message, "USER", Usr, "I");
                    }                
                    //do not throw any more treat users the same as other exports
                    //throw enew;
                }
			}
            RaiseStatusEvent(Convert.ToString(count) + " User records exported. " + Convert.ToString(exc) + " Users failed");
		}

        //tidy up
        protected void ExportFeeEarners(DataTable dtData)
        {
            int exc = 0; //exception counter
            int count = 0; //export counter

            //first check we have some rows to process
            if (dtData.Rows.Count < 1)
            {
                RaiseStatusEvent("0 User records to export.");
                return;
            }

            //now iterate each row
            foreach (DataRow row in dtData.Rows)
            {
                if (CheckStop())
                    return;

                string Usr = "";

                try
                {
                    Usr = Convert.ToString(row["feeusrID"]);
                    int iUno = ExportFeeEarner(row);
                    
                    //update OMS fee earner if located
                    if (iUno != 0)
                    {
                        string sUno = Convert.ToString(iUno);

                        //Parameters
                        List<SqlParameter> parList = new List<SqlParameter>();
                        SqlParameter parFEEUSRID = new SqlParameter("FEEUSRID ", Usr);
                        SqlParameter parFEEEXTID = new SqlParameter("FEEEXTID", sUno);
                        parList.Add(parFEEUSRID);
                        parList.Add(parFEEEXTID);

                        string sql = "update dbFeeEarner set feeExtID = @FEEEXTID WHERE FEEUSRID = @FEEUSRID";
                        int rows = ExecuteSQL(sql,parList);

                        if (rows < 1)
                            throw new Exception("Update of user failed with feeusrID of " + Usr + " and external ID of " + sUno);

                        count++;
                        _logCounter.UsersAdded += 1;
                    }
                }

                catch (Exception ex) //do something with each exception
                {
                    _logCounter.Errors += 1;
                    exc++;
                    _errorCounter++;

                    LogEntry("ExportFeeEarners:" + " ID:" + Usr + " " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    if (exc >= _exceptionThreashold)
                    {
                        LogEntry("ExportFeeEarners: Exception Threashold of " + Convert.ToString(exc) + " Reached", System.Diagnostics.EventLogEntryType.Warning);
                        CloseOMSConnection();
                        return;
                    }

                    if (_logToDB)
                    {
                        StaticLibrary.LogToDatabase(OMSConnection, ex.Message, "USER", Usr, "I");
                    }
                    //do not throw any more treat users the same as other exports
                    //throw enew;
                }
            }
            RaiseStatusEvent(Convert.ToString(count) + " FeeEarner records exported. " + Convert.ToString(exc) + " FeeEarners failed");
        }
		
		/// <summary>
		/// Main process to export Clients 
		/// </summary>
		/// <param name="clData"></param>
		protected void ExportClients(DataTable clData)
		{
			int exc = 0; //counter for exception threshold
            int count = 0; //used for logging at end of run

			//first check we have some frows to process
			if(clData.Rows.Count < 1)
			{
				RaiseStatusEvent("0 Clients to export.");
				return;
			}
            
			//now iterate each row
			foreach(DataRow row in clData.Rows)
			{
				if(CheckStop())
					return;
				
				string clID = "";
				string clExtID = "";
				try
				{
					clID = Convert.ToString(row["clID"]);
                    clExtID = Convert.ToString(ExportClient(row));

					//update OMS to flag client exported along with capture uno from CMS
					//here we need to check for the custom flag and use the custom string
                    string sql;
                    //Parameters
                    List<SqlParameter> parList = new List<SqlParameter>();
                    SqlParameter parCLID = new SqlParameter("CLID", clID);
                    SqlParameter parCLEXTID = new SqlParameter("CLEXTID", clExtID);
                    parList.Add(parCLID);
                    parList.Add(parCLEXTID);

                    if (_bCustomUpdateExClients)
                    {
                        if(_sCustomUpdateExClients.Contains(" "))
                            throw new Exception("Custom Update Setting cannot contain a space");

                        sql = string.Format("exec {0} @CLEXTID , @CLID", _sCustomUpdateExClients);

                    }
                    else
                    {
                        if (StaticLibrary.GetExportAppName() == "ENT")
                        {
                            sql = GetExportMappingSQL("Enterprise",ExportMappingEntityType.Client,clID,clExtID);
                        }
                        else
                        {
                            //  Tracking Column Type (Integer or String)
                            if (_sTrackingColumnType.Equals("String"))
                            {
                                sql = "update dbclient set clneedexport = 0 , clexttxtid = @CLEXTID WHERE CLID = @CLID";
                            }
                            else
                            {
                                sql = "update dbclient set clneedexport = 0 , clextid = @CLEXTID where clid = @CLID";
                            }
                        }
                    }

                    int rows = ExecuteSQL(sql,parList);

					if(rows < 1)
						throw new Exception("Update of client failed with ID of " + clID + " and external ID of " + clExtID);
                    count++;
					_logCounter.ClientsAdded +=1;

				}
				catch(Exception ex) //do something with each exception
				{
					
					_logCounter.Errors +=1;
					LogEntry("ExportClients:" + " ID:" + clID + " " + ex.Message,System.Diagnostics.EventLogEntryType.Error);
					//it doesn't just keep erroring
                    exc++;
                    _errorCounter++;
					if(exc >= _exceptionThreashold)
					{
						LogEntry("ExportClients:Exception Threashold of " + Convert.ToString(exc) + " Reached",System.Diagnostics.EventLogEntryType.Warning);
						CloseOMSConnection();
						return;
					}

                    if (_logToDB)
                    {
                        StaticLibrary.LogToDatabase(OMSConnection, ex.Message, "CLIENT", clID, "I");
                    }
				}
			}
			RaiseStatusEvent(Convert.ToString(count) + " Clients exported " + Convert.ToString(exc) + " Clients failed");
			
		}


		/// <summary>
		/// Process client records to update
		/// </summary>
		/// <param name="clData">data table of client records</param>
		protected void UpdateClients(DataTable clData)
		{
            int exc = 0; //counter for exception threshold
            int count = 0; //used for logging at end of run
			
			//first check we have some frows to process
			if(clData.Rows.Count < 1)
			{
				RaiseStatusEvent("0 Clients to update.");
				return;
			}
            			
			//now iterate each row
			foreach(DataRow row in clData.Rows)
			{
				
				if(CheckStop())
					return;
				
				string clID = "";
				try
				{
					clID = Convert.ToString(row["clID"]);
                    
					bool bVal = UpdateClient(row);
										
					if(bVal)
					{
                        //Parameters
                        List<SqlParameter> parList = new List<SqlParameter>();
                        SqlParameter parCLID = new SqlParameter("CLID", clID);

                        parList.Add(parCLID);
                        //update OMS to flag client exported along with capture uno from CMS
                        string sql;
                        if (_bCustomUpdateUpClients)
                        {
                            if (_sCustomUpdateUpClients.Contains(" "))
                                throw new Exception("Custom Update Setting cannot contain a space");
                            sql = string.Format("exec {0} NULL , @CLID",_sCustomUpdateUpClients);
                        }
                        else
                        {
                            sql = "update dbclient set clneedexport = 0 where clid = @CLID";
                        }
					
						int rows = ExecuteSQL(sql, parList);

						if(rows < 1)
							throw new Exception("Update of client failed with ID of " + clID);

						_logCounter.ClientsUpdated +=1;
                        count++;
					}
					else
					{
						exc++;
						_logCounter.Errors +=1;
						LogEntry("Update client returned false for client ID " + clID,EventLogEntryType.Warning);	
					}
				}
				catch(Exception ex) //do something with each exception
				{
					_logCounter.Errors +=1;
					LogEntry("UpdateClients:" + " ID:" + clID + " " + ex.Message,System.Diagnostics.EventLogEntryType.Error);
					//it doesn't just keep erroring
					exc ++;
                    _errorCounter++;
					if(exc >= _exceptionThreashold)
					{
						LogEntry("UpdateClients:Exception Threashold of " + Convert.ToString(exc) + " Reached",System.Diagnostics.EventLogEntryType.Warning);
						return;
					}

                    if (_logToDB)
                    {
                        StaticLibrary.LogToDatabase(OMSConnection, ex.Message, "CLIENT", clID, "U");
                    }
				}
			}
			RaiseStatusEvent(Convert.ToString(count) + " Clients updated " + Convert.ToString(exc) + " Clients failed");
		}

        /// <summary>
        /// Main process to export Contacts 
        /// </summary>
        /// <param name="clData"></param>
        protected void ExportContacts(DataTable contData)
        {
            int exc = 0; //counter for exception threshold
            int count = 0; //used for logging at end of run

            //first check we have some frows to process
            if (contData.Rows.Count < 1)
            {
                RaiseStatusEvent("0 Contacts to export.");
                return;
            }

            //now iterate each row
            foreach (DataRow row in contData.Rows)
            {
                if (CheckStop())
                    return;

                string contID = "";
                string contExtID = "";
                try
                {
                    contID = Convert.ToString(row["contID"]);
                    contExtID = Convert.ToString(ExportContact(row));
                    //Parameters
                    List<SqlParameter> parList = new List<SqlParameter>();
                    SqlParameter parCONTID = new SqlParameter("CONTID", contID);
                    SqlParameter parCONTEXTID = new SqlParameter("CONTEXTID", contExtID);
                    parList.Add(parCONTID);
                    parList.Add(parCONTEXTID);
                    //update OMS to flag client exported along with capture uno from CMS
                    //here we need to check for the custom flag and use the custom string
                    string sql;
                    if (_bCustomUpdateExContacts)
                    {
                        if(_sCustomUpdateExContacts.Contains(" "))
                            throw new Exception("Custom Update Setting cannot contain a space");
                        sql = string.Format("exec {0} @CONTEXTID , @CONTID", _sCustomUpdateExContacts);
                    }
                    else
                    {
                        //  Tracking Column Type (Integer or String)
                        if (_sTrackingColumnType.Equals("String"))
                        {
                            sql = "update dbcontact set contneedexport = 0 , contexttxtid = @CONTEXTID where contid = @CONTID";
                        }
                        else
                        {
                            sql = "update dbcontact set contneedexport = 0 , contextid = @CONTEXTID where contid = @CONTID";
                        }
                    }

                    int rows = ExecuteSQL(sql, parList);

                    if (rows < 1)
                        throw new Exception("Update of contact failed with ID of " + contID + " and external ID of " + contExtID);
                    count++;
                    _logCounter.ContactsAdded += 1;

                }
                catch (Exception ex) //do something with each exception
                {

                    _logCounter.Errors += 1;
                    LogEntry("ExportContacts:" + " ID:" + contID + " " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    //it doesn't just keep erroring
                    exc++;
                    _errorCounter++;
                    if (exc >= _exceptionThreashold)
                    {
                        LogEntry("ExportClients:Exception Threashold of " + Convert.ToString(exc) + " Reached", System.Diagnostics.EventLogEntryType.Warning);
                        CloseOMSConnection();
                        return;
                    }

                    if (_logToDB)
                    {
                        StaticLibrary.LogToDatabase(OMSConnection, ex.Message, "CONTACT", contID, "I");
                    }
                }
            }
            RaiseStatusEvent(Convert.ToString(count) + " Contacts exported " + Convert.ToString(exc) + " Contacts failed");

        }

        /// <summary>
        /// Process contact records to update
        /// </summary>
        /// <param name="clData">data table of contact records</param>
        protected void UpdateContacts(DataTable contData)
        {
            int exc = 0; //counter for exception threshold
            int count = 0; //used for logging at end of run

            //first check we have some frows to process
            if (contData.Rows.Count < 1)
            {
                RaiseStatusEvent("0 Contacts to update.");
                return;
            }

            //now iterate each row
            foreach (DataRow row in contData.Rows)
            {

                if (CheckStop())
                    return;

                string contID = "";
                try
                {
                    contID = Convert.ToString(row["contID"]);

                    bool bVal = UpdateContact(row);

                    if (bVal)
                    {
                        //Parameters
                        List<SqlParameter> parList = new List<SqlParameter>();
                        SqlParameter parCONTID = new SqlParameter("CONTID", contID);

                        parList.Add(parCONTID);
                        //update OMS to flag contact exported along with capture uno from CMS
                        string sql;
                        if (_bCustomUpdateUpContacts)
                        {
                            if (_sCustomUpdateUpContacts.Contains(" "))
                                throw new Exception("Custom Update Setting cannot contain a space");

                            sql = string.Format("exec {0} NULL , @CONTID", _sCustomUpdateUpContacts);
                        }
                        else
                        {
                            sql = "update dbContact set contneedexport = 0 where contID = @CONTID";
                        }

                        int rows = ExecuteSQL(sql, parList);

                        if (rows < 1)
                            throw new Exception("Update of contact failed with ID of " + contID);

                        _logCounter.ContactsUpdated += 1;
                        count++;
                    }
                    else
                    {
                        exc++;
                        _logCounter.Errors += 1;
                        LogEntry("Update contact returned false for contact ID " + contID, EventLogEntryType.Warning);
                    }
                }
                catch (Exception ex) //do something with each exception
                {
                    _logCounter.Errors += 1;
                    LogEntry("UpdateContacts:" + " ID:" + contID + " " + ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    //it doesn't just keep erroring
                    exc++;
                    _errorCounter++;
                    if (exc >= _exceptionThreashold)
                    {
                        LogEntry("UpdateContacts:Exception Threashold of " + Convert.ToString(exc) + " Reached", System.Diagnostics.EventLogEntryType.Warning);
                        return;
                    }

                    if (_logToDB)
                    {
                        StaticLibrary.LogToDatabase(OMSConnection, ex.Message, "CONTACT", contID, "U");
                    }
                }
            }
            RaiseStatusEvent(Convert.ToString(count) + " Contacts updated " + Convert.ToString(exc) + " Contacts failed");
        }

       /// <summary>
		/// Exports file records to external system
		/// </summary>
		/// <param name="dtMatters"></param>
		protected void ExportMatters(DataTable dtMatters)
		{
            int exc = 0; //counter for exception threshold
            int count = 0; //used for logging at end of run

			//first check we have some frows to process
			if(dtMatters.Rows.Count < 1)
			{
				RaiseStatusEvent("0 Matters to export.");
				return;
			}
						
			//now iterate each row
			foreach(DataRow row in dtMatters.Rows)
			{
				if(CheckStop())
					return;
				
				string matUno = "";
				string fileID = "";	
				
				try
				{

					fileID = Convert.ToString(row["fileID"]);
                    //matUno = (string)ExportMatter(row); - this caused failure so now using Convert.ToString - GM 05-Oct-2009
                    matUno = Convert.ToString(ExportMatter(row));
                    //Parameters
                    List<SqlParameter> parList = new List<SqlParameter>();
                    SqlParameter parFILEID = new SqlParameter("FILEID", fileID);
                    SqlParameter parFILEEXTLINKID = new SqlParameter("FILEEXTLINKID", matUno);

                    parList.Add(parFILEID);
                    parList.Add(parFILEEXTLINKID);
					//update OMS to flag client exported along with capture uno from CMS
					string sql; 

                    if (_bCustomUpdateExMatters)
                    {
                        if (_sCustomUpdateExMatters.Contains(" "))
                            throw new Exception("Custom Update Setting cannot contain a space");

                        sql = string.Format("exec {0} @FILEID , @FILEEXTID", _sCustomUpdateExMatters);
                    }
                    else
                    {
                        if (StaticLibrary.GetExportAppName() == "ENT")
                        {
                            sql = GetExportMappingSQL("Enterprise", ExportMappingEntityType.File, fileID, matUno);
                        }
                        else
                        {
                            
                            //  Tracking Column Type (Integer or String)
                            if (_sTrackingColumnType.Equals("String"))
                            {
                                sql = "update dbfile set fileneedexport = 0 , fileExtLinkTxtID = @FILEEXTLINKID WHERE FILEID = @FILEID";
                            }
                            else
                            {
                                sql = "update dbfile set fileneedexport = 0 , fileExtLinkID = @FILEEXTLINKID WHERE FILEID = @FILEID";
                            }
                        }
                    }

                    int rows = ExecuteSQL(sql,parList);

					if(rows < 1)
						throw new Exception("Update of matter failed with ID of " + fileID + " and external ID of " + matUno);

                    count++;
					_logCounter.MattersAdded +=1;
				}
				catch(Exception ex) //do something with each exception
				{
					
					_logCounter.Errors +=1;
					LogEntry("ExportMatters:" + " ID:" + fileID + " " + ex.Message,System.Diagnostics.EventLogEntryType.Error);
					//it doesn't just keep erroring
					exc ++;
                    _errorCounter++;
					if(exc >= _exceptionThreashold)
					{
						LogEntry("ExportMatters:Exception Threashold of " + Convert.ToString(exc) + " Reached",System.Diagnostics.EventLogEntryType.Warning);
						return;
					}

                    if (_logToDB)
                    {
                        StaticLibrary.LogToDatabase(OMSConnection, ex.Message, "MATTER", fileID, "I");
                    }
				}
				
			}
			RaiseStatusEvent(Convert.ToString(count) + " Matters exported " + Convert.ToString(exc) +" Matters failed");
		}
				

		/// <summary>
		/// Updates any changed Matters
		/// </summary>
		/// <param name="dtMatters"></param>
		protected void UpdateMatters(DataTable dtMatters)
		{
            int exc = 0; //counter for exception threshold
            int count = 0; //used for logging at end of run

			//first check we have some frows to process
			if(dtMatters.Rows.Count < 1)
			{
				RaiseStatusEvent("0 Matters to update.");
				return;
			}
		
			//now iterate each row
			foreach(DataRow row in dtMatters.Rows)
			{
				
				if(CheckStop())
					return;

				string fileID = "";

				try
				{
					fileID = Convert.ToString(row["fileID"]);

					bool bVal = UpdateMatter(row);
					
					if(bVal)
					{
                        //Parameters
                        List<SqlParameter> parList = new List<SqlParameter>();
                        SqlParameter parFILEID = new SqlParameter("FILEID", fileID);

                        parList.Add(parFILEID);
						//update OMS to flag client exported along with capture uno from CMS
                        string sql;

                        if (_bCustomUpdateUpMatters)
                        {
                            if (_sCustomUpdateUpMatters.Contains(" "))
                                throw new Exception("Custom Update Setting cannot contain a space");

                            sql = string.Format("exec {0} @FILEID",_sCustomUpdateUpMatters);
                        }
                        else
                        {
                            sql = "update dbfile set fileNeedExport = 0 where fileid = @FILEID";
                        }

						int rows = ExecuteSQL(sql,parList);

						if(rows < 1)
							throw new Exception("Update of dbFile failed with ID of " + fileID);

                        count++;
						_logCounter.MattersUpdated += 1;
					}
					else
					{
						LogEntry("Update Matter returned false for file ID " + fileID,EventLogEntryType.Warning);	
						exc++;
						_logCounter.Errors +=1;
					}
				}
				catch(Exception ex) //do something with each exception
				{
					
                    LogEntry("UpdateMatters : " + "ID:" + fileID + " " + ex.Message,System.Diagnostics.EventLogEntryType.Error);
					_logCounter.Errors +=1;

                    exc++; //it doesn't just keep erroring
                    _errorCounter++;
					if(exc >= _exceptionThreashold)
					{
						LogEntry("UpdateMatters:Exception Threashold of " + Convert.ToString(exc) + " Reached",System.Diagnostics.EventLogEntryType.Warning);
						return;
					}

                    if (_logToDB)
                    {
                        StaticLibrary.LogToDatabase(OMSConnection, ex.Message, "MATTER", fileID, "U");
                    }
				}
				
			}
			RaiseStatusEvent(Convert.ToString(count) + " Matters updated " + Convert.ToString(exc) +" Matters failed");
		}


		/// <summary>
		/// Export time records
		/// </summary>
		protected void ExportTime(DataTable dttime)
		{
            int exc = 0; //counter for exception threshold
            int count = 0; //used for logging at end of run

			//first check we have some frows to process
			if(dttime.Rows.Count < 1)
			{
				RaiseStatusEvent("0 Time records to export");
				return;
			}
			
			//now iterate each row
			foreach(DataRow row in dttime.Rows)
			{
				string timeID = "";

				if(CheckStop())
					return;
				
				try
				{
                    timeID = Convert.ToString(row["ID"]);
                    string uno = Convert.ToString(ExportTimeRecord(row));
                    //Parameters
                    List<SqlParameter> parList = new List<SqlParameter>();
                    SqlParameter parTIMEID = new SqlParameter("TIMEID", timeID);
                    SqlParameter parTIMETRANSFERREDID = new SqlParameter("TIMETRANSFERREDID", uno);

                    parList.Add(parTIMEID);
                    parList.Add(parTIMETRANSFERREDID);
                    //update OMS to flag client exported along with capture uno from CMS
                    string sql;

                    if (_bCustomUpdateTime)
                    {
                        if (_sCustomUpdateTime.Contains(" "))
                            throw new Exception("Custom Update Setting cannot contain a space");

                        sql = string.Format("exec {0} @TIMEID , @TIMETRANSFERREDID", _sCustomUpdateTime);
                    }
                    else
                    {
                        sql = "update dbtimeledger set timetransferreddate = getutcdate() , timeTransferred = 1 , timeTransferredID = @TIMETRANSFERREDID WHERE ID = @TIMEID";
                    }

					int rows = ExecuteSQL(sql, parList);

					if(rows < 1)
                        throw new Exception("Update of time ledger failed with ID of " + timeID + " and External ID of " + uno);
                    
                    count++;
					_logCounter.TimeAdded +=1;
				}
				catch(Exception ex) //do something with each exception
				{
					
					_logCounter.Errors +=1;
					LogEntry("ExportTime:" + " ID:" + timeID + " " + ex.Message,System.Diagnostics.EventLogEntryType.Error);
					//it doesn't just keep erroring
					exc ++;
                    _errorCounter++;
					if(exc >= _exceptionThreashold)
					{
						LogEntry("ExportTime:Exception Threashold of " + Convert.ToString(exc) + " Reached",System.Diagnostics.EventLogEntryType.Warning);
						return;
					}
                    if (_logToDB)
                    {
                        StaticLibrary.LogToDatabase(OMSConnection, ex.Message, "TIME", timeID, "I");
                    }

				}
				
			}
			RaiseStatusEvent(Convert.ToString(count) + " Time entries exported " + Convert.ToString(exc) +" Time entries failed");
		}
		
		/// <summary>
		/// Export time records
		/// </summary>
		protected void ExportFinancials(DataTable dt)
		{
            int exc = 0; //counter for exception threshold
            int count = 0; //used for logging at end of run
            
            //first check we have some frows to process
			if(dt.Rows.Count < 1)
			{
				RaiseStatusEvent("0 Financial records to export");
				return;
			}

			//now iterate each row
			foreach(DataRow row in dt.Rows)
			{
				string finID = "";
                string finUno = "";

				if(CheckStop())
					return;
				
				try
				{
					finID = Convert.ToString(row["ID"]);
                    finUno = Convert.ToString(ExportFinancialRecord(row));
                    //Parameters
                    List<SqlParameter> parList = new List<SqlParameter>();
                    SqlParameter parFINID = new SqlParameter("FINID", finID);
                    SqlParameter parFINEXTID = new SqlParameter("FINEXTID", finUno);
                    parList.Add(parFINID);
                    parList.Add(parFINEXTID);

                    string sql;
                    if (_bCustomUpdateFinancials)
                    {
                        if (_sCustomUpdateFinancials.Contains(" "))
                            throw new Exception("Custom Update Setting cannot contain a space");

                        sql = string.Format("exec {0} @FINID , @FINEXTID", _sCustomUpdateFinancials);                   
                    }
                    else
                    {
                        // Tracking Column Type (Integer or String)
                        if (_sTrackingColumnType.Equals("String"))
                        {
                            sql = "update dbFinancialLedger set finNeedExport = 0, finExtTxtID = @FINEXTID WHERE FINLOGID = @FINID";
                        }
                        else
                        {
                            sql = "update dbFinancialLedger set finNeedExport = 0, finExtID = @FINEXTID WHERE FINLOGID = @FINID";
                        }
                    }
					int rows = ExecuteSQL(sql,parList);

                    if (rows < 1)
                    {
                        throw new Exception("Update of financials failed with ID of " + finID + " and External ID of " + finUno);
                    }
                    else
                    {
                        string log = "spr" + ExportObject + "LogFinancialError " + Convert.ToString(finID) + ",'" + "Transferred OK" + "'";
                        ExecuteSQL(log);
                    }
                    count++;
					_logCounter.FinancialsAdded +=1;
				}
				catch(Exception ex) //do something with each exception
				{
                    //for financials only custom stored procedure to handle error logging
                    string sql = "spr" + ExportObject + "LogFinancialError " + Convert.ToString(finID) + ",'" + ex.Message + "'";
                    ExecuteSQL(sql);
					_logCounter.Errors +=1;
					LogEntry("ExportFinancials:" + " ID:" + finID + " " + ex.Message,System.Diagnostics.EventLogEntryType.Error);
					//it doesn't just keep erroring
					exc ++;
                    _errorCounter++;
					if(exc >= _exceptionThreashold)
					{
						LogEntry("ExportFinancials:Exception Threashold of " + Convert.ToString(exc) + " Reached",System.Diagnostics.EventLogEntryType.Warning);
						return;
					}

                    if (_logToDB)
                    {
                        StaticLibrary.LogToDatabase(OMSConnection, ex.Message, "FINANCIALS", finID, "I");
                    }
				}
			}
			
			RaiseStatusEvent(Convert.ToString(count) + " Financial entries exported " + Convert.ToString(exc) +" Financial entries failed");
		}

        /// <summary>
        /// Logs any errors to the event log if error logging is enabled in the registry.
        /// </summary>
        /// <param name="error">A description of the error.</param>
        /// <param name="type">An event log entry type defaulting to Error.</param>
        protected internal void LogEntry(string error, System.Diagnostics.EventLogEntryType type)
        {
            using (EventLog evt = new EventLog("OMSExport", ".", "FWBS OMS Export Service"))
            {
                evt.WriteEntry(error, type);
            }
            if (type == EventLogEntryType.Error)
            {
                _logCounter.LastError = DateTime.Now.ToShortDateString() + ":" + DateTime.Now.ToShortTimeString() + Environment.NewLine +  error;
            }
        }

		#endregion

        /// <summary>
        /// Gets databasse connection to OMS opening or creating if not already
        /// </summary>
        protected internal SqlConnection OMSConnection
        {
            get
            {
                OpenOMSConnection();              
                return _cnn;
            }
        }


		#region Abstract members

		
		/// <summary>
		/// Implements any inital logic in the sub class
		/// Called at the beginning fo the RunProcess procedure
		/// </summary>
		abstract protected internal void InitialiseProcess();
		
		/// <summary>
		/// Performs any custom logic in the sub class
		/// Called before the end of the RunProcess procedure
		/// </summary>
		virtual protected internal void CustomProcess() { }
		
		/// <summary>
		/// Performs any finishing of routines
		/// Called at the end of the run process
		/// </summary>
		abstract protected internal void FinaliseProcess();

        /// <summary>
        /// Exports an lookup Code
        /// </summary>
        /// <param name="row">Lookup Data Row</param>
        /// <returns>The code</returns>
        abstract protected internal string ExportLookup(DataRow row);

		/// <summary>
		/// Exports an individual user record
		/// </summary>
		/// <param name="row">data Row</param>
		/// <returns>external ID of user</returns>
		abstract protected internal int ExportUser(DataRow row);

        /// <summary>
        /// Exports an individual user record
        /// </summary>
        /// <param name="row">data Row</param>
        /// <returns>external ID of user</returns>
        abstract protected internal int ExportFeeEarner(DataRow row);

		/// <summary>
		/// Exports a client record to the relevant system
		/// </summary>
		/// <param name="clientRow">Data row of client record</param>
		abstract protected internal object ExportClient(DataRow row);

		/// <summary>
		/// Must implement a method to update a client
		/// </summary>
		/// <param name="row">Client data row</param>
		/// <returns>result of update</returns>
		abstract protected internal bool UpdateClient(DataRow row);


        /// <summary>
        /// Exports a contact record to the relevant system
        /// </summary>
        /// <param name="clientRow">Data row of contact record</param>
        abstract protected internal object ExportContact(DataRow row);

        /// <summary>
        /// Must implement a method to update a contact
        /// </summary>
        /// <param name="row">Contact data row</param>
        /// <returns>result of update</returns>
        abstract protected internal bool UpdateContact(DataRow row);

        /// <summary>
		/// Method to export a matter record
		/// </summary>
		/// <param name="row">matter data row</param>
		/// <returns></returns>
		abstract protected internal object ExportMatter(DataRow row); 
		
		/// <summary>
		/// Method to update a matter record
		/// </summary>
		/// <param name="row"></param>
		/// <returns></returns>
		abstract protected internal bool UpdateMatter(DataRow row);
		
		/// <summary>
		/// Exports a time entry record
		/// </summary>
		/// <param name="row">time record</param>
		/// <returns>id of newly inserted record</returns>
		abstract protected internal object ExportTimeRecord(DataRow row);

		/// <summary>
		/// Place holder for when we get onto financials
		/// </summary>
		/// <param name="row">Financlial record</param>
		/// <returns>ID of inserted record</returns>
		abstract protected internal object ExportFinancialRecord(DataRow row);
		
		/// <summary>
		/// Property to get the export object name
		/// </summary>
		abstract protected internal string ExportObject
		{
			get;
		}

		#endregion
		
		#region Private Members
		
		/// <summary>
		/// Occurs when the timer elapses currently every 10 seconds
		/// </summary>
		private void _statTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			_logCounter = FWBS.OMS.OMSEXPORT.StaticLibrary.WriteStats(_logCounter);
		}
	


		


		#endregion

		#region IDisposable Members


		public virtual void Dispose()
		{
			//disable the timer and do one final log entry
			_statTimer.Enabled = false;
			FWBS.OMS.OMSEXPORT.StaticLibrary.WriteStats(_logCounter);
			_cnn = null;
		}

		#endregion

        /// <summary>
        /// Temporarily write outputs so we can review what is going on
        /// </summary>
        /// <param name="type"></param>
        /// <param name="ID"></param>
        /// <param name="requestSent"></param>
        /// <param name="responseReceived"></param>
        /// <param name="location"></param>
        protected void WriteFile(string type, long ID, string requestSent, string responseReceived, string location)
        {
            if (!_debugMode)
            {
                return;
            }

            if (!location.EndsWith(@"\"))
            {
                location += @"\";
            }

            if (!Directory.Exists(location))
            {
                Directory.CreateDirectory(location);
            }

            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");

            WriteToFile($"{location}{type}-{ID}-Sent-{timestamp}.{dataFormat}", requestSent);
            WriteToFile($"{location}{type}-{ID}-Response-{timestamp}.{dataFormat}", responseReceived);
        }

        private void WriteToFile(string filePath, string fileData)
        {
            using (var sw = new StreamWriter(filePath))
            {
                sw.Write(fileData);
            }
        }

		/// <summary>
		/// Class to override the default event args allow passing a log message
		/// </summary>
		public class StatusEventArgs:EventArgs
		{
			#region Fields
		
			/// <summary>
			/// Message to pass back to consumers
			/// </summary>
			protected string _message;
			protected DateTime _timestamp;

			#endregion
		
			#region Constructor
		
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="message">the message to raise to subscribers</param>
			public StatusEventArgs(string message)
			{
				_message = message;
				_timestamp = DateTime.Now;
			}

			#endregion
		
			#region Properties
		
			/// <summary>
			/// the message contianed within the class
			/// </summary>
			public string Message
			{
				get { return _message; }
			}
			
			/// <summary>
			/// Not on the constructor as it is optional
			/// </summary>
			public DateTime TimeStamp
			{
				get
				{
					return _timestamp;
				}
				set
				{
					_timestamp = value;
				}
			}

			


			#endregion
		}

		/// <summary>
		/// Implement our own exception so that I can identify this particular problem
		/// </summary>
		protected class UserSearchException : Exception
		{
			//just implemet a contstructor for the message
			public UserSearchException(string Message)
			{
				new Exception(Message);
			}
		}

	}

	/// <summary>
	/// struct to store counter info
	/// </summary>
	public class LogCounter
	{
		#region Public Fields

		public long ClientsAdded =0;
		public long ClientsUpdated =0;
		public long MattersAdded=0;
		public long MattersUpdated=0;
		public long TimeAdded=0;
		public long FinancialsAdded=0;
		public long Errors=0;
		public long UsersAdded;
        public long LookupsAdded;
        public string LastError = "";
        public long ContactsAdded = 0;
        public long ContactsUpdated = 0;


		#endregion
		
		/// <summary>
		/// Override the to string method to return the value to write to log
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			string retval = ClientsAdded.ToString() + "," + ClientsUpdated.ToString() + ","
				+ MattersAdded.ToString() + "," + MattersUpdated.ToString() + ","
				+ TimeAdded.ToString() + "," + FinancialsAdded.ToString() + ","
                + Errors.ToString() + "," + UsersAdded.ToString() + ","
                + LookupsAdded.ToString() + "," + LastError;
			return retval;
		}


	}
}
