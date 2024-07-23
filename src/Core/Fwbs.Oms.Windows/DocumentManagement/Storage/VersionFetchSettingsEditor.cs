namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    using FWBS.OMS.DocumentManagement.Storage;

    public partial class VersionFetchSettingsEditor : StorageSettingsEditorBase
    {
        private string precVerCaption;

        public VersionFetchSettingsEditor()
        {
            CreatePrecedentVersionCodeLookups();
            InitializeComponent();
        }

        new protected VersionFetchSettings Settings
        {
            get
            {
                return (VersionFetchSettings)base.Settings;
            }
        }

        protected override void InternalInitialise()
        {
            if (StorageItem is FWBS.OMS.Precedent)
            {
                SetDisplayForPrecedentStorageItem();
            }
        }

        protected override void InternalRefreshSettings()
        {
            versions.StorageItem = StorageItem;
            bool edit = Settings.CanEdit;
            versions.Enabled = edit;
        }

        protected override void InternalApplySettings()
        {
            if (Settings.CanEdit)
            {
                IStorageItemVersion ver = versions.SelectedVersion;
                if (ver != null)
                {
                    Settings.Version = VersionFetchSettings.FetchAs.Specific;
                    Settings.VersionLabel = ver.Label;
                }
            }
        }
        
        #region Methods
        
        private string CreateResourceCodeLookup(string Code, string Description)
        {
            return Session.CurrentSession.Resources.GetResource(Code, Description, "").Text;
        }

        private void CreatePrecedentVersionCodeLookups()
        {
            precVerCaption = Session.CurrentSession.Resources.GetResource("LBLVERFSETPREC", "Please specify the particular version of the precedent you would like to open.", "").Text;
        }

        private void SetDisplayForPrecedentStorageItem()
        {
            //Needs to be set twice due to legacy issue of resource lookup item
            SetPrecedentText();
            SetPrecedentText();
        }

        private void SetPrecedentText()
        {
            this.label1.Text = precVerCaption;
        }

        #endregion


    }
}
