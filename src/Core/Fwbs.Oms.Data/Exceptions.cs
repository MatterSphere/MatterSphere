using System;
using System.Diagnostics;

namespace FWBS.OMS.Data.Exceptions
{

    public enum HelpIndexes
	{
		InvalidLogin = 4,
		InvalidServer = 5,
		MissingDatabase = 6,
		DeniedPermissions = 7,
		UnSupportedDataProvider = 22
	}

	/// <summary>
	/// Application scope exception.
	/// </summary>
	abstract public class DataException : ApplicationException
	{

		public DataException(HelpIndexes helpID, params string [] param) : this (null, helpID, param){}

		public DataException(Exception innerException, HelpIndexes helpID, params string [] param) : base (Global.GetResString(helpID.ToString(), param), innerException)
		{
			this.HelpLink = Convert.ToString((int)helpID);
			Trace.WriteLineIf(Global.LogSwitch.TraceError, base.Message, Global.LogSwitch.DisplayName);

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
	/// User does not exist within SQL or password is incorrect.
	/// </summary>
	public class UnSupportedDataProviderException: DataException
	{
		public UnSupportedDataProviderException(string provider) 
			: base (HelpIndexes.UnSupportedDataProvider, provider){}
		
		public UnSupportedDataProviderException(string provider, Exception innerException) 
			: base (innerException, HelpIndexes.UnSupportedDataProvider, provider){}

	}

	/// <summary>
	/// User does not exist within SQL or password is incorrect.
	/// </summary>
	public class InvalidLoginException: DataException
	{
		public InvalidLoginException (string userName, string server) 
			: base (HelpIndexes.InvalidLogin, userName, server){}
		
		public InvalidLoginException (string userName, string server, Exception innerException) 
			: base (innerException, HelpIndexes.InvalidLogin, userName, server){}

	}

	/// <summary>
	/// SQL server does not appear to be registered on the network.
	/// </summary>
	public class InvalidServerException : DataException
	{		
		public InvalidServerException (string server) 
			: base (HelpIndexes.InvalidServer, server){}
		
		public InvalidServerException (string server, Exception innerException) 
			: base (innerException, HelpIndexes.InvalidServer, server){}

	}


	/// <summary>
	/// The database name provided does not exist on the specified instance of SQL server.
	/// </summary>
	public class MissingDatabaseException : DataException
	{
		public MissingDatabaseException (string server, string userName) 
			: base (HelpIndexes.MissingDatabase, server, userName){}
		
		public MissingDatabaseException (string server, string userName, Exception innerException) 
			: base (innerException, HelpIndexes.MissingDatabase, server, userName){}
	}


	/// <summary>
	/// Lack of SQL permissions by the user logged into SQL server.
	/// </summary>
	public class DeniedPermissionsException : DataException
	{
		public DeniedPermissionsException (string userName) 
			: base (HelpIndexes.DeniedPermissions, userName){}
		
		public DeniedPermissionsException (string userName, Exception innerException) 
			: base (innerException, HelpIndexes.DeniedPermissions, userName){}
	}

    /// <summary>
    /// DataException when Execution Fails
    /// </summary>
    public class ConnectionException : Exception
    {
        public ConnectionException(string message, Exception innerException)
            : base(message, innerException)
        {}
    }
    
}
