using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace FWBS.Common
{
    /// <summary>
    /// Class to interrogate Active Directory.
    /// Requires a reference to System.DirectoryServices.dll
    /// </summary>
    public sealed class ActiveDirectoryInfo
	{
		#region Constructor
		public ActiveDirectoryInfo()
		{}
		#endregion
        private string overrideDomain;
		#region User Information section
        public string OverrideDomain
        {
            set { overrideDomain = value; }
        }     
        
		/// <summary>
		/// Overload for get users that retrives from current domain
		/// </summary>
		/// <returns>Data table of domain users</returns>
		public DataTable UserList()
		{
			return GetUsers(true,null,null,null);
		}
		
		
		/// <summary>
		/// Overload for UserList that allows passing of a domain
		/// Had a lot of trouble with this due to permissions and trusts but in theory should work OK
		/// </summary>
		/// <param name="Domain">Domain e.g admin.fwbs.net</param>
		/// <returns>Data table of domain users</returns>
		public DataTable UserList(string Domain)
		{
			return GetUsers(true,null,null,Domain);
		}
		
		
		/// <summary>
		/// Overload for UserList that allows passing of a username and password in case
		/// the currently logged in user does not have permission to query this information
		/// </summary>
		/// <param name="UserName">NT username</param>
		/// <param name="Password">NT password</param>
		/// <returns>Data table of domain users</returns>
		public DataTable UserList(string UserName, string Password)
		{
			return GetUsers(false,UserName,Password,null);
		}
		
		
		/// <summary>
		/// Overload for UserList that allows passing of domain name and user details
		/// Not tested!!
		/// </summary>
		/// <param name="Domain">Domain Name</param>
		/// <param name="UserName">NT User name</param>
		/// <param name="Password">NT password</param>
		/// <returns>Data table of domain users</returns>
		public DataTable UserList(string Domain, string UserName, string Password)
		{
			return GetUsers(false,UserName,Password,Domain);
		}


        /// <summary>
        /// Gets a list of AD groups based upon a Parent group
        /// Method only supports Integrated security as 
        /// designed to be used fro AD Import utility
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public DataTable GetUsersInDir(DirectoryEntry dir)
        {
            DataTable dtUsers = CreateUsersTable();

            foreach (DirectoryEntry de in dir.Children)
            {
                if (CheckIfUser(de))
                    PopulateUserRow(dtUsers, de);
            }

            return dtUsers;
        }

        /// <summary>
        /// Gets a list of AD groups based upon a Parent group
        /// Method only supports Integrated security as 
        /// designed to be used fro AD Import utility
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="otherEntries">Output collection of non-user entries</param>
        /// <returns></returns>
        public DataTable GetUsersInDirEx(DirectoryEntry dir, out List<DirectoryEntry> otherEntries)
        {
            otherEntries = new List<DirectoryEntry>();
            DataTable dtUsers = CreateUsersTable();

            foreach (DirectoryEntry de in dir.Children)
            {
                if (CheckIfUser(de))
                    PopulateUserRow(dtUsers, de);
                else
                    otherEntries.Add(de);
            }

            return dtUsers;
        }

        /// <summary>
        /// Checks if a Directory entry is a user
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        private static bool CheckIfUser(DirectoryEntry de)
        {
            string val = GetValue(de, "objectCategory");

            return val.Contains("CN=Person");
        }

		
		/// <summary>
		/// Private method to retrive a databale of users from the active directory
		/// </summary>
		/// <param name="IntegratedSecurity">flag to indicate wether to use integrated security</param>
		/// <param name="UserName">NT Username used when IntegratedSecurity = false</param>
		/// <param name="Password">NT password used when IntegratedSecurity = false</param>
		/// <param name="Domain">Domain Name e.g admin.fwbs.net</param>
		/// <returns>Data Table of domain users</returns>
		private DataTable GetUsers(bool IntegratedSecurity ,string UserName,string Password,string Domain)
		{
			DirectoryEntry root;			//where to start searching
			
			//create an empty datatable to return the results
            DataTable dtUsers = CreateUsersTable();

			
			// If domain is not specified it will default to currently logged in domain which is probably OK.
			if(Domain == null)
			{
				root =new DirectoryEntry();
			}
			else
			{
				//build an LDAP path for the root
				string path = @"LDAP://";
				string[] domarray = Domain.Split('.'); 
				for(int i = 0; i < domarray.Length; i++)
				{
					path += @"dc=" + domarray[i];
					if(domarray.Length-1 > i)
						path += ",";
				}
				//set root via built up path
				root=new DirectoryEntry(path);
			}
			
			// Integrated security is the easeiset to administer but may want to option to specify an account
			if(! IntegratedSecurity)
			{
				root.Username = UserName;
				root.Password = Password;
			}
			
			//create a searcher at the root of the domain
			using (DirectorySearcher searcher = new DirectorySearcher(root))
			{
				//filter for only users
				searcher.Filter = "(&(objectClass=user)(objectCategory=Person))";
				searcher.PageSize = 500;
				
				//get all results into SearchResultCollection
				using (SearchResultCollection results = searcher.FindAll())
				{
					foreach (SearchResult result in results)
					{
						//get the diretory entry object
						using (DirectoryEntry de = result.GetDirectoryEntry())
						{
							//if either of the fields are not populated then move on
							string username = GetValue(de,"sAMAccountName");
							if (username == "")
								continue;

							string displayname = GetValue(de,"displayName");
							if (displayname == "")
								continue;

							PopulateUserRow(dtUsers, de);
						}
					}
				}
			}

			root.Dispose();
			return dtUsers;
		}

        private void PopulateUserRow(DataTable dtUsers, System.DirectoryServices.DirectoryEntry de)
        {
            //add a row to our table
            DataRow row = dtUsers.NewRow();
            string username = GetValue(de, "sAMAccountName");

            //populate row with values  
            row["usrInits"] = GetUserIntials(de); //userName;
            row["usrAlias"] = username;
            row["usrADID"] = GetUserDomainName(de, username);
            row["usrSQLID"] = username;
            row["usrFullName"] = GetValue(de, "displayName");
            row["usrEmail"] = GetValue(de, "mail");
            row["usrDDI"] = GetValue(de, "telephoneNumber");
            row["usrDDIFax"] = GetValue(de, "facsimileTelephoneNumber");
            row["usrHomePage"] = GetValue(de, "wWWHomePage");
            row["usrPN"] = GetValue(de, "userPrincipalName");

            // add row to the table 
            dtUsers.Rows.Add(row);
        }

        private static DataTable CreateUsersTable()
        {
            DataTable dtUsers = new DataTable("Users");
            dtUsers.Columns.Add("usrInits", typeof(System.String));
            dtUsers.Columns.Add("usrAlias", typeof(System.String));
            dtUsers.Columns.Add("usrADID", typeof(System.String));
            dtUsers.Columns.Add("usrSQLID", typeof(System.String));
            dtUsers.Columns.Add("usrFullName", typeof(System.String));
            dtUsers.Columns.Add("usrEmail", typeof(System.String));
            dtUsers.Columns.Add("usrWorksFor", typeof(System.Int32));
            dtUsers.Columns.Add("usrDDI", typeof(System.String));
            dtUsers.Columns.Add("usrDDIFax", typeof(System.String));
            dtUsers.Columns.Add("usrHomePage", typeof(System.String));
            dtUsers.Columns.Add("usrPN", typeof(System.String));
            dtUsers.Columns.Add("brID", typeof(System.Int32));  //added as required by MC
            dtUsers.Columns.Add("usrcurISOCode", typeof(System.String)); //added as required by MC
            return dtUsers;
        }
		
		
		/// <summary>
		/// Utility function listing all user properties with their values
		/// </summary>
		/// <param name="firstonly">flag to indicate if we should print out all values</param>
		/// <returns>data Table of property names and values</returns>
		public DataTable ListAllUserProperties(bool firstonly)
		{
			DataTable dtReturn;
			DirectoryEntry root;			//where to start searching
			
			dtReturn =  new DataTable("UserProps");
            dtReturn.Columns.Add("Property", typeof(System.String));
            dtReturn.Columns.Add("Value", typeof(System.String));
			
			//create root entry
			root =new DirectoryEntry();
			
			//create a searcher at the root of the domain
			using (DirectorySearcher searcher = new DirectorySearcher(root))
			{
				//filter for only users
				searcher.Filter = "(&(objectClass=user)(objectCategory=Person))";
				searcher.PageSize = 500;
				
				//get all results into SearchResultCollection
				using (SearchResultCollection results = searcher.FindAll())
				{
					foreach(SearchResult result in results)
					{
						//get the diretory entry object
						using (DirectoryEntry de = result.GetDirectoryEntry())
						{
							foreach(string mykey in de.Properties.PropertyNames)
							{
								DataRow row = dtReturn.NewRow();

								string prop = mykey;
								string val = GetValue(de,prop);
								
								row[0] = prop;
								row[1] = val;

								dtReturn.Rows.Add(row);
							}
						}

						//get out if only getting 1
						if(firstonly)
							break;
					}
				}
			}

			root.Dispose();
			return dtReturn;
		}

		/// <summary>
		/// Concatenates 3 directory entries to create initials
		/// </summary>
		/// <param name="de"></param>
		/// <returns></returns>
		private string GetUserIntials(DirectoryEntry de)
		{
			string sReturn = "";
			try
			{
				string fn = GetValue(de,"sAMAccountName");
				if(fn.Length > 1)
					fn = fn.Substring(0,1);

				string mid = GetValue(de,"initials");
				
				string sn = GetValue(de,"sn");
				if(sn.Length > 1)
					sn = sn.Substring(0,1);
				
				sReturn = fn + mid + sn;
			}
			catch
			{
				sReturn = "";
			}
			return sReturn.ToUpper();
		}
		
		/// <summary>
		/// Builds up the users fully qualified domain name in the form domain\username
		/// </summary>
		/// <param name="de"></param>
		/// <param name="username"></param>
		/// <returns></returns>
		private string GetUserDomainName(DirectoryEntry de,string username)
		{
			string sReturn = "";
			string path = de.Path;
			
			if(username =="")
			{
				return username;
			}

			if(path != "")
			{
				string[] vals = path.Split(',');
				
				for(int i = 0; i < vals.Length; i++)
				{
					//look for the first instance of DC which will be the local Domain
                    //Add a look in SpecificData for ADSECDOMAIN if override is needed.
                    if (String.IsNullOrEmpty(overrideDomain))
                    {
                        if (vals[i].Substring(0, 3) == "DC=")
                        {
                            string sDomain = vals[i].Substring(3, vals[i].Length - 3);
                            sReturn = sDomain.ToUpper() + @"\" + username;
                            break;
                        }
                    }
                    else
                    {
                        sReturn = overrideDomain.ToUpper() + @"\" + username;
                        break;
                    }
				}
			}
			return sReturn;
		}

        public string GetUserPrincipalName(string userDomainName)
        {
            string userPrincipalName = null;
            string[] userNameParts = userDomainName.Split('\\');
            if (userNameParts.Length == 2)
            {
                DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, userNameParts[0]);
                Domain domain = Domain.GetDomain(context);
                using (DirectorySearcher searcher = new DirectorySearcher(domain.GetDirectoryEntry()))
                {
                    searcher.Filter = $"(&(objectClass=user)(objectCategory=Person)(sAMAccountName={userNameParts[1]}))";
                    searcher.PropertiesToLoad.Add("userPrincipalName");
                    SearchResult result = searcher.FindOne();
                    if (result != null)
                        userPrincipalName = Convert.ToString(result.Properties["userPrincipalName"][0]);
                }
            }
            return userPrincipalName;
        }


		#endregion

		#region Printer Information Section
		
		/// <summary>
		/// Overload for PrinterList that retrives from current domain
		/// </summary>
		/// <returns>Data table of domain users</returns>
		public DataTable PrinterList()
		{
			return GetPrinters(true,null,null,null);
		}
		
		
		/// <summary>
		/// Overload for PrinterList that allows passing of a domain
		/// Had a lot of trouble with this die to permissions and trusts but in theory should work OK
		/// </summary>
		/// <param name="Domain">Domain e.g admin.fwbs.net</param>
		/// <returns>Data table of domain users</returns>
		public DataTable PrinterList(string Domain)
		{
			return GetPrinters(true,null,null,Domain);
		}
		
		
		/// <summary>
		/// Overload for PrinterList that allows passing of a username and password in case
		/// the currently logged in user does not have permission to query this information
		/// </summary>
		/// <param name="UserName">NT username</param>
		/// <param name="Password">NT password</param>
		/// <returns>Data table of domain users</returns>
		public DataTable PrinterList(string UserName, string Password)
		{
			return GetPrinters(false,UserName,Password,null);
		}
		
		
		/// <summary>
		/// Overload for PrinterList that allows passing of domain name and user details
		/// Not tested!!
		/// </summary>
		/// <param name="Domain">Domain Name</param>
		/// <param name="UserName">NT User name</param>
		/// <param name="Password">NT password</param>
		/// <returns>Data table of domain users</returns>
		public DataTable PrinterList(string Domain, string UserName, string Password)
		{
			return GetPrinters(false,UserName,Password,Domain);
		}
		

		
		/// <summary>
		/// Private method to retrive a databale of users from the active directory
		/// </summary>
		/// <param name="IntegratedSecurity">flag to indicate wether to use integrated security</param>
		/// <param name="UserName">NT Username used when IntegratedSecurity = false</param>
		/// <param name="Password">NT password used when IntegratedSecurity = false</param>
		/// <param name="Domain">Domain Name e.g admin.fwbs.net</param>
		/// <returns>Data Table of domain users</returns>
		private DataTable GetPrinters(bool IntegratedSecurity ,string UserName,string Password,string Domain)
		{
			DataTable dtReturn = null;		//return value
			DirectoryEntry root;			//where to start searching
			
			//create an empty datatable to return the results
			dtReturn =  new DataTable("Printers");
            dtReturn.Columns.Add("printName", typeof(System.String));
            dtReturn.Columns.Add("printUNCName", typeof(System.String));
            dtReturn.Columns.Add("printLocation", typeof(System.String));
            dtReturn.Columns.Add("printDescription", typeof(System.String));
            dtReturn.Columns.Add("printTrays", typeof(System.Int32));
			
			// If domain is not specified it will default to currently logged in domain which is probably OK.
			if(Domain == null)
			{
				root =new DirectoryEntry();
			}
			else
			{
				//build an LDAP path for the root
				string path = @"LDAP://";
				string[] domarray = Domain.Split('.'); 
				for(int i = 0; i < domarray.Length; i++)
				{
					path += @"dc=" + domarray[i];
					if(domarray.Length-1 > i)
						path += ",";
				}
				//set root via built up path
				root=new DirectoryEntry(path);
			}
			
			// Integrated security is the easeiset to administer but may want to option to specify an account
			if(! IntegratedSecurity)
			{
				root.Username = UserName;
				root.Password = Password;
			}
			
			//create a searcher at the root of the domain	
			using (DirectorySearcher searcher = new DirectorySearcher(root))
			{
				//filter for only users
				searcher.Filter = "(objectCategory=printQueue)";

				//get all results into SearchResultCollection
				using (SearchResultCollection results = searcher.FindAll())
				{
					foreach(SearchResult result in results)
					{
						//get the diretory entry object
						using (DirectoryEntry de = result.GetDirectoryEntry())
						{
							//add a row to our table
							DataRow row = dtReturn.NewRow();

							// user this value for the 4 UID files unless we find something better
							string printLoc = GetValue(de,"Location");
							
							//populate row with values  
							row["printName"] = GetValue(de,"name");
							row["printUNCName"] = GetValue(de,"uncname");
							row["printLocation"] = GetValue(de,"location");
							row["printDescription"] = GetValue(de,"driverName");
							row["printTrays"] = 1; //set a value as it is not obtainable and does not allow nulls
							
							// add row to the table providing the userfullname field has a value 
							// this is a required field in the users table and should filter out system accounts
							if(Convert.ToString(row["printName"]) != "")
								dtReturn.Rows.Add(row);
						}
					}
				}
			}

			root.Dispose();
			return dtReturn;
		}
		
		/// <summary>
		/// Utility function listing all printer properties with their values
		/// </summary>
		/// <param name="firstonly">flag to indicate if we should print out all values</param>
		/// <returns>data Table of property names and values</returns>
		public DataTable ListAllPrinterProperties(bool firstonly)
		{
			DataTable dtReturn;
			DirectoryEntry root = null;			//where to start searching
			
			dtReturn =  new DataTable("PrinterProps");
            dtReturn.Columns.Add("Property", typeof(System.String));
            dtReturn.Columns.Add("Value", typeof(System.String));

			try
			{
				//create root entry
				root =new DirectoryEntry();
				
				//create a searcher at the root of the domain
				using (DirectorySearcher searcher = new DirectorySearcher(root))
				{
					//filter for only users
					searcher.Filter = "(objectCategory=printQueue)";
					
					//get all results into SearchResultCollection
					using (SearchResultCollection results = searcher.FindAll())
					{
						foreach(SearchResult result in results)
						{
							//get the diretory entry object
							using (DirectoryEntry de = result.GetDirectoryEntry())
							{
								foreach(string mykey in de.Properties.PropertyNames)
								{
									DataRow row = dtReturn.NewRow();

									string prop = mykey;
									string val = GetValue(de,prop);
									
									row[0] = prop;
									row[1] = val;

									dtReturn.Rows.Add(row);
								}
							}

							//get out if only getting 1
							if(firstonly)
								break;
						}
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Error: " + ex.Message);
			}
			finally
			{
				root?.Dispose();
			}
			return dtReturn;
		}
	



		#endregion

		#region Terminal Information section
		
		/// <summary>
		/// Overload for TerminalList that retrives from current domain
		/// </summary>
		/// <returns>Data table of domain users</returns>
		public DataTable TerminalList()
		{
			return GetTerminals(true,null,null,null);
		}
		
		
		/// <summary>
		/// Overload for TerminalList that allows passing of a domain
		/// Had a lot of trouble with this die to permissions and trusts but in theory should work OK
		/// </summary>
		/// <param name="Domain">Domain e.g admin.fwbs.net</param>
		/// <returns>Data table of domain users</returns>
		public DataTable TerminalList(string Domain)
		{
			return GetTerminals(true,null,null,Domain);
		}
		
		
		/// <summary>
		/// Overload for TerminalList that allows passing of a username and password in case
		/// the currently logged in user does not have permission to query this information
		/// </summary>
		/// <param name="UserName">NT username</param>
		/// <param name="Password">NT password</param>
		/// <returns>Data table of domain users</returns>
		public DataTable TerminalList(string UserName, string Password)
		{
			return GetTerminals(false,UserName,Password,null);
		}
		
		
		/// <summary>
		/// Overload for TerminalList that allows passing of domain name and user details
		/// Not tested!!
		/// </summary>
		/// <param name="Domain">Domain Name</param>
		/// <param name="UserName">NT User name</param>
		/// <param name="Password">NT password</param>
		/// <returns>Data table of domain users</returns>
		public DataTable TerminalList(string Domain, string UserName, string Password)
		{
			return GetTerminals(false,UserName,Password,Domain);
		}
		

		
		/// <summary>
		/// Private method to retrive a databale of users from the active directory
		/// </summary>
		/// <param name="IntegratedSecurity">flag to indicate wether to use integrated security</param>
		/// <param name="UserName">NT Username used when IntegratedSecurity = false</param>
		/// <param name="Password">NT password used when IntegratedSecurity = false</param>
		/// <param name="Domain">Domain Name e.g admin.fwbs.net</param>
		/// <returns>Data Table of domain users</returns>
		private DataTable GetTerminals(bool IntegratedSecurity ,string UserName,string Password,string Domain)
		{
			DataTable dtReturn = null;		//return value
			DirectoryEntry root;			//where to start searching
			
			//create an empty datatable to return the results
			dtReturn =  new DataTable("Terminals");
            dtReturn.Columns.Add("termName", typeof(System.String));
            dtReturn.Columns.Add("termADID", typeof(System.String));
			dtReturn.Columns.Add("termLoggedIn",typeof(Boolean));
			
			// If domain is not specified it will default to currently logged in domain which is probably OK.
			if(Domain == null)
			{
				root =new DirectoryEntry();
			}
			else
			{
				//build an LDAP path for the root
				string path = @"LDAP://";
				string[] domarray = Domain.Split('.'); 
				for(int i = 0; i < domarray.Length; i++)
				{
					path += @"dc=" + domarray[i];
					if(domarray.Length-1 > i)
						path += ",";
				}
				//set root via built up path
				root=new DirectoryEntry(path);
			}
			
			// Integrated security is the easeiset to administer but may want to option to specify an account
			if(! IntegratedSecurity)
			{
				root.Username = UserName;
				root.Password = Password;
			}
			
			//create a searcher at the root of the domain
			using (DirectorySearcher searcher = new DirectorySearcher(root))
			{
				//filter for only users
				searcher.Filter = "(objectCategory=computer)";
				searcher.PageSize = 500;

				//get all results into SearchResultCollection
				using (SearchResultCollection results = searcher.FindAll())
				{
					foreach(SearchResult result in results)
					{
						//get the diretory entry object
						using (DirectoryEntry de = result.GetDirectoryEntry())
						{
							//add a row to our table
							DataRow row = dtReturn.NewRow();
							
							//populate row with values  
							row["termName"] = GetValue(de,"name");
							row["termADID"] = ExtractGUID(de,"objectGUID");
							row["termLoggedIn"] = 0;
							
							// add row to the table providing the userfullname field has a value 
							// this is a required field in the users table and should filter out system accounts
							if(Convert.ToString(row["termName"]) != "")
								dtReturn.Rows.Add(row);
						}
					}
				}
			}

			root.Dispose();
			return dtReturn;
		}
		
		/// <summary>
		/// Utility function listing all printer properties with their values
		/// </summary>
		/// <param name="firstonly">flag to indicate if we should print out all values</param>
		/// <returns>data Table of property names and values</returns>
		public DataTable ListAllTerminalProperties(bool firstonly)
		{
			DataTable dtReturn;
			DirectoryEntry root = null;			//where to start searching
			
			dtReturn =  new DataTable("TerminalProps");
            dtReturn.Columns.Add("Property", typeof(System.String));
            dtReturn.Columns.Add("Value", typeof(System.String));

			try
			{
				//create root entry
				root =new DirectoryEntry();
				
				//create a searcher at the root of the domain
				using (DirectorySearcher searcher = new DirectorySearcher(root))
				{
					//filter for only users
					searcher.Filter = "(objectCategory=computer)";
					searcher.PageSize = 500;
					
					//get all results into SearchResultCollection
					using (SearchResultCollection results = searcher.FindAll())
					{
						foreach(SearchResult result in results)
						{
							//get the diretory entry object
							using (DirectoryEntry de = result.GetDirectoryEntry())
							{
								foreach(string mykey in de.Properties.PropertyNames)
								{
									DataRow row = dtReturn.NewRow();

									string prop = mykey;
									string val = GetValue(de,prop);
									
									row[0] = prop;
									row[1] = val;

									dtReturn.Rows.Add(row);
								}
							}

							//get out if only getting 1
							if(firstonly)
								break;
						}
					}
				}
			}
			catch(Exception ex)
			{
				throw new Exception("Error: " + ex.Message);
			}
			finally
			{
				root?.Dispose();
			}
			return dtReturn;
		}

		#endregion

		#region Shared methods

		/// <summary>
		/// Private method to encapsulate reading Active directory entries as they can error if they are not populated
		/// </summary>
		/// <param name="de">Directory entry object</param>
		/// <param name="propertyName">Name of property to query</param>
		/// <returns>the value of the property if it exists or an empty string</returns>
		private static string GetValue(DirectoryEntry de,string propertyName)
		{
			string sReturn;
			try
			{
				sReturn = de.Properties[propertyName].Value.ToString();
			}
			catch
			{
				sReturn = "";
			}
			return sReturn;
		}
		
		
		/// <summary>
		/// Extracts a string from a byte array
		/// </summary>
		/// <param name="de"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		private static string ExtractGUID(DirectoryEntry de,string propertyName)
		{
			string sReturn = "";
			try
			{
				System.Byte[] val = (byte[])de.Properties[propertyName].Value;
				
				System.Guid guid = new Guid(val);

				sReturn = guid.ToString();
			}
			catch
			{
				sReturn = "";
			}
			return sReturn;
		}
		

		#endregion
	}
}
