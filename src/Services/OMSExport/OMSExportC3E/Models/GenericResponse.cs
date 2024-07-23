using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FWBS.OMS.OMSEXPORT.Models
{
    class GenericResponse : BaseResponse
    {
        private bool _success;
        [JsonProperty("success")]
        public override bool Success
        {
            get
            {
                return _success && base.Success;
            }
            set
            {
                _success = value;
            }
        }

        public override string ErrorMessage
        {
            get
            {
                return string.Concat(base.ErrorMessage, GetErrorMessage());
            }
            set
            {
                base.ErrorMessage = value;
            }
        }

        private string GetErrorMessage()
        {
            StringBuilder errorMessage = new StringBuilder();
            string separator = "\r\n";

            if (DataCollection != null)
            {
                foreach (var item in DataCollection.Errors)
                {
                    errorMessage.Append(separator).AppendFormat("{0}: {1}", item.AttributeId, item.Error.Message);
                }

                foreach (var row in DataCollection.Rows)
                {
                    if (row.IsLocked)
                    {
                        errorMessage.Append(separator).Append(row.LockedMessage);
                    }

                    GetInnerErrorMessage(row.ChildObjects, errorMessage, separator);
                }
            }

            return errorMessage.ToString();
        }

        private StringBuilder GetInnerErrorMessage(List<BaseCollection> childObjects, StringBuilder errorMessage, string separator)
        {
            foreach (var childObject in childObjects)
            {
                foreach (var error in childObject.Errors)
                {
                    errorMessage.Append(separator).AppendFormat("{0}: {1}", error.AttributeId, error.Error.Message);
                }

                foreach (var row in childObject.Rows)
                {
                    GetInnerErrorMessage(row.ChildObjects, errorMessage, separator);
                }
            }

            return errorMessage;
        }

        public string GetItemID()
        {
            return DataCollection.Rows[0].Attributes["ItemID"].Value;
        }

        public bool IsRecordLocked()
        {
            if (DataCollection != null)
            {
                foreach (var row in DataCollection.Rows)
                {
                    if (row.IsLocked)
                        return true;
                }
            }
            return false;
        }

        [JsonProperty("dataCollection")]
        public BaseCollection DataCollection { get; set; }

        public class BaseCollection
        {
            public BaseCollection()
            {
                Errors = new List<ResponseErrors>();
                Rows = new List<ResponseRows>();
            }

            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("objectId")]
            public string ObjectId { get; set; }

            [JsonProperty("actualRowCount")]
            public int ActualRowCount { get; set; }

            [JsonProperty("errors")]
            public List<ResponseErrors> Errors { get; set; }

            [JsonProperty("rows")]
            public List<ResponseRows> Rows { get; set; }

            public class ResponseRows
            {
                public ResponseRows()
                {
                    ChildObjects = new List<BaseCollection>();
                }

                [JsonProperty("id")]
                public string Id { get; set; }

                [JsonProperty("attributes")]
                public IDictionary<string, AttributeItem> Attributes { get; set; }

                [JsonProperty("childObjects")]
                public List<BaseCollection> ChildObjects { get; set; }

                [JsonProperty("index")]
                public int Index { get; set; }

                [JsonProperty("isLocked")]
                public bool IsLocked { get; set; }

                [JsonProperty("lockedMessage")]
                public string LockedMessage { get; set; }

                [JsonProperty("subclassId")]
                public string SubclassId { get; set; }
            }

            public class ResponseErrors
            {
                [JsonProperty("error")]
                public ResponseError Error { get; set; }

                [JsonProperty("rowId")]
                public string RowId { get; set; }

                [JsonProperty("index")]
                public int Index { get; set; }

                [JsonProperty("attributeId")]
                public string AttributeId { get; set; }

                public class ResponseError
                {
                    [JsonProperty("message")]
                    public string Message { get; set; }

                    [JsonProperty("type")]
                    public string Type { get; set; }

                    [JsonProperty("severity")]
                    public int Severity { get; set; }
                }
            }

            public class AttributeItem
            {
                [JsonProperty("aliasValue")]
                public string AliasValue { get; set; }

                [JsonProperty("displayValue")]
                public string DisplayValue { get; set; }

                [JsonProperty("value")]
                public string Value { get; set; }

                [JsonProperty("error")]
                public ResponseErrors.ResponseError Error { get; set; }
            }
        }
    }
}
