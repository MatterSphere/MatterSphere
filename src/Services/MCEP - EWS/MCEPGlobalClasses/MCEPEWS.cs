using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Identity.Client;

namespace MCEPGlobalClasses
{
    class MCEPEWS : IDisposable
    {
        #region Private Global Variables
        private ExchangeService exchangeService;
        private DateTimeOffset? oAuthExpiresOn;
        // MatterSphere ExtendedPropertyDefinitions
        private ExtendedPropertyDefinition ExtendedPropertyCompanyID = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "COMPANYID", MapiPropertyType.Double);
        private ExtendedPropertyDefinition ExtendedPropertyClientID = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "CLIENTID", MapiPropertyType.Double);
        private ExtendedPropertyDefinition ExtendedPropertyFileID = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "FILEID", MapiPropertyType.Double);
        private ExtendedPropertyDefinition ExtendedPropertyDocID = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "DOCID", MapiPropertyType.Double);
        private ExtendedPropertyDefinition ExtendedPropertyAssocID = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "ASSOCID", MapiPropertyType.Double);
        private ExtendedPropertyDefinition ExtendedPropertySerialNo = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "SERIALNO", MapiPropertyType.Double);
        private ExtendedPropertyDefinition ExtendedPropertyVersionID = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "VERSIONID", MapiPropertyType.String);
        private ExtendedPropertyDefinition ExtendedPropertyVersionLabel = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "VERSIONLABEL", MapiPropertyType.String);
        private ExtendedPropertyDefinition ExtendedPropertyEdition = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "EDITION", MapiPropertyType.String);
        private ExtendedPropertyDefinition ExtendedPropertyBasePrecType = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "BASEPRECTYPE", MapiPropertyType.String);
        private ExtendedPropertyDefinition ExtendedPropertyDocIDEX = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "DOCIDEX", MapiPropertyType.String);
        private ExtendedPropertyDefinition ExtendedPropertyAppID = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "APPID", MapiPropertyType.Double);
        private ExtendedPropertyDefinition ExtendedPropertyAssocName = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "ASSOCNAME", MapiPropertyType.String);
        private ExtendedPropertyDefinition ExtendedPropertyBasePrecID = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "BASEPRECID", MapiPropertyType.Double);
        private ExtendedPropertyDefinition ExtendedPropertyClientName = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "CLNAME", MapiPropertyType.String);
        private ExtendedPropertyDefinition ExtendedPropertyFileDesc = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "FILEDESC", MapiPropertyType.String);
        private ExtendedPropertyDefinition ExtendedPropertyFileNo = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "FILENO", MapiPropertyType.String);
        private ExtendedPropertyDefinition ExtendedPropertyClientNo = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "CLNO", MapiPropertyType.String);
        private ExtendedPropertyDefinition[] MatterSphereFields;
        private PropertySet MatterSpherePropertiesAndBaseProperties;
        #endregion

        public MCEPEWS()
        {
            if (MCEPConfiguration.GetConfigurationItemBool("OverrideCertificateCheck"))
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(CheckCertificate);
            }
            SetExchangeService();
            MatterSphereFields = new ExtendedPropertyDefinition[] { ExtendedPropertyCompanyID, ExtendedPropertyClientID, ExtendedPropertyFileID, ExtendedPropertyDocID,
            ExtendedPropertyAssocID, ExtendedPropertySerialNo, ExtendedPropertyVersionID, ExtendedPropertyVersionLabel, 
            ExtendedPropertyEdition, ExtendedPropertyBasePrecType, ExtendedPropertyDocIDEX, ExtendedPropertyAppID, ExtendedPropertyAssocName,ExtendedPropertyBasePrecID,ExtendedPropertyClientName,
            ExtendedPropertyFileDesc,ExtendedPropertyFileNo, ExtendedPropertyClientNo};
            MatterSpherePropertiesAndBaseProperties = new PropertySet(BasePropertySet.FirstClassProperties, MatterSphereFields);
        }

        internal void Impersonate(string emailAddress)
        {
            if (oAuthExpiresOn.HasValue)
            {
                exchangeService.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, emailAddress);
                exchangeService.HttpHeaders["X-AnchorMailbox"] = emailAddress;
            }
        }

        private ExchangeService ExchangeService
        {
            get
            {
                if (oAuthExpiresOn.HasValue && oAuthExpiresOn.Value < DateTimeOffset.UtcNow)
                {
                    exchangeService.Credentials = new OAuthCredentials(GetOAuthToken());
                }
                return exchangeService;
            }
        }

        private void SetExchangeService()
        {
            if (exchangeService == null)
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
                ExchangeVersion version = LowestSupportedExchangeVersion();
                exchangeService = new ExchangeService(version,TimeZoneInfo.Local);
                exchangeService.UserAgent = "3E MatterSphere Exchange Profiler";
                string emailAddress = MCEPConfiguration.GetConfigurationItem("ServiceEmailAddress");

                if (MCEPConfiguration.GetConfigurationItemBool("UseOAuth"))
                {
                    exchangeService.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, emailAddress);
                    exchangeService.HttpHeaders.Add("X-AnchorMailbox", emailAddress);
                    exchangeService.EnableScpLookup = false;
                    exchangeService.Credentials = new OAuthCredentials(GetOAuthToken());
                }
                else if (MCEPConfiguration.GetConfigurationItemBool("OverrideExchangeCredentials"))
                {
                    if (string.IsNullOrWhiteSpace(MCEPConfiguration.GetConfigurationItem("ExcDomain")))
                        exchangeService.Credentials = new NetworkCredential(MCEPConfiguration.GetConfigurationItem("ExcUserName"), MCEPConfiguration.GetConfigurationItem("ExcPassword"));
                    else
                        exchangeService.Credentials = new NetworkCredential(MCEPConfiguration.GetConfigurationItem("ExcUserName"), MCEPConfiguration.GetConfigurationItem("ExcPassword"), MCEPConfiguration.GetConfigurationItem("ExcDomain"));
                }
                else
                {
                    exchangeService.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
                if (MCEPConfiguration.GetConfigurationItemBool("UseAutoDiscover"))
                {
                    exchangeService.AutodiscoverUrl(emailAddress);
                }
                else
                {
                    exchangeService.Url = new Uri(MCEPConfiguration.GetConfigurationItem("ExchangeWebServicesURL"));
                }
            }
        }

        private string GetOAuthToken()
        {
            var cca = ConfidentialClientApplicationBuilder
                .Create(MCEPConfiguration.GetConfigurationItem("OAuthAppId"))
                .WithClientSecret(MCEPConfiguration.GetConfigurationItem("OAuthClientSecret"))
                .WithTenantId(MCEPConfiguration.GetConfigurationItem("OAuthTenantId"))
                .Build();

            try
            {
                var ewsScopes = new string[] { "https://outlook.office365.com/.default" };
                var authResult = cca.AcquireTokenForClient(ewsScopes).ExecuteAsync().Result;
                oAuthExpiresOn = authResult.ExpiresOn.AddMinutes(-1);
                return authResult.AccessToken;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerException;
            }
        }

        private ExchangeVersion LowestSupportedExchangeVersion()
        {
            return ExchangeVersion.Exchange2010;
        }

        private string ConvertID(IdFormat inputFormat, IdFormat outputFormat, String MailBoxName, String orginalIDValue)
        {
            try
            {
                AlternateId alternateID = new AlternateId(inputFormat, orginalIDValue, MailBoxName);
                AlternateIdBase ewsResponse = ExchangeService.ConvertId(alternateID, outputFormat);
                AlternateId alternateIDOutput = (AlternateId)ewsResponse;
                return alternateIDOutput.UniqueId.ToString();
            }
            catch
            {
                return orginalIDValue;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            exchangeService = null;
            if (MCEPConfiguration.GetConfigurationItemBool("OverrideCertificateCheck"))
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback -= new RemoteCertificateValidationCallback(CheckCertificate);
            }
        }

        #endregion

        private bool CheckCertificate(Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (certificate.GetSerialNumberString() == MCEPConfiguration.GetConfigurationItem("CertificateSerialNumber"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal string GetRootFolderID(string userEmail, string RootFolderName)
        {
            FolderId userRootFolder = new FolderId(WellKnownFolderName.MsgFolderRoot, userEmail);
            FindFoldersResults findResults = ExchangeService.FindFolders(userRootFolder , new FolderView(int.MaxValue));
            foreach (Folder folder in findResults.Folders)
            {
                if (string.Equals(folder.DisplayName,RootFolderName, StringComparison.OrdinalIgnoreCase)) return folder.Id.ToString();
            }
            return null;
        }

        internal void FindMatterSphereMessages(string userEmail, string userFolderID, int userID, MCEPLogging mcepLog)
        {
            mcepLog.CreateLogEntry("Starting Folder Search for Email : " + userEmail);
            MCEPEWSFolder ewsFolder = new MCEPEWSFolder();
            ewsFolder.FolderID = userFolderID;
            ewsFolder.UserEmail = userEmail;
            ewsFolder.UserID = userID;
            CheckChildFolders(ewsFolder, mcepLog);
        }

        private void CheckChildFolders(MCEPEWSFolder ewsFolder, MCEPLogging mcepLog)
        {
            FolderId folderID = new FolderId(ewsFolder.FolderID);
            FindFoldersResults findResults = ExchangeService.FindFolders(folderID, new FolderView(int.MaxValue));
            foreach (Folder folder in findResults.Folders)
            {
                try
                {
                    mcepLog.CreateLogEntry("Checking Folder : " + folder.DisplayName);

                    MCEPEWSFolder ewsFolderChild = new MCEPEWSFolder();
                    ewsFolderChild.FolderID = folder.Id.UniqueId;
                    ewsFolderChild.UserID = ewsFolder.UserID;
                    ewsFolderChild.UserEmail = ewsFolder.UserEmail;
                    ewsFolderChild.FolderName = folder.DisplayName;
                    if (CheckFolderForMatterSphereProperties(ewsFolderChild, mcepLog))
                    {
                        mcepLog.CreateLogEntry("Folder : " + folder.DisplayName + " : Has MatterSphere Properties");
                        ProcessFolderForMailItems(ewsFolderChild, mcepLog);
                    }
                    if (folder.ChildFolderCount > 0) CheckChildFolders(ewsFolderChild, mcepLog);
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Error Occurred Processing Folder : " + folder.DisplayName);
                    sb.AppendLine("FolderID : " + folder.Id.UniqueId);
                    sb.AppendLine(ex.Message);
                    if (ex.InnerException != null)
                        sb.AppendLine("Inner Exception").AppendLine(ex.InnerException.Message);
                    mcepLog.CreateErrorEntry(sb.ToString());
                }
            }
        }

        private void ProcessFolderForMailItems(MCEPEWSFolder ewsFolder, MCEPLogging mcepLog)
        {
            mcepLog.CreateLogEntry("Checking Folder : " + ewsFolder.FolderName + " For Non Profiled Items");
            ItemView itemview = new ItemView(int.MaxValue);
            itemview.PropertySet = MatterSpherePropertiesAndBaseProperties;
            itemview.Traversal = ItemTraversal.Shallow;
            List<SearchFilter> searchFilterCollection = new List<SearchFilter>();
            searchFilterCollection.Add(new SearchFilter.Not(new SearchFilter.Exists(ExtendedPropertyCompanyID)));
            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());
            FindItemsResults<Item> findResults = ExchangeService.FindItems(ewsFolder.FolderID, searchFilter, itemview);
            mcepLog.CreateLogEntry("Message Not Profiled Found: " + Convert.ToString(findResults.TotalCount));
            foreach (EmailMessage message in findResults.Items)
            {
                try
                {
                    MCEPDatabase MCEPData = new MCEPDatabase();
                    MCEPProfilerItem mailItem = new MCEPProfilerItem();
                    mailItem.UserEmail = ewsFolder.UserEmail;
                    mailItem.UserID = ewsFolder.UserID;
                    mailItem.FolderID = ewsFolder.FolderID;
                    mailItem.FileID = ewsFolder.FileID;
                    mailItem.Processed = false;
                    mailItem.ItemCreated = DateTime.UtcNow;
                    mailItem.ItemUpdated = DateTime.UtcNow;
                    mailItem.MessageID = message.Id.UniqueId.ToString();

                    if (MCEPData.CreateQueueItem(mailItem))
                    {
                        mcepLog.CreateLogEntry("Message Saved to Queue : Subject : " + message.Subject);
                    }
                    else
                    {
                        mcepLog.CreateLogEntry("Message Not Saved to Queue : Subject : " + message.Subject);
                    }
                }
                catch (Exception ex)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Error Occurred Processing Email Message : " + message.Subject);
                    sb.AppendLine("MessageID : " + message.Id.UniqueId);
                    sb.AppendLine("FolderID : " + message.ParentFolderId.UniqueId);
                    sb.AppendLine(ex.Message);
                    if (ex.InnerException != null)
                        sb.AppendLine("Inner Exception").AppendLine(ex.InnerException.Message);
                    mcepLog.CreateErrorEntry(sb.ToString());
                }
            }

        }

        private bool CheckFolderForMatterSphereProperties(MCEPEWSFolder ewsFolder, MCEPLogging mcepLog)
        {
            object fileID;
            object clientID;
            object companyID;
            object serialNo;
            ItemView itemview = new ItemView(int.MaxValue);
            itemview.PropertySet = MatterSpherePropertiesAndBaseProperties;
            itemview.Traversal = ItemTraversal.Associated;
            List<SearchFilter> searchFilterCollection = new List<SearchFilter>();
            searchFilterCollection.Add(new SearchFilter.ContainsSubstring(ItemSchema.Subject, @"{297A22C6-6245-4c0f-B50F-56998081C2E3}"));
            SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And,searchFilterCollection.ToArray());
            FindItemsResults<Item> findResults = ExchangeService.FindItems(ewsFolder.FolderID, searchFilter, itemview);
            foreach (EmailMessage message in findResults.Items)
            {
                if (message.TryGetProperty(ExtendedPropertyCompanyID, out companyID))
                {
                    if (Convert.ToDouble(companyID) == Convert.ToDouble(MCEPConfiguration.GetConfigurationItem("CompanyID")))
                    {
                    message.TryGetProperty(ExtendedPropertyFileID, out fileID);
                    message.TryGetProperty(ExtendedPropertyClientID, out clientID);
                    message.TryGetProperty(ExtendedPropertySerialNo, out serialNo);
                    ewsFolder.ClientID = Convert.ToDouble(clientID);
                    ewsFolder.FileID = Convert.ToDouble(fileID);
                    ewsFolder.CompanyID = Convert.ToDouble(companyID);
                    ewsFolder.SerialNo = Convert.ToDouble(serialNo);
                    return true;
                    }
                }
            }
            return false;
        }



        internal EmailMessage GetEWSMailItem(string messsageID)
        {
            return EmailMessage.Bind(ExchangeService, messsageID);
        }

        internal System.IO.MemoryStream GetEMLFileStream(EmailMessage message)
        {
            PropertySet props = new PropertySet(BasePropertySet.FirstClassProperties, MatterSphereFields);
            props.Add(ItemSchema.MimeContent);
            message.Load(props);
            MimeContent mc = message.MimeContent;
            System.IO.MemoryStream fs = new System.IO.MemoryStream();
            fs.Write(mc.Content, 0, mc.Content.Length);
            fs.Position = 0;
            return fs;
        }

        internal List<FWBS.OMS.SubDocument> ExtractAttachments(EmailMessage message, HashSet<string> excludeExtensions, long maxFileSize)
        {
            message.Load(MatterSpherePropertiesAndBaseProperties);
            List<FWBS.OMS.SubDocument> subDocs = new List<FWBS.OMS.SubDocument>();
            foreach (Attachment attach in message.Attachments)
            {
                if (attach is FileAttachment)
                {
                    FileAttachment fileAttach = attach as FileAttachment;
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    path = System.IO.Path.Combine(path, "Elite", "MCEP", "Temp");
                    if (!System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.CreateDirectory(path);
                    }
                    path = System.IO.Path.Combine(path, DateTime.Now.Ticks.ToString() + fileAttach.Name);
                    using (System.IO.FileStream afs = new System.IO.FileStream(path, System.IO.FileMode.Create))
                    {
                        fileAttach.Load(afs);
                    }
                    var fileInfo = new System.IO.FileInfo(path);
                    FWBS.OMS.SubDocument subDoc = new FWBS.OMS.SubDocument(fileAttach.Name, fileInfo);
                    subDoc.Store = !(excludeExtensions.Contains(fileInfo.Extension.TrimStart('.')) && fileInfo.Length <= maxFileSize);
                    subDocs.Add(subDoc);
                }             
            }
            return subDocs;
        }

        internal MCEPEmailBasicProps GetBasicMailProperties(EmailMessage message)
        {
            MCEPLogging mcepLog = new MCEPLogging("MailMessageLog");
            mcepLog.CreateLogEntry("ID: " + message.Id.ToString());
            mcepLog.CreateLogEntry("Before Load Base Properties");
            message.Load(MatterSpherePropertiesAndBaseProperties);
            mcepLog.CreateLogEntry("Before New 'Email'");
            MCEPEmailBasicProps email = new MCEPEmailBasicProps();
            mcepLog.CreateLogEntry("Before To");
            StringBuilder sbTo = new StringBuilder();
            foreach (EmailAddress address in message.ToRecipients )
            {
                sbTo.Append(address.Address);
                sbTo.Append(";");
            }
            email.EmailTo = sbTo.ToString();
            mcepLog.CreateLogEntry("Before CC");
            StringBuilder sbCC = new StringBuilder();
            foreach (EmailAddress address in message.CcRecipients)
            {
                sbCC.Append(address.Address);
                sbCC.Append(";");
            }
            email.EmailCC = sbCC.ToString();
            mcepLog.CreateLogEntry("Before From");
            try
            {
                email.EmailFrom = message.From.Address;
            }
            catch
            {
                email.EmailFrom = string.Empty;
            }
            mcepLog.CreateLogEntry("Before Date Time Created");
            email.EmailCreated = message.DateTimeCreated;
            mcepLog.CreateLogEntry("Before Date Time Recieved");
            try
            {
                email.EmailRecieved = message.DateTimeReceived;
            }
            catch
            {
                email.EmailRecieved = email.EmailCreated;
            }
            mcepLog.CreateLogEntry("Before Date Time Sent");
            try
            {
                email.EmailSent = message.DateTimeSent;
            }
            catch
            {
                email.EmailSent = email.EmailCreated;
            }
            mcepLog.CreateLogEntry("Before Subject");
            email.Subject = message.Subject;
            mcepLog.CreateLogEntry("Done");
            return email;
        }

        internal void SetNamedPropertiesOnMessage(EmailMessage message, FWBS.OMS.OMSDocument doc)
        {
            message.Load(MatterSpherePropertiesAndBaseProperties);
            message.SetExtendedProperty(ExtendedPropertyAssocID, doc.Associate.ID);
            message.SetExtendedProperty(ExtendedPropertyBasePrecID, doc.BasePrecedent.ID);
            message.SetExtendedProperty(ExtendedPropertyBasePrecType, "EMAIL");
            message.SetExtendedProperty(ExtendedPropertyClientID, doc.OMSFile.ClientID);
            message.SetExtendedProperty(ExtendedPropertyClientName, doc.OMSFile.Client.ClientName);
            message.SetExtendedProperty(ExtendedPropertyClientNo, doc.OMSFile.Client.ClientNo);
            message.SetExtendedProperty(ExtendedPropertyCompanyID,FWBS.OMS.Session.CurrentSession.CompanyID);
            message.SetExtendedProperty(ExtendedPropertyDocID, doc.ID);
            message.SetExtendedProperty(ExtendedPropertyEdition, FWBS.OMS.Session.CurrentSession.Edition);
            message.SetExtendedProperty(ExtendedPropertyFileDesc, doc.OMSFile.FileDescription);
            message.SetExtendedProperty(ExtendedPropertyFileID, doc.OMSFileID);
            message.SetExtendedProperty(ExtendedPropertyFileNo, doc.OMSFile.FileNo);
            message.SetExtendedProperty(ExtendedPropertySerialNo, FWBS.OMS.Session.CurrentSession.SerialNumber);
            message.SetExtendedProperty(ExtendedPropertyVersionID, doc.GetLatestVersion().Id.ToString());
            message.SetExtendedProperty(ExtendedPropertyVersionLabel, doc.GetLatestVersion().Label);
            message.Update(ConflictResolutionMode.AlwaysOverwrite);

        }

        internal string CheckForExistingDOCID(EmailMessage message)
        {
            string returnvalue = string.Empty;
            message.Load(MatterSpherePropertiesAndBaseProperties);
            object docID;
            if (message.TryGetProperty(ExtendedPropertyDocID, out docID))
            {
                returnvalue = docID.ToString();
            }
            return returnvalue;
        }

        internal string GetAllInformation(EmailMessage message)
        {
            message.Load(MatterSpherePropertiesAndBaseProperties);
            StringBuilder stringToReturn = new StringBuilder();
            MCEPEmailBasicProps mailProp = GetBasicMailProperties(message);
            stringToReturn.AppendLine("From: " + mailProp.EmailFrom);
            stringToReturn.AppendLine("To: " + mailProp.EmailTo);
            stringToReturn.AppendLine("CC: " + mailProp.EmailCC);
            stringToReturn.AppendLine("Subject: " + mailProp.Subject);
            Folder folder = Folder.Bind(ExchangeService, message.ParentFolderId);
            stringToReturn.AppendLine("Folder Name: " + folder.DisplayName);
            int count = 1;
            while (folder.DisplayName != "Top of Information Store")
            {
                folder = Folder.Bind(ExchangeService, folder.ParentFolderId);
                stringToReturn.AppendLine("Parent Folder" + count.ToString() + ": " + folder.DisplayName);
                count++;
            }
            object companyID, ClientID, FileID, DocID, AssocID, SerialNo, VersionID;
            if (message.TryGetProperty(ExtendedPropertyCompanyID, out companyID))
            {
                stringToReturn.AppendLine("CompanyID: " + companyID.ToString());
            }
            if (message.TryGetProperty(ExtendedPropertyClientID, out ClientID))
            {
                stringToReturn.AppendLine("ClientID: " + ClientID.ToString());
            }
            if (message.TryGetProperty(ExtendedPropertyFileID, out FileID))
            {
                stringToReturn.AppendLine("FileID: " + FileID.ToString());
            }
            if (message.TryGetProperty(ExtendedPropertyDocID, out DocID))
            {
                stringToReturn.AppendLine("DocID: " + DocID.ToString());
            }
            if (message.TryGetProperty(ExtendedPropertyAssocID, out AssocID))
            {
                stringToReturn.AppendLine("AssocID: " + AssocID.ToString());
            }
            if (message.TryGetProperty(ExtendedPropertySerialNo, out SerialNo))
            {
                stringToReturn.AppendLine("SerialNo: " + SerialNo.ToString());
            }
            if (message.TryGetProperty(ExtendedPropertyVersionID, out VersionID))
            {
                stringToReturn.AppendLine("VersionID: " + VersionID.ToString());
            }
            return stringToReturn.ToString();
        }
    }
}
