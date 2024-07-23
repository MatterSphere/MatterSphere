using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace BundlerConsoleHost
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.ServicePointManager.SecurityProtocol |= System.Net.SecurityProtocolType.Tls12;
            Uri baseAddress = new Uri(string.Format("http://localhost:{0}/MatterSphereBundlerService", BundlerConsoleHost.Properties.Settings.Default.Port));

            using (ServiceHost host = new ServiceHost(typeof(MatterSphereBundlerWCFService.MatterSphereBundlerService), baseAddress))
            {
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = false;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                host.Open();

                Console.WriteLine("The service is ready at {0}", baseAddress);
                Console.WriteLine("Press <Enter> to stop the service.");
                Console.ReadLine();

                // Close the ServiceHost.
                host.Close();
            }


        }
    }
}
