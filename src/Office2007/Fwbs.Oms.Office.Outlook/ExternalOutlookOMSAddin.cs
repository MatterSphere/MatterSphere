using System;
using System.IO;
using System.Runtime.InteropServices;
using Fwbs.Oms.Office.Common;

namespace Fwbs.Oms.Office.Outlook
{

    [Guid("56620968-9FE3-45c2-BC75-F4C34715A819")]
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.None)]
    public class ExternalOutlookOMSAddin : ExternalOfficeOMSAddin, IExternalOutlookOMSAddin
    {
        
        public ExternalOutlookOMSAddin(OfficeOMSAddin addin) : base(addin)
        {
        }

        public ExternalOutlookOMSAddin()
        {
            // Blank Constructor override.
        }

        #region IExternalOutlookOMSAddin

        public ucFolderPage GetFolderHomePage()
        {
            return new ucFolderPage(Addin);
        }

        public void WinFormsControlCleanUp()
        {
            // Do any clean up here
        }

        #endregion


        internal static string CreateWebViewHtmFile()
        {
            return CreateWebViewHtmFile("");
        }

        /// <summary>
        /// Override to supply the FileName for the HTM File, this is the File only not the path.
        /// </summary>
        /// <param name="inFilename">The Filename is concatenated to the end of the FWBSPath and should only provide a string with an .HTM Extension</param>
        /// <returns></returns>
        internal static string CreateWebViewHtmFile(string inFilename)
        {
            string webViewFile;
            string webViewDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"FWBS\Outlook");
            if (Directory.Exists(webViewDirectory) == false)
            {
                Directory.CreateDirectory(webViewDirectory);
            }
            if (String.IsNullOrEmpty(inFilename))
            {
                webViewFile = Path.Combine(webViewDirectory, "FWBSDEFAULTVIEW" + ".htm");
            }
            else
            {
                webViewFile = Path.Combine(webViewDirectory, inFilename);
            }

            // Open a file stream and text writer for the Web view stream
            using (FileStream stm = new FileStream(webViewFile, FileMode.OpenOrCreate, FileAccess.Write))
            using (TextWriter writer = new StreamWriter(stm, System.Text.Encoding.Unicode))
            {
                try
                {
                    writer.Write(Properties.Resources.WebViewHtmFileContent);
                    writer.Close();
                    stm.Close();
                }
                catch (System.IO.IOException)
                {
                    System.Diagnostics.Debug.WriteLine("WebView htm file cannot be created");
                }
            }

            return webViewFile;
        }
    }
}
