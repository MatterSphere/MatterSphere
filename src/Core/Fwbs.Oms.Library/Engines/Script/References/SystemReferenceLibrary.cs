using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace FWBS.OMS.Script
{
    internal sealed class SystemReferenceLibrary : BaseReferenceLibrary
    {
        private readonly List<string> list = new List<string>();
        private readonly GACReferenceLibrary gac;

        [ImportingConstructor]
        public SystemReferenceLibrary(GACReferenceLibrary gac)
        {
            if (gac == null)
                throw new ArgumentNullException("gac");

            this.gac = gac;
            this.gac.Exclusions.Add(IsValid);
        }

        public override string Name
        {
            get { return "System Assemblies"; }
        }

        protected override IEnumerable<IReference> BuildReferences()
        {
            list.Clear();

            foreach (var ass in gac.AllAssemblies)
            {
                if (list.Contains(ass.AssemblyName))
                    continue;

                list.Add(ass.AssemblyName);

                if (IsValid(ass.AssemblyName))
                    yield return ass;
            }
        }

        private static bool IsValid(string name)
        {
            return name.StartsWith("System.");
        }
      
    }
}
