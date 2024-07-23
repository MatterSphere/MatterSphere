using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using Aspose.Words;
using Aspose.Words.Drawing;
using FWBS.OMS.DocumentManagement.Storage;


namespace FWBS.OMS.PDFConversion
{
    public interface IConverter
    {
        void Print(string name);
        FileInfo ConvertToPDF(string inName, string outName, bool acceptRevisions, string password = null);
    }

    public class Converter : IConverter
    {
        #region IConverter Members

        public void Print(string name)
        {
            throw new NotImplementedException();
        }


        public FileInfo ConvertToPDF(string inName, string outName, bool acceptRevisions, string password = null)
        {
            FileInfo inFile = new FileInfo(inName);
            
            //Throw out if a temporary file
            if (inFile.Name.StartsWith("~"))
                throw new Exception(string.Format("Temporary Files are not supported.  Cannot process file '{0}'", inName));
            
            if (!inFile.Exists)
                throw new Exception(string.Format("File '{0}' does not exist or you do not have access to it", inName));

            if (password == null && Tools.IsDocumentProtected(inFile.FullName))
                password = Tools.PromptForPassword(inFile.Name) ?? Environment.NewLine;

            switch (inFile.Extension.ToLower())
            {
                case ".doc":
                case ".docx":
                case ".docm":
                case ".rtf":
                    ConvertToPDFWord(inFile.FullName, outName, acceptRevisions, password);
                    return new FileInfo(outName);
                case ".xls":
                case ".xlsx":
                case ".xlsm":
                    ConvertToPDFExcel(inFile.FullName, outName, password);
                    return new FileInfo(outName);
                case ".ppt":
                case ".pptx":
                case ".pptm":
                    ConvertToPDFPresentation(inFile.FullName, outName, password);
                    return new FileInfo(outName);
                case ".msg":
                    ConvertToPDFOutlookMessage(inFile.FullName, outName);
                    return new FileInfo(outName);
                case ".png":
                case ".gif":
                case ".bmp":
                case ".jpg":
                case ".jpeg":
                case ".tiff":
                case ".tif":
                    return ConvertToPDFImage(inFile.FullName, outName);
                case ".pdf":
                    ConvertToPDFAdobe(inFile.FullName, outName, password);
                    return new FileInfo(outName);
                default:
                    throw new Exception(string.Format("Extension'{0}' is not supported for PDF Conversion.", inFile.Extension.ToLower()));
            }

        }


        #region "Convert To PDF (Excel)"
        private static void ConvertToPDFExcel(string inName, string outName, string password = null)
        {
            new Aspose.Cells.License().SetLicense("Aspose.Total.lic");
            using (Aspose.Cells.Workbook excel = new Aspose.Cells.Workbook(inName, new Aspose.Cells.LoadOptions() { Password = password }))
            {
                excel.Save(outName, new Aspose.Cells.PdfSaveOptions() { OnePagePerSheet = true });
            }
        }
        #endregion

        #region "Convert To PDF (Outlook)"
        private static void ConvertToPDFOutlookMessage(string inName, string outName)
        {
            new Aspose.Email.License().SetLicense("Aspose.Total.lic");
            //Add license set for Word to see if it resolves issue reported in WI 2819
            new Aspose.Words.License().SetLicense("Aspose.Total.lic");
            Aspose.Email.MailMessage msg = Aspose.Email.MailMessage.Load(inName, new Aspose.Email.MsgLoadOptions());
            //Set the date to be in correct date time format
            TimeSpan ts = TimeZone.CurrentTimeZone.GetUtcOffset(msg.Date);
            msg.Date = msg.Date.Add(ts);
            msg.TimeZoneOffset = new TimeSpan(0, 0, 0);
            msg.Date = DateTime.SpecifyKind(msg.Date, DateTimeKind.Utc);
            msg.Date = msg.Date.ToLocalTime();

            using (MemoryStream ms = new MemoryStream())
            {
                //Outlook msg files are converted to mhtml first before becoming a pdf file
                msg.Save(ms, Aspose.Email.SaveOptions.DefaultMhtml);
                msg.Dispose(); msg = null;

                //Create an instance of Document and load the MTHML from MemoryStream
                Aspose.Words.Document document = new Aspose.Words.Document(ms, new Aspose.Words.Loading.LoadOptions() { LoadFormat = LoadFormat.Mhtml });

                //Format the document - set smaller margins
                Aspose.Words.PageSetup pageSetup = document.Sections[0].PageSetup;
                pageSetup.LeftMargin = pageSetup.RightMargin = pageSetup.TopMargin = pageSetup.BottomMargin = Aspose.Words.ConvertUtil.InchToPoint(0.75);
                double contentWidth = pageSetup.PageWidth - pageSetup.LeftMargin - pageSetup.RightMargin;

                //Prevent tables from being cut off if they are too wide to fit to the page
                foreach (Aspose.Words.Tables.Table tableNode in document.GetChildNodes(Aspose.Words.NodeType.Table, true))
                {
                    if (tableNode.PreferredWidth.Type == Aspose.Words.Tables.PreferredWidthType.Points && tableNode.PreferredWidth.Value > contentWidth)
                    {
                        tableNode.PreferredWidth = Aspose.Words.Tables.PreferredWidth.FromPercent(100);
                    }
                }

                //Prevent embedded images from being cut off if they are too wide to fit to the page
                foreach (Aspose.Words.Drawing.Shape shape in document.GetChildNodes(Aspose.Words.NodeType.Shape, true))
                {
                    if (shape.ShapeType == Aspose.Words.Drawing.ShapeType.Image && shape.Width > contentWidth)
                    {
                        shape.AspectRatioLocked = true;
                        shape.Width = contentWidth;
                    }
                }

                document.Save(outName, Aspose.Words.SaveFormat.Pdf);
            }
        }
        #endregion


        #region "Convert To PDF (Word)"
        private static void ConvertToPDFWord(string inName, string outName, bool acceptRevisions, string password = null)
        {
            new Aspose.Words.License().SetLicense("Aspose.Total.lic");
            Aspose.Words.Document word = new Aspose.Words.Document(inName, new Aspose.Words.Loading.LoadOptions() { Password = password });
            if (acceptRevisions)
            {
                word.AcceptAllRevisions();
            }
            word.Save(outName, Aspose.Words.SaveFormat.Pdf);
        }
        #endregion
        

        #region "Convert To PDF (Powerpoint)"
        private static void ConvertToPDFPresentation(string inName, string outName, string password = null)
        {
            new Aspose.Slides.License().SetLicense("Aspose.Total.lic");
            using (Aspose.Slides.Presentation prest = new Aspose.Slides.Presentation(inName, new Aspose.Slides.LoadOptions() { Password = password }))
            {
                prest.Save(outName, Aspose.Slides.Export.SaveFormat.Pdf);
            }
        }
        #endregion

        #region "Convert To PDF (Adobe)"
        private static void ConvertToPDFAdobe(string inName, string outName, string password = null)
        {
            bool hasOpenPassword = false;
            new Aspose.Pdf.License().SetLicense("Aspose.Total.lic");
            using (var pdfFileInfo = new Aspose.Pdf.Facades.PdfFileInfo(inName))
            {
                hasOpenPassword = pdfFileInfo.HasOpenPassword;
            }
            if (hasOpenPassword)
            {
                using (Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(inName, password))
                {
                    pdfDocument.Decrypt();
                    pdfDocument.Save(outName);
                }
            }
            else
            {
                File.Copy(inName, outName, true);
            }
        }
        #endregion

        #region "Convert Image to Pdf"
        /// <summary>
        /// Converts image to Pdf
        /// </summary>
        /// <param name="inputFileName"></param>
        /// <param name="outputFileName"></param>
        /// <param name="stretchImage"></param>
        /// <returns></returns>
        public FileInfo ConvertToPDFImage(string inputFileName, string outputFileName, bool stretchImage = false)
        {
            Document newDocument = new Document();
            DocumentBuilder docBuilder = new DocumentBuilder(newDocument);

            using (Image newImage = Image.FromFile(inputFileName))
            {
                newImage.SelectActiveFrame(FrameDimension.Page, 0);

                double maximumPageHeight;
                double maximumPageWidth;
                CalculatePaperSize(out maximumPageHeight, out maximumPageWidth);

                double currentImageHeight;
                double currentImageWidth;
                SetCurrentImageDimensions(newImage, out currentImageHeight, out currentImageWidth);

                PageSetup pageSetupPDF = PDFPageSetup(stretchImage, docBuilder, newImage, maximumPageHeight, maximumPageWidth, ref currentImageHeight, ref currentImageWidth);

                docBuilder.InsertImage(newImage, RelativeHorizontalPosition.Page, 0, RelativeVerticalPosition.Page, 0, pageSetupPDF.PageWidth, pageSetupPDF.PageHeight, WrapType.None);
            }

            newDocument.Save(outputFileName);

            return new FileInfo(outputFileName);
        }
        #endregion

        #region "Set Current Image Dimensions"
        private static void SetCurrentImageDimensions(Image newImage, out double currentImageHeight, out double currentImageWidth)
        {
            currentImageHeight = ConvertUtil.PixelToPoint(newImage.Height, newImage.VerticalResolution);
            currentImageWidth = ConvertUtil.PixelToPoint(newImage.Width, newImage.HorizontalResolution);
        }
        #endregion

        #region "PDF Page Setup"
        private static PageSetup PDFPageSetup(bool stretchImage, DocumentBuilder builder, Image newImage, double maximumPageHeight, double maximumPageWidth, ref double currentImageHeight, ref double currentImageWidth)
        {
            PageSetup pageSetupPDF = builder.PageSetup;

            if (currentImageWidth >= maximumPageWidth || currentImageHeight >= maximumPageHeight)
                CalculateImageSize(newImage, maximumPageHeight, maximumPageWidth, out currentImageHeight, out currentImageWidth);

            pageSetupPDF.PageWidth = currentImageWidth;
            pageSetupPDF.PageHeight = currentImageHeight;

            if (stretchImage)
            {
                pageSetupPDF.PageHeight = maximumPageHeight;
                pageSetupPDF.PageWidth = maximumPageWidth;
            }
            return pageSetupPDF;
        }
        #endregion
        
        #region "Calculate size of Image in relation to paper size"
        /// <summary>
        /// Calculates size of Image
        /// </summary>
        /// <param name="img">Original image</param>
        /// <param name="containerHeight">Height of container where image should be inserted.</param>
        /// <param name="containerWidth">Width of container where image should be inserted.</param>
        /// <param name="targetHeight">Height of the image</param>
        /// <param name="targetWidth">Width of the image</param>
        public static void CalculateImageSize(Image img, double containerHeight, double containerWidth, out double targetHeight, out double targetWidth)
        {
            //Calculate width and height
            targetHeight = containerHeight;
            targetWidth = containerWidth;

            //Get size of an image
            double imgHeight = ConvertUtil.PixelToPoint(img.Height);
            double imgWidth = ConvertUtil.PixelToPoint(img.Width);

            if (imgHeight < targetHeight && imgWidth < targetWidth)
            {
                targetHeight = imgHeight;
                targetWidth = imgWidth;
            }
            else
            {
                //Calculate size of an image in the document
                double ratioWidth = imgWidth / targetWidth;
                double ratioHeight = imgHeight / targetHeight;

                if (ratioWidth > ratioHeight)
                    targetHeight = (targetHeight * (ratioHeight / ratioWidth));
                else
                    targetWidth = (targetWidth * (ratioWidth / ratioHeight));
            }
        }
        #endregion

        #region "Calculate Paper Size"
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Height"></param>
        /// <param name="Width"></param>
        private void CalculatePaperSize(out Double Height, out Double Width)
        {
            try
            {
                PaperKind paperType;
                using (PrintDialog p = new PrintDialog())
                {
                    paperType = p.PrinterSettings.DefaultPageSettings.PaperSize.Kind;
                }

                switch (paperType)
                {
                    case PaperKind.Legal:
                        //Height = 1008, Width = 612
                        Height = Aspose.Pdf.PageSize.PageLegal.Height;
                        Width = Aspose.Pdf.PageSize.PageLegal.Width;
                        break;
                    default:
                        //Height = 842, Width = 595
                        Height = Aspose.Pdf.PageSize.A4.Height;
                        Width = Aspose.Pdf.PageSize.A4.Width;
                        break;
                }
            }
            catch
            {
                //Reset to A4
                Height = Aspose.Pdf.PageSize.A4.Height;
                Width = Aspose.Pdf.PageSize.A4.Width;
            }
        }
        #endregion

        #endregion
    }
        
    public class ConvertPDF
    {
        public string SingleFile(string FileName)
        { 
            return SingleFile(new FileInfo(FileName), true);
        }
        
        public string SingleFile(string FileName, bool AcceptRevisions)
        {
            return SingleFile(new FileInfo(FileName), AcceptRevisions);
        }

        public string SingleFile(FileInfo SourceFile)
        {
            return SingleFile(SourceFile, true);
        }

        public string SingleFile(FileInfo SourceFile, bool AcceptRevisions)
        {
            string destinationFolder = SourceFile.DirectoryName;
            string outFile = "";

            try
            {
                IConverter ic = new Converter();
                string inFile = SourceFile.FullName;
                outFile = Path.Combine(destinationFolder, SourceFile.Name.Substring(0, SourceFile.Name.Length - SourceFile.Extension.Length) + ".pdf");
                Debug.WriteLine(string.Format("Convert from '{0}' to {1}", inFile, outFile), "Starting");
                ic.ConvertToPDF(inFile, outFile, AcceptRevisions);
                Debug.WriteLine(string.Format("Done"));
            }
            catch (Exception ex)
            {
               throw ex;
            }
            finally
            {
                Debug.WriteLine(string.Format("Complete"));
            }

            return outFile;
        }

    }

    public static class Tools
    {

        /// <summary>
        /// Returns true if a collection of documents should be published with document markup when converted to PDF
        /// </summary>
        /// <param name="DocumentCollection"></param>
        /// <returns></returns>
        public static Boolean PublishDocumentShowingMarkUp(List<System.IO.FileInfo> DocumentCollection)
        {
            return PublishDocumentShowingMarkUp(null, DocumentCollection, true);
        }

        /// <summary>
        /// Returns true if a collection of documents should be published with document markup when converted to PDF
        /// </summary>
        /// <param name="DocumentCollection"></param>
        /// <returns></returns>
        public static Boolean PublishDocumentShowingMarkUp(IWin32Window Owner, List<System.IO.FileInfo> DocumentCollection)
        {
            return PublishDocumentShowingMarkUp(Owner, DocumentCollection, true);
        }

        /// <summary>
        /// Returns true if a collection of documents should be published with document markup when converted to PDF
        /// </summary>
        /// <param name="DocumentCollection"></param>
        /// <returns></returns>
        public static Boolean PublishDocumentShowingMarkUp(IWin32Window Owner, List<System.IO.FileInfo> DocumentCollection, bool ShowPrompt)
        {
            if (!DocumentCollectionContainsAWordDocument(DocumentCollection))
            {
                return false;
            }
            else if (Session.CurrentSession.PromptToPublishPDFWithTrackChanges)
            {
                if (ShowPrompt)
                {
                    DialogResult res = MessageBox.Show(Owner, Session.CurrentSession.Resources.GetMessage("PDFTRKCHGWRN1", "One or more document(s) you selected to convert to PDF may contain tracked changes, would you like to publish showing document markup?", "").Text, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (res == DialogResult.Yes)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
         }

        /// <summary>
        /// Return true if a collection of documents contains a Word document
        /// </summary>
        /// <param name="DocumentCollection"></param>
        /// <returns></returns>
        public static Boolean DocumentCollectionContainsAWordDocument(List<System.IO.FileInfo> DocumentCollection)
        {
            foreach (System.IO.FileInfo doc in DocumentCollection)
            {
                string docExtension = doc.Extension.ToLower();
                if (docExtension == ".doc" || docExtension == ".docx" || docExtension == ".docm")
                    return true;
            }
            return false;
        }

        public static List<System.IO.FileInfo> DocumentFileInfoList(IStorageItem[] DocumentCollection)
        {
            List<System.IO.FileInfo> docFileNames = new List<System.IO.FileInfo>();
            foreach (IStorageItem docFile in DocumentCollection)
            {
                docFileNames.Add(docFile.GetIdealLocalFile());
            }
            return docFileNames;
        }

        /// <summary>
        /// Checks if a document is protected with a password.
        /// </summary>
        /// <param name="filePath">Document file path.</param>
        /// <returns>True if a document is protected, otherwise False.</returns>
        public static bool IsDocumentProtected(string filePath)
        {
            if (!File.Exists(filePath))
                return false;

            switch (Path.GetExtension(filePath).ToLower())
            {
                case ".doc":
                case ".docx":
                case ".docm":
                    new Aspose.Words.License().SetLicense("Aspose.Total.lic");
                    return Aspose.Words.FileFormatUtil.DetectFileFormat(filePath).IsEncrypted;
                case ".xls":
                case ".xlsx":
                case ".xlsm":
                    new Aspose.Cells.License().SetLicense("Aspose.Total.lic");
                    return Aspose.Cells.FileFormatUtil.DetectFileFormat(filePath).IsEncrypted;
                case ".ppt":
                case ".pptx":
                case ".pptm":
                    new Aspose.Slides.License().SetLicense("Aspose.Total.lic");
                    return Aspose.Slides.PresentationFactory.Instance.GetPresentationInfo(filePath).IsEncrypted;
                case ".pdf":
                    new Aspose.Pdf.License().SetLicense("Aspose.Total.lic");
                    using (var pdfFileInfo = new Aspose.Pdf.Facades.PdfFileInfo(filePath))
                        return pdfFileInfo.HasOpenPassword;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Displays frmPasswordRequest dialog and returns entered password without validation.
        /// </summary>
        /// <param name="displayName">Information to be shown on the password prompt, e.g. document name.</param>
        /// <returns>An entered password or null if the dialog has been canceled.</returns>
        public static string PromptForPassword(string displayName)
        {
            string password = null;
            if (Environment.UserInteractive)
            {
                var args = new System.ComponentModel.CancelEventArgs(true);
                var passwordProtected = new PasswordProtected(displayName);
                var OnPasswordRequest = typeof(Session).GetMethod("OnPasswordRequest", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                OnPasswordRequest.Invoke(Session.CurrentSession, new object[] { passwordProtected, args });
                if (!args.Cancel)
                    password = passwordProtected.CurrentPassword;
            }
            return password;
        }

        #region PasswordProtected

        private sealed class PasswordProtected : FWBS.Common.IPasswordProtected
        {
            private readonly string _displayName;

            public PasswordProtected(string displayName) { _displayName = displayName; }

            public bool IsInternal { get { return false; } }

            public string PasswordHint { get { return string.Empty; } }

            public bool HasPassword { get { return true; } }

            public string CurrentPassword { get; set; }

            public string ToPasswordString() { return _displayName; }

            public void ValidatePassword() { }

            public void PasswordAuthenticate(string userName, string password) { }
        }

        #endregion PasswordProtected
    }

}