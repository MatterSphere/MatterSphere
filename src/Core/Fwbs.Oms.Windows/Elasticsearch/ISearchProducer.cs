using System;
using System.Collections.Generic;
using System.Threading;

namespace FWBS.OMS.UI.Windows
{
    public interface ISearchProducer
    {
        event EventHandler SearchStarted;
        event EventHandler<FilterChangedEventArgs> FilterChanged;
        event EventHandler<QueryChangedEventArgs> QueryChanged;
        string Query { get; }
        void ClearQuery();
        void SetSuggests(string[] suggests);
        void ShowProgress(bool show);
    }

    public class FilterChangedEventArgs : EventArgs
    {
        public IEnumerable<Common.Elasticsearch.EntityTypeEnum> SelectedTypes { get; set; }
        public Tuple<string, string> DocumentsDateRange { get; set; }
    }

    public class QueryChangedEventArgs : EventArgs
    {
        public QueryChangedEventArgs(string query, CancellationToken cancellationToken)
        {
            Query = query;
            CancellationToken = cancellationToken;
        }

        public string Query { get; set; }
        public CancellationToken CancellationToken { get; private set; }
    }
}
