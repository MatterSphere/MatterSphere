using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class FileManagement : ImportBase, IDisposable
    {
        #region Fields
        DataSet _dsfma = new DataSet("FILEMANAGEMENT");
        System.IO.FileInfo _filename;
        private System.IO.DirectoryInfo _root;
        #endregion

        #region Contructors
        public FileManagement(string FileName)
        {
            _dsfma.ReadXml(FileName);
            _filename = new System.IO.FileInfo(FileName);
            _source = _dsfma;
            _root = _filename.Directory.Parent.Parent;
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            try
            {
                ImportTable _import = new ImportTable(this.Fieldreplacer);
                IDataParameter[] paramlist = new IDataParameter[1];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Convert.ToString(_dsfma.Tables["FILEMANAGEMENT"].Rows[0]["appCode"]));

                OnProgress("Importing File Management : " + Convert.ToString(_dsfma.Tables["FILEMANAGEMENT"].Rows[0]["appCode"]));

                DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbFileManagementApplication where appCode = @Code", "FILEMANAGEMENT", false, paramlist);
                if (_table.Rows.Count > 0)
                    _import.ImportRowOver(_dsfma.Tables["FILEMANAGEMENT"], _table, 0);
                else
                    _import.Import(_dsfma.Tables["FILEMANAGEMENT"], _table);
                Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbFileManagementApplication");

                if (_dsfma.Tables.Contains("LOOKUPS"))
                {
                    CodeLookups _lookups = new CodeLookups(_dsfma.Tables["LOOKUPS"], "FMAPPLICATION");
                    DataView msg1 = new DataView(_lookups.DataTable);
                    string fromvalue = Convert.ToString(_dsfma.Tables["FILEMANAGEMENT"].Rows[0]["appCode"]);
                    msg1.RowFilter = "cdCode = '" + fromvalue + "'";
                    foreach (DataRowView drv in msg1)
                    {
                        if (_lookups.CheckCodeExists(Convert.ToString(fromvalue)) == false)
                            _lookups.Create("FMAPPLICATION", Convert.ToString(fromvalue), Convert.ToString(drv["cdDesc"]), Convert.ToString(drv["cdHelp"]), Convert.ToString(drv["cdUICultureInfo"]));
                    }
                }

                string script = Convert.ToString(_dsfma.Tables["FILEMANAGEMENT"].Rows[0]["appScript"]);
                if (script != "")
                {
                    Scripts _script = new Scripts(_root.FullName + @"\Scripts\" + script + @"\manifest.xml");
                    _script.Fieldreplacer = this.Fieldreplacer;
                    _script.Execute();
                    _script.Dispose();
                }

                CodeLookupType _code = new CodeLookupType(_root.FullName + @"\CodeLookups\FMACTIONS\manifest.xml");
                _code.Execute();
                _code.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dsfma.Dispose();
        }
        #endregion
    }
}
