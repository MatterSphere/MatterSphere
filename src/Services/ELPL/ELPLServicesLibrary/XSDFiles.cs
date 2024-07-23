namespace ELPLServicesLibrary
{
    public interface IXSDSchemaFiles
    {
        string InterimSettlementPackRequest { get; }
        string SettlementPackRequest { get; }
        string InterimSettlementPackResponse { get; }
        string SettlementPackResponse { get; }
        string SettlementPackCounterOfferRequestCR { get; }
        string SettlementPackCounterOfferResponseCM { get; }
        string CourtProceedingsRequestCR { get; }
        string CourtProceedingsResponseCM { get; }
        string AddClaim { get; }
    }


    internal class XSDFileFactory
    {
        internal static IXSDSchemaFiles GetXSDObject(string release)
        {
            switch (release)
            {
                case "R0":
                    return new Release3XSDSchemaFiles();

                case "R5":
                    return new Release5XSDSchemaFiles();

                default:
                    return new Release6XSDSchemaFiles();
            }
        }
    }

    internal class Release3XSDSchemaFiles : IXSDSchemaFiles
    {
        public string InterimSettlementPackRequest
        {
            get
            {
                return "AddInterimSPFRequest_InterimSettlementPackRequest-v1.0.xsd";
            }
        }


        public string InterimSettlementPackResponse
        {
            get
            {
                return "AddInterimSPFResponse_InterimSettlementPackResponse-v1.0.xsd";
            }
        }


        public string SettlementPackRequest
        {
            get
            {
                return "AddStage2SPFRequest_S2SPFRequestXML-v1.0.xsd";
            }
        }


        public string SettlementPackResponse
        {
            get
            {
                return "AddStage2SPFResponse_S2SPFResponseXML-v1.0.xsd";
            }
        }


        public string SettlementPackCounterOfferRequestCR
        {
            get
            {
                return "AddStage2SPFCounterOfferByCR_S2SPFCounterOfferByCRXML-v1.0.xsd";
            }
        }


        public string SettlementPackCounterOfferResponseCM
        {
            get
            {
                return "AddStage2SPFCounterOfferByCM_S2SPFCounterOfferByCMXML-v1.0.xsd";
            }
        }


        public string CourtProceedingsRequestCR
        {
            get
            {
                return "AddCPPFRequest_CPPFRequestXML-v1.0.xsd";
            }
        }


        public string CourtProceedingsResponseCM
        {
            get
            {
                return "AddCPPFResponse_CPPFResponseXML-v1.0.xsd";
            }
        }


        public string AddClaim
        {
            get 
            {
                return "AddClaim_ClaimData-v1.1.xsd";
            }
        }
    }


    internal class Release5XSDSchemaFiles : IXSDSchemaFiles
    {
        public string InterimSettlementPackRequest
        {
            get
            {
                return "AddInterimSPFRequest_InterimSettlementPackRequest-v1.0.xsd";
            }
        }


        public string InterimSettlementPackResponse
        {
            get
            {
                return "AddInterimSPFResponse_InterimSettlementPackResponse-v1.1.xsd";
            }
        }


        public string SettlementPackRequest
        {
            get
            {
                return "AddStage2SPFRequest_S2SPFRequestXML-v1.0.xsd";
            }
        }


        public string SettlementPackResponse
        {
            get
            {
                return "AddStage2SPFResponse_S2SPFResponseXML-v1.1.xsd";
            }
        }


        public string SettlementPackCounterOfferRequestCR
        {
            get
            {
                return "AddStage2SPFCounterOfferByCR_S2SPFCounterOfferByCRXML-v1.0.xsd";
            }
        }


        public string SettlementPackCounterOfferResponseCM
        {
            get
            {
                return "AddStage2SPFCounterOfferByCM_S2SPFCounterOfferByCMXML-v1.0.xsd";
            }
        }


        public string CourtProceedingsRequestCR
        {
            get
            {
                return "AddCPPFRequest_CPPFRequestXML-v1.0.xsd";
            }
        }


        public string CourtProceedingsResponseCM
        {
            get
            {
                return "AddCPPFResponse_CPPFResponseXML-v1.0.xsd";
            }
        }


        public string AddClaim
        {
            get 
            {
                return "AddClaim_ClaimData-v1.1.xsd";    
            }
        }
    }

    internal class Release6XSDSchemaFiles : IXSDSchemaFiles
    {
        public string InterimSettlementPackRequest
        {
            get
            {
                return "AddInterimSPFRequest_InterimSettlementPackRequest-v1.0.xsd";
            }
        }


        public string InterimSettlementPackResponse
        {
            get
            {
                return "AddInterimSPFResponse_InterimSettlementPackResponse-v1.1.xsd";
            }
        }


        public string SettlementPackRequest
        {
            get
            {
                return "AddStage2SPFRequest_S2SPFRequestXML-v1.0.xsd";
            }
        }


        public string SettlementPackResponse
        {
            get
            {
                return "AddStage2SPFResponse_S2SPFResponseXML-v1.1.xsd";
            }
        }


        public string SettlementPackCounterOfferRequestCR
        {
            get
            {
                return "AddStage2SPFCounterOfferByCR_S2SPFCounterOfferByCRXML-v1.0.xsd";
            }
        }


        public string SettlementPackCounterOfferResponseCM
        {
            get
            {
                return "AddStage2SPFCounterOfferByCM_S2SPFCounterOfferByCMXML-v1.0.xsd";
            }
        }


        public string CourtProceedingsRequestCR
        {
            get
            {
                return "AddCPPFRequest_CPPFRequestXML-v1.0.xsd";
            }
        }


        public string CourtProceedingsResponseCM
        {
            get
            {
                return "AddCPPFResponse_CPPFResponseXML-v1.0.xsd";
            }
        }


        public string AddClaim
        {
            get
            {
                return "AddClaim_ClaimData-v1.4.xsd";
            }
        }
    }
}
