using System;

namespace iManageWork10.Shell.Exceptions
{
    public class WorkspaceNotFoundException : Exception
    {
        public WorkspaceNotFoundException()
        {}

        public WorkspaceNotFoundException(string message) : base(message)
        {}
    }
}
