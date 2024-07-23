using System.Collections.Generic;
using Fwbs.Office.Outlook;

namespace Fwbs.Oms.Office.Outlook
{
    internal class InspectorWrapper
    {
        private static List<InspectorWrapper> wrappers = new List<InspectorWrapper>();

        private Common.OfficeOMSAddin Addin;
        private OutlookInspector Inspector;

        public InspectorWrapper(OutlookInspector inspector, Common.OfficeOMSAddin addin)
        {
            Addin = addin;
            Inspector = inspector;

            AttachInspectorEvents();

            wrappers.Add(this);

        }

        private void DetachInspectorsEvents()
        {
            Inspector.Activated -= new Microsoft.Office.Interop.Outlook.InspectorEvents_10_ActivateEventHandler(OutlookOMSAddin_Activate);
            Inspector.Closed -= new Microsoft.Office.Interop.Outlook.InspectorEvents_10_CloseEventHandler(OutlookOMSAddin_Close);
        }

        private void AttachInspectorEvents()
        {
            Inspector.Activated += new Microsoft.Office.Interop.Outlook.InspectorEvents_10_ActivateEventHandler(OutlookOMSAddin_Activate);
            Inspector.Closed += new Microsoft.Office.Interop.Outlook.InspectorEvents_10_CloseEventHandler(OutlookOMSAddin_Close);
        }

         void OutlookOMSAddin_Close()
        {
            DetachInspectorsEvents();

             int nPanes = Addin.Panes.Count -1;

             for (int n = nPanes; n >= 0; n--)
             {
                 if (Addin.Panes[n].Window == Inspector.InternalObject)
                     Addin.Panes[n].Dispose();
             }
             

             if (wrappers.Contains(this))
                wrappers.Remove(this);
        }

        void OutlookOMSAddin_Activate()
        {
            //get the current window
            Addin.RefreshUI(false,Inspector.InternalObject);
        }

    }
}
