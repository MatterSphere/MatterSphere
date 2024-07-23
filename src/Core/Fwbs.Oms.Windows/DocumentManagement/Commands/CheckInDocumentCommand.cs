using System;
using System.IO;
using FWBS.OMS.Commands;
using FWBS.OMS.DocumentManagement;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    public sealed class CheckInDocumentCommand : FWBS.OMS.DocumentManagement.CheckInDocumentCommand
    {
        private System.Windows.Forms.IWin32Window owner;
        public System.Windows.Forms.IWin32Window Owner
        {
            set { owner = value; }
        }

        private VersionStoreSettings.StoreAs checkinOptions = VersionStoreSettings.StoreAs.NewMajorVersion;
        public VersionStoreSettings.StoreAs DefaultVersioning
        {
            set { checkinOptions = value; }
        }

        private bool applyDefaultVersioning = true;
        public bool ApplyDefaultVersioning
        {
            set{applyDefaultVersioning = value;}
        }

        public override ExecuteResult Execute()
        {
            ExecuteResult result = new ExecuteResult(CommandStatus.Success);

            bool quickSave = Session.CurrentSession.QuickSaveOnCheckIn;
            if (Docs.Count == 0)
                return result;
            else if (Docs.Count > 1 && !quickSave)
            {
                System.Windows.Forms.DialogResult res = MessageBox.ShowYesNoCancel(owner, "SHWSAVEFORCKN", "Do you want to run through the save wizard for each item?\r\n Selecting Yes will run save with the default settings");
                switch (res)
                {
                    case System.Windows.Forms.DialogResult.Cancel:
                        result.Status = CommandStatus.Canceled;
                        return result;
                    case System.Windows.Forms.DialogResult.No:
                        quickSave = true;
                        break;
                }

            }

            ShellOMS app = FWBS.OMS.Apps.ApplicationManager.CurrentManager.GetApplicationInstance("SHELL", true) as ShellOMS;
            app.SetActiveWindow(owner);

            foreach (IStorageItem item in Docs)
            {

                IStorageItemLockable lockable = item as IStorageItemLockable;

                if (!lockable.IsCheckedOut)
                {
                    result.Errors.Add(new StorageItemNotCheckedOutException(item));
                    continue;
                }
                else if (!FWBS.Common.Functions.IsCurrentComputer(lockable.CheckedOutMachine))
                {
                    result.Errors.Add(new FileNotFoundException("Document was not checked out on this machine"));
                    continue;
                }

                FileInfo localFile = new FileInfo(lockable.CheckedOutLocation);

                SaveSettings settings = SaveSettings.Default;

                if (applyDefaultVersioning)
                {
                    VersionStoreSettings vSettings = settings.StorageSettings.GetSettings<VersionStoreSettings>();
                    if (vSettings != null)
                        vSettings.SaveItemAs = checkinOptions;
                }

                if (quickSave)
                    settings.Mode = PrecSaveMode.Quick;

                string errorMessage = "Document cannot be checked in when it is open";
                if (IsFileLocked(localFile) && Session.CurrentSession.DocumentLocking == "E")
                {
                    result.Errors.Add(new IOException(errorMessage));
                    return result;
                }

                try
                {
                    using (ShellFile sfile = new ShellFile(localFile))
                    {
                        app.AttachDocumentVars(sfile, item as OMSDocument, item as DocumentVersion);
                        app.CheckIn(sfile, settings);
                    }
                }
                catch (IOException)
                {
                    result.Errors.Add(new IOException(errorMessage));
                }
                catch (Exception ex)
                {
                    result.Errors.Add(ex);
                }
            }
            return result;

        }

        private bool IsFileLocked(FileInfo file)
        {
            FileStream stream = null;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException)
            {
                return true;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            
            return false;
        }
    }
}
