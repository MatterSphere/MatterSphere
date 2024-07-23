using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using RTAServicesLibrary.PIPService;
using RTAServicesLibraryV2;

namespace RTAServicesLibrary
{
    public class RTAServices2 : RTAServiceBase
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loginDetails">Login details</param>
        /// <param name="tokenStorageProvider">Token storage provider</param>
        public RTAServices2(RTALoginDetails loginDetails, ITokenStorageProvider tokenStorageProvider) :
            base(loginDetails, tokenStorageProvider)
        {
            if (tokenStorageProvider == null)
                throw new ArgumentNullException(nameof(tokenStorageProvider));
        }

        #endregion Constructors


        #region GenerateInterimSettlementPackRequest()

        /// <summary>
        /// Generate Interim Settlement Pack Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GenerateInterimSettlementPackRequest(RTAServicesLibraryV2.InterimSettlementPackRequest request)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV2.InterimSettlementPackRequest));
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


        /// <summary>
        /// Generate Interim Settlement Pack Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GenerateInterimSettlementPackRequest(RTAServicesLibraryV3.InterimSettlementPackRequest request)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV3.InterimSettlementPackRequest));
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


        /// <summary>
        /// Generate Interim Settlement Pack Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GenerateInterimSettlementPackRequest<T>(T interimSettlementPackRequest)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, interimSettlementPackRequest);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate Interim Settlement Pack Request Error: " + ex.Message);
            }
        }

        #endregion


        #region InterimSettlementPackRequest()

        /// <summary>
        /// Get Interim Settlement Pack Request
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV2.InterimSettlementPackRequest GetInterimSettlementPackRequest(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV2.InterimSettlementPackRequest));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV2.InterimSettlementPackRequest)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack Request XML Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Get Interim Settlement Pack Request
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.InterimSettlementPackRequest GetInterimSettlementPackRequestV3(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV3.InterimSettlementPackRequest));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV3.InterimSettlementPackRequest)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack Request XML Error: " + ex.Message);
            }
        }



        /// <summary>
        /// Get Interim Settlement Pack Request.
        /// Send through an empty InterimSettlementPackRequest object to accompany the xml.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public TInterimSettlementPackRequest GetInterimSettlementPackRequest<TInterimSettlementPackRequest>(string xmlString, TInterimSettlementPackRequest interimSettlementPackRequest)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(interimSettlementPackRequest.GetType());
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (TInterimSettlementPackRequest)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack Request XML Error: " + ex.Message);
            }
        }


        #endregion


        #region GenerateInterimSettlementPackResponse()
        /// <summary>
        /// Generate Interim Settlement Pack Response
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GenerateInterimSettlementPackResponse(RTAServicesLibraryV2.InterimSettlementPackResponse response)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV2.InterimSettlementPackResponse));
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


        /// <summary>
        /// Generate Interim Settlement Pack Response
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GenerateInterimSettlementPackResponse(RTAServicesLibraryV3.InterimSettlementPackResponse response)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV3.InterimSettlementPackResponse));
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


        /// <summary>
        /// Generate Interim Settlement Pack Response
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public string GenerateInterimSettlementPackResponse<TInterimSettlementPackResponse>(TInterimSettlementPackResponse response)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(response.GetType());
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

        #endregion


        #region GetInterimSettlementPackResponse()
        /// <summary>
        /// Get Interim Settlement Pack Response
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV2.InterimSettlementPackResponse GetInterimSettlementPackResponse(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV2.InterimSettlementPackResponse));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV2.InterimSettlementPackResponse)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack Response XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get Interim Settlement Pack Response
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.InterimSettlementPackResponse GetInterimSettlementPackResponseV3(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV2.InterimSettlementPackResponse));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV3.InterimSettlementPackResponse)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack Response XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get Interim Settlement Pack Response.
        /// Send through an empty InterimSettlementPackResponse object to accompany the xml.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public TInterimSettlementPackResponse GetInterimSettlementPackResponse<TInterimSettlementPackResponse>(string xmlString, TInterimSettlementPackResponse response)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(response.GetType());
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (TInterimSettlementPackResponse)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack Response XML Error: " + ex.Message);
            }
        }


        #endregion

        
        #region GetInterimSettlementPackFromXML()
        /// <summary>
        /// Get InterimSettlement Pack From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV2.Data GetInterimSettlementPackFromXML(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV2.Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get InterimSettlement Pack From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.Data GetInterimSettlementPackFromXMLV3(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV3.Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get InterimSettlement Pack From XML.
        /// Provide a new Data object along with the xml.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public TData GetInterimSettlementPackFromXML<TData>(string xmlString, TData data)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(data.GetType());
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (TData)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Interim Settlement Pack XML Error: " + ex.Message);
            }
        }

        #endregion


        #region GetDataFromXML()
        /// <summary>
        /// Get Data From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV2.Data GetDataFromXML(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV2.Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV2.Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Data XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get Data From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.Data GetDataFromXMLV3(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV3.Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV3.Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Data XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get Data From XML.
        /// Supply a new Data object along with the xml.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public TData GetDataFromXML<TData>(string xmlString, TData data)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(data.GetType());
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (TData)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Data XML Error: " + ex.Message);
            }
        }

        #endregion


        #region GetStage2SettlementPackFromXML()
        /// <summary>
        /// Get Stage 2 Settlement Pack From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV2.Data GetStage2SettlementPackFromXML(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV2.Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV2.Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Stage 2 Settlement Pack XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get Stage 2 Settlement Pack From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.Data GetStage2SettlementPackFromXMLV3(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV3.Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV3.Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Stage 2 Settlement Pack XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get Stage 2 Settlement Pack From XML.
        /// Send in a new Data object along with the XML.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public TData GetStage2SettlementPackFromXML<TData>(string xmlString, TData data)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(data.GetType());
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (TData)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Stage 2 Settlement Pack XML Error: " + ex.Message);
            }
        }


        #endregion


        #region GetCourtProceedingsPackFromXML()
        /// <summary>
        /// Get Court Proceedings Pack From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV2.Data GetCourtProceedingsPackFromXML(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV2.Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV2.Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Court Proceedings Pack XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get Court Proceedings Pack From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.Data GetCourtProceedingsPackFromXMLV3(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV3.Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV3.Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Court Proceedings Pack XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get Court Proceedings Pack From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public TData GetCourtProceedingsPackFromXML<TData>(string xmlString, TData data)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(data.GetType());
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (TData)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Court Proceedings Pack XML Error: " + ex.Message);
            }
        }

        #endregion


        #region GetTimeOutsFromXML()
        /// <summary>
        /// Get Timeouts From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV2.Data GetTimeOutsFromXML(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV2.Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV2.Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Timeouts XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get Timeouts From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.Data GetTimeOutsFromXMLV3(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV3.Data));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV3.Data)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Timeouts XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get Timeouts From XML.
        /// Supply a new Data object along with the XML.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public TData GetTimeOutsFromXML<TData>(string xmlString, TData data)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(data.GetType());
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (TData)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Timeouts XML Error: " + ex.Message);
            }
        }

        #endregion


        #region GenerateStage2SettlementPackRequest()
        /// <summary>
        /// Generate Stage 2 Settlement Pack Request
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackRequest(RTAServicesLibraryV2.Stage2SettlementPackRequest packRequest)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV2.Stage2SettlementPackRequest));
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


        /// <summary>
        /// Generate Stage 2 Settlement Pack Request
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackRequest(RTAServicesLibraryV3.Stage2SettlementPackRequest packRequest)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV3.Stage2SettlementPackRequest));
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


        /// <summary>
        /// Generate Stage 2 Settlement Pack Request
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackRequest<T>(T settlementPackRequest)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, settlementPackRequest);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate Stage 2 Settlement Pack Request Error: " + ex.Message);
            }
        }
        #endregion


        #region GenerateStage2SettlementPackResponse()
        /// <summary>
        /// Generate Stage 2 Settlement Pack Response
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackResponse(RTAServicesLibraryV2.Stage2SettlementPackResponse packResponse)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV2.Stage2SettlementPackResponse));
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


        /// <summary>
        /// Generate Stage 2 Settlement Pack Response
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackResponse(RTAServicesLibraryV3.Stage2SettlementPackResponse packResponse)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV3.Stage2SettlementPackResponse));
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


        /// <summary>
        /// Generate Stage 2 Settlement Pack Response
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackResponse<TStage2SettlementPackResponse>(TStage2SettlementPackResponse packResponse)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(packResponse.GetType());
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
        #endregion


        #region GenerateStage2SettlementPackCounterOfferByCR()
        /// <summary>
        /// Generate Stage 2 Settlement Pack Counter Offer By CR
        /// </summary>
        /// <param name="counterOffer"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackCounterOfferByCR(RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCR counterOffer)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCR));
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

        /// <summary>
        /// Generate Stage 2 Settlement Pack Counter Offer By CR
        /// </summary>
        /// <param name="counterOffer"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackCounterOfferByCR(RTAServicesLibraryV3.Stage2SettlementPackCounterOfferByCR counterOffer)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV3.Stage2SettlementPackCounterOfferByCR));
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


        /// <summary>
        /// Generate Stage 2 Settlement Pack Counter Offer By CR
        /// </summary>
        /// <param name="counterOffer"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackCounterOfferByCR<TStage2SettlementPackCounterOfferByCR>(TStage2SettlementPackCounterOfferByCR counterOffer)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(counterOffer.GetType());
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

        #endregion


        #region GenerateStage2SettlementPackCounterOfferByCM()
        /// <summary>
        /// Generate Stage 2 Settlement Pack Counter Offer By CM
        /// </summary>
        /// <param name="counterOffer"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackCounterOfferByCM(RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCM counterOffer)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCM));
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

        /// <summary>
        /// Generate Stage 2 Settlement Pack Counter Offer By CM
        /// </summary>
        /// <param name="counterOffer"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackCounterOfferByCM(RTAServicesLibraryV3.Stage2SettlementPackCounterOfferByCM counterOffer)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV3.Stage2SettlementPackCounterOfferByCM));
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


        /// <summary>
        /// Generate Stage 2 Settlement Pack Counter Offer By CM
        /// </summary>
        /// <param name="counterOffer"></param>
        /// <returns></returns>
        public string GenerateStage2SettlementPackCounterOfferByCM<TStage2SettlementPackCounterOfferByCM>(TStage2SettlementPackCounterOfferByCM counterOffer)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(counterOffer.GetType());
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

        #endregion


        #region GenerateCourtProceedingPackRequest()
        /// <summary>
        /// Generate Court Proceeding Pack Request
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateCourtProceedingPackRequest(RTAServicesLibraryV2.CourtProceedingPackRequest packRequest)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV2.CourtProceedingPackRequest));
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

        /// <summary>
        /// Generate Court Proceeding Pack Request
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateCourtProceedingPackRequest(RTAServicesLibraryV3.CourtProceedingPackRequest packRequest)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV3.CourtProceedingPackRequest));
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


        /// <summary>
        /// Generate Court Proceeding Pack Request
        /// </summary>
        /// <param name="packRequest"></param>
        /// <returns></returns>
        public string GenerateCourtProceedingPackRequest<TCourtProceedingPackRequest>(TCourtProceedingPackRequest packRequest)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(packRequest.GetType());
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

        #endregion


        

        #region Public Methods

        #region GetClaimInterimSettlementPack
        /// <summary>
        /// Get Claim Interim Settlement Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV2.InterimSettlementPack GetClaimInterimSettlementPack(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    RTAServicesLibraryV2.Data data = GetInterimSettlementPackFromXML(response.stringResponse.value);

                    return data.InterimSettlementPack;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Get Claim Interim Settlement Pack " + errorMessage);
                }
            }
        }


        /// <summary>
        /// Get Claim Interim Settlement Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.InterimSettlementPack[] GetClaimInterimSettlementPackListV3(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    RTAServicesLibraryV3.Data data = GetInterimSettlementPackFromXMLV3(response.stringResponse.value);

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


        /// <summary>
        /// Get Claim Interim Settlement Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV5.InterimSettlementPack[] GetClaimInterimSettlementPackListV5(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    RTAServicesLibraryV5.Data data = GetInterimSettlementPackFromXML<RTAServicesLibraryV5.Data>(response.stringResponse.value, new RTAServicesLibraryV5.Data());

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

        /// <summary>
        /// Get Claim Interim Settlement Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV7.InterimSettlementPack[] GetClaimInterimSettlementPackListV7(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    RTAServicesLibraryV7.Data data = GetInterimSettlementPackFromXML<RTAServicesLibraryV7.Data>(response.stringResponse.value, new RTAServicesLibraryV7.Data());

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

        #endregion GetClaimInterimSettlementPack


        #region GetClaimStage2SettlementPack
        /// <summary>
        /// Get Claim Stage 2 Settlement Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV2.Stage2SettlementPack GetClaimStage2SettlementPack(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    RTAServicesLibraryV2.Data data = GetStage2SettlementPackFromXML(response.stringResponse.value);

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


        /// <summary>
        /// Get Claim Stage 2 Settlement Pack V3
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.Stage2SettlementPack GetClaimStage2SettlementPackV3(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    RTAServicesLibraryV3.Data data = GetStage2SettlementPackFromXMLV3(response.stringResponse.value);

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


        /// <summary>
        /// Get Claim Stage 2 Settlement Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV5.Stage2SettlementPack GetClaimStage2SettlementPackV5(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    var data = GetStage2SettlementPackFromXML<RTAServicesLibraryV5.Data>(response.stringResponse.value, new RTAServicesLibraryV5.Data());
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

        /// <summary>
        /// Get Claim Stage 2 Settlement Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV7.Stage2SettlementPack GetClaimStage2SettlementPackV7(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    var data = GetStage2SettlementPackFromXML<RTAServicesLibraryV7.Data>(response.stringResponse.value, new RTAServicesLibraryV7.Data());
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
            using (PIPService.PIPService service = GetRTAService())
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


        /// <summary>
        /// Get Claim Court Proceedings Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.CourtProceedingsPack GetClaimCourtProceedingsPackV3(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    var data = GetCourtProceedingsPackFromXML<RTAServicesLibraryV3.Data>(response.stringResponse.value, new RTAServicesLibraryV3.Data());

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


        /// <summary>
        /// Get Claim Court Proceedings Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV5.CourtProceedingsPack GetClaimCourtProceedingsPackV5(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    var data = GetCourtProceedingsPackFromXML<RTAServicesLibraryV5.Data>(response.stringResponse.value, new RTAServicesLibraryV5.Data());

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

        /// <summary>
        /// Get Claim Court Proceedings Pack
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV7.CourtProceedingsPack GetClaimCourtProceedingsPackV7(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    var data = GetCourtProceedingsPackFromXML<RTAServicesLibraryV7.Data>(response.stringResponse.value, new RTAServicesLibraryV7.Data());

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


        /// <summary>
        /// Get Claim TimeOuts
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public Timeouts GetClaimTimeOuts(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
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


        #region GetClaimData
        /// <summary>
        /// Get Claim Data
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV2.Data GetClaimData(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
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


        /// <summary>
        /// Get Claim Data for V3 claims
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.Data GetClaimDataV3(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    return GetDataFromXMLV3(response.stringResponse.value);
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Get Claim Data " + errorMessage);
                }
            }
        }


        /// <summary>
        /// Get Claim Data for V5 claims
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public TData GetClaimDataV5<TData>(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    return (TData)(object)GetDataFromXML<RTAServicesLibraryV5.Data>(response.stringResponse.value, new RTAServicesLibraryV5.Data());
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Get Claim Data " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Get Claim Data for V7 claims
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public TData GetClaimDataV7<TData>(string applicationId)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    return (TData)(object)GetDataFromXML<RTAServicesLibraryV7.Data>(response.stringResponse.value, new RTAServicesLibraryV7.Data());
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Get Claim Data " + errorMessage);
                }
            }
        }


        public delegate TData GetClaimDel<TData>(string claimData);

        /// <summary>
        /// Test method to get the claim data without the above double-ups of near identical methods.
        /// Needs testing.
        /// </summary>
        /// <param name="applicationId"></param>
        public void GetClaimDataTest(string applicationId)
        {
            var rta1 = new RTAServices1(LoginDetails, TokenStorageProvider);
            var version = rta1.GetSystemProcessVersion(applicationId, rta1);
            var versionCode = rta1.GetSystemProcessVersionReleaseCode(version);

            switch (versionCode)
            {
                case "R2":
                    var getR2ClaimXML = new GetClaimDel<RTAServicesLibraryV2.Data>((s) => GetDataFromXML<RTAServicesLibraryV2.Data>(s, new RTAServicesLibraryV2.Data()));
                    var R2ClaimData = GetClaimDataGeneric<RTAServicesLibraryV2.Data>(applicationId, getR2ClaimXML);
                    System.Diagnostics.Debugger.Launch();
                    break;

                case "R3":
                    var getR3ClaimXML = new GetClaimDel<RTAServicesLibraryV3.Data>((s) => GetDataFromXML<RTAServicesLibraryV3.Data>(s, new RTAServicesLibraryV3.Data()));
                    var R3ClaimData = GetClaimDataGeneric<RTAServicesLibraryV3.Data>(applicationId, getR3ClaimXML);
                    System.Diagnostics.Debugger.Launch();
                    break;

                case "R5":
                case "R6":
                    var getR5ClaimXML = new GetClaimDel<RTAServicesLibraryV5.Data>((s) => GetDataFromXML<RTAServicesLibraryV5.Data>(s, new RTAServicesLibraryV5.Data()));
                    var R5ClaimData = GetClaimDataGeneric<RTAServicesLibraryV5.Data>(applicationId, getR5ClaimXML);
                    System.Diagnostics.Debugger.Launch();
                    break;
                default: // "R7"
                    var getR7ClaimXML = new GetClaimDel<RTAServicesLibraryV7.Data>((s) => GetDataFromXML<RTAServicesLibraryV7.Data>(s, new RTAServicesLibraryV7.Data()));
                    var R7ClaimData = GetClaimDataGeneric<RTAServicesLibraryV7.Data>(applicationId, getR7ClaimXML);
                    System.Diagnostics.Debugger.Launch();
                    break;
            }
            
        }


        /// <summary>
        /// Get Claim Data for V3 claims
        /// </summary>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public TData GetClaimDataGeneric<TData>(string applicationId, GetClaimDel<TData> del)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    return del(response.stringResponse.value);
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

        /// <summary>
        /// Set Interim Payment Needed
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public claimInfo SetInterimPaymentNeeded(string activityEngineGuid, string applicationId, bool isInterimPaymentNeeded)
        {
            //  This request allows the CR to add an Interim Settlement Pack Form Request for a claim.
            using (PIPService.PIPService service = GetRTAService())
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



        #region AddInterimSPFRequest
        /// <summary>
        /// Add Interim SPF Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public claimInfo AddInterimSPFRequest(string activityEngineGuid, string applicationId, RTAServicesLibraryV2.InterimSettlementPackRequest packRequest)
        {
            //  This request allows the CR to add an Interim Settlement Pack Form Request for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addInterimSPFRequest request = new addInterimSPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                //  Generate the InterimSettlementPackRequest into xml
                string xml = GenerateInterimSettlementPackRequest(packRequest);
                ValidationServices.ValidateXML(xml, "AddInterimSPFRequest_InterimSettlementPackRequest-v2.0.xsd");
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

        /// <summary>
        /// Add Interim SPF Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public claimInfo AddInterimSPFRequest(string activityEngineGuid, string applicationId, RTAServicesLibraryV3.InterimSettlementPackRequest packRequest)
        {
            //  This request allows the CR to add an Interim Settlement Pack Form Request for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addInterimSPFRequest request = new addInterimSPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                //  Generate the InterimSettlementPackRequest into xml
                string xml = GenerateInterimSettlementPackRequest(packRequest);
                ValidationServices.ValidateXML(xml, "AddInterimSPFRequest_InterimSettlementPackRequest-v3.0.xsd");
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


        /// <summary>
        /// Add Interim SPF Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public claimInfo AddInterimSPFRequest<T>(string activityEngineGuid, string applicationId, T interimSettlementPackRequest)
        {
            //  This request allows the CR to add an Interim Settlement Pack Form Request for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addInterimSPFRequest request = new addInterimSPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                //  Generate the InterimSettlementPackRequest into xml
                string xml = GenerateInterimSettlementPackRequest(interimSettlementPackRequest);
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
        public claimInfo AddInterimSPFResponse(string activityEngineGuid, string applicationId, RTAServicesLibraryV2.InterimSettlementPackResponse packRequest)
        {
            //  This request allows the COMP to add an Interim Settlement Pack Form Response for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addInterimSPFResponse spfResponse = new addInterimSPFResponse();
                spfResponse.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                spfResponse.claimData = data;

                string xml = GenerateInterimSettlementPackResponse(packRequest);
                ValidationServices.ValidateXML(xml, "AddInterimSPFResponse_InterimSettlementPackResponse-v2.0.xsd");
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


        /// <summary>
        /// Add Interim SPF Response
        /// </summary>
        /// <param name="spfResponse"></param>
        /// <returns></returns>
        public claimInfo AddInterimSPFResponse(string activityEngineGuid, string applicationId, RTAServicesLibraryV3.InterimSettlementPackResponse packRequest)
        {
            //  This request allows the COMP to add an Interim Settlement Pack Form Response for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addInterimSPFResponse spfResponse = new addInterimSPFResponse();
                spfResponse.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                spfResponse.claimData = data;

                string xml = GenerateInterimSettlementPackResponse(packRequest);
                ValidationServices.ValidateXML(xml, "AddInterimSPFResponse_InterimSettlementPackResponse-v3.0.xsd");
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


        /// <summary>
        /// Add Interim SPF Response
        /// </summary>
        /// <param name="spfResponse"></param>
        /// <returns></returns>
        public claimInfo AddInterimSPFResponse(string activityEngineGuid, string applicationId, RTAServicesLibraryV5.InterimSettlementPackResponse packRequest)
        {
            //  This request allows the COMP to add an Interim Settlement Pack Form Response for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addInterimSPFResponse spfResponse = new addInterimSPFResponse();
                spfResponse.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                spfResponse.claimData = data;

                string xml = GenerateInterimSettlementPackResponse(packRequest);
                ValidationServices.ValidateXML(xml, "AddInterimSPFResponse_InterimSettlementPackResponse-v5.0.xsd");
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

        /// <summary>
        /// Add Interim SPF Response
        /// </summary>
        /// <param name="spfResponse"></param>
        /// <returns></returns>
        public claimInfo AddInterimSPFResponse(string activityEngineGuid, string applicationId, RTAServicesLibraryV7.InterimSettlementPackResponse packRequest)
        {
            //  This request allows the COMP to add an Interim Settlement Pack Form Response for a claim.
            using (PIPService.PIPService service = GetRTAService())
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

        /// <summary>
        /// Set Stage 21 Payments
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public claimInfo SetStage21Payments(string activityEngineGuid, string applicationId, bool isStage21Paid)
        {
            //  This request allows a CR to simply indicate that the Payment for the given 
            //  Interim Settlement Pack Form has been received or not.
            using (PIPService.PIPService service = GetRTAService())
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

        /// <summary>
        /// Accept Partial Interim Payment
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public claimInfo AcceptPartialInterimPayment(string activityEngineGuid, string applicationId, bool isPartialInterimPaymentAccepted)
        {
            //  This request allows a CR to accept or not to accept the offer for a partial payment made by the Compensator.
            using (PIPService.PIPService service = GetRTAService())
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
            using (PIPService.PIPService service = GetRTAService())
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

            using (PIPService.PIPService service = GetRTAService())
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

            using (PIPService.PIPService service = GetRTAService())
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

        /// <summary>
        /// Reject Interim Settlement Pack
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo RejectInterimSettlementPack(string activityEngineGuid, string applicationId, string rejectionComment)
        {
            //  This request allows the Compensator to reject the request of an Interim payment made by the Claimant Representative.

            using (PIPService.PIPService service = GetRTAService())
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

            using (PIPService.PIPService service = GetRTAService())
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
            using (PIPService.PIPService service = GetRTAService())
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
        public claimInfo AddStage2SPFRequest(string activityEngineGuid, string applicationId, RTAServicesLibraryV2.Stage2SettlementPackRequest packRequest)
        {
            //  This request allows to add a Stage 2 Settlement Pack Form Request for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addStage2SPFRequest request = new addStage2SPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                string xml = GenerateStage2SettlementPackRequest(packRequest);
                ValidationServices.ValidateXML(xml, "AddStage2SPFRequest_S2SPFRequestXML_v2.0.xsd");
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


        /// <summary>
        /// Add Stage 2 SPF Request
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="S2SPFRequestXML"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFRequest(string activityEngineGuid, string applicationId, RTAServicesLibraryV3.Stage2SettlementPackRequest packRequest)
        {
            //  This request allows to add a Stage 2 Settlement Pack Form Request for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addStage2SPFRequest request = new addStage2SPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                string xml = GenerateStage2SettlementPackRequest(packRequest);
                ValidationServices.ValidateXML(xml, "AddStage2SPFRequest_S2SPFRequestXML-v3.0.xsd");
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


        /// <summary>
        /// Add SPF Request
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFRequest<T>(string activityEngineGuid, string applicationId, T settlementPackRequest)
        {
            //  This request allows the CR to add an Interim Settlement Pack Form Request for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addStage2SPFRequest request = new addStage2SPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                //  Generate the InterimSettlementPackRequest into xml
                string xml = GenerateStage2SettlementPackRequest(settlementPackRequest);
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
        /// Add Stage 2 SPF Response
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="S2SPFResponseXML"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFResponse(string activityEngineGuid, string applicationId, RTAServicesLibraryV2.Stage2SettlementPackResponse packResponse)
        {
            //  This request allows to add a Stage 2 Settlement Pack Form Response for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addStage2SPFResponse res = new addStage2SPFResponse();
                res.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                res.claimData = data;

                string xml = GenerateStage2SettlementPackResponse(packResponse);
                ValidationServices.ValidateXML(xml, "AddStage2SPFResponse_S2SPFResponseXML_v2.0.xsd");
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


        /// <summary>
        /// Add Stage 2 SPF Response V3
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="S2SPFResponseXML"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFResponse(string activityEngineGuid, string applicationId, RTAServicesLibraryV3.Stage2SettlementPackResponse packResponse)
        {
            //  This request allows to add a Stage 2 Settlement Pack Form Response for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addStage2SPFResponse res = new addStage2SPFResponse();
                res.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                res.claimData = data;

                string xml = GenerateStage2SettlementPackResponse(packResponse);
                ValidationServices.ValidateXML(xml, "AddStage2SPFResponse_S2SPFResponseXML-v3.0.xsd");
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



        /// <summary>
        /// Add Stage 2 SPF Response
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFResponse<TSettlementPackResponse>(string activityEngineGuid, string applicationId, TSettlementPackResponse settlementPackResponse)
        {
            //  This request allows the CR to add an Interim Settlement Pack Form Request for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addStage2SPFResponse response = new addStage2SPFResponse();
                response.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                response.claimData = data;

                //  Generate the InterimSettlementPackRequest into xml
                string xml = GenerateStage2SettlementPackResponse(settlementPackResponse);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                 applicationId,
                                                                 ValidationServices.ProcessActions.SettlementPackResponse);

                ValidationServices.ValidateXML(xml, xsd);
                response.S2SPFResponseXML = xml;

                addStage2SPFResponseResponse returnedResponse = service.addStage2SPFResponse(response);

                if (returnedResponse.claimInfoResponse.code == responseCode.Ok)
                {
                    return returnedResponse.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(returnedResponse.claimInfoResponse);
                    throw new Exception("Add Stage 2 SPF Response " + errorMessage);
                }
            }
        }


        #endregion AddStage2SPFResponse



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
            using (PIPService.PIPService service = GetRTAService())
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
            using (PIPService.PIPService service = GetRTAService())
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



        #region AddStage2SPFCounterOfferByCM
        /// <summary>
        /// Add Stage 2 SPF Counter Offer By CM
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="S2SPFCounterOfferByCMXML"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFCounterOfferByCM(string activityEngineGuid, string applicationId, RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCM counterOffer)
        {
            //  This request allows a Compensator to send a Stage 2 Settlement Pack Form Counter Offer for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addStage2SPFCounterOfferByCM offer = new addStage2SPFCounterOfferByCM();
                offer.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                offer.claimData = data;

                string xml = GenerateStage2SettlementPackCounterOfferByCM(counterOffer);
                ValidationServices.ValidateXML(xml, "AddStage2SPFCounterOfferByCM_S2SPFCounterOfferByCMXML-2.0.xsd");
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

        /// <summary>
        /// Add Stage 2 SPF Counter Offer By CM V3
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="S2SPFCounterOfferByCMXML"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFCounterOfferByCM(string activityEngineGuid, string applicationId, RTAServicesLibraryV3.Stage2SettlementPackCounterOfferByCM counterOffer)
        {
            //  This request allows a Compensator to send a Stage 2 Settlement Pack Form Counter Offer for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addStage2SPFCounterOfferByCM offer = new addStage2SPFCounterOfferByCM();
                offer.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                offer.claimData = data;

                string xml = GenerateStage2SettlementPackCounterOfferByCM(counterOffer);
                ValidationServices.ValidateXML(xml, "AddStage2SPFCounterOfferByCM_S2SPFCounterOfferByCMXML-v3.0.xsd");
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

        /// <summary>
        /// Add Stage 2 SPF Counter Offer By CM V7
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="S2SPFCounterOfferByCMXML"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFCounterOfferByCM(string activityEngineGuid, string applicationId, RTAServicesLibraryV7.Stage2SettlementPackCounterOfferByCM counterOffer)
        {
            //  This request allows a Compensator to send a Stage 2 Settlement Pack Form Counter Offer for a claim.
            using (PIPService.PIPService service = GetRTAService())
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
        public claimInfo AddStage2SPFCounterOfferByCR(string activityEngineGuid, string applicationId, RTAServicesLibraryV2.Stage2SettlementPackCounterOfferByCR counterOffer)
        {
            //  This request allows a Claimant Representative to send a Stage 2 Settlement Pack Form Counter Offer for a claim.
            //  The reason why there are 2 similar functionalities to add a counter offer is because the set of fields inserted 
            //  as a counter offer by the CRs is different from the one inserted by the Compensators.
            using (PIPService.PIPService service = GetRTAService())
            {
                addStage2SPFCounterOfferByCR offer = new addStage2SPFCounterOfferByCR();
                offer.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                offer.claimData = data;

                string xml = GenerateStage2SettlementPackCounterOfferByCR(counterOffer);
                ValidationServices.ValidateXML(xml, "AddStage2SPFCounterOfferByCR_S2SPFCounterOfferByCRXML-2.0.xsd");
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


        /// <summary>
        /// Add Stage 2 SPF Counter Offer By CR
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFCounterOfferByCR(string activityEngineGuid, string applicationId, RTAServicesLibraryV3.Stage2SettlementPackCounterOfferByCR counterOffer)
        {
            //  This request allows a Claimant Representative to send a Stage 2 Settlement Pack Form Counter Offer for a claim.
            //  The reason why there are 2 similar functionalities to add a counter offer is because the set of fields inserted 
            //  as a counter offer by the CRs is different from the one inserted by the Compensators.
            using (PIPService.PIPService service = GetRTAService())
            {
                addStage2SPFCounterOfferByCR offer = new addStage2SPFCounterOfferByCR();
                offer.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                offer.claimData = data;

                string xml = GenerateStage2SettlementPackCounterOfferByCR(counterOffer);
                ValidationServices.ValidateXML(xml, "AddStage2SPFCounterOfferByCR_S2SPFCounterOfferByCRXML-v3.0.xsd");
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


        /// <summary>
        /// Add Stage 2 SPF Counter Offer By CR
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        public claimInfo AddStage2SPFCounterOfferByCR<TStage2SettlementPackCounterOfferByCR>(string activityEngineGuid, string applicationId, TStage2SettlementPackCounterOfferByCR counterOffer)
        {
            //  This request allows a Claimant Representative to send a Stage 2 Settlement Pack Form Counter Offer for a claim.
            //  The reason why there are 2 similar functionalities to add a counter offer is because the set of fields inserted 
            //  as a counter offer by the CRs is different from the one inserted by the Compensators.
            using (PIPService.PIPService service = GetRTAService())
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
            using (PIPService.PIPService service = GetRTAService())
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
            using (PIPService.PIPService service = GetRTAService())
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
            using (PIPService.PIPService service = GetRTAService())
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
            using (PIPService.PIPService service = GetRTAService())
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
            using (PIPService.PIPService service = GetRTAService())
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



        #region AddCPPFRequest
        /// <summary>
        /// Add CPPF Request
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="CPPFRequestXML"></param>
        /// <returns></returns>
        public claimInfo AddCPPFRequest(string activityEngineGuid, string applicationId, RTAServicesLibraryV2.CourtProceedingPackRequest packRequest)
        {
            //  This request allows to add a Court Proceedings Pack Form Request for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addCPPFRequest request = new addCPPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                string xml = GenerateCourtProceedingPackRequest(packRequest);
                ValidationServices.ValidateXML(xml, "AddCPPFRequest_CPPFRequestXML_v2.0.xsd");
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


        /// <summary>
        /// Add CPPF Request V3
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="CPPFRequestXML"></param>
        /// <returns></returns>
        public claimInfo AddCPPFRequest(string activityEngineGuid, string applicationId, RTAServicesLibraryV3.CourtProceedingPackRequest packRequest)
        {
            //  This request allows to add a Court Proceedings Pack Form Request for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addCPPFRequest request = new addCPPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                string xml = GenerateCourtProceedingPackRequest(packRequest);
                ValidationServices.ValidateXML(xml, "AddCPPFRequest_CPPFRequestXML-v3.1.xsd");
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


        /// <summary>
        /// Add CPPF Request
        /// </summary>
        /// <param name="activityEngineGuid"></param>
        /// <param name="applicationId"></param>
        /// <param name="CPPFRequestXML"></param>
        /// <returns></returns>
        public claimInfo AddCPPFRequest<TCourtProceedingPackRequest>(string activityEngineGuid, string applicationId, TCourtProceedingPackRequest packRequest)
        {
            //  This request allows to add a Court Proceedings Pack Form Request for a claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                addCPPFRequest request = new addCPPFRequest();
                request.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                request.claimData = data;

                string xml = GenerateCourtProceedingPackRequest(packRequest);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                    applicationId,
                                                                    ValidationServices.ProcessActions.CourtProceedingsPackRequestCR);

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
            using (PIPService.PIPService service = GetRTAService())
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
            using (PIPService.PIPService service = GetRTAService())
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
            using (PIPService.PIPService service = GetRTAService())
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

        /// <summary>
        /// Acknowledge Exit Process
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeExitProcess(string applicationId, string activityEngineGuid)
        {
            using (PIPService.PIPService service = GetRTAService())
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

            using (PIPService.PIPService service = GetRTAService())
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

        /// <summary>
        /// Set Additional Damages Existence
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <param name="additionalDamagesExist"></param>
        /// <returns></returns>
        public claimInfo SetAdditionalDamagesExistence(string applicationId, string activityEngineGuid, bool additionalDamagesExist)
        {
            //  This request allows to start the Additional Damages process for a claim.

            using (PIPService.PIPService service = GetRTAService())
            {
                setAdditionalDamagesExistence damages = new setAdditionalDamagesExistence();
                damages.accessToken = GetAccessToken();
                damages.additionalDamagesExist = additionalDamagesExist;
                damages.additionalDamagesExistSpecified = true;

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                damages.claimData = data;

                setAdditionalDamagesExistenceResponse response = service.setAdditionalDamagesExistence(damages);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Set Additional Damages Existence " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Add S2 SPF Additional Damages Request
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <param name="additionalDamagesRequestXML"></param>
        /// <returns></returns>
        public claimInfo AddS2SPFAdditionalDamagesRequest(string applicationId, string activityEngineGuid, string additionalDamagesRequestXML)
        {
            //  This request allows to add a Stage 2 Settlement Pack Additional Damages Form Request for a claim.

            using (PIPService.PIPService service = GetRTAService())
            {
                addS2SPFAdditionalDamagesRequest request = new addS2SPFAdditionalDamagesRequest();
                request.accessToken = GetAccessToken();
                request.additionalDamagesRequestXML = additionalDamagesRequestXML;

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                request.claimData = data;

                addS2SPFAdditionalDamagesRequestResponse response = service.addS2SPFAdditionalDamagesRequest(request);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add S2 SPF Additional Damages Request " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Add S2 SPF Additional Damages Response
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <param name="additionalDamagesResponseXML"></param>
        /// <returns></returns>
        public claimInfo AddS2SPFAdditionalDamagesResponse(string applicationId, string activityEngineGuid, string additionalDamagesResponseXML)
        {
            //  This request allows to add a Stage 2 Settlement Pack Additional Damages Form Response for a claim.

            using (PIPService.PIPService service = GetRTAService())
            {
                addS2SPFAdditionalDamagesResponse damagesResponse = new addS2SPFAdditionalDamagesResponse();
                damagesResponse.accessToken = GetAccessToken();
                damagesResponse.additionalDamagesResponseXML = additionalDamagesResponseXML;

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                damagesResponse.claimData = data;

                addS2SPFAdditionalDamagesResponseResponse response = service.addS2SPFAdditionalDamagesResponse(damagesResponse);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add S2 SPF Additional Damages Response " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Acknowledge All Damages Agreed
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeAllDamagesAgreed(string applicationId, string activityEngineGuid)
        {
            //  This request allows a Claimant Representative to simply indicate that they received the response to the Additional Damages 
            //  request where the Compensator agrees to all the damages. Nothing else: it is simply a step needed in the workflow.

            using (PIPService.PIPService service = GetRTAService())
            {
                acknowledgeAllDamagesAgreed damagesAgreed = new acknowledgeAllDamagesAgreed();
                damagesAgreed.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                damagesAgreed.claimData = data;

                acknowledgeAllDamagesAgreedResponse response = service.acknowledgeAllDamagesAgreed(damagesAgreed);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge All Damages Agreed " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Set S2 SPF Additional Damages Decision
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <param name="isAgreed"></param>
        /// <returns></returns>
        public claimInfo SetS2SPFAdditionalDamagesDecision(string applicationId, string activityEngineGuid, bool isAgreed)
        {
            //  This request allows the Claimant Representative to indicate whether they agree with the Additional Damages counter offer or not. 
            //  As a consequence the claim is moved ahead in the workflow.

            using (PIPService.PIPService service = GetRTAService())
            {
                setS2SPFAdditionalDamagesDecision decision = new setS2SPFAdditionalDamagesDecision();
                decision.accessToken = GetAccessToken();
                decision.isAgreed = isAgreed;
                decision.isAgreedSpecified = true;

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                decision.claimData = data;

                setS2SPFAdditionalDamagesDecisionResponse response = service.setS2SPFAdditionalDamagesDecision(decision);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Set S2 SPF Additional Damages Decision " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Acknowledge Additional Damages Agreement
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeAdditionalDamagesAgreement(string applicationId, string activityEngineGuid)
        {
            //  This request allows a Compensator to indicate that they received the final agreement decision taken by the Claimant representative 
            //  about the Original and Additional Damages. It also allows a Claimant Representative to acknowledge the result generated in case the 
            //  compensator does not send the response to the S2SP with Additional Damages Request within the time limit.  
            //  Nothing else: it is simply a step needed in the workflow.

            using (PIPService.PIPService service = GetRTAService())
            {
                acknowledgeAdditionalDamagesAgreement agreement = new acknowledgeAdditionalDamagesAgreement();
                agreement.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                agreement.claimData = data;

                acknowledgeAdditionalDamagesAgreementResponse response = service.acknowledgeAdditionalDamagesAgreement(agreement);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Additional Damages Agreement " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Extend Additional Damages Decision Timeout
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <param name="newTimeOutDate"></param>
        /// <param name="reasonForExtension"></param>
        /// <returns></returns>
        public claimInfo ExtendAdditionalDamagesDecisionTimeout(string applicationId, string activityEngineGuid, DateTime newTimeOutDate, string reasonForExtension)
        {
            //  This request allows a Compensator to extend the timeframe needed to take a decision for the Additional Damages.
            //  In case of success, the date of timeout to take a decision is re-set to a new value.

            using (PIPService.PIPService service = GetRTAService())
            {
                extendAdditionalDamagesDecisionTimeout decision = new extendAdditionalDamagesDecisionTimeout();
                decision.accessToken = GetAccessToken();
                decision.NewTimeOutDate = newTimeOutDate;
                decision.NewTimeOutDateSpecified = true;
                decision.ReasonForExtension = reasonForExtension;

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                decision.claimData = data;

                extendAdditionalDamagesDecisionTimeoutResponse response = service.extendAdditionalDamagesDecisionTimeout(decision);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Extend Additional Damages Decision Timeout " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Acknowledge Additional Damages Decision Timeout
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeAdditionalDamagesDecisionTimeout(string applicationId, string activityEngineGuid)
        {
            //  This command is needed to ensure the CR acknowledge that the claim reached the time limit to take a Stage2 Settlement Pack Additional Damages decision.

            using (PIPService.PIPService service = GetRTAService())
            {
                acknowledgeAdditionalDamagesDecisionTimeout decision = new acknowledgeAdditionalDamagesDecisionTimeout();
                decision.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                decision.claimData = data;

                acknowledgeAdditionalDamagesDecisionTimeoutResponse response = service.acknowledgeAdditionalDamagesDecisionTimeout(decision);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Additional Damages Decision Timeout " + errorMessage);
                }
            }
        }

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

            using (PIPService.PIPService service = GetRTAService())
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

        #endregion Stage 2.2 Public Methods
    }
}
