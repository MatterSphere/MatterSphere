using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    internal abstract class ImportBase
    {
        public event ProgressEventHandler Progress = null;
        private FWBS.OMS.Design.Export.FieldReplacer _fieldreplacer = null;

        protected bool OnProgress(string label)
        {
            if (Progress != null)
            {
                ProgressEventArgs e = new ProgressEventArgs(label);
                Progress(this, e);
                return e.Cancel;
            }
            else
                return false;
        }

        protected DataSet _source;

        public DataSet Source
        {
            get
            {
                return _source;
            }
        }

        public DataRowView CurrentRow
        {
            get;
            set;
        }

        public FWBS.OMS.Design.Export.FieldReplacer Fieldreplacer
        {
            get
            {
                return _fieldreplacer;
            }
            set
            {
                _fieldreplacer = value;
            }
        }

        public abstract bool Execute();

        public virtual bool Execute(bool Copy)
        {
            return Execute();
        }


        protected string[] GetColumns(DataTable dt, bool RemoveRowGuid)
        {
            System.Collections.Generic.List<string> columns = new System.Collections.Generic.List<string>();
            foreach (System.Data.DataColumn c in dt.Columns)
            if ((RemoveRowGuid && c.ColumnName != "rowguid") || RemoveRowGuid == false)
                columns.Add(c.ColumnName);
            return columns.ToArray();
        }

        protected string GetColumnsString(DataTable dt, bool RemoveRowGuid)
        {
            System.Collections.Generic.List<string> columns = new System.Collections.Generic.List<string>();
            foreach (System.Data.DataColumn c in dt.Columns)
                if ((RemoveRowGuid && c.ColumnName != "rowguid") || RemoveRowGuid == false)
                    columns.Add(c.ColumnName);
            return String.Join(",", columns.ToArray());
        }

    }
}
