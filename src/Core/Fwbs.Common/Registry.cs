using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace FWBS.Common
{
    /// <summary>
    /// Manipulates the registry as a wrapper around the exposed Microsoft Library of Microsoft.Win32.Registry.
    /// It is a sealed class with static methods so it can be used off the class rather than instantiated.
    /// </summary>
    /// 

    sealed public class RegistryAccess
	{	

		/// <summary>
		/// Used to not allow an instance of the object to be created. There is no point due to all menbers being static.
		/// </summary>
		private RegistryAccess(){}

		/// <summary>
		/// Returns the relevant registry hive based on what information is passed.  
		/// It also returns a reference to a remote key if a computer name is specified.
		/// </summary>
		/// <param name="hKey">Root registry key enumerated value.</param>
		/// <param name="computerName">Name of remote computer to connect to. An empty string assumes the local machine.</param>
		/// <returns>A reference to a root key for the registry base class to manipulate.</returns>
		/// <remarks>This is used internally as a private method.</remarks>
		static private RegistryKey GetHKey (RegistryHive hKey, string computerName)
		{
			RegistryKey key = Registry.LocalMachine;

			if (computerName == "") 
			{
				switch (hKey)
				{
					case RegistryHive.ClassesRoot:
						key = Registry.ClassesRoot;
						break;
					case RegistryHive.CurrentConfig:
						key = Registry.CurrentConfig;
						break;
					case RegistryHive.CurrentUser: 
						key = Registry.CurrentUser;
						break;
#pragma warning disable 0618
					case RegistryHive.DynData:
						key = Registry.DynData;
						break;
#pragma warning restore 0618
					case RegistryHive.LocalMachine:
						key = Registry.LocalMachine;
						break;
					case RegistryHive.PerformanceData:
						key = Registry.PerformanceData;
						break;
					case RegistryHive.Users:
						key = Registry.Users;
						break;
				}
			}
			else
				key = RegistryKey.OpenRemoteBaseKey(hKey, computerName);


			return key;
		}


		/// <summary>
		/// Retieves a value from a specified registry key and value.
		/// </summary>
		/// <param name="computerName">Remote computer to access, empty string assumes default.</param>
		/// <param name="hKey">Specifies the root registry key like HKEY_LOCAL_MACHINE etc...</param>
		/// <param name="subKey">Sub keys to access, with no trailing or preceding backslash, e.g, <c>@"SOFTWARE\FWBS\OMS"</c></param>
		/// <param name="val">The value to access within the sub key.  e.g, OPMODE</param>
		/// <returns>Returns an object which allows for different data types like <c>string, int</c> etc...</returns>
		static public object GetSetting(string computerName, RegistryHive hKey, string subKey, string val)
		{
			RegistryKey key = GetHKey(hKey, computerName);

			string [] regKeys = subKey.Split(@"\".ToCharArray());
			
			foreach (string regKey in regKeys)
			{
				try
				{
					key = key.OpenSubKey(regKey);
					if (key == null)
						return null;
				}
				catch 
				{
					return null;
				}
			}
			
			object ret = key.GetValue(val);
			key.Close();
			return ret;
		}

		/// <summary>
		/// Reads a registry setting based on the given hive and sub key information.
		/// </summary>
		/// <param name="hKey">Root key.</param>
		/// <param name="subKey">Sub key tree.</param>
		/// <param name="val">Value item to read.</param>
		/// <returns>Data from the registry.</returns>
		static public object GetSetting(RegistryHive hKey, string subKey, string val)
		{
			return GetSetting("", hKey, subKey, val);
		}


		/// <summary>
		/// Writes to the registry at a specified sub key and value item.
		/// </summary>
		/// <param name="computerName">Remote computer to access, empty string assumes default.</param>
		/// <param name="hKey">Specifies the root registry key like HKEY_LOCAL_MACHINE etc...</param>
		/// <param name="subKey">Sub keys to access, with no trailing or preceding backslash, e.g, <c>@"SOFTWARE\FWBS\OMS"</c></param>
		/// <param name="val">The value to access within the sub key.  e.g, OPMODE</param>
		/// <param name="data">Actual value to write to the registry, this can be any data type.</param>
		static public void SetSetting(string computerName, RegistryHive hKey, string subKey, string val, object data)
		{
			RegistryKey key = GetHKey(hKey, computerName);

			string [] regKeys = subKey.Split(@"\".ToCharArray());
			
			foreach (string regKey in regKeys)
			{
				key = key.CreateSubKey(regKey);
			}

			key.SetValue(val, data);
			key.Close();

		}

		/// <summary>
		/// Write registry information to a specific key.
		/// </summary>
		/// <param name="hKey">Root registry key.</param>
		/// <param name="subKey">Sub key tree.</param>
		/// <param name="val">Registry item to wrtie to.</param>
		/// <param name="data">Data to be written to the registry.</param>
		static public void SetSetting(RegistryHive hKey, string subKey, string val, object data)
		{
			SetSetting("", hKey, subKey, val, data);
		}


		/// <summary>
		/// Deletes a value setting at a specific sub key within the registry.
		/// </summary>
		/// <param name="computerName">Remote computer to access, empty string assumes default.</param>
		/// <param name="hKey">Specifies the root registry key like HKEY_LOCAL_MACHINE etc...</param>
		/// <param name="subKey">Sub keys to access, with no trailing or preceding backslash, e.g, <c>@"SOFTWARE\FWBS\OMS"</c></param>
		/// <param name="val">The value to access within the sub key.  e.g, OPMODE</param>
		static public void DeleteSetting(string computerName, RegistryHive hKey, string subKey, string val)
		{
			RegistryKey key = GetHKey(hKey, computerName);

			string [] regKeys = subKey.Split(@"\".ToCharArray());
			
			foreach (string regKey in regKeys)
			{
				key = key.OpenSubKey(regKey, true);
			}

			key.DeleteValue(val);
			key.Close();
		}

		/// <summary>
		/// Deletes a registry value.
		/// </summary>
		/// <param name="hKey">Root registry key.</param>
		/// <param name="subKey">Sub key tree.</param>
		/// <param name="val">Value item to delete.</param>
		static public void DeleteSetting(RegistryHive hKey, string subKey, string val)
		{
			DeleteSetting("", hKey, subKey, val);
		}



		/// <summary>
        /// Deletes a registry key and all child keys.
		/// </summary>
		/// <param name="computerName">Remote computer to access, empty string assumes default.</param>
		/// <param name="hKey">Specifies the root registry key like HKEY_LOCAL_MACHINE etc...</param>
		/// <param name="subKey">Sub key to delete, with no trailing or preceding backslash, e.g, <c>@"SOFTWARE\FWBS\OMS"</c></param>
		static public void DeleteKey(string computerName, RegistryHive hKey, string subKey)
		{
			RegistryKey key = GetHKey(hKey, "");
			key.DeleteSubKeyTree(subKey);
		}

		/// <summary>
		/// Deletes a registry key and all child keys.
		/// </summary>
		/// <param name="hKey">Root registry key.</param>
		/// <param name="subKey">Sub key tree. This key and child keys will be deleted.</param>
		static public void DeleteKey(RegistryHive hKey, string subKey)
		{
			DeleteKey("", hKey, subKey);
		}


	}


	/// <summary>
	/// An unmanaged sealed class that accesses the PrivateProfileString API directly.  It sets and retrieves
	/// values within INI files.
	/// </summary>
	sealed public class INIFile
	{
		
		/// <summary>
		/// Used to protect the class form be instantiated.  There is no point due to all menbers being static.
		/// </summary>
		private INIFile(){}

		/// <summary>
		/// Windows API to read INI file items.
		/// </summary>
		[DllImport("Kernel32.dll", SetLastError = true)]
		private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

		/// <summary>
		/// Windows API to write INI file items.
		/// </summary>
		[DllImport("Kernel32.dll", SetLastError = true)]
		private static extern int WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);



		/// <summary>
		/// Gets a value from a key and value within an INI file.
		/// </summary>
		/// <param name="filePath">File path location of INI file.</param>
		/// <param name="key">Section header within file.</param>
		/// <param name="val">Value item under the section key header.</param>
		/// <param name="def">Default value that will be returned back if no value is found.</param>
		/// <returns>Returns a string of the value within the INI file.</returns>
		static public string GetSetting (string filePath, string key, string val, string def)
		{
			StringBuilder buf = new StringBuilder(255);
			int ret = GetPrivateProfileString(key, val, def, buf, 255, filePath);
			return buf.ToString();
		}
		
		/// <summary>
		/// Gets a value from an INI file specifying an empty string as the default value.
		/// </summary>
		/// <param name="filePath">Path to file.</param>
		/// <param name="key">Key header within file.</param>
		/// <param name="val">Value item to retrieve.</param>
		/// <returns></returns>
		static public string GetSetting (string filePath, string key, string val)
		{
			return GetSetting(filePath, key, val, "");
		}

		/// <summary>
		/// Writes a value to an INI file.
		/// </summary>
		/// <param name="filePath">Location of INI file.</param>
		/// <param name="key">Key header.</param>
		/// <param name="val">Value under given header.</param>
		/// <param name="data">Actual data to be written to file.</param>
		static public void SetSetting (string filePath, string key, string val, string data)
		{
			int ret = WritePrivateProfileString(key, val, data, filePath);
		}

	}

	/// <summary>
	/// Correctly accesses and writes to the registry by reading from policy and local machine keys first,
	/// then current user key second.  Writing user information to the registry just applies to the current
	/// user key.
	/// </summary>
	public class ApplicationSetting
	{
		/// <summary>
		/// Computer Name used for remote registry connection.
		/// </summary>
		protected string _computer;
		/// <summary>
		/// Product name.
		/// </summary>
		protected string _application;
		/// <summary>
		/// Product version.
		/// </summary>
		protected string _version;
		/// <summary>
		/// Last registry root key used.
		/// </summary>
		protected RegistryHive _hKey;
		/// <summary>
		/// Holds the sub key information after the company, application and version number.
		/// </summary>
		protected string _subKey;
		/// <summary>
		/// Value item to manipulate within the sub key.
		/// </summary>
		protected string _val;
		/// <summary>
		/// Default data to read or write into the registry.
		/// </summary>
		protected object _def;



		/// <summary>
		/// Contructor which applies the default information needed to create an object.
		/// </summary>
		/// <param name="computerName">Remote computer name for registry access.  An empty string will default to the local machine.</param>
		/// <param name="application">Application / Product Name.</param>
		/// <param name="version">Version number of the product.</param>
		/// <param name="subKey">Sub key tree.</param>
		/// <param name="val">Registry item to manipulate</param>
		/// <param name="def">Default value icase no value is returned.</param>
		public ApplicationSetting(string computerName, string application, string version,  string subKey, string val, object def)
		{
			_computer = computerName;
			_application = application;
			_version = version;
			_subKey = subKey;
			while (_subKey.IndexOf(@"\\") > -1)
				_subKey = _subKey.Replace(@"\\", @"\");
			_val = val;
			_def = def;
			GetSetting();

		}

	
		public ApplicationSetting(string application, string version, string subKey, string val, object def) : this ("", application, version, subKey, val, def)
		{
		}
		
		/// <summary>
		/// Creates an application setting object based on an existing one.
		/// </summary>
		/// <param name="appSetting">Existing application setting object.</param>
		/// <param name="subKey">Sub key tree.</param>
		/// <param name="val">Value item.</param>
		/// <param name="def">Default value.</param>
		public ApplicationSetting(ApplicationSetting appSetting, string subKey, string val, object def) : this(appSetting._computer, appSetting._application, appSetting._version, subKey, val, def) 
		{
		}

		public ApplicationSetting(ApplicationSetting appSetting, string val, object def) : this(appSetting._computer, appSetting._application, appSetting._version , appSetting._subKey, val, def) 
		{
		}


		private ApplicationSetting(){}



		/// <summary>
		/// Reads a setting from the registry using the subkeys after the application version number.
		/// </summary>
		/// <param name="userSetting">Forces a user setting under current user.</param>
		/// <returns>The string, number from the registry.</returns>
		public object GetSetting(bool userSetting)
		{
			return ApplicationSetting.GetSetting(userSetting, _computer, _application, _version, _subKey, _val, _def, out _hKey);
		}

	
		public object GetSetting()
		{
			return GetSetting(false);
		}

	


		/// <summary>
		/// Writes a value to the registry.
		/// </summary>
		/// <param name="userSetting">Forces a user setting under current user.</param>
		/// <param name="data">Data to be written.</param>
		public void SetSetting(bool userSetting, object data)
		{
			ApplicationSetting.SetSetting(userSetting,_computer, ref _hKey, _application, _version, _subKey, _val, data);
		}

		public void SetSetting(object data)
		{
			SetSetting(true, data);
		}



		/// <summary>
		/// Static member function which use all data needed in one go to read an application registry
		/// setting.
		/// </summary>
		/// <param name="userSetting">Forces a user setting in current user.</param>
		/// <param name="computerName">Remote Computer Name.</param>
		/// <param name="application">Application / Product Name.</param>
		/// <param name="version">Verion Info.</param>
		/// <param name="subKey">Sub tree of registry keys.</param>
		/// <param name="val">Value item to read.</param>
		/// <param name="def">Default value to return if a value does not exist.</param>
		/// <param name="hKey">Returns a root key.</param>
		/// <returns>An object of the data type within the registry key.</returns>
		static public object GetSetting(bool userSetting, string computerName, string application, string version, string subKey, string val, object def, out RegistryHive hKey)
		{
			string appKey = "";
			string policiesKey = @"SOFTWARE\POLICIES\FWBS";
			string softwareKey = @"SOFTWARE\FWBS";
			object ret;
			
			if (application.Length > 0) appKey += @"\" + application;
			if (version.Length > 0) appKey += @"\" + version;
			if (subKey.Length > 0) appKey += @"\" + subKey;

			policiesKey += appKey;
			softwareKey += appKey;
			

			if (userSetting)
			{
				ret = RegistryAccess.GetSetting(computerName, RegistryHive.CurrentUser, softwareKey, val);
				hKey = RegistryHive.CurrentUser;
			}
			else
			{
				ret = RegistryAccess.GetSetting(computerName, RegistryHive.LocalMachine, policiesKey, val);
				if (ret == null)
				{
					ret = RegistryAccess.GetSetting(computerName, RegistryHive.CurrentUser, policiesKey, val);
					if (ret == null)
					{
						ret = RegistryAccess.GetSetting(computerName, RegistryHive.LocalMachine, softwareKey, val);
						if (ret == null)
						{
							ret = RegistryAccess.GetSetting(computerName, RegistryHive.CurrentUser, softwareKey, val);
							hKey = RegistryHive.CurrentUser;
						}
						else
							hKey = RegistryHive.LocalMachine;
					}
					else
						hKey = RegistryHive.CurrentUser;
				}
				else
					hKey = RegistryHive.LocalMachine;

			}

			if (ret == null)
				return def;
			else
				return ret;
		}


		/// <summary>
		/// A static method that writes a settings to the registry.  It requires all information to work.
		/// </summary>
		/// <param name="userSetting">Forces a user setting in current user.</param>
		/// <param name="computerName">Remote Computer Name.</param>
		/// <param name="hKey">Root registry key to update..</param>
		/// <param name="application">Application / Product Name.</param>
		/// <param name="version">Application version number.</param>
		/// <param name="subKey">Sub tree of registry keys.</param>
		/// <param name="val">Value item to write to.</param>
		/// <param name="data">Actual data to be written to the registry.</param>
		static public void SetSetting(bool userSetting, string computerName, ref RegistryHive hKey, string application, string version, string subKey, string val, object data)
		{
			string appKey = "";
			string softwareKey = @"SOFTWARE\FWBS";

			if (application.Length > 0) appKey += @"\" + application;
			if (version.Length > 0) appKey += @"\" + version;
			if (subKey.Length > 0) appKey += @"\" + subKey;

			softwareKey += appKey;

			if (userSetting)
				hKey = RegistryHive.CurrentUser;
				
			RegistryAccess.SetSetting(computerName, hKey, softwareKey, val, data);

		}

	
		/// <summary>
		/// A read only property that returns the application registry key value.
		/// </summary>
		protected string ApplicationKey
		{
			get
			{
				string appKey = "FWBS";
				if (_application.Length > 0) appKey += @"\" + _application;
				if (_version.Length > 0) appKey += @"\" + _version;
				return appKey;
			}
		}

		/// <summary>
		/// Exposes the root key used.
		/// </summary>
		public RegistryHive RootKey
		{
			get{return _hKey;}
		}

			
		/// <summary>
		/// Gets the registry value as a string type.
		/// </summary>
		/// <returns>Registry data item retuned as a string.</returns>
		public override string ToString()
		{
			return GetSetting().ToString();
		}


		/// <summary>
		/// Makes sure that the return value is a boolean, does not raise an exception.
		/// </summary>
		/// <returns>True if the value is true.</returns>
		public bool ToBoolean()
		{
			bool ret = false;
			object val = null;
			val = GetSetting();
			if (val.ToString() == "True")
				ret = true;
			return ret;
		}


		/// <summary>
		/// A read only property that exposes the value name being manipulated.
		/// </summary>
		public string Value
		{
			get{return _val;}
		}

		/// <summary>
		/// A read only property that reads the Sub key information string that
		/// </summary>
		public string Key
		{
			get
			{
				string key = ApplicationKey;
				if (!key.EndsWith(@"\"))
					key += @"\";
				key += _subKey;
				return key;
			}
		}
		
		/// <summary>
		/// Default value to use.  This can be changed through this property.
		/// </summary>
		public object DefaultValue
		{
			get{return _def;}
			set {_def = value;}
		}
	}


}


