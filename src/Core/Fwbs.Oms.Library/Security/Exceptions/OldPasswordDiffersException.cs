using System;

namespace FWBS.OMS.Security
{
    /// <summary>
    /// A password was unable to be changed due to the specified old password differing with the existing one.
    /// </summary>
    public sealed class OldPasswordDiffersException : PermissionsException
    {
        public OldPasswordDiffersException()
            : base(HelpIndexes.OldPasswordDiffers) { }

        public OldPasswordDiffersException(Exception innerException)
            : base(innerException, HelpIndexes.OldPasswordDiffers) { }
    }
}
