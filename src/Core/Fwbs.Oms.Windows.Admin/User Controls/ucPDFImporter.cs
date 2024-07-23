using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using FWBS.OMS.Data;
using FWBS.OMS.Data.Exceptions;


namespace FWBS.OMS.UI.Windows.Admin
{
    public partial class ucPDFImporter : UserControl, IOBjectDirty
    {
        #region "Global Variables"

        //Global variables
        ListView importList;
        private int precedentsAdded = 0;
        System.Collections.Generic.List<string> failedFiles = new System.Collections.Generic.List<string>();
        const string precSubDirectory = @"PDF Templates";
        string precDirectory;
        int existingFiles = 0;
        string msgBoxCaption = "";
        System.Collections.Generic.List<string> skippedFiles = new System.Collections.Generic.List<string>();

        #endregion 
        
        #region "ucPDFImporter Events"

        public ucPDFImporter()
        {
            InitializeComponent();
        }
        
        private void ucPDFImporter_Load(object sender, EventArgs e)
        {
            CreateCodeLookups();
            CheckPrecedentDirectoryExists();
            CreateListView();
            precDirectory = PDFPrecedentDirectoryName();
            this.chkOverwriteAll.Enabled = false;
            SetCaptions();
            msgBoxCaption = ResourceLookup.GetLookupText("PDFI_CAP");
        }

        #endregion

        #region "Other Events"

        private void btnSelect_Click(object sender, System.EventArgs e)
        {
            try
            {
                SelectPDFsRoutine();
            }
            catch (System.Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void OpenDialog_FileOK(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                if (((OpenFileDialog)sender).FileName.Contains(precDirectory))
                {
                    string msgImport = ResourceLookup.GetLookupText("PDFI_QIMP");

                    DialogResult res = System.Windows.Forms.MessageBox.Show(msgImport, msgBoxCaption, MessageBoxButtons.YesNo);
                    if (res == DialogResult.No)
                        e.Cancel = true;
                }
            }
            catch
            { }
        }
        
        private void btnImport_Click(object sender, System.EventArgs e)
        {
            try
            {
                ImportRoutine();
            }
            catch (System.Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        private void btnClear_Click(object sender, System.EventArgs e)
        {
            try
            {
                ClearListView(true);
            }
            catch (System.Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }

        #endregion
        
        #region "Methods"

        public string PDFPrecedentDirectoryName()
        {
            string _path = FWBS.OMS.Session.CurrentSession.GetSystemDirectory(FWBS.OMS.SystemDirectories.OMPrecedents).FullName;

            if (!_path.EndsWith(@"\"))
                _path += @"\";
            _path += precSubDirectory;

            return _path;
        }


        public void CreateListView()
        {
            CreateImportList();
            ConfigureImportButtons();
        }


        private void CreateImportList()
        {
            importList = (ListView)this.lvPDFList;
            importList.Clear();

            if (importList.Columns.Count == 0)
                importList.Columns.Add("Name");

            importList.Columns[0].Width = importList.Width - 10;
        }
        
       
        public void SelectPDFsRoutine()
        {
            string[] pdfFiles = GetPDFFiles();

            if (pdfFiles == null)
                return;

            System.Diagnostics.Debug.WriteLine(string.Format("Number of files selected to import ({0})", pdfFiles.Length), "PDF Import");

            if (pdfFiles.Length > 0)
                AddSelectedPDFsToList(pdfFiles);
        }


        private void AddSelectedPDFsToList(string[] pdfFiles)
        {
            string _destinationFile;

            foreach (string pdfFile in pdfFiles)
            {
                importList.Items.Add(Path.GetFileNameWithoutExtension(pdfFile)).Tag = pdfFile;

                _destinationFile = precDirectory + @"\" + Path.GetFileName(pdfFile);
                System.Diagnostics.Debug.WriteLine(string.Format("Destination File({0})", _destinationFile), "PDF Import");

                if (new FileInfo(_destinationFile).Exists)
                {
                    importList.Items[importList.Items.Count - 1].ForeColor = System.Drawing.Color.Red;
                    existingFiles++;
                }

                ConfigureImportButtons();
            }
        }


        public string[] GetPDFFiles()
        {
            System.Diagnostics.Debug.WriteLine("PDF File Names", "PDF IMPORT");
            OpenFileDialog fileDialog = PDFOpenFileDialog();

            //Call the ShowDialog method to show the dialog box.
            bool userClickedOK = (fileDialog.ShowDialog() == DialogResult.OK);

            if (userClickedOK)
                return fileDialog.FileNames;
            else
                return null;
        }


        private OpenFileDialog PDFOpenFileDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "PDF files (.pdf)|*.pdf";
            fileDialog.FilterIndex = 1;
            fileDialog.Multiselect = true;

            //Declare event
            fileDialog.FileOk -= new System.ComponentModel.CancelEventHandler(OpenDialog_FileOK);
            fileDialog.FileOk += new System.ComponentModel.CancelEventHandler(OpenDialog_FileOK);

            return fileDialog;
        }

        
        public void ImportRoutine()
        {
            precedentsAdded = 0;

            try
            {
                if (importList.Items.Count == 0)
                {
                    string msgSelectPDF = ResourceLookup.GetLookupText("PDFI_QSEL");
                    System.Windows.Forms.MessageBox.Show(msgSelectPDF, msgBoxCaption);
                    return;
                }

                bool allImported = CreatePDFPrecedents();
                System.Diagnostics.Debug.WriteLine("(" + precedentsAdded + ") precedents added", "PDF Import");
                ImportReportRoutine(allImported);
            }
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
            }
        }


        private void ImportReportRoutine(bool allImported)
        {
            if (allImported)
            {
                string msgSuccess = GenerateSuccessReport();
                System.Windows.Forms.MessageBox.Show(msgSuccess, msgBoxCaption);
                existingFiles = 0;
                CreateListView();
            }
            else
            {
                string report = GenerateSkippedAndFailedReport();
                System.Windows.Forms.MessageBox.Show(report, msgBoxCaption);
            }
        }

        private string GenerateSuccessReport()
        {
            string msgSuccess = ResourceLookup.GetLookupText("PDFI_QSUC");

            if (skippedFiles.Count > 0)
            {
                msgSuccess += "\n\n";
                string msgNotReplaced = ResourceLookup.GetLookupText("PDFI_NREP");

                msgSuccess += string.Format(msgNotReplaced, skippedFiles.Count);

                foreach (string skippedFile in skippedFiles)
                {
                    msgSuccess += "* " + skippedFile + "\n";
                }
            }
            return msgSuccess;
        }

        private string GenerateSkippedAndFailedReport()
        {
            string msgReport = ResourceLookup.GetLookupText("PDFI_QREP");
            string report = string.Format(msgReport, precedentsAdded, failedFiles.Count);

            report = SkippedAndFailedFiles(report);
            return report;
        }


        private string SkippedAndFailedFiles(string report)
        {
            foreach (string missedFile in failedFiles)
                report += "* " + missedFile + "\n";

            if (skippedFiles.Count > 0)
            {
                report += "\n";
                string msgNotReplaced = ResourceLookup.GetLookupText("PDFI_NREP");
                report += string.Format(msgNotReplaced, skippedFiles.Count);

                foreach (string skippedFile in skippedFiles)
                    report += "* " + skippedFile + "\n";
            }
            return report;
        }


        public bool CreatePDFPrecedents()
        {
            importList.Update();
            bool success = true;
            ClearFailedAndSkippedFiles();

            if (importList.Items.Count > 0)
            {
                foreach (ListViewItem item in importList.Items)
                    success = ImportFromList(success, item);
            }
            return success;
        }

        private bool ImportFromList(bool success, ListViewItem item)
        {
            string fullPath = Convert.ToString(item.Tag);
            string fileName = Convert.ToString(item.Text);

            System.Diagnostics.Debug.WriteLine(fullPath, "PDF Import");

            //Only import if file does not exist
            if (item.ForeColor != System.Drawing.Color.Red)
            {
                if (!CreatePDFPrecedentFromFile(fullPath, fileName))
                    success = false;
            }
            else
                 ImportAndOverwrite(item, fullPath, fileName);
            
            return success;
        }


        private void ImportAndOverwrite(ListViewItem item, string fullPath, string fileName)
        {
            bool _overwrite = this.chkOverwriteAll.Checked;

            if (!_overwrite)
            {
                string msgWarning = ResourceLookup.GetLookupText("PDFI_QWRN");
                string warningText = string.Format(msgWarning, item.Text + ".pdf");
                DialogResult res = System.Windows.Forms.MessageBox.Show(warningText, msgBoxCaption, MessageBoxButtons.YesNo);

                if (res == DialogResult.No)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("User chose to not replace file {0} as it already exists", item.Tag + ".pdf"), "PDF IMPORT");
                    skippedFiles.Add(fileName);
                }
                else
                    CopyPDFAndUpdatePrecedent(fullPath);
            }
            else
            {
                CopyPDFAndUpdatePrecedent(fullPath);
                System.Diagnostics.Debug.WriteLine(string.Format("Auto-copy, overwrite and update precedent for file {0}.", item.Tag + ".pdf"), "PDF IMPORT");
            }
        }

        private void ClearFailedAndSkippedFiles()
        {
            if (failedFiles != null)
                failedFiles.Clear();

            if (skippedFiles != null)
                skippedFiles.Clear();
        }


        public void CopyPDFAndUpdatePrecedent(string originalPath)
        {
            string _newPath = CopyPDFToPrecedentLocation(originalPath, precDirectory, true);
            //Update underlying precedent object
            string _precPath = _newPath.Substring(_newPath.IndexOf(precSubDirectory));
            UpdatePrecedent(_precPath);
        }


        public void UpdatePrecedent(string PrecedentPath)
        {
            DataTable dt = GetPrecedentsBasedOnFilePath(PrecedentPath);
            if (dt != null)
            {
                FWBS.OMS.Precedent prec;
                foreach (DataRow dr in dt.Rows)
                    prec = UpdatedPDFPrecedent(dr);
            }
        }

        private static DataTable GetPrecedentsBasedOnFilePath(string PrecedentPath)
        {
            System.Diagnostics.Debug.WriteLine(string.Format("Get Precedents with precPath of {0}", PrecedentPath), "PDFIMPORT");
            Session.CurrentSession.CheckLoggedIn();
            IDataParameter[] pars = new IDataParameter[1];
            pars[0] = Session.CurrentSession.CurrentConnection.CreateParameter("PRECPATH", PrecedentPath);
            DataTable dt = Session.CurrentSession.CurrentConnection.ExecuteSQL("SELECT precID from dbPrecedents WHERE precPath = @PRECPATH", pars);
            return dt;
        }

        private FWBS.OMS.Precedent UpdatedPDFPrecedent(DataRow dr)
        {
            FWBS.OMS.Precedent prec;
            prec = FWBS.OMS.Precedent.GetPrecedent((long)dr["precID"]);

            //WI.2825 - Include categories also on precedent update
            if (Session.CurrentSession.GetSpecificData("PDFI_UPDCAT").ToString().ToUpper() == "TRUE")
                prec.Category = this.xpPrecCat.Value.ToString();
            if (Session.CurrentSession.GetSpecificData("PDFI_UPDSUBCAT").ToString().ToUpper() == "TRUE")
                prec.SubCategory = this.xpPrecSubCat.Value.ToString();
            if (Session.CurrentSession.GetSpecificData("PDFI_UPDMINORCAT").ToString().ToUpper() == "TRUE")
                prec.MinorCategory = this.xpPrecMinorCat.Value.ToString();

            prec.SetExtraInfo("updated", DateTime.Now);
            prec.SetExtraInfo("updatedby", Session.CurrentSession.CurrentUser.ID);
            prec.Update();
            System.Diagnostics.Debug.WriteLine(string.Format("Update to precedent [{0}] {1}", prec.ID, prec.Title), "PDF IMPORT");
            return prec;
        }


        public bool CreatePDFPrecedentFromFile(string FullPath, string FileName)
        {
            bool _precCreated = false;

            string title;
            FWBS.OMS.Precedent precedent;
            NewPDFPrecedent(FileName, out title, out precedent);
            
            FullPath = CopyPDFToPrecedentLocation(FullPath, precDirectory, false);
            _precCreated = UpdateNewPDFPrecedent(FullPath, _precCreated, precedent);

            AddFailedFiles(FileName, _precCreated, title);

            System.Diagnostics.Debug.WriteLine(string.Format("All precedents created? = {0}", _precCreated), "PDF Import");
            return _precCreated;
        }


        private bool UpdateNewPDFPrecedent(string FullPath, bool _precCreated, FWBS.OMS.Precedent precedent)
        {
            try
            {
                _precCreated = UpdateNewPDFPrecedentFile(FullPath, _precCreated, precedent);
            }
            catch (ConnectionException cex)
            {
                CheckForAllowedConstraintException(cex, ParentForm);
                _precCreated = false;
            }
            //catch any other possible exception
            catch (Exception ex)
            {
                ErrorBox.Show(ParentForm, ex);
                _precCreated = false;
            }
            return _precCreated;
        }


        private void AddFailedFiles(string FileName, bool _precCreated, string title)
        {
            if (!_precCreated)
            {
                failedFiles.Add(FileName);
                System.Diagnostics.Debug.WriteLine("Unable to add: " + title, "PDF Import");
            }
        }

        private bool UpdateNewPDFPrecedentFile(string FullPath, bool _precCreated, FWBS.OMS.Precedent precedent)
        {
            precedent.Update();
            string _precpath = FullPath.Substring(FullPath.IndexOf(precSubDirectory));
            precedent.SetExtraInfo("precpath", _precpath);
            precedent.Update();
            precedentsAdded++;
            _precCreated = true;
            return _precCreated;
        }

        private void NewPDFPrecedent(string FileName, out string title, out FWBS.OMS.Precedent precedent)
        {
            title = TrimString(FileName.ToUpper(), 50);
            string description = TrimString(FileName, 100);

            precedent = new FWBS.OMS.Precedent(title.ToUpper(), "PDF", description, "pdf", 0);
            precedent.Category = this.xpPrecCat.Value.ToString();
            precedent.SubCategory = this.xpPrecSubCat.Value.ToString();
            precedent.MinorCategory = this.xpPrecMinorCat.Value.ToString();
            precedent.PublishersName = FWBS.OMS.Session.CurrentSession.CompanyName;
            precedent.SetExtraInfo("precprogtype", 8);
        }


        private string CopyPDFToPrecedentLocation(string PDFFullPath, string PDFPrecedentDirectory, bool Overwrite)
        {
            string _precDirectory = PDFPrecedentDirectory;
            System.Diagnostics.Debug.WriteLine(string.Format("FullPath: {0}, PDF Precedent Folder {1}", PDFFullPath, _precDirectory), "PDF Import");

            if (!PDFFullPath.Contains(_precDirectory))
            {
                System.IO.Directory.CreateDirectory(_precDirectory);
                FileInfo oldPDFFile = new FileInfo(PDFFullPath);
                FileInfo newPDFFile = oldPDFFile.CopyTo(_precDirectory + @"\" + Path.GetFileName(PDFFullPath), Overwrite);
                PDFFullPath = newPDFFile.FullName;
                System.Diagnostics.Debug.WriteLine(string.Format("New PDF Template Location: {0}", PDFFullPath));
            }
            return PDFFullPath;
        }

        private static void CheckForAllowedConstraintException(ConnectionException cex, IWin32Window owner = null)
        {
            SqlException sqlex = cex.InnerException as SqlException;
            if (sqlex == null || sqlex.Number != 2627)
            {
                //if the error is not the expected violation of unique index
                ErrorBox.Show(owner, cex);
            }
        }

        /// <summary>
        /// Cuts a passed string to the required length ignoring if shorter 
        /// </summary>
        /// <param name="val">string to examine</param>
        /// <param name="length">max length string can be</param>
        /// <returns>Shortened string if less than specified length</returns>
        private string TrimString(string val, int length)
        {
            if (val.Length <= length)
                return val;
            else
            {
                string retval;
                retval = val.Substring(0, length);
                return retval;
            }
        }

        public void ClearListView(bool RemoveAll)
        {
            if (importList.Items.Count > 0)
            {
                foreach (ListViewItem item in importList.Items)
                {
                    if (RemoveAll)
                        item.Remove();
                    else if (item.ForeColor != System.Drawing.Color.Red)
                        item.Remove();
                }
            }
            existingFiles = 0;
            ConfigureImportButtons();
        }


        public void ConfigureImportButtons()
        {
            int _items = importList.Items.Count;
            this.btnImport.Enabled = (_items > 0);
            this.btnClear.Enabled = (_items > 0);
            this.xpPrecCat.Enabled = (_items > 0);
            this.xpPrecSubCat.Enabled = (_items > 0);
            this.xpPrecMinorCat.Enabled = (_items > 0);
            this.chkOverwriteAll.Enabled = (existingFiles > 1);
            if (!this.chkOverwriteAll.Enabled)
                this.chkOverwriteAll.Checked = false;
        }


        public void CheckPrecedentDirectoryExists()
        {
            string _path = FWBS.OMS.Session.CurrentSession.GetSystemDirectory(FWBS.OMS.SystemDirectories.OMPrecedents).FullName;
            DirectoryInfo dir = new DirectoryInfo(_path);
            if (!dir.Exists)
                dir.Create();
        }


        public void SetCaptions()
        {
            lblTitle.Text = ResourceLookup.GetLookupText("PDFI_TITLE");
            grpPrecCats.Text = ResourceLookup.GetLookupText("PDFI_IMPCAT");
            btnSelect.Text = ResourceLookup.GetLookupText("PDFI_SELPDF");
            xpPrecCat.Text = ResourceLookup.GetLookupText("PDFI_XPCAT");
            xpPrecSubCat.Text = ResourceLookup.GetLookupText("PDFI_XPSCAT");
            xpPrecMinorCat.Text = ResourceLookup.GetLookupText("PDFI_XPMCAT");
            btnImport.Text = ResourceLookup.GetLookupText("PDFI_");
            chkOverwriteAll.Text = ResourceLookup.GetLookupText("PDFI_OVRWRT");
            btnClear.Text = ResourceLookup.GetLookupText("PDFI_CLR"); 
        }


        private void CreateCodeLookups()
        {
            CreateResourceCodeLookup("PDFI_QIMP", "You are attempting to import from within the precedent storage location. Are you sure you wish to proceed?");
            CreateResourceCodeLookup("PDFI_QSEL", "Please select at least one PDF file to be imported");
            CreateResourceCodeLookup("PDFI_QSUC", "Import routine successful");
            CreateResourceCodeLookup("PDFI_NREP", "{0} not replaced:\n");
            CreateResourceCodeLookup("PDFI_QREP", "{0} imported successfully.\n\n{1} failed:\n");
            CreateResourceCodeLookup("PDFI_QWRN", "The file {0} already exists in precedent storage location.\n\nDo you want to replace the existing file?");
            CreateResourceCodeLookup("PDFI_TITLE", "PDF Import Menu");
            CreateResourceCodeLookup("PDFI_IMPCAT", "Import into these precedent categories");
            CreateResourceCodeLookup("PDFI_SELPDF", "Select PDF Files");
            CreateResourceCodeLookup("PDFI_XPCAT", "Category");
            CreateResourceCodeLookup("PDFI_XPSCAT", "Sub Category");
            CreateResourceCodeLookup("PDFI_XPMCAT", "Minor Category");
            CreateResourceCodeLookup("PDFI_", "Import");
            CreateResourceCodeLookup("PDFI_OVRWRT", "Overwrite All");
            CreateResourceCodeLookup("PDFI_CLR", "Clear");
            CreateResourceCodeLookup("PDFI_CAP", "PDF Import");
        }
        

        private string CreateResourceCodeLookup(string Code, string Description)
        {
            return ResourceLookup.GetLookupText(Code, Description, null, null);
        }

        #endregion

        #region IObjectDirty Implementation

        private bool isdirty;
        public bool IsDirty { get { return isdirty; } }

        public bool IsObjectDirty()
        {
            return true;
        }

        #endregion
    }
}
