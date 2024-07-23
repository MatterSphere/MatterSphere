using System;
using System.Collections.Generic;
using System.IO;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.DocumentManagement
{
    public class ExportDocumentCommand : FWBS.OMS.Commands.Command
    {
        protected List<FileInfo> existingDocuments = new List<FileInfo>();

        #region Properties

        public int ExistingDocumentsCount
        {
            get
            {
                if (!overwriteExisting)
                    return existingDocuments.Count;
                else
                    return 0;
            }
        }

        private readonly List<IStorageItem> docs = new List<IStorageItem>();
        public List<IStorageItem> Documents
        {
            get
            {
                return docs;
            }
        }

        protected string exportLocation;
        public string ExportLocation
        {
            set { exportLocation = value; }
        }

        private bool overwriteExisting = false;
        public bool OverwriteExisting
        {
            get { return overwriteExisting; }
            set { overwriteExisting = value; }
        }
        
        #endregion

        protected string GetFileName(IStorageItem item)
        {
            string fileName = item.Name;
            DocumentVersion vers = item as DocumentVersion;
            if (vers != null)
                fileName = vers.ParentDocument.Description;

            fileName = FWBS.Common.FilePath.ExtractInvalidChars(fileName);

            return string.Format("{0} ({1}).{2}", fileName, item.DisplayID, item.Extension);
        }

        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {
            if (string.IsNullOrEmpty(exportLocation))
                throw new ArgumentNullException("exportLocation");
            else if (!Directory.Exists(exportLocation))
                throw new DirectoryNotFoundException(exportLocation);

            FWBS.OMS.Commands.ExecuteResult res = new FWBS.OMS.Commands.ExecuteResult();
            res.Status = FWBS.OMS.Commands.CommandStatus.Failed;

            FWBS.OMS.Session.CurrentSession.OnProgressStart();
            FWBS.Common.ProgressEventArgs _progress = new FWBS.Common.ProgressEventArgs(Documents.Count);
            FWBS.OMS.Session.CurrentSession.OnProgress(_progress);
            try
            {
                foreach (IStorageItem item in Documents)
                {
                    string fileName = GetFileName(item);

                    FileInfo info = new FileInfo(Path.Combine(exportLocation, fileName));

                    if (!OverwriteExisting && existingDocuments.Contains(info))
                        continue;

                    StorageSettingsCollection coll = new StorageSettingsCollection();
                    LockableFetchSettings fetchSettings = new LockableFetchSettings();
                    fetchSettings.CheckOut = false;
                    coll.Add(fetchSettings);

                    try
                    {

                        FetchResults fetch = item.GetStorageProvider().Fetch(item, true, coll);
                        _progress.Message = "Exporting : " + info.Name;

                        if (info.Exists)
                        {
                            if (overwriteExisting)
                                info.Delete();
                            else
                                continue;
                        }

                        fetch.LocalFile.CopyTo(info.FullName);
                        _progress.Current++;
                        FWBS.OMS.Session.CurrentSession.OnProgress(_progress);
                    }
                    catch (CancelStorageException)
                    { }
                    catch (Exception ex)
                    {
                        res.Errors.Add(ex);
                    }
                }
            }
            finally
            {
                FWBS.OMS.Session.CurrentSession.OnProgressFinished();
            }

            res.Status = FWBS.OMS.Commands.CommandStatus.Success;
            
            return res;
        }
    }
}
