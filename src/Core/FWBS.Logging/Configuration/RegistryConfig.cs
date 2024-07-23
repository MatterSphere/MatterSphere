#region References
using System.Collections.Generic;

using Microsoft.Win32;
#endregion

namespace FWBS.Configuration
{
    public class RegistryConfig : Configuration
	{
		#region Fields
		private RegistryKey rootKey = Microsoft.Win32.Registry.CurrentUser;
		#endregion

		#region Constructors
		public RegistryConfig()
		{
		}

		public RegistryConfig(string keyName) :
			base("")
		{
			this.Name = this.DefaultName + @"\" + keyName;
		}

		public RegistryConfig(RegistryKey rootKey, string subKeyName) :
			base("")
		{
			if (rootKey != null)
			{
				this.rootKey = rootKey;
			}
			if (subKeyName != null)
			{
				this.Name = subKeyName;
			}
		}

		public RegistryConfig(RegistryConfig reg) :
			base(reg)
		{
			this.rootKey = reg.rootKey;
		}
		#endregion

		#region Properties
		public override string DefaultName
		{
			get
			{
				return @"Software\FWBS\OMS\Configuration";
			}
		}

		public RegistryKey RootKey
		{
			get 
			{ 
				return this.rootKey; 
			}
			set 
			{
				this.VerifyNotReadOnly();
				if (this.rootKey == value)
				{
					return;
				}

				if (!this.RaiseChangeEvent(true, ConfigurationChangeType.Other, null, "RootKey", value))
				{
					return;
				}
				
				this.rootKey = value;
				this.RaiseChangeEvent(false, ConfigurationChangeType.Other, null, "RootKey", value);
			}
		}
		#endregion

		#region Clone
		public override object Clone()
		{
			return new RegistryConfig(this);
		}
		#endregion

		#region GetSubKey
		protected RegistryKey GetSubKey(string section, bool create, bool writable)		
		{
			this.VerifyName();
			
			string keyName = Name + "\\" + section;

			if (create)
			{
				return this.rootKey.CreateSubKey(keyName);
			}

			return this.rootKey.OpenSubKey(keyName, writable);
		}
		#endregion

		#region SetValue
		public override void SetValue(string section, string entry, object value)
		{
			// If the value is null, remove the entry
			if (value == null)
			{
				this.RemoveEntry(section, entry);
				return;
			}

			this.VerifyNotReadOnly();
			this.VerifyAndAdjustSection(ref section);
			this.VerifyAndAdjustEntry(ref entry);

			if (!this.RaiseChangeEvent(true, ConfigurationChangeType.SetValue, section, entry, value))
				return;

			using (RegistryKey subKey = this.GetSubKey(section, true, true))
			{
				subKey.SetValue(entry, value);
			}

			this.RaiseChangeEvent(false, ConfigurationChangeType.SetValue, section, entry, value);
		}
		#endregion

		#region GetValue
		public override object GetValue(string section, string entry)
		{
			this.VerifyAndAdjustSection(ref section);
			this.VerifyAndAdjustEntry(ref entry);

			using (RegistryKey subKey = this.GetSubKey(section, false, false))
			{
				return (subKey == null ? null : subKey.GetValue(entry));
			}
		}
		#endregion

		#region RemoveEntry
		public override void RemoveEntry(string section, string entry)
		{
			this.VerifyNotReadOnly();
			this.VerifyAndAdjustSection(ref section);
			this.VerifyAndAdjustEntry(ref entry);

			using (RegistryKey subKey = this.GetSubKey(section, false, true))
			{
				if (subKey != null && subKey.GetValue(entry) != null)
				{
					if (!this.RaiseChangeEvent(true, ConfigurationChangeType.RemoveEntry, section, entry, null))
					{
						return;
					}
			
					subKey.DeleteValue(entry, false);
					this.RaiseChangeEvent(false, ConfigurationChangeType.RemoveEntry, section, entry, null);
				}
			}	
		}
		#endregion

		#region RemoveSection
		public override void RemoveSection(string section)
		{
			this.VerifyNotReadOnly();
			this.VerifyName();
			this.VerifyAndAdjustSection(ref section);
			
			using (RegistryKey key = this.rootKey.OpenSubKey(Name, true))
			{
				if (key != null && this.HasSection(section))
				{
					if (!this.RaiseChangeEvent(true, ConfigurationChangeType.RemoveSection, section, null, null))
					{
						return;
					}
					
					key.DeleteSubKeyTree(section);
					this.RaiseChangeEvent(false, ConfigurationChangeType.RemoveSection, section, null, null);
				}
			}	
		}
		#endregion

		#region Remove
		public override void Remove()
		{
			this.VerifyNotReadOnly();
			this.VerifyName();

			HashSet<string> sections = this.GetSectionNames();
			foreach (string section in sections)
			{
				this.RemoveSection(section);
			}
		}
		#endregion

		#region GetEntryNames
		public override HashSet<string> GetEntryNames(string section)
		{
			this.VerifyAndAdjustSection(ref section);

			using (RegistryKey subKey = this.GetSubKey(section, false, false))
			{
				if (subKey == null)
					return null;
				
				return new HashSet<string>(subKey.GetValueNames());
			}				
		}
		#endregion

		#region GetSectionNames
		public override HashSet<string> GetSectionNames()
		{
			this.VerifyName();
			
			using (RegistryKey key = this.rootKey.OpenSubKey(Name))
			{
				if (key == null)
				{
					return null;
				}
				return new HashSet<string>(key.GetSubKeyNames());
			}
		}
		#endregion
	}
}
