using System;

namespace FWBS.OMS.Script
{
    internal sealed class ScriptTuple : Tuple<IScriptDefinition, ScriptReference, ScriptGen>
    {
        public ScriptTuple(IScriptDefinition definition, ScriptReference reference, ScriptGen gen)
            :base(definition, reference, gen)
        {
        }
    }
}
