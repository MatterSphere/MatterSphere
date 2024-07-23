using System;

namespace FWBS.OMS
{
    public class CommonObjectException : OMSException2
    {
        public CommonObjectException(string message) : base(message.GetHashCode().ToString("X"), message) { }
        public CommonObjectException(string message, Exception inner) : base(message.GetHashCode().ToString("X"), message, inner) { }
        public CommonObjectException(string code, string message, bool parse, params string[] pars) : base(code, message, null, parse, pars) { }
        public CommonObjectException(string code, string message, Exception innerException, bool parse, params string[] pars) : base(code, message, innerException, true, pars) { }
        public CommonObjectException(string code, string message, Exception innerException, params string[] pars) : base(code, message, innerException, true, pars) { }

    }

    public class PartialStateCommonObjectException : CommonObjectException
    {
        public PartialStateCommonObjectException() 
            : base("ERROBJPARTIAL", "The object is currently in a partial state. Please execute the fetch or create commands.", false)
        { }
    }

    public class MissingCommonObjectException : CommonObjectException
    {
        public MissingCommonObjectException(object id)
            : base("10001", "The specified object '%1%' does not exist within the system", null, false, id.ToString())
        {
        }
    }


}
