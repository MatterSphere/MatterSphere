using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using Aspose.OCR;

namespace OcrDocumentReader
{
    public class OcrTextReader : IDisposable
    {
        protected readonly FileInfo _fileInfo;
        protected readonly AsposeOcr _asposeOcr;

        internal OcrTextReader(FileInfo fileInfo)
        {
            _fileInfo = fileInfo;

            License license = new Aspose.OCR.License();
            license.SetLicense("Aspose.Total.lic");

            _asposeOcr = new AsposeOcr();
        }

        public void Dispose()
        {
        }

        public static OcrTextReader GetOcrTextReader(FileInfo fileInfo)
        {
            var extension = fileInfo.Extension.ToUpperInvariant();
            switch (extension)
            {
                case ".PDF":
                    return new PdfOcrTextReader(fileInfo);
                case ".PNG":
                case ".JPEG":
                case ".JPG":
                case ".GIF":
                case ".BMP":
                case ".TIFF":
                case ".TIF":
                    return new OcrTextReader(fileInfo);
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileInfo));
            }
        }

        public string GetContent()
        {
            LoadImage();
            return GetContentInternal();
        }

        protected virtual string GetContentInternal()
        {
            return _asposeOcr.RecognizeImage(_fileInfo.FullName);
        }

        protected virtual void LoadImage()
        {
        }
    }
}
