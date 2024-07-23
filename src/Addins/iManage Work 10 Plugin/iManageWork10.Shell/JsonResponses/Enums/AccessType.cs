using System.Runtime.Serialization;

namespace iManageWork10.Shell.JsonResponses.Enums
{
    public enum AccessType
    {
        [EnumMember(Value = "user")]
        User,
        [EnumMember(Value = "group")]
        Group
    }
}
