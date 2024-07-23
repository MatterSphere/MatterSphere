using System;

namespace FWBS.OMS.Connectivity
{
    [System.Runtime.InteropServices.Guid("2FEAF796-EF19-4ad4-B8A8-50CAD3B9CB41")]
    public sealed class DatabaseService : ConnectableService
    {
        private FWBS.OMS.Data.Connection connection = null;


        public DatabaseService(string name, FWBS.OMS.Data.Connection connection) : base(name, true)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            this.connection = connection;

            StartPolling();
        }

        protected override bool InternalDependsOn(IConnectableService service)
        {
           if (service.Id == ConnectivityManager.NETWORK_SERVICE)
                return true;
            else
                return false;
        }

        protected override void InternalTest()
        {
            connection.Connect();
            connection.Disconnect();
        }
    }
}
