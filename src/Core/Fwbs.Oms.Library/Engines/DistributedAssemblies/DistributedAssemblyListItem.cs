using System;

namespace FWBS.OMS
{
    internal sealed class DistributedAssemblyListItem
    {
        public DistributedAssemblyListItem(string assemblyname, DateTime modified, string version, string omsversion )
        {
            this.AssemblyName = assemblyname;
            this.Modified = modified;
            this.Version = version;
            this.OMSVersion = omsversion;
        }

        public string AssemblyName { get; private set; }
        public DateTime Modified { get; private set; }
        public string Version { get; private set; }
        public string OMSVersion { get; private set; }
    }
}
