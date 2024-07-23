using System;
using System.Collections.Generic;

namespace FWBS.OMS.Data
{
    using System.Data;

    public sealed class DatabaseInformation
    {
        #region Fields

        private readonly Connection connection;
         
        private readonly HashSet<string> procedures = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> tables = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> views = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> usercontextprocs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        #endregion

        #region Constants

        public const string UserContextParameter = "@USER_CONTEXT";

        #endregion

        #region Constructors

        public DatabaseInformation(Connection connection)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");

            this.connection = connection;
        }

        #endregion

        #region Methods

        public void Clear()
        {
            procedures.Clear();
            tables.Clear();
            views.Clear();
            usercontextprocs.Clear();
            IsLoaded = false;
        }

        public void Refresh()
        {
            Clear();
            Load();
        }

        public void Load()
        {
            if (IsLoaded)
                return;

            foreach (DataRow dr in connection.GetProcedures().Rows)
            {
                var schema = Convert.ToString(dr["SCHEMA"]);
                var name = Convert.ToString(dr["PROCEDURE_NAME"]);

                AddItem(procedures, schema, name);
            }

            foreach (DataRow dr in connection.GetTables().Rows)
            {
                var schema = Convert.ToString(dr["SCHEMA"]);
                var name = Convert.ToString(dr["TABLE_NAME"]);

                AddItem(tables, schema, name);
            }

            foreach (DataRow dr in connection.GetViews().Rows)
            {
                var schema = Convert.ToString(dr["SCHEMA"]);
                var name = Convert.ToString(dr["TABLE_NAME"]);

                AddItem(views, schema, name);
            }


            using (var dv = new DataView(connection.GetParameters(null)))
            {
                dv.RowFilter = String.Format("[PARAMETER_NAME] = '{0}'", UserContextParameter);

                foreach (DataRowView dr in dv)
                {
                    var schema = Convert.ToString(dr["SCHEMA"]);
                    var name = Convert.ToString(dr["PROCEDURE_NAME"]);

                    AddItem(usercontextprocs, schema, name);
                }
            }

            IsLoaded = true;
        }

        private static void AddItem(HashSet<string> hs, string schema, string name)
        {
            if (!hs.Contains(name))
                hs.Add(name);

            if (!String.IsNullOrWhiteSpace(schema))
            {
                var fullname = String.Format("{0}.{1}", schema, name);
                if (!hs.Contains(fullname))
                    hs.Add(fullname);
            }
        }


        public bool IsTable(string name)
        {
            return tables.Contains(name);
        }

        public bool IsView(string name)
        {
            return views.Contains(name);
        }

        public bool IsProcedure(string name)
        {
            return IsProcedure(name, false);   
        }

        public bool IsProcedure(string name, bool isUserContext)
        {
            if (isUserContext)
                return usercontextprocs.Contains(name);
            else
                return procedures.Contains(name);
        }

        #endregion

        #region Properties

        public bool IsLoaded { get; private set; }

        private bool? monitorenabled;
        public bool IsMonitoringEnabled
        {
            get
            {
                if (monitorenabled.HasValue)
                    return monitorenabled.Value;

                //Must check if the dbTableMonitor exists directly so that we can load table names from the local cache.
                monitorenabled = ((int)connection.ExecuteSQLScalar("select count(table_name) from information_schema.tables where table_name = 'dbTableMonitor'", null) > 0);
                return monitorenabled.Value;
            }
        }

        #endregion

    }
}
