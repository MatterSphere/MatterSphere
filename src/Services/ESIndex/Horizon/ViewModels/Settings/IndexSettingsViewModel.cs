using System;
using System.Windows.Input;
using Horizon.Common;
using Horizon.Common.Models.Repositories.IndexProcess;
using Horizon.DAL;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.Settings
{
    public class IndexSettingsViewModel : PageViewModel
    {
        private readonly Provider _provider;
        private DateTime? _previousDocumentDateLimit;
        private DateTime? _lastSavedDocumentDateLimit;

        public IndexSettingsViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            _provider = new Provider(mainViewModel.Settings.DbConnection);
            LoadSettings();
        }

        private bool _processOrderFromLatest;
        public bool ProcessOrderFromLatest
        {
            get { return _processOrderFromLatest; }
            set
            {
                if (_processOrderFromLatest != value)
                {
                    _processOrderFromLatest = value;
                    OnPropertyChanged(nameof(ProcessOrderFromLatest));
                    OnPropertyChanged(nameof(ProcessOrderLabel));
                }
            }
        }

        private long _batchSize;
        public long BatchSize
        {
            get { return _batchSize; }
            set
            {
                if (_batchSize != value)
                {
                    _batchSize = value;
                    OnPropertyChanged(nameof(BatchSize));
                }
            }
        }

        private DateTime? _documentDateLimit;
        public DateTime? DocumentDateLimit
        {
            get { return _documentDateLimit; }
            set
            {
                if (_documentDateLimit != value)
                {
                    _documentDateLimit = value;
                    OnPropertyChanged(nameof(DocumentDateLimit));
                }
            }
        }

        private bool _summaryFieldEnabled;
        public bool SummaryFieldEnabled
        {
            get { return _summaryFieldEnabled; }
            set
            {
                if (_summaryFieldEnabled != value)
                {
                    _summaryFieldEnabled = value;
                    OnPropertyChanged(nameof(SummaryFieldEnabled));
                }
            }
        }

        public string ProcessOrderLabel => $"Index from {(_processOrderFromLatest ? "Newest to Oldest" : "Oldest to Newest")}:";

        #region Commands

        #region Save
        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        rc => this.CanSave(),
                        rc => this.Save());
                }
                return _saveCommand;
            }
        }

        private bool CanSave()
        {
            if (_documentDateLimit.HasValue)
            {
                if (_previousDocumentDateLimit.HasValue)
                {
                    if (_documentDateLimit.Value > _previousDocumentDateLimit.Value)
                    {
                        return false;
                    }
                }
                if (_lastSavedDocumentDateLimit.HasValue)
                {
                    if (_documentDateLimit.Value > _lastSavedDocumentDateLimit)
                    {
                        return false;
                    }
                }
            }
            return BatchSize > 0;
        }

        private void Save()
        {
            var result = _provider.SaveIndexSettings(new IndexSettings(BatchSize, !ProcessOrderFromLatest, DocumentDateLimit, _previousDocumentDateLimit, _summaryFieldEnabled));
            if (result.IsSuccess)
            {
                _lastSavedDocumentDateLimit = _documentDateLimit;
            }
            else
            {
                ShowErrorMessage(result.ErrorMessage);
            }
        }
        #endregion

        #endregion

        #region Private methods

        private void LoadSettings()
        {
            var result = _provider.GetIndexSettings();

            if (result.Key.IsSuccess)
            {
                ProcessOrderFromLatest = !result.Value.ProcessOrderFromOldItems;
                BatchSize = result.Value.BatchSize;
                DocumentDateLimit = result.Value.DocumentDateLimit;
                _lastSavedDocumentDateLimit = result.Value.DocumentDateLimit;
                _previousDocumentDateLimit = result.Value.PreviousDocumentDateLimit;
                _summaryFieldEnabled = result.Value.SummaryFieldEnabled;
            }
            else
            {
                ShowErrorMessage(result.Key.ErrorMessage);
            }
        }

        #endregion
    }
}
