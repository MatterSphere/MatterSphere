using System;
using System.Diagnostics;

namespace FWBS.OMS
{



    /// <summary>
    /// Global OMS Application Scope Exception, All OMS exceptions inherit this exception.
    /// </summary>
    //[Obsolete("OMSException2 should be used instead.")]
    public class OMSException : ApplicationException
	{
		//TODO: Remove Help Indexes one weekend.
		private HelpIndexes _helpid;
		private string _code = "";
		private string _property = "";

		public OMSException(string code, string message, string property) : base(Session.CurrentSession.Resources.GetMessage(code,message,"").Text)
		{
			_code = code;
			_property = property;
			Trace.WriteLineIf(Global.LogSwitch.TraceError, base.Message, Global.LogSwitch.DisplayName);
		}
	
		public OMSException(Exception innerException, string code, string message, string property) : base(Session.CurrentSession.Resources.GetMessage(code,message,"").Text,innerException)
		{
			_code = code;
			_property = property;
			Trace.WriteLineIf(Global.LogSwitch.TraceError, base.Message, Global.LogSwitch.DisplayName);
		}
	
		public OMSException(Exception innerException, string code, string message, string property, bool parser, params string [] param) : base(Session.CurrentSession.Resources.GetMessage(code,message,"",parser,param).Text,innerException)
		{
			_code = code;
			_property = property;
			Trace.WriteLineIf(Global.LogSwitch.TraceError, base.Message, Global.LogSwitch.DisplayName);
		}

		public OMSException(string code, string message) : base(Session.CurrentSession.Resources.GetMessage(code,message,"").Text)
		{
			_code = code;
			Trace.WriteLineIf(Global.LogSwitch.TraceError, base.Message, Global.LogSwitch.DisplayName);
		}
	
		public OMSException(Exception innerException, string code, string message) : base(Session.CurrentSession.Resources.GetMessage(code,message,"").Text,innerException)
		{
			_code = code;
			Trace.WriteLineIf(Global.LogSwitch.TraceError, base.Message, Global.LogSwitch.DisplayName);
		}
	
		public OMSException(Exception innerException, string code, string message, bool parser, params string [] param) : base(Session.CurrentSession.Resources.GetMessage(code,message,"",parser,param).Text,innerException)
		{
			_code = code;
			Trace.WriteLineIf(Global.LogSwitch.TraceError, base.Message, Global.LogSwitch.DisplayName);
		}

		//TODO: Remove
		public OMSException(HelpIndexes helpID, params string [] param) : this (null, helpID, true, param){}
		//TODO: Remove
		public OMSException(Exception innerException, HelpIndexes helpID, params string [] param) : this (innerException, helpID, true, param){}
		//TODO: Remove
		public OMSException(HelpIndexes helpID, bool useParser, params string [] param) : this (null, helpID, useParser, param){}
		//TODO: Remove
		public OMSException(Exception innerException, HelpIndexes helpID, bool useParser, params string [] param) : base (Global.GetResString(helpID.ToString(), useParser, param), innerException)
		{
			this.HelpLink = Convert.ToString((int)helpID);
			_helpid = helpID;
			Trace.WriteLineIf(Global.LogSwitch.TraceError, base.Message, Global.LogSwitch.DisplayName);
		}


		//TODO: Remove
		public HelpIndexes HelpID
		{
			get
			{
				return _helpid;
			}
		}
		
		public string Code
		{
			get
			{
				return _code;
			}
		}

		public override string HelpLink
		{
			get
			{
				return base.HelpLink;
			}
			set 
			{
				base.HelpLink = Global.Help.GetSetting().ToString() + value;
			}

		}
	}


	
	/// <summary>
	/// Global OMS Application Scope Exception, All OMS exceptions inherit this exception.
	/// </summary>
	public class OMSException2 : ApplicationException
	{
		private string _errcode = "";
		private string _errdesc = "";
		private string _errproperty = "";

		public OMSException2(string errCode, string errDescription, string errProperty) : base(Session.CurrentSession.Resources.GetMessage(errCode,errDescription,"").Text)
		{
			_errcode = errCode;
			_errdesc = errDescription;
			_errproperty = errProperty;
		}
		public OMSException2(string errCode, string errDescription, string errProperty, Exception innerException) : base(Session.CurrentSession.Resources.GetMessage(errCode,errDescription,"").Text,innerException)
		{
			_errcode = errCode;
			_errdesc = errDescription;
			_errproperty = errProperty;
		}
		public OMSException2(string errCode, string errDescription, string errProperty, Exception innerException, bool parser, params string [] param) : base(Session.CurrentSession.Resources.GetMessage(errCode,errDescription,"",parser,param).Text,innerException)
		{
			_errcode = errCode;
			_errdesc = errDescription;
			_errproperty = errProperty;
		}

		public OMSException2(string errCode, string errDescription) : base(Session.CurrentSession.Resources.GetMessage(errCode,errDescription,"").Text)
		{
			_errcode = errCode;
			_errdesc = errDescription;
		}
		public OMSException2(string errCode, string errDescription, Exception innerException) : base(Session.CurrentSession.Resources.GetMessage(errCode,errDescription,"").Text,innerException)
		{
			_errcode = errCode;
			_errdesc = errDescription;
		}
		public OMSException2(string errCode, string errDescription, Exception innerException, bool parser, params string [] param) : base(Session.CurrentSession.Resources.GetMessage(errCode,errDescription,"",parser,param).Text,innerException)
		{
			_errcode = errCode;
			_errdesc = errDescription;
		}

		public string Code
		{
			get
			{
				return _errcode;
			}
		}

		public string Property
		{
			get
			{
				return _errproperty;
			}
		}
		
		public string Description
		{
			get
			{
				return _errdesc;
			}
		}
	}


    public class PackageException : OMSException2
    {
        public PackageException(string errCode, string errDescription, string errProperty) 
            : base(errCode, errDescription, errProperty)
		{
			
		}
		public PackageException(string errCode, string errDescription, string errProperty, Exception innerException) 
            : base(errCode, errDescription, errProperty, innerException) 
		{
			
		}
		public PackageException(string errCode, string errDescription, string errProperty, Exception innerException, bool parser, params string [] param) 
            : base(errCode, errDescription, errProperty, innerException, parser, param) 
		{
			
		}

		public PackageException(string errCode, string errDescription)
            : base(errCode, errDescription)
		{
		}
		public PackageException(string errCode, string errDescription, Exception innerException) 
            : base( errCode,  errDescription, innerException) 
		{
			
		}
        public PackageException(string errCode, string errDescription, Exception innerException, bool parser, params string[] param)
            : base (errCode, errDescription, innerException, parser, param)
		{
		}
    }

	
	/// <summary>
	/// An exception that deals with all external data manipulation errors.
	/// </summary>
	public class ExtendedDataException : OMSException
	{
		public ExtendedDataException (HelpIndexes helpID, params string [] param) 
			: base (helpID, false, param){}
		public ExtendedDataException (Exception innerException, HelpIndexes helpID, params string [] param) 
			: base (innerException, helpID, false, param){}
	}


	/// <summary>
	/// An exception that deals with all external data source type manipulation errors.
	/// </summary>
	public class OMSTypeException : OMSException
	{
		public OMSTypeException (HelpIndexes helpID, params string [] param) 
			: base (helpID, false, param){}
		public OMSTypeException (Exception innerException, HelpIndexes helpID, params string [] param) 
			: base (innerException, helpID, false, param){}
	}

	public class MailDisabledException : OMSException2
	{
		public MailDisabledException() : base("MAILDISABLED", "Mail is not enabled!", "")
		{
		}

	}


	[Obsolete("This class has been deprecated in V10.1")]
	public class SMSException : OMSException2
	{
		private SMS _sms = null;

		[Obsolete("This method has been deprecated in V10.1")]
		public SMSException(SMS sms, string errCode, string errDescription) : base(errCode,errDescription, "")
		{
			_sms = sms;
		}

		[Obsolete("This method has been deprecated in V10.1")]
		public SMSException(SMS sms, string errCode, string errDescription, Exception innerException) : base(errCode,errDescription,"",innerException)
		{
			_sms = sms;
		}

		[Obsolete("This method has been deprecated in V10.1")]
		public SMSException(SMS sms, string errCode, string errDescription, Exception innerException, bool parser, params string [] param) : base(errCode,errDescription,"",innerException, parser, param)
		{
			_sms = sms;
		}

		[Obsolete("This property has been deprecated in V10.1")]
		public SMS SMS
		{
			get
			{
				return _sms;
			}
		}
	}
}




namespace FWBS.OMS.EnquiryEngine
{

    /// <summary>
    /// Enquiry engine exception.
    /// </summary>
    public class EnquiryException : OMSException
	{
		public EnquiryException  (HelpIndexes helpID, params string [] param) : base (helpID, param){}
		public EnquiryException  (Exception innerException, HelpIndexes helpID, params string [] param) : base (innerException, helpID, param){}
	}


	/// <summary>
	/// An exception that gets raised when an enquiry update has been cancelled.
	/// </summary>
	public class UpdateCancelledException : EnquiryException
	{
		public UpdateCancelledException() 
			: base (HelpIndexes.EnquiryUpdateCancelled){}
		
		public UpdateCancelledException (Exception innerException) 
			: base (innerException, HelpIndexes.EnquiryUpdateCancelled){}

	}

	/// <summary>
	/// Validation information on a question if the validation fails.  Use the fieldname to find the name 
	/// of the control and the page number it resides on if in wizard style.
	/// </summary>
	public class ValidatedField
	{
		public readonly string FieldName;
		public readonly string Description;
		public readonly string Page;

		private ValidatedField(){}

		public ValidatedField (string fieldName, string description, string page)
		{
			FieldName = fieldName;
			Description = description.Replace("&", "").Replace(":", "");
			Page = page;
		}
	}



	/// <summary>
	/// Validation field exception.
	/// </summary>
	public class EnquiryValidationFieldException : EnquiryException
	{
		private ValidatedField _field;
		
		public EnquiryValidationFieldException (HelpIndexes helpID, ValidatedField field) 
			: base (helpID, field.Description)
		{
			_field = field;
		}

		public EnquiryValidationFieldException (Exception innerException, HelpIndexes helpID, ValidatedField field) 
			: base (innerException, helpID, field.Description)
		{
			_field = field;
		}

		public ValidatedField ValidatedField
		{
			get
			{
				return _field;
			}
		}

	}

}


namespace FWBS.OMS.SearchEngine
{

    /// <summary>
    /// Search engine exception.
    /// </summary>
    public class SearchException : OMSException
	{
		public SearchException   (HelpIndexes helpID, params string [] param) : base (helpID, param){}
		public SearchException   (Exception innerException, HelpIndexes helpID, params string [] param) : base (innerException, helpID, param){}
	}
}

namespace FWBS.OMS.Script
{
    /// <summary>
    /// Script engine exception.
    /// </summary>
    public class ScriptException : OMSException
	{
		public ScriptException   (HelpIndexes helpID, params string [] param) : base (helpID, param){}
		public ScriptException   (Exception innerException, HelpIndexes helpID, params string [] param) : base (innerException, helpID, param){}
	}
}

namespace FWBS.OMS.SourceEngine
{

    /// <summary>
    /// An exception that deals with all external data source type manipulation errors.
    /// </summary>
    public class SourceException : OMSException
	{
		public SourceException (HelpIndexes helpID, params string [] param) 
			: base (helpID, false, param){}
		public SourceException (Exception innerException, HelpIndexes helpID, params string [] param) 
			: base (innerException, helpID, false, param){}
	}
}