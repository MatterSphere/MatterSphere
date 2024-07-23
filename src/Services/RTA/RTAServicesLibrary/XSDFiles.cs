namespace RTAServicesLibrary
{
    public interface IXSDSchemaFiles
    {
        string InterimSettlementPackRequest { get; }
        string SettlementPackRequest { get; }
        string InterimSettlementPackResponse { get; }
        string SettlementPackResponse { get; }
        string SettlementPackCounterOfferRequestCR { get; }
        string SettlementPackCounterOfferResponseCM { get; }
        string AdditionalDamagesRequestCR { get; }
        string AdditionalDamagesResponseCM { get; }
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
                case "R2":
                    return new Release2XSDSchemaFiles();

                case "R3":
                    return new Release3XSDSchemaFiles();

                case "R5":
                    return new Release5XSDSchemaFiles();
                case "R6":
                    return new Release6XSDSchemaFiles();
                default:
                    return new Release7XSDSchemaFiles();
            }
        }
    }



    internal class Release2XSDSchemaFiles : IXSDSchemaFiles
    {
        public string InterimSettlementPackRequest
        {
            get
            {
                return "AddInterimSPFRequest_InterimSettlementPackRequest-v2.0.xsd";
            }
        }


        public string InterimSettlementPackResponse
        {
            get
            {
                return "AddInterimSPFResponse_InterimSettlementPackResponse-v2.0.xsd";
            }
        }


        public string SettlementPackRequest
        {
            get
            {
                return "AddStage2SPFRequest_S2SPFRequestXML-v2.0.xsd";
            }
        }


        public string SettlementPackResponse
        {
            get
            {
                return "AddStage2SPFResponse_S2SPFResponseXML-v2.0.xsd";
            }
        }


        public string SettlementPackCounterOfferRequestCR
        {
            get
            {
                return "AddStage2SPFCounterOfferByCR_S2SPFCounterOfferByCRXML-v2.0.xsd";
            }
        }


        public string SettlementPackCounterOfferResponseCM
        {
            get
            {
                return "AddStage2SPFCounterOfferByCM_S2SPFCounterOfferByCMXML-v2.0.xsd";
            }
        }


        public string AdditionalDamagesRequestCR
        {
            get
            {
                return "AddS2SPFAdditionalDamagesRequest_Stage2SettlementPackAdditionalDamagesRequest-v2.0.xsd";
            }
        }


        public string AdditionalDamagesResponseCM
        {
            get
            {
                return "AddS2SPFAdditionalDamagesResponse_Stage2SettlementPackAdditionalDamagesResponse-v2.0.xsd";
            }
        }


        public string CourtProceedingsRequestCR
        {
            get
            {
                return "AddCPPFRequest_CPPFRequestXML-v2.0.xsd";
            }
        }


        public string CourtProceedingsResponseCM
        {
            get
            {
                return "AddCPPFResponse_CPPFResponseXML-v2.0.xsd";
            }
        }


        public string AddClaim
        {
            get 
            {
                return "AddClaim_ClaimData-v2.0.cs";
            }
        }
    }


    internal class Release3XSDSchemaFiles : IXSDSchemaFiles
    {
        public string InterimSettlementPackRequest
        {
            get
            {
                return "AddInterimSPFRequest_InterimSettlementPackRequest-v3.0.xsd";
            }
        }


        public string InterimSettlementPackResponse
        {
            get
            {
                return "AddInterimSPFResponse_InterimSettlementPackResponse-v3.0.xsd";
            }
        }


        public string SettlementPackRequest
        {
            get
            {
                return "AddStage2SPFRequest_S2SPFRequestXML-v3.0.xsd";
            }
        }


        public string SettlementPackResponse
        {
            get
            {
                return "AddStage2SPFResponse_S2SPFResponseXML-v3.0.xsd";
            }
        }


        public string SettlementPackCounterOfferRequestCR
        {
            get
            {
                return "AddStage2SPFCounterOfferByCR_S2SPFCounterOfferByCRXML-v3.0.xsd";
            }
        }


        public string SettlementPackCounterOfferResponseCM
        {
            get
            {
                return "AddStage2SPFCounterOfferByCM_S2SPFCounterOfferByCMXML-v3.0.xsd";
            }
        }


        public string AdditionalDamagesRequestCR
        {
            get
            {
                return "AddS2SPFAdditionalDamagesRequest_Stage2SettlementPackAdditionalDamagesRequest-v3.0.xsd";
            }
        }


        public string AdditionalDamagesResponseCM
        {
            get
            {
                return "AddS2SPFAdditionalDamagesResponse_Stage2SettlementPackAdditionalDamagesResponse-v3.0.xsd";
            }
        }


        public string CourtProceedingsRequestCR
        {
            get
            {
                return "AddCPPFRequest_CPPFRequestXML-v3.1.xsd";
            }
        }


        public string CourtProceedingsResponseCM
        {
            get
            {
                return "AddCPPFResponse_CPPFResponseXML-v3.1.xsd";
            }
        }


        public string AddClaim
        {
            get 
            {
                return "AddClaim_ClaimData-v3.2.cs";
            }
        }
    }


    internal class Release5XSDSchemaFiles : IXSDSchemaFiles
    {
        public string InterimSettlementPackRequest
        {
            get
            {
                return "AddInterimSPFRequest_InterimSettlementPackRequest-v3.3.xsd";
            }
        }


        public string InterimSettlementPackResponse
        {
            get
            {
                return "AddInterimSPFResponse_InterimSettlementPackResponse-v3.2.xsd";
            }
        }


        public string SettlementPackRequest
        {
            get
            {
                return "AddStage2SPFRequest_S2SPFRequestXML-v3.3.xsd";
            }
        }


        public string SettlementPackResponse
        {
            get
            {
                return "AddStage2SPFResponse_S2SPFResponseXML-v3.2.xsd";
            }
        }


        public string SettlementPackCounterOfferRequestCR
        {
            get
            {
                return "AddStage2SPFCounterOfferByCR_S2SPFCounterOfferByCRXML-v3.0.xsd";
            }
        }


        public string SettlementPackCounterOfferResponseCM
        {
            get
            {
                return "AddStage2SPFCounterOfferByCM_S2SPFCounterOfferByCMXML-v3.0.xsd";
            }
        }


        public string AdditionalDamagesRequestCR
        {
            get
            {
                return "AddS2SPFAdditionalDamagesRequest_Stage2SettlementPackAdditionalDamagesRequest-v3.0.xsd";
            }
        }


        public string AdditionalDamagesResponseCM
        {
            get
            {
                return "AddS2SPFAdditionalDamagesResponse_Stage2SettlementPackAdditionalDamagesResponse-v3.0.xsd";
            }
        }


        public string CourtProceedingsRequestCR
        {
            get
            {
                return "AddCPPFRequest_CPPFRequestXML-v3.1.xsd";
            }
        }


        public string CourtProceedingsResponseCM
        {
            get
            {
                return "AddCPPFResponse_CPPFResponseXML-v3.1.xsd";
            }
        }


        public string AddClaim
        {
            get
            {
                return "AddClaim_ClaimData-v3.3.xsd";
            }
        }
    }
    
    internal class Release6XSDSchemaFiles: IXSDSchemaFiles
    {
        public string InterimSettlementPackRequest
        {
            get 
            {
                return "AddInterimSPFRequest_InterimSettlementPackRequest-v3.3.xsd";
            }
        }


        public string InterimSettlementPackResponse
        {
            get
            {
                return "AddInterimSPFResponse_InterimSettlementPackResponse-v3.2.xsd";
            }
        }


        public string SettlementPackRequest
        {
            get
            {
                return "AddStage2SPFRequest_S2SPFRequestXML-v3.3.xsd";
            }
        }


        public string SettlementPackResponse
        {
            get
            {
                return "AddStage2SPFResponse_S2SPFResponseXML-v3.2.xsd";
            }
        }


        public string SettlementPackCounterOfferRequestCR
        {
            get
            {
                return "AddStage2SPFCounterOfferByCR_S2SPFCounterOfferByCRXML-v3.0.xsd";
            }
        }


        public string SettlementPackCounterOfferResponseCM
        {
            get
            {
                return "AddStage2SPFCounterOfferByCM_S2SPFCounterOfferByCMXML-v3.0.xsd";
            }
        }


        public string AdditionalDamagesRequestCR
        {
            get
            {
                return "AddS2SPFAdditionalDamagesRequest_Stage2SettlementPackAdditionalDamagesRequest-v3.0.xsd";
            }
        }


        public string AdditionalDamagesResponseCM
        {
            get
            {
                return "AddS2SPFAdditionalDamagesResponse_Stage2SettlementPackAdditionalDamagesResponse-v3.0.xsd";
            }
        }


        public string CourtProceedingsRequestCR
        {
            get
            {
                return "AddCPPFRequest_CPPFRequestXML-v3.1.xsd";
            }
        }


        public string CourtProceedingsResponseCM
        {
            get
            {
                return "AddCPPFResponse_CPPFResponseXML-v3.1.xsd";
            }
        }


        public string AddClaim
        {
            get
            {
                return "AddClaim_ClaimData-v3.6.xsd";
            }
        }
    }

    internal class Release7XSDSchemaFiles : IXSDSchemaFiles
    {
        public string InterimSettlementPackRequest
        {
            get
            {
                return "AddInterimSPFRequest_InterimSettlementPackRequest-v3.4.xsd";
            }
        }


        public string InterimSettlementPackResponse
        {
            get
            {
                return "AddInterimSPFResponse_InterimSettlementPackResponse-v3.3.xsd";
            }
        }


        public string SettlementPackRequest
        {
            get
            {
                return "AddStage2SPFRequest_S2SPFRequestXML-v3.4.xsd";
            }
        }


        public string SettlementPackResponse
        {
            get
            {
                return "AddStage2SPFResponse_S2SPFResponseXML-v3.3.xsd";
            }
        }


        public string SettlementPackCounterOfferRequestCR
        {
            get
            {
                return "AddStage2SPFCounterOfferByCR_S2SPFCounterOfferByCRXML-v3.1.xsd";
            }
        }


        public string SettlementPackCounterOfferResponseCM
        {
            get
            {
                return "AddStage2SPFCounterOfferByCM_S2SPFCounterOfferByCMXML-v3.1.xsd";
            }
        }


        public string AdditionalDamagesRequestCR
        {
            get
            {
                return "AddS2SPFAdditionalDamagesRequest_Stage2SettlementPackAdditionalDamagesRequest-v3.2.xsd";
            }
        }


        public string AdditionalDamagesResponseCM
        {
            get
            {
                return "AddS2SPFAdditionalDamagesResponse_Stage2SettlementPackAdditionalDamagesResponse-v3.2.xsd";
            }
        }


        public string CourtProceedingsRequestCR
        {
            get
            {
                return "AddCPPFRequest_CPPFRequestXML-v3.3.xsd";
            }
        }


        public string CourtProceedingsResponseCM
        {
            get
            {
                return "AddCPPFResponse_CPPFResponseXML-v3.3.xsd";
            }
        }


        public string AddClaim
        {
            get
            {
                return "AddClaim_ClaimData-v3.8.xsd";
            }
        }
    }

}
