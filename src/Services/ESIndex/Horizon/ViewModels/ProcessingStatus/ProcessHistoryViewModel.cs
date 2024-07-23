using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Horizon.Common.Interfaces;
using Horizon.DAL;
using Horizon.ViewModels.Common;

namespace Horizon.ViewModels.ProcessingStatus
{
    public class ProcessingStatusViewModel : PageViewModel
    {
        private IDbProvider _provider;

        public ProcessingStatusViewModel(MainViewModel mainViewModel) : base(mainViewModel)
        {
            Title = "Processing Status";
            _provider = new Provider(mainViewModel.Settings.DbConnection);
            DateStart = DateTime.Now;
            Today = DateTime.Now;
            HistoryItems = new ObservableCollection<ProcessHistoryItemViewModel>();
        }
        
        public ObservableCollection<ProcessHistoryItemViewModel> HistoryItems { get; set; }

        private ProcessHistoryItemViewModel _selectedHistoryItem;
        public ProcessHistoryItemViewModel SelectedHistoryItem
        {
            get { return _selectedHistoryItem; }
            set
            {
                if (_selectedHistoryItem != value)
                {
                    _selectedHistoryItem = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Today { get; set; }

        private DateTime _dateStart;
        public DateTime DateStart
        {
            get { return _dateStart; }
            set
            {
                if (_dateStart != value)
                {
                    _dateStart = value;
                    OnPropertyChanged();

                    GetProcessHistory(_dateStart.Date, _dateStart.Date.AddDays(1).AddMilliseconds(-1));
                }
            }
        }

        private async void GetProcessHistory(DateTime startDate, DateTime finishDate)
        {
            ResponseWaiting = true;
            var historyItems = await Task.Run(() => LoadProcessHistory(startDate, finishDate));
            HistoryItems.Clear();
            foreach (var historyItem in historyItems)
            {
                HistoryItems.Add(historyItem);
            }

            foreach (var historyItem in HistoryItems)
            {
                historyItem.onError += ShowErrorMessage;
            }

            SetStatusBarContent();
            ResponseWaiting = false;
        }

        public override void SetStatusBarContent()
        {
            SetParameterToStatusBar(1, "Successful", HistoryItems.Count(item => item.Failed == 0));
            SetParameterToStatusBar(2, "With Errors", HistoryItems.Count(item => item.Failed > 0));
            SetParameterToStatusBar(3, "Not Completed", HistoryItems.Count(item => !item.FinishDate.HasValue));
        }

        private List<ProcessHistoryItemViewModel> LoadProcessHistory(DateTime startDate, DateTime finishDate)
        {
            var result = _provider.GetProcessHistory(startDate, finishDate);
            if (result.Key.IsSuccess)
            {
                return result.Value.Select(proc => new ProcessHistoryItemViewModel(proc.Id, proc.StartDate, _provider, this)
                {
                    FinishDate = proc.FinishDate,
                    Successful = proc.Successful,
                    Failed = proc.Failed,
                    ContentErrors = proc.ContentErrors
                }).OrderByDescending(proc => proc.StartDate).ToList();
            }

            ShowErrorMessage(result.Key.ErrorMessage);
            return new List<ProcessHistoryItemViewModel>();
        }
    }
}
