using System;
using System.Diagnostics;
using System.Threading;

namespace FWBS.OMS.OMSEXPORT
{
	public class OMSExportService : System.ServiceProcess.ServiceBase
	{
		#region Fields
		/// <summary>
		/// Registry constants
		/// </summary>
		private const string REG_APPLICATION_KEY = "OMS";
		private const string REG_VERSION_KEY = "2.0";
        /// <summary>
		/// Instance of the server that handles the running of the process
		/// </summary>
		FWBS.OMS.OMSEXPORT.OMSExportBase _service;
		/// <summary>
		/// variable to hold the timer
		/// </summary>
		private System.Timers.Timer _timer = new System.Timers.Timer();
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		/// <summary>
		/// flag to indicate the service should stop
		/// </summary>
		private bool _running;
        private EventLog _eventLog;
		/// <summary>
		/// Flag to indicate detailed event loggibg is enabled
		/// </summary>
		private bool _fullLog = false;

		#endregion

		public OMSExportService()
		{			
            // This call is required by the Windows.Forms Component Designer.
			InitializeComponent();
            if (!EventLog.SourceExists(_eventLog.Source))
            {
                EventLog.CreateEventSource(_eventLog.Source, _eventLog.Log);
            }
			_timer.Elapsed += new System.Timers.ElapsedEventHandler(_timerTick);
		}

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this._eventLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this._eventLog)).BeginInit();
            // 
            // _eventLog
            // 
            this._eventLog.Log = "OMSExport";
            this._eventLog.Source = "FWBS OMS Export Service";
            // 
            // OMSExportService
            // 
            this.CanPauseAndContinue = true;
            this.ServiceName = "3EMatterSphereOMSExportService";
            ((System.ComponentModel.ISupportInitialize)(this._eventLog)).EndInit();

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private void ParseArguments(string[] args)
        {
            string login = null, password = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("/login", StringComparison.InvariantCultureIgnoreCase) && i + 1 < args.Length)
                {
                    login = args[++i];
                }
                else if (args[i].Equals("/password", StringComparison.InvariantCultureIgnoreCase) && i + 1 < args.Length)
                {
                    password = args[++i];
                }
            }

            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(password))
            {
                string[] userNameParts = login.Split('\\');

                if (userNameParts.Length == 1) // user@domain
                    OMSExportBase.DefaultCredentials = new System.Net.NetworkCredential(userNameParts[0], password);
                else                           // domain\user
                    OMSExportBase.DefaultCredentials = new System.Net.NetworkCredential(userNameParts[1], password, userNameParts[0]);
            }
        }
		
		/// <summary>
		/// Set things in motion so service can do its work.
		/// </summary>
		protected override void OnStart(string[] args)
		{
			ParseArguments(args);

			try
			{
				//start the timer so this process can exit set timer interval to 1000
				_timer.Interval = 10000;
				_timer.AutoReset = false;
				_timer.Enabled = true;
				
				_timer.Start();
			}
			catch(Exception ex)
			{
				CriticalError("OnStart: " + ex.Message);
			}
		}
 
		/// <summary>
		/// Stop this service.
		/// </summary>
		protected override void OnStop()
		{
			//flag for loop routine within this class
			_running = false;
			
			// stop the sevice
			if(_service != null)
			{
				Thread thread = new Thread(new ThreadStart(_service.Stop));
				thread.Start();
			}					
			//need to build a delay into here to hold off returning until servie has actually stopped
			//also needs to be in try catch as we could get a null reference of _service
			try
			{
				while(! _service.Stopped)
					Thread.Sleep(1000);
			}
			catch{}
		}
		
		protected override void OnPause()
		{
			//Pause the service this happens outside of the main run sync
			if(_service != null)
				_service.Pause();
		}
		
		protected override void OnContinue()
		{
			if(_service != null)
				_service.Start();
		}
		
		/// <summary>
		/// allows a async starting of the run process
		/// </summary>
		private void _timerTick(object sender,System.Timers.ElapsedEventArgs e)
		{
			_timer.Enabled = false;
			_running = true;
			this.RunSync();
		}
		
		/// <summary>
		/// Runs the sync process until flagged to stop
		/// </summary>
		public void RunSync()
		{
			_running = true;

			try
			{
				while (_running)
				{
					if(_service == null)
					{
						if(_fullLog)
							LogEvent("Creating Service Object.");
						
						string typeName = GetExportObjectTypeName();
		
						Type objType = Type.GetType(typeName,false,true);
			
						if (objType == null)
							throw new Exception("Unable to create type '" + typeName + "'");
			
						//Get the default constructor with no arguments.
						System.Reflection.ConstructorInfo objConst = objType.GetConstructor(Type.EmptyTypes);
				
						//invoke the constructor to create the control
						_service = (OMSExportBase)objConst.Invoke(null);
					}
					else
					{
						if(_fullLog)
							LogEvent("_service object variable not null...bypassing creating object.");
					}
					
					_fullLog = _service.FullLoggingEnabled;
                    
					if(_fullLog)
						LogEvent("Subscribing to service_OnStatusChange event.");
					//subscribe to status change event
					_service.OnStatusChange += new FWBS.OMS.OMSEXPORT.OMSExportBase.StatusEventHandler(service_OnStatusChange);
									
					if(_fullLog)
						LogEvent("Calling Run Process.");
					//run export process
					_service.RunProcess();

                    //added 5/6/2006
                    //if there was any errors send an email
                    if (_service.ErrorCount > 0)
                    {
                        SendEmail("Run process generated " + Convert.ToString(_service.ErrorCount) + " Errors, consult the Event Log on the computer the service is running upon for more details.");
                    }

					if(_fullLog)
						LogEvent("OMS Export Run Process complete unsubscribing from event");
					
					//trying to track down object reference error put this little lot in a try catch
					try
					{
						//unsubscribe from events
						_service.OnStatusChange -= new FWBS.OMS.OMSEXPORT.OMSExportBase.StatusEventHandler(service_OnStatusChange);
					}
					catch{}

					int pauseInterval = _service.PauseInterval * 60;
					
					if(_fullLog)
						LogEvent("pausing for "  + Convert.ToString(pauseInterval) + " seconds");

					//counts down the iteration loop
					//if service has been paused and we are here then set flag to stop
					while(pauseInterval > 0)
					{
						Thread.Sleep(1000);
						//check if it has been flagged to stop and get out in a timely manner if so
						if(_running)
							pauseInterval--;
						else
							pauseInterval = 0;
						//if service has been paused wait here
						if(_service.Paused)
							pauseInterval = 1;
					}
					if(_fullLog)
						LogEvent("Pause finished disposing of service base object.");

					if(_fullLog)
						LogEvent("Iteration complete.");
				}
			}
			catch(Exception ex)
			{
				CriticalError("RunSync: " + ex.Message);
			}
			finally
			{
				if(_fullLog)
					LogEvent("Finalising RunSync...");

				if(_service != null)
				{
					_service.Dispose();
					_service = null;
				}
			}
		}
		
		/// <summary>
		/// this is the function to be called on any error in the service will shut the service down
		/// </summary>
		/// <param name="message"></param>
		private void CriticalError(string message)
		{
			LogEvent(message,EventLogEntryType.Error);

            SendEmail(message);
			
			_running = false;
			
			//### not used when windows app
			//this is the only way I have found to stop the service as there is not a method to do this
			//execute the dos net stop command
			System.Diagnostics.ProcessStartInfo startinfo = new System.Diagnostics.ProcessStartInfo();
			startinfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			startinfo.FileName = "net";
			startinfo.Arguments = "stop 3EMatterSphereOMSExportService";
			Process.Start(startinfo);

		}

        /// <summary>
        /// Sends email to configured administrator
        /// </summary>
        /// <param name="message"></param>
        private void SendEmail(string message)
        {
            bool emailAdminOnError = FWBS.OMS.OMSEXPORT.StaticLibrary.GetBoolSetting("EmailErrors", "", false);

            if (emailAdminOnError)
            {
                string emailaddress = FWBS.OMS.OMSEXPORT.StaticLibrary.GetSetting("EmailAddress", "", "");
                string emailFrom = FWBS.OMS.OMSEXPORT.StaticLibrary.GetSetting("EmailFrom", "", "");
                StaticLibrary.SmtpSettings smtp = FWBS.OMS.OMSEXPORT.StaticLibrary.GetSmtpSettings();
                FWBS.OMS.OMSEXPORT.StaticLibrary.SendEmail(emailaddress, emailFrom, "FWBS OMS Export Service - Error Occurred", message, smtp);
            }
        }


		/// <summary>
		/// writes to the event log
		/// </summary>
		/// <param name="message">message to write to log</param>
		private void LogEvent(string message)
		{
			//calls the standard log with 
			_eventLog.WriteEntry(message);
		}
		
		/// <summary>
		/// Wrirtes an entry to the evnt log with the passed event log entry type
		/// </summary>
		/// <param name="message">Log message</param>
		/// <param name="type"></param>
		private void LogEvent(string message, EventLogEntryType type)
		{
			_eventLog.WriteEntry(message,type);
		}
		
		/// <summary>
		/// Gets the export object type name currently in use from the registry
		/// </summary>
		/// <returns></returns>
		private string GetExportObjectTypeName()
		{
			try
			{
                string exObj = StaticLibrary.GetSetting("ExportObject", "", "");

				//check value has been set if not raise exception
				if(exObj == "")
					throw new Exception("No export object has been configured within the registry");
				
				return exObj;
			}
			catch(Exception ex)
			{
				string s = "GetExportObjectTypeName: " + ex.Message;
				Exception enew = new Exception(s);
				throw enew;
			}
		}
		
		/// <summary>
		/// Catches status change events and writes to log if full logging is enabled
		/// </summary>
		private void service_OnStatusChange(object sender, FWBS.OMS.OMSEXPORT.OMSExportBase.StatusEventArgs e)
		{
			//check if we are logging all events and if so write into log
			if(_fullLog)
			{
				LogEvent(e.Message);
			}
		}


	}
}
