using System;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class Report : ExportBase, IDisposable
    {
        #region Fields
        private DataSet _dsreport = new DataSet("REPORT");
        private string _code = "";
        private string _name = "";
        private string _desc = "";
        #endregion

        #region Contructors
        public Report(string Code, TreeView TreeView)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _treeview = TreeView;
            _code = Code;
            _name = FWBS.OMS.CodeLookup.GetLookup("OMSSEARCH", _code);
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dsreport.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT rptCode,rptVersion,rptBLOB,rptPubName,rptScript,rptKeywords,rptNotes,Created,CreatedBy,Updated,UpdatedBy FROM dbReport WHERE rptCode = @Code", "REPORT", false, paramlist));

            _desc = "Reports [" + _code + "] " + Environment.NewLine + Environment.NewLine + _name + Environment.NewLine + Environment.NewLine + "Version : " + Convert.ToString(_dsreport.Tables[0].Rows[0]["rptVersion"]);

            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dsreport.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbSearchListConfig WHERE schCode = @Code", "SEARCH", false, paramlist));

            string call = Convert.ToString(_dsreport.Tables["SEARCH"].Rows[0]["schSourceCall"]);
            string omstype = Convert.ToString(_dsreport.Tables["SEARCH"].Rows[0]["schSourceType"]);

            if (call.ToUpper().StartsWith("SELECT ") == false && omstype != "INSTANCE" && omstype != "DYNAMIC")
            {
                paramlist = new IDataParameter[1];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("name", call);
                _dsreport.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteProcedureTable("sprExportProcedure", "STOREPROC", false, paramlist));
                _desc += Environment.NewLine + Environment.NewLine + "Stored Procedure : " + call;
                if (_desc.Length >= 490) _desc = _desc.Substring(0, 490) + "...";
            }

            CodeLookupType _lookups = new CodeLookupType("OMSSEARCH", _code);
            _lookups.Add("REPORT", _code);
            _dsreport.Tables.Add(_lookups.DataTable);

        }
        #endregion

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            treeviewParentID = TreeView.Add(21, _code, _code, this.Active, treeviewParentID, PackageTypes.Reports, _desc, this.RootImportable, this.RunOnce);
            System.IO.FileInfo _filename = null;
            System.IO.DirectoryInfo _directory = null;
            _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("Reports");
            _directory = _directory.CreateSubdirectory(_code);
            _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");

            string enqform = Convert.ToString(_dsreport.Tables["SEARCH"].Rows[0]["schEnquiry"]);
            if (enqform != "")
            {
                using (EnquiryForm _enquiry = new EnquiryForm(enqform, TreeView))
                {
                    _enquiry.TreeViewParentID = treeviewParentID;
                    _enquiry.RootImportable = false;
                    _enquiry.ExportTo(Directory);
                }
            }

            _dsreport.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _dsreport.Dispose();
        }

        #endregion
    }
}
