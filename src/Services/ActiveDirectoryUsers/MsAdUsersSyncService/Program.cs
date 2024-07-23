using Topshelf;

namespace MsAdUsersSyncService
{
    class Program
    {
        private const string SERVICE_NAME = "3EMatterSphereADSyncService";
        private const string DISPLAY_NAME = "3E MatterSphere ADSync Service";
        private const string DESCRIPTION_NAME = "The 3E MatterSphere ADSync Service runs to automatically update the Active Directory Groups and any sub groups.";
        static void Main(string[] args)
        {
            HostFactory.Run(host =>
            {
                host.SetServiceName(SERVICE_NAME);
                host.SetDisplayName(DISPLAY_NAME);
                host.SetDescription(DESCRIPTION_NAME);
                host.StartAutomatically();
                host.Service<AdSyncService>();
            });
        }
    }
}
