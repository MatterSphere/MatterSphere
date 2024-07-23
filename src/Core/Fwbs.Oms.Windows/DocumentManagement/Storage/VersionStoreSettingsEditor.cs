using System;
using System.ComponentModel;
using System.Drawing;

namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    using FWBS.OMS.DocumentManagement.Storage;

    public partial class VersionStoreSettingsEditor : StorageSettingsEditorBase
    {
        #region Variables

        private IStorageItemVersionable versionable;
        private IStorageItemVersion workingversion;
        private int MaxrevisionCount = FWBS.OMS.Session.CurrentSession.DocumentMaximumRevisionCount;

        private string newPrecedentText;
        private string precedentDesc;
        private string precedentOverwrite;
        private string precedentMajorVersion;
        private string precedentNewSubVersion;
        private string precedentNewVersion;

        private bool chkcurrentset;
        private bool initiallylatest;
        PrecedentVersionRestorer restorer;
        private IStorageItemVersion originallatestver;

        #endregion
        
        public VersionStoreSettingsEditor()
        {
            CreatePrecedentVersionCodeLookups();
            InitializeComponent();
        }
        
        new protected VersionStoreSettings Settings
        {
            get
            {
                return (VersionStoreSettings)base.Settings;
            }
        }


        #region Methods

        protected override void InternalInitialise()
        {
            pnlNewVersion.Visible = false;
            pnlNew.Visible = false;

            if (StorageItem.IsNew)
            {
                pnlNewVersion.Visible = false;
                pnlNew.Visible = true;
            }
            else
            {
                versionable = StorageItem as IStorageItemVersionable;
                originallatestver = versionable.GetLatestVersion();

                if (versionable != null)
                {
                    workingversion = versionable.GetWorkingVersion();
                    pnlNewVersion.Visible = true;
                    pnlNew.Visible = false;
                }
                SetEditState();
                SetVersionCaptions();
            }

            if (StorageItem is FWBS.OMS.Precedent)
            {
                SetDisplayForPrecedentStorageItem();
            }
        }

        protected override void InternalRefreshSettings()
        {

            switch (Settings.SaveItemAs)
            {
                case VersionStoreSettings.StoreAs.NewMajorVersion:
                    rdoMajorVersion.Checked = true;
                    break;
                case VersionStoreSettings.StoreAs.NewSubVersion:
                    rdoNewSubVersion.Checked = true;

                    if (!(StorageItem is Precedent))
                    {
                        if (!(workingversion == null))
                        {
                            if ((workingversion.Label.Split('.').Length - 1) >= MaxrevisionCount)
                            {
                                rdoNewSubVersion.Checked = false;
                                rdoNewSubVersion.Enabled = false;
                                rdoMajorVersion.Enabled = true;
                                rdoMajorVersion.Checked = true;

                            }
                        }
                    }
                    break;
                case VersionStoreSettings.StoreAs.NewVersion:
                    rdoNewVersion.Checked = true;
                    break;
                case VersionStoreSettings.StoreAs.OriginalOverwrite:
                    rdoOverwrite.Checked = true;
                    break;
            }
            chkCurrent.Checked = Settings.MarkAsLatest;
            initiallylatest = Settings.MarkAsLatest;
            chkcurrentset = true;
            txtComments.Text = Settings.Comments;
            cboStatus.Value = Settings.Status;

            if (Settings.SaveItemAs == VersionStoreSettings.StoreAs.OriginalOverwrite)
            {
                if (txtComments.Text.Trim() == String.Empty && workingversion != null)
                {
                    txtComments.Text = workingversion.Comments;
                    rdoOverwrite.Tag = txtComments.Text;
                }
            }
        }

        protected override void InternalApplySettings()
        {
            if (Settings.CanEdit)
            {
                if (rdoMajorVersion.Checked)
                {
                    Settings.SaveItemAs = VersionStoreSettings.StoreAs.NewMajorVersion;
                }
                else if (rdoOverwrite.Checked)
                {
                    Settings.SaveItemAs = VersionStoreSettings.StoreAs.OriginalOverwrite;
                }
                else if (rdoNewVersion.Checked)
                {
                    Settings.SaveItemAs = VersionStoreSettings.StoreAs.NewVersion;
                }
                else if (rdoNewSubVersion.Checked)
                {
                    Settings.SaveItemAs = VersionStoreSettings.StoreAs.NewSubVersion;
                }
                Settings.Comments = txtComments.Text;
                Settings.MarkAsLatest = chkCurrent.Checked;
                Settings.Status = Convert.ToString(cboStatus.Value);
            }
        }


        private void SetDisplayForPrecedentStorageItem()
        {
            ToggleDisplayOfFlagAsLatestVersionCheckbox();
            //As stated in the SetVersionCaptions method, there is a bug with ResourceLookup so text has to be set twice
            SetPrecedentText();
            SetPrecedentText();
            this.cboStatus.Visible = false;
            this.chkCurrent.Location = new Point(14, 79);
        }
        
        private void ToggleDisplayOfFlagAsLatestVersionCheckbox()
        {
            if (StorageItemIsNewPrecedent())
            {
                this.chkCurrent.Visible = false;
                this.chkCurrent.Checked = true;
            }
        }

        private bool StorageItemIsNewPrecedent()
        {
            return ((FWBS.OMS.Precedent)StorageItem).ID == 0;
        }
        
        private void SetPrecedentText()
        {
            this.label5.Text = newPrecedentText;
            this.label4.Text = precedentDesc;
            this.lblOverwrite.Text = precedentOverwrite;
            this.lblMajorVersion.Text = precedentMajorVersion;
            this.lblNewSubVersion.Text = precedentNewSubVersion;
            this.lblNewVersion.Text = precedentNewVersion;
        }

        private string CreateResourceCodeLookup(string Code, string Description)
        {
            return Session.CurrentSession.Resources.GetResource(Code, Description, "").Text;
        }

        private void CreatePrecedentVersionCodeLookups()
        {
            newPrecedentText = Session.CurrentSession.Resources.GetResource("NEWPRECEDENT", "New Precedent", "").Text;
            precedentDesc = Session.CurrentSession.Resources.GetResource("NEWPRECDESC", "This is a new precedent and will be stored as version 1.", "").Text;
            precedentOverwrite = Session.CurrentSession.Resources.GetResource("LBLOVERWRITEPRC", "Warning! The precedent is going to overwrite the current working precedent.", "").Text;
            precedentMajorVersion = Session.CurrentSession.Resources.GetResource("LBLMAJVERPREC", "The precedent is going to be saved as the next available major version.", "").Text;
            precedentNewSubVersion = Session.CurrentSession.Resources.GetResource("LBLNEWSBVERPREC", "The precedent is going to be saved as a new sub version under the current working precedent.", "").Text;
            precedentNewVersion = Session.CurrentSession.Resources.GetResource("LBLNEWVERPREC", "The precedent is going to be saved as a new version of the current working precedent.", "").Text;
        }


        private void SetEditState()
        {
            bool edit = Settings.CanEdit;           
            if (edit == false)
            {
                rdoMajorVersion.Enabled = false;
                rdoNewVersion.Enabled = false;
                
                rdoNewSubVersion.Enabled = false;
                rdoOverwrite.Enabled = false;
                chkCurrent.Enabled = false;
                txtComments.Enabled = false;
                cboStatus.Enabled = false;
            }
            else
            {
                rdoMajorVersion.Enabled = true;
                rdoNewVersion.Enabled = true;
                rdoNewSubVersion.Enabled = true;

                if (!(StorageItem is Precedent))
                {
                    if (!(workingversion == null))                
                    {
                   
                        if ((workingversion.Label.Split('.').Length - 1) >= MaxrevisionCount)
                        {
                            rdoNewSubVersion.Checked = false;
                            rdoNewSubVersion.Enabled = false;                     
                            rdoMajorVersion.Enabled = true;
                            rdoMajorVersion.Checked = true;

                        }
                    }         
                }

                rdoOverwrite.Enabled = Settings.CanOverwrite;
                chkCurrent.Enabled = true;
                txtComments.Enabled = true;
                cboStatus.Enabled = Settings.CanChangeStatus;
            }
        }

        private void SetVersionCaptions()
        {
            if (workingversion != null)
            {
                //Duplicate the text due to the bug with ResourceLookup
                string[] parent_split = workingversion.Label.Split('.');
                rdoMajorVersion.Text = String.Format("{0} - x", rdoMajorVersion.Text);
                rdoMajorVersion.Text = String.Format("{0} - x", rdoMajorVersion.Text);
                rdoNewVersion.Text = String.Format("{0} - {1}.x", rdoNewVersion.Text, String.Join(".", parent_split, 0, parent_split.Length - 1));
                rdoNewVersion.Text = String.Format("{0} - {1}.x", rdoNewVersion.Text, String.Join(".", parent_split, 0, parent_split.Length - 1));
                rdoNewVersion.Visible = (parent_split.Length > 1);           
                rdoNewSubVersion.Text = String.Format("{0} - {1}.x", rdoNewSubVersion.Text, workingversion.Label);
                rdoNewSubVersion.Text = String.Format("{0} - {1}.x", rdoNewSubVersion.Text, workingversion.Label);                 
                rdoOverwrite.Text = String.Format("{0} - {1}", rdoOverwrite.Text, workingversion.Label);
                rdoOverwrite.Text = String.Format("{0} - {1}", rdoOverwrite.Text, workingversion.Label);

            }
        }

        #endregion
        
        #region Events
        private void rdoNewVersion_CheckedChanged(object sender, EventArgs e)
        {
            lblMajorVersion.Visible = (rdoMajorVersion== sender);
            lblNewSubVersion.Visible = (rdoNewSubVersion == sender);
            lblNewVersion.Visible = (rdoNewVersion == sender);
            lblOverwrite.Visible = (rdoOverwrite == sender);

            if (sender == rdoMajorVersion)
                txtComments.Text = (string)rdoMajorVersion.Tag ?? String.Empty;
            else if (sender == rdoNewVersion)
                txtComments.Text = (string)rdoNewVersion.Tag ?? String.Empty;
            else if (sender == rdoNewSubVersion)
                txtComments.Text = (String)rdoNewSubVersion.Tag ?? String.Empty;
            else if (sender == rdoOverwrite)
                txtComments.Text = (string)rdoOverwrite.Tag ?? String.Empty;
        }

        private void txtComments_TextChanged(object sender, EventArgs e)
        {
            if (rdoMajorVersion.Checked)
                rdoMajorVersion.Tag = txtComments.Text;
            else if (rdoNewVersion.Checked)
                rdoNewVersion.Tag = txtComments.Text;
            else if (rdoNewSubVersion.Checked)
                rdoNewSubVersion.Tag = txtComments.Text;
            else if (rdoOverwrite.Checked)
                rdoOverwrite.Tag = txtComments.Text;

        }

        private void chkCurrent_CheckedChanged(object sender, EventArgs e)
        {
            if (workingversion is FWBS.OMS.DocumentManagement.PrecedentVersion && chkcurrentset && chkCurrent.Checked && !initiallylatest)
            {
                restorer = new PrecedentVersionRestorer( workingversion, originallatestver, true);
                restorer.CheckForScriptRestoration();
            }
        }

        internal protected override void OnFinishing(EnquiryForm screen, CancelEventArgs args)
        {
            if (!args.Cancel)
            {
                if (restorer != null && rdoOverwrite.Checked)
                {
                    if (!initiallylatest && chkCurrent.Checked)
                        restorer.CreatePrecedentRestorationAuditRecord();
                    if (restorer.RestoreScript)
                        restorer.RestorePrecedentScriptVersion();
                }
            }
        }
        #endregion
    }
}
