using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Horizon.Common;
using Horizon.TestiFilter;
using Horizon.TestiFilter.Common;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.IFilters
{
    public class PredefinedTestViewModel : PageViewModel
    {
        public PredefinedTestViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Title = "Standard tests";
            LoadDocumentsList();
        }

        private ObservableCollection<PredefinedTestResultItemViewModel> _predefinedTestResultItems;
        public ObservableCollection<PredefinedTestResultItemViewModel> PredefinedTestResultItems
        {
            get { return _predefinedTestResultItems; }
            set
            {
                if (_predefinedTestResultItems != value)
                {
                    _predefinedTestResultItems = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ResultTableVisibility));
                }
            }
        }

        public Visibility ResultTableVisibility
        {
            get
            {
                return PredefinedTestResultItems != null && PredefinedTestResultItems.Any()
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
        }

        #region RunTest
        private ICommand _runTestCommand;
        public ICommand RunTestCommand
        {
            get
            {
                if (_runTestCommand == null)
                {
                    _runTestCommand = new RelayCommand(
                        rc => this.CanRunTest(),
                        rc => this.RunTest());
                }
                return _runTestCommand;
            }
        }

        private bool CanRunTest()
        {
            return true;
        }

        private void RunTest()
        {
            var provider = new PredefinedTestProvider();
            var manager = new TestManager(provider, Callback);
            manager.RunTestsAsync();
        }

        public void Callback(TestCallback callback)
        {
            PredefinedTestResultItemViewModel documentItem = new PredefinedTestResultItemViewModel(callback);

            if (PredefinedTestResultItems == null)
            {
                PredefinedTestResultItems =
                    new ObservableCollection<PredefinedTestResultItemViewModel>(new[] { documentItem });
            }
            else
            {
                var item = PredefinedTestResultItems.FirstOrDefault(it => it.DocumentType == documentItem.DocumentType);
                if (item != null)
                {
                    item.TestResult = documentItem.TestResult;
                    item.ErrorDetails = documentItem.ErrorDetails;
                }
                else
                {
                    PredefinedTestResultItems.Add(documentItem);
                }
            }
        }
        #endregion

        private void LoadDocumentsList()
        {
            var provider = new PredefinedTestProvider();
            var documents = provider.GetTestInfoList().Select(s => new PredefinedTestResultItemViewModel(s.File.Extension));
            PredefinedTestResultItems = new ObservableCollection<PredefinedTestResultItemViewModel>(documents);
        }
    }
}
