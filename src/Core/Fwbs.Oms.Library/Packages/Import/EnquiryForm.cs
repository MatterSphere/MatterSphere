using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class EnquiryForm : ImportBase, IDisposable
    {
        #region Fields
        private System.Data.DataSet _dataset;
        private CodeLookups _lookups;
        private CodeLookups _cueTextCodeLookups;
        private int _enquiryid = 0;
        private System.IO.DirectoryInfo _root;
        private System.Collections.ArrayList _extdone = new System.Collections.ArrayList();

        #endregion

        #region Contructors
        public EnquiryForm(string FileName)
        {
            _dataset = new System.Data.DataSet();
            if (System.IO.File.Exists(FileName) == false)
                return;
            _dataset.ReadXml(FileName);
            _source = _dataset;
            Random rnd = new Random();
            _enquiryid = rnd.Next(int.MaxValue);
            System.IO.FileInfo _filename = new System.IO.FileInfo(FileName);
            _root = _filename.Directory.Parent.Parent;
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            /*
            * if Enquiry Header Version is Greater than the Stored Enquiry Form then Delete the Enquiry Form and Re-Import
            * if Enquiry Header Does not Exists then Import
            * 
            * Import Pages
            * 
            * Import Question
            * 
            * Check Code Lookup
            * Check Extended Data
            * Check Control
            * Check Data List
            * 
            * Loop
            * 
            */

            /*
            * Contruct a Import Table Object
            */
            ImportTable _import = new ImportTable(this.Fieldreplacer);

            _lookups = new CodeLookups(_dataset.Tables["LOOKUPS"], "ENQQUESTION");
            _cueTextCodeLookups = new CodeLookups(_dataset.Tables["LOOKUPS"], "ENQQUESTCUETXT");

            /*
             * IMPORT HEADER
             */
            string enqcode = Convert.ToString(_dataset.Tables["ENQUIRY"].Rows[0]["enqCode"]);
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", enqcode);
            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbEnquiry where enqCode = @Code", "DBENQUIRY", false, paramlist);

            bool create = true;
            long sourceversion = 0;
            long destinversion = Convert.ToInt64(_dataset.Tables["ENQUIRY"].Rows[0]["enqVersion"]);
            if (_table.Rows.Count > 0) sourceversion = Convert.ToInt64(_table.Rows[0]["enqVersion"]);

            long destinuser = (long)FWBS.OMS.SystemUsers.Admin;
            long sourceuser = destinuser;
            if (_table.Rows.Count > 0) sourceuser = FWBS.Common.ConvertDef.ToInt64(_table.Rows[0]["UpdatedBy"], Session.CurrentSession.Administrator.ID);

            if (sourceuser != destinuser || (sourceuser == destinuser && destinversion < sourceversion))
            {
                string sourceusername = "{Unknown}";
                try { sourceusername = new User(Convert.ToInt32(sourceuser)).FullName; }
                catch { }
                AskEventArgs askargs = null;
                if (sourceuser != destinuser)
                    askargs = new AskEventArgs("ALLRDYEXISTV2", "The %1% [%2%] already exists. Do you want to overwrite %1% version %3% with version %4% ?" + Environment.NewLine + Environment.NewLine + "Last Modified By : %5%", "", FWBS.OMS.AskResult.No, "Screen", enqcode, sourceversion.ToString(), destinversion.ToString(), sourceusername);
                else
                    askargs = new AskEventArgs("ALLRDYEXISTV1", "The %1% [%2%] already exists. Do you want to overwrite %1% version %3% with version %4% ?", "", FWBS.OMS.AskResult.No, "Screen", enqcode, sourceversion.ToString(), destinversion.ToString());
                Session.CurrentSession.OnAsk(this, askargs);
                if (askargs.Result == FWBS.OMS.AskResult.Yes)
                    create = true;
                else
                    create = false;
            }

            OnProgress("Importing Screen : " + enqcode);


            if (create)
            {
                if (sourceversion > 0)
                {
                    paramlist = new IDataParameter[1];
                    paramlist[0] = Session.CurrentSession.Connection.AddParameter("id", _table.Rows[0]["enqID"]);
                    Session.CurrentSession.Connection.ExecuteSQLScalar("DELETE FROM dbEnquiryPage WHERE enqid = @id", paramlist);
                    paramlist = new IDataParameter[1];
                    paramlist[0] = Session.CurrentSession.Connection.AddParameter("id", _table.Rows[0]["enqID"]);
                    Session.CurrentSession.Connection.ExecuteSQLScalar("DELETE FROM dbEnquiryQuestion WHERE enqid = @id", paramlist);
                    paramlist = new IDataParameter[1];
                    paramlist[0] = Session.CurrentSession.Connection.AddParameter("id", _table.Rows[0]["enqID"]);
                    Session.CurrentSession.Connection.ExecuteSQLScalar("DELETE FROM dbEnquiryMethod WHERE enqid = @id", paramlist);
                    paramlist = new IDataParameter[1];
                    paramlist[0] = Session.CurrentSession.Connection.AddParameter("id", _table.Rows[0]["enqID"]);
                    Session.CurrentSession.Connection.ExecuteSQLScalar("DELETE FROM dbEnquiryDataSource WHERE enqid = @id", paramlist);
                }

                _import.OverrideColumnValue += new SetValueOverideHandler(Page_OverrideColumnValue);
                if (_table.Rows.Count == 0)
                    _import.Import(_dataset.Tables["ENQUIRY"], _table);
                else
                    _import.ImportRowOver(_dataset.Tables["ENQUIRY"], _table, 0);
                Session.CurrentSession.Connection.Update(_table, "select * from dbEnquiry");

                _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbEnquiryMethod", "ENQMETHOD", false, new System.Data.SqlClient.SqlParameter[] { });
                DataView dv = new DataView(_dataset.Tables["METHODS"]);
                dv.RowFilter = "enqID = " + Convert.ToString(_dataset.Tables["ENQUIRY"].Rows[0]["enqID"]);
                _import.ImportView(dv, _table);
                Session.CurrentSession.Connection.Update(_table, "select * from dbEnquiryMethod");
                _import.OverrideColumnValue -= new SetValueOverideHandler(Page_OverrideColumnValue);

                FWBS.OMS.CodeLookup.Create("ENQHEADER", enqcode, Convert.ToString(_dataset.Tables["ENQUIRY"].Rows[0]["enqdesc"]).Replace("~", ""), "", CodeLookup.DefaultCulture, true, true, true);

                string script = Convert.ToString(_dataset.Tables["ENQUIRY"].Rows[0]["enqScript"]);
                if (script != "")
                {
                    Scripts _script = new Scripts(_root.FullName + @"\Scripts\" + script + @"\manifest.xml");
                    _script.Fieldreplacer = this.Fieldreplacer;
                    _script.Execute();
                    _script.Dispose();
                }

                _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbEnquiryCommand", "ENQCOMMAND", false, new System.Data.SqlClient.SqlParameter[] { });
                _import.NewOnlyImport(_dataset.Tables["COMMANDS"], _table, "CMDCODE", true);

                Session.CurrentSession.Connection.Update(_table, "select * from dbEnquiryCommand");

                /*
                 * IMPORT PAGES
                 */
                _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbEnquiryPage", "DBENQUIRYPAGE", true, new System.Data.SqlClient.SqlParameter[] { });
                _import.OverrideColumnValue += new SetValueOverideHandler(Page_OverrideColumnValue);
                _import.Import(_dataset.Tables["PAGES"], _table);
                _import.OverrideColumnValue -= new SetValueOverideHandler(Page_OverrideColumnValue);
                Session.CurrentSession.Connection.Update(_table, "select * from dbEnquiryPage");

                /*
                 * IMPORT ANY MISSING CONTROLS
                 */
                Session.CurrentSession.Connection.Connect(true);
                _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbEnquiryControl", "DBENQUIRYCONTROLS", false, new System.Data.SqlClient.SqlParameter[] { });
                _import.NewOnlyImport(_dataset.Tables["CONTROLS"], _table, "ctrlID", false);
                paramlist = new IDataParameter[8];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("@ctrlID", SqlDbType.Int, 6, DBNull.Value);
                paramlist[0].SourceColumn = "ctrlID";
                paramlist[1] = Session.CurrentSession.Connection.AddParameter("@ctrlCode", SqlDbType.NVarChar, 15, DBNull.Value);
                paramlist[1].SourceColumn = "ctrlCode";
                paramlist[2] = Session.CurrentSession.Connection.AddParameter("@ctrlGroup", SqlDbType.NVarChar, 15, DBNull.Value);
                paramlist[2].SourceColumn = "ctrlGroup";
                paramlist[3] = Session.CurrentSession.Connection.AddParameter("@ctrlSystem", SqlDbType.Bit, 1, DBNull.Value);
                paramlist[3].SourceColumn = "ctrlSystem";
                paramlist[4] = Session.CurrentSession.Connection.AddParameter("@ctrlWinType", SqlDbType.NVarChar, 500, DBNull.Value);
                paramlist[4].SourceColumn = "ctrlWinType";
                paramlist[5] = Session.CurrentSession.Connection.AddParameter("@ctrlWebType", SqlDbType.NVarChar, 500, DBNull.Value);
                paramlist[5].SourceColumn = "ctrlWebType";
                paramlist[6] = Session.CurrentSession.Connection.AddParameter("@ctrlPDAType", SqlDbType.NVarChar, 500, DBNull.Value);
                paramlist[6].SourceColumn = "ctrlPDAType";
                paramlist[7] = Session.CurrentSession.Connection.AddParameter("@ctrlAssemblyFileName", SqlDbType.NVarChar, 100, DBNull.Value);
                paramlist[7].SourceColumn = "ctrlAssemblyFileName";

                Session.CurrentSession.Connection.Update(_table, "INSERT INTO dbEnquiryControl(ctrlID, ctrlCode , ctrlGroup , ctrlSystem , ctrlWinType , ctrlWebType , ctrlPDAType, ctrlAssemblyFileName ) VALUES (@ctrlID,  @ctrlCode , @ctrlGroup , @ctrlSystem , @ctrlWinType , @ctrlWebType , @ctrlPDAType, @ctrlAssemblyFileName)", "", "", paramlist);
                Session.CurrentSession.Connection.Disconnect(true);

                /*
                 * IMPORT QUESTIONS
                 */
                _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbEnquiryQuestion", "DBENQUIRYQUESTIONS", true, new System.Data.SqlClient.SqlParameter[] { });
                _table.Columns["quid"].AutoIncrement = true;

                _import.OverrideColumnValue += new SetValueOverideHandler(Question_OverrideColumnValue);
                _import.Import(_dataset.Tables["QUESTIONS"], _table);
                _import.OverrideColumnValue -= new SetValueOverideHandler(Question_OverrideColumnValue);
                Session.CurrentSession.Connection.Update(_table, "select * from dbEnquiryQuestion");

                /*
                 * IMPORT ENQUIRY DATASOURCE
                 */
                _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbEnquiryDataSource", "DBENQUIRYDATASOURCE", true, new System.Data.SqlClient.SqlParameter[] { });
                _import.OverrideColumnValue += new SetValueOverideHandler(Question_OverrideColumnValue);
                _import.Import(_dataset.Tables["DATASOURCE"], _table);
                _import.OverrideColumnValue -= new SetValueOverideHandler(Question_OverrideColumnValue);
                Session.CurrentSession.Connection.Update(_table, "select * from dbEnquiryDataSource");
                return true;
            }
            else
                return false;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dataset.Dispose();
        }
        #endregion

        #region Special Overrides
        private void Page_OverrideColumnValue(string name, object fromvalue, out object value)
        {
            if (name == "enqID")
                value = _enquiryid;
            else if (name == "enqWelcomeHeaderCode")
                value = CodeLookups.ImportOver(this.Fieldreplacer, "ENQPAGE", Convert.ToString(fromvalue), _dataset.Tables["LOOKUPS"]);
            else if (name == "enqWelcomeTextCode")
                value = CodeLookups.ImportOver(this.Fieldreplacer, "ENQWELCOME", Convert.ToString(fromvalue), _dataset.Tables["LOOKUPS"]);
            else if (name == "pgeCode")
                value = CodeLookups.ImportOver(this.Fieldreplacer, "ENQPAGE", Convert.ToString(fromvalue), _dataset.Tables["LOOKUPS"]);
            else
                value = null;

        }

        private void Question_OverrideColumnValue(string name, object fromvalue, out object value)
        {
            if (name == "quID")
                value = "<default>";
            else if (name == "enqID")
                value = _enquiryid;
            else if (name == "quExtendedData" && fromvalue != DBNull.Value)
            {
                if (_extdone.Contains(fromvalue) == false)
                {
                    _extdone.Add(fromvalue);
                    ExtendedData ext = new ExtendedData(_root.FullName + @"\ExtendedData\" + fromvalue + @"\manifest.xml");
                    ext.Execute();
                    ext.Dispose();
                }
                value = null;
            }
            else if (name == "quDataList" && fromvalue != DBNull.Value)
            {
                DataLists data = new DataLists(_root.FullName + @"\DataLists\" + fromvalue + @"\manifest.xml");
                data.Execute();
                data.Dispose();
                value = fromvalue;
            }
            else if (name == "quCode" && fromvalue != DBNull.Value && Convert.ToString(fromvalue) != "")
            {
                DataView msg = new DataView(_lookups.DataTable);
                msg.RowFilter = "cdCode = '" + Convert.ToString(fromvalue) + "'";
                foreach (DataRowView drv in msg)
                {
                    if (_lookups.CheckCodeExists(Convert.ToString(fromvalue)) == false)
                        _lookups.Create("ENQQUESTION", Convert.ToString(fromvalue), Convert.ToString(drv["cdDesc"]), Convert.ToString(drv["cdHelp"]), Convert.ToString(drv["cdUICultureInfo"]));
                }
                value = Convert.ToString(fromvalue);
            }
            else if (name == "quFilter" && fromvalue != DBNull.Value)
            {
                var fromValueStr = Convert.ToString(fromvalue);
                if (!string.IsNullOrEmpty(fromValueStr) && fromValueStr.Contains("CueTextCode"))
                {
                    DataView dataView = new DataView(_cueTextCodeLookups.DataTable);
                    var configSetting = new FWBS.Common.ConfigSetting(fromValueStr);
                    configSetting.Current = "custom";
                    string cueTextCode = configSetting.DocCurrent.GetAttribute("CueTextCode");

                    if (!string.IsNullOrEmpty(cueTextCode))
                    {
                        dataView.RowFilter = "cdCode = '" + cueTextCode + "'";
                        foreach (DataRowView drv in dataView)
                        {
                            if (_cueTextCodeLookups.CheckCodeExists(cueTextCode) == false)
                                _cueTextCodeLookups.Create("ENQQUESTCUETXT", cueTextCode, Convert.ToString(drv["cdDesc"]), Convert.ToString(drv["cdHelp"]), Convert.ToString(drv["cdUICultureInfo"]));
                        }
                    }
                }
                value = null;
            }
            else
                value = null;
        }
        #endregion
    }
}
