using System;
using System.Collections.Generic;
using System.Data;


namespace DocumentArchivingClass
{
    class DocumentVersionRecord
    {
        #region FieldNames       
        private const string DocumentIDFieldName = "docID";
        private const string DocumentVernumberFieldName = "verNumber";
        private const string DocumentVerLabelFieldName = "verLabel";
        private const string DocumentVerTokenFieldName = "verToken";       
        #endregion


        public DataRow DocumentVersionRecordDataRow;
        public List<String> updatedColumnList = new List<string>();
        public bool DocumentVersionRecordRowUpdated { get; private set; }


        public DocumentVersionRecord(DataRow row)
        {
            DocumentVersionRecordDataRow = row;
            DocumentVersionRecordRowUpdated = false;          
        }


        public Int64 DocumentID
        {
            get { return GetValue<Int64>(DocumentIDFieldName); }
        }


        public string DocumentVernumber 
        {
            get { return GetValue<string>(DocumentVernumberFieldName); }
        }



        public string DocumentVerLabel
        {
            get { return GetValue<string>(DocumentVerLabelFieldName); }
        }


        public string DocumentVerToken
        {
            get { return GetValue<string>(DocumentVerTokenFieldName); }
        }

        public bool VersionCopiedSucessfully { get; set; }

        public bool VersionDeletedSucessfully { get; set; }


        private T GetValue<T>(string columnName)
        {
            return DocumentVersionRecordDataRow.Field<T>(columnName);
        }

        private void SetValue<T>(string columnName, T value)
        {
            T colValue = GetValue<T>(columnName);
            if (colValue != null && colValue.Equals(value))
            {
                return;
            }
            DocumentVersionRecordDataRow.SetField(columnName, value);
            if (updatedColumnList.Contains(columnName) == false)
            {
                updatedColumnList.Add(columnName);
            }
            DocumentVersionRecordRowUpdated = true;
        }

    }
}
