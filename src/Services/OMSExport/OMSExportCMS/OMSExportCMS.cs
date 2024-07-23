using System;
using System.Data;
using System.Diagnostics;

namespace FWBS.OMS.OMSEXPORT
{
	/// <summary>
	/// Child class to export to Aderant.
	/// </summary>
	public class OMSExportCMS : OMSExportBase , IDisposable	
	{
		//constructor calls base contstructor
		public OMSExportCMS():base()
		{}
		
		#region Fields
		/// <summary>
		/// Required for logon
		/// </summary>
		private AuthenticationService.AuthenticationService _authSvs;
		/// <summary>
		/// Client web service
		/// </summary>
		private ClientService.ClientService _clientSvs;
		/// <summary>
		/// Matter web service
		/// </summary>
		private MatterService.MatterService _matterSvs;
		/// <summary>
		/// Personnel web service
		/// </summary>
		private PersonnelService.PersonnelService _personnelSvs;
		/// <summary>
		/// Name(contact) Web Service
		/// </summary>
		private NameService.NameService _nameSvs;
		/// <summary>
		/// Address web service
		/// </summary>
		private AddressService.AddressService _addressSvs;
		/// <summary>
		/// Bill group service required for matter creation
		/// </summary>
		private BillGroupService.BillGroupService _billgroupSvs;
		/// <summary>
		/// Time entry web service
		/// </summary>
		private TimeEntryService.TimeEntryService _timeSvs;

		private ClientContact.ClientContactService _cliContSvs;
		/// <summary>
		/// Cookie container is passed with each call
		/// </summary>
		private System.Net.CookieContainer _ccJar;
		/// <summary>
		/// Used when calling registry read function 
		/// </summary>
		private const string APPNAME = "CMS";
		/// <summary>
		/// Monitor if we are logged into CMS
		/// </summary>
		private bool _loggedIn = false;
		/// <summary>
		/// Flag to indicate if integrated security is being used
		/// </summary>
		private bool _integratedSecurity = false;
        /// <summary>
        /// Flag to indicate if the time values should be converted to local time.
        /// </summary>
        private bool _convertToLocalTime = false;

		
		#endregion

		#region CMS Specific functions		
			
		/// <summary>
		/// Gets around the bug where the bill group record is created as not default
		/// </summary>
		/// <param name="cliUNO">Newly Inserted Clinet UNO</param>
		/// <param name="code">Group Code </param>
		private void SetDefaultBillGroup(int cliUNO, string code,int addrUNO, int icliContact, 
				int iblEmplUno, int reminderDays,int ipbFormatUno,int iblFormatUno, int irsFormatUno,
				string jurisdicCode, int colEmplUno)
		{
			try
			{

				string filter = "(BillGroup.Client.Uno is " + Convert.ToString(cliUNO) +
					") AND (BillGroup.Code is " + code + ")";
			
				BillGroupService.BillGroupData[] bills = _billgroupSvs.Read(filter);
			
				//can only return 1 record
				BillGroupService.BillGroupData bill = bills[0];
			
				//grab the uno we'll need it later
				int uno = bill.BillGrpUno;

				//get data update object
				BillGroupService.BillGroupDataUpdate data = _billgroupSvs.GetDataUpdate(bill);
			
				//update default flag
				data.IsDefault = "Y";
				data.AddressUno = addrUNO;
				//added 22/3/2005 when organisation client contact is not created
				if(icliContact != 0)
				{
					data.ColContactUno = icliContact;
					data.ContactUno = icliContact;
				}

				data.BlEmplUno = iblEmplUno;
				data.ColAddressUno = addrUNO;
				data.CollectionsOk = "Y";

				//Collections employee uno changed 14/3/2005 
				data.ColTkEmplUno = colEmplUno;
				data.ReminderDays = reminderDays;
				data.JurisdicCode = jurisdicCode;
				
				data.BlFormatUno = iblFormatUno;
								
				//send change back to web service
                
				_billgroupSvs.Update(uno,data);
			}
			catch(Exception ex)
			{
				string s = ex.Message;
				Exception newex = new Exception("SetDefaultBillGroup:" + ex.Message);
				throw newex;
			}
		}
		
		
		/// <summary>
		/// Joins client and name together
		/// </summary>
		/// <param name="iClientUno"></param>
		/// <param name="iNameUno"></param>
		private int SetClientContact(int iClientUno,int iNameUno,string typeCode)
		{
			try
			{
				ClientContact.ClientContactDataCreate oData = _cliContSvs.InitializeDataCreate();
			
				oData.ClientUno = iClientUno;
				oData.NameUno = iNameUno;
				oData.Inactive = "N";
				oData.ClientContact = "Y"; //
				oData.ContTypeCode = typeCode;
			
					
				int icliContact = _cliContSvs.Create(oData);

				return icliContact;
			}
			catch(Exception ex)
			{
				string s = ex.Message;
				Exception newex = new Exception("SetClientContact:" + ex.Message);
				throw newex;
			}
		}
		
		/// <summary>
		/// Attempts to find the address if it cannot find it then creates a new one 
		/// </summary>
		/// <param name="row">data Row</param>
		/// <returns>ID of address</returns>
		private int GetAddressUno(DataRow row,int iUNO)
		{
			string filter;
						
			try
			{
				//first try to find
				filter = "(Address.Type.Code is " + Convert.ToString(row["AddrTypeCode"]) +
					") AND (Address.Name.Uno is "  + Convert.ToString(iUNO) + ")";
				
				AddressService.AddressData[] addData = _addressSvs.Read(filter);
				if(addData != null)
				{
                    int retval = addData[0].AddressUno;
					return retval;
				}
			}
			catch(Exception ex)
			{
				string s = ex.Message;
			}
			
			try
			{
				//if we get this far then we are unable to find this address
				//create the address
				AddressService.AddressDataCreate oAddressDC = new AddressService.AddressDataCreate();
				//this is important as it ties the address to the client
				oAddressDC.NameUno = iUNO;
				oAddressDC.Address1 = Convert.ToString(row["addline1"]);
				oAddressDC.Address2= Convert.ToString(row["addline2"]);
				oAddressDC.Address3= Convert.ToString(row["addline3"]);
				oAddressDC.Address4= Convert.ToString(row["addline4"]);
				oAddressDC.City = Convert.ToString(row["addline5"]);
				oAddressDC.PostCode = Convert.ToString(row["addpostcode"]);
				oAddressDC.CountryCode = Convert.ToString(row["ctryid"]);
				oAddressDC.Inactive = "N";
				oAddressDC.AddrTypeCode = Convert.ToString(row["AddrTypeCode"]); // MAIN, REMIT, MKTG, TA
				//for anticipated UTC support
                if(_convertToLocalTime)
                    oAddressDC.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"]).ToLocalTime();
                else
                    oAddressDC.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"]);

				oAddressDC.PhoneNumber = Convert.ToString(row["PhoneNumber"]);
				oAddressDC.FaxNumber = Convert.ToString(row["FaxNumber"]);

                // added 16/6/2011 for KN
                if (row.Table.Columns["DefaultAddress"] != null)
                    oAddressDC.DefaultAddress = Convert.ToBoolean(row["DefaultAddress"]);
                
                //added 15/10/2012 for Adrian Oz customer
                if (row.Table.Columns["StateCode"] != null)
                    oAddressDC.StateCode = Convert.ToString(row["StateCode"]);


				int val = _addressSvs.Create(oAddressDC);

				return val;
			}
			catch(Exception ex)
			{
				string s = "GetAddressUno: " + ex.Message;
				Exception enew = new Exception(s);
				throw enew;
			}
		}
				
		/// <summary>
		/// Logs into CMS and sets cookie containers on all CMS objects
		/// </summary>
		private void LoginToCMS()
		{
			RaiseStatusEvent("Logging into CMS system.");
            string baseURL = "??";

            try
            {

                _authSvs = new AuthenticationService.AuthenticationService();
                _clientSvs = new ClientService.ClientService();
                _matterSvs = new MatterService.MatterService();
                _personnelSvs = new PersonnelService.PersonnelService();
                _nameSvs = new NameService.NameService();
                _addressSvs = new AddressService.AddressService();
                _billgroupSvs = new BillGroupService.BillGroupService();
                _timeSvs = new TimeEntryService.TimeEntryService();
                _cliContSvs = new ClientContact.ClientContactService();

                RaiseStatusEvent("Instantiated CMS services ");
                
                _ccJar = new System.Net.CookieContainer();

                RaiseStatusEvent("Created Cookie jar");

                Debug.WriteLine("Cookie Container created", "OMSEXPORT");


                // LoginOptions contains authentication type
                AuthenticationService.LoginOptions loOpts = new AuthenticationService.LoginOptions();

                Debug.WriteLine("Login Options done", "OMSEXPORT");

                //get username and password to login to CMS
                string CMSLogin = StaticLibrary.GetSetting("CMSLogin", APPNAME, "");
                string CMSPassword = StaticLibrary.GetSetting("CMSPassword", APPNAME, "");
                baseURL = StaticLibrary.GetSetting("BaseURL", APPNAME, "");
                _integratedSecurity = StaticLibrary.GetBoolSetting("CMSIntegrated", APPNAME, false); 


                Debug.WriteLine("Got Settings, base URL = " + baseURL, "OMSEXPORT");

                //if any values are missing throw an exception
                if (CMSLogin == "" || CMSPassword == "" || baseURL == "")
                    throw new ApplicationException("CMS settings not configured.  Please check your settings.");

                Debug.WriteLine("Passed basic validation", "OMSEXPORT");


                //if it is set up with a trailing backslash then strip off
                if (baseURL.LastIndexOf(@"/") == baseURL.Length - 1)
                    baseURL = baseURL.Remove(baseURL.Length - 1, 1);

                //set the urls's of all the web services
                _addressSvs.Url = baseURL + @"/FileOpening/FileOpeningWS/AddressService.asmx";
                _authSvs.Url = baseURL + @"/Tools/ToolsWS/AuthenticationService.asmx";
                _clientSvs.Url = baseURL + @"/FileOpening/FileOpeningWS/ClientService.asmx";
                _matterSvs.Url = baseURL + @"/FileOpening/FileOpeningWS/MatterService.asmx";
                _nameSvs.Url = baseURL + @"/FileOpening/FileOpeningWS/NameService.asmx";
                _personnelSvs.Url = baseURL + @"/FileOpening/FileOpeningWS/PersonnelService.asmx";
                _billgroupSvs.Url = baseURL + @"/FileOpening/FileOpeningWS/BillGroupService.asmx";
                _timeSvs.Url = baseURL + @"/Time/TimeWS/TimeEntryService.asmx";
                _cliContSvs.Url = baseURL + @"/FileOpening/FileOpeningWS/ClientContactService.asmx";

                Debug.WriteLine("URL's set", "OMSEXPORT");
                
                _authSvs.CookieContainer = _ccJar;

                Debug.WriteLine("Cookie jar set", "OMSEXPORT");

                // added DMB 7/Mar/2005 they have started using integrated security so need to do this
                if (_integratedSecurity)
                {

                    Debug.WriteLine("Login authenticated", "OMSEXPORT");

                    _authSvs.Credentials = System.Net.CredentialCache.DefaultCredentials;
                    //Added GM 08/Mar/2005 
                    try
                    {
                        loOpts.LoginAuthority = AuthenticationService.LoginAuthorityType.Windows;
                        _authSvs.Login(CMSLogin, CMSPassword, loOpts);
                    }
                    catch { }
                }
                else // for now dont specify login opts as I never used to don't know what default is
                {
                    _authSvs.Login(CMSLogin, CMSPassword, loOpts);
                }


                Debug.WriteLine("testing auth service", "OMSEXPORT");
                //Test to see if we are logged in properly
                string test = _authSvs.WhoAmI();
                
                if (!_integratedSecurity)
                {
                    if (test.ToUpper() != CMSLogin.ToUpper()) //not logged in properly
                        throw new Exception("Not logged in with username '" + CMSLogin + "' and password '" + CMSPassword + "' CMS Returned " + test);
                }
                else
                {
                    //Need to find if it's possible to check logged in when using authenticated login
                }

                //always pass cookie container from autentication service
                _addressSvs.CookieContainer = _ccJar;
                _clientSvs.CookieContainer = _ccJar;
                _matterSvs.CookieContainer = _ccJar;
                _nameSvs.CookieContainer = _ccJar;
                _personnelSvs.CookieContainer = _ccJar;
                _billgroupSvs.CookieContainer = _ccJar;
                _timeSvs.CookieContainer = _ccJar;
                _cliContSvs.CookieContainer = _ccJar;

                //if using integrated security then pass credentials to all other services
                if (_integratedSecurity)
                {
                    _addressSvs.Credentials = _authSvs.Credentials;
                    _clientSvs.Credentials = _authSvs.Credentials;
                    _matterSvs.Credentials = _authSvs.Credentials;
                    _nameSvs.Credentials = _authSvs.Credentials;
                    _personnelSvs.Credentials = _authSvs.Credentials;
                    _billgroupSvs.Credentials = _authSvs.Credentials;
                    _timeSvs.Credentials = _authSvs.Credentials;
                    _cliContSvs.Credentials = _authSvs.Credentials;
                }

                RaiseStatusEvent("Logged into CMS system.");
                _loggedIn = true;
            }
            catch (Exception ex)
            {
                base.LogEntry(ex.Message + Environment.NewLine + ex.StackTrace, System.Diagnostics.EventLogEntryType.Error);
                
                throw new ApplicationException("Error Logging in to CMS: " + baseURL + ex.Message, ex);
                
            }
		}
		
		
		/// <summary>
		/// Logs out of the CMS system, use at the end of a run
		/// </summary>
		private void LogoutOfCMS()
		{
			try
			{
				
				if(_loggedIn)
				{
					if(_authSvs.LoggedIn())
						_authSvs.Logout();
				}
				//null all web service variables
				_addressSvs = null;
				_authSvs = null;
				_clientSvs = null;
				_matterSvs = null;
				_nameSvs = null;
				_personnelSvs = null;
				_authSvs = null;
				_timeSvs = null;
				_cliContSvs = null;
				_ccJar = null;
			
				//finally set flag
				_loggedIn = false;
			}
			catch(Exception ex)
			{
				base.LogEntry("LogoutOfCMS: " + ex.Message,System.Diagnostics.EventLogEntryType.Error);
			}
		}
		
		
				
		#endregion
	
		#region OMSExportBase Members

		//#############################################################################
		//NEEDS MOVING TO BASE CLASS SO IT CAN BE SHARED
		/// <summary>
		/// Implement a old VB6 style left function
		/// </summary>
		/// <param name="host">string to trim</param>
		/// <param name="index">number of characters</param>
		/// <returns>trimmed string</returns>
		public static string Left(string host, int length)
		{
			if(host.Length < length)
				return host;
			else
				return host.Substring(0,length);
		}
		//##########################################################################
		
		
		protected override string ExportObject
		{
			get
			{
				return APPNAME;
			}
		}
		
		
		/// <summary>
		/// Nothing to implement here for this onject
		/// Most is the first time any work needs doing
		/// </summary>
		protected override void InitialiseProcess()
		{
            //set the convert to local time flag
            _convertToLocalTime = StaticLibrary.GetBoolSetting("ConvertToLocalTime", "CMS", false);
        }
		
		/// <summary>
		/// performa any finishing off
		/// </summary>
		protected override void FinaliseProcess()
		{
			try
			{
				//Log out of CMS
				RaiseStatusEvent("Logging out of CMS");
				LogoutOfCMS();
			}
			catch{}
		}

        /// <summary>
        /// Not Needed for CMS yet
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
		/// <param name="row">Data row from client query</param>
		/// <returns>the UNO of the inserted client record</returns>
		protected override object ExportClient(System.Data.DataRow row)
		{
			//check if logged in and then login if not
			if(! _loggedIn)
				LoginToCMS();

			//Get data create objects
			NameService.NameDataCreate oNameCreate = _nameSvs.InitializeDataCreate();
			ClientService.ClientDataCreate oCDC = _clientSvs.InitializeDataCreate();
                        						
			try
			{
				//create name record varying if a person 
				oNameCreate.Name = Convert.ToString(row["clName"]);
				//Added 22/3/2005 to get around a bug in the API, CMS issue number 759897
				oNameCreate.NameSort = Left(Convert.ToString(row["clName"]),30);
				oNameCreate.Inactive = Convert.ToString(row["Inactive"]);
				oNameCreate.InternetAddr = Convert.ToString(row["InternetAddr"]);
				oNameCreate.NameType = Convert.ToString(row["NameType"]);
				if(oNameCreate.NameType == "P")//only populate these fields if it is a person
				{
					oNameCreate.FirstName = Convert.ToString(row["contChristianNames"]); //this is required
					oNameCreate.LastName = Convert.ToString(row["contSurname"]); //this is required
					//if firstname and lastname are empty populate one of them or it will error.
                    if(oNameCreate.LastName ==" " && oNameCreate.FirstName == " ")
						oNameCreate.LastName = "Not Given";
					
					oNameCreate.Title = Convert.ToString(row["contTitle"]);
									
					if(row["contMaritalStatus"] != DBNull.Value)
						oNameCreate.MaritalStatus = Convert.ToString(row["contMaritalStatus"]); //Need to get codes in sync
                    if (row["contDOB"] != DBNull.Value)
                    {
                        if (_convertToLocalTime)
                            oNameCreate.BirthDate = Convert.ToDateTime(row["contDOB"]).ToLocalTime();
                        else
                            oNameCreate.BirthDate = Convert.ToDateTime(row["contDOB"]);
                    }

					if(row["contSex"] != DBNull.Value)
						oNameCreate.Gender = Convert.ToString(row["contSEX"]);
                  					
				}
				

				//call web service to create the name
				int iNameUno = _nameSvs.Create(oNameCreate);
				oCDC.NameUno = iNameUno;
				
			
				//create the address record
				int iAddrUNO = GetAddressUno(row,iNameUno);
				oCDC.AddressUno = iAddrUNO;
			
				//Create client record
				//No property to track our clID CMS ClientNumber field available but not exposed
				oCDC.ClientName = Convert.ToString(row["clName"]);
				oCDC.ClientCode = Convert.ToString(row["clNo"]).Trim();
				oCDC.Offc = Convert.ToString(row["brCode"]).Trim();
				oCDC.Dept = Convert.ToString(row["Dept"]);
				oCDC.Prof = Convert.ToString(row["Prof"]);
				oCDC.StatusCode = Convert.ToString(row["StatusCode"]);
				if(row["RespEmplUno"] != DBNull.Value)
					oCDC.RespEmplUno = Convert.ToInt32(row["RespEmplUno"]); 
				if(row["BillEmplUno"] != DBNull.Value)
					oCDC.BillEmplUno = Convert.ToInt32(row["BillEmplUno"]);
				oCDC.TaskSetCode = Convert.ToString(row["TaskSetCode"]); //VALUES FROM PML_SET IN DATABASE awaiting response from MC Aderant   
				if(row["OpenEmplUno"] != DBNull.Value)
					oCDC.OpenEmplUno = Convert.ToInt32(row["OpenEmplUno"]);

				oCDC.ClntCatCode = Convert.ToString(row["clGroup"]);
				oCDC.ClntClassCode = Convert.ToString(row["clSource"]);
				oCDC.ClntTypeCode = Convert.ToString(row["clTypeCode"]);
				
				//added DMB 14/3/2005 extra fields requested by Nick Kates
				if(row["RateLevel"] != DBNull.Value)
					oCDC.RateLevel = Convert.ToInt32(row["RateLevel"]);
				oCDC.TimeInc = Convert.ToString(row["TimeInc"]);

                if (row.Table.Columns["TIMECLASS"] != null)
                    oCDC.TimeClass = Convert.ToString(row["TIMECLASS"]);
                if (row.Table.Columns["TIMETYPE"] != null)
                    oCDC.TimeTypeCode = Convert.ToString(row["TIMETYPE"]);


                //payor support 14/12/2011
                if (row.Table.Columns["EntityType"] != null)
                    oCDC.EntityType = Convert.ToString(row["EntityType"]);
               
				//call web service to create client
				int iClientUno = _clientSvs.Create(oCDC);
				
				//added 22/3/2005 as organisations would fail 
				int iCliContUno;
				if(oNameCreate.NameType == "P")
					//assigns the contact to the client Name Uno record must be a person
					iCliContUno = SetClientContact(iClientUno,iNameUno,Convert.ToString(row["clContTypeCode"]));
				else
					iCliContUno = 0;

				//Need to find the Bill group record that is created with a client and set the IS_DEFAULT flag to 'Y'
				//this is down to a bug that creates the record as not default
				SetDefaultBillGroup(iClientUno,"MAIN",iAddrUNO,iCliContUno,Convert.ToInt32(row["BillEmplUno"])
					,Convert.ToInt32(row["ReminderDays"]),Convert.ToInt32(row["pbFormatUNO"]),Convert.ToInt32(row["BlFormatUno"]),
					Convert.ToInt32(row["RsFormatUno"]),Convert.ToString(row["jurisdicCode"]),Convert.ToInt32(row["ColEmplUno"]));

				return iClientUno;
			}
			catch(Exception ex)
			{
				string s = "ExportClient: " + ex.Message;
				Exception enew = new Exception(s);
				throw enew;
			}
			
		}
		
				
		/// <summary>
		/// Updates the individual client record
		/// </summary>
		/// <param name="row">clinet data row</param>
		/// <returns>true if successful</returns>
		protected override bool UpdateClient(DataRow row)
		{
			// Update class inherited from ClientDataBase performs update wrappers up all the client data
			ClientService.ClientDataUpdate clntdataup; 
			ClientService.ClientData clnt;
						
			//check if logged in and then login if not
			if(! _loggedIn)
				LoginToCMS();
			
			int iUno = Convert.ToInt32(row["clextid"]);
			clnt = _clientSvs.ReadSingle(iUno);
			
			// returns a Data Update object with more methods than a create object
			clntdataup = _clientSvs.GetDataUpdate(clnt);
			
			if (clntdataup != null)
			{
				try
				{
					if (row["clNo"] != DBNull.Value)
                        clntdataup.ClientCode = Convert.ToString(row["clNo"]).Trim();
                    if (row["clName"] != DBNull.Value)
                        clntdataup.ClientName = Convert.ToString(row["clName"]);
                    if (row["brCode"] != DBNull.Value)
                        clntdataup.Offc = Convert.ToString(row["brCode"]);
                    if (row["Dept"] != DBNull.Value)
                        clntdataup.Dept = Convert.ToString(row["Dept"]);
                    if (row["Prof"] != DBNull.Value)
                    clntdataup.Prof = Convert.ToString(row["Prof"]);
					if(row["RespEmplUno"] != DBNull.Value)
						clntdataup.RespEmplUno = Convert.ToInt32(row["RespEmplUno"]); 
					if(row["BillEmplUno"] != DBNull.Value)
						clntdataup.BillEmplUno = Convert.ToInt32(row["BillEmplUno"]);
                    if (row["TaskSetCode"] != DBNull.Value)
                        clntdataup.TaskSetCode = Convert.ToString(row["TaskSetCode"]);
                    if (row["clGroup"] != DBNull.Value)
                        clntdataup.ClntCatCode = Convert.ToString(row["clGroup"]);
                    if (row["clSource"] != DBNull.Value)
                        clntdataup.ClntClassCode = Convert.ToString(row["clSource"]);
                    if (row["clTypeCode"] != DBNull.Value)
                        clntdataup.ClntTypeCode = Convert.ToString(row["clTypeCode"]);

                    //#####Added to support 7.5 sp1 of the services
                    if (row.Table.Columns["RoundUp"] != null)
                        clntdataup.RoundType = Convert.ToString(row["RoundUp"]);
                    else
                        clntdataup.RoundType = "S";
                    //#####
                    //added for work item 301
                    if (row.Table.Columns["STATUS"] != null)
                        clntdataup.StatusCode = Convert.ToString(row["STATUS"]);
                    if (row.Table.Columns["TIMECLASS"] != null)
                        clntdataup.TimeClass = Convert.ToString(row["TIMECLASS"]);
                    if (row.Table.Columns["TIMETYPE"] != null)
                        clntdataup.TimeTypeCode = Convert.ToString(row["TIMETYPE"]);                    

					// need to pass the uno and the DataUpdate object
					_clientSvs.Update(iUno,clntdataup);

					//update any name record changes
					NameService.NameData name = _nameSvs.ReadSingle(clntdataup.NameUno);
					NameService.NameDataUpdate nameupd = _nameSvs.GetDataUpdate(name);

                    if (row["clName"] != DBNull.Value)
					    nameupd.Name = Convert.ToString(row["clName"]);
                    if (row["Inactive"] != DBNull.Value)
                        nameupd.Inactive = Convert.ToString(row["Inactive"]);
                    if (row["InternetAddr"] != DBNull.Value)
                        nameupd.InternetAddr = Convert.ToString(row["InternetAddr"]);
					if(Convert.ToString(row["NameType"]) == "P")//only populate these fields if it is a person
					{
                        if (row["contChristianNames"] != DBNull.Value)
                            nameupd.FirstName = Convert.ToString(row["contChristianNames"]); //this is required
                        if (row["contSurname"] != DBNull.Value)
                            nameupd.LastName = Convert.ToString(row["contSurname"]); //this is required
						//if firstname and lastname are empty populate one of them or it will error.
						if(nameupd.LastName ==" " && nameupd.FirstName == " ")
							nameupd.LastName = "Not Given";

                        if (row["contTitle"] != DBNull.Value)
						    nameupd.Title = Convert.ToString(row["contTitle"]);
									
						if(row["contMaritalStatus"] != DBNull.Value)
							nameupd.MaritalStatus = Convert.ToString(row["contMaritalStatus"]); //Need to get codes in sync

                        if (row["contDOB"] != DBNull.Value)
                        {
                            if (_convertToLocalTime)
                                nameupd.BirthDate = Convert.ToDateTime(row["contDOB"]).ToLocalTime();
                            else
                                nameupd.BirthDate = Convert.ToDateTime(row["contDOB"]);
                        }
                        
						if(row["contSex"] != DBNull.Value)
							nameupd.Gender = Convert.ToString(row["contSEX"]);

                        
					}
					//update the name
					_nameSvs.Update(clntdataup.NameUno,nameupd);
					
					//Check that the address has not changed
					int iAddUno = clntdataup.AddressUno;
					AddressService.AddressData addData = _addressSvs.ReadSingle(iAddUno);

                    //added 29/10/2007 Addresses are not enforced
                    if (addData != null)
                    {
                        //check all fields are the same
                        if (addData.Address1 != Convert.ToString(row["addline1"]) || addData.Address2 != Convert.ToString(row["addline2"]) ||
                            addData.Address3 != Convert.ToString(row["addline3"]) || addData.Address4 != Convert.ToString(row["addline4"]) ||
                            addData.City != Convert.ToString(row["addline5"]) || addData.PostCode != Convert.ToString(row["addpostcode"]) ||
                            addData.CountryCode != Convert.ToString(row["ctryid"]) || addData.AddrTypeCode != Convert.ToString(row["AddrTypeCode"]))
                        {
                            //If any are different update
                            AddressService.AddressDataUpdate addUpd = _addressSvs.GetDataUpdate(addData);

                            //update all fields
                            if (row["addline1"] != DBNull.Value)
                                addUpd.Address1 = Convert.ToString(row["addline1"]);
                            if (row["addline2"] != DBNull.Value)
                                addUpd.Address2 = Convert.ToString(row["addline2"]);
                            if (row["addline3"] != DBNull.Value)
                                addUpd.Address3 = Convert.ToString(row["addline3"]);
                            if (row["addline4"] != DBNull.Value)
                                addUpd.Address4 = Convert.ToString(row["addline4"]);
                            if (row["addline5"] != DBNull.Value)
                                addUpd.City = Convert.ToString(row["addline5"]);
                            if (row["addpostcode"] != DBNull.Value)
                                addUpd.PostCode = Convert.ToString(row["addpostcode"]);
                            if (row["ctryid"] != DBNull.Value)
                                addUpd.CountryCode = Convert.ToString(row["ctryid"]);
                            if (row["AddrTypeCode"] != DBNull.Value)
                                addUpd.AddrTypeCode = Convert.ToString(row["AddrTypeCode"]); // MAIN, REMIT, MKTG, TA
                            if (row["EffectiveDate"] != DBNull.Value)
                            {
                                if (_convertToLocalTime)
                                    addUpd.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"]).ToLocalTime();
                                else
                                    addUpd.EffectiveDate = Convert.ToDateTime(row["EffectiveDate"]);
                            }

                            // added 16/6/2011 for KN
                            if (row.Table.Columns["DefaultAddress"] != null)
                                addUpd.DefaultAddress = Convert.ToBoolean(row["DefaultAddress"]);

                            //added for A Wheeler 15/10/2012 for Oz customer
                            if (row.Table.Columns["StateCode"] != null)
                                addUpd.StateCode = Convert.ToString(row["StateCode"]);
                
                            //update address record
                            _addressSvs.Update(iAddUno, addUpd);
                        }
                    }
					
					return true;

				}
				catch(Exception ex)
				{
					throw new Exception("UpdateClient failed for client ID " + Convert.ToString(row["clID"]) + 
						" with CMS UNO of " + Convert.ToString(clnt) + " Error: " + ex.Message);
				}
			}
			else
			{
				base.LogEntry("No CMS client found with Uno of " + Convert.ToString(clnt),EventLogEntryType.Warning);
				return false;
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
		
		/// <summary>
		/// Inserts a new matter record into CMS system
		/// </summary>
		/// <param name="row">Data row of matter</param>
		/// <returns>CMS UNO of new record</returns>
		protected override object ExportMatter(DataRow row)
		{
			//check if logged in and then login if not
			if(! _loggedIn)
				LoginToCMS();

			MatterService.MatterDataCreate oData = _matterSvs.InitializeDataCreate(); 
                        
			try
			{
				//populate matter data
				oData.MatterCode = Convert.ToString(row["fileNo"]).Trim();
				oData.ClientUno = Convert.ToInt32(row["clextID"]);
                if (row["OpenDate"] != DBNull.Value)
                {
                    if(_convertToLocalTime)
                        oData.OpenDate = Convert.ToDateTime(row["OpenDate"]).ToLocalTime();
                    else
                        oData.OpenDate = Convert.ToDateTime(row["OpenDate"]);
                }
				oData.MatterName = Convert.ToString(row["MatterName"]);
				oData.LongMattName = Convert.ToString(row["fileDesc"]);
				oData.DescText = Convert.ToString(row["fileExternalNotes"]);
				oData.StatusCode = Convert.ToString(row["fileStatus"]).Trim();
				oData.MattTypeCode = Convert.ToString(row["FileType"]).Trim();
				// no properties for AssignEmplUno & BillEmpUno & respEmplUno & 

				//Additional fields from list
				if(row["fileEstimate"] != DBNull.Value)
					oData.AmtArrange = Convert.ToDecimal(row["fileEstimate"]);

				oData.ClaimNumber = Convert.ToString(row["fileAccCode"]).Trim();

                //set ClientMatName if it exists added31/1/2013
                if (row.Table.Columns["ClientMatName"] != null)
                    oData.ClientMatName = Convert.ToString(row["ClientMatName"]);
                else
                    oData.ClientMatName = Convert.ToString(row["fileDesc"]);

				oData.EditDate = DateTime.Now;
				
				oData.MattClassCode = Convert.ToString(row["fileSource"]).Trim();
				if(row["OpenEmplUno"] != DBNull.Value)
					oData.OpenEmplUno = Convert.ToInt32(row["OpenEmplUno"]); 
												
				// Standard stuff with no defaults
				oData.FeeBillFormat = Convert.ToString(row["FeeBillFormat"]); // 1,2,3
				oData.Contingency = Convert.ToString(row["Contingency"]);
				oData.ProrateTime = Convert.ToString(row["ProrateTime"]); //Y
				oData.GenDisbFlag = Convert.ToString(row["GenDisbFlag"]);  // Y, N
				oData.AllowTime = Convert.ToString(row["AllowTime"]);  
				//added 15/3/2005 default is N, Y is required
				oData.AllowDisb = Convert.ToString(row["AllowDisp"]);
				
				//Added 22/3/2005 request from Nick Kates
				oData.AllowBill = Convert.ToString(row["AllowBill"]);
				// added by DMB just in case this is also required
				oData.AllowInterest = Convert.ToString(row["AllowInterest"]);

                
				//Business Logic Component Owns & manages matter in this case  
				int iMatterUno = _matterSvs.Create(oData);
				
				//We now have to retrive the matter again to set these properties that are not available 
				MatterService.MatterData data = _matterSvs.ReadSingle(iMatterUno);
				MatterService.MatterDataUpdate upd = _matterSvs.GetDataUpdate(data);

				if(row["AssignEmplUno"] != DBNull.Value)
					upd.AssignEmplUno = Convert.ToInt32(row["AssignEmplUno"]);
				if(row["RespEmplUno"] != DBNull.Value)
					upd.RespEmplUno = Convert.ToInt32(row["RespEmplUno"]);
				if(row["BillEmplUno"] != DBNull.Value)
					upd.BillEmplUno = Convert.ToInt32(row["BillEmplUno"]);
				
				upd.Offc = Convert.ToString(row["brCode"]).Trim();
				upd.Dept = Convert.ToString(row["fileDepartment"]).Trim();
				upd.Prof = Convert.ToString(row["fileFundCode"]).Trim();
				upd.TimeInc = Convert.ToString(row["TimeInc"]);  //T
                if (row["OpenDate"] != DBNull.Value)
                {
                    if(_convertToLocalTime)
                        upd.NextFreqDate = Convert.ToDateTime(row["OpenDate"]).ToLocalTime();
                    else
                        upd.NextFreqDate = Convert.ToDateTime(row["OpenDate"]);
                }

                //#####Added to support 7.5 sp1 of the services
                if (row.Table.Columns["RoundType"] != null)
                    upd.RoundType = Convert.ToString(row["RoundType"]);
                else
                    upd.RoundType = "S";
                //#####


                //added 18/4/2011 for KN but optionally can be used by others
                if (row.Table.Columns["TimeTypeCode"] != null)
                    upd.TimeTypeCode = Convert.ToString(row["TimeTypeCode"]);

                if (row.Table.Columns["MattCatCode"] != null)
                    upd.MattCatCode = Convert.ToString(row["MattCatCode"]);

                
				_matterSvs.Update(iMatterUno,upd);

				return iMatterUno;
			}
			catch(Exception ex)
			{
				string s = "ExportMatter: " + ex.Message;
				Exception enew = new Exception(s);
				throw enew;
			}

		}
		
				
		/// <summary>
		/// Updates matter in CMS where changed in OMS
		/// </summary>
		/// <param name="row">Data row for the matter</param>
		/// <returns>True if updated OK</returns>
		protected override bool UpdateMatter(DataRow row)
		{
			// Update class inherited from ClientDataBase performs update wrappers up all the client data
			MatterService.MatterDataUpdate oData;
			MatterService.MatterData matter;
			
			//check if logged in and then login if not
			if(! _loggedIn)
				LoginToCMS();
			
			// Get file record
			int iUno = Convert.ToInt32(row["fileExtLinkID"]);
			matter = _matterSvs.ReadSingle(iUno);
			
			// returns a Data Update object with more methods than a create object
			oData = _matterSvs.GetDataUpdate(matter);
			
			if (oData != null)
			{
				try
				{
					//Update any rows that are not dbNull
                    if (row["fileNo"] != DBNull.Value)
					    oData.MatterCode = Convert.ToString(row["fileNo"]).Trim();
                    if (row["clextID"] != DBNull.Value)
					    oData.ClientUno = Convert.ToInt32(row["clextID"]);
                    if (row["MatterName"] != DBNull.Value)
                        oData.MatterName = Convert.ToString(row["MatterName"]);
                    if (row["fileDesc"] != DBNull.Value)
					    oData.LongMattName = Convert.ToString(row["fileDesc"]);
                    if (row["fileExternalNotes"] != DBNull.Value)
					    oData.DescText = Convert.ToString(row["fileExternalNotes"]);
                    if (row["StatusCode"] != DBNull.Value)
                        oData.StatusCode = Convert.ToString(row["StatusCode"]).Trim();
                    if (row["fileType"] != DBNull.Value)
                        oData.MattTypeCode = Convert.ToString(row["fileType"]).Trim();
					if(row["AssignEmplUno"] != DBNull.Value)
						oData.AssignEmplUno = Convert.ToInt32(row["AssignEmplUno"]);
					if(row["RespEmplUno"] != DBNull.Value)
						oData.RespEmplUno = Convert.ToInt32(row["RespEmplUno"]);
					if(row["BillEmplUno"] != DBNull.Value)
						oData.BillEmplUno = Convert.ToInt32(row["BillEmplUno"]);
					if(row["fileEstimate"] != DBNull.Value)
						oData.AmtArrange = Convert.ToDecimal(row["fileEstimate"]);
                    if (row["fileAccCode"] != DBNull.Value)
                        oData.ClaimNumber = Convert.ToString(row["fileAccCode"]).Trim();
                    
                    //added 31/1/2013
                    //set ClientMatName if it exists added31/1/2013
                    if (row.Table.Columns["ClientMatName"] != null)
                    {
                        if (row["ClientMatName"] != DBNull.Value)
                            oData.ClientMatName = Convert.ToString(row["ClientMatName"]).Trim();
                    }
                    else
                    {
                        if (row["fileDesc"] != DBNull.Value)
                            oData.ClientMatName = Convert.ToString(row["fileDesc"]).Trim();
                    }
                    
                    
                    if (row["Updated"] != DBNull.Value)
                    {
                        if(_convertToLocalTime)
                            oData.EditDate = Convert.ToDateTime(row["Updated"]).ToLocalTime();
                        else
                            oData.EditDate = Convert.ToDateTime(row["Updated"]);
                    }

                    if (row["fileSource"] != DBNull.Value)
                        oData.MattClassCode = Convert.ToString(row["fileSource"]).Trim();
					
                    if (row["FeeBillFormat"] != DBNull.Value)
                        oData.FeeBillFormat = Convert.ToString(row["FeeBillFormat"]); // 1,2,3
                    if (row["Contingency"] != DBNull.Value)
					    oData.Contingency = Convert.ToString(row["Contingency"]);
                    if (row["ProrateTime"] != DBNull.Value)
                        oData.ProrateTime = Convert.ToString(row["ProrateTime"]);
                    if (row["GenDisbFlag"] != DBNull.Value)
                        oData.GenDisbFlag = Convert.ToString(row["GenDisbFlag"]);  // Y, N
                    if (row["AllowTime"] != DBNull.Value)
                        oData.AllowTime = Convert.ToString(row["AllowTime"]);
                    if (row["brCode"] != DBNull.Value)
                        oData.Offc = Convert.ToString(row["brCode"]).Trim();
                    if (row["fileDepartment"] != DBNull.Value)
                        oData.Dept = Convert.ToString(row["fileDepartment"]).Trim();
                    if (row["fileFundCode"] != DBNull.Value)
                        oData.Prof = Convert.ToString(row["fileFundCode"]).Trim();
                    if (row["TimeInc"] != DBNull.Value)
                        oData.TimeInc = Convert.ToString(row["TimeInc"]);
					
					if(row["CloseDate"] != DBNull.Value)
					{
						if(_convertToLocalTime)
                            oData.CloseDate = Convert.ToDateTime(row["CloseDate"]).ToLocalTime();
                        else
                            oData.CloseDate = Convert.ToDateTime(row["CloseDate"]);
						
                        if(row["CloseEmplUno"] != DBNull.Value)
							oData.CloseEmplUno = Convert.ToInt32(row["CloseEmplUno"]);
					}


                    //#####Added to support 7.5 sp1 of the services
                    if (row.Table.Columns["RoundUp"] != null)
                        oData.RoundType = Convert.ToString(row["RoundUp"]);
                    else
                        oData.RoundType = "S";
                    //#####


                    //added 18/4/2011 for KN but optionally can be used by others
                    if (row.Table.Columns["TimeTypeCode"] != null)
                        oData.TimeTypeCode = Convert.ToString(row["TimeTypeCode"]);

                    if (row.Table.Columns["MattCatCode"] != null)
                        oData.MattCatCode = Convert.ToString(row["MattCatCode"]);


                    //added for MAB 23/04/2013
                    if (row.Table.Columns["TmRateSetUno"] != null)
                    {
                        int iTmRateSetUno;
                        if (Int32.TryParse(Convert.ToString(row["TmRateSetUno"]),out iTmRateSetUno))
                        {
                            oData.TmRateSetUno = iTmRateSetUno;
                        }
                    }


					_matterSvs.Update(iUno,oData);

					return true;

				}
				catch(Exception ex)
				{
					throw new Exception("UpdateMatter failed for matter ID " + Convert.ToString(row["fileID"]) + 
						" with CMS UNO of " + Convert.ToString(iUno) + " Error: " + ex.Message);
				}
			}
			else
			{
				base.LogEntry("No CMS matter found with Uno of " + Convert.ToString(iUno),EventLogEntryType.Warning);
				return false;
			}
		}


		/// <summary>
		/// Doesn'at actually export the user as we cannot do this so just locates the record
		/// </summary>
		/// <param name="row"></param>
		/// <returns>CMS UNO of user</returns>
		protected override int ExportUser(DataRow row)
		{
			int retval = 0;
			
			//check if logged in and then login if not
			if(! _loggedIn)
				LoginToCMS();
			
			string usrInits = Convert.ToString(row["usrInits"]).ToUpper();
			
			try
			{
				string filter = "(Employee.Code is " + usrInits + ") AND (Employee.Inactive is N)";
												
				PersonnelService.PersonnelData[] oPerList = _personnelSvs.Read(filter);
				
                if(oPerList == null)
                    throw new ApplicationException("No Data Returned from Personnel Service Search");

				//should only return 1 will error if null
				retval = oPerList[0].EmplUno;
						
				return retval;
			}
			catch(Exception ex)
			{
                throw new ApplicationException("Cannot locate personnel record for employee with value " + usrInits + " " + ex.Message);
			}
		}

		
		/// <summary>
		/// Exports time entry record
		/// </summary>
		/// <param name="row">Time entry data row</param>
		/// <returns>CMS UNO of inserted record</returns>
		protected override object ExportTimeRecord(DataRow row)
		{
			//check if logged in and then login if not
			if(! _loggedIn)
				LoginToCMS();
			
			TimeEntryService.TimeEntryDataCreate timedc = _timeSvs.InitializeDataCreate();
            
			try
			{
				decimal basehours = Convert.ToDecimal(row["timeActualMins"]) /60;
				decimal tobillamount = Convert.ToDecimal(row["timeCharge"]);
				decimal tobillhours = Convert.ToDecimal(row["timeMins"]) /60;

				timedc.ActionCode =  Convert.ToString(row["timeActivityCode"]); //.PadRight(4,' '); // "DEF"C ; //time activity code
				timedc.BaseHours = basehours;
				timedc.ToBillAmount = tobillamount;
				timedc.ToBillHours = tobillhours;
				timedc.CalculateWip = Convert.ToString(row["CalculateWip"]);
				timedc.MatterUno = Convert.ToInt32(row["fileExtLinkID"]);
				timedc.Printed = Convert.ToString(row["Printed"]);
				timedc.CurrencyCode = Convert.ToString(row["filecurISOCode"]);
				timedc.NarrativeText = Convert.ToString(row["timeDesc"]);
				timedc.TimekeeperUno = Convert.ToInt32(row["usrextid"]);

                //Added August 2009 for requirement for PhaseTaskUno and ActivityCode properties to be passed to web service
                if (row.Table.Columns.Contains("PhaseTaskUno"))
                {
                    if (row["PhaseTaskUno"] != DBNull.Value)
                        timedc.PhaseTaskUno = Convert.ToInt32(row["PhaseTaskUno"]);
                }
                if (row.Table.Columns.Contains("ActivityCode"))
                {
                    if (row["ActivityCode"] != DBNull.Value)
                        timedc.ActivityCode = Convert.ToString(row["ActivityCode"]);
                }
                
				if(_convertToLocalTime)
                    timedc.TransactionDate = Convert.ToDateTime(row["timeRecorded"]).ToLocalTime();
                else
                    timedc.TransactionDate = Convert.ToDateTime(row["timeRecorded"]);
				
				int iUno = _timeSvs.Create(timedc,false);

				return iUno;
						
			}
			catch(Exception ex)
			{
				string s = "ExportTimeRecord: " + ex.Message;
				Exception enew = new Exception(s);
				throw enew;
			}
		}
		
		
		/// <summary>
		/// Export financial record
		/// </summary>
		/// <param name="row">Financial record</param>
		/// <returns>ID of record NOTE: No implementation at the moment so retuns 0</returns>
		protected override object ExportFinancialRecord(DataRow row)
		{
			return 0;
		}

        protected override int ExportFeeEarner(DataRow row)
        {
            return 0;
        }


		#endregion
				
		#region IDisposable Members
		
		/// <summary>
		/// Nulls all the web servce references
		/// </summary>
		new public void Dispose()
		{
			base.Dispose();
		}

		#endregion
	}
}
