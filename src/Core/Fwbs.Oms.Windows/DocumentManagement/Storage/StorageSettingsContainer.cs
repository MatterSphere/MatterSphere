using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FWBS.OMS.UI.Windows.DocumentManagement.Storage
{
    using FWBS.OMS.DocumentManagement.Storage;

    public partial class StorageSettingsContainer : UserControl
    {
        #region Fields

        private IStorageItem item;
        private StorageProvider provider;
        private StorageSettingsCollection settings;
        private SettingsType type = SettingsType.Store;

        #endregion

        #region Constructors

        public StorageSettingsContainer()
        {
            InitializeComponent();
        }

        #endregion

        #region Properties

        [Browsable(false)]
        [DefaultValue(null)]
        public IStorageItem StorageItem
        {
            get
            {
                return item;
            }
            set
            {
                if (item != value)
                {
                    item = value;

                    if (item != null)
                    {
                        provider = item.GetStorageProvider();
                        settings = item.GetSettings();
                    }

                    PopulateProviders();
                }
            }
        }

        [DefaultValue(SettingsType.Store)]
        public SettingsType SettingsType
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }


        #endregion

        #region Methods

        public void SetSettingsPage(string defaultSettings)
        {
            if (String.IsNullOrEmpty(defaultSettings))
                return;

            tabControl1.Focus();
            if (tabControl1.TabPages.ContainsKey(defaultSettings))
                tabControl1.SelectedTab = tabControl1.TabPages[defaultSettings];
        }

        private void PopulateProviders()
        {
            try
            {
                cboStorageProvider.SelectedIndexChanged -= new EventHandler(cboStorageProvider_SelectedIndexChanged);

                if (item != null)
                {
                    cboStorageProvider.DataSource = StorageManager.CurrentManager.GetRegisteredProviders();
                    cboStorageProvider.ValueMember = "spid";
                    cboStorageProvider.DisplayMember = "spdesc";
                    cboStorageProvider.SelectedValue = item.GetStorageProvider().Id;
                    pnlHeader.Visible = (item.IsNew && Session.CurrentSession.CurrentUser.IsInRoles("DEBUG"));
                    ChangeProvider(item.GetStorageProvider().Id);
                }
                else
                {
                    pnlHeader.Visible = false;
                }
            }
            finally
            {
                cboStorageProvider.SelectedIndexChanged += new EventHandler(cboStorageProvider_SelectedIndexChanged);
            }
        }

        private void BuildSettings()
        {
   
            tabControl1.TabPages.Clear();

            if (settings != null)
            {
           
                foreach (StorageSettings set in settings)
                {
                    if (set.SettingsType == type)
                    {
                        if (set.CanEdit && set.SettingsScreenVisible)
                        {
                            object[] attrs = set.GetType().GetCustomAttributes(typeof(StorageSettingsEditorAttribute), false);
                            if (attrs.Length > 0)
                            {
                                Type editortype = ((StorageSettingsEditorAttribute)attrs[0]).Type;
                                if (editortype != null)
                                {
                                    StorageSettingsEditorBase editor = Session.CurrentSession.TypeManager.Create(editortype) as StorageSettingsEditorBase;
                                    if (editor != null)
                                    {
                                        editor.Initialise(item, provider, set);
                                        TabPage page = new TabPage(set.Name);
                                        page.Name = set.GetType().Name;
                                        editor.Dock = DockStyle.Fill;
                                        page.Controls.Add(editor);
                                        tabControl1.TabPages.Add(page);
                                        continue;
                                    }
                                }
                            }
                        }

                    }
                }
            }

            if (tabControl1.TabCount > 0)
            {
                tabControl1.Visible = true;
                txtNotSupported.Visible = false;
                tabControl1.Focus();
            }
            else
            {
                tabControl1.Visible = false;
                txtNotSupported.Visible = true;
            }
           
            
        }

        public void OnFinishing(EnquiryForm screen, CancelEventArgs args)
        {

            foreach (TabPage page in tabControl1.TabPages)
            {
                StorageSettingsEditorBase editor = page.Controls[0] as StorageSettingsEditorBase;
                if (editor != null)
                {
                    editor.OnFinishing(screen, args);
                    if (args.Cancel)
                        return;
                }
            }
        }

        public void ApplySettings()
        {

            foreach (TabPage page in tabControl1.TabPages)
            {
                StorageSettingsEditorBase editor = page.Controls[0] as StorageSettingsEditorBase;
                if (editor != null)
                    editor.ApplySettings();

            }

            StorageItem.ChangeStorage(provider, false);
            StorageItem.ApplySettings(settings);
        }

        public void CancelSettings()
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                StorageSettingsEditorBase editor = page.Controls[0] as StorageSettingsEditorBase;
                if (editor != null)
                    editor.CancelSettings();

            }
        }

        public void RefreshSettings()
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                StorageSettingsEditorBase editor = page.Controls[0] as StorageSettingsEditorBase;
                if (editor != null)
                    editor.RefreshSettings();

            }

        }

        private void ChangeProvider()
        {
            short id = -1;
            System.Data.DataRowView row = cboStorageProvider.SelectedValue as System.Data.DataRowView;

            if (cboStorageProvider.SelectedValue is short)
                id = (short)cboStorageProvider.SelectedValue;

            if (row != null)
            {
                id = Convert.ToInt16(row["spid"]);
            }

            ChangeProvider(id);

        }

        private void ChangeProvider(short id)
        {
            if (provider == null || id != provider.Id || settings == null || settings.Count == 0)
            {
                try
                {
                    provider = StorageManager.CurrentManager.GetStorageProvider(id);
                    settings = provider.GetDefaultSettings(item, type);
                    BuildSettings();
                }
                catch (StorageLocationProviderItemNotInstalledException)
                {
                    //revert combo box
                    if ( provider != null)
                        cboStorageProvider.SelectedValue = provider.Id;
                    throw;
                }
            }
            else
            {
                settings = item.GetSettings();
                if (settings != null && provider != null)
                    BuildSettings();
            }
        }

        #endregion

        #region Captured Events


        private void cboStorageProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeProvider();
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void StorageSettingsContainer_ParentChanged(object sender, EventArgs e)
        {
            EnquiryForm screen = this.Parent as EnquiryForm;
            if (screen != null)
            {
                item = screen.Enquiry.Object as IStorageItem;


                if (item != null)
                {
                    provider = item.GetStorageProvider();
                    settings = item.GetSettings();
                }

                screen.Cancelled -= new EventHandler(screen_Cancelled);
                screen.Cancelled += new EventHandler(screen_Cancelled);
                screen.Finishing -= new CancelEventHandler(screen_Finishing);
                screen.Finishing += new CancelEventHandler(screen_Finishing);

                PopulateProviders();
            }           
        }

        private void screen_Cancelled(object sender, EventArgs e)
        {
            CancelSettings();
        }

        private void screen_Finishing(object sender, CancelEventArgs e)
        {
            OnFinishing((EnquiryForm)sender, e);
            if (e.Cancel)
                return;

            ApplySettings();
        }


        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            try
            {
                e.TabPage.Controls[0].Focus();
            }
            catch { }
        }

        private void StorageSettingsContainer_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                tabControl1.Focus();
                try
                {
                    tabControl1.SelectedTab.Controls[0].Focus();
                }
                catch { }
            }
        }


        #endregion



    }
}
