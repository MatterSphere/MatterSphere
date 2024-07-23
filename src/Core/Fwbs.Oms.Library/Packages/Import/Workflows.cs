using System;
using System.Data;
using System.IO;
using FWBS.WF.Packaging;

namespace FWBS.OMS.Design.Import
{
    internal class Workflows : ImportBase, IDisposable
    {
        #region Fields
        DataSet _dsworkflows = new DataSet("WORKFLOWS");
        System.IO.FileInfo _filename;
        DirectoryInfo _root;
        #endregion

        #region Contructors
        public Workflows(string FileName)
        {
            _dsworkflows.ReadXml(FileName);
            _filename = new System.IO.FileInfo(FileName);
            _source = _dsworkflows;
            _root = _filename.Directory.Parent.Parent;
        }
        #endregion

        #region Public Methods
        public override bool Execute()
        {
            ImportTable _import = new ImportTable(this.Fieldreplacer);
            IDataParameter[] paramlist = new IDataParameter[1];
            string code = Convert.ToString(_dsworkflows.Tables["WORKFLOWS"].Rows[0]["wfCode"]);
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", code);

            OnProgress("Importing Workflow : " + code);

            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbWorkflow where wfCode = @Code", "WORKFLOWS", false, paramlist);

            bool create = true;
            long destinuser = (long)FWBS.OMS.SystemUsers.Admin;
            long sourceuser = destinuser;
            if (_table.Rows.Count > 0) sourceuser = FWBS.Common.ConvertDef.ToInt64(_table.Rows[0]["UpdatedBy"], Session.CurrentSession.Administrator.ID);

            if (sourceuser != destinuser)
            {
                string sourceusername = "{Unknown}";
                try 
                { 
                    var user = FWBS.OMS.User.GetUser(Convert.ToInt32(sourceuser));
                    sourceusername = user.FullName; 
                }
                catch { }
                AskEventArgs askargs = new AskEventArgs("ALLRDYEXISTV3", "The %1% [%2%] already exists. Do you want to overwrite ?" + Environment.NewLine + Environment.NewLine + "Last Modified By : %3%", "", FWBS.OMS.AskResult.No, "Workflows", code, sourceusername);
                Session.CurrentSession.OnAsk(this,askargs);
                if (askargs.Result == FWBS.OMS.AskResult.Yes)
                    create = true;
                else
                    create = false;
            }


            if (create)
            {
                if (_table.Rows.Count > 0)
                    _import.ImportRowOver(_dsworkflows.Tables["WORKFLOWS"], _table, 0);
                else
                    _import.Import(_dsworkflows.Tables["WORKFLOWS"], _table);
                Session.CurrentSession.Connection.Update(_table, "SELECT * FROM dbWorkflow");
            }


            if (_dsworkflows.Tables.Contains("LOOKUPS"))
            {
                CodeLookups _lookups = new CodeLookups(_dsworkflows.Tables["LOOKUPS"], "WORKFLOW");
                DataView msg1 = new DataView(_lookups.DataTable);
                string fromvalue = Convert.ToString(_dsworkflows.Tables["WORKFLOWS"].Rows[0]["wfCodelookup"]);
                msg1.RowFilter = "cdCode = '" + fromvalue + "'";
                foreach (DataRowView drv in msg1)
                {
                    if (_lookups.CheckCodeExists(Convert.ToString(fromvalue)) == false)
                        _lookups.Create("WORKFLOW", Convert.ToString(fromvalue), Convert.ToString(drv["cdDesc"]), Convert.ToString(drv["cdHelp"]), Convert.ToString(drv["cdUICultureInfo"]));
                }
            }

            WorkflowXaml item = new WorkflowXaml();
            item.Fetch(code);
            var references = item.GetScriptCodes();
            foreach (var script in references)
            {
                Scripts _script = new Scripts(_root.FullName + @"\Scripts\" + script + @"\manifest.xml");
                _script.Fieldreplacer = this.Fieldreplacer;
                _script.Execute();
                _script.Dispose();
            }


            return true;
        }
        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dsworkflows.Dispose();
        }
        #endregion
    }
}
