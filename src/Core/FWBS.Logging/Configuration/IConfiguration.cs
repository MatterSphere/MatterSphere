#region References
using System;
using System.Collections.Generic;
#endregion

namespace FWBS.Configuration
{
	#region IReadOnlyConfiguration
	public interface IReadOnlyConfiguration : ICloneable
	{
		string Name	{ get; }

		object GetValue(string section, string entry);
		string GetValue(string section, string entry, string defaultValue);
		Int32 GetValue(string section, string entry, Int32 defaultValue);
		Int64 GetValue(string section, string entry, Int64 defaultValue);
		double GetValue(string section, string entry, double defaultValue);
		bool GetValue(string section, string entry, bool defaultValue);
		bool HasEntry(string section, string entry);
		bool HasSection(string section);
		HashSet<string> GetEntryNames(string section);
		HashSet<string> GetSectionNames();
	}
	#endregion

	#region IConfiguration
	public interface IConfiguration : IReadOnlyConfiguration
	{
		new string Name { get; set; }
		string DefaultName { get; }
		bool ReadOnly { get; set; }		
		void SetValue(string section, string entry, object value);
		void RemoveEntry(string section, string entry);
		void RemoveSection(string section);

		IReadOnlyConfiguration CloneReadOnly();
		event ConfigurationChangingHandler Changing;
		event ConfigurationChangedHandler Changed;
	}
	#endregion
}
