#region References
using System;
using System.Collections.Generic;
#endregion

namespace FWBS.Configuration
{
    #region Configuration
    public abstract class Configuration : IConfiguration
	{
		#region Fields
		private string name;
		private bool readOnly;

		public event ConfigurationChangingHandler Changing;
		public event ConfigurationChangedHandler Changed;
		#endregion

		#region Constructors
		protected Configuration()
		{			
			this.name = this.DefaultName;
		}
		
		protected Configuration(string name)
		{			
			this.name = name;
		}

		protected Configuration(Configuration config)
		{
			this.name = config.Name;
			this.readOnly = config.ReadOnly;
			this.Changing = config.Changing;
			this.Changed = config.Changed;
		}
		#endregion

		#region Properties
		public string Name
		{
			get 
			{ 
				return this.name; 
			}
			set 
			{ 
				this.VerifyNotReadOnly();
				if ((this.name == value.Trim()) ||
					(!this.RaiseChangeEvent(true, ConfigurationChangeType.Name, null, null, value)))
				{
					return;
				}

				this.name = value.Trim();
				this.RaiseChangeEvent(false, ConfigurationChangeType.Name, null, null, value);
			}
		}

		public bool ReadOnly
		{
			get 
			{ 
				return this.readOnly; 
			}
			
			set
			{
				this.VerifyNotReadOnly();
				if ((this.readOnly == value) ||
					(!this.RaiseChangeEvent(true, ConfigurationChangeType.ReadOnly, null, null, value)))
				{
					return;
				}

				this.readOnly = value;
				this.RaiseChangeEvent(false, ConfigurationChangeType.ReadOnly, null, null, value);
			}
		}
		#endregion

		#region Abstract bits for derived classes to implement
		public abstract string DefaultName { get; }
		public abstract object Clone();
		public abstract void SetValue(string section, string entry, object value);
		public abstract object GetValue(string section, string entry);

		public abstract void RemoveEntry(string section, string entry);
		public abstract void RemoveSection(string section);
		public abstract void Remove();
		public abstract HashSet<string> GetEntryNames(string section);
		public abstract HashSet<string> GetSectionNames();
		#endregion

		#region GetValue
		public virtual string GetValue(string section, string entry, string defaultValue)
		{
			object value = this.GetValue(section, entry);
			return (value == null ? defaultValue : value.ToString());
		}

		public virtual Int32 GetValue(string section, string entry, Int32 defaultValue)
		{
			object value = this.GetValue(section, entry);
			if (value == null)
			{
				return defaultValue;
			}

			try
			{
				return Convert.ToInt32(value);
			}
			catch 
			{
				return 0;
			}
		}

		public virtual Int64 GetValue(string section, string entry, Int64 defaultValue)
		{
			object value = this.GetValue(section, entry);
			if (value == null)
			{
				return defaultValue;
			}

			try
			{
				return Convert.ToInt64(value);
			}
			catch
			{
				return 0;
			}
		}

		public virtual double GetValue(string section, string entry, double defaultValue)
		{
			object value = this.GetValue(section, entry);
			if (value == null)
			{
				return defaultValue;
			}

			try
			{
				return Convert.ToDouble(value);
			}
			catch 
			{
				return 0;
			}
		}

		public virtual bool GetValue(string section, string entry, bool defaultValue)
		{
			object value = GetValue(section, entry);
			if (value == null)
			{
				return defaultValue;
			}

			try
			{
				return Convert.ToBoolean(value);
			}
			catch 
			{
				return false;
			}
		}
		#endregion

		#region HasEntry
		public virtual bool HasEntry(string section, string entry)
		{
			HashSet<string> entries = GetEntryNames(section);

			if (entries == null)
			{
				return false;
			}

			this.VerifyAndAdjustEntry(ref entry);
			return entries.Contains(entry);
		}
		#endregion

		#region HasSection
		public virtual bool HasSection(string section)
		{
			HashSet<string> sections = GetSectionNames();

			if (sections == null)
			{
				return false;
			}

			this.VerifyAndAdjustSection(ref section);
			return sections.Contains(section);
		}
		#endregion

		#region CloneReadOnly
		public virtual IReadOnlyConfiguration CloneReadOnly()
		{
			Configuration config = (Configuration)Clone();
			config.ReadOnly = true;

			return config;
		}
		#endregion

		#region DefaultNameWithoutExtension
		protected string DefaultNameWithoutExtension
		{
			get
			{
				try
				{
					string file = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
					return file.Substring(0, file.LastIndexOf('.'));
				}
				catch
				{
					return "configuration";  // if all else fails
				}
			}
		}
		#endregion

		#region Verification helpers
		protected virtual void VerifyAndAdjustSection(ref string section)
		{
			if (section == null)
			{
				throw new ArgumentNullException("section");
			}

			section = section.Trim();
		}

		protected virtual void VerifyAndAdjustEntry(ref string entry)
		{
			if (entry == null)
			{
				throw new ArgumentNullException("entry");
			}

			entry = entry.Trim();
		}
		
		protected internal virtual void VerifyName()
		{
			if (string.IsNullOrWhiteSpace(this.name))
			{
				throw new InvalidOperationException("Operation not allowed because Name property is null or empty.");
			}
		}

		protected internal virtual void VerifyNotReadOnly()
		{
			if (this.readOnly)
			{
				throw new InvalidOperationException("Operation not allowed because ReadOnly property is true.");
			}
		}
		#endregion

		#region Event raising
		protected bool RaiseChangeEvent(bool changing, ConfigurationChangeType changeType, string section, string entry, object value)
		{
			if (changing)
			{
				// Don't even bother if there are no handlers.
				if (this.Changing == null)
				{
					return true;
				}

				ConfigurationChangingArgs e = new ConfigurationChangingArgs(changeType, section, entry, value);
				this.OnChanging(e);
				return !e.Cancel;
			}
			
			// Don't even bother if there are no handlers.
			if (this.Changed != null)
			{
				this.OnChanged(new ConfigurationChangedArgs(changeType, section, entry, value));
			}
			return true;
		}

		protected virtual void OnChanging(ConfigurationChangingArgs e)
		{
			if (this.Changing == null)
			{
				return;
			}

			foreach (ConfigurationChangingHandler handler in this.Changing.GetInvocationList())
			{
				handler(this, e);
				
				// If a particular handler cancels the event, stop
				if (e.Cancel)
					break;
			}
		}

		protected virtual void OnChanged(ConfigurationChangedArgs e)
		{
			if (Changed != null)
			{
				this.Changed(this, e);
			}
		}
		#endregion
	}
	#endregion
}
