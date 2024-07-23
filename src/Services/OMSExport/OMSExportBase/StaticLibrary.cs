using System;
using System.Data.SqlClient;
using System.Net.Mail;

namespace FWBS.OMS.OMSEXPORT
{
	/// <summary>
	/// Contains a selection of static functions to be used by all export applications
	/// </summary>
	public class StaticLibrary
	{
		public const string REG_APPLICATION_KEY = "OMS";    
		public const string REG_VERSION_KEY = "2.0";
        public const string REG_KEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\FWBS\OMS\2.0\OMSExport";

        public class SmtpSettings
        {
            public string Address { get; set; }
            public string Encryption { get; set; }
            public bool Authenticate { get; set;}
            public string Login { get; set; }
            public string Password { get; set; }
        }

		#region Registry Functions
		
        /// <summary>
		/// Reads registry setting
		/// </summary>
		/// <param name="KeyName">Registry Key Name</param>
		/// <param name="ApplicationName">Export application or common if left empty</param>
		/// <returns></returns>
		public static string GetSetting(string valueName,string applicationName,string defaultValue)
		{

            string regKey = "";
            string val = defaultValue;
            try
            {
                //string Determine the sub key;
                if (applicationName == "")
                    regKey = REG_KEY + @"\Settings";
                else
                    regKey = REG_KEY + @"\" + applicationName;

                object oVal = Microsoft.Win32.Registry.GetValue(regKey, valueName, "NOTSET");

                if (oVal == null || Convert.ToString(oVal) == "NOTSET") //key doesn't exist or key not set
                {
                    //write the default value instead
                    UpdateSetting(valueName, applicationName, defaultValue);
                    val = defaultValue;
                }
                else
                {
                    val = Convert.ToString(oVal);
                }

            }
            catch (Exception ex)
            {
                LogErrorMessage(ex.Message);
            }
			return val;
		}
		
		/// <summary>
		/// Reads registry setting s a boolean
		/// </summary>
		/// <param name="KeyName">Registry Key Name</param>
		/// <param name="ApplicationName">Export application or common if left empty</param>
		/// <returns></returns>
		public static bool GetBoolSetting(string KeyName,string ApplicationName,bool defaultValue)
		{
			//  Reads setting from registry bassed on application name
            string def = "False";

            if (defaultValue)
            {
                def = "True";
            }

            string val = GetSetting(KeyName,ApplicationName,def);

            if (val.ToUpper() == "FALSE")
            {
                return false;
            }
            else
            {
                return true;
            }
		}

        /// <summary>
        /// Gets the 3 character name of the application we are exporting to
        /// </summary>
        /// <returns>name of app</returns>
        public static string GetExportAppName()
        {
            string exportObj = StaticLibrary.GetSetting("ExportObject", "", "FWBS.OMS.OMSEXPORT.OMSEXPORTE3E,OMSEXPORTE3E");

            if (exportObj.ToUpper() == "FWBS.OMS.OMSEXPORT.OMSEXPORTIGO,OMSEXPORTIGO")
            {
                return "IGO";
            }
            else if (exportObj.ToUpper() == "FWBS.OMS.OMSEXPORT.OMSEXPORTMIL,OMSEXPORTMIL")
            {
                return "MIL";
            }
            else if (exportObj.ToUpper() == "FWBS.OMS.OMSEXPORT.OMSEXPORTE3E,OMSEXPORTE3E")
            {
                return "E3E";
            }
            else if (exportObj.ToUpper() == "FWBS.OMS.OMSEXPORT.OMSEXPORTC3E,OMSEXPORTC3E")
            {
                return "C3E";
            }
            else if (exportObj.ToUpper() == "FWBS.OMS.OMSEXPORT.OMSEXPORTENT,OMSEXPORTENT")
            {
                return "ENT";
            }
            else
                throw new Exception("No export application has been configured.");

        }

        /// <summary>
        /// Gets the 3 character name of the application we are exporting to
        /// </summary>
        /// <returns>name of app</returns>
        public static void SetExportAppName(string appName)
        {
            if (appName == "IGO")
            {
                StaticLibrary.UpdateSetting("ExportObject", "", "FWBS.OMS.OMSEXPORT.OMSExportIGO,OMSExportIGO");
            }
            else if (appName == "MIL")
            {
                StaticLibrary.UpdateSetting("ExportObject", "", "FWBS.OMS.OMSEXPORT.OMSExportMIL,OMSExportMIL");
            }
            else if (appName == "E3E")
            {
                StaticLibrary.UpdateSetting("ExportObject", "", "FWBS.OMS.OMSEXPORT.OMSExportE3E,OMSExportE3E");
            }
            else if (appName == "C3E")
            {
                StaticLibrary.UpdateSetting("ExportObject", "", "FWBS.OMS.OMSEXPORT.OMSExportC3E,OMSExportC3E");
            }
            else if (appName == "ENT")
            {
                StaticLibrary.UpdateSetting("ExportObject", "", "FWBS.OMS.OMSEXPORT.OMSExportENT,OMSExportENT");
            }
            else
                throw new Exception("Choose the Export Application you wish to Integrate with.");

        }

        /// <summary>
        /// Reads SMTP Settings from the Registry
        /// </summary>
        /// <returns>SMTP Settings</returns>
        public static SmtpSettings GetSmtpSettings()
        {
            SmtpSettings smtpSettings = new SmtpSettings();
            smtpSettings.Address = GetSetting("SmtpServer", "", "");
            smtpSettings.Encryption = GetSetting("SmtpEncryption", "", "");
            smtpSettings.Authenticate = GetBoolSetting("SmtpAuthenticate", "", false);
            smtpSettings.Login = GetSetting("SmtpLogin", "", "");
            smtpSettings.Password = GetSetting("SmtpPassword", "", "");
            return smtpSettings;
        }

		/// <summary>
		/// Updates registry setting
		/// </summary>
		/// <param name="setting">Settings name</param>
		/// <param name="app">application name</param>
		/// <param name="newValue">value of setting</param>
        public static void UpdateSetting(string valueName, string applicationName, object newValue)
		{
            try
            {
                string regKey = "";

                //string Determine the sub key;
                if (applicationName == "")
                    regKey = REG_KEY + @"\Settings";
                else
                    regKey = REG_KEY + @"\" + applicationName;

                Microsoft.Win32.Registry.SetValue(regKey, valueName, newValue);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Updating Registry (" + valueName + ")\n" + ex.Message, ex);
            }
		}

		/// <summary>
		/// Returns the name of the log file
		/// </summary>
		/// <returns></returns>
		public static string LogFileName
		{
			get
			{
				string dir = FWBS.OMS.OMSEXPORT.StaticLibrary.GetSetting("StatLogFolder","Settings",@"c:\temp");
                System.IO.Directory.CreateDirectory(dir);
				string filename = dir + @"\omsexport.log";
				//strip any double backslashes
				return filename.Replace(@"\\",@"\");
			}
		}

		
		#endregion
		
		#region Email Functions
		
		/// <summary>
		/// Sends an email to the administrator on error
		/// </summary>
		/// <param name="MailTo"></param>
		/// <param name="MailFrom"></param>
		/// <param name="MailSubject"></param>
		/// <param name="MailBody"></param>
		/// <param name="SMTPServer"></param>
        /// <param name="RaiseException">Flag to indicate errors shopuld be raised and not logged</param>
        public static void SendEmail(string MailTo, string MailFrom, string MailSubject, string MailBody, SmtpSettings SMTPServer, bool RaiseException = false)
        {
            try
            {
                string[] smtpAddress = SMTPServer.Address.Split(':');
                using (SmtpClient smtpClient = (smtpAddress.Length == 1) ? new SmtpClient(smtpAddress[0]) : new SmtpClient(smtpAddress[0], Convert.ToInt32(smtpAddress[1])))
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                    string smtpEncryption = (SMTPServer.Encryption ?? string.Empty).ToUpper();
                    if (!string.IsNullOrEmpty(smtpEncryption) && smtpEncryption != "NONE")
                    {
                        smtpClient.EnableSsl = true;
                        if (smtpEncryption == "STARTTLS")
                            smtpClient.TargetName = smtpEncryption + "/" + smtpClient.Host;
                    }

                    if (SMTPServer.Authenticate)
                    {
                        if (!string.IsNullOrEmpty(SMTPServer.Login))
                        {
                            string[] userNameParts = SMTPServer.Login.Split('\\');
                            if (userNameParts.Length == 1) // user@domain.com
                                smtpClient.Credentials = new System.Net.NetworkCredential(userNameParts[0], SMTPServer.Password);
                            else                           // domain.com\user
                                smtpClient.Credentials = new System.Net.NetworkCredential(userNameParts[1], SMTPServer.Password, userNameParts[0]);
                        }
                        else
                        {
                            smtpClient.UseDefaultCredentials = true;
                        }
                    }

                    using (MailMessage Message = new MailMessage(MailFrom.Replace(" ", ""), MailTo))
                    {
                        Message.Subject = MailSubject;
                        Message.Body = MailBody;

                        smtpClient.Send(Message);
                    }
                }
            }
            catch (Exception e)
            {
                if (RaiseException)
                {
                    throw;
                }
                else
                {
                    LogErrorMessage("Attempt to send email errored with the following error: " + e.Message);
                }
            }
        }
		
		
		#endregion

		#region Stats functions
		
		/// <summary>
		/// gets current values of all stats from ther log file
		/// </summary>
		/// <returns></returns>
		public static LogCounter GetCounter()
		{
			LogCounter retVal = new LogCounter();
			string strRead = "";

			try
			{
                if (!System.IO.File.Exists(LogFileName))
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(LogFileName))
                    {
                        sw.WriteLine("0,0,0,0,0,0,0,0,0,0");
                    }
                }


				//read existing values into a string variable
				using (System.IO.StreamReader sr = new System.IO.StreamReader(LogFileName))
				{
					strRead = sr.ReadToEnd();
				}

                //split existing values by the comma seperationg them
				string[] vals = strRead.Split(",".ToCharArray());
				
				retVal.ClientsAdded = Convert.ToInt64(vals[0]);
				retVal.ClientsUpdated = Convert.ToInt64(vals[1]);
				retVal.MattersAdded = Convert.ToInt64(vals[2]);
				retVal.MattersUpdated = Convert.ToInt64(vals[3]);
				retVal.TimeAdded = Convert.ToInt64(vals[4]);
				retVal.FinancialsAdded = Convert.ToInt64(vals[5]);
				retVal.Errors = Convert.ToInt64(vals[6]);
                retVal.UsersAdded = Convert.ToInt64(vals[7]);
                retVal.LookupsAdded = Convert.ToInt64(vals[8]);
				retVal.LastError = Convert.ToString(vals[9]);
			}
			catch(Exception ex)
			{
				FWBS.OMS.OMSEXPORT.StaticLibrary.LogErrorMessage("LogCounter: " + ex.Message);
			}
			
			return retVal;
			
		}
		
		/// <summary>
		/// Writes stats within the log counter to disk
		/// </summary>
		/// <param name="logCounter">Log counter of stats</param>
		/// <returns>true if successful</returns>
		public static LogCounter WriteStats(LogCounter logCounter)
		{
			string strFile;
			string strDir;
			
			try
			{
				//capture the log file name
				strFile = FWBS.OMS.OMSEXPORT.StaticLibrary.LogFileName;
				//capture the log directory
				strDir = System.IO.Path.GetDirectoryName(strFile); 
				//check if directory exsts and create it if it doesn't
				System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(strDir);
				if(!dir.Exists)
				{
					dir.Create();
				}

				//Capture existing values this is in case they have been reset since
				//each iteration of the process starts with 0 values
				LogCounter existing = GetCounter();
				existing.ClientsAdded += logCounter.ClientsAdded;
				existing.ClientsUpdated += logCounter.ClientsUpdated;
				existing.MattersAdded += logCounter.MattersAdded;
				existing.MattersUpdated += logCounter.MattersUpdated;
				existing.TimeAdded += logCounter.TimeAdded;
				existing.FinancialsAdded += logCounter.FinancialsAdded;
				existing.Errors += logCounter.Errors;
                existing.UsersAdded += logCounter.UsersAdded;
                existing.LookupsAdded += logCounter.LookupsAdded;
                if(logCounter.LastError != "")
					existing.LastError = logCounter.LastError;

				//write new values to disk
				using (System.IO.StreamWriter sw = new System.IO.StreamWriter(strFile))
				{
					sw.Write(existing.ToString());
				}
				//capture the last error value from existing Logcounter and return a new one of zero values
				LogCounter retval = new LogCounter();
				retval.LastError = existing.LastError;
				return retval;
			}
			catch(Exception ex)
			{
				//write it for diagnostics
				FWBS.OMS.OMSEXPORT.StaticLibrary.LogErrorMessage("OMS Export Base-WriteStats: " + ex.Message);
				//if there is an error return the logcounter that was passes no values are not rest
				return logCounter;
			}
		}
		
		/// <summary>
		/// Logs an entry in the event log
		/// </summary>
		/// <param name="message">message to write</param>
        /// <param name="error">true to log as Error, false to log as Warning</param>
		public static void LogErrorMessage(string message, bool error = true)
		{
            try
            {
                using (System.Diagnostics.EventLog evt = new System.Diagnostics.EventLog("OMSExport", ".", "FWBS OMS Export Service"))
                {
                    evt.WriteEntry(message, error ? System.Diagnostics.EventLogEntryType.Error : System.Diagnostics.EventLogEntryType.Warning);
                }
            }
            catch { }
		}

		#endregion

        #region Logging Functions

        /// <summary>
        /// Logs errors to new database table
        /// </summary>
        public static void LogToDatabase(System.Data.SqlClient.SqlConnection cnn,string error,
            string entity, string id,string action)
        {
            try
            {

                string command = "insert fdExportServiceLog(Application,Entity,Errormessage,RecordID,Action)"
                 + " values(@Application,@Entity,@Errormessage,@RecordID,@Action)";

                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(command, cnn);

                string app = GetExportAppName();

                cmd.Parameters.Add(new SqlParameter("@Application", app));
                cmd.Parameters.Add(new SqlParameter("@Entity", entity));
                cmd.Parameters.Add(new SqlParameter("@Errormessage", error));
                cmd.Parameters.Add(new SqlParameter("@RecordID", id));
                cmd.Parameters.Add(new SqlParameter("@Action", action));

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                LogErrorMessage("Error Logging to OMS Database " + ex.Message);
            }
        }

        /// <summary>
        /// Gets a connection string to connect to OMS based on registry settings
        /// </summary>
        /// <returns></returns>
        public static string OMSConnectionString()
        {
            string SQLDatabaseName = GetSetting("OMSSQLDatabase", "", "");
            string SQLServerName = GetSetting("OMSSQLServer", "", "");
            string LoginType = GetSetting("OMSLoginType", "", "");
            string username = GetSetting("OMSSQLUID", "", "");
            string password = GetSetting("OMSSQLPWD", "", "");

            if (SQLDatabaseName == "" || SQLServerName == "" || LoginType == "")
                throw new Exception("Database setting are not configured");

            string strCnn = "Server=" + SQLServerName + ";Database=" + SQLDatabaseName;
            if (LoginType == "SQL")
            {
                strCnn += ";User Id=" + username + ";Password=" + password + ";Trusted_Connection=False";
            }
            else
            {
                strCnn += (LoginType == "AAD") ? ";Authentication=Active Directory Integrated" : ";Integrated Security=SSPI";
                strCnn += ";Persist Security Info = False";
            }

            return strCnn;
        }

        public static string IGOConnectionString()
        {
            string strCnn = "";

            string SQLDatabaseName = GetSetting("IGODatabase", "IGO", "");
            string SQLServerName = GetSetting("IGOServer", "IGO", "");
            string LoginType = GetSetting("IGOLoginType", "IGO", "");
            string username = GetSetting("IGOSQLUID", "IGO", "");
            string password = GetSetting("IGOSQLPWD", "IGO", "");

            if (SQLDatabaseName == "" || SQLServerName == "" || LoginType == "")
                throw new Exception("Database setting are not configured");

            if (LoginType == "SQL")
            {
                strCnn = "Server=" + SQLServerName + ";Database=" + SQLDatabaseName + ";User Id=" + username + ";Password=" + password + ";Trusted_Connection=False";
            }
            else
            {
                strCnn = "Persist Security Info=False;Integrated Security=SSPI;database=" + SQLDatabaseName + ";server=" + SQLServerName;
            }

            return strCnn;
        }

        /// <summary>
        /// Gets a Miles 33 connection string to connect to OMS based on registry settings
        /// </summary>
        /// <returns></returns>
        public static string MILConnectionString()
        {
            string strCnn = "";

            string SQLDatabaseName = GetSetting("MILDatabase", "MIL", "");
            string SQLServerName = GetSetting("MILServer", "MIL", "");    
            string LoginType = GetSetting("MILLoginType", "MIL", "");     
            string username = GetSetting("MILSQLUID", "MIL", "");         
            string password = GetSetting("MILSQLPWD", "MIL", "");         

            if (SQLDatabaseName == "" || SQLServerName == "" || LoginType == "")
            {
                throw new Exception("Database setting are not configured");
            }

            if (LoginType == "SQL")
            {
                strCnn = "Server=" + SQLServerName + ";Database=" + SQLDatabaseName + ";User Id=" + username + ";Password=" + password + ";Trusted_Connection=False";
            }
            else
            {
                strCnn = "Persist Security Info=False;Integrated Security=SSPI;database=" + SQLDatabaseName + ";server=" + SQLServerName;
            }

            return strCnn;
        }



        #endregion

    }
}
