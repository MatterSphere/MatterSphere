using System;
using System.IO;

namespace FWBS.OMS.DocumentManagement.Storage.Providers
{
    public class DocumentProvider : BaseStorageItemProvider, IStorageItemProvider
    {
        public FileInfo GeneratePath(IStorageItem item, out string token)
        {
            System.IO.FileInfo file;
            var document = item as OMSDocument;

            if (!CheckExistence(document, out file, out token))
            {
                short dirid = document.DirectoryID;

                token = GenerateToken(document);

                string path = Path.Combine(Session.CurrentSession.GetDirectory(dirid).FullName, token);
                file = new FileInfo(path);
            }
            return file;
        }

        private bool CheckExistence(IStorageItem item, out FileInfo file, out string token)
        {

            OMSDocument document = item as OMSDocument;

            file = FetchStorageItem(item);

            if (file != null && file.Exists)
            {
                token = item.Token;
                return true;
            }
            file = null;
            token = String.Empty;
            return false;
        }

        public System.IO.FileInfo FetchStorageItem(IStorageItem item)
        {
            System.IO.FileInfo file = null;
            var document = item as OMSDocument;
            string path;

            if (document.IsArchived)
            {
                //If the item has been archived then find the path of where it exists by
                //interigating the global or branch based archive directories.
                if (document.ArchiveDirectory == String.Empty || !System.IO.Directory.Exists(document.LiveDirectory))
                {
                    file = FindPath(item, SystemDirectories.OMArchive, document.GetExtraInfo("docbrid"));
                }
                else
                {
                    //If the archive directory can be found straight away at a specific location
                    //then combine the directory path and the virtual path together
                    //into a full file path.
                    path = System.IO.Path.Combine(document.ArchiveDirectory, item.Token);
                    if (System.IO.File.Exists(path))
                    {
                        file = new System.IO.FileInfo(path);
                    }
                    else
                        file = FindPath(item, SystemDirectories.OMArchive, document.GetExtraInfo("docbrid"));
                }
            }
            else
            {
                //If the item id still in the live system then find the path of where 
                //it exists by interigating the global or branch based document directories.
                if (document.LiveDirectory == String.Empty || !System.IO.Directory.Exists(document.LiveDirectory))
                {
                    file = FindPath(item, SystemDirectories.OMDocuments, document.GetExtraInfo("docbrid"));
                }
                else
                {
                    //If the live directory can be found straight away at a specific location
                    //then combine the directory path and the virtual path together
                    //into a full file path.
                    path = System.IO.Path.Combine(document.LiveDirectory, item.Token);
                    if (System.IO.File.Exists(path))
                    {
                        file = new System.IO.FileInfo(path);
                    }
                    else
                        file = FindPath(item, SystemDirectories.OMDocuments, document.GetExtraInfo("docbrid"));
                }
            }

            return file;
        }

        public string GenerateToken(IStorageItem item)
        {
            OMSDocument document = item as OMSDocument;
            return GenerateVirtualDocumentPath(document, item);
        }


        private string GenerateVirtualDocumentPath(OMSDocument document, IStorageItem item)
        {
            string path = String.Empty;
            path = System.IO.Path.Combine(path, document.ClientID.ToString());
            if (document.GetExtraInfo("fileid") != DBNull.Value)
                path = System.IO.Path.Combine(path, document.OMSFileID.ToString());

            path = System.IO.Path.Combine(path, document.Direction.ToString());
            path = System.IO.Path.Combine(path, item.Pointer);
            path += FWBS.Common.FilePath.ExtractInvalidChars((item.Extension.StartsWith(".") ? item.Extension : "." + item.Extension));
            return path;
        }
    }
}