using System;
using System.Web;
using iManage.Work.Tools;
using iManage.Work.Tools.Host;
using iManageWork10.Shell.RestAPI;

namespace iManageWork10HostRestApi
{
    public class HostRestApiWorker : RestApiWorker
    {
        private const int CANCELED_BY_USER_ERROR_CODE = 0;

        private const int UNSUCCESSFUL_LOGIN_WITH_EMPTY_CREDENTIALS_ERROR_CODE = 1;

        private const int ILLEGAL_CREDENTIALS_ERROR_CODE = 401;

        private readonly string _serverUrl;
        private readonly string _clientId;
        private readonly string _extensionId;
        private readonly IWHostSession2 _wHostSession;

        public HostRestApiWorker(string serverUrl, string clientId = null, string extensionId = null)
        {
            _serverUrl = serverUrl;
            _clientId = clientId;
            _extensionId = extensionId;
            if (string.IsNullOrEmpty(_extensionId))
            {
                if (string.IsNullOrEmpty(_clientId))
                {
                    _wHostSession = string.IsNullOrEmpty(_serverUrl) ?
                        WHostFactory.CreateEmptySession2() :
                        WHostFactory.CreateSession2(_serverUrl);
                }
                else
                {
                    _wHostSession = string.IsNullOrEmpty(_serverUrl) ?
                        WHostFactory.CreateEmptyOAuth2Session(_clientId) :
                        WHostFactory.CreateOAuth2Session(serverUrl, _clientId);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(_clientId))
                {
                    _wHostSession = string.IsNullOrEmpty(_serverUrl) ?
                        WHostFactory.CreateEmptySession2() :
                        WHostFactory.CreateSession2(_serverUrl);
                }
                else
                {
                    _wHostSession = string.IsNullOrEmpty(_serverUrl) ?
                        WHostFactory.CreateEmptyOAuth2SessionWithExtensionID(_clientId, _extensionId) :
                        WHostFactory.CreateOAuth2SessionWithExtensionID(serverUrl, _clientId, _extensionId);
                }
            }
        }

        public override string AuthToken
        {
            get
            {
                ValidateConnection();
                return _wHostSession.AuthToken;
            }
        }

        public override string PreferredLibrary
        {
            get
            {
                ValidateConnection();
                return _wHostSession.PreferredLibrary;
            }
        }

        public override bool Connect()
        {
            while (!_wHostSession.Connect())
            {
                if (_wHostSession.LastErrorCode == CANCELED_BY_USER_ERROR_CODE)
                {
                    return false;
                }

                if (_wHostSession.LastErrorCode == ILLEGAL_CREDENTIALS_ERROR_CODE || _wHostSession.LastErrorCode == UNSUCCESSFUL_LOGIN_WITH_EMPTY_CREDENTIALS_ERROR_CODE)
                {
                    continue;
                }

                throw new HttpException(_wHostSession.LastErrorCode, $"Error Code: {_wHostSession.LastErrorCode}{Environment.NewLine}Error Message: {_wHostSession.LastError}");
            }
            return true;
        }

        public override bool Disconnect()
        {
            return !_wHostSession.Connected || _wHostSession.Disconnect();
        }

        public override IWHttpRequester GetWHttpRequester
        {
            get
            {
                ValidateConnection();
                return WHostFactory.CreateHttpRequester(_wHostSession);
            }
        }

        private void ValidateConnection()
        {
            if (!_wHostSession.Connected)
            {
                Connect();
            }
        }
    }
}