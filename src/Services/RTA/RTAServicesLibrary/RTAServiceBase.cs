using System;
using System.Net;

namespace RTAServicesLibrary
{
    public class RTALoginDetails : ICloneable
    {
        public string Url { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string AsUser { get; set; }
        public int? MatterSphereUserId { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }


    public class RTAServiceBase
    {
        #region Non-public Fields

        private const string CLIENT_ID = "SH1029"; // Registered external software house for Thomson Reuters
        protected const string XML_HEADER_FILTER = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";

        #endregion Non-public Fields

        #region Public Fields

        //  User Auth Fields
        internal RTALoginDetails LoginDetails { get; private set; }
        internal ITokenStorageProvider TokenStorageProvider { get; private set; }

        //  Error Fields
        public PIPService.responseCode ErrorResponseCode { get; private set; }
        public string ErrorMessage { get; private set; }
        public string ErrorTrace { get; private set; }

        //  Validation Fields
        public string ValidationMessage { get; private set; }

        #endregion Public Fields

        #region Constructors

        static RTAServiceBase()
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loginDetails">Login details</param>
        /// <param name="tokenStorageProvider">Token storage provider</param>
        public RTAServiceBase(RTALoginDetails loginDetails, ITokenStorageProvider tokenStorageProvider)
        {
            if (loginDetails == null)
                throw new ArgumentNullException(nameof(loginDetails));

            LoginDetails = (RTALoginDetails)loginDetails.Clone();
            TokenStorageProvider = tokenStorageProvider;
        }

        #endregion Constructors

        #region Protected Methods

        /// <summary>
        /// Get RTA Service
        /// </summary>
        /// <returns></returns>
        protected PIPService.PIPService GetRTAService()
        {
            PIPService.PIPService service = new PIPService.PIPService();
            service.Url = LoginDetails.Url;
            service.Proxy = GetProxy(LoginDetails.Url);
            return service;
        }

        /// <summary>
        /// Get Access Token
        /// </summary>
        /// <returns>Access token</returns>
        protected string GetAccessToken()
        {
            TokenDetails tokenDetails = TokenStorageProvider.LoadToken(LoginDetails.MatterSphereUserId);
            if (tokenDetails != null && !tokenDetails.IsAccessTokenExpired)
            {
                return tokenDetails.AccessToken;
            }
            else if (tokenDetails == null || tokenDetails.IsRefreshTokenExpired)
            {
                tokenDetails = GetToken(tokenDetails);
            }
            else if (tokenDetails.IsAccessTokenExpired)
            {
                tokenDetails = RefreshToken(tokenDetails);
            }
            return tokenDetails.AccessToken;
        }

        /// <summary>
        /// Log Error Message
        /// </summary>
        /// <param name="response">Service response that contains information about an error.</param>
        /// <returns>Formatted error message.</returns>
        protected string LogErrorMessage(PIPService.response response)
        {
            ErrorResponseCode = response.code;
            ErrorMessage = response.message;
            ErrorTrace = response.trace;
            string errorDetails = string.IsNullOrEmpty(response.details) ? string.Empty : string.Format("Details:{0}{1}{0}{0}", Environment.NewLine, response.details);
            return string.Format("{1}.{0}{0}Message:{0}{2}{0}{0}{4}Trace:{0}{3}", Environment.NewLine, ErrorResponseCode, ErrorMessage, ErrorTrace, errorDetails);
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        /// Get Proxy
        /// </summary>
        /// <param name="RTAUrl"></param>
        /// <returns></returns>
        private IWebProxy GetProxy(string RTAUrl)
        {
            var proxy = WebRequest.DefaultWebProxy;
            Uri resource = new Uri(RTAUrl);

            // See what proxy is used for resource.
            Uri resourceProxy = proxy.GetProxy(resource);

            // Test to see whether a proxy was selected.
            if (resourceProxy == resource)
            {
                return null;
            }
            else
            {
                //  Set proxy to use the users default network credentials
                proxy.Credentials = CredentialCache.DefaultCredentials;
                return proxy;
            }
        }

        private TokenDetails GetToken(TokenDetails tokenDetails)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                PIPService.getToken request = new PIPService.getToken()
                {
                    userID = LoginDetails.UserName,
                    password = LoginDetails.UserPassword,
                    userAsID = LoginDetails.AsUser,
                    clientID = CLIENT_ID
                };
                PIPService.tokenResponse tokenResponse = service.getToken(request).tokenResponse;
                if (tokenResponse.code == PIPService.responseCode.Ok)
                {
                    tokenDetails = new TokenDetails()
                    {
                        AccessToken = tokenResponse.accessToken,
                        AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(Convert.ToInt32(tokenResponse.accessTokenExpiresIn) - 5*60),
                        RefreshToken = tokenResponse.refreshToken,
                        RefreshTokenExpiresAt = DateTime.UtcNow.AddSeconds(Convert.ToInt32(tokenResponse.refreshTokenExpiresIn) - 15*60)
                    };
                    TokenStorageProvider.StoreToken(LoginDetails.MatterSphereUserId, tokenDetails);
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(tokenResponse);
                    throw new Exception("Get Token " + errorMessage);
                }
            }
            return tokenDetails;
        }

        private TokenDetails RefreshToken(TokenDetails tokenDetails)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                PIPService.refreshToken request = new PIPService.refreshToken()
                {
                    userID = LoginDetails.UserName,
                    refreshToken1 = tokenDetails.RefreshToken,
                    userAsID = LoginDetails.AsUser,
                    clientID = CLIENT_ID
                };
                PIPService.tokenResponse tokenResponse = service.refreshToken(request).tokenResponse;
                if (tokenResponse.code == PIPService.responseCode.Ok)
                {
                    tokenDetails = new TokenDetails()
                    {
                        AccessToken = tokenResponse.accessToken,
                        AccessTokenExpiresAt = DateTime.UtcNow.AddSeconds(Convert.ToInt32(tokenResponse.accessTokenExpiresIn) - 5*60),
                        RefreshToken = tokenResponse.refreshToken,
                        RefreshTokenExpiresAt = DateTime.UtcNow.AddSeconds(Convert.ToInt32(tokenResponse.refreshTokenExpiresIn) - 15*60)
                    };
                    TokenStorageProvider.StoreToken(LoginDetails.MatterSphereUserId, tokenDetails);
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(tokenResponse);
                    throw new Exception("Refresh Token " + errorMessage);
                }
            }
            return tokenDetails;
        }

        #endregion Private Methods
    }


    public class RTAServicePassword : RTAServiceBase
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loginDetails">Login details</param>
        public RTAServicePassword(RTALoginDetails loginDetails) : base(loginDetails, null)
        {
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Change A2A password.
        /// </summary>
        /// <param name="newPassword"></param>
        public void ChangePassword(string newPassword)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                PIPService.changePassword request = new PIPService.changePassword()
                {
                    userID = LoginDetails.UserName,
                    oldPassword = LoginDetails.UserPassword,
                    newPassword = newPassword
                };
                PIPService.stringResponse response = service.changePassword(request).stringResponse;
                if (response.code != PIPService.responseCode.Ok)
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response);
                    throw new Exception("Change Password " + errorMessage);
                }
            }
        }

        #endregion Public Methods
    }
}
