using FWBS.OMS.UI.UserControls.Breadcrumbs;
using FWBS.OMS.UI.Windows.Interfaces;

namespace FWBS.OMS.UI.Windows
{
    internal class PageManager<T> where T : class, IDisplay
    {
        private readonly BreadCrumbsBuilder _bcBuilder;
        private readonly DisplayCollection<T> _displays;
        private ViewEnum _currentView = ViewEnum.Default;

        public PageManager(BreadCrumbsBuilder breadCrumbsBuilder, DisplayCollection<T> displays)
        {
            _bcBuilder = breadCrumbsBuilder;
            _displays = displays;
        }

        public void ShowPage(ViewEnum view, string title = null)
        {
            switch (view)
            {
                case ViewEnum.StartPoint:
                    _bcBuilder?.ShowRootItem(title, ViewEnum.StartPoint);
                    break;
                case ViewEnum.ElasticSearch:
                    ShowElasticSearch();
                    break;
                case ViewEnum.SearchManager:
                    ShowSearchManager();
                    break;
                default:
                    ShowDefaultPage();
                    break;
            }
        }

        public void HideElasticSearch()
        {
            if (_currentView != ViewEnum.ElasticSearch)
            {
                return;
            }

            CurrentDisplay.ResetElasticsearchResults();
            CurrentDisplay.RemoveElasticSearch();
            CurrentDisplay.ShowDefaultControls();
            _currentView = ViewEnum.Default;
        }

        public void SetViewType(ViewEnum view)
        {
            _currentView = view;
        }

        #region Private methods

        private IDisplay CurrentDisplay
        {
            get
            {
                return _displays?.LastDisplay;
            }
        }

        private void ShowElasticSearch()
        {
            switch (_currentView)
            {
                case ViewEnum.ElasticSearch:
                    return;
                case ViewEnum.SearchManager:
                    CurrentDisplay.HideSearchManager();
                    break;
            }

            _bcBuilder.AddSearchResultItem();
            CurrentDisplay.RemoveDefaultControls();
            CurrentDisplay.SetElasticsearch();
            _currentView = ViewEnum.ElasticSearch;
        }

        private void ShowSearchManager()
        {
            var title = Session.CurrentSession.Resources.GetResource("SMHEADING", "Search Manager", string.Empty).Text;
            _bcBuilder?.ShowRootItem(title, ViewEnum.SearchManager);
            _currentView = ViewEnum.SearchManager;
        }

        private void ShowDefaultPage()
        {
            switch (_currentView)
            {
                case ViewEnum.ElasticSearch:
                    HideElasticSearch();
                    break;
            }
            CurrentDisplay.ShowDefaultControls();
            _currentView = ViewEnum.Default;
        }

        #endregion
    }

    internal enum ViewEnum
    {
        StartPoint = 0,
        Default = 1,
        SearchManager = 2,
        ElasticSearch = 3
    }
}
