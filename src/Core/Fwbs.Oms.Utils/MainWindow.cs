using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FWBS.OMS.Utils
{
    using Fwbs.Oms.DialogInterceptor;
    using Fwbs.WinFinder;
    using FWBS.Common;
    using FWBS.OMS.DocumentManagement;

    using Global = FWBS.OMS.Global;

    public partial class MainWindow : Form
    {
        FileInfo originalFile;
        FileInfo newFile;
        private FileSystemInfo lastChangedPDF;
        private OMSDocument lastChangedDocument;
        private FWBS.OMS.DocumentManagement.DocumentVersion lastChangedDocVersion;
        Dictionary<IntPtr, HelperFunctions.PDFWindowInfo> PDFWindowCollection = new Dictionary<IntPtr, HelperFunctions.PDFWindowInfo>();
        private static int attachDocPropMaxTimeOut = 5;
        private static int attachDocPropInterval = 1;
        private readonly Interfaces.IVirtualDrive _virtualDrive;

        private enum MonitoringStatus
        {
            Off,
            On
        }

        private MonitoringStatus MonitorSessionStatus;
        private MonitoringStatus MonitorSaveStatus;
        private MonitoringStatus MonitorSaveAsStatus;

        #region Constructors

        public MainWindow()
        {
            InitializeComponent();
            lstQueue.ItemHeight = LogicalToDeviceUnits(16);
            string mountPoint;
            if (Session.CurrentSession.IsVirtualDriveFeatureInstalled(out mountPoint))
            {
                _virtualDrive = HelperFunctions.CreateVirtualDriveInstance(this);
            }
        }

        #endregion

        #region Captured Events

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            try
            {
                mnuWindowPicker.Visible = Common.ConvertDef.ToBoolean(new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, "", @"DialogInterceptor", "Debug").GetSetting(false), false);
            }
            catch { }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            try
            {
                AttachSessionEvents();

                SetNotifyTitle();

                mnuMonitorSession.Image = Properties.Resources.Running.ToBitmap();

                if (IsMonitorSessionDisabled)
                    OnMonitorSessionOff();
                else
                    OnMonitorSessionOn(true);

                if (IsMonitorSaveDisabled)
                    OnMonitorSaveOff();
                else
                    OnMonitorSaveOn(true);

                if (IsMonitorSaveAsDisabled)
                    OnMonitorSaveAsOff();
                else
                    OnMonitorSaveAsOn(true);

                if (!IsNotificationsDisabled)
                {
                    mnuDisableNotifications.Image = Properties.Resources.Running.ToBitmap();
                }
                else
                {
                    mnuDisableNotifications.Image = Properties.Resources.Stop.ToBitmap();
                }

                SetState();

                this.Text = String.Format(this.Text, Global.ApplicationName);
                Hide();
                notifyIcon.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
                return;
            }

            try
            {
                notifyIcon.Visible = false;
                Disconnect();
                DetachSessionEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OMS_Disconnected(object sender, EventArgs e)
        {
            lock (Globals.Synch)
            {
                Globals.ProcessingCommand = null;
                Globals.ChangedDocuments.Clear();
                Globals.Commands.Clear();
                lstQueue.Items.Clear();
                Globals.Cancel = true;
            }
            TryUnmountVirtualDrive();
        }

        private void OMS_Connected(object sender, EventArgs e)
        {
            BeginInvoke(new Action(TryMountVirtualDrive));
            SetState();
        }     

        private void tmrRunCommand_Tick(object sender, EventArgs e)
        {
            tmrRunCommand.Enabled = false;

            if (Globals.IsBusy)
                return;

            try
            {
                Cursor = Cursors.WaitCursor;

                Globals.IsBusy = true;

                Commands.RunCommand cmd = Dequeue();

                while (cmd != null && Globals.Cancel == false)
                {
                    Globals.ProcessingCommand = cmd;
                    lstQueue.Refresh();

                    RunCommand(cmd);
                    lstQueue.Items.Remove(cmd);
                    cmd = Dequeue();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Globals.ProcessingCommand = null;
                Globals.IsBusy = false;
                Cursor = Cursors.Default;
            }
        }


        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            ActivateDisplay();
        }
        private void notifySave_DoubleClick(object sender, EventArgs e)
        {
            ActivateDisplay();
        }

        private void lstQueue_DpiChangedAfterParent(object sender, EventArgs e)
        {
            lstQueue.ItemHeight = LogicalToDeviceUnits(16);
        }

        private void lstQueue_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index > -1)
            {
                Commands.RunCommand cmd = (Commands.RunCommand)lstQueue.Items[e.Index];
                Brush brush = (cmd == Globals.ProcessingCommand) ? Brushes.Red : Brushes.Black;

                e.DrawBackground();
                e.Graphics.DrawString(cmd.ToString(), e.Font, brush, e.Bounds, StringFormat.GenericDefault);
                e.DrawFocusRectangle();
            }
        }

        private void MenuClick(object sender, EventArgs e)
        {
            try
            {
                if (sender == mnuExit)
                    RunCommand(new Commands.ExitCommand());
                else if (sender == mnuAbout)
                    RunCommand(new Commands.AboutCommand());
                else if (sender == mnuWindowPicker)
                {
                    WindowFactory.ShowWindowPicker(null);
                }
                else if (sender == mnuConnect)
                {
                    OnMonitorSessionOff();
                    mnuConnect.Enabled = false;
                    if (FWBS.OMS.UI.Windows.Services.CheckLogin())
                        SetState();
                    else
                        mnuConnect.Enabled = true;
                }
                else if (sender == mnuDisconnect)
                {
                    OnMonitorSessionOff();
                    RunCommand(new Commands.DisconnectCommand());
                }
                else if (sender == mnuShow)
                {
                    if (mnuShow.Checked)
                    {
                        Show();
                        ActivateDisplay();
                    }
                    else
                    {
                        Hide();
                    }
                }
                else if (sender == mnuRestart)
                    RunCommand(new Commands.RestartCommand());
                else if (sender == mnuMonitorSave)
                {
                    IsMonitorSaveDisabled = MonitorSaveStatus == MonitoringStatus.Off ? false : true;
                    ToggleMonitorSave();
                }
                else if (sender == mnuMonitorSaveAs)
                {
                    IsMonitorSaveAsDisabled = MonitorSaveAsStatus == MonitoringStatus.Off ? false : true;
                    ToggleMonitorSaveAs();
                }
                else if (sender == mnuMonitorSession)
                {
                    IsMonitorSessionDisabled = MonitorSessionStatus == MonitoringStatus.Off ? false : true;
                    ToggleMonitorSession();
                }
                else if (sender == mnuDisableNotifications)
                {
                    IsNotificationsDisabled = !IsNotificationsDisabled;
                    if (!IsNotificationsDisabled)
                    {
                        mnuDisableNotifications.Image = Properties.Resources.Running.ToBitmap();
                    }
                    else
                    {
                        mnuDisableNotifications.Image = Properties.Resources.Stop.ToBitmap();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tmrSession_Tick(object sender, EventArgs e)
        {
            if (!Globals.IsBusy)
            {
                try
                {
                    tmrSession.Enabled = false;

                    ConnectedClientInfo[] clients = Session.CurrentSession.GetConnectedClients();

                    if (clients.Length <= 0)
                    {
                        if (FWBS.OMS.UI.Windows.Services.MainWindow != null &&
                            FWBS.OMS.UI.Windows.Services.MainWindow.OwnedForms.Length > 0)
                            return;

                        if (Session.CurrentSession.IsLoggedIn) 
                            Disconnect();
                    }
                    else if (clients.Length == 1)
                    {
                        if (clients[0].ProcessId == System.Diagnostics.Process.GetCurrentProcess().Id)
                        {
                            if (FWBS.OMS.UI.Windows.Services.MainWindow != null &&
                                FWBS.OMS.UI.Windows.Services.MainWindow.OwnedForms.Length > 0)
                                return;

                            if (Session.CurrentSession.IsLoggedIn)
                                Disconnect();
                        }
                        else
                        {
                            if (!Session.CurrentSession.IsLoggedIn)
                                Connect();
                        }
                    }
                    else
                    {
                        if (!Session.CurrentSession.IsLoggedIn) 
                            Connect();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }
                finally
                {
                    tmrSession.Enabled = true;
                }
            }

        }

        void fileSystemWatcher1_Renamed(object sender, System.IO.RenamedEventArgs e)
        {
            if ((HelperFunctions.FileIsHiddenOrTemporary(new FileInfo(e.OldFullPath)) || e.OldFullPath.ToUpper().EndsWith(".TMP")) && e.FullPath.ToUpper().EndsWith(".PDF"))
            {
                System.Diagnostics.Debug.WriteLine("File Rename captured", "OMSUTILS");
                fileSystemWatcher1_Changed(sender, e);
            }                      
        }
                     
        private void fileSystemWatcher1_Changed(object sender, System.IO.FileSystemEventArgs e)
        {
            try
            {
                bool fileExists = File.Exists(e.FullPath);
                System.Diagnostics.Debug.WriteLine(string.Format("Changed event. File Exists? = {0} - {1}", fileExists, e.FullPath), "OMSUTILS");
                if (!fileExists)
                    return;

                if (e.FullPath.ToUpper().EndsWith(".TMP"))
                {
                    CheckActiveWindowForLocalPDF();
                    return;
                }
                
                string fullpath;
                string ext;
                System.IO.FileSystemInfo info;
                         
                
                HelperFunctions.SetLocalFileObjects(e, out fullpath, out ext, out info);
                fileSystemWatcher1.EnableRaisingEvents = false;
                
                
                if (HelperFunctions.ExtensionIsPDF(ext))
                {
                    SetLastChangedObjects(fullpath, info);
                    AddActiveWindowToPDFWindowCollection();
                }
                
           
                if (FWBS.OMS.Utils.HelperFunctions.FileIsHiddenOrTemporary(info))
                {
                    CheckActiveWindowForLocalPDF();

                    if (HelperFunctions.OMSDocumentIsNotNull(lastChangedDocument) && HelperFunctions.OMSDocumentIsPDF(lastChangedDocument))
                    {
                        if (HelperFunctions.OMSDocumentIsNotNull(lastChangedDocument))
                        {
                            SetLocalFileObjects(ref fullpath, ref ext, ref info);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }

                if (String.IsNullOrEmpty(ext) || Array.IndexOf(FileSystemWatcherExclusionList, ext) > -1)
                    return;

                System.Diagnostics.Debug.WriteLine("Document extension is valid", "OMSUTILS");

                lock (Globals.Synch)
                {
                    bool justSaved = Globals.JustSavedDocuments.ContainsKey(fullpath.ToUpperInvariant());

                    if (justSaved)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("JustSavedDocuments() : {0}", fullpath), "OMSUTILS");

                        DateTime dt = Globals.JustSavedDocuments[fullpath.ToUpperInvariant()];

                        ////UTCFIX: DM - 30/11/06 - Both dates utc so valid comparison.
                        TimeSpan ts = DateTime.UtcNow.Subtract(dt);
                        if (ts.Seconds <= 5)
                        {
                            return;
                        }
                        else
                        {
                            Globals.RemoveSavedDocument(fullpath);
                        }
                    }


                    if (!Globals.ChangedDocuments.Contains(fullpath))
                    {
                        if (FWBS.OMS.Apps.ApplicationManager.CurrentManager.GetRegisteredApplicationByExtension(ext) == null)
                        {
                            OMSDocument doc = FWBS.OMS.UI.Windows.OMSApp.GetDocument(new System.IO.FileInfo(fullpath));
                                                        
                            if (doc == null)
                            {
                                //Obtain Max Time Out and Interval values from the registry
                                attachDocPropMaxTimeOut = FWBS.Common.ConvertDef.ToInt32(new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, "", @"DialogInterceptor\Processes", "MaxTimeOut").GetSetting(5), 5);
                                attachDocPropInterval = FWBS.Common.ConvertDef.ToInt32(new FWBS.Common.Reg.ApplicationSetting(FWBS.OMS.Global.ApplicationKey, "", @"DialogInterceptor\Processes", "Interval").GetSetting(1), 1);

                                System.Diagnostics.Debug.WriteLine(string.Format("Attach Doc Prop : Max Time Out:{0}, Interval:{1}", attachDocPropMaxTimeOut, attachDocPropInterval), "OMSUTILS");

                                //CM 12-01-17: The source application may have wiped out MatterSphere metadata by this point
                                doc = AttachDocPropertiesFromLocalCacheXML(e, fullpath, doc);
   
                                //With a delay in the previous line it is possible that the window may be open by the time document properties have been attached.
                                //If the local document has just been saved (i.e. first time opened), we do not need to proceed any further.
                                if (justSaved)
                                {
                                    System.Diagnostics.Debug.WriteLine("Item just been saved - now return", "OMSUTILS");
                                    return;
                                }
                            }
                                                        
                            if (doc != null)
                            {                               
                                WindowList wl = WindowList.Find("", String.Format("{0}*", System.IO.Path.GetFileNameWithoutExtension(fullpath)));

                                if (wl.Count == 0 && _virtualDrive != null && _virtualDrive.IsMounted)
                                    wl = WindowList.Find("", String.Format("{0}*", HelperFunctions.DocDescriptionToFileName(doc.Description)));

                                if (wl.Count == 0)
                                    return;

                                Window parent = HelperFunctions.GetParentWindow(wl);

                                if (parent == null)
                                    return;

                                Globals.ChangedDocuments.Add(fullpath.ToUpperInvariant());

                                System.Diagnostics.Debug.WriteLine("Queue Document to be saved", "OMSUTILS");

                                Commands.SaveCommand savecmd = new Commands.SaveCommand();
                                savecmd.Parent = parent;
                                savecmd.Parent.IsEnabled = false;
                                savecmd.Param = fullpath;
                                Queue(savecmd);

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                fileSystemWatcher1.EnableRaisingEvents = true;
            }
        }

        #region "New Methods"

        /// <summary>
        /// Attach Document properties to a given file, based on it's entry in the Local Document Cache
        /// </summary>
        /// <param name="e"></param>
        /// <param name="fullpath"></param>
        /// <param name="doc"></param>
        /// <param name="counter"></param>
        /// <returns></returns>
        private OMSDocument AttachDocPropertiesFromLocalCacheXML(System.IO.FileSystemEventArgs e, string fullpath, OMSDocument doc, int counter = 0)
        {
            //Delay
            if (counter == 0)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Attach Doc Properties from Local Cache XML - START - Wait {0} seconds", attachDocPropInterval), "OMSUTILS");
                System.Threading.Thread.Sleep(attachDocPropInterval * 1000);
            }
            
            //Get Local Document Info as a DataTable                                    
            System.Data.DataTable dt = FWBS.OMS.DocumentManagement.Storage.StorageManager.CurrentManager.LocalDocuments.GetLocalDocumentInfo();

            if (dt != null && dt.Rows.Count > 0)
            {
                int docID = 0;

                try
                {
                    //Obtain Doc ID from Local Document Info, search by FullPath since LocalDocumentCache.xml is common to multiple DB.
                    docID = ConvertDef.ToInt32(dt.Select(string.Format("FileLocalPath = '{0}'", e.FullPath.Replace("'", "''")))[0]["docID"], -1);
                    System.Diagnostics.Debug.WriteLine(string.Format("DocID from Local Document Info:{0}", docID), "OMSUTILS");                    
                }
                catch (System.IndexOutOfRangeException ex)
                {
                    //If we cannot retrieve information from Local Document Info, it may mean that the localdocumentcache.xml file is locked.
                    //Here we trap the Exception that we expect and retry again, up until the maximum time out

                    System.Diagnostics.Debug.WriteLine(string.Format("Error: {0}", ex), "OMSUTILS");
                                        
                    if (counter < attachDocPropMaxTimeOut)
                    {
                        counter++;
                        System.Diagnostics.Debug.WriteLine(string.Format("Attach Doc Props - retry attempt {0} - Wait {1} seconds...", counter, attachDocPropInterval), "OMSUTILS");
                        System.Threading.Thread.Sleep(attachDocPropInterval * 1000);
                        return File.Exists(e.FullPath) ? AttachDocPropertiesFromLocalCacheXML(e, fullpath, doc, counter) : doc;
                    }
                    else
                    {
                        try
                        {
                            using (System.Diagnostics.EventLog evt = new System.Diagnostics.EventLog("Application", Environment.MachineName, "OMSDOTNET"))
                            {
                                string message = string.Format("OMSUTILS: Error opening file ({0}). Error: ", fullpath) + ex.Message;
                                evt.WriteEntry(message, System.Diagnostics.EventLogEntryType.Error);
                            }
                        }
                        catch (Exception ex2) 
                        { 
                            System.Diagnostics.Debug.WriteLine("Unable to add to Event log: " + ex2.Message, "OMSUTILS"); 
                        }
                                                
                        SetNotifyIconError();
                    }
                }
                
                //Create OMSDocument object based on docID obtained from Local Document Info object
                if (docID > 0)
                    doc = FWBS.OMS.OMSDocument.GetDocument(docID);
                                
                if (doc != null)
                {
                    //Get the document version
                    string verLabel = Convert.ToString(dt.Select(string.Format("FileLocalPath = '{0}'", e.FullPath.Replace("'", "''")))[0]["verLabel"]);
                    DocumentVersion docVersion = doc.GetVersion(verLabel) as DocumentVersion;

                    if (docVersion != null)
                    {
                        //Re-attach document properties to the local cached file
                        FWBS.OMS.Interfaces.IOMSApp.AttachDocumentProperties(new FileInfo(fullpath), doc, docVersion);
                        System.Diagnostics.Debug.WriteLine(string.Format("Attach Doc Props - Done. [{0}]", fullpath), "OMSUTILS");
                    }
                }
            }
            return doc;
        }


        private void SetLocalFileObjects(ref string fullpath, ref string ext, ref System.IO.FileSystemInfo info)
        {
            info = lastChangedPDF;
            fullpath = info.FullName;
            ext = info.Extension.Substring(1);
        }

        private void CheckActiveWindowForLocalPDF()
        {
            //Obtain PDF Info from the active window
            FWBS.OMS.Utils.HelperFunctions.PDFWindowInfo getPDFWindow = new FWBS.OMS.Utils.HelperFunctions.PDFWindowInfo();
            PDFWindowCollection.TryGetValue(FWBS.Common.Functions.GetActiveWindow(), out getPDFWindow);
                       
            //If entry does not exist. Add window to the PDF Window Collection
            if (PDFWindowCollection.Equals(null) || PDFWindowCollection.Count == 0)
            {
                AddActiveWindowToPDFWindowCollection();
            }
       
            if (getPDFWindow.FileSysInfo != null)
            {
                //Set last changed objects to be what we have in our PDF Window Collection
                lastChangedPDF = getPDFWindow.FileSysInfo;
                lastChangedDocument = getPDFWindow.OMSDoc;
                lastChangedDocVersion = getPDFWindow.OMSDocVersion;
            }
        }

        /// <summary>
        /// Adds an entry to the PDF Window Collection
        /// </summary>
        private void AddActiveWindowToPDFWindowCollection()
        {
            //WI.16103 - Required for Adobe Acrobat DC: It has been observed that the file name can change to be a temporary file depending on what Adobe commands 
            //have been used prior to a Save. Here we keep a collection of the original PDF per window. This collection then be referenced (if needed), if we save
            //from Adobe into MatterSphere and Adobe has renamed the original file and/or cleared metadata.
            System.Diagnostics.Debug.WriteLine("AddActiveWindowToWindowCollection()", "OMSUTILS");

            IntPtr winHandle = FWBS.Common.Functions.GetActiveWindow();
            HelperFunctions.PDFWindowInfo winInfo = new HelperFunctions.PDFWindowInfo(winHandle, lastChangedPDF, lastChangedDocument, lastChangedDocVersion);

            if (!PDFWindowCollection.ContainsKey(winHandle) && lastChangedDocument != null && lastChangedDocVersion != null)
            {
                System.Diagnostics.Debug.WriteLine("Add To PDF Window Collection", "OMSUTILS");
                PDFWindowCollection.Add(winHandle, winInfo);
            }
        }

        private void SetLastChangedObjects(string fullpath, System.IO.FileSystemInfo info)
        {
            lastChangedPDF = info;
            lastChangedDocument = FWBS.OMS.UI.Windows.OMSApp.GetDocument(new System.IO.FileInfo(fullpath));
            lastChangedDocVersion = FWBS.OMS.UI.Windows.OMSApp.GetDocumentVersion(new System.IO.FileInfo(fullpath));
        }

        #endregion
        
        private enum DialogPromptVal
        {
            Choice = 0,
            Normal = 1,
            Auto = 2,
        }

        private DialogPromptVal GetDialogInterceptorPrompt(string process)
        {
            process = process.ToUpperInvariant();
            if (!process.EndsWith(".EXE"))
                process += ".EXE";

            FWBS.Common.Reg.ApplicationSetting setting = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, "", @"DialogInterceptor\Dialogs\SaveAs", "Prompt");
            FWBS.Common.Reg.ApplicationSetting procsetting = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, "", String.Format(@"DialogInterceptor\Processes\{0}\SaveAs", process), "Prompt");

            return (DialogPromptVal)FWBS.Common.ConvertDef.ToInt32(procsetting.GetSetting(setting.GetSetting(0)), 0);

        }


        /// <summary>
        /// The standard save routine for OMS Utils
        /// </summary>
        /// <param name="sadlg"></param>
        private void OriginalSaveRoutine(SaveAsDialog sadlg)
        {
            Commands.SaveAsCommand savecmd = new Commands.SaveAsCommand();

            savecmd.FileName = sadlg.FileName;
            if (savecmd.FileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                savecmd.FileName = null;
            sadlg.FileName = Path.Combine(DocumentManagement.Storage.StorageManager.CurrentManager.LocalDocuments.LocalDocumentDirectory.FullName, Guid.NewGuid().ToString("B"));
            Globals.AddSavedDocument(sadlg.FileName);

            savecmd.Param = sadlg.FileName;
            sadlg.Ok();

            savecmd.Parent = sadlg.Parent;
            Queue(savecmd);
        }      
   

        private void Interceptor_DialogCaptured(object sender, DialogCapturedEventArgs e)
        {

            bool adobeSaveRoutineRan = false;

            try
            {
                Globals.IsBusy = true;
                SaveAsDialog sadlg = e.Dialog as SaveAsDialog;
                                                
                if (sadlg != null)
                {
                    DialogPromptVal prompt = GetDialogInterceptorPrompt(sadlg.Parent.Process.ProcessName);
                    DialogResult res = DialogResult.No;

                    switch (prompt)
                    {
                        case DialogPromptVal.Choice:
                            res = MessageBox.Show(sadlg.Parent, Session.CurrentSession.RegistryRes("SaveAsQuestion", String.Format("Would you like to save to {0}?", Global.ApplicationName)), Global.ApplicationName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                            System.Diagnostics.Debug.WriteLine("Interceptor_DialogCaptured");
                            break;
                        case DialogPromptVal.Normal:
                            res = DialogResult.No;
                            break;
                        case DialogPromptVal.Auto:
                            res = DialogResult.Yes;
                            break;
                    }

                    switch (res)
                    {
                        case DialogResult.No:
                            {
                                sadlg.Show();
                                break;
                            }
                        case DialogResult.Yes:
                            {
                                if (FWBS.OMS.UI.Windows.Services.CheckLogin(sadlg.Parent))
                                {
                                    System.Diagnostics.Debug.WriteLine("CheckLogin");
                                    string processName = sadlg.Parent.Process.ProcessName.ToUpper();
                                    if (processName.StartsWith("ACRORD") || processName.StartsWith("ACROBAT"))
                                    {
                                        if (!sadlg.FileName.EndsWith(".pdf", StringComparison.CurrentCultureIgnoreCase))
                                            sadlg.FileName += ".pdf";

                                        FileInfo f = new FileInfo(Path.Combine(DocumentManagement.Storage.StorageManager.CurrentManager.LocalDocuments.LocalDocumentDirectory.FullName, sadlg.FileName));

                                        //CM: Bug 3292 - Check required to see if pdf file exists in MatterSphere document storage before saving
                                        if (File.Exists(f.FullName))
                                        {
                                            if (FWBS.OMS.UI.Windows.OMSApp.GetDocument(f) == null)
                                                OriginalSaveRoutine(sadlg);
                                            else
                                            {
                                                System.Diagnostics.Debug.WriteLine("PDF is a 3E MatterSphere document");
                                                AdobeReaderSaveRoutine(sadlg);
                                                adobeSaveRoutineRan = true;
                                            }
                                        }
                                        else
                                            OriginalSaveRoutine(sadlg);
                                    }
                                    else
                                    {
                                        OriginalSaveRoutine(sadlg);
                                    }
                                }
                                else
                                    sadlg.Cancel();
                            }
                            break;
                        case DialogResult.Cancel:
                            sadlg.Cancel();
                            break;
                    }
                }
                else
                    e.Dialog.Show();



            }
            catch (Exception ex)
            {
                try
                {
                    if (e.Dialog.IsValid)
                        e.Dialog.Cancel();
                }
                catch (Exception ex2)
                {
                    if (InvokeRequired)
                        return;
                    MessageBox.Show(e.Dialog.Parent, ex2.Message, Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (InvokeRequired)
                    return;

                MessageBox.Show(e.Dialog.Parent, ex.Message, Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (adobeSaveRoutineRan)
                {
                    DeleteTempFile(originalFile, newFile);
                    System.Diagnostics.Debug.WriteLine("DeleteTemp - complete.");
                    originalFile = null;
                    newFile = null;
                }
                Globals.IsBusy = false;
            }
        }

        
        #endregion

        #region PDF Methods
               

        /// <summary>
        /// OMS Utils save routine for pdf files that already exist in MatterSphere
        /// </summary>
        /// <param name="sadlg"></param>
        private void AdobeReaderSaveRoutine(SaveAsDialog sadlg)
        {
            System.Diagnostics.Debug.WriteLine("AdobeReaderSaveRoutine");

            //CM 21-10-13. New save routine needed for Adobe Reader (WI 2797)
            Commands.SaveCommand savecmd = InitialiseAdobeSaveCommand(sadlg);
            DocumentManagement.DocumentVersion originalDocVersion;
            SetFilesAndDocVersion(sadlg, out originalFile, out originalDocVersion, out newFile);
            System.Diagnostics.Debug.WriteLine("SetFilesAndDocVersion - complete.");
            
            AddNewFileToSaveCommand(sadlg, savecmd, newFile);
            System.Diagnostics.Debug.WriteLine("AddNewFileToSaveCommand - complete.");
            
            AttachPDFDocumentProperties(originalDocVersion, newFile);
            System.Diagnostics.Debug.WriteLine("AttachPDFDocumentProperties - complete.");
            savecmd.Parent = sadlg.Parent;
            Queue(savecmd);
            System.Diagnostics.Debug.WriteLine("Queue - complete.");          
        }

        private static void AddNewFileToSaveCommand(SaveAsDialog sadlg, Commands.SaveCommand savecmd, FileInfo newFile)
        {
            sadlg.FileName = newFile.FullName;
            Globals.ChangedDocuments.Add(newFile.FullName.ToUpperInvariant());
            savecmd.Param = newFile.FullName;
            sadlg.Ok();
        }
        
        private void SetFilesAndDocVersion(SaveAsDialog sadlg, out FileInfo originalFile, out DocumentManagement.DocumentVersion originalDocVersion, out FileInfo newFile)
        {
            var tempFolderName = DocumentManagement.Storage.StorageManager.CurrentManager.LocalDocuments.LocalDocumentDirectory.FullName;
            System.Diagnostics.Debug.WriteLine(string.Format("LocalDocumentDirectory.FullName:{0}", DocumentManagement.Storage.StorageManager.CurrentManager.LocalDocuments.LocalDocumentDirectory.FullName));
            System.Diagnostics.Debug.WriteLine(string.Format("sadlg.FileName:{0}, extension:{1}", sadlg.FileName, sadlg.Extension));
                        
            originalFile = new FileInfo(Path.Combine(tempFolderName, sadlg.FileName));
            System.Diagnostics.Debug.WriteLine(string.Format("originalFile:{0}.",originalFile));

            originalDocVersion = FWBS.OMS.UI.Windows.OMSApp.GetDocumentVersion(originalFile);
            newFile = new FileInfo(GenerateTempFilename(originalDocVersion, tempFolderName, ".pdf"));

            System.Diagnostics.Debug.WriteLine(string.Format("originalFile:{0},  newFile:{1}, tempFolderName:{2}, originalDocVersion:{3}", originalFile.FullName, newFile.FullName, tempFolderName, originalDocVersion.Name));
        }

        private static Commands.SaveCommand InitialiseAdobeSaveCommand(SaveAsDialog sadlg)
        {
            Commands.SaveCommand savecmd = new Commands.SaveCommand();

            //CM-07.11.13 To fix an issue with the Adobe Reader SaveAs Dialog found on Windows 7 (WI:2901)
            if (!sadlg.FileName.EndsWith(".pdf", StringComparison.CurrentCultureIgnoreCase))
                sadlg.FileName += ".pdf";

            savecmd.FileName = sadlg.FileName;
            System.Diagnostics.Debug.WriteLine(string.Format("Init: sadlg.FileName = {0}", sadlg.FileName));
            return savecmd;
        }

        private static void AttachPDFDocumentProperties(DocumentManagement.DocumentVersion originalDocVersion, FileInfo newFile)
        {
            if (originalDocVersion != null)
            {
                System.Threading.Thread.Sleep(500); //0.5s
                FWBS.OMS.UI.Windows.OMSApp.AttachDocumentProperties(newFile, originalDocVersion.ParentDocument, originalDocVersion);
                System.Diagnostics.Debug.WriteLine(string.Format("originalDocVersion.ParentDocument:{0}", originalDocVersion.ParentDocument.Description));
            }
        }

        private static void DeleteTempFile(FileInfo originalFile, FileInfo newFile)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Delete Temp : originalFile:{0}, newFile:{1}", originalFile.FullName, newFile.FullName));

            try
            {
                if (!originalFile.FullName.Equals(newFile.FullName, StringComparison.CurrentCultureIgnoreCase))
                    File.Delete(originalFile.FullName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Generates a user friendly temporary file name
        /// </summary>
        /// <param name="OriginalDocument"></param>
        /// <param name="TempFolderName"></param>
        /// <param name="Extension"></param>
        /// <returns></returns>
        private string GenerateTempFilename(DocumentManagement.DocumentVersion OriginalDocumentVersion, string TempFolderName, string Extension)
        {
            string filename = TempFilename(OriginalDocumentVersion);
            filename = GetFilename(TempFolderName, Extension, filename);
            return filename;
        }

        private static string GetFilename(string TempFolderName, string Extension, string filename)
        {
            if (File.Exists(Path.Combine(TempFolderName, filename + Extension)))
                filename = FilenameWithSuffix(TempFolderName, Extension, filename);
            else
                filename = Path.Combine(TempFolderName, filename + Extension);
            return filename;
        }

        private static string FilenameWithSuffix(string TempFolderName, string Extension, string filename)
        {
            int x = 0;
            do
            {
                x++;
            }
            while (File.Exists(Path.Combine(TempFolderName, string.Format("{0} ({1})", filename, x) + Extension)));

            filename = Path.Combine(TempFolderName, string.Format("{0} ({1})", filename, x) + Extension);
            return filename;
        }

        private static string TempFilename(DocumentManagement.DocumentVersion OriginalDocumentVersion)
        {
            string filename = "";
            filename = string.Format("{0} ({1})", OriginalDocumentVersion.ParentDocument.Description, OriginalDocumentVersion.DisplayID);
            FWBS.Common.FilePath.ExtractInvalidChars(filename);
            return filename;
        }

        #endregion
        
        #region Methods

        private void AttachSessionEvents()
        {
            DetachSessionEvents();
            
            Session.CurrentSession.LoggedIn += new EventHandler(OMS_Connected);
            Session.CurrentSession.LoggedOff += new EventHandler(OMS_Disconnected);
            Session.CurrentSession.Connected += new EventHandler(OMS_Connected);
            Session.CurrentSession.Disconnected += new EventHandler(OMS_Disconnected);
        }

        private void DetachSessionEvents()
        {
            Session.CurrentSession.LoggedIn -= new EventHandler(OMS_Connected);
            Session.CurrentSession.LoggedOff -= new EventHandler(OMS_Disconnected);
            Session.CurrentSession.Connected -= new EventHandler(OMS_Connected);
            Session.CurrentSession.Disconnected -= new EventHandler(OMS_Disconnected);
        }

        internal void Disconnect()
        {
            Session.CurrentSession.Disconnect();

            lock (Globals.Synch)
            {
                Globals.ProcessingCommand = null;
                Globals.ChangedDocuments.Clear();
                Globals.Commands.Clear();
                lstQueue.Items.Clear();
                Globals.Cancel = true;
            }

            SetState();

        }

        internal void Connect()
        {
            try
            {
                if (!Session.CurrentSession.IsLoggedIn)
                {
                    Session.CurrentSession.Connect();
                }
            }
            catch { }

            SetState();
        }

        private Common.Reg.ApplicationSetting regsessiondisabled = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, @"", "DisableMonitorSession");
        private bool IsMonitorSessionDisabled
        {
            get
            {
                return Common.ConvertDef.ToBoolean(regsessiondisabled.GetSetting(false), false);
            }
            set
            {
                regsessiondisabled.SetSetting(value);
            }
        }


        private Common.Reg.ApplicationSetting regnotifywindowdisabled = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, @"", "DisableNotifications");
        private bool IsNotificationsDisabled
        {
            get
            {
                return Common.ConvertDef.ToBoolean(regnotifywindowdisabled.GetSetting(false), false);
            }
            set
            {
                regnotifywindowdisabled.SetSetting(value);
            }
        }

        private Common.Reg.ApplicationSetting regexclusion  = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, @"", "MonitorSaveExclusionList");
        private string[] FileSystemWatcherExclusionList
        {
            get
            {
                string[] vals = Convert.ToString(regexclusion.GetSetting("TMP;DOCX;DOC;XLS;XLSX;MSG;OLF;LPD;LDD;XFD")).ToUpperInvariant().Split(';');
                Array.Sort<string>(vals);
                return vals;
            }
        }

        private void ToggleMonitorSession()
        {
            if (MonitorSessionStatus == MonitoringStatus.Off)
                OnMonitorSessionOn(false);
            else if (MonitorSessionStatus == MonitoringStatus.On)
                OnMonitorSessionOff();
        }

        
        private void OnMonitorSessionOn(bool silent)
        {
            try
            {
                if (MonitorSessionStatus == MonitoringStatus.Off || !tmrSession.Enabled)
                    tmrSession.Enabled = true;
                mnuMonitorSession.Image = Properties.Resources.Running.ToBitmap();
                MonitorSessionStatus = MonitoringStatus.On;
            }
            catch
            {
                OnMonitorSessionOff();
                if (!silent)
                    throw;
            }
        }

        private void OnMonitorSessionOff()
        {
            if (MonitorSessionStatus == MonitoringStatus.On || tmrSession.Enabled)
                tmrSession.Enabled = false;
            mnuMonitorSession.Image = Properties.Resources.Stop.ToBitmap();
            MonitorSessionStatus = MonitoringStatus.Off;
        }

        private Common.Reg.ApplicationSetting regsavedisabled = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, @"", "DisableMonitorSave");
        private bool IsMonitorSaveDisabled
        {
            get
            {
                return Common.ConvertDef.ToBoolean(regsavedisabled.GetSetting(false), false);
            }
            set
            {
                regsavedisabled.SetSetting(value);
            }
        }

        private void ToggleMonitorSave()
        {
            if (MonitorSaveStatus == MonitoringStatus.Off)
                OnMonitorSaveOn(false);
            else if (MonitorSaveStatus == MonitoringStatus.On)
                OnMonitorSaveOff();
        }

        private void OnMonitorSaveOn(bool silent)
        {
            try
            {
                if (string.IsNullOrEmpty(fileSystemWatcher1.Path))
                    silent = true;
                if (MonitorSaveStatus == MonitoringStatus.Off || !fileSystemWatcher1.EnableRaisingEvents)
                    fileSystemWatcher1.EnableRaisingEvents = true;
                mnuMonitorSave.Image = Properties.Resources.Running.ToBitmap();
                MonitorSaveStatus = MonitoringStatus.On;
            }
            catch
            {
                OnMonitorSaveOff();
                if (!silent)
                    throw;
            }
        }

        private void OnMonitorSaveOff()
        {
            if (MonitorSaveStatus == MonitoringStatus.On || fileSystemWatcher1.EnableRaisingEvents)
                fileSystemWatcher1.EnableRaisingEvents = false;
            mnuMonitorSave.Image = Properties.Resources.Stop.ToBitmap();
            MonitorSaveStatus = MonitoringStatus.Off;
        }

        private Common.Reg.ApplicationSetting regsaveasdisabled = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, @"", "DisableMonitorSaveAs");
        private bool IsMonitorSaveAsDisabled
        {
            get
            {
                return Common.ConvertDef.ToBoolean(regsaveasdisabled.GetSetting(true), true);
            }
            set
            {
                regsaveasdisabled.SetSetting(value);
            }
        }

        private void ToggleMonitorSaveAs()
        {
            if (MonitorSaveAsStatus == MonitoringStatus.Off)
                OnMonitorSaveAsOn(false);
            else if (MonitorSaveAsStatus == MonitoringStatus.On)
                OnMonitorSaveAsOff();
        }

        private void OnMonitorSaveAsOn(bool silent)
        {
            try
            {
                if (MonitorSaveAsStatus == MonitoringStatus.Off || !Interceptor.IsHooked)
                    Interceptor.DialogCaptured += new DialogCapturedEventHandler(Interceptor_DialogCaptured);
                mnuMonitorSaveAs.Image = Properties.Resources.Running.ToBitmap();
                MonitorSaveAsStatus = MonitoringStatus.On;
            }
            catch
            {
                OnMonitorSaveAsOff();
                if (!silent)
                    throw;
            }
        }

        private void OnMonitorSaveAsOff()
        {
            if (MonitorSaveAsStatus == MonitoringStatus.On || Interceptor.IsHooked)
                Interceptor.DialogCaptured -= new DialogCapturedEventHandler(Interceptor_DialogCaptured);
            mnuMonitorSaveAs.Image = Properties.Resources.Stop.ToBitmap();           
            MonitorSaveAsStatus = MonitoringStatus.Off;
        }

        private void SetState()
        {
            Globals.Cancel = false;
            if (Session.CurrentSession.IsLoggedIn)
            {
                fileSystemWatcher1.InternalBufferSize = 16384; //8192;//4096;
                fileSystemWatcher1.Path = FWBS.OMS.DocumentManagement.Storage.StorageManager.CurrentManager.LocalDocuments.LocalDocumentDirectory.FullName;
                fileSystemWatcher1.EnableRaisingEvents = (MonitorSaveStatus == MonitoringStatus.On);

                if (stripDisconnected.Visible || stripServer.Text != Session.CurrentSession.CurrentDatabase.Server || stripDatabase.Text != Session.CurrentSession.CurrentDatabase.DatabaseName)
                {
                    stripDisconnected.Visible = false;
                    stripServer.Text = Session.CurrentSession.CurrentDatabase.Server;
                    stripDatabase.Text = Session.CurrentSession.CurrentDatabase.DatabaseName;
                    mnuConnect.Visible = mnuConnect.Enabled = false;
                    mnuDisconnect.Visible = mnuDisconnect.Enabled = true;
                    SetNotifyTitle();
                    if (!IsNotificationsDisabled)
                    {
                        notifyIcon.ShowBalloonTip(50000);
                    }
                    mnuAbout.Visible = true;
                }
            }
            else
            {

                //Cannot do this when disconnected.
                fileSystemWatcher1.EnableRaisingEvents = false;

                if (mnuDisconnect.Enabled)
                {
                    SetNotifyTitle();
                    if (!IsNotificationsDisabled)
                    {
                        notifyIcon.ShowBalloonTip(50000);
                    }

                    stripServer.Text = String.Empty;
                    stripDatabase.Text = String.Empty;
                    stripDisconnected.Visible = true;
                    mnuConnect.Visible = mnuConnect.Enabled = true;
                    mnuDisconnect.Visible = mnuDisconnect.Enabled = false;

                    mnuAbout.Visible = false;
                }
            }
        }

        public void RunCommand(string[] args)
        {

            try
            {

                if (args.Length >= 1)
                {
                    string[] param = ExtractParameters(args);                   

                    if (param == null || param.Length == 0 || param[0] == "")
                    {
                        return;
                    }

                    Commands.RunCommand cmd = null;
                    switch (param[0].ToUpper())
                    {
                        case "OPEN":
                            cmd = new Commands.OpenCommand();

                            if (param.Length >= 2)
                                cmd.Param = param[1];

                            if (param.Length >= 3)
                                cmd.Param2 = param[2];

                            cmd.Execute(this);
                            return;
                        case "PRINT":
                            cmd = new Commands.PrintCommand();
                            break;
                        case "SAVE":
                        case "-SAVE":
                            {
                                FWBS.Common.Reg.ApplicationSetting dbver = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "", "DisableBulkImport");
                                Commands.BulkSaveCommand bulk = new Commands.BulkSaveCommand();
                                if (param.Length > 2 && Convert.ToBoolean(dbver.GetSetting(false)) == false)
                                    bulk.ImportIndividual = true;
                                if (param.Length > 1)
                                {
                                    for (int ctr = 1; ctr < param.Length; ctr++)
                                    {
                                        bulk.Files.Add(param[ctr]);
                                    }
                                }
                                cmd = bulk;
                            }
                            break;
                        case "SAVEAS":
                            {
                                FWBS.Common.Reg.ApplicationSetting dbver = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "", "DisableBulkImport");
                                Commands.BulkSaveAsCommand bulk = new Commands.BulkSaveAsCommand();
                                if (param.Length > 2 && Convert.ToBoolean(dbver.GetSetting(false)) == false)
                                    bulk.ImportIndividual = true;
                                if (param.Length > 1)
                                {
                                    for (int ctr = 1; ctr < param.Length; ctr++)
                                    {
                                        bulk.Files.Add(param[ctr]);
                                    }
                                }
                                cmd = bulk;
                                }
                            break;
                        case "SAVEDOC":
                            cmd = new Commands.SaveCommand();
                            break;
                        case "SAVEASDOC":
                            cmd = new Commands.SaveAsCommand();
                            break;
                        case "ABOUT":
                            cmd = new Commands.AboutCommand();
                            break;
                        case "COMMANDCENTRE":
                            cmd = new Commands.CommandCentreCommand();
                            break;
                        case "OPENDOCUMENT":
                            cmd = new Commands.OpenDocumentCommand();
                            break;
                        case "OPENDOCUMENTEXT":
                        case "OPENDOCUMENTEXTERNAL":
                            cmd = new Commands.OpenDocumentExternalCommand();
                            break;
                        case "OPENLOCALDOCUMENT":
                            cmd = new Commands.OpenLocalDocumentCommand();
                            break;
                        case "STARTTEMPLATE":
                            cmd = new Commands.TemplateStartCommand();
                            break;
                        case "STARTPREC":
                            cmd = new Commands.TemplateStartExCommand();
                            break;
                        case "VIEWCLIENT":
                            cmd = new Commands.ViewClientCommand();
                            break;
                        case "VIEWFILE" :
                        case "VIEWMATTER":
                            cmd = new Commands.ViewFileCommand();
                            break;
                        case "VIEWFILEID":
                        case "VIEWMATTERID":
                            cmd = new Commands.ViewFileIDCommand();
                            break;
                        case "VIEWREPORT":
                            cmd = new Commands.ViewReportCommand();
                            break;
                        case "VIEWCONTACT":
                            cmd = new Commands.ViewContactCommand();
                            break;
                        case "CREATECLIENT":
                            cmd = new Commands.CreateClientCommand();
                            break;
                        case "CREATEFILE":
                        case "CREATEMATTER":
                            cmd = new Commands.CreateFileCommand();
                            break;
                    }

                    if (cmd == null)
                        throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("COMMNOTSUPP", "Command Not Supported", "").Text);

                    if (param.Length >= 2)
                        cmd.Param = param[1];
                    
                    if (param.Length >= 3)
                        cmd.Param2 = param[2];

                    if (cmd != null)
                    {
                        Queue(cmd);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Global.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string[] ExtractParameters(string[] args)
        {
            FWBS.Common.Reg.ApplicationSetting dbver = new FWBS.Common.Reg.ApplicationSetting(Global.ApplicationKey, Global.VersionKey, "", "UseV1ParameterExtraction");

            bool useV1 = Convert.ToBoolean(dbver.GetSetting(false));
            
            if (useV1)
                return ExtractParametersV1(args);
                
            return ExtractParametersV2(args);
        }

        /// <summary>
        /// Extracts Parameters from args
        /// Note this method doesn't deal with spaces in arguments when the arguments are passed from from a url (omsmc:saveas "filename")
        /// It has been left in the code just in case there is a situation that hasn't been considered and it needs to be reverted to
        /// </summary>
        private string[] ExtractParametersV1(string[] args)
        {
            string[] param = null;
            try
            {
                if (args[0].ToUpper().Contains(":"))
                {
                    args[0] = args[0].Remove(0, args[0].IndexOf(":") + 1);
                    args[0] = args[0].Replace("%20", " ");
                    param = args[0].Split(' ');
                }
                else
                {
                    param = args;
                }
            }
            catch { param = null; }

            return param;
            
        }

        /// <summary>
        /// Extracts parameters from arguments
        /// Deals with arguments that are passed from a url when an argument contains spaces (eg omsmc:saveas "filename with spaces that are replaced with %20s")
        /// If the url is sent from IE then it will automatically decode %20s back to spaces therefore the user must use %2520 
        /// </summary>
        private string[] ExtractParametersV2(string[] args)
        {
            try
            {
                string ProtocolMoniker = "OMSMC:";

                if (args[0].ToUpper().StartsWith(ProtocolMoniker))
                {
                    //strips the protocol moniker from the string
                    args[0] = args[0].Substring(ProtocolMoniker.Length);
                    
                    if (!args[0].ToUpper().StartsWith("SAVE") & !args[0].ToUpper().StartsWith("-SAVE"))
                        args[0] = args[0].Replace("%20", " ");

                    //replace the space between the command and file name with a ;
                    args[0] = ReplaceFirstInstanceOfCharacter(args[0], " ", ";");
                    args = args[0].Split(';'); //assumption here that the args only contain 1 entry
                }
                
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = args[i].Replace("%2520", " "); //if the url is sent from IE then it will decode %20s to spaces therefore the user must use %2520. Since they might want to use same syntax from command line as the url.
                    args[i] = args[i].Replace("%20", " ");
                }

                return args;

            }
            catch { return null; }
        }


        
        private string ReplaceFirstInstanceOfCharacter(string originaltext, string tobereplaced, string replacementchar)
        {
            int index = originaltext.IndexOf(tobereplaced);
            if (index < 0)
            {
                return originaltext;
            }
            return originaltext.Substring(0, index) + replacementchar + originaltext.Substring(index + tobereplaced.Length);
        }
 

        private void RunCommand(Commands.RunCommand command)
        {
            try
            {
                notifyIcon.Icon = Properties.Resources.Busy;
                
                
                bool okay = false;
                if (command.RequiresLogin)
                    okay = FWBS.OMS.UI.Windows.Services.CheckLogin();
                else
                    okay = true;

                if (okay)
                {
                    command.OnBusy(notifyIcon);
                    command.Execute(this);
                }
            }
            finally
            {
                HideBusyNotification();
            }
        }



      
        public void ActivateDisplay()
        {
            Commands.FileCommand filecmd = Globals.ProcessingCommand as Commands.FileCommand;
            if (filecmd != null)
                ActivateWindow(filecmd.Param);
            else
                ActivateWindow(null);
        }

       
        public Window ActivateWindow(string file)
        {
            this.TopMost = true;
            this.BringToFront();
            this.Activate();

            if (!String.IsNullOrEmpty(file))
            {
                WindowList wl = CommonWindows.Desktop.Find(new WindowFilter("", System.IO.Path.GetFileNameWithoutExtension(file)));
                if (wl.Count > 0)
                {
                    Window parent = null;

                    foreach (Window w in wl)
                    {
                        if (w.IsVisible && w.IsValid && w.IsHung == false && w.Parent == null)
                        {
                            parent = w;
                            break;
                        }
                    }

                    if (parent != null)
                        return parent;
                }
            }          

            return WindowFactory.GetWindow(this.Handle);
        }
         

        new public void Hide()
        {
            this.Top = Screen.PrimaryScreen.WorkingArea.Top - this.Bottom;
            this.Left = Screen.PrimaryScreen.WorkingArea.Left - this.Right;
            mnuShow.Checked = false;
        }

        new public void Show()
        {
            Window wintb = CommonWindows.TaskBar;
            if (wintb != null)
            {
                Rectangle tbb = wintb.Bounds;
                this.Location = new Point(tbb.Right - Width, tbb.Top - Height);
            }
            base.Show();
            mnuShow.Checked = true;
        }


        private void HideBusyNotification()
        {
            notifyIcon.Icon = (Session.CurrentSession.IsLoggedIn ? Properties.Resources.Running : Properties.Resources.Stop);
            SetNotifyTitle();
        }

        private void SetNotifyIconError()
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                string title = Session.CurrentSession.Resources.GetMessage("NTFYERR_TITLE", "Error", "").Text;
                string text = Session.CurrentSession.Resources.GetMessage("NTFYERR_TEXT", "Unable to open the selected document", "").Text;

                notifyIcon.Icon = Properties.Resources.Stop;
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.BalloonTipTitle = String.Format("{0} - {1}", Global.ApplicationName, title);
                notifyIcon.BalloonTipText = String.Format(text);
                string message = notifyIcon.BalloonTipTitle;
                if (message.Length > 63)
                    message = message.Substring(0, 63);
                notifyIcon.Text = message;

                if (!IsNotificationsDisabled)
                {
                    notifyIcon.ShowBalloonTip(50000);
                }
            }
        }


        private void SetNotifyTitle()
        {
            if (Session.CurrentSession.IsLoggedIn)
            {
                notifyIcon.Icon = Properties.Resources.Running;
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.BalloonTipTitle = String.Format("{0} - {1}", Global.ApplicationName, Session.CurrentSession.Resources.GetResource("Connected", "Connected", "").Text);
                notifyIcon.BalloonTipText = String.Format(Session.CurrentSession.Resources.GetMessage("MSGCONNECTTODB", "Connected to {0}.{1}", "").Text, stripServer.Text, stripDatabase.Text);
                notifyIcon.Tag = Session.CurrentSession.Resources.GetMessage("MSGDSCNCTFROMDB", "Disconnected from {0}.{1}", "").Text;
                string message = String.Format("{0} - {1}", Global.ApplicationName, notifyIcon.BalloonTipText);
                if(message.Length>63)
                    message = message.Substring(0,63);
                notifyIcon.Text = message;
            }
            else
            {
                notifyIcon.Icon = Properties.Resources.Stop;
                notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon.BalloonTipTitle = String.Format("{0} - {1}", Global.ApplicationName, stripDisconnected.Text);
                notifyIcon.BalloonTipText = String.Format(notifyIcon.Tag as string ?? "Disconnected from {0}.{1}", stripServer.Text, stripDatabase.Text);
                string message = notifyIcon.BalloonTipTitle;
                if (message.Length > 63)
                    message = message.Substring(0, 63);
                notifyIcon.Text = message;
            }
        }

        #endregion

        #region Command Queue

        private void Queue(Commands.RunCommand cmd)
        {
            lstQueue.Items.Insert(0, cmd);
            Globals.Commands.Enqueue(cmd);

            tmrRunCommand.Enabled = true;
        }

        private Commands.RunCommand Dequeue()
        {
            if (Globals.Commands.Count == 0)
                return null;

            return Globals.Commands.Dequeue();
        }

        private Commands.RunCommand Peek()
        {
            return Globals.Commands.Peek();
        }

        #endregion

        #region Virtual Drive

        private void TryMountVirtualDrive()
        {
            string mountPoint;
            if (Session.CurrentSession.IsVirtualDriveFeatureInstalled(out mountPoint) && _virtualDrive != null && !_virtualDrive.IsMounted)
            {
                if (!string.IsNullOrWhiteSpace(mountPoint) && !backgroundWorker.IsBusy)
                    backgroundWorker.RunWorkerAsync(mountPoint);
            }
        }

        private void TryUnmountVirtualDrive()
        {
            if (_virtualDrive != null && (_virtualDrive.IsMounted || backgroundWorker.IsBusy))
                _virtualDrive.Unmount();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // Don't use Session.CurrentSession.CurrentConnection to avoid overhead of ConnectionWrapper's ValidateCaller.
            var connection = typeof(Session).GetProperty("Connection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(Session.CurrentSession);
            _virtualDrive.Mount((string)e.Argument, connection as Data.Connection);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    using (System.Diagnostics.EventLog evt = new System.Diagnostics.EventLog("Application", Environment.MachineName, "OMSDOTNET"))
                    {
                        evt.WriteEntry($"OMSUTILS: [virtual drive error] {e.Error.Message}", System.Diagnostics.EventLogEntryType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Unable to add to Event log: " + ex.Message, "OMSUTILS");
            }
        }

        #endregion
    }
}
