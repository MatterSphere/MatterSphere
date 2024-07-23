namespace FWBS.OMS.Security.Permissions
{
    public sealed class ClientPermission : Permission
    {
        public ClientPermission(Client client, string permissionType)
            : base(client, permissionType)
        {
        }

        public ClientPermission(Client client, StandardPermissionType permissionType)
            : base(client, permissionType)
        {
        }

        protected override bool Supports(StandardPermissionType type)
        {
            switch (type)
            {
                case StandardPermissionType.CreateFile:
                case StandardPermissionType.UpdatePermissions:
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
