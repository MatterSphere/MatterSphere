using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace FWBS.OMS.Script
{
    [InheritedExport(typeof(IReferenceLibrary))]
    internal abstract class BaseReferenceLibrary : IReferenceLibrary
    {
        private readonly List<IReference> references = new List<IReference>();

        
        public event EventHandler Loaded;

        public abstract string Name{get;}

 
        public virtual void Load()
        {
            if (IsLoaded)
            {
                OnLoaded();
                return;
            }

            try
            {
                IsLoaded = false;
                IsLoading = true;
                references.Clear();
                references.AddRange(BuildReferences());

            }
            finally
            {
                IsLoading = false;
            }

            OnLoaded();
        }

        public virtual void Refresh()
        {
            IsLoaded = false;
            Load();
        }

        protected void OnLoaded()
        {
            IsLoaded = true;

            var ev = Loaded;
            if (ev != null)
                ev(this, EventArgs.Empty);            
        }

        protected abstract IEnumerable<IReference> BuildReferences();

        public virtual IEnumerable<IReference> GetByDefinition(IScriptDefinition definition)
        {
            return references;
        }

        public bool IsLoading{get;private set;}

        public bool IsLoaded { get; private set; }

        public IEnumerable<IReference> References
        {
            get { return references.AsReadOnly(); }
        }

        public virtual int Count
        {
            get { return references.Count; }
        }


    }
}
