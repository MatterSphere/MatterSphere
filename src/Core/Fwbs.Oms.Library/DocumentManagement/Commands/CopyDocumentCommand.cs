using System;
using System.Collections.Generic;

namespace FWBS.OMS.DocumentManagement
{
    using FWBS.OMS.Interfaces;
    using Storage;

    public class CopyDocumentCommand : FWBS.OMS.Commands.Command
    {
        #region Properties

        private readonly List<OMSDocument> docs = new List<OMSDocument>();
        public List<OMSDocument> Documents
        {
            get
            {
                return docs;
            }
        }

        private readonly List<DocumentVersion> versions = new List<DocumentVersion>();
        public List<DocumentVersion> Versions
        {
            get
            {
                return versions;
            }
        }


        public Associate ToAssociate { get; set; }


        private readonly List<OMSDocument> newdocs = new List<OMSDocument>();
        public List<OMSDocument> NewDocuments
        {
            get
            {
                return newdocs;
            }
        }

        #endregion


        #region IProcess Members

        protected InvalidOperationException CreateAssociateMissingException()
        {
            throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("MSGASMSTBESPC", "Associate to copy to must be specified.", "").Text);
        }

        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {
            NewDocuments.Clear();

            FWBS.OMS.Commands.ExecuteResult res = new FWBS.OMS.Commands.ExecuteResult();

            //Makes sure that a default associate is specified.
            if (ToAssociate == null)
                throw CreateAssociateMissingException();

            foreach (OMSDocument doc in Documents)
            {
                try
                {
                    Execute(doc, (DocumentVersion)doc.GetLatestVersion(), res);
                    if (res.Status == FWBS.OMS.Commands.CommandStatus.Canceled)
                        return res;
                }
                catch (CancelStorageException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    if (ContinueOnError)
                    {
                        res.Errors.Add(ex);
                    }
                    else
                        throw;
                }
            }

            foreach (DocumentVersion version in Versions)
            {
                try
                {
                    Execute(version.ParentDocument, version, res);
                    if (res.Status == FWBS.OMS.Commands.CommandStatus.Canceled)
                        return res;
                }
                catch (CancelStorageException)
                {
                    continue;
                }
                catch (Exception ex)
                {
                    if (ContinueOnError)
                    {
                        res.Errors.Add(ex);
                    }
                    else
                        throw;
                }
            }

            res.Status = FWBS.OMS.Commands.CommandStatus.Success;

            return res;

        }

        protected virtual void Execute(OMSDocument originaldoc, DocumentVersion originalversion, FWBS.OMS.Commands.ExecuteResult res)
        {
            if (originaldoc == null || originalversion == null)
                return;

            OMSDocument newdoc = originaldoc.Clone();
            newdoc.AllowDuplication = true;
            newdoc.Accepted = false;
            newdoc.ChangeAssociate(ToAssociate);
            newdoc.Update();

            DocumentVersion newversion = (DocumentVersion)newdoc.GetLatestVersion();
            newversion.Preview = originalversion.Preview;

            Execute(originalversion, newversion, res);

        }

        protected virtual void Execute(DocumentVersion originalversion, DocumentVersion newversion, FWBS.OMS.Commands.ExecuteResult res)
        {
            StorageProvider fetchsp = originalversion.GetStorageProvider();
            StorageProvider storesp = newversion.GetStorageProvider();

            StorageSettingsCollection fetchsettings = fetchsp.GetDefaultSettings(originalversion, SettingsType.Fetch);
            StorageSettingsCollection storesettings = newversion.GetSettings();


            //Only override the following store settings defaults if they have not already been set
            //by the wizard etc...

            if (storesettings == null)
            {
                storesettings = storesp.GetDefaultSettings(newversion, SettingsType.Store);

                LockableStoreSettings storelocksettings = storesettings.GetSettings<LockableStoreSettings>();
                VersionStoreSettings storeversettings = storesettings.GetSettings<VersionStoreSettings>();

                if (storelocksettings != null)
                    storelocksettings.CheckIn = true;

                if (storeversettings != null)
                    storeversettings.SaveItemAs = VersionStoreSettings.StoreAs.OriginalOverwrite;
            }


            newversion.ApplySettings(storesettings);


            //Get the physical document
            LockableFetchSettings fetchlocksettings = fetchsettings.GetSettings<LockableFetchSettings>();
            VersionFetchSettings fetchversettings = fetchsettings.GetSettings<VersionFetchSettings>();

            if (fetchlocksettings != null)
                fetchlocksettings.CheckOut = false;
            if (fetchversettings != null)
                fetchversettings.Version = VersionFetchSettings.FetchAs.Current;

            FetchResults fr = fetchsp.Fetch(originalversion, true, fetchsettings);

            if (newversion.ParentDocument.IsNew)
                newversion.ParentDocument.Update();

            System.IO.FileInfo file = newversion.GetIdealLocalFile();


            System.IO.File.Copy(fr.LocalFile.FullName, file.FullName, true);

            //Set the document properties to the new client id , file id and associate id.
            IOMSApp.AttachDocumentProperties(file, newversion.ParentDocument, newversion);

          
            try
            {
                //Store the document back to the storage provider.
                StoreResults sr = storesp.Store(newversion, file, null, true, storesettings);
                sr.Item.AddActivity("COPIED", "FROM", originalversion.Id.ToString());
                originalversion.AddActivity("COPIED", "TO", newversion.Id.ToString());


                if (Session.CurrentSession.IsProcedureInstalled("sprDocumentAfterCopy"))
                {
                    List<System.Data.IDataParameter> pars = new List<System.Data.IDataParameter>();
                    pars.Add(Session.CurrentSession.Connection.AddParameter("NEWDOCID", newversion.ParentDocument.ID));
                    pars.Add(Session.CurrentSession.Connection.AddParameter("ORIGINALDOCID", originalversion.ParentDocument.ID));
                    Session.CurrentSession.Connection.ExecuteProcedure("sprDocumentAfterCopy", pars.ToArray());
                }

                NewDocuments.Add(newversion.ParentDocument);
            }
            catch (CancelStorageException)
            {
                res.Status = FWBS.OMS.Commands.CommandStatus.Canceled;
            }
            catch (StorageException)
            {
                newversion.ParentDocument.Cancel();
                newversion.ParentDocument.ClearSettings();
                throw;
            }
        }


        #endregion
    }
}
