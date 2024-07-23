using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.Identity.Client;

namespace MatterSphereEWS
{
    class EWS : IDisposable
    {

        #region Private Global Variables
        private ExchangeService exchangeService;
        private DateTimeOffset? oAuthExpiresOn;
        private string serviceEmailAddress;
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
        private ExtendedPropertyDefinition ExtendedPropertyEWSItemID = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.PublicStrings, "EWSItemID", MapiPropertyType.String);
        private ExtendedPropertyDefinition[] MatterSphereFields;
        private PropertySet MSEWSProperties;

        private Logging Log;
        #endregion

        #region BaseMethods
        public EWS(Logging log)
        {
            Log = log;
            if (Config.GetConfigurationItemBool("OverrideCertificateCheck"))
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(CheckCertificate);
            }
            SetExchangeService();
            MatterSphereFields = new ExtendedPropertyDefinition[] { ExtendedPropertyCompanyID, ExtendedPropertyClientID, ExtendedPropertyFileID, ExtendedPropertyDocID,
            ExtendedPropertyAssocID, ExtendedPropertySerialNo, ExtendedPropertyVersionID, ExtendedPropertyVersionLabel, 
            ExtendedPropertyEdition, ExtendedPropertyBasePrecType, ExtendedPropertyDocIDEX, ExtendedPropertyAppID, ExtendedPropertyAssocName,ExtendedPropertyBasePrecID,ExtendedPropertyClientName,
            ExtendedPropertyFileDesc,ExtendedPropertyFileNo, ExtendedPropertyClientNo, ExtendedPropertyEWSItemID};
            MSEWSProperties = new PropertySet(BasePropertySet.FirstClassProperties, MatterSphereFields) { RequestedBodyType = BodyType.Text };
            Log.CreateLogEntry("EWS Class Reference Setup");
        }

        private void Impersonate(string emailAddress)
        {
            if (oAuthExpiresOn.HasValue)
            {
                exchangeService.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, emailAddress);
                exchangeService.HttpHeaders["X-AnchorMailbox"] = emailAddress;
            }
        }

        private ExchangeVersion LowestSupportedExchangeVersion()
        {
            return ExchangeVersion.Exchange2007_SP1;
        }

        private bool CheckCertificate(Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (certificate.GetSerialNumberString() == Config.GetConfigurationItem("CertificateSerialNumber"))
            {
                return true;
            }
            else
            {
                return false;
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
                exchangeService = new ExchangeService(version,TimeZoneInfo.Utc);
                exchangeService.UserAgent = "3E MatterSphere Exchange Sync";
                serviceEmailAddress = Config.GetConfigurationItem("ServiceEmailAddress");

                if (Config.GetConfigurationItemBool("UseOAuth"))
                {
                    exchangeService.ImpersonatedUserId = new ImpersonatedUserId(ConnectingIdType.SmtpAddress, serviceEmailAddress);
                    exchangeService.HttpHeaders.Add("X-AnchorMailbox", serviceEmailAddress);
                    exchangeService.EnableScpLookup = false;
                    exchangeService.Credentials = new OAuthCredentials(GetOAuthToken());
                }
                else if (Config.GetConfigurationItemBool("OverrideExchangeCredentials"))
                {
                    if (string.IsNullOrWhiteSpace(Config.GetConfigurationItem("ExcDomain")))
                        exchangeService.Credentials = new NetworkCredential(Config.GetConfigurationItem("ExcUserName"), Config.GetConfigurationItem("ExcPassword"));
                    else
                        exchangeService.Credentials = new NetworkCredential(Config.GetConfigurationItem("ExcUserName"), Config.GetConfigurationItem("ExcPassword"), Config.GetConfigurationItem("ExcDomain"));
                }
                else
                {
                    exchangeService.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
                if (Config.GetConfigurationItemBool("UseAutoDiscover"))
                {
                    exchangeService.AutodiscoverUrl(serviceEmailAddress);
                }
                else
                {
                    exchangeService.Url = new Uri(Config.GetConfigurationItem("ExchangeWebServicesURL"));
                }
            }
        }

        private string GetOAuthToken()
        {
            var cca = ConfidentialClientApplicationBuilder
                .Create(Config.GetConfigurationItem("OAuthAppId"))
                .WithClientSecret(Config.GetConfigurationItem("OAuthClientSecret"))
                .WithTenantId(Config.GetConfigurationItem("OAuthTenantId"))
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

        private FolderId FindSpecifiedFolder(string fldName, string userEmail, string defaultFolderType)
        {
            FolderId userRootFolder = new FolderId(WellKnownFolderName.MsgFolderRoot, userEmail);
            FindFoldersResults findResults = ExchangeService.FindFolders(userRootFolder, new FolderView(int.MaxValue));
            foreach (Folder folder in findResults.Folders)
            {
                if (string.Equals(folder.DisplayName, fldName, StringComparison.OrdinalIgnoreCase)) return folder.Id.ToString();
            }
            if (defaultFolderType == "C") { return new FolderId(WellKnownFolderName.Calendar, userEmail); }
            else if (defaultFolderType == "T") { return new FolderId(WellKnownFolderName.Tasks, userEmail); }
            return null;
        }

        private FolderId GetAppointmentFolder(String MailBoxName)
        {
            return new FolderId(WellKnownFolderName.Calendar, MailBoxName);
        }

        public void Dispose()
        {
            if (Log != null) Log = null;
            exchangeService = null;
            if (Config.GetConfigurationItemBool("OverrideCertificateCheck"))
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback -= new RemoteCertificateValidationCallback(CheckCertificate);
            }
        }
        #endregion

        internal void ProcessNewAppointment(AppointmentItem appItem, UserSettings usrSettings, MatterSphereSettings msSettings)
        {
            Impersonate(usrSettings.MailBoxAddress);

            if (appItem.iExternal)
            {
                Log.CreateLogEntry("Item created from Outlook Item, Search for Item. AppID: " + appItem.ItemID.ToString());
                //Appointment Create in Outlook. Find Appointment by APPID if possible.
                FindAppoinment(appItem, usrSettings, msSettings);
            }
            else
            {
                Log.CreateLogEntry("New Appointment. AppID:" + appItem.ItemID.ToString());
                CreateNewAppointment(appItem, usrSettings, msSettings);
                //Appointment Generated in MatterSphere
            }
            if (appItem.iEWSID != null && appItem.iEntryID == null)
            {
                Log.CreateLogEntry("Get EntryID for Backwards Compatibility. AppID: " + appItem.ItemID.ToString());
                appItem.iEntryID = ConvertID(IdFormat.EwsId, IdFormat.HexEntryId, usrSettings.MailBoxAddress, appItem.iEWSID);
            }

        }

        internal void CheckItemExists(AppointmentItem appItem, UserSettings usrSettings, MatterSphereSettings msSettings)
        {
            Appointment appointment = null;
            try
            {
                try
                {
                    Impersonate(usrSettings.MailBoxAddress);
                    appointment = Appointment.Bind(ExchangeService, appItem.iEWSID, MSEWSProperties);
                }
                catch (ServiceResponseException)
                {
                    if (!oAuthExpiresOn.HasValue)
                        throw;
                    // Retry using the service account with delegation in case the appointment has been moved to another fee earner.
                    Impersonate(serviceEmailAddress);
                    appointment = Appointment.Bind(ExchangeService, appItem.iEWSID, MSEWSProperties);
                }
            }
            catch (ServiceResponseException appEx)
            {
                if (appEx.ErrorCode == ServiceError.ErrorItemNotFound)
                {
                    try
                    {
                        Impersonate(usrSettings.MailBoxAddress);
                        string tempEWSID = ConvertID(IdFormat.HexEntryId, IdFormat.EwsId, usrSettings.MailBoxAddress, appItem.iEntryID);
                        if (!String.IsNullOrEmpty(tempEWSID))
                        {
                            appointment = Appointment.Bind(ExchangeService, tempEWSID, MSEWSProperties);
                        }
                        else
                        {
                            appItem.iActive = false;
                            appItem.iEWSID = null;
                            appItem.iEntryID = null;
                        }
                    }
                    catch (ServiceResponseException appEx2)
                    {
                        if (appEx2.ErrorCode == ServiceError.ErrorItemNotFound)
                        {
                            appItem.iActive = false;
                            appItem.iEWSID = null;
                            appItem.iEntryID = null;
                        }
                    }
                  
                }

            }
        }

        internal void ProcessDeletedAppointment(AppointmentItem appItem, UserSettings usrSettings, MatterSphereSettings msSettings)
        {
            Appointment appointment = null;
            try
            {
                try
                {
                    Impersonate(usrSettings.MailBoxAddress);
                    appointment = Appointment.Bind(ExchangeService, appItem.iEWSID, MSEWSProperties);
                }
                catch (ServiceResponseException)
                {
                    if (!oAuthExpiresOn.HasValue)
                        throw;
                    // Retry using the service account with delegation in case the appointment has been moved to another fee earner and then deleted.
                    Impersonate(serviceEmailAddress);
                    appointment = Appointment.Bind(ExchangeService, appItem.iEWSID, MSEWSProperties);
                }
                appointment.Delete(DeleteMode.SoftDelete);
                appItem.iEWSID = null;
                appItem.iEntryID = null;
            }
            catch (ServiceResponseException appEx)
            {
                if (appEx.ErrorCode == ServiceError.ErrorItemNotFound)
                {
                    try
                    {
                        Impersonate(usrSettings.MailBoxAddress);
                        //Check EntryID
                        string tempEWSID = ConvertID(IdFormat.HexEntryId, IdFormat.EwsId, usrSettings.MailBoxAddress, appItem.iEntryID);
                        if (!String.IsNullOrEmpty(tempEWSID))
                        {
                            appointment = Appointment.Bind(ExchangeService, tempEWSID, MSEWSProperties);
                            appointment.Delete(DeleteMode.SoftDelete);
                        }
                        appItem.iEWSID = null;
                        appItem.iEntryID = null;
                    }
                    catch (ServiceResponseException appEx2)
                    {
                        if (appEx2.ErrorCode == ServiceError.ErrorItemNotFound || appEx2.ErrorCode == ServiceError.ErrorNoDestinationCASDueToVersionMismatch)
                        {
                            //Not found by Entry ID or EWSID or Item in Wrong OWA Versoin so mark as deleted.
                            appItem.iEWSID = null;
                            appItem.iEntryID = null;
                        }
                    }
                }
            }
        }

        internal void UpdateAppointmentBoth(AppointmentItem appItem, UserSettings usrSettings, MatterSphereSettings msSettings)
        {
            Impersonate(usrSettings.MailBoxAddress);

            Appointment appointment = FindAndAttachToEWSAppointment(appItem, usrSettings, msSettings);
            if (appointment != null)
            {
                if (appItem.iUpdated < appointment.LastModifiedTime)
                {
                    Log.CreateLogEntry("Appointment latest in EWS. Performing Update. AppID: " + appItem.ItemID.ToString());
                    UpdateAppointmentFromEWStoMatterSphere(appointment, appItem, usrSettings);
                }
                else
                {
                    // Added to avoid issue with Cached Mode Exchange when appointment is Saved using MatterSphere Save Button.
                    // Time delay to give Outlook time to update Exchange.
                    if (appItem.iUpdated < DateTime.UtcNow.AddSeconds(-Convert.ToDouble(Config.GetConfigurationItem("MatterSphereUpdateIgnore"))))
                    {
                        Log.CreateLogEntry("Appointment latest in MatterSphere. Performing Update. AppID: " + appItem.ItemID.ToString());
                        UpdateAppointmentFromMatterSphereToEWS(appointment, appItem, usrSettings);
                    }
                }
            }
        }

        private Appointment FindAndAttachToEWSAppointment(AppointmentItem appItem, UserSettings usrSettings, MatterSphereSettings msSettings, bool needServiceAccount = false)
        {
            Appointment appointment = null;
            try
            {
                if (appItem.iEWSID != null)
                {
                    try
                    {
                        if (needServiceAccount)
                            Impersonate(serviceEmailAddress);

                        appointment = Appointment.Bind(ExchangeService, appItem.iEWSID, MSEWSProperties);
                    }
                    finally
                    {
                        if (needServiceAccount)
                            Impersonate(usrSettings.MailBoxAddress);
                    }
                }
                else
                {
                    FindAppoinment(appItem, usrSettings, msSettings);
                    appointment = Appointment.Bind(ExchangeService, appItem.iEWSID, MSEWSProperties);
                }
                if (Folder.Bind(ExchangeService, GetAppointmentFolder(usrSettings.MailBoxAddress)).Id.ToString() != appointment.ParentFolderId.ToString())
                {
                    DeleteAndRecreateInCorrectFolder(appItem, usrSettings, msSettings, appointment);
                    appointment = null;
                }
            }
            catch (ServiceResponseException appEx)
            {
                appointment = FindMissingAppointment(appItem, usrSettings, appointment, appEx);
            }
            return appointment;
        }


        internal void UpdateAppointmentEWStoMS(AppointmentItem appItem, UserSettings usrSettings, MatterSphereSettings msSettings)
        {
            Impersonate(usrSettings.MailBoxAddress);

            Appointment appointment = FindAndAttachToEWSAppointment(appItem, usrSettings, msSettings);
            if (appointment != null)
            {
                UpdateAppointmentFromEWStoMatterSphere(appointment, appItem, usrSettings);
            }
        }

        internal void UpdateAppointmentMStoEWS(AppointmentItem appItem, UserSettings usrSettings, MatterSphereSettings msSettings)
        {
            Impersonate(usrSettings.MailBoxAddress);

            Appointment appointment = FindAndAttachToEWSAppointment(appItem, usrSettings, msSettings, true);
            if (appointment != null)
            {
                UpdateAppointmentFromMatterSphereToEWS(appointment, appItem, usrSettings);
            }
        }

        private void DeleteAndRecreateInCorrectFolder(AppointmentItem appItem, UserSettings usrSettings, MatterSphereSettings msSettings, Appointment appointment)
        {
            //Appoinment has been moved within MatterSphere. Delete and ReCreate
            try
            {
                Impersonate(serviceEmailAddress);
                appointment.Delete(DeleteMode.SoftDelete);
            }
            catch
            {
                //Unable to delete mark as inactive.
                appItem.iActive = false;
            }
            finally
            {
                Impersonate(usrSettings.MailBoxAddress);
                appItem.iEntryID = null;
                appItem.iEWSID = null;
            }

            CreateNewAppointment(appItem, usrSettings, msSettings);
            if (appItem.iEWSID != null && appItem.iEntryID == null)
            {
                Log.CreateLogEntry("Get EntryID for Backwards Compatibility. AppID: " + appItem.ItemID.ToString());
                appItem.iEntryID = ConvertID(IdFormat.EwsId, IdFormat.HexEntryId, usrSettings.MailBoxAddress, appItem.iEWSID);
            }
        }

        private Appointment FindMissingAppointment(AppointmentItem appItem, UserSettings usrSettings, Appointment appointment, ServiceResponseException appEx)
        {
            if (appEx.ErrorCode == ServiceError.ErrorItemNotFound)
            {
                Log.CreateLogEntry("Appointment Not Found from EWSID. Trying EntryID. AppID: " + appItem.ItemID.ToString());
                //Appointment is not found using EWSID Try using EntryID
                try
                {
                    string tempEWSID = ConvertID(IdFormat.HexEntryId, IdFormat.EwsId, usrSettings.MailBoxAddress, appItem.iEntryID);
                    appointment = Appointment.Bind(ExchangeService, tempEWSID, MSEWSProperties);
                    Log.CreateLogEntry("Appointment Not Found from EWSID. Found from EntryID. AppID: " + appItem.ItemID.ToString());
                    appItem.iEWSID = tempEWSID;
                }
                catch (ServiceResponseException appExi)
                {
                    if (appExi.ErrorCode == ServiceError.ErrorItemNotFound)
                    {
                        Log.CreateLogEntry("Appointment Not Found from EWSID. or from EntryID. Searching via Appointment ID. AppID: " + appItem.ItemID.ToString());
                        // Appointment Not Found from EntryID. Look via AppID
                        FindItemsResults<Item> appResults = FindAppointmentFromAppointmentID(appItem, usrSettings);
                        if (appResults.Items.Count == 1)
                        {
                            Log.CreateLogEntry("Appointment Found from AppointmentID. AppID: " + appItem.ItemID.ToString());
                            appItem.iEWSID = appResults.Items[0].Id.ToString();
                            appItem.iEntryID = ConvertID(IdFormat.EwsId, IdFormat.HexEntryId, usrSettings.MailBoxAddress, appItem.iEWSID);
                            appointment = Appointment.Bind(ExchangeService, appItem.iEWSID, MSEWSProperties);
                        }
                        else
                        {
                            //Appointment is missing completely. Mark InActive.
                            Log.CreateLogEntry("Appointment Not Found by above method. Marking as Inactive. AppID: " + appItem.ItemID.ToString());
                            appItem.iActive = false;
                        }
                    }
                }
            }
            return appointment;
        }

        private void UpdateAppointmentFromMatterSphereToEWS(Appointment appointment, AppointmentItem appItem, UserSettings usrSettings)
        {
            
            bool appUpdated = false;
            if (appointment.Subject != appItem.iSubject) 
            { 
                appointment.Subject = appItem.iSubject;
                appUpdated = true;
            }
            if (appItem.iLocation != null)
            {
                if (appointment.Location != appItem.iLocation) 
                { 
                    appointment.Location = appItem.iLocation;
                    appUpdated = true;
                }   
            }
            if (appointment.Body.Text != appItem.iBody)
            {
                if (!string.IsNullOrWhiteSpace(appointment.Body.Text) || !string.IsNullOrWhiteSpace(appItem.iBody))
                {
                    appointment.Body = appItem.iBody;
                    appUpdated = true;
                }
            }
            TimeZoneInfo timeZone;
            try
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(appItem.iTimeZone);
            }
            catch
            {
                timeZone = usrSettings.UserTimeZone;
            }
            if (appItem.iAllDayApp)
            {

                DateTime startDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iStartDate, timeZone);
                DateTime endDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iEndDate, timeZone);
                endDate = endDate.AddDays(1); //adding one day for MatterCentre
                DateTime EWSstartDate = TimeZoneInfo.ConvertTimeFromUtc(appointment.Start, timeZone);
                DateTime EWSendDate = TimeZoneInfo.ConvertTimeFromUtc(appointment.End, timeZone);

                if (appointment.IsAllDayEvent != appItem.iAllDayApp)
                {
                    appointment.IsAllDayEvent = appItem.iAllDayApp;
                    appUpdated = true; 
                }
                if (EWSstartDate != startDate)
                {
                    appointment.Start = startDate;
                    appointment.StartTimeZone = timeZone;
                    appUpdated = true;
                }
                if (EWSendDate != endDate)
                {
                    appointment.End = endDate;
                    appointment.StartTimeZone = timeZone;
                    appUpdated = true;
                }
            }
            else
            {
                if (appointment.IsAllDayEvent)
                {
                    appointment.IsAllDayEvent = false;
                }
                if (appointment.Start != (DateTime)appItem.iStartDate) 
                {
                    DateTime startDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iStartDate, timeZone);

                    appointment.Start = startDate;
                    appointment.StartTimeZone = timeZone;
                    appUpdated = true;
                }
                if (appointment.End != (DateTime)appItem.iEndDate) 
                {
                    DateTime endDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iEndDate, timeZone);

                    appointment.End = endDate;
                    appointment.StartTimeZone = timeZone;
                    appUpdated = true;
                }
            }
            if (appItem.iReminderActive)
            {
                if (!appointment.IsReminderSet) 
                { 
                    appointment.IsReminderSet = true;
                    appUpdated = true;
                }
                if (appointment.ReminderMinutesBeforeStart != appItem.iReminderMinutes)
                {
                    DateTime dte = appointment.Start;
                    appointment.ReminderDueBy = dte.AddMinutes(-(Int32)appItem.iReminderMinutes);
                    appointment.ReminderMinutesBeforeStart = ((Int32)appItem.iReminderMinutes);
                    appUpdated = true;
                }
            }
            else
            {
                appointment.IsReminderSet = false;
            }
            if (Config.GetConfigurationItemBool("SetAppointmentTypeAsCategory") && !appointment.Categories.Contains(appItem.iCategory))
            {
                appointment.Categories.Clear();
                appointment.Categories.Add(appItem.iCategory);
                appUpdated = true;
            }
            if (appUpdated)
            {
                Log.CreateLogEntry("Appointment Object Updated. Pre EWS Update. AppID: " + appItem.ItemID.ToString());
                appointment.Update(ConflictResolutionMode.AutoResolve, SendInvitationsOrCancellationsMode.SendToNone);
                Log.CreateLogEntry("Appointment Object Updated. Post EWS Update. AppID: " + appItem.ItemID.ToString());
            }

        }

        private void UpdateAppointmentFromEWStoMatterSphere(Appointment appointment, AppointmentItem appItem, UserSettings usrSettings)
        {
            if (appItem.iSubject != appointment.Subject)
            {
                appItem.iSubject = appointment.Subject;
            }

            if (appItem.iLocation != appointment.Location)
            {
                appItem.iLocation = appointment.Location;
            }

            if (appItem.iBody != appointment.Body.Text)
            {
                if (!string.IsNullOrWhiteSpace(appItem.iBody) || !string.IsNullOrWhiteSpace(appointment.Body.Text))
                    appItem.iBody = appointment.Body.Text;
            }
            if (appointment.IsAllDayEvent)
            {
                TimeZoneInfo timeZone, timeZoneEws;
                try
                {
                    timeZone = TimeZoneInfo.FindSystemTimeZoneById(appItem.iTimeZone);
                }
                catch
                {
                    timeZone = usrSettings.UserTimeZone;
                }
                try
                {
                    timeZoneEws = TimeZoneInfo.GetSystemTimeZones().First(tzi => tzi.DisplayName == appointment.TimeZone);
                }
                catch
                {
                    timeZoneEws = timeZone;
                }
                DateTime startDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iStartDate, timeZone);
                DateTime endDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iEndDate, timeZone);
                endDate = endDate.AddDays(1); //adding one day for MatterCentre
                DateTime EWSstartDate = TimeZoneInfo.ConvertTimeFromUtc(appointment.Start, timeZoneEws);
                DateTime EWSendDate = TimeZoneInfo.ConvertTimeFromUtc(appointment.End, timeZoneEws);


                if (appItem.iAllDayApp != appointment.IsAllDayEvent)
                {
                    appItem.iAllDayApp = appointment.IsAllDayEvent;
                }
                if (startDate != EWSstartDate)
                {
                    appItem.iStartDate = appointment.Start;
                }
                if (endDate != EWSendDate)
                {
                    appItem.iEndDate = appointment.End.AddDays(-1);
                }
                if (timeZone.Id != timeZoneEws.Id)
                {
                    appItem.iTimeZone = timeZoneEws.Id;
                }
            }
            else
            {
                if (appItem.iAllDayApp)
                {
                    appItem.iAllDayApp = false;
                }
                if ((DateTime)appItem.iStartDate != appointment.Start)
                {
                    appItem.iStartDate = appointment.Start;
                }
                if ((DateTime)appItem.iEndDate != appointment.End)
                {
                    appItem.iEndDate = appointment.End;
                }
            }
            if (appointment.IsReminderSet)
            {
                if (!appItem.iReminderActive) { appItem.iReminderActive = true; }
                if (appItem.iReminderMinutes != appointment.ReminderMinutesBeforeStart)
                {
                    appItem.iReminderMinutes = appointment.ReminderMinutesBeforeStart;
                }
            }
            else
            {
                appItem.iReminderActive = false;
            }
            Log.CreateLogEntry("MatterSphere Appointment Object Updated. AppID: " + appItem.ItemID.ToString());
        }

        private void FindAppoinment(AppointmentItem appItem, UserSettings usrSettings, MatterSphereSettings msSettings)
        {
            Log.CreateLogEntry("Begin Find Appointment by Appointment ID. AppID: " + appItem.ItemID.ToString());
            FindItemsResults<Item> appResults = FindAppointmentFromAppointmentID(appItem, usrSettings);
            if (appResults.Items.Count == 1)
            {
                Log.CreateLogEntry("Appointment Found");
                appItem.iEWSID = appResults.Items[0].Id.ToString();
                appItem.iExternal = false;
            }
            else
            {
                Log.CreateLogEntry("Appointment Not Found, checking if time limit has elasped. AppID: " + appItem.ItemID.ToString());
                DateTime chkDate = DateTime.UtcNow.AddMinutes(-Convert.ToDouble(Config.GetConfigurationItem("SearchAppoinmentMinutes")));
                if (appItem.iCreated < chkDate)
                {
                    Log.CreateLogEntry("Creating New Appointment for Missing Appointment");
                    //If Appointment is not found by Appointment ID and the process is running x minutes after creation date then appoinment is missing so create it.
                    appItem.iExternal = false;
                    CreateNewAppointment(appItem, usrSettings, msSettings);
                }
            }


        }

        private FindItemsResults<Item> FindAppointmentFromAppointmentID(AppointmentItem appItem, UserSettings usrSettings)
        {
            ItemView itemview = new ItemView(int.MaxValue);
            itemview.PropertySet = MSEWSProperties;
            itemview.Traversal = ItemTraversal.Shallow;
            SearchFilter searchfilter = new SearchFilter.IsEqualTo(ExtendedPropertyAppID, appItem.ItemID);
            FolderId usersCalendar = GetAppointmentFolder(usrSettings.MailBoxAddress);
            FindItemsResults<Item> appResults = ExchangeService.FindItems(usersCalendar, searchfilter, itemview);
            return appResults;
        }

        private void CreateNewAppointment(AppointmentItem appItem, UserSettings usrSettings, MatterSphereSettings msSettings)
        {
            Appointment appointment = new Appointment(ExchangeService);
            FolderId usersCalendar = GetAppointmentFolder(usrSettings.MailBoxAddress);
            appointment.Subject = appItem.iSubject;
            if (appItem.iLocation != null)
            {
                appointment.Location = appItem.iLocation;
            }
            appointment.Body = appItem.iBody;
            DateTime startDate;
            DateTime endDate;
            TimeZoneInfo timeZone = usrSettings.UserTimeZone;
            if (appItem.iAllDayApp)
            {


                // Need to Check Logic on this section!
                try
                {
                    TimeZoneInfo appTimeZone = TimeZoneInfo.FindSystemTimeZoneById(appItem.iTimeZone);
                    startDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iStartDate, appTimeZone);
                    endDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iEndDate, appTimeZone);                    
                }
                catch
                {
                    startDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iStartDate, timeZone);
                    endDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iEndDate, timeZone);
                }
                
                bool minuteadded = false;
                endDate = endDate.AddMinutes(1);
                minuteadded = true;
                appointment.IsAllDayEvent = true;
                appointment.Start = DateTime.SpecifyKind(startDate, DateTimeKind.Unspecified);
                appointment.StartTimeZone = timeZone;
                appointment.End = DateTime.SpecifyKind(endDate, DateTimeKind.Unspecified);
                TimeSpan span = endDate.Subtract(startDate);
                double days = Math.Floor(Convert.ToDouble(span.Days));
                DateTime dbStartTime = (DateTime)appItem.iStartDate;
                appItem.iEndDate = dbStartTime.AddDays(days);
                appointment.IsAllDayEvent = true;
                //Update MatterSphere appointment for Fee Earners TimeZone
                appItem.iTimeZone = timeZone.Id;
                DateTime EWSstartDate = TimeZoneInfo.ConvertTimeToUtc(appointment.Start, timeZone);
                DateTime EWSendDate = TimeZoneInfo.ConvertTimeToUtc(appointment.End, timeZone);
                if (appItem.iStartDate != EWSstartDate)
                {
                    appItem.iStartDate = EWSstartDate;
                }
                if (appItem.iEndDate != EWSendDate)
                {
                    if (minuteadded)
                    {
                        appItem.iEndDate = EWSendDate.AddMinutes(-1);
                    }
                    else
                    {
                        appItem.iEndDate = EWSendDate;
                    }
                }
                //End Update MatterSphere appointment for Fee Earners TimeZone
            }
            else
            {
                // Convert to Fee Earners TimeZone and Store TimeZone in Exchange and Appointment.
                startDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iStartDate, timeZone);
                endDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)appItem.iEndDate, timeZone);
                appointment.Start = startDate;
                appointment.StartTimeZone = timeZone;
                appointment.End = endDate;
                appItem.iTimeZone = timeZone.Id;
            }
            if (appItem.iReminderActive)
            {
                DateTime dte = appointment.Start;
                appointment.ReminderDueBy = dte.AddMinutes(-(Int32)appItem.iReminderMinutes);
                appointment.ReminderMinutesBeforeStart = ((Int32)appItem.iReminderMinutes);
            }
            else
            {
                appointment.IsReminderSet = false;
            }
            appointment.SetExtendedProperty(ExtendedPropertyClientID, appItem.iClientID);
            appointment.SetExtendedProperty(ExtendedPropertyFileID, appItem.iFileID);
            if (appItem.iAssocID != null)
            {
                appointment.SetExtendedProperty(ExtendedPropertyClientID, appItem.iClientID);
            }
            appointment.SetExtendedProperty(ExtendedPropertyCompanyID, msSettings.CompanyID);
            appointment.SetExtendedProperty(ExtendedPropertyEdition, msSettings.Edition);
            appointment.SetExtendedProperty(ExtendedPropertySerialNo, msSettings.SerialNo);
            appointment.SetExtendedProperty(ExtendedPropertyAppID, appItem.ItemID);
            appointment.LegacyFreeBusyStatus = GetDefaultFreeBusyStatus();
            if (Config.GetConfigurationItemBool("SetAppointmentTypeAsCategory"))
            {
                Log.CreateLogEntry("Attempting Category: " + appItem.iCategory);
                appointment.Categories.Add(appItem.iCategory);
            }
            Log.CreateLogEntry("New Appointment Object Created. Pre Save to EWS. AppID: " + appItem.ItemID.ToString());
            appointment.Save(usersCalendar, SendInvitationsMode.SendToNone);
            Log.CreateLogEntry("New Appointment Object Created. Post Save to EWS. AppID: " + appItem.ItemID.ToString());
            appItem.iEWSID = appointment.Id.ToString();
        }

        private LegacyFreeBusyStatus GetDefaultFreeBusyStatus()
        {
            LegacyFreeBusyStatus freeBusy;
            switch (Config.GetConfigurationItem("FreeBusyStatus"))
            {
                case "Busy":
                    freeBusy = LegacyFreeBusyStatus.Busy;
                    break;
                case "Free":
                    freeBusy = LegacyFreeBusyStatus.Free;
                    break;
                case "NoData":
                    freeBusy = LegacyFreeBusyStatus.NoData;
                    break;
                case "OOF":
                    freeBusy = LegacyFreeBusyStatus.OOF;
                    break;
                case "Tentative":
                    freeBusy = LegacyFreeBusyStatus.Tentative;
                    break;
                case "WorkingElsewhere":
                    freeBusy = LegacyFreeBusyStatus.WorkingElsewhere;
                    break;
                default:
                    freeBusy = LegacyFreeBusyStatus.Free;
                    break;
            }
            return freeBusy;
        }

        internal List<AppItemShort> GetUpdatedAppointments(string feeearneremail, DateTime lastrun)
        {
            Impersonate(feeearneremail);

            List<AppItemShort> appListEWS = null;
            FindItemsResults<Item> appResults = GetAppointments(feeearneremail, lastrun);
            if (appResults != null)
            {
                appListEWS = new List<AppItemShort>();
                foreach (Item app in appResults)
                {
                    AppItemShort appItem = new AppItemShort();
                    object appID;
                    app.TryGetProperty(ExtendedPropertyAppID, out appID);
                    appItem.AppointmentID = Convert.ToInt64(appID);
                    appItem.EWSID = app.Id.ToString();
                    appItem.EWSUpdated = app.LastModifiedTime;
                    appListEWS.Add(appItem);
                }
            }
            return appListEWS;
        }

        private FindItemsResults<Item> GetAppointments(string feeearneremail, DateTime lastrun)
        {
            try
            {
                ItemView itemview = new ItemView(int.MaxValue);
                itemview.PropertySet = MSEWSProperties;
                itemview.Traversal = ItemTraversal.Shallow;
                SearchFilter[] searchfilterCollection = new SearchFilter[2];
                searchfilterCollection[0] = new SearchFilter.Exists(ExtendedPropertyAppID);
                searchfilterCollection[1] = new SearchFilter.IsGreaterThan(ItemSchema.LastModifiedTime, lastrun);
                FolderId usersCalendar = GetAppointmentFolder(feeearneremail);
                SearchFilter searchfilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchfilterCollection);
                FindItemsResults<Item> appResults = ExchangeService.FindItems(usersCalendar, searchfilter, itemview);
                return appResults;
            }
            catch (Exception ex)
            {
                Log.CreateErrorEntry("Get Appointments Error", ex);
                return null;
            }
        }
    }
}
