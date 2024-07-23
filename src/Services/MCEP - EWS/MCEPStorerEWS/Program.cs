using System.ServiceProcess;

namespace MCEPStorerEWS
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
                new MCEPStorerEWS() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
