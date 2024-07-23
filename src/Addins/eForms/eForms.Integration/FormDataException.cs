using System;

namespace FWBS.OMS
{
    [global::System.Serializable]
    public class FormDataException : Exception
    {
        public FormDataException() { }
        public FormDataException(string message) : base(message) { }
        public FormDataException(string message, Exception inner) : base(message, inner) { }
        protected FormDataException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    [global::System.Serializable]
    public class FormDataUnknownTypeException : FormDataException
    {
        public FormDataUnknownTypeException() { }
        public FormDataUnknownTypeException(string message) : base(message) { }
        public FormDataUnknownTypeException(string message, Exception inner) : base(message, inner) { }
        protected FormDataUnknownTypeException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
   