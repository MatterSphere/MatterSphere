using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using FWBS.Common;
using FWBS.OMS.Data.Exceptions;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS
{
    public class DashboardSysObject : LookupTypeDescriptor, IExtraInfo, IUpdateable, IDisposable
    {
        private const string SQL_STATEMENT = "SELECT * FROM [dbo].[dbDashboards]";
        private const string SQL_PARAMETRIZED_STATEMENT = "SELECT * FROM [dbo].[dbDashboards] WHERE {0} = {1}";
        private const string TABLE_NAME = "DASHBOARDS";

        private DataTable _dataTable = null;

        public DashboardSysObject()
        {
            _dataTable = Session.CurrentSession.Connection.ExecuteSQLTable(string.Format(SQL_PARAMETRIZED_STATEMENT, 1, -1), TABLE_NAME, new IDataParameter[0]);

            _dataTable.Columns["dshSystem"].DefaultValue = false;
            _dataTable.Columns["dshActive"].DefaultValue = true;
            _dataTable.Columns["dshTypeCompatible"].DefaultValue = 0;
            _dataTable.Columns["dshConfig"].DefaultValue = "<config></config>";

            Global.CreateBlankRecord(ref _dataTable, true);
        }

        public DashboardSysObject(string code)
        {
            Fetch(code, null);
        }

        public DashboardSysObject(DashboardSysObject clone) : this()
        {
            _dataTable.Rows[0].ItemArray = ((IExtraInfo)clone).GetDataTable().Rows[0].ItemArray;
            _dataTable.Rows[0]["dshSystem"] = false;
        }

        public event EventHandler Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            Changed?.Invoke(this, e);
        }

        public virtual string Code
        {
            get
            {
                return Convert.ToString(((IExtraInfo)this).GetExtraInfo("dshObjCode"));
            }
            set
            {
                ((IExtraInfo)this).SetExtraInfo("dshObjCode", value);
            }
        }

        public virtual DashboardTypes DashboardType
        {
            get
            {
                return (DashboardTypes)ConvertDef.ToEnum(((IExtraInfo)this).GetExtraInfo("dshTypeCompatible"), DashboardTypes.UserCompatible);
            }
            set
            {
                if (value != DashboardTypes.UserCompatible && IsSystemDashboard(Code))
                {
                    throw new OMSException2("DSHMTCTRISSYST", "System Dashboard should be UserCompatible.");
                }
                ((IExtraInfo)this).SetExtraInfo("dshTypeCompatible", (byte)value);
            }
        }

        #region IUpdatable

        [Browsable(false)]
        public bool IsDirty
        {
            get
            {
                return _dataTable.GetChanges() != null;
            }
        }

        [Browsable(false)]
        public bool IsNew
        {
            get
            {
                try
                {
                    return _dataTable.Rows[0].RowState == DataRowState.Added;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void Update()
        {
            if (_dataTable.GetChanges() != null)
            {
                SetPrimaryKey();
                var dataSet = new DataSet("Dashboards");
                try
                {
                    dataSet.Tables.Add(_dataTable);
                    Session.CurrentSession.Connection.Update(dataSet, TABLE_NAME, SQL_STATEMENT);
                    dataSet.Tables.Remove(_dataTable);
                }
                catch (ConnectionException cex)
                {
                    dataSet.Tables.Remove(_dataTable);
                    SqlException sqlex = cex.InnerException as SqlException;
                    if (sqlex != null)
                        throw new OMSException2("DSHCODEEXISTS", "The Dashboard with the Code '%1%' already exists please change the Code", "", sqlex, true, Code);

                    throw;
                }
                catch (Exception ex)
                {
                    dataSet.Tables.Remove(_dataTable);
                    throw ex;
                }
            }
        }

        public void Refresh()
        {
            Refresh(false);
        }

        public void Refresh(bool applyChanges)
        {
            if (IsNew)
                return;

            DataTable changes = _dataTable.GetChanges();

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(Code, changes.Rows[0]);
            else
                Fetch(Code, null);
        }

        public void Cancel()
        {
            _dataTable.RejectChanges();
        }

        #endregion

        #region IDisposable

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_dataTable != null)
                {
                    _dataTable.Dispose();
                    _dataTable = null;
                }
            }
        }

        #endregion

        #region IExtraInfo

        void IExtraInfo.SetExtraInfo(string fieldName, object val)
        {
            if (val is DateTime)
            {
                DateTime dteval = (DateTime)val;
                if (dteval.Kind == DateTimeKind.Unspecified)
                    val = DateTime.SpecifyKind(dteval, DateTimeKind.Local);
            }

            _dataTable.Rows[0][fieldName] = val;
            OnChanged(EventArgs.Empty);
        }

        object IExtraInfo.GetExtraInfo(string fieldName)
        {
            object val = _dataTable.Rows[0][fieldName];
            if (val is DateTime)
                return ((DateTime)val).ToLocalTime();
            else
                return val;
        }

        Type IExtraInfo.GetExtraInfoType(string fieldName)
        {
            try
            {
                return _dataTable.Columns[fieldName].DataType;
            }
            catch (Exception ex)
            {
                throw new OMSException2("7001", "Error Getting Extra Info Field %1% Probably Not Initialized", ex, true, fieldName);
            }
        }

        DataSet IExtraInfo.GetDataset()
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(((IExtraInfo)this).GetDataTable());
            return ds;
        }

        DataTable IExtraInfo.GetDataTable()
        {
            return _dataTable.Copy();
        }

        #endregion

        private void Fetch(string code, DataRow merge)
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.NVarChar, 15, code);
            
            var data = Session.CurrentSession.Connection.ExecuteSQLTable(string.Format(SQL_PARAMETRIZED_STATEMENT, "dshObjCode", "@Code"), TABLE_NAME, paramlist);

            if ((data == null) || (data.Rows.Count == 0))
                throw new ArgumentException($"No dashboard found with {code} code.");

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _dataTable = data;
        }

        private void SetPrimaryKey()
        {
            if (_dataTable.PrimaryKey == null || _dataTable.PrimaryKey.Length == 0)
                _dataTable.PrimaryKey = new DataColumn[1] { _dataTable.Columns["dshObjCode"] };
        }

        #region Static

        public static DataTable GetDashboards()
        {
            Session.CurrentSession.CheckLoggedIn();
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("UI", System.Data.SqlDbType.VarChar, 15, System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            string lstsql = "SELECT *, dbo.GetCodeLookupDesc('DASHBOARDS', dshObjCode, @UI) AS dashboardDesc FROM [dbo].[dbDashboards]";
            return Session.CurrentSession.Connection.ExecuteSQLTable(lstsql, TABLE_NAME, paramlist);
        }

        public static bool Exists(string code)
        {
            Session.CurrentSession.CheckLoggedIn();
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.NVarChar, 15, code);
            var data = Session.CurrentSession.Connection.ExecuteSQLTable(string.Format(SQL_PARAMETRIZED_STATEMENT, "dshObjCode", "@Code"), TABLE_NAME, paramlist);
            return data.Rows.Count > 0;
        }

        public static bool PermanentlyDelete(string code)
        {
            Session.CurrentSession.CheckLoggedIn();
            ValidateDashboardRemoving(code);
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.VarChar, 15, code);
            Session.CurrentSession.Connection.ExecuteSQL("DELETE FROM [dbo].[dbDashboards] WHERE dshObjCode = @Code", paramlist);
            Session.CurrentSession.Connection.ExecuteSQL("DELETE FROM [dbo].[dbUserDashboards] WHERE dshObjCode = @Code", paramlist);
            return true;
        }

        public static bool Delete(string code)
        {
            return UpdateIsActiveState(code, false);
        }

        public static bool Restore(string code)
        {
            return UpdateIsActiveState(code, true);
        }

        private static void ValidateDashboardRemoving(string code)
        {
            if (IsSystemDashboard(code))
            {
                throw new OMSException2("DSHCODEISSYSTEM", "System Dashboard can not be removed.", "");
            }

            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.VarChar, 15, code);
            var commandCentreDt = Session.CurrentSession.Connection.ExecuteSQLTable("SELECT 0 FROM [dbo].[dbCommandCentreType] WHERE typeXML like '%' + @Code + '%'", TABLE_NAME, paramlist);
            if (commandCentreDt.Rows.Count > 0)
            {
                throw new OMSException2("DSHOBJINUSE", "Dashboard is used in CommandCentre and can not be removed.", "");
            }
        }

        private static bool UpdateIsActiveState(string code, bool isActive)
        {
            Session.CurrentSession.CheckLoggedIn();
            IDataParameter[] paramlist = new IDataParameter[2];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.VarChar, 15, code);
            paramlist[1] = Session.CurrentSession.Connection.AddParameter("IsActive", isActive ? 1 : 0);
            Session.CurrentSession.Connection.ExecuteSQL("UPDATE [dbo].[dbDashboards] set dshActive = @IsActive WHERE dshObjCode = @Code", paramlist);
            return true;
        }

        private static bool IsSystemDashboard(string code)
        {
            IDataParameter[] paramlist = new IDataParameter[1];
            paramlist[0] = Session.CurrentSession.Connection.AddParameter("Code", System.Data.SqlDbType.NVarChar, 15, code);
            var dashboardsDt = Session.CurrentSession.Connection.ExecuteSQLTable(string.Format(SQL_PARAMETRIZED_STATEMENT, "dshObjCode", "@Code"), TABLE_NAME, paramlist);
            return dashboardsDt.Rows.Count > 0 && ConvertDef.ToBoolean(dashboardsDt.Rows[0]["dshSystem"], false);
        }

        #endregion
    }

    public enum DashboardTypes: byte
    {
        UserCompatible = 0,
        MatterCompatible = 1
    }
}
