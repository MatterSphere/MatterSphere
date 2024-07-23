using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using FWBS.ExternalAggregatorClient.ServiceAggregator;


namespace FWBS.ExternalAggregatorClient
{
    public static class ExternalAggregator
    {

        public static void SetSecurityOnObject(string objectType, string objectId, string connection)
        {
            List<FWBS.ExternalAggregatorClient.IntegrationPartner> partners = FetchIntegrationPartners(objectType, connection);    
            if (partners.Count > 0)
            {
                foreach(FWBS.ExternalAggregatorClient.IntegrationPartner p in partners)
                {
                    FetchIntegrationSecurityProperties(p.databaseCall, connection, objectId);
                }

            }

        }

        private static List<FWBS.ExternalAggregatorClient.IntegrationPartner> FetchIntegrationPartners(string objectType, string connection)
        {
            List<FWBS.ExternalAggregatorClient.IntegrationPartner> partners = new List<FWBS.ExternalAggregatorClient.IntegrationPartner>();
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connection))
            {
                SqlCommand com = new SqlCommand("[config].[GetSecurityIntegrationPartners]" , con);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.Clear();
                com.Parameters.AddWithValue("@objectType", objectType);

                SqlDataAdapter da = new SqlDataAdapter(com);
                con.Open();
                da.Fill(dt);
                con.Close();
             }
            if (dt.Rows.Count>0)
            {
                foreach(DataRow row in dt.Rows)
                {
                    FWBS.ExternalAggregatorClient.IntegrationPartner partner = new FWBS.ExternalAggregatorClient.IntegrationPartner() 
                        { integrator = Convert.ToString(row["IntegrationPartner"]) , databaseCall = Convert.ToString(row["SecurityCall"]) };
                    partners.Add(partner);
                }

            }

            return partners;

        }

        private static void FetchIntegrationSecurityProperties(string databaseCall, string connection, string objectId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(connection))
            {
                SqlCommand com = new SqlCommand(databaseCall , con);
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Clear();
                com.Parameters.AddWithValue("@objectId", objectId);

                SqlDataAdapter da = new SqlDataAdapter(com);
                con.Open();
                da.Fill(dt);
                con.Close();

                if (dt.Rows.Count > 0)
                {
                    Dictionary<string, uint> users = new Dictionary<string, uint>();
                    Dictionary<string, uint> groups = new Dictionary<string, uint>();
                    DataRow r = dt.Rows[0];

                    string clientId = Convert.ToString(r["ClientID"]);
                    string fileId = Convert.ToString(r["FileID"]);
                    string documentId = Convert.ToString(r["DocumentID"]);
                    string contactId = Convert.ToString(r["ContactID"]);
                    string database = Convert.ToString(r["IntDatabase"]);
                    string server = Convert.ToString(r["IntServer"]);
                    string systemId = Convert.ToString(r["IntegrationPartner"]);
                    foreach (DataRow row in dt.Rows)
                    {
                        if (String.IsNullOrEmpty(Convert.ToString(row["UserName"])) == false) 
                            users.Add(Convert.ToString(row["UserName"]) , Convert.ToUInt32(row["PermissionMask"]));
                        if (String.IsNullOrEmpty(Convert.ToString(row["GroupName"])) == false) 
                        groups.Add(Convert.ToString(row["GroupName"]) , Convert.ToUInt32(row["PermissionMask"]));
                    }

                    SetSecurity(server, database, systemId, clientId, fileId, contactId, documentId, users, groups);
                }
            }

        }

        public static string ServiceURL
        {
            get;
            set;
        }

        private static SystemAggregator GetService()
        {
            if (string.IsNullOrEmpty(ServiceURL))
                throw new NotSupportedException("ServiceURL has not been set");

            SystemAggregator service = new SystemAggregator();
            service.Url = ServiceURL;
            if (!service.UseDefaultCredentials)
                service.UseDefaultCredentials = true;
            return service;
        }



        public static CreateClientResponse CreateClient(string server, string database, string systemId, string clientId, string clientName)
        {
            if (string.IsNullOrEmpty("systemId"))
                throw new ArgumentNullException("systemId");

            ClientDetailsRequest client = PopulateStandardRequestDetails<ClientDetailsRequest>(server, database, systemId);
            client.ClientId = clientId;
            client.ClientName = clientName;

            using (SystemAggregator AggClient = GetService())
            {
                CreateClientResponse response = AggClient.CreateClient(client);
                
                return response;
            }
        }

        public static CreateFileResponse CreateFile(string server, string database, string systemId, string clientId, string fileId, string fileName)
        {
            if (string.IsNullOrEmpty("systemId"))
                throw new ArgumentNullException("systemId");

            FileDetailsRequest file = PopulateStandardRequestDetails<FileDetailsRequest>(server, database, systemId);
            file.ClientId = clientId;
            file.FileId = fileId;
            file.FileName = fileName;

            using (SystemAggregator AggClient = GetService())
            {
                CreateFileResponse response = AggClient.CreateFile(file);

                if (response.HasErrorSpecified && response.HasError)
                    throw new InvalidOperationException(response.Error);

                return response;
            }

        }

        public static ArchiveDocumentResponse ArchiveDocument(string server, string database, string systemId, int documentId)
        {
            if (string.IsNullOrEmpty("systemId"))
                throw new ArgumentNullException("systemId");

            ArchiveDocumentRequest request = PopulateStandardRequestDetails<ArchiveDocumentRequest>(server, database, systemId);
            request.DocumentId = documentId;
            request.DocumentIdSpecified = true;

            using (SystemAggregator AggClient = GetService())
            {
                var response = AggClient.ArchiveDocument(request);

                if (response.HasErrorSpecified && response.HasError)
                    throw new InvalidOperationException(response.Error);

                return response;
            }

        }

        public static ArchiveFileResponse ArchiveFile(string server, string database, string systemId,string clientId, string fileId)
        {
            if (string.IsNullOrEmpty("systemId"))
                throw new ArgumentNullException("systemId");

            ArchiveFileRequest request = PopulateStandardRequestDetails<ArchiveFileRequest>(server, database, systemId);
            request.FileId = fileId;
            request.ClientId = clientId;

            using (SystemAggregator AggClient = GetService())
            {
                var response = AggClient.ArchiveFile(request);

                if (response.HasErrorSpecified && response.HasError)
                    throw new InvalidOperationException(response.Error);

                return response;
            }

        }

        

        public static ArchiveDocumentResponse ArchiveDocumentForceRelease(string server, string database, string systemId, int documentId, string sharedKey, string userId)
        {
            if (string.IsNullOrEmpty("systemId"))
                throw new ArgumentNullException("systemId");

            ArchiveDocumentRequest request = PopulateStandardRequestDetails<ArchiveDocumentRequest>(server, database, systemId);
            request.DocumentId = documentId;
            request.DocumentIdSpecified = true;
            request.SecurityKey = sharedKey;
            request.CallingUser = userId;

            using (SystemAggregator AggClient = GetService())
            {
                var response = AggClient.ArchiveDocumentForceRelease(request);

                if (response.HasErrorSpecified && response.HasError)
                    throw new InvalidOperationException(response.Error);

                return response;
            }

        }

        public static ArchiveFileResponse ArchiveFileForceRelease(string server, string database, string systemId, string clientId, string fileId, string sharedKey, string userId)
        {
            return ArchiveFileForceRelease(server, database, systemId, clientId, fileId, sharedKey, userId, 6000); 
        }
        public static ArchiveFileResponse ArchiveFileForceRelease(string server, string database, string systemId, string clientId, string fileId, string sharedKey, string userId, int timeout)
        {
            if (string.IsNullOrEmpty("systemId"))
                throw new ArgumentNullException("systemId");

            ArchiveFileRequest request = PopulateStandardRequestDetails<ArchiveFileRequest>(server, database, systemId);
            request.FileId = fileId;
            request.ClientId = clientId;
            request.SecurityKey = sharedKey;
            request.CallingUser = userId;

            using (SystemAggregator AggClient = GetService())
            {
                AggClient.Timeout = timeout;
                var response = AggClient.ArchiveFileForceRelease(request);
                
                if (response.HasErrorSpecified && response.HasError)
                    throw new InvalidOperationException(response.Error);

                return response;
            }

        }


        public static SetSecurityResponse SetSecurity(string server, string database, string systemId, string clientId, string fileId, string contactId, string documentId, Dictionary<string, uint> userPermissions, Dictionary<string, uint> groupPermissions)
        {
            if (string.IsNullOrEmpty("systemId"))
                throw new ArgumentNullException("systemId");

            SetSecurityRequest sercurity = PopulateStandardRequestDetails<SetSecurityRequest>(server, database, systemId);
            sercurity.ClientId = clientId;
            sercurity.ContactId = contactId;
            sercurity.FileId = fileId;
            sercurity.DocumentId = documentId;

            sercurity.UserPermissions = FormatPermissions(userPermissions).ToArray();
            sercurity.GroupPermissions = FormatPermissions(groupPermissions).ToArray();

            using (SystemAggregator AggClient = GetService())
            {
                SetSecurityResponse response = AggClient.SetSecurity(sercurity);

                return response;
            }
        }

        public static ApplyTemplateResponse ApplyTemplate(string server, string database, string systemId, string clientId, string fileId, string templateName, string displayName, string description)
        {
            return ApplyTemplate(server, database, systemId, clientId, fileId, templateName, displayName, description, null);
        }

        public static ApplyTemplateResponse ApplyTemplate(string server, string database, string systemId, string clientId, string fileId, string templateName, string displayName, string description, Dictionary<string ,string> additionalParameters)
        {
            if (string.IsNullOrEmpty("systemId"))
                throw new ArgumentNullException("systemId");

            ApplyTemplateRequest templateRequest = PopulateStandardRequestDetails<ApplyTemplateRequest>(server, database, systemId);
            templateRequest.ClientId = clientId;
            templateRequest.Description = description;
            templateRequest.DisplayName = displayName;
            templateRequest.FileId = fileId;
            templateRequest.TemplateName = templateName;

            if (additionalParameters != null)
            {
                List<ArrayOfKeyValueOfstringstringKeyValueOfstringstring> props = new List<ArrayOfKeyValueOfstringstringKeyValueOfstringstring>();

                foreach (var property in additionalParameters)
                {
                    props.Add(new ArrayOfKeyValueOfstringstringKeyValueOfstringstring() { Key = property.Key, Value = property.Value });
                }

                templateRequest.ExtendedProperties = props.ToArray();
            }

            using (SystemAggregator AggClient = GetService())
            {
                ApplyTemplateResponse response = AggClient.ApplyTemplate(templateRequest);

                return response;
            }
        }

        private static List<ArrayOfKeyValueOfstringunsignedIntKeyValueOfstringunsignedInt> FormatPermissions(Dictionary<string, uint> setPermissions)
        {

            List<ArrayOfKeyValueOfstringunsignedIntKeyValueOfstringunsignedInt> permisisons = new List<ArrayOfKeyValueOfstringunsignedIntKeyValueOfstringunsignedInt>();
            if (setPermissions != null)
            {
                foreach (var permission in setPermissions)
                {
                    permisisons.Add(new ArrayOfKeyValueOfstringunsignedIntKeyValueOfstringunsignedInt() { Key = permission.Key, Value = permission.Value });
                }
            }
            return permisisons;
        }

        private static T PopulateStandardRequestDetails<T>(string server, string database, string system)
            where T : Request
        {
            var request = Activator.CreateInstance<T>();
            request.System = system;
            request.ServerName = server;
            request.Database = database;

            return request;
        }


    }
}
