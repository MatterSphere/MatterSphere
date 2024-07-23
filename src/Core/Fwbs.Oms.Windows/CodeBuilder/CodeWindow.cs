using System;
using System.Windows.Forms;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.Design.CodeBuilder
{
    public partial class CodeWindow : BaseForm, ICodeSurface
    {
        public CodeWindow()
        {
            InitializeComponent();
        }

        public bool IsInitialized
        {
            get { return codeSurface1.IsInitialized; }
        }

        public void Init(Form owner)
        {
            codeSurface1.Init(owner);
        }

        public new void Load(Script.ScriptType scriptletType, Script.ScriptGen scriptlet)
        {
            codeSurface1.Load(scriptletType, scriptlet);
            codeSurface1.CloseScriptWindowMenuItem += new EventHandler(codeSurface1_CloseScriptWindowMenuItem);
            codeSurface1.IsCloseScriptWindowMenuItemVisible = this.IsCloseScriptWindowMenuItemVisible;
        }

        public void Unload()
        {
            codeSurface1.CloseScriptWindowMenuItem -= new EventHandler(codeSurface1_CloseScriptWindowMenuItem);
            codeSurface1.Unload();
        }

        void codeSurface1_CloseScriptWindowMenuItem(object sender, EventArgs e)
        {
            if (this.CloseScriptWindowMenuItem != null)
                CloseScriptWindowMenuItem(this, EventArgs.Empty);
        }

        public bool SupportsLanguage(Script.ScriptLanguage language)
        {
            return codeSurface1.SupportsLanguage(language);
        }

        public bool HasMethod(string method)
        {
            return codeSurface1.HasMethod(method);
        }

        public void GenerateHandler(string name, GenerateHandlerInfo info)
        {
            codeSurface1.GenerateHandler(name, info);
        }

        public void GotoMethod(string name)
        {
            codeSurface1.GotoMethod(name);
        }

        public string[] GetMethods()
        {
            return codeSurface1.GetMethods();
        }

        public bool IsDirty
        {
            get
            {
                return codeSurface1.IsDirty;
            }
            set
            {
                codeSurface1.IsDirty = value;
            }
        }

        public bool SaveAndCompile()
        {
            return codeSurface1.SaveAndCompile();
        }

     

        private bool _canclose = false;

        private void CodeWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !_canclose;
            if (!_canclose)
                this.Hide();
        }

        new public void Close()
        {
            _canclose = true;
            base.Close();
        }


        public Script.ScriptType ScriptType
        {
            get { return codeSurface1.ScriptType; }
        }

        public Script.ScriptGen ScriptGen
        {
            get { return codeSurface1.ScriptGen; }
        }

        new public object GetService(Type serviceType)
        {
            return codeSurface1.GetService(serviceType);
        }

        public T GetService<T>()
        {
            return codeSurface1.GetService<T>();
        }


        public event EventHandler CloseScriptWindowMenuItem;

        public bool IsCloseScriptWindowMenuItemVisible
        {
            get;
            set;
        }
    }
}
