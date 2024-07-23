using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using RTAServicesLibrary.PIPService;

namespace RTAServicesLibrary
{
    public class RTAServices1 : RTAServiceBase
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="loginDetails">Login details</param>
        /// <param name="tokenStorageProvider">Token storage provider</param>
        public RTAServices1(RTALoginDetails loginDetails, ITokenStorageProvider tokenStorageProvider) :
            base(loginDetails, tokenStorageProvider)
        {
            if (tokenStorageProvider == null)
                throw new ArgumentNullException(nameof(tokenStorageProvider));
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// Get Claims List Data Table
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static DataTable GetClaimsListDataTable(getClaimsListResponse response)
        {
            DataTable table = new DataTable("GetClaimsList");
            DataColumn column;
            DataRow row;

            //  Application Id
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "applicationId";
            column.Caption = "applicationId";
            table.Columns.Add(column);

            //  Activity Engine Guid
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "activityEngineGuid";
            column.Caption = "activityEngineGuid";
            table.Columns.Add(column);

            //  Application References
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "applicationReferences";
            column.Caption = "applicationReferences";
            table.Columns.Add(column);

            //  Attachments Count
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "attachmentsCount";
            column.Caption = "attachmentsCount";
            table.Columns.Add(column);

            //  Creation Time
            column = new DataColumn();
            column.DataType = typeof(DateTime);
            column.ColumnName = "creationTime";
            column.Caption = "creationTime";
            table.Columns.Add(column);

            //  Current User ID
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "currentUserID";
            column.Caption = "currentUserID";
            table.Columns.Add(column);

            //  Lock Status
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "lockStatus";
            column.Caption = "lockStatus";
            table.Columns.Add(column);

            //  Lock User Id
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "lockUserId";
            column.Caption = "lockUserId";
            table.Columns.Add(column);

            //  Phase Cache Id
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "phaseCacheId";
            column.Caption = "phaseCacheId";
            table.Columns.Add(column);

            //  Phase Cache Name
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "phaseCacheName";
            column.Caption = "phaseCacheName";
            table.Columns.Add(column);

            //  Printable Documents Count
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "printableDocumentsCount";
            column.Caption = "printableDocumentsCount";
            table.Columns.Add(column);

            //  Version Major
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "versionMajor";
            column.Caption = "versionMajor";
            table.Columns.Add(column);

            //  Version Minor
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "versionMinor";
            column.Caption = "versionMinor";
            table.Columns.Add(column);

            foreach (claim c in response.claimsListResponse.claimsList)
            {
                row = table.NewRow();
                row["applicationId"] = c.applicationId;
                row["activityEngineGuid"] = c.activityEngineGuid;
                row["applicationReferences"] = c.applicationReferences;
                row["attachmentsCount"] = c.attachmentsCount;
                row["creationTime"] = c.creationTime;
                row["currentUserID"] = c.currentUserID;
                row["lockStatus"] = c.lockStatus;
                row["lockUserId"] = c.lockUserId;
                row["phaseCacheId"] = c.phaseCacheId;
                row["phaseCacheName"] = c.phaseCacheName;
                row["printableDocumentsCount"] = c.printableDocumentsCount;
                row["versionMajor"] = c.versionMajor;
                row["versionMinor"] = c.versionMinor;
                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// Get Notifications List Data Table
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        private DataTable GetNotificationsListDataTable(notification[] notes)
        {
            DataTable table = new DataTable("GetNotificationsList");
            DataColumn column;
            DataRow row;

            //  Application Id
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "applicationId";
            column.Caption = "applicationId";
            table.Columns.Add(column);

            //  Formatted Date
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "formattedDate";
            column.Caption = "formattedDate";
            table.Columns.Add(column);

            //  Notification Date Time
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "notificationDateTime";
            column.Caption = "notificationDateTime";
            table.Columns.Add(column);

            //  Notification Guid
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "notificationGuid";
            column.Caption = "notificationGuid";
            table.Columns.Add(column);

            //  Notification Message
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "notificationMessage";
            column.Caption = "notificationMessage";
            table.Columns.Add(column);

            if (notes == null || notes.Length == 0)
            {
                CommonMethods.LogMessage("No RTA notifications retrieved from GetNotificationsList() call to the Claims Portal.", System.Diagnostics.EventLogEntryType.Error);
                return table;
            }

            foreach (notification note in notes)
            {
                row = table.NewRow();
                row["applicationId"] = note.applicationId;
                row["formattedDate"] = note.formattedDate;
                row["notificationDateTime"] = note.notificationDateTime;
                row["notificationGuid"] = note.notificationGuid;
                row["notificationMessage"] = note.notificationMessage;
                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// Seach Compensators Data Table
        /// </summary>
        /// <param name="orgInfo"></param>
        /// <returns></returns>
        private DataTable SeachCompensatorsDataTable(organisationInfo[] orgInfo)
        {
            DataTable table = new DataTable("SeachCompensators");
            DataColumn column;
            DataRow row;

            //  Organisation Id
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "organisationId";
            column.Caption = "organisationId";
            table.Columns.Add(column);

            //  Organisation Name
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "organisationName";
            column.Caption = "organisationName";
            table.Columns.Add(column);

            //  Organisation Path
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "organisationPath";
            column.Caption = "organisationPath";
            table.Columns.Add(column);

            foreach (organisationInfo info in orgInfo)
            {
                row = table.NewRow();
                row["organisationId"] = info.organisationId;
                row["organisationName"] = info.organisationName;
                row["organisationPath"] = info.organisationPath;
                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// Get Organisation Data Table
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        private DataTable GetOrganisationDataTable(organisation org)
        {
            DataTable table = new DataTable("GetOrganisation");
            DataColumn column;
            DataRow row;

            //  Organisation Id
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "organisationId";
            column.Caption = "organisationId";
            table.Columns.Add(column);

            //  Organisation Name
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "organisationName";
            column.Caption = "organisationName";
            table.Columns.Add(column);

            //  Organisation Path
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "organisationPath";
            column.Caption = "organisationPath";
            table.Columns.Add(column);

            //  Address Type
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "addressType";
            column.Caption = "addressType";
            table.Columns.Add(column);

            //  City
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "city";
            column.Caption = "city";
            table.Columns.Add(column);

            //  Country
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "country";
            column.Caption = "country";
            table.Columns.Add(column);

            //  County
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "county";
            column.Caption = "county";
            table.Columns.Add(column);

            //  District
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "district";
            column.Caption = "district";
            table.Columns.Add(column);

            //  House Name
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "houseName";
            column.Caption = "houseName";
            table.Columns.Add(column);

            //  House Number
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "houseNumber";
            column.Caption = "houseNumber";
            table.Columns.Add(column);

            //  PostCode
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "postCode";
            column.Caption = "postCode";
            table.Columns.Add(column);

            //  Street1
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "street1";
            column.Caption = "street1";
            table.Columns.Add(column);

            //  Street2
            column = new DataColumn();
            column.DataType = typeof(string);
            column.ColumnName = "street2";
            column.Caption = "street2";
            table.Columns.Add(column);

            row = table.NewRow();
            row["organisationId"] = org.organisationId;
            row["organisationName"] = org.organisationName;
            row["organisationPath"] = org.organisationPath;
            row["addressType"] = org.address.addressType;
            row["city"] = org.address.city;
            row["country"] = org.address.country;
            row["county"] = org.address.county;
            row["district"] = org.address.district;
            row["houseName"] = org.address.houseName;
            row["houseNumber"] = org.address.houseNumber;
            row["postCode"] = org.address.postCode;
            row["street1"] = org.address.street1;
            row["street2"] = org.address.street2;
            table.Rows.Add(row);

            return table;
        }


        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Class To XML String
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="myClass"></param>
        /// <returns></returns>
        public string ClassToXMLString<T>(T myClass)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, myClass);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Class To XML Error: " + ex.Message);
            }
        }

        /// <summary>
        /// XML String To Class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public T XMLStringToClass<T>(string xml)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV2.DocumentInput));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xml);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (T)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("XML To Class Error: " + ex.Message);
            }
        }


        /// <summary>
        /// XML String To Class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public T XMLStringToClassV3<T>(string xml)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV3.DocumentInput));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xml);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (T)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("XML To Class Error: " + ex.Message);
            }
        }


        /// <summary>
        /// XML String To Class.
        /// Send in a new DocumentInput object to convert the xml into.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public TDocumentInput XMLStringToClass<TDocumentInput>(string xml, TDocumentInput documentInput)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(documentInput.GetType());
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xml);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (TDocumentInput)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("XML To Class Error: " + ex.Message);
            }
        }

        #region GenerateClaimantNotificationFormXml()

        /// <summary>
        /// Generate Claimant Notification Form in XML
        /// </summary>
        /// <returns></returns>
        public string GenerateClaimantNotificationFormXml(RTAServicesLibraryV2.DocumentInput doc)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV2.DocumentInput));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, doc);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate CNF Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Generate Claimant Notification Form in XML
        /// </summary>
        /// <returns></returns>
        public string GenerateClaimantNotificationFormXml(RTAServicesLibraryV3.DocumentInput doc)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV3.DocumentInput));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, doc);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate CNF Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Generate Claimant Notification Form in XML
        /// </summary>
        /// <returns></returns>
        public string GenerateClaimantNotificationFormXml<TDocumentInput>(TDocumentInput documentInput)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(documentInput.GetType());
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, documentInput);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate CNF Error: " + ex.Message);
            }
        }

        #endregion


        #region GetDocumentInputFromXML()
        
        /// <summary>
        /// Get DocumentInput From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV2.DocumentInput GetDocumentInputFromXML(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV2.DocumentInput));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV2.DocumentInput)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Document Input XML Error: " + ex.Message);
            }
        }


        /// <summary>
        /// Get DocumentInput From XML
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public RTAServicesLibraryV3.DocumentInput GetDocumentInputFromXMLV3(string xmlString)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(typeof(RTAServicesLibraryV3.DocumentInput));
                UTF8Encoding encoding = new UTF8Encoding();
                Byte[] byteArray = encoding.GetBytes(xmlString);
                MemoryStream memoryStream = new MemoryStream(byteArray);
                return (RTAServicesLibraryV3.DocumentInput)xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw new System.Exception("Get Document Input XML Error: " + ex.Message);
            }
        }

        #endregion


        #region GenerateInsurerResponseA2AXML()

        /// <summary>
        /// Generate Insurer Response A2A XML
        /// </summary>
        /// <param name="insurer"></param>
        /// <returns></returns>
        private string GenerateInsurerResponseA2AXML(RTAServicesLibraryV2.InsurerResponseA2A insurer)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(RTAServicesLibraryV2.InsurerResponseA2A));
                StringWriter writer = new StringWriter();
                serializer.Serialize(writer, insurer);

                StringBuilder sb = new StringBuilder(writer.ToString());
                sb.Replace(XML_HEADER_FILTER, string.Empty);

                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new System.Exception("Generate InsurerResponseA2A Error: " + ex.Message);
            }
        }

        #endregion


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

        
        #endregion Public Methods
        
        #region Stage 1 Methods

        #region AddClaim()

        public claimInfo AddClaim<TDocumentInput>(TDocumentInput documentInput)
        {
            var documentInputType = documentInput.GetType();

            if (documentInputType == typeof(RTAServicesLibraryV2.DocumentInput))
            {
                var V2DocInput = documentInput as RTAServicesLibraryV2.DocumentInput;
                return AddClaim(V2DocInput);
            }
            else if (documentInputType == typeof(RTAServicesLibraryV3.DocumentInput))
            {
                var V3DocInput = documentInput as RTAServicesLibraryV3.DocumentInput;
                return AddClaim(V3DocInput);
            }
            else if (documentInputType == typeof(RTAServicesLibraryV5.DocumentInput))
            {
                var V5DocInput = documentInput as RTAServicesLibraryV5.DocumentInput;
                return AddClaim(V5DocInput);
            }
            else if (documentInputType == typeof(RTAServicesLibraryV6.DocumentInput))
            {
                var V6DocInput = documentInput as RTAServicesLibraryV6.DocumentInput;
                return AddClaim(V6DocInput);
            }
            else // V7
            {
                var V7DocInput = documentInput as RTAServicesLibraryV7.DocumentInput;
                return AddClaim(V7DocInput);
            }
        }


        /// <summary>
        /// Add Claim - allows the Claimant Representative to add a new claim into the system.
        /// </summary>
        public claimInfo AddClaim(RTAServicesLibraryV2.DocumentInput doc)
        {
            //  This request allows you to add a new claim into the system.
            using (PIPService.PIPService service = GetRTAService())
            {
                addClaim claim = new addClaim();
                claim.accessToken = GetAccessToken();

                //  Generate the CNF into xml
                string xml = GenerateClaimantNotificationFormXml(doc);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                    "",
                                                                    ValidationServices.ProcessActions.AddClaim);

                //  Validate the xml CNF
                ValidationServices.ValidateXML(xml, xsd);

                claim.claimXML = xml;

                addClaimResponse response = service.addClaim(claim);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add Claim " + errorMessage);
                }
            }
        }


        /// <summary>
        /// Add Claim - allows the Claimant Representative to add a new claim into the system.
        /// </summary>
        public claimInfo AddClaim(RTAServicesLibraryV3.DocumentInput doc)
        {
            //  This request allows you to add a new claim into the system.
            using (PIPService.PIPService service = GetRTAService())
            {
                addClaim claim = new addClaim();
                claim.accessToken = GetAccessToken();

                //  Generate the CNF into xml
                string xml = GenerateClaimantNotificationFormXml(doc);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                    "",
                                                                    ValidationServices.ProcessActions.AddClaim);


                //  Validate the xml CNF
                ValidationServices.ValidateXML(xml, xsd);
                claim.claimXML = xml;

                addClaimResponse response = service.addClaim(claim);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add Claim " + errorMessage);
                }
            }
        }


        /// <summary>
        /// Add Claim - allows the Claimant Representative to add a new claim into the system.
        /// </summary>
        public claimInfo AddClaim(RTAServicesLibraryV5.DocumentInput doc)
        {
            //  This request allows you to add a new claim into the system.
            using (PIPService.PIPService service = GetRTAService())
            {
                addClaim claim = new addClaim();
                claim.accessToken = GetAccessToken();

                //  Generate the CNF into xml
                string xml = GenerateClaimantNotificationFormXml(doc);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                    "",
                                                                    ValidationServices.ProcessActions.AddClaim);


                //  Validate the xml CNF
                ValidationServices.ValidateXML(xml, xsd);
                claim.claimXML = xml;

                addClaimResponse response = service.addClaim(claim);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add Claim " + errorMessage);
                }
            }
        }


        /// <summary>
        /// Add Claim - allows the Claimant Representative to add a new claim into the system.
        /// </summary>
        public claimInfo AddClaim(RTAServicesLibraryV6.DocumentInput doc)
        {
            //  This request allows you to add a new claim into the system.
            using (PIPService.PIPService service = GetRTAService())
            {
                addClaim claim = new addClaim();
                claim.accessToken = GetAccessToken();

                //  Generate the CNF into xml
                string xml = GenerateClaimantNotificationFormXml(doc);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                    "",
                                                                    ValidationServices.ProcessActions.AddClaim);


                //  Validate the xml CNF
                ValidationServices.ValidateXML(xml, xsd);
                claim.claimXML = xml;

                addClaimResponse response = service.addClaim(claim);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add Claim " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Add Claim - allows the Claimant Representative to add a new claim into the system.
        /// </summary>
        public claimInfo AddClaim(RTAServicesLibraryV7.DocumentInput doc)
        {
            //  This request allows you to add a new claim into the system.
            using (PIPService.PIPService service = GetRTAService())
            {
                addClaim claim = new addClaim();
                claim.accessToken = GetAccessToken();

                //  Generate the CNF into xml
                string xml = GenerateClaimantNotificationFormXml(doc);
                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                    "",
                                                                    ValidationServices.ProcessActions.AddClaim);


                //  Validate the xml CNF
                ValidationServices.ValidateXML(xml, xsd);
                claim.claimXML = xml;

                addClaimResponse response = service.addClaim(claim);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add Claim " + errorMessage);
                }
            }
        }

        #endregion

        /// <summary>
        /// Accept Claim - allows a Compensator to officially indicate that the claim is theirs.
        /// </summary>
        public claimInfo AcceptClaim(string activityEngineGuid, string applicationId)
        {
            //  This request allows a Compensator to officially indicate that the claim is theirs. 
            //  From this moment on, a Compensator can not re-assign the claim to another compensator 
            //  or to resend it back to the CR.            
            using (PIPService.PIPService service = GetRTAService())
            {
                acceptClaim claim = new acceptClaim();
                claim.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                claim.claimData = data;

                acceptClaimResponse response = service.acceptClaim(claim);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Accept Claim " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Resend a Rejected Claim
        /// </summary>
        /// <param name="claim"></param>
        /// <returns></returns>
        public claimInfo ResendRejectedClaim(string activityEngineGuid, string applicationId, string claimXML)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                resendRejectedClaim rejectedClaim = new resendRejectedClaim();
                rejectedClaim.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                rejectedClaim.claimData = data;
                rejectedClaim.claimXML = claimXML;

                resendRejectedClaimResponse response = service.resendRejectedClaim(rejectedClaim);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Resend Rejected Claim " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Acknowledge or Denied Liability
        /// </summary>
        public claimInfo AcknowledgeDeniedLiability(string activityEngineGuid, string applicationId)
        {
            //  This request allows a CR to simply indicate that the Payment for the given Claim has been received or not
            using (PIPService.PIPService service = GetRTAService())
            {
                acknowledgeDeniedLiability liability = new acknowledgeDeniedLiability();
                liability.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                liability.claimData = data;

                acknowledgeDeniedLiabilityResponse response = service.acknowledgeDeniedLiability(liability);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Denied Liability " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Acknowledge Fraud Stated
        /// </summary>
        /// <param name="fraud"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeFraudStated(string activityEngineGuid, string applicationId)
        {
            //  This request allows a CR to simply indicate that they saw the message about Fraud stated by the Compensator
            using (PIPService.PIPService service = GetRTAService())
            {
                acknowledgeFraudStated fraud = new acknowledgeFraudStated();
                fraud.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                fraud.claimData = data;

                acknowledgeFraudStatedResponse response = service.acknowledgeFraudStated(fraud);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Fraud Stated " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Compensator Sends a Liability Decision
        /// </summary>
        public claimInfo SendLiabilityDecision(string activityEngineGuid, string applicationId, RTAServicesLibraryV2.InsurerResponseA2A insurerResponse)
        {
            //  This request allows a CM to send the response about the Liability for the given Claim
            using (PIPService.PIPService service = GetRTAService())
            {
                sendLiabilityDecision decision = new sendLiabilityDecision();
                decision.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                decision.claimData = data;
                decision.insurerResponseXml = GenerateInsurerResponseA2AXML(insurerResponse);

                sendLiabilityDecisionResponse response = service.sendLiabilityDecision(decision);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Send Liability Decision " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Compensator Apply Article 75
        /// </summary>
        public claimInfo ApplyArticle75(string activityEngineGuid, string applicationId, bool isArticle75)
        {
            //  This request allows a CM to apply the Article 75 for a claim
            //  If the CM is not a MIB they can apply for Article 75
            //  Article 75 will increase the Time Limit from 15 to 30 days
            using (PIPService.PIPService service = GetRTAService())
            {
                applyArticle75 article75 = new applyArticle75();
                article75.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                article75.claimData = data;
                article75.isArticle75 = isArticle75;

                applyArticle75Response response = service.applyArticle75(article75);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Apply Article 75 " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Get a Claim
        /// </summary>
        public string GetClaim(string applicationId)
        {
            //  This request allows to get all the information stored within a particular claim 
            //  up to the phase in which the claim has arrived. The data returned by the functionality 
            //  includes process and business information.
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaim claim = new getClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                getClaimResponse response = service.getClaim(claim);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    return response.stringResponse.value;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Get Claim " + errorMessage);
                }
            }
        }


        /// <summary>
        /// Get Claims List
        /// </summary>
        public DataTable GetClaimsList()
        {
            //  This request allows to get the information found on the work list of a single user. 
            //  The data structure returned includes all the information needed for retrieving 
            //  details on a single claim and its status.
            using (PIPService.PIPService service = GetRTAService())
            {
                getClaimsList claimList = new getClaimsList();
                claimList.accessToken = GetAccessToken();

                getClaimsListResponse response = service.getClaimsList(claimList);


                if (response.claimsListResponse.code == responseCode.Ok)
                {
                    return GetClaimsListDataTable(response);
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimsListResponse);
                    throw new Exception("Get Claims List " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Get a Claims Status
        /// </summary>
        public claimInfo GetClaimsStatus(string applicationId)
        {
            //  This request allows to retrieve the status of a particular claim. 
            //  This function can be used in order to know the position in the 
            //  process of a particular claim and so to update the claim using the right activityEngineGUID.

            using (PIPService.PIPService service = GetRTAService())
            {
                getClaimsStatus claimStatus = new getClaimsStatus();
                claimStatus.accessToken = GetAccessToken();
                claimStatus.applicationId = applicationId;

                getClaimsStatusResponse response = service.getClaimsStatus(claimStatus);

                if (response.claimInfo2Response.code == responseCode.Ok)
                {
                    return response.claimInfo2Response.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfo2Response);
                    throw new Exception("Get Claims Status " + errorMessage);
                }
            }
        }


        /// <summary>
        /// Reject a Claim back to the Claimant Representative.
        /// </summary>
        public claimInfo RejectClaimToCR(string activityEngineGuid, string applicationId)
        {
            var reject = new rejectClaimToCR();
            var claimInfo = RejectClaimToCR(activityEngineGuid, applicationId, reject);
            return claimInfo;
        }


        /// <summary>
        /// Reject a Claim back to the Claimant Representative. 
        /// This method is for use with Release 5+ and now requires a rejection reason.
        /// </summary>
        public claimInfo RejectClaimToCR(string activityEngineGuid, string applicationId, string rejectionReasonCode)
        {
            var reject = new rejectClaimToCR() { rejectionReasonCode = rejectionReasonCode };
            var claimInfo = RejectClaimToCR(activityEngineGuid, applicationId, reject);
            return claimInfo;
        }


        private claimInfo RejectClaimToCR(string activityEngineGuid, string applicationId, rejectClaimToCR reject)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                reject.claimData = data;
                reject.accessToken = GetAccessToken();

                rejectClaimToCRResponse response = service.rejectClaimToCR(reject);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Reject Claim To CR " + errorMessage);
                }
            }
        }


        /// <summary>
        /// Acknowledge a rejected claim
        /// </summary>
        public claimInfo AcknowledgeRejectedClaim(string activityEngineGuid, string applicationId)
        {
            //  This request allows a CR to send an acknowledgment of the rejected claim back to the COMP.
            using (PIPService.PIPService service = GetRTAService())
            {
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;

                var arc = new acknowledgeRejectedClaim();
                arc.claimData = data;
                arc.accessToken = GetAccessToken();

                var response = service.acknowledgeRejectedClaim(arc);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Rejected Claim " + errorMessage);
                }
            }
        }


        /// <summary>
        /// Exit a rejected claim
        /// </summary>
        public claimInfo ExitRejectedClaim(string activityEngineGuid, string applicationId)
        {
            //  This request allows a CR to exit a rejected claim, notifying the COMP.
            using (PIPService.PIPService service = GetRTAService())
            {
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;

                var erc = new exitRejectedClaim();
                erc.claimData = data;
                erc.accessToken = GetAccessToken();

                var response = service.exitRejectedClaim(erc);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Exit Rejected Claim " + errorMessage);
                }
            }
        }



        /// <summary>
        /// Reassign Claim to another Compensator
        /// </summary>
        public claimInfo ReassignToAnotherCM(string activityEngineGuid, string applicationId, string organisationPath)
        {
            //  This request allows a CM to re-assign the claim to another CR.
            using (PIPService.PIPService service = GetRTAService())
            {
                reassignToAnotherCM reassign = new reassignToAnotherCM();
                reassign.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                reassign.claimData = data;
                reassign.organisationPath = organisationPath;

                reassignToAnotherCMResponse response = service.reassignToAnotherCM(reassign);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Reassign To Another CM " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Set Stage1 Payment
        /// </summary>
        public claimInfo SetStage1Payment(string activityEngineGuid, string applicationId, bool isStage1Paid)
        {
            //  This request allows a CR to simply indicate that the Payment for the 
            //  given Claim has been received or not
            using (PIPService.PIPService service = GetRTAService())
            {
                setStage1Payment payment = new setStage1Payment();
                payment.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                payment.claimData = data;
                payment.isStage1Paid = isStage1Paid;

                setStage1PaymentResponse response = service.setStage1Payment(payment);

                if (response.response.code == responseCode.Ok)
                {
                    return response.response.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.response);
                    throw new Exception("Set Stage 1 Payment " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Search for Claims
        /// </summary>
        public claim[] SearchClaims(searchClaims sClaims)
        {
            //  This request allows to search for a claim by specifying a set of search criteria.
            //  Search criteria:
            //  claimID, Claim CR Reference number, Claim CM Reference Number, phase, branchID, start and end dates
            using (PIPService.PIPService service = GetRTAService())
            {
                sClaims.accessToken = GetAccessToken();
                searchClaimsResponse response = service.searchClaims(sClaims);

                if (response.claimInfosListResponse.code == responseCode.Ok)
                {
                    return response.claimInfosListResponse.claimsList;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfosListResponse);
                    throw new Exception("Search Claims " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Lock a Claim
        /// </summary>
        public void LockClaim(string activityEngineGuid, string applicationId)
        {
            //  This request allows to lock a claim.
            //  Only the user who has locked the claim can then update it. 
            //  The claim can be unlocked by the same user who has locked 
            //  using an unlock claim method or can be unlocked by other users using the force unlock method.
            using (PIPService.PIPService service = GetRTAService())
            {
                lockClaim claim = new lockClaim();
                claim.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                claim.claimData = data;

                lockClaimResponse response = service.lockClaim(claim);

                if (response.response.code == responseCode.Ok)
                {
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.response);
                    throw new Exception("Lock Claim " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Unlock a Claim
        /// </summary>
        public void UnlockClaim(string applicationId)
        {
            //  This request allows to unlock a claim.
            //  Only the user who had locked the claim can unlock it.
            using (PIPService.PIPService service = GetRTAService())
            {
                unlockClaim claim = new unlockClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                unlockClaimResponse response = service.unlockClaim(claim);

                if (response.response.code == responseCode.Ok)
                {
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.response);
                    throw new Exception("Unlock Claim " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Force Unlock Claim
        /// </summary>
        public void ForceUnlockClaim(string applicationId)
        {
            //  This request allows to force the unlocking of any claim, currently locked by any user.
            using (PIPService.PIPService service = GetRTAService())
            {
                forceUnlockClaim claim = new forceUnlockClaim();
                claim.accessToken = GetAccessToken();
                claim.applicationId = applicationId;

                forceUnlockClaimResponse response = service.forceUnlockClaim(claim);

                if (response.response.code == responseCode.Ok)
                {
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.response);
                    throw new Exception("Force Unlock Claim " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Get an Organisation
        /// </summary>
        public DataTable GetOrganisation(string organisationPath)
        {
            //  This request allows to get the details of an Organisation starting from the path
            using (PIPService.PIPService service = GetRTAService())
            {
                getOrganisation org = new getOrganisation();
                org.accessToken = GetAccessToken();
                org.organisationPath = organisationPath;

                getOrganisationResponse response = service.getOrganisation(org);

                if (response.organisationResponse.code == PIPService.responseCode.Ok)
                {
                    return GetOrganisationDataTable(response.organisationResponse.organisation);
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.organisationResponse);
                    throw new Exception("Get Organisation " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Get Hospitals List
        /// </summary>
        public hospital[] GetHospitalsList(string hospitalName, string postCode)
        {
            //  This request allows to get a list of public Hospitals in the system, given some 
            //  search criteria. It can be used to align the client system with the whole list 
            //  of public (NHS) Hospitals in the DB.
            using (PIPService.PIPService service = GetRTAService())
            {
                getHospitalsList list = new getHospitalsList();
                list.accessToken = GetAccessToken();
                list.hospitalName = hospitalName;
                list.postCode = postCode;

                getHospitalsListResponse response = service.getHospitalsList(list);

                if (response.hospitalsListResponse.code == responseCode.Ok)
                {
                    return response.hospitalsListResponse.hospitalsList;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.hospitalsListResponse);
                    throw new Exception("Get Hospitals List " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Get Branches List
        /// </summary>
        public organisationInfo[] GetBranchesList()
        {
            //  This request allows to get the list of the branches of the Organisation of the user “asUser”
            //  Since the list of branches does not change so often, it should be called only in case of 
            //  reasonable doubts that the list was updated after the last time the same function was called.
            using (PIPService.PIPService service = GetRTAService())
            {
                getBranchesList list = new getBranchesList();
                list.accessToken = GetAccessToken();

                getBranchesListResponse response = service.getBranchesList(list);

                if (response.organisationInfosListResponse.code == responseCode.Ok)
                {
                    return response.organisationInfosListResponse.organisationInfosList;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.organisationInfosListResponse);
                    throw new Exception("Get Branches List " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Allocate Claim To Branch
        /// </summary>
        public claimInfo AllocateClaimToBranch(string activityEngineGuid, string applicationId, string branchId)
        {
            //  This request allows a CM to allocate the claim to a branch of their organisation
            using (PIPService.PIPService service = GetRTAService())
            {
                allocateClaimToBranch branch = new allocateClaimToBranch();
                branch.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                branch.claimData = data;
                branch.branchId = branchId;

                allocateClaimToBranchResponse response = service.allocateClaimToBranch(branch);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Allocate Claim To Branch " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Add Attachment to a claim
        /// </summary>
        public string AddAttachment(string applicationID, string filePath, string fileName, string title, string description)
        {
            //  This request allows to add attachment to a particular claim. 
            //  The attachment has to be passed to the interface as stream of bytes. 
            //  The attachment is identified in the system with an attachment ID, a title and a description.
            //  The maximum size allowed is 4 MB.
            using (PIPService.PIPService service = GetRTAService())
            {
                addAttachment addAtt = new addAttachment();
                addAtt.accessToken = GetAccessToken();

                attachment att = new attachment();

                att.applicationId = applicationID;
                att.dataAttachmentDesc = description;
                att.dataAttachmentTitle = title;
                att.dataAttachmentFileName = fileName;

                Stream inputStream = File.OpenRead(filePath);
                byte[] buffer = new byte[inputStream.Length];
                inputStream.Read(buffer, 0, Convert.ToInt32(inputStream.Length));

                att.dataAttachmentFileZip = buffer;
                addAtt.attachment = att;

                addAttachmentResponse response = service.addAttachment(addAtt);

                inputStream.Close();

                if (response.stringResponse.code == responseCode.Ok)
                {
                    return response.stringResponse.value;    //  The ID of the Attachment
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Add Attachment " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Save Attachment File To Path
        /// </summary>
        /// <param name="att"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string SaveAttachmentFileToPath(attachment attach, string filePath)
        {
            string path = filePath + attach.dataAttachmentTitle;
            Stream outStream = File.OpenWrite(filePath);
            outStream.Write(attach.dataAttachmentFileZip, 0, attach.dataAttachmentFileZip.Length);
            outStream.Close();

            return path;
        }

        /// <summary>
        /// Get an Attachment from a claim
        /// </summary>
        public attachment GetAttachment(string attachmentGuid)
        {
            //  This request allows to get an attachment
            using (PIPService.PIPService service = GetRTAService())
            {
                getAttachment attachment = new getAttachment();
                attachment.accessToken = GetAccessToken();
                attachment.attachmentGuid = attachmentGuid;

                getAttachmentResponse response = service.getAttachment(attachment);

                if (response.attachmentResponse.code == responseCode.Ok)
                {
                    return response.attachmentResponse.attachment;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.attachmentResponse);
                    throw new Exception("Get Attachment " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Get Attachments List
        /// </summary>
        public attachment[] GetAttachmentsList(string applicationId)
        {
            //  This request allows to get the list of attachments of to a particular claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                getAttachmentsList list = new getAttachmentsList();
                list.accessToken = GetAccessToken();
                list.applicationId = applicationId;

                getAttachmentsListResponse response = service.getAttachmentsList(list);

                if (response.attachmentListResponse.code == responseCode.Ok)
                {
                    return response.attachmentListResponse.attachemntsList;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.attachmentListResponse);
                    throw new Exception("Get Attachments List " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Seach Compensators
        /// </summary>
        /// <returns></returns>
        public DataTable SeachCompensators(string organisationName, string compensatorType)
        {
            //  This request allows to search a compensator, in order to get the OrgnisationId to send the claim.
            using (PIPService.PIPService service = GetRTAService())
            {
                searchCompensators compensators = new searchCompensators();
                compensators.accessToken = GetAccessToken();
                compensators.compensatorType = compensatorType;
                compensators.organisationName = organisationName;

                searchCompensatorsResponse response = service.searchCompensators(compensators);

                if (response.organisationInfosListResponse.code == responseCode.Ok)
                {
                    return SeachCompensatorsDataTable(response.organisationInfosListResponse.organisationInfosList);
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.organisationInfosListResponse);
                    throw new Exception("Seach Compensators " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Search Compensators By Insurer Index
        /// </summary>
        /// <param name="organisationName"></param>
        /// <param name="compensatorType"></param>
        /// <returns></returns>
        public insurerInfo[] SearchCompensatorsByInsurerIndex(string organisationName, string compensatorType)
        {
            //  This request allows to search a compensator, using the new InsurersIndexTable introduced 
            //  in the second phase of the project, in order to get the OrgnisationId to send the claim 
            //  to and the Insurer Name to be inserted in the CNF in the field “InsurerName” of the Defendant’s Insurer.
            using (PIPService.PIPService service = GetRTAService())
            {
                searchCompensatorsByInsurerIndex insurerIndex = new searchCompensatorsByInsurerIndex();
                insurerIndex.accessToken = GetAccessToken();
                insurerIndex.compensatorType = compensatorType;
                insurerIndex.organisationName = organisationName;

                searchCompensatorsByInsurerIndexResponse response = service.searchCompensatorsByInsurerIndex(insurerIndex);

                if (response.InsurerIndexList.code == responseCode.Ok)
                {
                    return response.InsurerIndexList.insurerInfoList;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.InsurerIndexList);
                    throw new Exception("Search Compensators By Insurer Index " + errorMessage);
                }
            }
        }


        /// <summary>
        /// Get Notifications List
        /// </summary>
        /// <returns></returns>
        public DataTable GetNotificationsList(bool a2aNotification = false)
        {
            //  This request allows to get the list of the Notifications available to the 
            //  user “asUser” (and displayed to this user in the section “Notifications” of the webUI) 
            //  not yet deleted from the system.
            using (PIPService.PIPService service = GetRTAService())
            {
                getNotificationsList list = new getNotificationsList();
                list.accessToken = GetAccessToken();

                list.A2ANotification = a2aNotification;
                list.A2ANotificationSpecified = true;

                getNotificationsListResponse response = service.getNotificationsList(list);

                if (response.notificationsListResponse.code == responseCode.Ok)
                {
                    return GetNotificationsListDataTable(response.notificationsListResponse.notificationsList);
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.notificationsListResponse);
                    throw new Exception("Get Notifications List " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Remove Notification
        /// </summary>
        public void RemoveNotification(string notificationGuid)
        {
            //  This request allows to delete the notification from the system.
            using (PIPService.PIPService service = GetRTAService())
            {
                removeNotification note = new removeNotification();
                note.accessToken = GetAccessToken();
                note.notificationGuid = notificationGuid;

                removeNotificationResponse response = service.removeNotification(note);

                if (response.response.code == responseCode.Ok)
                {
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.response);
                    throw new Exception("Remove Notification " + errorMessage);
                }
            }
        }

        /// <summary>
        /// State Fraud
        /// </summary>
        /// <returns></returns>
        public claimInfo StateFraud(string activityEngineGuid, string applicationId, string reasonCode, string reasonDescription)
        {
            //  This request allows a CM to throw the claim out of the process due to a Fraud, 
            //  adding also a reason for this action.
            using (PIPService.PIPService service = GetRTAService())
            {
                stateFraud fraud = new stateFraud();
                fraud.accessToken = GetAccessToken();
                claimData data = new claimData();
                data.activityEngineGuid = activityEngineGuid;
                data.applicationId = applicationId;
                fraud.claimData = data;
                fraud.reasonCode = reasonCode;
                fraud.reasonDescription = reasonDescription;

                stateFraudResponse response = service.stateFraud(fraud);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("State Fraud " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Get Printable Document
        /// </summary>
        /// <returns></returns>
        public attachment GetPrintableDocument(string printableDocumentId)
        {
            //  This request allows to get a Printable Document.
            using (PIPService.PIPService service = GetRTAService())
            {
                getPrintableDocument doc = new getPrintableDocument();
                doc.accessToken = GetAccessToken();
                doc.printableDocumentId = printableDocumentId;

                getPrintableDocumentResponse response = service.getPrintableDocument(doc);

                if (response.attachmentResponse.code == responseCode.Ok)
                {
                    return response.attachmentResponse.attachment;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.attachmentResponse);
                    throw new Exception("Get Printable Document " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Get Printable Documents List
        /// </summary>
        /// <returns></returns>
        public attachment[] GetPrintableDocumentsList(string applicationId)
        {
            //  This request allows to get an attachment as a stream of bytes.	
            using (PIPService.PIPService service = GetRTAService())
            {
                getPrintableDocumentsList list = new getPrintableDocumentsList();
                list.accessToken = GetAccessToken();
                list.applicationId = applicationId;

                getPrintableDocumentsListResponse response = service.getPrintableDocumentsList(list);

                if (response.attachmentsListResponse.code == responseCode.Ok)
                {
                    return response.attachmentsListResponse.attachemntsList;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.attachmentsListResponse);
                    throw new Exception("Get Printable Documents List " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Add Claim - allows you to add a new claim into the system.
        /// </summary>
        public claimInfo AddXMLClaim(string xml)
        {
            //  This request allows you to add a new claim into the system.
            using (PIPService.PIPService service = GetRTAService())
            {
                addClaim claim = new addClaim();
                claim.accessToken = GetAccessToken();

                var xsd = ValidationServices.GetXSDForValidation(this,
                                                                    "",
                                                                    ValidationServices.ProcessActions.AddClaim);

                //  Validate the xml CNF
                ValidationServices.ValidateXML(xml, xsd);

                claim.claimXML = xml;

                addClaimResponse response = service.addClaim(claim);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Add XML Claim " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Allocate User
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <param name="targetUserID"></param>
        /// <returns></returns>
        public claimInfo AllocateUser(string applicationId, string activityEngineGuid, string targetUserID)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                allocateUser user = new allocateUser();
                user.accessToken = GetAccessToken();
                user.targetUserID = targetUserID;

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                user.claimData = data;

                allocateUserResponse response = service.allocateUser(user);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Allocate User " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Deallocate User
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo DeallocateUser(string applicationId, string activityEngineGuid)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                deallocateUser user = new deallocateUser();
                user.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                user.claimData = data;

                deallocateUserResponse response = service.deallocateUser(user);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Deallocate User " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Get My Users List
        /// </summary>
        /// <returns></returns>
        public userInfo[] GetMyUsersList()
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getMyUsersList list = new getMyUsersList();
                list.accessToken = GetAccessToken();

                getMyUsersListResponse response = service.getMyUsersList(list);

                if (response.userInfosListResponse.code == responseCode.Ok)
                {
                    return response.userInfosListResponse.userInfosList;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.userInfosListResponse);
                    throw new Exception("Get My Users List " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Add Attachment
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="dataAttachmentDesc"></param>
        /// <param name="dataAttachmentFileName"></param>
        /// <param name="bytes"></param>
        /// <param name="dataAttachmentGuid"></param>
        /// <param name="dataAttachmentTitle"></param>
        /// <returns></returns>
        public string AddAttachment(string applicationId, string dataAttachmentDesc, string dataAttachmentFileName, byte[] bytes, string dataAttachmentGuid, string dataAttachmentTitle)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                addAttachment addAttach = new addAttachment();
                addAttach.accessToken = GetAccessToken();
                attachment attch = new attachment();
                attch.applicationId = applicationId;
                attch.dataAttachmentDesc = dataAttachmentDesc;
                attch.dataAttachmentFileName = dataAttachmentFileName;
                attch.dataAttachmentFileZip = bytes;
                attch.dataAttachmentGuid = dataAttachmentGuid;
                attch.dataAttachmentTitle = dataAttachmentTitle;

                addAttach.attachment = attch;

                addAttachmentResponse response = service.addAttachment(addAttach);

                if (response.stringResponse.code == responseCode.Ok)
                {
                    return response.stringResponse.message;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.stringResponse);
                    throw new Exception("Add Attachment Response " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Acknowledge Liability Admitted With Neg
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeLiabilityAdmittedWithNeg(string applicationId, string activityEngineGuid)
        {
            //  This command is needed to ensure the CR acknowledge the liability decision taken by the COMP.

            using (PIPService.PIPService service = GetRTAService())
            {

                acknowledgeLiabilityAdmittedWithNeg negligence = new acknowledgeLiabilityAdmittedWithNeg();
                negligence.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                negligence.claimData = data;

                acknowledgeLiabilityAdmittedWithNegResponse response = service.acknowledgeLiabilityAdmittedWithNeg(negligence);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Liability Admitted With Negligence " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Acknowledge Liability Admitted For Child
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeLiabilityAdmittedForChild(string applicationId, string activityEngineGuid)
        {
            //  This command is needed to ensure the CR acknowledge the liability decision taken by the COMP.

            using (PIPService.PIPService service = GetRTAService())
            {
                acknowledgeLiabilityAdmittedForChild child = new acknowledgeLiabilityAdmittedForChild();
                child.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                child.claimData = data;

                acknowledgeLiabilityAdmittedForChildResponse response = service.acknowledgeLiabilityAdmittedForChild(child);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Liability Admitted For Child " + errorMessage);
                }
            }
        }

        /// <summary>
        /// Exit Process
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <param name="exitReasonCode"></param>
        /// <param name="exitComment"></param>
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
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
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
                    throw new Exception("Exit Process " + errorMessage);
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
            //  This request allows a CR/COMP to simply indicate that they saw the message about the Exit Process stated by the other organisation

            using (PIPService.PIPService service = GetRTAService())
            {
                acknowledgeExitProcess exit = new acknowledgeExitProcess();
                exit.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                exit.claimData = data;

                acknowledgeExitProcessResponse response = service.acknowledgeExitProcess(exit);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Exit Process " + errorMessage);
                }
            }
        }


        #region GetSystemProcessVersion
        /// <summary>
        /// GetSystemProcessVersion but facilitate the switching between calls which and without applicationIDs
        /// </summary>
        /// <param name="applicationID"></param>
        /// <param name="services"></param>
        /// <returns></returns>
        public string GetSystemProcessVersion(string applicationID, RTAServices1 services)
        {
            string versionStamp = "";

            if (string.IsNullOrEmpty(applicationID))
            {
                versionStamp = services.GetSystemProcessVersion();
            }
            else
            {
                versionStamp = services.GetSystemProcessVersion(services, applicationID);
            }

            return versionStamp;
        }


        public string GetSystemProcessVersionReleaseCode(string versionStampString)
        {
            double versionStamp = Convert.ToDouble(versionStampString);

            if (ClaimsPortalTestEnvironmentInUse())
            {
                return GetTestEnvironmentVersionReleaseCode(versionStamp);
            }

            return GetProductionEnvironmentVersionReleaseCode(versionStamp);
        }


        private string GetProductionEnvironmentVersionReleaseCode(double versionStamp)
        {
            if (versionStamp < 2.0) return "R0";
            if (versionStamp >= 2.1 && versionStamp <= 3.0) return "R1";
            if (versionStamp >= 3.1 && versionStamp <= 3.2) return "R2";
            if (versionStamp >= 3.3 && versionStamp <= 3.9) return "R3";
            if (versionStamp >= 4.0 && versionStamp <= 4.1) return "R3"; // No A2A changes in Release 4
            if (versionStamp > 4.1 && versionStamp < 6.0)  return "R5"; // Release 5 jumps to version 5.0
            if (versionStamp >= 6.0 && versionStamp < 7.0) return "R6"; // Release 6 jumps to version 6.0
            if (versionStamp >= 7.0) return "R7";

            return "";
        }


        private string GetTestEnvironmentVersionReleaseCode(double versionStamp)
        {
            if (versionStamp < 3.5) return "R0";
            if (versionStamp >= 3.5 && versionStamp <= 4.3) return "R1";
            if (versionStamp >= 4.4 && versionStamp <= 5.4) return "R2";
            if (versionStamp >= 5.5 && versionStamp <= 6.4) return "R3";
            if (versionStamp >= 6.5 && versionStamp <= 6.7) return "R3"; // No A2A changes in Release 4
            if (versionStamp > 6.5 && versionStamp < 8.0) return "R5";
            if (versionStamp >= 8.0 && versionStamp <= 8.2) return "R6";
            if (versionStamp >= 9.0) return "R7";

            return "";
        }


        private const string TEST_ENVIRONMENT_URL = @"https://piptesta2a.crif.com/PIP.WSTK/PIPWSTK";

        private bool ClaimsPortalTestEnvironmentInUse()
        {
            return LoginDetails.Url.StartsWith(TEST_ENVIRONMENT_URL, StringComparison.InvariantCultureIgnoreCase);
        }


        /// <summary>
        /// Get System Process Version
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public systemProcessVersionResponse GetSystemProcessVersion(string applicationId, string activityEngineGuid)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getSystemProcessVersion process = new getSystemProcessVersion();
                process.accessToken = GetAccessToken();

                getSystemProcessVersionResponse response = service.getSystemProcessVersion(process);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Get System Process Version " + errorMessage);
                }
            }
        }



        /// <summary>
        /// Get current System Process Version for specific claims using a supplied applicationID
        /// </summary>
        /// <param name="RTA"></param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        private string GetSystemProcessVersion(RTAServices1 RTA, string applicationId)
        {
            searchClaims search = new searchClaims();
            searchClaimCriteria criteria = new searchClaimCriteria();
            criteria.applicationId = Convert.ToString(applicationId);
            search.searchClaimCriteria = criteria;
            claim[] claims = RTA.SearchClaims(search);
            return string.Format("{0}.{1}", claims[0].versionMajor, claims[0].versionMinor);
        }



        /// <summary>
        /// Get current System Process Version for claims not yet created and without a applicationID
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        private string GetSystemProcessVersion()
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                getSystemProcessVersion process = new getSystemProcessVersion();
                process.accessToken = GetAccessToken();

                getSystemProcessVersionResponse response = service.getSystemProcessVersion(process);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return string.Format("{0}.{1}", response.claimInfoResponse.systemProcessVersion.versionMajor, response.claimInfoResponse.systemProcessVersion.versionMinor);
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Get System Process Version " + errorMessage);
                }
            }
        }


        #endregion GetSystemProcessVersion



        /// <summary>
        /// Acknowledge Liability Decision Timeout
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeLiabilityDecisionTimeout(string applicationId, string activityEngineGuid)
        {
            using (PIPService.PIPService service = GetRTAService())
            {
                acknowledgeLiabilityDecisionTimeout decision = new acknowledgeLiabilityDecisionTimeout();
                decision.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                decision.claimData = data;

                acknowledgeLiabilityDecisionTimeoutResponse response = service.acknowledgeLiabilityDecisionTimeout(decision);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Liability Decision Timeout " + errorMessage);
                }
            }
        }


        #region AcknowledgeLiabilityAdmitted
        /// <summary>
        /// Acknowledge Liability Admitted
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="activityEngineGuid"></param>
        /// <returns></returns>
        public claimInfo AcknowledgeLiabilityAdmitted(string applicationId, string activityEngineGuid)
        {
            //  This command is needed to ensure the CR acknowledge the liability decision taken by the COMP.

            using (PIPService.PIPService service = GetRTAService())
            {
                acknowledgeLiabilityAdmitted liabilityAdmitted = new acknowledgeLiabilityAdmitted();
                liabilityAdmitted.accessToken = GetAccessToken();

                claimData data = new claimData();
                data.applicationId = applicationId;
                data.activityEngineGuid = activityEngineGuid;
                liabilityAdmitted.claimData = data;

                acknowledgeLiabilityAdmittedResponse response = service.acknowledgeLiabilityAdmitted(liabilityAdmitted);

                if (response.claimInfoResponse.code == responseCode.Ok)
                {
                    return response.claimInfoResponse.claimInfo;
                }
                else
                {
                    //  Failure or Error
                    string errorMessage = LogErrorMessage(response.claimInfoResponse);
                    throw new Exception("Acknowledge Liability Admitted " + errorMessage);
                }
            }
        }
        #endregion AcknowledgeLiabilityAdmitted

        #endregion Stage 1 Methods
    }
}
