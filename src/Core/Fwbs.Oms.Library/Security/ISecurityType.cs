namespace FWBS.OMS.Security
{
    public interface ISecurityType
    {
        bool IsGranted(Permissions.Permission permission);
        void ApplyDefaultSettings(ISecurable parent, ISecurable objectToSecure);
        void Refresh(ISecurable securedObject);
    }
}
