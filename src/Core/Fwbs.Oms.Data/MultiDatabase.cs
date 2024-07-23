
using System;
using System.Xml;
using FWBS.Common;

namespace FWBS.OMS.Data
{
    /// <summary>
    /// A class that retrieves database settings from default to multi database settings.
    /// </summary>
    public class DatabaseConnections
	{
		#region Fields
		
		/// <summary>
		/// Holds the xml file to manipulate the database settings.
		/// </summary>
		internal XmlDocument _xml = new XmlDocument();

		/// <summary>
		/// Holds the location of the multidb file.
		/// </summary>
		internal FWBS.Common.Reg.ApplicationSetting _multiLocation = null;

		/// <summary>
		/// Holds the default multidb entry.
		/// </summary>
		internal FWBS.Common.Reg.ApplicationSetting _multiDefault = null;

		/// <summary>
		/// Displays the multi db list to the client.
		/// </summary>
		internal FWBS.Common.Reg.ApplicationSetting _multiEnabled = null;

		/// <summary>
		/// Long application name used for connection strings.
		/// </summary>
		private string _appName = "FWBS Application";

		/// <summary>
		/// Short application name used for the root element.
		/// </summary>
		private string _shortAppName = "FWBS";

		/// <summary>
		/// The location of the multidb file.
		/// </summary>
		private string _path = "";

		#endregion

		#region Constructors

		public DatabaseConnections(string appName, string appKey, string appVersion)
		{
			_appName = appName;
			_shortAppName = appKey;

			_multiLocation = new FWBS.Common.Reg.ApplicationSetting(_shortAppName, appVersion, "Data", "MultiDBLocation");
			_multiDefault = new FWBS.Common.Reg.ApplicationSetting(_shortAppName, appVersion, "Data", "MultiDBDefault");
			_multiEnabled = new FWBS.Common.Reg.ApplicationSetting(_shortAppName, appVersion, "Data", "MultiDBEnabled");
			
			Open(Convert.ToString(_multiLocation.GetSetting("")));

		}

		#endregion
		
		#region Properties

		/// <summary>
		/// Retieves the default databse object.
		/// </summary>
		public DatabaseSettings Default
		{
			get
			{
				try
				{
					int def = Common.ConvDef.ToInt16(_multiDefault.GetSetting(0), 0);
					if (def >= Count)
						def = 0;
					if (Count == 0)
						return CreateDatabaseSettings();
					else
						return this[def];
				}
				catch
				{
					return CreateDatabaseSettings();
				}
			}
			set
			{
				if (value != null)
					_multiDefault.SetSetting(value.Number);
			}
		}


		/// <summary>
		/// Determines whether the multi databse feature is enabled.
		/// </summary>
		public bool MultiDBFeatureEnabled
		{
			get
			{
				try
				{
					return ConvDef.ToBoolean(_multiEnabled.GetSetting(true), true);
				}
				catch
				{
					return true;
				}
			}
			set
			{
				_multiEnabled.SetSetting(value);
			}
		}

		/// <summary>
		/// Indexer to access the previously build array list.
		/// </summary>
		public DatabaseSettings this [int number]
		{
			get
			{
				BuildXml();
				XmlElement data = _xml.SelectSingleNode("/" + XmlConvert.EncodeName(_shortAppName) + "/MultiDatabase/Data") as XmlElement;
				XmlElement cnn = data.ChildNodes[number] as XmlElement;
				return new DatabaseSettings(cnn, number);
			}
		}


		/// <summary>
		/// Number of databases within the multi database logic.
		/// </summary>
		public int Count
		{
			get
			{
				BuildXml();
				XmlElement data = _xml.SelectSingleNode("/" + XmlConvert.EncodeName(_shortAppName) + "/MultiDatabase/Data") as XmlElement;
				if (data == null)
					return 0;
				else
					return data.ChildNodes.Count;

			}
		}

		public string CurrentLocation
		{
			get
			{
				return _path;
			}
		}

		#endregion

		#region Methods

		private void BuildXml()
		{
			if (_xml == null)
				_xml = new XmlDocument();

			System.Xml.XmlNode root = _xml.SelectSingleNode("/" + XmlConvert.EncodeName(_shortAppName));
			if (root == null)
			{
				root = _xml.CreateElement(XmlConvert.EncodeName(_shortAppName));
				_xml.AppendChild(root);
			}
 
			System.Xml.XmlElement multi = root.SelectSingleNode("MultiDatabase") as System.Xml.XmlElement;
			if (multi == null)
			{
				multi = _xml.CreateElement("MultiDatabase");
				root.AppendChild(multi);
			}

			System.Xml.XmlElement data = multi.SelectSingleNode("Data") as System.Xml.XmlElement;
			if (data == null)
			{
				data = _xml.CreateElement("Data");
				multi.AppendChild(data);
			}

		}

		public DatabaseSettings CreateDatabaseSettings()
		{
			int count = Count + 1;
			BuildXml();
			XmlElement el = _xml.CreateElement("Connection");
			XmlElement data = _xml.SelectSingleNode("/" + XmlConvert.EncodeName(_shortAppName) + "/MultiDatabase/Data") as XmlElement;
			data.AppendChild(el);

            DatabaseSettings db = new DatabaseSettings(el, Count - 1);
			db.Description = "Connection " + Count.ToString();
			return db;
		}

		public void Save()
		{
			SaveAs(_path);
		}

		public void SaveAs(string path)
		{
			BuildXml();
			_xml.Save(path);
			_path = path;

			_multiLocation.SetSetting(_path);
		}

		public void Open(string path)
		{
			if (path == String.Empty || System.IO.File.Exists(path) == false || System.IO.Path.IsPathRooted(path) == false)
			{
				path = SpecialFolders.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
				path = System.IO.Path.Combine(path, "FWBS");
				path = System.IO.Path.Combine(path, _shortAppName);
				System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(path);
				if (dir.Exists == false)
				{
					dir.Create();
				}
			
				path = System.IO.Path.Combine(path, "multidb.xml");
			}

			if (System.IO.File.Exists(path))
				_xml.Load(path);
			
			_path = path;

			_multiLocation.SetSetting(_path);

			BuildXml();
		}

		public void Remove(DatabaseSettings setting)
		{
			try
			{
				setting._el.ParentNode.RemoveChild(setting._el);
			}
			catch{}
		}

		#endregion

	}

	/// <summary>
	/// Wrapper class for the data section in the registry, or separate file.
	/// </summary>
	public class DatabaseSettings
	{

		#region Fields


		private System.Xml.XmlDocument _doc = null;
		internal System.Xml.XmlNode _el = null;
		private int _number = 0;

		#endregion

		#region Constructors

		private DatabaseSettings(){}

		/// <summary>
		/// Internal construction only.
		/// </summary>
        /// <param name="el"></param>
		/// <param name="number"></param>
		internal DatabaseSettings(XmlElement el, int number)
		{
			_doc = el.OwnerDocument;
			_el =  el;
			_number = number;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the index number of the setting item.
		/// </summary>
		public int Number
		{
			get
			{
				return _number;
			}
            internal set
            {
                _number = value;
            }

		}

		/// <summary>
		/// Server type (SQL, OLEDB, ORACLE etc...).
		/// </summary>
		public string Provider
		{
			get
			{
				return GetExtraInfo("@provider");
			}
			set
			{
				SetExtraInfo("@provider", value);
			}
		}

        public bool ConnectionAlwaysOpen
        {
            get
            {
                return ConvertDef.ToBoolean(GetExtraInfo("@ConnectionAlwaysOpen"), false);
            }
            set
            {
                SetExtraInfo("@ConnectionAlwaysOpen", value.ToString());
            }
        }


		/// <summary>
		/// Database name.
		/// </summary>
		public string DatabaseName
		{
			get
			{
				return GetExtraInfo("@database");
			}
			set
			{
				SetExtraInfo("@database", value);
			}
		}

		/// <summary>
		/// Description of database.
		/// </summary>
		public string Description
		{
			get
			{
				return GetExtraInfo(".");
			}
			set
			{
				SetExtraInfo(".", value);
			}
		}

	
		/// <summary>
		/// SQL authentication, NT authentication or other authentication.
		/// </summary>
		public string LoginType
		{
			get
			{
				return GetExtraInfo("@loginType");
			}
			set
			{
				SetExtraInfo("@loginType", value);
			}
		}

		/// <summary>
		/// Server name / TCPIP address of database location.
		/// </summary>
		public string Server
		{
			get
			{
				return GetExtraInfo("@server");
			}
			set
			{
				SetExtraInfo("@server", value);
			}
		}

			
		/// <summary>
		/// Connection string.
		/// </summary>
		public string ConnectionString
		{
			get
			{
				string cnn = GetExtraInfo("@connectionString");
				
				if (Provider == "SQL")
				{
					if (cnn.IndexOf("Persist Security Info") < 0)
					{
						if (cnn.EndsWith(";") == false)
							cnn += ";";
						cnn+= "Persist Security Info=false";
					}

					if (cnn.IndexOf("Data Source") < 0)
					{
						if (cnn.EndsWith(";") == false)
							cnn += ";";
						cnn+= "Data Source=%SERVER%";
					}

					if (cnn.IndexOf("Initial Catalog") < 0)
					{
						if (cnn.EndsWith(";") == false)
							cnn += ";";
						cnn+= "Initial Catalog=%DATABASE%";
					}

					if (cnn.IndexOf("Application Name") < 0)
					{
						if (cnn.EndsWith(";") == false)
							cnn += ";";
						cnn += "Application Name=FWBS Application";
					}

					if (LoginType == "NT")
					{
						if (cnn.IndexOf("Integrated Security=SSPI") < 0)
						{
							if (cnn.EndsWith(";") == false)
								cnn += ";";
							cnn += "Integrated Security=SSPI";
						}
					}
					else if (LoginType == "AAD")
					{
						if (cnn.IndexOf("Authentication") < 0)
						{
							if (cnn.EndsWith(";") == false)
								cnn += ";";
							cnn += "Authentication=Active Directory Integrated";
						}
					}
					else
					{
						if (cnn.IndexOf("User ID") < 0)
						{
							if (cnn.EndsWith(";") == false)
								cnn += ";";
							cnn+= "User ID=%USER%";
						}

						if (cnn.IndexOf("Password") < 0)
						{
							if (cnn.EndsWith(";") == false)
								cnn += ";";
							cnn+= "Password=%PASSWORD%";
						}
					}
				}
				
				return cnn;
			}
			set
			{
				SetExtraInfo("@connectionString", value);
			}
		}


		/// <summary>
		/// Default login user name.
		/// </summary>
		public string UserName
		{
			get
			{
				return GetExtraInfo("@defaultUser");
			}
		}

		/// <summary>
		/// Default, login password.
		/// </summary>
		public string Password
		{
			get
			{
				return GetExtraInfo("@defaultPassword");
			}
		}
        /// <summary>
        /// Application Role name.
        /// </summary>
        public string ApplicationRoleName
        {
            get
            {
                return GetExtraInfo("@applicationRoleName");
            }
        }
        /// <summary>
        /// Application Role password.
        /// </summary>
        public string ApplicationRolePassword
        {
            get
            {
                return GetExtraInfo("@applicationRolePassword");
            }
        }

        /// <summary>
        /// MatterSphere servicelocation
        /// </summary>
        public string ServiceLocation
        {
            get
            {
                return GetExtraInfo("@serviceLocation");
            }
            set
            {
                SetExtraInfo("@serviceLocation", value);
            }
        }

 
        /// <summary>
        /// Automatically query with the NoLock hint where possible.
        /// </summary>
        public bool UseNoLock
        {
            get
            {
                return ConvertDef.ToBoolean(GetExtraInfo("@UseNoLock"), true);
            }
            set
            {
                SetExtraInfo("@UseNoLock", value.ToString());
            }
        }

		#endregion

		#region Methods

		public void ChangeUser(string newUser)
		{
			SetExtraInfo("@defaultUser", Common.Security.Cryptography.Encryption.NewKeyEncrypt(newUser, 30));
		}

		public void ChangePassword(string newPassword)
		{
			SetExtraInfo("@defaultPassword", Common.Security.Cryptography.Encryption.NewKeyEncrypt(newPassword, 30));
		}
        //Set the application role name in encrypted format
        public void ChangeAppRoleName(string newAppRoleName)
        {
            if (newAppRoleName == "")
                SetExtraInfo("@applicationRoleName", "");
            else
                SetExtraInfo("@applicationRoleName", Common.Security.Cryptography.Encryption.NewKeyEncrypt(newAppRoleName, 30));
        }
        //Set the application role password in encrypted format
        public void ChangeAppRolePassword(string newAppRolePassword)
        {
            if (newAppRolePassword == "")
                SetExtraInfo("@applicationRolePassword", "");
            else
                SetExtraInfo("@applicationRolePassword", Common.Security.Cryptography.Encryption.NewKeyEncrypt(newAppRolePassword, 30));
        }


		private string GetExtraInfo(string setting)
		{
			string def = "";
			string ret = "";
			
			switch (setting)
			{
				case "@provider":
					def = "SQL";
					break;
				case "@loginType":
					def = "SQL";
					break;
				case ".":
					def = "N/A";
					break;
			}
			try
			{
                XmlNode nd = _el.SelectSingleNode(setting);
				if (nd != null)
                    ret = nd.InnerText;
			}
			catch
			{
				ret = "";
			}
		
			if (ret=="") ret = def;
			return ret;
		}


		private void SetExtraInfo(string setting, string val)
		{
			if (_el != null)
			{
				if (_el.SelectSingleNode(setting) == null)
				{
					XmlAttribute a = _doc.CreateAttribute(setting.Replace("@", ""));
					_el.Attributes.Append(a);
				}
				_el.SelectSingleNode(setting).InnerText = val;
				
			}
		}



		/// <summary>
		/// String representation of the object.
		/// </summary>
		/// <returns>The description of the connection.</returns>
		public override string ToString()
		{
			return this.Description;
		}

		#endregion

	}

}
