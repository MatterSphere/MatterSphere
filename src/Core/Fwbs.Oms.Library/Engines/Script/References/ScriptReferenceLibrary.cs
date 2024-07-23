using System;
using System.Collections.Generic;
using System.Data;

namespace FWBS.OMS.Script
{

    internal sealed class ScriptReferenceLibrary : BaseReferenceLibrary
    {
        public override string Name
        {
            get { return "System Scripts"; }
        }

        protected override IEnumerable<IReference> BuildReferences()
        {
            using (var dt = ScriptGen.GetScripts("SYSTEM"))
            {
              
                    foreach (DataRow r in dt.Rows)
                    {
                        yield return new ScriptReference(Convert.ToString(r["scrcode"]));
                    }
            }
        }

        public override IEnumerable<IReference> GetByDefinition(IScriptDefinition definition)
        {
            if (definition == null)
            {
                foreach (var item in References)
                    yield return item;

                yield break;
            }

            foreach (var item in References)
            {

                if (item.Name.Equals(definition.Name, StringComparison.OrdinalIgnoreCase))
                    continue;

                yield return item;
            }
        }
    }
}
