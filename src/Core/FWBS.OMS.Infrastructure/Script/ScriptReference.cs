using System;

namespace FWBS.OMS.Script
{
    public class ScriptReference : IReference
    {
        public ScriptReference(string name)
        {
            Name = name;
            AssemblyName = string.Format("SCRIPT-{0}", Name);
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
            return String.Format("{0} (Script)", Name);
        }
    }
}
