using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Aspose.Pdf.Devices;

namespace FWBS.OMS.TIFFConversion
{
    public interface IConverter
    {
        FileInfo ConvertToTIFF(string inName, string outName);
    }

    public class Converter : IConverter
    {
        #region IConverter Members

        public FileInfo ConvertToTIFF(string inName, string outName)
        {
            FileInfo inFile = new FileInfo(inName);

            //Throw out if a temporary file
            if (inFile.Name.StartsWith("~"))
                throw new Exception(string.Format("Temporary Files are not supported.  Cannot process file '{0}'", inName));

            if (!inFile.Exists)
                throw new Exception(string.Format("File '{0}' does not exist or you do not have access to it", inName));
            
            switch (inFile.Extension.ToLower())
            {
                case ".png":
                case ".jpeg":
                case ".jpg":
                case ".gif":
                case ".bmp":
                    SaveImageAsTIFF(inName, outName);
                    return new FileInfo(outName);
                case ".pdf":
                    SavePDFAsTiff(inName, outName);
                    return new FileInfo(outName);
                case ".tiff":
                case ".tif":
                    throw new Exception(string.Format("File with extension '{0}' does not need converting", inFile.Extension.ToLower()));
                default:
                    throw new Exception(string.Format("Extension'{0}' is not supported for TIFF Conversion", inFile.Extension.ToLower()));
            }
        }


        private static void SavePDFAsTiff(string inName, string outName)
        {
            Aspose.Pdf.License l = new Aspose.Pdf.License();
            l.SetLicense("Aspose.Total.lic");

            using (Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(inName))
            {
                TiffDevice tiffDevice = IdealTiffDevice(new Resolution(300));
                tiffDevice.Process(pdfDocument, outName);
            }
        }

        private static TiffDevice IdealTiffDevice(Resolution resolution)
        {
            TiffSettings tiffSettings = IdealTiffSettings();
            TiffDevice tiffDevice = new TiffDevice(resolution, tiffSettings);
            return tiffDevice;
        }

        private static TiffSettings IdealTiffSettings()
        {
            TiffSettings tiffSettings = new TiffSettings();
            tiffSettings.Compression = CompressionType.LZW;
            tiffSettings.Depth = Aspose.Pdf.Devices.ColorDepth.Default;
            tiffSettings.SkipBlankPages = false;
            return tiffSettings;
        }

        /// <summary>
        /// Saves a copy of an image file to a TIFF format 
        /// </summary>
        /// <param name="filename">Source filename of image to be converted</param>
        /// <returns></returns>
        private static void SaveImageAsTIFF(string inName, string outName)
        {
            using (Image img = Image.FromFile(inName))
            {
                img.Save(outName, ImageFormat.Tiff);
            }
        }
        
        #endregion
    }


    public class ConvertTIFF
    {
        public string SingleFile(string SourceFile)
        {
            return SingleFile(new FileInfo(SourceFile));
        }

        public string SingleFile(FileInfo SourceFile)
        {
            return SingleFile(SourceFile, null);
        }

        public string SingleFile(string SourceFile, string OutputFile)
        {
            return SingleFile(new FileInfo(SourceFile), new FileInfo(OutputFile));
        }

        public string SingleFile(FileInfo SourceFile, FileInfo OutputFile)
        {
            string outFile = "";

            if (OutputFile == null)
                outFile = Path.Combine(SourceFile.DirectoryName, SourceFile.Name.Substring(0, SourceFile.Name.Length - SourceFile.Extension.Length) + ".tiff");
            else
                outFile = OutputFile.FullName;

            try
            {
                IConverter ic = new Converter();
                try
                {
                    string inFile = SourceFile.FullName;
                    Debug.WriteLine(string.Format("Convert from '{0}' to {1}", inFile, outFile), "Starting");
                    ic.ConvertToTIFF(inFile, outFile);
                    Debug.WriteLine(string.Format("Done"));
                }
                catch (Exception ex)
                {
                    throw ex;
                }

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
}
