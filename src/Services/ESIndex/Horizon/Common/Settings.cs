namespace Horizon.Common
{
    public class Settings
    {
        public Settings(string connection, int interval, string queue, string esServer, string apiKey)
        {
            DbConnection = connection;
            Interval = interval;
            Queue = queue;
            ElasticsearchServer = esServer;
            ElasticsearchApiKey = apiKey;
            PageSize = 50;
        }

        public string DbConnection { get; set; }
        public int Interval { get; set; }
        public string Queue { get; set; }
        public int PageSize { get; set; }
        public string ElasticsearchServer { get; set; }
        public string ElasticsearchApiKey { get; set; }
    }
}
