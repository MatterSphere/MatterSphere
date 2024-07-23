using System;

namespace FWBS.Common
{
	/// <summary>
	/// An interface that exposes progress status information on an object.
	/// </summary>
	public interface IProgress
	{
		event ProgressEventHandler Progress;
		event EventHandler ProgressStart;
		event EventHandler ProgressFinished;
		event MessageEventHandler ProgressError;
	}

    /// <summary>
    /// An interface for those objects that are password protected.
    /// </summary>
    public interface IPasswordProtected
    {
        // Gets a boolean value that indicates whether the passowrd is internal (stored in DB) or external (protected office document).
        bool IsInternal { get; }
        // Gets or sets the password hint
        string PasswordHint { get; }
        // Gets a boolean value that indicates whether the item has a password on it.
        bool HasPassword { get; }
        // Sets the current password to use on the object.
        string CurrentPassword { set; }
        // Gets the objects string representation for the password request.
        string ToPasswordString();
        // Validates wether the current password is valid and raises an exception if it is not.
        void ValidatePassword();
        // Authenticates a username and password to potentially bypass the password check. Applicable only when IsInternal is true.
        void PasswordAuthenticate(string userName, string password);
    }
}
