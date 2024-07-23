using System;

namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    using FWBS.OMS.DocumentManagement.Storage;

    public partial class LockableStoreSettingsEditor : StorageSettingsEditorBase
    {
        private IStorageItemLockable lockable;

        public LockableStoreSettingsEditor()
        {
            InitializeComponent();
        }

        new protected LockableStoreSettings Settings
        {
            get
            {
                return (LockableStoreSettings)base.Settings;
            }
        }

        protected override void InternalInitialise()
        {            
        }

        protected override void InternalRefreshSettings()
        {
            chkCheckIn.Checked = false;
            chkCheckIn.Enabled = false;

            lockable = StorageItem.GetStorageProvider().GetLockableItem(StorageItem);

            if (lockable != null)
            {
                if (lockable.IsCheckedOut)
                {
                    User user = lockable.CheckedOutBy;
                    if (user.ID == Session.CurrentSession.CurrentUser.ID)
                    {
                        chkCheckIn.Enabled = true;
                        chkCheckIn.Checked = Settings.CheckIn;
                    }

                    pnlAlreadyLocked.Visible = true;

                    //Set Text twice due to ResourceLookup bug
                    lblCheckedOutBy.Text = String.Format("{0} - {1}", user.FullName, lockable.CheckedOutTime.Value);
                    lblCheckedOutBy.Text = String.Format("{0} - {1}", user.FullName, lockable.CheckedOutTime.Value);
                    
                }
                else
                {
                    if (StorageItem.IsNew)
                    {
                        chkCheckIn.Enabled = true;
                        chkCheckIn.Checked = Settings.CheckIn;
                    }

                    pnlAlreadyLocked.Visible = false;
                    lblCheckedOutBy.Text = String.Empty;
                }
            }
        }

        protected override void InternalApplySettings()
        {
            if (Settings.CanEdit)
            {
                if (chkCheckIn.Enabled)
                    Settings.CheckIn = chkCheckIn.Checked;
            }
        }

      

    }
}
