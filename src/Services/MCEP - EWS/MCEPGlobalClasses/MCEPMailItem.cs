using System;
using System.Collections.Generic;
using System.Data;

namespace MCEPGlobalClasses
{
    class MCEPMailItem
    {
        #region FieldNames
        private const string IDFieldName = "ID";
        private const string UserEmailFieldName = "UserEmail";
        private const string FolderIDFieldName = "FolderID";
        private const string MessageIDFieldName = "MessageID";
        private const string UserIDFieldName = "UserID";
        private const string FileIDFieldName = "FileID";
        private const string AssociateIDFieldName = "AssocID";
        private const string DocIDFieldName = "DocID";
        private const string CreatedFieldName = "Created";
        private const string UpdatedFieldName = "Updated";
        private const string ProcessedFieldName = "Processed";
        private const string ResultFieldName = "Result";
        private const string ErrorFieldName = "ErrorMessage";
        #endregion

        public DataRow MailItemDataRow;
        public List<String> updatedColumnList = new List<string>();

        public bool MailItemRowUpdated { get; private set; }

        public MCEPMailItem(DataRow row)
        {
            MailItemDataRow = row;
            MailItemRowUpdated = false;
        }

        public Int32 ItemID
        {
            get { return GetValue<Int32>(IDFieldName); }
        }

        public string userEmail
        {
            get { return GetValue<string>(UserEmailFieldName); }
            set { SetValue<string>(UserEmailFieldName, value); }
        }

        public string FolderID
        {
            get { return GetValue<string>(FolderIDFieldName); }
            set { SetValue<string>(FolderIDFieldName, value); }
        }

        public string MessageID
        {
            get { return GetValue<string>(MessageIDFieldName); }
            set { SetValue<string>(MessageIDFieldName, value); }
        }

        public Int64? UserID
        {
            get { return GetValue<Int64?>(UserIDFieldName); }
            set { SetValue<Int64?>(UserIDFieldName, value); }
        }

        public Int64? FileID
        {
            get { return GetValue<Int64?>(FileIDFieldName); }
            set { SetValue<Int64?>(FileIDFieldName, value); }
        }

        public Int64? AssocID
        {
            get { return GetValue<Int64?>(AssociateIDFieldName); }
            set { SetValue<Int64?>(AssociateIDFieldName, value); }
        }

        public Int64? DocID
        {
            get { return GetValue<Int64?>(DocIDFieldName); }
 
            set { SetValue<Int64?>(DocIDFieldName, value); }
        }

        public DateTime? ItemCreated
        {
            get { return GetValue<DateTime?>(CreatedFieldName); }
            set { SetValue<DateTime?>(CreatedFieldName, value); }
        }

        public DateTime? ItemUpdated
        {
            get { return GetValue<DateTime?>(UpdatedFieldName); }
            set { SetValue<DateTime?>(UpdatedFieldName, value); }
        }

        public bool Processed
        {
            get { return GetValue<bool>(ProcessedFieldName); }
            set { SetValue<bool>(ProcessedFieldName, value); }
        }

        public string Result
        {
            get { return GetValue<string>(ResultFieldName); }
            set { SetValue<string>(ResultFieldName, value); }
        }

        public string ErrorMessage
        {
            get { return GetValue<string>(ErrorFieldName); }
            set { SetValue<string>(ErrorFieldName, value); }
        }

        private T GetValue<T>(string columnName)
        {
            return MailItemDataRow.Field<T>(columnName);
        }

        private void SetValue<T>(string columnName, T value)
        {
            T colValue = GetValue<T>(columnName);
            if (colValue != null && colValue.Equals(value))
            {
                return;
            }
            MailItemDataRow.SetField(columnName, value);
            if (updatedColumnList.Contains(columnName) == false)
            {
                updatedColumnList.Add(columnName);
            }
            MailItemRowUpdated = true;
        }
    }
}
