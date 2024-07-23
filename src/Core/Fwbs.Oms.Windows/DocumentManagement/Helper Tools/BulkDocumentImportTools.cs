using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    public class BulkDocumentImportTools
    {
        public event EventHandler<AfterMultipleDocumentSaveArgs> AfterMultipleDocumentSave;

        private List<string> _files;
        private FWBS.Common.KeyValueCollection _kvc;
        private FWBS.OMS.UI.Windows.ShellOMS _appcontroller = new ShellOMS();

        private void OnAfterMultipleDocumentSave()
        {
            if (AfterMultipleDocumentSave != null)
                AfterMultipleDocumentSave(this, new AfterMultipleDocumentSaveArgs(_kvc));
        }
                
        
        public BulkDocumentImportTools(IEnumerable<string> selectedFiles, FWBS.OMS.UI.Windows.ShellOMS AppController)
        {
            _files = new List<string>(selectedFiles);
            _kvc = new Common.KeyValueCollection();
            _appcontroller = AppController;
        }


        #region "Bulk Document Save methods"


        public void SaveMultipleDocuments(long FileID, Guid folderGuid)
        {
            _kvc.Add("TargetFile", FWBS.OMS.OMSFile.GetFile(FileID));
            _kvc.Add("DocumentFolder", folderGuid);
            SaveMultipleDocuments();
        }


        public void SaveMultipleDocuments()
        {
            using (var contextBlock = ShellOMS.DpiAwareness > 0 ? new DPIContextBlock(ShellOMS.DpiAwareness) : null)
            {
                InitialiseDocumentSaveProcess();
            }
        }


        private void InitialiseDocumentSaveProcess()
        {
            List<ShellFile> shellFiles;
            if (CheckFilesForDocIDs(out shellFiles))
            {
                if (System.Windows.Forms.MessageBox.Show(
                     Session.CurrentSession.Resources.GetResource("BULKDOCSAVREV", "Some files have been saved previously in 3E MatterSphere\n\nPlease review all files to determine if you want to save them using the existing profile information, or want to change the profile information.\n\nWould you like to continue?", "").Text,
                     Session.CurrentSession.Resources.GetResource("BULKDOCREVCAP", "File Import", "").Text,
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }
            }
            if (shellFiles.Count > 0)
            {
                _kvc.Add("SelectedFiles", shellFiles);
                ExecuteMultipleDocumentSave();
                foreach (IDisposable sf in shellFiles)
                    sf.Dispose();
            }
        }


        private void ExecuteMultipleDocumentSave()
        {
            BulkDocumentImport bdi = new BulkDocumentImport();
            if (FWBS.OMS.EnquiryEngine.Enquiry.Exists(Session.CurrentSession.BulkImportWizard))
            {
                bdi = FWBS.OMS.UI.Windows.Services.Wizards.GetWizard(Session.CurrentSession.BulkImportWizard, bdi, _kvc) as BulkDocumentImport;
                if (bdi != null)
                {
                    SaveSettings saveSettings = new SaveSettings(true) { UseDefaultAssociate = true, Mode = PrecSaveMode.Quick, TargetAssociate = bdi.SelectedAssociate, FolderGuid = bdi.FolderGuid, AllowRelink = false };
                    _appcontroller.BulkDocumentProcessCancelled -= new EventHandler(_appcontroller_BulkDocumentProcessCancelled);
                    _appcontroller.BulkDocumentProcessCancelled += new EventHandler(_appcontroller_BulkDocumentProcessCancelled);

                    SaveBulkImportDocuments(bdi, saveSettings, DocumentSaveOptions.Unprofiled);
                    if (!BulkImportCancelled)
                        SaveBulkImportDocuments(bdi, saveSettings, DocumentSaveOptions.Profiled);
                    if (!BulkImportCancelled)
                        SaveBulkImportDocuments(bdi, saveSettings, DocumentSaveOptions.Exsiting);
                }
                else
                {
                    return;
                }
            }
            CheckForSaveFailure();
            OnAfterMultipleDocumentSave();
        }

        bool BulkImportCancelled;
        private void _appcontroller_BulkDocumentProcessCancelled(object sender, System.EventArgs e)
        {
            BulkImportCancelled = true;
        }


        private void SaveBulkImportDocuments(BulkDocumentImport bdi, SaveSettings saveSettings, DocumentSaveOptions saveOption)
        {
            switch (saveOption)
            {
                case DocumentSaveOptions.Unprofiled:

                    SaveDocumentsToCurrentMatter(bdi, bdi.UnProfiledDocumentsToSave, saveSettings, saveOption);
                    break;

                case DocumentSaveOptions.Profiled:

                    SaveDocumentsToCurrentMatter(bdi, bdi.ProfiledDocumentsToSaveAsNew, saveSettings, saveOption);
                    break;

                case DocumentSaveOptions.Exsiting:

                    saveSettings.UseExistingAssociate = true;
                    saveSettings.TargetAssociate = null;
                    saveSettings.FolderGuid = Guid.Empty;
                    bdi.FolderGuid = Guid.Empty;
                    SaveDocumentsToCurrentMatter(bdi, bdi.ProfiledDocumentsToSaveWithExistingProfileInformation, saveSettings, saveOption);
                    break;
            }
        }

        private void SaveDocumentsToCurrentMatter(BulkDocumentImport bdi, System.Collections.IEnumerable documents, SaveSettings saveSettings, DocumentSaveOptions saveOption)
        {
            if (documents.Any())
            {
                _appcontroller.BulkDocumentProcessCompleted -= new EventHandler<OMS_Applications.BulkDocumentProcessCompletedArgs>(_appcontroller_BulkDocumentProcessCompleted);
                _appcontroller.BulkDocumentProcessCompleted += new EventHandler<OMS_Applications.BulkDocumentProcessCompletedArgs>(_appcontroller_BulkDocumentProcessCompleted);
                if (saveOption == DocumentSaveOptions.Profiled || saveOption == DocumentSaveOptions.Unprofiled)
                {
                    _appcontroller.SaveAs(documents.ToArray<ShellFile>(), saveSettings, bdi);
                }
                else
                {
                    _appcontroller.Save(documents.ToArray<ShellFile>(), saveSettings, bdi);
                }
            }
        }

        string failresponse = "";
        private void _appcontroller_BulkDocumentProcessCompleted(object sender, OMS_Applications.BulkDocumentProcessCompletedArgs e)
        {
            int i = 0;
            foreach (var item in e.Exceptions)
            {
                failresponse += String.Format(
                    Session.CurrentSession.Resources.GetResource("DOCSAVEERRFILE", "Error in Saving ", "").Text
                    + "{0}\r\n" +
                    Session.CurrentSession.Resources.GetResource("DOCSAVEERREXCP", "Reason: ", "").Text
                    + "{1}\r\n\r\n", 
                    e.ErrorFiles[i], item.Message
                );
                i++;
            }
        }


        private void CheckForSaveFailure()
        {
            if (!string.IsNullOrWhiteSpace(failresponse))
            {
                if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("FAILBOX", "Errors have occured. Do you wish to view them?") == DialogResult.Yes)
                {
                    DisplaySaveFailureFeedback();
                }
            }
            else
            {
                if(!BulkImportCancelled)
                    DisplaySaveSuccessFeedback();
            }
        }


        private void DisplaySaveFailureFeedback()
        {
            using (Dialogs.frmMultiLineText f = new Dialogs.frmMultiLineText(
                Session.CurrentSession.Resources.GetResource("DOCSAVEERRSFCAP", "Document Save Errors", "").Text,
                Session.CurrentSession.Resources.GetResource("DOCSAVEERRSLCAP", "The files listed below failed to save correctly.", "").Text,
                failresponse))
            {
                f.ShowDialog();
            }
        }


        private void DisplaySaveSuccessFeedback()
        {
            System.Windows.Forms.MessageBox.Show(
                Session.CurrentSession.Resources.GetResource("BULKDOCSUCCESS", "All files successfully transferred to 3E MatterSphere", "").Text,
                Session.CurrentSession.Resources.GetResource("BULKDOCREVCAP", "File Import", "").Text,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }


        public enum DocumentSaveOptions
        {
            Unprofiled,
            Profiled,
            Exsiting
        }


        private bool CheckFilesForDocIDs(out List<ShellFile> shellFiles)
        {
            bool ret = false;
            shellFiles = new List<ShellFile>();
            FWBS.OMS.UI.Windows.ShellOMS appcontroller = new FWBS.OMS.UI.Windows.ShellOMS();
            FWBS.OMS.Apps.ApplicationManager.CurrentManager.InitialiseInstance("SHELL", appcontroller);
            foreach (string file in _files)
            {
                try
                {
                    ShellFile sf = new ShellFile(new FileInfo(file), null);
                    sf.Name = sf.File.FullName;
                    shellFiles.Add(sf);
                    if (CheckForDocIDVariable(appcontroller, sf) && CheckDocForCompanyID(appcontroller, sf))
                        ret = true;
                }
                catch (Security.SecurityException ex)
                {
                    if (ex.HelpID == HelpIndexes.PasswordRequestCancelled)
                        ErrorBox.Show(ex);
                    else
                        throw;
                }
            }
            return ret;
        }

        public static bool CheckDocForCompanyID(ShellOMS appcontroller, ShellFile sf)
        {
            return (appcontroller.GetDocVariable(sf, OMSApp.COMPANY, "-1") == Session.CurrentSession.CompanyID.ToString());
        }

        public static bool CheckForDocIDVariable(ShellOMS appcontroller, ShellFile sf)
        {
            return appcontroller.HasDocVariable(sf, OMSApp.DOCUMENT);
        }


        public class AfterMultipleDocumentSaveArgs : EventArgs
        {
            private FWBS.OMS.OMSFile _file;
            private Guid _folderGuid;
            private List<string> _selectedFiles;
            private string _errorFiles;

            public FWBS.OMS.OMSFile File 
            { 
                get
                {
                    return _file;
                }
            }

            public Guid FolderGuid 
            { 
                get
                {
                    return _folderGuid;
                }
            }

            public List<string> SelectedFiles
            {
                get
                {
                    return _selectedFiles;
                }
            }


            public string ErrorFiles
            {
                get
                {
                    return _errorFiles;
                }
            }


            public AfterMultipleDocumentSaveArgs(FWBS.Common.KeyValueCollection kvc)
            {
                if(kvc.Contains("TargetFile"))
                    _file = kvc["TargetFile"].Value as FWBS.OMS.OMSFile;
                if (kvc.Contains("DocumentFolder"))
                    _folderGuid = (Guid)kvc["DocumentFolder"].Value == Guid.Empty ? Guid.Empty : (Guid)kvc["DocumentFolder"].Value;
                if (kvc.Contains("SelectedFiles"))
                    _selectedFiles = (kvc["SelectedFiles"].Value as List<ShellFile>).Select(sf => sf.Name).ToList();
            }

        }


        #endregion
    }
}
