using System;
using System.CodeDom;

namespace FWBS.OMS.Script
{
    public class PrecedentScriptType : ScriptType
    {
        #region Fields

        /// <summary>
        /// Holds a reference to the PrecedentObject.
        /// </summary>
        private Precedent _prec = null;

        #endregion

        #region Properties
        /// <summary>
        /// Gets the Precedent Object reference that the script is associated to.
        /// </summary>
        public Precedent Precedent
        {
            get
            {
                return _prec;
            }
        }

        public override object CurrentObj
        {
            get
            {
                return _prec;
            }
        }

        new protected PrecedentScriptType CurrentScript
        {
            get
            {
                return (PrecedentScriptType)base.CurrentScript;
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
					new CodeNamespaceImport("FWBS.Common.UI"),
					new CodeNamespaceImport("FWBS.Common"),
					new CodeNamespaceImport("FWBS.OMS.UI.Windows"),
					new CodeNamespaceImport("FWBS.OMS"),
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
                    if (_prec != null)
                    {
                        _prec.Load -= new EventHandler(CurrentScript.Load);
                        _prec.Validating -= new PrecedentLinkCancelHandler(CurrentScript.Validating);
                        _prec.Merging -= new PrecedentLinkCancelHandler(CurrentScript.Merging);
                        _prec.SecondStageMerging -= new PrecedentSecondStageMergingEventHandler(CurrentScript.SecondStageMerging);
                        _prec.DocumentSaving -= new DocumentSavingHandler(CurrentScript.Saving);
                        _prec.DocumentSaved -= new DocumentSavedHandler(CurrentScript.Saved);
                        _prec.PhysicalDocumentSaved -= new PhysicalDocumentSavedEventHandler(CurrentScript.DocumentSaved);
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
                return _contextfactory.CreateContext(_prec.Parent, null);
            }
        }
        #endregion

        #region Script Virtual Methods

        /// <summary>
        /// BeforeSave Event this can be used to set preferences for the save routine to have,
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [ScriptMethodOverridable()]
        protected virtual void Saving(object sender, DocumentSavingEventArgs e)
        {
        }

        /// <summary>
        /// AfterSave event will run after a normal save routine has occured and the OMSDocument is constructed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [ScriptMethodOverridable()]
        protected virtual void Saved(object sender, DocumentSavedEventArgs e)
        {
        }

        /// <summary>
        /// This Virtual Method will fire on the Construction of a Precedent, this will enable a precedent
        /// to cancel the continuation of the process due to a validation based error.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns>if validation is succesfull then return true</returns>
        [ScriptMethodOverridable()]
        protected virtual void Validating(object sender, PrecedentLinkCancelEventArgs e)
        {
        }

        /// <summary>
        /// Create a System Merge File that can be used for additional fields, this can be overidden to run a 
        /// create dataset method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [ScriptMethodOverridable()]
        protected virtual void SecondStageMerging(object sender, PrecedentSecondStageMergingEventArgs e)
        {
        }

        /// <summary>
        /// A method that gets called before the primary merge is called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        [ScriptMethodOverridable()]
        protected virtual void Merging(object sender, PrecedentLinkCancelEventArgs e)
        {
        }

        /// <summary>
        /// Load of a Precedent Object, will run before validate, Useful for running a process wizard before use 
        /// or for adding potential data.
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual void Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// A method that gets called once the physical document has saved
        /// </summary>
        [ScriptMethodOverridable()]
        public virtual void DocumentSaved(object sender, PhysicalDocumentSavedEventArgs e)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the internal precedent object.
        /// </summary>
        /// <param name="precedent">Precedent object to use.</param>
        internal void SetPrecedentObject(Precedent precedent)
        {
            if (_prec == null)
            {
                _prec = precedent;

                _prec.Load -= new EventHandler(CurrentScript.Load);
                _prec.Validating -= new PrecedentLinkCancelHandler(CurrentScript.Validating);
                _prec.Merging -= new PrecedentLinkCancelHandler(CurrentScript.Merging);
                _prec.SecondStageMerging -= new PrecedentSecondStageMergingEventHandler(CurrentScript.SecondStageMerging);
                _prec.DocumentSaving -= new DocumentSavingHandler(CurrentScript.Saving);
                _prec.DocumentSaved -= new DocumentSavedHandler(CurrentScript.Saved);
                _prec.PhysicalDocumentSaved -= new PhysicalDocumentSavedEventHandler(CurrentScript.DocumentSaved);

                _prec.Load += new EventHandler(CurrentScript.Load);
                _prec.Validating += new PrecedentLinkCancelHandler(CurrentScript.Validating);
                _prec.Merging += new PrecedentLinkCancelHandler(CurrentScript.Merging);
                _prec.SecondStageMerging += new PrecedentSecondStageMergingEventHandler(CurrentScript.SecondStageMerging);
                _prec.DocumentSaving += new DocumentSavingHandler(CurrentScript.Saving);
                _prec.DocumentSaved += new DocumentSavedHandler(CurrentScript.Saved);
                _prec.PhysicalDocumentSaved += new PhysicalDocumentSavedEventHandler(CurrentScript.DocumentSaved);
            }
        }

        #endregion
    }
}
