using System;

namespace FWBS.Common
{
	/// <summary>
	/// A Common FWBS application exception.
	/// </summary>
	public class FWBSException : Exception
	{
		#region Fields

		/// <summary>
		/// Unique Exception Code.
		/// </summary>
		private string _code = "";
		

		#endregion

		#region Constructors

		public FWBSException()
		{
		}

		public FWBSException (string code, string message, params string[] param) : base (GetRes(code, message, param).Text)
		{
			_code = code;
		}


		public FWBSException (string code, string message, Exception innerException, params string[] param) : base(GetRes(code, message, param).Text, innerException)
		{
			_code = code;
		}


		#endregion

		#region Static Methods

		private static Resources.ResourceItemAttribute GetRes(string code, string message, params string [] param)
		{
			Resources.ResourceItemAttribute res = new Resources.ResourceItemAttribute(code, message);
			return Resources.ResourceLookup.GetMessage(res, param);
		}

		#endregion
	}

	namespace Security
	{
		/// <summary>
		/// Security related exceptions.
		/// </summary>
		public class SecurityException : FWBSException
		{

			public SecurityException(string code, string message, params string [] param) 
				: base(code, message, param){}

			public SecurityException(string code, string message, Exception innerException, params string [] param) 
				: base(code, message, innerException, param){}
		
		}

		/// <summary>
		/// An exception that will be raised when critical information is tried to be accessed.
		/// </summary>
		public class AccessCiriticalDataException : SecurityException
		{
			private const string CODE = "CRITDATAACC";
			private const string MESSAGE = "The calling application does not have access to the data specified: {0}";

			public AccessCiriticalDataException(string dataName) 
				: base (CODE, MESSAGE, dataName){}

			public AccessCiriticalDataException(string dataName, Exception innerException) 
				: base (CODE, MESSAGE, innerException, dataName){}

		}

		#region Login Exceptions

		/// <summary>
		/// Login related exceptions.
		/// </summary>
		public class LoginException : SecurityException
		{
			public LoginException(string code, string message, params string [] param) 
				: base(code, message, param){}

			public LoginException(string code, string message, Exception innerException, params string [] param) 
				: base(code, message, innerException, param){}
		}

	
		/// <summary>
		/// User does not exist within OMS databse.
		/// </summary>
		public class InvalidUserException : LoginException
		{
			private const string CODE = "INVALIDUSER";
			private const string MESSAGE = "The user '{0}' does not exist within the database.";

			public InvalidUserException (string userName) 
				: base (CODE, MESSAGE, userName){}
		
			public InvalidUserException (string userName, Exception innerException) 
				: base (CODE, MESSAGE, innerException, userName){}

		}

		/// <summary>
		/// User name and password match with an OMS user but the user is marked as inactive.
		/// </summary>
		public class InactiveUserException : LoginException
		{
			private const string CODE = "INACTIVEUSER";
			private const string MESSAGE = "The user '{0}' has been made inactive.";

			public InactiveUserException (string userName) 
				: base(CODE, MESSAGE, userName){}
		
			public InactiveUserException (string userName,Exception innerException) 
				: base (CODE, MESSAGE, innerException, userName){}
		}

		/// <summary>
		/// User name and password match with an OMS user but the user is marked as inactive.
		/// </summary>
		public class ServiceUserException: LoginException
		{
			private const string CODE = "SERVICEUSER";
			private const string MESSAGE = "The user '{0}' cannot login as it belongs to a service application.";

			public ServiceUserException(string userName) 
				: base(CODE, MESSAGE, userName){}
		
			public ServiceUserException (string userName, Exception innerException) 
				: base (CODE, MESSAGE, innerException, userName){}
		}
	
	
		/// <summary>
		/// Warns the user that they are not logged in if items within session are trying
		/// to be used when the user is not logged in.
		/// </summary>
		public class NotLoggedInException : LoginException
		{
			private const string CODE = "NOTLOGGEDIN";
			private const string MESSAGE = "The current session is not currenlty logged in.";

			public NotLoggedInException() 
				: base (CODE, MESSAGE){}
		
			public NotLoggedInException (Exception innerException) 
				: base (CODE, MESSAGE, innerException){}
		}

		#endregion
	
		#region Permission Exceptions

		/// <summary>
		/// Permissions exception.
		/// </summary>
		public class PermissionsException : SecurityException
		{
			public PermissionsException(string code, string message, params string [] param) 
				: base(code, message, param){}

			public PermissionsException(string code, string message, Exception innerException, params string [] param) 
				: base(code, message, innerException, param){}

		}

		public class CreatePermissionException : PermissionsException
		{
			private const string CODE = "NOCREATEPERM";
			private const string MESSAGE = "You do not have permissions to create a new object.";

			public CreatePermissionException() 
				: base (CODE, MESSAGE){}

			public CreatePermissionException(Exception innerException) 
				: base (CODE, MESSAGE, innerException){}

		}

		public class DeletePermissionException : PermissionsException
		{
			private const string CODE = "NODELETEPERM";
			private const string MESSAGE = "You do not have permissions to delete the object.";

			public DeletePermissionException() 
				: base (CODE, MESSAGE){}

			public DeletePermissionException(Exception innerException) 
				: base (CODE, MESSAGE, innerException){}

		}

		public class UpdatePermissionException : PermissionsException
		{
			private const string CODE = "NOUPDATEPERM";
			private const string MESSAGE = "You do not have permissions to edit / update the object.";

			public UpdatePermissionException() 
				: base (CODE, MESSAGE){}

			public UpdatePermissionException(Exception innerException) 
				: base (CODE, MESSAGE, innerException){}

		}

		public class ViewPermissionException : PermissionsException
		{
			private const string CODE = "NOVIEWPERM";
			private const string MESSAGE = "You do not have permissions to view data from the specified object.";

			public ViewPermissionException() 
				: base (CODE, MESSAGE){}

			public ViewPermissionException(Exception innerException) 
				: base (CODE, MESSAGE, innerException){}

		}

		/// <summary>
		/// A password was unable to be changed due to the specified old password differing with the existing one.
		/// </summary>
		public class OldPasswordDiffersException: PermissionsException
		{
			private const string CODE = "OLDPWDDIFFERS";
			private const string MESSAGE = "The old password specified differs from the one specified.";

			public OldPasswordDiffersException() 
				: base (CODE, MESSAGE){}
		
			public OldPasswordDiffersException (Exception innerException) 
				: base (CODE, MESSAGE, innerException){}
		}

		/// <summary>
		/// The password was incorrect.
		/// </summary>
		public class IncorrectPasswordException : PermissionsException
		{
			private const string CODE = "WRONGPWD";
			private const string MESSAGE = "The specified password is incorrect.";

			public IncorrectPasswordException () 
				: base(CODE, MESSAGE){}
			public IncorrectPasswordException (Exception innerException) 
				: base (CODE, MESSAGE, innerException){}
		}


		/// <summary>
		/// A password was unable to be changed due to the specified confirmation password differing with the new one.
		/// </summary>
		public class ConfirmationPasswordDiffersException: PermissionsException
		{
			private const string CODE = "CFMPWDDIFFERS";
			private const string MESSAGE = "The specified confirmation password differs to the one specified.";

			public ConfirmationPasswordDiffersException() 
				: base (CODE, MESSAGE){}
		
			public ConfirmationPasswordDiffersException (Exception innerException) 
				: base (CODE, MESSAGE, innerException){}
		}

		#endregion


	}
}
