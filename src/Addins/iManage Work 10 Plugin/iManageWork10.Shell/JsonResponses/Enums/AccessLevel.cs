using System.Runtime.Serialization;

namespace iManageWork10.Shell.JsonResponses.Enums
{
    public enum AccessLevel
    {
        [EnumMember(Value = "no_access")]
        NoAccess,
        [EnumMember(Value = "read")]
        Read,
        [EnumMember(Value = "read_write")]
        ReadWrite,
        [EnumMember(Value = "full_access")]
        FullAccess,
        [EnumMember(Value = "change_security")]
        ChangeSecurity,
        [EnumMember(Value = "unknown")]
        Unknown
    }
    
}