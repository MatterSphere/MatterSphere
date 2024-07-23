using System;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace FWBS.Common.Reg
{
    /// <summary>
    /// A base application setting object.
    /// </summary>
    public abstract class Setting 
	{
		#region Methods

		public abstract object GetSetting(object def);

		public abstract void SetSetting(object val);

		public abstract void DeleteKey();

		public abstract void DeleteSetting();

		public override string ToString()
		{
			return Convert.ToString(GetSetting(""));
		}

		#endregion

	}

	/// <summary>
	/// Manipulates a registry setting.
	/// </summary>
	public class RegistrySetting : Setting
	{	
		#region Fields

		private RegistryKey _hkey = Registry.LocalMachine;
		private RegistryKey _key = null;
		private string _keyStr = "";
		private string _name = "";

		#endregion

		#region Constructors

		private RegistrySetting(){}

		public RegistrySetting(string computerName, RegistryHive hKey, string key, string name)
		{
			//Make sure that a whole root node is not selected.
			if (key == "") key = "Software";

			while (key.IndexOf(@"\\") > -1)
				key = key.Replace(@"\\", @"\");

			if (computerName == "") 
			{
				switch (hKey)
				{
					case RegistryHive.ClassesRoot:
						_hkey = Registry.ClassesRoot;
						break;
					case RegistryHive.CurrentConfig:
						_hkey = Registry.CurrentConfig;
						break;
					case RegistryHive.CurrentUser: 
						_hkey = Registry.CurrentUser;
						break;
#pragma warning disable 0618
					case RegistryHive.DynData:
						_hkey = Registry.DynData;
						break;
#pragma warning restore 0618
					case RegistryHive.LocalMachine:
						_hkey = Registry.LocalMachine;
						break;
					case RegistryHive.PerformanceData:
						_hkey = Registry.PerformanceData;
						break;
					case RegistryHive.Users:
						_hkey = Registry.Users;
						break;
				}
			}
			else
				_hkey = RegistryKey.OpenRemoteBaseKey(hKey, computerName);

			_keyStr = key;
			_name = name;
		}

		public RegistrySetting(RegistryHive hKey, string key, string name) : this("", hKey, key, name)
		{
		}

		#endregion

		#region Methods



		private void OpenKey (RegistryKey root, string key, bool autocreate, bool writable)
		{
			RegistryKey subkey = root;
			
			string [] keys = key.Split(@"\".ToCharArray());

			int ctr = 0;
			foreach (string k in keys)
			{
				RegistryKey nwkey = null;
				if (autocreate)
					nwkey = subkey.CreateSubKey(k);
				else
					nwkey = subkey.OpenSubKey(k, writable);

				subkey = nwkey;

				if (subkey == null)
					break;
				
				ctr++;
			}

			_key = subkey;
		}


		#endregion
		
		#region Setting

		public override object GetSetting(object def)
		{
			OpenKey(_hkey, _keyStr, false, false);
			if (_key == null)
				return def;
			else
			{
				return _key.GetValue(_name, def);
			}
		}

		public override void SetSetting(object val)
		{
			OpenKey(_hkey, _keyStr, true, true);
			_key.SetValue(_name, val);
		}

		public override void DeleteSetting()
		{
			OpenKey(_hkey, _keyStr, false, true);
			if (_key != null)
			{
				_key.DeleteValue(_name);
				_key.Close();
				_key = null;
			}
		}


		public override void DeleteKey()
		{
			OpenKey(_hkey, _keyStr, false, true);
			if (_key != null)
			{
				_key.DeleteSubKeyTree("");
				_key.Close();
				_key = null;
			}
		}


		#endregion

	}


	/// <summary>
	/// Manipulates an INI file setting.
	/// </summary>
	public class INISetting : Setting
	{
		#region WINAPI

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


		#endregion

		#region Fields

		private string _file = "";
		private string _key = "";
		private string _name = "";

		private const int MAX_BUFFER = 255;

		#endregion

		#region Constructors

		private INISetting(){}

		public INISetting(string file, string key, string name)
		{
			_file = file;
			_key = key;
			_name = name;
		}


		#endregion

		#region Setting

		public override object GetSetting(object def)
		{
			StringBuilder buf = new StringBuilder(MAX_BUFFER);
			int ret = GetPrivateProfileString(_key, _name, Convert.ToString(def), buf, MAX_BUFFER, _file);
			return buf.ToString();
		}

		public override void SetSetting(object val)
		{
			int ret = WritePrivateProfileString(_key, _name, Convert.ToString(val), _file);
		}

		public override void DeleteSetting()
		{
			SetSetting("");
		}


		public override void DeleteKey()
		{
			throw new NotSupportedException(Global.GetResString("DELETENOTSUPP")); //Delete Key is not supported for INISetting
        }


        #endregion

    }


	
    /// <summary>
	/// Correctly accesses and writes to the registry by reading from policy and local machine keys first,
	/// then current user key second.  Writing user information to the registry just applies to the current
	/// user key.
	/// </summary>
	public class ApplicationSetting : Setting
	{
		#region Fields

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
		protected string _key;
		/// <summary>
		/// Value item to manipulate within the sub key.
		/// </summary>
		protected string _name;


		#endregion

		#region Constructors

		private ApplicationSetting(){}

		public ApplicationSetting(string application, string version,  string key, string name)
		{
			_application = application;
			_version = version;
			_key = key;
			while (_key.IndexOf(@"\\") > -1)
				_key = _key.Replace(@"\\", @"\");
			_name = name;
			GetSetting(null);
		}

		#endregion
	
		#region Setting

		public override object GetSetting(object def)
		{
			return GetSetting(false, def);
		}

		public object GetSetting(bool userSetting, object def)
		{
			RegistrySetting reg = null;
			return GetSetting(userSetting, def, out _hKey, out reg);
		}

		private object GetSetting(bool userSetting, object def, out RegistryHive hKey, out RegistrySetting reg)
		{
			string appKey = Key;
			string policiesKey = @"SOFTWARE\POLICIES\FWBS";
			string softwareKey = @"SOFTWARE\FWBS";
			object ret;
		
			policiesKey += appKey;
			softwareKey += appKey;
	
			if (userSetting)
			{
				reg = new RegistrySetting(RegistryHive.CurrentUser, softwareKey, _name);
				ret = reg.GetSetting(null);
				hKey = RegistryHive.CurrentUser;
			}
			else
			{
				reg = new RegistrySetting(RegistryHive.LocalMachine, policiesKey, _name);
				ret = reg.GetSetting(null);
				if (ret == null)
				{
					reg = new RegistrySetting(RegistryHive.CurrentUser, policiesKey, _name);
					ret = reg.GetSetting(null);
					if (ret == null)
					{
						reg = new RegistrySetting(RegistryHive.LocalMachine, softwareKey, _name);
						ret = reg.GetSetting(null);
						if (ret == null)
						{
							reg = new RegistrySetting(RegistryHive.CurrentUser, softwareKey, _name);
							ret = reg.GetSetting(null);
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
	

		public override void SetSetting(object val)
		{
			SetSetting(true, val, ref _hKey);
		}

		public void SetSetting(bool userSetting, object val)
		{
			SetSetting(userSetting, val, ref _hKey);
		}

		private void SetSetting(bool userSetting, object val, ref RegistryHive hKey)
		{
			string appKey = Key;
			string softwareKey = @"SOFTWARE\FWBS";

			softwareKey += appKey;

			if (userSetting)
				hKey = RegistryHive.CurrentUser;
				
			RegistrySetting reg = new RegistrySetting(hKey, softwareKey, _name);
			reg.SetSetting(val);
		}


		public override void DeleteSetting()
		{
			RegistrySetting reg = null;
			GetSetting(false, null, out _hKey, out reg);
			reg.DeleteSetting();
		}


		public override void DeleteKey()
		{
			RegistrySetting reg = null;
			GetSetting(false, null, out _hKey, out reg);
			reg.DeleteKey();
		}

		#endregion

		#region Properties

		private string Key
		{
			get
			{
				string appKey = "";
				if (_application.Length > 0) appKey += @"\" + _application;
				if (_version.Length > 0) appKey += @"\" + _version;
				if (_key.Length > 0) appKey += @"\" + _key;
				return appKey;
			}
		}

		#endregion

	}
}
