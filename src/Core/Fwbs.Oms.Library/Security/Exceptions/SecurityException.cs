using System;

namespace FWBS.OMS.Security
{
    /// <summary>
    /// Security related exceptions.
    /// </summary>
    public class SecurityException : OMSException
    {

        public SecurityException(string code, string message, bool useParser, params string[] param)
            : base(null, code, message, useParser, param) { }

        public SecurityException(Exception innerException, string code, string message, bool useParser, params string[] param)
            : base(innerException, code, message, useParser, param) { }

        public SecurityException(string code, string message, params string[] param)
            : base(null, code, message, true, param) { }

        public SecurityException(Exception innerException, string code, string message, params string[] param)
            : base(innerException, code, message, true, param) { }

        public SecurityException(HelpIndexes helpID, bool useParser, params string[] param)
            : base(helpID, useParser, param) { }
        public SecurityException(Exception innerException, HelpIndexes helpID, bool useParser, params string[] param)
            : base(innerException, helpID, useParser, param) { }

        public SecurityException(HelpIndexes helpID, params string[] param)
            : base(helpID, false, param) { }
        public SecurityException(Exception innerException, HelpIndexes helpID, params string[] param)
            : base(innerException, helpID, false, param) { }
    }

}
