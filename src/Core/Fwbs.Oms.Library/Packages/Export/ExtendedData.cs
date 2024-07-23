using System;
using System.Collections.Generic;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class ExtendedData : ExportBase, IDisposable, ILinkedObjects
    {
        #region Fields
        private DataSet _dsextend = new DataSet("EXTENDEDDATA");
        private string _code = "";
        private string _name = "";
        private string _desc = "";

        #endregion

        #region Contructors
        public ExtendedData(string Code, TreeView TreeView)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _code = Code;
            _treeview = TreeView;
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dsextend.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbExtendedData where extCode = @Code", "EXTENDEDDATA", false, paramlist));

            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dsextend.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbOMSObjects where objType = 'ExtData' AND ObjWinNamespace = @Code", "OBJECTS", false, paramlist));

            if (_dsextend.Tables["OBJECTS"].Rows.Count > 0)
            {
                paramlist = new IDataParameter[2];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("OCode", _dsextend.Tables["OBJECTS"].Rows[0]["objCode"]);
                paramlist[1] = Session.CurrentSession.Connection.AddParameter("Code", Code);
                _dsextend.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where (cdType = 'EXTENDEDDATA' AND cdCode = @Code) OR (cdType = 'OMSOBJECT' AND cdCode = @OCode)", "LOOKUPS", false, paramlist));
            }
            else
            {
                paramlist = new IDataParameter[1];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
                _dsextend.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where (cdType = 'EXTENDEDDATA' AND cdCode = @Code)", "LOOKUPS", false, paramlist));
            }

            _name = _code;
            _desc = _code;

            linkedobjects = new List<LinkedObject>();
            linkedobjects = FWBS.OMS.Design.Export.LinkedObjectCollector.BuildLinkedObjectList("SELECT * FROM dbOMSObjects WHERE ObjCode = @Code", _code, "OMSOBJECT");
        }
        #endregion

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("ExtendedData");
            _directory = _directory.CreateSubdirectory(_code);

            System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
            _dsextend.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);

            treeviewParentID = TreeView.Add(8, _code, _name, this.Active, treeviewParentID, PackageTypes.ExtendedData, _desc, this.RootImportable, this.RunOnce);
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dsextend.Dispose();
        }
        #endregion
    }
}
