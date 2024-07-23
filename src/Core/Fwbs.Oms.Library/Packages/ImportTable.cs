using System;
using System.Data;

namespace FWBS.OMS.Design.Import
{
    public class ImportTable
    {
        #region Fields
        private FWBS.OMS.Design.Export.FieldReplacer _fieldreplace = null;
        #endregion

        #region Deligates
        public event SetValueOverideHandler OverrideColumnValue;
        #endregion

        #region Constructors
        public ImportTable(FWBS.OMS.Design.Export.FieldReplacer fieldreplacer)
        {
            _fieldreplace = fieldreplacer;
        }
        #endregion

        #region Public Methods
        public void NewOnlyImport(DataTable FromTable, DataTable ToTable, string PrimaryKey, bool RequireQuotes)
        {
            foreach (DataColumn col in ToTable.Columns)
                if (!col.AllowDBNull) col.AllowDBNull = true;

            foreach (DataRow dr in FromTable.Rows)
            {
                string newfilter = "";
                string[] keys = PrimaryKey.Split(",".ToCharArray());
                foreach (string key in keys)
                {
                    if (newfilter != "") newfilter += " AND ";
                    if (RequireQuotes)
                        newfilter = newfilter + key + " = '" + Convert.ToString(dr[key]).Replace("'", "''") + "'";
                    else
                        newfilter = newfilter + key + " = " + Convert.ToString(dr[key]).Replace("'", "''");
                }
                ToTable.DefaultView.RowFilter = newfilter;

                if (ToTable.DefaultView.Count == 0)
                {
                    DataRow newdr = ToTable.NewRow();
                    object objvalue = null;
                    foreach (DataColumn cm in FromTable.Columns)
                    {
                        objvalue = null;
                        if (ToTable.Columns.Contains(cm.ColumnName))
                        {
                            if (OverrideColumnValue != null)
                                OverrideColumnValue(cm.ColumnName, dr[cm.ColumnName], out objvalue);
                            if (_fieldreplace != null && _fieldreplace.Goto(FromTable.TableName, cm.ColumnName))
                            {
                                if (_fieldreplace.ChangeType == 1)
                                    newdr[cm.ColumnName] = DBNull.Value;
                                else if (_fieldreplace.ChangeType == 2)
                                    newdr[cm.ColumnName] = _fieldreplace.ChangeValue;
                                else if (objvalue != null)
                                    newdr[cm.ColumnName] = objvalue;
                                else
                                    newdr[cm.ColumnName] = dr[cm.ColumnName];
                            }
                            else if (objvalue != null)
                                newdr[cm.ColumnName] = objvalue;
                            else
                                newdr[cm.ColumnName] = dr[cm.ColumnName];

                        }
                    }
                    ToTable.Rows.Add(newdr);
                }
            }
        }

        public void ImportOver(DataTable FromTable, DataTable ToTable, string PrimaryKey, bool RequireQuotes)
        {
            foreach (DataColumn col in ToTable.Columns)
                if (!col.AllowDBNull) col.AllowDBNull = true;

            foreach (DataRow dr in FromTable.Rows)
            {
                string newfilter = "";
                string[] keys = PrimaryKey.Split(",".ToCharArray());
                foreach (string key in keys)
                {
                    if (newfilter != "") newfilter += " AND ";
                    if (RequireQuotes)
                        newfilter = newfilter + key + " = '" + Convert.ToString(dr[key]).Replace("'", "''") + "'";
                    else
                        newfilter = newfilter + key + " = " + Convert.ToString(dr[key]).Replace("'", "''");
                }
                ToTable.DefaultView.RowFilter = newfilter;

                if (ToTable.DefaultView.Count == 0)
                {
                    DataRow newdr = ToTable.NewRow();
                    object objvalue = null;
                    foreach (DataColumn cm in FromTable.Columns)
                    {
                        objvalue = null;
                        if (ToTable.Columns.Contains(cm.ColumnName))
                        {
                            if (OverrideColumnValue != null)
                                OverrideColumnValue(cm.ColumnName, dr[cm.ColumnName], out objvalue);
                            if (_fieldreplace != null && _fieldreplace.Goto(FromTable.TableName, cm.ColumnName))
                            {
                                if (_fieldreplace.ChangeType == 1)
                                    newdr[cm.ColumnName] = DBNull.Value;
                                else if (_fieldreplace.ChangeType == 2)
                                    newdr[cm.ColumnName] = _fieldreplace.ChangeValue;
                                else if (objvalue != null)
                                    newdr[cm.ColumnName] = objvalue;
                                else
                                    newdr[cm.ColumnName] = dr[cm.ColumnName];
                            }
                            else if (objvalue != null)
                                newdr[cm.ColumnName] = objvalue;
                            else
                                newdr[cm.ColumnName] = dr[cm.ColumnName];

                        }
                    }
                    ToTable.Rows.Add(newdr);
                }
                else if (ToTable.DefaultView.Count == 1)
                {
                    DataRow newdr = ToTable.DefaultView[0].Row;
                    object objvalue = null;
                    foreach (DataColumn cm in FromTable.Columns)
                    {
                        objvalue = null;
                        if (cm.ColumnName != "rowguid")
                        {
                            if (ToTable.Columns.Contains(cm.ColumnName))
                            {
                                if (OverrideColumnValue != null)
                                    OverrideColumnValue(cm.ColumnName, dr[cm.ColumnName], out objvalue);
                                if (_fieldreplace != null && _fieldreplace.Goto(FromTable.TableName, cm.ColumnName))
                                {
                                    if (_fieldreplace.ChangeType == 1)
                                        newdr[cm.ColumnName] = DBNull.Value;
                                    else if (_fieldreplace.ChangeType == 2)
                                        newdr[cm.ColumnName] = _fieldreplace.ChangeValue;
                                    else if (objvalue != null)
                                        newdr[cm.ColumnName] = objvalue;
                                    else
                                        newdr[cm.ColumnName] = dr[cm.ColumnName];
                                }
                                else if (objvalue != null)
                                    newdr[cm.ColumnName] = objvalue;
                                else
                                    newdr[cm.ColumnName] = dr[cm.ColumnName];

                            }
                        }
                    }
                }
            }
        }


        public void ImportRowOver(DataTable FromTable, DataTable ToTable, int RowIndex)
        {
            foreach (DataColumn col in ToTable.Columns)
                if (!col.AllowDBNull) col.AllowDBNull = true;

            DataRow newdr = ToTable.Rows[0];
            DataRow dr = FromTable.Rows[0];

            object objvalue = null;
            foreach (DataColumn cm in FromTable.Columns)
            {
                objvalue = null;
                if (cm.ColumnName != "rowguid")
                {
                    if (ToTable.Columns.Contains(cm.ColumnName))
                    {
                        if (cm.ColumnName.ToLower() == "created" && dr[cm.ColumnName] == DBNull.Value)
                            newdr[cm.ColumnName] = DateTime.Now;
                        else if (cm.ColumnName.ToLower() == "updated" && dr[cm.ColumnName] == DBNull.Value)
                            newdr[cm.ColumnName] = DateTime.Now;
                        else if (cm.ColumnName.ToLower() == "createdby")
                            newdr[cm.ColumnName] = FWBS.OMS.SystemUsers.Admin;
                        else if (cm.ColumnName.ToLower() == "updatedby")
                            newdr[cm.ColumnName] = FWBS.OMS.SystemUsers.Admin;
                        else
                        {
                            if (OverrideColumnValue != null)
                                OverrideColumnValue(cm.ColumnName, dr[cm.ColumnName], out objvalue);
                            if (_fieldreplace != null && _fieldreplace.Goto(FromTable.TableName, cm.ColumnName))
                            {
                                if (_fieldreplace.ChangeType == 1)
                                    newdr[cm.ColumnName] = DBNull.Value;
                                else if (_fieldreplace.ChangeType == 2)
                                    newdr[cm.ColumnName] = _fieldreplace.ChangeValue;
                                else if (objvalue != null)
                                {
                                    if (Convert.ToString(objvalue) != "<default>")
                                        newdr[cm.ColumnName] = objvalue;
                                }
                                else
                                    newdr[cm.ColumnName] = dr[cm.ColumnName];
                            }
                            else if (objvalue != null)
                            {
                                if (Convert.ToString(objvalue) != "<default>")
                                    newdr[cm.ColumnName] = objvalue;
                            }
                            else
                                newdr[cm.ColumnName] = dr[cm.ColumnName];
                        }
                    }
                }
            }
        }

        public void Import(DataTable FromTable, DataTable ToTable)
        {
            try
            {
                foreach (DataColumn col in ToTable.Columns)
                    if (!col.AllowDBNull) col.AllowDBNull = true;

                foreach (DataRow dr in FromTable.Rows)
                {
                    DataRow newdr = ToTable.NewRow();
                    object objvalue = null;
                    foreach (DataColumn cm in FromTable.Columns)
                    {
                        objvalue = null;
                        if (ToTable.Columns.Contains(cm.ColumnName))
                        {
                            if (cm.ColumnName.ToLower() == "created" && dr[cm.ColumnName] == DBNull.Value)
                                newdr[cm.ColumnName] = DateTime.Now;
                            else if (cm.ColumnName.ToLower() == "updated" && dr[cm.ColumnName] == DBNull.Value)
                                newdr[cm.ColumnName] = DateTime.Now;
                            else if (cm.ColumnName.ToLower() == "createdby")
                                newdr[cm.ColumnName] = FWBS.OMS.SystemUsers.Admin;
                            else if (cm.ColumnName.ToLower() == "updatedby")
                                newdr[cm.ColumnName] = FWBS.OMS.SystemUsers.Admin;
                            else
                            {
                                if (OverrideColumnValue != null)
                                    OverrideColumnValue(cm.ColumnName, dr[cm.ColumnName], out objvalue);
                                if (_fieldreplace != null && _fieldreplace.Goto(FromTable.TableName, cm.ColumnName))
                                {
                                    if (_fieldreplace.ChangeType == 1)
                                        newdr[cm.ColumnName] = DBNull.Value;
                                    else if (_fieldreplace.ChangeType == 2)
                                        newdr[cm.ColumnName] = _fieldreplace.ChangeValue;
                                    else if (objvalue != null)
                                    {
                                        if (Convert.ToString(objvalue) != "<default>")
                                            newdr[cm.ColumnName] = objvalue;
                                    }
                                    else
                                        newdr[cm.ColumnName] = dr[cm.ColumnName];
                                }
                                else if (objvalue != null)
                                {
                                    if (Convert.ToString(objvalue) != "<default>")
                                        newdr[cm.ColumnName] = objvalue;
                                }
                                else
                                    newdr[cm.ColumnName] = dr[cm.ColumnName];
                            }
                        }
                    }
                    ToTable.Rows.Add(newdr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ImportView(DataView FromView, DataTable ToTable)
        {
            foreach (DataColumn col in ToTable.Columns)
                if (!col.AllowDBNull) col.AllowDBNull = true;

            foreach (DataRowView dr in FromView)
            {
                DataRow newdr = ToTable.NewRow();
                object objvalue = null;
                foreach (DataColumn cm in FromView.Table.Columns)
                {
                    objvalue = null;
                    if (ToTable.Columns.Contains(cm.ColumnName))
                    {
                        if (OverrideColumnValue != null)
                            OverrideColumnValue(cm.ColumnName, dr[cm.ColumnName], out objvalue);
                        if (_fieldreplace != null && _fieldreplace.Goto(FromView.Table.TableName, cm.ColumnName))
                        {
                            if (_fieldreplace.ChangeType == 1)
                                newdr[cm.ColumnName] = DBNull.Value;
                            else if (_fieldreplace.ChangeType == 2)
                                newdr[cm.ColumnName] = _fieldreplace.ChangeValue;
                            else if (objvalue != null)
                            {
                                if (Convert.ToString(objvalue) != "<default>")
                                    newdr[cm.ColumnName] = objvalue;
                            }
                            else
                                newdr[cm.ColumnName] = dr[cm.ColumnName];
                        }
                        else if (objvalue != null)
                        {
                            if (Convert.ToString(objvalue) != "<default>")
                                newdr[cm.ColumnName] = objvalue;
                        }
                        else
                            newdr[cm.ColumnName] = dr[cm.ColumnName];

                    }
                }
                ToTable.Rows.Add(newdr);
            }
        }
        #endregion
    }
}
