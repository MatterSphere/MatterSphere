using System;
using System.IO;
using FWBS.OMS.DocumentManagement;

namespace FWBS.OMS.UI.Windows.DocumentManagement
{
    public sealed class ImportDocumentCommand : FWBS.OMS.DocumentManagement.ImportDocumentCommand
    {

        private System.Windows.Forms.IWin32Window owner;
        public System.Windows.Forms.IWin32Window Owner
        {
            set { owner = value; }
        }

        private SaveSettings settings;
        public SaveSettings Settings
        {
          set { settings = value; }
        }

        private bool autoConfirmFile;
        public bool AutoConfirmFile
        {
            set { autoConfirmFile = value; }
        }

        private OMSFile toFile;
        public OMSFile ToFile
        {
            set{ toFile = value; }
        }


        public override FWBS.OMS.Commands.ExecuteResult Execute()
        {
            FWBS.OMS.Commands.ExecuteResult result = new FWBS.OMS.Commands.ExecuteResult(FWBS.OMS.Commands.CommandStatus.Canceled);
            
            OMSDocument doc = parent as OMSDocument;
            DocumentVersion version = parent as DocumentVersion;

            if (parent != null)
            {
                if (version != null)
                    doc = version.ParentDocument;
                else if (doc != null)
                    version = doc.GetLatestVersion() as DocumentVersion;
                else
                    throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("MSGINVDCTP", "Invalid Document Type", "").Text);
            }

            if (AllowUI && string.IsNullOrEmpty(fileName))
            {
                using (System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog())
                {
                    if (parent != null)
                        dialog.Filter = GetFileFilter(parent.Extension);

                    dialog.Title = ResourceLookup.GetLookupText("IMPORT", "Import", "");
                    if (doc != null)
                        dialog.Title += " " + doc.Description;

                    if (dialog.ShowDialog(owner) == System.Windows.Forms.DialogResult.Cancel)
                        return result;

                    fileName = dialog.FileName;
                }
            }

            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            else if (!File.Exists(fileName))
                throw new FileNotFoundException(fileName);
            else if (string.IsNullOrEmpty(Path.GetExtension(fileName)))
                throw new FileFormatException();

            if (settings == null)
                settings = SaveSettings.Default;

            settings.AllowContinueEditing = false;

            ShellOMS app = FWBS.OMS.Apps.ApplicationManager.CurrentManager.GetApplicationInstance("SHELL", true) as ShellOMS;
            app.SetActiveWindow(owner);
           
            using (ShellFile sfile = new ShellFile(new System.IO.FileInfo(fileName)))
            {
                if (parent == null &&
                    BulkDocumentImportTools.CheckForDocIDVariable(app, sfile) &&
                    BulkDocumentImportTools.CheckDocForCompanyID(app, sfile) &&
                    UserWantsToUseExistingProfileInformation())
                {
                    string additionalSaveCommands = Session.CurrentSession.CurrentUser.AdditionalDocumentSaveCommands;
                    if (string.IsNullOrEmpty(additionalSaveCommands))
                        additionalSaveCommands = Session.CurrentSession.AdditionalDocumentSaveCommands;

                    if (additionalSaveCommands.Contains("QUICK"))
                        settings.Mode = PrecSaveMode.Quick;
                }
                else
                {
                    if (toAssociate == null && parent == null)
                    {
                        SelectAssociate assocPicker = (toFile == null) ? new SelectAssociate() : new SelectAssociate(toFile);
                        assocPicker.AutoConfirm = autoConfirmFile;
                        toAssociate = assocPicker.Show(owner);

                        if (toAssociate == null || toAssociate == Associate.Private)
                            return result;
                    }

                    app.DettachDocumentVars(sfile);
                    if (parent != null)
                        app.AttachDocumentVars(sfile, doc, version);
                    else
                        app.AttachDocumentVars(sfile, false, toAssociate);
                }

                app.Save(sfile, settings);
            }

            result.Status = FWBS.OMS.Commands.CommandStatus.Success;
            return result;
        }

        private bool UserWantsToUseExistingProfileInformation()
        {
            return MessageBox.Show(owner,
                       Session.CurrentSession.Resources.GetResource("UCBUILTIN_2", "The document has been saved already, would you like to save it using the existing profile information?", "").Text,
                       Session.CurrentSession.Resources.GetResource("UCBUILTIN_3", "Document Save", "").Text,
                       System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes;
        }

        private string GetFileFilter(string extension)
        {
            string filter;
            switch (extension.ToLowerInvariant())
            {
                case "doc":
                case "docx":
                    filter = "|*.doc;*.docx";
                    break;
                case "xls":
                case "xlsx":
                    filter = "|*.xls;*.xlsx";
                    break;
                case "ppt":
                case "pptx":
                    filter = "|*.ppt;*.pptx";
                    break;
                case "jpg":
                case "jpeg":
                    filter = "|*.jpg;*.jpeg";
                    break;
                case "tif":
                case "tiff":
                    filter = "|*.tif;*.tiff";
                    break;
                default:
                    filter = "|*." + extension;
                    break;
            }

            if (Convert.ToInt32(Microsoft.Win32.Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Explorer\Advanced", "HideFileExt", 0)) != 0)
                filter = string.Format("({0}){1}", filter.TrimStart('|'), filter);

            return filter;
        }
    }
}
