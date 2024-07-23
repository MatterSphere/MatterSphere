using System;
using System.Collections.Generic;

namespace FWBS.OMS.DocumentManagement
{
    using FWBS.OMS.Interfaces;
    using Storage;


    public class MoveDocumentCommand : FWBS.OMS.Commands.Command
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

        public Associate ToAssociate { get; set; }

        #endregion

        #region IProcess Members

        protected InvalidOperationException CreateAssociateMissingException()
        {
            throw new InvalidOperationException(Session.CurrentSession.Resources.GetMessage("MSGASMSTBESPC", "Associate to copy to must be specified.", "").Text);
        }

        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {
            FWBS.OMS.Commands.ExecuteResult res = new FWBS.OMS.Commands.ExecuteResult();

            //Makes sure that a default associate is specified.
            if (ToAssociate == null)
                throw CreateAssociateMissingException();


            foreach (OMSDocument document in Documents)
            {
                if (document == null)
                    continue;

                try
                {
                    Execute(document, res);

                    if (res.Status == FWBS.OMS.Commands.CommandStatus.Canceled)
                        return res;
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


        protected virtual void Execute(OMSDocument document, FWBS.OMS.Commands.ExecuteResult res)
        {
            OMSDocument doc = document;

            Associate originalassoc = doc.Associate;

            //If the associate is the same then ignore the process.
            if (doc.Associate.ID == ToAssociate.ID)
                return;

            Dictionary<IStorageItem, FetchResults> files = new Dictionary<IStorageItem, FetchResults>();

            foreach (IStorageItem version in doc.GetVersions())
            {

                StorageProvider fetchsp = version.GetStorageProvider();
                StorageSettingsCollection fetchsettings = fetchsp.GetDefaultSettings(version, SettingsType.Fetch);

                //Get the physical document
                LockableFetchSettings fetchlocksettings = fetchsettings.GetSettings<LockableFetchSettings>();
                VersionFetchSettings fetchversettings = fetchsettings.GetSettings<VersionFetchSettings>();

                if (fetchlocksettings != null)
                    fetchlocksettings.CheckOut = false;
                if (fetchversettings != null)
                    fetchversettings.Version = VersionFetchSettings.FetchAs.Current;

                FetchResults fr = fetchsp.Fetch(version, true, fetchsettings);

                files.Add(version, fr);            
   
            }


            //Change the existing document header data 
            //Set the client id, file id and associate id based on the new associate.
            doc.ChangeAssociate(ToAssociate);

            //Store after the associate changes.
            foreach (IStorageItem version in files.Keys)
            {

                StorageProvider storesp = version.GetStorageProvider();
                StorageSettingsCollection storesettings = version.GetSettings();

                //Only override the following store settings defaults if they have not already been set
                //by the wizard etc...
                if (storesettings == null)
                {
                    storesettings = storesp.GetDefaultSettings(version, SettingsType.Store);

                    LockableStoreSettings storelocksettings = storesettings.GetSettings<LockableStoreSettings>();
                    VersionStoreSettings storeversettings = storesettings.GetSettings<VersionStoreSettings>();

                    if (storelocksettings != null)
                        storelocksettings.CheckIn = true;

                    if (storeversettings != null)
                        storeversettings.SaveItemAs = VersionStoreSettings.StoreAs.OriginalOverwrite;

                }

                version.ApplySettings(storesettings);

                FetchResults fr = files[version];

                //Set the document properties to the new client id , file id and associate id.
                using (System.IO.OleFileInfo props = new System.IO.OleFileInfo(fr.LocalFile.FullName))
                {
                    IOMSApp.SetDocumentProperty<long>(props, IOMSApp.CLIENT, doc.OMSFile.ClientID);
                    IOMSApp.SetDocumentProperty<long>(props, IOMSApp.FILE, doc.OMSFileID);
                    IOMSApp.SetDocumentProperty<long>(props, IOMSApp.ASSOCIATE, doc.ID);
                    props.Save();
                }

                try
                {

                    //Store the document back to the storage provider.
                    StoreResults sr = storesp.Store(version, fr.LocalFile, null, true, storesettings);
                    sr.Item.AddActivity("MOVED", "", originalassoc.ID.ToString());

                    if (Session.CurrentSession.IsProcedureInstalled("sprDocumentAfterMove"))
                    {
                        List<System.Data.IDataParameter> pars = new List<System.Data.IDataParameter>();
                        pars.Add(Session.CurrentSession.Connection.AddParameter("DOCID", document.ID));
                        Session.CurrentSession.Connection.ExecuteProcedure("sprDocumentAfterMove", pars.ToArray());
                    }
                }
                catch (CancelStorageException)
                {
                    res.Status = FWBS.OMS.Commands.CommandStatus.Canceled;
                }
                catch (StorageException)
                {
                    doc.Cancel();
                    doc.ClearSettings();
                    throw;
                }
            }
        }



        #endregion
    }
}
