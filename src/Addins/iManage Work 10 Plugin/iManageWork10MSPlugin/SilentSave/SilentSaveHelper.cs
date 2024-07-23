using System.Threading;
using iManage.Work.Tools;

namespace MatterSphereIntercept.SilentSave
{
    public static class SilentSaveHelper
    {
        public static void Set(this AutoResetEvent evnt, IPlugInHostLog wLog, string docid, string stage)
        {
            if (evnt != null)
            {
                evnt.Set();
#pragma warning disable 618
                wLog.Info($"Event [{evnt.Handle}] is set to complete silen save thread on stage '{stage}'. DocumentId is: {docid}");
#pragma warning restore 618
            }
        }
    }
}