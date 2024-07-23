using System;
using System.Collections.Generic;
using System.IO;

namespace FWBS.OMS.UI.Windows
{
    using System.Windows.Forms;
    using FWBS.OMS.DocumentManagement;
    using FWBS.OMS.DocumentManagement.Storage;
    using FWBS.OMS.Security.Permissions;
    using Document = OMSDocument;

    /// <summary>
    /// An OMSApp which deals with PDF OMS document types.
    /// </summary>
    [System.Runtime.InteropServices.Guid("1765ebdc-3db7-4fd5-93ea-95da2a192873")]
    class PDF : OMSApp
    {
        public event EventHandler<BulkDocumentProcessArgs> BulkDocumentProcessing;

        protected void OnBulkDocumentProcessing(string name)
        {
            if (BulkDocumentProcessing != null)
                BulkDocumentProcessing(this, new BulkDocumentProcessArgs() { Name = name });
        }


        #region Field Variables
        private System.Windows.Forms.IWin32Window activeWindow = null;
        private BulkDocumentImport _documentsettings;
        private ShellFile _item = null;
        private FileInfo _pdfPrecedent;
        #endregion

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

            ShellFile sf = (ShellFile)obj;

            StorageProvider provider = doc.GetStorageProvider();
            provider.Store(si, sf.File, obj, true, this);
        }

        protected override void InternalPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec)
        {
            throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("MSGOMSDNTSPPR", "PDF OMSApp does not support storing of precedents.", "").Text);
        }

        protected override void InternalPrecedentSave(object obj, PrecSaveMode saveMode, PrecPrintMode printMode, Precedent prec, PrecedentVersion version = null)
        {
            throw new NotSupportedException(Session.CurrentSession.Resources.GetMessage("MSGOMSDNTSPPR", "PDF OMSApp does not support storing of precedents.", "").Text);
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
            ShellFile sf = (ShellFile)obj;
            sf.Save();
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

            ShellFile sf = (ShellFile)obj;

            return sf.GetProperty(varName);
        }

        public override bool HasDocVariable(object obj, string varName)
        {
            CheckObjectIsDoc(ref obj);

            ShellFile sf = (ShellFile)obj;

            return sf.HasProperty(varName);
        }

        public override void RemoveDocVariable(object obj, string varName)
        {
            CheckObjectIsDoc(ref obj);

            ShellFile sf = (ShellFile)obj;

            sf.RemoveProperty(varName);
        }


        protected override bool SetDocVariable(object obj, string varName, object val)
        {
           
            CheckObjectIsDoc(ref obj);

            ShellFile sf = (ShellFile)obj;

            try
            {
                sf.SetProperty(varName, val);
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

            ShellFile sf = (ShellFile)obj;

            return sf.File;
        }

        public override void Close(object obj)
        {
            ShellFile sf = (ShellFile)obj;

            sf.Save();
        }

        public override string DefaultDocType
        {
            get
            {
                return "PDF";
            }
        }

        public override string GetDocExtension(object obj)
        {
            CheckObjectIsDoc(ref obj);
            ShellFile sf = (ShellFile)obj;
            string ext = sf.File.Extension;
            ext = ext.Replace(".", "");
            return ext;
        }

        public override string ExtractPreview(object obj)
        {
            return "";
        }

        protected override string GenerateDocDesc(object obj)
        {
            CheckObjectIsDoc(ref obj);

            ShellFile sf = (ShellFile)obj;

            switch (GetActiveDocType(obj))
            {
                case "SHELL":
                case "PDF":
                    return Path.GetFileNameWithoutExtension(sf.File.Name);
                default:
                    return base.GenerateDocDesc(obj);
            }
        }


        public override string ModuleName
        {
            get
            {
                return "PDF Document Application";
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
                throw new System.IO.FileNotFoundException(Session.CurrentSession.Resources.GetMessage("MSGFLDNTEXT", "File does not exist.", "").Text, file.FullName);

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


        protected override object TemplateStart(object obj, PrecedentLink preclink)
        {
            System.IO.FileInfo tmppath = null;

            //Get the Precedent File to Load...
            FWBS.OMS.DocumentManagement.Storage.FetchResults fetch = preclink.Merge();

            if (fetch == null)
                return null;
            
            tmppath = fetch.LocalFile;

            CreatePDFDocument(obj, preclink, tmppath);

            return _pdfPrecedent;
        }


        private object CreatePDFDocument(object obj, PrecedentLink preclink, System.IO.FileInfo tmppath)
        {
            if (tmppath != null)
            {
                switch (preclink.Precedent.PrecedentType.ToUpper())
                {
                    case "PDF":
                        OMSDocument newPDFDocument = CreateNewPDFDocument(obj, preclink, tmppath);
                        OpenDocument(newPDFDocument);
                        break;
                }
                return true;
            }
            else
                return false;
        }

        #endregion

        #region Methods

        protected override void CheckObjectIsDoc(ref object obj)
        {
            if (obj is ShellFile)
                obj = (ShellFile)obj;
            else if (obj is System.IO.FileInfo)
                obj = new ShellFile(obj as System.IO.FileInfo);
            else if (obj is PDF)
                obj = (PDF)obj;
            else
                throw new Exception(Session.CurrentSession.Resources.GetMessage("MSGPRMNTACPT", "The passed parameter is not an acceptable PDF document object.", "").Text);
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
            _documentsettings = documentsettings;
            List<Exception> errors = new List<Exception>();
            List<string> errnames = new List<string>();
            settings.UsePreviousAssoicate = true;
            int success = 0;
            int fails = 0;
            try
            {
                OnProgressStart("Multiple Document Save", "Saving selected documents...", objs.Length, true);
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

                    if (cancel) return;
                }

                string msg = "#" + Session.CurrentSession.Resources.GetResource("SAVESUCCCOMP", "%1% file(s) added successfully" + Environment.NewLine + "%2% file(s) failed", "", success.ToString(), fails.ToString()).Text;
                OnBulkDocumentProcessing(msg);

            }
            finally
            {
                OnProgressFinished();
                if (fails > 0)
                {
                    if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("FAILBOX", "Errors have occured. Do you wish to view them?") == DialogResult.Yes)
                    {
                        int i = 0;
                        foreach (var item in errors)
                        {
                            Exception e = new Exception(Session.CurrentSession.Resources.GetMessage("MSGERRINSVG", "Error in Saving %1%\n\n%2%", "", errnames[i], item.Message).Text, item);
                            ErrorBox.Show(e);
                            i++;
                        }
                    }
                }
            }
            _documentsettings = null;
        }

        public void SaveAs(ShellFile[] objs, SaveSettings settings, BulkDocumentImport documentsettings)
        {
            _documentsettings = documentsettings;
            // Check System Security
            new SystemPermission(StandardPermissionType.SaveDocument).Check();
            List<Exception> errors = new List<Exception>();
            List<string> errnames = new List<string>();
            int fails = 0;

            try
            {
                Associate assoc = null;
                if (settings.UseDefaultAssociate)
                    assoc = Services.SelectDefaultAssociate(ActiveWindow);
                else
                    assoc = Services.SelectAssociate(ActiveWindow);

                if (assoc != null)
                {
                    try
                    {
                        OnProgressStart("Multiple Document Save", "Saving selected documents...", objs.Length, true);

                        int ctr = 0;
                        int success = 0;
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

                            if (cancel) return;

                        }

                        string msg = "#" + Session.CurrentSession.Resources.GetResource("SAVESUCCCOMP", "%1% files added successfully" + Environment.NewLine + "%2% files failed", "", success.ToString(), fails.ToString()).Text;
                        OnBulkDocumentProcessing(msg);

                    }
                    finally
                    {
                        OnProgressFinished();
                        if (fails > 0)
                        {
                            if (FWBS.OMS.UI.Windows.MessageBox.ShowYesNoQuestion("FAILBOX", "Errors have occured. Do you wish to view them?") == DialogResult.Yes)
                            {
                                int i = 0;
                                foreach (var item in errors)
                                {
                                    Exception e = new Exception(Session.CurrentSession.Resources.GetMessage("MSGERRINSVG", "Error in Saving %1%\n\n%2%", "", errnames[i], item.Message).Text, item);
                                    ErrorBox.Show(e);
                                    i++;
                                }
                            }
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
            base.BeforeDocumentSave(obj, doc, version);
        }



        /// <summary>
        /// Saves the PDF Precedent as a OMSDocument (with option to Open)
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="preclink"></param>
        /// <param name="tmppath"></param>
        private FWBS.OMS.OMSDocument CreateNewPDFDocument(object obj, PrecedentLink preclink, System.IO.FileInfo tmppath)
        {
            FileInfo pdfDocument = TemporaryPDFFile(tmppath);
            using (ShellFile sf = new ShellFile(pdfDocument))
            {
                DettachPrecedentVars(sf);
                DettachDocumentVars(sf);
                sf.Save();
            }
            SaveSettings settings = PDFDocumentSaveSettings(preclink);
            Save(pdfDocument, settings);

            OMSDocument doc = FWBS.OMS.UI.Windows.OMSApp.GetDocument(new System.IO.FileInfo(pdfDocument.FullName));
            return doc;
        }

        private static FileInfo TemporaryPDFFile(System.IO.FileInfo tmppath)
        {
            FileInfo pdfPrecedent = new FileInfo(tmppath.FullName);
            string tempFileName = Path.ChangeExtension(FWBS.OMS.Global.GetTempFile().FullName, "pdf");
            FileInfo pdfDocument = pdfPrecedent.CopyTo(tempFileName, true);
            return pdfDocument;
        }

        private static SaveSettings PDFDocumentSaveSettings(PrecedentLink preclink)
        {
            SaveSettings settings = SaveSettings.Default;
            settings.Printing.Mode = PrecPrintMode.None;
            settings.Mode = PrecSaveMode.Quick;
            settings.DocumentDescription = preclink.Precedent.Description;
            settings.UsePreviousAssoicate = false;
            settings.TargetAssociate = preclink.Associate;
            return settings;
        }

        
        /// <summary>
        /// Opens an OMS document based on the document's file path
        /// </summary>
        /// <param name="DocumentFilePath"></param>
        /// <returns></returns>
        private bool OpenDocument(FWBS.OMS.OMSDocument Document)
        {
            if (Document == null) 
                return false;
            else
            {
                Services.OpenDocument(Document, DocOpenMode.Edit);
                return true;
            }
        }

    }
}
