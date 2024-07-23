using System;
using XML = System.Xml;

namespace FWBS.OMS
{
	/// <summary>
	/// A base abstract class for those objects that are password protected.
	/// </summary>
	public abstract class PasswordProtectedBase : LookupTypeDescriptor, FWBS.Common.IPasswordProtected
	{
		/// <summary>
		/// A password caching object.
		/// </summary>
		private static XML.XmlDocument _cache = new XML.XmlDocument();

		/// <summary>
		/// The user to be authenticated for a bypass.
		/// </summary>
		private User _authenticated = null;

		/// <summary>
		/// The current password to use.  This is used to enable any changes or modifications
		/// to the item, only if the password is the same as the one stored against the stored
		/// item.
		/// </summary>
		private string _currentPassword = "";

		/// <summary>
		/// Password Changed Event
		/// </summary>
		public event EventHandler PasswordChanged;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public PasswordProtectedBase()
		{
		}

		// Indicates that the passowrd is internal (stored in DB).
		[System.ComponentModel.Browsable(false)]
		public bool IsInternal { get { return true; } }

		/// <summary>
		/// Gets the password of the storage item from the derived object.  This is protected
		/// and abstract so that the password cannot be retrieved from outside the object scope 
		/// itself.
		/// </summary>
		protected abstract string Password{get;set;}

		/// <summary>
		/// Gets the password hint
		/// </summary>
		[EnquiryEngine.EnquiryUsage(true)]
		public abstract string PasswordHint{get;set;}

		/// <summary>
		/// Gets the objects string representation for the password request.
		/// </summary>
		/// <returns></returns>
		public abstract string ToPasswordString();

		/// <summary>
		/// Gets the current password after the cache has been checked.
		/// </summary>
		/// <returns></returns>
		private string GetCurrentPassword()
		{
			return _currentPassword;
		}

		/// <summary>
		/// Allows the user to change the password by passing the old and new one through.
		/// An exception is raised if the password change fails.
		/// </summary>
		/// <param name="oldPassword">Old password to validate against.</param>
		/// <param name="newPassword">New password to set.</param>
		/// <param name="confirmPassword">Confirms the new password.</param>
		public virtual void ChangePassword(string oldPassword, string newPassword, string confirmPassword)
		{ 
			if (oldPassword == "" && newPassword == "" && confirmPassword == "") return;

			if (IsPasswordValid(oldPassword))
			{
				if (newPassword == confirmPassword)
				{
					if (newPassword == "")
						Password = "";
					else
                        Password = FWBS.Common.Security.Cryptography.Encryption.NewKeyEncrypt(newPassword, 25);
					CurrentPassword = newPassword;
					if (PasswordChanged != null)
						PasswordChanged(this,EventArgs.Empty);
				}
				else
					throw new Security.ConfirmationPasswordDiffersException();
			}
			else
				throw new Security.OldPasswordDiffersException();
		}


		/// <summary>
		/// Checks whether the password is correct.
		/// </summary>
		/// <param name="password">Password to compare.</param>
		/// <returns>True if the password matches.</returns>
		public virtual bool IsPasswordValid(string password)
		{
            return (password == FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(Password));
		}

		/// <summary>
		/// Checks whether the current password is correct.
		/// </summary>
		/// <returns>True if the current password matches.</returns>
		public bool IsPasswordValid()
		{
            return IsPasswordValid(FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(GetCurrentPassword()));
		}

		/// <summary>
		/// Validates wether the current password is valid and raises an exception if it is not.
		/// </summary>
		public void ValidatePassword()
		{
            ValidatePassword(FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(GetCurrentPassword()));
		}

		
		/// <summary>
		/// Validates wether the specified password is valid and raises an exception if it is not.
		/// </summary>
		/// <param name="password">Password to compare.</param>
		public virtual void ValidatePassword(string password)
		{
			if (!IsPasswordValid(password))
			{
				throw new Security.InvalidOMSPasswordException();
			}
		}

		/// <summary>
		/// Gets a boolean value that indicates whether the storage item has a password on it.
		/// </summary>
		[System.ComponentModel.Browsable(false)]
		public bool HasPassword
		{
			get
			{
				return (Password != String.Empty);
			}
		}

		/// <summary>
		/// Sets the current password to use on the object.
		/// </summary>
		public string CurrentPassword
		{
			set
			{
                _currentPassword = FWBS.Common.Security.Cryptography.Encryption.NewKeyEncrypt(value, 25);
			}
		}


        /// <summary>
        /// Authenticates a username and password to potentially bypass the password check.
        /// </summary>
        public void PasswordAuthenticate(string userName, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    throw new System.Security.SecurityException("User Name cannot be blank");

                if (string.IsNullOrWhiteSpace(password))
                    throw new System.Security.SecurityException("Password cannot be blank");

                //Validate UserName as password for Authenticated User
                _authenticated = User.AuthenticateUser(userName, password);

                bool authenticatedUserHasValidRole = false;
                foreach (string roleToCheck in PasswordAuthenticatedRoles)
                {
                    if (authenticatedUserHasValidRole)
                        break;
                    foreach (string authenticatedUserRole in _authenticated.Roles.Split(";,".ToCharArray()))
                    {
                        if (authenticatedUserRole == roleToCheck)
                        {
                            authenticatedUserHasValidRole = true;
                            break;
                        }
                    }
                }

                //If a valid role is not found, then raise exception
                if (authenticatedUserHasValidRole == false)
                    throw new System.Security.SecurityException(string.Format("Authenticated user '{0}' does not have any of the roles required to allow access", _authenticated.FullName));

                CurrentPassword = FWBS.Common.Security.Cryptography.Encryption.NewKeyDecrypt(Password);
            }
            catch (Exception ex)
            {
                _authenticated = null;
                throw ex;
            }
        }



		/// <summary>
		/// Gets an array of roles which the authenticated user must have to bypass the password.
		/// </summary>
		protected virtual string[] PasswordAuthenticatedRoles
		{
			get
			{
				return new string [2]{User.ROLE_PARTNER, User.ROLE_ADMIN};
			}
		}
		
	}

	/// <summary>
	/// A standalone password protected objhect class.
	/// </summary>
	public class PasswordProtected : PasswordProtectedBase
	{
		object _parent = null;
		string _password = "";
		string _hint = "";
		string [] _authroles = new string [2]{User.ROLE_PARTNER, User.ROLE_ADMIN};

		public PasswordProtected (object parent, string encryptedPassword, string hint, string [] authenticatedRoles)
		{
			_parent = parent;
			_password = encryptedPassword;
			_hint = hint;
			if (_authroles != null)
				_authroles = authenticatedRoles;
		}

		protected override string Password
		{
			get
			{
				return _password;
			}
			set
			{
				_password = value;
			}
		}

		protected override string[] PasswordAuthenticatedRoles
		{
			get
			{
				return _authroles;
			}
		}

		public override string PasswordHint
		{
			get
			{
				return _hint;
			}
			set
			{
				_hint = value;
			}
		}

		public override string ToPasswordString()
		{
			if (_parent != null)
				return _parent.ToString();
			else
				return "";
		}

		public string ToEncryptedPassword()
		{
			return Password;
		}

	}
	

}
