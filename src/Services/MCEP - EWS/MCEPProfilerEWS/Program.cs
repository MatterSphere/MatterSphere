using System.ServiceProcess;

namespace MCEPProfilerEWS
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
                new MCEPProfilerEWS() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
