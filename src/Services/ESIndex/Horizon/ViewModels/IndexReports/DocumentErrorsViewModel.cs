using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Horizon.Common;
using Horizon.Common.Models.Common;
using Horizon.DAL;
using Horizon.Models.IndexReports;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.IndexReports
{
    public class DocumentErrorsViewModel : PageViewModel
    {
        private readonly Provider _provider;
        private readonly string _extension;
        private readonly string _errorCode;
        private readonly long _itemsNumber;
        private readonly ContentableEntityTypeEnum _entityType;

        public DocumentErrorsViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Title = "List of Documents";
        }

        public DocumentErrorsViewModel(MainViewModel mainViewModel, string extension, string errorCode, long itemsNumber, ContentableEntityTypeEnum entityType) : this(mainViewModel)
        {
            _provider = new Provider(mainViewModel.Settings.DbConnection);
            _extension = extension;
            _errorCode = errorCode;
            _itemsNumber = itemsNumber;
            _entityType = entityType;

            var details = ExceptionConverter.GetExceptionDescription(errorCode);
            Title = $"List of documents with {_extension} extension. The error is {details.Explanation}";

            Pager = new PageControlViewModel(itemsNumber, mainViewModel.Settings.PageSize);
            Pager.onPageChange += PageChange;

            PageChange();
            SetStatusBarContent();
        }

        public override void SetStatusBarContent()
        {
            SetParameterToStatusBar(1, "Page size", Pager.PageSize);
            SetParameterToStatusBar(2, "Failed documents", _itemsNumber);
        }

        private ObservableCollection<DocumentError> _documentErrors;
        public ObservableCollection<DocumentError> DocumentErrors
        {
            get { return _documentErrors; }
            private set
            {
                if (_documentErrors != value)
                {
                    _documentErrors = value;
                    OnPropertyChanged();
                }
            }
        }

        private DocumentError _selectedDocumentError;
        public DocumentError SelectedDocumentError
        {
            get { return _selectedDocumentError; }
            set
            {
                if (_selectedDocumentError != value)
                {
                    _selectedDocumentError = value;
                    OnPropertyChanged();
                }
            }
        }

        public PageControlViewModel Pager { get; private set; }

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

        #region OpenFolder
        private ICommand _openFolderCommand;
        public ICommand OpenFolderCommand
        {
            get
            {
                if (_openFolderCommand == null)
                {
                    _openFolderCommand = new RelayCommand(
                        rc => this.CanOpenFolder(),
                        rc => this.OpenFolder());
                }
                return _openFolderCommand;
            }
        }

        private bool CanOpenFolder()
        {
            return SelectedDocumentError != null && !string.IsNullOrEmpty(SelectedDocumentError.Path);
        }

        private void OpenFolder()
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo(SelectedDocumentError.Path);
            Process.Start("explorer.exe", "/select, \"" + startInfo.FileName + "\"");
        }
        #endregion

        private void PageChange()
        {
            DocumentErrors = new ObservableCollection<DocumentError>(LoadErrors(_extension, _errorCode, Pager.CurrentPage, Pager.PageSize));
        }

        private List<DocumentError> LoadErrors(string extension, string errorCode, int page, int pageSize)
        {
            var result = _provider.GetDocumentErrors(extension, errorCode, page, pageSize, _entityType);
            if (result.Key.IsSuccess)
            {
                int number = Pager.GetStartNumber();
                return result.Value.Select(s => new DocumentError(s, number++)).ToList();
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            return new List<DocumentError>();
        }
    }
}
