using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Aspose.Pdf.Facades;
using Aspose.Pdf.Optimization;
using Aspose.Pdf.Text;
using FWBS.OMS;
using FWBS.OMS.PDFConversion;
using MatterSphereBundlerLibrary;
using Microsoft.Win32;

namespace Bundler
{


    #region BundleTools
    public class BundleTools : IDisposable
    {
        #region Constants

        private const string REG_APPLICATION_KEY = "OMS";
        private const string REG_VERSION_KEY = "2.0";
        private const string REG_KEY = @"HKEY_LOCAL_MACHINE\SOFTWARE\FWBS\OMS\2.0\MatterSphereBundlerService";
        private const string HTML_BR = "<BR>";

        private const string EVENT_LOG_NAME = "PDFBundler";
        private const string EVENT_LOG_SOURCE = "MatterSphere Bundler Service";

        #endregion Constants

        #region Members

        private int numberOfTableOfContentPages = 0;
        private string workingPath;
        private string startPath;
        private string inputPath;
        private string outputPath;
        private readonly EventLog _eventLog;

        #endregion Members

        static BundleTools()
        {
            typeof(Session).GetField("_installLocation", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                .SetValue(Session.CurrentSession, AppDomain.CurrentDomain.BaseDirectory);
        }

        public BundleTools()
        {
            if (!EventLog.SourceExists(EVENT_LOG_SOURCE))
            {
                EventLog.CreateEventSource(EVENT_LOG_SOURCE, EVENT_LOG_NAME);
            }
            _eventLog = new EventLog(EVENT_LOG_NAME, ".", EVENT_LOG_SOURCE);
        }

        public void Dispose()
        {
            _eventLog.Dispose();
        }

        #region Properties

        public int NumberOfContentsPerPage
        {
            get
            {
                return 18;
            }
        }


        public int NumberOfTableOfContentPages
        {
            get
            {
                return numberOfTableOfContentPages;
            }
            set
            {
                numberOfTableOfContentPages = value;
            }
        }


        public string StartPath
        {
            get
            {
                return startPath;
            }
            set
            {
                startPath = value;
            }

        }


        public string InputPath
        {
            get
            {
                return Path.Combine(inputPath,"Input");
            }
            set
            {
                inputPath = value;
            }

        }


        public string WorkingPath
        {
            get
            {
                return Path.Combine(startPath, "WorkingFolder");
            }
            set
            {
                workingPath = value;
            }

        }


        public string OutputPath
        {
            get
            {
                return Path.Combine(startPath, "Output");
            }
            set
            {
                outputPath = value;
            }

        }


        public string XMLFileLocation
        {
            get 
            {
                return Convert.ToString(Registry.GetValue(@"HKEY_CURRENT_USER\Software\FWBS\BUNDLER", "XMLLocation", null));
            }
        }

        #endregion Properties

        #region Methods

        


        /// <summary>
        /// Insert empty page(s) into a pdf Document for use as Table of Contents pages
        /// </summary>
        /// <param name="concatenated_pdfDocument"></param>
        /// <param name="pdfFilesInConcatenatedPDFDocument"></param>
        private void InsertBlankTableOfContentsPages(Aspose.Pdf.Document concatenated_pdfDocument, string[] pdfFilesInConcatenatedPDFDocument)
        {
            int result;
            int documentCount = 0;

            NumberOfTableOfContentPages = 1;            // Start with 1 table of contents page... 
            concatenated_pdfDocument.Pages.Insert(1);   // ...and insert into document

            // Establish how many more Table Of Contents pages are required
            foreach (string pdfFile in pdfFilesInConcatenatedPDFDocument)
            {
                if (documentCount > 0)
                {
                    Math.DivRem(documentCount, NumberOfContentsPerPage, out result);

                    if (result == 0)
                    {
                        // Keep track of the NumberOfTableOfContentPages and insert new pages accordingly
                        NumberOfTableOfContentPages += 1;
                        concatenated_pdfDocument.Pages.Insert(1);
                    }
                }

                documentCount += 1;
            }
        }


        /// <summary>
        /// Delete an array of files. Filenames must be in full with path so delete can be effective.
        /// </summary>
        /// <param name="pdfFiles"></param>
        public void DeleteFiles(string[] pdfFiles)
        {
            foreach (string pdfFile in pdfFiles)
            {
                if (File.Exists(pdfFile))
                {
                    System.IO.File.Delete(pdfFile);
                }
            }
        }


        


        /// <summary>
        /// Create a bundled PDF File based on an array of BundleDetails objects being supplied
        /// </summary>
        /// <param name="bundle">Array of BundleDetails objects</param>
        public void CreateBundledPDFFile(BundleDetails bundle)
        {
            try {
                ValidateBundle(bundle);
            }
            catch (Exception ex) {
                LogErrorMessage(ex.Message);
                return;
            }
            
            LogMessage("Creating bundle");
            Aspose.Pdf.License pdf1Lic = new Aspose.Pdf.License();
            pdf1Lic.SetLicense("Aspose.Total.lic");

            bool hasErrors = false;
            //Set input path based on XML file location
            string xmlFile = bundle.ApplicationInformation.XMLBundleFileName;
            StartPath = Path.GetDirectoryName(xmlFile);
            string destinationFile = string.Format(@"{0}\{1}.pdf", OutputPath, Path.GetFileNameWithoutExtension(xmlFile));
            string[] pdfFiles = new string[0];

            try
            {
                LogMessage("Start: Creating required directories.");
                CreateDirectories();
                LogMessage("End: Creating required directories.");

                AppendLog("Note", string.Format("Publish Showing Markup set to {0}.", bundle.ApplicationInformation.PublishShowingMarkup), bundle);

                LogMessage("Start: Converting documents in bundle to pdf files.");
                pdfFiles = ConvertDocumentsInBundleToPDFs(bundle);
                LogMessage("End: Converting documents in bundle to pdf files.");

                hasErrors = pdfFiles.Length != bundle.Documents.Length;
                if (pdfFiles.Length == 0)
                    throw new Exception("Unable to create PDF Bundle.");

                if (bundle.Options.FirstDocumentInListAsCoverPage)
                {
                    LogMessage("Start: Removing first document from pdf list (for use as a cover page).");
                    pdfFiles = RemoveFirstDocumentFromPDFFiles(pdfFiles);
                    LogMessage("End: Removing first document from pdf list (for use as a cover page).");
                }

                PageNumbering pageNumbering = bundle.Options.PageNumbering;
                if (pageNumbering.IncludeInBundledPDF && pageNumbering.PerDocument)
                {
                    LogMessage("Start: Adding page numbers to pdf list (per-document).");
                    foreach (string pdfFile in pdfFiles)
                    {
                        AddBundlePageNumbersToPDFDocument(pdfFile, pdfFile + ".pdf", pageNumbering);
                        MoveFile(pdfFile + ".pdf", pdfFile);
                    }
                    LogMessage("End: Adding page numbers to pdf list (per-document).");
                }

                LogMessage("Start: Compiling pdf files into concatenated file.");
                PdfFileEditor pdfEditor = new PdfFileEditor() { CloseConcatenatedStreams = true };
                pdfEditor.Concatenate(pdfFiles, destinationFile);
                LogMessage("End: Compiling pdf files into concatenated file.");

                LogMessage("Start: Building table of contents.");
                BuildTableOfContents(bundle, pdfFiles, destinationFile);
                LogMessage("End: Building table of contents.");

                ApplyBundleOptions(bundle, destinationFile);

                if (File.Exists(destinationFile))
                    AppendLog("Success", "PDF Bundle has been created.", bundle);
            }
            catch (Exception ex)
            {
                hasErrors = true;
                LogErrorMessage(ex.ToString());
                AppendLog("Critical", ex.Message, bundle);
                destinationFile = null;
            }

            try
            {
                LogMessage("Start: Deleting temporary pdf files.");
                DeleteFiles(pdfFiles);
                LogMessage("End: Deleting temporary pdf files.");
            }
            catch (Exception ex)
            {
                LogErrorMessage(ex.Message, true);
            }

            if (GetMatterSphereSession())
            {
                try
                {
                    if (bundle.ApplicationInformation.SaveBundleToMatter && destinationFile != null)
                    {
                        FWBS.OMS.User user = FWBS.OMS.User.GetUser(bundle.ApplicationInformation.UserID);

                        LogMessage("Start: Saving pdf to MatterSphere.");
                        if (SaveDocumentToMatterSphere(FWBS.OMS.OMSFile.GetFile(bundle.ApplicationInformation.MatterID), destinationFile, user, bundle.ApplicationInformation.BundleDescription, bundle.ApplicationInformation.DocFolderGuid) == -1)
                            AppendLog("Warning", "Failed to save Bundle to a Matter.", bundle);
                        LogMessage("End: Saving pdf to MatterSphere.");
                    }
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex.Message);
                    AppendLog("Warning", "Failed to save Bundle to a Matter.", bundle);
                }
                finally
                {
                    if (bundle.BundleID != 0)
                    {
                        UpdateBundleStatus(bundle, destinationFile != null, hasErrors);
                    }
                }
            }

            if (bundle.ApplicationInformation.RequestedByEmailAddress != "")
            {
                LogMessage(string.Format("Constructing email to '{0}'", bundle.ApplicationInformation.RequestedByEmailAddress));
                try
                {
                    SendMail(bundle, OutputPath, destinationFile);
                }
                catch (Exception ex)
                {
                    LogErrorMessage(ex.Message);
                }
            }

            try
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Directory.Delete(WorkingPath, true);
            }
            catch (Exception ex)
            {
                LogErrorMessage(ex.Message, true);
            }
        }


        private void ApplyBundleOptions(BundleDetails bundle, string destinationFile)
        {
            if (File.Exists(destinationFile))
            {
                string sourceFile = Path.Combine(WorkingPath, Path.GetFileName(destinationFile));

                PageNumbering pageNumbering = bundle.Options.PageNumbering;
                if (pageNumbering.IncludeInBundledPDF && !pageNumbering.PerDocument)
                {
                    LogMessage("Start: Adding page numbers to the pdf.");
                    MoveFile(destinationFile, sourceFile);
                    AddBundlePageNumbersToPDFDocument(sourceFile, destinationFile, pageNumbering);
                    LogMessage("End: Adding page numbers to the pdf.");
                }

                if (bundle.Options.FirstDocumentInListAsCoverPage)
                {
                    LogMessage("Start: Adding the first document as a cover page.");
                    MoveFile(destinationFile, sourceFile);
                    AddCoverPage(bundle, sourceFile, destinationFile);
                    LogMessage("End: Adding the first document as a cover page.");
                }

                LogMessage("Start: Optimizing the bundle.");
                MoveFile(destinationFile, sourceFile);
                CompressPdf(bundle, sourceFile, destinationFile);
                LogMessage("End: Optimizing the bundle.");
            }
        }


        private void BuildTableOfContents(BundleDetails bundle, string[] pdfFiles, string destinationFile)
        {
            if (bundle.Options.TableOfContents.IncludeInBundledPDF && File.Exists(destinationFile))
            {
                // Insert a blank page at the begining of concatenated file to display Table of Contents
                // hold the resultant file with empty page added
                using (MemoryStream Document_With_BlankPage = new MemoryStream())
                {
                    using (Aspose.Pdf.Document concatenated_pdfDocument = new Aspose.Pdf.Document(destinationFile))
                    {
                        LogMessage("Start: Calculating and inserting required number of table of contents pages.");
                        InsertBlankTableOfContentsPages(concatenated_pdfDocument, pdfFiles);
                        LogMessage("End: Calculating and inserting required number of table of contents pages.");

                        concatenated_pdfDocument.Save(Document_With_BlankPage);
                    }

                    using (var Document_with_TOC_Heading = new MemoryStream())
                    {
                        int locationOnPage;
                        int page;
                            
                        LogMessage("Start: Adding items to table of contents.");
                        AddTableOfContentsItems(bundle, pdfFiles, Document_With_BlankPage, Document_with_TOC_Heading, out locationOnPage, out page);
                        LogMessage("End: Adding items to table of contents.");

                        LogMessage("Start: Adding links to table of contents items.");
                        AddLinksToTableOfContents(bundle, pdfFiles, Document_with_TOC_Heading, ref locationOnPage, ref page);
                        LogMessage("End: Adding links to table of contents items.");
                    }
                }
            }
        }


        private void AddTableOfContentsItems(BundleDetails bundle, string[] pdfFiles, MemoryStream Document_With_BlankPage, MemoryStream Document_with_TOC_Heading, out int locationOnPage, out int page)
        {
            //Add Table Of Contents logo as stamp to PDF file
            PdfFileStamp fileStamp = new PdfFileStamp();
            fileStamp.BindPdf(Document_With_BlankPage);                         // find the input file
            Aspose.Pdf.Facades.Stamp stamp = new Aspose.Pdf.Facades.Stamp();    //set Text Stamp to display string Table Of Contents
            stamp.BindLogo(new FormattedText("INDEX OF BUNDLE", System.Drawing.Color.Black, System.Drawing.Color.Transparent, Aspose.Pdf.Facades.FontStyle.TimesRoman, EncodingType.Winansi, true, 18));
            stamp.SetOrigin((new PdfFileInfo(Document_With_BlankPage).GetPageWidth(1) / 3), 700); // specify the origin of Stamp. We are getting the page width and specifying the X coordinate for stamp
            stamp.Pages = new int[] { 1 };                                      //set particular pages
            fileStamp.AddStamp(stamp);                                          //add stamp to PDF file

            int documentNo = 1;
            locationOnPage = 650;
            page = 1;
            int documentCount = 0;
            DataTable pageRangeDataTable = BuildPageRangeDataTable(pdfFiles);

            foreach (Document pdfFile in bundle.Documents)
            {
                int result;

                //create stamp text for first item in Table Of Contents
                var documentLink_PageRange = new Aspose.Pdf.Facades.Stamp();
                var documentLink_FileDescription = new Aspose.Pdf.Facades.Stamp();

                // Check to see if file was successfully converted to pdf
                if (!File.Exists(string.Format(@"{0}\{1}.pdf", WorkingPath, Path.GetFileNameWithoutExtension(pdfFile.Path))))
                { 
                    documentCount++;
                    continue;
                }

                if (bundle.Options.FirstDocumentInListAsCoverPage && (documentCount == 0))
                {
                    documentCount++;
                    continue;
                }

                string name = FormatFileDescriptionLength(pdfFile.Name);
                
                documentLink_PageRange.BindLogo(new FormattedText(string.Format("{0}.   {1}", documentNo, GetPageRange(pageRangeDataTable, documentNo)), System.Drawing.Color.Black, System.Drawing.Color.Transparent, Aspose.Pdf.Facades.FontStyle.TimesRoman, EncodingType.Winansi, true, 12));
                documentLink_FileDescription.BindLogo(new FormattedText(string.Format("{0}", name), System.Drawing.Color.Black, System.Drawing.Color.Transparent, Aspose.Pdf.Facades.FontStyle.TimesRoman, EncodingType.Winansi, true, 12));

                if (documentCount > 0)
                {
                    Math.DivRem(documentCount, NumberOfContentsPerPage, out result);

                    if (result == 0)
                    {
                        page += 1;
                        locationOnPage = 700;
                    }
                }

                // specify the origin of Stamp. We are getting the page width and specifying the X coordinate for stamp
                documentLink_PageRange.SetOrigin(50, locationOnPage);
                documentLink_FileDescription.SetOrigin(185, locationOnPage);

                //set particular pages on which stamp should be displayed
                documentLink_PageRange.Pages = new int[] { page };
                documentLink_FileDescription.Pages = new int[] { page };
                
                //add stamp to PDF file
                fileStamp.AddStamp(documentLink_PageRange);
                fileStamp.AddStamp(documentLink_FileDescription);
                locationOnPage -= 30;
                documentNo += 1;
                documentCount += 1;
            }

            //save updated PDF file
            fileStamp.Save(Document_with_TOC_Heading);
            fileStamp.Close();
        }


        private string FormatFileDescriptionLength(string description)
        {
            if (description.Length > 80)
            {
                description = description.Substring(0, 80) + "...";
            }

            return description;
        }


        private void AddLinksToTableOfContents(BundleDetails bundle, string[] pdfFiles, MemoryStream Document_with_TOC_Heading, ref int locationOnPage, ref int page)
        {
            // Add Heading for Table Of Contents and links for documents
            PdfContentEditor contentEditor = new PdfContentEditor();
            // bind the PDF file in which we added the blank page
            contentEditor.BindPdf(Document_with_TOC_Heading);

            int docPageCount;
            int pageStartPosition = NumberOfTableOfContentPages + 1;
            locationOnPage = 648;
            int docCount = 0;
            int result1;
            page = 1;

            foreach (string pdfFile in pdfFiles)
            {
                using (Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(pdfFile))
                {
                    docPageCount = pdfDocument.Pages.Count;
                }

                if (docCount > 0)
                {
                    Math.DivRem(docCount, NumberOfContentsPerPage, out result1);

                    if (result1 == 0)
                    {
                        page += 1;
                        locationOnPage = 700;
                    }
                }

                contentEditor.CreateLocalLink(new System.Drawing.Rectangle(50, locationOnPage, 500, 17), pageStartPosition, page, System.Drawing.Color.Transparent);
                pageStartPosition += docPageCount;
                locationOnPage -= 30;

                docCount += 1;
            }

            string fullCombinedPath = string.Format(@"{0}\{1}.pdf", OutputPath, Path.GetFileNameWithoutExtension(bundle.ApplicationInformation.XMLBundleFileName));
            contentEditor.Save(fullCombinedPath);
            contentEditor.Close();
        }


        public void AddCoverPage(BundleDetails bundle, string savedConcatenatedPDFDocument, string destinationFile)
        {
            Document firstDocumentInBundle = bundle.Documents[0];
            string convertedDocument = ConvertIndividualDocumentToPDF(firstDocumentInBundle, bundle);
            
            string[] pdfFiles = new string[2];
            pdfFiles[0] = convertedDocument;
            pdfFiles[1] = savedConcatenatedPDFDocument;

            PdfFileEditor pdfEditor = new PdfFileEditor() { CloseConcatenatedStreams = true };
            pdfEditor.Concatenate(pdfFiles, destinationFile);
        }


        /// <summary>
        /// Remove the first document from the list of pdf files if it is to be used as a cover page. 
        /// By doing so, Aspose will not concatenate it into a singular pdf document.
        /// </summary>
        /// <param name="pdfFiles"></param>
        /// <returns></returns>
        private string[] RemoveFirstDocumentFromPDFFiles(string[] pdfFiles)
        {
            if (pdfFiles.Length > 0)
            {
                string[] newFiles = new string[pdfFiles.Length - 1];
                Array.Copy(pdfFiles, 1, newFiles, 0, newFiles.Length);
                pdfFiles = newFiles;
            }
            return pdfFiles;
        }


        private string GetPageRange(DataTable pageRangeDataTable, int documentNo)
        {
            foreach (DataRow currentRow in pageRangeDataTable.Rows)
            {
                if (Convert.ToInt32(currentRow["DocumentNo"]) == documentNo)
                {
                    return Convert.ToString(currentRow["PageRange"]);
                }
            }

            return "";
        }


        private DataTable BuildPageRangeDataTable(string[] pdfFiles)
        {
            DataTable pageRanges = new DataTable();
            pageRanges.Columns.Add("DocumentNo", typeof(int));
            pageRanges.Columns.Add("PageRange", typeof(string));

            int documentNo = 1;
            int documentStartPage = NumberOfTableOfContentPages + 1;
            int documentEndPage = 0;

            foreach (string pdfFile in pdfFiles)
            {
                using (Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(pdfFile))
                {
                    documentEndPage = documentStartPage + (pdfDocument.Pages.Count - 1);
                }

                string pageString = (documentStartPage == documentEndPage) ? "Page" : "Pages";
                string pageRange = (documentStartPage == documentEndPage) ? string.Format("{0}   {1}", pageString, PadPageRangeString(documentEndPage.ToString())) : string.Format("{0}  {1}", pageString, PadPageRangeString(string.Format("{0} - {1}", documentStartPage.ToString(), documentEndPage.ToString())));

                pageRanges.Rows.Add(documentNo, pageRange);
                documentNo += 1;
                documentStartPage = documentEndPage + 1;
            }

            return pageRanges;
        }


        private string PadPageRangeString(string pageRange)
        { 
            return pageRange.PadRight(15, ' ');
        }


        private void CreateDirectories()
        {
            Directory.CreateDirectory(WorkingPath);
            Directory.CreateDirectory(OutputPath);
        }

        private void MoveFile(string sourceFileName, string destFileName)
        {
            if (File.Exists(destFileName))
                File.Delete(destFileName);

            File.Move(sourceFileName, destFileName);
        }


        private void AddBundlePageNumbersToPDFDocument(string pdfFile, string destinationFile, PageNumbering pageNumbering)
        {
            using (Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(pdfFile))
            {
                if (pdfDocument.Pages.Count > 0)
                {
                    string pageNumberingFormat = string.IsNullOrWhiteSpace(pageNumbering.Format) ? PageNumbering.DefaultFormat : pageNumbering.Format;
                    pageNumberingFormat = pageNumberingFormat.Replace("##", pdfDocument.Pages.Count.ToString());

                    foreach (Aspose.Pdf.Page pdfPage in pdfDocument.Pages)
                    {
                        //create page number stamp
                        Aspose.Pdf.PageNumberStamp pageNumberStamp = new Aspose.Pdf.PageNumberStamp();

                        //whether the stamp is background
                        pageNumberStamp.Background = false;
                        pageNumberStamp.Format = pageNumberingFormat;
                        pageNumberStamp.BottomMargin = 10;
                        pageNumberStamp.LeftMargin = 10;
                        pageNumberStamp.RightMargin = 10;
                        pageNumberStamp.HorizontalAlignment = (Aspose.Pdf.HorizontalAlignment)pageNumbering.Alignment;
                        pageNumberStamp.StartingNumber = 1;

                        //set text properties
                        pageNumberStamp.TextState.Font = FontRepository.FindFont("Arial");
                        pageNumberStamp.TextState.FontSize = 14.0F;
                        pageNumberStamp.TextState.FontStyle = FontStyles.Bold;
                        pageNumberStamp.TextState.ForegroundColor = Aspose.Pdf.Color.Black;

                        pdfPage.AddStamp(pageNumberStamp);
                    }
                }

                //Now resave as new name
                pdfDocument.Save(destinationFile);
            }
        }


        /// <summary>
        /// Convert files to PDF documents. Position documents where service can work with them. Create PDF bundles.
        /// </summary>
        /// <param name="bundles"></param>
        public void ProcessBundles(BundleDetails[] bundleDetails, string incomingKey)
        {
            if(ValidateKey(incomingKey))
            {
                LogMessage("Start:ProcessBundles(BundleDetails[] bundleDetails, string incomingKey)");
                foreach (BundleDetails bundle in bundleDetails)
                {
                    CreateBundledPDFFile(bundle);
                }
                LogMessage("End:ProcessBundles(BundleDetails[] bundleDetails, string incomingKey)");
            }
        }

        public void ProcessBundles(string xmlInstructionFile, string incomingKey)
        {
            if(ValidateKey(incomingKey))
            {
                LogMessage("Start:ProcessBundles(string xmlInstructionFile, string incomingKey)");
                BundleDetails bundleDetails = BundlerUtil.XMLStringToClass<BundleDetails>(File.ReadAllText(xmlInstructionFile, Encoding.UTF8));
                CreateBundledPDFFile(bundleDetails);
                LogMessage("End:ProcessBundles(string xmlInstructionFile, string incomingKey)");
            }
        }


        public bool ValidateKey(string incomingKey)
        {
            try
            {
                string expectedKey = GetSetting("UsageKey","").ToUpper();
                incomingKey = incomingKey.ToUpper();

                if(string.IsNullOrWhiteSpace(expectedKey))
                    throw new Exception("UsageKey not set for validation");
                else
                {
                    if(incomingKey==expectedKey)
                        return true;
                    else
                    {
                        throw new Exception("Invalid UsageKey used");
                    }
                }
            }
            catch(Exception ex)
            {
                LogErrorMessage(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Convert documents listed in the bundle XML files to PDFs for processing
        /// </summary>
        /// <param name="bundle"></param>
        /// <returns></returns>
        public string[] ConvertDocumentsInBundleToPDFs(BundleDetails bundle)
        {
            List<string> pdfFiles = new List<string>();
            Converter pdfConverter = new Converter();

            foreach (Document document in bundle.Documents)
            {
                string sourceFile = document.Path;
                string destinationFile = string.Format(@"{0}\{1}.pdf", WorkingPath, Path.GetFileNameWithoutExtension(sourceFile));

                if (FileIsPDF(sourceFile))
                {
                    try
                    {
                        string password = string.IsNullOrEmpty(document.Token) ? null : EncryptionV2.Decrypt(document.Token, sourceFile);
                        ValidatePDF(sourceFile, destinationFile, password);
                        pdfFiles.Add(destinationFile);
                        AppendLog("Success", string.Format("File {0} is already a PDF. No conversion required.", sourceFile), bundle);
                    }
                    catch (Exception ex)
                    {
                        AppendLog("Error", string.Format("Unable to process: {0}\t\t{1}", sourceFile, ex.Message), bundle);
                        if (bundle.Options.FirstDocumentInListAsCoverPage && document == bundle.Documents[0])
                            bundle.Options.FirstDocumentInListAsCoverPage = false;
                    }
                }
                else
                {
                    try
                    {
                        string password = string.IsNullOrEmpty(document.Token) ? null : EncryptionV2.Decrypt(document.Token, sourceFile);
                        pdfConverter.ConvertToPDF(sourceFile, destinationFile, !bundle.ApplicationInformation.PublishShowingMarkup, password);
                        AppendLog("Success", string.Format("Successfully converted: {0}", sourceFile), bundle);
                        pdfFiles.Add(destinationFile);
                    }
                    catch (Exception ex)
                    {
                        AppendLog("Error", string.Format("Unable to convert: {0}\t\t{1}", sourceFile, ex.Message), bundle);
                        if (bundle.Options.FirstDocumentInListAsCoverPage && document == bundle.Documents[0])
                            bundle.Options.FirstDocumentInListAsCoverPage = false;
                    }
                }     
            }
            
            return pdfFiles.ToArray();
        }


        private bool FileIsPDF(string sourceFile)
        {
            return Path.GetExtension(sourceFile).ToUpper() == ".PDF";
        }

        private void ValidatePDF(string sourceFile, string destinationFile, string password)
        {
            bool hasOpenPassword = false;
            using (PdfFileInfo pdfFileInfo = new PdfFileInfo(sourceFile))
            {
                hasOpenPassword = pdfFileInfo.HasOpenPassword;
            }
            if (hasOpenPassword)
            {
                using (Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(sourceFile, password))
                {
                    pdfDocument.Decrypt();
                    pdfDocument.Save(destinationFile);
                }
            }
            else
            {
                File.Copy(sourceFile, destinationFile, true);
            }
        }

        private void AppendLog(string code, string message, BundleDetails bundle)
        {
            using (StreamWriter sw = new StreamWriter(new FileStream(Path.ChangeExtension(bundle.ApplicationInformation.XMLBundleFileName, ".log"), FileMode.Append)))
            {
                sw.WriteLine(string.Format("{0}\t{1}\t{2}\t{3}", code, DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), message));
                sw.Flush();
            }
        }

        private void CompressPdf(BundleDetails bundle, string sourceFile, string destinationFile)
        {
            using (var optimizedPdfDocument = new Aspose.Pdf.Document(sourceFile))
            {
                var options = Aspose.Pdf.Optimization.OptimizationOptions.All();
                options.AllowReusePageContent = true;
                if (bundle.Options.ImageCompressionOptions != null && bundle.Options.ImageCompressionOptions.CompressImages)
                {
                    options.ImageCompressionOptions.CompressImages = bundle.Options.ImageCompressionOptions.CompressImages;
                    options.ImageCompressionOptions.ImageQuality = bundle.Options.ImageCompressionOptions.ImageQuality;
                    options.ImageCompressionOptions.ResizeImages = bundle.Options.ImageCompressionOptions.ResizeImages;
                    options.ImageCompressionOptions.MaxResolution = bundle.Options.ImageCompressionOptions.MaxResolution;

                    options.ImageCompressionOptions.Encoding = ImageEncoding.Unchanged;
                    options.ImageCompressionOptions.Version = ImageCompressionVersion.Mixed;
                }

                optimizedPdfDocument.OptimizeResources(options);
                optimizedPdfDocument.Optimize();
                optimizedPdfDocument.Save(destinationFile);
            }
        }
        
        /// <summary>
        /// Convert an indivdual document to a PDF file
        /// </summary>
        /// <param name="document">Document object containing document details specific to bundling</param>
        /// <returns></returns>
        public string ConvertIndividualDocumentToPDF(Document document)
        {
            return ConvertIndividualDocumentToPDF(document, null);
        }

        /// <summary>
        /// Convert an indivdual document to a PDF file
        /// </summary>
        /// <param name="document">Document object containing document details specific to bundling</param>
        /// <returns></returns>
        public string ConvertIndividualDocumentToPDF(Document document, BundleDetails bundle)
        {
            string sourceFile = document.Path;
            string destinationFile = string.Format(@"{0}\{1}.pdf", WorkingPath, Path.GetFileNameWithoutExtension(sourceFile));

            if (FileIsPDF(sourceFile))
            {
                File.Copy(sourceFile, destinationFile, true);
            }
            else
            {
                Converter pdfConverter = new Converter();               
                pdfConverter.ConvertToPDF(sourceFile, destinationFile, (bundle != null) ? !bundle.ApplicationInformation.PublishShowingMarkup : true);
            }

            return destinationFile;
        }


        private void SendMail(BundleDetails bundle, string subject, string fullFilePath)
        {
            MatterSphereBundlerWCFService.Properties.Settings s = new MatterSphereBundlerWCFService.Properties.Settings();

            string smtpFromAddress = GetSetting("SmtpFromAddress", "");
            string smtpClient = GetSetting("SmtpClient", "");
            string smtpEncryption = GetSetting("SmtpEncryption", "").ToUpper();
            string smtpMailSubject = GetSetting("SmtpMailSubject", "");
            string smtpMailBody = GetSetting("SmtpMailBody", "");

            if (string.IsNullOrEmpty(smtpFromAddress))
                throw new Exception("SmtpClient setting has not been configured");

            if (string.IsNullOrEmpty(smtpClient))
                throw new Exception("SmtpClient setting has not been configured");

            if (string.IsNullOrEmpty(smtpMailSubject))
                throw new Exception("SmtpMailSubject setting has not been configured");

            if (string.IsNullOrEmpty(smtpMailBody))
                throw new Exception("SmtpMailBody setting has not been configured");

            LogMessage(string.Format("Sending email: {0}smtpClient:{1}{0}smtpFromAddress:{2}{0}smtpMailSubject:{3}{0}smtpMailBody:{4}"
                , Environment.NewLine, smtpClient, smtpFromAddress, smtpMailSubject, smtpMailBody));

            string[] smtpServer = smtpClient.Split(':');
            using (SmtpClient client = (smtpServer.Length == 1) ? new SmtpClient(smtpServer[0]) : new SmtpClient(smtpServer[0], Convert.ToInt32(smtpServer[1])))
            {
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                if (!string.IsNullOrEmpty(smtpEncryption) && smtpEncryption != "NONE")
                {
                    client.EnableSsl = true;
                    if (smtpEncryption == "STARTTLS")
                        client.TargetName = smtpEncryption + "/" + client.Host;
                }

                if (GetSetting("UseAuthenticatedSMTP", "") == "True")
                {
                    if (GetSetting("UseNetworkCredentialsforSMTP", "") == "True")
                    {
                        client.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                    }
                    else
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = GetSpecificSMTPCredentials(GetSetting("SmtpUserName", ""), GetSetting("SmtpPassword", ""), GetSetting("SmtpDomain", ""));
                    }
                }

                using (MailMessage mail = new MailMessage(smtpFromAddress, bundle.ApplicationInformation.RequestedByEmailAddress))
                {
                    // build body of email
                    mail.IsBodyHtml = true;
                    string body = GetClientInformationForEmail(bundle) + GetHTMLStringForLinkToDocument("Click here to open", "An error occurred while creating the bundle.", fullFilePath);
                    //expecting {0} to be carriage return, and {1} to be path to output file
                    mail.Body = string.Format(smtpMailBody, HTML_BR, body);
                    mail.Subject = string.Format(smtpMailSubject, bundle.ApplicationInformation.BundleDescription);

                    Attachment attachment = new Attachment(Path.ChangeExtension(bundle.ApplicationInformation.XMLBundleFileName, ".log"));
                    mail.Attachments.Add(attachment);
                    client.Send(mail);
                }
            }
        }

        
        private string GetClientInformationForEmail(BundleDetails bundle)
        {
            return string.Concat(bundle.ApplicationInformation.ClientMatterRef
                                , HTML_BR
                                , bundle.ApplicationInformation.ClientName
                                , HTML_BR
                                , bundle.ApplicationInformation.MatterDescription
                                , HTML_BR
                                , HTML_BR);
        }


        private string GetHTMLStringForLinkToDocument(string hyperlinkText, string errorText, string fullFilePath)
        {
            return fullFilePath != null
                ? string.Format(@"<a href=""{0}"">{1}</a>", fullFilePath, hyperlinkText)
                : string.Format(@"<span style=""color:red"">{0}</span>", errorText);
        }


        private System.Net.NetworkCredential GetSpecificSMTPCredentials(string SMTPUserName, string SMTPPassword, string SMTPDomain)
        {
            if (string.IsNullOrEmpty(SMTPUserName))
                throw new Exception("SmtpUserName setting has not been configured");

            if (string.IsNullOrEmpty(SMTPPassword))
                throw new Exception("SmtpPassword setting has not been configured");

            return new System.Net.NetworkCredential(SMTPUserName, SMTPPassword, SMTPDomain);
        }


        private void LogMessage(string message)
        {
            try
            {
                _eventLog.WriteEntry(message, EventLogEntryType.Information);
            }
            catch { }
        }

        private void LogErrorMessage(string message, bool warning = false)
        {
            try
            {
                _eventLog.WriteEntry(message, warning ? EventLogEntryType.Warning : EventLogEntryType.Error);
            }
            catch { }
        }

        private void LogMatterSphereError(Exception ex)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MatterSphere Error").AppendLine(ex.Message);
            if (ex.InnerException != null)
                sb.AppendLine("Inner Exception").AppendLine(ex.InnerException.Message);
            LogErrorMessage(sb.ToString());
        }

        /// <summary>
		/// Reads registry setting
		/// </summary>
		/// <param name="KeyName">Registry Key Name</param>
		/// <param name="ApplicationName">Export application or common if left empty</param>
		/// <returns></returns>
		private string GetSetting(string valueName,string defaultValue)
		{
            string val = defaultValue;
            try
            {
                object oVal = Microsoft.Win32.Registry.GetValue(REG_KEY, valueName, "NOTSET");

                if (oVal == null || Convert.ToString(oVal) == "NOTSET") //key doesn't exist or key not set
                {
                    //write the default value instead
                    UpdateSetting(valueName, defaultValue);
                    val = defaultValue;
                }
                else
                {
                    val = Convert.ToString(oVal);
                }

            }
            catch (Exception ex)
            {
                LogErrorMessage(ex.Message);
            }
			return val;
		}


        /// <summary>
        /// Updates registry setting
        /// </summary>
        /// <param name="setting">Settings name</param>
        /// <param name="app">application name</param>
        /// <param name="newValue">value of setting</param>
        private void UpdateSetting(string valueName, object newValue)
        {
            try
            {
                Microsoft.Win32.Registry.SetValue(REG_KEY, valueName, newValue);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error Updating Registry " + ex.Message, ex);
            }
        }


        private bool GetMatterSphereSession()
        {
            try
            {
                FWBS.OMS.Data.DatabaseSettings settings;
                string databaseServer = GetSetting("DatabaseServer", "");
                string databaseName = GetSetting("DatabaseName", "");
                string databaseLoginType = GetSetting("DatabaseLoginType", "NT");
                string databaseProvider = GetSetting("DatabaseProvider", "SQL");

                try
                {
                    LogMessage("Beginning MatterSphere login.");
                    LogMessage("Checking for existing connection.");
                    Session.CurrentSession.APIConsumer = System.Reflection.Assembly.GetExecutingAssembly();
                    Session.CurrentSession.Connect();   // Try to connect to existing session

                    settings = Session.CurrentSession.CurrentDatabase;
                    if (!settings.Server.Equals(databaseServer, StringComparison.CurrentCultureIgnoreCase) ||
                        !settings.DatabaseName.Equals(databaseName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        Session.CurrentSession.Disconnect();
                        throw new InvalidOperationException("Bundler and MatterSphere database mismatch!");
                    }
                    LogMessage(string.Format("MatterSphere login to {0} successful.", settings.DatabaseName));
                    return true;
                }
                catch (InvalidOperationException)
                {
                    throw;
                }
                catch (Exception)
                {
                    LogMessage("No existing connection. Trying explicit connection.");

                    if (string.IsNullOrEmpty(databaseServer))
                        throw new Exception("DatabaseServer setting has not been configured.");

                    if (string.IsNullOrEmpty(databaseName))
                        throw new Exception("DatabaseName setting has not been configured.");

                    if (string.IsNullOrEmpty(databaseLoginType))
                        throw new Exception("DatabaseLoginType setting has not been configured.");

                    if (string.IsNullOrEmpty(databaseProvider))
                        throw new Exception("DatabaseProvider setting has not been configured.");

                    FWBS.OMS.Data.DatabaseConnections connections = new FWBS.OMS.Data.DatabaseConnections("BUNDLER", "OMS", "2.0");
                    settings = connections.CreateDatabaseSettings();
                    settings.Server = databaseServer;
                    settings.DatabaseName = databaseName;
                    settings.LoginType = databaseLoginType;
                    settings.Provider = databaseProvider;

                    Session.CurrentSession.LogOn(settings, Environment.UserName, "", true);
                    LogMessage(string.Format("MatterSphere login to {0} successful.", settings.DatabaseName));
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogMatterSphereError(ex);
                return false;
            }
        }


        /// <summary>
        /// Save document to MatterSphere
        /// </summary>
        /// <param name="file"></param>
        /// <param name="fullFilePath"></param>
        /// <returns>Document ID if saved successfully, or -1 in case of failure.</returns>
        private long SaveDocumentToMatterSphere(OMSFile file, string fullFilePath, User mimicUser, string docDescription, Guid folderGuid)
        {
            try
            {
                //Get referecence to the file to be saved
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(fullFilePath);
                string extension = fileInfo.Extension.Trim('.');

                //Construct a new document
                OMSDocument docNew = new OMSDocument(file.DefaultAssociate, docDescription, FWBS.OMS.Precedent.GetDefaultPrecedent("SHELL", null), null, 0, FWBS.OMS.DocumentDirection.In, extension, -1, null, folderGuid);
                docNew.TimeRecords.SkipTime = true;
                //Set User based on user to mimic
                docNew.SetExtraInfo("CreatedBy", mimicUser.ID);
                docNew.SetExtraInfo("UpdatedBy", mimicUser.ID);
                //Set document version as the latest version
                FWBS.OMS.DocumentManagement.Storage.IStorageItemVersionable versionable = docNew;
                FWBS.OMS.DocumentManagement.Storage.IStorageItemVersion version = versionable.GetLatestVersion();
                versionable.SetWorkingVersion(version);
                //Update document to create initial row
                docNew.Update();
                //construct storage provider
                FWBS.OMS.DocumentManagement.Storage.StorageProvider sp = docNew.GetStorageProvider();
                //Save File to destination
                sp.Store(version, fileInfo);
                //Return ID number
                return docNew.ID;
            }
            catch (Exception ex)
            {
                LogMatterSphereError(ex);
                return -1; // return -1 if it drops out
            }
        }

        /// <summary>
        /// Save bundle status and log to database.
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="bundleCreated"></param>
        /// <param name="hasErrors"></param>
        /// <returns>True if updates successfully, False otherwise.</returns>
        private bool UpdateBundleStatus(BundleDetails bundle, bool bundleCreated, bool hasErrors)
        {
            try
            {
                BundleDetails.Status bundleStatus = bundleCreated ? (hasErrors ? BundleDetails.Status.Warning : BundleDetails.Status.Success) : BundleDetails.Status.Error;
                string bundleLog = File.ReadAllText(Path.ChangeExtension(bundle.ApplicationInformation.XMLBundleFileName, ".log"));

                var connection = Session.CurrentSession.CurrentConnection;
                var pars = new FWBS.OMS.Data.ExecuteParameters
                {
                    CommandType = CommandType.Text,
                    Sql = "UPDATE dbPDFBundleHeader SET bundleStatus = @bundleStatus, bundleLog = @bundleLog WHERE bundleID = @bundleID"
                };
                pars.Parameters.Add(connection.CreateParameter("bundleStatus", (byte)bundleStatus));
                pars.Parameters.Add(connection.CreateParameter("bundleLog", bundleLog));
                pars.Parameters.Add(connection.CreateParameter("bundleID", bundle.BundleID));
                connection.ExecuteScalar(pars);
                return true;
            }
            catch (Exception ex)
            {
                LogMatterSphereError(ex);
                return false;
            }
        }

        /// <summary>
        /// Bundle validation for success process handling
        /// ArgumentNullException - bundle is null
        /// ArgumentOutOfRangeException - documents count equals to zero 
        /// NotSupportedException - FirstDocumentInListAsCoverPage is checked and extension of file is not supported
        /// </summary>
        /// <returns></returns>
        public static void ValidateBundle(BundleDetails bundle)
        {
            if (bundle == null)
            {
                throw new ArgumentNullException("bundle");
            }

            if (bundle.Documents == null || bundle.Documents.Count() == 0)
            {
                throw new ArgumentOutOfRangeException("Must be at least one document in a bunble.");
            }

            if (bundle.Options.FirstDocumentInListAsCoverPage)
            {
                string extension = Path.GetExtension(bundle.Documents[0].Path);

                if (!BundlerUtil.IsExtensionSupported(extension))
                {
                    throw new NotSupportedException("First file in a bunble is not supported for cover page.");
                }
            }
        }

        #endregion Methods
    }
    #endregion BundleTools




}
