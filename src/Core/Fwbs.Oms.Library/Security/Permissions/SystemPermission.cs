namespace FWBS.OMS.Security.Permissions
{
    public sealed class SystemPermission : Permission
    {
        public SystemPermission(string permissionType)
            : base(Session.OMS, permissionType)
        {
        }

         public SystemPermission(StandardPermissionType permissionType)
            : base(Session.OMS, permissionType)
        {
        }

        protected override bool Supports(StandardPermissionType type)
        {
            switch (type)
            {
                case StandardPermissionType.CreateFile:
                case StandardPermissionType.CreateContact:
                case StandardPermissionType.SaveDocument:
                case StandardPermissionType.CreateClient:
                case StandardPermissionType.CreatePrecedent:
                case StandardPermissionType.CreateAssociate:
                case StandardPermissionType.UpdateAssociate:
                case StandardPermissionType.UpdateClient:
                case StandardPermissionType.UpdateContact:
                case StandardPermissionType.UpdateDocument:
                case StandardPermissionType.UpdateFile:
                case StandardPermissionType.UpdatePrecedent:
                case StandardPermissionType.AdminKit:
                case StandardPermissionType.ReportsViewer:
                case StandardPermissionType.ViewPermissions:
                case StandardPermissionType.DeleteDocument:
                case StandardPermissionType.DeleteAssociate:
                case StandardPermissionType.SecurityAdmin:
                    return true;
            }

            return false;
        }
    }
}
