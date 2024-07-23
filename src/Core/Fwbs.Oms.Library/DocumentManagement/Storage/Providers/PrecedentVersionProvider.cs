using System;
using System.IO;

namespace FWBS.OMS.DocumentManagement.Storage.Providers
{
    public class PrecedentVersionProvider : BaseStorageItemProvider, IStorageItemProvider
    {
        public FileInfo GeneratePath(IStorageItem item, out string token)
        {
            PrecedentVersion precVersion = item as PrecedentVersion;

            FileInfo file;

            if (!CheckExistence(precVersion, out file, out token))
            {
                short dirid = precVersion.ParentDocument.DirectoryID;

                token = GenerateToken(precVersion);
                string path = Path.Combine(Session.CurrentSession.GetDirectory(dirid).FullName, token);
                file = new FileInfo(path);
            }

            return file;
        }

        private bool CheckExistence(IStorageItem item, out FileInfo file, out string token)
        {
            PrecedentVersion precversion = item as PrecedentVersion;
            file = FetchPrecedentVersion(precversion, item);

            if (file != null && file.Exists)
            {
                token = item.Token;
                return true;
            }

            file = null;
            token = String.Empty;
            return false;
        }


        private System.IO.FileInfo FetchPrecedentVersion(PrecedentVersion precversion, IStorageItem item)
        {
            System.IO.FileInfo file = null;
            string path;

            Precedent precedent = precversion.ParentDocument;

            //If the item id still in the live system then find the path of where it exists by interrogating the global or branch based precedent directories.
            if (precedent.LiveDirectory == String.Empty || !System.IO.Directory.Exists(precedent.LiveDirectory))
            {
                file = FindPath(item, SystemDirectories.OMPrecedents, precedent.GetExtraInfo("brid"));
            }
            else
            {
                //If the live directory can be found straight away at a specific location then combine the directory path and the virtual path together into a full file path.
                path = System.IO.Path.Combine(precedent.LiveDirectory, item.Token);
                if (System.IO.File.Exists(path))
                {
                    file = new System.IO.FileInfo(path);
                }
                else
                    file = FindPath(item, SystemDirectories.OMDocuments, precedent.GetExtraInfo("brid"));
            }

            return file;
        }

        public string GenerateToken(IStorageItem item)
        {
            PrecedentVersion precversion = item as PrecedentVersion;
            return GenerateVirtualPrecedentVersionPath(precversion, item);
        }

        public FileInfo FetchStorageItem(IStorageItem item)
        {
            PrecedentVersion precversion = item as PrecedentVersion;
            return FetchPrecedentVersion(precversion, item);
        }

        private string GenerateVirtualPrecedentVersionPath(PrecedentVersion precversion, IStorageItem item)
        {
            string path = String.Empty;
            path = System.IO.Path.Combine(path, "Company");
            path = System.IO.Path.Combine(path, item.Pointer);
            path += FWBS.Common.FilePath.ExtractInvalidChars((item.Extension.StartsWith(".") ? item.Extension : "." + item.Extension));
            return path;
        }
    }
}