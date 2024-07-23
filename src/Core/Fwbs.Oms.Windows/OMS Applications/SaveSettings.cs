using System;

namespace FWBS.OMS.UI.Windows
{
    public class SaveSettings
    {
        public bool AllowMove {get;set;}

        public bool Move { get; set; }

        private readonly PrintSettings printing;
        public PrintSettings Printing
        {
            get
            {
                return printing;
            }
        }

        public PrecSaveMode Mode{get;set;}
        public bool UseDefaultAssociate {get;set;}
        public bool ContinueEditing {get;set;}
        public bool ContinueOnError {get;set;}
        public string DocumentDescription{get;set;}
        public bool AllowContinueEditing {get;set;}
        public bool AllowRelink { get; set; }
        public bool UsePreviousAssoicate { get; set; }
        public Associate PreviousAssociate { get; set; }

        public bool UseExistingAssociate { get; set; }
        public Associate TargetAssociate { get; set; }
        public Guid FolderGuid { get; set; }
        public bool SkipTimeRecords { get; set; }

        private FWBS.OMS.DocumentManagement.Storage.StorageSettingsCollection storageSettings;
        public FWBS.OMS.DocumentManagement.Storage.StorageSettingsCollection StorageSettings
        {
            get 
            {
                if (storageSettings == null)
                    storageSettings = new FWBS.OMS.DocumentManagement.Storage.StorageSettingsCollection();

                return storageSettings; 
            }
        }

        public SaveSettings(bool BulkPrint)
        {
            printing = BulkPrint ? PrintSettings.BulkPrint : PrintSettings.Default;

            Mode = PrecSaveMode.Save;
            UseDefaultAssociate = false;
            ContinueOnError = false;
            ContinueEditing = false;
            AllowContinueEditing = true;
            AllowMove = true;
            Move = true;
            AllowRelink = true;
                
        }


        public SaveSettings()
        {
            printing = PrintSettings.Default;
            Mode = PrecSaveMode.Save;
            UseDefaultAssociate = false;
            ContinueOnError = false;
            ContinueEditing = false;
            AllowContinueEditing = true;
            AllowMove = true;
            Move = true;
            AllowRelink = true;
        }

        public static SaveSettings Default
        {
            get
            {
                SaveSettings settings = new SaveSettings();

                if ((Session.CurrentSession.CurrentUser.AutoPrint == FWBS.Common.TriState.Null && Session.CurrentSession.AutoPrint == false) || Session.CurrentSession.CurrentUser.AutoPrint == FWBS.Common.TriState.False)
                    settings.Printing.Mode = PrecPrintMode.None;
                else
                    settings.Printing.Mode = PrecPrintMode.Dialog;

                if ((Session.CurrentSession.CurrentUser.UseDefaultAssociate == FWBS.Common.TriState.Null && Session.CurrentSession.UseDefaultAssociate == false) || Session.CurrentSession.CurrentUser.UseDefaultAssociate == FWBS.Common.TriState.False)
                    settings.UseDefaultAssociate = false;
                else
                    settings.UseDefaultAssociate = true;

                settings.ContinueEditing = Session.CurrentSession.CurrentUser.ContinueEditingAfterSave;

                return settings;
            }
        }
    }
}
