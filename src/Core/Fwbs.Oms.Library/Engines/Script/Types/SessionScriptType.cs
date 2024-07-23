using System;
using System.CodeDom;

namespace FWBS.OMS.Script
{
    public class SessionScriptType : ScriptType
    {
        private Session session;

        #region Constructors & Destructors

        /// <summary>
        /// Constructs a new session scriptlet and delegates the exposed script session
        /// events.
        /// </summary>
        public SessionScriptType()
        {
        }

        public override object CurrentObj
        {
            get
            {
                return session;
            }
        }


        /// <summary>
        /// Disposes all internal objects used by this object.
        /// </summary>
        /// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    Session.CurrentSession.LoggedIn -= new EventHandler(CurrentScript.LogOn);
                    Session.CurrentSession.LoggedOff -= new EventHandler(CurrentScript.LogOff);
                }
            }
            finally
            {
                base.Dispose(disposing);
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
				};
                return ns;
            }
        }

    

        public void SetSessionObject(Session session)
        {
            if (session == null)
                throw new ArgumentNullException("session");

            this.session = session;

            Session.CurrentSession.LoggedIn -= new EventHandler(CurrentScript.LogOn);
            Session.CurrentSession.LoggedOff -= new EventHandler(CurrentScript.LogOff);

            Session.CurrentSession.LoggedIn += new EventHandler(CurrentScript.LogOn);
            Session.CurrentSession.LoggedOff += new EventHandler(CurrentScript.LogOff);
        }

        private ContextFactory _contextfactory = new ContextFactory();

        public override IContext Context
        {
            get
            {
                return _contextfactory.CreateContext(session, null);
            }
        }


        #endregion

        #region Script Virtual Methods

        [ScriptMethodOverridable()]
        protected virtual void LogOn(object sender, EventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void LogOff(object sender, EventArgs e)
        {
        }
        [ScriptMethodOverridable()]
        protected virtual void AutoSaveDocument(object sender, EventArgs e)
        {
        }
        [ScriptMethodOverridable()]
        protected virtual void SaveForeignDocument(object sender, EventArgs e)
        {
        }

        #endregion

        #region Properties

        new protected SessionScriptType CurrentScript
        {
            get
            {
                return (SessionScriptType)base.CurrentScript;
            }
        }

        #endregion

    }
}
