using System;
using System.Runtime.InteropServices;
using System.Threading;
using iManage.Work.Tools;
using iManageWork10.Shell.JsonResponses;

namespace MatterSphereIntercept.SilentSave
{
    public class SilentSaveThreadStateInfo : IDisposable
    {

        public SilentSaveThreadStateInfo(dynamic document, IWDocumentProfile documentProfile)
        {
            Document = IncreaseRefCount(document);
            DocumentProfile = documentProfile;
            AutoEvent = new AutoResetEvent(false);
        }

        public dynamic Document { get; private set; }

        public IWDocumentProfile DocumentProfile { get; }
     
        public AutoResetEvent AutoEvent { get; private set; }
        
        public DocumentCheckOut DocumentCheckOut { get; set; }

        public void Dispose()
        {
            if (Document != null)
            {
                Marshal.ReleaseComObject(Document);
                Document = null;
            }

            if (AutoEvent != null)
            {
                AutoEvent.Dispose();
                AutoEvent = null;
            }
        }

        private static dynamic IncreaseRefCount(dynamic obj)
        {
            IntPtr ptr = Marshal.GetIUnknownForObject(obj);
            obj = Marshal.GetObjectForIUnknown(ptr);
            Marshal.Release(ptr);
            return obj;
        }
        
    }
}