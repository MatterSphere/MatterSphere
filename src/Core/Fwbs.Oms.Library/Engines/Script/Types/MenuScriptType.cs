using System;
using System.CodeDom;

namespace FWBS.OMS.Script
{
    public class MenuScriptType : ScriptType
    {

        #region Fields

        private object sourceApp;
        private FWBS.OMS.Interfaces.IOMSApp sourceController;

        #endregion

        #region Constructors & Destructors

        /// <summary>
        /// Constructs a new session scriptlet and delegates the exposed script session
        /// events.
        /// </summary>
        public MenuScriptType()
        {
        }

        public override object CurrentObj
        {
            get
            {
                return sourceController;
            }
        }

        #endregion

        #region Abstraction Layer

        /// <summary>
        /// Gets all the code namespace imports.
        /// </summary>
        internal protected override CodeNamespaceImport[] NamespaceImports
        {
            get
            {
                CodeNamespaceImport[] ns = new CodeNamespaceImport[]
				{
					new CodeNamespaceImport("System"),
					new CodeNamespaceImport("System.Data"),
					new CodeNamespaceImport("System.Windows.Forms"),
					new CodeNamespaceImport("FWBS.Common.UI"),
					new CodeNamespaceImport("FWBS.Common"),
					new CodeNamespaceImport("System.Xml"),
				};
                return ns;
            }
        }


        private ContextFactory _contextfactory = new ContextFactory();

        public override IContext Context
        {
            get
            {
                return _contextfactory.CreateContext(null, null);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the internal precedent object.
        /// </summary>
        public void SetAppObject(Object app, FWBS.OMS.Interfaces.IOMSApp appcontroller)
        {
            if ((sourceApp == null) && (app != null))
            {
                // Set the Controlling Application Object, 
                sourceApp = app;
            }
            if ((sourceController == null) && (appcontroller != null))
            {
                sourceController = appcontroller;
            }
        }

        #endregion

        #region Properties

        new protected MenuScriptType CurrentScript
        {
            get
            {
                return (MenuScriptType)base.CurrentScript;
            }
        }


        public object SourceApp
        {
            get
            {
                return sourceApp;
            }
        }

        public FWBS.OMS.Interfaces.IOMSApp SourceController
        {
            get
            {
                return sourceController;
            }
        }

        public object Application
        {
            get
            {
                return sourceApp;
            }
        }

        public FWBS.OMS.Interfaces.IOMSApp OMSApplication
        {
            get
            {
                return sourceController;
            }
        }

        #endregion

        #region Script Override Methods

        [ScriptMethodOverridable()]
        public virtual bool ParseCommand(object doc, ref string command)
        {
            return false;
        }

        [ScriptMethodOverridable()]
        public virtual bool Execute(object doc, string command)
        {
            return false;
        }

        [ScriptMethodOverridable()]
        public virtual bool Validate(MenuItem item, object doc)
        {
            return false;
        }

        [ScriptMethodOverridable()]
        public virtual string OnControlLabelRequest(string Id, string Command, ref bool Handled)
        {
            Handled = false;
            return null;
        }
        [ScriptMethodOverridable()]
        public virtual string OnControlDescriptionRequest(string Id, string Command, ref bool Handled)
        {
            Handled = false;
            return null;
        }
        [ScriptMethodOverridable()]
        public virtual string OnControlScreenTipRequest(string Id, string Command, ref bool Handled)
        {
            Handled = false;
            return null;
        }
        [ScriptMethodOverridable()]
        public virtual string OnControlSuperTipRequest(string Id, string Command, ref bool Handled)
        {
            Handled = false;
            return null;
        }
        [ScriptMethodOverridable()]
        public virtual string OnControlKeyTipRequest(string Id, string Command, ref bool Handled)
        {
            Handled = false;
            return null;
        }
        [ScriptMethodOverridable()]
        public virtual void OnControlDynamicMenuRequest(string Id, string Command, System.Xml.XmlElement Control, ref bool Handled)
        {
            Handled = false;
        }
        [ScriptMethodOverridable()]
        public virtual string OnControlSizeRequest(string Id, string Command, ref bool Handled)
        {
            Handled = false;
            return null;
        }
        [ScriptMethodOverridable()]
        public virtual bool OnControlShowImageRequest(string Id, string Command, ref bool Handled)
        {
            Handled = false;
            return false;
        }
        [ScriptMethodOverridable()]
        public virtual bool OnControlEnabledRequest(string Id, string Command, ref bool Handled)
        {
            Handled = false;
            return false;
        }
        [ScriptMethodOverridable()]
        public virtual bool OnControlVisibleRequest(string Id, string Command, ref bool Handled)
        {
            Handled = false;
            return false;
        }
        [ScriptMethodOverridable()]
        public virtual bool OnControlShowLabelRequest(string Id, string Command, ref bool Handled)
        {
            Handled = false;
            return false;
        }
        [ScriptMethodOverridable()]
        public virtual bool OnControlPressedRequest(string Id, string Command, ref bool Handled)
        {
            Handled = false;
            return false;
        }
        [ScriptMethodOverridable()]
        public virtual void OnExecuteAction(string Id, string Command, ref bool Handled)
        {
            Handled = false;
        }
        [ScriptMethodOverridable()]
        public virtual void OnExecuteToggle(string Id, string Command, bool Pressed, ref bool Handled)
        {
            Handled = false;
        }

        #endregion

    }
}
