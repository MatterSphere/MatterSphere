using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FWBS.Scanning
{
    public partial class frmConvertProgress : Form
    {
        #region "Form Events"

        public frmConvertProgress()
        {
            InitializeComponent();
            this.Text = Application.SafeTopLevelCaptionFormat;
        }
        
        private void frmConvertProgress_Shown(object sender, EventArgs e)
        {
            bgw.RunWorkerAsync();
        }

        #endregion
        

        #region "Background Worker Events"

        private void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            GetFilesForTifConversion();
        }

        private void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void bgw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        #endregion
        

        #region "Tif conversion methods"

        private void GetFilesForTifConversion()
        {
            Dictionary<ArrayList, DirectoryInfo> _convertList = (Dictionary<ArrayList, DirectoryInfo>)this.Tag;
            this.Tag = ConvertFilesToTif(_convertList.First().Key, _convertList.First().Value);
        }
        
        public Dictionary<string, string> ConvertFilesToTif(ArrayList foundFiles, DirectoryInfo ScanLocation)
        {
            if (!Directory.Exists(string.Concat(ScanLocation.FullName, Properties.CONVERT_FOLDER)))
                Directory.CreateDirectory(string.Concat(ScanLocation.FullName, Properties.CONVERT_FOLDER));

            FileInfo[] _filesToConvert = (System.IO.FileInfo[])foundFiles.ToArray(typeof(System.IO.FileInfo));
            string destination = string.Concat(ScanLocation.FullName, Properties.CONVERT_FILENAME);

            Dictionary<string, string> _dic = new Dictionary<string, string>();
                        
            for (int i = 0; i < _filesToConvert.Length; i++)
            {
                FileInfo f = _filesToConvert[i];
                System.Diagnostics.Debug.WriteLine(string.Format("In: {0}, Out: {1}", f.FullName, string.Format(destination, f.Name)));

                SaveAsTif(f.FullName, string.Format(destination, f.Name));
                
                _dic.Add(string.Format(destination, f.Name).ToLower(), f.FullName.ToLower());

                bgw.ReportProgress(((i + 1) * 100) / _filesToConvert.Length);
            }

            return _dic;
        }
        
        public void SaveAsTif(string SourceFile, string OutputFile)
        {
            try
            {
                FWBS.OMS.TIFFConversion.ConvertTIFF _tiffConverter = new FWBS.OMS.TIFFConversion.ConvertTIFF();
                _tiffConverter.SingleFile(SourceFile, OutputFile);
            }
            catch (Exception ex)
            {
                Invoke((Action)delegate ()
                {
                    FWBS.OMS.UI.Windows.MessageBox.Show(this, SourceFile + "\n\n" + ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                });
            }
        }

        #endregion



    }


    public class Properties
    {
        public const string CONVERT_FOLDER = @"\converted";
        public const string CONVERT_FILENAME = @"\converted\~{0}.tif";
    }

}
