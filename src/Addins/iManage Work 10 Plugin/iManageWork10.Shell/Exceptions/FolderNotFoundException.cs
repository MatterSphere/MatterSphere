using System;

namespace iManageWork10.Shell.Exceptions
{
    public class FolderNotFoundException : Exception
    {
        public FolderNotFoundException()
        {}

        public FolderNotFoundException(string message) : base(message)
        {}
    }
}
