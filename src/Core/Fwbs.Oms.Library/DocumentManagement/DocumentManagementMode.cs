using System;

namespace FWBS.OMS.DocumentManagement
{

    /// <summary>
    /// Document Management Operation, used to acknowledge and support the level of interfaces supported by the Document Management System
    /// </summary>
    [Flags()]
    public enum DocumentManagementMode
    {
        None = 0,
        Save = 1,
        Print = 2,
        Open = 4,
        Full = 7
    }
}
