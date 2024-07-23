namespace FWBS.OMS.Security.Permissions
{
    public sealed class DocumentPermission : Permission
    {
        public DocumentPermission(OMSDocument document, string permissionType)
            : base(document, permissionType)
        {
        }

        public DocumentPermission(OMSDocument document, StandardPermissionType permissionType)
            : base(document, permissionType)
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
