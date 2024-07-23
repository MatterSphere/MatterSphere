using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class DataPackage : ImportBase, IDisposable
    {
        #region Fields
        private DataSet _dsdatapackage = new DataSet("DATAPACKAGE");
        private System.IO.FileInfo _filename;
        #endregion

        #region Contructors
        public DataPackage(string FileName)
        {
            _dsdatapackage.ReadXml(FileName);
            _source = _dsdatapackage;
            _filename = new System.IO.FileInfo(FileName);
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            OnProgress("Importing Data Package : " + Convert.ToString(_dsdatapackage.Tables["PACKAGEHEAD"].Rows[0]["pkdCode"]));
            FWBS.OMS.Design.Package.PackageData _package = new FWBS.OMS.Design.Package.PackageData(_dsdatapackage.Tables["PACKAGEHEAD"]);
            _package.LoadTable(_dsdatapackage.Tables["SOURCE"], this.Fieldreplacer);
            _package.UpdateData();
            FWBS.OMS.Design.Package.PackageData.Delete(Convert.ToString(_dsdatapackage.Tables["PACKAGEHEAD"].Rows[0]["pkdCode"]));
            _package.Description = Convert.ToString(_dsdatapackage.Tables["PACKAGEHEAD"].Rows[0]["pkdDesc"]);
            _package.Update();
            return true;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dsdatapackage.Dispose();
        }
        #endregion
    }
}
