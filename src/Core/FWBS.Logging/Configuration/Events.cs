#region References
using System;
#endregion

namespace FWBS.Configuration
{
	#region Delegates
	public delegate void ConfigurationChangingHandler(object sender, ConfigurationChangingArgs e);
	public delegate void ConfigurationChangedHandler(object sender, ConfigurationChangedArgs e);
	#endregion

	#region ConfigurationChangeType enums
	public enum ConfigurationChangeType
	{
		Name,
		ReadOnly,
		SetValue,
		RemoveEntry,
		RemoveSection,
		Other
	}
	#endregion

	#region ConfigurationChangedArgs
	public class ConfigurationChangedArgs : EventArgs
	{
		#region Constructor
		public ConfigurationChangedArgs(ConfigurationChangeType changeType, string section, string entry, object value) 
		{
			this.ChangeType = changeType;
			this.Section = section;
			this.Entry = entry;
			this.Value = value;
		}
		#endregion

		#region Properties
		public ConfigurationChangeType ChangeType { get; private set; }
		public string Section { get; private set; }
		public string Entry { get; private set; }
		public object Value { get; private set; }
		#endregion
	}
	#endregion

	#region ConfigurationChangingArgs
	public class ConfigurationChangingArgs : ConfigurationChangedArgs
	{
		#region Constructor
		public ConfigurationChangingArgs(ConfigurationChangeType changeType, string section, string entry, object value) :
			base(changeType, section, entry, value)
		{
		}
		#endregion

		#region Properties
		public bool Cancel {get; set; }
		#endregion
	}
	#endregion
}
