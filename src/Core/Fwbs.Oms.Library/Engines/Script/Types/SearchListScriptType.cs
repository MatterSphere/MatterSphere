using System.CodeDom;

namespace FWBS.OMS.Script
{
    public class SearchListScriptType : ScriptType
    {
        #region Fields

        /// <summary>
        /// Holds a reference to the search list engine object being used.
        /// </summary>
        private SearchEngine.SearchList _search = null;

        #endregion

        #region Constructors & Destructors

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
                    if (_search != null)
                    {
                        _search.Searched -= new FWBS.OMS.SearchEngine.SearchedEventHandler(CurrentScript.Searched);
                        _search.Searching -= new System.ComponentModel.CancelEventHandler(CurrentScript.Searching);
                        _search.Error -= new MessageEventHandler(CurrentScript.Error);
                        _search = null;
                    }
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
					new CodeNamespaceImport("System.Windows.Forms"),
					new CodeNamespaceImport("FWBS.Common.UI"),
					new CodeNamespaceImport("FWBS.Common"),
					new CodeNamespaceImport("FWBS.OMS.UI.Windows"),
				};
                return ns;
            }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Sets the internal enquiry engine object.
        /// </summary>
        /// <param name="search">Enquiry engine object to use.</param>
        internal void SetSearchObject(SearchEngine.SearchList search)
        {
            if (_search != null)
            {
                _search.Searched -= new FWBS.OMS.SearchEngine.SearchedEventHandler(CurrentScript.Searched);
                _search.Searching -= new System.ComponentModel.CancelEventHandler(CurrentScript.Searching);
                _search.Error -= new MessageEventHandler(CurrentScript.Error);
            }
            _search = search;
            if (_search != null)
            {
                _search.Searched += new FWBS.OMS.SearchEngine.SearchedEventHandler(CurrentScript.Searched);
                _search.Searching += new System.ComponentModel.CancelEventHandler(CurrentScript.Searching);
                _search.Error += new MessageEventHandler(CurrentScript.Error);
            }
        }

        #endregion

        #region Properties

        new protected SearchListScriptType CurrentScript
        {
            get
            {
                return (SearchListScriptType)base.CurrentScript;
            }
        }

        /// <summary>
        /// Gets the seach engine object reference that the script is associated to.
        /// </summary>
        public SearchEngine.SearchList SearchList
        {
            get
            {
                return _search;
            }
        }

        public override object CurrentObj
        {
            get
            {
                return _search;
            }
        }

        private ContextFactory _contextfactory = new ContextFactory();

        public override IContext Context
        {
            get
            {
                return _contextfactory.CreateContext(null, _search.Parent);
            }
        }

        #endregion

        #region Script Virtual Methods

        [ScriptMethodOverridable()]
        protected virtual void Searching(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        [ScriptMethodOverridable()]
        protected virtual void Searched(object sender, FWBS.OMS.SearchEngine.SearchedEventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void Error(object sender, MessageEventArgs e)
        {
        }

        #endregion


    }


}
