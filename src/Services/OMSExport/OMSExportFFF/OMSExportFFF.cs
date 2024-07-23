using System;
using System.IO;

namespace FWBS.OMS.OMSEXPORT
{
    /*     
     * Exports a flat file string 
     * First creates the file in a tempory location so the importing process does not grab it
     * then in the finalise method it copies it over to the import location and flags the records as exported.
     * */
    public class OMSExportFFF : OMSExportBase
    {
        /// <summary>
        /// Used when calling registry read function 
        /// </summary>
        private const string APPNAME = "FFF";

        private string _tempFilename = "";
        private string _destFilename = "";


        #region OMSExportBase members

        /// <summary>
        /// Flags the data ready for export
        /// </summary>
        protected override void InitialiseProcess()
        {
            string folder = StaticLibrary.GetSetting("Foldername", "FFF", "");
            string extension = StaticLibrary.GetSetting("FileExtension","FFF","txt");
            string dateFormat =  "{0:" + StaticLibrary.GetSetting("FileNaming","FFF","yyyyMMddHHmmss") + "}";
                        

            if (string.IsNullOrEmpty(folder))
                throw new ApplicationException("Flat File export folder is not configured.");

            if(!Directory.Exists(folder))
                throw new ApplicationException("Flat File export folder does not exist.");


            string file = String.Format(dateFormat, DateTime.Now) + "." + extension;
            
            _destFilename = Path.Combine(folder, file);

            _tempFilename = Path.GetTempFileName();
                        
            // flag data for export
            ExecuteSQL("sprFFFMarkRecords");    
        }
        
        

        protected override object ExportTimeRecord(System.Data.DataRow row)
        {
            string data = Convert.ToString(row["data"]);
                       
            using(TextWriter tw = new StreamWriter(_tempFilename,true))
            {
                tw.WriteLine(data + Environment.NewLine);
            }
            return null;
        }

        protected override void FinaliseProcess()
        {
            //copy temp file to import location
            File.Copy(_tempFilename, _destFilename);
            
            
            // flag as exported
            ExecuteSQL("sprFFFMarkAsProcessed");
        }

        protected override string ExportLookup(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override int ExportUser(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override int ExportFeeEarner(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override object ExportClient(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdateClient(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override object ExportMatter(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdateMatter(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }



        protected override object ExportFinancialRecord(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override object ExportContact(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override bool UpdateContact(System.Data.DataRow row)
        {
            throw new NotImplementedException();
        }

        protected override string ExportObject
        {
            get
            {
                return APPNAME;
            }
        }

        #endregion
    }
}
