using System.Collections.Generic;

namespace FWBS.OMS.Script
{
    internal sealed class DistributedAssemblyReferenceLibrary : BaseReferenceLibrary
    {

        public override string Name
        {
            get { return "Distributed Assemblies"; }
        }

        protected override IEnumerable<IReference> BuildReferences()
        {
            foreach (var item in Session.CurrentSession.DistributedAssemblyManager.ListActiveAssemblies())
            {
                yield return new DistributedAssemblyReference(item.AssemblyName);
            }
        }
    }
}
