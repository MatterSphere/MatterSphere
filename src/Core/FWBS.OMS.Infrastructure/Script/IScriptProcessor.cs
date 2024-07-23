using System.CodeDom.Compiler;
using System.Collections.Generic;

namespace FWBS.OMS.Script
{
    public interface IScriptProcessor
    {
        IEnumerable<ProcessLink> Compile(string script, CompilerResults results);

        bool Process(string script, ProcessLink link);
    }

    public class ProcessLink
    {
        public string Id{get;set;}

        public string Text{get;set;}
    }

}
