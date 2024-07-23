using System;
using iManage.Work.Tools;

namespace iManageWork10.Shell.RestAPI
{
    public sealed class PlugInRestApiWorkerProvider : RestApiWorkerProvider
    {
        private readonly PlugInBase _plugInBase;

        private readonly IPlugInHost _plugInHost;
        
        public PlugInRestApiWorkerProvider(PlugInBase plugInBase, IPlugInHost plugInHost)
        {
            if (plugInBase == null)
            {
                throw new ArgumentNullException(nameof(plugInBase));
            }
            if (plugInHost == null)
            {
                throw new ArgumentNullException(nameof(plugInHost));
            }

            _plugInBase = plugInBase;
            _plugInHost = plugInHost;
        }

        public override RestApiWorker GetRestApiWorker()
        {
            return new PlugInRestApiWorker(_plugInBase, _plugInHost);
        }
    }
}
