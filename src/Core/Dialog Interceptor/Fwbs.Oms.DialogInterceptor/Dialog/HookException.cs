using System;

namespace Fwbs.Oms.DialogInterceptor
{
    [Serializable]
    public class HookException : Exception
    {
        public HookException()
        {
        }

        public HookException(string message) : base(message)
        {
        }

        public HookException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected HookException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) : base(info, context)
        {
        }
    }
}
