using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using FWBS.Common;
using FWBS.ExternalAggregatorClient;
using FWBS.OMS.Data;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS
{
    using Fwbs.Documents;
    using Fwbs.Framework.ComponentModel.Composition;

    /// <summary>
    /// Main session object that holds the flow of execution for the OMS business layer.
    /// This object is enquiry compatible due to being derived from branch.
    /// </summary>
    [Security.SecurableType("SYSTEM")]
    public class Session : Branch, FWBS.OMS.Data.IDatabaseSchema, Interfaces.IEnquiryCompatible, Security.ISecurable
    {
        #region RegistryRes
        public string RegistryRes(string Code, string Default)
        {
            string culture = Convert.ToString(new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "UICulture", "OverrideUI").GetSetting(Thread.CurrentThread.CurrentUICulture.Name));
            return Convert.ToString(new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "UICulture\\" + culture, Code).GetSetting(Default));
        }

        #endregion

        #region Events

        /// <summary>
        /// This event gets raised when the session object successfully logs
        /// into the system.
        /// </summary>
        public event EventHandler LoggedIn = null;

        /// <summary>
        /// An event that gets raised when the session logs off successfully.
        /// </summary>
        public event EventHandler LoggedOff = null;

        /// <summary>
        /// An event that gets raised when a session has been connected.
        /// </summary>
        public event EventHandler Connected = null;

        /// <summary>
        /// An event that gets raised when a session has been disconnected.
        /// </summary>
        public event EventHandler Disconnected = null;

        /// <summary>
        /// An event that gets raised when a session is connecting.
        /// </summary>
        public event EventHandler Connecting = null;

        /// <summary>
        /// An event that gets raised when a session is being disconnected.
        /// </summary>
        public event CancelEventHandler Disconnecting = null;

        /// <summary>
        /// An event that gets raised when the session has not yet been logged in.
        /// This will only get raised if the event sink has been assigned, otherwise an
        /// exception will be raised.
        /// </summary>
        public event EventHandler NotLoggedIn = null;

        /// <summary>
        /// An event that gets raised if a warning is to be shown rather than an exception
        /// breaking the flow of code.
        /// </summary>
        public event MessageEventHandler Warning = null;

        /// <summary>
        /// An event that gets raised if a Exception is raised but you dont want to break the flow of the code
        /// </summary>
        public event MessageEventHandler ShowException = null;

        /// <summary>
        /// An event that gets raised when a password is required before continuing
        /// a process.
        /// </summary>
        public event PasswordRequestEventHandler PasswordRequest = null;

        /// <summary>
        /// An event that gets raised when a question needs to be asked to the UI layer and a result
        /// be known within the business layer.
        /// </summary>
        public event AskEventHandler Ask = null;

        /// <summary>
        /// An event that gets raised when the user needs to be prompted for something other that a dialog
        /// result of a message box.
        /// </summary>
        public event PromptEventHandler Prompt = null;


        /// <summary>
        /// An event that gets raised when a search list needs to be displayed in its current state.
        /// </summary>
        public event ShowSearchEventHandler ShowSearch = null;

        /// <summary>
        /// An event that gets raised when a enquiry form needs to be displayed in its current state.
        /// </summary>
        public event ShowEnquiryEventHandler ShowEnquiry = null;

        /// <summary>
        /// An event that gets raised when a enquiry form needs to be displayed in its current state.
        /// </summary>
        public event ShowExtendedDataEventHandler ShowExtendedData = null;

        /// <summary>
        /// An event that gets raised when a wizard needs to be displayed in its current state.
        /// </summary>
        public event ShowEnquiryEventHandler ShowWizard = null;

        public event ConnectionErrorEventHandler ConnectionError = null;

        public event EventHandler ShutdownRequest = null;

        #endregion

        #region IProgress
        public event ProgressEventHandler Progress = null;
        public event EventHandler ProgressStart = null;
        public event EventHandler ProgressFinished = null;
        public event MessageEventHandler ProgressError = null;

        public void OnProgress(FWBS.Common.ProgressEventArgs e)
        {
            if (Progress != null)
                Progress(this, e);
        }

        public void OnProgressStart()
        {
            if (ProgressStart != null)
                ProgressStart(this, EventArgs.Empty);
        }

        public void OnProgressFinished()
        {
            if (ProgressFinished != null)
                ProgressFinished(this, EventArgs.Empty);
        }

        public void OnProgressError(MessageEventArgs e)
        {
            if (ProgressError != null)
                ProgressError(this, e);
        }
        #endregion

        #region Fields
        private DistributedAssemblyManager _distributionmanager = null;

        private bool? _isSearchConfigured = null;
        private bool? _isESSearchConfigured = null;
        private bool? _isMSSearchConfigured = null;
        private bool? _isSearchSummaryFieldEnabled = null;
        private int? _minimalSymbolsCountForSuggests = null;
        private int? _maximumSuggestsAmount = null;
        private string _installLocation;
        private bool shuttingDown = false;

        private SessionSingleton singleton = null;

        /// <summary>
        /// API consumer assembly object.
        /// </summary>
        private Assembly _apiConsumer = null;

        /// <summary>
        /// Currently logged in flag.
        /// </summary>
        private bool _loggedIn = false;

        /// <summary>
        /// Currently File Gathered from Alternative method.
        /// </summary>
        private bool _currentfilegathered = false;

        /// <summary>
        /// Session configuration XML document.
        /// </summary>
        private ConfigSetting _sessionconfig = null;

        /// <summary>
        /// Currently logged on user.
        /// </summary>
        internal User _currentUser = null;

        /// <summary>
        /// Current power user settings.
        /// </summary>
        internal Power _currentPowerUserSettings = null;
        /// <summary>
        /// Currently logged on user last updated
        /// </summary>
        internal DateTimeNULL _currentUserLastUpdated = DBNull.Value;

        /// <summary>
        /// Currently logged on user's fee earner last updated
        /// </summary>
        internal DateTimeNULL _currentFeeEarnerLastUpdated = DBNull.Value;

        /// <summary>
        /// Currently logged on user's fee earner user record last updated
        /// </summary>
        internal DateTimeNULL _currentFeeEarnerUserLastUpdated = DBNull.Value;

        /// <summary>
        /// Currently Selected Printer.
        /// </summary>
        private Printer _currentPrinter = null;

        /// <summary>
        /// Currently Selected Fee Earner.
        /// </summary>
        private FeeEarner _currentFeeEarner = null;

        /// <summary>
        /// Current terminal registration.
        /// </summary>
        private Terminal _currentTerminal = null;
        /// <summary>
        /// Current branch registration.
        /// </summary>
        private Branch _currentBranch = null;

        /// <summary>
        /// Current favourites list for the currentley logged in user.
        /// </summary>
        private Favourites _currentFavourites = null;

        /// <summary>
        /// System parameters built into dbRegInfo table.
        /// </summary>
        internal DataSet _regInfo = null;

        /// <summary>
        /// A flag to indicate whether any changes have been made.
        /// </summary>
        private bool _isdirty = false;

        /// <summary>
        /// Terminology parser instance.
        /// </summary>
        private Terminology _terminolgy = null;

        /// <summary>
        /// Common resource strings.
        /// </summary>
        private Res _resources = null;

        /// <summary>
        /// A flag that holds whether the calling client application is a web client or not.
        /// </summary>
        private UIClientType _clientType = UIClientType.Windows;

        /// <summary>
        /// Is the session currently in enquiry design mode.
        /// </summary>
        internal bool _designMode = false;

        /// <summary>
        /// Is an admin kit instance.
        /// </summary>
        internal bool _isAdminInstance = false;

        /// <summary>
        /// A flag that indicates that the calling application is a service application.
        /// </summary>
        internal bool _service = false;

        /// <summary>
        /// Reg information SQL statement.
        /// </summary>
        new internal static string Sql = "select top 1 * from dbRegInfo R";

        /// <summary>
        /// Table Name of internal store.
        /// </summary>
        new public const string Table = "REGINFO";

        /// <summary>
        /// Table Name of internal branch table.
        /// </summary>
        internal const string Table_Branch = "BRANCH";
        /// <summary>
        /// Table Name of internal captains log schema.
        /// </summary>
        internal const string Table_Log = "CAPTAINSLOG";
        /// <summary>
        /// Table Name of internal captains log types.
        /// </summary>
        internal const string Table_LogType = "CAPTAINSLOGTYPE";
        /// <summary>
        /// Table Name of internal directories..
        /// </summary>
        internal const string Table_Directory = "DIRECTORIES";

        /// <summary>
        /// Static OMS session object that is to be used.  This is the current session.
        /// </summary>
        private static Session _oms = null;

        /// <summary>
        /// A flag that enables the public key validation check when accessing data from outside
        /// the assembly.
        /// </summary>
        internal bool ValidateCriticalDataAccess = true;

        /// <summary>
        /// The script object to use for the currently logged in user.
        /// </summary>
        private Script.ScriptGen _sessionScript = null;

        /// <summary>
        /// Job list object array of instructions todo.
        /// </summary>
        private PrecedentJobList _joblist = null;

        /// <summary>
        /// The multi database conntaction information.
        /// </summary>
        internal DatabaseSettings _multidb = null;

        /// <summary>
        /// The default session currency used.
        /// </summary>
        private FWBS.OMS.Currency _currency = null;

        /// <summary>
        /// A reference to the global extensible addin manager.
        /// </summary>
        private Extensibility.AddinManager _addinman = null;

        /// <summary>
        /// Skips the check loggin if set to true.
        /// </summary>
        internal bool _skiplogincheck = false;

        /// <summary>
        /// Returns a DataTable of Installed Packages
        /// </summary>
        private DataTable _installedpackages = null;

        private DataTable _countries = null;

        /// <summary>
        /// A list of oms services.
        /// </summary>
        private System.Collections.Generic.List<IService> services;

        /// <summary>
        /// Used by the ApplicationRolePresent property to determine if the OMSApplicationRole is in use
        /// </summary>
        private DatabaseSettings databaseSettings = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Tries to connect to a session statically.
        /// </summary>
        static Session()
        {
            PrintStackTrace("Session static constructor", "Session");
        }

        /// <summary>
        /// Creates a session object passing a true value to the base branch object
        /// telling the object that it is being derived, don't do anything fancy at all.
        /// </summary>
        private Session()
            : base(true)
        {
            PrintStackTrace("Session constructor", "Session");
            _installLocation = GetInstallLocation();
        }

        #endregion

        #region Destructors



        /// <summary>
        /// Disposes all internal objects used by this object.
        /// </summary>
        /// <param name="disposing">A flag that allows the use of freeing other state managed objects.</param>
        protected override void Dispose(bool disposing)
        {

            try
            {
                if (disposing)
                {

                    try
                    {

                        if (_currentUser != null)
                        {
                            _currentUser.Dispose();
                            _currentUser = null;
                        }
                    }
                    catch { }

                    try
                    {
                        if (_currentPrinter != null)
                        {
                            _currentPrinter.Dispose();
                            _currentPrinter = null;
                        }
                    }
                    catch { }

                    try
                    {
                        if (_currentFeeEarner != null)
                        {
                            _currentFeeEarner.Dispose();
                            _currentFeeEarner = null;
                        }
                    }
                    catch { }

                    try
                    {
                        if (_currentTerminal != null)
                        {
                            _currentTerminal.Dispose();
                            _currentTerminal = null;
                        }
                    }
                    catch { }

                    try
                    {
                        if (_currentFavourites != null)
                        {
                            _currentFavourites.Dispose();
                            _currentFavourites = null;
                        }
                    }
                    catch { }

                    try
                    {
                        if (_sessionScript != null)
                        {
                            _sessionScript.Dispose();
                            _sessionScript = null;
                        }
                    }
                    catch { }

                    try
                    {
                        if (memorycache != null)
                        {
                            foreach (LimitCollection mc in memorycache.Values)
                            {
                                mc.Dispose();
                                mc.Clear();
                            }
                            memorycache.Clear();
                        }
                    }
                    catch { }



                    _joblist = null;

                    _terminolgy = null;

                    _loggedIn = false;

                    try
                    {
                        if (_regInfo != null)
                        {
                            _regInfo.Dispose();
                            _regInfo = null;
                        }
                    }
                    catch { }


                    UnloadServices();

                    try
                    {
                        if (singleton != null)
                        {
                            singleton.Dispose();
                            singleton = null;
                        }
                    }
                    catch { }

                    try
                    {
                        DisposeContainer();
                    }
                    catch { }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }


        #endregion

        #region Event Methods

        internal void OnObjectEvent(Extensibility.ObjectEventArgs e)
        {
            if (e == null || e.Sender == null) return;

            if (EnableAddins)
            {
                Addins.OnObjectEvent(e);
            }
        }

        /// <summary>
        /// Raises the log in event.
        /// </summary>
        protected void OnLoggedIn()
        {
            if (LoggedIn != null)
                LoggedIn(this, EventArgs.Empty);

        }

        /// <summary>
        /// Raises the log off event.
        /// </summary>
        protected void OnLoggedOff()
        {
            if (LoggedOff != null)
                LoggedOff(this, EventArgs.Empty);

        }

        /// <summary>
        /// Raises the connected event.
        /// </summary>
        protected void OnConnected()
        {
            if (Connected != null)
                Connected(this, EventArgs.Empty);
        }

        protected void OnConnecting()
        {
            if (Connecting != null)
                Connecting(this, EventArgs.Empty);
        }


        /// <summary>
        /// Raises the disconnected event.
        /// </summary>
        protected void OnDisconnected()
        {
            if (Disconnected != null)
                Disconnected(this, EventArgs.Empty);
        }

        /// <summary>
        /// Raises the disconnected event.
        /// </summary>
        protected void OnDisconnecting(CancelEventArgs e)
        {
            if (Disconnecting != null)
                Disconnecting(this, e);
        }

        /// <summary>
        /// Raises the not logged in event.
        /// </summary>
        protected void OnNotLoggedIn()
        {
            if (NotLoggedIn != null)
                NotLoggedIn(this, EventArgs.Empty);
        }


        /// <summary>
        /// Raises the warning event.
        /// </summary>
        /// <param name="sender">Object that has thrown the warning.</param>
        /// <param name="e">Message arguments.</param>
        internal protected void OnWarning(object sender, MessageEventArgs e)
        {
            if (IsAutomation)
            {
                Console.WriteLine("[Automation] - " + e.Message);
                return;
            }

            if (Warning != null)
            {
                Warning(sender, e);
            }
        }

        /// <summary>
        /// Raises the warning event if the event is captures, otherwise the passed exception is thrown.
        /// </summary>
        /// <param name="sender">Object that has thrown the warning / exception.</param>
        /// <param name="ex">Exception to raise if the warning event is not captured.</param>
        internal protected void OnWarning(object sender, Exception ex)
        {
            if (Warning == null)
                throw ex;
            else
                Warning(sender, new MessageEventArgs(ex));
        }

        /// <summary>
        /// Raises the warning event if the event is captures, otherwise the passed exception is thrown.
        /// </summary>
        /// <param name="sender">Object that has thrown the warning / exception.</param>
        /// <param name="ex">Exception to raise if the warning event is not captured.</param>
        internal protected void OnShowException(object sender, Exception ex)
        {
            if (ShowException == null)
                throw ex;
            else
                ShowException(sender, new MessageEventArgs(ex));
        }

        /// <summary>
        /// Raises the password request event.
        /// </summary>
        /// <param name="sender">Object that has thrown the password request.</param>
        /// <param name="e">Password request arguments.</param>
        internal protected void OnPasswordRequest(IPasswordProtected sender, CancelEventArgs e)
        {
            if (PasswordRequest != null && sender.HasPassword)
                PasswordRequest(sender, e);
            else
                sender.ValidatePassword();
        }

        /// <summary>
        /// Raises the ask event.
        /// </summary>
        /// <param name="sender">Object that has thrown the question.</param>
        /// <param name="e">Ask event arguments.</param>
        internal protected void OnAsk(object sender, AskEventArgs e)
        {
            if (IsAutomation)
            {
                e.Result = AskResult.No;

                Console.WriteLine("[Automation] - " + e.Message.Text + " - " + e.Result);
                return;
            }

            if (Ask != null)
                Ask(sender, e);
        }

        private void OnShutdownRequest()
        {
            if (ShutdownRequest != null)
                ShutdownRequest(this, EventArgs.Empty);
        }

        private void OnConnectionError(ConnectionErrorEventArgs e)
        {
            if (ConnectionError != null)
                ConnectionError(this, e);
        }

        /// <summary>
        /// Raises the prompt event.
        /// </summary>
        /// <param name="sender">Object that has thrown the question.</param>
        /// <param name="e">Ask event arguments.</param>
        internal protected void OnPrompt(object sender, PromptEventArgs e)
        {
            if (Prompt != null)
                Prompt(sender, e);
        }

        /// <summary>
        /// Raises the list prompt event with the specified arguments.
        /// </summary>
        internal protected void OnShowSearch(ShowSearchEventArgs e)
        {
            if (ShowSearch != null)
                ShowSearch(this, e);
        }

        /// <summary>
        /// Raises the enquiry form prompt event with the specified arguments.
        /// </summary>
        internal protected void OnShowEnquiry(ShowEnquiryEventArgs e)
        {
            if (ShowEnquiry != null)
                ShowEnquiry(this, e);
        }

        /// <summary>
        /// Raises the enquiry form prompt event with the specified arguments.
        /// </summary>
        internal protected void OnShowExtendedData(ShowExtendedDataEventArgs e)
        {
            if (ShowExtendedData != null)
                ShowExtendedData(this, e);
        }

        /// <summary>
        /// Raises the enquiry wizard form prompt event with the specified arguments.
        /// </summary>
        internal protected void OnShowWizard(ShowEnquiryEventArgs e)
        {
            if (ShowWizard != null)
                ShowWizard(this, e);
        }

        #endregion

        #region Caches

        private bool EnableQueryCache
        {
            get
            {
                var reg = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Memory", "EnableQueryCaching", true);
                return reg.ToBoolean();
            }
        }


        private void ConfigureCache1()
        {
            CachedItems.Clear();
            CachedQueries.Clear();

            if (EnableQueryCache)
            {
                if (dbinfo.IsMonitoringEnabled)
                {
                    var qc_1 = new Caching.Queries.TablesQueryCache();
                    CachedItems.Add(qc_1.Key, qc_1.Cache);
                    CachedQueries.Add(qc_1);

                    var qc_2 = new Caching.Queries.StoredProcedureQueryCache();
                    CachedItems.Add(qc_2.Key, qc_2.Cache);
                    CachedQueries.Add(qc_2);

                    var qc_3 = new Caching.Queries.ParametersQueryCache();
                    CachedItems.Add(qc_3.Key, qc_3.Cache);
                    CachedQueries.Add(qc_3);

                    var qc_4 = new Caching.Queries.FieldsQueryCache();
                    CachedItems.Add(qc_4.Key, qc_4.Cache);
                    CachedQueries.Add(qc_4);

                    var qc_5 = new Caching.Queries.SchemaQueryLocalCache();
                    CachedItems.Add(qc_5.Key, qc_5.Cache);
                    CachedQueries.Add(qc_5);
                }
                else
                {

                    CachedQueries.Add<Caching.SchemaQueryCache>();
                    CachedItems.Add("OMS:SCHEMAS", new Caching.DataTableCache());

                    CachedItems.Add("OMS:DATABASEOBJECTS", new Caching.DataTableCache());
                    CachedQueries.Add<Caching.DatabaseObjectCache>();
                }
            }
        }


        private void ConfigureCache2()
        {
            if (EnableQueryCache && !_isAdminInstance)
            {
                if (dbinfo.IsMonitoringEnabled)
                {
                    var clkp_qc = new Caching.Queries.CodeLookupLocalQueryCache();
                    CachedItems.Add(clkp_qc.Key, clkp_qc.Cache);
                    CachedQueries.Add<Caching.Queries.ICodeLookupQueryCache>(clkp_qc);

                    var ass_qc = new Caching.Queries.AssemblyQueryCache();
                    CachedItems.Add(ass_qc.Key, ass_qc.Cache);
                    CachedQueries.Add(ass_qc);

                    var objs_qc = new Caching.Queries.OMSObjectsQueryCache();
                    CachedItems.Add(objs_qc.Key, objs_qc.Cache);
                    CachedQueries.Add(objs_qc);

                    var pack_qc = new Caching.Queries.PackagesQueryCache();
                    CachedItems.Add(pack_qc.Key, pack_qc.Cache);
                    CachedQueries.Add(pack_qc);

                    var ext_qc = new Caching.Queries.ExtensibilityQueryCache();
                    CachedItems.Add(ext_qc.Key, ext_qc.Cache);
                    CachedQueries.Add(ext_qc);

                    var app_qc = new Caching.Queries.ApplicationQueryCache();
                    CachedItems.Add(app_qc.Key, app_qc.Cache);
                    CachedQueries.Add(app_qc);

                    var sp_qc = new Caching.Queries.StorageProviderQueryCache();
                    CachedItems.Add(sp_qc.Key, sp_qc.Cache);
                    CachedQueries.Add(sp_qc);
                }
                else
                {
                    CachedItems.Add("OMS:CODELOOKUPS", new Caching.DataTableCache());
                    CachedQueries.Add<Caching.Queries.ICodeLookupQueryCache>(new Caching.CodeLookupQueryCache());
                }


                CachedItems.Add("OMS:CONFIGS", new Caching.DataTableCache());
                CachedQueries.Add<Caching.AddressFormatQueryCache>();
            }

            CachedItems.Add("SESSIONSTATE", this.SessionState);
        }


        private Caching.CachedItemCollection cacheitems;
        /// <summary>
        /// Gets a collection of cached items which will be cleared on logging off / disconnecting.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Caching.CachedItemCollection CachedItems
        {
            get
            {
                if (cacheitems == null)
                    cacheitems = new Caching.CachedItemCollection();
                return cacheitems;
            }
        }

        private Caching.DictionaryCache sessionstate;

        [EnquiryUsage(false)]
        [Browsable(false)]
        public Caching.DictionaryCache SessionState
        {
            get
            {
                if (sessionstate == null)
                    sessionstate = new Caching.DictionaryCache();
                return sessionstate;
            }
        }

        private Caching.QueryCacheCollection cachedqueries;
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Caching.QueryCacheCollection CachedQueries
        {
            get
            {
                if (cachedqueries == null)
                    cachedqueries = new Caching.QueryCacheCollection();
                return cachedqueries;
            }
        }

        /// <summary>
        /// Clears Cache.
        /// </summary>
        public void ClearCache()
        {
            ClearCache(false);
        }

        /// <summary>
        /// Clears Cache.
        /// </summary>
        public void ClearCache(bool all)
        {
            if (all)
            {
                try
                {
                    System.IO.Directory.Delete(Global.GetCachePath().ToString(), true);
                }
                catch { }
                try
                {
                    Global.GetTempPath().Delete(true);
                }
                catch { }
            }

            try
            {
                GetSystemInfo();
            }
            catch { }

            foreach (LimitCollection mc in MemoryCache.Values)
            {
                mc.Clear();
            }
            MemoryCache.Clear();

            this.dbver = null;
            this._sessionconfig = null;

            FieldParser.ClearFields();

            if (cacheitems != null)
            {
                foreach (Caching.ICacheable ci in cacheitems.Values)
                {
                    if (ci != null)
                    {
                        ci.Clear();
                    }
                }
            }
        }

        #endregion

        #region Memory Cache Properties

        //Holds all memory object caches.
        private System.Collections.Generic.Dictionary<string, FWBS.Common.LimitCollection> memorycache = new System.Collections.Generic.Dictionary<string, LimitCollection>();
        public System.Collections.Generic.Dictionary<string, FWBS.Common.LimitCollection> MemoryCache
        {
            get
            {
                return memorycache;
            }
        }

        /// <summary>
        /// Gets the current contacts that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentContacts
        {
            get
            {
                return GetMemoryCache("Contacts");
            }
        }

        /// <summary>
        /// Gets the current clients that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentClients
        {
            get
            {
                return GetMemoryCache("Clients");
            }
        }

        /// <summary>
        /// Gets the current files that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentFiles
        {
            get
            {
                return GetMemoryCache("Files");
            }
        }

        /// <summary>
        /// Gets the current precedents that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentAssociates
        {
            get
            {
                return GetMemoryCache("Associates");
            }
        }



        /// <summary>
        /// Gets the current precedents that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentPrecedents
        {
            get
            {
                return GetMemoryCache("Precedents");
            }
        }

        /// <summary>
        /// Gets the current documents that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentDocuments
        {
            get
            {
                return GetMemoryCache("Documents");
            }
        }

        /// <summary>
        /// Gets the current contact types that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentContactTypes
        {
            get
            {
                return GetMemoryCache("ContactTypes");
            }
        }

        /// <summary>
        /// Gets the current user types that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentUserTypes
        {
            get
            {
                return GetMemoryCache("UserTypes");
            }
        }

        /// <summary>
        /// Gets the current associate types that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentAssociateTypes
        {
            get
            {
                return GetMemoryCache("AssociateTypes");
            }
        }


        /// <summary>
        /// Gets the current command centre types that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentCommandCentreTypes
        {
            get
            {
                return GetMemoryCache("CentreTypes");
            }
        }

        /// <summary>
        /// Gets the current fee earner types that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentFeeEarnerTypes
        {
            get
            {
                return GetMemoryCache("FeeEarnerTypes");
            }
        }

        /// <summary>
        /// Gets the current client types that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentClientTypes
        {
            get
            {
                return GetMemoryCache("ClientTypes");
            }
        }

        /// <summary>
        /// Gets the current file types that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentFileTypes
        {
            get
            {
                return GetMemoryCache("FileTypes");
            }
        }

        /// <summary>
        /// Gets the current document types that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentDocumentTypes
        {
            get
            {
                return GetMemoryCache("DocumentTypes");
            }
        }

        /// <summary>
        /// Gets the current precedent types that have been recently looked at.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentPrecedentTypes
        {
            get
            {
                return GetMemoryCache("PrecedentTypes");
            }
        }

        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentFeeEarners
        {
            get
            {
                return GetMemoryCache("FeeEarners");
            }
        }


        [EnquiryUsage(false)]
        [Browsable(false)]
        public LimitCollection CurrentUsers
        {
            get
            {
                return GetMemoryCache("Users");
            }
        }

        private LimitCollection GetMemoryCache(string name)
        {
            CheckLoggedIn();

            if (memorycache.ContainsKey(name))
                return memorycache[name];
            else
            {
                ApplicationSetting regmax = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Memory", String.Format("Max{0}", name), 5);
                ApplicationSetting regmins = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Memory", String.Format("{0}Timeout", name), 10);

                int max = Convert.ToInt32(regmax.GetSetting());
                int mins = Convert.ToInt32(regmins.GetSetting());
                if (max < 1) max = 1;
                LimitCollection mc = new LimitCollection(max, mins);
                memorycache.Add(name, mc);
                return mc;
            }
        }

        #endregion

        #region Connection Methods

        /// <summary>
        /// Connection object.
        /// </summary>
        private Connection connection = null;
        private ConnectionWrapper connectionwrapper = null;
        private DatabaseInformation dbinfo = null;

        internal Connection Connection
        {
            get
            {
                return connection;
            }
        }

        public IConnection CurrentConnection
        {
            get
            {
                return connectionwrapper;
            }
        }

        /// <summary>
        /// Login routine which validates the type of login and what user credentials supplied.
        /// </summary>
        public void LogOn(int databaseSelection, string userName, string password)
        {
            LogOn(databaseSelection, userName, password, false);
        }

        /// <summary>
        /// Login routine which validates the type of login and what user credentials supplied.
        /// </summary>
        public void LogOn(int databaseSelection, string userName, string password, bool clearCache)
        {
            DatabaseConnections cnns = new DatabaseConnections(Global.ApplicationName, Global.ApplicationKey, Global.VersionKey);
            LogOn(cnns[databaseSelection], userName, password, clearCache);
        }


        /// <summary>
        /// Login routine which validates the type of login and what user credentials supplied.
        /// </summary>
        public void LogOn(DatabaseSettings settings, string userName, string password, bool clearCache)
        {
            this.databaseSettings = settings;

            if (IsAlreadyConnected)
            {
                if (singleton.AllowMultiLogin == false)
                {
                    Disconnect();
                    if (IsAlreadyConnected)
                    {
                        throw new Exception("Cannot log in whilst there is a session already connected.");
                    }
                }
            }
            InternalConnect(settings, userName, password, clearCache, true);

        }


        public void Connect()
        {
            if (IsLoggedIn)
                return;

            try
            {
                DatabaseSettings settings;
                string server;
                string database;
                string userId;
                string password;

                if (GetCachedConnectionSettings(out settings, out server, out database, out userId, out password))
                    InternalConnect(settings, userId, password, false, false);
                else
                    throw new Security.LoginException(HelpIndexes.CannotConnectToSession);
            }
            catch (Exception ex)
            {
                throw new Security.LoginException(ex, HelpIndexes.CannotConnectToSession);
            }
        }

        private void InternalConnect(DatabaseSettings settings, string userName, string password, bool clearCache, bool newsession)
        {

            shuttingDown = false;

            userName = (userName ?? "");
            password = (password ?? "");

            if (settings == null)
                throw new ArgumentNullException("settings");

            try
            {
                //Allows all calls, even those that need to be logged in to be, to be called during the log in procedure.
                _skiplogincheck = true;

                //Sets the memory database settings passed settings.
                _multidb = settings;

                if (Connection != null)
                {
                    Connection.ConnectionError -= new ConnectionErrorEventHandler(Connection_ConnectionError);
                    Connection.BeforeExecute -= new EventHandler<ExecuteEventArgs>(Connection_BeforeExecute);
                    Connection.BeforeExecuteTable -= new ExecuteTableEventHandler(Connection_BeforeExecuteTable);
                    Connection.BeforeExecuteDataSet -= new ExecuteDataSetEventHandler(Connection_BeforeExecuteDataSet);
                    Connection.AfterExecuteTable -= new ExecuteTableEventHandler(Connection_AfterExecuteTable);
                    Connection.AfterExecuteDataSet -= new ExecuteDataSetEventHandler(Connection_AfterExecuteDataSet);
                    Connection.ShutdownRequest -= new EventHandler(Connection_ShutdownRequest);
                    Connection.StateChanged -= new EventHandler<StateChangeEventArgs>(Connection_StateChanged);
                }

                //Set the session connection.
                connection = Connection.GetOMSConnection(_multidb, userName, password);
                dbinfo = new DatabaseInformation(connection);


                //Capture all errors from the database.
                Connection.ConnectionError += new ConnectionErrorEventHandler(Connection_ConnectionError);
                Connection.BeforeExecute += new EventHandler<ExecuteEventArgs>(Connection_BeforeExecute);
                Connection.BeforeExecuteTable += new ExecuteTableEventHandler(Connection_BeforeExecuteTable);
                Connection.BeforeExecuteDataSet += new ExecuteDataSetEventHandler(Connection_BeforeExecuteDataSet);
                Connection.AfterExecuteTable += new ExecuteTableEventHandler(Connection_AfterExecuteTable);
                Connection.AfterExecuteDataSet += new ExecuteDataSetEventHandler(Connection_AfterExecuteDataSet);
                Connection.ShutdownRequest += new EventHandler(Connection_ShutdownRequest);
                Connection.StateChanged += new EventHandler<StateChangeEventArgs>(Connection_StateChanged);

                //Keep the connection open at log on but make sure there is no exception catch
                //as the UI will want to receive any thrown errors, just use the finally block
                //to disconnect from the database..
                try
                {


                    Connection.Connect(true);

                    //DM - 18/06/04 - As Mike requested if any versions change.
                    //If the engine version or database version clear the cache.
                    ApplicationSetting engver = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "", "EngineVersion", "");
                    ApplicationSetting dbver = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "", "DatabaseVersion", "");
                    Version ev = EngineVersion;
                    Version dbv = DatabaseVersion;

                    if (!clearCache)
                    {
                        if (engver.GetSetting().ToString() != ev.ToString())
                            clearCache = true;
                        else
                            engver.SetSetting(ev.ToString());

                        if (dbver.GetSetting().ToString() != dbv.ToString())
                            clearCache = true;
                        else
                            dbver.SetSetting(dbv.ToString());
                    }

                    dbver.SetSetting(dbv.ToString());
                    engver.SetSetting(ev.ToString());

                    //Slear the cache if specified.
                    if (clearCache)
                        ClearCache(true);

                    IsConnecting = true;

                    //Configure the schema cache collections
                    ConfigureCache1();

                    //Make sure that the database information is loaded after the caching mechanism is put in place so that the schema can be made use of.
                    dbinfo.Load();

                    SetupContainer();

                    OnConnecting();

                    _distributionmanager = new DistributedAssemblyManager();

                    AssemblyManager.Register(new DistributedAssemblyResolver(AssemblyVersion, _distributionmanager));
                    AssemblyManager.Register(new Script.ScriptAssemblyResolver());

                    //Make sure that the entry point program is a valid registered API applicable client.
                    ValidateAPIClient();

                    //Configure the data cache collections
                    ConfigureCache2();

                    //Download multiple tables of system information.
                    GetSystemInfo();

                    //Download all user specific login informaiton.
                    GetLoginInfo(_multidb.LoginType, userName);


                    //Validate the user.
                    if (newsession)
                    {
                        //A flag that will check the passed password.
                        bool checkPassword = true;

                        switch (_multidb.LoginType)
                        {
                            case "OMS":	//Logs into OMS validating with dbUser and using a common SQL server login.
                                checkPassword = true;
                                break;
                            case "SQL":  //Actual user name and password validates the user as a SQL server login.
                                checkPassword = false;
                                break;
                            case "AAD":
                            case "NT":	//Uses NT to authenticate SQL server login then matches up with a OMS user.
                                checkPassword = false;
                                break;
                            case "ADID":
                                checkPassword = false;
                                break;
                            default:
                                goto case "OMS";
                        }



                        if (this.MaximumLoginAttempts > 0)
                        {
                            //If the password is wrong then updated the failed count.
                            if (this._currentUser.FailedLoginAttempts >= this.MaximumLoginAttempts)
                            {
                                this._currentUser.MakeInactive();
                                this._currentUser.ResetFailedAttempts();
                                this._currentUser.DoNotSetUpdatedFieldsOnNextUpdate = true;
                                this._currentUser.Update();
                            }
                        }

                        if (this._currentUser.UserTypeCode == "SERVICE" && _service == false)
                            throw new Security.ServiceUserException(userName);


                        //Check to see if the user is active.
                        if (!this._currentUser.IsActive)
                        {
                            //DM - Allow system and service user types to log in even if made inactive.
                            if (this._currentUser.UserTypeCode != "SYSTEM" && this._currentUser.UserTypeCode != "SERVICE")
                                throw new Security.InactiveOMSUserException(userName);
                        }

                        //DM - Skip the password check if the user is a service and the calling application is a service.
                        if (this._currentUser.UserTypeCode == "SERVICE" && _service == true)
                            checkPassword = false;

                        if (checkPassword)
                        {
                            //Validate that the password is correct.
                            if (!this._currentUser.ValidatePassword(ref password))
                            {
                                if (this.MaximumLoginAttempts > 0)
                                {
                                    this._currentUser.FailedLoginAttempts++;
                                    this._currentUser.DoNotSetUpdatedFieldsOnNextUpdate = true;
                                    this._currentUser.Update();
                                }
                                throw new Security.InvalidOMSPasswordException();
                            }
                        }
                    }

                    if (SupplyUserContext)
                        ApplyUserContext(_currentUser);

                    // Uninstall any Distributed Assembly that has been Deleted
                    _distributionmanager.UninstallCheck();

                    // Extract all Distributed Modules. Only if Updated
                    _distributionmanager.ExtractModules();

                    DynamicCatalog.Catalogs.Add(new DirectoryCatalog(_distributionmanager.DistributedAssembliesModuleDirectory, InternalSession));

                    //SetupContainer the wrappered connection after the container and API license manager has been intialized.
                    connectionwrapper = new ConnectionWrapper(connection, APILicenseManager);

                    //Load the users session script.
                    _sessionScript = _currentUser.Script;
                    if (_sessionScript != null)
                    {
                        _sessionScript.Load();
                        var sst = (Script.SessionScriptType)_sessionScript.Scriptlet;
                        sst.SetSessionObject(this);
                    }


                    if (newsession || _currentTerminal.IsLoggedIn == false)
                    {
                        //Log the activity.
                        Logging.CaptainsLog.CreateLoginEntry();

                        //Once logged in update the user, terminal and captains log tables.
                        this._currentUser.SetExtraInfo("usrLoggedIn", true);
                        this._currentUser.SetExtraInfo("usrLastLogin", DateTime.Now);
                        this._currentUser.SetExtraInfo("usrtermName", _currentTerminal.TerminalName);
                        this._currentUser.ResetFailedAttempts();
                        this._currentTerminal.SetVersionInfo(EngineVersion);
                        this._currentTerminal.SetExtraInfo("termLoggedIn", true);
                        this._currentTerminal.SetExtraInfo("termLastUser", _currentUser.ID);

                        //Added on Build 504 - 13/10/04
                        try
                        {
                            this._currentTerminal.SetExtraInfo("termLastUserInits", _currentUser.Initials);
                        }
                        catch { }

                        this._currentTerminal.SetExtraInfo("termLastLogin", DateTime.Now);
                        this._currentUser.DoNotSetUpdatedFieldsOnNextUpdate = true;
                        this._currentUser.Update();
                        this._currentTerminal.Update();


                    }
                    //Important, this needs to fire on all sessions, not just newsession = true
                    //TODO - check for NULL data - also does UPDATED flag get updated if the usrLastLogin and usrTermName etc are updated?
                    this._currentUserLastUpdated = ConvertDef.ToDateTimeNULL(this._currentUser.GetExtraInfo("UPDATED"), System.DateTime.UtcNow);
                    Debug.WriteLine(string.Format("_currentUserLastUpdated : {0}", _currentUserLastUpdated));


                    //If the program flow gets this far then flag the session as logged in.
                    this._loggedIn = _currentTerminal.IsLoggedIn;


                    //Apply the multi databse connection settings to the session file.
                    System.Collections.Generic.Dictionary<string, string> settingstopersist = new System.Collections.Generic.Dictionary<string, string>();
                    settingstopersist.Add("ConnectionIndex", _multidb.Number.ToString());
                    settingstopersist.Add("Server", _multidb.Server);
                    settingstopersist.Add("Database", _multidb.DatabaseName);
                    settingstopersist.Add("UserId", userName);
                    settingstopersist.Add("Password", Common.Security.Cryptography.Encryption.NewKeyEncrypt(password, 50));
                    singleton.SetProperties(settingstopersist);



                    //If logged in set the prefered culture information.
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(this.DefaultCulture);
                    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;


                    //Download frequently used system codelookups.
                    CacheLookups();

                    //Set the terminology settings.
                    _terminolgy = new Terminology();

                    //Set the commonly used resource strings.
                    _resources = new Res();

                    LoadServices();

                    SetAggregatorService();

                    OnConnected();

                    IsConnected = true;

                    // Check loaded Assemblies against the Assemblies in the Connecting Database
                    var warnings = _distributionmanager.CheckLoadedAssemblies();
                    if (warnings.Count > 0 && IsInDebug)
                    {
                        _distributionmanager.ShowUpdateWarning(warnings);
                    }


                    if (newsession)
                        OnLoggedIn();

                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                    throw;
                }
                finally
                {
                    IsConnecting = false;

                    Connection.Disconnect(true);
                }
            }
            finally
            {
                _skiplogincheck = false;
            }
        }


        private void SetupDefaultPowerProfile()
        {
            if (DataLists.Exists("dPUPList") && string.IsNullOrWhiteSpace(Convert.ToString(GetXmlProperty("DefaultPowerProfileID", ""))))
            {
                DefaultPowerProfile defaultProfile = new DefaultPowerProfile();
                if (!string.IsNullOrWhiteSpace(defaultProfile.PowerRoles) || !string.IsNullOrWhiteSpace(defaultProfile.PowerMenuItem))
                {
                    PowerProfile powerProfile = new PowerProfile();
                    powerProfile.PowerRoles = defaultProfile.PowerRoles;
                    powerProfile.PowerMenuItem = UpgradePowerMenuItem(defaultProfile.PowerMenuItem);  
                    powerProfile.Description = "Default Power User Profile";
                    powerProfile.Update();
                    SetXmlProperty("DefaultPowerProfileID", powerProfile.ID);
                    this.Update();
                }
            }
        }


        private string UpgradePowerMenuItem(string menuItems)
        {
            string[] menuItemsArray = menuItems.Split(';');
            int indexOfPower = Array.IndexOf(menuItemsArray, "POWER");
            if (indexOfPower >= 0)
            {
                menuItemsArray[indexOfPower] = "LPUProfiles";
                menuItems = string.Join(";", menuItemsArray);
            }
            return menuItems;
        }


        public bool IsInDebug
        {
            get
            {
                Common.ApplicationSetting aei = new Common.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Debug", "AdvancedErrorInfo", "true");
                return (aei.ToBoolean() || (IsLoggedIn && CurrentUser.IsInRoles("DEBUG")));
            }
        }

        private void SetAggregatorService()
        {
            ExternalAggregator.ServiceURL = Convert.ToString(GetSpecificData("ExtAggURL"));
        }

        private bool IsAlreadyConnected
        {
            get
            {
                return GetCachedConnectionSettings() || IsLoggedIn;
            }
        }

        private bool GetCachedConnectionSettings()
        {
            DatabaseSettings settings;
            string server;
            string database;
            string userId;
            string password;
            return GetCachedConnectionSettings(out settings, out server, out database, out userId, out password);

        }

        private bool GetCachedConnectionSettings(out DatabaseSettings settings, out string server, out string database, out string userId, out string password)
        {
            if (singleton == null)
            {
                singleton = new SessionSingleton();
            }

            singleton.Connect();

            settings = null;
            server = null;
            database = null;
            userId = null;
            password = null;

            int dbindex;
            try
            {
                if (int.TryParse(singleton.GetProperty("ConnectionIndex", "-1"), out dbindex))
                {
                    if (dbindex >= 0)
                    {
                        server = singleton.GetProperty("Server", "");
                        database = singleton.GetProperty("Database", "");
                        userId = singleton.GetProperty("UserId", "");
                        try
                        {
                            password = Common.Security.Cryptography.Encryption.NewKeyDecrypt(singleton.GetProperty("Password", ""));
                        }
                        catch { }

                        DatabaseConnections cnns = new DatabaseConnections(Global.ApplicationName, Global.ApplicationKey, Global.VersionKey);
                        settings = cnns[dbindex];

                        if (settings.DatabaseName == database && settings.Server == server)
                            return true;
                        else
                            return false;
                    }

                }
            }
            catch
            {
            }

            return false;
        }


        public ConnectedClientInfo[] GetConnectedClients()
        {
            if (singleton == null)
                singleton = new SessionSingleton();


            return singleton.GetConnectedClients();
        }

        /// <summary>
        /// Disconnects the current connected session.  This does not log the user
        /// of the system unless it is the last reference to the session.
        /// </summary>
        /// <returns>Success</returns>
        public bool Disconnect()
        {
            if (!shuttingDown)
                return InternalDisconnect(false);

            return true;
        }
        private bool InternalDisconnect(bool force)
        {
            PrintStackTrace("InternalDisconnect", "Session");

            bool check = _skiplogincheck;
            bool disconnectCancelled = false;
            try
            {
                IsDisconnecting = true;
                _skiplogincheck = true;

                if (!force)
                {
                    CancelEventArgs e = new CancelEventArgs(false);
                    OnDisconnecting(e);
                    if (e.Cancel)
                    {
                        disconnectCancelled = true;
                        return false;
                    }
                }

                if (singleton == null)
                    singleton = new SessionSingleton();
                UnloadServices();

                bool last = singleton.IsLastConnection;
                bool _clearcache = false;

                if (!force)
                {
                    if (_loggedIn)
                    {
                        try
                        {
                            Connection.Connect(true);
                            _clearcache = true;
                            _loggedIn = false;

                            if (last)
                            {
                                Logging.CaptainsLog.CreateLogoffEntry();

                                _currentUser.SetExtraInfo("usrLoggedIn", false);
                                _currentTerminal.SetExtraInfo("termLoggedIn", false);
                                _currentUser.DoNotSetUpdatedFieldsOnNextUpdate = true;
                                _currentUser.Update();
                                _currentTerminal.Update();


                            }
                            Logging.CaptainsLog.Flush();
                        }
                        finally
                        {
                            Connection.Disconnect(true);
                        }

                    }
                }

                _installedpackages = null;
                _isSearchConfigured = null;
                _isESSearchConfigured = null;
                _isMSSearchConfigured = null;
                _isSearchSummaryFieldEnabled = null;
                _minimalSymbolsCountForSuggests = null;
                _maximumSuggestsAmount = null;
                UseExternalBalances = null;

                singleton.Disconnect();

                IsConnected = false;

                OnDisconnected();

                if (last)
                    OnLoggedOff();

                if (_clearcache)
                    ClearCache(false);

                _currentBranch = null;

                Dispose();
            }
            finally
            {
                IsDisconnecting = false;
                _skiplogincheck = check;

                if (!disconnectCancelled)
                    DisposeContainer();
            }

            return true;
        }

        private static void PrintStackTrace(string description, string category)
        {
            try
            {
                Trace.WriteLine(string.Format("Start stack trace for method: {0}", description), category);

                StackTrace stackTrace = new StackTrace();           // get call stack
                StackFrame[] stackFrames = stackTrace.GetFrames();  // get method calls (frames)

                int order = 0;
                // write call stack method names
                foreach (StackFrame stackFrame in stackFrames)
                {
                    order++;
                    MethodBase method = stackFrame.GetMethod();
                    Type type = method.ReflectedType;

                    string methodName = method.Name;
                    string typeName = string.Empty;

                    if (type != null)
                        typeName = type.FullName;

                    Trace.WriteLine(string.Format("{0}: {1}.{2}", order, typeName, methodName), category);
                }

                Trace.WriteLine(string.Format("End stack trace for method: {0}", description), category);
            }
            catch { }
        }


        private void CacheLookups()
        {

            //Only cache the lookups at login when the registry key is valid
            //And it is not an Admin Kit instance.
            ApplicationSetting regquerycache = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Memory", "EnableQueryCaching", true);

            if (regquerycache.ToBoolean() && !IsTable("dbTableMonitor"))
            {
                if (_isAdminInstance == false)
                {
                    if (IsProcedureInstalled("sprCodeLookupCache"))
                    {
                        System.Collections.Generic.List<IDataParameter> paramlist = new System.Collections.Generic.List<IDataParameter>();
                        paramlist.Add(Connection.AddParameter("UI", SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentUICulture.Name));
                        Connection.ExecuteProcedureDataSet("sprCodeLookupCache", new string[] { "CACHEDLOOKUPS" }, paramlist.ToArray());
                    }
                }
            }
        }

        private void GetLoginInfo(string loginType, string userName)
        {
            if (IsProcedureInstalled("sprLogin"))
            {
                //Setup the default power profile for the current branch
                SetupDefaultPowerProfile();
                System.Collections.Generic.List<IDataParameter> pars = new System.Collections.Generic.List<IDataParameter>();
                pars.Add(Connection.CreateParameter("LOGINTYPE", loginType));
                pars.Add(Connection.CreateParameter("USERNAME", userName));
                pars.Add(Connection.CreateParameter("TERMNAME", Common.Functions.GetMachineName()));
                using (DataSet ds = Connection.ExecuteProcedureDataSet("sprLogin", new string[] { "USER", "TERMINAL", "PRINTER", "FEEUSER", "FEEEARNER", "FAVOURITES" }, pars.ToArray()))
                {
                    this._currentUser = new User(ds.Tables["USER"], userName);
                    this._currentTerminal = new Terminal(ds.Tables["TERMINAL"]);
                    if (ds.Tables["PRINTER"].Rows.Count == 0)
                        this._currentPrinter = null;
                    else
                        this._currentPrinter = new Printer(ds.Tables["PRINTER"]);

                    if (ds.Tables["FEEEARNER"].Rows.Count == 0)
                        this._currentFeeEarner = null;
                    else
                        this._currentFeeEarner = new FeeEarner(ds.Tables["FEEEARNER"], ds.Tables["FEEUSER"]);

                    this._currentFavourites = new Favourites(ds.Tables["FAVOURITES"], _currentUser.ID);
                }
            }
            else
            {
                this._currentUser = new User(loginType, userName);
                this._currentTerminal = new Terminal(Common.Functions.GetMachineName());
                this._currentPrinter = _currentUser.DefaultPrinter;
                this._currentFeeEarner = _currentUser.WorksFor;
                this._currentFavourites = new Favourites(_currentUser.ID);
            }

            if (_currentFeeEarner != null)
            {
                //TODO - check this
                this._currentFeeEarnerLastUpdated = ConvertDef.ToDateTimeNULL(this._currentFeeEarner.GetExtraInfo("UPDATED"), System.DateTime.UtcNow);
                Debug.WriteLine(string.Format("_currentFeeEarnerLastUpdated : {0}", _currentFeeEarnerLastUpdated));
                this._currentFeeEarnerUserLastUpdated = ConvertDef.ToDateTimeNULL(FWBS.OMS.User.GetUser(this._currentFeeEarner.ID).GetExtraInfo("UPDATED"), System.DateTime.UtcNow);
                Debug.WriteLine(string.Format("_currentFeeEarnerUserLastUpdated : {0}", _currentFeeEarnerUserLastUpdated));
            }
        }


        private static Power RefreshPowerUserProfile()
        {
            Power power = null;

            if (CurrentSession.IsTable("dbPowerUserProfiles"))
            {
                //check if the user has a power user policy assigned to them
                int powerUserProfileID = GetPowerUserProfileID();
                if (powerUserProfileID != 0)
                {
                    power = new Power(new PowerProfile(powerUserProfileID));
                }
                else
                {
                    power = AssignDefaultPUPtoUserWithPowerRole();
                }
            }
            else
            {
                power = new Power(new DefaultPowerProfile());
            }

            return power;
        }


        private static Power AssignDefaultPUPtoUserWithPowerRole()
        {
            Power power = null;

            int defaultpowerprofileid = ConvertDef.ToInt32(CurrentSession.CurrentBranch.GetXmlProperty("DefaultPowerProfileID", null), 0);
            if (defaultpowerprofileid != 0)
            {
                power = new Power(new PowerProfile(defaultpowerprofileid));

                AssignPUPIfNullAndHasPowerRole(defaultpowerprofileid);
            }
            else
            {
                power = Power.Empty;
            }

            return power;
        }


        private static void AssignPUPIfNullAndHasPowerRole(int defaultpowerprofileid)
        {
            string[] vals = CurrentSession.CurrentUser.Roles.Split(',');
            if (Array.IndexOf(vals, "POWER") > -1 && GetPowerUserProfileID() == 0)
            {
                CurrentSession.CurrentUser.SetExtraInfo("usrPowerUserProfileID", defaultpowerprofileid);
            }
        }

        private static int GetPowerUserProfileID()
        {
            return ConvertDef.ToInt32(CurrentSession.CurrentUser.GetExtraInfo("usrPowerUserProfileID"), 0);
        }


        private void GetSystemInfo()
        {
            _regInfo = Connection.ExecuteProcedureDataSet("sprSystemInfo", new String[5] { Session.Table, Session.Table_Branch, Session.Table_Log, Session.Table_LogType, Session.Table_Directory }, new IDataParameter[0]);

            if ((_regInfo == null) || (_regInfo.Tables[Table].Rows.Count == 0))
                throw new OMSException(HelpIndexes.NoCompanyRegistration);
            else
            {
                if (_regInfo.Tables.Contains(Table_Branch))
                    base.SetDataSource(_regInfo.Tables[Table_Branch]);
                else
                    base.SetDataSource(null);

            }

        }



        #endregion

        #region Validation Methods

        /// <summary>
        /// Checks to see if the critical data can be accessed by comparing 
        /// the calling and executing assembly key information to make sure that
        /// only FWBS can access the information.  Otherwise a security exception
        /// is raised. 
        /// </summary>
        /// <param name="callingAssembly">A reference to the calling assembly.</param>
        /// <param name="data">The data field trying to be accessed.</param>
        internal void CheckCriticalDataAccess(Assembly callingAssembly, string data)
        {
            if (ValidateCriticalDataAccess)
            {
                AssemblyName callingName = callingAssembly.GetName();
                AssemblyName execName = Assembly.GetExecutingAssembly().GetName();
                byte[] first = callingName.GetPublicKey();
                byte[] second = execName.GetPublicKey();
                bool ret = true;

                if (first.Length == second.Length)
                {
                    int length = first.Length;
                    for (int i = 0; i < length; i++)
                    {
                        if (first[i] != second[i])
                        {
                            ret = false;
                            break;
                        }
                    }
                }
                else
                    ret = false;

                if (ret == false)
                    throw new Security.AccessCiriticalDataException(data);
            }
        }

        /// <summary>
        /// Validates that the calling client application is actually a valid application
        /// with a license to use the API.
        /// </summary>
        private void ValidateAPIClient()
        {
            if (_apiConsumer == null)
                _apiConsumer = Assembly.GetEntryAssembly();

            if (_apiConsumer == null)
                _apiConsumer = Assembly.GetCallingAssembly();

            if (_apiConsumer == null)
                _apiConsumer = Assembly.GetExecutingAssembly();

            ValidateAPIClient(_apiConsumer, true);
        }

        internal void ValidateAPIClient(Assembly consumer)
        {
            ValidateAPIClient(consumer, false);
        }

        private void ValidateAPIClient(Assembly consumer, bool isHost)
        {
            if (IsProcedureInstalled("GetAPIConsumers"))
            {
                ValidateAPIClientNew(consumer, isHost);
            }
            else
                ValidateAPIClientOld(consumer, isHost);
        }

        private void ValidateAPIClientNew(Assembly consumer, bool isHost)
        {
            object[] atts = consumer.GetCustomAttributes(typeof(AssemblyAPIClientAttribute), false);
            if (atts.Length > 0)
            {
                AssemblyAPIClientAttribute attr = (AssemblyAPIClientAttribute)atts[0];
                string name = consumer.GetName().Name;
                Guid _appKey = attr.ApplicationKey;

                Fwbs.Framework.Licensing.API.IConsumerInfo info;
                if (!APILicenseManager.IsAllowed(consumer, out info))
                    throw new OMSException(HelpIndexes.AssemblyNotOMSClient, false, consumer.GetName().Name);

                if (isHost)
                    ApplyHostProfileFromAPIClient(_appKey, info);
            }
            else
                throw new OMSException(HelpIndexes.AssemblyNotOMSClient, false, consumer.GetName().Name);

        }


        private void ApplyHostProfileFromAPIClient(Guid _appKey, IDataReader rdr)
        {
            _clientType = (UIClientType)Enum.ToObject(typeof(UIClientType), rdr.GetByte(0));
            _designMode = rdr.GetBoolean(1);
            _service = rdr.GetBoolean(2);
            _isAdminInstance = (_appKey == new Guid("{31ED0BE4-3B00-469F-B50B-7CF27154AF9C}"));
        }

        private void ApplyHostProfileFromAPIClient(Guid _appKey, Fwbs.Framework.Licensing.API.IConsumerInfo info)
        {
            switch (info.Target)
            {
                case Fwbs.Framework.Licensing.API.ConsumerTarget.Mobile:
                    _clientType = UIClientType.PDA;
                    break;
                case Fwbs.Framework.Licensing.API.ConsumerTarget.Web:
                    _clientType = UIClientType.Web;
                    break;
                case Fwbs.Framework.Licensing.API.ConsumerTarget.Windows:
                    _clientType = UIClientType.Windows;
                    break;
                default:
                    _clientType = (UIClientType)(int)info.Target;
                    break;
            }

            _designMode = info.Type.HasFlag(Fwbs.Framework.Licensing.API.ConsumerType.Designer);
            _service = info.Type.HasFlag(Fwbs.Framework.Licensing.API.ConsumerType.Service);
            _isAdminInstance = info.Type.HasFlag(Fwbs.Framework.Licensing.API.ConsumerType.Admin) || _appKey == new Guid("{31ED0BE4-3B00-469F-B50B-7CF27154AF9C}");
        }

        private void ValidateAPIClientOld(Assembly consumer, bool isHost)
        {
            object[] atts = consumer.GetCustomAttributes(typeof(AssemblyAPIClientAttribute), false);
            if (atts.Length > 0)
            {
                AssemblyAPIClientAttribute attr = (AssemblyAPIClientAttribute)atts[0];
                string name = consumer.GetName().Name;
                Guid _appKey = attr.ApplicationKey;
                IDataParameter[] paramlist = new IDataParameter[2];
                paramlist[0] = Connection.AddParameter("ApplicationKey", SqlDbType.UniqueIdentifier, 0, _appKey);
                paramlist[1] = Connection.AddParameter("Code", SqlDbType.NVarChar, 100, name);
                try
                {
                    IDataReader rdr = Connection.ExecuteProcedureReader("sprAPILicenseCheck", paramlist);

                    bool rec = false;
                    while (rdr.Read())
                    {
                        rec = true;
                        ApplyHostProfileFromAPIClient(_appKey, rdr);
                        break;
                    }
                    rdr.Close();
                    if (!rec)
                        throw new OMSException(HelpIndexes.AssemblyNotOMSClient, false, consumer.GetName().Name);

                }
                finally
                {
                }
            }
            else
                throw new OMSException(HelpIndexes.AssemblyNotOMSClient, false, consumer.GetName().Name);

        }





        /// <summary>
        /// Raises an exception if the session is not logged in.
        /// </summary>
        public void CheckLoggedIn()
        {
            if (_skiplogincheck == false)
            {

                if (!IsLoggedIn)
                {
                    if (NotLoggedIn == null)
                        throw new Security.NotLoggedInException();
                    else
                        OnNotLoggedIn();
                }
            }
        }



        #endregion

        #region Methods

        private void LoadService(IService service)
        {
            if (service != null)
            {
                if (service.IsLoaded)
                    service.Unload();

                service.Load();

                if (!Services.Contains(service))
                    Services.Add(service);
            }
        }

        private void LoadServices()
        {
            //Initialise the connectivity services.
            LoadService(Security.SecurityManager.CurrentManager);

            //Load Services.
            if (EnableAddins)
            {
                LoadService(Addins);
            }

            //Initialise the connectivity services.
            LoadService(Connectivity.ConnectivityManager.CurrentManager);

            //Initialise the storage providers.
            LoadService(DocumentManagement.Storage.StorageManager.CurrentManager);

            //Initialise the registered OMS applications.
            LoadService(Apps.ApplicationManager.CurrentManager);
        }

        private void UnloadServices()
        {
            if (services != null)
            {
                foreach (IService srv in services)
                {
                    if (srv != null)
                    {
                        try
                        {
                            srv.Unload();
                            var disp = srv as IDisposable;
                            if (disp != null)
                                disp.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine(String.Format("Service unload error: {0}", ex.Message));
                        }
                    }
                }

                services.Clear();
            }
        }

        /// <summary>
        /// Is a OMS Stored Procedure and Version Installed
        /// </summary>
        /// <param name="ProcedureName">The Procedure Name</param>
        /// <param name="Version">Version to be Equals or Greater</param>
        /// <returns></returns>
        public bool IsOMSStoredProcedureInstalled(string ProcedureName, int Version)
        {
            try
            {
                IDataParameter[] paramlist = new IDataParameter[1];
                paramlist[0] = Connection.AddParameter("ProcedureName", SqlDbType.NVarChar, 200, ProcedureName);
                DataTable result = Connection.ExecuteProcedureTable("sprCheckspVersion", "Validate", paramlist);
                int v = ConvertDef.ToInt32(result.Rows[0][0], 0);
                if (v >= Version) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Stored Procedure Validation Failure.", ex);
            }
        }

        /// <summary>
        /// What date is numdays (Number of Days) for this user including company Holidays only.
        /// </summary>
        /// <param name="startDate">Startdate to calculate from</param>
        /// <param name="noOfdays">Number of days Positive or Negative to Add to the Startdate including Business Days of company holidays</param>
        /// <returns>Date</returns>
        public DateTime AddBusinessDays(DateTime startDate, int noOfdays)
        {
            // Use the Database functionality to run a Function to check the days
            CheckLoggedIn();
            object retval;
            string sql = "SELECT dbo.GetNextBusinessDate ( @startDate , @noDays , @usrID ,@UI) ";
            retval = Connection.ExecuteSQLScalar(sql, new IDataParameter[4] { Connection.AddParameter("usrID", SqlDbType.Int, 0, DBNull.Value), Connection.AddParameter("noDays", SqlDbType.Int, 0, noOfdays), Connection.AddParameter("startDate", SqlDbType.DateTime, 0, startDate), Connection.AddParameter("@UI", SqlDbType.NVarChar, 15, Thread.CurrentThread.CurrentCulture.Name) });
            if (retval is DBNull || retval == null)
                throw new NullReferenceException("Unable to get Business Days Function");
            else
                return (DateTime)retval;

        }

        /// <summary>
        /// How many business days are there between start and end date including the Holidays that are for the Company Holidays.
        /// </summary>
        /// <param name="startDate">Startdate to calculate from</param>
        /// <param name="endDate">endDate to calculate to</param>
        /// <returns>Number of days</returns>
        public int NoOfBusinessDays(DateTime startDate, DateTime endDate)
        {
            // Use the Database functionality to run a Function to check the days
            CheckLoggedIn();
            object retval;
            string sql = "SELECT dbo.GetNoOfBusinessDays ( @startDate , @endDate , @usrID ,@UI) ";
            retval = Connection.ExecuteSQLScalar(sql, new IDataParameter[4] { Connection.AddParameter("usrID", SqlDbType.Int, 0, DBNull.Value), Connection.AddParameter("endDate", SqlDbType.DateTime, 0, endDate), Connection.AddParameter("startDate", SqlDbType.DateTime, 0, startDate), Connection.AddParameter("@UI", SqlDbType.NVarChar, 15, Thread.CurrentThread.CurrentCulture.Name) });
            if (retval is DBNull || retval == null)
                throw new NullReferenceException("Unable to get Business Days Function");
            else
                return (int)retval;

        }


        /// <summary>
        /// Clears the current session favourites to force a re-load upon Client/Matter selection
        /// </summary>
        public void ClearCurrentFavourites()
        {
            _currentFavourites = null;
        }

        public bool IsVirtualDriveFeatureInstalled(out string mountPoint)
        {
            bool driveInstalled = File.Exists(Path.Combine(GetBaseApplicationLocation(), "OMS.Drive.dll")) &&
                ConvertDef.ToBoolean(new Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "VirtualDrive", "Enabled").GetSetting(true), true);

            if (driveInstalled && IsLoggedIn)
                mountPoint = Convert.ToString(GetSpecificData("MDRIVEMOUNTPNT"));
            else
                mountPoint = null;

            return driveInstalled;
        }

        /// <summary>
        /// Returns a Boolean if the Table Name exists
        /// </summary>
        /// <param name="name">The Table Name</param>
        /// <returns>Returns True if Package has been Installed</returns>
        public bool IsTable(string name)
        {
            return dbinfo.IsTable(name);
        }

        public bool IsProcedureInstalled(string name)
        {
            return dbinfo.IsProcedure(name);
        }



        /// <summary>
        /// Returns a Boolean if the Package Name has been Installed
        /// </summary>
        /// <param name="PackageName">The Package Name</param>
        /// <param name="ForceRefresh">This will force a refresh of installed packages from Server</param>
        /// <returns>Returns True if Package has been Installed</returns>
        public bool IsPackageInstalled(string PackageName, bool ForceRefresh)
        {
            if (_installedpackages == null || ForceRefresh)
                _installedpackages = FWBS.OMS.Design.Package.Packages.GetImportedPackageList();
            _installedpackages.DefaultView.RowFilter = "pkgCode = '" + PackageName.Replace("'", "''") + "'";
            return _installedpackages.DefaultView.Count > 0;
        }

        private readonly static MethodInfo IsPackageInstalledByNameMethod = typeof(Session).GetMethod("IsPackageInstalledByName", BindingFlags.Instance | BindingFlags.NonPublic);

        public bool IsPackageInstalled(string expression)
        {
            if (String.IsNullOrEmpty(expression))
                return IsPackageInstalledByName(expression);

            try
            {
                var exp = new SimpleExpression(IsPackageInstalledByNameMethod, expression);
                return exp.Evaluate(this);
            }
            catch
            {
                return false;
            }
        }

        private bool IsPackageInstalledByName(string PackageName)
        {
            return IsPackageInstalled(PackageName, false);
        }


        /// <summary>
        /// Validate Conditional
        /// </summary>
        /// <param name="obj">The Oms Type</param>
        /// <param name="conditions">The Conditions</param>
        /// <returns>Boolean</returns>
        public bool ValidateConditional(object obj, string[] conditions)
        {
            return ValidateConditional(obj, conditions, false);
        }

        public bool ValidateConditional(object obj, string[] conditions, bool onValidateFailError)
        {
            string result = "";
            try
            {
                if (conditions.Length == 1 && conditions[0] == "") { return true; }
                if (conditions.Length == 0) { return true; }

                foreach (string cmdline in conditions)
                {
                    string[] cmds = cmdline.Split('(');
                    string cmd = "";
                    if (cmds.Length > 0)
                        cmd = cmds[0].ToLower();
                    string[] parameters = new string[] { "" };
                    if (cmds.Length > 1)
                        parameters = cmds[1].Trim(@")""".ToCharArray()).Replace("\"", "").Split(@",".ToCharArray());

                    switch (cmd)
                    {
                        case "":
                            break;
                        case "issearchconfigured":
                            {
                                if (!IsSearchConfigured) { result = "requires Search to be configured"; return false; }
                            }
                            break;
                        case "isspversioninstalled":
                            {
                                if (IsOMSStoredProcedureInstalled(parameters[0], ConvertDef.ToInt32(parameters[1], 0)) == false) { result = String.Format("requires Stored Procedure '{0}' with Version '{1}' or higher", parameters[0], parameters[1]); return false; }
                            }
                            break;
                        case "!isspversioninstalled":
                            {
                                if (IsOMSStoredProcedureInstalled(parameters[0], ConvertDef.ToInt32(parameters[1], 0)) == true) { result = String.Format("requires Stored Procedure '{0}' to be removed", parameters[0]); return false; }
                            }
                            break;
                        case "ispackageinstalled":
                            {
                                if (IsPackageInstalled(parameters[0]) == false) { result = String.Format("requires Package '{0}'", parameters[0]); return false; }
                            }
                            break;
                        case "!ispackageinstalled":
                            {
                                if (IsPackageInstalled(parameters[0]) == true) { result = String.Format("requires Package '{0}' to be removed", parameters[0]); return false; }
                            }
                            break;
                        case "islicensedfor":
                            {
                                if (IsLicensedFor(parameters[0]) == false) { result = String.Format("requires License '{0}'", parameters[0]); return false; }
                            }
                            break;
                        case "!islicensedfor":
                            {
                                if (IsLicensedFor(parameters[0]) == true) { result = String.Format("requires License '{0}' to be removed", parameters[0]); return false; }
                            }
                            break;
                        case "istable":
                            {
                                if (IsTable(parameters[0]) == false) { result = String.Format("requires Table '{0}'", parameters[0]); return false; }
                            }
                            break;
                        case "!istable":
                            {
                                if (IsTable(parameters[0]) == true) { result = String.Format("requires Table '{0}' to be removed", parameters[0]); return false; }
                            }
                            break;
                        case "issecuritygranted":
                            {
                                if (IsOMSObjectSecurityGranted(parameters[0], obj) == false) { result = String.Format("Requires permission '{0}'", parameters[0]); return false; }
                            }
                            break;
                        case "!issecuritygranted":
                            {
                                if (IsOMSObjectSecurityGranted(parameters[0], obj) == true) { result = String.Format("Requires permission '{0}' to be removed", parameters[0]); return false; }
                            }
                            break;
                        case "islogintype":
                            {
                                if (_multidb.LoginType.Equals(parameters[0]) == false) { result = String.Format("requires login type '{0}'", parameters[0]); return false; }
                            }
                            break;
                        case "!islogintype":
                            {
                                if (_multidb.LoginType.Equals(parameters[0]) == true) { result = String.Format("requires login type not to be '{0}'", parameters[0]); return false; }
                            }
                            break;
                        default:
                            {
                                if (obj is FWBS.OMS.Interfaces.IConditional)
                                {
                                    string objcond = ((FWBS.OMS.Interfaces.IConditional)obj).Conditionals.ToLower();
                                    string[] brkcond = cmd.Split(';');
                                    bool flag = true;
                                    foreach (string cond in brkcond)
                                    {
                                        if (objcond.IndexOf(cond) == -1)
                                            flag = false;
                                    }
                                    if (flag == false) { result = "Conditional Failiure"; return false; }
                                }
                            }
                            break;
                    }
                }
                return true;
            }
            finally
            {
                if (result != "" && onValidateFailError)
                    throw new Exception(result);
            }
        }

        private bool IsOMSObjectSecurityGranted(string permType, object obj)
        {
            bool isGranted = true;
            Security.Permissions.StandardPermissionType permissionType = Security.Permissions.Permission.StringToStandardType(permType);
            if (permissionType != Security.Permissions.StandardPermissionType.Unknown)
            {
                isGranted = Security.SecurityManager.CurrentManager.IsGranted(new Security.Permissions.SystemPermission(permissionType));
                if (isGranted)
                {
                    var permission = Security.Permissions.Permission.CreatePermission(permissionType, obj);
                    isGranted = (permission == null) || Security.SecurityManager.CurrentManager.IsGranted(permission);
                }
            }
            return isGranted;
        }

        public void SendMail(System.Net.Mail.MailMessage mail)
        {
            if (mail == null)
                throw new ArgumentNullException("mail");

            using (System.Net.Mail.SmtpClient smtp = CreateSmtpClient())
                smtp.Send(mail);
        }

        /// <summary>
        /// Send a SMPT mail.
        /// </summary>
        public void SendMail(string from, string to, string subject, string body)
        {
            using (System.Net.Mail.SmtpClient smtp = CreateSmtpClient())
                smtp.Send(from, to, subject, body);
        }

        private System.Net.Mail.SmtpClient CreateSmtpClient()
        {
            string[] smtpServer = SMTPServer.Split(':');
            System.Net.Mail.SmtpClient smtpClient = (smtpServer.Length == 1) ? new System.Net.Mail.SmtpClient(smtpServer[0]) : new System.Net.Mail.SmtpClient(smtpServer[0], Convert.ToInt32(smtpServer[1]));

            smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = true;

            string smtpEncryption = SMTPEncryption;
            if (!string.IsNullOrEmpty(smtpEncryption) && smtpEncryption != "NONE")
            {
                smtpClient.EnableSsl = true;
                if (smtpEncryption == "STARTTLS")
                    smtpClient.TargetName = smtpEncryption + "/" + smtpClient.Host;
            }

            return smtpClient;
        }

        public string GetExternalDocumentPropertyName(string name)
        {
            if (name == null)
                name = String.Empty;
            else
                name = name.ToUpperInvariant();

            string prefix = ExternalDocumentPropertyPrefix;
            if (String.IsNullOrEmpty(prefix))
                return name;
            else
            {
                if (name.StartsWith(prefix))
                    return name;
                else
                {
                    string[] names = ExternalDocumentPropertyNames.ToUpperInvariant().Split(';');

                    Array.Sort(names);

                    if (Array.IndexOf<string>(names, name) > -1)
                        return prefix + name;
                    else
                        return name;
                }
            }
        }

        public string GetInternalDocumentPropertyName(string name)
        {
            if (name == null)
                name = String.Empty;
            else
                name = name.ToUpperInvariant();

            string prefix = ExternalDocumentPropertyPrefix;
            if (String.IsNullOrEmpty(prefix))
                return name;
            else
            {
                if (name.StartsWith(prefix))
                {
                    string newname = name.Remove(0, prefix.Length);

                    string[] names = ExternalDocumentPropertyNames.ToUpperInvariant().Split(';');

                    Array.Sort(names);

                    if (Array.IndexOf<string>(names, newname) > -1)
                        return newname;
                    else
                        return name;
                }
            }

            return name;
        }

        #region DEFAULT SYSTEM SEARCH LIST GROUPS

        public string DefaultSystemSearchListGroups(SystemSearchListGroups group)
        {
            CheckLoggedIn();
            string code = String.Empty;

            code = _currentUser.GetSystemSearchListGroups(group);

            if (code == String.Empty)
                code = Convert.ToString(GetSessionConfigSetting("/config/defaultSearchListsGroups/" + group.ToString(), String.Empty));

            ApplicationSetting regmax = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "DefaultsVersion", "", 2);
            int version = Convert.ToInt32(regmax.GetSetting());

            if (code == String.Empty && version == 1)
            {
                switch (group)
                {
                    case SystemSearchListGroups.None:
                        code = "NOTSET";
                        break;
                    case SystemSearchListGroups.Client: //CLIENT
                        code = "CLIENT";
                        break;
                    case SystemSearchListGroups.ClientConflict: //CLIENTCONFLICT
                        code = "CLIENTCONFLICT";
                        break;
                    case SystemSearchListGroups.ContactAssociate: //CONTACTASS
                        code = "CONTACTASS";
                        break;
                    case SystemSearchListGroups.ContactConflict: //CONTACT
                        code = "CONTACT";
                        break;
                    case SystemSearchListGroups.SearchManagerContact: //CONTACTSM
                        code = "CONTACTSM";
                        break;
                    case SystemSearchListGroups.SearchManagerClient: //CLSM
                        code = "CLSM";
                        break;
                    case SystemSearchListGroups.SearchManagerFile: //CLFILESM
                        code = "CLFILESM";
                        break;
                    case SystemSearchListGroups.Package: //PACKAGE
                        code = "PACKAGE";
                        break;
                    case SystemSearchListGroups.User: //USER
                        code = "USER";
                        break;
                    case SystemSearchListGroups.FeeEarner: //FEEEARNER
                        code = "FEEEARNER";
                        break;
                    case SystemSearchListGroups.Document: //DOCUMENT
                        code = "DOCUMENT";
                        break;
                    case SystemSearchListGroups.DocumentAll: //DOCUMENTALL
                        code = "DOCUMENTALL";
                        break;
                    case SystemSearchListGroups.ClientFile: //CLIENT&FILE
                        code = "CLIENT&FILE";
                        break;
                    case SystemSearchListGroups.FindCodeLookup: //FINDCDLK
                        code = "FINDCDLK";
                        break;
                    case SystemSearchListGroups.DocumentClient: //DOCUMENTCLT
                        code = "DOCUMENTCLT";
                        break;
                    case SystemSearchListGroups.DocumentLast: //DOCUMENTLAST
                        code = "DOCUMENTLAST";
                        break;
                    case SystemSearchListGroups.DocumentLatestUpdate:
                        code = "DOCLUPDATE";
                        break;
                    case SystemSearchListGroups.DocumentLatestOpened:
                        code = "DOCLOPENED";
                        break;
                    case SystemSearchListGroups.SelectMilestone: //MILESTONECNF
                        code = "MILESTONECNF";
                        break;
                }
            }
            else if (code == String.Empty && version == 2)
            {
                switch (group)
                {
                    case SystemSearchListGroups.None:
                        code = "NOTSET";
                        break;
                    case SystemSearchListGroups.Client: //CLIENT
                        code = "GRPCLICLIENT";
                        break;
                    case SystemSearchListGroups.ClientConflict: //CLIENTCONFLICT
                        code = "GRPCLICONFLICT";
                        break;
                    case SystemSearchListGroups.ContactAssociate: //CONTACTASS
                        code = "GRPCONCONTASS";
                        break;
                    case SystemSearchListGroups.ContactConflict: //CONTACT
                        code = "GRPCONCONTACT";
                        break;
                    case SystemSearchListGroups.SearchManagerContact: //CONTACTSM
                        code = "GRPCONCONTSM";
                        break;
                    case SystemSearchListGroups.SearchManagerClient: //CLSM
                        code = "GRPCLICLSM";
                        break;
                    case SystemSearchListGroups.SearchManagerFile: //CLFILESM
                        code = "GRPCLIFILESM";
                        break;
                    case SystemSearchListGroups.Package: //PACKAGE
                        code = "GRPSYSPACKAGE";
                        break;
                    case SystemSearchListGroups.User: //USER
                        code = "GRPUSRUSER";
                        break;
                    case SystemSearchListGroups.FeeEarner: //FEEEARNER
                        code = "FEEEARNER";
                        break;
                    case SystemSearchListGroups.Document: //DOCUMENT
                        code = "GRPDOCDOC";
                        break;
                    case SystemSearchListGroups.DocumentAll: //DOCUMENTALL
                        code = "GRPDOCALL";
                        break;
                    case SystemSearchListGroups.ClientFile: //CLIENT&FILE
                        code = "GRPCLIFILE";
                        break;
                    case SystemSearchListGroups.FindCodeLookup: //FINDCDLK
                        code = "GRPSYSFINDCDLK";
                        break;
                    case SystemSearchListGroups.DocumentClient: //DOCUMENTCLT
                        code = "GRPDOCCLT";
                        break;
                    case SystemSearchListGroups.DocumentContact:
                        code = "GRPDOCCNT";
                        break;
                    case SystemSearchListGroups.DocumentLast: //DOCUMENTLAST
                        code = "GRPDOCLAST";
                        break;
                    case SystemSearchListGroups.DocumentCheckedOut:
                        code = "GRPDOCCHECKOUT";
                        break;
                    case SystemSearchListGroups.DocumentLocal:
                        code = "GRPDOCLOCAL";
                        break;
                    case SystemSearchListGroups.DocumentLatestUpdate:
                        code = "GRPDOCLATEST";
                        break;
                    case SystemSearchListGroups.DocumentLatestOpened:
                        code = "GRPDOCLOPENED";
                        break;
                    case SystemSearchListGroups.SelectMilestone: //MILESTONECNF
                        code = "GRPFILMILESTONE";
                        break;
                    case SystemSearchListGroups.AddressService:
                        code = "GRPCONADDWEBSRV";
                        break;
                    case SystemSearchListGroups.Address:
                        code = "GRPCONADDRESS";
                        break;
                    case SystemSearchListGroups.FMSelectMilestone:
                        code = "GRPFMMILESTONE";
                        break;
                }
            }

            if (code == String.Empty)
                return DefaultSystemSearchListGroups(group.ToString());
            else
                return code;
        }

        //NOTE: USE THIS ONE FOR NOW ON.

        public string DefaultSystemSearchListGroups(string group)
        {
            CheckLoggedIn();
            string code = String.Empty;

            code = _currentUser.GetSystemSearchListGroups(group);

            if (code == String.Empty)
                code = Convert.ToString(GetSessionConfigSetting("/config/defaultSearchListsGroups/" + group, String.Empty));

            ApplicationSetting regmax = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "DefaultsVersion", "", 2);
            int version = Convert.ToInt32(regmax.GetSetting());

            if (code == String.Empty && version == 1)
            {
                switch (group.ToLower())
                {
                    case "none":
                        code = "NOTSET";
                        break;
                    case "client": //CLIENT
                        code = "CLIENT";
                        break;
                    case "clientconflict": //CLIENTCONFLICT
                        code = "CLIENTCONFLICT";
                        break;
                    case "contactassociate": //CONTACTASS
                        code = "CONTACTASS";
                        break;
                    case "contactconflict": //CONTACT
                        code = "CONTACT";
                        break;
                    case "searchmanagercontact": //CONTACTSM
                        code = "CONTACTSM";
                        break;
                    case "searchmanagerclient": //CLSM
                        code = "CLSM";
                        break;
                    case "searchmanagefile": //CLFILESM
                        code = "CLFILESM";
                        break;
                    case "package": //PACKAGE
                        code = "PACKAGE";
                        break;
                    case "user": //USER
                        code = "USER";
                        break;
                    case "feeearner": //FEEEARNER
                        code = "FEEEARNER";
                        break;
                    case "document": //DOCUMENT
                        code = "DOCUMENT";
                        break;
                    case "documentall": //DOCUMENTALL
                        code = "DOCUMENTALL";
                        break;
                    case "clientfile": //CLIENT&FILE
                        code = "CLIENT&FILE";
                        break;
                    case "findcodelookup": //FINDCDLK
                        code = "FINDCDLK";
                        break;
                    case "documentclient": //DOCUMENTCLT
                        code = "DOCUMENTCLT";
                        break;
                    case "documentlast": //DOCUMENTLAST
                        code = "DOCUMENTLAST";
                        break;
                    case "selectmilestone": //MILESTONECNF
                        code = "MILESTONECNF";
                        break;
                }
            }
            else if (code == String.Empty && version == 2)
            {
                switch (group.ToLower())
                {
                    case "none":
                        code = "NOTSET";
                        break;
                    case "client": //CLIENT
                        code = "GRPCLICLIENT";
                        break;
                    case "clientconflict": //CLIENTCONFLICT
                        code = "GRPCLICONFLICT";
                        break;
                    case "contactassociate": //CONTACTASS
                        code = "GRPCONCONTASS";
                        break;
                    case "contactconflict": //CONTACT
                        code = "GRPCONCONTACT";
                        break;
                    case "searchmanagercontact": //CONTACTSM
                        code = "GRPCONCONTSM";
                        break;
                    case "searchmanagerclient": //CLSM
                        code = "GRPCLICLSM";
                        break;
                    case "searchmanagerfile": //CLFILESM
                        code = "GRPCLIFILESM";
                        break;
                    case "package": //PACKAGE
                        code = "GRPSYSPACKAGE";
                        break;
                    case "user": //USER
                        code = "GRPUSRUSER";
                        break;
                    case "feeearner": //FEEEARNER
                        code = "FEEEARNER";
                        break;
                    case "document": //DOCUMENT
                        code = "GRPDOCDOC";
                        break;
                    case "documentall": //DOCUMENTALL
                        code = "GRPDOCALL";
                        break;
                    case "clientfile": //CLIENT&FILE
                        code = "GRPCLIFILE";
                        break;
                    case "findcodelookup": //FINDCDLK
                        code = "GRPSYSFINDCDLK";
                        break;
                    case "documentclient": //DOCUMENTCLT
                        code = "GRPDOCCLT";
                        break;
                    case "documentlast": //DOCUMENTLAST
                        code = "GRPDOCLAST";
                        break;
                    case "selectmilestone": //MILESTONECNF
                        code = "GRPFILMILESTONE";
                        break;
                    case "addressservice":
                        code = "GRPCONADDWEBSRV";
                        break;
                    case "documentcheckedout":
                        code = "GRPDOCCHECKOUT";
                        break;
                    case "documentlocal":
                        code = "GRPDOCLOCAL";
                        break;
                    case "documentlatestupdate":
                        code = "GRPDOCLATEST";
                        break;
                    case "documentlatestopen":
                        code = "GRPDOCLATESTOPEN";
                        break;
                    case "address":
                        code = "GRPCONADDRESS";
                        break;
                }
            }
            return code;
        }

        #endregion

        #region DEFAULT SYSTEM SEARCH LISTS

        public string DefaultSystemSearchList(SystemSearchLists list)
        {
            CheckLoggedIn();
            string code = String.Empty;

            code = _currentUser.GetSystemSearchLists(list);

            if (code == String.Empty)
                code = Convert.ToString(GetSessionConfigSetting("/config/defaultSearchLists/" + list.ToString(), String.Empty));

            ApplicationSetting regmax = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "DefaultsVersion", "", 2);
            int version = Convert.ToInt32(regmax.GetSetting());

            if (code == String.Empty && version == 1)
            {
                switch (list)
                {
                    case SystemSearchLists.None:
                        code = "NOTSET";
                        break;
                    case SystemSearchLists.AssociateFilter: //SYSASSOCFILTER
                        code = "SYSASSOCFILTER";
                        break;
                    case SystemSearchLists.Associates: //SYSASSOCLIST
                        code = "SYSASSOCLIST";
                        break;
                    case SystemSearchLists.Appointments: //APPOINTMENTS
                        code = "APPOINTMENTS";
                        break;
                    case SystemSearchLists.Contact: //SYSCONTACTLIST
                        code = "SYSCONTACTLIST";
                        break;
                    case SystemSearchLists.File: //SYSFILELIST
                        code = "SYSFILELIST";
                        break;
                    case SystemSearchLists.TimeRecording: //SYSTIMEREC
                        code = "SYSTIMEREC";
                        break;
                    case SystemSearchLists.FieldCommon: //SYSFLDCOMMON
                        code = "SYSFLDCOMMON";
                        break;
                    case SystemSearchLists.FieldAssociates: //SYSFLDASSOC
                        code = "SYSFLDASSOC";
                        break;
                    case SystemSearchLists.FieldAppointments: //SYSFLDAPP
                        code = "SYSFLDAPP";
                        break;
                    case SystemSearchLists.FieldExtendedData: //SYSFLDEXTLIST
                        code = "SYSFLDEXTLIST";
                        break;
                    case SystemSearchLists.DocumentTimeRecording: //SYSDOCTIMEREC
                        code = "SYSDOCTIMEREC";
                        break;
                    case SystemSearchLists.Precedent: //SYSPRECLIST
                        code = "SYSPRECLIST";
                        break;
                    case SystemSearchLists.PrecedentFilter: //SYSPRECFILTER
                        code = "SYSPRECFILTER";
                        break;
                    case SystemSearchLists.RemoteAssociates: //ASSOCREMOTE
                        code = "ASSOCREMOTE";
                        break;
                    case SystemSearchLists.Help: //SYSHELP
                        code = "SYSHELP";
                        break;
                    case SystemSearchLists.SearchListPicker: //SCHPICKER
                        code = "SCHPICKER";
                        break;
                    case SystemSearchLists.ReportPicker: // RPTPICKER
                        code = "RPTPICKER";
                        break;
                    case SystemSearchLists.DocumentAttachments: //DOCUMENTATT
                        code = "DOCUMENTATT";
                        break;
                    case SystemSearchLists.Address: //ADDRESS
                        code = "ADDRESS";
                        break;
                    case SystemSearchLists.SearchContacts: //CONTACTS
                        code = "CONTACTS";
                        break;
                    ////////////////////// # PACKAGE AND DEPLOY LISTS #////////////////////////////
                    case SystemSearchLists.PackageCodeLookups:
                        code = "PDCDLOOKUPS";
                        break;
                    case SystemSearchLists.PackageEnquiry:
                        code = "PDENQUIRY";
                        break;
                    case SystemSearchLists.PackageExtendedData:
                        code = "PDEXTEND";
                        break;
                    case SystemSearchLists.PackageObjects:
                        code = "PDOBJECTS";
                        break;
                    case SystemSearchLists.PackagePrecedents:
                        code = "PDPRECLIST";
                        break;
                    case SystemSearchLists.PackageReports:
                        code = "PDREPORTS";
                        break;
                    case SystemSearchLists.PackageScripts:
                        code = "PDSCRIPTS";
                        break;
                    case SystemSearchLists.PackageSearchLists:
                        code = "PDSEARCHLISTS";
                        break;
                    case SystemSearchLists.PackageSqlScripts:
                        code = "PDSQLSCRIPTS";
                        break;


                    /////////////////////////////////////////////////////////
                    case SystemSearchLists.OyezAliases: //OYEZALIAS
                        code = "OYEZALIAS";
                        break;
                    case SystemSearchLists.LaserAliases: //LASERALIAS
                        code = "LASERALIAS";
                        break;
                    case SystemSearchLists.DocumentDuplicates: // DOCUMENTSDUP
                        code = "DOCUMENTSDUP";
                        break;
                    case SystemSearchLists.ReportServerPicker:
                        code = "SCHRSRPTPICKER";
                        break;
                    case SystemSearchLists.DocumentLog:
                        code = "SCHDOCLOG";
                        break;
                }
            }
            else if (code == String.Empty && version == 2)
            {
                switch (list)
                {
                    case SystemSearchLists.None:
                        code = "NOTSET";
                        break;
                    case SystemSearchLists.AssociateFilter: //SYSASSOCFILTER
                        code = "SCHASSFILTER";
                        break;
                    case SystemSearchLists.Associates: //SYSASSOCLIST
                        code = "SCHFILASSLIST2";
                        break;
                    case SystemSearchLists.AssociatesShort:
                        code = "SCHFILASSLISTST";
                        break;
                    case SystemSearchLists.Appointments: //APPOINTMENTS
                        code = "SCHFILAPPOINTS";
                        break;
                    case SystemSearchLists.Contact: //SYSCONTACTLIST
                        code = "SCHCLICONTACTS";
                        break;
                    case SystemSearchLists.File: //SYSFILELIST
                        code = "SCHFILTRANSLIST";
                        break;
                    case SystemSearchLists.TimeRecording: //SYSTIMEREC
                        code = "SCHFILTIMERECD";
                        break;
                    case SystemSearchLists.FieldCommon: //SYSFLDCOMMON
                        code = "SCHSYSCOMFIELDS";
                        break;
                    case SystemSearchLists.FieldAssociates: //SYSFLDASSOC
                        code = "SCHASSFIELDS";
                        break;
                    case SystemSearchLists.FieldAppointments: //SYSFLDAPP
                        code = "SCHFILAPPFIELDS";
                        break;
                    case SystemSearchLists.FieldExtendedData: //SYSFLDEXTLIST
                        code = "SCHSYSSCHFLDS";
                        break;
                    case SystemSearchLists.DocumentTimeRecording: //SYSDOCTIMEREC
                        code = "SCHDOCTIME";
                        break;
                    case SystemSearchLists.Precedent: //SYSPRECLIST
                        code = "SCHSYSPRECLST";
                        break;
                    case SystemSearchLists.PrecedentFavourites: //SCHSYSPRECFAV
                        code = "SCHSYSPRECFAV";
                        break;
                    case SystemSearchLists.PrecedentFilter: //SYSPRECFILTER
                        code = "SCHSYSPRECFLTR";
                        break;
                    case SystemSearchLists.RemoteAssociates: //ASSOCREMOTE
                        code = "SCHASSREMOTE";
                        break;
                    case SystemSearchLists.Help: //SYSHELP
                        code = "SCHSYSHELP";
                        break;
                    case SystemSearchLists.SearchListPicker: //SCHPICKER
                        code = "SCHSYSSCHPICK";
                        break;
                    case SystemSearchLists.ReportPicker: // RPTPICKER
                        code = "SCHSYSRPTPICK";
                        break;
                    case SystemSearchLists.DocumentAttachments: //DOCUMENTATT
                        code = "SCHDOCATTLST";
                        break;
                    case SystemSearchLists.Address: //ADDRESS
                        code = "SCHCONADDRESS";
                        break;
                    case SystemSearchLists.SearchContacts: //CONTACTS
                        code = "SCHCONSEARCH";
                        break;
                    case SystemSearchLists.WorkflowPicker:
                        code = "SCHWFPICKER";
                        break;
                    ////////////////////// # PACKAGE AND DEPLOY LISTS #////////////////////////////
                    case SystemSearchLists.PackageCodeLookups:
                        code = "SCHSYSPKGCODE";
                        break;
                    case SystemSearchLists.PackageEnquiry:
                        code = "SCHSYSPKGENQ";
                        break;
                    case SystemSearchLists.PackageExtendedData:
                        code = "SCHSYSPKGEXTD";
                        break;
                    case SystemSearchLists.PackageObjects:
                        code = "SCHSYSPKGOBJ";
                        break;
                    case SystemSearchLists.PackagePrecedents:
                        code = "SCHSYSPKGPREC2";
                        break;
                    case SystemSearchLists.PackageReports:
                        code = "SCHSYSPKGRPT";
                        break;
                    case SystemSearchLists.PackageScripts:
                        code = "SCHSYSPKGSCRPT";
                        break;
                    case SystemSearchLists.PackageSearchLists:
                        code = "SCHSYSPKGSCH";
                        break;
                    case SystemSearchLists.PackageSqlScripts:
                        code = "SCHSYSPKGSQL";
                        break;
                    case SystemSearchLists.PackageDataLists:
                        code = "SCHSYSPKGDATALS";
                        break;
                    case SystemSearchLists.PackageDataPackages:
                        code = "SCHSYSPKGPKDT";
                        break;
                    case SystemSearchLists.PackageWorkflows:
                        code = "SCHSYSPKGWF";
                        break;
                    ////////////////////////////////////
                    case SystemSearchLists.OyezAliases: //OYEZALIAS
                        code = "SCHSYSOYEZALIAS";
                        break;
                    case SystemSearchLists.LaserAliases: //LASERALIAS
                        code = "SCHLASERALIAS";
                        break;
                    case SystemSearchLists.AddressPostcode:
                        code = "SCHCONADDPCAPCD";
                        break;
                    case SystemSearchLists.DocumentDuplicates: // DOCUMENTSDUP
                        code = "SCHDOCDUPID";
                        break;
                    case SystemSearchLists.DocumentDuplicatesExternal: // DOCUMENTSDUP
                        code = "SCHDOCDUPIDEXT";
                        break;
                    case SystemSearchLists.ContactInfoUsageCheck:
                        code = "SCHCONTINFOCHK";
                        break;
                    case SystemSearchLists.FileAssociateCopy: //SCHFILASSOCCOPY
                        code = "SCHFILASSOCCOPY";
                        break;
                    case SystemSearchLists.ReportServerPicker:
                        code = "SCHRSRPTPICKER";
                        break;
                    case SystemSearchLists.DocumentLog:
                        code = "SCHDOCLOG";
                        break;
                }
            }

            if (code == String.Empty)
                return DefaultSystemSearchList(list.ToString());
            else
                return code;
        }

        //NOTE: USE THIS ONE FOR NOW ON
        public string DefaultSystemSearchList(string list)
        {
            CheckLoggedIn();
            string code = String.Empty;

            code = _currentUser.GetSystemSearchLists(list);

            if (code == String.Empty)
                code = Convert.ToString(GetSessionConfigSetting("/config/defaultSearchLists/" + list, String.Empty));

            ApplicationSetting regmax = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "DefaultsVersion", "", 2);
            int version = Convert.ToInt32(regmax.GetSetting());

            if (code == String.Empty && version == 1)
            {
                switch (list.ToLower())
                {
                    case "none":
                        code = "NOTSET";
                        break;
                    case "associatefilter": //SYSASSOCFILTER
                        code = "SYSASSOCFILTER";
                        break;
                    case "associates": //SYSASSOCLIST
                        code = "SYSASSOCLIST";
                        break;
                    case "appointments": //APPOINTMENTS
                        code = "APPOINTMENTS";
                        break;
                    case "contact": //SYSCONTACTLIST
                        code = "SYSCONTACTLIST";
                        break;
                    case "file": //SYSFILELIST
                        code = "SYSFILELIST";
                        break;
                    case "timerecording": //SYSTIMEREC
                        code = "SYSTIMEREC";
                        break;
                    case "fieldcommon": //SYSFLDCOMMON
                        code = "SYSFLDCOMMON";
                        break;
                    case "fieldassociates": //SYSFLDASSOC
                        code = "SYSFLDASSOC";
                        break;
                    case "fieldappointments": //SYSFLDAPP
                        code = "SYSFLDAPP";
                        break;
                    case "fieldextendeddata": //SYSFLDEXTLIST
                        code = "SYSFLDEXTLIST";
                        break;
                    case "documenttimerecording": //SYSDOCTIMEREC
                        code = "SYSDOCTIMEREC";
                        break;
                    case "precedent": //SYSPRECLIST
                        code = "SYSPRECLIST";
                        break;
                    case "precedentfilter": //SYSPRECFILTER
                        code = "SYSPRECFILTER";
                        break;
                    case "remoteassociates": //ASSOCREMOTE
                        code = "ASSOCREMOTE";
                        break;
                    case "help": //SYSHELP
                        code = "SYSHELP";
                        break;
                    case "searchlistpicker": //SCHPICKER
                        code = "SCHPICKER";
                        break;
                    case "reportpicker": // RPTPICKER
                        code = "RPTPICKER";
                        break;
                    case "documentattachments": //DOCUMENTATT
                        code = "DOCUMENTATT";
                        break;
                    case "address": //ADDRESS
                        code = "ADDRESS";
                        break;
                    case "searchcontacts": //CONTACTS
                        code = "CONTACTS";
                        break;
                    ////////////////////// # PACKAGE AND DEPLOY LISTS #////////////////////////////
                    case "packagecodeLookups":
                        code = "PDCDLOOKUPS";
                        break;
                    case "packageenquiry":
                        code = "PDENQUIRY";
                        break;
                    case "packageextendeddata":
                        code = "PDEXTEND";
                        break;
                    case "packageobjects":
                        code = "PDOBJECTS";
                        break;
                    case "packageprecedents":
                        code = "PDPRECLIST";
                        break;
                    case "packagereports":
                        code = "PDREPORTS";
                        break;
                    case "packagescripts":
                        code = "PDSCRIPTS";
                        break;
                    case "packagesearchlists":
                        code = "PDSEARCHLISTS";
                        break;
                    case "packagesqlscripts":
                        code = "PDSQLSCRIPTS";
                        break;
                    /////////////////////////////////////////////////////////
                    case "oyezaliases": //OYEZALIAS
                        code = "OYEZALIAS";
                        break;
                    case "laseraliases": //LASERALIAS
                        code = "LASERALIAS";
                        break;
                    case "documentduplicates": // DOCUMENTSDUP
                        code = "DOCUMENTSDUP";
                        break;
                }
            }
            else if (code == String.Empty && version == 2)
            {
                switch (list.ToLower())
                {
                    case "none":
                        code = "NOTSET";
                        break;
                    case "associatefilter": //SYSASSOCFILTER
                        code = "SCHASSFILTER";
                        break;
                    case "associates": //SYSASSOCLIST
                        code = "SCHFILASSLIST2";
                        break;
                    case "associatesshort":
                        code = "SCHFILASSLISTST";
                        break;
                    case "appointments": //APPOINTMENTS
                        code = "SCHFILAPPOINTS";
                        break;
                    case "contact": //SYSCONTACTLIST
                        code = "SCHCLICONTACTS";
                        break;
                    case "file": //SYSFILELIST
                        code = "SCHFILTRANSLIST";
                        break;
                    case "timerecording": //SYSTIMEREC
                        code = "SCHFILTIMERECD";
                        break;
                    case "fieldcommon": //SYSFLDCOMMON
                        code = "SCHSYSCOMFIELDS";
                        break;
                    case "fieldassociates": //SYSFLDASSOC
                        code = "SCHASSFIELDS";
                        break;
                    case "fieldappointments": //SYSFLDAPP
                        code = "SCHFILAPPFIELDS";
                        break;
                    case "fieldextendeddata": //SYSFLDEXTLIST
                        code = "SCHSYSSCHFLDS";
                        break;
                    case "documenttimerecording": //SYSDOCTIMEREC
                        code = "SCHDOCTIME";
                        break;
                    case "precedent": //SYSPRECLIST
                        code = "SCHSYSPRECLST";
                        break;
                    case "precedentfilter": //SYSPRECFILTER
                        code = "SCHSYSPRECFLTR";
                        break;
                    case "remoteassociates": //ASSOCREMOTE
                        code = "SCHASSREMOTE";
                        break;
                    case "help": //SYSHELP
                        code = "SCHSYSHELP";
                        break;
                    case "searchlistpicker": //SCHPICKER
                        code = "SCHSYSSCHPICK";
                        break;
                    case "reportpicker": // RPTPICKER
                        code = "SCHSYSRPTPICK";
                        break;
                    case "documentattachments": //DOCUMENTATT
                        code = "SCHDOCATTLST";
                        break;
                    case "address": //ADDRESS
                        code = "SCHCONADDRESS";
                        break;
                    case "searchcontacts": //CONTACTS
                        code = "SCHCONSEARCH";
                        break;
                    case "workflowpicker":
                        code = "SCHWFPICKER";
                        break;
                    ////////////////////// # PACKAGE AND DEPLOY LISTS #////////////////////////////
                    case "packagecodelookups":
                        code = "SCHSYSPKGCODE";
                        break;
                    case "packageenquiry":
                        code = "SCHSYSPKGENQ";
                        break;
                    case "packageextendeddata":
                        code = "SCHSYSPKGEXTD";
                        break;
                    case "packageobjects":
                        code = "SCHSYSPKGOBJ";
                        break;
                    case "packageprecedents":
                        code = "SCHSYSPKGPREC";
                        break;
                    case "packagereports":
                        code = "SCHSYSPKGRPT";
                        break;
                    case "packagescripts":
                        code = "SCHSYSPKGSCRPT";
                        break;
                    case "packagesearchlists":
                        code = "SCHSYSPKGSCH";
                        break;
                    case "packagesqlscripts":
                        code = "SCHSYSPKGSQL";
                        break;
                    case "packagedatalists":
                        code = "SCHSYSPKGDATALS";
                        break;
                    case "packagedatapackages":
                        code = "SCHSYSPKGPKDT";
                        break;
                    case "packageworkflows":
                        code = "SCHSYSPKGWF";
                        break;
                    ////////////////////////////////////
                    case "oyezaliases": //OYEZALIAS
                        code = "SCHSYSOYEZALIAS";
                        break;
                    case "laseraliases": //LASERALIAS
                        code = "SCHLASERALIAS";
                        break;
                    case "addresspostcode":
                        code = "SCHCONADDPCAPCD";
                        break;
                    case "documentduplicates": // DOCUMENTSDUP
                        code = "SCHDOCDUPID";
                        break;
                    case "contactinfousagecheck":
                        code = "SCHCONTINFOCHK";
                        break;
                    case "fileassociatecopy": //SCHFILASSOCCOPY
                        code = "SCHFILASSOCCOPY";
                        break;
                }
            }

            return code;
        }

        #endregion

        #region DEFAULT SYSTEM FORMS

        /// <summary>
        /// Gets an enquiry form code to use for certain system forms.
        /// </summary>
        /// <param name="form">Type of form to fetch.</param>
        /// <returns>An enquiry form code.</returns>
        public string DefaultSystemForm(SystemForms form)
        {
            CheckLoggedIn();
            string code = String.Empty;

            code = _currentUser.GetSystemForm(form);

            if (code == String.Empty)
                code = Convert.ToString(GetSessionConfigSetting("/config/defaultForms/" + form.ToString(), String.Empty));

            ApplicationSetting regmax = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "DefaultsVersion", "", 2);
            int version = Convert.ToInt32(regmax.GetSetting());


            if (code == String.Empty && version == 1)
            {
                switch (form)
                {
                    case SystemForms.None:
                        code = "NOTSET";
                        break;
                    case SystemForms.AddressEdit:
                        code = "ENQADDRESSEDT";
                        break;
                    case SystemForms.AddressWizard:
                        code = "ENQADDRESS";
                        break;
                    case SystemForms.AssociateEdit:
                        code = "ENQASSOCIATE";
                        break;
                    case SystemForms.AssociateWizard:
                        code = "ENQNEWASSOCIATE";
                        break;
                    case SystemForms.ClientContactEdit:
                        code = "ENQCLIENTCONT";
                        break;
                    case SystemForms.ClientWizard:
                        code = "ENQCLIENTTAKEON";
                        break;
                    case SystemForms.ContactGroupWizard:
                        code = "ENQCONTGROUP";
                        break;
                    case SystemForms.ContactWizard:
                        code = "ENQCONTTAKEON";
                        break;
                    case SystemForms.FeeEarnerAdmin:
                        code = "ENQFEEEARNER";
                        break;
                    case SystemForms.FeeEarnerWizard:
                        code = "ENQFEEEARNER";
                        break;
                    case SystemForms.FileWizard:
                        code = "ENQFILETAKEON";
                        break;
                    case SystemForms.UserAdmin:
                        code = "ENQUSER";
                        break;
                    case SystemForms.UserSettings:
                        code = "ENQWELCOME";
                        break;
                    case SystemForms.UserWizard:
                        code = "ENQUSER";
                        break;
                    case SystemForms.PrecedentEdit:
                        code = "PRECINFO";
                        break;
                    case SystemForms.PrecedentWizard:
                        code = "ENQPRECEDENT";
                        break;
                    case SystemForms.FaxTransmission:
                        code = "ENQFAXTRANS";
                        break;
                    case SystemForms.SaveDocumentWizard:
                        code = "DOCSAVE";
                        break;
                    case SystemForms.TimeRecordEdit:
                        code = "ENQTIMEREC";
                        break;
                    case SystemForms.SMS:
                        code = "SMS";
                        break;
                    case SystemForms.SMSEdit:
                        code = "SMSRO";
                        break;
                    case SystemForms.DocReceipt:
                        code = "DOCRECEIPT";
                        break;
                    case SystemForms.PrecedentSearch:
                        code = "ENQPRECSEARCHNL";
                        break;
                    case SystemForms.SavePrecedentWizard:
                        code = "ENQPRECEDENT";
                        break;
                    case SystemForms.DateWizard:
                        code = "DWHEADER";
                        break;
                    case SystemForms.TaskWizard:
                        code = "TASK";
                        break;
                    case SystemForms.TaskEdit:
                        code = "TASK";
                        break;
                    case SystemForms.TaskItem:
                        code = "TASK";
                        break;
                    case SystemForms.AppointmentWizard:
                        code = "APPOINTMENT";
                        break;
                    case SystemForms.AppointmentEdit:
                        code = "APPOINTMENT";
                        break;
                    case SystemForms.AppointmentItem:
                        code = "APPOINTMENT";
                        break;
                    case SystemForms.Email:
                        code = "EMAIL";
                        break;
                    case SystemForms.TelephoneNumber:
                        code = "TELNUMBER";
                        break;
                    case SystemForms.FileReview:
                        code = "FILEREVIEW";
                        break;
                    case SystemForms.FaxTransmissionNoAssociate:
                        code = "ENQFAXTRANSNA";
                        break;
                    case SystemForms.FinanceLogEdit:
                        code = "FINANCELOG";
                        break;
                    case SystemForms.FinanceLogWizard:
                        code = "FINANCIALLOG";
                        break;
                    case SystemForms.RegisterOMSObject:
                        code = "OMSOBJECTS";
                        break;
                    case SystemForms.ManualTimeWizard:
                        code = "MANUALTIMEWIZ";
                        break;
                    case SystemForms.AddApplication:
                        code = "ADAPPLICATION";
                        break;
                    case SystemForms.SecurityCheck:
                        code = "SECCHECK";
                        break;
                    case SystemForms.FileFunding:
                        code = "FILEFUNDING";
                        break;
                    case SystemForms.RemoteAccountWizard:
                        code = "REMACCTWIZARD";
                        break;
                    case SystemForms.PreClientType:
                        code = "PRECLIENTTYPE";
                        break;
                    case SystemForms.ClientTypePicker:
                        code = "ENQCLIENTPICK";
                        break;
                    case SystemForms.OriginalClientType:
                        code = "ENQCLIENTTYPE";
                        break;
                    case SystemForms.ContactTypePicker:
                        code = "ENQCONTTYPE";
                        break;
                    case SystemForms.FileTypePicker:
                        code = "ENQFILETYPE";
                        break;
                    case SystemForms.MenuItem:
                        code = "ADITEM";
                        break;
                    case SystemForms.MenuFolder:
                        code = "ADFOLDER";
                        break;
                    case SystemForms.DocumentEdit:
                        code = "DOCINFO";
                        break;
                    case SystemForms.EmailEdit:
                        code = "EDITEMAIL";
                        break;
                    case SystemForms.TelephoneNumberEdit:
                        code = "EDITTELNUMBER";
                        break;
                    case SystemForms.LicenseUpload:
                        code = "LICUPLOAD";
                        break;
                    case SystemForms.LicenseDownload:
                        code = "LICDOWNLOAD";
                        break;
                    case SystemForms.Milestones:
                        code = "SCRFILMSOMS2K";
                        break;
                }
            }
            else if (code == String.Empty && version == 2)
            {
                switch (form)
                {
                    case SystemForms.None:
                        code = "NOTSET";
                        break;
                    case SystemForms.AddressEdit:
                        code = "SCRADDEDIT";
                        break;
                    case SystemForms.AddressWizard:
                        code = "SCRADDNEW";
                        break;
                    case SystemForms.AssociateEdit:
                        code = "SCRASSEDIT";
                        break;
                    case SystemForms.AssociateWizard:
                        code = "SCRASSNEW";
                        break;
                    case SystemForms.AssociateTypePicker:
                        code = "SCRASSPICKNEW";
                        break;
                    case SystemForms.ClientContactEdit:
                        code = "SCRCLICONTACT";
                        break;
                    case SystemForms.ClientWizard:
                        code = "SCRCLINEW";
                        break;
                    case SystemForms.ContactGroupWizard:
                        code = "SCRCONGROUP";
                        break;
                    case SystemForms.ContactWizard:
                        code = "SCRCONNEW";
                        break;
                    case SystemForms.FeeEarnerAdmin:
                        code = "SCRFEEMAIN";
                        break;
                    case SystemForms.FeeEarnerWizard:
                        code = "SCRFEEMAIN";
                        break;
                    case SystemForms.FileWizard:
                        code = "SCRFILNEW";
                        break;
                    case SystemForms.UserAdmin:
                        code = "SCRUSRMAIN_2";
                        break;
                    case SystemForms.UserSettings:
                        code = "SCRUSRWELCOME_2";
                        break;
                    case SystemForms.UserWizard:
                        code = "SCRUSRMAIN_2";
                        break;
                    case SystemForms.UserImportWizard:
                        code = "SCRUSRIMPORT";
                        break;
                    case SystemForms.PrecedentEdit:
                        code = "SCRPRENEW";
                        break;
                    case SystemForms.PrecedentWizard:
                        code = "SCRPRENEW";
                        break;
                    case SystemForms.FaxTransmission:
                        code = "SCRDOCFAXTRANS";
                        break;
                    case SystemForms.SaveDocumentWizard:
                        code = DefaultSaveWizard;
                        break;
                    case SystemForms.TimeRecordEdit:
                        code = "SCRTIMNEW_2";
                        break;
                    case SystemForms.SMS:
                        code = "SCRDOCSMSNEW";
                        break;
                    case SystemForms.SMSEdit:
                        code = "SCRDOCSMSEDIT";
                        break;
                    case SystemForms.DocReceipt:
                        code = "SCRDOCINNEW";
                        break;
                    case SystemForms.PrecedentSearch:
                        code = "FLTPRESEARCHNL2";
                        break;
                    case SystemForms.SavePrecedentWizard:
                        code = "SCRPRENEW";
                        break;
                    case SystemForms.DateWizard:
                        code = "SCRDWZNEW";
                        break;
                    case SystemForms.TaskWizard:
                        code = "SCRFILTASK";
                        break;
                    case SystemForms.TaskEdit:
                        code = "SIPFILTASK";
                        break;
                    case SystemForms.TaskItem:
                        code = "SIPFILTASK";
                        break;
                    case SystemForms.AppointmentWizard:
                        code = "SCRFILAPPT";
                        break;
                    case SystemForms.AppointmentEdit:
                        code = "SIPFILAPPT";
                        break;
                    case SystemForms.AppointmentItem:
                        code = "SIPFILAPPT";
                        break;
                    case SystemForms.Email:
                        code = "SCRCONEMAILNEW";
                        break;
                    case SystemForms.TelephoneNumber:
                        code = "SCRCONTELNEW";
                        break;
                    case SystemForms.FileReview:
                        code = "SCRFILREVIEW";
                        break;
                    case SystemForms.FaxTransmissionNoAssociate:
                        code = "SCRDOCFAXTRANNA";
                        break;
                    case SystemForms.FinanceLogEdit:
                        code = "SCRFILFINANCE";
                        break;
                    case SystemForms.FinanceLogWizard:
                        code = "SCRFILFINANCE";
                        break;
                    case SystemForms.RegisterOMSObject:
                        code = "SCRSYSOBJECTNEW";
                        break;
                    case SystemForms.ManualTimeWizard:
                        code = "SCRTIMMANUALNEW";
                        break;
                    case SystemForms.AddApplication:
                        code = "SCRSYSAPPNEW";
                        break;
                    case SystemForms.SecurityCheck:
                        code = "SCRCONSECCHECK";
                        break;
                    case SystemForms.FileFunding:
                        code = "SCRFILFUNDING";
                        break;
                    case SystemForms.RemoteAccountWizard:
                        code = "SCRCONREMACCNEW";
                        break;
                    case SystemForms.PreClientType:
                        code = "SCRCLIPRENEW";
                        break;
                    case SystemForms.ClientTypePicker:
                        code = "SCRCLIPICKNEW";
                        break;
                    case SystemForms.OriginalClientType:
                        code = "SCRCLIPICKORIG";
                        break;
                    case SystemForms.ContactTypePicker:
                        code = "SCRCONPICKNEW";
                        break;
                    case SystemForms.FileTypePicker:
                        code = "SCRFILPICKNEW";
                        break;
                    case SystemForms.MenuItem:
                        code = "SCRSYSMENUITM";
                        break;
                    case SystemForms.ConsoleItem:
                        code = "sCONSOLEMGR";
                        break;
                    case SystemForms.MenuFolder:
                        code = "SCRSYSMENUFLD";
                        break;
                    case SystemForms.DocumentEdit:
                        code = "SCRDOCEDIT2";
                        break;
                    case SystemForms.EmailEdit:
                        code = "SCRCONEMAILEDIT";
                        break;
                    case SystemForms.TelephoneNumberEdit:
                        code = "SCRCONTELEDIT";
                        break;
                    case SystemForms.LicenseUpload:
                        code = "SCRLICUPLOAD2";
                        break;
                    case SystemForms.LicenseDownload:
                        code = "SCRLICDOWNLOAD2";
                        break;
                    case SystemForms.Milestones:
                        code = "SCRFILMSOMS2K";
                        break;
                    case SystemForms.BranchEdit:
                        code = "SCRSYSBRANCH";
                        break;
                    case SystemForms.SystemEdit:
                        code = "SCRSYSSETTINGS";
                        break;
                    case SystemForms.UFNInformation:
                        code = "SCRFILUFNTAKEON";
                        break;
                    case SystemForms.FilePhaseEdit:
                        code = "SCRFILPHASE";
                        break;
                    case SystemForms.FilePhaseWizard:
                        code = "SCRFILPHASE";
                        break;
                    case SystemForms.AddDistributedAssembly:
                        code = "SCRADDDISASS";
                        break;
                    case SystemForms.AddDistributedModule:
                        code = "SCRADDDISMOD";
                        break;
                    default:
                        goto case SystemForms.None;
                }
            }

            if (code == String.Empty)
                return DefaultSystemForm(form.ToString());
            else
                return code;
        }


        //NOTE: USE THIS ONE FOR NOW ON
        public string DefaultSystemForm(string form)
        {
            CheckLoggedIn();
            string code = String.Empty;

            code = _currentUser.GetSystemForm(form);

            if (code == String.Empty)
                code = Convert.ToString(GetSessionConfigSetting("/config/defaultForms/" + form, String.Empty));

            ApplicationSetting regmax = new ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "DefaultsVersion", "", 2);
            int version = Convert.ToInt32(regmax.GetSetting());

            if (code == String.Empty && version == 1)
            {
                switch (form.ToLower())
                {
                    case "none":
                        code = "NOTSET";
                        break;
                    case "addressedit":
                        code = "ENQADDRESSEDT";
                        break;
                    case "addresswizard":
                        code = "ENQADDRESS";
                        break;
                    case "associateedit":
                        code = "ENQASSOCIATE";
                        break;
                    case "associatewizard":
                        code = "ENQNEWASSOCIATE";
                        break;
                    case "clientcontactedit":
                        code = "ENQCLIENTCONT";
                        break;
                    case "clientwizard":
                        code = "ENQCLIENTTAKEON";
                        break;
                    case "contactgroupwizard":
                        code = "ENQCONTGROUP";
                        break;
                    case "contactwizard":
                        code = "ENQCONTTAKEON";
                        break;
                    case "feeearneradmin":
                        code = "ENQFEEEARNER";
                        break;
                    case "feeearnerwizard":
                        code = "ENQFEEEARNER";
                        break;
                    case "filewizard":
                        code = "ENQFILETAKEON";
                        break;
                    case "useradmin":
                        code = "ENQUSER";
                        break;
                    case "usersettings":
                        code = "ENQWELCOME";
                        break;
                    case "userwizard":
                        code = "ENQUSER";
                        break;
                    case "precedentedit":
                        code = "PRECINFO";
                        break;
                    case "precedentwizard":
                        code = "ENQPRECEDENT";
                        break;
                    case "faxtransmission":
                        code = "ENQFAXTRANS";
                        break;
                    case "savedocumentwizard":
                        code = "DOCSAVE";
                        break;
                    case "timerecordedit":
                        code = "ENQTIMEREC";
                        break;
                    case "sms":
                        code = "SMS";
                        break;
                    case "smsedit":
                        code = "SMSRO";
                        break;
                    case "docreceipt":
                        code = "DOCRECEIPT";
                        break;
                    case "precedentsearch":
                        code = "ENQPRECSEARCHNL";
                        break;
                    case "saveprecedentwizard":
                        code = "ENQPRECEDENT";
                        break;
                    case "datewizard":
                        code = "DWHEADER";
                        break;
                    case "taskwizard":
                        code = "TASK";
                        break;
                    case "taskedit":
                        code = "TASK";
                        break;
                    case "taskitem":
                        code = "TASK";
                        break;
                    case "appointmentwizard":
                        code = "APPOINTMENT";
                        break;
                    case "appointmentedit":
                        code = "APPOINTMENT";
                        break;
                    case "appointmentitem":
                        code = "APPOINTMENT";
                        break;
                    case "email":
                        code = "EMAIL";
                        break;
                    case "telephonenumber":
                        code = "TELNUMBER";
                        break;
                    case "filereview":
                        code = "FILEREVIEW";
                        break;
                    case "faxtransmissionnoassociate":
                        code = "ENQFAXTRANSNA";
                        break;
                    case "financelogedit":
                        code = "FINANCELOG";
                        break;
                    case "financelogwizard":
                        code = "FINANCIALLOG";
                        break;
                    case "registeromsobject":
                        code = "OMSOBJECTS";
                        break;
                    case "manualtimewizard":
                        code = "MANUALTIMEWIZ";
                        break;
                    case "addapplication":
                        code = "ADAPPLICATION";
                        break;
                    case "securitycheck":
                        code = "SECCHECK";
                        break;
                    case "filefunding":
                        code = "FILEFUNDING";
                        break;
                    case "remoteaccountwizard":
                        code = "REMACCTWIZARD";
                        break;
                    case "preclienttype":
                        code = "PRECLIENTTYPE";
                        break;
                    case "clienttypepicker":
                        code = "ENQCLIENTPICK";
                        break;
                    case "originalclienttype":
                        code = "ENQCLIENTTYPE";
                        break;
                    case "contacttypepicker":
                        code = "ENQCONTTYPE";
                        break;
                    case "filetypepicker":
                        code = "ENQFILETYPE";
                        break;
                    case "menuitem":
                        code = "ADITEM";
                        break;
                    case "menufolder":
                        code = "ADFOLDER";
                        break;
                    case "documentedit":
                        code = "DOCINFO";
                        break;
                    case "emailedit":
                        code = "EDITEMAIL";
                        break;
                    case "telephonenumberedit":
                        code = "EDITTELNUMBER";
                        break;
                    case "licenseupload":
                        code = "LICUPLOAD";
                        break;
                    case "licensedownload":
                        code = "LICDOWNLOAD";
                        break;
                    case "milestones":
                        code = "SCRFILMSOMS2K";
                        break;
                }
            }
            else if (code == String.Empty && version == 2)
            {
                switch (form.ToLower())
                {
                    case "none":
                        code = "NOTSET";
                        break;
                    case "addressedit":
                        code = "SCRADDEDIT";
                        break;
                    case "addresswizard":
                        code = "SCRADDNEW";
                        break;
                    case "associateedit":
                        code = "SCRASSEDIT";
                        break;
                    case "associatewizard":
                        code = "SCRASSNEW";
                        break;
                    case "clientcontactedit":
                        code = "SCRCLICONTACT";
                        break;
                    case "clientwizard":
                        code = "SCRCLINEW";
                        break;
                    case "contactgroupwizard":
                        code = "SCRCONGROUP";
                        break;
                    case "contactwizard":
                        code = "SCRCONNEW";
                        break;
                    case "feeearneradmin":
                        code = "SCRFEEMAIN";
                        break;
                    case "feeearnerwizard":
                        code = "SCRFEEMAIN";
                        break;
                    case "filewizard":
                        code = "SCRFILNEW";
                        break;
                    case "useradmin":
                        code = "SCRUSRMAIN_2";
                        break;
                    case "usersettings":
                        code = "SCRUSRWELCOME_2";
                        break;
                    case "userwizard":
                        code = "SCRUSRMAIN_2";
                        break;
                    case "userimportwizard":
                        code = "SCRUSRIMPORT";
                        break;
                    case "precedentedit":
                        code = "SCRPRENEW";
                        break;
                    case "precedentwizard":
                        code = "SCRPRENEW";
                        break;
                    case "faxtransmission":
                        code = "SCRDOCFAXTRANS";
                        break;
                    case "savedocumentwizard":
                        code = DefaultSaveWizard;
                        break;
                    case "timerecordedit":
                        code = "SCRTIMNEW_2";
                        break;
                    case "sms":
                        code = "SCRDOCSMSNEW";
                        break;
                    case "smsedit":
                        code = "SCRDOCSMSEDIT";
                        break;
                    case "docreceipt":
                        code = "SCRDOCINNEW";
                        break;
                    case "precedentsearch":
                        code = "FLTPRESEARCHNL";
                        break;
                    case "saveprecedentwizard":
                        code = "SCRPRENEW";
                        break;
                    case "datewizard":
                        code = "SCRDWZNEW";
                        break;
                    case "taskwizard":
                        code = "SCRFILTASK";
                        break;
                    case "taskedit":
                        code = "SIPFILTASK";
                        break;
                    case "taskitem":
                        code = "SIPFILTASK";
                        break;
                    case "appointmentwizard":
                        code = "SCRFILAPPT";
                        break;
                    case "appointmentedit":
                        code = "SIPFILAPPT";
                        break;
                    case "appointmentitem":
                        code = "SIPFILAPPT";
                        break;
                    case "email":
                        code = "SCRCONEMAILNEW";
                        break;
                    case "telephonenumber":
                        code = "SCRCONTELNEW";
                        break;
                    case "filereview":
                        code = "SCRFILREVIEW";
                        break;
                    case "faxtransmissionnoassociate":
                        code = "SCRDOCFAXTRANNA";
                        break;
                    case "financelogedit":
                        code = "SCRFILFINANCE";
                        break;
                    case "financelogwizard":
                        code = "SCRFILFINANCE";
                        break;
                    case "registeromsobject":
                        code = "SCRSYSOBJECTNEW";
                        break;
                    case "manualtimewizard":
                        code = "SCRTIMMANUALNEW";
                        break;
                    case "addapplication":
                        code = "SCRSYSAPPNEW";
                        break;
                    case "securitycheck":
                        code = "SCRCONSECCHECK";
                        break;
                    case "filefunding":
                        code = "SCRFILFUNDING";
                        break;
                    case "remoteaccountwizard":
                        code = "SCRCONREMACCNEW";
                        break;
                    case "preclienttype":
                        code = "SCRCLIPRENEW";
                        break;
                    case "clienttypepicker":
                        code = "SCRCLIPICKNEW";
                        break;
                    case "originalclienttype":
                        code = "SCRCLIPICKORIG";
                        break;
                    case "contacttypepicker":
                        code = "SCRCONPICKNEW";
                        break;
                    case "filetypepicker":
                        code = "SCRFILPICKNEW";
                        break;
                    case "menuitem":
                        code = "SCRSYSMENUITM";
                        break;
                    case "menufolder":
                        code = "SCRSYSMENUFLD";
                        break;
                    case "documentedit":
                        code = "SCRDOCEDIT2";
                        break;
                    case "emailedit":
                        code = "SCRCONEMAILEDIT";
                        break;
                    case "telephonenumberedit":
                        code = "SCRCONTELEDIT";
                        break;
                    case "licenseupload":
                        code = "SCRLICUPLOAD2";
                        break;
                    case "licensedownload":
                        code = "SCRLICDOWNLOAD2";
                        break;
                    case "milestones":
                        code = "SCRFILMSOMS2K";
                        break;
                    case "branchedit":
                        code = "SCRSYSBRANCH";
                        break;
                    case "systemedit":
                        code = "SCRSYSSETTINGS";
                        break;
                    case "ufninformation":
                        code = "SCRFILUFNTAKEON";
                        break;
                    case "filephaseedit":
                        code = "SCRFILPHASE";
                        break;
                    case "filephasewizard":
                        code = "SCRFILPHASE";
                        break;
                    case "adddistributedassembly":
                        code = "SCRADDDISASS";
                        break;
                    case "adddistributedmodule":
                        code = "SCRADDDISMOD";
                        break;
                    default:
                        goto case "none";
                }
            }


            return code;
        }


        #endregion

        /// <summary>
        /// Gets a specific data item from a specified cope.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public object GetSpecificData(string code)
        {
            IDataParameter[] paramlist = new IDataParameter[2];
            paramlist[0] = Connection.AddParameter("Code", SqlDbType.NVarChar, 30, code);
            paramlist[1] = Connection.AddParameter("Branch", SqlDbType.Int, 0, CurrentBranch.ID);
            object ret = null;

            ret = Connection.ExecuteProcedureScalar("sprGetSpecificData", paramlist);

            if (ret == null)
                return "";
            else
                return ret;
        }

        /// <summary>
        /// Gets the command bar data set file that the client will render in its own
        /// way. This method checks the the version of the cached XML interpretation of 
        /// command bar schema with the relational version in the database.
        /// </summary>
        /// <param name="commandBar">The unique command bar code.</param>
        /// <returns>A data set object that holds the relefant information.</returns>
        public DataSet GetCommandBar(string commandBar)
        {
            return GetCommandBar(commandBar, true);
        }

        public DataTable GetAllCommandBarControls()
        {
            CheckLoggedIn();

            System.Collections.Generic.List<IDataParameter> pars = new System.Collections.Generic.List<IDataParameter>();
            pars.Add(Connection.CreateParameter("UI", Thread.CurrentThread.CurrentUICulture.Name));
            return Connection.ExecuteSQLTable(@"select *, 
dbo.GetCodeLookupDesc('CBCCAPTIONS', ctrlcode, @UI) as [ctrldesc],
dbo.GetCodeLookupHelp('CBCCAPTIONS', ctrlcode, @UI) as [ctrlhelp]
from dbcommandbarcontrol", "CONTROLS", pars.ToArray());
        }

        public DataSet GetCommandBar(string commandBar, bool filtered)
        {

            DataSet ds = null;		//Internal schema used.
            DataSet fds = null;		//Cached schema version.
            long version = 0;		//Version specifier.
            bool local = false;

            //Loads the cached version of the command bar schema and gets the version
            //from the header information.  If there is an error opening the file
            //then set the version is set to zero.  The enquiry form will then be 
            //completely refreshed from the database.
            try
            {
                fds = Global.GetCache(@"commandbars\" + Thread.CurrentThread.CurrentUICulture.Name, commandBar + "." + Global.CacheExt);
                if (fds != null)
                    version = (long)fds.Tables["COMMANDBAR"].Rows[0]["cbversion"];
                else
                    version = 0;
            }
            catch
            {
                fds = null;
                version = 0;
            }



            //Run the sprCommandBarBuilder stored procedure and pass it the found version 
            //number.  If there is a newer version then cache the newly generated schema.
            IDataParameter[] paramlist = new IDataParameter[4];
            paramlist[0] = Connection.AddParameter("Code", SqlDbType.NVarChar, 15, commandBar);
            paramlist[1] = Connection.AddParameter("Version", SqlDbType.BigInt, 0, (_designMode ? 0 : version));
            paramlist[2] = Connection.AddParameter("Force", SqlDbType.Bit, 0, 0);
            paramlist[3] = Connection.AddParameter("UI", SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name);

            //Allow an execution error to escalate through the stack.
            ds = Connection.ExecuteProcedureDataSet("sprCommandBarBuilder", new string[1] { "COMMANDBAR" }, paramlist);


            //Make sure that there is a valid command bar returned.  
            //If not then use the already cached version of the command bar.
            if ((ds == null) || (ds.Tables.Count == 0))
            {
                if ((fds == null) || (fds.Tables["COMMANDBAR"] == null))
                {
                    //The returned data set schema is invalid and there is not cached version
                    //to rely on.
                    throw new OMSException(HelpIndexes.CommandBarDoesNotExist, commandBar);
                }
                else
                {
                    //The locally cached version is being used.
                    Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Using local version of command bar '" + commandBar + "'", "BAL.Session.GetCommandBar()");
                    ds = fds;
                    local = true;
                }
            }
            else
            {
                Trace.WriteLineIf(Global.LogSwitch.TraceInfo, "Using database version of command bar '" + commandBar + "'", "BAL.Session.GetCommandBar()");

                //Name the data set for neatnes reasons.
                ds.DataSetName = commandBar;

                //Name the required schema tables.
                ds.Tables[1].TableName = "CONTROLS";

            }


            //Cache the enquiry item to the users application data folder.
            //(e.g., "%APPDATA%\fwbs\oms\enquiries\user.xml").
            if (!local)
            {
                Global.Cache(ds, @"commandbars\" + Thread.CurrentThread.CurrentUICulture.Name, commandBar + "." + Global.CacheExt);
                Global.Cache(ds, @"commandbars", commandBar + "." + Global.CacheExt);
            }

            //Loop through each control and terminology parse the text and check wether it can be used by
            //the roles, packages, licensing supplied.
            User usr = CurrentUser;
            for (int ctr = ds.Tables[1].Rows.Count - 1; ctr >= 0; ctr--)
            {
                DataRow ctrl = ds.Tables[1].Rows[ctr];

                if (filtered)
                {

                    if (usr.IsInRoles(Convert.ToString(ctrl["ctrlrole"])) == false)
                    {
                        ctrl.Delete();
                        continue;
                    }

                    if (ValidateConditional(null, Convert.ToString(ctrl["ctrlcondition"]).Split(Environment.NewLine.ToCharArray())) == false)
                    {
                        ctrl.Delete();
                        continue;
                    }
                }

                ctrl["ctrldesc"] = _terminolgy.Parse(ctrl["ctrldesc"].ToString(), true);
                ctrl["ctrlhelp"] = _terminolgy.Parse(ctrl["ctrlhelp"].ToString(), true);
                ctrl["ctrlparentdesc"] = _terminolgy.Parse(ctrl["ctrlparentdesc"].ToString(), true);
            }

            ds.AcceptChanges();

            return ds;
        }



        /// <summary>
        /// Returns a Data Table based on the countries in the specific user interface culture.
        /// </summary>
        /// <returns>Data Table full of the different countries.</returns>
        public DataTable GetCountries()
        {
            CheckLoggedIn();
            if (_countries != null)
            {
                return _countries.Copy();
            }
            else
            {
                _countries = Connection.ExecuteProcedureTable("sprCountries", "DSCOUNTRIES", new IDataParameter[1] { Connection.AddParameter("UI", SqlDbType.NVarChar, 10, Thread.CurrentThread.CurrentCulture.Name) });
                return _countries.Copy();
            }
        }

        /// <summary>
        /// Gets the country id based on the country name passed (whatever the stored language).
        /// </summary>
        /// <param name="name">country name</param>
        /// <returns>country ID value</returns>
        public int GetCountryIDByName(string name)
        {
            string sql = "select ctryid from dbcodelookup inner join dbcountry on ctrycode = cdcode where cdtype = 'COUNTRIES' and cddesc = @country group by ctryid";
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Connection.AddParameter("country", name);
            DataTable dt = Connection.ExecuteSQLTable(sql, "COUNTRIES", pars);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["ctryid"]);
            else
                return DefaultCountry;
        }

        /// <summary>
        /// Gets the country id based on the country code passed (whatever the stored language).
        /// </summary>
        /// <param name="code">country code</param>
        /// <returns>country ID value</returns>
        public int GetCountryIDByCode(string code)
        {
            string sql = "select ctryID from dbcountry where ctryCode = @CCODE";
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Connection.AddParameter("CCODE", code);
            DataTable dt = Connection.ExecuteSQLTable(sql, "COUNTRIES", pars);
            if (dt.Rows.Count > 0)
                return Convert.ToInt32(dt.Rows[0]["ctryid"]);
            else
                return DefaultCountry;
        }



        /// <summary>
        /// GetSessionConfig Setting, takes an XPath or Field Name and will try and return the 
        /// system setting for this request, It will first look through GetExtraInfo and then
        /// through XML in the DbRegInfo.RegXML.
        /// </summary>
        /// <param name="settingPath">Property wishing to be returned</param>
        /// <param name="defaultValue">Default Value to be returned if not found</param>
        /// <returns></returns>
        public string GetSessionConfigSetting(string settingPath, string defaultValue)
        {
            CheckLoggedIn();
            if (_regInfo.Tables[Table].Columns.Contains(settingPath))
            {
                // Check if settingpath is a field and return the path
                return Convert.ToString(GetExtraInfo(settingPath));
            }

            if (_sessionconfig == null)
            {
                // _sessionconfig not initialised then init.
                _sessionconfig = new ConfigSetting(Convert.ToString(GetExtraInfo("regXML")));
            }

            if (_sessionconfig.DocObject.DocumentElement.SelectNodes(settingPath).Count > 0)
            {
                return _sessionconfig.DocObject.DocumentElement.SelectSingleNode(settingPath).InnerText;
            }
            else
            {
                return defaultValue;
            }

        }


        /// <summary>
        /// Gets a directory location of a specified directory id.
        /// </summary>
        /// <param name="id">Directory identifier.</param>
        /// <returns>A directory object.</returns>
        public DirectoryInfo GetDirectory(short id)
        {
            CheckLoggedIn();
            DataView vw = new DataView(_regInfo.Tables[Table_Directory]);
            vw.RowFilter = "dirid = '" + id.ToString() + "'";
            if (vw.Count > 0)
            {
                string dir = Convert.ToString(vw[0]["dirpath"]);
                return new DirectoryInfo(dir);
            }
            else
                return null;
        }

        /// <summary>
        /// Gets a directory location of a specified directory type.
        /// </summary>
        /// <param name="type">Directory type.</param>
        /// <returns>A directory object.</returns>
        public DirectoryInfo GetDirectory(string type)
        {
            short id;
            return GetDirectory(type, out id);
        }

        public DataTable GetDirectories()
        {
            return _regInfo.Tables[Table_Directory].Copy();
        }

        /// <summary>
        /// Gets a directory location of a specified directory type.
        /// </summary>
        /// <param name="type">Directory type.</param>
        /// <param name="directoryID">The directory identifier that was used.</param>
        /// <returns>A directory object.</returns>
        public DirectoryInfo GetDirectory(string type, out short directoryID)
        {
            CheckLoggedIn();
            DataView vw = new DataView(_regInfo.Tables[Table_Directory]);
            vw.RowFilter = String.Format("dircode = '{0}' and (brid is null or brid = '{1}')", SQLRoutines.RemoveRubbish(type), CurrentBranch.ID);
            vw.Sort = "brid asc";
            string dir = String.Empty;
            if (vw.Count == 2)
            {
                directoryID = Convert.ToInt16(vw[1]["dirid"]);
                dir = Convert.ToString(vw[1]["dirpath"]);
            }
            else if (vw.Count == 1)
            {
                directoryID = Convert.ToInt16(vw[0]["dirid"]);
                dir = Convert.ToString(vw[0]["dirpath"]);
            }
            else
                throw new OMSException(HelpIndexes.DirectoryNotSetup, type);

            return new DirectoryInfo(dir);
        }

        /// <summary>
        /// Gets a directory location of a specified system directory.
        /// </summary>
        /// <param name="directory">Directory code.</param>
        /// <param name="directoryID">The directory identifier that was used.</param>
        /// <returns>A directory object.</returns>
        public DirectoryInfo GetSystemDirectory(SystemDirectories directory, out short directoryID)
        {
            return GetDirectory(directory.ToString(), out directoryID);
        }

        /// <summary>
        /// Gets a directory location of a specified system directory.
        /// </summary>
        /// <param name="directory">Directory code.</param>
        /// <returns>A directory object.</returns>
        public DirectoryInfo GetSystemDirectory(SystemDirectories directory)
        {
            short id;
            return GetDirectory(directory.ToString(), out id);
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Logs into the database using the multi database selector identifier.
        /// </summary>
        public static Session Login(int databaseSelection, string userName, string password)
        {
            CurrentSession.LogOn(databaseSelection, userName, password);
            return CurrentSession;
        }


        /// <summary>
        /// Logs into the database using the multi database selector identifier.
        /// </summary>
        public static Session Login(int databaseSelection, string userName, string password, bool clearCache)
        {
            CurrentSession.LogOn(databaseSelection, userName, password, clearCache);
            return CurrentSession;
        }

        /// <summary>
        /// Logs into the database using the multi database selector identifier.
        /// </summary>
        public static Session Login(DatabaseSettings db, string userName, string password, bool clearCache)
        {
            CurrentSession.LogOn(db, userName, password, clearCache);
            return CurrentSession;
        }

        /// <summary>
        /// Logs into the database using the default zero based multi database selector identifier.
        /// </summary>
        public static Session Login(string userName, string password)
        {
            return Login(0, userName, password);
        }

        /// <summary>
        /// Gets the command bar data set file from the local machine that the client 
        /// will render in its own way.
        /// </summary>
        /// <param name="commandBar">The unique command bar code.</param>
        /// <returns>A data set object that holds the relefant information.</returns>
        static public DataSet GetLocalCommandBar(string commandBar)
        {

            DataSet fds = null;		//Cached schema version.


            //Loads the cached version of the command.
            try
            {
                fds = Global.GetCache(@"commandbars", commandBar + "." + Global.CacheExt);
            }
            catch
            {
                fds = null;
            }


            if ((fds == null) || (fds.Tables.Count == 0))
                throw new Exception(String.Format("Command bar '{0}' does not exist.", commandBar));
            else
            {
                //Name the data set for neatnes reasons.
                fds.DataSetName = commandBar;

                //Name the required schema tables.
                fds.Tables[1].TableName = "CONTROLS";

            }

            return fds;
        }


        #endregion

        #region Licensing


        [EnquiryUsage(false)]
        [Browsable(false)]
        public Extensibility.AddinManager Addins
        {
            get
            {
                if (_addinman == null)
                {
                    _addinman = new Extensibility.AddinManager();
                }
                return _addinman;
            }
        }

        /// <summary>
        /// Gets whether the current session object is logged into the system or not.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public bool IsLoggedIn
        {
            get
            {
                if ((_regInfo == null) || (!_loggedIn) || (_currentUser == null) || (_currentTerminal == null))
                    return false;
                else
                    return true;
            }
        }

        internal FWBS.OMS.Script.ScriptGen Script
        {
            get
            {
                return _sessionScript;
            }
        }

        /// <summary>
        /// Sets the assembly api consumer if the consumer is not an EXE starting in an app domain.
        /// An example of this is if the API is being used from a COM addin or a web ASP application.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Assembly APIConsumer
        {
            set
            {
                if (_apiConsumer == null)
                    _apiConsumer = value;
            }
        }

        /// <summary>
        /// A quick boolean check to see if the current session is licensed for a particular
        /// license.  This will combine the terminal and user check.
        /// </summary>
        /// <param name="expression">Code string interprestation of the license.</param>
        /// <returns>True if the session is licensed to use the specified module.</returns>
        public bool IsLicensedFor(string expression)
        {
            return true;
        }

        /// <summary>
        /// Checks wheter the current session is licensed for the particular module.
        /// If not then an exception is raised.
        /// </summary>
        /// <param name="expression">Code string interpretation of the License module to check for.</param>
        public void ValidateLicensedFor(string expression)
        {
        }

        #endregion

        #region Composition

        private Fwbs.Framework.Session internalsession;
        private Fwbs.Framework.ComponentModel.Composition.AggregateCatalog dynamiccatalog;
        private Fwbs.Framework.Licensing.ILicensingManager apilicman;

        private void ValidateContainer()
        {
            if (internalsession == null)
                throw new InvalidOperationException("The session container has not been configured.");
        }

        private Fwbs.Framework.Session InternalSession
        {
            get
            {
                ValidateContainer();

                return internalsession;
            }
        }

        public IContainer Container
        {
            get
            {
                return InternalSession.GetService<Fwbs.Framework.ComponentModel.Composition.IContainer>();
            }
        }

        public Fwbs.Framework.Reflection.ITypeManager TypeManager
        {
            get
            {
                return InternalSession.GetService<Fwbs.Framework.Reflection.ITypeManager>();
            }
        }

        public Fwbs.Framework.Licensing.ILicensingManager APILicenseManager
        {
            get
            {
                ValidateContainer();

                return apilicman;
            }
        }

        public Fwbs.Framework.Reflection.IAssemblyManager AssemblyManager
        {
            get
            {
                return new AssemblyManager(InternalSession.GetService<Fwbs.Framework.Reflection.IAssemblyManager>());
            }
        }

        internal Fwbs.Framework.ComponentModel.Composition.AggregateCatalog DynamicCatalog
        {
            get
            {
                ValidateContainer();

                return dynamiccatalog;
            }
        }

        private void SetupContainer()
        {
            PrintStackTrace("SetupContainer", "Session");

            if (internalsession != null)
                return;

            apilicman = new FWBS.OMS.Licensing.APILicensingManager();

            internalsession = new Fwbs.Framework.Session(Fwbs.Framework.SessionType.Client, apilicman);

            dynamiccatalog = new Fwbs.Framework.ComponentModel.Composition.AggregateCatalog();

            internalsession.Initialize += new EventHandler<Fwbs.Framework.SessionInitializeEventArgs>(sessioncontainer_Initialize);

            Container.Export<Fwbs.Framework.Licensing.ILicensingManager>(apilicman);

            AssemblyManager.Alias("EnquiryControls.WinUI", "FWBS.Common.UI");

            DocumentInfo.SetSession(internalsession);

        }

        private void DisposeContainer()
        {
            PrintStackTrace("DisposeContainer", "Session");

            var disp = internalsession as IDisposable;

            internalsession = null;
            DocumentInfo.SetSession(null);
            dynamiccatalog = null;

            if (disp == null)
                return;

            disp.Dispose();

            disp = apilicman as IDisposable;
            if (disp != null)
                disp.Dispose();
            apilicman = null;
        }


        private string GetInstallLocation()
        {
            var installLocationKey = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, string.Empty, "InstallPath");
            var location = installLocationKey.GetSetting(string.Empty) as string;

            return location;
        }

        private void sessioncontainer_Initialize(object sender, Fwbs.Framework.SessionInitializeEventArgs e)
        {
            PrintStackTrace("sessioncontainer_Initialize", "Session");

            string baseApplicationLocation = GetBaseApplicationLocation();

            AddBaseApplicationDirectoryFrameworkCatalog(e, baseApplicationLocation);

            e.Catalogs.Add(new FileInfo(Path.Combine(baseApplicationLocation, "FWBS.Common.dll")), e.Session);
            e.Catalogs.Add(new FileInfo(Path.Combine(baseApplicationLocation, "FWBS.Common.UI.dll")), e.Session);
            e.Catalogs.Add(new FileInfo(Path.Combine(baseApplicationLocation, "OMS.Data.dll")), e.Session);
            e.Catalogs.Add(new FileInfo(Path.Combine(baseApplicationLocation, "OMS.Library.dll")), e.Session);
            e.Catalogs.Add(new FileInfo(Path.Combine(baseApplicationLocation, "OMS.Reports.dll")), e.Session);

            var finfo = new FileInfo(Path.Combine(baseApplicationLocation, "FWBS.DocumentPreviewer.dll"));
            Debug.WriteLine(finfo.FullName);
            e.Catalogs.Add(finfo, e.Session);

            e.Catalogs.Add(new DirectoryCatalog(Path.Combine(baseApplicationLocation, "Modules"), e.Session));

            var iManageWork10DependenciesPath = Path.Combine(baseApplicationLocation, "iManageWork10.Shell");
            if (System.IO.Directory.Exists(iManageWork10DependenciesPath))
            {
                AppDomain.CurrentDomain.AssemblyResolve += ResolveAssembly;
                e.Catalogs.Add(new DirectoryCatalog(iManageWork10DependenciesPath, e.Session));
            }
            
            e.Catalogs.Add(dynamiccatalog);

            DocumentInfo.AddCatalogs(e, baseApplicationLocation);
        }

        private Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            string assemblyName = args.Name;
            if (assemblyName.StartsWith("Microsoft.Web.WebView2") && !assemblyName.Contains(".resources"))
            {
                string filename = $"{assemblyName.Split(',')[0]}.dll";
                try
                {
                    return Assembly.LoadFrom(Path.Combine(GetBaseApplicationLocation(), filename));
                }
                catch
                { }
            }

            return null;
        }

        public string GetBaseApplicationLocation()
        {
            string baseApplicationLocation = AppDomain.CurrentDomain.BaseDirectory;

            if (!string.IsNullOrWhiteSpace(_installLocation))
                baseApplicationLocation = _installLocation;

            baseApplicationLocation = baseApplicationLocation.TrimEnd('\\');
            return baseApplicationLocation;
        }

        private const string FrameworkAssembliesSearchPattern = "Fwbs.Framework*.dll";

        private static void AddBaseApplicationDirectoryFrameworkCatalog(Fwbs.Framework.SessionInitializeEventArgs args, string baseApplicationLocation)
        {
            if (!CatalogsContainBaseDirectoryFrameworkCatalog(args, baseApplicationLocation))
                args.Catalogs.Add(new DirectoryCatalog(baseApplicationLocation, FrameworkAssembliesSearchPattern, args.Session));
        }

        private static bool CatalogsContainBaseDirectoryFrameworkCatalog(Fwbs.Framework.SessionInitializeEventArgs args, string baseApplicationLocation)
        {
            foreach (var catalog in args.Catalogs)
            {
                var dirCatalog = catalog as DirectoryCatalog;

                if (dirCatalog == null)
                    continue;

                string catalogPath = dirCatalog.Path;

                string searchPattern = GetSearchPatternFromDirectoryCatalog(dirCatalog);

                if (string.IsNullOrWhiteSpace(catalogPath) || string.IsNullOrWhiteSpace(searchPattern))
                    continue;

                catalogPath = catalogPath.TrimEnd('\\');
                baseApplicationLocation = baseApplicationLocation.TrimEnd('\\');

                if (catalogPath.Equals(baseApplicationLocation, StringComparison.InvariantCultureIgnoreCase)
                    && searchPattern.Equals(FrameworkAssembliesSearchPattern, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        private static string GetSearchPatternFromDirectoryCatalog(DirectoryCatalog catalog)
        {
            string searchPattern = null;

            try
            {
                BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
                var MEFCatalog = catalog.GetType().GetProperty("MEFCatalog", flags).GetValue(catalog, new object[] { });
                searchPattern = (string)MEFCatalog.GetType().GetProperty("SearchPattern", flags).GetValue(MEFCatalog, new object[] { });
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("GetSearchPatternFromDirectoryCatalog(): Failed to get search pattern from catalog. The error message is {0}", ex.Message));
                return null;
            }
            return searchPattern;
        }


        [EnquiryUsage(true)]
        public bool OnlyAllowStrongNamedAssemblies
        {
            get
            {
                if (IsLoggedIn)
                    return ConvertDef.ToBoolean(GetXmlProperty("OnlyAllowStrongNamedAssemblies", false), false);
                return false;
            }
            set
            {
                CheckLoggedIn();

                if (!_isAdminInstance)
                    throw new InvalidOperationException("OnlyAllowStrongNamedAssemblies property cannot only be set in the admin kit.");

                SetXmlProperty("OnlyAllowStrongNamedAssemblies", value);
            }
        }

        [EnquiryUsage(true)]
        public bool OnlyAllowRegisteredAssemblies
        {
            get
            {
                if (IsLoggedIn)
                    return ConvertDef.ToBoolean(GetXmlProperty("OnlyAllowRegisteredAssemblies", false), false);
                return false;
            }
            set
            {
                CheckLoggedIn();

                if (!_isAdminInstance)
                    throw new InvalidOperationException("OnlyAllowRegisteredAssemblies property cannot only be set in the admin kit.");

                SetXmlProperty("OnlyAllowRegisteredAssemblies", value);
            }
        }

        #endregion

        #region Session Properties

        public DistributedAssemblyManager DistributedAssemblyManager
        {
            get
            {
                if (_distributionmanager == null)
                    throw new InvalidOperationException("DistributedAssemblyManager has not been initialized. A connection to the database has exist first.");
                return _distributionmanager;
            }
        }

        public bool IsShuttingDown
        {
            get
            {
                return shuttingDown;
            }
        }

        public bool IsDisconnecting { get; private set; }

        public bool IsConnecting { get; private set; }

        public bool IsConnected { get; private set; }

        /// <summary>
        /// Gets the currently logged in session of the system.
        /// </summary>
        [Obsolete("We have created another accessor for this as CurrentSession to be more friendly.")]
        public static Session OMS
        {
            get
            {
                if (_oms == null)
                {
                    PrintStackTrace("OMS - Session _oms IS NULL", "Session");
                    _oms = new Session();
                }
                return _oms;
            }
        }


        /// <summary>
        /// Gets the currently logged in session of the system.
        /// </summary>
        public static Session CurrentSession
        {
            get
            {
                if (_oms == null)
                {
                    PrintStackTrace("CurrentSession - Session _oms IS NULL", "Session");
                    _oms = new Session();
                }
                return _oms;
            }
        }


        private string helpBasePathString = "NOTSET";  //Set as NOTSET as GetSpecificData returns "" if not set anyway.  We just need to know we've read the value once and prevent further hits to the database.
        public string GetHelpPath(string objectName)
        {
            string pathSeparator = @"\";
            string helpPath = string.Empty;

            if (IsLoggedIn)
            {
                //Get the basepath from Specific data

                if (helpBasePathString == "NOTSET")
                {
                    helpBasePathString = Convert.ToString(GetSpecificData("HELPFILEPATH"));
                }
                if (helpBasePathString.ToLower().StartsWith("http"))
                {
                    pathSeparator = @"/";
                }
                //Append pathSeparator if not there
                if (!helpBasePathString.EndsWith(pathSeparator))
                {
                    helpBasePathString += pathSeparator;
                }

                helpPath = helpBasePathString + CurrentUser.PreferedCulture + pathSeparator + objectName + ".htm";
                //Two options - HTTP and local (or network) file access
                if (helpBasePathString.ToLower().StartsWith("http"))
                {
                    //Can only call HTTP
                    return helpPath;
                }
                else
                {
                    //Check file exists for both culture, and no culture
                    if (File.Exists(helpPath))
                    {
                        return helpPath;
                    }
                    else
                    {
                        helpPath = helpBasePathString + objectName + ".htm";
                        if (File.Exists(helpPath))
                        {
                            return helpPath;
                        }
                        else
                        {
                            return "";
                        }
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// Gets the current UI type (windows, web etc...).
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        internal UIClientType UIType
        {
            get
            {
                return _clientType;
            }
        }

        /// <summary>
        /// Gets the current user who has logged into the system.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public User CurrentUser
        {
            get
            {
                //DM - 17/08/06 - Interferes with licensing.
                //CheckLoggedIn();
                return _currentUser;
            }
        }

        /// <summary>
        /// Gets the current power user settings.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Power CurrentPowerUserSettings
        {
            get
            {
                if (_currentPowerUserSettings == null)
                    _currentPowerUserSettings = RefreshPowerUserProfile();
                return _currentPowerUserSettings;
            }
        }

        /// <summary>
        /// Gets the current users last updated date
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public DateTimeNULL CurrentUserLastUpdated
        {
            get
            {
                return _currentUserLastUpdated;
            }
            set
            {
                _currentUserLastUpdated = value;
            }
        }

        /// <summary>
        /// Gets the current users last updated date
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public DateTimeNULL CurrentFeeEarnerLastUpdated
        {
            get
            {
                return _currentFeeEarnerLastUpdated;
            }
            set
            {
                _currentFeeEarnerLastUpdated = value;
            }
        }

        /// <summary>
        /// Gets the current users last updated date
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public DateTimeNULL CurrentFeeEarnerUserLastUpdated
        {
            get
            {
                return _currentFeeEarnerUserLastUpdated;
            }
            set
            {
                _currentFeeEarnerUserLastUpdated = value;
            }
        }

        /// <summary>
        /// Gets or Sets the current printer that.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Printer CurrentPrinter
        {
            get
            {
                CheckLoggedIn();
                if (_currentPrinter == null)
                    _currentPrinter = CurrentUser.DefaultPrinter;
                return _currentPrinter;
            }
            set
            {
                CheckLoggedIn();
                _currentPrinter = value;
            }
        }

        /// <summary>
        /// Gets or Sets the current branch.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Branch CurrentBranch
        {
            get
            {
                CheckLoggedIn();
                if (_currentBranch == null)
                {
                    _currentBranch = this;

                    if (SingleDatabaseInstance)
                        _currentBranch = _currentUser.Branch;
                }

                return _currentBranch;
            }
            set
            {
                CheckLoggedIn();
                _currentBranch = value;
            }
        }

        /// <summary>
        /// Gets or Sets the current Fee Earner is being used.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public FeeEarner CurrentFeeEarner
        {
            get
            {
                CheckLoggedIn();
                if (_currentFeeEarner == null)
                    _currentFeeEarner = CurrentUser.WorksFor;
                return _currentFeeEarner;
            }
            set
            {
                CheckLoggedIn();
                _currentFeeEarner = value;
            }
        }

        /// <summary>
        /// Gets the teminal object that is currently logged into the system through the session.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Terminal CurrentTerminal
        {
            get
            {
                return _currentTerminal;
            }
        }


        /// <summary>
        /// Gets the currently logged in users favourites list.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Favourites CurrentFavourites
        {
            get
            {
                CheckLoggedIn();
                _currentFavourites = new Favourites(CurrentUser.ID);
                return _currentFavourites;
            }
        }


        /// <summary>
        /// Gets or Sets the client object that is currently being worked on.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Client CurrentClient
        {
            get
            {
                CheckLoggedIn();
                return CurrentClients.LastItem as Client;
            }
            set
            {
                if (value != null)
                    CurrentClients.Add(value.ClientID.ToString(), value);
            }
        }

        /// <summary>
        /// Gets or Sets the file object that is currently being worked on.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public OMSFile CurrentFile
        {
            get
            {
                CheckLoggedIn();
                return CurrentFiles.LastItem as OMSFile;
            }
            set
            {
                if (value != null)
                    CurrentFiles.Add(value.ID.ToString(), value);
            }
        }

        /// <summary>
        /// Gets or Sets the whether the File Object was found through the Fileacccode or alternative method.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public bool CurrentFileGatheredFromAlternative
        {
            get
            {
                return _currentfilegathered;
            }
            set
            {
                _currentfilegathered = value;
            }
        }



        /// <summary>
        /// Gets or Sets the file object that is currently being worked on.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Associate CurrentAssociate
        {
            get
            {
                CheckLoggedIn();
                return CurrentAssociates.LastItem as Associate;
            }
            set
            {
                if (value != null)
                    CurrentAssociates.Add(value.ID.ToString(), value);
            }
        }

        /// <summary>
        /// Gets the current contact being worked on.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Contact CurrentContact
        {
            get
            {
                CheckLoggedIn();
                return CurrentContacts.LastItem as Contact;
            }
            set
            {
                if (value != null)
                    CurrentContacts.Add(value.ID.ToString(), value);
            }
        }




        [EnquiryUsage(true)]
        public bool UseTimeRecordedDate
        {
            get
            {
                return Convert.ToBoolean(GetXmlProperty("UseTimeRecordedDate", false));
            }
            set
            {
                SetXmlProperty("UseTimeRecordedDate", value);
            }
        }


        [EnquiryUsage(true)]
        public bool EnableDeleteMilstoneRole
        {
            get
            {
                return Convert.ToBoolean(GetXmlProperty("EnableDeleteMilstoneRole", false));
            }
            set
            {
                SetXmlProperty("EnableDeleteMilstoneRole", value);
            }
        }


        /// <summary>
        /// Gets a culture object for what ever sets it in the database. The current
        /// users UICultureInfo will override the branch and reg info one etc...
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public string DefaultCulture
        {
            get
            {
                if (!IsLoggedIn)
                {
                    return CultureInfo.CurrentUICulture.Name;
                }
                else
                {
                    string cult = _currentUser.PreferedCulture;
                    if (cult == "")
                    {
                        cult = base.PreferedCulture;
                        if (cult == "")
                        {
                            cult = PreferedCulture;
                        }
                    }
                    return cult;
                }
            }
        }


        public CultureInfo DefaultCultureInfo 
        {
            get
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(this.DefaultCulture);
                return Thread.CurrentThread.CurrentCulture;
            }
        }

        /// <summary>
        /// Gets the current precedent job list.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public PrecedentJobList CurrentPrecedentJobList
        {
            get
            {
                CheckLoggedIn();
                if (_joblist == null)
                    _joblist = new PrecedentJobList();
                return _joblist;
            }
        }

        /// <summary>
        /// Gets the default country identifier.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        [Obsolete("Use Address.CountryID instead.")]
        public int DefaultCountry
        {
            get
            {
                CheckLoggedIn();
                return Address.CountryID;
            }

        }


        /// <summary>
        /// Gets a collection of serice which will be cleared on logging off / disconnecting.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        internal System.Collections.Generic.List<IService> Services
        {
            get
            {
                if (services == null)
                    services = new System.Collections.Generic.List<IService>();
                return services;
            }
        }

        /// <summary>
        /// Gets the default currency to use using the system configuration.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public string DefaultCurrency
        {
            get
            {
                CheckLoggedIn();
                string ret = CurrentUser.Currency;
                if (ret == String.Empty)
                {
                    ret = base.Currency;
                    if (ret == String.Empty)
                        ret = Convert.ToString(GetExtraInfo("regcurISOCode"));
                }
                return ret;
            }
        }

        /// <summary>
        /// Gets the default currency format in a number info format object.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public NumberFormatInfo DefaultCurrencyFormat
        {
            get
            {
                if (_currency == null)
                    _currency = FWBS.OMS.Currency.GetCurrency(DefaultCurrency);
                return _currency.CurrencyFormat;
            }
        }

        /// <summary>
        /// Gets the default datetime format in a datetime info format object.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public DateTimeFormatInfo DefaultDateTimeFormat
        {
            get
            {
                return CultureInfo.CreateSpecificCulture(PreferedCulture).DateTimeFormat;
            }
        }
        /// <summary>
        /// Gets the current terminology parsing object that replaces common system terminolgies in different languages.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Terminology Terminology
        {
            get
            {
                CheckLoggedIn();
                return _terminolgy;
            }
        }

        /// <summary>
        /// Gets the commonly used resource strings of the system.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Res Resources
        {
            get
            {
                //This could be used the logon procedure so CheckLoggedIn() cannot be used.
                if (IsLoggedIn)
                {
                    if (_resources == null)
                        _resources = new Res();

                    return _resources;
                }
                else
                    return null;
            }
        }




        #endregion

        #region Preferences

        #region Time Recording Preferences
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool HideTimeLedgerTab
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regHideTimeLedgerTab", "false"), false);
            }
            set
            {
                SetXmlProperty("regHideTimeLedgerTab", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool HideCalculatedChargeCost
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regHideCalculatedChargeCost", "false"), false);
            }
            set
            {
                SetXmlProperty("regHideCalculatedChargeCost", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool HideBalances
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regHideBalances", "false"), false);
            }
            set
            {
                SetXmlProperty("regHideBalances", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string SelectAssociateEnquiryOverride
        {
            get
            {
                return Convert.ToString(GetXmlProperty("regSelectAssociateEnquiryOverride", ""));
            }
            set
            {
                SetXmlProperty("regSelectAssociateEnquiryOverride", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string SelectFileEnquiryOverride
        {
            get
            {
                return Convert.ToString(GetXmlProperty("regSelectFileEnquiryOverride", ""));
            }
            set
            {
                SetXmlProperty("regSelectFileEnquiryOverride", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string SelectClientEnquiryOverride
        {
            get
            {
                return Convert.ToString(GetXmlProperty("regSelectClientEnquiryOverride", ""));
            }
            set
            {
                SetXmlProperty("regSelectClientEnquiryOverride", value);
            }
        }
        #endregion

        #region Email Preferences

        /// <summary>
        /// Gets or Sets a flag that indicates whether the user automatically saves email attachments.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public bool AutoSaveAttachments
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regAutoSaveAttachments", true), true);
            }
            set
            {
                SetXmlProperty("regAutoSaveAttachments", value);
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public string ExcludeFileExtensions
        {
            get
            {
                return Convert.ToString(GetXmlProperty("ExcludeFileExtensions", "BMP|JPG|PNG"));
            }
            set
            {
                SetXmlProperty("ExcludeFileExtensions", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public int ExcludeFileSize
        {
            get
            {
                return ConvertDef.ToInt32(GetXmlProperty("ExcludeFileSize", 20), 20);
            }
            set
            {
                SetXmlProperty("ExcludeFileSize", value);
            }
        }

        /// <summary>
        /// Gets or Sets whether the a saved email is automatically deleted or moved after saved.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public string SavedEmailOption
        {
            get
            {
                /* M = Move
				 * D = Delete
				 * L = Leave
				 */
                return Convert.ToString(GetXmlProperty("regSavedEmailOption", "L")).ToUpper();
            }
            set
            {
                SetXmlProperty("regSavedEmailOption", value.ToUpper());
            }
        }

        /// <summary>
        /// Gets or Sets whether the a saved email is automatically deleted or moved after saved.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public string SavedSentEmailOption
        {
            get
            {
                /* M = Move
				 * D = Delete
				 * L = Leave
				 */
                return Convert.ToString(GetXmlProperty("regSavedSentEmailOption", "")).ToUpper();
            }
            set
            {
                SetXmlProperty("regSavedSentEmailOption", value.ToUpper());
            }
        }


        /// <summary>
        /// Gets or Sets the folder to move a newly saved email to, if the auto save operation is set to move.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public string SavedEmailFolderLocation
        {
            get
            {
                return Convert.ToString(GetXmlProperty("regSavedEmailFolderLocation", @"OMS\%CLNO%\%FILENO%"));
            }
            set
            {
                SetXmlProperty("regSavedEmailFolderLocation", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public bool EmailResolveAddress
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEmailResolveAddress", "true"), true);
            }
            set
            {
                SetXmlProperty("regEmailResolveAddress", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public bool EmailChecksum
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEmailChecksumSubject", "false"), false);
            }
            set
            {
                SetXmlProperty("regEmailChecksumSubject", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public bool EmailQuickSave
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEmailQuickSave", "false"), false);
            }
            set
            {
                SetXmlProperty("regEmailQuickSave", value);
            }
        }



        #endregion

        #region Email Profiling

        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public string EmailMessageTypes
        {
            get
            {
                return Convert.ToString(GetXmlProperty("regEmailMessageTypes", "IPM.NOTE*;REPORT.IPM.NOTE*;IPM.APPOINTMENT;IPM.TASK")).ToUpper();
            }
            set
            {
                if (value != EmailMessageTypes)
                {
                    SetXmlProperty("regEmailMessageTypes", value.ToUpper());
                }
            }
        }

        /// <summary>
        /// Gets or Sets the email profiling level.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public string EmailProfileLevel
        {
            get
            {
                /* S = Strict
				 * M = Medium
				 * D = Default
				 * C = Custom
				 */
                return Convert.ToString(GetXmlProperty("regEmailProfileLevel", "D")).ToUpper();
            }
            set
            {
                if (value != EmailProfileLevel)
                {
                    SetXmlProperty("regEmailProfileLevel", value.ToUpper());
                    switch (value)
                    {
                        case "S":
                            SetXmlProperty("regEmailProfileOnClose", true);
                            SetXmlProperty("regEmailProfileOnNew", true);
                            SetXmlProperty("regEmailProfileOnReply", true);
                            SetXmlProperty("regEmailProfileOnForward", true);
                            SetXmlProperty("regEmailProfileOnDelete", true);
                            SetXmlProperty("regEmailProfileAllowEdit", false);
                            SetXmlProperty("regEmailProfileOnMove", true);
                            break;
                        case "M":
                            SetXmlProperty("regEmailProfileOnClose", false);
                            SetXmlProperty("regEmailProfileOnNew", true);
                            SetXmlProperty("regEmailProfileOnReply", true);
                            SetXmlProperty("regEmailProfileOnForward", true);
                            SetXmlProperty("regEmailProfileOnDelete", false);
                            SetXmlProperty("regEmailProfileAllowEdit", false);
                            SetXmlProperty("regEmailProfileOnMove", true);
                            break;
                        case "D":
                            SetXmlProperty("regEmailProfileOnClose", "");
                            SetXmlProperty("regEmailProfileOnNew", "");
                            SetXmlProperty("regEmailProfileOnReply", "");
                            SetXmlProperty("regEmailProfileOnForward", "");
                            SetXmlProperty("regEmailProfileOnDelete", "");
                            SetXmlProperty("regEmailProfileAllowEdit", "");
                            SetXmlProperty("regEmailProfileOnMove", "");
                            break;
                    }

                    OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnClose", EmailProfileOnClose));
                    OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnMove", EmailProfileOnClose));
                    OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnNew", EmailProfileOnNew));
                    OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnReply", EmailProfileOnReply));
                    OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnForward", EmailProfileOnForward));
                    OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileOnDelete", EmailProfileOnDelete));
                    OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileAllowEdit", EmailProfileAllowEdit));
                }
            }
        }

        private void EmailProfileLevelResolve()
        {
            string val = "D";
            string old = EmailProfileLevel;
            if (EmailProfileOnClose && EmailProfileOnDelete && EmailProfileOnForward && EmailProfileOnNew && EmailProfileOnReply && EmailProfileOnMove && EmailProfileAllowEdit == false)
            {
                val = "S";
            }
            else if (EmailProfileOnClose == false && EmailProfileOnDelete == false && EmailProfileOnForward && EmailProfileOnNew && EmailProfileOnReply && EmailProfileOnMove && EmailProfileAllowEdit == false)
            {
                val = "M";
            }
            else if (EmailProfileOnClose == false && EmailProfileOnDelete == false && EmailProfileOnForward == false && EmailProfileOnNew == false && EmailProfileOnReply == false && EmailProfileOnMove == false && EmailProfileAllowEdit)
            {
                val = "D";
            }
            else
            {
                val = "C";
            }

            if (old != val)
            {
                SetXmlProperty("regEmailProfileLevel", val.ToUpper());
                OnPropertyChanged(new FWBS.OMS.EnquiryEngine.PropertyChangedEventArgs("EmailProfileLevel", old, val));
            }

        }

        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public bool EmailProfileOnClose
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEmailProfileOnClose", "false"), false);
            }
            set
            {
                SetXmlProperty("regEmailProfileOnClose", value);
                EmailProfileLevelResolve();
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public bool EmailProfileOnMove
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEmailProfileOnMove", "false"), false);
            }
            set
            {
                SetXmlProperty("regEmailProfileOnMove", value);
                EmailProfileLevelResolve();
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public bool EmailProfileOnNew
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEmailProfileOnNew", "false"), false);
            }
            set
            {
                SetXmlProperty("regEmailProfileOnNew", value);
                EmailProfileLevelResolve();
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public bool EmailProfileOnReply
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEmailProfileOnReply", "false"), false);
            }
            set
            {
                SetXmlProperty("regEmailProfileOnReply", value);
                EmailProfileLevelResolve();
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public bool EmailProfileOnForward
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEmailProfileOnForward", "false"), false);
            }
            set
            {
                SetXmlProperty("regEmailProfileOnForward", value);
                EmailProfileLevelResolve();
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public bool EmailProfileOnDelete
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEmailProfileOnDelete", "false"), false);
            }
            set
            {
                SetXmlProperty("regEmailProfileOnDelete", value);
                EmailProfileLevelResolve();
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("EMAIL")]
        public bool EmailProfileAllowEdit
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEmailProfileAllowEdit", "true"), true);
            }
            set
            {
                SetXmlProperty("regEmailProfileAllowEdit", value);
                EmailProfileLevelResolve();
            }
        }


        #endregion

        /// <summary>
        /// Flag to allow signatures to be embedded into Word Documents rather than linked as per default behaviour
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool EmbedSignaturesIntoWordDocument
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEmbedSignaturesIntoWordDocument", false), false);
            }
            set
            {
                SetXmlProperty("regEmbedSignaturesIntoWordDocument", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool AdvancedDocPropHandler
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regAdvancedDocPropHandler", false), false);
            }
            set
            {
                SetXmlProperty("regAdvancedDocPropHandler", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool PromptDocAlreadyOpen
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regPromptDocAlreadyOpen", false), false);
            }
            set
            {
                SetXmlProperty("regPromptDocAlreadyOpen", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public int PromptDocAlreadyOpenTime
        {
            get
            {
                return ConvertDef.ToInt32(GetXmlProperty("regPromptDocAlreadyOpenTime", 0), 0);
            }
            set
            {
                SetXmlProperty("regPromptDocAlreadyOpenTime", value);
            }
        }


        [EnquiryUsage(false)]
        [Browsable(false)]
        [Obsolete("Track change options no longer controlled by 3E MatterSphere", false)]
        [LocCategory("OMS")]
        public bool EnableTrackChanges
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEnableTrackChanges", "true"), true);
            }
            set
            {
                SetXmlProperty("regEnableTrackChanges", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool EnableTrackChangesWarning
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regEnableTrackChangesWarning", "false"), false);
            }
            set
            {
                SetXmlProperty("regEnableTrackChangesWarning", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool OverrideUsersInitials
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("OverrideUsersInitials", true), true);
            }
            set
            {
                SetXmlProperty("OverrideUsersInitials", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool HideAllWizardWelcomePages
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("HideAllWizardWelcomePage", false), false);
            }
            set
            {
                SetXmlProperty("HideAllWizardWelcomePage", value);
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool HideCancellationConfirmationDialog
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("HideCancellationConfirmationDialog", false), false);
            }
            set
            {
                SetXmlProperty("HideCancellationConfirmationDialog", value);
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool SpellingAndGrammarCheckRequired
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("SpellingAndGrammarCheckRequired", true), true);
            }
            set
            {
                SetXmlProperty("SpellingAndGrammarCheckRequired", value);
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string BulkImportWizard
        {
            get
            {
                return Convert.ToString(GetXmlProperty("BulkImportWizard", bulkImportWizardCode));
            }
            set
            {
                SetXmlProperty("BulkImportWizard", value);
            }
        }

        private string bulkImportWizardCode
        {
            get
            {
                if (this.EngineVersion.Major > 7)
                    return "sDOCBULK8";
                else
                    return "SCRDOCBULK";
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string SystemwideInitials
        {
            get
            {
                return Convert.ToString(GetXmlProperty("SystemwideInitials", string.Empty));
            }
            set
            {
                SetXmlProperty("SystemwideInitials", value);
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string CompareTool
        {
            get
            {
                return Convert.ToString(GetXmlProperty("CompareTool", null));
            }
            set
            {
                SetXmlProperty("CompareTool", value);
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string SystemwideUserName
        {
            get
            {
                return Convert.ToString(GetXmlProperty("SystemwideUserName", string.Empty));
            }
            set
            {
                SetXmlProperty("SystemwideUserName", value);
            }
        }

        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string DefaultSaveWizard
        {
            get
            {
                return Convert.ToString(GetXmlProperty("DefaultSaveWizard", "SCRDOCNEW"));
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                    value = "SCRDOCNEW";
                SetXmlProperty("DefaultSaveWizard", value);
            }
        }

        private const string defaultClientFileReturnCount = "DefaultClientFileReturnCount";
        /// <summary>
        /// The Number of files to be returned in the first request for files from the client.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public long DefaultClientFileReturnCount
        {
            get
            {
                return Convert.ToInt64(GetXmlProperty(defaultClientFileReturnCount, 100));
            }
            set
            {
                SetXmlProperty(defaultClientFileReturnCount, value);
            }

        }


        [EnquiryUsage(true)]
        [LocCategory("SCRIPT")]
        public string ScriptProviderOptions
        {
            get
            {
                return Convert.ToString(GetXmlProperty("ScriptProviderOptions", String.Empty));
            }
            set
            {
                SetXmlProperty("ScriptProviderOptions", value ?? String.Empty);
            }

        }

        [EnquiryUsage(true)]
        [LocCategory("SCRIPT")]
        public string ScriptCompilerOptions
        {
            get
            {
                return Convert.ToString(GetXmlProperty("ScriptCompilerOptions", String.Empty));
            }
            set
            {
                SetXmlProperty("ScriptCompilerOptions", value ?? String.Empty);
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("SCRIPT")]
        public string ScriptConditionalCompilationSymbols
        {
            get
            {
                return Convert.ToString(GetXmlProperty("ScriptConditionalCompilationSymbols", String.Empty));
            }
            set
            {
                SetXmlProperty("ScriptConditionalCompilationSymbols", value ?? String.Empty);
            }
        }


        /// <summary>
        /// Gets or Sets a flag that indicates whether a document will auto print at the end of a document save.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool AutoPrint
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regAutoPrint", true), true);
            }
            set
            {
                SetXmlProperty("regAutoPrint", value);
            }
        }

        public bool? AutoPrintByApplication(Guid ApplicationGuid)
        {
            string setting = Convert.ToString(GetXmlProperty("AP" + ApplicationGuid.ToString(), ""));
            if (string.IsNullOrEmpty(setting))
                return null;

            return ConvertDef.ToBoolean(setting, true);
        }

        public static DataTable ListAutoPrintByApplication()
        {
            CurrentSession.CheckLoggedIn();

            DataTable table = CurrentSession.Connection.ExecuteSQLTable("SELECT appGUID, appName FROM dbApplication", "AUTOPRINT", null);
            foreach (DataRow row in table.Rows)
            {
                bool? value = CurrentSession.AutoPrintByApplication(new Guid(Convert.ToString(row["appGUID"])));
                if (value == null)
                    row["appName"] = "[  ] " + Convert.ToString(row["appName"]);
                else if (value == true)
                    row["appName"] = "[O] " + Convert.ToString(row["appName"]);
                else if (value == false)
                    row["appName"] = "[X] " + Convert.ToString(row["appName"]);
            }
            return table;
        }

        public void SetAutoPrintByApplication(Guid ApplicationGuid, bool? value)
        {
            if (value == null)
                SetXmlProperty("AP" + ApplicationGuid.ToString(), "");
            else
                SetXmlProperty("AP" + ApplicationGuid.ToString(), value.ToString());
        }



        /// <summary>
        /// Gets or Sets a flag that indicates whether a the default associate is always used, when picking an associate.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool UseDefaultAssociate
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regUseDefaultAssociate", false), false);
            }
            set
            {
                SetXmlProperty("regUseDefaultAssociate", value);
            }
        }

        /// <summary>
        /// Gets or Sets the system wide default storage location for documents.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(false)]
        public short DefaultDocStorageProvider
        {
            get
            {
                CheckLoggedIn();
                return Common.ConvertDef.ToInt16(GetExtraInfo("regDefDocStorageLoc"), -1);
            }
            set
            {
                CheckLoggedIn();
                if (value < 0)
                    SetExtraInfo("regDefDocStorageLoc", DBNull.Value);
                else
                    SetExtraInfo("regDefDocStorageLoc", value);
            }
        }

        [EnquiryUsage(true)]
        [Browsable(false)]
        public short DefaultPrecStorageProvider
        {
            get
            {
                CheckLoggedIn();
                return Common.ConvertDef.ToInt16(GetExtraInfo("regDefPrecStorageLoc"), -1);
            }
            set
            {
                CheckLoggedIn();
                if (value < 0)
                    SetExtraInfo("regDefPrecStorageLoc", DBNull.Value);
                else
                    SetExtraInfo("regDefPrecStorageLoc", value);
            }
        }

        /// <summary>
        /// Gets or Sets the SMTP server (may also include a port number) that is to be used when sending quick emails.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        public string SMTPServer
        {
            get
            {
                CheckLoggedIn();
                return Convert.ToString(GetExtraInfo("regSMTPServer")).Split(';')[0];
            }
            set
            {
                CheckLoggedIn();
                string server = (value ?? string.Empty).Trim(), encryption = SMTPEncryption;
                if (encryption.Length > 0)
                    server += ";" + encryption;

                if (server.Length == 0)
                    SetExtraInfo("regSMTPServer", DBNull.Value);
                else
                    SetExtraInfo("regSMTPServer", server);
            }
        }

        /// <summary>
        /// Gets or Sets the SMTP encryption mode that is to be used when sending quick emails.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        public string SMTPEncryption
        {
            get
            {
                CheckLoggedIn();
                string[] parts = Convert.ToString(GetExtraInfo("regSMTPServer")).Split(';');
                return parts.Length > 1 ? parts[parts.Length - 1] : string.Empty;
            }
            set
            {
                CheckLoggedIn();
                string server = SMTPServer, encryption = (value ?? string.Empty).Trim();
                if (encryption.Length > 0)
                    server += ";" + encryption;

                if (server.Length == 0)
                    SetExtraInfo("regSMTPServer", DBNull.Value);
                else
                    SetExtraInfo("regSMTPServer", server);
            }
        }


        /// <summary>
        /// Gets a value which determines whether the database supports external document id's
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public bool SupportsExternalDocumentIds
        {
            get
            {
                CheckLoggedIn();
                return IsProcedureInstalled("sprDocumentRecordExternal");
            }
        }

        private bool? showExternalDocumentIds = null;

        [EnquiryUsage(false)]
        [Browsable(false)]
        public bool ShowExternalDocumentIds
        {
            get
            {
                if (showExternalDocumentIds == null)
                    showExternalDocumentIds = Convert.ToBoolean(new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "Data", "ShowExDocID").GetSetting(false));

                return showExternalDocumentIds.Value;
            }
        }

        [EnquiryUsage(false)]
        [Browsable(false)]
        public bool SupportsConfigurableDocumentTypes
        {
            get
            {
                CheckLoggedIn();
                return IsProcedureInstalled("sprDocumentType");
            }
        }


        /// <summary>
        /// Gets or Sets the boolean property that determines that a check for duplicate document
        /// identifiers must be performed because of OMS2k non replicated document tables.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool DuplicateDocumentIDs
        {
            get
            {
                CheckLoggedIn();
                return Common.ConvertDef.ToBoolean(GetExtraInfo("regduplicatedocid"), false);
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regduplicatedocid", value);
            }
        }

        /// <summary>
        /// Gets or Sets the single database instance.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool SingleDatabaseInstance
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("SingleDatabaseInstance", "false"), false);
            }
            set
            {
                SetXmlProperty("SingleDatabaseInstance", value);
            }
        }


        /// <summary>
        /// Gets or Sets the exclusive object locking value.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool ObjectLocking
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("ObjectLocking", "true"), true);
            }
            set
            {
                SetXmlProperty("ObjectLocking", value);
            }
        }


        /// <summary>
        /// Gets or Sets the Information Panel docked location.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string InformationPanelDockLocation
        {
            get
            {
                return Convert.ToString(GetXmlProperty("InformationPanelDockLocation", "DOCK2LFT"));
            }
            set
            {
                SetXmlProperty("InformationPanelDockLocation", value);
            }
        }

        /// <summary>
        /// Gets or Sets the succinct type display form caption value.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool SuccinctTypeDisplayFormCaption
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("SuccinctTypeDisplayFormCaption", "false"), false);
            }
            set
            {
                SetXmlProperty("SuccinctTypeDisplayFormCaption", value);
            }
        }

        /// <summary>
        /// Gets or Sets the Seed Number for Override.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public int SeedNumberOverride
        {
            get
            {
                try
                {
                    return ConvertDef.ToInt32(GetExtraInfo("regBranchConfig"), 0);
                }
                catch
                {
                    return 0;
                }

            }
            set
            {
                SetExtraInfo("regBranchConfig", value);
            }
        }

        /// <summary>
        /// Allows a namespace type prefix to be added to all document properties that
        /// for example appear in Word ole properties.  This is so that they do not conflict
        /// with other systems that may added the same property names.  Works in conjunction
        /// with ExternalDocumentPropertyNames.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string ExternalDocumentPropertyPrefix
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                if (_regInfo.Tables[Table].Columns.Contains("regExternalDocumentPropertyPrefix"))
                    return Convert.ToString(GetExtraInfo("regExternalDocumentPropertyPrefix"));
                else
                    return String.Empty;
            }
        }


        /// <summary>
        /// Allows a namespace type prefix to be added to all the listed fields.  Works in
        /// conjunction with ExternalDocumentPropertyPrefix
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string ExternalDocumentPropertyNames
        {
            get
            {
                string val = null;

                if (_regInfo.Tables[Table].Columns.Contains("regExternalDocumentPropertyNames"))
                    val = Convert.ToString(GetExtraInfo("regExternalDocumentPropertyNames"));

                if (String.IsNullOrEmpty(val))
                    return "EDITION;CLIENTID;FILEID;ASSOCID;DOCID;DOCIDEX;COMPANYID;VIEWMODE;CUSTOMFORM;DATAKEY;SERIALNO;BASEPRECID;BASEPRECTYPE;VERSIONID;VERSIONLABEL";
                else
                    return val;
            }
        }


        /// <summary>
        /// Gets or Sets the illegal file extensions for storage.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string IllegalFileExtensions
        {
            get
            {
                /*
					ADE - Microsoft Access Project Extension 
					ADP - Microsoft Access Project 
					BAS - Visual Basic Class Module 
					BAT - Batch File 
					CHM - Compiled HTML Help File 
					CMD - Windows NT Command Script 
					COM - MS-DOS Application 
					CPL - Control Panel Extension 
					CRT - Security Certificate 
					DLL - Dynamic Link Library 
					DO* - Word Documents and Templates 
					EXE - Application 
					HLP - Windows Help File 
					HTA - HTML Applications 
					INF - Setup Information File 
					INS - Internet Communication Settings 
					ISP - Internet Communication Settings 
					JS - JScript File 
					JSE - JScript Encoded Script File 
					LNK - Shortcut 
					MDB - Microsoft Access Application 
					MDE - Microsoft Access MDE Database 
					MSC - Microsoft Common Console Document 
					MSI - Windows Installer Package 
					MSP - Windows Installer Patch 
					MST - Visual Test Source File 
					OCX - ActiveX Objects 
					PCD - Photo CD Image 
					PIF - Shortcut to MS-DOS Program 
					POT - PowerPoint Templates 
					PPT - PowerPoint Files 
					REG - Registration Entries 
					SCR - Screen Saver 
					SCT - Windows Script Component 
					SHB - Document Shortcut File 
					SHS - Shell Scrap Object 
					SYS - System Config/Driver 
					URL - Internet Shortcut (Uniform Resource Locator) 
					VB - VBScript File 
					VBE - VBScript Encoded Script File 
					VBS - VBScript Script File 
					WSC - Windows Script Component 
					WSF - Windows Script File 
					WSH - Windows Scripting Host Settings File 
					XL* - Excel Files and Templates


				 */

                return Convert.ToString(GetXmlProperty("regIllegalFileExtensions", "ADE ADP BAS BAT CHM CMD COM CPL CRT DLL EXE HLP HTA INF INS ISP JS JSE LNK MDB MDE MSC MSI MSP MST OCX PCD PIF POT REG SCR SCT SHB SHS SYS URL VB VBE VBS WSC WSF WSH")).ToUpper();
            }
            set
            {
                if (value == null)
                    value = String.Empty;

                SetXmlProperty("regIllegalFileExtensions", value.ToUpper());
            }
        }

        /// <summary>
        /// Gets or Sets the duplicate document checking range.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string DuplicateDocumentCheckerLevel
        {
            get
            {
                /* O = Off
				 * S = System Wide
				 * C = By Client
				 * F = By File
				 */
                return Convert.ToString(GetXmlProperty("regDuplicateDocumentCheckerLevel", "F")).ToUpper();
            }
            set
            {
                SetXmlProperty("regDuplicateDocumentCheckerLevel", value.ToUpper());
            }
        }

        /// <summary>
        /// Gets or Sets how to deal with a duplicate document.
        /// </summary>
        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public string DuplicateDocumentAction
        {
            get
            {
                /* NODUPLICATION = No De-Duplication
				 * PROMPT = Prompt (Current Design)
				 * ATTACH = Attach to current document.
				 */
                return Convert.ToString(GetXmlProperty("regDuplicateDocumentAction", "PROMPT")).ToUpper();
            }
            set
            {
                SetXmlProperty("regDuplicateDocumentAction", value.ToUpper());
            }
        }

        /// <summary>
        /// Gets or Sets the number of days it takes to physically delete 
        /// a document / precedent after it is flagged  as deleted.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [DefaultValue(30)]
        public short DeletionRetentionPeriod
        {
            get
            {
                try
                {
                    return Convert.ToInt16(GetExtraInfo("regDocRetensionDays"));
                }
                catch
                {
                    return 30;
                }

            }
            set
            {
                try
                {
                    SetExtraInfo("regDocRetensionDays", value);
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets or sets the option to store previews of documents and precedents.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [DefaultValue(true)]
        public bool DocumentPreviewEnabled
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("regDocPreview", true), true);
            }
            set
            {
                try
                {
                    SetXmlProperty("regDocPreview", value);
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets or sets the system wide Document Versioning option.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [DefaultValue("O")]
        public string DocumentVersioning
        {
            get
            {
                //O = Overwrite (Standard)
                //N = Creates a brand new document (Save As);
                //V = Automatically creates new version based on the current level version.
                //M = Creates a new root/major version.
                string val = Convert.ToString(GetXmlProperty("regDocVersioning", "O"));
                switch (val)
                {
                    case "O":
                        return val;
                    case "N":
                        return val;
                    case "V":
                        return val;
                    case "M":
                        return val;
                    default:
                        return "O";
                }
            }
            set
            {
                switch (value)
                {
                    case "O":
                        break;
                    case "N":
                        break;
                    case "V":
                        break;
                    case "M":
                        break;
                    default:
                        value = "O";
                        break;
                }

                SetXmlProperty("regDocVersioning", value);
            }
        }

        /// <summary>
        /// Gets or sets the system wide Subdocument Versioning option.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [DefaultValue("V")]
        public string SubdocumentVersioning
        {
            get
            {
                //N = Creates a brand new document.
                //V = Automatically creates new version based on the current level version.
                string val = Convert.ToString(GetXmlProperty("regSubdocVersioning", "V"));
                switch (val)
                {
                    case "N":
                        return val;
                    case "V":
                        return val;
                    default:
                        return "V";
                }
            }
            set
            {
                switch (value)
                {
                    case "N":
                        break;
                    case "V":
                        break;
                    default:
                        value = "V";
                        break;
                }

                SetXmlProperty("regSubdocVersioning", value);
            }
        }

        [EnquiryUsage(true)]
        [Browsable(true)]
        public string DocumentAddinOverride
        {
            get
            {
                return Convert.ToString(GetXmlProperty("DocumentAddinOverride", ""));
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetXmlProperty("DocumentAddinOverride", value);
            }
        }

        [EnquiryUsage(true)]
        [Browsable(true)]
        public string DocumentOpenAddinOverride
        {
            get
            {
                return Convert.ToString(GetXmlProperty("DocumentOpenAddinOverride", ""));
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetXmlProperty("DocumentOpenAddinOverride", value);
            }
        }

        [EnquiryUsage(true)]
        [Browsable(true)]
        public int DocumentMaximumRevisionCount
        {
            get
            {
                return ConvertDef.ToInt32(GetXmlProperty("DocumentMaximumRevisionCount", 3),3);
            }
            set
            {
                SetXmlProperty("DocumentMaximumRevisionCount", value);
            }
        }

        [EnquiryUsage(true)]
        [Browsable(true)]
        public bool EnablePrecedentVersioning
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("EnablePrecedentVersioning", false), false);
            }
            set
            {
                SetXmlProperty("EnablePrecedentVersioning", value);
            }
        }


        [EnquiryUsage(true)]
        [Browsable(true)]
        public string DocumentLockingFileStatuses
        {
            get
            {
                return Convert.ToString(GetXmlProperty("DocumentLockingFileStatuses", ""));
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetXmlProperty("DocumentLockingFileStatuses", value);
            }
        }


        [EnquiryUsage(true)]
        [Browsable(true)]
        public string DocumentPickerAddinOverride
        {
            get
            {
                return Convert.ToString(GetXmlProperty("DocumentPickerAddinOverride", ""));
            }
            set
            {
                if (value == null)
                    value = string.Empty;
                SetXmlProperty("DocumentPickerAddinOverride", value);
            }
        }

        /// <summary>
        /// Gets or sets the file type wide Document locking option.
        /// </summary>
        [DefaultValue("S")]
        [EnquiryUsage(true)]
        [Browsable(true)]
        public string DocumentLocking
        {
            get
            {

                //E = Exclusive
                //S = Shared;

                string val = Convert.ToString(GetXmlProperty("typeDocLocking", "S"));
                switch (val)
                {
                    case "E":
                        return val;
                    case "S":
                        return val;
                    default:
                        return "S";
                }
            }
            set
            {
                switch (value)
                {
                    case "E":
                        break;
                    case "S":
                        break;
                    default:
                        value = String.Empty;
                        break;
                }

                SetXmlProperty("typeDocLocking", value);
            }
        }



        /// <summary>
        /// Gets or sets the policy option for what happens to documents after a file is terminated.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [DefaultValue("")]
        public string DocumentRetentionPolicy
        {
            get
            {
                return Convert.ToString(GetXmlProperty("regDocRetentionPolicy", ""));
            }
            set
            {
                try
                {
                    SetXmlProperty("regDocRetentionPolicy", value);
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets or sets the number of days a document should be archived before after a file is terminated.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [DefaultValue(-1)]
        public int DocumentRetentionPeriod
        {
            get
            {
                return ConvertDef.ToInt32(GetXmlProperty("regDocRetentionPeriod", -1), -1);
            }
            set
            {
                try
                {
                    SetXmlProperty("regDocRetentionPeriod", value);
                }
                catch { }
            }
        }

        /// <summary>
        /// Gets the UI culture string ID (en-GB) for the branch.  This will only be accessed by the
        /// Session class and not outside.  This is because the session object will determine
        /// the currently used UICulture setting by checking whether one exists on the
        /// branch, reginfo or user.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        new public string PreferedCulture
        {
            get
            {
                if (!IsLoggedIn)
                {
                    return CultureInfo.CurrentUICulture.Name;
                }
                else
                {
                    try
                    {
                        string culture = Convert.ToString(GetExtraInfo("regUICultureInfo"));
                        CultureInfo.CreateSpecificCulture(culture);
                        return culture;
                    }
                    catch
                    {
                        return "";
                    }
                }
            }
        }

        /// <summary>
        /// Gets the maximum of times a user can enter a bas password.  If exceeds then the user is made inactive.
        /// </summary>
        [EnquiryUsage(true)]
        public byte MaximumLoginAttempts
        {
            get
            {
                CheckLoggedIn();
                return (byte)GetExtraInfo("regFailedLoginAttempts");
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regFailedLoginAttempts", value);
            }
        }


        [EnquiryUsage(true)]
        public bool EnableAddins
        {
            get
            {
                try
                {
                    return ConvertDef.ToBoolean(GetXmlProperty("regEnableAddins", "false"), false);
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                try
                {
                    SetXmlProperty("regEnableAddins", value);
                }
                catch { }
            }
        }

        [Flags]
        private enum SecurityEnabled { Document = 128, File = 256, Client = 512, Contact = 1024 }

        private SecurityEnabled GetSecurityEnabled()
        {
            SecurityEnabled sec = (SecurityEnabled)ConvertDef.ToEnum(Convert.ToInt32(this.GetSpecificData("SECLEVEL")), SecurityEnabled.Client | SecurityEnabled.Contact | SecurityEnabled.File | SecurityEnabled.Document);
            return sec;
        }

        [EnquiryUsage(true)]
        public bool AdvancedSecurity
        {
            get
            {
                return AdvancedSecurityEnabled;
            }
        }

        [EnquiryUsage(true)]
        public bool AdvancedSecurityEnabled
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(GetXmlProperty("AdvancedSecurityEnabled", false), CultureInfo.InvariantCulture);
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                SetXmlProperty("AdvancedSecurityEnabled", value);
            }
        }

        [EnquiryUsage(true)]
        public bool MatterSphereSecurityEnabled
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(GetXmlProperty("MatterSphereSecurityEnabled", false), CultureInfo.InvariantCulture);
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                SetXmlProperty("MatterSphereSecurityEnabled", value);
            }
        }

        [EnquiryUsage(true)]
        public bool AdvancedSecurityClientActive
        {
            get
            {
                try
                {
                    return (GetSecurityEnabled() & SecurityEnabled.Client) == SecurityEnabled.Client;
                }
                catch
                {
                    return true;
                }
            }
        }

        [EnquiryUsage(true)]
        public bool AdvancedSecurityDocumentActive
        {
            get
            {
                try
                {
                    return (GetSecurityEnabled() & SecurityEnabled.Document) == SecurityEnabled.Document;
                }
                catch
                {
                    return true;
                }
            }
        }

        [EnquiryUsage(true)]
        public bool AdvancedSecurityContactActive
        {
            get
            {
                try
                {
                    return (GetSecurityEnabled() & SecurityEnabled.Contact) == SecurityEnabled.Contact;
                }
                catch
                {
                    return true;
                }
            }
        }

        [EnquiryUsage(true)]
        public bool AdvancedSecurityFileActive
        {
            get
            {
                try
                {
                    return (GetSecurityEnabled() & SecurityEnabled.File) == SecurityEnabled.File;
                }
                catch
                {
                    return true;
                }
            }
        }


        [EnquiryUsage(true)]
        public bool AutoCheckInUnchangedDocuments
        {
            get
            {
                try
                {
                    return Convert.ToBoolean(GetXmlProperty("AutoCheckInUnchangedDocuments", false), CultureInfo.InvariantCulture);
                }
                catch
                {
                    return false;
                }
            }
            set
            {
                SetXmlProperty("AutoCheckInUnchangedDocuments", value);
            }
        }


        [EnquiryUsage(true)]
        public string AdditionalDocumentSaveCommands
        {
            get
            {
                return Convert.ToString(GetXmlProperty("AdditionalDocumentSaveCommands", ""));
            }
            set
            {
                SetXmlProperty("AdditionalDocumentSaveCommands", value);
            }
        }

        [EnquiryUsage(true)]
        public bool PromptBeforeSaveAsOnResave
        {
            get
            {
                return Convert.ToBoolean(GetXmlProperty("PromptBeforeSaveAsOnResave", false));
            }
            set
            {
                SetXmlProperty("PromptBeforeSaveAsOnResave", value);
            }
        }

        /// <summary>
        /// Gets or sets the system wide Document Versioning option.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [DefaultValue("I")]
        public string DefaultOptionOnResaveSaveAs
        {
            get
            {
                //O = Overwrite (Standard)
                //N = Creates a brand new document (Save As);
                //V = Automatically creates new version based on the current level version.
                //M = Creates a new root/major version.
                //I = Dont use this seetting
                string val = Convert.ToString(GetXmlProperty("DefaultOptionOnResaveSaveAs", "I"));
                switch (val)
                {
                    case "O":
                    case "N":
                    case "V":
                    case "M":
                        return val;
                    default:
                        return "I";
                }
            }
            set
            {
                switch (value)
                {
                    case "O":
                    case "N":
                    case "V":
                    case "M":
                        break;
                    default:
                        value = "I";
                        break;
                }

                SetXmlProperty("DefaultOptionOnResaveSaveAs", value);
            }
        }


        [EnquiryUsage(true)]
        public bool ForceSaveAsWhenEditedByAnother
        {
            get
            {
                return Convert.ToBoolean(GetXmlProperty("ForceSaveAsWhenEditedByAnother", false));
            }
            set
            {
                SetXmlProperty("ForceSaveAsWhenEditedByAnother", value);
            }
        }

        [EnquiryUsage(true)]
        public bool UnlockOriginalOnSaveAs
        {
            get
            {
                return Convert.ToBoolean(GetXmlProperty("UnlockOriginalOnSaveAs", false));
            }
            set
            {
                SetXmlProperty("UnlockOriginalOnSaveAs", value);
            }
        }

        [EnquiryUsage(true)]
        public bool QuickSaveOnCheckIn
        {
            get
            {
                return Convert.ToBoolean(GetXmlProperty("QuickSaveOnCheckIn", false));
            }
            set
            {
                SetXmlProperty("QuickSaveOnCheckIn", value);
            }
        }



        [EnquiryUsage(true)]
        internal bool SupplyUserContext
        {
            get
            {
                CheckLoggedIn();

                return ConvertDef.ToBoolean(GetXmlProperty("SupplyUserToken", true), true);
            }
            set
            {
                CheckLoggedIn();

                SetXmlProperty("SupplyUserToken", value);
            }
        }


        [EnquiryUsage(true)]
        [Browsable(true)]
        [DefaultValue(false)]
        public bool PromptToPublishPDFWithTrackChanges
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("PromptToPublishPDFWithTrackChanges", false), false);
            }
            set
            {
                SetXmlProperty("PromptToPublishPDFWithTrackChanges", value);
            }
        }


        [EnquiryUsage(true)]
        [LocCategory("OMS")]
        public bool NotifyOfOpeningUnsupportedFiletype
        {
            get
            {
                return ConvertDef.ToBoolean(GetXmlProperty("NotifyOfOpeningUnsupportedFiletype", true), true);
            }
            set
            {
                SetXmlProperty("NotifyOfOpeningUnsupportedFiletype", value);
            }
        }


        #endregion

        #region "Document Archiving settings"

        /// <summary>
        /// Maximum number of documents that can be archived at a time
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [DefaultValue(1000)]
        public short MaximumArchiveDocumentCount
        {
            get
            {
                return Convert.ToInt16(GetXmlProperty("MaximumArchiveDocumentCount", 1000));
            }
            set
            {
                if (value < 0)
                    value = 0;

                SetXmlProperty("MaximumArchiveDocumentCount", value);
            }
        }


        /// <summary>
        /// Number of days since the creation of the document. Decides when documents are available for archiving.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [DefaultValue(365)]
        public short DocumentArchivingRetentionPeriod
        {
            get
            {
                return Convert.ToInt16(GetXmlProperty("DocumentArchivingRetentionPeriod", 365));
            }
            set
            {
                if (value < 0)
                    value = 0;

                SetXmlProperty("DocumentArchivingRetentionPeriod", value);
            }
        }


        /// <summary>
        /// Matter statuses to exclude from archiving
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        public string DocumentArchivingExclusionFileStatuses
        {
            get
            {
                return Convert.ToString(GetXmlProperty("DocumentArchivingExclusionFileStatuses", ""));
            }
            set
            {
                SetXmlProperty("DocumentArchivingExclusionFileStatuses", value);
            }
        }


        /// <summary>
        /// The default document archiving directory
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        public short DefaultDocumentArchivingDirectoryID
        {
            get
            {
                return Convert.ToInt16(GetXmlProperty("DefaultDocumentArchivingDirectoryID", -1));
            }
            set
            {
                SetXmlProperty("DefaultDocumentArchivingDirectoryID", value);
            }
        }


        #endregion

        #region PostCodeAnyWhere Settings
        /// <summary>
        /// Gets or Sets the PostCode Anywhere license type.  True if done per terminal
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [Obsolete("This property has been deprecated in V10.1")]
        public bool PCATerminalLicense
        {
            get
            {
                CheckLoggedIn();
                return ConvertDef.ToBoolean(GetXmlProperty("pcaTerminalLicense", "false"), false);
            }
            set
            {
                CheckLoggedIn();
                SetXmlProperty("pcaTerminalLicense", value);
            }
        }

        /// <summary>
        /// Gets or Sets the web address to the PostCode Anywhere web service.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [Obsolete("This property has been deprecated in V10.1")]
        public string PCAWebServiceURL
        {
            get
            {
                CheckLoggedIn();
                return Convert.ToString(GetXmlProperty("pcaWebServiceURL", "http://services.postcodeanywhere.co.uk/uk/lookup.asmx"));
            }
            set
            {
                CheckLoggedIn();
                SetXmlProperty("pcaWebServiceURL", value);
            }
        }


        /// <summary>
        /// Gets or Sets the global license account code to log into PostCode Anywhere.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [Obsolete("This property has been deprecated in V10.1")]
        public string PCAAccountCode
        {
            get
            {
                CheckLoggedIn();
                return Convert.ToString(GetXmlProperty("pcaAccountCode", ""));
            }
            set
            {
                CheckLoggedIn();
                SetXmlProperty("pcaAccountCode", value);
            }
        }

        /// <summary>
        /// Gets or Sets the global license key to log into PostCode Anywhere.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [Obsolete("This property has been deprecated in V10.1")]
        public string PCALicenseKey
        {
            get
            {
                CheckLoggedIn();
                return Convert.ToString(GetXmlProperty("pcaLicenseKey", ""));
            }
            set
            {
                CheckLoggedIn();
                SetXmlProperty("pcaLicenseKey", value);
            }
        }

        /// <summary>
        /// Gets or Set  the global machine id to log into PostCode Anywhere.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        [Obsolete("This property has been deprecated in V10.1")]
        public string PCAMachineId
        {
            get
            {
                CheckLoggedIn();
                return Convert.ToString(GetXmlProperty("pcaMachineId", ""));
            }
            set
            {
                CheckLoggedIn();
                SetXmlProperty("pcaMachineId", value);
            }
        }

        #endregion

        #region Properties

        public bool ApplicationRolePresent
        {
            get
            {
                if (databaseSettings == null || databaseSettings.ApplicationRoleName.Length == 0)
                {
                    return false;
                }

                if (Encryption.NewKeyDecrypt(databaseSettings.ApplicationRoleName).ToUpper() == "OMSAPPLICATIONROLE")
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Search Button open the Search Manager
        /// </summary>
        [EnquiryUsage(true)]
        public bool SearchButtonUseSearchManager
        {
            get
            {
                return Convert.ToBoolean(GetXmlProperty("SearchButtonUseSearchManager", true));
            }
            set
            {
                SetXmlProperty("SearchButtonUseSearchManager", value);
            }
        }

        /// <summary>
        /// Is The Search Server Configured
        /// </summary>
        public bool IsSearchConfigured
        {
            get
            {
                if (_isSearchConfigured == null)
                {
                    _isSearchConfigured = (IsMSSearchConfigured || IsESSearchConfigured) &&
                                          (string.IsNullOrEmpty(CodeLookup.GetLookup("USRROLES", "HIDESEARCH")) || !CurrentUser.IsInRoles("HIDESEARCH"));
                }
                return (bool)_isSearchConfigured;
            }
        }

        /// <summary>
        /// Is Elasticsearch Search Configured
        /// </summary>
        public bool IsESSearchConfigured
        {
            get
            {
                if (_isESSearchConfigured == null)
                {
                    _isESSearchConfigured = !string.IsNullOrEmpty(Convert.ToString(GetSpecificData("ES_SERV"))) &&
                                            !string.IsNullOrEmpty(Convert.ToString(GetSpecificData("ES_DIND")));
                }
                return (bool)_isESSearchConfigured;
            }
        }

        /// <summary>
        /// Is MatterSphere Search Configured
        /// </summary>
        public bool IsMSSearchConfigured
        {
            get
            {
                if (_isMSSearchConfigured == null)
                {
                    _isMSSearchConfigured = !string.IsNullOrEmpty(Convert.ToString(GetSpecificData("CDS_CON")));
                }
                return (bool)_isMSSearchConfigured;
            }
        }

        public bool IsSearchSummaryFieldEnabled
        {
            get
            {
                if (_isSearchSummaryFieldEnabled == null)
                {
                    const string sql = "SELECT TOP 1 SummaryFieldEnabled FROM [search].[ChangeVersionControl]";
                    _isSearchSummaryFieldEnabled = IsESSearchConfigured && ConvertDef.ToBoolean(Connection.ExecuteSQLScalar(sql, null), false);
                }
                return (bool)_isSearchSummaryFieldEnabled;
            }
        }

        private const int defaultMinimalSymbolsCountForSuggests = 1;

        /// <summary>
        /// Minimal Symbols Count To Request Suggests
        /// </summary>
        public int MinimalSymbolsCountForSuggests
        {
            get
            {
                if (_minimalSymbolsCountForSuggests == null)
                {
                    string[] options = Convert.ToString(GetSpecificData("SUGGESTOPTIONS")).Split(';');
                    _minimalSymbolsCountForSuggests = ConvertDef.ToInt32(options[0].Trim(), defaultMinimalSymbolsCountForSuggests);
                }
                return (int)_minimalSymbolsCountForSuggests;
            }
        }

        private const int defaultMaximumSuggestsAmount = 10;

        /// <summary>
        /// Maximum Suggests Amount To Request
        /// </summary>
        public int MaximumSuggestsAmount
        {
            get
            {
                if (_maximumSuggestsAmount == null)
                {
                    string[] options = Convert.ToString(GetSpecificData("SUGGESTOPTIONS")).Split(';');
                    _maximumSuggestsAmount = (options.Length > 1) ? ConvertDef.ToInt32(options[1].Trim(), defaultMaximumSuggestsAmount) : defaultMaximumSuggestsAmount;
                }
                return (int)_maximumSuggestsAmount;
            }
        }

        /// <summary>
        /// Use External Balances
        /// </summary>
        public bool? UseExternalBalances
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        internal bool? IsPowerConfigured
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the company email Disclaimer.  This will be added to all emails merged by
        /// the system.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(true)]
        public string Disclaimer
        {
            get
            {
                CheckLoggedIn();
                return Convert.ToString(GetExtraInfo("regDisclaimer"));
            }
            set
            {
                CheckLoggedIn();
                if (value.Trim() == String.Empty)
                    SetExtraInfo("regDisclaimer", DBNull.Value);
                else
                    SetExtraInfo("regDisclaimer", value);
            }
        }

        /// <summary>
        /// Gets or Sets the default company signature.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(false)]
        new public Signature CompanySignature
        {
            get
            {
                CheckLoggedIn();
                if (GetExtraInfo("regSignature") == DBNull.Value)
                    return Signature.Empty;
                else
                    return new Signature((byte[])GetExtraInfo("regSignature"), "company");
            }
            set
            {
                CheckLoggedIn();
                if (value == null || value.Equals(Signature.Empty) || value == Signature.Empty)
                    SetExtraInfo("regSignature", DBNull.Value);
                else
                    SetExtraInfo("regSignature", value.ToByteArray());

                base.CompanySignature = value;
            }
        }

        /// <summary>
        /// Gets or Sets the default file copy logo.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(false)]
        public Signature FileCopyLogo
        {
            get
            {
                CheckLoggedIn();
                if (GetExtraInfo("regFileCopyLogo") == DBNull.Value)
                    return new Signature();
                else
                    return new Signature((byte[])GetExtraInfo("regFileCopyLogo"), "filecopy");
            }
            set
            {
                CheckLoggedIn();
                if (value == null || value.ToBitmap() == null)
                    SetExtraInfo("regFileCopyLogo", new Signature(new System.Drawing.Bitmap(16, 16)).ToByteArray());
                else
                    SetExtraInfo("regFileCopyLogo", value.ToByteArray());
            }
        }

        /// <summary>
        /// Gets or Sets the default draft logo.
        /// </summary>
        [EnquiryUsage(true)]
        [Browsable(false)]
        public Signature DraftLogo
        {
            get
            {
                CheckLoggedIn();
                if (GetExtraInfo("regDraftLogo") == DBNull.Value)
                    return new Signature();
                else
                    return new Signature((byte[])GetExtraInfo("regDraftLogo"), "draft");
            }
            set
            {
                CheckLoggedIn();
                if (value == null || value.ToBitmap() == null)
                    SetExtraInfo("regDraftLogo", new Signature(new System.Drawing.Bitmap(16, 16)).ToByteArray());
                else
                    SetExtraInfo("regDraftLogo", value.ToByteArray());
            }
        }

        /// <summary>
        /// Gets the Partner Company Name.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public string PartnerCompanyName
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                return Convert.ToString(GetExtraInfo("regPartnerCompanyName"));
            }
        }



        /// <summary>
        /// Gets the Partner Website Details.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public string PartnerWebSite
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                return Convert.ToString(GetExtraInfo("regPartnerWebSite"));
            }
        }

        /// <summary>
        /// Gets the Partner Website Details.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public string PartnerSupportTelephone
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                return Convert.ToString(GetExtraInfo("regPartnerSupportTel"));
            }
        }

        /// <summary>
        /// Gets the Partner Support Email.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public string PartnerSupportEmail
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                return Convert.ToString(GetExtraInfo("regPartnerSupportEmail"));
            }
        }

        /// <summary>
        /// Gets the Partner Telephone.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public string PartnerTelephone
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                return Convert.ToString(GetExtraInfo("regPartnerTel"));
            }
        }

        /// <summary>
        /// Gets the Partner Fax.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public string PartnerFax
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                return Convert.ToString(GetExtraInfo("regPartnerFax"));
            }
        }

        /// <summary>
        /// Gets the Partner Address.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public Address PartnerAddress
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                return FWBS.OMS.Address.GetAddress(Convert.ToInt64(GetExtraInfo("regPartnerAddress")));
            }
        }



        /// <summary>
        /// Gets the company name.
        /// </summary>
        [EnquiryUsage(true)]
        public string CompanyName
        {
            get
            {
                CheckLoggedIn();
                return GetExtraInfo("regCompanyName").ToString();
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regCompanyName", value);
            }
        }

        /// <summary>
        /// Company registration identifier.  Multiple branches may have the same id number.
        /// </summary>
        [EnquiryUsage(false)]
        public long CompanyID
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                return Common.ConvertDef.ToInt64(GetExtraInfo("regCompanyID"), 0);
            }
        }

        /// <summary>
        /// Data instance identifier.  Collates multiple replicated database instance into a common key.
        /// This will be used alongside SerialNumber and CompanyID as metadata inside documents.
        /// </summary>
        [EnquiryUsage(false)]
        public string DataKey
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                if (_regInfo.Tables[Table].Columns.Contains("regDataKey"))
                    return Convert.ToString(GetExtraInfo("regDataKey"));
                else
                    return String.Empty;
            }
        }

        /// <summary>
        /// Gets the OMS edition / version type - EN = Enterprise, EP = Epitome.
        /// </summary>
        [EnquiryUsage(false)]
        public string Edition
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                return Convert.ToString(GetExtraInfo("regEdition"));
            }
        }


        /// <summary>
        /// Gets the serial number of the software.
        /// </summary>
        [EnquiryUsage(false)]
        public long SerialNumber
        {
            get
            {
                //NOTE: This has to be used before loggin. CheckLoggedIn() cannot be used.
                return Common.ConvertDef.ToInt64(GetExtraInfo("regSerialNo"), 0);
            }
        }




        /// <summary>
        /// Gets the company administrator of the system.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public User Administrator
        {
            get
            {
                CheckLoggedIn();
                if (GetExtraInfo("regAdministrator") is System.DBNull)
                    return null;
                else
                    return new User((int)GetExtraInfo("regAdministrator"));
            }
        }

        /// <summary>
        /// Gets the number of time units within an hour.
        /// </summary>
        [EnquiryUsage(true)]
        public byte TimeUnitValue
        {
            get
            {
                CheckLoggedIn();
                try
                {
                    return (byte)GetExtraInfo("regTimeUnitValue");
                }
                catch
                {
                    return 6;
                }
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regTimeUnitValue", value);
            }
        }

        /// <summary>
        /// Gets the date at which the companies financial year starts.
        /// </summary>
        [EnquiryUsage(true)]
        public DateTime FinancialStartDate
        {
            get
            {
                CheckLoggedIn();
                try
                {
                    return (DateTime)GetExtraInfo("regFinancialStart");
                }
                catch
                {
                    return DateTime.Now;
                }
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regFinancialStart", value);
            }
        }

        /// <summary>
        /// Gets the sales tax rate used (VAT).
        /// </summary>
        [EnquiryUsage(true)]
        public float SalesTaxRate
        {
            get
            {
                CheckLoggedIn();
                return Convert.ToSingle(GetExtraInfo("regSalesTaxRate"));
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regSalesTaxRate", value);
            }
        }

        /// <summary>
        /// Gets the interest rate that the company charges.
        /// </summary>
        [EnquiryUsage(true)]
        public float InterestRate
        {
            get
            {
                CheckLoggedIn();
                return Convert.ToSingle(GetExtraInfo("regInterestRate"));
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regInterestRate", value);
            }
        }

        /// <summary>
        /// Gets the default number of days for a task reminder.
        /// </summary>
        [EnquiryUsage(true)]
        public short TaskReminder
        {
            get
            {
                CheckLoggedIn();
                return (short)GetExtraInfo("regTaskReminder");
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regTaskReminder", value);
            }
        }

        /// <summary>
        /// Gets whether the system mail server is enabled or not.
        /// </summary>
        [EnquiryUsage(true)]
        public bool IsMailEnabled
        {
            get
            {
                CheckLoggedIn();
                return (bool)GetExtraInfo("regMailEnabled");
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regMailEnabled", value);
            }
        }

        /// <summary>
        /// Gets whether the client conflicting check is enabled.
        /// </summary>
        [EnquiryUsage(true)]
        public bool IsClientConflictCheckEnabled
        {
            get
            {
                CheckLoggedIn();
                return (bool)GetExtraInfo("regConflictClientCheck");
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regConflictClientCheck", value);
            }
        }


        /// <summary>
        /// Gets whether the file conflicting check is enabled.
        /// </summary>
        [EnquiryUsage(true)]
        public bool IsFileConflictCheckEnabled
        {
            get
            {
                CheckLoggedIn();
                return (bool)GetExtraInfo("regConflictFileCheck");
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regConflictFileCheck", value);
            }
        }


        /// <summary>
        /// Gets or Sets whether to market newly created clients.
        /// </summary>
        [EnquiryUsage(true)]
        public bool IsMarketNewClientEnabled
        {
            get
            {
                CheckLoggedIn();
                return ConvertDef.ToBoolean(GetExtraInfo("regMarketNewClient"), false);
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regMarketNewClient", value);
            }
        }


        /// <summary>
        /// Gets or Sets the logging activity severity value.
        /// </summary>
        [EnquiryUsage(true)]
        public byte LoggingActivitySeverity
        {
            get
            {
                CheckLoggedIn();
                return (byte)GetExtraInfo("regLoggingSeverity");
            }
            set
            {
                CheckLoggedIn();
                SetExtraInfo("regLoggingSeverity", value);
            }
        }

        /// <summary>
        /// Gets or Sets the Minimise On Opening a Documentp roperty
        /// </summary>
        [EnquiryUsage(true)]
        public bool MinimiseWindowOnOpen
        {
            get
            {
                CheckLoggedIn();
                return ConvertDef.ToBoolean(GetXmlProperty("MinimiseWindowOnOpen", false), false);
            }
            set
            {
                CheckLoggedIn();
                SetXmlProperty("MinimiseWindowOnOpen", value);
            }
        }

        [EnquiryUsage(false)]
        [Browsable(false)]
        public bool IsAutomation { get; set; }

        #endregion

        #region Information

        /// <summary>
        /// Gets the connection settings of the current session.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public DatabaseSettings CurrentDatabase
        {
            get
            {
                CheckLoggedIn();
                CheckCriticalDataAccess(Assembly.GetCallingAssembly(), "DatabaseSettings");
                return _multidb;
            }
        }

        /// <summary>
        /// Gets the database version.
        /// </summary>
        private Version dbver = null;

        [EnquiryUsage(false)]
        public Version DatabaseVersion
        {
            get
            {
                if (dbver == null || dbver.Equals(new Version(0, 0, 0, 0)))
                {
                    try
                    {
                        DataTable dt = Connection.ExecuteSQLTable("select top 1 vermajor, verminor, verbuild, verrevision from dbversion order by vermajor desc, verminor desc, verbuild desc, verrevision desc", "VERSION", new IDataParameter[0]);
                        DataRow r = dt.Rows[0];
                        dbver = new Version(ConvertDef.ToInt32(r["vermajor"], -1), ConvertDef.ToInt32(r["verminor"], -1), ConvertDef.ToInt32(r["verbuild"], -1), ConvertDef.ToInt32(r["verrevision"], -1));
                    }
                    catch
                    {
                        dbver = new Version(0, 0, 0, 0);
                    }
                }
                return dbver;
            }
        }

        /// <summary>
        /// Gets the database version.
        /// </summary>
        [EnquiryUsage(false)]
        public Version MinimumDatabaseVersion
        {
            get
            {
                Version ver = null;

                // Minimum Database Version Required to run this code base.
                ver = new Version(8, 0, 0, 1);

                return ver;
            }
        }


        /// <summary>
        /// Gets the Executing Business Layer Assembly File Version
        /// </summary>
        [EnquiryUsage(false)]
        public Version EngineVersion
        {
            get
            {
                Version ver = null;
                try
                {
                    object[] attrs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyFileVersionAttribute), false);
                    AssemblyFileVersionAttribute attr = (AssemblyFileVersionAttribute)attrs[0];
                    ver = new Version(attr.Version);
                }
                catch
                {
                    try
                    {
                        ver = Assembly.GetExecutingAssembly().GetName(true).Version;
                    }
                    catch
                    {
                        ver = new Version(0, 0, 0, 0);
                    }
                }

                return ver;

            }
        }

        /// <summary>
        /// Gets the Executing Business Layer Assembly Version
        /// </summary>
        [EnquiryUsage(false)]
        public Version AssemblyVersion
        {
            get
            {
                var ver = Assembly.GetExecutingAssembly().GetName(true).Version;
                return ver;
            }
        }

        #endregion

        #region IExtraInfo Implementation

        public bool ContainsExtraInfo(string fieldName)
        {
            return _regInfo.Tables[Table].Columns.Contains(fieldName);
        }

        new public object GetExtraInfo(string fieldName)
        {
            if (_regInfo.Tables[Table].Columns.Contains(fieldName))
            {
                object val = _regInfo.Tables[Table].Rows[0][fieldName];
                //UTCFIX: DM - 30/11/06 - return local time
                if (val is DateTime)
                    return ((DateTime)val).ToLocalTime();
                else
                    return val;
            }
            else
                return base.GetExtraInfo(fieldName);
        }

        new public void SetExtraInfo(string fieldName, object val)
        {
            //UTCFIX: DM - 04/12/06 - Make unspecified dates default to local;
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

            if (_regInfo.Tables[Table].Columns.Contains(fieldName))
                _regInfo.Tables[Table].Rows[0][fieldName] = val;
            else
                base.SetExtraInfo(fieldName, val);

        }

        new public Type GetExtraInfoType(string fieldName)
        {
            if (_regInfo.Tables[Table].Columns.Contains(fieldName))
                return _regInfo.Tables[Table].Columns[fieldName].DataType;
            else
                return base.GetExtraInfoType(fieldName);
        }

        new public DataSet GetDataset()
        {
            CheckLoggedIn();
            return _regInfo.Copy();
        }

        new public DataTable GetDataTable()
        {
            CheckLoggedIn();
            return _regInfo.Tables[Table].Copy();
        }

        #endregion

        #region IUpdateable Implementation

        public override void Update()
        {
            CheckLoggedIn();
            base.Update();

            DataRow row = _regInfo.Tables[Table].Rows[0];

            if (_regInfo.Tables[Table].GetChanges() != null)
            {
                //Set the primary key of the underlying table if not already done so for conccurency and merging issues.
                if (_regInfo.Tables[Table].PrimaryKey == null || _regInfo.Tables[Table].PrimaryKey.Length == 0)
                    _regInfo.Tables[Table].PrimaryKey = new DataColumn[1] { _regInfo.Tables[Table].Columns["brid"] };

                Connection.Update(row, "dbRegInfo");

                _isdirty = false;
            }
        }

        /// <summary>
        /// Cancels any changes made to the object.
        /// </summary>
        public override void Cancel()
        {
            _sessionconfig = null;
            _regInfo.Tables[Table].RejectChanges();
            base.Cancel();
        }

        /// <summary>
        /// Gets a boolean flag indicating whether any changes have been made to the object.
        /// </summary>
        [EnquiryUsage(false)]
        [Browsable(false)]
        public override bool IsDirty
        {
            get
            {
                bool dirty = base.IsDirty;
                if (dirty == false)
                    dirty = (_isdirty || _regInfo.Tables[Table].GetChanges() != null);

                return dirty;
            }
        }

        #endregion

        #region IDatabaseSchema Implementation

        /// <summary>
        /// Returns a list of tables within the database.
        /// </summary>
        /// <returns>A data table of tables.</returns>
        public DataTable GetTables()
        {
            CheckLoggedIn();
            return Connection.GetTables();
        }

        /// <summary>
        /// Returns a list of views within the database.
        /// </summary>
        /// <returns>A data table of views.</returns>
        public DataTable GetViews()
        {
            CheckLoggedIn();
            return Connection.GetViews();
        }

        /// <summary>
        /// Returns a list of columns within a specified table / view.
        /// </summary>
        /// <param name="objectName">Table / view name.</param>
        /// <returns>A data table of columns.</returns>
        public DataTable GetColumns(string objectName)
        {
            CheckLoggedIn();
            return Connection.GetColumns(objectName);
        }

        /// <summary>
        /// Returns a list of stored procedures within the database.
        /// </summary>
        /// <returns>A data table of stored procedures.</returns>
        public DataTable GetProcedures()
        {
            CheckLoggedIn();
            return Connection.GetProcedures();
        }

        /// <summary>
        /// Returns a list of columns within a specified table / view.
        /// </summary>
        /// <param name="procedureName">Procedure name.</param>
        /// <returns>A data table of parameters.</returns>
        public DataTable GetParameters(string procedureName)
        {
            CheckLoggedIn();
            return Connection.GetParameters(procedureName);
        }

        /// <summary>
        /// Fetches the primary key field name of a particular table.
        /// </summary>
        /// <param name="tableName">Table name within the database.</param>
        /// <returns>Data table of primary key information.</returns>
        public DataTable GetPrimaryKey(string tableName)
        {
            CheckLoggedIn();
            return Connection.GetPrimaryKey(tableName);
        }

        /// <summary>
        /// Fetches the list of columns that the stored procedure is going to return back.
        /// </summary>
        /// <param name="procedureName">Stored procedure name.</param>
        /// <returns>A data table of columns.</returns>
        public DataTable GetProcedureColumns(string procedureName)
        {
            CheckLoggedIn();
            return Connection.GetProcedureColumns(procedureName);
        }

        /// <summary>
        /// Fetches a list of linked servers associated to the server.
        /// </summary>
        /// <returns>A data table of linked servers.</returns>
        public DataTable GetLinkedServers()
        {
            CheckLoggedIn();
            return Connection.GetLinkedServers();
        }

        #endregion

        #region IEnquiryCompatible

        new public Enquiry Edit(KeyValueCollection param)
        {
            return Edit(DefaultSystemForm(SystemForms.SystemEdit), param);
        }

        #endregion

        #region Captured Events

        private void Connection_StateChanged(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Open)
            {
                ApplyUserContext(_currentUser);
            }
        }

        private void ApplyUserContext(User user)
        {
            if (IsLoggedIn && user != null && IsProcedureInstalled("SetUserContext") && SupplyUserContext)
            {
                var pars = new IDataParameter[] { Connection.CreateParameter("USERID", user.ActiveDirectoryID) };
                Connection.ExecuteProcedure("SetUserContext", pars);
            }
        }

        private void ApplyUserContext(ExecuteEventArgs args)
        {
            if (IsLoggedIn && _currentUser != null && SupplyUserContext)
            {
                if (args.CommandType == CommandType.StoredProcedure && !dbinfo.IsProcedure(args.SQL, true))
                {
                    return;
                }

                if (args.SQL.IndexOf(DatabaseInformation.UserContextParameter, StringComparison.OrdinalIgnoreCase) > -1)
                {
                    args.AdditionalParameters.Add(args.Connection.CreateParameter(DatabaseInformation.UserContextParameter, _currentUser.ActiveDirectoryID));
                }
            }
        }

        private void Connection_ConnectionError(object sender, ConnectionErrorEventArgs e)
        {
            OnConnectionError(e);
        }

        private void Connection_ShutdownRequest(object sender, EventArgs e)
        {
            shuttingDown = true;
            this.InternalDisconnect(true);

            OnShutdownRequest();
        }

        private void Connection_BeforeExecute(object sender, ExecuteEventArgs e)
        {
            ApplyUserContext(e);
        }

        private void Connection_BeforeExecuteTable(object sender, ExecuteTableEventArgs e)
        {
            ApplyUserContext(e);

            foreach (Caching.IQueryCache qc in CachedQueries)
            {
                if (qc.Handles(e))
                {
                    e.Data = qc.GetData(e);
                    return;
                }
            }
        }

        private void Connection_BeforeExecuteDataSet(object sender, ExecuteDataSetEventArgs e)
        {
            ApplyUserContext(e);

            foreach (Caching.IQueryCache qc in CachedQueries)
            {
                if (qc.Handles(e))
                {
                    e.Data = qc.GetData(e);
                    return;
                }
            }
        }

        private void Connection_AfterExecuteTable(object sender, ExecuteTableEventArgs e)
        {
            if (!e.Cached)
            {
                foreach (Caching.IQueryCache qc in CachedQueries)
                {
                    if (qc.Handles(e))
                    {
                        qc.SetData(e);
                        return;
                    }
                }
            }
        }

        private void Connection_AfterExecuteDataSet(object sender, ExecuteDataSetEventArgs e)
        {
            if (!e.Cached)
            {
                foreach (Caching.IQueryCache qc in CachedQueries)
                {
                    if (qc.Handles(e))
                    {
                        qc.SetData(e);
                        return;
                    }
                }
            }
        }

        internal void ClearCurrentPowerUserSettings()
        {
            _currentPowerUserSettings = null;
        }

        #endregion

        #region ISecurable Members

        string Security.ISecurable.SecurityId
        {
            get { return "*"; }
        }

        FWBS.OMS.Security.SecurityOptions FWBS.OMS.Security.ISecurable.SecurityOptions
        {
            get
            {
                return (FWBS.OMS.Security.SecurityOptions)0;
            }
            set
            {
                throw new NotSupportedException("Security Flags are not valid on a Session");
            }
        }

        #endregion


        public void ConfigureCache()
        {
            //Configure the schema cache collections
            ConfigureCache1();
            //Configure the data cache collections
            ConfigureCache2();
        }
    }


    /// <summary>
    /// A list of predefined system user identifiers.
    /// </summary>
    public enum SystemUsers
    {
        Admin = -1,
        Guest = -2,
        Partner = -3,
        FWBS = -4,
        JobProcessor = -100,
        CoreImport = -101
    }



    /// <summary>
    /// Password request delegate.
    /// </summary>
    public delegate void PasswordRequestEventHandler(IPasswordProtected sender, CancelEventArgs e);



    /// <summary>
    /// Ask request event handler.
    /// </summary>
    public delegate void AskEventHandler(object sender, AskEventArgs e);

    public class AskEventArgs : EventArgs
    {
        private ResourceItem _message;
        private AskResult _result;

        private AskEventArgs() { }

        public AskEventArgs(string code, string type, AskResult result, params string[] param)
        {
            string question = CodeLookup.GetLookup(type, code);
            for (int ctr = param.GetLowerBound(0); ctr <= param.GetUpperBound(0); ctr++)
            {
                System.Text.StringBuilder str = new System.Text.StringBuilder();
                str.Append("%");
                str.Append((ctr + 1).ToString());
                str.Append("%");
                question = question.Replace(str.ToString(), param[ctr].ToString());
            }
            question = Session.CurrentSession.Terminology.Parse(question, true);
            _message = new ResourceItem(question, "");
            _result = result;
        }

        public AskEventArgs(string code, string message, string help, AskResult result, params string[] param)
        {
            _message = Session.CurrentSession.Resources.GetMessage(code, message, help, true, param);
            _result = result;
        }

        public ResourceItem Message
        {
            get
            {
                return _message;
            }
        }


        public AskResult Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }
    }

    /// <summary>
    /// An enumration used by the ask event to get decisions from the user interface.
    /// </summary>
    [Flags()]
    public enum AskResult
    {
        No,
        Yes
    }


    /// <summary>
    /// Prompt request event handler.
    /// </summary>
    public delegate void PromptEventHandler(object sender, PromptEventArgs e);

    public class PromptEventArgs : EventArgs
    {
        private PromptType _type = PromptType.Search;
        private string _code = "";
        private object _result = null;
        private object[] _filter = new object[0];
        private string _message = "";

        private PromptEventArgs() { }

        public PromptEventArgs(PromptType type, string code, object[] filter, string message)
        {
            _code = code;
            _type = type;
            _filter = filter;
            _message = message;
        }

        public PromptType Type
        {
            get
            {
                return _type;
            }
        }

        public string Code
        {
            get
            {
                return _code;
            }
        }


        public object Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public object[] Filter
        {
            get
            {
                return _filter;
            }
        }

    }

    public enum PromptType
    {
        Search,
        InputBox
    }

    /// <summary>
    /// A structure which holds created and modification dates and users.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ModificationData : LookupTypeDescriptor
    {
        private DateTimeNULL _created;
        private string _createdBy;
        private DateTimeNULL _updated;
        private string _updatedBy;

        public override string ToString()
        {
            return this.Updated + ", " + this.UpdatedBy;
        }


        public ModificationData(DateTimeNULL created, string createdBy, DateTimeNULL updated, string updatedBy)
        {
            _created = created;
            _createdBy = createdBy;
            _updated = updated;
            _updatedBy = updatedBy;
        }

        public ModificationData(DateTimeNULL created, int createdBy, DateTimeNULL updated, int updatedBy)
        {
            User current = Session.CurrentSession.CurrentUser;
            _created = created;
            _createdBy = "";
            if (createdBy == current.ID)
                _createdBy = current.FullName;
            else
            {
                if (createdBy != 0)
                {
                    try
                    {
                        _createdBy = User.GetUser(createdBy).FullName;
                    }
                    catch { }
                }
            }

            _updated = updated;
            _updatedBy = "";

            if (updatedBy == current.ID)
                _updatedBy = current.FullName;
            else
            {
                if (updatedBy != 0)
                {
                    try
                    {
                        _updatedBy = User.GetUser(updatedBy).FullName;
                    }
                    catch { }
                }
            }
        }

        public DateTimeNULL Created
        {
            get
            {
                return _created;
            }
        }

        public string CreatedBy
        {
            get
            {
                return _createdBy;
            }
        }

        public DateTimeNULL Updated
        {
            get
            {
                return _updated;
            }
        }

        public string UpdatedBy
        {
            get
            {
                return _updatedBy;
            }
        }
    }



    public struct Alert : IComparable<Alert>
    {
        string _message;
        AlertStatus _status;

        public Alert(string message, AlertStatus status)
        {
            _message = message;
            _status = status;
        }

        public string Message
        {
            get
            {
                if (_message == null) _message = "";
                return _message;
            }
        }

        public AlertStatus Status
        {
            get
            {
                return _status;
            }
        }

        internal void ChangeStatus(AlertStatus status)
        {
            _status = status;
        }

        public enum AlertStatus
        {
            Red = 0,
            Amber = 1,
            Green = 2,
            Off = -1
        }

        #region IComparable<Alert> Members

        public int CompareTo(Alert other)
        {
            return -Status.CompareTo(other.Status);
        }

        #endregion
    }



    /// <summary>
    /// Alert delegate.
    /// </summary>
    public delegate void AlertEventHandler(object sender, AlertEventArgs e);


    /// <summary>
    /// Alert event arguments.
    /// </summary>
    public class AlertEventArgs : EventArgs
    {
        /// <summary>
        /// The list of alerts.
        /// </summary>
        private Alert[] _alerts;

        /// <summary>
        /// Default constructor not used.
        /// </summary>
        private AlertEventArgs() { }

        /// <summary>
        /// Creates an instance of the alert event arguments.
        /// </summary>
        /// <param name="alerts">An array of alerts.</param>
        public AlertEventArgs(Alert[] alerts)
        {
            _alerts = alerts;
        }

        /// <summary>
        /// Alert message that would be displayed to the user.  The message should already
        /// be language specific.
        /// </summary>
        public Alert[] Alerts
        {
            get
            {
                return _alerts;
            }
        }
    }

    /// <summary>
    /// Prompt request for a search object.
    /// </summary>
    public delegate void ShowSearchEventHandler(object sender, ShowSearchEventArgs e);

    public class ShowSearchEventArgs : EventArgs
    {
        private string _message = "";
        private SearchEngine.SearchList _sch;

        private ShowSearchEventArgs() { }

        public ShowSearchEventArgs(SearchEngine.SearchList sch, string message)
        {
            _sch = sch;
            _message = message;
        }

        public string Message
        {
            get
            {
                return _message;
            }
        }

        public SearchEngine.SearchList SearchList
        {
            get
            {
                return _sch;
            }
        }
    }

    /// <summary>
    /// Prompt request for a enquiry object.
    /// </summary>
    public delegate void ShowEnquiryEventHandler(object sender, ShowEnquiryEventArgs e);

    public class ShowEnquiryEventArgs : CancelEventArgs
    {
        private EnquiryEngine.Enquiry _enq;

        public object ReturnObject = null;

        private ShowEnquiryEventArgs() { }

        public ShowEnquiryEventArgs(EnquiryEngine.Enquiry enq)
        {
            _enq = enq;
        }

        public EnquiryEngine.Enquiry Enquiry
        {
            get
            {
                return _enq;
            }
        }
    }



    /// <summary>
    /// Prompt request for a ExtendedData object.
    /// </summary>
    public delegate void ShowExtendedDataEventHandler(object sender, ShowExtendedDataEventArgs e);

    public class ShowExtendedDataEventArgs : CancelEventArgs
    {
        private string _extcode;
        private FWBS.OMS.Interfaces.IEnquiryCompatible _enq;

        private ShowExtendedDataEventArgs() { }

        public ShowExtendedDataEventArgs(string ExtendedCode, FWBS.OMS.Interfaces.IEnquiryCompatible EnquiryObject)
        {
            _extcode = ExtendedCode;
            _enq = EnquiryObject;
        }

        public string ExtendedCode
        {
            get
            {
                return _extcode;
            }
        }

        public FWBS.OMS.Interfaces.IEnquiryCompatible Object
        {
            get
            {
                return _enq;
            }
        }
    }
}

