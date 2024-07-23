using System;
using System.Windows.Forms;
using FWBS.OMS.UI.UserControls.Browsers.Api;
using FWBS.OMS.UI.UserControls.Dashboard.CellControls.Common;

namespace FWBS.OMS.FinancialTile
{
    class PageProvider : IPageProvider
    {
        private const string CLIENT_FIN_OMS_CODE = "CLNFINHDR";
        private const string MATTER_FIN_OMS_CODE = "MTTRFINHDR";
        private DataProvider _dataProvider;

        private const string transactionService2xCode = "E3EURLTRANSSERV2";
        private const string transactionService3xCode = "E3EURLTRANSSERV3";
        private const string restApiOnPremCode = "E3EURLAPIONPREM";
        private const string restApiCloudCode = "E3EURLAPICLOUD";

        private const string transactionService2xDefault = "webui/TransactionService.asmx";
        private const string transactionService3xDefault = "web/TransactionService.asmx";
        private const string restApiOnPremDefault = "web/api/";
        private const string restApiCloudDefault = "3e/integration/";

        private static string GetRelativeUrl(string serviceType)
        {
            string result = Convert.ToString(Session.CurrentSession.GetSpecificData(serviceType));
            if (string.IsNullOrEmpty(result))
            {
                switch (serviceType)
                {
                    case transactionService2xCode:
                        result = transactionService2xDefault;
                        break;
                    case transactionService3xCode:
                        result = transactionService3xDefault;
                        break;
                    case restApiOnPremCode:
                        result = restApiOnPremDefault;
                        break;
                    case restApiCloudCode:
                        result = restApiCloudDefault;
                        break;
                    default:
                        result = transactionService2xDefault;
                        break;
                }
            }

            return result;
        }

        public PageProvider()
        {
            Headers = new[]
            {
                CLIENT_FIN_OMS_CODE,
                MATTER_FIN_OMS_CODE
            };
        }

        public string[] Headers { get; }

        public BaseContainerPage GetPage(string header)
        {
            if (_dataProvider == null)
            {
                _dataProvider = CreateProvider();
            }

            switch (header)
            {
                case CLIENT_FIN_OMS_CODE:
                    return new FinancialDashboardItem(FinancialDashboardItem.FinancialPageEnum.Clients, _dataProvider)
                    {
                        Code = CLIENT_FIN_OMS_CODE,
                        Title = CodeLookup.GetLookup("DASHBOARD", "CLNTS", "Clients"),
                        Dock = DockStyle.Fill
                    };
                case MATTER_FIN_OMS_CODE:
                    return new FinancialDashboardItem(FinancialDashboardItem.FinancialPageEnum.Matters, _dataProvider)
                    {
                        Code = MATTER_FIN_OMS_CODE,
                        Title = CodeLookup.GetLookup("DASHBOARD", "MTTRS", "Matters"),
                        Dock = DockStyle.Fill
                    };
            }

            throw new ArgumentOutOfRangeException();
        }

        public PageDetails GetDetails(string header)
        {
            switch (header)
            {
                case CLIENT_FIN_OMS_CODE:
                    return new PageDetails(CLIENT_FIN_OMS_CODE, CodeLookup.GetLookup("DASHBOARD", "CLNTS", "Clients"));
                case MATTER_FIN_OMS_CODE:
                    return new PageDetails(MATTER_FIN_OMS_CODE, CodeLookup.GetLookup("DASHBOARD", "MTTRS", "Matters"));
            }

            throw new ArgumentOutOfRangeException();
        }

        private static DataProvider CreateProvider()
        {
            DataProvider dataProvider = null;
            string baseUrl = Convert.ToString(Session.CurrentSession.GetSpecificData("__fdE3EBASEURL"));
            string endpoint = Convert.ToString(Session.CurrentSession.GetSpecificData("__fdE3EDASHBOARD"));

            if (string.IsNullOrWhiteSpace(baseUrl) || string.IsNullOrWhiteSpace(endpoint))
                return dataProvider;
            
            string env = Convert.ToString(Session.CurrentSession.GetSpecificData("__fdE3EAADENV"));
            string relativeUrl;
            string url;
            if (string.IsNullOrWhiteSpace(env))
            {
                relativeUrl = GetRelativeUrl(restApiOnPremCode);
                url = baseUrl.TrimEnd('/') + "/" + relativeUrl;

                bool useNetworkCredential = Common.ConvertDef.ToBoolean(Session.CurrentSession.GetSpecificData("__fdE3ECREDENTIALS"), false);
                if (useNetworkCredential)
                {
                    string host = new Uri(baseUrl).Host;
                    var credentialsDialog = new WindowsSecurityDialogApi(
                        string.Format("{0} : {1}", Branding.APPLICATION_NAME, host),
                        Session.CurrentSession.Resources.GetResource("ENTRCRDNTLS", "Enter your credentials", "").Text,
                        Session.CurrentSession.Resources.GetResource("CRDNTLWLSBEUSD", "These credentials will be used to connect to '%1%'", "", host).Text,
                        IntPtr.Zero);
                    bool authenticationUsedCredManager;
                    if (credentialsDialog.ValidateCredentials(out authenticationUsedCredManager))
                    {
                        dataProvider = new DataProvider(url, endpoint, credentialsDialog.Credentials);
                    }
                }
                else
                {
                    dataProvider = new DataProvider(url, endpoint, System.Net.CredentialCache.DefaultCredentials);
                }
            }
            else
            {
                try
                {
                    relativeUrl = GetRelativeUrl(restApiCloudCode);
                    url = baseUrl.TrimEnd('/') + "/" + relativeUrl;

                    byte[] bytes = Convert.FromBase64String(Convert.ToString(Session.CurrentSession.GetSpecificData("__fdE3EAADCLIENT")));
                    string[] value = System.Text.Encoding.UTF8.GetString(EncryptionV2.Decrypt(bytes, "__fdE3EAADCLIENT")).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (value.Length == 2)
                    {
                        var credentials = new System.Net.NetworkCredential(
                            value[0], //ClientId
                            value[1], //Client Secret
                            Convert.ToString(Session.CurrentSession.GetSpecificData("__fdE3EAADAUDIENCE")) + " " +
                            Convert.ToString(Session.CurrentSession.GetSpecificData("__fdE3EAADTENANTID")) + " " +
                            Convert.ToString(Session.CurrentSession.GetSpecificData("__fdE3EAADINSTANCEID")));
                        dataProvider = new DataProvider(url, endpoint, credentials, env);
                    }
                }
                catch { }
            }
            return dataProvider;
        }
    }
}
