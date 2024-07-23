using System;
using System.Data;
using FWBS.WF.Packaging;

namespace FWBS.OMS.Design.Export
{
    public class Workflows : FWBS.OMS.Design.Export.ExportBase, IDisposable
    {
        #region Fields
        private DataSet _dsworkflows = new DataSet("WORKFLOWS");
        private string _code = "";
        private string _name = "";
        private string _desc = "";
        #endregion

        #region Contructors
        public Workflows(string code, TreeView treeView)
        {
            if (treeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _code = code;
            _treeview = treeView;
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", code);
            _dsworkflows.Tables.Add(Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbWorkflow where wfCode = @Code", "WORKFLOWS", false, paramlist));
            _name = _code;
            if (_dsworkflows.Tables["WORKFLOWS"].Rows.Count > 0)
                _desc = "Workflows [" + _code + "]" + Environment.NewLine + "Version : " + Convert.ToString(_dsworkflows.Tables["WORKFLOWS"].Rows[0]["wfVersion"]);
            else
                throw new Exception(Session.CurrentSession.Resources.GetMessage("ERRNOWRKFFRCD", "Error no Workflow for Code : %1%", "", _code).Text);

            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", code);
            _dsworkflows.Tables.Add(Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where (cdType = 'WORKFLOW' AND cdCode = @Code)", "LOOKUPS", false, paramlist));

        }
        #endregion

        #region Public Methods
        public override void ExportTo(string directory)
        {
            System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(directory);
            _directory = _directory.CreateSubdirectory("Workflows");
            _directory = _directory.CreateSubdirectory(_code);

            System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
            _dsworkflows.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);

            treeviewParentID = TreeView.Add(40, _code, _name, this.Active, treeviewParentID, PackageTypes.Workflows, _desc, this.RootImportable, this.RunOnce);

            WorkflowXaml item = new WorkflowXaml();
            item.Fetch(_code);
            var references = item.GetScriptCodes();

            foreach (var script in references)
            {
                if (script != "")
                {
                    Scripts _script = new Scripts(script, TreeView);
                    _script.TreeViewParentID = treeviewParentID;
                    _script.RootImportable = false;
                    _script.ExportTo(directory);
                    _script.Dispose();
                }
            }
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
