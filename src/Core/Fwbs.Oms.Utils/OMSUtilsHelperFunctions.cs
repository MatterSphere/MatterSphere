using System;
using System.IO;
using System.Reflection;
using FWBS.OMS.DocumentManagement;

namespace FWBS.OMS.Utils
{
    class HelperFunctions
    {
        internal struct PDFWindowInfo
        {
            internal IntPtr WinHandle;
            internal FileSystemInfo FileSysInfo;
            internal OMSDocument OMSDoc;
            internal DocumentVersion OMSDocVersion;

            internal PDFWindowInfo(IntPtr winHandle, FileSystemInfo LastChangedPDF, OMSDocument PDFDocument, DocumentVersion PDFDocumentVersion)
            {
                WinHandle = winHandle;
                FileSysInfo = LastChangedPDF;
                OMSDoc = PDFDocument;
                OMSDocVersion = PDFDocumentVersion;
            }
        }

        internal static void SetLocalFileObjects(System.IO.FileSystemEventArgs e, out string fullpath, out string ext, out System.IO.FileSystemInfo info)
        {
            fullpath = e.FullPath.ToUpper();
            ext = System.IO.Path.GetExtension(fullpath).ToUpperInvariant();
            if (ext.StartsWith("."))
                ext = ext.Substring(1);
            info = new System.IO.FileInfo(fullpath);
        }

        internal static bool ExtensionIsPDF(string ext)
        {
            return (ext.ToUpper() == "PDF");
        }

        internal static bool FileIsHiddenOrTemporary(FileSystemInfo file)
        {
            return (file.Attributes == (file.Attributes | System.IO.FileAttributes.Hidden)) || (file.Attributes == (file.Attributes | System.IO.FileAttributes.Temporary));
        }

        internal static bool OMSDocumentIsNotNull(OMSDocument doc)
        {
            return (doc != null);
        }

        internal static bool OMSDocumentIsPDF(OMSDocument doc)
        {
            return (new FileInfo(doc.GetIdealLocalFile().FullName).Extension.ToUpper() == ".PDF");
        }

        internal static Fwbs.WinFinder.Window GetParentWindow(Fwbs.WinFinder.WindowList wl)
        {
            Fwbs.WinFinder.Window parent = null;

            foreach (Fwbs.WinFinder.Window w in wl)
            {
                if (w.IsVisible && w.IsValid && w.IsHung == false && w.Parent == null)
                {
                    parent = w;
                    break;
                }
            }
            return parent;
        }

        /// <summary>
        /// Replace invalid characters from the document description the same way as VirtualDriveReplaceInvalidCharsInFileName SQL function does.
        /// </summary>
        /// <param name="description">The document description string.</param>
        /// <returns>A file name formed from the doc description.</returns>
        internal static string DocDescriptionToFileName(string description)
        {
            var sb = new System.Text.StringBuilder(description.Trim());
            sb.Replace(':', '_').Replace('/', '_').Replace('\\', '_').Replace('*', '_').Replace('|', '_').Replace('?', '_').Replace('<', '_').Replace('>', '_');
            sb.Replace('"', '\'').Replace('\r', ' ').Replace('\n', ' ').Replace('\t', ' ');
            return sb.ToString();
        }

        internal static Interfaces.IVirtualDrive CreateVirtualDriveInstance(MainWindow window)
        {
            Interfaces.IVirtualDrive virtualDrive = null;
            try
            {
                Assembly assembly = Assembly.LoadFrom(Path.Combine(Session.CurrentSession.GetBaseApplicationLocation(), "OMS.Drive.dll"));
                Type type = assembly.GetType("FWBS.OMS.Drive.DMSOperations");
                virtualDrive = Activator.CreateInstance(type, window) as Interfaces.IVirtualDrive;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
            return virtualDrive;
        }
    }
}
