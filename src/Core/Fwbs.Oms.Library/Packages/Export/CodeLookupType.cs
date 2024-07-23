using System;
using System.Data;

namespace FWBS.OMS.Design.Export
{
    public class CodeLookupType : ExportBase,IDisposable
	{
		#region Fields
		private DataSet _dscodelookups = new DataSet("CODELOOKUPS");
		private string _codes = "";
		private string _type = "";
		#endregion

		#region Contructors
		public CodeLookupType(string Type)
		{
			_type = Type;
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Type",Type);
			_dscodelookups.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where (cdType = @Type) OR (cdCode = @Type AND cdGroup = 1)","CODELOOKUPS",false,paramlist));
			_dscodelookups.Tables["CODELOOKUPS"].ExtendedProperties.Add(_dscodelookups.Tables["CODELOOKUPS"].ExtendedProperties.Count,Type);
		}
		
		public CodeLookupType(string Type,string Codes)
		{
			_type = Type;
			_codes = Codes;
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Type",Type);
			_dscodelookups.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where cdType = @Type and cdCode in('" + Codes + "')","CODELOOKUPS",false,paramlist));
			_dscodelookups.Tables["CODELOOKUPS"].ExtendedProperties.Add(_dscodelookups.Tables["CODELOOKUPS"].ExtendedProperties.Count,Type);
		}

		public CodeLookupType(string Type, TreeView treeview)
		{
			_type = Type;
			_treeview = treeview;
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Type",Type);
			_dscodelookups.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where (cdType = @Type) OR (cdCode = @Type AND cdGroup = 1)","CODELOOKUPS",false,paramlist));
			_dscodelookups.Tables["CODELOOKUPS"].ExtendedProperties.Add(_dscodelookups.Tables["CODELOOKUPS"].ExtendedProperties.Count,Type);
		}

        public CodeLookupType(string Type, TreeView treeview, string SearchIn, string FormatBy)
        {
            _type = Type;
            _treeview = treeview;
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Type", Type);
            _dscodelookups.Tables.Add(OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where (cdType = @Type) OR (cdCode = @Type AND cdGroup = 1)", "CODELOOKUPS", false, paramlist));
            if (SearchIn != "")
            {
                DataView n = new DataView(_dscodelookups.Tables["CODELOOKUPS"], "", "", DataViewRowState.OriginalRows);
                foreach (DataRowView dr in n)
                {
                    string cd = Convert.ToString(dr["cdCode"]);
                    if (FormatBy != "") cd = String.Format(FormatBy, cd);
                    if (SearchIn.IndexOf(cd) == -1) dr.Delete();
                }
                _dscodelookups.Tables["CODELOOKUPS"].AcceptChanges();
            }

            _dscodelookups.Tables["CODELOOKUPS"].ExtendedProperties.Add(_dscodelookups.Tables["CODELOOKUPS"].ExtendedProperties.Count, Type);
        }


		#endregion

		#region Public Methods
		public void Add(string Type,string Codes)
		{
			_type = Type;
			_codes = Codes;
			IDataParameter[] paramlist = new IDataParameter[1];
			paramlist[0] = Session.CurrentSession.Connection.AddParameter("Type",Type);
			DataTable _add = OMS.Session.CurrentSession.Connection.ExecuteSQLTable("SELECT * FROM dbCodeLookup where cdType = @Type and cdCode in('" + Codes + "')","CODELOOKUPS",false,paramlist);
			FWBS.OMS.Design.Import.ImportTable _import = new FWBS.OMS.Design.Import.ImportTable(null);
			_import.NewOnlyImport(_add,_dscodelookups.Tables["CODELOOKUPS"],"cdType,cdCode,cdUICultureInfo",true);
		}
		
		public override void ExportTo(string Directory)
		{
			System.IO.DirectoryInfo _directory = new System.IO.DirectoryInfo(Directory);
			_directory = _directory.CreateSubdirectory("CodeLookups");
			_directory = _directory.CreateSubdirectory(_type);

			System.IO.FileInfo _filename = new System.IO.FileInfo(_directory.FullName + @"\" + "manifest.xml");
			this.TreeView.Add(0,_type,_type + " (" + this.Count.ToString() + ")",this.Active,this.TreeViewParentID,PackageTypes.CodeLookups,"",this.RootImportable,this.RunOnce);
			_dscodelookups.WriteXml(_filename.FullName,XmlWriteMode.WriteSchema);

		}

		#endregion

		#region Properties
		public DataTable DataTable
		{
			get
			{
				return _dscodelookups.Tables["CODELOOKUPS"].Copy();
			}
		}

		public int Count
		{
			get
			{
				return _dscodelookups.Tables["CODELOOKUPS"].Rows.Count;
			}
		}
		#endregion
		
		#region IDisposable Members
		public void Dispose()
		{
			_dscodelookups.Dispose();
		}
		#endregion
	}
}
