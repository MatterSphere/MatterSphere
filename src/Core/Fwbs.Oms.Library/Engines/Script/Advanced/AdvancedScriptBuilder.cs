using System;
using System.CodeDom;
using System.ComponentModel.Composition;

namespace FWBS.OMS.Script
{
    [Export(typeof(IScriptBuilder))]
    [Export("ADVANCED", typeof(IScriptBuilder))]
    internal sealed class AdvancedScriptBuilder : ScriptBuilder
    {
        protected override CodeCompileUnit GenerateMainCode(IScriptDefinition definition)
        {
            var def = (AdvancedScriptDefinition)definition;

            var unit = new CodeSnippetCompileUnit(def.Code);

            return unit;
        }

        protected override System.CodeDom.CodeCompileUnit CreateAssemblyUnit(IScriptDefinition definition)
        {
            var def = (AdvancedScriptDefinition)definition;

            if (!String.IsNullOrWhiteSpace(def.Code))
            {
                switch (def.Language)
                {
                    case ScriptLanguage.VB:
                        {
                            if (def.Code.IndexOf("Assembly: System.Reflection.AssemblyVersion") > -1)
                                return null;
                        }
                        break;
                    case ScriptLanguage.CSharp:
                        {
                            if (def.Code.IndexOf("[assembly: System.Reflection.AssemblyVersion") > -1)
                                return null;
                        }
                        break;
                }
            }

            return base.CreateAssemblyUnit(definition);
        }
    }
}
