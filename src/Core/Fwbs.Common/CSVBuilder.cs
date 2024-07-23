using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace FWBS.Common
{
    /// <summary>
    /// Create a CSV file
    /// </summary>
    public class CSVBuilder
    {
        private const char surroundChar = '"';
        private const char separator = ',';
        /// <summary>
        /// Create a CSV file from a datatable
        /// </summary>
        /// <param name="dt">Table to be converted to a CSV</param>
        /// <param name="fileName">file to be created</param>
        /// <param name="excludeHeaders">exclude the header row</param>
        public static void FromDataTable(DataTable dt, string fileName, bool excludeHeaders)
        {
            List<string> columns = new List<string>();

            if (!excludeHeaders)
            {
                foreach (DataColumn col in dt.Columns)
                {
                    columns.Add(col.ColumnName);
                }
            }

            FromDataTable(dt, fileName, excludeHeaders, columns);
        }

        /// <summary>
        /// Create a CSV file from a datatable
        /// </summary>
        /// <param name="dt">Table to be converted to a CSV</param>
        /// <param name="fileName">file to be created</param>
        /// <param name="excludeHeaders">exclude the header row</param>
        /// <param name="columns">List of columns to be included in the CSV</param>
        public static void FromDataTable(DataTable dt, string fileName, bool excludeHeaders, List<string> columns)
        {

            List<string> remove = new List<string>();

            foreach (string colName in columns)
            {
                if (!dt.Columns.Contains(colName))
                    remove.Add(colName);
            }

            foreach (string colName in remove)
                columns.Remove(colName);

            Validate(dt);
            FileInfo info = new FileInfo(fileName);
            using (StreamWriter stream = info.CreateText())
            {
                if (!excludeHeaders)
                    PopulateHeader(columns, stream);

                PopulateData(dt, columns, stream);
            }
        }

       

        /// <summary>
        /// Create a CSV file from a datatable
        /// </summary>
        /// <param name="dt">Table to be converted to a CSV</param>
        /// <param name="fileName">file to be created</param>
        /// <param name="columns">column to be incuded in the CSV, and the name to be aliased as</param>
        public static void FromDataTable(DataTable dt, string fileName, Dictionary<string, string> columns)
        {

            List<string> headers = new List<string>();
            List<string> cols = new List<string>();

            foreach (KeyValuePair<string, string> columData in columns)
            {
                string colName = columData.Key;

                if (!dt.Columns.Contains(colName))
                    continue;

                headers.Add(columData.Value);
                cols.Add(colName);
            }
            Validate(dt);

            int nocolumns = cols.Count;
            FileInfo info = new FileInfo(fileName);
            using (StreamWriter stream = info.CreateText())
            {

                PopulateHeader(headers, stream);

                PopulateData(dt, cols, stream);
                
            }
        }

        private static void PopulateData(DataTable dt, List<string> columns, StreamWriter stream)
        {
            int nocolumns = columns.Count;
            foreach (DataRow row in dt.Rows)
            {
                int counter = 0;
                foreach (string column in columns)
                {
                    counter++;
                    stream.Write(surroundChar);
                    stream.Write(row[column]);
                    stream.Write(surroundChar);

                    if (counter < nocolumns)
                        stream.Write(separator);
                }
                stream.WriteLine();
            }
        }

        private static void PopulateHeader(List<string> columns, StreamWriter stream)
        {
            int nocolumns = columns.Count;
            int counter = 0;
            foreach (string col in columns)
            {
                counter++;
                stream.Write(surroundChar);
                stream.Write(col);
                stream.Write(surroundChar);

                if (counter < nocolumns)
                    stream.Write(separator);
            }
            stream.WriteLine();
        }


        

        private static void Validate(DataTable dt)
        {
            if (dt == null)
                throw new NullReferenceException("Data Table is not Initialised");

        }

       

    }
}
