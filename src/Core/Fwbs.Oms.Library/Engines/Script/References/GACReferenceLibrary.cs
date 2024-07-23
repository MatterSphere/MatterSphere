using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Fwbs.Framework.Reflection;

namespace FWBS.OMS.Script
{
    [Export]
    internal sealed class GACReferenceLibrary : BaseReferenceLibrary 
    {
        private readonly List<string> list = new List<string>();
        private readonly List<Predicate<string>> exclusions = new List<Predicate<string>>();

        public GACReferenceLibrary()
        {
            exclusions.Add((n) => n.EndsWith(".resources", StringComparison.InvariantCultureIgnoreCase));
            exclusions.Add((n) => n.StartsWith("Policy.", StringComparison.InvariantCultureIgnoreCase));
        }

        public override string Name
        {
            get { return "Global Assemblies"; }
        }

        public List<Predicate<string>> Exclusions
        {
            get
            {
                return exclusions;
            }
        }

        public IEnumerable<IReference> AllAssemblies
        {
            get
            {
                IAssemblyName an;
                var enu = AssemblyCache.CreateGACEnum();
                while (AssemblyCache.GetNextAssembly(enu, out an) == 0)
                {
                    var name = AssemblyCache.GetName(an);

                    yield return new AssemblyReference(name);
                }
            }
        }

        protected override IEnumerable<IReference> BuildReferences()
        {
            list.Clear();

            foreach (var ass in AllAssemblies)
            {
                if (IsExcluded(ass.AssemblyName))
                    continue;

                if (list.Contains(ass.AssemblyName))
                    continue;

                list.Add(ass.AssemblyName);

                yield return ass;
            }
        }




        private bool IsExcluded(string name)
        {
            foreach (var pred in exclusions)
            {
                if (pred(name))
                    return true;
            }

            return false;
        }
    }
}
