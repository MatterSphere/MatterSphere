using System;

namespace FWBS
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Property, AllowMultiple=true)]
    public sealed class VersionConditionalAttribute : Attribute
    {
        private readonly Version version;

        public VersionConditionalAttribute(string version)
        {
            this.version = new Version(version);
        }

        public Version Version
        {
            get
            {
                return version;
            }
        }
    }
}
