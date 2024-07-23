namespace FWBS.OMS.Connectivity
{
    [System.Runtime.InteropServices.Guid("A108018F-D7B8-49b2-A439-9680BA787E55")]
    public sealed class OMSService : ConnectableService
    {
        public OMSService()
            : base("Database", false)
        {
            StartPolling();
        }

        protected override void InternalTest()
        {
            using (OMS.Data.Connection cnn = (OMS.Data.Connection)Session.CurrentSession.Connection.Clone())
            {
                cnn.ExecuteSQLScalar("select count(brid) from dbreginfo", null);
            }
        }

        protected override bool InternalDependsOn(IConnectableService service)
        {
            if (service.Id == ConnectivityManager.NETWORK_SERVICE)
                return true;
            else
                return false;
        }

        protected override void InternalOnDependentEvent(IConnectableService service, ConnectivityEvent serviceEvent)
        {
            if (service.Id == ConnectivityManager.NETWORK_SERVICE)
            {
                switch (serviceEvent)
                {
                    case ConnectivityEvent.Connected:
                        {
                            if (!IsConnected)
                                Test();
                        }
                        break;
                    case ConnectivityEvent.Disconnected:
                        {
                            if (IsConnected)
                                Test();
                        }
                        break;
                }
            }

            base.InternalOnDependentEvent(service, serviceEvent);
        }

    }
}
