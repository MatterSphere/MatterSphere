using System.Linq;
using System.Threading.Tasks;
using Horizon.Common.Models.Common;
using Horizon.DAL;
using Horizon.Models.StartPage;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.StartPage
{
    public class StartPageViewModel : PageViewModel
    {
        private Provider _provider;
        private int _interval = 5000;
        private string _queue;

        public StartPageViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            _provider = new Provider(MainViewModel.Settings.DbConnection);
            _interval = mainViewModel.Settings.Interval > 0
                ? mainViewModel.Settings.Interval
                : _interval;
            _queue = MainViewModel.Settings.Queue;

            Task.Factory.StartNew(LoadDocumentDetails);
            Task.Factory.StartNew(LoadPrecedentDetails);
            Task.Factory.StartNew(LoadBlacklistDetails);
            Task.Factory.StartNew(LoadIndexingDetails);
        }

        #region Properties

        private StartPageItemViewModel _documentDetails;
        public StartPageItemViewModel DocumentDetails
        {
            get { return _documentDetails; }
            set
            {
                if (_documentDetails != value)
                {
                    _documentDetails = value;
                    OnPropertyChanged();
                }
            }
        }

        private StartPageItemViewModel _precedentDetails;
        public StartPageItemViewModel PrecedentDetails
        {
            get { return _precedentDetails; }
            set
            {
                if (_precedentDetails != value)
                {
                    _precedentDetails = value;
                    OnPropertyChanged();
                }
            }
        }

        private StartPageItemViewModel _blacklistDetails;
        public StartPageItemViewModel BlacklistDetails
        {
            get { return _blacklistDetails; }
            set
            {
                if (_blacklistDetails != value)
                {
                    _blacklistDetails = value;
                    OnPropertyChanged();
                }
            }
        }

        private StartPageItemViewModel _indexingDetails;
        public StartPageItemViewModel IndexingDetails
        {
            get { return _indexingDetails; }
            set
            {
                if (_indexingDetails != value)
                {
                    _indexingDetails = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Private methods

        private void LoadDocumentDetails()
        {
            var result = _provider.GetDocumentBuckets(ContentableEntityTypeEnum.Document);
            if (result.Key.IsSuccess)
            {
                DocumentDetails = new DocumentDetailsViewModel(
                    startPageInfoType: StartPageInfoTypeEnum.DocumentsRead,
                    total: result.Value.Sum(extension => extension.Success + extension.Failed),
                    subNumber: result.Value.Sum(extension => extension.Failed),
                    subNumberLabel: "failed",
                    main: MainViewModel);
                return;
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            DocumentDetails = new DocumentDetailsViewModel(MainViewModel);
        }

        private void LoadPrecedentDetails()
        {
            var result = _provider.GetDocumentBuckets(ContentableEntityTypeEnum.Precedent);
            if (result.Key.IsSuccess)
            {
                PrecedentDetails = new DocumentDetailsViewModel(
                    startPageInfoType: StartPageInfoTypeEnum.PrecedentsRead,
                    total: result.Value.Sum(extension => extension.Success + extension.Failed),
                    subNumber: result.Value.Sum(extension => extension.Failed),
                    subNumberLabel: "failed",
                    main: MainViewModel);
                return;
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            PrecedentDetails = new DocumentDetailsViewModel(MainViewModel);
        }

        private void LoadBlacklistDetails()
        {
            var result = _provider.GetBlacklist();
            if (result.Key.IsSuccess)
            {
                BlacklistDetails = new DocumentDetailsViewModel(
                    startPageInfoType: StartPageInfoTypeEnum.Blacklist,
                    total: result.Value.GroupBy(item => item.Extension).Count(),
                    subNumber: null,
                    subNumberLabel: "extension(s)",
                    main: MainViewModel);
                return;
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            BlacklistDetails = new DocumentDetailsViewModel(MainViewModel);
        }

        private void LoadIndexingDetails()
        {
            var result = _provider.GetActualProcessDetail(_interval+3);
            if (!result.Key.IsSuccess)
            {
                ShowErrorMessage(result.Key.ErrorMessage);
                IndexingDetails = new DocumentDetailsViewModel(MainViewModel);
                return;
            }

            var queueLengthResult = _provider.GetQueueLength(_queue);
            if (!queueLengthResult.Key.IsSuccess)
            {
                ShowErrorMessage(queueLengthResult.Key.ErrorMessage);
                IndexingDetails = new DocumentDetailsViewModel(MainViewModel);
                return;
            }

            IndexingDetails = new DocumentDetailsViewModel(
                startPageInfoType: StartPageInfoTypeEnum.IndexingStatus,
                total: result.Value.Sum(extension => extension.Messages),
                subNumber: queueLengthResult.Value,
                subNumberLabel: "left",
                main: MainViewModel);
        }

        #endregion
    }
}
