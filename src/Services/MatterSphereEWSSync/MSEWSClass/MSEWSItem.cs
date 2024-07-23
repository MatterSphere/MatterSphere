using System;
using System.Collections.Generic;
using System.Data;

namespace MSEWSClass
{
    class MSEWSItem
    {
        #region FieldNames
        private const string ItemIDFieldName = "ItemID";
        private const string UserEmailFieldName = "UserEmail";
        private const string ItemTypeFieldName = "ItemType";
        private const string ItemSubjectFieldName = "ItemSubject";
        private const string ItemBodyFieldName = "ItemBody";
        private const string ItemAllDayAppFieldName = "ItemAllDayApp";
        private const string ItemStartDateFieldName = "ItemStartDate";
        private const string ItemEndDateFieldName = "ItemEndDate";
        private const string ItemDueDateFieldName = "ItemDueDate";
        private const string ItemReminderFieldName = "ItemReminder";
        private const string ItemReminderDateTimeFieldName = "ItemReminderDateTime";
        private const string ItemCategoryFieldName = "ItemCategory";
        private const string ItemDefaultFolderFieldName = "ItemDefaultFolder";
        private const string ItemFolderFieldName = "ItemFolder";
        private const string ItemDateUTCFieldName = "ItemDateUTC";
        private const string ItemDateTimeZoneFieldName = "ItemDateTimeZone";
        private const string ItemLastUpdatedDateFieldName = "ItemLastUpdatedDate";
        private const string ItemTaskStatusFieldName = "ItemTaskStatus";
        private const string ItemStatusFieldName = "ItemStatus";
        private const string ItemLastProcessedFieldName = "ItemLastProcessed";
        private const string ItemEWSIDFieldName = "ItemEWSID";
        private const string ItemErrorMessageFieldName = "ItemErrorMessage";
        private const string ItemLocationFieldName = "ItemLocation";
        private const string ItemMeetingAttendeesFieldName = "ItemMeetingAttendees";

        #endregion

        public DataRow MSEWSItemDataRow;
        public List<String> updatedColumnList = new List<string>();

        public bool MSEWSItemRowUpdated { get; private set; }

        public MSEWSItem(DataRow row)
        {
            MSEWSItemDataRow = row;
            MSEWSItemRowUpdated = false;
        }

        public string ItemID
        {
            get { return GetValue<string>(ItemIDFieldName); }
        }

        public string userEmail
        {
            get { return GetValue<string>(UserEmailFieldName); }
            set { SetValue<string>(UserEmailFieldName, value); }
        }
        public string iType
        {
            get { return GetValue<string>(ItemTypeFieldName); }
            set { SetValue<string>(ItemTypeFieldName, value); }
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
        public DateTime? iDueDate
        {
            get { return GetValue<DateTime?>(ItemDueDateFieldName); }
            set { SetValue<DateTime?>(ItemDueDateFieldName, value); }
        }
        public bool iReminderActive
        {
            get { return GetValue<bool>(ItemReminderFieldName); }
            set { SetValue<bool>(ItemReminderFieldName, value); }
        }
        public DateTime? iReminderDate
        {
            get { return GetValue<DateTime?>(ItemReminderDateTimeFieldName); }
            set { SetValue<DateTime?>(ItemReminderDateTimeFieldName, value); }
        }
        public string iCategory
        {
            get { return GetValue<string>(ItemCategoryFieldName); }
            set { SetValue<string>(ItemCategoryFieldName, value); }
        }
        public bool iDefaultFolder
        {
            get { return GetValue<bool>(ItemDefaultFolderFieldName); }
            set { SetValue<bool>(ItemDefaultFolderFieldName, value); }
        }
        public string iFolder
        {
            get { return GetValue<string>(ItemFolderFieldName); }
            set { SetValue<string>(ItemFolderFieldName, value); }
        }
        public bool iDateUTC
        {
            get { return GetValue<bool>(ItemDateUTCFieldName); }
            set { SetValue<bool>(ItemDateUTCFieldName, value); }
        }
        public string iDateTimeZone
        {
            get { return GetValue<string>(ItemDateTimeZoneFieldName); }
            set { SetValue<string>(ItemDateTimeZoneFieldName, value); }
        }
        public DateTime? iLastUpdatedDate
        {
            get { return GetValue<DateTime?>(ItemLastUpdatedDateFieldName); }
            set { SetValue<DateTime?>(ItemLastUpdatedDateFieldName, value); }
        }
        public DateTime? iLastProcessed
        {
            get { return GetValue<DateTime?>(ItemLastProcessedFieldName); }
            set { SetValue<DateTime?>(ItemLastProcessedFieldName, value); }
        }
        public string iTaskStatus
        {
            get { return GetValue<string>(ItemTaskStatusFieldName); }
            set { SetValue<string>(ItemTaskStatusFieldName, value); }
        }
        public Int64? iItemStatus
        {
            get { return GetValue<Int64?>(ItemStatusFieldName); }
            set { SetValue<Int64?>(ItemStatusFieldName, value); }
        }
        public string iEWSID
        {
            get { return GetValue<string>(ItemEWSIDFieldName); }
            set { SetValue<string>(ItemEWSIDFieldName, value); }
        }
        public string iErrorMessage
        {
            get { return GetValue<string>(ItemErrorMessageFieldName); }
            set { SetValue<string>(ItemErrorMessageFieldName, value); }
        }
        public string iLocation
        {
            get { return GetValue<string>(ItemLocationFieldName); }
            set { SetValue<string>(ItemLocationFieldName, value); }
        }
        public string iMeetingAttendees
        {
            get { return GetValue<string>(ItemMeetingAttendeesFieldName); }
            set { SetValue<string>(ItemMeetingAttendeesFieldName, value); }
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
