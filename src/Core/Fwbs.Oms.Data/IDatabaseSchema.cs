using System.Data;

namespace FWBS.OMS.Data
{
    public interface IDatabaseSchema
    {
        /// <summary>
        /// Returns a list of tables within the database.
        /// </summary>
        /// <returns>A data table of tables.</returns>
        DataTable GetTables();
        /// <summary>
        /// Returns a list of views within the database.
        /// </summary>
        /// <returns>A data table of views.</returns>
        DataTable GetViews();
        /// <summary>
        /// Returns a list of columns within a specified table / view.
        /// </summary>
        /// <param name="objectName">Table / view name.</param>
        /// <returns>A data table of columns.</returns>
        DataTable GetColumns(string objectName);
        /// <summary>
        /// Returns a list of stored procedures within the database.
        /// </summary>
        /// <returns>A data table of stored procedures.</returns>
        DataTable GetProcedures();
        /// <summary>
        /// Returns a list of columns within a specified table / view.
        /// </summary>
        /// <param name="procedureName">Procedure name.</param>
        /// <returns>A data table of parameters.</returns>
        DataTable GetParameters(string procedureName);
        /// <summary>
        /// Fetches the primary key field name of a particular table.
        /// </summary>
        /// <param name="tableName">Table name within the database.</param>
        /// <returns>Data table of primary key information.</returns>
        DataTable GetPrimaryKey(string tableName);
        /// <summary>
        /// Fetches the list of columns that the stored procedure is going to return back.
        /// </summary>
        /// <param name="procedureName">Stored procedure name.</param>
        /// <returns>A data table of columns.</returns>
        DataTable GetProcedureColumns(string procedureName);

    }

}
