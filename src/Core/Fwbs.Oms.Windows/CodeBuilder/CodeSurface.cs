using System;
using System.Windows.Forms;
using FWBS.Common;
using FWBS.OMS.UI.Windows;

namespace FWBS.OMS.Design.CodeBuilder
{
    internal class CodeSurfaceMenuStrip : MenuStrip
    {
        [System.ComponentModel.Browsable(false)]
        [System.ComponentModel.DefaultValue(false)]
        public bool EnableHotKeys { get; set; }
    }

    internal class CodeSurfaceMenuItem : ToolStripMenuItem
    {
        protected override bool ProcessCmdKey(ref Message m, Keys keyData)
        {
            if (GetToolStrip().EnableHotKeys)
            {
                return base.ProcessCmdKey(ref m, keyData);
            }

            return false;
        }

        private CodeSurfaceMenuStrip GetToolStrip()
        {
            ToolStripItem item = this;
            while (item.OwnerItem != null)
                item = item.OwnerItem;

            return (CodeSurfaceMenuStrip)item.Owner;
        }
    }

    public partial class CodeSurface : UserControl, ICodeSurface
    {
        #region Fields

        private ICodeSurface surface;
        private FWBS.OMS.Script.ScriptType scriptype;
        private FWBS.OMS.Script.ScriptGen script;

        #endregion

        #region Constructors

        public CodeSurface()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

        }

        #endregion

        #region IServiceProvider

        new public object GetService(Type serviceType)
        {
            if (surface == null)
                return null;

            return surface.GetService(serviceType);
        }

        public T GetService<T>()
        {
            return (T)GetService(typeof(T));
        }

        #endregion

        #region ICodeSurface

        private bool isinit;
        private Form parentForm;

        public void Init(Form parentForm)
        {
            isinit = true;
            this.parentForm = parentForm;
        }
        
        public bool IsInitialized
        {
            get
            {
                return isinit;
            }
        }

        public bool SupportsLanguage(FWBS.OMS.Script.ScriptLanguage language)
        {
            return surface.SupportsLanguage(language);
        }

        new public void Load(FWBS.OMS.Script.ScriptType scriptletType, FWBS.OMS.Script.ScriptGen scriptlet)
        {
            var oldscriptlet = script;
            scriptype = scriptletType;
            script = scriptlet;

            var ctrl = surface as Control;

            if (oldscriptlet != null)
            {
                oldscriptlet.Converted -= new EventHandler(scriptlet_Converted);
                oldscriptlet.NewScript -= new EventHandler(scriptlet_NewScript);
            }

            if ((script.AdvancedScript && ctrl is CodeSurfaceV1) || (!script.AdvancedScript && ctrl is CodeSurfaceV2) || (ctrl == null))
            {
                if (surface != null)
                {
                    surface.CloseScriptWindowMenuItem -= new EventHandler(surface_CloseScriptWindowMenuItem);
                    surface.Unload();

                    if (ctrl != null)
                    {
                        this.Controls.Remove(ctrl);
                        ctrl.Dispose();
                    }
                }

                // Check for new here as you need to know if a new script is created new being one that has been given a name
                scriptlet.NewScript += new EventHandler(scriptlet_NewScript);

                if (string.IsNullOrEmpty(script.Code))
                {
                    label1.Visible = true;
                    surface = null;
                    return;
                }

                label1.Visible = false;

                if (script.AdvancedScript)
                    ctrl = new CodeSurfaceV2();
                else
                {
                    ctrl = new CodeSurfaceV1();
                }
                scriptlet.Converted += new EventHandler(scriptlet_Converted);
                this.Controls.Add(ctrl, true);
                ctrl.Dock = DockStyle.Fill;
                surface = (ICodeSurface)ctrl;
                surface.IsCloseScriptWindowMenuItemVisible = this.IsCloseScriptWindowMenuItemVisible;
                surface.Init(parentForm);
                surface.CloseScriptWindowMenuItem += new EventHandler(surface_CloseScriptWindowMenuItem);
            }
            
            ctrl.Visible = false;
            
            if (script.AdvancedScript == false) 
            {
                script.ConvertToAdvanced();
                this.Unload();
                this.Load(scriptletType, scriptlet);
                this.SaveAndCompile();
                return;
            }
            
            ctrl.Visible = true;

            surface.Load(scriptletType, scriptlet);
        }

        void surface_CloseScriptWindowMenuItem(object sender, EventArgs e)
        {
            if (this.CloseScriptWindowMenuItem != null)
                CloseScriptWindowMenuItem(this, EventArgs.Empty);
        }


        void scriptlet_NewScript(object sender, EventArgs e)
        {
            FWBS.OMS.Favourites favs = new Favourites("SCRIPTLANG");
            Script.ScriptLanguage? language = null;
            if (favs.Count > 0)
                language = (Script.ScriptLanguage)ConvertDef.ToEnum(favs.Param1(0),Script.ScriptLanguage.CSharp);
            if (!language.HasValue)
            {
                using (CodeSurfaceLanguage lang = new CodeSurfaceLanguage())
                {
                    switch (lang.ShowDialog())
                    {
                        case DialogResult.Yes:
                            language = Script.ScriptLanguage.CSharp;
                            break;
                        case DialogResult.No:
                            language = Script.ScriptLanguage.VB;
                            break;
                    }
                    if (lang.DontAskAgain)
                    {
                        favs.AddFavourite("Language", "", language.ToString());
                        favs.Update();
                    }
                }
            }

            if (language.HasValue)
                ScriptGen.Language = language.Value;
        }

        void scriptlet_Converted(object sender, EventArgs e)
        {
            Load(scriptype, script);

            surface.IsDirty = true;
        }

        public void Unload()
        {
            this.IsDirty = false;

            if (surface != null)
                surface.Unload();
        }

        public bool IsDirty
        {
            get
            {
                if (surface != null)
                    return surface.IsDirty;

                return false;
            }
            set
            {
                if (surface != null)
                    surface.IsDirty = value;
            }
        }

        public bool SaveAndCompile()
        {
            if (surface != null)
                return surface.SaveAndCompile();
            return false;
        }

        public void GenerateHandler(string name, GenerateHandlerInfo info)
        {
            if (surface != null)
                surface.GenerateHandler(name, info);
        }


        public string[] GetMethods()
        {
            if (surface != null)
                return surface.GetMethods();
            return null;
        }

        public void GotoMethod(string name)
        {
            if (surface != null)
                surface.GotoMethod(name);
        }

        public bool HasMethod(string name)
        {
            if (surface != null)
                return surface.HasMethod(name);
            return false;
        }


        #endregion

        #region Methods

        new public void Show()
        {
        }

        public Script.ScriptType ScriptType
        {
            get { return scriptype; }
        }

        public Script.ScriptGen ScriptGen
        {
            get { return script; }
        }

        #endregion


        public event EventHandler CloseScriptWindowMenuItem;

        public bool IsCloseScriptWindowMenuItemVisible
        {
            get;
            set;
        }
    }
}
