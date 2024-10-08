using System;
using System.Data;

namespace FWBS.OMS.Data
{
    internal sealed class DataReaderWrapper : IDataReader
    {
        private readonly IDataReader _reader;
        private readonly ExecuteParameters _pars;

        public DataReaderWrapper(IDataReader reader, ExecuteParameters pars)
        {
            this._reader = reader;
            this._pars = pars;
        }

        #region IDataReader Members

        public void Close()
        {
            _reader.Close();
        }

        public int Depth
        {
            get { return _reader.Depth; }
        }

        public DataTable GetSchemaTable()
        {
            return _reader.GetSchemaTable();
        }

        public bool IsClosed
        {
            get { return _reader.IsClosed; }
        }

        public bool NextResult()
        {
            return _reader.NextResult();
        }

        public bool Read()
        {
            return _reader.Read();
        }

        public int RecordsAffected
        {
            get { return _reader.RecordsAffected; }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _reader.Dispose();
        }

        #endregion

        #region IDataRecord Members

        public int FieldCount
        {
            get { return _reader.FieldCount; }
        }

        public bool GetBoolean(int i)
        {
            return _reader.GetBoolean(i);
        }

        public byte GetByte(int i)
        {
            return _reader.GetByte(i);
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            return _reader.GetBytes(i, fieldOffset, buffer, bufferoffset, length);
        }

        public char GetChar(int i)
        {
            return _reader.GetChar(i);
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            return _reader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
        }

        public IDataReader GetData(int i)
        {
            return _reader.GetData(i);
        }

        public string GetDataTypeName(int i)
        {
            return _reader.GetDataTypeName(i);
        }

        public DateTime GetDateTime(int i)
        {
            var date = _reader.GetDateTime(i);

            return date;

        }

        public decimal GetDecimal(int i)
        {
            return _reader.GetDecimal(i);
        }

        public double GetDouble(int i)
        {
            return _reader.GetDouble(i);
        }

        public Type GetFieldType(int i)
        {
            return _reader.GetFieldType(i);
        }

        public float GetFloat(int i)
        {
            return _reader.GetFloat(i);
        }

        public Guid GetGuid(int i)
        {
            return _reader.GetGuid(i);
        }

        public short GetInt16(int i)
        {
            return _reader.GetInt16(i);
        }

        public int GetInt32(int i)
        {
            return _reader.GetInt32(i);
        }

        public long GetInt64(int i)
        {
            return _reader.GetInt64(i);
        }

        public string GetName(int i)
        {
            return _reader.GetName(i);
        }

        public int GetOrdinal(string name)
        {
            return _reader.GetOrdinal(name);
        }

        public string GetString(int i)
        {
            return _reader.GetString(i);
        }

        public object GetValue(int i)
        {

            return Parse(GetName(i), _reader.GetValue(i));
        }

        public int GetValues(object[] values)
        {
            if (values == null)
                return 0;

            var ret = _reader.GetValues(values);

            for (int i = 0; i < values.Length && i < _reader.FieldCount; i++)
            {
                values[i] = Parse(GetName(i), values[i]);
            }

            return ret;
        }

        public bool IsDBNull(int i)
        {
            return _reader.IsDBNull(i);
        }

        public object this[string name]
        {
            get
            {
                return Parse(name, _reader[name]);

            }
        }

        public object this[int i]
        {
            get
            {
                return Parse(GetName(i), _reader[i]);
            }
        }

        #endregion

        private object Parse(string name, object val)
        {
 
            var dt = val as DateTime?;

            if (dt.HasValue)
            {
                if (dt.Value.Kind == DateTimeKind.Unspecified)
                {
                    DateTimeItem col;

                    if (_pars.DateTimeColumns.TryGetValue(name, out col))
                    {
                        return DateTime.SpecifyKind(dt.Value, col.Kind);
                    }
                    else
                    {
                        return DateTime.SpecifyKind(dt.Value, _pars.DefaultDateTimeKind);
                    }
                }

                return dt;
            }

            return val;

        }
    }
}
