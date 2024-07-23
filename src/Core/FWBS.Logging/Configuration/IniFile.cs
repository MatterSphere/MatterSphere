#region References
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
#endregion

namespace FWBS.Configuration
{
    public sealed class IniFile : Configuration
	{
		#region The Win32 API methods
		[DllImport("Kernel32.dll", SetLastError = true)]
		static extern int WritePrivateProfileString(string section, string key, string value, string fileName);
		[DllImport("Kernel32.dll", SetLastError = true)]
		static extern int WritePrivateProfileString(string section, string key, int value, string fileName);
		[DllImport("Kernel32.dll", SetLastError = true)]
		static extern int WritePrivateProfileString(string section, int key, string value, string fileName);
		[DllImport("Kernel32.dll", SetLastError = true)]
		static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder result, int size, string fileName);
		[DllImport("Kernel32.dll", SetLastError = true)]
		static extern int GetPrivateProfileString(string section, int key, string defaultValue, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);
		[DllImport("Kernel32.dll", SetLastError = true)]
		static extern int GetPrivateProfileString(int section, string key, string defaultValue, [MarshalAs(UnmanagedType.LPArray)] byte[] result, int size, string fileName);
		#endregion

		#region Constructors
		public IniFile()
		{
		}

		public IniFile(string fileName) :
			base(fileName)
		{
		}

		public IniFile(IniFile iniFile) :
			base(iniFile)
		{
		}
		#endregion

		#region Properties
		public override string DefaultName
		{
			get
			{
				return this.DefaultNameWithoutExtension + ".ini";
			}
		}
		#endregion

		#region Clone
		public override object Clone()
		{
			return new IniFile(this);
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
			this.VerifyName();
			this.VerifyAndAdjustSection(ref section);
			this.VerifyAndAdjustEntry(ref entry);

			if (!this.RaiseChangeEvent(true, ConfigurationChangeType.SetValue, section, entry, value))
			{
				return;
			}

			if (WritePrivateProfileString(section, entry, value.ToString(), Name) == 0)
			{
				throw new Win32Exception();
			}

			this.RaiseChangeEvent(false, ConfigurationChangeType.SetValue, section, entry, value);
		}
		#endregion

		#region GetValue
		public override object GetValue(string section, string entry)
		{
			this.VerifyName();
			this.VerifyAndAdjustSection(ref section);
			this.VerifyAndAdjustEntry(ref entry);

			// Loop until the buffer has grown enough to fit the value
			for (int maxSize = 250; true; maxSize *= 2)
			{
				StringBuilder result = new StringBuilder(maxSize);
				int size = GetPrivateProfileString(section, entry, "", result, maxSize, Name);
				
				if (size < maxSize - 1)
				{
					if (size == 0 && !HasEntry(section, entry))
					{
						return null;
					}
					return result.ToString();
				}
			}
		}
		#endregion

		#region RemoveEntry
		public override void RemoveEntry(string section, string entry)
		{
			// Verify the entry exists
			if (!this.HasEntry(section, entry))
			{
				return;
			}

			this.VerifyNotReadOnly();
			this.VerifyName();
			this.VerifyAndAdjustSection(ref section);
			this.VerifyAndAdjustEntry(ref entry);

			if (!this.RaiseChangeEvent(true, ConfigurationChangeType.RemoveEntry, section, entry, null))
			{
				return;
			}

			if (WritePrivateProfileString(section, entry, 0, Name) == 0)
			{
				throw new Win32Exception();
			}

			this.RaiseChangeEvent(false, ConfigurationChangeType.RemoveEntry, section, entry, null);
		}
		#endregion

		#region RemoveSection
		public override void RemoveSection(string section)
		{
			// Verify the section exists
			if (!this.HasSection(section))
			{
				return;
			}

			this.VerifyNotReadOnly();
			this.VerifyName();
			this.VerifyAndAdjustSection(ref section);

			if (!this.RaiseChangeEvent(true, ConfigurationChangeType.RemoveSection, section, null, null))
			{
				return;
			}

			if (WritePrivateProfileString(section, 0, "", Name) == 0)
			{
				throw new Win32Exception();
			}

			this.RaiseChangeEvent(false, ConfigurationChangeType.RemoveSection, section, null, null);
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
			// Verify the section exists
			if (!this.HasSection(section))
			{
				return null;
			}

			this.VerifyAndAdjustSection(ref section);
				
			// Loop until the buffer has grown enough to fit the value
			for (int maxSize = 500; true; maxSize *= 2)
			{
				byte[] bytes = new byte[maxSize];				
				int size = GetPrivateProfileString(section, 0, "", bytes, maxSize, Name);
				
				if (size < maxSize - 2)
				{
					// Convert the buffer to a string and split it
					string entries = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
					if (entries == "")
					{
						return new HashSet<string>();
					}
					return new HashSet<string>(entries.Split(new char[] {'\0'}));
				}
			}
		}
		#endregion

		#region GetSectionNames
		public override HashSet<string> GetSectionNames()
		{
			// Verify the file exists
			if (!File.Exists(Name))
			{
				return null;
			}
			
			// Loop until the buffer has grown enough to fit the value
			for (int maxSize = 500; true; maxSize *= 2)
			{
				byte[] bytes = new byte[maxSize];				
				int size = GetPrivateProfileString(0, "", "", bytes, maxSize, Name);
				
				if (size < maxSize - 2)
				{
					// Convert the buffer to a string and split it
					string sections = Encoding.ASCII.GetString(bytes, 0, size - (size > 0 ? 1 : 0));
					if (sections == "")
					{
						return new HashSet<string>();
					}
					return new HashSet<string>(sections.Split(new char[] {'\0'}));
				}
			}
		}
		#endregion
	}
}
