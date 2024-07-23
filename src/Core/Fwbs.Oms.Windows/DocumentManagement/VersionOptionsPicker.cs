using System;
using System.Windows.Forms;
using FWBS.OMS.DocumentManagement.Storage;

namespace FWBS.OMS.UI.DocumentManagement
{
    public partial class VersionOptionsPicker : FWBS.OMS.UI.Windows.BaseForm
    {
        public enum VersionPicked
        {
            NewDocument,
            NewMajorVersion,
            NewVersion,
            NewSubVersion,
            Overwrite,
        }

        private VersionPicked pickedVersion;

        public VersionPicked PickedVersion
        {
            get { return pickedVersion; }
        }

        public VersionOptionsPicker() : base()
        {
            InitializeComponent();
        }

        public bool Render(IStorageItem item)
        {
            if (item.IsNew)
                return false; //should i be throwing an exception here?!

            SupportedStorageFeatures features = item.GetStorageProvider().Supports(item);

            string[] displayID = item.DisplayID.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            int  MaxRevisionCount = Session.CurrentSession.DocumentMaximumRevisionCount;
            
            if(displayID.Length==2)
                SetupScreen(false, features.CreateSubVersion, features.AllowOverwrite);
            else
                SetupScreen(features.CreateVersion, features.CreateSubVersion, features.AllowOverwrite);

            StorageSettingsCollection  sSettings = item.GetSettings();

            if (sSettings == null)
                sSettings = item.GetStorageProvider().GetDefaultSettings(item, SettingsType.Store);

            VersionStoreSettings versionSettings = sSettings.GetSettings<VersionStoreSettings>();

            if (versionSettings == null)
                return false;

            
            if (displayID.Length > 1)
            {
                label2.Visible = true;
                lblVersion.Visible = true;
                lblDocID.Text = displayID[0];

                string versionStamp = "";
                for (int i = 1; i < displayID.Length; i++)
                {
                    if (versionStamp != "")
                        versionStamp += ".";
                    versionStamp += displayID[i];

                }

                lblVersion.Text = versionStamp;
            }
            else
                lblDocID.Text = item.DisplayID;

            OMSDocument doc = item as OMSDocument;
            if (doc == null)
            {
                FWBS.OMS.DocumentManagement.DocumentVersion vers = item as FWBS.OMS.DocumentManagement.DocumentVersion;

                if (vers == null)
                    throw new NotSupportedException("Unsuported Document Type");

                doc = vers.ParentDocument;

            }

            lblDesc.Text = doc.Description; //whats this going to show :S

            //O = Overwrite (Standard)
            //N = Creates a brand new document (Save As);
            //V = Automatically creates new version based on the current level version.
            //M = Creates a new root/major version.
            //I = Dont use this seetting
            switch (Session.CurrentSession.DefaultOptionOnResaveSaveAs)
            {
                case "O":
                    {
                        rdoOverwrite.Checked = true;
                        break;
                    }
                case "N":
                    {
                        rdoNewDoc.Checked = true;
                        break;
                    }
                case "M":
                    {
                        rdoMajorVersion.Checked = true;
                        break;
                    }
                case "V":
                    {
                        rdoNewSubVersion.Checked = true;
                        break;
                    }
                    
                case "I":
                    {
                        switch (versionSettings.SaveItemAs)
                        {
                            case VersionStoreSettings.StoreAs.NewMajorVersion:
                                rdoMajorVersion.Checked = true;
                                break;
                            case VersionStoreSettings.StoreAs.NewVersion:
                                rdoNewVersion.Checked = true;
                                break;
                            case VersionStoreSettings.StoreAs.NewSubVersion:
                                rdoNewSubVersion.Checked = true;
                                break;
                            case VersionStoreSettings.StoreAs.OriginalOverwrite:
                                rdoOverwrite.Checked = true;
                                break;
                        }

                        break;
                    }
            }

            if (!features.CreateVersion && !features.CreateSubVersion && !features.AllowOverwrite)
            {
                pickedVersion = VersionPicked.NewDocument;
                rdoNewDoc.Checked = true;
                return false;
            }
           
            if ((displayID.Length - 2) >= MaxRevisionCount)
            {
                rdoNewSubVersion.Enabled = false;
                if (rdoNewSubVersion.Checked)
                {
                    rdoNewVersion.Checked = true;
                }

            }


            return true;
        }

        private void SetupScreen(bool showNewVersion, bool showNewSubVersion, bool showOverwrite)
        {
            int subtract = 0;
            if (!showNewVersion)
            {
                subtract += rdoNewVersion.Height;
                rdoNewVersion.Visible = false;
            }
            if (!showNewSubVersion)
            {
                subtract += rdoNewSubVersion.Height;
                rdoNewSubVersion.Visible = false;
            }
            if (!showOverwrite)
            {
                subtract += rdoOverwrite.Height;
                rdoOverwrite.Visible = false;
            }

            if (subtract > 0)
            {
                this.Height -= subtract;
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;

            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

           if (rdoNewDoc.Checked)
                pickedVersion = VersionPicked.NewDocument;
            else if (rdoMajorVersion.Checked)
                pickedVersion = VersionPicked.NewMajorVersion;
           else if (rdoNewVersion.Checked)
               pickedVersion = VersionPicked.NewVersion;
           else if (rdoNewSubVersion.Checked)
               pickedVersion = VersionPicked.NewSubVersion;
            else
                pickedVersion = VersionPicked.Overwrite;

            this.DialogResult = DialogResult.OK;

            this.Close();
        }

    }
}
