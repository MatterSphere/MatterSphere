using System.IO;
using System.Text;
using Aspose.Pdf;
using Aspose.Pdf.Devices;

namespace OcrDocumentReader
{
    public class PdfOcrTextReader : OcrTextReader
    {
        internal PdfOcrTextReader(FileInfo fileInfo) : base(fileInfo)
        {
            var lic = new Aspose.Pdf.License();
            lic.SetLicense("Aspose.Total.lic");
        }

        protected override void LoadImage()
        {
        }

        protected override string GetContentInternal()
        {
            int pagesCount;
            using (Document pdfDocument = new Document(_fileInfo.FullName))
            {
                pagesCount = pdfDocument.Pages.Count;
            }
            var results = _asposeOcr.RecognizePdf(_fileInfo.FullName,
                                                  new Aspose.OCR.DocumentRecognitionSettings() { 
                                                      PagesNumber = pagesCount, 
                                                      Language = Aspose.OCR.Language.Eng });
            var recognizedText = new StringBuilder();
            foreach (var result in results)
            {
                recognizedText.AppendLine(result.RecognitionText);
            }
            return recognizedText.ToString();
        }
    }
}
