namespace FWBS.OMS.Security.Permissions
{
    public sealed class FilePermission : Permission
    {
        public FilePermission(OMSFile file, string permissionType)
            : base(file, permissionType)
        {
        }

        public FilePermission(OMSFile file, StandardPermissionType permissionType)
            : base(file, permissionType)
        {
        }

        protected override bool Supports(StandardPermissionType type)
        {
            switch (type)
            {
                case StandardPermissionType.CreateAssociate:
                case StandardPermissionType.SaveDocument:
                case StandardPermissionType.List:
                case StandardPermissionType.Read:
                case StandardPermissionType.Update:
                case StandardPermissionType.UpdatePermissions:
                case StandardPermissionType.Delete:
                case StandardPermissionType.DeleteAssociate:
                case StandardPermissionType.UpdateAssociate:
                    return true;
            }

            return false;
        }
    }
}
