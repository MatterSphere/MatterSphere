using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Horizon.Common;
using Horizon.DAL;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.IndexProcess
{
    public class ReindexDocumentsViewModel : PageViewModel
    {
        private Provider _provider;

        public ReindexDocumentsViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Title = "Reindexing";
            _provider = new Provider(mainViewModel.Settings.DbConnection);
            Extensions = new ObservableCollection<string>();
            Extensions = new ObservableCollection<string>(LoadExtensions());
            ReindexNotStarted = true;
        }

        public ObservableCollection<string> Extensions { get; set; }

        private string _addedExtension;
        public string AddedExtension
        {
            get { return _addedExtension; }
            set
            {
                if (_addedExtension != value)
                {
                    _addedExtension = value;
                    OnPropertyChanged();
                }
            }
        }
        
        private bool _reindexNotStarted;
        public bool ReindexNotStarted
        {
            get { return _reindexNotStarted; }
            set
            {
                if (_reindexNotStarted != value)
                {
                    _reindexNotStarted = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _reindexStarted;
        public bool ReindexStarted
        {
            get { return _reindexStarted; }
            set
            {
                if (_reindexStarted != value)
                {
                    _reindexStarted = value;
                    OnPropertyChanged();
                }
            }
        }

        #region Commands

        #region AddExtension
        private ICommand _addExtensionCommand;
        public ICommand AddExtensionCommand
        {
            get
            {
                if (_addExtensionCommand == null)
                {
                    _addExtensionCommand = new RelayCommand(
                        rc => this.CanAddExtension(),
                        rc => this.AddExtension());
                }
                return _addExtensionCommand;
            }
        }

        private bool CanAddExtension()
        {
            return !string.IsNullOrWhiteSpace(AddedExtension);
        }

        private void AddExtension()
        {
            var result = _provider.AddExtensionForReindexing(AddedExtension);
            if (result.IsSuccess)
            {
                Extensions.Add(AddedExtension);
                var extensions = Extensions.OrderBy(ext => ext).ToList();
                Extensions.Clear();
                foreach (var extension in extensions)
                {
                    Extensions.Add(extension);
                }

                AddedExtension = null;
            }
            else
            {
                ShowErrorMessage(result.ErrorMessage);
            }
        }
        #endregion

        #region Reindex All Failed Documents
        private ICommand _reindexAllFailedDocumentsCommand;
        public ICommand ReindexAllFailedDocumentsCommand
        {
            get
            {
                if (_reindexAllFailedDocumentsCommand == null)
                {
                    _reindexAllFailedDocumentsCommand = new RelayCommand(
                        rc => this.CanReindexAllFailedDocuments(),
                        rc => this.ReindexAllFailedDocuments());
                }
                return _reindexAllFailedDocumentsCommand;
            }
        }

        private bool CanReindexAllFailedDocuments()
        {
            return ReindexNotStarted;
        }

        private void ReindexAllFailedDocuments()
        {
            var result = _provider.ReindexAllFailedDocuments();

            if (result.IsSuccess)
            {
                ReindexNotStarted = false;
            }
            else
            {
                ReindexStarted = false;
                ShowErrorMessage(result.ErrorMessage);
            }
        }
        #endregion

        #endregion

        #region Private methods

        private List<string> LoadExtensions()
        {
            var result = _provider.GetExtensionsForReindexing();
            if (result.Key.IsSuccess)
            {
                return result.Value.OrderBy(ext => ext).ToList();
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            return new List<string>();
        }

        #endregion
    }
}
