using System;
using System.IO;

namespace FWBS.OMS.DocumentManagement.Storage.Providers
{
    public class PrecedentProvider : BaseStorageItemProvider, IStorageItemProvider
    {
        public FileInfo GeneratePath(IStorageItem item, out string token)
        {
            Precedent precedent = item as Precedent;

            FileInfo file;

            if (!CheckExistence(precedent, out file, out token))
            {
                short dirid = precedent.DirectoryID;

                token = GenerateToken(precedent);
                string path = Path.Combine(Session.CurrentSession.GetDirectory(dirid).FullName, token);
                file = new FileInfo(path);
            }

            return file;
        }

        public bool CheckExistence(IStorageItem item, out FileInfo file, out string token)
        {
            Precedent precedent = item as Precedent;
            file = FetchPrecedent(precedent, item);

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
            Precedent precedent = item as Precedent;
            return GenerateVirtualPrecedentPath(precedent, item);
        }

        public FileInfo FetchStorageItem(IStorageItem item)
        {
            System.IO.FileInfo file = null;
            Precedent precedent = item as Precedent;

            //If the item id still in the live system then find the path of where 
            //it exists by interigating the global or branch based document directories.
            if (precedent.LiveDirectory == String.Empty || !System.IO.Directory.Exists(precedent.LiveDirectory))
            {
                file = FindPath(item, SystemDirectories.OMPrecedents, precedent.GetExtraInfo("brid"));
            }
            else
            {
                //If the live directory can be found straight away at a specific location
                //then combine the directory path and the virtual path together
                //into a full file path.
                string path = System.IO.Path.Combine(precedent.LiveDirectory, item.Token);
                if (System.IO.File.Exists(path))
                {
                    file = new System.IO.FileInfo(path);
                }
                else
                    file = FindPath(item, SystemDirectories.OMPrecedents, precedent.GetExtraInfo("brid"));
            }

            return file;
        }

        private System.IO.FileInfo FetchPrecedent(FWBS.OMS.Precedent precedent, IStorageItem item)
        {
            System.IO.FileInfo file = null;

            //If the item id still in the live system then find the path of where 
            //it exists by interigating the global or branch based document directories.
            if (precedent.LiveDirectory == String.Empty || !System.IO.Directory.Exists(precedent.LiveDirectory))
            {
                file = FindPath(item, SystemDirectories.OMPrecedents, precedent.GetExtraInfo("brid"));
            }
            else
            {
                //If the live directory can be found straight away at a specific location
                //then combine the directory path and the virtual path together
                //into a full file path.
                string path = System.IO.Path.Combine(precedent.LiveDirectory, item.Token);
                if (System.IO.File.Exists(path))
                {
                    file = new System.IO.FileInfo(path);
                }
                else
                    file = FindPath(item, SystemDirectories.OMPrecedents, precedent.GetExtraInfo("brid"));
            }

            return file;
        }

        private string GenerateVirtualPrecedentPath(Precedent precedent, IStorageItem item)
        {
            string path = String.Empty;
            path = System.IO.Path.Combine(path, "Company");
            path = System.IO.Path.Combine(path, item.Pointer);
            path += FWBS.Common.FilePath.ExtractInvalidChars((item.Extension.StartsWith(".") ? item.Extension : "." + item.Extension));
            return path;
        }
    }
}