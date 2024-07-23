namespace FWBS.OMS.StatusManagement
{
    public class Checks
    {
        public static void PreClientCreation()
        {
            ClientStatusList clientStatuses = new ClientStatusList("PROSPECT");
            if (!clientStatuses.ClientStatusListIsNullOrEmpty())
            {
                if (!clientStatuses.FileCreation)
                    throw new FWBS.OMS.Security.PermissionsException("ERRCLMSTACTDENY", "Activity Denied");
            }
        }
    }

}
