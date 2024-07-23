namespace FWBS.OMS.Security.Permissions
{
    public sealed class AssociatePermission : Permission
    {
        public AssociatePermission(Associate associate, string permissionType)
            : base(associate, permissionType)
        {
        }

        public AssociatePermission(Associate associate, StandardPermissionType permissionType)
            : base(associate, permissionType)
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
                    return true;
            }

            return false;
        }
    }
}
