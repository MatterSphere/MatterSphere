using FWBS.Common.Elasticsearch;
using FWBS.OMS.Search;

namespace FWBS.OMS.UI.Elasticsearch
{
    public class DbProviderFactory
    {
        public IDbProvider CreateDbProvider()
        {
            if (Session.CurrentSession.IsESSearchConfigured)
            {
                return new OMS.Elasticsearch.DbProvider();
            }

            if (Session.CurrentSession.IsMSSearchConfigured)
            {
                return new DbProvider();
            }

            return new StubDbProvider();
        }
    }
}
