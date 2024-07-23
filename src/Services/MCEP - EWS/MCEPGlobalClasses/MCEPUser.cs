using System;
using System.Collections.Generic;
using System.Data;
namespace MCEPGlobalClasses
{
    public class MCEPUser
    {
        #region FieldNames
        private const string IDFieldName = "UserID";
        private const string EmailFieldName = "Email";
        private const string RootFolderNameFieldName = "RootFolderName";
        private const string RootFolderIDFieldName = "RootFolderID";
        private const string ActiveFieldName = "Active";
        private const string LastRanFieldName = "LastRan";
        private const string CreatedFieldName = "Created";
        private const string UpdatedFieldName = "Updated";
        #endregion

        public DataRow UserDataRow;
        public List<String> updatedColumnList = new List<string>();

        public bool UserRowUpdated { get; private set; }

        public MCEPUser(DataRow row)
        {
            UserDataRow = row;
            UserRowUpdated = false;
        }

        public Int32 UserID
        {
            get { return GetValue<Int32>(IDFieldName); }
            set { SetValue<Int32>(IDFieldName, value); }
        }

        public string EmailAddress
        {
            get { return GetValue<string>(EmailFieldName); }
            set { SetValue<string>(EmailFieldName, value); }
        }

        public string RootFolderName
        {
            get { return GetValue<string>(RootFolderNameFieldName); }
            set { SetValue<string>(RootFolderNameFieldName, value); }
        }

        public string RootFolderID
        {
            get { return GetValue<string>(RootFolderIDFieldName); }
            set { SetValue<string>(RootFolderIDFieldName, value); }
        }

        public bool UserActive
        {
            get { return GetValue<bool>(ActiveFieldName); }
            set { SetValue<bool>(ActiveFieldName, value); }
        }

        public DateTime? UserLastRan
        {
            get { return GetValue<DateTime?>(LastRanFieldName); }
            set { SetValue<DateTime?>(LastRanFieldName, value); }
        }

        public DateTime? UserCreated
        {
            get { return GetValue<DateTime?>(CreatedFieldName); }
            set { SetValue<DateTime?>(CreatedFieldName, value); }
        }

        public DateTime? UserUpdated
        {
            get { return GetValue<DateTime?>(UpdatedFieldName); }
            set { SetValue<DateTime?>(UpdatedFieldName, value); }
        }

        private T GetValue<T>(string columnName)
        {
            return UserDataRow.Field<T>(columnName);
        }

        private void SetValue<T>(string columnName, T value)
        {
            T colValue = GetValue<T>(columnName);
            if (colValue != null && colValue.Equals(value))
            {
                return;
            }
            UserDataRow.SetField(columnName, value);
            if (updatedColumnList.Contains(columnName) == false)
            {
                updatedColumnList.Add(columnName);
            }
            UserRowUpdated = true;
        }

    }
}
