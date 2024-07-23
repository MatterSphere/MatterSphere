using System;

namespace FWBS.OMS.Script
{
    public class DistributedAssemblyReference : IReference
    {
        public DistributedAssemblyReference(string name)
        {
            Name = AssemblyReference.GetFileNameWithExtension(name);
            AssemblyName = AssemblyReference.GetFileNameWithoutExtension(name);
        }

        public string Name { get; private set; }

        public string AssemblyName { get; private set; }

        public string Location { get; set; }

        public bool IsGlobal
        {
            get
            {
                return false;
            }
        }

        public bool IsRequired
        {
            get
            {
                return false;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} (Distributed)", Name);
        }
    }

}
