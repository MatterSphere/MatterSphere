using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using FWBS.OMS;
using FWBS.OMS.DocumentManagement;
using FWBS.OMS.DocumentManagement.Storage;
using Microsoft.Exchange.WebServices.Data;
using Microsoft.VisualBasic.CompilerServices;

namespace MCEPGlobalClasses
{
    public class MCEPStorerClass
    {
        private MCEPGlobalClasses.MCEPEWS mcepEWS;
        private MCEPGlobalClasses.MCEPDatabase mcepDB;
        private MCEPGlobalClasses.MCEPLogging mcepLog;
        private MCEPGlobalClasses.MCEPRedemption mcepRed;

        static MCEPStorerClass()
        {
            typeof(Session).GetField("_installLocation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(Session.CurrentSession, AppDomain.CurrentDomain.BaseDirectory);
        }

        private void GenerateClassReferences()
        {
            if (mcepLog == null) mcepLog = new MCEPLogging("MCEPStorerEWS");
            if (mcepEWS == null) mcepEWS = new MCEPEWS();
            if (mcepDB == null) mcepDB = new MCEPDatabase();
            if (mcepRed == null) mcepRed = new MCEPRedemption();
        }

        private void DisposeOfClassReferences()
        {
            if (mcepEWS != null)
            {
                mcepEWS.Dispose();
                mcepEWS = null;
            }
            if (mcepDB != null)
            {
                mcepDB.Dispose();
                mcepDB = null;
            }
            if (mcepLog != null) mcepLog = null;
            if (mcepRed != null) mcepRed = null;
        }

        public bool IsCancellationRequested { get; set; }

        public void RunProcess()
        {
            try
            {
                GenerateClassReferences();
                mcepLog.CreateLogEntry("MCEP Storer - Process Started");  
                if (!GetMatterSphereSession())
                {
                    mcepLog.CreateErrorEntry("Error Creating MatterSphere Login: Ending Process");
                    return;
                }
                mcepLog.CreateLogEntry("Getting List of Emails to Store");
                List<MCEPMailItem> mcepMails = GetItemsToProcess();
                if (mcepMails == null || mcepMails.Count == 0)
                {
                    mcepLog.CreateLogEntry("No Items Returned to Process");
                    return;
                }
                mcepLog.CreateLogEntry("No of Items Returned : " + Convert.ToString(mcepMails.Count));
                mcepLog.CreateLogEntry("Processing of Items Begins");
                ProcessItemsList(mcepMails);
                mcepLog.CreateLogEntry("Item Processing Ended");
            }
            catch (Exception ex)
            {
                mcepLog.CreateErrorEntry("Run Process Error", ex);
            }
            finally
            {
                mcepLog.CreateLogEntry("MCEP Storer - Process Ended");
                CleanUpTempFolder();
                DisposeOfClassReferences();
            }
        }

        private void CleanUpTempFolder()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            path = Path.Combine(path, "Elite", "MCEP", "Temp");
            DirectoryInfo directory = new DirectoryInfo(path);
            if (directory.Exists)
            {
                foreach (FileInfo file in directory.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            if (Session.CurrentSession.IsLoggedIn)
            {
                CleanUpMatterSphereFolder();
            }
        }

        private void CleanUpMatterSphereFolder()
        {
            List<FileInfo> files = new List<FileInfo>(StorageManager.CurrentManager.LocalDocuments.LocalPrecedentDirectory.GetFiles());
            files.AddRange(StorageManager.CurrentManager.LocalDocuments.LocalDocumentDirectory.GetFiles());
            foreach (FileInfo file in files)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception)
                {
                }
            }
        }

        private bool GetMatterSphereSession()
        {
            try
            {
                FWBS.OMS.Data.DatabaseSettings settings;
                string databaseServer = MCEPConfiguration.GetConfigurationItem("MatterSphereServer");
                string databaseName = MCEPConfiguration.GetConfigurationItem("MatterSphereDatabase");
                string databaseLoginType = MCEPConfiguration.GetConfigurationItem("MatterSphereLoginType");

                try
                {
                    mcepLog.CreateLogEntry("Beginning MatterSphere Login");
                    mcepLog.CreateLogEntry("Checking for exisiting connection");
                    Session.CurrentSession.APIConsumer = System.Reflection.Assembly.GetExecutingAssembly();
                    Session.CurrentSession.Connect();   // Try to connect to existing session

                    settings = Session.CurrentSession.CurrentDatabase;
                    if (!settings.Server.Equals(databaseServer, StringComparison.CurrentCultureIgnoreCase) ||
                        !settings.DatabaseName.Equals(databaseName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Session.CurrentSession.Disconnect();
                        throw new InvalidOperationException("MCEPEWS and MatterSphere database mismatch!");
                    }
                    mcepLog.CreateLogEntry(string.Format("MatterSphere login to {0} successful.", settings.DatabaseName));
                    return true;
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
                catch (Exception)
                {
                    mcepLog.CreateLogEntry("No existing connection. Trying explicit connection.");

                    if (string.IsNullOrEmpty(databaseServer))
                        throw new Exception("MatterSphereServer setting has not been configured.");

                    if (string.IsNullOrEmpty(databaseName))
                        throw new Exception("MatterSphereDatabase setting has not been configured.");

                    if (string.IsNullOrEmpty(databaseLoginType))
                        throw new Exception("MatterSphereLoginType setting has not been configured.");

                    FWBS.OMS.Data.DatabaseConnections connections = new FWBS.OMS.Data.DatabaseConnections("MCEPEWS", "OMS", "2.0");
                    settings = connections.CreateDatabaseSettings();
                    settings.Server = databaseServer;
                    settings.DatabaseName = databaseName;
                    settings.LoginType = databaseLoginType;
                    settings.Provider = "SQL";

                    Session.CurrentSession.LogOn(settings, Environment.UserName, "", true);
                    mcepLog.CreateLogEntry(string.Format("MatterSphere login to {0} successful.", settings.DatabaseName));
                    return true;
                }
            }
            catch (Exception ex)
            {
                mcepLog.CreateErrorEntry("MatterSphere Error", ex);
                return false;
            }
        }

        private void ProcessItemsList(List<MCEPMailItem> mcepMails)
        {
            foreach (MCEPMailItem item in mcepMails)
            {
                if (IsCancellationRequested)
                    break;

                ProcessItem(item);
            }
        }

        private void ProcessItem(MCEPMailItem item)
        {
            try
            {
                mcepLog.CreateLogEntry("Starting Process for Item : " + item.MessageID);
                mcepLog.CreateLogEntry("Attempting to Open Item in EWS");
                mcepEWS.Impersonate(item.userEmail);
                EmailMessage message = mcepEWS.GetEWSMailItem(item.MessageID);
                if (message == null) return;
                string existingDOCID = mcepEWS.CheckForExistingDOCID(message);
                if (existingDOCID != string.Empty)
                {
                    item.Processed = true;
                    item.Result = "Message saved outside process as DOCID : " + existingDOCID;
                    mcepLog.CreateLogEntry("Message was saved outside of MCEP - DOCID: " + existingDOCID);
                    return;
                }
                if (!CanSaveAsDocument(message))
                {
                    item.Processed = true;
                    item.Result = "Unsupported";
                    mcepLog.CreateLogEntry("Unsupported message class : " + message.ItemClass);
                    return;
                }
                mcepLog.CreateLogEntry("Item Location and Binded to in EWS");
                MCEPEmailBasicProps mailprop = mcepEWS.GetBasicMailProperties(message);
                mcepLog.CreateLogEntry("Beginning Save to MatterSphere");
                OMSDocument doc = SaveMessageToMatterSphere(item, message, mailprop);
                mcepLog.CreateLogEntry("Item Saved to MatterSphere DOCID : " + doc.ID.ToString());
                mcepLog.CreateLogEntry("Updating Exchange Item in EWS with MatterSphere Properties");
                mcepEWS.SetNamedPropertiesOnMessage(message, doc);
                mcepLog.CreateLogEntry("Exchange Item Updated Succesfully");
                UpdateItemProperties(item, doc);
            }
            catch (Exception ex)
            {
                item.Processed = true;
                item.Result = "Error Occurred";
                item.ErrorMessage = ex.Message;
                mcepLog.CreateErrorEntry("Process Item Error", ex);
            }
            finally
            {
                mcepLog.CreateLogEntry("Updating Database Entry with Result");
                SaveItemUpdatetoDatabase(item);
                mcepLog.CreateLogEntry("Database Entry Updated with Result");
                mcepLog.CreateLogEntry("Ending Process for Item : " + item.MessageID);
            }
        }

        private void SaveItemUpdatetoDatabase(MCEPMailItem item)
        {
            mcepDB.UpdateItemRow(item);
        }

        private void UpdateItemProperties(MCEPMailItem item, OMSDocument doc)
        {
            item.Processed = true;
            item.Result = "Item Saved";
            item.ErrorMessage = null;
            item.AssocID = doc.Associate.ID;
            item.DocID = doc.ID;
            item.ItemUpdated = DateTime.UtcNow;
        }

        private bool CanSaveAsDocument(EmailMessage message)
        {
            string messageType = message.ItemClass.ToUpper();
            string[] messageTypes = Session.CurrentSession.EmailMessageTypes.Split(';');

            foreach (string mt in messageTypes)
            {
                if (LikeOperator.LikeString(messageType, mt, Microsoft.VisualBasic.CompareMethod.Binary))
                    return true;
            }

            return false;
        }

        private OMSDocument SaveMessageToMatterSphere(MCEPMailItem item, EmailMessage message, MCEPEmailBasicProps mailprop)
        {
            mcepLog.CreateLogEntry("MatterSphere Save - Start");
            OMSFile file = OMSFile.GetFile(Convert.ToInt64(item.FileID));
            Associate assoc = file.DefaultAssociate;
            Precedent prec = Precedent.GetDefaultPrecedent("EMAIL", assoc);
            DocumentDirection direction = (mailprop.EmailFrom == item.userEmail) ? DocumentDirection.Out : DocumentDirection.In;
            mcepLog.CreateLogEntry("MatterSphere Save - After OMSFile, Associate, Precedent and Document Direction Found");
            OMSDocument doc;
            mcepLog.CreateLogEntry("MatterSphere Save - After Document Object Created");
            if (CheckAutoSaveAttachments())
            {
                mcepLog.CreateLogEntry("MatterSphere Save - Attachment Strip Enabled. Check For Attachments.");
                var excludeExtensions = new HashSet<string>(Session.CurrentSession.ExcludeFileExtensions.Split('|'), StringComparer.InvariantCultureIgnoreCase);
                long maxFileSize = Session.CurrentSession.ExcludeFileSize * 1024;
                List<SubDocument> subDocs = mcepEWS.ExtractAttachments(message, excludeExtensions, maxFileSize);
                mcepLog.CreateLogEntry("MatterSphere Save - Attachments extracted - No : " + subDocs.Count.ToString());
                doc = new OMSDocument(assoc, mailprop.Subject, prec, null, 0, direction, ".msg", -1, subDocs.ToArray());
            }
            else
            {
                mcepLog.CreateLogEntry("MatterSphere Save - Attachment Strip NOT Enabled");
                doc = new OMSDocument(assoc, mailprop.Subject, prec, null, 0, direction, ".msg", -1);
            }
            mcepLog.CreateLogEntry("MatterSphere Save - After Document Object Populated");
            DocumentVersion version = SetAdditonalPropertiesAndGenerateDocumentVersion(item, mailprop, doc);
            doc.TimeRecords.SkipTime = true;
            mcepLog.CreateLogEntry("MatterSphere Save - After Various additonal properties generated. Before Document Update.");
            try
            {
                doc.Update();
                mcepLog.CreateLogEntry("MatterSphere Save - Document Updated - Attachments Stored if in place.");
            }
            catch (StorageItemDuplicatedException)
            {
                mcepLog.CreateLogEntry("MatterSphere Save - Duplicate Exception.");
            }
            if (MCEPConfiguration.GetConfigurationItemBool("TimeRecordingEnabled"))
            {
                mcepLog.CreateLogEntry("MatterSphere Save - Time Recording Enabled.");
                GenerateDocumentTimeRecord(item, mailprop, doc);
                mcepLog.CreateLogEntry("MatterSphere Save - Time Recording Added if Licensed.");
            }
            FWBS.OMS.DocumentManagement.Storage.StorageProvider storageProvider = doc.GetStorageProvider();
            FWBS.OMS.DocumentManagement.EmailDocument emailDoc;
            emailDoc = GenerateAndPopulateEmailDocumentObject(mailprop, doc);
            mcepLog.CreateLogEntry("MatterSphere Save - Email Document Generated");
            var si = version as IStorageItem;
            if (si == null) si = doc;
            System.IO.FileInfo docFile = storageProvider.GetLocalFile(si);
            mcepLog.CreateLogEntry("MatterSphere Save - LocalFile Path generated for StorageItem");
            mcepLog.CreateLogEntry("docfile.FullName = " + docFile.FullName);
            using (System.IO.FileStream afs = new System.IO.FileStream(docFile.FullName, System.IO.FileMode.Create))
            {
                using (MemoryStream fs = GenerateMSGFile(message))
                {
                    fs.WriteTo(afs);
                }
            }
            mcepLog.CreateLogEntry("MatterSphere Save - MSG File for Email Extracted");
            string tempPath = mcepRed.AddNamedPropertiesToSavedMsgFile(docFile, doc, version);
            mcepLog.CreateLogEntry("MatterSphere Save - Properties added to MSG File");
            try
            {
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }
            }
            catch { }
            StoreResults result =  storageProvider.Store(si, docFile);
            mcepLog.CreateLogEntry("MatterSphere Save - File Stored into Database.");
            return doc;
        }

        private static DocumentVersion SetAdditonalPropertiesAndGenerateDocumentVersion(MCEPMailItem item, MCEPEmailBasicProps mailprop, OMSDocument doc)
        {
            doc.AuthoredDate = mailprop.EmailCreated;
            User user = User.GetUser(Convert.ToInt32(item.UserID));
            doc.Author = user;
            IStorageItem storeItem = doc;
            IStorageItemVersionable versionable = doc;
            DocumentVersion version = (DocumentVersion)versionable.GetLatestVersion();
            versionable.SetWorkingVersion(version);
            IStorageItemDuplication duplication = doc;
            IStorageItemLockable lockable = doc.GetStorageProvider().GetLockableItem(doc);
            doc.DocProgType = FWBS.OMS.Apps.ApplicationManager.CurrentManager.GetRegisteredApplicationByExtension(storeItem.Extension);
            return version;
        }

        private EmailDocument GenerateAndPopulateEmailDocumentObject(MCEPEmailBasicProps mailprop, OMSDocument doc)
        {
            FWBS.OMS.DocumentManagement.EmailDocument emailDoc;
            try
            {
                mcepLog.CreateLogEntry("MatterSphere Save - EmailDocument to be Generated");
                emailDoc = new FWBS.OMS.DocumentManagement.EmailDocument(doc);
                mcepLog.CreateLogEntry("MatterSphere Save - Attached to existing EmailDocument Record");
            }
            catch (MissingCommonObjectException) //  No email document record
            {
                emailDoc = new EmailDocument();
                emailDoc.SetExtraInfo("docid", doc.ID);
                mcepLog.CreateLogEntry("MatterSphere Save - Attached to new EmailDocument Record");
            }
            emailDoc.Class = "IPM.Note";
            emailDoc.From = mailprop.EmailFrom;
            emailDoc.To = mailprop.EmailTo;
            emailDoc.CC = mailprop.EmailCC;
            emailDoc.Sent = mailprop.EmailSent;
            emailDoc.Received = mailprop.EmailRecieved;
            emailDoc.Update();
            return emailDoc;
        }

        private static void GenerateDocumentTimeRecord(MCEPMailItem item, MCEPEmailBasicProps mailprop, OMSDocument doc)
        {
            doc.TimeRecords.SkipTime = false;
            FeeEarner feeEarner = FWBS.OMS.User.GetUser(Convert.ToInt32(item.UserID)).WorksFor;
            string activityType;
            if (mailprop.EmailFrom == item.userEmail)
            {
                activityType = MCEPConfiguration.GetConfigurationItem("TimeRecordingActivityCodeOut");
            }
            else
            {
                activityType = MCEPConfiguration.GetConfigurationItem("TimeRecordingActivityCodeIn");
            }
            TimeRecord timeRecord;
            if (doc.TimeRecords.Count == 0)
            {
                timeRecord = new TimeRecord(doc, true);
            }
            else
            {
                timeRecord = doc.TimeRecords[0];
            }
            if (feeEarner != null) timeRecord.FeeEarnerID = feeEarner.ID;
            timeRecord.TimeDate = DateTime.UtcNow;
            timeRecord.TimeUnits = Convert.ToInt32(MCEPConfiguration.GetConfigurationItem("TimeRecordingUnits"));
            timeRecord.ActivityCode = activityType;
            doc.TimeRecords[0] = timeRecord;
            doc.Update();
        }

       

        private MemoryStream GenerateMSGFile(EmailMessage message)
        {
            mcepLog.CreateLogEntry("Generate MSG - Start");
            MemoryStream fs = mcepEWS.GetEMLFileStream(message);
            mcepLog.CreateLogEntry("After Start FileStream");

            MemoryStream msg = new MemoryStream();
            mcepLog.CreateLogEntry("After Memory Stream");
            Aspose.Email.License license = new Aspose.Email.License();
            license.SetLicense("Aspose.Total.lic");
            mcepLog.CreateLogEntry("After Aspose License");
            using (Aspose.Email.MailMessage asposeMessage = Aspose.Email.MailMessage.Load(fs, new Aspose.Email.EmlLoadOptions()))
            {
                mcepLog.CreateLogEntry("After Generate AsposeMessage");
                asposeMessage.Save(msg, new Aspose.Email.MsgSaveOptions(Aspose.Email.MailMessageSaveType.OutlookMessageFormatUnicode));
            }
            mcepLog.CreateLogEntry("After Aspose Stuff");
            fs.Close();

            return msg;
        }

        private List<MCEPMailItem> GetItemsToProcess()
        {
            try
            {
                DataTable itemsTable = mcepDB.ItemsToSyncFromDatabase();
                if (itemsTable == null)
                {
                    return null;
                }
                List<MCEPMailItem> items = new List<MCEPMailItem>();
                foreach (DataRow row in itemsTable.Rows)
                {
                    items.Add(new MCEPMailItem(row));
                }
                itemsTable = null;
                return items;
            }
            catch (Exception ex)
            {
                mcepLog.CreateErrorEntry(ex.Message);
                return null;
            }
        }

        private bool CheckAutoSaveAttachments()
        {
            User currentUser = Session.CurrentSession.CurrentUser;
            return (currentUser.AutoSaveAttachments == FWBS.Common.TriState.True) ||
                (currentUser.AutoSaveAttachments == FWBS.Common.TriState.Null && Session.CurrentSession.AutoSaveAttachments);
        }

        public string GetMailInformation(string MailItemID)
        {
            try
            {
                GenerateClassReferences();
                EmailMessage message = mcepEWS.GetEWSMailItem(MailItemID);
                MCEPEmailBasicProps mailprop = mcepEWS.GetBasicMailProperties(message);
                if (message == null) return "No Message";
                return mcepEWS.GetAllInformation(message);
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            finally
            {
                DisposeOfClassReferences();
            }
        }
    }
}
