using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal class CodeLookups
    {
        #region Fields
        private DataTable _lookups;
        private DataTable _fromlookups;
        #endregion

        #region Constructors
        public CodeLookups(DataTable fromLookups, string Type)
        {
            _lookups = CodeLookup.GetAllLookups(Type);
            _fromlookups = fromLookups;
        }
        #endregion

        #region Static
        public static string ImportOver(FWBS.OMS.Design.Export.FieldReplacer fieldreplacer, string type, string code, DataTable _lookupdt)
        {
            ImportTable _import = new ImportTable(fieldreplacer);
            DataView _lookups = new DataView(_lookupdt);
            _lookups.RowFilter = "cdType = '" + type.Replace("'", "''") + "' AND cdCode = '" + code.Replace("'", "''") + "'";
            if (_lookups.Count > 0)
            {
                IDataParameter[] paramlist = new IDataParameter[2];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", code);
                paramlist[1] = Session.CurrentSession.Connection.AddParameter("Type", type);
                Session.CurrentSession.Connection.ExecuteSQLScalar("DELETE FROM dbCodeLookup WHERE cdType = @Type and cdCode = @Code", paramlist);
            }
            DataTable _table = Session.CurrentSession.Connection.ExecuteSQLTable("select * from dbCodeLookup", "LOOKUPS", true, new System.Data.SqlClient.SqlParameter[] { });
            _import.ImportView(_lookups, _table);
            Session.CurrentSession.Connection.Update(_table, "select * from dbCodeLookup");
            return code;
        }
        #endregion

        #region Public Methods
        public void Create(string type, string code, string description, string help, string culture)
        {
            description = description.Trim(" :".ToCharArray());
            DataRow nr = _lookups.NewRow();
            nr["cdCode"] = code;
            nr["cdDesc"] = description;
            nr["cdhelp"] = help;
            nr["cduicultureinfo"] = culture;
            _lookups.Rows.Add(nr);
            IDataParameter[] paramlist = new IDataParameter[5];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("@Type", SqlDbType.NVarChar, 15, type);
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("@Code", SqlDbType.NVarChar, 15, code);
            paramlist[2] = Session.CurrentSession.Connection.AddParameter("@Culture", SqlDbType.NVarChar, 15, culture);
            paramlist[3] = Session.CurrentSession.Connection.AddParameter("@Desc", SqlDbType.NVarChar, 1000, description);
            paramlist[4] = Session.CurrentSession.Connection.AddParameter("@Help", SqlDbType.NVarChar, 500, help);
            try
            {
                Session.CurrentSession.Connection.ExecuteSQL("INSERT INTO dbCodeLookup (cdType,cdCode,cdUICultureInfo,cdDesc,cdHelp) VALUES (@Type,@Code,@Culture,@Desc,@Help)", paramlist);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public bool CheckCodeExists(string Code)
        {
            _lookups.DefaultView.RowFilter = "cdCode = '" + Code + "'";
            return (_lookups.DefaultView.Count > 0);
        }

        public string CheckTooDescription(string SearchFor, string Culture)
        {
            SearchFor = SearchFor.Trim(" :".ToCharArray());
            _lookups.DefaultView.RowFilter = "cdDesc = '" + SearchFor.Replace("'", "''") + "' AND cdUICultureInfo = '" + Culture + "'";
            if (_lookups.DefaultView.Count > 0)
            {
                return Convert.ToString(_lookups.DefaultView[0]["cdCode"]);
            }
            else
            {
                return "";
            }
        }

        public DataView GetFromDescriptions(string Code)
        {
            DataView newview = new DataView(_fromlookups);
            newview.RowFilter = "cdCode = '" + Code.Replace("'", "''") + "'";
            if (newview.Count > 0)
            {
                return newview;
            }
            else
            {
                return null;
            }
        }

        public DataTable DataTable
        {
            get
            {
                return _fromlookups;
            }
        }
        #endregion
    }
}
