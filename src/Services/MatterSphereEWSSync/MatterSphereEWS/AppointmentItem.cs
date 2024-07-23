using System;
using System.Collections.Generic;
using System.Data;

namespace MatterSphereEWS
{
    class AppointmentItem
    {
        #region FieldNames
        private const string ItemIDFieldName = "appID";
        private const string UserEmailFieldName = "appMailbox";
        private const string ItemSubjectFieldName = "appDesc";
        private const string ItemBodyFieldName = "appNotes";
        private const string ItemLocationFieldName = "appLocation";
        private const string ItemAllDayAppFieldName = "appAllDay";
        private const string ItemStartDateFieldName = "appDate";
        private const string ItemEndDateFieldName = "appEndDate";
        private const string ItemReminderFieldName = "appReminderSet";
        private const string ItemReminderMinutesFieldName = "appReminder";
        private const string ItemActiveFieldName = "AppActive";
        private const string ItemMAPIEntryIDFieldName = "appMAPIEntryID";
        private const string ItemEWSIDFieldName = "appMAPIStoreID";
        private const string ItemExternalFieldName = "appExternal";
        private const string ItemFeeUsrIDFieldName = "feeUsrID";
        private const string ItemUpdatedFieldName = "Updated";
        private const string ItemCreatedFieldName = "Created";
        private const string ItemFileIDFieldName = "fileID";
        private const string ItemClientIDFieldName = "clID";
        private const string ItemAssocIDFieldName = "AssocID";
        private const string ItemTimeZoneFieldName = "AppTimeZone";
        private const string ItemCategory = "Category";

        #endregion

        public DataRow MSEWSItemDataRow;
        public List<String> updatedColumnList = new List<string>();

        public bool MSEWSItemRowUpdated { get; private set; }

        public AppointmentItem(DataRow row)
        {
            MSEWSItemDataRow = row;
            MSEWSItemRowUpdated = false;
        }

        public Int64 ItemID
        {
            get { return GetValue<Int64>(ItemIDFieldName); }
        }

        public string userEmail
        {
            get { return GetValue<string>(UserEmailFieldName); }
            set { SetValue<string>(UserEmailFieldName, value); }
        }

        public string iSubject
        {
            get { return GetValue<string>(ItemSubjectFieldName); }
            set { SetValue<string>(ItemSubjectFieldName, value); }
        }
        public string iBody
        {
            get { return GetValue<string>(ItemBodyFieldName); }
            set { SetValue<string>(ItemBodyFieldName, value); }
        }
        public string iLocation
        {
            get { return GetValue<string>(ItemLocationFieldName); }
            set { SetValue<string>(ItemLocationFieldName, value); }
        }
        public bool iAllDayApp
        {
            get { return GetValue<bool>(ItemAllDayAppFieldName); }
            set { SetValue<bool>(ItemAllDayAppFieldName, value); }
        }
        public DateTime? iStartDate
        {
            get { return GetValue<DateTime?>(ItemStartDateFieldName); }
            set { SetValue<DateTime?>(ItemStartDateFieldName, value); }
        }
        public DateTime? iEndDate
        {
            get { return GetValue<DateTime?>(ItemEndDateFieldName); }
            set { SetValue<DateTime?>(ItemEndDateFieldName, value); }
        }
        public bool iReminderActive
        {
            get { return GetValue<bool>(ItemReminderFieldName); }
            set { SetValue<bool>(ItemReminderFieldName, value); }
        }
        public Int32? iReminderMinutes
        {
            get { return GetValue<Int32?>(ItemReminderMinutesFieldName); }
            set { SetValue<Int32?>(ItemReminderMinutesFieldName, value); }
        }
        public bool iActive
        {
            get { return GetValue<bool>(ItemActiveFieldName); }
            set { SetValue<bool>(ItemActiveFieldName, value); }
        }
        public string iEntryID
        {
            get { return GetValue<string>(ItemMAPIEntryIDFieldName); }
            set { SetValue<string>(ItemMAPIEntryIDFieldName, value); }
        }
        public string iEWSID
        {
            get { return GetValue<string>(ItemEWSIDFieldName); }
            set { SetValue<string>(ItemEWSIDFieldName, value); }
        }
        public bool iExternal
        {
            get { return GetValue<bool>(ItemExternalFieldName); }
            set { SetValue<bool>(ItemExternalFieldName, value); }
        }
        public Int32 iFeeEarnerID
        {
            get { return GetValue<Int32>(ItemFeeUsrIDFieldName); }
            set { SetValue<Int32>(ItemFeeUsrIDFieldName, value); }
        }
        public DateTime iCreated
        {
            get { return GetValue<DateTime>(ItemCreatedFieldName); }
            set { SetValue<DateTime>(ItemCreatedFieldName, value); }
        }
        public DateTime iUpdated
        {
            get { return GetValue<DateTime>(ItemUpdatedFieldName); }
            set { SetValue<DateTime>(ItemUpdatedFieldName, value); }
        }
        public Int64 iFileID
        {
            get { return GetValue<Int64>(ItemFileIDFieldName); }
            set { SetValue<Int64>(ItemFileIDFieldName, value); }
        }
        public Int64 iClientID
        {
            get { return GetValue<Int64>(ItemClientIDFieldName); }
            set { SetValue<Int64>(ItemClientIDFieldName, value); }
        }
        public Int64? iAssocID
        {
            get { return GetValue<Int64?>(ItemAssocIDFieldName); }
            set { SetValue<Int64?>(ItemAssocIDFieldName, value); }
        }
        public string iTimeZone
        {
            get { return GetValue<string>(ItemTimeZoneFieldName); }
            set { SetValue<string>(ItemTimeZoneFieldName, value); }
        }
        public string iCategory
        {
            get { return GetValue<string>(ItemCategory); }
            set { SetValue<string>(ItemCategory, value); }
        }
        private T GetValue<T>(string columnName)
        {
            return MSEWSItemDataRow.Field<T>(columnName);
        }

        private void SetValue<T>(string columnName, T value)
        {
            T colValue = GetValue<T>(columnName);
            if (colValue != null && colValue.Equals(value))
            {
                return;
            }
            MSEWSItemDataRow.SetField(columnName, value);
            if (updatedColumnList.Contains(columnName) == false)
            {
                updatedColumnList.Add(columnName);
            }
            MSEWSItemRowUpdated = true;
        }
    }
}
