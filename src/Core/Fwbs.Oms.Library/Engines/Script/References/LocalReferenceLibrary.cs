using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Fwbs.Framework.Reflection;

namespace FWBS.OMS.Script
{

    internal sealed class LocalReferenceLibrary : BaseReferenceLibrary
    {
        public override string Name
        {
            get { return "Local Assemblies"; }
        }

        protected override IEnumerable<IReference> BuildReferences()
        {
            FileInfo current = new FileInfo(Assembly.GetExecutingAssembly().Location);

            foreach (var item in current.Directory.GetFiles("*.dll"))
            {
                if (IsExcluded(item) || IsExcludedLocal(item))
                    continue;

                yield return new AssemblyReference(item.Name);
            }
        }

        private bool IsExcluded(FileInfo file)
        {
            var ahi = new AssemblyFileInfo(file);
            if (!ahi.IsAssembly)
                return true;

            switch (ahi.CPU)
            {
                case CPUVersion.Any:
                    return false;
                case CPUVersion.x64:
                    return !Environment.Is64BitProcess;
                case CPUVersion.x86:
                    return Environment.Is64BitProcess;
                default:
                    return false;
            }
        }

        private bool IsExcludedLocal(FileInfo file)
        {
            var name = file.Name;

            if (name.StartsWith("Infragistics2.", StringComparison.InvariantCultureIgnoreCase))
                return true;
            if (name.StartsWith("Telerik.Windows.", StringComparison.InvariantCultureIgnoreCase))
                return true;
            if (name.StartsWith("ActiproSoftware.", StringComparison.InvariantCultureIgnoreCase))
                return true;

            return false;
        }

    }
}
