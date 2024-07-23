using System;
using System.Collections.Generic;
using System.Linq;

namespace Fwbs.Office.Outlook
{
    using System.Diagnostics;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public sealed class OutlookExplorers : 
        OutlookObject,
        MSOutlook.Explorers,
        IEnumerable<OutlookExplorer>
    {

        #region Fields

        private Dictionary<MSOutlook.Explorer, OutlookExplorer> explorers = new Dictionary<MSOutlook.Explorer, OutlookExplorer>();
        private MSOutlook.Explorers exps;
        private readonly OutlookApplication app;

        #endregion

        #region Constructors

        internal OutlookExplorers(OutlookApplication app, MSOutlook.Explorers exps)
        {
            if (app == null)
                throw new ArgumentNullException("app");
            if (exps == null)
                throw new ArgumentNullException("exps");

            this.app = app;
            this.exps = exps ;
 
            Init(exps);
        }


        #endregion

        #region OfficeObject

        public override OutlookApplication Application
        {
            get
            {
                return app;
            }
        }

        protected override void Init(object obj)
        {
            base.Init(obj);

            foreach (var exp in exps.Cast<MSOutlook.Explorer>())
            {
                AddExplorer(exp);
            }


        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {

                    foreach (var exp in explorers.Values)
                        exp.Dispose();
                    explorers.Clear();

                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }


        protected override void OnAttachEvents()
        {
            base.OnAttachEvents();


            if (app != null)
            {
                exps.NewExplorer += Explorers_NewExplorer;
            }
        }

        protected override void OnDetachEvents()
        {
            base.OnDetachEvents();

            if (app != null)
            {
                try
                {
                    exps.NewExplorer -= Explorers_NewExplorer;
                }
                catch
                { }
            }

        }

        #endregion

        #region Captured Events


        private void Explorers_NewExplorer(MSOutlook.Explorer Explorer)
        {
            try
            {
                var exp = AddExplorer(Explorer);
                OnNewExplorer(exp);
                exp.Activate(true);
                
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "Outlook.Application.NewExplorer");
                throw;
            }
        }


        #endregion


        #region _Explorers Members

        public MSOutlook.Explorer Add(object Folder, MSOutlook.OlFolderDisplayMode DisplayMode)
        {
            var olfolder = Folder as OutlookFolder;
            if (olfolder != null)
                Folder = olfolder.InternalItem;

            var newexp = exps.Add(Folder, DisplayMode);

            return AddExplorer(newexp);
        }



        MSOutlook.Application MSOutlook._Explorers.Application
        {
            get {return Application; }
        }

        public MSOutlook.OlObjectClass Class
        {
            get { return exps.Class; }
        }

        public int Count
        {
            get { return explorers.Count; }
        }

        //TODO: Check
        public object Parent
        {
            get { return exps.Parent; }
        }

        public MSOutlook.NameSpace Session
        {
            get { return Application.Session; }
        }

        public MSOutlook.Explorer this[object Index]
        {
            get { return AddExplorer(exps[Index]); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            foreach (var exp in explorers.Values)
                yield return exp;

            yield break;
        }

        #endregion

        #region ExplorersEvents_Event Members

        public event MSOutlook.ExplorersEvents_NewExplorerEventHandler NewExplorer;

        private void OnNewExplorer(OutlookExplorer explorer)
        {
            var ev = NewExplorer;
            if (ev != null)
                ev(explorer);
        }

        #endregion

        #region Explorer Methods

        internal OutlookExplorer AddExplorer(MSOutlook.Explorer exp)
        {
            OutlookExplorer oexp = exp as OutlookExplorer;
            if (oexp != null)
                return oexp;

            if (exp == null)
                return null;

            if (explorers.ContainsKey(exp))
                return explorers[exp];
            else
            {
                var ew = new OutlookExplorer(this, exp);
                explorers.Add(exp, ew);
                return ew;
            }
        }


        internal void RemoveExplorer(MSOutlook.Explorer exp)
        {
            if (explorers.ContainsKey(exp))
            {
                var ew = explorers[exp];
                explorers.Remove(exp);
                ew.Dispose();
            }
        }

        #endregion

        #region IEnumerable<Explorer> Members

        IEnumerator<OutlookExplorer> IEnumerable<OutlookExplorer>.GetEnumerator()
        {
            foreach (var exp in explorers.Values)
                yield return exp;

            yield break;
        }

        #endregion
    }
}
