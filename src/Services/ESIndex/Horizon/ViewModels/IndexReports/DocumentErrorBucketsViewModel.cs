using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Horizon.Common;
using Horizon.Common.Models.Common;
using Horizon.DAL;
using Horizon.Models.IndexReports;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.IndexReports
{
    public class DocumentErrorBucketsViewModel : PageViewModel
    {
        private string _extension = null;
        private ContentableEntityTypeEnum _entityType;

        public DocumentErrorBucketsViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Title = "List of Document Errors";
        }
        public DocumentErrorBucketsViewModel(MainViewModel mainViewModel, string extension, ContentableEntityTypeEnum entityType) : this(mainViewModel)
        {
            _extension = extension;
            _entityType = entityType;
            Title = $"List of Document Errors for {_extension.ToUpper()}";
            DocumentErrorBuckets = new ObservableCollection<DocumentErrorBucket>(LoadErrors(_extension));
            SetStatusBarContent();
        }

        public override void SetStatusBarContent()
        {
            SetParameterToStatusBar(1, "Document type", _extension);
            SetParameterToStatusBar(2, "Error types", DocumentErrorBuckets.Count);
            SetParameterToStatusBar(3, "Failed documents", DocumentErrorBuckets.Sum(d => d.Number));
        }

        public ObservableCollection<DocumentErrorBucket> DocumentErrorBuckets { get; set; }

        private DocumentErrorBucket _selectedDocumentErrorBucket;
        public DocumentErrorBucket SelectedDocumentErrorBucket
        {
            get { return _selectedDocumentErrorBucket; }
            set
            {
                if (_selectedDocumentErrorBucket != value)
                {
                    _selectedDocumentErrorBucket = value;
                    OnPropertyChanged();
                }
            }
        }

        #region Commands

        #region OpenDocumentList
        private ICommand _openDocumentListCommand;
        public ICommand OpenDocumentListCommand
        {
            get
            {
                if (_openDocumentListCommand == null)
                {
                    _openDocumentListCommand = new RelayCommand(
                        rc => this.CanOpenDocumentList(),
                        rc => this.OpenDocumentList());
                }
                return _openDocumentListCommand;
            }
        }

        private bool CanOpenDocumentList()
        {
            return SelectedDocumentErrorBucket != null && SelectedDocumentErrorBucket.Number > 0;
        }

        private void OpenDocumentList()
        {
            var nextPage = new DocumentErrorsViewModel(this.MainViewModel, _extension, SelectedDocumentErrorBucket.ErrorType, SelectedDocumentErrorBucket.Number, _entityType);
            nextPage.PreviousPage = this;
            MainViewModel.CurrentPage = nextPage;
        }
        #endregion

        #region GoBack
        private ICommand _goBackCommand;
        public ICommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new RelayCommand(
                        rc => this.CanGoBack(),
                        rc => this.GoBack());
                }
                return _goBackCommand;
            }
        }

        private bool CanGoBack()
        {
            return PreviousPage != null;
        }

        private void GoBack()
        {
            MainViewModel.CurrentPage = PreviousPage;
        }
        #endregion

        #endregion

        private List<DocumentErrorBucket> LoadErrors(string extension)
        {
            var provider = new Provider(MainViewModel.Settings.DbConnection);
            var result = provider.GetDocumentErrorBuckets(extension, _entityType);
            if (result.Key.IsSuccess)
            {
                return result.Value.Select(s => new DocumentErrorBucket(s)).ToList();
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            return new List<DocumentErrorBucket>();
        }
    }
}
