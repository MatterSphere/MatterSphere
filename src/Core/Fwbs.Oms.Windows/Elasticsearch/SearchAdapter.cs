using System;
using System.Linq;
using System.Threading.Tasks;
using FWBS.Common;
using FWBS.Common.Elasticsearch;
using FWBS.OMS.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    class SearchAdapter : ISearchAdapter
    {
        private ISearchProducer _producer;
        private ISearchConsumer _consumer;
        private ISearchBuilder _builder;
        private readonly int _maximumSuggestsAmount;

        public SearchAdapter(ISearchProducer producer, ISearchConsumer consumer, ISearchBuilder builder)
        {
            _producer = producer;
            _consumer = consumer;
            _builder = builder;
            _maximumSuggestsAmount = Session.CurrentSession.MaximumSuggestsAmount;

            _consumer.onSearchOptionChanged += ChangeSearchOptions;
            _consumer.onSearchReseted += ResetSearch;
            _producer.QueryChanged += Producer_QueryChanged;
            _producer.FilterChanged += Producer_FilterChanged;
        }

        #region ISearchAdapter
        public void SetPageSource(IOMSType entity)
        {
            _consumer?.SetEntity(entity);
        }

        public async void SearchAsync()
        {
            var request = new SearchFilter(_producer.Query)
            {
                PageInfo = new SearchFilter.PageData(
                    page: _consumer.CurrentPage - 1,
                    size: _consumer.PageSize),
                EntityFilter = _consumer.EntityFilter,
                LinkedEntityFilter = _consumer.LinkedEntityFilter.ToList(),
                TypesFilter = _consumer.EntityTypes.ToList(),
                FieldsFilter = _consumer.FieldsFilter.ToList(),
                WithHighlights = true
            };
            if (!string.IsNullOrEmpty(_consumer.SortField) && !string.IsNullOrEmpty(_consumer.SortOrder))
            {
                request.SortInfo = new SearchFilter.SortData
                {
                    Field = _consumer.SortField,
                    Order = _consumer.SortOrder
                };
            }

            _consumer.Enabled = false;
            _producer.ShowProgress(true);
            try
            {
                var result = await System.Threading.Tasks.Task.Run(() => _builder.Search(request));
                _consumer?.SetResult(result);
                _producer?.ShowProgress(false);
            }
            catch (Exception e)
            {
                _producer?.ShowProgress(false);
                _consumer?.ShowError(e);
            }
            finally
            {
                if (_consumer != null)
                    _consumer.Enabled = true;
            }
        }
        #endregion ISearchAdapter
        
        private void ChangeSearchOptions(object sender, EventArgs e)
        {
            SearchAsync();
        }

        private void ResetSearch(object sender, EventArgs e)
        {
            _producer.ClearQuery();
        }

        private async void Producer_QueryChanged(object sender, QueryChangedEventArgs e)
        {
            try
            {
                var result = await System.Threading.Tasks.Task.Run(
                    () => _builder.GetSuggests(e.Query, _maximumSuggestsAmount), e.CancellationToken);
                if (!e.CancellationToken.IsCancellationRequested)
                {
                    _producer?.SetSuggests(result.ToArray());
                }
            }
            catch (TaskCanceledException)
            {
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex);
            }
        }

        private void Producer_FilterChanged(object sender, FilterChangedEventArgs e)
        {
            _consumer.EntityTypes.Clear();
            _consumer.EntityTypes.AddRange(e.SelectedTypes);

            _consumer.FieldsFilter.RemoveAll(item => item.Field.Equals("documentStartDate", StringComparison.OrdinalIgnoreCase) || item.Field.Equals("documentEndDate", StringComparison.OrdinalIgnoreCase));
            foreach (var docOrEmail in e.SelectedTypes.Where(x => x == EntityTypeEnum.Document || x == EntityTypeEnum.Email))
            {
                if (!string.IsNullOrEmpty(e.DocumentsDateRange.Item1))
                    _consumer.FieldsFilter.Add(new SearchFilter.FieldFilterItem("documentStartDate", e.DocumentsDateRange.Item1)
                        { EntityType = docOrEmail, Operator = ComparisonOperator.GreaterOrEqual, TargetField = "modifieddate" });

                if (!string.IsNullOrEmpty(e.DocumentsDateRange.Item2))
                    _consumer.FieldsFilter.Add(new SearchFilter.FieldFilterItem("documentEndDate", e.DocumentsDateRange.Item2)
                        { EntityType = docOrEmail, Operator = ComparisonOperator.LessOrEqual, TargetField = "modifieddate" });
            }
        }

        #region IDisposable
        public void Dispose()
        {
            if (_consumer != null)
            {
                _consumer.onSearchOptionChanged -= ChangeSearchOptions;
                _consumer.onSearchReseted -= ResetSearch;
                _consumer = null;
            }
            if (_producer != null)
            {
                _producer.QueryChanged -= Producer_QueryChanged;
                _producer.FilterChanged -= Producer_FilterChanged;
                _producer = null;
            }
        }
        #endregion IDisposable
    }
}
