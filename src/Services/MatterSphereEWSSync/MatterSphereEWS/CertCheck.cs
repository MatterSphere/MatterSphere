using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Identity.Client;

namespace MatterSphereEWS
{
    public class CertCheck : IDisposable
    {

        #region Private Global Variables
        private ExchangeService exchangeService;
        private DateTimeOffset? oAuthExpiresOn;
        private string CertNumber;
        private Logging Log;
        #endregion

        #region BaseMethods
        public CertCheck()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckCertificate);
            SetExchangeService();
        }
        public string GetCertNumber()
        {

            try
            {
                String BuildNumerb = ConvertID(IdFormat.EntryId, IdFormat.EwsId, "bob@bob.com", "Fred");
            }
            catch
            { }
            return CertNumber;
        }

        private ExchangeVersion LowestSupportedExchangeVersion()
        {
            return ExchangeVersion.Exchange2007_SP1;
        }

        private bool CheckCertificate(Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            CertNumber = certificate.GetSerialNumberString();
            return true;
        }

        ExchangeService ExchangeService
        {
            get
            {
                if (oAuthExpiresOn.HasValue && oAuthExpiresOn.Value < DateTimeOffset.UtcNow)
                {
                    exchangeService.Credentials = new OAuthCredentials(GetOAuthToken());
                }
                return exchangeService;
            }
        }

        private void SetExchangeService()
        {
            if (exchangeService == null)
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                ExchangeVersion version = LowestSupportedExchangeVersion();
                exchangeService = new ExchangeService(version, TimeZoneInfo.Utc);
                string emailAddress = Config.GetConfigurationItem("ServiceEmailAddress");

                if (Config.GetConfigurationItemBool("UseOAuth"))
                {
                    exchangeService.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, emailAddress);
                    exchangeService.HttpHeaders.Add("X-AnchorMailbox", emailAddress);

                    exchangeService.Credentials = new OAuthCredentials(GetOAuthToken());
                }
                else if (Config.GetConfigurationItemBool("OverrideExchangeCredentials"))
                {
                    if (string.IsNullOrWhiteSpace(Config.GetConfigurationItem("ExcDomain")))
                        exchangeService.Credentials = new NetworkCredential(Config.GetConfigurationItem("ExcUserName"), Config.GetConfigurationItem("ExcPassword"));
                    else
                        exchangeService.Credentials = new NetworkCredential(Config.GetConfigurationItem("ExcUserName"), Config.GetConfigurationItem("ExcPassword"), Config.GetConfigurationItem("ExcDomain"));
                }
                else
                {
                    exchangeService.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
                if (Config.GetConfigurationItemBool("UseAutoDiscover"))
                {
                    exchangeService.AutodiscoverUrl(emailAddress);
                }
                else
                {
                    exchangeService.Url = new Uri(Config.GetConfigurationItem("ExchangeWebServicesURL"));
                }
            }
        }

        private string GetOAuthToken()
        {
            var cca = ConfidentialClientApplicationBuilder
                .Create(Config.GetConfigurationItem("OAuthAppId"))
                .WithClientSecret(Config.GetConfigurationItem("OAuthClientSecret"))
                .WithTenantId(Config.GetConfigurationItem("OAuthTenantId"))
                .Build();

            try
            {
                var ewsScopes = new string[] { "https://outlook.office365.com/.default" };
                var authResult = cca.AcquireTokenForClient(ewsScopes).ExecuteAsync().Result;
                oAuthExpiresOn = authResult.ExpiresOn.AddMinutes(-1);
                return authResult.AccessToken;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }

        private string ConvertID(IdFormat inputFormat, IdFormat outputFormat, String MailBoxName, String orginalIDValue)
        {
            try
            {
                AlternateId alternateID = new AlternateId(inputFormat, orginalIDValue, MailBoxName);
                AlternateIdBase ewsResponse = ExchangeService.ConvertId(alternateID, outputFormat);
                AlternateId alternateIDOutput = (AlternateId)ewsResponse;
                return alternateIDOutput.UniqueId.ToString();
            }
            catch
            {
                return orginalIDValue;
            }
        }
       
        public void Dispose()
        {
            if (Log != null) Log = null;
            exchangeService = null;
            if (Config.GetConfigurationItemBool("OverrideCertificateCheck"))
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback -= new RemoteCertificateValidationCallback(CheckCertificate);
            }
        }
        #endregion


     
    }
}
