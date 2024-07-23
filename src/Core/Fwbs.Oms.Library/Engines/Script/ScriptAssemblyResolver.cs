using System;
using System.Reflection;
using Fwbs.Framework.Reflection;

namespace FWBS.OMS.Script
{
    internal sealed class ScriptAssemblyResolver : IAssemblyResolver
    {
        public Assembly Resolve(AN assemblyName)
        {
            if (assemblyName == null)
                return null;

            var name = assemblyName.Name;

            if (!name.StartsWith("SCRIPT-", StringComparison.OrdinalIgnoreCase))
            {
                ScriptFileName sfn;
                if (!ScriptFileName.TryParse(name, out sfn))
                {
                    return null;
                }

                name = sfn.Name;
            }
            else
            {
                name = name.Substring(7);
            }

            try
            {
                var gen = ScriptGen.GetScript(name);

                gen.Load();

                return gen.Scriptlet.GetType().Assembly;
            }
            catch (ScriptException sex)
            {
                if (sex.HelpID == HelpIndexes.ScriptNotFound)
                    return null;

                throw;
            }

        }
    }
}
