using System;

namespace Fwbs.Documents
{
    [Serializable]
    public sealed class FileClosedException : System.IO.IOException
    {
        public FileClosedException() : base("File is closed.")
        {
        }

        public FileClosedException(string message) : base(message)
        {
        }

        public FileClosedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        private FileClosedException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
