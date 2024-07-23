using System;
using System.Collections.Generic;

namespace FWBS.OMS.Script
{
    public interface IScriptDefinition
    {
        string Name { get; }

        string Namespace { get; }

        long Version { get; }

        string Author { get; }

        string Description { get; }

        string TypeCode { get; }

        string TypeDescription { get; }

        Type BaseType{get;}

        ScriptLanguage Language { get; }

        IEnumerable<IReference> References { get; }

        IScriptBuilder CreateBuilder();

        IScriptBuilder CreateDefaultBuilder();

        IEnumerable<Tuple<string, string>> ProviderOptions { get; }

        IEnumerable<string> CompilerOptions { get; }

        IEnumerable<string> ConditionalCompilationSymbols { get; }
    }


    [Serializable]
    public class InvalidScriptDefinitionException : Exception
    {
        public InvalidScriptDefinitionException() { }
        public InvalidScriptDefinitionException(string message) : base(message) { }
        public InvalidScriptDefinitionException(string message, Exception inner) : base(message, inner) { }
        protected InvalidScriptDefinitionException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}
