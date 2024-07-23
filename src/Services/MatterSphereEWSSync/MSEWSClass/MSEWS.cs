using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Exchange.WebServices.Data;

namespace MSEWSClass
{
    class MSEWS : IDisposable
    {
        #region Private Global Variables
        private ExchangeService exchangeService;
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
        private string password = @"NothingToSeeHere";
        #endregion

        #region BaseMethods
        public MSEWS()
        {
            if (MSEWSConfiguration.GetConfigurationItemBool("OverrideCertificateCheck"))
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(CheckCertificate);
            }
            SetExchangeService();
            MatterSphereFields = new ExtendedPropertyDefinition[] { ExtendedPropertyCompanyID, ExtendedPropertyClientID, ExtendedPropertyFileID, ExtendedPropertyDocID,
            ExtendedPropertyAssocID, ExtendedPropertySerialNo, ExtendedPropertyVersionID, ExtendedPropertyVersionLabel, 
            ExtendedPropertyEdition, ExtendedPropertyBasePrecType, ExtendedPropertyDocIDEX, ExtendedPropertyAppID, ExtendedPropertyAssocName,ExtendedPropertyBasePrecID,ExtendedPropertyClientName,
            ExtendedPropertyFileDesc,ExtendedPropertyFileNo, ExtendedPropertyClientNo, ExtendedPropertyEWSItemID};
            MSEWSProperties = new PropertySet(BasePropertySet.FirstClassProperties, MatterSphereFields);
        }

        private ExchangeVersion LowestSupportedExchangeVersion()
        {
            return ExchangeVersion.Exchange2007_SP1;
        }

        private bool CheckCertificate(Object obj, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (certificate.GetSerialNumberString() == MSEWSConfiguration.GetConfigurationItem("CertificateSerialNumber"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SetExchangeService()
        {
            if (exchangeService == null)
            {
                ExchangeVersion version = LowestSupportedExchangeVersion();
                exchangeService = new ExchangeService(version,TimeZoneInfo.Utc);
                if (MSEWSConfiguration.GetConfigurationItemBool("OverrideExchangeCredentials"))
                {
                    exchangeService.Credentials = new NetworkCredential(MSEWSConfiguration.GetConfigurationItem("ExcUserName"), MSEWSConfiguration.GetConfigurationItem("ExcPassword"), MSEWSConfiguration.GetConfigurationItem("ExcDomain"));
                }
                else
                {
                    exchangeService.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
                if (MSEWSConfiguration.GetConfigurationItemBool("UseAutoDiscover"))
                {
                    string emailaddress = MSEWSConfiguration.GetConfigurationItem("ServiceEmailAddress");
                    exchangeService.AutodiscoverUrl(emailaddress);
                }
                else
                {
                    exchangeService.Url = new Uri(MSEWSConfiguration.GetConfigurationItem("ExchangeWebServicesURL"));
                }
            }
        }

        private string ConvertID(IdFormat inputFormat, IdFormat outputFormat, String MailBoxName, String orginalIDValue)
        {
            try
            {
                AlternateId alternateID = new AlternateId(inputFormat, orginalIDValue, MailBoxName);
                AlternateIdBase ewsResponse = exchangeService.ConvertId(alternateID, outputFormat);
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
            FindFoldersResults findResults = exchangeService.FindFolders(userRootFolder, new FolderView(int.MaxValue));
            foreach (Folder folder in findResults.Folders)
            {
                if (string.Equals(folder.DisplayName, fldName, StringComparison.OrdinalIgnoreCase)) return folder.Id.ToString();
            }
            if (defaultFolderType == "C") { return new FolderId(WellKnownFolderName.Calendar, userEmail); }
            else if (defaultFolderType == "T") { return new FolderId(WellKnownFolderName.Tasks, userEmail); }
            return null;
        }

        public void Dispose()
        {
            exchangeService = null;
            if (MSEWSConfiguration.GetConfigurationItemBool("OverrideCertificateCheck"))
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback -= new RemoteCertificateValidationCallback(CheckCertificate);
            }
        }
        #endregion

        #region DeleteItem

        internal void DeleteItem(MSEWSItem item)
        {
            Item ewsItem = Item.Bind(exchangeService, item.iEWSID);
            ewsItem.Delete(DeleteMode.SoftDelete);
            item.iItemStatus = 4;
            item.iLastProcessed = DateTime.UtcNow;
        }

        #endregion

        #region Appointments

        internal void CreateNewAppointement(MSEWSItem item)
        {
            Appointment appointment = new Appointment(exchangeService);
            FolderId usersCalendar; 
            if (item.iDefaultFolder){ usersCalendar = new FolderId(WellKnownFolderName.Calendar, item.userEmail);}
            else {usersCalendar = FindSpecifiedFolder(item.iFolder,item.userEmail,"C");}
            appointment.Subject = item.iSubject;
            if (item.iBody != null) { appointment.Body = item.iBody; }
            TimeZoneInfo tzInfo;
            if (item.iDateUTC) { tzInfo = TimeZoneInfo.Utc; }
            else { tzInfo = TimeZoneInfo.FindSystemTimeZoneById(item.iDateTimeZone); }
            if (item.iReminderActive && item.iReminderDate != null)
            {
                appointment.IsReminderSet = true;
                appointment.ReminderDueBy = (DateTime)item.iReminderDate;
            }
            else
            {
                appointment.IsReminderSet = false;
            }
            appointment.Start = (DateTime)item.iStartDate;
            appointment.StartTimeZone = tzInfo;
            if (item.iAllDayApp) { appointment.IsAllDayEvent = true; }
            if (item.iEndDate != null)
            {
                appointment.End = (DateTime)item.iEndDate;
            }
            if (item.iLocation != null)
            {
                appointment.Location = item.iLocation;
            }
            appointment.SetExtendedProperty(ExtendedPropertyEWSItemID, item.ItemID);
            if (item.iCategory != null) { appointment.Categories.Add(item.iCategory); }
            appointment.Save(usersCalendar, SendInvitationsMode.SendToNone);
            item.iEWSID = appointment.Id.ToString();
            item.iLastProcessed = DateTime.UtcNow;
            item.iItemStatus = 2;
        }
        
        internal void UpdateAppointement(MSEWSItem item)
        {
            Appointment ewsAppointment = Appointment.Bind(exchangeService, item.iEWSID,MSEWSProperties);
            object itemIDfromEWS;
            ewsAppointment.TryGetProperty(ExtendedPropertyEWSItemID, out itemIDfromEWS);
            if ((string)itemIDfromEWS != item.ItemID)
            {
                item.iItemStatus = 5;
                item.iLastProcessed = DateTime.UtcNow;
                item.iErrorMessage = "Appointment does not have Appointment ID set correctly";
                return;
            }
            if (item.iLastUpdatedDate > ewsAppointment.LastModifiedTime)
            {
                UpdateAppointmentSQLtoEWS(item, ewsAppointment);
            }
            else
            {
                if (!MSEWSConfiguration.GetConfigurationItemBool("DatabaseToEWSUpdateOnly"))
                {
                    UpdateAppointmentEWStoSQL(item, ewsAppointment);
                }
            }
        }

        private void UpdateAppointmentSQLtoEWS(MSEWSItem item, Appointment ewsAppointment)
        {
            UpdateAppointmentSQLToEWSCommon(item, ewsAppointment);
            ewsAppointment.Update(ConflictResolutionMode.AlwaysOverwrite,SendInvitationsOrCancellationsMode.SendToNone);
            item.iLastProcessed = DateTime.UtcNow;
        }

        private void UpdateAppointmentEWStoSQL(MSEWSItem item, Appointment ewsAppointment)
        {
            UpdateAppointmentEWSToSQLCommon(item, ewsAppointment);
            item.iLastProcessed = DateTime.UtcNow;
        }

        #endregion

        #region Tasks

        internal void CreateNewTask(MSEWSItem item)
        {
            Microsoft.Exchange.WebServices.Data.Task task = new Microsoft.Exchange.WebServices.Data.Task(exchangeService);
            FolderId usersTasks;
            if (item.iDefaultFolder) { usersTasks = new FolderId(WellKnownFolderName.Tasks, item.userEmail); }
            else { usersTasks = FindSpecifiedFolder(item.iFolder, item.userEmail, "T"); }
            task.Subject = item.iSubject;
            if (item.iBody != null) { task.Body = item.iBody; }
            if (item.iReminderActive && item.iReminderDate != null)
            {
                task.IsReminderSet = true;
                task.ReminderDueBy = (DateTime)item.iReminderDate;
            }
            else
            {
                task.IsReminderSet = false;
            }
            if (item.iStartDate != null) { task.StartDate = (DateTime)item.iStartDate; }
            if (item.iDueDate != null) { task.DueDate = item.iDueDate; }
            task.SetExtendedProperty(ExtendedPropertyEWSItemID, item.ItemID);
            switch (item.iTaskStatus)
            {
                case "Completed":
                    task.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.Completed;
                    break;
                case "Deferred":
                    task.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.Deferred;
                    break;
                case "InProgress":
                    task.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.InProgress;
                    break;
                case "NotStarted":
                    task.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.NotStarted;
                    break;
                case "WaitingOnOthers":
                    task.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.WaitingOnOthers;
                    break;
                default:
                    task.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.NotStarted;
                    break;
            }
            task.Save(usersTasks);
            item.iEWSID = task.Id.ToString();
            item.iLastProcessed = DateTime.UtcNow;
            item.iItemStatus = 2;
        }

        internal void UpdateTask(MSEWSItem item)
        {
            Microsoft.Exchange.WebServices.Data.Task ewsTask = Microsoft.Exchange.WebServices.Data.Task.Bind(exchangeService, item.iEWSID, MSEWSProperties);
            object itemIDfromEWS;
            ewsTask.TryGetProperty(ExtendedPropertyEWSItemID, out itemIDfromEWS);
            if ((string)itemIDfromEWS != item.ItemID)
            {
                item.iItemStatus = 5;
                item.iLastProcessed = DateTime.UtcNow;
                item.iErrorMessage = "Task does not have Task ID set correctly";
                return;
            }
            
            if (item.iLastUpdatedDate > ewsTask.LastModifiedTime)
            {
                UpdateTaskSQLtoEWS(item, ewsTask);
            }
            else
            {
                if (!MSEWSConfiguration.GetConfigurationItemBool("DatabaseToEWSUpdateOnly"))
                {
                    UpdateTaskEWStoSQL(item, ewsTask);
                }
            }


        }

        private static void UpdateTaskSQLtoEWS(MSEWSItem item, Microsoft.Exchange.WebServices.Data.Task ewsTask)
        {
            // SQL was updated after Outlook.
            if (item.iSubject != ewsTask.Subject) { ewsTask.Subject = item.iSubject; }
            if (item.iBody != null)
            {
                if (item.iBody != ewsTask.Body.Text) { ewsTask.Body = item.iBody; }
            }
            if (item.iReminderActive && item.iReminderDate != null)
            {
                ewsTask.IsReminderSet = true;
                if ((DateTime)item.iReminderDate != ewsTask.ReminderDueBy) { ewsTask.ReminderDueBy = (DateTime)item.iReminderDate; }
            }
            if (item.iStartDate != null)
            {
                if (ewsTask.StartDate != (DateTime)item.iStartDate) { ewsTask.StartDate = (DateTime)item.iStartDate; }
            }
            if (item.iDueDate != null)
            {
                if (ewsTask.DueDate != item.iDueDate) { ewsTask.DueDate = item.iDueDate; }
            }
            switch (item.iTaskStatus)
            {
                case "Completed":
                    if (ewsTask.Status != Microsoft.Exchange.WebServices.Data.TaskStatus.Completed) { ewsTask.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.Completed; }
                    break;
                case "Deferred":
                    if (ewsTask.Status != Microsoft.Exchange.WebServices.Data.TaskStatus.Deferred) { ewsTask.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.Deferred; }
                    break;
                case "InProgress":
                    if (ewsTask.Status != Microsoft.Exchange.WebServices.Data.TaskStatus.InProgress) { ewsTask.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.InProgress; }
                    break;
                case "NotStarted":
                    if (ewsTask.Status != Microsoft.Exchange.WebServices.Data.TaskStatus.NotStarted) { ewsTask.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.NotStarted; }
                    break;
                case "WaitingOnOthers":
                    if (ewsTask.Status != Microsoft.Exchange.WebServices.Data.TaskStatus.WaitingOnOthers) { ewsTask.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.WaitingOnOthers; }
                    break;
                default:
                    if (ewsTask.Status != Microsoft.Exchange.WebServices.Data.TaskStatus.NotStarted) { ewsTask.Status = Microsoft.Exchange.WebServices.Data.TaskStatus.NotStarted; }
                    break;
            }
            ewsTask.Update(ConflictResolutionMode.AutoResolve);
            item.iLastProcessed = DateTime.UtcNow;
        }

        private static void UpdateTaskEWStoSQL(MSEWSItem item, Microsoft.Exchange.WebServices.Data.Task ewsTask)
        {
            // SQL was updated after Outlook.
            if (item.iSubject != ewsTask.Subject) { item.iSubject = ewsTask.Subject; }
            if (!String.IsNullOrWhiteSpace(ewsTask.Body.Text))
            {
                if (item.iBody == null)
                {
                    item.iBody = ewsTask.Body.Text;
                }
                else
                {
                    if (item.iBody != ewsTask.Body.Text) { item.iBody = ewsTask.Body.Text; }
                }
            }

            if (ewsTask.IsReminderSet)
            {
                item.iReminderActive = ewsTask.IsReminderSet;
                item.iReminderDate = ewsTask.ReminderDueBy;
            }
            if (ewsTask.StartDate != item.iStartDate) { item.iStartDate = ewsTask.StartDate; }
            
            if (ewsTask.DueDate != item.iDueDate) { item.iDueDate = ewsTask.DueDate; }

            switch (ewsTask.Status)
            {
                case Microsoft.Exchange.WebServices.Data.TaskStatus.Completed:
                    if (item.iTaskStatus != "Completed") { item.iTaskStatus = "Completed"; }
                    break;
                case Microsoft.Exchange.WebServices.Data.TaskStatus.Deferred:
                    if (item.iTaskStatus != "Deferred") { item.iTaskStatus = "Deferred"; }
                    break;
                case Microsoft.Exchange.WebServices.Data.TaskStatus.InProgress:
                    if (item.iTaskStatus != "InProgress") { item.iTaskStatus = "InProgress"; }
                    break;
                case Microsoft.Exchange.WebServices.Data.TaskStatus.NotStarted:
                    if (item.iTaskStatus != "NotStarted") { item.iTaskStatus = "NotStarted"; }
                    break;
                case Microsoft.Exchange.WebServices.Data.TaskStatus.WaitingOnOthers:
                    if (item.iTaskStatus != "WaitingOnOthers") { item.iTaskStatus = "WaitingOnOthers"; }
                    break;
                default:
                    if (item.iTaskStatus != "NotStarted") { item.iTaskStatus = "NotStarted"; }
                    break;
            }
            item.iLastProcessed = DateTime.UtcNow;
        }

        #endregion

        #region Meetings
        
        internal void CreateNewMeeting(MSEWSItem item)
        {
            Appointment appointment = new Appointment(exchangeService);
            FolderId usersCalendar;
            if (item.iDefaultFolder) { usersCalendar = new FolderId(WellKnownFolderName.Calendar, item.userEmail); }
            else { usersCalendar = FindSpecifiedFolder(item.iFolder, item.userEmail, "C"); }
            appointment.Subject = item.iSubject;
            if (item.iBody != null) { appointment.Body = item.iBody; }
            TimeZoneInfo tzInfo;
            if (item.iDateUTC) { tzInfo = TimeZoneInfo.Utc; }
            else { tzInfo = TimeZoneInfo.FindSystemTimeZoneById(item.iDateTimeZone); }
            if (item.iReminderActive && item.iReminderDate != null)
            {
                appointment.IsReminderSet = true;
                appointment.ReminderDueBy = (DateTime)item.iReminderDate;
            }
            else
            {
                appointment.IsReminderSet = false;
            }
            appointment.Start = (DateTime)item.iStartDate;
            appointment.StartTimeZone = tzInfo;
            if (item.iAllDayApp) { appointment.IsAllDayEvent = true; }
            if (item.iEndDate != null)
            {
                appointment.End = (DateTime)item.iEndDate;
            }
            if (item.iLocation != null)
            {
                appointment.Location = item.iLocation;
            }
            List<String> itemAttendees = new List<string>(item.iMeetingAttendees.Split(';'));
            foreach (String lAttendee in itemAttendees)
            {
                Attendee attendee = new Attendee(lAttendee);
                appointment.RequiredAttendees.Add(attendee);
            }
            appointment.SetExtendedProperty(ExtendedPropertyEWSItemID, item.ItemID);
            if (item.iCategory != null) { appointment.Categories.Add(item.iCategory); }
            appointment.Save(usersCalendar, SendInvitationsMode.SendOnlyToAll);
            item.iEWSID = appointment.Id.ToString();
            item.iLastProcessed = DateTime.UtcNow;
            item.iItemStatus = 2;
        }

        internal void UpdateMeeting(MSEWSItem item)
        {
            Appointment ewsAppointment = Appointment.Bind(exchangeService, item.iEWSID, MSEWSProperties);
            object itemIDfromEWS;
            ewsAppointment.TryGetProperty(ExtendedPropertyEWSItemID, out itemIDfromEWS);
            if ((string)itemIDfromEWS != item.ItemID)
            {
                item.iItemStatus = 5;
                item.iLastProcessed = DateTime.UtcNow;
                item.iErrorMessage = "Task does not have Appointment ID set correctly";
                return;
            }
            if (item.iLastUpdatedDate > ewsAppointment.LastModifiedTime)
            {
                UpdateMeetingSQLtoOutlook(item, ewsAppointment);
            }
            else
            {
                if (!MSEWSConfiguration.GetConfigurationItemBool("DatabaseToEWSUpdateOnly"))
                {
                    UpdateMeetingOutlooktoSQL(item, ewsAppointment);
                }
            }
        }

        private void UpdateMeetingSQLtoOutlook(MSEWSItem item, Appointment ewsAppointment)
        {
            UpdateAppointmentSQLToEWSCommon(item, ewsAppointment);
            List<String> itemAttendees = new List<string>(item.iMeetingAttendees.Split(';'));
            foreach (String lAttendee in itemAttendees)
            {
                if (String.IsNullOrWhiteSpace(lAttendee)) { break; }
                bool found = false;
                foreach (Attendee attendee in ewsAppointment.RequiredAttendees)
                {
                    if (attendee.Address == lAttendee) 
                    { 
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    Attendee newattendee = new Attendee(lAttendee);
                    ewsAppointment.RequiredAttendees.Add(newattendee);
                }
            }
            List<Attendee> attendeestoDelete = new List<Attendee>();
            foreach (Attendee attendee in ewsAppointment.RequiredAttendees)
            {
                bool found = false;
                foreach (String lAttendee in itemAttendees)
                {
                    if (lAttendee == attendee.Address) 
                    { 
                        found = true; 
                        break; 
                    }
                }
                if (!found) { attendeestoDelete.Add(attendee); }
            }
            if (attendeestoDelete.Count > 0)
            {
                foreach (Attendee attendeeToDelete in attendeestoDelete)
                {
                    ewsAppointment.RequiredAttendees.Remove(attendeeToDelete);
                    try
                    {
                        ewsAppointment.OptionalAttendees.Remove(attendeeToDelete);
                    }
                    catch { }
                }
            }
            ewsAppointment.Update(ConflictResolutionMode.AlwaysOverwrite, SendInvitationsOrCancellationsMode.SendOnlyToChanged);
            item.iLastProcessed = DateTime.UtcNow;
        }

        private void UpdateMeetingOutlooktoSQL(MSEWSItem item, Appointment ewsAppointment)
        {
            UpdateAppointmentEWSToSQLCommon(item, ewsAppointment);
            StringBuilder attendeelist = new StringBuilder();
            foreach (Attendee attendee in ewsAppointment.RequiredAttendees)
            {
                attendeelist.Append(attendee.Address);
                attendeelist.Append(";");
            }
            if (item.iMeetingAttendees != attendeelist.ToString()) { item.iMeetingAttendees = attendeelist.ToString(); }
            item.iLastProcessed = DateTime.UtcNow;
        }

        #endregion

        #region AppointmentsCommonUpdate

        private static void UpdateAppointmentSQLToEWSCommon(MSEWSItem item, Appointment ewsAppointment)
        {
            if (item.iSubject != ewsAppointment.Subject) { ewsAppointment.Subject = item.iSubject; }
            if (item.iBody != null)
            {
                if (item.iBody != ewsAppointment.Body.Text) { ewsAppointment.Body = item.iBody; }
            }
            TimeZoneInfo tzInfo;
            if (item.iDateUTC) { tzInfo = TimeZoneInfo.Utc; }
            else { tzInfo = TimeZoneInfo.FindSystemTimeZoneById(item.iDateTimeZone); }
            if (item.iReminderActive && item.iReminderDate != null)
            {
                ewsAppointment.IsReminderSet = true;
                ewsAppointment.ReminderDueBy = (DateTime)item.iReminderDate;
            }
            else
            {
                ewsAppointment.IsReminderSet = false;
            }
            if (ewsAppointment.IsAllDayEvent != item.iAllDayApp) { ewsAppointment.IsAllDayEvent = item.iAllDayApp; }
            if (ewsAppointment.Start != (DateTime)item.iStartDate)
            {
                ewsAppointment.Start = (DateTime)item.iStartDate;
                ewsAppointment.StartTimeZone = tzInfo;
            }
            if (item.iEndDate != null)
            {
                if (ewsAppointment.End != item.iEndDate) { ewsAppointment.End = (DateTime)item.iEndDate; }
            }
            if (item.iLocation != null)
            {
                if (ewsAppointment.Location != item.iLocation) { ewsAppointment.Location = item.iLocation; }
            }
        }

        private static void UpdateAppointmentEWSToSQLCommon(MSEWSItem item, Appointment ewsAppointment)
        {
            if (item.iSubject != ewsAppointment.Subject) { item.iSubject = ewsAppointment.Subject; }
            if (ewsAppointment.Body.Text != null)
            {
                if (item.iBody != ewsAppointment.Body.Text) { item.iBody = ewsAppointment.Body; }
            }
            if (ewsAppointment.IsReminderSet)
            {
                item.iReminderActive = ewsAppointment.IsReminderSet;
                item.iReminderDate = ewsAppointment.ReminderDueBy;
            }
            TimeZoneInfo tzInfo;
            if (item.iDateUTC)
            { tzInfo = TimeZoneInfo.Utc; }
            else
            { tzInfo = TimeZoneInfo.FindSystemTimeZoneById(item.iDateTimeZone); }

            if (ewsAppointment.IsAllDayEvent != item.iAllDayApp) { item.iAllDayApp = ewsAppointment.IsAllDayEvent; }
            DateTime convertedStartTime = ewsAppointment.Start;
            DateTime convertedEndTime = ewsAppointment.Start;
            if (!item.iDateUTC)
            {
                convertedStartTime = TimeZoneInfo.ConvertTime(ewsAppointment.Start, TimeZoneInfo.Utc, tzInfo);
                convertedEndTime = TimeZoneInfo.ConvertTime(ewsAppointment.End, TimeZoneInfo.Utc, tzInfo);

            }
            if (convertedStartTime != item.iStartDate) { item.iStartDate = convertedStartTime; }
            if (convertedEndTime != (DateTime)item.iEndDate) { item.iEndDate = convertedEndTime; }
            if (ewsAppointment.Location != null)
            {
                if (ewsAppointment.Location != item.iLocation) { item.iLocation = ewsAppointment.Location; }
            }
        }

        #endregion
    }
}
