using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Horizon.Common;
using Horizon.Common.Interfaces;
using Horizon.Common.Models.Repositories.ProcessingStatus;
using Horizon.DAL;
using Horizon.Models.ProcessingStatus;
using Horizon.Providers;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.ProcessingStatus
{
    public class ErrorCodesViewModel : PageViewModel
    {
        private readonly IDbProvider _provider;
        private readonly int _processId;

        public ErrorCodesViewModel(MainViewModel mainViewModel, int processId) : base(mainViewModel)
        {
            Title = "List of Document Errors";
            _provider = new Provider(mainViewModel.Settings.DbConnection);
            _processId = processId;
            ErrorCodes = new ObservableCollection<ErrorCodeModel>();
            SetErrorCodes();
        }

        private ErrorCodeModel _selectedErrorCode;
        public ErrorCodeModel SelectedErrorCode
        {
            get { return _selectedErrorCode; }
            set
            {
                if (_selectedErrorCode != value)
                {
                    _selectedErrorCode = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public ObservableCollection<ErrorCodeModel> ErrorCodes { get; set; }

        #region Commands

        #region OpenDocumentTypes
        private ICommand _openDocumentTypesCommand;
        public ICommand OpenDocumentTypesCommand
        {
            get
            {
                if (_openDocumentTypesCommand == null)
                {
                    _openDocumentTypesCommand = new RelayCommand(
                        rc => this.CanOpenDocumentTypes(),
                        rc => this.OpenDocumentTypes());
                }
                return _openDocumentTypesCommand;
            }
        }

        private bool CanOpenDocumentTypes()
        {
            return SelectedErrorCode != null;
        }

        private void OpenDocumentTypes()
        {
            var nextPage = new DocumentTypesViewModel(MainViewModel, _processId, SelectedErrorCode.ErrorCode)
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
            return ErrorCodes != null && ErrorCodes.Any();
        }

        private void SaveReport()
        {
            CreateReport();
        }
        #endregion

        #endregion

        private async void SetErrorCodes()
        {
            var errorCodes = await Task.Run(() => LoadErrorCodes(_processId));

            foreach (var errorCode in errorCodes)
            {
                ErrorCodes.Add(errorCode);
            }
        }

        private List<ErrorCodeModel> LoadErrorCodes(int processId)
        {
            var result = _provider.GetErrorCodes(processId);
            if (result.Key.IsSuccess)
            {
                return result.Value.Select(s => new ErrorCodeModel(s)).ToList();
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            return new List<ErrorCodeModel>();
        }

        private async void CreateReport()
        {
            var errors = await Task.Run(() => LoadReport(_processId));
            var reportProvider = new ReportProvider();
            reportProvider.GenerateErrorsReport(errors);
        }

        private List<DocumentErrorInfo> LoadReport(int processId)
        {
            var result = _provider.GetDocumentErrorsReport(processId);
            if (result.Key.IsSuccess)
            {
                return result.Value.ToList();
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            return new List<DocumentErrorInfo>();
        }
    }
}
