using System.CodeDom;
using System.ComponentModel.Composition;
using System.Linq;

namespace FWBS.OMS.Script
{
    [Export("BASIC", typeof(IScriptBuilder))]
    internal sealed class BasicScriptBuilder : ScriptBuilder
    {
        protected override CodeCompileUnit GenerateMainCode(IScriptDefinition definition)
        {
            var def = (BasicScriptDefinition)definition;

            var ns = new CodeNamespace(definition.Namespace);

            var unit = new CodeCompileUnit();

            unit.Namespaces.Add(ns);

            ns.Imports.AddRange(def.Imports.ToArray());

            CodeTypeDeclaration cl = new CodeTypeDeclaration(def.Name);
            cl.IsPartial = true;
            cl.BaseTypes.Add(definition.BaseType);
            ns.Types.Add(cl);


            cl.Members.AddRange(def.Fields.ToArray());

            cl.Members.AddRange(def.Methods.ToArray());
          
            return unit;
        }
    }
}
