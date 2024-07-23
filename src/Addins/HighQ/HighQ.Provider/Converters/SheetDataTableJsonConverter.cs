using System;
using System.Collections.Generic;
using System.Data;
using FWBS.OMS.HighQ.Converters.TokenPathProviders;
using FWBS.OMS.HighQ.Models.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FWBS.OMS.HighQ.Converters
{
    class SheetDataTableJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var columns = new Dictionary<string, ColumnHead>();
            var dataTable = new DataTable();

            JObject o = JObject.Load(reader);
            int recordCount = Convert.ToInt32(o.SelectToken("isheet.totalrecordcount"));
            ITokenPathProvider tokenPathProvider = recordCount == 1 
                ? (ITokenPathProvider)new SingleItemTokenPathProvider() 
                : new PluralItemsTokenPathProvider();
            dataTable.Columns.Add("ItemId", typeof(string));
            foreach (var headColumnToken in o.SelectTokens("isheet.head.headcolumn").Children())
            {
                var headColumn = headColumnToken.ToObject<ColumnHead>();
                dataTable.Columns.Add(Convert.ToString(headColumn.ColumnValue), typeof(string));
                columns.Add(headColumn.ColumnId, headColumn);
            }

            for (int i = 0; i < recordCount; i++)
            {
                DataRow dataRow = dataTable.NewRow();

                dataRow["ItemId"] = o.SelectToken(tokenPathProvider.GetItemIdPath(i));
                for (int j = 0; j < columns.Count; j++)
                {
                    var columnHeader = columns[Convert.ToString(o.SelectToken(tokenPathProvider.GetColumnIdPath(i, j)))];
                    var type = GetColumnType(columnHeader.ColumnTypeAlias);
                    var columnValue = o.SelectToken(tokenPathProvider.GetColumnValuePath(i, j)).ToObject(type);
                    dataRow[columnHeader.ColumnValue] = columnValue.ToString();
                }
                dataTable.Rows.Add(dataRow);
            }

            return dataTable;
        }

        private Type GetColumnType(string columnHeaderColumnTypeAlias)
        {
            switch (columnHeaderColumnTypeAlias)
            {
                case "SHEET_COLUMN_TYPE_SINGLE_LINE_TEXT":
                case "SHEET_COLUMN_TYPE_MULTIPLE_LINE_TEXT":
                case "SHEET_COLUMN_TYPE_DATE_AND_TIME":
                    return typeof(ColumnValue<TextColumn>);
                case "SHEET_COLUMN_TYPE_CHOICE":
                    return typeof(ColumnValue<ChoicesColumn>);
                case "SHEET_COLUMN_TYPE_LOOKUP":
                    return typeof(ColumnValue<LookupColumn>);
                case "SHEET_COLUMN_TYPE_FOLDER_LINK":
                    return typeof(ColumnValue<FoldersColumn>);
            }
            return typeof(ColumnValue<TextColumn>);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DataTable);
        }

        class ColumnHead
        {
            [JsonProperty("columnid")]
            public string ColumnId { get; set; }
            [JsonProperty("columnvalue")]
            public string ColumnValue { get; set; }
            [JsonProperty("columntypealias")]
            public string ColumnTypeAlias { get; set; }
        }

        class ColumnValue<T>
        {
            [JsonProperty("displaydata")]
            T DisplayData { get; set; }

            public override string ToString()
            {
                return DisplayData.ToString();
            }
        }

        class TextColumn
        {
            [JsonProperty("value")]
            public string Value { get; set; }

            public override string ToString()
            {
                return Value;
            }
        }

        class ChoicesColumn
        {
            [JsonProperty("choices")]
            public ResponseModels.ChoicesModel Choices { get; set; }

            public override string ToString()
            {
                return Choices?.ToString();
            }
        }

        class LookupColumn
        {
            [JsonProperty("lookupusers")]
            public ResponseModels.LookupUsersModel LookupUsers { get; set; }

            public override string ToString()
            {
                return LookupUsers?.ToString();
            }
        }

        class FoldersColumn
        {
            [JsonProperty("folders")]
            public ResponseModels.FoldersModel Folders { get; set; }

            public override string ToString()
            {
                return Folders?.ToString();
            }
        }
    }
}
