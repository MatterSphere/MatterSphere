using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace RTAServicesLibrary
{
    static class ValidationServices
    {
        /// <summary>
        /// Validate XML
        /// </summary>
        public static void ValidateXML(string xmlData, string xmlScemas)
        {
            try
            {
                //GJ 03 September 2010 - Added section to get working folder from Registry if Set otherwise does nothing.
                //Check for Registry Setting - Also checks Policies
                string _fileLocation = Convert.ToString(new ApplicationSetting("RAPID", "", "", "XSDLocation").GetSetting("NotSet"));
                //If a Registry Setting is returned else do nothing.
                if (_fileLocation != "NotSet")
                {
                    //Check to see if a slash is needed at the end of the path if so add one.
                    if (_fileLocation.Substring(_fileLocation.Length - 1) != @"\")
                    {
                        _fileLocation = _fileLocation + @"\";
                    }
                    //Add XSD FileName to Path
                    _fileLocation = _fileLocation + xmlScemas;
                    //If File Exists in Location else do notthing.
                    if (System.IO.File.Exists(_fileLocation))
                    {
                        //Generate URI format string to pass to Schemas.Add
                        Uri _fileURI = new Uri(_fileLocation);
                        xmlScemas = _fileURI.ToString();
                    }
                }
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, xmlScemas);
                settings.ValidationType = ValidationType.Schema;

                XmlReader reader = XmlReader.Create(new StringReader(xmlData), settings);
                XmlDocument document = new XmlDocument();
                document.Load(reader);

                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);
                document.Validate(eventHandler);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Validate XML Error: " + ex.Message);
            }
        }

        /// <summary>
        /// Validation Event Handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    throw new System.Exception("XML Error: " + e.Message);
                case XmlSeverityType.Warning:
                    throw new System.Exception("XML Warning: " + e.Message);
            }

        }


        internal enum ProcessActions
        {
            AddClaim,
            InterimSettlementPackRequest,
            InterimSettlementPackResponse,
            SettlementPackRequest,
            SettlementPackResponse,
            SettlementPackCounterOfferRequestCR,
            SettlementPackCounterOfferResponseCM,
            AdditionalDamagesRequestCR,
            AdditionalDamagesResponseCM,
            CourtProceedingsPackRequestCR,
            CourtProceedingsPackResponseCM
        }


        internal static string GetXSDForValidation(RTAServiceBase serviceBase, string applicationId, ProcessActions action)
        {
            RTAServices1 rtaServices1 = (serviceBase as RTAServices1) ?? new RTAServices1(serviceBase.LoginDetails, serviceBase.TokenStorageProvider);

            var processVersion = rtaServices1.GetSystemProcessVersion(applicationId, rtaServices1);
            var processVersionCode = rtaServices1.GetSystemProcessVersionReleaseCode(processVersion);

            IXSDSchemaFiles xsds = XSDFileFactory.GetXSDObject(processVersionCode);

            switch (action)
            {
                case ProcessActions.InterimSettlementPackRequest:
                    return xsds.InterimSettlementPackRequest;

                case ProcessActions.InterimSettlementPackResponse:
                    return xsds.InterimSettlementPackResponse;

                case ProcessActions.SettlementPackRequest:
                    return xsds.SettlementPackRequest;

                case ProcessActions.SettlementPackResponse:
                    return xsds.SettlementPackResponse;

                case ProcessActions.SettlementPackCounterOfferRequestCR:
                    return xsds.SettlementPackCounterOfferRequestCR;

                case ProcessActions.SettlementPackCounterOfferResponseCM:
                    return xsds.SettlementPackCounterOfferResponseCM;

                case ProcessActions.AdditionalDamagesRequestCR:
                    return xsds.AdditionalDamagesRequestCR;

                case ProcessActions.AdditionalDamagesResponseCM:
                    return xsds.AdditionalDamagesResponseCM;

                case ProcessActions.CourtProceedingsPackRequestCR:
                    return xsds.CourtProceedingsRequestCR;

                case ProcessActions.CourtProceedingsPackResponseCM:
                    return xsds.CourtProceedingsResponseCM;

                case ProcessActions.AddClaim:
                    return xsds.AddClaim;
            }

            return "";
        }
    }
}
