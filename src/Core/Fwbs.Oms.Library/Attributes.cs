using System;

namespace FWBS.OMS.EnquiryEngine
{
	/// <summary>
	/// Attribute that flags classes, methods, properties as enquiry usable.
	/// This COULD be used by the enquiry engine to do a precheck on these programmatic items
	/// before continuing, but does not as of yet. 
	/// Or the windows enquiry from designer could loop through the business layer displaying what properties
	/// class, and methods can be used.
	/// </summary>
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple=false)]
	public sealed class EnquiryUsageAttribute : Attribute
	{
        
        /// <summary>
		/// Flag to store whether the attribute target is intended to be used in
		/// enquiries or not.
		/// </summary>
		private bool _useable = false;

		/// <summary>
		/// Constructs the attribute passing the usable flag.
		/// </summary>
		/// <param name="usable">Usable flag for the item.</param>
		public EnquiryUsageAttribute(bool usable)
		{
			_useable = usable;
		}

		/// <summary>
		/// Gets the usable flag.  If false then the item is not intended to be used within enquiries.
		/// </summary>
		public bool Usable
		{
			get
			{
				return _useable;
			}
		}
	}

}

namespace FWBS.OMS
{
	/// <summary>
	/// Attribute that flags an assembly that it is a valid licensable client for the
	/// OMS system.
	/// </summary>
	[AttributeUsage(AttributeTargets.Assembly, AllowMultiple=false)]
	public sealed class AssemblyAPIClientAttribute : Attribute
	{
		/// <summary>
		/// A unique identifier that describes the uniqueness of the assembly.
		/// </summary>
		private readonly Guid _appKey;

        /// <summary>
        /// Constructs the attribute passing the unique identifier.
        /// </summary>
        /// <param name="applicationKey">Application key GUID.</param>
        public AssemblyAPIClientAttribute(string applicationKey)
		{
            if (!Guid.TryParse(applicationKey, out _appKey))
                throw new FormatException(Session.CurrentSession.Resources.GetMessage("MSGASKMTBEVLD", "The assembly api key must be a valid GUID format.", "").Text);
		}

		/// <summary>
		/// Gets the application key unique identifier value.
		/// </summary>
		public Guid ApplicationKey
		{
			get
			{
				return _appKey;
			}
		}

	}


	/// <summary>
	/// Overrided Localized Category Attribute
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class LocCategoryAttribute : System.ComponentModel.CategoryAttribute 
	{
		public LocCategoryAttribute(string categoryKey) : 
			base(categoryKey)
		{
		}

		protected override string GetLocalizedString(string key) 
		{
			if (Session.CurrentSession.IsLoggedIn)
			{
				string ret = FWBS.OMS.CodeLookup.GetLookup("Category",key);
				if (ret == "") 
					ret = "#" + key + "#";
				return Session.CurrentSession.Terminology.Parse(ret,true);
			}
			else
				return key;
		}
	}

	/// <summary>
	/// A code lookup attribute that holds a code to a resourced code lookup.
	/// </summary>
	[AttributeUsage(AttributeTargets.All)]
	public class LookupAttribute : Attribute
	{
		string _code = "";

		public LookupAttribute(string code)
		{
			_code = code;
		}

		public string Code
		{
			get
			{
				return _code;
			}
		}
	}

	/// <summary>
	/// Script Type Code to be used with the Script List Class in the Admin Kit
	/// </summary>
	public class ScriptTypeParamAttribute : Attribute
	{
		string _code = "";

		public ScriptTypeParamAttribute(string code)
		{
			_code = code;
		}

		public string Code
		{
			get
			{
				return _code;
			}
		}
	}
}