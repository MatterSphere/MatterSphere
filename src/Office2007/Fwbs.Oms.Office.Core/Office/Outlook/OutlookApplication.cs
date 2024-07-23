using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Fwbs.Office.Outlook
{

    using System.Windows.Forms;
    using MSOffice = Microsoft.Office.Core;
    using MSOutlook = Microsoft.Office.Interop.Outlook;



    public sealed partial class OutlookApplication :
        OfficeApplication,
        MSOutlook.Application
    {
        #region Static



        internal static readonly int WM_REFRESH_PROPS = WinFinder.WindowFactory.RegisterMessage("WM_REFRESH_PROPS");

        private static OutlookApplication globalapp = null;

        private static object _lock = new object();

        public static OutlookApplication CreateApplication(MSOutlook.Application app, bool addinInstance)
        {
            if (globalapp == null)
            {
                lock (_lock)
                {
                    if (globalapp == null)
                        globalapp = new OutlookApplication(app, addinInstance);

                    if (globalapp.InternalObject != app)
                    {
                        globalapp.Dispose();
                        globalapp = new OutlookApplication(app, addinInstance);
                    }
                }
            }

            return globalapp;
        }
        public static OutlookApplication GetApplication(MSOutlook.Application app)
        {
            if (globalapp == null)
                throw new InvalidOperationException(FWBS.OMS.Session.CurrentSession.Resources.GetMessage("OUTNTCRTD", "Outlook Application not created.", "").Text);

            return globalapp;
        }

        #endregion

        #region Events

        public event System.ComponentModel.CancelEventHandler Closing;

        internal bool OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            var ev = Closing;
            if (ev != null)
            {
                ev(this, e);
                return true;
            }
            return false;
        }


        public event EventHandler<BeforeDeleteItemsEventArgs> BeforeDeleteItems;
        internal void OnBeforeDeleteItems(BeforeDeleteItemsEventArgs e)
        {
            var ev = BeforeDeleteItems;
            if (ev != null)
            {
                e.Handled = true;
                ev(this, e);
            }
        }

        public event EventHandler<BeforeSaveItemsEventArgs> BeforeSaveItems;

        internal void OnBeforeSaveItems(BeforeSaveItemsEventArgs e)
        {
            var ev = BeforeSaveItems;
            if (ev != null)
            {
                e.Handled = true;
                ev(this, e);
            }
        }

        public event EventHandler<BeforeMoveItemsEventArgs> BeforeMoveItems;

        internal void OnBeforeMoveItems(BeforeMoveItemsEventArgs e)
        {
            var ev = BeforeMoveItems;
            if (ev != null)
            {
                e.Handled = true;
                ev(this, e);
            }
        }


        public event EventHandler<BeforeOpenItemsEventArgs> BeforeOpenItems;

        internal void OnBeforeOpenItems(BeforeOpenItemsEventArgs e)
        {
            var ev = BeforeOpenItems;
            if (ev != null)
            {
                e.Handled = true;
                ev(this, e);
            }

        }


        public event EventHandler<BeforePrintItemsEventArgs> BeforePrintItems;

        internal void OnBeforePrintItems(BeforePrintItemsEventArgs e)
        {
            var ev = BeforePrintItems;
            if (ev != null)
            {
                e.Handled = true;
                ev(this, e);
            }
        }


        public event EventHandler<BeforeItemEventArgs> BeforeDeleteItem;

        internal void OnBeforeDeleteItem(BeforeItemEventArgs e)
        {
            var ev = BeforeDeleteItem;
            if (ev != null)
            {
                e.Handled = true;
                ev(this, e);
            }
        }

        public event EventHandler<BeforeForwardItemEventArgs> BeforeForwardItem;

        internal void OnBeforeForwardItem(BeforeForwardItemEventArgs e)
        {
            var ev = BeforeForwardItem;
            if (ev != null)
            {
                e.Handled = true;
                ev(this, e);
            }
        }

        public event EventHandler<BeforeReplyItemEventArgs> BeforeReplyItem;

        internal void OnBeforeReplyItem(BeforeReplyItemEventArgs e)
        {
            var ev = BeforeReplyItem;
            if (ev != null)
            {
                e.Handled = true;
                ev(this, e);
            }
        }

        public event EventHandler<BeforeItemEventArgs> BeforeOpenItem;

        internal void OnBeforeOpenItem(BeforeItemEventArgs e)
        {
            var ev = BeforeOpenItem;
            if (ev != null)
            {
                e.Handled = true;
                ev(this, e);
            }
        }

        public event EventHandler<BeforeItemEventArgs> BeforeCloseItem;

        internal void OnBeforeCloseItem(BeforeItemEventArgs e)
        {
            var ev = BeforeCloseItem;
            if (ev != null)
            {
                e.Handled = true;
                ev(this, e);
            }
        }

        public event EventHandler<BeforeItemEventArgs> BeforeActivateItem;

        internal void OnBeforeActivateItem(BeforeItemEventArgs e)
        {
            var ev = BeforeActivateItem;
            if (ev != null)
            {
                e.Handled = true;
                ev(this, e);
            }
        }

        #endregion

        #region Fields

        private MSOutlook.Application app;

        private OutlookFoldersContainer folders;
        private OutlookItemsContainer items;
        private OutlookSession session;
        private OutlookExplorers explorers;
        private OutlookInspectors inspectors;
        private Settings.ApplicationSettings settings;
        private Settings.ApplicationSettings defaultsettings;
        private bool _enableItemSendHandler = true;
        private bool _disposed = false;

        #endregion

        #region Constructors

        internal OutlookApplication(MSOutlook.Application app, bool isAddinInstance)
            : base(isAddinInstance)
        {
            if (app == null)
                throw new ArgumentNullException("app");

            //Must add this first incase any other object uses it.
            globalapp = this;

            this.app = app;

            Init(app);
        }

        protected override void OnDetach()
        {
            base.OnDetach();

            if (Settings.Activation.ForceRelease)
            {
                Marshal.FinalReleaseComObject(app);
            }
        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                // Check to see if Dispose has already been called.
                if (!_disposed)
                {
                    // If disposing equals true, dispose all managed
                    // and unmanaged resources.
                    if (disposing)
                    {
                        if (inspectors != null)
                        {
                            inspectors.Dispose();
                            inspectors = null;
                        }

                        if (explorers != null)
                        {
                            explorers.Dispose();
                            explorers = null;
                        }

                        if (session != null)
                        {
                            session.Dispose();
                            session = null;
                        }

                        if (folders != null)
                        {
                            folders.Dispose();
                            folders = null;
                        }

                        if (items != null)
                        {
                            items.Dispose();
                            items = null;
                        }

                    }

                    // Call the appropriate methods to clean up unmanaged resources here.
                    // If disposing is false, only the following code is executed.
                    DisposeRedemptionSession();

                    if (app != null)
                    {
                        Marshal.FinalReleaseComObject(app);
                    }

                    globalapp = null;

                    // Note disposing has been done.
                    _disposed = true;
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        #endregion

        #region Overrides

        protected sealed override object UnwrapValue(object value)
        {
            var folder = value as OutlookFolder;
            if (folder != null)
                return folder.InternalItem;

            var item = value as OutlookItem;
            if (item != null)
                return item.InternalItem;

            var session = value as OutlookSession;
            if (session != null)
                return session.InternalItem;

            var app = value as OutlookApplication;
            if (app != null)
                return app.InternalItem;

            var insp = value as OutlookInspector;
            if (insp != null)
                return insp.InternalItem;

            var exp = value as OutlookExplorer;
            if (exp != null)
                return exp.InternalItem;

            return base.UnwrapValue(value);
        }

        protected sealed override object WrapValue(object value)
        {
            var folder = value as MSOutlook.MAPIFolder;
            if (folder != null)
                return GetFolder(folder);

            var item = value as MSOutlook.ItemEvents_10_Event;
            if (item != null)
                return GetItem(item);

            var session = value as MSOutlook.NameSpace;
            if (session != null)
                return GetSession(session);

            var app = value as MSOutlook.Application;
            if (app != null)
                return OutlookApplication.GetApplication(app);

            var insp = value as MSOutlook.Inspector;
            if (insp != null)
                return GetInspector(insp);

            var exp = value as MSOutlook.Explorer;
            if (exp != null)
                return GetExplorer(exp);


            return base.WrapValue(value);
        }

        protected override void OnAttachEvents()
        {
            base.OnAttachEvents();

            //Attaching to application events from automathing Outlook seems to keep messages open.
            if (!IsAddinInstance)
                return;

            app.ItemSend += app_ItemSend;
            app.NewMailEx += app_NewMailEx;

            if (System.Version.Parse(app.Version).Major <= 12)
            {
                app.FolderContextMenuDisplay += app_FolderContextMenuDisplay;
                app.ItemContextMenuDisplay += app_ItemContextMenuDisplay;
            }
        }



        protected override void OnDetachEvents()
        {
            base.OnDetachEvents();

            if (!IsAddinInstance)
                return;

            try { app.ItemSend -= app_ItemSend; }
            catch { }

            try { app.NewMailEx -= app_NewMailEx; }
            catch { }

            try
            {
                if (System.Version.Parse(app.Version).Major <= 12)
                {
                    try { app.FolderContextMenuDisplay -= app_FolderContextMenuDisplay; }
                    catch { }

                    try { app.ItemContextMenuDisplay -= app_ItemContextMenuDisplay; }
                    catch { }
                }
            }
            catch { }

        }

        #endregion

        #region Redemption Interop


        private Redemption.RDOSession rdosession;
        internal Redemption.RDOSession RDOSession
        {
            get
            {
                ConnectRedemptionSession();

                return rdosession;
            }
        }

        private void ConnectRedemptionSession()
        {
            if (rdosession == null)
            {
                rdosession = Redemption.RedemptionFactory.Default.CreateRDOSession();
                rdosession.MAPIOBJECT = app.Session.MAPIOBJECT;
            }
        }


        private void DisposeRedemptionSession()
        {
            var masterProcess = System.Diagnostics.Process.GetCurrentProcess();
            
            if (rdosession != null && masterProcess.ProcessName.ToLower().Contains("outlook"))
            {
                if (Marshal.IsComObject(rdosession))
                {
                    Marshal.FinalReleaseComObject(rdosession);
                    rdosession = null;
                }
            }
        }

        #endregion

        #region OfficeApplication

        protected override void Init(object obj)
        {
            base.Init(obj);

            //Important to keep the order of contruction like this as OutlookInspectors may
            //access the items collection
            this.items = new OutlookItemsContainer(this);
            this.folders = new OutlookFoldersContainer(this);
            this.session = new OutlookSession(this, app.Session);
            this.explorers = new OutlookExplorers(this, app.Explorers);
            this.inspectors = new OutlookInspectors(this, app.Inspectors);

        }


        #endregion

        #region IApplication Members


        public void Activate()
        {
            var exp = (MSOutlook._Explorer)ActiveExplorer();
            if (exp != null)
                exp.Activate();
        }

        public string Name
        {
            get
            {
                return "Outlook";
            }
        }

        private string lastcaption = String.Empty;
        public string Caption
        {
            get
            {
                var exp = ActiveExplorer();
                if (exp != null)
                    return exp.Caption;
                else
                    return lastcaption;
            }
            set
            {
                lastcaption = value ?? String.Empty;
                var exp = (OutlookExplorer)ActiveExplorer();
                if (exp != null)
                    exp.Caption = lastcaption;

            }
        }

        public bool Visible
        {
            get
            {
                return (from w in explorers where w.Visible select w) != null;
            }
            set
            {
                var exps = explorers.ToArray();
                Array.ForEach(exps, e => e.Visible = value);
            }
        }

        public IWin32Window ActiveWindow
        {
            get
            {
                try
                {
                    return OutlookUtils.FindWindow(app.ActiveWindow());
                }
                catch
                {
                    return null;
                }
            }
        }

        public IWin32Window GetWindow(object obj)
        {
            return OutlookUtils.FindWindow(obj);
        }

        #endregion

        #region Properties

        internal MSOutlook.Application InternalItem
        {
            get
            {
                CheckIfDetached();
                CheckIfDisposed();

                return app;
            }
        }


        public Settings.ApplicationSettings Settings
        {
            get
            {
                if (settings == null)
                    settings = new Settings.ApplicationSettings(this);
                return settings;
            }
        }

        internal Settings.ApplicationSettings DefaultSettings
        {
            get
            {
                if (defaultsettings == null)
                    defaultsettings = new Settings.ApplicationSettings(this);
                return defaultsettings;
            }
        }

        internal OutlookItemsContainer LoadedItems
        {
            get
            {
                return items;
            }
        }

        internal OutlookFoldersContainer LoadedFolders
        {
            get
            {
                return folders;
            }
        }

        public bool IsProcessing { get; private set; }

        public IDisposable BeginProcess()
        {
            return new ApplicationProcess(this);
        }

        #endregion

        #region Retrieval


        public OutlookExplorer GetExplorer(MSOutlook.Explorer obj)
        {
            return explorers.AddExplorer(obj);
        }

        public OutlookInspector GetInspector(MSOutlook.Inspector obj)
        {
            return inspectors.AddInspector(obj);
        }

        public OutlookFolder GetFolder(MSOutlook.MAPIFolder obj)
        {
            return folders.GetFolder(obj);
        }

        internal OutlookSession GetSession(MSOutlook.NameSpace obj)
        {
            var s = obj as OutlookSession;
            if (s != null)
                return s;

            return new OutlookSession(this, obj);
        }

        public OutlookItem GetItem(object obj)
        {
            return items.GetItem(() => obj);
        }

        public OutlookItem GetItem(object obj, bool pin)
        {
            return items.GetItem(() => obj, pin);
        }

        #endregion

        #region Folder Methods

        public OutlookItem GetItemFromId(string entryId, string storeId)
        {
            return LoadedItems.GetItem(() => app.Session.GetItemFromID(entryId, storeId));
        }

        public OutlookFolder GetFolderFromId(string folderId, string storeId)
        {
            return folders.GetFolder(app.Session.GetFolderFromID(folderId, storeId));
        }


        #endregion

        #region _Application Members


        internal MSOutlook.Explorer CurrentExplorer { get; set; }

        public MSOutlook.Explorer ActiveExplorer()
        {
            if (CurrentExplorer != null)
                return CurrentExplorer;

            var exp = app.ActiveExplorer();
            if (exp == null)
                return null;

            CurrentExplorer = explorers.AddExplorer(exp);

            return CurrentExplorer;
        }

        public MSOutlook.Inspector ActiveInspector()
        {
            var olinsp = app.ActiveInspector();
            if (olinsp == null)
                return null;
            return inspectors.AddInspector(olinsp);
        }

        object MSOutlook._Application.ActiveWindow()
        {
            return app.ActiveWindow();
        }

        public MSOutlook.Search AdvancedSearch(string Scope, object Filter, object SearchSubFolders, object Tag)
        {
            return app.AdvancedSearch(Scope, Filter, SearchSubFolders, Tag);
        }

        public MSOffice.AnswerWizard AnswerWizard
        {
            get { return app.AnswerWizard; }
        }

        public MSOutlook.Application Application
        {
            get { return this; }
        }


        public bool IsDeleted
        {
            get
            {
                try
                {
                    var ver = app.Version;
                    return false;
                }
                catch (InvalidComObjectException)
                {
                    return true;
                }
                catch (COMException comex)
                {
                    if (comex.ErrorCode == HResults.RPC_UNAVAILABLE)
                        return true;

                    return false;
                }
            }

        }

        public MSOffice.Assistant Assistant
        {
            get { return app.Assistant; }
        }

        public MSOffice.COMAddIns COMAddIns
        {
            get { return app.COMAddIns; }
        }

        public MSOutlook.OlObjectClass Class
        {
            get { return app.Class; }
        }

        public object CopyFile(string FilePath, string DestFolderPath)
        {
            //TODO: Test this
            return LoadedItems.GetItem(() => app.CopyFile(FilePath, DestFolderPath));
        }

        public object CreateItem(MSOutlook.OlItemType ItemType)
        {
            return LoadedItems.GetItem(() => app.CreateItem(ItemType));
        }

        public object CreateItemFromTemplate(string TemplatePath)
        {
            return LoadedItems.GetItem(() => app.CreateItemFromTemplate(TemplatePath, Type.Missing));
        }

        public object CreateItemFromTemplate(string TemplatePath, object InFolder)
        {
            return LoadedItems.GetItem(() => app.CreateItemFromTemplate(TemplatePath, InFolder));
        }

        public object CreateObject(string ObjectName)
        {
            return LoadedItems.GetItem(() => app.CreateObject(ObjectName));
        }


        public Microsoft.Office.Interop.Outlook.Explorers Explorers
        {
            get { return explorers; }
        }

        public MSOffice.MsoFeatureInstall FeatureInstall
        {
            get
            {
                return app.FeatureInstall;
            }
            set
            {
                app.FeatureInstall = value;
            }
        }

        public MSOutlook.NameSpace GetNamespace(string Type)
        {
            var ns = app.GetNamespace(Type);
            if (ns == app.Session)
                return session;
            else
                return new OutlookSession(this, ns);
        }

        public void GetNewNickNames(ref object pvar)
        {
            app.GetNewNickNames(ref pvar);
        }


        public MSOutlook.Inspectors Inspectors
        {
            get { return inspectors; }
        }

        public bool IsSearchSynchronous(string LookInFolders)
        {
            return app.IsSearchSynchronous(LookInFolders);
        }


        public MSOffice.LanguageSettings LanguageSettings
        {
            get { return app.LanguageSettings; }
        }

        //TODO: Check
        public object Parent
        {
            get { return app.Parent; }
        }

        public string ProductCode
        {
            get { return app.ProductCode; }
        }

        void MSOutlook._Application.Quit()
        {
            ((MSOutlook._Application)app).Quit();
        }

        public MSOutlook.Reminders Reminders
        {
            get { return app.Reminders; }
        }

        public OutlookSession Session
        {
            get
            {
                return session;
            }
        }

        MSOutlook.NameSpace MSOutlook._Application.Session
        {
            get { return session; }
        }


        public string Version
        {
            get { return app.Version; }
        }

        public int MajorVersion
        {
            get
            {
                try
                {
                    return Convert.ToInt32(this.Version.Split('.')[0]);
                }
                catch (Exception)
                {
                    // ignored
                }
                return -1;
            }
        }

        #endregion

        #region ApplicationEvents_11_Event Members

        public event MSOutlook.ApplicationEvents_11_AdvancedSearchCompleteEventHandler AdvancedSearchComplete
        {
            add
            {
                app.AdvancedSearchComplete += value;
            }
            remove
            {
                app.AdvancedSearchComplete -= value;
            }
        }

        public event MSOutlook.ApplicationEvents_11_AdvancedSearchStoppedEventHandler AdvancedSearchStopped
        {
            add
            {
                app.AdvancedSearchStopped += value;
            }
            remove
            {
                app.AdvancedSearchStopped -= value;
            }
        }



        public event MSOutlook.ApplicationEvents_11_ItemSendEventHandler ItemSend;

        public event MSOutlook.ApplicationEvents_11_MAPILogonCompleteEventHandler MAPILogonComplete
        {
            add
            {
                app.MAPILogonComplete += value;
            }
            remove
            {
                app.MAPILogonComplete -= value;
            }
        }

        public event MSOutlook.ApplicationEvents_11_NewMailEventHandler NewMail
        {
            add
            {
                app.NewMail += value;
            }
            remove
            {
                app.NewMail -= value;
            }
        }

        public event MSOutlook.ApplicationEvents_11_NewMailExEventHandler NewMailEx;


        public event MSOutlook.ApplicationEvents_11_OptionsPagesAddEventHandler OptionsPagesAdd
        {
            add
            {
                app.OptionsPagesAdd += value;
            }
            remove
            {
                app.OptionsPagesAdd -= value;
            }
        }

        public event MSOutlook.ApplicationEvents_11_QuitEventHandler Quit
        {
            add
            {
                ((MSOutlook.ApplicationEvents_11_Event)app).Quit += value;
            }
            remove
            {
                try
                {
                    ((MSOutlook.ApplicationEvents_11_Event)app).Quit -= value;
                }
                catch { }
            }
        }

        public event MSOutlook.ApplicationEvents_11_ReminderEventHandler Reminder
        {
            add
            {
                app.Reminder += value;
            }
            remove
            {
                app.Reminder -= value;
            }
        }



        public event MSOutlook.ApplicationEvents_11_StartupEventHandler Startup
        {
            add
            {
                app.Startup += value;
            }
            remove
            {
                app.Startup -= value;
            }
        }



        #endregion

        #region Methods

        public void SendOutbox()
        {
            MSOutlook.NameSpace ns = (MSOutlook.NameSpace)session.InternalObject;
            if (!ns.Offline)
            {
                MSOutlook.MAPIFolder folder = ns.GetDefaultFolder(MSOutlook.OlDefaultFolders.olFolderOutbox);
                _enableItemSendHandler = false;
                foreach (dynamic item in folder.Items)
                {
                    try
                    {
                        item.Send();
                    }
                    catch (Exception ex)
                    {
                        WriteLog("Send Outbox Item", "Unable to Send Item.", "Refer to the following exception", ex);
                    }
                    finally
                    {
                        Marshal.ReleaseComObject(item);
                    }
                }
                _enableItemSendHandler = true;
                Marshal.ReleaseComObject(folder);
            }
        }

        public void EnableHooks()
        {
            if (DefaultSettings.KeyHooks.Enabled)
            {
                Settings.KeyHooks.Enabled = true;

                foreach (OutlookExplorer exp in explorers)
                {
                    exp.InstallKeyHooks();
                }
            }
        }

        public void DisableHooks()
        {
            Settings.KeyHooks.Enabled = false;

            foreach (OutlookExplorer exp in explorers)
            {
                exp.UnInstallKeyHooks();
            }
        }

        internal OutlookItem[] GetCurrentSelectedItemsFromExplorers()
        {
            var selected = new List<OutlookItem>();

            foreach (var exp in explorers.OfType<MSOutlook.Explorer>())
            {
                try
                {
                    using (var sel = (OutlookSelection)exp.Selection)
                    {
                        if (sel.Count > 0)
                        {
                            var item = (OutlookItem)sel[1];
                            if (item != null)
                                selected.Add(item);
                        }
                    }
                }
                catch (COMException comex)
                {
                    System.Diagnostics.Debug.WriteLine(comex, "OutlookApplication.GetCurrentSelectedItemsFromExplorers");
                }
            }

            return selected.ToArray();
        }

        internal void RemoveInspector(MSOutlook.Inspector insp)
        {
            inspectors.RemoveInspector(insp);
        }

        internal void RemoveExplorer(MSOutlook.Explorer exp)
        {
            explorers.RemoveExplorer(exp);
        }

        internal void RemoveFolder(MSOutlook.MAPIFolder folder)
        {
            folders.RemoveFolder(folder);
        }

        #endregion

        #region Logging

        public void WriteLog(string title, string text, string resolution, Exception ex)
        {
            Framework.EventLogger.Write(title, text, resolution, ex);
        }

        #endregion

        #region Captured Events

        private void app_NewMailEx(string entryId)
        {
            if (!Settings.Events.NewMail.Enabled)
                return;

            var ev = NewMailEx;
            var item = GetItemFromId(entryId, null);
            if (item != null)
            {
                item.Attach();
                item.AttachEvents();
            }
            if (ev != null && item != null)
            {

                ev(entryId);
            }

            LoadedItems.Refresh(item);
        }

        private void app_ItemSend(object item, ref bool cancel)
        {
            OutlookItem oi = null;
            try
            {
                var ev = ItemSend;

                if (ev != null && _enableItemSendHandler)
                {
                    oi = GetItem(item, true);
                    oi.RequiresSpellCheck = false;
                    ev(oi, ref cancel);
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error Sending Item", "", "", ex);
                throw;
            }
            finally
            {
                if (oi != null)
                    oi.IsPinned = false;
            }
        }

        #endregion


        private class ApplicationProcess : IDisposable
        {
            private bool original;
            private OutlookApplication app;

            public ApplicationProcess(OutlookApplication app)
            {
                this.app = app;
                original = app.IsProcessing;
                this.app.IsProcessing = true;
            }

            #region IDisposable Members

            public void Dispose()
            {
                app.IsProcessing = original;
            }

            #endregion
        }

    }

}
