using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FWBS.OMS.DocumentManagement.Storage;
using FWBS.OMS.EnquiryEngine;

namespace FWBS.OMS.UI.Windows
{
    /// <summary>
    /// Windows UI application controlling object.  This static object holds current state information
    /// of the OMS login.  It also runs commonly used forms and wizards.
    /// </summary>
    sealed public partial class Services
    {
        #region DLLIMPORTS


        [DllImport("tapi32.dll")]
        public static extern int tapiRequestMakeCall(string stNumber, string applicationname, string called, string comment);


        #endregion

        #region Events

        public static event EventHandler OMSTypeRefresh = null;

        #endregion

        #region Fields
        /// <summary>
        /// Capture the Back & Forward Message Filter
        /// </summary>
        public static BackForwardMessageFilter BackForwardMouse = new BackForwardMessageFilter();

        /// <summary>
        /// A cached splash screen form.
        /// </summary>
        private static frmSplash _frmsplash = null;
        /// <summary>
        /// A cached precedent screen form.
        /// </summary>
        private static frmPrecedent _frmprec = null;

        /// <summary>
        /// An invisible form to use to show non dialog forms as an owner of Word for instance.
        /// </summary>
        private static Form _parent = null;

        /// <summary>
        /// The Progress Form
        /// </summary>
        private static frmProgress frmProgress1;

        /// <summary>
        /// Enables the Private Associate button to appear on the select associate and select default associate screens.
        /// </summary>
        public static bool AllowPrivateAssociate = false;

        #endregion

        #region Constructors

        /// <summary>
        /// Cannot create an instance of this class.
        /// </summary>
        private Services() { }

        /// <summary>
        /// Static constructor to assign session events.
        /// </summary>
        static Services()
        {
            AttachDelegates = true;
            System.Windows.Forms.Application.AddMessageFilter(BackForwardMouse);
            _parent = new Form();
            _parent.Left = 0;
            _parent.Top = 0;
        }

        #endregion

        #region Properties

        public static Form[] VisibleForms
        {
            get
            {
                List<Form> list = new List<Form>();
                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Visible)
                        list.Add(frm);
                }
                return list.ToArray();
            }
        }

        public static bool DisableVisibleFormsCheck
        {
            get;
            set;
        }

        public static Form MainWindow
        {
            get
            {
                return _parent;
            }
        }

        #endregion

        #region Event Methods

        /// <summary>
        /// Attaches the delegates to the services layer.
        /// </summary>
        public static bool AttachDelegates
        {
            set
            {
                FWBS.OMS.Session.CurrentSession.NotLoggedIn -= new EventHandler(OnNotLoggedIn);
                FWBS.OMS.Session.CurrentSession.Warning -= new FWBS.OMS.MessageEventHandler(OnWarning);
                FWBS.OMS.Session.CurrentSession.ShowException -= new FWBS.OMS.MessageEventHandler(OnShowException);
                FWBS.OMS.Session.CurrentSession.PasswordRequest -= new FWBS.OMS.PasswordRequestEventHandler(OnPasswordRequest);
                FWBS.OMS.Session.CurrentSession.Ask -= new FWBS.OMS.AskEventHandler(OnAsk);
                FWBS.OMS.Session.CurrentSession.Prompt -= new FWBS.OMS.PromptEventHandler(OnPrompt);
                FWBS.OMS.Session.CurrentSession.Disconnecting -= new System.ComponentModel.CancelEventHandler(OnDisconnecting);
                FWBS.OMS.Session.CurrentSession.Disconnecting -= new System.ComponentModel.CancelEventHandler(OnDisconnecting);
                FWBS.OMS.Session.CurrentSession.ShowSearch -= new ShowSearchEventHandler(ShowSearch);
                FWBS.OMS.Session.CurrentSession.ShowEnquiry -= new ShowEnquiryEventHandler(ShowEnquiry);
                FWBS.OMS.Session.CurrentSession.ShowWizard -= new ShowEnquiryEventHandler(ShowWizard);
                FWBS.OMS.Session.CurrentSession.Progress -= new FWBS.Common.ProgressEventHandler(OnProgress);
                FWBS.OMS.Session.CurrentSession.ShowExtendedData -= new ShowExtendedDataEventHandler(ShowExtendedData);
                FWBS.OMS.Session.CurrentSession.ConnectionError -= new FWBS.OMS.Data.ConnectionErrorEventHandler(CurrentSession_ConnectionError);
                FWBS.OMS.Session.CurrentSession.ShutdownRequest -= new EventHandler(CurrentSession_ShutdownRequest);

                if (value)
                {
                    FWBS.OMS.Session.CurrentSession.NotLoggedIn += new EventHandler(OnNotLoggedIn);
                    FWBS.OMS.Session.CurrentSession.Warning += new FWBS.OMS.MessageEventHandler(OnWarning);
                    FWBS.OMS.Session.CurrentSession.ShowException += new FWBS.OMS.MessageEventHandler(OnShowException);
                    FWBS.OMS.Session.CurrentSession.PasswordRequest += new FWBS.OMS.PasswordRequestEventHandler(OnPasswordRequest);
                    FWBS.OMS.Session.CurrentSession.Ask += new FWBS.OMS.AskEventHandler(OnAsk);
                    FWBS.OMS.Session.CurrentSession.Prompt += new FWBS.OMS.PromptEventHandler(OnPrompt);
                    FWBS.OMS.Session.CurrentSession.Disconnecting += new System.ComponentModel.CancelEventHandler(OnDisconnecting);
                    FWBS.OMS.Session.CurrentSession.ShowSearch += new ShowSearchEventHandler(ShowSearch);
                    FWBS.OMS.Session.CurrentSession.ShowEnquiry += new ShowEnquiryEventHandler(ShowEnquiry);
                    FWBS.OMS.Session.CurrentSession.ShowWizard += new ShowEnquiryEventHandler(ShowWizard);
                    FWBS.OMS.Session.CurrentSession.Progress += new FWBS.Common.ProgressEventHandler(OnProgress);
                    FWBS.OMS.Session.CurrentSession.ProgressStart += new EventHandler(OnProgressStart);
                    FWBS.OMS.Session.CurrentSession.ProgressFinished += new EventHandler(OnProgressFinished);
                    FWBS.OMS.Session.CurrentSession.ShowExtendedData += new ShowExtendedDataEventHandler(ShowExtendedData);
                    FWBS.OMS.Session.CurrentSession.ConnectionError += new FWBS.OMS.Data.ConnectionErrorEventHandler(CurrentSession_ConnectionError);
                    FWBS.OMS.Session.CurrentSession.ShutdownRequest += new EventHandler(CurrentSession_ShutdownRequest);

                }
            }
        }

        static void CurrentSession_ConnectionError(object sender, Data.ConnectionErrorEventArgs e)
        {
            if (!Session.CurrentSession.IsShuttingDown && !Session.CurrentSession.IsDisconnecting)
            {
                //Improve message, use reg, add passed through error message

                System.Text.StringBuilder message = new System.Text.StringBuilder(Session.CurrentSession.RegistryRes("ConnError", "Connection to the database has been lost. Would you like to try to reconnect? \nPressing No will log you out the system, and could result in data loss."));
                message.AppendLine();
                message.Append(e.ConnectionException.Message);

                if (System.Windows.Forms.MessageBox.Show(message.ToString(), FWBS.OMS.Global.ApplicationName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                    e.Cancel = false;
            }
            else
                e.Cancel = true;

        }

        static void CurrentSession_ShutdownRequest(object sender, EventArgs e)
        {
            for (int n = Application.OpenForms.Count - 1; n >= 0; n--)
            {
                Application.OpenForms[n].Close();
            }
        }


        public static void OnOMSTypeRefresh()
        {
            if (OMSTypeRefresh != null)
                OMSTypeRefresh(null, EventArgs.Empty);
        }


        /// <summary>
        /// Captures the sessions not logged in event so that an exception is NOT raise
        /// and that the log in form can be shown instead.
        /// </summary>
        /// <param name="sender">Session instance.</param>
        /// <param name="e">Empty event arguments.</param>
        private static void OnNotLoggedIn(object sender, EventArgs e)
        {
            CheckLogin();
        }

        /// <summary>
        /// Captures the warning in event so that an exception is NOT raised
        /// and that the message box can be shown instead.
        /// </summary>
        /// <param name="sender">Object instance that raised the warning.</param>
        /// 		/// <param name="e">Message event arguments.</param>
        private static void OnWarning(object sender, FWBS.OMS.MessageEventArgs e)
        {
            if (_frmsplash != null)
            {
                if (_frmsplash.Visible)
                    _frmsplash.Close();
            }
            MessageBox.Show(e.Message, sender.ToString(), MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Captures the Exception in event
        /// </summary>
        /// <param name="sender">Object instance that raised the warning.</param>
        /// 		/// <param name="e">Message event arguments.</param>
        private static void OnShowException(object sender, FWBS.OMS.MessageEventArgs e)
        {
            if (_frmsplash != null)
            {
                if (_frmsplash.Visible)
                    _frmsplash.Close();
            }
            ErrorBox.Show(e.Exception);
        }


        /// <summary>
        /// Captures the password request event so that there is a chance for the user to
        /// enter a password in for password protected object.
        /// </summary>
        /// <param name="sender">Object instance that raised the password request.</param>
        /// <param name="e">Password request event arguments.</param>
        private static void OnPasswordRequest(FWBS.Common.IPasswordProtected sender, System.ComponentModel.CancelEventArgs e)
        {
            if (CheckLogin())
            {
                using (frmPasswordRequest frm = new frmPasswordRequest(sender))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        e.Cancel = false;
                    else
                        e.Cancel = true;
                }
            }
        }

        public static bool ShowPasswordRequest(string Password, string Hint)
        {
            if (CheckLogin())
            {
                using (frmPasswordRequest frm = new frmPasswordRequest(Password, Hint))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        /// <summary>
        /// Captures a question being asked from the business layer.		
        /// </summary>
        private static void OnAsk(object sender, AskEventArgs e)
        {
            if (_frmsplash != null)
            {
                if (_frmsplash.Visible)
                    _frmsplash.Close();
            }

            if (CheckLogin())
            {
                MessageBoxDefaultButton def;
                if (e.Result == AskResult.Yes)
                    def = MessageBoxDefaultButton.Button1;
                else
                    def = MessageBoxDefaultButton.Button2;

                DialogResult res = MessageBox.Show(e.Message, null, MessageBoxButtons.YesNo, MessageBoxIcon.Question, def);
                if (res == DialogResult.Yes)
                    e.Result = AskResult.Yes;
                else
                    e.Result = AskResult.No;
                Application.DoEvents();
            }
        }

        //INFO: Temporary for prompting for associates, precedents and duplicate documents.
        /// <summary>
        /// Captures a question being asked from the business layer.		
        /// </summary>
        private static void OnPrompt(object sender, PromptEventArgs e)
        {
            if (_frmsplash != null)
            {
                if (_frmsplash.Visible)
                    _frmsplash.Close();
            }

            try
            {
                if (CheckLogin())
                {
                    if (e.Type == PromptType.Search)
                    {
                        switch (e.Code)
                        {
                            case "PICKASSOCIATE":
                                e.Result = Services.Searches.PickAssociate(null, sender as OMSFile, e.Filter[0].ToString(), e.Filter[1].ToString(), e.Message, false);
                                break;
                            default:
                                {

                                    if (e.Code == Session.CurrentSession.DefaultSystemSearchList(SystemSearchLists.DocumentDuplicates) ||
                                        e.Code == Session.CurrentSession.DefaultSystemSearchList(SystemSearchLists.DocumentDuplicatesExternal))
                                    {
                                        Common.KeyValueCollection pars = new Common.KeyValueCollection();
                                        pars.Add("DOCID", e.Filter[0]);
                                        Searches sch = new Searches();
                                        sch.AsType = false;
                                        sch.Code = e.Code;
                                        sch.Message = e.Message;
                                        sch.Parameters = pars;
                                        pars = sch.Show();
                                        if (pars == null || pars.Contains("DOCID") == false)
                                            e.Result = null;
                                        else
                                            e.Result = pars["DOCID"].Value;
                                    }

                                    else if (e.Code == Session.CurrentSession.DefaultSystemSearchList(SystemSearchLists.PrecedentFilter))
                                    {
                                        Common.KeyValueCollection pars = new Common.KeyValueCollection();
                                        pars.Add("TITLE", e.Filter[0]);
                                        pars.Add("LANGUAGE", e.Filter[1]);
                                        Searches sch = new Searches();
                                        sch.AsType = false;
                                        sch.Code = e.Code;
                                        sch.Message = e.Message;
                                        sch.Parameters = pars;
                                        sch.AutoSelect = true;
                                        pars = sch.Show();
                                        if (pars == null || pars.Contains("precid") == false)
                                            e.Result = null;
                                        else
                                            e.Result = pars["precid"].Value;
                                    }
                                }
                                break;

                        }


                    }
                }
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ex);
            }
        }

        private static void ShowSearch(object sender, ShowSearchEventArgs e)
        {
            Searches sch = new Searches(e.SearchList);
            sch.Message = e.Message;
            sch.HideButtons = true;
            sch.Show(null);
        }

        private static void ShowExtendedData(object sender, ShowExtendedDataEventArgs e)
        {
            ExtendedDataScreen scr = new ExtendedDataScreen(e.ExtendedCode, e.Object);
            object ret = scr.Show();
            if (ret == null)
                e.Cancel = true;
        }

        private static void ShowEnquiry(object sender, ShowEnquiryEventArgs e)
        {
            Screens scr = new Screens(e.Enquiry);
            object ret = scr.Show();
            e.ReturnObject = ret;
            if (ret == null)
                e.Cancel = true;
        }


        private static void ShowWizard(object sender, ShowEnquiryEventArgs e)
        {
            Wizards wiz = new Wizards(e.Enquiry);
            object ret = wiz.Show();
            e.ReturnObject = ret;
            if (ret == null)
                e.Cancel = true;
        }


        private static void OnDisconnecting(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Make sure no non dialog forms are open when disconnecting.
            if (_frmprec != null)
            {
                _frmprec.Close();
                _frmprec.Dispose();
                _frmprec = null;
            }

            if (_parent != null)
            {
                if (_parent.OwnedForms.Length > 0)
                {
                    e.Cancel = true;

                    Form frm = _parent.OwnedForms[0];

                    MessageBox.ShowInformation("OPENFORMS", "Please close all forms before disconnecting from the system.");
                    frm.Visible = true;
                    frm.Activate();
                    return;
                }
            }

        }

        #endregion

        #region Login Methods

        /// <summary>
        /// Gets the current OMS session.
        /// </summary>
        public static Session OMS
        {
            get
            {
                if (!Session.CurrentSession.IsLoggedIn)
                {
                    Login();
                }
                return Session.OMS;
            }
        }

        public static bool Login(System.Windows.Forms.IWin32Window owner)
        {
            return Login(owner, null);
        }

        /// <summary>
        /// Calls up the login form with a specified owner window.
        /// </summary>
        /// <param name="owner">Owner window that is used to stop the form from falling behind others.</param>
        ///<param name="exception">Pass through exception.</param>
        /// <returns>True if the login was successfull.</returns>
        public static bool Login(System.Windows.Forms.IWin32Window owner, Exception exception)
        {
            try
            {
                if (_frmprec != null)
                {
                    _frmprec.Dispose();
                    _frmprec = null;
                }

                if (_parent != null)
                {
                    _parent.Dispose();
                    _parent = null;
                    _parent = new Form();
                }

                //There was no dispose here to begin with!!
                using (frmLogin frm = new frmLogin())//;
                {
                    frm.ShowException(exception);
                    switch (frm.ShowDialog(owner))
                    {
                        case System.Windows.Forms.DialogResult.OK:
                            {
                                return true;
                            }
                        default:
                            return false;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorBox.Show(owner, ex);
                return false;
            }

        }

        /// <summary>
        /// Calls up the the Login form.
        /// </summary>
        /// <returns>True if the login was successfull.</returns>
        public static bool Login()
        {
            return Login(null, null);
        }




        /// <summary>
        /// Use this method on every call to make sure that the user is logged in.
        /// There is no point using any function if the user is not logged in.
        /// </summary>
        /// <returns>True if logged in.</returns>
        public static bool CheckLogin()
        {
            return CheckLogin(null);
        }

        public static bool CheckLogin(IWin32Window owner)
        {
            if (Session.CurrentSession.IsLoggedIn)
                return true;
            else
            {
                try
                {
                    Session.CurrentSession.Connect();
                    FWBS.OMS.UI.Windows.Services.CloseSplash();
                    return true;
                }
                catch (FWBS.OMS.Security.LoginException ex)
                {
                    if (Session.CurrentSession.IsLoggedIn)
                        return true;

                    FWBS.OMS.UI.Windows.Services.CloseSplash();

                    if (ex.HelpID == HelpIndexes.CannotConnectToSession)
                        return Login(owner, null);
                    else
                        return Login(owner, ex);
                }
                catch (Exception ex)
                {
                    if (Session.CurrentSession.IsLoggedIn)
                        return true;

                    FWBS.OMS.UI.Windows.Services.CloseSplash();
                    return Login(owner, ex);
                }
            }
        }


        #endregion

        #region Methods

        /// <summary>
        /// Run TAPI Service Call to Dial a Number
        /// </summary>
        /// <param name="user">The user object whose job list to view.</param>
        /// <returns>True if there are some changes to be made.</returns>
        public static bool DialNumber(string number, string name, string description)
        {
            int retval;
            // pick up the phone.
            if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("DIALNUM", "Please pickup your telephone and click Yes to dial : %1", number) == DialogResult.Yes)
            {
                try
                {
                    retval = tapiRequestMakeCall(number, "OMSMC", name, description);
                    return true;
                }
                catch
                {
                    return false;
                }


            }
            else
            {
                return false;
            }

        }

        public static bool DialNumber(string number)
        {
            return DialNumber(number, "", "");
        }


        public static void ProcessStart(string name, string arguments, Func<string, bool> validateInput = null)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(name)
            {
                Arguments = arguments
            };
            if (validateInput == null)
            {
                ProcessStart(psi);
            }
            else
            {
                ProcessStart(psi, validateInput(arguments));
            }            
        }

        public static void ProcessStart(System.Diagnostics.ProcessStartInfo start, bool inputParamsValidated = true)
        {
            string name = start.FileName;
            System.IO.FileInfo file = new System.IO.FileInfo(start.FileName);
            if (System.IO.File.Exists(name))
                start.FileName = file.FullName;
            else
            {
                Uri uri = new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                System.IO.FileInfo currentAssembly = new System.IO.FileInfo(uri.LocalPath);

                string fullname = System.IO.Path.Combine(currentAssembly.Directory.FullName, name);
                if (System.IO.File.Exists(fullname))
                    start.FileName = fullname;
            }
            if (inputParamsValidated)
            {
                System.Diagnostics.Process.Start(start);
            }
        }

        public static void CheckForUnsupportedFiles(List<OMSDocument> docs, bool checkForExclusiveLocking)
        {
            if (docs.Count == 0)
            {
                return;
            }

            if (docs.Count == 1 && checkForExclusiveLocking)
            {
                if (!IsExclusiveLockingSetOnDocument(docs[0]))
                    return;
            }

            int unsupportedCount = 0;
            if (IsNotificationOfUnsupportedFileTypeOn())
            {
                DataTable dtUnsupported = FWBS.OMS.CodeLookup.GetLookups("UNSFILETYPES");

                foreach (OMSDocument doc in docs)
                {
                    IStorageItem item = (IStorageItem)doc;
                    string fileExtension = item.Extension;
                    DataRow[] matches = dtUnsupported.Select("cdDesc = '" + fileExtension + "'");

                    if (!checkForExclusiveLocking && matches.Length > 0)
                    {
                        unsupportedCount++;
                    }
                    else if (checkForExclusiveLocking && IsExclusiveLockingSetOnDocument(doc) && matches.Length > 0)
                    {
                        unsupportedCount++;
                    }
                }

                if (unsupportedCount == 1)
                    FeedBackOnUnsupportedFiles("MSGUNSFILETYPE", "Please remember to check your document back in.");
                else if (unsupportedCount > 1)
                    FeedBackOnUnsupportedFiles("MSGUNSFILETYPES", "Please remember to check your documents back in.");
            }
        }

        private static bool IsNotificationOfUnsupportedFileTypeOn()
        {
            return Session.CurrentSession.CurrentUser.NotifyOfOpeningUnsupportedFiletype == FWBS.Common.TriState.True || (Session.CurrentSession.CurrentUser.NotifyOfOpeningUnsupportedFiletype == FWBS.Common.TriState.Null && Session.CurrentSession.NotifyOfOpeningUnsupportedFiletype == true);
        }

        private static bool IsExclusiveLockingSetOnDocument(OMSDocument doc)
        {
            if (String.IsNullOrEmpty(doc.OMSFile.CurrentFileType.DocumentLocking))
            {
                return (Session.CurrentSession.DocumentLocking.ToUpper() == "E");
            }
            else
            {
                return (doc.OMSFile.CurrentFileType.DocumentLocking.ToUpper() == "E");
            }
        }

        private static void FeedBackOnUnsupportedFiles(String code, string description)
        {
            System.Windows.Forms.MessageBox.Show(Session.CurrentSession.Resources.GetResource(code, description, "").Text,
                                      Session.CurrentSession.Resources.GetResource("CAPUNSFILETYPE", "Warning", "").Text
                                      , MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }


        #endregion

        #region Select Methods

        /// <summary>
        /// Displays another users persisted job list.
        /// </summary>
        /// <param name="user">The user object whose job list to view.</param>
        /// <returns>True if there are some changes to be made.</returns>
        public static bool SelectJobs(IWin32Window owner, User user)
        {
            if (CheckLogin())
            {
                PrecedentJobList list = new PrecedentJobList(user);
                using (frmJobs frm = new frmJobs(list))
                {
                    DialogResult res = frm.ShowDialog(owner);

                    switch (frm.DialogResult)
                    {
                        case System.Windows.Forms.DialogResult.OK:
                            //Process the jobs here.
                            Services.ProcessJobList(null);
                            return true;
                        case System.Windows.Forms.DialogResult.Yes:
                            //if dialog returns okay and selected jobs then adopt those jobs
                            //into the current jobs to process.
                            foreach (PrecedentJob job in frm.SelectedJobs)
                                job.Adopt();
                            return true;
                        default:
                            return false;

                    }
                }
            }
            return false;
        }

        public static bool SelectJobs(User user)
        {
            return SelectJobs(null, user);
        }

        /// <summary>
        /// Displays the current sessions job list.
        /// </summary>
        public static void ShowCurrentJobs(IWin32Window owner)
        {
            if (CheckLogin())
            {
                using (frmJobs frm = new frmJobs(Session.CurrentSession.CurrentPrecedentJobList))//;
                {
                    DialogResult res = frm.ShowDialog(owner);
                    switch (frm.DialogResult)
                    {
                        case System.Windows.Forms.DialogResult.OK:
                            //Process the jobs here.
                            Services.ProcessJobList(null);
                            break;
                        case System.Windows.Forms.DialogResult.Yes:
                            //if dialog returns okay and selected jobs then adopt those jobs
                            //into the current jobs to process.
                            foreach (PrecedentJob job in frm.SelectedJobs)
                                job.Adopt();
                            break;
                    }
                }
            }
        }

        public static void ShowCurrentJobs()
        {
            ShowCurrentJobs(null);
        }

        /// <summary>
        /// Returns the current client object which was selected from the selection form.
        /// </summary>
        /// <returns>Client object which will relate to the current client of the session object.</returns>
        public static Client SelectClient(IWin32Window owner)
        {
            if (CheckLogin())
            {
                SelectClient dlg = new SelectClient(FWBS.OMS.Session.CurrentSession.CurrentClient);
                return dlg.Show(owner);
            }
            else
                return null;
        }

        public static Client SelectClient()
        {
            return SelectClient(null);
        }

        public static string SelectClientReturnClientNo()
        {
            Client c = SelectClient(null);
            if (c != null)
                return c.ClientNo;
            else
                return null;
        }

        /// <summary>
        /// Returns the current file object which was selected from the selection form.
        /// </summary>
        /// <returns>OMS file object which will relate to the current client / file of the session object.</returns>
        public static OMSFile SelectFile(IWin32Window owner)
        {

            if (CheckLogin())
            {
                try
                {
                    SelectFile dlg = new SelectFile(Session.CurrentSession.CurrentFile);
                    return dlg.Show(owner);
                }
                catch (OMSException ex)
                {
                    if (ex.HelpID == FWBS.OMS.HelpIndexes.PasswordRequestCancelled)
                    {
                        throw new FWBS.OMS.Security.SecurityException(FWBS.OMS.HelpIndexes.PasswordRequestCancelled);
                    }
                    return null;
                }
            }
            else
                return null;
        }

        public static OMSFile SelectFile()
        {
            return SelectFile(null);
        }




        /// <summary>
        /// Returns the current Default Associate object which was selected from the selection form.
        /// </summary>
        /// <returns>Associate object will relate to the current client / file of the session object.</returns>
        public static Associate SelectDefaultAssociate(IWin32Window owner)
        {
            if (CheckLogin())
            {
                if (Session.CurrentSession.SelectFileEnquiryOverride != "")
                {
                    FWBS.Common.KeyValueCollection x = new FWBS.Common.KeyValueCollection();
                    x.Add("Private", AllowPrivateAssociate);
                    Enquiry e = Enquiry.GetEnquiry(Session.CurrentSession.SelectFileEnquiryOverride, null, EnquiryMode.Add, x);
                    Screens n = new Screens(e);
                    object obj = n.Show();
                    OMSFile file = obj as OMSFile;
                    Associate ass = obj as Associate;
                    if (ass != null)
                        return ass;
                    else if (file != null)
                        return file.DefaultAssociate;
                    else
                        return null;
                }
                else
                {
                    SelectAssociate dlg = new SelectAssociate(FWBS.OMS.Session.CurrentSession.CurrentFile);
                    dlg.AllowPrivateAssociate = AllowPrivateAssociate;
                    dlg.UseDefault = true;
                    return dlg.Show(owner);
                }

            }
            else
                return null;
        }

        public static Associate SelectDefaultAssociate()
        {
            return SelectDefaultAssociate(null);
        }

        /// <summary>
        /// Returns the current Associate object which was selected from the selection form.
        /// </summary>
        /// <returns>Associate object will relate to the current client / file of the session object.</returns>
        public static Associate SelectAssociate(IWin32Window owner)
        {
            if (CheckLogin())
            {
                SelectAssociate dlg = new SelectAssociate(FWBS.OMS.Session.CurrentSession.CurrentFile);
                dlg.AllowPrivateAssociate = AllowPrivateAssociate;
                return dlg.Show(owner);
            }
            else
                return null;
        }

        public static Associate SelectAssociate()
        {
            return SelectAssociate(null);
        }

        #endregion

        #region Show Methods

        public static string AddField(IWin32Window owner, FWBS.OMS.Interfaces.IOMSApp controlApp)
        {
            string fld = "";
            bool ssm = false;
            frmFields frm = null;

            if (CheckLogin())
            {
                frm = new frmFields(controlApp);
                frm.Owner = _parent;
                if (controlApp == null)
                {
                    frm.ShowDialog(owner);
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        fld = frm.SelectedField;
                        ssm = frm.AsSecondStageMergeField;
                    }
                    frm.Dispose();
                }
                else
                {
                    FWBS.Common.Functions.SetParentWindow(owner, _parent);
                    frm.Show();
                    return "";
                }
            }

            if (fld != String.Empty && controlApp is FWBS.OMS.Interfaces.ISecondStageMergeOMSApp)
            {
                if (ssm)
                    ((FWBS.OMS.Interfaces.ISecondStageMergeOMSApp)controlApp).AddSecondStageMergeField(controlApp, fld);
                else
                    controlApp.AddField(controlApp, fld);
            }
            else if (fld != String.Empty && controlApp != null)
                controlApp.AddField(controlApp, fld);

            return fld;

        }


        /// <summary>
        /// Deletes the selected field from the active document on an OMS App.
        /// </summary>
        /// <param name="controlApp"></param>
        public static string DeleteField(IWin32Window owner, FWBS.OMS.Interfaces.IOMSApp controlApp)
        {
            string fld = "";
            if (CheckLogin())
            {
                using (frmDeleteField frm = new frmDeleteField(controlApp))//;
                {
                    frm.ShowDialog(owner);
                    if (frm.DialogResult == DialogResult.OK)
                    {
                        fld = frm.SelectedField;
                    }
                }
            }

            return fld;
        }

        /// <summary>
        /// Displays the Set Password Dialog for a PasswordProtectedBase
        /// </summary>
        public static void ChangePassword(IWin32Window owner, PasswordProtectedBase obj)
        {
            if (CheckLogin())
            {
                using (frmSetPassword frm = new frmSetPassword(obj))
                {
                    frm.ShowDialog(owner);
                }
            }
        }

        public static void ChangePassword(PasswordProtectedBase obj)
        {
            ChangePassword(null, obj);
        }

        /// <summary>
        /// Displays the about box.
        /// </summary>
        public static void ShowAbout()
        {
            ShowAbout(null);
        }

        public static void ShowAbout(IWin32Window owner)
        {
            using (frmAbout frm = new frmAbout())
            {
                frm.ShowDialog(owner);
            }
        }

        /// <summary>
        /// Show the Splash Screen
        /// </summary>
        public static void ShowSplash(bool modal)
        {
            ShowSplash(null, modal);
        }

        /// <summary>
        /// Show the Splash Screen
        /// </summary>
        public static void ShowSplash(Form owner, bool modal)
        {
            if (modal)
            {
                using (frmSplash oSplash = new frmSplash())
                {
                    oSplash.ShowDialog();
                }
            }

            else
            {
                _frmsplash = new frmSplash();
                if (owner != null)
                    _frmsplash.Owner = owner;
                _frmsplash.Show();
            }
        }

        /// <summary>
        /// Close Splash Screen
        /// </summary>
        public static void CloseSplash()
        {
            if (_frmsplash != null)
            {
                _frmsplash.Close();
                _frmsplash = null;
            }
        }


        /// <summary>
        /// Displays the current user settings dialog.
        /// </summary>
        public static void DisplayCurrentUserSettings(IWin32Window owner)
        {
            if (CheckLogin())
                ShowOMSItem(owner, Session.CurrentSession.DefaultSystemForm(SystemForms.UserSettings), null, Session.CurrentSession.CurrentUser, null);
        }

        public static void DisplayCurrentUserSettings()
        {
            DisplayCurrentUserSettings(null);
        }

        /// <summary>
        /// Displays a oms type dialog from based on the type of object passed.
        /// </summary>
        /// <param name="Style">The SearchManager</param>
        public static void ShowSearchManager(IWin32Window owner, SearchManager Style)
        {
            if (CheckLogin())
            {
                var clientform = new frmOMSTypeV2(Style);
                
                if (owner != null)
                {
                    clientform.Owner = _parent;
                    FWBS.Common.Functions.SetParentWindow(owner, _parent);
                }
                
                clientform.Show();
            }
        }


        public static void ShowSearchManager(SearchManager Style)
        {
            ShowSearchManager(null, Style);
        }

        /// <summary>
        /// Displays the OMS Open Document Dialog
        /// </summary>
        public static void ShowOpenDocument(IWin32Window owner, FWBS.OMS.Interfaces.IOMSApp controlApp)
        {
            if (CheckLogin())
            {
                frmOMSDocumentDialog omsdoc = new frmOMSDocumentDialog(controlApp);
                if (owner != null)
                {
                    omsdoc.Owner = _parent;
                    FWBS.Common.Functions.SetParentWindow(owner, _parent);
                    omsdoc.Show();
                }
                else
                {
                    omsdoc.ShowDialog(owner);
                    omsdoc.Dispose();
                }
            }
        }

        public static void ShowOpenDocument(FWBS.OMS.Interfaces.IOMSApp controlApp)
        {
            ShowOpenDocument(null, controlApp);
        }

        public static void ShowLocalModifiedDocuments(IWin32Window owner)
        {
            DataTable dt = FWBS.OMS.DocumentManagement.Storage.StorageManager.CurrentManager.LocalDocuments.GetLocalDocumentInfo();
            DataView vw = new DataView(dt);
            vw.RowFilter = "[HasChanged] = true";
            if (vw.Count > 0)
            {
                if (MessageBox.ShowYesNoQuestion(owner, "MSGLOCDOCCHANGE", "There are changes to the offline local documents.  Would you like to view them?") == DialogResult.Yes)
                {
                    DocumentManagement.DocumentPicker picker = new FWBS.OMS.UI.Windows.DocumentManagement.DocumentPicker();
                    picker.Title = "Modified Local Documents";
                    System.IO.FileInfo[] files = picker.ShowLocal(owner, true);
                    if (files != null)
                    {
                        foreach (System.IO.FileInfo file in files)
                        {
                            if (System.IO.File.Exists(file.FullName))
                            {
                                Services.ProcessStart("OMS.UTILS.EXE", string.Format("OPEN \"{0}\"", file), InputValidation.ValidateOpenFileInput);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Displays a oms type dialog from based on the type of object passed.
        /// </summary>
        /// <param name="obj">IOMSType object.</param>
        /// <param name="defaultPage">The default page to show.</param>
        internal static void ShowOMSType(IWin32Window owner, FWBS.OMS.Interfaces.IOMSType obj, OMSType omst, string defaultPage)
        {
            OMSTypeScreen screen = new OMSTypeScreen(obj);
            screen.DefaultPage = defaultPage;
            screen.OmsType = omst;
            if (owner == null)
                screen.ShowDialog(owner);
            else
                screen.Show(owner);
        }

        /// <summary>
        /// Displays a oms type dialog from based on the type of object passed.
        /// </summary>
        /// <param name="obj">IOMSType object.</param>
        /// <param name="defaultPage">The default page to show.</param>
        public static void ShowOMSType(IWin32Window owner, FWBS.OMS.Interfaces.IOMSType obj, string defaultPage)
        {
            ShowOMSType(owner, obj, null, defaultPage);

        }

        public static void ShowOMSType(FWBS.OMS.Interfaces.IOMSType obj, string defaultPage)
        {
            ShowOMSType(null, obj, defaultPage);
        }



        /// <summary>
        /// Displays the current users command centre.
        /// </summary>
        public static void ShowCommandCentre(IWin32Window owner, string defaultPage)
        {
            if (!CheckLogin())
                return;

            User usr = Session.CurrentSession.CurrentUser;
            if (usr != null)
                ShowOMSType(owner, usr, usr.CommandCentre, defaultPage);
        }

        /// <summary>
        /// Displays the client form by specifying whether the client prompt / search screen
        /// will appear first.
        /// </summary>
        /// <param name="prompt">If false then the current client will be used.</param>
        /// <param name="defaultPage">The default page to show.</param>
        public static void ShowClient(IWin32Window owner, bool prompt, string defaultPage)
        {
            if (CheckLogin())
            {
                if (prompt == false)
                {
                    if (Session.CurrentSession.CurrentClient == null)
                    {
                        Client cl = SelectClient();
                        if (cl != null)
                            ShowOMSType(owner, cl, defaultPage);
                    }
                    else
                        ShowOMSType(owner, Session.CurrentSession.CurrentClient, defaultPage);
                }
                else
                {
                    Client cl = SelectClient(owner);
                    if (cl != null)
                        ShowOMSType(owner, cl, defaultPage);
                }
            }
        }

        public static void ShowClient(bool prompt, string defaultPage)
        {
            ShowClient(null, prompt, defaultPage);
        }

        public static void ShowClient(IWin32Window owner, Client client, string defaultPage)
        {
            if (client != null)
                ShowOMSType(owner, client, defaultPage);
        }
        public static void ShowClient(Client client, string defaultPage)
        {
            if (client != null)
                ShowOMSType(null, client, defaultPage);
        }

        public static void ShowClient(Client client)
        {
            if (client != null)
                ShowOMSType(null, client, null);
        }

        public static void ShowClient(bool prompt)
        {
            ShowClient(null, prompt, null);
        }

        /// <summary>
        /// Displays the file form by specifying whether the file prompt / search screen
        /// will appear first.
        /// </summary>
        /// <param name="prompt">If false then the current file will be used.</param>
        /// <param name="defaultPage">The default page to show.</param>
        public static void ShowFile(IWin32Window owner, bool prompt, string defaultPage)
        {

            if (CheckLogin())
            {
                if (prompt == false)
                {
                    if (Session.CurrentSession.CurrentFile == null)
                    {
                        try
                        {
                            OMSFile file = SelectFile(owner);
                            if (file != null)
                                ShowOMSType(owner, file, defaultPage);
                        }
                        catch (OMSException ex)
                        {
                            if (ex.HelpID == FWBS.OMS.HelpIndexes.PasswordRequestCancelled)
                            {
                                throw new FWBS.OMS.Security.SecurityException(FWBS.OMS.HelpIndexes.PasswordRequestCancelled);
                            }
                        }
                    }
                    else
                    {
                        ShowOMSType(owner, Session.CurrentSession.CurrentFile, defaultPage);
                    }
                }
                else
                {
                    try
                    {
                        OMSFile file = SelectFile(owner);
                        if (file != null)
                            ShowOMSType(owner, file, defaultPage);
                    }
                    catch (OMSException ex)
                    {
                        if (ex.HelpID == FWBS.OMS.HelpIndexes.PasswordRequestCancelled)
                        {
                            throw new FWBS.OMS.Security.SecurityException(FWBS.OMS.HelpIndexes.PasswordRequestCancelled);
                        }
                    }
                }
            }
        }

        public static void ShowFile(bool prompt, string defaultPage)
        {
            ShowFile(null, prompt, defaultPage);
        }

        public static void ShowFile(IWin32Window owner, OMSFile file, string defaultPage)
        {
            if (file != null)
                ShowOMSType(owner, file, defaultPage);
        }

        public static void ShowFile(OMSFile file, string defaultPage)
        {
            if (file != null)
                ShowOMSType(null, file, defaultPage);
        }

        public static void ShowFile(OMSFile file)
        {
            if (file != null)
                ShowOMSType(null, file, null);
        }


        public static void ShowFile(Int64 fileid)
        {
            OMSFile file = OMSFile.GetFile(fileid);
            ShowOMSType(null, file, null);
        }

        public static void ShowFile(Int64 fileid, string defaultPage)
        {
            OMSFile file = OMSFile.GetFile(fileid);
            ShowOMSType(null, file, defaultPage);
        }


        public static void ShowFile(bool prompt)
        {
            ShowFile(null, prompt, null);
        }


        /// <summary>
        /// Displays the associate form by specifying whether the file prompt / search screen
        /// will appear first.
        /// </summary>
        /// <param name="prompt">If false then the current associate will be used.</param>
        /// <param name="defaultPage">The default page to show.</param>
        public static void ShowAssociate(IWin32Window owner, bool prompt, string defaultPage)
        {
            if (CheckLogin())
            {
                if (prompt == false)
                {
                    if (Session.CurrentSession.CurrentAssociate == null)
                    {
                        Associate assoc = SelectAssociate(owner);
                        if (assoc != null)
                            ShowOMSType(owner, assoc, defaultPage);
                    }
                    else
                    {
                        ShowOMSType(owner, Session.CurrentSession.CurrentAssociate, defaultPage);
                    }
                }
                else
                {
                    Associate assoc = SelectAssociate(owner);
                    if (assoc != null)
                        ShowOMSType(owner, assoc, defaultPage);
                }
            }
        }

        public static void ShowAssociate(bool prompt, string defaultPage)
        {
            ShowAssociate(null, prompt, defaultPage);
        }

        public static void ShowAssociate(IWin32Window owner, Associate assoc, string defaultPage)
        {
            if (assoc != null)
                ShowOMSType(owner, assoc, defaultPage);
        }

        public static void ShowAssociate(Associate assoc, string defaultPage)
        {
            if (assoc != null)
                ShowOMSType(null, assoc, defaultPage);
        }

        public static void ShowAssociate(Associate assoc)
        {
            if (assoc != null)
                ShowOMSType(null, assoc, null);
        }


        public static void ShowAssociate(Int64 associd)
        {
            Associate assoc = Associate.GetAssociate(associd);
            ShowOMSType(null, assoc, null);
        }

        public static void ShowAssociate(Int64 associd, string defaultPage)
        {
            Associate assoc = Associate.GetAssociate(associd);
            ShowOMSType(null, assoc, defaultPage);
        }


        public static void ShowAssociate(bool prompt)
        {
            ShowAssociate(null, prompt, null);
        }

        public static void ShowDocument(IWin32Window owner, OMSDocument doc, string defaultPage)
        {
            if (doc == null)
                throw new ArgumentNullException("doc");

            if (CheckLogin())
            {
                ShowOMSType(owner, doc, defaultPage);
            }
        }

        public static void ShowDocument(IWin32Window owner, string defaultPage)
        {
            DocumentManagement.DocumentPicker picker = new FWBS.OMS.UI.Windows.DocumentManagement.DocumentPicker();
            OMSDocument[] docs = picker.Show(owner);
            if (docs == null || docs.Length == 0)
                return;

            ShowDocument(owner, docs[0], defaultPage);
        }

        public static void ShowDocument(IWin32Window owner, OMSDocument doc)
        {
            ShowDocument(owner, doc, "");
        }

        public static void ShowDocument(OMSDocument doc, string defaultPage)
        {
            ShowDocument(null, doc, defaultPage);
        }

        public static void ShowDocument(OMSDocument doc)
        {
            ShowDocument(null, doc, "");
        }


        public static void ShowDocument(long docid)
        {
            OMSDocument doc = OMSDocument.GetDocument(docid);
            ShowDocument(null, doc, "");
        }

        public static void ShowDocument(long docid, string defaultPage)
        {
            OMSDocument doc = OMSDocument.GetDocument(docid);
            ShowDocument(null, doc, defaultPage);
        }


        /// <summary>
        /// Displays the contact form by specifying the contact object.
        /// </summary>
        /// <param name="contact">Contact object.</param>
        /// <param name="defaultPage">The default page to show.</param>
        public static void ShowContact(IWin32Window owner, Contact contact, string defaultPage)
        {
            if (contact != null)
                ShowOMSType(owner, contact, defaultPage);
        }

        public static void ShowContact(Contact contact, string defaultPage)
        {
            ShowContact(null, contact, defaultPage);
        }

        public static void ShowContact(Contact contact)
        {
            if (contact != null)
                ShowOMSType(null, contact, null);
        }



        /// <summary>
        /// Show the Precedent Library related and linked via an associate to work with
        /// </summary>
        /// <param name="controlApp">Controlling Application</param>
        /// <param name="obj">Client / Associate Object</param>
        /// <param name="activeType">The active type to be used within the precedent library.</param>
        /// <param name="library">The Library</param>
        public static void ShowPrecedentLibrary(IWin32Window owner, FWBS.OMS.Interfaces.IOMSApp controlApp, Associate assoc, string activeType, string library)
        {
            if (CheckLogin())
            {
                if (_frmprec == null)
                    _frmprec = new FWBS.OMS.UI.Windows.frmPrecedent() { Owner = _parent };
                else if (!_frmprec.IsHandleCreated)
                    return;
                try
                {
                    // This routine will show the precedent library passing through
                    // the above objects which maybe null or of type FWBS.OMS.Associate or an IOMSAPP object
                    // this will allow the precedent manager to filter down the 
                    // type of documents available
                    if (assoc != null)
                    {
                        _frmprec.Precedent.SetDefaults(controlApp, assoc as Associate, activeType, assoc.OMSFile.PrecedentLibrary);
                    }
                    else
                    {
                        _frmprec.Precedent.SetDefaults(controlApp, activeType);
                    }
                    FWBS.Common.Functions.SetParentWindow(owner, _parent);
                    _frmprec.Show();
                }
                catch
                {
                    if (!_frmprec.IsHandleCreated)
                        _frmprec = null;
                    throw;
                }
            }
        }

        public static void ShowPrecedentLibrary(FWBS.OMS.Interfaces.IOMSApp controlApp, Associate assoc, string activeType, string library = "")
        {
            ShowPrecedentLibrary(null, controlApp, assoc, activeType, library);
        }


        /// <summary>
        /// Gets a enquiry form using an enquiry code and enquiry mode.  It will return
        /// an output parameter that will give the object that the wizard is manipulating.
        /// </summary>
        /// <param name="code">Unique enquiry code.</param>
        /// <param name="parent">The parent to use for the enquiry form.</param>
        /// <param name="mode">Specifies the form to be in add or edit mode.  If in edit mode an item will be need in the param parameter.</param>
        /// <param name="offline">Specifies whether the database will be updated or not.</param>
        /// <param name="obj">Outputs an object reference that the form has created or edited.</param>
        /// <param name="param">Parameters that may be needed to replace items in the enquiry to run properly.</param>
        public static object ShowOMSItem(IWin32Window owner, string code, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param, EnquiryFormSettings settings = EnquiryFormSettings.None)
        {
            object obj = null;
            if (CheckLogin())
            {
                try
                {
                    using (var frm = FWBS.OMS.UI.Factory.frmOMSItemFactory.GetFrmOMSItem(code, parent, mode, offline, param))
                    {
                        frm.Settings = settings;
                        frm.Text = frm.EnquiryForm.Description;
                        frm.FormStorageID = code;

                        if (frm.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                            obj = frm.EnquiryForm.Enquiry.Object;
                    }
                }
                catch (Exception ex)
                {
                    obj = null;
                    ErrorBox.Show(owner, ex);
                }
            }
            return obj;
        }


        /// <summary>
        /// Edits an existing library object with the specified enquiry code. It will return
        /// an output parameter that will give the object that the wizard is manipulating.
        /// </summary>
        /// <param name="code">Enquiry code to use for the edit.</param>
        /// <param name="parent">The parent to use for the enquiry form.</param>
        /// <param name="businessObject">Business object to edit (i.e, CurrentUser)</param>
        /// <param name="param">Parameters that may be needed to replace items in the enquiry to run properly.</param>
        public static object ShowOMSItem(IWin32Window owner, string code, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible businessObject, Common.KeyValueCollection param, EnquiryFormSettings settings = EnquiryFormSettings.None)
        {
            object obj = null;
            if (CheckLogin())
            {
                try
                {
                    using (var frm = FWBS.OMS.UI.Factory.frmOMSItemFactory.GetFrmOMSItem(code, parent, businessObject, param))
                    {
                        frm.Settings = settings;
                        frm.Text = frm.EnquiryForm.Description;
                        frm.FormStorageID = code;
                        frm.Size = new System.Drawing.Size(frm.EnquiryForm.Width, frm.EnquiryForm.Size.Height + 60);

                        if (frm.ShowDialog(owner) == System.Windows.Forms.DialogResult.OK)
                            obj = frm.EnquiryForm.Enquiry.Object;
                    }
                }
                catch (Exception ex)
                {
                    obj = null;
                    ErrorBox.Show(owner, ex);
                }
            }

            return obj;
        }

        public static object ShowOMSItem(string code, object parent, EnquiryMode mode, Common.KeyValueCollection param)
        {
            return ShowOMSItem(null, code, parent, mode, false, param);
        }

        public static object ShowOMSItem(string code, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param)
        {
            return ShowOMSItem(null, code, parent, mode, offline, param);
        }

        public static object ShowOMSItem(string code, object parent, FWBS.OMS.Interfaces.IEnquiryCompatible businessObject, Common.KeyValueCollection param)
        {
            return ShowOMSItem(null, code, parent, businessObject, param);
        }

        /// <summary>
        /// Gets a enquiry form using an enquiry code and enquiry mode.  It will return
        /// an output parameter that will give the object that the wizard is manipulating.
        /// </summary>
        /// <param name="code">Unique enquiry code.</param>
        /// <param name="parent">The parent to use for the enquiry form.</param>
        /// <param name="mode">Specifies the form to be in add or edit mode.  If in edit mode an item will be need in the param parameter.</param>
        /// <param name="offline">Specifies whether the database will be updated or not.</param>
        /// <param name="param">Parameters that may be needed to replace items in the enquiry to run properly.</param>
        public static ucOMSItemBase GetOMSItemControl(string code, object parent, EnquiryMode mode, bool offline, Common.KeyValueCollection param)
        {
            if (CheckLogin())
            {
                try
                {
                    return OMSItemFactory.CreateOMSItem(code, parent, mode, offline, param);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        public static ucOMSItemBase GetOMSItemControl(string code, object parent, OMS.Interfaces.IEnquiryCompatible businessObject, bool offline, Common.KeyValueCollection param)
        {
            if (CheckLogin())
            {
                try
                {
                    return OMSItemFactory.CreateOMSItem(code, parent, businessObject, offline, param);
                }
                catch (Exception ex)
                {
                    ErrorBox.Show(ex);
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets suitable OMSItem control.
        /// </summary>
        /// <returns>OMSItem control with Task enquiry.</returns>
        public static ucOMSItemBase SaveTask()
        {
            return GetOMSItemControl(Session.CurrentSession.DefaultSystemForm(SystemForms.TaskItem), null, EnquiryMode.Add, false, null);
        }

        /// <summary>
        /// Creates a task by prompting the user for a file.
        /// </summary>
        /// <returns>OMS item control with task enquiry, or null if form is cancelled</returns>
        public static ucOMSItemBase CreateTask()
        {
            return CreateTask(null);
        }

        /// <summary>
        /// Creates a task with the specified file.
        /// </summary>
        /// <param name="file">The file to create the task under.</param>
        /// <returns>OMS Item control with required appointment enquiry, or null if form is cancelled.</returns>
        public static ucOMSItemBase CreateTask(OMSFile file)
        {
            ucOMSItemBase tsk = null;

            if (file == null)
                file = SelectFile();

            if (file != null)
            {
                tsk = SaveTask();
            }
            return tsk;
        }


        /// <summary>
        /// Gets suitable OMSItem control
        /// </summary>
        /// <returns>OMSItem control with Appointment enquiry</returns>
        public static object SaveAppointment()
        {
            return GetOMSItemControl(Session.CurrentSession.DefaultSystemForm(SystemForms.AppointmentItem), null, EnquiryMode.Add, false, null);
        }


        /// <summary>
        /// Creates an appointment by prompting the user for a file.
        /// </summary>
        /// <returns>OMS item control with appointment enquiry, or null if form is cancelled</returns>
        public static object CreateAppointment()
        {
            return CreateAppointment(null);
        }

        /// <summary>
        /// Creates an appointment with the specified file.
        /// </summary>
        /// <param name="file">The file to create the appointment under.</param>
        /// <returns>OMS Item control with required appointment enquiry, or null if form is cancelled.</returns>
        public static object CreateAppointment(OMSFile file)
        {
            object result = null;
            if (file == null)
                file = SelectFile();

            if (file != null)
            {
                result = SaveAppointment();
            }
            return result;
        }

        internal static void ShowUserAutoWindows(IWin32Window owner)
        {
            //If the just logged in user needs to be shown a welcome wizard then display it.
            if (Session.CurrentSession.CurrentUser.WelcomeWizard)
            {
                Services.Wizards.Welcome();
            }

            //If the just logged in user needs to be shown his/her command centre then display it.
            if (Session.CurrentSession.CurrentUser.DisplayCommandCentreAtLogon)
            {
                Services.ShowCommandCentre(owner, "");
            }

            //local documents message popup
            if (Session.CurrentSession.CurrentUser.PromptLocalChangedDocuments)
            {
                Services.ShowLocalModifiedDocuments(owner);
            }
        }

        #endregion

        #region Job Methods

        public static void SendDocViaEmail(IWin32Window owner, FWBS.OMS.DocumentManagement.Storage.IStorageItem[] docs, Associate assoc)
        {
            SendDocViaEmail(owner, docs, assoc, false);
        }

        /// <summary>
        /// send document link as email.
        /// </summary>
        /// <param name="msgbody">Email body.</param>
        /// <param name="msgsub">Email subject.</param>
        /// 
        public static void SendDocLinkViaEmail(IWin32Window owner, Associate assoc, string msgbody, string msgsub)
        {
            DocumentManagement.SendDocumentViaEmailCommand send = new DocumentManagement.SendDocumentViaEmailCommand();
            send.AllowUI = true;
            send.DisplayAssociatePicker = FWBS.OMS.Commands.DisplayWhen.ValueNotSpecified;
            send.AssociatePicker.UseDefault = true;
            send.SendLink = true;
            send.To = string.Empty;
            send.ToAssociate = assoc;
            send.HtmlBody = msgbody;
            send.Subject = msgsub;
            send.Execute();
        }


        public static void SendDocViaEmail(IWin32Window owner, FWBS.OMS.DocumentManagement.Storage.IStorageItem[] docs, Associate assoc, bool convertToPDF)
        {
            DocumentManagement.SendDocumentViaEmailCommand send = new DocumentManagement.SendDocumentViaEmailCommand();
            send.Owner = owner;
            send.AllowUI = true;
            send.DisplayAssociatePicker = FWBS.OMS.Commands.DisplayWhen.ValueNotSpecified;
            if ((Session.CurrentSession.CurrentUser.UseDefaultAssociate == FWBS.Common.TriState.Null && Session.CurrentSession.UseDefaultAssociate == false) || Session.CurrentSession.CurrentUser.UseDefaultAssociate == FWBS.Common.TriState.False)
                send.AssociatePicker.UseDefault = false;
            else
                send.AssociatePicker.UseDefault = true;
            send.ToAssociate = assoc;
            send.AdditionalCCs = true;
            send.EmailTemplate = Precedent.GetPrecedent("DEFAULT", "EMAIL", String.Empty, String.Empty, String.Empty, String.Empty);
            send.DocumentsToAttach.AddRange(docs);
            send.ConvertToPDF = convertToPDF;
            send.Execute();
        }

        /// <summary>
        /// Opens a document from its document identifier.
        /// </summary>
        /// <param name="id">The document id to open.</param>
        /// <param name="mode">Opening Mode.</param>
        public static bool OpenDocument(long id, DocOpenMode mode)
        {
            OMSDocument doc = OMSDocument.GetDocument(id);
            return OpenDocument(doc, mode, true);
        }

        public static bool OpenDocument(OMSDocument doc, DocOpenMode mode)
        {
            return OpenDocument(doc, mode, true);
        }

        public static bool OpenDocument(FWBS.OMS.DocumentManagement.DocumentVersion version, DocOpenMode mode)
        {
            if (version == null)
                throw new ArgumentNullException("version");

            if (!version.Accepted)
            {
                MessageBox.ShowInformation("DOCDELNOOPN", "The Document cannot be opened, it has been deleted");
                return false;
            }

            Apps.RegisteredApplication app = ((OMSDocument)version.BaseStorageItem).DocProgType;
            FWBS.OMS.Interfaces.IOMSApp appcontroller = Apps.ApplicationManager.CurrentManager.GetApplicationInstance(app, true);

            try
            {

                object obj = appcontroller.Open(version, mode);
                if (obj == null)
                {
                    MessageBox.Show(Session.CurrentSession.Resources.GetMessage("4008", "The Document cannot be used with the specified mode %1%", "", mode.ToString()));
                    return false;
                }
                else
                {
                    if (mode == DocOpenMode.Print)
                        appcontroller.Close(obj);
                }
                return true;
            }
            catch (FWBS.OMS.DocumentManagement.Storage.CancelStorageException)
            {
                return false;
            }
            catch (FWBS.OMS.OMSException omsex)
            {
                //added DMB 9/8/2004 If a password prompt is cancelled it is not valid to display an error box
                if (omsex.HelpID != HelpIndexes.PasswordRequestCancelled)
                    MessageBox.Show(omsex);

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex);
                return false;
            }
        }

        public static bool OpenDocument(OMSDocument doc, DocOpenMode mode, bool latestVersion)
        {
            if (!doc.Accepted)
            {
                MessageBox.ShowInformation("DOCDELNOOPN", "The Document cannot be opened, it has been deleted");
                return false;
            }

            if (!latestVersion)
            {
                using (DocumentManagement.Storage.StorageSettingsForm settings = new FWBS.OMS.UI.Windows.DocumentManagement.Storage.StorageSettingsForm(doc, SettingsType.Fetch))
                {
                    if (settings.ShowDialog(null, typeof(FWBS.OMS.DocumentManagement.Storage.VersionFetchSettings).Name) == DialogResult.Cancel)
                        return false;

                }
            }


            FWBS.OMS.DocumentManagement.Storage.IStorageItem item = doc;
            Apps.RegisteredApplication app = doc.DocProgType;

            //Check to see if the current app is the shelloms app.  If so try and change the
            //existing app to one that is registered against the file type.
            if (app.Code == "SHELL")
            {
                app = Apps.ApplicationManager.CurrentManager.GetRegisteredApplicationByExtension(item.Extension);
                if (app == null)
                    app = doc.DocProgType;
                else
                {
                    doc.DocProgType = app;
                    doc.Update();
                }
            }

            FWBS.OMS.Interfaces.IOMSApp appcontroller = Apps.ApplicationManager.CurrentManager.GetApplicationInstance(app, true);

            try
            {

                object obj = appcontroller.Open(doc, mode);
                if (obj == null)
                {
                    MessageBox.Show(Session.CurrentSession.Resources.GetMessage("4008", "The Document cannot be used with the specified mode %1%", "", mode.ToString()));
                    return false;
                }
                else
                {
                    if (mode == DocOpenMode.Print)
                        appcontroller.Close(obj);
                }
                return true;
            }
            catch (FWBS.OMS.DocumentManagement.Storage.CancelStorageException)
            {
                return false;
            }
            catch (FWBS.OMS.OMSException omsex)
            {
                //added DMB 9/8/2004 If a password prompt is cancelled it is not valid to display an error box
                if (omsex.HelpID != HelpIndexes.PasswordRequestCancelled)
                    MessageBox.Show(omsex);

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex);
                return false;
            }
        }



        public static bool OpenDocument(OMSDocument doc, DocOpenMode mode, bool latestVersion, bool bulkprint, int nocopies)
        {
            if (!doc.Accepted)
            {
                MessageBox.ShowInformation("DOCDELNOOPN", "The Document cannot be opened, it has been deleted");
                return false;
            }

            if (!latestVersion)
            {
                using (DocumentManagement.Storage.StorageSettingsForm settings = new FWBS.OMS.UI.Windows.DocumentManagement.Storage.StorageSettingsForm(doc, SettingsType.Fetch))
                {
                    if (settings.ShowDialog(null, typeof(FWBS.OMS.DocumentManagement.Storage.VersionFetchSettings).Name) == DialogResult.Cancel)
                        return false;

                }
            }


            FWBS.OMS.DocumentManagement.Storage.IStorageItem item = doc;
            Apps.RegisteredApplication app = doc.DocProgType;

            //Check to see if the current app is the shelloms app.  If so try and change the
            //existing app to one that is registered against the file type.
            if (app.Code == "SHELL")
            {
                app = Apps.ApplicationManager.CurrentManager.GetRegisteredApplicationByExtension(item.Extension);
                if (app == null)
                    app = doc.DocProgType;
                else
                {
                    doc.DocProgType = app;
                    doc.Update();
                }
            }

            FWBS.OMS.Interfaces.IOMSApp appcontroller = Apps.ApplicationManager.CurrentManager.GetApplicationInstance(app, true);

            try
            {

                object obj = appcontroller.Open(doc, mode, bulkprint, nocopies);
                if (obj == null)
                {
                    MessageBox.Show(Session.CurrentSession.Resources.GetMessage("4008", "The Document cannot be used with the specified mode %1%", "", mode.ToString()));
                    return false;
                }
                else
                {
                    if (mode == DocOpenMode.Print)
                        appcontroller.Close(obj);
                }
                return true;
            }
            catch (FWBS.OMS.DocumentManagement.Storage.CancelStorageException)
            {
                return false;
            }
            catch (FWBS.OMS.OMSException omsex)
            {
                //added DMB 9/8/2004 If a password prompt is cancelled it is not valid to display an error box
                if (omsex.HelpID != HelpIndexes.PasswordRequestCancelled)
                    MessageBox.Show(omsex);

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex);
                return false;
            }
        }




        /// <summary>
        /// Opens a precedent from its precedent identifier.
        /// </summary>
        /// <param name="id">The precedent id to open.</param>
        /// <param name="mode">Opening Mode.</param>
        public static bool OpenPrecedent(long id, DocOpenMode mode)
        {
            Precedent prec = Precedent.GetPrecedent(id);
            return OpenPrecedent(prec, mode);
        }

    
        /// <summary>
        /// Opens a precedent.
        /// </summary>
        /// <param name="prec">The precedent to open.</param>
        /// <param name="mode">Opening Mode.</param>
        public static bool OpenPrecedent(Precedent prec, DocOpenMode mode)
        {
            return OpenPrecedent(prec, mode, true);
        }


        /// <summary>
        /// Opens a precedent.
        /// </summary>
        /// <param name="prec">The precedent to open.</param>
        /// <param name="mode">Opening Mode.</param>
        /// <param name="latestVersion">Should the latest version be opened</param>
        public static bool OpenPrecedent(Precedent prec, DocOpenMode mode, bool latestVersion, bool fromAdminKit = false)
        {            
            if (!latestVersion)
            {
                if (prec.GetVersionsTable(false).Rows.Count > 1)
                {
                    using (DocumentManagement.Storage.StorageSettingsForm settings = new FWBS.OMS.UI.Windows.DocumentManagement.Storage.StorageSettingsForm(prec, SettingsType.Fetch, fromAdminKit))
                    {
                        if (settings.ShowDialog(null, typeof(FWBS.OMS.DocumentManagement.Storage.VersionFetchSettings).Name) == DialogResult.Cancel)
                            return false;
                    }
                }
            }
            
            FWBS.OMS.DocumentManagement.Storage.IStorageItem item = prec;
            Apps.RegisteredApplication app = prec.PrecProgType;

            //Check to see if the current app is the shelloms app.  If so try and change the
            //existing app to one that is registered against the file type.
            if (app.Code == "SHELL")
            {
                app = Apps.ApplicationManager.CurrentManager.GetRegisteredApplicationByExtension(item.Extension);
                if (app == null)
                    app = prec.PrecProgType;
                else
                {
                    prec.PrecProgType = app;
                    prec.Update();
                }
            }

            FWBS.OMS.Interfaces.IOMSApp appcontroller = Apps.ApplicationManager.CurrentManager.GetApplicationInstance(app, true);

            try
            {
                object obj = appcontroller.Open(prec, mode);
                if (obj == null)
                {
                    MessageBox.Show(Session.CurrentSession.Resources.GetMessage("4007", "The %PRECEDENT% cannot be used with the specified mode %1%", "", mode.ToString()));
                    return false;
                }
                else
                {
                    if (mode == DocOpenMode.Print)
                        appcontroller.Close(obj);
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex);
                return false;
            }
        }

        /// <summary>
        /// Processes a single job.
        /// </summary>
        /// <param name="controlApp">Controling Application if known</param>
        /// <param name="job">The job to run.</param>
        public static ProcessJobStatus ProcessJob(FWBS.OMS.Interfaces.IOMSApp controlApp, PrecedentJob job)
        {
            ProcessJobStatus status = ProcessJobStatus.Finished;

            if (job.Completed == false)
            {
                try
                {
                    FWBS.OMS.Interfaces.IOMSApp app = null;                
                    Type prectype = job.Precedent.PrecProgType.Type;

                    //Now run the new method overload with the precedent object.
                    if (controlApp == null || prectype != controlApp.GetType())
                    {
                        Apps.RegisteredApplication regapp = job.Precedent.PrecProgType;
                        app = Apps.ApplicationManager.CurrentManager.GetApplicationInstance(regapp, true);
                    }
                    else
                    {
                        app = controlApp;
                    }

                    status = app.ProcessJob(job);
                    switch (status)
                    {
                        case ProcessJobStatus.Finished:
                            job.Completed = true;
                            break;
                        case ProcessJobStatus.Error:
                            job.HasError = true;
                            job.ErrorMessage = "Internal OMS Application Error";
                            break;
                        case ProcessJobStatus.PauseJobs:
                            job.Completed = true;
                            break;
                    }
                }
                catch (TargetInvocationException tex)
                {
                    job.HasError = true;
                    if (tex.InnerException == null)
                        job.ErrorMessage = tex.Message;
                    else
                        job.ErrorMessage = tex.InnerException.Message;
                }
                catch (Exception ex)
                {
                    job.HasError = true;
                    job.ErrorMessage = ex.Message;
                }
            }

            return status;
        }

        /// <summary>
        /// Check the precedent job list and ask to process the list.
        /// </summary>
        /// <param name="controlApp">Controling Application if known</param>
        public static void CheckJobList(FWBS.OMS.Interfaces.IOMSApp controlApp)
        {
            if (Session.CurrentSession.CurrentPrecedentJobList.LiveCount > 0)
            {
                MessageBox msg = new MessageBox(MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                msg.Text = Session.CurrentSession.Resources.GetMessage("RUSUREPROCJOB", "There are Active Jobs would you like to process?", "", false);
                msg.Buttons[2] = "VIEW";
                switch (msg.Show())
                {
                    case "YES":
                        ProcessJobList(controlApp);
                        break;
                    case "VIEW":
                        ShowCurrentJobs();
                        break;
                }
            }
        }

        /// <summary>
        /// Process Job list will initiate the Process Engine.
        /// </summary>
        /// <param name="controlApp">Controling Application if known</param>
        public static void ProcessJobList(FWBS.OMS.Interfaces.IOMSApp controlApp)
        {
            foreach (PrecedentJob pj in FWBS.OMS.Session.CurrentSession.CurrentPrecedentJobList)
            {
                if (ProcessJob(controlApp, pj) == ProcessJobStatus.PauseJobs)
                    break;
            }

            for (int ctr = FWBS.OMS.Session.CurrentSession.CurrentPrecedentJobList.Count - 1; ctr >= 0; ctr--)
            {
                PrecedentJob job = FWBS.OMS.Session.CurrentSession.CurrentPrecedentJobList[ctr];
                if (job.Completed == true)
                    FWBS.OMS.Session.CurrentSession.CurrentPrecedentJobList.Remove(job);
            }

            //Updates the persisted job list.
            FWBS.OMS.Session.CurrentSession.CurrentPrecedentJobList.Update();
        }


        /// <summary>
        /// Starts a template on a specified OMSApp application and precedent name, merging
        /// with an associate.
        /// </summary>
        /// <param name="controlApp">The application controller to start the template on.</param>
        /// <param name="precName">The precedent code to use.</param>
        /// <param name="assocobj">The associate object to merge with.</param>
        /// <returns>The precedent job that was used foir the template.</returns>
        public static PrecedentJob TemplateStart(FWBS.OMS.Interfaces.IOMSApp controlApp, string precName, FWBS.OMS.Associate assocobj)
        {
            return TemplateStart(controlApp, precName, assocobj, null);
        }

        public static PrecedentJob TemplateStart(FWBS.OMS.Interfaces.IOMSApp controlApp, string precName, FWBS.OMS.Associate assocobj, Common.KeyValueCollection pars)
        {
            // Get the Default Precedent ID if precid is currently a string such as LETTERHEAD
            if (assocobj == null)
            {
                assocobj = SelectAssociate();

                if (assocobj == null) // Associate still not set so return
                    return null;
            }

            FWBS.OMS.Precedent precobj = Precedent.GetDefaultPrecedent(precName, assocobj);

            if (precobj == null)
            {
                throw new OMSException(HelpIndexes.PrecedentNotFound, precName);
            }

            // Call the Overload Method
            return TemplateStart(controlApp, precobj, assocobj, pars);

        }

        /// Starts a template on a specified OMSApp application and precedent name, merging
        /// with an associate.
        /// </summary>
        /// <param name="controlApp">The application controller to start the template on.</param>
        /// <param name="precobj">The precedent object to use.</param>
        /// <param name="assocobj">The associate object to merge with.</param>
        /// <returns>The precedent job that was used foir the template.</returns>
        public static PrecedentJob TemplateStart(FWBS.OMS.Interfaces.IOMSApp controlApp, FWBS.OMS.Precedent precobj, FWBS.OMS.Associate assocobj)
        {
            return TemplateStart(controlApp, precobj, assocobj, null);
        }

        public static PrecedentJob TemplateStart(FWBS.OMS.Interfaces.IOMSApp controlApp, FWBS.OMS.Precedent precobj, FWBS.OMS.Associate assocobj, Common.KeyValueCollection pars)
        {

            //Make sure a precedent object is passed.
            if (precobj == null)
                return null;

            // Get the Default Precedent ID if precid is currently a string such as LETTERHEAD
            if (assocobj == null)
            {
                assocobj = SelectAssociate();

                if (assocobj == null) // Associate still not set so return
                    return null;
            }

            PrecedentJob job = new PrecedentJob(precobj);
            job.AsNewTemplate = true;
            job.PrintMode = PrecPrintMode.None;
            job.SaveMode = PrecSaveMode.None;
            job.Associate = assocobj;

            if (pars != null)
            {
                foreach (string key in pars.Keys)
                {
                    job.Params.Add(key, pars[key].Value);
                }
            }

            ProcessJob(controlApp, job);

            return job;

        }

        #endregion


        #region Run Commands


        /// <summary>
        /// Executes a static method on the services class layer. 
        /// </summary>
        /// <param name="type">An object type to manipulate.</param>
        /// <param name="method">Method to run.</param>
        /// <param name="parameters">Parameters xml file, use null for no parameters.</param>
        /// <param name="replacementParameters">Thje values used to replace %n% parameters.</param>
        /// <returns>Object returned (if any).</returns>
        public static object Run(string type, string method, string parameters, Common.KeyValueCollection replacementParameters)
        {
            Type tpe = Session.CurrentSession.TypeManager.Load(type);
            FWBS.OMS.SourceEngine.StaticMethodSource run = new FWBS.OMS.SourceEngine.StaticMethodSource(tpe, method, parameters);
            run.ChangeParameters(replacementParameters);
            object ret = run.Run();
            return ret;
        }

        /// <summary>
        /// Launches an Enquiry Command from the Database
        /// </summary>
        /// <param name="enquiryCommand">Name of the Enquiry Command</param>
        /// <param name="parent">A parent object to add to the field parsing.</param>
        /// <param name="replacementParameters">Parameters to Replace</param>
        /// <returns>The return object of the Run Command</returns>
        public static object Run(string enquiryCommand, object parent, Common.KeyValueCollection replacementParameters)
        {
            EnquiryCommand rcmd = new EnquiryCommand(enquiryCommand);
            Type tpe = Session.CurrentSession.TypeManager.Load(rcmd.Type);
            FWBS.OMS.SourceEngine.StaticMethodSource run = new FWBS.OMS.SourceEngine.StaticMethodSource(tpe, rcmd.Method, rcmd.Parameters);
            run.ChangeParent(parent);
            run.ChangeParameters(replacementParameters);
            object ret = run.Run();
            return ret;
        }

        /// <summary>
        /// Executes a static method on the services class layer. 
        /// </summary>
        /// <param name="method">Method to invoke.</param>
        /// <returns>Return value, if any.</returns>
        public static object Run(string method)
        {
            return Run(typeof(Services).FullName, method, null, new Common.KeyValueCollection());
        }


        /// <summary>
        /// Execute the specified Data List using the parameters in the KeyValueCollection.
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="kvc"></param>
        /// <returns></returns>
        public static System.Data.DataTable RunDataList(string dataList, FWBS.Common.KeyValueCollection kvc)
        {
            FWBS.OMS.EnquiryEngine.DataLists dl = new FWBS.OMS.EnquiryEngine.DataLists(dataList);
            dl.ChangeParameters(kvc);
            System.Data.DataTable dt = dl.Run(false) as System.Data.DataTable;
            return dt;
        }


        /// <summary>
        /// Execute the specified Data List without parameters.
        /// </summary>
        /// <param name="dataList"></param>
        /// <returns></returns>
        public static System.Data.DataTable RunDataList(string dataList)
        {
            FWBS.OMS.EnquiryEngine.DataLists dl = new FWBS.OMS.EnquiryEngine.DataLists(dataList);
            System.Data.DataTable dt = dl.Run(false) as System.Data.DataTable;
            return dt;
        }



        #endregion

        #region IOMSApp Methods

        /// <summary>
        /// Gets a reference to the current OMS apps created within the windows session.
        /// </summary>
        [Obsolete("Use Apps.ApplicationManager.CurrentManager.InitialiseInstance instead")]
        static public void RegisterOMSApp(string type, FWBS.OMS.Interfaces.IOMSApp controlApp)
        {
            Apps.ApplicationManager.CurrentManager.InitialiseInstance(type, controlApp);
        }

        /// <summary>
        /// Gets a reference to the current OMS apps created within the windows session.
        /// </summary>
        [Obsolete("Use Apps.ApplicationManager.CurrentManager.GetApplicationInstance instead.")]
        static public FWBS.OMS.Interfaces.IOMSApp FetchOMSApp(RegisteredApplication app, bool create)
        {
            return Apps.ApplicationManager.CurrentManager.GetApplicationInstance(app.AppCode, create);
        }

        #endregion


        private static void OnProgress(object sender, FWBS.Common.ProgressEventArgs e)
        {
            if (frmProgress1 != null)
            {
                frmProgress1.CanCancel = false;
                frmProgress1.ProgressBar.Maximum = e.Total;
                frmProgress1.Label = e.Message;
                frmProgress1.ProgressBar.Value = e.Current;
                Application.DoEvents();
            }
        }

        private static void OnProgressStart(object sender, EventArgs e)
        {
            if (frmProgress1 == null)
                frmProgress1 = new frmProgress();
            frmProgress1.Show();
        }

        private static void OnProgressFinished(object sender, EventArgs e)
        {
            if (frmProgress1 != null)
            {
                frmProgress1.Close();
                frmProgress1 = null;
            }
        }
    }


    public class BackForwardMessageFilter : System.Windows.Forms.IMessageFilter
    {
        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            // Captures the Back Button on the Mouse
            if (m.Msg == Convert.ToInt32(Crownwood.Magic.Win32.Msgs.WM_XBUTTONUP))
            {
                if (m.WParam.ToInt32() == 65536)
                {
                    if (BackButtonClicked != null)
                        BackButtonClicked(this, EventArgs.Empty);
                }
                else
                {
                    if (ForwardButtonClicked != null)
                        ForwardButtonClicked(this, EventArgs.Empty);
                }
                return true;
            }
            return false;
        }

        public event EventHandler BackButtonClicked;
        public event EventHandler ForwardButtonClicked;

    }

}
