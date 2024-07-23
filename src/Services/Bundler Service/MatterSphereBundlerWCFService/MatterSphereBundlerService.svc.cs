using System;
using System.Reflection;

namespace MatterSphereBundlerWCFService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class MatterSphereBundlerService : IMatterSphereBundlerService
    {
        /// <summary>
        /// Test the connection to the service.
        /// </summary>
        /// <returns></returns>
        public string TestConnection()
        {
            return string.Format("Test connection succeeded.  Service running on {0}.", Environment.MachineName);
        } 

        /// <summary>
        /// Get the version of the service
        /// </summary>
        /// <returns></returns>
        public string ServiceVersion()
        {
            string version = ((AssemblyFileVersionAttribute)Attribute.GetCustomAttribute(
                Assembly.GetExecutingAssembly(), typeof(AssemblyFileVersionAttribute), false)).Version;

            return version;
        }

        /// <summary>
        /// Pick and send an instruction file to the service
        /// </summary>
        /// <param name="xmlFile"></param>
        public void ProcessBundles(string xmlFile, string incomingKey)
        {
            using (Bundler.BundleTools bt = new Bundler.BundleTools())
                bt.ProcessBundles(xmlFile, incomingKey);
        }

        /// <summary>
        /// Pick and send an instruction file to the service
        /// </summary>
        /// <param name="xmlFile"></param>
        public bool ValidateKey(string incomingKey)
        {
            using (Bundler.BundleTools bt = new Bundler.BundleTools())
                return bt.ValidateKey(incomingKey);
        }
    }
}
