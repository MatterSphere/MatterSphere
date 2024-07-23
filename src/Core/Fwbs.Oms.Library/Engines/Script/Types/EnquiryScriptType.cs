using System;
using System.CodeDom;

namespace FWBS.OMS.Script
{
    public class EnquiryScriptType : ScriptType
    {
        #region Fields

        /// <summary>
        /// Holds a reference to the enquiry form being used.
        /// </summary>
        private EnquiryEngine.Enquiry _enq = null;

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
                    if (_enq != null)
                    {
                        _enq.DataChanged -= new EventHandler(CurrentScript.DataChanged);
                        _enq.Refreshed -= new EventHandler(CurrentScript.Refreshed);
                        _enq.Updated -= new EventHandler(CurrentScript.Updated);
                        _enq.Updating -= new System.ComponentModel.CancelEventHandler(CurrentScript.Updating);
                        _enq = null;
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
        /// <param name="enquiry">Enquiry engine object to use.</param>
        internal void SetEnquiryObject(EnquiryEngine.Enquiry enquiry)
        {
            if (_enq != null)
            {
                _enq.DataChanged -= new EventHandler(CurrentScript.DataChanged);
                _enq.Refreshed -= new EventHandler(CurrentScript.Refreshed);
                _enq.Updated -= new EventHandler(CurrentScript.Updated);
                _enq.Updating -= new System.ComponentModel.CancelEventHandler(CurrentScript.Updating);
            }
            _enq = enquiry;
            if (_enq != null)
            {
                _enq.DataChanged += new EventHandler(CurrentScript.DataChanged);
                _enq.Refreshed += new EventHandler(CurrentScript.Refreshed);
                _enq.Updated += new EventHandler(CurrentScript.Updated);
                _enq.Updating += new System.ComponentModel.CancelEventHandler(CurrentScript.Updating);
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the enquiry engine object reference that the script is associated to.
        /// </summary>
        public EnquiryEngine.Enquiry Enquiry
        {
            get
            {
                return _enq;
            }
        }

        new protected EnquiryScriptType CurrentScript
        {
            get
            {
                return (EnquiryScriptType)base.CurrentScript;
            }
        }

        public override object CurrentObj
        {
            get
            {
                return _enq;
            }
        }

        private ContextFactory _contextfactory = new ContextFactory();

        public override IContext Context
        {
            get
            {
                return _contextfactory.CreateContext(_enq.Object, _enq.Parent);
            }
        }

        #endregion

        #region Script Virtual Methods

        [ScriptMethodOverridable()]
        protected virtual void Refreshed(object sender, EventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void Updated(object sender, EventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void Updating(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        [ScriptMethodOverridable()]
        protected virtual void DataChanged(object sender, EventArgs e)
        {
        }

        #endregion
    }
}
