using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using MSOffice = Microsoft.Office.Core;
using MSOutlook = Microsoft.Office.Interop.Outlook;

namespace Fwbs.Oms.Office.Outlook
{


    using Fwbs.Office.Outlook;
    using FWBS.Common;
    using FWBS.OMS;
    using FWBS.OMS.UI.Windows.Office;

    public partial class OutlookOMSAddin
    {
        #region Fields

        private Common.OfficeOMSAddin addin;
        private MSOutlook.Inspectors inspectors;
        private MSOutlook.Explorers explorers;
        private OutlookOMS omsapp;
        private Common.ExternalOfficeOMSAddin extAddin;
     

        /// <summary>
        /// Get the OMSOutlook Application to work with the core functions.
        /// </summary>
        public OutlookOMS OmsApp
        {
            get { return omsapp; }
        }

        private OutlookApplication wrappedapp;
        public OutlookApplication WrappedApplication
        {
            get
            {
                if (wrappedapp == null)
                    wrappedapp = OutlookApplication.GetApplication(Application);
                return wrappedapp;
            }
        }

        #endregion

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
           

            if (!DisableFolderSwitch)
            {
                FWBS.OMS.Session.CurrentSession.Disconnected += new EventHandler(CurrentSession_Disconnected);
                FWBS.OMS.Session.CurrentSession.Connected += new EventHandler(CurrentSession_Connected);
            }
            Connect();

            if (!DisableFolderSwitch)
            {
                //This next line needs to be here as the WrappedApplication is not instantiated until the 
                //Connect method is called.
                var exp = this.WrappedApplication.ActiveExplorer();

                if (exp != null)
                    exp.BeforeFolderSwitch += new Microsoft.Office.Interop.Outlook.ExplorerEvents_10_BeforeFolderSwitchEventHandler(OutlookOMSAddin_BeforeFolderSwitch);
            }

        }

        private void CurrentSession_Connected(object sender, EventArgs e)
        {

            var exp = this.WrappedApplication.ActiveExplorer();

            if (exp == null)
                return;

            if (exp.CurrentFolder == null)
                return;

            var url = exp.CurrentFolder.WebViewURL;

            if ((exp.CurrentFolder.WebViewOn == false) && (!String.IsNullOrEmpty(url) && url.Contains("FWBS")))
            {
                exp.CurrentFolder.WebViewOn = true;

                exp.SelectFolder(exp.CurrentFolder);

            }
        }

        private void CurrentSession_Disconnected(object sender, EventArgs e)
        {
            var exp = this.WrappedApplication.ActiveExplorer();

            if (exp == null)
                return;

            if (exp.CurrentFolder == null)
                return;

            var url = exp.CurrentFolder.WebViewURL;

            if ((exp.CurrentFolder.WebViewOn == true) && (!String.IsNullOrEmpty(url) && url.Contains("FWBS")))
            {
                exp.CurrentFolder.WebViewOn = false;
                exp.SelectFolder(exp.CurrentFolder);
            }

        }

        private string lastfolder = String.Empty;
        private void OutlookOMSAddin_BeforeFolderSwitch(object NewFolder, ref bool Cancel)
        {
            var inFolder = NewFolder as Microsoft.Office.Interop.Outlook.MAPIFolder;

            if (inFolder == null)
                return;

            if (String.IsNullOrEmpty(inFolder.WebViewURL))
                return;


            const string ExistingMarker = @"FWBS\Outlook";
            const string NewMarker = "FWBS";


  

            if (!FWBS.OMS.Session.CurrentSession.IsLoggedIn)
            {
                if (inFolder.WebViewURL.Contains(ExistingMarker) || inFolder.WebViewURL.StartsWith(NewMarker))
                {
                    // If an FWBS Folder and the toggle is pressed then change the webview only if FWBS otherwise don't change.
                    inFolder.WebViewOn = false;
                }

                return;
            }

            if (lastfolder == inFolder.FolderPath)
            {
                lastfolder = String.Empty;
                return;
            }

            lastfolder = inFolder.FullFolderPath;

            // Session logged in and available to check folder for Properties for Matter Centre.
            var of = omsapp.GetFolderMessage(inFolder, false);
            if (of == null)
            {
                if (inFolder.WebViewURL.Contains(ExistingMarker) || inFolder.WebViewURL.StartsWith(NewMarker))
                {
                    inFolder.WebViewOn = false;
                }

                return;
            }


            string file = String.Empty;

            if (inFolder.WebViewURL.StartsWith(NewMarker))
            {
                file = ExternalOutlookOMSAddin.CreateWebViewHtmFile(inFolder.WebViewURL);                
            }

            if (inFolder.WebViewURL.Contains(ExistingMarker))
            {
                file = inFolder.WebViewURL;
            }



            if (!File.Exists(file))
            {
                file = ExternalOutlookOMSAddin.CreateWebViewHtmFile(Path.GetFileName(file));
            }

            if (file != inFolder.WebViewURL)
            {
                inFolder.WebViewURL = file;
            }

            inFolder.WebViewOn = true;


        }


        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            Disconnect();
        }

        protected override object RequestService(Guid serviceGuid)
        {
            if (serviceGuid == typeof(MSOffice.IRibbonExtensibility).GUID)
            {
                CreateOMSApp();
                return addin; 
            }

            return base.RequestService(serviceGuid);
        }



        protected override object RequestComAddInAutomationService()
        {
            if (extAddin == null)
            {
                extAddin = new ExternalOutlookOMSAddin();
            }
            return extAddin;
        }

        #region Methods


        private void CreateOMSApp()
        {
            if (addin == null)
            {
                omsapp = new OutlookOMS(Application, "OUTLOOK", Val(Application.Version) < 14);
                addin = new Common.OfficeOMSAddin(omsapp, Application, Application.ActiveExplorer().CommandBars, CustomTaskPanes, false);

                addin.ControlAction += new Fwbs.Oms.Office.Common.ControlActionCallbackDelegate(addin_ControlAction);
                addin.ControlResourceRequest += new Fwbs.Oms.Office.Common.ControlResourceRequestDelegate(addin_ControlResourceRequest);
                addin.ControlSuperTipRequest += new Fwbs.Oms.Office.Common.ControlStringCallbackDelegate(addin_ControlSuperTipRequest);
                addin.ControlVisibleRequest += new Fwbs.Oms.Office.Common.ControlVisibleCallbackDelegate(addin_ControlVisibleRequest);
                
                if (extAddin != null)
                {
                    extAddin.Addin = addin;
                }
            }
        }

        private static int Val(string value)
        {
            if (String.IsNullOrEmpty(value))
                return 0;

            return int.Parse(value.Trim().Split('.')[0]); 
        }

        private void Connect()
        {
            CreateOMSApp();
            AttachEvents();
            addin.AutoConnect();
        }

        private void Disconnect()
        {
            DetachEvents();
            Session.CurrentSession.Disconnect();
        }

        private void AttachEvents()
        {
            inspectors = WrappedApplication.Inspectors;
            explorers = WrappedApplication.Explorers;

            inspectors.NewInspector += new Microsoft.Office.Interop.Outlook.InspectorsEvents_NewInspectorEventHandler(inspectors_NewInspector);

            WrappedApplication.FolderContextMenuDisplay += new Microsoft.Office.Interop.Outlook.ApplicationEvents_11_FolderContextMenuDisplayEventHandler(Application_FolderContextMenuDisplay);
        }



        private void DetachEvents()
        {
            inspectors.NewInspector -= new Microsoft.Office.Interop.Outlook.InspectorsEvents_NewInspectorEventHandler(inspectors_NewInspector);
            WrappedApplication.FolderContextMenuDisplay -= new Microsoft.Office.Interop.Outlook.ApplicationEvents_11_FolderContextMenuDisplayEventHandler(Application_FolderContextMenuDisplay);
        }

        private static Microsoft.Office.Core.CommandBarButton CreateFolderButton(string code, string text, Microsoft.Office.Core.CommandBar bar, Microsoft.Office.Core._CommandBarButtonEvents_ClickEventHandler handler)
        {
            var btn = (Microsoft.Office.Core.CommandBarButton)bar.Controls.Add(Microsoft.Office.Core.MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            btn.Caption = FWBS.OMS.Session.CurrentSession.Resources.GetResource(code, text, "").Text;
            btn.Tag = "F_CONTEXT" + code; 
            btn.Click += handler;
            return btn;
        }


        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        private static void NavigateToAssociatesView(long fileID)
        {
            OMSFile file = OMSFile.GetFile(fileID);
            FileType fileType = FileType.GetFileType(file.FileTypeCode);

            foreach (OMSType.Tab tab in fileType.Tabs)
            {
                if (tab.Description.Contains(CodeLookup.GetLookup(Session.CurrentSession.Edition, "%ASSOCIATES%")))
                    FWBS.OMS.UI.Windows.Services.ShowFile(file, tab.OMSObjectCode);
            }
        }

        #endregion

        #region Captured Events

        private void addin_ControlAction(object sender, Fwbs.Oms.Office.Common.ControlActionCallbackEventArgs e)
        {
            string[] commands = e.Command.Split(';');

            if (commands.Length > 1 && commands[0].ToUpper() == "ADDIN")
            {
                string command = commands[1].ToUpper();
                if (command == "ASSOCDOCSPANE")
                {
                    Common.Panes.AssociatedDocumentsPane.Create<OutlookAssociatedDocuments>((Common.OfficeOMSAddin)sender, command, e.Context, false).Visible = true;
                    e.Handled = true;
                }
                else if (command == "SENDOUTBOX")
                {
                    WrappedApplication.SendOutbox();
                    e.Handled = true;
                }
                else if (command == "DELETEITEM")
                {
                    e.CancelDefault = false;
                    if (OmsApp.CheckEmailProfileOption(EmailProfileOption.epoDelete))
                    {
                        OutlookExplorer explorer = WrappedApplication.GetExplorer(e.Context as MSOutlook.Explorer);
                        if (explorer != null && explorer.IsMailView)
                        {
                            object[] args = new object[] { null, false };
                            explorer.GetType().InvokeMember("ButtonClickPermanentDelete", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.InvokeMethod, null, explorer, args);
                            e.CancelDefault = (bool)args[1];
                        }
                    }
                    e.Handled = true;
                }
                else if (command == "EDITMESSAGE")
                {
                    e.CancelDefault = false;
                    if (!OmsApp.CheckEmailProfileOption(EmailProfileOption.epoAllowEdit))
                    {
                        OutlookInspector inspector = WrappedApplication.GetInspector(e.Context as MSOutlook.Inspector);
                        if (inspector != null && OmsApp.CanSaveItemAsDocument(inspector.CurrentItem))
                        {
                            e.CancelDefault = true;
                            FWBS.OMS.UI.Windows.MessageBox.Show(inspector, Session.CurrentSession.Resources.GetMessage("MSGNOTALLOWEDIT", "You are not allowed to edit this message according to the Email Profiling settings.", ""));
                        }
                    }
                    e.Handled = true;
                }
                else if (command == "SETPROFILEDFOLDER" || command == "F_CONTEXTVIEWCLIENT" || command == "F_CONTEXTVIEWFILE" || command == "F_CONTEXTVIEWASSOCIATES")
                {
                    var folder = e.Context as MSOutlook.MAPIFolder;
                    if (folder != null)
                    {
                        var message = OmsApp.GetFolderMessage(folder, false);
                        switch (command)
                        {
                            case "SETPROFILEDFOLDER":
                                if (message == null)
                                {
                                    var file = FWBS.OMS.UI.Windows.Services.SelectFile(OmsApp.ActiveWindow);
                                    if (file != null)
                                        OmsApp.SetAsProfileFolder(folder, file, false);
                                }
                                break;

                            case "F_CONTEXTVIEWCLIENT":
                                if (message != null)
                                {
                                    long clientid = 0;
                                    if (!long.TryParse(OmsApp.GetDocVariable(message, FWBS.OMS.UI.Windows.OMSApp.CLIENT, String.Empty), out clientid))
                                        throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("NVLDCLNMB", "Invalid client Number", "").Text);

                                    FWBS.OMS.UI.Windows.Services.ShowClient(addin.OMSApplication.ActiveWindow, Client.GetClient(clientid), null);
                                }
                                break;

                            case "F_CONTEXTVIEWFILE":
                                if (message != null)
                                {
                                    long fileid = 0;
                                    if (!long.TryParse(OmsApp.GetDocVariable(message, FWBS.OMS.UI.Windows.OMSApp.FILE, String.Empty), out fileid))
                                        throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("NVLDFLNMB", "Invalid file Number", "").Text);

                                    FWBS.OMS.UI.Windows.Services.ShowFile(addin.OMSApplication.ActiveWindow, OMSFile.GetFile(fileid), null);
                                }
                                break;
                            case "F_CONTEXTVIEWASSOCIATES":
                                if (message != null)
                                {
                                    long fileid = 0;

                                    if (!long.TryParse(OmsApp.GetDocVariable(message, FWBS.OMS.UI.Windows.OMSApp.FILE, String.Empty), out fileid))
                                        throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("NVLDFLNMB", "Invalid file Number", "").Text);

                                    NavigateToAssociatesView(fileid);
                                }
                                break;
                        }
                    }

                    e.Handled = true;
                }
            }
            else if (commands.Length > 1 && commands[0].ToUpper() == "OMS")
            {
                string command = commands[1].ToUpper();
                if (command == "SEARCH")
                {
                    Common.Panes.BasePane.Create<Common.Panes.GlobalSearchPane>((Common.OfficeOMSAddin)sender, command, e.Context, false).Visible = true;
                    e.Handled = true;
                }
            }
        }

        private void Application_FolderContextMenuDisplay(Microsoft.Office.Core.CommandBar CommandBar, Microsoft.Office.Interop.Outlook.MAPIFolder Folder)
        {
            Microsoft.Office.Core.CommandBarControl last = null;

            if (!Session.CurrentSession.IsLoggedIn)
                return;

            var message = OmsApp.GetFolderMessage(Folder, false);

            if (message == null)
            {
                var btn = CreateFolderButton("SETPROFFOLDER", "Set As Profile Folder", CommandBar, makeprofilefolder_Click);
                btn.BeginGroup = last == null;
                btn.Tag = Folder.EntryID;
                last = btn;
                return;
            }

            var clientid = OmsApp.GetDocVariable(message, FWBS.OMS.UI.Windows.OMSApp.CLIENT, String.Empty);
            if (!string.IsNullOrEmpty(clientid))
            {
                var btn = CreateFolderButton("VIEWCLIENT", "View %CLIENT%", CommandBar, clientFolder_Click);
                btn.BeginGroup = last == null;
                btn.Tag = clientid;
                last = btn;
            }

            string fileid = OmsApp.GetDocVariable(message, FWBS.OMS.UI.Windows.OMSApp.FILE, String.Empty);
            if (!string.IsNullOrEmpty(fileid))
            {
                var btn = CreateFolderButton("VIEWFILE", "View %FILE%", CommandBar, fileItem_Click);
                btn.BeginGroup = last == null;
                btn.Tag = fileid;
                last = btn;
            }
        }

        private void makeprofilefolder_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            Microsoft.Office.Interop.Outlook.MAPIFolder folder = WrappedApplication.Session.GetFolderFromID(Ctrl.Tag, null);

            var file = FWBS.OMS.UI.Windows.Services.SelectFile(OmsApp.ActiveWindow);
            if (file != null)
                OmsApp.SetAsProfileFolder(folder, file, false);

        }

        private void clientFolder_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            long clientid = 0;

            if (!long.TryParse(Ctrl.Tag, out clientid))
                throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("NVLDCLNMB", "Invalid client Number", "").Text);
            FWBS.OMS.UI.Windows.Services.ShowClient(addin.OMSApplication.ActiveWindow, Client.GetClient(clientid), null);
        }

        private void fileItem_Click(Microsoft.Office.Core.CommandBarButton Ctrl, ref bool CancelDefault)
        {
            long fileid = 0;

            if (!long.TryParse(Ctrl.Tag, out fileid))
                throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("NVLDFLNMB", "Invalid file Number", "").Text);

            FWBS.OMS.UI.Windows.Services.ShowFile(addin.OMSApplication.ActiveWindow, OMSFile.GetFile(fileid), null);
        }

        private void inspectors_NewInspector(Microsoft.Office.Interop.Outlook.Inspector Inspector)
        {
            var oi = Inspector as OutlookInspector;

            new InspectorWrapper(oi, addin);

            if (oi != null)
                addin.RefreshUI(false, oi.InternalObject);
        }



        private void addin_ControlResourceRequest(object sender, Fwbs.Oms.Office.Common.ControlResourceRequestEventArgs e)
        {
            switch (e.Id.ToUpperInvariant())
            {
                case "MICROSOFT.OUTLOOK.EXPLORER":
                    e.Resource = GetResourceText("Fwbs.Oms.Office.Outlook.DefaultMainRibbon.xml");
                    e.Config = GetResourceText("Fwbs.Oms.Office.Outlook.DefaultMainRibbonConfig.xml");
                    e.Commands = GetResourceText("Fwbs.Oms.Office.Outlook.RibbonCommands.xml");
                    break;
                default:
                    e.Resource = GetResourceText("Fwbs.Oms.Office.Outlook.DefaultItemRibbon.xml");
                    e.Config = GetResourceText("Fwbs.Oms.Office.Outlook.DefaultItemRibbonConfig.xml");
                    e.Commands = GetResourceText("Fwbs.Oms.Office.Outlook.RibbonItemCommands.xml");
                    break;
            }
        }

        private void addin_ControlLabelRequest(object sender, Fwbs.Oms.Office.Common.ControlStringCallbackEventArgs e)
        {

            CheckForHomeTabResource(e);
            if (e.Handled)
                return;

            if (e.Id.ToUpperInvariant() == "TABOMSDOCUMENT_VIEW_FILEINFO")
            {
                MSOutlook.Inspector insp = WrappedApplication.ActiveInspector();
                long fileid = addin.OMSApplication.GetDocVariable(insp.CurrentItem, FWBS.OMS.Interfaces.IOMSApp.FILE, 0);

                if (fileid != 0)
                {
                    try
                    {
                        OMSFile file = OMSFile.GetFile(fileid);
                        e.ReturnValue = String.Format("{0}\n{1}", file.FileNo, file.FileDescription);
                    }
                    catch
                    {
                        e.ReturnValue = "???";
                    }
                    e.Handled = true;
                    return;
                }
            }

            e.ReturnValue = Properties.Resources.ResourceManager.GetString(e.Id);
            if (String.IsNullOrEmpty(e.ReturnValue) == false)
                e.Handled = true;

        }

        private void CheckForHomeTabResource(Fwbs.Oms.Office.Common.ControlStringCallbackEventArgs e)
        {
            string id = e.Id;

            const string TabHome = "TabHome";

            var checks = new[]{"TABMAIL","TABCALENDARTABLEVIEW","TABCALENDAR","TABTASKS","TABCONTACTS"};

            foreach (var check in checks)
            {
                if (id.ToUpperInvariant().StartsWith(check))
                {
                    e.Handled = true;
                    id = TabHome + id.Substring(check.Length);
                    break;
                }
            }

            if (e.Handled)
                e.ReturnValue = Properties.Resources.ResourceManager.GetString(id);


            if (String.IsNullOrEmpty(e.ReturnValue))
                e.Handled = false;
        }

        private void addin_ControlSuperTipRequest(object sender, Fwbs.Oms.Office.Common.ControlStringCallbackEventArgs e)
        {
            e.ReturnValue = Properties.Resources.ResourceManager.GetString(String.Format("SuperTip_{0}", e.Id));
            if (String.IsNullOrEmpty(e.ReturnValue) == false)
                e.Handled = true;
        }

        private void addin_ControlVisibleRequest(object sender, Fwbs.Oms.Office.Common.ControlVisibleCallbackEventArgs e)
        {
            MSOutlook.Inspector insp = e.Context as MSOutlook.Inspector;
            if (insp != null)
            {               
                if (Session.CurrentSession.IsLoggedIn)
                {
                    insp = WrappedApplication.GetInspector(insp);

                    if (!OmsApp.CanSaveItemAsDocument(insp.CurrentItem))
                    {
                         e.ReturnValue = false;
                         e.Handled = true;
                        return;
                    }
                }

            }

            e.ReturnValue = false;
            e.Handled = false;

            if (String.IsNullOrEmpty(e.Filter))
                return;

            string[] filters = e.Filter.Split(';');

            foreach (string filt in filters)
            {
                switch (filt)
                {
                    case "AUTHORISE":
                        {
                            FWBS.OMS.DocumentManagement.Storage.IStorageItem item = ((FWBS.OMS.UI.Windows.Office.OutlookOMS)addin.OMSApplication).GetDocumentToAuthorise(addin.OMSApplication);
                            if (item != null)
                            {
                                e.ReturnValue = true;
                                break;
                            }

                            e.Handled = true;
                        }
                        break;
                    case "AUTHORISE+OUT":
                        {
                            e.ReturnValue = ((FWBS.OMS.UI.Windows.Office.OutlookOMS)addin.OMSApplication).OutwardAuthorisation(addin.OMSApplication);
                            e.Handled = true;
                        }
                        break;
                    case "AUTHORISE+RETURN":
                        {
                            e.ReturnValue = ((FWBS.OMS.UI.Windows.Office.OutlookOMS)addin.OMSApplication).ReturnedAuthorisation(addin.OMSApplication);
                            e.Handled = true;
                        }
                        break;
                    case "PROFILEDFOLDER":
                        {
                            string[] commands = e.Command.Split(';');
                            e.Handled = (commands.Length > 1 && commands[0].ToUpper() == "ADDIN") && (e.Context is MSOutlook.MAPIFolder);
                            if (e.Handled)
                            {
                                string command = commands[1].ToUpper();
                                var message = OmsApp.GetFolderMessage((MSOutlook.MAPIFolder)e.Context, false);
                                e.ReturnValue = (command == "SETPROFILEDFOLDER" && message == null) ||
                                    (command == "F_CONTEXTVIEWCLIENT" && message != null) ||
                                    (command == "F_CONTEXTVIEWFILE" && message != null) ||
                                    (command == "F_CONTEXTVIEWASSOCIATES" && message != null);
                            }
                        }
                        break;
                    case "DOC+CANSAVE":
                        {
                            MSOutlook.Selection selection = e.Context as MSOutlook.Selection;
                            if (selection != null && selection.Count == 1)
                            {
                                OutlookItem item = WrappedApplication.GetItem(selection[1], true);
                                try { e.ReturnValue = OmsApp.CanSaveItemAsDocument(item); }
                                finally { typeof(OutlookItem).GetProperty("IsPinned", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(item, false); }
                                e.Handled = true;
                            }
                        }
                        break;
                }
            }


        }


        #endregion

        #region VSTO generated code

        public override void BeginInit()
        {
            try
            {
                Type DpiHelperType = typeof(System.Windows.Forms.Control).Assembly.GetType("System.Windows.Forms.DpiHelper");
                DpiHelperType.GetField("dpiAwarenessValue", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, "PerMonitorV2");
                DpiHelperType.GetField("enableDpiChangedHighDpiImprovements", BindingFlags.Static | BindingFlags.NonPublic).SetValue(null, true);

                AppDomainSetup appDomainSetup = (AppDomainSetup)typeof(AppDomain).GetProperty("FusionStore", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(AppDomain.CurrentDomain);
                appDomainSetup.TargetFrameworkName = Assembly.GetExecutingAssembly().GetCustomAttribute<System.Runtime.Versioning.TargetFrameworkAttribute>().FrameworkName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                base.BeginInit();
            }
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        #endregion

        #region Properties

        internal bool DisableFolderSwitch
        {
            get
            {
                return ConvertDef.ToBoolean(new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, FWBS.OMS.Global.VersionKey, "Outlook", "DisableFolderSwitch").GetSetting(false), false);
            }
        }

        #endregion
    }
}
