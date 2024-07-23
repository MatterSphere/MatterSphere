using System;

namespace FWBS.OMS.Script
{
    internal sealed class ScriptFactory
    {
        public IScriptDefinition CreateDefinition(ScriptGen gen)
        {
            if (gen == null)
                throw new ArgumentNullException("gen");

            if (System.IO.File.Exists(gen.OutputName))
            {
                return new AdvancedScriptDefinition(gen);
            }

            if (gen.AdvancedScript)
                return new AdvancedScriptDefinition(gen);
            else
                return new BasicScriptDefinition(gen);
        }

        public IScriptCompiler CreateCompiler(IScriptDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");

            return new ScriptCompiler(definition, CreateBuilder(definition));
        }

        public IScriptBuilder CreateBuilder(IScriptDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");

            return definition.CreateBuilder();
        }

        public IScriptLoader CreateLoader(IScriptDefinition definition)
        {
            if (definition == null)
                throw new ArgumentNullException("definition");

            return new ScriptLoader(definition, CreateCompiler(definition));
        }
    }
}
