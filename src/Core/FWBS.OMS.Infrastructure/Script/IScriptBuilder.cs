using System.CodeDom;
using System.Collections.Generic;

namespace FWBS.OMS.Script
{
    public interface IScriptBuilder
    {
        IEnumerable<CodeCompileUnit> Build(IScriptDefinition definition);
    }
}
