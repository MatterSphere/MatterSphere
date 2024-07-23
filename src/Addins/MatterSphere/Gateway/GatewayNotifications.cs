using System;
using System.Net;
using System.ServiceModel.Description;
using FWBS.MatterSphereIntegration.Gateway;

namespace FWBS.MatterSphereIntegration
{


    public class GatewayNotifications
    {
        private long companyID = -1;
        
        public string EndPointAddress { get; set; }
        public string Passcode { get; set; }
        
        public long CompanyID
        {
            get { return companyID; }
            set { companyID = value; }
        }

        public string UserName { get; set; }

        public GatewayNotifications()
        { }

        public GatewayNotifications(string EndPointAddress, string Passcode, long CompanyID, string UserName)
        {
            this.EndPointAddress = EndPointAddress;
            this.Passcode=Passcode;
            this.CompanyID=CompanyID;
            this.UserName=UserName;
        }

        private GatewayAdminClient GetAdminClient()
        {
            if (string.IsNullOrEmpty(EndPointAddress))
                throw new Exception("EndPointAddress must be set before making a service request");

            if (string.IsNullOrEmpty(Passcode))
                throw new Exception("Passcode must be set before making a service request");

            if (CompanyID==-1) //not set
                throw new Exception("CompanyID must be set before making a service request");

            if (string.IsNullOrEmpty(UserName))
                throw new Exception("UserName must be set before making a service request");

            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();

            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;

            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(EndPointAddress);

            //Create a new client message insector to add headers to the service
            FWBS.MatterSphereIntegration.Gateway.ClientMessageInspector cli = new FWBS.MatterSphereIntegration.Gateway.ClientMessageInspector();

            cli.CompanyID = CompanyID;
            cli.Passcode = Passcode;
            cli.MachineName = System.Environment.MachineName;
            cli.UserName = UserName;

            SetProxyCredentials();

            var svc = new GatewayAdminClient(binding, address);

            svc.Endpoint.Behaviors.Add((IEndpointBehavior)cli);

            return svc;

        }       


        public GatewayResponse CreateInteractiveLawAccount(UserDetails user)
        {
            GatewayResponse ret = new GatewayResponse();

            using (GatewayAdminClient svc = GetAdminClient())
            {                
                AdminResponse resp = svc.RegisterRemoteUser(user);
                ret.Success = resp.Success;
                ret.ErrorMessage = resp.Message;
            }

            return ret;
        }

        public GatewayResponse CreateInteractiveLawAccount(string email)
        {
            UserDetails user = new UserDetails();
            user.MobileNumber = "";
            user.UserEmail = email;            

            return CreateInteractiveLawAccount(user);
        }

        public GatewayResponse RemoveInteractiveLawAccount(UserDetails user)
        {
            GatewayResponse ret = new GatewayResponse();

            using (GatewayAdminClient svc = GetAdminClient())
            {   
                AdminResponse resp = svc.RevokeRemoteUser(user);
                ret.Success = resp.Success;
                ret.ErrorMessage = resp.Message;
            }

            return ret;
        }

        public GatewayResponse RemoveInteractiveLawAccount(string email)
        {
            UserDetails user = new UserDetails();
            user.MobileNumber = "";
            user.UserEmail = email;

            return RemoveInteractiveLawAccount(user);
        }

        public FeeEarnerListRespose ListFeeEarners()
        {
            using (GatewayAdminClient svc = GetAdminClient())
            {
                return svc.ListRemoteFeeEarners();
            }
        }

        public AccountDetails GetCompanyAccountDetails()
        {
            using (GatewayAdminClient svc = GetAdminClient())
            {
                return svc.ListInteractiveLawAccountDetails();
            }
        }

        public AdminResponse UpdateCompanyAccountDetails(AccountDetails details)
        {
            using (GatewayAdminClient svc = GetAdminClient())
            {
                return svc.UpdateInteractiveLawAccountDetails(details);
            }
        }


    



        public GatewayResponse GrantFeeEarnerAccess(FeeEarnerMobileRequest fReq)
        {
            GatewayResponse ret = new GatewayResponse();

            using (GatewayAdminClient svc = GetAdminClient())
            {
                FeeEarnerDetails fed = GetFeeEarnerDetails(fReq);

                AdminResponse resp = svc.GrantFeeEarner(fed);

                ret.Success = resp.Success;
                ret.ErrorMessage = resp.Message;
            }

            return ret;
        }


        public GatewayResponse RevokeFeeEarnerAccess(FeeEarnerMobileRequest fReq)
        {
            GatewayResponse ret = new GatewayResponse();

            using (GatewayAdminClient svc = GetAdminClient())
            {
                FeeEarnerDetails fed = GetFeeEarnerDetails(fReq);
                
                AdminResponse resp = svc.RevokeFeeEarner(fed);

                ret.Success = resp.Success;
                ret.ErrorMessage = resp.Message;
            }

            return ret;
        }


        private static void SetProxyCredentials()
        {
            if (WebRequest.DefaultWebProxy != null)
                WebRequest.DefaultWebProxy.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
        }


        private FeeEarnerDetails GetFeeEarnerDetails(FeeEarnerMobileRequest fReq)
        {
            FeeEarnerDetails fed = new FeeEarnerDetails();
            MobileDevice m = new MobileDevice();

            m.SerialNumber = fReq.SerialNumber;
            fed.Device = m;
            fed.UserEmail = fReq.EmailAddress;

            return fed;
        }


    }


   

    




}
