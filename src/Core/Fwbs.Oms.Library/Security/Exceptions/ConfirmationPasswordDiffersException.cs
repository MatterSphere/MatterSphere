using System;

namespace FWBS.OMS.Security
{

    /// <summary>
    /// A password was unable to be changed due to the specified confirmation password differing with the new one.
    /// </summary>
    public sealed class ConfirmationPasswordDiffersException : PermissionsException
    {
        public ConfirmationPasswordDiffersException()
            : base(HelpIndexes.ConfirmationPasswordDiffers) { }

        public ConfirmationPasswordDiffersException(Exception innerException)
            : base(innerException, HelpIndexes.ConfirmationPasswordDiffers) { }
    }
}
