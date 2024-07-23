using System;
using System.Collections.Generic;
using System.Data;

namespace DocumentArchivingClass
{

    class DocumentRecord
    {
        #region FieldNames
        private const string ArchiveIDFieldName = "ArchID";
        private const string DocumentIDFieldName = "ArchDocID";
        private const string DocumentIDFileFieldName = "ArchDocFileID";
        private const string ArchiveActionFieldName = "ArchAction";
        private const string ArchiveMessageFieldName = "ArchMessage";
        private const string ArchiveDirectoryIDFieldName = "ArchDirID";
        private const string CreatedByFieldName = "CreatedBy";
        private const string CreatedDateFieldName = "Created";
        private const string RunIDFieldName = "RunID";
        private const string ArchivedFieldName = "Archived";
        private const string DocumentSourceDirectoryIDFieldName = "docdirID";


        private const string ArchiveStatusFieldName = "ArchStatus";// its not used in sprDocArchiveGetList this might go as a newcolumn in dbDocumentArchiveInfo?
        private const string SourceDirectoryFieldName = "SourceDirectory";
        private const string DestinationDirectoryFieldName = "DestinationDirectory";
        #endregion

        public DataRow DocumentRecordDataRow;
        public List<String> updatedColumnList = new List<string>();

        public bool DocumentRecordRowUpdated { get; private set; }

        public DocumentRecord(DataRow row)
        {
            DocumentRecordDataRow = row;
            DocumentRecordRowUpdated = false;           
         }

        public Int64 ArchiveID
        {
            get { return GetValue<Int64>(ArchiveIDFieldName); }
        }

        public Int64 DocumentID
        {
            get { return GetValue<Int64>(DocumentIDFieldName); }
            set { SetValue<Int64>(DocumentIDFieldName, value); }
        }

        public Int64 DocumentFileID
        {
            get { return GetValue<Int64>(DocumentIDFileFieldName); }
            set { SetValue<Int64>(DocumentIDFileFieldName, value); }
        }
        public string ArchiveAction
        {
            get { return GetValue<string>(ArchiveActionFieldName); }
            set { SetValue<string>(ArchiveActionFieldName, value); }
        }
        public string ArchiveMessage
        {
            get { return GetValue<string>(ArchiveMessageFieldName); }
            set { SetValue<string>(ArchiveMessageFieldName, value); }
        }

        public string DestinationDirectory 
        {
            get { return GetValue<string>(DestinationDirectoryFieldName); }
            set { SetValue<string>(DestinationDirectoryFieldName, value); }
        }

        public string SourceDirectory  
        {
            get { return GetValue<string>(SourceDirectoryFieldName); }
            set { SetValue<string>(SourceDirectoryFieldName, value); }
        }
        public short ArchiveDirectoryID
        {
            get { return GetValue<short>(ArchiveDirectoryIDFieldName); }
            set { SetValue<short>(ArchiveDirectoryIDFieldName, value); }
        }
        public int CreatedBy
        {
            get { return GetValue<int>(CreatedByFieldName); }
            set { SetValue<int>(CreatedByFieldName, value); }
        }
        public DateTime CreatedDate
        {
            get { return GetValue<DateTime>(CreatedDateFieldName); }
            set { SetValue<DateTime>(CreatedDateFieldName, value); }
        }
        public Int64 RunID
        {
            get { return GetValue<Int64>(RunIDFieldName); }
            set { SetValue<Int64>(RunIDFieldName, value); }
        }


        public bool? Archived
        {
            get { return GetValue<bool?>(ArchivedFieldName); }
            set { SetValue<bool?>(ArchivedFieldName, value); }
        }


        public int? ArchiveStatus
        {
            get { return GetValue<int?>(ArchiveStatusFieldName); }
            set { SetValue<int?>(ArchiveStatusFieldName, value); }
        }


        public short DocumentSourceDirectoryID
        {
            get { return GetValue<short>(DocumentSourceDirectoryIDFieldName); }
            set { SetValue<short>(DocumentSourceDirectoryIDFieldName, value); }
        }



        private T GetValue<T>(string columnName)
        {
            return DocumentRecordDataRow.Field<T>(columnName);
        }

        private void SetValue<T>(string columnName, T value)
        {
            T colValue = GetValue<T>(columnName);
            if (colValue != null && colValue.Equals(value))
            {
                return;
            }
            DocumentRecordDataRow.SetField(columnName, value);
            if (updatedColumnList.Contains(columnName) == false)
            {
                updatedColumnList.Add(columnName);
            }
            DocumentRecordRowUpdated = true;
        }

    }
}
