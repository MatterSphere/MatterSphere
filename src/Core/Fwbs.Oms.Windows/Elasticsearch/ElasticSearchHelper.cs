using System.Windows.Forms;

namespace FWBS.OMS.UI.Elasticsearch
{
    static class ElasticsearchHelper
    {
        public static string ToElasticsearchString(this SortOrder sortOrder)
        {
            switch (sortOrder)
            {
                case SortOrder.Ascending:
                    return "asc";
                case SortOrder.Descending:
                    return "desc";
                default:
                    return string.Empty;
            }
        }
    }
}