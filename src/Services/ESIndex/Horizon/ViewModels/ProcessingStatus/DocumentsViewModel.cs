using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Horizon.Common;
using Horizon.Common.Interfaces;
using Horizon.DAL;
using Horizon.Models.ProcessingStatus;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.ProcessingStatus
{
    public class DocumentsViewModel : PageViewModel
    {
        private readonly IDbProvider _provider;
        private readonly int _processId;
        private readonly string _errorCode;
        private readonly string _extension;

        public DocumentsViewModel(MainViewModel mainViewModel, int processId, string errorCode, string extension, int count) : base(mainViewModel)
        {
            Title = "List of Document Errors";
            _provider = new Provider(mainViewModel.Settings.DbConnection);
            _processId = processId;
            _errorCode = errorCode;
            _extension = extension;

            Pager = new PageControlViewModel(count, mainViewModel.Settings.PageSize);
            Pager.onPageChange += PageChange;

            PageChange();
        }

        public PageControlViewModel Pager { get; private set; }

        private ObservableCollection<DocumentModel> _documents;
        public ObservableCollection<DocumentModel> Documents
        {
            get { return _documents; }
            private set
            {
                if (_documents != value)
                {
                    _documents = value;
                    OnPropertyChanged();
                }
            }
        }

        private DocumentModel _selectedDocument;
        public DocumentModel SelectedDocument
        {
            get { return _selectedDocument; }
            set
            {
                if (_selectedDocument != value)
                {
                    _selectedDocument = value;
                    OnPropertyChanged();
                }
            }
        }

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
            return SelectedDocument != null && !string.IsNullOrEmpty(SelectedDocument.Path);
        }

        private void OpenFolder()
        {
            var startInfo = new System.Diagnostics.ProcessStartInfo(SelectedDocument.Path);
            Process.Start("explorer.exe", "/select, \"" + startInfo.FileName + "\"");
        }

        #endregion

        private void PageChange()
        {
            Documents = new ObservableCollection<DocumentModel>(LoadDocuments(_processId, _errorCode, _extension, Pager.CurrentPage, Pager.PageSize));
        }

        private List<DocumentModel> LoadDocuments(int processId, string errorCode, string extension, int page, int pageSize)
        {
            var result = _provider.GetDocuments(processId, errorCode, extension, page, pageSize);
            if (result.Key.IsSuccess)
            {
                int number = Pager.GetStartNumber();
                return result.Value.Select(s => new DocumentModel(s, number++)).ToList();
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            return new List<DocumentModel>();
        }
    }
}
