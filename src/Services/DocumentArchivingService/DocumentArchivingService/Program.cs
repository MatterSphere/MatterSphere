using System.ServiceProcess;

namespace DocumentArchivingService
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
                new DocumentArchiveService() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
