using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses
{
    public class DocumentProfile
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("extension")]
        public string Extension { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("class")]
        public string Class { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }
        
        [JsonProperty("custom1")]
        public string Custom1 { get; set; }

        [JsonProperty("custom2")]
        public string Custom2 { get; set; }

        [JsonProperty("custom3")]
        public string Custom3 { get; set; }

        [JsonProperty("custom4")]
        public string Custom4 { get; set; }

        [JsonProperty("custom5")]
        public string Custom5 { get; set; }

        [JsonProperty("custom6")]
        public string Custom6 { get; set; }

        [JsonProperty("custom7")]
        public string Custom7 { get; set; }

        [JsonProperty("custom8")]
        public string Custom8 { get; set; }

        [JsonProperty("custom9")]
        public string Custom9 { get; set; }

        [JsonProperty("custom10")]
        public string Custom10 { get; set; }

        [JsonProperty("custom11")]
        public string Custom11 { get; set; }

        [JsonProperty("custom12")]
        public string Custom12 { get; set; }

        [JsonProperty("custom13")]
        public string Custom13 { get; set; }

        [JsonProperty("custom14")]
        public string Custom14 { get; set; }

        [JsonProperty("custom15")]
        public string Custom15 { get; set; }

        [JsonProperty("custom16")]
        public string Custom16 { get; set; }

        [JsonProperty("custom17")]
        public string Custom17 { get; set; }

        [JsonProperty("custom18")]
        public string Custom18 { get; set; }

        [JsonProperty("custom19")]
        public string Custom19 { get; set; }

        [JsonProperty("custom20")]
        public string Custom20 { get; set; }

        [JsonProperty("custom21")]
        public string Custom21 { get; set; }

        [JsonProperty("custom22")]
        public string Custom22 { get; set; }

        [JsonProperty("custom23")]
        public string Custom23 { get; set; }

        [JsonProperty("custom24")]
        public string Custom24 { get; set; }

        [JsonProperty("custom25")]
        public string Custom25 { get; set; }

        [JsonProperty("custom26")]
        public string Custom26 { get; set; }

        [JsonProperty("custom27")]
        public string Custom27 { get; set; }

        [JsonProperty("custom28")]
        public string Custom28 { get; set; }

        [JsonProperty("custom29")]
        public string Custom29 { get; set; }

        [JsonProperty("custom30")]
        public string Custom30 { get; set; }

        public override string ToString()
        {
            return $"{GetType()}: [Id: {Id}, Name: {Name}, Size: {Size}]";
        }
    }
}
