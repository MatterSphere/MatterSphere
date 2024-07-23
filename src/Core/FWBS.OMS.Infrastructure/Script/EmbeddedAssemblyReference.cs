using System;

namespace FWBS.OMS.Script
{
    public class EmbeddedAssemblyReference : IReference
    {
        public EmbeddedAssemblyReference(string name)
        {
            Name = AssemblyReference.GetFileNameWithExtension(name);
            AssemblyName = AssemblyReference.GetFileNameWithoutExtension(name);
        }

        public string Name { get; private set; }

        public string AssemblyName{get; private set;}

        public string Data { get; set; }

        public string Location { get; set; }

        public DateTime? Modified { get; set; }

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
            return String.Format("{0} (Embedded)", Name);
        }
    }

}
