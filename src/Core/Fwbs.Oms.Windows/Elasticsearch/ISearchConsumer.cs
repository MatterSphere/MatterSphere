using System;
using System.Collections.Generic;
using FWBS.Common.Elasticsearch;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    public interface ISearchConsumer
    {
        event EventHandler onSearchOptionChanged;
        event EventHandler onSearchReseted;

        bool Enabled { get; set; }
        int PageSize { get; }
        int CurrentPage { get; }
        SearchFilter.EntityFilterData EntityFilter { get; }
        List<SearchFilter.EntityFilterData> LinkedEntityFilter { get; }
        List<EntityTypeEnum> EntityTypes { get; }
        List<SearchFilter.FieldFilterItem> FieldsFilter { get; }
        string SortField { get; }
        string SortOrder { get; }

        void SetResult(Common.Elasticsearch.SearchResult result);
        void SetEntity(IOMSType entity);
        void StartSearch();
        void ShowError(Exception exception);
    }
}
