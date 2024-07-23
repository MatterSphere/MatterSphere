using iManage.Work.Tools;

namespace iManageWork10.Shell.RestAPI
{
    public class PlugInRestApiWorker : RestApiWorker
    {

        private readonly PlugInBase _plugInBase;

        private readonly IPlugInHost _plugInHost;

        private IPlugInSession2 _plugInSession;

        public PlugInRestApiWorker(PlugInBase plugInBase, IPlugInHost plugInHost)
        {
            _plugInBase = plugInBase;
            _plugInHost = plugInHost;
        }

        public override string AuthToken
        {
            get
            {
                ValidateSession();
                return _plugInSession.AuthToken;
            }
        }

        public override string PreferredLibrary
        {
            get
            {
                ValidateSession();
                return _plugInSession.PreferredLibrary;
            }
        }

        public override IWHttpRequester GetWHttpRequester
        {
            get
            {
                ValidateSession();
                return _plugInSession.Factory.CreateHttpRequester();
            }
        }

        private void ValidateSession()
        {
            if (_plugInSession == null)
            {
                _plugInSession = _plugInHost.CreateSession2(_plugInBase);               
                if (!_plugInSession.Connected)
                {
                    _plugInSession.RenewAuthToken();
                }                
            }
        }

        public override bool Connect()
        {
            return _plugInSession.Connected;
        }

        public override bool Disconnect()
        {
            if (!_plugInSession.Connected)
            {
                return true;
            }
            _plugInSession.Logout();
            return !_plugInSession.Connected;
        }
    }
}
