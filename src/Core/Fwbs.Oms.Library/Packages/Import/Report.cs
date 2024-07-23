using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class Report : ImportBase, IDisposable
    {
        #region Fields
        DataSet _dsreports = new DataSet("REPORTS");
        System.IO.FileInfo _filename;
        System.IO.DirectoryInfo _root;
        #endregion

        #region Constructors
        public Report(string FileName)
        {
            _dsreports.ReadXml(FileName);
            _filename = new System.IO.FileInfo(FileName);
            _root = _filename.Directory.Parent.Parent;
            _source = _dsreports;
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            ImportTable _import = new ImportTable(this.Fieldreplacer);

            OnProgress("Importing Report : " + Convert.ToString(_dsreports.Tables["SEARCH"].Rows[0]["schCode"]));

            string enqform = Convert.ToString(_dsreports.Tables["SEARCH"].Rows[0]["schEnquiry"]);
            if (enqform != "")
            {
                using (EnquiryForm _enquiry = new EnquiryForm(_root.FullName + @"\EnquiryForms\" + enqform + @"\manifest.xml"))
                {
                    _enquiry.Fieldreplacer = this.Fieldreplacer;
                    if (!_enquiry.Execute())
                        return false;
                }
            }

            CodeLookupType _lookup = new CodeLookupType(_dsreports.Tables["CODELOOKUPS"].Copy());
            _lookup.Execute();
            _lookup.Dispose();

            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Convert.ToString(_dsreports.Tables["SEARCH"].Rows[0]["schCode"]));
            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbSearchListConfig where schCode = @Code", "SEARCH", false, paramlist);

            if (_table.Rows.Count > 0)
                _import.ImportRowOver(_dsreports.Tables["SEARCH"], _table, 0);
            else
                _import.Import(_dsreports.Tables["SEARCH"], _table);
            Session.CurrentSession.Connection.Update(_table, "select * from dbSearchListConfig");

            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Convert.ToString(_dsreports.Tables["REPORT"].Rows[0]["rptCode"]));
            _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbReport where rptCode = @Code", "REPORT", false, paramlist);
            if (_table.Rows.Count > 0)
                _import.ImportRowOver(_dsreports.Tables["REPORT"], _table, 0);
            else
                _import.Import(_dsreports.Tables["REPORT"], _table);
            Session.CurrentSession.Connection.Update(_table, "select * from dbReport");

            return true;
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _dsreports.Dispose();
        }

        #endregion
    }
}
