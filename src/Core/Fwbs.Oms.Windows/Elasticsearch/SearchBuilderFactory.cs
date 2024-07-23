using System;
using FWBS.Common;
using FWBS.OMS.Search;

namespace FWBS.OMS.UI.Elasticsearch
{
    public class SearchBuilderFactory
    {
        public ISearchBuilder CreateSearchBuilder(bool withFacets = true)
        {
            if (Session.CurrentSession.IsESSearchConfigured)
            {
                return CreateElasticsearchBuilder(withFacets);
            }

            return CreateMatterSphereSearch(withFacets);
        }

        private FWBS.OMS.Elasticsearch.SearchBuilder CreateElasticsearchBuilder(bool withFacets = true)
        {
            var url = Convert.ToString(Session.CurrentSession.GetSpecificData("ES_SERV"));
            var apiKey = Convert.ToString(Session.CurrentSession.GetSpecificData("ES_APIKEY"));
            var dataIndex = Convert.ToString(Session.CurrentSession.GetSpecificData("ES_DIND"));
            FWBS.OMS.Elasticsearch.SearchBuilder searchBuilder;

            if (Session.CurrentSession.AdvancedSecurity && !Session.CurrentSession.CurrentUser.IsInRoles("SECADMIN"))
            {
                var userIndex = Convert.ToString(Session.CurrentSession.GetSpecificData("ES_UIND"));
                searchBuilder = new FWBS.OMS.Elasticsearch.SearchBuilder(url, apiKey, dataIndex, userIndex);
                searchBuilder.SetUser(
                    Session.CurrentSession.CurrentUser.ActiveDirectoryID,
                    Session.CurrentSession.CurrentUser.SQLServerLogin);
            }
            else
            {
                searchBuilder = new FWBS.OMS.Elasticsearch.SearchBuilder(url, apiKey, dataIndex);
            }

            int facetSize = withFacets ? ConvertDef.ToInt32(Session.CurrentSession.GetSpecificData("ES_FACSIZE"), 5) : 0;
            searchBuilder.Build(facetSize);

            return searchBuilder;
        }

        private MSSearchBuilder CreateMatterSphereSearch(bool withFacets = true)
        {
            var cdsConnection = Convert.ToString(Session.CurrentSession.GetSpecificData("CDS_CON"));
            MSSearchBuilder searchBuilder = new MSSearchBuilder(cdsConnection);

            int facetSize = withFacets ? ConvertDef.ToInt32(Session.CurrentSession.GetSpecificData("MS_FACSIZE"), 5) : 0;
            searchBuilder.Build(facetSize);

            return searchBuilder;
        }
    }
}
