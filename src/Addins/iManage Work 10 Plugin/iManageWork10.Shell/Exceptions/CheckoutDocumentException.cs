using System;

namespace iManageWork10.Shell.Exceptions
{
    public class CheckoutDocumentException : Exception
    {
        public CheckoutDocumentException()
        {}

        public CheckoutDocumentException(string message) : base(message)
        {}
    }
}
