using System;

namespace FWBS.OMS.Security
{
    /// <summary>
    /// Permissions exception.
    /// </summary>
    public class PermissionsException : SecurityException
    {
        public PermissionsException(string code, string message, bool useParser, params string[] param)
            : base(code, message, useParser, param) { }

        public PermissionsException(Exception innerException, string code, string message, bool useParser, params string[] param)
            : base(innerException, code, message, useParser, param) { }

        public PermissionsException(string code, string message, params string[] param)
            : base(code, message, true, param) { }

        public PermissionsException(Exception innerException, string code, string message, params string[] param)
            : base(innerException, code, message, true, param) { }


        public PermissionsException(HelpIndexes helpID, params string[] param)
            : base(helpID, param) { }
        public PermissionsException(Exception innerException, HelpIndexes helpID, params string[] param)
            : base(innerException, helpID, param) { }
    }
}
