namespace Models.ElasticsearchModels
{
    public class ElasticsearchClientParameters
    {
        public string Url { get; private set; }
        public string ApiKey { get; private set; }

        public ElasticsearchClientParameters(string url, string apiKey)
        {
            this.Url = url;
            this.ApiKey = apiKey;
        }
    }
}
