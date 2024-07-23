using System;

namespace FWBS.OMS.Commands
{
    [Flags]
    public enum CommandStatus
    {
        None = 0,
        Canceled = 1,
        Failed = 2,
        Success = 4        
    }
}
