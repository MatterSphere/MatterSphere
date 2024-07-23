namespace FWBS.OMS.Connectivity
{
    using System.Net.NetworkInformation;

    [System.Runtime.InteropServices.Guid("2631510F-BAD1-4c8f-AAB5-4B66363DA97A")]
    public sealed class NetworkService : ConnectableService
    {
        private bool attached;

        public NetworkService()
            : base("Network", false)
        {
            if (Enabled)
            {
                NetworkChange.NetworkAvailabilityChanged += new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
                attached = true;
                StartPolling();
            }
        }

            
        private void NetworkChange_NetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
                OnConnected();
            else
                OnDisconnected(new MessageEventArgs("The network is not available."));
        }

        protected override void InternalTest()
        {
            
            if (!NetworkInterface.GetIsNetworkAvailable())
                throw new ConnectivityException(Session.CurrentSession.Resources.GetMessage("MSGNTWNTAVL", "The network is not available.", "").Text);
        }



        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (attached)
                    {
                        attached = false;
                        NetworkChange.NetworkAvailabilityChanged -= new NetworkAvailabilityChangedEventHandler(NetworkChange_NetworkAvailabilityChanged);
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
