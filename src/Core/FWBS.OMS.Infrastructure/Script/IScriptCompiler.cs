using System;
using System.CodeDom.Compiler;

namespace FWBS.OMS.Script
{
    public interface IScriptCompiler
    {
        event EventHandler Start;
        event EventHandler Finished;
        event EventHandler Error;

        CompilerResults Compile(CompileOptions options);
    }

    public class CompileOptions
    {
        public bool Force { get; set; }
    }

}
