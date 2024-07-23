using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    using FWBS.OMS.DocumentManagement.Storage;

    public partial class StorageSettingsEditorBase : UserControl
    {
        private StorageSettings settings;
        private IStorageItem item;
        private StorageProvider provider;

        public StorageSettingsEditorBase()
        {
            InitializeComponent();
        }

        public void Initialise(IStorageItem item, StorageProvider provider, StorageSettings settings)
        {
            if (item == null)
                throw new ArgumentNullException("item");

            if (provider == null)
                throw new ArgumentNullException("provider");

            if (settings == null)
                throw new ArgumentNullException("settings");

            this.item = item;
            this.provider = provider;
            this.settings = settings;

            InternalInitialise();
            RefreshSettings();
        }


        protected StorageSettings Settings
        {
            get
            {
                return settings;
            }
        }

        protected IStorageItem StorageItem
        {
            get
            {
                return item;
            }
        }

        protected StorageProvider Provider
        {
            get
            {
                return provider;
            }
        }

        protected StorageSettingsContainer Container
        {
            get
            {
                Control ctrl = Parent;
                while (ctrl != null)
                {
                    StorageSettingsContainer ctrlsettings = ctrl as StorageSettingsContainer;
                    if (ctrlsettings != null)
                        return ctrlsettings;
                    ctrl = ctrl.Parent;
                }

                return null;
            }
        }

        
        public void RefreshSettings()
        {
            InternalRefreshSettings();
        }

        public void CancelSettings()
        {
            StorageItem.ClearSettings();
        }

        public void ApplySettings()
        {
            InternalApplySettings();
        }

        internal protected virtual void OnFinishing(EnquiryForm screen, CancelEventArgs args)
        {
        }

        protected virtual void InternalInitialise()
        {
        }

        protected virtual void InternalRefreshSettings()
        {
        }

        protected virtual void InternalApplySettings()
        {
        }




    }
}
