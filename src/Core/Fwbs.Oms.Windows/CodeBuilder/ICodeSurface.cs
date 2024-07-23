using System;
using System.Windows.Forms;
using Fwbs.Framework;

namespace FWBS.OMS.Design.CodeBuilder
{
    public interface ICodeSurface : IServiceProviderEx
    {
        bool IsInitialized { get; }
        void Init(Form owner);

        void Load(FWBS.OMS.Script.ScriptType scriptletType, FWBS.OMS.Script.ScriptGen scriptlet);
        void Unload();

        bool SupportsLanguage(FWBS.OMS.Script.ScriptLanguage language);

        bool HasMethod(string name);

        void GenerateHandler(string name, GenerateHandlerInfo info);
        
        void GotoMethod(string name);
        string[] GetMethods();

        bool IsDirty { get; set; }

        bool SaveAndCompile();
       
        FWBS.OMS.Script.ScriptType ScriptType { get; }
        FWBS.OMS.Script.ScriptGen ScriptGen { get; }

        event EventHandler CloseScriptWindowMenuItem;
        bool IsCloseScriptWindowMenuItemVisible { get; set; }
    }

    public interface ICodeSurfaceControls
    {
        void Attach(string name);
        void Clear();
        void Detach(string name);
    }

    public class GenerateHandlerInfo
    {
        public string DelegateType{get;set;}
        public string Workflow { get; set; }
    }
}
