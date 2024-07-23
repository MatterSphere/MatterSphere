using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Horizon.Annotations;
using Horizon.Common;
using Horizon.Common.Models.Common;
using Horizon.ViewModels.Common;
using Horizon.ViewModels.IFilters;
using Horizon.ViewModels.IndexProcess;
using Horizon.ViewModels.IndexReports;
using Horizon.ViewModels.ProcessingStatus;
using Horizon.ViewModels.Settings;
using Horizon.ViewModels.StartPage;

namespace Horizon.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public delegate void SearchDelegate(string search);
        public event SearchDelegate onSearch;

        public MainViewModel(Window wnd)
        {
            Window = wnd;

            int interval;
            var convertInterval = Int32.TryParse(ConfigurationManager.AppSettings["Interval"], out interval);

            Settings = new Horizon.Common.Settings(
                ConfigurationManager.AppSettings["DbConnection"],
                convertInterval ? interval : 5000,
                ConfigurationManager.AppSettings["Queue"],
                ConfigurationManager.AppSettings["ElasticsearchServer"],
                ConfigurationManager.AppSettings["ElasticsearchApiKey"]);

            OpenStartPage();
            StartPageChecked = true;
        }

        #region Properties

        public Window Window { get; private set; }

        private PageViewModel _currentPage;
        public PageViewModel CurrentPage
        {
            get { return _currentPage; }
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    _currentPage.SetStatusBarContent();
                    OnPropertyChanged();
                }
            }
        }

        private Horizon.Common.Settings _settings;
        public Horizon.Common.Settings Settings
        {
            get { return _settings; }
            set
            {
                if (_settings != value)
                {
                    _settings = value;
                    OnPropertyChanged();
                }
            }
        }

        private StatusBarViewModel _statusBar;
        public StatusBarViewModel StatusBar
        {
            get { return _statusBar; }
            set
            {
                if (_statusBar != value)
                {
                    _statusBar = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<IFilterListItemViewModel> IFilters { get; set; }

        #endregion

        #region Commands

        #region OpenPredefinedTests
        private ICommand _openPredefinedTestsCommand;
        public ICommand OpenPredefinedTestsCommand
        {
            get
            {
                if (_openPredefinedTestsCommand == null)
                {
                    _openPredefinedTestsCommand = new RelayCommand(
                        rc => this.CanOpenPredefinedTests(),
                        rc => this.OpenPredefinedTests());
                }
                return _openPredefinedTestsCommand;
            }
        }

        private bool CanOpenPredefinedTests()
        {
            return true;
        }

        private void OpenPredefinedTests()
        {
            CurrentPage = new PredefinedTestViewModel(this);
        }
        #endregion

        #region OpenUserDocumentTest
        private ICommand _openUserDocumentTestCommand;
        public ICommand OpenUserDocumentTestCommand
        {
            get
            {
                if (_openUserDocumentTestCommand == null)
                {
                    _openUserDocumentTestCommand = new RelayCommand(
                        rc => this.CanOpenUserDocumentTest(),
                        rc => this.OpenUserDocumentTest());
                }
                return _openUserDocumentTestCommand;
            }
        }

        private bool CanOpenUserDocumentTest()
        {
            return true;
        }

        private void OpenUserDocumentTest()
        {
            CurrentPage = new UserDocumentTestViewModel(this);
        }
        #endregion

        #region OpenIFilterList
        private ICommand _openIFilterListCommand;
        public ICommand OpenIFilterListCommand
        {
            get
            {
                if (_openIFilterListCommand == null)
                {
                    _openIFilterListCommand = new RelayCommand(
                        rc => this.CanOpenIFilterList(),
                        rc => this.OpenIFilterList());
                }
                return _openIFilterListCommand;
            }
        }

        private bool CanOpenIFilterList()
        {
            return true;
        }

        private void OpenIFilterList()
        {
            CurrentPage = new IFilterListViewModel(this, IFilters);
        }
        #endregion

        #region Search
        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    Search();
                }
            }
        }

        private void Search()
        {
            if (onSearch != null)
            {
                onSearch(SearchText);
            }
        }

        private bool _canSearch;
        public bool CanSearch
        {
            get { return _canSearch; }
            set
            {
                if (_canSearch != value)
                {
                    _canSearch = value;
                    OnPropertyChanged();

                    if (!value)
                    {
                        onSearch = null;
                        SearchText = null;
                    }
                }
            }
        }
        #endregion

        #region IndexStructure
        private ICommand _openIndexStructureCommand;
        public ICommand OpenIndexStructureCommand
        {
            get
            {
                if (_openIndexStructureCommand == null)
                {
                    _openIndexStructureCommand = new RelayCommand(
                        rc => this.CanOpenIndexStructure(),
                        rc => this.OpenIndexStructure());
                }
                return _openIndexStructureCommand;
            }
        }

        private bool CanOpenIndexStructure()
        {
            return true;
        }

        private void OpenIndexStructure()
        {
            CurrentPage = new IndexStructureViewModel(this);
        }
        #endregion

        #region OpenDocuments
        private ICommand _openDocumentsCommand;
        public ICommand OpenDocumentsCommand
        {
            get
            {
                if (_openDocumentsCommand == null)
                {
                    _openDocumentsCommand = new RelayCommand(
                        rc => this.CanOpenDocuments(),
                        rc => this.OpenDocuments());
                }
                return _openDocumentsCommand;
            }
        }

        private bool CanOpenDocuments()
        {
            return Settings != null && !string.IsNullOrWhiteSpace(Settings.DbConnection);
        }

        private void OpenDocuments()
        {
            CurrentPage = new DocumentsDataViewModel(this, ContentableEntityTypeEnum.Document);
        }
        #endregion

        #region OpenPrecedents
        private ICommand _openPrecedentsCommand;
        public ICommand OpenPrecedentsCommand
        {
            get
            {
                if (_openPrecedentsCommand == null)
                {
                    _openPrecedentsCommand = new RelayCommand(
                        rc => this.CanOpenPrecedents(),
                        rc => this.OpenPrecedents());
                }
                return _openPrecedentsCommand;
            }
        }

        private bool CanOpenPrecedents()
        {
            return Settings != null && !string.IsNullOrWhiteSpace(Settings.DbConnection);
        }

        private void OpenPrecedents()
        {
            CurrentPage = new DocumentsDataViewModel(this, ContentableEntityTypeEnum.Precedent);
        }
        #endregion

        #region OpenHistory
        private ICommand _openHistoryCommand;
        public ICommand OpenHistoryCommand
        {
            get
            {
                if (_openHistoryCommand == null)
                {
                    _openHistoryCommand = new RelayCommand(
                        rc => this.CanOpenHistory(),
                        rc => this.OpenHistory());
                }
                return _openHistoryCommand;
            }
        }

        private bool CanOpenHistory()
        {
            return Settings != null && !string.IsNullOrWhiteSpace(Settings.DbConnection);
        }

        private void OpenHistory()
        {
            CurrentPage = new ProcessingStatusViewModel(this);
        }
        #endregion

        #region OpenBlacklist
        private ICommand _openBlacklistCommand;
        public ICommand OpenBlacklistCommand
        {
            get
            {
                if (_openBlacklistCommand == null)
                {
                    _openBlacklistCommand = new RelayCommand(
                        rc => this.CanOpenBlacklist(),
                        rc => this.OpenBlacklist());
                }
                return _openBlacklistCommand;
            }
        }

        private bool CanOpenBlacklist()
        {
            return Settings != null && !string.IsNullOrWhiteSpace(Settings.DbConnection);
        }

        private void OpenBlacklist()
        {
            CurrentPage = new BlacklistViewModel(this);
        }
        #endregion

        #region OpenReindexDocuments
        private ICommand _openReindexDocumentsCommand;
        public ICommand OpenReindexDocumentsCommand
        {
            get
            {
                if (_openReindexDocumentsCommand == null)
                {
                    _openReindexDocumentsCommand = new RelayCommand(
                        rc => this.CanOpenReindexDocuments(),
                        rc => this.OpenReindexDocuments());
                }
                return _openReindexDocumentsCommand;
            }
        }

        private bool CanOpenReindexDocuments()
        {
            return Settings != null && !string.IsNullOrWhiteSpace(Settings.DbConnection);
        }

        private void OpenReindexDocuments()
        {
            CurrentPage = new ReindexDocumentsViewModel(this);
        }
        #endregion

        #region OpenIndexingSettings
        private ICommand _openIndexSettingsCommand;
        public ICommand OpenIndexSettingsCommand
        {
            get
            {
                if (_openIndexSettingsCommand == null)
                {
                    _openIndexSettingsCommand = new RelayCommand(
                        rc => this.CanOpenIndexSettings(),
                        rc => this.OpenIndexSettings());
                }
                return _openIndexSettingsCommand;
            }
        }

        private bool CanOpenIndexSettings()
        {
            return Settings != null && !string.IsNullOrWhiteSpace(Settings.DbConnection);
        }

        private void OpenIndexSettings()
        {
            CurrentPage = new IndexSettingsViewModel(this);
        }
        #endregion

        #region Open About

        private ICommand _openAboutCommand;
        public ICommand OpenAboutCommand
        {
            get
            {
                if (_openAboutCommand == null)
                {
                    _openAboutCommand = new RelayCommand(
                        rc => Settings != null,
                        rc => this.OpenAbout());
                }
                return _openAboutCommand;
            }
        }

        private void OpenAbout()
        {
            CurrentPage = new AboutViewModel(this);
        }

        #endregion

        #region OpenStartPage
        private ICommand _openStartPageCommand;
        public ICommand OpenStartPageCommand
        {
            get
            {
                if (_openStartPageCommand == null)
                {
                    _openStartPageCommand = new RelayCommand(
                        rc => this.CanOpenStartPage(),
                        rc => this.OpenStartPage());
                }
                return _openStartPageCommand;
            }
        }

        private bool CanOpenStartPage()
        {
            return Settings != null && !string.IsNullOrWhiteSpace(Settings.DbConnection);
        }

        private void OpenStartPage()
        {
            CurrentPage = new StartPageViewModel(this);
        }
        #endregion

        #region OpenIndexStatus
        private ICommand _openIndexStatusCommand;
        public ICommand OpenIndexStatusCommand
        {
            get
            {
                if (_openIndexStatusCommand == null)
                {
                    _openIndexStatusCommand = new RelayCommand(
                        rc => this.CanOpenIndexStatus(),
                        rc => this.OpenIndexStatus());
                }
                return _openIndexStatusCommand;
            }
        }

        private bool CanOpenIndexStatus()
        {
            return Settings != null && !string.IsNullOrWhiteSpace(Settings.DbConnection);
        }

        private void OpenIndexStatus()
        {
            CurrentPage = new IndexStatusViewModel(this);
        }
        #endregion

        private ICommand _openFacetOrderCommand;
        public ICommand OpenFacetOrderCommand
        {
            get
            {
                if (_openFacetOrderCommand == null)
                {
                    _openFacetOrderCommand = new RelayCommand(
                        rc => this.CanOpenFacetOrderCommand(),
                        rc => this.OpenFacetOrder());
                }
                return _openFacetOrderCommand;
            }
        }

        private bool CanOpenFacetOrderCommand()
        {
            return Settings != null && !string.IsNullOrWhiteSpace(Settings.DbConnection);
        }

        private void OpenFacetOrder()
        {
            CurrentPage = new FacetOrderViewModel(this);
        }


        #endregion

        #region Menu navigation
        private bool _startPageChecked;
        public bool StartPageChecked
        {
            get { return _startPageChecked; }
            set
            {
                if (_startPageChecked != value)
                {
                    _startPageChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _documentsReadChecked;
        public bool DocumentsReadChecked
        {
            get { return _documentsReadChecked; }
            set
            {
                if (_documentsReadChecked != value)
                {
                    _documentsReadChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _precedentsReadChecked;
        public bool PrecedentsReadChecked
        {
            get { return _precedentsReadChecked; }
            set
            {
                if (_precedentsReadChecked != value)
                {
                    _precedentsReadChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _blacklistChecked;
        public bool BlacklistChecked
        {
            get { return _blacklistChecked; }
            set
            {
                if (_blacklistChecked != value)
                {
                    _blacklistChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _processStatusChecked;
        public bool ProcessStatusChecked
        {
            get { return _processStatusChecked; }
            set
            {
                if (_processStatusChecked != value)
                {
                    _processStatusChecked = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isTestsExpended;
        public bool IsTestsExpended
        {
            get { return _isTestsExpended; }
            set
            {
                if (_isTestsExpended != value)
                {
                    _isTestsExpended = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isCrawlProcessesExpended;
        public bool IsCrawlProcessesExpended
        {
            get { return _isCrawlProcessesExpended; }
            set
            {
                if (_isCrawlProcessesExpended != value)
                {
                    _isCrawlProcessesExpended = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isiFiltersExpended;
        public bool IsiFiltersExpended
        {
            get { return _isiFiltersExpended; }
            set
            {
                if (_isiFiltersExpended != value)
                {
                    _isiFiltersExpended = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isSettingsExpended;
        public bool IsSettingsExpended
        {
            get { return _isSettingsExpended; }
            set
            {
                if (_isSettingsExpended != value)
                {
                    _isSettingsExpended = value;
                    OnPropertyChanged();
                }
            }
        }

        public void OpenCrawlProcessMenu()
        {
            IsTestsExpended = false;
            IsCrawlProcessesExpended = true;
            IsiFiltersExpended = false;
            IsSettingsExpended = false;
        }

        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
