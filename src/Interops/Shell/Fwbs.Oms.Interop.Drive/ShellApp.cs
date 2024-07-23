using System;
using System.IO;

namespace FWBS.OMS.Drive
{
    using DocumentManagement.Storage;
    using UI.Windows;

    [System.Runtime.InteropServices.Guid("80B394E7-15C0-4819-A467-75CCD5292823")]
    class ShellApp : ShellOMS
    {
        private static readonly Interfaces.IOMSApp _appController = new ShellApp();

        public static FileInfo EditDocument(long docID)
        {
            FileInfo localFileInfo = null;
            try
            {
                OMSDocument doc = OMSDocument.GetDocument(docID);
                if (doc != null && doc.Accepted)
                {
                    DateTime? date;
                    IStorageItem item = doc.GetLatestVersion();
                    localFileInfo = StorageManager.CurrentManager.LocalDocuments.GetLocalFile(item, out date);
                    if (localFileInfo == null)
                    {
                        ShellFile localFile = _appController.Open(doc, DocOpenMode.Edit) as ShellFile;
                        if (localFile != null)
                        {
                            localFileInfo = localFile.File;
                        }
                        else
                        {
                            MessageBox.Show(Session.CurrentSession.Resources.GetMessage("4008", "The Document cannot be used with the specified mode %1%", "", DocOpenMode.Edit.ToString()));
                        }
                    }
                }
                else
                {
                    MessageBox.ShowInformation("DOCDELNOOPN", "The Document cannot be opened, it has been deleted");
                }
            }
            catch (CancelStorageException)
            {
            }
            catch (OMSException omsex)
            {
                if (omsex.HelpID != HelpIndexes.PasswordRequestCancelled)
                    MessageBox.Show(omsex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex);
            }
            return localFileInfo;
        }

        protected override object OpenFile(FileInfo file)
        {
            return new ShellFile(file, null);
        }

        protected override object InternalDocumentOpen(OMSDocument document, FetchResults fetchData, OpenSettings settings)
        {
            FileInfo file = fetchData.LocalFile;
            file.Refresh();

            if (!file.Exists)
                throw new FileNotFoundException(Session.CurrentSession.Resources.GetResource("SHELL_2", "File does not exist.", "").Text, file.FullName);

            return new ShellFile(file, null);
        }
    }
}
