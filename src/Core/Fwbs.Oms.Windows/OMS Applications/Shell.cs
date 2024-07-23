using System;
using System.Collections.Generic;

namespace FWBS.OMS.UI.Windows
{
    using System.Windows.Forms;
    using FWBS.OMS.DocumentManagement;
    using FWBS.OMS.DocumentManagement.Storage;
    using FWBS.OMS.Security.Permissions;
    using FWBS.OMS.UI.OMS_Applications;
    using Document = OMSDocument;

    /// <summary>
    /// An OMSApp which deals with Shell files document types.
    /// </summary>
    [System.Runtime.InteropServices.Guid("15573BF1-C9C5-48d8-81C9-0BB287615C9B")]
    public class ShellOMS : OMSApp
    {
        private int fails;

        public event EventHandler<BulkDocumentProcessArgs> BulkDocumentProcessing;

        public event EventHandler<BulkDocumentProcessCompletedArgs> BulkDocumentProcessCompleted;

        public event EventHandler BulkDocumentProcessCancelled;

        protected void OnBulkDocumentProcessing(string name)
        {
            if (BulkDocumentProcessing != null)
                BulkDocumentProcessing(this, new BulkDocumentProcessArgs() { Name = name });
        }

        protected void OnBulkDocumentProcessCompleted(List<string> errnames, List<Exception> errors)
        {
            if (BulkDocumentProcessCompleted != null)
                BulkDocumentProcessCompleted(this, new BulkDocumentProcessCompletedArgs(errnames, errors));
        }

        protected void OnBulkDocumentProcessCancelled(bool cancel)
        {
            if (BulkDocumentProcessCancelled != null)
                BulkDocumentProcessCancelled(this, EventArgs.Empty);
        }

        #region Field Variables
        private BulkDocumentImport _documentsettings;
        private ShellFile _item = null;
        private System.Windows.Forms.IWin32Window activeWindow = null;
        public static readonly DPIAwareness.DPI_AWARENESS DpiAwareness = DPIAwareness.DPI_AWARENESS.UNAWARE;

        #endregion

        static ShellOMS()
        {
            var officeApps = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "WINWORD", "EXCEL", "OUTLOOK" };
            if (officeApps.Contains(System.Diagnostics.Process.GetCurrentProcess().ProcessName) &&
                AppDomain.CurrentDomain.GetData("DPI_AWARENESS") != null)
            {
                DpiAwareness = (DPIAwareness.DPI_AWARENESS)AppDomain.CurrentDomain.GetData("DPI_AWARENESS");
            }
        }

        #region Save Methods


        protected override FWBS.OMS.Apps.RegisteredApplication GetRegisteredApplication(object obj, string extension)
        {
            CheckObjectIsDoc(ref obj);

            Apps.RegisteredApplication reg = Apps.ApplicationManager.CurrentManager.GetRegisteredApplicationByExtension(extension);
            if (reg == null)
                return base.GetRegisteredApplication(obj, extension);
            else
                return reg;
        }

        protected override void InternalDocumentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Document doc, DocumentVersion version)
        {
            CheckObjectIsDoc(ref obj);

            IStorageItem si = version;
            if (si == null)
                si = doc;

            ShellFile sf = _item;

            StorageProvider provider = doc.GetStorageProvider();
            provider.Store(si, sf.File, obj, true, this);
        }

        protected override void InternalPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec)
        {
            throw new NotSupportedException(Session.CurrentSession.Resources.GetResource("SHELL_1", "Shell OMS does not support storing of precedents.", "").Text);
        }

        protected override void InternalPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec, PrecedentVersion version)
        {
            throw new NotSupportedException(Session.CurrentSession.Resources.GetResource("SHELL_1", "Shell OMS does not support storing of precedents.", "").Text);
        }


        protected override SubDocument[] AttachSubDocuments(object obj)
        {
            CheckObjectIsDoc(ref obj);
            ShellFile sf = _item;
            SubDocument[] doc = new SubDocument[sf.Attachments.GetLength(0)];
            for (int ctr = 0; ctr < sf.Attachments.GetLength(0); ctr++)
            {
                System.IO.FileInfo f = sf.Attachments[ctr];
                doc[ctr] = new SubDocument(f.Name, f);
            }
            return doc;
        }

        #endregion

        #region Print Methods

        public override void InternalPrint(object obj, int copies)
        {
            for (int ctr = 1; ctr <= copies; ctr++)
            {
                ShellFile sf = (ShellFile)obj;
                Services.ProcessStart("OMS.UTILS.EXE", string.Format("PRINT \"{0}\"", sf.File.FullName), InputValidation.ValidatePrintFileInput);
            }
        }

        protected override void InternalSave(object obj)
        {
            CheckObjectIsDoc(ref obj);
            ActiveFile.Save();
        }

        #endregion

        #region Field Routines

        public override void AddField(object obj, string name)
        {
            return;
        }

        public override int GetFieldCount(object obj)
        {
            return 0;
        }

        public override string GetFieldName(object obj, int index)
        {
            return null;
        }

        public override object GetFieldValue(object obj, int index)
        {
            return null;
        }

        public override object GetFieldValue(object obj, string name)
        {
            return null;
        }

        public override void CheckFields(object obj)
        {

        }

        public override void SetFieldValue(object obj, int index, object val)
        {

        }

        public override void SetFieldValue(object obj, string name, object val)
        {

        }

        #endregion

        #region Document Variable Routines


        protected override object GetDocVariable(object obj, string varName)
        {
            CheckObjectIsDoc(ref obj);
            return ActiveFile.GetProperty(varName);
        }

        public override bool HasDocVariable(object obj, string varName)
        {
            CheckObjectIsDoc(ref obj);
            return ActiveFile.HasProperty(varName);
        }

        public override void RemoveDocVariable(object obj, string varName)
        {
            CheckObjectIsDoc(ref obj);
            ActiveFile.RemoveProperty(varName);
        }


        protected override bool SetDocVariable(object obj, string varName, object val)
        {
            CheckObjectIsDoc(ref obj);
            try
            {
                ActiveFile.SetProperty(varName, val);
                return true;
            }
            catch
            {
                return false;
            }
        }


        #endregion

        #region IOMSApp Implementation

        protected override System.IO.FileInfo GetCurrentFileLocation(object obj)
        {
            CheckObjectIsDoc(ref obj);
            return ActiveFile.File;
        }

        public override void Close(object obj)
        {
            ActiveFile.Save();
        }

        public override string DefaultDocType
        {
            get
            {
                return "SHELL";
            }
        }

        public override string GetDocExtension(object obj)
        {
            CheckObjectIsDoc(ref obj);
            string ext = ActiveFile.File.Extension;
            ext = ext.Replace(".", "");
            return ext;
        }

        public override string ExtractPreview(object obj)
        {
            return "";
        }

        protected override string GenerateDocDesc(object obj)
        {
            switch (GetActiveDocType(obj))
            {
                case "SHELL":
                    return System.IO.Path.GetFileNameWithoutExtension(ActiveFile.Name);
                default:
                    return base.GenerateDocDesc(obj);
            }
        }


        public override string ModuleName
        {
            get
            {
                return "Shell File Document Application";
            }
        }

        public override System.Windows.Forms.IWin32Window ActiveWindow
        {
            get
            {
                return activeWindow;
            }

        }

        public void SetActiveWindow(System.Windows.Forms.IWin32Window window)
        {
            activeWindow = window;
        }

        protected override object OpenFile(System.IO.FileInfo file)
        {
            ShellFile sf = new ShellFile(file, null);
            Services.ProcessStart("OMS.UTILS.EXE", string.Format("OPEN \"{0}\"", file.FullName), InputValidation.ValidateOpenFileInput);
            return sf;
        }

        protected override object InternalDocumentOpen(Document document, FetchResults fetchData, OpenSettings settings)
        {
            System.IO.FileInfo file = fetchData.LocalFile;

            if (System.IO.File.Exists(file.FullName) == false)
                throw new System.IO.FileNotFoundException(Session.CurrentSession.Resources.GetResource("SHELL_2", "File does not exist.", "").Text, file.FullName);

            ShellFile sf = new ShellFile(file, null);

            switch (settings.Mode)
            {
                case DocOpenMode.Edit:
                    Services.ProcessStart("OMS.UTILS.EXE", string.Format("OPEN \"{0}\"", sf.File.FullName), InputValidation.ValidateOpenFileInput);
                    return sf;
                case DocOpenMode.Print:
                    object[] docs = new object[1];
                    docs[0] = sf;
                    BeginPrint(docs, settings.Printing);
                    return sf;
                case DocOpenMode.View:
                    Services.ProcessStart("OMS.UTILS.EXE", string.Format("OPEN \"{0}\"", sf.File.FullName), InputValidation.ValidateOpenFileInput);
                    return sf;
            }
            return null;

        }

    

        protected override object InternalPrecedentOpen(Precedent precedent, FetchResults fetchData, OpenSettings settings)
        {

            System.IO.FileInfo file = fetchData.LocalFile;

            ShellFile sf = new ShellFile(file, null);

            switch (settings.Mode)
            {
                case DocOpenMode.Edit:
                    Services.ProcessStart("OMS.UTILS.EXE", string.Format("OPEN \"{0}\"", sf.File.FullName), InputValidation.ValidateOpenFileInput);
                    return sf;
                case DocOpenMode.Print:
                    object[] docs = new object[1];
                    docs[0] = sf;
                    BeginPrint(docs, settings.Printing);
                    return sf;
                case DocOpenMode.View:
                    Services.ProcessStart("OMS.UTILS.EXE", string.Format("OPEN \"{0}\"", sf.File.FullName), InputValidation.ValidateOpenFileInput);
                    return sf;
            }
            return null;

        }

        protected override DocSaveStatus BeginSave(object obj, SaveSettings settings)
        {
            using (var contextBlock = DpiAwareness > 0 ? new DPIContextBlock(DpiAwareness) : null)
            {
                return base.BeginSave(obj, settings);
            }
        }

        protected override object TemplateStart(object obj, PrecedentLink preclink)
        {
            throw new NotSupportedException(Session.CurrentSession.Resources.GetResource("SHELL_3", "A Shell Document Cannot be created from a template.", "").Text);
        }

        #endregion

        #region Methods


        protected override void CheckObjectIsDoc(ref object obj)
        {
            if (obj == this)
                obj = ActiveFile;
            else if (obj is ShellFile)
                ActiveFile = (ShellFile)obj;
            else if (obj is System.IO.FileInfo)
                ActiveFile = new ShellFile(obj as System.IO.FileInfo);
            else
                throw new Exception(Session.CurrentSession.Resources.GetResource("SHELL_4", "The passed parameter is not an acceptable shell document object.", "").Text);
            obj = ActiveFile;
        }

        protected override void InsertText(object obj, PrecedentLink precLink)
        {
        }


        protected override void SetWindowCaption(object obj, string caption)
        {
            CheckObjectIsDoc(ref obj);
        }

        public void Save(ShellFile[] objs, SaveSettings settings, BulkDocumentImport documentsettings)
        {
            List<string> errnames = new List<string>();
            List<Exception> errors = new List<Exception>(); 

            _documentsettings = documentsettings;
            settings.UsePreviousAssoicate = true;
            int success = 0;
            fails = 0;
            try
            {
                OnProgressStart(Session.CurrentSession.Resources.GetResource("SHELL_5", "Multiple Document Save", "").Text,
                                Session.CurrentSession.Resources.GetResource("SHELL_6", "Saving selected documents...", "").Text, objs.Length, true);
                int ctr = 0;
                foreach (ShellFile obj in objs)
                {
                    ctr++;
                    bool cancel = false;
                    DocSaveStatus status = DocSaveStatus.Error;
                    try
                    {
                        Application.DoEvents();
                        OnProgress(String.Format("Saving - {0} ...", obj.Name), ctr, out cancel);
                        OnBulkDocumentProcessing(obj.Name);
                        Application.DoEvents();

                        status = BeginSave(obj, settings);
                        
                        Application.DoEvents();
                        if (status == DocSaveStatus.Error)
                        {
                            DettachDocumentVars(obj);
                            fails++;
                            errnames.Add(obj.Name);
                            errors.Add(new Exception(""));
                            OnProgress(String.Format("Error - {0} ...", obj.Name), ctr, out cancel);
                        }
                        else if (status == DocSaveStatus.Success)
                        {
                            success++;
                        }
                        Application.DoEvents();
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex);
                        errnames.Add(obj.Name);
                        Application.DoEvents();
                        OnProgress(String.Format("Error - {0} ...", obj.Name), ctr);
                        fails++;
                        Application.DoEvents();
                    }
                    finally
                    {
                    }

                    if (cancel)
                    {
                        OnBulkDocumentProcessCancelled(cancel);
                        return;
                    }
                }

                string msg = "#" + Session.CurrentSession.Resources.GetResource("SAVESUCCCOMP", "%1% file(s) added successfully" + Environment.NewLine + "%2% file(s) failed", "", success.ToString(), fails.ToString()).Text;
                OnBulkDocumentProcessing(msg);

            }
            finally
            {
                OnProgressFinished();
                if (fails > 0)
                {
                    OnBulkDocumentProcessCompleted(errnames, errors);
                }
            }
            _documentsettings = null;
        }


        public void SaveAs(ShellFile[] objs, SaveSettings settings, BulkDocumentImport documentsettings)
        {
            List<string> errnames = new List<string>();
            List<Exception> errors = new List<Exception>();

            _documentsettings = documentsettings;
            // Check System Security
            new SystemPermission(StandardPermissionType.SaveDocument).Check();

            fails = 0;

            try
            {
                bool cancel = false;
                Associate assoc = null;

                if (settings.TargetAssociate != null)
                {
                    assoc = settings.TargetAssociate;
                }
                else
                {
                    if (settings.UseDefaultAssociate)
                        assoc = Services.SelectDefaultAssociate(ActiveWindow);
                    else
                        assoc = Services.SelectAssociate(ActiveWindow);
                }

                if (assoc != null)
                {
                    try
                    {
                        OnProgressStart(Session.CurrentSession.Resources.GetResource("SHELL_5", "Multiple Document Save", "").Text,
                                        Session.CurrentSession.Resources.GetResource("SHELL_6", "Saving selected documents...", "").Text, objs.Length, true);

                        int ctr = 0;
                        int success = 0;
                        foreach (ShellFile obj in objs)
                        {
                            ctr++;
                            DocSaveStatus status = DocSaveStatus.Error;

                            try
                            {

                                Application.DoEvents();
                                OnProgress(String.Format("Saving - {0} ...", obj.Name), ctr, out cancel);
                                OnBulkDocumentProcessing(obj.Name);
                                Application.DoEvents();


                                var assocrl = GetCurrentAssociate(obj);

                                DettachDocumentVars(obj);
                                DettachPrecedentVars(obj);

                                if (assocrl != null)
                                    SetDocVariable(obj, RELINK, assocrl.ID);
                                else
                                    RemoveDocVariable(obj, RELINK);

                                AttachDocumentVars(obj, settings.UseDefaultAssociate, assoc);
                                settings.AllowRelink = false;
                                status = BeginSave(obj, settings);
                                Application.DoEvents();
                                if (status == DocSaveStatus.Error)
                                {
                                    DettachDocumentVars(obj);
                                    fails++;
                                    errnames.Add(obj.Name);
                                    errors.Add(new Exception());
                                    OnProgress(String.Format("Error - {0} ...", obj.Name), ctr, out cancel);
                                }
                                else if (status == DocSaveStatus.Success)
                                {
                                    success++;
                                }
                                Application.DoEvents();
                            }
                            catch (Exception ex)
                            {
                                errors.Add(ex);
                                errnames.Add(obj.Name);
                                Application.DoEvents();
                                OnProgress(String.Format("Error - {0} ...", obj.Name), ctr);
                                Application.DoEvents();
                                fails++;
                            }
                            finally
                            {
                            }

                            if (cancel)
                            {
                                OnBulkDocumentProcessCancelled(cancel);
                                return;
                            }
                        }

                        string msg = "#" + Session.CurrentSession.Resources.GetResource("SAVESUCCCOMP", "%1% files added successfully" + Environment.NewLine + "%2% files failed","",success.ToString(), fails.ToString()).Text;
                        OnBulkDocumentProcessing(msg);

                    }
                    finally
                    {
                        OnProgressFinished();
                        if (fails > 0)
                        {
                            OnBulkDocumentProcessCompleted(errnames, errors);
                        }
                    }
                }
            }
            finally
            {
                _documentsettings = null;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the active file.
        /// </summary>
        public ShellFile ActiveFile
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;

            }
        }

        #endregion

        protected override void BeforeDocumentSave(object obj, OMSDocument doc, DocumentVersion version)
        {
            if (_documentsettings == null)
                return;

            string _filename = ActiveFile.File.FullName;
            foreach (var prop in _documentsettings.GetType().GetProperties())
            {
                object value = prop.GetValue(_documentsettings, null);
                string strvalue = value as string;
                if (strvalue != null)
                {
                    strvalue = strvalue.Replace("%filename%", System.IO.Path.GetFileName(_filename));
                    strvalue = strvalue.Replace("%namenoext%", System.IO.Path.GetFileNameWithoutExtension(_filename));
                    strvalue = strvalue.Replace("%fullpath%", System.IO.Path.GetFullPath(_filename));
                    strvalue = strvalue.Replace("%extention%", System.IO.Path.GetExtension(_filename));
                    strvalue = strvalue.Replace("%foldername%", System.IO.Path.GetDirectoryName(_filename));
                    value = strvalue;
                }
                var setprop = doc.GetType().GetProperty(prop.Name);
                if (setprop != null && setprop.CanWrite)
                    setprop.SetValue(doc, value, null);
            }
            if (_documentsettings.SkipTime)
            {
                doc.TimeRecords.SkipTime = true;
            }
            else
            {
                if (doc.TimeRecords.Count == 0)
                    doc.TimeRecords.SkipTime = true;

                foreach (TimeRecord item in doc.TimeRecords)
                {
                    if (item.TimeUnits == 0)
                        doc.TimeRecords.SkipTime = true;
                }
            }
            if (_documentsettings.AuthoredBy != null)
            {
                doc.Author = _documentsettings.AuthoredBy;
            }
            base.BeforeDocumentSave(obj, doc, version);
        }

    }


}