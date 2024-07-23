using System;
using System.CodeDom;

namespace FWBS.OMS.Script
{
    public class ReportScriptType : ScriptType
    {
        #region Fields
        /// <summary>
        /// Holds a reference to the Reports Object
        /// </summary>
        private Report _report = null;
        #endregion

        #region Properties
        public Report Report
        {
            get
            {
                return _report;
            }
        }

        public override object CurrentObj
        {
            get
            {
                return _report;
            }
        }

        new protected ReportScriptType CurrentScript
        {
            get
            {
                return (ReportScriptType)base.CurrentScript;
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
				};
                return ns;
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_report != null)
                    {
                        _report.Load -= new EventHandler(CurrentScript.Load);
                        _report.Show -= new EventHandler(CurrentScript.Show);
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }


        private ContextFactory _contextfactory = new ContextFactory();

        public override IContext Context
        {
            get
            {
                return _contextfactory.CreateContext(null, _report.SearchList.Parent);
            }
        }

        #endregion

        #region Script Virtual Methods
        /// <summary>
        /// Load of a Report Object, will run before The Parameters or Parent is set
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual void Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Show of a Report Object in the UI
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual void Show(object sender, EventArgs e)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Sets the internal Report object.
        /// </summary>
        internal void SetReportObject(Report report)
        {
            if (_report == null)
            {
                _report = report;

                _report.Load -= new EventHandler(CurrentScript.Load);
                _report.Show -= new EventHandler(CurrentScript.Show);

                _report.Load += new EventHandler(CurrentScript.Load);
                _report.Show += new EventHandler(CurrentScript.Show);
            }
        }

        #endregion

    }

	
}
