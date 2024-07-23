using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace FWBS.OMS
{
    public abstract class CommonObject2 : CommonObject
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public sealed override string FieldPrimaryKey
        {
            get { return FieldPrimaryKeys[0]; }
        }

        protected override void Fetch(object id)
        {
            Fetch(id, null);
        }


        protected override void Fetch(object id, DataRow merge)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (FieldPrimaryKeys.Length > 1)
                throw new CommonObjectException("The Number of Primary Keys do not match the ID's passed");

            this.Fetch(new object[1] { id }, merge);
        }


        public override bool Exists(object id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (FieldPrimaryKeys.Length > 1)
                throw new CommonObjectException("The Number of Primary Keys do not match the ID's passed");
            return this.Exists(new object[1] { id });
        }

        protected abstract string[] FieldPrimaryKeys
        {
            get;
        }

        protected virtual void Fetch(params object[] id)
        {
            Fetch(id, null);
        }

        protected virtual void Fetch(object[] id, DataRow merge)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (id.Length != FieldPrimaryKeys.Length)
                throw new CommonObjectException("The Number of Primary Keys do not match the ID's passed");

            StringBuilder sql = new StringBuilder();
            sql.Append(SelectStatement);
            sql.Append(" where ");
            int i = 0;
            IDataParameter[] paramlist = new IDataParameter[FieldPrimaryKeys.Length];
            foreach (string keys in FieldPrimaryKeys)
            {
                if (i > 0) sql.Append(" and ");
                sql.Append(keys);
                sql.Append(" = @ID");
                sql.Append(i);
                paramlist[i] = Session.CurrentSession.Connection.AddParameter("ID" + i.ToString(), id[i]);
                i++;
            }

            DataTable data = Session.CurrentSession.Connection.ExecuteSQLTable(sql.ToString(), PrimaryTableName, paramlist);
            
            data.DefaultView.RowStateFilter = DataViewRowState.OriginalRows;
            data.DefaultView.RowStateFilter = DataViewRowState.CurrentRows;
            if ((data == null) || (data.Rows.Count == 0))
            {
                throw GetMissingException(id);
            }

            if (merge != null)
                Global.Merge(data.Rows[0], merge);

            _data = data;
           
            SetPrimaryKeys();

            this.OnExtLoaded();
        }

        public virtual bool Exists(params object[] id)
        {
            if (id == null)
                throw new ArgumentNullException("id"); 
            
            if (id.Length != FieldPrimaryKeys.Length)
                throw new CommonObjectException("The Number of Primary Keys do not match the ID's passed");

            try
            {
                if (String.IsNullOrEmpty(this.SelectExistsStatement))
                {
                    Fetch(id);
                    return true;
                }
                else
                {
                    Session.CurrentSession.CheckLoggedIn();
                    StringBuilder sql = new StringBuilder();
                    sql.Append(SelectStatement);
                    sql.Append(" where ");
                    int i = 0;
                    IDataParameter[] paramlist = new IDataParameter[FieldPrimaryKeys.Length];
                    foreach (string keys in FieldPrimaryKeys)
                    {
                        if (i > 0) sql.Append(" and ");
                        sql.Append(keys);
                        sql.Append(" = @ID");
                        sql.Append(i);
                        paramlist[i] = Session.CurrentSession.Connection.AddParameter("ID" + i.ToString(), id[i]);
                        i++;
                    }
                    object dt = Session.CurrentSession.Connection.ExecuteSQLScalar(sql.ToString(), paramlist);
                    if (dt != null)
                        return true;
                    else
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public override void Refresh(bool applyChanges)
        {
            if (IsNew)
                return;

            if (this.OnExtRefreshing())
                return;

            DataTable changes = _data.GetChanges();

            List<object> vals = new List<object>();
            foreach (string k in this.FieldPrimaryKeys)
                vals.Add(GetExtraInfo(k));

            if (changes != null && applyChanges && changes.Rows.Count > 0)
                Fetch(vals.ToArray(), changes.Rows[0]);
            else
                Fetch(vals.ToArray(), null);

            this.OnExtRefreshed();
        }

        protected override void SetPrimaryKeys()
        {
            if (_data != null)
            {
                List<DataColumn> primarykeys = new List<DataColumn>();
                foreach (string keys in FieldPrimaryKeys)
                {
                    primarykeys.Add(_data.Columns[keys]);
                }
                _data.PrimaryKey = primarykeys.ToArray();
            }
        }

        protected override void HasUniqueIDChanged(string fieldName)
        {
            foreach (string keys in FieldPrimaryKeys)
            {
                if (keys == fieldName)
                {
                    OnUniqueIDChanged();
                    return;
                }
            }
        }
    }


}
