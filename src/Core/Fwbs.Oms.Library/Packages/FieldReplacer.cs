using System;
using System.Data;
namespace FWBS.OMS.Design.Export
{
    public class FieldReplacer
    {
        #region Fields
        protected DataTable _data;
        protected DataRowView _rw = null;
        protected int _pos = -1;
        protected DataView _view = null;
        #endregion

        #region Constructors
        public FieldReplacer()
        {
            _data = new DataTable("FIELDREPLACER");
            _data.Columns.Add("ID", typeof(System.Int32));
            DataColumn c1 = _data.Columns.Add("FieldName", typeof(System.String));
            _data.Columns.Add("FieldDesc", typeof(System.String));
            _data.Columns.Add("FieldHelp", typeof(System.String));
            DataColumn c2 = _data.Columns.Add("Table", typeof(System.String));
            _data.Columns.Add("Select", typeof(System.String));
            DataColumn c3 = _data.Columns.Add("Category", typeof(System.String));
            DataColumn c4 = _data.Columns.Add("FieldValue", typeof(System.Object));
            _data.Columns.Add("FieldValueDesc", typeof(System.String));
            _data.PrimaryKey = new DataColumn[4] { c1, c2, c3, c4 };
            _data.Columns.Add("AllowNull", typeof(System.Boolean));

            _data.Columns["ID"].AutoIncrement = true;
            _data.Columns["ID"].AutoIncrementSeed = 1;
            _data.Columns["ID"].AllowDBNull = false;

        }

        public FieldReplacer(DataTable Table)
        {
            _data = Table;
        }

        internal FieldReplacer(FieldReplacer clone)
        {
            _data = clone._data.Copy();
        }
        #endregion

        #region Public
        public int Add(string FieldName, string FieldDesc, string FieldHelp, string FieldValue, string Table, string Select, string Category)
        {
            return Add(FieldName, FieldDesc, FieldHelp, FieldValue, Table, Select, Category, true);
        }

        public int Add(string FieldName, string FieldDesc, string FieldHelp, string FieldValue, string Table, string Select, string Category, bool AllowNull)
        {
            try
            {
                DataRow dr = _data.NewRow();
                dr["FieldName"] = FieldName;
                dr["FieldDesc"] = FieldDesc;
                dr["FieldHelp"] = FieldHelp;
                dr["FieldValue"] = FieldValue;
                dr["Table"] = Table;
                dr["Select"] = Select;
                dr["Category"] = Category;
                if (_data.Columns.Contains("AllowNull"))
                    dr["AllowNull"] = AllowNull;

                if (Select != "")
                {
                    IDataParameter[] paramlist = new IDataParameter[1];
                    paramlist[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.VarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                    DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(Select, "SOURCE", false, paramlist);
                    dt.DefaultView.Sort = dt.Columns[0].ColumnName;
                    try
                    {
                        dt.DefaultView.RowFilter = dt.Columns[0].ColumnName + " = " + FieldValue;
                    }
                    catch
                    {
                        dt.DefaultView.RowFilter = dt.Columns[0].ColumnName + " = '" + FieldValue + "'";
                    }
                    if (dt.DefaultView.Count == 1 && dt.Columns.Count > 1)
                        dr["FieldValueDesc"] = dt.DefaultView[0][1];
                }


                _data.Rows.Add(dr);
                Goto(Convert.ToInt32(dr["ID"]));
                return this.ID;
            }
            catch
            {
                return -1;
            }
        }

        public void RemoteAt(int Index)
        {
            try
            {
                _data.DefaultView[Index].Delete();
            }
            catch
            { }
        }

        public void Remove(DataRowView Row)
        {
            Row.Row.Delete();
        }

        public bool Goto(string Table, string Field)
        {
            _view = new DataView(_data, "FieldName = '" + Field + "' AND Table = '" + Table + "'", "", DataViewRowState.CurrentRows);
            _pos = -1;
            if (_view.Count > 0)
            {
                _rw = _view[0];
                return true;
            }
            else
                return false;
        }

        public bool Goto(int ID)
        {
            _view = new DataView(_data, "ID = " + ID.ToString(), "", DataViewRowState.CurrentRows);
            _pos = -1;
            if (_view.Count > 0)
            {
                _rw = _view[0];
                return true;
            }
            else
                return false;
        }

        public bool Goto(DataRowView Row)
        {
            try
            {
                _rw = Row;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Clear()
        {
            _data.Rows.Clear();
            _data.AcceptChanges();
        }

        public int Count
        {
            get
            {
                return _data.DefaultView.Count;
            }
        }

        public DataRowView RowView
        {
            get
            {
                return _rw;
            }
        }

        public void Load(System.IO.FileInfo filename)
        {
            DataSet _dataset = new DataSet();
            _dataset.ReadXml(filename.FullName);
            if (_dataset.DataSetName == "FIELDREPLACER")
            {
                _data = _dataset.Tables[0].Copy();
                _data.TableName = "FIELDREPLACER";
                if (_data.Columns.Contains("ChangeType") == false)
                    _data.Columns.Add("ChangeType", typeof(System.Int32));
                if (_data.Columns.Contains("ChangeValue") == false)
                    _data.Columns.Add("ChangeValue", typeof(System.Object));

            }

        }
        #endregion

        #region Properties
        public int ID
        {
            get
            {
                if (_rw != null)
                    return Convert.ToInt32(_rw["ID"]);
                else
                    return 0;
            }
            set
            {
                if (_rw != null)
                    _rw["ID"] = value;
            }
        }

        public object ChangeValue
        {
            get
            {
                if (_rw != null && _data.Columns.Contains("ChangeValue"))
                {
                    if (Convert.ToString(_rw["ChangeValue"]) == "")
                        return DBNull.Value;
                    else
                        return _rw["ChangeValue"];
                }
                else
                    return DBNull.Value;
            }
            set
            {
                if (_rw != null && _data.Columns.Contains("ChangeValue"))
                    _rw["ChangeValue"] = value;
            }
        }

        public int ChangeType
        {
            get
            {
                if (_rw != null && _data.Columns.Contains("ChangeType"))
                    return FWBS.Common.ConvertDef.ToInt32(_rw["ChangeType"], 0);
                else
                    return 0;
            }
            set
            {
                if (_rw != null && _data.Columns.Contains("ChangeType"))
                    _rw["ChangeType"] = value;
            }
        }

        public bool AllowNull
        {
            get
            {
                if (_rw != null && _data.Columns.Contains("AllowNull"))
                    return Convert.ToBoolean(_rw["AllowNull"]);
                else
                    return true;
            }
            set
            {
                if (_rw != null && _data.Columns.Contains("AllowNull"))
                    _rw["AllowNull"] = value;
            }
        }

        public string FieldName
        {
            get
            {
                if (_rw != null)
                    return Convert.ToString(_rw["FieldName"]);
                else
                    return "";
            }
            set
            {
                if (_rw != null)
                    _rw["FieldName"] = value;
            }
        }

        public string FieldDesc
        {
            get
            {
                if (_rw != null)
                    return Convert.ToString(_rw["FieldDesc"]);
                else
                    return "";
            }
            set
            {
                if (_rw != null)
                    _rw["FieldDesc"] = value;
            }
        }

        public DataTable DataSource
        {
            get
            {
                IDataParameter[] paramlist = new IDataParameter[1];
                paramlist[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.VarChar, 10, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                DataTable dt = Session.CurrentSession.Connection.ExecuteSQLTable(this.Select, "SOURCE", false, paramlist);
                dt.DefaultView.Sort = dt.Columns[0].ColumnName;
                return dt;
            }
        }

        public string FieldHelp
        {
            get
            {
                if (_rw != null)
                    return Convert.ToString(_rw["FieldHelp"]);
                else
                    return "";
            }
            set
            {
                if (_rw != null)
                    _rw["FieldHelp"] = value;
            }
        }

        public object FieldValue
        {
            get
            {
                if (_rw != null)
                {
                    if (Convert.ToString(_rw["FieldValue"]) == "")
                        return DBNull.Value;
                    else
                        return _rw["FieldValue"];
                }
                else
                    return DBNull.Value;
            }
            set
            {
                if (_rw != null)
                    _rw["FieldValue"] = value;
            }
        }

        public string Table
        {
            get
            {
                if (_rw != null)
                    return Convert.ToString(_rw["Table"]);
                else
                    return "";
            }
            set
            {
                if (_rw != null)
                    _rw["Table"] = value;
            }
        }

        public string Select
        {
            get
            {
                if (_rw != null)
                    return Convert.ToString(_rw["Select"]);
                else
                    return "";
            }
            set
            {
                if (_rw != null)
                    _rw["Select"] = value;
            }
        }

        public string Category
        {
            get
            {
                if (_rw != null)
                    return Convert.ToString(_rw["Category"]);
                else
                    return "";
            }
            set
            {
                if (_rw != null)
                    _rw["Category"] = value;
            }
        }

        public DataTable Source
        {
            get
            {
                return _data;
            }
        }

        public DataTable Copy
        {
            get
            {
                return _data.Copy();
            }
        }

        public FieldReplacer Clone()
        {
            return new FieldReplacer(this);
        }

        public bool EOF
        {
            get
            {
                return (_pos == _data.DefaultView.Count);
            }
        }
        #endregion
    }
}
