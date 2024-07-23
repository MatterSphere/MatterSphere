namespace Models.ElasticsearchModels
{
    public class ProcessingItemLog
    {
        public string ItemId { get; set; }
        public bool IsSucceeded { get; set; }
        public string Result { get; set; }
        public string ErrorType { get; set; }
        public string ErrorReason { get; set; }
    }
}
