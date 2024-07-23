using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using ELPLServicesLibrary.PIPService;

namespace ELPLServicesLibrary
{
    public class ELPLServices2 : ELPLServiceBase
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loginDetails">Login details</param>
        /// <param name="tokenStorageProvider">Token storage provider</param>
        public ELPLServices2(ELPLLoginDetails loginDetails, ITokenStorageProvider tokenStorageProvider) :
            base(loginDetails, tokenStorageProvider)
        {
            if (tokenStorageProvider == null)
                throw new ArgumentNullException(nameof(tokenStorageProvider));
        }

        #endregion Constructors


        #region GenerateInterimSettlementPackRequest
        /// <summary>
        /// Generate Interim Settlement Pack Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GenerateInterimSettlementPackRequest(InterimSettlementPackRequest request)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(InterimSettlementPackRequest));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, request);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate Interim Settlement Pack Request Error: " + ex.Message);
            }
        }
        #endregion GenerateInterimSettlementPackRequest


        #region InterimSettlementPackRequest
        /// <summary>
        /// Get Interim Settlement Pack Request
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public InterimSettlementPackRequest GetInterimSettlementPackRequest(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(InterimSettlementPackRequest));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (InterimSettlementPackRequest)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack Request XML Error: " + ex.Message);
            }
        }

        #endregion InterimSettlementPackRequest


        #region GenerateInterimSettlementPackResponse
        /// <summary>
        /// Generate Interim Settlement Pack Response
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GenerateInterimSettlementPackResponse(InterimSettlementPackResponse response)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(InterimSettlementPackResponse));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, response);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate Interim Settlement Pack Response Error: " + ex.Message);
            }
        }
        #endregion GenerateInterimSettlementPackResponse


        #region GetInterimSettlementPackResponse
        /// <summary>
        /// Get Interim Settlement Pack Response
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public InterimSettlementPackResponse GetInterimSettlementPackResponse(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(InterimSettlementPackResponse));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (InterimSettlementPackResponse)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack Response XML Error: " + ex.Message);
            }
        }
        #endregion GetInterimSettlementPackResponse


        #region GetInterimSettlementPackFromXML
        /// <summary>
        /// Get InterimSettlement Pack From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public Data GetInterimSettlementPackFromXML(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack XML Error: " + ex.Message);
            }
        }

        #endregion GetInterimSettlementPackFromXML


        #region GetDataFromXML
        /// <summary>
        /// Get Data From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public Data GetDataFromXML(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Data XML Error: " + ex.Message);
            }
        }

        #endregion GetDataFromXML


        #region GetStage2SettlementPackFromXML
        /// <summary>
        /// Get Stage 2 Settlement Pack From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public Data GetStage2SettlementPackFromXML(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Stage 2 Settlement Pack XML Error: " + ex.Message);
            }
        }
        #endregion GetStage2SettlementPackFromXML


        #region GetCourtProceedingsPackFromXML
        /// <summary>
        /// Get Court Proceedings Pack From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public Data GetCourtProceedingsPackFromXML(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Court Proceedings Pack XML Error: " + ex.Message);
            }
        }

        #endregion GetCourtProceedingsPackFromXML


        #region GetTimeOutsFromXML
        /// <summary>
        /// Get Timeouts From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public Data GetTimeOutsFromXML(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Timeouts XML Error: " + ex.Message);
            }
        }

        #endregion GetTimeOutsFromXML


        #region GenerateStage2SettlementPackRequest
        /// <summary>
        /// Generate Stage 2 Settlement Pack Request
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackRequest(Stage2SettlementPackRequest packRequest)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Stage2SettlementPackRequest));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, packRequest);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate Stage 2 Settlement Pack Request Error: " + ex.Message);
            }
        }
        #endregion GenerateStage2SettlementPackRequest


        #region GenerateStage2SettlementPackResponse
        /// <summary>
        /// Generate Stage 2 Settlement Pack Response
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackResponse(Stage2SettlementPackResponse packResponse)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Stage2SettlementPackResponse));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, packResponse);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate Stage 2 Settlement Pack Response Error: " + ex.Message);
            }
        }
        #endregion GenerateStage2SettlementPackResponse


        #region GenerateStage2SettlementPackCounterOfferByCR
        /// <summary>
        /// Generate Stage 2 Settlement Pack Counter Offer By CR
        /// </summary>
        /// <param name="counterOffer"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackCounterOfferByCR(Stage2SettlementPackCounterOfferByCR counterOffer)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Stage2SettlementPackCounterOfferByCR));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, counterOffer);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate Stage 2 Settlement Pack Counter Offer By CR Error: " + ex.Message);
            }
        }

        #endregion GenerateStage2SettlementPackCounterOfferByCR


        #region GenerateStage2SettlementPackCounterOfferByCM
        /// <summary>
        /// Generate Stage 2 Settlement Pack Counter Offer By CM
        /// </summary>
        /// <param name="counterOffer"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackCounterOfferByCM(Stage2SettlementPackCounterOfferByCM counterOffer)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Stage2SettlementPackCounterOfferByCM));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, counterOffer);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate Stage 2 Settlement Pack Counter Offer By CM Error: " + ex.Message);
            }
        }

        #endregion GenerateStage2SettlementPackCounterOfferByCM


        #region GenerateCourtProceedingPackRequest
        /// <summary>
        /// Generate Court Proceeding Pack Request
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateCourtProceedingPackRequest(CourtProceedingPackRequest packRequest)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CourtProceedingPackRequest));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, packRequest);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate Court Proceeding Pack Request Error: " + ex.Message);
            }
        }

        #endregion GenerateCourtProceedingPackRequest

        #region Public Methods

        #region GetClaimInterimSettlementPackList
        /// <summary>
        /// Get Claim Interim Settlement Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public InterimSettlementPack[] GetClaimInterimSettlementPackList(string applicationId)
        {
            using (PIPService.ELPLService service = GetELPLService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    Data data = GetInterimSettlementPackFromXML(response.stringResponse.value);

                    return data.InterimSettlementPackList;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Get Claim Interim Settlement Pack " + errorMessage);
                }
            }
        }
        #endregion GetClaimInterimSettlementPackList


        #region GetClaimStage2SettlementPack
        /// <summary>
        /// Get Claim Stage 2 Settlement Pack V3
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public Stage2SettlementPack GetClaimStage2SettlementPack(string applicationId)
        {
            using (PIPService.ELPLService service = GetELPLService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    Data data = GetStage2SettlementPackFromXML(response.stringResponse.value);

                    return data.Stage2SettlementPack;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Get Claim Stage 2 Settlement Pack " + errorMessage);
                }
            }
        }
        #endregion GetClaimStage2SettlementPack


        #region GetClaimCourtProceedingsPack
        /// <summary>
        /// Get Claim Court Proceedings Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public CourtProceedingsPack GetClaimCourtProceedingsPack(string applicationId)
        {
            using (PIPService.ELPLService service = GetELPLService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    Data data = GetCourtProceedingsPackFromXML(response.stringResponse.value);

                    return data.CourtProceedingsPack;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Get Claim Court Proceedings Pack " + errorMessage);
                }
            }
        }
        #endregion GetClaimCourtProceedingsPack


        #region GetClaimTimeOuts
        /// <summary>
        /// Get Claim TimeOuts
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public Timeouts GetClaimTimeOuts(string applicationId)
        {
            using (PIPService.ELPLService service = GetELPLService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    Data data = GetTimeOutsFromXML(response.stringResponse.value);

                    return data.Timeouts;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Get Claim TimeOuts " + errorMessage);
                }
            }
        }
        #endregion GetClaimTimeOuts


        #region GetClaimData
        /// <summary>
        /// Get Claim Data for V3 claims
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public Data GetClaimData(string applicationId)
        {
            using (PIPService.ELPLService service = GetELPLService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    return GetDataFromXML(response.stringResponse.value);
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Get Claim Data " + errorMessage);
                }
            }
        }
        #endregion GetClaimData

        #endregion Public Methods

        #region Stage 2.1 Public Methods

        #region SetInterimPaymentNeeded
        /// <summary>
        /// Set Interim Payment Needed
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public claimInfo SetInterimPaymentNeeded(string activityEngineGuid, string applicationId, bool isInterimPaymentNeeded)
        {
            //  This request allows the CR to add an Interim Settlement Pack Form Request for a claim.
            using (PIPService.ELPLService service = GetELPLService())
            {
                setInterimPaymentNeeded payment = new setInterimPaymentNeeded();
                payment.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                payment.claimData = data;
                payment.isInterimPaymentNeeded = isInterimPaymentNeeded;

                setInterimPaymentNeededResponse response = service.setInterimPaymentNeeded(payment);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Set Interim Payment Needed " + errorMessage);
                }
            }
        }
        #endregion SetInterimPaymentNeeded


        #region AddInterimSPFRequest
        /// <summary>
        /// Add Interim SPF Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public claimInfo AddInterimSPFRequest(string activityEngineGuid, string applicationId, InterimSettlementPackRequest packRequest)
        {
            //  This request allows the CR to add an Interim Settlement Pack Form Request for a claim.
            using (PIPService.ELPLService service = GetELPLService())
            {
                addInterimSPFRequest request = new addInterimSPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                //  Generate the InterimSettlementPackRequest into xml
                string xml = GenerateInterimSettlementPackRequest(packRequest);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                 applicationId, 
                                                                 ValidationServices.ProcessActions.InterimSettlementPackRequest);

                ValidationServices.ValidateXML(xml, xsd);
                request.ISPFRequestXML = xml;

                addInterimSPFRequestResponse response = service.addInterimSPFRequest(request);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add Interim SPF Request " + errorMessage);
                }
            }
        }
        #endregion AddInterimSPFRequest


        #region AddInterimSPFResponse
        /// <summary>
        /// Add Interim SPF Response
        /// </summary>
        /// <param name="spfResponse"></param>
        /// <returns></returns>
        public claimInfo AddInterimSPFResponse(string activityEngineGuid, string applicationId, InterimSettlementPackResponse packRequest)
        {
            //  This request allows the COMP to add an Interim Settlement Pack Form Response for a claim.
            using (PIPService.ELPLService service = GetELPLService())
            {
                addInterimSPFResponse spfResponse = new addInterimSPFResponse();
                spfResponse.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                spfResponse.claimData = data;

                string xml = GenerateInterimSettlementPackResponse(packRequest);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                 applicationId,
                                                                 ValidationServices.ProcessActions.InterimSettlementPackResponse);

                ValidationServices.ValidateXML(xml, xsd);
                spfResponse.ISPFResponseXML = xml;

                addInterimSPFResponseResponse response = service.addInterimSPFResponse(spfResponse);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add Interim SPF Response " + errorMessage);
                }
            }
        }
        #endregion AddInterimSPFResponse


        #region SetStage21Payments
        /// <summary>
        /// Set Stage 21 Payments
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public claimInfo SetStage21Payments(string activityEngineGuid, string applicationId, bool isStage21Paid)
        {
            //  This request allows a CR to simply indicate that the Payment for the given 
            //  Interim Settlement Pack Form has been received or not.
            using (PIPService.ELPLService service = GetELPLService())
            {
                setStage21Payment payment = new setStage21Payment();
                payment.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                payment.claimData = data;
                payment.isStage21Paid = isStage21Paid;

                setStage21PaymentResponse response = service.setStage21Payment(payment);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Set Stage 2.1 Payments " + errorMessage);
                }
            }
        }
        #endregion SetStage21Payments


        #region AcceptPartialInterimPayment
        /// <summary>
        /// Accept Partial Interim Payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public claimInfo AcceptPartialInterimPayment(string activityEngineGuid, string applicationId, bool isPartialInterimPaymentAccepted)
        {
            //  This request allows a CR to accept or not to accept the offer for a partial payment made by the Compensator.
            using (PIPService.ELPLService service = GetELPLService())
            {
                acceptPartialInterimPayment payment = new acceptPartialInterimPayment();
                payment.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                payment.claimData = data;
                payment.isPartialInterimPaymentAccepted = isPartialInterimPaymentAccepted;

                acceptPartialInterimPaymentResponse response = service.acceptPartialInterimPayment(payment);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Accept Partial Interim Payment " + errorMessage);
                }
            }
        }
        #endregion AcceptPartialInterimPayment


        #region AcknowledgePartialPaymentDecision
        /// <summary>
        /// Acknowledge Partial Payment Decision
        /// </summary>
        /// <param name="decision"></param>
        /// <returns></returns>
        public claimInfo AcknowledgePartialPaymentDecision(string activityEngineGuid, string applicationId)
        {
            //  This request allows a Compensator to simply indicate that they received the message with the 
            //  decision on the Partial Payment taken by the CR. Nothing else: it is simply.
            //  A step needed in the workflow.
            using (PIPService.ELPLService service = GetELPLService())
            {
                acknowledgePartialPaymentDecision decision = new acknowledgePartialPaymentDecision();
                decision.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                decision.claimData = data;

                acknowledgePartialPaymentDecisionResponse response = service.acknowledgePartialPaymentDecision(decision);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Partial Payment Decision " + errorMessage);
                }
            }
        }
        #endregion AcknowledgePartialPaymentDecision


        #region AcknowledgeInterimPaymentDecisionTimeout
        /// <summary>
        /// Acknowledge Interim Payment Decision Timeout
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeInterimPaymentDecisionTimeout(string activityEngineGuid, string applicationId)
        {
            //  This command is needed to ensure the CR acknowledges that the claim reached the time limit to take an interim payment decision. 
            //  The claim proceeds to Stage 2 Settlement Pack Form.

            using (PIPService.ELPLService service = GetELPLService())
            {
                acknowledgeInterimPaymentDecisionTimeout timeout = new acknowledgeInterimPaymentDecisionTimeout();
                timeout.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                timeout.claimData = data;

                acknowledgeInterimPaymentDecisionTimeoutResponse response = service.acknowledgeInterimPaymentDecisionTimeout(timeout);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Interim Payment Decision Timeout " + errorMessage);
                }
            }
        }
        #endregion AcknowledgeInterimPaymentDecisionTimeout


        #region ExtendInterimPaymentDecisionTimeout
        /// <summary>
        /// Extend Interim Payment Decision Timeout
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo ExtendInterimPaymentDecisionTimeout(string activityEngineGuid, string applicationId)
        {
            //  This request allows a Compensator to extend the timeframe needed to take a decision for the Staqe2SP Request, only if the Interym payment 
            //  requested by the Claimant representative is greater than £1000.
            //  In case of success, the date of timeout to take a decision is re-set to 30 business days from the date of sent of Interim payment request.
            //  NOTE: it is possible to extend the timeout only once: after the timeout has been successfully extended to 30 business days, it will not be possible to extend it anymore.

            using (PIPService.ELPLService service = GetELPLService())
            {
                extendInterimPaymentDecisionTimeout timeout = new extendInterimPaymentDecisionTimeout();
                timeout.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                timeout.claimData = data;

                extendInterimPaymentDecisionTimeoutResponse response = service.extendInterimPaymentDecisionTimeout(timeout);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Extend Interim Payment Decision Timeout " + errorMessage);
                }
            }
        }
        #endregion ExtendInterimPaymentDecisionTimeout


        #region RejectInterimSettlementPack
        /// <summary>
        /// Reject Interim Settlement Pack
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo RejectInterimSettlementPack(string activityEngineGuid, string applicationId, string rejectionComment)
        {
            //  This request allows the Compensator to reject the request of an Interim payment made by the Claimant Representative.

            using (PIPService.ELPLService service = GetELPLService())
            {
                rejectInterimSettlementPack pack = new rejectInterimSettlementPack();
                pack.accessToken = GetAccessToken();
                pack.rejectionComment = rejectionComment;

                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                pack.claimData = data;

                rejectInterimSettlementPackResponse response = service.rejectInterimSettlementPack(pack);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Reject Interim Settlement Pack " + errorMessage);
                }
            }
        }
        #endregion RejectInterimSettlementPack


        #region AcknowledgeRejectedInterimSettlementPack
        /// <summary>
        /// Acknowledge Rejected Interim Settlement Pack
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeRejectedInterimSettlementPack(string activityEngineGuid, string applicationId)
        {
            //  This request allows the Claimant Representative to simply indicate that they received the message with the rejection of Interim Settlement Pack Form. 
            //  Nothing else: it is simply a step needed in the workflow.

            using (PIPService.ELPLService service = GetELPLService())
            {
                acknowledgeRejectedInterimSettlementPack pack = new acknowledgeRejectedInterimSettlementPack();
                pack.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                pack.claimData = data;

                acknowledgeRejectedInterimSettlementPackResponse response = service.acknowledgeRejectedInterimSettlementPack(pack);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Rejected Interim Settlement Pack " + errorMessage);
                }
            }
        }
        #endregion AcknowledgeRejectedInterimSettlementPack


        #region ReturnToStartOfStage21
        /// <summary>
        /// Return to Start of Stage 2.1
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo ReturnToStartOfStage21(string activityEngineGuid, string applicationId)
        {
            //  This request allows the Claimant Representative to simply indicate that they want to move the claim back to the start of Stage 2.1. 
            using (PIPService.ELPLService service = GetELPLService())
            {
                returnToStartOfStage21 pack = new returnToStartOfStage21();
                pack.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                pack.claimData = data;

                returnToStartOfStage21Response response = service.returnToStartOfStage21(pack);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Return to Start of Stage 2.1 " + errorMessage);
                }
            }
        }
        #endregion ReturnToStartOfStage21


        #endregion Stage 2.1 Methods

        #region Stage 2.2 Public Methods

        #region AddStage2SPFRequest
        /// <summary>
        /// Add Stage 2 SPF Request
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="S2SPFRequestXML"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFRequest(string activityEngineGuid, string applicationId, Stage2SettlementPackRequest packRequest)
        {
            //  This request allows to add a Stage 2 Settlement Pack Form Request for a claim.
            using (PIPService.ELPLService service = GetELPLService())
            {
                addStage2SPFRequest request = new addStage2SPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                string xml = GenerateStage2SettlementPackRequest(packRequest);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                 applicationId,
                                                                 ValidationServices.ProcessActions.SettlementPackRequest);

                ValidationServices.ValidateXML(xml, xsd);
                request.S2SPFRequestXML = xml;

                addStage2SPFRequestResponse response = service.addStage2SPFRequest(request);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add Stage 2 SPF Request " + errorMessage);
                }
            }
        }
        #endregion AddStage2SPFRequest


        #region AddStage2SPFResponse
        /// <summary>
        /// Add Stage 2 SPF Response V3
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="S2SPFResponseXML"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFResponse(string activityEngineGuid, string applicationId, Stage2SettlementPackResponse packResponse)
        {
            //  This request allows to add a Stage 2 Settlement Pack Form Response for a claim.
            using (PIPService.ELPLService service = GetELPLService())
            {
                addStage2SPFResponse res = new addStage2SPFResponse();
                res.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                res.claimData = data;

                string xml = GenerateStage2SettlementPackResponse(packResponse);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                 applicationId,
                                                                 ValidationServices.ProcessActions.SettlementPackResponse);

                ValidationServices.ValidateXML(xml, xsd);
                res.S2SPFResponseXML = xml;

                addStage2SPFResponseResponse response = service.addStage2SPFResponse(res);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add Stage 2 SPF Response " + errorMessage);
                }
            }
        }
        #endregion AddStage2SPFResponse


        #region AcknowledgeStage2SPFRepudiation
        /// <summary>
        /// Acknowledge Stage 2 SPF Repudiation
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeStage2SPFRepudiation(string activityEngineGuid, string applicationId)
        {
            //  This request allows a Claimant Representative to simply indicate that they received the 
            //  message with the decision on the Stage 2 Settlement Pack Form taken by the Compensator. 
            //  Nothing else: it is simply a step needed in the workflow.
            using (PIPService.ELPLService service = GetELPLService())
            {
                acknowledgeStage2SPFRepudiation repudiation = new acknowledgeStage2SPFRepudiation();
                repudiation.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                repudiation.claimData = data;

                acknowledgeStage2SPFRepudiationResponse response = service.acknowledgeStage2SPFRepudiation(repudiation);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Stage 2 SPF Repudiation " + errorMessage);
                }
            }
        }
        #endregion AcknowledgeStage2SPFRepudiation


        #region AcknowledgeStage2SPFConfirmation
        /// <summary>
        /// Acknowledge Stage 2 SPF Confirmation
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeStage2SPFConfirmation(string activityEngineGuid, string applicationId)
        {
            //  This request allows a Claimant Representative to simply indicate that they received the 
            //  message with the decision on the Stage 2 Settlement Pack Form taken by the Compensator. 
            //  Nothing else: it is simply a step needed in the workflow.
            using (PIPService.ELPLService service = GetELPLService())
            {
                acknowledgeStage2SPFConfirmation confirmation = new acknowledgeStage2SPFConfirmation();
                confirmation.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                confirmation.claimData = data;

                acknowledgeStage2SPFConfirmationResponse response = service.acknowledgeStage2SPFConfirmation(confirmation);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Stage 2 SPF Confirmation " + errorMessage);
                }
            }
        }
        #endregion AcknowledgeStage2SPFConfirmation


        #region AddStage2SPFCounterOfferByCM
        /// <summary>
        /// Add Stage 2 SPF Counter Offer By CM V3
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="S2SPFCounterOfferByCMXML"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFCounterOfferByCM(string activityEngineGuid, string applicationId, Stage2SettlementPackCounterOfferByCM counterOffer)
        {
            //  This request allows a Compensator to send a Stage 2 Settlement Pack Form Counter Offer for a claim.
            using (PIPService.ELPLService service = GetELPLService())
            {
                addStage2SPFCounterOfferByCM offer = new addStage2SPFCounterOfferByCM();
                offer.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                offer.claimData = data;

                string xml = GenerateStage2SettlementPackCounterOfferByCM(counterOffer);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                 applicationId,
                                                                 ValidationServices.ProcessActions.SettlementPackCounterOfferResponseCM);

                ValidationServices.ValidateXML(xml, xsd);
                offer.S2SPFCounterOfferByCMXML = xml;

                addStage2SPFCounterOfferByCMResponse response = service.addStage2SPFCounterOfferByCM(offer);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add Stage 2 SPF Counter Offer By CM " + errorMessage);
                }
            }
        }
        #endregion AddStage2SPFCounterOfferByCM


        #region AddStage2SPFCounterOfferByCR
        /// <summary>
        /// Add Stage 2 SPF Counter Offer By CR
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFCounterOfferByCR(string activityEngineGuid, string applicationId, Stage2SettlementPackCounterOfferByCR counterOffer)
        {
            //  This request allows a Claimant Representative to send a Stage 2 Settlement Pack Form Counter Offer for a claim.
            //  The reason why there are 2 similar functionalities to add a counter offer is because the set of fields inserted 
            //  as a counter offer by the CRs is different from the one inserted by the Compensators.
            using (PIPService.ELPLService service = GetELPLService())
            {
                addStage2SPFCounterOfferByCR offer = new addStage2SPFCounterOfferByCR();
                offer.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                offer.claimData = data;

                string xml = GenerateStage2SettlementPackCounterOfferByCR(counterOffer);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                 applicationId,
                                                                 ValidationServices.ProcessActions.SettlementPackCounterOfferRequestCR);

                ValidationServices.ValidateXML(xml, xsd);
                offer.S2SPFCounterOfferByCRXML = xml;

                addStage2SPFCounterOfferByCRResponse response = service.addStage2SPFCounterOfferByCR(offer);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add Stage 2 SPF Counter Offer By CR " + errorMessage);
                }
            }
        }
        #endregion AddStage2SPFCounterOfferByCR


        #region SetStage2SPFCounterOfferNeeded
        /// <summary>
        /// Set Stage 2 SPF Counter Offer Needed
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="isStage2SPFCounterOfferNeeded"></param>
        /// <returns></returns>
        public claimInfo SetStage2SPFCounterOfferNeeded(string activityEngineGuid, string applicationId, bool isStage2SPFCounterOfferNeeded)
        {
            //  This request allows a Claimant Representative to indicate that they don’t need to send to the Compensator 
            //  a new counter offer for a given claim. As a consequence the claim is moved ahead in the workflow.
            using (PIPService.ELPLService service = GetELPLService())
            {
                setStage2SPFCounterOfferNeeded offer = new setStage2SPFCounterOfferNeeded();
                offer.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                offer.claimData = data;
                offer.isStage2SPFCounterOfferNeeded = isStage2SPFCounterOfferNeeded;

                setStage2SPFCounterOfferNeededResponse response = service.setStage2SPFCounterOfferNeeded(offer);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Set Stage 2 SPF Counter Offer Needed " + errorMessage);
                }
            }
        }
        #endregion SetStage2SPFCounterOfferNeeded


        #region ExtendStage2SPFDecisionTimeout
        /// <summary>
        /// Extend Stage 2 SPF Decision Timeout
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="newTimeOut"></param>
        /// <returns></returns>
        public claimInfo ExtendStage2SPFDecisionTimeout(string activityEngineGuid, string applicationId, DateTime newTimeOut)
        {
            //  This request allows a Compensator to extend the timeframe needed to take a decision for the Staqe2SP Request.
            using (PIPService.ELPLService service = GetELPLService())
            {
                extendStage2SPFDecisionTimeout decision = new extendStage2SPFDecisionTimeout();
                decision.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                decision.claimData = data;

                if (newTimeOut != null)
                {
                    decision.newTimeOut = newTimeOut;
                    decision.newTimeOutSpecified = true;
                }

                extendStage2SPFDecisionTimeoutResponse response = service.extendStage2SPFDecisionTimeout(decision);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Extend Stage 2 SPF Decision Timeout " + errorMessage);
                }
            }
        }
        #endregion ExtendStage2SPFDecisionTimeout


        #region ExtendStage2SPFCounterOfferTimeout
        /// <summary>
        /// Extend Stage 2 SPF Counter Offer Timeout
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="newTimeOut"></param>
        /// <returns></returns>
        public claimInfo ExtendStage2SPFCounterOfferTimeout(string activityEngineGuid, string applicationId, DateTime newTimeOut)
        {
            //  This request allows a Compensator or a CR to extend the timeframe needed to make a counter offer for the Staqe2SP.        
            using (PIPService.ELPLService service = GetELPLService())
            {
                extendStage2SPFCounterOfferTimeout offer = new extendStage2SPFCounterOfferTimeout();
                offer.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                offer.claimData = data;

                if (newTimeOut != null)
                {
                    offer.newTimeOut = newTimeOut;
                    offer.newTimeOutSpecified = true;
                }

                extendStage2SPFCounterOfferTimeoutResponse response = service.extendStage2SPFCounterOfferTimeout(offer);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Extend Stage 2 SPF Counter Offer Timeout " + errorMessage);
                }
            }
        }
        #endregion ExtendStage2SPFCounterOfferTimeout


        #region SetStage2SPFAgreementDecision
        /// <summary>
        /// Set Stage 2 SPF Agreement Decision
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="isAgreed"></param>
        /// <returns></returns>
        public claimInfo SetStage2SPFAgreementDecision(string activityEngineGuid, string applicationId, bool isAgreed)
        {
            //  This request allows a Claimant Representative to indicate whether they agree with the S2SPF counter 
            //  offer or not. As a consequence the claim is moved ahead in the workflow.
            using (PIPService.ELPLService service = GetELPLService())
            {
                setStage2SPFAgreementDecision decision = new setStage2SPFAgreementDecision();
                decision.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                decision.claimData = data;
                decision.isAgreed = isAgreed;

                setStage2SPFAgreementDecisionResponse response = service.setStage2SPFAgreementDecision(decision);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Set Stage 2 SPF Agreement Decision " + errorMessage);
                }
            }
        }
        #endregion SetStage2SPFAgreementDecision


        #region AcknowledgeStage2SPFAgreed
        /// <summary>
        /// Acknowledge Stage 2 SPF Agreed
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeStage2SPFAgreed(string activityEngineGuid, string applicationId)
        {
            //  This request allows a Compensator to simply indicate that they received the message 
            //  that inform them that the Stage 2 Settlement Pack Form was agreed by the Claimant Representative. 
            //  Nothing else: it is simply a step needed in the workflow.
            using (PIPService.ELPLService service = GetELPLService())
            {
                acknowledgeStage2SPFAgreed agreed = new acknowledgeStage2SPFAgreed();
                agreed.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                agreed.claimData = data;

                acknowledgeStage2SPFAgreedResponse response = service.acknowledgeStage2SPFAgreed(agreed);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Stage 2 SPF Agreed " + errorMessage);
                }
            }
        }
        #endregion AcknowledgeStage2SPFAgreed


        #region AddCPPFRequest
        /// <summary>
        /// Add CPPF Request V3
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="CPPFRequestXML"></param>
        /// <returns></returns>
        public claimInfo AddCPPFRequest(string activityEngineGuid, string applicationId, CourtProceedingPackRequest packRequest)
        {
            //  This request allows to add a Court Proceedings Pack Form Request for a claim.
            using (PIPService.ELPLService service = GetELPLService())
            {
                addCPPFRequest request = new addCPPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                string xml = GenerateCourtProceedingPackRequest(packRequest);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                 "",
                                                                 ValidationServices.ProcessActions.AddClaim);

                ValidationServices.ValidateXML(xml, xsd);
                request.CPPFRequestXML = xml;

                addCPPFRequestResponse response = service.addCPPFRequest(request);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add CPPF Request " + errorMessage);
                }
            }
        }
        #endregion AddCPPFRequest


        #region AddCPPFResponse
        /// <summary>
        /// Add CPPF Response
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="CPPFResponseXML"></param>
        /// <returns></returns>
        public claimInfo AddCPPFResponse(string activityEngineGuid, string applicationId, string CPPFResponseXML)
        {
            //  This request allows to add a Court Proceedings Pack Form Response for a claim.
            using (PIPService.ELPLService service = GetELPLService())
            {
                addCPPFResponse addResponse = new addCPPFResponse();
                addResponse.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                addResponse.claimData = data;
                addResponse.CPPFResponseXML = CPPFResponseXML;

                addCPPFResponseResponse response = service.addCPPFResponse(addResponse);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add CPPF Response " + errorMessage);
                }
            }
        }
        #endregion AddCPPFResponse


        #region AcknowledgeCPPFResponse
        /// <summary>
        /// Acknowledge CPPF Response
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeCPPFResponse(string activityEngineGuid, string applicationId)
        {
            //  This request allows a Claimant Representative to simply indicate that they received 
            //  the response to the CPPF request. Nothing else: it is simply a step needed in the workflow.
            using (PIPService.ELPLService service = GetELPLService())
            {
                acknowledgeCPPFResponse acknowledge = new acknowledgeCPPFResponse();
                acknowledge.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                acknowledge.claimData = data;

                acknowledgeCPPFResponseResponse response = service.acknowledgeCPPFResponse(acknowledge);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge CPPF Response " + errorMessage);
                }
            }
        }
        #endregion AcknowledgeCPPFResponse


        #region ExitProcess
        /// <summary>
        /// Exit Process
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <param name="exitComment"></param>
        /// <param name="exitReasonCode"></param>
        /// <returns></returns>
        public claimInfo ExitProcess(string applicationId, string activityEngineGuid, string exitComment, string exitReasonCode)
        {
            using (PIPService.ELPLService service = GetELPLService())
            {
                exitProcess exit = new exitProcess();
                exit.accessToken = GetAccessToken();
                exit.exitComment = exitComment;
                exit.exitReasonCode = exitReasonCode;

                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                exit.claimData = data;

                exitProcessResponse response = service.exitProcess(exit);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Exit Process Response " + errorMessage);
                }
            }
        }
        #endregion ExitProcess


        #region AcknowledgeExitProcess
        /// <summary>
        /// Acknowledge Exit Process
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeExitProcess(string applicationId, string activityEngineGuid)
        {
            using (PIPService.ELPLService service = GetELPLService())
            {
                acknowledgeExitProcess exitProcess = new acknowledgeExitProcess();
                exitProcess.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                exitProcess.claimData = data;

                acknowledgeExitProcessResponse response = service.acknowledgeExitProcess(exitProcess);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Exit Process Response " + errorMessage);
                }
            }
        }
        #endregion AcknowledgeExitProcess


        #region AcknowledgeStage2SPFNotAgreed
        /// <summary>
        /// Acknowledge Stage 2 SPF Not Agreed
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeStage2SPFNotAgreed(string applicationId, string activityEngineGuid)
        {
            //  This request allows the Claimant Representative to simply indicate that they received the message that inform them that the 
            //  Stage 2 Settlement Pack Form was NOT agreed by the Compensator. Nothing else: it is simply a step needed in the workflow.

            using (PIPService.ELPLService service = GetELPLService())
            {
                acknowledgeStage2SPFNotAgreed notAgreed = new acknowledgeStage2SPFNotAgreed();
                notAgreed.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                notAgreed.claimData = data;

                acknowledgeStage2SPFNotAgreedResponse response = service.acknowledgeStage2SPFNotAgreed(notAgreed);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Stage 2 SPF Not Agreed " + errorMessage);
                }
            }
        }
        #endregion AcknowledgeStage2SPFNotAgreed


        #region AcknowledgeStage2SPFDecisionTimeout
        /// <summary>
        /// Acknowledge Stage 2 SPF Decision Timeout
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeStage2SPFDecisionTimeout(string applicationId, string activityEngineGuid)
        {
            //  This command is needed to ensure the CR acknowledge that the claim reached the time limit to take a Stage2 Settlement Pack decision.
            //  As a consequence the claim moves into the phase Stage 2 Settlement Pack Decision Timeout End and enters in the Process Status “END”, 
            //  which removes the claim from the worklist.

            using (PIPService.ELPLService service = GetELPLService())
            {
                acknowledgeStage2SPFDecisionTimeout timeout = new acknowledgeStage2SPFDecisionTimeout();
                timeout.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                timeout.claimData = data;

                acknowledgeStage2SPFDecisionTimeoutResponse response = service.acknowledgeStage2SPFDecisionTimeout(timeout);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Stage 2 SPF Decision Timeout " + errorMessage);
                }
            }
        }
        #endregion AcknowledgeStage2SPFDecisionTimeout

        #endregion Stage 2.2 Public Methods
    }
}
