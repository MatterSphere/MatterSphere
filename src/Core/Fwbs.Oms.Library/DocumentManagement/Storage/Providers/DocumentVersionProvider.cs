using System;
using System.IO;

namespace FWBS.OMS.DocumentManagement.Storage.Providers
{
    public class DocumentVersionProvider : BaseStorageItemProvider, IStorageItemProvider2
    {

        public FileInfo GeneratePath(IStorageItem item, FileInfo source, out string token)
        {
            var fileInfo = GeneratePath(item, out token);
            if (!Path.GetExtension(token).Equals(source.Extension, StringComparison.InvariantCultureIgnoreCase))
            {
                token = Path.ChangeExtension(token, source.Extension);
                fileInfo = new FileInfo(Path.ChangeExtension(fileInfo.FullName, source.Extension));
            }
            return fileInfo;
        }

        public FileInfo GeneratePath(IStorageItem item, out string token)
        {
            DocumentVersion docversion = item as DocumentVersion;
            OMSDocument document = docversion.ParentDocument;

             System.IO.FileInfo file;

            if (!CheckExistence(docversion, out file, out token))
            {
                short dirid = document.DirectoryID;

                token = GenerateToken(docversion);

                string path = Path.Combine(Session.CurrentSession.GetDirectory(dirid).FullName, token);
                file = new FileInfo(path);
            }

            return file;
        }

        private bool CheckExistence(IStorageItem item, out FileInfo file, out string token)
        {
            DocumentVersion docversion = item as DocumentVersion;

            file = FetchDocumentVersion(docversion, item);

            if (file != null && file.Exists)
            {
                token = item.Token;
                return true;
            }
            file = null;
            token = String.Empty;
            return false;
        }

        public string GenerateToken(IStorageItem item)
        {
            DocumentVersion docversion = item as DocumentVersion;
            return GenerateVirtualDocumentVersionPath(docversion, item);
        }

        public FileInfo FetchStorageItem(IStorageItem item)
        {
            System.IO.FileInfo file = null;
            string path;
            DocumentVersion docversion = item as DocumentVersion;

            OMSDocument document = docversion.ParentDocument;

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

            return file;
        }

        private string GenerateVirtualDocumentVersionPath(DocumentVersion docversion, IStorageItem item)
        {
            OMSDocument document = docversion.ParentDocument;
            string path = String.Empty;
            path = System.IO.Path.Combine(path, document.ClientID.ToString());
            if (document.GetExtraInfo("fileid") != DBNull.Value)
                path = System.IO.Path.Combine(path, document.OMSFileID.ToString());

            path = System.IO.Path.Combine(path, document.Direction.ToString());
            path = System.IO.Path.Combine(path, item.Pointer);
            path += FWBS.Common.FilePath.ExtractInvalidChars((item.Extension.StartsWith(".") ? item.Extension : "." + item.Extension));
            return path;
        }

        private System.IO.FileInfo FetchDocumentVersion(DocumentVersion docversion, IStorageItem item)
        {
            System.IO.FileInfo file = null;
            string path;

            OMSDocument document = docversion.ParentDocument;

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

            return file;
        }
    }
}