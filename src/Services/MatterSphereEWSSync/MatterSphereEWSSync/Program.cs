using System.ServiceProcess;

namespace MatterSphereEWSSync
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new MatterSphereEWSSyncService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
