namespace FWBS.OMS.Security.Permissions
{
    public sealed class PrecedentPermission : Permission
    {
        public PrecedentPermission(Precedent precedent, string permissionType)
            : base(precedent, permissionType)
        {
        }

        public PrecedentPermission(Precedent precedent, StandardPermissionType permissionType)
            : base(precedent, permissionType)
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
