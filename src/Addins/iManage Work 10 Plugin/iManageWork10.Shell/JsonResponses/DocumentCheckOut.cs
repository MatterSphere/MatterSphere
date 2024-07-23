using Newtonsoft.Json;

namespace iManageWork10.Shell.JsonResponses
{
    public class DocumentCheckOut
    {
        [JsonProperty("in_use_by")]
        public string InUseBy { get; set; }

        [JsonProperty("checkout_due_date")]
        public string DueDate { get; set; }

        [JsonProperty("checkout_path")]
        public string Path { get; set; }

        [JsonProperty("checkout_comments")]
        public string Comments { get; set; }

        [JsonProperty("checkout_location")]
        public string Location { get; set; }

        public override string ToString()
        {
            return $"{GetType()}: [InUseBy: {InUseBy}, DueDate: {DueDate}, Path: {Path}, Comments: {Comments}, Location: {Location}]";
        }
    }
}
