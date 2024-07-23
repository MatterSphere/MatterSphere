using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace ElasticsearchProvider.Models
{
    public class UpdateItemInfo
    {
        public UpdateItemInfo()
        {
            Items = new List<Item>();
        }

        [JsonProperty("errors")]
        public bool HasErrors { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        public int SuccessNumber
        {
            get { return Items.Count(item => item.Info != null && item.Info.IsSuccess); }
        }

        public int FailedNumber
        {
            get { return Items.Count(item => item.Info != null && !item.Info.IsSuccess); }
        }

        public class InfoItem
        {
            [JsonProperty("_id")]
            public string Id { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("result")]
            public string Result { get; set; }

            [JsonProperty("error")]
            public ErrorInfo Error { get; set; }

            public bool IsSuccess
            {
                get { return Status == "200" || Status == "201"; }
            }

            public Guid Key
            {
                get { return Guid.Parse(Id); }
            }
        }

        public class Item
        {
            [JsonProperty("update")]
            public InfoItem Info { get; set; }
        }

        public class ErrorInfo
        {
            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("reason")]
            public string Reason { get; set; }
        }
    }
}
