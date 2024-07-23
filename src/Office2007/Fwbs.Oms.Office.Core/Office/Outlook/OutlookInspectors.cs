using System;
using System.Collections.Generic;
using System.Linq;

namespace Fwbs.Office.Outlook
{
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using MSOutlook = Microsoft.Office.Interop.Outlook;

    public sealed class OutlookInspectors :
        OutlookObject,
        MSOutlook.Inspectors,
        IEnumerable<OutlookInspector>
    {

        #region Fields

        private Dictionary<MSOutlook.Inspector, OutlookInspector> inspectors = new Dictionary<MSOutlook.Inspector, OutlookInspector>();
        private MSOutlook.Inspectors insps;
        private readonly OutlookApplication app;

        #endregion

        #region Constructors

        internal OutlookInspectors(OutlookApplication app, MSOutlook.Inspectors insps)
        {
            if (app == null)
                throw new ArgumentNullException("app");
            if (insps == null)
                throw new ArgumentNullException("insps");

            this.app = app;
            this.insps = insps;
 
            Init(insps);
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


            foreach (var insp in insps.Cast<MSOutlook.Inspector>())
            {
                AddInspector(insp);
            }

        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    
                    foreach (var insp in inspectors.Values)
                        insp.Dispose();
                    inspectors.Clear();

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
                insps.NewInspector += olinsps_NewInspector;

            }
        }

        protected override void OnDetachEvents()
        {
            base.OnDetachEvents();

            if (app != null)
            {
                try
                {
                    insps.NewInspector -= olinsps_NewInspector;
                }
                catch 
                { }
            }

        }

        #endregion

        #region Captured Events

        private void olinsps_NewInspector(MSOutlook.Inspector Inspector)
        {
            try
            {
                if (!Application.Settings.IsConnected())
                    return;

                OnNewInspector(AddInspector(Inspector));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex, "Outlook.Application.NewInspector");
                throw;
            }
        }

        #endregion

        #region _Inspectors Members

        public MSOutlook.Inspector Add(object Item)
        {
            return AddInspector(insps.Add(Item));
        }



        MSOutlook.Application MSOutlook._Inspectors.Application
        {
            get { return Application; }
        }

        public MSOutlook.OlObjectClass Class
        {
            get { return app.Class; }
        }

        public int Count
        {
            get { return inspectors.Count; }
        }

        public object Parent
        {
            get { return insps.Parent; }
        }

        public MSOutlook.NameSpace Session
        {
            get { return Application.Session; }
        }

        public MSOutlook.Inspector this[object Index]
        {
            get { return AddInspector(insps[Index]); }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            foreach (var exp in inspectors.Values)
                yield return exp;

            yield break;
        }

        #endregion

        #region InspectorsEvents_Event Members

        public event MSOutlook.InspectorsEvents_NewInspectorEventHandler NewInspector;

        private void OnNewInspector(OutlookInspector inspector)
        {
            var ev = NewInspector;
            if (ev != null)
                ev(inspector);
        }


        #endregion

        #region Inspector Methods


        internal OutlookInspector AddInspector(MSOutlook.Inspector insp)
        {
            OutlookInspector oinsp = insp as OutlookInspector;
            if (oinsp != null)
                return oinsp;

            if (insp == null)
                return null;

            if (inspectors.ContainsKey(insp))
                return inspectors[insp];
            else
            {
                var iw = new OutlookInspector(this, insp);
                inspectors.Add(insp, iw);
                try
                {
                    var item = GetItem(() => insp.CurrentItem);
                    item.Inspector = iw;
                }
                catch (COMException comex)
                {
                    if (comex.ErrorCode != HResults.E_QUOTA_EXCEEDED)
                        throw;
                }
                return iw;
            }
        }


        internal void RemoveInspector(MSOutlook.Inspector insp)
        {
            if (inspectors.ContainsKey(insp))
            {
                var ew = inspectors[insp];
                inspectors.Remove(insp);
                ew.Dispose();
            }
        }

 

        #endregion


        #region IEnumerable<Explorer> Members

        IEnumerator<OutlookInspector> IEnumerable<OutlookInspector>.GetEnumerator()
        {
            foreach (var insp in inspectors.Values)
                yield return insp;

            yield break;
        }

        #endregion
    }
}
