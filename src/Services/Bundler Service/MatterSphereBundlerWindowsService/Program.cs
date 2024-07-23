using System.ServiceProcess;

namespace MatterSphereBundlerWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
            ServiceBase[] ServicesToRun = new ServiceBase[] 
			{ 
				new Main() 
			};
            ServiceBase.Run(ServicesToRun);
        }
        
    }
}
