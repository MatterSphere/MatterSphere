using System;
using System.IO;
using System.Reflection;
using Fwbs.Framework.Reflection;

namespace FWBS.OMS
{

    internal sealed class DistributedAssemblyResolver : IAssemblyResolver
    {
        // Set the Location of the Distributed Assmeblies
        private DirectoryInfo distributedAssembliesDirectory;
        private DistributedAssemblyManager _manager;

        public DistributedAssemblyResolver(Version assemblyversion, DistributedAssemblyManager manager)
        {
            if (manager == null)
                throw new ArgumentNullException("manager");

            if (assemblyversion == null)
                throw new ArgumentNullException("assemblyversion");

            _manager = manager;
            
            // Set the Location of the Distributed Assmeblies
            distributedAssembliesDirectory = new DirectoryInfo(Global.GetDBAppDataPath() + @"\Distributed\" + assemblyversion.ToString());
            if (distributedAssembliesDirectory.Exists == false)
                distributedAssembliesDirectory.Create();

        }

        public System.Reflection.Assembly Resolve(AN assemblyName)
        {
            if (assemblyName == null)
                return null;


            string fileName = assemblyName.Name + ".dll";

            var result = _manager.CheckForUpdatedAssembly(fileName);
            FileInfo file = null;
            switch (result)
            {
                case DistributedCheckResult.Ok:
                    file = DistributedAssemblyManager.GetExtractedAssemblyFileInfo(fileName,distributedAssembliesDirectory.FullName);
                    break;
                case DistributedCheckResult.NotFound:
                    return null;
                case DistributedCheckResult.OutOfDate:
                case DistributedCheckResult.NotDownloaded:
                    {
                        DistributedAssemblies distrib = new DistributedAssemblies();
                        distrib.FetchCurrent(fileName);
                        file = distrib.ExtractFromDatabase();
                        break;
                    }
                case DistributedCheckResult.Error:
                    return null;
            }
            if (file == null)
                throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("CHKUPDASUNXPRS", "Check for Updated Assembly return unexpected result.", "").Text);

            return Assembly.LoadFrom(file.FullName);
        }

        private static bool ParseIsSatelliteAssembly(string assemblyName)
        {
            int firstComma = assemblyName.IndexOf(',');
            if (firstComma == -1)
                return false;

            if (string.Equals(assemblyName.Substring(firstComma - 10, 10), ".resources"))
            {
                int cultureStart = assemblyName.IndexOf("Culture=", firstComma) + 8;
                int cultureEnd = assemblyName.IndexOf(',', cultureStart);

                if (cultureStart == -1 || cultureEnd == -1)
                    return false;

                if (!string.Equals(assemblyName.Substring(cultureStart, cultureEnd - cultureStart), "neutral"))
                    return true;
            }

            return false;
        }

    }
}
