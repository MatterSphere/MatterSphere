using System;

namespace iManageWork10.Shell.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException()
        {}

        public AccessDeniedException(string message) :base(message)
        {}
    }
}
