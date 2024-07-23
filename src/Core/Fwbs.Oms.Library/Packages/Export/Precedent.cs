using System;
using System.Collections.Generic;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class Precedent : ExportBase, IDisposable, ILinkedObjects
    {
        #region Fields
        private DataSet _dsprecedents = new DataSet("PRECEDENTS");
        private string _code = "";
        private string _name = "";
        private string _desc = "";
        private FWBS.OMS.Precedent _prec;

        #endregion

        #region Contructors
        public Precedent(string Code, TreeView TreeView)
        {
            if (TreeView == null)
                throw new Exception(Session.CurrentSession.Resources.GetMessage("NOBJPLSCRTRVW", "Error No ''FWBS.OMS.DESIGN.EXPORT.TREEVIEW'' Object Please create a blank TreeView and Recompile", "").Text);
            _code = Code;
            _treeview = TreeView;
            _prec = FWBS.OMS.Precedent.GetPrecedent(Convert.ToInt64(Code));

            _name = _prec.Description;
            _desc = "Precedent [" + _code + "] " 
                    + Environment.NewLine 
                    + Environment.NewLine + _name 
                    + Environment.NewLine 
                    + Environment.NewLine + "Library : " + _prec.Library
                    + Environment.NewLine + "Type : " + _prec.PrecedentType
                    + Environment.NewLine + "Category : " + _prec.Category 
                    + Environment.NewLine + "Subcategory : " + _prec.SubCategory
                    + Environment.NewLine + "Minorcategory : " + _prec.MinorCategory;

            // Main Precedent
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dsprecedents.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbPrecedents where PrecID = @Code", "PRECEDENT", false, paramlist));

            _dsprecedents.Tables["PRECEDENT"].Rows[0]["brID"] = DBNull.Value;

            // Precedent Storage
            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dsprecedents.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbPrecedentStorage where PrecID = @Code", "PRECEDENTSTORAGE", false, paramlist));

            // dbActivites
            paramlist = new IDataParameter[2];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code1", _dsprecedents.Tables["PRECEDENT"].Rows[0]["PrecTimeRecCode"]);
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("Code2", _dsprecedents.Tables["PRECEDENT"].Rows[0]["precTimeRecInCode"]);
            _dsprecedents.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbActivities where actCode = @Code1 or actCode = @Code2", "ACTIVITIES", false, paramlist));

            // dbContactType 
            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", _dsprecedents.Tables["PRECEDENT"].Rows[0]["PrecAddressee"]);
            _dsprecedents.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbContactType where typeCode = @Code", "CONTACTTYPES", false, paramlist));

            // dbDirectory 
            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", _dsprecedents.Tables["PRECEDENT"].Rows[0]["PrecDirID"]);
            _dsprecedents.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbDirectory where dirID = @Code", "DIRECTORY", false, paramlist));

            // dbDocumentType  
            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", _dsprecedents.Tables["PRECEDENT"].Rows[0]["PrecType"]);
            _dsprecedents.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbDocumentType where typeCode = @Code", "DOCTYPE", false, paramlist));

            // dbLanguage  
            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", _dsprecedents.Tables["PRECEDENT"].Rows[0]["PrecLanguage"]);
            _dsprecedents.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbLanguage where langCode = @Code", "LANGUAGE", false, paramlist));

            // dbStorageProvider   
            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", _dsprecedents.Tables["PRECEDENT"].Rows[0]["PrecLocation"]);
            _dsprecedents.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbStorageProvider where spID = @Code", "STORAGEPROVIDER", false, paramlist));

            // Multi-Precedent Info
            paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", Code);
            _dsprecedents.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbPrecedentMulti where multiMasterID = @Code", "PRECEDENTMULTI", false, paramlist));

            if (_dsprecedents.Tables.Contains("PRECEDENTMULTI"))
                BuildChildPrecedentList();
        }

        #endregion

        #region Private Methods

        private void BuildChildPrecedentList()
        {
            linkedobjects = new List<LinkedObject>();
            foreach (System.Data.DataRow r in _dsprecedents.Tables["PRECEDENTMULTI"].Rows)
            {
                string slcode = OMS.Session.CurrentSession.DefaultSystemSearchList(FWBS.OMS.SystemSearchLists.PackagePrecedents);
                LinkedObject mp = new LinkedObject(slcode, Convert.ToString(r["multiChildID"]), PackageTypes.Precedents);
                linkedobjects.Add(mp);
            }
        }

        #endregion

        #region Public Methods
        public override void ExportTo(string Directory)
        {
            System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(Directory);
            _directory = _directory.CreateSubdirectory("Precedents");
            _directory = _directory.CreateSubdirectory(_code);

            if (_dsprecedents.Tables["PRECEDENTSTORAGE"].Rows.Count == 0 && _prec.GetExtraInfo("PrecPath") != DBNull.Value)
            {
                DocumentManagement.Storage.StorageProvider store = ((DocumentManagement.Storage.IStorageItem)_prec).GetStorageProvider();
                System.IO.FileInfo source = store.Fetch(_prec, true).LocalFile;
                string filename = _prec.PrecedentPath;
                System.IO.FileInfo destination = new System.IO.FileInfo(_directory.FullName + @"\" + filename);
                destination.Directory.Create();
                if (source != null)
                    source.CopyTo(_directory.FullName + @"\" + filename, true);
            }
            System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
            _dsprecedents.WriteXml(_filename.FullName, XmlWriteMode.WriteSchema);
            TreeView.FieldReplacer.Add("PrecTimeRecCode", "Outbound Time Activity Code", "The Precedent Time Activity Code for Outbound Documents", _prec.TimeRecordingActivityCode, "PRECEDENT", "SELECT actCode, dbo.GetCodeLookupDesc('TIMEACTCODE', actCode, @UI) as actDesc FROM DBACTIVITIES", "PRECEDENTS");
            TreeView.FieldReplacer.Add("PrecTimeRecIncode", "Inbound Time Activity Code", "The Precedent Time Activity Code for Inbound Documents", _prec.TimeRecordingActivityInwardCode, "PRECEDENT", "SELECT actCode, dbo.GetCodeLookupDesc('TIMEACTCODE', actCode, @UI) as actDesc FROM DBACTIVITIES", "PRECEDENTS");
            TreeView.FieldReplacer.Add("brID", "Branch", "The Branch that will get exclusive access to the Precedent", "", "PRECEDENT", "SELECT brID, brName FROM DBBranch", "PRECEDENTS");
            TreeView.FieldReplacer.Add("PrecAddressee", "Contact Type", "The Contact Type (Addressee) for this Precedent", _prec.ContactType, "PRECEDENT", "SELECT typeCode, dbo.GetCodeLookupDesc('CONTTYPE', typeCode, @UI) as typeDesc FROM dbContactType", "PRECEDENTS", false);
            TreeView.FieldReplacer.Add("PrecDirID", "The Precedent Location", "The Root Locations for the Precedent", _prec.DirectoryID.ToString(), "PRECEDENT", "SELECT dirID, dirPath FROM dbDirectory", "PRECEDENTS", false);
            TreeView.FieldReplacer.Add("PrecType", "The Precedent Type", "The Precedent Type", _prec.PrecedentType, "PRECEDENT", "SELECT typeCode, dbo.GetCodeLookupDesc('DOCTYPE', typeCode, @UI) as typeDesc FROM dbDocumentType", "PRECEDENTS", false);
            treeviewParentID = TreeView.Add(34, _prec.ID.ToString(), _prec.Title, this.Active, treeviewParentID, PackageTypes.Precedents, _desc, this.RootImportable, this.RunOnce);

            string script = _prec.Script.Code;
            if (script != "")
            {
                Scripts _script = new Scripts(script, TreeView);
                _script.TreeViewParentID = treeviewParentID;
                _script.RootImportable = false;
                _script.ExportTo(Directory);
                _script.Dispose();
            }
        }

        #endregion

        #region IDisposable Members
        public void Dispose()
        {
            _dsprecedents.Dispose();
        }
        #endregion
    }
}
