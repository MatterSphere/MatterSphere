using System;

namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    using FWBS.OMS.DocumentManagement.Storage;

    public partial class LockableFetchSettingsEditor : StorageSettingsEditorBase
    {
        private IStorageItemLockable lockable;

        public LockableFetchSettingsEditor()
        {
            InitializeComponent();
        }

        new protected LockableFetchSettings Settings
        {
            get
            {
                return (LockableFetchSettings)base.Settings;
            }
        }

        protected override void InternalInitialise()
        {
            
        }

        protected override void InternalRefreshSettings()
        {
            chkCheckOut.Checked = false;
            chkCheckOut.Enabled = false;

            lockable = StorageItem.GetStorageProvider().GetLockableItem(StorageItem);

            if (lockable != null)
            {
                if (lockable.IsCheckedOut)
                {
                    User user = lockable.CheckedOutBy;
                    pnlAlreadyLocked.Visible = true;


                    //Duplicate text due to ResourceLookup bug
                    lblCheckedOutBy.Text = String.Format("{0} - {1}", user.FullName, lockable.CheckedOutTime.Value);
                    lblCheckedOutBy.Text = String.Format("{0} - {1}", user.FullName, lockable.CheckedOutTime.Value);

                }
                else
                {
                    pnlAlreadyLocked.Visible = false;
                    lblCheckedOutBy.Text = String.Empty;
                    chkCheckOut.Enabled = true;
                    chkCheckOut.Checked = StorageItem.GetStorageProvider().GetCheckoutOption(StorageItem);
                }

            }
        }

        protected override void InternalApplySettings()
        {
            if (Settings.CanEdit)
            {
                if (chkCheckOut.Enabled)
                    Settings.CheckOut = chkCheckOut.Checked;
            }
        }

    }
}
