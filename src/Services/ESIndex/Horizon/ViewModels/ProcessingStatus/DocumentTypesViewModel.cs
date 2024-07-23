using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Horizon.Common;
using Horizon.Common.Interfaces;
using Horizon.DAL;
using Horizon.Models.ProcessingStatus;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.ProcessingStatus
{
    public class DocumentTypesViewModel : PageViewModel
    {
        private readonly IDbProvider _provider;
        private readonly int _processId;
        private readonly string _errorCode;

        public DocumentTypesViewModel(MainViewModel mainViewModel, int processId, string errorCode) : base(mainViewModel)
        {
            Title = "List of Document Errors";
            _provider = new Provider(mainViewModel.Settings.DbConnection);
            _processId = processId;
            _errorCode = errorCode;
            DocumentTypes = new ObservableCollection<DocumentTypeModel>();
            SetDocumentTypes();
        }

        private DocumentTypeModel _selectedDocumentType;
        public DocumentTypeModel SelectedDocumentType
        {
            get { return _selectedDocumentType; }
            set
            {
                if (_selectedDocumentType != value)
                {
                    _selectedDocumentType = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ErrorCode
        {
            get { return $"Error code: {_errorCode}"; }
        }
        public ObservableCollection<DocumentTypeModel> DocumentTypes { get; set; }

        #region Commands

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
            return SelectedDocumentType != null;
        }

        private void OpenDocuments()
        {
            var nextPage = new DocumentsViewModel(MainViewModel, _processId, _errorCode, SelectedDocumentType.Extension, SelectedDocumentType.Count)
            {
                PreviousPage = this
            };
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

        private async void SetDocumentTypes()
        {
            var documentTypes = await Task.Run(() => LoadDocumentTypes(_processId, _errorCode));

            foreach (var documentType in documentTypes)
            {
                DocumentTypes.Add(documentType);
            }
        }

        private List<DocumentTypeModel> LoadDocumentTypes(int processId, string errorCode)
        {
            var result = _provider.GetDocumentTypes(processId, errorCode);
            if (result.Key.IsSuccess)
            {
                return result.Value.Select(s => new DocumentTypeModel(s)).ToList();
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            return new List<DocumentTypeModel>();
        }
    }
}
