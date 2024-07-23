namespace FWBS.OMS.Security.Permissions
{
    public sealed class ContactPermission : Permission
    {
        public ContactPermission(Contact contact, string permissionType)
            : base(contact, permissionType)
        {
        }

        public ContactPermission(Contact contact, StandardPermissionType permissionType)
            : base(contact, permissionType)
        {
        }

        protected override bool Supports(StandardPermissionType type)
        {
            switch (type)
            {
                case StandardPermissionType.List:
                case StandardPermissionType.Read:
                case StandardPermissionType.Update:
                case StandardPermissionType.Delete:
                case StandardPermissionType.UpdatePermissions:
                    return true;
            }

            return false;
        }
    }
}
