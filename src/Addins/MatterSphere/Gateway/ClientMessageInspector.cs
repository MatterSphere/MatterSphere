using System;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace FWBS.MatterSphereIntegration.Gateway
{
    public class ClientMessageInspector : IClientMessageInspector, IEndpointBehavior
    {

        #region IClientMessageInspector Members

        public Int64 CompanyID = 0;
        public string Passcode = null;
        public string UserName = null;
        public string MachineName = null;


        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        { }

        /// <summary>
        /// Override to add a custom header
        /// </summary>
        /// <param name="request"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {
            if (!String.IsNullOrEmpty(Passcode))
            {
                //add company passcode to header
                MessageHeader<string> tokenStr = new MessageHeader<string>(Passcode, false, "", true);
                MessageHeader passcodeHeader = tokenStr.GetUntypedHeader("Passcode", "urn:GatewayAdminServices");
                request.Headers.Add(passcodeHeader);
            }
            else
            {
                throw new InvalidMessageContractException("Passcode must be specified in the Headers.");
            }

            //add company ID
            MessageHeader<Int64> companyStr = new MessageHeader<Int64>(CompanyID, false, "", true);
            MessageHeader companyIDHeader = companyStr.GetUntypedHeader("CompanyID", "urn:GatewayAdminServices");
            request.Headers.Add(companyIDHeader);

            // usrname not required to validate but nice to have for logging
            if (!String.IsNullOrEmpty(UserName))
            {
                //add company passcode to header
                MessageHeader<string> userStr = new MessageHeader<string>(UserName, false, "", true);
                MessageHeader userHeader = userStr.GetUntypedHeader("UserName", "urn:GatewayAdminServices");
                request.Headers.Add(userHeader);
            }

            //machine name is another nice to have but not essential or validation only used in logging
            if (!String.IsNullOrEmpty(MachineName))
            {
                //add company passcode to header
                MessageHeader<string> machineStr = new MessageHeader<string>(MachineName, false, "", true);
                MessageHeader machineHeader = machineStr.GetUntypedHeader("MachineName", "urn:GatewayAdminServices");
                request.Headers.Add(machineHeader);
            }

            //always add client IP to header
            IPHostEntry iphe = Dns.GetHostEntry(Dns.GetHostName());
            string ipAddr = iphe.AddressList[0].ToString();
            MessageHeader<string> ipStr = new MessageHeader<string>(ipAddr, false, "", true);
            MessageHeader ipHeader = ipStr.GetUntypedHeader("IPAddress", "urn:GatewayAdminServices");
            request.Headers.Add(ipHeader);
            return null;
        }


        #endregion

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add((IClientMessageInspector)this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        { }

        public void Validate(ServiceEndpoint endpoint)
        { }

        #endregion
    }
}
