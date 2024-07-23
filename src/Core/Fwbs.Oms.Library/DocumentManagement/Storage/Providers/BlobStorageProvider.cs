using System;
using System.Data;

namespace FWBS.OMS.DocumentManagement.Storage.Providers
{
    using Document = OMSDocument;

    public sealed class BlobStorageProvider : StorageProvider
    {
        #region Purge

        protected override void InternalPurge(IStorageItem item, StorageSettingsCollection settings)
        {
            Document document = item as Document;
            Precedent precedent = item as Precedent;

            if (document != null)
            {
                string sql = "delete from dbdocumentstorage where docid = @DOCID";
                Session.CurrentSession.Connection.ExecuteSQL(sql, new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("DOCID", SqlDbType.BigInt, 0, document.ID) });
            }
            else if (precedent != null)
            {
                string sql = "delete from dbprecedentstorage where precid = @PRECID";
                Session.CurrentSession.Connection.ExecuteSQLScalar(sql, new IDataParameter[1] { Session.CurrentSession.Connection.AddParameter("PRECID", SqlDbType.BigInt, 0, precedent.ID) });
            }
            else
                throw GetUnsupportedStorageItemException(item);
        }

        #endregion

        #region Store

        protected override StoreResults InternalStore(IStorageItem item, System.IO.FileInfo source, object tag, StorageSettingsCollection settings)
        {
            Document document = item as Document;
            DocumentVersion version = item as DocumentVersion;
            Precedent precedent = item as Precedent;

            byte[] data = null;

            string token = GenerateToken(item);


            try
            {
                using (System.IO.FileStream stream = source.OpenRead())
                {
                    data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    stream.Close();
                }
            }
            catch (System.IO.IOException)
            {
                //Captures the Read Only file exception.  Create a new temp file and use that to store the file.
                System.IO.FileInfo temp = FWBS.OMS.Global.GetTempFile();
                source.CopyTo(temp.FullName);

                using (System.IO.FileStream stream = temp.OpenRead())
                {
                    data = new byte[stream.Length];
                    stream.Read(data, 0, data.Length);
                    stream.Close();
                }
            }

            if (document != null)
            {
                IDataParameter[] pars = new IDataParameter[4];

                pars[0] = Session.CurrentSession.Connection.CreateParameter("token", token);
                pars[1] = Session.CurrentSession.Connection.CreateParameter("data", SqlDbType.Image, 0, data);
                pars[2] = Session.CurrentSession.Connection.CreateParameter("docid", document.ID);
                pars[3] = Session.CurrentSession.Connection.CreateParameter("latest", true);

                Session.CurrentSession.Connection.ExecuteProcedure("sprStoreStorageItem", pars);

                
            }
            else if (precedent != null)
            {
                IDataParameter[] pars = new IDataParameter[4];

                pars[0] = Session.CurrentSession.Connection.CreateParameter("token", token);
                pars[1] = Session.CurrentSession.Connection.CreateParameter("data", SqlDbType.Image, 0, data);
                pars[2] = Session.CurrentSession.Connection.CreateParameter("precid", precedent.ID);
                pars[3] = Session.CurrentSession.Connection.CreateParameter("latest", true);

                Session.CurrentSession.Connection.ExecuteProcedure("sprStoreStorageItem", pars);

            }
            else if (version != null)
            {
                IDataParameter[] pars = new IDataParameter[4];

                pars[0] = Session.CurrentSession.Connection.CreateParameter("token", token);
                pars[1] = Session.CurrentSession.Connection.CreateParameter("data", SqlDbType.Image, 0, data);
                pars[2] = Session.CurrentSession.Connection.CreateParameter("docid", version.ParentDocument.ID);
                pars[3] = Session.CurrentSession.Connection.CreateParameter("latest", false);

                if (Supports(StorageFeature.Versioning, item))
                {
                    VersionStoreSettings versettings = GetSettings<VersionStoreSettings>(settings);

                    if (versettings.MarkAsLatest)
                    {
                        pars[3].Value = true;
                    }
                }

                Session.CurrentSession.Connection.ExecuteProcedure("sprStoreStorageItem", pars);
            }
            else
                throw GetUnsupportedStorageItemException(item);


          

            return new StoreResults(item, token, source);

        }

        #endregion

        #region Fetch

        protected override FetchResults InternalFetch(IStorageItem item, StorageSettingsCollection settings)
        {

            Document document = item as Document;
            DocumentVersion version = item as DocumentVersion;
            Precedent precedent = item as Precedent;

            byte[] data = null;

    
            if (document != null)
            {
                IDataParameter[] pars = new IDataParameter[1];
                if (String.IsNullOrEmpty(item.Token))
                    pars[0] = Session.CurrentSession.Connection.CreateParameter("docid", document.ID);
                else
                {
                    pars[0] = Session.CurrentSession.Connection.CreateParameter("token", ((IStorageItem)document).Token);
                }
                data = (byte[])Session.CurrentSession.Connection.ExecuteProcedureScalar("sprFetchStorageItem", pars);
            }
            else if (precedent != null)
            {
                IDataParameter[] pars = new IDataParameter[1];
                if (String.IsNullOrEmpty(item.Token))
                    pars[0] = Session.CurrentSession.Connection.CreateParameter("precid", precedent.ID);
                else
                    pars[0] = Session.CurrentSession.Connection.CreateParameter("token", version.Token);
                data = (byte[])Session.CurrentSession.Connection.ExecuteProcedureScalar("sprFetchStorageItem", pars);
            }
            else if (version != null)
            {
                IDataParameter[] pars = new IDataParameter[1];
                if (String.IsNullOrEmpty(item.Token))
                    pars[0] = Session.CurrentSession.Connection.CreateParameter("docid", version.ParentDocument.ID);
                else
                    pars[0] = Session.CurrentSession.Connection.CreateParameter("token", version.Token);
                data = (byte[])Session.CurrentSession.Connection.ExecuteProcedureScalar("sprFetchStorageItem", pars);
            }
            else
                throw GetUnsupportedStorageItemException(item);

            if (data == null)
                return null;

            System.IO.FileInfo local = GetLocalFile(item);


            using (System.IO.FileStream stream = new System.IO.FileStream(local.FullName, System.IO.FileMode.Create))
            {
                stream.Write(data, 0, data.Length);
                stream.Flush();
            }

            return new FetchResults(item, local);

        }


        #endregion

        #region Methods

        protected override StorageProviderService CreateService()
        {
            return new BlobStorageProviderService();
        }

        protected override string GenerateToken(IStorageItem item)
        {
            if (String.IsNullOrEmpty(item.Token))
                return Guid.NewGuid().ToString();
            else
                return item.Token;
        }

        protected override bool SupportsBuiltinFeature(StorageFeature feature)
        {
            switch (feature)
            {
                case StorageFeature.Versioning:
                case StorageFeature.AllowOverwrite:
                case StorageFeature.CreateSubVersion:
                case StorageFeature.CreateVersion:
                    return true;
                case StorageFeature.Locking:
                    return true;
            }

            return base.SupportsBuiltinFeature(feature);
        }

        #endregion

    }
}
