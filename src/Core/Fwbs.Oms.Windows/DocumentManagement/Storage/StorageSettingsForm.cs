using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    using System.Reflection;
    using FWBS.OMS.DocumentManagement.Storage;

    internal partial class StorageSettingsForm : frmNewBrandIdent
    {
        private bool fromadminkit;
        private string precedentDialogCaption;
        private IStorageItem _storageItem;
        private SettingsType _settingsType;

        public StorageSettingsForm(IStorageItem storageItem, SettingsType settingsType)
        {
            precedentDialogCaption = CreateResourceCodeLookup("PRECVERRETSET", "Precedent Retrieval Settings");
            _storageItem = storageItem;
            _settingsType = settingsType;
            InitializeComponent();
        }

        public StorageSettingsForm(IStorageItem storageItem, SettingsType settingsType, bool FromAdminKit) : this(storageItem, settingsType)
        {
            fromadminkit = FromAdminKit;
        }

        [Browsable(false)]
        public IStorageItem StorageItem
        {
            get
            {
                return settings.StorageItem;
            }
            set
            {
                settings.StorageItem = value;
            }
        }

        [DefaultValue(SettingsType.Store)]
        public SettingsType SettingsType
        {
            get
            {
                return settings.SettingsType;
            }
            set
            {
                settings.SettingsType = value;
            }
        }


        public DialogResult ShowDialog(IWin32Window owner, string defaultSetting)
        {
            settings.SetSettingsPage(defaultSetting);

            if (settings.StorageItem is FWBS.OMS.Precedent)
            {
                this.Text = precedentDialogCaption;
            }

            return ShowDialog(owner);
        }
        
        #region Events

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                settings.ApplySettings();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                settings.CancelSettings();
                UnlockStorageItem();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(this, ex);
            }
        }

        #endregion
        
        #region Methods

        private void UnlockStorageItem()
        {
            if (!fromadminkit)
            {
                if (this.StorageItem.GetType() == typeof(FWBS.OMS.Precedent))
                    UnlockPrecedent(((FWBS.OMS.Precedent)this.StorageItem).ID);
            }
        }

        private void UnlockPrecedent(long precID)
        {

            Assembly assembly = Assembly.Load("OMS.UI, Version=" + Session.CurrentSession.AssemblyVersion.ToString() + ", Culture=neutral, PublicKeyToken=7212801a92a1726d");
            Type lockstate = assembly.GetType("FWBS.OMS.UI.Windows.LockState");
            MethodInfo unlockPrecedent = lockstate.GetMethod("UnlockPrecedentObject");
            ParameterInfo[] parameters = unlockPrecedent.GetParameters();
            object ls = Activator.CreateInstance(lockstate);
            object[] methodparameters = new object[] { Convert.ToString(precID) };
            unlockPrecedent.Invoke(ls, methodparameters);
        }

        private string CreateResourceCodeLookup(string Code, string Description)
        {
            return Session.CurrentSession.Resources.GetResource(Code, Description, "").Text;
        }

        #endregion
    }
}