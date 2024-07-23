using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Horizon.Common;
using Horizon.Common.Models.Common;
using Horizon.Common.Models.Repositories.Blacklist;
using Horizon.DAL;
using Horizon.Models.ReportProvider;
using Horizon.Providers;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.IndexReports
{
    public class DocumentsDataViewModel : PageViewModel
    {
        private Provider _provider;
        private List<BlacklistItem> _blacklist;
        private ContentableEntityTypeEnum _entityType;

        public DocumentsDataViewModel(MainViewModel mainViewModel, ContentableEntityTypeEnum entityType) : base(mainViewModel)
        {
            _entityType = entityType;
            _provider = new Provider(MainViewModel.Settings.DbConnection);
            _blacklist = new List<BlacklistItem>();
            Documents = new ObservableCollection<DocumentBucketViewModel>();
            LoadDocumentBuckets();
        }

        #region Properties
        public ObservableCollection<DocumentBucketViewModel> Documents { get; set; }

        private DocumentBucketViewModel _selectedDocument;
        public DocumentBucketViewModel SelectedDocument
        {
            get { return _selectedDocument; }
            set
            {
                if (_selectedDocument != value)
                {
                    _selectedDocument = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(AddToBlacklistVisible));
                    OnPropertyChanged(nameof(ExcludeFromBlacklistVisible));
                }
            }
        }
        #endregion

        #region Commands

        #region OpenDocumentErrorList
        private ICommand _openDocumentErrorsCommand;
        public ICommand OpenDocumentErrorsCommand
        {
            get
            {
                if (_openDocumentErrorsCommand == null)
                {
                    _openDocumentErrorsCommand = new RelayCommand(
                        rc => this.CanOpenDocumentError(),
                        rc => this.OpenDocumentError());
                }
                return _openDocumentErrorsCommand;
            }
        }

        private bool CanOpenDocumentError()
        {
            return SelectedDocument != null && SelectedDocument.Failed > 0;
        }

        private void OpenDocumentError()
        {
            var nextPage = new DocumentErrorBucketsViewModel(this.MainViewModel, SelectedDocument.Type, _entityType);
            nextPage.PreviousPage = this;
            MainViewModel.CurrentPage = nextPage;
        }
        #endregion

        #region AddToBlacklist
        private ICommand _addToBlacklistCommand;
        public ICommand AddToBlacklistCommand
        {
            get
            {
                if (_addToBlacklistCommand == null)
                {
                    _addToBlacklistCommand = new RelayCommand(
                        rc => this.CanAddToBlacklist(),
                        rc => this.AddToBlacklist());
                }
                return _addToBlacklistCommand;
            }
        }

        private bool CanAddToBlacklist()
        {
            return SelectedDocument != null && !SelectedDocument.Blacklist.Any();
        }

        public bool AddToBlacklistVisible
        {
            get
            {
                return SelectedDocument != null && !SelectedDocument.Blacklist.Any();
            }
        }

        private void AddToBlacklist()
        {
            var blacklistItem = new BlacklistItem(SelectedDocument.Type);
            var result = _provider.AddBlacklistItem(blacklistItem);
            if (result.IsSuccess)
            {
                SelectedDocument.Blacklist.Add(blacklistItem);
                _blacklist.Add(blacklistItem);

                OnPropertyChanged(nameof(AddToBlacklistVisible));
                OnPropertyChanged(nameof(ExcludeFromBlacklistVisible));

                var breProvider = new BusinessRulesEngineProvider(_blacklist.ToList());
                var data = breProvider.GetBREData(SelectedDocument.Type, SelectedDocument.Failed);
                SelectedDocument.SetBREData(data);
            }
            else
            {
                ShowErrorMessage(result.ErrorMessage);
            }
        }
        #endregion

        #region ExcludeFromBlacklist
        private ICommand _excludeFromBlacklistCommand;
        public ICommand ExcludeFromBlacklistCommand
        {
            get
            {
                if (_excludeFromBlacklistCommand == null)
                {
                    _excludeFromBlacklistCommand = new RelayCommand(
                        rc => this.CanExcludeFromBlacklist(),
                        rc => this.ExcludeFromBlacklist());
                }
                return _excludeFromBlacklistCommand;
            }
        }

        private bool CanExcludeFromBlacklist()
        {
            return SelectedDocument != null && SelectedDocument.Blacklist.Any();
        }

        public bool ExcludeFromBlacklistVisible
        {
            get
            {
                return SelectedDocument != null && SelectedDocument.Blacklist.Any();
            }
        }

        private void ExcludeFromBlacklist()
        {
            var result = _provider.RemoveBlacklistItem(SelectedDocument.Type);
            if (result.IsSuccess)
            {
                SelectedDocument.Blacklist = new List<BlacklistItem>();
                _blacklist = new List<BlacklistItem>();

                OnPropertyChanged(nameof(AddToBlacklistVisible));
                OnPropertyChanged(nameof(ExcludeFromBlacklistVisible));

                var breProvider = new BusinessRulesEngineProvider(_blacklist);
                var data = breProvider.GetBREData(SelectedDocument.Type, SelectedDocument.Failed);
                SelectedDocument.SetBREData(data);
            }
            else
            {
                ShowErrorMessage(result.ErrorMessage);
            }
        }
        #endregion

        #region SaveReport
        private ICommand _saveReportCommand;
        public ICommand SaveReportCommand
        {
            get
            {
                if (_saveReportCommand == null)
                {
                    _saveReportCommand = new RelayCommand(
                        rc => this.CanSaveReport(),
                        rc => this.SaveReport());
                }
                return _saveReportCommand;
            }
        }

        private bool CanSaveReport()
        {
            return Documents != null && Documents.Any();
        }

        private void SaveReport()
        {
            var buckets = new List<ReportItem>();

            foreach (var document in Documents)
            {
                buckets.Add(new ReportItem(document.Type, document.Success, document.Failed));
                if (document.Failed > 0)
                {
                    var errorResult = _provider.GetDocumentErrorBuckets(document.Type, ContentableEntityTypeEnum.Document);
                    if (errorResult.Key.IsSuccess)
                    {
                        buckets.AddRange(errorResult.Value.Select(error => new ReportItem(error.ErrorType, error.Number)));
                    }
                }
            }

            if (buckets.Any())
            {
                var reportProvider = new ReportProvider();
                reportProvider.GenerateDocumentsReport(buckets);
            }
        }
        #endregion

        #endregion

        #region Private methods
        private async void LoadDocumentBuckets()
        {
            ResponseWaiting = true;
            var documentTypes = await Task.Run(() => LoadExtensions());
            var documents = documentTypes.OrderByDescending(it => it.Failed).ThenBy(it => it.Type);
            foreach (var document in documents)
            {
                Documents.Add(document);
            }

            SetStatusBarContent();
            ResponseWaiting = false;
        }

        public override void SetStatusBarContent()
        {
            SetParameterToStatusBar(1, "Extensions", Documents.Count);
            SetParameterToStatusBar(2, "Documents", Documents.Sum(d => d.Success + d.Failed));
            SetParameterToStatusBar(3, "Failed documents", Documents.Sum(d => d.Failed));
        }

        private List<DocumentBucketViewModel> LoadExtensions()
        {
            var documentsResult = _provider.GetDocumentBuckets(_entityType);

            if (!documentsResult.Key.IsSuccess)
            {
                ShowErrorMessage(documentsResult.Key.ErrorMessage);
                return new List<DocumentBucketViewModel>();
            }

            var blacklistResult = _provider.GetBlacklist();
            if (!blacklistResult.Key.IsSuccess)
            {
                ShowErrorMessage(blacklistResult.Key.ErrorMessage);
                return new List<DocumentBucketViewModel>();
            }

            var buckets = documentsResult.Value.Select(s => new DocumentBucketViewModel(s)).ToList();
            _blacklist = blacklistResult.Value;
            var breProvider = new BusinessRulesEngineProvider(_blacklist);

            foreach (var bucket in buckets)
            {
                var data = breProvider.GetBREData(bucket.Type, bucket.Failed);
                bucket.SetBREData(data);
            }

            return buckets;
        }
        #endregion
    }
}
