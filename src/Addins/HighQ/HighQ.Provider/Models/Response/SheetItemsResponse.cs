using Newtonsoft.Json;

namespace FWBS.OMS.HighQ.Models.Response
{
    internal class SheetItemsResponse
    {
        [JsonProperty("isheet")]
        public ContentModel Content { get; set; }

        [JsonIgnore]
        public ResponseModels.HeadColumn[] HeadColumns
        {
            get { return Content.Head.HeadColumns; }
        }

        [JsonIgnore]
        public ResponseModels.ItemModel[] Items
        {
            get { return Content.Data.Items; }
        }

        #region Classes

        public class ContentModel
        {
            [JsonProperty("head")]
            public ResponseModels.HeadModel Head { get; set; }

            [JsonProperty("data")]
            public DataModel Data { get; set; }
        }

        public class DataModel
        {
            [JsonProperty("item")]
            public ResponseModels.ItemModel[] Items { get; set; }
        }

        #endregion
    }
}
